using System;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using Mono.Net.Security.Private;
using Mono.Security.Interface;

namespace Mono.Net.Security
{
	// Token: 0x02000096 RID: 150
	internal class ChainValidationHelper : ICertificateValidator
	{
		// Token: 0x0600024B RID: 587 RVA: 0x00006C5C File Offset: 0x00004E5C
		internal static ChainValidationHelper GetInternalValidator(SslStream owner, MobileTlsProvider provider, MonoTlsSettings settings)
		{
			if (settings == null)
			{
				return new ChainValidationHelper(owner, provider, null, false, null);
			}
			if (settings.CertificateValidator != null)
			{
				return (ChainValidationHelper)settings.CertificateValidator;
			}
			return new ChainValidationHelper(owner, provider, settings, false, null);
		}

		// Token: 0x0600024C RID: 588 RVA: 0x00006C8C File Offset: 0x00004E8C
		internal static ICertificateValidator GetDefaultValidator(MonoTlsSettings settings)
		{
			MobileTlsProvider providerInternal = MonoTlsProviderFactory.GetProviderInternal();
			if (settings == null)
			{
				return new ChainValidationHelper(null, providerInternal, null, false, null);
			}
			if (settings.CertificateValidator != null)
			{
				throw new NotSupportedException();
			}
			return new ChainValidationHelper(null, providerInternal, settings, false, null);
		}

		// Token: 0x0600024D RID: 589 RVA: 0x00006CC8 File Offset: 0x00004EC8
		internal static ChainValidationHelper Create(MobileTlsProvider provider, ref MonoTlsSettings settings, MonoTlsStream stream)
		{
			ChainValidationHelper chainValidationHelper = new ChainValidationHelper(null, provider, settings, true, stream);
			settings = chainValidationHelper.settings;
			return chainValidationHelper;
		}

		// Token: 0x0600024E RID: 590 RVA: 0x00006CEC File Offset: 0x00004EEC
		private ChainValidationHelper(SslStream owner, MobileTlsProvider provider, MonoTlsSettings settings, bool cloneSettings, MonoTlsStream stream)
		{
			if (settings == null)
			{
				settings = MonoTlsSettings.CopyDefaultSettings();
			}
			if (cloneSettings)
			{
				settings = settings.CloneWithValidator(this);
			}
			if (provider == null)
			{
				provider = MonoTlsProviderFactory.GetProviderInternal();
			}
			this.provider = provider;
			this.settings = settings;
			this.tlsStream = stream;
			if (owner != null)
			{
				this.owner = new WeakReference<SslStream>(owner);
			}
			bool flag = false;
			if (settings != null)
			{
				this.certValidationCallback = ChainValidationHelper.GetValidationCallback(settings);
				this.certSelectionCallback = CallbackHelpers.MonoToInternal(settings.ClientCertificateSelectionCallback);
				flag = settings.UseServicePointManagerCallback ?? (stream != null);
			}
			if (stream != null)
			{
				this.request = stream.Request;
				if (this.certValidationCallback == null)
				{
					this.certValidationCallback = this.request.ServerCertValidationCallback;
				}
				if (this.certSelectionCallback == null)
				{
					this.certSelectionCallback = new LocalCertSelectionCallback(ChainValidationHelper.DefaultSelectionCallback);
				}
				if (settings == null)
				{
					flag = true;
				}
			}
			if (flag && this.certValidationCallback == null)
			{
				this.certValidationCallback = ServicePointManager.ServerCertValidationCallback;
			}
		}

		// Token: 0x0600024F RID: 591 RVA: 0x00006DE8 File Offset: 0x00004FE8
		private static ServerCertValidationCallback GetValidationCallback(MonoTlsSettings settings)
		{
			if (settings.RemoteCertificateValidationCallback == null)
			{
				return null;
			}
			return new ServerCertValidationCallback(delegate(object s, X509Certificate c, X509Chain ch, SslPolicyErrors e)
			{
				string text = null;
				SslStream sslStream = s as SslStream;
				if (sslStream != null)
				{
					text = sslStream.InternalTargetHost;
				}
				else
				{
					HttpWebRequest httpWebRequest = s as HttpWebRequest;
					if (httpWebRequest != null)
					{
						text = httpWebRequest.Host;
						if (!string.IsNullOrEmpty(text))
						{
							int num = text.IndexOf(':');
							if (num > 0)
							{
								text = text.Substring(0, num);
							}
						}
					}
				}
				return settings.RemoteCertificateValidationCallback(text, c, ch, (MonoSslPolicyErrors)e);
			});
		}

		// Token: 0x06000250 RID: 592 RVA: 0x00006E24 File Offset: 0x00005024
		private static X509Certificate DefaultSelectionCallback(string targetHost, X509CertificateCollection localCertificates, X509Certificate remoteCertificate, string[] acceptableIssuers)
		{
			X509Certificate x509Certificate;
			if (localCertificates == null || localCertificates.Count == 0)
			{
				x509Certificate = null;
			}
			else
			{
				x509Certificate = localCertificates[0];
			}
			return x509Certificate;
		}

		// Token: 0x17000056 RID: 86
		// (get) Token: 0x06000251 RID: 593 RVA: 0x00006E49 File Offset: 0x00005049
		public MonoTlsProvider Provider
		{
			get
			{
				return this.provider;
			}
		}

		// Token: 0x17000057 RID: 87
		// (get) Token: 0x06000252 RID: 594 RVA: 0x00006E51 File Offset: 0x00005051
		public MonoTlsSettings Settings
		{
			get
			{
				return this.settings;
			}
		}

		// Token: 0x17000058 RID: 88
		// (get) Token: 0x06000253 RID: 595 RVA: 0x00006E59 File Offset: 0x00005059
		public bool HasCertificateSelectionCallback
		{
			get
			{
				return this.certSelectionCallback != null;
			}
		}

		// Token: 0x06000254 RID: 596 RVA: 0x00006E64 File Offset: 0x00005064
		public bool SelectClientCertificate(string targetHost, X509CertificateCollection localCertificates, X509Certificate remoteCertificate, string[] acceptableIssuers, out X509Certificate clientCertificate)
		{
			if (this.certSelectionCallback == null)
			{
				clientCertificate = null;
				return false;
			}
			clientCertificate = this.certSelectionCallback(targetHost, localCertificates, remoteCertificate, acceptableIssuers);
			return true;
		}

		// Token: 0x06000255 RID: 597 RVA: 0x00006E88 File Offset: 0x00005088
		internal X509Certificate SelectClientCertificate(string targetHost, X509CertificateCollection localCertificates, X509Certificate remoteCertificate, string[] acceptableIssuers)
		{
			if (this.certSelectionCallback == null)
			{
				return null;
			}
			return this.certSelectionCallback(targetHost, localCertificates, remoteCertificate, acceptableIssuers);
		}

		// Token: 0x06000256 RID: 598 RVA: 0x00006EA4 File Offset: 0x000050A4
		internal bool ValidateClientCertificate(X509Certificate certificate, MonoSslPolicyErrors errors)
		{
			X509CertificateCollection x509CertificateCollection = new X509CertificateCollection();
			x509CertificateCollection.Add(new X509Certificate2(certificate.GetRawCertData()));
			ValidationResult validationResult = this.ValidateChain(string.Empty, true, certificate, null, x509CertificateCollection, (SslPolicyErrors)errors);
			return validationResult != null && validationResult.Trusted && !validationResult.UserDenied;
		}

		// Token: 0x06000257 RID: 599 RVA: 0x00006EF4 File Offset: 0x000050F4
		public ValidationResult ValidateCertificate(string host, bool serverMode, X509CertificateCollection certs)
		{
			ValidationResult validationResult2;
			try
			{
				X509Certificate x509Certificate;
				if (certs != null && certs.Count != 0)
				{
					x509Certificate = certs[0];
				}
				else
				{
					x509Certificate = null;
				}
				ValidationResult validationResult = this.ValidateChain(host, serverMode, x509Certificate, null, certs, SslPolicyErrors.None);
				if (this.tlsStream != null)
				{
					this.tlsStream.CertificateValidationFailed = validationResult == null || !validationResult.Trusted || validationResult.UserDenied;
				}
				validationResult2 = validationResult;
			}
			catch
			{
				if (this.tlsStream != null)
				{
					this.tlsStream.CertificateValidationFailed = true;
				}
				throw;
			}
			return validationResult2;
		}

		// Token: 0x06000258 RID: 600 RVA: 0x00006F78 File Offset: 0x00005178
		public ValidationResult ValidateCertificate(string host, bool serverMode, X509Certificate leaf, X509Chain chain)
		{
			ValidationResult validationResult2;
			try
			{
				ValidationResult validationResult = this.ValidateChain(host, serverMode, leaf, chain, null, SslPolicyErrors.None);
				if (this.tlsStream != null)
				{
					this.tlsStream.CertificateValidationFailed = validationResult == null || !validationResult.Trusted || validationResult.UserDenied;
				}
				validationResult2 = validationResult;
			}
			catch
			{
				if (this.tlsStream != null)
				{
					this.tlsStream.CertificateValidationFailed = true;
				}
				throw;
			}
			return validationResult2;
		}

		// Token: 0x06000259 RID: 601 RVA: 0x00006FE8 File Offset: 0x000051E8
		private ValidationResult ValidateChain(string host, bool server, X509Certificate leaf, X509Chain chain, X509CertificateCollection certs, SslPolicyErrors errors)
		{
			X509Chain x509Chain = chain;
			bool flag = chain == null;
			ValidationResult validationResult2;
			try
			{
				ValidationResult validationResult = this.ValidateChain(host, server, leaf, ref chain, certs, errors);
				if (chain != x509Chain)
				{
					flag = true;
				}
				validationResult2 = validationResult;
			}
			finally
			{
				if (flag && chain != null)
				{
					chain.Dispose();
				}
			}
			return validationResult2;
		}

		// Token: 0x0600025A RID: 602 RVA: 0x00007038 File Offset: 0x00005238
		private ValidationResult ValidateChain(string host, bool server, X509Certificate leaf, ref X509Chain chain, X509CertificateCollection certs, SslPolicyErrors errors)
		{
			bool flag = false;
			bool flag2 = false;
			if (this.tlsStream != null)
			{
				this.request.ServicePoint.UpdateServerCertificate(leaf);
			}
			if (leaf == null)
			{
				errors |= SslPolicyErrors.RemoteCertificateNotAvailable;
				if (this.certValidationCallback != null)
				{
					flag2 = this.InvokeCallback(leaf, null, errors);
					flag = !flag2;
				}
				return new ValidationResult(flag2, flag, 0, new MonoSslPolicyErrors?((MonoSslPolicyErrors)errors));
			}
			if (!string.IsNullOrEmpty(host))
			{
				int num = host.IndexOf(':');
				if (num > 0)
				{
					host = host.Substring(0, num);
				}
			}
			ICertificatePolicy legacyCertificatePolicy = ServicePointManager.GetLegacyCertificatePolicy();
			int num2 = 0;
			bool flag3 = SystemCertificateValidator.NeedsChain(this.settings);
			if (!flag3 && this.certValidationCallback != null && (this.settings == null || this.settings.CallbackNeedsCertificateChain))
			{
				flag3 = true;
			}
			flag2 = this.provider.ValidateCertificate(this, host, server, certs, flag3, ref chain, ref errors, ref num2);
			if (num2 == 0 && errors != SslPolicyErrors.None)
			{
				num2 = -2146762485;
			}
			if (legacyCertificatePolicy != null && (!(legacyCertificatePolicy is DefaultCertificatePolicy) || this.certValidationCallback == null))
			{
				ServicePoint servicePoint = null;
				if (this.request != null)
				{
					servicePoint = this.request.ServicePointNoLock;
				}
				flag2 = legacyCertificatePolicy.CheckValidationResult(servicePoint, leaf, this.request, num2);
				flag = !flag2 && !(legacyCertificatePolicy is DefaultCertificatePolicy);
			}
			if (this.certValidationCallback != null)
			{
				flag2 = this.InvokeCallback(leaf, chain, errors);
				flag = !flag2;
			}
			return new ValidationResult(flag2, flag, num2, new MonoSslPolicyErrors?((MonoSslPolicyErrors)errors));
		}

		// Token: 0x0600025B RID: 603 RVA: 0x0000718C File Offset: 0x0000538C
		private bool InvokeCallback(X509Certificate leaf, X509Chain chain, SslPolicyErrors errors)
		{
			object obj = null;
			SslStream sslStream;
			if (this.request != null)
			{
				obj = this.request;
			}
			else if (this.owner != null && this.owner.TryGetTarget(out sslStream))
			{
				obj = sslStream;
			}
			return this.certValidationCallback.Invoke(obj, leaf, chain, errors);
		}

		// Token: 0x0600025C RID: 604 RVA: 0x000071D4 File Offset: 0x000053D4
		private bool InvokeSystemValidator(string targetHost, bool serverMode, X509CertificateCollection certificates, X509Chain chain, ref MonoSslPolicyErrors xerrors, ref int status11)
		{
			SslPolicyErrors sslPolicyErrors = (SslPolicyErrors)xerrors;
			bool flag = SystemCertificateValidator.Evaluate(this.settings, targetHost, certificates, chain, ref sslPolicyErrors, ref status11);
			xerrors = (MonoSslPolicyErrors)sslPolicyErrors;
			return flag;
		}

		// Token: 0x04000226 RID: 550
		private readonly WeakReference<SslStream> owner;

		// Token: 0x04000227 RID: 551
		private readonly MonoTlsSettings settings;

		// Token: 0x04000228 RID: 552
		private readonly MobileTlsProvider provider;

		// Token: 0x04000229 RID: 553
		private readonly ServerCertValidationCallback certValidationCallback;

		// Token: 0x0400022A RID: 554
		private readonly LocalCertSelectionCallback certSelectionCallback;

		// Token: 0x0400022B RID: 555
		private readonly MonoTlsStream tlsStream;

		// Token: 0x0400022C RID: 556
		private readonly HttpWebRequest request;
	}
}
