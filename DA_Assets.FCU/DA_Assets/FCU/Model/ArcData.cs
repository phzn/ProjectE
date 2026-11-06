using System;
using Newtonsoft.Json;

namespace DA_Assets.FCU.Model
{
	// Token: 0x02000061 RID: 97
	public struct ArcData
	{
		// Token: 0x17000134 RID: 308
		// (get) Token: 0x060002DD RID: 733 RVA: 0x000076C0 File Offset: 0x000058C0
		// (set) Token: 0x060002DE RID: 734 RVA: 0x000076C8 File Offset: 0x000058C8
		[JsonProperty("startingAngle")]
		public float StartingAngle { readonly get; set; }

		// Token: 0x17000135 RID: 309
		// (get) Token: 0x060002DF RID: 735 RVA: 0x000076D1 File Offset: 0x000058D1
		// (set) Token: 0x060002E0 RID: 736 RVA: 0x000076D9 File Offset: 0x000058D9
		[JsonProperty("endingAngle")]
		public float EndingAngle { readonly get; set; }

		// Token: 0x17000136 RID: 310
		// (get) Token: 0x060002E1 RID: 737 RVA: 0x000076E2 File Offset: 0x000058E2
		// (set) Token: 0x060002E2 RID: 738 RVA: 0x000076EA File Offset: 0x000058EA
		[JsonProperty("innerRadius")]
		public float InnerRadius { readonly get; set; }
	}
}
