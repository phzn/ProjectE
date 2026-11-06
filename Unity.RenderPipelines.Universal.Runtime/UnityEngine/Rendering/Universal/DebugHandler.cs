using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;

namespace UnityEngine.Rendering.Universal
{
	// Token: 0x0200005C RID: 92
	internal class DebugHandler : IDebugDisplaySettingsQuery
	{
		// Token: 0x1700010B RID: 267
		// (get) Token: 0x06000361 RID: 865 RVA: 0x00014AFF File Offset: 0x00012CFF
		private DebugDisplaySettingsLighting LightingSettings
		{
			get
			{
				return this.m_DebugDisplaySettings.lightingSettings;
			}
		}

		// Token: 0x1700010C RID: 268
		// (get) Token: 0x06000362 RID: 866 RVA: 0x00014B0C File Offset: 0x00012D0C
		private DebugDisplaySettingsMaterial MaterialSettings
		{
			get
			{
				return this.m_DebugDisplaySettings.materialSettings;
			}
		}

		// Token: 0x1700010D RID: 269
		// (get) Token: 0x06000363 RID: 867 RVA: 0x00014B19 File Offset: 0x00012D19
		private DebugDisplaySettingsRendering RenderingSettings
		{
			get
			{
				return this.m_DebugDisplaySettings.renderingSettings;
			}
		}

		// Token: 0x1700010E RID: 270
		// (get) Token: 0x06000364 RID: 868 RVA: 0x00014B26 File Offset: 0x00012D26
		public bool AreAnySettingsActive
		{
			get
			{
				return this.m_DebugDisplaySettings.AreAnySettingsActive;
			}
		}

		// Token: 0x1700010F RID: 271
		// (get) Token: 0x06000365 RID: 869 RVA: 0x00014B33 File Offset: 0x00012D33
		public bool IsPostProcessingAllowed
		{
			get
			{
				return this.m_DebugDisplaySettings.IsPostProcessingAllowed;
			}
		}

		// Token: 0x17000110 RID: 272
		// (get) Token: 0x06000366 RID: 870 RVA: 0x00014B40 File Offset: 0x00012D40
		public bool IsLightingActive
		{
			get
			{
				return this.m_DebugDisplaySettings.IsLightingActive;
			}
		}

		// Token: 0x17000111 RID: 273
		// (get) Token: 0x06000367 RID: 871 RVA: 0x00014B50 File Offset: 0x00012D50
		internal bool IsActiveModeUnsupportedForDeferred
		{
			get
			{
				return this.m_DebugDisplaySettings.lightingSettings.lightingDebugMode != DebugLightingMode.None || this.m_DebugDisplaySettings.lightingSettings.lightingFeatureFlags != DebugLightingFeatureFlags.None || this.m_DebugDisplaySettings.renderingSettings.sceneOverrideMode != DebugSceneOverrideMode.None || this.m_DebugDisplaySettings.materialSettings.materialDebugMode != DebugMaterialMode.None || this.m_DebugDisplaySettings.materialSettings.vertexAttributeDebugMode != DebugVertexAttributeMode.None || this.m_DebugDisplaySettings.materialSettings.materialValidationMode > DebugMaterialValidationMode.None;
			}
		}

		// Token: 0x06000368 RID: 872 RVA: 0x00014BCC File Offset: 0x00012DCC
		public bool TryGetScreenClearColor(ref Color color)
		{
			return this.m_DebugDisplaySettings.TryGetScreenClearColor(ref color);
		}

		// Token: 0x17000112 RID: 274
		// (get) Token: 0x06000369 RID: 873 RVA: 0x00014BDA File Offset: 0x00012DDA
		internal Material ReplacementMaterial
		{
			get
			{
				return this.m_ReplacementMaterial;
			}
		}

		// Token: 0x17000113 RID: 275
		// (get) Token: 0x0600036A RID: 874 RVA: 0x00014BE2 File Offset: 0x00012DE2
		internal UniversalRenderPipelineDebugDisplaySettings DebugDisplaySettings
		{
			get
			{
				return this.m_DebugDisplaySettings;
			}
		}

		// Token: 0x17000114 RID: 276
		// (get) Token: 0x0600036B RID: 875 RVA: 0x00014BEA File Offset: 0x00012DEA
		internal ref RTHandle DebugScreenColorHandle
		{
			get
			{
				return ref this.m_DebugScreenColorHandle;
			}
		}

		// Token: 0x17000115 RID: 277
		// (get) Token: 0x0600036C RID: 876 RVA: 0x00014BF2 File Offset: 0x00012DF2
		internal ref RTHandle DebugScreenDepthHandle
		{
			get
			{
				return ref this.m_DebugScreenDepthHandle;
			}
		}

		// Token: 0x17000116 RID: 278
		// (get) Token: 0x0600036D RID: 877 RVA: 0x00014BFA File Offset: 0x00012DFA
		internal HDRDebugViewPass hdrDebugViewPass
		{
			get
			{
				return this.m_HDRDebugViewPass;
			}
		}

		// Token: 0x0600036E RID: 878 RVA: 0x00014C02 File Offset: 0x00012E02
		internal bool HDRDebugViewIsActive(ref CameraData cameraData)
		{
			return this.DebugDisplaySettings.lightingSettings.hdrDebugMode != HDRDebugMode.None && cameraData.resolveFinalTarget;
		}

		// Token: 0x0600036F RID: 879 RVA: 0x00014C1E File Offset: 0x00012E1E
		internal bool WriteToDebugScreenTexture(ref CameraData cameraData)
		{
			return this.HDRDebugViewIsActive(ref cameraData);
		}

		// Token: 0x17000117 RID: 279
		// (get) Token: 0x06000370 RID: 880 RVA: 0x00014C28 File Offset: 0x00012E28
		internal bool IsScreenClearNeeded
		{
			get
			{
				Color black = Color.black;
				return this.TryGetScreenClearColor(ref black);
			}
		}

		// Token: 0x17000118 RID: 280
		// (get) Token: 0x06000371 RID: 881 RVA: 0x00014C43 File Offset: 0x00012E43
		internal bool IsRenderPassSupported
		{
			get
			{
				return this.RenderingSettings.sceneOverrideMode == DebugSceneOverrideMode.None || this.RenderingSettings.sceneOverrideMode == DebugSceneOverrideMode.Overdraw;
			}
		}

		// Token: 0x06000372 RID: 882 RVA: 0x00014C64 File Offset: 0x00012E64
		internal DebugHandler(ScriptableRendererData scriptableRendererData)
		{
			Shader debugReplacementPS = scriptableRendererData.debugShaders.debugReplacementPS;
			Shader hdrDebugViewPS = scriptableRendererData.debugShaders.hdrDebugViewPS;
			this.m_DebugDisplaySettings = DebugDisplaySettings<UniversalRenderPipelineDebugDisplaySettings>.Instance;
			this.m_ReplacementMaterial = ((debugReplacementPS == null) ? null : CoreUtils.CreateEngineMaterial(debugReplacementPS));
			this.m_HDRDebugViewMaterial = ((hdrDebugViewPS == null) ? null : CoreUtils.CreateEngineMaterial(hdrDebugViewPS));
			this.m_HDRDebugViewPass = new HDRDebugViewPass(this.m_HDRDebugViewMaterial);
		}

		// Token: 0x06000373 RID: 883 RVA: 0x00014CDC File Offset: 0x00012EDC
		public void Dispose()
		{
			this.m_HDRDebugViewPass.Dispose();
			RTHandle debugScreenColorHandle = this.m_DebugScreenColorHandle;
			if (debugScreenColorHandle != null)
			{
				debugScreenColorHandle.Release();
			}
			RTHandle debugScreenDepthHandle = this.m_DebugScreenDepthHandle;
			if (debugScreenDepthHandle != null)
			{
				debugScreenDepthHandle.Release();
			}
			CoreUtils.Destroy(this.m_HDRDebugViewMaterial);
			CoreUtils.Destroy(this.m_ReplacementMaterial);
		}

		// Token: 0x06000374 RID: 884 RVA: 0x00014D2C File Offset: 0x00012F2C
		internal bool IsActiveForCamera(ref CameraData cameraData)
		{
			return !cameraData.isPreviewCamera && this.AreAnySettingsActive;
		}

		// Token: 0x06000375 RID: 885 RVA: 0x00014D40 File Offset: 0x00012F40
		internal bool TryGetFullscreenDebugMode(out DebugFullScreenMode debugFullScreenMode)
		{
			int num;
			return this.TryGetFullscreenDebugMode(out debugFullScreenMode, out num);
		}

		// Token: 0x06000376 RID: 886 RVA: 0x00014D56 File Offset: 0x00012F56
		internal bool TryGetFullscreenDebugMode(out DebugFullScreenMode debugFullScreenMode, out int textureHeightPercent)
		{
			debugFullScreenMode = this.RenderingSettings.fullScreenDebugMode;
			textureHeightPercent = this.RenderingSettings.fullScreenDebugModeOutputSizeScreenPercent;
			return debugFullScreenMode > DebugFullScreenMode.None;
		}

		// Token: 0x06000377 RID: 887 RVA: 0x00014D77 File Offset: 0x00012F77
		internal static void ConfigureColorDescriptorForDebugScreen(ref RenderTextureDescriptor descriptor, int cameraWidth, int cameraHeight)
		{
			descriptor.width = cameraWidth;
			descriptor.height = cameraHeight;
			descriptor.useMipMap = false;
			descriptor.autoGenerateMips = false;
			descriptor.useDynamicScale = true;
			descriptor.depthBufferBits = 0;
		}

		// Token: 0x06000378 RID: 888 RVA: 0x00014DA3 File Offset: 0x00012FA3
		internal static void ConfigureDepthDescriptorForDebugScreen(ref RenderTextureDescriptor descriptor, int depthBufferBits, int cameraWidth, int cameraHeight)
		{
			descriptor.width = cameraWidth;
			descriptor.height = cameraHeight;
			descriptor.useMipMap = false;
			descriptor.autoGenerateMips = false;
			descriptor.useDynamicScale = true;
			descriptor.depthBufferBits = depthBufferBits;
		}

		// Token: 0x06000379 RID: 889 RVA: 0x00014DD0 File Offset: 0x00012FD0
		[Conditional("DEVELOPMENT_BUILD")]
		[Conditional("UNITY_EDITOR")]
		internal void SetupShaderProperties(CommandBuffer cmd, int passIndex = 0)
		{
			if (this.LightingSettings.lightingDebugMode == DebugLightingMode.ShadowCascades)
			{
				cmd.EnableShaderKeyword("_DEBUG_ENVIRONMENTREFLECTIONS_OFF");
			}
			else
			{
				cmd.DisableShaderKeyword("_DEBUG_ENVIRONMENTREFLECTIONS_OFF");
			}
			switch (this.RenderingSettings.sceneOverrideMode)
			{
			case DebugSceneOverrideMode.Overdraw:
			{
				float num = 1f / (float)this.RenderingSettings.maxOverdrawCount;
				cmd.SetGlobalColor(DebugHandler.k_DebugColorPropertyId, new Color(num, num, num, 1f));
				break;
			}
			case DebugSceneOverrideMode.Wireframe:
				cmd.SetGlobalColor(DebugHandler.k_DebugColorPropertyId, Color.black);
				break;
			case DebugSceneOverrideMode.SolidWireframe:
				cmd.SetGlobalColor(DebugHandler.k_DebugColorPropertyId, (passIndex == 0) ? Color.white : Color.black);
				break;
			case DebugSceneOverrideMode.ShadedWireframe:
				if (passIndex == 0)
				{
					cmd.DisableShaderKeyword("DEBUG_DISPLAY");
				}
				else if (passIndex == 1)
				{
					cmd.SetGlobalColor(DebugHandler.k_DebugColorPropertyId, Color.black);
					cmd.EnableShaderKeyword("DEBUG_DISPLAY");
				}
				break;
			}
			DebugMaterialValidationMode materialValidationMode = this.MaterialSettings.materialValidationMode;
			if (materialValidationMode == DebugMaterialValidationMode.Albedo)
			{
				cmd.SetGlobalFloat(DebugHandler.k_DebugValidateAlbedoMinLuminanceId, this.MaterialSettings.albedoMinLuminance);
				cmd.SetGlobalFloat(DebugHandler.k_DebugValidateAlbedoMaxLuminanceId, this.MaterialSettings.albedoMaxLuminance);
				cmd.SetGlobalFloat(DebugHandler.k_DebugValidateAlbedoSaturationToleranceId, this.MaterialSettings.albedoSaturationTolerance);
				cmd.SetGlobalFloat(DebugHandler.k_DebugValidateAlbedoHueToleranceId, this.MaterialSettings.albedoHueTolerance);
				cmd.SetGlobalColor(DebugHandler.k_DebugValidateAlbedoCompareColorId, this.MaterialSettings.albedoCompareColor.linear);
				return;
			}
			if (materialValidationMode != DebugMaterialValidationMode.Metallic)
			{
				return;
			}
			cmd.SetGlobalFloat(DebugHandler.k_DebugValidateMetallicMinValueId, this.MaterialSettings.metallicMinValue);
			cmd.SetGlobalFloat(DebugHandler.k_DebugValidateMetallicMaxValueId, this.MaterialSettings.metallicMaxValue);
		}

		// Token: 0x0600037A RID: 890 RVA: 0x00014F6E File Offset: 0x0001316E
		internal void SetDebugRenderTarget(RenderTargetIdentifier renderTargetIdentifier, Rect displayRect, bool supportsStereo)
		{
			this.m_HasDebugRenderTarget = true;
			this.m_DebugRenderTargetSupportsStereo = supportsStereo;
			this.m_DebugRenderTargetIdentifier = renderTargetIdentifier;
			this.m_DebugRenderTargetPixelRect = new Vector4(displayRect.x, displayRect.y, displayRect.width, displayRect.height);
		}

		// Token: 0x0600037B RID: 891 RVA: 0x00014FAC File Offset: 0x000131AC
		internal void ResetDebugRenderTarget()
		{
			this.m_HasDebugRenderTarget = false;
		}

		// Token: 0x0600037C RID: 892 RVA: 0x00014FB8 File Offset: 0x000131B8
		[Conditional("DEVELOPMENT_BUILD")]
		[Conditional("UNITY_EDITOR")]
		internal void UpdateShaderGlobalPropertiesForFinalValidationPass(CommandBuffer cmd, ref CameraData cameraData, bool isFinalPass)
		{
			if (!isFinalPass || !cameraData.resolveFinalTarget)
			{
				cmd.DisableShaderKeyword("DEBUG_DISPLAY");
				return;
			}
			if (this.IsActiveForCamera(ref cameraData))
			{
				cmd.EnableShaderKeyword("DEBUG_DISPLAY");
			}
			else
			{
				cmd.DisableShaderKeyword("DEBUG_DISPLAY");
			}
			if (this.m_HasDebugRenderTarget)
			{
				cmd.SetGlobalTexture(this.m_DebugRenderTargetSupportsStereo ? DebugHandler.k_DebugTexturePropertyId : DebugHandler.k_DebugTextureNoStereoPropertyId, this.m_DebugRenderTargetIdentifier);
				cmd.SetGlobalVector(DebugHandler.k_DebugTextureDisplayRect, this.m_DebugRenderTargetPixelRect);
				cmd.SetGlobalInteger(DebugHandler.k_DebugRenderTargetSupportsStereo, this.m_DebugRenderTargetSupportsStereo ? 1 : 0);
			}
			DebugDisplaySettingsRendering renderingSettings = this.m_DebugDisplaySettings.renderingSettings;
			if (renderingSettings.validationMode == DebugValidationMode.HighlightOutsideOfRange)
			{
				cmd.SetGlobalInteger(DebugHandler.k_ValidationChannelsId, (int)renderingSettings.validationChannels);
				cmd.SetGlobalFloat(DebugHandler.k_RangeMinimumId, renderingSettings.validationRangeMin);
				cmd.SetGlobalFloat(DebugHandler.k_RangeMaximumId, renderingSettings.validationRangeMax);
			}
		}

		// Token: 0x0600037D RID: 893 RVA: 0x00015098 File Offset: 0x00013298
		[Conditional("DEVELOPMENT_BUILD")]
		[Conditional("UNITY_EDITOR")]
		internal void Setup(ScriptableRenderContext context, ref RenderingData renderingData)
		{
			CommandBuffer commandBuffer = renderingData.commandBuffer;
			CameraData cameraData = renderingData.cameraData;
			if (this.IsActiveForCamera(ref cameraData))
			{
				commandBuffer.EnableShaderKeyword("DEBUG_DISPLAY");
				commandBuffer.SetGlobalFloat(DebugHandler.k_DebugMaterialModeId, (float)this.MaterialSettings.materialDebugMode);
				commandBuffer.SetGlobalFloat(DebugHandler.k_DebugVertexAttributeModeId, (float)this.MaterialSettings.vertexAttributeDebugMode);
				commandBuffer.SetGlobalInteger(DebugHandler.k_DebugMaterialValidationModeId, (int)this.MaterialSettings.materialValidationMode);
				commandBuffer.SetGlobalInteger(DebugHandler.k_DebugMipInfoModeId, (int)this.RenderingSettings.mipInfoMode);
				commandBuffer.SetGlobalInteger(DebugHandler.k_DebugSceneOverrideModeId, (int)this.RenderingSettings.sceneOverrideMode);
				commandBuffer.SetGlobalInteger(DebugHandler.k_DebugFullScreenModeId, (int)this.RenderingSettings.fullScreenDebugMode);
				commandBuffer.SetGlobalInteger(DebugHandler.k_DebugMaxPixelCost, this.RenderingSettings.maxOverdrawCount);
				commandBuffer.SetGlobalInteger(DebugHandler.k_DebugValidationModeId, (int)this.RenderingSettings.validationMode);
				commandBuffer.SetGlobalColor(DebugHandler.k_DebugValidateBelowMinThresholdColorPropertyId, Color.red);
				commandBuffer.SetGlobalColor(DebugHandler.k_DebugValidateAboveMaxThresholdColorPropertyId, Color.blue);
				commandBuffer.SetGlobalFloat(DebugHandler.k_DebugLightingModeId, (float)this.LightingSettings.lightingDebugMode);
				commandBuffer.SetGlobalInteger(DebugHandler.k_DebugLightingFeatureFlagsId, (int)this.LightingSettings.lightingFeatureFlags);
				commandBuffer.SetGlobalColor(DebugHandler.k_DebugColorInvalidModePropertyId, Color.red);
			}
			else
			{
				commandBuffer.DisableShaderKeyword("DEBUG_DISPLAY");
			}
			context.ExecuteCommandBuffer(commandBuffer);
			commandBuffer.Clear();
		}

		// Token: 0x0600037E RID: 894 RVA: 0x000151F5 File Offset: 0x000133F5
		internal IEnumerable<DebugRenderSetup> CreateDebugRenderSetupEnumerable(ScriptableRenderContext context, CommandBuffer commandBuffer, FilteringSettings filteringSettings)
		{
			return new DebugHandler.DebugRenderPassEnumerable(this, context, commandBuffer, filteringSettings);
		}

		// Token: 0x0600037F RID: 895 RVA: 0x00015200 File Offset: 0x00013400
		internal void DrawWithDebugRenderState(ScriptableRenderContext context, CommandBuffer cmd, ref RenderingData renderingData, ref DrawingSettings drawingSettings, ref FilteringSettings filteringSettings, ref RenderStateBlock renderStateBlock, DebugHandler.DrawFunction func)
		{
			foreach (DebugRenderSetup debugRenderSetup in this.CreateDebugRenderSetupEnumerable(context, cmd, filteringSettings))
			{
				DrawingSettings drawingSettings2 = debugRenderSetup.CreateDrawingSettings(drawingSettings);
				RenderStateBlock renderStateBlock2 = debugRenderSetup.GetRenderStateBlock(renderStateBlock);
				func(context, ref renderingData, ref drawingSettings2, ref filteringSettings, ref renderStateBlock2);
			}
		}

		// Token: 0x0400027C RID: 636
		private static readonly int k_DebugColorInvalidModePropertyId = Shader.PropertyToID("_DebugColorInvalidMode");

		// Token: 0x0400027D RID: 637
		private static readonly int k_DebugColorPropertyId = Shader.PropertyToID("_DebugColor");

		// Token: 0x0400027E RID: 638
		private static readonly int k_DebugTexturePropertyId = Shader.PropertyToID("_DebugTexture");

		// Token: 0x0400027F RID: 639
		private static readonly int k_DebugTextureNoStereoPropertyId = Shader.PropertyToID("_DebugTextureNoStereo");

		// Token: 0x04000280 RID: 640
		private static readonly int k_DebugTextureDisplayRect = Shader.PropertyToID("_DebugTextureDisplayRect");

		// Token: 0x04000281 RID: 641
		private static readonly int k_DebugRenderTargetSupportsStereo = Shader.PropertyToID("_DebugRenderTargetSupportsStereo");

		// Token: 0x04000282 RID: 642
		private static readonly int k_DebugMaterialModeId = Shader.PropertyToID("_DebugMaterialMode");

		// Token: 0x04000283 RID: 643
		private static readonly int k_DebugVertexAttributeModeId = Shader.PropertyToID("_DebugVertexAttributeMode");

		// Token: 0x04000284 RID: 644
		private static readonly int k_DebugMaterialValidationModeId = Shader.PropertyToID("_DebugMaterialValidationMode");

		// Token: 0x04000285 RID: 645
		private static readonly int k_DebugMipInfoModeId = Shader.PropertyToID("_DebugMipInfoMode");

		// Token: 0x04000286 RID: 646
		private static readonly int k_DebugSceneOverrideModeId = Shader.PropertyToID("_DebugSceneOverrideMode");

		// Token: 0x04000287 RID: 647
		private static readonly int k_DebugFullScreenModeId = Shader.PropertyToID("_DebugFullScreenMode");

		// Token: 0x04000288 RID: 648
		private static readonly int k_DebugValidationModeId = Shader.PropertyToID("_DebugValidationMode");

		// Token: 0x04000289 RID: 649
		private static readonly int k_DebugValidateBelowMinThresholdColorPropertyId = Shader.PropertyToID("_DebugValidateBelowMinThresholdColor");

		// Token: 0x0400028A RID: 650
		private static readonly int k_DebugValidateAboveMaxThresholdColorPropertyId = Shader.PropertyToID("_DebugValidateAboveMaxThresholdColor");

		// Token: 0x0400028B RID: 651
		private static readonly int k_DebugMaxPixelCost = Shader.PropertyToID("_DebugMaxPixelCost");

		// Token: 0x0400028C RID: 652
		private static readonly int k_DebugLightingModeId = Shader.PropertyToID("_DebugLightingMode");

		// Token: 0x0400028D RID: 653
		private static readonly int k_DebugLightingFeatureFlagsId = Shader.PropertyToID("_DebugLightingFeatureFlags");

		// Token: 0x0400028E RID: 654
		private static readonly int k_DebugValidateAlbedoMinLuminanceId = Shader.PropertyToID("_DebugValidateAlbedoMinLuminance");

		// Token: 0x0400028F RID: 655
		private static readonly int k_DebugValidateAlbedoMaxLuminanceId = Shader.PropertyToID("_DebugValidateAlbedoMaxLuminance");

		// Token: 0x04000290 RID: 656
		private static readonly int k_DebugValidateAlbedoSaturationToleranceId = Shader.PropertyToID("_DebugValidateAlbedoSaturationTolerance");

		// Token: 0x04000291 RID: 657
		private static readonly int k_DebugValidateAlbedoHueToleranceId = Shader.PropertyToID("_DebugValidateAlbedoHueTolerance");

		// Token: 0x04000292 RID: 658
		private static readonly int k_DebugValidateAlbedoCompareColorId = Shader.PropertyToID("_DebugValidateAlbedoCompareColor");

		// Token: 0x04000293 RID: 659
		private static readonly int k_DebugValidateMetallicMinValueId = Shader.PropertyToID("_DebugValidateMetallicMinValue");

		// Token: 0x04000294 RID: 660
		private static readonly int k_DebugValidateMetallicMaxValueId = Shader.PropertyToID("_DebugValidateMetallicMaxValue");

		// Token: 0x04000295 RID: 661
		private static readonly int k_ValidationChannelsId = Shader.PropertyToID("_ValidationChannels");

		// Token: 0x04000296 RID: 662
		private static readonly int k_RangeMinimumId = Shader.PropertyToID("_RangeMinimum");

		// Token: 0x04000297 RID: 663
		private static readonly int k_RangeMaximumId = Shader.PropertyToID("_RangeMaximum");

		// Token: 0x04000298 RID: 664
		private readonly Material m_ReplacementMaterial;

		// Token: 0x04000299 RID: 665
		private readonly Material m_HDRDebugViewMaterial;

		// Token: 0x0400029A RID: 666
		private HDRDebugViewPass m_HDRDebugViewPass;

		// Token: 0x0400029B RID: 667
		private RTHandle m_DebugScreenColorHandle;

		// Token: 0x0400029C RID: 668
		private RTHandle m_DebugScreenDepthHandle;

		// Token: 0x0400029D RID: 669
		private bool m_HasDebugRenderTarget;

		// Token: 0x0400029E RID: 670
		private bool m_DebugRenderTargetSupportsStereo;

		// Token: 0x0400029F RID: 671
		private Vector4 m_DebugRenderTargetPixelRect;

		// Token: 0x040002A0 RID: 672
		private RenderTargetIdentifier m_DebugRenderTargetIdentifier;

		// Token: 0x040002A1 RID: 673
		private readonly UniversalRenderPipelineDebugDisplaySettings m_DebugDisplaySettings;

		// Token: 0x0200017A RID: 378
		private class DebugRenderPassEnumerable : IEnumerable<DebugRenderSetup>, IEnumerable
		{
			// Token: 0x06000A93 RID: 2707 RVA: 0x00048A30 File Offset: 0x00046C30
			public DebugRenderPassEnumerable(DebugHandler debugHandler, ScriptableRenderContext context, CommandBuffer commandBuffer, FilteringSettings filteringSettings)
			{
				this.m_DebugHandler = debugHandler;
				this.m_Context = context;
				this.m_CommandBuffer = commandBuffer;
				this.m_FilteringSettings = filteringSettings;
			}

			// Token: 0x06000A94 RID: 2708 RVA: 0x00048A55 File Offset: 0x00046C55
			public IEnumerator<DebugRenderSetup> GetEnumerator()
			{
				return new DebugHandler.DebugRenderPassEnumerable.Enumerator(this.m_DebugHandler, this.m_Context, this.m_CommandBuffer, this.m_FilteringSettings);
			}

			// Token: 0x06000A95 RID: 2709 RVA: 0x00048A74 File Offset: 0x00046C74
			IEnumerator IEnumerable.GetEnumerator()
			{
				return this.GetEnumerator();
			}

			// Token: 0x04000A0B RID: 2571
			private readonly DebugHandler m_DebugHandler;

			// Token: 0x04000A0C RID: 2572
			private readonly ScriptableRenderContext m_Context;

			// Token: 0x04000A0D RID: 2573
			private readonly CommandBuffer m_CommandBuffer;

			// Token: 0x04000A0E RID: 2574
			private readonly FilteringSettings m_FilteringSettings;

			// Token: 0x0200022A RID: 554
			private class Enumerator : IEnumerator<DebugRenderSetup>, IEnumerator, IDisposable
			{
				// Token: 0x1700026B RID: 619
				// (get) Token: 0x06000C3E RID: 3134 RVA: 0x0004D33F File Offset: 0x0004B53F
				// (set) Token: 0x06000C3F RID: 3135 RVA: 0x0004D347 File Offset: 0x0004B547
				public DebugRenderSetup Current { get; private set; }

				// Token: 0x1700026C RID: 620
				// (get) Token: 0x06000C40 RID: 3136 RVA: 0x0004D350 File Offset: 0x0004B550
				object IEnumerator.Current
				{
					get
					{
						return this.Current;
					}
				}

				// Token: 0x06000C41 RID: 3137 RVA: 0x0004D358 File Offset: 0x0004B558
				public Enumerator(DebugHandler debugHandler, ScriptableRenderContext context, CommandBuffer commandBuffer, FilteringSettings filteringSettings)
				{
					DebugSceneOverrideMode sceneOverrideMode = debugHandler.DebugDisplaySettings.renderingSettings.sceneOverrideMode;
					this.m_DebugHandler = debugHandler;
					this.m_Context = context;
					this.m_CommandBuffer = commandBuffer;
					this.m_FilteringSettings = filteringSettings;
					this.m_NumIterations = ((sceneOverrideMode == DebugSceneOverrideMode.SolidWireframe || sceneOverrideMode == DebugSceneOverrideMode.ShadedWireframe) ? 2 : 1);
					this.m_Index = -1;
				}

				// Token: 0x06000C42 RID: 3138 RVA: 0x0004D3B4 File Offset: 0x0004B5B4
				public bool MoveNext()
				{
					DebugRenderSetup debugRenderSetup = this.Current;
					if (debugRenderSetup != null)
					{
						debugRenderSetup.Dispose();
					}
					int num = this.m_Index + 1;
					this.m_Index = num;
					if (num >= this.m_NumIterations)
					{
						return false;
					}
					this.Current = new DebugRenderSetup(this.m_DebugHandler, this.m_Context, this.m_CommandBuffer, this.m_Index, this.m_FilteringSettings);
					return true;
				}

				// Token: 0x06000C43 RID: 3139 RVA: 0x0004D417 File Offset: 0x0004B617
				public void Reset()
				{
					if (this.Current != null)
					{
						this.Current.Dispose();
						this.Current = null;
					}
					this.m_Index = -1;
				}

				// Token: 0x06000C44 RID: 3140 RVA: 0x0004D43A File Offset: 0x0004B63A
				public void Dispose()
				{
					DebugRenderSetup debugRenderSetup = this.Current;
					if (debugRenderSetup == null)
					{
						return;
					}
					debugRenderSetup.Dispose();
				}

				// Token: 0x04000D24 RID: 3364
				private readonly DebugHandler m_DebugHandler;

				// Token: 0x04000D25 RID: 3365
				private readonly ScriptableRenderContext m_Context;

				// Token: 0x04000D26 RID: 3366
				private readonly CommandBuffer m_CommandBuffer;

				// Token: 0x04000D27 RID: 3367
				private readonly FilteringSettings m_FilteringSettings;

				// Token: 0x04000D28 RID: 3368
				private readonly int m_NumIterations;

				// Token: 0x04000D29 RID: 3369
				private int m_Index;
			}
		}

		// Token: 0x0200017B RID: 379
		// (Invoke) Token: 0x06000A97 RID: 2711
		internal delegate void DrawFunction(ScriptableRenderContext context, ref RenderingData renderingData, ref DrawingSettings drawingSettings, ref FilteringSettings filteringSettings, ref RenderStateBlock renderStateBlock);
	}
}
