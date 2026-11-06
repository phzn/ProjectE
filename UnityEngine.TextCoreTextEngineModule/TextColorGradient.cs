using System;

namespace UnityEngine.TextCore.Text
{
	// Token: 0x0200001F RID: 31
	[ExcludeFromPreset]
	[ExcludeFromObjectFactory]
	[Serializable]
	public class TextColorGradient : ScriptableObject
	{
		// Token: 0x0600010D RID: 269 RVA: 0x00008568 File Offset: 0x00006768
		public TextColorGradient()
		{
			this.colorMode = ColorGradientMode.FourCornersGradient;
			this.topLeft = TextColorGradient.k_DefaultColor;
			this.topRight = TextColorGradient.k_DefaultColor;
			this.bottomLeft = TextColorGradient.k_DefaultColor;
			this.bottomRight = TextColorGradient.k_DefaultColor;
		}

		// Token: 0x0600010E RID: 270 RVA: 0x000085B7 File Offset: 0x000067B7
		public TextColorGradient(Color color)
		{
			this.colorMode = ColorGradientMode.FourCornersGradient;
			this.topLeft = color;
			this.topRight = color;
			this.bottomLeft = color;
			this.bottomRight = color;
		}

		// Token: 0x0600010F RID: 271 RVA: 0x000085EB File Offset: 0x000067EB
		public TextColorGradient(Color color0, Color color1, Color color2, Color color3)
		{
			this.colorMode = ColorGradientMode.FourCornersGradient;
			this.topLeft = color0;
			this.topRight = color1;
			this.bottomLeft = color2;
			this.bottomRight = color3;
		}

		// Token: 0x040000D2 RID: 210
		public ColorGradientMode colorMode = ColorGradientMode.FourCornersGradient;

		// Token: 0x040000D3 RID: 211
		public Color topLeft;

		// Token: 0x040000D4 RID: 212
		public Color topRight;

		// Token: 0x040000D5 RID: 213
		public Color bottomLeft;

		// Token: 0x040000D6 RID: 214
		public Color bottomRight;

		// Token: 0x040000D7 RID: 215
		private const ColorGradientMode k_DefaultColorMode = ColorGradientMode.FourCornersGradient;

		// Token: 0x040000D8 RID: 216
		private static readonly Color k_DefaultColor = Color.white;
	}
}
