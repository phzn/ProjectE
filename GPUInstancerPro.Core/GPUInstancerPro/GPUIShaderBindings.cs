using System;
using System.Collections.Generic;
using UnityEngine;

namespace GPUInstancerPro
{
	// Token: 0x02000033 RID: 51
	public class GPUIShaderBindings : ScriptableObject
	{
		// Token: 0x17000048 RID: 72
		// (get) Token: 0x060001D7 RID: 471 RVA: 0x00009AED File Offset: 0x00007CED
		// (set) Token: 0x060001D8 RID: 472 RVA: 0x00009B0B File Offset: 0x00007D0B
		public static GPUIShaderBindings Instance
		{
			get
			{
				if (GPUIShaderBindings._instance == null)
				{
					GPUIShaderBindings._instance = GPUIShaderBindings.GetDefaultShaderBindings();
				}
				return GPUIShaderBindings._instance;
			}
			set
			{
				GPUIShaderBindings._instance = value;
			}
		}

		// Token: 0x060001D9 RID: 473 RVA: 0x00009B14 File Offset: 0x00007D14
		public static GPUIShaderBindings GetDefaultShaderBindings()
		{
			GPUIShaderBindings gpuishaderBindings = GPUIUtility.LoadResource<GPUIShaderBindings>("GPUIShaderBindings");
			if (gpuishaderBindings == null)
			{
				gpuishaderBindings = ScriptableObject.CreateInstance<GPUIShaderBindings>();
			}
			return gpuishaderBindings;
		}

		// Token: 0x060001DA RID: 474 RVA: 0x00009B3C File Offset: 0x00007D3C
		public bool IsShaderSetupForGPUI(Shader shader, string extensionCode)
		{
			return shader != null && this.IsShaderSetupForGPUI(shader.name, extensionCode);
		}

		// Token: 0x060001DB RID: 475 RVA: 0x00009B58 File Offset: 0x00007D58
		public bool IsShaderSetupForGPUI(string shaderName, string extensionCode)
		{
			foreach (GPUIShaderInstance gpuishaderInstance in this.shaderInstances)
			{
				if ((gpuishaderInstance.shaderName.Equals(shaderName) || gpuishaderInstance.replacementShaderName.Equals(shaderName)) && GPUIShaderBindings.IsExtensionEqual(gpuishaderInstance, extensionCode))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x060001DC RID: 476 RVA: 0x00009BD0 File Offset: 0x00007DD0
		public bool IsShaderSetupForGPUIAnyExtension(string shaderName)
		{
			if (string.IsNullOrEmpty(shaderName))
			{
				return false;
			}
			foreach (GPUIShaderInstance gpuishaderInstance in this.shaderInstances)
			{
				if (gpuishaderInstance.shaderName.Equals(shaderName) || gpuishaderInstance.replacementShaderName.Equals(shaderName))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x060001DD RID: 477 RVA: 0x00009C4C File Offset: 0x00007E4C
		public virtual bool GetInstancedShader(Shader shader, string extensionCode, out Shader resultShader)
		{
			resultShader = this.ErrorShader;
			return shader != null && this.GetInstancedShader(shader.name, extensionCode, out resultShader);
		}

		// Token: 0x060001DE RID: 478 RVA: 0x00009C70 File Offset: 0x00007E70
		public virtual bool GetInstancedShader(string shaderName, string extensionCode, out Shader resultShader)
		{
			this.ClearEmptyShaderInstances();
			resultShader = this.ErrorShader;
			if (string.IsNullOrEmpty(shaderName))
			{
				return false;
			}
			if (this.shaderInstances == null)
			{
				this.shaderInstances = new List<GPUIShaderInstance>();
			}
			foreach (GPUIShaderInstance gpuishaderInstance in this.shaderInstances)
			{
				if ((gpuishaderInstance.shaderName.Equals(shaderName) || gpuishaderInstance.replacementShaderName.Equals(shaderName)) && GPUIShaderBindings.IsExtensionEqual(gpuishaderInstance, extensionCode))
				{
					resultShader = gpuishaderInstance.replacementShader;
					return true;
				}
			}
			if (shaderName.Contains("GPUInstancerPro"))
			{
				shaderName = shaderName.Replace("GPUInstancerPro/", "").Replace("GPUInstancerPro/CrowdAnimations/", "");
				foreach (GPUIShaderInstance gpuishaderInstance2 in this.shaderInstances)
				{
					if ((gpuishaderInstance2.shaderName.Equals(shaderName) || gpuishaderInstance2.replacementShaderName.Equals(shaderName)) && GPUIShaderBindings.IsExtensionEqual(gpuishaderInstance2, extensionCode))
					{
						resultShader = gpuishaderInstance2.replacementShader;
						return true;
					}
				}
			}
			Shader shader = Shader.Find(GPUIUtility.ConvertToGPUIShaderName(shaderName, extensionCode, null));
			if (shader != null)
			{
				this.AddShaderInstance(shaderName, shader, extensionCode, false);
				resultShader = shader;
				return true;
			}
			return false;
		}

		// Token: 0x060001DF RID: 479 RVA: 0x00009DE8 File Offset: 0x00007FE8
		public virtual bool GetInstancedMaterial(Material originalMaterial, string extensionCode, out Material replacementMat)
		{
			replacementMat = this.ErrorMaterial;
			if (originalMaterial == null || originalMaterial.shader == null)
			{
				if (Application.isPlaying)
				{
					Debug.LogWarning(GPUIConstants.LOG_PREFIX + "One of the GPUI Renderers is missing material reference! Check the Material references in MeshRenderer.");
				}
				return false;
			}
			Shader shader;
			if (this.GetInstancedShader(originalMaterial.shader, extensionCode, out shader) && shader != null)
			{
				if (originalMaterial.shader == shader)
				{
					replacementMat = originalMaterial;
				}
				else
				{
					replacementMat = originalMaterial.CopyWithShader(shader);
				}
				return true;
			}
			return false;
		}

		// Token: 0x060001E0 RID: 480 RVA: 0x00009E6C File Offset: 0x0000806C
		public void AddShaderInstance(string shaderName, Shader replacementShader, string extensionCode, bool isUseOriginal = false)
		{
			if (this.shaderInstances == null)
			{
				this.shaderInstances = new List<GPUIShaderInstance>();
			}
			if (shaderName == "Hidden/InternalErrorShader")
			{
				return;
			}
			for (int i = 0; i < this.shaderInstances.Count; i++)
			{
				if (this.shaderInstances[i].shaderName == shaderName && GPUIShaderBindings.IsExtensionEqual(this.shaderInstances[i], extensionCode))
				{
					Debug.LogWarning(GPUIConstants.LOG_PREFIX + "Shader Instance already exists for shader: " + shaderName);
					return;
				}
			}
			this.shaderInstances.Add(new GPUIShaderInstance
			{
				shaderName = shaderName,
				replacementShaderName = replacementShader.name,
				extensionCode = extensionCode,
				isUseOriginal = isUseOriginal,
				modifiedDate = DateTime.Now.ToDateString()
			});
		}

		// Token: 0x060001E1 RID: 481 RVA: 0x00009F38 File Offset: 0x00008138
		public virtual void ClearEmptyShaderInstances()
		{
			if (this.shaderInstances != null)
			{
				int i = this.shaderInstances.Count - 1;
				while (i >= 0)
				{
					GPUIShaderInstance gpuishaderInstance = this.shaderInstances[i];
					if (gpuishaderInstance == null)
					{
						goto IL_006C;
					}
					Shader originalShader = gpuishaderInstance.originalShader;
					Shader replacementShader = gpuishaderInstance.replacementShader;
					if (!(originalShader != null) || !(originalShader.name != "Hidden/InternalErrorShader") || !(replacementShader != null) || !(replacementShader.name != "Hidden/InternalErrorShader"))
					{
						goto IL_006C;
					}
					IL_0078:
					i--;
					continue;
					IL_006C:
					this.shaderInstances.RemoveAt(i);
					goto IL_0078;
				}
			}
		}

		// Token: 0x060001E2 RID: 482 RVA: 0x00009FC5 File Offset: 0x000081C5
		public static bool IsExtensionEqual(GPUIShaderInstance shaderInstance, string extensionCode)
		{
			return (string.IsNullOrEmpty(extensionCode) && string.IsNullOrEmpty(shaderInstance.extensionCode)) || (extensionCode != null && extensionCode.Equals(shaderInstance.extensionCode));
		}

		// Token: 0x17000049 RID: 73
		// (get) Token: 0x060001E3 RID: 483 RVA: 0x00009FEF File Offset: 0x000081EF
		public Shader ErrorShader
		{
			get
			{
				if (this._errorShader == null)
				{
					this._errorShader = GPUIUtility.FindShader("Hidden/GPUInstancerPro/InternalErrorShader");
				}
				return this._errorShader;
			}
		}

		// Token: 0x1700004A RID: 74
		// (get) Token: 0x060001E4 RID: 484 RVA: 0x0000A018 File Offset: 0x00008218
		public Material ErrorMaterial
		{
			get
			{
				if (this._errorMaterial == null && this.ErrorShader != null)
				{
					this._errorMaterial = new Material(this.ErrorShader);
					this._errorMaterial.name = "GPUIInternalErrorMaterial";
				}
				return this._errorMaterial;
			}
		}

		// Token: 0x0400013D RID: 317
		public static readonly string GPUI_REPLACEMENT_MATERIAL_NAME_SUFFIX = "_GPUIReplacement";

		// Token: 0x0400013E RID: 318
		[SerializeField]
		public List<GPUIShaderInstance> shaderInstances;

		// Token: 0x0400013F RID: 319
		[SerializeField]
		public bool stripObjectMotionVectorVariants;

		// Token: 0x04000140 RID: 320
		[SerializeField]
		public bool stripPerInstanceLightProbeVariants;

		// Token: 0x04000141 RID: 321
		private static GPUIShaderBindings _instance;

		// Token: 0x04000142 RID: 322
		private Shader _errorShader;

		// Token: 0x04000143 RID: 323
		private Material _errorMaterial;
	}
}
