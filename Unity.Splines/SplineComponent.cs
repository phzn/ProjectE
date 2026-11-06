using System;
using Unity.Mathematics;

namespace UnityEngine.Splines
{
	// Token: 0x0200001E RID: 30
	public abstract class SplineComponent : MonoBehaviour
	{
		// Token: 0x06000153 RID: 339 RVA: 0x00006787 File Offset: 0x00004987
		protected float3 GetAxis(SplineComponent.AlignAxis axis)
		{
			return this.m_AlignAxisToVector[(int)axis];
		}

		// Token: 0x0400006A RID: 106
		private readonly float3[] m_AlignAxisToVector = new float3[]
		{
			math.right(),
			math.up(),
			math.forward(),
			math.left(),
			math.down(),
			math.back()
		};

		// Token: 0x0200005D RID: 93
		public enum AlignAxis
		{
			// Token: 0x04000133 RID: 307
			[InspectorName("Object X+")]
			XAxis,
			// Token: 0x04000134 RID: 308
			[InspectorName("Object Y+")]
			YAxis,
			// Token: 0x04000135 RID: 309
			[InspectorName("Object Z+")]
			ZAxis,
			// Token: 0x04000136 RID: 310
			[InspectorName("Object X-")]
			NegativeXAxis,
			// Token: 0x04000137 RID: 311
			[InspectorName("Object Y-")]
			NegativeYAxis,
			// Token: 0x04000138 RID: 312
			[InspectorName("Object Z-")]
			NegativeZAxis
		}
	}
}
