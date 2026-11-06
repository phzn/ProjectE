using System;
using Pathfinding.Jobs;
using Unity.Burst;
using Unity.Collections;
using Unity.Mathematics;

namespace Pathfinding.RVO
{
	// Token: 0x020002B3 RID: 691
	[BurstCompile(FloatMode = FloatMode.Fast)]
	public struct JobHardCollisions<MovementPlaneWrapper> : IJobParallelForBatched where MovementPlaneWrapper : struct, IMovementPlaneWrapper
	{
		// Token: 0x1700027B RID: 635
		// (get) Token: 0x0600112C RID: 4396 RVA: 0x0001AA07 File Offset: 0x00018C07
		public bool allowBoundsChecks
		{
			get
			{
				return false;
			}
		}

		// Token: 0x0600112D RID: 4397 RVA: 0x00067C90 File Offset: 0x00065E90
		public void Execute(int startIndex, int count)
		{
			if (!this.enabled)
			{
				for (int i = startIndex; i < startIndex + count; i++)
				{
					this.collisionVelocityOffsets[i] = float2.zero;
				}
				return;
			}
			for (int j = startIndex; j < startIndex + count; j++)
			{
				if (!this.agentData.version[j].Valid || this.agentData.locked[j])
				{
					this.collisionVelocityOffsets[j] = float2.zero;
				}
				else
				{
					NativeSlice<int> nativeSlice = this.neighbours.Slice<int>(j * 50, 50);
					float num = this.agentData.radius[j];
					float2 @float = float2.zero;
					float num2 = 0f;
					float3 float2 = this.agentData.position[j];
					MovementPlaneWrapper movementPlaneWrapper = new MovementPlaneWrapper();
					movementPlaneWrapper.Set(this.agentData.movementPlane[j]);
					int num3 = 0;
					while (num3 < nativeSlice.Length && nativeSlice[num3] != -1)
					{
						int num4 = nativeSlice[num3];
						float2 float3 = movementPlaneWrapper.ToPlane(float2 - this.agentData.position[num4]);
						float num5 = math.lengthsq(float3);
						float num6 = this.agentData.radius[num4] + num;
						if (num5 < num6 * num6)
						{
							if (num5 > 1E-08f)
							{
								float num7 = math.sqrt(num5);
								float2 float4 = float3 * (1f / num7);
								float num8 = num6 - num7;
								float2 float5 = float4 * num8 * num8;
								@float += float5;
								num2 += num8;
							}
							else
							{
								float num9;
								float num10;
								math.sincos(math.frac((float)j * 0.521353f + (float)num3 * 0.753177f) * 3.1415927f * 2f, out num9, out num10);
								@float += 100f * new float2(num10, num9) * num * 0.01f;
								num2 += 100f;
							}
						}
						num3++;
					}
					float2 float6 = @float * (1f / (0.0001f + num2));
					float6 *= 0.4f / this.deltaTime;
					this.collisionVelocityOffsets[j] = float6;
				}
			}
		}

		// Token: 0x04000C73 RID: 3187
		[ReadOnly]
		public SimulatorBurst.AgentData agentData;

		// Token: 0x04000C74 RID: 3188
		[ReadOnly]
		public NativeArray<int> neighbours;

		// Token: 0x04000C75 RID: 3189
		[WriteOnly]
		public NativeArray<float2> collisionVelocityOffsets;

		// Token: 0x04000C76 RID: 3190
		public float deltaTime;

		// Token: 0x04000C77 RID: 3191
		public bool enabled;

		// Token: 0x04000C78 RID: 3192
		private const float CollisionStrength = 0.8f;
	}
}
