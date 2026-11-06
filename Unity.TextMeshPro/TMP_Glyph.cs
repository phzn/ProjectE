using System;

namespace TMPro
{
	// Token: 0x02000031 RID: 49
	[Serializable]
	public class TMP_Glyph : TMP_TextElement_Legacy
	{
		// Token: 0x06000202 RID: 514 RVA: 0x0001C47C File Offset: 0x0001A67C
		public static TMP_Glyph Clone(TMP_Glyph source)
		{
			return new TMP_Glyph
			{
				id = source.id,
				x = source.x,
				y = source.y,
				width = source.width,
				height = source.height,
				xOffset = source.xOffset,
				yOffset = source.yOffset,
				xAdvance = source.xAdvance,
				scale = source.scale
			};
		}
	}
}
