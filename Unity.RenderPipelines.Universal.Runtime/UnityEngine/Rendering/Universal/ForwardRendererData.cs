using System;

namespace UnityEngine.Rendering.Universal
{
	// Token: 0x02000084 RID: 132
	[Obsolete("ForwardRendererData has been deprecated (UnityUpgradable) -> UniversalRendererData", true)]
	[ReloadGroup]
	[ExcludeFromPreset]
	[Serializable]
	public class ForwardRendererData : ScriptableRendererData
	{
		// Token: 0x06000511 RID: 1297 RVA: 0x0001DD77 File Offset: 0x0001BF77
		protected override ScriptableRenderer Create()
		{
			Debug.LogWarning("Forward Renderer Data has been deprecated, " + base.name + " will be upgraded to a UniversalRendererData.");
			return null;
		}

		// Token: 0x17000169 RID: 361
		// (get) Token: 0x06000512 RID: 1298 RVA: 0x0001DD94 File Offset: 0x0001BF94
		// (set) Token: 0x06000513 RID: 1299 RVA: 0x0001DDA0 File Offset: 0x0001BFA0
		public LayerMask opaqueLayerMask
		{
			get
			{
				throw new NotSupportedException("ForwardRendererData has been deprecated. Use UniversalRendererData instead");
			}
			set
			{
				throw new NotSupportedException("ForwardRendererData has been deprecated. Use UniversalRendererData instead");
			}
		}

		// Token: 0x1700016A RID: 362
		// (get) Token: 0x06000514 RID: 1300 RVA: 0x0001DDAC File Offset: 0x0001BFAC
		// (set) Token: 0x06000515 RID: 1301 RVA: 0x0001DDB8 File Offset: 0x0001BFB8
		public LayerMask transparentLayerMask
		{
			get
			{
				throw new NotSupportedException("ForwardRendererData has been deprecated. Use UniversalRendererData instead");
			}
			set
			{
				throw new NotSupportedException("ForwardRendererData has been deprecated. Use UniversalRendererData instead");
			}
		}

		// Token: 0x1700016B RID: 363
		// (get) Token: 0x06000516 RID: 1302 RVA: 0x0001DDC4 File Offset: 0x0001BFC4
		// (set) Token: 0x06000517 RID: 1303 RVA: 0x0001DDD0 File Offset: 0x0001BFD0
		public StencilStateData defaultStencilState
		{
			get
			{
				throw new NotSupportedException("ForwardRendererData has been deprecated. Use UniversalRendererData instead");
			}
			set
			{
				throw new NotSupportedException("ForwardRendererData has been deprecated. Use UniversalRendererData instead");
			}
		}

		// Token: 0x1700016C RID: 364
		// (get) Token: 0x06000518 RID: 1304 RVA: 0x0001DDDC File Offset: 0x0001BFDC
		// (set) Token: 0x06000519 RID: 1305 RVA: 0x0001DDE8 File Offset: 0x0001BFE8
		public bool shadowTransparentReceive
		{
			get
			{
				throw new NotSupportedException("ForwardRendererData has been deprecated. Use UniversalRendererData instead");
			}
			set
			{
				throw new NotSupportedException("ForwardRendererData has been deprecated. Use UniversalRendererData instead");
			}
		}

		// Token: 0x1700016D RID: 365
		// (get) Token: 0x0600051A RID: 1306 RVA: 0x0001DDF4 File Offset: 0x0001BFF4
		// (set) Token: 0x0600051B RID: 1307 RVA: 0x0001DE00 File Offset: 0x0001C000
		public RenderingMode renderingMode
		{
			get
			{
				throw new NotSupportedException("ForwardRendererData has been deprecated. Use UniversalRendererData instead");
			}
			set
			{
				throw new NotSupportedException("ForwardRendererData has been deprecated. Use UniversalRendererData instead");
			}
		}

		// Token: 0x1700016E RID: 366
		// (get) Token: 0x0600051C RID: 1308 RVA: 0x0001DE0C File Offset: 0x0001C00C
		// (set) Token: 0x0600051D RID: 1309 RVA: 0x0001DE18 File Offset: 0x0001C018
		public bool accurateGbufferNormals
		{
			get
			{
				throw new NotSupportedException("ForwardRendererData has been deprecated. Use UniversalRendererData instead");
			}
			set
			{
				throw new NotSupportedException("ForwardRendererData has been deprecated. Use UniversalRendererData instead");
			}
		}

		// Token: 0x040003A6 RID: 934
		private const string k_ErrorMessage = "ForwardRendererData has been deprecated. Use UniversalRendererData instead";

		// Token: 0x040003A7 RID: 935
		public ForwardRendererData.ShaderResources shaders;

		// Token: 0x040003A8 RID: 936
		public PostProcessData postProcessData;

		// Token: 0x040003A9 RID: 937
		[Reload("Runtime/Data/XRSystemData.asset", ReloadAttribute.Package.Root)]
		public XRSystemData xrSystemData;

		// Token: 0x040003AA RID: 938
		[SerializeField]
		private LayerMask m_OpaqueLayerMask;

		// Token: 0x040003AB RID: 939
		[SerializeField]
		private LayerMask m_TransparentLayerMask;

		// Token: 0x040003AC RID: 940
		[SerializeField]
		private StencilStateData m_DefaultStencilState;

		// Token: 0x040003AD RID: 941
		[SerializeField]
		private bool m_ShadowTransparentReceive;

		// Token: 0x040003AE RID: 942
		[SerializeField]
		private RenderingMode m_RenderingMode;

		// Token: 0x040003AF RID: 943
		[SerializeField]
		private DepthPrimingMode m_DepthPrimingMode;

		// Token: 0x040003B0 RID: 944
		[SerializeField]
		private bool m_AccurateGbufferNormals;

		// Token: 0x040003B1 RID: 945
		[SerializeField]
		private bool m_ClusteredRendering;

		// Token: 0x040003B2 RID: 946
		[SerializeField]
		private TileSize m_TileSize;

		// Token: 0x0200018E RID: 398
		[ReloadGroup]
		[Serializable]
		public sealed class ShaderResources
		{
			// Token: 0x04000A70 RID: 2672
			[Reload("Shaders/Utils/Blit.shader", ReloadAttribute.Package.Root)]
			public Shader blitPS;

			// Token: 0x04000A71 RID: 2673
			[Reload("Shaders/Utils/CopyDepth.shader", ReloadAttribute.Package.Root)]
			public Shader copyDepthPS;

			// Token: 0x04000A72 RID: 2674
			[Obsolete("Obsolete, this feature will be supported by new 'ScreenSpaceShadows' renderer feature")]
			public Shader screenSpaceShadowPS;

			// Token: 0x04000A73 RID: 2675
			[Reload("Shaders/Utils/Sampling.shader", ReloadAttribute.Package.Root)]
			public Shader samplingPS;

			// Token: 0x04000A74 RID: 2676
			[Reload("Shaders/Utils/StencilDeferred.shader", ReloadAttribute.Package.Root)]
			public Shader stencilDeferredPS;

			// Token: 0x04000A75 RID: 2677
			[Reload("Shaders/Utils/FallbackError.shader", ReloadAttribute.Package.Root)]
			public Shader fallbackErrorPS;

			// Token: 0x04000A76 RID: 2678
			[Reload("Shaders/Utils/FallbackLoading.shader", ReloadAttribute.Package.Root)]
			public Shader fallbackLoadingPS;

			// Token: 0x04000A77 RID: 2679
			[Obsolete("Use fallbackErrorPS instead")]
			[Reload("Shaders/Utils/MaterialError.shader", ReloadAttribute.Package.Root)]
			public Shader materialErrorPS;

			// Token: 0x04000A78 RID: 2680
			[Reload("Shaders/Utils/CoreBlit.shader", ReloadAttribute.Package.Root)]
			[SerializeField]
			internal Shader coreBlitPS;

			// Token: 0x04000A79 RID: 2681
			[Reload("Shaders/Utils/CoreBlitColorAndDepth.shader", ReloadAttribute.Package.Root)]
			[SerializeField]
			internal Shader coreBlitColorAndDepthPS;

			// Token: 0x04000A7A RID: 2682
			[Reload("Shaders/CameraMotionVectors.shader", ReloadAttribute.Package.Root)]
			public Shader cameraMotionVector;

			// Token: 0x04000A7B RID: 2683
			[Reload("Shaders/ObjectMotionVectors.shader", ReloadAttribute.Package.Root)]
			public Shader objectMotionVector;
		}
	}
}
