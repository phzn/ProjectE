using System;
using UnityEngine.TextCore;

namespace TMPro
{
	// Token: 0x02000020 RID: 32
	[Serializable]
	public class TMP_Character : TMP_TextElement
	{
		// Token: 0x06000115 RID: 277 RVA: 0x000173F5 File Offset: 0x000155F5
		public TMP_Character()
		{
			this.m_ElementType = TextElementType.Character;
			base.scale = 1f;
		}

		// Token: 0x06000116 RID: 278 RVA: 0x0001740F File Offset: 0x0001560F
		public TMP_Character(uint unicode, Glyph glyph)
		{
			this.m_ElementType = TextElementType.Character;
			base.unicode = unicode;
			base.textAsset = null;
			base.glyph = glyph;
			base.glyphIndex = glyph.index;
			base.scale = 1f;
		}

		// Token: 0x06000117 RID: 279 RVA: 0x0001744A File Offset: 0x0001564A
		public TMP_Character(uint unicode, TMP_FontAsset fontAsset, Glyph glyph)
		{
			this.m_ElementType = TextElementType.Character;
			base.unicode = unicode;
			base.textAsset = fontAsset;
			base.glyph = glyph;
			base.glyphIndex = glyph.index;
			base.scale = 1f;
		}

		// Token: 0x06000118 RID: 280 RVA: 0x00017485 File Offset: 0x00015685
		internal TMP_Character(uint unicode, uint glyphIndex)
		{
			this.m_ElementType = TextElementType.Character;
			base.unicode = unicode;
			base.textAsset = null;
			base.glyph = null;
			base.glyphIndex = glyphIndex;
			base.scale = 1f;
		}
	}
}
