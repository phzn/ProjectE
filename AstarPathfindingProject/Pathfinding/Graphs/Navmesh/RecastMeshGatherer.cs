using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Pathfinding.Collections;
using Pathfinding.Graphs.Navmesh.Voxelization.Burst;
using Pathfinding.Jobs;
using Pathfinding.Pooling;
using Pathfinding.Util;
using Unity.Burst;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Profiling;
using UnityEngine;
using UnityEngine.Rendering;

namespace Pathfinding.Graphs.Navmesh
{
	// Token: 0x020001AA RID: 426
	[BurstCompile]
	public class RecastMeshGatherer
	{
		// Token: 0x06000C57 RID: 3159 RVA: 0x00044308 File Offset: 0x00042508
		public RecastMeshGatherer(PhysicsScene physicsScene, PhysicsScene2D physicsScene2D, Bounds bounds, int terrainDownsamplingFactor, LayerMask mask, List<string> tagMask, List<RecastGraph.PerLayerModification> perLayerModifications, List<RecastGraph.PerTerrainLayerModification> perTerrainLayerModifications, float maxColliderApproximationError)
		{
			terrainDownsamplingFactor = Math.Max(terrainDownsamplingFactor, 1);
			this.bounds = bounds;
			this.terrainDownsamplingFactor = terrainDownsamplingFactor;
			this.mask = mask;
			this.tagMask = tagMask ?? new List<string>();
			this.maxColliderApproximationError = maxColliderApproximationError;
			this.physicsScene = physicsScene;
			this.physicsScene2D = physicsScene2D;
			this.perTerrainLayerModifications = perTerrainLayerModifications;
			this.meshes = new UnsafeList<RecastMeshGatherer.GatheredMesh>(16, Allocator.Persistent, NativeArrayOptions.UninitializedMemory);
			this.vertexBuffers = new UnsafeList<UnsafeSpan<Vector3>>(16, Allocator.Persistent, NativeArrayOptions.UninitializedMemory);
			this.triangleBuffers = new UnsafeList<UnsafeSpan<int>>(16, Allocator.Persistent, NativeArrayOptions.UninitializedMemory);
			this.tagsBuffers = new UnsafeList<UnsafeSpan<int>>(16, Allocator.Persistent, NativeArrayOptions.UninitializedMemory);
			this.cachedMeshes = ObjectPoolSimple<Dictionary<RecastMeshGatherer.MeshCacheItem, int>>.Claim();
			this.meshData = ListPool<Mesh>.Claim();
			this.modificationsByLayer = RecastGraph.PerLayerModification.ToLayerLookup(perLayerModifications, RecastGraph.PerLayerModification.Default);
			RecastGraph.PerLayerModification @default = RecastGraph.PerLayerModification.Default;
			@default.mode = RecastNavmeshModifier.Mode.UnwalkableSurface;
			this.modificationsByLayer2D = RecastGraph.PerLayerModification.ToLayerLookup(perLayerModifications, @default);
		}

		// Token: 0x06000C58 RID: 3160 RVA: 0x00044415 File Offset: 0x00042615
		[BurstCompile]
		private static void CalculateBounds(ref UnsafeSpan<float3> vertices, ref float4x4 localToWorldMatrix, out Bounds bounds)
		{
			RecastMeshGatherer.CalculateBounds_00000B92$BurstDirectCall.Invoke(ref vertices, ref localToWorldMatrix, out bounds);
		}

		// Token: 0x06000C59 RID: 3161 RVA: 0x00044420 File Offset: 0x00042620
		private static void GetMissingMeshDataAndBounds(List<Mesh> meshData, ref UnsafeList<RecastMeshGatherer.GatheredMesh> gatheredMeshes, ref UnsafeList<UnsafeSpan<Vector3>> vertexBuffers, ref UnsafeList<UnsafeSpan<int>> triangleBuffers)
		{
			Mesh.MeshDataArray meshDataArray = Mesh.AcquireReadOnlyMeshData(meshData);
			int length = vertexBuffers.Length;
			meshData.Clear();
			for (int i = 0; i < meshDataArray.Length; i++)
			{
				NativeArray<Vector3> nativeArray;
				NativeArray<int> nativeArray2;
				MeshUtility.GetMeshData(meshDataArray, i, out nativeArray, out nativeArray2);
				UnsafeSpan<Vector3> unsafeSpan = nativeArray.MoveToUnsafeSpan<Vector3>();
				vertexBuffers.Add(in unsafeSpan);
				UnsafeSpan<int> unsafeSpan2 = nativeArray2.MoveToUnsafeSpan<int>();
				triangleBuffers.Add(in unsafeSpan2);
			}
			meshDataArray.Dispose();
			RecastMeshGatherer.PatchMissingMeshDataAndBounds(ref gatheredMeshes, ref vertexBuffers, length);
		}

		// Token: 0x06000C5A RID: 3162 RVA: 0x0004448E File Offset: 0x0004268E
		[BurstCompile]
		private static void PatchMissingMeshDataAndBounds(ref UnsafeList<RecastMeshGatherer.GatheredMesh> gatheredMeshes, ref UnsafeList<UnsafeSpan<Vector3>> vertexBuffers, int meshBufferOffset)
		{
			RecastMeshGatherer.PatchMissingMeshDataAndBounds_00000B94$BurstDirectCall.Invoke(ref gatheredMeshes, ref vertexBuffers, meshBufferOffset);
		}

		// Token: 0x06000C5B RID: 3163 RVA: 0x00044498 File Offset: 0x00042698
		public new RecastMeshGatherer.MeshCollection Finalize()
		{
			RecastMeshGatherer.GetMissingMeshDataAndBounds(this.meshData, ref this.meshes, ref this.vertexBuffers, ref this.triangleBuffers);
			NativeArray<RasterizationMesh> nativeArray = new NativeArray<RasterizationMesh>(this.meshes.Length, Allocator.Persistent, NativeArrayOptions.ClearMemory);
			UnsafeSpan<RasterizationMesh> unsafeSpan = nativeArray.AsUnsafeSpan<RasterizationMesh>();
			RecastMeshGatherer.CreateRasterizationMeshes(ref this.meshes, ref this.vertexBuffers, ref this.triangleBuffers, ref this.tagsBuffers, ref unsafeSpan);
			this.cachedMeshes.Clear();
			ObjectPoolSimple<Dictionary<RecastMeshGatherer.MeshCacheItem, int>>.Release(ref this.cachedMeshes);
			this.meshes.Dispose();
			return new RecastMeshGatherer.MeshCollection(this.vertexBuffers, this.triangleBuffers, this.tagsBuffers, nativeArray);
		}

		// Token: 0x06000C5C RID: 3164 RVA: 0x00044535 File Offset: 0x00042735
		[BurstCompile]
		private static void CreateRasterizationMeshes(ref UnsafeList<RecastMeshGatherer.GatheredMesh> meshes, ref UnsafeList<UnsafeSpan<Vector3>> vertexBuffers, ref UnsafeList<UnsafeSpan<int>> triangleBuffers, ref UnsafeList<UnsafeSpan<int>> tagsBuffers, ref UnsafeSpan<RasterizationMesh> rasterizationMeshesOutput)
		{
			RecastMeshGatherer.CreateRasterizationMeshes_00000B96$BurstDirectCall.Invoke(ref meshes, ref vertexBuffers, ref triangleBuffers, ref tagsBuffers, ref rasterizationMeshesOutput);
		}

		// Token: 0x06000C5D RID: 3165 RVA: 0x00044542 File Offset: 0x00042742
		public int AddMeshBuffers(Vector3[] vertices, int[] triangles)
		{
			return this.AddMeshBuffers(new UnsafeSpan<Vector3>(vertices, Allocator.Persistent), new UnsafeSpan<int>(triangles, Allocator.Persistent));
		}

		// Token: 0x06000C5E RID: 3166 RVA: 0x00044558 File Offset: 0x00042758
		public int AddMeshBuffers(UnsafeSpan<Vector3> vertices, UnsafeSpan<int> triangles)
		{
			int num = -this.vertexBuffers.Length - 1;
			this.vertexBuffers.Add(in vertices);
			this.triangleBuffers.Add(in triangles);
			return num;
		}

		// Token: 0x06000C5F RID: 3167 RVA: 0x00044584 File Offset: 0x00042784
		public void AddMesh(Renderer renderer, Mesh gatheredMesh)
		{
			RecastMeshGatherer.GatheredMesh gatheredMesh2;
			if (this.ConvertMeshToGatheredMesh(renderer, gatheredMesh, out gatheredMesh2))
			{
				this.meshes.Add(in gatheredMesh2);
			}
		}

		// Token: 0x06000C60 RID: 3168 RVA: 0x000445AA File Offset: 0x000427AA
		public void AddMesh(RecastMeshGatherer.GatheredMesh gatheredMesh)
		{
			this.meshes.Add(in gatheredMesh);
		}

		// Token: 0x06000C61 RID: 3169 RVA: 0x000445BC File Offset: 0x000427BC
		private bool MeshFilterShouldBeIncluded(MeshFilter filter)
		{
			Renderer renderer;
			RecastNavmeshModifier recastNavmeshModifier;
			return filter.TryGetComponent<Renderer>(out renderer) && filter.sharedMesh != null && renderer.enabled && (((1 << filter.gameObject.layer) & this.mask) != 0 || (this.tagMask.Count > 0 && this.tagMask.Contains(filter.tag))) && (!filter.TryGetComponent<RecastNavmeshModifier>(out recastNavmeshModifier) || !recastNavmeshModifier.enabled);
		}

		// Token: 0x06000C62 RID: 3170 RVA: 0x00044640 File Offset: 0x00042840
		private bool ConvertMeshToGatheredMesh(Renderer renderer, Mesh mesh, out RecastMeshGatherer.GatheredMesh gatheredMesh)
		{
			if (!mesh.HasVertexAttribute(VertexAttribute.Position))
			{
				gatheredMesh = default(RecastMeshGatherer.GatheredMesh);
				return false;
			}
			if (!mesh.isReadable)
			{
				if (!this.anyNonReadableMesh)
				{
					Debug.LogError("Some meshes could not be included when scanning the graph because they are marked as not readable. This includes the mesh '" + mesh.name + "'. You need to mark the mesh with read/write enabled in the mesh importer. Alternatively you can only rasterize colliders and not meshes. Mesh Collider meshes still need to be readable.", mesh);
				}
				this.anyNonReadableMesh = true;
				gatheredMesh = default(RecastMeshGatherer.GatheredMesh);
				return false;
			}
			renderer.GetSharedMaterials(this.dummyMaterials);
			MeshRenderer meshRenderer = renderer as MeshRenderer;
			int num = ((meshRenderer != null) ? meshRenderer.subMeshStartIndex : 0);
			int count = this.dummyMaterials.Count;
			int num2 = 0;
			int num3 = -1;
			if (num > 0 || count < mesh.subMeshCount)
			{
				SubMeshDescriptor subMesh = mesh.GetSubMesh(num);
				SubMeshDescriptor subMesh2 = mesh.GetSubMesh(num + count - 1);
				num2 = subMesh.indexStart;
				num3 = subMesh2.indexStart + subMesh2.indexCount;
			}
			int count2;
			if (!this.cachedMeshes.TryGetValue(new RecastMeshGatherer.MeshCacheItem(mesh), out count2))
			{
				count2 = this.meshData.Count;
				this.meshData.Add(mesh);
				this.cachedMeshes[new RecastMeshGatherer.MeshCacheItem(mesh)] = count2;
			}
			gatheredMesh = new RecastMeshGatherer.GatheredMesh
			{
				meshDataIndex = count2,
				bounds = renderer.bounds,
				indexStart = num2,
				indexEnd = num3,
				matrix = renderer.localToWorldMatrix,
				doubleSided = false,
				flatten = false,
				tagDataIndex = -1
			};
			return true;
		}

		// Token: 0x06000C63 RID: 3171 RVA: 0x000447A8 File Offset: 0x000429A8
		private RecastMeshGatherer.GatheredMesh? GetColliderMesh(MeshCollider collider, Matrix4x4 localToWorldMatrix)
		{
			if (!(collider.sharedMesh != null))
			{
				return null;
			}
			Mesh sharedMesh = collider.sharedMesh;
			if (!sharedMesh.HasVertexAttribute(VertexAttribute.Position))
			{
				return null;
			}
			if (!sharedMesh.isReadable)
			{
				if (!this.anyNonReadableMesh)
				{
					Debug.LogError("Some mesh collider meshes could not be included when scanning the graph because they are marked as not readable. This includes the mesh '" + sharedMesh.name + "'. You need to mark the mesh with read/write enabled in the mesh importer.", sharedMesh);
				}
				this.anyNonReadableMesh = true;
				return null;
			}
			int count;
			if (!this.cachedMeshes.TryGetValue(new RecastMeshGatherer.MeshCacheItem(sharedMesh), out count))
			{
				count = this.meshData.Count;
				this.meshData.Add(sharedMesh);
				this.cachedMeshes[new RecastMeshGatherer.MeshCacheItem(sharedMesh)] = count;
			}
			return new RecastMeshGatherer.GatheredMesh?(new RecastMeshGatherer.GatheredMesh
			{
				meshDataIndex = count,
				bounds = collider.bounds,
				areaIsTag = false,
				area = 0,
				tagDataIndex = -1,
				indexStart = 0,
				indexEnd = -1,
				solid = collider.convex,
				matrix = localToWorldMatrix,
				doubleSided = false,
				flatten = false
			});
		}

		// Token: 0x06000C64 RID: 3172 RVA: 0x000448D8 File Offset: 0x00042AD8
		public void CollectSceneMeshes()
		{
			if (this.tagMask.Count > 0 || this.mask != 0)
			{
				MeshFilter[] array = UnityCompatibility.FindObjectsByTypeSorted<MeshFilter>();
				bool flag = false;
				foreach (MeshFilter meshFilter in array)
				{
					if (this.MeshFilterShouldBeIncluded(meshFilter))
					{
						Renderer renderer;
						meshFilter.TryGetComponent<Renderer>(out renderer);
						RecastMeshGatherer.GatheredMesh gatheredMesh;
						if (renderer.isPartOfStaticBatch)
						{
							flag = true;
						}
						else if (renderer.bounds.Intersects(this.bounds) && this.ConvertMeshToGatheredMesh(renderer, meshFilter.sharedMesh, out gatheredMesh))
						{
							gatheredMesh.ApplyLayerModification(this.modificationsByLayer[meshFilter.gameObject.layer]);
							this.meshes.Add(in gatheredMesh);
						}
					}
				}
				if (flag)
				{
					Debug.LogWarning("Some meshes were statically batched. These meshes can not be used for navmesh calculation due to technical constraints.\nDuring runtime scripts cannot access the data of meshes which have been statically batched.\nOne way to solve this problem is to use cached startup (Save & Load tab in the inspector) to only calculate the graph when the game is not playing.");
				}
			}
		}

		// Token: 0x06000C65 RID: 3173 RVA: 0x000449A0 File Offset: 0x00042BA0
		private static int AreaFromSurfaceMode(RecastNavmeshModifier.Mode mode, int surfaceID)
		{
			switch (mode)
			{
			default:
				return -1;
			case RecastNavmeshModifier.Mode.WalkableSurface:
				return 0;
			case RecastNavmeshModifier.Mode.WalkableSurfaceWithSeam:
			case RecastNavmeshModifier.Mode.WalkableSurfaceWithTag:
				return surfaceID;
			}
		}

		// Token: 0x06000C66 RID: 3174 RVA: 0x000449C0 File Offset: 0x00042BC0
		public void CollectRecastNavmeshModifiers()
		{
			List<RecastNavmeshModifier> list = ListPool<RecastNavmeshModifier>.Claim();
			RecastNavmeshModifier.GetAllInBounds(list, this.bounds);
			for (int i = 0; i < list.Count; i++)
			{
				this.AddNavmeshModifier(list[i]);
			}
			ListPool<RecastNavmeshModifier>.Release(ref list);
		}

		// Token: 0x06000C67 RID: 3175 RVA: 0x00044A04 File Offset: 0x00042C04
		private void AddNavmeshModifier(RecastNavmeshModifier navmeshModifier)
		{
			if (navmeshModifier.includeInScan == RecastNavmeshModifier.ScanInclusion.AlwaysExclude)
			{
				return;
			}
			if (navmeshModifier.includeInScan == RecastNavmeshModifier.ScanInclusion.Auto && ((this.mask >> navmeshModifier.gameObject.layer) & 1) == 0 && !this.tagMask.Contains(navmeshModifier.tag))
			{
				return;
			}
			MeshFilter meshFilter;
			Collider collider;
			Collider2D collider2D;
			navmeshModifier.ResolveMeshSource(out meshFilter, out collider, out collider2D);
			if (meshFilter != null)
			{
				Mesh sharedMesh = meshFilter.sharedMesh;
				MeshRenderer meshRenderer;
				RecastMeshGatherer.GatheredMesh gatheredMesh;
				if (meshFilter.TryGetComponent<MeshRenderer>(out meshRenderer) && sharedMesh != null && this.ConvertMeshToGatheredMesh(meshRenderer, meshFilter.sharedMesh, out gatheredMesh))
				{
					gatheredMesh.ApplyNavmeshModifier(navmeshModifier);
					this.meshes.Add(in gatheredMesh);
					return;
				}
			}
			else if (collider != null)
			{
				RecastMeshGatherer.GatheredMesh? gatheredMesh2 = this.ConvertColliderToGatheredMesh(collider);
				if (gatheredMesh2 != null)
				{
					RecastMeshGatherer.GatheredMesh valueOrDefault = gatheredMesh2.GetValueOrDefault();
					valueOrDefault.ApplyNavmeshModifier(navmeshModifier);
					this.meshes.Add(in valueOrDefault);
					return;
				}
			}
			else if (!(collider2D != null))
			{
				if (navmeshModifier.geometrySource == RecastNavmeshModifier.GeometrySource.Auto)
				{
					Debug.LogError("Couldn't get geometry source for RecastNavmeshModifier (" + navmeshModifier.gameObject.name + "). It didn't have a collider or MeshFilter+Renderer attached", navmeshModifier.gameObject);
					return;
				}
				Debug.LogError(string.Concat(new string[]
				{
					"Couldn't get geometry source for RecastNavmeshModifier (",
					navmeshModifier.gameObject.name,
					"). It didn't have a ",
					navmeshModifier.geometrySource.ToString(),
					" attached"
				}), navmeshModifier.gameObject);
			}
		}

		// Token: 0x06000C68 RID: 3176 RVA: 0x00044B7C File Offset: 0x00042D7C
		public void CollectTerrainMeshes(bool rasterizeTrees, float desiredChunkSize)
		{
			Terrain[] activeTerrains = Terrain.activeTerrains;
			if (activeTerrains.Length != 0)
			{
				for (int i = 0; i < activeTerrains.Length; i++)
				{
					if (!(activeTerrains[i].terrainData == null))
					{
						bool flag = this.GenerateTerrainChunks(activeTerrains[i], this.bounds, desiredChunkSize);
						if (rasterizeTrees && flag)
						{
							this.CollectTreeMeshes(activeTerrains[i]);
						}
					}
				}
			}
		}

		// Token: 0x06000C69 RID: 3177 RVA: 0x00044BD0 File Offset: 0x00042DD0
		private static int NonNegativeModulus(int x, int m)
		{
			int num = x % m;
			if (num >= 0)
			{
				return num;
			}
			return num + m;
		}

		// Token: 0x06000C6A RID: 3178 RVA: 0x00044BEA File Offset: 0x00042DEA
		private static int CeilDivision(int lhs, int rhs)
		{
			return (lhs + rhs - 1) / rhs;
		}

		// Token: 0x06000C6B RID: 3179 RVA: 0x00044BF4 File Offset: 0x00042DF4
		private unsafe static void GetAlphamaps(List<RecastGraph.PerTerrainLayerModification> perTerrainLayerModifications, out UnsafeSpan<UnsafeSpan<byte>> alphamaps, out UnsafeSpan<int> areaMapping, out UnsafeSpan<float> areaMappingThresholds, out float alphamapScale, TerrainData terrainData, int terrainDownsamplingFactor)
		{
			int heightmapResolution = terrainData.heightmapResolution;
			if (perTerrainLayerModifications.Count > 0)
			{
				alphamaps = new UnsafeSpan<UnsafeSpan<byte>>(Allocator.TempJob, terrainData.alphamapTextureCount);
				int alphamapResolution = terrainData.alphamapResolution;
				int num = math.max(0, math.min((int)math.log2((float)alphamapResolution), (int)math.floor(math.log2((float)alphamapResolution / (float)((heightmapResolution - 1) / terrainDownsamplingFactor)) + 0.001f)));
				for (int i = 0; i < terrainData.alphamapTextureCount; i++)
				{
					terrainData.GetAlphamapTexture(i);
					*alphamaps[i] = terrainData.GetAlphamapTexture(i).GetPixelData<byte>(num).AsUnsafeReadOnlySpan<byte>();
				}
				areaMapping = new UnsafeSpan<int>(Allocator.TempJob, terrainData.alphamapTextureCount * 4);
				areaMappingThresholds = new UnsafeSpan<float>(Allocator.TempJob, terrainData.alphamapTextureCount * 4);
				areaMapping.FillZeros<int>();
				areaMappingThresholds.Fill(0.5f);
				int num2 = alphamapResolution >> num;
				alphamapScale = (float)(num2 - 1) / (float)(heightmapResolution - 1);
				return;
			}
			alphamaps = default(UnsafeSpan<UnsafeSpan<byte>>);
			areaMapping = default(UnsafeSpan<int>);
			areaMappingThresholds = default(UnsafeSpan<float>);
			alphamapScale = 0f;
		}

		// Token: 0x06000C6C RID: 3180 RVA: 0x00044D18 File Offset: 0x00042F18
		private static void CalculateTerrainChunkLayout(float desiredChunkSize, Vector3 sampleSize, int terrainDownsamplingFactor, int heightmapResolution, Bounds bounds, Vector3 offset, out IntRect sampleRect, out Vector2Int chunks, out Vector2Int chunkSize)
		{
			chunkSize = new Vector2Int(Mathf.CeilToInt(Mathf.Max(desiredChunkSize / (sampleSize.x * (float)terrainDownsamplingFactor), 12f)) * terrainDownsamplingFactor, Mathf.CeilToInt(Mathf.Max(desiredChunkSize / (sampleSize.z * (float)terrainDownsamplingFactor), 12f)) * terrainDownsamplingFactor);
			chunkSize.x = Mathf.Min(chunkSize.x, heightmapResolution);
			chunkSize.y = Mathf.Min(chunkSize.y, heightmapResolution);
			Vector2Int vector2Int;
			if (float.IsFinite(bounds.size.x))
			{
				vector2Int = new Vector2Int(Mathf.FloorToInt((bounds.min.x - offset.x) / sampleSize.x), Mathf.FloorToInt((bounds.min.z - offset.z) / sampleSize.z));
				vector2Int.x -= RecastMeshGatherer.NonNegativeModulus(vector2Int.x, terrainDownsamplingFactor);
				vector2Int.y -= RecastMeshGatherer.NonNegativeModulus(vector2Int.y, terrainDownsamplingFactor);
				float num = (float)chunkSize.x * sampleSize.x;
				float num2 = (float)chunkSize.y * sampleSize.z;
				chunks = new Vector2Int(Mathf.CeilToInt((bounds.max.x - offset.x - (float)vector2Int.x * sampleSize.x) / num), Mathf.CeilToInt((bounds.max.z - offset.z - (float)vector2Int.y * sampleSize.z) / num2));
			}
			else
			{
				vector2Int = new Vector2Int(0, 0);
				chunks = new Vector2Int(RecastMeshGatherer.CeilDivision(heightmapResolution, chunkSize.x), RecastMeshGatherer.CeilDivision(heightmapResolution, chunkSize.y));
			}
			sampleRect = new IntRect(0, 0, chunks.x * chunkSize.x, chunks.y * chunkSize.y).Offset(vector2Int);
			IntRect intRect = new IntRect(0, 0, heightmapResolution - 1, heightmapResolution - 1);
			sampleRect = IntRect.Intersection(sampleRect, intRect);
			chunks = new Vector2Int(RecastMeshGatherer.CeilDivision(sampleRect.Width, chunkSize.x), RecastMeshGatherer.CeilDivision(sampleRect.Height, chunkSize.y));
		}

		// Token: 0x06000C6D RID: 3181 RVA: 0x00044F64 File Offset: 0x00043164
		private unsafe bool GenerateTerrainChunks(Terrain terrain, Bounds bounds, float desiredChunkSize)
		{
			TerrainData terrainData = terrain.terrainData;
			if (terrainData == null)
			{
				throw new ArgumentException("Terrain contains no terrain data");
			}
			Vector3 position = terrain.GetPosition();
			Vector3 size = terrainData.size;
			Vector3 vector = position + size * 0.5f;
			Bounds bounds2 = new Bounds(vector, size);
			if (!bounds2.Intersects(bounds))
			{
				return false;
			}
			Vector3 heightmapScale = terrainData.heightmapScale;
			heightmapScale.y = size.y;
			IntRect intRect;
			Vector2Int vector2Int;
			Vector2Int vector2Int2;
			RecastMeshGatherer.CalculateTerrainChunkLayout(desiredChunkSize, heightmapScale, this.terrainDownsamplingFactor, terrainData.heightmapResolution, bounds, position, out intRect, out vector2Int, out vector2Int2);
			if (!intRect.IsValid())
			{
				return false;
			}
			float[,] heights = terrainData.GetHeights(intRect.xmin, intRect.ymin, intRect.Width, intRect.Height);
			bool[,] holes = terrainData.GetHoles(intRect.xmin, intRect.ymin, intRect.Width - 1, intRect.Height - 1);
			UnsafeSpan<UnsafeSpan<byte>> unsafeSpan;
			UnsafeSpan<int> unsafeSpan2;
			UnsafeSpan<float> unsafeSpan3;
			float num;
			RecastMeshGatherer.GetAlphamaps(this.perTerrainLayerModifications, out unsafeSpan, out unsafeSpan2, out unsafeSpan3, out num, terrainData, this.terrainDownsamplingFactor);
			for (int i = 0; i < this.perTerrainLayerModifications.Count; i++)
			{
				RecastGraph.PerTerrainLayerModification perTerrainLayerModification = this.perTerrainLayerModifications[i];
				if (perTerrainLayerModification.layer >= 0 && perTerrainLayerModification.layer < unsafeSpan2.Length)
				{
					*unsafeSpan2[perTerrainLayerModification.layer] = RecastMeshGatherer.AreaFromSurfaceMode(perTerrainLayerModification.mode, perTerrainLayerModification.surfaceID) | ((perTerrainLayerModification.mode == RecastNavmeshModifier.Mode.WalkableSurfaceWithTag) ? 16384 : 0);
					*unsafeSpan3[perTerrainLayerModification.layer] = perTerrainLayerModification.threshold;
				}
			}
			ulong num2;
			UnsafeSpan<float> unsafeSpan4 = new UnsafeSpan<float>(heights, out num2);
			ulong num3;
			UnsafeSpan<bool> unsafeSpan5 = new UnsafeSpan<bool>(holes, out num3);
			Matrix4x4 matrix4x = Matrix4x4.TRS(position + new Vector3((float)intRect.xmin * heightmapScale.x, 0f, (float)intRect.ymin * heightmapScale.z), Quaternion.identity, heightmapScale);
			NativeArray<RecastMeshGatherer.JobGenerateHeightmapChunk.TerrainChunk> nativeArray = new NativeArray<RecastMeshGatherer.JobGenerateHeightmapChunk.TerrainChunk>(vector2Int.x * vector2Int.y, Allocator.TempJob, NativeArrayOptions.ClearMemory);
			new RecastMeshGatherer.JobGenerateHeightmapChunk
			{
				heights = unsafeSpan4,
				holes = unsafeSpan5,
				sampleRect = intRect,
				chunkSize = vector2Int2,
				chunks = vector2Int,
				stride = this.terrainDownsamplingFactor,
				alphamapScale = num,
				alphaMaps = unsafeSpan,
				areaMapping = unsafeSpan2,
				areaMappingThresholds = unsafeSpan3,
				output = nativeArray
			}.ScheduleParallel(vector2Int.x * vector2Int.y, 1, default(JobHandle)).Complete();
			for (int j = 0; j < vector2Int.y; j++)
			{
				for (int k = 0; k < vector2Int.x; k++)
				{
					RecastMeshGatherer.JobGenerateHeightmapChunk.TerrainChunk terrainChunk = nativeArray[j * vector2Int.x + k];
					int num4 = this.AddMeshBuffers(terrainChunk.verts, terrainChunk.tris);
					int num5 = -1;
					if (terrainChunk.tags.Length > 0)
					{
						num5 = this.tagsBuffers.Length;
						this.tagsBuffers.Add(in terrainChunk.tags);
					}
					Vector3 vector2 = matrix4x.MultiplyPoint3x4(new Vector3((float)(k * vector2Int2.x), 0f, (float)(j * vector2Int2.y)));
					Vector3 vector3 = matrix4x.MultiplyPoint3x4(new Vector3((float)Mathf.Min((k + 1) * vector2Int2.x, intRect.xmax), 1f, (float)Mathf.Min((j + 1) * vector2Int2.y, intRect.ymax)));
					vector2 = Vector3.Max(vector2, bounds.min);
					vector3 = Vector3.Min(vector3, bounds.max);
					Bounds bounds3 = default(Bounds);
					bounds3.SetMinMax(vector2, vector3);
					RecastMeshGatherer.GatheredMesh gatheredMesh = new RecastMeshGatherer.GatheredMesh
					{
						meshDataIndex = num4,
						bounds = bounds3,
						indexStart = 0,
						indexEnd = -1,
						areaIsTag = false,
						area = 0,
						tagDataIndex = num5,
						solid = false,
						matrix = matrix4x,
						doubleSided = false,
						flatten = false
					};
					gatheredMesh.ApplyLayerModification(this.modificationsByLayer[terrain.gameObject.layer]);
					this.meshes.Add(in gatheredMesh);
				}
			}
			nativeArray.Dispose();
			unsafeSpan.Free(Allocator.TempJob);
			unsafeSpan2.Free(Allocator.TempJob);
			unsafeSpan3.Free(Allocator.TempJob);
			UnsafeUtility.ReleaseGCObject(num2);
			UnsafeUtility.ReleaseGCObject(num3);
			return true;
		}

		// Token: 0x06000C6E RID: 3182 RVA: 0x000453F0 File Offset: 0x000435F0
		private unsafe void CollectTreeMeshes(Terrain terrain)
		{
			TerrainData terrainData = terrain.terrainData;
			TreeInstance[] treeInstances = terrainData.treeInstances;
			TreePrototype[] treePrototypes = terrainData.treePrototypes;
			UnsafeSpan<RecastMeshGatherer.TreeInfo> unsafeSpan = new UnsafeSpan<RecastMeshGatherer.TreeInfo>(Allocator.Temp, treePrototypes.Length);
			List<Collider> list = ListPool<Collider>.Claim();
			UnsafeList<RecastMeshGatherer.GatheredMesh> unsafeList = new UnsafeList<RecastMeshGatherer.GatheredMesh>(4, Allocator.Temp, NativeArrayOptions.UninitializedMemory);
			List<Mesh> list2 = this.meshData;
			Dictionary<RecastMeshGatherer.MeshCacheItem, int> dictionary = this.cachedMeshes;
			this.meshData = new List<Mesh>();
			this.cachedMeshes = new Dictionary<RecastMeshGatherer.MeshCacheItem, int>();
			Dictionary<GameObject, RecastMeshGatherer.TreeInfo> dictionary2 = new Dictionary<GameObject, RecastMeshGatherer.TreeInfo>();
			for (int i = 0; i < treePrototypes.Length; i++)
			{
				TreePrototype treePrototype = treePrototypes[i];
				if (treePrototype.prefab == null)
				{
					*unsafeSpan[i] = new RecastMeshGatherer.TreeInfo
					{
						submeshIndices = new UnsafeList<int>(0, Allocator.Temp, NativeArrayOptions.UninitializedMemory)
					};
				}
				else
				{
					RecastMeshGatherer.TreeInfo treeInfo;
					if (!dictionary2.TryGetValue(treePrototype.prefab, out treeInfo))
					{
						treeInfo.submeshIndices = new UnsafeList<int>(4, Allocator.Temp, NativeArrayOptions.UninitializedMemory);
						LODGroup lodgroup;
						treeInfo.supportsRotation = treePrototype.prefab.TryGetComponent<LODGroup>(out lodgroup);
						treeInfo.localScale = treePrototype.prefab.transform.localScale;
						Matrix4x4 inverse = treePrototype.prefab.transform.localToWorldMatrix.inverse;
						list.Clear();
						treePrototype.prefab.GetComponentsInChildren<Collider>(false, list);
						int j = 0;
						while (j < list.Count)
						{
							Collider collider = list[j];
							RecastNavmeshModifier recastNavmeshModifier;
							bool flag = collider.gameObject.TryGetComponent<RecastNavmeshModifier>(out recastNavmeshModifier) && recastNavmeshModifier.enabled;
							if (flag)
							{
								if (recastNavmeshModifier.includeInScan != RecastNavmeshModifier.ScanInclusion.AlwaysExclude)
								{
									if (recastNavmeshModifier.includeInScan == RecastNavmeshModifier.ScanInclusion.AlwaysInclude || this.ShouldIncludeColliderInPrefab(collider))
									{
										goto IL_019F;
									}
								}
							}
							else if (this.ShouldIncludeColliderInPrefab(collider))
							{
								goto IL_019F;
							}
							IL_0222:
							j++;
							continue;
							IL_019F:
							RecastMeshGatherer.GatheredMesh? gatheredMesh = this.ConvertColliderToGatheredMesh(collider, inverse * collider.transform.localToWorldMatrix);
							if (gatheredMesh != null)
							{
								RecastMeshGatherer.GatheredMesh valueOrDefault = gatheredMesh.GetValueOrDefault();
								if (flag)
								{
									valueOrDefault.ApplyNavmeshModifier(recastNavmeshModifier);
								}
								else
								{
									valueOrDefault.ApplyLayerModification(this.modificationsByLayer[collider.gameObject.layer]);
								}
								valueOrDefault.RecalculateBounds();
								int length = unsafeList.Length;
								treeInfo.submeshIndices.Add(in length);
								unsafeList.Add(in valueOrDefault);
								goto IL_0222;
							}
							goto IL_0222;
						}
						dictionary2[treePrototype.prefab] = treeInfo;
					}
					*unsafeSpan[i] = treeInfo;
				}
			}
			ListPool<Collider>.Release(ref list);
			RecastMeshGatherer.GetMissingMeshDataAndBounds(this.meshData, ref unsafeList, ref this.vertexBuffers, ref this.triangleBuffers);
			this.meshData = list2;
			this.cachedMeshes = dictionary;
			ulong num;
			UnsafeSpan<TreeInstance> unsafeSpan2 = new UnsafeSpan<TreeInstance>(treeInstances, out num);
			Bounds bounds = this.bounds;
			float3 @float = terrain.transform.position;
			float3 float2 = terrainData.size;
			RecastMeshGatherer.ConvertTreesToMeshes(ref unsafeSpan2, ref @float, ref float2, ref unsafeSpan, ref unsafeList, ref bounds, ref this.meshes);
			unsafeSpan.Free(Allocator.Temp);
			UnsafeUtility.ReleaseGCObject(num);
		}

		// Token: 0x06000C6F RID: 3183 RVA: 0x000456EA File Offset: 0x000438EA
		[BurstCompile]
		private static void ConvertTreesToMeshes(ref UnsafeSpan<TreeInstance> treeInstances, ref float3 terrainPos, ref float3 terrainSize, ref UnsafeSpan<RecastMeshGatherer.TreeInfo> treeInfos, ref UnsafeList<RecastMeshGatherer.GatheredMesh> allSubmeshes, ref Bounds graphBounds, ref UnsafeList<RecastMeshGatherer.GatheredMesh> meshes)
		{
			RecastMeshGatherer.ConvertTreesToMeshes_00000BA9$BurstDirectCall.Invoke(ref treeInstances, ref terrainPos, ref terrainSize, ref treeInfos, ref allSubmeshes, ref graphBounds, ref meshes);
		}

		// Token: 0x06000C70 RID: 3184 RVA: 0x000456FC File Offset: 0x000438FC
		private bool ShouldIncludeCollider(Collider collider)
		{
			RecastNavmeshModifier recastNavmeshModifier;
			return this.ShouldIncludeColliderInPrefab(collider) && collider.bounds.Intersects(this.bounds) && (!collider.TryGetComponent<RecastNavmeshModifier>(out recastNavmeshModifier) || !recastNavmeshModifier.enabled);
		}

		// Token: 0x06000C71 RID: 3185 RVA: 0x00045740 File Offset: 0x00043940
		private bool ShouldIncludeColliderInPrefab(Collider collider)
		{
			if (!collider.enabled || collider.isTrigger)
			{
				return false;
			}
			GameObject gameObject = collider.gameObject;
			if (((this.mask >> gameObject.layer) & 1) != 0)
			{
				return true;
			}
			for (int i = 0; i < this.tagMask.Count; i++)
			{
				if (gameObject.CompareTag(this.tagMask[i]))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06000C72 RID: 3186 RVA: 0x000457B0 File Offset: 0x000439B0
		public void CollectColliderMeshes()
		{
			if (this.tagMask.Count == 0 && this.mask == 0)
			{
				return;
			}
			int num = 256;
			Collider[] array = null;
			bool flag = math.all(math.isfinite(this.bounds.extents));
			if (!flag)
			{
				array = UnityCompatibility.FindObjectsByTypeSorted<Collider>();
				num = array.Length;
			}
			else
			{
				do
				{
					if (array != null)
					{
						ArrayPool<Collider>.Release(ref array, false);
					}
					array = ArrayPool<Collider>.Claim(num * 4);
					num = this.physicsScene.OverlapBox(this.bounds.center, this.bounds.extents, array, Quaternion.identity, -1, QueryTriggerInteraction.Ignore);
				}
				while (num == array.Length);
			}
			for (int i = 0; i < num; i++)
			{
				Collider collider = array[i];
				if (this.ShouldIncludeCollider(collider))
				{
					RecastMeshGatherer.GatheredMesh? gatheredMesh = this.ConvertColliderToGatheredMesh(collider);
					if (gatheredMesh != null)
					{
						RecastMeshGatherer.GatheredMesh valueOrDefault = gatheredMesh.GetValueOrDefault();
						valueOrDefault.ApplyLayerModification(this.modificationsByLayer[collider.gameObject.layer]);
						this.meshes.Add(in valueOrDefault);
					}
				}
			}
			if (flag)
			{
				ArrayPool<Collider>.Release(ref array, false);
			}
		}

		// Token: 0x06000C73 RID: 3187 RVA: 0x000458CD File Offset: 0x00043ACD
		private RecastMeshGatherer.GatheredMesh? ConvertColliderToGatheredMesh(Collider col)
		{
			return this.ConvertColliderToGatheredMesh(col, col.transform.localToWorldMatrix);
		}

		// Token: 0x06000C74 RID: 3188 RVA: 0x000458E4 File Offset: 0x00043AE4
		public RecastMeshGatherer.GatheredMesh? ConvertColliderToGatheredMesh(Collider col, Matrix4x4 localToWorldMatrix)
		{
			BoxCollider boxCollider = col as BoxCollider;
			if (boxCollider != null)
			{
				return new RecastMeshGatherer.GatheredMesh?(this.RasterizeBoxCollider(boxCollider, localToWorldMatrix));
			}
			if (col is SphereCollider || col is CapsuleCollider)
			{
				SphereCollider sphereCollider = col as SphereCollider;
				CapsuleCollider capsuleCollider = col as CapsuleCollider;
				float num = ((sphereCollider != null) ? sphereCollider.radius : capsuleCollider.radius);
				float num2 = ((sphereCollider != null) ? 0f : (capsuleCollider.height * 0.5f / num - 1f));
				Quaternion quaternion = Quaternion.identity;
				if (capsuleCollider != null)
				{
					quaternion = Quaternion.Euler((float)((capsuleCollider.direction == 2) ? 90 : 0), 0f, (float)((capsuleCollider.direction == 0) ? 90 : 0));
				}
				Matrix4x4 matrix4x = Matrix4x4.TRS((sphereCollider != null) ? sphereCollider.center : capsuleCollider.center, quaternion, Vector3.one * num);
				matrix4x = localToWorldMatrix * matrix4x;
				return new RecastMeshGatherer.GatheredMesh?(this.RasterizeCapsuleCollider(num, num2, col.bounds, matrix4x));
			}
			MeshCollider meshCollider = col as MeshCollider;
			if (meshCollider != null)
			{
				return this.GetColliderMesh(meshCollider, localToWorldMatrix);
			}
			return null;
		}

		// Token: 0x06000C75 RID: 3189 RVA: 0x00045A10 File Offset: 0x00043C10
		private RecastMeshGatherer.GatheredMesh RasterizeBoxCollider(BoxCollider collider, Matrix4x4 localToWorldMatrix)
		{
			Matrix4x4 matrix4x = Matrix4x4.TRS(collider.center, Quaternion.identity, collider.size * 0.5f);
			matrix4x = localToWorldMatrix * matrix4x;
			int num;
			if (!this.cachedMeshes.TryGetValue(RecastMeshGatherer.MeshCacheItem.Box, out num))
			{
				num = this.AddMeshBuffers(RecastMeshGatherer.BoxColliderVerts, RecastMeshGatherer.BoxColliderTris);
				this.cachedMeshes[RecastMeshGatherer.MeshCacheItem.Box] = num;
			}
			return new RecastMeshGatherer.GatheredMesh
			{
				meshDataIndex = num,
				bounds = collider.bounds,
				indexStart = 0,
				indexEnd = -1,
				areaIsTag = false,
				area = 0,
				tagDataIndex = -1,
				solid = true,
				matrix = matrix4x,
				doubleSided = false,
				flatten = false
			};
		}

		// Token: 0x06000C76 RID: 3190 RVA: 0x00045AE4 File Offset: 0x00043CE4
		private static int CircleSteps(Matrix4x4 matrix, float radius, float maxError)
		{
			float num = math.sqrt(math.max(math.max(math.lengthsq(matrix.GetColumn(0)), math.lengthsq(matrix.GetColumn(1))), math.lengthsq(matrix.GetColumn(2))));
			float num2 = radius * num;
			float num3 = 1f - maxError / num2;
			if (num3 >= 0f)
			{
				return (int)math.ceil(3.1415927f / math.acos(num3));
			}
			return 3;
		}

		// Token: 0x06000C77 RID: 3191 RVA: 0x00045B70 File Offset: 0x00043D70
		private static float CircleRadiusAdjustmentFactor(int steps)
		{
			return 0.5f * (1f - math.cos(6.2831855f / (float)steps));
		}

		// Token: 0x06000C78 RID: 3192 RVA: 0x00045B8C File Offset: 0x00043D8C
		private unsafe RecastMeshGatherer.GatheredMesh RasterizeCapsuleCollider(float radius, float height, Bounds bounds, Matrix4x4 localToWorldMatrix)
		{
			int num = RecastMeshGatherer.CircleSteps(localToWorldMatrix, radius, this.maxColliderApproximationError);
			int num2 = num;
			RecastMeshGatherer.MeshCacheItem meshCacheItem = new RecastMeshGatherer.MeshCacheItem
			{
				type = RecastMeshGatherer.MeshType.Capsule,
				mesh = null,
				rows = num,
				quantizedHeight = Mathf.RoundToInt(height / this.maxColliderApproximationError)
			};
			int num3;
			if (!this.cachedMeshes.TryGetValue(meshCacheItem, out num3))
			{
				UnsafeSpan<Vector3> unsafeSpan = new UnsafeSpan<Vector3>(Allocator.Persistent, num * num2 + 2);
				UnsafeSpan<int> unsafeSpan2 = new UnsafeSpan<int>(Allocator.Persistent, num * num2 * 2 * 3);
				for (int i = 0; i < num; i++)
				{
					for (int j = 0; j < num2; j++)
					{
						*unsafeSpan[j + i * num2] = new Vector3(Mathf.Cos((float)j * 3.1415927f * 2f / (float)num2) * Mathf.Sin((float)i * 3.1415927f / (float)(num - 1)), Mathf.Cos((float)i * 3.1415927f / (float)(num - 1)) + ((i < num / 2) ? height : (-height)), Mathf.Sin((float)j * 3.1415927f * 2f / (float)num2) * Mathf.Sin((float)i * 3.1415927f / (float)(num - 1)));
					}
				}
				*unsafeSpan[unsafeSpan.Length - 1] = Vector3.up;
				*unsafeSpan[unsafeSpan.Length - 2] = Vector3.down;
				int num4 = 0;
				int k = 0;
				int num5 = num2 - 1;
				while (k < num2)
				{
					*unsafeSpan2[num4] = unsafeSpan.Length - 1;
					*unsafeSpan2[num4 + 1] = num5;
					*unsafeSpan2[num4 + 2] = k;
					num4 += 3;
					num5 = k++;
				}
				for (int l = 1; l < num; l++)
				{
					int m = 0;
					int num6 = num2 - 1;
					while (m < num2)
					{
						*unsafeSpan2[num4] = l * num2 + m;
						*unsafeSpan2[num4 + 1] = l * num2 + num6;
						*unsafeSpan2[num4 + 2] = (l - 1) * num2 + m;
						num4 += 3;
						*unsafeSpan2[num4] = (l - 1) * num2 + num6;
						*unsafeSpan2[num4 + 1] = (l - 1) * num2 + m;
						*unsafeSpan2[num4 + 2] = l * num2 + num6;
						num4 += 3;
						num6 = m++;
					}
				}
				int n = 0;
				int num7 = num2 - 1;
				while (n < num2)
				{
					*unsafeSpan2[num4] = unsafeSpan.Length - 2;
					*unsafeSpan2[num4 + 1] = (num - 1) * num2 + num7;
					*unsafeSpan2[num4 + 2] = (num - 1) * num2 + n;
					num4 += 3;
					num7 = n++;
				}
				num3 = this.AddMeshBuffers(unsafeSpan, unsafeSpan2);
				this.cachedMeshes[meshCacheItem] = num3;
			}
			return new RecastMeshGatherer.GatheredMesh
			{
				meshDataIndex = num3,
				bounds = bounds,
				areaIsTag = false,
				area = 0,
				tagDataIndex = -1,
				indexStart = 0,
				indexEnd = -1,
				solid = true,
				matrix = localToWorldMatrix,
				doubleSided = false,
				flatten = false
			};
		}

		// Token: 0x06000C79 RID: 3193 RVA: 0x00045ED8 File Offset: 0x000440D8
		private bool ShouldIncludeCollider2D(Collider2D collider)
		{
			if (((this.mask >> collider.gameObject.layer) & 1) != 0)
			{
				return true;
			}
			RecastNavmeshModifier recastNavmeshModifier;
			if ((collider.attachedRigidbody ?? collider).TryGetComponent<RecastNavmeshModifier>(out recastNavmeshModifier) && recastNavmeshModifier.enabled && recastNavmeshModifier.includeInScan == RecastNavmeshModifier.ScanInclusion.AlwaysInclude)
			{
				return true;
			}
			for (int i = 0; i < this.tagMask.Count; i++)
			{
				if (collider.CompareTag(this.tagMask[i]))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06000C7A RID: 3194 RVA: 0x00045F5C File Offset: 0x0004415C
		public unsafe void Collect2DColliderMeshes()
		{
			if (this.tagMask.Count == 0 && this.mask == 0)
			{
				return;
			}
			int num = 256;
			Collider2D[] array = null;
			bool flag = math.isfinite(this.bounds.extents.x) && math.isfinite(this.bounds.extents.y);
			if (!flag)
			{
				array = UnityCompatibility.FindObjectsByTypeSorted<Collider2D>();
				num = array.Length;
			}
			else
			{
				Vector2 vector = this.bounds.min;
				Vector2 vector2 = this.bounds.max;
				ContactFilter2D contactFilter2D = default(ContactFilter2D).NoFilter();
				contactFilter2D.useTriggers = false;
				do
				{
					if (array != null)
					{
						ArrayPool<Collider2D>.Release(ref array, false);
					}
					array = ArrayPool<Collider2D>.Claim(num * 4);
					num = this.physicsScene2D.OverlapArea(vector, vector2, contactFilter2D, array);
				}
				while (num == array.Length);
			}
			for (int i = 0; i < num; i++)
			{
				if (!this.ShouldIncludeCollider2D(array[i]))
				{
					array[i] = null;
				}
			}
			UnsafeSpan<float3> unsafeSpan;
			UnsafeSpan<int> unsafeSpan2;
			UnsafeSpan<ColliderMeshBuilder2D.ShapeMesh> unsafeSpan3;
			int num2 = ColliderMeshBuilder2D.GenerateMeshesFromColliders(array, num, this.maxColliderApproximationError, out unsafeSpan, out unsafeSpan2, out unsafeSpan3);
			int num3 = this.AddMeshBuffers(unsafeSpan.Reinterpret<Vector3>(), unsafeSpan2);
			for (int j = 0; j < num2; j++)
			{
				ColliderMeshBuilder2D.ShapeMesh shapeMesh = *unsafeSpan3[j];
				if (this.bounds.Intersects(shapeMesh.bounds))
				{
					Collider2D collider2D = array[shapeMesh.tag];
					RecastNavmeshModifier recastNavmeshModifier;
					(collider2D.attachedRigidbody ?? collider2D).TryGetComponent<RecastNavmeshModifier>(out recastNavmeshModifier);
					RecastMeshGatherer.GatheredMesh gatheredMesh = new RecastMeshGatherer.GatheredMesh
					{
						meshDataIndex = num3,
						bounds = shapeMesh.bounds,
						indexStart = shapeMesh.startIndex,
						indexEnd = shapeMesh.endIndex,
						tagDataIndex = -1,
						areaIsTag = false,
						area = -1,
						solid = false,
						matrix = shapeMesh.matrix,
						doubleSided = true,
						flatten = true
					};
					if (recastNavmeshModifier != null)
					{
						if (recastNavmeshModifier.includeInScan == RecastNavmeshModifier.ScanInclusion.AlwaysExclude)
						{
							goto IL_0247;
						}
						gatheredMesh.ApplyNavmeshModifier(recastNavmeshModifier);
					}
					else
					{
						gatheredMesh.ApplyLayerModification(this.modificationsByLayer2D[collider2D.gameObject.layer]);
					}
					gatheredMesh.solid = false;
					this.meshes.Add(in gatheredMesh);
				}
				IL_0247:;
			}
			if (flag)
			{
				ArrayPool<Collider2D>.Release(ref array, false);
			}
			unsafeSpan3.Free();
		}

		// Token: 0x06000C7C RID: 3196 RVA: 0x000462D8 File Offset: 0x000444D8
		[BurstCompile]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal unsafe static void CalculateBounds$BurstManaged(ref UnsafeSpan<float3> vertices, ref float4x4 localToWorldMatrix, out Bounds bounds)
		{
			if (vertices.Length == 0)
			{
				bounds = default(Bounds);
				return;
			}
			float3 @float = float.NegativeInfinity;
			float3 float2 = float.PositiveInfinity;
			uint num = 0U;
			while ((ulong)num < (ulong)((long)vertices.Length))
			{
				float3 float3 = math.transform(localToWorldMatrix, *vertices[num]);
				@float = math.max(@float, float3);
				float2 = math.min(float2, float3);
				num += 1U;
			}
			bounds = new Bounds((float2 + @float) * 0.5f, @float - float2);
		}

		// Token: 0x06000C7D RID: 3197 RVA: 0x00046374 File Offset: 0x00044574
		[BurstCompile]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal static void PatchMissingMeshDataAndBounds$BurstManaged(ref UnsafeList<RecastMeshGatherer.GatheredMesh> gatheredMeshes, ref UnsafeList<UnsafeSpan<Vector3>> vertexBuffers, int meshBufferOffset)
		{
			UnsafeSpan<RecastMeshGatherer.GatheredMesh> unsafeSpan = gatheredMeshes.AsUnsafeSpan<RecastMeshGatherer.GatheredMesh>();
			for (int i = 0; i < gatheredMeshes.Length; i++)
			{
				ref RecastMeshGatherer.GatheredMesh ptr = ref unsafeSpan[i];
				if (ptr.meshDataIndex >= 0)
				{
					int num = meshBufferOffset + ptr.meshDataIndex;
					ptr.meshDataIndex = -num - 1;
				}
				if (ptr.bounds == default(Bounds))
				{
					int num2 = -(ptr.meshDataIndex + 1);
					UnsafeSpan<float3> unsafeSpan2 = vertexBuffers[num2].Reinterpret<float3>();
					float4x4 float4x = ptr.matrix;
					RecastMeshGatherer.CalculateBounds(ref unsafeSpan2, ref float4x, out ptr.bounds);
				}
			}
		}

		// Token: 0x06000C7E RID: 3198 RVA: 0x00046418 File Offset: 0x00044618
		[BurstCompile]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal unsafe static void CreateRasterizationMeshes$BurstManaged(ref UnsafeList<RecastMeshGatherer.GatheredMesh> meshes, ref UnsafeList<UnsafeSpan<Vector3>> vertexBuffers, ref UnsafeList<UnsafeSpan<int>> triangleBuffers, ref UnsafeList<UnsafeSpan<int>> tagsBuffers, ref UnsafeSpan<RasterizationMesh> rasterizationMeshesOutput)
		{
			for (int i = 0; i < meshes.Length; i++)
			{
				RecastMeshGatherer.GatheredMesh gatheredMesh = meshes[i];
				int num = -(gatheredMesh.meshDataIndex + 1);
				Bounds bounds = gatheredMesh.bounds;
				UnsafeSpan<int> unsafeSpan = triangleBuffers[num];
				*rasterizationMeshesOutput[i] = new RasterizationMesh
				{
					vertices = vertexBuffers[num].Reinterpret<float3>(),
					triangles = unsafeSpan.Slice(gatheredMesh.indexStart, ((gatheredMesh.indexEnd != -1) ? gatheredMesh.indexEnd : unsafeSpan.Length) - gatheredMesh.indexStart),
					area = (gatheredMesh.area | (gatheredMesh.areaIsTag ? 16384 : 0)),
					areas = ((gatheredMesh.tagDataIndex != -1) ? tagsBuffers[gatheredMesh.tagDataIndex] : default(UnsafeSpan<int>)),
					bounds = bounds,
					matrix = gatheredMesh.matrix,
					solid = gatheredMesh.solid,
					doubleSided = gatheredMesh.doubleSided,
					flatten = gatheredMesh.flatten
				};
			}
		}

		// Token: 0x06000C7F RID: 3199 RVA: 0x00046544 File Offset: 0x00044744
		[BurstCompile]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal unsafe static void ConvertTreesToMeshes$BurstManaged(ref UnsafeSpan<TreeInstance> treeInstances, ref float3 terrainPos, ref float3 terrainSize, ref UnsafeSpan<RecastMeshGatherer.TreeInfo> treeInfos, ref UnsafeList<RecastMeshGatherer.GatheredMesh> allSubmeshes, ref Bounds graphBounds, ref UnsafeList<RecastMeshGatherer.GatheredMesh> meshes)
		{
			for (int i = 0; i < treeInstances.Length; i++)
			{
				TreeInstance treeInstance = *treeInstances[i];
				RecastMeshGatherer.TreeInfo treeInfo = *treeInfos[treeInstance.prototypeIndex];
				if (!treeInfo.submeshIndices.IsEmpty)
				{
					float3 @float = terrainPos + treeInstance.position * terrainSize;
					float3 float2 = new float3(treeInstance.widthScale, treeInstance.heightScale, treeInstance.widthScale) * treeInfo.localScale;
					Quaternion quaternion = (treeInfo.supportsRotation ? Quaternion.AngleAxis(treeInstance.rotation * 57.29578f, Vector3.up) : Quaternion.identity);
					Matrix4x4 matrix4x = Matrix4x4.TRS(@float, quaternion, float2);
					for (int j = 0; j < treeInfo.submeshIndices.Length; j++)
					{
						RecastMeshGatherer.GatheredMesh gatheredMesh = allSubmeshes[treeInfo.submeshIndices[j]];
						Matrix4x4 matrix4x2 = matrix4x * gatheredMesh.matrix;
						gatheredMesh.matrix = matrix4x2;
						gatheredMesh.bounds = MathExtensions.BoundsOfTransformedBounds(matrix4x2, gatheredMesh.bounds);
						if (graphBounds.Intersects(gatheredMesh.bounds))
						{
							meshes.Add(in gatheredMesh);
						}
					}
				}
			}
		}

		// Token: 0x04000831 RID: 2097
		private readonly int terrainDownsamplingFactor;

		// Token: 0x04000832 RID: 2098
		public readonly LayerMask mask;

		// Token: 0x04000833 RID: 2099
		public readonly List<string> tagMask;

		// Token: 0x04000834 RID: 2100
		private readonly float maxColliderApproximationError;

		// Token: 0x04000835 RID: 2101
		public readonly Bounds bounds;

		// Token: 0x04000836 RID: 2102
		public readonly PhysicsScene physicsScene;

		// Token: 0x04000837 RID: 2103
		public readonly PhysicsScene2D physicsScene2D;

		// Token: 0x04000838 RID: 2104
		private Dictionary<RecastMeshGatherer.MeshCacheItem, int> cachedMeshes = new Dictionary<RecastMeshGatherer.MeshCacheItem, int>();

		// Token: 0x04000839 RID: 2105
		private UnsafeList<UnsafeSpan<Vector3>> vertexBuffers;

		// Token: 0x0400083A RID: 2106
		private UnsafeList<UnsafeSpan<int>> triangleBuffers;

		// Token: 0x0400083B RID: 2107
		private UnsafeList<UnsafeSpan<int>> tagsBuffers;

		// Token: 0x0400083C RID: 2108
		private List<Mesh> meshData;

		// Token: 0x0400083D RID: 2109
		private readonly RecastGraph.PerLayerModification[] modificationsByLayer;

		// Token: 0x0400083E RID: 2110
		private readonly RecastGraph.PerLayerModification[] modificationsByLayer2D;

		// Token: 0x0400083F RID: 2111
		private readonly List<RecastGraph.PerTerrainLayerModification> perTerrainLayerModifications;

		// Token: 0x04000840 RID: 2112
		private bool anyNonReadableMesh;

		// Token: 0x04000841 RID: 2113
		private UnsafeList<RecastMeshGatherer.GatheredMesh> meshes;

		// Token: 0x04000842 RID: 2114
		private List<Material> dummyMaterials = new List<Material>();

		// Token: 0x04000843 RID: 2115
		private static readonly int[] BoxColliderTris = new int[]
		{
			0, 1, 2, 0, 2, 3, 6, 5, 4, 7,
			6, 4, 0, 5, 1, 0, 4, 5, 1, 6,
			2, 1, 5, 6, 2, 7, 3, 2, 6, 7,
			3, 4, 0, 3, 7, 4
		};

		// Token: 0x04000844 RID: 2116
		private static readonly Vector3[] BoxColliderVerts = new Vector3[]
		{
			new Vector3(-1f, -1f, -1f),
			new Vector3(1f, -1f, -1f),
			new Vector3(1f, -1f, 1f),
			new Vector3(-1f, -1f, 1f),
			new Vector3(-1f, 1f, -1f),
			new Vector3(1f, 1f, -1f),
			new Vector3(1f, 1f, 1f),
			new Vector3(-1f, 1f, 1f)
		};

		// Token: 0x020001AB RID: 427
		private static class Markers
		{
			// Token: 0x04000845 RID: 2117
			public static readonly ProfilerMarker MarkerCalculateBounds = new ProfilerMarker("CalculateBounds");

			// Token: 0x04000846 RID: 2118
			public static readonly ProfilerMarker MarkerGetMissingMeshDataAndBounds = new ProfilerMarker("GetMissingMeshDataAndBounds");

			// Token: 0x04000847 RID: 2119
			public static readonly ProfilerMarker MarkerPatchMissingMeshDataAndBounds = new ProfilerMarker("PatchMissingMeshDataAndBounds");

			// Token: 0x04000848 RID: 2120
			public static readonly ProfilerMarker MarkerCreateRasterizationMeshes = new ProfilerMarker("CreateRasterizationMeshes");
		}

		// Token: 0x020001AC RID: 428
		private struct TreeInfo
		{
			// Token: 0x04000849 RID: 2121
			public UnsafeList<int> submeshIndices;

			// Token: 0x0400084A RID: 2122
			public Vector3 localScale;

			// Token: 0x0400084B RID: 2123
			public bool supportsRotation;
		}

		// Token: 0x020001AD RID: 429
		public struct MeshCollection : IArenaDisposable
		{
			// Token: 0x06000C81 RID: 3201 RVA: 0x000466D8 File Offset: 0x000448D8
			public MeshCollection(UnsafeList<UnsafeSpan<Vector3>> vertexBuffers, UnsafeList<UnsafeSpan<int>> triangleBuffers, UnsafeList<UnsafeSpan<int>> tagsBuffers, NativeArray<RasterizationMesh> meshes)
			{
				this.vertexBuffers = vertexBuffers;
				this.triangleBuffers = triangleBuffers;
				this.tagsBuffers = tagsBuffers;
				this.meshes = meshes;
			}

			// Token: 0x06000C82 RID: 3202 RVA: 0x000466F8 File Offset: 0x000448F8
			void IArenaDisposable.DisposeWith(DisposeArena arena)
			{
				for (int i = 0; i < this.vertexBuffers.Length; i++)
				{
					arena.Add<Vector3>(this.vertexBuffers[i]);
					arena.Add<int>(this.triangleBuffers[i]);
				}
				for (int j = 0; j < this.tagsBuffers.Length; j++)
				{
					arena.Add<int>(this.tagsBuffers[j]);
				}
				arena.Add<RasterizationMesh>(this.meshes);
				arena.Add<UnsafeSpan<Vector3>>(this.vertexBuffers);
				arena.Add<UnsafeSpan<int>>(this.triangleBuffers);
				arena.Add<UnsafeSpan<int>>(this.tagsBuffers);
			}

			// Token: 0x0400084C RID: 2124
			private UnsafeList<UnsafeSpan<Vector3>> vertexBuffers;

			// Token: 0x0400084D RID: 2125
			private UnsafeList<UnsafeSpan<int>> triangleBuffers;

			// Token: 0x0400084E RID: 2126
			private UnsafeList<UnsafeSpan<int>> tagsBuffers;

			// Token: 0x0400084F RID: 2127
			public NativeArray<RasterizationMesh> meshes;
		}

		// Token: 0x020001AE RID: 430
		public struct GatheredMesh
		{
			// Token: 0x06000C83 RID: 3203 RVA: 0x00046797 File Offset: 0x00044997
			public void RecalculateBounds()
			{
				this.bounds = default(Bounds);
			}

			// Token: 0x06000C84 RID: 3204 RVA: 0x000467A5 File Offset: 0x000449A5
			public void ApplyNavmeshModifier(RecastNavmeshModifier navmeshModifier)
			{
				this.area = RecastMeshGatherer.AreaFromSurfaceMode(navmeshModifier.mode, navmeshModifier.surfaceID);
				this.areaIsTag = navmeshModifier.mode == RecastNavmeshModifier.Mode.WalkableSurfaceWithTag;
				this.solid |= navmeshModifier.solid;
			}

			// Token: 0x06000C85 RID: 3205 RVA: 0x000467E0 File Offset: 0x000449E0
			public void ApplyLayerModification(RecastGraph.PerLayerModification modification)
			{
				this.area = RecastMeshGatherer.AreaFromSurfaceMode(modification.mode, modification.surfaceID);
				this.areaIsTag = modification.mode == RecastNavmeshModifier.Mode.WalkableSurfaceWithTag;
			}

			// Token: 0x04000850 RID: 2128
			public int meshDataIndex;

			// Token: 0x04000851 RID: 2129
			public int area;

			// Token: 0x04000852 RID: 2130
			public int tagDataIndex;

			// Token: 0x04000853 RID: 2131
			public int indexStart;

			// Token: 0x04000854 RID: 2132
			public int indexEnd;

			// Token: 0x04000855 RID: 2133
			public Bounds bounds;

			// Token: 0x04000856 RID: 2134
			public Matrix4x4 matrix;

			// Token: 0x04000857 RID: 2135
			public bool solid;

			// Token: 0x04000858 RID: 2136
			public bool doubleSided;

			// Token: 0x04000859 RID: 2137
			public bool flatten;

			// Token: 0x0400085A RID: 2138
			public bool areaIsTag;
		}

		// Token: 0x020001AF RID: 431
		private enum MeshType
		{
			// Token: 0x0400085C RID: 2140
			Mesh,
			// Token: 0x0400085D RID: 2141
			Box,
			// Token: 0x0400085E RID: 2142
			Capsule
		}

		// Token: 0x020001B0 RID: 432
		private struct MeshCacheItem : IEquatable<RecastMeshGatherer.MeshCacheItem>
		{
			// Token: 0x06000C86 RID: 3206 RVA: 0x00046808 File Offset: 0x00044A08
			public MeshCacheItem(Mesh mesh)
			{
				this.type = RecastMeshGatherer.MeshType.Mesh;
				this.mesh = mesh;
				this.rows = 0;
				this.quantizedHeight = 0;
			}

			// Token: 0x06000C87 RID: 3207 RVA: 0x00046828 File Offset: 0x00044A28
			public bool Equals(RecastMeshGatherer.MeshCacheItem other)
			{
				return this.type == other.type && this.mesh == other.mesh && this.rows == other.rows && this.quantizedHeight == other.quantizedHeight;
			}

			// Token: 0x06000C88 RID: 3208 RVA: 0x00046874 File Offset: 0x00044A74
			public override int GetHashCode()
			{
				return (int)((((((this.type * (RecastMeshGatherer.MeshType)31) ^ (RecastMeshGatherer.MeshType)((this.mesh != null) ? this.mesh.GetHashCode() : (-1))) * (RecastMeshGatherer.MeshType)31) ^ (RecastMeshGatherer.MeshType)this.rows) * (RecastMeshGatherer.MeshType)31) ^ (RecastMeshGatherer.MeshType)this.quantizedHeight);
			}

			// Token: 0x0400085F RID: 2143
			public RecastMeshGatherer.MeshType type;

			// Token: 0x04000860 RID: 2144
			public Mesh mesh;

			// Token: 0x04000861 RID: 2145
			public int rows;

			// Token: 0x04000862 RID: 2146
			public int quantizedHeight;

			// Token: 0x04000863 RID: 2147
			public static readonly RecastMeshGatherer.MeshCacheItem Box = new RecastMeshGatherer.MeshCacheItem
			{
				type = RecastMeshGatherer.MeshType.Box,
				mesh = null,
				rows = 0,
				quantizedHeight = 0
			};
		}

		// Token: 0x020001B1 RID: 433
		[BurstCompile]
		private struct JobGenerateHeightmapChunk : IJobFor
		{
			// Token: 0x06000C8A RID: 3210 RVA: 0x000468EC File Offset: 0x00044AEC
			public unsafe void Execute(int index)
			{
				int x = this.chunkSize.x;
				int y = this.chunkSize.y;
				int num = index % this.chunks.x * x;
				int num2 = index / this.chunks.x * y;
				int width = this.sampleRect.Width;
				int height = this.sampleRect.Height;
				int num3 = RecastMeshGatherer.CeilDivision(Mathf.Min(x, width - num), this.stride) + 1;
				int num4 = RecastMeshGatherer.CeilDivision(Mathf.Min(y, height - num2), this.stride) + 1;
				int num5 = num3 * num4;
				int num6 = (num3 - 1) * (num4 - 1);
				UnsafeSpan<Vector3> unsafeSpan = new UnsafeSpan<Vector3>(Allocator.Persistent, num5);
				UnsafeSpan<int> unsafeSpan2 = new UnsafeSpan<int>(Allocator.Persistent, num6 * 2 * 3);
				for (int i = 0; i < num4; i++)
				{
					int num7 = Math.Min(num2 + i * this.stride, height - 1);
					for (int j = 0; j < num3; j++)
					{
						int num8 = Math.Min(num + j * this.stride, width - 1);
						*unsafeSpan[i * num3 + j] = new Vector3((float)num8, *this.heights[num7 * width + num8], (float)num7);
					}
				}
				int num9 = 0;
				UnsafeSpan<int> unsafeSpan3 = default(UnsafeSpan<int>);
				if (this.alphaMaps.Length > 0)
				{
					num9 = (int)math.sqrt((float)(this.alphaMaps[0].Length / 4));
					for (int k = 0; k < this.alphaMaps.Length; k++)
					{
					}
					unsafeSpan3 = new UnsafeSpan<int>(Allocator.Persistent, num6 * 2);
				}
				int num10 = 0;
				int num11 = 0;
				for (int l = 0; l < num4 - 1; l++)
				{
					for (int m = 0; m < num3 - 1; m++)
					{
						int num12 = Math.Min(num + this.stride / 2 + m * this.stride, width - 2);
						int num13 = Math.Min(num2 + this.stride / 2 + l * this.stride, height - 2);
						if (*this.holes[num13 * (width - 1) + num12])
						{
							if (this.alphaMaps.Length > 0)
							{
								int2 @int = math.clamp((int2)math.round((new float2((float)(num + this.sampleRect.xmin + m * this.stride), (float)(num2 + this.sampleRect.ymin + l * this.stride)) + 0.5f * (float)this.stride) * this.alphamapScale), 0, num9 - 1);
								uint num14 = (uint)((@int.x + @int.y * num9) * 4);
								int num15 = 0;
								float num16 = -1f;
								uint num17 = 0U;
								while ((ulong)num17 < (ulong)((long)this.areaMappingThresholds.Length))
								{
									float num18 = (float)(*this.alphaMaps[num17 / 4U][num14 + num17 % 4U]) * 0.003921569f - *this.areaMappingThresholds[num17];
									if (num18 > num16)
									{
										num16 = num18;
										num15 = *this.areaMapping[num17];
									}
									num17 += 1U;
								}
								*unsafeSpan3[num11] = num15;
								*unsafeSpan3[num11 + 1] = num15;
								num11 += 2;
							}
							*unsafeSpan2[num10] = l * num3 + m;
							*unsafeSpan2[num10 + 1] = (l + 1) * num3 + m + 1;
							*unsafeSpan2[num10 + 2] = l * num3 + m + 1;
							num10 += 3;
							*unsafeSpan2[num10] = l * num3 + m;
							*unsafeSpan2[num10 + 1] = (l + 1) * num3 + m;
							*unsafeSpan2[num10 + 2] = (l + 1) * num3 + m + 1;
							num10 += 3;
						}
					}
				}
				unsafeSpan2 = unsafeSpan2.Slice(0, num10);
				if (unsafeSpan3.Length > 0)
				{
					unsafeSpan3 = unsafeSpan3.Slice(0, num11);
				}
				this.output[index] = new RecastMeshGatherer.JobGenerateHeightmapChunk.TerrainChunk
				{
					verts = unsafeSpan,
					tris = unsafeSpan2,
					tags = unsafeSpan3
				};
			}

			// Token: 0x04000864 RID: 2148
			public UnsafeSpan<float> heights;

			// Token: 0x04000865 RID: 2149
			public UnsafeSpan<bool> holes;

			// Token: 0x04000866 RID: 2150
			public IntRect sampleRect;

			// Token: 0x04000867 RID: 2151
			public Vector2Int chunkSize;

			// Token: 0x04000868 RID: 2152
			public Vector2Int chunks;

			// Token: 0x04000869 RID: 2153
			public int stride;

			// Token: 0x0400086A RID: 2154
			public float alphamapScale;

			// Token: 0x0400086B RID: 2155
			public UnsafeSpan<UnsafeSpan<byte>> alphaMaps;

			// Token: 0x0400086C RID: 2156
			public UnsafeSpan<int> areaMapping;

			// Token: 0x0400086D RID: 2157
			public UnsafeSpan<float> areaMappingThresholds;

			// Token: 0x0400086E RID: 2158
			public NativeArray<RecastMeshGatherer.JobGenerateHeightmapChunk.TerrainChunk> output;

			// Token: 0x020001B2 RID: 434
			public struct TerrainChunk
			{
				// Token: 0x0400086F RID: 2159
				public UnsafeSpan<Vector3> verts;

				// Token: 0x04000870 RID: 2160
				public UnsafeSpan<int> tris;

				// Token: 0x04000871 RID: 2161
				public UnsafeSpan<int> tags;
			}
		}

		// Token: 0x020001B3 RID: 435
		// (Invoke) Token: 0x06000C8C RID: 3212
		internal delegate void CalculateBounds_00000B92$PostfixBurstDelegate(ref UnsafeSpan<float3> vertices, ref float4x4 localToWorldMatrix, out Bounds bounds);

		// Token: 0x020001B4 RID: 436
		internal static class CalculateBounds_00000B92$BurstDirectCall
		{
			// Token: 0x06000C8F RID: 3215 RVA: 0x00046D20 File Offset: 0x00044F20
			[BurstDiscard]
			private static void GetFunctionPointerDiscard(ref IntPtr A_0)
			{
				if (RecastMeshGatherer.CalculateBounds_00000B92$BurstDirectCall.Pointer == 0)
				{
					RecastMeshGatherer.CalculateBounds_00000B92$BurstDirectCall.Pointer = BurstCompiler.GetILPPMethodFunctionPointer2(RecastMeshGatherer.CalculateBounds_00000B92$BurstDirectCall.DeferredCompilation, methodof(RecastMeshGatherer.CalculateBounds$BurstManaged(ref UnsafeSpan<float3>, ref float4x4, ref Bounds)).MethodHandle, typeof(RecastMeshGatherer.CalculateBounds_00000B92$PostfixBurstDelegate).TypeHandle);
				}
				A_0 = RecastMeshGatherer.CalculateBounds_00000B92$BurstDirectCall.Pointer;
			}

			// Token: 0x06000C90 RID: 3216 RVA: 0x00046D4C File Offset: 0x00044F4C
			private static IntPtr GetFunctionPointer()
			{
				IntPtr intPtr = (IntPtr)0;
				RecastMeshGatherer.CalculateBounds_00000B92$BurstDirectCall.GetFunctionPointerDiscard(ref intPtr);
				return intPtr;
			}

			// Token: 0x06000C91 RID: 3217 RVA: 0x00046D64 File Offset: 0x00044F64
			public static void Constructor()
			{
				RecastMeshGatherer.CalculateBounds_00000B92$BurstDirectCall.DeferredCompilation = BurstCompiler.CompileILPPMethod2(methodof(RecastMeshGatherer.CalculateBounds(ref UnsafeSpan<float3>, ref float4x4, ref Bounds)).MethodHandle);
			}

			// Token: 0x06000C92 RID: 3218 RVA: 0x00003788 File Offset: 0x00001988
			public static void Initialize()
			{
			}

			// Token: 0x06000C93 RID: 3219 RVA: 0x00046D75 File Offset: 0x00044F75
			// Note: this type is marked as 'beforefieldinit'.
			static CalculateBounds_00000B92$BurstDirectCall()
			{
				RecastMeshGatherer.CalculateBounds_00000B92$BurstDirectCall.Constructor();
			}

			// Token: 0x06000C94 RID: 3220 RVA: 0x00046D7C File Offset: 0x00044F7C
			public static void Invoke(ref UnsafeSpan<float3> vertices, ref float4x4 localToWorldMatrix, out Bounds bounds)
			{
				if (BurstCompiler.IsEnabled)
				{
					IntPtr functionPointer = RecastMeshGatherer.CalculateBounds_00000B92$BurstDirectCall.GetFunctionPointer();
					if (functionPointer != 0)
					{
						calli(System.Void(Pathfinding.Collections.UnsafeSpan`1<Unity.Mathematics.float3>&,Unity.Mathematics.float4x4&,UnityEngine.Bounds&), ref vertices, ref localToWorldMatrix, ref bounds, functionPointer);
						return;
					}
				}
				RecastMeshGatherer.CalculateBounds$BurstManaged(ref vertices, ref localToWorldMatrix, out bounds);
			}

			// Token: 0x04000872 RID: 2162
			private static IntPtr Pointer;

			// Token: 0x04000873 RID: 2163
			private static IntPtr DeferredCompilation;
		}

		// Token: 0x020001B5 RID: 437
		// (Invoke) Token: 0x06000C96 RID: 3222
		internal delegate void PatchMissingMeshDataAndBounds_00000B94$PostfixBurstDelegate(ref UnsafeList<RecastMeshGatherer.GatheredMesh> gatheredMeshes, ref UnsafeList<UnsafeSpan<Vector3>> vertexBuffers, int meshBufferOffset);

		// Token: 0x020001B6 RID: 438
		internal static class PatchMissingMeshDataAndBounds_00000B94$BurstDirectCall
		{
			// Token: 0x06000C99 RID: 3225 RVA: 0x00046DB1 File Offset: 0x00044FB1
			[BurstDiscard]
			private static void GetFunctionPointerDiscard(ref IntPtr A_0)
			{
				if (RecastMeshGatherer.PatchMissingMeshDataAndBounds_00000B94$BurstDirectCall.Pointer == 0)
				{
					RecastMeshGatherer.PatchMissingMeshDataAndBounds_00000B94$BurstDirectCall.Pointer = BurstCompiler.GetILPPMethodFunctionPointer2(RecastMeshGatherer.PatchMissingMeshDataAndBounds_00000B94$BurstDirectCall.DeferredCompilation, methodof(RecastMeshGatherer.PatchMissingMeshDataAndBounds$BurstManaged(ref UnsafeList<RecastMeshGatherer.GatheredMesh>, ref UnsafeList<UnsafeSpan<Vector3>>, int)).MethodHandle, typeof(RecastMeshGatherer.PatchMissingMeshDataAndBounds_00000B94$PostfixBurstDelegate).TypeHandle);
				}
				A_0 = RecastMeshGatherer.PatchMissingMeshDataAndBounds_00000B94$BurstDirectCall.Pointer;
			}

			// Token: 0x06000C9A RID: 3226 RVA: 0x00046DE0 File Offset: 0x00044FE0
			private static IntPtr GetFunctionPointer()
			{
				IntPtr intPtr = (IntPtr)0;
				RecastMeshGatherer.PatchMissingMeshDataAndBounds_00000B94$BurstDirectCall.GetFunctionPointerDiscard(ref intPtr);
				return intPtr;
			}

			// Token: 0x06000C9B RID: 3227 RVA: 0x00046DF8 File Offset: 0x00044FF8
			public static void Constructor()
			{
				RecastMeshGatherer.PatchMissingMeshDataAndBounds_00000B94$BurstDirectCall.DeferredCompilation = BurstCompiler.CompileILPPMethod2(methodof(RecastMeshGatherer.PatchMissingMeshDataAndBounds(ref UnsafeList<RecastMeshGatherer.GatheredMesh>, ref UnsafeList<UnsafeSpan<Vector3>>, int)).MethodHandle);
			}

			// Token: 0x06000C9C RID: 3228 RVA: 0x00003788 File Offset: 0x00001988
			public static void Initialize()
			{
			}

			// Token: 0x06000C9D RID: 3229 RVA: 0x00046E09 File Offset: 0x00045009
			// Note: this type is marked as 'beforefieldinit'.
			static PatchMissingMeshDataAndBounds_00000B94$BurstDirectCall()
			{
				RecastMeshGatherer.PatchMissingMeshDataAndBounds_00000B94$BurstDirectCall.Constructor();
			}

			// Token: 0x06000C9E RID: 3230 RVA: 0x00046E10 File Offset: 0x00045010
			public static void Invoke(ref UnsafeList<RecastMeshGatherer.GatheredMesh> gatheredMeshes, ref UnsafeList<UnsafeSpan<Vector3>> vertexBuffers, int meshBufferOffset)
			{
				if (BurstCompiler.IsEnabled)
				{
					IntPtr functionPointer = RecastMeshGatherer.PatchMissingMeshDataAndBounds_00000B94$BurstDirectCall.GetFunctionPointer();
					if (functionPointer != 0)
					{
						calli(System.Void(Unity.Collections.LowLevel.Unsafe.UnsafeList`1<Pathfinding.Graphs.Navmesh.RecastMeshGatherer/GatheredMesh>&,Unity.Collections.LowLevel.Unsafe.UnsafeList`1<Pathfinding.Collections.UnsafeSpan`1<UnityEngine.Vector3>>&,System.Int32), ref gatheredMeshes, ref vertexBuffers, meshBufferOffset, functionPointer);
						return;
					}
				}
				RecastMeshGatherer.PatchMissingMeshDataAndBounds$BurstManaged(ref gatheredMeshes, ref vertexBuffers, meshBufferOffset);
			}

			// Token: 0x04000874 RID: 2164
			private static IntPtr Pointer;

			// Token: 0x04000875 RID: 2165
			private static IntPtr DeferredCompilation;
		}

		// Token: 0x020001B7 RID: 439
		// (Invoke) Token: 0x06000CA0 RID: 3232
		internal delegate void CreateRasterizationMeshes_00000B96$PostfixBurstDelegate(ref UnsafeList<RecastMeshGatherer.GatheredMesh> meshes, ref UnsafeList<UnsafeSpan<Vector3>> vertexBuffers, ref UnsafeList<UnsafeSpan<int>> triangleBuffers, ref UnsafeList<UnsafeSpan<int>> tagsBuffers, ref UnsafeSpan<RasterizationMesh> rasterizationMeshesOutput);

		// Token: 0x020001B8 RID: 440
		internal static class CreateRasterizationMeshes_00000B96$BurstDirectCall
		{
			// Token: 0x06000CA3 RID: 3235 RVA: 0x00046E45 File Offset: 0x00045045
			[BurstDiscard]
			private static void GetFunctionPointerDiscard(ref IntPtr A_0)
			{
				if (RecastMeshGatherer.CreateRasterizationMeshes_00000B96$BurstDirectCall.Pointer == 0)
				{
					RecastMeshGatherer.CreateRasterizationMeshes_00000B96$BurstDirectCall.Pointer = BurstCompiler.GetILPPMethodFunctionPointer2(RecastMeshGatherer.CreateRasterizationMeshes_00000B96$BurstDirectCall.DeferredCompilation, methodof(RecastMeshGatherer.CreateRasterizationMeshes$BurstManaged(ref UnsafeList<RecastMeshGatherer.GatheredMesh>, ref UnsafeList<UnsafeSpan<Vector3>>, ref UnsafeList<UnsafeSpan<int>>, ref UnsafeList<UnsafeSpan<int>>, ref UnsafeSpan<RasterizationMesh>)).MethodHandle, typeof(RecastMeshGatherer.CreateRasterizationMeshes_00000B96$PostfixBurstDelegate).TypeHandle);
				}
				A_0 = RecastMeshGatherer.CreateRasterizationMeshes_00000B96$BurstDirectCall.Pointer;
			}

			// Token: 0x06000CA4 RID: 3236 RVA: 0x00046E74 File Offset: 0x00045074
			private static IntPtr GetFunctionPointer()
			{
				IntPtr intPtr = (IntPtr)0;
				RecastMeshGatherer.CreateRasterizationMeshes_00000B96$BurstDirectCall.GetFunctionPointerDiscard(ref intPtr);
				return intPtr;
			}

			// Token: 0x06000CA5 RID: 3237 RVA: 0x00046E8C File Offset: 0x0004508C
			public static void Constructor()
			{
				RecastMeshGatherer.CreateRasterizationMeshes_00000B96$BurstDirectCall.DeferredCompilation = BurstCompiler.CompileILPPMethod2(methodof(RecastMeshGatherer.CreateRasterizationMeshes(ref UnsafeList<RecastMeshGatherer.GatheredMesh>, ref UnsafeList<UnsafeSpan<Vector3>>, ref UnsafeList<UnsafeSpan<int>>, ref UnsafeList<UnsafeSpan<int>>, ref UnsafeSpan<RasterizationMesh>)).MethodHandle);
			}

			// Token: 0x06000CA6 RID: 3238 RVA: 0x00003788 File Offset: 0x00001988
			public static void Initialize()
			{
			}

			// Token: 0x06000CA7 RID: 3239 RVA: 0x00046E9D File Offset: 0x0004509D
			// Note: this type is marked as 'beforefieldinit'.
			static CreateRasterizationMeshes_00000B96$BurstDirectCall()
			{
				RecastMeshGatherer.CreateRasterizationMeshes_00000B96$BurstDirectCall.Constructor();
			}

			// Token: 0x06000CA8 RID: 3240 RVA: 0x00046EA4 File Offset: 0x000450A4
			public static void Invoke(ref UnsafeList<RecastMeshGatherer.GatheredMesh> meshes, ref UnsafeList<UnsafeSpan<Vector3>> vertexBuffers, ref UnsafeList<UnsafeSpan<int>> triangleBuffers, ref UnsafeList<UnsafeSpan<int>> tagsBuffers, ref UnsafeSpan<RasterizationMesh> rasterizationMeshesOutput)
			{
				if (BurstCompiler.IsEnabled)
				{
					IntPtr functionPointer = RecastMeshGatherer.CreateRasterizationMeshes_00000B96$BurstDirectCall.GetFunctionPointer();
					if (functionPointer != 0)
					{
						calli(System.Void(Unity.Collections.LowLevel.Unsafe.UnsafeList`1<Pathfinding.Graphs.Navmesh.RecastMeshGatherer/GatheredMesh>&,Unity.Collections.LowLevel.Unsafe.UnsafeList`1<Pathfinding.Collections.UnsafeSpan`1<UnityEngine.Vector3>>&,Unity.Collections.LowLevel.Unsafe.UnsafeList`1<Pathfinding.Collections.UnsafeSpan`1<System.Int32>>&,Unity.Collections.LowLevel.Unsafe.UnsafeList`1<Pathfinding.Collections.UnsafeSpan`1<System.Int32>>&,Pathfinding.Collections.UnsafeSpan`1<Pathfinding.Graphs.Navmesh.Voxelization.Burst.RasterizationMesh>&), ref meshes, ref vertexBuffers, ref triangleBuffers, ref tagsBuffers, ref rasterizationMeshesOutput, functionPointer);
						return;
					}
				}
				RecastMeshGatherer.CreateRasterizationMeshes$BurstManaged(ref meshes, ref vertexBuffers, ref triangleBuffers, ref tagsBuffers, ref rasterizationMeshesOutput);
			}

			// Token: 0x04000876 RID: 2166
			private static IntPtr Pointer;

			// Token: 0x04000877 RID: 2167
			private static IntPtr DeferredCompilation;
		}

		// Token: 0x020001B9 RID: 441
		// (Invoke) Token: 0x06000CAA RID: 3242
		internal delegate void ConvertTreesToMeshes_00000BA9$PostfixBurstDelegate(ref UnsafeSpan<TreeInstance> treeInstances, ref float3 terrainPos, ref float3 terrainSize, ref UnsafeSpan<RecastMeshGatherer.TreeInfo> treeInfos, ref UnsafeList<RecastMeshGatherer.GatheredMesh> allSubmeshes, ref Bounds graphBounds, ref UnsafeList<RecastMeshGatherer.GatheredMesh> meshes);

		// Token: 0x020001BA RID: 442
		internal static class ConvertTreesToMeshes_00000BA9$BurstDirectCall
		{
			// Token: 0x06000CAD RID: 3245 RVA: 0x00046EDF File Offset: 0x000450DF
			[BurstDiscard]
			private static void GetFunctionPointerDiscard(ref IntPtr A_0)
			{
				if (RecastMeshGatherer.ConvertTreesToMeshes_00000BA9$BurstDirectCall.Pointer == 0)
				{
					RecastMeshGatherer.ConvertTreesToMeshes_00000BA9$BurstDirectCall.Pointer = BurstCompiler.GetILPPMethodFunctionPointer2(RecastMeshGatherer.ConvertTreesToMeshes_00000BA9$BurstDirectCall.DeferredCompilation, methodof(RecastMeshGatherer.ConvertTreesToMeshes$BurstManaged(ref UnsafeSpan<TreeInstance>, ref float3, ref float3, ref UnsafeSpan<RecastMeshGatherer.TreeInfo>, ref UnsafeList<RecastMeshGatherer.GatheredMesh>, ref Bounds, ref UnsafeList<RecastMeshGatherer.GatheredMesh>)).MethodHandle, typeof(RecastMeshGatherer.ConvertTreesToMeshes_00000BA9$PostfixBurstDelegate).TypeHandle);
				}
				A_0 = RecastMeshGatherer.ConvertTreesToMeshes_00000BA9$BurstDirectCall.Pointer;
			}

			// Token: 0x06000CAE RID: 3246 RVA: 0x00046F0C File Offset: 0x0004510C
			private static IntPtr GetFunctionPointer()
			{
				IntPtr intPtr = (IntPtr)0;
				RecastMeshGatherer.ConvertTreesToMeshes_00000BA9$BurstDirectCall.GetFunctionPointerDiscard(ref intPtr);
				return intPtr;
			}

			// Token: 0x06000CAF RID: 3247 RVA: 0x00046F24 File Offset: 0x00045124
			public static void Constructor()
			{
				RecastMeshGatherer.ConvertTreesToMeshes_00000BA9$BurstDirectCall.DeferredCompilation = BurstCompiler.CompileILPPMethod2(methodof(RecastMeshGatherer.ConvertTreesToMeshes(ref UnsafeSpan<TreeInstance>, ref float3, ref float3, ref UnsafeSpan<RecastMeshGatherer.TreeInfo>, ref UnsafeList<RecastMeshGatherer.GatheredMesh>, ref Bounds, ref UnsafeList<RecastMeshGatherer.GatheredMesh>)).MethodHandle);
			}

			// Token: 0x06000CB0 RID: 3248 RVA: 0x00003788 File Offset: 0x00001988
			public static void Initialize()
			{
			}

			// Token: 0x06000CB1 RID: 3249 RVA: 0x00046F35 File Offset: 0x00045135
			// Note: this type is marked as 'beforefieldinit'.
			static ConvertTreesToMeshes_00000BA9$BurstDirectCall()
			{
				RecastMeshGatherer.ConvertTreesToMeshes_00000BA9$BurstDirectCall.Constructor();
			}

			// Token: 0x06000CB2 RID: 3250 RVA: 0x00046F3C File Offset: 0x0004513C
			public static void Invoke(ref UnsafeSpan<TreeInstance> treeInstances, ref float3 terrainPos, ref float3 terrainSize, ref UnsafeSpan<RecastMeshGatherer.TreeInfo> treeInfos, ref UnsafeList<RecastMeshGatherer.GatheredMesh> allSubmeshes, ref Bounds graphBounds, ref UnsafeList<RecastMeshGatherer.GatheredMesh> meshes)
			{
				if (BurstCompiler.IsEnabled)
				{
					IntPtr functionPointer = RecastMeshGatherer.ConvertTreesToMeshes_00000BA9$BurstDirectCall.GetFunctionPointer();
					if (functionPointer != 0)
					{
						calli(System.Void(Pathfinding.Collections.UnsafeSpan`1<UnityEngine.TreeInstance>&,Unity.Mathematics.float3&,Unity.Mathematics.float3&,Pathfinding.Collections.UnsafeSpan`1<Pathfinding.Graphs.Navmesh.RecastMeshGatherer/TreeInfo>&,Unity.Collections.LowLevel.Unsafe.UnsafeList`1<Pathfinding.Graphs.Navmesh.RecastMeshGatherer/GatheredMesh>&,UnityEngine.Bounds&,Unity.Collections.LowLevel.Unsafe.UnsafeList`1<Pathfinding.Graphs.Navmesh.RecastMeshGatherer/GatheredMesh>&), ref treeInstances, ref terrainPos, ref terrainSize, ref treeInfos, ref allSubmeshes, ref graphBounds, ref meshes, functionPointer);
						return;
					}
				}
				RecastMeshGatherer.ConvertTreesToMeshes$BurstManaged(ref treeInstances, ref terrainPos, ref terrainSize, ref treeInfos, ref allSubmeshes, ref graphBounds, ref meshes);
			}

			// Token: 0x04000878 RID: 2168
			private static IntPtr Pointer;

			// Token: 0x04000879 RID: 2169
			private static IntPtr DeferredCompilation;
		}
	}
}
