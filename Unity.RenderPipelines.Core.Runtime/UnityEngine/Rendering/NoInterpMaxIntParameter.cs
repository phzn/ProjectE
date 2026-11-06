using System;
using System.Diagnostics;

namespace UnityEngine.Rendering
{
	// Token: 0x020000F7 RID: 247
	[DebuggerDisplay("{m_Value} ({m_OverrideState})")]
	[Serializable]
	public class NoInterpMaxIntParameter : VolumeParameter<int>
	{
		// Token: 0x17000133 RID: 307
		// (get) Token: 0x06000806 RID: 2054 RVA: 0x00026C1F File Offset: 0x00024E1F
		// (set) Token: 0x06000807 RID: 2055 RVA: 0x00026C27 File Offset: 0x00024E27
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

		// Token: 0x06000808 RID: 2056 RVA: 0x00026C3B File Offset: 0x00024E3B
		public NoInterpMaxIntParameter(int value, int max, bool overrideState = false)
			: base(value, overrideState)
		{
			this.max = max;
		}

		// Token: 0x040004D9 RID: 1241
		[NonSerialized]
		public int max;
	}
}
