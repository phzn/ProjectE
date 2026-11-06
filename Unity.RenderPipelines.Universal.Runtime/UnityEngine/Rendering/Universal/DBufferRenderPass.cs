using System;
using System.Collections.Generic;
using UnityEngine.Experimental.Rendering;

namespace UnityEngine.Rendering.Universal
{
	// Token: 0x02000061 RID: 97
	internal class DBufferRenderPass : ScriptableRenderPass
	{
		// Token: 0x17000127 RID: 295
		// (get) Token: 0x060003A1 RID: 929 RVA: 0x0001597D File Offset: 0x00013B7D
		// (set) Token: 0x060003A2 RID: 930 RVA: 0x00015985 File Offset: 0x00013B85
		internal RTHandle[] dBufferColorHandles { get; private set; }

		// Token: 0x17000128 RID: 296
		// (get) Token: 0x060003A3 RID: 931 RVA: 0x0001598E File Offset: 0x00013B8E
		// (set) Token: 0x060003A4 RID: 932 RVA: 0x00015996 File Offset: 0x00013B96
		internal RTHandle depthHandle { get; private set; }

		// Token: 0x17000129 RID: 297
		// (get) Token: 0x060003A5 RID: 933 RVA: 0x0001599F File Offset: 0x00013B9F
		internal RTHandle dBufferDepth
		{
			get
			{
				return this.m_DBufferDepth;
			}
		}

		// Token: 0x060003A6 RID: 934 RVA: 0x000159A8 File Offset: 0x00013BA8
		public DBufferRenderPass(Material dBufferClear, DBufferSettings settings, DecalDrawDBufferSystem drawSystem, bool decalLayers)
		{
			base.renderPassEvent = (RenderPassEvent)201;
			ScriptableRenderPassInput scriptableRenderPassInput = ScriptableRenderPassInput.Normal;
			base.ConfigureInput(scriptableRenderPassInput);
			this.m_DrawSystem = drawSystem;
			this.m_Settings = settings;
			this.m_DBufferClear = dBufferClear;
			this.m_ProfilingSampler = new ProfilingSampler("DBuffer Render");
			this.m_DBufferClearSampler = new ProfilingSampler("Clear");
			this.m_FilteringSettings = new FilteringSettings(new RenderQueueRange?(RenderQueueRange.opaque), -1, uint.MaxValue, 0);
			this.m_DecalLayers = decalLayers;
			this.m_ShaderTagIdList = new List<ShaderTagId>();
			this.m_ShaderTagIdList.Add(new ShaderTagId("DBufferMesh"));
			int num = (int)(settings.surfaceData + 1);
			this.dBufferColorHandles = new RTHandle[num];
		}

		// Token: 0x060003A7 RID: 935 RVA: 0x00015A5C File Offset: 0x00013C5C
		public void Dispose()
		{
			RTHandle dbufferDepth = this.m_DBufferDepth;
			if (dbufferDepth != null)
			{
				dbufferDepth.Release();
			}
			foreach (RTHandle rthandle in this.dBufferColorHandles)
			{
				if (rthandle != null)
				{
					rthandle.Release();
				}
			}
		}

		// Token: 0x060003A8 RID: 936 RVA: 0x00015AA0 File Offset: 0x00013CA0
		public void Setup(in CameraData cameraData)
		{
			RenderTextureDescriptor cameraTargetDescriptor = cameraData.cameraTargetDescriptor;
			cameraTargetDescriptor.graphicsFormat = GraphicsFormat.None;
			cameraTargetDescriptor.depthStencilFormat = cameraData.cameraTargetDescriptor.depthStencilFormat;
			cameraTargetDescriptor.msaaSamples = 1;
			RenderingUtils.ReAllocateIfNeeded(ref this.m_DBufferDepth, in cameraTargetDescriptor, FilterMode.Point, TextureWrapMode.Repeat, false, 1, 0f, DBufferRenderPass.s_DBufferDepthName);
			this.Setup(in cameraData, this.m_DBufferDepth);
		}

		// Token: 0x060003A9 RID: 937 RVA: 0x00015B00 File Offset: 0x00013D00
		public void Setup(in CameraData cameraData, RTHandle depthTextureHandle)
		{
			RenderTextureDescriptor cameraTargetDescriptor = cameraData.cameraTargetDescriptor;
			cameraTargetDescriptor.graphicsFormat = ((QualitySettings.activeColorSpace == ColorSpace.Linear) ? GraphicsFormat.R8G8B8A8_SRGB : GraphicsFormat.R8G8B8A8_UNorm);
			cameraTargetDescriptor.depthBufferBits = 0;
			cameraTargetDescriptor.msaaSamples = 1;
			RenderingUtils.ReAllocateIfNeeded(ref this.dBufferColorHandles[0], in cameraTargetDescriptor, FilterMode.Point, TextureWrapMode.Repeat, false, 1, 0f, DBufferRenderPass.s_DBufferNames[0]);
			if (this.m_Settings.surfaceData == DecalSurfaceData.AlbedoNormal || this.m_Settings.surfaceData == DecalSurfaceData.AlbedoNormalMAOS)
			{
				RenderTextureDescriptor cameraTargetDescriptor2 = cameraData.cameraTargetDescriptor;
				cameraTargetDescriptor2.graphicsFormat = GraphicsFormat.R8G8B8A8_UNorm;
				cameraTargetDescriptor2.depthBufferBits = 0;
				cameraTargetDescriptor2.msaaSamples = 1;
				RenderingUtils.ReAllocateIfNeeded(ref this.dBufferColorHandles[1], in cameraTargetDescriptor2, FilterMode.Point, TextureWrapMode.Repeat, false, 1, 0f, DBufferRenderPass.s_DBufferNames[1]);
			}
			if (this.m_Settings.surfaceData == DecalSurfaceData.AlbedoNormalMAOS)
			{
				RenderTextureDescriptor cameraTargetDescriptor3 = cameraData.cameraTargetDescriptor;
				cameraTargetDescriptor3.graphicsFormat = GraphicsFormat.R8G8B8A8_UNorm;
				cameraTargetDescriptor3.depthBufferBits = 0;
				cameraTargetDescriptor3.msaaSamples = 1;
				RenderingUtils.ReAllocateIfNeeded(ref this.dBufferColorHandles[2], in cameraTargetDescriptor3, FilterMode.Point, TextureWrapMode.Repeat, false, 1, 0f, DBufferRenderPass.s_DBufferNames[2]);
			}
			this.depthHandle = depthTextureHandle;
		}

		// Token: 0x060003AA RID: 938 RVA: 0x00015C12 File Offset: 0x00013E12
		public override void OnCameraSetup(CommandBuffer cmd, ref RenderingData renderingData)
		{
			base.ConfigureTarget(this.dBufferColorHandles, this.depthHandle);
		}

		// Token: 0x060003AB RID: 939 RVA: 0x00015C28 File Offset: 0x00013E28
		public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
		{
			SortingCriteria defaultOpaqueSortFlags = renderingData.cameraData.defaultOpaqueSortFlags;
			DrawingSettings drawingSettings = base.CreateDrawingSettings(this.m_ShaderTagIdList, ref renderingData, defaultOpaqueSortFlags);
			CommandBuffer commandBuffer = renderingData.commandBuffer;
			using (new ProfilingScope(commandBuffer, this.m_ProfilingSampler))
			{
				context.ExecuteCommandBuffer(commandBuffer);
				commandBuffer.Clear();
				commandBuffer.SetGlobalTexture(this.dBufferColorHandles[0].name, this.dBufferColorHandles[0].nameID);
				if (this.m_Settings.surfaceData == DecalSurfaceData.AlbedoNormal || this.m_Settings.surfaceData == DecalSurfaceData.AlbedoNormalMAOS)
				{
					commandBuffer.SetGlobalTexture(this.dBufferColorHandles[1].name, this.dBufferColorHandles[1].nameID);
				}
				if (this.m_Settings.surfaceData == DecalSurfaceData.AlbedoNormalMAOS)
				{
					commandBuffer.SetGlobalTexture(this.dBufferColorHandles[2].name, this.dBufferColorHandles[2].nameID);
				}
				CoreUtils.SetKeyword(commandBuffer, "_DBUFFER_MRT1", this.m_Settings.surfaceData == DecalSurfaceData.Albedo);
				CoreUtils.SetKeyword(commandBuffer, "_DBUFFER_MRT2", this.m_Settings.surfaceData == DecalSurfaceData.AlbedoNormal);
				CoreUtils.SetKeyword(commandBuffer, "_DBUFFER_MRT3", this.m_Settings.surfaceData == DecalSurfaceData.AlbedoNormalMAOS);
				CoreUtils.SetKeyword(commandBuffer, "_DECAL_LAYERS", this.m_DecalLayers);
				using (new ProfilingScope(commandBuffer, this.m_DBufferClearSampler))
				{
					Blitter.BlitTexture(commandBuffer, this.dBufferColorHandles[0], new Vector4(1f, 1f, 0f, 0f), this.m_DBufferClear, 0);
				}
				context.ExecuteCommandBuffer(commandBuffer);
				commandBuffer.Clear();
				this.m_DrawSystem.Execute(commandBuffer);
				context.DrawRenderers(renderingData.cullResults, ref drawingSettings, ref this.m_FilteringSettings);
			}
		}

		// Token: 0x060003AC RID: 940 RVA: 0x00015E18 File Offset: 0x00014018
		public override void OnCameraCleanup(CommandBuffer cmd)
		{
			if (cmd == null)
			{
				throw new ArgumentNullException("cmd");
			}
			CoreUtils.SetKeyword(cmd, "_DBUFFER_MRT1", false);
			CoreUtils.SetKeyword(cmd, "_DBUFFER_MRT2", false);
			CoreUtils.SetKeyword(cmd, "_DBUFFER_MRT3", false);
			CoreUtils.SetKeyword(cmd, "_DECAL_LAYERS", false);
		}

		// Token: 0x040002AD RID: 685
		private static string[] s_DBufferNames = new string[] { "_DBufferTexture0", "_DBufferTexture1", "_DBufferTexture2", "_DBufferTexture3" };

		// Token: 0x040002AE RID: 686
		private static string s_DBufferDepthName = "DBufferDepth";

		// Token: 0x040002AF RID: 687
		private DecalDrawDBufferSystem m_DrawSystem;

		// Token: 0x040002B0 RID: 688
		private DBufferSettings m_Settings;

		// Token: 0x040002B1 RID: 689
		private Material m_DBufferClear;

		// Token: 0x040002B2 RID: 690
		private FilteringSettings m_FilteringSettings;

		// Token: 0x040002B3 RID: 691
		private List<ShaderTagId> m_ShaderTagIdList;

		// Token: 0x040002B4 RID: 692
		private ProfilingSampler m_ProfilingSampler;

		// Token: 0x040002B5 RID: 693
		private ProfilingSampler m_DBufferClearSampler;

		// Token: 0x040002B6 RID: 694
		private bool m_DecalLayers;

		// Token: 0x040002B7 RID: 695
		private RTHandle m_DBufferDepth;
	}
}
