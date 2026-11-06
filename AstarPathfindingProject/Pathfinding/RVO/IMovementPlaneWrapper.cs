using System;
using Pathfinding.Util;
using Unity.Mathematics;
using UnityEngine;

namespace Pathfinding.RVO
{
	// Token: 0x020002BB RID: 699
	public interface IMovementPlaneWrapper
	{
		// Token: 0x0600114F RID: 4431
		float2 ToPlane(float3 p);

		// Token: 0x06001150 RID: 4432
		float2 ToPlane(float3 p, out float elevation);

		// Token: 0x06001151 RID: 4433
		float3 ToWorld(float2 p, float elevation = 0f);

		// Token: 0x06001152 RID: 4434
		Bounds ToWorld(Bounds bounds);

		// Token: 0x1700027E RID: 638
		// (get) Token: 0x06001153 RID: 4435
		float4x4 matrix { get; }

		// Token: 0x06001154 RID: 4436
		void Set(NativeMovementPlane plane);
	}
}
