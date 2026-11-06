using System;
using System.Runtime.CompilerServices;
using UnityEngine.Experimental.Rendering;
using UnityEngine.Experimental.Rendering.RenderGraphModule;

namespace UnityEngine.Rendering.Universal.Internal
{
	// Token: 0x0200011C RID: 284
	public class ColorGradingLutPass : ScriptableRenderPass
	{
		// Token: 0x0600094F RID: 2383 RVA: 0x0003DD70 File Offset: 0x0003BF70
		public ColorGradingLutPass(RenderPassEvent evt, PostProcessData data)
		{
			base.profilingSampler = new ProfilingSampler("ColorGradingLutPass");
			base.renderPassEvent = evt;
			base.overrideCameraTarget = true;
			this.m_LutBuilderLdr = ColorGradingLutPass.<.ctor>g__Load|7_0(data.shaders.lutBuilderLdrPS);
			this.m_LutBuilderHdr = ColorGradingLutPass.<.ctor>g__Load|7_0(data.shaders.lutBuilderHdrPS);
			if (SystemInfo.IsFormatSupported(GraphicsFormat.R16G16B16A16_SFloat, FormatUsage.Blend))
			{
				this.m_HdrLutFormat = GraphicsFormat.R16G16B16A16_SFloat;
			}
			else if (SystemInfo.IsFormatSupported(GraphicsFormat.B10G11R11_UFloatPack32, FormatUsage.Blend))
			{
				this.m_HdrLutFormat = GraphicsFormat.B10G11R11_UFloatPack32;
			}
			else
			{
				this.m_HdrLutFormat = GraphicsFormat.R8G8B8A8_UNorm;
			}
			this.m_LdrLutFormat = GraphicsFormat.R8G8B8A8_UNorm;
			base.useNativeRenderPass = false;
			if (SystemInfo.graphicsDeviceType == GraphicsDeviceType.OpenGLES3 && Graphics.minOpenGLESVersion <= OpenGLESVersion.OpenGLES30 && SystemInfo.graphicsDeviceName.StartsWith("Adreno (TM) 3"))
			{
				this.m_AllowColorGradingACESHDR = false;
			}
			this.m_PassData = new ColorGradingLutPass.PassData();
		}

		// Token: 0x06000950 RID: 2384 RVA: 0x0003DE45 File Offset: 0x0003C045
		public void Setup(in RTHandle internalLut)
		{
			this.m_InternalLut = internalLut;
		}

		// Token: 0x06000951 RID: 2385 RVA: 0x0003DE50 File Offset: 0x0003C050
		public void ConfigureDescriptor(in PostProcessingData postProcessingData, out RenderTextureDescriptor descriptor, out FilterMode filterMode)
		{
			bool flag = postProcessingData.gradingMode == ColorGradingMode.HighDynamicRange;
			int lutSize = postProcessingData.lutSize;
			int num = lutSize * lutSize;
			GraphicsFormat graphicsFormat = (flag ? this.m_HdrLutFormat : this.m_LdrLutFormat);
			descriptor = new RenderTextureDescriptor(num, lutSize, graphicsFormat, 0);
			descriptor.vrUsage = VRTextureUsage.None;
			filterMode = FilterMode.Bilinear;
		}

		// Token: 0x06000952 RID: 2386 RVA: 0x0003DE9C File Offset: 0x0003C09C
		public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
		{
			this.m_PassData.lutBuilderLdr = this.m_LutBuilderLdr;
			this.m_PassData.lutBuilderHdr = this.m_LutBuilderHdr;
			this.m_PassData.allowColorGradingACESHDR = this.m_AllowColorGradingACESHDR;
			if (renderingData.cameraData.xr.supportsFoveatedRendering)
			{
				renderingData.commandBuffer.SetFoveatedRenderingMode(FoveatedRenderingMode.Disabled);
			}
			CoreUtils.SetRenderTarget(renderingData.commandBuffer, this.m_InternalLut, RenderBufferLoadAction.DontCare, RenderBufferStoreAction.Store, ClearFlag.None, Color.clear, 0, CubemapFace.Unknown, -1);
			ColorGradingLutPass.ExecutePass(context, this.m_PassData, ref renderingData, this.m_InternalLut);
		}

		// Token: 0x06000953 RID: 2387 RVA: 0x0003DF2C File Offset: 0x0003C12C
		private static void ExecutePass(ScriptableRenderContext context, ColorGradingLutPass.PassData passData, ref RenderingData renderingData, RTHandle internalLutTarget)
		{
			CommandBuffer commandBuffer = renderingData.commandBuffer;
			Material lutBuilderLdr = passData.lutBuilderLdr;
			Material lutBuilderHdr = passData.lutBuilderHdr;
			bool allowColorGradingACESHDR = passData.allowColorGradingACESHDR;
			using (new ProfilingScope(commandBuffer, ProfilingSampler.Get<URPProfileId>(URPProfileId.ColorGradingLUT)))
			{
				VolumeStack stack = VolumeManager.instance.stack;
				ChannelMixer component = stack.GetComponent<ChannelMixer>();
				ColorAdjustments component2 = stack.GetComponent<ColorAdjustments>();
				ColorCurves component3 = stack.GetComponent<ColorCurves>();
				LiftGammaGain component4 = stack.GetComponent<LiftGammaGain>();
				ShadowsMidtonesHighlights component5 = stack.GetComponent<ShadowsMidtonesHighlights>();
				SplitToning component6 = stack.GetComponent<SplitToning>();
				Tonemapping component7 = stack.GetComponent<Tonemapping>();
				WhiteBalance component8 = stack.GetComponent<WhiteBalance>();
				bool flag = renderingData.postProcessingData.gradingMode == ColorGradingMode.HighDynamicRange;
				ref CameraData ptr = ref renderingData.cameraData;
				Material material = (flag ? lutBuilderHdr : lutBuilderLdr);
				Vector3 vector = ColorUtils.ColorBalanceToLMSCoeffs(component8.temperature.value, component8.tint.value);
				Vector4 vector2 = new Vector4(component2.hueShift.value / 360f, component2.saturation.value / 100f + 1f, component2.contrast.value / 100f + 1f, 0f);
				Vector4 vector3 = new Vector4(component.redOutRedIn.value / 100f, component.redOutGreenIn.value / 100f, component.redOutBlueIn.value / 100f, 0f);
				Vector4 vector4 = new Vector4(component.greenOutRedIn.value / 100f, component.greenOutGreenIn.value / 100f, component.greenOutBlueIn.value / 100f, 0f);
				Vector4 vector5 = new Vector4(component.blueOutRedIn.value / 100f, component.blueOutGreenIn.value / 100f, component.blueOutBlueIn.value / 100f, 0f);
				Vector4 vector6 = new Vector4(component5.shadowsStart.value, component5.shadowsEnd.value, component5.highlightsStart.value, component5.highlightsEnd.value);
				Vector4 vector7 = component5.shadows.value;
				Vector4 vector8 = component5.midtones.value;
				Vector4 vector9 = component5.highlights.value;
				ValueTuple<Vector4, Vector4, Vector4> valueTuple = ColorUtils.PrepareShadowsMidtonesHighlights(in vector7, in vector8, in vector9);
				Vector4 item = valueTuple.Item1;
				Vector4 item2 = valueTuple.Item2;
				Vector4 item3 = valueTuple.Item3;
				vector7 = component4.lift.value;
				vector8 = component4.gamma.value;
				vector9 = component4.gain.value;
				ValueTuple<Vector4, Vector4, Vector4> valueTuple2 = ColorUtils.PrepareLiftGammaGain(in vector7, in vector8, in vector9);
				Vector4 item4 = valueTuple2.Item1;
				Vector4 item5 = valueTuple2.Item2;
				Vector4 item6 = valueTuple2.Item3;
				vector7 = component6.shadows.value;
				vector8 = component6.highlights.value;
				ValueTuple<Vector4, Vector4> valueTuple3 = ColorUtils.PrepareSplitToning(in vector7, in vector8, component6.balance.value);
				Vector4 item7 = valueTuple3.Item1;
				Vector4 item8 = valueTuple3.Item2;
				int lutSize = renderingData.postProcessingData.lutSize;
				int num = lutSize * lutSize;
				Vector4 vector10 = new Vector4((float)lutSize, 0.5f / (float)num, 0.5f / (float)lutSize, (float)lutSize / ((float)lutSize - 1f));
				material.SetVector(ColorGradingLutPass.ShaderConstants._Lut_Params, vector10);
				material.SetVector(ColorGradingLutPass.ShaderConstants._ColorBalance, vector);
				material.SetVector(ColorGradingLutPass.ShaderConstants._ColorFilter, component2.colorFilter.value.linear);
				material.SetVector(ColorGradingLutPass.ShaderConstants._ChannelMixerRed, vector3);
				material.SetVector(ColorGradingLutPass.ShaderConstants._ChannelMixerGreen, vector4);
				material.SetVector(ColorGradingLutPass.ShaderConstants._ChannelMixerBlue, vector5);
				material.SetVector(ColorGradingLutPass.ShaderConstants._HueSatCon, vector2);
				material.SetVector(ColorGradingLutPass.ShaderConstants._Lift, item4);
				material.SetVector(ColorGradingLutPass.ShaderConstants._Gamma, item5);
				material.SetVector(ColorGradingLutPass.ShaderConstants._Gain, item6);
				material.SetVector(ColorGradingLutPass.ShaderConstants._Shadows, item);
				material.SetVector(ColorGradingLutPass.ShaderConstants._Midtones, item2);
				material.SetVector(ColorGradingLutPass.ShaderConstants._Highlights, item3);
				material.SetVector(ColorGradingLutPass.ShaderConstants._ShaHiLimits, vector6);
				material.SetVector(ColorGradingLutPass.ShaderConstants._SplitShadows, item7);
				material.SetVector(ColorGradingLutPass.ShaderConstants._SplitHighlights, item8);
				material.SetTexture(ColorGradingLutPass.ShaderConstants._CurveMaster, component3.master.value.GetTexture());
				material.SetTexture(ColorGradingLutPass.ShaderConstants._CurveRed, component3.red.value.GetTexture());
				material.SetTexture(ColorGradingLutPass.ShaderConstants._CurveGreen, component3.green.value.GetTexture());
				material.SetTexture(ColorGradingLutPass.ShaderConstants._CurveBlue, component3.blue.value.GetTexture());
				material.SetTexture(ColorGradingLutPass.ShaderConstants._CurveHueVsHue, component3.hueVsHue.value.GetTexture());
				material.SetTexture(ColorGradingLutPass.ShaderConstants._CurveHueVsSat, component3.hueVsSat.value.GetTexture());
				material.SetTexture(ColorGradingLutPass.ShaderConstants._CurveLumVsSat, component3.lumVsSat.value.GetTexture());
				material.SetTexture(ColorGradingLutPass.ShaderConstants._CurveSatVsSat, component3.satVsSat.value.GetTexture());
				if (flag)
				{
					material.shaderKeywords = null;
					TonemappingMode value = component7.mode.value;
					if (value != TonemappingMode.Neutral)
					{
						if (value == TonemappingMode.ACES)
						{
							material.EnableKeyword(allowColorGradingACESHDR ? "_TONEMAP_ACES" : "_TONEMAP_NEUTRAL");
						}
					}
					else
					{
						material.EnableKeyword("_TONEMAP_NEUTRAL");
					}
					if (ptr.isHDROutputActive)
					{
						Vector4 vector11;
						UniversalRenderPipeline.GetHDROutputLuminanceParameters(ptr.hdrDisplayInformation, ptr.hdrDisplayColorGamut, component7, out vector11);
						Vector4 vector12;
						UniversalRenderPipeline.GetHDROutputGradingParameters(component7, out vector12);
						material.SetVector(ShaderPropertyId.hdrOutputLuminanceParams, vector11);
						material.SetVector(ShaderPropertyId.hdrOutputGradingParams, vector12);
						HDROutputUtils.ConfigureHDROutput(material, ptr.hdrDisplayColorGamut, HDROutputUtils.Operation.ColorConversion);
					}
				}
				ptr.xr.StopSinglePass(commandBuffer);
				if (ptr.xr.supportsFoveatedRendering)
				{
					commandBuffer.SetFoveatedRenderingMode(FoveatedRenderingMode.Disabled);
				}
				Blitter.BlitCameraTexture(commandBuffer, internalLutTarget, internalLutTarget, RenderBufferLoadAction.DontCare, RenderBufferStoreAction.Store, material, 0);
				ptr.xr.StartSinglePass(commandBuffer);
			}
		}

		// Token: 0x06000954 RID: 2388 RVA: 0x0003E540 File Offset: 0x0003C740
		internal void Render(RenderGraph renderGraph, out TextureHandle internalColorLut, ref RenderingData renderingData)
		{
			ColorGradingLutPass.PassData passData;
			using (RenderGraphBuilder renderGraphBuilder = renderGraph.AddRenderPass<ColorGradingLutPass.PassData>("Color Lut Pass", out passData, base.profilingSampler))
			{
				RenderTextureDescriptor renderTextureDescriptor;
				FilterMode filterMode;
				this.ConfigureDescriptor(in renderingData.postProcessingData, out renderTextureDescriptor, out filterMode);
				internalColorLut = UniversalRenderer.CreateRenderGraphTexture(renderGraph, renderTextureDescriptor, "_InternalGradingLut", true, FilterMode.Point, TextureWrapMode.Clamp);
				passData.internalLut = renderGraphBuilder.UseColorBuffer(in internalColorLut, 0);
				passData.lutBuilderLdr = this.m_LutBuilderLdr;
				passData.lutBuilderHdr = this.m_LutBuilderHdr;
				passData.renderingData = renderingData;
				passData.allowColorGradingACESHDR = this.m_AllowColorGradingACESHDR;
				renderGraphBuilder.AllowPassCulling(false);
				renderGraphBuilder.SetRenderFunc<ColorGradingLutPass.PassData>(delegate(ColorGradingLutPass.PassData data, RenderGraphContext context)
				{
					ColorGradingLutPass.ExecutePass(context.renderContext, data, ref data.renderingData, data.internalLut);
				});
			}
		}

		// Token: 0x06000955 RID: 2389 RVA: 0x0003E614 File Offset: 0x0003C814
		public void Cleanup()
		{
			CoreUtils.Destroy(this.m_LutBuilderLdr);
			CoreUtils.Destroy(this.m_LutBuilderHdr);
		}

		// Token: 0x06000956 RID: 2390 RVA: 0x0003E62C File Offset: 0x0003C82C
		[CompilerGenerated]
		internal static Material <.ctor>g__Load|7_0(Shader shader)
		{
			if (shader == null)
			{
				Debug.LogError("Missing shader. ColorGradingLutPass render pass will not execute. Check for missing reference in the renderer resources.");
				return null;
			}
			return CoreUtils.CreateEngineMaterial(shader);
		}

		// Token: 0x0400086E RID: 2158
		private readonly Material m_LutBuilderLdr;

		// Token: 0x0400086F RID: 2159
		private readonly Material m_LutBuilderHdr;

		// Token: 0x04000870 RID: 2160
		internal readonly GraphicsFormat m_HdrLutFormat;

		// Token: 0x04000871 RID: 2161
		internal readonly GraphicsFormat m_LdrLutFormat;

		// Token: 0x04000872 RID: 2162
		private ColorGradingLutPass.PassData m_PassData;

		// Token: 0x04000873 RID: 2163
		private RTHandle m_InternalLut;

		// Token: 0x04000874 RID: 2164
		private bool m_AllowColorGradingACESHDR = true;

		// Token: 0x020001DF RID: 479
		private class PassData
		{
			// Token: 0x04000C65 RID: 3173
			internal RenderingData renderingData;

			// Token: 0x04000C66 RID: 3174
			internal Material lutBuilderLdr;

			// Token: 0x04000C67 RID: 3175
			internal Material lutBuilderHdr;

			// Token: 0x04000C68 RID: 3176
			internal bool allowColorGradingACESHDR;

			// Token: 0x04000C69 RID: 3177
			internal TextureHandle internalLut;
		}

		// Token: 0x020001E0 RID: 480
		private static class ShaderConstants
		{
			// Token: 0x04000C6A RID: 3178
			public static readonly int _Lut_Params = Shader.PropertyToID("_Lut_Params");

			// Token: 0x04000C6B RID: 3179
			public static readonly int _ColorBalance = Shader.PropertyToID("_ColorBalance");

			// Token: 0x04000C6C RID: 3180
			public static readonly int _ColorFilter = Shader.PropertyToID("_ColorFilter");

			// Token: 0x04000C6D RID: 3181
			public static readonly int _ChannelMixerRed = Shader.PropertyToID("_ChannelMixerRed");

			// Token: 0x04000C6E RID: 3182
			public static readonly int _ChannelMixerGreen = Shader.PropertyToID("_ChannelMixerGreen");

			// Token: 0x04000C6F RID: 3183
			public static readonly int _ChannelMixerBlue = Shader.PropertyToID("_ChannelMixerBlue");

			// Token: 0x04000C70 RID: 3184
			public static readonly int _HueSatCon = Shader.PropertyToID("_HueSatCon");

			// Token: 0x04000C71 RID: 3185
			public static readonly int _Lift = Shader.PropertyToID("_Lift");

			// Token: 0x04000C72 RID: 3186
			public static readonly int _Gamma = Shader.PropertyToID("_Gamma");

			// Token: 0x04000C73 RID: 3187
			public static readonly int _Gain = Shader.PropertyToID("_Gain");

			// Token: 0x04000C74 RID: 3188
			public static readonly int _Shadows = Shader.PropertyToID("_Shadows");

			// Token: 0x04000C75 RID: 3189
			public static readonly int _Midtones = Shader.PropertyToID("_Midtones");

			// Token: 0x04000C76 RID: 3190
			public static readonly int _Highlights = Shader.PropertyToID("_Highlights");

			// Token: 0x04000C77 RID: 3191
			public static readonly int _ShaHiLimits = Shader.PropertyToID("_ShaHiLimits");

			// Token: 0x04000C78 RID: 3192
			public static readonly int _SplitShadows = Shader.PropertyToID("_SplitShadows");

			// Token: 0x04000C79 RID: 3193
			public static readonly int _SplitHighlights = Shader.PropertyToID("_SplitHighlights");

			// Token: 0x04000C7A RID: 3194
			public static readonly int _CurveMaster = Shader.PropertyToID("_CurveMaster");

			// Token: 0x04000C7B RID: 3195
			public static readonly int _CurveRed = Shader.PropertyToID("_CurveRed");

			// Token: 0x04000C7C RID: 3196
			public static readonly int _CurveGreen = Shader.PropertyToID("_CurveGreen");

			// Token: 0x04000C7D RID: 3197
			public static readonly int _CurveBlue = Shader.PropertyToID("_CurveBlue");

			// Token: 0x04000C7E RID: 3198
			public static readonly int _CurveHueVsHue = Shader.PropertyToID("_CurveHueVsHue");

			// Token: 0x04000C7F RID: 3199
			public static readonly int _CurveHueVsSat = Shader.PropertyToID("_CurveHueVsSat");

			// Token: 0x04000C80 RID: 3200
			public static readonly int _CurveLumVsSat = Shader.PropertyToID("_CurveLumVsSat");

			// Token: 0x04000C81 RID: 3201
			public static readonly int _CurveSatVsSat = Shader.PropertyToID("_CurveSatVsSat");
		}
	}
}
