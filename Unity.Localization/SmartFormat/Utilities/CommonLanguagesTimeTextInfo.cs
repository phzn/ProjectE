using System;

namespace UnityEngine.Localization.SmartFormat.Utilities
{
	// Token: 0x02000049 RID: 73
	public static class CommonLanguagesTimeTextInfo
	{
		// Token: 0x17000084 RID: 132
		// (get) Token: 0x0600028B RID: 651 RVA: 0x00009EB8 File Offset: 0x000080B8
		public static TimeTextInfo English
		{
			get
			{
				return new TimeTextInfo(PluralRules.GetPluralRule("en"), new string[] { "{0} week", "{0} weeks" }, new string[] { "{0} day", "{0} days" }, new string[] { "{0} hour", "{0} hours" }, new string[] { "{0} minute", "{0} minutes" }, new string[] { "{0} second", "{0} seconds" }, new string[] { "{0} millisecond", "{0} milliseconds" }, new string[] { "{0}w" }, new string[] { "{0}d" }, new string[] { "{0}h" }, new string[] { "{0}m" }, new string[] { "{0}s" }, new string[] { "{0}ms" }, "less than {0}");
			}
		}

		// Token: 0x0600028C RID: 652 RVA: 0x00009FB1 File Offset: 0x000081B1
		public static TimeTextInfo GetTimeTextInfo(string twoLetterIsoLanguageName)
		{
			if (twoLetterIsoLanguageName == "en")
			{
				return CommonLanguagesTimeTextInfo.English;
			}
			return null;
		}
	}
}
