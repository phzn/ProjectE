using System;

namespace UnityEngine.Localization.Settings
{
	// Token: 0x020000A6 RID: 166
	[Serializable]
	public class SpecificLocaleSelector : IStartupLocaleSelector
	{
		// Token: 0x17000110 RID: 272
		// (get) Token: 0x0600049B RID: 1179 RVA: 0x0001069B File Offset: 0x0000E89B
		// (set) Token: 0x0600049C RID: 1180 RVA: 0x000106A3 File Offset: 0x0000E8A3
		public LocaleIdentifier LocaleId
		{
			get
			{
				return this.m_LocaleId;
			}
			set
			{
				this.m_LocaleId = value;
			}
		}

		// Token: 0x0600049D RID: 1181 RVA: 0x000106AC File Offset: 0x0000E8AC
		public Locale GetStartupLocale(ILocalesProvider availableLocales)
		{
			return availableLocales.GetLocale(this.LocaleId);
		}

		// Token: 0x04000159 RID: 345
		[SerializeField]
		private LocaleIdentifier m_LocaleId = new LocaleIdentifier(SystemLanguage.English);
	}
}
