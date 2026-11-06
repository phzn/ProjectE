using System;

namespace UnityEngine.Rendering.Universal
{
	// Token: 0x0200005D RID: 93
	internal class DebugRenderSetup : IDisposable
	{
		// Token: 0x17000119 RID: 281
		// (get) Token: 0x06000381 RID: 897 RVA: 0x00015429 File Offset: 0x00013629
		private DebugDisplaySettingsMaterial MaterialSettings
		{
			get
			{
				return this.m_DebugHandler.DebugDisplaySettings.materialSettings;
			}
		}

		// Token: 0x1700011A RID: 282
		// (get) Token: 0x06000382 RID: 898 RVA: 0x0001543B File Offset: 0x0001363B
		private DebugDisplaySettingsRendering RenderingSettings
		{
			get
			{
				return this.m_DebugHandler.DebugDisplaySettings.renderingSettings;
			}
		}

		// Token: 0x1700011B RID: 283
		// (get) Token: 0x06000383 RID: 899 RVA: 0x0001544D File Offset: 0x0001364D
		private DebugDisplaySettingsLighting LightingSettings
		{
			get
			{
				return this.m_DebugHandler.DebugDisplaySettings.lightingSettings;
			}
		}

		// Token: 0x06000384 RID: 900 RVA: 0x00015460 File Offset: 0x00013660
		private void Begin()
		{
			DebugSceneOverrideMode sceneOverrideMode = this.RenderingSettings.sceneOverrideMode;
			if (sceneOverrideMode != DebugSceneOverrideMode.Wireframe)
			{
				if (sceneOverrideMode - DebugSceneOverrideMode.SolidWireframe <= 1)
				{
					if (this.m_Index == 1)
					{
						this.m_Context.Submit();
						GL.wireframe = true;
					}
				}
			}
			else
			{
				this.m_Context.Submit();
				GL.wireframe = true;
			}
			this.m_Context.ExecuteCommandBuffer(this.m_CommandBuffer);
			this.m_CommandBuffer.Clear();
		}

		// Token: 0x06000385 RID: 901 RVA: 0x000154D8 File Offset: 0x000136D8
		private void End()
		{
			DebugSceneOverrideMode sceneOverrideMode = this.RenderingSettings.sceneOverrideMode;
			if (sceneOverrideMode == DebugSceneOverrideMode.Wireframe)
			{
				this.m_Context.Submit();
				GL.wireframe = false;
				return;
			}
			if (sceneOverrideMode - DebugSceneOverrideMode.SolidWireframe > 1)
			{
				return;
			}
			if (this.m_Index == 1)
			{
				this.m_Context.Submit();
				GL.wireframe = false;
			}
		}

		// Token: 0x06000386 RID: 902 RVA: 0x0001552E File Offset: 0x0001372E
		internal DebugRenderSetup(DebugHandler debugHandler, ScriptableRenderContext context, CommandBuffer commandBuffer, int index, FilteringSettings filteringSettings)
		{
			this.m_DebugHandler = debugHandler;
			this.m_Context = context;
			this.m_CommandBuffer = commandBuffer;
			this.m_Index = index;
			this.m_FilteringSettings = filteringSettings;
			this.Begin();
		}

		// Token: 0x06000387 RID: 903 RVA: 0x00015564 File Offset: 0x00013764
		internal DrawingSettings CreateDrawingSettings(DrawingSettings drawingSettings)
		{
			if (this.MaterialSettings.vertexAttributeDebugMode > DebugVertexAttributeMode.None)
			{
				Material replacementMaterial = this.m_DebugHandler.ReplacementMaterial;
				DrawingSettings drawingSettings2 = drawingSettings;
				drawingSettings2.overrideMaterial = replacementMaterial;
				drawingSettings2.overrideMaterialPassIndex = 0;
				return drawingSettings2;
			}
			return drawingSettings;
		}

		// Token: 0x06000388 RID: 904 RVA: 0x000155A4 File Offset: 0x000137A4
		internal RenderStateBlock GetRenderStateBlock(RenderStateBlock renderStateBlock)
		{
			DebugSceneOverrideMode sceneOverrideMode = this.RenderingSettings.sceneOverrideMode;
			if (sceneOverrideMode != DebugSceneOverrideMode.Overdraw)
			{
				if (sceneOverrideMode - DebugSceneOverrideMode.SolidWireframe <= 1)
				{
					if (this.m_Index == 1)
					{
						renderStateBlock.rasterState = new RasterState(CullMode.Back, -1, -1f, true);
						renderStateBlock.mask = RenderStateMask.Raster;
					}
				}
			}
			else
			{
				bool flag = this.m_FilteringSettings.renderQueueRange == RenderQueueRange.opaque || this.m_FilteringSettings.renderQueueRange == RenderQueueRange.all;
				bool flag2 = this.m_FilteringSettings.renderQueueRange == RenderQueueRange.transparent || this.m_FilteringSettings.renderQueueRange == RenderQueueRange.all;
				bool flag3 = this.m_DebugHandler.DebugDisplaySettings.renderingSettings.overdrawMode == DebugOverdrawMode.Opaque || this.m_DebugHandler.DebugDisplaySettings.renderingSettings.overdrawMode == DebugOverdrawMode.All;
				bool flag4 = this.m_DebugHandler.DebugDisplaySettings.renderingSettings.overdrawMode == DebugOverdrawMode.Transparent || this.m_DebugHandler.DebugDisplaySettings.renderingSettings.overdrawMode == DebugOverdrawMode.All;
				BlendMode blendMode = (((flag && flag3) || (flag2 && flag4)) ? BlendMode.One : BlendMode.Zero);
				RenderTargetBlendState renderTargetBlendState = new RenderTargetBlendState(ColorWriteMask.All, BlendMode.One, blendMode, BlendMode.One, BlendMode.Zero, BlendOp.Add, BlendOp.Add);
				renderStateBlock.blendState = new BlendState
				{
					blendState0 = renderTargetBlendState
				};
				renderStateBlock.mask = RenderStateMask.Blend;
			}
			return renderStateBlock;
		}

		// Token: 0x06000389 RID: 905 RVA: 0x00015712 File Offset: 0x00013912
		public void Dispose()
		{
			this.End();
		}

		// Token: 0x040002A2 RID: 674
		private readonly DebugHandler m_DebugHandler;

		// Token: 0x040002A3 RID: 675
		private readonly ScriptableRenderContext m_Context;

		// Token: 0x040002A4 RID: 676
		private readonly CommandBuffer m_CommandBuffer;

		// Token: 0x040002A5 RID: 677
		private readonly int m_Index;

		// Token: 0x040002A6 RID: 678
		private readonly FilteringSettings m_FilteringSettings;
	}
}
