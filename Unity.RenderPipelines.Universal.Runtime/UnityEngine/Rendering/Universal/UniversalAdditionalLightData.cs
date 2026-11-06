using System;

namespace UnityEngine.Rendering.Universal
{
	// Token: 0x020000ED RID: 237
	[DisallowMultipleComponent]
	[RequireComponent(typeof(Light))]
	public class UniversalAdditionalLightData : MonoBehaviour, ISerializationCallbackReceiver, IAdditionalData
	{
		// Token: 0x170001B9 RID: 441
		// (get) Token: 0x06000753 RID: 1875 RVA: 0x0002D28E File Offset: 0x0002B48E
		internal int version
		{
			get
			{
				return this.m_Version;
			}
		}

		// Token: 0x170001BA RID: 442
		// (get) Token: 0x06000754 RID: 1876 RVA: 0x0002D296 File Offset: 0x0002B496
		// (set) Token: 0x06000755 RID: 1877 RVA: 0x0002D29E File Offset: 0x0002B49E
		public bool usePipelineSettings
		{
			get
			{
				return this.m_UsePipelineSettings;
			}
			set
			{
				this.m_UsePipelineSettings = value;
			}
		}

		// Token: 0x170001BB RID: 443
		// (get) Token: 0x06000756 RID: 1878 RVA: 0x0002D2A7 File Offset: 0x0002B4A7
		internal Light light
		{
			get
			{
				if (!this.m_Light)
				{
					base.TryGetComponent<Light>(out this.m_Light);
				}
				return this.m_Light;
			}
		}

		// Token: 0x170001BC RID: 444
		// (get) Token: 0x06000757 RID: 1879 RVA: 0x0002D2C9 File Offset: 0x0002B4C9
		public int additionalLightsShadowResolutionTier
		{
			get
			{
				return this.m_AdditionalLightsShadowResolutionTier;
			}
		}

		// Token: 0x170001BD RID: 445
		// (get) Token: 0x06000758 RID: 1880 RVA: 0x0002D2D1 File Offset: 0x0002B4D1
		// (set) Token: 0x06000759 RID: 1881 RVA: 0x0002D2D9 File Offset: 0x0002B4D9
		[Obsolete("This is obsolete, please use renderingLayerMask instead.", false)]
		public LightLayerEnum lightLayerMask
		{
			get
			{
				return this.m_LightLayerMask;
			}
			set
			{
				this.m_LightLayerMask = value;
			}
		}

		// Token: 0x170001BE RID: 446
		// (get) Token: 0x0600075A RID: 1882 RVA: 0x0002D2E2 File Offset: 0x0002B4E2
		// (set) Token: 0x0600075B RID: 1883 RVA: 0x0002D2EA File Offset: 0x0002B4EA
		public uint renderingLayers
		{
			get
			{
				return this.m_RenderingLayers;
			}
			set
			{
				if (this.m_RenderingLayers != value)
				{
					this.m_RenderingLayers = value;
					this.SyncLightAndShadowLayers();
				}
			}
		}

		// Token: 0x170001BF RID: 447
		// (get) Token: 0x0600075C RID: 1884 RVA: 0x0002D302 File Offset: 0x0002B502
		// (set) Token: 0x0600075D RID: 1885 RVA: 0x0002D30A File Offset: 0x0002B50A
		public bool customShadowLayers
		{
			get
			{
				return this.m_CustomShadowLayers;
			}
			set
			{
				if (this.m_CustomShadowLayers != value)
				{
					this.m_CustomShadowLayers = value;
					this.SyncLightAndShadowLayers();
				}
			}
		}

		// Token: 0x170001C0 RID: 448
		// (get) Token: 0x0600075E RID: 1886 RVA: 0x0002D322 File Offset: 0x0002B522
		// (set) Token: 0x0600075F RID: 1887 RVA: 0x0002D32A File Offset: 0x0002B52A
		[Obsolete("This is obsolete, please use shadowRenderingLayerMask instead.", false)]
		public LightLayerEnum shadowLayerMask
		{
			get
			{
				return this.m_ShadowLayerMask;
			}
			set
			{
				this.m_ShadowLayerMask = value;
			}
		}

		// Token: 0x170001C1 RID: 449
		// (get) Token: 0x06000760 RID: 1888 RVA: 0x0002D333 File Offset: 0x0002B533
		// (set) Token: 0x06000761 RID: 1889 RVA: 0x0002D33B File Offset: 0x0002B53B
		public uint shadowRenderingLayers
		{
			get
			{
				return this.m_ShadowRenderingLayers;
			}
			set
			{
				if (value != this.m_ShadowRenderingLayers)
				{
					this.m_ShadowRenderingLayers = value;
					this.SyncLightAndShadowLayers();
				}
			}
		}

		// Token: 0x170001C2 RID: 450
		// (get) Token: 0x06000762 RID: 1890 RVA: 0x0002D353 File Offset: 0x0002B553
		// (set) Token: 0x06000763 RID: 1891 RVA: 0x0002D35B File Offset: 0x0002B55B
		[Tooltip("Controls the size of the cookie mask currently assigned to the light.")]
		public Vector2 lightCookieSize
		{
			get
			{
				return this.m_LightCookieSize;
			}
			set
			{
				this.m_LightCookieSize = value;
			}
		}

		// Token: 0x170001C3 RID: 451
		// (get) Token: 0x06000764 RID: 1892 RVA: 0x0002D364 File Offset: 0x0002B564
		// (set) Token: 0x06000765 RID: 1893 RVA: 0x0002D36C File Offset: 0x0002B56C
		[Tooltip("Controls the offset of the cookie mask currently assigned to the light.")]
		public Vector2 lightCookieOffset
		{
			get
			{
				return this.m_LightCookieOffset;
			}
			set
			{
				this.m_LightCookieOffset = value;
			}
		}

		// Token: 0x170001C4 RID: 452
		// (get) Token: 0x06000766 RID: 1894 RVA: 0x0002D375 File Offset: 0x0002B575
		// (set) Token: 0x06000767 RID: 1895 RVA: 0x0002D37D File Offset: 0x0002B57D
		[Tooltip("Controls the filtering quality of soft shadows. Higher quality has lower performance.")]
		public SoftShadowQuality softShadowQuality
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

		// Token: 0x06000768 RID: 1896 RVA: 0x0002D386 File Offset: 0x0002B586
		public void OnBeforeSerialize()
		{
		}

		// Token: 0x06000769 RID: 1897 RVA: 0x0002D388 File Offset: 0x0002B588
		public void OnAfterDeserialize()
		{
			if (this.m_Version < 2)
			{
				this.m_RenderingLayers = (uint)this.m_LightLayerMask;
				this.m_ShadowRenderingLayers = (uint)this.m_ShadowLayerMask;
				this.m_Version = 2;
			}
			if (this.m_Version < 3)
			{
				this.m_SoftShadowQuality = (SoftShadowQuality)Math.Clamp((int)(this.m_SoftShadowQuality + 1), 0, 3);
				this.m_Version = 3;
			}
		}

		// Token: 0x0600076A RID: 1898 RVA: 0x0002D3E2 File Offset: 0x0002B5E2
		private void SyncLightAndShadowLayers()
		{
			if (this.light)
			{
				this.light.renderingLayerMask = (int)(this.m_CustomShadowLayers ? this.m_ShadowRenderingLayers : this.m_RenderingLayers);
			}
		}

		// Token: 0x040005E3 RID: 1507
		[SerializeField]
		private int m_Version = 3;

		// Token: 0x040005E4 RID: 1508
		[Tooltip("Controls if light Shadow Bias parameters use pipeline settings.")]
		[SerializeField]
		private bool m_UsePipelineSettings = true;

		// Token: 0x040005E5 RID: 1509
		public static readonly int AdditionalLightsShadowResolutionTierCustom = -1;

		// Token: 0x040005E6 RID: 1510
		public static readonly int AdditionalLightsShadowResolutionTierLow = 0;

		// Token: 0x040005E7 RID: 1511
		public static readonly int AdditionalLightsShadowResolutionTierMedium = 1;

		// Token: 0x040005E8 RID: 1512
		public static readonly int AdditionalLightsShadowResolutionTierHigh = 2;

		// Token: 0x040005E9 RID: 1513
		public static readonly int AdditionalLightsShadowDefaultResolutionTier = UniversalAdditionalLightData.AdditionalLightsShadowResolutionTierHigh;

		// Token: 0x040005EA RID: 1514
		public static readonly int AdditionalLightsShadowDefaultCustomResolution = 128;

		// Token: 0x040005EB RID: 1515
		[NonSerialized]
		private Light m_Light;

		// Token: 0x040005EC RID: 1516
		public static readonly int AdditionalLightsShadowMinimumResolution = 128;

		// Token: 0x040005ED RID: 1517
		[Tooltip("Controls if light shadow resolution uses pipeline settings.")]
		[SerializeField]
		private int m_AdditionalLightsShadowResolutionTier = UniversalAdditionalLightData.AdditionalLightsShadowDefaultResolutionTier;

		// Token: 0x040005EE RID: 1518
		[Obsolete("This is obsolete, please use m_RenderingLayerMask instead.", false)]
		[SerializeField]
		private LightLayerEnum m_LightLayerMask = LightLayerEnum.LightLayerDefault;

		// Token: 0x040005EF RID: 1519
		[SerializeField]
		private uint m_RenderingLayers = 1U;

		// Token: 0x040005F0 RID: 1520
		[SerializeField]
		private bool m_CustomShadowLayers;

		// Token: 0x040005F1 RID: 1521
		[SerializeField]
		private LightLayerEnum m_ShadowLayerMask = LightLayerEnum.LightLayerDefault;

		// Token: 0x040005F2 RID: 1522
		[SerializeField]
		private uint m_ShadowRenderingLayers = 1U;

		// Token: 0x040005F3 RID: 1523
		[SerializeField]
		private Vector2 m_LightCookieSize = Vector2.one;

		// Token: 0x040005F4 RID: 1524
		[SerializeField]
		private Vector2 m_LightCookieOffset = Vector2.zero;

		// Token: 0x040005F5 RID: 1525
		[SerializeField]
		private SoftShadowQuality m_SoftShadowQuality;
	}
}
