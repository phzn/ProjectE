using System;
using System.Diagnostics;

namespace UnityEngine.Rendering
{
	// Token: 0x02000107 RID: 263
	[DebuggerDisplay("{m_Value} ({m_OverrideState})")]
	[Serializable]
	public class NoInterpVector2Parameter : VolumeParameter<Vector2>
	{
		// Token: 0x06000832 RID: 2098 RVA: 0x000270A3 File Offset: 0x000252A3
		public NoInterpVector2Parameter(Vector2 value, bool overrideState = false)
			: base(value, overrideState)
		{
		}
	}
}
