using System;
using System.Runtime.CompilerServices;
using Pathfinding.Collections;
using Pathfinding.Serialization;
using Pathfinding.Util;
using Unity.Burst;
using Unity.Mathematics;
using Unity.Profiling;
using UnityEngine;

namespace Pathfinding
{
	// Token: 0x020000F0 RID: 240
	[BurstCompile]
	public sealed class TriangleMeshNode : MeshNode
	{
		// Token: 0x06000882 RID: 2178 RVA: 0x0002C051 File Offset: 0x0002A251
		public TriangleMeshNode()
		{
			base.HierarchicalNodeIndex = 0;
			base.NodeIndex = 268435454U;
		}

		// Token: 0x06000883 RID: 2179 RVA: 0x0002C06B File Offset: 0x0002A26B
		public TriangleMeshNode(AstarPath astar)
		{
			astar.InitializeNode(this);
		}

		// Token: 0x1700018D RID: 397
		// (get) Token: 0x06000884 RID: 2180 RVA: 0x0002C07A File Offset: 0x0002A27A
		internal override int PathNodeVariants
		{
			get
			{
				return 3;
			}
		}

		// Token: 0x06000885 RID: 2181 RVA: 0x0002C07D File Offset: 0x0002A27D
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static INavmeshHolder GetNavmeshHolder(uint graphIndex)
		{
			return TriangleMeshNode._navmeshHolders[(int)graphIndex];
		}

		// Token: 0x1700018E RID: 398
		// (get) Token: 0x06000886 RID: 2182 RVA: 0x0002C086 File Offset: 0x0002A286
		public int TileIndex
		{
			get
			{
				return (this.v0 >> 12) & 524287;
			}
		}

		// Token: 0x06000887 RID: 2183 RVA: 0x0002C098 File Offset: 0x0002A298
		public static void SetNavmeshHolder(int graphIndex, INavmeshHolder graph)
		{
			object obj = TriangleMeshNode.lockObject;
			lock (obj)
			{
				if (graphIndex >= TriangleMeshNode._navmeshHolders.Length)
				{
					INavmeshHolder[] array = new INavmeshHolder[graphIndex + 1];
					TriangleMeshNode._navmeshHolders.CopyTo(array, 0);
					TriangleMeshNode._navmeshHolders = array;
				}
				TriangleMeshNode._navmeshHolders[graphIndex] = graph;
			}
		}

		// Token: 0x06000888 RID: 2184 RVA: 0x0002C100 File Offset: 0x0002A300
		public static void ClearNavmeshHolder(int graphIndex, INavmeshHolder graph)
		{
			object obj = TriangleMeshNode.lockObject;
			lock (obj)
			{
				if (graphIndex < TriangleMeshNode._navmeshHolders.Length && TriangleMeshNode._navmeshHolders[graphIndex] == graph)
				{
					TriangleMeshNode._navmeshHolders[graphIndex] = null;
				}
			}
		}

		// Token: 0x06000889 RID: 2185 RVA: 0x0002C158 File Offset: 0x0002A358
		public void UpdatePositionFromVertices()
		{
			Int3 @int;
			Int3 int2;
			Int3 int3;
			this.GetVertices(out @int, out int2, out int3);
			this.position = (@int + int2 + int3) * 0.333333f;
		}

		// Token: 0x0600088A RID: 2186 RVA: 0x0002C18E File Offset: 0x0002A38E
		[IgnoredByDeepProfiler]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int GetVertexIndex(int i)
		{
			if (i == 0)
			{
				return this.v0;
			}
			if (i != 1)
			{
				return this.v2;
			}
			return this.v1;
		}

		// Token: 0x0600088B RID: 2187 RVA: 0x0002C1AB File Offset: 0x0002A3AB
		public int GetVertexArrayIndex(int i)
		{
			return TriangleMeshNode.GetNavmeshHolder(base.GraphIndex).GetVertexArrayIndex((i == 0) ? this.v0 : ((i == 1) ? this.v1 : this.v2));
		}

		// Token: 0x0600088C RID: 2188 RVA: 0x0002C1DC File Offset: 0x0002A3DC
		public void GetVertices(out Int3 v0, out Int3 v1, out Int3 v2)
		{
			INavmeshHolder navmeshHolder = TriangleMeshNode.GetNavmeshHolder(base.GraphIndex);
			v0 = navmeshHolder.GetVertex(this.v0);
			v1 = navmeshHolder.GetVertex(this.v1);
			v2 = navmeshHolder.GetVertex(this.v2);
		}

		// Token: 0x0600088D RID: 2189 RVA: 0x0002C22C File Offset: 0x0002A42C
		public void GetVerticesInGraphSpace(out Int3 v0, out Int3 v1, out Int3 v2)
		{
			INavmeshHolder navmeshHolder = TriangleMeshNode.GetNavmeshHolder(base.GraphIndex);
			v0 = navmeshHolder.GetVertexInGraphSpace(this.v0);
			v1 = navmeshHolder.GetVertexInGraphSpace(this.v1);
			v2 = navmeshHolder.GetVertexInGraphSpace(this.v2);
		}

		// Token: 0x0600088E RID: 2190 RVA: 0x0002C27B File Offset: 0x0002A47B
		[IgnoredByDeepProfiler]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public override Int3 GetVertex(int i)
		{
			return TriangleMeshNode.GetNavmeshHolder(base.GraphIndex).GetVertex(this.GetVertexIndex(i));
		}

		// Token: 0x0600088F RID: 2191 RVA: 0x0002C294 File Offset: 0x0002A494
		public Int3 GetVertexInGraphSpace(int i)
		{
			return TriangleMeshNode.GetNavmeshHolder(base.GraphIndex).GetVertexInGraphSpace(this.GetVertexIndex(i));
		}

		// Token: 0x06000890 RID: 2192 RVA: 0x0002C07A File Offset: 0x0002A27A
		public override int GetVertexCount()
		{
			return 3;
		}

		// Token: 0x06000891 RID: 2193 RVA: 0x0002C2B0 File Offset: 0x0002A4B0
		public Vector3 ProjectOnSurface(Vector3 point)
		{
			Int3 @int;
			Int3 int2;
			Int3 int3;
			this.GetVertices(out @int, out int2, out int3);
			Vector3 vector = (Vector3)@int;
			Vector3 vector2 = (Vector3)int2;
			Vector3 vector3 = (Vector3)int3;
			Vector3 normalized = Vector3.Cross(vector2 - vector, vector3 - vector).normalized;
			return point - normalized * Vector3.Dot(normalized, point - vector);
		}

		// Token: 0x06000892 RID: 2194 RVA: 0x0002C318 File Offset: 0x0002A518
		public override Vector3 ClosestPointOnNode(Vector3 p)
		{
			Int3 @int;
			Int3 int2;
			Int3 int3;
			this.GetVertices(out @int, out int2, out int3);
			return Polygon.ClosestPointOnTriangle((Vector3)@int, (Vector3)int2, (Vector3)int3, p);
		}

		// Token: 0x06000893 RID: 2195 RVA: 0x0002C364 File Offset: 0x0002A564
		internal Int3 ClosestPointOnNodeXZInGraphSpace(Vector3 p)
		{
			Int3 @int;
			Int3 int2;
			Int3 int3;
			this.GetVerticesInGraphSpace(out @int, out int2, out int3);
			p = TriangleMeshNode.GetNavmeshHolder(base.GraphIndex).transform.InverseTransform(p);
			Int3 int4 = (Int3)Polygon.ClosestPointOnTriangleXZ((Vector3)@int, (Vector3)int2, (Vector3)int3, p);
			if (this.ContainsPointInGraphSpace(int4))
			{
				return int4;
			}
			for (int i = -1; i <= 1; i++)
			{
				for (int j = -1; j <= 1; j++)
				{
					if (i != 0 || j != 0)
					{
						Int3 int5 = new Int3(int4.x + i, int4.y, int4.z + j);
						if (this.ContainsPointInGraphSpace(int5))
						{
							return int5;
						}
					}
				}
			}
			long sqrMagnitudeLong = (@int - int4).sqrMagnitudeLong;
			long sqrMagnitudeLong2 = (int2 - int4).sqrMagnitudeLong;
			long sqrMagnitudeLong3 = (int3 - int4).sqrMagnitudeLong;
			if (sqrMagnitudeLong >= sqrMagnitudeLong2)
			{
				if (sqrMagnitudeLong2 >= sqrMagnitudeLong3)
				{
					return int3;
				}
				return int2;
			}
			else
			{
				if (sqrMagnitudeLong >= sqrMagnitudeLong3)
				{
					return int3;
				}
				return @int;
			}
		}

		// Token: 0x06000894 RID: 2196 RVA: 0x0002C464 File Offset: 0x0002A664
		public override Vector3 ClosestPointOnNodeXZ(Vector3 p)
		{
			Int3 @int;
			Int3 int2;
			Int3 int3;
			this.GetVertices(out @int, out int2, out int3);
			return Polygon.ClosestPointOnTriangleXZ((Vector3)@int, (Vector3)int2, (Vector3)int3, p);
		}

		// Token: 0x06000895 RID: 2197 RVA: 0x0002C495 File Offset: 0x0002A695
		public override bool ContainsPoint(Vector3 p)
		{
			return this.ContainsPointInGraphSpace((Int3)TriangleMeshNode.GetNavmeshHolder(base.GraphIndex).transform.InverseTransform(p));
		}

		// Token: 0x06000896 RID: 2198 RVA: 0x0002C4B8 File Offset: 0x0002A6B8
		public bool ContainsPoint(Vector3 p, NativeMovementPlane movementPlane)
		{
			Int3 @int;
			Int3 int2;
			Int3 int3;
			this.GetVertices(out @int, out int2, out int3);
			int3 int4 = (int3)@int;
			int3 int5 = (int3)int2;
			int3 int6 = (int3)int3;
			int3 int7 = (int3)((Int3)p);
			return Polygon.ContainsPoint(ref int4, ref int5, ref int6, ref int7, ref movementPlane);
		}

		// Token: 0x06000897 RID: 2199 RVA: 0x0002C504 File Offset: 0x0002A704
		public override bool ContainsPointInGraphSpace(Int3 p)
		{
			Int3 @int;
			Int3 int2;
			Int3 int3;
			this.GetVerticesInGraphSpace(out @int, out int2, out int3);
			return (long)(int2.x - @int.x) * (long)(p.z - @int.z) - (long)(p.x - @int.x) * (long)(int2.z - @int.z) <= 0L && (long)(int3.x - int2.x) * (long)(p.z - int2.z) - (long)(p.x - int2.x) * (long)(int3.z - int2.z) <= 0L && (long)(@int.x - int3.x) * (long)(p.z - int3.z) - (long)(p.x - int3.x) * (long)(@int.z - int3.z) <= 0L;
		}

		// Token: 0x06000898 RID: 2200 RVA: 0x0002C5E4 File Offset: 0x0002A7E4
		public override Int3 DecodeVariantPosition(uint pathNodeIndex, uint fractionAlongEdge)
		{
			int num = (int)(pathNodeIndex - base.NodeIndex);
			Int3 vertex = this.GetVertex(num);
			Int3 vertex2 = this.GetVertex((num + 1) % 3);
			Int3 @int;
			TriangleMeshNode.InterpolateEdge(ref vertex, ref vertex2, fractionAlongEdge, out @int);
			return @int;
		}

		// Token: 0x06000899 RID: 2201 RVA: 0x0002C61B File Offset: 0x0002A81B
		[BurstCompile(FloatMode = FloatMode.Fast)]
		private static void InterpolateEdge(ref Int3 p1, ref Int3 p2, uint fractionAlongEdge, out Int3 pos)
		{
			TriangleMeshNode.InterpolateEdge_0000084C$BurstDirectCall.Invoke(ref p1, ref p2, fractionAlongEdge, out pos);
		}

		// Token: 0x0600089A RID: 2202 RVA: 0x0002C626 File Offset: 0x0002A826
		public override void OpenAtPoint(ref Path.SearchContext ctx, uint pathNodeIndex, Int3 point, uint gScore)
		{
			this.OpenAtPoint(ref ctx, pathNodeIndex, point, -1, gScore);
		}

		// Token: 0x0600089B RID: 2203 RVA: 0x0002C634 File Offset: 0x0002A834
		public override void Open(ref Path.SearchContext ctx, uint pathNodeIndex, uint gScore)
		{
			int num = (int)(pathNodeIndex - base.NodeIndex);
			this.OpenAtPoint(ref ctx, pathNodeIndex, this.DecodeVariantPosition(pathNodeIndex, ctx.pathHandler.pathNodes[pathNodeIndex].fractionAlongEdge), num, gScore);
		}

		// Token: 0x0600089C RID: 2204 RVA: 0x0002C674 File Offset: 0x0002A874
		private unsafe void OpenAtPoint(ref Path.SearchContext ctx, uint pathNodeIndex, Int3 pos, int edge, uint gScore)
		{
			PathHandler pathHandler = ctx.pathHandler;
			PathNode pathNode = *pathHandler.pathNodes[pathNodeIndex];
			float traversalCostMultiplier = ctx.traversalCosts.GetTraversalCostMultiplier(this);
			if (pathNode.flag1)
			{
				ctx.OpenCandidateConnectionsToEndNode(pos, pathNodeIndex, base.NodeIndex, gScore, traversalCostMultiplier);
			}
			int num = 0;
			bool flag = pathNode.parentIndex >= base.NodeIndex && pathNode.parentIndex < base.NodeIndex + 3U;
			if (this.connections != null)
			{
				for (int i = this.connections.Length - 1; i >= 0; i--)
				{
					Connection connection = this.connections[i];
					if (connection.isOutgoing)
					{
						GraphNode node = connection.node;
						if (connection.isEdgeShared)
						{
							int adjacentShapeEdge = connection.adjacentShapeEdge;
							uint num2 = node.NodeIndex + (uint)adjacentShapeEdge;
							if (num2 != pathNode.parentIndex)
							{
								if (connection.shapeEdge == edge)
								{
									if (ctx.traversalConstraint.CanTraverse(this, node))
									{
										TriangleMeshNode triangleMeshNode = node as TriangleMeshNode;
										if (ctx.ShouldConsiderPathNode(num2))
										{
											uint num3 = gScore + ctx.traversalCosts.GetConnectionCost(this, node);
											if (connection.edgesAreIdentical)
											{
												ctx.SkipOverNode(ref ctx, num2, pathNodeIndex, PathNode.ReverseFractionAlongEdge(pathNode.fractionAlongEdge), uint.MaxValue, num3);
												triangleMeshNode.OpenAtPoint(ref ctx, num2, pos, adjacentShapeEdge, num3);
											}
											else
											{
												this.OpenSingleEdge(ref ctx, pathNodeIndex, triangleMeshNode, adjacentShapeEdge, pos, num3, traversalCostMultiplier);
											}
										}
									}
								}
								else if (!flag && (num & (1 << connection.shapeEdge)) == 0)
								{
									num |= 1 << connection.shapeEdge;
									this.OpenSingleEdge(ref ctx, pathNodeIndex, this, connection.shapeEdge, pos, gScore, traversalCostMultiplier);
								}
							}
						}
						else if (!flag && ctx.traversalConstraint.CanTraverse(this, node) && ctx.ShouldConsiderPathNode(node.NodeIndex))
						{
							uint num4 = (uint)Math.Round((double)((node.position - pos).magnitude * traversalCostMultiplier));
							if (edge != -1)
							{
								ctx.OpenCandidateConnection(pathNodeIndex, node.NodeIndex, gScore + num4, 0U, node.position);
							}
							else
							{
								uint num5 = pathHandler.AddTemporaryNode(new TemporaryNode
								{
									associatedNode = base.NodeIndex,
									position = pos,
									targetIndex = 0,
									type = TemporaryNodeType.Ignore
								});
								ref PathNode ptr = ref pathHandler.pathNodes[num5];
								ptr.pathID = ctx.pathID;
								ptr.parentIndex = pathNodeIndex;
								ctx.OpenCandidateConnection(num5, node.NodeIndex, gScore + num4, 0U, node.position);
							}
						}
					}
				}
			}
			if (edge == -1)
			{
				if (pathHandler.pathNodes[base.NodeIndex].flag1)
				{
					ctx.OpenCandidateConnectionsToEndNode(pos, pathNodeIndex, base.NodeIndex, gScore, traversalCostMultiplier);
				}
				if (num == 0)
				{
					this.OpenSingleEdge(ref ctx, pathNodeIndex, this, 0, pos, gScore, traversalCostMultiplier);
				}
			}
		}

		// Token: 0x0600089D RID: 2205 RVA: 0x0002C95C File Offset: 0x0002AB5C
		private void OpenSingleEdge(ref Path.SearchContext ctx, uint pathNodeIndex, TriangleMeshNode other, int sharedEdgeOnOtherNode, Int3 pos, uint gScore, float traversalCostFactor)
		{
			uint num = other.NodeIndex + (uint)sharedEdgeOnOtherNode;
			if (!ctx.ShouldConsiderPathNode(num))
			{
				return;
			}
			Int3 vertex = other.GetVertex(sharedEdgeOnOtherNode);
			Int3 vertex2 = other.GetVertex((sharedEdgeOnOtherNode + 1) % 3);
			PathHandler pathHandler = ctx.pathHandler;
			TriangleMeshNode.OpenSingleEdgeBurst(ref vertex, ref vertex2, ref pos, ctx.pathID, pathNodeIndex, num, other.NodeIndex, gScore, traversalCostFactor, ref pathHandler.pathNodes, ref pathHandler.heap, ref ctx.heuristicObjective);
		}

		// Token: 0x0600089E RID: 2206 RVA: 0x0002C9CC File Offset: 0x0002ABCC
		[BurstCompile]
		private static void OpenSingleEdgeBurst(ref Int3 s1, ref Int3 s2, ref Int3 pos, ushort pathID, uint pathNodeIndex, uint candidatePathNodeIndex, uint candidateNodeIndex, uint candidateG, float traversalCostFactor, ref UnsafeSpan<PathNode> pathNodes, ref BinaryHeap heap, ref HeuristicObjective heuristicObjective)
		{
			TriangleMeshNode.OpenSingleEdgeBurst_00000851$BurstDirectCall.Invoke(ref s1, ref s2, ref pos, pathID, pathNodeIndex, candidatePathNodeIndex, candidateNodeIndex, candidateG, traversalCostFactor, ref pathNodes, ref heap, ref heuristicObjective);
		}

		// Token: 0x0600089F RID: 2207 RVA: 0x0002C9F2 File Offset: 0x0002ABF2
		[BurstCompile]
		private static void CalculateBestEdgePosition(ref Int3 s1, ref Int3 s2, ref Int3 pos, out int3 closestPointAlongEdge, out uint quantizedFractionAlongEdge, out float cost)
		{
			TriangleMeshNode.CalculateBestEdgePosition_00000852$BurstDirectCall.Invoke(ref s1, ref s2, ref pos, out closestPointAlongEdge, out quantizedFractionAlongEdge, out cost);
		}

		// Token: 0x060008A0 RID: 2208 RVA: 0x0002CA04 File Offset: 0x0002AC04
		public int SharedEdge(GraphNode other)
		{
			int num = -1;
			if (this.connections != null)
			{
				for (int i = 0; i < this.connections.Length; i++)
				{
					if (this.connections[i].node == other && this.connections[i].isEdgeShared)
					{
						num = this.connections[i].shapeEdge;
					}
				}
			}
			return num;
		}

		// Token: 0x060008A1 RID: 2209 RVA: 0x0002CA68 File Offset: 0x0002AC68
		public override bool GetPortal(GraphNode toNode, out Vector3 left, out Vector3 right)
		{
			int num;
			int num2;
			return this.GetPortal(toNode, out left, out right, out num, out num2);
		}

		// Token: 0x060008A2 RID: 2210 RVA: 0x0002CA84 File Offset: 0x0002AC84
		public bool GetPortalInGraphSpace(TriangleMeshNode toNode, out Int3 a, out Int3 b, out int aIndex, out int bIndex)
		{
			aIndex = -1;
			bIndex = -1;
			a = Int3.zero;
			b = Int3.zero;
			if (toNode.GraphIndex != base.GraphIndex)
			{
				return false;
			}
			int num = -1;
			int num2 = -1;
			if (this.connections != null)
			{
				for (int i = 0; i < this.connections.Length; i++)
				{
					if (this.connections[i].node == toNode && this.connections[i].isEdgeShared)
					{
						num = this.connections[i].shapeEdge;
						num2 = this.connections[i].adjacentShapeEdge;
					}
				}
			}
			if (num == -1)
			{
				return false;
			}
			aIndex = num;
			bIndex = (num + 1) % 3;
			INavmeshHolder navmeshHolder = TriangleMeshNode.GetNavmeshHolder(base.GraphIndex);
			a = navmeshHolder.GetVertexInGraphSpace(this.GetVertexIndex(aIndex));
			b = navmeshHolder.GetVertexInGraphSpace(this.GetVertexIndex(bIndex));
			int tileIndex = this.TileIndex;
			int tileIndex2 = toNode.TileIndex;
			if (tileIndex != tileIndex2)
			{
				Int3 vertexInGraphSpace = toNode.GetVertexInGraphSpace(num2);
				Int3 vertexInGraphSpace2 = toNode.GetVertexInGraphSpace((num2 + 1) % 3);
				int num3;
				int num4;
				navmeshHolder.GetTileCoordinates(tileIndex, out num3, out num4);
				int num5;
				int num6;
				navmeshHolder.GetTileCoordinates(tileIndex2, out num5, out num6);
				int num7 = ((num3 == num5) ? 0 : 2);
				int num8 = Mathf.Min(vertexInGraphSpace[num7], vertexInGraphSpace2[num7]);
				int num9 = Mathf.Max(vertexInGraphSpace[num7], vertexInGraphSpace2[num7]);
				a[num7] = Mathf.Clamp(a[num7], num8, num9);
				b[num7] = Mathf.Clamp(b[num7], num8, num9);
			}
			return true;
		}

		// Token: 0x060008A3 RID: 2211 RVA: 0x0002CC30 File Offset: 0x0002AE30
		public bool GetPortal(GraphNode toNode, out Vector3 left, out Vector3 right, out int aIndex, out int bIndex)
		{
			TriangleMeshNode triangleMeshNode = toNode as TriangleMeshNode;
			Int3 @int;
			Int3 int2;
			if (triangleMeshNode != null && this.GetPortalInGraphSpace(triangleMeshNode, out @int, out int2, out aIndex, out bIndex))
			{
				INavmeshHolder navmeshHolder = TriangleMeshNode.GetNavmeshHolder(base.GraphIndex);
				left = navmeshHolder.transform.Transform((Vector3)@int);
				right = navmeshHolder.transform.Transform((Vector3)int2);
				return true;
			}
			aIndex = -1;
			bIndex = -1;
			left = Vector3.zero;
			right = Vector3.zero;
			return false;
		}

		// Token: 0x060008A4 RID: 2212 RVA: 0x0002CCB4 File Offset: 0x0002AEB4
		public override float SurfaceArea()
		{
			INavmeshHolder navmeshHolder = TriangleMeshNode.GetNavmeshHolder(base.GraphIndex);
			return (float)Math.Abs(VectorMath.SignedTriangleAreaTimes2XZ(navmeshHolder.GetVertex(this.v0), navmeshHolder.GetVertex(this.v1), navmeshHolder.GetVertex(this.v2))) * 0.5f;
		}

		// Token: 0x060008A5 RID: 2213 RVA: 0x0002CD04 File Offset: 0x0002AF04
		public override Vector3 RandomPointOnSurface()
		{
			float2 @float;
			do
			{
				@float = AstarMath.ThreadSafeRandomFloat2();
			}
			while (@float.x + @float.y > 1f);
			Int3 @int;
			Int3 int2;
			Int3 int3;
			this.GetVertices(out @int, out int2, out int3);
			return (Vector3)(int2 - @int) * @float.x + (Vector3)(int3 - @int) * @float.y + (Vector3)@int;
		}

		// Token: 0x060008A6 RID: 2214 RVA: 0x0002CD75 File Offset: 0x0002AF75
		public override void SerializeNode(GraphSerializationContext ctx)
		{
			base.SerializeNode(ctx);
			ctx.writer.Write(this.v0);
			ctx.writer.Write(this.v1);
			ctx.writer.Write(this.v2);
		}

		// Token: 0x060008A7 RID: 2215 RVA: 0x0002CDB1 File Offset: 0x0002AFB1
		public override void DeserializeNode(GraphSerializationContext ctx)
		{
			base.DeserializeNode(ctx);
			this.v0 = ctx.reader.ReadInt32();
			this.v1 = ctx.reader.ReadInt32();
			this.v2 = ctx.reader.ReadInt32();
		}

		// Token: 0x060008A9 RID: 2217 RVA: 0x0002CE40 File Offset: 0x0002B040
		[BurstCompile(FloatMode = FloatMode.Fast)]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal static void InterpolateEdge$BurstManaged(ref Int3 p1, ref Int3 p2, uint fractionAlongEdge, out Int3 pos)
		{
			int3 @int = (int3)math.lerp((int3)p1, (int3)p2, PathNode.UnQuantizeFractionAlongEdge(fractionAlongEdge));
			pos = new Int3(@int.x, @int.y, @int.z);
		}

		// Token: 0x060008AA RID: 2218 RVA: 0x0002CE9C File Offset: 0x0002B09C
		[BurstCompile]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal static void OpenSingleEdgeBurst$BurstManaged(ref Int3 s1, ref Int3 s2, ref Int3 pos, ushort pathID, uint pathNodeIndex, uint candidatePathNodeIndex, uint candidateNodeIndex, uint candidateG, float traversalCostFactor, ref UnsafeSpan<PathNode> pathNodes, ref BinaryHeap heap, ref HeuristicObjective heuristicObjective)
		{
			int3 @int;
			uint num;
			float num2;
			TriangleMeshNode.CalculateBestEdgePosition(ref s1, ref s2, ref pos, out @int, out num, out num2);
			candidateG += (uint)(num2 * traversalCostFactor);
			Path.SearchContext.OpenCandidateParams openCandidateParams = new Path.SearchContext.OpenCandidateParams
			{
				pathID = pathID,
				parentPathNode = pathNodeIndex,
				targetPathNode = candidatePathNodeIndex,
				targetNodeIndex = candidateNodeIndex,
				targetG = candidateG,
				fractionAlongEdge = num,
				targetNodePosition = @int,
				pathNodes = pathNodes
			};
			Path.SearchContext.OpenCandidateConnectionBurst(ref openCandidateParams, ref heap, ref heuristicObjective);
		}

		// Token: 0x060008AB RID: 2219 RVA: 0x0002CF24 File Offset: 0x0002B124
		[BurstCompile]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal static void CalculateBestEdgePosition$BurstManaged(ref Int3 s1, ref Int3 s2, ref Int3 pos, out int3 closestPointAlongEdge, out uint quantizedFractionAlongEdge, out float cost)
		{
			float3 @float = (int3)s1;
			float3 float2 = (int3)s2;
			int3 @int = (int3)pos;
			float num = math.clamp(VectorMath.ClosestPointOnLineFactor(@float, float2, @int), 0f, 1f);
			quantizedFractionAlongEdge = PathNode.QuantizeFractionAlongEdge(num);
			num = PathNode.UnQuantizeFractionAlongEdge(quantizedFractionAlongEdge);
			float3 float3 = math.lerp(@float, float2, num);
			closestPointAlongEdge = (int3)float3;
			int3 int2 = @int - closestPointAlongEdge;
			cost = new Int3(int2.x, int2.y, int2.z).magnitude;
		}

		// Token: 0x04000542 RID: 1346
		public const bool InaccuratePathSearch = false;

		// Token: 0x04000543 RID: 1347
		public int v0;

		// Token: 0x04000544 RID: 1348
		public int v1;

		// Token: 0x04000545 RID: 1349
		public int v2;

		// Token: 0x04000546 RID: 1350
		private static INavmeshHolder[] _navmeshHolders = new INavmeshHolder[0];

		// Token: 0x04000547 RID: 1351
		private static readonly object lockObject = new object();

		// Token: 0x04000548 RID: 1352
		public static readonly ProfilerMarker MarkerDecode = new ProfilerMarker("Decode");

		// Token: 0x04000549 RID: 1353
		public static readonly ProfilerMarker MarkerGetVertices = new ProfilerMarker("GetVertex");

		// Token: 0x0400054A RID: 1354
		public static readonly ProfilerMarker MarkerClosest = new ProfilerMarker("MarkerClosest");

		// Token: 0x020000F1 RID: 241
		// (Invoke) Token: 0x060008AD RID: 2221
		internal delegate void InterpolateEdge_0000084C$PostfixBurstDelegate(ref Int3 p1, ref Int3 p2, uint fractionAlongEdge, out Int3 pos);

		// Token: 0x020000F2 RID: 242
		internal static class InterpolateEdge_0000084C$BurstDirectCall
		{
			// Token: 0x060008B0 RID: 2224 RVA: 0x0002CFD5 File Offset: 0x0002B1D5
			[BurstDiscard]
			private static void GetFunctionPointerDiscard(ref IntPtr A_0)
			{
				if (TriangleMeshNode.InterpolateEdge_0000084C$BurstDirectCall.Pointer == 0)
				{
					TriangleMeshNode.InterpolateEdge_0000084C$BurstDirectCall.Pointer = BurstCompiler.GetILPPMethodFunctionPointer2(TriangleMeshNode.InterpolateEdge_0000084C$BurstDirectCall.DeferredCompilation, methodof(TriangleMeshNode.InterpolateEdge$BurstManaged(ref Int3, ref Int3, uint, ref Int3)).MethodHandle, typeof(TriangleMeshNode.InterpolateEdge_0000084C$PostfixBurstDelegate).TypeHandle);
				}
				A_0 = TriangleMeshNode.InterpolateEdge_0000084C$BurstDirectCall.Pointer;
			}

			// Token: 0x060008B1 RID: 2225 RVA: 0x0002D004 File Offset: 0x0002B204
			private static IntPtr GetFunctionPointer()
			{
				IntPtr intPtr = (IntPtr)0;
				TriangleMeshNode.InterpolateEdge_0000084C$BurstDirectCall.GetFunctionPointerDiscard(ref intPtr);
				return intPtr;
			}

			// Token: 0x060008B2 RID: 2226 RVA: 0x0002D01C File Offset: 0x0002B21C
			public static void Constructor()
			{
				TriangleMeshNode.InterpolateEdge_0000084C$BurstDirectCall.DeferredCompilation = BurstCompiler.CompileILPPMethod2(methodof(TriangleMeshNode.InterpolateEdge(ref Int3, ref Int3, uint, ref Int3)).MethodHandle);
			}

			// Token: 0x060008B3 RID: 2227 RVA: 0x00003788 File Offset: 0x00001988
			public static void Initialize()
			{
			}

			// Token: 0x060008B4 RID: 2228 RVA: 0x0002D02D File Offset: 0x0002B22D
			// Note: this type is marked as 'beforefieldinit'.
			static InterpolateEdge_0000084C$BurstDirectCall()
			{
				TriangleMeshNode.InterpolateEdge_0000084C$BurstDirectCall.Constructor();
			}

			// Token: 0x060008B5 RID: 2229 RVA: 0x0002D034 File Offset: 0x0002B234
			public static void Invoke(ref Int3 p1, ref Int3 p2, uint fractionAlongEdge, out Int3 pos)
			{
				if (BurstCompiler.IsEnabled)
				{
					IntPtr functionPointer = TriangleMeshNode.InterpolateEdge_0000084C$BurstDirectCall.GetFunctionPointer();
					if (functionPointer != 0)
					{
						calli(System.Void(Pathfinding.Int3&,Pathfinding.Int3&,System.UInt32,Pathfinding.Int3&), ref p1, ref p2, fractionAlongEdge, ref pos, functionPointer);
						return;
					}
				}
				TriangleMeshNode.InterpolateEdge$BurstManaged(ref p1, ref p2, fractionAlongEdge, out pos);
			}

			// Token: 0x0400054B RID: 1355
			private static IntPtr Pointer;

			// Token: 0x0400054C RID: 1356
			private static IntPtr DeferredCompilation;
		}

		// Token: 0x020000F3 RID: 243
		// (Invoke) Token: 0x060008B7 RID: 2231
		internal delegate void OpenSingleEdgeBurst_00000851$PostfixBurstDelegate(ref Int3 s1, ref Int3 s2, ref Int3 pos, ushort pathID, uint pathNodeIndex, uint candidatePathNodeIndex, uint candidateNodeIndex, uint candidateG, float traversalCostFactor, ref UnsafeSpan<PathNode> pathNodes, ref BinaryHeap heap, ref HeuristicObjective heuristicObjective);

		// Token: 0x020000F4 RID: 244
		internal static class OpenSingleEdgeBurst_00000851$BurstDirectCall
		{
			// Token: 0x060008BA RID: 2234 RVA: 0x0002D06B File Offset: 0x0002B26B
			[BurstDiscard]
			private static void GetFunctionPointerDiscard(ref IntPtr A_0)
			{
				if (TriangleMeshNode.OpenSingleEdgeBurst_00000851$BurstDirectCall.Pointer == 0)
				{
					TriangleMeshNode.OpenSingleEdgeBurst_00000851$BurstDirectCall.Pointer = BurstCompiler.GetILPPMethodFunctionPointer2(TriangleMeshNode.OpenSingleEdgeBurst_00000851$BurstDirectCall.DeferredCompilation, methodof(TriangleMeshNode.OpenSingleEdgeBurst$BurstManaged(ref Int3, ref Int3, ref Int3, ushort, uint, uint, uint, uint, float, ref UnsafeSpan<PathNode>, ref BinaryHeap, ref HeuristicObjective)).MethodHandle, typeof(TriangleMeshNode.OpenSingleEdgeBurst_00000851$PostfixBurstDelegate).TypeHandle);
				}
				A_0 = TriangleMeshNode.OpenSingleEdgeBurst_00000851$BurstDirectCall.Pointer;
			}

			// Token: 0x060008BB RID: 2235 RVA: 0x0002D098 File Offset: 0x0002B298
			private static IntPtr GetFunctionPointer()
			{
				IntPtr intPtr = (IntPtr)0;
				TriangleMeshNode.OpenSingleEdgeBurst_00000851$BurstDirectCall.GetFunctionPointerDiscard(ref intPtr);
				return intPtr;
			}

			// Token: 0x060008BC RID: 2236 RVA: 0x0002D0B0 File Offset: 0x0002B2B0
			public static void Constructor()
			{
				TriangleMeshNode.OpenSingleEdgeBurst_00000851$BurstDirectCall.DeferredCompilation = BurstCompiler.CompileILPPMethod2(methodof(TriangleMeshNode.OpenSingleEdgeBurst(ref Int3, ref Int3, ref Int3, ushort, uint, uint, uint, uint, float, ref UnsafeSpan<PathNode>, ref BinaryHeap, ref HeuristicObjective)).MethodHandle);
			}

			// Token: 0x060008BD RID: 2237 RVA: 0x00003788 File Offset: 0x00001988
			public static void Initialize()
			{
			}

			// Token: 0x060008BE RID: 2238 RVA: 0x0002D0C1 File Offset: 0x0002B2C1
			// Note: this type is marked as 'beforefieldinit'.
			static OpenSingleEdgeBurst_00000851$BurstDirectCall()
			{
				TriangleMeshNode.OpenSingleEdgeBurst_00000851$BurstDirectCall.Constructor();
			}

			// Token: 0x060008BF RID: 2239 RVA: 0x0002D0C8 File Offset: 0x0002B2C8
			public static void Invoke(ref Int3 s1, ref Int3 s2, ref Int3 pos, ushort pathID, uint pathNodeIndex, uint candidatePathNodeIndex, uint candidateNodeIndex, uint candidateG, float traversalCostFactor, ref UnsafeSpan<PathNode> pathNodes, ref BinaryHeap heap, ref HeuristicObjective heuristicObjective)
			{
				if (BurstCompiler.IsEnabled)
				{
					IntPtr functionPointer = TriangleMeshNode.OpenSingleEdgeBurst_00000851$BurstDirectCall.GetFunctionPointer();
					if (functionPointer != 0)
					{
						calli(System.Void(Pathfinding.Int3&,Pathfinding.Int3&,Pathfinding.Int3&,System.UInt16,System.UInt32,System.UInt32,System.UInt32,System.UInt32,System.Single,Pathfinding.Collections.UnsafeSpan`1<Pathfinding.PathNode>&,Pathfinding.BinaryHeap&,Pathfinding.HeuristicObjective&), ref s1, ref s2, ref pos, pathID, pathNodeIndex, candidatePathNodeIndex, candidateNodeIndex, candidateG, traversalCostFactor, ref pathNodes, ref heap, ref heuristicObjective, functionPointer);
						return;
					}
				}
				TriangleMeshNode.OpenSingleEdgeBurst$BurstManaged(ref s1, ref s2, ref pos, pathID, pathNodeIndex, candidatePathNodeIndex, candidateNodeIndex, candidateG, traversalCostFactor, ref pathNodes, ref heap, ref heuristicObjective);
			}

			// Token: 0x0400054D RID: 1357
			private static IntPtr Pointer;

			// Token: 0x0400054E RID: 1358
			private static IntPtr DeferredCompilation;
		}

		// Token: 0x020000F5 RID: 245
		// (Invoke) Token: 0x060008C1 RID: 2241
		internal delegate void CalculateBestEdgePosition_00000852$PostfixBurstDelegate(ref Int3 s1, ref Int3 s2, ref Int3 pos, out int3 closestPointAlongEdge, out uint quantizedFractionAlongEdge, out float cost);

		// Token: 0x020000F6 RID: 246
		internal static class CalculateBestEdgePosition_00000852$BurstDirectCall
		{
			// Token: 0x060008C4 RID: 2244 RVA: 0x0002D11F File Offset: 0x0002B31F
			[BurstDiscard]
			private static void GetFunctionPointerDiscard(ref IntPtr A_0)
			{
				if (TriangleMeshNode.CalculateBestEdgePosition_00000852$BurstDirectCall.Pointer == 0)
				{
					TriangleMeshNode.CalculateBestEdgePosition_00000852$BurstDirectCall.Pointer = BurstCompiler.GetILPPMethodFunctionPointer2(TriangleMeshNode.CalculateBestEdgePosition_00000852$BurstDirectCall.DeferredCompilation, methodof(TriangleMeshNode.CalculateBestEdgePosition$BurstManaged(ref Int3, ref Int3, ref Int3, ref int3, ref uint, ref float)).MethodHandle, typeof(TriangleMeshNode.CalculateBestEdgePosition_00000852$PostfixBurstDelegate).TypeHandle);
				}
				A_0 = TriangleMeshNode.CalculateBestEdgePosition_00000852$BurstDirectCall.Pointer;
			}

			// Token: 0x060008C5 RID: 2245 RVA: 0x0002D14C File Offset: 0x0002B34C
			private static IntPtr GetFunctionPointer()
			{
				IntPtr intPtr = (IntPtr)0;
				TriangleMeshNode.CalculateBestEdgePosition_00000852$BurstDirectCall.GetFunctionPointerDiscard(ref intPtr);
				return intPtr;
			}

			// Token: 0x060008C6 RID: 2246 RVA: 0x0002D164 File Offset: 0x0002B364
			public static void Constructor()
			{
				TriangleMeshNode.CalculateBestEdgePosition_00000852$BurstDirectCall.DeferredCompilation = BurstCompiler.CompileILPPMethod2(methodof(TriangleMeshNode.CalculateBestEdgePosition(ref Int3, ref Int3, ref Int3, ref int3, ref uint, ref float)).MethodHandle);
			}

			// Token: 0x060008C7 RID: 2247 RVA: 0x00003788 File Offset: 0x00001988
			public static void Initialize()
			{
			}

			// Token: 0x060008C8 RID: 2248 RVA: 0x0002D175 File Offset: 0x0002B375
			// Note: this type is marked as 'beforefieldinit'.
			static CalculateBestEdgePosition_00000852$BurstDirectCall()
			{
				TriangleMeshNode.CalculateBestEdgePosition_00000852$BurstDirectCall.Constructor();
			}

			// Token: 0x060008C9 RID: 2249 RVA: 0x0002D17C File Offset: 0x0002B37C
			public static void Invoke(ref Int3 s1, ref Int3 s2, ref Int3 pos, out int3 closestPointAlongEdge, out uint quantizedFractionAlongEdge, out float cost)
			{
				if (BurstCompiler.IsEnabled)
				{
					IntPtr functionPointer = TriangleMeshNode.CalculateBestEdgePosition_00000852$BurstDirectCall.GetFunctionPointer();
					if (functionPointer != 0)
					{
						calli(System.Void(Pathfinding.Int3&,Pathfinding.Int3&,Pathfinding.Int3&,Unity.Mathematics.int3&,System.UInt32&,System.Single&), ref s1, ref s2, ref pos, ref closestPointAlongEdge, ref quantizedFractionAlongEdge, ref cost, functionPointer);
						return;
					}
				}
				TriangleMeshNode.CalculateBestEdgePosition$BurstManaged(ref s1, ref s2, ref pos, out closestPointAlongEdge, out quantizedFractionAlongEdge, out cost);
			}

			// Token: 0x0400054F RID: 1359
			private static IntPtr Pointer;

			// Token: 0x04000550 RID: 1360
			private static IntPtr DeferredCompilation;
		}
	}
}
