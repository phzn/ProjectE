using System;
using UnityEngine;

namespace GPUInstancerPro.TerrainModule
{
	// Token: 0x0200001C RID: 28
	public static class GPUITerrainConstants
	{
		// Token: 0x06000152 RID: 338 RVA: 0x00009702 File Offset: 0x00007902
		public static string GetPackagesPath()
		{
			if (string.IsNullOrEmpty(GPUITerrainConstants._packagesPath))
			{
				GPUITerrainConstants._packagesPath = "Packages/com.gurbu.gpui-pro.terrain/";
			}
			return GPUITerrainConstants._packagesPath;
		}

		// Token: 0x17000009 RID: 9
		// (get) Token: 0x06000153 RID: 339 RVA: 0x0000971F File Offset: 0x0000791F
		public static GPUIProfile DefaultDetailTextureProfile
		{
			get
			{
				if (GPUITerrainConstants._defaultDetailTextureProfile == null)
				{
					GPUITerrainConstants._defaultDetailTextureProfile = ScriptableObject.CreateInstance<GPUIProfile>();
					GPUITerrainConstants._defaultDetailTextureProfile.isShadowCasting = false;
					GPUITerrainConstants._defaultDetailTextureProfile.isDistanceCulling = false;
					GPUITerrainConstants._defaultDetailTextureProfile.isDefaultProfile = true;
				}
				return GPUITerrainConstants._defaultDetailTextureProfile;
			}
		}

		// Token: 0x1700000A RID: 10
		// (get) Token: 0x06000154 RID: 340 RVA: 0x00009760 File Offset: 0x00007960
		public static GPUIProfile DefaultDetailPrefabProfile
		{
			get
			{
				if (GPUITerrainConstants._defaultDetailPrefabProfile == null)
				{
					GPUITerrainConstants._defaultDetailPrefabProfile = ScriptableObject.CreateInstance<GPUIProfile>();
					GPUITerrainConstants._defaultDetailPrefabProfile.isShadowCasting = true;
					GPUITerrainConstants._defaultDetailPrefabProfile.isShadowFrustumCulling = true;
					GPUITerrainConstants._defaultDetailPrefabProfile.isShadowOcclusionCulling = true;
					GPUITerrainConstants._defaultDetailPrefabProfile.isDistanceCulling = false;
					GPUITerrainConstants._defaultDetailPrefabProfile.isDefaultProfile = true;
				}
				return GPUITerrainConstants._defaultDetailPrefabProfile;
			}
		}

		// Token: 0x1700000B RID: 11
		// (get) Token: 0x06000155 RID: 341 RVA: 0x000097C0 File Offset: 0x000079C0
		public static GPUIProfile DefaultTreeProfile
		{
			get
			{
				if (GPUITerrainConstants._defaultTreeProfile == null)
				{
					GPUITerrainConstants._defaultTreeProfile = ScriptableObject.CreateInstance<GPUIProfile>();
					GPUITerrainConstants._defaultTreeProfile.isShadowCasting = true;
					GPUITerrainConstants._defaultTreeProfile.isLODCrossFade = true;
					GPUITerrainConstants._defaultTreeProfile.isDistanceCulling = false;
					GPUITerrainConstants._defaultTreeProfile.isDefaultProfile = true;
				}
				return GPUITerrainConstants._defaultTreeProfile;
			}
		}

		// Token: 0x1700000C RID: 12
		// (get) Token: 0x06000156 RID: 342 RVA: 0x00009815 File Offset: 0x00007A15
		// (set) Token: 0x06000157 RID: 343 RVA: 0x00009839 File Offset: 0x00007A39
		public static Mesh DefaultDetailMesh
		{
			get
			{
				if (GPUITerrainConstants._defaultDetailMesh == null)
				{
					GPUITerrainConstants._defaultDetailMesh = GPUITerrainUtility.CreateCrossQuadsMesh("DefaultDetailMesh", 1);
				}
				return GPUITerrainConstants._defaultDetailMesh;
			}
			set
			{
				if (value != null)
				{
					GPUITerrainConstants._defaultDetailMesh = value;
				}
			}
		}

		// Token: 0x1700000D RID: 13
		// (get) Token: 0x06000158 RID: 344 RVA: 0x0000984C File Offset: 0x00007A4C
		public static Material DefaultDetailMaterial
		{
			get
			{
				if (GPUITerrainConstants._defaultDetailMaterial == null)
				{
					Shader defaultDetailShader = GPUITerrainConstants.GetDefaultDetailShader();
					if (defaultDetailShader == null)
					{
						return new Material(GPUITerrainConstants.GetDefaultDetailShaderFallback());
					}
					GPUITerrainConstants._defaultDetailMaterial = new Material(defaultDetailShader);
					if (GPUITerrainConstants.DefaultHealthyDryNoiseTexture != null)
					{
						GPUITerrainConstants._defaultDetailMaterial.SetTexture("_HealthyDryNoiseTexture", GPUITerrainConstants.DefaultHealthyDryNoiseTexture);
					}
					if (GPUITerrainConstants.DefaultNoiseNormal != null)
					{
						GPUITerrainConstants._defaultDetailMaterial.SetTexture("_WindWaveNormalTexture", GPUITerrainConstants.DefaultNoiseNormal);
					}
					GPUITerrainConstants._defaultDetailMaterial.SetFloat("_WindWavesOn", 1f);
					if (QualitySettings.billboardsFaceCameraPosition)
					{
						GPUITerrainConstants._defaultDetailMaterial.EnableKeyword("BILLBOARD_FACE_CAMERA_POS");
					}
					else
					{
						GPUITerrainConstants._defaultDetailMaterial.DisableKeyword("BILLBOARD_FACE_CAMERA_POS");
					}
				}
				return GPUITerrainConstants._defaultDetailMaterial;
			}
		}

		// Token: 0x06000159 RID: 345 RVA: 0x00009910 File Offset: 0x00007B10
		public static Shader GetDefaultDetailShader()
		{
			GPUIRenderPipeline renderPipeline = GPUIRuntimeSettings.Instance.RenderPipeline;
			if (renderPipeline == GPUIRenderPipeline.URP)
			{
				return GPUIUtility.FindShader(GPUITerrainConstants.SHADER_DEFAULT_DETAIL_URP);
			}
			if (renderPipeline != GPUIRenderPipeline.HDRP)
			{
				return GPUIUtility.FindShader(GPUITerrainConstants.SHADER_DEFAULT_DETAIL_Builtin);
			}
			return GPUIUtility.FindShader(GPUITerrainConstants.SHADER_DEFAULT_DETAIL_HDRP);
		}

		// Token: 0x0600015A RID: 346 RVA: 0x0000995C File Offset: 0x00007B5C
		public static Shader GetDefaultDetailShaderFallback()
		{
			GPUIRenderPipeline renderPipeline = GPUIRuntimeSettings.Instance.RenderPipeline;
			if (renderPipeline == GPUIRenderPipeline.URP)
			{
				return GPUIUtility.FindShader("Universal Render Pipeline/Lit");
			}
			if (renderPipeline != GPUIRenderPipeline.HDRP)
			{
				return GPUIUtility.FindShader("Standard");
			}
			return GPUIUtility.FindShader("HDRP/Lit");
		}

		// Token: 0x0600015B RID: 347 RVA: 0x000099A6 File Offset: 0x00007BA6
		public static bool IsDefaultDetailShader(Shader shader)
		{
			return shader != null && shader.name.StartsWith(GPUITerrainConstants.SHADER_DEFAULT_DETAIL_Builtin);
		}

		// Token: 0x1700000E RID: 14
		// (get) Token: 0x0600015C RID: 348 RVA: 0x000099C3 File Offset: 0x00007BC3
		public static GPUIDetailMaterialDescription DefaultDetailMaterialDescription
		{
			get
			{
				if (GPUITerrainConstants._defaultDetailMaterialDescription == null)
				{
					GPUITerrainConstants._defaultDetailMaterialDescription = ScriptableObject.CreateInstance<GPUIDetailMaterialDescription>();
					GPUITerrainConstants._defaultDetailMaterialDescription.SetDefaultValues();
				}
				return GPUITerrainConstants._defaultDetailMaterialDescription;
			}
		}

		// Token: 0x1700000F RID: 15
		// (get) Token: 0x0600015D RID: 349 RVA: 0x000099EB File Offset: 0x00007BEB
		public static Texture2D DefaultHealthyDryNoiseTexture
		{
			get
			{
				return GPUITerrainConstants._defaultHealthyDryNoiseTexture;
			}
		}

		// Token: 0x17000010 RID: 16
		// (get) Token: 0x0600015E RID: 350 RVA: 0x000099F2 File Offset: 0x00007BF2
		public static Texture2D DefaultNoiseNormal
		{
			get
			{
				return GPUITerrainConstants._defaultNoiseNormal;
			}
		}

		// Token: 0x17000011 RID: 17
		// (get) Token: 0x0600015F RID: 351 RVA: 0x000099F9 File Offset: 0x00007BF9
		public static ComputeShader CS_TerrainDetailCapture
		{
			get
			{
				if (GPUITerrainConstants._CS_TerrainDetailCapture == null)
				{
					GPUITerrainConstants._CS_TerrainDetailCapture = GPUIUtility.LoadResource<ComputeShader>(GPUITerrainConstants.FILE_CS_TerrainDetailCapture);
				}
				return GPUITerrainConstants._CS_TerrainDetailCapture;
			}
		}

		// Token: 0x17000012 RID: 18
		// (get) Token: 0x06000160 RID: 352 RVA: 0x00009A1C File Offset: 0x00007C1C
		public static ComputeShader CS_TerrainDetailCaptureFromInstanceTransforms
		{
			get
			{
				if (GPUITerrainConstants._CS_TerrainDetailCaptureFromInstanceTransforms == null)
				{
					GPUITerrainConstants._CS_TerrainDetailCaptureFromInstanceTransforms = GPUIUtility.LoadResource<ComputeShader>(GPUITerrainConstants.FILE_CS_TerrainDetailCaptureFromInstanceTransforms);
				}
				return GPUITerrainConstants._CS_TerrainDetailCaptureFromInstanceTransforms;
			}
		}

		// Token: 0x17000013 RID: 19
		// (get) Token: 0x06000161 RID: 353 RVA: 0x00009A3F File Offset: 0x00007C3F
		public static ComputeShader CS_VegetationGenerator
		{
			get
			{
				if (GPUITerrainConstants._CS_VegetationGenerator == null)
				{
					GPUITerrainConstants._CS_VegetationGenerator = GPUIUtility.LoadResource<ComputeShader>(GPUITerrainConstants.FILE_CS_VegetationGenerator);
				}
				return GPUITerrainConstants._CS_VegetationGenerator;
			}
		}

		// Token: 0x17000014 RID: 20
		// (get) Token: 0x06000162 RID: 354 RVA: 0x00009A62 File Offset: 0x00007C62
		public static ComputeShader CS_TerrainTreeGenerator
		{
			get
			{
				if (GPUITerrainConstants._CS_TerrainTreeGenerator == null)
				{
					GPUITerrainConstants._CS_TerrainTreeGenerator = GPUIUtility.LoadResource<ComputeShader>(GPUITerrainConstants.FILE_CS_TerrainTreeGenerator);
				}
				return GPUITerrainConstants._CS_TerrainTreeGenerator;
			}
		}

		// Token: 0x17000015 RID: 21
		// (get) Token: 0x06000163 RID: 355 RVA: 0x00009A85 File Offset: 0x00007C85
		public static ComputeShader CS_TerrainTreeGeneratorBatched
		{
			get
			{
				if (GPUITerrainConstants._CS_TerrainTreeGeneratorBatched == null)
				{
					GPUITerrainConstants._CS_TerrainTreeGeneratorBatched = GPUIUtility.LoadResource<ComputeShader>(GPUITerrainConstants.FILE_CS_TerrainTreeGeneratorBatched);
				}
				return GPUITerrainConstants._CS_TerrainTreeGeneratorBatched;
			}
		}

		// Token: 0x17000016 RID: 22
		// (get) Token: 0x06000164 RID: 356 RVA: 0x00009AA8 File Offset: 0x00007CA8
		public static ComputeShader CS_TerrainDetailDensityModifier
		{
			get
			{
				if (GPUITerrainConstants._CS_TerrainDetailDensityModifier == null)
				{
					GPUITerrainConstants._CS_TerrainDetailDensityModifier = GPUIUtility.LoadResource<ComputeShader>(GPUITerrainConstants.FILE_CS_TerrainDetailDensityModifier);
				}
				return GPUITerrainConstants._CS_TerrainDetailDensityModifier;
			}
		}

		// Token: 0x17000017 RID: 23
		// (get) Token: 0x06000165 RID: 357 RVA: 0x00009ACB File Offset: 0x00007CCB
		public static RenderTextureFormat R8_RenderTextureFormat
		{
			get
			{
				if (!GPUIRuntimeSettings.Instance.API_HAS_GUARANTEED_R8_SUPPORT)
				{
					return RenderTextureFormat.RFloat;
				}
				return RenderTextureFormat.R8;
			}
		}

		// Token: 0x17000018 RID: 24
		// (get) Token: 0x06000166 RID: 358 RVA: 0x00009ADE File Offset: 0x00007CDE
		public static RenderTextureFormat R16_RenderTextureFormat
		{
			get
			{
				if (!GPUIRuntimeSettings.Instance.API_HAS_GUARANTEED_R8_SUPPORT)
				{
					return RenderTextureFormat.RFloat;
				}
				return RenderTextureFormat.R16;
			}
		}

		// Token: 0x040000F0 RID: 240
		public const string FILE_DEFAULT_DETAIL_TEXTURE_PROFILE = "GPUIDefaultDetailTextureProfile";

		// Token: 0x040000F1 RID: 241
		public const string FILE_DEFAULT_DETAIL_PREFAB_PROFILE = "GPUIDefaultDetailPrefabProfile";

		// Token: 0x040000F2 RID: 242
		public const string FILE_DEFAULT_TREE_PROFILE = "GPUIDefaultTreeProfile";

		// Token: 0x040000F3 RID: 243
		public const string FILE_DEFAULT_DETAIL_MESH = "DefaultDetailMesh";

		// Token: 0x040000F4 RID: 244
		public const string FILE_DEFAULT_DETAIL_MATERIAL = "DefaultDetailMaterial";

		// Token: 0x040000F5 RID: 245
		public const string FILE_DEFAULT_DETAIL_MATERIAL_DESC = "DefaultDetailMaterialDesc";

		// Token: 0x040000F6 RID: 246
		public const string NAME_SUFFIX_DETAILTEXTURE = "_GPUIDetailLayer_";

		// Token: 0x040000F7 RID: 247
		public const string FOLDER_BAKED_DETAIL_TEXTURES = "GPUIDetailTextures";

		// Token: 0x040000F8 RID: 248
		private static string _packagesPath;

		// Token: 0x040000F9 RID: 249
		public static readonly string SHADER_DEFAULT_DETAIL_Builtin = "GPUInstancerPro/Foliage";

		// Token: 0x040000FA RID: 250
		public static readonly string SHADER_DEFAULT_DETAIL_Lambert_Builtin = "GPUInstancerPro/FoliageLambert";

		// Token: 0x040000FB RID: 251
		public static readonly string SHADER_DEFAULT_DETAIL_URP = "GPUInstancerPro/Foliage_SG";

		// Token: 0x040000FC RID: 252
		public static readonly string SHADER_DEFAULT_DETAIL_HDRP = "GPUInstancerPro/Foliage_SG";

		// Token: 0x040000FD RID: 253
		private static GPUIProfile _defaultDetailTextureProfile;

		// Token: 0x040000FE RID: 254
		private static GPUIProfile _defaultDetailPrefabProfile;

		// Token: 0x040000FF RID: 255
		private static GPUIProfile _defaultTreeProfile;

		// Token: 0x04000100 RID: 256
		private static Mesh _defaultDetailMesh;

		// Token: 0x04000101 RID: 257
		private static Material _defaultDetailMaterial;

		// Token: 0x04000102 RID: 258
		private static GPUIDetailMaterialDescription _defaultDetailMaterialDescription;

		// Token: 0x04000103 RID: 259
		private static Texture2D _defaultHealthyDryNoiseTexture;

		// Token: 0x04000104 RID: 260
		private static Texture2D _defaultNoiseNormal;

		// Token: 0x04000105 RID: 261
		public static readonly string FILE_CS_TerrainDetailCapture = "GPUITerrainDetailCaptureCS";

		// Token: 0x04000106 RID: 262
		private static ComputeShader _CS_TerrainDetailCapture;

		// Token: 0x04000107 RID: 263
		public static readonly string FILE_CS_TerrainDetailCaptureFromInstanceTransforms = "GPUITerrainDetailCaptureFromInstanceTransformsCS";

		// Token: 0x04000108 RID: 264
		private static ComputeShader _CS_TerrainDetailCaptureFromInstanceTransforms;

		// Token: 0x04000109 RID: 265
		public static readonly string FILE_CS_VegetationGenerator = "GPUIVegetationGeneratorCS";

		// Token: 0x0400010A RID: 266
		private static ComputeShader _CS_VegetationGenerator;

		// Token: 0x0400010B RID: 267
		public static readonly string Kw_GPUI_DETAIL_DENSITY_REDUCE = "GPUI_DETAIL_DENSITY_REDUCE";

		// Token: 0x0400010C RID: 268
		public static readonly string Kw_GPUI_TERRAIN_HOLES = "GPUI_TERRAIN_HOLES";

		// Token: 0x0400010D RID: 269
		public static readonly string Kw_GPUI_TWO_CHANNEL_HEIGHTMAP = "GPUI_TWO_CHANNEL_HEIGHTMAP";

		// Token: 0x0400010E RID: 270
		public static readonly string FILE_CS_TerrainTreeGenerator = "GPUITerrainTreeGeneratorCS";

		// Token: 0x0400010F RID: 271
		private static ComputeShader _CS_TerrainTreeGenerator;

		// Token: 0x04000110 RID: 272
		public static readonly string Kw_GPUI_TREE_INSTANCE_COLOR = "GPUI_TREE_INSTANCE_COLOR";

		// Token: 0x04000111 RID: 273
		public static readonly string FILE_CS_TerrainTreeGeneratorBatched = "GPUITerrainTreeGeneratorBatchedCS";

		// Token: 0x04000112 RID: 274
		private static ComputeShader _CS_TerrainTreeGeneratorBatched;

		// Token: 0x04000113 RID: 275
		public static readonly string FILE_CS_TerrainDetailDensityModifier = "GPUITerrainDetailDensityModifierCS";

		// Token: 0x04000114 RID: 276
		private static ComputeShader _CS_TerrainDetailDensityModifier;

		// Token: 0x04000115 RID: 277
		public static readonly int PROP_terrainDetailTexture = Shader.PropertyToID("terrainDetailTexture");

		// Token: 0x04000116 RID: 278
		public static readonly int PROP_detailCounterBuffer = Shader.PropertyToID("detailCounterBuffer");

		// Token: 0x04000117 RID: 279
		public static readonly int PROP_terrainHoleTexture = Shader.PropertyToID("terrainHoleTexture");

		// Token: 0x04000118 RID: 280
		public static readonly int PROP_detailLayerBuffer = Shader.PropertyToID("detailLayerBuffer");

		// Token: 0x04000119 RID: 281
		public static readonly int PROP_detailResolution = Shader.PropertyToID("detailResolution");

		// Token: 0x0400011A RID: 282
		public static readonly int PROP_heightmapTexture = Shader.PropertyToID("heightmapTexture");

		// Token: 0x0400011B RID: 283
		public static readonly int PROP_terrainHeightmapResolution = Shader.PropertyToID("terrainHeightmapResolution");

		// Token: 0x0400011C RID: 284
		public static readonly int PROP_terrainPosition = Shader.PropertyToID("terrainPosition");

		// Token: 0x0400011D RID: 285
		public static readonly int PROP_terrainSize = Shader.PropertyToID("terrainSize");

		// Token: 0x0400011E RID: 286
		public static readonly int PROP_alphaMapTexture = Shader.PropertyToID("alphaMapTexture");

		// Token: 0x0400011F RID: 287
		public static readonly int PROP_alphamapResolution = Shader.PropertyToID("alphamapResolution");

		// Token: 0x04000120 RID: 288
		public static readonly int PROP_detailTextureSize = Shader.PropertyToID("detailTextureSize");

		// Token: 0x04000121 RID: 289
		public static readonly int PROP_heightmapTextureSize = Shader.PropertyToID("heightmapTextureSize");

		// Token: 0x04000122 RID: 290
		public static readonly int PROP_startPosition = Shader.PropertyToID("startPosition");

		// Token: 0x04000123 RID: 291
		public static readonly int PROP_cameraPos = Shader.PropertyToID("cameraPos");

		// Token: 0x04000124 RID: 292
		public static readonly int PROP_density = Shader.PropertyToID("density");

		// Token: 0x04000125 RID: 293
		public static readonly int PROP_detailObjectDistance = Shader.PropertyToID("detailObjectDistance");

		// Token: 0x04000126 RID: 294
		public static readonly int PROP_healthyDryNoiseTexture = Shader.PropertyToID("healthyDryNoiseTexture");

		// Token: 0x04000127 RID: 295
		public static readonly int PROP_gpuiTreeInstanceDataBuffer = Shader.PropertyToID("gpuiTreeInstanceDataBuffer");

		// Token: 0x04000128 RID: 296
		public static readonly int PROP_terrainPrototypeIndex = Shader.PropertyToID("terrainPrototypeIndex");

		// Token: 0x04000129 RID: 297
		public static readonly int PROP_treeData = Shader.PropertyToID("treeData");

		// Token: 0x0400012A RID: 298
		public static readonly int PROP_prefabScale = Shader.PropertyToID("prefabScale");

		// Token: 0x0400012B RID: 299
		public static readonly int PROP_applyRotation = Shader.PropertyToID("applyRotation");

		// Token: 0x0400012C RID: 300
		public static readonly int PROP_applyHeight = Shader.PropertyToID("applyHeight");

		// Token: 0x0400012D RID: 301
		public static readonly int PROP_terrainDataCount = Shader.PropertyToID("terrainDataCount");

		// Token: 0x0400012E RID: 302
		public static readonly int PROP_terrainDataBuffer = Shader.PropertyToID("terrainDataBuffer");
	}
}
