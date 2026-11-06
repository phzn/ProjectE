using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Pathfinding.Graphs.Navmesh;
using Pathfinding.Graphs.Navmesh.Jobs;
using Pathfinding.Serialization;
using Pathfinding.Sync;
using Pathfinding.Util;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;

namespace Pathfinding
{
	// Token: 0x020000E6 RID: 230
	[JsonOptIn]
	[Preserve]
	public class NavMeshGraph : NavmeshBase, IUpdatableGraph
	{
		// Token: 0x17000173 RID: 371
		// (get) Token: 0x060007F1 RID: 2033 RVA: 0x0002968E File Offset: 0x0002788E
		public override float NavmeshCuttingCharacterRadius
		{
			get
			{
				return this.navmeshCuttingCharacterRadius;
			}
		}

		// Token: 0x17000174 RID: 372
		// (get) Token: 0x060007F2 RID: 2034 RVA: 0x00029696 File Offset: 0x00027896
		public override bool RecalculateNormals
		{
			get
			{
				return this.recalculateNormals;
			}
		}

		// Token: 0x17000175 RID: 373
		// (get) Token: 0x060007F3 RID: 2035 RVA: 0x0002969E File Offset: 0x0002789E
		public override float TileWorldSizeX
		{
			get
			{
				return this.forcedBoundsSize.x;
			}
		}

		// Token: 0x17000176 RID: 374
		// (get) Token: 0x060007F4 RID: 2036 RVA: 0x000296AB File Offset: 0x000278AB
		public override float TileWorldSizeZ
		{
			get
			{
				return this.forcedBoundsSize.z;
			}
		}

		// Token: 0x17000177 RID: 375
		// (get) Token: 0x060007F5 RID: 2037 RVA: 0x00005C0D File Offset: 0x00003E0D
		public override float MaxTileConnectionEdgeDistance
		{
			get
			{
				return 0f;
			}
		}

		// Token: 0x060007F6 RID: 2038 RVA: 0x000296B8 File Offset: 0x000278B8
		public override bool IsInsideBounds(Vector3 point)
		{
			if (this.tiles == null || this.tiles.Length == 0 || this.sourceMesh == null)
			{
				return false;
			}
			Vector3 vector = this.transform.InverseTransform(point);
			Vector3 vector2 = this.sourceMesh.bounds.size * this.scale;
			return vector.x >= -0.0001f && vector.y >= -0.0001f && vector.z >= -0.0001f && vector.x <= vector2.x + 0.0001f && vector.y <= vector2.y + 0.0001f && vector.z <= vector2.z + 0.0001f;
		}

		// Token: 0x17000178 RID: 376
		// (get) Token: 0x060007F7 RID: 2039 RVA: 0x0002977C File Offset: 0x0002797C
		public override Bounds bounds
		{
			get
			{
				if (this.sourceMesh == null)
				{
					return default(Bounds);
				}
				float4x4 float4x = this.CalculateTransform().matrix;
				return new ToWorldMatrix(new float3x3(float4x.c0.xyz, float4x.c1.xyz, float4x.c2.xyz)).ToWorld(new Bounds(Vector3.zero, this.sourceMesh.bounds.size * this.scale));
			}
		}

		// Token: 0x060007F8 RID: 2040 RVA: 0x00029810 File Offset: 0x00027A10
		public override GraphTransform CalculateTransform()
		{
			return new GraphTransform(Matrix4x4.TRS(this.offset, Quaternion.Euler(this.rotation), Vector3.one) * Matrix4x4.TRS((this.sourceMesh != null) ? (this.sourceMesh.bounds.min * this.scale) : (this.cachedSourceMeshBoundsMin * this.scale), Quaternion.identity, Vector3.one));
		}

		// Token: 0x060007F9 RID: 2041 RVA: 0x00029890 File Offset: 0x00027A90
		IGraphUpdatePromise IUpdatableGraph.ScheduleGraphUpdates(List<GraphUpdateObject> graphUpdates)
		{
			return new NavMeshGraph.NavMeshGraphUpdatePromise
			{
				graph = this,
				graphUpdates = graphUpdates
			};
		}

		// Token: 0x060007FA RID: 2042 RVA: 0x000298A8 File Offset: 0x00027AA8
		public static void UpdateArea(GraphUpdateObject o, INavmeshHolder graph)
		{
			Bounds bounds = graph.transform.InverseTransform(o.bounds);
			IntRect irect = new IntRect(Mathf.FloorToInt(bounds.min.x * 1000f), Mathf.FloorToInt(bounds.min.z * 1000f), Mathf.CeilToInt(bounds.max.x * 1000f), Mathf.CeilToInt(bounds.max.z * 1000f));
			Int3 a = new Int3(irect.xmin, 0, irect.ymin);
			Int3 b = new Int3(irect.xmin, 0, irect.ymax);
			Int3 c = new Int3(irect.xmax, 0, irect.ymin);
			Int3 d = new Int3(irect.xmax, 0, irect.ymax);
			int ymin = ((Int3)bounds.min).y;
			int ymax = ((Int3)bounds.max).y;
			graph.GetNodes(delegate(GraphNode _node)
			{
				TriangleMeshNode triangleMeshNode = _node as TriangleMeshNode;
				bool flag = false;
				int num = 0;
				int num2 = 0;
				int num3 = 0;
				int num4 = 0;
				for (int i = 0; i < 3; i++)
				{
					Int3 vertexInGraphSpace = triangleMeshNode.GetVertexInGraphSpace(i);
					if (irect.Contains(vertexInGraphSpace.x, vertexInGraphSpace.z))
					{
						flag = true;
						break;
					}
					if (vertexInGraphSpace.x < irect.xmin)
					{
						num++;
					}
					if (vertexInGraphSpace.x > irect.xmax)
					{
						num2++;
					}
					if (vertexInGraphSpace.z < irect.ymin)
					{
						num3++;
					}
					if (vertexInGraphSpace.z > irect.ymax)
					{
						num4++;
					}
				}
				if (!flag && (num == 3 || num2 == 3 || num3 == 3 || num4 == 3))
				{
					return;
				}
				for (int j = 0; j < 3; j++)
				{
					int num5 = ((j > 1) ? 0 : (j + 1));
					Int3 vertexInGraphSpace2 = triangleMeshNode.GetVertexInGraphSpace(j);
					Int3 vertexInGraphSpace3 = triangleMeshNode.GetVertexInGraphSpace(num5);
					if (VectorMath.SegmentsIntersectXZ(a, b, vertexInGraphSpace2, vertexInGraphSpace3))
					{
						flag = true;
						break;
					}
					if (VectorMath.SegmentsIntersectXZ(a, c, vertexInGraphSpace2, vertexInGraphSpace3))
					{
						flag = true;
						break;
					}
					if (VectorMath.SegmentsIntersectXZ(c, d, vertexInGraphSpace2, vertexInGraphSpace3))
					{
						flag = true;
						break;
					}
					if (VectorMath.SegmentsIntersectXZ(d, b, vertexInGraphSpace2, vertexInGraphSpace3))
					{
						flag = true;
						break;
					}
				}
				if (flag || triangleMeshNode.ContainsPointInGraphSpace(a) || triangleMeshNode.ContainsPointInGraphSpace(b) || triangleMeshNode.ContainsPointInGraphSpace(c) || triangleMeshNode.ContainsPointInGraphSpace(d))
				{
					flag = true;
				}
				if (!flag)
				{
					return;
				}
				int num6 = 0;
				int num7 = 0;
				for (int k = 0; k < 3; k++)
				{
					Int3 vertexInGraphSpace4 = triangleMeshNode.GetVertexInGraphSpace(k);
					if (vertexInGraphSpace4.y < ymin)
					{
						num7++;
					}
					if (vertexInGraphSpace4.y > ymax)
					{
						num6++;
					}
				}
				if (num7 == 3 || num6 == 3)
				{
					return;
				}
				o.WillUpdateNode(triangleMeshNode);
				o.Apply(triangleMeshNode);
			});
		}

		// Token: 0x060007FB RID: 2043 RVA: 0x00029A08 File Offset: 0x00027C08
		protected override IGraphUpdatePromise ScanInternal(bool async)
		{
			return new NavMeshGraph.NavMeshGraphScanPromise
			{
				graph = this
			};
		}

		// Token: 0x060007FC RID: 2044 RVA: 0x00029A16 File Offset: 0x00027C16
		protected override void PostDeserialization(GraphSerializationContext ctx)
		{
			if (ctx.meta.version < AstarSerializer.V4_3_74)
			{
				this.navmeshCuttingCharacterRadius = 0f;
			}
			base.PostDeserialization(ctx);
		}

		// Token: 0x04000503 RID: 1283
		[JsonMember]
		public Mesh sourceMesh;

		// Token: 0x04000504 RID: 1284
		[JsonMember]
		public Vector3 offset;

		// Token: 0x04000505 RID: 1285
		[JsonMember]
		public Vector3 rotation;

		// Token: 0x04000506 RID: 1286
		[JsonMember]
		public float scale = 1f;

		// Token: 0x04000507 RID: 1287
		[JsonMember]
		public bool recalculateNormals = true;

		// Token: 0x04000508 RID: 1288
		[JsonMember]
		private Vector3 cachedSourceMeshBoundsMin;

		// Token: 0x04000509 RID: 1289
		[JsonMember]
		public float navmeshCuttingCharacterRadius = 0.5f;

		// Token: 0x020000E7 RID: 231
		private class NavMeshGraphUpdatePromise : IGraphUpdatePromise
		{
			// Token: 0x060007FE RID: 2046 RVA: 0x00029A68 File Offset: 0x00027C68
			public void Apply(IGraphUpdateContext ctx)
			{
				for (int i = 0; i < this.graphUpdates.Count; i++)
				{
					GraphUpdateObject graphUpdateObject = this.graphUpdates[i];
					NavMeshGraph.UpdateArea(graphUpdateObject, this.graph);
					ctx.DirtyBounds(graphUpdateObject.bounds);
				}
			}

			// Token: 0x0400050A RID: 1290
			public NavMeshGraph graph;

			// Token: 0x0400050B RID: 1291
			public List<GraphUpdateObject> graphUpdates;
		}

		// Token: 0x020000E8 RID: 232
		private class NavMeshGraphScanPromise : IGraphUpdatePromise
		{
			// Token: 0x06000800 RID: 2048 RVA: 0x00029AB0 File Offset: 0x00027CB0
			public IEnumerator<JobHandle> Prepare()
			{
				Mesh sourceMesh = this.graph.sourceMesh;
				this.graph.cachedSourceMeshBoundsMin = ((sourceMesh != null) ? sourceMesh.bounds.min : Vector3.zero);
				this.transform = this.graph.CalculateTransform();
				if (sourceMesh == null)
				{
					this.emptyGraph = true;
					yield break;
				}
				if (!sourceMesh.isReadable)
				{
					Debug.LogError("The source mesh " + sourceMesh.name + " is not readable. Enable Read/Write in the mesh's import settings.", sourceMesh);
					this.emptyGraph = true;
					yield break;
				}
				Mesh.MeshDataArray meshDataArray = Mesh.AcquireReadOnlyMeshData(sourceMesh);
				NativeArray<Vector3> vertices;
				NativeArray<int> indices;
				MeshUtility.GetMeshData(meshDataArray, 0, out vertices, out indices);
				meshDataArray.Dispose();
				float scale = this.graph.scale;
				Matrix4x4 matrix4x = Matrix4x4.TRS(-sourceMesh.bounds.min * scale, Quaternion.identity, Vector3.one * scale);
				Promise<TileBuilder.TileBuilderOutput> promise = JobBuildTileMeshFromVertices.Schedule(vertices, indices, matrix4x, this.graph.RecalculateNormals);
				this.forcedBoundsSize = sourceMesh.bounds.size * scale;
				this.tileRect = new IntRect(0, 0, 0, 0);
				this.tiles = new NavmeshTile[this.tileRect.Area];
				GCHandle tilesGCHandle = GCHandle.Alloc(this.tiles);
				TileLayout tileLayout = new TileLayout(new Bounds(this.transform.Transform(this.forcedBoundsSize * 0.5f), this.forcedBoundsSize), Quaternion.Euler(this.graph.rotation), 0.001f, 0, false);
				this.cutSettings = new NavmeshUpdates.NavmeshUpdateSettings(this.graph, tileLayout);
				Promise<TileCutter.TileCutterOutput> cutPromise = RecastBuilder.CutTiles(this.graph, this.cutSettings.clipperLookup, tileLayout).Schedule(promise);
				NativeArray<JobCalculateTriangleConnections.TileNodeConnectionsUnsafe> tileNodeConnections = new NativeArray<JobCalculateTriangleConnections.TileNodeConnectionsUnsafe>(this.tiles.Length, Allocator.Persistent, NativeArrayOptions.ClearMemory);
				TileCutter.TileCutterOutput value = cutPromise.GetValue();
				TileBuilder.TileBuilderOutput value2 = promise.GetValue();
				NativeArray<TileMesh.TileMeshUnsafe> nativeArray;
				if (value.tileMeshes.tileMeshes.IsCreated)
				{
					nativeArray = value.tileMeshes.tileMeshes;
				}
				else
				{
					nativeArray = value2.tileMeshes.tileMeshes;
				}
				JobHandle jobHandle = new JobCalculateTriangleConnections
				{
					tileMeshes = nativeArray,
					nodeConnections = tileNodeConnections
				}.Schedule(cutPromise.handle);
				JobHandle jobHandle2 = new JobCreateTiles
				{
					preCutTileMeshes = (value.tileMeshes.tileMeshes.IsCreated ? value2.tileMeshes.tileMeshes : default(NativeArray<TileMesh.TileMeshUnsafe>)),
					tileMeshes = nativeArray,
					tiles = tilesGCHandle,
					tileRect = this.tileRect,
					graphTileCount = new Vector2Int(this.tileRect.Width, this.tileRect.Height),
					graphIndex = this.graph.graphIndex,
					initialPenalty = this.graph.initialPenalty,
					recalculateNormals = this.graph.recalculateNormals,
					graphToWorldSpace = this.transform.matrix,
					tileWorldSize = tileLayout.TileWorldSize
				}.Schedule(cutPromise.handle);
				JobHandle jobHandle3 = new JobWriteNodeConnections
				{
					tiles = tilesGCHandle,
					nodeConnections = tileNodeConnections
				}.Schedule(JobHandle.CombineDependencies(jobHandle2, jobHandle));
				yield return jobHandle3;
				promise.Complete().Dispose();
				cutPromise.Complete().Dispose();
				tileNodeConnections.Dispose();
				vertices.Dispose();
				indices.Dispose();
				tilesGCHandle.Free();
				yield break;
			}

			// Token: 0x06000801 RID: 2049 RVA: 0x00029AC0 File Offset: 0x00027CC0
			public void Apply(IGraphUpdateContext ctx)
			{
				this.graph.DestroyAllNodes();
				if (this.emptyGraph)
				{
					this.graph.forcedBoundsSize = Vector3.zero;
					this.graph.transform = this.transform;
					this.graph.tileZCount = (this.graph.tileXCount = 1);
					TriangleMeshNode.SetNavmeshHolder(AstarPath.active.data.GetGraphIndex(this.graph), this.graph);
					this.graph.FillWithEmptyTiles();
					this.graph.navmeshUpdateData.Dispose();
					return;
				}
				for (int i = 0; i < this.tiles.Length; i++)
				{
					AstarPath active = AstarPath.active;
					GraphNode[] nodes = this.tiles[i].nodes;
					active.InitializeNodes(nodes);
				}
				this.graph.forcedBoundsSize = this.forcedBoundsSize;
				this.graph.transform = this.transform;
				this.graph.tileXCount = this.tileRect.Width;
				this.graph.tileZCount = this.tileRect.Height;
				this.graph.tiles = this.tiles;
				TriangleMeshNode.SetNavmeshHolder(this.graph.active.data.GetGraphIndex(this.graph), this.graph);
				this.cutSettings.AttachToGraph();
				if (this.graph.OnRecalculatedTiles != null)
				{
					this.graph.OnRecalculatedTiles(this.tiles.Clone() as NavmeshTile[]);
				}
			}

			// Token: 0x0400050C RID: 1292
			public NavMeshGraph graph;

			// Token: 0x0400050D RID: 1293
			private bool emptyGraph;

			// Token: 0x0400050E RID: 1294
			private GraphTransform transform;

			// Token: 0x0400050F RID: 1295
			private NavmeshTile[] tiles;

			// Token: 0x04000510 RID: 1296
			private Vector3 forcedBoundsSize;

			// Token: 0x04000511 RID: 1297
			private IntRect tileRect;

			// Token: 0x04000512 RID: 1298
			private NavmeshUpdates.NavmeshUpdateSettings cutSettings;
		}
	}
}
