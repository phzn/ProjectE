using System;

namespace UnityEngine.Rendering.Universal
{
	// Token: 0x020000C8 RID: 200
	[DisallowMultipleRendererFeature("Screen Space Ambient Occlusion")]
	[Tooltip("The Ambient Occlusion effect darkens creases, holes, intersections and surfaces that are close to each other.")]
	internal class ScreenSpaceAmbientOcclusion : ScriptableRendererFeature
	{
		// Token: 0x1700018A RID: 394
		// (get) Token: 0x06000630 RID: 1584 RVA: 0x00025E5B File Offset: 0x0002405B
		internal ref ScreenSpaceAmbientOcclusionSettings settings
		{
			get
			{
				return ref this.m_Settings;
			}
		}

		// Token: 0x06000631 RID: 1585 RVA: 0x00025E64 File Offset: 0x00024064
		public override void Create()
		{
			if (this.m_SSAOPass == null)
			{
				this.m_SSAOPass = new ScreenSpaceAmbientOcclusion.ScreenSpaceAmbientOcclusionPass();
			}
			if (this.m_Settings.SampleCount > 0)
			{
				this.m_Settings.AOMethod = ScreenSpaceAmbientOcclusionSettings.AOMethodOptions.InterleavedGradient;
				if (this.m_Settings.SampleCount > 11)
				{
					this.m_Settings.Samples = ScreenSpaceAmbientOcclusionSettings.AOSampleOption.High;
				}
				else if (this.m_Settings.SampleCount > 8)
				{
					this.m_Settings.Samples = ScreenSpaceAmbientOcclusionSettings.AOSampleOption.Medium;
				}
				else
				{
					this.m_Settings.Samples = ScreenSpaceAmbientOcclusionSettings.AOSampleOption.Low;
				}
				this.m_Settings.SampleCount = -1;
			}
		}

		// Token: 0x06000632 RID: 1586 RVA: 0x00025EF0 File Offset: 0x000240F0
		public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
		{
			if (UniversalRenderer.IsOffscreenDepthTexture(in renderingData.cameraData))
			{
				return;
			}
			if (!this.GetMaterials())
			{
				Debug.LogErrorFormat("{0}.AddRenderPasses(): Missing material. {1} render pass will not be added.", new object[]
				{
					base.GetType().Name,
					base.name
				});
				return;
			}
			if (this.m_SSAOPass.Setup(ref this.m_Settings, ref renderer, ref this.m_Material, ref this.m_BlueNoise256Textures))
			{
				renderer.EnqueuePass(this.m_SSAOPass);
			}
		}

		// Token: 0x06000633 RID: 1587 RVA: 0x00025F68 File Offset: 0x00024168
		protected override void Dispose(bool disposing)
		{
			ScreenSpaceAmbientOcclusion.ScreenSpaceAmbientOcclusionPass ssaopass = this.m_SSAOPass;
			if (ssaopass != null)
			{
				ssaopass.Dispose();
			}
			this.m_SSAOPass = null;
			CoreUtils.Destroy(this.m_Material);
		}

		// Token: 0x06000634 RID: 1588 RVA: 0x00025F8D File Offset: 0x0002418D
		private bool GetMaterials()
		{
			if (this.m_Material == null && this.m_Shader != null)
			{
				this.m_Material = CoreUtils.CreateEngineMaterial(this.m_Shader);
			}
			return this.m_Material != null;
		}

		// Token: 0x04000514 RID: 1300
		[SerializeField]
		private ScreenSpaceAmbientOcclusionSettings m_Settings = new ScreenSpaceAmbientOcclusionSettings();

		// Token: 0x04000515 RID: 1301
		[SerializeField]
		[HideInInspector]
		[Reload("Textures/BlueNoise256/LDR_LLL1_{0}.png", 0, 7, ReloadAttribute.Package.Root)]
		internal Texture2D[] m_BlueNoise256Textures;

		// Token: 0x04000516 RID: 1302
		[SerializeField]
		[HideInInspector]
		[Reload("Shaders/Utils/ScreenSpaceAmbientOcclusion.shader", ReloadAttribute.Package.Root)]
		private Shader m_Shader;

		// Token: 0x04000517 RID: 1303
		private Material m_Material;

		// Token: 0x04000518 RID: 1304
		private ScreenSpaceAmbientOcclusion.ScreenSpaceAmbientOcclusionPass m_SSAOPass;

		// Token: 0x04000519 RID: 1305
		internal const string k_AOInterleavedGradientKeyword = "_INTERLEAVED_GRADIENT";

		// Token: 0x0400051A RID: 1306
		internal const string k_AOBlueNoiseKeyword = "_BLUE_NOISE";

		// Token: 0x0400051B RID: 1307
		internal const string k_OrthographicCameraKeyword = "_ORTHOGRAPHIC";

		// Token: 0x0400051C RID: 1308
		internal const string k_SourceDepthLowKeyword = "_SOURCE_DEPTH_LOW";

		// Token: 0x0400051D RID: 1309
		internal const string k_SourceDepthMediumKeyword = "_SOURCE_DEPTH_MEDIUM";

		// Token: 0x0400051E RID: 1310
		internal const string k_SourceDepthHighKeyword = "_SOURCE_DEPTH_HIGH";

		// Token: 0x0400051F RID: 1311
		internal const string k_SourceDepthNormalsKeyword = "_SOURCE_DEPTH_NORMALS";

		// Token: 0x04000520 RID: 1312
		internal const string k_SampleCountLowKeyword = "_SAMPLE_COUNT_LOW";

		// Token: 0x04000521 RID: 1313
		internal const string k_SampleCountMediumKeyword = "_SAMPLE_COUNT_MEDIUM";

		// Token: 0x04000522 RID: 1314
		internal const string k_SampleCountHighKeyword = "_SAMPLE_COUNT_HIGH";

		// Token: 0x020001B2 RID: 434
		internal class ScreenSpaceAmbientOcclusionPass : ScriptableRenderPass
		{
			// Token: 0x17000258 RID: 600
			// (get) Token: 0x06000B0A RID: 2826 RVA: 0x0004A5D3 File Offset: 0x000487D3
			private bool isRendererDeferred
			{
				get
				{
					return this.m_Renderer != null && this.m_Renderer is UniversalRenderer && ((UniversalRenderer)this.m_Renderer).renderingModeRequested == RenderingMode.Deferred;
				}
			}

			// Token: 0x06000B0B RID: 2827 RVA: 0x0004A600 File Offset: 0x00048800
			internal ScreenSpaceAmbientOcclusionPass()
			{
				this.m_CurrentSettings = new ScreenSpaceAmbientOcclusionSettings();
			}

			// Token: 0x06000B0C RID: 2828 RVA: 0x0004A680 File Offset: 0x00048880
			internal bool Setup(ref ScreenSpaceAmbientOcclusionSettings featureSettings, ref ScriptableRenderer renderer, ref Material material, ref Texture2D[] blueNoiseTextures)
			{
				this.m_BlueNoiseTextures = blueNoiseTextures;
				this.m_Material = material;
				this.m_Renderer = renderer;
				this.m_CurrentSettings = featureSettings;
				if (this.isRendererDeferred)
				{
					base.renderPassEvent = (this.m_CurrentSettings.AfterOpaque ? RenderPassEvent.AfterRenderingOpaques : RenderPassEvent.AfterRenderingGbuffer);
					this.m_CurrentSettings.Source = ScreenSpaceAmbientOcclusionSettings.DepthSource.DepthNormals;
				}
				else
				{
					base.renderPassEvent = (this.m_CurrentSettings.AfterOpaque ? RenderPassEvent.AfterRenderingOpaques : ((RenderPassEvent)201));
				}
				ScreenSpaceAmbientOcclusionSettings.DepthSource source = this.m_CurrentSettings.Source;
				if (source != ScreenSpaceAmbientOcclusionSettings.DepthSource.Depth)
				{
					if (source != ScreenSpaceAmbientOcclusionSettings.DepthSource.DepthNormals)
					{
						throw new ArgumentOutOfRangeException();
					}
					base.ConfigureInput(ScriptableRenderPassInput.Normal);
				}
				else
				{
					base.ConfigureInput(ScriptableRenderPassInput.Depth);
				}
				switch (this.m_CurrentSettings.BlurQuality)
				{
				case ScreenSpaceAmbientOcclusionSettings.BlurQualityOptions.High:
					this.m_BlurType = ScreenSpaceAmbientOcclusion.ScreenSpaceAmbientOcclusionPass.BlurTypes.Bilateral;
					break;
				case ScreenSpaceAmbientOcclusionSettings.BlurQualityOptions.Medium:
					this.m_BlurType = ScreenSpaceAmbientOcclusion.ScreenSpaceAmbientOcclusionPass.BlurTypes.Gaussian;
					break;
				case ScreenSpaceAmbientOcclusionSettings.BlurQualityOptions.Low:
					this.m_BlurType = ScreenSpaceAmbientOcclusion.ScreenSpaceAmbientOcclusionPass.BlurTypes.Kawase;
					break;
				default:
					throw new ArgumentOutOfRangeException();
				}
				return this.m_Material != null && this.m_CurrentSettings.Intensity > 0f && this.m_CurrentSettings.Radius > 0f && this.m_CurrentSettings.Falloff > 0f;
			}

			// Token: 0x06000B0D RID: 2829 RVA: 0x0004A7B8 File Offset: 0x000489B8
			public override void OnCameraSetup(CommandBuffer cmd, ref RenderingData renderingData)
			{
				RenderTextureDescriptor cameraTargetDescriptor = renderingData.cameraData.cameraTargetDescriptor;
				int num = (this.m_CurrentSettings.Downsample ? 2 : 1);
				int num2 = ((renderingData.cameraData.xr.enabled && renderingData.cameraData.xr.singlePassEnabled) ? 2 : 1);
				for (int i = 0; i < num2; i++)
				{
					Matrix4x4 viewMatrix = renderingData.cameraData.GetViewMatrix(i);
					Matrix4x4 projectionMatrix = renderingData.cameraData.GetProjectionMatrix(i);
					this.m_CameraViewProjections[i] = projectionMatrix * viewMatrix;
					Matrix4x4 matrix4x = viewMatrix;
					matrix4x.SetColumn(3, new Vector4(0f, 0f, 0f, 1f));
					Matrix4x4 inverse = (projectionMatrix * matrix4x).inverse;
					Vector4 vector = inverse.MultiplyPoint(new Vector4(-1f, 1f, -1f, 1f));
					Vector4 vector2 = inverse.MultiplyPoint(new Vector4(1f, 1f, -1f, 1f));
					Vector4 vector3 = inverse.MultiplyPoint(new Vector4(-1f, -1f, -1f, 1f));
					Vector4 vector4 = inverse.MultiplyPoint(new Vector4(0f, 0f, 1f, 1f));
					this.m_CameraTopLeftCorner[i] = vector;
					this.m_CameraXExtent[i] = vector2 - vector;
					this.m_CameraYExtent[i] = vector3 - vector;
					this.m_CameraZExtent[i] = vector4;
				}
				this.m_Material.SetVector(ScreenSpaceAmbientOcclusion.ScreenSpaceAmbientOcclusionPass.s_ProjectionParams2ID, new Vector4(1f / renderingData.cameraData.camera.nearClipPlane, 0f, 0f, 0f));
				this.m_Material.SetMatrixArray(ScreenSpaceAmbientOcclusion.ScreenSpaceAmbientOcclusionPass.s_CameraViewProjectionsID, this.m_CameraViewProjections);
				this.m_Material.SetVectorArray(ScreenSpaceAmbientOcclusion.ScreenSpaceAmbientOcclusionPass.s_CameraViewTopLeftCornerID, this.m_CameraTopLeftCorner);
				this.m_Material.SetVectorArray(ScreenSpaceAmbientOcclusion.ScreenSpaceAmbientOcclusionPass.s_CameraViewXExtentID, this.m_CameraXExtent);
				this.m_Material.SetVectorArray(ScreenSpaceAmbientOcclusion.ScreenSpaceAmbientOcclusionPass.s_CameraViewYExtentID, this.m_CameraYExtent);
				this.m_Material.SetVectorArray(ScreenSpaceAmbientOcclusion.ScreenSpaceAmbientOcclusionPass.s_CameraViewZExtentID, this.m_CameraZExtent);
				CoreUtils.SetKeyword(this.m_Material, "_ORTHOGRAPHIC", renderingData.cameraData.camera.orthographic);
				CoreUtils.SetKeyword(this.m_Material, "_BLUE_NOISE", false);
				CoreUtils.SetKeyword(this.m_Material, "_INTERLEAVED_GRADIENT", false);
				ScreenSpaceAmbientOcclusionSettings.AOMethodOptions aomethod = this.m_CurrentSettings.AOMethod;
				if (aomethod != ScreenSpaceAmbientOcclusionSettings.AOMethodOptions.BlueNoise)
				{
					if (aomethod != ScreenSpaceAmbientOcclusionSettings.AOMethodOptions.InterleavedGradient)
					{
						throw new ArgumentOutOfRangeException();
					}
					CoreUtils.SetKeyword(this.m_Material, "_INTERLEAVED_GRADIENT", true);
					this.m_Material.SetVector(ScreenSpaceAmbientOcclusion.ScreenSpaceAmbientOcclusionPass.s_SSAOParamsID, new Vector4(this.m_CurrentSettings.Intensity, this.m_CurrentSettings.Radius, 1f / (float)num, this.m_CurrentSettings.Falloff));
				}
				else
				{
					CoreUtils.SetKeyword(this.m_Material, "_BLUE_NOISE", true);
					this.m_BlueNoiseTextureIndex = (this.m_BlueNoiseTextureIndex + 1) % this.m_BlueNoiseTextures.Length;
					this.m_BlurRandomOffsetX = Random.value;
					this.m_BlurRandomOffsetY = Random.value;
					Texture2D texture2D = this.m_BlueNoiseTextures[this.m_BlueNoiseTextureIndex];
					this.m_Material.SetTexture(ScreenSpaceAmbientOcclusion.ScreenSpaceAmbientOcclusionPass.s_BlueNoiseTextureID, texture2D);
					this.m_Material.SetVector(ScreenSpaceAmbientOcclusion.ScreenSpaceAmbientOcclusionPass.s_SSAOParamsID, new Vector4(this.m_CurrentSettings.Intensity, this.m_CurrentSettings.Radius * 1.5f, 1f / (float)num, this.m_CurrentSettings.Falloff));
					this.m_Material.SetVector(ScreenSpaceAmbientOcclusion.ScreenSpaceAmbientOcclusionPass.s_SSAOBlueNoiseParamsID, new Vector4((float)renderingData.cameraData.pixelWidth / (float)texture2D.width, (float)renderingData.cameraData.pixelHeight / (float)texture2D.height, this.m_BlurRandomOffsetX, this.m_BlurRandomOffsetY));
				}
				CoreUtils.SetKeyword(this.m_Material, "_SAMPLE_COUNT_LOW", false);
				CoreUtils.SetKeyword(this.m_Material, "_SAMPLE_COUNT_MEDIUM", false);
				CoreUtils.SetKeyword(this.m_Material, "_SAMPLE_COUNT_HIGH", false);
				ScreenSpaceAmbientOcclusionSettings.AOSampleOption samples = this.m_CurrentSettings.Samples;
				if (samples != ScreenSpaceAmbientOcclusionSettings.AOSampleOption.High)
				{
					if (samples != ScreenSpaceAmbientOcclusionSettings.AOSampleOption.Medium)
					{
						CoreUtils.SetKeyword(this.m_Material, "_SAMPLE_COUNT_LOW", true);
					}
					else
					{
						CoreUtils.SetKeyword(this.m_Material, "_SAMPLE_COUNT_MEDIUM", true);
					}
				}
				else
				{
					CoreUtils.SetKeyword(this.m_Material, "_SAMPLE_COUNT_HIGH", true);
				}
				CoreUtils.SetKeyword(this.m_Material, "_ORTHOGRAPHIC", renderingData.cameraData.camera.orthographic);
				if (this.m_CurrentSettings.Source == ScreenSpaceAmbientOcclusionSettings.DepthSource.Depth)
				{
					CoreUtils.SetKeyword(this.m_Material, "_SOURCE_DEPTH_NORMALS", false);
					switch (this.m_CurrentSettings.NormalSamples)
					{
					case ScreenSpaceAmbientOcclusionSettings.NormalQuality.Low:
						CoreUtils.SetKeyword(this.m_Material, "_SOURCE_DEPTH_LOW", true);
						CoreUtils.SetKeyword(this.m_Material, "_SOURCE_DEPTH_MEDIUM", false);
						CoreUtils.SetKeyword(this.m_Material, "_SOURCE_DEPTH_HIGH", false);
						break;
					case ScreenSpaceAmbientOcclusionSettings.NormalQuality.Medium:
						CoreUtils.SetKeyword(this.m_Material, "_SOURCE_DEPTH_LOW", false);
						CoreUtils.SetKeyword(this.m_Material, "_SOURCE_DEPTH_MEDIUM", true);
						CoreUtils.SetKeyword(this.m_Material, "_SOURCE_DEPTH_HIGH", false);
						break;
					case ScreenSpaceAmbientOcclusionSettings.NormalQuality.High:
						CoreUtils.SetKeyword(this.m_Material, "_SOURCE_DEPTH_LOW", false);
						CoreUtils.SetKeyword(this.m_Material, "_SOURCE_DEPTH_MEDIUM", false);
						CoreUtils.SetKeyword(this.m_Material, "_SOURCE_DEPTH_HIGH", true);
						break;
					default:
						throw new ArgumentOutOfRangeException();
					}
				}
				else
				{
					CoreUtils.SetKeyword(this.m_Material, "_SOURCE_DEPTH_LOW", false);
					CoreUtils.SetKeyword(this.m_Material, "_SOURCE_DEPTH_MEDIUM", false);
					CoreUtils.SetKeyword(this.m_Material, "_SOURCE_DEPTH_HIGH", false);
					CoreUtils.SetKeyword(this.m_Material, "_SOURCE_DEPTH_NORMALS", true);
				}
				RenderTextureDescriptor renderTextureDescriptor = cameraTargetDescriptor;
				renderTextureDescriptor.msaaSamples = 1;
				renderTextureDescriptor.depthBufferBits = 0;
				this.m_AOPassDescriptor = renderTextureDescriptor;
				this.m_AOPassDescriptor.width = this.m_AOPassDescriptor.width / num;
				this.m_AOPassDescriptor.height = this.m_AOPassDescriptor.height / num;
				this.m_AOPassDescriptor.colorFormat = ((this.m_SupportsR8RenderTextureFormat && this.m_BlurType > ScreenSpaceAmbientOcclusion.ScreenSpaceAmbientOcclusionPass.BlurTypes.Bilateral) ? RenderTextureFormat.R8 : RenderTextureFormat.ARGB32);
				RenderingUtils.ReAllocateIfNeeded(ref this.m_SSAOTextures[0], in this.m_AOPassDescriptor, FilterMode.Bilinear, TextureWrapMode.Clamp, false, 1, 0f, "_SSAO_OcclusionTexture0");
				RenderingUtils.ReAllocateIfNeeded(ref this.m_SSAOTextures[1], in this.m_AOPassDescriptor, FilterMode.Bilinear, TextureWrapMode.Clamp, false, 1, 0f, "_SSAO_OcclusionTexture1");
				RenderingUtils.ReAllocateIfNeeded(ref this.m_SSAOTextures[2], in this.m_AOPassDescriptor, FilterMode.Bilinear, TextureWrapMode.Clamp, false, 1, 0f, "_SSAO_OcclusionTexture2");
				this.m_AOPassDescriptor.width = this.m_AOPassDescriptor.width * num;
				this.m_AOPassDescriptor.height = this.m_AOPassDescriptor.height * num;
				this.m_AOPassDescriptor.colorFormat = (this.m_SupportsR8RenderTextureFormat ? RenderTextureFormat.R8 : RenderTextureFormat.ARGB32);
				RenderingUtils.ReAllocateIfNeeded(ref this.m_SSAOTextures[3], in this.m_AOPassDescriptor, FilterMode.Bilinear, TextureWrapMode.Clamp, false, 1, 0f, "_SSAO_OcclusionTexture");
				base.ConfigureTarget(this.m_CurrentSettings.AfterOpaque ? this.m_Renderer.cameraColorTargetHandle : this.m_SSAOTextures[3]);
				base.ConfigureClear(ClearFlag.None, Color.white);
			}

			// Token: 0x06000B0E RID: 2830 RVA: 0x0004AF20 File Offset: 0x00049120
			public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
			{
				if (this.m_Material == null)
				{
					Debug.LogErrorFormat("{0}.Execute(): Missing material. ScreenSpaceAmbientOcclusion pass will not execute. Check for missing reference in the renderer resources.", new object[] { base.GetType().Name });
					return;
				}
				CommandBuffer commandBuffer = renderingData.commandBuffer;
				using (new ProfilingScope(commandBuffer, this.m_ProfilingSampler))
				{
					if (!this.m_CurrentSettings.AfterOpaque)
					{
						CoreUtils.SetKeyword(commandBuffer, "_SCREEN_SPACE_OCCLUSION", true);
					}
					PostProcessUtils.SetSourceSize(commandBuffer, this.m_AOPassDescriptor);
					commandBuffer.SetGlobalTexture("_ScreenSpaceOcclusionTexture", this.m_SSAOTextures[3]);
					if (renderingData.cameraData.xr.supportsFoveatedRendering)
					{
						if (this.m_CurrentSettings.Downsample || SystemInfo.foveatedRenderingCaps == FoveatedRenderingCaps.NonUniformRaster || (SystemInfo.foveatedRenderingCaps == FoveatedRenderingCaps.FoveationImage && this.m_CurrentSettings.Source == ScreenSpaceAmbientOcclusionSettings.DepthSource.Depth))
						{
							commandBuffer.SetFoveatedRenderingMode(FoveatedRenderingMode.Disabled);
						}
						else if (SystemInfo.foveatedRenderingCaps == FoveatedRenderingCaps.FoveationImage)
						{
							commandBuffer.SetFoveatedRenderingMode(FoveatedRenderingMode.Enabled);
						}
					}
					if (this.m_BlurType == ScreenSpaceAmbientOcclusion.ScreenSpaceAmbientOcclusionPass.BlurTypes.Kawase)
					{
						commandBuffer.SetGlobalInt(ScreenSpaceAmbientOcclusion.ScreenSpaceAmbientOcclusionPass.s_LastKawasePass, 1);
						commandBuffer.SetGlobalFloat(ScreenSpaceAmbientOcclusion.ScreenSpaceAmbientOcclusionPass.s_KawaseBlurIterationID, 0f);
					}
					int[] array;
					ScreenSpaceAmbientOcclusion.ScreenSpaceAmbientOcclusionPass.ShaderPasses[] array2;
					ScreenSpaceAmbientOcclusion.ScreenSpaceAmbientOcclusionPass.GetPassOrder(this.m_BlurType, this.m_CurrentSettings.AfterOpaque, out array, out array2);
					RTHandle cameraDepthTargetHandle = this.m_Renderer.cameraDepthTargetHandle;
					ScreenSpaceAmbientOcclusion.ScreenSpaceAmbientOcclusionPass.RenderAndSetBaseMap(ref commandBuffer, ref renderingData, ref this.m_Renderer, ref this.m_Material, ref cameraDepthTargetHandle, ref this.m_SSAOTextures[0], ScreenSpaceAmbientOcclusion.ScreenSpaceAmbientOcclusionPass.ShaderPasses.AmbientOcclusion);
					for (int i = 0; i < array2.Length; i++)
					{
						int num = array[i];
						int num2 = array[i + 1];
						ScreenSpaceAmbientOcclusion.ScreenSpaceAmbientOcclusionPass.RenderAndSetBaseMap(ref commandBuffer, ref renderingData, ref this.m_Renderer, ref this.m_Material, ref this.m_SSAOTextures[num], ref this.m_SSAOTextures[num2], array2[i]);
					}
					commandBuffer.SetGlobalVector(ScreenSpaceAmbientOcclusion.ScreenSpaceAmbientOcclusionPass.s_AmbientOcclusionParamID, new Vector4(1f, 0f, 0f, this.m_CurrentSettings.DirectLightingStrength));
				}
			}

			// Token: 0x06000B0F RID: 2831 RVA: 0x0004B118 File Offset: 0x00049318
			private static void RenderAndSetBaseMap(ref CommandBuffer cmd, ref RenderingData renderingData, ref ScriptableRenderer renderer, ref Material mat, ref RTHandle baseMap, ref RTHandle target, ScreenSpaceAmbientOcclusion.ScreenSpaceAmbientOcclusionPass.ShaderPasses pass)
			{
				if (ScreenSpaceAmbientOcclusion.ScreenSpaceAmbientOcclusionPass.IsAfterOpaquePass(ref pass))
				{
					Blitter.BlitCameraTexture(cmd, baseMap, renderer.cameraColorTargetHandle, RenderBufferLoadAction.Load, RenderBufferStoreAction.Store, mat, (int)pass);
					return;
				}
				if (baseMap.rt == null)
				{
					Vector2 vector = (baseMap.useScaling ? new Vector2(baseMap.rtHandleProperties.rtHandleScale.x, baseMap.rtHandleProperties.rtHandleScale.y) : Vector2.one);
					CoreUtils.SetRenderTarget(cmd, target, ClearFlag.None, 0, CubemapFace.Unknown, -1);
					Blitter.BlitTexture(cmd, baseMap.nameID, vector, mat, (int)pass);
					return;
				}
				Blitter.BlitCameraTexture(cmd, baseMap, target, mat, (int)pass);
			}

			// Token: 0x06000B10 RID: 2832 RVA: 0x0004B1CC File Offset: 0x000493CC
			private static void GetPassOrder(ScreenSpaceAmbientOcclusion.ScreenSpaceAmbientOcclusionPass.BlurTypes blurType, bool isAfterOpaque, out int[] textureIndices, out ScreenSpaceAmbientOcclusion.ScreenSpaceAmbientOcclusionPass.ShaderPasses[] shaderPasses)
			{
				switch (blurType)
				{
				case ScreenSpaceAmbientOcclusion.ScreenSpaceAmbientOcclusionPass.BlurTypes.Bilateral:
					textureIndices = ScreenSpaceAmbientOcclusion.ScreenSpaceAmbientOcclusionPass.m_BilateralTexturesIndices;
					shaderPasses = (isAfterOpaque ? ScreenSpaceAmbientOcclusion.ScreenSpaceAmbientOcclusionPass.m_BilateralAfterOpaquePasses : ScreenSpaceAmbientOcclusion.ScreenSpaceAmbientOcclusionPass.m_BilateralPasses);
					return;
				case ScreenSpaceAmbientOcclusion.ScreenSpaceAmbientOcclusionPass.BlurTypes.Gaussian:
					textureIndices = ScreenSpaceAmbientOcclusion.ScreenSpaceAmbientOcclusionPass.m_GaussianTexturesIndices;
					shaderPasses = (isAfterOpaque ? ScreenSpaceAmbientOcclusion.ScreenSpaceAmbientOcclusionPass.m_GaussianAfterOpaquePasses : ScreenSpaceAmbientOcclusion.ScreenSpaceAmbientOcclusionPass.m_GaussianPasses);
					return;
				case ScreenSpaceAmbientOcclusion.ScreenSpaceAmbientOcclusionPass.BlurTypes.Kawase:
					textureIndices = ScreenSpaceAmbientOcclusion.ScreenSpaceAmbientOcclusionPass.m_KawaseTexturesIndices;
					shaderPasses = (isAfterOpaque ? ScreenSpaceAmbientOcclusion.ScreenSpaceAmbientOcclusionPass.m_KawaseAfterOpaquePasses : ScreenSpaceAmbientOcclusion.ScreenSpaceAmbientOcclusionPass.m_KawasePasses);
					return;
				default:
					throw new ArgumentOutOfRangeException();
				}
			}

			// Token: 0x06000B11 RID: 2833 RVA: 0x0004B23D File Offset: 0x0004943D
			private static bool IsAfterOpaquePass(ref ScreenSpaceAmbientOcclusion.ScreenSpaceAmbientOcclusionPass.ShaderPasses pass)
			{
				return pass == ScreenSpaceAmbientOcclusion.ScreenSpaceAmbientOcclusionPass.ShaderPasses.BilateralAfterOpaque || pass == ScreenSpaceAmbientOcclusion.ScreenSpaceAmbientOcclusionPass.ShaderPasses.GaussianAfterOpaque || pass == ScreenSpaceAmbientOcclusion.ScreenSpaceAmbientOcclusionPass.ShaderPasses.KawaseAfterOpaque;
			}

			// Token: 0x06000B12 RID: 2834 RVA: 0x0004B251 File Offset: 0x00049451
			public override void OnCameraCleanup(CommandBuffer cmd)
			{
				if (cmd == null)
				{
					throw new ArgumentNullException("cmd");
				}
				if (!this.m_CurrentSettings.AfterOpaque)
				{
					CoreUtils.SetKeyword(cmd, "_SCREEN_SPACE_OCCLUSION", false);
				}
			}

			// Token: 0x06000B13 RID: 2835 RVA: 0x0004B27C File Offset: 0x0004947C
			public void Dispose()
			{
				RTHandle rthandle = this.m_SSAOTextures[0];
				if (rthandle != null)
				{
					rthandle.Release();
				}
				RTHandle rthandle2 = this.m_SSAOTextures[1];
				if (rthandle2 != null)
				{
					rthandle2.Release();
				}
				RTHandle rthandle3 = this.m_SSAOTextures[2];
				if (rthandle3 != null)
				{
					rthandle3.Release();
				}
				RTHandle rthandle4 = this.m_SSAOTextures[3];
				if (rthandle4 == null)
				{
					return;
				}
				rthandle4.Release();
			}

			// Token: 0x04000B46 RID: 2886
			internal string profilerTag;

			// Token: 0x04000B47 RID: 2887
			private bool m_SupportsR8RenderTextureFormat = SystemInfo.SupportsRenderTextureFormat(RenderTextureFormat.R8);

			// Token: 0x04000B48 RID: 2888
			private int m_BlueNoiseTextureIndex;

			// Token: 0x04000B49 RID: 2889
			private float m_BlurRandomOffsetX;

			// Token: 0x04000B4A RID: 2890
			private float m_BlurRandomOffsetY;

			// Token: 0x04000B4B RID: 2891
			private Material m_Material;

			// Token: 0x04000B4C RID: 2892
			private Texture2D[] m_BlueNoiseTextures;

			// Token: 0x04000B4D RID: 2893
			private Vector4[] m_CameraTopLeftCorner = new Vector4[2];

			// Token: 0x04000B4E RID: 2894
			private Vector4[] m_CameraXExtent = new Vector4[2];

			// Token: 0x04000B4F RID: 2895
			private Vector4[] m_CameraYExtent = new Vector4[2];

			// Token: 0x04000B50 RID: 2896
			private Vector4[] m_CameraZExtent = new Vector4[2];

			// Token: 0x04000B51 RID: 2897
			private RTHandle[] m_SSAOTextures = new RTHandle[4];

			// Token: 0x04000B52 RID: 2898
			private ScreenSpaceAmbientOcclusion.ScreenSpaceAmbientOcclusionPass.BlurTypes m_BlurType;

			// Token: 0x04000B53 RID: 2899
			private Matrix4x4[] m_CameraViewProjections = new Matrix4x4[2];

			// Token: 0x04000B54 RID: 2900
			private ProfilingSampler m_ProfilingSampler = ProfilingSampler.Get<URPProfileId>(URPProfileId.SSAO);

			// Token: 0x04000B55 RID: 2901
			private ScriptableRenderer m_Renderer;

			// Token: 0x04000B56 RID: 2902
			private RenderTextureDescriptor m_AOPassDescriptor;

			// Token: 0x04000B57 RID: 2903
			private ScreenSpaceAmbientOcclusionSettings m_CurrentSettings;

			// Token: 0x04000B58 RID: 2904
			private const int k_FinalTexID = 3;

			// Token: 0x04000B59 RID: 2905
			private const string k_SSAOTextureName = "_ScreenSpaceOcclusionTexture";

			// Token: 0x04000B5A RID: 2906
			private const string k_AmbientOcclusionParamName = "_AmbientOcclusionParam";

			// Token: 0x04000B5B RID: 2907
			internal static readonly int s_AmbientOcclusionParamID = Shader.PropertyToID("_AmbientOcclusionParam");

			// Token: 0x04000B5C RID: 2908
			private static readonly int s_SSAOParamsID = Shader.PropertyToID("_SSAOParams");

			// Token: 0x04000B5D RID: 2909
			private static readonly int s_SSAOBlueNoiseParamsID = Shader.PropertyToID("_SSAOBlueNoiseParams");

			// Token: 0x04000B5E RID: 2910
			private static readonly int s_LastKawasePass = Shader.PropertyToID("_LastKawasePass");

			// Token: 0x04000B5F RID: 2911
			private static readonly int s_BlueNoiseTextureID = Shader.PropertyToID("_BlueNoiseTexture");

			// Token: 0x04000B60 RID: 2912
			private static readonly int s_CameraViewXExtentID = Shader.PropertyToID("_CameraViewXExtent");

			// Token: 0x04000B61 RID: 2913
			private static readonly int s_CameraViewYExtentID = Shader.PropertyToID("_CameraViewYExtent");

			// Token: 0x04000B62 RID: 2914
			private static readonly int s_CameraViewZExtentID = Shader.PropertyToID("_CameraViewZExtent");

			// Token: 0x04000B63 RID: 2915
			private static readonly int s_ProjectionParams2ID = Shader.PropertyToID("_ProjectionParams2");

			// Token: 0x04000B64 RID: 2916
			private static readonly int s_KawaseBlurIterationID = Shader.PropertyToID("_KawaseBlurIteration");

			// Token: 0x04000B65 RID: 2917
			private static readonly int s_CameraViewProjectionsID = Shader.PropertyToID("_CameraViewProjections");

			// Token: 0x04000B66 RID: 2918
			private static readonly int s_CameraViewTopLeftCornerID = Shader.PropertyToID("_CameraViewTopLeftCorner");

			// Token: 0x04000B67 RID: 2919
			private static readonly int[] m_BilateralTexturesIndices = new int[] { 0, 1, 2, 3 };

			// Token: 0x04000B68 RID: 2920
			private static readonly ScreenSpaceAmbientOcclusion.ScreenSpaceAmbientOcclusionPass.ShaderPasses[] m_BilateralPasses = new ScreenSpaceAmbientOcclusion.ScreenSpaceAmbientOcclusionPass.ShaderPasses[]
			{
				ScreenSpaceAmbientOcclusion.ScreenSpaceAmbientOcclusionPass.ShaderPasses.BilateralBlurHorizontal,
				ScreenSpaceAmbientOcclusion.ScreenSpaceAmbientOcclusionPass.ShaderPasses.BilateralBlurVertical,
				ScreenSpaceAmbientOcclusion.ScreenSpaceAmbientOcclusionPass.ShaderPasses.BilateralBlurFinal
			};

			// Token: 0x04000B69 RID: 2921
			private static readonly ScreenSpaceAmbientOcclusion.ScreenSpaceAmbientOcclusionPass.ShaderPasses[] m_BilateralAfterOpaquePasses = new ScreenSpaceAmbientOcclusion.ScreenSpaceAmbientOcclusionPass.ShaderPasses[]
			{
				ScreenSpaceAmbientOcclusion.ScreenSpaceAmbientOcclusionPass.ShaderPasses.BilateralBlurHorizontal,
				ScreenSpaceAmbientOcclusion.ScreenSpaceAmbientOcclusionPass.ShaderPasses.BilateralBlurVertical,
				ScreenSpaceAmbientOcclusion.ScreenSpaceAmbientOcclusionPass.ShaderPasses.BilateralAfterOpaque
			};

			// Token: 0x04000B6A RID: 2922
			private static readonly int[] m_GaussianTexturesIndices = new int[] { 0, 1, 3, 3 };

			// Token: 0x04000B6B RID: 2923
			private static readonly ScreenSpaceAmbientOcclusion.ScreenSpaceAmbientOcclusionPass.ShaderPasses[] m_GaussianPasses = new ScreenSpaceAmbientOcclusion.ScreenSpaceAmbientOcclusionPass.ShaderPasses[]
			{
				ScreenSpaceAmbientOcclusion.ScreenSpaceAmbientOcclusionPass.ShaderPasses.GaussianBlurHorizontal,
				ScreenSpaceAmbientOcclusion.ScreenSpaceAmbientOcclusionPass.ShaderPasses.GaussianBlurVertical
			};

			// Token: 0x04000B6C RID: 2924
			private static readonly ScreenSpaceAmbientOcclusion.ScreenSpaceAmbientOcclusionPass.ShaderPasses[] m_GaussianAfterOpaquePasses = new ScreenSpaceAmbientOcclusion.ScreenSpaceAmbientOcclusionPass.ShaderPasses[]
			{
				ScreenSpaceAmbientOcclusion.ScreenSpaceAmbientOcclusionPass.ShaderPasses.GaussianBlurHorizontal,
				ScreenSpaceAmbientOcclusion.ScreenSpaceAmbientOcclusionPass.ShaderPasses.GaussianAfterOpaque
			};

			// Token: 0x04000B6D RID: 2925
			private static readonly int[] m_KawaseTexturesIndices = new int[] { 0, 3 };

			// Token: 0x04000B6E RID: 2926
			private static readonly ScreenSpaceAmbientOcclusion.ScreenSpaceAmbientOcclusionPass.ShaderPasses[] m_KawasePasses = new ScreenSpaceAmbientOcclusion.ScreenSpaceAmbientOcclusionPass.ShaderPasses[] { ScreenSpaceAmbientOcclusion.ScreenSpaceAmbientOcclusionPass.ShaderPasses.KawaseBlur };

			// Token: 0x04000B6F RID: 2927
			private static readonly ScreenSpaceAmbientOcclusion.ScreenSpaceAmbientOcclusionPass.ShaderPasses[] m_KawaseAfterOpaquePasses = new ScreenSpaceAmbientOcclusion.ScreenSpaceAmbientOcclusionPass.ShaderPasses[] { ScreenSpaceAmbientOcclusion.ScreenSpaceAmbientOcclusionPass.ShaderPasses.KawaseAfterOpaque };

			// Token: 0x02000233 RID: 563
			private enum BlurTypes
			{
				// Token: 0x04000D3D RID: 3389
				Bilateral,
				// Token: 0x04000D3E RID: 3390
				Gaussian,
				// Token: 0x04000D3F RID: 3391
				Kawase
			}

			// Token: 0x02000234 RID: 564
			private enum ShaderPasses
			{
				// Token: 0x04000D41 RID: 3393
				AmbientOcclusion,
				// Token: 0x04000D42 RID: 3394
				BilateralBlurHorizontal,
				// Token: 0x04000D43 RID: 3395
				BilateralBlurVertical,
				// Token: 0x04000D44 RID: 3396
				BilateralBlurFinal,
				// Token: 0x04000D45 RID: 3397
				BilateralAfterOpaque,
				// Token: 0x04000D46 RID: 3398
				GaussianBlurHorizontal,
				// Token: 0x04000D47 RID: 3399
				GaussianBlurVertical,
				// Token: 0x04000D48 RID: 3400
				GaussianAfterOpaque,
				// Token: 0x04000D49 RID: 3401
				KawaseBlur,
				// Token: 0x04000D4A RID: 3402
				KawaseAfterOpaque
			}
		}
	}
}
