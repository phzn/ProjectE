using System;
using System.Globalization;

namespace UnityEngine.Localization.Settings
{
	// Token: 0x020000A7 RID: 167
	[Serializable]
	public class SystemLocaleSelector : IStartupLocaleSelector
	{
		// Token: 0x0600049F RID: 1183 RVA: 0x000106D0 File Offset: 0x0000E8D0
		public Locale GetStartupLocale(ILocalesProvider availableLocales)
		{
			Locale locale = SystemLocaleSelector.FindLocaleOrFallback(this.GetSystemCulture(), availableLocales);
			SystemLanguage applicationSystemLanguage = this.GetApplicationSystemLanguage();
			if (locale == null && applicationSystemLanguage != SystemLanguage.Unknown)
			{
				locale = SystemLocaleSelector.FindLocaleOrFallback(applicationSystemLanguage, availableLocales);
			}
			return locale;
		}

		// Token: 0x060004A0 RID: 1184 RVA: 0x00010714 File Offset: 0x0000E914
		private static Locale FindLocaleOrFallback(LocaleIdentifier localeIdentifier, ILocalesProvider availableLocales)
		{
			CultureInfo cultureInfo = localeIdentifier.CultureInfo;
			if (cultureInfo == null)
			{
				return null;
			}
			Locale locale = availableLocales.GetLocale(cultureInfo);
			if (locale == null)
			{
				cultureInfo = cultureInfo.Parent;
				while (cultureInfo != CultureInfo.InvariantCulture && locale == null)
				{
					locale = availableLocales.GetLocale(cultureInfo);
					cultureInfo = cultureInfo.Parent;
				}
				if (locale != null)
				{
					Debug.Log(string.Format("The Locale '{0}' is not available, however the parent locale '{1}' is available.", localeIdentifier, locale.Identifier));
				}
			}
			return locale;
		}

		// Token: 0x060004A1 RID: 1185 RVA: 0x0001079C File Offset: 0x0000E99C
		protected virtual CultureInfo GetSystemCulture()
		{
			return CultureInfo.CurrentUICulture;
		}

		// Token: 0x060004A2 RID: 1186 RVA: 0x000107A3 File Offset: 0x0000E9A3
		protected virtual SystemLanguage GetApplicationSystemLanguage()
		{
			return Application.systemLanguage;
		}
	}
}
