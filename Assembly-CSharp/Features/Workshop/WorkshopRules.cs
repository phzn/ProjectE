using System;

namespace Features.Workshop
{
	// Token: 0x02000B44 RID: 2884
	public static class WorkshopRules
	{
		// Token: 0x060051DF RID: 20959 RVA: 0x001FB54F File Offset: 0x001F974F
		public static int NormalizePage(int page)
		{
			return Math.Max(0, Math.Min(page, 42949671));
		}

		// Token: 0x060051E0 RID: 20960 RVA: 0x001FB562 File Offset: 0x001F9762
		public static string NormalizeSearch(string search)
		{
			search = (search ?? string.Empty).Trim();
			if (search.Length > 128)
			{
				return search.Substring(0, 128);
			}
			return search;
		}

		// Token: 0x060051E1 RID: 20961 RVA: 0x001FB590 File Offset: 0x001F9790
		public static string SafePreviewUrl(string value)
		{
			Uri uri;
			if (!Uri.TryCreate(value, UriKind.Absolute, out uri) || uri.Scheme != Uri.UriSchemeHttps || !string.IsNullOrEmpty(uri.UserInfo) || uri.IsLoopback || uri.HostNameType != UriHostNameType.Dns)
			{
				return string.Empty;
			}
			string text = uri.DnsSafeHost.ToLowerInvariant();
			if (!(text == "steamusercontent.com") && !text.EndsWith(".steamusercontent.com", StringComparison.Ordinal) && !(text == "steamuserimages-a.akamaihd.net") && !(text == "steamcommunity.com") && !text.EndsWith(".steamstatic.com", StringComparison.Ordinal) && !(text == "steamstatic.com"))
			{
				return string.Empty;
			}
			return uri.AbsoluteUri;
		}

		// Token: 0x060051E2 RID: 20962 RVA: 0x001FB64C File Offset: 0x001F984C
		public static string SteamLanguage(string locale)
		{
			uint num = <PrivateImplementationDetails>.ComputeStringHash(locale);
			if (num <= 1176137065U)
			{
				if (num <= 89862570U)
				{
					if (num != 5974475U)
					{
						if (num == 89862570U)
						{
							if (locale == "zh-Hans")
							{
								return "schinese";
							}
						}
					}
					else if (locale == "zh-Hant")
					{
						return "tchinese";
					}
				}
				else if (num != 1095059089U)
				{
					if (num != 1111292255U)
					{
						if (num == 1176137065U)
						{
							if (locale == "es")
							{
								return "spanish";
							}
						}
					}
					else if (locale == "ko")
					{
						return "koreana";
					}
				}
				else if (locale == "th")
				{
					return "thai";
				}
			}
			else if (num <= 1461901041U)
			{
				if (num != 1194886160U)
				{
					if (num != 1213488160U)
					{
						if (num == 1461901041U)
						{
							if (locale == "fr")
							{
								return "french";
							}
						}
					}
					else if (locale == "ru")
					{
						return "russian";
					}
				}
				else if (locale == "it")
				{
					return "italian";
				}
			}
			else if (num != 1664981344U)
			{
				if (num != 1816099348U)
				{
					if (num == 3436621126U)
					{
						if (locale == "es-419")
						{
							return "latam";
						}
					}
				}
				else if (locale == "ja")
				{
					return "japanese";
				}
			}
			else if (locale == "pt-BR")
			{
				return "brazilian";
			}
			return "english";
		}

		// Token: 0x04005283 RID: 21123
		public const int PageSize = 50;
	}
}
