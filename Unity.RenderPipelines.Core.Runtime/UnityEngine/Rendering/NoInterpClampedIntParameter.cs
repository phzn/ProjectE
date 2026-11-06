using System;
using System.Diagnostics;

namespace UnityEngine.Rendering
{
	// Token: 0x020000F9 RID: 249
	[DebuggerDisplay("{m_Value} ({m_OverrideState})")]
	[Serializable]
	public class NoInterpClampedIntParameter : VolumeParameter<int>
	{
		// Token: 0x17000135 RID: 309
		// (get) Token: 0x0600080C RID: 2060 RVA: 0x00026C87 File Offset: 0x00024E87
		// (set) Token: 0x0600080D RID: 2061 RVA: 0x00026C8F File Offset: 0x00024E8F
		public override int value
		{
			get
			{
				return this.m_Value;
			}
			set
			{
				this.m_Value = Mathf.Clamp(value, this.min, this.max);
			}
		}

		// Token: 0x0600080E RID: 2062 RVA: 0x00026CA9 File Offset: 0x00024EA9
		public NoInterpClampedIntParameter(int value, int min, int max, bool overrideState = false)
			: base(value, overrideState)
		{
			this.min = min;
			this.max = max;
		}

		// Token: 0x040004DC RID: 1244
		[NonSerialized]
		public int min;

		// Token: 0x040004DD RID: 1245
		[NonSerialized]
		public int max;
	}
}
