using System;
using System.Diagnostics;

namespace UnityEngine.Rendering
{
	// Token: 0x020000F5 RID: 245
	[DebuggerDisplay("{m_Value} ({m_OverrideState})")]
	[Serializable]
	public class NoInterpMinIntParameter : VolumeParameter<int>
	{
		// Token: 0x17000131 RID: 305
		// (get) Token: 0x06000800 RID: 2048 RVA: 0x00026BC5 File Offset: 0x00024DC5
		// (set) Token: 0x06000801 RID: 2049 RVA: 0x00026BCD File Offset: 0x00024DCD
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

		// Token: 0x06000802 RID: 2050 RVA: 0x00026BE1 File Offset: 0x00024DE1
		public NoInterpMinIntParameter(int value, int min, bool overrideState = false)
			: base(value, overrideState)
		{
			this.min = min;
		}

		// Token: 0x040004D7 RID: 1239
		[NonSerialized]
		public int min;
	}
}
