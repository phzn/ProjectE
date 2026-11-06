using System;
using Features.CielVoice;
using TMPro;
using UnityEngine;

namespace ProjectExhibitionist.PoiTravel
{
	// Token: 0x02000945 RID: 2373
	[CreateAssetMenu(fileName = "PoiArrivalCatalog", menuName = "POI/Arrival Catalog")]
	public sealed class PoiArrivalCatalogSO : ScriptableObject
	{
		// Token: 0x17000AEB RID: 2795
		// (get) Token: 0x0600409B RID: 16539 RVA: 0x0017F969 File Offset: 0x0017DB69
		public TMP_FontAsset TitleFont
		{
			get
			{
				return this.titleFont;
			}
		}

		// Token: 0x17000AEC RID: 2796
		// (get) Token: 0x0600409C RID: 16540 RVA: 0x0017F971 File Offset: 0x0017DB71
		public TMP_FontAsset PortalFont
		{
			get
			{
				if (!(this.portalFont != null))
				{
					return this.titleFont;
				}
				return this.portalFont;
			}
		}

		// Token: 0x17000AED RID: 2797
		// (get) Token: 0x0600409D RID: 16541 RVA: 0x0017F98E File Offset: 0x0017DB8E
		public Shader PortalMarkerShader
		{
			get
			{
				return this.portalMarkerShader;
			}
		}

		// Token: 0x17000AEE RID: 2798
		// (get) Token: 0x0600409E RID: 16542 RVA: 0x0017F996 File Offset: 0x0017DB96
		public Sprite PortalPattern
		{
			get
			{
				return this.portalPattern;
			}
		}

		// Token: 0x0600409F RID: 16543 RVA: 0x0017F9A0 File Offset: 0x0017DBA0
		public bool TryGetZone(string sceneName, out PoiArrivalCatalogSO.Zone zone)
		{
			if (this.zones != null)
			{
				for (int i = 0; i < this.zones.Length; i++)
				{
					PoiArrivalCatalogSO.Zone zone2 = this.zones[i];
					if (zone2 != null && string.Equals(zone2.sceneName, sceneName, StringComparison.Ordinal))
					{
						zone = zone2;
						return true;
					}
				}
			}
			zone = null;
			return false;
		}

		// Token: 0x04004065 RID: 16485
		[SerializeField]
		private TMP_FontAsset titleFont;

		// Token: 0x04004066 RID: 16486
		[SerializeField]
		private TMP_FontAsset portalFont;

		// Token: 0x04004067 RID: 16487
		[SerializeField]
		private Shader portalMarkerShader;

		// Token: 0x04004068 RID: 16488
		[SerializeField]
		private Sprite portalPattern;

		// Token: 0x04004069 RID: 16489
		[SerializeField]
		private PoiArrivalCatalogSO.Zone[] zones;

		// Token: 0x02000946 RID: 2374
		[Serializable]
		public sealed class TrafficCue
		{
			// Token: 0x0400406A RID: 16490
			public AudioClip clip;

			// Token: 0x0400406B RID: 16491
			[Min(0f)]
			public float delay;

			// Token: 0x0400406C RID: 16492
			[Range(0f, 1f)]
			public float volume = 0.25f;

			// Token: 0x0400406D RID: 16493
			[Range(-1f, 1f)]
			public float startPan;

			// Token: 0x0400406E RID: 16494
			[Range(-1f, 1f)]
			public float endPan;

			// Token: 0x0400406F RID: 16495
			[Range(0.5f, 2f)]
			public float pitch = 1f;

			// Token: 0x04004070 RID: 16496
			public bool passingCar;
		}

		// Token: 0x02000947 RID: 2375
		[Serializable]
		public sealed class TrafficPreset
		{
			// Token: 0x17000AEF RID: 2799
			// (get) Token: 0x060040A2 RID: 16546 RVA: 0x0017FA0C File Offset: 0x0017DC0C
			public bool HasPlayableCue
			{
				get
				{
					if (this.cues == null)
					{
						return false;
					}
					foreach (PoiArrivalCatalogSO.TrafficCue trafficCue in this.cues)
					{
						if (trafficCue != null && trafficCue.clip != null)
						{
							return true;
						}
					}
					return false;
				}
			}

			// Token: 0x04004071 RID: 16497
			public string name;

			// Token: 0x04004072 RID: 16498
			public PoiArrivalCatalogSO.TrafficCue[] cues;
		}

		// Token: 0x02000948 RID: 2376
		[Serializable]
		public sealed class Zone
		{
			// Token: 0x04004073 RID: 16499
			public string sceneName;

			// Token: 0x04004074 RID: 16500
			public string displayName;

			// Token: 0x04004075 RID: 16501
			public CielVoiceSO[] voiceLines;

			// Token: 0x04004076 RID: 16502
			[Range(-180f, 180f)]
			public float cameraYawOffset;

			// Token: 0x04004077 RID: 16503
			public bool overrideGameplayCameraOrbit;

			// Token: 0x04004078 RID: 16504
			public Vector2 gameplayCameraOrbit;

			// Token: 0x04004079 RID: 16505
			public float gameplayCameraDistance;

			// Token: 0x0400407A RID: 16506
			public float cameraReturnDurationOverride;

			// Token: 0x0400407B RID: 16507
			public float titleHoldDurationOverride;

			// Token: 0x0400407C RID: 16508
			public AudioClip trafficPassClip;

			// Token: 0x0400407D RID: 16509
			public AudioClip trafficHornOneClip;

			// Token: 0x0400407E RID: 16510
			public AudioClip trafficHornTwoClip;

			// Token: 0x0400407F RID: 16511
			public PoiArrivalCatalogSO.TrafficPreset[] trafficPresets;
		}
	}
}
