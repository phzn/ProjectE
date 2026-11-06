using System;
using UnityEngine.Bindings;
using UnityEngine.Scripting;

namespace UnityEngine.TextCore.LowLevel
{
	// Token: 0x0200002C RID: 44
	[UsedByNativeCode]
	[Serializable]
	internal struct ChainingContextualSubstitutionRecord
	{
		// Token: 0x1700004A RID: 74
		// (get) Token: 0x06000177 RID: 375 RVA: 0x00005234 File Offset: 0x00003434
		// (set) Token: 0x06000178 RID: 376 RVA: 0x0000524C File Offset: 0x0000344C
		public GlyphIDSequence[] backtrackGlyphSequences
		{
			get
			{
				return this.m_BacktrackGlyphSequences;
			}
			set
			{
				this.m_BacktrackGlyphSequences = value;
			}
		}

		// Token: 0x1700004B RID: 75
		// (get) Token: 0x06000179 RID: 377 RVA: 0x00005258 File Offset: 0x00003458
		// (set) Token: 0x0600017A RID: 378 RVA: 0x00005270 File Offset: 0x00003470
		public GlyphIDSequence[] inputGlyphSequences
		{
			get
			{
				return this.m_InputGlyphSequences;
			}
			set
			{
				this.m_InputGlyphSequences = value;
			}
		}

		// Token: 0x1700004C RID: 76
		// (get) Token: 0x0600017B RID: 379 RVA: 0x0000527C File Offset: 0x0000347C
		// (set) Token: 0x0600017C RID: 380 RVA: 0x00005294 File Offset: 0x00003494
		public GlyphIDSequence[] lookaheadGlyphSequences
		{
			get
			{
				return this.m_LookaheadGlyphSequences;
			}
			set
			{
				this.m_LookaheadGlyphSequences = value;
			}
		}

		// Token: 0x1700004D RID: 77
		// (get) Token: 0x0600017D RID: 381 RVA: 0x000052A0 File Offset: 0x000034A0
		// (set) Token: 0x0600017E RID: 382 RVA: 0x000052B8 File Offset: 0x000034B8
		public SequenceLookupRecord[] sequenceLookupRecords
		{
			get
			{
				return this.m_SequenceLookupRecords;
			}
			set
			{
				this.m_SequenceLookupRecords = value;
			}
		}

		// Token: 0x040000DD RID: 221
		[SerializeField]
		[NativeName("backtrackGlyphSequences")]
		private GlyphIDSequence[] m_BacktrackGlyphSequences;

		// Token: 0x040000DE RID: 222
		[SerializeField]
		[NativeName("inputGlyphSequences")]
		private GlyphIDSequence[] m_InputGlyphSequences;

		// Token: 0x040000DF RID: 223
		[NativeName("lookaheadGlyphSequences")]
		[SerializeField]
		private GlyphIDSequence[] m_LookaheadGlyphSequences;

		// Token: 0x040000E0 RID: 224
		[SerializeField]
		[NativeName("sequenceLookupRecords")]
		private SequenceLookupRecord[] m_SequenceLookupRecords;
	}
}
