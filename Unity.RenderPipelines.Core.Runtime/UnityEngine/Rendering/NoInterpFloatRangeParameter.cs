using System;
using System.Diagnostics;

namespace UnityEngine.Rendering
{
	// Token: 0x02000103 RID: 259
	[DebuggerDisplay("{m_Value} ({m_OverrideState})")]
	[Serializable]
	public class NoInterpFloatRangeParameter : VolumeParameter<Vector2>
	{
		// Token: 0x1700013D RID: 317
		// (get) Token: 0x06000828 RID: 2088 RVA: 0x00026EBB File Offset: 0x000250BB
		// (set) Token: 0x06000829 RID: 2089 RVA: 0x00026EC3 File Offset: 0x000250C3
		public override Vector2 value
		{
			get
			{
				return this.m_Value;
			}
			set
			{
				this.m_Value.x = Mathf.Max(value.x, this.min);
				this.m_Value.y = Mathf.Min(value.y, this.max);
			}
		}

		// Token: 0x0600082A RID: 2090 RVA: 0x00026EFD File Offset: 0x000250FD
		public NoInterpFloatRangeParameter(Vector2 value, float min, float max, bool overrideState = false)
			: base(value, overrideState)
		{
			this.min = min;
			this.max = max;
		}

		// Token: 0x040004E8 RID: 1256
		[NonSerialized]
		public float min;

		// Token: 0x040004E9 RID: 1257
		[NonSerialized]
		public float max;
	}
}
