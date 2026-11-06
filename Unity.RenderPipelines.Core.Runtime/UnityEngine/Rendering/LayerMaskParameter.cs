using System;
using System.Diagnostics;

namespace UnityEngine.Rendering
{
	// Token: 0x020000F1 RID: 241
	[DebuggerDisplay("{m_Value} ({m_OverrideState})")]
	[Serializable]
	public class LayerMaskParameter : VolumeParameter<LayerMask>
	{
		// Token: 0x060007F9 RID: 2041 RVA: 0x00026B68 File Offset: 0x00024D68
		public LayerMaskParameter(LayerMask value, bool overrideState = false)
			: base(value, overrideState)
		{
		}
	}
}
