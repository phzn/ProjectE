using System;
using UnityEngine.Bindings;
using UnityEngine.Scripting;

namespace UnityEngine.TextCore.LowLevel
{
	// Token: 0x02000026 RID: 38
	[UsedByNativeCode]
	[Serializable]
	internal struct MultipleSubstitutionRecord
	{
		// Token: 0x1700003F RID: 63
		// (get) Token: 0x06000161 RID: 353 RVA: 0x000050A8 File Offset: 0x000032A8
		// (set) Token: 0x06000162 RID: 354 RVA: 0x000050C0 File Offset: 0x000032C0
		public uint targetGlyphID
		{
			get
			{
				return this.m_TargetGlyphID;
			}
			set
			{
				this.m_TargetGlyphID = value;
			}
		}

		// Token: 0x17000040 RID: 64
		// (get) Token: 0x06000163 RID: 355 RVA: 0x000050CC File Offset: 0x000032CC
		// (set) Token: 0x06000164 RID: 356 RVA: 0x000050E4 File Offset: 0x000032E4
		public uint[] substituteGlyphIDs
		{
			get
			{
				return this.m_SubstituteGlyphIDs;
			}
			set
			{
				this.m_SubstituteGlyphIDs = value;
			}
		}

		// Token: 0x040000D2 RID: 210
		[NativeName("targetGlyphID")]
		[SerializeField]
		private uint m_TargetGlyphID;

		// Token: 0x040000D3 RID: 211
		[NativeName("substituteGlyphIDs")]
		[SerializeField]
		private uint[] m_SubstituteGlyphIDs;
	}
}
