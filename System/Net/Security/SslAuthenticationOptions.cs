using System;
using System.Collections.Generic;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;

namespace System.Net.Security
{
	// Token: 0x02000853 RID: 2131
	internal class SslAuthenticationOptions
	{
		// Token: 0x060043AE RID: 17326 RVA: 0x000EC39C File Offset: 0x000EA59C
		internal SslAuthenticationOptions(SslClientAuthenticationOptions sslClientAuthenticationOptions, RemoteCertValidationCallback remoteCallback, LocalCertSelectionCallback localCallback)
		{
			this.AllowRenegotiation = sslClientAuthenticationOptions.AllowRenegotiation;
			this.ApplicationProtocols = sslClientAuthenticationOptions.ApplicationProtocols;
			this.CertValidationDelegate = remoteCallback;
			this.CheckCertName = true;
			this.EnabledSslProtocols = sslClientAuthenticationOptions.EnabledSslProtocols;
			this.EncryptionPolicy = sslClientAuthenticationOptions.EncryptionPolicy;
			this.IsServer = false;
			this.RemoteCertRequired = true;
			this.RemoteCertificateValidationCallback = sslClientAuthenticationOptions.RemoteCertificateValidationCallback;
			this.TargetHost = sslClientAuthenticationOptions.TargetHost;
			this.CertSelectionDelegate = localCallback;
			this.CertificateRevocationCheckMode = sslClientAuthenticationOptions.CertificateRevocationCheckMode;
			this.ClientCertificates = sslClientAuthenticationOptions.ClientCertificates;
			this.LocalCertificateSelectionCallback = sslClientAuthenticationOptions.LocalCertificateSelectionCallback;
		}

		// Token: 0x060043AF RID: 17327 RVA: 0x000EC440 File Offset: 0x000EA640
		internal SslAuthenticationOptions(SslServerAuthenticationOptions sslServerAuthenticationOptions)
		{
			this.AllowRenegotiation = sslServerAuthenticationOptions.AllowRenegotiation;
			this.ApplicationProtocols = sslServerAuthenticationOptions.ApplicationProtocols;
			this.CheckCertName = false;
			this.EnabledSslProtocols = sslServerAuthenticationOptions.EnabledSslProtocols;
			this.EncryptionPolicy = sslServerAuthenticationOptions.EncryptionPolicy;
			this.IsServer = true;
			this.RemoteCertRequired = sslServerAuthenticationOptions.ClientCertificateRequired;
			this.RemoteCertificateValidationCallback = sslServerAuthenticationOptions.RemoteCertificateValidationCallback;
			this.TargetHost = string.Empty;
			this.CertificateRevocationCheckMode = sslServerAuthenticationOptions.CertificateRevocationCheckMode;
			this.ServerCertificate = sslServerAuthenticationOptions.ServerCertificate;
		}

		// Token: 0x17000F29 RID: 3881
		// (get) Token: 0x060043B0 RID: 17328 RVA: 0x000EC4CC File Offset: 0x000EA6CC
		// (set) Token: 0x060043B1 RID: 17329 RVA: 0x000EC4D4 File Offset: 0x000EA6D4
		internal bool AllowRenegotiation { get; set; }

		// Token: 0x17000F2A RID: 3882
		// (get) Token: 0x060043B2 RID: 17330 RVA: 0x000EC4DD File Offset: 0x000EA6DD
		// (set) Token: 0x060043B3 RID: 17331 RVA: 0x000EC4E5 File Offset: 0x000EA6E5
		internal string TargetHost { get; set; }

		// Token: 0x17000F2B RID: 3883
		// (get) Token: 0x060043B4 RID: 17332 RVA: 0x000EC4EE File Offset: 0x000EA6EE
		// (set) Token: 0x060043B5 RID: 17333 RVA: 0x000EC4F6 File Offset: 0x000EA6F6
		internal X509CertificateCollection ClientCertificates { get; set; }

		// Token: 0x17000F2C RID: 3884
		// (get) Token: 0x060043B6 RID: 17334 RVA: 0x000EC4FF File Offset: 0x000EA6FF
		internal List<SslApplicationProtocol> ApplicationProtocols { get; }

		// Token: 0x17000F2D RID: 3885
		// (get) Token: 0x060043B7 RID: 17335 RVA: 0x000EC507 File Offset: 0x000EA707
		// (set) Token: 0x060043B8 RID: 17336 RVA: 0x000EC50F File Offset: 0x000EA70F
		internal bool IsServer { get; set; }

		// Token: 0x17000F2E RID: 3886
		// (get) Token: 0x060043B9 RID: 17337 RVA: 0x000EC518 File Offset: 0x000EA718
		// (set) Token: 0x060043BA RID: 17338 RVA: 0x000EC520 File Offset: 0x000EA720
		internal RemoteCertificateValidationCallback RemoteCertificateValidationCallback { get; set; }

		// Token: 0x17000F2F RID: 3887
		// (get) Token: 0x060043BB RID: 17339 RVA: 0x000EC529 File Offset: 0x000EA729
		// (set) Token: 0x060043BC RID: 17340 RVA: 0x000EC531 File Offset: 0x000EA731
		internal LocalCertificateSelectionCallback LocalCertificateSelectionCallback { get; set; }

		// Token: 0x17000F30 RID: 3888
		// (get) Token: 0x060043BD RID: 17341 RVA: 0x000EC53A File Offset: 0x000EA73A
		// (set) Token: 0x060043BE RID: 17342 RVA: 0x000EC542 File Offset: 0x000EA742
		internal X509Certificate ServerCertificate { get; set; }

		// Token: 0x17000F31 RID: 3889
		// (get) Token: 0x060043BF RID: 17343 RVA: 0x000EC54B File Offset: 0x000EA74B
		// (set) Token: 0x060043C0 RID: 17344 RVA: 0x000EC553 File Offset: 0x000EA753
		internal SslProtocols EnabledSslProtocols { get; set; }

		// Token: 0x17000F32 RID: 3890
		// (get) Token: 0x060043C1 RID: 17345 RVA: 0x000EC55C File Offset: 0x000EA75C
		// (set) Token: 0x060043C2 RID: 17346 RVA: 0x000EC564 File Offset: 0x000EA764
		internal X509RevocationMode CertificateRevocationCheckMode { get; set; }

		// Token: 0x17000F33 RID: 3891
		// (get) Token: 0x060043C3 RID: 17347 RVA: 0x000EC56D File Offset: 0x000EA76D
		// (set) Token: 0x060043C4 RID: 17348 RVA: 0x000EC575 File Offset: 0x000EA775
		internal EncryptionPolicy EncryptionPolicy { get; set; }

		// Token: 0x17000F34 RID: 3892
		// (get) Token: 0x060043C5 RID: 17349 RVA: 0x000EC57E File Offset: 0x000EA77E
		// (set) Token: 0x060043C6 RID: 17350 RVA: 0x000EC586 File Offset: 0x000EA786
		internal bool RemoteCertRequired { get; set; }

		// Token: 0x17000F35 RID: 3893
		// (get) Token: 0x060043C7 RID: 17351 RVA: 0x000EC58F File Offset: 0x000EA78F
		// (set) Token: 0x060043C8 RID: 17352 RVA: 0x000EC597 File Offset: 0x000EA797
		internal bool CheckCertName { get; set; }

		// Token: 0x17000F36 RID: 3894
		// (get) Token: 0x060043C9 RID: 17353 RVA: 0x000EC5A0 File Offset: 0x000EA7A0
		// (set) Token: 0x060043CA RID: 17354 RVA: 0x000EC5A8 File Offset: 0x000EA7A8
		internal RemoteCertValidationCallback CertValidationDelegate { get; set; }

		// Token: 0x17000F37 RID: 3895
		// (get) Token: 0x060043CB RID: 17355 RVA: 0x000EC5B1 File Offset: 0x000EA7B1
		// (set) Token: 0x060043CC RID: 17356 RVA: 0x000EC5B9 File Offset: 0x000EA7B9
		internal LocalCertSelectionCallback CertSelectionDelegate { get; set; }

		// Token: 0x17000F38 RID: 3896
		// (get) Token: 0x060043CD RID: 17357 RVA: 0x000EC5C2 File Offset: 0x000EA7C2
		// (set) Token: 0x060043CE RID: 17358 RVA: 0x000EC5CA File Offset: 0x000EA7CA
		internal ServerCertSelectionCallback ServerCertSelectionDelegate { get; set; }
	}
}
