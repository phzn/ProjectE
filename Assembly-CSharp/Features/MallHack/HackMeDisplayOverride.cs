using System;
using UnityEngine;

namespace Features.MallHack
{
	// Token: 0x02001024 RID: 4132
	[Serializable]
	public sealed class HackMeDisplayOverride
	{
		// Token: 0x04008126 RID: 33062
		public Renderer renderer;

		// Token: 0x04008127 RID: 33063
		[Min(0f)]
		public int materialIndex;

		// Token: 0x04008128 RID: 33064
		public HackMeDisplayMode mode;

		// Token: 0x04008129 RID: 33065
		public Rect atlasUvRect = new Rect(0f, 0f, 1f, 1f);

		// Token: 0x0400812A RID: 33066
		public bool rotateAtlasRegion180;
	}
}
