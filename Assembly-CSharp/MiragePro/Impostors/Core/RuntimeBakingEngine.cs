using System;
using System.Collections.Generic;
using MiragePro.Impostors.Elements;
using UnityEngine;
using UnityEngine.Rendering;

namespace MiragePro.Impostors.Core
{
	// Token: 0x0200027C RID: 636
	public class RuntimeBakingEngine : IBakingEngine
	{
		// Token: 0x170002C6 RID: 710
		// (get) Token: 0x06001090 RID: 4240 RVA: 0x0006E071 File Offset: 0x0006C271
		// (set) Token: 0x06001091 RID: 4241 RVA: 0x0006E079 File Offset: 0x0006C279
		public RuntimeRendererUtility RuntimeRenderer { get; private set; }

		// Token: 0x170002C7 RID: 711
		// (get) Token: 0x06001092 RID: 4242 RVA: 0x0006E084 File Offset: 0x0006C284
		private Shader depthShader
		{
			get
			{
				if (!(this._depthShader != null))
				{
					return this._depthShader = Shader.Find("MiragePro/Internal/Depth");
				}
				return this._depthShader;
			}
		}

		// Token: 0x170002C8 RID: 712
		// (get) Token: 0x06001093 RID: 4243 RVA: 0x0006E0BC File Offset: 0x0006C2BC
		private Shader mergeShader
		{
			get
			{
				if (!(this._mergeShader != null))
				{
					return this._mergeShader = Shader.Find("MiragePro/Internal/MergeChannels");
				}
				return this._mergeShader;
			}
		}

		// Token: 0x170002C9 RID: 713
		// (get) Token: 0x06001094 RID: 4244 RVA: 0x0006E0F4 File Offset: 0x0006C2F4
		private Shader postProcShader
		{
			get
			{
				if (!(this._postProcShader != null))
				{
					return this._postProcShader = Shader.Find("MiragePro/Internal/AtlasPostProcessor");
				}
				return this._postProcShader;
			}
		}

		// Token: 0x170002CA RID: 714
		// (get) Token: 0x06001095 RID: 4245 RVA: 0x0006E12C File Offset: 0x0006C32C
		private Shader maskEstimatorShader
		{
			get
			{
				if (!(this._maskEstimatorShader != null))
				{
					return this._maskEstimatorShader = Shader.Find("MiragePro/Internal/MaskEstimator");
				}
				return this._maskEstimatorShader;
			}
		}

		// Token: 0x170002CB RID: 715
		// (get) Token: 0x06001096 RID: 4246 RVA: 0x0006E164 File Offset: 0x0006C364
		private Shader normalShader
		{
			get
			{
				if (!(this._normalShader != null))
				{
					return this._normalShader = Shader.Find("MiragePro/Internal/Normals");
				}
				return this._normalShader;
			}
		}

		// Token: 0x170002CC RID: 716
		// (get) Token: 0x06001097 RID: 4247 RVA: 0x0006E199 File Offset: 0x0006C399
		private Material depthMaterial
		{
			get
			{
				if (this._depthMaterial == null)
				{
					this._depthMaterial = new Material(this.depthShader);
					this._depthMaterial.hideFlags = HideFlags.HideAndDontSave;
				}
				return this._depthMaterial;
			}
		}

		// Token: 0x170002CD RID: 717
		// (get) Token: 0x06001098 RID: 4248 RVA: 0x0006E1CD File Offset: 0x0006C3CD
		private Material mergeMaterial
		{
			get
			{
				if (this._mergeMaterial == null)
				{
					this._mergeMaterial = new Material(this.mergeShader);
					this._mergeMaterial.hideFlags = HideFlags.HideAndDontSave;
				}
				return this._mergeMaterial;
			}
		}

		// Token: 0x170002CE RID: 718
		// (get) Token: 0x06001099 RID: 4249 RVA: 0x0006E201 File Offset: 0x0006C401
		private Material postProcMaterial
		{
			get
			{
				if (this._postProcMaterial == null)
				{
					this._postProcMaterial = new Material(this.postProcShader);
					this._postProcMaterial.hideFlags = HideFlags.HideAndDontSave;
				}
				return this._postProcMaterial;
			}
		}

		// Token: 0x170002CF RID: 719
		// (get) Token: 0x0600109A RID: 4250 RVA: 0x0006E235 File Offset: 0x0006C435
		private Material maskEstimatorMaterial
		{
			get
			{
				if (this._maskEstimatorMaterial == null)
				{
					this._maskEstimatorMaterial = new Material(this.maskEstimatorShader);
					this._maskEstimatorMaterial.hideFlags = HideFlags.HideAndDontSave;
				}
				return this._maskEstimatorMaterial;
			}
		}

		// Token: 0x170002D0 RID: 720
		// (get) Token: 0x0600109B RID: 4251 RVA: 0x0006E269 File Offset: 0x0006C469
		private Material normalMaterial
		{
			get
			{
				if (this._normalMaterial == null)
				{
					this._normalMaterial = new Material(this.normalShader);
					this._normalMaterial.hideFlags = HideFlags.HideAndDontSave;
				}
				return this._normalMaterial;
			}
		}

		// Token: 0x0600109C RID: 4252 RVA: 0x0006E2A0 File Offset: 0x0006C4A0
		public RuntimeBakingEngine(List<Vector3> povs, List<Mesh> meshes, List<Material[]> materials, List<Matrix4x4> meshesTransforms, int textureSize, float orthographicSize, LightingMethod lightingMethod = LightingMethod.SurfaceEstimation)
		{
			this.RuntimeRenderer = new RuntimeRendererUtility();
			this.camera = this.RuntimeRenderer.camera;
			this.camera.orthographic = true;
			this.camera.nearClipPlane = 0.01f;
			this.camera.farClipPlane = orthographicSize * 1.4142f * 2f;
			this.camera.orthographicSize = orthographicSize;
			Light[] lights = this.RuntimeRenderer.lights;
			for (int i = 0; i < lights.Length; i++)
			{
				lights[i].gameObject.SetActive(false);
			}
			this.camera.backgroundColor = Color.black;
			this.camera.clearFlags = CameraClearFlags.Color;
			this.camera.cameraType = CameraType.Reflection;
			switch (lightingMethod)
			{
			case LightingMethod.SurfaceEstimation:
				this.RuntimeRenderer.ambientColor = Color.white;
				break;
			case LightingMethod.UseSunSource:
				if (RenderSettings.sun != null)
				{
					this.RuntimeRenderer.ambientColor = RenderSettings.ambientLight;
					this.RuntimeRenderer.lights[0].gameObject.SetActive(true);
					this.RuntimeRenderer.lights[0] = RenderSettings.sun;
					this.RuntimeRenderer.lights[0].transform.rotation = RenderSettings.sun.transform.rotation;
				}
				else
				{
					this.RuntimeRenderer.ambientColor = Color.white;
					Debug.LogWarning("[Mirage Pro] Lighting settings: Sun Source is null. Output Impostor will appear unlit.");
				}
				break;
			case LightingMethod.ForwardLighting:
			{
				this.RuntimeRenderer.ambientColor = Color.white;
				Light[] array = new Light[4];
				for (int j = 0; j < array.Length; j++)
				{
					GameObject gameObject = new GameObject();
					this.RuntimeRenderer.AddSingleGO(gameObject);
					Light light = gameObject.AddComponent<Light>();
					gameObject.SetActive(true);
					light.type = LightType.Directional;
					light.intensity = 1f / (float)array.Length;
					light.range = this.RuntimeRenderer.camera.farClipPlane;
					light.transform.parent = this.RuntimeRenderer.camera.transform;
					light.transform.localPosition = new Vector3(Mathf.Cos((float)(j * 2) * 3.1415927f / (float)array.Length), Mathf.Sin((float)(j * 2) * 3.1415927f / (float)array.Length), -orthographicSize) * orthographicSize / 10f;
					light.transform.LookAt(this.RuntimeRenderer.camera.transform.position);
					light.color = Color.white;
					array[j] = light;
				}
				break;
			}
			}
			base.Initialize(povs, meshes, materials, meshesTransforms, textureSize);
		}

		// Token: 0x0600109D RID: 4253 RVA: 0x0006E548 File Offset: 0x0006C748
		~RuntimeBakingEngine()
		{
		}

		// Token: 0x0600109E RID: 4254 RVA: 0x0006E570 File Offset: 0x0006C770
		public override Texture2D ComputeColorMaps()
		{
			int num = Mathf.CeilToInt(Mathf.Sqrt((float)base.CameraPositions.Count));
			int num2 = Mathf.FloorToInt((float)base.TextureSize / (float)num);
			AmbientMode ambientMode = RenderSettings.ambientMode;
			Color ambientLight = RenderSettings.ambientLight;
			DefaultReflectionMode defaultReflectionMode = RenderSettings.defaultReflectionMode;
			float reflectionIntensity = RenderSettings.reflectionIntensity;
			Texture customReflectionTexture = RenderSettings.customReflectionTexture;
			RenderSettings.customReflectionTexture = Resources.Load<Texture>("MirageWhiteBakingCubeMap");
			RenderSettings.ambientMode = AmbientMode.Flat;
			RenderSettings.ambientLight = this.RuntimeRenderer.ambientColor;
			RenderSettings.defaultReflectionMode = DefaultReflectionMode.Custom;
			RenderSettings.reflectionIntensity = 1f;
			this.camera.backgroundColor = Color.gray;
			Texture2D texture2D = new Texture2D(num2 * num, num2 * num, TextureFormat.ARGB32, false);
			Color[] array = new Color[texture2D.width * texture2D.height];
			texture2D.SetPixels(array);
			Texture2D texture2D2 = new Texture2D(num2 * num, num2 * num, TextureFormat.ARGB32, false);
			for (int i = 0; i < base.CameraPositions.Count; i++)
			{
				this.camera.transform.position = base.CameraPositions[i];
				this.camera.transform.LookAt(Vector3.zero);
				for (int j = 0; j < base.Meshes.Count; j++)
				{
					Mesh mesh = base.Meshes[j];
					for (int k = 0; k < base.Materials[j].Length; k++)
					{
						Material material = new Material(base.Materials[j][k]);
						material.DisableKeyword("_GLOSSYREFLECTIONS_OFF");
						this.RuntimeRenderer.DrawMesh(mesh, base.MeshesTransforms[j], material, (k < mesh.subMeshCount) ? k : (mesh.subMeshCount - 1), null, null, false);
					}
				}
				this.camera.backgroundColor = Color.gray;
				Rect rect = new Rect(0f, 0f, (float)num2, (float)num2);
				this.RuntimeRenderer.BeginStaticPreview(rect);
				this.RuntimeRenderer.Render(true);
				Texture2D texture2D3 = this.RuntimeRenderer.EndStaticPreview();
				for (int l = 0; l < base.Meshes.Count; l++)
				{
					Mesh mesh2 = base.Meshes[l];
					for (int m = 0; m < base.Materials[l].Length; m++)
					{
						Material material2 = new Material(base.Materials[l][m]);
						material2.DisableKeyword("_GLOSSYREFLECTIONS_OFF");
						this.RuntimeRenderer.DrawMesh(mesh2, base.MeshesTransforms[l], material2, (m < mesh2.subMeshCount) ? m : (mesh2.subMeshCount - 1), null, null, false);
					}
				}
				this.camera.backgroundColor = Color.black;
				this.RuntimeRenderer.BeginStaticPreview(rect);
				this.RuntimeRenderer.Render(true);
				Texture2D texture2D4 = this.RuntimeRenderer.EndStaticPreview();
				int num3 = i % num;
				int num4 = Mathf.FloorToInt((float)i / (float)num);
				texture2D.SetPixels(num3 * num2, num4 * num2, num2, num2, texture2D3.GetPixels());
				texture2D2.SetPixels(num3 * num2, num4 * num2, num2, num2, texture2D4.GetPixels());
			}
			texture2D.Apply();
			texture2D = ImpostorTextureUtilities.ResizeSquared(texture2D, Mathf.ClosestPowerOfTwo(num2 * num));
			texture2D2.Apply();
			texture2D2 = ImpostorTextureUtilities.ResizeSquared(texture2D2, Mathf.ClosestPowerOfTwo(num2 * num));
			RenderSettings.ambientMode = ambientMode;
			RenderSettings.ambientLight = ambientLight;
			RenderSettings.customReflectionTexture = customReflectionTexture;
			RenderSettings.defaultReflectionMode = defaultReflectionMode;
			RenderSettings.reflectionIntensity = reflectionIntensity;
			this.camera.backgroundColor = Color.black;
			Texture2D texture2D5 = new Texture2D(num2 * num, num2 * num, TextureFormat.ARGB32, false);
			for (int n = 0; n < base.CameraPositions.Count; n++)
			{
				this.camera.transform.position = base.CameraPositions[n];
				this.camera.transform.LookAt(Vector3.zero);
				for (int num5 = 0; num5 < base.Meshes.Count; num5++)
				{
					Mesh mesh3 = base.Meshes[num5];
					for (int num6 = 0; num6 < mesh3.subMeshCount; num6++)
					{
						this.RuntimeRenderer.DrawMesh(mesh3, base.MeshesTransforms[num5], this.depthMaterial, num6, null, null, false);
					}
				}
				Rect rect2 = new Rect(0f, 0f, (float)num2, (float)num2);
				this.RuntimeRenderer.BeginStaticPreview(rect2);
				this.RuntimeRenderer.Render(true);
				Texture2D texture2D6 = this.RuntimeRenderer.EndStaticPreview();
				int num7 = n % num;
				int num8 = Mathf.FloorToInt((float)n / (float)num);
				texture2D5.SetPixels(num7 * num2, num8 * num2, num2, num2, texture2D6.GetPixels());
			}
			texture2D5.Apply();
			texture2D5 = ImpostorTextureUtilities.ResizeSquared(texture2D5, Mathf.ClosestPowerOfTwo(num2 * num));
			RenderTexture renderTexture = new RenderTexture(texture2D.width, texture2D.height, 24, RenderTextureFormat.ARGB32);
			renderTexture.enableRandomWrite = true;
			renderTexture.Create();
			this.mergeMaterial.SetTexture("_DepthTex", texture2D5);
			this.mergeMaterial.SetTexture("_AlphaClippingDiscriminatorTex", texture2D2);
			Graphics.Blit(texture2D, renderTexture, this.mergeMaterial);
			RenderTexture temporary = RenderTexture.GetTemporary(renderTexture.width, renderTexture.height, 0, renderTexture.format);
			this.postProcMaterial.SetTexture("_MainTex", renderTexture);
			this.postProcMaterial.SetTexture("_DepthTex", renderTexture);
			Graphics.Blit(renderTexture, temporary, this.postProcMaterial);
			Graphics.Blit(temporary, renderTexture);
			RenderTexture.ReleaseTemporary(temporary);
			RenderTexture.active = renderTexture;
			texture2D.ReadPixels(new Rect(0f, 0f, (float)renderTexture.width, (float)renderTexture.height), 0, 0);
			texture2D.Apply();
			RenderTexture.active = null;
			renderTexture.Release();
			return texture2D;
		}

		// Token: 0x0600109F RID: 4255 RVA: 0x0006EB50 File Offset: 0x0006CD50
		public override Texture2D ComputeNormalMaps()
		{
			int num = Mathf.CeilToInt(Mathf.Sqrt((float)base.CameraPositions.Count));
			int num2 = Mathf.FloorToInt((float)base.TextureSize / (float)num);
			Texture2D texture2D = new Texture2D(num2 * num, num2 * num, TextureFormat.RGB24, false);
			this.camera.backgroundColor = new Color(0.5f, 0.5f, 1f);
			for (int i = 0; i < base.CameraPositions.Count; i++)
			{
				this.camera.transform.position = base.CameraPositions[i];
				this.camera.transform.LookAt(Vector3.zero);
				for (int j = 0; j < base.Meshes.Count; j++)
				{
					Mesh mesh = base.Meshes[j];
					for (int k = 0; k < mesh.subMeshCount; k++)
					{
						this.RuntimeRenderer.DrawMesh(mesh, base.MeshesTransforms[j], this.normalMaterial, k, null, null, false);
					}
				}
				Rect rect = new Rect(0f, 0f, (float)num2, (float)num2);
				this.RuntimeRenderer.BeginStaticPreview(rect);
				this.RuntimeRenderer.Render(true);
				Texture2D texture2D2 = this.RuntimeRenderer.EndStaticPreview();
				int num3 = i % num;
				int num4 = Mathf.FloorToInt((float)i / (float)num);
				texture2D.SetPixels(num3 * num2, num4 * num2, num2, num2, texture2D2.GetPixels());
			}
			texture2D.Apply();
			return ImpostorTextureUtilities.ResizeSquared(texture2D, Mathf.ClosestPowerOfTwo(num2 * num));
		}

		// Token: 0x060010A0 RID: 4256 RVA: 0x0006ECDC File Offset: 0x0006CEDC
		public override Texture2D ComputeMaskMaps()
		{
			int num = Mathf.CeilToInt(Mathf.Sqrt((float)base.CameraPositions.Count));
			int num2 = Mathf.FloorToInt((float)base.TextureSize / (float)num);
			AmbientMode ambientMode = RenderSettings.ambientMode;
			Color ambientLight = RenderSettings.ambientLight;
			DefaultReflectionMode defaultReflectionMode = RenderSettings.defaultReflectionMode;
			float reflectionIntensity = RenderSettings.reflectionIntensity;
			RenderSettings.ambientMode = AmbientMode.Flat;
			RenderSettings.ambientLight = Color.white;
			RenderSettings.defaultReflectionMode = DefaultReflectionMode.Custom;
			this.camera.backgroundColor = Color.gray;
			Texture2D texture2D = new Texture2D(num2 * num, num2 * num, TextureFormat.RGB24, false);
			for (int i = 0; i < base.CameraPositions.Count; i++)
			{
				this.camera.transform.position = base.CameraPositions[i];
				this.camera.transform.LookAt(Vector3.zero);
				Rect rect = new Rect(0f, 0f, (float)num2, (float)num2);
				for (int j = 0; j < base.Meshes.Count; j++)
				{
					Mesh mesh = base.Meshes[j];
					for (int k = 0; k < base.Materials[j].Length; k++)
					{
						Material material = new Material(base.Materials[j][k]);
						material.DisableKeyword("_GLOSSYREFLECTIONS_OFF");
						this.RuntimeRenderer.DrawMesh(mesh, base.MeshesTransforms[j], material, (k < mesh.subMeshCount) ? k : (mesh.subMeshCount - 1), null, null, false);
					}
				}
				this.RuntimeRenderer.BeginStaticPreview(rect);
				Texture customReflectionTexture = RenderSettings.customReflectionTexture;
				RenderSettings.customReflectionTexture = Resources.Load<Texture>("MirageWhiteBakingCubeMap");
				this.RuntimeRenderer.Render(true);
				Texture2D texture2D2 = this.RuntimeRenderer.EndStaticPreview();
				RenderSettings.customReflectionTexture = customReflectionTexture;
				int num3 = i % num;
				int num4 = Mathf.FloorToInt((float)i / (float)num);
				texture2D.SetPixels(num3 * num2, num4 * num2, num2, num2, texture2D2.GetPixels());
			}
			texture2D.Apply();
			texture2D = ImpostorTextureUtilities.ResizeSquared(texture2D, Mathf.ClosestPowerOfTwo(num2 * num));
			Texture2D texture2D3 = new Texture2D(num2 * num, num2 * num, TextureFormat.RGB24, false);
			for (int l = 0; l < base.CameraPositions.Count; l++)
			{
				this.camera.transform.position = base.CameraPositions[l];
				this.camera.transform.LookAt(Vector3.zero);
				Rect rect2 = new Rect(0f, 0f, (float)num2, (float)num2);
				for (int m = 0; m < base.Meshes.Count; m++)
				{
					Mesh mesh2 = base.Meshes[m];
					for (int n = 0; n < base.Materials[m].Length; n++)
					{
						Material material2 = new Material(base.Materials[m][n]);
						material2.DisableKeyword("_GLOSSYREFLECTIONS_OFF");
						this.RuntimeRenderer.DrawMesh(mesh2, base.MeshesTransforms[m], material2, (n < mesh2.subMeshCount) ? n : (mesh2.subMeshCount - 1), null, null, false);
					}
				}
				this.RuntimeRenderer.BeginStaticPreview(rect2);
				Texture customReflectionTexture2 = RenderSettings.customReflectionTexture;
				RenderSettings.customReflectionTexture = Resources.Load<Texture>("MirageBlackBakingCubeMap");
				this.RuntimeRenderer.Render(true);
				Texture2D texture2D4 = this.RuntimeRenderer.EndStaticPreview();
				RenderSettings.customReflectionTexture = customReflectionTexture2;
				int num5 = l % num;
				int num6 = Mathf.FloorToInt((float)l / (float)num);
				texture2D3.SetPixels(num5 * num2, num6 * num2, num2, num2, texture2D4.GetPixels());
			}
			texture2D3.Apply();
			texture2D3 = ImpostorTextureUtilities.ResizeSquared(texture2D3, Mathf.ClosestPowerOfTwo(num2 * num));
			RenderSettings.ambientMode = ambientMode;
			RenderSettings.ambientLight = ambientLight;
			RenderSettings.defaultReflectionMode = defaultReflectionMode;
			RenderSettings.reflectionIntensity = reflectionIntensity;
			this.RuntimeRenderer.ambientColor = Color.white;
			RenderTexture renderTexture = new RenderTexture(texture2D.width, texture2D.height, 24, RenderTextureFormat.ARGB32);
			renderTexture.enableRandomWrite = true;
			renderTexture.Create();
			this.maskEstimatorMaterial.SetTexture("_TexER0", texture2D3);
			Graphics.Blit(texture2D, renderTexture, this.maskEstimatorMaterial);
			RenderTexture.active = renderTexture;
			Texture2D texture2D5 = new Texture2D(renderTexture.width, renderTexture.height, TextureFormat.RGB24, false);
			texture2D5.ReadPixels(new Rect(0f, 0f, (float)renderTexture.width, (float)renderTexture.height), 0, 0);
			texture2D5.Apply();
			RenderTexture.active = null;
			renderTexture.Release();
			return texture2D5;
		}

		// Token: 0x060010A1 RID: 4257 RVA: 0x0006F158 File Offset: 0x0006D358
		public override void ApplyPostProcessing(ref Texture2D map, Texture2D colorDepthMap)
		{
			RenderTexture temporary = RenderTexture.GetTemporary(map.width, map.height, 0, RenderTextureFormat.ARGB32);
			this.postProcMaterial.SetTexture("_MainTex", map);
			this.postProcMaterial.SetTexture("_DepthTex", colorDepthMap);
			Graphics.Blit(map, temporary, this.postProcMaterial);
			RenderTexture.active = temporary;
			map.ReadPixels(new Rect(0f, 0f, (float)map.width, (float)map.height), 0, 0);
			map.Apply();
			RenderTexture.ReleaseTemporary(temporary);
		}

		// Token: 0x060010A2 RID: 4258 RVA: 0x0006F1E7 File Offset: 0x0006D3E7
		public override void Cleanup()
		{
			this.RuntimeRenderer.Cleanup();
		}

		// Token: 0x0400120A RID: 4618
		private Camera camera;

		// Token: 0x0400120B RID: 4619
		private Shader _depthShader;

		// Token: 0x0400120C RID: 4620
		private Shader _mergeShader;

		// Token: 0x0400120D RID: 4621
		private Shader _postProcShader;

		// Token: 0x0400120E RID: 4622
		private Shader _maskEstimatorShader;

		// Token: 0x0400120F RID: 4623
		private Shader _normalShader;

		// Token: 0x04001210 RID: 4624
		private Material _depthMaterial;

		// Token: 0x04001211 RID: 4625
		private Material _mergeMaterial;

		// Token: 0x04001212 RID: 4626
		private Material _postProcMaterial;

		// Token: 0x04001213 RID: 4627
		private Material _maskEstimatorMaterial;

		// Token: 0x04001214 RID: 4628
		private Material _normalMaterial;
	}
}
