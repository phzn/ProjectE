using System;
using Newtonsoft.Json;

namespace DA_Assets.FCU.Model
{
	// Token: 0x02000065 RID: 101
	public struct Styles
	{
		// Token: 0x1700013D RID: 317
		// (get) Token: 0x060002EF RID: 751 RVA: 0x00007759 File Offset: 0x00005959
		// (set) Token: 0x060002F0 RID: 752 RVA: 0x00007761 File Offset: 0x00005961
		[JsonProperty("stroke")]
		public string Stroke { readonly get; set; }

		// Token: 0x1700013E RID: 318
		// (get) Token: 0x060002F1 RID: 753 RVA: 0x0000776A File Offset: 0x0000596A
		// (set) Token: 0x060002F2 RID: 754 RVA: 0x00007772 File Offset: 0x00005972
		[JsonProperty("fill")]
		public string Fill { readonly get; set; }

		// Token: 0x1700013F RID: 319
		// (get) Token: 0x060002F3 RID: 755 RVA: 0x0000777B File Offset: 0x0000597B
		// (set) Token: 0x060002F4 RID: 756 RVA: 0x00007783 File Offset: 0x00005983
		[JsonProperty("fills")]
		public string Fills { readonly get; set; }

		// Token: 0x17000140 RID: 320
		// (get) Token: 0x060002F5 RID: 757 RVA: 0x0000778C File Offset: 0x0000598C
		// (set) Token: 0x060002F6 RID: 758 RVA: 0x00007794 File Offset: 0x00005994
		[JsonProperty("text")]
		public string Text { readonly get; set; }

		// Token: 0x17000141 RID: 321
		// (get) Token: 0x060002F7 RID: 759 RVA: 0x0000779D File Offset: 0x0000599D
		// (set) Token: 0x060002F8 RID: 760 RVA: 0x000077A5 File Offset: 0x000059A5
		[JsonProperty("effect")]
		public string Effect { readonly get; set; }

		// Token: 0x17000142 RID: 322
		// (get) Token: 0x060002F9 RID: 761 RVA: 0x000077AE File Offset: 0x000059AE
		// (set) Token: 0x060002FA RID: 762 RVA: 0x000077B6 File Offset: 0x000059B6
		[JsonProperty("strokes")]
		public string Strokes { readonly get; set; }

		// Token: 0x17000143 RID: 323
		// (get) Token: 0x060002FB RID: 763 RVA: 0x000077BF File Offset: 0x000059BF
		// (set) Token: 0x060002FC RID: 764 RVA: 0x000077C7 File Offset: 0x000059C7
		[JsonProperty("grid")]
		public string Grid { readonly get; set; }
	}
}
