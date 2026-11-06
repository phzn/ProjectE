using System;
using Newtonsoft.Json;

namespace DA_Assets.FCU.Model
{
	// Token: 0x02000062 RID: 98
	public struct BoundingBox
	{
		// Token: 0x17000137 RID: 311
		// (get) Token: 0x060002E3 RID: 739 RVA: 0x000076F3 File Offset: 0x000058F3
		// (set) Token: 0x060002E4 RID: 740 RVA: 0x000076FB File Offset: 0x000058FB
		[JsonProperty("x")]
		public float? X { readonly get; set; }

		// Token: 0x17000138 RID: 312
		// (get) Token: 0x060002E5 RID: 741 RVA: 0x00007704 File Offset: 0x00005904
		// (set) Token: 0x060002E6 RID: 742 RVA: 0x0000770C File Offset: 0x0000590C
		[JsonProperty("y")]
		public float? Y { readonly get; set; }

		// Token: 0x17000139 RID: 313
		// (get) Token: 0x060002E7 RID: 743 RVA: 0x00007715 File Offset: 0x00005915
		// (set) Token: 0x060002E8 RID: 744 RVA: 0x0000771D File Offset: 0x0000591D
		[JsonProperty("width")]
		public float? Width { readonly get; set; }

		// Token: 0x1700013A RID: 314
		// (get) Token: 0x060002E9 RID: 745 RVA: 0x00007726 File Offset: 0x00005926
		// (set) Token: 0x060002EA RID: 746 RVA: 0x0000772E File Offset: 0x0000592E
		[JsonProperty("height")]
		public float? Height { readonly get; set; }
	}
}
