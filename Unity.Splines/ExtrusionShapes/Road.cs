using System;
using Unity.Mathematics;

namespace UnityEngine.Splines.ExtrusionShapes
{
	// Token: 0x0200004F RID: 79
	[Serializable]
	public sealed class Road : IExtrudeShape
	{
		// Token: 0x170000AF RID: 175
		// (get) Token: 0x06000328 RID: 808 RVA: 0x000102D0 File Offset: 0x0000E4D0
		public int SideCount
		{
			get
			{
				return 3;
			}
		}

		// Token: 0x06000329 RID: 809 RVA: 0x000102D3 File Offset: 0x0000E4D3
		public float2 GetPosition(float t, int index)
		{
			return Road.k_Sides[3 - index];
		}

		// Token: 0x04000100 RID: 256
		private static readonly float2[] k_Sides = new float2[]
		{
			new float2(-0.6f, -0.1f),
			new float2(-0.5f, 0f),
			new float2(0.5f, 0f),
			new float2(0.6f, -0.1f)
		};
	}
}
