using System;
using UnityEngine.Rendering;
using UnityEngine.XR;

namespace UnityEngine.Experimental.Rendering
{
	// Token: 0x02000008 RID: 8
	internal static class XRMirrorView
	{
		// Token: 0x06000027 RID: 39 RVA: 0x00002CAC File Offset: 0x00000EAC
		internal static void RenderMirrorView(CommandBuffer cmd, Camera camera, Material mat, XRDisplaySubsystem display)
		{
			if (Application.platform == RuntimePlatform.Android && !XRGraphicsAutomatedTests.running)
			{
				return;
			}
			if (display == null || !display.running || mat == null)
			{
				return;
			}
			int preferredMirrorBlitMode = display.GetPreferredMirrorBlitMode();
			XRDisplaySubsystem.XRMirrorViewBlitDesc xrmirrorViewBlitDesc;
			if (display.GetMirrorViewBlitDesc(null, out xrmirrorViewBlitDesc, preferredMirrorBlitMode))
			{
				using (new ProfilingScope(cmd, XRMirrorView.k_MirrorViewProfilingSampler))
				{
					cmd.SetRenderTarget((camera.targetTexture != null) ? camera.targetTexture : new RenderTargetIdentifier(BuiltinRenderTextureType.CameraTarget));
					if (xrmirrorViewBlitDesc.nativeBlitAvailable)
					{
						display.AddGraphicsThreadMirrorViewBlit(cmd, xrmirrorViewBlitDesc.nativeBlitInvalidStates, preferredMirrorBlitMode);
					}
					else
					{
						for (int i = 0; i < xrmirrorViewBlitDesc.blitParamsCount; i++)
						{
							XRDisplaySubsystem.XRBlitParams xrblitParams;
							xrmirrorViewBlitDesc.GetBlitParameter(i, out xrblitParams);
							Vector4 vector = new Vector4(xrblitParams.srcRect.width, xrblitParams.srcRect.height, xrblitParams.srcRect.x, xrblitParams.srcRect.y);
							Vector4 vector2 = new Vector4(xrblitParams.destRect.width, xrblitParams.destRect.height, xrblitParams.destRect.x, xrblitParams.destRect.y);
							if (camera.targetTexture != null || camera.cameraType == CameraType.SceneView || camera.cameraType == CameraType.Preview)
							{
								vector.y = -vector.y;
								vector.w += xrblitParams.srcRect.height;
							}
							HDROutputSettings main = HDROutputSettings.main;
							if (xrblitParams.srcHdrEncoded || main.active)
							{
								ColorGamut colorGamut = (main.active ? main.displayColorGamut : ColorGamut.sRGB);
								object obj = (xrblitParams.srcHdrEncoded ? xrblitParams.srcHdrColorGamut : ColorGamut.sRGB);
								ColorPrimaries colorPrimaries = ColorGamutUtility.GetColorPrimaries(colorGamut);
								object obj2 = obj;
								ColorPrimaries colorPrimaries2 = ColorGamutUtility.GetColorPrimaries(obj2);
								HDROutputUtils.ConfigureHDROutput(XRMirrorView.s_MirrorViewMaterialProperty, colorGamut);
								HDROutputUtils.ConfigureHDROutput(mat, HDROutputUtils.Operation.ColorConversion | HDROutputUtils.Operation.ColorEncoding);
								int num;
								HDROutputUtils.GetColorEncodingForGamut(obj2, out num);
								XRMirrorView.s_MirrorViewMaterialProperty.SetInteger(XRMirrorView.k_SourceHDREncoding, num);
								Matrix4x4 matrix4x = Matrix4x4.identity;
								matrix4x.m33 = 0f;
								if (colorPrimaries2 == ColorPrimaries.Rec709)
								{
									matrix4x = ColorSpaceUtils.Rec709ToRec2020Mat;
								}
								else if (colorPrimaries2 == ColorPrimaries.P3)
								{
									matrix4x = ColorSpaceUtils.P3D65ToRec2020Mat;
								}
								Matrix4x4 matrix4x2 = Matrix4x4.identity;
								matrix4x2.m33 = 0f;
								if (colorPrimaries == ColorPrimaries.Rec709)
								{
									matrix4x2 = ColorSpaceUtils.Rec2020ToRec709Mat;
								}
								else if (colorPrimaries == ColorPrimaries.P3)
								{
									matrix4x2 = ColorSpaceUtils.Rec2020ToP3D65Mat;
								}
								Matrix4x4 matrix4x3 = matrix4x * matrix4x2;
								XRMirrorView.s_MirrorViewMaterialProperty.SetMatrix(XRMirrorView.k_ColorTransform, matrix4x3);
								XRMirrorView.s_MirrorViewMaterialProperty.SetFloat(XRMirrorView.k_MaxNits, main.active ? ((float)main.maxToneMapLuminance) : 160f);
								XRMirrorView.s_MirrorViewMaterialProperty.SetFloat(XRMirrorView.k_SourceMaxNits, xrblitParams.srcHdrEncoded ? ((float)xrblitParams.srcHdrMaxLuminance) : 160f);
							}
							bool flag = !xrblitParams.srcTex.sRGB && (xrblitParams.srcTex.graphicsFormat == GraphicsFormat.R8G8B8A8_UNorm || xrblitParams.srcTex.graphicsFormat == GraphicsFormat.B8G8R8A8_UNorm);
							XRMirrorView.s_MirrorViewMaterialProperty.SetFloat(XRMirrorView.k_SRGBRead, flag ? 1f : 0f);
							XRMirrorView.s_MirrorViewMaterialProperty.SetFloat(XRMirrorView.k_SRGBWrite, (QualitySettings.activeColorSpace == ColorSpace.Linear) ? 0f : 1f);
							XRMirrorView.s_MirrorViewMaterialProperty.SetTexture(XRMirrorView.k_SourceTex, xrblitParams.srcTex);
							XRMirrorView.s_MirrorViewMaterialProperty.SetVector(XRMirrorView.k_ScaleBias, vector);
							XRMirrorView.s_MirrorViewMaterialProperty.SetVector(XRMirrorView.k_ScaleBiasRt, vector2);
							XRMirrorView.s_MirrorViewMaterialProperty.SetFloat(XRMirrorView.k_SourceTexArraySlice, (float)xrblitParams.srcTexArraySlice);
							if (XRSystem.foveatedRenderingCaps.HasFlag(FoveatedRenderingCaps.NonUniformRaster) && xrblitParams.foveatedRenderingInfo != IntPtr.Zero)
							{
								cmd.ConfigureFoveatedRendering(xrblitParams.foveatedRenderingInfo);
								cmd.EnableShaderKeyword("_FOVEATED_RENDERING_NON_UNIFORM_RASTER");
							}
							int num2 = ((xrblitParams.srcTex.dimension == TextureDimension.Tex2DArray) ? 1 : 0);
							cmd.DrawProcedural(Matrix4x4.identity, mat, num2, MeshTopology.Quads, 4, 1, XRMirrorView.s_MirrorViewMaterialProperty);
						}
					}
				}
			}
			if (XRSystem.foveatedRenderingCaps.HasFlag(FoveatedRenderingCaps.NonUniformRaster))
			{
				cmd.DisableShaderKeyword("_FOVEATED_RENDERING_NON_UNIFORM_RASTER");
				cmd.ConfigureFoveatedRendering(IntPtr.Zero);
			}
		}

		// Token: 0x0400001E RID: 30
		private static readonly MaterialPropertyBlock s_MirrorViewMaterialProperty = new MaterialPropertyBlock();

		// Token: 0x0400001F RID: 31
		private static readonly ProfilingSampler k_MirrorViewProfilingSampler = new ProfilingSampler("XR Mirror View");

		// Token: 0x04000020 RID: 32
		private static readonly int k_SourceTex = Shader.PropertyToID("_SourceTex");

		// Token: 0x04000021 RID: 33
		private static readonly int k_SourceTexArraySlice = Shader.PropertyToID("_SourceTexArraySlice");

		// Token: 0x04000022 RID: 34
		private static readonly int k_ScaleBias = Shader.PropertyToID("_ScaleBias");

		// Token: 0x04000023 RID: 35
		private static readonly int k_ScaleBiasRt = Shader.PropertyToID("_ScaleBiasRt");

		// Token: 0x04000024 RID: 36
		private static readonly int k_SRGBRead = Shader.PropertyToID("_SRGBRead");

		// Token: 0x04000025 RID: 37
		private static readonly int k_SRGBWrite = Shader.PropertyToID("_SRGBWrite");

		// Token: 0x04000026 RID: 38
		private static readonly int k_MaxNits = Shader.PropertyToID("_MaxNits");

		// Token: 0x04000027 RID: 39
		private static readonly int k_SourceMaxNits = Shader.PropertyToID("_SourceMaxNits");

		// Token: 0x04000028 RID: 40
		private static readonly int k_SourceHDREncoding = Shader.PropertyToID("_SourceHDREncoding");

		// Token: 0x04000029 RID: 41
		private static readonly int k_ColorTransform = Shader.PropertyToID("_ColorTransform");
	}
}
