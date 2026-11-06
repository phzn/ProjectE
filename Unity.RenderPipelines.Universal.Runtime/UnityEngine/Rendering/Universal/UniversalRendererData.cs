using System;

namespace UnityEngine.Rendering.Universal
{
	// Token: 0x020000F2 RID: 242
	[ReloadGroup]
	[ExcludeFromPreset]
	[Serializable]
	public class UniversalRendererData : ScriptableRendererData, ISerializationCallbackReceiver
	{
		// Token: 0x06000799 RID: 1945 RVA: 0x00030603 File Offset: 0x0002E803
		protected override ScriptableRenderer Create()
		{
			if (!Application.isPlaying)
			{
				this.ReloadAllNullProperties();
			}
			return new UniversalRenderer(this);
		}

		// Token: 0x170001CE RID: 462
		// (get) Token: 0x0600079A RID: 1946 RVA: 0x00030618 File Offset: 0x0002E818
		// (set) Token: 0x0600079B RID: 1947 RVA: 0x00030620 File Offset: 0x0002E820
		public LayerMask opaqueLayerMask
		{
			get
			{
				return this.m_OpaqueLayerMask;
			}
			set
			{
				base.SetDirty();
				this.m_OpaqueLayerMask = value;
			}
		}

		// Token: 0x170001CF RID: 463
		// (get) Token: 0x0600079C RID: 1948 RVA: 0x0003062F File Offset: 0x0002E82F
		// (set) Token: 0x0600079D RID: 1949 RVA: 0x00030637 File Offset: 0x0002E837
		public LayerMask transparentLayerMask
		{
			get
			{
				return this.m_TransparentLayerMask;
			}
			set
			{
				base.SetDirty();
				this.m_TransparentLayerMask = value;
			}
		}

		// Token: 0x170001D0 RID: 464
		// (get) Token: 0x0600079E RID: 1950 RVA: 0x00030646 File Offset: 0x0002E846
		// (set) Token: 0x0600079F RID: 1951 RVA: 0x0003064E File Offset: 0x0002E84E
		public StencilStateData defaultStencilState
		{
			get
			{
				return this.m_DefaultStencilState;
			}
			set
			{
				base.SetDirty();
				this.m_DefaultStencilState = value;
			}
		}

		// Token: 0x170001D1 RID: 465
		// (get) Token: 0x060007A0 RID: 1952 RVA: 0x0003065D File Offset: 0x0002E85D
		// (set) Token: 0x060007A1 RID: 1953 RVA: 0x00030665 File Offset: 0x0002E865
		public bool shadowTransparentReceive
		{
			get
			{
				return this.m_ShadowTransparentReceive;
			}
			set
			{
				base.SetDirty();
				this.m_ShadowTransparentReceive = value;
			}
		}

		// Token: 0x170001D2 RID: 466
		// (get) Token: 0x060007A2 RID: 1954 RVA: 0x00030674 File Offset: 0x0002E874
		// (set) Token: 0x060007A3 RID: 1955 RVA: 0x0003067C File Offset: 0x0002E87C
		public RenderingMode renderingMode
		{
			get
			{
				return this.m_RenderingMode;
			}
			set
			{
				base.SetDirty();
				this.m_RenderingMode = value;
			}
		}

		// Token: 0x170001D3 RID: 467
		// (get) Token: 0x060007A4 RID: 1956 RVA: 0x0003068B File Offset: 0x0002E88B
		// (set) Token: 0x060007A5 RID: 1957 RVA: 0x00030693 File Offset: 0x0002E893
		public DepthPrimingMode depthPrimingMode
		{
			get
			{
				return this.m_DepthPrimingMode;
			}
			set
			{
				base.SetDirty();
				this.m_DepthPrimingMode = value;
			}
		}

		// Token: 0x170001D4 RID: 468
		// (get) Token: 0x060007A6 RID: 1958 RVA: 0x000306A2 File Offset: 0x0002E8A2
		// (set) Token: 0x060007A7 RID: 1959 RVA: 0x000306AA File Offset: 0x0002E8AA
		public CopyDepthMode copyDepthMode
		{
			get
			{
				return this.m_CopyDepthMode;
			}
			set
			{
				base.SetDirty();
				this.m_CopyDepthMode = value;
			}
		}

		// Token: 0x170001D5 RID: 469
		// (get) Token: 0x060007A8 RID: 1960 RVA: 0x000306B9 File Offset: 0x0002E8B9
		// (set) Token: 0x060007A9 RID: 1961 RVA: 0x000306C1 File Offset: 0x0002E8C1
		public bool accurateGbufferNormals
		{
			get
			{
				return this.m_AccurateGbufferNormals;
			}
			set
			{
				base.SetDirty();
				this.m_AccurateGbufferNormals = value;
			}
		}

		// Token: 0x170001D6 RID: 470
		// (get) Token: 0x060007AA RID: 1962 RVA: 0x000306D0 File Offset: 0x0002E8D0
		// (set) Token: 0x060007AB RID: 1963 RVA: 0x000306D8 File Offset: 0x0002E8D8
		public IntermediateTextureMode intermediateTextureMode
		{
			get
			{
				return this.m_IntermediateTextureMode;
			}
			set
			{
				base.SetDirty();
				this.m_IntermediateTextureMode = value;
			}
		}

		// Token: 0x060007AC RID: 1964 RVA: 0x000306E7 File Offset: 0x0002E8E7
		protected override void OnEnable()
		{
			base.OnEnable();
			if (this.shaders == null)
			{
				return;
			}
			this.ReloadAllNullProperties();
		}

		// Token: 0x060007AD RID: 1965 RVA: 0x000306FE File Offset: 0x0002E8FE
		private void ReloadAllNullProperties()
		{
		}

		// Token: 0x060007AE RID: 1966 RVA: 0x00030700 File Offset: 0x0002E900
		void ISerializationCallbackReceiver.OnBeforeSerialize()
		{
			this.m_AssetVersion = 2;
		}

		// Token: 0x060007AF RID: 1967 RVA: 0x00030709 File Offset: 0x0002E909
		void ISerializationCallbackReceiver.OnAfterDeserialize()
		{
			if (this.m_AssetVersion <= 1)
			{
				this.m_CopyDepthMode = CopyDepthMode.AfterOpaques;
			}
			this.m_AssetVersion = 2;
		}

		// Token: 0x04000646 RID: 1606
		public PostProcessData postProcessData;

		// Token: 0x04000647 RID: 1607
		[Reload("Runtime/Data/XRSystemData.asset", ReloadAttribute.Package.Root)]
		public XRSystemData xrSystemData;

		// Token: 0x04000648 RID: 1608
		public UniversalRendererData.ShaderResources shaders;

		// Token: 0x04000649 RID: 1609
		private const int k_LatestAssetVersion = 2;

		// Token: 0x0400064A RID: 1610
		[SerializeField]
		private int m_AssetVersion;

		// Token: 0x0400064B RID: 1611
		[SerializeField]
		private LayerMask m_OpaqueLayerMask = -1;

		// Token: 0x0400064C RID: 1612
		[SerializeField]
		private LayerMask m_TransparentLayerMask = -1;

		// Token: 0x0400064D RID: 1613
		[SerializeField]
		private StencilStateData m_DefaultStencilState = new StencilStateData
		{
			passOperation = StencilOp.Replace
		};

		// Token: 0x0400064E RID: 1614
		[SerializeField]
		private bool m_ShadowTransparentReceive = true;

		// Token: 0x0400064F RID: 1615
		[SerializeField]
		private RenderingMode m_RenderingMode;

		// Token: 0x04000650 RID: 1616
		[SerializeField]
		private DepthPrimingMode m_DepthPrimingMode;

		// Token: 0x04000651 RID: 1617
		[SerializeField]
		private CopyDepthMode m_CopyDepthMode = CopyDepthMode.AfterTransparents;

		// Token: 0x04000652 RID: 1618
		[SerializeField]
		private bool m_AccurateGbufferNormals;

		// Token: 0x04000653 RID: 1619
		[SerializeField]
		private IntermediateTextureMode m_IntermediateTextureMode = IntermediateTextureMode.Always;

		// Token: 0x020001C5 RID: 453
		[ReloadGroup]
		[Serializable]
		public sealed class ShaderResources
		{
			// Token: 0x04000BC5 RID: 3013
			[Reload("Shaders/Utils/Blit.shader", ReloadAttribute.Package.Root)]
			public Shader blitPS;

			// Token: 0x04000BC6 RID: 3014
			[Reload("Shaders/Utils/CopyDepth.shader", ReloadAttribute.Package.Root)]
			public Shader copyDepthPS;

			// Token: 0x04000BC7 RID: 3015
			[Obsolete("Obsolete, this feature will be supported by new 'ScreenSpaceShadows' renderer feature")]
			public Shader screenSpaceShadowPS;

			// Token: 0x04000BC8 RID: 3016
			[Reload("Shaders/Utils/Sampling.shader", ReloadAttribute.Package.Root)]
			public Shader samplingPS;

			// Token: 0x04000BC9 RID: 3017
			[Reload("Shaders/Utils/StencilDeferred.shader", ReloadAttribute.Package.Root)]
			public Shader stencilDeferredPS;

			// Token: 0x04000BCA RID: 3018
			[Reload("Shaders/Utils/FallbackError.shader", ReloadAttribute.Package.Root)]
			public Shader fallbackErrorPS;

			// Token: 0x04000BCB RID: 3019
			[Reload("Shaders/Utils/FallbackLoading.shader", ReloadAttribute.Package.Root)]
			public Shader fallbackLoadingPS;

			// Token: 0x04000BCC RID: 3020
			[Obsolete("Use fallbackErrorPS instead")]
			[Reload("Shaders/Utils/MaterialError.shader", ReloadAttribute.Package.Root)]
			public Shader materialErrorPS;

			// Token: 0x04000BCD RID: 3021
			[Reload("Shaders/Utils/CoreBlit.shader", ReloadAttribute.Package.Root)]
			[SerializeField]
			internal Shader coreBlitPS;

			// Token: 0x04000BCE RID: 3022
			[Reload("Shaders/Utils/CoreBlitColorAndDepth.shader", ReloadAttribute.Package.Root)]
			[SerializeField]
			internal Shader coreBlitColorAndDepthPS;

			// Token: 0x04000BCF RID: 3023
			[Reload("Shaders/Utils/BlitHDROverlay.shader", ReloadAttribute.Package.Root)]
			[SerializeField]
			internal Shader blitHDROverlay;

			// Token: 0x04000BD0 RID: 3024
			[Reload("Shaders/CameraMotionVectors.shader", ReloadAttribute.Package.Root)]
			public Shader cameraMotionVector;

			// Token: 0x04000BD1 RID: 3025
			[Reload("Shaders/ObjectMotionVectors.shader", ReloadAttribute.Package.Root)]
			public Shader objectMotionVector;

			// Token: 0x04000BD2 RID: 3026
			[Reload("Shaders/PostProcessing/LensFlareDataDriven.shader", ReloadAttribute.Package.Root)]
			public Shader dataDrivenLensFlare;

			// Token: 0x04000BD3 RID: 3027
			[Reload("Shaders/Terrain/TerrainDetailLit.shader", ReloadAttribute.Package.Root)]
			public Shader terrainDetailLitPS;

			// Token: 0x04000BD4 RID: 3028
			[Reload("Shaders/Terrain/WavingGrass.shader", ReloadAttribute.Package.Root)]
			public Shader terrainDetailGrassPS;

			// Token: 0x04000BD5 RID: 3029
			[Reload("Shaders/Terrain/WavingGrassBillboard.shader", ReloadAttribute.Package.Root)]
			public Shader terrainDetailGrassBillboardPS;
		}
	}
}
