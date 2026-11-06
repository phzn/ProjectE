using System;
using System.Diagnostics;

namespace UnityEngine.Rendering
{
	// Token: 0x02000116 RID: 278
	[DebuggerDisplay("{m_Value} ({m_OverrideState})")]
	[Serializable]
	public class MaterialParameter : VolumeParameter<Material>
	{
		// Token: 0x06000858 RID: 2136 RVA: 0x000275D6 File Offset: 0x000257D6
		public MaterialParameter(Material value, bool overrideState = false)
			: base(value, overrideState)
		{
		}
	}
}
