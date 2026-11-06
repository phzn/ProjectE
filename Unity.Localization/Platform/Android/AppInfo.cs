using System;
using UnityEngine.Localization.Metadata;

namespace UnityEngine.Localization.Platform.Android
{
	// Token: 0x020000B6 RID: 182
	[DisplayName("Android App Info", "Packages/com.unity.localization/Editor/Icons/Android/Android.png")]
	[Metadata(AllowedTypes = MetadataType.LocalizationSettings, AllowMultiple = false, MenuItem = "Android/App Info")]
	[Serializable]
	public class AppInfo : IMetadata
	{
		// Token: 0x1700012C RID: 300
		// (get) Token: 0x0600050B RID: 1291 RVA: 0x00012179 File Offset: 0x00010379
		// (set) Token: 0x0600050C RID: 1292 RVA: 0x00012181 File Offset: 0x00010381
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

		// Token: 0x0400017A RID: 378
		[Tooltip("The user-visible name for the bundle, used by Google Assistant and visible on the Android Home screen.\n")]
		[SerializeField]
		private LocalizedString m_DisplayName = new LocalizedString();
	}
}
