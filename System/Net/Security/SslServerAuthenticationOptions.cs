using System;
using System.Collections.Generic;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;

namespace System.Net.Security
{
	// Token: 0x02000855 RID: 2133
	public class SslServerAuthenticationOptions
	{
		// Token: 0x17000F42 RID: 3906
		// (get) Token: 0x060043E2 RID: 17378 RVA: 0x000EC6C5 File Offset: 0x000EA8C5
		// (set) Token: 0x060043E3 RID: 17379 RVA: 0x000EC6CD File Offset: 0x000EA8CD
		public bool AllowRenegotiation
		{
			get
			{
				return this._allowRenegotiation;
			}
			set
			{
				this._allowRenegotiation = value;
			}
		}

		// Token: 0x17000F43 RID: 3907
		// (get) Token: 0x060043E4 RID: 17380 RVA: 0x000EC6D6 File Offset: 0x000EA8D6
		// (set) Token: 0x060043E5 RID: 17381 RVA: 0x000EC6DE File Offset: 0x000EA8DE
		public bool ClientCertificateRequired { get; set; }

		// Token: 0x17000F44 RID: 3908
		// (get) Token: 0x060043E6 RID: 17382 RVA: 0x000EC6E7 File Offset: 0x000EA8E7
		// (set) Token: 0x060043E7 RID: 17383 RVA: 0x000EC6EF File Offset: 0x000EA8EF
		public List<SslApplicationProtocol> ApplicationProtocols { get; set; }

		// Token: 0x17000F45 RID: 3909
		// (get) Token: 0x060043E8 RID: 17384 RVA: 0x000EC6F8 File Offset: 0x000EA8F8
		// (set) Token: 0x060043E9 RID: 17385 RVA: 0x000EC700 File Offset: 0x000EA900
		public RemoteCertificateValidationCallback RemoteCertificateValidationCallback { get; set; }

		// Token: 0x17000F46 RID: 3910
		// (get) Token: 0x060043EA RID: 17386 RVA: 0x000EC709 File Offset: 0x000EA909
		// (set) Token: 0x060043EB RID: 17387 RVA: 0x000EC711 File Offset: 0x000EA911
		public ServerCertificateSelectionCallback ServerCertificateSelectionCallback { get; set; }

		// Token: 0x17000F47 RID: 3911
		// (get) Token: 0x060043EC RID: 17388 RVA: 0x000EC71A File Offset: 0x000EA91A
		// (set) Token: 0x060043ED RID: 17389 RVA: 0x000EC722 File Offset: 0x000EA922
		public X509Certificate ServerCertificate { get; set; }

		// Token: 0x17000F48 RID: 3912
		// (get) Token: 0x060043EE RID: 17390 RVA: 0x000EC72B File Offset: 0x000EA92B
		// (set) Token: 0x060043EF RID: 17391 RVA: 0x000EC733 File Offset: 0x000EA933
		public SslProtocols EnabledSslProtocols
		{
			get
			{
				return this._enabledSslProtocols;
			}
			set
			{
				this._enabledSslProtocols = value;
			}
		}

		// Token: 0x17000F49 RID: 3913
		// (get) Token: 0x060043F0 RID: 17392 RVA: 0x000EC73C File Offset: 0x000EA93C
		// (set) Token: 0x060043F1 RID: 17393 RVA: 0x000EC744 File Offset: 0x000EA944
		public X509RevocationMode CertificateRevocationCheckMode
		{
			get
			{
				return this._checkCertificateRevocation;
			}
			set
			{
				if (value != X509RevocationMode.NoCheck && value != X509RevocationMode.Offline && value != X509RevocationMode.Online)
				{
					throw new ArgumentException(SR.Format("The specified value is not valid in the '{0}' enumeration.", "X509RevocationMode"), "value");
				}
				this._checkCertificateRevocation = value;
			}
		}

		// Token: 0x17000F4A RID: 3914
		// (get) Token: 0x060043F2 RID: 17394 RVA: 0x000EC772 File Offset: 0x000EA972
		// (set) Token: 0x060043F3 RID: 17395 RVA: 0x000EC77A File Offset: 0x000EA97A
		public EncryptionPolicy EncryptionPolicy
		{
			get
			{
				return this._encryptionPolicy;
			}
			set
			{
				if (value != EncryptionPolicy.RequireEncryption && value != EncryptionPolicy.AllowNoEncryption && value != EncryptionPolicy.NoEncryption)
				{
					throw new ArgumentException(SR.Format("The specified value is not valid in the '{0}' enumeration.", "EncryptionPolicy"), "value");
				}
				this._encryptionPolicy = value;
			}
		}

		// Token: 0x04002907 RID: 10503
		private X509RevocationMode _checkCertificateRevocation;

		// Token: 0x04002908 RID: 10504
		private SslProtocols _enabledSslProtocols;

		// Token: 0x04002909 RID: 10505
		private EncryptionPolicy _encryptionPolicy;

		// Token: 0x0400290A RID: 10506
		private bool _allowRenegotiation = true;
	}
}
