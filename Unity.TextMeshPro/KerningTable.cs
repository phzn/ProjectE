using System;
using System.Collections.Generic;
using System.Linq;

namespace TMPro
{
	// Token: 0x02000037 RID: 55
	[Serializable]
	public class KerningTable
	{
		// Token: 0x06000214 RID: 532 RVA: 0x0001C733 File Offset: 0x0001A933
		public KerningTable()
		{
			this.kerningPairs = new List<KerningPair>();
		}

		// Token: 0x06000215 RID: 533 RVA: 0x0001C748 File Offset: 0x0001A948
		public void AddKerningPair()
		{
			if (this.kerningPairs.Count == 0)
			{
				this.kerningPairs.Add(new KerningPair(0U, 0U, 0f));
				return;
			}
			uint firstGlyph = this.kerningPairs.Last<KerningPair>().firstGlyph;
			uint secondGlyph = this.kerningPairs.Last<KerningPair>().secondGlyph;
			float xOffset = this.kerningPairs.Last<KerningPair>().xOffset;
			this.kerningPairs.Add(new KerningPair(firstGlyph, secondGlyph, xOffset));
		}

		// Token: 0x06000216 RID: 534 RVA: 0x0001C7C0 File Offset: 0x0001A9C0
		public int AddKerningPair(uint first, uint second, float offset)
		{
			if (this.kerningPairs.FindIndex((KerningPair item) => item.firstGlyph == first && item.secondGlyph == second) == -1)
			{
				this.kerningPairs.Add(new KerningPair(first, second, offset));
				return 0;
			}
			return -1;
		}

		// Token: 0x06000217 RID: 535 RVA: 0x0001C81C File Offset: 0x0001AA1C
		public int AddGlyphPairAdjustmentRecord(uint first, GlyphValueRecord_Legacy firstAdjustments, uint second, GlyphValueRecord_Legacy secondAdjustments)
		{
			if (this.kerningPairs.FindIndex((KerningPair item) => item.firstGlyph == first && item.secondGlyph == second) == -1)
			{
				this.kerningPairs.Add(new KerningPair(first, firstAdjustments, second, secondAdjustments));
				return 0;
			}
			return -1;
		}

		// Token: 0x06000218 RID: 536 RVA: 0x0001C87C File Offset: 0x0001AA7C
		public void RemoveKerningPair(int left, int right)
		{
			int num = this.kerningPairs.FindIndex((KerningPair item) => (ulong)item.firstGlyph == (ulong)((long)left) && (ulong)item.secondGlyph == (ulong)((long)right));
			if (num != -1)
			{
				this.kerningPairs.RemoveAt(num);
			}
		}

		// Token: 0x06000219 RID: 537 RVA: 0x0001C8C5 File Offset: 0x0001AAC5
		public void RemoveKerningPair(int index)
		{
			this.kerningPairs.RemoveAt(index);
		}

		// Token: 0x0600021A RID: 538 RVA: 0x0001C8D4 File Offset: 0x0001AAD4
		public void SortKerningPairs()
		{
			if (this.kerningPairs.Count > 0)
			{
				this.kerningPairs = (from s in this.kerningPairs
					orderby s.firstGlyph, s.secondGlyph
					select s).ToList<KerningPair>();
			}
		}

		// Token: 0x040001E9 RID: 489
		public List<KerningPair> kerningPairs;
	}
}
