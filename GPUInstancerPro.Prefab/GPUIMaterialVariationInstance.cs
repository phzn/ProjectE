using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace GPUInstancerPro.PrefabModule
{
	// Token: 0x02000017 RID: 23
	[ExecuteInEditMode]
	[DefaultExecutionOrder(2000)]
	[RequireComponent(typeof(GPUIPrefab))]
	[HelpURL("https://wiki.gurbu.com/index.php?title=GPU_Instancer_Pro:BestPractices#Prefab_Manager_Material_Variations")]
	public class GPUIMaterialVariationInstance : MonoBehaviour
	{
		// Token: 0x0600008F RID: 143 RVA: 0x00005470 File Offset: 0x00003670
		private void OnEnable()
		{
			if (this.variationDefinition == null)
			{
				return;
			}
			if (this._gpuiPrefab == null)
			{
				this._gpuiPrefab = base.GetComponent<GPUIPrefab>();
			}
			this.ApplyVariation();
			GPUIPrefab gpuiPrefab = this._gpuiPrefab;
			gpuiPrefab.OnInstancingStatusModified = (UnityAction)Delegate.Remove(gpuiPrefab.OnInstancingStatusModified, new UnityAction(this.ApplyVariation));
			GPUIPrefab gpuiPrefab2 = this._gpuiPrefab;
			gpuiPrefab2.OnInstancingStatusModified = (UnityAction)Delegate.Combine(gpuiPrefab2.OnInstancingStatusModified, new UnityAction(this.ApplyVariation));
			GPUIPrefab gpuiPrefab3 = this._gpuiPrefab;
			gpuiPrefab3.OnBufferIndexModified = (Action<int>)Delegate.Remove(gpuiPrefab3.OnBufferIndexModified, new Action<int>(this.ApplyVariationWithBufferIndex));
			GPUIPrefab gpuiPrefab4 = this._gpuiPrefab;
			gpuiPrefab4.OnBufferIndexModified = (Action<int>)Delegate.Combine(gpuiPrefab4.OnBufferIndexModified, new Action<int>(this.ApplyVariationWithBufferIndex));
		}

		// Token: 0x06000090 RID: 144 RVA: 0x00005548 File Offset: 0x00003748
		private void OnDisable()
		{
			if (this._gpuiPrefab != null)
			{
				GPUIPrefab gpuiPrefab = this._gpuiPrefab;
				gpuiPrefab.OnInstancingStatusModified = (UnityAction)Delegate.Remove(gpuiPrefab.OnInstancingStatusModified, new UnityAction(this.ApplyVariation));
				GPUIPrefab gpuiPrefab2 = this._gpuiPrefab;
				gpuiPrefab2.OnBufferIndexModified = (Action<int>)Delegate.Remove(gpuiPrefab2.OnBufferIndexModified, new Action<int>(this.ApplyVariationWithBufferIndex));
			}
			this.RevertVariation();
		}

		// Token: 0x06000091 RID: 145 RVA: 0x000055B8 File Offset: 0x000037B8
		private void LoadVariationRenderers()
		{
			if (this._variationRenderers == null)
			{
				this._variationRenderers = new List<Renderer>();
			}
			else
			{
				this._variationRenderers.Clear();
			}
			foreach (Renderer renderer in base.GetComponentsInChildren<Renderer>())
			{
				Material[] sharedMaterials = renderer.sharedMaterials;
				for (int j = 0; j < sharedMaterials.Length; j++)
				{
					if (sharedMaterials[j] == this.variationDefinition.material)
					{
						this._variationRenderers.Add(renderer);
					}
				}
			}
		}

		// Token: 0x06000092 RID: 146 RVA: 0x00005639 File Offset: 0x00003839
		private void CreateVariationMPB()
		{
			this._variationMPB = new MaterialPropertyBlock();
			this.SetMPBValues();
		}

		// Token: 0x06000093 RID: 147 RVA: 0x0000564C File Offset: 0x0000384C
		private void SetMPBValues()
		{
			for (int i = 0; i < this.variationDefinition.items.Length; i++)
			{
				switch (this.variationDefinition.items[i].variationType)
				{
				case GPUIMaterialVariationType.Vector4:
					this._variationMPB.SetVector(this.variationDefinition.items[i].propertyName, this.GetVariation(i));
					break;
				case GPUIMaterialVariationType.Color:
					this._variationMPB.SetColor(this.variationDefinition.items[i].propertyName, this.GetVariation(i));
					break;
				case GPUIMaterialVariationType.Integer:
					this._variationMPB.SetInt(this.variationDefinition.items[i].propertyName, (int)this.GetVariation(i).x);
					break;
				case GPUIMaterialVariationType.Float:
					this._variationMPB.SetFloat(this.variationDefinition.items[i].propertyName, this.GetVariation(i).x);
					break;
				}
			}
		}

		// Token: 0x06000094 RID: 148 RVA: 0x00005762 File Offset: 0x00003962
		public Vector4 GetVariation(int index)
		{
			if (this.values != null && this.values.Length > index)
			{
				return this.values[index];
			}
			return this.variationDefinition.items[index].defaultValue;
		}

		// Token: 0x06000095 RID: 149 RVA: 0x0000579C File Offset: 0x0000399C
		public void ApplyVariation()
		{
			if (!base.enabled || this.variationDefinition == null || this.variationDefinition.items == null)
			{
				return;
			}
			if (this._gpuiPrefab == null)
			{
				this._gpuiPrefab = base.GetComponent<GPUIPrefab>();
			}
			if (this._gpuiPrefab.IsInstanced)
			{
				this.SetVariationBufferData();
				return;
			}
			if (!this._gpuiPrefab.IsRenderersDisabled)
			{
				this.ApplyVariationMPB();
			}
		}

		// Token: 0x06000096 RID: 150 RVA: 0x0000580E File Offset: 0x00003A0E
		private void ApplyVariationWithBufferIndex(int bufferIndex)
		{
			this.ApplyVariation();
		}

		// Token: 0x06000097 RID: 151 RVA: 0x00005818 File Offset: 0x00003A18
		public void SetVariation(int index, Vector4 variationValue)
		{
			if (this.values == null)
			{
				this.values = new Vector4[index + 1];
			}
			if (this.values.Length <= index)
			{
				Array.Resize<Vector4>(ref this.values, index + 1);
			}
			this.values[index] = variationValue;
			this.ApplyVariation();
		}

		// Token: 0x06000098 RID: 152 RVA: 0x00005868 File Offset: 0x00003A68
		private void RevertVariation()
		{
			if (this._gpuiPrefab == null)
			{
				return;
			}
			if (!this._gpuiPrefab.IsInstanced)
			{
				if (this._variationRenderers == null)
				{
					this.LoadVariationRenderers();
				}
				foreach (Renderer renderer in this._variationRenderers)
				{
					renderer.SetPropertyBlock(GPUIRenderingSystem.EmptyMPB);
				}
			}
		}

		// Token: 0x06000099 RID: 153 RVA: 0x000058E8 File Offset: 0x00003AE8
		private void SetVariationBufferData()
		{
			int num = this.variationDefinition.items.Length;
			for (int i = 0; i < num; i++)
			{
				if (this.variationDefinition.items[i].variationType == GPUIMaterialVariationType.Color && QualitySettings.activeColorSpace == ColorSpace.Linear)
				{
					this.variationDefinition.AddVariation(this._gpuiPrefab.renderKey, this._gpuiPrefab.bufferIndex * num + i, this.GetVariation(i).linear);
				}
				else
				{
					this.variationDefinition.AddVariation(this._gpuiPrefab.renderKey, this._gpuiPrefab.bufferIndex * num + i, this.GetVariation(i));
				}
			}
		}

		// Token: 0x0600009A RID: 154 RVA: 0x000059A4 File Offset: 0x00003BA4
		private void ApplyVariationMPB()
		{
			if (this._variationMPB == null)
			{
				this.CreateVariationMPB();
			}
			else
			{
				this.SetMPBValues();
			}
			if (this._variationRenderers == null)
			{
				this.LoadVariationRenderers();
			}
			foreach (Renderer renderer in this._variationRenderers)
			{
				renderer.SetPropertyBlock(this._variationMPB);
			}
		}

		// Token: 0x04000074 RID: 116
		[SerializeField]
		public GPUIMaterialVariationDefinition variationDefinition;

		// Token: 0x04000075 RID: 117
		[SerializeField]
		public Vector4[] values;

		// Token: 0x04000076 RID: 118
		[NonSerialized]
		private GPUIPrefab _gpuiPrefab;

		// Token: 0x04000077 RID: 119
		[NonSerialized]
		private List<Renderer> _variationRenderers;

		// Token: 0x04000078 RID: 120
		[NonSerialized]
		private MaterialPropertyBlock _variationMPB;
	}
}
