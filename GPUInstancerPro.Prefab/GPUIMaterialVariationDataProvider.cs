using System;
using UnityEngine;

namespace GPUInstancerPro.PrefabModule
{
	// Token: 0x0200000D RID: 13
	public class GPUIMaterialVariationDataProvider : GPUIDataProvider<int, GPUIMaterialVariationData>
	{
		// Token: 0x17000003 RID: 3
		// (get) Token: 0x0600002B RID: 43 RVA: 0x00002A36 File Offset: 0x00000C36
		public static GPUIMaterialVariationDataProvider Instance
		{
			get
			{
				if (GPUIMaterialVariationDataProvider._instance == null)
				{
					GPUIMaterialVariationDataProvider._instance = new GPUIMaterialVariationDataProvider();
					GPUIMaterialVariationDataProvider._instance.Initialize();
				}
				return GPUIMaterialVariationDataProvider._instance;
			}
		}

		// Token: 0x0600002C RID: 44 RVA: 0x00002A58 File Offset: 0x00000C58
		public override void Initialize()
		{
			if (!GPUIRuntimeSettings.IsSupportedPlatform())
			{
				return;
			}
			if (GPUIMaterialVariationDataProvider._instance != null && GPUIMaterialVariationDataProvider._instance != this)
			{
				Debug.LogWarning(GPUIConstants.LOG_PREFIX + "Duplicate GPUIMaterialVariationDataProvider initialization.");
				return;
			}
			base.Initialize();
			GPUIRenderingSystem.InitializeRenderingSystem();
			GPUIRenderingSystem.AddDependentDisposable(this);
		}

		// Token: 0x0600002D RID: 45 RVA: 0x00002A98 File Offset: 0x00000C98
		public void UpdateVariationBuffers()
		{
			if (!base.IsInitialized)
			{
				return;
			}
			foreach (GPUIMaterialVariationData gpuimaterialVariationData in base.Values)
			{
				gpuimaterialVariationData.UpdateVariationBuffer();
			}
		}

		// Token: 0x0600002E RID: 46 RVA: 0x00002AF4 File Offset: 0x00000CF4
		internal void UpdateVariationBuffers(GPUICameraData cameraData)
		{
			this.UpdateVariationBuffers();
		}

		// Token: 0x0600002F RID: 47 RVA: 0x00002AFC File Offset: 0x00000CFC
		public static GPUIMaterialVariationData GetMaterialVariationData(GPUIMaterialVariationDefinition materialVariationDefinition, int renderKey)
		{
			return GPUIMaterialVariationDataProvider.GetMaterialVariationDataWithHash(materialVariationDefinition, GPUIUtility.GenerateHash(new int[]
			{
				materialVariationDefinition.GetInstanceIDGPUI(),
				renderKey
			}));
		}

		// Token: 0x06000030 RID: 48 RVA: 0x00002B1C File Offset: 0x00000D1C
		public static GPUIMaterialVariationData GetMaterialVariationDataWithHash(GPUIMaterialVariationDefinition materialVariationDefinition, int hash)
		{
			GPUIMaterialVariationData gpuimaterialVariationData;
			if (!GPUIMaterialVariationDataProvider.Instance.TryGetData(hash, out gpuimaterialVariationData))
			{
				gpuimaterialVariationData = new GPUIMaterialVariationData(materialVariationDefinition);
				gpuimaterialVariationData.Initialize();
				GPUIMaterialVariationDataProvider.Instance.AddOrSet(hash, gpuimaterialVariationData);
			}
			return gpuimaterialVariationData;
		}

		// Token: 0x04000035 RID: 53
		public static GPUIMaterialVariationDataProvider _instance;
	}
}
