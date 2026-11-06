using System;
using System.Collections.Generic;
using UnityEngine.Localization.Metadata;

namespace UnityEngine.Localization.Platform.Android
{
	// Token: 0x020000B9 RID: 185
	[DisplayName("Android Round Icon Info", null)]
	[Metadata(AllowedTypes = MetadataType.LocalizationSettings, AllowMultiple = false, MenuItem = "Android/Round Icon")]
	[Serializable]
	public class RoundIconsInfo : IMetadata
	{
		// Token: 0x06000521 RID: 1313 RVA: 0x000122C0 File Offset: 0x000104C0
		internal void RefreshRoundIcons()
		{
			this.RoundIcons.Clear();
			this.RoundIcons.Add(this.m_Round_idpi);
			this.RoundIcons.Add(this.m_Round_mdpi);
			this.RoundIcons.Add(this.m_Round_hdpi);
			this.RoundIcons.Add(this.m_Round_xhdpi);
			this.RoundIcons.Add(this.m_Round_xxhdpi);
			this.RoundIcons.Add(this.m_Round_xxxhdpi);
		}

		// Token: 0x17000135 RID: 309
		// (get) Token: 0x06000522 RID: 1314 RVA: 0x0001233E File Offset: 0x0001053E
		// (set) Token: 0x06000523 RID: 1315 RVA: 0x00012346 File Offset: 0x00010546
		public LocalizedTexture RoundHdpi
		{
			get
			{
				return this.m_Round_hdpi;
			}
			set
			{
				this.m_Round_hdpi = value;
			}
		}

		// Token: 0x17000136 RID: 310
		// (get) Token: 0x06000524 RID: 1316 RVA: 0x0001234F File Offset: 0x0001054F
		// (set) Token: 0x06000525 RID: 1317 RVA: 0x00012357 File Offset: 0x00010557
		public LocalizedTexture RoundIdpi
		{
			get
			{
				return this.m_Round_idpi;
			}
			set
			{
				this.m_Round_idpi = value;
			}
		}

		// Token: 0x17000137 RID: 311
		// (get) Token: 0x06000526 RID: 1318 RVA: 0x00012360 File Offset: 0x00010560
		// (set) Token: 0x06000527 RID: 1319 RVA: 0x00012368 File Offset: 0x00010568
		public LocalizedTexture RoundMdpi
		{
			get
			{
				return this.m_Round_mdpi;
			}
			set
			{
				this.m_Round_mdpi = value;
			}
		}

		// Token: 0x17000138 RID: 312
		// (get) Token: 0x06000528 RID: 1320 RVA: 0x00012371 File Offset: 0x00010571
		// (set) Token: 0x06000529 RID: 1321 RVA: 0x00012379 File Offset: 0x00010579
		public LocalizedTexture RoundXhdpi
		{
			get
			{
				return this.m_Round_xhdpi;
			}
			set
			{
				this.m_Round_xhdpi = value;
			}
		}

		// Token: 0x17000139 RID: 313
		// (get) Token: 0x0600052A RID: 1322 RVA: 0x00012382 File Offset: 0x00010582
		// (set) Token: 0x0600052B RID: 1323 RVA: 0x0001238A File Offset: 0x0001058A
		public LocalizedTexture RoundXXHdpi
		{
			get
			{
				return this.m_Round_xxhdpi;
			}
			set
			{
				this.m_Round_xxhdpi = value;
			}
		}

		// Token: 0x1700013A RID: 314
		// (get) Token: 0x0600052C RID: 1324 RVA: 0x00012393 File Offset: 0x00010593
		// (set) Token: 0x0600052D RID: 1325 RVA: 0x0001239B File Offset: 0x0001059B
		public LocalizedTexture RoundXXXHdpi
		{
			get
			{
				return this.m_Round_xxxhdpi;
			}
			set
			{
				this.m_Round_xxxhdpi = value;
			}
		}

		// Token: 0x04000184 RID: 388
		[SerializeField]
		private LocalizedTexture m_Round_idpi;

		// Token: 0x04000185 RID: 389
		[SerializeField]
		private LocalizedTexture m_Round_mdpi;

		// Token: 0x04000186 RID: 390
		[SerializeField]
		private LocalizedTexture m_Round_hdpi;

		// Token: 0x04000187 RID: 391
		[SerializeField]
		private LocalizedTexture m_Round_xhdpi;

		// Token: 0x04000188 RID: 392
		[SerializeField]
		private LocalizedTexture m_Round_xxhdpi;

		// Token: 0x04000189 RID: 393
		[SerializeField]
		private LocalizedTexture m_Round_xxxhdpi;

		// Token: 0x0400018A RID: 394
		internal List<LocalizedTexture> RoundIcons = new List<LocalizedTexture>();
	}
}
