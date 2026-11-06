using System;

namespace UnityEngine.TextCore.Text
{
	// Token: 0x0200001C RID: 28
	[Serializable]
	public class SpriteGlyph : Glyph
	{
		// Token: 0x06000100 RID: 256 RVA: 0x000083E1 File Offset: 0x000065E1
		public SpriteGlyph()
		{
		}

		// Token: 0x06000101 RID: 257 RVA: 0x000083EB File Offset: 0x000065EB
		public SpriteGlyph(uint index, GlyphMetrics metrics, GlyphRect glyphRect, float scale, int atlasIndex)
		{
			base.index = index;
			base.metrics = metrics;
			base.glyphRect = glyphRect;
			base.scale = scale;
			base.atlasIndex = atlasIndex;
		}

		// Token: 0x06000102 RID: 258 RVA: 0x0000841F File Offset: 0x0000661F
		public SpriteGlyph(uint index, GlyphMetrics metrics, GlyphRect glyphRect, float scale, int atlasIndex, Sprite sprite)
		{
			base.index = index;
			base.metrics = metrics;
			base.glyphRect = glyphRect;
			base.scale = scale;
			base.atlasIndex = atlasIndex;
			this.sprite = sprite;
		}

		// Token: 0x040000C7 RID: 199
		public Sprite sprite;
	}
}
