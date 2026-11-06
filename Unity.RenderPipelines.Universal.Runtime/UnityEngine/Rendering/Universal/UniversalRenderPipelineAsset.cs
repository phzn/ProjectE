using System;
using System.ComponentModel;
using UnityEngine.Experimental.Rendering;

namespace UnityEngine.Rendering.Universal
{
	// Token: 0x02000053 RID: 83
	[ExcludeFromPreset]
	public class UniversalRenderPipelineAsset : RenderPipelineAsset, ISerializationCallbackReceiver
	{
		// Token: 0x1700008F RID: 143
		// (get) Token: 0x06000277 RID: 631 RVA: 0x0001315E File Offset: 0x0001135E
		internal ReadOnlySpan<ScriptableRenderer> renderers
		{
			get
			{
				return this.m_Renderers;
			}
		}

		// Token: 0x06000278 RID: 632 RVA: 0x0001316B File Offset: 0x0001136B
		public ScriptableRendererData LoadBuiltinRendererData(RendererType type = RendererType.UniversalRenderer)
		{
			this.m_RendererDataList[0] = null;
			return this.m_RendererDataList[0];
		}

		// Token: 0x06000279 RID: 633 RVA: 0x00013180 File Offset: 0x00011380
		protected override RenderPipeline CreatePipeline()
		{
			if (this.m_RendererDataList == null)
			{
				this.m_RendererDataList = new ScriptableRendererData[1];
			}
			if (!(this.m_RendererDataList[this.m_DefaultRendererIndex] == null))
			{
				this.DestroyRenderers();
				UniversalRenderPipeline universalRenderPipeline = new UniversalRenderPipeline(this);
				this.CreateRenderers();
				ScriptableRendererData[] rendererDataList = this.m_RendererDataList;
				for (int i = 0; i < rendererDataList.Length; i++)
				{
					UniversalRendererData universalRendererData = rendererDataList[i] as UniversalRendererData;
					if (universalRendererData != null)
					{
						Blitter.Initialize(universalRendererData.shaders.coreBlitPS, universalRendererData.shaders.coreBlitColorAndDepthPS);
						break;
					}
				}
				return universalRenderPipeline;
			}
			if (this.k_AssetPreviousVersion != this.k_AssetVersion)
			{
				return null;
			}
			if (this.m_RendererDataList[this.m_DefaultRendererIndex].GetType().ToString().Contains("Universal.ForwardRendererData"))
			{
				return null;
			}
			Debug.LogError("Default Renderer is missing, make sure there is a Renderer assigned as the default on the current Universal RP asset:" + UniversalRenderPipeline.asset.name, this);
			return null;
		}

		// Token: 0x0600027A RID: 634 RVA: 0x0001325C File Offset: 0x0001145C
		internal void DestroyRenderers()
		{
			if (this.m_Renderers == null)
			{
				return;
			}
			for (int i = 0; i < this.m_Renderers.Length; i++)
			{
				this.DestroyRenderer(ref this.m_Renderers[i]);
			}
		}

		// Token: 0x0600027B RID: 635 RVA: 0x00013297 File Offset: 0x00011497
		private void DestroyRenderer(ref ScriptableRenderer renderer)
		{
			if (renderer != null)
			{
				renderer.Dispose();
				renderer = null;
			}
		}

		// Token: 0x0600027C RID: 636 RVA: 0x000132A7 File Offset: 0x000114A7
		protected override void OnDisable()
		{
			this.DestroyRenderers();
			base.OnDisable();
		}

		// Token: 0x0600027D RID: 637 RVA: 0x000132B8 File Offset: 0x000114B8
		private void CreateRenderers()
		{
			if (this.m_Renderers != null)
			{
				for (int i = 0; i < this.m_Renderers.Length; i++)
				{
					if (this.m_Renderers[i] != null)
					{
						Debug.LogError(string.Format("Creating renderers but previous instance wasn't properly destroyed: m_Renderers[{0}]", i));
					}
				}
			}
			if (this.m_Renderers == null || this.m_Renderers.Length != this.m_RendererDataList.Length)
			{
				this.m_Renderers = new ScriptableRenderer[this.m_RendererDataList.Length];
			}
			for (int j = 0; j < this.m_RendererDataList.Length; j++)
			{
				if (this.m_RendererDataList[j] != null)
				{
					this.m_Renderers[j] = this.m_RendererDataList[j].InternalCreateRenderer();
				}
			}
		}

		// Token: 0x0600027E RID: 638 RVA: 0x00013364 File Offset: 0x00011564
		private Material GetMaterial(DefaultMaterialType materialType)
		{
			return null;
		}

		// Token: 0x17000090 RID: 144
		// (get) Token: 0x0600027F RID: 639 RVA: 0x00013368 File Offset: 0x00011568
		public ScriptableRenderer scriptableRenderer
		{
			get
			{
				ScriptableRendererData[] rendererDataList = this.m_RendererDataList;
				int? num = ((rendererDataList != null) ? new int?(rendererDataList.Length) : null);
				int defaultRendererIndex = this.m_DefaultRendererIndex;
				if (((num.GetValueOrDefault() > defaultRendererIndex) & (num != null)) && this.m_RendererDataList[this.m_DefaultRendererIndex] == null)
				{
					Debug.LogError("Default renderer is missing from the current Pipeline Asset.", this);
					return null;
				}
				if (this.scriptableRendererData.isInvalidated || this.m_Renderers[this.m_DefaultRendererIndex] == null)
				{
					this.DestroyRenderer(ref this.m_Renderers[this.m_DefaultRendererIndex]);
					this.m_Renderers[this.m_DefaultRendererIndex] = this.scriptableRendererData.InternalCreateRenderer();
				}
				return this.m_Renderers[this.m_DefaultRendererIndex];
			}
		}

		// Token: 0x06000280 RID: 640 RVA: 0x00013428 File Offset: 0x00011628
		public ScriptableRenderer GetRenderer(int index)
		{
			if (index == -1)
			{
				index = this.m_DefaultRendererIndex;
			}
			if (index >= this.m_RendererDataList.Length || index < 0 || this.m_RendererDataList[index] == null)
			{
				Debug.LogWarning("Renderer at index " + index.ToString() + " is missing, falling back to Default Renderer " + this.m_RendererDataList[this.m_DefaultRendererIndex].name, this);
				index = this.m_DefaultRendererIndex;
			}
			if (this.m_Renderers == null || this.m_Renderers.Length < this.m_RendererDataList.Length)
			{
				this.DestroyRenderers();
				this.CreateRenderers();
			}
			if (this.m_RendererDataList[index].isInvalidated || this.m_Renderers[index] == null)
			{
				this.DestroyRenderer(ref this.m_Renderers[index]);
				this.m_Renderers[index] = this.m_RendererDataList[index].InternalCreateRenderer();
			}
			return this.m_Renderers[index];
		}

		// Token: 0x17000091 RID: 145
		// (get) Token: 0x06000281 RID: 641 RVA: 0x00013504 File Offset: 0x00011704
		internal ScriptableRendererData scriptableRendererData
		{
			get
			{
				if (this.m_RendererDataList[this.m_DefaultRendererIndex] == null)
				{
					this.CreatePipeline();
				}
				return this.m_RendererDataList[this.m_DefaultRendererIndex];
			}
		}

		// Token: 0x17000092 RID: 146
		// (get) Token: 0x06000282 RID: 642 RVA: 0x00013530 File Offset: 0x00011730
		internal GraphicsFormat additionalLightsCookieFormat
		{
			get
			{
				GraphicsFormat graphicsFormat = GraphicsFormat.None;
				foreach (GraphicsFormat graphicsFormat2 in UniversalRenderPipelineAsset.s_LightCookieFormatList[(int)this.m_AdditionalLightsCookieFormat])
				{
					if (SystemInfo.IsFormatSupported(graphicsFormat2, FormatUsage.Render))
					{
						graphicsFormat = graphicsFormat2;
						break;
					}
				}
				if (QualitySettings.activeColorSpace == ColorSpace.Gamma)
				{
					graphicsFormat = GraphicsFormatUtility.GetLinearFormat(graphicsFormat);
				}
				if (graphicsFormat == GraphicsFormat.None)
				{
					graphicsFormat = GraphicsFormat.R8G8B8A8_UNorm;
					Debug.LogWarning(string.Format("Additional Lights Cookie Format ({0}) is not supported by the platform. Falling back to {1}-bit format ({2})", this.m_AdditionalLightsCookieFormat.ToString(), GraphicsFormatUtility.GetBlockSize(graphicsFormat) * 8U, GraphicsFormatUtility.GetFormatString(graphicsFormat)));
				}
				return graphicsFormat;
			}
		}

		// Token: 0x17000093 RID: 147
		// (get) Token: 0x06000283 RID: 643 RVA: 0x000135B2 File Offset: 0x000117B2
		internal Vector2Int additionalLightsCookieResolution
		{
			get
			{
				return new Vector2Int((int)this.m_AdditionalLightsCookieResolution, (int)this.m_AdditionalLightsCookieResolution);
			}
		}

		// Token: 0x17000094 RID: 148
		// (get) Token: 0x06000284 RID: 644 RVA: 0x000135C8 File Offset: 0x000117C8
		internal int[] rendererIndexList
		{
			get
			{
				int[] array = new int[this.m_RendererDataList.Length + 1];
				for (int i = 0; i < array.Length; i++)
				{
					array[i] = i - 1;
				}
				return array;
			}
		}

		// Token: 0x17000095 RID: 149
		// (get) Token: 0x06000285 RID: 645 RVA: 0x000135FA File Offset: 0x000117FA
		// (set) Token: 0x06000286 RID: 646 RVA: 0x00013602 File Offset: 0x00011802
		public bool supportsCameraDepthTexture
		{
			get
			{
				return this.m_RequireDepthTexture;
			}
			set
			{
				this.m_RequireDepthTexture = value;
			}
		}

		// Token: 0x17000096 RID: 150
		// (get) Token: 0x06000287 RID: 647 RVA: 0x0001360B File Offset: 0x0001180B
		// (set) Token: 0x06000288 RID: 648 RVA: 0x00013613 File Offset: 0x00011813
		public bool supportsCameraOpaqueTexture
		{
			get
			{
				return this.m_RequireOpaqueTexture;
			}
			set
			{
				this.m_RequireOpaqueTexture = value;
			}
		}

		// Token: 0x17000097 RID: 151
		// (get) Token: 0x06000289 RID: 649 RVA: 0x0001361C File Offset: 0x0001181C
		public Downsampling opaqueDownsampling
		{
			get
			{
				return this.m_OpaqueDownsampling;
			}
		}

		// Token: 0x17000098 RID: 152
		// (get) Token: 0x0600028A RID: 650 RVA: 0x00013624 File Offset: 0x00011824
		public bool supportsTerrainHoles
		{
			get
			{
				return this.m_SupportsTerrainHoles;
			}
		}

		// Token: 0x17000099 RID: 153
		// (get) Token: 0x0600028B RID: 651 RVA: 0x0001362C File Offset: 0x0001182C
		// (set) Token: 0x0600028C RID: 652 RVA: 0x00013634 File Offset: 0x00011834
		public StoreActionsOptimization storeActionsOptimization
		{
			get
			{
				return this.m_StoreActionsOptimization;
			}
			set
			{
				this.m_StoreActionsOptimization = value;
			}
		}

		// Token: 0x1700009A RID: 154
		// (get) Token: 0x0600028D RID: 653 RVA: 0x0001363D File Offset: 0x0001183D
		// (set) Token: 0x0600028E RID: 654 RVA: 0x00013645 File Offset: 0x00011845
		public bool supportsHDR
		{
			get
			{
				return this.m_SupportsHDR;
			}
			set
			{
				this.m_SupportsHDR = value;
			}
		}

		// Token: 0x1700009B RID: 155
		// (get) Token: 0x0600028F RID: 655 RVA: 0x0001364E File Offset: 0x0001184E
		// (set) Token: 0x06000290 RID: 656 RVA: 0x00013656 File Offset: 0x00011856
		public HDRColorBufferPrecision hdrColorBufferPrecision
		{
			get
			{
				return this.m_HDRColorBufferPrecision;
			}
			set
			{
				this.m_HDRColorBufferPrecision = value;
			}
		}

		// Token: 0x1700009C RID: 156
		// (get) Token: 0x06000291 RID: 657 RVA: 0x0001365F File Offset: 0x0001185F
		// (set) Token: 0x06000292 RID: 658 RVA: 0x00013667 File Offset: 0x00011867
		public int msaaSampleCount
		{
			get
			{
				return (int)this.m_MSAA;
			}
			set
			{
				this.m_MSAA = (MsaaQuality)value;
			}
		}

		// Token: 0x1700009D RID: 157
		// (get) Token: 0x06000293 RID: 659 RVA: 0x00013670 File Offset: 0x00011870
		// (set) Token: 0x06000294 RID: 660 RVA: 0x00013678 File Offset: 0x00011878
		public float renderScale
		{
			get
			{
				return this.m_RenderScale;
			}
			set
			{
				this.m_RenderScale = this.ValidateRenderScale(value);
			}
		}

		// Token: 0x1700009E RID: 158
		// (get) Token: 0x06000295 RID: 661 RVA: 0x00013687 File Offset: 0x00011887
		public bool enableLODCrossFade
		{
			get
			{
				return this.m_EnableLODCrossFade;
			}
		}

		// Token: 0x1700009F RID: 159
		// (get) Token: 0x06000296 RID: 662 RVA: 0x0001368F File Offset: 0x0001188F
		public LODCrossFadeDitheringType lodCrossFadeDitheringType
		{
			get
			{
				return this.m_LODCrossFadeDitheringType;
			}
		}

		// Token: 0x170000A0 RID: 160
		// (get) Token: 0x06000297 RID: 663 RVA: 0x00013697 File Offset: 0x00011897
		// (set) Token: 0x06000298 RID: 664 RVA: 0x0001369F File Offset: 0x0001189F
		public UpscalingFilterSelection upscalingFilter
		{
			get
			{
				return this.m_UpscalingFilter;
			}
			set
			{
				this.m_UpscalingFilter = value;
			}
		}

		// Token: 0x170000A1 RID: 161
		// (get) Token: 0x06000299 RID: 665 RVA: 0x000136A8 File Offset: 0x000118A8
		// (set) Token: 0x0600029A RID: 666 RVA: 0x000136B0 File Offset: 0x000118B0
		public bool fsrOverrideSharpness
		{
			get
			{
				return this.m_FsrOverrideSharpness;
			}
			set
			{
				this.m_FsrOverrideSharpness = value;
			}
		}

		// Token: 0x170000A2 RID: 162
		// (get) Token: 0x0600029B RID: 667 RVA: 0x000136B9 File Offset: 0x000118B9
		// (set) Token: 0x0600029C RID: 668 RVA: 0x000136C1 File Offset: 0x000118C1
		public float fsrSharpness
		{
			get
			{
				return this.m_FsrSharpness;
			}
			set
			{
				this.m_FsrSharpness = value;
			}
		}

		// Token: 0x170000A3 RID: 163
		// (get) Token: 0x0600029D RID: 669 RVA: 0x000136CA File Offset: 0x000118CA
		// (set) Token: 0x0600029E RID: 670 RVA: 0x000136D2 File Offset: 0x000118D2
		public ShEvalMode shEvalMode
		{
			get
			{
				return this.m_ShEvalMode;
			}
			internal set
			{
				this.m_ShEvalMode = value;
			}
		}

		// Token: 0x170000A4 RID: 164
		// (get) Token: 0x0600029F RID: 671 RVA: 0x000136DB File Offset: 0x000118DB
		// (set) Token: 0x060002A0 RID: 672 RVA: 0x000136E3 File Offset: 0x000118E3
		public LightRenderingMode mainLightRenderingMode
		{
			get
			{
				return this.m_MainLightRenderingMode;
			}
			internal set
			{
				this.m_MainLightRenderingMode = value;
			}
		}

		// Token: 0x170000A5 RID: 165
		// (get) Token: 0x060002A1 RID: 673 RVA: 0x000136EC File Offset: 0x000118EC
		// (set) Token: 0x060002A2 RID: 674 RVA: 0x000136F4 File Offset: 0x000118F4
		public bool supportsMainLightShadows
		{
			get
			{
				return this.m_MainLightShadowsSupported;
			}
			internal set
			{
				this.m_MainLightShadowsSupported = value;
			}
		}

		// Token: 0x170000A6 RID: 166
		// (get) Token: 0x060002A3 RID: 675 RVA: 0x000136FD File Offset: 0x000118FD
		// (set) Token: 0x060002A4 RID: 676 RVA: 0x00013705 File Offset: 0x00011905
		public int mainLightShadowmapResolution
		{
			get
			{
				return (int)this.m_MainLightShadowmapResolution;
			}
			internal set
			{
				this.m_MainLightShadowmapResolution = (ShadowResolution)value;
			}
		}

		// Token: 0x170000A7 RID: 167
		// (get) Token: 0x060002A5 RID: 677 RVA: 0x0001370E File Offset: 0x0001190E
		// (set) Token: 0x060002A6 RID: 678 RVA: 0x00013716 File Offset: 0x00011916
		public LightRenderingMode additionalLightsRenderingMode
		{
			get
			{
				return this.m_AdditionalLightsRenderingMode;
			}
			internal set
			{
				this.m_AdditionalLightsRenderingMode = value;
			}
		}

		// Token: 0x170000A8 RID: 168
		// (get) Token: 0x060002A7 RID: 679 RVA: 0x0001371F File Offset: 0x0001191F
		// (set) Token: 0x060002A8 RID: 680 RVA: 0x00013727 File Offset: 0x00011927
		public int maxAdditionalLightsCount
		{
			get
			{
				return this.m_AdditionalLightsPerObjectLimit;
			}
			set
			{
				this.m_AdditionalLightsPerObjectLimit = this.ValidatePerObjectLights(value);
			}
		}

		// Token: 0x170000A9 RID: 169
		// (get) Token: 0x060002A9 RID: 681 RVA: 0x00013736 File Offset: 0x00011936
		// (set) Token: 0x060002AA RID: 682 RVA: 0x0001373E File Offset: 0x0001193E
		public bool supportsAdditionalLightShadows
		{
			get
			{
				return this.m_AdditionalLightShadowsSupported;
			}
			internal set
			{
				this.m_AdditionalLightShadowsSupported = value;
			}
		}

		// Token: 0x170000AA RID: 170
		// (get) Token: 0x060002AB RID: 683 RVA: 0x00013747 File Offset: 0x00011947
		// (set) Token: 0x060002AC RID: 684 RVA: 0x0001374F File Offset: 0x0001194F
		public int additionalLightsShadowmapResolution
		{
			get
			{
				return (int)this.m_AdditionalLightsShadowmapResolution;
			}
			internal set
			{
				this.m_AdditionalLightsShadowmapResolution = (ShadowResolution)value;
			}
		}

		// Token: 0x170000AB RID: 171
		// (get) Token: 0x060002AD RID: 685 RVA: 0x00013758 File Offset: 0x00011958
		// (set) Token: 0x060002AE RID: 686 RVA: 0x00013760 File Offset: 0x00011960
		public int additionalLightsShadowResolutionTierLow
		{
			get
			{
				return this.m_AdditionalLightsShadowResolutionTierLow;
			}
			internal set
			{
				this.m_AdditionalLightsShadowResolutionTierLow = value;
			}
		}

		// Token: 0x170000AC RID: 172
		// (get) Token: 0x060002AF RID: 687 RVA: 0x00013769 File Offset: 0x00011969
		// (set) Token: 0x060002B0 RID: 688 RVA: 0x00013771 File Offset: 0x00011971
		public int additionalLightsShadowResolutionTierMedium
		{
			get
			{
				return this.m_AdditionalLightsShadowResolutionTierMedium;
			}
			internal set
			{
				this.m_AdditionalLightsShadowResolutionTierMedium = value;
			}
		}

		// Token: 0x170000AD RID: 173
		// (get) Token: 0x060002B1 RID: 689 RVA: 0x0001377A File Offset: 0x0001197A
		// (set) Token: 0x060002B2 RID: 690 RVA: 0x00013782 File Offset: 0x00011982
		public int additionalLightsShadowResolutionTierHigh
		{
			get
			{
				return this.m_AdditionalLightsShadowResolutionTierHigh;
			}
			internal set
			{
				this.m_AdditionalLightsShadowResolutionTierHigh = value;
			}
		}

		// Token: 0x060002B3 RID: 691 RVA: 0x0001378B File Offset: 0x0001198B
		internal int GetAdditionalLightsShadowResolution(int additionalLightsShadowResolutionTier)
		{
			if (additionalLightsShadowResolutionTier <= UniversalAdditionalLightData.AdditionalLightsShadowResolutionTierLow)
			{
				return this.additionalLightsShadowResolutionTierLow;
			}
			if (additionalLightsShadowResolutionTier == UniversalAdditionalLightData.AdditionalLightsShadowResolutionTierMedium)
			{
				return this.additionalLightsShadowResolutionTierMedium;
			}
			if (additionalLightsShadowResolutionTier >= UniversalAdditionalLightData.AdditionalLightsShadowResolutionTierHigh)
			{
				return this.additionalLightsShadowResolutionTierHigh;
			}
			return this.additionalLightsShadowResolutionTierMedium;
		}

		// Token: 0x170000AE RID: 174
		// (get) Token: 0x060002B4 RID: 692 RVA: 0x000137C0 File Offset: 0x000119C0
		// (set) Token: 0x060002B5 RID: 693 RVA: 0x000137C8 File Offset: 0x000119C8
		public bool reflectionProbeBlending
		{
			get
			{
				return this.m_ReflectionProbeBlending;
			}
			internal set
			{
				this.m_ReflectionProbeBlending = value;
			}
		}

		// Token: 0x170000AF RID: 175
		// (get) Token: 0x060002B6 RID: 694 RVA: 0x000137D1 File Offset: 0x000119D1
		// (set) Token: 0x060002B7 RID: 695 RVA: 0x000137D9 File Offset: 0x000119D9
		public bool reflectionProbeBoxProjection
		{
			get
			{
				return this.m_ReflectionProbeBoxProjection;
			}
			internal set
			{
				this.m_ReflectionProbeBoxProjection = value;
			}
		}

		// Token: 0x170000B0 RID: 176
		// (get) Token: 0x060002B8 RID: 696 RVA: 0x000137E2 File Offset: 0x000119E2
		// (set) Token: 0x060002B9 RID: 697 RVA: 0x000137EA File Offset: 0x000119EA
		public float shadowDistance
		{
			get
			{
				return this.m_ShadowDistance;
			}
			set
			{
				this.m_ShadowDistance = Mathf.Max(0f, value);
			}
		}

		// Token: 0x170000B1 RID: 177
		// (get) Token: 0x060002BA RID: 698 RVA: 0x000137FD File Offset: 0x000119FD
		// (set) Token: 0x060002BB RID: 699 RVA: 0x00013805 File Offset: 0x00011A05
		public int shadowCascadeCount
		{
			get
			{
				return this.m_ShadowCascadeCount;
			}
			set
			{
				if (value < 1 || value > 4)
				{
					throw new ArgumentException(string.Format("Value ({0}) needs to be between {1} and {2}.", value, 1, 4));
				}
				this.m_ShadowCascadeCount = value;
			}
		}

		// Token: 0x170000B2 RID: 178
		// (get) Token: 0x060002BC RID: 700 RVA: 0x00013838 File Offset: 0x00011A38
		// (set) Token: 0x060002BD RID: 701 RVA: 0x00013840 File Offset: 0x00011A40
		public float cascade2Split
		{
			get
			{
				return this.m_Cascade2Split;
			}
			internal set
			{
				this.m_Cascade2Split = value;
			}
		}

		// Token: 0x170000B3 RID: 179
		// (get) Token: 0x060002BE RID: 702 RVA: 0x00013849 File Offset: 0x00011A49
		// (set) Token: 0x060002BF RID: 703 RVA: 0x00013851 File Offset: 0x00011A51
		public Vector2 cascade3Split
		{
			get
			{
				return this.m_Cascade3Split;
			}
			internal set
			{
				this.m_Cascade3Split = value;
			}
		}

		// Token: 0x170000B4 RID: 180
		// (get) Token: 0x060002C0 RID: 704 RVA: 0x0001385A File Offset: 0x00011A5A
		// (set) Token: 0x060002C1 RID: 705 RVA: 0x00013862 File Offset: 0x00011A62
		public Vector3 cascade4Split
		{
			get
			{
				return this.m_Cascade4Split;
			}
			internal set
			{
				this.m_Cascade4Split = value;
			}
		}

		// Token: 0x170000B5 RID: 181
		// (get) Token: 0x060002C2 RID: 706 RVA: 0x0001386B File Offset: 0x00011A6B
		// (set) Token: 0x060002C3 RID: 707 RVA: 0x00013873 File Offset: 0x00011A73
		public float cascadeBorder
		{
			get
			{
				return this.m_CascadeBorder;
			}
			set
			{
				this.m_CascadeBorder = value;
			}
		}

		// Token: 0x170000B6 RID: 182
		// (get) Token: 0x060002C4 RID: 708 RVA: 0x0001387C File Offset: 0x00011A7C
		// (set) Token: 0x060002C5 RID: 709 RVA: 0x00013884 File Offset: 0x00011A84
		public float shadowDepthBias
		{
			get
			{
				return this.m_ShadowDepthBias;
			}
			set
			{
				this.m_ShadowDepthBias = this.ValidateShadowBias(value);
			}
		}

		// Token: 0x170000B7 RID: 183
		// (get) Token: 0x060002C6 RID: 710 RVA: 0x00013893 File Offset: 0x00011A93
		// (set) Token: 0x060002C7 RID: 711 RVA: 0x0001389B File Offset: 0x00011A9B
		public float shadowNormalBias
		{
			get
			{
				return this.m_ShadowNormalBias;
			}
			set
			{
				this.m_ShadowNormalBias = this.ValidateShadowBias(value);
			}
		}

		// Token: 0x170000B8 RID: 184
		// (get) Token: 0x060002C8 RID: 712 RVA: 0x000138AA File Offset: 0x00011AAA
		// (set) Token: 0x060002C9 RID: 713 RVA: 0x000138B2 File Offset: 0x00011AB2
		public bool supportsSoftShadows
		{
			get
			{
				return this.m_SoftShadowsSupported;
			}
			internal set
			{
				this.m_SoftShadowsSupported = value;
			}
		}

		// Token: 0x170000B9 RID: 185
		// (get) Token: 0x060002CA RID: 714 RVA: 0x000138BB File Offset: 0x00011ABB
		// (set) Token: 0x060002CB RID: 715 RVA: 0x000138C3 File Offset: 0x00011AC3
		internal SoftShadowQuality softShadowQuality
		{
			get
			{
				return this.m_SoftShadowQuality;
			}
			set
			{
				this.m_SoftShadowQuality = value;
			}
		}

		// Token: 0x170000BA RID: 186
		// (get) Token: 0x060002CC RID: 716 RVA: 0x000138CC File Offset: 0x00011ACC
		// (set) Token: 0x060002CD RID: 717 RVA: 0x000138D4 File Offset: 0x00011AD4
		public bool supportsDynamicBatching
		{
			get
			{
				return this.m_SupportsDynamicBatching;
			}
			set
			{
				this.m_SupportsDynamicBatching = value;
			}
		}

		// Token: 0x170000BB RID: 187
		// (get) Token: 0x060002CE RID: 718 RVA: 0x000138DD File Offset: 0x00011ADD
		public bool supportsMixedLighting
		{
			get
			{
				return this.m_MixedLightingSupported;
			}
		}

		// Token: 0x170000BC RID: 188
		// (get) Token: 0x060002CF RID: 719 RVA: 0x000138E5 File Offset: 0x00011AE5
		public bool supportsLightCookies
		{
			get
			{
				return this.m_SupportsLightCookies;
			}
		}

		// Token: 0x170000BD RID: 189
		// (get) Token: 0x060002D0 RID: 720 RVA: 0x000138ED File Offset: 0x00011AED
		[Obsolete("This is obsolete, UnityEngine.Rendering.ShaderVariantLogLevel instead.", false)]
		public bool supportsLightLayers
		{
			get
			{
				return this.m_SupportsLightLayers;
			}
		}

		// Token: 0x170000BE RID: 190
		// (get) Token: 0x060002D1 RID: 721 RVA: 0x000138F5 File Offset: 0x00011AF5
		public bool useRenderingLayers
		{
			get
			{
				return this.m_SupportsLightLayers;
			}
		}

		// Token: 0x170000BF RID: 191
		// (get) Token: 0x060002D2 RID: 722 RVA: 0x000138FD File Offset: 0x00011AFD
		public VolumeFrameworkUpdateMode volumeFrameworkUpdateMode
		{
			get
			{
				return this.m_VolumeFrameworkUpdateMode;
			}
		}

		// Token: 0x170000C0 RID: 192
		// (get) Token: 0x060002D3 RID: 723 RVA: 0x00013905 File Offset: 0x00011B05
		[Obsolete("PipelineDebugLevel is deprecated and replaced to use the profiler. Calling debugLevel is not necessary.", false)]
		public PipelineDebugLevel debugLevel
		{
			get
			{
				return PipelineDebugLevel.Disabled;
			}
		}

		// Token: 0x170000C1 RID: 193
		// (get) Token: 0x060002D4 RID: 724 RVA: 0x00013908 File Offset: 0x00011B08
		// (set) Token: 0x060002D5 RID: 725 RVA: 0x00013910 File Offset: 0x00011B10
		public bool useSRPBatcher
		{
			get
			{
				return this.m_UseSRPBatcher;
			}
			set
			{
				this.m_UseSRPBatcher = value;
			}
		}

		// Token: 0x170000C2 RID: 194
		// (get) Token: 0x060002D6 RID: 726 RVA: 0x00013919 File Offset: 0x00011B19
		// (set) Token: 0x060002D7 RID: 727 RVA: 0x00013921 File Offset: 0x00011B21
		internal bool enableRenderGraph
		{
			get
			{
				return this.m_EnableRenderGraph;
			}
			set
			{
				this.m_EnableRenderGraph = value;
			}
		}

		// Token: 0x170000C3 RID: 195
		// (get) Token: 0x060002D8 RID: 728 RVA: 0x0001392A File Offset: 0x00011B2A
		// (set) Token: 0x060002D9 RID: 729 RVA: 0x00013932 File Offset: 0x00011B32
		public ColorGradingMode colorGradingMode
		{
			get
			{
				return this.m_ColorGradingMode;
			}
			set
			{
				this.m_ColorGradingMode = value;
			}
		}

		// Token: 0x170000C4 RID: 196
		// (get) Token: 0x060002DA RID: 730 RVA: 0x0001393B File Offset: 0x00011B3B
		// (set) Token: 0x060002DB RID: 731 RVA: 0x00013943 File Offset: 0x00011B43
		public int colorGradingLutSize
		{
			get
			{
				return this.m_ColorGradingLutSize;
			}
			set
			{
				this.m_ColorGradingLutSize = Mathf.Clamp(value, 16, 65);
			}
		}

		// Token: 0x170000C5 RID: 197
		// (get) Token: 0x060002DC RID: 732 RVA: 0x00013955 File Offset: 0x00011B55
		public bool useFastSRGBLinearConversion
		{
			get
			{
				return this.m_UseFastSRGBLinearConversion;
			}
		}

		// Token: 0x170000C6 RID: 198
		// (get) Token: 0x060002DD RID: 733 RVA: 0x0001395D File Offset: 0x00011B5D
		public bool supportDataDrivenLensFlare
		{
			get
			{
				return this.m_SupportDataDrivenLensFlare;
			}
		}

		// Token: 0x170000C7 RID: 199
		// (get) Token: 0x060002DE RID: 734 RVA: 0x00013965 File Offset: 0x00011B65
		// (set) Token: 0x060002DF RID: 735 RVA: 0x0001396D File Offset: 0x00011B6D
		public bool useAdaptivePerformance
		{
			get
			{
				return this.m_UseAdaptivePerformance;
			}
			set
			{
				this.m_UseAdaptivePerformance = value;
			}
		}

		// Token: 0x170000C8 RID: 200
		// (get) Token: 0x060002E0 RID: 736 RVA: 0x00013976 File Offset: 0x00011B76
		// (set) Token: 0x060002E1 RID: 737 RVA: 0x0001397E File Offset: 0x00011B7E
		public bool conservativeEnclosingSphere
		{
			get
			{
				return this.m_ConservativeEnclosingSphere;
			}
			set
			{
				this.m_ConservativeEnclosingSphere = value;
			}
		}

		// Token: 0x170000C9 RID: 201
		// (get) Token: 0x060002E2 RID: 738 RVA: 0x00013987 File Offset: 0x00011B87
		// (set) Token: 0x060002E3 RID: 739 RVA: 0x0001398F File Offset: 0x00011B8F
		public int numIterationsEnclosingSphere
		{
			get
			{
				return this.m_NumIterationsEnclosingSphere;
			}
			set
			{
				this.m_NumIterationsEnclosingSphere = value;
			}
		}

		// Token: 0x170000CA RID: 202
		// (get) Token: 0x060002E4 RID: 740 RVA: 0x00013998 File Offset: 0x00011B98
		public override Material defaultMaterial
		{
			get
			{
				return this.GetMaterial(DefaultMaterialType.Standard);
			}
		}

		// Token: 0x170000CB RID: 203
		// (get) Token: 0x060002E5 RID: 741 RVA: 0x000139A1 File Offset: 0x00011BA1
		public override Material defaultParticleMaterial
		{
			get
			{
				return this.GetMaterial(DefaultMaterialType.Particle);
			}
		}

		// Token: 0x170000CC RID: 204
		// (get) Token: 0x060002E6 RID: 742 RVA: 0x000139AA File Offset: 0x00011BAA
		public override Material defaultLineMaterial
		{
			get
			{
				return this.GetMaterial(DefaultMaterialType.Particle);
			}
		}

		// Token: 0x170000CD RID: 205
		// (get) Token: 0x060002E7 RID: 743 RVA: 0x000139B3 File Offset: 0x00011BB3
		public override Material defaultTerrainMaterial
		{
			get
			{
				return this.GetMaterial(DefaultMaterialType.Terrain);
			}
		}

		// Token: 0x170000CE RID: 206
		// (get) Token: 0x060002E8 RID: 744 RVA: 0x000139BC File Offset: 0x00011BBC
		public override Material defaultUIMaterial
		{
			get
			{
				return this.GetMaterial(DefaultMaterialType.UnityBuiltinDefault);
			}
		}

		// Token: 0x170000CF RID: 207
		// (get) Token: 0x060002E9 RID: 745 RVA: 0x000139C5 File Offset: 0x00011BC5
		public override Material defaultUIOverdrawMaterial
		{
			get
			{
				return this.GetMaterial(DefaultMaterialType.UnityBuiltinDefault);
			}
		}

		// Token: 0x170000D0 RID: 208
		// (get) Token: 0x060002EA RID: 746 RVA: 0x000139CE File Offset: 0x00011BCE
		public override Material defaultUIETC1SupportedMaterial
		{
			get
			{
				return this.GetMaterial(DefaultMaterialType.UnityBuiltinDefault);
			}
		}

		// Token: 0x170000D1 RID: 209
		// (get) Token: 0x060002EB RID: 747 RVA: 0x000139D7 File Offset: 0x00011BD7
		public override Material default2DMaterial
		{
			get
			{
				return this.GetMaterial(DefaultMaterialType.Sprite);
			}
		}

		// Token: 0x170000D2 RID: 210
		// (get) Token: 0x060002EC RID: 748 RVA: 0x000139E0 File Offset: 0x00011BE0
		public override Material default2DMaskMaterial
		{
			get
			{
				return this.GetMaterial(DefaultMaterialType.SpriteMask);
			}
		}

		// Token: 0x170000D3 RID: 211
		// (get) Token: 0x060002ED RID: 749 RVA: 0x000139E9 File Offset: 0x00011BE9
		public Material decalMaterial
		{
			get
			{
				return this.GetMaterial(DefaultMaterialType.Decal);
			}
		}

		// Token: 0x170000D4 RID: 212
		// (get) Token: 0x060002EE RID: 750 RVA: 0x000139F2 File Offset: 0x00011BF2
		public override Shader defaultShader
		{
			get
			{
				if (this.m_DefaultShader == null)
				{
					this.m_DefaultShader = Shader.Find(ShaderUtils.GetShaderPath(ShaderPathID.Lit));
				}
				return this.m_DefaultShader;
			}
		}

		// Token: 0x170000D5 RID: 213
		// (get) Token: 0x060002EF RID: 751 RVA: 0x00013A1C File Offset: 0x00011C1C
		public override Shader terrainDetailLitShader
		{
			get
			{
				ScriptableRendererData[] rendererDataList = this.m_RendererDataList;
				for (int i = 0; i < rendererDataList.Length; i++)
				{
					UniversalRendererData universalRendererData = rendererDataList[i] as UniversalRendererData;
					if (universalRendererData != null)
					{
						return universalRendererData.shaders.terrainDetailLitPS;
					}
				}
				return null;
			}
		}

		// Token: 0x170000D6 RID: 214
		// (get) Token: 0x060002F0 RID: 752 RVA: 0x00013A58 File Offset: 0x00011C58
		public override Shader terrainDetailGrassShader
		{
			get
			{
				ScriptableRendererData[] rendererDataList = this.m_RendererDataList;
				for (int i = 0; i < rendererDataList.Length; i++)
				{
					UniversalRendererData universalRendererData = rendererDataList[i] as UniversalRendererData;
					if (universalRendererData != null)
					{
						return universalRendererData.shaders.terrainDetailGrassPS;
					}
				}
				return null;
			}
		}

		// Token: 0x170000D7 RID: 215
		// (get) Token: 0x060002F1 RID: 753 RVA: 0x00013A94 File Offset: 0x00011C94
		public override Shader terrainDetailGrassBillboardShader
		{
			get
			{
				ScriptableRendererData[] rendererDataList = this.m_RendererDataList;
				for (int i = 0; i < rendererDataList.Length; i++)
				{
					UniversalRendererData universalRendererData = rendererDataList[i] as UniversalRendererData;
					if (universalRendererData != null)
					{
						return universalRendererData.shaders.terrainDetailGrassBillboardPS;
					}
				}
				return null;
			}
		}

		// Token: 0x170000D8 RID: 216
		// (get) Token: 0x060002F2 RID: 754 RVA: 0x00013ACF File Offset: 0x00011CCF
		public override string[] renderingLayerMaskNames
		{
			get
			{
				return UniversalRenderPipelineGlobalSettings.instance.renderingLayerMaskNames;
			}
		}

		// Token: 0x170000D9 RID: 217
		// (get) Token: 0x060002F3 RID: 755 RVA: 0x00013ADB File Offset: 0x00011CDB
		public override string[] prefixedRenderingLayerMaskNames
		{
			get
			{
				return UniversalRenderPipelineGlobalSettings.instance.prefixedRenderingLayerMaskNames;
			}
		}

		// Token: 0x170000DA RID: 218
		// (get) Token: 0x060002F4 RID: 756 RVA: 0x00013AE7 File Offset: 0x00011CE7
		[Obsolete("This is obsolete, please use renderingLayerMaskNames instead.", false)]
		public string[] lightLayerMaskNames
		{
			get
			{
				return new string[0];
			}
		}

		// Token: 0x170000DB RID: 219
		// (get) Token: 0x060002F5 RID: 757 RVA: 0x00013AEF File Offset: 0x00011CEF
		public UniversalRenderPipelineAsset.TextureResources textures
		{
			get
			{
				if (this.m_Textures == null)
				{
					this.m_Textures = new UniversalRenderPipelineAsset.TextureResources();
				}
				return this.m_Textures;
			}
		}

		// Token: 0x060002F6 RID: 758 RVA: 0x00013B0A File Offset: 0x00011D0A
		public void OnBeforeSerialize()
		{
		}

		// Token: 0x060002F7 RID: 759 RVA: 0x00013B0C File Offset: 0x00011D0C
		public void OnAfterDeserialize()
		{
			if (this.k_AssetVersion < 3)
			{
				this.m_SoftShadowsSupported = this.m_ShadowType == ShadowQuality.SoftShadows;
				this.k_AssetPreviousVersion = this.k_AssetVersion;
				this.k_AssetVersion = 3;
			}
			if (this.k_AssetVersion < 4)
			{
				this.m_AdditionalLightShadowsSupported = this.m_LocalShadowsSupported;
				this.m_AdditionalLightsShadowmapResolution = this.m_LocalShadowsAtlasResolution;
				this.m_AdditionalLightsPerObjectLimit = this.m_MaxPixelLights;
				this.m_MainLightShadowmapResolution = this.m_ShadowAtlasResolution;
				this.k_AssetPreviousVersion = this.k_AssetVersion;
				this.k_AssetVersion = 4;
			}
			if (this.k_AssetVersion < 5)
			{
				if (this.m_RendererType == RendererType.Custom)
				{
					this.m_RendererDataList[0] = this.m_RendererData;
				}
				this.k_AssetPreviousVersion = this.k_AssetVersion;
				this.k_AssetVersion = 5;
			}
			if (this.k_AssetVersion < 6)
			{
				int shadowCascades = (int)this.m_ShadowCascades;
				if (shadowCascades == 2)
				{
					this.m_ShadowCascadeCount = 4;
				}
				else
				{
					this.m_ShadowCascadeCount = shadowCascades + 1;
				}
				this.k_AssetVersion = 6;
			}
			if (this.k_AssetVersion < 7)
			{
				this.k_AssetPreviousVersion = this.k_AssetVersion;
				this.k_AssetVersion = 7;
			}
			if (this.k_AssetVersion < 8)
			{
				this.k_AssetPreviousVersion = this.k_AssetVersion;
				this.m_CascadeBorder = 0.1f;
				this.k_AssetVersion = 8;
			}
			if (this.k_AssetVersion < 9)
			{
				if (this.m_AdditionalLightsShadowResolutionTierHigh == UniversalRenderPipelineAsset.AdditionalLightsDefaultShadowResolutionTierHigh && this.m_AdditionalLightsShadowResolutionTierMedium == UniversalRenderPipelineAsset.AdditionalLightsDefaultShadowResolutionTierMedium && this.m_AdditionalLightsShadowResolutionTierLow == UniversalRenderPipelineAsset.AdditionalLightsDefaultShadowResolutionTierLow)
				{
					this.m_AdditionalLightsShadowResolutionTierHigh = (int)this.m_AdditionalLightsShadowmapResolution;
					this.m_AdditionalLightsShadowResolutionTierMedium = Mathf.Max(this.m_AdditionalLightsShadowResolutionTierHigh / 2, UniversalAdditionalLightData.AdditionalLightsShadowMinimumResolution);
					this.m_AdditionalLightsShadowResolutionTierLow = Mathf.Max(this.m_AdditionalLightsShadowResolutionTierMedium / 2, UniversalAdditionalLightData.AdditionalLightsShadowMinimumResolution);
				}
				this.k_AssetPreviousVersion = this.k_AssetVersion;
				this.k_AssetVersion = 9;
			}
			if (this.k_AssetVersion < 10)
			{
				this.k_AssetPreviousVersion = this.k_AssetVersion;
				this.k_AssetVersion = 10;
			}
			if (this.k_AssetVersion < 11)
			{
				this.k_AssetPreviousVersion = this.k_AssetVersion;
				this.k_AssetVersion = 11;
			}
		}

		// Token: 0x060002F8 RID: 760 RVA: 0x00013CF7 File Offset: 0x00011EF7
		private float ValidateShadowBias(float value)
		{
			return Mathf.Max(0f, Mathf.Min(value, UniversalRenderPipeline.maxShadowBias));
		}

		// Token: 0x060002F9 RID: 761 RVA: 0x00013D0E File Offset: 0x00011F0E
		private int ValidatePerObjectLights(int value)
		{
			return Math.Max(0, Math.Min(value, UniversalRenderPipeline.maxPerObjectLights));
		}

		// Token: 0x060002FA RID: 762 RVA: 0x00013D21 File Offset: 0x00011F21
		private float ValidateRenderScale(float value)
		{
			return Mathf.Max(UniversalRenderPipeline.minRenderScale, Mathf.Min(value, UniversalRenderPipeline.maxRenderScale));
		}

		// Token: 0x060002FB RID: 763 RVA: 0x00013D38 File Offset: 0x00011F38
		internal bool ValidateRendererDataList(bool partial = false)
		{
			int num = 0;
			for (int i = 0; i < this.m_RendererDataList.Length; i++)
			{
				num += (this.ValidateRendererData(i) ? 0 : 1);
			}
			if (partial)
			{
				return num == 0;
			}
			return num != this.m_RendererDataList.Length;
		}

		// Token: 0x060002FC RID: 764 RVA: 0x00013D80 File Offset: 0x00011F80
		internal bool ValidateRendererData(int index)
		{
			if (index == -1)
			{
				index = this.m_DefaultRendererIndex;
			}
			return index < this.m_RendererDataList.Length && this.m_RendererDataList[index] != null;
		}

		// Token: 0x170000DC RID: 220
		// (get) Token: 0x060002FD RID: 765 RVA: 0x00013DA9 File Offset: 0x00011FA9
		// (set) Token: 0x060002FE RID: 766 RVA: 0x00013DB5 File Offset: 0x00011FB5
		[Obsolete("Use UniversalRenderPipelineGlobalSettings.instance.shaderVariantLogLevel", false)]
		public ShaderVariantLogLevel shaderVariantLogLevel
		{
			get
			{
				return (ShaderVariantLogLevel)UniversalRenderPipelineGlobalSettings.instance.shaderVariantLogLevel;
			}
			set
			{
				UniversalRenderPipelineGlobalSettings.instance.shaderVariantLogLevel = (ShaderVariantLogLevel)value;
			}
		}

		// Token: 0x170000DD RID: 221
		// (get) Token: 0x060002FF RID: 767 RVA: 0x00013DC4 File Offset: 0x00011FC4
		// (set) Token: 0x06000300 RID: 768 RVA: 0x00013E02 File Offset: 0x00012002
		[Obsolete("This is obsolete, please use shadowCascadeCount instead.", false)]
		public ShadowCascadesOption shadowCascadeOption
		{
			get
			{
				switch (this.shadowCascadeCount)
				{
				case 1:
					return ShadowCascadesOption.NoCascades;
				case 2:
					return ShadowCascadesOption.TwoCascades;
				case 4:
					return ShadowCascadesOption.FourCascades;
				}
				throw new InvalidOperationException("Cascade count is not compatible with obsolete API, please use shadowCascadeCount instead.");
			}
			set
			{
				switch (value)
				{
				case ShadowCascadesOption.NoCascades:
					this.shadowCascadeCount = 1;
					return;
				case ShadowCascadesOption.TwoCascades:
					this.shadowCascadeCount = 2;
					return;
				case ShadowCascadesOption.FourCascades:
					this.shadowCascadeCount = 4;
					return;
				default:
					throw new InvalidOperationException("Cascade count is not compatible with obsolete API, please use shadowCascadeCount instead.");
				}
			}
		}

		// Token: 0x04000206 RID: 518
		private Shader m_DefaultShader;

		// Token: 0x04000207 RID: 519
		private ScriptableRenderer[] m_Renderers = new ScriptableRenderer[1];

		// Token: 0x04000208 RID: 520
		[SerializeField]
		private int k_AssetVersion = 11;

		// Token: 0x04000209 RID: 521
		[SerializeField]
		private int k_AssetPreviousVersion = 11;

		// Token: 0x0400020A RID: 522
		[SerializeField]
		private RendererType m_RendererType = RendererType.UniversalRenderer;

		// Token: 0x0400020B RID: 523
		[EditorBrowsable(EditorBrowsableState.Never)]
		[Obsolete("Use m_RendererDataList instead.")]
		[SerializeField]
		internal ScriptableRendererData m_RendererData;

		// Token: 0x0400020C RID: 524
		[SerializeField]
		internal ScriptableRendererData[] m_RendererDataList = new ScriptableRendererData[1];

		// Token: 0x0400020D RID: 525
		[SerializeField]
		internal int m_DefaultRendererIndex;

		// Token: 0x0400020E RID: 526
		[SerializeField]
		private bool m_RequireDepthTexture;

		// Token: 0x0400020F RID: 527
		[SerializeField]
		private bool m_RequireOpaqueTexture;

		// Token: 0x04000210 RID: 528
		[SerializeField]
		private Downsampling m_OpaqueDownsampling = Downsampling._2xBilinear;

		// Token: 0x04000211 RID: 529
		[SerializeField]
		private bool m_SupportsTerrainHoles = true;

		// Token: 0x04000212 RID: 530
		[SerializeField]
		private bool m_SupportsHDR = true;

		// Token: 0x04000213 RID: 531
		[SerializeField]
		private HDRColorBufferPrecision m_HDRColorBufferPrecision;

		// Token: 0x04000214 RID: 532
		[SerializeField]
		private MsaaQuality m_MSAA = MsaaQuality.Disabled;

		// Token: 0x04000215 RID: 533
		[SerializeField]
		private float m_RenderScale = 1f;

		// Token: 0x04000216 RID: 534
		[SerializeField]
		private UpscalingFilterSelection m_UpscalingFilter;

		// Token: 0x04000217 RID: 535
		[SerializeField]
		private bool m_FsrOverrideSharpness;

		// Token: 0x04000218 RID: 536
		[SerializeField]
		private float m_FsrSharpness = 0.92f;

		// Token: 0x04000219 RID: 537
		[SerializeField]
		private bool m_EnableLODCrossFade = true;

		// Token: 0x0400021A RID: 538
		[SerializeField]
		private LODCrossFadeDitheringType m_LODCrossFadeDitheringType = LODCrossFadeDitheringType.BlueNoise;

		// Token: 0x0400021B RID: 539
		[SerializeField]
		private ShEvalMode m_ShEvalMode;

		// Token: 0x0400021C RID: 540
		[SerializeField]
		private LightRenderingMode m_MainLightRenderingMode = LightRenderingMode.PerPixel;

		// Token: 0x0400021D RID: 541
		[SerializeField]
		private bool m_MainLightShadowsSupported = true;

		// Token: 0x0400021E RID: 542
		[SerializeField]
		private ShadowResolution m_MainLightShadowmapResolution = ShadowResolution._2048;

		// Token: 0x0400021F RID: 543
		[SerializeField]
		private LightRenderingMode m_AdditionalLightsRenderingMode = LightRenderingMode.PerPixel;

		// Token: 0x04000220 RID: 544
		[SerializeField]
		private int m_AdditionalLightsPerObjectLimit = 4;

		// Token: 0x04000221 RID: 545
		[SerializeField]
		private bool m_AdditionalLightShadowsSupported;

		// Token: 0x04000222 RID: 546
		[SerializeField]
		private ShadowResolution m_AdditionalLightsShadowmapResolution = ShadowResolution._2048;

		// Token: 0x04000223 RID: 547
		[SerializeField]
		private int m_AdditionalLightsShadowResolutionTierLow = UniversalRenderPipelineAsset.AdditionalLightsDefaultShadowResolutionTierLow;

		// Token: 0x04000224 RID: 548
		[SerializeField]
		private int m_AdditionalLightsShadowResolutionTierMedium = UniversalRenderPipelineAsset.AdditionalLightsDefaultShadowResolutionTierMedium;

		// Token: 0x04000225 RID: 549
		[SerializeField]
		private int m_AdditionalLightsShadowResolutionTierHigh = UniversalRenderPipelineAsset.AdditionalLightsDefaultShadowResolutionTierHigh;

		// Token: 0x04000226 RID: 550
		[SerializeField]
		private bool m_ReflectionProbeBlending;

		// Token: 0x04000227 RID: 551
		[SerializeField]
		private bool m_ReflectionProbeBoxProjection;

		// Token: 0x04000228 RID: 552
		[SerializeField]
		private float m_ShadowDistance = 50f;

		// Token: 0x04000229 RID: 553
		[SerializeField]
		private int m_ShadowCascadeCount = 1;

		// Token: 0x0400022A RID: 554
		[SerializeField]
		private float m_Cascade2Split = 0.25f;

		// Token: 0x0400022B RID: 555
		[SerializeField]
		private Vector2 m_Cascade3Split = new Vector2(0.1f, 0.3f);

		// Token: 0x0400022C RID: 556
		[SerializeField]
		private Vector3 m_Cascade4Split = new Vector3(0.067f, 0.2f, 0.467f);

		// Token: 0x0400022D RID: 557
		[SerializeField]
		private float m_CascadeBorder = 0.2f;

		// Token: 0x0400022E RID: 558
		[SerializeField]
		private float m_ShadowDepthBias = 1f;

		// Token: 0x0400022F RID: 559
		[SerializeField]
		private float m_ShadowNormalBias = 1f;

		// Token: 0x04000230 RID: 560
		[SerializeField]
		private bool m_SoftShadowsSupported;

		// Token: 0x04000231 RID: 561
		[SerializeField]
		private bool m_ConservativeEnclosingSphere;

		// Token: 0x04000232 RID: 562
		[SerializeField]
		private int m_NumIterationsEnclosingSphere = 64;

		// Token: 0x04000233 RID: 563
		[SerializeField]
		private SoftShadowQuality m_SoftShadowQuality = SoftShadowQuality.Medium;

		// Token: 0x04000234 RID: 564
		[SerializeField]
		private LightCookieResolution m_AdditionalLightsCookieResolution = LightCookieResolution._2048;

		// Token: 0x04000235 RID: 565
		[SerializeField]
		private LightCookieFormat m_AdditionalLightsCookieFormat = LightCookieFormat.ColorHigh;

		// Token: 0x04000236 RID: 566
		[SerializeField]
		private bool m_UseSRPBatcher = true;

		// Token: 0x04000237 RID: 567
		[SerializeField]
		private bool m_SupportsDynamicBatching;

		// Token: 0x04000238 RID: 568
		[SerializeField]
		private bool m_MixedLightingSupported = true;

		// Token: 0x04000239 RID: 569
		[SerializeField]
		private bool m_SupportsLightCookies = true;

		// Token: 0x0400023A RID: 570
		[SerializeField]
		private bool m_SupportsLightLayers;

		// Token: 0x0400023B RID: 571
		[SerializeField]
		[Obsolete]
		private PipelineDebugLevel m_DebugLevel;

		// Token: 0x0400023C RID: 572
		[SerializeField]
		private StoreActionsOptimization m_StoreActionsOptimization;

		// Token: 0x0400023D RID: 573
		[SerializeField]
		private bool m_EnableRenderGraph;

		// Token: 0x0400023E RID: 574
		[SerializeField]
		private bool m_UseAdaptivePerformance = true;

		// Token: 0x0400023F RID: 575
		[SerializeField]
		private ColorGradingMode m_ColorGradingMode;

		// Token: 0x04000240 RID: 576
		[SerializeField]
		private int m_ColorGradingLutSize = 32;

		// Token: 0x04000241 RID: 577
		[SerializeField]
		private bool m_UseFastSRGBLinearConversion;

		// Token: 0x04000242 RID: 578
		[SerializeField]
		private bool m_SupportDataDrivenLensFlare = true;

		// Token: 0x04000243 RID: 579
		[SerializeField]
		private ShadowQuality m_ShadowType = ShadowQuality.HardShadows;

		// Token: 0x04000244 RID: 580
		[SerializeField]
		private bool m_LocalShadowsSupported;

		// Token: 0x04000245 RID: 581
		[SerializeField]
		private ShadowResolution m_LocalShadowsAtlasResolution = ShadowResolution._256;

		// Token: 0x04000246 RID: 582
		[SerializeField]
		private int m_MaxPixelLights;

		// Token: 0x04000247 RID: 583
		[SerializeField]
		private ShadowResolution m_ShadowAtlasResolution = ShadowResolution._256;

		// Token: 0x04000248 RID: 584
		[SerializeField]
		private VolumeFrameworkUpdateMode m_VolumeFrameworkUpdateMode;

		// Token: 0x04000249 RID: 585
		[SerializeField]
		private UniversalRenderPipelineAsset.TextureResources m_Textures;

		// Token: 0x0400024A RID: 586
		public const int k_MinLutSize = 16;

		// Token: 0x0400024B RID: 587
		public const int k_MaxLutSize = 65;

		// Token: 0x0400024C RID: 588
		internal const int k_ShadowCascadeMinCount = 1;

		// Token: 0x0400024D RID: 589
		internal const int k_ShadowCascadeMaxCount = 4;

		// Token: 0x0400024E RID: 590
		public static readonly int AdditionalLightsDefaultShadowResolutionTierLow = 256;

		// Token: 0x0400024F RID: 591
		public static readonly int AdditionalLightsDefaultShadowResolutionTierMedium = 512;

		// Token: 0x04000250 RID: 592
		public static readonly int AdditionalLightsDefaultShadowResolutionTierHigh = 1024;

		// Token: 0x04000251 RID: 593
		private static GraphicsFormat[][] s_LightCookieFormatList = new GraphicsFormat[][]
		{
			new GraphicsFormat[] { GraphicsFormat.R8_UNorm },
			new GraphicsFormat[] { GraphicsFormat.R16_UNorm },
			new GraphicsFormat[]
			{
				GraphicsFormat.R5G6B5_UNormPack16,
				GraphicsFormat.B5G6R5_UNormPack16,
				GraphicsFormat.R5G5B5A1_UNormPack16,
				GraphicsFormat.B5G5R5A1_UNormPack16
			},
			new GraphicsFormat[]
			{
				GraphicsFormat.A2B10G10R10_UNormPack32,
				GraphicsFormat.R8G8B8A8_SRGB,
				GraphicsFormat.B8G8R8A8_SRGB
			},
			new GraphicsFormat[] { GraphicsFormat.B10G11R11_UFloatPack32 }
		};

		// Token: 0x04000252 RID: 594
		[SerializeField]
		private int m_ShaderVariantLogLevel;

		// Token: 0x04000253 RID: 595
		[Obsolete("This is obsolete, please use shadowCascadeCount instead.", false)]
		[SerializeField]
		private ShadowCascadesOption m_ShadowCascades;

		// Token: 0x02000167 RID: 359
		[ReloadGroup]
		[Serializable]
		public sealed class TextureResources
		{
			// Token: 0x06000A67 RID: 2663 RVA: 0x00047306 File Offset: 0x00045506
			public bool NeedsReload()
			{
				return this.blueNoise64LTex == null || this.bayerMatrixTex == null;
			}

			// Token: 0x040009C5 RID: 2501
			[Reload("Textures/BlueNoise64/L/LDR_LLL1_0.png", ReloadAttribute.Package.Root)]
			public Texture2D blueNoise64LTex;

			// Token: 0x040009C6 RID: 2502
			[Reload("Textures/BayerMatrix.png", ReloadAttribute.Package.Root)]
			public Texture2D bayerMatrixTex;
		}
	}
}
