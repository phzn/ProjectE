using System;

namespace TMPro
{
	// Token: 0x0200003E RID: 62
	public struct GlyphPairKey
	{
		// Token: 0x06000241 RID: 577 RVA: 0x0001D060 File Offset: 0x0001B260
		public GlyphPairKey(uint firstGlyphIndex, uint secondGlyphIndex)
		{
			this.firstGlyphIndex = firstGlyphIndex;
			this.secondGlyphIndex = secondGlyphIndex;
			this.key = (secondGlyphIndex << 16) | firstGlyphIndex;
		}

		// Token: 0x06000242 RID: 578 RVA: 0x0001D07C File Offset: 0x0001B27C
		internal GlyphPairKey(TMP_GlyphPairAdjustmentRecord record)
		{
			this.firstGlyphIndex = record.firstAdjustmentRecord.glyphIndex;
			this.secondGlyphIndex = record.secondAdjustmentRecord.glyphIndex;
			this.key = (this.secondGlyphIndex << 16) | this.firstGlyphIndex;
		}

		// Token: 0x040001FB RID: 507
		public uint firstGlyphIndex;

		// Token: 0x040001FC RID: 508
		public uint secondGlyphIndex;

		// Token: 0x040001FD RID: 509
		public uint key;
	}
}
