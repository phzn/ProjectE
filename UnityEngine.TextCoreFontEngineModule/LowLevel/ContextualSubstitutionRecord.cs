using System;
using UnityEngine.Bindings;
using UnityEngine.Scripting;

namespace UnityEngine.TextCore.LowLevel
{
	// Token: 0x0200002B RID: 43
	[UsedByNativeCode]
	[Serializable]
	internal struct ContextualSubstitutionRecord
	{
		// Token: 0x17000048 RID: 72
		// (get) Token: 0x06000173 RID: 371 RVA: 0x000051EC File Offset: 0x000033EC
		// (set) Token: 0x06000174 RID: 372 RVA: 0x00005204 File Offset: 0x00003404
		public GlyphIDSequence[] inputSequences
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

		// Token: 0x17000049 RID: 73
		// (get) Token: 0x06000175 RID: 373 RVA: 0x00005210 File Offset: 0x00003410
		// (set) Token: 0x06000176 RID: 374 RVA: 0x00005228 File Offset: 0x00003428
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

		// Token: 0x040000DB RID: 219
		[SerializeField]
		[NativeName("inputGlyphSequences")]
		private GlyphIDSequence[] m_InputGlyphSequences;

		// Token: 0x040000DC RID: 220
		[SerializeField]
		[NativeName("sequenceLookupRecords")]
		private SequenceLookupRecord[] m_SequenceLookupRecords;
	}
}
