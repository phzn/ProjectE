using System;
using System.Diagnostics;

namespace UnityEngine.Rendering
{
	// Token: 0x020000FA RID: 250
	[DebuggerDisplay("{m_Value} ({m_OverrideState})")]
	[Serializable]
	public class FloatParameter : VolumeParameter<float>
	{
		// Token: 0x0600080F RID: 2063 RVA: 0x00026CC2 File Offset: 0x00024EC2
		public FloatParameter(float value, bool overrideState = false)
			: base(value, overrideState)
		{
		}

		// Token: 0x06000810 RID: 2064 RVA: 0x00026CCC File Offset: 0x00024ECC
		public sealed override void Interp(float from, float to, float t)
		{
			this.m_Value = from + (to - from) * t;
		}
	}
}
