using System;
using UnityEngine;

namespace MoreMountains.Tools
{
	// Token: 0x0200001D RID: 29
	[Serializable]
	public class FogSettings
	{
		// Token: 0x0400009F RID: 159
		public bool FogEnabled = true;

		// Token: 0x040000A0 RID: 160
		public Color FogColor = Color.white;

		// Token: 0x040000A1 RID: 161
		public float FogDensity = 0.01f;

		// Token: 0x040000A2 RID: 162
		public FogMode FogMode = FogMode.ExponentialSquared;
	}
}
