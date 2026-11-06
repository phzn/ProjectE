using System;
using System.Diagnostics;

namespace UnityEngine.Rendering
{
	// Token: 0x02000100 RID: 256
	[DebuggerDisplay("{m_Value} ({m_OverrideState})")]
	[Serializable]
	public class ClampedFloatParameter : FloatParameter
	{
		// Token: 0x1700013A RID: 314
		// (get) Token: 0x0600081E RID: 2078 RVA: 0x00026D99 File Offset: 0x00024F99
		// (set) Token: 0x0600081F RID: 2079 RVA: 0x00026DA1 File Offset: 0x00024FA1
		public override float value
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

		// Token: 0x06000820 RID: 2080 RVA: 0x00026DBB File Offset: 0x00024FBB
		public ClampedFloatParameter(float value, float min, float max, bool overrideState = false)
			: base(value, overrideState)
		{
			this.min = min;
			this.max = max;
		}

		// Token: 0x040004E2 RID: 1250
		[NonSerialized]
		public float min;

		// Token: 0x040004E3 RID: 1251
		[NonSerialized]
		public float max;
	}
}
