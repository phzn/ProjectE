using System;
using System.Diagnostics;

namespace UnityEngine.Rendering
{
	// Token: 0x020000FF RID: 255
	[DebuggerDisplay("{m_Value} ({m_OverrideState})")]
	[Serializable]
	public class NoInterpMaxFloatParameter : VolumeParameter<float>
	{
		// Token: 0x17000139 RID: 313
		// (get) Token: 0x0600081B RID: 2075 RVA: 0x00026D6C File Offset: 0x00024F6C
		// (set) Token: 0x0600081C RID: 2076 RVA: 0x00026D74 File Offset: 0x00024F74
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

		// Token: 0x0600081D RID: 2077 RVA: 0x00026D88 File Offset: 0x00024F88
		public NoInterpMaxFloatParameter(float value, float max, bool overrideState = false)
			: base(value, overrideState)
		{
			this.max = max;
		}

		// Token: 0x040004E1 RID: 1249
		[NonSerialized]
		public float max;
	}
}
