using System;
using System.Text.RegularExpressions;
using UnityEngine;

namespace GPUInstancerPro
{
	// Token: 0x02000061 RID: 97
	public static class GPUIConstants
	{
		// Token: 0x0600037A RID: 890 RVA: 0x00013985 File Offset: 0x00011B85
		public static string GetDefaultPath()
		{
			return "Assets/GPUInstancerPro/";
		}

		// Token: 0x0600037B RID: 891 RVA: 0x0001398C File Offset: 0x00011B8C
		public static string GetPackagesPath()
		{
			if (string.IsNullOrEmpty(GPUIConstants._packagesPath))
			{
				GPUIConstants._packagesPath = "Packages/com.gurbu.gpui-pro/";
			}
			return GPUIConstants._packagesPath;
		}

		// Token: 0x0600037C RID: 892 RVA: 0x000139A9 File Offset: 0x00011BA9
		public static string GetDefaultUserDataPath()
		{
			return GPUIConstants.GetDefaultPath();
		}

		// Token: 0x0600037D RID: 893 RVA: 0x000139B0 File Offset: 0x00011BB0
		public static string GetGeneratedShaderPath()
		{
			return GPUIConstants.GetDefaultUserDataPath() + "Shaders/";
		}

		// Token: 0x0600037E RID: 894 RVA: 0x000139C1 File Offset: 0x00011BC1
		public static string GetProfilesPath()
		{
			return GPUIConstants.GetDefaultUserDataPath() + "Profiles/";
		}

		// Token: 0x0600037F RID: 895 RVA: 0x000139D2 File Offset: 0x00011BD2
		public static string GetStandardAssetsPath()
		{
			return GPUIConstants.GetDefaultUserDataPath() + "StandardAssets/";
		}

		// Token: 0x06000380 RID: 896 RVA: 0x000139E3 File Offset: 0x00011BE3
		public static string GetProceduralDataPath()
		{
			return GPUIConstants.GetDefaultUserDataPath() + "ProceduralData/";
		}

		// Token: 0x06000381 RID: 897 RVA: 0x000139F4 File Offset: 0x00011BF4
		public static string GetExtensionsUserDataPath()
		{
			return GPUIConstants.GetDefaultPath() + "Extensions/";
		}

		// Token: 0x17000069 RID: 105
		// (get) Token: 0x06000382 RID: 898 RVA: 0x00013A08 File Offset: 0x00011C08
		public static Shader DefaultRPShader
		{
			get
			{
				GPUIRenderPipeline renderPipeline = GPUIRuntimeSettings.Instance.RenderPipeline;
				if (renderPipeline == GPUIRenderPipeline.URP)
				{
					return Shader.Find("Universal Render Pipeline/Lit");
				}
				if (renderPipeline != GPUIRenderPipeline.HDRP)
				{
					return Shader.Find("Standard");
				}
				return Shader.Find("HDRP/Lit");
			}
		}

		// Token: 0x1700006A RID: 106
		// (get) Token: 0x06000383 RID: 899 RVA: 0x00013A52 File Offset: 0x00011C52
		public static GameObject PreviewSceneElements
		{
			get
			{
				return GPUIConstants._previewSceneElements;
			}
		}

		// Token: 0x1700006B RID: 107
		// (get) Token: 0x06000384 RID: 900 RVA: 0x00013A59 File Offset: 0x00011C59
		public static Shader ShaderUnityInternalError
		{
			get
			{
				if (GPUIConstants._ShaderUnityInternalError == null)
				{
					GPUIConstants._ShaderUnityInternalError = Shader.Find("Hidden/InternalErrorShader");
				}
				return GPUIConstants._ShaderUnityInternalError;
			}
		}

		// Token: 0x06000385 RID: 901 RVA: 0x00013A7C File Offset: 0x00011C7C
		public static string GetShaderNamePrefix(string extensionCode)
		{
			if (extensionCode == "CROWD")
			{
				return "GPUInstancerPro/CrowdAnimations/";
			}
			return "GPUInstancerPro/";
		}

		// Token: 0x1700006C RID: 108
		// (get) Token: 0x06000386 RID: 902 RVA: 0x00013A96 File Offset: 0x00011C96
		public static ComputeShader CS_GraphicsBufferUtility
		{
			get
			{
				if (GPUIConstants._CS_GraphicsBufferUtility == null)
				{
					GPUIConstants._CS_GraphicsBufferUtility = GPUIUtility.LoadResource<ComputeShader>("GPUIGraphicsBufferUtilityCS");
				}
				return GPUIConstants._CS_GraphicsBufferUtility;
			}
		}

		// Token: 0x1700006D RID: 109
		// (get) Token: 0x06000387 RID: 903 RVA: 0x00013AB9 File Offset: 0x00011CB9
		public static ComputeShader CS_CommandBufferUtility
		{
			get
			{
				if (GPUIConstants._CS_CommandBufferUtility == null)
				{
					GPUIConstants._CS_CommandBufferUtility = GPUIUtility.LoadResource<ComputeShader>("GPUICommandBufferUtilityCS");
				}
				return GPUIConstants._CS_CommandBufferUtility;
			}
		}

		// Token: 0x1700006E RID: 110
		// (get) Token: 0x06000388 RID: 904 RVA: 0x00013ADC File Offset: 0x00011CDC
		public static ComputeShader CS_CameraVisibility
		{
			get
			{
				if (GPUIConstants._CS_CameraVisibility == null)
				{
					GPUIConstants._CS_CameraVisibility = GPUIUtility.LoadResource<ComputeShader>("GPUICameraVisibilityCS");
				}
				return GPUIConstants._CS_CameraVisibility;
			}
		}

		// Token: 0x1700006F RID: 111
		// (get) Token: 0x06000389 RID: 905 RVA: 0x00013AFF File Offset: 0x00011CFF
		public static ComputeShader CS_CameraVisibilityXR
		{
			get
			{
				if (GPUIConstants._CS_CameraVisibilityXR == null)
				{
					GPUIConstants._CS_CameraVisibilityXR = GPUIUtility.LoadResource<ComputeShader>("GPUICameraVisibilityXRCS");
				}
				return GPUIConstants._CS_CameraVisibilityXR;
			}
		}

		// Token: 0x17000070 RID: 112
		// (get) Token: 0x0600038A RID: 906 RVA: 0x00013B22 File Offset: 0x00011D22
		public static ComputeShader CS_TextureUtility
		{
			get
			{
				if (GPUIConstants._CS_TextureUtility == null)
				{
					GPUIConstants._CS_TextureUtility = GPUIUtility.LoadResource<ComputeShader>("GPUITextureUtilityCS");
				}
				return GPUIConstants._CS_TextureUtility;
			}
		}

		// Token: 0x17000071 RID: 113
		// (get) Token: 0x0600038B RID: 907 RVA: 0x00013B45 File Offset: 0x00011D45
		public static ComputeShader CS_HiZTextureCopy
		{
			get
			{
				if (GPUIConstants._CS_HiZTextureCopy == null)
				{
					GPUIConstants._CS_HiZTextureCopy = GPUIUtility.LoadResource<ComputeShader>("GPUIHiZTextureCopyCS");
				}
				return GPUIConstants._CS_HiZTextureCopy;
			}
		}

		// Token: 0x17000072 RID: 114
		// (get) Token: 0x0600038C RID: 908 RVA: 0x00013B68 File Offset: 0x00011D68
		public static ComputeShader CS_TextureReduce
		{
			get
			{
				if (GPUIConstants._CS_TextureReduce == null)
				{
					GPUIConstants._CS_TextureReduce = GPUIUtility.LoadResource<ComputeShader>("GPUITextureReduceCS");
				}
				return GPUIConstants._CS_TextureReduce;
			}
		}

		// Token: 0x17000073 RID: 115
		// (get) Token: 0x0600038D RID: 909 RVA: 0x00013B8B File Offset: 0x00011D8B
		public static ComputeShader CS_Billboard
		{
			get
			{
				if (GPUIConstants._CS_Billboard == null)
				{
					GPUIConstants._CS_Billboard = GPUIUtility.LoadResource<ComputeShader>("GPUIBillboardCS");
				}
				return GPUIConstants._CS_Billboard;
			}
		}

		// Token: 0x17000074 RID: 116
		// (get) Token: 0x0600038E RID: 910 RVA: 0x00013BAE File Offset: 0x00011DAE
		public static ComputeShader CS_BufferToTexture
		{
			get
			{
				if (GPUIConstants._CS_BufferToTexture == null)
				{
					GPUIConstants._CS_BufferToTexture = GPUIUtility.LoadResource<ComputeShader>("GPUIBufferToTextureCS");
				}
				return GPUIConstants._CS_BufferToTexture;
			}
		}

		// Token: 0x17000075 RID: 117
		// (get) Token: 0x0600038F RID: 911 RVA: 0x00013BD1 File Offset: 0x00011DD1
		public static ComputeShader CS_TransformModifications
		{
			get
			{
				if (GPUIConstants._CS_TransformModifications == null)
				{
					GPUIConstants._CS_TransformModifications = GPUIUtility.LoadResource<ComputeShader>("GPUITransformModificationsCS");
				}
				return GPUIConstants._CS_TransformModifications;
			}
		}

		// Token: 0x17000076 RID: 118
		// (get) Token: 0x06000390 RID: 912 RVA: 0x00013BF4 File Offset: 0x00011DF4
		public static ComputeShader CS_OptionalRenderer
		{
			get
			{
				if (GPUIConstants._CS_OptionalRenderer == null)
				{
					GPUIConstants._CS_OptionalRenderer = GPUIUtility.LoadResource<ComputeShader>("GPUIOptionalRendererCS");
				}
				return GPUIConstants._CS_OptionalRenderer;
			}
		}

		// Token: 0x17000077 RID: 119
		// (get) Token: 0x06000391 RID: 913 RVA: 0x00013C17 File Offset: 0x00011E17
		public static ComputeShader CS_LightProbeUtility
		{
			get
			{
				if (GPUIConstants._CS_LightProbeUtility == null)
				{
					GPUIConstants._CS_LightProbeUtility = GPUIUtility.LoadResource<ComputeShader>("GPUILightProbeUtilityCS");
				}
				return GPUIConstants._CS_LightProbeUtility;
			}
		}

		// Token: 0x17000078 RID: 120
		// (get) Token: 0x06000392 RID: 914 RVA: 0x00013C3A File Offset: 0x00011E3A
		public static ComputeShader CS_CalculateInstancingBounds
		{
			get
			{
				if (GPUIConstants._CS_CalculateInstancingBounds == null)
				{
					GPUIConstants._CS_CalculateInstancingBounds = GPUIUtility.LoadResource<ComputeShader>("GPUICalculateInstancingBoundsCS");
				}
				return GPUIConstants._CS_CalculateInstancingBounds;
			}
		}

		// Token: 0x17000079 RID: 121
		// (get) Token: 0x06000393 RID: 915 RVA: 0x00013C60 File Offset: 0x00011E60
		public static string UNITY_VERSION
		{
			get
			{
				if (string.IsNullOrEmpty(GPUIConstants._sanitizedUnityVersion))
				{
					Match match = Regex.Match(Application.unityVersion, "^(\\d+(\\.\\d+)*)");
					GPUIConstants._sanitizedUnityVersion = (match.Success ? match.Value : "");
				}
				return GPUIConstants._sanitizedUnityVersion;
			}
		}

		// Token: 0x1700007A RID: 122
		// (get) Token: 0x06000394 RID: 916 RVA: 0x00013CA8 File Offset: 0x00011EA8
		public static string UNITY_VERSION_TITLE
		{
			get
			{
				string unity_VERSION = GPUIConstants.UNITY_VERSION;
				if (string.IsNullOrEmpty(unity_VERSION))
				{
					return unity_VERSION;
				}
				int num = unity_VERSION.IndexOf('.');
				if (num < 0)
				{
					return unity_VERSION;
				}
				int num2;
				if (!int.TryParse(unity_VERSION.Substring(0, num), out num2))
				{
					return unity_VERSION;
				}
				int num3 = num2;
				if (num2 > 3000)
				{
					num3 = num2 / 1000;
				}
				string text = unity_VERSION.Substring(num + 1);
				string text2 = "";
				GPUIRenderPipeline renderPipeline = GPUIRuntimeSettings.Instance.RenderPipeline;
				if (renderPipeline != GPUIRenderPipeline.BuiltIn)
				{
					if (renderPipeline != GPUIRenderPipeline.URP)
					{
						if (renderPipeline == GPUIRenderPipeline.HDRP)
						{
							text2 = "HD";
						}
					}
					else
					{
						text2 = "U";
					}
				}
				else
				{
					text2 = "Bi";
				}
				return string.Concat(new string[]
				{
					num3.ToString(),
					".",
					text,
					" ",
					text2
				});
			}
		}

		// Token: 0x04000274 RID: 628
		public const string PATH_LOCATOR_GUID = "70144417e5298854fbabfec8cdd9eb41";

		// Token: 0x04000275 RID: 629
		public const string PATH_RESOURCES = "Resources/";

		// Token: 0x04000276 RID: 630
		public const string PATH_SETTINGS = "Settings/";

		// Token: 0x04000277 RID: 631
		public const string PATH_RUNTIME = "Runtime/";

		// Token: 0x04000278 RID: 632
		public const string PATH_PREFABS = "Prefabs/";

		// Token: 0x04000279 RID: 633
		public const string PATH_EDITOR = "Editor/";

		// Token: 0x0400027A RID: 634
		public const string PATH_SHADER = "Shaders/";

		// Token: 0x0400027B RID: 635
		public const string PATH_COMPUTE = "Compute/";

		// Token: 0x0400027C RID: 636
		public const string PATH_SHADER_INCLUDE = "Include/";

		// Token: 0x0400027D RID: 637
		public const string PATH_TEXTURES = "Textures/";

		// Token: 0x0400027E RID: 638
		public const string PATH_STANDARD_ASSETS = "StandardAssets/";

		// Token: 0x0400027F RID: 639
		public const string PATH_PROFILES = "Profiles/";

		// Token: 0x04000280 RID: 640
		public const string PATH_PROCEDURALDATA = "ProceduralData/";

		// Token: 0x04000281 RID: 641
		public const string PATH_EXTENSIONS = "Extensions/";

		// Token: 0x04000282 RID: 642
		public const string FILE_EDITOR_SETTINGS = "GPUIEditorSettings";

		// Token: 0x04000283 RID: 643
		public const string FILE_RUNTIME_SETTINGS = "GPUIRuntimeSettings";

		// Token: 0x04000284 RID: 644
		public const string FILE_SHADER_BINDINGS = "GPUIShaderBindings";

		// Token: 0x04000285 RID: 645
		public const string FILE_SHADER_VARIANT_COLLECTION = "GPUIShaderVariantCollection";

		// Token: 0x04000286 RID: 646
		public const string FILE_DEFAULT_PROFILE = "GPUIDefaultProfile";

		// Token: 0x04000287 RID: 647
		public const string FILE_PREVIEV_SCENE_ELEMENTS = "Preview/PreviewSceneElements";

		// Token: 0x04000288 RID: 648
		public const string FILE_CALIBRATION_FLOOR_MATERIAL = "Preview/CalibrationFloor";

		// Token: 0x04000289 RID: 649
		public const string FILE_DEBUGGER_CANVAS = "GPUIDebuggerCanvas";

		// Token: 0x0400028A RID: 650
		public const string FILE_CS_CameraVisibility = "GPUICameraVisibilityCS";

		// Token: 0x0400028B RID: 651
		public const string FILE_CS_CameraVisibilityXR = "GPUICameraVisibilityXRCS";

		// Token: 0x0400028C RID: 652
		public const string FILE_CS_CommandBufferUtility = "GPUICommandBufferUtilityCS";

		// Token: 0x0400028D RID: 653
		public const string FILE_CS_GraphicsBufferUtility = "GPUIGraphicsBufferUtilityCS";

		// Token: 0x0400028E RID: 654
		public const string FILE_CS_TextureUtility = "GPUITextureUtilityCS";

		// Token: 0x0400028F RID: 655
		public const string FILE_CS_HiZTextureCopy = "GPUIHiZTextureCopyCS";

		// Token: 0x04000290 RID: 656
		public const string FILE_CS_TextureReduce = "GPUITextureReduceCS";

		// Token: 0x04000291 RID: 657
		public const string FILE_CS_Billboard = "GPUIBillboardCS";

		// Token: 0x04000292 RID: 658
		public const string FILE_CS_BufferToTexture = "GPUIBufferToTextureCS";

		// Token: 0x04000293 RID: 659
		public const string FILE_CS_TransformModifications = "GPUITransformModificationsCS";

		// Token: 0x04000294 RID: 660
		public const string FILE_CS_OptionalRenderer = "GPUIOptionalRendererCS";

		// Token: 0x04000295 RID: 661
		public const string FILE_CS_LightProbeUtility = "GPUILightProbeUtilityCS";

		// Token: 0x04000296 RID: 662
		public const string FILE_CS_CalculateInstancingBounds = "GPUICalculateInstancingBoundsCS";

		// Token: 0x04000297 RID: 663
		private const string _initialAssetsPath = "Assets/GPUInstancerPro/";

		// Token: 0x04000298 RID: 664
		private static string _packagesPath;

		// Token: 0x04000299 RID: 665
		private static GameObject _previewSceneElements;

		// Token: 0x0400029A RID: 666
		public static readonly string[] SHADERS_UNITY_BUILTIN = new string[]
		{
			"Standard", "Standard (Specular setup)", "Legacy Shaders/VertexLit", "Nature/SpeedTree", "Nature/Tree Creator Bark", "Hidden/Nature/Tree Creator Bark Optimized", "Nature/Tree Creator Leaves", "Hidden/Nature/Tree Creator Leaves Optimized", "Nature/Tree Creator Leaves Fast", "Hidden/Nature/Tree Creator Leaves Fast Optimized",
			"Nature/Tree Soft Occlusion Bark", "Nature/Tree Soft Occlusion Leaves"
		};

		// Token: 0x0400029B RID: 667
		public const string SHADER_UNITY_INTERNAL_ERROR = "Hidden/InternalErrorShader";

		// Token: 0x0400029C RID: 668
		public const string SHADER_UNITY_SPEEDTREE = "Nature/SpeedTree";

		// Token: 0x0400029D RID: 669
		public const string SHADER_UNITY_SPEEDTREE8 = "Nature/SpeedTree8";

		// Token: 0x0400029E RID: 670
		public const string SHADER_UNITY_SPEEDTREE9 = "Nature/SpeedTree9";

		// Token: 0x0400029F RID: 671
		public const string SHADER_GPUI_ERROR = "Hidden/GPUInstancerPro/InternalErrorShader";

		// Token: 0x040002A0 RID: 672
		public const string SHADER_GPUI_TREE_PROXY = "Hidden/GPUInstancerPro/Nature/TreeProxy";

		// Token: 0x040002A1 RID: 673
		public const string SHADER_GPUI_BILLBOARD_ALBEDO_BAKER = "Hidden/GPUInstancerPro/Billboard/AlbedoBake";

		// Token: 0x040002A2 RID: 674
		public const string SHADER_GPUI_BILLBOARD_NORMAL_BAKER = "Hidden/GPUInstancerPro/Billboard/NormalBake";

		// Token: 0x040002A3 RID: 675
		public const string SHADER_GPUI_BILLBOARD_URP = "GPUInstancerPro/Billboard/BillboardURP_GPUIPro";

		// Token: 0x040002A4 RID: 676
		public const string SHADER_GPUI_BILLBOARD_HDRP = "GPUInstancerPro/Billboard/BillboardHDRP_GPUIPro";

		// Token: 0x040002A5 RID: 677
		public const string SHADER_GPUI_BILLBOARD_Builtin = "GPUInstancerPro/Billboard/BillboardBuiltin_GPUIPro";

		// Token: 0x040002A6 RID: 678
		public const string SHADER_GPUI_BILLBOARD_Builtin_SpeedTree = "GPUInstancerPro/Billboard/2DRendererSpeedTree";

		// Token: 0x040002A7 RID: 679
		public const string SHADER_GPUI_BILLBOARD_Builtin_TreeCreator = "GPUInstancerPro/Billboard/2DRendererTreeCreator";

		// Token: 0x040002A8 RID: 680
		public const string SHADER_GPUI_BILLBOARD_Builtin_SoftOcclusion = "GPUInstancerPro/Billboard/2DRendererSoftOcclusion";

		// Token: 0x040002A9 RID: 681
		public static Shader _ShaderUnityInternalError;

		// Token: 0x040002AA RID: 682
		public const string Kw_LOD_FADE_CROSSFADE = "LOD_FADE_CROSSFADE";

		// Token: 0x040002AB RID: 683
		public const string EXTENSION_CODE_CROWD = "CROWD";

		// Token: 0x040002AC RID: 684
		public const string SHADER_NAME_PREFIX = "GPUInstancerPro/";

		// Token: 0x040002AD RID: 685
		public const string SHADER_NAME_PREFIX_CROWD = "GPUInstancerPro/CrowdAnimations/";

		// Token: 0x040002AE RID: 686
		public const string PACKAGE_NAME_CROWD = "com.gurbu.gpui-pro.crowd-animations";

		// Token: 0x040002AF RID: 687
		public static float CS_THREAD_COUNT;

		// Token: 0x040002B0 RID: 688
		public static float CS_THREAD_COUNT_HEAVY;

		// Token: 0x040002B1 RID: 689
		public static float CS_THREAD_COUNT_2D;

		// Token: 0x040002B2 RID: 690
		public static float CS_THREAD_COUNT_3D;

		// Token: 0x040002B3 RID: 691
		public static int TEXTURE_MAX_SIZE;

		// Token: 0x040002B4 RID: 692
		public static long MAX_BUFFER_SIZE;

		// Token: 0x040002B5 RID: 693
		private static ComputeShader _CS_GraphicsBufferUtility;

		// Token: 0x040002B6 RID: 694
		private static ComputeShader _CS_CommandBufferUtility;

		// Token: 0x040002B7 RID: 695
		private static ComputeShader _CS_CameraVisibility;

		// Token: 0x040002B8 RID: 696
		private static ComputeShader _CS_CameraVisibilityXR;

		// Token: 0x040002B9 RID: 697
		public const string Kw_GPUI_LOD = "GPUI_LOD";

		// Token: 0x040002BA RID: 698
		public const string Kw_GPUI_LOD_CROSSFADE = "GPUI_LOD_CROSSFADE";

		// Token: 0x040002BB RID: 699
		public const string Kw_GPUI_LOD_CROSSFADE_ANIMATE = "GPUI_LOD_CROSSFADE_ANIMATE";

		// Token: 0x040002BC RID: 700
		public const string Kw_GPUI_OCCLUSION_CULLING = "GPUI_OCCLUSION_CULLING";

		// Token: 0x040002BD RID: 701
		public const string Kw_GPUI_SHADOWCASTING = "GPUI_SHADOWCASTING";

		// Token: 0x040002BE RID: 702
		public const string Kw_GPUI_SHADOWCULLED = "GPUI_SHADOWCULLED";

		// Token: 0x040002BF RID: 703
		public const string Kw_GPUI_NO_BUFFER = "GPUI_NO_BUFFER";

		// Token: 0x040002C0 RID: 704
		public const string Kw_GPUI_TRANSFORM_OFFSET = "GPUI_TRANSFORM_OFFSET";

		// Token: 0x040002C1 RID: 705
		public const string Kw_GPUI_THREAD_SIZE_512 = "GPUI_THREAD_SIZE_512";

		// Token: 0x040002C2 RID: 706
		public const string Kw_GPUI_THREAD_SIZE_256 = "GPUI_THREAD_SIZE_256";

		// Token: 0x040002C3 RID: 707
		public const string Kw_GPUI_THREAD_SIZE_HEAVY_256 = "GPUI_THREAD_SIZE_HEAVY_256";

		// Token: 0x040002C4 RID: 708
		public const string Kw_GPUI_TRANSFORM_DATA = "GPUI_TRANSFORM_DATA";

		// Token: 0x040002C5 RID: 709
		public const string Kw_GPUI_UINT_BUFFER = "GPUI_UINT_BUFFER";

		// Token: 0x040002C6 RID: 710
		private static ComputeShader _CS_TextureUtility;

		// Token: 0x040002C7 RID: 711
		private static ComputeShader _CS_HiZTextureCopy;

		// Token: 0x040002C8 RID: 712
		private static ComputeShader _CS_TextureReduce;

		// Token: 0x040002C9 RID: 713
		private static ComputeShader _CS_Billboard;

		// Token: 0x040002CA RID: 714
		private static ComputeShader _CS_BufferToTexture;

		// Token: 0x040002CB RID: 715
		public const string Kw_GPUI_FLOAT4_BUFFER = "GPUI_FLOAT4_BUFFER";

		// Token: 0x040002CC RID: 716
		private static ComputeShader _CS_TransformModifications;

		// Token: 0x040002CD RID: 717
		private static ComputeShader _CS_OptionalRenderer;

		// Token: 0x040002CE RID: 718
		private static ComputeShader _CS_LightProbeUtility;

		// Token: 0x040002CF RID: 719
		private static ComputeShader _CS_CalculateInstancingBounds;

		// Token: 0x040002D0 RID: 720
		public static readonly int PROP_CameraDepthTexture = Shader.PropertyToID("_CameraDepthTexture");

		// Token: 0x040002D1 RID: 721
		public static readonly int PROP_unity_LODFade = Shader.PropertyToID("unity_LODFade");

		// Token: 0x040002D2 RID: 722
		public static readonly int PROP_unity_LightmapST = Shader.PropertyToID("unity_LightmapST");

		// Token: 0x040002D3 RID: 723
		public static readonly int PROP_unity_DynamicLightmapST = Shader.PropertyToID("unity_DynamicLightmapST");

		// Token: 0x040002D4 RID: 724
		public static readonly int PROP_unity_SHAr = Shader.PropertyToID("unity_SHAr");

		// Token: 0x040002D5 RID: 725
		public static readonly int PROP_unity_SHAg = Shader.PropertyToID("unity_SHAg");

		// Token: 0x040002D6 RID: 726
		public static readonly int PROP_unity_SHAb = Shader.PropertyToID("unity_SHAb");

		// Token: 0x040002D7 RID: 727
		public static readonly int PROP_unity_SHBr = Shader.PropertyToID("unity_SHBr");

		// Token: 0x040002D8 RID: 728
		public static readonly int PROP_unity_SHBg = Shader.PropertyToID("unity_SHBg");

		// Token: 0x040002D9 RID: 729
		public static readonly int PROP_unity_SHBb = Shader.PropertyToID("unity_SHBb");

		// Token: 0x040002DA RID: 730
		public static readonly int PROP_unity_SHC = Shader.PropertyToID("unity_SHC");

		// Token: 0x040002DB RID: 731
		public static readonly int PROP_unity_ProbesOcclusion = Shader.PropertyToID("unity_ProbesOcclusion");

		// Token: 0x040002DC RID: 732
		public static readonly int PROP_gpuiTransformBuffer = Shader.PropertyToID("gpuiTransformBuffer");

		// Token: 0x040002DD RID: 733
		public static readonly int PROP_gpuiPreviousFrameTransformBuffer = Shader.PropertyToID("gpuiPreviousFrameTransformBuffer");

		// Token: 0x040002DE RID: 734
		public static readonly int PROP_hasPreviousFrameTransformBuffer = Shader.PropertyToID("hasPreviousFrameTransformBuffer");

		// Token: 0x040002DF RID: 735
		public static readonly int PROP_gpuiTransformBufferTexture = Shader.PropertyToID("gpuiTransformBufferTexture");

		// Token: 0x040002E0 RID: 736
		public static readonly int PROP_hasPerInstanceLightProbes = Shader.PropertyToID("hasPerInstanceLightProbes");

		// Token: 0x040002E1 RID: 737
		public static readonly int PROP_gpuiPerInstanceLightProbesBuffer = Shader.PropertyToID("gpuiPerInstanceLightProbesBuffer");

		// Token: 0x040002E2 RID: 738
		public static readonly int PROP_gpuiInstanceDataBuffer = Shader.PropertyToID("gpuiInstanceDataBuffer");

		// Token: 0x040002E3 RID: 739
		public static readonly int PROP_gpuiInstanceDataBufferTexture = Shader.PropertyToID("gpuiInstanceDataBufferTexture");

		// Token: 0x040002E4 RID: 740
		public static readonly int PROP_visibilityBuffer = Shader.PropertyToID("visibilityBuffer");

		// Token: 0x040002E5 RID: 741
		public static readonly int PROP_gpuiTransformOffset = Shader.PropertyToID("gpuiTransformOffset");

		// Token: 0x040002E6 RID: 742
		public static readonly int PROP_bufferSize = Shader.PropertyToID("bufferSize");

		// Token: 0x040002E7 RID: 743
		public static readonly int PROP_multiplier = Shader.PropertyToID("multiplier");

		// Token: 0x040002E8 RID: 744
		public static readonly int PROP_maxTextureSize = Shader.PropertyToID("maxTextureSize");

		// Token: 0x040002E9 RID: 745
		public static readonly int PROP_mvpMatrix = Shader.PropertyToID("mvpMatrix");

		// Token: 0x040002EA RID: 746
		public static readonly int PROP_mvpMatrix2 = Shader.PropertyToID("mvpMatrix2");

		// Token: 0x040002EB RID: 747
		public static readonly int PROP_cameraPositionAndHalfAngle = Shader.PropertyToID("cameraPositionAndHalfAngle");

		// Token: 0x040002EC RID: 748
		public static readonly int PROP_commandBuffer = Shader.PropertyToID("commandBuffer");

		// Token: 0x040002ED RID: 749
		public static readonly int PROP_parameterBuffer = Shader.PropertyToID("parameterBuffer");

		// Token: 0x040002EE RID: 750
		public static readonly int PROP_source = Shader.PropertyToID("source");

		// Token: 0x040002EF RID: 751
		public static readonly int PROP_textureArray = Shader.PropertyToID("textureArray");

		// Token: 0x040002F0 RID: 752
		public static readonly int PROP_destination = Shader.PropertyToID("destination");

		// Token: 0x040002F1 RID: 753
		public static readonly int PROP_offsetX = Shader.PropertyToID("offsetX");

		// Token: 0x040002F2 RID: 754
		public static readonly int PROP_offsetY = Shader.PropertyToID("offsetY");

		// Token: 0x040002F3 RID: 755
		public static readonly int PROP_sourceSizeX = Shader.PropertyToID("sourceSizeX");

		// Token: 0x040002F4 RID: 756
		public static readonly int PROP_sourceSizeY = Shader.PropertyToID("sourceSizeY");

		// Token: 0x040002F5 RID: 757
		public static readonly int PROP_destinationSizeX = Shader.PropertyToID("destinationSizeX");

		// Token: 0x040002F6 RID: 758
		public static readonly int PROP_destinationSizeY = Shader.PropertyToID("destinationSizeY");

		// Token: 0x040002F7 RID: 759
		public static readonly int PROP_reverseZ = Shader.PropertyToID("reverseZ");

		// Token: 0x040002F8 RID: 760
		public static readonly int PROP_textureArrayIndex = Shader.PropertyToID("textureArrayIndex");

		// Token: 0x040002F9 RID: 761
		public static readonly int PROP_hiZMap = Shader.PropertyToID("hiZMap");

		// Token: 0x040002FA RID: 762
		public static readonly int PROP_hiZTxtrSize = Shader.PropertyToID("hiZTxtrSize");

		// Token: 0x040002FB RID: 763
		public static readonly int PROP_sizeAndIndexes = Shader.PropertyToID("sizeAndIndexes");

		// Token: 0x040002FC RID: 764
		public static readonly int PROP_sizeAndIndexes2 = Shader.PropertyToID("sizeAndIndexes2");

		// Token: 0x040002FD RID: 765
		public static readonly int PROP_instanceCount = Shader.PropertyToID("instanceCount");

		// Token: 0x040002FE RID: 766
		public static readonly int PROP_sourceBuffer = Shader.PropertyToID("sourceBuffer");

		// Token: 0x040002FF RID: 767
		public static readonly int PROP_targetBuffer = Shader.PropertyToID("targetBuffer");

		// Token: 0x04000300 RID: 768
		public static readonly int PROP_filterBuffer = Shader.PropertyToID("filterBuffer");

		// Token: 0x04000301 RID: 769
		public static readonly int PROP_targetTexture = Shader.PropertyToID("targetTexture");

		// Token: 0x04000302 RID: 770
		public static readonly int PROP_sourceStartIndex = Shader.PropertyToID("sourceStartIndex");

		// Token: 0x04000303 RID: 771
		public static readonly int PROP_targetStartIndex = Shader.PropertyToID("targetStartIndex");

		// Token: 0x04000304 RID: 772
		public static readonly int PROP_count = Shader.PropertyToID("count");

		// Token: 0x04000305 RID: 773
		public static readonly int PROP_additionalValues = Shader.PropertyToID("additionalValues");

		// Token: 0x04000306 RID: 774
		public static readonly int PROP_position = Shader.PropertyToID("position");

		// Token: 0x04000307 RID: 775
		public static readonly int PROP_matrix44 = Shader.PropertyToID("matrix44");

		// Token: 0x04000308 RID: 776
		public static readonly int PROP_transformBufferSize = Shader.PropertyToID("transformBufferSize");

		// Token: 0x04000309 RID: 777
		public static readonly int PROP_instanceDataBufferSize = Shader.PropertyToID("instanceDataBufferSize");

		// Token: 0x0400030A RID: 778
		public static readonly int PROP_outputTexture = Shader.PropertyToID("outputTexture");

		// Token: 0x0400030B RID: 779
		public static readonly int PROP_startIndex = Shader.PropertyToID("startIndex");

		// Token: 0x0400030C RID: 780
		public static readonly int PROP_prototypeIndex = Shader.PropertyToID("prototypeIndex");

		// Token: 0x0400030D RID: 781
		public static readonly int PROP_prototypeCount = Shader.PropertyToID("prototypeCount");

		// Token: 0x0400030E RID: 782
		public static readonly int PROP_counterBuffer = Shader.PropertyToID("counterBuffer");

		// Token: 0x0400030F RID: 783
		public static readonly int PROP_boundsCenter = Shader.PropertyToID("boundsCenter");

		// Token: 0x04000310 RID: 784
		public static readonly int PROP_boundsExtents = Shader.PropertyToID("boundsExtents");

		// Token: 0x04000311 RID: 785
		public static readonly int PROP_modifierTransform = Shader.PropertyToID("modifierTransform");

		// Token: 0x04000312 RID: 786
		public static readonly int PROP_modifierRadius = Shader.PropertyToID("modifierRadius");

		// Token: 0x04000313 RID: 787
		public static readonly int PROP_modifierHeight = Shader.PropertyToID("modifierHeight");

		// Token: 0x04000314 RID: 788
		public static readonly int PROP_valueToSet = Shader.PropertyToID("valueToSet");

		// Token: 0x04000315 RID: 789
		public static readonly int PROP_transformBufferStartIndex = Shader.PropertyToID("transformBufferStartIndex");

		// Token: 0x04000316 RID: 790
		public static readonly int PROP_textureDataSingleChannel = Shader.PropertyToID("textureDataSingleChannel");

		// Token: 0x04000317 RID: 791
		public static readonly int PROP_currentTime = Shader.PropertyToID("currentTime");

		// Token: 0x04000318 RID: 792
		public static readonly int PROP_optionalRendererStatusBuffer = Shader.PropertyToID("optionalRendererStatusBuffer");

		// Token: 0x04000319 RID: 793
		public static readonly int PROP_commandParamsStartIndex = Shader.PropertyToID("commandParamsStartIndex");

		// Token: 0x0400031A RID: 794
		public static readonly int PROP_rsgCommandStartIndex = Shader.PropertyToID("rsgCommandStartIndex");

		// Token: 0x0400031B RID: 795
		public static readonly int PROP_sphericalHarmonicsBuffer = Shader.PropertyToID("sphericalHarmonicsBuffer");

		// Token: 0x0400031C RID: 796
		public static readonly int PROP_occlusionProbesBuffer = Shader.PropertyToID("occlusionProbesBuffer");

		// Token: 0x0400031D RID: 797
		public static readonly int PROP_isLinearSpace = Shader.PropertyToID("isLinearSpace");

		// Token: 0x0400031E RID: 798
		public static readonly int CONST_GPUILODConstants = Shader.PropertyToID("GPUILODConstants");

		// Token: 0x0400031F RID: 799
		public const string Kw_BILLBOARD_FACE_CAMERA_POS = "BILLBOARD_FACE_CAMERA_POS";

		// Token: 0x04000320 RID: 800
		public const string Kw_GPUI_OBJECT_MOTION_VECTOR_ON = "GPUI_OBJECT_MOTION_VECTOR_ON";

		// Token: 0x04000321 RID: 801
		public const string Kw_GPUI_PER_INSTANCE_LIGHTPROBES_ON = "GPUI_PER_INSTANCE_LIGHTPROBES_ON";

		// Token: 0x04000322 RID: 802
		public static readonly int PROP_GPUIWindZone = Shader.PropertyToID("_GPUIWindZone");

		// Token: 0x04000323 RID: 803
		public static readonly int PROP_GPUIWindDirection = Shader.PropertyToID("_GPUIWindDirection");

		// Token: 0x04000324 RID: 804
		public static readonly int PROP_gpuiBoundsMinMax = Shader.PropertyToID("gpuiBoundsMinMax");

		// Token: 0x04000325 RID: 805
		public const string TAG_MainCamera = "MainCamera";

		// Token: 0x04000326 RID: 806
		private static string _sanitizedUnityVersion = "";

		// Token: 0x04000327 RID: 807
		public static readonly GPUITransformData TRANSFORM_DATA_IDENTITY = new GPUITransformData
		{
			position = Vector3.one,
			rotation = Quaternion.identity,
			scale = Vector3.one
		};

		// Token: 0x04000328 RID: 808
		public static readonly GPUITransformData TRANSFORM_DATA_ZERO = new GPUITransformData
		{
			position = Vector3.zero,
			rotation = new Quaternion(0f, 0f, 0f, 0f),
			scale = Vector3.zero
		};

		// Token: 0x04000329 RID: 809
		public static readonly int[] INSTANCING_BOUNDS_DEFAULT_BUFFER_VALUES = new int[] { int.MaxValue, int.MaxValue, int.MaxValue, int.MinValue, int.MinValue, int.MinValue };

		// Token: 0x0400032A RID: 810
		public static readonly Matrix4x4 IDENTITY_Matrix4x4 = Matrix4x4.identity;

		// Token: 0x0400032B RID: 811
		public static readonly Quaternion IDENTITY_Quaternion = Quaternion.identity;

		// Token: 0x0400032C RID: 812
		public static readonly Bounds Bounds_ZERO = default(Bounds);

		// Token: 0x0400032D RID: 813
		public static readonly Vector4 Vector4_ZERO = Vector4.zero;

		// Token: 0x0400032E RID: 814
		public static readonly Vector4 Vector3_ZERO = Vector3.zero;

		// Token: 0x0400032F RID: 815
		public static readonly Vector4 Vector3_MAX = new Vector3(float.MaxValue, float.MaxValue, float.MaxValue);

		// Token: 0x04000330 RID: 816
		public static readonly Vector4 Vector3_MIN = new Vector3(float.MinValue, float.MinValue, float.MinValue);

		// Token: 0x04000331 RID: 817
		public static readonly string LOG_PREFIX = "<b>[GPU Instancer Pro]</b> ";

		// Token: 0x04000332 RID: 818
		public static readonly string LOG_PREFIX_DEV = "";

		// Token: 0x04000333 RID: 819
		public static readonly float LABEL_WIDTH = 180f;
	}
}
