using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Pathfinding.Collections;
using Pathfinding.Pooling;
using Unity.Burst;
using Unity.Collections;
using Unity.Mathematics;
using UnityEngine;

namespace Pathfinding
{
	// Token: 0x0200014B RID: 331
	[BurstCompile]
	public static class Funnel
	{
		// Token: 0x06000AB1 RID: 2737 RVA: 0x00038EFC File Offset: 0x000370FC
		public static List<Funnel.PathPart> SplitIntoParts(Path path)
		{
			List<GraphNode> path2 = path.path;
			List<Funnel.PathPart> list = ListPool<Funnel.PathPart>.Claim();
			if (path2 == null || path2.Count == 0)
			{
				return list;
			}
			for (int i = 0; i < path2.Count; i++)
			{
				GraphNode graphNode = path2[i];
				if (graphNode is TriangleMeshNode || graphNode is GridNodeBase)
				{
					int num = i;
					uint graphIndex = graphNode.GraphIndex;
					while (i < path2.Count && (path2[i].GraphIndex == graphIndex || path2[i] is NodeLink3Node))
					{
						i++;
					}
					i--;
					int num2 = i;
					List<Funnel.PathPart> list2 = list;
					Funnel.PathPart pathPart = new Funnel.PathPart
					{
						type = Funnel.PartType.NodeSequence,
						startIndex = num,
						endIndex = num2,
						startPoint = ((num == 0) ? path.vectorPath[0] : ((Vector3)path2[num - 1].position)),
						endPoint = ((num2 == path2.Count - 1) ? path.vectorPath[path.vectorPath.Count - 1] : ((Vector3)path2[num2 + 1].position))
					};
					list2.Add(pathPart);
				}
				else
				{
					if (!(graphNode is LinkNode))
					{
						throw new Exception("Unsupported node type or null node");
					}
					int num3 = i;
					uint graphIndex2 = graphNode.GraphIndex;
					while (i < path2.Count && path2[i].GraphIndex == graphIndex2)
					{
						i++;
					}
					i--;
					if (i - num3 == 0)
					{
						if (num3 > 0 && num3 + 1 < path2.Count && path2[num3 - 1] == path2[num3 + 1])
						{
							path2.RemoveRange(num3, 2);
							i--;
							throw new Exception("Link node connected back to the previous node in the path. This should not happen.");
						}
						path2.RemoveAt(num3);
						i--;
					}
					else
					{
						if (i - num3 != 1)
						{
							throw new Exception("Off mesh link included more than two nodes: " + (i - num3 + 1).ToString());
						}
						List<Funnel.PathPart> list3 = list;
						Funnel.PathPart pathPart = new Funnel.PathPart
						{
							type = Funnel.PartType.OffMeshLink,
							startIndex = num3,
							endIndex = i,
							startPoint = (Vector3)path2[num3].position,
							endPoint = (Vector3)path2[i].position
						};
						list3.Add(pathPart);
					}
				}
			}
			if (list[0].type == Funnel.PartType.OffMeshLink)
			{
				list.RemoveAt(0);
			}
			if (list[list.Count - 1].type == Funnel.PartType.OffMeshLink)
			{
				list.RemoveAt(list.Count - 1);
			}
			return list;
		}

		// Token: 0x06000AB2 RID: 2738 RVA: 0x00039188 File Offset: 0x00037388
		public static void Simplify(List<Funnel.PathPart> parts, ref List<GraphNode> nodes, ref TraversalCosts traversalCosts, ref TraversalConstraint traversalConstraint)
		{
			List<GraphNode> list = ListPool<GraphNode>.Claim();
			int i = 0;
			while (i < parts.Count)
			{
				Funnel.PathPart pathPart = parts[i];
				Funnel.PathPart pathPart2 = pathPart;
				pathPart2.startIndex = list.Count;
				if (pathPart.type != Funnel.PartType.NodeSequence)
				{
					goto IL_0070;
				}
				IRaycastableGraph raycastableGraph = nodes[pathPart.startIndex].Graph as IRaycastableGraph;
				if (raycastableGraph == null)
				{
					goto IL_0070;
				}
				Funnel.Simplify(pathPart, raycastableGraph, nodes, list, ref traversalCosts, ref traversalConstraint);
				pathPart2.endIndex = list.Count - 1;
				parts[i] = pathPart2;
				IL_00B0:
				i++;
				continue;
				IL_0070:
				for (int j = pathPart.startIndex; j <= pathPart.endIndex; j++)
				{
					list.Add(nodes[j]);
				}
				pathPart2.endIndex = list.Count - 1;
				parts[i] = pathPart2;
				goto IL_00B0;
			}
			ListPool<GraphNode>.Release(ref nodes);
			nodes = list;
		}

		// Token: 0x06000AB3 RID: 2739 RVA: 0x00039260 File Offset: 0x00037460
		public static void Simplify(Funnel.PathPart part, IRaycastableGraph graph, List<GraphNode> nodes, List<GraphNode> result, ref TraversalCosts traversalCosts, ref TraversalConstraint traversalConstraint)
		{
			int num = part.startIndex;
			int endIndex = part.endIndex;
			Vector3 startPoint = part.startPoint;
			Vector3 endPoint = part.endPoint;
			if (graph == null)
			{
				throw new ArgumentNullException("graph");
			}
			if (num > endIndex)
			{
				throw new ArgumentException("start > end");
			}
			GraphHitInfo graphHitInfo;
			if (!graph.Linecast(startPoint, endPoint, out graphHitInfo) && graphHitInfo.node == nodes[endIndex])
			{
				graph.Linecast(startPoint, endPoint, out graphHitInfo, result);
				long num2;
				float num3;
				Funnel.<Simplify>g__PathCost|5_1(nodes, num, endIndex, ref traversalCosts, out num2, out num3);
				long num4;
				float num5;
				Funnel.<Simplify>g__PathCost|5_1(result, 0, result.Count - 1, ref traversalCosts, out num4, out num5);
				if (num4 <= num2 && num5 <= 1.1f * num3 && Funnel.<Simplify>g__PathTraversable|5_0(result, 0, result.Count - 1, ref traversalConstraint))
				{
					return;
				}
				result.Clear();
			}
			int num6 = num;
			int num7 = 0;
			while (num7++ <= 1000)
			{
				if (num == endIndex)
				{
					result.Add(nodes[endIndex]);
					Funnel.RemoveBacktracking(result, num6, result.Count - 2);
					return;
				}
				int count = result.Count;
				int i = endIndex + 1;
				int num8 = num + 1;
				bool flag = false;
				while (i > num8 + 1)
				{
					int num9 = (i + num8) / 2;
					Vector3 vector = ((num == num6) ? startPoint : ((Vector3)nodes[num].position));
					Vector3 vector2 = ((num9 == endIndex) ? endPoint : ((Vector3)nodes[num9].position));
					GraphHitInfo graphHitInfo2;
					if (graph.Linecast(vector, vector2, out graphHitInfo2) || graphHitInfo2.node != nodes[num9])
					{
						i = num9;
					}
					else
					{
						flag = true;
						num8 = num9;
					}
				}
				if (!flag)
				{
					result.Add(nodes[num]);
					Funnel.RemoveBacktracking(result, num6, result.Count - 2);
					num = num8;
				}
				else
				{
					Vector3 vector3 = ((num == num6) ? startPoint : ((Vector3)nodes[num].position));
					Vector3 vector4 = ((num8 == endIndex) ? endPoint : ((Vector3)nodes[num8].position));
					GraphHitInfo graphHitInfo3;
					graph.Linecast(vector3, vector4, out graphHitInfo3, result);
					long num10;
					float num11;
					Funnel.<Simplify>g__PathCost|5_1(nodes, num, endIndex, ref traversalCosts, out num10, out num11);
					long num12;
					float num13;
					Funnel.<Simplify>g__PathCost|5_1(result, 0, result.Count - 1, ref traversalCosts, out num12, out num13);
					if (result[result.Count - 1] != nodes[num8] || num12 > num10 || num13 > 1.1f * num11 || !Funnel.<Simplify>g__PathTraversable|5_0(result, 0, result.Count - 1, ref traversalConstraint))
					{
						result.RemoveRange(count, result.Count - count);
						result.Add(nodes[num]);
						num++;
					}
					else
					{
						Funnel.RemoveBacktracking(result, num6, count);
						result.RemoveAt(result.Count - 1);
						num = num8;
					}
				}
			}
			Debug.LogError("Was the path really long or have we got cought in an infinite loop?");
		}

		// Token: 0x06000AB4 RID: 2740 RVA: 0x000394FF File Offset: 0x000376FF
		private static void RemoveBacktracking(List<GraphNode> nodes, int listStartIndex, int aroundIndex)
		{
			while (aroundIndex - 1 > listStartIndex && aroundIndex + 1 < nodes.Count && nodes[aroundIndex - 1] == nodes[aroundIndex + 1])
			{
				nodes.RemoveRange(aroundIndex, 2);
				aroundIndex--;
			}
		}

		// Token: 0x06000AB5 RID: 2741 RVA: 0x00039538 File Offset: 0x00037738
		public static Funnel.FunnelPortals ConstructFunnelPortals(List<GraphNode> nodes, Funnel.PathPart part)
		{
			if (nodes == null || nodes.Count == 0)
			{
				return new Funnel.FunnelPortals
				{
					left = ListPool<Vector3>.Claim(0),
					right = ListPool<Vector3>.Claim(0)
				};
			}
			if (part.endIndex < part.startIndex || part.startIndex < 0 || part.endIndex > nodes.Count)
			{
				throw new ArgumentOutOfRangeException();
			}
			List<Vector3> list = ListPool<Vector3>.Claim(nodes.Count + 1);
			List<Vector3> list2 = ListPool<Vector3>.Claim(nodes.Count + 1);
			list.Add(part.startPoint);
			list2.Add(part.startPoint);
			for (int i = part.startIndex; i < part.endIndex; i++)
			{
				Vector3 vector;
				Vector3 vector2;
				if (nodes[i].GetPortal(nodes[i + 1], out vector, out vector2))
				{
					list.Add(vector);
					list2.Add(vector2);
				}
				else
				{
					list.Add((Vector3)nodes[i].position);
					list2.Add((Vector3)nodes[i].position);
					list.Add((Vector3)nodes[i + 1].position);
					list2.Add((Vector3)nodes[i + 1].position);
				}
			}
			list.Add(part.endPoint);
			list2.Add(part.endPoint);
			return new Funnel.FunnelPortals
			{
				left = list,
				right = list2
			};
		}

		// Token: 0x06000AB6 RID: 2742 RVA: 0x000396B0 File Offset: 0x000378B0
		private static float2 Unwrap(float3 leftPortal, float3 rightPortal, float2 leftUnwrappedPortal, float2 rightUnwrappedPortal, float3 point, float sideMultiplier, float3 projectionAxis)
		{
			if (!math.all(projectionAxis == 0f))
			{
				leftPortal -= projectionAxis * math.dot(leftPortal, projectionAxis);
				rightPortal -= projectionAxis * math.dot(rightPortal, projectionAxis);
				point -= projectionAxis * math.dot(point, projectionAxis);
			}
			float3 @float = rightPortal - leftPortal;
			float num = 1f / math.lengthsq(@float);
			if (float.IsPositiveInfinity(num))
			{
				return leftUnwrappedPortal + new float2(-math.length(point - leftPortal), 0f);
			}
			float num2 = math.length(math.cross(point - leftPortal, @float)) * num;
			float num3 = math.dot(point - leftPortal, @float) * num;
			if (num2 < 0.002f)
			{
				if (math.abs(num3) < 0.002f)
				{
					return leftUnwrappedPortal;
				}
				if (math.abs(num3 - 1f) < 0.002f)
				{
					return rightUnwrappedPortal;
				}
			}
			float2 float2 = rightUnwrappedPortal - leftUnwrappedPortal;
			float2 float3 = new float2(-float2.y, float2.x);
			return leftUnwrappedPortal + math.mad(float2, num3, float3 * (num2 * sideMultiplier));
		}

		// Token: 0x06000AB7 RID: 2743 RVA: 0x000397E3 File Offset: 0x000379E3
		private static bool RightOrColinear(Vector2 a, Vector2 b)
		{
			return a.x * b.y - b.x * a.y <= 0f;
		}

		// Token: 0x06000AB8 RID: 2744 RVA: 0x0003980A File Offset: 0x00037A0A
		private static bool LeftOrColinear(Vector2 a, Vector2 b)
		{
			return a.x * b.y - b.x * a.y >= 0f;
		}

		// Token: 0x06000AB9 RID: 2745 RVA: 0x00039834 File Offset: 0x00037A34
		public unsafe static List<Vector3> Calculate(Funnel.FunnelPortals funnel, bool splitAtEveryPortal)
		{
			Funnel.FunnelState funnelState = new Funnel.FunnelState(funnel, Allocator.Temp);
			float3 @float = *funnelState.leftFunnel.First;
			float3 float2 = *funnelState.leftFunnel.Last;
			funnelState.PopStart();
			funnelState.PopEnd();
			NativeList<float3> nativeList = new NativeList<float3>(Allocator.Temp);
			funnelState.CalculateNextCorners(int.MaxValue, splitAtEveryPortal, @float, float2, nativeList);
			funnelState.Dispose();
			List<Vector3> list = ListPool<Vector3>.Claim(nativeList.Length);
			for (int i = 0; i < nativeList.Length; i++)
			{
				list.Add(nativeList[i]);
			}
			nativeList.Dispose();
			return list;
		}

		// Token: 0x06000ABA RID: 2746 RVA: 0x000398E4 File Offset: 0x00037AE4
		[BurstCompile]
		private static int Calculate(ref NativeCircularBuffer<float4> unwrappedPortals, ref NativeCircularBuffer<float3> leftPortals, ref NativeCircularBuffer<float3> rightPortals, ref float3 startPoint, ref float3 endPoint, ref UnsafeSpan<int> funnelPath, int maxCorners, ref float3 projectionAxis, out bool lastCorner)
		{
			return Funnel.Calculate_00000A41$BurstDirectCall.Invoke(ref unwrappedPortals, ref leftPortals, ref rightPortals, ref startPoint, ref endPoint, ref funnelPath, maxCorners, ref projectionAxis, out lastCorner);
		}

		// Token: 0x06000ABB RID: 2747 RVA: 0x00039904 File Offset: 0x00037B04
		[CompilerGenerated]
		internal static bool <Simplify>g__PathTraversable|5_0(List<GraphNode> nodes, int start, int end, ref TraversalConstraint traversalConstraint)
		{
			for (int i = start; i <= end; i++)
			{
				if ((i == start) ? (!traversalConstraint.CanTraverse(nodes[i])) : (!traversalConstraint.CanTraverse(nodes[i - 1], nodes[i])))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06000ABC RID: 2748 RVA: 0x00039954 File Offset: 0x00037B54
		[CompilerGenerated]
		internal static void <Simplify>g__PathCost|5_1(List<GraphNode> nodes, int start, int end, ref TraversalCosts traversalCosts, out long connectionCost, out float avgCost)
		{
			connectionCost = 0L;
			avgCost = 0f;
			for (int i = start; i < end; i++)
			{
				connectionCost += (long)((ulong)traversalCosts.GetConnectionCost(nodes[i], nodes[i + 1]));
				avgCost += traversalCosts.GetTraversalCostMultiplier(nodes[i]);
			}
			avgCost += traversalCosts.GetTraversalCostMultiplier(nodes[end]);
			avgCost /= (float)(end - start + 1);
		}

		// Token: 0x06000ABD RID: 2749 RVA: 0x000399D0 File Offset: 0x00037BD0
		[BurstCompile]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal unsafe static int Calculate$BurstManaged(ref NativeCircularBuffer<float4> unwrappedPortals, ref NativeCircularBuffer<float3> leftPortals, ref NativeCircularBuffer<float3> rightPortals, ref float3 startPoint, ref float3 endPoint, ref UnsafeSpan<int> funnelPath, int maxCorners, ref float3 projectionAxis, out bool lastCorner)
		{
			lastCorner = false;
			if (leftPortals.Length <= 0)
			{
				lastCorner = true;
				return 0;
			}
			if (maxCorners <= 0)
			{
				return 0;
			}
			int num = 0;
			int num2 = 0;
			int num3 = 0;
			if (unwrappedPortals.Length == 0)
			{
				unwrappedPortals.PushEnd(new float4(new float2(0f, 0f), new float2(math.length(rightPortals[0] - leftPortals[0]))));
			}
			float2 @float = Funnel.Unwrap(leftPortals[0], rightPortals[0], unwrappedPortals[0].xy, unwrappedPortals[0].zw, startPoint, -1f, projectionAxis);
			float2 float2 = float2.zero;
			float2 float3 = float2.zero;
			int i = 0;
			while (i <= leftPortals.Length)
			{
				float2 float5;
				float2 float4;
				if (i == unwrappedPortals.Length)
				{
					if (i == leftPortals.Length)
					{
						float4 = (float5 = Funnel.Unwrap(leftPortals[i - 1], rightPortals[i - 1], unwrappedPortals[i - 1].xy, unwrappedPortals[i - 1].zw, endPoint, 1f, projectionAxis) - @float);
					}
					else
					{
						float2 float6 = Funnel.Unwrap(leftPortals[i - 1], rightPortals[i - 1], unwrappedPortals[i - 1].xy, unwrappedPortals[i - 1].zw, leftPortals[i], 1f, projectionAxis);
						float2 float7 = Funnel.Unwrap(leftPortals[i], rightPortals[i - 1], float6, unwrappedPortals[i - 1].zw, rightPortals[i], 1f, projectionAxis);
						unwrappedPortals.PushEnd(new float4(float6, float7));
						float5 = float6 - @float;
						float4 = float7 - @float;
					}
				}
				else
				{
					float5 = unwrappedPortals[i].xy - @float;
					float4 = unwrappedPortals[i].zw - @float;
				}
				if (!Funnel.LeftOrColinear(float3, float4))
				{
					goto IL_0296;
				}
				if (Funnel.RightOrColinear(float2, float4))
				{
					float3 = float4;
					num = i;
					goto IL_0296;
				}
				float2 = (float3 = float2.zero);
				int num4 = (i = (num = num2));
				@float = unwrappedPortals[i].xy;
				*funnelPath[num3++] = num4;
				if (num3 >= maxCorners)
				{
					return num3;
				}
				IL_0308:
				i++;
				continue;
				IL_0296:
				if (!Funnel.RightOrColinear(float2, float5))
				{
					goto IL_0308;
				}
				if (Funnel.LeftOrColinear(float3, float5))
				{
					float2 = float5;
					num2 = i;
					goto IL_0308;
				}
				float2 = (float3 = float2.zero);
				num4 = (i = (num2 = num));
				@float = unwrappedPortals[i].zw;
				*funnelPath[num3++] = num4 | 1073741824;
				if (num3 >= maxCorners)
				{
					return num3;
				}
				goto IL_0308;
			}
			lastCorner = true;
			return num3;
		}

		// Token: 0x04000707 RID: 1799
		public const int RightSideBit = 1073741824;

		// Token: 0x04000708 RID: 1800
		public const int FunnelPortalIndexMask = 1073741823;

		// Token: 0x0200014C RID: 332
		public struct FunnelPortals
		{
			// Token: 0x04000709 RID: 1801
			public List<Vector3> left;

			// Token: 0x0400070A RID: 1802
			public List<Vector3> right;
		}

		// Token: 0x0200014D RID: 333
		public enum PartType
		{
			// Token: 0x0400070C RID: 1804
			OffMeshLink,
			// Token: 0x0400070D RID: 1805
			NodeSequence
		}

		// Token: 0x0200014E RID: 334
		public struct PathPart
		{
			// Token: 0x0400070E RID: 1806
			public int startIndex;

			// Token: 0x0400070F RID: 1807
			public int endIndex;

			// Token: 0x04000710 RID: 1808
			public Vector3 startPoint;

			// Token: 0x04000711 RID: 1809
			public Vector3 endPoint;

			// Token: 0x04000712 RID: 1810
			public Funnel.PartType type;
		}

		// Token: 0x0200014F RID: 335
		[BurstCompile]
		public struct FunnelState
		{
			// Token: 0x06000ABE RID: 2750 RVA: 0x00039D00 File Offset: 0x00037F00
			public FunnelState(int initialCapacity, Allocator allocator)
			{
				this.leftFunnel = new NativeCircularBuffer<float3>(initialCapacity, allocator);
				this.rightFunnel = new NativeCircularBuffer<float3>(initialCapacity, allocator);
				this.unwrappedPortals = new NativeCircularBuffer<float4>(initialCapacity, allocator);
				this.projectionAxis = float3.zero;
			}

			// Token: 0x06000ABF RID: 2751 RVA: 0x00039D50 File Offset: 0x00037F50
			public FunnelState(Funnel.FunnelPortals portals, Allocator allocator)
			{
				this = new Funnel.FunnelState(portals.left.Count, allocator);
				if (portals.left.Count != portals.right.Count)
				{
					throw new ArgumentException("portals.left.Count != portals.right.Count");
				}
				for (int i = 0; i < portals.left.Count; i++)
				{
					this.PushEnd(portals.left[i], portals.right[i]);
				}
			}

			// Token: 0x06000AC0 RID: 2752 RVA: 0x00039DC8 File Offset: 0x00037FC8
			public Funnel.FunnelState Clone()
			{
				return new Funnel.FunnelState
				{
					leftFunnel = this.leftFunnel.Clone(),
					rightFunnel = this.rightFunnel.Clone(),
					unwrappedPortals = this.unwrappedPortals.Clone(),
					projectionAxis = this.projectionAxis
				};
			}

			// Token: 0x06000AC1 RID: 2753 RVA: 0x00039E21 File Offset: 0x00038021
			public void Clear()
			{
				this.leftFunnel.Clear();
				this.rightFunnel.Clear();
				this.unwrappedPortals.Clear();
				this.projectionAxis = float3.zero;
			}

			// Token: 0x06000AC2 RID: 2754 RVA: 0x00039E4F File Offset: 0x0003804F
			public void PopStart()
			{
				this.leftFunnel.PopStart();
				this.rightFunnel.PopStart();
				if (this.unwrappedPortals.Length > 0)
				{
					this.unwrappedPortals.PopStart();
				}
			}

			// Token: 0x06000AC3 RID: 2755 RVA: 0x00039E83 File Offset: 0x00038083
			public void PopEnd()
			{
				this.leftFunnel.PopEnd();
				this.rightFunnel.PopEnd();
				this.unwrappedPortals.TrimTo(this.leftFunnel.Length);
			}

			// Token: 0x06000AC4 RID: 2756 RVA: 0x00039EB3 File Offset: 0x000380B3
			public void Pop(bool fromStart)
			{
				if (fromStart)
				{
					this.PopStart();
					return;
				}
				this.PopEnd();
			}

			// Token: 0x06000AC5 RID: 2757 RVA: 0x00039EC5 File Offset: 0x000380C5
			public void PushStart(float3 newLeftPortal, float3 newRightPortal)
			{
				Funnel.FunnelState.PushStart(ref this.leftFunnel, ref this.rightFunnel, ref this.unwrappedPortals, ref newLeftPortal, ref newRightPortal, ref this.projectionAxis);
			}

			// Token: 0x06000AC6 RID: 2758 RVA: 0x00039EE8 File Offset: 0x000380E8
			private static bool DifferentSidesOfLine(float3 start, float3 end, float3 a, float3 b)
			{
				float3 @float = math.normalizesafe(end - start, default(float3));
				float3 float2 = a - start;
				float3 float3 = b - start;
				float2 -= @float * math.dot(float2, @float);
				float3 -= @float * math.dot(float3, @float);
				return math.dot(float2, float3) < 0f;
			}

			// Token: 0x06000AC7 RID: 2759 RVA: 0x00039F54 File Offset: 0x00038154
			public unsafe bool IsReasonableToPopStart(float3 startPoint, float3 endPoint)
			{
				if (this.leftFunnel.Length == 0)
				{
					return false;
				}
				int num = 1;
				while (num < this.leftFunnel.Length && VectorMath.IsColinear(*this.leftFunnel.First, *this.rightFunnel.First, this.leftFunnel[num]))
				{
					num++;
				}
				return !Funnel.FunnelState.DifferentSidesOfLine(*this.leftFunnel.First, *this.rightFunnel.First, startPoint, (num < this.leftFunnel.Length) ? this.leftFunnel[num] : endPoint);
			}

			// Token: 0x06000AC8 RID: 2760 RVA: 0x0003A010 File Offset: 0x00038210
			public unsafe bool IsReasonableToPopEnd(float3 startPoint, float3 endPoint)
			{
				if (this.leftFunnel.Length == 0)
				{
					return false;
				}
				int num = this.leftFunnel.Length - 1;
				while (num >= 0 && VectorMath.IsColinear(*this.leftFunnel.Last, *this.rightFunnel.Last, this.leftFunnel[num]))
				{
					num--;
				}
				return !Funnel.FunnelState.DifferentSidesOfLine(*this.leftFunnel.Last, *this.rightFunnel.Last, endPoint, (num >= 0) ? this.leftFunnel[num] : startPoint);
			}

			// Token: 0x06000AC9 RID: 2761 RVA: 0x0003A0C2 File Offset: 0x000382C2
			[BurstCompile]
			private static void PushStart(ref NativeCircularBuffer<float3> leftPortals, ref NativeCircularBuffer<float3> rightPortals, ref NativeCircularBuffer<float4> unwrappedPortals, ref float3 newLeftPortal, ref float3 newRightPortal, ref float3 projectionAxis)
			{
				Funnel.FunnelState.PushStart_00000A4F$BurstDirectCall.Invoke(ref leftPortals, ref rightPortals, ref unwrappedPortals, ref newLeftPortal, ref newRightPortal, ref projectionAxis);
			}

			// Token: 0x06000ACA RID: 2762 RVA: 0x0003A0D1 File Offset: 0x000382D1
			public void Splice(int startIndex, int toRemove, List<float3> newLeftPortal, List<float3> newRightPortal)
			{
				this.leftFunnel.Splice(startIndex, toRemove, newLeftPortal);
				this.rightFunnel.Splice(startIndex, toRemove, newRightPortal);
				this.unwrappedPortals.TrimTo(startIndex);
			}

			// Token: 0x06000ACB RID: 2763 RVA: 0x0003A0FC File Offset: 0x000382FC
			public void PushEnd(Vector3 newLeftPortal, Vector3 newRightPortal)
			{
				this.leftFunnel.PushEnd(newLeftPortal);
				this.rightFunnel.PushEnd(newRightPortal);
			}

			// Token: 0x06000ACC RID: 2764 RVA: 0x0003A120 File Offset: 0x00038320
			public void Push(bool toStart, Vector3 newLeftPortal, Vector3 newRightPortal)
			{
				if (toStart)
				{
					this.PushStart(newLeftPortal, newRightPortal);
					return;
				}
				this.PushEnd(newLeftPortal, newRightPortal);
			}

			// Token: 0x06000ACD RID: 2765 RVA: 0x0003A140 File Offset: 0x00038340
			public void Dispose()
			{
				this.leftFunnel.Dispose();
				this.rightFunnel.Dispose();
				this.unwrappedPortals.Dispose();
			}

			// Token: 0x06000ACE RID: 2766 RVA: 0x0003A164 File Offset: 0x00038364
			public int CalculateNextCornerIndices(int maxCorners, NativeArray<int> result, float3 startPoint, float3 endPoint, out bool lastCorner)
			{
				if (result.Length < math.min(maxCorners, this.leftFunnel.Length))
				{
					throw new ArgumentException("result array may not be large enough to hold all corners");
				}
				UnsafeSpan<int> unsafeSpan = result.AsUnsafeSpan<int>();
				return Funnel.Calculate(ref this.unwrappedPortals, ref this.leftFunnel, ref this.rightFunnel, ref startPoint, ref endPoint, ref unsafeSpan, maxCorners, ref this.projectionAxis, out lastCorner);
			}

			// Token: 0x06000ACF RID: 2767 RVA: 0x0003A1C4 File Offset: 0x000383C4
			public void CalculateNextCorners(int maxCorners, bool splitAtEveryPortal, float3 startPoint, float3 endPoint, NativeList<float3> result)
			{
				NativeArray<int> nativeArray = new NativeArray<int>(math.min(maxCorners, this.leftFunnel.Length), Allocator.Temp, NativeArrayOptions.ClearMemory);
				bool flag;
				int num = this.CalculateNextCornerIndices(maxCorners, nativeArray, startPoint, endPoint, out flag);
				this.ConvertCornerIndicesToPath(nativeArray, num, splitAtEveryPortal, startPoint, endPoint, flag, result);
				nativeArray.Dispose();
			}

			// Token: 0x06000AD0 RID: 2768 RVA: 0x0003A210 File Offset: 0x00038410
			public unsafe void ConvertCornerIndicesToPath(NativeArray<int> indices, int numCorners, bool splitAtEveryPortal, float3 startPoint, float3 endPoint, bool lastCorner, NativeList<float3> result)
			{
				if (result.Capacity < numCorners)
				{
					result.Capacity = numCorners;
				}
				result.Add(in startPoint);
				if (this.leftFunnel.Length == 0)
				{
					if (lastCorner)
					{
						result.Add(in endPoint);
					}
					return;
				}
				if (splitAtEveryPortal)
				{
					float2 @float = Funnel.Unwrap(this.leftFunnel[0], this.rightFunnel[0], this.unwrappedPortals[0].xy, this.unwrappedPortals[0].zw, startPoint, -1f, this.projectionAxis);
					int num = -1;
					for (int i = 0; i < numCorners; i++)
					{
						int num2 = indices[i] & 1073741823;
						bool flag = (indices[i] & 1073741824) != 0;
						float2 float2 = (flag ? this.unwrappedPortals[num2].zw : this.unwrappedPortals[num2].xy);
						Funnel.FunnelState.CalculatePortalIntersections(num + 1, num2 - 1, this.leftFunnel, this.rightFunnel, this.unwrappedPortals, @float, float2, result);
						num = math.abs(num2);
						@float = float2;
						float3 float3 = (flag ? this.rightFunnel[num2] : this.leftFunnel[num2]);
						result.Add(in float3);
					}
					if (lastCorner)
					{
						float2 float4 = Funnel.Unwrap(*this.leftFunnel.Last, *this.rightFunnel.Last, this.unwrappedPortals.Last.xy, this.unwrappedPortals.Last.zw, endPoint, 1f, this.projectionAxis);
						Funnel.FunnelState.CalculatePortalIntersections(num + 1, this.unwrappedPortals.Length, this.leftFunnel, this.rightFunnel, this.unwrappedPortals, @float, float4, result);
						result.Add(in endPoint);
						return;
					}
				}
				else
				{
					for (int j = 0; j < numCorners; j++)
					{
						int num3 = indices[j];
						float3 float3 = (((num3 & 1073741824) != 0) ? this.rightFunnel[num3 & 1073741823] : this.leftFunnel[num3 & 1073741823]);
						result.Add(in float3);
					}
					if (lastCorner)
					{
						result.Add(in endPoint);
					}
				}
			}

			// Token: 0x06000AD1 RID: 2769 RVA: 0x0003A464 File Offset: 0x00038664
			public void ConvertCornerIndicesToPathProjected(UnsafeSpan<int> indices, bool splitAtEveryPortal, float3 startPoint, float3 endPoint, bool lastCorner, NativeList<float3> result, float3 up)
			{
				int num = indices.Length + 1 + (lastCorner ? 1 : 0);
				if (result.Capacity < num)
				{
					result.Capacity = num;
				}
				result.ResizeUninitialized(num);
				UnsafeSpan<float3> unsafeSpan = result.AsUnsafeSpan<float3>();
				Funnel.FunnelState.ConvertCornerIndicesToPathProjected(ref this, ref indices, splitAtEveryPortal, in startPoint, in endPoint, lastCorner, in this.projectionAxis, ref unsafeSpan, in up);
			}

			// Token: 0x06000AD2 RID: 2770 RVA: 0x0003A4C0 File Offset: 0x000386C0
			public float4x3 UnwrappedPortalsToWorldMatrix(float3 up)
			{
				int num = 0;
				while (num < this.unwrappedPortals.Length && math.lengthsq(this.unwrappedPortals[num].xy - this.unwrappedPortals[num].zw) <= 1E-05f)
				{
					num++;
				}
				if (num >= this.unwrappedPortals.Length)
				{
					return new float4x3(1f, 0f, 0f, 0f, 0f, 1f, 0f, 0f, 0f, 0f, 0f, 1f);
				}
				float2 xy = this.unwrappedPortals[num].xy;
				float2 zw = this.unwrappedPortals[num].zw;
				float3 @float = this.leftFunnel[num];
				float3 float2 = this.rightFunnel[num];
				float2 float3 = zw - xy;
				float3 float4 = float2 - @float;
				float2 float5 = float3 * math.rcp(math.lengthsq(float3));
				float2x2 float2x = new float2x2(new float2(float5.x, -float5.y), new float2(float5.y, float5.x));
				float2 float6 = math.mul(float2x, -xy);
				float4x3 float4x = new float4x3(new float4(float2x.c0.x, 0f, float2x.c0.y, 0f), new float4(float2x.c1.x, 0f, float2x.c1.y, 0f), new float4(float6.x, 0f, float6.y, 1f));
				return math.mul(new float4x4(new float4(float4, 0f), new float4(up, 0f), new float4(math.cross(float4, up), 0f), new float4(@float, 1f)), float4x);
			}

			// Token: 0x06000AD3 RID: 2771 RVA: 0x0003A6C8 File Offset: 0x000388C8
			[BurstCompile]
			public static void ConvertCornerIndicesToPathProjected(ref Funnel.FunnelState funnelState, ref UnsafeSpan<int> indices, bool splitAtEveryPortal, in float3 startPoint, in float3 endPoint, bool lastCorner, in float3 projectionAxis, ref UnsafeSpan<float3> result, in float3 up)
			{
				Funnel.FunnelState.ConvertCornerIndicesToPathProjected_00000A59$BurstDirectCall.Invoke(ref funnelState, ref indices, splitAtEveryPortal, in startPoint, in endPoint, lastCorner, in projectionAxis, ref result, in up);
			}

			// Token: 0x06000AD4 RID: 2772 RVA: 0x0003A6E8 File Offset: 0x000388E8
			private static void CalculatePortalIntersections(int startIndex, int endIndex, NativeCircularBuffer<float3> leftPortals, NativeCircularBuffer<float3> rightPortals, NativeCircularBuffer<float4> unwrappedPortals, float2 from, float2 to, NativeList<float3> result)
			{
				for (int i = startIndex; i < endIndex; i++)
				{
					float4 @float = unwrappedPortals[i];
					float2 xy = @float.xy;
					float2 zw = @float.zw;
					float num;
					if (!VectorMath.LineLineIntersectionFactor(xy, zw - xy, from, to - from, out num))
					{
						num = 0.5f;
					}
					float3 float2 = math.lerp(leftPortals[i], rightPortals[i], num);
					result.Add(in float2);
				}
			}

			// Token: 0x06000AD5 RID: 2773 RVA: 0x0003A760 File Offset: 0x00038960
			[BurstCompile]
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			internal unsafe static void PushStart$BurstManaged(ref NativeCircularBuffer<float3> leftPortals, ref NativeCircularBuffer<float3> rightPortals, ref NativeCircularBuffer<float4> unwrappedPortals, ref float3 newLeftPortal, ref float3 newRightPortal, ref float3 projectionAxis)
			{
				if (unwrappedPortals.Length == 0)
				{
					leftPortals.PushStart(newLeftPortal);
					rightPortals.PushStart(newRightPortal);
					return;
				}
				float4 @float = *unwrappedPortals.First;
				float2 float2 = Funnel.Unwrap(*leftPortals.First, *rightPortals.First, @float.xy, @float.zw, newRightPortal, -1f, projectionAxis);
				float2 float3 = Funnel.Unwrap(*leftPortals.First, newRightPortal, @float.xy, float2, newLeftPortal, -1f, projectionAxis);
				leftPortals.PushStart(newLeftPortal);
				rightPortals.PushStart(newRightPortal);
				unwrappedPortals.PushStart(new float4(float3, float2));
			}

			// Token: 0x06000AD6 RID: 2774 RVA: 0x0003A830 File Offset: 0x00038A30
			[BurstCompile]
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			internal unsafe static void ConvertCornerIndicesToPathProjected$BurstManaged(ref Funnel.FunnelState funnelState, ref UnsafeSpan<int> indices, bool splitAtEveryPortal, in float3 startPoint, in float3 endPoint, bool lastCorner, in float3 projectionAxis, ref UnsafeSpan<float3> result, in float3 up)
			{
				int num = 0;
				*result[num++] = startPoint;
				if (funnelState.leftFunnel.Length == 0)
				{
					if (lastCorner)
					{
						*result[num++] = endPoint;
					}
					return;
				}
				float4x3 float4x = funnelState.UnwrappedPortalsToWorldMatrix(up);
				if (splitAtEveryPortal)
				{
					throw new NotImplementedException();
				}
				for (int i = 0; i < indices.Length; i++)
				{
					int num2 = *indices[i];
					float2 @float = (((num2 & 1073741824) != 0) ? funnelState.unwrappedPortals[num2 & 1073741823].zw : funnelState.unwrappedPortals[num2 & 1073741823].xy);
					*result[num++] = math.mul(float4x, new float3(@float, 1f)).xyz;
				}
				if (lastCorner)
				{
					float2 float2 = Funnel.Unwrap(*funnelState.leftFunnel.Last, *funnelState.rightFunnel.Last, funnelState.unwrappedPortals.Last.xy, funnelState.unwrappedPortals.Last.zw, endPoint, 1f, projectionAxis);
					*result[num++] = math.mul(float4x, new float3(float2, 1f)).xyz;
				}
			}

			// Token: 0x04000713 RID: 1811
			public NativeCircularBuffer<float3> leftFunnel;

			// Token: 0x04000714 RID: 1812
			public NativeCircularBuffer<float3> rightFunnel;

			// Token: 0x04000715 RID: 1813
			public NativeCircularBuffer<float4> unwrappedPortals;

			// Token: 0x04000716 RID: 1814
			public float3 projectionAxis;

			// Token: 0x02000150 RID: 336
			// (Invoke) Token: 0x06000AD8 RID: 2776
			internal delegate void PushStart_00000A4F$PostfixBurstDelegate(ref NativeCircularBuffer<float3> leftPortals, ref NativeCircularBuffer<float3> rightPortals, ref NativeCircularBuffer<float4> unwrappedPortals, ref float3 newLeftPortal, ref float3 newRightPortal, ref float3 projectionAxis);

			// Token: 0x02000151 RID: 337
			internal static class PushStart_00000A4F$BurstDirectCall
			{
				// Token: 0x06000ADB RID: 2779 RVA: 0x0003A9B0 File Offset: 0x00038BB0
				[BurstDiscard]
				private static void GetFunctionPointerDiscard(ref IntPtr A_0)
				{
					if (Funnel.FunnelState.PushStart_00000A4F$BurstDirectCall.Pointer == 0)
					{
						Funnel.FunnelState.PushStart_00000A4F$BurstDirectCall.Pointer = BurstCompiler.GetILPPMethodFunctionPointer2(Funnel.FunnelState.PushStart_00000A4F$BurstDirectCall.DeferredCompilation, methodof(Funnel.FunnelState.PushStart$BurstManaged(ref NativeCircularBuffer<float3>, ref NativeCircularBuffer<float3>, ref NativeCircularBuffer<float4>, ref float3, ref float3, ref float3)).MethodHandle, typeof(Funnel.FunnelState.PushStart_00000A4F$PostfixBurstDelegate).TypeHandle);
					}
					A_0 = Funnel.FunnelState.PushStart_00000A4F$BurstDirectCall.Pointer;
				}

				// Token: 0x06000ADC RID: 2780 RVA: 0x0003A9DC File Offset: 0x00038BDC
				private static IntPtr GetFunctionPointer()
				{
					IntPtr intPtr = (IntPtr)0;
					Funnel.FunnelState.PushStart_00000A4F$BurstDirectCall.GetFunctionPointerDiscard(ref intPtr);
					return intPtr;
				}

				// Token: 0x06000ADD RID: 2781 RVA: 0x0003A9F4 File Offset: 0x00038BF4
				public static void Constructor()
				{
					Funnel.FunnelState.PushStart_00000A4F$BurstDirectCall.DeferredCompilation = BurstCompiler.CompileILPPMethod2(methodof(Funnel.FunnelState.PushStart(ref NativeCircularBuffer<float3>, ref NativeCircularBuffer<float3>, ref NativeCircularBuffer<float4>, ref float3, ref float3, ref float3)).MethodHandle);
				}

				// Token: 0x06000ADE RID: 2782 RVA: 0x00003788 File Offset: 0x00001988
				public static void Initialize()
				{
				}

				// Token: 0x06000ADF RID: 2783 RVA: 0x0003AA05 File Offset: 0x00038C05
				// Note: this type is marked as 'beforefieldinit'.
				static PushStart_00000A4F$BurstDirectCall()
				{
					Funnel.FunnelState.PushStart_00000A4F$BurstDirectCall.Constructor();
				}

				// Token: 0x06000AE0 RID: 2784 RVA: 0x0003AA0C File Offset: 0x00038C0C
				public static void Invoke(ref NativeCircularBuffer<float3> leftPortals, ref NativeCircularBuffer<float3> rightPortals, ref NativeCircularBuffer<float4> unwrappedPortals, ref float3 newLeftPortal, ref float3 newRightPortal, ref float3 projectionAxis)
				{
					if (BurstCompiler.IsEnabled)
					{
						IntPtr functionPointer = Funnel.FunnelState.PushStart_00000A4F$BurstDirectCall.GetFunctionPointer();
						if (functionPointer != 0)
						{
							calli(System.Void(Pathfinding.Collections.NativeCircularBuffer`1<Unity.Mathematics.float3>&,Pathfinding.Collections.NativeCircularBuffer`1<Unity.Mathematics.float3>&,Pathfinding.Collections.NativeCircularBuffer`1<Unity.Mathematics.float4>&,Unity.Mathematics.float3&,Unity.Mathematics.float3&,Unity.Mathematics.float3&), ref leftPortals, ref rightPortals, ref unwrappedPortals, ref newLeftPortal, ref newRightPortal, ref projectionAxis, functionPointer);
							return;
						}
					}
					Funnel.FunnelState.PushStart$BurstManaged(ref leftPortals, ref rightPortals, ref unwrappedPortals, ref newLeftPortal, ref newRightPortal, ref projectionAxis);
				}

				// Token: 0x04000717 RID: 1815
				private static IntPtr Pointer;

				// Token: 0x04000718 RID: 1816
				private static IntPtr DeferredCompilation;
			}

			// Token: 0x02000152 RID: 338
			// (Invoke) Token: 0x06000AE2 RID: 2786
			internal delegate void ConvertCornerIndicesToPathProjected_00000A59$PostfixBurstDelegate(ref Funnel.FunnelState funnelState, ref UnsafeSpan<int> indices, bool splitAtEveryPortal, in float3 startPoint, in float3 endPoint, bool lastCorner, in float3 projectionAxis, ref UnsafeSpan<float3> result, in float3 up);

			// Token: 0x02000153 RID: 339
			internal static class ConvertCornerIndicesToPathProjected_00000A59$BurstDirectCall
			{
				// Token: 0x06000AE5 RID: 2789 RVA: 0x0003AA4B File Offset: 0x00038C4B
				[BurstDiscard]
				private static void GetFunctionPointerDiscard(ref IntPtr A_0)
				{
					if (Funnel.FunnelState.ConvertCornerIndicesToPathProjected_00000A59$BurstDirectCall.Pointer == 0)
					{
						Funnel.FunnelState.ConvertCornerIndicesToPathProjected_00000A59$BurstDirectCall.Pointer = BurstCompiler.GetILPPMethodFunctionPointer2(Funnel.FunnelState.ConvertCornerIndicesToPathProjected_00000A59$BurstDirectCall.DeferredCompilation, methodof(Funnel.FunnelState.ConvertCornerIndicesToPathProjected$BurstManaged(ref Funnel.FunnelState, ref UnsafeSpan<int>, bool, ref float3, ref float3, bool, ref float3, ref UnsafeSpan<float3>, ref float3)).MethodHandle, typeof(Funnel.FunnelState.ConvertCornerIndicesToPathProjected_00000A59$PostfixBurstDelegate).TypeHandle);
					}
					A_0 = Funnel.FunnelState.ConvertCornerIndicesToPathProjected_00000A59$BurstDirectCall.Pointer;
				}

				// Token: 0x06000AE6 RID: 2790 RVA: 0x0003AA78 File Offset: 0x00038C78
				private static IntPtr GetFunctionPointer()
				{
					IntPtr intPtr = (IntPtr)0;
					Funnel.FunnelState.ConvertCornerIndicesToPathProjected_00000A59$BurstDirectCall.GetFunctionPointerDiscard(ref intPtr);
					return intPtr;
				}

				// Token: 0x06000AE7 RID: 2791 RVA: 0x0003AA90 File Offset: 0x00038C90
				public static void Constructor()
				{
					Funnel.FunnelState.ConvertCornerIndicesToPathProjected_00000A59$BurstDirectCall.DeferredCompilation = BurstCompiler.CompileILPPMethod2(methodof(Funnel.FunnelState.ConvertCornerIndicesToPathProjected(ref Funnel.FunnelState, ref UnsafeSpan<int>, bool, ref float3, ref float3, bool, ref float3, ref UnsafeSpan<float3>, ref float3)).MethodHandle);
				}

				// Token: 0x06000AE8 RID: 2792 RVA: 0x00003788 File Offset: 0x00001988
				public static void Initialize()
				{
				}

				// Token: 0x06000AE9 RID: 2793 RVA: 0x0003AAA1 File Offset: 0x00038CA1
				// Note: this type is marked as 'beforefieldinit'.
				static ConvertCornerIndicesToPathProjected_00000A59$BurstDirectCall()
				{
					Funnel.FunnelState.ConvertCornerIndicesToPathProjected_00000A59$BurstDirectCall.Constructor();
				}

				// Token: 0x06000AEA RID: 2794 RVA: 0x0003AAA8 File Offset: 0x00038CA8
				public static void Invoke(ref Funnel.FunnelState funnelState, ref UnsafeSpan<int> indices, bool splitAtEveryPortal, in float3 startPoint, in float3 endPoint, bool lastCorner, in float3 projectionAxis, ref UnsafeSpan<float3> result, in float3 up)
				{
					if (BurstCompiler.IsEnabled)
					{
						IntPtr functionPointer = Funnel.FunnelState.ConvertCornerIndicesToPathProjected_00000A59$BurstDirectCall.GetFunctionPointer();
						if (functionPointer != 0)
						{
							calli(System.Void(Pathfinding.Funnel/FunnelState&,Pathfinding.Collections.UnsafeSpan`1<System.Int32>&,System.Boolean,Unity.Mathematics.float3&,Unity.Mathematics.float3&,System.Boolean,Unity.Mathematics.float3&,Pathfinding.Collections.UnsafeSpan`1<Unity.Mathematics.float3>&,Unity.Mathematics.float3&), ref funnelState, ref indices, splitAtEveryPortal, ref startPoint, ref endPoint, lastCorner, ref projectionAxis, ref result, ref up, functionPointer);
							return;
						}
					}
					Funnel.FunnelState.ConvertCornerIndicesToPathProjected$BurstManaged(ref funnelState, ref indices, splitAtEveryPortal, in startPoint, in endPoint, lastCorner, in projectionAxis, ref result, in up);
				}

				// Token: 0x04000719 RID: 1817
				private static IntPtr Pointer;

				// Token: 0x0400071A RID: 1818
				private static IntPtr DeferredCompilation;
			}
		}

		// Token: 0x02000154 RID: 340
		// (Invoke) Token: 0x06000AEC RID: 2796
		internal delegate int Calculate_00000A41$PostfixBurstDelegate(ref NativeCircularBuffer<float4> unwrappedPortals, ref NativeCircularBuffer<float3> leftPortals, ref NativeCircularBuffer<float3> rightPortals, ref float3 startPoint, ref float3 endPoint, ref UnsafeSpan<int> funnelPath, int maxCorners, ref float3 projectionAxis, out bool lastCorner);

		// Token: 0x02000155 RID: 341
		internal static class Calculate_00000A41$BurstDirectCall
		{
			// Token: 0x06000AEF RID: 2799 RVA: 0x0003AAF3 File Offset: 0x00038CF3
			[BurstDiscard]
			private static void GetFunctionPointerDiscard(ref IntPtr A_0)
			{
				if (Funnel.Calculate_00000A41$BurstDirectCall.Pointer == 0)
				{
					Funnel.Calculate_00000A41$BurstDirectCall.Pointer = BurstCompiler.GetILPPMethodFunctionPointer2(Funnel.Calculate_00000A41$BurstDirectCall.DeferredCompilation, methodof(Funnel.Calculate$BurstManaged(ref NativeCircularBuffer<float4>, ref NativeCircularBuffer<float3>, ref NativeCircularBuffer<float3>, ref float3, ref float3, ref UnsafeSpan<int>, int, ref float3, ref bool)).MethodHandle, typeof(Funnel.Calculate_00000A41$PostfixBurstDelegate).TypeHandle);
				}
				A_0 = Funnel.Calculate_00000A41$BurstDirectCall.Pointer;
			}

			// Token: 0x06000AF0 RID: 2800 RVA: 0x0003AB20 File Offset: 0x00038D20
			private static IntPtr GetFunctionPointer()
			{
				IntPtr intPtr = (IntPtr)0;
				Funnel.Calculate_00000A41$BurstDirectCall.GetFunctionPointerDiscard(ref intPtr);
				return intPtr;
			}

			// Token: 0x06000AF1 RID: 2801 RVA: 0x0003AB38 File Offset: 0x00038D38
			public static void Constructor()
			{
				Funnel.Calculate_00000A41$BurstDirectCall.DeferredCompilation = BurstCompiler.CompileILPPMethod2(methodof(Funnel.Calculate(ref NativeCircularBuffer<float4>, ref NativeCircularBuffer<float3>, ref NativeCircularBuffer<float3>, ref float3, ref float3, ref UnsafeSpan<int>, int, ref float3, ref bool)).MethodHandle);
			}

			// Token: 0x06000AF2 RID: 2802 RVA: 0x00003788 File Offset: 0x00001988
			public static void Initialize()
			{
			}

			// Token: 0x06000AF3 RID: 2803 RVA: 0x0003AB49 File Offset: 0x00038D49
			// Note: this type is marked as 'beforefieldinit'.
			static Calculate_00000A41$BurstDirectCall()
			{
				Funnel.Calculate_00000A41$BurstDirectCall.Constructor();
			}

			// Token: 0x06000AF4 RID: 2804 RVA: 0x0003AB50 File Offset: 0x00038D50
			public static int Invoke(ref NativeCircularBuffer<float4> unwrappedPortals, ref NativeCircularBuffer<float3> leftPortals, ref NativeCircularBuffer<float3> rightPortals, ref float3 startPoint, ref float3 endPoint, ref UnsafeSpan<int> funnelPath, int maxCorners, ref float3 projectionAxis, out bool lastCorner)
			{
				if (BurstCompiler.IsEnabled)
				{
					IntPtr functionPointer = Funnel.Calculate_00000A41$BurstDirectCall.GetFunctionPointer();
					if (functionPointer != 0)
					{
						return calli(System.Int32(Pathfinding.Collections.NativeCircularBuffer`1<Unity.Mathematics.float4>&,Pathfinding.Collections.NativeCircularBuffer`1<Unity.Mathematics.float3>&,Pathfinding.Collections.NativeCircularBuffer`1<Unity.Mathematics.float3>&,Unity.Mathematics.float3&,Unity.Mathematics.float3&,Pathfinding.Collections.UnsafeSpan`1<System.Int32>&,System.Int32,Unity.Mathematics.float3&,System.Boolean&), ref unwrappedPortals, ref leftPortals, ref rightPortals, ref startPoint, ref endPoint, ref funnelPath, maxCorners, ref projectionAxis, ref lastCorner, functionPointer);
					}
				}
				return Funnel.Calculate$BurstManaged(ref unwrappedPortals, ref leftPortals, ref rightPortals, ref startPoint, ref endPoint, ref funnelPath, maxCorners, ref projectionAxis, out lastCorner);
			}

			// Token: 0x0400071B RID: 1819
			private static IntPtr Pointer;

			// Token: 0x0400071C RID: 1820
			private static IntPtr DeferredCompilation;
		}
	}
}
