using System;
using System.Diagnostics;

namespace UnityEngine.Rendering
{
	// Token: 0x020000FB RID: 251
	[DebuggerDisplay("{m_Value} ({m_OverrideState})")]
	[Serializable]
	public class NoInterpFloatParameter : VolumeParameter<float>
	{
		// Token: 0x06000811 RID: 2065 RVA: 0x00026CDB File Offset: 0x00024EDB
		public NoInterpFloatParameter(float value, bool overrideState = false)
			: base(value, overrideState)
		{
		}
	}
}
