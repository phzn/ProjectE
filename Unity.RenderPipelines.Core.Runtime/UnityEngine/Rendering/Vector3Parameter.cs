using System;
using System.Diagnostics;

namespace UnityEngine.Rendering
{
	// Token: 0x02000108 RID: 264
	[DebuggerDisplay("{m_Value} ({m_OverrideState})")]
	[Serializable]
	public class Vector3Parameter : VolumeParameter<Vector3>
	{
		// Token: 0x06000833 RID: 2099 RVA: 0x000270AD File Offset: 0x000252AD
		public Vector3Parameter(Vector3 value, bool overrideState = false)
			: base(value, overrideState)
		{
		}

		// Token: 0x06000834 RID: 2100 RVA: 0x000270B8 File Offset: 0x000252B8
		public override void Interp(Vector3 from, Vector3 to, float t)
		{
			this.m_Value.x = from.x + (to.x - from.x) * t;
			this.m_Value.y = from.y + (to.y - from.y) * t;
			this.m_Value.z = from.z + (to.z - from.z) * t;
		}
	}
}
