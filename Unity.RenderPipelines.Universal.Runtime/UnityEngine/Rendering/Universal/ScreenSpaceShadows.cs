using System;
using UnityEngine.Experimental.Rendering;

namespace UnityEngine.Rendering.Universal
{
	// Token: 0x020000CA RID: 202
	[DisallowMultipleRendererFeature("Screen Space Shadows")]
	[Tooltip("Screen Space Shadows")]
	internal class ScreenSpaceShadows : ScriptableRendererFeature
	{
		// Token: 0x06000637 RID: 1591 RVA: 0x00025FE4 File Offset: 0x000241E4
		public override void Create()
		{
			if (this.m_SSShadowsPass == null)
			{
				this.m_SSShadowsPass = new ScreenSpaceShadows.ScreenSpaceShadowsPass();
			}
			if (this.m_SSShadowsPostPass == null)
			{
				this.m_SSShadowsPostPass = new ScreenSpaceShadows.ScreenSpaceShadowsPostPass();
			}
			this.LoadMaterial();
			this.m_SSShadowsPass.renderPassEvent = RenderPassEvent.AfterRenderingGbuffer;
			this.m_SSShadowsPostPass.renderPassEvent = RenderPassEvent.BeforeRenderingTransparents;
		}

		// Token: 0x06000638 RID: 1592 RVA: 0x00026040 File Offset: 0x00024240
		public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
		{
			if (UniversalRenderer.IsOffscreenDepthTexture(in renderingData.cameraData))
			{
				return;
			}
			if (!this.LoadMaterial())
			{
				Debug.LogErrorFormat("{0}.AddRenderPasses(): Missing material. {1} render pass will not be added. Check for missing reference in the renderer resources.", new object[]
				{
					base.GetType().Name,
					base.name
				});
				return;
			}
			if (renderingData.shadowData.supportsMainLightShadows && renderingData.lightData.mainLightIndex != -1 && this.m_SSShadowsPass.Setup(this.m_Settings, this.m_Material))
			{
				bool flag = renderer is UniversalRenderer && ((UniversalRenderer)renderer).renderingModeRequested == RenderingMode.Deferred;
				this.m_SSShadowsPass.renderPassEvent = (flag ? RenderPassEvent.AfterRenderingGbuffer : ((RenderPassEvent)201));
				renderer.EnqueuePass(this.m_SSShadowsPass);
				renderer.EnqueuePass(this.m_SSShadowsPostPass);
			}
		}

		// Token: 0x06000639 RID: 1593 RVA: 0x00026116 File Offset: 0x00024316
		protected override void Dispose(bool disposing)
		{
			ScreenSpaceShadows.ScreenSpaceShadowsPass ssshadowsPass = this.m_SSShadowsPass;
			if (ssshadowsPass != null)
			{
				ssshadowsPass.Dispose();
			}
			this.m_SSShadowsPass = null;
			CoreUtils.Destroy(this.m_Material);
		}

		// Token: 0x0600063A RID: 1594 RVA: 0x0002613C File Offset: 0x0002433C
		private bool LoadMaterial()
		{
			if (this.m_Material != null)
			{
				return true;
			}
			if (this.m_Shader == null)
			{
				this.m_Shader = Shader.Find("Hidden/Universal Render Pipeline/ScreenSpaceShadows");
				if (this.m_Shader == null)
				{
					return false;
				}
			}
			this.m_Material = CoreUtils.CreateEngineMaterial(this.m_Shader);
			return this.m_Material != null;
		}

		// Token: 0x04000523 RID: 1315
		[SerializeField]
		[HideInInspector]
		private Shader m_Shader;

		// Token: 0x04000524 RID: 1316
		[SerializeField]
		private ScreenSpaceShadowsSettings m_Settings = new ScreenSpaceShadowsSettings();

		// Token: 0x04000525 RID: 1317
		private Material m_Material;

		// Token: 0x04000526 RID: 1318
		private ScreenSpaceShadows.ScreenSpaceShadowsPass m_SSShadowsPass;

		// Token: 0x04000527 RID: 1319
		private ScreenSpaceShadows.ScreenSpaceShadowsPostPass m_SSShadowsPostPass;

		// Token: 0x04000528 RID: 1320
		private const string k_ShaderName = "Hidden/Universal Render Pipeline/ScreenSpaceShadows";

		// Token: 0x020001B3 RID: 435
		private class ScreenSpaceShadowsPass : ScriptableRenderPass
		{
			// Token: 0x06000B15 RID: 2837 RVA: 0x0004B441 File Offset: 0x00049641
			internal ScreenSpaceShadowsPass()
			{
				this.m_CurrentSettings = new ScreenSpaceShadowsSettings();
			}

			// Token: 0x06000B16 RID: 2838 RVA: 0x0004B454 File Offset: 0x00049654
			public void Dispose()
			{
				RTHandle renderTarget = this.m_RenderTarget;
				if (renderTarget == null)
				{
					return;
				}
				renderTarget.Release();
			}

			// Token: 0x06000B17 RID: 2839 RVA: 0x0004B466 File Offset: 0x00049666
			internal bool Setup(ScreenSpaceShadowsSettings featureSettings, Material material)
			{
				this.m_CurrentSettings = featureSettings;
				this.m_Material = material;
				base.ConfigureInput(ScriptableRenderPassInput.Depth);
				return this.m_Material != null;
			}

			// Token: 0x06000B18 RID: 2840 RVA: 0x0004B48C File Offset: 0x0004968C
			public override void OnCameraSetup(CommandBuffer cmd, ref RenderingData renderingData)
			{
				RenderTextureDescriptor cameraTargetDescriptor = renderingData.cameraData.cameraTargetDescriptor;
				cameraTargetDescriptor.depthBufferBits = 0;
				cameraTargetDescriptor.msaaSamples = 1;
				cameraTargetDescriptor.graphicsFormat = (RenderingUtils.SupportsGraphicsFormat(GraphicsFormat.R8_UNorm, FormatUsage.Blend) ? GraphicsFormat.R8_UNorm : GraphicsFormat.B8G8R8A8_UNorm);
				RenderingUtils.ReAllocateIfNeeded(ref this.m_RenderTarget, in cameraTargetDescriptor, FilterMode.Point, TextureWrapMode.Clamp, false, 1, 0f, "_ScreenSpaceShadowmapTexture");
				cmd.SetGlobalTexture(this.m_RenderTarget.name, this.m_RenderTarget.nameID);
				base.ConfigureTarget(this.m_RenderTarget);
				base.ConfigureClear(ClearFlag.None, Color.white);
			}

			// Token: 0x06000B19 RID: 2841 RVA: 0x0004B51C File Offset: 0x0004971C
			public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
			{
				if (this.m_Material == null)
				{
					Debug.LogErrorFormat("{0}.Execute(): Missing material. ScreenSpaceShadows pass will not execute. Check for missing reference in the renderer resources.", new object[] { base.GetType().Name });
					return;
				}
				CommandBuffer commandBuffer = renderingData.commandBuffer;
				using (new ProfilingScope(commandBuffer, ScreenSpaceShadows.ScreenSpaceShadowsPass.m_ProfilingSampler))
				{
					Blitter.BlitCameraTexture(commandBuffer, this.m_RenderTarget, this.m_RenderTarget, this.m_Material, 0);
					CoreUtils.SetKeyword(commandBuffer, "_MAIN_LIGHT_SHADOWS", false);
					CoreUtils.SetKeyword(commandBuffer, "_MAIN_LIGHT_SHADOWS_CASCADE", false);
					CoreUtils.SetKeyword(commandBuffer, "_MAIN_LIGHT_SHADOWS_SCREEN", true);
				}
			}

			// Token: 0x04000B70 RID: 2928
			private static string m_ProfilerTag = "ScreenSpaceShadows";

			// Token: 0x04000B71 RID: 2929
			private static ProfilingSampler m_ProfilingSampler = new ProfilingSampler(ScreenSpaceShadows.ScreenSpaceShadowsPass.m_ProfilerTag);

			// Token: 0x04000B72 RID: 2930
			private Material m_Material;

			// Token: 0x04000B73 RID: 2931
			private ScreenSpaceShadowsSettings m_CurrentSettings;

			// Token: 0x04000B74 RID: 2932
			private RTHandle m_RenderTarget;
		}

		// Token: 0x020001B4 RID: 436
		private class ScreenSpaceShadowsPostPass : ScriptableRenderPass
		{
			// Token: 0x06000B1B RID: 2843 RVA: 0x0004B5E3 File Offset: 0x000497E3
			public override void Configure(CommandBuffer cmd, RenderTextureDescriptor cameraTextureDescriptor)
			{
				base.ConfigureTarget(ScreenSpaceShadows.ScreenSpaceShadowsPostPass.k_CurrentActive);
			}

			// Token: 0x06000B1C RID: 2844 RVA: 0x0004B5F0 File Offset: 0x000497F0
			public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
			{
				CommandBuffer commandBuffer = renderingData.commandBuffer;
				using (new ProfilingScope(commandBuffer, ScreenSpaceShadows.ScreenSpaceShadowsPostPass.m_ProfilingSampler))
				{
					int mainLightShadowCascadesCount = renderingData.shadowData.mainLightShadowCascadesCount;
					bool supportsMainLightShadows = renderingData.shadowData.supportsMainLightShadows;
					bool flag = supportsMainLightShadows && mainLightShadowCascadesCount == 1;
					bool flag2 = supportsMainLightShadows && mainLightShadowCascadesCount > 1;
					CoreUtils.SetKeyword(commandBuffer, "_MAIN_LIGHT_SHADOWS_SCREEN", false);
					CoreUtils.SetKeyword(commandBuffer, "_MAIN_LIGHT_SHADOWS", flag);
					CoreUtils.SetKeyword(commandBuffer, "_MAIN_LIGHT_SHADOWS_CASCADE", flag2);
				}
			}

			// Token: 0x04000B75 RID: 2933
			private static string m_ProfilerTag = "ScreenSpaceShadows Post";

			// Token: 0x04000B76 RID: 2934
			private static ProfilingSampler m_ProfilingSampler = new ProfilingSampler(ScreenSpaceShadows.ScreenSpaceShadowsPostPass.m_ProfilerTag);

			// Token: 0x04000B77 RID: 2935
			private static readonly RTHandle k_CurrentActive = RTHandles.Alloc(BuiltinRenderTextureType.CurrentActive);
		}
	}
}
