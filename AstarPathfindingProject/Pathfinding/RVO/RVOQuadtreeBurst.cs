using System;
using Pathfinding.Drawing;
using Pathfinding.ECS.RVO;
using Pathfinding.Util;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;

namespace Pathfinding.RVO
{
	// Token: 0x020002D3 RID: 723
	public struct RVOQuadtreeBurst
	{
		// Token: 0x060011F2 RID: 4594 RVA: 0x0006D558 File Offset: 0x0006B758
		static RVOQuadtreeBurst()
		{
			for (int i = 0; i < 256; i++)
			{
				for (int j = 0; j < 8; j++)
				{
					if (((i >> j) & 1) != 0)
					{
						RVOQuadtreeBurst.ChildLookup[i] = (byte)j;
						break;
					}
				}
			}
		}

		// Token: 0x170002B6 RID: 694
		// (get) Token: 0x060011F3 RID: 4595 RVA: 0x0006D5A8 File Offset: 0x0006B7A8
		public Rect bounds
		{
			get
			{
				if (!this.boundingBoxBuffer.IsCreated)
				{
					return default(Rect);
				}
				return Rect.MinMaxRect(this.boundingBoxBuffer[0].x, this.boundingBoxBuffer[0].y, this.boundingBoxBuffer[1].x, this.boundingBoxBuffer[1].y);
			}
		}

		// Token: 0x060011F4 RID: 4596 RVA: 0x0006D615 File Offset: 0x0006B815
		private static int InnerNodeCountUpperBound(int numAgents, MovementPlane movementPlane)
		{
			return (((movementPlane == MovementPlane.Arbitrary) ? 8 : 4) * 10 * numAgents + 16 - 1) / 16;
		}

		// Token: 0x060011F5 RID: 4597 RVA: 0x0006D62C File Offset: 0x0006B82C
		public void Dispose()
		{
			this.agents.Dispose();
			this.childPointers.Dispose();
			this.boundingBoxBuffer.Dispose();
			this.agentCountBuffer.Dispose();
			this.maxSpeeds.Dispose();
			this.maxRadius.Dispose();
			this.nodeAreas.Dispose();
			this.agentPositions.Dispose();
			this.agentRadii.Dispose();
		}

		// Token: 0x060011F6 RID: 4598 RVA: 0x0006D69C File Offset: 0x0006B89C
		private void Reserve(int minSize)
		{
			if (!this.boundingBoxBuffer.IsCreated)
			{
				this.boundingBoxBuffer = new NativeArray<float3>(4, Allocator.Persistent, NativeArrayOptions.ClearMemory);
				this.agentCountBuffer = new NativeArray<int>(1, Allocator.Persistent, NativeArrayOptions.ClearMemory);
			}
			int num = math.ceilpow2(minSize);
			Memory.Realloc<int>(ref this.agents, num, Allocator.Persistent, NativeArrayOptions.ClearMemory);
			Memory.Realloc<float3>(ref this.agentPositions, num, Allocator.Persistent, NativeArrayOptions.ClearMemory);
			Memory.Realloc<float>(ref this.agentRadii, num, Allocator.Persistent, NativeArrayOptions.ClearMemory);
			Memory.Realloc<int>(ref this.childPointers, RVOQuadtreeBurst.InnerNodeCountUpperBound(num, this.movementPlane), Allocator.Persistent, NativeArrayOptions.ClearMemory);
			Memory.Realloc<float>(ref this.maxSpeeds, this.childPointers.Length, Allocator.Persistent, NativeArrayOptions.ClearMemory);
			Memory.Realloc<float>(ref this.nodeAreas, this.childPointers.Length, Allocator.Persistent, NativeArrayOptions.ClearMemory);
			Memory.Realloc<float>(ref this.maxRadius, this.childPointers.Length, Allocator.Persistent, NativeArrayOptions.ClearMemory);
		}

		// Token: 0x060011F7 RID: 4599 RVA: 0x0006D764 File Offset: 0x0006B964
		public RVOQuadtreeBurst.JobBuild BuildJob(NativeArray<float3> agentPositions, NativeArray<AgentIndex> agentVersions, NativeArray<float> agentSpeeds, NativeArray<float> agentRadii, int numAgents, MovementPlane movementPlane)
		{
			if (numAgents >= 32767)
			{
				throw new Exception("Too many agents. Cannot have more than " + 32767.ToString());
			}
			this.Reserve(numAgents);
			this.movementPlane = movementPlane;
			return new RVOQuadtreeBurst.JobBuild
			{
				agents = this.agents,
				agentVersions = agentVersions,
				agentPositions = agentPositions,
				agentSpeeds = agentSpeeds,
				agentRadii = agentRadii,
				outMaxSpeeds = this.maxSpeeds,
				outMaxRadius = this.maxRadius,
				outArea = this.nodeAreas,
				outAgentRadii = this.agentRadii,
				outAgentPositions = this.agentPositions,
				outBoundingBox = this.boundingBoxBuffer,
				outAgentCount = this.agentCountBuffer,
				outChildPointers = this.childPointers,
				numAgents = numAgents,
				movementPlane = movementPlane
			};
		}

		// Token: 0x060011F8 RID: 4600 RVA: 0x0006D858 File Offset: 0x0006BA58
		public int QueryKNearest(RVOQuadtreeBurst.QuadtreeQuery query)
		{
			if (!this.agents.IsCreated)
			{
				return 0;
			}
			float num = 1E+30f;
			for (int i = 0; i < query.maxCount; i++)
			{
				query.result[query.outputStartIndex + i] = -1;
			}
			for (int j = 0; j < query.maxCount; j++)
			{
				query.resultDistances[j] = 1E+30f;
			}
			this.QueryRec(ref query, 0, this.boundingBoxBuffer[0], this.boundingBoxBuffer[1], ref num);
			int num2 = 0;
			while (num2 < query.maxCount && query.resultDistances[num2] < 1E+30f)
			{
				num2++;
			}
			return num2;
		}

		// Token: 0x060011F9 RID: 4601 RVA: 0x0006D90C File Offset: 0x0006BB0C
		private void QueryRec(ref RVOQuadtreeBurst.QuadtreeQuery query, int treeNodeIndex, float3 nodeMin, float3 nodeMax, ref float maxRadius)
		{
			float num = math.min(math.max((this.maxSpeeds[treeNodeIndex] + query.speed) * query.timeHorizon, query.agentRadius) + query.agentRadius, maxRadius);
			float3 position = query.position;
			if ((this.childPointers[treeNodeIndex] & 1073741824) != 0)
			{
				int maxCount = query.maxCount;
				int num2 = this.childPointers[treeNodeIndex] & 32767;
				int num3 = (this.childPointers[treeNodeIndex] >> 15) & 32767;
				NativeArray<int> result = query.result;
				NativeArray<float> resultDistances = query.resultDistances;
				for (int i = num2; i < num3; i++)
				{
					int num4 = this.agents[i];
					float num5 = math.lengthsq(position - this.agentPositions[num4]);
					if (num5 < num * num && (query.layers[num4] & query.layerMask) != (RVOLayer)0)
					{
						int j = 0;
						while (j < maxCount)
						{
							if (num5 < resultDistances[j])
							{
								for (int k = maxCount - 1; k > j; k--)
								{
									result[query.outputStartIndex + k] = result[query.outputStartIndex + k - 1];
									resultDistances[k] = resultDistances[k - 1];
								}
								result[query.outputStartIndex + j] = num4;
								resultDistances[j] = num5;
								if (j == maxCount - 1)
								{
									maxRadius = math.min(maxRadius, math.sqrt(num5));
									num = math.min(num, maxRadius);
									break;
								}
								break;
							}
							else
							{
								j++;
							}
						}
					}
				}
				return;
			}
			int num6 = this.childPointers[treeNodeIndex];
			float3 @float = (nodeMin + nodeMax) * 0.5f;
			if (this.movementPlane == MovementPlane.Arbitrary)
			{
				int num7 = ((position.x < @float.x) ? 0 : 4) | ((position.y < @float.y) ? 0 : 2) | ((position.z < @float.z) ? 0 : 1);
				bool3 @bool = new bool3((num7 & 4) != 0, (num7 & 2) != 0, (num7 & 1) != 0);
				float3 float2 = math.select(nodeMin, @float, @bool);
				float3 float3 = math.select(@float, nodeMax, @bool);
				this.QueryRec(ref query, num6 + num7, float2, float3, ref maxRadius);
				num = math.min(num, maxRadius);
				bool3 bool2 = position - num < @float;
				bool3 bool3 = position + num > @float;
				int3 @int = math.select(new int3(240, 204, 170), new int3(255, 255, 255), bool2);
				int3 int2 = math.select(new int3(15, 51, 85), new int3(255, 255, 255), bool3);
				int3 int3 = @int & int2;
				int num8 = int3.x & int3.y & int3.z;
				byte b;
				for (num8 &= ~(1 << num7); num8 != 0; num8 &= ~(1 << (int)b))
				{
					b = RVOQuadtreeBurst.ChildLookup[num8];
					bool3 bool4 = new bool3((b & 4) > 0, (b & 2) > 0, (b & 1) > 0);
					float3 float4 = math.select(nodeMin, @float, bool4);
					float3 float5 = math.select(@float, nodeMax, bool4);
					this.QueryRec(ref query, num6 + (int)b, float4, float5, ref maxRadius);
					num = math.min(num, maxRadius);
				}
				return;
			}
			if (this.movementPlane == MovementPlane.XY)
			{
				int num9 = ((position.x < @float.x) ? 0 : 2) | ((position.y < @float.y) ? 0 : 1);
				bool3 bool5 = new bool3((num9 & 2) != 0, (num9 & 1) != 0, false);
				float3 float6 = math.select(nodeMin, @float, bool5);
				float3 float7 = math.select(@float, nodeMax, bool5);
				this.QueryRec(ref query, num6 + num9, float6, float7, ref maxRadius);
				num = math.min(num, maxRadius);
				bool2 bool6 = position.xy - num < @float.xy;
				bool2 bool7 = position.xy + num > @float.xy;
				bool4 bool8 = new bool4(bool6.x & bool6.y, bool6.x & bool7.y, bool7.x & bool6.y, bool7.x & bool7.y);
				int num10 = (bool8.x ? 1 : 0) | (bool8.y ? 2 : 0) | (bool8.z ? 4 : 0) | (bool8.w ? 8 : 0);
				byte b2;
				for (num10 &= ~(1 << num9); num10 != 0; num10 &= ~(1 << (int)b2))
				{
					b2 = RVOQuadtreeBurst.ChildLookup[num10];
					bool3 bool9 = new bool3((b2 & 2) > 0, (b2 & 1) > 0, false);
					float3 float8 = math.select(nodeMin, @float, bool9);
					float3 float9 = math.select(@float, nodeMax, bool9);
					this.QueryRec(ref query, num6 + (int)b2, float8, float9, ref maxRadius);
					num = math.min(num, maxRadius);
				}
				return;
			}
			int num11 = ((position.x < @float.x) ? 0 : 2) | ((position.z < @float.z) ? 0 : 1);
			bool3 bool10 = new bool3((num11 & 2) != 0, false, (num11 & 1) != 0);
			float3 float10 = math.select(nodeMin, @float, bool10);
			float3 float11 = math.select(@float, nodeMax, bool10);
			this.QueryRec(ref query, num6 + num11, float10, float11, ref maxRadius);
			num = math.min(num, maxRadius);
			bool2 bool11 = position.xz - num < @float.xz;
			bool2 bool12 = position.xz + num > @float.xz;
			bool4 bool13 = new bool4(bool11.x & bool11.y, bool11.x & bool12.y, bool12.x & bool11.y, bool12.x & bool12.y);
			int num12 = (bool13.x ? 1 : 0) | (bool13.y ? 2 : 0) | (bool13.z ? 4 : 0) | (bool13.w ? 8 : 0);
			byte b3;
			for (num12 &= ~(1 << num11); num12 != 0; num12 &= ~(1 << (int)b3))
			{
				b3 = RVOQuadtreeBurst.ChildLookup[num12];
				bool3 bool14 = new bool3((b3 & 2) > 0, false, (b3 & 1) > 0);
				float3 float12 = math.select(nodeMin, @float, bool14);
				float3 float13 = math.select(@float, nodeMax, bool14);
				this.QueryRec(ref query, num6 + (int)b3, float12, float13, ref maxRadius);
				num = math.min(num, maxRadius);
			}
		}

		// Token: 0x060011FA RID: 4602 RVA: 0x0006DFD4 File Offset: 0x0006C1D4
		public float QueryArea(float3 position, float radius)
		{
			if (!this.agents.IsCreated || this.agentCountBuffer[0] == 0)
			{
				return 0f;
			}
			return 3.1415927f * this.QueryAreaRec(0, position, radius, this.boundingBoxBuffer[0], this.boundingBoxBuffer[1]);
		}

		// Token: 0x060011FB RID: 4603 RVA: 0x0006E02C File Offset: 0x0006C22C
		private float QueryAreaRec(int treeNodeIndex, float3 p, float radius, float3 nodeMin, float3 nodeMax)
		{
			float3 @float = (nodeMin + nodeMax) * 0.5f;
			float num = math.length(nodeMax - @float);
			float num2 = math.lengthsq(@float - p);
			float num3 = this.maxRadius[treeNodeIndex];
			float num4 = radius - (num + num3);
			if (num4 > 0f && num2 < num4 * num4)
			{
				return this.nodeAreas[treeNodeIndex];
			}
			if (num2 > (radius + (num + num3)) * (radius + (num + num3)))
			{
				return 0f;
			}
			if ((this.childPointers[treeNodeIndex] & 1073741824) != 0)
			{
				int num5 = this.childPointers[treeNodeIndex] & 32767;
				int num6 = (this.childPointers[treeNodeIndex] >> 15) & 32767;
				float num7 = 0f;
				float num8 = 0f;
				for (int i = num5; i < num6; i++)
				{
					int num9 = this.agents[i];
					num7 += this.agentRadii[num9] * this.agentRadii[num9];
					float num10 = math.lengthsq(p - this.agentPositions[num9]);
					float num11 = this.agentRadii[num9];
					if (num10 < (radius + num11) * (radius + num11))
					{
						float num12 = radius - num11;
						float num13 = ((num10 < num12 * num12) ? 1f : (1f - (math.sqrt(num10) - num12) / (2f * num11)));
						num8 += num11 * num11 * num13;
					}
				}
				return num8;
			}
			float num14 = 0f;
			int num15 = this.childPointers[treeNodeIndex];
			float num16 = radius + num3;
			if (this.movementPlane == MovementPlane.Arbitrary)
			{
				bool3 @bool = p - num16 < @float;
				bool3 bool2 = p + num16 > @float;
				if (@bool[0])
				{
					if (@bool[1])
					{
						if (@bool[2])
						{
							num14 += this.QueryAreaRec(num15, p, radius, new float3(nodeMin.x, nodeMin.y, nodeMin.z), new float3(@float.x, @float.y, @float.z));
						}
						if (bool2[2])
						{
							num14 += this.QueryAreaRec(num15 + 1, p, radius, new float3(nodeMin.x, nodeMin.y, @float.z), new float3(@float.x, @float.y, nodeMax.z));
						}
					}
					if (bool2[1])
					{
						if (@bool[2])
						{
							num14 += this.QueryAreaRec(num15 + 2, p, radius, new float3(nodeMin.x, @float.y, nodeMin.z), new float3(@float.x, nodeMax.y, @float.z));
						}
						if (bool2[2])
						{
							num14 += this.QueryAreaRec(num15 + 3, p, radius, new float3(nodeMin.x, @float.y, @float.z), new float3(@float.x, nodeMax.y, nodeMax.z));
						}
					}
				}
				if (bool2[0])
				{
					if (@bool[1])
					{
						if (@bool[2])
						{
							num14 += this.QueryAreaRec(num15 + 4, p, radius, new float3(@float.x, nodeMin.y, nodeMin.z), new float3(nodeMax.x, @float.y, @float.z));
						}
						if (bool2[2])
						{
							num14 += this.QueryAreaRec(num15 + 5, p, radius, new float3(@float.x, nodeMin.y, @float.z), new float3(nodeMax.x, @float.y, nodeMax.z));
						}
					}
					if (bool2[1])
					{
						if (@bool[2])
						{
							num14 += this.QueryAreaRec(num15 + 6, p, radius, new float3(@float.x, @float.y, nodeMin.z), new float3(nodeMax.x, nodeMax.y, @float.z));
						}
						if (bool2[2])
						{
							num14 += this.QueryAreaRec(num15 + 7, p, radius, new float3(@float.x, @float.y, @float.z), new float3(nodeMax.x, nodeMax.y, nodeMax.z));
						}
					}
				}
			}
			else if (this.movementPlane == MovementPlane.XY)
			{
				bool2 bool3 = (p - num16).xy < @float.xy;
				bool2 bool4 = (p + num16).xy > @float.xy;
				if (bool3[0])
				{
					if (bool3[1])
					{
						num14 += this.QueryAreaRec(num15, p, radius, new float3(nodeMin.x, nodeMin.y, nodeMin.z), new float3(@float.x, @float.y, nodeMax.z));
					}
					if (bool4[1])
					{
						num14 += this.QueryAreaRec(num15 + 1, p, radius, new float3(nodeMin.x, @float.y, nodeMin.z), new float3(@float.x, nodeMax.y, nodeMax.z));
					}
				}
				if (bool4[0])
				{
					if (bool3[1])
					{
						num14 += this.QueryAreaRec(num15 + 2, p, radius, new float3(@float.x, nodeMin.y, nodeMin.z), new float3(nodeMax.x, @float.y, nodeMax.z));
					}
					if (bool4[1])
					{
						num14 += this.QueryAreaRec(num15 + 3, p, radius, new float3(@float.x, @float.y, nodeMin.z), new float3(nodeMax.x, nodeMax.y, nodeMax.z));
					}
				}
			}
			else
			{
				bool2 bool5 = (p - num16).xz < @float.xz;
				bool2 bool6 = (p + num16).xz > @float.xz;
				if (bool5[0])
				{
					if (bool5[1])
					{
						num14 += this.QueryAreaRec(num15, p, radius, new float3(nodeMin.x, nodeMin.y, nodeMin.z), new float3(@float.x, nodeMax.y, @float.z));
					}
					if (bool6[1])
					{
						num14 += this.QueryAreaRec(num15 + 1, p, radius, new float3(nodeMin.x, nodeMin.y, @float.z), new float3(@float.x, nodeMax.y, nodeMax.z));
					}
				}
				if (bool6[0])
				{
					if (bool5[1])
					{
						num14 += this.QueryAreaRec(num15 + 2, p, radius, new float3(@float.x, nodeMin.y, nodeMin.z), new float3(nodeMax.x, nodeMax.y, @float.z));
					}
					if (bool6[1])
					{
						num14 += this.QueryAreaRec(num15 + 3, p, radius, new float3(@float.x, nodeMin.y, @float.z), new float3(nodeMax.x, nodeMax.y, nodeMax.z));
					}
				}
			}
			return num14;
		}

		// Token: 0x060011FC RID: 4604 RVA: 0x0006E7DC File Offset: 0x0006C9DC
		public void DebugDraw(CommandBuilder draw)
		{
			if (!this.agentCountBuffer.IsCreated)
			{
				return;
			}
			int num = this.agentCountBuffer[0];
			if (num == 0)
			{
				return;
			}
			this.DebugDraw(0, this.boundingBoxBuffer[0], this.boundingBoxBuffer[1], draw);
			for (int i = 0; i < num; i++)
			{
				draw.Cross(this.agentPositions[this.agents[i]], 0.5f, Palette.Colorbrewer.Set1.Red);
			}
		}

		// Token: 0x060011FD RID: 4605 RVA: 0x0006E85C File Offset: 0x0006CA5C
		private void DebugDraw(int nodeIndex, float3 nodeMin, float3 nodeMax, CommandBuilder draw)
		{
			float3 @float = (nodeMin + nodeMax) * 0.5f;
			draw.WireBox(@float, nodeMax - nodeMin, Palette.Colorbrewer.Set1.Orange);
			if ((this.childPointers[nodeIndex] & 1073741824) != 0)
			{
				int num = this.childPointers[nodeIndex] & 32767;
				int num2 = (this.childPointers[nodeIndex] >> 15) & 32767;
				for (int i = num; i < num2; i++)
				{
					draw.Line(@float, this.agentPositions[this.agents[i]], Color.black);
				}
				return;
			}
			int num3 = this.childPointers[nodeIndex];
			if (this.movementPlane == MovementPlane.Arbitrary)
			{
				this.DebugDraw(num3, new float3(nodeMin.x, nodeMin.y, nodeMin.z), new float3(@float.x, @float.y, @float.z), draw);
				this.DebugDraw(num3 + 1, new float3(nodeMin.x, nodeMin.y, @float.z), new float3(@float.x, @float.y, nodeMax.z), draw);
				this.DebugDraw(num3 + 2, new float3(nodeMin.x, @float.y, nodeMin.z), new float3(@float.x, nodeMax.y, @float.z), draw);
				this.DebugDraw(num3 + 3, new float3(nodeMin.x, @float.y, @float.z), new float3(@float.x, nodeMax.y, nodeMax.z), draw);
				this.DebugDraw(num3 + 4, new float3(@float.x, nodeMin.y, nodeMin.z), new float3(nodeMax.x, @float.y, @float.z), draw);
				this.DebugDraw(num3 + 5, new float3(@float.x, nodeMin.y, @float.z), new float3(nodeMax.x, @float.y, nodeMax.z), draw);
				this.DebugDraw(num3 + 6, new float3(@float.x, @float.y, nodeMin.z), new float3(nodeMax.x, nodeMax.y, @float.z), draw);
				this.DebugDraw(num3 + 7, new float3(@float.x, @float.y, @float.z), new float3(nodeMax.x, nodeMax.y, nodeMax.z), draw);
				return;
			}
			if (this.movementPlane == MovementPlane.XY)
			{
				this.DebugDraw(num3, new float3(nodeMin.x, nodeMin.y, nodeMin.z), new float3(@float.x, @float.y, nodeMax.z), draw);
				this.DebugDraw(num3 + 1, new float3(nodeMin.x, @float.y, nodeMin.z), new float3(@float.x, nodeMax.y, nodeMax.z), draw);
				this.DebugDraw(num3 + 2, new float3(@float.x, nodeMin.y, nodeMin.z), new float3(nodeMax.x, @float.y, nodeMax.z), draw);
				this.DebugDraw(num3 + 3, new float3(@float.x, @float.y, nodeMin.z), new float3(nodeMax.x, nodeMax.y, nodeMax.z), draw);
				return;
			}
			this.DebugDraw(num3, new float3(nodeMin.x, nodeMin.y, nodeMin.z), new float3(@float.x, nodeMax.y, @float.z), draw);
			this.DebugDraw(num3 + 1, new float3(nodeMin.x, nodeMin.y, @float.z), new float3(@float.x, nodeMax.y, nodeMax.z), draw);
			this.DebugDraw(num3 + 2, new float3(@float.x, nodeMin.y, nodeMin.z), new float3(nodeMax.x, nodeMax.y, @float.z), draw);
			this.DebugDraw(num3 + 3, new float3(@float.x, nodeMin.y, @float.z), new float3(nodeMax.x, nodeMax.y, nodeMax.z), draw);
		}

		// Token: 0x04000D19 RID: 3353
		private const int LeafSize = 16;

		// Token: 0x04000D1A RID: 3354
		private const int MaxDepth = 10;

		// Token: 0x04000D1B RID: 3355
		private NativeArray<int> agents;

		// Token: 0x04000D1C RID: 3356
		private NativeArray<int> childPointers;

		// Token: 0x04000D1D RID: 3357
		private NativeArray<float3> boundingBoxBuffer;

		// Token: 0x04000D1E RID: 3358
		private NativeArray<int> agentCountBuffer;

		// Token: 0x04000D1F RID: 3359
		private NativeArray<float3> agentPositions;

		// Token: 0x04000D20 RID: 3360
		private NativeArray<float> agentRadii;

		// Token: 0x04000D21 RID: 3361
		private NativeArray<float> maxSpeeds;

		// Token: 0x04000D22 RID: 3362
		private NativeArray<float> maxRadius;

		// Token: 0x04000D23 RID: 3363
		private NativeArray<float> nodeAreas;

		// Token: 0x04000D24 RID: 3364
		private MovementPlane movementPlane;

		// Token: 0x04000D25 RID: 3365
		private const int LeafNodeBit = 1073741824;

		// Token: 0x04000D26 RID: 3366
		private const int BitPackingShift = 15;

		// Token: 0x04000D27 RID: 3367
		private const int BitPackingMask = 32767;

		// Token: 0x04000D28 RID: 3368
		private const int MaxAgents = 32767;

		// Token: 0x04000D29 RID: 3369
		private static readonly byte[] ChildLookup = new byte[256];

		// Token: 0x04000D2A RID: 3370
		private const float DistanceInfinity = 1E+30f;

		// Token: 0x020002D4 RID: 724
		[BurstCompile(CompileSynchronously = true, FloatMode = FloatMode.Fast)]
		public struct JobBuild : IJob
		{
			// Token: 0x060011FE RID: 4606 RVA: 0x0006ECAC File Offset: 0x0006CEAC
			private static int Partition(NativeSlice<int> indices, int startIndex, int endIndex, NativeSlice<float> coordinates, float splitPoint)
			{
				for (int i = startIndex; i < endIndex; i++)
				{
					if (coordinates[indices[i]] > splitPoint)
					{
						endIndex--;
						int num = indices[i];
						indices[i] = indices[endIndex];
						indices[endIndex] = num;
						i--;
					}
				}
				return endIndex;
			}

			// Token: 0x060011FF RID: 4607 RVA: 0x0006ED04 File Offset: 0x0006CF04
			private void BuildNode(float3 boundsMin, float3 boundsMax, int depth, int agentsStart, int agentsEnd, int nodeOffset, ref int firstFreeChild)
			{
				if (agentsEnd - agentsStart <= 16 || depth >= 10)
				{
					this.outChildPointers[nodeOffset] = agentsStart | (agentsEnd << 15) | 1073741824;
					return;
				}
				if (this.movementPlane == MovementPlane.Arbitrary)
				{
					NativeSlice<float> nativeSlice = new NativeSlice<float3>(this.agentPositions).SliceWithStride<float>(0);
					NativeSlice<float> nativeSlice2 = new NativeSlice<float3>(this.agentPositions).SliceWithStride<float>(4);
					NativeSlice<float> nativeSlice3 = new NativeSlice<float3>(this.agentPositions).SliceWithStride<float>(8);
					float3 @float = (boundsMin + boundsMax) * 0.5f;
					int num = RVOQuadtreeBurst.JobBuild.Partition(this.agents, agentsStart, agentsEnd, nativeSlice, @float.x);
					int num2 = RVOQuadtreeBurst.JobBuild.Partition(this.agents, agentsStart, num, nativeSlice2, @float.y);
					int num3 = RVOQuadtreeBurst.JobBuild.Partition(this.agents, num, agentsEnd, nativeSlice2, @float.y);
					int num4 = RVOQuadtreeBurst.JobBuild.Partition(this.agents, agentsStart, num2, nativeSlice3, @float.z);
					int num5 = RVOQuadtreeBurst.JobBuild.Partition(this.agents, num2, num, nativeSlice3, @float.z);
					int num6 = RVOQuadtreeBurst.JobBuild.Partition(this.agents, num, num3, nativeSlice3, @float.z);
					int num7 = RVOQuadtreeBurst.JobBuild.Partition(this.agents, num3, agentsEnd, nativeSlice3, @float.z);
					int num8 = firstFreeChild;
					this.outChildPointers[nodeOffset] = num8;
					firstFreeChild += 8;
					float3 float2 = @float;
					this.BuildNode(new float3(boundsMin.x, boundsMin.y, boundsMin.z), new float3(float2.x, float2.y, float2.z), depth + 1, agentsStart, num4, num8, ref firstFreeChild);
					this.BuildNode(new float3(boundsMin.x, boundsMin.y, float2.z), new float3(float2.x, float2.y, boundsMax.z), depth + 1, num4, num2, num8 + 1, ref firstFreeChild);
					this.BuildNode(new float3(boundsMin.x, float2.y, boundsMin.z), new float3(float2.x, boundsMax.y, float2.z), depth + 1, num2, num5, num8 + 2, ref firstFreeChild);
					this.BuildNode(new float3(boundsMin.x, float2.y, float2.z), new float3(float2.x, boundsMax.y, boundsMax.z), depth + 1, num5, num, num8 + 3, ref firstFreeChild);
					this.BuildNode(new float3(float2.x, boundsMin.y, boundsMin.z), new float3(boundsMax.x, float2.y, float2.z), depth + 1, num, num6, num8 + 4, ref firstFreeChild);
					this.BuildNode(new float3(float2.x, boundsMin.y, float2.z), new float3(boundsMax.x, float2.y, boundsMax.z), depth + 1, num6, num3, num8 + 5, ref firstFreeChild);
					this.BuildNode(new float3(float2.x, float2.y, boundsMin.z), new float3(boundsMax.x, boundsMax.y, float2.z), depth + 1, num3, num7, num8 + 6, ref firstFreeChild);
					this.BuildNode(new float3(float2.x, float2.y, float2.z), new float3(boundsMax.x, boundsMax.y, boundsMax.z), depth + 1, num7, agentsEnd, num8 + 7, ref firstFreeChild);
					return;
				}
				if (this.movementPlane == MovementPlane.XY)
				{
					NativeSlice<float> nativeSlice4 = new NativeSlice<float3>(this.agentPositions).SliceWithStride<float>(0);
					NativeSlice<float> nativeSlice5 = new NativeSlice<float3>(this.agentPositions).SliceWithStride<float>(4);
					float3 float3 = (boundsMin + boundsMax) * 0.5f;
					int num9 = RVOQuadtreeBurst.JobBuild.Partition(this.agents, agentsStart, agentsEnd, nativeSlice4, float3.x);
					int num10 = RVOQuadtreeBurst.JobBuild.Partition(this.agents, agentsStart, num9, nativeSlice5, float3.y);
					int num11 = RVOQuadtreeBurst.JobBuild.Partition(this.agents, num9, agentsEnd, nativeSlice5, float3.y);
					int num12 = firstFreeChild;
					this.outChildPointers[nodeOffset] = num12;
					firstFreeChild += 4;
					this.BuildNode(new float3(boundsMin.x, boundsMin.y, boundsMin.z), new float3(float3.x, float3.y, boundsMax.z), depth + 1, agentsStart, num10, num12, ref firstFreeChild);
					this.BuildNode(new float3(boundsMin.x, float3.y, boundsMin.z), new float3(float3.x, boundsMax.y, boundsMax.z), depth + 1, num10, num9, num12 + 1, ref firstFreeChild);
					this.BuildNode(new float3(float3.x, boundsMin.y, boundsMin.z), new float3(boundsMax.x, float3.y, boundsMax.z), depth + 1, num9, num11, num12 + 2, ref firstFreeChild);
					this.BuildNode(new float3(float3.x, float3.y, boundsMin.z), new float3(boundsMax.x, boundsMax.y, boundsMax.z), depth + 1, num11, agentsEnd, num12 + 3, ref firstFreeChild);
					return;
				}
				NativeSlice<float> nativeSlice6 = new NativeSlice<float3>(this.agentPositions).SliceWithStride<float>(0);
				NativeSlice<float> nativeSlice7 = new NativeSlice<float3>(this.agentPositions).SliceWithStride<float>(8);
				float3 float4 = (boundsMin + boundsMax) * 0.5f;
				int num13 = RVOQuadtreeBurst.JobBuild.Partition(this.agents, agentsStart, agentsEnd, nativeSlice6, float4.x);
				int num14 = RVOQuadtreeBurst.JobBuild.Partition(this.agents, agentsStart, num13, nativeSlice7, float4.z);
				int num15 = RVOQuadtreeBurst.JobBuild.Partition(this.agents, num13, agentsEnd, nativeSlice7, float4.z);
				int num16 = firstFreeChild;
				this.outChildPointers[nodeOffset] = num16;
				firstFreeChild += 4;
				this.BuildNode(new float3(boundsMin.x, boundsMin.y, boundsMin.z), new float3(float4.x, boundsMax.y, float4.z), depth + 1, agentsStart, num14, num16, ref firstFreeChild);
				this.BuildNode(new float3(boundsMin.x, boundsMin.y, float4.z), new float3(float4.x, boundsMax.y, boundsMax.z), depth + 1, num14, num13, num16 + 1, ref firstFreeChild);
				this.BuildNode(new float3(float4.x, boundsMin.y, boundsMin.z), new float3(boundsMax.x, boundsMax.y, float4.z), depth + 1, num13, num15, num16 + 2, ref firstFreeChild);
				this.BuildNode(new float3(float4.x, boundsMin.y, float4.z), new float3(boundsMax.x, boundsMax.y, boundsMax.z), depth + 1, num15, agentsEnd, num16 + 3, ref firstFreeChild);
			}

			// Token: 0x06001200 RID: 4608 RVA: 0x0006F478 File Offset: 0x0006D678
			private void CalculateSpeeds(int nodeCount)
			{
				for (int i = nodeCount - 1; i >= 0; i--)
				{
					if ((this.outChildPointers[i] & 1073741824) != 0)
					{
						int num = this.outChildPointers[i] & 32767;
						int num2 = (this.outChildPointers[i] >> 15) & 32767;
						float num3 = 0f;
						for (int j = num; j < num2; j++)
						{
							num3 = math.max(num3, this.agentSpeeds[this.agents[j]]);
						}
						this.outMaxSpeeds[i] = num3;
						float num4 = 0f;
						for (int k = num; k < num2; k++)
						{
							num4 = math.max(num4, this.agentRadii[this.agents[k]]);
						}
						this.outMaxRadius[i] = num4;
						float num5 = 0f;
						for (int l = num; l < num2; l++)
						{
							num5 += this.agentRadii[this.agents[l]] * this.agentRadii[this.agents[l]];
						}
						this.outArea[i] = num5;
					}
					else
					{
						int num6 = this.outChildPointers[i];
						if (this.movementPlane == MovementPlane.Arbitrary)
						{
							float num7 = 0f;
							float num8 = 0f;
							float num9 = 0f;
							for (int m = 0; m < 8; m++)
							{
								num7 = math.max(num7, this.outMaxSpeeds[num6 + m]);
								num8 = math.max(num8, this.outMaxSpeeds[num6 + m]);
								num9 += this.outArea[num6 + m];
							}
							this.outMaxSpeeds[i] = num7;
							this.outMaxRadius[i] = num8;
							this.outArea[i] = num9;
						}
						else
						{
							this.outMaxSpeeds[i] = math.max(math.max(this.outMaxSpeeds[num6], this.outMaxSpeeds[num6 + 1]), math.max(this.outMaxSpeeds[num6 + 2], this.outMaxSpeeds[num6 + 3]));
							this.outMaxRadius[i] = math.max(math.max(this.outMaxRadius[num6], this.outMaxRadius[num6 + 1]), math.max(this.outMaxRadius[num6 + 2], this.outMaxRadius[num6 + 3]));
							this.outArea[i] = this.outArea[num6] + this.outArea[num6 + 1] + this.outArea[num6 + 2] + this.outArea[num6 + 3];
						}
					}
				}
			}

			// Token: 0x06001201 RID: 4609 RVA: 0x0006F76C File Offset: 0x0006D96C
			public void Execute()
			{
				float3 @float = float.PositiveInfinity;
				float3 float2 = float.NegativeInfinity;
				int num = 0;
				for (int i = 0; i < this.numAgents; i++)
				{
					if (this.agentVersions[i].Valid)
					{
						this.agents[num++] = i;
						@float = math.min(@float, this.agentPositions[i]);
						float2 = math.max(float2, this.agentPositions[i]);
					}
				}
				this.outAgentCount[0] = num;
				if (num == 0)
				{
					this.outBoundingBox[0] = (this.outBoundingBox[1] = float3.zero);
					return;
				}
				this.outBoundingBox[0] = @float;
				this.outBoundingBox[1] = float2;
				int num2 = 1;
				this.BuildNode(@float, float2, 0, 0, num, 0, ref num2);
				this.CalculateSpeeds(num2);
				NativeArray<float3>.Copy(this.agentPositions, this.outAgentPositions, this.numAgents);
				NativeArray<float>.Copy(this.agentRadii, this.outAgentRadii, this.numAgents);
			}

			// Token: 0x04000D2B RID: 3371
			public NativeArray<int> agents;

			// Token: 0x04000D2C RID: 3372
			[ReadOnly]
			public NativeArray<float3> agentPositions;

			// Token: 0x04000D2D RID: 3373
			[ReadOnly]
			public NativeArray<AgentIndex> agentVersions;

			// Token: 0x04000D2E RID: 3374
			[ReadOnly]
			public NativeArray<float> agentSpeeds;

			// Token: 0x04000D2F RID: 3375
			[ReadOnly]
			public NativeArray<float> agentRadii;

			// Token: 0x04000D30 RID: 3376
			[WriteOnly]
			public NativeArray<float3> outBoundingBox;

			// Token: 0x04000D31 RID: 3377
			[WriteOnly]
			public NativeArray<int> outAgentCount;

			// Token: 0x04000D32 RID: 3378
			public NativeArray<int> outChildPointers;

			// Token: 0x04000D33 RID: 3379
			public NativeArray<float> outMaxSpeeds;

			// Token: 0x04000D34 RID: 3380
			public NativeArray<float> outMaxRadius;

			// Token: 0x04000D35 RID: 3381
			public NativeArray<float> outArea;

			// Token: 0x04000D36 RID: 3382
			[WriteOnly]
			public NativeArray<float3> outAgentPositions;

			// Token: 0x04000D37 RID: 3383
			[WriteOnly]
			public NativeArray<float> outAgentRadii;

			// Token: 0x04000D38 RID: 3384
			public int numAgents;

			// Token: 0x04000D39 RID: 3385
			public MovementPlane movementPlane;
		}

		// Token: 0x020002D5 RID: 725
		public struct QuadtreeQuery
		{
			// Token: 0x04000D3A RID: 3386
			public float3 position;

			// Token: 0x04000D3B RID: 3387
			public float speed;

			// Token: 0x04000D3C RID: 3388
			public float timeHorizon;

			// Token: 0x04000D3D RID: 3389
			public float agentRadius;

			// Token: 0x04000D3E RID: 3390
			public int outputStartIndex;

			// Token: 0x04000D3F RID: 3391
			public int maxCount;

			// Token: 0x04000D40 RID: 3392
			public RVOLayer layerMask;

			// Token: 0x04000D41 RID: 3393
			public NativeArray<RVOLayer> layers;

			// Token: 0x04000D42 RID: 3394
			public NativeArray<int> result;

			// Token: 0x04000D43 RID: 3395
			public NativeArray<float> resultDistances;
		}

		// Token: 0x020002D6 RID: 726
		[BurstCompile]
		public struct DebugDrawJob : IJob
		{
			// Token: 0x06001202 RID: 4610 RVA: 0x0006F88A File Offset: 0x0006DA8A
			public void Execute()
			{
				this.quadtree.DebugDraw(this.draw);
			}

			// Token: 0x04000D44 RID: 3396
			public CommandBuilder draw;

			// Token: 0x04000D45 RID: 3397
			[ReadOnly]
			public RVOQuadtreeBurst quadtree;
		}
	}
}
