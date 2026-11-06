using System;

namespace Features.Workshop
{
	// Token: 0x02000B45 RID: 2885
	public sealed class WorkshopModStatus
	{
		// Token: 0x17000DB8 RID: 3512
		// (get) Token: 0x060051E3 RID: 20963 RVA: 0x001FB800 File Offset: 0x001F9A00
		// (set) Token: 0x060051E4 RID: 20964 RVA: 0x001FB808 File Offset: 0x001F9A08
		public bool Supported { get; internal set; }

		// Token: 0x17000DB9 RID: 3513
		// (get) Token: 0x060051E5 RID: 20965 RVA: 0x001FB811 File Offset: 0x001F9A11
		// (set) Token: 0x060051E6 RID: 20966 RVA: 0x001FB819 File Offset: 0x001F9A19
		public bool Enabled { get; internal set; }

		// Token: 0x17000DBA RID: 3514
		// (get) Token: 0x060051E7 RID: 20967 RVA: 0x001FB822 File Offset: 0x001F9A22
		// (set) Token: 0x060051E8 RID: 20968 RVA: 0x001FB82A File Offset: 0x001F9A2A
		public bool Loaded { get; internal set; }

		// Token: 0x17000DBB RID: 3515
		// (get) Token: 0x060051E9 RID: 20969 RVA: 0x001FB833 File Offset: 0x001F9A33
		// (set) Token: 0x060051EA RID: 20970 RVA: 0x001FB83B File Offset: 0x001F9A3B
		public string StatusKey { get; internal set; }

		// Token: 0x17000DBC RID: 3516
		// (get) Token: 0x060051EB RID: 20971 RVA: 0x001FB844 File Offset: 0x001F9A44
		// (set) Token: 0x060051EC RID: 20972 RVA: 0x001FB84C File Offset: 0x001F9A4C
		public string StatusFallback { get; internal set; }

		// Token: 0x17000DBD RID: 3517
		// (get) Token: 0x060051ED RID: 20973 RVA: 0x001FB855 File Offset: 0x001F9A55
		// (set) Token: 0x060051EE RID: 20974 RVA: 0x001FB85D File Offset: 0x001F9A5D
		public string Detail { get; internal set; }
	}
}
