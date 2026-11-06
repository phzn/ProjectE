using System;
using System.Diagnostics;

namespace UnityEngine.Rendering
{
	// Token: 0x02000102 RID: 258
	[DebuggerDisplay("{m_Value} ({m_OverrideState})")]
	[Serializable]
	public class FloatRangeParameter : VolumeParameter<Vector2>
	{
		// Token: 0x1700013C RID: 316
		// (get) Token: 0x06000824 RID: 2084 RVA: 0x00026E0F File Offset: 0x0002500F
		// (set) Token: 0x06000825 RID: 2085 RVA: 0x00026E17 File Offset: 0x00025017
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

		// Token: 0x06000826 RID: 2086 RVA: 0x00026E51 File Offset: 0x00025051
		public FloatRangeParameter(Vector2 value, float min, float max, bool overrideState = false)
			: base(value, overrideState)
		{
			this.min = min;
			this.max = max;
		}

		// Token: 0x06000827 RID: 2087 RVA: 0x00026E6C File Offset: 0x0002506C
		public override void Interp(Vector2 from, Vector2 to, float t)
		{
			this.m_Value.x = from.x + (to.x - from.x) * t;
			this.m_Value.y = from.y + (to.y - from.y) * t;
		}

		// Token: 0x040004E6 RID: 1254
		[NonSerialized]
		public float min;

		// Token: 0x040004E7 RID: 1255
		[NonSerialized]
		public float max;
	}
}
