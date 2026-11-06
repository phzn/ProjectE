using System;
using System.Collections.Generic;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;

namespace GPUInstancerPro.TerrainModule
{
	// Token: 0x0200000F RID: 15
	public class GPUIInfiniteTerrainGenerator : MonoBehaviour
	{
		// Token: 0x060000C8 RID: 200 RVA: 0x00005840 File Offset: 0x00003A40
		private void OnEnable()
		{
			global::UnityEngine.Random.InitState(this.seed);
			this.centerTransform.position = new Vector3(0f, this.terrainHeight, 0f);
			this._expectedTerrainPositions = new HashSet<int2>();
			this._activeTerrains = new Dictionary<int2, Terrain>();
			this._terrainsToDestroy = new List<int2>();
			this._terrainPool = new Queue<Terrain>();
			this._heightmapResolution = Mathf.FloorToInt((float)this.terrainSize / 2f + 1f);
			this._heightmapNative = new NativeArray<float>(this._heightmapResolution * this._heightmapResolution, Allocator.Persistent, NativeArrayOptions.ClearMemory);
			this._heightMap1D = new float[this._heightmapResolution * this._heightmapResolution];
			this._heightMap = new float[this._heightmapResolution, this._heightmapResolution];
			this._detailArrays = new List<int[,]>();
			int num = this.detailSettings.Length;
			for (int i = 0; i < num; i++)
			{
				int[,] array = new int[this.detailResolution, this.detailResolution];
				for (int j = 0; j < this.detailResolution; j++)
				{
					for (int k = 0; k < this.detailResolution; k++)
					{
						array[j, k] = Mathf.RoundToInt(global::UnityEngine.Random.Range(this.detailSettings[i].minMaxDensity.x, this.detailSettings[i].minMaxDensity.y));
					}
				}
				this._detailArrays.Add(array);
			}
			int num2 = this.treePrefabs.Length;
			this._terrainTreePrototypes = new TreePrototype[num2];
			for (int l = 0; l < num2; l++)
			{
				this._terrainTreePrototypes[l] = new TreePrototype
				{
					prefab = this.treePrefabs[l]
				};
			}
			this._treeInstances = new TreeInstance[this.treeCountPerTerrain];
			this.Update();
			this.SetCenterTransformStartPosition();
		}

		// Token: 0x060000C9 RID: 201 RVA: 0x00005A18 File Offset: 0x00003C18
		private void Update()
		{
			if (Vector3.Distance(this._lastUpdatePosition, this.centerTransform.position) > this.updateDistance)
			{
				this._lastUpdatePosition = this.centerTransform.position;
				this.GenerateExpectedTerrainPositions();
				this.GenerateTerrainsFromExpectedPositions();
			}
		}

		// Token: 0x060000CA RID: 202 RVA: 0x00005A55 File Offset: 0x00003C55
		private void OnDisable()
		{
			this.Dispose();
		}

		// Token: 0x060000CB RID: 203 RVA: 0x00005A60 File Offset: 0x00003C60
		private void SetCenterTransformStartPosition()
		{
			RaycastHit raycastHit;
			if (Physics.Raycast(this.centerTransform.position, Vector3.down, out raycastHit, this.terrainHeight * 2f, 64))
			{
				this.centerTransform.position = new Vector3(0f, raycastHit.point.y + this.startPosOffset, 0f);
			}
		}

		// Token: 0x060000CC RID: 204 RVA: 0x00005AC4 File Offset: 0x00003CC4
		private void Dispose()
		{
			if (this._activeTerrains != null)
			{
				foreach (Terrain terrain in this._activeTerrains.Values)
				{
					if (terrain)
					{
						if (Application.isPlaying)
						{
							Object.Destroy(terrain.gameObject);
						}
						else
						{
							Object.DestroyImmediate(terrain.gameObject);
						}
					}
				}
			}
			if (this._heightmapNative.IsCreated)
			{
				this._heightmapNative.Dispose();
			}
		}

		// Token: 0x060000CD RID: 205 RVA: 0x00005B5C File Offset: 0x00003D5C
		private void GenerateExpectedTerrainPositions()
		{
			this._expectedTerrainPositions.Clear();
			int num = Mathf.FloorToInt(this.centerTransform.position.x / (float)this.terrainSize);
			int num2 = Mathf.FloorToInt(this.centerTransform.position.z / (float)this.terrainSize);
			int num3 = Mathf.CeilToInt(this.terrainVisibilityDistance / (float)this.terrainSize) + 2;
			for (int i = num - num3; i <= num + num3; i++)
			{
				for (int j = num2 - num3; j <= num2 + num3; j++)
				{
					int2 @int = new int2(i, j);
					if (Vector3.Distance(this.GetTerrainIDCenter(@int, this.terrainSize), this.centerTransform.position) < this.terrainVisibilityDistance + (float)this.terrainSize)
					{
						this._expectedTerrainPositions.Add(@int);
					}
				}
			}
		}

		// Token: 0x060000CE RID: 206 RVA: 0x00005C30 File Offset: 0x00003E30
		private void GenerateTerrainsFromExpectedPositions()
		{
			this._terrainsToDestroy.Clear();
			foreach (int2 @int in this._activeTerrains.Keys)
			{
				if (!this._expectedTerrainPositions.Contains(@int))
				{
					this._terrainsToDestroy.Add(@int);
				}
			}
			foreach (int2 int2 in this._terrainsToDestroy)
			{
				Terrain terrain = this._activeTerrains[int2];
				this._terrainPool.Enqueue(terrain);
				this._activeTerrains.Remove(int2);
				terrain.gameObject.SetActive(false);
			}
			foreach (int2 int3 in this._expectedTerrainPositions)
			{
				if (!this._activeTerrains.ContainsKey(int3))
				{
					this._activeTerrains.Add(int3, this.InitializeTerrainObject(int3));
				}
			}
		}

		// Token: 0x060000CF RID: 207 RVA: 0x00005D78 File Offset: 0x00003F78
		private Terrain InitializeTerrainObject(int2 tid)
		{
			Vector3 terrainIDPosition = this.GetTerrainIDPosition(tid, this.terrainSize);
			Terrain terrain;
			if (this._terrainPool.Count == 0)
			{
				string text = "Terrain ";
				int2 @int = tid;
				GameObject gameObject = new GameObject(text + @int.ToString());
				gameObject.SetActive(false);
				gameObject.transform.SetParent(base.transform);
				terrain = gameObject.AddComponent<Terrain>();
				TerrainCollider terrainCollider = gameObject.AddComponent<TerrainCollider>();
				terrain.allowAutoConnect = true;
				terrain.groupingID = 1;
				terrain.drawInstanced = true;
				terrain.materialTemplate = this.terrainMaterial;
				terrain.gameObject.transform.position = terrainIDPosition;
				this.SetTerrainNeighbors(tid, terrain);
				terrain.detailObjectDensity = this.detailObjectDensity;
				TerrainData terrainData = this.CreateTerrainData(terrainIDPosition);
				terrainCollider.terrainData = terrainData;
				terrain.terrainData = terrainData;
				gameObject.layer = 6;
				this.SetDetailLayers(terrain);
			}
			else
			{
				terrain = this._terrainPool.Dequeue();
				terrain.gameObject.transform.position = terrainIDPosition;
				Object gameObject2 = terrain.gameObject;
				string text2 = "Terrain ";
				int2 @int = tid;
				gameObject2.name = text2 + @int.ToString();
				terrain.detailObjectDensity = this.detailObjectDensity;
				terrain.detailObjectDistance = ((this.detailManager != null && this.detailManager.IsInitialized) ? 0f : this._detailObjectDistance);
				this.SetTerrainNeighbors(tid, terrain);
			}
			this.SetHeightmapData(terrainIDPosition, terrain);
			this.SetTreeInstances(terrain);
			GPUITerrainBuiltin gpuiterrainBuiltin = terrain.AddOrGetComponent<GPUITerrainBuiltin>(false);
			gpuiterrainBuiltin.LoadTerrainData();
			terrain.gameObject.SetActive(true);
			if (!this.readDetailLayerFromTerrain)
			{
				for (int i = 0; i < this.detailSettings.Length; i++)
				{
					gpuiterrainBuiltin.SetDetailLayer(i, this._detailArrays[i]);
				}
			}
			if (this.detailManager != null)
			{
				GPUITerrainAPI.AddTerrain<GPUIDetailPrototypeData>(this.detailManager, gpuiterrainBuiltin);
			}
			if (this.treeManager != null)
			{
				GPUITerrainAPI.AddTerrain<GPUITreePrototypeData>(this.treeManager, gpuiterrainBuiltin);
			}
			return terrain;
		}

		// Token: 0x060000D0 RID: 208 RVA: 0x00005F70 File Offset: 0x00004170
		private TerrainData CreateTerrainData(Vector3 terrainPosition)
		{
			TerrainData terrainData = ((this.dummyTerrain != null) ? Object.Instantiate<TerrainData>(this.dummyTerrain) : new TerrainData());
			terrainData.heightmapResolution = this._heightmapResolution;
			terrainData.baseMapResolution = this._baseTextureResolution;
			terrainData.alphamapResolution = this.terrainSize;
			terrainData.terrainLayers = this.terrainLayers;
			terrainData.size = new Vector3((float)this.terrainSize, this.terrainHeight, (float)this.terrainSize);
			return terrainData;
		}

		// Token: 0x060000D1 RID: 209 RVA: 0x00005FEC File Offset: 0x000041EC
		private void SetHeightmapData(Vector3 terrainPosition, Terrain terrain)
		{
			TerrainData terrainData = terrain.terrainData;
			new GPUIInfiniteTerrainGenerator.CreateHeightmapArrayJob
			{
				heightMap = this._heightmapNative,
				heightMapResolution = this._heightmapResolution,
				terrainPosition = terrainPosition
			}.Schedule(this._heightmapResolution * this._heightmapResolution, this._heightmapResolution, default(JobHandle)).Complete();
			this._heightmapNative.CopyTo(this._heightMap1D);
			Buffer.BlockCopy(this._heightMap1D, 0, this._heightMap, 0, 4 * this._heightmapResolution * this._heightmapResolution);
			terrainData.SetHeights(0, 0, this._heightMap);
		}

		// Token: 0x060000D2 RID: 210 RVA: 0x00006094 File Offset: 0x00004294
		private void SetTerrainNeighbors(int2 tid, Terrain terrain)
		{
			Terrain terrain2;
			this._activeTerrains.TryGetValue(new int2(tid.x - 1, tid.y), out terrain2);
			Terrain terrain3;
			this._activeTerrains.TryGetValue(new int2(tid.x, tid.y + 1), out terrain3);
			Terrain terrain4;
			this._activeTerrains.TryGetValue(new int2(tid.x + 1, tid.y), out terrain4);
			Terrain terrain5;
			this._activeTerrains.TryGetValue(new int2(tid.x, tid.y - 1), out terrain5);
			terrain.SetNeighbors(terrain2, terrain3, terrain4, terrain5);
		}

		// Token: 0x060000D3 RID: 211 RVA: 0x00006130 File Offset: 0x00004330
		private void SetDetailLayers(Terrain terrain)
		{
			int num = this.detailSettings.Length;
			if (num == 0)
			{
				return;
			}
			TerrainData terrainData = terrain.terrainData;
			terrainData.SetDetailScatterMode(DetailScatterMode.InstanceCountMode);
			terrainData.SetDetailResolution(this.detailResolution, 16);
			terrain.detailObjectDistance = ((this.detailManager != null && this.detailManager.IsInitialized) ? 0f : this._detailObjectDistance);
			DetailPrototype[] array = new DetailPrototype[num];
			for (int i = 0; i < num; i++)
			{
				if (this.detailSettings[i].texture != null)
				{
					array[i] = new DetailPrototype
					{
						noiseSeed = global::UnityEngine.Random.Range(1, 10000),
						prototypeTexture = this.detailSettings[i].texture,
						usePrototypeMesh = false,
						renderMode = DetailRenderMode.Grass,
						healthyColor = this.detailSettings[i].healthyColor,
						dryColor = this.detailSettings[i].dryColor,
						useInstancing = false,
						alignToGround = 0.5f,
						minWidth = this.detailSettings[i].minMaxScale.x,
						maxWidth = this.detailSettings[i].minMaxScale.y,
						minHeight = this.detailSettings[i].minMaxScale.x,
						maxHeight = this.detailSettings[i].minMaxScale.y,
						useDensityScaling = true
					};
				}
				else
				{
					array[i] = new DetailPrototype
					{
						noiseSeed = global::UnityEngine.Random.Range(1, 10000),
						prototype = this.detailSettings[i].prefab,
						usePrototypeMesh = true,
						renderMode = DetailRenderMode.VertexLit,
						healthyColor = this.detailSettings[i].healthyColor,
						dryColor = this.detailSettings[i].dryColor,
						useInstancing = true,
						alignToGround = 0.5f,
						minWidth = this.detailSettings[i].minMaxScale.x,
						maxWidth = this.detailSettings[i].minMaxScale.y,
						minHeight = this.detailSettings[i].minMaxScale.x,
						maxHeight = this.detailSettings[i].minMaxScale.y,
						useDensityScaling = true
					};
				}
			}
			terrainData.detailPrototypes = array;
			for (int j = 0; j < num; j++)
			{
				terrainData.SetDetailLayer(0, 0, j, this._detailArrays[j]);
			}
		}

		// Token: 0x060000D4 RID: 212 RVA: 0x000063E8 File Offset: 0x000045E8
		private void SetTreeInstances(Terrain terrain)
		{
			int num = this.treePrefabs.Length;
			if (num == 0 || this.treeCountPerTerrain <= 0)
			{
				return;
			}
			TerrainData terrainData = terrain.terrainData;
			Vector3 position = terrain.GetPosition();
			terrainData.treePrototypes = this._terrainTreePrototypes;
			for (int i = 0; i < this.treeCountPerTerrain; i++)
			{
				Vector3 vector = new Vector3(global::UnityEngine.Random.value, 0f, global::UnityEngine.Random.value);
				vector.y = terrain.SampleHeight(vector * (float)this.terrainSize + position) / this.terrainHeight;
				float num2 = global::UnityEngine.Random.Range(this.treeSizeRange.x, this.treeSizeRange.y);
				this._treeInstances[i] = new TreeInstance
				{
					position = vector,
					rotation = global::UnityEngine.Random.value * 360f,
					heightScale = num2,
					widthScale = num2,
					color = Color.white,
					prototypeIndex = global::UnityEngine.Random.Range(0, num)
				};
			}
			terrainData.treeInstances = this._treeInstances;
		}

		// Token: 0x060000D5 RID: 213 RVA: 0x00006505 File Offset: 0x00004705
		private Vector3 GetTerrainIDPosition(int2 tid, int terrainSize)
		{
			return new Vector3((float)(tid.x * terrainSize), 0f, (float)(tid.y * terrainSize));
		}

		// Token: 0x060000D6 RID: 214 RVA: 0x00006523 File Offset: 0x00004723
		private Vector3 GetTerrainIDCenter(int2 tid, int terrainSize)
		{
			return new Vector3((float)(tid.x * terrainSize) + (float)terrainSize / 2f, 0f, (float)(tid.y * terrainSize) + (float)terrainSize / 2f);
		}

		// Token: 0x060000D7 RID: 215 RVA: 0x00006554 File Offset: 0x00004754
		public void SetDetailObjectDensity(float density)
		{
			this.detailObjectDensity = density;
			if (this._activeTerrains != null)
			{
				foreach (Terrain terrain in this._activeTerrains.Values)
				{
					terrain.detailObjectDensity = density;
				}
			}
			if (this.detailManager != null)
			{
				this.detailManager.RequireUpdate();
			}
		}

		// Token: 0x060000D8 RID: 216 RVA: 0x000065D4 File Offset: 0x000047D4
		public void SetDetailObjectDistance(float distance)
		{
			this._detailObjectDistance = distance;
			if (this.detailManager != null)
			{
				this.detailManager.SetDetailObjectDistance(distance);
				if (!this.detailManager.IsInitialized && this._activeTerrains != null)
				{
					foreach (Terrain terrain in this._activeTerrains.Values)
					{
						terrain.detailObjectDistance = distance;
					}
				}
			}
		}

		// Token: 0x0400007D RID: 125
		public int seed = 42;

		// Token: 0x0400007E RID: 126
		public Transform centerTransform;

		// Token: 0x0400007F RID: 127
		public float startPosOffset = 10f;

		// Token: 0x04000080 RID: 128
		public float updateDistance = 20f;

		// Token: 0x04000081 RID: 129
		public float terrainVisibilityDistance = 2048f;

		// Token: 0x04000082 RID: 130
		[Space]
		[Header("Terrain Settings")]
		public int terrainSize = 1024;

		// Token: 0x04000083 RID: 131
		public float terrainHeight = 128f;

		// Token: 0x04000084 RID: 132
		public Material terrainMaterial;

		// Token: 0x04000085 RID: 133
		public TerrainLayer[] terrainLayers;

		// Token: 0x04000086 RID: 134
		public TerrainData dummyTerrain;

		// Token: 0x04000087 RID: 135
		[Space]
		[Header("Detail Settings")]
		public GPUIDetailManager detailManager;

		// Token: 0x04000088 RID: 136
		public GPUIInfiniteTerrainGenerator.DetailSettings[] detailSettings;

		// Token: 0x04000089 RID: 137
		public int detailResolution = 512;

		// Token: 0x0400008A RID: 138
		[Range(0f, 1f)]
		public float detailObjectDensity = 1f;

		// Token: 0x0400008B RID: 139
		public bool readDetailLayerFromTerrain;

		// Token: 0x0400008C RID: 140
		[Space]
		[Header("Tree Settings")]
		public GPUITreeManager treeManager;

		// Token: 0x0400008D RID: 141
		public GameObject[] treePrefabs;

		// Token: 0x0400008E RID: 142
		public int treeCountPerTerrain = 100;

		// Token: 0x0400008F RID: 143
		public Vector2 treeSizeRange = Vector2.one;

		// Token: 0x04000090 RID: 144
		private int _baseTextureResolution = 16;

		// Token: 0x04000091 RID: 145
		private int _heightmapResolution;

		// Token: 0x04000092 RID: 146
		private HashSet<int2> _expectedTerrainPositions;

		// Token: 0x04000093 RID: 147
		private Dictionary<int2, Terrain> _activeTerrains;

		// Token: 0x04000094 RID: 148
		private List<int2> _terrainsToDestroy;

		// Token: 0x04000095 RID: 149
		private Vector3 _lastUpdatePosition;

		// Token: 0x04000096 RID: 150
		private Queue<Terrain> _terrainPool;

		// Token: 0x04000097 RID: 151
		private List<int[,]> _detailArrays;

		// Token: 0x04000098 RID: 152
		private NativeArray<float> _heightmapNative;

		// Token: 0x04000099 RID: 153
		private float[] _heightMap1D;

		// Token: 0x0400009A RID: 154
		private float[,] _heightMap;

		// Token: 0x0400009B RID: 155
		private TreePrototype[] _terrainTreePrototypes;

		// Token: 0x0400009C RID: 156
		private TreeInstance[] _treeInstances;

		// Token: 0x0400009D RID: 157
		private float _detailObjectDistance = 750f;

		// Token: 0x02000010 RID: 16
		[Serializable]
		public struct DetailSettings
		{
			// Token: 0x0400009E RID: 158
			public Texture2D texture;

			// Token: 0x0400009F RID: 159
			public GameObject prefab;

			// Token: 0x040000A0 RID: 160
			public Color healthyColor;

			// Token: 0x040000A1 RID: 161
			public Color dryColor;

			// Token: 0x040000A2 RID: 162
			public Vector2 minMaxScale;

			// Token: 0x040000A3 RID: 163
			public Vector2 minMaxDensity;
		}

		// Token: 0x02000011 RID: 17
		[BurstCompile]
		private struct CreateHeightmapArrayJob : IJobParallelFor
		{
			// Token: 0x060000DA RID: 218 RVA: 0x000066F0 File Offset: 0x000048F0
			public void Execute(int i)
			{
				int num = i % this.heightMapResolution;
				int num2 = i / this.heightMapResolution;
				float num3 = Mathf.PerlinNoise(((float)num * 2f + this.terrainPosition.x + 1000f) / 350f, ((float)num2 * 2f + this.terrainPosition.z + 1000f) / 350f) * 0.8f;
				num3 += Mathf.PerlinNoise(((float)num * 2f + this.terrainPosition.x - 1000f) / 100f, ((float)num2 * 2f + this.terrainPosition.z - 1000f) / 100f) * 0.2f;
				num3 = Mathf.Clamp01(num3);
				this.heightMap[num2 * this.heightMapResolution + num] = num3;
			}

			// Token: 0x040000A4 RID: 164
			public NativeArray<float> heightMap;

			// Token: 0x040000A5 RID: 165
			[ReadOnly]
			public int heightMapResolution;

			// Token: 0x040000A6 RID: 166
			[ReadOnly]
			public Vector3 terrainPosition;
		}
	}
}
