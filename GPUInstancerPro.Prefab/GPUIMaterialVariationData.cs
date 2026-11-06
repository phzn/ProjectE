using System;
using System.Collections.Generic;
using UnityEngine;

namespace GPUInstancerPro.PrefabModule
{
	// Token: 0x02000006 RID: 6
	public class GPUIMaterialVariationData : IGPUIDisposable, IDisposable
	{
		// Token: 0x0600000E RID: 14 RVA: 0x00002239 File Offset: 0x00000439
		public GPUIMaterialVariationData(GPUIMaterialVariationDefinition definition)
		{
			this._definition = definition;
		}

		// Token: 0x0600000F RID: 15 RVA: 0x00002248 File Offset: 0x00000448
		internal void Initialize()
		{
			if (this._isInitialized || !GPUIRuntimeSettings.IsSupportedPlatform())
			{
				return;
			}
			if (this._definition == null)
			{
				Debug.LogError(GPUIConstants.LOG_PREFIX + "Can not find Material Variation Definition.");
				return;
			}
			Shader shader = this._definition.replacementShader;
			if (shader == null)
			{
				Debug.LogError(GPUIConstants.LOG_PREFIX + "Can not find Replacement Shader for the Material Variation Definition. Please make sure to Generate Shader for the Material Variation Definition.", this._definition);
				shader = GPUIShaderBindings.Instance.ErrorShader;
			}
			this._isInitialized = true;
			if (this._variationBuffer == null)
			{
				this._variationBuffer = new GPUIDataBuffer<Vector4>(this._definition.bufferName, 0, GraphicsBuffer.Target.Structured);
			}
			if (this._renderKeys == null)
			{
				this._renderKeys = new List<int>();
			}
			if (this._definition.material.shader == shader)
			{
				return;
			}
			GPUIRenderingSystem.InitializeRenderingSystem();
			GPUIMaterialProvider materialProvider = GPUIRenderingSystem.Instance.MaterialProvider;
			materialProvider.OnInitialized = (Action)Delegate.Remove(materialProvider.OnInitialized, new Action(this.OnMaterialProviderInitialized));
			GPUIMaterialProvider materialProvider2 = GPUIRenderingSystem.Instance.MaterialProvider;
			materialProvider2.OnInitialized = (Action)Delegate.Combine(materialProvider2.OnInitialized, new Action(this.OnMaterialProviderInitialized));
			List<string> list = new List<string> { GPUIPrefabConstants.Kw_GPUI_MATERIAL_VARIATION };
			int num = this._definition.material.GetInstanceIDGPUI();
			int num2 = GPUIUtility.GenerateHash(new int[]
			{
				num,
				"CROWD".GetHashCode()
			});
			num = GPUIUtility.GenerateHash(new int[]
			{
				num,
				string.Concat(list).GetHashCode()
			});
			Material material;
			if (GPUIRenderingSystem.Instance.MaterialProvider.TryGetData(num, out material) && material != null && material.name.EndsWith("_MV" + GPUIShaderBindings.GPUI_REPLACEMENT_MATERIAL_NAME_SUFFIX))
			{
				return;
			}
			material = new Material(shader);
			material.CopyPropertiesFromMaterial(this._definition.material);
			material.name = this._definition.material.name + "_MV" + GPUIShaderBindings.GPUI_REPLACEMENT_MATERIAL_NAME_SUFFIX;
			material.hideFlags = HideFlags.HideAndDontSave;
			material.EnableKeyword(GPUIPrefabConstants.Kw_GPUI_MATERIAL_VARIATION);
			GPUIRenderingSystem.Instance.MaterialProvider.AddOrSet(num, material);
			if (this._definition.isCrowdAnimations)
			{
				GPUIRenderingSystem.Instance.MaterialProvider.AddOrSet(GPUIUtility.GenerateHash(new int[]
				{
					num2,
					string.Concat(list).GetHashCode()
				}), material);
			}
			list.Add("LOD_FADE_CROSSFADE");
			list.Sort();
			num = GPUIUtility.GenerateHash(new int[]
			{
				this._definition.material.GetInstanceIDGPUI(),
				string.Concat(list).GetHashCode()
			});
			material = new Material(material);
			material.name = this._definition.material.name + "_MV" + GPUIShaderBindings.GPUI_REPLACEMENT_MATERIAL_NAME_SUFFIX;
			material.EnableKeyword("LOD_FADE_CROSSFADE");
			GPUIRenderingSystem.Instance.MaterialProvider.AddOrSet(num, material);
			if (this._definition.isCrowdAnimations)
			{
				GPUIRenderingSystem.Instance.MaterialProvider.AddOrSet(GPUIUtility.GenerateHash(new int[]
				{
					num2,
					string.Concat(list).GetHashCode()
				}), material);
			}
		}

		// Token: 0x06000010 RID: 16 RVA: 0x00002576 File Offset: 0x00000776
		private void OnMaterialProviderInitialized()
		{
			this._isInitialized = false;
			this.Initialize();
		}

		// Token: 0x06000011 RID: 17 RVA: 0x00002585 File Offset: 0x00000785
		public void ReleaseBuffers()
		{
			if (!this._isInitialized)
			{
				return;
			}
			this._isInitialized = false;
			if (this._variationBuffer != null)
			{
				this._variationBuffer.ReleaseBuffers();
			}
		}

		// Token: 0x06000012 RID: 18 RVA: 0x000025AA File Offset: 0x000007AA
		public void Dispose()
		{
			this.ReleaseBuffers();
			if (this._variationBuffer != null)
			{
				this._variationBuffer.Dispose();
			}
			this._variationBuffer = null;
			this._renderKeys = null;
		}

		// Token: 0x06000013 RID: 19 RVA: 0x000025D3 File Offset: 0x000007D3
		public void AddVariation(int renderKey, int bufferIndex, Vector4 value)
		{
			this.Initialize();
			if (!this._isInitialized)
			{
				return;
			}
			this._variationBuffer.AddOrSet(bufferIndex, value);
			if (!this._renderKeys.Contains(renderKey))
			{
				this._renderKeys.Add(renderKey);
				GPUIRenderingSystem.AddDependentDisposable(renderKey, this);
			}
		}

		// Token: 0x06000014 RID: 20 RVA: 0x00002614 File Offset: 0x00000814
		public void UpdateVariationBuffer()
		{
			if (!this._isInitialized)
			{
				return;
			}
			if (this._variationBuffer.UpdateBufferData(false))
			{
				foreach (int num in this._renderKeys)
				{
					GPUIRenderingSystem.AddMaterialPropertyOverride(num, this._definition.bufferName, this._variationBuffer.Buffer, -1, -1, false);
				}
			}
		}

		// Token: 0x0400000B RID: 11
		private GPUIMaterialVariationDefinition _definition;

		// Token: 0x0400000C RID: 12
		private GPUIDataBuffer<Vector4> _variationBuffer;

		// Token: 0x0400000D RID: 13
		private bool _isInitialized;

		// Token: 0x0400000E RID: 14
		private List<int> _renderKeys;
	}
}
