using System;
using Newtonsoft.Json;
using UnityEngine;

namespace DA_Assets.FCU.Model
{
	// Token: 0x02000069 RID: 105
	public struct LayoutGrid
	{
		// Token: 0x17000149 RID: 329
		// (get) Token: 0x06000307 RID: 775 RVA: 0x00007825 File Offset: 0x00005A25
		// (set) Token: 0x06000308 RID: 776 RVA: 0x0000782D File Offset: 0x00005A2D
		[JsonProperty("pattern")]
		public string Pattern { readonly get; set; }

		// Token: 0x1700014A RID: 330
		// (get) Token: 0x06000309 RID: 777 RVA: 0x00007836 File Offset: 0x00005A36
		// (set) Token: 0x0600030A RID: 778 RVA: 0x0000783E File Offset: 0x00005A3E
		[JsonProperty("sectionSize")]
		public float SectionSize { readonly get; set; }

		// Token: 0x1700014B RID: 331
		// (get) Token: 0x0600030B RID: 779 RVA: 0x00007847 File Offset: 0x00005A47
		// (set) Token: 0x0600030C RID: 780 RVA: 0x0000784F File Offset: 0x00005A4F
		[JsonProperty("visible")]
		public bool Visible { readonly get; set; }

		// Token: 0x1700014C RID: 332
		// (get) Token: 0x0600030D RID: 781 RVA: 0x00007858 File Offset: 0x00005A58
		// (set) Token: 0x0600030E RID: 782 RVA: 0x00007860 File Offset: 0x00005A60
		[JsonProperty("color")]
		public Color Color { readonly get; set; }

		// Token: 0x1700014D RID: 333
		// (get) Token: 0x0600030F RID: 783 RVA: 0x00007869 File Offset: 0x00005A69
		// (set) Token: 0x06000310 RID: 784 RVA: 0x00007871 File Offset: 0x00005A71
		[JsonProperty("alignment")]
		public string Alignment { readonly get; set; }

		// Token: 0x1700014E RID: 334
		// (get) Token: 0x06000311 RID: 785 RVA: 0x0000787A File Offset: 0x00005A7A
		// (set) Token: 0x06000312 RID: 786 RVA: 0x00007882 File Offset: 0x00005A82
		[JsonProperty("gutterSize")]
		public float GutterSize { readonly get; set; }

		// Token: 0x1700014F RID: 335
		// (get) Token: 0x06000313 RID: 787 RVA: 0x0000788B File Offset: 0x00005A8B
		// (set) Token: 0x06000314 RID: 788 RVA: 0x00007893 File Offset: 0x00005A93
		[JsonProperty("offset")]
		public float Offset { readonly get; set; }

		// Token: 0x17000150 RID: 336
		// (get) Token: 0x06000315 RID: 789 RVA: 0x0000789C File Offset: 0x00005A9C
		// (set) Token: 0x06000316 RID: 790 RVA: 0x000078A4 File Offset: 0x00005AA4
		[JsonProperty("count")]
		public int Count { readonly get; set; }
	}
}
