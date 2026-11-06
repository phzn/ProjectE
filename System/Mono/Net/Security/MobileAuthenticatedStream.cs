using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Net;
using System.Net.Security;
using System.Runtime.CompilerServices;
using System.Runtime.ExceptionServices;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using System.Threading.Tasks;
using Mono.Security.Interface;

namespace Mono.Net.Security
{
	// Token: 0x02000098 RID: 152
	internal abstract class MobileAuthenticatedStream : AuthenticatedStream, IMonoSslStream, IDisposable
	{
		// Token: 0x0600025F RID: 607 RVA: 0x00007264 File Offset: 0x00005464
		public MobileAuthenticatedStream(Stream innerStream, bool leaveInnerStreamOpen, SslStream owner, MonoTlsSettings settings, MobileTlsProvider provider)
			: base(innerStream, leaveInnerStreamOpen)
		{
			this.SslStream = owner;
			this.Settings = settings;
			this.Provider = provider;
			this.readBuffer = new BufferOffsetSize2(16500);
			this.writeBuffer = new BufferOffsetSize2(16384);
			this.operation = MobileAuthenticatedStream.Operation.None;
		}

		// Token: 0x17000059 RID: 89
		// (get) Token: 0x06000260 RID: 608 RVA: 0x000072D5 File Offset: 0x000054D5
		public SslStream SslStream { get; }

		// Token: 0x1700005A RID: 90
		// (get) Token: 0x06000261 RID: 609 RVA: 0x000072DD File Offset: 0x000054DD
		public MonoTlsSettings Settings { get; }

		// Token: 0x1700005B RID: 91
		// (get) Token: 0x06000262 RID: 610 RVA: 0x000072E5 File Offset: 0x000054E5
		public MobileTlsProvider Provider { get; }

		// Token: 0x1700005C RID: 92
		// (get) Token: 0x06000263 RID: 611 RVA: 0x000072ED File Offset: 0x000054ED
		MonoTlsProvider IMonoSslStream.Provider
		{
			get
			{
				return this.Provider;
			}
		}

		// Token: 0x1700005D RID: 93
		// (get) Token: 0x06000264 RID: 612 RVA: 0x000072F5 File Offset: 0x000054F5
		internal bool HasContext
		{
			get
			{
				return this.xobileTlsContext != null;
			}
		}

		// Token: 0x1700005E RID: 94
		// (get) Token: 0x06000265 RID: 613 RVA: 0x00007300 File Offset: 0x00005500
		// (set) Token: 0x06000266 RID: 614 RVA: 0x00007308 File Offset: 0x00005508
		internal string TargetHost { get; private set; }

		// Token: 0x06000267 RID: 615 RVA: 0x00007314 File Offset: 0x00005514
		internal void CheckThrow(bool authSuccessCheck, bool shutdownCheck = false)
		{
			if (this.lastException != null)
			{
				this.lastException.Throw();
			}
			if (authSuccessCheck && !this.IsAuthenticated)
			{
				throw new InvalidOperationException("This operation is only allowed using a successfully authenticated context.");
			}
			if (shutdownCheck && this.shutdown)
			{
				throw new InvalidOperationException("Write operations are not allowed after the channel was shutdown.");
			}
		}

		// Token: 0x06000268 RID: 616 RVA: 0x00007360 File Offset: 0x00005560
		internal static Exception GetSSPIException(Exception e)
		{
			if (e is OperationCanceledException || e is IOException || e is ObjectDisposedException || e is AuthenticationException || e is NotSupportedException)
			{
				return e;
			}
			return new AuthenticationException("Authentication failed, see inner exception.", e);
		}

		// Token: 0x06000269 RID: 617 RVA: 0x00007397 File Offset: 0x00005597
		internal static Exception GetIOException(Exception e, string message)
		{
			if (e is OperationCanceledException || e is IOException || e is ObjectDisposedException || e is AuthenticationException || e is NotSupportedException)
			{
				return e;
			}
			return new IOException(message, e);
		}

		// Token: 0x0600026A RID: 618 RVA: 0x000073CC File Offset: 0x000055CC
		internal static Exception GetRenegotiationException(string message)
		{
			TlsException ex = new TlsException(AlertDescription.NoRenegotiation, message);
			return new AuthenticationException("Authentication failed, see inner exception.", ex);
		}

		// Token: 0x0600026B RID: 619 RVA: 0x000073ED File Offset: 0x000055ED
		internal static Exception GetInternalError()
		{
			throw new InvalidOperationException("Internal error.");
		}

		// Token: 0x0600026C RID: 620 RVA: 0x000073F9 File Offset: 0x000055F9
		internal static Exception GetInvalidNestedCallException()
		{
			throw new InvalidOperationException("Invalid nested call.");
		}

		// Token: 0x0600026D RID: 621 RVA: 0x00007408 File Offset: 0x00005608
		internal ExceptionDispatchInfo SetException(Exception e)
		{
			ExceptionDispatchInfo exceptionDispatchInfo = ExceptionDispatchInfo.Capture(e);
			return Interlocked.CompareExchange<ExceptionDispatchInfo>(ref this.lastException, exceptionDispatchInfo, null) ?? exceptionDispatchInfo;
		}

		// Token: 0x0600026E RID: 622 RVA: 0x00007430 File Offset: 0x00005630
		public void AuthenticateAsClient(string targetHost, X509CertificateCollection clientCertificates, SslProtocols enabledSslProtocols, bool checkCertificateRevocation)
		{
			MonoSslClientAuthenticationOptions monoSslClientAuthenticationOptions = new MonoSslClientAuthenticationOptions
			{
				TargetHost = targetHost,
				ClientCertificates = clientCertificates,
				EnabledSslProtocols = enabledSslProtocols,
				CertificateRevocationCheckMode = (checkCertificateRevocation ? X509RevocationMode.Online : X509RevocationMode.NoCheck),
				EncryptionPolicy = EncryptionPolicy.RequireEncryption
			};
			Task task = this.ProcessAuthentication(true, monoSslClientAuthenticationOptions, CancellationToken.None);
			try
			{
				task.Wait();
			}
			catch (Exception ex)
			{
				throw HttpWebRequest.FlattenException(ex);
			}
		}

		// Token: 0x0600026F RID: 623 RVA: 0x0000749C File Offset: 0x0000569C
		public void AuthenticateAsServer(X509Certificate serverCertificate, bool clientCertificateRequired, SslProtocols enabledSslProtocols, bool checkCertificateRevocation)
		{
			MonoSslServerAuthenticationOptions monoSslServerAuthenticationOptions = new MonoSslServerAuthenticationOptions
			{
				ServerCertificate = serverCertificate,
				ClientCertificateRequired = clientCertificateRequired,
				EnabledSslProtocols = enabledSslProtocols,
				CertificateRevocationCheckMode = (checkCertificateRevocation ? X509RevocationMode.Online : X509RevocationMode.NoCheck),
				EncryptionPolicy = EncryptionPolicy.RequireEncryption
			};
			Task task = this.ProcessAuthentication(true, monoSslServerAuthenticationOptions, CancellationToken.None);
			try
			{
				task.Wait();
			}
			catch (Exception ex)
			{
				throw HttpWebRequest.FlattenException(ex);
			}
		}

		// Token: 0x06000270 RID: 624 RVA: 0x00007508 File Offset: 0x00005708
		public Task AuthenticateAsClientAsync(string targetHost, X509CertificateCollection clientCertificates, SslProtocols enabledSslProtocols, bool checkCertificateRevocation)
		{
			MonoSslClientAuthenticationOptions monoSslClientAuthenticationOptions = new MonoSslClientAuthenticationOptions
			{
				TargetHost = targetHost,
				ClientCertificates = clientCertificates,
				EnabledSslProtocols = enabledSslProtocols,
				CertificateRevocationCheckMode = (checkCertificateRevocation ? X509RevocationMode.Online : X509RevocationMode.NoCheck),
				EncryptionPolicy = EncryptionPolicy.RequireEncryption
			};
			return this.ProcessAuthentication(false, monoSslClientAuthenticationOptions, CancellationToken.None);
		}

		// Token: 0x06000271 RID: 625 RVA: 0x00007552 File Offset: 0x00005752
		public Task AuthenticateAsClientAsync(IMonoSslClientAuthenticationOptions sslClientAuthenticationOptions, CancellationToken cancellationToken)
		{
			return this.ProcessAuthentication(false, (MonoSslClientAuthenticationOptions)sslClientAuthenticationOptions, cancellationToken);
		}

		// Token: 0x06000272 RID: 626 RVA: 0x00007564 File Offset: 0x00005764
		public Task AuthenticateAsServerAsync(X509Certificate serverCertificate, bool clientCertificateRequired, SslProtocols enabledSslProtocols, bool checkCertificateRevocation)
		{
			MonoSslServerAuthenticationOptions monoSslServerAuthenticationOptions = new MonoSslServerAuthenticationOptions
			{
				ServerCertificate = serverCertificate,
				ClientCertificateRequired = clientCertificateRequired,
				EnabledSslProtocols = enabledSslProtocols,
				CertificateRevocationCheckMode = (checkCertificateRevocation ? X509RevocationMode.Online : X509RevocationMode.NoCheck),
				EncryptionPolicy = EncryptionPolicy.RequireEncryption
			};
			return this.ProcessAuthentication(false, monoSslServerAuthenticationOptions, CancellationToken.None);
		}

		// Token: 0x06000273 RID: 627 RVA: 0x000075AE File Offset: 0x000057AE
		public Task AuthenticateAsServerAsync(IMonoSslServerAuthenticationOptions sslServerAuthenticationOptions, CancellationToken cancellationToken)
		{
			return this.ProcessAuthentication(false, (MonoSslServerAuthenticationOptions)sslServerAuthenticationOptions, cancellationToken);
		}

		// Token: 0x06000274 RID: 628 RVA: 0x000075C0 File Offset: 0x000057C0
		public Task ShutdownAsync()
		{
			AsyncShutdownRequest asyncShutdownRequest = new AsyncShutdownRequest(this);
			return this.StartOperation(MobileAuthenticatedStream.OperationType.Shutdown, asyncShutdownRequest, CancellationToken.None);
		}

		// Token: 0x1700005F RID: 95
		// (get) Token: 0x06000275 RID: 629 RVA: 0x000075E1 File Offset: 0x000057E1
		public AuthenticatedStream AuthenticatedStream
		{
			get
			{
				return this;
			}
		}

		// Token: 0x06000276 RID: 630 RVA: 0x000075E4 File Offset: 0x000057E4
		private async Task ProcessAuthentication(bool runSynchronously, MonoSslAuthenticationOptions options, CancellationToken cancellationToken)
		{
			if (options.ServerMode)
			{
				if (options.ServerCertificate == null && options.ServerCertSelectionDelegate == null)
				{
					throw new ArgumentException("ServerCertificate");
				}
			}
			else
			{
				if (options.TargetHost == null)
				{
					throw new ArgumentException("TargetHost");
				}
				if (options.TargetHost.Length == 0)
				{
					options.TargetHost = "?" + Interlocked.Increment(ref MobileAuthenticatedStream.uniqueNameInteger).ToString(NumberFormatInfo.InvariantInfo);
				}
				this.TargetHost = options.TargetHost;
			}
			if (this.lastException != null)
			{
				this.lastException.Throw();
			}
			AsyncHandshakeRequest asyncHandshakeRequest = new AsyncHandshakeRequest(this, runSynchronously);
			if (Interlocked.CompareExchange<AsyncProtocolRequest>(ref this.asyncHandshakeRequest, asyncHandshakeRequest, null) != null)
			{
				throw MobileAuthenticatedStream.GetInvalidNestedCallException();
			}
			if (Interlocked.CompareExchange<AsyncProtocolRequest>(ref this.asyncReadRequest, asyncHandshakeRequest, null) != null)
			{
				throw MobileAuthenticatedStream.GetInvalidNestedCallException();
			}
			if (Interlocked.CompareExchange<AsyncProtocolRequest>(ref this.asyncWriteRequest, asyncHandshakeRequest, null) != null)
			{
				throw MobileAuthenticatedStream.GetInvalidNestedCallException();
			}
			AsyncProtocolResult asyncProtocolResult;
			try
			{
				object obj = this.ioLock;
				lock (obj)
				{
					if (this.xobileTlsContext != null)
					{
						throw new InvalidOperationException();
					}
					this.readBuffer.Reset();
					this.writeBuffer.Reset();
					this.xobileTlsContext = this.CreateContext(options);
				}
				try
				{
					asyncProtocolResult = await asyncHandshakeRequest.StartOperation(cancellationToken).ConfigureAwait(false);
				}
				catch (Exception ex)
				{
					asyncProtocolResult = new AsyncProtocolResult(this.SetException(MobileAuthenticatedStream.GetSSPIException(ex)));
				}
			}
			finally
			{
				object obj = this.ioLock;
				bool flag = false;
				try
				{
					Monitor.Enter(obj, ref flag);
					this.readBuffer.Reset();
					this.writeBuffer.Reset();
					this.asyncWriteRequest = null;
					this.asyncReadRequest = null;
					this.asyncHandshakeRequest = null;
				}
				finally
				{
					int num;
					if (num < 0 && flag)
					{
						Monitor.Exit(obj);
					}
				}
			}
			if (asyncProtocolResult.Error != null)
			{
				asyncProtocolResult.Error.Throw();
			}
		}

		// Token: 0x06000277 RID: 631
		protected abstract MobileTlsContext CreateContext(MonoSslAuthenticationOptions options);

		// Token: 0x06000278 RID: 632 RVA: 0x00007640 File Offset: 0x00005840
		public override int Read(byte[] buffer, int offset, int count)
		{
			AsyncReadRequest asyncReadRequest = new AsyncReadRequest(this, true, buffer, offset, count);
			return this.StartOperation(MobileAuthenticatedStream.OperationType.Read, asyncReadRequest, CancellationToken.None).Result;
		}

		// Token: 0x06000279 RID: 633 RVA: 0x0000766C File Offset: 0x0000586C
		public override void Write(byte[] buffer, int offset, int count)
		{
			AsyncWriteRequest asyncWriteRequest = new AsyncWriteRequest(this, true, buffer, offset, count);
			this.StartOperation(MobileAuthenticatedStream.OperationType.Write, asyncWriteRequest, CancellationToken.None).Wait();
		}

		// Token: 0x0600027A RID: 634 RVA: 0x00007698 File Offset: 0x00005898
		public override Task<int> ReadAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken)
		{
			AsyncReadRequest asyncReadRequest = new AsyncReadRequest(this, false, buffer, offset, count);
			return this.StartOperation(MobileAuthenticatedStream.OperationType.Read, asyncReadRequest, cancellationToken);
		}

		// Token: 0x0600027B RID: 635 RVA: 0x000076BC File Offset: 0x000058BC
		public override Task WriteAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken)
		{
			AsyncWriteRequest asyncWriteRequest = new AsyncWriteRequest(this, false, buffer, offset, count);
			return this.StartOperation(MobileAuthenticatedStream.OperationType.Write, asyncWriteRequest, cancellationToken);
		}

		// Token: 0x17000060 RID: 96
		// (get) Token: 0x0600027C RID: 636 RVA: 0x000076DE File Offset: 0x000058DE
		public bool CanRenegotiate
		{
			get
			{
				this.CheckThrow(true, false);
				return this.xobileTlsContext != null && this.xobileTlsContext.CanRenegotiate;
			}
		}

		// Token: 0x0600027D RID: 637 RVA: 0x00007700 File Offset: 0x00005900
		public Task RenegotiateAsync(CancellationToken cancellationToken)
		{
			AsyncRenegotiateRequest asyncRenegotiateRequest = new AsyncRenegotiateRequest(this);
			return this.StartOperation(MobileAuthenticatedStream.OperationType.Renegotiate, asyncRenegotiateRequest, cancellationToken);
		}

		// Token: 0x0600027E RID: 638 RVA: 0x00007720 File Offset: 0x00005920
		private async Task<int> StartOperation(MobileAuthenticatedStream.OperationType type, AsyncProtocolRequest asyncRequest, CancellationToken cancellationToken)
		{
			this.CheckThrow(true, type > MobileAuthenticatedStream.OperationType.Read);
			if (type == MobileAuthenticatedStream.OperationType.Read)
			{
				if (Interlocked.CompareExchange<AsyncProtocolRequest>(ref this.asyncReadRequest, asyncRequest, null) != null)
				{
					throw MobileAuthenticatedStream.GetInvalidNestedCallException();
				}
			}
			else if (type == MobileAuthenticatedStream.OperationType.Renegotiate)
			{
				if (Interlocked.CompareExchange<AsyncProtocolRequest>(ref this.asyncHandshakeRequest, asyncRequest, null) != null)
				{
					throw MobileAuthenticatedStream.GetInvalidNestedCallException();
				}
				if (Interlocked.CompareExchange<AsyncProtocolRequest>(ref this.asyncReadRequest, asyncRequest, null) != null)
				{
					throw MobileAuthenticatedStream.GetInvalidNestedCallException();
				}
				if (Interlocked.CompareExchange<AsyncProtocolRequest>(ref this.asyncWriteRequest, asyncRequest, null) != null)
				{
					throw MobileAuthenticatedStream.GetInvalidNestedCallException();
				}
			}
			else if (Interlocked.CompareExchange<AsyncProtocolRequest>(ref this.asyncWriteRequest, asyncRequest, null) != null)
			{
				throw MobileAuthenticatedStream.GetInvalidNestedCallException();
			}
			AsyncProtocolResult asyncProtocolResult;
			try
			{
				object obj = this.ioLock;
				lock (obj)
				{
					if (type == MobileAuthenticatedStream.OperationType.Read)
					{
						this.readBuffer.Reset();
					}
					else
					{
						this.writeBuffer.Reset();
					}
				}
				asyncProtocolResult = await asyncRequest.StartOperation(cancellationToken).ConfigureAwait(false);
			}
			catch (Exception ex)
			{
				asyncProtocolResult = new AsyncProtocolResult(this.SetException(MobileAuthenticatedStream.GetIOException(ex, asyncRequest.Name + " failed")));
			}
			finally
			{
				object obj = this.ioLock;
				bool flag = false;
				try
				{
					Monitor.Enter(obj, ref flag);
					if (type == MobileAuthenticatedStream.OperationType.Read)
					{
						this.readBuffer.Reset();
						this.asyncReadRequest = null;
					}
					else if (type == MobileAuthenticatedStream.OperationType.Renegotiate)
					{
						this.readBuffer.Reset();
						this.writeBuffer.Reset();
						this.asyncHandshakeRequest = null;
						this.asyncReadRequest = null;
						this.asyncWriteRequest = null;
					}
					else
					{
						this.writeBuffer.Reset();
						this.asyncWriteRequest = null;
					}
				}
				finally
				{
					int num;
					if (num < 0 && flag)
					{
						Monitor.Exit(obj);
					}
				}
			}
			if (asyncProtocolResult.Error != null)
			{
				asyncProtocolResult.Error.Throw();
			}
			return asyncProtocolResult.UserResult;
		}

		// Token: 0x0600027F RID: 639 RVA: 0x00003917 File Offset: 0x00001B17
		[Conditional("MONO_TLS_DEBUG")]
		protected internal void Debug(string format, params object[] args)
		{
		}

		// Token: 0x06000280 RID: 640 RVA: 0x00003917 File Offset: 0x00001B17
		[Conditional("MONO_TLS_DEBUG")]
		protected internal void Debug(string message)
		{
		}

		// Token: 0x06000281 RID: 641 RVA: 0x0000777C File Offset: 0x0000597C
		internal int InternalRead(byte[] buffer, int offset, int size, out bool outWantMore)
		{
			int num;
			try
			{
				AsyncProtocolRequest asyncProtocolRequest = this.asyncHandshakeRequest ?? this.asyncReadRequest;
				ValueTuple<int, bool> valueTuple = this.InternalRead(asyncProtocolRequest, this.readBuffer, buffer, offset, size);
				int item = valueTuple.Item1;
				bool item2 = valueTuple.Item2;
				outWantMore = item2;
				num = item;
			}
			catch (Exception ex)
			{
				this.SetException(MobileAuthenticatedStream.GetIOException(ex, "InternalRead() failed"));
				outWantMore = false;
				num = -1;
			}
			return num;
		}

		// Token: 0x06000282 RID: 642 RVA: 0x000077F0 File Offset: 0x000059F0
		private ValueTuple<int, bool> InternalRead(AsyncProtocolRequest asyncRequest, BufferOffsetSize internalBuffer, byte[] buffer, int offset, int size)
		{
			if (asyncRequest == null)
			{
				throw new InvalidOperationException();
			}
			if (internalBuffer.Size == 0 && !internalBuffer.Complete)
			{
				internalBuffer.Offset = (internalBuffer.Size = 0);
				asyncRequest.RequestRead(size);
				return new ValueTuple<int, bool>(0, true);
			}
			int num = Math.Min(internalBuffer.Size, size);
			Buffer.BlockCopy(internalBuffer.Buffer, internalBuffer.Offset, buffer, offset, num);
			internalBuffer.Offset += num;
			internalBuffer.Size -= num;
			return new ValueTuple<int, bool>(num, !internalBuffer.Complete && num < size);
		}

		// Token: 0x06000283 RID: 643 RVA: 0x0000788C File Offset: 0x00005A8C
		internal bool InternalWrite(byte[] buffer, int offset, int size)
		{
			bool flag;
			try
			{
				AsyncProtocolRequest asyncProtocolRequest;
				switch (this.operation)
				{
				case MobileAuthenticatedStream.Operation.Handshake:
				case MobileAuthenticatedStream.Operation.Renegotiate:
					asyncProtocolRequest = this.asyncHandshakeRequest;
					goto IL_0057;
				case MobileAuthenticatedStream.Operation.Read:
					asyncProtocolRequest = this.asyncReadRequest;
					if (this.xobileTlsContext.PendingRenegotiation())
					{
						goto IL_0057;
					}
					goto IL_0057;
				case MobileAuthenticatedStream.Operation.Write:
				case MobileAuthenticatedStream.Operation.Close:
					asyncProtocolRequest = this.asyncWriteRequest;
					goto IL_0057;
				}
				throw MobileAuthenticatedStream.GetInternalError();
				IL_0057:
				if (asyncProtocolRequest == null && this.operation != MobileAuthenticatedStream.Operation.Close)
				{
					throw MobileAuthenticatedStream.GetInternalError();
				}
				flag = this.InternalWrite(asyncProtocolRequest, this.writeBuffer, buffer, offset, size);
			}
			catch (Exception ex)
			{
				this.SetException(MobileAuthenticatedStream.GetIOException(ex, "InternalWrite() failed"));
				flag = false;
			}
			return flag;
		}

		// Token: 0x06000284 RID: 644 RVA: 0x00007940 File Offset: 0x00005B40
		private bool InternalWrite(AsyncProtocolRequest asyncRequest, BufferOffsetSize2 internalBuffer, byte[] buffer, int offset, int size)
		{
			if (asyncRequest == null)
			{
				if (this.lastException != null)
				{
					return false;
				}
				if (Interlocked.Exchange(ref this.closeRequested, 1) == 0)
				{
					internalBuffer.Reset();
				}
				else if (internalBuffer.Remaining == 0)
				{
					throw new InvalidOperationException();
				}
			}
			internalBuffer.AppendData(buffer, offset, size);
			if (asyncRequest != null)
			{
				asyncRequest.RequestWrite();
			}
			return true;
		}

		// Token: 0x06000285 RID: 645 RVA: 0x00007994 File Offset: 0x00005B94
		internal async Task<int> InnerRead(bool sync, int requestedSize, CancellationToken cancellationToken)
		{
			cancellationToken.ThrowIfCancellationRequested();
			int len = Math.Min(this.readBuffer.Remaining, requestedSize);
			if (len == 0)
			{
				throw new InvalidOperationException();
			}
			Task<int> task;
			if (sync)
			{
				task = Task.Run<int>(() => this.InnerStream.Read(this.readBuffer.Buffer, this.readBuffer.EndOffset, len));
			}
			else
			{
				task = base.InnerStream.ReadAsync(this.readBuffer.Buffer, this.readBuffer.EndOffset, len, cancellationToken);
			}
			int num = await task.ConfigureAwait(false);
			if (num >= 0)
			{
				this.readBuffer.Size += num;
				this.readBuffer.TotalBytes += num;
			}
			if (num == 0)
			{
				this.readBuffer.Complete = true;
				if (this.readBuffer.TotalBytes > 0)
				{
					num = -1;
				}
			}
			return num;
		}

		// Token: 0x06000286 RID: 646 RVA: 0x000079F0 File Offset: 0x00005BF0
		internal async Task InnerWrite(bool sync, CancellationToken cancellationToken)
		{
			cancellationToken.ThrowIfCancellationRequested();
			if (this.writeBuffer.Size != 0)
			{
				Task task;
				if (sync)
				{
					task = Task.Run(delegate
					{
						base.InnerStream.Write(this.writeBuffer.Buffer, this.writeBuffer.Offset, this.writeBuffer.Size);
					});
				}
				else
				{
					task = base.InnerStream.WriteAsync(this.writeBuffer.Buffer, this.writeBuffer.Offset, this.writeBuffer.Size);
				}
				await task.ConfigureAwait(false);
				this.writeBuffer.TotalBytes += this.writeBuffer.Size;
				BufferOffsetSize bufferOffsetSize = this.writeBuffer;
				BufferOffsetSize bufferOffsetSize2 = this.writeBuffer;
				int num = 0;
				bufferOffsetSize2.Size = num;
				bufferOffsetSize.Offset = num;
			}
		}

		// Token: 0x06000287 RID: 647 RVA: 0x00007A44 File Offset: 0x00005C44
		internal AsyncOperationStatus ProcessHandshake(AsyncOperationStatus status, bool renegotiate)
		{
			object obj = this.ioLock;
			AsyncOperationStatus asyncOperationStatus;
			lock (obj)
			{
				switch (this.operation)
				{
				case MobileAuthenticatedStream.Operation.None:
					if (renegotiate)
					{
						throw MobileAuthenticatedStream.GetInternalError();
					}
					this.operation = MobileAuthenticatedStream.Operation.Handshake;
					break;
				case MobileAuthenticatedStream.Operation.Handshake:
				case MobileAuthenticatedStream.Operation.Renegotiate:
					break;
				case MobileAuthenticatedStream.Operation.Authenticated:
					if (!renegotiate)
					{
						throw MobileAuthenticatedStream.GetInternalError();
					}
					this.operation = MobileAuthenticatedStream.Operation.Renegotiate;
					break;
				default:
					throw MobileAuthenticatedStream.GetInternalError();
				}
				switch (status)
				{
				case AsyncOperationStatus.Initialize:
					if (renegotiate)
					{
						this.xobileTlsContext.Renegotiate();
					}
					else
					{
						this.xobileTlsContext.StartHandshake();
					}
					asyncOperationStatus = AsyncOperationStatus.Continue;
					break;
				case AsyncOperationStatus.Continue:
				{
					AsyncOperationStatus asyncOperationStatus2 = AsyncOperationStatus.Continue;
					try
					{
						if (this.xobileTlsContext.ProcessHandshake())
						{
							this.xobileTlsContext.FinishHandshake();
							this.operation = MobileAuthenticatedStream.Operation.Authenticated;
							asyncOperationStatus2 = AsyncOperationStatus.Complete;
						}
					}
					catch (Exception ex)
					{
						this.SetException(MobileAuthenticatedStream.GetSSPIException(ex));
						base.Dispose();
						throw;
					}
					if (this.lastException != null)
					{
						this.lastException.Throw();
					}
					asyncOperationStatus = asyncOperationStatus2;
					break;
				}
				case AsyncOperationStatus.ReadDone:
					throw new IOException("Authentication failed because the remote party has closed the transport stream.");
				default:
					throw new InvalidOperationException();
				}
			}
			return asyncOperationStatus;
		}

		// Token: 0x06000288 RID: 648 RVA: 0x00007B70 File Offset: 0x00005D70
		[return: TupleElementNames(new string[] { "ret", "wantMore" })]
		internal ValueTuple<int, bool> ProcessRead(BufferOffsetSize userBuffer)
		{
			object obj = this.ioLock;
			ValueTuple<int, bool> valueTuple2;
			lock (obj)
			{
				if (this.operation != MobileAuthenticatedStream.Operation.Authenticated)
				{
					throw MobileAuthenticatedStream.GetInternalError();
				}
				this.operation = MobileAuthenticatedStream.Operation.Read;
				ValueTuple<int, bool> valueTuple = this.xobileTlsContext.Read(userBuffer.Buffer, userBuffer.Offset, userBuffer.Size);
				if (this.lastException != null)
				{
					this.lastException.Throw();
				}
				this.operation = MobileAuthenticatedStream.Operation.Authenticated;
				valueTuple2 = valueTuple;
			}
			return valueTuple2;
		}

		// Token: 0x06000289 RID: 649 RVA: 0x00007BFC File Offset: 0x00005DFC
		[return: TupleElementNames(new string[] { "ret", "wantMore" })]
		internal ValueTuple<int, bool> ProcessWrite(BufferOffsetSize userBuffer)
		{
			object obj = this.ioLock;
			ValueTuple<int, bool> valueTuple2;
			lock (obj)
			{
				if (this.operation != MobileAuthenticatedStream.Operation.Authenticated)
				{
					throw MobileAuthenticatedStream.GetInternalError();
				}
				this.operation = MobileAuthenticatedStream.Operation.Write;
				ValueTuple<int, bool> valueTuple = this.xobileTlsContext.Write(userBuffer.Buffer, userBuffer.Offset, userBuffer.Size);
				if (this.lastException != null)
				{
					this.lastException.Throw();
				}
				this.operation = MobileAuthenticatedStream.Operation.Authenticated;
				valueTuple2 = valueTuple;
			}
			return valueTuple2;
		}

		// Token: 0x0600028A RID: 650 RVA: 0x00007C88 File Offset: 0x00005E88
		internal AsyncOperationStatus ProcessShutdown(AsyncOperationStatus status)
		{
			object obj = this.ioLock;
			AsyncOperationStatus asyncOperationStatus;
			lock (obj)
			{
				if (this.operation != MobileAuthenticatedStream.Operation.Authenticated)
				{
					throw MobileAuthenticatedStream.GetInternalError();
				}
				this.operation = MobileAuthenticatedStream.Operation.Close;
				this.xobileTlsContext.Shutdown();
				this.shutdown = true;
				this.operation = MobileAuthenticatedStream.Operation.Authenticated;
				asyncOperationStatus = AsyncOperationStatus.Complete;
			}
			return asyncOperationStatus;
		}

		// Token: 0x17000061 RID: 97
		// (get) Token: 0x0600028B RID: 651 RVA: 0x00007CF4 File Offset: 0x00005EF4
		public override bool IsServer
		{
			get
			{
				this.CheckThrow(false, false);
				return this.xobileTlsContext != null && this.xobileTlsContext.IsServer;
			}
		}

		// Token: 0x17000062 RID: 98
		// (get) Token: 0x0600028C RID: 652 RVA: 0x00007D14 File Offset: 0x00005F14
		public override bool IsAuthenticated
		{
			get
			{
				object obj = this.ioLock;
				bool flag2;
				lock (obj)
				{
					flag2 = this.xobileTlsContext != null && this.lastException == null && this.xobileTlsContext.IsAuthenticated;
				}
				return flag2;
			}
		}

		// Token: 0x17000063 RID: 99
		// (get) Token: 0x0600028D RID: 653 RVA: 0x00007D70 File Offset: 0x00005F70
		public override bool IsMutuallyAuthenticated
		{
			get
			{
				object obj = this.ioLock;
				bool flag2;
				lock (obj)
				{
					if (!this.IsAuthenticated)
					{
						flag2 = false;
					}
					else if ((this.xobileTlsContext.IsServer ? this.xobileTlsContext.LocalServerCertificate : this.xobileTlsContext.LocalClientCertificate) == null)
					{
						flag2 = false;
					}
					else
					{
						flag2 = this.xobileTlsContext.IsRemoteCertificateAvailable;
					}
				}
				return flag2;
			}
		}

		// Token: 0x0600028E RID: 654 RVA: 0x00007DF0 File Offset: 0x00005FF0
		protected override void Dispose(bool disposing)
		{
			try
			{
				object obj = this.ioLock;
				lock (obj)
				{
					this.SetException(new ObjectDisposedException("MobileAuthenticatedStream"));
					if (this.xobileTlsContext != null)
					{
						this.xobileTlsContext.Dispose();
						this.xobileTlsContext = null;
					}
				}
			}
			finally
			{
				base.Dispose(disposing);
			}
		}

		// Token: 0x0600028F RID: 655 RVA: 0x00007E6C File Offset: 0x0000606C
		public override void Flush()
		{
			base.InnerStream.Flush();
		}

		// Token: 0x17000064 RID: 100
		// (get) Token: 0x06000290 RID: 656 RVA: 0x00007E7C File Offset: 0x0000607C
		public SslProtocols SslProtocol
		{
			get
			{
				object obj = this.ioLock;
				SslProtocols negotiatedProtocol;
				lock (obj)
				{
					this.CheckThrow(true, false);
					negotiatedProtocol = (SslProtocols)this.xobileTlsContext.NegotiatedProtocol;
				}
				return negotiatedProtocol;
			}
		}

		// Token: 0x17000065 RID: 101
		// (get) Token: 0x06000291 RID: 657 RVA: 0x00007ECC File Offset: 0x000060CC
		public X509Certificate RemoteCertificate
		{
			get
			{
				object obj = this.ioLock;
				X509Certificate remoteCertificate;
				lock (obj)
				{
					this.CheckThrow(true, false);
					remoteCertificate = this.xobileTlsContext.RemoteCertificate;
				}
				return remoteCertificate;
			}
		}

		// Token: 0x17000066 RID: 102
		// (get) Token: 0x06000292 RID: 658 RVA: 0x00007F1C File Offset: 0x0000611C
		public X509Certificate LocalCertificate
		{
			get
			{
				object obj = this.ioLock;
				X509Certificate internalLocalCertificate;
				lock (obj)
				{
					this.CheckThrow(true, false);
					internalLocalCertificate = this.InternalLocalCertificate;
				}
				return internalLocalCertificate;
			}
		}

		// Token: 0x17000067 RID: 103
		// (get) Token: 0x06000293 RID: 659 RVA: 0x00007F68 File Offset: 0x00006168
		public X509Certificate InternalLocalCertificate
		{
			get
			{
				object obj = this.ioLock;
				X509Certificate x509Certificate;
				lock (obj)
				{
					this.CheckThrow(false, false);
					if (this.xobileTlsContext == null)
					{
						x509Certificate = null;
					}
					else
					{
						x509Certificate = (this.xobileTlsContext.IsServer ? this.xobileTlsContext.LocalServerCertificate : this.xobileTlsContext.LocalClientCertificate);
					}
				}
				return x509Certificate;
			}
		}

		// Token: 0x06000294 RID: 660 RVA: 0x00007FE0 File Offset: 0x000061E0
		public MonoTlsConnectionInfo GetConnectionInfo()
		{
			object obj = this.ioLock;
			MonoTlsConnectionInfo connectionInfo;
			lock (obj)
			{
				this.CheckThrow(true, false);
				connectionInfo = this.xobileTlsContext.ConnectionInfo;
			}
			return connectionInfo;
		}

		// Token: 0x06000295 RID: 661 RVA: 0x000044FA File Offset: 0x000026FA
		public override long Seek(long offset, SeekOrigin origin)
		{
			throw new NotSupportedException();
		}

		// Token: 0x06000296 RID: 662 RVA: 0x00008030 File Offset: 0x00006230
		public override void SetLength(long value)
		{
			base.InnerStream.SetLength(value);
		}

		// Token: 0x17000068 RID: 104
		// (get) Token: 0x06000297 RID: 663 RVA: 0x000044FA File Offset: 0x000026FA
		public TransportContext TransportContext
		{
			get
			{
				throw new NotSupportedException();
			}
		}

		// Token: 0x17000069 RID: 105
		// (get) Token: 0x06000298 RID: 664 RVA: 0x0000803E File Offset: 0x0000623E
		public override bool CanRead
		{
			get
			{
				return this.IsAuthenticated && base.InnerStream.CanRead;
			}
		}

		// Token: 0x1700006A RID: 106
		// (get) Token: 0x06000299 RID: 665 RVA: 0x00008055 File Offset: 0x00006255
		public override bool CanTimeout
		{
			get
			{
				return base.InnerStream.CanTimeout;
			}
		}

		// Token: 0x1700006B RID: 107
		// (get) Token: 0x0600029A RID: 666 RVA: 0x00008062 File Offset: 0x00006262
		public override bool CanWrite
		{
			get
			{
				return (this.IsAuthenticated & base.InnerStream.CanWrite) && !this.shutdown;
			}
		}

		// Token: 0x1700006C RID: 108
		// (get) Token: 0x0600029B RID: 667 RVA: 0x00003062 File Offset: 0x00001262
		public override bool CanSeek
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700006D RID: 109
		// (get) Token: 0x0600029C RID: 668 RVA: 0x00008083 File Offset: 0x00006283
		public override long Length
		{
			get
			{
				return base.InnerStream.Length;
			}
		}

		// Token: 0x1700006E RID: 110
		// (get) Token: 0x0600029D RID: 669 RVA: 0x00008090 File Offset: 0x00006290
		// (set) Token: 0x0600029E RID: 670 RVA: 0x000044FA File Offset: 0x000026FA
		public override long Position
		{
			get
			{
				return base.InnerStream.Position;
			}
			set
			{
				throw new NotSupportedException();
			}
		}

		// Token: 0x1700006F RID: 111
		// (get) Token: 0x0600029F RID: 671 RVA: 0x0000809D File Offset: 0x0000629D
		public override bool IsEncrypted
		{
			get
			{
				return this.IsAuthenticated;
			}
		}

		// Token: 0x17000070 RID: 112
		// (get) Token: 0x060002A0 RID: 672 RVA: 0x0000809D File Offset: 0x0000629D
		public override bool IsSigned
		{
			get
			{
				return this.IsAuthenticated;
			}
		}

		// Token: 0x17000071 RID: 113
		// (get) Token: 0x060002A1 RID: 673 RVA: 0x000080A5 File Offset: 0x000062A5
		// (set) Token: 0x060002A2 RID: 674 RVA: 0x000080B2 File Offset: 0x000062B2
		public override int ReadTimeout
		{
			get
			{
				return base.InnerStream.ReadTimeout;
			}
			set
			{
				base.InnerStream.ReadTimeout = value;
			}
		}

		// Token: 0x17000072 RID: 114
		// (get) Token: 0x060002A3 RID: 675 RVA: 0x000080C0 File Offset: 0x000062C0
		// (set) Token: 0x060002A4 RID: 676 RVA: 0x000080CD File Offset: 0x000062CD
		public override int WriteTimeout
		{
			get
			{
				return base.InnerStream.WriteTimeout;
			}
			set
			{
				base.InnerStream.WriteTimeout = value;
			}
		}

		// Token: 0x17000073 RID: 115
		// (get) Token: 0x060002A5 RID: 677 RVA: 0x000080DC File Offset: 0x000062DC
		public global::System.Security.Authentication.CipherAlgorithmType CipherAlgorithm
		{
			get
			{
				this.CheckThrow(true, false);
				MonoTlsConnectionInfo connectionInfo = this.GetConnectionInfo();
				if (connectionInfo == null)
				{
					return global::System.Security.Authentication.CipherAlgorithmType.None;
				}
				switch (connectionInfo.CipherAlgorithmType)
				{
				case Mono.Security.Interface.CipherAlgorithmType.Aes128:
				case Mono.Security.Interface.CipherAlgorithmType.AesGcm128:
					return global::System.Security.Authentication.CipherAlgorithmType.Aes128;
				case Mono.Security.Interface.CipherAlgorithmType.Aes256:
				case Mono.Security.Interface.CipherAlgorithmType.AesGcm256:
					return global::System.Security.Authentication.CipherAlgorithmType.Aes256;
				default:
					return global::System.Security.Authentication.CipherAlgorithmType.None;
				}
			}
		}

		// Token: 0x17000074 RID: 116
		// (get) Token: 0x060002A6 RID: 678 RVA: 0x0000812C File Offset: 0x0000632C
		public global::System.Security.Authentication.HashAlgorithmType HashAlgorithm
		{
			get
			{
				this.CheckThrow(true, false);
				MonoTlsConnectionInfo connectionInfo = this.GetConnectionInfo();
				if (connectionInfo == null)
				{
					return global::System.Security.Authentication.HashAlgorithmType.None;
				}
				Mono.Security.Interface.HashAlgorithmType hashAlgorithmType = connectionInfo.HashAlgorithmType;
				if (hashAlgorithmType != Mono.Security.Interface.HashAlgorithmType.Md5)
				{
					if (hashAlgorithmType - Mono.Security.Interface.HashAlgorithmType.Sha1 <= 4)
					{
						return global::System.Security.Authentication.HashAlgorithmType.Sha1;
					}
					if (hashAlgorithmType != Mono.Security.Interface.HashAlgorithmType.Md5Sha1)
					{
						return global::System.Security.Authentication.HashAlgorithmType.None;
					}
				}
				return global::System.Security.Authentication.HashAlgorithmType.Md5;
			}
		}

		// Token: 0x17000075 RID: 117
		// (get) Token: 0x060002A7 RID: 679 RVA: 0x00008174 File Offset: 0x00006374
		public global::System.Security.Authentication.ExchangeAlgorithmType KeyExchangeAlgorithm
		{
			get
			{
				this.CheckThrow(true, false);
				MonoTlsConnectionInfo connectionInfo = this.GetConnectionInfo();
				if (connectionInfo == null)
				{
					return global::System.Security.Authentication.ExchangeAlgorithmType.None;
				}
				switch (connectionInfo.ExchangeAlgorithmType)
				{
				case Mono.Security.Interface.ExchangeAlgorithmType.Dhe:
				case Mono.Security.Interface.ExchangeAlgorithmType.EcDhe:
					return global::System.Security.Authentication.ExchangeAlgorithmType.DiffieHellman;
				case Mono.Security.Interface.ExchangeAlgorithmType.Rsa:
					return global::System.Security.Authentication.ExchangeAlgorithmType.RsaSign;
				default:
					return global::System.Security.Authentication.ExchangeAlgorithmType.None;
				}
			}
		}

		// Token: 0x17000076 RID: 118
		// (get) Token: 0x060002A8 RID: 680 RVA: 0x000081C0 File Offset: 0x000063C0
		public int CipherStrength
		{
			get
			{
				this.CheckThrow(true, false);
				MonoTlsConnectionInfo connectionInfo = this.GetConnectionInfo();
				if (connectionInfo == null)
				{
					return 0;
				}
				switch (connectionInfo.CipherAlgorithmType)
				{
				case Mono.Security.Interface.CipherAlgorithmType.None:
				case Mono.Security.Interface.CipherAlgorithmType.Aes128:
				case Mono.Security.Interface.CipherAlgorithmType.AesGcm128:
					return 128;
				case Mono.Security.Interface.CipherAlgorithmType.Aes256:
				case Mono.Security.Interface.CipherAlgorithmType.AesGcm256:
					return 256;
				default:
					throw new ArgumentOutOfRangeException("CipherAlgorithmType");
				}
			}
		}

		// Token: 0x17000077 RID: 119
		// (get) Token: 0x060002A9 RID: 681 RVA: 0x0000821C File Offset: 0x0000641C
		public int HashStrength
		{
			get
			{
				this.CheckThrow(true, false);
				MonoTlsConnectionInfo connectionInfo = this.GetConnectionInfo();
				if (connectionInfo == null)
				{
					return 0;
				}
				Mono.Security.Interface.HashAlgorithmType hashAlgorithmType = connectionInfo.HashAlgorithmType;
				switch (hashAlgorithmType)
				{
				case Mono.Security.Interface.HashAlgorithmType.Md5:
					break;
				case Mono.Security.Interface.HashAlgorithmType.Sha1:
					return 160;
				case Mono.Security.Interface.HashAlgorithmType.Sha224:
					return 224;
				case Mono.Security.Interface.HashAlgorithmType.Sha256:
					return 256;
				case Mono.Security.Interface.HashAlgorithmType.Sha384:
					return 384;
				case Mono.Security.Interface.HashAlgorithmType.Sha512:
					return 512;
				default:
					if (hashAlgorithmType != Mono.Security.Interface.HashAlgorithmType.Md5Sha1)
					{
						throw new ArgumentOutOfRangeException("HashAlgorithmType");
					}
					break;
				}
				return 128;
			}
		}

		// Token: 0x17000078 RID: 120
		// (get) Token: 0x060002AA RID: 682 RVA: 0x00003062 File Offset: 0x00001262
		public int KeyExchangeStrength
		{
			get
			{
				return 0;
			}
		}

		// Token: 0x17000079 RID: 121
		// (get) Token: 0x060002AB RID: 683 RVA: 0x0000829A File Offset: 0x0000649A
		public bool CheckCertRevocationStatus
		{
			get
			{
				throw new NotImplementedException();
			}
		}

		// Token: 0x0400022E RID: 558
		private MobileTlsContext xobileTlsContext;

		// Token: 0x0400022F RID: 559
		private ExceptionDispatchInfo lastException;

		// Token: 0x04000230 RID: 560
		private AsyncProtocolRequest asyncHandshakeRequest;

		// Token: 0x04000231 RID: 561
		private AsyncProtocolRequest asyncReadRequest;

		// Token: 0x04000232 RID: 562
		private AsyncProtocolRequest asyncWriteRequest;

		// Token: 0x04000233 RID: 563
		private BufferOffsetSize2 readBuffer;

		// Token: 0x04000234 RID: 564
		private BufferOffsetSize2 writeBuffer;

		// Token: 0x04000235 RID: 565
		private object ioLock = new object();

		// Token: 0x04000236 RID: 566
		private int closeRequested;

		// Token: 0x04000237 RID: 567
		private bool shutdown;

		// Token: 0x04000238 RID: 568
		private MobileAuthenticatedStream.Operation operation;

		// Token: 0x04000239 RID: 569
		private static int uniqueNameInteger = 123;

		// Token: 0x0400023E RID: 574
		private static int nextId;

		// Token: 0x0400023F RID: 575
		internal readonly int ID = ++MobileAuthenticatedStream.nextId;

		// Token: 0x02000099 RID: 153
		private enum Operation
		{
			// Token: 0x04000241 RID: 577
			None,
			// Token: 0x04000242 RID: 578
			Handshake,
			// Token: 0x04000243 RID: 579
			Authenticated,
			// Token: 0x04000244 RID: 580
			Renegotiate,
			// Token: 0x04000245 RID: 581
			Read,
			// Token: 0x04000246 RID: 582
			Write,
			// Token: 0x04000247 RID: 583
			Close
		}

		// Token: 0x0200009A RID: 154
		private enum OperationType
		{
			// Token: 0x04000249 RID: 585
			Read,
			// Token: 0x0400024A RID: 586
			Write,
			// Token: 0x0400024B RID: 587
			Renegotiate,
			// Token: 0x0400024C RID: 588
			Shutdown
		}
	}
}
