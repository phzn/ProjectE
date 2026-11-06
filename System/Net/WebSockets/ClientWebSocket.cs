using System;
using System.Threading;
using System.Threading.Tasks;

namespace System.Net.WebSockets
{
	/// <summary>Provides a client for connecting to WebSocket services.</summary>
	// Token: 0x02000829 RID: 2089
	public sealed class ClientWebSocket : WebSocket
	{
		/// <summary>Creates an instance of the <see cref="T:System.Net.WebSockets.ClientWebSocket" /> class.</summary>
		// Token: 0x060042B5 RID: 17077 RVA: 0x000E80C8 File Offset: 0x000E62C8
		public ClientWebSocket()
		{
			if (NetEventSource.IsEnabled)
			{
				NetEventSource.Enter(this, null, ".ctor");
			}
			WebSocketHandle.CheckPlatformSupport();
			this._state = 0;
			this._options = new ClientWebSocketOptions
			{
				Proxy = ClientWebSocket.DefaultWebProxy.Instance
			};
			if (NetEventSource.IsEnabled)
			{
				NetEventSource.Exit(this, null, ".ctor");
			}
		}

		/// <summary>Gets the WebSocket options for the <see cref="T:System.Net.WebSockets.ClientWebSocket" /> instance.</summary>
		/// <returns>The WebSocket options for the <see cref="T:System.Net.WebSockets.ClientWebSocket" /> instance.</returns>
		// Token: 0x17000EF2 RID: 3826
		// (get) Token: 0x060042B6 RID: 17078 RVA: 0x000E8123 File Offset: 0x000E6323
		public ClientWebSocketOptions Options
		{
			get
			{
				return this._options;
			}
		}

		/// <summary>Gets the reason why the close handshake was initiated on <see cref="T:System.Net.WebSockets.ClientWebSocket" /> instance.</summary>
		/// <returns>The reason why the close handshake was initiated.</returns>
		// Token: 0x17000EF3 RID: 3827
		// (get) Token: 0x060042B7 RID: 17079 RVA: 0x000E812C File Offset: 0x000E632C
		public override WebSocketCloseStatus? CloseStatus
		{
			get
			{
				if (WebSocketHandle.IsValid(this._innerWebSocket))
				{
					return this._innerWebSocket.CloseStatus;
				}
				return null;
			}
		}

		/// <summary>Gets a description of the reason why the <see cref="T:System.Net.WebSockets.ClientWebSocket" /> instance was closed.</summary>
		/// <returns>The description of the reason why the <see cref="T:System.Net.WebSockets.ClientWebSocket" /> instance was closed.</returns>
		// Token: 0x17000EF4 RID: 3828
		// (get) Token: 0x060042B8 RID: 17080 RVA: 0x000E815B File Offset: 0x000E635B
		public override string CloseStatusDescription
		{
			get
			{
				if (WebSocketHandle.IsValid(this._innerWebSocket))
				{
					return this._innerWebSocket.CloseStatusDescription;
				}
				return null;
			}
		}

		/// <summary>Gets the supported WebSocket sub-protocol for the <see cref="T:System.Net.WebSockets.ClientWebSocket" /> instance.</summary>
		/// <returns>The supported WebSocket sub-protocol.</returns>
		// Token: 0x17000EF5 RID: 3829
		// (get) Token: 0x060042B9 RID: 17081 RVA: 0x000E8177 File Offset: 0x000E6377
		public override string SubProtocol
		{
			get
			{
				if (WebSocketHandle.IsValid(this._innerWebSocket))
				{
					return this._innerWebSocket.SubProtocol;
				}
				return null;
			}
		}

		/// <summary>Gets the WebSocket state of the <see cref="T:System.Net.WebSockets.ClientWebSocket" /> instance.</summary>
		/// <returns>The WebSocket state of the <see cref="T:System.Net.WebSockets.ClientWebSocket" /> instance.</returns>
		// Token: 0x17000EF6 RID: 3830
		// (get) Token: 0x060042BA RID: 17082 RVA: 0x000E8194 File Offset: 0x000E6394
		public override WebSocketState State
		{
			get
			{
				if (WebSocketHandle.IsValid(this._innerWebSocket))
				{
					return this._innerWebSocket.State;
				}
				ClientWebSocket.InternalState state = (ClientWebSocket.InternalState)this._state;
				if (state == ClientWebSocket.InternalState.Created)
				{
					return WebSocketState.None;
				}
				if (state != ClientWebSocket.InternalState.Connecting)
				{
					return WebSocketState.Closed;
				}
				return WebSocketState.Connecting;
			}
		}

		/// <summary>Connect to a WebSocket server as an asynchronous operation.</summary>
		/// <param name="uri">The URI of the WebSocket server to connect to.</param>
		/// <param name="cancellationToken">A cancellation token used to propagate notification that the  operation should be canceled.</param>
		/// <returns>The task object representing the asynchronous operation.</returns>
		// Token: 0x060042BB RID: 17083 RVA: 0x000E81D0 File Offset: 0x000E63D0
		public Task ConnectAsync(Uri uri, CancellationToken cancellationToken)
		{
			if (uri == null)
			{
				throw new ArgumentNullException("uri");
			}
			if (!uri.IsAbsoluteUri)
			{
				throw new ArgumentException("This operation is not supported for a relative URI.", "uri");
			}
			if (uri.Scheme != "ws" && uri.Scheme != "wss")
			{
				throw new ArgumentException("Only Uris starting with 'ws://' or 'wss://' are supported.", "uri");
			}
			ClientWebSocket.InternalState internalState = (ClientWebSocket.InternalState)Interlocked.CompareExchange(ref this._state, 1, 0);
			if (internalState == ClientWebSocket.InternalState.Disposed)
			{
				throw new ObjectDisposedException(base.GetType().FullName);
			}
			if (internalState != ClientWebSocket.InternalState.Created)
			{
				throw new InvalidOperationException("The WebSocket has already been started.");
			}
			this._options.SetToReadOnly();
			return this.ConnectAsyncCore(uri, cancellationToken);
		}

		// Token: 0x060042BC RID: 17084 RVA: 0x000E8284 File Offset: 0x000E6484
		private async Task ConnectAsyncCore(Uri uri, CancellationToken cancellationToken)
		{
			this._innerWebSocket = WebSocketHandle.Create();
			try
			{
				if (Interlocked.CompareExchange(ref this._state, 2, 1) != 1)
				{
					throw new ObjectDisposedException(base.GetType().FullName);
				}
				await this._innerWebSocket.ConnectAsyncCore(uri, cancellationToken, this._options).ConfigureAwait(false);
			}
			catch (Exception ex)
			{
				if (NetEventSource.IsEnabled)
				{
					NetEventSource.Error(this, ex, "ConnectAsyncCore");
				}
				throw;
			}
		}

		/// <summary>Send data on <see cref="T:System.Net.WebSockets.ClientWebSocket" /> as an asynchronous operation.</summary>
		/// <param name="buffer">The buffer containing the message to be sent.</param>
		/// <param name="messageType">Specifies whether the buffer is clear text or in a binary format.</param>
		/// <param name="endOfMessage">Specifies whether this is the final asynchronous send. Set to <see langword="true" /> if this is the final send; <see langword="false" /> otherwise.</param>
		/// <param name="cancellationToken">A cancellation token used to propagate notification that this  operation should be canceled.</param>
		/// <returns>The task object representing the asynchronous operation.</returns>
		// Token: 0x060042BD RID: 17085 RVA: 0x000E82D7 File Offset: 0x000E64D7
		public override Task SendAsync(ArraySegment<byte> buffer, WebSocketMessageType messageType, bool endOfMessage, CancellationToken cancellationToken)
		{
			this.ThrowIfNotConnected();
			return this._innerWebSocket.SendAsync(buffer, messageType, endOfMessage, cancellationToken);
		}

		// Token: 0x060042BE RID: 17086 RVA: 0x000E82EF File Offset: 0x000E64EF
		public override ValueTask SendAsync(ReadOnlyMemory<byte> buffer, WebSocketMessageType messageType, bool endOfMessage, CancellationToken cancellationToken)
		{
			this.ThrowIfNotConnected();
			return this._innerWebSocket.SendAsync(buffer, messageType, endOfMessage, cancellationToken);
		}

		/// <summary>Receives data on <see cref="T:System.Net.WebSockets.ClientWebSocket" /> as an asynchronous operation.</summary>
		/// <param name="buffer">The buffer to receive the response.</param>
		/// <param name="cancellationToken">A cancellation token used to propagate notification that this  operation should be canceled.</param>
		/// <returns>The task object representing the asynchronous operation.</returns>
		// Token: 0x060042BF RID: 17087 RVA: 0x000E8307 File Offset: 0x000E6507
		public override Task<WebSocketReceiveResult> ReceiveAsync(ArraySegment<byte> buffer, CancellationToken cancellationToken)
		{
			this.ThrowIfNotConnected();
			return this._innerWebSocket.ReceiveAsync(buffer, cancellationToken);
		}

		// Token: 0x060042C0 RID: 17088 RVA: 0x000E831C File Offset: 0x000E651C
		public override ValueTask<ValueWebSocketReceiveResult> ReceiveAsync(Memory<byte> buffer, CancellationToken cancellationToken)
		{
			this.ThrowIfNotConnected();
			return this._innerWebSocket.ReceiveAsync(buffer, cancellationToken);
		}

		/// <summary>Close the <see cref="T:System.Net.WebSockets.ClientWebSocket" /> instance as an asynchronous operation.</summary>
		/// <param name="closeStatus">The WebSocket close status.</param>
		/// <param name="statusDescription">A description of the close status.</param>
		/// <param name="cancellationToken">A cancellation token used to propagate notification that this  operation should be canceled.</param>
		/// <returns>The task object representing the asynchronous operation.</returns>
		// Token: 0x060042C1 RID: 17089 RVA: 0x000E8331 File Offset: 0x000E6531
		public override Task CloseAsync(WebSocketCloseStatus closeStatus, string statusDescription, CancellationToken cancellationToken)
		{
			this.ThrowIfNotConnected();
			return this._innerWebSocket.CloseAsync(closeStatus, statusDescription, cancellationToken);
		}

		/// <summary>Close the output for the <see cref="T:System.Net.WebSockets.ClientWebSocket" /> instance as an asynchronous operation.</summary>
		/// <param name="closeStatus">The WebSocket close status.</param>
		/// <param name="statusDescription">A description of the close status.</param>
		/// <param name="cancellationToken">A cancellation token used to propagate notification that this  operation should be canceled.</param>
		/// <returns>The task object representing the asynchronous operation.</returns>
		// Token: 0x060042C2 RID: 17090 RVA: 0x000E8347 File Offset: 0x000E6547
		public override Task CloseOutputAsync(WebSocketCloseStatus closeStatus, string statusDescription, CancellationToken cancellationToken)
		{
			this.ThrowIfNotConnected();
			return this._innerWebSocket.CloseOutputAsync(closeStatus, statusDescription, cancellationToken);
		}

		/// <summary>Aborts the connection and cancels any pending IO operations.</summary>
		// Token: 0x060042C3 RID: 17091 RVA: 0x000E835D File Offset: 0x000E655D
		public override void Abort()
		{
			if (this._state == 3)
			{
				return;
			}
			if (WebSocketHandle.IsValid(this._innerWebSocket))
			{
				this._innerWebSocket.Abort();
			}
			this.Dispose();
		}

		/// <summary>Releases the unmanaged resources used by the <see cref="T:System.Net.WebSockets.ClientWebSocket" /> instance.</summary>
		// Token: 0x060042C4 RID: 17092 RVA: 0x000E8387 File Offset: 0x000E6587
		public override void Dispose()
		{
			if (Interlocked.Exchange(ref this._state, 3) == 3)
			{
				return;
			}
			if (WebSocketHandle.IsValid(this._innerWebSocket))
			{
				this._innerWebSocket.Dispose();
			}
		}

		// Token: 0x060042C5 RID: 17093 RVA: 0x000E83B1 File Offset: 0x000E65B1
		private void ThrowIfNotConnected()
		{
			if (this._state == 3)
			{
				throw new ObjectDisposedException(base.GetType().FullName);
			}
			if (this._state != 2)
			{
				throw new InvalidOperationException("The WebSocket is not connected.");
			}
		}

		// Token: 0x04002837 RID: 10295
		private readonly ClientWebSocketOptions _options;

		// Token: 0x04002838 RID: 10296
		private WebSocketHandle _innerWebSocket;

		// Token: 0x04002839 RID: 10297
		private int _state;

		// Token: 0x0200082A RID: 2090
		private enum InternalState
		{
			// Token: 0x0400283B RID: 10299
			Created,
			// Token: 0x0400283C RID: 10300
			Connecting,
			// Token: 0x0400283D RID: 10301
			Connected,
			// Token: 0x0400283E RID: 10302
			Disposed
		}

		// Token: 0x0200082B RID: 2091
		internal sealed class DefaultWebProxy : IWebProxy
		{
			// Token: 0x17000EF7 RID: 3831
			// (get) Token: 0x060042C6 RID: 17094 RVA: 0x000E83E1 File Offset: 0x000E65E1
			public static ClientWebSocket.DefaultWebProxy Instance { get; } = new ClientWebSocket.DefaultWebProxy();

			// Token: 0x17000EF8 RID: 3832
			// (get) Token: 0x060042C7 RID: 17095 RVA: 0x000044FA File Offset: 0x000026FA
			// (set) Token: 0x060042C8 RID: 17096 RVA: 0x000044FA File Offset: 0x000026FA
			public ICredentials Credentials
			{
				get
				{
					throw new NotSupportedException();
				}
				set
				{
					throw new NotSupportedException();
				}
			}

			// Token: 0x060042C9 RID: 17097 RVA: 0x000044FA File Offset: 0x000026FA
			public Uri GetProxy(Uri destination)
			{
				throw new NotSupportedException();
			}

			// Token: 0x060042CA RID: 17098 RVA: 0x000044FA File Offset: 0x000026FA
			public bool IsBypassed(Uri host)
			{
				throw new NotSupportedException();
			}
		}
	}
}
