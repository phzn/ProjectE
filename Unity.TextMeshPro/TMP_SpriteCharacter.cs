using System;
using UnityEngine;

namespace TMPro
{
	// Token: 0x02000055 RID: 85
	[Serializable]
	public class TMP_SpriteCharacter : TMP_TextElement
	{
		// Token: 0x170000D2 RID: 210
		// (get) Token: 0x060003C0 RID: 960 RVA: 0x000267AD File Offset: 0x000249AD
		// (set) Token: 0x060003C1 RID: 961 RVA: 0x000267B5 File Offset: 0x000249B5
		public string name
		{
			get
			{
				return this.m_Name;
			}
			set
			{
				if (value == this.m_Name)
				{
					return;
				}
				this.m_Name = value;
				this.m_HashCode = TMP_TextParsingUtilities.GetHashCodeCaseSensitive(this.m_Name);
			}
		}

		// Token: 0x170000D3 RID: 211
		// (get) Token: 0x060003C2 RID: 962 RVA: 0x000267DE File Offset: 0x000249DE
		public int hashCode
		{
			get
			{
				return this.m_HashCode;
			}
		}

		// Token: 0x060003C3 RID: 963 RVA: 0x000267E6 File Offset: 0x000249E6
		public TMP_SpriteCharacter()
		{
			this.m_ElementType = TextElementType.Sprite;
		}

		// Token: 0x060003C4 RID: 964 RVA: 0x000267F5 File Offset: 0x000249F5
		public TMP_SpriteCharacter(uint unicode, TMP_SpriteGlyph glyph)
		{
			this.m_ElementType = TextElementType.Sprite;
			base.unicode = unicode;
			base.glyphIndex = glyph.index;
			base.glyph = glyph;
			base.scale = 1f;
		}

		// Token: 0x060003C5 RID: 965 RVA: 0x00026829 File Offset: 0x00024A29
		public TMP_SpriteCharacter(uint unicode, TMP_SpriteAsset spriteAsset, TMP_SpriteGlyph glyph)
		{
			this.m_ElementType = TextElementType.Sprite;
			base.unicode = unicode;
			base.textAsset = spriteAsset;
			base.glyph = glyph;
			base.glyphIndex = glyph.index;
			base.scale = 1f;
		}

		// Token: 0x060003C6 RID: 966 RVA: 0x00026864 File Offset: 0x00024A64
		internal TMP_SpriteCharacter(uint unicode, uint glyphIndex)
		{
			this.m_ElementType = TextElementType.Sprite;
			base.unicode = unicode;
			base.textAsset = null;
			base.glyph = null;
			base.glyphIndex = glyphIndex;
			base.scale = 1f;
		}

		// Token: 0x040003A7 RID: 935
		[SerializeField]
		private string m_Name;

		// Token: 0x040003A8 RID: 936
		[SerializeField]
		private int m_HashCode;
	}
}
