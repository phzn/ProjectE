using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Pathfinding.Collections;
using Pathfinding.Drawing;
using Pathfinding.Jobs;
using Unity.Burst;
using Unity.Burst.CompilerServices;
using Unity.Collections;
using Unity.Mathematics;
using Unity.Profiling;
using UnityEngine;

namespace Pathfinding.RVO
{
	// Token: 0x020002B7 RID: 695
	[BurstCompile(CompileSynchronously = true, FloatMode = FloatMode.Default)]
	public struct JobRVO<MovementPlaneWrapper> : IJobParallelForBatched where MovementPlaneWrapper : struct, IMovementPlaneWrapper
	{
		// Token: 0x1700027D RID: 637
		// (get) Token: 0x06001133 RID: 4403 RVA: 0x00019DC2 File Offset: 0x00017FC2
		public bool allowBoundsChecks
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06001134 RID: 4404 RVA: 0x0006882B File Offset: 0x00066A2B
		public void Execute(int startIndex, int batchSize)
		{
			this.ExecuteORCA(startIndex, batchSize);
		}

		// Token: 0x06001135 RID: 4405 RVA: 0x00068838 File Offset: 0x00066A38
		private unsafe static void InsertionSort<[IsUnmanaged] T, U>(UnsafeSpan<T> data, U comparer) where T : struct, ValueType where U : IComparer<T>
		{
			for (int i = 1; i < data.Length; i++)
			{
				T t = *data[i];
				int num = i - 1;
				while (num >= 0 && comparer.Compare(*data[num], t) > 0)
				{
					*data[num + 1] = *data[num];
					num--;
				}
				*data[num + 1] = t;
			}
		}

		// Token: 0x06001136 RID: 4406 RVA: 0x000688C0 File Offset: 0x00066AC0
		private unsafe void GenerateObstacleVOs(int agentIndex, NativeList<int> adjacentObstacleIdsScratch, NativeArray<int2> adjacentObstacleVerticesScratch, NativeArray<float> segmentDistancesScratch, NativeArray<int> sortedVerticesScratch, NativeArray<JobRVO<MovementPlaneWrapper>.ORCALine> orcaLines, NativeArray<int> orcaLineToAgent, [NoAlias] ref int numLines, [NoAlias] in MovementPlaneWrapper movementPlane, float2 optimalVelocity)
		{
			if (!this.useNavmeshAsObstacle)
			{
				return;
			}
			MovementPlaneWrapper movementPlaneWrapper = movementPlane;
			float num;
			float2 @float = movementPlaneWrapper.ToPlane(this.agentData.position[agentIndex], out num);
			float num2 = this.agentData.height[agentIndex];
			float num3 = this.agentData.radius[agentIndex];
			float num4 = num3 * 0.01f;
			float num5 = math.rcp(this.agentData.obstacleTimeHorizon[agentIndex]);
			Aliasing.ExpectNotAliased<NativeArray<float3>, NativeArray<float3>>(in this.agentData.collisionNormal, in this.agentData.position);
			int num6 = this.agentData.hierarchicalNodeIndex[agentIndex];
			if (num6 == -1)
			{
				return;
			}
			float3 float2 = (num4 + num3 + this.agentData.obstacleTimeHorizon[agentIndex] * this.agentData.maxSpeed[agentIndex]) * new float3(2f, 0f, 2f);
			float2.y = this.agentData.height[agentIndex] * 2f;
			Bounds bounds = new Bounds(new Vector3(@float.x, num, @float.y), float2);
			float num7 = math.lengthsq(bounds.extents);
			adjacentObstacleIdsScratch.Clear();
			movementPlaneWrapper = movementPlane;
			Bounds bounds2 = movementPlaneWrapper.ToWorld(bounds);
			this.navmeshEdgeData.GetObstaclesInRange(num6, bounds2, adjacentObstacleIdsScratch);
			for (int i = 0; i < adjacentObstacleIdsScratch.Length; i++)
			{
				int num8 = adjacentObstacleIdsScratch[i];
				UnmanagedObstacle unmanagedObstacle = this.navmeshEdgeData.obstacleData.obstacles[num8];
				UnsafeSpan<float3> span = this.navmeshEdgeData.obstacleData.obstacleVertices.GetSpan(unmanagedObstacle.verticesAllocation);
				UnsafeSpan<ObstacleVertexGroup> span2 = this.navmeshEdgeData.obstacleData.obstacleVertexGroups.GetSpan(unmanagedObstacle.groupsAllocation);
				int num9 = 0;
				int num10 = 0;
				for (int j = 0; j < span2.Length; j++)
				{
					ObstacleVertexGroup obstacleVertexGroup = *span2[j];
					if (!math.all((obstacleVertexGroup.boundsMx >= bounds2.min) & (obstacleVertexGroup.boundsMn <= bounds2.max)))
					{
						num9 += obstacleVertexGroup.vertexCount;
					}
					else
					{
						int num11 = num9;
						int num12 = num9 + obstacleVertexGroup.vertexCount - 1;
						if (num12 >= adjacentObstacleVerticesScratch.Length)
						{
							break;
						}
						for (int k = num11; k < num11 + obstacleVertexGroup.vertexCount; k++)
						{
							adjacentObstacleVerticesScratch[k] = new int2(k - 1, k + 1);
						}
						adjacentObstacleVerticesScratch[num11] = new int2((obstacleVertexGroup.type == ObstacleType.Loop) ? num12 : num11, adjacentObstacleVerticesScratch[num11].y);
						adjacentObstacleVerticesScratch[num12] = new int2(adjacentObstacleVerticesScratch[num12].x, (obstacleVertexGroup.type == ObstacleType.Loop) ? num11 : num12);
						for (int l = 0; l < obstacleVertexGroup.vertexCount; l++)
						{
							float3 float3 = *span[l + num9];
							int y = adjacentObstacleVerticesScratch[l + num11].y;
							movementPlaneWrapper = movementPlane;
							float2 float4 = movementPlaneWrapper.ToPlane(float3) - @float;
							movementPlaneWrapper = movementPlane;
							float2 float5 = movementPlaneWrapper.ToPlane(*span[y]) - @float - float4;
							float num13 = JobRVO<MovementPlaneWrapper>.ClosestPointOnSegment(float4, float5 / math.lengthsq(float5), float2.zero, 0f, 1f);
							float num14 = math.lengthsq(float4 + float5 * num13);
							segmentDistancesScratch[l + num11] = num14;
							if (num14 <= num7 && num10 < sortedVerticesScratch.Length)
							{
								sortedVerticesScratch[num10] = l + num11;
								num10++;
							}
						}
						num9 += obstacleVertexGroup.vertexCount;
					}
				}
				JobRVO<MovementPlaneWrapper>.InsertionSort<int, JobRVO<MovementPlaneWrapper>.SortByKey>(sortedVerticesScratch.AsUnsafeSpan<int>().Slice(0, num10), new JobRVO<MovementPlaneWrapper>.SortByKey
				{
					keys = segmentDistancesScratch.AsUnsafeSpan<float>().Slice(0, num9)
				});
				int num15 = 0;
				while (num15 < num10 && numLines < 50)
				{
					int num16 = sortedVerticesScratch[num15];
					if (segmentDistancesScratch[num16] > 0.25f * float2.x * float2.x)
					{
						break;
					}
					int x = adjacentObstacleVerticesScratch[num16].x;
					int y2 = adjacentObstacleVerticesScratch[num16].y;
					if (y2 != num16)
					{
						int y3 = adjacentObstacleVerticesScratch[y2].y;
						float3 float6 = *span[x];
						float3 float7 = *span[num16];
						float3 float8 = *span[y2];
						float3 float9 = *span[y3];
						movementPlaneWrapper = movementPlane;
						float2 float10 = movementPlaneWrapper.ToPlane(float6) - @float;
						movementPlaneWrapper = movementPlane;
						float num17;
						float2 float11 = movementPlaneWrapper.ToPlane(float7, out num17) - @float;
						movementPlaneWrapper = movementPlane;
						float num18;
						float2 float12 = movementPlaneWrapper.ToPlane(float8, out num18) - @float;
						movementPlaneWrapper = movementPlane;
						float2 float13 = movementPlaneWrapper.ToPlane(float9) - @float;
						if (math.max(num17, num18) + num2 >= num && math.min(num17, num18) <= num + num2)
						{
							float num19 = math.length(float12 - float11);
							if (num19 >= 0.0001f)
							{
								float2 float14 = (float12 - float11) * math.rcp(num19);
								if (JobRVO<MovementPlaneWrapper>.det(float14, -float11) <= num4)
								{
									bool flag = false;
									for (int m = 0; m < numLines; m++)
									{
										JobRVO<MovementPlaneWrapper>.ORCALine orcaline = orcaLines[m];
										if (JobRVO<MovementPlaneWrapper>.det(num5 * float11 - orcaline.point, orcaline.direction) - num5 * num4 >= -0.0001f && JobRVO<MovementPlaneWrapper>.det(num5 * float12 - orcaline.point, orcaline.direction) - num5 * num4 >= -0.0001f)
										{
											flag = true;
											break;
										}
									}
									if (!flag)
									{
										float2 zero = float2.zero;
										float num20 = math.dot(zero - float11, float14);
										float2 float15 = float11 + num20 * float14;
										float num21 = math.lengthsq(float15 - zero);
										float num22 = math.lengthsq(float11 + math.clamp(num20, 0f, num19) * float14);
										bool flag2 = JobRVO<MovementPlaneWrapper>.leftOrColinear(float11 - float10, float14);
										bool flag3 = JobRVO<MovementPlaneWrapper>.leftOrColinear(float14, float13 - float12);
										if (num22 < num4 * num4)
										{
											if (num20 < 0f)
											{
												if (flag2)
												{
													orcaLineToAgent[numLines] = -1;
													int num23 = numLines;
													numLines = num23 + 1;
													orcaLines[num23] = new JobRVO<MovementPlaneWrapper>.ORCALine
													{
														point = -float11 * 0.1f,
														direction = math.normalizesafe(JobRVO<MovementPlaneWrapper>.rot90(float11), default(float2))
													};
												}
											}
											else if (num20 > num19)
											{
												if (flag3 && JobRVO<MovementPlaneWrapper>.leftOrColinear(float12, float13 - float12))
												{
													orcaLineToAgent[numLines] = -1;
													int num23 = numLines;
													numLines = num23 + 1;
													orcaLines[num23] = new JobRVO<MovementPlaneWrapper>.ORCALine
													{
														point = -float12 * 0.1f,
														direction = math.normalizesafe(JobRVO<MovementPlaneWrapper>.rot90(float12), default(float2))
													};
												}
											}
											else
											{
												orcaLineToAgent[numLines] = -1;
												int num23 = numLines;
												numLines = num23 + 1;
												orcaLines[num23] = new JobRVO<MovementPlaneWrapper>.ORCALine
												{
													point = -float15 * 0.1f,
													direction = -float14
												};
											}
										}
										else
										{
											float2 float17;
											float2 float18;
											if ((num20 < 0f || num20 > 1f) && num21 <= num4 * num4)
											{
												if (num20 < 0f)
												{
													if (!flag2)
													{
														goto IL_0D6A;
													}
													float13 = float12;
													float12 = float11;
													flag3 = flag2;
												}
												else
												{
													if (!flag3)
													{
														goto IL_0D6A;
													}
													float10 = float11;
													float11 = float12;
													flag2 = flag3;
												}
												float num24 = math.lengthsq(float11);
												float num25 = math.sqrt(num24 - num4 * num4);
												float2 float16 = new float2(-float11.y, float11.x);
												float17 = (float11 * num25 + float16 * num4) / num24;
												float18 = (float11 * num25 - float16 * num4) / num24;
											}
											else
											{
												if (flag2)
												{
													float num26 = math.lengthsq(float11);
													float num27 = math.sqrt(num26 - num4 * num4);
													float2 float19 = new float2(-float11.y, float11.x);
													float17 = (float11 * num27 + float19 * num4) / num26;
												}
												else
												{
													float17 = -float14;
												}
												if (flag3)
												{
													float num28 = math.lengthsq(float12);
													float num29 = math.sqrt(num28 - num4 * num4);
													float2 float20 = new float2(-float12.y, float12.x);
													float18 = (float12 * num29 - float20 * num4) / num28;
												}
												else
												{
													float18 = float14;
												}
											}
											bool flag4 = false;
											bool flag5 = false;
											if (flag2 && JobRVO<MovementPlaneWrapper>.left(float17, float10 - float11))
											{
												float17 = float10 - float11;
												flag4 = true;
											}
											if (flag3 && JobRVO<MovementPlaneWrapper>.right(float18, float13 - float12))
											{
												float18 = float13 - float12;
												flag5 = true;
											}
											float2 float21 = num5 * float11;
											float2 float22 = num5 * float12;
											float2 float23 = float22 - float21;
											float num30 = math.lengthsq(float23);
											float num31 = ((num30 <= 1E-05f) ? 0.5f : (math.dot(optimalVelocity - float21, float23) / num30));
											float num32 = math.dot(optimalVelocity - float21, float17);
											float num33 = math.dot(optimalVelocity - float22, float18);
											if ((num31 < 0f && num32 < 0f) || (num31 > 1f && num33 < 0f) || (num30 <= 1E-05f && num32 < 0f && num33 < 0f))
											{
												float2 float24 = ((num31 <= 0.5f) ? float21 : float22);
												float2 float25 = math.normalizesafe(optimalVelocity - float24, default(float2));
												orcaLineToAgent[numLines] = -1;
												int num23 = numLines;
												numLines = num23 + 1;
												orcaLines[num23] = new JobRVO<MovementPlaneWrapper>.ORCALine
												{
													point = float24 + num4 * num5 * float25,
													direction = new float2(float25.y, -float25.x)
												};
											}
											else
											{
												float num34 = ((num31 > 1f || num31 < 0f || num30 < 0.0001f) ? float.PositiveInfinity : math.lengthsq(optimalVelocity - (float21 + num31 * float23)));
												float num35 = ((num32 < 0f) ? float.PositiveInfinity : math.lengthsq(optimalVelocity - (float21 + num32 * float17)));
												float num36 = ((num33 < 0f) ? float.PositiveInfinity : math.lengthsq(optimalVelocity - (float22 + num33 * float18)));
												int num37 = 0;
												float num38 = num34;
												if (num35 < num38)
												{
													num38 = num35;
													num37 = 1;
												}
												if (num36 < num38)
												{
													num37 = 2;
												}
												if (num37 == 0)
												{
													orcaLineToAgent[numLines] = -1;
													int num23 = numLines;
													numLines = num23 + 1;
													orcaLines[num23] = new JobRVO<MovementPlaneWrapper>.ORCALine
													{
														point = float21 + num4 * num5 * new float2(float14.y, -float14.x),
														direction = -float14
													};
												}
												else if (num37 == 1)
												{
													if (!flag4)
													{
														orcaLineToAgent[numLines] = -1;
														int num23 = numLines;
														numLines = num23 + 1;
														orcaLines[num23] = new JobRVO<MovementPlaneWrapper>.ORCALine
														{
															point = float21 + num4 * num5 * new float2(-float17.y, float17.x),
															direction = float17
														};
													}
												}
												else if (num37 == 2 && !flag5)
												{
													orcaLineToAgent[numLines] = -1;
													int num23 = numLines;
													numLines = num23 + 1;
													orcaLines[num23] = new JobRVO<MovementPlaneWrapper>.ORCALine
													{
														point = float22 + num4 * num5 * new float2(float18.y, -float18.x),
														direction = -float18
													};
												}
											}
										}
									}
								}
							}
						}
					}
					IL_0D6A:
					num15++;
				}
			}
		}

		// Token: 0x06001137 RID: 4407 RVA: 0x0006965C File Offset: 0x0006785C
		public void ExecuteORCA(int startIndex, int batchSize)
		{
			int num = startIndex + batchSize;
			NativeArray<JobRVO<MovementPlaneWrapper>.ORCALine> nativeArray = new NativeArray<JobRVO<MovementPlaneWrapper>.ORCALine>(100, Allocator.Temp, NativeArrayOptions.UninitializedMemory);
			NativeArray<JobRVO<MovementPlaneWrapper>.ORCALine> nativeArray2 = new NativeArray<JobRVO<MovementPlaneWrapper>.ORCALine>(100, Allocator.Temp, NativeArrayOptions.UninitializedMemory);
			NativeArray<float> nativeArray3 = new NativeArray<float>(256, Allocator.Temp, NativeArrayOptions.UninitializedMemory);
			NativeArray<int> nativeArray4 = new NativeArray<int>(256, Allocator.Temp, NativeArrayOptions.UninitializedMemory);
			NativeArray<int2> nativeArray5 = new NativeArray<int2>(1024, Allocator.Temp, NativeArrayOptions.UninitializedMemory);
			NativeArray<int> nativeArray6 = new NativeArray<int>(100, Allocator.Temp, NativeArrayOptions.UninitializedMemory);
			NativeList<int> nativeList = new NativeList<int>(16, Allocator.Temp);
			for (int i = startIndex; i < num; i++)
			{
				if (this.agentData.version[i].Valid)
				{
					if (this.agentData.manuallyControlled[i])
					{
						this.output.speed[i] = this.agentData.desiredSpeed[i];
						this.output.targetPoint[i] = this.agentData.targetPoint[i];
						this.output.blockedByAgents[i * 7] = -1;
					}
					else
					{
						float3 @float = this.agentData.position[i];
						if (this.agentData.locked[i])
						{
							this.output.speed[i] = 0f;
							this.output.targetPoint[i] = @float;
							this.output.blockedByAgents[i * 7] = -1;
						}
						else
						{
							MovementPlaneWrapper movementPlaneWrapper = default(MovementPlaneWrapper);
							movementPlaneWrapper.Set(this.agentData.movementPlane[i]);
							float2 float2 = movementPlaneWrapper.ToPlane(this.temporaryAgentData.currentVelocity[i]);
							int num2 = 0;
							this.GenerateObstacleVOs(i, nativeList, nativeArray5, nativeArray3, nativeArray4, nativeArray, nativeArray6, ref num2, in movementPlaneWrapper, float2);
							int num3 = num2;
							NativeSlice<int> nativeSlice = this.temporaryAgentData.neighbours.Slice<int>(i * 50, 50);
							float num4 = this.agentData.agentTimeHorizon[i];
							float num5 = math.rcp(num4);
							float num6 = this.agentData.priority[i];
							float2 float3 = movementPlaneWrapper.ToPlane(@float);
							float num7 = this.agentData.radius[i];
							float num8 = (math.all(math.isfinite(this.agentData.endOfPath[i])) ? math.lengthsq(this.agentData.endOfPath[i] - @float) : float.PositiveInfinity);
							for (int j = 0; j < nativeSlice.Length; j++)
							{
								int num9 = nativeSlice[j];
								if (num9 == -1)
								{
									break;
								}
								float3 float4 = this.agentData.position[num9];
								float2 float5 = movementPlaneWrapper.ToPlane(float4 - @float);
								float num10 = num7 + this.agentData.radius[num9];
								float num11 = this.agentData.priority[num9] * this.priorityMultiplier;
								float num12;
								if (this.agentData.locked[num9] || this.agentData.manuallyControlled[num9])
								{
									num12 = 1f;
								}
								else if (num11 > 1E-05f || num6 > 1E-05f)
								{
									num12 = num11 / (num6 + num11);
								}
								else
								{
									num12 = 0.5f;
								}
								float2 float6 = movementPlaneWrapper.ToPlane(math.lerp(this.temporaryAgentData.currentVelocity[num9], this.temporaryAgentData.desiredVelocity[num9], math.clamp(2f * num12 - 1f, 0f, 1f)));
								if (this.agentData.flowFollowingStrength[num9] > 0f)
								{
									float num13 = this.agentData.flowFollowingStrength[num9] * this.agentData.flowFollowingStrength[i];
									float2 float7 = math.normalizesafe(float5, default(float2));
									float6 -= float7 * (num13 * math.min(0f, math.dot(float6, float7)));
								}
								float num14 = math.length(float5);
								float num15 = math.max(0f, num14 - num10);
								if (!this.agentData.locked[num9] || num15 * num15 <= num8)
								{
									float num16 = num15 / math.max(num10, this.agentData.desiredSpeed[i] + this.agentData.desiredSpeed[num9]);
									float num17 = math.clamp((num16 * num5 - 0.5f) * 2f, 0f, 1f);
									num10 *= 1f - num17;
									float num18 = 1f / math.max(0.1f * num4, num4 * math.clamp(math.sqrt(2f * num16), 0f, 1f));
									nativeArray[num2] = new JobRVO<MovementPlaneWrapper>.ORCALine(float3, float5, float2, float6, num10, 0.1f, num18);
									nativeArray6[num2] = num9;
									num2++;
								}
							}
							float2 float8 = math.normalizesafe(movementPlaneWrapper.ToPlane(this.agentData.collisionNormal[i]), default(float2));
							if (math.any(float8 != 0f))
							{
								nativeArray[num2] = new JobRVO<MovementPlaneWrapper>.ORCALine
								{
									point = float2.zero,
									direction = new float2(float8.y, -float8.x)
								};
								nativeArray6[num2] = -1;
								num2++;
							}
							float2 float9 = movementPlaneWrapper.ToPlane(this.temporaryAgentData.desiredVelocity[i]);
							float2 float10 = this.temporaryAgentData.desiredTargetPointInVelocitySpace[i];
							float num19 = this.symmetryBreakingBias * (1f - this.agentData.flowFollowingStrength[i]);
							if (!JobRVO<MovementPlaneWrapper>.BiasDesiredVelocity(nativeArray.AsUnsafeSpan<JobRVO<MovementPlaneWrapper>.ORCALine>().Slice(num3, num2 - num3), ref float9, ref float10, num19) && JobRVO<MovementPlaneWrapper>.DistanceInsideVOs(nativeArray.AsUnsafeSpan<JobRVO<MovementPlaneWrapper>.ORCALine>().Slice(0, num2), float9) <= 0f && math.all(math.abs(this.temporaryAgentData.collisionVelocityOffsets[i]) < 0.001f))
							{
								this.output.targetPoint[i] = @float + movementPlaneWrapper.ToWorld(float10, 0f);
								this.output.speed[i] = this.agentData.desiredSpeed[i];
								this.output.blockedByAgents[i * 7] = -1;
								this.output.forwardClearance[i] = float.PositiveInfinity;
							}
							else
							{
								float num20 = this.agentData.maxSpeed[i];
								float2 float11 = this.agentData.allowedVelocityDeviationAngles[i];
								JobRVO<MovementPlaneWrapper>.LinearProgram2Output linearProgram2Output;
								if (math.all(float11 == 0f))
								{
									linearProgram2Output = JobRVO<MovementPlaneWrapper>.LinearProgram2D(nativeArray, num2, num20, float9, false);
								}
								else
								{
									float2 float12;
									float2 float13;
									math.sincos(float11, out float12, out float13);
									float2 float14 = float9.x * float13 - float9.y * float12;
									float2 float15 = float9.x * float12 + float9.y * float13;
									float2 float16 = new float2(float14.x, float15.x);
									float2 float17 = new float2(float14.y, float15.y);
									float2 float18 = float9 - float16;
									float num21 = math.length(float18);
									float18 = math.select(float2.zero, float18 * math.rcp(num21), num21 > 1.1754944E-38f);
									float2 float19 = float9 - float17;
									float num22 = math.length(float19);
									float19 = math.select(float2.zero, float19 * math.rcp(num22), num22 > 1.1754944E-38f);
									JobRVO<MovementPlaneWrapper>.LinearProgram2Output linearProgram2Output2 = JobRVO<MovementPlaneWrapper>.LinearProgram2DSegment(nativeArray, num2, num20, float16, float18, 0f, num21, 1f);
									JobRVO<MovementPlaneWrapper>.LinearProgram2Output linearProgram2Output3 = JobRVO<MovementPlaneWrapper>.LinearProgram2DSegment(nativeArray, num2, num20, float17, float19, 0f, num22, 1f);
									if (linearProgram2Output2.firstFailedLineIndex < linearProgram2Output3.firstFailedLineIndex)
									{
										linearProgram2Output = linearProgram2Output2;
									}
									else if (linearProgram2Output3.firstFailedLineIndex < linearProgram2Output2.firstFailedLineIndex)
									{
										linearProgram2Output = linearProgram2Output3;
									}
									else
									{
										linearProgram2Output = ((math.lengthsq(linearProgram2Output2.velocity - float9) < math.lengthsq(linearProgram2Output3.velocity - float9)) ? linearProgram2Output2 : linearProgram2Output3);
									}
								}
								float2 float20;
								if (linearProgram2Output.firstFailedLineIndex < num2)
								{
									float20 = linearProgram2Output.velocity;
									JobRVO<MovementPlaneWrapper>.LinearProgram3D(nativeArray, num2, num3, linearProgram2Output.firstFailedLineIndex, num20, ref float20, nativeArray2);
								}
								else
								{
									float20 = linearProgram2Output.velocity;
								}
								int num23 = 0;
								int num24 = 0;
								while (num24 < num2 && num23 < 7)
								{
									if (nativeArray6[num24] != -1 && JobRVO<MovementPlaneWrapper>.det(nativeArray[num24].direction, nativeArray[num24].point - float20) >= -0.001f)
									{
										this.output.blockedByAgents[i * 7 + num23] = nativeArray6[num24];
										num23++;
									}
									num24++;
								}
								if (num23 < 7)
								{
									this.output.blockedByAgents[i * 7 + num23] = -1;
								}
								if (math.any(this.temporaryAgentData.collisionVelocityOffsets[i] != 0f))
								{
									float20 += this.temporaryAgentData.collisionVelocityOffsets[i];
									float20 = JobRVO<MovementPlaneWrapper>.LinearProgram2D(nativeArray, num3, num20, float20, false).velocity;
								}
								this.output.targetPoint[i] = @float + movementPlaneWrapper.ToWorld(float20, 0f);
								this.output.speed[i] = math.min(math.length(float20), num20);
								float2 float21 = math.normalizesafe(movementPlaneWrapper.ToPlane(this.agentData.targetPoint[i] - @float), default(float2));
								float num25 = this.CalculateForwardClearance(nativeSlice, movementPlaneWrapper, @float, num7, float21);
								this.output.forwardClearance[i] = num25;
							}
						}
					}
				}
			}
		}

		// Token: 0x06001138 RID: 4408 RVA: 0x0006A11C File Offset: 0x0006831C
		private float CalculateForwardClearance(NativeSlice<int> neighbours, MovementPlaneWrapper movementPlane, float3 position, float radius, float2 targetDir)
		{
			float num = float.PositiveInfinity;
			for (int i = 0; i < neighbours.Length; i++)
			{
				int num2 = neighbours[i];
				if (num2 == -1)
				{
					break;
				}
				float3 @float = this.agentData.position[num2];
				float num3 = radius + this.agentData.radius[num2];
				float2 float2 = movementPlane.ToPlane(@float - position);
				float num4 = math.dot(math.normalizesafe(float2, default(float2)), targetDir);
				if (num4 >= 0f)
				{
					float num5 = math.lengthsq(float2);
					float num6 = math.sqrt(num5) * num4;
					float num7 = num3 * num3 - (num5 - num6 * num6);
					if (num7 >= 0f)
					{
						float num8 = num6 - math.sqrt(num7);
						num = math.min(num, num8);
					}
				}
			}
			return num;
		}

		// Token: 0x06001139 RID: 4409 RVA: 0x0006A1FC File Offset: 0x000683FC
		private static bool leftOrColinear(float2 vector1, float2 vector2)
		{
			return JobRVO<MovementPlaneWrapper>.det(vector1, vector2) >= 0f;
		}

		// Token: 0x0600113A RID: 4410 RVA: 0x0006A20F File Offset: 0x0006840F
		private static bool left(float2 vector1, float2 vector2)
		{
			return JobRVO<MovementPlaneWrapper>.det(vector1, vector2) > 0f;
		}

		// Token: 0x0600113B RID: 4411 RVA: 0x0006A21F File Offset: 0x0006841F
		private static bool rightOrColinear(float2 vector1, float2 vector2)
		{
			return JobRVO<MovementPlaneWrapper>.det(vector1, vector2) <= 0f;
		}

		// Token: 0x0600113C RID: 4412 RVA: 0x0006A232 File Offset: 0x00068432
		private static bool right(float2 vector1, float2 vector2)
		{
			return JobRVO<MovementPlaneWrapper>.det(vector1, vector2) < 0f;
		}

		// Token: 0x0600113D RID: 4413 RVA: 0x0000DAAF File Offset: 0x0000BCAF
		private static float det(float2 vector1, float2 vector2)
		{
			return vector1.x * vector2.y - vector1.y * vector2.x;
		}

		// Token: 0x0600113E RID: 4414 RVA: 0x0006A242 File Offset: 0x00068442
		private static float2 rot90(float2 v)
		{
			return new float2(-v.y, v.x);
		}

		// Token: 0x0600113F RID: 4415 RVA: 0x0006A258 File Offset: 0x00068458
		private static float DistanceInsideVOs(UnsafeSpan<JobRVO<MovementPlaneWrapper>.ORCALine> lines, float2 velocity)
		{
			float num = 0f;
			for (int i = 0; i < lines.Length; i++)
			{
				float num2 = JobRVO<MovementPlaneWrapper>.det(lines[i].direction, lines[i].point - velocity);
				num = math.max(num, num2);
			}
			return num;
		}

		// Token: 0x06001140 RID: 4416 RVA: 0x0006A2AC File Offset: 0x000684AC
		private static bool BiasDesiredVelocity(UnsafeSpan<JobRVO<MovementPlaneWrapper>.ORCALine> lines, ref float2 desiredVelocity, ref float2 targetPointInVelocitySpace, float maxBiasRadians)
		{
			float num = JobRVO<MovementPlaneWrapper>.DistanceInsideVOs(lines, desiredVelocity);
			if (num == 0f)
			{
				return false;
			}
			float num2 = math.length(desiredVelocity);
			if (num2 >= 0.001f)
			{
				float num3 = math.min(maxBiasRadians, num / num2);
				desiredVelocity += new float2(desiredVelocity.y, -desiredVelocity.x) * num3;
				targetPointInVelocitySpace += new float2(targetPointInVelocitySpace.y, -targetPointInVelocitySpace.x) * num3;
			}
			return true;
		}

		// Token: 0x06001141 RID: 4417 RVA: 0x0006A344 File Offset: 0x00068544
		private static bool ClipLine(JobRVO<MovementPlaneWrapper>.ORCALine line, JobRVO<MovementPlaneWrapper>.ORCALine clipper, ref float tLeft, ref float tRight)
		{
			float num = JobRVO<MovementPlaneWrapper>.det(line.direction, clipper.direction);
			float num2 = JobRVO<MovementPlaneWrapper>.det(clipper.direction, line.point - clipper.point);
			if (math.abs(num) < 0.0001f)
			{
				return false;
			}
			float num3 = num2 / num;
			if (num >= 0f)
			{
				tRight = math.min(tRight, num3);
			}
			else
			{
				tLeft = math.max(tLeft, num3);
			}
			return true;
		}

		// Token: 0x06001142 RID: 4418 RVA: 0x0006A3B4 File Offset: 0x000685B4
		private static bool ClipBoundary(NativeArray<JobRVO<MovementPlaneWrapper>.ORCALine> lines, int lineIndex, float radius, out float tLeft, out float tRight)
		{
			JobRVO<MovementPlaneWrapper>.ORCALine orcaline = lines[lineIndex];
			if (!VectorMath.LineCircleIntersectionFactors(orcaline.point, orcaline.direction, radius, out tLeft, out tRight))
			{
				return false;
			}
			for (int i = 0; i < lineIndex; i++)
			{
				float num = JobRVO<MovementPlaneWrapper>.det(orcaline.direction, lines[i].direction);
				float num2 = JobRVO<MovementPlaneWrapper>.det(lines[i].direction, orcaline.point - lines[i].point);
				if (math.abs(num) < 0.0001f)
				{
					if (num2 < 0f)
					{
						return false;
					}
				}
				else
				{
					float num3 = num2 / num;
					if (num >= 0f)
					{
						tRight = math.min(tRight, num3);
					}
					else
					{
						tLeft = math.max(tLeft, num3);
					}
					if (tLeft > tRight)
					{
						return false;
					}
				}
			}
			return true;
		}

		// Token: 0x06001143 RID: 4419 RVA: 0x0006A484 File Offset: 0x00068684
		private static bool LinearProgram1D(NativeArray<JobRVO<MovementPlaneWrapper>.ORCALine> lines, int lineIndex, float radius, float2 optimalVelocity, bool directionOpt, ref float2 result)
		{
			float num;
			float num2;
			if (!JobRVO<MovementPlaneWrapper>.ClipBoundary(lines, lineIndex, radius, out num, out num2))
			{
				return false;
			}
			JobRVO<MovementPlaneWrapper>.ORCALine orcaline = lines[lineIndex];
			if (directionOpt)
			{
				if (math.dot(optimalVelocity, orcaline.direction) > 0f)
				{
					result = orcaline.point + num2 * orcaline.direction;
				}
				else
				{
					result = orcaline.point + num * orcaline.direction;
				}
			}
			else
			{
				float num3 = math.dot(orcaline.direction, optimalVelocity - orcaline.point);
				result = orcaline.point + math.clamp(num3, num, num2) * orcaline.direction;
			}
			return true;
		}

		// Token: 0x06001144 RID: 4420 RVA: 0x0006A540 File Offset: 0x00068740
		private static JobRVO<MovementPlaneWrapper>.LinearProgram2Output LinearProgram2D(NativeArray<JobRVO<MovementPlaneWrapper>.ORCALine> lines, int numLines, float radius, float2 optimalVelocity, bool directionOpt)
		{
			float2 @float;
			if (directionOpt)
			{
				@float = optimalVelocity * radius;
			}
			else if (math.lengthsq(optimalVelocity) > radius * radius)
			{
				@float = math.normalize(optimalVelocity) * radius;
			}
			else
			{
				@float = optimalVelocity;
			}
			for (int i = 0; i < numLines; i++)
			{
				if (JobRVO<MovementPlaneWrapper>.det(lines[i].direction, lines[i].point - @float) > 0f)
				{
					float2 float2 = @float;
					if (!JobRVO<MovementPlaneWrapper>.LinearProgram1D(lines, i, radius, optimalVelocity, directionOpt, ref @float))
					{
						return new JobRVO<MovementPlaneWrapper>.LinearProgram2Output
						{
							velocity = float2,
							firstFailedLineIndex = i
						};
					}
				}
			}
			return new JobRVO<MovementPlaneWrapper>.LinearProgram2Output
			{
				velocity = @float,
				firstFailedLineIndex = numLines
			};
		}

		// Token: 0x06001145 RID: 4421 RVA: 0x0006A5F3 File Offset: 0x000687F3
		private static float ClosestPointOnSegment(float2 a, float2 dir, float2 p, float t0, float t1)
		{
			return math.clamp(math.dot(p - a, dir), t0, t1);
		}

		// Token: 0x06001146 RID: 4422 RVA: 0x0006A60C File Offset: 0x0006880C
		private static float2 ClosestSegmentSegmentPointNonIntersecting(JobRVO<MovementPlaneWrapper>.ORCALine a, JobRVO<MovementPlaneWrapper>.ORCALine b, float ta1, float ta2, float tb1, float tb2)
		{
			float2 @float = a.point + a.direction * ta1;
			float2 float2 = a.point + a.direction * ta2;
			float2 float3 = b.point + b.direction * tb1;
			float2 float4 = b.point + b.direction * tb2;
			float num = JobRVO<MovementPlaneWrapper>.ClosestPointOnSegment(a.point, a.direction, float3, ta1, ta2);
			float num2 = JobRVO<MovementPlaneWrapper>.ClosestPointOnSegment(a.point, a.direction, float4, ta1, ta2);
			float num3 = JobRVO<MovementPlaneWrapper>.ClosestPointOnSegment(b.point, b.direction, @float, tb1, tb2);
			float num4 = JobRVO<MovementPlaneWrapper>.ClosestPointOnSegment(b.point, b.direction, float2, tb1, tb2);
			float2 float5 = a.point + a.direction * num;
			float2 float6 = a.point + a.direction * num2;
			float2 float7 = b.point + b.direction * num3;
			float2 float8 = b.point + b.direction * num4;
			float num5 = math.lengthsq(float5 - float3);
			float num6 = math.lengthsq(float6 - float4);
			float num7 = math.lengthsq(float7 - @float);
			float num8 = math.lengthsq(float8 - float2);
			float2 float9 = float5;
			float num9 = num5;
			if (num6 < num9)
			{
				float9 = float6;
				num9 = num6;
			}
			if (num7 < num9)
			{
				float9 = @float;
				num9 = num7;
			}
			if (num8 < num9)
			{
				float9 = float2;
			}
			return float9;
		}

		// Token: 0x06001147 RID: 4423 RVA: 0x0006A7AC File Offset: 0x000689AC
		private static JobRVO<MovementPlaneWrapper>.LinearProgram2Output LinearProgram2DCollapsedSegment(NativeArray<JobRVO<MovementPlaneWrapper>.ORCALine> lines, int numLines, int startLine, float radius, float2 currentResult, float2 optimalVelocityStart, float2 optimalVelocityDir, float optimalTLeft, float optimalTRight)
		{
			for (int i = startLine; i < numLines; i++)
			{
				if (JobRVO<MovementPlaneWrapper>.det(lines[i].direction, lines[i].point - currentResult) > 0f)
				{
					float num;
					float num2;
					if (!JobRVO<MovementPlaneWrapper>.ClipBoundary(lines, i, radius, out num, out num2))
					{
						return new JobRVO<MovementPlaneWrapper>.LinearProgram2Output
						{
							velocity = currentResult,
							firstFailedLineIndex = i
						};
					}
					currentResult = JobRVO<MovementPlaneWrapper>.ClosestSegmentSegmentPointNonIntersecting(lines[i], new JobRVO<MovementPlaneWrapper>.ORCALine
					{
						point = optimalVelocityStart,
						direction = optimalVelocityDir
					}, num, num2, optimalTLeft, optimalTRight);
				}
			}
			return new JobRVO<MovementPlaneWrapper>.LinearProgram2Output
			{
				velocity = currentResult,
				firstFailedLineIndex = numLines
			};
		}

		// Token: 0x06001148 RID: 4424 RVA: 0x0006A86C File Offset: 0x00068A6C
		private static JobRVO<MovementPlaneWrapper>.LinearProgram2Output LinearProgram2DSegment(NativeArray<JobRVO<MovementPlaneWrapper>.ORCALine> lines, int numLines, float radius, float2 optimalVelocityStart, float2 optimalVelocityDir, float optimalTLeft, float optimalTRight, float optimalT)
		{
			float num;
			float num2;
			bool flag = VectorMath.LineCircleIntersectionFactors(optimalVelocityStart, optimalVelocityDir, radius, out num, out num2);
			num = math.max(num, optimalTLeft);
			num2 = math.min(num2, optimalTRight);
			if (!(flag & (num <= num2)))
			{
				float num3 = math.clamp(math.dot(-optimalVelocityStart, optimalVelocityDir), optimalTLeft, optimalTRight);
				float2 @float = math.normalizesafe(optimalVelocityStart + optimalVelocityDir * num3, default(float2)) * radius;
				return JobRVO<MovementPlaneWrapper>.LinearProgram2DCollapsedSegment(lines, numLines, 0, radius, @float, optimalVelocityStart, optimalVelocityDir, optimalTLeft, optimalTRight);
			}
			for (int i = 0; i < numLines; i++)
			{
				JobRVO<MovementPlaneWrapper>.ORCALine orcaline = lines[i];
				bool flag2 = JobRVO<MovementPlaneWrapper>.det(orcaline.direction, orcaline.point - (optimalVelocityStart + optimalVelocityDir * num)) > 0f;
				bool flag3 = JobRVO<MovementPlaneWrapper>.det(orcaline.direction, orcaline.point - (optimalVelocityStart + optimalVelocityDir * num2)) > 0f;
				if (flag2 || flag3)
				{
					float num4;
					float num5;
					if (!JobRVO<MovementPlaneWrapper>.ClipBoundary(lines, i, radius, out num4, out num5))
					{
						return new JobRVO<MovementPlaneWrapper>.LinearProgram2Output
						{
							velocity = optimalVelocityStart + optimalVelocityDir * math.clamp(optimalT, num, num2),
							firstFailedLineIndex = i
						};
					}
					if (flag2 && flag3)
					{
						if (math.abs(JobRVO<MovementPlaneWrapper>.det(orcaline.direction, optimalVelocityDir)) >= 0.001f)
						{
							float2 float2 = JobRVO<MovementPlaneWrapper>.ClosestSegmentSegmentPointNonIntersecting(orcaline, new JobRVO<MovementPlaneWrapper>.ORCALine
							{
								point = optimalVelocityStart,
								direction = optimalVelocityDir
							}, num4, num5, optimalTLeft, optimalTRight);
							return JobRVO<MovementPlaneWrapper>.LinearProgram2DCollapsedSegment(lines, numLines, i + 1, radius, float2, optimalVelocityStart, optimalVelocityDir, optimalTLeft, optimalTRight);
						}
						float num6 = JobRVO<MovementPlaneWrapper>.ClosestPointOnSegment(orcaline.point, orcaline.direction, optimalVelocityStart + optimalVelocityDir * num, num4, num5);
						float num7 = JobRVO<MovementPlaneWrapper>.ClosestPointOnSegment(orcaline.point, orcaline.direction, optimalVelocityStart + optimalVelocityDir * num2, num4, num5);
						float num8 = JobRVO<MovementPlaneWrapper>.ClosestPointOnSegment(orcaline.point, orcaline.direction, optimalVelocityStart + optimalVelocityDir * optimalT, num4, num5);
						optimalVelocityStart = orcaline.point;
						optimalVelocityDir = orcaline.direction;
						num = num6;
						num2 = num7;
						optimalT = num8;
					}
					else
					{
						JobRVO<MovementPlaneWrapper>.ClipLine(new JobRVO<MovementPlaneWrapper>.ORCALine
						{
							point = optimalVelocityStart,
							direction = optimalVelocityDir
						}, orcaline, ref num, ref num2);
					}
				}
			}
			float num9 = math.clamp(optimalT, num, num2);
			return new JobRVO<MovementPlaneWrapper>.LinearProgram2Output
			{
				velocity = optimalVelocityStart + optimalVelocityDir * num9,
				firstFailedLineIndex = numLines
			};
		}

		// Token: 0x06001149 RID: 4425 RVA: 0x0006AB0C File Offset: 0x00068D0C
		private static void LinearProgram3D(NativeArray<JobRVO<MovementPlaneWrapper>.ORCALine> lines, int numLines, int numFixedLines, int beginLine, float radius, ref float2 result, NativeArray<JobRVO<MovementPlaneWrapper>.ORCALine> scratchBuffer)
		{
			float num = 0f;
			NativeArray<JobRVO<MovementPlaneWrapper>.ORCALine> nativeArray = scratchBuffer;
			NativeArray<JobRVO<MovementPlaneWrapper>.ORCALine>.Copy(lines, nativeArray, numFixedLines);
			for (int i = beginLine; i < numLines; i++)
			{
				if (JobRVO<MovementPlaneWrapper>.det(lines[i].direction, lines[i].point - result) > num)
				{
					int num2 = numFixedLines;
					for (int j = numFixedLines; j < i; j++)
					{
						float num3 = JobRVO<MovementPlaneWrapper>.det(lines[i].direction, lines[j].direction);
						if (math.abs(num3) < 0.001f)
						{
							if (math.dot(lines[i].direction, lines[j].direction) <= 0f)
							{
								nativeArray[num2] = new JobRVO<MovementPlaneWrapper>.ORCALine
								{
									point = 0.5f * (lines[i].point + lines[j].point),
									direction = math.normalize(lines[j].direction - lines[i].direction)
								};
								num2++;
							}
						}
						else
						{
							nativeArray[num2] = new JobRVO<MovementPlaneWrapper>.ORCALine
							{
								point = lines[i].point + JobRVO<MovementPlaneWrapper>.det(lines[j].direction, lines[i].point - lines[j].point) / num3 * lines[i].direction,
								direction = math.normalize(lines[j].direction - lines[i].direction)
							};
							num2++;
						}
					}
					JobRVO<MovementPlaneWrapper>.LinearProgram2Output linearProgram2Output = JobRVO<MovementPlaneWrapper>.LinearProgram2D(nativeArray, num2, radius, new float2(-lines[i].direction.y, lines[i].direction.x), true);
					if (linearProgram2Output.firstFailedLineIndex >= num2)
					{
						result = linearProgram2Output.velocity;
					}
					num = JobRVO<MovementPlaneWrapper>.det(lines[i].direction, lines[i].point - result);
				}
			}
		}

		// Token: 0x0600114A RID: 4426 RVA: 0x00003788 File Offset: 0x00001988
		private static void DrawVO(CommandBuilder draw, float2 circleCenter, float radius, float2 origin, Color color)
		{
		}

		// Token: 0x04000C86 RID: 3206
		[ReadOnly]
		public SimulatorBurst.AgentData agentData;

		// Token: 0x04000C87 RID: 3207
		[ReadOnly]
		public SimulatorBurst.TemporaryAgentData temporaryAgentData;

		// Token: 0x04000C88 RID: 3208
		[ReadOnly]
		public NavmeshEdges.NavmeshBorderData navmeshEdgeData;

		// Token: 0x04000C89 RID: 3209
		[WriteOnly]
		public SimulatorBurst.AgentOutputData output;

		// Token: 0x04000C8A RID: 3210
		public float deltaTime;

		// Token: 0x04000C8B RID: 3211
		public float symmetryBreakingBias;

		// Token: 0x04000C8C RID: 3212
		public float priorityMultiplier;

		// Token: 0x04000C8D RID: 3213
		public bool useNavmeshAsObstacle;

		// Token: 0x04000C8E RID: 3214
		private const int MaxObstacleCount = 50;

		// Token: 0x04000C8F RID: 3215
		private static readonly ProfilerMarker MarkerConvertObstacles1 = new ProfilerMarker("RVOConvertObstacles1");

		// Token: 0x04000C90 RID: 3216
		private static readonly ProfilerMarker MarkerConvertObstacles2 = new ProfilerMarker("RVOConvertObstacles2");

		// Token: 0x020002B8 RID: 696
		private struct SortByKey : IComparer<int>
		{
			// Token: 0x0600114C RID: 4428 RVA: 0x0006AD97 File Offset: 0x00068F97
			public unsafe int Compare(int x, int y)
			{
				return this.keys[x].CompareTo(*this.keys[y]);
			}

			// Token: 0x04000C91 RID: 3217
			public UnsafeSpan<float> keys;
		}

		// Token: 0x020002B9 RID: 697
		private struct ORCALine
		{
			// Token: 0x0600114D RID: 4429 RVA: 0x0006ADB8 File Offset: 0x00068FB8
			public void DrawAsHalfPlane(CommandBuilder draw, float halfPlaneLength, float halfPlaneWidth, Color color)
			{
				float2 @float = new float2(this.direction.y, -this.direction.x);
				draw.xy.Line(this.point - this.direction * 10f, this.point + this.direction * 10f, color);
				float2 float2 = this.point + @float * halfPlaneWidth * 0.5f;
				draw.SolidBox(new float3(float2, 0f), quaternion.RotateZ(math.atan2(this.direction.y, this.direction.x)), new float3(halfPlaneLength, halfPlaneWidth, 0.01f), new Color(0f, 0f, 0f, 0.5f));
			}

			// Token: 0x0600114E RID: 4430 RVA: 0x0006AEA0 File Offset: 0x000690A0
			public ORCALine(float2 position, float2 relativePosition, float2 velocity, float2 otherVelocity, float combinedRadius, float timeStep, float invTimeHorizon)
			{
				float2 @float = velocity - otherVelocity;
				float num = combinedRadius * combinedRadius;
				float num2 = math.lengthsq(relativePosition);
				if (num2 <= num)
				{
					float num3 = math.rcp(timeStep);
					float num4 = math.sqrt(num2);
					float2 float2 = math.select(0, relativePosition / num4, num4 > 1.1754944E-38f) * (num4 - combinedRadius - 0.001f) * 0.3f * num3;
					this.direction = math.normalizesafe(new float2(float2.y, -float2.x), default(float2));
					this.point = math.lerp(velocity, otherVelocity, 0.5f) + float2 * 0.5f;
					return;
				}
				combinedRadius *= 1.001f;
				float2 float3 = @float - invTimeHorizon * relativePosition;
				float num5 = math.lengthsq(float3);
				float num6 = math.dot(float3, relativePosition);
				if (num6 < 0f && num6 * num6 > num * num5)
				{
					float num7 = math.sqrt(num5);
					float2 float4 = float3 / num7;
					this.direction = new float2(float4.y, -float4.x);
					float2 float5 = (combinedRadius * invTimeHorizon - num7) * float4;
					this.point = velocity + 0.5f * float5;
					return;
				}
				float num8 = math.sqrt(num2 - num);
				if (JobRVO<MovementPlaneWrapper>.det(relativePosition, float3) > 0f)
				{
					this.direction = (relativePosition * num8 + new float2(-relativePosition.y, relativePosition.x) * combinedRadius) / num2;
				}
				else
				{
					this.direction = (-relativePosition * num8 + new float2(-relativePosition.y, relativePosition.x) * combinedRadius) / num2;
				}
				float2 float6 = math.dot(@float, this.direction) * this.direction - @float;
				this.point = velocity + 0.5f * float6;
			}

			// Token: 0x04000C92 RID: 3218
			public float2 point;

			// Token: 0x04000C93 RID: 3219
			public float2 direction;
		}

		// Token: 0x020002BA RID: 698
		private struct LinearProgram2Output
		{
			// Token: 0x04000C94 RID: 3220
			public float2 velocity;

			// Token: 0x04000C95 RID: 3221
			public int firstFailedLineIndex;
		}
	}
}
