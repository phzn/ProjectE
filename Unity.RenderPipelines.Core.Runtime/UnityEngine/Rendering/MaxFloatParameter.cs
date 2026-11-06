using System;
using System.Diagnostics;

namespace UnityEngine.Rendering
{
	// Token: 0x020000FE RID: 254
	[DebuggerDisplay("{m_Value} ({m_OverrideState})")]
	[Serializable]
	public class MaxFloatParameter : FloatParameter
	{
		// Token: 0x17000138 RID: 312
		// (get) Token: 0x06000818 RID: 2072 RVA: 0x00026D3F File Offset: 0x00024F3F
		// (set) Token: 0x06000819 RID: 2073 RVA: 0x00026D47 File Offset: 0x00024F47
		public override float value
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

		// Token: 0x0600081A RID: 2074 RVA: 0x00026D5B File Offset: 0x00024F5B
		public MaxFloatParameter(float value, float max, bool overrideState = false)
			: base(value, overrideState)
		{
			this.max = max;
		}

		// Token: 0x040004E0 RID: 1248
		[NonSerialized]
		public float max;
	}
}
