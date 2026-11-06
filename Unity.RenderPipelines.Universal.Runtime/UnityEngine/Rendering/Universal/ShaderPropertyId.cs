using System;

namespace UnityEngine.Rendering.Universal
{
	// Token: 0x02000100 RID: 256
	internal static class ShaderPropertyId
	{
		// Token: 0x040006DB RID: 1755
		public static readonly int glossyEnvironmentColor = Shader.PropertyToID("_GlossyEnvironmentColor");

		// Token: 0x040006DC RID: 1756
		public static readonly int subtractiveShadowColor = Shader.PropertyToID("_SubtractiveShadowColor");

		// Token: 0x040006DD RID: 1757
		public static readonly int glossyEnvironmentCubeMap = Shader.PropertyToID("_GlossyEnvironmentCubeMap");

		// Token: 0x040006DE RID: 1758
		public static readonly int glossyEnvironmentCubeMapHDR = Shader.PropertyToID("_GlossyEnvironmentCubeMap_HDR");

		// Token: 0x040006DF RID: 1759
		public static readonly int ambientSkyColor = Shader.PropertyToID("unity_AmbientSky");

		// Token: 0x040006E0 RID: 1760
		public static readonly int ambientEquatorColor = Shader.PropertyToID("unity_AmbientEquator");

		// Token: 0x040006E1 RID: 1761
		public static readonly int ambientGroundColor = Shader.PropertyToID("unity_AmbientGround");

		// Token: 0x040006E2 RID: 1762
		public static readonly int time = Shader.PropertyToID("_Time");

		// Token: 0x040006E3 RID: 1763
		public static readonly int sinTime = Shader.PropertyToID("_SinTime");

		// Token: 0x040006E4 RID: 1764
		public static readonly int cosTime = Shader.PropertyToID("_CosTime");

		// Token: 0x040006E5 RID: 1765
		public static readonly int deltaTime = Shader.PropertyToID("unity_DeltaTime");

		// Token: 0x040006E6 RID: 1766
		public static readonly int timeParameters = Shader.PropertyToID("_TimeParameters");

		// Token: 0x040006E7 RID: 1767
		public static readonly int scaledScreenParams = Shader.PropertyToID("_ScaledScreenParams");

		// Token: 0x040006E8 RID: 1768
		public static readonly int worldSpaceCameraPos = Shader.PropertyToID("_WorldSpaceCameraPos");

		// Token: 0x040006E9 RID: 1769
		public static readonly int screenParams = Shader.PropertyToID("_ScreenParams");

		// Token: 0x040006EA RID: 1770
		public static readonly int alphaToMaskAvailable = Shader.PropertyToID("_AlphaToMaskAvailable");

		// Token: 0x040006EB RID: 1771
		public static readonly int projectionParams = Shader.PropertyToID("_ProjectionParams");

		// Token: 0x040006EC RID: 1772
		public static readonly int zBufferParams = Shader.PropertyToID("_ZBufferParams");

		// Token: 0x040006ED RID: 1773
		public static readonly int orthoParams = Shader.PropertyToID("unity_OrthoParams");

		// Token: 0x040006EE RID: 1774
		public static readonly int globalMipBias = Shader.PropertyToID("_GlobalMipBias");

		// Token: 0x040006EF RID: 1775
		public static readonly int screenSize = Shader.PropertyToID("_ScreenSize");

		// Token: 0x040006F0 RID: 1776
		public static readonly int screenCoordScaleBias = Shader.PropertyToID("_ScreenCoordScaleBias");

		// Token: 0x040006F1 RID: 1777
		public static readonly int screenSizeOverride = Shader.PropertyToID("_ScreenSizeOverride");

		// Token: 0x040006F2 RID: 1778
		public static readonly int viewMatrix = Shader.PropertyToID("unity_MatrixV");

		// Token: 0x040006F3 RID: 1779
		public static readonly int projectionMatrix = Shader.PropertyToID("glstate_matrix_projection");

		// Token: 0x040006F4 RID: 1780
		public static readonly int viewAndProjectionMatrix = Shader.PropertyToID("unity_MatrixVP");

		// Token: 0x040006F5 RID: 1781
		public static readonly int inverseViewMatrix = Shader.PropertyToID("unity_MatrixInvV");

		// Token: 0x040006F6 RID: 1782
		public static readonly int inverseProjectionMatrix = Shader.PropertyToID("unity_MatrixInvP");

		// Token: 0x040006F7 RID: 1783
		public static readonly int inverseViewAndProjectionMatrix = Shader.PropertyToID("unity_MatrixInvVP");

		// Token: 0x040006F8 RID: 1784
		public static readonly int cameraProjectionMatrix = Shader.PropertyToID("unity_CameraProjection");

		// Token: 0x040006F9 RID: 1785
		public static readonly int inverseCameraProjectionMatrix = Shader.PropertyToID("unity_CameraInvProjection");

		// Token: 0x040006FA RID: 1786
		public static readonly int worldToCameraMatrix = Shader.PropertyToID("unity_WorldToCamera");

		// Token: 0x040006FB RID: 1787
		public static readonly int cameraToWorldMatrix = Shader.PropertyToID("unity_CameraToWorld");

		// Token: 0x040006FC RID: 1788
		public static readonly int shadowBias = Shader.PropertyToID("_ShadowBias");

		// Token: 0x040006FD RID: 1789
		public static readonly int lightDirection = Shader.PropertyToID("_LightDirection");

		// Token: 0x040006FE RID: 1790
		public static readonly int lightPosition = Shader.PropertyToID("_LightPosition");

		// Token: 0x040006FF RID: 1791
		public static readonly int cameraWorldClipPlanes = Shader.PropertyToID("unity_CameraWorldClipPlanes");

		// Token: 0x04000700 RID: 1792
		public static readonly int billboardNormal = Shader.PropertyToID("unity_BillboardNormal");

		// Token: 0x04000701 RID: 1793
		public static readonly int billboardTangent = Shader.PropertyToID("unity_BillboardTangent");

		// Token: 0x04000702 RID: 1794
		public static readonly int billboardCameraParams = Shader.PropertyToID("unity_BillboardCameraParams");

		// Token: 0x04000703 RID: 1795
		public static readonly int blitTexture = Shader.PropertyToID("_BlitTexture");

		// Token: 0x04000704 RID: 1796
		public static readonly int blitScaleBias = Shader.PropertyToID("_BlitScaleBias");

		// Token: 0x04000705 RID: 1797
		public static readonly int sourceTex = Shader.PropertyToID("_SourceTex");

		// Token: 0x04000706 RID: 1798
		public static readonly int scaleBias = Shader.PropertyToID("_ScaleBias");

		// Token: 0x04000707 RID: 1799
		public static readonly int scaleBiasRt = Shader.PropertyToID("_ScaleBiasRt");

		// Token: 0x04000708 RID: 1800
		public static readonly int rendererColor = Shader.PropertyToID("_RendererColor");

		// Token: 0x04000709 RID: 1801
		public static readonly int ditheringTexture = Shader.PropertyToID("_DitheringTexture");

		// Token: 0x0400070A RID: 1802
		public static readonly int ditheringTextureInvSize = Shader.PropertyToID("_DitheringTextureInvSize");

		// Token: 0x0400070B RID: 1803
		public static readonly int renderingLayerMaxInt = Shader.PropertyToID("_RenderingLayerMaxInt");

		// Token: 0x0400070C RID: 1804
		public static readonly int renderingLayerRcpMaxInt = Shader.PropertyToID("_RenderingLayerRcpMaxInt");

		// Token: 0x0400070D RID: 1805
		public static readonly int overlayUITexture = Shader.PropertyToID("_OverlayUITexture");

		// Token: 0x0400070E RID: 1806
		public static readonly int hdrOutputLuminanceParams = Shader.PropertyToID("_HDROutputLuminanceParams");

		// Token: 0x0400070F RID: 1807
		public static readonly int hdrOutputGradingParams = Shader.PropertyToID("_HDROutputGradingParams");
	}
}
