using System;

namespace UnityEngine.Localization.Settings
{
	// Token: 0x020000A4 RID: 164
	[Serializable]
	public class CommandLineLocaleSelector : IStartupLocaleSelector
	{
		// Token: 0x1700010E RID: 270
		// (get) Token: 0x06000492 RID: 1170 RVA: 0x00010535 File Offset: 0x0000E735
		// (set) Token: 0x06000493 RID: 1171 RVA: 0x0001053D File Offset: 0x0000E73D
		public string CommandLineArgument
		{
			get
			{
				return this.m_CommandLineArgument;
			}
			set
			{
				this.m_CommandLineArgument = value;
			}
		}

		// Token: 0x06000494 RID: 1172 RVA: 0x00010548 File Offset: 0x0000E748
		public Locale GetStartupLocale(ILocalesProvider availableLocales)
		{
			if (string.IsNullOrEmpty(this.m_CommandLineArgument))
			{
				return null;
			}
			foreach (string text in Environment.GetCommandLineArgs())
			{
				if (text.StartsWith(this.m_CommandLineArgument, StringComparison.OrdinalIgnoreCase))
				{
					string text2 = text.Substring(this.m_CommandLineArgument.Length);
					Locale locale = availableLocales.GetLocale(text2);
					if (locale != null)
					{
						Debug.LogFormat("Found a matching locale({0}) for command line argument: `{1}`.", new object[] { text2, locale });
					}
					else
					{
						Debug.LogWarningFormat("Could not find a matching locale for command line argument: `{0}`", new object[] { text2 });
					}
					return locale;
				}
			}
			return null;
		}

		// Token: 0x04000157 RID: 343
		[SerializeField]
		private string m_CommandLineArgument = "-language=";
	}
}
