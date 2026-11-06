using System;

namespace UnityEngine.Localization.Settings
{
	// Token: 0x020000A5 RID: 165
	[Serializable]
	public class PlayerPrefLocaleSelector : IStartupLocaleSelector, IInitialize
	{
		// Token: 0x1700010F RID: 271
		// (get) Token: 0x06000496 RID: 1174 RVA: 0x000105FB File Offset: 0x0000E7FB
		// (set) Token: 0x06000497 RID: 1175 RVA: 0x00010603 File Offset: 0x0000E803
		public string PlayerPreferenceKey
		{
			get
			{
				return this.m_PlayerPreferenceKey;
			}
			set
			{
				this.m_PlayerPreferenceKey = value;
			}
		}

		// Token: 0x06000498 RID: 1176 RVA: 0x0001060C File Offset: 0x0000E80C
		public void PostInitialization(LocalizationSettings settings)
		{
			if (PlaymodeState.IsPlayingOrWillChangePlaymode)
			{
				Locale selectedLocale = settings.GetSelectedLocale();
				if (selectedLocale != null)
				{
					PlayerPrefs.SetString(this.PlayerPreferenceKey, selectedLocale.Identifier.Code);
				}
			}
		}

		// Token: 0x06000499 RID: 1177 RVA: 0x0001064C File Offset: 0x0000E84C
		public Locale GetStartupLocale(ILocalesProvider availableLocales)
		{
			if (PlayerPrefs.HasKey(this.PlayerPreferenceKey))
			{
				string @string = PlayerPrefs.GetString(this.PlayerPreferenceKey);
				if (!string.IsNullOrEmpty(@string))
				{
					return availableLocales.GetLocale(@string);
				}
			}
			return null;
		}

		// Token: 0x04000158 RID: 344
		[SerializeField]
		private string m_PlayerPreferenceKey = "selected-locale";
	}
}
