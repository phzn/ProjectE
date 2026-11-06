using System;
using Pathfinding.Collections;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Profiling;

namespace Pathfinding.RVO
{
	// Token: 0x020002B5 RID: 693
	[BurstCompile(CompileSynchronously = false, FloatMode = FloatMode.Fast)]
	public struct JobDestinationReached<MovementPlaneWrapper> : IJob where MovementPlaneWrapper : struct, IMovementPlaneWrapper
	{
		// Token: 0x06001131 RID: 4401 RVA: 0x00068158 File Offset: 0x00066358
		public void Execute()
		{
			for (int i = 0; i < this.numAgents; i++)
			{
				this.output.effectivelyReachedDestination[i] = ReachedEndOfPath.NotReached;
			}
			NativeArray<int> nativeArray = new NativeArray<int>(this.agentData.position.Length * 7, Allocator.Temp, NativeArrayOptions.UninitializedMemory);
			NativeArray<int> nativeArray2 = new NativeArray<int>(this.agentData.position.Length, Allocator.Temp, NativeArrayOptions.ClearMemory);
			NativeCircularBuffer<int> nativeCircularBuffer = new NativeCircularBuffer<int>(16, Allocator.Temp);
			NativeArray<bool> nativeArray3 = new NativeArray<bool>(this.numAgents, Allocator.Temp, NativeArrayOptions.ClearMemory);
			NativeArray<JobDestinationReached<MovementPlaneWrapper>.TempAgentData> nativeArray4 = new NativeArray<JobDestinationReached<MovementPlaneWrapper>.TempAgentData>(this.numAgents, Allocator.Temp, NativeArrayOptions.UninitializedMemory);
			for (int j = 0; j < this.numAgents; j++)
			{
				if (this.agentData.version[j].Valid)
				{
					for (int k = 0; k < 7; k++)
					{
						int num = this.output.blockedByAgents[j * 7 + k];
						if (num == -1)
						{
							break;
						}
						int num2 = nativeArray2[num];
						if (num2 < 7)
						{
							nativeArray[num * 7 + num2] = j;
							nativeArray2[num] = num2 + 1;
						}
					}
				}
			}
			for (int l = 0; l < this.numAgents; l++)
			{
				if (this.agentData.version[l].Valid)
				{
					float3 @float = this.agentData.position[l];
					MovementPlaneWrapper movementPlaneWrapper = default(MovementPlaneWrapper);
					movementPlaneWrapper.Set(this.agentData.movementPlane[l]);
					float num3 = this.output.speed[l];
					float3 float2 = this.agentData.endOfPath[l];
					if (math.isfinite(float2.x))
					{
						float num5;
						float num4 = math.lengthsq(movementPlaneWrapper.ToPlane(float2 - @float, out num5));
						float num6 = this.agentData.height[l];
						bool flag = false;
						bool flag2 = false;
						float num7 = this.agentData.radius[l];
						float num8 = this.output.forwardClearance[l];
						if (num4 < num7 * num7 * 0.25f && num5 < num6 && num5 > -num6 * 0.5f)
						{
							flag = true;
						}
						bool flag3 = num8 < num7 * 0.5f;
						bool flag4 = num3 * num3 < math.max(0.0001f, math.lengthsq(this.temporaryAgentData.desiredVelocity[l]) * 0.25f);
						bool flag5 = flag3 && flag4;
						nativeArray4[l] = new JobDestinationReached<MovementPlaneWrapper>.TempAgentData
						{
							blockedAndSlow = flag5,
							distToEndSq = num4
						};
						for (int m = 0; m < 7; m++)
						{
							int num9 = this.output.blockedByAgents[l * 7 + m];
							if (num9 == -1)
							{
								break;
							}
							float3 float3 = this.agentData.position[num9];
							float num10 = (math.sqrt(math.lengthsq(movementPlaneWrapper.ToPlane(@float - float3))) + num7 + this.agentData.radius[num9]) * 0.5f;
							if (math.lengthsq(movementPlaneWrapper.ToPlane(float2 - 0.5f * (@float + float3))) < num10 * num10)
							{
								bool flag6 = false;
								for (int n = 0; n < 7; n++)
								{
									int num11 = nativeArray[l * 7 + n];
									if (num11 == -1)
									{
										break;
									}
									if (num11 == num9)
									{
										flag6 = true;
										break;
									}
								}
								if (flag6)
								{
									flag2 = true;
									if (flag5)
									{
										flag = true;
									}
								}
							}
						}
						ReachedEndOfPath reachedEndOfPath = (flag ? ReachedEndOfPath.Reached : (flag2 ? ReachedEndOfPath.ReachedSoon : ReachedEndOfPath.NotReached));
						if (reachedEndOfPath != this.output.effectivelyReachedDestination[l])
						{
							this.output.effectivelyReachedDestination[l] = reachedEndOfPath;
							if (reachedEndOfPath == ReachedEndOfPath.Reached)
							{
								nativeArray3[l] = true;
								int num12 = nativeArray2[l];
								for (int num13 = 0; num13 < num12; num13++)
								{
									int num14 = nativeArray[l * 7 + num13];
									if (!nativeArray3[num14])
									{
										nativeCircularBuffer.PushEnd(num14);
									}
								}
							}
						}
					}
				}
			}
			int num15 = 0;
			while (nativeCircularBuffer.Length > 0)
			{
				int num16 = nativeCircularBuffer.PopStart();
				num15++;
				if (this.output.effectivelyReachedDestination[num16] != ReachedEndOfPath.Reached)
				{
					nativeArray3[num16] = false;
					float num17 = this.output.speed[num16];
					float3 float4 = this.agentData.endOfPath[num16];
					if (math.isfinite(float4.x))
					{
						float3 float5 = this.agentData.position[num16];
						bool blockedAndSlow = nativeArray4[num16].blockedAndSlow;
						float distToEndSq = nativeArray4[num16].distToEndSq;
						float num18 = this.agentData.radius[num16];
						bool flag7 = false;
						bool flag8 = false;
						for (int num19 = 0; num19 < 7; num19++)
						{
							int num20 = this.output.blockedByAgents[num16 * 7 + num19];
							if (num20 == -1)
							{
								break;
							}
							float3 float6 = this.agentData.endOfPath[num20];
							float num21 = this.agentData.radius[num20];
							bool flag9 = math.lengthsq(float6 - float4) <= distToEndSq * 0.25f;
							if (this.output.effectivelyReachedDestination[num20] == ReachedEndOfPath.Reached && (flag9 || math.lengthsq(float4 - this.agentData.position[num20]) < math.lengthsq(num18 + num21)))
							{
								float num22 = this.output.speed[num20];
								flag8 |= math.min(num17, num22) < 0.01f;
								flag7 = flag7 || blockedAndSlow;
							}
						}
						ReachedEndOfPath reachedEndOfPath2 = (flag7 ? ReachedEndOfPath.Reached : (flag8 ? ReachedEndOfPath.ReachedSoon : ReachedEndOfPath.NotReached));
						reachedEndOfPath2 = (ReachedEndOfPath)math.max((int)reachedEndOfPath2, (int)this.output.effectivelyReachedDestination[num16]);
						if (reachedEndOfPath2 != this.output.effectivelyReachedDestination[num16])
						{
							this.output.effectivelyReachedDestination[num16] = reachedEndOfPath2;
							if (reachedEndOfPath2 == ReachedEndOfPath.Reached)
							{
								nativeArray3[num16] = true;
								int num23 = nativeArray2[num16];
								for (int num24 = 0; num24 < num23; num24++)
								{
									int num25 = nativeArray[num16 * 7 + num24];
									if (!nativeArray3[num25])
									{
										nativeCircularBuffer.PushEnd(num25);
									}
								}
							}
						}
					}
				}
			}
		}

		// Token: 0x04000C7D RID: 3197
		[ReadOnly]
		public SimulatorBurst.AgentData agentData;

		// Token: 0x04000C7E RID: 3198
		[ReadOnly]
		public SimulatorBurst.TemporaryAgentData temporaryAgentData;

		// Token: 0x04000C7F RID: 3199
		public SimulatorBurst.AgentOutputData output;

		// Token: 0x04000C80 RID: 3200
		public int numAgents;

		// Token: 0x04000C81 RID: 3201
		private static readonly ProfilerMarker MarkerInvert = new ProfilerMarker("InvertArrows");

		// Token: 0x04000C82 RID: 3202
		private static readonly ProfilerMarker MarkerAlloc = new ProfilerMarker("Alloc");

		// Token: 0x04000C83 RID: 3203
		private static readonly ProfilerMarker MarkerFirstPass = new ProfilerMarker("FirstPass");

		// Token: 0x020002B6 RID: 694
		private struct TempAgentData
		{
			// Token: 0x04000C84 RID: 3204
			public bool blockedAndSlow;

			// Token: 0x04000C85 RID: 3205
			public float distToEndSq;
		}
	}
}
