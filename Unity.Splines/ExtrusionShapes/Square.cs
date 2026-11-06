using System;
using Unity.Mathematics;

namespace UnityEngine.Splines.ExtrusionShapes
{
	// Token: 0x0200004E RID: 78
	[Serializable]
	public sealed class Square : IExtrudeShape
	{
		// Token: 0x170000AE RID: 174
		// (get) Token: 0x06000324 RID: 804 RVA: 0x00010246 File Offset: 0x0000E446
		public int SideCount
		{
			get
			{
				return 4;
			}
		}

		// Token: 0x06000325 RID: 805 RVA: 0x00010249 File Offset: 0x0000E449
		public float2 GetPosition(float t, int index)
		{
			return Square.k_Sides[index];
		}

		// Token: 0x040000FF RID: 255
		private static readonly float2[] k_Sides = new float2[]
		{
			new float2(-0.5f, -0.5f),
			new float2(0.5f, -0.5f),
			new float2(0.5f, 0.5f),
			new float2(-0.5f, 0.5f)
		};
	}
}
