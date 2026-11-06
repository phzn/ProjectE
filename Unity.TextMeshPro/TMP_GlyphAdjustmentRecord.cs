using System;
using UnityEngine;
using UnityEngine.TextCore.LowLevel;

namespace TMPro
{
	// Token: 0x0200003C RID: 60
	[Serializable]
	public struct TMP_GlyphAdjustmentRecord
	{
		// Token: 0x17000060 RID: 96
		// (get) Token: 0x06000233 RID: 563 RVA: 0x0001CF8A File Offset: 0x0001B18A
		// (set) Token: 0x06000234 RID: 564 RVA: 0x0001CF92 File Offset: 0x0001B192
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

		// Token: 0x17000061 RID: 97
		// (get) Token: 0x06000235 RID: 565 RVA: 0x0001CF9B File Offset: 0x0001B19B
		// (set) Token: 0x06000236 RID: 566 RVA: 0x0001CFA3 File Offset: 0x0001B1A3
		public TMP_GlyphValueRecord glyphValueRecord
		{
			get
			{
				return this.m_GlyphValueRecord;
			}
			set
			{
				this.m_GlyphValueRecord = value;
			}
		}

		// Token: 0x06000237 RID: 567 RVA: 0x0001CFAC File Offset: 0x0001B1AC
		public TMP_GlyphAdjustmentRecord(uint glyphIndex, TMP_GlyphValueRecord glyphValueRecord)
		{
			this.m_GlyphIndex = glyphIndex;
			this.m_GlyphValueRecord = glyphValueRecord;
		}

		// Token: 0x06000238 RID: 568 RVA: 0x0001CFBC File Offset: 0x0001B1BC
		internal TMP_GlyphAdjustmentRecord(GlyphAdjustmentRecord adjustmentRecord)
		{
			this.m_GlyphIndex = adjustmentRecord.glyphIndex;
			this.m_GlyphValueRecord = new TMP_GlyphValueRecord(adjustmentRecord.glyphValueRecord);
		}

		// Token: 0x040001F6 RID: 502
		[SerializeField]
		internal uint m_GlyphIndex;

		// Token: 0x040001F7 RID: 503
		[SerializeField]
		internal TMP_GlyphValueRecord m_GlyphValueRecord;
	}
}
