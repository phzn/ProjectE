using System;
using System.Collections.Generic;
using UnityEngine.Experimental.Rendering.Universal;

namespace UnityEngine.Rendering.Universal
{
	// Token: 0x0200002E RID: 46
	internal class Render2DLightingPass : ScriptableRenderPass, IRenderPass2D
	{
		// Token: 0x06000199 RID: 409 RVA: 0x0000DCE1 File Offset: 0x0000BEE1
		public Render2DLightingPass(Renderer2DData rendererData, Material blitMaterial, Material samplingMaterial)
		{
			this.m_Renderer2DData = rendererData;
			this.m_BlitMaterial = blitMaterial;
			this.m_SamplingMaterial = samplingMaterial;
			this.m_CameraSortingLayerBoundsIndex = Render2DLightingPass.GetCameraSortingLayerBoundsIndex(this.m_Renderer2DData);
		}

		// Token: 0x0600019A RID: 410 RVA: 0x0000DD0F File Offset: 0x0000BF0F
		internal void Setup(bool useDepth)
		{
			this.m_NeedsDepth = useDepth;
		}

		// Token: 0x0600019B RID: 411 RVA: 0x0000DD18 File Offset: 0x0000BF18
		private void GetTransparencySortingMode(Camera camera, ref SortingSettings sortingSettings)
		{
			TransparencySortMode transparencySortMode = this.m_Renderer2DData.transparencySortMode;
			if (transparencySortMode == TransparencySortMode.Default)
			{
				transparencySortMode = (camera.orthographic ? TransparencySortMode.Orthographic : TransparencySortMode.Perspective);
			}
			if (transparencySortMode == TransparencySortMode.Perspective)
			{
				sortingSettings.distanceMetric = DistanceMetric.Perspective;
				return;
			}
			if (transparencySortMode != TransparencySortMode.Orthographic)
			{
				sortingSettings.distanceMetric = DistanceMetric.CustomAxis;
				sortingSettings.customAxis = this.m_Renderer2DData.transparencySortAxis;
				return;
			}
			sortingSettings.distanceMetric = DistanceMetric.Orthographic;
		}

		// Token: 0x0600019C RID: 412 RVA: 0x0000DD74 File Offset: 0x0000BF74
		private void CopyCameraSortingLayerRenderTexture(ScriptableRenderContext context, RenderingData renderingData, RenderBufferStoreAction mainTargetStoreAction)
		{
			CommandBuffer commandBuffer = renderingData.commandBuffer;
			this.CreateCameraSortingLayerRenderTexture(renderingData, commandBuffer, this.m_Renderer2DData.cameraSortingLayerDownsamplingMethod);
			Material material = this.m_SamplingMaterial;
			int num = 0;
			if (this.m_Renderer2DData.cameraSortingLayerDownsamplingMethod != Downsampling._4xBox)
			{
				material = this.m_BlitMaterial;
				num = ((base.colorAttachmentHandle.rt.filterMode == FilterMode.Bilinear) ? 1 : 0);
			}
			Blitter.BlitCameraTexture(commandBuffer, base.colorAttachmentHandle, this.m_Renderer2DData.cameraSortingLayerRenderTarget, RenderBufferLoadAction.DontCare, RenderBufferStoreAction.Store, material, num);
			CoreUtils.SetRenderTarget(commandBuffer, base.colorAttachmentHandle, RenderBufferLoadAction.Load, mainTargetStoreAction, base.depthAttachmentHandle, RenderBufferLoadAction.Load, mainTargetStoreAction, ClearFlag.None, Color.clear, 0, CubemapFace.Unknown, -1);
			commandBuffer.SetGlobalTexture(this.m_Renderer2DData.cameraSortingLayerRenderTargetId, this.m_Renderer2DData.cameraSortingLayerRenderTarget.nameID);
			context.ExecuteCommandBuffer(commandBuffer);
			commandBuffer.Clear();
		}

		// Token: 0x0600019D RID: 413 RVA: 0x0000DE3C File Offset: 0x0000C03C
		internal static short GetCameraSortingLayerBoundsIndex(Renderer2DData rendererData)
		{
			SortingLayer[] cachedSortingLayer = Light2DManager.GetCachedSortingLayer();
			short num = 0;
			while ((int)num < cachedSortingLayer.Length)
			{
				if (cachedSortingLayer[(int)num].id == rendererData.cameraSortingLayerTextureBound)
				{
					return (short)cachedSortingLayer[(int)num].value;
				}
				num += 1;
			}
			return short.MinValue;
		}

		// Token: 0x0600019E RID: 414 RVA: 0x0000DE88 File Offset: 0x0000C088
		private void DetermineWhenToResolve(int startIndex, int batchesDrawn, int batchCount, LayerBatch[] layerBatches, out int resolveDuringBatch, out bool resolveIsAfterCopy)
		{
			bool flag = false;
			List<Light2D> visibleLights = this.m_Renderer2DData.lightCullResult.visibleLights;
			for (int i = 0; i < visibleLights.Count; i++)
			{
				flag = visibleLights[i].renderVolumetricShadows;
				if (flag)
				{
					break;
				}
			}
			int num = -1;
			if (flag)
			{
				for (int j = startIndex + batchesDrawn - 1; j >= startIndex; j--)
				{
					if (layerBatches[j].lightStats.totalVolumetricUsage > 0)
					{
						num = j;
						break;
					}
				}
			}
			if (this.m_Renderer2DData.useCameraSortingLayerTexture)
			{
				short cameraSortingLayerBoundsIndex = Render2DLightingPass.GetCameraSortingLayerBoundsIndex(this.m_Renderer2DData);
				int num2 = -1;
				for (int k = startIndex; k < startIndex + batchesDrawn; k++)
				{
					LayerBatch layerBatch = layerBatches[k];
					if (cameraSortingLayerBoundsIndex >= layerBatch.layerRange.lowerBound && cameraSortingLayerBoundsIndex <= layerBatch.layerRange.upperBound)
					{
						num2 = k;
						break;
					}
				}
				resolveIsAfterCopy = num2 > num;
				resolveDuringBatch = (resolveIsAfterCopy ? num2 : num);
				return;
			}
			resolveDuringBatch = num;
			resolveIsAfterCopy = false;
		}

		// Token: 0x0600019F RID: 415 RVA: 0x0000DF7C File Offset: 0x0000C17C
		private void Render(ScriptableRenderContext context, CommandBuffer cmd, ref RenderingData renderingData, ref FilteringSettings filterSettings, DrawingSettings drawSettings)
		{
			DebugHandler activeDebugHandler = ScriptableRenderPass.GetActiveDebugHandler(ref renderingData);
			if (activeDebugHandler != null)
			{
				RenderStateBlock renderStateBlock = default(RenderStateBlock);
				activeDebugHandler.DrawWithDebugRenderState(context, cmd, ref renderingData, ref drawSettings, ref filterSettings, ref renderStateBlock, delegate(ScriptableRenderContext ctx, ref RenderingData data, ref DrawingSettings ds, ref FilteringSettings fs, ref RenderStateBlock rsb)
				{
					ctx.DrawRenderers(data.cullResults, ref ds, ref fs, ref rsb);
				});
				return;
			}
			context.DrawRenderers(renderingData.cullResults, ref drawSettings, ref filterSettings);
		}

		// Token: 0x060001A0 RID: 416 RVA: 0x0000DFDC File Offset: 0x0000C1DC
		private int DrawLayerBatches(LayerBatch[] layerBatches, int batchCount, int startIndex, CommandBuffer cmd, ScriptableRenderContext context, ref RenderingData renderingData, ref DrawingSettings normalsDrawSettings, ref DrawingSettings drawSettings, ref RenderTextureDescriptor desc)
		{
			DebugHandler activeDebugHandler = ScriptableRenderPass.GetActiveDebugHandler(ref renderingData);
			bool flag = activeDebugHandler == null || activeDebugHandler.IsLightingActive;
			int num = 0;
			uint num2 = 0U;
			bool flag2 = true;
			using (new ProfilingScope(cmd, Render2DLightingPass.m_ProfilingDrawLights))
			{
				for (int i = startIndex; i < batchCount; i++)
				{
					ref LayerBatch ptr = ref layerBatches[i];
					uint num3 = ptr.lightStats.blendStylesUsed;
					uint num4 = 0U;
					while (num3 > 0U)
					{
						num4 += num3 & 1U;
						num3 >>= 1;
					}
					num2 += num4;
					if (num2 > LayerUtility.maxTextureCount)
					{
						break;
					}
					num++;
					if (ptr.useNormals)
					{
						FilteringSettings filteringSettings;
						LayerUtility.GetFilterSettings(this.m_Renderer2DData, ref ptr, out filteringSettings);
						RenderTargetIdentifier renderTargetIdentifier = (this.m_NeedsDepth ? base.depthAttachmentHandle.nameID : BuiltinRenderTextureType.None);
						this.RenderNormals(context, renderingData, normalsDrawSettings, filteringSettings, renderTargetIdentifier, flag2);
						flag2 = false;
					}
					using (new ProfilingScope(cmd, Render2DLightingPass.m_ProfilingDrawLightTextures))
					{
						this.RenderLights(renderingData, cmd, ptr.startLayerID, ref ptr, ref desc);
					}
				}
			}
			bool flag3 = renderingData.cameraData.cameraTargetDescriptor.msaaSamples > 1;
			bool flag4 = startIndex + num >= batchCount;
			int num5 = -1;
			bool flag5 = false;
			if (flag3 && flag4)
			{
				this.DetermineWhenToResolve(startIndex, num, batchCount, layerBatches, out num5, out flag5);
			}
			int num6 = this.m_Renderer2DData.lightBlendStyles.Length;
			using (new ProfilingScope(cmd, Render2DLightingPass.m_ProfilingDrawRenderers))
			{
				RenderBufferStoreAction renderBufferStoreAction;
				if (flag3)
				{
					renderBufferStoreAction = ((num5 < startIndex) ? RenderBufferStoreAction.Resolve : RenderBufferStoreAction.StoreAndResolve);
				}
				else
				{
					renderBufferStoreAction = RenderBufferStoreAction.Store;
				}
				CoreUtils.SetRenderTarget(cmd, base.colorAttachmentHandle, RenderBufferLoadAction.Load, renderBufferStoreAction, base.depthAttachmentHandle, RenderBufferLoadAction.Load, renderBufferStoreAction, ClearFlag.None, Color.clear, 0, CubemapFace.Unknown, -1);
				for (int j = startIndex; j < startIndex + num; j++)
				{
					using (new ProfilingScope(cmd, Render2DLightingPass.m_ProfilingDrawLayerBatch))
					{
						LayerBatch layerBatch = layerBatches[j];
						if (layerBatch.lightStats.totalLights > 0)
						{
							for (int k = 0; k < num6; k++)
							{
								uint num7 = 1U << k;
								bool flag6 = (layerBatch.lightStats.blendStylesUsed & num7) > 0U;
								if (flag6)
								{
									RenderTargetIdentifier rtid = layerBatch.GetRTId(cmd, desc, k);
									cmd.SetGlobalTexture(Render2DLightingPass.k_ShapeLightTextureIDs[k], rtid);
								}
								RendererLighting.EnableBlendStyle(cmd, k, flag6);
							}
						}
						else
						{
							for (int l = 0; l < Render2DLightingPass.k_ShapeLightTextureIDs.Length; l++)
							{
								cmd.SetGlobalTexture(Render2DLightingPass.k_ShapeLightTextureIDs[l], Texture2D.blackTexture);
								RendererLighting.EnableBlendStyle(cmd, l, l == 0);
							}
						}
						context.ExecuteCommandBuffer(cmd);
						cmd.Clear();
						short cameraSortingLayerBoundsIndex = Render2DLightingPass.GetCameraSortingLayerBoundsIndex(this.m_Renderer2DData);
						RenderBufferStoreAction renderBufferStoreAction2;
						if (flag3)
						{
							renderBufferStoreAction2 = ((num5 == j && flag5) ? RenderBufferStoreAction.Resolve : RenderBufferStoreAction.StoreAndResolve);
						}
						else
						{
							renderBufferStoreAction2 = RenderBufferStoreAction.Store;
						}
						FilteringSettings filteringSettings2;
						LayerUtility.GetFilterSettings(this.m_Renderer2DData, ref layerBatch, out filteringSettings2);
						this.Render(context, cmd, ref renderingData, ref filteringSettings2, drawSettings);
						if (this.m_Renderer2DData.useCameraSortingLayerTexture && cameraSortingLayerBoundsIndex >= layerBatch.layerRange.lowerBound && cameraSortingLayerBoundsIndex <= layerBatch.layerRange.upperBound)
						{
							this.CopyCameraSortingLayerRenderTexture(context, renderingData, renderBufferStoreAction2);
						}
						if (flag && layerBatch.lightStats.totalVolumetricUsage > 0)
						{
							string text = "Render 2D Light Volumes";
							cmd.BeginSample(text);
							RenderBufferStoreAction renderBufferStoreAction3;
							if (flag3)
							{
								renderBufferStoreAction3 = ((num5 == j && !flag5) ? RenderBufferStoreAction.Resolve : RenderBufferStoreAction.StoreAndResolve);
							}
							else
							{
								renderBufferStoreAction3 = RenderBufferStoreAction.Store;
							}
							this.RenderLightVolumes(renderingData, cmd, layerBatch.startLayerID, layerBatch.endLayerValue, base.colorAttachmentHandle.nameID, base.depthAttachmentHandle.nameID, RenderBufferStoreAction.Store, renderBufferStoreAction3, false, this.m_Renderer2DData.lightCullResult.visibleLights);
							cmd.EndSample(text);
						}
					}
				}
			}
			for (int m = startIndex; m < startIndex + num; m++)
			{
				layerBatches[m].ReleaseRT(cmd);
			}
			return num;
		}

		// Token: 0x060001A1 RID: 417 RVA: 0x0000E444 File Offset: 0x0000C644
		public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
		{
			bool flag = true;
			Camera camera = renderingData.cameraData.camera;
			FilteringSettings filteringSettings = default(FilteringSettings);
			filteringSettings.renderQueueRange = RenderQueueRange.all;
			filteringSettings.layerMask = -1;
			filteringSettings.renderingLayerMask = uint.MaxValue;
			filteringSettings.sortingLayerRange = SortingLayerRange.all;
			LayerUtility.InitializeBudget(this.m_Renderer2DData.lightRenderTextureMemoryBudget);
			ShadowRendering.InitializeBudget(this.m_Renderer2DData.shadowRenderTextureMemoryBudget);
			PixelPerfectCamera pixelPerfectCamera;
			camera.TryGetComponent<PixelPerfectCamera>(out pixelPerfectCamera);
			if (pixelPerfectCamera != null && pixelPerfectCamera.enabled && pixelPerfectCamera.offscreenRTSize != Vector2Int.zero)
			{
				int x = pixelPerfectCamera.offscreenRTSize.x;
				int y = pixelPerfectCamera.offscreenRTSize.y;
				renderingData.commandBuffer.SetGlobalVector(ShaderPropertyId.screenParams, new Vector4((float)x, (float)y, 1f + 1f / (float)x, 1f + 1f / (float)y));
			}
			if (this.m_Renderer2DData.lightCullResult.IsSceneLit())
			{
				DrawingSettings drawingSettings = base.CreateDrawingSettings(Render2DLightingPass.k_ShaderTags, ref renderingData, SortingCriteria.CommonTransparent);
				DrawingSettings drawingSettings2 = base.CreateDrawingSettings(Render2DLightingPass.k_NormalsRenderingPassName, ref renderingData, SortingCriteria.CommonTransparent);
				SortingSettings sortingSettings = drawingSettings.sortingSettings;
				this.GetTransparencySortingMode(camera, ref sortingSettings);
				drawingSettings.sortingSettings = sortingSettings;
				drawingSettings2.sortingSettings = sortingSettings;
				CommandBuffer commandBuffer = renderingData.commandBuffer;
				commandBuffer.SetGlobalFloat(Render2DLightingPass.k_HDREmulationScaleID, this.m_Renderer2DData.hdrEmulationScale);
				commandBuffer.SetGlobalFloat(Render2DLightingPass.k_InverseHDREmulationScaleID, 1f / this.m_Renderer2DData.hdrEmulationScale);
				commandBuffer.SetGlobalFloat(Render2DLightingPass.k_UseSceneLightingID, flag ? 1f : 0f);
				commandBuffer.SetGlobalColor(Render2DLightingPass.k_RendererColorID, Color.white);
				this.SetShapeLightShaderGlobals(commandBuffer);
				RenderTextureDescriptor blendStyleRenderTextureDesc = this.GetBlendStyleRenderTextureDesc(renderingData);
				int num;
				LayerBatch[] array = LayerUtility.CalculateBatches(this.m_Renderer2DData, out num);
				int num2;
				for (int i = 0; i < num; i += num2)
				{
					num2 = this.DrawLayerBatches(array, num, i, commandBuffer, context, ref renderingData, ref drawingSettings2, ref drawingSettings, ref blendStyleRenderTextureDesc);
				}
				this.DisableAllKeywords(commandBuffer);
				this.ReleaseRenderTextures(commandBuffer);
				context.ExecuteCommandBuffer(commandBuffer);
				commandBuffer.Clear();
			}
			else
			{
				DrawingSettings drawingSettings3 = base.CreateDrawingSettings(Render2DLightingPass.k_ShaderTags, ref renderingData, SortingCriteria.CommonTransparent);
				RenderBufferStoreAction renderBufferStoreAction = ((renderingData.cameraData.cameraTargetDescriptor.msaaSamples > 1) ? RenderBufferStoreAction.Resolve : RenderBufferStoreAction.Store);
				SortingSettings sortingSettings2 = drawingSettings3.sortingSettings;
				this.GetTransparencySortingMode(camera, ref sortingSettings2);
				drawingSettings3.sortingSettings = sortingSettings2;
				CommandBuffer commandBuffer2 = renderingData.commandBuffer;
				using (new ProfilingScope(commandBuffer2, Render2DLightingPass.m_ProfilingSamplerUnlit))
				{
					CoreUtils.SetRenderTarget(commandBuffer2, base.colorAttachmentHandle, RenderBufferLoadAction.Load, renderBufferStoreAction, base.depthAttachmentHandle, RenderBufferLoadAction.Load, renderBufferStoreAction, ClearFlag.None, Color.clear, 0, CubemapFace.Unknown, -1);
					commandBuffer2.SetGlobalFloat(Render2DLightingPass.k_UseSceneLightingID, flag ? 1f : 0f);
					commandBuffer2.SetGlobalColor(Render2DLightingPass.k_RendererColorID, Color.white);
					for (int j = 0; j < Render2DLightingPass.k_ShapeLightTextureIDs.Length; j++)
					{
						if (j == 0)
						{
							commandBuffer2.SetGlobalTexture(Render2DLightingPass.k_ShapeLightTextureIDs[j], Texture2D.blackTexture);
						}
						RendererLighting.EnableBlendStyle(commandBuffer2, j, j == 0);
					}
				}
				this.DisableAllKeywords(commandBuffer2);
				context.ExecuteCommandBuffer(commandBuffer2);
				commandBuffer2.Clear();
				if (this.m_Renderer2DData.useCameraSortingLayerTexture)
				{
					filteringSettings.sortingLayerRange = new SortingLayerRange(short.MinValue, this.m_CameraSortingLayerBoundsIndex);
					this.Render(context, commandBuffer2, ref renderingData, ref filteringSettings, drawingSettings3);
					this.CopyCameraSortingLayerRenderTexture(context, renderingData, renderBufferStoreAction);
					filteringSettings.sortingLayerRange = new SortingLayerRange(this.m_CameraSortingLayerBoundsIndex + 1, short.MaxValue);
					this.Render(context, commandBuffer2, ref renderingData, ref filteringSettings, drawingSettings3);
				}
				else
				{
					this.Render(context, commandBuffer2, ref renderingData, ref filteringSettings, drawingSettings3);
				}
			}
			filteringSettings.sortingLayerRange = SortingLayerRange.all;
		}

		// Token: 0x17000052 RID: 82
		// (get) Token: 0x060001A2 RID: 418 RVA: 0x0000E818 File Offset: 0x0000CA18
		Renderer2DData IRenderPass2D.rendererData
		{
			get
			{
				return this.m_Renderer2DData;
			}
		}

		// Token: 0x040000EA RID: 234
		private static readonly int k_HDREmulationScaleID = Shader.PropertyToID("_HDREmulationScale");

		// Token: 0x040000EB RID: 235
		private static readonly int k_InverseHDREmulationScaleID = Shader.PropertyToID("_InverseHDREmulationScale");

		// Token: 0x040000EC RID: 236
		private static readonly int k_UseSceneLightingID = Shader.PropertyToID("_UseSceneLighting");

		// Token: 0x040000ED RID: 237
		private static readonly int k_RendererColorID = Shader.PropertyToID("_RendererColor");

		// Token: 0x040000EE RID: 238
		private static readonly int[] k_ShapeLightTextureIDs = new int[]
		{
			Shader.PropertyToID("_ShapeLightTexture0"),
			Shader.PropertyToID("_ShapeLightTexture1"),
			Shader.PropertyToID("_ShapeLightTexture2"),
			Shader.PropertyToID("_ShapeLightTexture3")
		};

		// Token: 0x040000EF RID: 239
		private static readonly ShaderTagId k_CombinedRenderingPassName = new ShaderTagId("Universal2D");

		// Token: 0x040000F0 RID: 240
		private static readonly ShaderTagId k_NormalsRenderingPassName = new ShaderTagId("NormalsRendering");

		// Token: 0x040000F1 RID: 241
		private static readonly ShaderTagId k_LegacyPassName = new ShaderTagId("SRPDefaultUnlit");

		// Token: 0x040000F2 RID: 242
		private static readonly List<ShaderTagId> k_ShaderTags = new List<ShaderTagId>
		{
			Render2DLightingPass.k_LegacyPassName,
			Render2DLightingPass.k_CombinedRenderingPassName
		};

		// Token: 0x040000F3 RID: 243
		private static readonly ProfilingSampler m_ProfilingDrawLights = new ProfilingSampler("Draw 2D Lights");

		// Token: 0x040000F4 RID: 244
		private static readonly ProfilingSampler m_ProfilingDrawLightTextures = new ProfilingSampler("Draw 2D Lights Textures");

		// Token: 0x040000F5 RID: 245
		private static readonly ProfilingSampler m_ProfilingDrawRenderers = new ProfilingSampler("Draw All Renderers");

		// Token: 0x040000F6 RID: 246
		private static readonly ProfilingSampler m_ProfilingDrawLayerBatch = new ProfilingSampler("Draw Layer Batch");

		// Token: 0x040000F7 RID: 247
		private static readonly ProfilingSampler m_ProfilingSamplerUnlit = new ProfilingSampler("Render Unlit");

		// Token: 0x040000F8 RID: 248
		private Material m_BlitMaterial;

		// Token: 0x040000F9 RID: 249
		private Material m_SamplingMaterial;

		// Token: 0x040000FA RID: 250
		private readonly Renderer2DData m_Renderer2DData;

		// Token: 0x040000FB RID: 251
		private bool m_NeedsDepth;

		// Token: 0x040000FC RID: 252
		private short m_CameraSortingLayerBoundsIndex;
	}
}
