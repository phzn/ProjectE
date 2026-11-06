using System;
using System.Diagnostics;

namespace UnityEngine.Rendering
{
	// Token: 0x020000F4 RID: 244
	[DebuggerDisplay("{m_Value} ({m_OverrideState})")]
	[Serializable]
	public class MinIntParameter : IntParameter
	{
		// Token: 0x17000130 RID: 304
		// (get) Token: 0x060007FD RID: 2045 RVA: 0x00026B98 File Offset: 0x00024D98
		// (set) Token: 0x060007FE RID: 2046 RVA: 0x00026BA0 File Offset: 0x00024DA0
		public override int value
		{
			get
			{
				return this.m_Value;
			}
			set
			{
				this.m_Value = Mathf.Max(value, this.min);
			}
		}

		// Token: 0x060007FF RID: 2047 RVA: 0x00026BB4 File Offset: 0x00024DB4
		public MinIntParameter(int value, int min, bool overrideState = false)
			: base(value, overrideState)
		{
			this.min = min;
		}

		// Token: 0x040004D6 RID: 1238
		[NonSerialized]
		public int min;
	}
}
