using System;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;

namespace Pathfinding.RVO
{
	// Token: 0x020002B0 RID: 688
	[BurstCompile(CompileSynchronously = false, FloatMode = FloatMode.Fast)]
	public struct JobRVOPreprocess<MovementPlaneWrapper> : IJob where MovementPlaneWrapper : struct, IMovementPlaneWrapper
	{
		// Token: 0x06001125 RID: 4389 RVA: 0x00067390 File Offset: 0x00065590
		public void Execute()
		{
			for (int i = this.startIndex; i < this.endIndex; i++)
			{
				if (this.agentData.version[i].Valid)
				{
					if (this.agentData.locked[i] & !this.agentData.manuallyControlled[i])
					{
						this.temporaryAgentData.desiredTargetPointInVelocitySpace[i] = float2.zero;
						this.temporaryAgentData.desiredVelocity[i] = float3.zero;
						this.temporaryAgentData.currentVelocity[i] = float3.zero;
					}
					else
					{
						MovementPlaneWrapper movementPlaneWrapper = default(MovementPlaneWrapper);
						movementPlaneWrapper.Set(this.agentData.movementPlane[i]);
						float2 @float = movementPlaneWrapper.ToPlane(this.agentData.targetPoint[i] - this.agentData.position[i]);
						this.temporaryAgentData.desiredTargetPointInVelocitySpace[i] = @float;
						float3 float2 = math.normalizesafe(this.previousOutput.targetPoint[i] - this.agentData.position[i], default(float3)) * this.previousOutput.speed[i];
						this.temporaryAgentData.desiredVelocity[i] = movementPlaneWrapper.ToWorld(math.normalizesafe(@float, default(float2)) * this.agentData.desiredSpeed[i], 0f);
						float3 float3 = math.normalizesafe(this.agentData.collisionNormal[i], default(float3));
						float num = math.dot(float2, float3);
						float2 -= math.min(0f, num) * float3;
						this.temporaryAgentData.currentVelocity[i] = float2;
					}
				}
			}
		}

		// Token: 0x04000C64 RID: 3172
		[ReadOnly]
		public SimulatorBurst.AgentData agentData;

		// Token: 0x04000C65 RID: 3173
		[ReadOnly]
		public SimulatorBurst.AgentOutputData previousOutput;

		// Token: 0x04000C66 RID: 3174
		[WriteOnly]
		public SimulatorBurst.TemporaryAgentData temporaryAgentData;

		// Token: 0x04000C67 RID: 3175
		public int startIndex;

		// Token: 0x04000C68 RID: 3176
		public int endIndex;
	}
}
