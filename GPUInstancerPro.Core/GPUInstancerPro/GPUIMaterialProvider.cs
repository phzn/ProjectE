using System;
using System.Collections.Generic;
using UnityEngine;

namespace GPUInstancerPro
{
	// Token: 0x0200003B RID: 59
	public class GPUIMaterialProvider : GPUIDataProvider<int, Material>
	{
		// Token: 0x06000245 RID: 581 RVA: 0x0000C314 File Offset: 0x0000A514
		public override void Initialize()
		{
			base.Initialize();
			Action onInitialized = this.OnInitialized;
			if (onInitialized == null)
			{
				return;
			}
			onInitialized();
		}

		// Token: 0x06000246 RID: 582 RVA: 0x0000C32C File Offset: 0x0000A52C
		public override void Dispose()
		{
			if (this._dataDict != null)
			{
				foreach (Material material in this._dataDict.Values)
				{
					if (material != null && material.name.EndsWith(GPUIShaderBindings.GPUI_REPLACEMENT_MATERIAL_NAME_SUFFIX))
					{
						material.DestroyGeneric();
					}
				}
			}
			base.Dispose();
		}

		// Token: 0x06000247 RID: 583 RVA: 0x0000C3AC File Offset: 0x0000A5AC
		public bool TryGetReplacementMaterial(Material originalMat, List<string> keywords, string extensionCode, out Material replacementMat)
		{
			replacementMat = GPUIShaderBindings.Instance.ErrorMaterial;
			if (!base.IsInitialized || originalMat == null || originalMat.shader == null)
			{
				return false;
			}
			int num = originalMat.GetInstanceIDGPUI();
			if (!string.IsNullOrEmpty(extensionCode))
			{
				num = GPUIUtility.GenerateHash(new int[]
				{
					num,
					extensionCode.GetHashCode()
				});
			}
			Material material;
			if (this.TryGetData(num, out material))
			{
				if (material == null || material.shader == null || material.shader == GPUIConstants.ShaderUnityInternalError)
				{
					this._dataDict.Remove(num);
				}
				else
				{
					if (keywords == null || keywords.Count == 0)
					{
						replacementMat = material;
						return true;
					}
					keywords.Sort();
					int num2 = GPUIUtility.GenerateHash(new int[]
					{
						num,
						string.Concat(keywords).GetHashCode()
					});
					Material material2;
					if (this.TryGetData(num2, out material2))
					{
						if (!(material2 == null) && !(material2.shader == null) && !(material2.shader == GPUIConstants.ShaderUnityInternalError))
						{
							replacementMat = material2;
							return true;
						}
						this._dataDict.Remove(num2);
					}
					bool flag = true;
					foreach (string text in keywords)
					{
						if (!material.IsKeywordEnabled(text))
						{
							flag = false;
							break;
						}
					}
					if (flag)
					{
						material2 = material;
					}
					else
					{
						material2 = material.CopyWithShader(material.shader);
						foreach (string text2 in keywords)
						{
							material2.EnableKeyword(text2);
						}
					}
					this._dataDict.Add(num2, material2);
					replacementMat = material2;
					return true;
				}
			}
			if (GPUIShaderBindings.Instance.GetInstancedMaterial(originalMat, extensionCode, out replacementMat))
			{
				this._dataDict.Add(num, replacementMat);
				return keywords == null || keywords.Count <= 0 || this.TryGetReplacementMaterial(originalMat, keywords, extensionCode, out replacementMat);
			}
			if (this._missingBuildShaders == null)
			{
				this._missingBuildShaders = new List<Shader>();
			}
			if (!this._missingBuildShaders.Contains(originalMat.shader))
			{
				this._missingBuildShaders.Add(originalMat.shader);
				Debug.LogError(GPUIConstants.LOG_PREFIX + "Can not find GPU Instancer Pro setup for shader: " + originalMat.shader.name);
			}
			return false;
		}

		// Token: 0x0400016E RID: 366
		private List<Shader> _missingBuildShaders;
	}
}
