using System;

namespace Mono.Btls
{
	// Token: 0x020000F8 RID: 248
	internal class MonoBtlsX509Exception : Exception
	{
		// Token: 0x170000FC RID: 252
		// (get) Token: 0x060005B4 RID: 1460 RVA: 0x00010628 File Offset: 0x0000E828
		// (set) Token: 0x060005B5 RID: 1461 RVA: 0x00010630 File Offset: 0x0000E830
		public MonoBtlsX509Error ErrorCode { get; private set; }

		// Token: 0x170000FD RID: 253
		// (get) Token: 0x060005B6 RID: 1462 RVA: 0x00010639 File Offset: 0x0000E839
		// (set) Token: 0x060005B7 RID: 1463 RVA: 0x00010641 File Offset: 0x0000E841
		public string ErrorMessage { get; private set; }

		// Token: 0x060005B8 RID: 1464 RVA: 0x0001064A File Offset: 0x0000E84A
		public MonoBtlsX509Exception(MonoBtlsX509Error code, string message)
			: base(message)
		{
			this.ErrorCode = code;
			this.ErrorMessage = message;
		}

		// Token: 0x060005B9 RID: 1465 RVA: 0x00010661 File Offset: 0x0000E861
		public override string ToString()
		{
			return string.Format("[MonoBtlsX509Exception: ErrorCode={0}, ErrorMessage={1}]", this.ErrorCode, this.ErrorMessage);
		}
	}
}
