using System;
using UnityEngine;
using UnityEngine.Rendering;

namespace GPUInstancerPro
{
	// Token: 0x0200005F RID: 95
	public static class GPUIBillboardUtility
	{
		// Token: 0x06000365 RID: 869 RVA: 0x00012833 File Offset: 0x00010A33
		public static GPUIBillboard GenerateBillboardData(GameObject prefabObject)
		{
			return GPUIBillboardUtility.GenerateBillboardData(prefabObject, 2048, 8, 0.5f, 0.5f, 1f);
		}

		// Token: 0x06000366 RID: 870 RVA: 0x00012850 File Offset: 0x00010A50
		public static GPUIBillboard GenerateBillboardData(GameObject prefabObject, int atlasResolution, int frameCount, float brightness, float cutoffOverride, float normalStrength = 1f)
		{
			Bounds bounds = prefabObject.GetBounds(true);
			bounds.size = Vector3.Scale(bounds.size, prefabObject.transform.lossyScale.Reciprocal());
			bounds.center = Vector3.Scale(bounds.center, prefabObject.transform.lossyScale.Reciprocal());
			Vector2 vector = new Vector2(Vector2.Distance(Vector2.zero, new Vector2(bounds.size.x, bounds.size.z)), bounds.size.y);
			float num = -bounds.min.y;
			GPUIBillboard gpuibillboard = ScriptableObject.CreateInstance<GPUIBillboard>();
			gpuibillboard.name = prefabObject.name + "_Billboard";
			gpuibillboard.prefabObject = prefabObject;
			gpuibillboard.atlasResolution = (GPUIBillboard.GPUIBillboardResolution)atlasResolution;
			gpuibillboard.frameCount = frameCount;
			gpuibillboard.brightness = brightness;
			gpuibillboard.cutoffOverride = cutoffOverride;
			gpuibillboard.normalStrength = normalStrength;
			gpuibillboard.quadSize = vector;
			gpuibillboard.yPivotOffset = num;
			gpuibillboard.billboardShaderType = GPUIBillboard.GPUIBillboardShaderType.Default;
			if (GPUIRuntimeSettings.Instance.IsBuiltInRP)
			{
				gpuibillboard.billboardShaderType = GPUIBillboardUtility.DetermineBillboardShaderType(prefabObject);
			}
			return gpuibillboard;
		}

		// Token: 0x06000367 RID: 871 RVA: 0x0001296C File Offset: 0x00010B6C
		private static GPUIBillboard.GPUIBillboardShaderType DetermineBillboardShaderType(GameObject prefabObject)
		{
			MeshRenderer[] componentsInChildren = prefabObject.GetComponentsInChildren<MeshRenderer>();
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				foreach (Material material in componentsInChildren[i].sharedMaterials)
				{
					if (!(material == null) && !(material.shader == null))
					{
						if (material.shader.name.Contains("Tree Creator"))
						{
							return GPUIBillboard.GPUIBillboardShaderType.TreeCreator;
						}
						if (material.shader.name.Contains("SpeedTree"))
						{
							return GPUIBillboard.GPUIBillboardShaderType.SpeedTree;
						}
						if (material.shader.name.Contains("Tree Soft Occlusion"))
						{
							return GPUIBillboard.GPUIBillboardShaderType.SoftOcclusion;
						}
					}
				}
			}
			return GPUIBillboard.GPUIBillboardShaderType.Default;
		}

		// Token: 0x06000368 RID: 872 RVA: 0x00012A18 File Offset: 0x00010C18
		public static bool GenerateBillboardDelayed(GPUIBillboard billboard, bool saveAsAsset = false)
		{
			bool flag = GPUIBillboardUtility.GenerateBillboard(billboard, false);
			GPUIRenderingSystem.RegenerateRenderers();
			return flag;
		}

		// Token: 0x06000369 RID: 873 RVA: 0x00012A28 File Offset: 0x00010C28
		public static bool GenerateBillboard(GPUIBillboard billboard, bool saveAsAsset = false)
		{
			if (billboard == null || billboard.prefabObject == null)
			{
				Debug.LogError(GPUIConstants.LOG_PREFIX + "Can no generate billboard. Prefab is null!");
				return false;
			}
			GameObject gameObject = null;
			GameObject gameObject2 = null;
			int globalTextureMipmapLimit = QualitySettings.globalTextureMipmapLimit;
			QualitySettings.globalTextureMipmapLimit = 0;
			RenderPipelineAsset defaultRenderPipeline = GraphicsSettings.defaultRenderPipeline;
			RenderPipelineAsset renderPipeline = QualitySettings.renderPipeline;
			try
			{
				int num = (int)(billboard.atlasResolution / (GPUIBillboard.GPUIBillboardResolution)billboard.frameCount);
				billboard.albedoAtlasRT = new RenderTexture((int)billboard.atlasResolution, num, 32, RenderTextureFormat.ARGB32, RenderTextureReadWrite.Default)
				{
					enableRandomWrite = true,
					wrapMode = TextureWrapMode.Repeat,
					name = billboard.prefabObject.name + "_Albedo"
				};
				billboard.albedoAtlasRT.Create();
				billboard.normalAtlasRT = new RenderTexture((int)billboard.atlasResolution, num, 32, RenderTextureFormat.ARGB32, RenderTextureReadWrite.Default)
				{
					enableRandomWrite = true,
					wrapMode = TextureWrapMode.Repeat,
					name = billboard.prefabObject.name + "_Normal"
				};
				billboard.albedoAtlasRT.Create();
				gameObject = Object.Instantiate<GameObject>(billboard.prefabObject, Vector3.zero, Quaternion.identity);
				gameObject.transform.localScale = Vector3.one;
				gameObject.hideFlags = HideFlags.DontSave;
				Bounds bounds = gameObject.GetBounds(true);
				billboard.quadSize = new Vector2(Vector2.Distance(Vector2.zero, new Vector2(bounds.size.x, bounds.size.z)), bounds.size.y);
				billboard.yPivotOffset = -bounds.min.y;
				int num2 = 31;
				MeshRenderer[] componentsInChildren = gameObject.GetComponentsInChildren<MeshRenderer>();
				if (componentsInChildren == null || componentsInChildren.Length == 0)
				{
					Debug.LogError(GPUIConstants.LOG_PREFIX + "Cannot create GPU Instancer billboard for " + billboard.prefabObject.name + " : no mesh renderers found in prefab!");
					Object.DestroyImmediate(gameObject);
					return false;
				}
				for (int i = 0; i < componentsInChildren.Length; i++)
				{
					componentsInChildren[i].gameObject.layer = num2;
					for (int j = 0; j < componentsInChildren[i].sharedMaterials.Length; j++)
					{
						Material material = componentsInChildren[i].sharedMaterials[j];
						if (material != null && material.shader != null)
						{
							GPUIBillboard.GPUIBillboardCustomShaderProperties shaderCustomProperty = billboard.GetShaderCustomProperty(material.shader.name);
							if (!material.shader.GetPropertyNamesForType(ShaderPropertyType.Texture, true).Contains("_MainTex"))
							{
								if (shaderCustomProperty != null && !string.IsNullOrEmpty(shaderCustomProperty.mainTextureProperty) && material.HasProperty(shaderCustomProperty.mainTextureProperty))
								{
									material.SetTexture("_MainTex", material.GetTexture(shaderCustomProperty.mainTextureProperty));
								}
								else if (material.HasProperty("_MainTexture"))
								{
									material.SetTexture("_MainTex", material.GetTexture("_MainTexture"));
								}
								else if (material.HasProperty("_BaseMap"))
								{
									material.SetTexture("_MainTex", material.GetTexture("_BaseMap"));
								}
								if (shaderCustomProperty != null && shaderCustomProperty.useRedChannelCutoff)
								{
									material.EnableKeyword("GPUI_USE_RED_CHANNEL_CUTOFF");
								}
								else
								{
									material.DisableKeyword("GPUI_USE_RED_CHANNEL_CUTOFF");
								}
							}
							if (!material.shader.GetPropertyNamesForType(ShaderPropertyType.Color, true).Contains("_Color"))
							{
								if (shaderCustomProperty != null && !string.IsNullOrEmpty(shaderCustomProperty.mainColorProperty) && material.HasProperty(shaderCustomProperty.mainColorProperty))
								{
									material.SetColor("_Color", material.GetColor(shaderCustomProperty.mainColorProperty));
								}
								else if (material.HasProperty("_BaseColor"))
								{
									material.SetColor("_Color", material.GetColor("_BaseColor"));
								}
							}
						}
					}
				}
				Shader shader = GPUIUtility.FindShader("Hidden/GPUInstancerPro/Billboard/AlbedoBake");
				Shader shader2 = GPUIUtility.FindShader("Hidden/GPUInstancerPro/Billboard/NormalBake");
				Shader.SetGlobalFloat("_GPUIBillboardBrightness", billboard.brightness);
				Shader.SetGlobalFloat("_GPUIBillboardCutoffOverride", billboard.cutoffOverride);
				float num3 = Mathf.Max(billboard.quadSize.x, billboard.quadSize.y);
				gameObject2 = new GameObject("GPUI_BillboardCameraPivot");
				Camera camera = new GameObject().AddComponent<Camera>();
				camera.transform.SetParent(gameObject2.transform);
				camera.gameObject.hideFlags = HideFlags.DontSave;
				camera.cullingMask = 1 << num2;
				camera.clearFlags = CameraClearFlags.Color;
				camera.backgroundColor = Color.clear;
				camera.orthographic = true;
				camera.nearClipPlane = 0f;
				camera.farClipPlane = num3;
				camera.orthographicSize = num3 * 0.5f;
				camera.allowMSAA = false;
				camera.enabled = false;
				camera.renderingPath = RenderingPath.Forward;
				camera.transform.localPosition = new Vector3(0f, bounds.center.y, -num3 / 2f);
				int frameCount = billboard.frameCount;
				float num4 = 360f / (float)frameCount;
				GraphicsSettings.defaultRenderPipeline = null;
				QualitySettings.renderPipeline = null;
				RenderTexture temporary = RenderTexture.GetTemporary(num, num, 32, RenderTextureFormat.ARGB32, RenderTextureReadWrite.Linear);
				temporary.enableRandomWrite = true;
				temporary.Create();
				camera.targetTexture = temporary;
				for (int k = 0; k < frameCount; k++)
				{
					gameObject2.transform.rotation = Quaternion.AngleAxis(num4 * (float)k, Vector3.up);
					camera.RenderWithShader(shader, string.Empty);
					GPUITextureUtility.CopyTextureWithComputeShader(temporary, billboard.albedoAtlasRT, k * num, 0, 0, 0);
					camera.RenderWithShader(shader2, string.Empty);
					GPUITextureUtility.CopyTextureWithComputeShader(temporary, billboard.normalAtlasRT, k * num, 0, 0, 0);
				}
				GPUIBillboardUtility.DilateBillboardTexture(billboard.albedoAtlasRT, frameCount, false);
				GPUIBillboardUtility.DilateBillboardTexture(billboard.normalAtlasRT, frameCount, true);
			}
			catch (Exception ex)
			{
				GraphicsSettings.defaultRenderPipeline = defaultRenderPipeline;
				QualitySettings.renderPipeline = renderPipeline;
				string log_PREFIX = GPUIConstants.LOG_PREFIX;
				string text = "Error on billboard generation for: ";
				GameObject prefabObject = billboard.prefabObject;
				Debug.LogError(log_PREFIX + text + ((prefabObject != null) ? prefabObject.ToString() : null));
				QualitySettings.globalTextureMipmapLimit = globalTextureMipmapLimit;
				if (gameObject)
				{
					Object.DestroyImmediate(gameObject);
				}
				if (gameObject2)
				{
					Object.DestroyImmediate(gameObject2);
				}
				if (billboard.albedoAtlasRT)
				{
					Object.DestroyImmediate(billboard.albedoAtlasRT);
				}
				if (billboard.normalAtlasRT)
				{
					Object.DestroyImmediate(billboard.normalAtlasRT);
				}
				Debug.LogException(ex);
			}
			GraphicsSettings.defaultRenderPipeline = defaultRenderPipeline;
			QualitySettings.renderPipeline = renderPipeline;
			QualitySettings.globalTextureMipmapLimit = globalTextureMipmapLimit;
			Object.DestroyImmediate(gameObject);
			Object.DestroyImmediate(gameObject2);
			return true;
		}

		// Token: 0x0600036A RID: 874 RVA: 0x00013094 File Offset: 0x00011294
		public static GPUIBillboard FindBillboardAsset(GameObject prefabObject)
		{
			if (GPUIRuntimeSettings.Instance.billboardAssets != null)
			{
				foreach (GPUIBillboard gpuibillboard in GPUIRuntimeSettings.Instance.billboardAssets)
				{
					if (gpuibillboard != null && gpuibillboard.prefabObject == prefabObject)
					{
						return gpuibillboard;
					}
				}
			}
			return null;
		}

		// Token: 0x0600036B RID: 875 RVA: 0x00013110 File Offset: 0x00011310
		public static void DilateBillboardTexture(RenderTexture billboardTexture, int frameCount, bool isNormal)
		{
			RenderTexture renderTexture = new RenderTexture(billboardTexture.width, billboardTexture.height, billboardTexture.depth, billboardTexture.format, RenderTextureReadWrite.Linear)
			{
				enableRandomWrite = true,
				wrapMode = billboardTexture.wrapMode,
				name = billboardTexture.name
			};
			renderTexture.Create();
			ComputeShader cs_Billboard = GPUIConstants.CS_Billboard;
			cs_Billboard.SetTexture(0, "result", renderTexture);
			cs_Billboard.SetTexture(0, "billboardSource", billboardTexture);
			cs_Billboard.SetInts("billboardSize", new int[] { billboardTexture.width, billboardTexture.height });
			cs_Billboard.SetInt("frameCount", frameCount);
			cs_Billboard.SetBool("isNormal", isNormal);
			cs_Billboard.Dispatch(0, Mathf.CeilToInt((float)billboardTexture.width / (GPUIConstants.CS_THREAD_COUNT_2D * (float)frameCount)), Mathf.CeilToInt((float)billboardTexture.height / GPUIConstants.CS_THREAD_COUNT_2D), frameCount);
			GPUITextureUtility.CopyTextureWithComputeShader(renderTexture, billboardTexture, 0, 0, 0, 0);
			renderTexture.DestroyRenderTexture();
		}

		// Token: 0x0600036C RID: 876 RVA: 0x00013200 File Offset: 0x00011400
		public static Material CreateBillboardMaterial(Texture albedo, Texture normal, float cutOff, int frameCount, float normalStrength, GPUIBillboard.GPUIBillboardShaderType shaderType)
		{
			Material material = new Material(GPUIBillboardUtility.GetBillboardShader(shaderType));
			material.SetTexture("_AlbedoAtlas", albedo);
			material.SetTexture("_NormalAtlas", normal);
			material.SetFloat("_CutOff_GPUI", cutOff);
			material.SetInt("_FrameCount_GPUI", frameCount);
			material.SetFloat("_NormalStrength_GPUI", normalStrength);
			if (QualitySettings.billboardsFaceCameraPosition)
			{
				material.EnableKeyword("BILLBOARD_FACE_CAMERA_POS");
			}
			else
			{
				material.DisableKeyword("BILLBOARD_FACE_CAMERA_POS");
			}
			return material;
		}

		// Token: 0x0600036D RID: 877 RVA: 0x00013278 File Offset: 0x00011478
		public static Material CreateBillboardMaterial(GPUIBillboard billboard)
		{
			if (billboard._billboardMaterial != null)
			{
				billboard._billboardMaterial.DestroyGeneric();
			}
			Material material;
			if (billboard.albedoAtlasRT != null && billboard.normalAtlasRT != null)
			{
				material = GPUIBillboardUtility.CreateBillboardMaterial(billboard.albedoAtlasRT, billboard.normalAtlasRT, billboard.cutoffOverride, billboard.frameCount, billboard.normalStrength, billboard.billboardShaderType);
			}
			else
			{
				material = GPUIBillboardUtility.CreateBillboardMaterial(billboard.albedoAtlasTexture, billboard.normalAtlasTexture, billboard.cutoffOverride, billboard.frameCount, billboard.normalStrength, billboard.billboardShaderType);
			}
			if (billboard.billboardShaderType == GPUIBillboard.GPUIBillboardShaderType.SpeedTree)
			{
				Renderer componentInChildren = billboard.prefabObject.GetComponentInChildren<MeshRenderer>();
				if (componentInChildren != null)
				{
					if (componentInChildren.sharedMaterial.IsKeywordEnabled("EFFECT_HUE_VARIATION"))
					{
						material.EnableKeyword("SPDTREE_HUE_VARIATION");
						material.SetFloat("_UseSPDHueVariation", 1f);
						if (componentInChildren.sharedMaterial.HasProperty("_HueVariation"))
						{
							material.SetVector("_SPDHueVariation", componentInChildren.sharedMaterial.GetVector("_HueVariation"));
						}
						if (componentInChildren.sharedMaterial.HasProperty("_HueVariationColor"))
						{
							material.SetVector("_SPDHueVariation", componentInChildren.sharedMaterial.GetVector("_HueVariationColor"));
						}
					}
					else
					{
						material.DisableKeyword("SPDTREE_HUE_VARIATION");
					}
				}
			}
			billboard._billboardMaterial = material;
			return material;
		}

		// Token: 0x0600036E RID: 878 RVA: 0x000133D0 File Offset: 0x000115D0
		public static Mesh GenerateQuadMesh(GPUIBillboard billboard)
		{
			if (billboard._quadMesh != null)
			{
				billboard._quadMesh.DestroyGeneric();
			}
			Rect rect = new Rect(0f, 0f, 1f, 1f);
			if (billboard.quadSize.x < billboard.quadSize.y)
			{
				rect.width = billboard.quadSize.x / billboard.quadSize.y;
				rect.x = (1f - rect.width) / 2f;
			}
			else if (billboard.quadSize.x > billboard.quadSize.y)
			{
				rect.height = billboard.quadSize.y / billboard.quadSize.x;
				rect.y = (1f - rect.height) / 2f;
			}
			billboard._quadMesh = GPUIUtility.GenerateQuadMesh(billboard.quadSize.x, billboard.quadSize.y, new Rect?(rect), true, 0f, billboard.yPivotOffset, false);
			return billboard._quadMesh;
		}

		// Token: 0x0600036F RID: 879 RVA: 0x000134F0 File Offset: 0x000116F0
		public static Shader GetBillboardShader(GPUIBillboard.GPUIBillboardShaderType shaderType)
		{
			GPUIRenderPipeline renderPipeline = GPUIRuntimeSettings.Instance.RenderPipeline;
			Shader shader;
			if (renderPipeline != GPUIRenderPipeline.URP)
			{
				if (renderPipeline != GPUIRenderPipeline.HDRP)
				{
					switch (shaderType)
					{
					case GPUIBillboard.GPUIBillboardShaderType.SpeedTree:
						shader = GPUIUtility.FindShader("GPUInstancerPro/Billboard/2DRendererSpeedTree");
						break;
					case GPUIBillboard.GPUIBillboardShaderType.TreeCreator:
						shader = GPUIUtility.FindShader("GPUInstancerPro/Billboard/2DRendererTreeCreator");
						break;
					case GPUIBillboard.GPUIBillboardShaderType.SoftOcclusion:
						shader = GPUIUtility.FindShader("GPUInstancerPro/Billboard/2DRendererSoftOcclusion");
						break;
					default:
						shader = GPUIUtility.FindShader("GPUInstancerPro/Billboard/BillboardBuiltin_GPUIPro");
						break;
					}
				}
				else
				{
					shader = GPUIUtility.FindShader("GPUInstancerPro/Billboard/BillboardHDRP_GPUIPro");
				}
			}
			else
			{
				shader = GPUIUtility.FindShader("GPUInstancerPro/Billboard/BillboardURP_GPUIPro");
			}
			if (shader == null)
			{
				Debug.LogError(string.Concat(new string[]
				{
					GPUIConstants.LOG_PREFIX,
					"Cannot find the GPUI billboard shader. Please select 'Tools > GPU Instancer Pro > Reimport Packages' and make sure the billboard shaders are imported correctly. RenderPipeline: ",
					GPUIRuntimeSettings.Instance.RenderPipeline.ToString(),
					" ShaderType: ",
					shaderType.ToString()
				}));
				shader = GPUIShaderBindings.Instance.ErrorShader;
			}
			return shader;
		}
	}
}
