using System;

namespace UnityEngine.Rendering.Universal
{
	// Token: 0x02000102 RID: 258
	public static class ShaderKeywordStrings
	{
		// Token: 0x04000714 RID: 1812
		public const string MainLightShadows = "_MAIN_LIGHT_SHADOWS";

		// Token: 0x04000715 RID: 1813
		public const string MainLightShadowCascades = "_MAIN_LIGHT_SHADOWS_CASCADE";

		// Token: 0x04000716 RID: 1814
		public const string MainLightShadowScreen = "_MAIN_LIGHT_SHADOWS_SCREEN";

		// Token: 0x04000717 RID: 1815
		public const string CastingPunctualLightShadow = "_CASTING_PUNCTUAL_LIGHT_SHADOW";

		// Token: 0x04000718 RID: 1816
		public const string AdditionalLightsVertex = "_ADDITIONAL_LIGHTS_VERTEX";

		// Token: 0x04000719 RID: 1817
		public const string AdditionalLightsPixel = "_ADDITIONAL_LIGHTS";

		// Token: 0x0400071A RID: 1818
		internal const string ForwardPlus = "_FORWARD_PLUS";

		// Token: 0x0400071B RID: 1819
		public const string AdditionalLightShadows = "_ADDITIONAL_LIGHT_SHADOWS";

		// Token: 0x0400071C RID: 1820
		public const string ReflectionProbeBoxProjection = "_REFLECTION_PROBE_BOX_PROJECTION";

		// Token: 0x0400071D RID: 1821
		public const string ReflectionProbeBlending = "_REFLECTION_PROBE_BLENDING";

		// Token: 0x0400071E RID: 1822
		public const string SoftShadows = "_SHADOWS_SOFT";

		// Token: 0x0400071F RID: 1823
		public const string SoftShadowsLow = "_SHADOWS_SOFT_LOW";

		// Token: 0x04000720 RID: 1824
		public const string SoftShadowsMedium = "_SHADOWS_SOFT_MEDIUM";

		// Token: 0x04000721 RID: 1825
		public const string SoftShadowsHigh = "_SHADOWS_SOFT_HIGH";

		// Token: 0x04000722 RID: 1826
		public const string MixedLightingSubtractive = "_MIXED_LIGHTING_SUBTRACTIVE";

		// Token: 0x04000723 RID: 1827
		public const string LightmapShadowMixing = "LIGHTMAP_SHADOW_MIXING";

		// Token: 0x04000724 RID: 1828
		public const string ShadowsShadowMask = "SHADOWS_SHADOWMASK";

		// Token: 0x04000725 RID: 1829
		public const string LightLayers = "_LIGHT_LAYERS";

		// Token: 0x04000726 RID: 1830
		public const string RenderPassEnabled = "_RENDER_PASS_ENABLED";

		// Token: 0x04000727 RID: 1831
		public const string BillboardFaceCameraPos = "BILLBOARD_FACE_CAMERA_POS";

		// Token: 0x04000728 RID: 1832
		public const string LightCookies = "_LIGHT_COOKIES";

		// Token: 0x04000729 RID: 1833
		public const string DepthNoMsaa = "_DEPTH_NO_MSAA";

		// Token: 0x0400072A RID: 1834
		public const string DepthMsaa2 = "_DEPTH_MSAA_2";

		// Token: 0x0400072B RID: 1835
		public const string DepthMsaa4 = "_DEPTH_MSAA_4";

		// Token: 0x0400072C RID: 1836
		public const string DepthMsaa8 = "_DEPTH_MSAA_8";

		// Token: 0x0400072D RID: 1837
		public const string LinearToSRGBConversion = "_LINEAR_TO_SRGB_CONVERSION";

		// Token: 0x0400072E RID: 1838
		internal const string UseFastSRGBLinearConversion = "_USE_FAST_SRGB_LINEAR_CONVERSION";

		// Token: 0x0400072F RID: 1839
		public const string DBufferMRT1 = "_DBUFFER_MRT1";

		// Token: 0x04000730 RID: 1840
		public const string DBufferMRT2 = "_DBUFFER_MRT2";

		// Token: 0x04000731 RID: 1841
		public const string DBufferMRT3 = "_DBUFFER_MRT3";

		// Token: 0x04000732 RID: 1842
		public const string DecalNormalBlendLow = "_DECAL_NORMAL_BLEND_LOW";

		// Token: 0x04000733 RID: 1843
		public const string DecalNormalBlendMedium = "_DECAL_NORMAL_BLEND_MEDIUM";

		// Token: 0x04000734 RID: 1844
		public const string DecalNormalBlendHigh = "_DECAL_NORMAL_BLEND_HIGH";

		// Token: 0x04000735 RID: 1845
		public const string DecalLayers = "_DECAL_LAYERS";

		// Token: 0x04000736 RID: 1846
		public const string WriteRenderingLayers = "_WRITE_RENDERING_LAYERS";

		// Token: 0x04000737 RID: 1847
		public const string SmaaLow = "_SMAA_PRESET_LOW";

		// Token: 0x04000738 RID: 1848
		public const string SmaaMedium = "_SMAA_PRESET_MEDIUM";

		// Token: 0x04000739 RID: 1849
		public const string SmaaHigh = "_SMAA_PRESET_HIGH";

		// Token: 0x0400073A RID: 1850
		public const string PaniniGeneric = "_GENERIC";

		// Token: 0x0400073B RID: 1851
		public const string PaniniUnitDistance = "_UNIT_DISTANCE";

		// Token: 0x0400073C RID: 1852
		public const string BloomLQ = "_BLOOM_LQ";

		// Token: 0x0400073D RID: 1853
		public const string BloomHQ = "_BLOOM_HQ";

		// Token: 0x0400073E RID: 1854
		public const string BloomLQDirt = "_BLOOM_LQ_DIRT";

		// Token: 0x0400073F RID: 1855
		public const string BloomHQDirt = "_BLOOM_HQ_DIRT";

		// Token: 0x04000740 RID: 1856
		public const string UseRGBM = "_USE_RGBM";

		// Token: 0x04000741 RID: 1857
		public const string Distortion = "_DISTORTION";

		// Token: 0x04000742 RID: 1858
		public const string ChromaticAberration = "_CHROMATIC_ABERRATION";

		// Token: 0x04000743 RID: 1859
		public const string HDRGrading = "_HDR_GRADING";

		// Token: 0x04000744 RID: 1860
		public const string TonemapACES = "_TONEMAP_ACES";

		// Token: 0x04000745 RID: 1861
		public const string TonemapNeutral = "_TONEMAP_NEUTRAL";

		// Token: 0x04000746 RID: 1862
		public const string FilmGrain = "_FILM_GRAIN";

		// Token: 0x04000747 RID: 1863
		public const string Fxaa = "_FXAA";

		// Token: 0x04000748 RID: 1864
		public const string Dithering = "_DITHERING";

		// Token: 0x04000749 RID: 1865
		public const string ScreenSpaceOcclusion = "_SCREEN_SPACE_OCCLUSION";

		// Token: 0x0400074A RID: 1866
		public const string PointSampling = "_POINT_SAMPLING";

		// Token: 0x0400074B RID: 1867
		public const string Rcas = "_RCAS";

		// Token: 0x0400074C RID: 1868
		public const string EasuRcasAndHDRInput = "_EASU_RCAS_AND_HDR_INPUT";

		// Token: 0x0400074D RID: 1869
		public const string Gamma20 = "_GAMMA_20";

		// Token: 0x0400074E RID: 1870
		public const string FxaaAndGamma20 = "_FXAA_AND_GAMMA_20";

		// Token: 0x0400074F RID: 1871
		public const string HighQualitySampling = "_HIGH_QUALITY_SAMPLING";

		// Token: 0x04000750 RID: 1872
		public const string _SPOT = "_SPOT";

		// Token: 0x04000751 RID: 1873
		public const string _DIRECTIONAL = "_DIRECTIONAL";

		// Token: 0x04000752 RID: 1874
		public const string _POINT = "_POINT";

		// Token: 0x04000753 RID: 1875
		public const string _DEFERRED_STENCIL = "_DEFERRED_STENCIL";

		// Token: 0x04000754 RID: 1876
		public const string _DEFERRED_FIRST_LIGHT = "_DEFERRED_FIRST_LIGHT";

		// Token: 0x04000755 RID: 1877
		public const string _DEFERRED_MAIN_LIGHT = "_DEFERRED_MAIN_LIGHT";

		// Token: 0x04000756 RID: 1878
		public const string _GBUFFER_NORMALS_OCT = "_GBUFFER_NORMALS_OCT";

		// Token: 0x04000757 RID: 1879
		public const string _DEFERRED_MIXED_LIGHTING = "_DEFERRED_MIXED_LIGHTING";

		// Token: 0x04000758 RID: 1880
		public const string LIGHTMAP_ON = "LIGHTMAP_ON";

		// Token: 0x04000759 RID: 1881
		public const string DYNAMICLIGHTMAP_ON = "DYNAMICLIGHTMAP_ON";

		// Token: 0x0400075A RID: 1882
		public const string _ALPHATEST_ON = "_ALPHATEST_ON";

		// Token: 0x0400075B RID: 1883
		public const string DIRLIGHTMAP_COMBINED = "DIRLIGHTMAP_COMBINED";

		// Token: 0x0400075C RID: 1884
		public const string _DETAIL_MULX2 = "_DETAIL_MULX2";

		// Token: 0x0400075D RID: 1885
		public const string _DETAIL_SCALED = "_DETAIL_SCALED";

		// Token: 0x0400075E RID: 1886
		public const string _CLEARCOAT = "_CLEARCOAT";

		// Token: 0x0400075F RID: 1887
		public const string _CLEARCOATMAP = "_CLEARCOATMAP";

		// Token: 0x04000760 RID: 1888
		public const string DEBUG_DISPLAY = "DEBUG_DISPLAY";

		// Token: 0x04000761 RID: 1889
		public const string LOD_FADE_CROSSFADE = "LOD_FADE_CROSSFADE";

		// Token: 0x04000762 RID: 1890
		public const string USE_UNITY_CROSSFADE = "USE_UNITY_CROSSFADE";

		// Token: 0x04000763 RID: 1891
		public const string _EMISSION = "_EMISSION";

		// Token: 0x04000764 RID: 1892
		public const string _RECEIVE_SHADOWS_OFF = "_RECEIVE_SHADOWS_OFF";

		// Token: 0x04000765 RID: 1893
		public const string _SURFACE_TYPE_TRANSPARENT = "_SURFACE_TYPE_TRANSPARENT";

		// Token: 0x04000766 RID: 1894
		public const string _ALPHAPREMULTIPLY_ON = "_ALPHAPREMULTIPLY_ON";

		// Token: 0x04000767 RID: 1895
		public const string _ALPHAMODULATE_ON = "_ALPHAMODULATE_ON";

		// Token: 0x04000768 RID: 1896
		public const string _NORMALMAP = "_NORMALMAP";

		// Token: 0x04000769 RID: 1897
		public const string EDITOR_VISUALIZATION = "EDITOR_VISUALIZATION";

		// Token: 0x0400076A RID: 1898
		public const string DisableTexture2DXArray = "DISABLE_TEXTURE2D_X_ARRAY";

		// Token: 0x0400076B RID: 1899
		public const string BlitSingleSlice = "BLIT_SINGLE_SLICE";

		// Token: 0x0400076C RID: 1900
		public const string XROcclusionMeshCombined = "XR_OCCLUSION_MESH_COMBINED";

		// Token: 0x0400076D RID: 1901
		public const string SCREEN_COORD_OVERRIDE = "SCREEN_COORD_OVERRIDE";

		// Token: 0x0400076E RID: 1902
		public const string DOWNSAMPLING_SIZE_2 = "DOWNSAMPLING_SIZE_2";

		// Token: 0x0400076F RID: 1903
		public const string DOWNSAMPLING_SIZE_4 = "DOWNSAMPLING_SIZE_4";

		// Token: 0x04000770 RID: 1904
		public const string DOWNSAMPLING_SIZE_8 = "DOWNSAMPLING_SIZE_8";

		// Token: 0x04000771 RID: 1905
		public const string DOWNSAMPLING_SIZE_16 = "DOWNSAMPLING_SIZE_16";

		// Token: 0x04000772 RID: 1906
		public const string FoveatedRenderingNonUniformRaster = "_FOVEATED_RENDERING_NON_UNIFORM_RASTER";

		// Token: 0x04000773 RID: 1907
		public const string EVALUATE_SH_MIXED = "EVALUATE_SH_MIXED";

		// Token: 0x04000774 RID: 1908
		public const string EVALUATE_SH_VERTEX = "EVALUATE_SH_VERTEX";

		// Token: 0x04000775 RID: 1909
		public const string UseDrawProcedural = "_USE_DRAW_PROCEDURAL";
	}
}
