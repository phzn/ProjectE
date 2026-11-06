using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.TextCore.LowLevel;

namespace UnityEngine.TextCore.Text
{
	// Token: 0x0200000F RID: 15
	[Serializable]
	public class FontFeatureTable
	{
		// Token: 0x1700001F RID: 31
		// (get) Token: 0x0600009C RID: 156 RVA: 0x000064A0 File Offset: 0x000046A0
		// (set) Token: 0x0600009D RID: 157 RVA: 0x000064B8 File Offset: 0x000046B8
		internal List<MultipleSubstitutionRecord> multipleSubstitutionRecords
		{
			get
			{
				return this.m_MultipleSubstitutionRecords;
			}
			set
			{
				this.m_MultipleSubstitutionRecords = value;
			}
		}

		// Token: 0x17000020 RID: 32
		// (get) Token: 0x0600009E RID: 158 RVA: 0x000064C4 File Offset: 0x000046C4
		// (set) Token: 0x0600009F RID: 159 RVA: 0x000064DC File Offset: 0x000046DC
		internal List<LigatureSubstitutionRecord> ligatureRecords
		{
			get
			{
				return this.m_LigatureSubstitutionRecords;
			}
			set
			{
				this.m_LigatureSubstitutionRecords = value;
			}
		}

		// Token: 0x17000021 RID: 33
		// (get) Token: 0x060000A0 RID: 160 RVA: 0x000064E8 File Offset: 0x000046E8
		// (set) Token: 0x060000A1 RID: 161 RVA: 0x00006500 File Offset: 0x00004700
		internal List<GlyphPairAdjustmentRecord> glyphPairAdjustmentRecords
		{
			get
			{
				return this.m_GlyphPairAdjustmentRecords;
			}
			set
			{
				this.m_GlyphPairAdjustmentRecords = value;
			}
		}

		// Token: 0x17000022 RID: 34
		// (get) Token: 0x060000A2 RID: 162 RVA: 0x0000650C File Offset: 0x0000470C
		// (set) Token: 0x060000A3 RID: 163 RVA: 0x00006524 File Offset: 0x00004724
		internal List<MarkToBaseAdjustmentRecord> MarkToBaseAdjustmentRecords
		{
			get
			{
				return this.m_MarkToBaseAdjustmentRecords;
			}
			set
			{
				this.m_MarkToBaseAdjustmentRecords = value;
			}
		}

		// Token: 0x17000023 RID: 35
		// (get) Token: 0x060000A4 RID: 164 RVA: 0x00006530 File Offset: 0x00004730
		// (set) Token: 0x060000A5 RID: 165 RVA: 0x00006548 File Offset: 0x00004748
		internal List<MarkToMarkAdjustmentRecord> MarkToMarkAdjustmentRecords
		{
			get
			{
				return this.m_MarkToMarkAdjustmentRecords;
			}
			set
			{
				this.m_MarkToMarkAdjustmentRecords = value;
			}
		}

		// Token: 0x060000A6 RID: 166 RVA: 0x00006554 File Offset: 0x00004754
		internal FontFeatureTable()
		{
			this.m_LigatureSubstitutionRecords = new List<LigatureSubstitutionRecord>();
			this.m_LigatureSubstitutionRecordLookup = new Dictionary<uint, List<LigatureSubstitutionRecord>>();
			this.m_GlyphPairAdjustmentRecords = new List<GlyphPairAdjustmentRecord>();
			this.m_GlyphPairAdjustmentRecordLookup = new Dictionary<uint, GlyphPairAdjustmentRecord>();
			this.m_MarkToBaseAdjustmentRecords = new List<MarkToBaseAdjustmentRecord>();
			this.m_MarkToBaseAdjustmentRecordLookup = new Dictionary<uint, MarkToBaseAdjustmentRecord>();
			this.m_MarkToMarkAdjustmentRecords = new List<MarkToMarkAdjustmentRecord>();
			this.m_MarkToMarkAdjustmentRecordLookup = new Dictionary<uint, MarkToMarkAdjustmentRecord>();
		}

		// Token: 0x060000A7 RID: 167 RVA: 0x000065C4 File Offset: 0x000047C4
		public void SortGlyphPairAdjustmentRecords()
		{
			bool flag = this.m_GlyphPairAdjustmentRecords.Count > 0;
			if (flag)
			{
				this.m_GlyphPairAdjustmentRecords = (from s in this.m_GlyphPairAdjustmentRecords
					orderby s.firstAdjustmentRecord.glyphIndex, s.secondAdjustmentRecord.glyphIndex
					select s).ToList<GlyphPairAdjustmentRecord>();
			}
		}

		// Token: 0x060000A8 RID: 168 RVA: 0x00006640 File Offset: 0x00004840
		public void SortMarkToBaseAdjustmentRecords()
		{
			bool flag = this.m_MarkToBaseAdjustmentRecords.Count > 0;
			if (flag)
			{
				this.m_MarkToBaseAdjustmentRecords = (from s in this.m_MarkToBaseAdjustmentRecords
					orderby s.baseGlyphID, s.markGlyphID
					select s).ToList<MarkToBaseAdjustmentRecord>();
			}
		}

		// Token: 0x060000A9 RID: 169 RVA: 0x000066BC File Offset: 0x000048BC
		public void SortMarkToMarkAdjustmentRecords()
		{
			bool flag = this.m_MarkToMarkAdjustmentRecords.Count > 0;
			if (flag)
			{
				this.m_MarkToMarkAdjustmentRecords = (from s in this.m_MarkToMarkAdjustmentRecords
					orderby s.baseMarkGlyphID, s.combiningMarkGlyphID
					select s).ToList<MarkToMarkAdjustmentRecord>();
			}
		}

		// Token: 0x0400006A RID: 106
		[SerializeField]
		internal List<MultipleSubstitutionRecord> m_MultipleSubstitutionRecords;

		// Token: 0x0400006B RID: 107
		[SerializeField]
		internal List<LigatureSubstitutionRecord> m_LigatureSubstitutionRecords;

		// Token: 0x0400006C RID: 108
		[SerializeField]
		internal List<GlyphPairAdjustmentRecord> m_GlyphPairAdjustmentRecords;

		// Token: 0x0400006D RID: 109
		[SerializeField]
		internal List<MarkToBaseAdjustmentRecord> m_MarkToBaseAdjustmentRecords;

		// Token: 0x0400006E RID: 110
		[SerializeField]
		internal List<MarkToMarkAdjustmentRecord> m_MarkToMarkAdjustmentRecords;

		// Token: 0x0400006F RID: 111
		internal Dictionary<uint, List<LigatureSubstitutionRecord>> m_LigatureSubstitutionRecordLookup;

		// Token: 0x04000070 RID: 112
		internal Dictionary<uint, GlyphPairAdjustmentRecord> m_GlyphPairAdjustmentRecordLookup;

		// Token: 0x04000071 RID: 113
		internal Dictionary<uint, MarkToBaseAdjustmentRecord> m_MarkToBaseAdjustmentRecordLookup;

		// Token: 0x04000072 RID: 114
		internal Dictionary<uint, MarkToMarkAdjustmentRecord> m_MarkToMarkAdjustmentRecordLookup;
	}
}
