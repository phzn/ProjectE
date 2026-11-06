using System;
using System.Collections.Generic;
using UnityEngine.Localization.Metadata;

namespace UnityEngine.Localization.Platform.Android
{
	// Token: 0x020000B8 RID: 184
	[DisplayName("Android Adaptive Icon Info", null)]
	[Metadata(AllowedTypes = MetadataType.LocalizationSettings, AllowMultiple = false, MenuItem = "Android/Adaptive Icon")]
	[Serializable]
	public class AdaptiveIconsInfo : IMetadata
	{
		// Token: 0x06000513 RID: 1299 RVA: 0x000121C8 File Offset: 0x000103C8
		internal void RefreshAdaptiveIcons()
		{
			this.AdaptiveIcons.Clear();
			this.AdaptiveIcons.Add(this.m_Adaptive_idpi);
			this.AdaptiveIcons.Add(this.m_Adaptive_mdpi);
			this.AdaptiveIcons.Add(this.m_Adaptive_hdpi);
			this.AdaptiveIcons.Add(this.m_Adaptive_xhdpi);
			this.AdaptiveIcons.Add(this.m_Adaptive_xxhdpi);
			this.AdaptiveIcons.Add(this.m_Adaptive_xxxhdpi);
		}

		// Token: 0x1700012F RID: 303
		// (get) Token: 0x06000514 RID: 1300 RVA: 0x00012246 File Offset: 0x00010446
		// (set) Token: 0x06000515 RID: 1301 RVA: 0x0001224E File Offset: 0x0001044E
		public AdaptiveIcon AdaptiveHdpi
		{
			get
			{
				return this.m_Adaptive_hdpi;
			}
			set
			{
				this.m_Adaptive_hdpi = value;
			}
		}

		// Token: 0x17000130 RID: 304
		// (get) Token: 0x06000516 RID: 1302 RVA: 0x00012257 File Offset: 0x00010457
		// (set) Token: 0x06000517 RID: 1303 RVA: 0x0001225F File Offset: 0x0001045F
		public AdaptiveIcon AdaptiveIdpi
		{
			get
			{
				return this.m_Adaptive_idpi;
			}
			set
			{
				this.m_Adaptive_idpi = value;
			}
		}

		// Token: 0x17000131 RID: 305
		// (get) Token: 0x06000518 RID: 1304 RVA: 0x00012268 File Offset: 0x00010468
		// (set) Token: 0x06000519 RID: 1305 RVA: 0x00012270 File Offset: 0x00010470
		public AdaptiveIcon AdaptiveMdpi
		{
			get
			{
				return this.m_Adaptive_mdpi;
			}
			set
			{
				this.m_Adaptive_mdpi = value;
			}
		}

		// Token: 0x17000132 RID: 306
		// (get) Token: 0x0600051A RID: 1306 RVA: 0x00012279 File Offset: 0x00010479
		// (set) Token: 0x0600051B RID: 1307 RVA: 0x00012281 File Offset: 0x00010481
		public AdaptiveIcon AdaptiveXhdpi
		{
			get
			{
				return this.m_Adaptive_xhdpi;
			}
			set
			{
				this.m_Adaptive_xhdpi = value;
			}
		}

		// Token: 0x17000133 RID: 307
		// (get) Token: 0x0600051C RID: 1308 RVA: 0x0001228A File Offset: 0x0001048A
		// (set) Token: 0x0600051D RID: 1309 RVA: 0x00012292 File Offset: 0x00010492
		public AdaptiveIcon AdaptiveXXHdpi
		{
			get
			{
				return this.m_Adaptive_xxhdpi;
			}
			set
			{
				this.m_Adaptive_xxhdpi = value;
			}
		}

		// Token: 0x17000134 RID: 308
		// (get) Token: 0x0600051E RID: 1310 RVA: 0x0001229B File Offset: 0x0001049B
		// (set) Token: 0x0600051F RID: 1311 RVA: 0x000122A3 File Offset: 0x000104A3
		public AdaptiveIcon AdaptiveXXXHdpi
		{
			get
			{
				return this.m_Adaptive_xxxhdpi;
			}
			set
			{
				this.m_Adaptive_xxxhdpi = value;
			}
		}

		// Token: 0x0400017D RID: 381
		[SerializeField]
		private AdaptiveIcon m_Adaptive_idpi;

		// Token: 0x0400017E RID: 382
		[SerializeField]
		private AdaptiveIcon m_Adaptive_mdpi;

		// Token: 0x0400017F RID: 383
		[SerializeField]
		private AdaptiveIcon m_Adaptive_hdpi;

		// Token: 0x04000180 RID: 384
		[SerializeField]
		private AdaptiveIcon m_Adaptive_xhdpi;

		// Token: 0x04000181 RID: 385
		[SerializeField]
		private AdaptiveIcon m_Adaptive_xxhdpi;

		// Token: 0x04000182 RID: 386
		[SerializeField]
		private AdaptiveIcon m_Adaptive_xxxhdpi;

		// Token: 0x04000183 RID: 387
		internal List<AdaptiveIcon> AdaptiveIcons = new List<AdaptiveIcon>();
	}
}
