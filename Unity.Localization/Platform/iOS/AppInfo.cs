using System;
using UnityEngine.Localization.Metadata;

namespace UnityEngine.Localization.Platform.iOS
{
	// Token: 0x020000B5 RID: 181
	[DisplayName("Apple App Info", null)]
	[Metadata(AllowedTypes = MetadataType.LocalizationSettings, AllowMultiple = false, MenuItem = "Apple/App Info")]
	[Serializable]
	public class AppInfo : IMetadata
	{
		// Token: 0x17000126 RID: 294
		// (get) Token: 0x060004FE RID: 1278 RVA: 0x000120BB File Offset: 0x000102BB
		// (set) Token: 0x060004FF RID: 1279 RVA: 0x000120C3 File Offset: 0x000102C3
		public LocalizedString ShortName
		{
			get
			{
				return this.m_ShortName;
			}
			set
			{
				this.m_ShortName = value;
			}
		}

		// Token: 0x17000127 RID: 295
		// (get) Token: 0x06000500 RID: 1280 RVA: 0x000120CC File Offset: 0x000102CC
		// (set) Token: 0x06000501 RID: 1281 RVA: 0x000120D4 File Offset: 0x000102D4
		public LocalizedString DisplayName
		{
			get
			{
				return this.m_DisplayName;
			}
			set
			{
				this.m_DisplayName = value;
			}
		}

		// Token: 0x17000128 RID: 296
		// (get) Token: 0x06000502 RID: 1282 RVA: 0x000120DD File Offset: 0x000102DD
		// (set) Token: 0x06000503 RID: 1283 RVA: 0x000120E5 File Offset: 0x000102E5
		public LocalizedString CameraUsageDescription
		{
			get
			{
				return this.m_CameraUsageDescription;
			}
			set
			{
				this.m_CameraUsageDescription = value;
			}
		}

		// Token: 0x17000129 RID: 297
		// (get) Token: 0x06000504 RID: 1284 RVA: 0x000120EE File Offset: 0x000102EE
		// (set) Token: 0x06000505 RID: 1285 RVA: 0x000120F6 File Offset: 0x000102F6
		public LocalizedString MicrophoneUsageDescription
		{
			get
			{
				return this.m_MicrophoneUsageDescription;
			}
			set
			{
				this.m_MicrophoneUsageDescription = value;
			}
		}

		// Token: 0x1700012A RID: 298
		// (get) Token: 0x06000506 RID: 1286 RVA: 0x000120FF File Offset: 0x000102FF
		// (set) Token: 0x06000507 RID: 1287 RVA: 0x00012107 File Offset: 0x00010307
		public LocalizedString LocationUsageDescription
		{
			get
			{
				return this.m_LocationUsageDescription;
			}
			set
			{
				this.m_LocationUsageDescription = value;
			}
		}

		// Token: 0x1700012B RID: 299
		// (get) Token: 0x06000508 RID: 1288 RVA: 0x00012110 File Offset: 0x00010310
		// (set) Token: 0x06000509 RID: 1289 RVA: 0x00012118 File Offset: 0x00010318
		public LocalizedString UserTrackingUsageDescription
		{
			get
			{
				return this.m_UserTrackingUsageDescription;
			}
			set
			{
				this.m_UserTrackingUsageDescription = value;
			}
		}

		// Token: 0x04000174 RID: 372
		[Tooltip("The user-visible name for the bundle, used by Siri, visible on the iOS Home screen and Mac app menu.\nThis name can contain up to 15 characters.\nCFBundleName field in xcode projects info.plist file.")]
		[SerializeField]
		private LocalizedString m_ShortName = new LocalizedString();

		// Token: 0x04000175 RID: 373
		[Tooltip("The user-visible name for the bundle, used by Siri visible on the iOS Home screen and Mac app menu.\nUse this key if you want a product name that's longer than Bundle Name.\nCFBundleDisplayName field in xcode projects info.plist file.")]
		[SerializeField]
		private LocalizedString m_DisplayName = new LocalizedString();

		// Token: 0x04000176 RID: 374
		[Tooltip("A message that tells the user why the app is requesting access to the device’s camera.\nNSCameraUsageDescription field in xcode projects info.plist file.")]
		[SerializeField]
		private LocalizedString m_CameraUsageDescription = new LocalizedString();

		// Token: 0x04000177 RID: 375
		[Tooltip("A message that tells the user why the app is requesting access to the device’s microphone.\nNSMicrophoneUsageDescription field in xcode projects info.plist file.")]
		[SerializeField]
		private LocalizedString m_MicrophoneUsageDescription = new LocalizedString();

		// Token: 0x04000178 RID: 376
		[Tooltip("A message that tells the user why the app is requesting access to the user’s location information while the app is running in the foreground.\nNSLocationWhenInUseUsageDescription field in xcode projects info.plist file.")]
		[SerializeField]
		private LocalizedString m_LocationUsageDescription = new LocalizedString();

		// Token: 0x04000179 RID: 377
		[Tooltip("A message that informs the user why an app is requesting permission to use data for tracking the user or the device.\nNSUserTrackingUsageDescription field in xcode projects info.plist file.")]
		[SerializeField]
		private LocalizedString m_UserTrackingUsageDescription = new LocalizedString();
	}
}
