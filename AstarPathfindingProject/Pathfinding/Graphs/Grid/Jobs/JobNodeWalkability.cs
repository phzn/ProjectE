using System;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;

namespace Pathfinding.Graphs.Grid.Jobs
{
	// Token: 0x0200022E RID: 558
	[BurstCompile(FloatMode = FloatMode.Fast)]
	public struct JobNodeWalkability : IJob
	{
		// Token: 0x06000E3E RID: 3646 RVA: 0x000580E8 File Offset: 0x000562E8
		public void Execute()
		{
			float num = math.cos(math.radians(this.maxSlope));
			float4 @float = new float4(this.up.x, this.up.y, this.up.z, 0f);
			float3 xyz = @float.xyz;
			for (int i = 0; i < this.nodeNormals.Length; i++)
			{
				bool flag = math.any(this.nodeNormals[i]);
				bool flag2 = flag;
				if (!flag && !this.unwalkableWhenNoGround && i < this.layerStride)
				{
					flag2 = true;
					this.nodeNormals[i] = @float;
				}
				if (flag2 && this.useRaycastNormal && flag && math.dot(this.nodeNormals[i], @float) < num)
				{
					flag2 = false;
				}
				if (flag2 && i + this.layerStride < this.nodeNormals.Length && math.any(this.nodeNormals[i + this.layerStride]))
				{
					flag2 = math.dot(xyz, this.nodePositions[i + this.layerStride] - this.nodePositions[i]) >= this.characterHeight;
				}
				this.nodeWalkable[i] = flag2;
			}
		}

		// Token: 0x04000A7B RID: 2683
		public bool useRaycastNormal;

		// Token: 0x04000A7C RID: 2684
		public float maxSlope;

		// Token: 0x04000A7D RID: 2685
		public Vector3 up;

		// Token: 0x04000A7E RID: 2686
		public bool unwalkableWhenNoGround;

		// Token: 0x04000A7F RID: 2687
		public float characterHeight;

		// Token: 0x04000A80 RID: 2688
		public int layerStride;

		// Token: 0x04000A81 RID: 2689
		[ReadOnly]
		public NativeArray<float3> nodePositions;

		// Token: 0x04000A82 RID: 2690
		public NativeArray<float4> nodeNormals;

		// Token: 0x04000A83 RID: 2691
		[WriteOnly]
		public NativeArray<bool> nodeWalkable;
	}
}
