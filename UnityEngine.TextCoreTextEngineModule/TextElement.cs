using System;

namespace UnityEngine.TextCore.Text
{
	// Token: 0x02000021 RID: 33
	[Serializable]
	public abstract class TextElement
	{
		// Token: 0x17000030 RID: 48
		// (get) Token: 0x06000111 RID: 273 RVA: 0x0000862C File Offset: 0x0000682C
		public TextElementType elementType
		{
			get
			{
				return this.m_ElementType;
			}
		}

		// Token: 0x17000031 RID: 49
		// (get) Token: 0x06000112 RID: 274 RVA: 0x00008644 File Offset: 0x00006844
		// (set) Token: 0x06000113 RID: 275 RVA: 0x0000865C File Offset: 0x0000685C
		public uint unicode
		{
			get
			{
				return this.m_Unicode;
			}
			set
			{
				this.m_Unicode = value;
			}
		}

		// Token: 0x17000032 RID: 50
		// (get) Token: 0x06000114 RID: 276 RVA: 0x00008668 File Offset: 0x00006868
		// (set) Token: 0x06000115 RID: 277 RVA: 0x00008680 File Offset: 0x00006880
		public TextAsset textAsset
		{
			get
			{
				return this.m_TextAsset;
			}
			set
			{
				this.m_TextAsset = value;
			}
		}

		// Token: 0x17000033 RID: 51
		// (get) Token: 0x06000116 RID: 278 RVA: 0x0000868C File Offset: 0x0000688C
		// (set) Token: 0x06000117 RID: 279 RVA: 0x000086A4 File Offset: 0x000068A4
		public Glyph glyph
		{
			get
			{
				return this.m_Glyph;
			}
			set
			{
				this.m_Glyph = value;
			}
		}

		// Token: 0x17000034 RID: 52
		// (get) Token: 0x06000118 RID: 280 RVA: 0x000086B0 File Offset: 0x000068B0
		// (set) Token: 0x06000119 RID: 281 RVA: 0x000086C8 File Offset: 0x000068C8
		public uint glyphIndex
		{
			get
			{
				return this.m_GlyphIndex;
			}
			set
			{
				this.m_GlyphIndex = value;
			}
		}

		// Token: 0x17000035 RID: 53
		// (get) Token: 0x0600011A RID: 282 RVA: 0x000086D4 File Offset: 0x000068D4
		// (set) Token: 0x0600011B RID: 283 RVA: 0x000086EC File Offset: 0x000068EC
		public float scale
		{
			get
			{
				return this.m_Scale;
			}
			set
			{
				this.m_Scale = value;
			}
		}

		// Token: 0x040000DC RID: 220
		[SerializeField]
		protected TextElementType m_ElementType;

		// Token: 0x040000DD RID: 221
		[SerializeField]
		internal uint m_Unicode;

		// Token: 0x040000DE RID: 222
		internal TextAsset m_TextAsset;

		// Token: 0x040000DF RID: 223
		internal Glyph m_Glyph;

		// Token: 0x040000E0 RID: 224
		[SerializeField]
		internal uint m_GlyphIndex;

		// Token: 0x040000E1 RID: 225
		[SerializeField]
		internal float m_Scale;
	}
}
