using System;
using UnityEngine;
using UnityEngine.TextCore;

namespace TMPro
{
	// Token: 0x02000069 RID: 105
	[Serializable]
	public class TMP_TextElement
	{
		// Token: 0x1700014F RID: 335
		// (get) Token: 0x0600055F RID: 1375 RVA: 0x0003540C File Offset: 0x0003360C
		public TextElementType elementType
		{
			get
			{
				return this.m_ElementType;
			}
		}

		// Token: 0x17000150 RID: 336
		// (get) Token: 0x06000560 RID: 1376 RVA: 0x00035414 File Offset: 0x00033614
		// (set) Token: 0x06000561 RID: 1377 RVA: 0x0003541C File Offset: 0x0003361C
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

		// Token: 0x17000151 RID: 337
		// (get) Token: 0x06000562 RID: 1378 RVA: 0x00035425 File Offset: 0x00033625
		// (set) Token: 0x06000563 RID: 1379 RVA: 0x0003542D File Offset: 0x0003362D
		public TMP_Asset textAsset
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

		// Token: 0x17000152 RID: 338
		// (get) Token: 0x06000564 RID: 1380 RVA: 0x00035436 File Offset: 0x00033636
		// (set) Token: 0x06000565 RID: 1381 RVA: 0x0003543E File Offset: 0x0003363E
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

		// Token: 0x17000153 RID: 339
		// (get) Token: 0x06000566 RID: 1382 RVA: 0x00035447 File Offset: 0x00033647
		// (set) Token: 0x06000567 RID: 1383 RVA: 0x0003544F File Offset: 0x0003364F
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

		// Token: 0x17000154 RID: 340
		// (get) Token: 0x06000568 RID: 1384 RVA: 0x00035458 File Offset: 0x00033658
		// (set) Token: 0x06000569 RID: 1385 RVA: 0x00035460 File Offset: 0x00033660
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

		// Token: 0x0400051C RID: 1308
		[SerializeField]
		protected TextElementType m_ElementType;

		// Token: 0x0400051D RID: 1309
		[SerializeField]
		internal uint m_Unicode;

		// Token: 0x0400051E RID: 1310
		internal TMP_Asset m_TextAsset;

		// Token: 0x0400051F RID: 1311
		internal Glyph m_Glyph;

		// Token: 0x04000520 RID: 1312
		[SerializeField]
		internal uint m_GlyphIndex;

		// Token: 0x04000521 RID: 1313
		[SerializeField]
		internal float m_Scale;
	}
}
