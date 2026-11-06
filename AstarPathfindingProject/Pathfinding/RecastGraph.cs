using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Pathfinding.Collections;
using Pathfinding.Graphs.Navmesh;
using Pathfinding.Graphs.Navmesh.Jobs;
using Pathfinding.Jobs;
using Pathfinding.Pooling;
using Pathfinding.Serialization;
using Pathfinding.Sync;
using Pathfinding.Util;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Profiling;
using UnityEngine;

namespace Pathfinding
{
	// Token: 0x02000102 RID: 258
	[JsonOptIn]
	[Preserve]
	public class RecastGraph : NavmeshBase, IUpdatableGraph
	{
		// Token: 0x17000195 RID: 405
		// (get) Token: 0x06000918 RID: 2328 RVA: 0x0002F18B File Offset: 0x0002D38B
		// (set) Token: 0x06000919 RID: 2329 RVA: 0x0002F198 File Offset: 0x0002D398
		[Obsolete("Use collectionSettings.rasterizeColliders instead")]
		public bool rasterizeColliders
		{
			get
			{
				return this.collectionSettings.rasterizeColliders;
			}
			set
			{
				this.collectionSettings.rasterizeColliders = value;
			}
		}

		// Token: 0x17000196 RID: 406
		// (get) Token: 0x0600091A RID: 2330 RVA: 0x0002F1A6 File Offset: 0x0002D3A6
		// (set) Token: 0x0600091B RID: 2331 RVA: 0x0002F1B3 File Offset: 0x0002D3B3
		[Obsolete("Use collectionSettings.rasterizeMeshes instead")]
		public bool rasterizeMeshes
		{
			get
			{
				return this.collectionSettings.rasterizeMeshes;
			}
			set
			{
				this.collectionSettings.rasterizeMeshes = value;
			}
		}

		// Token: 0x17000197 RID: 407
		// (get) Token: 0x0600091C RID: 2332 RVA: 0x0002F1C1 File Offset: 0x0002D3C1
		// (set) Token: 0x0600091D RID: 2333 RVA: 0x0002F1CE File Offset: 0x0002D3CE
		[Obsolete("Use collectionSettings.rasterizeTerrain instead")]
		public bool rasterizeTerrain
		{
			get
			{
				return this.collectionSettings.rasterizeTerrain;
			}
			set
			{
				this.collectionSettings.rasterizeTerrain = value;
			}
		}

		// Token: 0x17000198 RID: 408
		// (get) Token: 0x0600091E RID: 2334 RVA: 0x0002F1DC File Offset: 0x0002D3DC
		// (set) Token: 0x0600091F RID: 2335 RVA: 0x0002F1E9 File Offset: 0x0002D3E9
		[Obsolete("Use collectionSettings.rasterizeTrees instead")]
		public bool rasterizeTrees
		{
			get
			{
				return this.collectionSettings.rasterizeTrees;
			}
			set
			{
				this.collectionSettings.rasterizeTrees = value;
			}
		}

		// Token: 0x17000199 RID: 409
		// (get) Token: 0x06000920 RID: 2336 RVA: 0x0002F1F7 File Offset: 0x0002D3F7
		// (set) Token: 0x06000921 RID: 2337 RVA: 0x0002F204 File Offset: 0x0002D404
		[Obsolete("Use collectionSettings.colliderRasterizeDetail instead")]
		public float colliderRasterizeDetail
		{
			get
			{
				return this.collectionSettings.colliderRasterizeDetail;
			}
			set
			{
				this.collectionSettings.colliderRasterizeDetail = value;
			}
		}

		// Token: 0x1700019A RID: 410
		// (get) Token: 0x06000922 RID: 2338 RVA: 0x0002F212 File Offset: 0x0002D412
		// (set) Token: 0x06000923 RID: 2339 RVA: 0x0002F21F File Offset: 0x0002D41F
		[Obsolete("Use collectionSettings.layerMask instead")]
		public LayerMask mask
		{
			get
			{
				return this.collectionSettings.layerMask;
			}
			set
			{
				this.collectionSettings.layerMask = value;
			}
		}

		// Token: 0x1700019B RID: 411
		// (get) Token: 0x06000924 RID: 2340 RVA: 0x0002F22D File Offset: 0x0002D42D
		// (set) Token: 0x06000925 RID: 2341 RVA: 0x0002F23A File Offset: 0x0002D43A
		[Obsolete("Use collectionSettings.tagMask instead")]
		public List<string> tagMask
		{
			get
			{
				return this.collectionSettings.tagMask;
			}
			set
			{
				this.collectionSettings.tagMask = value;
			}
		}

		// Token: 0x1700019C RID: 412
		// (get) Token: 0x06000926 RID: 2342 RVA: 0x0002F248 File Offset: 0x0002D448
		// (set) Token: 0x06000927 RID: 2343 RVA: 0x0002F255 File Offset: 0x0002D455
		[Obsolete("Use collectionSettings.terrainHeightmapDownsamplingFactor instead")]
		public int terrainSampleSize
		{
			get
			{
				return this.collectionSettings.terrainHeightmapDownsamplingFactor;
			}
			set
			{
				this.collectionSettings.terrainHeightmapDownsamplingFactor = value;
			}
		}

		// Token: 0x1700019D RID: 413
		// (get) Token: 0x06000928 RID: 2344 RVA: 0x0002F263 File Offset: 0x0002D463
		public override float NavmeshCuttingCharacterRadius
		{
			get
			{
				return this.characterRadius;
			}
		}

		// Token: 0x1700019E RID: 414
		// (get) Token: 0x06000929 RID: 2345 RVA: 0x00019DC2 File Offset: 0x00017FC2
		public override bool RecalculateNormals
		{
			get
			{
				return true;
			}
		}

		// Token: 0x1700019F RID: 415
		// (get) Token: 0x0600092A RID: 2346 RVA: 0x0002F26B File Offset: 0x0002D46B
		public override float TileWorldSizeX
		{
			get
			{
				return (float)this.tileSizeX * this.cellSize;
			}
		}

		// Token: 0x170001A0 RID: 416
		// (get) Token: 0x0600092B RID: 2347 RVA: 0x0002F27B File Offset: 0x0002D47B
		public override float TileWorldSizeZ
		{
			get
			{
				return (float)this.tileSizeZ * this.cellSize;
			}
		}

		// Token: 0x170001A1 RID: 417
		// (get) Token: 0x0600092C RID: 2348 RVA: 0x0002F28B File Offset: 0x0002D48B
		public override float MaxTileConnectionEdgeDistance
		{
			get
			{
				return this.walkableClimb;
			}
		}

		// Token: 0x170001A2 RID: 418
		// (get) Token: 0x0600092D RID: 2349 RVA: 0x0002F294 File Offset: 0x0002D494
		public override Bounds bounds
		{
			get
			{
				float4x4 float4x = this.CalculateTransform().matrix;
				Bounds bounds = new ToWorldMatrix(new float3x3(float4x.c0.xyz, float4x.c1.xyz, float4x.c2.xyz)).ToWorld(new Bounds(Vector3.zero, this.forcedBoundsSize));
				bounds.center += this.forcedBoundsCenter;
				return bounds;
			}
		}

		// Token: 0x0600092E RID: 2350 RVA: 0x0002F314 File Offset: 0x0002D514
		public override bool IsInsideBounds(Vector3 point)
		{
			if (this.tiles == null || this.tiles.Length == 0)
			{
				return false;
			}
			float3 @float = this.transform.InverseTransform(point);
			if (this.dimensionMode == RecastGraph.DimensionMode.Dimension2D)
			{
				return @float.x >= 0f && @float.z >= 0f && @float.x <= this.forcedBoundsSize.x && @float.z <= this.forcedBoundsSize.z;
			}
			return math.all(@float >= 0f) && math.all(@float <= this.forcedBoundsSize);
		}

		// Token: 0x0600092F RID: 2351 RVA: 0x0002F3BE File Offset: 0x0002D5BE
		[Obsolete("Use SnapBoundsToScene instead")]
		public void SnapForceBoundsToScene()
		{
			this.SnapBoundsToScene();
		}

		// Token: 0x06000930 RID: 2352 RVA: 0x0002F3C8 File Offset: 0x0002D5C8
		public void SnapBoundsToScene()
		{
			DisposeArena disposeArena = new DisposeArena();
			RecastMeshGatherer.MeshCollection meshCollection = new TileBuilder(this, TileLayout.FromGraph(this), default(IntRect)).CollectMeshes(new Bounds(Vector3.zero, new Vector3(float.PositiveInfinity, float.PositiveInfinity, float.PositiveInfinity)));
			if (meshCollection.meshes.Length > 0)
			{
				ToWorldMatrix toWorldMatrix = new ToWorldMatrix(new float3x3(Quaternion.Inverse(Quaternion.Euler(this.rotation))));
				Bounds bounds = toWorldMatrix.ToWorld(meshCollection.meshes[0].bounds);
				for (int i = 1; i < meshCollection.meshes.Length; i++)
				{
					bounds.Encapsulate(toWorldMatrix.ToWorld(meshCollection.meshes[i].bounds));
				}
				bounds.max += Vector3.up * (this.walkableHeight * 1.1f);
				bounds.min -= Vector3.up * 0.01f;
				this.forcedBoundsCenter = Quaternion.Euler(this.rotation) * bounds.center;
				this.forcedBoundsSize = Vector3.Max(bounds.size, Vector3.one * 0.01f);
			}
			disposeArena.Add<RecastMeshGatherer.MeshCollection>(meshCollection);
			disposeArena.DisposeAll();
		}

		// Token: 0x06000931 RID: 2353 RVA: 0x0002F535 File Offset: 0x0002D735
		IGraphUpdatePromise IUpdatableGraph.ScheduleGraphUpdates(List<GraphUpdateObject> graphUpdates)
		{
			return new RecastGraph.RecastGraphUpdatePromise(this, graphUpdates);
		}

		// Token: 0x06000932 RID: 2354 RVA: 0x0002F53E File Offset: 0x0002D73E
		public IGraphUpdatePromise TranslateInDirection(int dx, int dz)
		{
			return new RecastGraph.RecastMovePromise(this, new Vector2Int(dx, dz));
		}

		// Token: 0x06000933 RID: 2355 RVA: 0x0002F54D File Offset: 0x0002D74D
		protected override IGraphUpdatePromise ScanInternal(bool async)
		{
			return new RecastGraph.RecastGraphScanPromise
			{
				graph = this
			};
		}

		// Token: 0x06000934 RID: 2356 RVA: 0x0002F55B File Offset: 0x0002D75B
		public override GraphTransform CalculateTransform()
		{
			return RecastGraph.CalculateTransform(new Bounds(this.forcedBoundsCenter, this.forcedBoundsSize), Quaternion.Euler(this.rotation));
		}

		// Token: 0x06000935 RID: 2357 RVA: 0x0002F57E File Offset: 0x0002D77E
		public static GraphTransform CalculateTransform(Bounds bounds, Quaternion rotation)
		{
			return new GraphTransform(Matrix4x4.TRS(bounds.center, rotation, Vector3.one) * Matrix4x4.TRS(-bounds.extents, Quaternion.identity, Vector3.one));
		}

		// Token: 0x06000936 RID: 2358 RVA: 0x0002F5B7 File Offset: 0x0002D7B7
		protected override void SetLayout(TileLayout info)
		{
			base.SetLayout(info);
			this.tileSizeX = info.tileSizeInVoxels.x;
			this.tileSizeZ = info.tileSizeInVoxels.y;
		}

		// Token: 0x170001A3 RID: 419
		// (get) Token: 0x06000937 RID: 2359 RVA: 0x0002F5E4 File Offset: 0x0002D7E4
		internal int CharacterRadiusInVoxels
		{
			get
			{
				return Mathf.CeilToInt(this.characterRadius / this.cellSize - 0.1f);
			}
		}

		// Token: 0x170001A4 RID: 420
		// (get) Token: 0x06000938 RID: 2360 RVA: 0x0002F5FE File Offset: 0x0002D7FE
		internal int TileBorderSizeInVoxels
		{
			get
			{
				return this.CharacterRadiusInVoxels + 3;
			}
		}

		// Token: 0x170001A5 RID: 421
		// (get) Token: 0x06000939 RID: 2361 RVA: 0x0002F608 File Offset: 0x0002D808
		internal float TileBorderSizeInWorldUnits
		{
			get
			{
				return (float)this.TileBorderSizeInVoxels * this.cellSize;
			}
		}

		// Token: 0x0600093A RID: 2362 RVA: 0x0002F618 File Offset: 0x0002D818
		public unsafe override void Resize(IntRect newTileBounds)
		{
			base.AssertSafeToUpdateGraph();
			if (!newTileBounds.IsValid())
			{
				throw new ArgumentException("Invalid tile bounds");
			}
			if (newTileBounds == new IntRect(0, 0, this.tileXCount - 1, this.tileZCount - 1))
			{
				return;
			}
			if (newTileBounds.Area == 0)
			{
				throw new ArgumentException("Tile count must at least 1x1");
			}
			if (!this.useTiles)
			{
				throw new InvalidOperationException("Cannot resize graph when tiles are not enabled");
			}
			this.navmeshUpdateData.MapBoostPrepareForGraphMutation(true);
			base.StartBatchTileUpdate(true);
			NavmeshTile[] array = new NavmeshTile[newTileBounds.Area];
			for (int i = 0; i < this.tileZCount; i++)
			{
				for (int j = 0; j < this.tileXCount; j++)
				{
					if (newTileBounds.Contains(j, i))
					{
						NavmeshTile navmeshTile = this.tiles[j + i * this.tileXCount];
						array[j - newTileBounds.xmin + (i - newTileBounds.ymin) * newTileBounds.Width] = navmeshTile;
					}
					else
					{
						base.ClearTile(j, i, null);
						base.DirtyBounds(base.GetTileBounds(j, i, 1, 1));
					}
				}
			}
			this.forcedBoundsSize = new Vector3((float)newTileBounds.Width * this.TileWorldSizeX, this.forcedBoundsSize.y, (float)newTileBounds.Height * this.TileWorldSizeZ);
			this.forcedBoundsCenter = this.transform.Transform(new Vector3((float)(newTileBounds.xmin + newTileBounds.xmax + 1) * 0.5f * this.TileWorldSizeX, this.forcedBoundsSize.y * 0.5f, (float)(newTileBounds.ymin + newTileBounds.ymax + 1) * 0.5f * this.TileWorldSizeZ));
			this.transform = this.CalculateTransform();
			Int3 @int = -(Int3)new Vector3(this.TileWorldSizeX * (float)newTileBounds.xmin, 0f, this.TileWorldSizeZ * (float)newTileBounds.ymin);
			for (int k = 0; k < newTileBounds.Height; k++)
			{
				for (int l = 0; l < newTileBounds.Width; l++)
				{
					int num = l + k * newTileBounds.Width;
					NavmeshTile navmeshTile2 = array[num];
					if (navmeshTile2 == null)
					{
						array[num] = base.NewEmptyTile(l, k);
					}
					else
					{
						navmeshTile2.x = l;
						navmeshTile2.z = k;
						for (int m = 0; m < navmeshTile2.nodes.Length; m++)
						{
							TriangleMeshNode triangleMeshNode = navmeshTile2.nodes[m];
							triangleMeshNode.v0 = (triangleMeshNode.v0 & 4095) | (num << 12);
							triangleMeshNode.v1 = (triangleMeshNode.v1 & 4095) | (num << 12);
							triangleMeshNode.v2 = (triangleMeshNode.v2 & 4095) | (num << 12);
						}
						for (int n = 0; n < navmeshTile2.vertsInGraphSpace.Length; n++)
						{
							*navmeshTile2.vertsInGraphSpace[n] += @int;
						}
						navmeshTile2.vertsInGraphSpace.CopyTo(navmeshTile2.verts);
						this.transform.Transform(navmeshTile2.verts);
						navmeshTile2.bbTree.Dispose();
						navmeshTile2.bbTree = new BBTree(navmeshTile2.tris, navmeshTile2.vertsInGraphSpace);
					}
				}
			}
			this.tiles = array;
			this.tileXCount = newTileBounds.Width;
			this.tileZCount = newTileBounds.Height;
			base.EndBatchTileUpdate();
			this.navmeshUpdateData.OnResized(newTileBounds, TileLayout.FromGraph(this));
		}

		// Token: 0x0600093B RID: 2363 RVA: 0x0002F990 File Offset: 0x0002DB90
		protected override void PostDeserialization(GraphSerializationContext ctx)
		{
			base.PostDeserialization(ctx);
			if (ctx.meta.version < AstarSerializer.V4_3_80)
			{
				this.collectionSettings.colliderRasterizeDetail = 2f * this.cellSize * this.collectionSettings.colliderRasterizeDetail * this.collectionSettings.colliderRasterizeDetail / 9.869605f;
			}
			if (ctx.meta.version < AstarSerializer.V5_1_0)
			{
				if (this.collectionSettings.tagMask.Count > 0 && this.collectionSettings.layerMask != -1)
				{
					Debug.LogError("In version 5.1.0 or higher of the A* Pathfinding Project you can no longer include objects both using a tag mask and a layer mask. Please choose in the recast graph inspector which one you want to use.");
					return;
				}
				if (this.collectionSettings.tagMask.Count > 0)
				{
					this.collectionSettings.collectionMode = RecastGraph.CollectionSettings.FilterMode.Tags;
				}
			}
		}

		// Token: 0x0400058B RID: 1419
		private static readonly ProfilerMarker MapBoostNodeFenceMarker = new ProfilerMarker("MapBoost.Nav.RecastNodeFence");

		// Token: 0x0400058C RID: 1420
		private static readonly ProfilerMarker MapBoostBorderFenceMarker = new ProfilerMarker("MapBoost.Nav.RecastBorderFence");

		// Token: 0x0400058D RID: 1421
		private static readonly ProfilerMarker MapBoostMoveApplyMarker = new ProfilerMarker("MapBoost.Nav.RecastMoveApply");

		// Token: 0x0400058E RID: 1422
		[JsonMember]
		public float characterRadius = 0.5f;

		// Token: 0x0400058F RID: 1423
		[JsonMember]
		public float contourMaxError = 2f;

		// Token: 0x04000590 RID: 1424
		[JsonMember]
		public float cellSize = 0.25f;

		// Token: 0x04000591 RID: 1425
		[JsonMember]
		public float walkableHeight = 2f;

		// Token: 0x04000592 RID: 1426
		[JsonMember]
		public float walkableClimb = 0.5f;

		// Token: 0x04000593 RID: 1427
		[JsonMember]
		public float maxSlope = 30f;

		// Token: 0x04000594 RID: 1428
		[JsonMember]
		public float maxEdgeLength = 20f;

		// Token: 0x04000595 RID: 1429
		[JsonMember]
		public float minRegionSize = 3f;

		// Token: 0x04000596 RID: 1430
		[JsonMember]
		public int editorTileSize = 128;

		// Token: 0x04000597 RID: 1431
		[JsonMember]
		public int tileSizeX = 128;

		// Token: 0x04000598 RID: 1432
		[JsonMember]
		public int tileSizeZ = 128;

		// Token: 0x04000599 RID: 1433
		[JsonMember]
		public bool useTiles = true;

		// Token: 0x0400059A RID: 1434
		public bool scanEmptyGraph;

		// Token: 0x0400059B RID: 1435
		[JsonMember]
		public List<RecastGraph.PerLayerModification> perLayerModifications = new List<RecastGraph.PerLayerModification>();

		// Token: 0x0400059C RID: 1436
		[JsonMember]
		public List<RecastGraph.PerTerrainLayerModification> perTerrainLayerModifications = new List<RecastGraph.PerTerrainLayerModification>();

		// Token: 0x0400059D RID: 1437
		[JsonMember]
		public RecastGraph.DimensionMode dimensionMode = RecastGraph.DimensionMode.Dimension3D;

		// Token: 0x0400059E RID: 1438
		[JsonMember]
		public RecastGraph.BackgroundTraversability backgroundTraversability;

		// Token: 0x0400059F RID: 1439
		[JsonMember]
		public RecastGraph.RelevantGraphSurfaceMode relevantGraphSurfaceMode;

		// Token: 0x040005A0 RID: 1440
		[JsonMember]
		public RecastGraph.CollectionSettings collectionSettings = new RecastGraph.CollectionSettings();

		// Token: 0x040005A1 RID: 1441
		[JsonMember]
		public Vector3 rotation;

		// Token: 0x040005A2 RID: 1442
		[JsonMember]
		public Vector3 forcedBoundsCenter;

		// Token: 0x040005A3 RID: 1443
		private DisposeArena pendingGraphUpdateArena = new DisposeArena();

		// Token: 0x040005A4 RID: 1444
		private bool hasExtendedInX;

		// Token: 0x040005A5 RID: 1445
		private bool hasExtendedInZ;

		// Token: 0x02000103 RID: 259
		public enum RelevantGraphSurfaceMode
		{
			// Token: 0x040005A7 RID: 1447
			DoNotRequire,
			// Token: 0x040005A8 RID: 1448
			OnlyForCompletelyInsideTile,
			// Token: 0x040005A9 RID: 1449
			RequireForAll
		}

		// Token: 0x02000104 RID: 260
		public enum DimensionMode
		{
			// Token: 0x040005AB RID: 1451
			Dimension2D,
			// Token: 0x040005AC RID: 1452
			Dimension3D
		}

		// Token: 0x02000105 RID: 261
		public enum BackgroundTraversability
		{
			// Token: 0x040005AE RID: 1454
			Walkable,
			// Token: 0x040005AF RID: 1455
			Unwalkable
		}

		// Token: 0x02000106 RID: 262
		[Serializable]
		public struct PerLayerModification
		{
			// Token: 0x170001A6 RID: 422
			// (get) Token: 0x0600093E RID: 2366 RVA: 0x0002FB50 File Offset: 0x0002DD50
			public static RecastGraph.PerLayerModification Default
			{
				get
				{
					return new RecastGraph.PerLayerModification
					{
						layer = 0,
						mode = RecastNavmeshModifier.Mode.WalkableSurface,
						surfaceID = 1
					};
				}
			}

			// Token: 0x0600093F RID: 2367 RVA: 0x0002FB80 File Offset: 0x0002DD80
			public static RecastGraph.PerLayerModification[] ToLayerLookup(List<RecastGraph.PerLayerModification> perLayerModifications, RecastGraph.PerLayerModification defaultValue)
			{
				RecastGraph.PerLayerModification[] array = new RecastGraph.PerLayerModification[32];
				int num = 0;
				for (int i = 0; i < array.Length; i++)
				{
					array[i] = defaultValue;
					array[i].layer = i;
				}
				for (int j = 0; j < perLayerModifications.Count; j++)
				{
					if (perLayerModifications[j].layer < 0 || perLayerModifications[j].layer >= 32)
					{
						Debug.LogError("Layer " + perLayerModifications[j].layer.ToString() + " is out of range. Layers must be in the range [0...31]");
					}
					else if ((num & (1 << perLayerModifications[j].layer)) != 0)
					{
						Debug.LogError("Several per layer modifications refer to the same layer '" + LayerMask.LayerToName(perLayerModifications[j].layer) + "'");
					}
					else
					{
						num |= 1 << perLayerModifications[j].layer;
						array[perLayerModifications[j].layer] = perLayerModifications[j];
					}
				}
				return array;
			}

			// Token: 0x040005B0 RID: 1456
			public int layer;

			// Token: 0x040005B1 RID: 1457
			public RecastNavmeshModifier.Mode mode;

			// Token: 0x040005B2 RID: 1458
			public int surfaceID;
		}

		// Token: 0x02000107 RID: 263
		[Serializable]
		public struct PerTerrainLayerModification
		{
			// Token: 0x170001A7 RID: 423
			// (get) Token: 0x06000940 RID: 2368 RVA: 0x0002FC88 File Offset: 0x0002DE88
			public static RecastGraph.PerTerrainLayerModification Default
			{
				get
				{
					return new RecastGraph.PerTerrainLayerModification
					{
						layer = 0,
						mode = RecastNavmeshModifier.Mode.WalkableSurface,
						surfaceID = 1,
						threshold = 0.5f
					};
				}
			}

			// Token: 0x040005B3 RID: 1459
			public int layer;

			// Token: 0x040005B4 RID: 1460
			public RecastNavmeshModifier.Mode mode;

			// Token: 0x040005B5 RID: 1461
			public int surfaceID;

			// Token: 0x040005B6 RID: 1462
			public float threshold;
		}

		// Token: 0x02000108 RID: 264
		[Serializable]
		public class CollectionSettings
		{
			// Token: 0x040005B7 RID: 1463
			public RecastGraph.CollectionSettings.FilterMode collectionMode;

			// Token: 0x040005B8 RID: 1464
			[NonSerialized]
			public PhysicsScene? physicsScene;

			// Token: 0x040005B9 RID: 1465
			[NonSerialized]
			public PhysicsScene2D? physicsScene2D;

			// Token: 0x040005BA RID: 1466
			public LayerMask layerMask = -1;

			// Token: 0x040005BB RID: 1467
			public List<string> tagMask = new List<string>();

			// Token: 0x040005BC RID: 1468
			public bool rasterizeColliders = true;

			// Token: 0x040005BD RID: 1469
			public bool rasterizeMeshes;

			// Token: 0x040005BE RID: 1470
			public bool rasterizeTerrain = true;

			// Token: 0x040005BF RID: 1471
			public bool rasterizeTrees = true;

			// Token: 0x040005C0 RID: 1472
			public int terrainHeightmapDownsamplingFactor = 3;

			// Token: 0x040005C1 RID: 1473
			public float colliderRasterizeDetail = 1f;

			// Token: 0x040005C2 RID: 1474
			public Action<RecastMeshGatherer> onCollectMeshes;

			// Token: 0x02000109 RID: 265
			public enum FilterMode
			{
				// Token: 0x040005C4 RID: 1476
				Layers,
				// Token: 0x040005C5 RID: 1477
				Tags
			}
		}

		// Token: 0x0200010A RID: 266
		private class RecastGraphUpdatePromise : IGraphUpdatePromise
		{
			// Token: 0x06000942 RID: 2370 RVA: 0x0002FD18 File Offset: 0x0002DF18
			public RecastGraphUpdatePromise(RecastGraph graph, List<GraphUpdateObject> graphUpdates)
			{
				this.promises = ListPool<ValueTuple<Promise<TileBuilder.TileBuilderOutput>, Promise<TileCutter.TileCutterOutput>, Promise<JobBuildNodes.BuildNodeTilesOutput>>>.Claim();
				this.graph = graph;
				this.graphHash = RecastGraph.RecastGraphUpdatePromise.HashSettings(graph);
				List<ValueTuple<IntRect, GraphUpdateObject>> list = ListPool<ValueTuple<IntRect, GraphUpdateObject>>.Claim();
				for (int i = graphUpdates.Count - 1; i >= 0; i--)
				{
					GraphUpdateObject graphUpdateObject = graphUpdates[i];
					if (graphUpdateObject.updatePhysics)
					{
						graphUpdates.RemoveAt(i);
						IntRect touchingTiles = graph.GetTouchingTiles(graphUpdateObject.bounds, graph.TileBorderSizeInWorldUnits);
						if (touchingTiles.IsValid())
						{
							list.Add(new ValueTuple<IntRect, GraphUpdateObject>(touchingTiles, graphUpdateObject));
						}
					}
				}
				this.graphUpdates = graphUpdates;
				if (list.Count > 1)
				{
					list.Sort((ValueTuple<IntRect, GraphUpdateObject> a, ValueTuple<IntRect, GraphUpdateObject> b) => b.Item1.Area.CompareTo(a.Item1.Area));
				}
				int j = 0;
				while (j < list.Count)
				{
					IntRect item = list[j].Item1;
					if (list.Count <= 1)
					{
						goto IL_0136;
					}
					bool flag = false;
					for (int k = item.ymin; k <= item.ymax; k++)
					{
						for (int l = item.xmin; l <= item.xmax; l++)
						{
							NavmeshTile tile = graph.GetTile(l, k);
							flag |= !tile.flag;
							tile.flag = true;
						}
					}
					if (flag)
					{
						goto IL_0136;
					}
					IL_01AC:
					j++;
					continue;
					IL_0136:
					TileLayout tileLayout = TileLayout.FromGraph(graph);
					Promise<TileBuilder.TileBuilderOutput> promise = RecastBuilder.BuildTileMeshes(graph, tileLayout, item).Schedule(graph.pendingGraphUpdateArena);
					Promise<TileCutter.TileCutterOutput> promise2 = RecastBuilder.CutTiles(graph, graph.navmeshUpdateData.clipperLookup, tileLayout).Schedule(promise);
					Promise<JobBuildNodes.BuildNodeTilesOutput> promise3 = RecastBuilder.BuildNodeTiles(graph, tileLayout).Schedule(graph.pendingGraphUpdateArena, promise, promise2);
					this.promises.Add(new ValueTuple<Promise<TileBuilder.TileBuilderOutput>, Promise<TileCutter.TileCutterOutput>, Promise<JobBuildNodes.BuildNodeTilesOutput>>(promise, promise2, promise3));
					goto IL_01AC;
				}
				if (list.Count > 1)
				{
					for (int m = 0; m < list.Count; m++)
					{
						IntRect item2 = list[m].Item1;
						for (int n = item2.ymin; n <= item2.ymax; n++)
						{
							for (int num = item2.xmin; num <= item2.xmax; num++)
							{
								graph.GetTile(num, n).flag = false;
							}
						}
					}
				}
				ListPool<ValueTuple<IntRect, GraphUpdateObject>>.Release(ref list);
			}

			// Token: 0x06000943 RID: 2371 RVA: 0x0002FF60 File Offset: 0x0002E160
			public IEnumerator<JobHandle> Prepare()
			{
				int num;
				for (int i = 0; i < this.promises.Count; i = num + 1)
				{
					yield return this.promises[i].Item2.handle;
					yield return this.promises[i].Item1.handle;
					num = i;
				}
				yield break;
			}

			// Token: 0x06000944 RID: 2372 RVA: 0x0002FF70 File Offset: 0x0002E170
			private static int HashSettings(RecastGraph graph)
			{
				return (((graph.tileXCount * 31) ^ graph.tileZCount) * 31) ^ (graph.TileWorldSizeX.GetHashCode() * 31) ^ graph.TileWorldSizeZ.GetHashCode();
			}

			// Token: 0x06000945 RID: 2373 RVA: 0x0002FFB4 File Offset: 0x0002E1B4
			public void Apply(IGraphUpdateContext ctx)
			{
				if (RecastGraph.RecastGraphUpdatePromise.HashSettings(this.graph) != this.graphHash)
				{
					throw new InvalidOperationException("Recast graph changed while a graph update was in progress. This is not allowed. Use AstarPath.active.AddWorkItem if you need to update graphs.");
				}
				for (int i = 0; i < this.promises.Count; i++)
				{
					Promise<TileBuilder.TileBuilderOutput> item = this.promises[i].Item1;
					Promise<TileCutter.TileCutterOutput> item2 = this.promises[i].Item2;
					Promise<JobBuildNodes.BuildNodeTilesOutput> item3 = this.promises[i].Item3;
					JobBuildNodes.BuildNodeTilesOutput buildNodeTilesOutput;
					using (RecastGraph.MapBoostNodeFenceMarker.Auto())
					{
						buildNodeTilesOutput = item3.Complete();
					}
					IntRect tileRect = buildNodeTilesOutput.progressSource.tileMeshes.tileRect;
					NavmeshTile[] tiles = buildNodeTilesOutput.tiles;
					item.Dispose();
					item2.Dispose();
					item3.Dispose();
					for (int j = 0; j < tiles.Length; j++)
					{
						AstarPath active = AstarPath.active;
						GraphNode[] nodes = tiles[j].nodes;
						active.InitializeNodes(nodes);
					}
					this.graph.StartBatchTileUpdate(true);
					for (int k = 0; k < tileRect.Height; k++)
					{
						for (int l = 0; l < tileRect.Width; l++)
						{
							NavmeshTile navmeshTile = tiles[k * tileRect.Width + l];
							navmeshTile.graph = this.graph;
							this.graph.ClearTile(l + tileRect.xmin, k + tileRect.ymin, navmeshTile);
						}
					}
					this.graph.EndBatchTileUpdate();
					GCHandle gchandle = GCHandle.Alloc(this.graph.tiles);
					IntRect intRect = new IntRect(0, 0, this.graph.tileXCount - 1, this.graph.tileZCount - 1);
					using (RecastGraph.MapBoostBorderFenceMarker.Auto())
					{
						JobConnectTiles.ScheduleRecalculateBorders(gchandle, default(JobHandle), intRect, tileRect, new Vector2(this.graph.TileWorldSizeX, this.graph.TileWorldSizeZ), this.graph.MaxTileConnectionEdgeDistance).Complete();
					}
					gchandle.Free();
					if (this.graph.OnRecalculatedTiles != null)
					{
						this.graph.OnRecalculatedTiles(tiles);
					}
					ctx.DirtyBounds(this.graph.GetTileBounds(tileRect));
				}
				this.graph.pendingGraphUpdateArena.DisposeAll();
				if (this.graphUpdates != null)
				{
					for (int m = 0; m < this.graphUpdates.Count; m++)
					{
						GraphUpdateObject graphUpdateObject = this.graphUpdates[m];
						IntRect touchingTiles = this.graph.GetTouchingTiles(graphUpdateObject.bounds, this.graph.TileBorderSizeInWorldUnits);
						if (touchingTiles.IsValid())
						{
							for (int n = touchingTiles.ymin; n <= touchingTiles.ymax; n++)
							{
								for (int num = touchingTiles.xmin; num <= touchingTiles.xmax; num++)
								{
									NavmeshTile navmeshTile2 = this.graph.tiles[n * this.graph.tileXCount + num];
									NavMeshGraph.UpdateArea(graphUpdateObject, navmeshTile2);
								}
							}
							ctx.DirtyBounds(this.graph.GetTileBounds(touchingTiles));
						}
					}
				}
			}

			// Token: 0x040005C6 RID: 1478
			public List<ValueTuple<Promise<TileBuilder.TileBuilderOutput>, Promise<TileCutter.TileCutterOutput>, Promise<JobBuildNodes.BuildNodeTilesOutput>>> promises;

			// Token: 0x040005C7 RID: 1479
			public List<GraphUpdateObject> graphUpdates;

			// Token: 0x040005C8 RID: 1480
			public RecastGraph graph;

			// Token: 0x040005C9 RID: 1481
			private int graphHash;
		}

		// Token: 0x0200010D RID: 269
		private class RecastGraphScanPromise : IGraphUpdatePromise
		{
			// Token: 0x170001AA RID: 426
			// (get) Token: 0x0600094F RID: 2383 RVA: 0x00030431 File Offset: 0x0002E631
			public float Progress
			{
				get
				{
					if (this.progressSource == null)
					{
						return 1f;
					}
					return this.progressSource.Progress;
				}
			}

			// Token: 0x06000950 RID: 2384 RVA: 0x0003044C File Offset: 0x0002E64C
			public IEnumerator<JobHandle> Prepare()
			{
				TriangleMeshNode.SetNavmeshHolder(AstarPath.active.data.GetGraphIndex(this.graph), this.graph);
				if (!Application.isPlaying)
				{
					RelevantGraphSurface.FindAllGraphSurfaces();
				}
				RelevantGraphSurface.UpdateAllPositions();
				this.tileLayout = TileLayout.FromGraph(this.graph);
				if (this.graph.scanEmptyGraph || this.tileLayout.tileCount.x * this.tileLayout.tileCount.y <= 0)
				{
					this.emptyGraph = true;
					yield break;
				}
				DisposeArena arena = new DisposeArena();
				IntRect intRect = new IntRect(0, 0, this.tileLayout.tileCount.x - 1, this.tileLayout.tileCount.y - 1);
				Promise<TileBuilder.TileBuilderOutput> tileMeshesPromise = RecastBuilder.BuildTileMeshes(this.graph, this.tileLayout, intRect).Schedule(arena);
				this.cutSettings = new NavmeshUpdates.NavmeshUpdateSettings(this.graph, this.tileLayout);
				Promise<TileCutter.TileCutterOutput> cutPromise = RecastBuilder.CutTiles(this.graph, this.cutSettings.clipperLookup, this.tileLayout).Schedule(tileMeshesPromise);
				this.cutSettings.DiscardPending();
				Promise<JobBuildNodes.BuildNodeTilesOutput> tilesPromise = RecastBuilder.BuildNodeTiles(this.graph, this.tileLayout).Schedule(arena, tileMeshesPromise, cutPromise);
				this.progressSource = tilesPromise;
				yield return tilesPromise.handle;
				this.progressSource = null;
				JobBuildNodes.BuildNodeTilesOutput buildNodeTilesOutput = tilesPromise.Complete();
				TileBuilder.TileBuilderOutput tileBuilderOutput = tileMeshesPromise.Complete();
				TileCutter.TileCutterOutput tileCutterOutput = cutPromise.Complete();
				this.tiles = buildNodeTilesOutput.tiles;
				tileBuilderOutput.Dispose();
				tileCutterOutput.Dispose();
				buildNodeTilesOutput.Dispose();
				arena.DisposeAll();
				yield break;
			}

			// Token: 0x06000951 RID: 2385 RVA: 0x0003045C File Offset: 0x0002E65C
			public void Apply(IGraphUpdateContext ctx)
			{
				this.graph.DestroyAllNodes();
				this.graph.hasExtendedInZ = false;
				this.graph.hasExtendedInX = false;
				this.cutSettings.AttachToGraph();
				if (this.emptyGraph)
				{
					this.graph.SetLayout(this.tileLayout);
					this.graph.FillWithEmptyTiles();
				}
				else
				{
					for (int i = 0; i < this.tiles.Length; i++)
					{
						AstarPath active = AstarPath.active;
						GraphNode[] nodes = this.tiles[i].nodes;
						active.InitializeNodes(nodes);
					}
					this.graph.SetLayout(this.tileLayout);
					this.graph.tiles = this.tiles;
					for (int j = 0; j < this.tiles.Length; j++)
					{
						this.tiles[j].graph = this.graph;
					}
				}
				if (this.graph.OnRecalculatedTiles != null)
				{
					this.graph.OnRecalculatedTiles(this.graph.tiles.Clone() as NavmeshTile[]);
				}
			}

			// Token: 0x040005D0 RID: 1488
			public RecastGraph graph;

			// Token: 0x040005D1 RID: 1489
			private TileLayout tileLayout;

			// Token: 0x040005D2 RID: 1490
			private bool emptyGraph;

			// Token: 0x040005D3 RID: 1491
			private NavmeshTile[] tiles;

			// Token: 0x040005D4 RID: 1492
			private IProgress progressSource;

			// Token: 0x040005D5 RID: 1493
			private NavmeshUpdates.NavmeshUpdateSettings cutSettings;
		}

		// Token: 0x0200010F RID: 271
		private class RecastMovePromise : IGraphUpdatePromise
		{
			// Token: 0x06000959 RID: 2393 RVA: 0x000307A0 File Offset: 0x0002E9A0
			public RecastMovePromise(RecastGraph graph, Vector2Int delta)
			{
				this.graph = graph;
				this.delta = delta;
				if (delta.x != 0 && delta.y != 0)
				{
					throw new ArgumentException("Only translation in a single direction is supported. delta.x == 0 || delta.y == 0 must hold.");
				}
			}

			// Token: 0x0600095A RID: 2394 RVA: 0x000307D3 File Offset: 0x0002E9D3
			public IEnumerator<JobHandle> Prepare()
			{
				if (this.delta.x == 0 && this.delta.y == 0)
				{
					yield break;
				}
				IntRect intRect = new IntRect(0, 0, this.graph.tileXCount - 1, this.graph.tileZCount - 1);
				this.newTileRect = intRect.Offset(this.delta);
				IntRect createdTiles = IntRect.Exclude(this.newTileRect, intRect);
				if (!this.graph.hasExtendedInX && this.delta.x != 0)
				{
					if (this.delta.x > 0)
					{
						createdTiles.xmin--;
					}
					this.graph.hasExtendedInX = true;
				}
				if (!this.graph.hasExtendedInZ && this.delta.y != 0)
				{
					if (this.delta.y > 0)
					{
						createdTiles.ymin--;
					}
					this.graph.hasExtendedInZ = true;
				}
				DisposeArena disposeArena = new DisposeArena();
				TileLayout tileLayout = TileLayout.FromGraph(this.graph);
				tileLayout.graphSpaceSize.x = float.PositiveInfinity;
				tileLayout.graphSpaceSize.z = float.PositiveInfinity;
				Promise<TileBuilder.TileBuilderOutput> pendingPromise = RecastBuilder.BuildTileMeshes(this.graph, tileLayout, createdTiles).Schedule(disposeArena);
				yield return pendingPromise.handle;
				TileBuilder.TileBuilderOutput value = pendingPromise.GetValue();
				this.tileMeshes = value.tileMeshes.ToManaged();
				pendingPromise.Dispose();
				disposeArena.DisposeAll();
				this.tileMeshes.tileRect = createdTiles.Offset(-this.delta);
				yield break;
			}

			// Token: 0x0600095B RID: 2395 RVA: 0x000307E4 File Offset: 0x0002E9E4
			public void Apply(IGraphUpdateContext ctx)
			{
				if (this.delta.x == 0 && this.delta.y == 0)
				{
					return;
				}
				using (RecastGraph.MapBoostMoveApplyMarker.Auto())
				{
					this.graph.Resize(this.newTileRect);
					this.graph.ReplaceTiles(this.tileMeshes, 0f);
				}
			}

			// Token: 0x040005DD RID: 1501
			private RecastGraph graph;

			// Token: 0x040005DE RID: 1502
			private TileMeshes tileMeshes;

			// Token: 0x040005DF RID: 1503
			private Vector2Int delta;

			// Token: 0x040005E0 RID: 1504
			private IntRect newTileRect;
		}
	}
}
