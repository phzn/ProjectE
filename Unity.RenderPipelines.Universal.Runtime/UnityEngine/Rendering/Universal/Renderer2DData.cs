using System;
using System.Collections.Generic;
using UnityEngine.Scripting.APIUpdating;
using UnityEngine.Serialization;

namespace UnityEngine.Rendering.Universal
{
	// Token: 0x02000037 RID: 55
	[ReloadGroup]
	[ExcludeFromPreset]
	[MovedFrom("UnityEngine.Experimental.Rendering.Universal")]
	[Serializable]
	public class Renderer2DData : ScriptableRendererData
	{
		// Token: 0x17000063 RID: 99
		// (get) Token: 0x060001FD RID: 509 RVA: 0x0001158D File Offset: 0x0000F78D
		public float hdrEmulationScale
		{
			get
			{
				return this.m_HDREmulationScale;
			}
		}

		// Token: 0x17000064 RID: 100
		// (get) Token: 0x060001FE RID: 510 RVA: 0x00011595 File Offset: 0x0000F795
		internal float lightRenderTextureScale
		{
			get
			{
				return this.m_LightRenderTextureScale;
			}
		}

		// Token: 0x17000065 RID: 101
		// (get) Token: 0x060001FF RID: 511 RVA: 0x0001159D File Offset: 0x0000F79D
		public Light2DBlendStyle[] lightBlendStyles
		{
			get
			{
				return this.m_LightBlendStyles;
			}
		}

		// Token: 0x17000066 RID: 102
		// (get) Token: 0x06000200 RID: 512 RVA: 0x000115A5 File Offset: 0x0000F7A5
		internal bool useDepthStencilBuffer
		{
			get
			{
				return this.m_UseDepthStencilBuffer;
			}
		}

		// Token: 0x17000067 RID: 103
		// (get) Token: 0x06000201 RID: 513 RVA: 0x000115AD File Offset: 0x0000F7AD
		internal Texture2D fallOffLookup
		{
			get
			{
				return this.m_FallOffLookup;
			}
		}

		// Token: 0x17000068 RID: 104
		// (get) Token: 0x06000202 RID: 514 RVA: 0x000115B5 File Offset: 0x0000F7B5
		internal Shader shapeLightShader
		{
			get
			{
				return this.m_ShapeLightShader;
			}
		}

		// Token: 0x17000069 RID: 105
		// (get) Token: 0x06000203 RID: 515 RVA: 0x000115BD File Offset: 0x0000F7BD
		internal Shader shapeLightVolumeShader
		{
			get
			{
				return this.m_ShapeLightVolumeShader;
			}
		}

		// Token: 0x1700006A RID: 106
		// (get) Token: 0x06000204 RID: 516 RVA: 0x000115C5 File Offset: 0x0000F7C5
		internal Shader pointLightShader
		{
			get
			{
				return this.m_PointLightShader;
			}
		}

		// Token: 0x1700006B RID: 107
		// (get) Token: 0x06000205 RID: 517 RVA: 0x000115CD File Offset: 0x0000F7CD
		internal Shader pointLightVolumeShader
		{
			get
			{
				return this.m_PointLightVolumeShader;
			}
		}

		// Token: 0x1700006C RID: 108
		// (get) Token: 0x06000206 RID: 518 RVA: 0x000115D5 File Offset: 0x0000F7D5
		internal Shader blitShader
		{
			get
			{
				return this.m_CoreBlitShader;
			}
		}

		// Token: 0x1700006D RID: 109
		// (get) Token: 0x06000207 RID: 519 RVA: 0x000115DD File Offset: 0x0000F7DD
		internal Shader blitHDROverlay
		{
			get
			{
				return this.m_BlitHDROverlay;
			}
		}

		// Token: 0x1700006E RID: 110
		// (get) Token: 0x06000208 RID: 520 RVA: 0x000115E5 File Offset: 0x0000F7E5
		internal Shader coreBlitPS
		{
			get
			{
				return this.m_CoreBlitShader;
			}
		}

		// Token: 0x1700006F RID: 111
		// (get) Token: 0x06000209 RID: 521 RVA: 0x000115ED File Offset: 0x0000F7ED
		internal Shader coreBlitColorAndDepthPS
		{
			get
			{
				return this.m_CoreBlitColorAndDepthPS;
			}
		}

		// Token: 0x17000070 RID: 112
		// (get) Token: 0x0600020A RID: 522 RVA: 0x000115F5 File Offset: 0x0000F7F5
		internal Shader samplingShader
		{
			get
			{
				return this.m_SamplingShader;
			}
		}

		// Token: 0x17000071 RID: 113
		// (get) Token: 0x0600020B RID: 523 RVA: 0x000115FD File Offset: 0x0000F7FD
		// (set) Token: 0x0600020C RID: 524 RVA: 0x00011605 File Offset: 0x0000F805
		internal PostProcessData postProcessData
		{
			get
			{
				return this.m_PostProcessData;
			}
			set
			{
				this.m_PostProcessData = value;
			}
		}

		// Token: 0x17000072 RID: 114
		// (get) Token: 0x0600020D RID: 525 RVA: 0x0001160E File Offset: 0x0000F80E
		internal Shader spriteShadowShader
		{
			get
			{
				return this.m_SpriteShadowShader;
			}
		}

		// Token: 0x17000073 RID: 115
		// (get) Token: 0x0600020E RID: 526 RVA: 0x00011616 File Offset: 0x0000F816
		internal Shader spriteUnshadowShader
		{
			get
			{
				return this.m_SpriteUnshadowShader;
			}
		}

		// Token: 0x17000074 RID: 116
		// (get) Token: 0x0600020F RID: 527 RVA: 0x0001161E File Offset: 0x0000F81E
		internal Shader geometryUnshadowShader
		{
			get
			{
				return this.m_GeometryUnshadowShader;
			}
		}

		// Token: 0x17000075 RID: 117
		// (get) Token: 0x06000210 RID: 528 RVA: 0x00011626 File Offset: 0x0000F826
		internal Shader projectedShadowShader
		{
			get
			{
				return this.m_ProjectedShadowShader;
			}
		}

		// Token: 0x17000076 RID: 118
		// (get) Token: 0x06000211 RID: 529 RVA: 0x0001162E File Offset: 0x0000F82E
		internal TransparencySortMode transparencySortMode
		{
			get
			{
				return this.m_TransparencySortMode;
			}
		}

		// Token: 0x17000077 RID: 119
		// (get) Token: 0x06000212 RID: 530 RVA: 0x00011636 File Offset: 0x0000F836
		internal Vector3 transparencySortAxis
		{
			get
			{
				return this.m_TransparencySortAxis;
			}
		}

		// Token: 0x17000078 RID: 120
		// (get) Token: 0x06000213 RID: 531 RVA: 0x0001163E File Offset: 0x0000F83E
		internal uint lightRenderTextureMemoryBudget
		{
			get
			{
				return this.m_MaxLightRenderTextureCount;
			}
		}

		// Token: 0x17000079 RID: 121
		// (get) Token: 0x06000214 RID: 532 RVA: 0x00011646 File Offset: 0x0000F846
		internal uint shadowRenderTextureMemoryBudget
		{
			get
			{
				return this.m_MaxShadowRenderTextureCount;
			}
		}

		// Token: 0x1700007A RID: 122
		// (get) Token: 0x06000215 RID: 533 RVA: 0x0001164E File Offset: 0x0000F84E
		internal bool useCameraSortingLayerTexture
		{
			get
			{
				return this.m_UseCameraSortingLayersTexture;
			}
		}

		// Token: 0x1700007B RID: 123
		// (get) Token: 0x06000216 RID: 534 RVA: 0x00011656 File Offset: 0x0000F856
		internal int cameraSortingLayerTextureBound
		{
			get
			{
				return this.m_CameraSortingLayersTextureBound;
			}
		}

		// Token: 0x1700007C RID: 124
		// (get) Token: 0x06000217 RID: 535 RVA: 0x0001165E File Offset: 0x0000F85E
		internal Downsampling cameraSortingLayerDownsamplingMethod
		{
			get
			{
				return this.m_CameraSortingLayerDownsamplingMethod;
			}
		}

		// Token: 0x1700007D RID: 125
		// (get) Token: 0x06000218 RID: 536 RVA: 0x00011666 File Offset: 0x0000F866
		internal LayerMask layerMask
		{
			get
			{
				return this.m_LayerMask;
			}
		}

		// Token: 0x06000219 RID: 537 RVA: 0x0001166E File Offset: 0x0000F86E
		protected override ScriptableRenderer Create()
		{
			return new Renderer2D(this);
		}

		// Token: 0x0600021A RID: 538 RVA: 0x00011678 File Offset: 0x0000F878
		internal void Dispose()
		{
			for (int i = 0; i < this.m_LightBlendStyles.Length; i++)
			{
				RTHandle renderTargetHandle = this.m_LightBlendStyles[i].renderTargetHandle;
				if (renderTargetHandle != null)
				{
					renderTargetHandle.Release();
				}
			}
			foreach (KeyValuePair<uint, Material> keyValuePair in this.lightMaterials)
			{
				CoreUtils.Destroy(keyValuePair.Value);
			}
			this.lightMaterials.Clear();
		}

		// Token: 0x0600021B RID: 539 RVA: 0x0001170C File Offset: 0x0000F90C
		protected override void OnEnable()
		{
			base.OnEnable();
			for (int i = 0; i < this.m_LightBlendStyles.Length; i++)
			{
				this.m_LightBlendStyles[i].renderTargetHandleId = Shader.PropertyToID(string.Format("_ShapeLightTexture{0}", i));
				this.m_LightBlendStyles[i].renderTargetHandle = RTHandles.Alloc(this.m_LightBlendStyles[i].renderTargetHandleId, string.Format("_ShapeLightTexture{0}", i));
			}
			this.normalsRenderTargetId = Shader.PropertyToID("_NormalMap");
			this.normalsRenderTarget = RTHandles.Alloc(this.normalsRenderTargetId, "_NormalMap");
			this.shadowsRenderTargetId = Shader.PropertyToID("_ShadowTex");
			this.shadowsRenderTarget = RTHandles.Alloc(this.shadowsRenderTargetId, "_ShadowTex");
			this.cameraSortingLayerRenderTargetId = Shader.PropertyToID("_CameraSortingLayerTexture");
			this.cameraSortingLayerRenderTarget = RTHandles.Alloc(this.cameraSortingLayerRenderTargetId, "_CameraSortingLayerTexture");
			this.spriteSelfShadowMaterial = null;
			this.spriteUnshadowMaterial = null;
			this.projectedShadowMaterial = null;
			this.stencilOnlyShadowMaterial = null;
		}

		// Token: 0x1700007E RID: 126
		// (get) Token: 0x0600021C RID: 540 RVA: 0x00011831 File Offset: 0x0000FA31
		internal Dictionary<uint, Material> lightMaterials { get; } = new Dictionary<uint, Material>();

		// Token: 0x1700007F RID: 127
		// (get) Token: 0x0600021D RID: 541 RVA: 0x00011839 File Offset: 0x0000FA39
		// (set) Token: 0x0600021E RID: 542 RVA: 0x00011841 File Offset: 0x0000FA41
		internal Material[] spriteSelfShadowMaterial { get; set; }

		// Token: 0x17000080 RID: 128
		// (get) Token: 0x0600021F RID: 543 RVA: 0x0001184A File Offset: 0x0000FA4A
		// (set) Token: 0x06000220 RID: 544 RVA: 0x00011852 File Offset: 0x0000FA52
		internal Material[] spriteUnshadowMaterial { get; set; }

		// Token: 0x17000081 RID: 129
		// (get) Token: 0x06000221 RID: 545 RVA: 0x0001185B File Offset: 0x0000FA5B
		// (set) Token: 0x06000222 RID: 546 RVA: 0x00011863 File Offset: 0x0000FA63
		internal Material[] geometryUnshadowMaterial { get; set; }

		// Token: 0x17000082 RID: 130
		// (get) Token: 0x06000223 RID: 547 RVA: 0x0001186C File Offset: 0x0000FA6C
		// (set) Token: 0x06000224 RID: 548 RVA: 0x00011874 File Offset: 0x0000FA74
		internal Material[] projectedShadowMaterial { get; set; }

		// Token: 0x17000083 RID: 131
		// (get) Token: 0x06000225 RID: 549 RVA: 0x0001187D File Offset: 0x0000FA7D
		// (set) Token: 0x06000226 RID: 550 RVA: 0x00011885 File Offset: 0x0000FA85
		internal Material[] stencilOnlyShadowMaterial { get; set; }

		// Token: 0x17000084 RID: 132
		// (get) Token: 0x06000227 RID: 551 RVA: 0x0001188E File Offset: 0x0000FA8E
		// (set) Token: 0x06000228 RID: 552 RVA: 0x00011896 File Offset: 0x0000FA96
		internal bool isNormalsRenderTargetValid { get; set; }

		// Token: 0x17000085 RID: 133
		// (get) Token: 0x06000229 RID: 553 RVA: 0x0001189F File Offset: 0x0000FA9F
		// (set) Token: 0x0600022A RID: 554 RVA: 0x000118A7 File Offset: 0x0000FAA7
		internal float normalsRenderTargetScale { get; set; }

		// Token: 0x17000086 RID: 134
		// (get) Token: 0x0600022B RID: 555 RVA: 0x000118B0 File Offset: 0x0000FAB0
		// (set) Token: 0x0600022C RID: 556 RVA: 0x000118B8 File Offset: 0x0000FAB8
		internal ILight2DCullResult lightCullResult { get; set; }

		// Token: 0x04000150 RID: 336
		[SerializeField]
		private LayerMask m_LayerMask = -1;

		// Token: 0x04000151 RID: 337
		[SerializeField]
		private TransparencySortMode m_TransparencySortMode;

		// Token: 0x04000152 RID: 338
		[SerializeField]
		private Vector3 m_TransparencySortAxis = Vector3.up;

		// Token: 0x04000153 RID: 339
		[SerializeField]
		private float m_HDREmulationScale = 1f;

		// Token: 0x04000154 RID: 340
		[SerializeField]
		[Range(0.01f, 1f)]
		private float m_LightRenderTextureScale = 0.5f;

		// Token: 0x04000155 RID: 341
		[SerializeField]
		[FormerlySerializedAs("m_LightOperations")]
		private Light2DBlendStyle[] m_LightBlendStyles;

		// Token: 0x04000156 RID: 342
		[SerializeField]
		private bool m_UseDepthStencilBuffer = true;

		// Token: 0x04000157 RID: 343
		[SerializeField]
		private bool m_UseCameraSortingLayersTexture;

		// Token: 0x04000158 RID: 344
		[SerializeField]
		private int m_CameraSortingLayersTextureBound;

		// Token: 0x04000159 RID: 345
		[SerializeField]
		private Downsampling m_CameraSortingLayerDownsamplingMethod;

		// Token: 0x0400015A RID: 346
		[SerializeField]
		private uint m_MaxLightRenderTextureCount = 16U;

		// Token: 0x0400015B RID: 347
		[SerializeField]
		private uint m_MaxShadowRenderTextureCount = 1U;

		// Token: 0x0400015C RID: 348
		[SerializeField]
		[Reload("Shaders/2D/Light2D-Shape.shader", ReloadAttribute.Package.Root)]
		private Shader m_ShapeLightShader;

		// Token: 0x0400015D RID: 349
		[SerializeField]
		[Reload("Shaders/2D/Light2D-Shape-Volumetric.shader", ReloadAttribute.Package.Root)]
		private Shader m_ShapeLightVolumeShader;

		// Token: 0x0400015E RID: 350
		[SerializeField]
		[Reload("Shaders/2D/Light2D-Point.shader", ReloadAttribute.Package.Root)]
		private Shader m_PointLightShader;

		// Token: 0x0400015F RID: 351
		[SerializeField]
		[Reload("Shaders/2D/Light2D-Point-Volumetric.shader", ReloadAttribute.Package.Root)]
		private Shader m_PointLightVolumeShader;

		// Token: 0x04000160 RID: 352
		[SerializeField]
		[Reload("Shaders/Utils/CoreBlit.shader", ReloadAttribute.Package.Root)]
		private Shader m_CoreBlitShader;

		// Token: 0x04000161 RID: 353
		[SerializeField]
		[Reload("Shaders/Utils/BlitHDROverlay.shader", ReloadAttribute.Package.Root)]
		private Shader m_BlitHDROverlay;

		// Token: 0x04000162 RID: 354
		[SerializeField]
		[Reload("Shaders/Utils/CoreBlitColorAndDepth.shader", ReloadAttribute.Package.Root)]
		private Shader m_CoreBlitColorAndDepthPS;

		// Token: 0x04000163 RID: 355
		[SerializeField]
		[Reload("Shaders/Utils/Sampling.shader", ReloadAttribute.Package.Root)]
		private Shader m_SamplingShader;

		// Token: 0x04000164 RID: 356
		[SerializeField]
		[Reload("Shaders/2D/Shadow2D-Projected.shader", ReloadAttribute.Package.Root)]
		private Shader m_ProjectedShadowShader;

		// Token: 0x04000165 RID: 357
		[SerializeField]
		[Reload("Shaders/2D/Shadow2D-Shadow-Sprite.shader", ReloadAttribute.Package.Root)]
		private Shader m_SpriteShadowShader;

		// Token: 0x04000166 RID: 358
		[SerializeField]
		[Reload("Shaders/2D/Shadow2D-Unshadow-Sprite.shader", ReloadAttribute.Package.Root)]
		private Shader m_SpriteUnshadowShader;

		// Token: 0x04000167 RID: 359
		[SerializeField]
		[Reload("Shaders/2D/Shadow2D-Unshadow-Geometry.shader", ReloadAttribute.Package.Root)]
		private Shader m_GeometryUnshadowShader;

		// Token: 0x04000168 RID: 360
		[SerializeField]
		[Reload("Shaders/Utils/FallbackError.shader", ReloadAttribute.Package.Root)]
		private Shader m_FallbackErrorShader;

		// Token: 0x04000169 RID: 361
		[SerializeField]
		private PostProcessData m_PostProcessData;

		// Token: 0x0400016A RID: 362
		[SerializeField]
		[Reload("Runtime/2D/Data/Textures/FalloffLookupTexture.png", ReloadAttribute.Package.Root)]
		[HideInInspector]
		private Texture2D m_FallOffLookup;

		// Token: 0x04000173 RID: 371
		internal RTHandle normalsRenderTarget;

		// Token: 0x04000174 RID: 372
		internal int normalsRenderTargetId;

		// Token: 0x04000175 RID: 373
		internal RTHandle shadowsRenderTarget;

		// Token: 0x04000176 RID: 374
		internal int shadowsRenderTargetId;

		// Token: 0x04000177 RID: 375
		internal RTHandle cameraSortingLayerRenderTarget;

		// Token: 0x04000178 RID: 376
		internal int cameraSortingLayerRenderTargetId;

		// Token: 0x02000161 RID: 353
		internal enum Renderer2DDefaultMaterialType
		{
			// Token: 0x040009A4 RID: 2468
			Lit,
			// Token: 0x040009A5 RID: 2469
			Unlit,
			// Token: 0x040009A6 RID: 2470
			Custom
		}
	}
}
