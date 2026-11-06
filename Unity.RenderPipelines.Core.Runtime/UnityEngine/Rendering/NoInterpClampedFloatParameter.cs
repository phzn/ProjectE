using System;
using System.Diagnostics;

namespace UnityEngine.Rendering
{
	// Token: 0x02000101 RID: 257
	[DebuggerDisplay("{m_Value} ({m_OverrideState})")]
	[Serializable]
	public class NoInterpClampedFloatParameter : VolumeParameter<float>
	{
		// Token: 0x1700013B RID: 315
		// (get) Token: 0x06000821 RID: 2081 RVA: 0x00026DD4 File Offset: 0x00024FD4
		// (set) Token: 0x06000822 RID: 2082 RVA: 0x00026DDC File Offset: 0x00024FDC
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

		// Token: 0x06000823 RID: 2083 RVA: 0x00026DF6 File Offset: 0x00024FF6
		public NoInterpClampedFloatParameter(float value, float min, float max, bool overrideState = false)
			: base(value, overrideState)
		{
			this.min = min;
			this.max = max;
		}

		// Token: 0x040004E4 RID: 1252
		[NonSerialized]
		public float min;

		// Token: 0x040004E5 RID: 1253
		[NonSerialized]
		public float max;
	}
}
