using System;
using UnityEngine;
using UnityEngine.TextCore.LowLevel;

namespace TMPro
{
	// Token: 0x0200003D RID: 61
	[Serializable]
	public class TMP_GlyphPairAdjustmentRecord
	{
		// Token: 0x17000062 RID: 98
		// (get) Token: 0x06000239 RID: 569 RVA: 0x0001CFDD File Offset: 0x0001B1DD
		// (set) Token: 0x0600023A RID: 570 RVA: 0x0001CFE5 File Offset: 0x0001B1E5
		public TMP_GlyphAdjustmentRecord firstAdjustmentRecord
		{
			get
			{
				return this.m_FirstAdjustmentRecord;
			}
			set
			{
				this.m_FirstAdjustmentRecord = value;
			}
		}

		// Token: 0x17000063 RID: 99
		// (get) Token: 0x0600023B RID: 571 RVA: 0x0001CFEE File Offset: 0x0001B1EE
		// (set) Token: 0x0600023C RID: 572 RVA: 0x0001CFF6 File Offset: 0x0001B1F6
		public TMP_GlyphAdjustmentRecord secondAdjustmentRecord
		{
			get
			{
				return this.m_SecondAdjustmentRecord;
			}
			set
			{
				this.m_SecondAdjustmentRecord = value;
			}
		}

		// Token: 0x17000064 RID: 100
		// (get) Token: 0x0600023D RID: 573 RVA: 0x0001CFFF File Offset: 0x0001B1FF
		// (set) Token: 0x0600023E RID: 574 RVA: 0x0001D007 File Offset: 0x0001B207
		public FontFeatureLookupFlags featureLookupFlags
		{
			get
			{
				return this.m_FeatureLookupFlags;
			}
			set
			{
				this.m_FeatureLookupFlags = value;
			}
		}

		// Token: 0x0600023F RID: 575 RVA: 0x0001D010 File Offset: 0x0001B210
		public TMP_GlyphPairAdjustmentRecord(TMP_GlyphAdjustmentRecord firstAdjustmentRecord, TMP_GlyphAdjustmentRecord secondAdjustmentRecord)
		{
			this.m_FirstAdjustmentRecord = firstAdjustmentRecord;
			this.m_SecondAdjustmentRecord = secondAdjustmentRecord;
			this.m_FeatureLookupFlags = FontFeatureLookupFlags.None;
		}

		// Token: 0x06000240 RID: 576 RVA: 0x0001D02D File Offset: 0x0001B22D
		internal TMP_GlyphPairAdjustmentRecord(GlyphPairAdjustmentRecord glyphPairAdjustmentRecord)
		{
			this.m_FirstAdjustmentRecord = new TMP_GlyphAdjustmentRecord(glyphPairAdjustmentRecord.firstAdjustmentRecord);
			this.m_SecondAdjustmentRecord = new TMP_GlyphAdjustmentRecord(glyphPairAdjustmentRecord.secondAdjustmentRecord);
			this.m_FeatureLookupFlags = FontFeatureLookupFlags.None;
		}

		// Token: 0x040001F8 RID: 504
		[SerializeField]
		internal TMP_GlyphAdjustmentRecord m_FirstAdjustmentRecord;

		// Token: 0x040001F9 RID: 505
		[SerializeField]
		internal TMP_GlyphAdjustmentRecord m_SecondAdjustmentRecord;

		// Token: 0x040001FA RID: 506
		[SerializeField]
		internal FontFeatureLookupFlags m_FeatureLookupFlags;
	}
}
