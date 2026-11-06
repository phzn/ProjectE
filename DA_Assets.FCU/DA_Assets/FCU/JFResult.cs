using System;

namespace DA_Assets.FCU
{
	// Token: 0x02000011 RID: 17
	public struct JFResult
	{
		// Token: 0x17000043 RID: 67
		// (get) Token: 0x0600007B RID: 123 RVA: 0x0000312C File Offset: 0x0000132C
		// (set) Token: 0x0600007C RID: 124 RVA: 0x00003134 File Offset: 0x00001334
		public bool IsValid { readonly get; set; }

		// Token: 0x17000044 RID: 68
		// (get) Token: 0x0600007D RID: 125 RVA: 0x0000313D File Offset: 0x0000133D
		// (set) Token: 0x0600007E RID: 126 RVA: 0x00003145 File Offset: 0x00001345
		public string Json { readonly get; set; }

		// Token: 0x17000045 RID: 69
		// (get) Token: 0x0600007F RID: 127 RVA: 0x0000314E File Offset: 0x0000134E
		// (set) Token: 0x06000080 RID: 128 RVA: 0x00003156 File Offset: 0x00001356
		public bool MatchTargetType { readonly get; set; }
	}
}
