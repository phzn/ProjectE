using System;
using System.Buffers;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace System.Net.WebSockets
{
	// Token: 0x02000815 RID: 2069
	internal sealed class ManagedWebSocket : WebSocket
	{
		// Token: 0x06004250 RID: 16976 RVA: 0x000E4E78 File Offset: 0x000E3078
		public static ManagedWebSocket CreateFromConnectedStream(Stream stream, bool isServer, string subprotocol, TimeSpan keepAliveInterval)
		{
			return new ManagedWebSocket(stream, isServer, subprotocol, keepAliveInterval);
		}

		// Token: 0x17000EE0 RID: 3808
		// (get) Token: 0x06004251 RID: 16977 RVA: 0x000E4E83 File Offset: 0x000E3083
		private object StateUpdateLock
		{
			get
			{
				return this._abortSource;
			}
		}

		// Token: 0x17000EE1 RID: 3809
		// (get) Token: 0x06004252 RID: 16978 RVA: 0x000E4E8B File Offset: 0x000E308B
		private object ReceiveAsyncLock
		{
			get
			{
				return this._utf8TextState;
			}
		}

		// Token: 0x06004253 RID: 16979 RVA: 0x000E4E94 File Offset: 0x000E3094
		private ManagedWebSocket(Stream stream, bool isServer, string subprotocol, TimeSpan keepAliveInterval)
		{
			this._stream = stream;
			this._isServer = isServer;
			this._subprotocol = subprotocol;
			this._receiveBuffer = new byte[125];
			this._abortSource.Token.Register(delegate(object s)
			{
				ManagedWebSocket managedWebSocket = (ManagedWebSocket)s;
				object stateUpdateLock = managedWebSocket.StateUpdateLock;
				lock (stateUpdateLock)
				{
					WebSocketState state = managedWebSocket._state;
					if (state != WebSocketState.Closed && state != WebSocketState.Aborted)
					{
						managedWebSocket._state = ((state != WebSocketState.None && state != WebSocketState.Connecting) ? WebSocketState.Aborted : WebSocketState.Closed);
					}
				}
			}, this);
			if (keepAliveInterval > TimeSpan.Zero)
			{
				this._keepAliveTimer = new Timer(delegate(object s)
				{
					((ManagedWebSocket)s).SendKeepAliveFrameAsync();
				}, this, keepAliveInterval, keepAliveInterval);
			}
		}

		// Token: 0x06004254 RID: 16980 RVA: 0x000E4F94 File Offset: 0x000E3194
		public override void Dispose()
		{
			object stateUpdateLock = this.StateUpdateLock;
			lock (stateUpdateLock)
			{
				this.DisposeCore();
			}
		}

		// Token: 0x06004255 RID: 16981 RVA: 0x000E4FD4 File Offset: 0x000E31D4
		private void DisposeCore()
		{
			if (!this._disposed)
			{
				this._disposed = true;
				Timer keepAliveTimer = this._keepAliveTimer;
				if (keepAliveTimer != null)
				{
					keepAliveTimer.Dispose();
				}
				Stream stream = this._stream;
				if (stream != null)
				{
					stream.Dispose();
				}
				if (this._state < WebSocketState.Aborted)
				{
					this._state = WebSocketState.Closed;
				}
			}
		}

		// Token: 0x17000EE2 RID: 3810
		// (get) Token: 0x06004256 RID: 16982 RVA: 0x000E5022 File Offset: 0x000E3222
		public override WebSocketCloseStatus? CloseStatus
		{
			get
			{
				return this._closeStatus;
			}
		}

		// Token: 0x17000EE3 RID: 3811
		// (get) Token: 0x06004257 RID: 16983 RVA: 0x000E502A File Offset: 0x000E322A
		public override string CloseStatusDescription
		{
			get
			{
				return this._closeStatusDescription;
			}
		}

		// Token: 0x17000EE4 RID: 3812
		// (get) Token: 0x06004258 RID: 16984 RVA: 0x000E5032 File Offset: 0x000E3232
		public override WebSocketState State
		{
			get
			{
				return this._state;
			}
		}

		// Token: 0x17000EE5 RID: 3813
		// (get) Token: 0x06004259 RID: 16985 RVA: 0x000E503A File Offset: 0x000E323A
		public override string SubProtocol
		{
			get
			{
				return this._subprotocol;
			}
		}

		// Token: 0x0600425A RID: 16986 RVA: 0x000E5044 File Offset: 0x000E3244
		public override Task SendAsync(ArraySegment<byte> buffer, WebSocketMessageType messageType, bool endOfMessage, CancellationToken cancellationToken)
		{
			if (messageType != WebSocketMessageType.Text && messageType != WebSocketMessageType.Binary)
			{
				throw new ArgumentException(SR.Format("The message type '{0}' is not allowed for the '{1}' operation. Valid message types are: '{2}, {3}'. To close the WebSocket, use the '{4}' operation instead. ", new object[] { "Close", "SendAsync", "Binary", "Text", "CloseOutputAsync" }), "messageType");
			}
			WebSocketValidate.ValidateArraySegment(buffer, "buffer");
			return this.SendPrivateAsync(buffer, messageType, endOfMessage, cancellationToken).AsTask();
		}

		// Token: 0x0600425B RID: 16987 RVA: 0x000E50C0 File Offset: 0x000E32C0
		private ValueTask SendPrivateAsync(ReadOnlyMemory<byte> buffer, WebSocketMessageType messageType, bool endOfMessage, CancellationToken cancellationToken)
		{
			if (messageType != WebSocketMessageType.Text && messageType != WebSocketMessageType.Binary)
			{
				throw new ArgumentException(SR.Format("The message type '{0}' is not allowed for the '{1}' operation. Valid message types are: '{2}, {3}'. To close the WebSocket, use the '{4}' operation instead. ", new object[] { "Close", "SendAsync", "Binary", "Text", "CloseOutputAsync" }), "messageType");
			}
			try
			{
				WebSocketValidate.ThrowIfInvalidState(this._state, this._disposed, ManagedWebSocket.s_validSendStates);
			}
			catch (Exception ex)
			{
				return new ValueTask(Task.FromException(ex));
			}
			ManagedWebSocket.MessageOpcode messageOpcode = (this._lastSendWasFragment ? ManagedWebSocket.MessageOpcode.Continuation : ((messageType == WebSocketMessageType.Binary) ? ManagedWebSocket.MessageOpcode.Binary : ManagedWebSocket.MessageOpcode.Text));
			ValueTask valueTask = this.SendFrameAsync(messageOpcode, endOfMessage, buffer, cancellationToken);
			this._lastSendWasFragment = !endOfMessage;
			return valueTask;
		}

		// Token: 0x0600425C RID: 16988 RVA: 0x000E5178 File Offset: 0x000E3378
		public override Task<WebSocketReceiveResult> ReceiveAsync(ArraySegment<byte> buffer, CancellationToken cancellationToken)
		{
			WebSocketValidate.ValidateArraySegment(buffer, "buffer");
			Task<WebSocketReceiveResult> task2;
			try
			{
				WebSocketValidate.ThrowIfInvalidState(this._state, this._disposed, ManagedWebSocket.s_validReceiveStates);
				object receiveAsyncLock = this.ReceiveAsyncLock;
				lock (receiveAsyncLock)
				{
					this.ThrowIfOperationInProgress(this._lastReceiveAsync.IsCompleted, "ReceiveAsync");
					Task<WebSocketReceiveResult> task = this.ReceiveAsyncPrivate<ManagedWebSocket.WebSocketReceiveResultGetter, WebSocketReceiveResult>(buffer, cancellationToken, default(ManagedWebSocket.WebSocketReceiveResultGetter)).AsTask();
					this._lastReceiveAsync = task;
					task2 = task;
				}
			}
			catch (Exception ex)
			{
				task2 = Task.FromException<WebSocketReceiveResult>(ex);
			}
			return task2;
		}

		// Token: 0x0600425D RID: 16989 RVA: 0x000E522C File Offset: 0x000E342C
		public override Task CloseAsync(WebSocketCloseStatus closeStatus, string statusDescription, CancellationToken cancellationToken)
		{
			WebSocketValidate.ValidateCloseStatus(closeStatus, statusDescription);
			try
			{
				WebSocketValidate.ThrowIfInvalidState(this._state, this._disposed, ManagedWebSocket.s_validCloseStates);
			}
			catch (Exception ex)
			{
				return Task.FromException(ex);
			}
			return this.CloseAsyncPrivate(closeStatus, statusDescription, cancellationToken);
		}

		// Token: 0x0600425E RID: 16990 RVA: 0x000E527C File Offset: 0x000E347C
		public override Task CloseOutputAsync(WebSocketCloseStatus closeStatus, string statusDescription, CancellationToken cancellationToken)
		{
			WebSocketValidate.ValidateCloseStatus(closeStatus, statusDescription);
			try
			{
				WebSocketValidate.ThrowIfInvalidState(this._state, this._disposed, ManagedWebSocket.s_validCloseOutputStates);
			}
			catch (Exception ex)
			{
				return Task.FromException(ex);
			}
			return this.SendCloseFrameAsync(closeStatus, statusDescription, cancellationToken);
		}

		// Token: 0x0600425F RID: 16991 RVA: 0x000E52CC File Offset: 0x000E34CC
		public override void Abort()
		{
			this._abortSource.Cancel();
			this.Dispose();
		}

		// Token: 0x06004260 RID: 16992 RVA: 0x000E52DF File Offset: 0x000E34DF
		private ValueTask SendFrameAsync(ManagedWebSocket.MessageOpcode opcode, bool endOfMessage, ReadOnlyMemory<byte> payloadBuffer, CancellationToken cancellationToken)
		{
			if (!cancellationToken.CanBeCanceled && this._sendFrameAsyncLock.Wait(0))
			{
				return this.SendFrameLockAcquiredNonCancelableAsync(opcode, endOfMessage, payloadBuffer);
			}
			return new ValueTask(this.SendFrameFallbackAsync(opcode, endOfMessage, payloadBuffer, cancellationToken));
		}

		// Token: 0x06004261 RID: 16993 RVA: 0x000E5314 File Offset: 0x000E3514
		private ValueTask SendFrameLockAcquiredNonCancelableAsync(ManagedWebSocket.MessageOpcode opcode, bool endOfMessage, ReadOnlyMemory<byte> payloadBuffer)
		{
			ValueTask valueTask = default(ValueTask);
			bool flag = true;
			try
			{
				int num = this.WriteFrameToSendBuffer(opcode, endOfMessage, payloadBuffer.Span);
				valueTask = this._stream.WriteAsync(new ReadOnlyMemory<byte>(this._sendBuffer, 0, num), default(CancellationToken));
				if (valueTask.IsCompleted)
				{
					return valueTask;
				}
				flag = false;
			}
			catch (Exception ex)
			{
				return new ValueTask(Task.FromException((ex is OperationCanceledException) ? ex : ((this._state == WebSocketState.Aborted) ? ManagedWebSocket.CreateOperationCanceledException(ex, default(CancellationToken)) : new WebSocketException(WebSocketError.ConnectionClosedPrematurely, ex))));
			}
			finally
			{
				if (flag)
				{
					this.ReleaseSendBuffer();
					this._sendFrameAsyncLock.Release();
				}
			}
			return new ValueTask(this.WaitForWriteTaskAsync(valueTask));
		}

		// Token: 0x06004262 RID: 16994 RVA: 0x000E53F0 File Offset: 0x000E35F0
		private async Task WaitForWriteTaskAsync(ValueTask writeTask)
		{
			try
			{
				await writeTask.ConfigureAwait(false);
			}
			catch (Exception ex) when (!(ex is OperationCanceledException))
			{
				throw (this._state == WebSocketState.Aborted) ? ManagedWebSocket.CreateOperationCanceledException(ex, default(CancellationToken)) : new WebSocketException(WebSocketError.ConnectionClosedPrematurely, ex);
			}
			finally
			{
				this.ReleaseSendBuffer();
				this._sendFrameAsyncLock.Release();
			}
		}

		// Token: 0x06004263 RID: 16995 RVA: 0x000E543C File Offset: 0x000E363C
		private async Task SendFrameFallbackAsync(ManagedWebSocket.MessageOpcode opcode, bool endOfMessage, ReadOnlyMemory<byte> payloadBuffer, CancellationToken cancellationToken)
		{
			await this._sendFrameAsyncLock.WaitAsync().ConfigureAwait(false);
			try
			{
				int num = this.WriteFrameToSendBuffer(opcode, endOfMessage, payloadBuffer.Span);
				using (cancellationToken.Register(delegate(object s)
				{
					((ManagedWebSocket)s).Abort();
				}, this))
				{
					await this._stream.WriteAsync(new ReadOnlyMemory<byte>(this._sendBuffer, 0, num), cancellationToken).ConfigureAwait(false);
				}
				CancellationTokenRegistration cancellationTokenRegistration = default(CancellationTokenRegistration);
			}
			catch (Exception ex) when (!(ex is OperationCanceledException))
			{
				throw (this._state == WebSocketState.Aborted) ? ManagedWebSocket.CreateOperationCanceledException(ex, cancellationToken) : new WebSocketException(WebSocketError.ConnectionClosedPrematurely, ex);
			}
			finally
			{
				this.ReleaseSendBuffer();
				this._sendFrameAsyncLock.Release();
			}
		}

		// Token: 0x06004264 RID: 16996 RVA: 0x000E54A0 File Offset: 0x000E36A0
		private int WriteFrameToSendBuffer(ManagedWebSocket.MessageOpcode opcode, bool endOfMessage, ReadOnlySpan<byte> payloadBuffer)
		{
			this.AllocateSendBuffer(payloadBuffer.Length + 14);
			int? num = null;
			int num2;
			if (this._isServer)
			{
				num2 = ManagedWebSocket.WriteHeader(opcode, this._sendBuffer, payloadBuffer, endOfMessage, false);
			}
			else
			{
				num = new int?(ManagedWebSocket.WriteHeader(opcode, this._sendBuffer, payloadBuffer, endOfMessage, true));
				num2 = num.GetValueOrDefault() + 4;
			}
			if (payloadBuffer.Length > 0)
			{
				payloadBuffer.CopyTo(new Span<byte>(this._sendBuffer, num2, payloadBuffer.Length));
				if (num != null)
				{
					ManagedWebSocket.ApplyMask(new Span<byte>(this._sendBuffer, num2, payloadBuffer.Length), this._sendBuffer, num.Value, 0);
				}
			}
			return num2 + payloadBuffer.Length;
		}

		// Token: 0x06004265 RID: 16997 RVA: 0x000E555C File Offset: 0x000E375C
		private void SendKeepAliveFrameAsync()
		{
			if (this._sendFrameAsyncLock.Wait(0))
			{
				ValueTask valueTask = this.SendFrameLockAcquiredNonCancelableAsync(ManagedWebSocket.MessageOpcode.Ping, true, Memory<byte>.Empty);
				if (valueTask.IsCompletedSuccessfully)
				{
					valueTask.GetAwaiter().GetResult();
					return;
				}
				valueTask.AsTask().ContinueWith(delegate(Task p)
				{
					AggregateException exception = p.Exception;
				}, CancellationToken.None, TaskContinuationOptions.NotOnRanToCompletion | TaskContinuationOptions.NotOnCanceled | TaskContinuationOptions.ExecuteSynchronously, TaskScheduler.Default);
			}
		}

		// Token: 0x06004266 RID: 16998 RVA: 0x000E55E0 File Offset: 0x000E37E0
		private static int WriteHeader(ManagedWebSocket.MessageOpcode opcode, byte[] sendBuffer, ReadOnlySpan<byte> payload, bool endOfMessage, bool useMask)
		{
			sendBuffer[0] = (byte)opcode;
			if (endOfMessage)
			{
				int num = 0;
				sendBuffer[num] |= 128;
			}
			int num2;
			if (payload.Length <= 125)
			{
				sendBuffer[1] = (byte)payload.Length;
				num2 = 2;
			}
			else if (payload.Length <= 65535)
			{
				sendBuffer[1] = 126;
				sendBuffer[2] = (byte)(payload.Length / 256);
				sendBuffer[3] = (byte)payload.Length;
				num2 = 4;
			}
			else
			{
				sendBuffer[1] = 127;
				int num3 = payload.Length;
				for (int i = 9; i >= 2; i--)
				{
					sendBuffer[i] = (byte)num3;
					num3 /= 256;
				}
				num2 = 10;
			}
			if (useMask)
			{
				int num4 = 1;
				sendBuffer[num4] |= 128;
				ManagedWebSocket.WriteRandomMask(sendBuffer, num2);
			}
			return num2;
		}

		// Token: 0x06004267 RID: 16999 RVA: 0x000E5699 File Offset: 0x000E3899
		private static void WriteRandomMask(byte[] buffer, int offset)
		{
			ManagedWebSocket.s_random.GetBytes(buffer, offset, 4);
		}

		// Token: 0x06004268 RID: 17000 RVA: 0x000E56A8 File Offset: 0x000E38A8
		private ValueTask<TWebSocketReceiveResult> ReceiveAsyncPrivate<TWebSocketReceiveResultGetter, TWebSocketReceiveResult>(Memory<byte> payloadBuffer, CancellationToken cancellationToken, TWebSocketReceiveResultGetter resultGetter = default(TWebSocketReceiveResultGetter)) where TWebSocketReceiveResultGetter : struct, ManagedWebSocket.IWebSocketReceiveResultGetter<TWebSocketReceiveResult>
		{
			ManagedWebSocket.<ReceiveAsyncPrivate>d__61<TWebSocketReceiveResultGetter, TWebSocketReceiveResult> <ReceiveAsyncPrivate>d__;
			<ReceiveAsyncPrivate>d__.<>4__this = this;
			<ReceiveAsyncPrivate>d__.payloadBuffer = payloadBuffer;
			<ReceiveAsyncPrivate>d__.cancellationToken = cancellationToken;
			<ReceiveAsyncPrivate>d__.resultGetter = resultGetter;
			<ReceiveAsyncPrivate>d__.<>t__builder = AsyncValueTaskMethodBuilder<TWebSocketReceiveResult>.Create();
			<ReceiveAsyncPrivate>d__.<>1__state = -1;
			<ReceiveAsyncPrivate>d__.<>t__builder.Start<ManagedWebSocket.<ReceiveAsyncPrivate>d__61<TWebSocketReceiveResultGetter, TWebSocketReceiveResult>>(ref <ReceiveAsyncPrivate>d__);
			return <ReceiveAsyncPrivate>d__.<>t__builder.Task;
		}

		// Token: 0x06004269 RID: 17001 RVA: 0x000E5704 File Offset: 0x000E3904
		private unsafe async Task HandleReceivedCloseAsync(ManagedWebSocket.MessageHeader header, CancellationToken cancellationToken)
		{
			object stateUpdateLock = this.StateUpdateLock;
			lock (stateUpdateLock)
			{
				this._receivedCloseFrame = true;
				if (this._state < WebSocketState.CloseReceived)
				{
					this._state = WebSocketState.CloseReceived;
				}
			}
			WebSocketCloseStatus closeStatus = WebSocketCloseStatus.NormalClosure;
			string closeStatusDescription = string.Empty;
			if (header.PayloadLength == 1L)
			{
				await this.CloseWithReceiveErrorAndThrowAsync(WebSocketCloseStatus.ProtocolError, WebSocketError.Faulted, null).ConfigureAwait(false);
			}
			else if (header.PayloadLength >= 2L)
			{
				if ((long)this._receiveBufferCount < header.PayloadLength)
				{
					await this.EnsureBufferContainsAsync((int)header.PayloadLength, cancellationToken, true).ConfigureAwait(false);
				}
				if (this._isServer)
				{
					ManagedWebSocket.ApplyMask(this._receiveBuffer.Span.Slice(this._receiveBufferOffset, (int)header.PayloadLength), header.Mask, 0);
				}
				closeStatus = (WebSocketCloseStatus)(((int)(*this._receiveBuffer.Span[this._receiveBufferOffset]) << 8) | (int)(*this._receiveBuffer.Span[this._receiveBufferOffset + 1]));
				if (!ManagedWebSocket.IsValidCloseStatus(closeStatus))
				{
					await this.CloseWithReceiveErrorAndThrowAsync(WebSocketCloseStatus.ProtocolError, WebSocketError.Faulted, null).ConfigureAwait(false);
				}
				if (header.PayloadLength > 2L)
				{
					int num = 0;
					try
					{
						closeStatusDescription = ManagedWebSocket.s_textEncoding.GetString(this._receiveBuffer.Span.Slice(this._receiveBufferOffset + 2, (int)header.PayloadLength - 2));
					}
					catch (DecoderFallbackException stateUpdateLock)
					{
						num = 1;
					}
					if (num == 1)
					{
						await this.CloseWithReceiveErrorAndThrowAsync(WebSocketCloseStatus.ProtocolError, WebSocketError.Faulted, (DecoderFallbackException)stateUpdateLock).ConfigureAwait(false);
					}
				}
				this.ConsumeFromBuffer((int)header.PayloadLength);
			}
			this._closeStatus = new WebSocketCloseStatus?(closeStatus);
			this._closeStatusDescription = closeStatusDescription;
			if (!this._isServer && this._sentCloseFrame)
			{
				await this.WaitForServerToCloseConnectionAsync(cancellationToken).ConfigureAwait(false);
			}
		}

		// Token: 0x0600426A RID: 17002 RVA: 0x000E5758 File Offset: 0x000E3958
		private async Task WaitForServerToCloseConnectionAsync(CancellationToken cancellationToken)
		{
			ValueTask<int> valueTask = this._stream.ReadAsync(this._receiveBuffer, cancellationToken);
			if (!valueTask.IsCompletedSuccessfully)
			{
				using (CancellationTokenSource finalCts = new CancellationTokenSource(1000))
				{
					using (finalCts.Token.Register(delegate(object s)
					{
						((ManagedWebSocket)s).Abort();
					}, this))
					{
						try
						{
							await valueTask.ConfigureAwait(false);
						}
						catch
						{
						}
					}
					CancellationTokenRegistration cancellationTokenRegistration = default(CancellationTokenRegistration);
				}
				CancellationTokenSource finalCts = null;
			}
		}

		// Token: 0x0600426B RID: 17003 RVA: 0x000E57A4 File Offset: 0x000E39A4
		private async Task HandleReceivedPingPongAsync(ManagedWebSocket.MessageHeader header, CancellationToken cancellationToken)
		{
			if (header.PayloadLength > 0L && (long)this._receiveBufferCount < header.PayloadLength)
			{
				await this.EnsureBufferContainsAsync((int)header.PayloadLength, cancellationToken, true).ConfigureAwait(false);
			}
			if (header.Opcode == ManagedWebSocket.MessageOpcode.Ping)
			{
				if (this._isServer)
				{
					ManagedWebSocket.ApplyMask(this._receiveBuffer.Span.Slice(this._receiveBufferOffset, (int)header.PayloadLength), header.Mask, 0);
				}
				await this.SendFrameAsync(ManagedWebSocket.MessageOpcode.Pong, true, this._receiveBuffer.Slice(this._receiveBufferOffset, (int)header.PayloadLength), default(CancellationToken)).ConfigureAwait(false);
			}
			if (header.PayloadLength > 0L)
			{
				this.ConsumeFromBuffer((int)header.PayloadLength);
			}
		}

		// Token: 0x0600426C RID: 17004 RVA: 0x000E57F7 File Offset: 0x000E39F7
		private static bool IsValidCloseStatus(WebSocketCloseStatus closeStatus)
		{
			return closeStatus >= WebSocketCloseStatus.NormalClosure && closeStatus < (WebSocketCloseStatus)5000 && (closeStatus >= (WebSocketCloseStatus)3000 || (closeStatus - WebSocketCloseStatus.NormalClosure <= 3 || closeStatus - WebSocketCloseStatus.InvalidPayloadData <= 4));
		}

		// Token: 0x0600426D RID: 17005 RVA: 0x000E582C File Offset: 0x000E3A2C
		private async Task CloseWithReceiveErrorAndThrowAsync(WebSocketCloseStatus closeStatus, WebSocketError error, Exception innerException = null)
		{
			if (!this._sentCloseFrame)
			{
				await this.CloseOutputAsync(closeStatus, string.Empty, default(CancellationToken)).ConfigureAwait(false);
			}
			this._receiveBufferCount = 0;
			throw new WebSocketException(error, innerException);
		}

		// Token: 0x0600426E RID: 17006 RVA: 0x000E5888 File Offset: 0x000E3A88
		private unsafe bool TryParseMessageHeaderFromReceiveBuffer(out ManagedWebSocket.MessageHeader resultHeader)
		{
			ManagedWebSocket.MessageHeader messageHeader = default(ManagedWebSocket.MessageHeader);
			Span<byte> span = this._receiveBuffer.Span;
			messageHeader.Fin = (*span[this._receiveBufferOffset] & 128) > 0;
			bool flag = (*span[this._receiveBufferOffset] & 112) > 0;
			messageHeader.Opcode = (ManagedWebSocket.MessageOpcode)(*span[this._receiveBufferOffset] & 15);
			bool flag2 = (*span[this._receiveBufferOffset + 1] & 128) > 0;
			messageHeader.PayloadLength = (long)(*span[this._receiveBufferOffset + 1] & 127);
			this.ConsumeFromBuffer(2);
			if (messageHeader.PayloadLength == 126L)
			{
				messageHeader.PayloadLength = (long)(((int)(*span[this._receiveBufferOffset]) << 8) | (int)(*span[this._receiveBufferOffset + 1]));
				this.ConsumeFromBuffer(2);
			}
			else if (messageHeader.PayloadLength == 127L)
			{
				messageHeader.PayloadLength = 0L;
				for (int i = 0; i < 8; i++)
				{
					messageHeader.PayloadLength = (messageHeader.PayloadLength << 8) | (long)((ulong)(*span[this._receiveBufferOffset + i]));
				}
				this.ConsumeFromBuffer(8);
			}
			bool flag3 = flag;
			if (flag2)
			{
				if (!this._isServer)
				{
					flag3 = true;
				}
				messageHeader.Mask = ManagedWebSocket.CombineMaskBytes(span, this._receiveBufferOffset);
				this.ConsumeFromBuffer(4);
			}
			switch (messageHeader.Opcode)
			{
			case ManagedWebSocket.MessageOpcode.Continuation:
				if (this._lastReceiveHeader.Fin)
				{
					flag3 = true;
					goto IL_01CD;
				}
				goto IL_01CD;
			case ManagedWebSocket.MessageOpcode.Text:
			case ManagedWebSocket.MessageOpcode.Binary:
				if (!this._lastReceiveHeader.Fin)
				{
					flag3 = true;
					goto IL_01CD;
				}
				goto IL_01CD;
			case ManagedWebSocket.MessageOpcode.Close:
			case ManagedWebSocket.MessageOpcode.Ping:
			case ManagedWebSocket.MessageOpcode.Pong:
				if (messageHeader.PayloadLength > 125L || !messageHeader.Fin)
				{
					flag3 = true;
					goto IL_01CD;
				}
				goto IL_01CD;
			}
			flag3 = true;
			IL_01CD:
			resultHeader = messageHeader;
			return !flag3;
		}

		// Token: 0x0600426F RID: 17007 RVA: 0x000E5A70 File Offset: 0x000E3C70
		private async Task CloseAsyncPrivate(WebSocketCloseStatus closeStatus, string statusDescription, CancellationToken cancellationToken)
		{
			if (!this._sentCloseFrame)
			{
				await this.SendCloseFrameAsync(closeStatus, statusDescription, cancellationToken).ConfigureAwait(false);
			}
			byte[] closeBuffer = ArrayPool<byte>.Shared.Rent(139);
			object obj;
			try
			{
				while (!this._receivedCloseFrame)
				{
					obj = this.ReceiveAsyncLock;
					Task task;
					lock (obj)
					{
						if (this._receivedCloseFrame)
						{
							break;
						}
						task = this._lastReceiveAsync;
						task = (this._lastReceiveAsync = this.ValidateAndReceiveAsync(task, closeBuffer, cancellationToken));
					}
					await task.ConfigureAwait(false);
				}
			}
			finally
			{
				ArrayPool<byte>.Shared.Return(closeBuffer, false);
			}
			obj = this.StateUpdateLock;
			lock (obj)
			{
				this.DisposeCore();
				if (this._state < WebSocketState.Closed)
				{
					this._state = WebSocketState.Closed;
				}
			}
		}

		// Token: 0x06004270 RID: 17008 RVA: 0x000E5ACC File Offset: 0x000E3CCC
		private async Task SendCloseFrameAsync(WebSocketCloseStatus closeStatus, string closeStatusDescription, CancellationToken cancellationToken)
		{
			byte[] buffer = null;
			try
			{
				int num = 2;
				if (string.IsNullOrEmpty(closeStatusDescription))
				{
					buffer = ArrayPool<byte>.Shared.Rent(num);
				}
				else
				{
					num += ManagedWebSocket.s_textEncoding.GetByteCount(closeStatusDescription);
					buffer = ArrayPool<byte>.Shared.Rent(num);
					ManagedWebSocket.s_textEncoding.GetBytes(closeStatusDescription, 0, closeStatusDescription.Length, buffer, 2);
				}
				ushort num2 = (ushort)closeStatus;
				buffer[0] = (byte)(num2 >> 8);
				buffer[1] = (byte)(num2 & 255);
				await this.SendFrameAsync(ManagedWebSocket.MessageOpcode.Close, true, new Memory<byte>(buffer, 0, num), cancellationToken).ConfigureAwait(false);
			}
			finally
			{
				if (buffer != null)
				{
					ArrayPool<byte>.Shared.Return(buffer, false);
				}
			}
			object stateUpdateLock = this.StateUpdateLock;
			lock (stateUpdateLock)
			{
				this._sentCloseFrame = true;
				if (this._state <= WebSocketState.CloseReceived)
				{
					this._state = WebSocketState.CloseSent;
				}
			}
			if (!this._isServer && this._receivedCloseFrame)
			{
				await this.WaitForServerToCloseConnectionAsync(cancellationToken).ConfigureAwait(false);
			}
		}

		// Token: 0x06004271 RID: 17009 RVA: 0x000E5B27 File Offset: 0x000E3D27
		private void ConsumeFromBuffer(int count)
		{
			this._receiveBufferCount -= count;
			this._receiveBufferOffset += count;
		}

		// Token: 0x06004272 RID: 17010 RVA: 0x000E5B48 File Offset: 0x000E3D48
		private async Task EnsureBufferContainsAsync(int minimumRequiredBytes, CancellationToken cancellationToken, bool throwOnPrematureClosure = true)
		{
			if (this._receiveBufferCount < minimumRequiredBytes)
			{
				if (this._receiveBufferCount > 0)
				{
					this._receiveBuffer.Span.Slice(this._receiveBufferOffset, this._receiveBufferCount).CopyTo(this._receiveBuffer.Span);
				}
				this._receiveBufferOffset = 0;
				while (this._receiveBufferCount < minimumRequiredBytes)
				{
					int num = await this._stream.ReadAsync(this._receiveBuffer.Slice(this._receiveBufferCount, this._receiveBuffer.Length - this._receiveBufferCount), cancellationToken).ConfigureAwait(false);
					if (num <= 0)
					{
						this.ThrowIfEOFUnexpected(throwOnPrematureClosure);
						break;
					}
					this._receiveBufferCount += num;
				}
			}
		}

		// Token: 0x06004273 RID: 17011 RVA: 0x000E5BA3 File Offset: 0x000E3DA3
		private void ThrowIfEOFUnexpected(bool throwOnPrematureClosure)
		{
			if (this._disposed)
			{
				throw new ObjectDisposedException("WebSocket");
			}
			if (throwOnPrematureClosure)
			{
				throw new WebSocketException(WebSocketError.ConnectionClosedPrematurely);
			}
		}

		// Token: 0x06004274 RID: 17012 RVA: 0x000E5BC2 File Offset: 0x000E3DC2
		private void AllocateSendBuffer(int minLength)
		{
			this._sendBuffer = ArrayPool<byte>.Shared.Rent(minLength);
		}

		// Token: 0x06004275 RID: 17013 RVA: 0x000E5BD8 File Offset: 0x000E3DD8
		private void ReleaseSendBuffer()
		{
			byte[] sendBuffer = this._sendBuffer;
			if (sendBuffer != null)
			{
				this._sendBuffer = null;
				ArrayPool<byte>.Shared.Return(sendBuffer, false);
			}
		}

		// Token: 0x06004276 RID: 17014 RVA: 0x000E5C02 File Offset: 0x000E3E02
		private static int CombineMaskBytes(Span<byte> buffer, int maskOffset)
		{
			return BitConverter.ToInt32(buffer.Slice(maskOffset));
		}

		// Token: 0x06004277 RID: 17015 RVA: 0x000E5C16 File Offset: 0x000E3E16
		private static int ApplyMask(Span<byte> toMask, byte[] mask, int maskOffset, int maskOffsetIndex)
		{
			return ManagedWebSocket.ApplyMask(toMask, ManagedWebSocket.CombineMaskBytes(mask, maskOffset), maskOffsetIndex);
		}

		// Token: 0x06004278 RID: 17016 RVA: 0x000E5C2C File Offset: 0x000E3E2C
		private unsafe static int ApplyMask(Span<byte> toMask, int mask, int maskIndex)
		{
			int num = maskIndex * 8;
			int num2 = (int)(((uint)mask >> num) | (uint)((uint)mask << 32 - num));
			int i = toMask.Length;
			if (i > 0)
			{
				fixed (byte* reference = MemoryMarshal.GetReference<byte>(toMask))
				{
					byte* ptr = reference;
					if (ptr % 4L == null)
					{
						while (i >= 4)
						{
							i -= 4;
							*(int*)ptr ^= num2;
							ptr += 4;
						}
					}
					if (i > 0)
					{
						byte* ptr2 = (byte*)(&mask);
						byte* ptr3 = ptr + i;
						while (ptr < ptr3)
						{
							byte* ptr4 = ptr++;
							*ptr4 ^= ptr2[maskIndex];
							maskIndex = (maskIndex + 1) & 3;
						}
					}
				}
			}
			return maskIndex;
		}

		// Token: 0x06004279 RID: 17017 RVA: 0x000E5CB7 File Offset: 0x000E3EB7
		private void ThrowIfOperationInProgress(bool operationCompleted, [CallerMemberName] string methodName = null)
		{
			if (!operationCompleted)
			{
				this.Abort();
				this.ThrowOperationInProgress(methodName);
			}
		}

		// Token: 0x0600427A RID: 17018 RVA: 0x000E5CC9 File Offset: 0x000E3EC9
		private void ThrowOperationInProgress(string methodName)
		{
			throw new InvalidOperationException(SR.Format("There is already one outstanding '{0}' call for this WebSocket instance. ReceiveAsync and SendAsync can be called simultaneously, but at most one outstanding operation for each of them is allowed at the same time.", methodName));
		}

		// Token: 0x0600427B RID: 17019 RVA: 0x000E5CDB File Offset: 0x000E3EDB
		private static Exception CreateOperationCanceledException(Exception innerException, CancellationToken cancellationToken = default(CancellationToken))
		{
			return new OperationCanceledException(new OperationCanceledException().Message, innerException, cancellationToken);
		}

		// Token: 0x0600427C RID: 17020 RVA: 0x000E5CF0 File Offset: 0x000E3EF0
		private unsafe static bool TryValidateUtf8(Span<byte> span, bool endOfMessage, ManagedWebSocket.Utf8MessageState state)
		{
			int i = 0;
			while (i < span.Length)
			{
				if (!state.SequenceInProgress)
				{
					state.SequenceInProgress = true;
					byte b = *span[i];
					i++;
					if ((b & 128) == 0)
					{
						state.AdditionalBytesExpected = 0;
						state.CurrentDecodeBits = (int)(b & 127);
						state.ExpectedValueMin = 0;
					}
					else
					{
						if ((b & 192) == 128)
						{
							return false;
						}
						if ((b & 224) == 192)
						{
							state.AdditionalBytesExpected = 1;
							state.CurrentDecodeBits = (int)(b & 31);
							state.ExpectedValueMin = 128;
						}
						else if ((b & 240) == 224)
						{
							state.AdditionalBytesExpected = 2;
							state.CurrentDecodeBits = (int)(b & 15);
							state.ExpectedValueMin = 2048;
						}
						else
						{
							if ((b & 248) != 240)
							{
								return false;
							}
							state.AdditionalBytesExpected = 3;
							state.CurrentDecodeBits = (int)(b & 7);
							state.ExpectedValueMin = 65536;
						}
					}
				}
				while (state.AdditionalBytesExpected > 0 && i < span.Length)
				{
					byte b2 = *span[i];
					if ((b2 & 192) != 128)
					{
						return false;
					}
					i++;
					state.AdditionalBytesExpected--;
					state.CurrentDecodeBits = (state.CurrentDecodeBits << 6) | (int)(b2 & 63);
					if (state.AdditionalBytesExpected == 1 && state.CurrentDecodeBits >= 864 && state.CurrentDecodeBits <= 895)
					{
						return false;
					}
					if (state.AdditionalBytesExpected == 2 && state.CurrentDecodeBits >= 272)
					{
						return false;
					}
				}
				if (state.AdditionalBytesExpected == 0)
				{
					state.SequenceInProgress = false;
					if (state.CurrentDecodeBits < state.ExpectedValueMin)
					{
						return false;
					}
				}
			}
			return !endOfMessage || !state.SequenceInProgress;
		}

		// Token: 0x0600427D RID: 17021 RVA: 0x000E5EB4 File Offset: 0x000E40B4
		private Task ValidateAndReceiveAsync(Task receiveTask, byte[] buffer, CancellationToken cancellationToken)
		{
			if (receiveTask != null)
			{
				if (receiveTask.Status != TaskStatus.RanToCompletion)
				{
					return receiveTask;
				}
				Task<WebSocketReceiveResult> task = receiveTask as Task<WebSocketReceiveResult>;
				if (task != null && task.Result.MessageType == WebSocketMessageType.Close)
				{
					return receiveTask;
				}
			}
			receiveTask = this.ReceiveAsyncPrivate<ManagedWebSocket.WebSocketReceiveResultGetter, WebSocketReceiveResult>(new ArraySegment<byte>(buffer), cancellationToken, default(ManagedWebSocket.WebSocketReceiveResultGetter)).AsTask();
			return receiveTask;
		}

		// Token: 0x040027A0 RID: 10144
		private static readonly RandomNumberGenerator s_random = RandomNumberGenerator.Create();

		// Token: 0x040027A1 RID: 10145
		private static readonly UTF8Encoding s_textEncoding = new UTF8Encoding(false, true);

		// Token: 0x040027A2 RID: 10146
		private static readonly WebSocketState[] s_validSendStates = new WebSocketState[]
		{
			WebSocketState.Open,
			WebSocketState.CloseReceived
		};

		// Token: 0x040027A3 RID: 10147
		private static readonly WebSocketState[] s_validReceiveStates = new WebSocketState[]
		{
			WebSocketState.Open,
			WebSocketState.CloseSent
		};

		// Token: 0x040027A4 RID: 10148
		private static readonly WebSocketState[] s_validCloseOutputStates = new WebSocketState[]
		{
			WebSocketState.Open,
			WebSocketState.CloseReceived
		};

		// Token: 0x040027A5 RID: 10149
		private static readonly WebSocketState[] s_validCloseStates = new WebSocketState[]
		{
			WebSocketState.Open,
			WebSocketState.CloseReceived,
			WebSocketState.CloseSent
		};

		// Token: 0x040027A6 RID: 10150
		private static readonly Task<WebSocketReceiveResult> s_cachedCloseTask = Task.FromResult<WebSocketReceiveResult>(new WebSocketReceiveResult(0, WebSocketMessageType.Close, true));

		// Token: 0x040027A7 RID: 10151
		internal const int MaxMessageHeaderLength = 14;

		// Token: 0x040027A8 RID: 10152
		private const int MaxControlPayloadLength = 125;

		// Token: 0x040027A9 RID: 10153
		private const int MaskLength = 4;

		// Token: 0x040027AA RID: 10154
		private readonly Stream _stream;

		// Token: 0x040027AB RID: 10155
		private readonly bool _isServer;

		// Token: 0x040027AC RID: 10156
		private readonly string _subprotocol;

		// Token: 0x040027AD RID: 10157
		private readonly Timer _keepAliveTimer;

		// Token: 0x040027AE RID: 10158
		private readonly CancellationTokenSource _abortSource = new CancellationTokenSource();

		// Token: 0x040027AF RID: 10159
		private Memory<byte> _receiveBuffer;

		// Token: 0x040027B0 RID: 10160
		private readonly ManagedWebSocket.Utf8MessageState _utf8TextState = new ManagedWebSocket.Utf8MessageState();

		// Token: 0x040027B1 RID: 10161
		private readonly SemaphoreSlim _sendFrameAsyncLock = new SemaphoreSlim(1, 1);

		// Token: 0x040027B2 RID: 10162
		private WebSocketState _state = WebSocketState.Open;

		// Token: 0x040027B3 RID: 10163
		private bool _disposed;

		// Token: 0x040027B4 RID: 10164
		private bool _sentCloseFrame;

		// Token: 0x040027B5 RID: 10165
		private bool _receivedCloseFrame;

		// Token: 0x040027B6 RID: 10166
		private WebSocketCloseStatus? _closeStatus;

		// Token: 0x040027B7 RID: 10167
		private string _closeStatusDescription;

		// Token: 0x040027B8 RID: 10168
		private ManagedWebSocket.MessageHeader _lastReceiveHeader = new ManagedWebSocket.MessageHeader
		{
			Opcode = ManagedWebSocket.MessageOpcode.Text,
			Fin = true
		};

		// Token: 0x040027B9 RID: 10169
		private int _receiveBufferOffset;

		// Token: 0x040027BA RID: 10170
		private int _receiveBufferCount;

		// Token: 0x040027BB RID: 10171
		private int _receivedMaskOffsetOffset;

		// Token: 0x040027BC RID: 10172
		private byte[] _sendBuffer;

		// Token: 0x040027BD RID: 10173
		private bool _lastSendWasFragment;

		// Token: 0x040027BE RID: 10174
		private Task _lastReceiveAsync = Task.CompletedTask;

		// Token: 0x02000816 RID: 2070
		private sealed class Utf8MessageState
		{
			// Token: 0x040027BF RID: 10175
			internal bool SequenceInProgress;

			// Token: 0x040027C0 RID: 10176
			internal int AdditionalBytesExpected;

			// Token: 0x040027C1 RID: 10177
			internal int ExpectedValueMin;

			// Token: 0x040027C2 RID: 10178
			internal int CurrentDecodeBits;
		}

		// Token: 0x02000817 RID: 2071
		private enum MessageOpcode : byte
		{
			// Token: 0x040027C4 RID: 10180
			Continuation,
			// Token: 0x040027C5 RID: 10181
			Text,
			// Token: 0x040027C6 RID: 10182
			Binary,
			// Token: 0x040027C7 RID: 10183
			Close = 8,
			// Token: 0x040027C8 RID: 10184
			Ping,
			// Token: 0x040027C9 RID: 10185
			Pong
		}

		// Token: 0x02000818 RID: 2072
		[StructLayout(LayoutKind.Auto)]
		private struct MessageHeader
		{
			// Token: 0x040027CA RID: 10186
			internal ManagedWebSocket.MessageOpcode Opcode;

			// Token: 0x040027CB RID: 10187
			internal bool Fin;

			// Token: 0x040027CC RID: 10188
			internal long PayloadLength;

			// Token: 0x040027CD RID: 10189
			internal int Mask;
		}

		// Token: 0x02000819 RID: 2073
		private interface IWebSocketReceiveResultGetter<TResult>
		{
			// Token: 0x06004280 RID: 17024
			TResult GetResult(int count, WebSocketMessageType messageType, bool endOfMessage, WebSocketCloseStatus? closeStatus, string closeDescription);
		}

		// Token: 0x0200081A RID: 2074
		private readonly struct WebSocketReceiveResultGetter : ManagedWebSocket.IWebSocketReceiveResultGetter<WebSocketReceiveResult>
		{
			// Token: 0x06004281 RID: 17025 RVA: 0x000E5F90 File Offset: 0x000E4190
			public WebSocketReceiveResult GetResult(int count, WebSocketMessageType messageType, bool endOfMessage, WebSocketCloseStatus? closeStatus, string closeDescription)
			{
				return new WebSocketReceiveResult(count, messageType, endOfMessage, closeStatus, closeDescription);
			}
		}
	}
}
