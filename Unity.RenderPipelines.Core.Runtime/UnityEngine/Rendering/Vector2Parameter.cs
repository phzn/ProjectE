using System;
using System.Diagnostics;

namespace UnityEngine.Rendering
{
	// Token: 0x02000106 RID: 262
	[DebuggerDisplay("{m_Value} ({m_OverrideState})")]
	[Serializable]
	public class Vector2Parameter : VolumeParameter<Vector2>
	{
		// Token: 0x06000830 RID: 2096 RVA: 0x00027048 File Offset: 0x00025248
		public Vector2Parameter(Vector2 value, bool overrideState = false)
			: base(value, overrideState)
		{
		}

		// Token: 0x06000831 RID: 2097 RVA: 0x00027054 File Offset: 0x00025254
		public override void Interp(Vector2 from, Vector2 to, float t)
		{
			this.m_Value.x = from.x + (to.x - from.x) * t;
			this.m_Value.y = from.y + (to.y - from.y) * t;
		}
	}
}
