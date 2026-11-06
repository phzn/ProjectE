using System;
using Newtonsoft.Json;
using UnityEngine;

namespace DA_Assets.FCU.Model
{
	// Token: 0x0200005C RID: 92
	public struct Effect : IVisible
	{
		// Token: 0x17000113 RID: 275
		// (get) Token: 0x0600029B RID: 667 RVA: 0x0000748F File Offset: 0x0000568F
		// (set) Token: 0x0600029C RID: 668 RVA: 0x00007497 File Offset: 0x00005697
		[JsonProperty("type")]
		public EffectType Type { readonly get; set; }

		// Token: 0x17000114 RID: 276
		// (get) Token: 0x0600029D RID: 669 RVA: 0x000074A0 File Offset: 0x000056A0
		// (set) Token: 0x0600029E RID: 670 RVA: 0x000074A8 File Offset: 0x000056A8
		[JsonProperty("visible")]
		public bool? Visible { readonly get; set; }

		// Token: 0x17000115 RID: 277
		// (get) Token: 0x0600029F RID: 671 RVA: 0x000074B1 File Offset: 0x000056B1
		// (set) Token: 0x060002A0 RID: 672 RVA: 0x000074B9 File Offset: 0x000056B9
		[JsonProperty("color")]
		public Color Color { readonly get; set; }

		// Token: 0x17000116 RID: 278
		// (get) Token: 0x060002A1 RID: 673 RVA: 0x000074C2 File Offset: 0x000056C2
		// (set) Token: 0x060002A2 RID: 674 RVA: 0x000074CA File Offset: 0x000056CA
		[JsonProperty("opacity")]
		public float? Opacity { readonly get; set; }

		// Token: 0x17000117 RID: 279
		// (get) Token: 0x060002A3 RID: 675 RVA: 0x000074D3 File Offset: 0x000056D3
		// (set) Token: 0x060002A4 RID: 676 RVA: 0x000074DB File Offset: 0x000056DB
		[JsonProperty("blendMode")]
		public string BlendMode { readonly get; set; }

		// Token: 0x17000118 RID: 280
		// (get) Token: 0x060002A5 RID: 677 RVA: 0x000074E4 File Offset: 0x000056E4
		// (set) Token: 0x060002A6 RID: 678 RVA: 0x000074EC File Offset: 0x000056EC
		[JsonProperty("offset")]
		public Vector2 Offset { readonly get; set; }

		// Token: 0x17000119 RID: 281
		// (get) Token: 0x060002A7 RID: 679 RVA: 0x000074F5 File Offset: 0x000056F5
		// (set) Token: 0x060002A8 RID: 680 RVA: 0x000074FD File Offset: 0x000056FD
		[JsonProperty("radius")]
		public float Radius { readonly get; set; }

		// Token: 0x1700011A RID: 282
		// (get) Token: 0x060002A9 RID: 681 RVA: 0x00007506 File Offset: 0x00005706
		// (set) Token: 0x060002AA RID: 682 RVA: 0x0000750E File Offset: 0x0000570E
		[JsonProperty("showShadowBehindNode")]
		public bool? ShowShadowBehindNode { readonly get; set; }

		// Token: 0x1700011B RID: 283
		// (get) Token: 0x060002AB RID: 683 RVA: 0x00007517 File Offset: 0x00005717
		// (set) Token: 0x060002AC RID: 684 RVA: 0x0000751F File Offset: 0x0000571F
		[JsonProperty("spread")]
		public float? Spread { readonly get; set; }
	}
}
