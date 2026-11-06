using System;

namespace UnityEngine.Localization.Settings
{
	// Token: 0x020000A0 RID: 160
	public interface IStartupLocaleSelector
	{
		// Token: 0x0600044D RID: 1101
		Locale GetStartupLocale(ILocalesProvider availableLocales);
	}
}
