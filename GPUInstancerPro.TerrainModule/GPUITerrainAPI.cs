using System;
using System.Collections.Generic;
using UnityEngine;

namespace GPUInstancerPro.TerrainModule
{
	// Token: 0x0200001B RID: 27
	public static class GPUITerrainAPI
	{
		// Token: 0x06000146 RID: 326 RVA: 0x0000964C File Offset: 0x0000784C
		public static void AddTerrains<T>(GPUITerrainManager<T> terrainManager, IEnumerable<Terrain> terrains) where T : GPUIPrototypeData, new()
		{
			terrainManager.AddTerrains(terrains);
		}

		// Token: 0x06000147 RID: 327 RVA: 0x00009655 File Offset: 0x00007855
		public static void AddTerrains<T>(GPUITerrainManager<T> terrainManager, IEnumerable<GPUITerrain> gpuiTerrains) where T : GPUIPrototypeData, new()
		{
			terrainManager.AddTerrains(gpuiTerrains);
		}

		// Token: 0x06000148 RID: 328 RVA: 0x0000965E File Offset: 0x0000785E
		public static bool AddTerrain<T>(GPUITerrainManager<T> terrainManager, Terrain terrain) where T : GPUIPrototypeData, new()
		{
			return terrainManager.AddTerrain(terrain);
		}

		// Token: 0x06000149 RID: 329 RVA: 0x00009667 File Offset: 0x00007867
		public static bool AddTerrain<T>(GPUITerrainManager<T> terrainManager, GPUITerrain gpuiTerrain) where T : GPUIPrototypeData, new()
		{
			return terrainManager.AddTerrain(gpuiTerrain);
		}

		// Token: 0x0600014A RID: 330 RVA: 0x00009670 File Offset: 0x00007870
		public static bool RemoveTerrain<T>(GPUITerrainManager<T> terrainManager, Terrain terrain) where T : GPUIPrototypeData, new()
		{
			return terrainManager.RemoveTerrain(terrain);
		}

		// Token: 0x0600014B RID: 331 RVA: 0x00009679 File Offset: 0x00007879
		public static bool RemoveTerrain<T>(GPUITerrainManager<T> terrainManager, GPUITerrain gpuiTerrain) where T : GPUIPrototypeData, new()
		{
			return terrainManager.RemoveTerrain(gpuiTerrain);
		}

		// Token: 0x0600014C RID: 332 RVA: 0x00009682 File Offset: 0x00007882
		public static bool ContainsTerrains<T>(GPUITerrainManager<T> terrainManager, IEnumerable<Terrain> terrains) where T : GPUIPrototypeData, new()
		{
			return terrainManager.ContainsTerrains(terrains);
		}

		// Token: 0x0600014D RID: 333 RVA: 0x0000968B File Offset: 0x0000788B
		public static bool ContainsTerrain<T>(GPUITerrainManager<T> terrainManager, Terrain terrain) where T : GPUIPrototypeData, new()
		{
			return terrainManager.ContainsTerrain(terrain);
		}

		// Token: 0x0600014E RID: 334 RVA: 0x00009694 File Offset: 0x00007894
		public static void RequireUpdate(GPUIDetailManager detailManager, bool forceImmediateUpdate = false, bool reloadTerrainDetailTextures = false)
		{
			detailManager.RequireUpdate(forceImmediateUpdate, reloadTerrainDetailTextures);
		}

		// Token: 0x0600014F RID: 335 RVA: 0x0000969E File Offset: 0x0000789E
		public static void RequireUpdate(GPUITreeManager treeManager, bool reloadTreeInstances = true)
		{
			treeManager.RequireUpdate(reloadTreeInstances);
		}

		// Token: 0x06000150 RID: 336 RVA: 0x000096A8 File Offset: 0x000078A8
		public static void SetDetailDensityAdjustment(GPUIDetailManager detailManager, float newDensityValue)
		{
			if (detailManager == null)
			{
				return;
			}
			for (int i = 0; i < detailManager.GetPrototypeCount(); i++)
			{
				GPUIDetailPrototypeData prototypeData = detailManager.GetPrototypeData(i);
				if (prototypeData != null)
				{
					prototypeData.densityAdjustment = newDensityValue;
					prototypeData.SetParameterBufferData();
				}
			}
			detailManager.RequireUpdate();
		}

		// Token: 0x06000151 RID: 337 RVA: 0x000096EE File Offset: 0x000078EE
		public static void SetTreeInstances(GPUITerrain gpuiTerrain, TreeInstance[] treeInstances, bool applyToTerrainData = true)
		{
			if (gpuiTerrain == null)
			{
				return;
			}
			gpuiTerrain.SetTreeInstances(treeInstances, applyToTerrainData);
		}
	}
}
