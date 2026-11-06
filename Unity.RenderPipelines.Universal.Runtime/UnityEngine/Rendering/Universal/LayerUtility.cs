using System;
using Unity.Mathematics;

namespace UnityEngine.Rendering.Universal
{
	// Token: 0x02000031 RID: 49
	internal static class LayerUtility
	{
		// Token: 0x17000053 RID: 83
		// (get) Token: 0x060001AC RID: 428 RVA: 0x0000EB53 File Offset: 0x0000CD53
		// (set) Token: 0x060001AD RID: 429 RVA: 0x0000EB5A File Offset: 0x0000CD5A
		public static uint maxTextureCount { get; private set; }

		// Token: 0x060001AE RID: 430 RVA: 0x0000EB62 File Offset: 0x0000CD62
		public static void InitializeBudget(uint maxTextureCount)
		{
			LayerUtility.maxTextureCount = math.max(4U, maxTextureCount);
		}

		// Token: 0x060001AF RID: 431 RVA: 0x0000EB70 File Offset: 0x0000CD70
		private static bool CanBatchLightsInLayer(int layerIndex1, int layerIndex2, SortingLayer[] sortingLayers, ILight2DCullResult lightCullResult)
		{
			int id = sortingLayers[layerIndex1].id;
			int id2 = sortingLayers[layerIndex2].id;
			foreach (Light2D light2D in lightCullResult.visibleLights)
			{
				if (light2D.IsLitLayer(id) != light2D.IsLitLayer(id2))
				{
					return false;
				}
			}
			foreach (ShadowCasterGroup2D shadowCasterGroup2D in lightCullResult.visibleShadows)
			{
				foreach (ShadowCaster2D shadowCaster2D in shadowCasterGroup2D.GetShadowCasters())
				{
					if (shadowCaster2D.IsShadowedLayer(id) != shadowCaster2D.IsShadowedLayer(id2))
					{
						return false;
					}
				}
			}
			return true;
		}

		// Token: 0x060001B0 RID: 432 RVA: 0x0000EC80 File Offset: 0x0000CE80
		private static bool CanBatchCameraSortingLayer(int startLayerIndex, SortingLayer[] sortingLayers, Renderer2DData rendererData)
		{
			if (rendererData.useCameraSortingLayerTexture)
			{
				short cameraSortingLayerBoundsIndex = Render2DLightingPass.GetCameraSortingLayerBoundsIndex(rendererData);
				return sortingLayers[startLayerIndex].value == (int)cameraSortingLayerBoundsIndex;
			}
			return false;
		}

		// Token: 0x060001B1 RID: 433 RVA: 0x0000ECB0 File Offset: 0x0000CEB0
		private static int FindUpperBoundInBatch(int startLayerIndex, SortingLayer[] sortingLayers, Renderer2DData rendererData)
		{
			if (LayerUtility.CanBatchCameraSortingLayer(startLayerIndex, sortingLayers, rendererData))
			{
				return startLayerIndex;
			}
			for (int i = startLayerIndex + 1; i < sortingLayers.Length; i++)
			{
				if (!LayerUtility.CanBatchLightsInLayer(startLayerIndex, i, sortingLayers, rendererData.lightCullResult))
				{
					return i - 1;
				}
				if (LayerUtility.CanBatchCameraSortingLayer(i, sortingLayers, rendererData))
				{
					return i;
				}
			}
			return sortingLayers.Length - 1;
		}

		// Token: 0x060001B2 RID: 434 RVA: 0x0000ED00 File Offset: 0x0000CF00
		private static void InitializeBatchInfos(SortingLayer[] cachedSortingLayers)
		{
			int num = cachedSortingLayers.Length;
			bool flag = LayerUtility.s_LayerBatches == null;
			if (LayerUtility.s_LayerBatches == null)
			{
				LayerUtility.s_LayerBatches = new LayerBatch[num];
			}
			if (flag)
			{
				for (int i = 0; i < LayerUtility.s_LayerBatches.Length; i++)
				{
					LayerUtility.s_LayerBatches[i].InitRTIds(i);
				}
			}
		}

		// Token: 0x060001B3 RID: 435 RVA: 0x0000ED50 File Offset: 0x0000CF50
		public static LayerBatch[] CalculateBatches(Renderer2DData rendererData, out int batchCount)
		{
			SortingLayer[] cachedSortingLayer = Light2DManager.GetCachedSortingLayer();
			LayerUtility.InitializeBatchInfos(cachedSortingLayer);
			bool flag = false;
			batchCount = 0;
			int num2;
			for (int i = 0; i < cachedSortingLayer.Length; i = num2 + 1)
			{
				int id = cachedSortingLayer[i].id;
				LightStats lightStatsByLayer = rendererData.lightCullResult.GetLightStatsByLayer(id);
				LayerBatch[] array = LayerUtility.s_LayerBatches;
				int num = batchCount;
				batchCount = num + 1;
				ref LayerBatch ptr = ref array[num];
				num2 = LayerUtility.FindUpperBoundInBatch(i, cachedSortingLayer, rendererData);
				short num3 = (short)cachedSortingLayer[i].value;
				short num4 = ((i == 0) ? short.MinValue : num3);
				short num5 = (short)cachedSortingLayer[num2].value;
				short num6 = ((num2 == cachedSortingLayer.Length - 1) ? short.MaxValue : num5);
				SortingLayerRange sortingLayerRange = new SortingLayerRange(num4, num6);
				ptr.startLayerID = id;
				ptr.endLayerValue = (int)num5;
				ptr.layerRange = sortingLayerRange;
				ptr.lightStats = lightStatsByLayer;
				flag |= ptr.lightStats.useNormalMap;
			}
			for (int j = 0; j < batchCount; j++)
			{
				LayerBatch[] array2 = LayerUtility.s_LayerBatches;
				int num7 = j;
				bool flag2 = SpriteMaskUtility.HasSpriteMaskInLayerRange(array2[num7].layerRange);
				array2[num7].useNormals = array2[num7].lightStats.useNormalMap || (flag && flag2);
			}
			return LayerUtility.s_LayerBatches;
		}

		// Token: 0x060001B4 RID: 436 RVA: 0x0000EE86 File Offset: 0x0000D086
		public static void GetFilterSettings(Renderer2DData rendererData, ref LayerBatch layerBatch, out FilteringSettings filterSettings)
		{
			filterSettings = FilteringSettings.defaultValue;
			filterSettings.renderQueueRange = RenderQueueRange.all;
			filterSettings.layerMask = rendererData.layerMask;
			filterSettings.renderingLayerMask = uint.MaxValue;
			filterSettings.sortingLayerRange = layerBatch.layerRange;
		}

		// Token: 0x04000108 RID: 264
		private static LayerBatch[] s_LayerBatches;
	}
}
