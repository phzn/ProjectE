using System;

namespace Mono.Security.Interface
{
	// Token: 0x02000043 RID: 67
	public class MonoTlsConnectionInfo
	{
		// Token: 0x170000C8 RID: 200
		// (get) Token: 0x06000291 RID: 657 RVA: 0x0000EF7D File Offset: 0x0000D17D
		// (set) Token: 0x06000292 RID: 658 RVA: 0x0000EF85 File Offset: 0x0000D185
		[CLSCompliant(false)]
		public CipherSuiteCode CipherSuiteCode { get; set; }

		// Token: 0x170000C9 RID: 201
		// (get) Token: 0x06000293 RID: 659 RVA: 0x0000EF8E File Offset: 0x0000D18E
		// (set) Token: 0x06000294 RID: 660 RVA: 0x0000EF96 File Offset: 0x0000D196
		public TlsProtocols ProtocolVersion { get; set; }

		// Token: 0x170000CA RID: 202
		// (get) Token: 0x06000295 RID: 661 RVA: 0x0000EF9F File Offset: 0x0000D19F
		// (set) Token: 0x06000296 RID: 662 RVA: 0x0000EFA7 File Offset: 0x0000D1A7
		public CipherAlgorithmType CipherAlgorithmType { get; set; }

		// Token: 0x170000CB RID: 203
		// (get) Token: 0x06000297 RID: 663 RVA: 0x0000EFB0 File Offset: 0x0000D1B0
		// (set) Token: 0x06000298 RID: 664 RVA: 0x0000EFB8 File Offset: 0x0000D1B8
		public HashAlgorithmType HashAlgorithmType { get; set; }

		// Token: 0x170000CC RID: 204
		// (get) Token: 0x06000299 RID: 665 RVA: 0x0000EFC1 File Offset: 0x0000D1C1
		// (set) Token: 0x0600029A RID: 666 RVA: 0x0000EFC9 File Offset: 0x0000D1C9
		public ExchangeAlgorithmType ExchangeAlgorithmType { get; set; }

		// Token: 0x170000CD RID: 205
		// (get) Token: 0x0600029B RID: 667 RVA: 0x0000EFD2 File Offset: 0x0000D1D2
		// (set) Token: 0x0600029C RID: 668 RVA: 0x0000EFDA File Offset: 0x0000D1DA
		public string PeerDomainName { get; set; }

		// Token: 0x0600029D RID: 669 RVA: 0x0000EFE3 File Offset: 0x0000D1E3
		public override string ToString()
		{
			return string.Format("[MonoTlsConnectionInfo: {0}:{1}]", this.ProtocolVersion, this.CipherSuiteCode);
		}
	}
}
