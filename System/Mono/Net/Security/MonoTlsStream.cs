using System;
using System.IO;
using System.Net;
using System.Net.Security;
using System.Net.Sockets;
using System.Security.Authentication;
using System.Threading;
using System.Threading.Tasks;
using Mono.Net.Security.Private;
using Mono.Security.Interface;

namespace Mono.Net.Security
{
	// Token: 0x020000A6 RID: 166
	internal class MonoTlsStream : IDisposable
	{
		// Token: 0x170000B3 RID: 179
		// (get) Token: 0x0600033F RID: 831 RVA: 0x000099A0 File Offset: 0x00007BA0
		internal HttpWebRequest Request
		{
			get
			{
				return this.request;
			}
		}

		// Token: 0x170000B4 RID: 180
		// (get) Token: 0x06000340 RID: 832 RVA: 0x000099A8 File Offset: 0x00007BA8
		internal SslStream SslStream
		{
			get
			{
				return this.sslStream;
			}
		}

		// Token: 0x170000B5 RID: 181
		// (get) Token: 0x06000341 RID: 833 RVA: 0x000099B0 File Offset: 0x00007BB0
		internal WebExceptionStatus ExceptionStatus
		{
			get
			{
				return this.status;
			}
		}

		// Token: 0x170000B6 RID: 182
		// (get) Token: 0x06000342 RID: 834 RVA: 0x000099B8 File Offset: 0x00007BB8
		// (set) Token: 0x06000343 RID: 835 RVA: 0x000099C0 File Offset: 0x00007BC0
		internal bool CertificateValidationFailed { get; set; }

		// Token: 0x06000344 RID: 836 RVA: 0x000099CC File Offset: 0x00007BCC
		public MonoTlsStream(HttpWebRequest request, NetworkStream networkStream)
		{
			this.request = request;
			this.networkStream = networkStream;
			this.settings = request.TlsSettings;
			if (this.settings == null)
			{
				this.settings = MonoTlsSettings.CopyDefaultSettings();
			}
			if (this.settings.RemoteCertificateValidationCallback == null)
			{
				this.settings.RemoteCertificateValidationCallback = CallbackHelpers.PublicToMono(request.ServerCertificateValidationCallback);
			}
			this.provider = request.TlsProvider ?? MonoTlsProviderFactory.GetProviderInternal();
			this.status = WebExceptionStatus.SecureChannelFailure;
			ChainValidationHelper.Create(this.provider, ref this.settings, this);
		}

		// Token: 0x06000345 RID: 837 RVA: 0x00009A6C File Offset: 0x00007C6C
		internal async Task<Stream> CreateStream(WebConnectionTunnel tunnel, CancellationToken cancellationToken)
		{
			Socket socket = this.networkStream.InternalSocket;
			this.sslStream = new SslStream(this.networkStream, false, this.provider, this.settings);
			try
			{
				string text = this.request.Host;
				if (!string.IsNullOrEmpty(text))
				{
					int num = text.IndexOf(':');
					if (num > 0)
					{
						text = text.Substring(0, num);
					}
				}
				await this.sslStream.AuthenticateAsClientAsync(text, this.request.ClientCertificates, (SslProtocols)ServicePointManager.SecurityProtocol, ServicePointManager.CheckCertificateRevocationList).ConfigureAwait(false);
				this.status = WebExceptionStatus.Success;
				this.request.ServicePoint.UpdateClientCertificate(this.sslStream.LocalCertificate);
			}
			catch (Exception)
			{
				if (socket.CleanedUp)
				{
					this.status = WebExceptionStatus.RequestCanceled;
				}
				else if (this.CertificateValidationFailed)
				{
					this.status = WebExceptionStatus.TrustFailure;
				}
				else
				{
					this.status = WebExceptionStatus.SecureChannelFailure;
				}
				this.request.ServicePoint.UpdateClientCertificate(null);
				this.CloseSslStream();
				throw;
			}
			try
			{
				if (((tunnel != null) ? tunnel.Data : null) != null)
				{
					await this.sslStream.WriteAsync(tunnel.Data, 0, tunnel.Data.Length, cancellationToken).ConfigureAwait(false);
				}
			}
			catch
			{
				this.status = WebExceptionStatus.SendFailure;
				this.CloseSslStream();
				throw;
			}
			return this.sslStream;
		}

		// Token: 0x06000346 RID: 838 RVA: 0x00009ABF File Offset: 0x00007CBF
		public void Dispose()
		{
			this.CloseSslStream();
		}

		// Token: 0x06000347 RID: 839 RVA: 0x00009AC8 File Offset: 0x00007CC8
		private void CloseSslStream()
		{
			object obj = this.sslStreamLock;
			lock (obj)
			{
				if (this.sslStream != null)
				{
					this.sslStream.Dispose();
					this.sslStream = null;
				}
			}
		}

		// Token: 0x04000280 RID: 640
		private readonly MobileTlsProvider provider;

		// Token: 0x04000281 RID: 641
		private readonly NetworkStream networkStream;

		// Token: 0x04000282 RID: 642
		private readonly HttpWebRequest request;

		// Token: 0x04000283 RID: 643
		private readonly MonoTlsSettings settings;

		// Token: 0x04000284 RID: 644
		private SslStream sslStream;

		// Token: 0x04000285 RID: 645
		private readonly object sslStreamLock = new object();

		// Token: 0x04000286 RID: 646
		private WebExceptionStatus status;
	}
}
