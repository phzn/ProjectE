using System;
using System.Diagnostics;

namespace UnityEngine.Rendering
{
	// Token: 0x0200010B RID: 267
	[DebuggerDisplay("{m_Value} ({m_OverrideState})")]
	[Serializable]
	public class NoInterpVector4Parameter : VolumeParameter<Vector4>
	{
		// Token: 0x06000838 RID: 2104 RVA: 0x000271CD File Offset: 0x000253CD
		public NoInterpVector4Parameter(Vector4 value, bool overrideState = false)
			: base(value, overrideState)
		{
		}
	}
}
