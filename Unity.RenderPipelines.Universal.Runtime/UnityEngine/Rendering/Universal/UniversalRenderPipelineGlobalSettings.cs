using System;

namespace UnityEngine.Rendering.Universal
{
	// Token: 0x02000105 RID: 261
	internal class UniversalRenderPipelineGlobalSettings : RenderPipelineGlobalSettings, ISerializationCallbackReceiver, IShaderVariantSettings
	{
		// Token: 0x0600081F RID: 2079 RVA: 0x000341FE File Offset: 0x000323FE
		public void OnBeforeSerialize()
		{
		}

		// Token: 0x06000820 RID: 2080 RVA: 0x00034200 File Offset: 0x00032400
		public void OnAfterDeserialize()
		{
		}

		// Token: 0x170001F4 RID: 500
		// (get) Token: 0x06000821 RID: 2081 RVA: 0x00034202 File Offset: 0x00032402
		public static UniversalRenderPipelineGlobalSettings instance
		{
			get
			{
				if (UniversalRenderPipelineGlobalSettings.cachedInstance == null)
				{
					UniversalRenderPipelineGlobalSettings.cachedInstance = GraphicsSettings.GetSettingsForRenderPipeline<UniversalRenderPipeline>() as UniversalRenderPipelineGlobalSettings;
				}
				return UniversalRenderPipelineGlobalSettings.cachedInstance;
			}
		}

		// Token: 0x06000822 RID: 2082 RVA: 0x00034225 File Offset: 0x00032425
		internal static void UpdateGraphicsSettings(UniversalRenderPipelineGlobalSettings newSettings)
		{
			if (newSettings == UniversalRenderPipelineGlobalSettings.cachedInstance)
			{
				return;
			}
			if (newSettings != null)
			{
				GraphicsSettings.RegisterRenderPipelineSettings<UniversalRenderPipeline>(newSettings);
			}
			else
			{
				GraphicsSettings.UnregisterRenderPipelineSettings<UniversalRenderPipeline>();
			}
			UniversalRenderPipelineGlobalSettings.cachedInstance = newSettings;
		}

		// Token: 0x06000823 RID: 2083 RVA: 0x00034251 File Offset: 0x00032451
		private void Reset()
		{
			this.UpdateRenderingLayerNames();
		}

		// Token: 0x170001F5 RID: 501
		// (get) Token: 0x06000824 RID: 2084 RVA: 0x00034259 File Offset: 0x00032459
		private string[] renderingLayerNames
		{
			get
			{
				if (this.m_RenderingLayerNames == null)
				{
					this.UpdateRenderingLayerNames();
				}
				return this.m_RenderingLayerNames;
			}
		}

		// Token: 0x170001F6 RID: 502
		// (get) Token: 0x06000825 RID: 2085 RVA: 0x0003426F File Offset: 0x0003246F
		private string[] prefixedRenderingLayerNames
		{
			get
			{
				if (this.m_PrefixedRenderingLayerNames == null)
				{
					this.UpdateRenderingLayerNames();
				}
				return this.m_PrefixedRenderingLayerNames;
			}
		}

		// Token: 0x170001F7 RID: 503
		// (get) Token: 0x06000826 RID: 2086 RVA: 0x00034285 File Offset: 0x00032485
		public string[] renderingLayerMaskNames
		{
			get
			{
				return this.renderingLayerNames;
			}
		}

		// Token: 0x170001F8 RID: 504
		// (get) Token: 0x06000827 RID: 2087 RVA: 0x0003428D File Offset: 0x0003248D
		public string[] prefixedRenderingLayerMaskNames
		{
			get
			{
				return this.prefixedRenderingLayerNames;
			}
		}

		// Token: 0x170001F9 RID: 505
		// (get) Token: 0x06000828 RID: 2088 RVA: 0x00034295 File Offset: 0x00032495
		public uint validRenderingLayers
		{
			get
			{
				if (this.m_PrefixedRenderingLayerNames == null)
				{
					this.UpdateRenderingLayerNames();
				}
				return this.m_ValidRenderingLayers;
			}
		}

		// Token: 0x06000829 RID: 2089 RVA: 0x000342AC File Offset: 0x000324AC
		internal void UpdateRenderingLayerNames()
		{
			if (this.m_PrefixedRenderingLayerNames == null)
			{
				this.m_PrefixedRenderingLayerNames = new string[32];
			}
			for (int i = 0; i < this.m_PrefixedRenderingLayerNames.Length; i++)
			{
				uint num = 1U << i;
				this.m_ValidRenderingLayers = ((i < this.m_RenderingLayerNames.Length) ? (this.m_ValidRenderingLayers | num) : (this.m_ValidRenderingLayers & ~num));
				this.m_PrefixedRenderingLayerNames[i] = ((i < this.m_RenderingLayerNames.Length) ? this.m_RenderingLayerNames[i] : string.Format("Unused Layer {0}", i));
			}
			DecalProjector.UpdateAllDecalProperties();
		}

		// Token: 0x170001FA RID: 506
		// (get) Token: 0x0600082A RID: 2090 RVA: 0x0003433E File Offset: 0x0003253E
		[Obsolete("This is obsolete, please use prefixedRenderingLayerMaskNames instead.", false)]
		public string[] prefixedLightLayerNames
		{
			get
			{
				return new string[0];
			}
		}

		// Token: 0x170001FB RID: 507
		// (get) Token: 0x0600082B RID: 2091 RVA: 0x00034346 File Offset: 0x00032546
		[Obsolete("This is obsolete, please use renderingLayerMaskNames instead.", false)]
		public string[] lightLayerNames
		{
			get
			{
				return new string[0];
			}
		}

		// Token: 0x0600082C RID: 2092 RVA: 0x0003434E File Offset: 0x0003254E
		internal void ResetRenderingLayerNames()
		{
			this.m_RenderingLayerNames = new string[] { "Default" };
		}

		// Token: 0x170001FC RID: 508
		// (get) Token: 0x0600082D RID: 2093 RVA: 0x00034364 File Offset: 0x00032564
		// (set) Token: 0x0600082E RID: 2094 RVA: 0x0003436C File Offset: 0x0003256C
		public bool stripDebugVariants
		{
			get
			{
				return this.m_StripDebugVariants;
			}
			set
			{
				this.m_StripDebugVariants = value;
			}
		}

		// Token: 0x170001FD RID: 509
		// (get) Token: 0x0600082F RID: 2095 RVA: 0x00034375 File Offset: 0x00032575
		// (set) Token: 0x06000830 RID: 2096 RVA: 0x0003437D File Offset: 0x0003257D
		public bool stripUnusedPostProcessingVariants
		{
			get
			{
				return this.m_StripUnusedPostProcessingVariants;
			}
			set
			{
				this.m_StripUnusedPostProcessingVariants = value;
			}
		}

		// Token: 0x170001FE RID: 510
		// (get) Token: 0x06000831 RID: 2097 RVA: 0x00034386 File Offset: 0x00032586
		// (set) Token: 0x06000832 RID: 2098 RVA: 0x0003438E File Offset: 0x0003258E
		public bool stripUnusedVariants
		{
			get
			{
				return this.m_StripUnusedVariants;
			}
			set
			{
				this.m_StripUnusedVariants = value;
			}
		}

		// Token: 0x170001FF RID: 511
		// (get) Token: 0x06000833 RID: 2099 RVA: 0x00034397 File Offset: 0x00032597
		// (set) Token: 0x06000834 RID: 2100 RVA: 0x0003439F File Offset: 0x0003259F
		[Obsolete("No longer used as Shader Prefiltering automatically strips out unused LOD Crossfade variants. Please use the LOD Crossfade setting in the URP Asset to disable the feature if not used.", false)]
		public bool stripUnusedLODCrossFadeVariants
		{
			get
			{
				return this.m_StripUnusedLODCrossFadeVariants;
			}
			set
			{
				this.m_StripUnusedLODCrossFadeVariants = value;
			}
		}

		// Token: 0x17000200 RID: 512
		// (get) Token: 0x06000835 RID: 2101 RVA: 0x000343A8 File Offset: 0x000325A8
		// (set) Token: 0x06000836 RID: 2102 RVA: 0x000343B0 File Offset: 0x000325B0
		public bool stripScreenCoordOverrideVariants
		{
			get
			{
				return this.m_StripScreenCoordOverrideVariants;
			}
			set
			{
				this.m_StripScreenCoordOverrideVariants = value;
			}
		}

		// Token: 0x17000201 RID: 513
		// (get) Token: 0x06000837 RID: 2103 RVA: 0x000343B9 File Offset: 0x000325B9
		// (set) Token: 0x06000838 RID: 2104 RVA: 0x000343C1 File Offset: 0x000325C1
		public ShaderVariantLogLevel shaderVariantLogLevel
		{
			get
			{
				return this.m_ShaderVariantLogLevel;
			}
			set
			{
				this.m_ShaderVariantLogLevel = value;
			}
		}

		// Token: 0x17000202 RID: 514
		// (get) Token: 0x06000839 RID: 2105 RVA: 0x000343CA File Offset: 0x000325CA
		// (set) Token: 0x0600083A RID: 2106 RVA: 0x000343D2 File Offset: 0x000325D2
		public bool exportShaderVariants
		{
			get
			{
				return this.m_ExportShaderVariants;
			}
			set
			{
				this.m_ExportShaderVariants = true;
			}
		}

		// Token: 0x0400079A RID: 1946
		[SerializeField]
		private int k_AssetVersion = 3;

		// Token: 0x0400079B RID: 1947
		private static UniversalRenderPipelineGlobalSettings cachedInstance = null;

		// Token: 0x0400079C RID: 1948
		public static readonly string defaultAssetName = "UniversalRenderPipelineGlobalSettings";

		// Token: 0x0400079D RID: 1949
		[SerializeField]
		private string[] m_RenderingLayerNames = new string[] { "Default" };

		// Token: 0x0400079E RID: 1950
		[NonSerialized]
		private string[] m_PrefixedRenderingLayerNames;

		// Token: 0x0400079F RID: 1951
		[SerializeField]
		private uint m_ValidRenderingLayers;

		// Token: 0x040007A0 RID: 1952
		[Obsolete("This is obsolete, please use renderingLayerMaskNames instead.", false)]
		public string lightLayerName0;

		// Token: 0x040007A1 RID: 1953
		[Obsolete("This is obsolete, please use renderingLayerMaskNames instead.", false)]
		public string lightLayerName1;

		// Token: 0x040007A2 RID: 1954
		[Obsolete("This is obsolete, please use renderingLayerMaskNames instead.", false)]
		public string lightLayerName2;

		// Token: 0x040007A3 RID: 1955
		[Obsolete("This is obsolete, please use renderingLayerMaskNames instead.", false)]
		public string lightLayerName3;

		// Token: 0x040007A4 RID: 1956
		[Obsolete("This is obsolete, please use renderingLayerMaskNames instead.", false)]
		public string lightLayerName4;

		// Token: 0x040007A5 RID: 1957
		[Obsolete("This is obsolete, please use renderingLayerMaskNames instead.", false)]
		public string lightLayerName5;

		// Token: 0x040007A6 RID: 1958
		[Obsolete("This is obsolete, please use renderingLayerMaskNames instead.", false)]
		public string lightLayerName6;

		// Token: 0x040007A7 RID: 1959
		[Obsolete("This is obsolete, please use renderingLayerNames instead.", false)]
		public string lightLayerName7;

		// Token: 0x040007A8 RID: 1960
		[SerializeField]
		private bool m_StripDebugVariants = true;

		// Token: 0x040007A9 RID: 1961
		[SerializeField]
		private bool m_StripUnusedPostProcessingVariants;

		// Token: 0x040007AA RID: 1962
		[SerializeField]
		private bool m_StripUnusedVariants = true;

		// Token: 0x040007AB RID: 1963
		[SerializeField]
		private bool m_StripUnusedLODCrossFadeVariants = true;

		// Token: 0x040007AC RID: 1964
		[SerializeField]
		private bool m_StripScreenCoordOverrideVariants = true;

		// Token: 0x040007AD RID: 1965
		[Obsolete("Please use stripRuntimeDebugShaders instead.", false)]
		public bool supportRuntimeDebugDisplay;

		// Token: 0x040007AE RID: 1966
		[SerializeField]
		internal ShaderVariantLogLevel m_ShaderVariantLogLevel;

		// Token: 0x040007AF RID: 1967
		[SerializeField]
		internal bool m_ExportShaderVariants = true;
	}
}
