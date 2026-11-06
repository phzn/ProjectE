using System;
using UnityEngine;

namespace MoreMountains.Tools
{
	// Token: 0x02000019 RID: 25
	[Serializable]
	public class Ratio
	{
		// Token: 0x06000091 RID: 145 RVA: 0x00003254 File Offset: 0x00001454
		public Ratio(bool drawRatio, Vector2 size, Color ratioColor)
		{
			this.DrawRatio = drawRatio;
			this.Size = size;
			this.RatioColor = ratioColor;
		}

		// Token: 0x04000086 RID: 134
		public bool DrawRatio = true;

		// Token: 0x04000087 RID: 135
		public Vector2 Size;

		// Token: 0x04000088 RID: 136
		public Color RatioColor;
	}
}
