using System;
using System.Collections.Generic;

namespace UnityEngine.Localization.Settings
{
	// Token: 0x0200009E RID: 158
	public interface ILocalesProvider
	{
		// Token: 0x170000FB RID: 251
		// (get) Token: 0x06000448 RID: 1096
		List<Locale> Locales { get; }

		// Token: 0x06000449 RID: 1097
		Locale GetLocale(LocaleIdentifier id);

		// Token: 0x0600044A RID: 1098
		void AddLocale(Locale locale);

		// Token: 0x0600044B RID: 1099
		bool RemoveLocale(Locale locale);
	}
}
