using System;
using System.Collections.Generic;
using UnityEngine.Experimental.Rendering;

namespace UnityEngine.Rendering.Universal
{
	// Token: 0x0200003C RID: 60
	internal static class ShadowRendering
	{
		// Token: 0x1700008E RID: 142
		// (get) Token: 0x06000257 RID: 599 RVA: 0x00012150 File Offset: 0x00010350
		// (set) Token: 0x06000258 RID: 600 RVA: 0x00012157 File Offset: 0x00010357
		public static uint maxTextureCount { get; private set; }

		// Token: 0x06000259 RID: 601 RVA: 0x00012160 File Offset: 0x00010360
		public static void InitializeBudget(uint maxTextureCount)
		{
			if (ShadowRendering.m_RenderTargets == null || (long)ShadowRendering.m_RenderTargets.Length != (long)((ulong)maxTextureCount))
			{
				ShadowRendering.m_RenderTargets = new RTHandle[maxTextureCount];
				ShadowRendering.m_RenderTargetIds = new int[maxTextureCount];
				ShadowRendering.maxTextureCount = maxTextureCount;
				int num = 0;
				while ((long)num < (long)((ulong)maxTextureCount))
				{
					ShadowRendering.m_RenderTargetIds[num] = Shader.PropertyToID(string.Format("ShadowTex_{0}", num));
					ShadowRendering.m_RenderTargets[num] = RTHandles.Alloc(ShadowRendering.m_RenderTargetIds[num], string.Format("ShadowTex_{0}", num));
					num++;
				}
			}
			if (ShadowRendering.m_LightInputTextures == null || (long)ShadowRendering.m_LightInputTextures.Length != (long)((ulong)maxTextureCount))
			{
				ShadowRendering.m_LightInputTextures = new RenderTargetIdentifier[maxTextureCount];
			}
		}

		// Token: 0x0600025A RID: 602 RVA: 0x0001220C File Offset: 0x0001040C
		private static Material[] CreateMaterials(Shader shader, int pass = 0)
		{
			Material[] array = new Material[4];
			for (int i = 0; i < 4; i++)
			{
				array[i] = CoreUtils.CreateEngineMaterial(shader);
				array[i].SetInt(ShadowRendering.k_ShadowColorMaskID, 1 << i);
				array[i].SetPass(pass);
			}
			return array;
		}

		// Token: 0x0600025B RID: 603 RVA: 0x00012254 File Offset: 0x00010454
		private static Material GetProjectedShadowMaterial(this Renderer2DData rendererData, int colorIndex)
		{
			if (rendererData.projectedShadowMaterial == null || rendererData.projectedShadowMaterial.Length == 0 || rendererData.projectedShadowShader != rendererData.projectedShadowMaterial[0].shader)
			{
				rendererData.projectedShadowMaterial = ShadowRendering.CreateMaterials(rendererData.projectedShadowShader, 0);
			}
			return rendererData.projectedShadowMaterial[colorIndex];
		}

		// Token: 0x0600025C RID: 604 RVA: 0x000122A8 File Offset: 0x000104A8
		private static Material GetStencilOnlyShadowMaterial(this Renderer2DData rendererData, int colorIndex)
		{
			if (rendererData.stencilOnlyShadowMaterial == null || rendererData.stencilOnlyShadowMaterial.Length == 0 || rendererData.projectedShadowShader != rendererData.stencilOnlyShadowMaterial[0].shader)
			{
				rendererData.stencilOnlyShadowMaterial = ShadowRendering.CreateMaterials(rendererData.projectedShadowShader, 1);
			}
			return rendererData.stencilOnlyShadowMaterial[colorIndex];
		}

		// Token: 0x0600025D RID: 605 RVA: 0x000122FC File Offset: 0x000104FC
		private static Material GetSpriteSelfShadowMaterial(this Renderer2DData rendererData, int colorIndex)
		{
			if (rendererData.spriteSelfShadowMaterial == null || rendererData.spriteSelfShadowMaterial.Length == 0 || rendererData.spriteShadowShader != rendererData.spriteSelfShadowMaterial[0].shader)
			{
				rendererData.spriteSelfShadowMaterial = ShadowRendering.CreateMaterials(rendererData.spriteShadowShader, 0);
			}
			return rendererData.spriteSelfShadowMaterial[colorIndex];
		}

		// Token: 0x0600025E RID: 606 RVA: 0x00012350 File Offset: 0x00010550
		private static Material GetSpriteUnshadowMaterial(this Renderer2DData rendererData, int colorIndex)
		{
			if (rendererData.spriteUnshadowMaterial == null || rendererData.spriteUnshadowMaterial.Length == 0 || rendererData.spriteUnshadowShader != rendererData.spriteUnshadowMaterial[0].shader)
			{
				rendererData.spriteUnshadowMaterial = ShadowRendering.CreateMaterials(rendererData.spriteUnshadowShader, 0);
			}
			return rendererData.spriteUnshadowMaterial[colorIndex];
		}

		// Token: 0x0600025F RID: 607 RVA: 0x000123A4 File Offset: 0x000105A4
		private static Material GetGeometryUnshadowMaterial(this Renderer2DData rendererData, int colorIndex)
		{
			if (rendererData.geometryUnshadowMaterial == null || rendererData.geometryUnshadowMaterial.Length == 0 || rendererData.geometryUnshadowShader != rendererData.geometryUnshadowMaterial[0].shader)
			{
				rendererData.geometryUnshadowMaterial = ShadowRendering.CreateMaterials(rendererData.geometryUnshadowShader, 0);
			}
			return rendererData.geometryUnshadowMaterial[colorIndex];
		}

		// Token: 0x06000260 RID: 608 RVA: 0x000123F6 File Offset: 0x000105F6
		public static void CreateShadowRenderTexture(IRenderPass2D pass, RenderingData renderingData, CommandBuffer cmdBuffer, int shadowIndex)
		{
			ShadowRendering.CreateShadowRenderTexture(pass, ShadowRendering.m_RenderTargetIds[shadowIndex], renderingData, cmdBuffer);
		}

		// Token: 0x06000261 RID: 609 RVA: 0x00012408 File Offset: 0x00010608
		public static bool PrerenderShadows(IRenderPass2D pass, RenderingData renderingData, CommandBuffer cmdBuffer, int layerToRender, Light2D light, int shadowIndex, float shadowIntensity)
		{
			int num = shadowIndex % 4;
			int num2 = shadowIndex / 4;
			if (num == 0)
			{
				ShadowRendering.CreateShadowRenderTexture(pass, renderingData, cmdBuffer, num2);
			}
			bool flag = ShadowRendering.RenderShadows(pass, renderingData, cmdBuffer, layerToRender, light, shadowIntensity, ShadowRendering.m_RenderTargets[num2].nameID, num);
			ShadowRendering.m_LightInputTextures[num2] = ShadowRendering.m_RenderTargets[num2].nameID;
			return flag;
		}

		// Token: 0x06000262 RID: 610 RVA: 0x0001245C File Offset: 0x0001065C
		public static void SetGlobalShadowTexture(CommandBuffer cmdBuffer, Light2D light, int shadowIndex)
		{
			int num = shadowIndex % 4;
			int num2 = shadowIndex / 4;
			cmdBuffer.SetGlobalTexture("_ShadowTex", ShadowRendering.m_LightInputTextures[num2]);
			cmdBuffer.SetGlobalColor(ShadowRendering.k_ShadowColorMaskID, ShadowRendering.k_ColorLookup[num]);
			cmdBuffer.SetGlobalFloat(ShadowRendering.k_ShadowIntensityID, 1f - light.shadowIntensity);
			cmdBuffer.SetGlobalFloat(ShadowRendering.k_ShadowVolumeIntensityID, 1f - light.shadowVolumeIntensity);
		}

		// Token: 0x06000263 RID: 611 RVA: 0x000124CB File Offset: 0x000106CB
		public static void DisableGlobalShadowTexture(CommandBuffer cmdBuffer)
		{
			cmdBuffer.SetGlobalFloat(ShadowRendering.k_ShadowIntensityID, 1f);
			cmdBuffer.SetGlobalFloat(ShadowRendering.k_ShadowVolumeIntensityID, 1f);
		}

		// Token: 0x06000264 RID: 612 RVA: 0x000124F0 File Offset: 0x000106F0
		private static void CreateShadowRenderTexture(IRenderPass2D pass, int handleId, RenderingData renderingData, CommandBuffer cmdBuffer)
		{
			float num = Mathf.Clamp(pass.rendererData.lightRenderTextureScale, 0.01f, 1f);
			int num2 = (int)((float)renderingData.cameraData.cameraTargetDescriptor.width * num);
			int num3 = (int)((float)renderingData.cameraData.cameraTargetDescriptor.height * num);
			cmdBuffer.GetTemporaryRT(handleId, new RenderTextureDescriptor(num2, num3)
			{
				useMipMap = false,
				autoGenerateMips = false,
				depthBufferBits = 24,
				graphicsFormat = GraphicsFormat.R8G8B8A8_UNorm,
				msaaSamples = 1,
				dimension = TextureDimension.Tex2D
			}, FilterMode.Bilinear);
		}

		// Token: 0x06000265 RID: 613 RVA: 0x00012588 File Offset: 0x00010788
		public static void ReleaseShadowRenderTexture(CommandBuffer cmdBuffer, int shadowIndex)
		{
			bool flag = shadowIndex % 4 != 0;
			int num = shadowIndex / 4;
			if (!flag)
			{
				cmdBuffer.ReleaseTemporaryRT(ShadowRendering.m_RenderTargetIds[num]);
			}
		}

		// Token: 0x06000266 RID: 614 RVA: 0x000125AB File Offset: 0x000107AB
		public static void SetShadowProjectionGlobals(CommandBuffer cmdBuffer, ShadowCaster2D shadowCaster)
		{
			cmdBuffer.SetGlobalVector(ShadowRendering.k_ShadowModelScaleID, shadowCaster.m_CachedLossyScale);
			cmdBuffer.SetGlobalMatrix(ShadowRendering.k_ShadowModelMatrixID, shadowCaster.m_CachedShadowMatrix);
			cmdBuffer.SetGlobalMatrix(ShadowRendering.k_ShadowModelInvMatrixID, shadowCaster.m_CachedInverseShadowMatrix);
		}

		// Token: 0x06000267 RID: 615 RVA: 0x000125E8 File Offset: 0x000107E8
		public static bool RenderShadows(IRenderPass2D pass, RenderingData renderingData, CommandBuffer cmdBuffer, int layerToRender, Light2D light, float shadowIntensity, RenderTargetIdentifier renderTexture, int colorBit)
		{
			bool flag2;
			using (new ProfilingScope(cmdBuffer, ShadowRendering.m_ProfilingSamplerShadows))
			{
				bool flag = false;
				List<ShadowCasterGroup2D> shadowCasterGroups = ShadowCasterGroup2DManager.shadowCasterGroups;
				if (shadowCasterGroups != null && shadowCasterGroups.Count > 0)
				{
					for (int i = 0; i < shadowCasterGroups.Count; i++)
					{
						List<ShadowCaster2D> shadowCasters = shadowCasterGroups[i].GetShadowCasters();
						if (shadowCasters != null)
						{
							for (int j = 0; j < shadowCasters.Count; j++)
							{
								ShadowCaster2D shadowCaster2D = shadowCasters[j];
								if (shadowCaster2D != null && shadowCaster2D.IsLit(light) && shadowCaster2D.IsShadowedLayer(layerToRender))
								{
									flag = true;
									break;
								}
							}
						}
					}
					if (flag)
					{
						cmdBuffer.SetRenderTarget(renderTexture, RenderBufferLoadAction.DontCare, RenderBufferStoreAction.Store, RenderBufferLoadAction.DontCare, RenderBufferStoreAction.DontCare);
						using (new ProfilingScope(cmdBuffer, ShadowRendering.m_ProfilingSamplerShadowColorsLookup[colorBit]))
						{
							if (colorBit == 0)
							{
								cmdBuffer.ClearRenderTarget(true, true, Color.clear);
							}
							else
							{
								cmdBuffer.ClearRenderTarget(true, false, Color.clear);
							}
							float radius = light.boundingSphere.radius;
							cmdBuffer.SetGlobalVector(ShadowRendering.k_LightPosID, light.transform.position);
							cmdBuffer.SetGlobalFloat(ShadowRendering.k_ShadowRadiusID, radius);
							cmdBuffer.SetGlobalColor(ShadowRendering.k_ShadowColorMaskID, ShadowRendering.k_ColorLookup[colorBit]);
							Material geometryUnshadowMaterial = pass.rendererData.GetGeometryUnshadowMaterial(colorBit);
							Material projectedShadowMaterial = pass.rendererData.GetProjectedShadowMaterial(colorBit);
							Material spriteSelfShadowMaterial = pass.rendererData.GetSpriteSelfShadowMaterial(colorBit);
							Material spriteUnshadowMaterial = pass.rendererData.GetSpriteUnshadowMaterial(colorBit);
							pass.rendererData.GetStencilOnlyShadowMaterial(colorBit);
							for (int k = 0; k < shadowCasterGroups.Count; k++)
							{
								List<ShadowCaster2D> shadowCasters2 = shadowCasterGroups[k].GetShadowCasters();
								if (shadowCasters2 != null)
								{
									for (int l = 0; l < shadowCasters2.Count; l++)
									{
										ShadowCaster2D shadowCaster2D2 = shadowCasters2[l];
										if (shadowCaster2D2.IsLit(light) && shadowCaster2D2 != null && projectedShadowMaterial != null && shadowCaster2D2.IsShadowedLayer(layerToRender) && shadowCaster2D2.castsShadows)
										{
											ShadowRendering.SetShadowProjectionGlobals(cmdBuffer, shadowCaster2D2);
											cmdBuffer.DrawMesh(shadowCaster2D2.mesh, shadowCaster2D2.m_CachedLocalToWorldMatrix, geometryUnshadowMaterial, 0, 0);
											cmdBuffer.DrawMesh(shadowCaster2D2.mesh, shadowCaster2D2.m_CachedLocalToWorldMatrix, projectedShadowMaterial, 0, 0);
											cmdBuffer.DrawMesh(shadowCaster2D2.mesh, shadowCaster2D2.m_CachedLocalToWorldMatrix, geometryUnshadowMaterial, 0, 1);
										}
									}
									for (int m = 0; m < shadowCasters2.Count; m++)
									{
										ShadowCaster2D shadowCaster2D3 = shadowCasters2[m];
										if (shadowCaster2D3.IsLit(light) && shadowCaster2D3 != null && shadowCaster2D3.IsShadowedLayer(layerToRender))
										{
											if (shadowCaster2D3.useRendererSilhouette)
											{
												Renderer renderer = null;
												shadowCaster2D3.TryGetComponent<Renderer>(out renderer);
												if (renderer != null)
												{
													Material material = (shadowCaster2D3.selfShadows ? spriteSelfShadowMaterial : spriteUnshadowMaterial);
													if (material != null)
													{
														cmdBuffer.DrawRenderer(renderer, material);
													}
												}
											}
											else
											{
												Matrix4x4 cachedLocalToWorldMatrix = shadowCaster2D3.m_CachedLocalToWorldMatrix;
												Material material2 = (shadowCaster2D3.selfShadows ? spriteSelfShadowMaterial : spriteUnshadowMaterial);
												if (material2 != null)
												{
													cmdBuffer.DrawMesh(shadowCaster2D3.mesh, cachedLocalToWorldMatrix, material2);
												}
											}
										}
									}
									for (int n = 0; n < shadowCasters2.Count; n++)
									{
										ShadowCaster2D shadowCaster2D4 = shadowCasters2[n];
										if (shadowCaster2D4.IsLit(light) && shadowCaster2D4 != null && projectedShadowMaterial != null && shadowCaster2D4.IsShadowedLayer(layerToRender) && shadowCaster2D4.castsShadows)
										{
											ShadowRendering.SetShadowProjectionGlobals(cmdBuffer, shadowCaster2D4);
											cmdBuffer.DrawMesh(shadowCaster2D4.mesh, shadowCaster2D4.m_CachedLocalToWorldMatrix, projectedShadowMaterial, 0, 1);
										}
									}
								}
							}
						}
					}
				}
				flag2 = flag;
			}
			return flag2;
		}

		// Token: 0x04000195 RID: 405
		private static readonly int k_LightPosID = Shader.PropertyToID("_LightPos");

		// Token: 0x04000196 RID: 406
		private static readonly int k_SelfShadowingID = Shader.PropertyToID("_SelfShadowing");

		// Token: 0x04000197 RID: 407
		private static readonly int k_ShadowStencilGroupID = Shader.PropertyToID("_ShadowStencilGroup");

		// Token: 0x04000198 RID: 408
		private static readonly int k_ShadowIntensityID = Shader.PropertyToID("_ShadowIntensity");

		// Token: 0x04000199 RID: 409
		private static readonly int k_ShadowVolumeIntensityID = Shader.PropertyToID("_ShadowVolumeIntensity");

		// Token: 0x0400019A RID: 410
		private static readonly int k_ShadowRadiusID = Shader.PropertyToID("_ShadowRadius");

		// Token: 0x0400019B RID: 411
		private static readonly int k_ShadowColorMaskID = Shader.PropertyToID("_ShadowColorMask");

		// Token: 0x0400019C RID: 412
		private static readonly int k_ShadowModelMatrixID = Shader.PropertyToID("_ShadowModelMatrix");

		// Token: 0x0400019D RID: 413
		private static readonly int k_ShadowModelInvMatrixID = Shader.PropertyToID("_ShadowModelInvMatrix");

		// Token: 0x0400019E RID: 414
		private static readonly int k_ShadowModelScaleID = Shader.PropertyToID("_ShadowModelScale");

		// Token: 0x0400019F RID: 415
		private static readonly ProfilingSampler m_ProfilingSamplerShadows = new ProfilingSampler("Draw 2D Shadow Texture");

		// Token: 0x040001A0 RID: 416
		private static readonly ProfilingSampler m_ProfilingSamplerShadowsA = new ProfilingSampler("Draw 2D Shadows (A)");

		// Token: 0x040001A1 RID: 417
		private static readonly ProfilingSampler m_ProfilingSamplerShadowsR = new ProfilingSampler("Draw 2D Shadows (R)");

		// Token: 0x040001A2 RID: 418
		private static readonly ProfilingSampler m_ProfilingSamplerShadowsG = new ProfilingSampler("Draw 2D Shadows (G)");

		// Token: 0x040001A3 RID: 419
		private static readonly ProfilingSampler m_ProfilingSamplerShadowsB = new ProfilingSampler("Draw 2D Shadows (B)");

		// Token: 0x040001A4 RID: 420
		private static RTHandle[] m_RenderTargets = null;

		// Token: 0x040001A5 RID: 421
		private static int[] m_RenderTargetIds = null;

		// Token: 0x040001A6 RID: 422
		private static RenderTargetIdentifier[] m_LightInputTextures = null;

		// Token: 0x040001A7 RID: 423
		private static readonly Color[] k_ColorLookup = new Color[]
		{
			new Color(0f, 0f, 0f, 1f),
			new Color(0f, 0f, 1f, 0f),
			new Color(0f, 1f, 0f, 0f),
			new Color(1f, 0f, 0f, 0f)
		};

		// Token: 0x040001A8 RID: 424
		private static readonly ProfilingSampler[] m_ProfilingSamplerShadowColorsLookup = new ProfilingSampler[]
		{
			ShadowRendering.m_ProfilingSamplerShadowsA,
			ShadowRendering.m_ProfilingSamplerShadowsB,
			ShadowRendering.m_ProfilingSamplerShadowsG,
			ShadowRendering.m_ProfilingSamplerShadowsR
		};
	}
}
