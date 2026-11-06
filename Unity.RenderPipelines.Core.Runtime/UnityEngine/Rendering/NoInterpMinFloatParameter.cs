using System;
using System.Diagnostics;

namespace UnityEngine.Rendering
{
	// Token: 0x020000FD RID: 253
	[DebuggerDisplay("{m_Value} ({m_OverrideState})")]
	[Serializable]
	public class NoInterpMinFloatParameter : VolumeParameter<float>
	{
		// Token: 0x17000137 RID: 311
		// (get) Token: 0x06000815 RID: 2069 RVA: 0x00026D12 File Offset: 0x00024F12
		// (set) Token: 0x06000816 RID: 2070 RVA: 0x00026D1A File Offset: 0x00024F1A
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

		// Token: 0x06000817 RID: 2071 RVA: 0x00026D2E File Offset: 0x00024F2E
		public NoInterpMinFloatParameter(float value, float min, bool overrideState = false)
			: base(value, overrideState)
		{
			this.min = min;
		}

		// Token: 0x040004DF RID: 1247
		[NonSerialized]
		public float min;
	}
}
