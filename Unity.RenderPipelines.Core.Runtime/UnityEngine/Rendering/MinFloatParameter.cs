using System;
using System.Diagnostics;

namespace UnityEngine.Rendering
{
	// Token: 0x020000FC RID: 252
	[DebuggerDisplay("{m_Value} ({m_OverrideState})")]
	[Serializable]
	public class MinFloatParameter : FloatParameter
	{
		// Token: 0x17000136 RID: 310
		// (get) Token: 0x06000812 RID: 2066 RVA: 0x00026CE5 File Offset: 0x00024EE5
		// (set) Token: 0x06000813 RID: 2067 RVA: 0x00026CED File Offset: 0x00024EED
		public override float value
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

		// Token: 0x06000814 RID: 2068 RVA: 0x00026D01 File Offset: 0x00024F01
		public MinFloatParameter(float value, float min, bool overrideState = false)
			: base(value, overrideState)
		{
			this.min = min;
		}

		// Token: 0x040004DE RID: 1246
		[NonSerialized]
		public float min;
	}
}
