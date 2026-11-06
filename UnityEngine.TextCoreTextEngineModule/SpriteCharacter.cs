using System;

namespace UnityEngine.TextCore.Text
{
	// Token: 0x0200001B RID: 27
	[Serializable]
	public class SpriteCharacter : TextElement
	{
		// Token: 0x1700002A RID: 42
		// (get) Token: 0x060000FB RID: 251 RVA: 0x00008308 File Offset: 0x00006508
		// (set) Token: 0x060000FC RID: 252 RVA: 0x00008320 File Offset: 0x00006520
		public string name
		{
			get
			{
				return this.m_Name;
			}
			set
			{
				bool flag = value == this.m_Name;
				if (!flag)
				{
					this.m_Name = value;
				}
			}
		}

		// Token: 0x060000FD RID: 253 RVA: 0x00008347 File Offset: 0x00006547
		public SpriteCharacter()
		{
			this.m_ElementType = TextElementType.Sprite;
		}

		// Token: 0x060000FE RID: 254 RVA: 0x00008358 File Offset: 0x00006558
		public SpriteCharacter(uint unicode, SpriteGlyph glyph)
		{
			this.m_ElementType = TextElementType.Sprite;
			base.unicode = unicode;
			base.glyphIndex = glyph.index;
			base.glyph = glyph;
			base.scale = 1f;
		}

		// Token: 0x060000FF RID: 255 RVA: 0x00008394 File Offset: 0x00006594
		public SpriteCharacter(uint unicode, SpriteAsset spriteAsset, SpriteGlyph glyph)
		{
			this.m_ElementType = TextElementType.Sprite;
			base.unicode = unicode;
			base.textAsset = spriteAsset;
			base.glyph = glyph;
			base.glyphIndex = glyph.index;
			base.scale = 1f;
		}

		// Token: 0x040000C6 RID: 198
		[SerializeField]
		private string m_Name;
	}
}
