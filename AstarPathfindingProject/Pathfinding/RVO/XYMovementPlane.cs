using System;
using Pathfinding.Util;
using Unity.Mathematics;
using UnityEngine;

namespace Pathfinding.RVO
{
	// Token: 0x020002BC RID: 700
	public struct XYMovementPlane : IMovementPlaneWrapper
	{
		// Token: 0x06001155 RID: 4437 RVA: 0x0006B0B7 File Offset: 0x000692B7
		public float2 ToPlane(float3 p)
		{
			return p.xy;
		}

		// Token: 0x06001156 RID: 4438 RVA: 0x0006B0C0 File Offset: 0x000692C0
		public float2 ToPlane(float3 p, out float elevation)
		{
			elevation = p.z;
			return p.xy;
		}

		// Token: 0x06001157 RID: 4439 RVA: 0x0006B0D1 File Offset: 0x000692D1
		public float3 ToWorld(float2 p, float elevation = 0f)
		{
			return new float3(p.x, p.y, elevation);
		}

		// Token: 0x06001158 RID: 4440 RVA: 0x0006B0E8 File Offset: 0x000692E8
		public Bounds ToWorld(Bounds bounds)
		{
			Vector3 center = bounds.center;
			Vector3 size = bounds.size;
			return new Bounds(new Vector3(center.x, center.z, center.y), new Vector3(size.x, size.z, size.y));
		}

		// Token: 0x1700027F RID: 639
		// (get) Token: 0x06001159 RID: 4441 RVA: 0x0006B138 File Offset: 0x00069338
		public float4x4 matrix
		{
			get
			{
				return float4x4.identity;
			}
		}

		// Token: 0x0600115A RID: 4442 RVA: 0x00003788 File Offset: 0x00001988
		public void Set(NativeMovementPlane plane)
		{
		}
	}
}
