using System;
using System.Diagnostics;

namespace UnityEngine.Rendering
{
	// Token: 0x020000F2 RID: 242
	[DebuggerDisplay("{m_Value} ({m_OverrideState})")]
	[Serializable]
	public class IntParameter : VolumeParameter<int>
	{
		// Token: 0x060007FA RID: 2042 RVA: 0x00026B72 File Offset: 0x00024D72
		public IntParameter(int value, bool overrideState = false)
			: base(value, overrideState)
		{
		}

		// Token: 0x060007FB RID: 2043 RVA: 0x00026B7C File Offset: 0x00024D7C
		public sealed override void Interp(int from, int to, float t)
		{
			this.m_Value = (int)((float)from + (float)(to - from) * t);
		}
	}
}
