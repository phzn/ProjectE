using System;
using Pathfinding.Jobs;
using Unity.Burst;
using Unity.Collections;
using Unity.Mathematics;
using UnityEngine;

namespace Pathfinding.RVO
{
	// Token: 0x020002B1 RID: 689
	[BurstCompile(FloatMode = FloatMode.Fast)]
	public struct JobHorizonAvoidancePhase1<MovementPlaneWrapper> : IJobParallelForBatched where MovementPlaneWrapper : struct, IMovementPlaneWrapper
	{
		// Token: 0x17000279 RID: 633
		// (get) Token: 0x06001126 RID: 4390 RVA: 0x00019DC2 File Offset: 0x00017FC2
		public bool allowBoundsChecks
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06001127 RID: 4391 RVA: 0x000675A8 File Offset: 0x000657A8
		private static void Sort<T>(NativeSlice<T> arr, NativeSlice<float> keys) where T : struct
		{
			bool flag = true;
			while (flag)
			{
				flag = false;
				for (int i = 0; i < arr.Length - 1; i++)
				{
					if (keys[i] > keys[i + 1])
					{
						float num = keys[i];
						T t = arr[i];
						keys[i] = keys[i + 1];
						keys[i + 1] = num;
						arr[i] = arr[i + 1];
						arr[i + 1] = t;
						flag = true;
					}
				}
			}
		}

		// Token: 0x06001128 RID: 4392 RVA: 0x00067634 File Offset: 0x00065834
		public static float DeltaAngle(float current, float target)
		{
			float num = Mathf.Repeat(target - current, 6.2831855f);
			if (num > 3.1415927f)
			{
				num -= 6.2831855f;
			}
			return num;
		}

		// Token: 0x06001129 RID: 4393 RVA: 0x00067660 File Offset: 0x00065860
		public void Execute(int startIndex, int count)
		{
			NativeArray<float> nativeArray = new NativeArray<float>(100, Allocator.Temp, NativeArrayOptions.ClearMemory);
			NativeArray<int> nativeArray2 = new NativeArray<int>(100, Allocator.Temp, NativeArrayOptions.ClearMemory);
			for (int i = startIndex; i < startIndex + count; i++)
			{
				if (this.agentData.version[i].Valid)
				{
					if (this.agentData.locked[i] || this.agentData.manuallyControlled[i])
					{
						this.horizonAgentData.horizonSide[i] = 0;
						this.horizonAgentData.horizonMinAngle[i] = 0f;
						this.horizonAgentData.horizonMaxAngle[i] = 0f;
					}
					else
					{
						float num = math.atan2(this.desiredTargetPointInVelocitySpace[i].y, this.desiredTargetPointInVelocitySpace[i].x);
						int num2 = 0;
						int num3 = 0;
						float num4 = this.agentData.radius[i];
						float3 @float = this.agentData.position[i];
						MovementPlaneWrapper movementPlaneWrapper = default(MovementPlaneWrapper);
						movementPlaneWrapper.Set(this.agentData.movementPlane[i]);
						float num5 = (math.all(math.isfinite(this.agentData.endOfPath[i])) ? math.lengthsq(this.agentData.endOfPath[i] - @float) : float.PositiveInfinity);
						NativeSlice<int> nativeSlice = this.neighbours.Slice<int>(i * 50, 50);
						int num6 = 0;
						while (num6 < nativeSlice.Length && nativeSlice[num6] != -1)
						{
							int num7 = nativeSlice[num6];
							if (this.agentData.locked[num7] || this.agentData.manuallyControlled[num7])
							{
								float2 float2 = movementPlaneWrapper.ToPlane(this.agentData.position[num7] - @float);
								float num8 = math.length(float2);
								float num9 = this.agentData.radius[num7];
								float num10 = num8 - (num4 + num9);
								if (num10 * num10 <= num5)
								{
									float num11 = math.atan2(float2.y, float2.x) - num;
									float num12;
									if (num10 <= 0f)
									{
										num12 = 1.5393804f;
									}
									else
									{
										num12 = math.asin((num4 + num9) / num8) + 0.017453292f;
									}
									float num13 = JobHorizonAvoidancePhase1<MovementPlaneWrapper>.DeltaAngle(0f, num11 - num12);
									float num14 = num13 + JobHorizonAvoidancePhase1<MovementPlaneWrapper>.DeltaAngle(num13, num11 + num12);
									if (num13 < 0f && num14 > 0f)
									{
										num3++;
									}
									nativeArray[num2] = num13;
									nativeArray2[num2] = 1;
									num2++;
									nativeArray[num2] = num14;
									nativeArray2[num2] = -1;
									num2++;
								}
							}
							num6++;
						}
						if (num3 == 0)
						{
							this.horizonAgentData.horizonSide[i] = 0;
							this.horizonAgentData.horizonMinAngle[i] = 0f;
							this.horizonAgentData.horizonMaxAngle[i] = 0f;
						}
						else
						{
							JobHorizonAvoidancePhase1<MovementPlaneWrapper>.Sort<int>(nativeArray2.Slice<int>(0, num2), nativeArray.Slice<float>(0, num2));
							int num15 = 0;
							while (num15 < num2 && nativeArray[num15] <= 0f)
							{
								num15++;
							}
							int num16 = num3;
							int j;
							for (j = num15; j < num2; j++)
							{
								num16 += nativeArray2[j];
								if (num16 == 0)
								{
									break;
								}
							}
							float num17 = ((j == num2) ? 3.1415927f : nativeArray[j]);
							num16 = num3;
							for (j = num15 - 1; j >= 0; j--)
							{
								num16 -= nativeArray2[j];
								if (num16 == 0)
								{
									break;
								}
							}
							float num18 = ((j == -1) ? (-3.1415927f) : nativeArray[j]);
							if (this.horizonAgentData.horizonSide[i] == 0)
							{
								this.horizonAgentData.horizonSide[i] = 2;
							}
							this.horizonAgentData.horizonMinAngle[i] = num18 + num;
							this.horizonAgentData.horizonMaxAngle[i] = num17 + num;
						}
					}
				}
			}
		}

		// Token: 0x04000C69 RID: 3177
		[ReadOnly]
		public SimulatorBurst.AgentData agentData;

		// Token: 0x04000C6A RID: 3178
		[ReadOnly]
		public NativeArray<float2> desiredTargetPointInVelocitySpace;

		// Token: 0x04000C6B RID: 3179
		[ReadOnly]
		public NativeArray<int> neighbours;

		// Token: 0x04000C6C RID: 3180
		public SimulatorBurst.HorizonAgentData horizonAgentData;
	}
}
