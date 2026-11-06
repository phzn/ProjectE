using System;
using System.Diagnostics;
using UnityEngine.Rendering.Universal.Internal;

namespace UnityEngine.Rendering.Universal
{
	// Token: 0x020000C5 RID: 197
	[DisallowMultipleRendererFeature("Decal")]
	[Tooltip("With this Renderer Feature, Unity can project specific Materials (decals) onto other objects in the Scene.")]
	internal class DecalRendererFeature : ScriptableRendererFeature
	{
		// Token: 0x17000184 RID: 388
		// (get) Token: 0x06000616 RID: 1558 RVA: 0x00025681 File Offset: 0x00023881
		private static SharedDecalEntityManager sharedDecalEntityManager { get; } = new SharedDecalEntityManager();

		// Token: 0x17000185 RID: 389
		// (get) Token: 0x06000617 RID: 1559 RVA: 0x00025688 File Offset: 0x00023888
		internal ref DecalSettings settings
		{
			get
			{
				return ref this.m_Settings;
			}
		}

		// Token: 0x17000186 RID: 390
		// (get) Token: 0x06000618 RID: 1560 RVA: 0x00025690 File Offset: 0x00023890
		internal bool intermediateRendering
		{
			get
			{
				return this.m_Technique == DecalTechnique.DBuffer;
			}
		}

		// Token: 0x17000187 RID: 391
		// (get) Token: 0x06000619 RID: 1561 RVA: 0x0002569B File Offset: 0x0002389B
		internal bool requiresDecalLayers
		{
			get
			{
				return this.m_Settings.decalLayers;
			}
		}

		// Token: 0x17000188 RID: 392
		// (get) Token: 0x0600061A RID: 1562 RVA: 0x000256A8 File Offset: 0x000238A8
		internal static bool isGLDevice
		{
			get
			{
				return SystemInfo.graphicsDeviceType == GraphicsDeviceType.OpenGLES2 || SystemInfo.graphicsDeviceType == GraphicsDeviceType.OpenGLES3 || SystemInfo.graphicsDeviceType == GraphicsDeviceType.OpenGLCore;
			}
		}

		// Token: 0x0600061B RID: 1563 RVA: 0x000256C6 File Offset: 0x000238C6
		public override void Create()
		{
			this.m_DecalPreviewPass = new DecalPreviewPass();
			this.m_RecreateSystems = true;
		}

		// Token: 0x0600061C RID: 1564 RVA: 0x000256DC File Offset: 0x000238DC
		internal override bool RequireRenderingLayers(bool isDeferred, bool needsGBufferAccurateNormals, out RenderingLayerUtils.Event atEvent, out RenderingLayerUtils.MaskSize maskSize)
		{
			bool isPlaying = Application.isPlaying;
			DecalTechnique technique = this.GetTechnique(isDeferred, needsGBufferAccurateNormals, isPlaying);
			atEvent = ((technique == DecalTechnique.DBuffer) ? RenderingLayerUtils.Event.DepthNormalPrePass : RenderingLayerUtils.Event.Opaque);
			maskSize = RenderingLayerUtils.MaskSize.Bits8;
			return this.requiresDecalLayers;
		}

		// Token: 0x0600061D RID: 1565 RVA: 0x0002570D File Offset: 0x0002390D
		internal DBufferSettings GetDBufferSettings()
		{
			if (this.m_Settings.technique == DecalTechniqueOption.Automatic)
			{
				return new DBufferSettings
				{
					surfaceData = DecalSurfaceData.AlbedoNormalMAOS
				};
			}
			return this.m_Settings.dBufferSettings;
		}

		// Token: 0x0600061E RID: 1566 RVA: 0x00025734 File Offset: 0x00023934
		internal DecalScreenSpaceSettings GetScreenSpaceSettings()
		{
			if (this.m_Settings.technique == DecalTechniqueOption.Automatic)
			{
				return new DecalScreenSpaceSettings
				{
					normalBlend = DecalNormalBlend.Low
				};
			}
			return this.m_Settings.screenSpaceSettings;
		}

		// Token: 0x0600061F RID: 1567 RVA: 0x0002575C File Offset: 0x0002395C
		internal DecalTechnique GetTechnique(ScriptableRendererData renderer)
		{
			UniversalRendererData universalRendererData = renderer as UniversalRendererData;
			if (universalRendererData == null)
			{
				Debug.LogError("Only universal renderer supports Decal renderer feature.");
				return DecalTechnique.Invalid;
			}
			bool flag = universalRendererData.renderingMode == RenderingMode.Deferred;
			return this.GetTechnique(flag, universalRendererData.accurateGbufferNormals, true);
		}

		// Token: 0x06000620 RID: 1568 RVA: 0x000257A0 File Offset: 0x000239A0
		internal DecalTechnique GetTechnique(ScriptableRenderer renderer)
		{
			UniversalRenderer universalRenderer = renderer as UniversalRenderer;
			if (universalRenderer == null)
			{
				Debug.LogError("Only universal renderer supports Decal renderer feature.");
				return DecalTechnique.Invalid;
			}
			bool flag = universalRenderer.renderingModeActual == RenderingMode.Deferred;
			return this.GetTechnique(flag, universalRenderer.accurateGbufferNormals, true);
		}

		// Token: 0x06000621 RID: 1569 RVA: 0x000257DC File Offset: 0x000239DC
		internal DecalTechnique GetTechnique(bool isDeferred, bool needsGBufferAccurateNormals, bool checkForInvalidTechniques = true)
		{
			DecalTechnique decalTechnique = DecalTechnique.Invalid;
			switch (this.m_Settings.technique)
			{
			case DecalTechniqueOption.Automatic:
				if (this.IsAutomaticDBuffer() || (isDeferred && needsGBufferAccurateNormals))
				{
					decalTechnique = DecalTechnique.DBuffer;
				}
				else if (isDeferred)
				{
					decalTechnique = DecalTechnique.GBuffer;
				}
				else
				{
					decalTechnique = DecalTechnique.ScreenSpace;
				}
				break;
			case DecalTechniqueOption.DBuffer:
				decalTechnique = DecalTechnique.DBuffer;
				break;
			case DecalTechniqueOption.ScreenSpace:
				if (isDeferred)
				{
					decalTechnique = DecalTechnique.GBuffer;
				}
				else
				{
					decalTechnique = DecalTechnique.ScreenSpace;
				}
				break;
			}
			if (!checkForInvalidTechniques)
			{
				return decalTechnique;
			}
			if (SystemInfo.graphicsDeviceType == GraphicsDeviceType.OpenGLES2)
			{
				Debug.LogError("Decals are not supported with OpenGLES2.");
				return DecalTechnique.Invalid;
			}
			if (decalTechnique == DecalTechnique.DBuffer && DecalRendererFeature.isGLDevice)
			{
				Debug.LogError("Decal DBuffer technique is not supported with OpenGL.");
				return DecalTechnique.Invalid;
			}
			bool flag = SystemInfo.supportedRenderTargetCount >= 4;
			if (decalTechnique == DecalTechnique.DBuffer && !flag)
			{
				Debug.LogError("Decal DBuffer technique requires MRT4 support.");
				return DecalTechnique.Invalid;
			}
			if (decalTechnique == DecalTechnique.GBuffer && !flag)
			{
				Debug.LogError("Decal useGBuffer option requires MRT4 support.");
				return DecalTechnique.Invalid;
			}
			return decalTechnique;
		}

		// Token: 0x06000622 RID: 1570 RVA: 0x00025897 File Offset: 0x00023A97
		private bool IsAutomaticDBuffer()
		{
			return Application.platform != RuntimePlatform.WebGLPlayer && !GraphicsSettings.HasShaderDefine(BuiltinShaderDefine.SHADER_API_MOBILE);
		}

		// Token: 0x06000623 RID: 1571 RVA: 0x000258B0 File Offset: 0x00023AB0
		private bool RecreateSystemsIfNeeded(ScriptableRenderer renderer, in CameraData cameraData)
		{
			if (!this.m_RecreateSystems)
			{
				return true;
			}
			this.m_Technique = this.GetTechnique(renderer);
			if (this.m_Technique == DecalTechnique.Invalid)
			{
				return false;
			}
			this.m_DBufferSettings = this.GetDBufferSettings();
			this.m_ScreenSpaceSettings = this.GetScreenSpaceSettings();
			this.m_CopyDepthMaterial = CoreUtils.CreateEngineMaterial(this.m_CopyDepthPS);
			this.m_DBufferClearMaterial = CoreUtils.CreateEngineMaterial(this.m_DBufferClear);
			if (this.m_DecalEntityManager == null)
			{
				this.m_DecalEntityManager = DecalRendererFeature.sharedDecalEntityManager.Get();
			}
			this.m_DecalUpdateCachedSystem = new DecalUpdateCachedSystem(this.m_DecalEntityManager);
			this.m_DecalUpdateCulledSystem = new DecalUpdateCulledSystem(this.m_DecalEntityManager);
			this.m_DecalCreateDrawCallSystem = new DecalCreateDrawCallSystem(this.m_DecalEntityManager, this.m_Settings.maxDrawDistance);
			if (this.intermediateRendering)
			{
				this.m_DecalUpdateCullingGroupSystem = new DecalUpdateCullingGroupSystem(this.m_DecalEntityManager, this.m_Settings.maxDrawDistance);
			}
			else
			{
				this.m_DecalSkipCulledSystem = new DecalSkipCulledSystem(this.m_DecalEntityManager);
			}
			this.m_DrawErrorSystem = new DecalDrawErrorSystem(this.m_DecalEntityManager, this.m_Technique);
			UniversalRenderer universalRenderer = renderer as UniversalRenderer;
			switch (this.m_Technique)
			{
			case DecalTechnique.DBuffer:
				this.m_CopyDepthPass = new CopyDepthPass((RenderPassEvent)201, this.m_CopyDepthMaterial, false, false, false);
				this.m_DecalDrawDBufferSystem = new DecalDrawDBufferSystem(this.m_DecalEntityManager);
				this.m_DBufferRenderPass = new DBufferRenderPass(this.m_DBufferClearMaterial, this.m_DBufferSettings, this.m_DecalDrawDBufferSystem, this.m_Settings.decalLayers);
				this.m_DecalDrawForwardEmissiveSystem = new DecalDrawFowardEmissiveSystem(this.m_DecalEntityManager);
				this.m_ForwardEmissivePass = new DecalForwardEmissivePass(this.m_DecalDrawForwardEmissiveSystem);
				break;
			case DecalTechnique.ScreenSpace:
				this.m_DecalDrawScreenSpaceSystem = new DecalDrawScreenSpaceSystem(this.m_DecalEntityManager);
				this.m_ScreenSpaceDecalRenderPass = new DecalScreenSpaceRenderPass(this.m_ScreenSpaceSettings, this.intermediateRendering ? this.m_DecalDrawScreenSpaceSystem : null, this.m_Settings.decalLayers);
				break;
			case DecalTechnique.GBuffer:
				this.m_DeferredLights = universalRenderer.deferredLights;
				this.m_DrawGBufferSystem = new DecalDrawGBufferSystem(this.m_DecalEntityManager);
				this.m_GBufferRenderPass = new DecalGBufferRenderPass(this.m_ScreenSpaceSettings, this.intermediateRendering ? this.m_DrawGBufferSystem : null, this.m_Settings.decalLayers);
				break;
			}
			this.m_RecreateSystems = false;
			return true;
		}

		// Token: 0x06000624 RID: 1572 RVA: 0x00025AF0 File Offset: 0x00023CF0
		public override void OnCameraPreCull(ScriptableRenderer renderer, in CameraData cameraData)
		{
			if (cameraData.cameraType == CameraType.Preview)
			{
				return;
			}
			if (!this.RecreateSystemsIfNeeded(renderer, in cameraData))
			{
				return;
			}
			this.m_DecalEntityManager.Update();
			this.m_DecalUpdateCachedSystem.Execute();
			if (this.intermediateRendering)
			{
				this.m_DecalUpdateCullingGroupSystem.Execute(cameraData.camera);
			}
			else
			{
				this.m_DecalSkipCulledSystem.Execute(cameraData.camera);
				this.m_DecalCreateDrawCallSystem.Execute();
				if (this.m_Technique == DecalTechnique.ScreenSpace)
				{
					this.m_DecalDrawScreenSpaceSystem.Execute(in cameraData);
				}
				else if (this.m_Technique == DecalTechnique.GBuffer)
				{
					this.m_DrawGBufferSystem.Execute(in cameraData);
				}
			}
			this.m_DrawErrorSystem.Execute(in cameraData);
		}

		// Token: 0x06000625 RID: 1573 RVA: 0x00025B98 File Offset: 0x00023D98
		public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
		{
			if (UniversalRenderer.IsOffscreenDepthTexture(in renderingData.cameraData))
			{
				return;
			}
			if (renderingData.cameraData.cameraType == CameraType.Preview)
			{
				renderer.EnqueuePass(this.m_DecalPreviewPass);
				return;
			}
			if (!this.RecreateSystemsIfNeeded(renderer, in renderingData.cameraData))
			{
				return;
			}
			if (this.intermediateRendering)
			{
				this.m_DecalUpdateCulledSystem.Execute();
				this.m_DecalCreateDrawCallSystem.Execute();
			}
			switch (this.m_Technique)
			{
			case DecalTechnique.DBuffer:
				renderer.EnqueuePass(this.m_CopyDepthPass);
				renderer.EnqueuePass(this.m_DBufferRenderPass);
				renderer.EnqueuePass(this.m_ForwardEmissivePass);
				return;
			case DecalTechnique.ScreenSpace:
				renderer.EnqueuePass(this.m_ScreenSpaceDecalRenderPass);
				return;
			case DecalTechnique.GBuffer:
				this.m_GBufferRenderPass.Setup(this.m_DeferredLights);
				renderer.EnqueuePass(this.m_GBufferRenderPass);
				return;
			default:
				return;
			}
		}

		// Token: 0x06000626 RID: 1574 RVA: 0x00025C67 File Offset: 0x00023E67
		internal override bool SupportsNativeRenderPass()
		{
			return this.m_Technique == DecalTechnique.GBuffer || this.m_Technique == DecalTechnique.ScreenSpace;
		}

		// Token: 0x06000627 RID: 1575 RVA: 0x00025C80 File Offset: 0x00023E80
		public override void SetupRenderPasses(ScriptableRenderer renderer, in RenderingData renderingData)
		{
			if (UniversalRenderer.IsOffscreenDepthTexture(in renderingData.cameraData))
			{
				return;
			}
			if (this.m_Technique != DecalTechnique.DBuffer)
			{
				if (this.m_Technique == DecalTechnique.GBuffer && this.m_DeferredLights.UseRenderPass)
				{
					this.m_GBufferRenderPass.Configure(null, renderingData.cameraData.cameraTargetDescriptor);
				}
				return;
			}
			this.m_DBufferRenderPass.Setup(in renderingData.cameraData);
			UniversalRenderer universalRenderer = renderer as UniversalRenderer;
			if (universalRenderer.renderingModeActual == RenderingMode.Deferred)
			{
				this.m_DBufferRenderPass.Setup(in renderingData.cameraData, renderer.cameraDepthTargetHandle);
				this.m_CopyDepthPass.Setup(renderer.cameraDepthTargetHandle, universalRenderer.m_DepthTexture);
				return;
			}
			this.m_DBufferRenderPass.Setup(in renderingData.cameraData);
			this.m_CopyDepthPass.Setup(universalRenderer.m_DepthTexture, this.m_DBufferRenderPass.dBufferDepth);
			this.m_CopyDepthPass.CopyToDepth = true;
			this.m_CopyDepthPass.MssaSamples = 1;
		}

		// Token: 0x06000628 RID: 1576 RVA: 0x00025D6C File Offset: 0x00023F6C
		protected override void Dispose(bool disposing)
		{
			DBufferRenderPass dbufferRenderPass = this.m_DBufferRenderPass;
			if (dbufferRenderPass != null)
			{
				dbufferRenderPass.Dispose();
			}
			CoreUtils.Destroy(this.m_CopyDepthMaterial);
			CoreUtils.Destroy(this.m_DBufferClearMaterial);
			if (this.m_DecalEntityManager != null)
			{
				this.m_DecalEntityManager = null;
				DecalRendererFeature.sharedDecalEntityManager.Release(this.m_DecalEntityManager);
			}
		}

		// Token: 0x06000629 RID: 1577 RVA: 0x00025DBF File Offset: 0x00023FBF
		[Conditional("ADAPTIVE_PERFORMANCE_4_0_0_OR_NEWER")]
		private void ChangeAdaptivePerformanceDrawDistances()
		{
		}

		// Token: 0x040004EC RID: 1260
		[SerializeField]
		private DecalSettings m_Settings = new DecalSettings();

		// Token: 0x040004ED RID: 1261
		[SerializeField]
		[HideInInspector]
		[Reload("Shaders/Utils/CopyDepth.shader", ReloadAttribute.Package.Root)]
		private Shader m_CopyDepthPS;

		// Token: 0x040004EE RID: 1262
		[SerializeField]
		[HideInInspector]
		[Reload("Runtime/Decal/DBuffer/DBufferClear.shader", ReloadAttribute.Package.Root)]
		private Shader m_DBufferClear;

		// Token: 0x040004EF RID: 1263
		private DecalTechnique m_Technique;

		// Token: 0x040004F0 RID: 1264
		private DBufferSettings m_DBufferSettings;

		// Token: 0x040004F1 RID: 1265
		private DecalScreenSpaceSettings m_ScreenSpaceSettings;

		// Token: 0x040004F2 RID: 1266
		private bool m_RecreateSystems;

		// Token: 0x040004F3 RID: 1267
		private CopyDepthPass m_CopyDepthPass;

		// Token: 0x040004F4 RID: 1268
		private DecalPreviewPass m_DecalPreviewPass;

		// Token: 0x040004F5 RID: 1269
		private Material m_CopyDepthMaterial;

		// Token: 0x040004F6 RID: 1270
		private DecalEntityManager m_DecalEntityManager;

		// Token: 0x040004F7 RID: 1271
		private DecalUpdateCachedSystem m_DecalUpdateCachedSystem;

		// Token: 0x040004F8 RID: 1272
		private DecalUpdateCullingGroupSystem m_DecalUpdateCullingGroupSystem;

		// Token: 0x040004F9 RID: 1273
		private DecalUpdateCulledSystem m_DecalUpdateCulledSystem;

		// Token: 0x040004FA RID: 1274
		private DecalCreateDrawCallSystem m_DecalCreateDrawCallSystem;

		// Token: 0x040004FB RID: 1275
		private DecalDrawErrorSystem m_DrawErrorSystem;

		// Token: 0x040004FC RID: 1276
		private DBufferRenderPass m_DBufferRenderPass;

		// Token: 0x040004FD RID: 1277
		private DecalForwardEmissivePass m_ForwardEmissivePass;

		// Token: 0x040004FE RID: 1278
		private DecalDrawDBufferSystem m_DecalDrawDBufferSystem;

		// Token: 0x040004FF RID: 1279
		private DecalDrawFowardEmissiveSystem m_DecalDrawForwardEmissiveSystem;

		// Token: 0x04000500 RID: 1280
		private Material m_DBufferClearMaterial;

		// Token: 0x04000501 RID: 1281
		private DecalScreenSpaceRenderPass m_ScreenSpaceDecalRenderPass;

		// Token: 0x04000502 RID: 1282
		private DecalDrawScreenSpaceSystem m_DecalDrawScreenSpaceSystem;

		// Token: 0x04000503 RID: 1283
		private DecalSkipCulledSystem m_DecalSkipCulledSystem;

		// Token: 0x04000504 RID: 1284
		private DecalGBufferRenderPass m_GBufferRenderPass;

		// Token: 0x04000505 RID: 1285
		private DecalDrawGBufferSystem m_DrawGBufferSystem;

		// Token: 0x04000506 RID: 1286
		private DeferredLights m_DeferredLights;
	}
}
