using System;
using System.Collections.Generic;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;

namespace System.Net.Security
{
	// Token: 0x02000854 RID: 2132
	public class SslClientAuthenticationOptions
	{
		// Token: 0x17000F39 RID: 3897
		// (get) Token: 0x060043CF RID: 17359 RVA: 0x000EC5D3 File Offset: 0x000EA7D3
		// (set) Token: 0x060043D0 RID: 17360 RVA: 0x000EC5DB File Offset: 0x000EA7DB
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

		// Token: 0x17000F3A RID: 3898
		// (get) Token: 0x060043D1 RID: 17361 RVA: 0x000EC5E4 File Offset: 0x000EA7E4
		// (set) Token: 0x060043D2 RID: 17362 RVA: 0x000EC5EC File Offset: 0x000EA7EC
		public LocalCertificateSelectionCallback LocalCertificateSelectionCallback { get; set; }

		// Token: 0x17000F3B RID: 3899
		// (get) Token: 0x060043D3 RID: 17363 RVA: 0x000EC5F5 File Offset: 0x000EA7F5
		// (set) Token: 0x060043D4 RID: 17364 RVA: 0x000EC5FD File Offset: 0x000EA7FD
		public RemoteCertificateValidationCallback RemoteCertificateValidationCallback { get; set; }

		// Token: 0x17000F3C RID: 3900
		// (get) Token: 0x060043D5 RID: 17365 RVA: 0x000EC606 File Offset: 0x000EA806
		// (set) Token: 0x060043D6 RID: 17366 RVA: 0x000EC60E File Offset: 0x000EA80E
		public List<SslApplicationProtocol> ApplicationProtocols { get; set; }

		// Token: 0x17000F3D RID: 3901
		// (get) Token: 0x060043D7 RID: 17367 RVA: 0x000EC617 File Offset: 0x000EA817
		// (set) Token: 0x060043D8 RID: 17368 RVA: 0x000EC61F File Offset: 0x000EA81F
		public string TargetHost { get; set; }

		// Token: 0x17000F3E RID: 3902
		// (get) Token: 0x060043D9 RID: 17369 RVA: 0x000EC628 File Offset: 0x000EA828
		// (set) Token: 0x060043DA RID: 17370 RVA: 0x000EC630 File Offset: 0x000EA830
		public X509CertificateCollection ClientCertificates { get; set; }

		// Token: 0x17000F3F RID: 3903
		// (get) Token: 0x060043DB RID: 17371 RVA: 0x000EC639 File Offset: 0x000EA839
		// (set) Token: 0x060043DC RID: 17372 RVA: 0x000EC641 File Offset: 0x000EA841
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

		// Token: 0x17000F40 RID: 3904
		// (get) Token: 0x060043DD RID: 17373 RVA: 0x000EC66F File Offset: 0x000EA86F
		// (set) Token: 0x060043DE RID: 17374 RVA: 0x000EC677 File Offset: 0x000EA877
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

		// Token: 0x17000F41 RID: 3905
		// (get) Token: 0x060043DF RID: 17375 RVA: 0x000EC6A5 File Offset: 0x000EA8A5
		// (set) Token: 0x060043E0 RID: 17376 RVA: 0x000EC6AD File Offset: 0x000EA8AD
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

		// Token: 0x040028FE RID: 10494
		private EncryptionPolicy _encryptionPolicy;

		// Token: 0x040028FF RID: 10495
		private X509RevocationMode _checkCertificateRevocation;

		// Token: 0x04002900 RID: 10496
		private SslProtocols _enabledSslProtocols;

		// Token: 0x04002901 RID: 10497
		private bool _allowRenegotiation = true;
	}
}
