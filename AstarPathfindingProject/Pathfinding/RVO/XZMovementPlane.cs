using System;
using Pathfinding.Util;
using Unity.Mathematics;
using UnityEngine;

namespace Pathfinding.RVO
{
	// Token: 0x020002BD RID: 701
	public struct XZMovementPlane : IMovementPlaneWrapper
	{
		// Token: 0x0600115B RID: 4443 RVA: 0x0006B13F File Offset: 0x0006933F
		public float2 ToPlane(float3 p)
		{
			return p.xz;
		}

		// Token: 0x0600115C RID: 4444 RVA: 0x0006B148 File Offset: 0x00069348
		public float2 ToPlane(float3 p, out float elevation)
		{
			elevation = p.y;
			return p.xz;
		}

		// Token: 0x0600115D RID: 4445 RVA: 0x0006B159 File Offset: 0x00069359
		public float3 ToWorld(float2 p, float elevation = 0f)
		{
			return new float3(p.x, elevation, p.y);
		}

		// Token: 0x0600115E RID: 4446 RVA: 0x000295B1 File Offset: 0x000277B1
		public Bounds ToWorld(Bounds bounds)
		{
			return bounds;
		}

		// Token: 0x0600115F RID: 4447 RVA: 0x00003788 File Offset: 0x00001988
		public void Set(NativeMovementPlane plane)
		{
		}

		// Token: 0x17000280 RID: 640
		// (get) Token: 0x06001160 RID: 4448 RVA: 0x0006B16D File Offset: 0x0006936D
		public float4x4 matrix
		{
			get
			{
				return float4x4.RotateX(math.radians(90f));
			}
		}
	}
}
