using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Pathfinding.Collections;
using Pathfinding.Drawing;
using Pathfinding.Graphs.Navmesh;
using Pathfinding.Pooling;
using Pathfinding.Serialization;
using Pathfinding.Util;
using Unity.Burst;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Mathematics;
using UnityEngine;

namespace Pathfinding
{
	// Token: 0x020000E2 RID: 226
	[BurstCompile]
	public abstract class NavmeshBase : NavGraph, INavmeshHolder, ITransformedGraph, IRaycastableGraph
	{
		// Token: 0x1700016C RID: 364
		// (get) Token: 0x06000799 RID: 1945
		public abstract float NavmeshCuttingCharacterRadius { get; }

		// Token: 0x1700016D RID: 365
		// (get) Token: 0x0600079A RID: 1946
		public abstract float TileWorldSizeX { get; }

		// Token: 0x1700016E RID: 366
		// (get) Token: 0x0600079B RID: 1947
		public abstract float TileWorldSizeZ { get; }

		// Token: 0x1700016F RID: 367
		// (get) Token: 0x0600079C RID: 1948
		public abstract float MaxTileConnectionEdgeDistance { get; }

		// Token: 0x17000170 RID: 368
		// (get) Token: 0x0600079D RID: 1949 RVA: 0x00025E24 File Offset: 0x00024024
		GraphTransform ITransformedGraph.transform
		{
			get
			{
				return this.transform;
			}
		}

		// Token: 0x17000171 RID: 369
		// (get) Token: 0x0600079E RID: 1950
		public abstract bool RecalculateNormals { get; }

		// Token: 0x17000172 RID: 370
		// (get) Token: 0x0600079F RID: 1951 RVA: 0x00025E2C File Offset: 0x0002402C
		public override bool isScanned
		{
			get
			{
				return this.tiles != null;
			}
		}

		// Token: 0x060007A0 RID: 1952
		public abstract GraphTransform CalculateTransform();

		// Token: 0x060007A1 RID: 1953 RVA: 0x00025E37 File Offset: 0x00024037
		public NavmeshTile GetTile(int x, int z)
		{
			return this.tiles[x + z * this.tileXCount];
		}

		// Token: 0x060007A2 RID: 1954 RVA: 0x00025E4C File Offset: 0x0002404C
		public Int3 GetVertex(int index)
		{
			int num = (index >> 12) & 524287;
			return this.tiles[num].GetVertex(index);
		}

		// Token: 0x060007A3 RID: 1955 RVA: 0x00025E74 File Offset: 0x00024074
		public Int3 GetVertexInGraphSpace(int index)
		{
			int num = (index >> 12) & 524287;
			return this.tiles[num].GetVertexInGraphSpace(index);
		}

		// Token: 0x060007A4 RID: 1956 RVA: 0x00025E9A File Offset: 0x0002409A
		public static int GetTileIndex(int index)
		{
			return (index >> 12) & 524287;
		}

		// Token: 0x060007A5 RID: 1957 RVA: 0x00025EA6 File Offset: 0x000240A6
		public int GetVertexArrayIndex(int index)
		{
			return index & 4095;
		}

		// Token: 0x060007A6 RID: 1958 RVA: 0x00025EAF File Offset: 0x000240AF
		public void GetTileCoordinates(int tileIndex, out int x, out int z)
		{
			z = tileIndex / this.tileXCount;
			x = tileIndex - z * this.tileXCount;
		}

		// Token: 0x060007A7 RID: 1959 RVA: 0x00025EC8 File Offset: 0x000240C8
		public NavmeshTile[] GetTiles()
		{
			return this.tiles;
		}

		// Token: 0x060007A8 RID: 1960 RVA: 0x00025ED0 File Offset: 0x000240D0
		public Bounds GetTileBounds(IntRect rect)
		{
			return this.GetTileBounds(rect.xmin, rect.ymin, rect.Width, rect.Height);
		}

		// Token: 0x060007A9 RID: 1961 RVA: 0x00025EF2 File Offset: 0x000240F2
		public Bounds GetTileBounds(int x, int z, int width = 1, int depth = 1)
		{
			return this.transform.Transform(this.GetTileBoundsInGraphSpace(x, z, width, depth));
		}

		// Token: 0x060007AA RID: 1962 RVA: 0x00025F0A File Offset: 0x0002410A
		public Bounds GetTileBoundsInGraphSpace(IntRect rect)
		{
			return this.GetTileBoundsInGraphSpace(rect.xmin, rect.ymin, rect.Width, rect.Height);
		}

		// Token: 0x060007AB RID: 1963 RVA: 0x00025F2C File Offset: 0x0002412C
		public Bounds GetTileBoundsInGraphSpace(int x, int z, int width = 1, int depth = 1)
		{
			Bounds bounds = default(Bounds);
			bounds.SetMinMax(new Vector3((float)x * this.TileWorldSizeX, 0f, (float)z * this.TileWorldSizeZ), new Vector3((float)(x + width) * this.TileWorldSizeX, this.forcedBoundsSize.y, (float)(z + depth) * this.TileWorldSizeZ));
			return bounds;
		}

		// Token: 0x060007AC RID: 1964 RVA: 0x00025F8C File Offset: 0x0002418C
		public Vector2Int GetTileCoordinates(Vector3 position)
		{
			position = this.transform.InverseTransform(position);
			position.x /= this.TileWorldSizeX;
			position.z /= this.TileWorldSizeZ;
			return new Vector2Int((int)position.x, (int)position.z);
		}

		// Token: 0x060007AD RID: 1965 RVA: 0x00025FDC File Offset: 0x000241DC
		protected override void OnDestroy()
		{
			base.OnDestroy();
			TriangleMeshNode.ClearNavmeshHolder((int)this.graphIndex, this);
		}

		// Token: 0x060007AE RID: 1966 RVA: 0x00025FF0 File Offset: 0x000241F0
		protected override void DisposeUnmanagedData()
		{
			base.DisposeUnmanagedData();
			this.navmeshUpdateData.Dispose();
		}

		// Token: 0x060007AF RID: 1967 RVA: 0x00026004 File Offset: 0x00024204
		protected override void DestroyAllNodes()
		{
			this.navmeshUpdateData.MapBoostPrepareForGraphMutation(false);
			this.GetNodes(delegate(GraphNode node)
			{
				node.GetConnections<GraphNode>(delegate(GraphNode other, ref GraphNode node)
				{
					if (node.GraphIndex != other.GraphIndex)
					{
						other.RemovePartialConnection(node);
					}
				}, ref node, 32);
				node.ClearConnections(false);
				node.Destroy();
			});
			if (this.tiles != null)
			{
				for (int i = 0; i < this.tiles.Length; i++)
				{
					this.tiles[i].Dispose();
				}
				this.tiles = null;
			}
			this.navmeshUpdateData.Dispose();
		}

		// Token: 0x060007B0 RID: 1968 RVA: 0x0002607C File Offset: 0x0002427C
		public override void RelocateNodes(Matrix4x4 deltaMatrix)
		{
			throw new Exception("This method cannot be used for navmesh or recast graphs. Please use the other overload of RelocateNodes instead");
		}

		// Token: 0x060007B1 RID: 1969 RVA: 0x00026088 File Offset: 0x00024288
		public void RelocateNodes(GraphTransform newTransform)
		{
			this.RelocateNodes(newTransform, null);
		}

		// Token: 0x060007B2 RID: 1970 RVA: 0x000260A5 File Offset: 0x000242A5
		public void RelocateNodesForWorldOrigin(Vector3 delta)
		{
			this.RelocateNodes(new GraphTransform(Matrix4x4.Translate(delta) * this.transform.matrix), new Vector3?(delta));
		}

		// Token: 0x060007B3 RID: 1971 RVA: 0x000260D0 File Offset: 0x000242D0
		private void RelocateNodes(GraphTransform newTransform, Vector3? worldOriginDelta)
		{
			base.AssertSafeToUpdateGraph();
			base.DirtyBounds(this.bounds);
			this.transform = newTransform;
			if (this.tiles != null)
			{
				for (int i = 0; i < this.tiles.Length; i++)
				{
					NavmeshTile navmeshTile = this.tiles[i];
					if (navmeshTile != null)
					{
						navmeshTile.vertsInGraphSpace.CopyTo(navmeshTile.verts);
						this.transform.Transform(navmeshTile.verts);
						for (int j = 0; j < navmeshTile.nodes.Length; j++)
						{
							navmeshTile.nodes[j].UpdatePositionFromVertices();
						}
					}
				}
				base.DirtyBounds(this.bounds);
				if (worldOriginDelta != null)
				{
					this.navmeshUpdateData.TranslateWorldOrigin(worldOriginDelta.Value);
					return;
				}
				this.navmeshUpdateData.UpdateLayoutFromGraph();
			}
		}

		// Token: 0x060007B4 RID: 1972 RVA: 0x00026198 File Offset: 0x00024398
		protected NavmeshTile NewEmptyTile(int x, int z)
		{
			return new NavmeshTile
			{
				x = x,
				z = z,
				w = 1,
				d = 1,
				verts = default(UnsafeSpan<Int3>),
				vertsInGraphSpace = default(UnsafeSpan<Int3>),
				tris = default(UnsafeSpan<int>),
				nodes = new TriangleMeshNode[0],
				bbTree = default(BBTree),
				graph = this
			};
		}

		// Token: 0x060007B5 RID: 1973 RVA: 0x0002620C File Offset: 0x0002440C
		public override void GetNodes(Action<GraphNode> action)
		{
			if (this.tiles == null)
			{
				return;
			}
			for (int i = 0; i < this.tiles.Length; i++)
			{
				if (this.tiles[i] != null && this.tiles[i].x + this.tiles[i].z * this.tileXCount == i)
				{
					TriangleMeshNode[] nodes = this.tiles[i].nodes;
					if (nodes != null)
					{
						for (int j = 0; j < nodes.Length; j++)
						{
							action(nodes[j]);
						}
					}
				}
			}
		}

		// Token: 0x060007B6 RID: 1974 RVA: 0x0002628C File Offset: 0x0002448C
		public override void GetNodes<T>(GraphNode.NodeActionWithData<T> action, ref T data)
		{
			if (this.tiles == null)
			{
				return;
			}
			for (int i = 0; i < this.tiles.Length; i++)
			{
				if (this.tiles[i] != null && this.tiles[i].x + this.tiles[i].z * this.tileXCount == i)
				{
					TriangleMeshNode[] nodes = this.tiles[i].nodes;
					if (nodes != null)
					{
						for (int j = 0; j < nodes.Length; j++)
						{
							action(nodes[j], ref data);
						}
					}
				}
			}
		}

		// Token: 0x060007B7 RID: 1975 RVA: 0x00026310 File Offset: 0x00024510
		public IntRect GetTouchingTiles(Bounds bounds, float margin = 0f)
		{
			bounds = this.transform.InverseTransform(bounds);
			return IntRect.Intersection(new IntRect(Mathf.FloorToInt((bounds.min.x - margin) / this.TileWorldSizeX), Mathf.FloorToInt((bounds.min.z - margin) / this.TileWorldSizeZ), Mathf.FloorToInt((bounds.max.x + margin) / this.TileWorldSizeX), Mathf.FloorToInt((bounds.max.z + margin) / this.TileWorldSizeZ)), new IntRect(0, 0, this.tileXCount - 1, this.tileZCount - 1));
		}

		// Token: 0x060007B8 RID: 1976 RVA: 0x000263B4 File Offset: 0x000245B4
		public IntRect GetTouchingTilesInGraphSpace(Rect rect)
		{
			return IntRect.Intersection(new IntRect(Mathf.FloorToInt(rect.xMin / this.TileWorldSizeX), Mathf.FloorToInt(rect.yMin / this.TileWorldSizeZ), Mathf.FloorToInt(rect.xMax / this.TileWorldSizeX), Mathf.FloorToInt(rect.yMax / this.TileWorldSizeZ)), new IntRect(0, 0, this.tileXCount - 1, this.tileZCount - 1));
		}

		// Token: 0x060007B9 RID: 1977 RVA: 0x00026430 File Offset: 0x00024630
		protected void ConnectTileWithNeighbours(NavmeshTile tile, bool onlyUnflagged = false)
		{
			if (tile.w != 1 || tile.d != 1)
			{
				throw new ArgumentException("Tile widths or depths other than 1 are not supported. The fields exist mainly for possible future expansions.");
			}
			for (int i = -1; i <= 1; i++)
			{
				int num = tile.z + i;
				if (num >= 0 && num < this.tileZCount)
				{
					for (int j = -1; j <= 1; j++)
					{
						int num2 = tile.x + j;
						if (num2 >= 0 && num2 < this.tileXCount && j == 0 != (i == 0))
						{
							NavmeshTile navmeshTile = this.tiles[num2 + num * this.tileXCount];
							if (!onlyUnflagged || !navmeshTile.flag)
							{
								NavmeshBase.ConnectTiles(navmeshTile, tile, this.TileWorldSizeX, this.TileWorldSizeZ, this.MaxTileConnectionEdgeDistance);
							}
						}
					}
				}
			}
		}

		// Token: 0x060007BA RID: 1978 RVA: 0x000264E4 File Offset: 0x000246E4
		public override float NearestNodeDistanceSqrLowerBound(Vector3 position, ref NearestNodeConstraint constraint)
		{
			if (this.tiles == null)
			{
				return float.PositiveInfinity;
			}
			float3 @float = this.transform.InverseTransform(position);
			return new BBTree.ProjectionParams(ref constraint.distanceMetric, this.transform).SquaredRectPointDistanceOnPlane(new IntRect(0, 0, (int)((float)(1000 * this.tileXCount) * this.TileWorldSizeX), (int)((float)(1000 * this.tileZCount) * this.TileWorldSizeZ)), @float);
		}

		// Token: 0x060007BB RID: 1979 RVA: 0x0002655C File Offset: 0x0002475C
		public override NNInfo GetNearest(Vector3 position, ref NearestNodeConstraint constraint)
		{
			if (this.tiles == null)
			{
				return NNInfo.Empty;
			}
			float3 @float = this.transform.InverseTransform(position);
			int num = (int)(@float.x / this.TileWorldSizeX);
			int num2 = (int)(@float.z / this.TileWorldSizeZ);
			num = Mathf.Clamp(num, 0, this.tileXCount - 1);
			num2 = Mathf.Clamp(num2, 0, this.tileZCount - 1);
			int num3 = Math.Max(this.tileXCount, this.tileZCount);
			NNInfo empty = NNInfo.Empty;
			float num4 = constraint.maxDistanceSqrOrDefault(this.active);
			BBTree.ProjectionParams projectionParams = new BBTree.ProjectionParams(ref constraint.distanceMetric, this.transform);
			float num5 = Math.Min(this.TileWorldSizeX, this.TileWorldSizeX);
			for (int i = 0; i < num3; i++)
			{
				int num6 = Math.Min(i + num2 + 1, this.tileZCount);
				for (int j = Math.Max(-i + num2, 0); j < num6; j++)
				{
					int num7 = Math.Abs(i - Math.Abs(j - num2));
					int num8 = num7;
					do
					{
						int num9 = -num8 + num;
						if (num9 >= 0 && num9 < this.tileXCount)
						{
							NavmeshTile navmeshTile = this.tiles[num9 + j * this.tileXCount];
							if (navmeshTile != null && navmeshTile.bbTree.DistanceSqrLowerBound(@float, in projectionParams) <= num4)
							{
								NavmeshTile navmeshTile2 = navmeshTile;
								float3 float2 = @float;
								GraphNode[] nodes = navmeshTile.nodes;
								navmeshTile2.bbTree.QueryClosest(float2, ref constraint, in projectionParams, ref num4, ref empty, nodes, navmeshTile.tris, navmeshTile.vertsInGraphSpace);
							}
						}
						num8 = -num8;
					}
					while (num8 != num7);
				}
				int num10 = i + 1;
				float num11 = (float)math.max(0, num10 - 2) * num5;
				if (projectionParams.alignedWithXZPlane && num4 - 1E-05f <= num11 * num11)
				{
					break;
				}
			}
			if (empty.node != null)
			{
				empty = new NNInfo(empty.node, this.transform.Transform(empty.position), empty.distanceCostSqr);
			}
			return empty;
		}

		// Token: 0x060007BC RID: 1980 RVA: 0x00026750 File Offset: 0x00024950
		public override NNInfo RandomPointOnSurface(NearestNodeConstraint constraint, bool highQuality = true)
		{
			if (highQuality)
			{
				return base.RandomPointOnSurface(constraint, highQuality);
			}
			if (!this.isScanned || this.tiles.Length == 0)
			{
				return NNInfo.Empty;
			}
			int num = global::UnityEngine.Random.Range(0, this.tiles.Length);
			for (int i = 0; i < this.tiles.Length; i++)
			{
				NavmeshTile navmeshTile = this.tiles[(num + i) % this.tiles.Length];
				if (navmeshTile.nodes.Length != 0)
				{
					TriangleMeshNode triangleMeshNode = navmeshTile.nodes[global::UnityEngine.Random.Range(0, navmeshTile.nodes.Length)];
					if (constraint.Suitable(triangleMeshNode))
					{
						return new NNInfo(triangleMeshNode, triangleMeshNode.RandomPointOnSurface(), 0f);
					}
				}
			}
			return NNInfo.Empty;
		}

		// Token: 0x060007BD RID: 1981 RVA: 0x000267F6 File Offset: 0x000249F6
		[Obsolete("Use the overload that takes a NearestNodeConstraint instead. See the migration guide for version 5.4 for more details.")]
		public GraphNode PointOnNavmesh(Vector3 position, NNConstraint constraint)
		{
			return this.PointOnNavmesh(position, constraint.ToNearestNodeConstraint());
		}

		// Token: 0x060007BE RID: 1982 RVA: 0x00026808 File Offset: 0x00024A08
		public GraphNode PointOnNavmesh(Vector3 position, NearestNodeConstraint constraint)
		{
			if (this.tiles == null)
			{
				return null;
			}
			constraint.distanceMetric = DistanceMetric.ClosestAsSeenFromAbove();
			constraint.distanceMetric.distanceScaleAlongProjectionDirection = 0f;
			constraint.maxDistanceSqr = 0f;
			return this.GetNearest(position, ref constraint).node;
		}

		// Token: 0x060007BF RID: 1983 RVA: 0x00026858 File Offset: 0x00024A58
		protected void FillWithEmptyTiles()
		{
			this.tiles = new NavmeshTile[this.tileXCount * this.tileZCount];
			for (int i = 0; i < this.tileZCount; i++)
			{
				for (int j = 0; j < this.tileXCount; j++)
				{
					this.tiles[i * this.tileXCount + j] = this.NewEmptyTile(j, i);
				}
			}
		}

		// Token: 0x060007C0 RID: 1984 RVA: 0x000268B8 File Offset: 0x00024AB8
		protected static void CreateNodeConnections(TriangleMeshNode[] nodes, bool keepExistingConnections)
		{
			List<Connection> list = ListPool<Connection>.Claim();
			Dictionary<Vector2Int, int> dictionary = ObjectPoolSimple<Dictionary<Vector2Int, int>>.Claim();
			dictionary.Clear();
			for (int i = 0; i < nodes.Length; i++)
			{
				TriangleMeshNode triangleMeshNode = nodes[i];
				int vertexCount = triangleMeshNode.GetVertexCount();
				for (int j = 0; j < vertexCount; j++)
				{
					Vector2Int vector2Int = new Vector2Int(triangleMeshNode.GetVertexIndex(j), triangleMeshNode.GetVertexIndex((j + 1) % vertexCount));
					dictionary.TryAdd(vector2Int, i);
				}
			}
			foreach (TriangleMeshNode triangleMeshNode2 in nodes)
			{
				list.Clear();
				if (keepExistingConnections && triangleMeshNode2.connections != null)
				{
					list.AddRange(triangleMeshNode2.connections);
				}
				int vertexCount2 = triangleMeshNode2.GetVertexCount();
				for (int l = 0; l < vertexCount2; l++)
				{
					int vertexIndex = triangleMeshNode2.GetVertexIndex(l);
					int vertexIndex2 = triangleMeshNode2.GetVertexIndex((l + 1) % vertexCount2);
					int num;
					if (dictionary.TryGetValue(new Vector2Int(vertexIndex2, vertexIndex), out num))
					{
						TriangleMeshNode triangleMeshNode3 = nodes[num];
						int vertexCount3 = triangleMeshNode3.GetVertexCount();
						for (int m = 0; m < vertexCount3; m++)
						{
							if (triangleMeshNode3.GetVertexIndex(m) == vertexIndex2 && triangleMeshNode3.GetVertexIndex((m + 1) % vertexCount3) == vertexIndex)
							{
								list.Add(new Connection(triangleMeshNode3, (uint)(triangleMeshNode2.position - triangleMeshNode3.position).costMagnitude, Connection.PackShapeEdgeInfo((byte)l, (byte)m, true, true, true)));
								break;
							}
						}
					}
				}
				triangleMeshNode2.connections = list.ToArrayFromPool<Connection>();
				triangleMeshNode2.SetConnectivityDirty();
			}
			dictionary.Clear();
			ObjectPoolSimple<Dictionary<Vector2Int, int>>.Release(ref dictionary);
			ListPool<Connection>.Release(ref list);
		}

		// Token: 0x060007C1 RID: 1985 RVA: 0x00026A54 File Offset: 0x00024C54
		internal static void ConnectTiles(NavmeshTile tile1, NavmeshTile tile2, float tileWorldSizeX, float tileWorldSizeZ, float maxTileConnectionEdgeDistance)
		{
			if (tile1 == null || tile2 == null)
			{
				return;
			}
			if (tile1.nodes == null)
			{
				throw new ArgumentException("tile1 does not contain any nodes");
			}
			if (tile2.nodes == null)
			{
				throw new ArgumentException("tile2 does not contain any nodes");
			}
			int num = Mathf.Clamp(tile2.x, tile1.x, tile1.x + tile1.w - 1);
			int num2 = Mathf.Clamp(tile1.x, tile2.x, tile2.x + tile2.w - 1);
			int num3 = Mathf.Clamp(tile2.z, tile1.z, tile1.z + tile1.d - 1);
			int num4 = Mathf.Clamp(tile1.z, tile2.z, tile2.z + tile2.d - 1);
			int num5;
			int num6;
			int num7;
			int num8;
			float num9;
			if (num == num2)
			{
				num5 = 2;
				num6 = 0;
				num7 = num3;
				num8 = num4;
				num9 = tileWorldSizeZ;
			}
			else
			{
				if (num3 != num4)
				{
					throw new ArgumentException("Tiles are not adjacent (neither x or z coordinates match)");
				}
				num5 = 0;
				num6 = 2;
				num7 = num;
				num8 = num2;
				num9 = tileWorldSizeX;
			}
			if (Math.Abs(num7 - num8) != 1)
			{
				throw new ArgumentException(string.Concat(new string[]
				{
					"Tiles are not adjacent (tile coordinates must differ by exactly 1. Got '",
					num7.ToString(),
					"' and '",
					num8.ToString(),
					"')"
				}));
			}
			int num10 = (int)Math.Round((double)((float)Math.Max(num7, num8) * num9 * 1000f));
			TriangleMeshNode[] nodes = tile1.nodes;
			TriangleMeshNode[] nodes2 = tile2.nodes;
			TriangleMeshNode[] array = ArrayPool<TriangleMeshNode>.Claim(nodes2.Length);
			int num11 = 0;
			for (int i = 0; i < nodes2.Length; i++)
			{
				TriangleMeshNode triangleMeshNode = nodes2[i];
				int vertexCount = triangleMeshNode.GetVertexCount();
				for (int j = 0; j < vertexCount; j++)
				{
					Int3 vertexInGraphSpace = tile2.GetVertexInGraphSpace(triangleMeshNode.GetVertexIndex(j));
					Int3 vertexInGraphSpace2 = tile2.GetVertexInGraphSpace(triangleMeshNode.GetVertexIndex((j + 1) % vertexCount));
					if (Math.Abs(vertexInGraphSpace[num5] - num10) < 2 && Math.Abs(vertexInGraphSpace2[num5] - num10) < 2)
					{
						array[num11] = nodes2[i];
						num11++;
						break;
					}
				}
			}
			foreach (TriangleMeshNode triangleMeshNode2 in nodes)
			{
				int vertexCount2 = triangleMeshNode2.GetVertexCount();
				for (int l = 0; l < vertexCount2; l++)
				{
					Int3 vertexInGraphSpace3 = tile1.GetVertexInGraphSpace(triangleMeshNode2.GetVertexIndex(l));
					Int3 vertexInGraphSpace4 = tile1.GetVertexInGraphSpace(triangleMeshNode2.GetVertexIndex((l + 1) % vertexCount2));
					if (Math.Abs(vertexInGraphSpace3[num5] - num10) < 2 && Math.Abs(vertexInGraphSpace4[num5] - num10) < 2)
					{
						int num12 = Math.Min(vertexInGraphSpace3[num6], vertexInGraphSpace4[num6]);
						int num13 = Math.Max(vertexInGraphSpace3[num6], vertexInGraphSpace4[num6]);
						if (num12 != num13)
						{
							for (int m = 0; m < num11; m++)
							{
								TriangleMeshNode triangleMeshNode3 = array[m];
								int vertexCount3 = triangleMeshNode3.GetVertexCount();
								for (int n = 0; n < vertexCount3; n++)
								{
									Int3 vertexInGraphSpace5 = tile2.GetVertexInGraphSpace(triangleMeshNode3.GetVertexIndex(n));
									Int3 vertexInGraphSpace6 = tile2.GetVertexInGraphSpace(triangleMeshNode3.GetVertexIndex((n + 1) % vertexCount3));
									if (Math.Abs(vertexInGraphSpace5[num5] - num10) < 2 && Math.Abs(vertexInGraphSpace6[num5] - num10) < 2)
									{
										int num14 = Math.Min(vertexInGraphSpace5[num6], vertexInGraphSpace6[num6]);
										int num15 = Math.Max(vertexInGraphSpace5[num6], vertexInGraphSpace6[num6]);
										if (num14 != num15 && num13 > num14 && num12 < num15)
										{
											bool flag = (vertexInGraphSpace3 == vertexInGraphSpace5 && vertexInGraphSpace4 == vertexInGraphSpace6) || (vertexInGraphSpace3 == vertexInGraphSpace6 && vertexInGraphSpace4 == vertexInGraphSpace5);
											if (flag || VectorMath.SqrDistanceSegmentSegment((Vector3)vertexInGraphSpace3, (Vector3)vertexInGraphSpace4, (Vector3)vertexInGraphSpace5, (Vector3)vertexInGraphSpace6) < maxTileConnectionEdgeDistance * maxTileConnectionEdgeDistance)
											{
												uint costMagnitude = (uint)(triangleMeshNode2.position - triangleMeshNode3.position).costMagnitude;
												triangleMeshNode2.AddPartialConnection(triangleMeshNode3, costMagnitude, Connection.PackShapeEdgeInfo((byte)l, (byte)n, flag, true, true));
												triangleMeshNode3.AddPartialConnection(triangleMeshNode2, costMagnitude, Connection.PackShapeEdgeInfo((byte)n, (byte)l, flag, true, true));
											}
										}
									}
								}
							}
						}
					}
				}
			}
			ArrayPool<TriangleMeshNode>.Release(ref array, false);
		}

		// Token: 0x060007C2 RID: 1986 RVA: 0x00026ECE File Offset: 0x000250CE
		public void StartBatchTileUpdate(bool exclusive = false)
		{
			base.AssertSafeToUpdateGraph();
			if (exclusive && this.batchTileUpdate > 0)
			{
				throw new InvalidOperationException("Calling StartBatchTileUpdate when batching is already enabled");
			}
			this.batchTileUpdate++;
		}

		// Token: 0x060007C3 RID: 1987 RVA: 0x00026EFC File Offset: 0x000250FC
		private static void DestroyNodes(List<MeshNode> nodes)
		{
			for (int i = 0; i < nodes.Count; i++)
			{
				nodes[i].TemporaryFlag1 = true;
			}
			for (int j = 0; j < nodes.Count; j++)
			{
				MeshNode meshNode = nodes[j];
				if (meshNode.connections != null)
				{
					for (int k = 0; k < meshNode.connections.Length; k++)
					{
						GraphNode node = meshNode.connections[k].node;
						if (!node.TemporaryFlag1)
						{
							node.RemovePartialConnection(meshNode);
						}
					}
					ArrayPool<Connection>.Release(ref meshNode.connections, true);
				}
				meshNode.Destroy();
			}
		}

		// Token: 0x060007C4 RID: 1988 RVA: 0x00026F94 File Offset: 0x00025194
		private void TryConnect(int tileIdx1, int tileIdx2)
		{
			if (this.tiles[tileIdx1].flag && this.tiles[tileIdx2].flag && tileIdx1 >= tileIdx2)
			{
				return;
			}
			NavmeshBase.ConnectTiles(this.tiles[tileIdx1], this.tiles[tileIdx2], this.TileWorldSizeX, this.TileWorldSizeZ, this.MaxTileConnectionEdgeDistance);
		}

		// Token: 0x060007C5 RID: 1989 RVA: 0x00026FEC File Offset: 0x000251EC
		public void EndBatchTileUpdate()
		{
			if (this.batchTileUpdate < 0)
			{
				throw new InvalidOperationException("Calling EndBatchTileUpdate when batching had not yet been started");
			}
			if (this.batchTileUpdate > 1)
			{
				this.batchTileUpdate--;
				return;
			}
			if (this.batchPendingNavmeshCutting)
			{
				this.batchPendingNavmeshCutting = false;
				this.navmeshUpdateData.ReloadDirtyTilesImmediately();
			}
			this.batchTileUpdate--;
			NavmeshBase.DestroyNodes(this.batchNodesToDestroy);
			this.batchNodesToDestroy.ClearFast<MeshNode>();
			if (this.batchUpdatedTiles.Count == 0)
			{
				return;
			}
			for (int i = 0; i < this.batchUpdatedTiles.Count; i++)
			{
				this.tiles[this.batchUpdatedTiles[i]].flag = true;
			}
			IntRect intRect = default(IntRect);
			for (int j = 0; j < this.batchUpdatedTiles.Count; j++)
			{
				int num = this.batchUpdatedTiles[j] % this.tileXCount;
				int num2 = this.batchUpdatedTiles[j] / this.tileXCount;
				if (j == 0)
				{
					intRect = new IntRect(num, num2, num, num2);
				}
				else
				{
					intRect = intRect.ExpandToContain(num, num2);
				}
				if (num > 0)
				{
					this.TryConnect(this.batchUpdatedTiles[j], this.batchUpdatedTiles[j] - 1);
				}
				if (num < this.tileXCount - 1)
				{
					this.TryConnect(this.batchUpdatedTiles[j], this.batchUpdatedTiles[j] + 1);
				}
				if (num2 > 0)
				{
					this.TryConnect(this.batchUpdatedTiles[j], this.batchUpdatedTiles[j] - this.tileXCount);
				}
				if (num2 < this.tileZCount - 1)
				{
					this.TryConnect(this.batchUpdatedTiles[j], this.batchUpdatedTiles[j] + this.tileXCount);
				}
			}
			for (int k = 0; k < this.batchUpdatedTiles.Count; k++)
			{
				this.tiles[this.batchUpdatedTiles[k]].flag = false;
			}
			this.batchUpdatedTiles.ClearFast<int>();
			base.DirtyBounds(this.GetTileBounds(intRect));
		}

		// Token: 0x060007C6 RID: 1990 RVA: 0x00027200 File Offset: 0x00025400
		public void ClearTiles(IntRect tileRect)
		{
			base.AssertSafeToUpdateGraph();
			this.StartBatchTileUpdate(false);
			IntRect intRect = new IntRect(0, 0, this.tileXCount - 1, this.tileZCount - 1);
			tileRect = IntRect.Intersection(tileRect, intRect);
			for (int i = tileRect.ymin; i <= tileRect.ymax; i++)
			{
				for (int j = tileRect.xmin; j <= tileRect.xmax; j++)
				{
					NavmeshTile navmeshTile = this.NewEmptyTile(j, i);
					NavmeshTile navmeshTile2 = navmeshTile;
					NavmeshTile tile = this.GetTile(j, i);
					navmeshTile2.isCut = tile != null && tile.isCut;
					this.ClearTile(j, i, navmeshTile);
				}
			}
			this.EndBatchTileUpdate();
		}

		// Token: 0x060007C7 RID: 1991 RVA: 0x0002729C File Offset: 0x0002549C
		protected void ClearTile(int x, int z, NavmeshTile replacement)
		{
			if (this.batchTileUpdate == 0)
			{
				throw new Exception("Must be called during a batch update. See StartBatchTileUpdate");
			}
			NavmeshTile tile = this.GetTile(x, z);
			if (tile == null)
			{
				return;
			}
			TriangleMeshNode[] nodes = tile.nodes;
			for (int i = 0; i < nodes.Length; i++)
			{
				if (nodes[i] != null)
				{
					this.batchNodesToDestroy.Add(nodes[i]);
				}
			}
			tile.Dispose();
			this.tiles[x + z * this.tileXCount] = replacement;
		}

		// Token: 0x060007C8 RID: 1992 RVA: 0x0002730C File Offset: 0x0002550C
		private unsafe void PrepareNodeRecycling(int x, int z, UnsafeSpan<Int3> verts, UnsafeSpan<int> tris, TriangleMeshNode[] recycledNodeBuffer)
		{
			NavmeshTile tile = this.GetTile(x, z);
			if (tile == null || tile.nodes.Length == 0)
			{
				return;
			}
			TriangleMeshNode[] nodes = tile.nodes;
			Dictionary<int, int> dictionary = this.nodeRecyclingHashBuffer;
			int i = 0;
			int num = 0;
			while (i < tris.Length)
			{
				dictionary[(verts[*tris[i]].GetHashCode() * 31) ^ (verts[*tris[i + 1]].GetHashCode() * 196613) ^ (verts[*tris[i + 2]].GetHashCode() * 3145739)] = num;
				i += 3;
				num++;
			}
			List<Connection> list = ListPool<Connection>.Claim();
			for (int j = 0; j < nodes.Length; j++)
			{
				TriangleMeshNode triangleMeshNode = nodes[j];
				Int3 @int;
				Int3 int2;
				Int3 int3;
				triangleMeshNode.GetVerticesInGraphSpace(out @int, out int2, out int3);
				int num2 = (@int.GetHashCode() * 31) ^ (int2.GetHashCode() * 196613) ^ (int3.GetHashCode() * 3145739);
				int num3;
				if (dictionary.TryGetValue(num2, out num3) && *verts[*tris[3 * num3]] == @int && *verts[*tris[3 * num3 + 1]] == int2 && *verts[*tris[3 * num3 + 2]] == int3)
				{
					recycledNodeBuffer[num3] = triangleMeshNode;
					nodes[j] = null;
					if (triangleMeshNode.connections != null)
					{
						for (int k = 0; k < triangleMeshNode.connections.Length; k++)
						{
							if (triangleMeshNode.connections[k].node.GraphIndex != triangleMeshNode.GraphIndex)
							{
								list.Add(triangleMeshNode.connections[k]);
							}
						}
						ArrayPool<Connection>.Release(ref triangleMeshNode.connections, true);
					}
					if (list.Count > 0)
					{
						triangleMeshNode.connections = list.ToArrayFromPool<Connection>();
						triangleMeshNode.SetConnectivityDirty();
						list.Clear();
					}
				}
			}
			dictionary.Clear();
			ListPool<Connection>.Release(ref list);
		}

		// Token: 0x060007C9 RID: 1993 RVA: 0x0002755C File Offset: 0x0002575C
		public void ReplaceTile(int x, int z, Int3[] verts, int[] tris, uint[] tags = null, bool tryPreserveExistingTagsAndPenalties = true)
		{
			ulong num;
			UnsafeSpan<Int3> unsafeSpan = new UnsafeSpan<Int3>(verts, out num);
			ulong num2;
			UnsafeSpan<int> unsafeSpan2 = new UnsafeSpan<int>(tris, out num2);
			ulong num3 = 0UL;
			UnsafeSpan<uint> unsafeSpan3 = ((tags != null) ? new UnsafeSpan<uint>(tags, out num3) : default(UnsafeSpan<uint>));
			try
			{
				this.ReplaceTile(x, z, unsafeSpan, unsafeSpan2, unsafeSpan3, tryPreserveExistingTagsAndPenalties);
			}
			finally
			{
				UnsafeUtility.ReleaseGCObject(num);
				UnsafeUtility.ReleaseGCObject(num2);
				if (tags != null)
				{
					UnsafeUtility.ReleaseGCObject(num3);
				}
			}
		}

		// Token: 0x060007CA RID: 1994 RVA: 0x000275D4 File Offset: 0x000257D4
		public void ReplaceTile(int x, int z, UnsafeSpan<Int3> verts, UnsafeSpan<int> tris, UnsafeSpan<uint> tags, bool tryPreserveExistingTagsAndPenalties = true)
		{
			base.AssertSafeToUpdateGraph();
			if (tris.Length % 3 != 0)
			{
				throw new ArgumentException("Triangle array's length must be a multiple of 3 (tris)");
			}
			if (tags.Length > 0 && tags.Length != tris.Length / 3)
			{
				throw new ArgumentException("Triangle array must be 3 times the size of the tags array");
			}
			NavmeshTile tile = this.GetTile(x, z);
			if (tile.isCut)
			{
				this.StartBatchTileUpdate(false);
				tile.preCutTags.Free(Allocator.Persistent);
				tile.preCutTris.Free(Allocator.Persistent);
				tile.preCutVertsInTileSpace.Free(Allocator.Persistent);
				if (tags.Length == 0)
				{
					tile.preCutTags = new UnsafeSpan<uint>(Allocator.Persistent, tris.Length / 3);
					tile.preCutTags.FillZeros<uint>();
				}
				else
				{
					tile.preCutTags = tags.Clone(Allocator.Persistent);
				}
				tile.preCutTris = tris.Clone(Allocator.Persistent);
				tile.preCutVertsInTileSpace = verts.Clone(Allocator.Persistent);
				tile.isCut = true;
				this.navmeshUpdateData.MarkTilesDirty(new IntRect(x, z, x, z));
				this.batchPendingNavmeshCutting = true;
				this.EndBatchTileUpdate();
				return;
			}
			this.ReplaceTilePostCut(x, z, verts, tris, tags, tryPreserveExistingTagsAndPenalties, false);
		}

		// Token: 0x060007CB RID: 1995 RVA: 0x000276F4 File Offset: 0x000258F4
		internal unsafe void ReplaceTilePostCut(int x, int z, UnsafeSpan<Int3> verts, UnsafeSpan<int> tris, UnsafeSpan<uint> tags, bool tryPreserveExistingTagsAndPenalties = true, bool preservePreCutData = false)
		{
			base.AssertSafeToUpdateGraph();
			int num = 1;
			int num2 = 1;
			if (x + num > this.tileXCount || z + num2 > this.tileZCount || x < 0 || z < 0)
			{
				throw new ArgumentException(string.Concat(new string[]
				{
					"Tile is placed at an out of bounds position or extends out of the graph bounds (",
					x.ToString(),
					", ",
					z.ToString(),
					" [",
					num.ToString(),
					", ",
					num2.ToString(),
					"] ",
					this.tileXCount.ToString(),
					" ",
					this.tileZCount.ToString(),
					")"
				}));
			}
			if (tris.Length % 3 != 0)
			{
				throw new ArgumentException("Triangle array's length must be a multiple of 3 (tris)");
			}
			if (tags.Length > 0 && tags.Length != tris.Length / 3)
			{
				throw new ArgumentException("Triangle array must be 3 times the size of the tags array");
			}
			if (verts.Length > 4095)
			{
				Debug.LogError(string.Concat(new string[]
				{
					"Too many vertices in the tile (",
					verts.Length.ToString(),
					" > ",
					4095.ToString(),
					")\nYou can enable ASTAR_RECAST_LARGER_TILES under the 'Optimizations' tab in the A* Inspector to raise this limit. Or you can use a higher simplification level (RecastGraph.contourMaxError) or a smaller tile size to reduce the likelihood of this happening (a tile size between 64 and 256 is recommended, but you can go outside it if necessary). The affected tile will be left empty."
				}));
				verts = default(UnsafeSpan<Int3>);
				tris = default(UnsafeSpan<int>);
			}
			if (tris.Length == 0 && this.GetTile(x, z).nodes.Length == 0 && preservePreCutData)
			{
				return;
			}
			this.StartBatchTileUpdate(false);
			UnsafeSpan<int> unsafeSpan = tris.Clone(Allocator.Persistent);
			unsafeSpan.CopyFrom<int>(tris);
			UnsafeSpan<Int3> unsafeSpan2 = verts.Clone(Allocator.Persistent);
			Int3 @int = (Int3)new Vector3((float)x * this.TileWorldSizeX, 0f, (float)z * this.TileWorldSizeZ);
			for (int i = 0; i < verts.Length; i++)
			{
				*unsafeSpan2[i] += @int;
			}
			UnsafeSpan<Int3> unsafeSpan3 = unsafeSpan2.Clone(Allocator.Persistent);
			this.transform.Transform(unsafeSpan3);
			BBTree bbtree = new BBTree(unsafeSpan, unsafeSpan2);
			NavmeshTile navmeshTile = new NavmeshTile
			{
				x = x,
				z = z,
				w = num,
				d = num2,
				tris = unsafeSpan,
				vertsInGraphSpace = unsafeSpan2,
				verts = unsafeSpan3,
				bbTree = bbtree,
				graph = this
			};
			if (!Mathf.Approximately((float)x * this.TileWorldSizeX * 1000f, (float)Math.Round((double)((float)x * this.TileWorldSizeX * 1000f))))
			{
				Debug.LogWarning("Possible numerical imprecision. Consider adjusting tile size and/or voxel size");
			}
			if (!Mathf.Approximately((float)z * this.TileWorldSizeZ * 1000f, (float)Math.Round((double)((float)z * this.TileWorldSizeZ * 1000f))))
			{
				Debug.LogWarning("Possible numerical imprecision. Consider adjusting tile size and/or voxel size");
			}
			navmeshTile.nodes = new TriangleMeshNode[unsafeSpan.Length / 3];
			this.PrepareNodeRecycling(x, z, unsafeSpan2, unsafeSpan, navmeshTile.nodes);
			if (this.RecalculateNormals)
			{
				MeshUtility.MakeTrianglesClockwise(ref navmeshTile.vertsInGraphSpace, ref navmeshTile.tris);
			}
			NavmeshBase.CreateNodes(navmeshTile, navmeshTile.tris, x + z * this.tileXCount, (uint)this.active.data.GetGraphIndex(this), tags, true, this.active, this.initialPenalty, tryPreserveExistingTagsAndPenalties);
			NavmeshBase.CreateNodeConnections(navmeshTile.nodes, true);
			if (preservePreCutData)
			{
				NavmeshTile tile = this.GetTile(x, z);
				navmeshTile.preCutVertsInTileSpace = tile.preCutVertsInTileSpace;
				navmeshTile.preCutTris = tile.preCutTris;
				navmeshTile.preCutTags = tile.preCutTags;
				navmeshTile.isCut = tile.isCut;
				tile.preCutVertsInTileSpace = default(UnsafeSpan<Int3>);
				tile.preCutTris = default(UnsafeSpan<int>);
				tile.preCutTags = default(UnsafeSpan<uint>);
			}
			this.ClearTile(x, z, navmeshTile);
			this.batchUpdatedTiles.Add(x + z * this.tileXCount);
			this.EndBatchTileUpdate();
		}

		// Token: 0x060007CC RID: 1996 RVA: 0x00027AEC File Offset: 0x00025CEC
		internal unsafe static void CreateNodes(NavmeshTile tile, UnsafeSpan<int> tris, int tileIndex, uint graphIndex, UnsafeSpan<uint> tags, bool initializeNodes, AstarPath astar, uint initialPenalty, bool tryPreserveExistingTagsAndPenalties)
		{
			TriangleMeshNode[] nodes = tile.nodes;
			if (nodes == null || nodes.Length < tris.Length / 3)
			{
				throw new ArgumentException("nodes must be non null and at least as large as tris.Length/3");
			}
			tileIndex <<= 12;
			for (int i = 0; i < nodes.Length; i++)
			{
				TriangleMeshNode triangleMeshNode = nodes[i];
				bool flag = false;
				if (triangleMeshNode == null)
				{
					flag = true;
					if (initializeNodes)
					{
						triangleMeshNode = (nodes[i] = new TriangleMeshNode(astar));
					}
					else
					{
						triangleMeshNode = (nodes[i] = new TriangleMeshNode());
					}
				}
				if (!tryPreserveExistingTagsAndPenalties || flag)
				{
					if (tags.Length > 0)
					{
						triangleMeshNode.Tag = *tags[i];
					}
					triangleMeshNode.Penalty = initialPenalty;
				}
				triangleMeshNode.Walkable = true;
				triangleMeshNode.GraphIndex = graphIndex;
				triangleMeshNode.v0 = *tris[i * 3] | tileIndex;
				triangleMeshNode.v1 = *tris[i * 3 + 1] | tileIndex;
				triangleMeshNode.v2 = *tris[i * 3 + 2] | tileIndex;
				triangleMeshNode.position = (tile.GetVertex(triangleMeshNode.v0) + tile.GetVertex(triangleMeshNode.v1) + tile.GetVertex(triangleMeshNode.v2)) * 0.33333334f;
			}
		}

		// Token: 0x060007CD RID: 1997 RVA: 0x00027C18 File Offset: 0x00025E18
		public NavmeshBase()
		{
			new NavmeshUpdates.NavmeshUpdateSettings(this).AttachToGraph();
		}

		// Token: 0x060007CE RID: 1998 RVA: 0x00027C91 File Offset: 0x00025E91
		public bool Linecast(Vector3 start, Vector3 end)
		{
			return this.Linecast(start, end, null);
		}

		// Token: 0x060007CF RID: 1999 RVA: 0x00027C9C File Offset: 0x00025E9C
		public bool Linecast(Vector3 start, Vector3 end, GraphNode hint, out GraphHitInfo hit)
		{
			TraversalConstraint none = TraversalConstraint.None;
			return NavmeshBase.Linecast(this, start, end, hint, out hit, ref none, null);
		}

		// Token: 0x060007D0 RID: 2000 RVA: 0x00027CC0 File Offset: 0x00025EC0
		public bool Linecast(Vector3 start, Vector3 end, GraphNode hint)
		{
			TraversalConstraint none = TraversalConstraint.None;
			GraphHitInfo graphHitInfo;
			return NavmeshBase.Linecast(this, start, end, hint, out graphHitInfo, ref none, null);
		}

		// Token: 0x060007D1 RID: 2001 RVA: 0x00027CE4 File Offset: 0x00025EE4
		public bool Linecast(Vector3 start, Vector3 end, GraphNode hint, out GraphHitInfo hit, List<GraphNode> trace)
		{
			TraversalConstraint none = TraversalConstraint.None;
			return NavmeshBase.Linecast(this, start, end, hint, out hit, ref none, trace);
		}

		// Token: 0x060007D2 RID: 2002 RVA: 0x00027D06 File Offset: 0x00025F06
		public bool Linecast(Vector3 start, Vector3 end, out GraphHitInfo hit, ref TraversalConstraint traversalConstraint, List<GraphNode> trace)
		{
			return NavmeshBase.Linecast(this, start, end, null, out hit, ref traversalConstraint, trace);
		}

		// Token: 0x060007D3 RID: 2003 RVA: 0x00027D16 File Offset: 0x00025F16
		public bool Linecast(Vector3 start, Vector3 end, GraphNode hint, out GraphHitInfo hit, ref TraversalConstraint traversalConstraint, List<GraphNode> trace)
		{
			return NavmeshBase.Linecast(this, start, end, hint, out hit, ref traversalConstraint, trace);
		}

		// Token: 0x060007D4 RID: 2004 RVA: 0x00027D28 File Offset: 0x00025F28
		public static bool Linecast(NavmeshBase graph, Vector3 start, Vector3 end, GraphNode hint, out GraphHitInfo hit)
		{
			TraversalConstraint none = TraversalConstraint.None;
			return NavmeshBase.Linecast(graph, start, end, hint, out hit, ref none, null);
		}

		// Token: 0x060007D5 RID: 2005 RVA: 0x00027D4C File Offset: 0x00025F4C
		static NavmeshBase()
		{
			Side[] array = new Side[3];
			for (int i = 0; i < NavmeshBase.LinecastShapeEdgeLookup.Length; i++)
			{
				array[0] = (Side)(i & 3);
				array[1] = (Side)((i >> 2) & 3);
				array[2] = (Side)((i >> 4) & 3);
				NavmeshBase.LinecastShapeEdgeLookup[i] = byte.MaxValue;
				if (array[0] != (Side)3 && array[1] != (Side)3 && array[2] != (Side)3)
				{
					int num = int.MaxValue;
					for (int j = 0; j < 3; j++)
					{
						if ((array[j] == Side.Left || array[j] == Side.Colinear) && (array[(j + 1) % 3] == Side.Right || array[(j + 1) % 3] == Side.Colinear))
						{
							int num2 = ((array[j] == Side.Colinear) ? 1 : 0) + ((array[(j + 1) % 3] == Side.Colinear) ? 1 : 0);
							if (num2 < num)
							{
								NavmeshBase.LinecastShapeEdgeLookup[i] = (byte)j;
								num = num2;
							}
						}
					}
				}
			}
		}

		// Token: 0x060007D6 RID: 2006 RVA: 0x00027E18 File Offset: 0x00026018
		public static bool Linecast(NavmeshBase graph, Vector3 origin, Vector3 end, GraphNode hint, out GraphHitInfo hit, ref TraversalConstraint traversalConstraint, List<GraphNode> trace)
		{
			if (!graph.RecalculateNormals)
			{
				throw new InvalidOperationException("The graph is configured to not recalculate normals. This is typically used for spherical navmeshes or other non-planar ones. Linecasts cannot be done on such navmeshes. Enable 'Recalculate Normals' on the navmesh graph if you want to use linecasts.");
			}
			hit = default(GraphHitInfo);
			if (float.IsNaN(origin.x + origin.y + origin.z))
			{
				throw new ArgumentException("origin is NaN");
			}
			if (float.IsNaN(end.x + end.y + end.z))
			{
				throw new ArgumentException("end is NaN");
			}
			TriangleMeshNode triangleMeshNode = hint as TriangleMeshNode;
			NearestNodeConstraint none = NearestNodeConstraint.None;
			none.distanceMetric = DistanceMetric.ClosestAsSeenFromAbove();
			none.maxDistance = new float?(0.001f);
			if (triangleMeshNode == null)
			{
				triangleMeshNode = graph.GetNearest(origin, ref none).node as TriangleMeshNode;
				if (triangleMeshNode == null)
				{
					hit.origin = origin;
					hit.point = origin;
					hit.tangentOrigin = hit.origin;
					return true;
				}
			}
			Int3 @int = triangleMeshNode.ClosestPointOnNodeXZInGraphSpace(origin);
			hit.origin = graph.transform.Transform((Vector3)@int);
			if (!traversalConstraint.CanTraverse(triangleMeshNode))
			{
				hit.node = triangleMeshNode;
				hit.point = hit.origin;
				hit.tangentOrigin = hit.origin;
				return true;
			}
			Int3 int2 = (Int3)graph.transform.InverseTransform(end);
			if (@int == int2)
			{
				hit.point = hit.origin;
				hit.node = triangleMeshNode;
				if (trace != null)
				{
					trace.Add(triangleMeshNode);
				}
				return false;
			}
			int num = 0;
			Int3 int3;
			Int3 int4;
			Int3 int5;
			int num3;
			TriangleMeshNode triangleMeshNode2;
			for (;;)
			{
				num++;
				if (num > 2000)
				{
					break;
				}
				if (trace != null)
				{
					trace.Add(triangleMeshNode);
				}
				triangleMeshNode.GetVerticesInGraphSpace(out int3, out int4, out int5);
				int num2 = (int)VectorMath.SideXZ(@int, int2, int3);
				num2 |= (int)((int)VectorMath.SideXZ(@int, int2, int4) << 2);
				num2 |= (int)((int)VectorMath.SideXZ(@int, int2, int5) << 4);
				num3 = (int)NavmeshBase.LinecastShapeEdgeLookup[num2];
				if (num3 == 255)
				{
					goto Block_11;
				}
				Side side = VectorMath.SideXZ((num3 == 0) ? int3 : ((num3 == 1) ? int4 : int5), (num3 == 0) ? int4 : ((num3 == 1) ? int5 : int3), int2);
				if (side != Side.Left)
				{
					hit.point = end;
					hit.node = triangleMeshNode;
					triangleMeshNode2 = graph.GetNearest(end, ref none).node as TriangleMeshNode;
					if (triangleMeshNode2 == triangleMeshNode || triangleMeshNode2 == null)
					{
						return false;
					}
					if (side != Side.Colinear)
					{
						return true;
					}
					if (int3 == int2 || int4 == int2 || int5 == int2)
					{
						goto IL_0281;
					}
				}
				bool flag = false;
				Connection[] connections = triangleMeshNode.connections;
				for (int i = 0; i < connections.Length; i++)
				{
					if (connections[i].isEdgeShared && connections[i].isOutgoing && connections[i].shapeEdge == num3)
					{
						TriangleMeshNode triangleMeshNode3 = connections[i].node as TriangleMeshNode;
						if (triangleMeshNode3 != null && traversalConstraint.CanTraverse(triangleMeshNode, triangleMeshNode3))
						{
							int adjacentShapeEdge = connections[i].adjacentShapeEdge;
							Side side2 = VectorMath.SideXZ(@int, int2, triangleMeshNode3.GetVertexInGraphSpace(adjacentShapeEdge));
							Side side3 = VectorMath.SideXZ(@int, int2, triangleMeshNode3.GetVertexInGraphSpace((adjacentShapeEdge + 1) % 3));
							flag = (side2 == Side.Right || side2 == Side.Colinear) && (side3 == Side.Left || side3 == Side.Colinear);
							if (flag)
							{
								triangleMeshNode = triangleMeshNode3;
								break;
							}
						}
					}
				}
				if (!flag)
				{
					goto Block_30;
				}
			}
			Debug.LogError("Linecast was stuck in infinite loop. Breaking.");
			return true;
			Block_11:
			Debug.LogError("Line does not intersect node at all");
			hit.node = triangleMeshNode;
			hit.point = (hit.tangentOrigin = hit.origin);
			return true;
			IL_0281:
			return !NavmeshBase.FindNodeAroundVertex(triangleMeshNode, triangleMeshNode2, int2, false) && !NavmeshBase.FindNodeAroundVertex(triangleMeshNode, triangleMeshNode2, int2, true);
			Block_30:
			Vector3 vector = (Vector3)((num3 == 0) ? int3 : ((num3 == 1) ? int4 : int5));
			Vector3 vector2 = (Vector3)((num3 == 0) ? int4 : ((num3 == 1) ? int5 : int3));
			Vector3 vector3 = VectorMath.LineIntersectionPointXZ(vector, vector2, (Vector3)@int, (Vector3)int2);
			hit.point = graph.transform.Transform(vector3);
			hit.node = triangleMeshNode;
			Vector3 vector4 = graph.transform.Transform(vector);
			Vector3 vector5 = graph.transform.Transform(vector2);
			hit.tangent = vector5 - vector4;
			hit.tangentOrigin = vector4;
			return true;
		}

		// Token: 0x060007D7 RID: 2007 RVA: 0x0002824C File Offset: 0x0002644C
		[Obsolete("Use the overload that takes a TraversalConstraint instead. See the migration guide for version 5.4 for more information.")]
		public bool Linecast(Vector3 start, Vector3 end, out GraphHitInfo hit, List<GraphNode> trace, Func<GraphNode, bool> filter)
		{
			TraversalConstraint none = TraversalConstraint.None;
			none.filter = filter;
			return NavmeshBase.Linecast(this, start, end, null, out hit, ref none, trace);
		}

		// Token: 0x060007D8 RID: 2008 RVA: 0x00028278 File Offset: 0x00026478
		[Obsolete("Use the overload that takes a TraversalConstraint instead. See the migration guide for version 5.4 for more information.")]
		public bool Linecast(Vector3 start, Vector3 end, GraphNode hint, out GraphHitInfo hit, List<GraphNode> trace, Func<GraphNode, bool> filter)
		{
			TraversalConstraint none = TraversalConstraint.None;
			none.filter = filter;
			return NavmeshBase.Linecast(this, start, end, hint, out hit, ref none, trace);
		}

		// Token: 0x060007D9 RID: 2009 RVA: 0x000282A4 File Offset: 0x000264A4
		[Obsolete("Use the overload that takes a TraversalConstraint instead. See the migration guide for version 5.4 for more information.")]
		public static bool Linecast(NavmeshBase graph, Vector3 origin, Vector3 end, GraphNode hint, out GraphHitInfo hit, List<GraphNode> trace, Func<GraphNode, bool> filter = null)
		{
			TraversalConstraint none = TraversalConstraint.None;
			none.filter = filter;
			return NavmeshBase.Linecast(graph, origin, end, hint, out hit, ref none, trace);
		}

		// Token: 0x060007DA RID: 2010 RVA: 0x000282D0 File Offset: 0x000264D0
		private static bool FindNodeAroundVertex(TriangleMeshNode node, TriangleMeshNode targetNode, Int3 vertexInGraphSpace, bool oppositeDirection)
		{
			TriangleMeshNode triangleMeshNode = node;
			bool flag = true;
			while (flag)
			{
				flag = false;
				int num = 0;
				while (num < 3 && !flag)
				{
					if (node.GetVertexInGraphSpace(num) == vertexInGraphSpace)
					{
						int num2 = (oppositeDirection ? ((num - 1 + 3) % 3) : num);
						int i = 0;
						while (i < node.connections.Length)
						{
							Connection connection = node.connections[i];
							if (connection.isEdgeShared && connection.edgesAreIdentical && connection.shapeEdge == num2)
							{
								node = connection.node as TriangleMeshNode;
								flag = true;
								if (node == targetNode)
								{
									return true;
								}
								if (node == triangleMeshNode)
								{
									return false;
								}
								break;
							}
							else
							{
								i++;
							}
						}
					}
					num++;
				}
			}
			return false;
		}

		// Token: 0x060007DB RID: 2011 RVA: 0x0002837C File Offset: 0x0002657C
		public virtual void Resize(IntRect newTileBounds)
		{
			base.AssertSafeToUpdateGraph();
			if (newTileBounds != new IntRect(0, 0, 0, 0))
			{
				throw new InvalidOperationException("Navmesh graphs can only have a single tile. Use a RecastGraph for tiled navmeshes.");
			}
		}

		// Token: 0x060007DC RID: 2012 RVA: 0x000283A0 File Offset: 0x000265A0
		protected virtual void SetLayout(TileLayout info)
		{
			this.tileXCount = info.tileCount.x;
			this.tileZCount = info.tileCount.y;
			this.transform = info.transform;
		}

		// Token: 0x060007DD RID: 2013 RVA: 0x000283D4 File Offset: 0x000265D4
		public TileMeshes ToTileMeshes(IntRect? tileRect = null)
		{
			if (this.tiles == null)
			{
				return new TileMeshes
				{
					tileRect = new IntRect(0, 0, 0, 0),
					tileWorldSize = new Vector2(0f, 0f),
					tileMeshes = new TileMesh[0]
				};
			}
			IntRect intRect = new IntRect(0, 0, this.tileXCount - 1, this.tileZCount - 1);
			IntRect intRect2 = IntRect.Intersection(tileRect ?? intRect, intRect);
			int width = intRect2.Width;
			int height = intRect2.Height;
			TileMeshes tileMeshes = new TileMeshes
			{
				tileRect = intRect2,
				tileWorldSize = new Vector2(this.TileWorldSizeX, this.TileWorldSizeZ),
				tileMeshes = new TileMesh[width * height]
			};
			for (int i = 0; i < height; i++)
			{
				for (int j = 0; j < width; j++)
				{
					int num = intRect2.xmin + j;
					int num2 = intRect2.ymin + i;
					NavmeshTile tile = this.GetTile(num, num2);
					TileMesh tileMesh = default(TileMesh);
					if (tile == null)
					{
						tileMesh.verticesInTileSpace = new Int3[0];
						tileMesh.triangles = new int[0];
						tileMesh.tags = new uint[0];
					}
					else if (tile.isCut)
					{
						tileMesh.verticesInTileSpace = tile.preCutVertsInTileSpace.ToArray();
						tileMesh.triangles = tile.preCutTris.ToArray();
						tileMesh.tags = tile.preCutTags.ToArray();
					}
					else
					{
						tileMesh.verticesInTileSpace = tile.vertsInGraphSpace.ToArray();
						tileMesh.triangles = tile.tris.ToArray();
						tileMesh.tags = new uint[tile.nodes.Length];
						for (int k = 0; k < tile.nodes.Length; k++)
						{
							tileMesh.tags[k] = tile.nodes[k].Tag;
						}
						Int3 @int = (Int3)new Vector3((float)num * this.TileWorldSizeX, 0f, (float)num2 * this.TileWorldSizeZ);
						for (int l = 0; l < tileMesh.verticesInTileSpace.Length; l++)
						{
							tileMesh.verticesInTileSpace[l] -= @int;
						}
					}
					tileMeshes.tileMeshes[j + i * width] = tileMesh;
				}
			}
			return tileMeshes;
		}

		// Token: 0x060007DE RID: 2014 RVA: 0x00028655 File Offset: 0x00026855
		public void EnsureInitialized()
		{
			base.AssertSafeToUpdateGraph();
			if (this.tiles == null)
			{
				TriangleMeshNode.SetNavmeshHolder(AstarPath.active.data.GetGraphIndex(this), this);
				this.SetLayout(TileLayout.FromGraph(this));
				this.FillWithEmptyTiles();
			}
		}

		// Token: 0x060007DF RID: 2015 RVA: 0x00028690 File Offset: 0x00026890
		public void ReplaceTiles(TileMeshes tileMeshes, float yOffset = 0f)
		{
			base.AssertSafeToUpdateGraph();
			this.EnsureInitialized();
			if (tileMeshes.tileWorldSize.x != this.TileWorldSizeX || tileMeshes.tileWorldSize.y != this.TileWorldSizeZ)
			{
				string[] array = new string[7];
				array[0] = "Loaded tile size does not match this graph's tile size.\nThe source tiles have a world-space tile size of ";
				int num = 1;
				Vector2 tileWorldSize = tileMeshes.tileWorldSize;
				array[num] = tileWorldSize.ToString();
				array[2] = " while this graph's tile size is (";
				array[3] = this.TileWorldSizeX.ToString();
				array[4] = ",";
				array[5] = this.TileWorldSizeZ.ToString();
				array[6] = ").\nFor a recast graph, the world-space tile size is defined as the cell size * the tile size in voxels";
				throw new Exception(string.Concat(array));
			}
			int width = tileMeshes.tileRect.Width;
			int height = tileMeshes.tileRect.Height;
			IntRect intRect = IntRect.Union(new IntRect(0, 0, this.tileXCount - 1, this.tileZCount - 1), tileMeshes.tileRect);
			this.Resize(intRect);
			tileMeshes.tileRect = tileMeshes.tileRect.Offset(-intRect.Min);
			this.StartBatchTileUpdate(false);
			NavmeshTile[] array2 = new NavmeshTile[width * height];
			for (int i = 0; i < height; i++)
			{
				for (int j = 0; j < width; j++)
				{
					TileMesh tileMesh = tileMeshes.tileMeshes[j + i * width];
					Int3 @int = (Int3)new Vector3(0f, yOffset, 0f);
					for (int k = 0; k < tileMesh.verticesInTileSpace.Length; k++)
					{
						tileMesh.verticesInTileSpace[k] += @int;
					}
					Vector2Int vector2Int = new Vector2Int(j, i) + tileMeshes.tileRect.Min;
					this.ReplaceTile(vector2Int.x, vector2Int.y, tileMesh.verticesInTileSpace, tileMesh.triangles, tileMesh.tags, false);
					array2[j + i * width] = this.GetTile(vector2Int.x, vector2Int.y);
				}
			}
			this.EndBatchTileUpdate();
			if (this.OnRecalculatedTiles != null)
			{
				this.OnRecalculatedTiles(array2);
			}
		}

		// Token: 0x060007E0 RID: 2016 RVA: 0x000288BC File Offset: 0x00026ABC
		public override void OnDrawGizmos(DrawingData gizmos, bool drawNodes, RedrawScope redrawScope, bool renderInGame)
		{
			if (!drawNodes)
			{
				return;
			}
			using (CommandBuilder builder = gizmos.GetBuilder(redrawScope, renderInGame))
			{
				Bounds bounds = default(Bounds);
				bounds.SetMinMax(Vector3.zero, this.forcedBoundsSize);
				using (builder.WithMatrix(this.CalculateTransform().matrix))
				{
					builder.WireBox(bounds, Color.white);
				}
			}
			if (this.tiles != null && (this.showMeshSurface || this.showMeshOutline || this.showNodeConnections))
			{
				NodeHasher nodeHasher = new NodeHasher(this.active);
				nodeHasher.Add<int>(this.showMeshOutline ? 1 : 0);
				nodeHasher.Add<int>(this.showMeshSurface ? 1 : 0);
				nodeHasher.Add<int>(this.showNodeConnections ? 1 : 0);
				int num = 0;
				NodeHasher nodeHasher2 = nodeHasher;
				int num2 = 0;
				for (int i = 0; i < this.tiles.Length; i++)
				{
					if (this.tiles[i] != null)
					{
						TriangleMeshNode[] nodes = this.tiles[i].nodes;
						for (int j = 0; j < nodes.Length; j++)
						{
							nodeHasher2.HashNode(nodes[j]);
						}
						num2 += nodes.Length;
						if (num2 > 1024 || i % this.tileXCount == this.tileXCount - 1 || i == this.tiles.Length - 1)
						{
							if (!gizmos.Draw(nodeHasher2, redrawScope))
							{
								using (GraphGizmoHelper gizmoHelper = GraphGizmoHelper.GetGizmoHelper(gizmos, this.active, nodeHasher2, redrawScope, renderInGame))
								{
									if (this.showMeshSurface || this.showMeshOutline)
									{
										this.CreateNavmeshSurfaceVisualization(this.tiles, num, i + 1, gizmoHelper);
										NavmeshBase.CreateNavmeshOutlineVisualization(this.tiles, num, i + 1, gizmoHelper);
									}
									if (this.showNodeConnections)
									{
										if (gizmoHelper.showSearchTree)
										{
											gizmoHelper.builder.PushLineWidth(2f, true);
										}
										for (int k = num; k <= i; k++)
										{
											if (this.tiles[k] != null)
											{
												TriangleMeshNode[] nodes2 = this.tiles[k].nodes;
												for (int l = 0; l < nodes2.Length; l++)
												{
													gizmoHelper.DrawConnections(nodes2[l]);
												}
											}
										}
										if (gizmoHelper.showSearchTree)
										{
											gizmoHelper.builder.PopLineWidth();
										}
									}
								}
							}
							num = i + 1;
							nodeHasher2 = nodeHasher;
							num2 = 0;
						}
					}
				}
			}
			if (this.active.showUnwalkableNodes)
			{
				base.DrawUnwalkableNodes(gizmos, this.active.unwalkableNodeDebugSize, redrawScope, renderInGame);
			}
		}

		// Token: 0x060007E1 RID: 2017 RVA: 0x00028B80 File Offset: 0x00026D80
		private void CreateNavmeshSurfaceVisualization(NavmeshTile[] tiles, int startTile, int endTile, GraphGizmoHelper helper)
		{
			int num = 0;
			for (int i = startTile; i < endTile; i++)
			{
				if (tiles[i] != null)
				{
					num += tiles[i].nodes.Length;
				}
			}
			Vector3[] array = ArrayPool<Vector3>.Claim(num * 3);
			Color[] array2 = ArrayPool<Color>.Claim(num * 3);
			int num2 = 0;
			for (int j = startTile; j < endTile; j++)
			{
				NavmeshTile navmeshTile = tiles[j];
				if (navmeshTile != null)
				{
					for (int k = 0; k < navmeshTile.nodes.Length; k++)
					{
						TriangleMeshNode triangleMeshNode = navmeshTile.nodes[k];
						Int3 @int;
						Int3 int2;
						Int3 int3;
						triangleMeshNode.GetVertices(out @int, out int2, out int3);
						int num3 = num2 + k * 3;
						array[num3] = (Vector3)@int;
						array[num3 + 1] = (Vector3)int2;
						array[num3 + 2] = (Vector3)int3;
						Color color = helper.NodeColor(triangleMeshNode);
						array2[num3] = (array2[num3 + 1] = (array2[num3 + 2] = color));
					}
					num2 += navmeshTile.nodes.Length * 3;
				}
			}
			if (this.showMeshSurface)
			{
				helper.DrawTriangles(array, array2, num);
			}
			if (this.showMeshOutline)
			{
				helper.DrawWireTriangles(array, array2, num);
			}
			ArrayPool<Vector3>.Release(ref array, false);
			ArrayPool<Color>.Release(ref array2, false);
		}

		// Token: 0x060007E2 RID: 2018 RVA: 0x00028CD0 File Offset: 0x00026ED0
		private static void CreateNavmeshOutlineVisualization(NavmeshTile[] tiles, int startTile, int endTile, GraphGizmoHelper helper)
		{
			bool[] array = new bool[3];
			for (int i = startTile; i < endTile; i++)
			{
				NavmeshTile navmeshTile = tiles[i];
				if (navmeshTile != null)
				{
					for (int j = 0; j < navmeshTile.nodes.Length; j++)
					{
						array[0] = (array[1] = (array[2] = false));
						TriangleMeshNode triangleMeshNode = navmeshTile.nodes[j];
						if (triangleMeshNode.connections != null)
						{
							for (int k = 0; k < triangleMeshNode.connections.Length; k++)
							{
								TriangleMeshNode triangleMeshNode2 = triangleMeshNode.connections[k].node as TriangleMeshNode;
								if (triangleMeshNode2 != null && triangleMeshNode2.GraphIndex == triangleMeshNode.GraphIndex)
								{
									for (int l = 0; l < 3; l++)
									{
										for (int m = 0; m < 3; m++)
										{
											if (triangleMeshNode.GetVertexIndex(l) == triangleMeshNode2.GetVertexIndex((m + 1) % 3) && triangleMeshNode.GetVertexIndex((l + 1) % 3) == triangleMeshNode2.GetVertexIndex(m))
											{
												array[l] = true;
												l = 3;
												break;
											}
										}
									}
								}
							}
						}
						Color color = helper.NodeColor(triangleMeshNode);
						for (int n = 0; n < 3; n++)
						{
							if (!array[n])
							{
								helper.builder.Line((Vector3)triangleMeshNode.GetVertex(n), (Vector3)triangleMeshNode.GetVertex((n + 1) % 3), color);
							}
						}
					}
				}
			}
		}

		// Token: 0x060007E3 RID: 2019 RVA: 0x00028E38 File Offset: 0x00027038
		protected unsafe override void SerializeExtraInfo(GraphSerializationContext ctx)
		{
			BinaryWriter writer = ctx.writer;
			if (this.tiles == null)
			{
				writer.Write(-1);
				return;
			}
			writer.Write(this.tileXCount);
			writer.Write(this.tileZCount);
			for (int i = 0; i < this.tileZCount; i++)
			{
				for (int j = 0; j < this.tileXCount; j++)
				{
					NavmeshTile navmeshTile = this.tiles[j + i * this.tileXCount];
					if (navmeshTile == null)
					{
						throw new NullReferenceException();
					}
					writer.Write(navmeshTile.x);
					writer.Write(navmeshTile.z);
					if (navmeshTile.x == j && navmeshTile.z == i)
					{
						writer.Write(navmeshTile.w);
						writer.Write(navmeshTile.d);
						writer.Write(navmeshTile.tris.Length);
						for (int k = 0; k < navmeshTile.tris.Length; k++)
						{
							writer.Write(*navmeshTile.tris[k]);
						}
						writer.Write(navmeshTile.verts.Length);
						for (int l = 0; l < navmeshTile.verts.Length; l++)
						{
							ctx.SerializeInt3(*navmeshTile.verts[l]);
						}
						writer.Write(navmeshTile.vertsInGraphSpace.Length);
						for (int m = 0; m < navmeshTile.vertsInGraphSpace.Length; m++)
						{
							ctx.SerializeInt3(*navmeshTile.vertsInGraphSpace[m]);
						}
						if (navmeshTile.isCut)
						{
							writer.Write(true);
							writer.Write(navmeshTile.preCutTags.Length);
							for (int n = 0; n < navmeshTile.preCutTags.Length; n++)
							{
								writer.Write(*navmeshTile.preCutTags[n]);
							}
							writer.Write(navmeshTile.preCutVertsInTileSpace.Length);
							for (int num = 0; num < navmeshTile.preCutVertsInTileSpace.Length; num++)
							{
								ctx.SerializeInt3(*navmeshTile.preCutVertsInTileSpace[num]);
							}
							writer.Write(navmeshTile.preCutTris.Length);
							for (int num2 = 0; num2 < navmeshTile.preCutTris.Length; num2++)
							{
								writer.Write(*navmeshTile.preCutTris[num2]);
							}
						}
						else
						{
							writer.Write(false);
						}
						writer.Write(navmeshTile.nodes.Length);
						for (int num3 = 0; num3 < navmeshTile.nodes.Length; num3++)
						{
							navmeshTile.nodes[num3].SerializeNode(ctx);
						}
					}
				}
			}
		}

		// Token: 0x060007E4 RID: 2020 RVA: 0x000290DC File Offset: 0x000272DC
		protected unsafe override void DeserializeExtraInfo(GraphSerializationContext ctx)
		{
			BinaryReader reader = ctx.reader;
			this.tileXCount = reader.ReadInt32();
			if (this.tileXCount < 0)
			{
				return;
			}
			this.tileZCount = reader.ReadInt32();
			this.transform = this.CalculateTransform();
			this.tiles = new NavmeshTile[this.tileXCount * this.tileZCount];
			TriangleMeshNode.SetNavmeshHolder((int)ctx.graphIndex, this);
			for (int i = 0; i < this.tileZCount; i++)
			{
				for (int j = 0; j < this.tileXCount; j++)
				{
					int num = j + i * this.tileXCount;
					int num2 = reader.ReadInt32();
					if (num2 < 0)
					{
						throw new Exception("Invalid tile coordinates (x < 0)");
					}
					int num3 = reader.ReadInt32();
					if (num3 < 0)
					{
						throw new Exception("Invalid tile coordinates (z < 0)");
					}
					if (num2 != j || num3 != i)
					{
						this.tiles[num] = this.tiles[num3 * this.tileXCount + num2];
					}
					else
					{
						NavmeshTile[] array = this.tiles;
						int num4 = num;
						NavmeshTile navmeshTile = new NavmeshTile();
						navmeshTile.x = num2;
						navmeshTile.z = num3;
						navmeshTile.w = reader.ReadInt32();
						navmeshTile.d = reader.ReadInt32();
						navmeshTile.bbTree = default(BBTree);
						navmeshTile.graph = this;
						navmeshTile.isCut = false;
						NavmeshTile navmeshTile2 = navmeshTile;
						array[num4] = navmeshTile;
						NavmeshTile navmeshTile3 = navmeshTile2;
						navmeshTile3.tris = ctx.ReadSpan<int>(Allocator.Persistent);
						if (navmeshTile3.tris.Length % 3 != 0)
						{
							throw new Exception("Corrupt data. Triangle indices count must be divisable by 3. Read " + navmeshTile3.tris.Length.ToString());
						}
						navmeshTile3.verts = ctx.ReadSpan<Int3>(Allocator.Persistent);
						if (ctx.meta.version.Major >= 4)
						{
							navmeshTile3.vertsInGraphSpace = ctx.ReadSpan<Int3>(Allocator.Persistent);
							if (navmeshTile3.vertsInGraphSpace.Length != navmeshTile3.verts.Length)
							{
								throw new Exception("Corrupt data. Array lengths did not match");
							}
						}
						else
						{
							navmeshTile3.vertsInGraphSpace = new UnsafeSpan<Int3>(Allocator.Persistent, navmeshTile3.verts.Length);
							navmeshTile3.verts.CopyTo(navmeshTile3.vertsInGraphSpace);
							this.transform.InverseTransform(navmeshTile3.vertsInGraphSpace);
						}
						if (ctx.meta.version >= AstarSerializer.V5_2_0)
						{
							navmeshTile3.isCut = reader.ReadBoolean();
							if (navmeshTile3.isCut)
							{
								navmeshTile3.preCutTags = ctx.ReadSpan<uint>(Allocator.Persistent);
								navmeshTile3.preCutVertsInTileSpace = ctx.ReadSpan<Int3>(Allocator.Persistent);
								navmeshTile3.preCutTris = ctx.ReadSpan<int>(Allocator.Persistent);
							}
						}
						int num5 = reader.ReadInt32();
						navmeshTile3.nodes = new TriangleMeshNode[num5];
						num <<= 12;
						for (int k = 0; k < navmeshTile3.nodes.Length; k++)
						{
							TriangleMeshNode triangleMeshNode = new TriangleMeshNode(this.active);
							navmeshTile3.nodes[k] = triangleMeshNode;
							triangleMeshNode.DeserializeNode(ctx);
							triangleMeshNode.v0 = *navmeshTile3.tris[k * 3] | num;
							triangleMeshNode.v1 = *navmeshTile3.tris[k * 3 + 1] | num;
							triangleMeshNode.v2 = *navmeshTile3.tris[k * 3 + 2] | num;
							triangleMeshNode.UpdatePositionFromVertices();
						}
						navmeshTile3.bbTree = new BBTree(navmeshTile3.tris, navmeshTile3.vertsInGraphSpace);
					}
				}
			}
		}

		// Token: 0x060007E5 RID: 2021 RVA: 0x00029424 File Offset: 0x00027624
		protected override void PostDeserialization(GraphSerializationContext ctx)
		{
			if (ctx.meta.version < AstarSerializer.V4_3_68 && this.tiles != null)
			{
				Dictionary<TriangleMeshNode, Connection[]> conns = this.tiles.SelectMany<NavmeshTile, TriangleMeshNode>((NavmeshTile s) => s.nodes).ToDictionary<TriangleMeshNode, TriangleMeshNode, Connection[]>((TriangleMeshNode n) => n, (TriangleMeshNode n) => n.connections ?? new Connection[0]);
				NavmeshTile[] array = this.tiles;
				for (int i = 0; i < array.Length; i++)
				{
					NavmeshBase.CreateNodeConnections(array[i].nodes, false);
				}
				foreach (NavmeshTile navmeshTile in this.tiles)
				{
					this.ConnectTileWithNeighbours(navmeshTile, false);
				}
				this.GetNodes(delegate(GraphNode node)
				{
					TriangleMeshNode triNode = node as TriangleMeshNode;
					IEnumerable<Connection> enumerable = conns[triNode];
					Func<Connection, bool> <>9__4;
					Func<Connection, bool> func;
					if ((func = <>9__4) == null)
					{
						func = (<>9__4 = (Connection conn) => !triNode.ContainsOutgoingConnection(conn.node));
					}
					foreach (Connection connection in enumerable.Where<Connection>(func).ToList<Connection>())
					{
						triNode.AddPartialConnection(connection.node, connection.cost, connection.shapeEdgeInfo);
					}
				});
			}
			this.transform = this.CalculateTransform();
			if (this.enableNavmeshCutting && this.isScanned)
			{
				this.navmeshUpdateData.Enable();
			}
		}

		// Token: 0x040004E5 RID: 1253
		public const int VertexIndexMask = 4095;

		// Token: 0x040004E6 RID: 1254
		public const int TileIndexMask = 524287;

		// Token: 0x040004E7 RID: 1255
		public const int TileIndexOffset = 12;

		// Token: 0x040004E8 RID: 1256
		[JsonMember]
		public Vector3 forcedBoundsSize = new Vector3(100f, 40f, 100f);

		// Token: 0x040004E9 RID: 1257
		[JsonMember]
		public bool showMeshOutline = true;

		// Token: 0x040004EA RID: 1258
		[JsonMember]
		public bool showNodeConnections;

		// Token: 0x040004EB RID: 1259
		[JsonMember]
		public bool showMeshSurface = true;

		// Token: 0x040004EC RID: 1260
		public int tileXCount;

		// Token: 0x040004ED RID: 1261
		public int tileZCount;

		// Token: 0x040004EE RID: 1262
		protected NavmeshTile[] tiles;

		// Token: 0x040004EF RID: 1263
		[JsonMember]
		[Obsolete("Set the appropriate fields on the NearestNodeConstraint instead")]
		public bool nearestSearchOnlyXZ;

		// Token: 0x040004F0 RID: 1264
		[JsonMember]
		public bool enableNavmeshCutting = true;

		// Token: 0x040004F1 RID: 1265
		public NavmeshUpdates.NavmeshUpdateSettings navmeshUpdateData;

		// Token: 0x040004F2 RID: 1266
		private int batchTileUpdate;

		// Token: 0x040004F3 RID: 1267
		private bool batchPendingNavmeshCutting;

		// Token: 0x040004F4 RID: 1268
		private List<int> batchUpdatedTiles = new List<int>();

		// Token: 0x040004F5 RID: 1269
		private List<MeshNode> batchNodesToDestroy = new List<MeshNode>();

		// Token: 0x040004F6 RID: 1270
		public GraphTransform transform = GraphTransform.identityTransform;

		// Token: 0x040004F7 RID: 1271
		public Action<NavmeshTile[]> OnRecalculatedTiles;

		// Token: 0x040004F8 RID: 1272
		private Dictionary<int, int> nodeRecyclingHashBuffer = new Dictionary<int, int>();

		// Token: 0x040004F9 RID: 1273
		private static readonly byte[] LinecastShapeEdgeLookup = new byte[64];
	}
}
