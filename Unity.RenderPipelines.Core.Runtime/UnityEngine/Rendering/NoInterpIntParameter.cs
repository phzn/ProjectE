using System;
using System.Diagnostics;

namespace UnityEngine.Rendering
{
	// Token: 0x020000F3 RID: 243
	[DebuggerDisplay("{m_Value} ({m_OverrideState})")]
	[Serializable]
	public class NoInterpIntParameter : VolumeParameter<int>
	{
		// Token: 0x060007FC RID: 2044 RVA: 0x00026B8E File Offset: 0x00024D8E
		public NoInterpIntParameter(int value, bool overrideState = false)
			: base(value, overrideState)
		{
		}
	}
}
