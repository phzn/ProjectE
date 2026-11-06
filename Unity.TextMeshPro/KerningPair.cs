using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace TMPro
{
	// Token: 0x02000036 RID: 54
	[Serializable]
	public class KerningPair
	{
		// Token: 0x17000056 RID: 86
		// (get) Token: 0x06000208 RID: 520 RVA: 0x0001C64A File Offset: 0x0001A84A
		// (set) Token: 0x06000209 RID: 521 RVA: 0x0001C652 File Offset: 0x0001A852
		public uint firstGlyph
		{
			get
			{
				return this.m_FirstGlyph;
			}
			set
			{
				this.m_FirstGlyph = value;
			}
		}

		// Token: 0x17000057 RID: 87
		// (get) Token: 0x0600020A RID: 522 RVA: 0x0001C65B File Offset: 0x0001A85B
		public GlyphValueRecord_Legacy firstGlyphAdjustments
		{
			get
			{
				return this.m_FirstGlyphAdjustments;
			}
		}

		// Token: 0x17000058 RID: 88
		// (get) Token: 0x0600020B RID: 523 RVA: 0x0001C663 File Offset: 0x0001A863
		// (set) Token: 0x0600020C RID: 524 RVA: 0x0001C66B File Offset: 0x0001A86B
		public uint secondGlyph
		{
			get
			{
				return this.m_SecondGlyph;
			}
			set
			{
				this.m_SecondGlyph = value;
			}
		}

		// Token: 0x17000059 RID: 89
		// (get) Token: 0x0600020D RID: 525 RVA: 0x0001C674 File Offset: 0x0001A874
		public GlyphValueRecord_Legacy secondGlyphAdjustments
		{
			get
			{
				return this.m_SecondGlyphAdjustments;
			}
		}

		// Token: 0x1700005A RID: 90
		// (get) Token: 0x0600020E RID: 526 RVA: 0x0001C67C File Offset: 0x0001A87C
		public bool ignoreSpacingAdjustments
		{
			get
			{
				return this.m_IgnoreSpacingAdjustments;
			}
		}

		// Token: 0x0600020F RID: 527 RVA: 0x0001C684 File Offset: 0x0001A884
		public KerningPair()
		{
			this.m_FirstGlyph = 0U;
			this.m_FirstGlyphAdjustments = default(GlyphValueRecord_Legacy);
			this.m_SecondGlyph = 0U;
			this.m_SecondGlyphAdjustments = default(GlyphValueRecord_Legacy);
		}

		// Token: 0x06000210 RID: 528 RVA: 0x0001C6B2 File Offset: 0x0001A8B2
		public KerningPair(uint left, uint right, float offset)
		{
			this.firstGlyph = left;
			this.m_SecondGlyph = right;
			this.xOffset = offset;
		}

		// Token: 0x06000211 RID: 529 RVA: 0x0001C6CF File Offset: 0x0001A8CF
		public KerningPair(uint firstGlyph, GlyphValueRecord_Legacy firstGlyphAdjustments, uint secondGlyph, GlyphValueRecord_Legacy secondGlyphAdjustments)
		{
			this.m_FirstGlyph = firstGlyph;
			this.m_FirstGlyphAdjustments = firstGlyphAdjustments;
			this.m_SecondGlyph = secondGlyph;
			this.m_SecondGlyphAdjustments = secondGlyphAdjustments;
		}

		// Token: 0x06000212 RID: 530 RVA: 0x0001C6F4 File Offset: 0x0001A8F4
		internal void ConvertLegacyKerningData()
		{
			this.m_FirstGlyphAdjustments.xAdvance = this.xOffset;
		}

		// Token: 0x040001E2 RID: 482
		[FormerlySerializedAs("AscII_Left")]
		[SerializeField]
		private uint m_FirstGlyph;

		// Token: 0x040001E3 RID: 483
		[SerializeField]
		private GlyphValueRecord_Legacy m_FirstGlyphAdjustments;

		// Token: 0x040001E4 RID: 484
		[FormerlySerializedAs("AscII_Right")]
		[SerializeField]
		private uint m_SecondGlyph;

		// Token: 0x040001E5 RID: 485
		[SerializeField]
		private GlyphValueRecord_Legacy m_SecondGlyphAdjustments;

		// Token: 0x040001E6 RID: 486
		[FormerlySerializedAs("XadvanceOffset")]
		public float xOffset;

		// Token: 0x040001E7 RID: 487
		internal static KerningPair empty = new KerningPair(0U, default(GlyphValueRecord_Legacy), 0U, default(GlyphValueRecord_Legacy));

		// Token: 0x040001E8 RID: 488
		[SerializeField]
		private bool m_IgnoreSpacingAdjustments;
	}
}
