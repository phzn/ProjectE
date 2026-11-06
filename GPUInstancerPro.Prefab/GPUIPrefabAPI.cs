using System;
using System.Collections.Generic;
using UnityEngine;

namespace GPUInstancerPro.PrefabModule
{
	// Token: 0x0200001A RID: 26
	public static class GPUIPrefabAPI
	{
		// Token: 0x060000A1 RID: 161 RVA: 0x00005B1C File Offset: 0x00003D1C
		public static int AddPrototype(GPUIPrefabManager prefabManager, GameObject prefab)
		{
			return prefabManager.AddPrototype(prefab);
		}

		// Token: 0x060000A2 RID: 162 RVA: 0x00005B25 File Offset: 0x00003D25
		public static void AddPrefabInstance(GPUIPrefab gpuiPrefab)
		{
			GPUIPrefabManager.AddPrefabInstance(gpuiPrefab);
		}

		// Token: 0x060000A3 RID: 163 RVA: 0x00005B2D File Offset: 0x00003D2D
		public static void AddPrefabInstances(IEnumerable<GPUIPrefab> gpuiPrefabs)
		{
			GPUIPrefabManager.AddPrefabInstances(gpuiPrefabs);
		}

		// Token: 0x060000A4 RID: 164 RVA: 0x00005B35 File Offset: 0x00003D35
		public static bool AddPrefabInstances(GPUIPrefabManager prefabManager, IEnumerable<GPUIPrefab> instances, int prototypeIndex)
		{
			return prefabManager.AddPrefabInstances(instances, prototypeIndex);
		}

		// Token: 0x060000A5 RID: 165 RVA: 0x00005B3F File Offset: 0x00003D3F
		public static void AddPrefabInstances(GPUIPrefabManager prefabManager, IEnumerable<GameObject> gameObjects, int prototypeIndex)
		{
			prefabManager.AddPrefabInstances(gameObjects, prototypeIndex);
		}

		// Token: 0x060000A6 RID: 166 RVA: 0x00005B49 File Offset: 0x00003D49
		public static bool AddPrefabInstance(GPUIPrefabManager prefabManager, GameObject go, int prototypeIndex)
		{
			return prefabManager.AddPrefabInstance(go, prototypeIndex);
		}

		// Token: 0x060000A7 RID: 167 RVA: 0x00005B53 File Offset: 0x00003D53
		public static bool AddPrefabInstance(GPUIPrefabManager prefabManager, GPUIPrefab gpuiPrefab, int prototypeIndex = -1)
		{
			return prefabManager.AddPrefabInstance(gpuiPrefab, prototypeIndex);
		}

		// Token: 0x060000A8 RID: 168 RVA: 0x00005B5D File Offset: 0x00003D5D
		public static int AddPrefabInstanceImmediate(GPUIPrefabManager prefabManager, GPUIPrefab gpuiPrefab, int prototypeIndex = -1)
		{
			return prefabManager.AddPrefabInstanceImmediate(gpuiPrefab, prototypeIndex);
		}

		// Token: 0x060000A9 RID: 169 RVA: 0x00005B67 File Offset: 0x00003D67
		public static void RemovePrefabInstance(GPUIPrefab gpuiPrefab)
		{
			gpuiPrefab.RemovePrefabInstance();
		}

		// Token: 0x060000AA RID: 170 RVA: 0x00005B6F File Offset: 0x00003D6F
		public static void UpdateTransformData(GPUIPrefabManager prefabManager)
		{
			prefabManager.UpdateTransformData();
		}

		// Token: 0x060000AB RID: 171 RVA: 0x00005B77 File Offset: 0x00003D77
		public static void UpdateTransformData(GPUIPrefabManager prefabManager, int prototypeIndex)
		{
			prefabManager.UpdateTransformData(prototypeIndex);
		}

		// Token: 0x060000AC RID: 172 RVA: 0x00005B80 File Offset: 0x00003D80
		public static void UpdateTransformData(GPUIPrefabManager prefabManager, GPUIPrefab gpuiPrefab)
		{
			prefabManager.UpdateTransformData(gpuiPrefab);
		}

		// Token: 0x060000AD RID: 173 RVA: 0x00005B89 File Offset: 0x00003D89
		public static void RequireTransformUpdate(GPUIPrefabManager prefabManager)
		{
			prefabManager.RequireTransformUpdate();
		}
	}
}
