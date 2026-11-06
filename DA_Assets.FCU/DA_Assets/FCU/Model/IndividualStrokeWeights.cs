using System;
using Newtonsoft.Json;

namespace DA_Assets.FCU.Model
{
	// Token: 0x0200006B RID: 107
	public struct IndividualStrokeWeights
	{
		// Token: 0x17000152 RID: 338
		// (get) Token: 0x06000319 RID: 793 RVA: 0x000078BE File Offset: 0x00005ABE
		// (set) Token: 0x0600031A RID: 794 RVA: 0x000078C6 File Offset: 0x00005AC6
		[JsonProperty("top")]
		public float Top { readonly get; set; }

		// Token: 0x17000153 RID: 339
		// (get) Token: 0x0600031B RID: 795 RVA: 0x000078CF File Offset: 0x00005ACF
		// (set) Token: 0x0600031C RID: 796 RVA: 0x000078D7 File Offset: 0x00005AD7
		[JsonProperty("right")]
		public float Right { readonly get; set; }

		// Token: 0x17000154 RID: 340
		// (get) Token: 0x0600031D RID: 797 RVA: 0x000078E0 File Offset: 0x00005AE0
		// (set) Token: 0x0600031E RID: 798 RVA: 0x000078E8 File Offset: 0x00005AE8
		[JsonProperty("bottom")]
		public float Bottom { readonly get; set; }

		// Token: 0x17000155 RID: 341
		// (get) Token: 0x0600031F RID: 799 RVA: 0x000078F1 File Offset: 0x00005AF1
		// (set) Token: 0x06000320 RID: 800 RVA: 0x000078F9 File Offset: 0x00005AF9
		[JsonProperty("left")]
		public float Left { readonly get; set; }
	}
}
