using System;
using Pathfinding.ECS.RVO;
using Pathfinding.Jobs;
using Pathfinding.Util;
using Unity.Burst;
using Unity.Collections;
using Unity.Mathematics;

namespace Pathfinding.RVO
{
	// Token: 0x020002B2 RID: 690
	[BurstCompile(FloatMode = FloatMode.Fast)]
	public struct JobHorizonAvoidancePhase2<MovementPlaneWrapper> : IJobParallelForBatched where MovementPlaneWrapper : struct, IMovementPlaneWrapper
	{
		// Token: 0x1700027A RID: 634
		// (get) Token: 0x0600112A RID: 4394 RVA: 0x0001AA07 File Offset: 0x00018C07
		public bool allowBoundsChecks
		{
			get
			{
				return false;
			}
		}

		// Token: 0x0600112B RID: 4395 RVA: 0x00067AA4 File Offset: 0x00065CA4
		public void Execute(int startIndex, int count)
		{
			for (int i = startIndex; i < startIndex + count; i++)
			{
				if (this.versions[i].Valid && this.horizonAgentData.horizonSide[i] != 0)
				{
					if (this.horizonAgentData.horizonSide[i] == 2)
					{
						float num = 0f;
						NativeSlice<int> nativeSlice = this.neighbours.Slice<int>(i * 50, 50);
						int num2 = 0;
						while (num2 < nativeSlice.Length && nativeSlice[num2] != -1)
						{
							int num3 = nativeSlice[num2];
							float num4 = -(this.horizonAgentData.horizonMinAngle[num3] + this.horizonAgentData.horizonMaxAngle[num3]);
							num += num4;
							num2++;
						}
						float num5 = -(this.horizonAgentData.horizonMinAngle[i] + this.horizonAgentData.horizonMaxAngle[i]);
						num += num5;
						this.horizonAgentData.horizonSide[i] = ((num < 0f) ? (-1) : 1);
					}
					float2 @float;
					math.sincos((this.horizonAgentData.horizonSide[i] < 0) ? this.horizonAgentData.horizonMinAngle[i] : this.horizonAgentData.horizonMaxAngle[i], out @float.y, out @float.x);
					MovementPlaneWrapper movementPlaneWrapper = default(MovementPlaneWrapper);
					movementPlaneWrapper.Set(this.movementPlane[i]);
					this.desiredVelocity[i] = movementPlaneWrapper.ToWorld(math.length(this.desiredVelocity[i]) * @float, 0f);
					this.desiredTargetPointInVelocitySpace[i] = math.length(this.desiredTargetPointInVelocitySpace[i]) * @float;
				}
			}
		}

		// Token: 0x04000C6D RID: 3181
		[ReadOnly]
		public NativeArray<int> neighbours;

		// Token: 0x04000C6E RID: 3182
		[ReadOnly]
		public NativeArray<AgentIndex> versions;

		// Token: 0x04000C6F RID: 3183
		public NativeArray<float3> desiredVelocity;

		// Token: 0x04000C70 RID: 3184
		public NativeArray<float2> desiredTargetPointInVelocitySpace;

		// Token: 0x04000C71 RID: 3185
		[ReadOnly]
		public NativeArray<NativeMovementPlane> movementPlane;

		// Token: 0x04000C72 RID: 3186
		public SimulatorBurst.HorizonAgentData horizonAgentData;
	}
}
