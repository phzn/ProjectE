using System;
using UnityEngine;
using UnityEngine.Events;

namespace GPUInstancerPro.PrefabModule
{
	// Token: 0x02000015 RID: 21
	[DefaultExecutionOrder(200)]
	[HelpURL("https://wiki.gurbu.com/index.php?title=GPU_Instancer_Pro:GettingStarted#GPUI_Prefab")]
	public class GPUIPrefab : GPUIPrefabBase
	{
		// Token: 0x17000005 RID: 5
		// (get) Token: 0x0600004E RID: 78 RVA: 0x000037A0 File Offset: 0x000019A0
		public bool IsRenderersDisabled
		{
			get
			{
				return this._isRenderersDisabled;
			}
		}

		// Token: 0x17000006 RID: 6
		// (get) Token: 0x0600004F RID: 79 RVA: 0x000037A8 File Offset: 0x000019A8
		// (set) Token: 0x06000050 RID: 80 RVA: 0x000037B0 File Offset: 0x000019B0
		public GPUIPrefabManager registeredManager { get; internal set; }

		// Token: 0x06000051 RID: 81 RVA: 0x000037BC File Offset: 0x000019BC
		internal void SetInstancingData(GPUIPrefabManager registeredManager, int prefabID, int renderKey, int bufferIndex)
		{
			this.registeredManager = registeredManager;
			base.renderKey = renderKey;
			base.bufferIndex = bufferIndex;
			if (this._prefabID == 0)
			{
				this._prefabID = prefabID;
			}
			registeredManager.SetPrefabInstanceRenderersEnabled(this, false);
			this._isBeingAddedToThePrefabManager = false;
			UnityAction onInstancingStatusModified = this.OnInstancingStatusModified;
			if (onInstancingStatusModified == null)
			{
				return;
			}
			onInstancingStatusModified();
		}

		// Token: 0x06000052 RID: 82 RVA: 0x00003810 File Offset: 0x00001A10
		internal void ClearInstancingData(bool enableRenderers)
		{
			if (enableRenderers && this.registeredManager != null)
			{
				this.registeredManager.SetPrefabInstanceRenderersEnabled(this, true);
			}
			this.registeredManager = null;
			base.renderKey = 0;
			this.SetBufferIndex(-1);
			this._isBeingAddedToThePrefabManager = false;
			UnityAction onInstancingStatusModified = this.OnInstancingStatusModified;
			if (onInstancingStatusModified == null)
			{
				return;
			}
			onInstancingStatusModified();
		}

		// Token: 0x06000053 RID: 83 RVA: 0x00003868 File Offset: 0x00001A68
		public void RemovePrefabInstance()
		{
			if (!base.IsInstanced)
			{
				return;
			}
			if (!this.registeredManager.RemovePrefabInstance(this, -1))
			{
				Debug.LogError(GPUIConstants.LOG_PREFIX + "Can not remove prefab instance with prefab ID: " + base.GetPrefabID().ToString(), this);
			}
		}

		// Token: 0x06000054 RID: 84 RVA: 0x000038B0 File Offset: 0x00001AB0
		internal void UpdateTransformData()
		{
			if (!base.IsInstanced)
			{
				return;
			}
			if (base.CachedTransform.hasChanged)
			{
				this.registeredManager.UpdateTransformData(this);
			}
		}

		// Token: 0x06000055 RID: 85 RVA: 0x000038D4 File Offset: 0x00001AD4
		public void SetRenderersEnabled(bool enabled)
		{
			if (this._isRenderersDisabled != enabled)
			{
				return;
			}
			GPUIRenderingSystem.prefabRendererList.Clear();
			base.transform.GetPrefabRenderers(GPUIRenderingSystem.prefabRendererList);
			foreach (Renderer renderer in GPUIRenderingSystem.prefabRendererList)
			{
				renderer.enabled = enabled;
			}
			LODGroup lodgroup;
			if (base.TryGetComponent<LODGroup>(out lodgroup))
			{
				lodgroup.enabled = enabled;
			}
			this._isRenderersDisabled = !enabled;
		}

		// Token: 0x06000056 RID: 86 RVA: 0x00003964 File Offset: 0x00001B64
		internal void SetBufferIndex(int bufferIndex)
		{
			int bufferIndex2 = base.bufferIndex;
			if (bufferIndex2 != bufferIndex)
			{
				base.bufferIndex = bufferIndex;
				Action<int> onBufferIndexModified = this.OnBufferIndexModified;
				if (onBufferIndexModified == null)
				{
					return;
				}
				onBufferIndexModified(bufferIndex2);
			}
		}

		// Token: 0x06000057 RID: 87 RVA: 0x00003994 File Offset: 0x00001B94
		public override void OnOptionalRendererStatusChanged()
		{
			GPUIPrefabPrototypeData gpuiprefabPrototypeData;
			if (!base.IsInstanced || !this.registeredManager.IsInitialized || !this.registeredManager.TryGetPrefabPrototypeDataWithPrefabID(this._prefabID, out gpuiprefabPrototypeData))
			{
				return;
			}
			int num = (gpuiprefabPrototypeData._optionalRendererCount - 1) / 32 + 1;
			gpuiprefabPrototypeData.optionalRendererStatusData[base.bufferIndex * num] = this.optionalRendererStatus;
			gpuiprefabPrototypeData.isOptionalRendererStatusModified = true;
		}

		// Token: 0x06000058 RID: 88 RVA: 0x000039FC File Offset: 0x00001BFC
		public override void OnOptionalRendererStatusExtraChanged()
		{
			GPUIPrefabPrototypeData gpuiprefabPrototypeData;
			if (!base.IsInstanced || !this.registeredManager.IsInitialized || this.optionalRendererStatusExtra == null || !this.registeredManager.TryGetPrefabPrototypeDataWithPrefabID(this._prefabID, out gpuiprefabPrototypeData))
			{
				return;
			}
			int num = (gpuiprefabPrototypeData._optionalRendererCount - 1) / 32 + 1;
			int num2 = 0;
			while (num2 < num - 1 && num2 < this.optionalRendererStatusExtra.Length)
			{
				gpuiprefabPrototypeData.optionalRendererStatusData[base.bufferIndex * num + num2 + 1] = this.optionalRendererStatusExtra[num2];
				num2++;
			}
			gpuiprefabPrototypeData.isOptionalRendererStatusModified = true;
		}

		// Token: 0x06000059 RID: 89 RVA: 0x00003A8C File Offset: 0x00001C8C
		public override void SetMaterialVariation(int index, Vector4 variationValue)
		{
			GPUIMaterialVariationInstance gpuimaterialVariationInstance;
			if (!base.TryGetComponent<GPUIMaterialVariationInstance>(out gpuimaterialVariationInstance))
			{
				Debug.LogError(GPUIConstants.LOG_PREFIX + "The prefab instance does not contain a GPUIMaterialVariationInstance component, so the variation cannot be set.");
				return;
			}
			gpuimaterialVariationInstance.SetVariation(index, variationValue);
		}

		// Token: 0x04000066 RID: 102
		[SerializeField]
		private bool _isRenderersDisabled;

		// Token: 0x04000068 RID: 104
		internal bool _isBeingAddedToThePrefabManager;
	}
}
