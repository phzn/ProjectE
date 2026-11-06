using System;
using Newtonsoft.Json;
using UnityEngine;

namespace DA_Assets.FCU.Model
{
	// Token: 0x0200005F RID: 95
	public struct GradientStop
	{
		// Token: 0x17000131 RID: 305
		// (get) Token: 0x060002D7 RID: 727 RVA: 0x0000768D File Offset: 0x0000588D
		// (set) Token: 0x060002D8 RID: 728 RVA: 0x00007695 File Offset: 0x00005895
		[JsonProperty("color")]
		public Color Color { readonly get; set; }

		// Token: 0x17000132 RID: 306
		// (get) Token: 0x060002D9 RID: 729 RVA: 0x0000769E File Offset: 0x0000589E
		// (set) Token: 0x060002DA RID: 730 RVA: 0x000076A6 File Offset: 0x000058A6
		[JsonProperty("position")]
		public float Position { readonly get; set; }
	}
}
