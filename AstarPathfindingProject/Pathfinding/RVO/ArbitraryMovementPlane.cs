using System;
using Pathfinding.Util;
using Unity.Mathematics;
using UnityEngine;

namespace Pathfinding.RVO
{
	// Token: 0x020002BE RID: 702
	public struct ArbitraryMovementPlane : IMovementPlaneWrapper
	{
		// Token: 0x06001161 RID: 4449 RVA: 0x0006B17E File Offset: 0x0006937E
		public float2 ToPlane(float3 p)
		{
			return this.plane.ToPlane(p);
		}

		// Token: 0x06001162 RID: 4450 RVA: 0x0006B18C File Offset: 0x0006938C
		public float2 ToPlane(float3 p, out float elevation)
		{
			return this.plane.ToPlane(p, out elevation);
		}

		// Token: 0x06001163 RID: 4451 RVA: 0x0006B19B File Offset: 0x0006939B
		public float3 ToWorld(float2 p, float elevation = 0f)
		{
			return this.plane.ToWorld(p, elevation);
		}

		// Token: 0x06001164 RID: 4452 RVA: 0x0006B1AA File Offset: 0x000693AA
		public Bounds ToWorld(Bounds bounds)
		{
			return this.plane.ToWorld(bounds);
		}

		// Token: 0x06001165 RID: 4453 RVA: 0x0006B1B8 File Offset: 0x000693B8
		public void Set(NativeMovementPlane plane)
		{
			this.plane = plane;
		}

		// Token: 0x17000281 RID: 641
		// (get) Token: 0x06001166 RID: 4454 RVA: 0x0006B1C4 File Offset: 0x000693C4
		public float4x4 matrix
		{
			get
			{
				return math.mul(float4x4.TRS(0, this.plane.rotation, 1), new float4x4(new float4(1f, 0f, 0f, 0f), new float4(0f, 0f, 1f, 0f), new float4(0f, 1f, 0f, 0f), new float4(0f, 0f, 0f, 1f)));
			}
		}

		// Token: 0x04000C96 RID: 3222
		private NativeMovementPlane plane;
	}
}
