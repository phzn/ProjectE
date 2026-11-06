using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.XR;

namespace GPUInstancerPro
{
	// Token: 0x0200002D RID: 45
	public class GPUIRuntimeSettings : ScriptableObject
	{
		// Token: 0x17000032 RID: 50
		// (get) Token: 0x0600019F RID: 415 RVA: 0x000093AD File Offset: 0x000075AD
		// (set) Token: 0x060001A0 RID: 416 RVA: 0x000093B5 File Offset: 0x000075B5
		public GraphicsDeviceType GraphicsDeviceType { get; private set; }

		// Token: 0x17000033 RID: 51
		// (get) Token: 0x060001A1 RID: 417 RVA: 0x000093BE File Offset: 0x000075BE
		// (set) Token: 0x060001A2 RID: 418 RVA: 0x000093C6 File Offset: 0x000075C6
		public GPUIRenderPipeline RenderPipeline { get; private set; }

		// Token: 0x17000034 RID: 52
		// (get) Token: 0x060001A3 RID: 419 RVA: 0x000093CF File Offset: 0x000075CF
		// (set) Token: 0x060001A4 RID: 420 RVA: 0x000093D7 File Offset: 0x000075D7
		public GPUIMaxComputeWorkGroupSize ComputeWorkGroupSize { get; private set; }

		// Token: 0x17000035 RID: 53
		// (get) Token: 0x060001A5 RID: 421 RVA: 0x000093E0 File Offset: 0x000075E0
		// (set) Token: 0x060001A6 RID: 422 RVA: 0x000093E8 File Offset: 0x000075E8
		public float ComputeThreadCount { get; private set; }

		// Token: 0x17000036 RID: 54
		// (get) Token: 0x060001A7 RID: 423 RVA: 0x000093F1 File Offset: 0x000075F1
		// (set) Token: 0x060001A8 RID: 424 RVA: 0x000093F9 File Offset: 0x000075F9
		public float ComputeThreadCountHeavy { get; private set; }

		// Token: 0x17000037 RID: 55
		// (get) Token: 0x060001A9 RID: 425 RVA: 0x00009402 File Offset: 0x00007602
		// (set) Token: 0x060001AA RID: 426 RVA: 0x0000940A File Offset: 0x0000760A
		public float ComputeThreadCount2D { get; private set; }

		// Token: 0x17000038 RID: 56
		// (get) Token: 0x060001AB RID: 427 RVA: 0x00009413 File Offset: 0x00007613
		// (set) Token: 0x060001AC RID: 428 RVA: 0x0000941B File Offset: 0x0000761B
		public float ComputeThreadCount3D { get; private set; }

		// Token: 0x17000039 RID: 57
		// (get) Token: 0x060001AD RID: 429 RVA: 0x00009424 File Offset: 0x00007624
		// (set) Token: 0x060001AE RID: 430 RVA: 0x0000942C File Offset: 0x0000762C
		public int TextureMaxSize { get; private set; }

		// Token: 0x1700003A RID: 58
		// (get) Token: 0x060001AF RID: 431 RVA: 0x00009435 File Offset: 0x00007635
		// (set) Token: 0x060001B0 RID: 432 RVA: 0x0000943D File Offset: 0x0000763D
		public bool DisableShaderBuffers { get; private set; }

		// Token: 0x1700003B RID: 59
		// (get) Token: 0x060001B1 RID: 433 RVA: 0x00009446 File Offset: 0x00007646
		// (set) Token: 0x060001B2 RID: 434 RVA: 0x0000944E File Offset: 0x0000764E
		public bool DisableOcclusionCulling { get; private set; }

		// Token: 0x1700003C RID: 60
		// (get) Token: 0x060001B3 RID: 435 RVA: 0x00009457 File Offset: 0x00007657
		// (set) Token: 0x060001B4 RID: 436 RVA: 0x0000945F File Offset: 0x0000765F
		public bool DisablePreviousFrameTransformBuffer { get; private set; }

		// Token: 0x1700003D RID: 61
		// (get) Token: 0x060001B5 RID: 437 RVA: 0x00009468 File Offset: 0x00007668
		// (set) Token: 0x060001B6 RID: 438 RVA: 0x00009470 File Offset: 0x00007670
		public bool DisablePerInstanceLightProbesBuffer { get; private set; }

		// Token: 0x1700003E RID: 62
		// (get) Token: 0x060001B7 RID: 439 RVA: 0x00009479 File Offset: 0x00007679
		// (set) Token: 0x060001B8 RID: 440 RVA: 0x00009481 File Offset: 0x00007681
		public bool DisableInstancingBoundsCalculation { get; private set; }

		// Token: 0x1700003F RID: 63
		// (get) Token: 0x060001B9 RID: 441 RVA: 0x0000948A File Offset: 0x0000768A
		// (set) Token: 0x060001BA RID: 442 RVA: 0x00009492 File Offset: 0x00007692
		public long MaxBufferSize { get; private set; }

		// Token: 0x17000040 RID: 64
		// (get) Token: 0x060001BB RID: 443 RVA: 0x0000949B File Offset: 0x0000769B
		// (set) Token: 0x060001BC RID: 444 RVA: 0x000094A3 File Offset: 0x000076A3
		public bool ReversedZBuffer { get; private set; }

		// Token: 0x17000041 RID: 65
		// (get) Token: 0x060001BD RID: 445 RVA: 0x000094AC File Offset: 0x000076AC
		// (set) Token: 0x060001BE RID: 446 RVA: 0x000094B4 File Offset: 0x000076B4
		public bool API_HAS_GUARANTEED_R8_SUPPORT { get; private set; }

		// Token: 0x17000042 RID: 66
		// (get) Token: 0x060001BF RID: 447 RVA: 0x000094BD File Offset: 0x000076BD
		// (set) Token: 0x060001C0 RID: 448 RVA: 0x000094C5 File Offset: 0x000076C5
		public bool IsVREnabled { get; private set; }

		// Token: 0x17000043 RID: 67
		// (get) Token: 0x060001C1 RID: 449 RVA: 0x000094CE File Offset: 0x000076CE
		// (set) Token: 0x060001C2 RID: 450 RVA: 0x000094D6 File Offset: 0x000076D6
		public bool Unsupported_Unity_Version { get; private set; }

		// Token: 0x17000044 RID: 68
		// (get) Token: 0x060001C3 RID: 451 RVA: 0x000094DF File Offset: 0x000076DF
		// (set) Token: 0x060001C4 RID: 452 RVA: 0x000094FD File Offset: 0x000076FD
		public static GPUIRuntimeSettings Instance
		{
			get
			{
				if (GPUIRuntimeSettings._instance == null)
				{
					GPUIRuntimeSettings._instance = GPUIRuntimeSettings.GetDefaultGPUIRuntimeSettings();
				}
				return GPUIRuntimeSettings._instance;
			}
			set
			{
				GPUIRuntimeSettings._instance = value;
			}
		}

		// Token: 0x17000045 RID: 69
		// (get) Token: 0x060001C5 RID: 453 RVA: 0x00009505 File Offset: 0x00007705
		public bool IsHDRP
		{
			get
			{
				return this.RenderPipeline == GPUIRenderPipeline.HDRP;
			}
		}

		// Token: 0x17000046 RID: 70
		// (get) Token: 0x060001C6 RID: 454 RVA: 0x00009514 File Offset: 0x00007714
		public bool IsURP
		{
			get
			{
				return this.RenderPipeline == GPUIRenderPipeline.URP;
			}
		}

		// Token: 0x17000047 RID: 71
		// (get) Token: 0x060001C7 RID: 455 RVA: 0x00009523 File Offset: 0x00007723
		public bool IsBuiltInRP
		{
			get
			{
				return this.RenderPipeline == GPUIRenderPipeline.BuiltIn;
			}
		}

		// Token: 0x060001C8 RID: 456 RVA: 0x00009530 File Offset: 0x00007730
		private static GPUIRuntimeSettings GetDefaultGPUIRuntimeSettings()
		{
			GPUIRuntimeSettings gpuiruntimeSettings = null;
			GPUIRuntimeSettingsOverwrite gpuiruntimeSettingsOverwrite = GPUIUtility.FindFirstObjectByType<GPUIRuntimeSettingsOverwrite>();
			if (gpuiruntimeSettingsOverwrite != null && gpuiruntimeSettingsOverwrite.runtimeSettingsOverwrite != null)
			{
				gpuiruntimeSettings = gpuiruntimeSettingsOverwrite.runtimeSettingsOverwrite;
				gpuiruntimeSettings.DetermineOperationMode();
			}
			if (gpuiruntimeSettings == null)
			{
				gpuiruntimeSettings = ScriptableObject.CreateInstance<GPUIRuntimeSettings>();
				gpuiruntimeSettings.DetermineOperationMode();
				gpuiruntimeSettings.SetDefaultValues();
			}
			return gpuiruntimeSettings;
		}

		// Token: 0x060001C9 RID: 457 RVA: 0x00009585 File Offset: 0x00007785
		internal static void OverwriteSettings(GPUIRuntimeSettings overwriteSettings)
		{
			if (overwriteSettings == null)
			{
				return;
			}
			overwriteSettings.DetermineOperationMode();
			GPUIRuntimeSettings._instance = overwriteSettings;
		}

		// Token: 0x060001CA RID: 458 RVA: 0x000095A0 File Offset: 0x000077A0
		public void DetermineRenderPipeline()
		{
			this.RenderPipeline = GPUIRenderPipeline.BuiltIn;
			UniversalRenderPipelineAsset universalRenderPipelineAsset;
			if (GPUIRuntimeSettings.TryGetURPAsset(out universalRenderPipelineAsset))
			{
				this.RenderPipeline = GPUIRenderPipeline.URP;
			}
		}

		// Token: 0x060001CB RID: 459 RVA: 0x000095C8 File Offset: 0x000077C8
		public void DetermineOperationMode()
		{
			this.DetermineRenderPipeline();
			if (!GPUIRuntimeSettings.IsSupportedPlatform())
			{
				return;
			}
			this.GraphicsDeviceType = SystemInfo.graphicsDeviceType;
			int maxComputeBufferInputsFragment = SystemInfo.maxComputeBufferInputsFragment;
			this.DisableShaderBuffers = !GPUIRuntimeSettings.IsSafeAssumeMaxComputeBufferInputsFragmentGt2(Application.platform) && (this.forceDisableShaderBuffers || maxComputeBufferInputsFragment < 4);
			this.DisablePreviousFrameTransformBuffer = this.forceDisableShaderBuffers || maxComputeBufferInputsFragment < 8;
			this.DisablePerInstanceLightProbesBuffer = this.forceDisableShaderBuffers || maxComputeBufferInputsFragment < 8;
			this.DisableInstancingBoundsCalculation = this.forceDisableShaderBuffers || !SystemInfo.supportsAsyncGPUReadback;
			if (this.DisableShaderBuffers)
			{
				Shader.EnableKeyword("GPUI_NO_BUFFER");
			}
			else
			{
				Shader.DisableKeyword("GPUI_NO_BUFFER");
			}
			GraphicsDeviceType graphicsDeviceType;
			if (this.DisableShaderBuffers)
			{
				graphicsDeviceType = this.GraphicsDeviceType;
				this.DisableOcclusionCulling = graphicsDeviceType != GraphicsDeviceType.OpenGLES3 && graphicsDeviceType != GraphicsDeviceType.Vulkan;
			}
			else
			{
				this.DisableOcclusionCulling = false;
			}
			graphicsDeviceType = this.GraphicsDeviceType;
			this.API_HAS_GUARANTEED_R8_SUPPORT = graphicsDeviceType != GraphicsDeviceType.OpenGLES3 && graphicsDeviceType != GraphicsDeviceType.Vulkan;
			int num = SystemInfo.maxComputeWorkGroupSizeX;
			if (this.overrideComputeWorkGroupSize)
			{
				num = Mathf.Max((int)this.computeWorkGroupSizeOverride, 64);
			}
			if (num >= 512)
			{
				this.ComputeWorkGroupSize = GPUIMaxComputeWorkGroupSize.x512;
				this.ComputeThreadCount = 512f;
				this.ComputeThreadCount2D = 16f;
				this.ComputeThreadCount3D = 8f;
				this.ComputeThreadCountHeavy = 256f;
				Shader.EnableKeyword("GPUI_THREAD_SIZE_512");
				Shader.EnableKeyword("GPUI_THREAD_SIZE_HEAVY_256");
				Shader.DisableKeyword("GPUI_THREAD_SIZE_256");
			}
			else if (num >= 256)
			{
				this.ComputeWorkGroupSize = GPUIMaxComputeWorkGroupSize.x256;
				this.ComputeThreadCount = 256f;
				this.ComputeThreadCount2D = 16f;
				this.ComputeThreadCount3D = 4f;
				this.ComputeThreadCountHeavy = 256f;
				Shader.DisableKeyword("GPUI_THREAD_SIZE_512");
				Shader.EnableKeyword("GPUI_THREAD_SIZE_HEAVY_256");
				Shader.EnableKeyword("GPUI_THREAD_SIZE_256");
			}
			else
			{
				this.ComputeWorkGroupSize = GPUIMaxComputeWorkGroupSize.x64;
				this.ComputeThreadCount = 64f;
				this.ComputeThreadCount2D = 8f;
				this.ComputeThreadCount3D = 4f;
				this.ComputeThreadCountHeavy = 64f;
				Shader.DisableKeyword("GPUI_THREAD_SIZE_512");
				Shader.DisableKeyword("GPUI_THREAD_SIZE_HEAVY_256");
				Shader.DisableKeyword("GPUI_THREAD_SIZE_256");
			}
			this.TextureMaxSize = SystemInfo.maxTextureSize;
			this.ClearEmptyBillboardAssets();
			this.MaxBufferSize = SystemInfo.maxGraphicsBufferSize / 64L;
			graphicsDeviceType = SystemInfo.graphicsDeviceType;
			this.ReversedZBuffer = graphicsDeviceType == GraphicsDeviceType.OpenGLCore || graphicsDeviceType == GraphicsDeviceType.OpenGLES3;
			GPUIConstants.CS_THREAD_COUNT = this.ComputeThreadCount;
			GPUIConstants.CS_THREAD_COUNT_HEAVY = this.ComputeThreadCountHeavy;
			GPUIConstants.CS_THREAD_COUNT_2D = this.ComputeThreadCount2D;
			GPUIConstants.CS_THREAD_COUNT_3D = this.ComputeThreadCount3D;
			GPUIConstants.TEXTURE_MAX_SIZE = this.TextureMaxSize;
			GPUIConstants.MAX_BUFFER_SIZE = this.MaxBufferSize;
		}

		// Token: 0x060001CC RID: 460 RVA: 0x00009874 File Offset: 0x00007A74
		public static bool IsSafeAssumeMaxComputeBufferInputsFragmentGt2(RuntimePlatform platform)
		{
			if (platform <= RuntimePlatform.LinuxPlayer)
			{
				if (platform > RuntimePlatform.WindowsPlayer && platform != RuntimePlatform.WindowsEditor && platform != RuntimePlatform.LinuxPlayer)
				{
					return false;
				}
			}
			else if (platform <= RuntimePlatform.PS4)
			{
				if (platform != RuntimePlatform.LinuxEditor && platform != RuntimePlatform.PS4)
				{
					return false;
				}
			}
			else if (platform != RuntimePlatform.XboxOne && platform - RuntimePlatform.GameCoreXboxSeries > 2)
			{
				return false;
			}
			return true;
		}

		// Token: 0x060001CD RID: 461 RVA: 0x000098AC File Offset: 0x00007AAC
		public void SetDefaultValues()
		{
			RuntimePlatform platform = Application.platform;
			if (platform == RuntimePlatform.Android || platform == RuntimePlatform.IPhonePlayer || platform == RuntimePlatform.Switch || platform == RuntimePlatform.WebGLPlayer)
			{
				this.occlusionCullingCondition = GPUIOcclusionCullingCondition.IfDepthAvailable;
				return;
			}
			this.occlusionCullingCondition = GPUIOcclusionCullingCondition.Always;
		}

		// Token: 0x060001CE RID: 462 RVA: 0x000098E4 File Offset: 0x00007AE4
		public void ClearEmptyBillboardAssets()
		{
			if (this.billboardAssets == null)
			{
				this.billboardAssets = new List<GPUIBillboard>();
			}
			for (int i = 0; i < this.billboardAssets.Count; i++)
			{
				if (this.billboardAssets[i] == null)
				{
					this.billboardAssets.RemoveAt(i);
					i--;
				}
			}
		}

		// Token: 0x060001CF RID: 463 RVA: 0x0000993E File Offset: 0x00007B3E
		public void SetRuntimeSettings()
		{
			this.IsVREnabled = XRSettings.enabled;
			this.DetermineRenderPipeline();
		}

		// Token: 0x060001D0 RID: 464 RVA: 0x00009954 File Offset: 0x00007B54
		public static bool IsSupportedPlatform()
		{
			if (GPUIRuntimeSettings._isSupportedPlatform != null)
			{
				return GPUIRuntimeSettings._isSupportedPlatform.Value;
			}
			GPUIRuntimeSettings._isSupportedPlatform = new bool?(false);
			if (!SystemInfo.supportsInstancing)
			{
				Debug.LogError(GPUIConstants.LOG_PREFIX + "Current platform does not support GPU instancing.");
				return false;
			}
			if (!SystemInfo.supportsComputeShaders)
			{
				Debug.LogError(GPUIConstants.LOG_PREFIX + "Current platform does not support Compute Shaders.");
				return false;
			}
			if (SystemInfo.graphicsShaderLevel < 35)
			{
				Debug.LogError(GPUIConstants.LOG_PREFIX + "Current platform's Graphics Shader Level is under 35. Current shader level: " + SystemInfo.graphicsShaderLevel.ToString());
				return false;
			}
			if (SystemInfo.maxComputeWorkGroupSize < 64)
			{
				Debug.LogError(GPUIConstants.LOG_PREFIX + "Current platform's Max. Compute Work Group Size is under 64. Current Max. Compute Work Group Size: " + SystemInfo.maxComputeWorkGroupSize.ToString());
				return false;
			}
			GPUIRuntimeSettings._isSupportedPlatform = new bool?(true);
			return true;
		}

		// Token: 0x060001D1 RID: 465 RVA: 0x00009A24 File Offset: 0x00007C24
		public float GetDefaultShadowDistance()
		{
			if (this.RenderPipeline != GPUIRenderPipeline.URP)
			{
				return QualitySettings.shadowDistance;
			}
			UniversalRenderPipelineAsset universalRenderPipelineAsset;
			if (GPUIRuntimeSettings.TryGetURPAsset(out universalRenderPipelineAsset))
			{
				return universalRenderPipelineAsset.shadowDistance;
			}
			return QualitySettings.shadowDistance;
		}

		// Token: 0x060001D2 RID: 466 RVA: 0x00009A59 File Offset: 0x00007C59
		public static bool TryGetRenderPipelineAsset(out RenderPipelineAsset renderPipelineAsset)
		{
			if (QualitySettings.renderPipeline != null)
			{
				renderPipelineAsset = QualitySettings.renderPipeline;
				return true;
			}
			if (GraphicsSettings.defaultRenderPipeline != null)
			{
				renderPipelineAsset = GraphicsSettings.defaultRenderPipeline;
				return true;
			}
			renderPipelineAsset = null;
			return false;
		}

		// Token: 0x060001D3 RID: 467 RVA: 0x00009A8C File Offset: 0x00007C8C
		public static bool TryGetURPAsset(out UniversalRenderPipelineAsset urpAsset)
		{
			RenderPipelineAsset renderPipelineAsset;
			if (GPUIRuntimeSettings.TryGetRenderPipelineAsset(out renderPipelineAsset))
			{
				UniversalRenderPipelineAsset universalRenderPipelineAsset = renderPipelineAsset as UniversalRenderPipelineAsset;
				if (universalRenderPipelineAsset != null)
				{
					urpAsset = universalRenderPipelineAsset;
					return true;
				}
			}
			urpAsset = null;
			return false;
		}

		// Token: 0x060001D4 RID: 468 RVA: 0x000028F4 File Offset: 0x00000AF4
		public static bool IsAdaptiveProbeVolumesEnabled()
		{
			return false;
		}

		// Token: 0x0400010C RID: 268
		[SerializeField]
		public GPUICameraLoadingType cameraLoadingType;

		// Token: 0x0400010D RID: 269
		[SerializeField]
		public GPUIOcclusionCullingCondition occlusionCullingCondition;

		// Token: 0x0400010E RID: 270
		[SerializeField]
		public GPUIOcclusionCullingData.GPUIOcclusionCullingMode occlusionCullingMode;

		// Token: 0x0400010F RID: 271
		[SerializeField]
		public Vector3 instancingBoundsSize = new Vector3(1000f, 1000f, 1000f);

		// Token: 0x04000110 RID: 272
		[SerializeField]
		public float defaultHDRPShadowDistance = 250f;

		// Token: 0x04000111 RID: 273
		[SerializeField]
		public List<GPUIBillboard> billboardAssets;

		// Token: 0x04000112 RID: 274
		[SerializeField]
		public bool loadShadersFromAddressables;

		// Token: 0x04000113 RID: 275
		[SerializeField]
		public bool loadResourcesFromAddressables;

		// Token: 0x04000114 RID: 276
		[SerializeField]
		public bool forceDisableShaderBuffers;

		// Token: 0x04000115 RID: 277
		[SerializeField]
		public bool overrideComputeWorkGroupSize;

		// Token: 0x04000116 RID: 278
		[SerializeField]
		public GPUIMaxComputeWorkGroupSize computeWorkGroupSizeOverride = GPUIMaxComputeWorkGroupSize.x512;

		// Token: 0x04000129 RID: 297
		private static bool? _isSupportedPlatform;

		// Token: 0x0400012A RID: 298
		private static GPUIRuntimeSettings _instance;
	}
}
