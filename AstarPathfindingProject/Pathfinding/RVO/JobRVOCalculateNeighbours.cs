using System;
using Pathfinding.Jobs;
using Unity.Burst;
using Unity.Collections;
using Unity.Mathematics;

namespace Pathfinding.RVO
{
	// Token: 0x020002B4 RID: 692
	[BurstCompile(CompileSynchronously = false, FloatMode = FloatMode.Fast)]
	public struct JobRVOCalculateNeighbours<MovementPlaneWrapper> : IJobParallelForBatched where MovementPlaneWrapper : struct, IMovementPlaneWrapper
	{
		// Token: 0x1700027C RID: 636
		// (get) Token: 0x0600112E RID: 4398 RVA: 0x0001AA07 File Offset: 0x00018C07
		public bool allowBoundsChecks
		{
			get
			{
				return false;
			}
		}

		// Token: 0x0600112F RID: 4399 RVA: 0x00067EF8 File Offset: 0x000660F8
		public void Execute(int startIndex, int count)
		{
			NativeArray<float> nativeArray = new NativeArray<float>(50, Allocator.Temp, NativeArrayOptions.UninitializedMemory);
			for (int i = startIndex; i < startIndex + count; i++)
			{
				if (this.agentData.version[i].Valid)
				{
					this.CalculateNeighbours(i, this.outNeighbours, nativeArray);
				}
			}
		}

		// Token: 0x06001130 RID: 4400 RVA: 0x00067F48 File Offset: 0x00066148
		private void CalculateNeighbours(int agentIndex, NativeArray<int> neighbours, NativeArray<float> neighbourDistances)
		{
			int num = math.min(50, this.agentData.maxNeighbours[agentIndex]);
			int num2 = agentIndex * 50;
			int num3 = this.quadtree.QueryKNearest(new RVOQuadtreeBurst.QuadtreeQuery
			{
				position = this.agentData.position[agentIndex],
				speed = this.agentData.maxSpeed[agentIndex],
				agentRadius = this.agentData.radius[agentIndex],
				timeHorizon = this.agentData.agentTimeHorizon[agentIndex],
				outputStartIndex = num2,
				maxCount = num,
				result = neighbours,
				layerMask = this.agentData.collidesWith[agentIndex],
				layers = this.agentData.layer,
				resultDistances = neighbourDistances
			});
			this.output.numNeighbours[agentIndex] = num3;
			MovementPlaneWrapper movementPlaneWrapper = default(MovementPlaneWrapper);
			movementPlaneWrapper.Set(this.agentData.movementPlane[agentIndex]);
			float num4;
			movementPlaneWrapper.ToPlane(this.agentData.position[agentIndex], out num4);
			for (int i = 0; i < num3; i++)
			{
				int num5 = neighbours[num2 + i];
				if (num5 == -1)
				{
					throw new Exception("Invalid neighbour index");
				}
				float num6;
				movementPlaneWrapper.ToPlane(this.agentData.position[num5], out num6);
				float num7 = math.min(num4 + this.agentData.height[agentIndex], num6 + this.agentData.height[num5]);
				float num8 = math.max(num4, num6);
				if ((num7 < num8) | (num5 == agentIndex))
				{
					num3--;
					neighbours[num2 + i] = neighbours[num2 + num3];
					i--;
				}
			}
			if (num3 < 50)
			{
				neighbours[num2 + num3] = -1;
			}
		}

		// Token: 0x04000C79 RID: 3193
		[ReadOnly]
		public SimulatorBurst.AgentData agentData;

		// Token: 0x04000C7A RID: 3194
		[ReadOnly]
		public RVOQuadtreeBurst quadtree;

		// Token: 0x04000C7B RID: 3195
		public NativeArray<int> outNeighbours;

		// Token: 0x04000C7C RID: 3196
		[WriteOnly]
		public SimulatorBurst.AgentOutputData output;
	}
}
