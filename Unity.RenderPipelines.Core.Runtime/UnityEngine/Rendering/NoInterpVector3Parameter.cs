using System;
using System.Diagnostics;

namespace UnityEngine.Rendering
{
	// Token: 0x02000109 RID: 265
	[DebuggerDisplay("{m_Value} ({m_OverrideState})")]
	[Serializable]
	public class NoInterpVector3Parameter : VolumeParameter<Vector3>
	{
		// Token: 0x06000835 RID: 2101 RVA: 0x00027128 File Offset: 0x00025328
		public NoInterpVector3Parameter(Vector3 value, bool overrideState = false)
			: base(value, overrideState)
		{
		}
	}
}
