using System;
using System.Collections.Generic;
using UnityEngine.Localization.Metadata;

namespace UnityEngine.Localization.Platform.Android
{
	// Token: 0x020000BA RID: 186
	[DisplayName("Android Legacy Icon Info", null)]
	[Metadata(AllowedTypes = MetadataType.LocalizationSettings, AllowMultiple = false, MenuItem = "Android/Legacy Icon")]
	[Serializable]
	public class LegacyIconsInfo : IMetadata
	{
		// Token: 0x0600052F RID: 1327 RVA: 0x000123B8 File Offset: 0x000105B8
		internal void RefreshLegacyIcons()
		{
			this.LegacyIcons.Clear();
			this.LegacyIcons.Add(this.m_Legacy_idpi);
			this.LegacyIcons.Add(this.m_Legacy_mdpi);
			this.LegacyIcons.Add(this.m_Legacy_hdpi);
			this.LegacyIcons.Add(this.m_Legacy_xhdpi);
			this.LegacyIcons.Add(this.m_Legacy_xxhdpi);
			this.LegacyIcons.Add(this.m_Legacy_xxxhdpi);
		}

		// Token: 0x1700013B RID: 315
		// (get) Token: 0x06000530 RID: 1328 RVA: 0x00012436 File Offset: 0x00010636
		// (set) Token: 0x06000531 RID: 1329 RVA: 0x0001243E File Offset: 0x0001063E
		public LocalizedTexture LegacyHdpi
		{
			get
			{
				return this.m_Legacy_hdpi;
			}
			set
			{
				this.m_Legacy_hdpi = value;
			}
		}

		// Token: 0x1700013C RID: 316
		// (get) Token: 0x06000532 RID: 1330 RVA: 0x00012447 File Offset: 0x00010647
		// (set) Token: 0x06000533 RID: 1331 RVA: 0x0001244F File Offset: 0x0001064F
		public LocalizedTexture LegacyIdpi
		{
			get
			{
				return this.m_Legacy_idpi;
			}
			set
			{
				this.m_Legacy_idpi = value;
			}
		}

		// Token: 0x1700013D RID: 317
		// (get) Token: 0x06000534 RID: 1332 RVA: 0x00012458 File Offset: 0x00010658
		// (set) Token: 0x06000535 RID: 1333 RVA: 0x00012460 File Offset: 0x00010660
		public LocalizedTexture LegacyMdpi
		{
			get
			{
				return this.m_Legacy_mdpi;
			}
			set
			{
				this.m_Legacy_mdpi = value;
			}
		}

		// Token: 0x1700013E RID: 318
		// (get) Token: 0x06000536 RID: 1334 RVA: 0x00012469 File Offset: 0x00010669
		// (set) Token: 0x06000537 RID: 1335 RVA: 0x00012471 File Offset: 0x00010671
		public LocalizedTexture LegacyXhdpi
		{
			get
			{
				return this.m_Legacy_xhdpi;
			}
			set
			{
				this.m_Legacy_xhdpi = value;
			}
		}

		// Token: 0x1700013F RID: 319
		// (get) Token: 0x06000538 RID: 1336 RVA: 0x0001247A File Offset: 0x0001067A
		// (set) Token: 0x06000539 RID: 1337 RVA: 0x00012482 File Offset: 0x00010682
		public LocalizedTexture LegacyXXHdpi
		{
			get
			{
				return this.m_Legacy_xxhdpi;
			}
			set
			{
				this.m_Legacy_xxhdpi = value;
			}
		}

		// Token: 0x17000140 RID: 320
		// (get) Token: 0x0600053A RID: 1338 RVA: 0x0001248B File Offset: 0x0001068B
		// (set) Token: 0x0600053B RID: 1339 RVA: 0x00012493 File Offset: 0x00010693
		public LocalizedTexture LegacyXXXHdpi
		{
			get
			{
				return this.m_Legacy_xxxhdpi;
			}
			set
			{
				this.m_Legacy_xxxhdpi = value;
			}
		}

		// Token: 0x0400018B RID: 395
		[SerializeField]
		private LocalizedTexture m_Legacy_idpi;

		// Token: 0x0400018C RID: 396
		[SerializeField]
		private LocalizedTexture m_Legacy_mdpi;

		// Token: 0x0400018D RID: 397
		[SerializeField]
		private LocalizedTexture m_Legacy_hdpi;

		// Token: 0x0400018E RID: 398
		[SerializeField]
		private LocalizedTexture m_Legacy_xhdpi;

		// Token: 0x0400018F RID: 399
		[SerializeField]
		private LocalizedTexture m_Legacy_xxhdpi;

		// Token: 0x04000190 RID: 400
		[SerializeField]
		private LocalizedTexture m_Legacy_xxxhdpi;

		// Token: 0x04000191 RID: 401
		internal List<LocalizedTexture> LegacyIcons = new List<LocalizedTexture>();
	}
}
