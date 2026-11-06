using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Pathfinding.Collections;
using Pathfinding.Drawing;
using Pathfinding.Pooling;
using Pathfinding.Util;
using Unity.Burst;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Jobs.LowLevel.Unsafe;
using Unity.Mathematics;
using Unity.Profiling;
using UnityEngine;

namespace Pathfinding
{
	// Token: 0x0200015A RID: 346
	[BurstCompile]
	public struct PathTracer
	{
		// Token: 0x170001DD RID: 477
		// (get) Token: 0x06000B05 RID: 2821 RVA: 0x0003BB4E File Offset: 0x00039D4E
		// (set) Token: 0x06000B06 RID: 2822 RVA: 0x0003BB58 File Offset: 0x00039D58
		private bool pathEmptyAndUnrepairable
		{
			readonly get
			{
				return this.pathEmptyAndUnrepairableBacking;
			}
			set
			{
				this.pathEmptyAndUnrepairableBacking = value;
				if (value)
				{
					this.RemoveAllButFirstNode();
				}
				ushort version = this.version;
				this.version = version + 1;
			}
		}

		// Token: 0x170001DE RID: 478
		// (get) Token: 0x06000B07 RID: 2823 RVA: 0x0003BB86 File Offset: 0x00039D86
		// (set) Token: 0x06000B08 RID: 2824 RVA: 0x0003BB8E File Offset: 0x00039D8E
		public ushort version
		{
			[IgnoredByDeepProfiler]
			readonly get;
			[IgnoredByDeepProfiler]
			private set;
		}

		// Token: 0x170001DF RID: 479
		// (get) Token: 0x06000B09 RID: 2825 RVA: 0x0003BB97 File Offset: 0x00039D97
		public readonly bool isCreated
		{
			get
			{
				return this.funnelState.unwrappedPortals.IsCreated;
			}
		}

		// Token: 0x170001E0 RID: 480
		// (get) Token: 0x06000B0A RID: 2826 RVA: 0x0003BBA9 File Offset: 0x00039DA9
		// (set) Token: 0x06000B0B RID: 2827 RVA: 0x0003BBC8 File Offset: 0x00039DC8
		public GraphNode startNode
		{
			[IgnoredByDeepProfiler]
			readonly get
			{
				if (this.startNodeInternal == null || this.startNodeInternal.Destroyed)
				{
					return null;
				}
				return this.startNodeInternal;
			}
			[IgnoredByDeepProfiler]
			private set
			{
				this.startNodeInternal = value;
			}
		}

		// Token: 0x170001E1 RID: 481
		// (get) Token: 0x06000B0C RID: 2828 RVA: 0x0003BBD1 File Offset: 0x00039DD1
		public readonly bool isStale
		{
			[IgnoredByDeepProfiler]
			get
			{
				return !this.endIsUpToDate || !this.startIsUpToDate || this.firstPartContainsDestroyedNodes || this.pathEmptyAndUnrepairableBacking;
			}
		}

		// Token: 0x170001E2 RID: 482
		// (get) Token: 0x06000B0D RID: 2829 RVA: 0x0003BBF3 File Offset: 0x00039DF3
		public readonly int partCount
		{
			get
			{
				if (this.parts == null)
				{
					return 0;
				}
				return this.parts.Length - this.firstPartIndex;
			}
		}

		// Token: 0x170001E3 RID: 483
		// (get) Token: 0x06000B0E RID: 2830 RVA: 0x0003BC0E File Offset: 0x00039E0E
		public readonly bool hasPath
		{
			get
			{
				return this.partCount > 0;
			}
		}

		// Token: 0x170001E4 RID: 484
		// (get) Token: 0x06000B0F RID: 2831 RVA: 0x0003BC19 File Offset: 0x00039E19
		public readonly bool hasValidEndPoint
		{
			get
			{
				return float.IsFinite(this.unclampedEndPoint.x) && !this.pathEmptyAndUnrepairable;
			}
		}

		// Token: 0x170001E5 RID: 485
		// (get) Token: 0x06000B10 RID: 2832 RVA: 0x0003BC38 File Offset: 0x00039E38
		public readonly Vector3 startPoint
		{
			get
			{
				return this.parts[this.firstPartIndex].startPoint;
			}
		}

		// Token: 0x170001E6 RID: 486
		// (get) Token: 0x06000B11 RID: 2833 RVA: 0x0003BC50 File Offset: 0x00039E50
		public readonly Vector3 endPoint
		{
			get
			{
				return this.parts[this.parts.Length - 1].endPoint;
			}
		}

		// Token: 0x170001E7 RID: 487
		// (get) Token: 0x06000B12 RID: 2834 RVA: 0x0003BC6C File Offset: 0x00039E6C
		public readonly Vector3 endPointOfFirstPart
		{
			get
			{
				return this.parts[this.firstPartIndex].endPoint;
			}
		}

		// Token: 0x170001E8 RID: 488
		// (get) Token: 0x06000B13 RID: 2835 RVA: 0x0003BC84 File Offset: 0x00039E84
		public int desiredCornersForGoodSimplification
		{
			get
			{
				if (this.partGraphType != PathTracer.PartGraphType.Grid)
				{
					return 2;
				}
				return 3;
			}
		}

		// Token: 0x170001E9 RID: 489
		// (get) Token: 0x06000B14 RID: 2836 RVA: 0x0003BC92 File Offset: 0x00039E92
		public readonly bool isNextPartValidLink
		{
			get
			{
				return this.partCount > 1 && this.GetPartType(1) == Funnel.PartType.OffMeshLink && !this.PartContainsDestroyedNodes(1);
			}
		}

		// Token: 0x06000B15 RID: 2837 RVA: 0x0003BCB4 File Offset: 0x00039EB4
		public PathTracer(Allocator allocator)
		{
			this.funnelState = new Funnel.FunnelState(16, allocator);
			this.parts = null;
			this.nodes = new CircularBuffer<GraphNode>(16);
			this.portalIsNotInnerCorner = new CircularBuffer<byte>(16);
			this.nodeHashes = new CircularBuffer<int>(16);
			this.unclampedEndPoint = (this.unclampedStartPoint = Vector3.zero);
			this.firstPartIndex = 0;
			this.startIsUpToDate = false;
			this.endIsUpToDate = false;
			this.firstPartContainsDestroyedNodes = false;
			this.startNodeInternal = null;
			this.version = 1;
			this.traversalConstraint = TraversalConstraint.None;
			this.traversalCosts = default(TraversalCosts);
			this.partGraphType = PathTracer.PartGraphType.Navmesh;
			this.pathEmptyAndUnrepairableBacking = false;
			this.Clear();
		}

		// Token: 0x06000B16 RID: 2838 RVA: 0x0003BD66 File Offset: 0x00039F66
		public void Dispose()
		{
			this.Clear();
			this.funnelState.Dispose();
		}

		// Token: 0x06000B17 RID: 2839 RVA: 0x0003BD79 File Offset: 0x00039F79
		public void SetEmptyAndUnrepairable()
		{
			this.pathEmptyAndUnrepairable = true;
		}

		// Token: 0x06000B18 RID: 2840 RVA: 0x0003BD82 File Offset: 0x00039F82
		public Vector3 UpdateStart(Vector3 position, PathTracer.RepairQuality quality, NativeMovementPlane movementPlane)
		{
			this.Repair(position, true, quality, movementPlane, true);
			return this.parts[this.firstPartIndex].startPoint;
		}

		// Token: 0x06000B19 RID: 2841 RVA: 0x0003BDA5 File Offset: 0x00039FA5
		public Vector3 UpdateEnd(Vector3 position, PathTracer.RepairQuality quality, NativeMovementPlane movementPlane)
		{
			this.Repair(position, false, quality, movementPlane, true);
			return this.parts[this.parts.Length - 1].endPoint;
		}

		// Token: 0x06000B1A RID: 2842 RVA: 0x0003BDCC File Offset: 0x00039FCC
		private void AppendNode(bool toStart, GraphNode node)
		{
			int num = (toStart ? this.firstPartIndex : (this.parts.Length - 1));
			ref Funnel.PathPart ptr = ref this.parts[num];
			GraphNode graphNode = ((ptr.endIndex >= ptr.startIndex) ? this.nodes.GetBoundaryValue(toStart) : null);
			if (node == graphNode)
			{
				return;
			}
			if (node == null)
			{
				throw new ArgumentNullException();
			}
			if (ptr.endIndex >= ptr.startIndex + 1 && this.nodes.GetAbsolute(toStart ? (ptr.startIndex + 1) : (ptr.endIndex - 1)) == node)
			{
				if (toStart)
				{
					ptr.startIndex++;
				}
				else
				{
					ptr.endIndex--;
				}
				this.nodes.Pop(toStart);
				this.nodeHashes.Pop(toStart);
				if (num == this.firstPartIndex && this.funnelState.leftFunnel.Length > 0)
				{
					this.funnelState.Pop(toStart);
					this.portalIsNotInnerCorner.Pop(toStart);
				}
				return;
			}
			if (num == this.firstPartIndex && graphNode != null)
			{
				Vector3 vector;
				Vector3 vector2;
				if (toStart)
				{
					if (!node.GetPortal(graphNode, out vector, out vector2))
					{
						throw new NotImplementedException();
					}
				}
				else if (!graphNode.GetPortal(node, out vector, out vector2))
				{
					throw new NotImplementedException();
				}
				this.funnelState.Push(toStart, vector, vector2);
				this.portalIsNotInnerCorner.Push(toStart, 0);
			}
			this.nodes.Push(toStart, node);
			this.nodeHashes.Push(toStart, PathTracer.HashNode(node));
			if (toStart)
			{
				ptr.startIndex--;
				return;
			}
			ptr.endIndex++;
		}

		// Token: 0x06000B1B RID: 2843 RVA: 0x0003BF54 File Offset: 0x0003A154
		private unsafe void AppendPath(bool toStart, CircularBuffer<GraphNode> path)
		{
			if (path.Length == 0)
			{
				return;
			}
			while (path.Length > 0)
			{
				this.AppendNode(toStart, path.PopStart());
			}
			if (toStart)
			{
				this.startNode = *this.nodes.First;
				int num = Mathf.Min(this.parts[this.firstPartIndex].startIndex + 5, this.parts[this.firstPartIndex].endIndex);
				bool flag = false;
				for (int i = this.parts[this.firstPartIndex].startIndex; i <= num; i++)
				{
					flag |= !this.ValidInPath(i);
				}
				this.firstPartContainsDestroyedNodes = flag;
			}
		}

		// Token: 0x06000B1C RID: 2844 RVA: 0x00003788 File Offset: 0x00001988
		[Conditional("UNITY_EDITOR")]
		private void CheckInvariants()
		{
		}

		// Token: 0x06000B1D RID: 2845 RVA: 0x0003C004 File Offset: 0x0003A204
		private bool SplicePath(int startIndex, int toRemove, List<GraphNode> toInsert)
		{
			ref Funnel.PathPart ptr = ref this.parts[this.firstPartIndex];
			if (startIndex < ptr.startIndex || startIndex + toRemove - 1 > ptr.endIndex)
			{
				throw new ArgumentException("This method can only handle splicing the first part of the path");
			}
			if (toInsert != null)
			{
				int num = 0;
				int num2 = 0;
				while (num < toInsert.Count && num < toRemove && toInsert[num] == this.nodes.GetAbsolute(startIndex + num))
				{
					num++;
				}
				if (num == toInsert.Count && num == toRemove)
				{
					return true;
				}
				while (num2 < toInsert.Count - num && num2 < toRemove - num && toInsert[toInsert.Count - num2 - 1] == this.nodes.GetAbsolute(startIndex + toRemove - num2 - 1))
				{
					num2++;
				}
				toInsert.RemoveRange(toInsert.Count - num2, num2);
				toInsert.RemoveRange(0, num);
				startIndex += num;
				toRemove -= num + num2;
			}
			int num3 = ((toInsert != null) ? toInsert.Count : 0);
			if (startIndex - 1 >= ptr.startIndex && !this.ValidInPath(startIndex - 1))
			{
				return false;
			}
			if (startIndex + toRemove <= ptr.endIndex && !this.ValidInPath(startIndex + toRemove))
			{
				return false;
			}
			this.nodes.SpliceAbsolute(startIndex, toRemove, toInsert);
			this.nodeHashes.SpliceUninitializedAbsolute(startIndex, toRemove, num3);
			if (toInsert != null)
			{
				for (int i = 0; i < toInsert.Count; i++)
				{
					this.nodeHashes.SetAbsolute(startIndex + i, PathTracer.HashNode(toInsert[i]));
				}
			}
			int num4 = num3 - toRemove;
			int num5 = math.max(startIndex - 1, ptr.startIndex);
			int num6 = math.min(startIndex + toRemove, ptr.endIndex) - num5;
			ptr.endIndex += num4;
			for (int j = this.firstPartIndex + 1; j < this.parts.Length; j++)
			{
				Funnel.PathPart[] array = this.parts;
				int num7 = j;
				array[num7].startIndex = array[num7].startIndex + num4;
				Funnel.PathPart[] array2 = this.parts;
				int num8 = j;
				array2[num8].endIndex = array2[num8].endIndex + num4;
			}
			int num9 = startIndex + num3 - 1;
			int num10 = math.max(startIndex - 1, ptr.startIndex);
			int num11 = math.min(num9 + 1, ptr.endIndex);
			List<float3> list;
			List<float3> list2;
			PathTracer.GetTemporaryPortalLists(out list, out list2);
			this.CalculateFunnelPortals(num10, num11, list, list2);
			this.funnelState.Splice(num10 - ptr.startIndex, num6, list, list2);
			this.portalIsNotInnerCorner.SpliceUninitialized(num10 - ptr.startIndex, num6, list.Count);
			for (int k = 0; k < list.Count; k++)
			{
				this.portalIsNotInnerCorner[num10 - ptr.startIndex + k] = 0;
			}
			return true;
		}

		// Token: 0x06000B1E RID: 2846 RVA: 0x0003C2A8 File Offset: 0x0003A4A8
		private static bool ContainsPoint(GraphNode node, Vector3 point, NativeMovementPlane plane)
		{
			TriangleMeshNode triangleMeshNode = node as TriangleMeshNode;
			if (triangleMeshNode != null)
			{
				return triangleMeshNode.ContainsPoint(point, plane);
			}
			return node.ContainsPoint(point);
		}

		// Token: 0x06000B1F RID: 2847 RVA: 0x0003C2CF File Offset: 0x0003A4CF
		[BurstCompile]
		private static bool ContainsAndProject(ref Int3 a, ref Int3 b, ref Int3 c, ref Vector3 p, float height, ref NativeMovementPlane movementPlane, out Vector3 projected)
		{
			return PathTracer.ContainsAndProject_00000A85$BurstDirectCall.Invoke(ref a, ref b, ref c, ref p, height, ref movementPlane, out projected);
		}

		// Token: 0x06000B20 RID: 2848 RVA: 0x0003C2E0 File Offset: 0x0003A4E0
		private static float3 ProjectOnSurface(float3 a, float3 b, float3 c, float3 p, float3 up)
		{
			float3 @float = math.cross(c - a, b - a);
			float num = math.dot(@float, up);
			if (math.abs(num) > 1.1754944E-38f)
			{
				float3 float2 = p - a;
				float num2 = -math.dot(@float, float2) / num;
				return p + num2 * up;
			}
			return p;
		}

		// Token: 0x06000B21 RID: 2849 RVA: 0x0003C33C File Offset: 0x0003A53C
		private void Repair(Vector3 point, bool isStart, PathTracer.RepairQuality quality, NativeMovementPlane movementPlane, bool allowCache = true)
		{
			int num;
			int num2;
			GraphNode graphNode;
			bool flag;
			if (isStart)
			{
				num = this.firstPartIndex;
				num2 = this.parts[num].startIndex;
				graphNode = this.nodes.GetAbsolute(num2);
				flag = this.unclampedStartPoint == point;
			}
			else
			{
				num = this.parts.Length - 1;
				num2 = this.parts[num].endIndex;
				graphNode = this.nodes.GetAbsolute(num2);
				flag = this.unclampedEndPoint == point;
				if (this.pathEmptyAndUnrepairable)
				{
					return;
				}
			}
			bool flag2 = this.ValidInPath(num2);
			if (allowCache && flag && flag2)
			{
				return;
			}
			if (this.partGraphType == PathTracer.PartGraphType.OffMeshLink)
			{
				throw new InvalidOperationException("Cannot repair path while on an off-mesh link");
			}
			ref Funnel.PathPart ptr = ref this.parts[num];
			ushort num3;
			if (float.IsFinite(point.x))
			{
				if (flag2)
				{
					bool flag3 = false;
					Vector3 vector = Vector3.zero;
					TriangleMeshNode triangleMeshNode = graphNode as TriangleMeshNode;
					if (triangleMeshNode != null)
					{
						Int3 @int;
						Int3 int2;
						Int3 int3;
						triangleMeshNode.GetVertices(out @int, out int2, out int3);
						flag3 = PathTracer.ContainsAndProject(ref @int, ref int2, ref int3, ref point, 1f, ref movementPlane, out vector);
					}
					else
					{
						GridNode gridNode = graphNode as GridNode;
						if (gridNode != null && gridNode.ContainsPoint(point))
						{
							flag3 = true;
							vector = gridNode.ClosestPointOnNode(point);
						}
					}
					if (flag3)
					{
						if (isStart)
						{
							ptr.startPoint = vector;
							this.unclampedStartPoint = point;
							this.startIsUpToDate = true;
							this.startNode = graphNode;
							if (this.pathEmptyAndUnrepairable)
							{
								ptr.endPoint = ptr.startPoint;
							}
						}
						else
						{
							ptr.endPoint = vector;
							this.unclampedEndPoint = point;
							this.endIsUpToDate = true;
						}
						num3 = this.version;
						this.version = num3 + 1;
						return;
					}
				}
				this.RepairFull(point, isStart, quality, movementPlane);
				num3 = this.version;
				this.version = num3 + 1;
				return;
			}
			if (isStart)
			{
				throw new ArgumentException("Position must be a finite vector");
			}
			this.unclampedEndPoint = point;
			this.endIsUpToDate = false;
			this.RemoveAllPartsExceptFirst();
			ref Funnel.PathPart ptr2 = ref this.parts[this.firstPartIndex];
			if (ptr2.endIndex > ptr2.startIndex)
			{
				this.SplicePath(ptr2.startIndex + 1, ptr2.endIndex - ptr2.startIndex, null);
			}
			ptr2.endPoint = ptr2.startPoint;
			num3 = this.version;
			this.version = num3 + 1;
		}

		// Token: 0x06000B22 RID: 2850 RVA: 0x0003C57C File Offset: 0x0003A77C
		private unsafe void HeuristicallyPopPortals(bool isStartOfPart, Vector3 point)
		{
			ref Funnel.PathPart ptr = ref this.parts[this.firstPartIndex];
			if (isStartOfPart)
			{
				while (this.funnelState.IsReasonableToPopStart(point, ptr.endPoint))
				{
					ptr.startIndex++;
					this.nodes.PopStart();
					this.nodeHashes.PopStart();
					this.funnelState.PopStart();
					this.portalIsNotInnerCorner.PopStart();
				}
				if (this.ValidInPath(this.nodes.AbsoluteStartIndex))
				{
					this.startNode = *this.nodes.First;
				}
			}
			else
			{
				int num = 0;
				while (this.funnelState.IsReasonableToPopEnd(ptr.startPoint, point))
				{
					ptr.endIndex--;
					num++;
					this.funnelState.PopEnd();
					this.portalIsNotInnerCorner.PopEnd();
				}
				if (num > 0)
				{
					this.nodes.SpliceAbsolute(ptr.endIndex + 1, num, null);
					this.nodeHashes.SpliceAbsolute(ptr.endIndex + 1, num, null);
					for (int i = this.firstPartIndex + 1; i < this.parts.Length; i++)
					{
						Funnel.PathPart[] array = this.parts;
						int num2 = i;
						array[num2].startIndex = array[num2].startIndex - num;
						Funnel.PathPart[] array2 = this.parts;
						int num3 = i;
						array2[num3].endIndex = array2[num3].endIndex - num;
					}
				}
			}
			int num4 = Mathf.Min(ptr.startIndex + 5, ptr.endIndex);
			bool flag = false;
			for (int j = ptr.startIndex; j <= num4; j++)
			{
				flag |= !this.ValidInPath(j);
			}
			this.firstPartContainsDestroyedNodes = flag;
		}

		// Token: 0x06000B23 RID: 2851 RVA: 0x00003788 File Offset: 0x00001988
		[Conditional("UNITY_ASSERTIONS")]
		private void AssertValidInPath(int absoluteNodeIndex)
		{
		}

		// Token: 0x06000B24 RID: 2852 RVA: 0x0003C72A File Offset: 0x0003A92A
		[IgnoredByDeepProfiler]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private readonly bool ValidInPath(int absoluteNodeIndex)
		{
			return PathTracer.HashNode(this.nodes.GetAbsolute(absoluteNodeIndex)) == this.nodeHashes.GetAbsolute(absoluteNodeIndex);
		}

		// Token: 0x06000B25 RID: 2853 RVA: 0x0003C74B File Offset: 0x0003A94B
		[IgnoredByDeepProfiler]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static bool Valid(GraphNode node)
		{
			return !node.Destroyed && node.Walkable;
		}

		// Token: 0x06000B26 RID: 2854 RVA: 0x0003C760 File Offset: 0x0003A960
		[IgnoredByDeepProfiler]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static int HashNode(GraphNode node)
		{
			int num = (int)node.NodeIndex;
			num ^= (node.Walkable ? 100663319 : 0);
			GridNodeBase gridNodeBase = node as GridNodeBase;
			if (gridNodeBase != null)
			{
				num ^= gridNodeBase.position.GetHashCode();
			}
			return num;
		}

		// Token: 0x06000B27 RID: 2855 RVA: 0x0003C7A8 File Offset: 0x0003A9A8
		private unsafe void RepairFull(Vector3 point, bool isStart, PathTracer.RepairQuality quality, NativeMovementPlane movementPlane)
		{
			int num = ((quality == PathTracer.RepairQuality.High) ? 16 : 9);
			int num2 = (isStart ? this.firstPartIndex : (this.parts.Length - 1));
			ref Funnel.PathPart ptr = ref this.parts[num2];
			int num3 = (isStart ? ptr.startIndex : ptr.endIndex);
			if ((!this.ValidInPath(num3) || (ptr.endIndex != ptr.startIndex && !this.ValidInPath(isStart ? (ptr.startIndex + 1) : (ptr.endIndex - 1)))) && num2 == this.firstPartIndex)
			{
				this.HeuristicallyPopPortals(isStart, point);
				num3 = (isStart ? ptr.startIndex : ptr.endIndex);
			}
			if (!this.ValidInPath(num3))
			{
				if (isStart)
				{
					this.firstPartContainsDestroyedNodes = true;
					this.unclampedStartPoint = point;
					this.startIsUpToDate = false;
					NearestNodeConstraint nearestNodeConstraint = this.traversalConstraint.ToNearestNodeConstraint();
					nearestNodeConstraint.distanceMetric = DistanceMetric.ClosestAsSeenFromAboveSoft(movementPlane.up);
					NNInfo nninfo = ((AstarPath.active != null) ? AstarPath.active.GetNearest(point, nearestNodeConstraint) : NNInfo.Empty);
					this.startNode = nninfo.node;
					if (this.startNode != null)
					{
						Vector3 position = nninfo.position;
						ptr.startPoint = position;
						if (ptr.endIndex - ptr.startIndex < 5 && this.partCount <= 1)
						{
							this.Clear();
							this.startNode = nninfo.node;
							this.partGraphType = PathTracer.PartGraphTypeFromNode(this.startNode);
							this.unclampedStartPoint = point;
							this.unclampedEndPoint = position;
							this.nodes.PushEnd(nninfo.node);
							this.nodeHashes.PushEnd(PathTracer.HashNode(nninfo.node));
							this.parts = new Funnel.PathPart[1];
							this.parts[0] = new Funnel.PathPart
							{
								startIndex = this.nodes.AbsoluteStartIndex,
								endIndex = this.nodes.AbsoluteEndIndex,
								startPoint = position,
								endPoint = position
							};
						}
					}
					else
					{
						ptr.startPoint = point;
					}
				}
				else
				{
					this.unclampedEndPoint = point;
					ptr.endPoint = point;
					this.endIsUpToDate = false;
				}
			}
			else
			{
				CircularBuffer<GraphNode> circularBuffer = this.LocalSearch(this.nodes.GetAbsolute(num3), point, num, movementPlane, isStart);
				GraphNode graphNode = *circularBuffer.Last;
				NearestNodeConstraint nearestNodeConstraint2 = this.traversalConstraint.ToNearestNodeConstraint();
				nearestNodeConstraint2.area = (int)graphNode.Area;
				nearestNodeConstraint2.distanceMetric = DistanceMetric.ClosestAsSeenFromAboveSoft(movementPlane.up);
				NNInfo nearest = AstarPath.active.GetNearest(point, nearestNodeConstraint2);
				Vector3 vector = (isStart ? ptr.startPoint : ptr.endPoint);
				bool flag;
				Vector3 vector2;
				if (nearest.node == graphNode)
				{
					flag = true;
					vector2 = nearest.position;
				}
				else
				{
					float sqrMagnitude = ((isStart ? this.unclampedStartPoint : this.unclampedEndPoint) - vector).sqrMagnitude;
					bool flag2 = (isStart ? this.startIsUpToDate : this.endIsUpToDate);
					vector2 = graphNode.ClosestPointOnNode(point);
					float sqrMagnitude2 = (point - vector2).sqrMagnitude;
					flag = flag2 && sqrMagnitude2 <= sqrMagnitude + 0.010000001f;
				}
				if (!flag && !isStart)
				{
					circularBuffer.Clear();
					vector2 = vector;
				}
				this.AppendPath(isStart, circularBuffer);
				circularBuffer.Pool();
				if (isStart)
				{
					this.startNode = *this.nodes.First;
				}
				if (isStart)
				{
					this.unclampedStartPoint = point;
					ptr.startPoint = vector2;
					this.startIsUpToDate = true;
				}
				else
				{
					this.unclampedEndPoint = point;
					ptr.endPoint = vector2;
					this.endIsUpToDate = flag;
				}
			}
			if (this.pathEmptyAndUnrepairable)
			{
				this.RemoveAllButFirstNode();
			}
		}

		// Token: 0x06000B28 RID: 2856 RVA: 0x0003CB5C File Offset: 0x0003AD5C
		private static float SquaredDistanceToNode(GraphNode node, Vector3 point, ref BBTree.ProjectionParams projectionParams)
		{
			TriangleMeshNode triangleMeshNode = node as TriangleMeshNode;
			if (triangleMeshNode != null)
			{
				Int3 @int;
				Int3 int2;
				Int3 int3;
				triangleMeshNode.GetVerticesInGraphSpace(out @int, out int2, out int3);
				float3 @float;
				float num;
				float num2;
				Polygon.ClosestPointOnTriangleProjected(ref @int, ref int2, ref int3, ref projectionParams, UnsafeUtility.As<Vector3, float3>(ref point), out @float, out num, out num2);
				return num;
			}
			GridNodeBase gridNodeBase = node as GridNodeBase;
			if (gridNodeBase != null)
			{
				Vector2Int coordinatesInGrid = gridNodeBase.CoordinatesInGrid;
				float num3 = math.clamp(point.x, (float)coordinatesInGrid.x, (float)(coordinatesInGrid.x + 1));
				float num4 = math.clamp(point.z, (float)coordinatesInGrid.y, (float)(coordinatesInGrid.y + 1));
				return math.lengthsq(new float3(num3, 0f, num4) - point);
			}
			Vector3 vector = node.ClosestPointOnNode(point);
			return (point - vector).sqrMagnitude;
		}

		// Token: 0x06000B29 RID: 2857 RVA: 0x0003CC24 File Offset: 0x0003AE24
		private static bool QueueHasNode(PathTracer.QueueItem[] queue, int count, GraphNode node)
		{
			for (int i = 0; i < count; i++)
			{
				if (queue[i].node == node)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06000B2A RID: 2858 RVA: 0x0003CC50 File Offset: 0x0003AE50
		private void GetTempQueue(out PathTracer.QueueItem[] queue, out List<GraphNode> connections)
		{
			int threadIndex = JobsUtility.ThreadIndex;
			queue = PathTracer.TempQueues[threadIndex];
			connections = PathTracer.TempConnectionLists[threadIndex];
			if (queue == null)
			{
				queue = (PathTracer.TempQueues[threadIndex] = new PathTracer.QueueItem[16]);
				connections = (PathTracer.TempConnectionLists[threadIndex] = new List<GraphNode>());
			}
		}

		// Token: 0x06000B2B RID: 2859 RVA: 0x0003CCA0 File Offset: 0x0003AEA0
		private CircularBuffer<GraphNode> LocalSearch(GraphNode currentNode, Vector3 point, int maxNodesToSearch, NativeMovementPlane movementPlane, bool reverse)
		{
			PathTracer.QueueItem[] array;
			List<GraphNode> list;
			this.GetTempQueue(out array, out list);
			int i = 0;
			int num = 0;
			NavGraph graph = currentNode.Graph;
			BBTree.ProjectionParams projectionParams;
			Vector3 vector;
			if (this.partGraphType == PathTracer.PartGraphType.Navmesh)
			{
				DistanceMetric distanceMetric = DistanceMetric.ClosestAsSeenFromAboveSoft(movementPlane.up);
				NavmeshBase navmeshBase = graph as NavmeshBase;
				projectionParams = new BBTree.ProjectionParams(ref distanceMetric, navmeshBase.transform);
				vector = navmeshBase.transform.InverseTransform(point);
			}
			else if (this.partGraphType == PathTracer.PartGraphType.Grid)
			{
				projectionParams = default(BBTree.ProjectionParams);
				vector = (graph as GridGraph).transform.InverseTransform(point);
			}
			else
			{
				projectionParams = default(BBTree.ProjectionParams);
				vector = point;
			}
			float num2 = PathTracer.SquaredDistanceToNode(currentNode, vector, ref projectionParams);
			array[0] = new PathTracer.QueueItem
			{
				node = currentNode,
				parent = -1,
				distance = num2
			};
			num++;
			int num3 = 0;
			while (i < num)
			{
				int num4 = i;
				GraphNode node2 = array[num4].node;
				i++;
				if (PathTracer.ContainsPoint(node2, point, movementPlane))
				{
					num3 = num4;
					break;
				}
				float distance = array[num4].distance;
				if (distance < num2)
				{
					num2 = distance;
					num3 = num4;
				}
				float num5 = distance * 1.1024998f + 0.05f;
				node2.GetConnections<List<GraphNode>>(delegate(GraphNode node, ref List<GraphNode> ls)
				{
					ls.Add(node);
				}, ref list, 32);
				for (int j = 0; j < list.Count; j++)
				{
					GraphNode graphNode = list[j];
					if (num < maxNodesToSearch && graphNode.GraphIndex == node2.GraphIndex && (reverse ? (this.traversalConstraint.CanTraverse(graphNode) && this.traversalConstraint.CanTraverse(graphNode, node2)) : this.traversalConstraint.CanTraverse(node2, graphNode)))
					{
						float num6 = PathTracer.SquaredDistanceToNode(graphNode, vector, ref projectionParams);
						if (num6 < num5 && !PathTracer.QueueHasNode(array, num, graphNode))
						{
							array[num] = new PathTracer.QueueItem
							{
								node = graphNode,
								parent = num4,
								distance = num6
							};
							num++;
						}
					}
				}
				list.Clear();
			}
			CircularBuffer<GraphNode> circularBuffer = new CircularBuffer<GraphNode>(8);
			while (num3 != -1)
			{
				circularBuffer.PushStart(array[num3].node);
				num3 = array[num3].parent;
			}
			list.Clear();
			for (int k = 0; k < num; k++)
			{
				array[k].node = null;
			}
			if (this.partGraphType == PathTracer.PartGraphType.Grid)
			{
				CircularBuffer<int> circularBuffer2 = default(CircularBuffer<int>);
				PathTracer.RemoveGridPathDiagonals(null, 0, ref circularBuffer, ref circularBuffer2, ref this.traversalConstraint);
			}
			return circularBuffer;
		}

		// Token: 0x06000B2C RID: 2860 RVA: 0x0003CF54 File Offset: 0x0003B154
		public void DrawFunnel(CommandBuilder draw, NativeMovementPlane movementPlane)
		{
			if (this.parts == null)
			{
				return;
			}
			Funnel.PathPart pathPart = this.parts[this.firstPartIndex];
			this.funnelState.PushStart(pathPart.startPoint, pathPart.startPoint);
			this.funnelState.PushEnd(pathPart.endPoint, pathPart.endPoint);
			using (draw.WithLineWidth(2f, true))
			{
				draw.Polyline<NativeCircularBuffer<float3>>(this.funnelState.leftFunnel, false);
				draw.Polyline<NativeCircularBuffer<float3>>(this.funnelState.rightFunnel, false);
			}
			if (this.funnelState.unwrappedPortals.Length > 1)
			{
				using (draw.WithLineWidth(1f, true))
				{
					float3 up = movementPlane.up;
					float4x3 float4x = this.funnelState.UnwrappedPortalsToWorldMatrix(up);
					float4x4 float4x2 = new float4x4(float4x.c0, float4x.c1, new float4(0f, 0f, 1f, 0f), float4x.c2);
					using (draw.WithMatrix(float4x2))
					{
						float2 @float = this.funnelState.unwrappedPortals[0].xy;
						float2 float2 = this.funnelState.unwrappedPortals[1].xy;
						for (int i = 0; i < this.funnelState.unwrappedPortals.Length; i++)
						{
							float2 xy = this.funnelState.unwrappedPortals[i].xy;
							float2 zw = this.funnelState.unwrappedPortals[i].zw;
							draw.xy.Line(xy, zw, Palette.Colorbrewer.Set1.Brown);
							draw.xy.Line(@float, xy, Palette.Colorbrewer.Set1.Brown);
							draw.xy.Line(float2, zw, Palette.Colorbrewer.Set1.Brown);
							@float = xy;
							float2 = zw;
						}
					}
				}
			}
			using (draw.WithColor(new Color(0f, 0f, 0f, 0.2f)))
			{
				for (int j = 0; j < this.funnelState.leftFunnel.Length - 1; j++)
				{
					draw.SolidTriangle(this.funnelState.leftFunnel[j], this.funnelState.rightFunnel[j], this.funnelState.rightFunnel[j + 1]);
					draw.SolidTriangle(this.funnelState.leftFunnel[j], this.funnelState.leftFunnel[j + 1], this.funnelState.rightFunnel[j + 1]);
				}
			}
			using (draw.WithColor(new Color(0f, 0f, 1f, 0.5f)))
			{
				for (int k = 0; k < this.funnelState.leftFunnel.Length; k++)
				{
					draw.Line(this.funnelState.leftFunnel[k], this.funnelState.rightFunnel[k]);
				}
			}
			this.funnelState.PopStart();
			this.funnelState.PopEnd();
		}

		// Token: 0x06000B2D RID: 2861 RVA: 0x0003D364 File Offset: 0x0003B564
		private static Int3 MaybeSetYZero(Int3 p, bool setYToZero)
		{
			if (setYToZero)
			{
				p.y = 0;
			}
			return p;
		}

		// Token: 0x06000B2E RID: 2862 RVA: 0x0003D374 File Offset: 0x0003B574
		private static bool IsInnerVertex(CircularBuffer<GraphNode> nodes, Funnel.PathPart part, int portalIndex, bool rightSide, List<GraphNode> alternativeNodes, ref TraversalConstraint traversalConstraint, ref TraversalCosts traversalCosts, out int startIndex, out int endIndex)
		{
			GraphNode absolute = nodes.GetAbsolute(portalIndex);
			if (absolute is TriangleMeshNode)
			{
				return PathTracer.IsInnerVertexTriangleMesh(nodes, part, portalIndex, rightSide, alternativeNodes, ref traversalConstraint, ref traversalCosts, out startIndex, out endIndex);
			}
			if (absolute is GridNodeBase)
			{
				throw new InvalidOperationException("Grid nodes are not supported. Should have been handled by the SimplifyGridInnerVertex method");
			}
			startIndex = portalIndex;
			endIndex = portalIndex + 1;
			return false;
		}

		// Token: 0x06000B2F RID: 2863 RVA: 0x0003D3C4 File Offset: 0x0003B5C4
		private static bool IsInnerVertexTriangleMesh(CircularBuffer<GraphNode> nodes, Funnel.PathPart part, int portalIndex, bool rightSide, List<GraphNode> alternativeNodes, ref TraversalConstraint traversalConstraint, ref TraversalCosts traversalCosts, out int startIndex, out int endIndex)
		{
			startIndex = portalIndex;
			endIndex = portalIndex + 1;
			TriangleMeshNode triangleMeshNode = nodes.GetAbsolute(startIndex) as TriangleMeshNode;
			TriangleMeshNode triangleMeshNode2 = nodes.GetAbsolute(endIndex) as TriangleMeshNode;
			if (triangleMeshNode == null || triangleMeshNode2 == null || !PathTracer.Valid(triangleMeshNode) || !PathTracer.Valid(triangleMeshNode2))
			{
				return false;
			}
			Int3 @int;
			Int3 int2;
			int num;
			int num2;
			if (!triangleMeshNode.GetPortalInGraphSpace(triangleMeshNode2, out @int, out int2, out num, out num2))
			{
				return false;
			}
			bool recalculateNormals = (triangleMeshNode.Graph as NavmeshBase).RecalculateNormals;
			Int3 int3 = PathTracer.MaybeSetYZero(rightSide ? int2 : @int, recalculateNormals);
			while (startIndex > part.startIndex)
			{
				TriangleMeshNode triangleMeshNode3 = nodes.GetAbsolute(startIndex - 1) as TriangleMeshNode;
				Int3 int4;
				Int3 int5;
				if (triangleMeshNode3 == null || !PathTracer.Valid(triangleMeshNode3) || !triangleMeshNode3.GetPortalInGraphSpace(triangleMeshNode, out int4, out int5, out num2, out num) || !(PathTracer.MaybeSetYZero(rightSide ? int5 : int4, recalculateNormals) == int3))
				{
					break;
				}
				triangleMeshNode = triangleMeshNode3;
				startIndex--;
			}
			while (endIndex < part.endIndex)
			{
				TriangleMeshNode triangleMeshNode4 = nodes.GetAbsolute(endIndex + 1) as TriangleMeshNode;
				Int3 int6;
				Int3 int7;
				if (triangleMeshNode4 == null || !PathTracer.Valid(triangleMeshNode4) || !triangleMeshNode2.GetPortalInGraphSpace(triangleMeshNode4, out int6, out int7, out num, out num2) || !(PathTracer.MaybeSetYZero(rightSide ? int7 : int6, recalculateNormals) == int3))
				{
					break;
				}
				triangleMeshNode2 = triangleMeshNode4;
				endIndex++;
				if (triangleMeshNode2 == triangleMeshNode)
				{
					break;
				}
			}
			TriangleMeshNode triangleMeshNode5 = triangleMeshNode;
			int num3 = 0;
			alternativeNodes.Add(triangleMeshNode);
			if (triangleMeshNode == triangleMeshNode2)
			{
				return true;
			}
			bool flag = false;
			while (!flag)
			{
				bool flag2 = false;
				int i = 0;
				while (i < triangleMeshNode5.connections.Length)
				{
					TriangleMeshNode triangleMeshNode6 = triangleMeshNode5.connections[i].node as TriangleMeshNode;
					Int3 int8;
					Int3 int9;
					if (triangleMeshNode6 != null && triangleMeshNode5.connections[i].isOutgoing && traversalConstraint.CanTraverse(triangleMeshNode5, triangleMeshNode6) && triangleMeshNode5.GetPortalInGraphSpace(triangleMeshNode6, out int8, out int9, out num2, out num) && PathTracer.MaybeSetYZero(rightSide ? int8 : int9, recalculateNormals) == int3)
					{
						triangleMeshNode5 = triangleMeshNode6;
						alternativeNodes.Add(triangleMeshNode5);
						flag2 = true;
						if (num3++ > 100)
						{
							throw new Exception("Caught in a potentially infinite loop. The navmesh probably contains degenerate geometry.");
						}
						if (triangleMeshNode5 == triangleMeshNode2)
						{
							flag = true;
							break;
						}
						break;
					}
					else
					{
						i++;
					}
				}
				if (!flag2)
				{
					return false;
				}
			}
			int num4 = 0;
			for (int j = 0; j < alternativeNodes.Count - 1; j++)
			{
				num4 += (int)traversalCosts.GetConnectionCost(alternativeNodes[j], alternativeNodes[j + 1]);
			}
			for (int k = startIndex; k < endIndex; k++)
			{
				num4 -= (int)traversalCosts.GetConnectionCost(nodes.GetAbsolute(k), nodes.GetAbsolute(k + 1));
			}
			if (num4 > 0)
			{
				return false;
			}
			float num5 = 0f;
			num5 -= 0.5f * traversalCosts.GetTraversalCostMultiplier(triangleMeshNode);
			num5 -= 0.5f * traversalCosts.GetTraversalCostMultiplier(triangleMeshNode2);
			float num6 = 0f;
			for (int l = 1; l < alternativeNodes.Count; l++)
			{
				num6 = math.max(num6, traversalCosts.GetTraversalCostMultiplier(alternativeNodes[l]));
			}
			num5 += num6;
			return num5 <= 0f;
		}

		// Token: 0x06000B30 RID: 2864 RVA: 0x0003D6E4 File Offset: 0x0003B8E4
		private bool FirstInnerVertex(NativeArray<int> indices, int numCorners, List<GraphNode> alternativePath, out int alternativeStartIndex, out int alternativeEndIndex)
		{
			Funnel.PathPart pathPart = this.parts[this.firstPartIndex];
			for (int i = 0; i < numCorners; i++)
			{
				int num = indices[i];
				bool flag = (num & 1073741824) != 0;
				int num2 = num & 1073741823;
				if ((this.portalIsNotInnerCorner[num2] & ((flag || 2 != 0) ? 1 : 0)) == 0)
				{
					alternativePath.Clear();
					if (PathTracer.IsInnerVertex(this.nodes, pathPart, pathPart.startIndex + num2, flag, alternativePath, ref this.traversalConstraint, ref this.traversalCosts, out alternativeStartIndex, out alternativeEndIndex))
					{
						return true;
					}
					this.portalIsNotInnerCorner[num2] = this.portalIsNotInnerCorner[num2] | (flag ? 1 : 2);
				}
			}
			alternativeStartIndex = -1;
			alternativeEndIndex = -1;
			return false;
		}

		// Token: 0x06000B31 RID: 2865 RVA: 0x0003D79F File Offset: 0x0003B99F
		public float EstimateRemainingPath(int maxCorners, ref NativeMovementPlane movementPlane)
		{
			return PathTracer.EstimateRemainingPath(ref this.funnelState, ref this.parts[this.firstPartIndex], maxCorners, ref movementPlane);
		}

		// Token: 0x06000B32 RID: 2866 RVA: 0x0003D7BF File Offset: 0x0003B9BF
		[BurstCompile]
		private static float EstimateRemainingPath(ref Funnel.FunnelState funnelState, ref Funnel.PathPart part, int maxCorners, ref NativeMovementPlane movementPlane)
		{
			return PathTracer.EstimateRemainingPath_00000A98$BurstDirectCall.Invoke(ref funnelState, ref part, maxCorners, ref movementPlane);
		}

		// Token: 0x06000B33 RID: 2867 RVA: 0x0003D7CC File Offset: 0x0003B9CC
		public void GetNextCorners(NativeList<float3> buffer, int maxCorners, ref NativeArray<int> scratchArray, Allocator allocator, bool allowSimplify = true)
		{
			bool flag;
			int nextCornerIndices = this.GetNextCornerIndices(ref scratchArray, maxCorners, allocator, allowSimplify, out flag);
			Funnel.PathPart pathPart = this.parts[this.firstPartIndex];
			this.funnelState.ConvertCornerIndicesToPath(scratchArray, nextCornerIndices, false, pathPart.startPoint, pathPart.endPoint, flag, buffer);
		}

		// Token: 0x06000B34 RID: 2868 RVA: 0x0003D828 File Offset: 0x0003BA28
		public int GetNextCornerIndices(ref NativeArray<int> buffer, int maxCorners, Allocator allocator, bool allowSimplify, out bool lastCorner)
		{
			if (this.pathEmptyAndUnrepairable)
			{
				lastCorner = true;
				return 0;
			}
			int num = (allowSimplify ? 3 : 0);
			maxCorners--;
			if (PathTracer.scratchList == null)
			{
				PathTracer.scratchList = new List<GraphNode>(8);
			}
			List<GraphNode> list = PathTracer.scratchList;
			int num3;
			for (;;)
			{
				int num2 = math.max(0, math.min(maxCorners, this.funnelState.leftFunnel.Length));
				if (!buffer.IsCreated || buffer.Length < num2)
				{
					if (buffer.IsCreated)
					{
						buffer.Dispose();
					}
					buffer = new NativeArray<int>(math.ceilpow2(num2), allocator, NativeArrayOptions.UninitializedMemory);
				}
				NativeArray<int> nativeArray = buffer;
				Funnel.PathPart pathPart = this.parts[this.firstPartIndex];
				num3 = this.funnelState.CalculateNextCornerIndices(num2, nativeArray, pathPart.startPoint, pathPart.endPoint, out lastCorner);
				if (num <= 0)
				{
					return num3;
				}
				if (this.partGraphType == PathTracer.PartGraphType.Grid)
				{
					int num4;
					int num5;
					if (!PathTracer.SimplifyGridInnerVertex(ref this.nodes, nativeArray.AsUnsafeSpan<int>().Slice(0, num3), pathPart, ref this.portalIsNotInnerCorner, list, out num4, out num5, ref this.traversalConstraint, ref this.traversalCosts, lastCorner))
					{
						return num3;
					}
					if (!this.SplicePath(num4, num5 - num4 + 1, list))
					{
						break;
					}
					num--;
					ushort num6 = this.version;
					this.version = num6 + 1;
				}
				else
				{
					int num7;
					int num8;
					if (!this.FirstInnerVertex(nativeArray, num3, list, out num7, out num8))
					{
						return num3;
					}
					if (!this.SplicePath(num7, num8 - num7 + 1, list))
					{
						goto IL_0186;
					}
					num--;
					ushort num6 = this.version;
					this.version = num6 + 1;
				}
			}
			this.firstPartContainsDestroyedNodes = true;
			return num3;
			IL_0186:
			this.firstPartContainsDestroyedNodes = true;
			return num3;
		}

		// Token: 0x06000B35 RID: 2869 RVA: 0x0003D9C4 File Offset: 0x0003BBC4
		public void ConvertCornerIndicesToPathProjected(NativeArray<int> cornerIndices, int numCorners, bool lastCorner, NativeList<float3> buffer, float3 up)
		{
			Funnel.PathPart pathPart = this.parts[this.firstPartIndex];
			this.funnelState.ConvertCornerIndicesToPathProjected(cornerIndices.AsUnsafeReadOnlySpan<int>().Slice(0, numCorners), false, pathPart.startPoint, pathPart.endPoint, lastCorner, buffer, up);
		}

		// Token: 0x06000B36 RID: 2870 RVA: 0x0003DA1A File Offset: 0x0003BC1A
		[BurstCompile]
		public static float RemainingDistanceLowerBound(in UnsafeSpan<float3> nextCorners, in float3 endOfPart, in NativeMovementPlane movementPlane)
		{
			return PathTracer.RemainingDistanceLowerBound_00000A9C$BurstDirectCall.Invoke(in nextCorners, in endOfPart, in movementPlane);
		}

		// Token: 0x06000B37 RID: 2871 RVA: 0x0003DA24 File Offset: 0x0003BC24
		public unsafe void PopParts(int count)
		{
			if (this.firstPartIndex + count >= this.parts.Length)
			{
				throw new InvalidOperationException("Cannot pop the last part of a path");
			}
			this.firstPartIndex += count;
			ushort version = this.version;
			this.version = version + 1;
			Funnel.PathPart pathPart = this.parts[this.firstPartIndex];
			while (this.nodes.AbsoluteStartIndex < pathPart.startIndex)
			{
				this.nodes.PopStart();
				this.nodeHashes.PopStart();
			}
			this.startNode = ((this.nodes.Length > 0) ? (*this.nodes.First) : null);
			this.firstPartContainsDestroyedNodes = false;
			if (this.GetPartType(0) == Funnel.PartType.OffMeshLink)
			{
				this.partGraphType = PathTracer.PartGraphType.OffMeshLink;
				this.SetFunnelState(pathPart);
				return;
			}
			this.partGraphType = PathTracer.PartGraphTypeFromNode(this.startNode);
			int i = pathPart.startIndex;
			while (i <= pathPart.endIndex)
			{
				if (!this.ValidInPath(i))
				{
					this.RemoveAllPartsExceptFirst();
					while (this.nodes.AbsoluteEndIndex > i)
					{
						this.nodes.PopEnd();
						this.nodeHashes.PopEnd();
					}
					pathPart.endIndex = i;
					this.parts[this.firstPartIndex] = pathPart;
					if (i == pathPart.startIndex)
					{
						this.firstPartContainsDestroyedNodes = true;
						this.funnelState.Clear();
						this.portalIsNotInnerCorner.Clear();
						this.startNode = null;
						return;
					}
					this.endIsUpToDate = false;
					this.nodes.PopEnd();
					this.nodeHashes.PopEnd();
					pathPart.endIndex = i - 1;
					this.parts[this.firstPartIndex] = pathPart;
					break;
				}
				else
				{
					i++;
				}
			}
			if (this.partGraphType == PathTracer.PartGraphType.Grid)
			{
				PathTracer.RemoveGridPathDiagonals(this.parts, this.firstPartIndex, ref this.nodes, ref this.nodeHashes, ref this.traversalConstraint);
				pathPart = this.parts[this.firstPartIndex];
			}
			this.SetFunnelState(pathPart);
		}

		// Token: 0x06000B38 RID: 2872 RVA: 0x0003DC20 File Offset: 0x0003BE20
		public void RemoveAllButFirstNode()
		{
			if (this.hasPath)
			{
				this.RemoveAllPartsExceptFirst();
				ref Funnel.PathPart ptr = ref this.parts[this.firstPartIndex];
				while (this.nodes.AbsoluteEndIndex > ptr.startIndex)
				{
					this.nodes.PopEnd();
					this.nodeHashes.PopEnd();
				}
				ptr.endIndex = ptr.startIndex;
				ptr.endPoint = ptr.startPoint;
				this.SetFunnelState(ptr);
				ushort version = this.version;
				this.version = version + 1;
			}
		}

		// Token: 0x06000B39 RID: 2873 RVA: 0x0003DCB0 File Offset: 0x0003BEB0
		private void RemoveAllPartsExceptFirst()
		{
			if (this.partCount <= 1)
			{
				return;
			}
			this.parts = new Funnel.PathPart[] { this.parts[this.firstPartIndex] };
			this.firstPartIndex = 0;
			while (this.nodes.AbsoluteEndIndex > this.parts[0].endIndex)
			{
				this.nodes.PopEnd();
				this.nodeHashes.PopEnd();
			}
			ushort version = this.version;
			this.version = version + 1;
		}

		// Token: 0x06000B3A RID: 2874 RVA: 0x0003DD3D File Offset: 0x0003BF3D
		public readonly Funnel.PartType GetPartType(int partIndex = 0)
		{
			return this.parts[this.firstPartIndex + partIndex].type;
		}

		// Token: 0x06000B3B RID: 2875 RVA: 0x0003DD58 File Offset: 0x0003BF58
		public readonly bool PartContainsDestroyedNodes(int partIndex = 0)
		{
			if (partIndex < 0 || partIndex >= this.partCount)
			{
				throw new ArgumentOutOfRangeException("partIndex");
			}
			Funnel.PathPart pathPart = this.parts[this.firstPartIndex + partIndex];
			for (int i = pathPart.startIndex; i <= pathPart.endIndex; i++)
			{
				if (!this.ValidInPath(i))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06000B3C RID: 2876 RVA: 0x0003DDB4 File Offset: 0x0003BFB4
		public OffMeshLinks.OffMeshLinkTracer GetLinkInfo(int partIndex = 0)
		{
			if (partIndex < 0 || partIndex >= this.partCount)
			{
				throw new ArgumentOutOfRangeException("partIndex");
			}
			if (this.GetPartType(partIndex) != Funnel.PartType.OffMeshLink)
			{
				throw new ArgumentException("Part is not an off-mesh link");
			}
			Funnel.PathPart pathPart = this.parts[this.firstPartIndex + partIndex];
			LinkNode linkNode = this.nodes.GetAbsolute(pathPart.startIndex) as LinkNode;
			LinkNode linkNode2 = this.nodes.GetAbsolute(pathPart.endIndex) as LinkNode;
			if (linkNode == null)
			{
				throw new Exception("Expected a link node");
			}
			if (linkNode2 == null)
			{
				throw new Exception("Expected a link node");
			}
			if (linkNode.Destroyed)
			{
				throw new Exception("Start node is destroyed");
			}
			if (linkNode2.Destroyed)
			{
				throw new Exception("End node is destroyed");
			}
			bool flag;
			if (linkNode.linkConcrete.startLinkNode == linkNode)
			{
				flag = false;
			}
			else
			{
				if (linkNode.linkConcrete.startLinkNode != linkNode2)
				{
					throw new Exception("Link node is not part of the link");
				}
				flag = true;
			}
			return new OffMeshLinks.OffMeshLinkTracer(linkNode.linkConcrete, flag);
		}

		// Token: 0x06000B3D RID: 2877 RVA: 0x0003DEAC File Offset: 0x0003C0AC
		private static void GetTemporaryPortalLists(out List<float3> left, out List<float3> right)
		{
			if (PathTracer.tmpLeftPortals == null)
			{
				PathTracer.tmpLeftPortals = new List<float3>();
			}
			if (PathTracer.tmpRightPortals == null)
			{
				PathTracer.tmpRightPortals = new List<float3>();
			}
			PathTracer.tmpLeftPortals.Clear();
			PathTracer.tmpRightPortals.Clear();
			left = PathTracer.tmpLeftPortals;
			right = PathTracer.tmpRightPortals;
		}

		// Token: 0x06000B3E RID: 2878 RVA: 0x0003DF00 File Offset: 0x0003C100
		private void SetFunnelState(Funnel.PathPart part)
		{
			this.funnelState.Clear();
			this.portalIsNotInnerCorner.Clear();
			if (part.type == Funnel.PartType.NodeSequence)
			{
				GridGraph gridGraph = this.nodes.GetAbsolute(part.startIndex).Graph as GridGraph;
				if (gridGraph != null)
				{
					this.funnelState.projectionAxis = gridGraph.transform.WorldUpAtGraphPosition(Vector3.zero);
				}
				if (part.endIndex - part.startIndex > 0)
				{
					List<float3> list;
					List<float3> list2;
					PathTracer.GetTemporaryPortalLists(out list, out list2);
					list.Capacity = Mathf.Max(list.Capacity, part.endIndex - part.startIndex);
					list2.Capacity = Mathf.Max(list2.Capacity, part.endIndex - part.startIndex);
					this.CalculateFunnelPortals(part.startIndex, part.endIndex, list, list2);
					this.funnelState.Splice(0, 0, list, list2);
					for (int i = 0; i < list.Count; i++)
					{
						this.portalIsNotInnerCorner.PushEnd(0);
					}
				}
			}
			ushort version = this.version;
			this.version = version + 1;
		}

		// Token: 0x06000B3F RID: 2879 RVA: 0x0003E01C File Offset: 0x0003C21C
		private void CalculateFunnelPortals(int startNodeIndex, int endNodeIndex, List<float3> outLeftPortals, List<float3> outRightPortals)
		{
			GraphNode graphNode = this.nodes.GetAbsolute(startNodeIndex);
			for (int i = startNodeIndex + 1; i <= endNodeIndex; i++)
			{
				GraphNode absolute = this.nodes.GetAbsolute(i);
				Vector3 vector;
				Vector3 vector2;
				if (!graphNode.GetPortal(absolute, out vector, out vector2))
				{
					string[] array = new string[6];
					array[0] = "Couldn't find a portal from ";
					int num = 1;
					GraphNode graphNode2 = graphNode;
					array[num] = ((graphNode2 != null) ? graphNode2.ToString() : null);
					array[2] = " ";
					int num2 = 3;
					GraphNode graphNode3 = absolute;
					array[num2] = ((graphNode3 != null) ? graphNode3.ToString() : null);
					array[4] = " ";
					array[5] = graphNode.ContainsOutgoingConnection(absolute).ToString();
					throw new InvalidOperationException(string.Concat(array));
				}
				outLeftPortals.Add(vector);
				outRightPortals.Add(vector2);
				graphNode = absolute;
			}
		}

		// Token: 0x06000B40 RID: 2880 RVA: 0x0003E0E0 File Offset: 0x0003C2E0
		public void SetFromSingleNode(GraphNode node, Vector3 position, NativeMovementPlane movementPlane, TraversalConstraint traversalConstraint, TraversalCosts traversalCosts)
		{
			this.SetPath(new List<Funnel.PathPart>
			{
				new Funnel.PathPart
				{
					startIndex = 0,
					endIndex = 0,
					startPoint = position,
					endPoint = position
				}
			}, new List<GraphNode> { node }, position, position, movementPlane, traversalConstraint, traversalCosts);
		}

		// Token: 0x06000B41 RID: 2881 RVA: 0x0003E13C File Offset: 0x0003C33C
		public void Clear()
		{
			this.funnelState.Clear();
			this.parts = null;
			this.nodes.Clear();
			this.nodeHashes.Clear();
			this.portalIsNotInnerCorner.Clear();
			this.unclampedEndPoint = (this.unclampedStartPoint = Vector3.zero);
			this.firstPartIndex = 0;
			this.startIsUpToDate = false;
			this.endIsUpToDate = false;
			this.firstPartContainsDestroyedNodes = false;
			this.startNodeInternal = null;
			this.partGraphType = PathTracer.PartGraphType.Navmesh;
			this.pathEmptyAndUnrepairable = false;
		}

		// Token: 0x06000B42 RID: 2882 RVA: 0x0003E1C4 File Offset: 0x0003C3C4
		private unsafe static int2 ResolveNormalizedGridPoint(GridGraph grid, ref CircularBuffer<GraphNode> nodes, UnsafeSpan<int> cornerIndices, Funnel.PathPart part, int index, out int nodeIndex)
		{
			if (index < 0 || index >= cornerIndices.Length)
			{
				Vector3 vector = ((index < 0) ? part.startPoint : part.endPoint);
				nodeIndex = ((index < 0) ? part.startIndex : part.endIndex);
				Vector3 vector2 = grid.transform.InverseTransform(vector);
				Vector2Int coordinatesInGrid = (nodes.GetAbsolute(nodeIndex) as GridNodeBase).CoordinatesInGrid;
				return new int2(math.clamp((int)(1024f * (vector2.x - (float)coordinatesInGrid.x)), 0, 1024), math.clamp((int)(1024f * (vector2.z - (float)coordinatesInGrid.y)), 0, 1024));
			}
			bool flag = (*cornerIndices[index] & 1073741824) != 0;
			nodeIndex = part.startIndex + (*cornerIndices[index] & 1073741823);
			GridNodeBase gridNodeBase = nodes.GetAbsolute(nodeIndex) as GridNodeBase;
			GridNodeBase gridNodeBase2 = nodes.GetAbsolute(nodeIndex + 1) as GridNodeBase;
			Vector2Int coordinatesInGrid2 = gridNodeBase.CoordinatesInGrid;
			Vector2Int coordinatesInGrid3 = gridNodeBase2.CoordinatesInGrid;
			int num = coordinatesInGrid3.x - coordinatesInGrid2.x;
			int num2 = coordinatesInGrid3.y - coordinatesInGrid2.y;
			int num3 = GridNodeBase.OffsetToConnectionDirection(num, num2);
			if (num3 > 4)
			{
				throw new Exception("Diagonal connections are not supported");
			}
			int num4 = GridGraph.neighbourXOffsets[num3] + GridGraph.neighbourXOffsets[(num3 + (flag ? (-1) : 1) + 4) % 4];
			int num5 = GridGraph.neighbourZOffsets[num3] + GridGraph.neighbourZOffsets[(num3 + (flag ? (-1) : 1) + 4) % 4];
			return new int2(512 + 512 * num4, 512 + 512 * num5);
		}

		// Token: 0x06000B43 RID: 2883 RVA: 0x0003E370 File Offset: 0x0003C570
		private unsafe static bool SimplifyGridInnerVertex(ref CircularBuffer<GraphNode> nodes, UnsafeSpan<int> cornerIndices, Funnel.PathPart part, ref CircularBuffer<byte> portalIsNotInnerCorner, List<GraphNode> alternativePath, out int alternativeStartIndex, out int alternativeEndIndex, ref TraversalConstraint traversalConstraint, ref TraversalCosts traversalCosts, bool lastCorner)
		{
			bool flag = (lastCorner ? cornerIndices.Length : (cornerIndices.Length - 1)) != 0;
			alternativeStartIndex = -1;
			alternativeEndIndex = -1;
			if (!flag)
			{
				return false;
			}
			int num = 0;
			int num2 = *cornerIndices[num] & 1073741823;
			int num3 = (int)(portalIsNotInnerCorner[num2] % 32);
			portalIsNotInnerCorner[num2] = (byte)(num3 + 1);
			if ((num3 & 3) != 0)
			{
				return false;
			}
			num3 /= 4;
			int num4 = ((cornerIndices.length < 2U) ? part.endIndex : math.min(part.endIndex, part.startIndex + (*cornerIndices[1] & 1073741823) + 1));
			for (int i = part.startIndex; i < num4; i++)
			{
				GraphNode absolute = nodes.GetAbsolute(i);
				GraphNode absolute2 = nodes.GetAbsolute(i + 1);
				if (!PathTracer.Valid(absolute2) || !absolute.ContainsOutgoingConnection(absolute2))
				{
					return false;
				}
			}
			GridGraph gridGraph = GridNode.GetGridGraph(nodes.GetAbsolute(part.startIndex).GraphIndex);
			int num5;
			int2 @int = PathTracer.ResolveNormalizedGridPoint(gridGraph, ref nodes, cornerIndices, part, num - 1, out num5);
			int num6;
			int2 int2 = PathTracer.ResolveNormalizedGridPoint(gridGraph, ref nodes, cornerIndices, part, num + 1, out num6);
			int num7;
			int2 int3 = PathTracer.ResolveNormalizedGridPoint(gridGraph, ref nodes, cornerIndices, part, num, out num7);
			GridNodeBase gridNodeBase = nodes.GetAbsolute(num5) as GridNodeBase;
			GridNodeBase gridNodeBase2 = nodes.GetAbsolute(num7) as GridNodeBase;
			GridNodeBase gridNodeBase3 = nodes.GetAbsolute(num6) as GridNodeBase;
			if (num3 > 0)
			{
				int num8 = PathTracer.SplittingCoefficients[num3 * 2];
				int num9 = PathTracer.SplittingCoefficients[num3 * 2 + 1];
				num6 += (num7 - num6) * num8 / num9;
				if (num6 == num7)
				{
					return false;
				}
				Vector2Int vector2Int = gridNodeBase3.CoordinatesInGrid;
				Vector2Int coordinatesInGrid = gridNodeBase2.CoordinatesInGrid;
				int2 int4 = new int2(coordinatesInGrid.x * 1024, coordinatesInGrid.y * 1024) + int3;
				int2 int5 = new int2(vector2Int.x * 1024, vector2Int.y * 1024) + int2;
				gridNodeBase3 = nodes.GetAbsolute(num6) as GridNodeBase;
				vector2Int = gridNodeBase3.CoordinatesInGrid;
				float num10 = VectorMath.ClosestPointOnLineFactor(new Vector2Int(int4.x, int4.y), new Vector2Int(int5.x, int5.y), new Vector2Int(vector2Int.x * 1024 + 512, vector2Int.y * 1024 + 512));
				int2 int6 = new int2((int)math.lerp((float)int4.x, (float)int5.x, num10), (int)math.lerp((float)int4.y, (float)int5.y, num10)) - new int2(vector2Int.x * 1024, vector2Int.y * 1024);
				int2 = new int2(math.clamp(int6.x, 0, 1024), math.clamp(int6.y, 0, 1024));
			}
			alternativePath.Clear();
			GridHitInfo gridHitInfo;
			if (gridGraph.Linecast(gridNodeBase, @int, gridNodeBase3, int2, out gridHitInfo, ref traversalConstraint, alternativePath, false))
			{
				return false;
			}
			uint num11 = 0U;
			for (int j = 0; j < alternativePath.Count - 1; j++)
			{
				num11 += traversalCosts.GetConnectionCost(alternativePath[j], alternativePath[j + 1]);
			}
			uint num12 = 0U;
			for (int k = num5; k < num6; k++)
			{
				num12 += traversalCosts.GetConnectionCost(nodes.GetAbsolute(k), nodes.GetAbsolute(k + 1));
			}
			float num13 = 0f;
			for (int l = 0; l < alternativePath.Count; l++)
			{
				num13 += traversalCosts.GetTraversalCostMultiplier(alternativePath[l]);
			}
			num13 /= (float)alternativePath.Count;
			float num14 = 0f;
			for (int m = num5; m <= num6; m++)
			{
				num14 += traversalCosts.GetTraversalCostMultiplier(nodes.GetAbsolute(m));
			}
			num14 /= (float)(num6 - num5 + 1);
			Vector3 vector = gridNodeBase.UnNormalizePoint(@int / 1024f);
			Vector3 vector2 = gridNodeBase2.UnNormalizePoint(int2 / 1024f);
			Vector3 vector3 = gridNodeBase3.UnNormalizePoint(int2 / 1024f);
			float num15 = 1000f * (vector - vector3).magnitude;
			float num16 = 1000f * ((vector - vector2).magnitude + (vector2 - vector3).magnitude);
			float num17 = num11 + num15 * num13;
			float num18 = num12 + num16 * num14;
			if (num17 > num18)
			{
				return false;
			}
			alternativeStartIndex = num5;
			alternativeEndIndex = num6;
			return true;
		}

		// Token: 0x06000B44 RID: 2884 RVA: 0x0003E824 File Offset: 0x0003CA24
		private static void RemoveGridPathDiagonals(Funnel.PathPart[] parts, int partIndex, ref CircularBuffer<GraphNode> path, ref CircularBuffer<int> pathNodeHashes, ref TraversalConstraint traversalConstraint)
		{
			int num = 0;
			Funnel.PathPart pathPart = ((parts != null) ? parts[partIndex] : new Funnel.PathPart
			{
				startIndex = path.AbsoluteStartIndex,
				endIndex = path.AbsoluteEndIndex
			});
			for (int i = pathPart.endIndex - 1; i >= pathPart.startIndex; i--)
			{
				GridNodeBase gridNodeBase = path.GetAbsolute(i) as GridNodeBase;
				GridNodeBase gridNodeBase2 = path.GetAbsolute(i + 1) as GridNodeBase;
				int num2 = gridNodeBase2.XCoordinateInGrid - gridNodeBase.XCoordinateInGrid;
				int num3 = gridNodeBase2.ZCoordinateInGrid - gridNodeBase.ZCoordinateInGrid;
				int num4 = GridNodeBase.OffsetToConnectionDirection(num2, num3);
				if (num4 >= 4)
				{
					int num5 = num4 - 4;
					int num6 = (num4 - 4 + 1) % 4;
					GridNodeBase gridNodeBase3 = gridNodeBase.GetNeighbourAlongDirection(num5);
					if (gridNodeBase3 != null && !traversalConstraint.CanTraverse(gridNodeBase, gridNodeBase3))
					{
						gridNodeBase3 = null;
					}
					if (gridNodeBase3 != null && gridNodeBase3.GetNeighbourAlongDirection(num6) == gridNodeBase2 && traversalConstraint.CanTraverse(gridNodeBase3, gridNodeBase2))
					{
						path.InsertAbsolute(i + 1, gridNodeBase3);
						if (pathNodeHashes.Length > 0)
						{
							pathNodeHashes.InsertAbsolute(i + 1, PathTracer.HashNode(gridNodeBase3));
						}
						num++;
					}
					else
					{
						GridNodeBase gridNodeBase4 = gridNodeBase.GetNeighbourAlongDirection(num6);
						if (gridNodeBase4 != null && !traversalConstraint.CanTraverse(gridNodeBase, gridNodeBase4))
						{
							gridNodeBase4 = null;
						}
						if (gridNodeBase4 == null || gridNodeBase4.GetNeighbourAlongDirection(num5) != gridNodeBase2 || !traversalConstraint.CanTraverse(gridNodeBase4, gridNodeBase2))
						{
							throw new Exception("Axis-aligned connection not found");
						}
						path.InsertAbsolute(i + 1, gridNodeBase4);
						if (pathNodeHashes.Length > 0)
						{
							pathNodeHashes.InsertAbsolute(i + 1, PathTracer.HashNode(gridNodeBase4));
						}
						num++;
					}
				}
			}
			if (parts != null)
			{
				parts[partIndex].endIndex = parts[partIndex].endIndex + num;
				for (int j = partIndex + 1; j < parts.Length; j++)
				{
					int num7 = j;
					parts[num7].startIndex = parts[num7].startIndex + num;
					int num8 = j;
					parts[num8].endIndex = parts[num8].endIndex + num;
				}
			}
		}

		// Token: 0x06000B45 RID: 2885 RVA: 0x0003EA05 File Offset: 0x0003CC05
		private static PathTracer.PartGraphType PartGraphTypeFromNode(GraphNode node)
		{
			if (node == null)
			{
				return PathTracer.PartGraphType.Navmesh;
			}
			if (node is GridNodeBase)
			{
				return PathTracer.PartGraphType.Grid;
			}
			if (node is TriangleMeshNode)
			{
				return PathTracer.PartGraphType.Navmesh;
			}
			throw new Exception("The PathTracer (and by extension FollowerEntity component) cannot be used on graphs of type " + node.Graph.GetType().Name);
		}

		// Token: 0x06000B46 RID: 2886 RVA: 0x0003EA40 File Offset: 0x0003CC40
		public void SetPath(ABPath path, NativeMovementPlane movementPlane)
		{
			List<Funnel.PathPart> list = Funnel.SplitIntoParts(path);
			this.SetPath(list, path.path, path.originalStartPoint, path.originalEndPoint, movementPlane, path.traversalConstraint, path.traversalCosts);
			ListPool<Funnel.PathPart>.Release(ref list);
		}

		// Token: 0x06000B47 RID: 2887 RVA: 0x0003EA84 File Offset: 0x0003CC84
		public void SetPath(List<Funnel.PathPart> parts, List<GraphNode> nodes, Vector3 unclampedStartPoint, Vector3 unclampedEndPoint, NativeMovementPlane movementPlane, TraversalConstraint traversalConstraint, TraversalCosts traversalCosts)
		{
			this.traversalConstraint = traversalConstraint;
			this.traversalCosts = traversalCosts;
			this.startNode = ((nodes.Count > 0) ? nodes[0] : null);
			this.partGraphType = PathTracer.PartGraphTypeFromNode(this.startNode);
			this.unclampedEndPoint = unclampedEndPoint;
			this.unclampedStartPoint = unclampedStartPoint;
			this.firstPartContainsDestroyedNodes = false;
			this.startIsUpToDate = true;
			this.endIsUpToDate = true;
			this.parts = parts.ToArray();
			this.nodes.Clear();
			this.nodes.AddRange(nodes);
			this.nodeHashes.Clear();
			this.pathEmptyAndUnrepairable = false;
			for (int i = 0; i < nodes.Count; i++)
			{
				this.nodeHashes.PushEnd(PathTracer.HashNode(nodes[i]));
			}
			this.firstPartIndex = 0;
			if (this.partGraphType == PathTracer.PartGraphType.Grid)
			{
				PathTracer.RemoveGridPathDiagonals(this.parts, 0, ref this.nodes, ref this.nodeHashes, ref traversalConstraint);
			}
			this.SetFunnelState(this.parts[this.firstPartIndex]);
			ushort version = this.version;
			this.version = version + 1;
			this.Repair(unclampedStartPoint, true, PathTracer.RepairQuality.Low, movementPlane, false);
			this.Repair(unclampedEndPoint, false, PathTracer.RepairQuality.Low, movementPlane, false);
		}

		// Token: 0x06000B48 RID: 2888 RVA: 0x0003EBB8 File Offset: 0x0003CDB8
		public PathTracer Clone()
		{
			return new PathTracer
			{
				parts = ((this.parts != null) ? (this.parts.Clone() as Funnel.PathPart[]) : null),
				nodes = this.nodes.Clone(),
				nodeHashes = this.nodeHashes.Clone(),
				portalIsNotInnerCorner = this.portalIsNotInnerCorner.Clone(),
				funnelState = this.funnelState.Clone(),
				unclampedEndPoint = this.unclampedEndPoint,
				unclampedStartPoint = this.unclampedStartPoint,
				startNodeInternal = this.startNodeInternal,
				pathEmptyAndUnrepairableBacking = this.pathEmptyAndUnrepairableBacking,
				firstPartIndex = this.firstPartIndex,
				startIsUpToDate = this.startIsUpToDate,
				endIsUpToDate = this.endIsUpToDate,
				firstPartContainsDestroyedNodes = this.firstPartContainsDestroyedNodes,
				version = this.version,
				traversalConstraint = this.traversalConstraint,
				traversalCosts = this.traversalCosts,
				partGraphType = this.partGraphType
			};
		}

		// Token: 0x06000B4A RID: 2890 RVA: 0x0003ED70 File Offset: 0x0003CF70
		[BurstCompile]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal static bool ContainsAndProject$BurstManaged(ref Int3 a, ref Int3 b, ref Int3 c, ref Vector3 p, float height, ref NativeMovementPlane movementPlane, out Vector3 projected)
		{
			int3 @int = (int3)a;
			int3 int2 = (int3)b;
			int3 int3 = (int3)c;
			int3 int4 = (int3)((Int3)p);
			if (!Polygon.ContainsPoint(ref @int, ref int2, ref int3, ref int4, ref movementPlane))
			{
				projected = Vector3.zero;
				return false;
			}
			float3 @float = (Vector3)a;
			float3 float2 = (Vector3)b;
			float3 float3 = (Vector3)c;
			float3 float4 = p;
			float num = height * 0.5f;
			float3 float5 = PathTracer.ProjectOnSurface(@float, float2, float3, float4, movementPlane.up);
			if (math.lengthsq(float5 - float4) > num * num)
			{
				projected = Vector3.zero;
				return false;
			}
			projected = float5;
			return true;
		}

		// Token: 0x06000B4B RID: 2891 RVA: 0x0003EE64 File Offset: 0x0003D064
		[BurstCompile]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal static float EstimateRemainingPath$BurstManaged(ref Funnel.FunnelState funnelState, ref Funnel.PathPart part, int maxCorners, ref NativeMovementPlane movementPlane)
		{
			NativeList<float3> nativeList = new NativeList<float3>(maxCorners, Allocator.Temp);
			NativeArray<int> nativeArray = new NativeArray<int>(maxCorners, Allocator.Temp, NativeArrayOptions.ClearMemory);
			maxCorners--;
			maxCorners = math.max(0, math.min(maxCorners, funnelState.leftFunnel.Length));
			bool flag;
			int num = funnelState.CalculateNextCornerIndices(maxCorners, nativeArray, part.startPoint, part.endPoint, out flag);
			funnelState.ConvertCornerIndicesToPath(nativeArray, num, false, part.startPoint, part.endPoint, flag, nativeList);
			UnsafeSpan<float3> unsafeSpan = nativeList.AsUnsafeSpan<float3>();
			float3 @float = part.endPoint;
			return PathTracer.RemainingDistanceLowerBound(in unsafeSpan, in @float, in movementPlane);
		}

		// Token: 0x06000B4C RID: 2892 RVA: 0x0003EF08 File Offset: 0x0003D108
		[BurstCompile]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal unsafe static float RemainingDistanceLowerBound$BurstManaged(in UnsafeSpan<float3> nextCorners, in float3 endOfPart, in NativeMovementPlane movementPlane)
		{
			if (nextCorners.Length == 0)
			{
				return 0f;
			}
			float3 @float = *nextCorners[0];
			float num = 0f;
			for (int i = 1; i < nextCorners.Length; i++)
			{
				float3 float2 = *nextCorners[i];
				num += math.length(movementPlane.ToPlane(float2 - @float));
				@float = float2;
			}
			return num + math.length(movementPlane.ToPlane(@float - endOfPart));
		}

		// Token: 0x04000732 RID: 1842
		private Funnel.PathPart[] parts;

		// Token: 0x04000733 RID: 1843
		private CircularBuffer<GraphNode> nodes;

		// Token: 0x04000734 RID: 1844
		private CircularBuffer<int> nodeHashes;

		// Token: 0x04000735 RID: 1845
		private CircularBuffer<byte> portalIsNotInnerCorner;

		// Token: 0x04000736 RID: 1846
		private Funnel.FunnelState funnelState;

		// Token: 0x04000737 RID: 1847
		private Vector3 unclampedEndPoint;

		// Token: 0x04000738 RID: 1848
		private Vector3 unclampedStartPoint;

		// Token: 0x04000739 RID: 1849
		private GraphNode startNodeInternal;

		// Token: 0x0400073A RID: 1850
		private TraversalConstraint traversalConstraint;

		// Token: 0x0400073B RID: 1851
		private TraversalCosts traversalCosts;

		// Token: 0x0400073C RID: 1852
		private int firstPartIndex;

		// Token: 0x0400073D RID: 1853
		private bool startIsUpToDate;

		// Token: 0x0400073E RID: 1854
		private bool endIsUpToDate;

		// Token: 0x0400073F RID: 1855
		private bool pathEmptyAndUnrepairableBacking;

		// Token: 0x04000740 RID: 1856
		private bool firstPartContainsDestroyedNodes;

		// Token: 0x04000741 RID: 1857
		public PathTracer.PartGraphType partGraphType;

		// Token: 0x04000743 RID: 1859
		private static readonly ProfilerMarker MarkerContains = new ProfilerMarker("ContainsNode");

		// Token: 0x04000744 RID: 1860
		private static readonly ProfilerMarker MarkerClosest = new ProfilerMarker("ClosestPointOnNode");

		// Token: 0x04000745 RID: 1861
		private static readonly ProfilerMarker MarkerGetNearest = new ProfilerMarker("GetNearest");

		// Token: 0x04000746 RID: 1862
		private static readonly ProfilerMarker MarkerSetFunnelState = new ProfilerMarker("SetFunnelState");

		// Token: 0x04000747 RID: 1863
		private static readonly ProfilerMarker MarkerCalculatePortals = new ProfilerMarker("CalculatePortals");

		// Token: 0x04000748 RID: 1864
		private const int NODES_TO_CHECK_FOR_DESTRUCTION = 5;

		// Token: 0x04000749 RID: 1865
		private static readonly PathTracer.QueueItem[][] TempQueues = new PathTracer.QueueItem[JobsUtility.ThreadIndexCount][];

		// Token: 0x0400074A RID: 1866
		private static readonly List<GraphNode>[] TempConnectionLists = new List<GraphNode>[JobsUtility.ThreadIndexCount];

		// Token: 0x0400074B RID: 1867
		[ThreadStatic]
		private static List<float3> tmpLeftPortals;

		// Token: 0x0400074C RID: 1868
		[ThreadStatic]
		private static List<float3> tmpRightPortals;

		// Token: 0x0400074D RID: 1869
		[ThreadStatic]
		private static List<GraphNode> scratchList;

		// Token: 0x0400074E RID: 1870
		private static int[] SplittingCoefficients = new int[]
		{
			0, 1, 1, 2, 1, 4, 3, 4, 1, 8,
			3, 8, 5, 8, 7, 8
		};

		// Token: 0x0400074F RID: 1871
		private static readonly ProfilerMarker MarkerSimplify = new ProfilerMarker("Simplify");

		// Token: 0x0200015B RID: 347
		public enum PartGraphType : byte
		{
			// Token: 0x04000751 RID: 1873
			Navmesh,
			// Token: 0x04000752 RID: 1874
			Grid,
			// Token: 0x04000753 RID: 1875
			OffMeshLink
		}

		// Token: 0x0200015C RID: 348
		public enum RepairQuality
		{
			// Token: 0x04000755 RID: 1877
			Low,
			// Token: 0x04000756 RID: 1878
			High
		}

		// Token: 0x0200015D RID: 349
		private struct QueueItem
		{
			// Token: 0x04000757 RID: 1879
			public GraphNode node;

			// Token: 0x04000758 RID: 1880
			public int parent;

			// Token: 0x04000759 RID: 1881
			public float distance;
		}

		// Token: 0x0200015F RID: 351
		// (Invoke) Token: 0x06000B51 RID: 2897
		internal delegate bool ContainsAndProject_00000A85$PostfixBurstDelegate(ref Int3 a, ref Int3 b, ref Int3 c, ref Vector3 p, float height, ref NativeMovementPlane movementPlane, out Vector3 projected);

		// Token: 0x02000160 RID: 352
		internal static class ContainsAndProject_00000A85$BurstDirectCall
		{
			// Token: 0x06000B54 RID: 2900 RVA: 0x0003EF9C File Offset: 0x0003D19C
			[BurstDiscard]
			private static void GetFunctionPointerDiscard(ref IntPtr A_0)
			{
				if (PathTracer.ContainsAndProject_00000A85$BurstDirectCall.Pointer == 0)
				{
					PathTracer.ContainsAndProject_00000A85$BurstDirectCall.Pointer = BurstCompiler.GetILPPMethodFunctionPointer2(PathTracer.ContainsAndProject_00000A85$BurstDirectCall.DeferredCompilation, methodof(PathTracer.ContainsAndProject$BurstManaged(ref Int3, ref Int3, ref Int3, ref Vector3, float, ref NativeMovementPlane, ref Vector3)).MethodHandle, typeof(PathTracer.ContainsAndProject_00000A85$PostfixBurstDelegate).TypeHandle);
				}
				A_0 = PathTracer.ContainsAndProject_00000A85$BurstDirectCall.Pointer;
			}

			// Token: 0x06000B55 RID: 2901 RVA: 0x0003EFC8 File Offset: 0x0003D1C8
			private static IntPtr GetFunctionPointer()
			{
				IntPtr intPtr = (IntPtr)0;
				PathTracer.ContainsAndProject_00000A85$BurstDirectCall.GetFunctionPointerDiscard(ref intPtr);
				return intPtr;
			}

			// Token: 0x06000B56 RID: 2902 RVA: 0x0003EFE0 File Offset: 0x0003D1E0
			public static void Constructor()
			{
				PathTracer.ContainsAndProject_00000A85$BurstDirectCall.DeferredCompilation = BurstCompiler.CompileILPPMethod2(methodof(PathTracer.ContainsAndProject(ref Int3, ref Int3, ref Int3, ref Vector3, float, ref NativeMovementPlane, ref Vector3)).MethodHandle);
			}

			// Token: 0x06000B57 RID: 2903 RVA: 0x00003788 File Offset: 0x00001988
			public static void Initialize()
			{
			}

			// Token: 0x06000B58 RID: 2904 RVA: 0x0003EFF1 File Offset: 0x0003D1F1
			// Note: this type is marked as 'beforefieldinit'.
			static ContainsAndProject_00000A85$BurstDirectCall()
			{
				PathTracer.ContainsAndProject_00000A85$BurstDirectCall.Constructor();
			}

			// Token: 0x06000B59 RID: 2905 RVA: 0x0003EFF8 File Offset: 0x0003D1F8
			public static bool Invoke(ref Int3 a, ref Int3 b, ref Int3 c, ref Vector3 p, float height, ref NativeMovementPlane movementPlane, out Vector3 projected)
			{
				if (BurstCompiler.IsEnabled)
				{
					IntPtr functionPointer = PathTracer.ContainsAndProject_00000A85$BurstDirectCall.GetFunctionPointer();
					if (functionPointer != 0)
					{
						return calli(System.Boolean(Pathfinding.Int3&,Pathfinding.Int3&,Pathfinding.Int3&,UnityEngine.Vector3&,System.Single,Pathfinding.Util.NativeMovementPlane&,UnityEngine.Vector3&), ref a, ref b, ref c, ref p, height, ref movementPlane, ref projected, functionPointer);
					}
				}
				return PathTracer.ContainsAndProject$BurstManaged(ref a, ref b, ref c, ref p, height, ref movementPlane, out projected);
			}

			// Token: 0x0400075C RID: 1884
			private static IntPtr Pointer;

			// Token: 0x0400075D RID: 1885
			private static IntPtr DeferredCompilation;
		}

		// Token: 0x02000161 RID: 353
		// (Invoke) Token: 0x06000B5B RID: 2907
		internal delegate float EstimateRemainingPath_00000A98$PostfixBurstDelegate(ref Funnel.FunnelState funnelState, ref Funnel.PathPart part, int maxCorners, ref NativeMovementPlane movementPlane);

		// Token: 0x02000162 RID: 354
		internal static class EstimateRemainingPath_00000A98$BurstDirectCall
		{
			// Token: 0x06000B5E RID: 2910 RVA: 0x0003F03B File Offset: 0x0003D23B
			[BurstDiscard]
			private static void GetFunctionPointerDiscard(ref IntPtr A_0)
			{
				if (PathTracer.EstimateRemainingPath_00000A98$BurstDirectCall.Pointer == 0)
				{
					PathTracer.EstimateRemainingPath_00000A98$BurstDirectCall.Pointer = BurstCompiler.GetILPPMethodFunctionPointer2(PathTracer.EstimateRemainingPath_00000A98$BurstDirectCall.DeferredCompilation, methodof(PathTracer.EstimateRemainingPath$BurstManaged(ref Funnel.FunnelState, ref Funnel.PathPart, int, ref NativeMovementPlane)).MethodHandle, typeof(PathTracer.EstimateRemainingPath_00000A98$PostfixBurstDelegate).TypeHandle);
				}
				A_0 = PathTracer.EstimateRemainingPath_00000A98$BurstDirectCall.Pointer;
			}

			// Token: 0x06000B5F RID: 2911 RVA: 0x0003F068 File Offset: 0x0003D268
			private static IntPtr GetFunctionPointer()
			{
				IntPtr intPtr = (IntPtr)0;
				PathTracer.EstimateRemainingPath_00000A98$BurstDirectCall.GetFunctionPointerDiscard(ref intPtr);
				return intPtr;
			}

			// Token: 0x06000B60 RID: 2912 RVA: 0x0003F080 File Offset: 0x0003D280
			public static void Constructor()
			{
				PathTracer.EstimateRemainingPath_00000A98$BurstDirectCall.DeferredCompilation = BurstCompiler.CompileILPPMethod2(methodof(PathTracer.EstimateRemainingPath(ref Funnel.FunnelState, ref Funnel.PathPart, int, ref NativeMovementPlane)).MethodHandle);
			}

			// Token: 0x06000B61 RID: 2913 RVA: 0x00003788 File Offset: 0x00001988
			public static void Initialize()
			{
			}

			// Token: 0x06000B62 RID: 2914 RVA: 0x0003F091 File Offset: 0x0003D291
			// Note: this type is marked as 'beforefieldinit'.
			static EstimateRemainingPath_00000A98$BurstDirectCall()
			{
				PathTracer.EstimateRemainingPath_00000A98$BurstDirectCall.Constructor();
			}

			// Token: 0x06000B63 RID: 2915 RVA: 0x0003F098 File Offset: 0x0003D298
			public static float Invoke(ref Funnel.FunnelState funnelState, ref Funnel.PathPart part, int maxCorners, ref NativeMovementPlane movementPlane)
			{
				if (BurstCompiler.IsEnabled)
				{
					IntPtr functionPointer = PathTracer.EstimateRemainingPath_00000A98$BurstDirectCall.GetFunctionPointer();
					if (functionPointer != 0)
					{
						return calli(System.Single(Pathfinding.Funnel/FunnelState&,Pathfinding.Funnel/PathPart&,System.Int32,Pathfinding.Util.NativeMovementPlane&), ref funnelState, ref part, maxCorners, ref movementPlane, functionPointer);
					}
				}
				return PathTracer.EstimateRemainingPath$BurstManaged(ref funnelState, ref part, maxCorners, ref movementPlane);
			}

			// Token: 0x0400075E RID: 1886
			private static IntPtr Pointer;

			// Token: 0x0400075F RID: 1887
			private static IntPtr DeferredCompilation;
		}

		// Token: 0x02000163 RID: 355
		// (Invoke) Token: 0x06000B65 RID: 2917
		internal delegate float RemainingDistanceLowerBound_00000A9C$PostfixBurstDelegate(in UnsafeSpan<float3> nextCorners, in float3 endOfPart, in NativeMovementPlane movementPlane);

		// Token: 0x02000164 RID: 356
		internal static class RemainingDistanceLowerBound_00000A9C$BurstDirectCall
		{
			// Token: 0x06000B68 RID: 2920 RVA: 0x0003F0CF File Offset: 0x0003D2CF
			[BurstDiscard]
			private static void GetFunctionPointerDiscard(ref IntPtr A_0)
			{
				if (PathTracer.RemainingDistanceLowerBound_00000A9C$BurstDirectCall.Pointer == 0)
				{
					PathTracer.RemainingDistanceLowerBound_00000A9C$BurstDirectCall.Pointer = BurstCompiler.GetILPPMethodFunctionPointer2(PathTracer.RemainingDistanceLowerBound_00000A9C$BurstDirectCall.DeferredCompilation, methodof(PathTracer.RemainingDistanceLowerBound$BurstManaged(ref UnsafeSpan<float3>, ref float3, ref NativeMovementPlane)).MethodHandle, typeof(PathTracer.RemainingDistanceLowerBound_00000A9C$PostfixBurstDelegate).TypeHandle);
				}
				A_0 = PathTracer.RemainingDistanceLowerBound_00000A9C$BurstDirectCall.Pointer;
			}

			// Token: 0x06000B69 RID: 2921 RVA: 0x0003F0FC File Offset: 0x0003D2FC
			private static IntPtr GetFunctionPointer()
			{
				IntPtr intPtr = (IntPtr)0;
				PathTracer.RemainingDistanceLowerBound_00000A9C$BurstDirectCall.GetFunctionPointerDiscard(ref intPtr);
				return intPtr;
			}

			// Token: 0x06000B6A RID: 2922 RVA: 0x0003F114 File Offset: 0x0003D314
			public static void Constructor()
			{
				PathTracer.RemainingDistanceLowerBound_00000A9C$BurstDirectCall.DeferredCompilation = BurstCompiler.CompileILPPMethod2(methodof(PathTracer.RemainingDistanceLowerBound(ref UnsafeSpan<float3>, ref float3, ref NativeMovementPlane)).MethodHandle);
			}

			// Token: 0x06000B6B RID: 2923 RVA: 0x00003788 File Offset: 0x00001988
			public static void Initialize()
			{
			}

			// Token: 0x06000B6C RID: 2924 RVA: 0x0003F125 File Offset: 0x0003D325
			// Note: this type is marked as 'beforefieldinit'.
			static RemainingDistanceLowerBound_00000A9C$BurstDirectCall()
			{
				PathTracer.RemainingDistanceLowerBound_00000A9C$BurstDirectCall.Constructor();
			}

			// Token: 0x06000B6D RID: 2925 RVA: 0x0003F12C File Offset: 0x0003D32C
			public static float Invoke(in UnsafeSpan<float3> nextCorners, in float3 endOfPart, in NativeMovementPlane movementPlane)
			{
				if (BurstCompiler.IsEnabled)
				{
					IntPtr functionPointer = PathTracer.RemainingDistanceLowerBound_00000A9C$BurstDirectCall.GetFunctionPointer();
					if (functionPointer != 0)
					{
						return calli(System.Single(Pathfinding.Collections.UnsafeSpan`1<Unity.Mathematics.float3>&,Unity.Mathematics.float3&,Pathfinding.Util.NativeMovementPlane&), ref nextCorners, ref endOfPart, ref movementPlane, functionPointer);
					}
				}
				return PathTracer.RemainingDistanceLowerBound$BurstManaged(in nextCorners, in endOfPart, in movementPlane);
			}

			// Token: 0x04000760 RID: 1888
			private static IntPtr Pointer;

			// Token: 0x04000761 RID: 1889
			private static IntPtr DeferredCompilation;
		}
	}
}
