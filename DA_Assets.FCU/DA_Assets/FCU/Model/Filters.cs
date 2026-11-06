using System;
using Newtonsoft.Json;

namespace DA_Assets.FCU.Model
{
	// Token: 0x0200005E RID: 94
	public struct Filters
	{
		// Token: 0x1700012A RID: 298
		// (get) Token: 0x060002C9 RID: 713 RVA: 0x00007616 File Offset: 0x00005816
		// (set) Token: 0x060002CA RID: 714 RVA: 0x0000761E File Offset: 0x0000581E
		[JsonProperty("exposure")]
		public float? Exposure { readonly get; set; }

		// Token: 0x1700012B RID: 299
		// (get) Token: 0x060002CB RID: 715 RVA: 0x00007627 File Offset: 0x00005827
		// (set) Token: 0x060002CC RID: 716 RVA: 0x0000762F File Offset: 0x0000582F
		[JsonProperty("contrast")]
		public float? Contrast { readonly get; set; }

		// Token: 0x1700012C RID: 300
		// (get) Token: 0x060002CD RID: 717 RVA: 0x00007638 File Offset: 0x00005838
		// (set) Token: 0x060002CE RID: 718 RVA: 0x00007640 File Offset: 0x00005840
		[JsonProperty("saturation")]
		public float? Saturation { readonly get; set; }

		// Token: 0x1700012D RID: 301
		// (get) Token: 0x060002CF RID: 719 RVA: 0x00007649 File Offset: 0x00005849
		// (set) Token: 0x060002D0 RID: 720 RVA: 0x00007651 File Offset: 0x00005851
		[JsonProperty("temperature")]
		public float? Temperature { readonly get; set; }

		// Token: 0x1700012E RID: 302
		// (get) Token: 0x060002D1 RID: 721 RVA: 0x0000765A File Offset: 0x0000585A
		// (set) Token: 0x060002D2 RID: 722 RVA: 0x00007662 File Offset: 0x00005862
		[JsonProperty("tint")]
		public float? Tint { readonly get; set; }

		// Token: 0x1700012F RID: 303
		// (get) Token: 0x060002D3 RID: 723 RVA: 0x0000766B File Offset: 0x0000586B
		// (set) Token: 0x060002D4 RID: 724 RVA: 0x00007673 File Offset: 0x00005873
		[JsonProperty("highlights")]
		public float? Highlights { readonly get; set; }

		// Token: 0x17000130 RID: 304
		// (get) Token: 0x060002D5 RID: 725 RVA: 0x0000767C File Offset: 0x0000587C
		// (set) Token: 0x060002D6 RID: 726 RVA: 0x00007684 File Offset: 0x00005884
		[JsonProperty("shadows")]
		public float? Shadows { readonly get; set; }
	}
}
