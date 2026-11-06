using System;
using UnityEngine.Experimental.Rendering;
using UnityEngine.Experimental.Rendering.RenderGraphModule;
using UnityEngine.Serialization;

namespace UnityEngine.Rendering.Universal
{
	// Token: 0x020000DA RID: 218
	public static class TemporalAA
	{
		// Token: 0x060006DB RID: 1755 RVA: 0x00028BCC File Offset: 0x00026DCC
		internal static Matrix4x4 CalculateJitterMatrix(ref CameraData cameraData)
		{
			Matrix4x4 matrix4x = Matrix4x4.identity;
			if (cameraData.IsTemporalAAEnabled())
			{
				int jitterFrameCountOffset = cameraData.taaSettings.jitterFrameCountOffset;
				int num = Time.frameCount + jitterFrameCountOffset;
				float num2 = (float)cameraData.cameraTargetDescriptor.width;
				float num3 = (float)cameraData.cameraTargetDescriptor.height;
				float jitterScale = cameraData.taaSettings.jitterScale;
				Vector2 vector = TemporalAA.CalculateJitter(num) * jitterScale;
				float num4 = vector.x * (2f / num2);
				float num5 = vector.y * (2f / num3);
				matrix4x = Matrix4x4.Translate(new Vector3(num4, num5, 0f));
			}
			return matrix4x;
		}

		// Token: 0x060006DC RID: 1756 RVA: 0x00028C64 File Offset: 0x00026E64
		internal static Vector2 CalculateJitter(int frameIndex)
		{
			float num = HaltonSequence.Get((frameIndex & 1023) + 1, 2) - 0.5f;
			float num2 = HaltonSequence.Get((frameIndex & 1023) + 1, 3) - 0.5f;
			return new Vector2(num, num2);
		}

		// Token: 0x060006DD RID: 1757 RVA: 0x00028CA4 File Offset: 0x00026EA4
		internal static float[] CalculateFilterWeights(float jitterScale)
		{
			float num = 0f;
			for (int i = 0; i < 9; i++)
			{
				Vector2 vector = TemporalAA.CalculateJitter(Time.frameCount) * jitterScale;
				float num2 = TemporalAA.taaFilterOffsets[i].x - vector.x;
				float num3 = TemporalAA.taaFilterOffsets[i].y - vector.y;
				float num4 = num2 * num2 + num3 * num3;
				TemporalAA.taaFilterWeights[i] = Mathf.Exp(-2.2727273f * num4);
				num += TemporalAA.taaFilterWeights[i];
			}
			for (int j = 0; j < 9; j++)
			{
				TemporalAA.taaFilterWeights[j] /= num;
			}
			return TemporalAA.taaFilterWeights;
		}

		// Token: 0x060006DE RID: 1758 RVA: 0x00028D54 File Offset: 0x00026F54
		internal static string ValidateAndWarn(ref CameraData cameraData)
		{
			string text = null;
			if (cameraData.taaPersistentData == null)
			{
				text = "Disabling TAA due to invalid persistent data.";
			}
			if (text == null && cameraData.cameraTargetDescriptor.msaaSamples != 1)
			{
				if (cameraData.xr != null && cameraData.xr.enabled)
				{
					text = "Disabling TAA because MSAA is on. MSAA must be disabled globally for all cameras in XR mode.";
				}
				else
				{
					text = "Disabling TAA because MSAA is on.";
				}
			}
			UniversalAdditionalCameraData universalAdditionalCameraData;
			if (text == null && cameraData.camera.TryGetComponent<UniversalAdditionalCameraData>(out universalAdditionalCameraData) && (universalAdditionalCameraData.renderType == CameraRenderType.Overlay || universalAdditionalCameraData.cameraStack.Count > 0))
			{
				text = "Disabling TAA because camera is stacked.";
			}
			if (text == null && cameraData.camera.allowDynamicResolution)
			{
				text = "Disabling TAA because camera has dynamic resolution enabled. You can use a constant render scale instead.";
			}
			if (text == null && !cameraData.postProcessEnabled)
			{
				text = "Disabling TAA because camera has post-processing disabled.";
			}
			if (Time.frameCount % 60 == 0)
			{
				Debug.LogWarning(text);
			}
			return text;
		}

		// Token: 0x060006DF RID: 1759 RVA: 0x00028E0C File Offset: 0x0002700C
		internal static void ExecutePass(CommandBuffer cmd, Material taaMaterial, ref CameraData cameraData, RTHandle source, RTHandle destination, RenderTexture motionVectors)
		{
			using (new ProfilingScope(cmd, ProfilingSampler.Get<URPProfileId>(URPProfileId.TemporalAA)))
			{
				int multipassId = cameraData.xr.multipassId;
				bool flag = cameraData.taaPersistentData.GetLastAccumFrameIndex(multipassId) != Time.frameCount;
				RTHandle rthandle = cameraData.taaPersistentData.accumulationTexture(multipassId);
				taaMaterial.SetTexture(TemporalAA.ShaderConstants._TaaAccumulationTex, rthandle);
				taaMaterial.SetTexture(TemporalAA.ShaderConstants._TaaMotionVectorTex, flag ? motionVectors : Texture2D.blackTexture);
				ref TemporalAA.Settings ptr = ref cameraData.taaSettings;
				float num = ((ptr.resetHistoryFrames == 0) ? ptr.m_FrameInfluence : 1f);
				taaMaterial.SetFloat(TemporalAA.ShaderConstants._TaaFrameInfluence, num);
				taaMaterial.SetFloat(TemporalAA.ShaderConstants._TaaVarianceClampScale, ptr.varianceClampScale);
				if (ptr.quality == TemporalAAQuality.VeryHigh)
				{
					taaMaterial.SetFloatArray(TemporalAA.ShaderConstants._TaaFilterWeights, TemporalAA.CalculateFilterWeights(ptr.jitterScale));
				}
				GraphicsFormat graphicsFormat = rthandle.rt.graphicsFormat;
				if (graphicsFormat == GraphicsFormat.R8G8B8A8_UNorm || graphicsFormat == GraphicsFormat.B8G8R8A8_UNorm || graphicsFormat == GraphicsFormat.B10G11R11_UFloatPack32)
				{
					taaMaterial.EnableKeyword(TemporalAA.ShaderKeywords.TAA_LOW_PRECISION_SOURCE);
				}
				else
				{
					taaMaterial.DisableKeyword(TemporalAA.ShaderKeywords.TAA_LOW_PRECISION_SOURCE);
				}
				Blitter.BlitCameraTexture(cmd, source, destination, RenderBufferLoadAction.DontCare, RenderBufferStoreAction.Store, taaMaterial, (int)ptr.quality);
				if (flag)
				{
					int num2 = taaMaterial.shader.passCount - 1;
					Blitter.BlitCameraTexture(cmd, destination, rthandle, RenderBufferLoadAction.DontCare, RenderBufferStoreAction.Store, taaMaterial, num2);
					cameraData.taaPersistentData.SetLastAccumFrameIndex(multipassId, Time.frameCount);
				}
			}
		}

		// Token: 0x060006E0 RID: 1760 RVA: 0x00028F8C File Offset: 0x0002718C
		internal static void Render(RenderGraph renderGraph, Material taaMaterial, ref CameraData cameraData, ref TextureHandle srcColor, ref TextureHandle srcDepth, ref TextureHandle srcMotionVectors, ref TextureHandle dstColor)
		{
			int multipassId = cameraData.xr.multipassId;
			ref TemporalAA.Settings ptr = ref cameraData.taaSettings;
			bool flag = cameraData.taaPersistentData.GetLastAccumFrameIndex(multipassId) != Time.frameCount;
			float num = ((ptr.resetHistoryFrames == 0) ? ptr.m_FrameInfluence : 1f);
			RTHandle rthandle = cameraData.taaPersistentData.accumulationTexture(multipassId);
			TextureHandle textureHandle = renderGraph.ImportTexture(rthandle);
			TextureHandle textureHandle2 = (flag ? srcMotionVectors : renderGraph.defaultResources.blackTexture);
			TemporalAA.TaaPassData taaPassData;
			using (RenderGraphBuilder renderGraphBuilder = renderGraph.AddRenderPass<TemporalAA.TaaPassData>("Temporal Anti-aliasing", out taaPassData, ProfilingSampler.Get<URPProfileId>(URPProfileId.TemporalAA)))
			{
				taaPassData.dstTex = renderGraphBuilder.UseColorBuffer(in dstColor, 0);
				taaPassData.srcColorTex = renderGraphBuilder.ReadTexture(in srcColor);
				taaPassData.srcDepthTex = renderGraphBuilder.ReadTexture(in srcDepth);
				taaPassData.srcMotionVectorTex = renderGraphBuilder.ReadTexture(in textureHandle2);
				taaPassData.srcTaaAccumTex = renderGraphBuilder.ReadTexture(in textureHandle);
				taaPassData.material = taaMaterial;
				taaPassData.passIndex = (int)ptr.quality;
				taaPassData.taaFrameInfluence = num;
				taaPassData.taaVarianceClampScale = ptr.varianceClampScale;
				if (ptr.quality == TemporalAAQuality.VeryHigh)
				{
					taaPassData.taaFilterWeights = TemporalAA.CalculateFilterWeights(ptr.jitterScale);
				}
				else
				{
					taaPassData.taaFilterWeights = null;
				}
				GraphicsFormat graphicsFormat = rthandle.rt.graphicsFormat;
				if (graphicsFormat == GraphicsFormat.R8G8B8A8_UNorm || graphicsFormat == GraphicsFormat.B8G8R8A8_UNorm || graphicsFormat == GraphicsFormat.B10G11R11_UFloatPack32)
				{
					taaPassData.taaLowPrecisionSource = true;
				}
				else
				{
					taaPassData.taaLowPrecisionSource = false;
				}
				renderGraphBuilder.SetRenderFunc<TemporalAA.TaaPassData>(delegate(TemporalAA.TaaPassData data, RenderGraphContext context)
				{
					data.material.SetFloat(TemporalAA.ShaderConstants._TaaFrameInfluence, data.taaFrameInfluence);
					data.material.SetFloat(TemporalAA.ShaderConstants._TaaVarianceClampScale, data.taaVarianceClampScale);
					data.material.SetTexture(TemporalAA.ShaderConstants._TaaAccumulationTex, data.srcTaaAccumTex);
					data.material.SetTexture(TemporalAA.ShaderConstants._TaaMotionVectorTex, data.srcMotionVectorTex);
					data.material.SetTexture(TemporalAA.ShaderConstants._CameraDepthTexture, data.srcDepthTex);
					CoreUtils.SetKeyword(data.material, TemporalAA.ShaderKeywords.TAA_LOW_PRECISION_SOURCE, data.taaLowPrecisionSource);
					if (data.taaFilterWeights != null)
					{
						data.material.SetFloatArray(TemporalAA.ShaderConstants._TaaFilterWeights, data.taaFilterWeights);
					}
					Blitter.BlitTexture(context.cmd, data.srcColorTex, Vector2.one, data.material, data.passIndex);
				});
			}
			if (flag)
			{
				int num2 = taaMaterial.shader.passCount - 1;
				TemporalAA.TaaPassData taaPassData2;
				using (RenderGraphBuilder renderGraphBuilder2 = renderGraph.AddRenderPass<TemporalAA.TaaPassData>("Temporal Anti-aliasing Copy History", out taaPassData2, new ProfilingSampler("TemporalAAHistoryCopy")))
				{
					taaPassData2.dstTex = renderGraphBuilder2.UseColorBuffer(in textureHandle, 0);
					taaPassData2.srcColorTex = renderGraphBuilder2.ReadTexture(in dstColor);
					taaPassData2.material = taaMaterial;
					taaPassData2.passIndex = num2;
					renderGraphBuilder2.SetRenderFunc<TemporalAA.TaaPassData>(delegate(TemporalAA.TaaPassData data, RenderGraphContext context)
					{
						Blitter.BlitTexture(context.cmd, data.srcColorTex, Vector2.one, data.material, data.passIndex);
					});
				}
			}
		}

		// Token: 0x0400056C RID: 1388
		private static readonly Vector2[] taaFilterOffsets = new Vector2[]
		{
			new Vector2(0f, 0f),
			new Vector2(0f, 1f),
			new Vector2(1f, 0f),
			new Vector2(-1f, 0f),
			new Vector2(0f, -1f),
			new Vector2(-1f, 1f),
			new Vector2(1f, -1f),
			new Vector2(1f, 1f),
			new Vector2(-1f, -1f)
		};

		// Token: 0x0400056D RID: 1389
		private static readonly float[] taaFilterWeights = new float[TemporalAA.taaFilterOffsets.Length + 1];

		// Token: 0x020001B9 RID: 441
		internal static class ShaderConstants
		{
			// Token: 0x04000B83 RID: 2947
			public static readonly int _TaaAccumulationTex = Shader.PropertyToID("_TaaAccumulationTex");

			// Token: 0x04000B84 RID: 2948
			public static readonly int _TaaMotionVectorTex = Shader.PropertyToID("_TaaMotionVectorTex");

			// Token: 0x04000B85 RID: 2949
			public static readonly int _TaaFilterWeights = Shader.PropertyToID("_TaaFilterWeights");

			// Token: 0x04000B86 RID: 2950
			public static readonly int _TaaFrameInfluence = Shader.PropertyToID("_TaaFrameInfluence");

			// Token: 0x04000B87 RID: 2951
			public static readonly int _TaaVarianceClampScale = Shader.PropertyToID("_TaaVarianceClampScale");

			// Token: 0x04000B88 RID: 2952
			public static readonly int _CameraDepthTexture = Shader.PropertyToID("_CameraDepthTexture");
		}

		// Token: 0x020001BA RID: 442
		internal static class ShaderKeywords
		{
			// Token: 0x04000B89 RID: 2953
			public static readonly string TAA_LOW_PRECISION_SOURCE = "TAA_LOW_PRECISION_SOURCE";
		}

		// Token: 0x020001BB RID: 443
		[Serializable]
		public struct Settings
		{
			// Token: 0x17000259 RID: 601
			// (get) Token: 0x06000B24 RID: 2852 RVA: 0x0004B74B File Offset: 0x0004994B
			// (set) Token: 0x06000B25 RID: 2853 RVA: 0x0004B753 File Offset: 0x00049953
			public TemporalAAQuality quality
			{
				get
				{
					return this.m_Quality;
				}
				set
				{
					this.m_Quality = (TemporalAAQuality)Mathf.Clamp((int)value, 0, 4);
				}
			}

			// Token: 0x1700025A RID: 602
			// (get) Token: 0x06000B26 RID: 2854 RVA: 0x0004B763 File Offset: 0x00049963
			// (set) Token: 0x06000B27 RID: 2855 RVA: 0x0004B771 File Offset: 0x00049971
			public float baseBlendFactor
			{
				get
				{
					return 1f - this.m_FrameInfluence;
				}
				set
				{
					this.m_FrameInfluence = Mathf.Clamp01(1f - value);
				}
			}

			// Token: 0x1700025B RID: 603
			// (get) Token: 0x06000B28 RID: 2856 RVA: 0x0004B785 File Offset: 0x00049985
			// (set) Token: 0x06000B29 RID: 2857 RVA: 0x0004B78D File Offset: 0x0004998D
			public float jitterScale
			{
				get
				{
					return this.m_JitterScale;
				}
				set
				{
					this.m_JitterScale = Mathf.Clamp01(value);
				}
			}

			// Token: 0x1700025C RID: 604
			// (get) Token: 0x06000B2A RID: 2858 RVA: 0x0004B79B File Offset: 0x0004999B
			// (set) Token: 0x06000B2B RID: 2859 RVA: 0x0004B7A3 File Offset: 0x000499A3
			public float mipBias
			{
				get
				{
					return this.m_MipBias;
				}
				set
				{
					this.m_MipBias = Mathf.Clamp(value, -1f, 0f);
				}
			}

			// Token: 0x1700025D RID: 605
			// (get) Token: 0x06000B2C RID: 2860 RVA: 0x0004B7BB File Offset: 0x000499BB
			// (set) Token: 0x06000B2D RID: 2861 RVA: 0x0004B7C3 File Offset: 0x000499C3
			public float varianceClampScale
			{
				get
				{
					return this.m_VarianceClampScale;
				}
				set
				{
					this.m_VarianceClampScale = Mathf.Clamp(value, 0.001f, 10f);
				}
			}

			// Token: 0x1700025E RID: 606
			// (get) Token: 0x06000B2E RID: 2862 RVA: 0x0004B7DB File Offset: 0x000499DB
			// (set) Token: 0x06000B2F RID: 2863 RVA: 0x0004B7E3 File Offset: 0x000499E3
			public float contrastAdaptiveSharpening
			{
				get
				{
					return this.m_ContrastAdaptiveSharpening;
				}
				set
				{
					this.m_ContrastAdaptiveSharpening = Mathf.Clamp01(value);
				}
			}

			// Token: 0x06000B30 RID: 2864 RVA: 0x0004B7F4 File Offset: 0x000499F4
			public static TemporalAA.Settings Create()
			{
				TemporalAA.Settings settings;
				settings.m_Quality = TemporalAAQuality.High;
				settings.m_FrameInfluence = 0.1f;
				settings.m_JitterScale = 1f;
				settings.m_MipBias = 0f;
				settings.m_VarianceClampScale = 0.9f;
				settings.m_ContrastAdaptiveSharpening = 0f;
				settings.resetHistoryFrames = 0;
				settings.jitterFrameCountOffset = 0;
				return settings;
			}

			// Token: 0x04000B8A RID: 2954
			[SerializeField]
			[FormerlySerializedAs("quality")]
			internal TemporalAAQuality m_Quality;

			// Token: 0x04000B8B RID: 2955
			[SerializeField]
			[FormerlySerializedAs("frameInfluence")]
			internal float m_FrameInfluence;

			// Token: 0x04000B8C RID: 2956
			[SerializeField]
			[FormerlySerializedAs("jitterScale")]
			internal float m_JitterScale;

			// Token: 0x04000B8D RID: 2957
			[SerializeField]
			[FormerlySerializedAs("mipBias")]
			internal float m_MipBias;

			// Token: 0x04000B8E RID: 2958
			[SerializeField]
			[FormerlySerializedAs("varianceClampScale")]
			internal float m_VarianceClampScale;

			// Token: 0x04000B8F RID: 2959
			[SerializeField]
			[FormerlySerializedAs("contrastAdaptiveSharpening")]
			internal float m_ContrastAdaptiveSharpening;

			// Token: 0x04000B90 RID: 2960
			[NonSerialized]
			internal int resetHistoryFrames;

			// Token: 0x04000B91 RID: 2961
			[NonSerialized]
			internal int jitterFrameCountOffset;
		}

		// Token: 0x020001BC RID: 444
		private class TaaPassData
		{
			// Token: 0x04000B92 RID: 2962
			internal TextureHandle dstTex;

			// Token: 0x04000B93 RID: 2963
			internal TextureHandle srcColorTex;

			// Token: 0x04000B94 RID: 2964
			internal TextureHandle srcDepthTex;

			// Token: 0x04000B95 RID: 2965
			internal TextureHandle srcMotionVectorTex;

			// Token: 0x04000B96 RID: 2966
			internal TextureHandle srcTaaAccumTex;

			// Token: 0x04000B97 RID: 2967
			internal Material material;

			// Token: 0x04000B98 RID: 2968
			internal int passIndex;

			// Token: 0x04000B99 RID: 2969
			internal float taaFrameInfluence;

			// Token: 0x04000B9A RID: 2970
			internal float taaVarianceClampScale;

			// Token: 0x04000B9B RID: 2971
			internal float[] taaFilterWeights;

			// Token: 0x04000B9C RID: 2972
			internal bool taaLowPrecisionSource;
		}
	}
}
