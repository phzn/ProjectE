using System;
using UnityEngine;

namespace Invector.Throw
{
	// Token: 0x02000506 RID: 1286
	[CreateAssetMenu(menuName = "Invector/Throw/New ThrowVisualSettings")]
	public class vThrowVisualSettings : ScriptableObject
	{
		// Token: 0x04001FDD RID: 8157
		[vSeparator("Visual Line", "")]
		public bool useLine;

		// Token: 0x04001FDE RID: 8158
		[ColorUsage(true, true)]
		public Color lineRendererColor = Color.white;

		// Token: 0x04001FDF RID: 8159
		public float lineRendererWidth = 1f;

		// Token: 0x04001FE0 RID: 8160
		internal LineAlignment lineAlignment = LineAlignment.Local;

		// Token: 0x04001FE1 RID: 8161
		public Texture2D lineTexture;

		// Token: 0x04001FE2 RID: 8162
		public LineTextureMode lineTextureMode;

		// Token: 0x04001FE3 RID: 8163
		public Vector2 lineTile = Vector2.one;

		// Token: 0x04001FE4 RID: 8164
		public Vector2 lineOffset = Vector2.zero;

		// Token: 0x04001FE5 RID: 8165
		[vSeparator("Visual Indicator", "")]
		public bool useIndicator;

		// Token: 0x04001FE6 RID: 8166
		[ColorUsage(true, true)]
		public Color indicatorColor = Color.white;

		// Token: 0x04001FE7 RID: 8167
		public float indicatorRange = 1f;

		// Token: 0x04001FE8 RID: 8168
		public Texture2D indicatorTexture;
	}
}
