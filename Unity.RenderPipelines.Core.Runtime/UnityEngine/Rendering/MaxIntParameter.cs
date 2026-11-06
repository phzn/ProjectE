using System;
using System.Diagnostics;

namespace UnityEngine.Rendering
{
	// Token: 0x020000F6 RID: 246
	[DebuggerDisplay("{m_Value} ({m_OverrideState})")]
	[Serializable]
	public class MaxIntParameter : IntParameter
	{
		// Token: 0x17000132 RID: 306
		// (get) Token: 0x06000803 RID: 2051 RVA: 0x00026BF2 File Offset: 0x00024DF2
		// (set) Token: 0x06000804 RID: 2052 RVA: 0x00026BFA File Offset: 0x00024DFA
		public override int value
		{
			get
			{
				return this.m_Value;
			}
			set
			{
				this.m_Value = Mathf.Min(value, this.max);
			}
		}

		// Token: 0x06000805 RID: 2053 RVA: 0x00026C0E File Offset: 0x00024E0E
		public MaxIntParameter(int value, int max, bool overrideState = false)
			: base(value, overrideState)
		{
			this.max = max;
		}

		// Token: 0x040004D8 RID: 1240
		[NonSerialized]
		public int max;
	}
}
