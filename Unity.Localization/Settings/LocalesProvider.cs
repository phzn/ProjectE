using System;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine.Localization.Pseudo;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace UnityEngine.Localization.Settings
{
	// Token: 0x020000A1 RID: 161
	[Serializable]
	public class LocalesProvider : ILocalesProvider, IPreloadRequired, IReset, IDisposable
	{
		// Token: 0x170000FC RID: 252
		// (get) Token: 0x0600044E RID: 1102 RVA: 0x0000F820 File Offset: 0x0000DA20
		public List<Locale> Locales
		{
			get
			{
				if (PlaymodeState.IsPlayingOrWillChangePlaymode && !this.PreloadOperation.IsDone)
				{
					this.PreloadOperation.WaitForCompletion();
				}
				return this.m_Locales;
			}
		}

		// Token: 0x170000FD RID: 253
		// (get) Token: 0x0600044F RID: 1103 RVA: 0x0000F85C File Offset: 0x0000DA5C
		public AsyncOperationHandle PreloadOperation
		{
			get
			{
				if (!this.m_LoadOperation.IsValid())
				{
					this.m_Locales.Clear();
					this.m_LoadOperation = AddressablesInterface.LoadAssetsWithLabel<Locale>("Locale", new Action<Locale>(this.AddLocale));
				}
				return this.m_LoadOperation;
			}
		}

		// Token: 0x06000450 RID: 1104 RVA: 0x0000F8AC File Offset: 0x0000DAAC
		public Locale GetLocale(LocaleIdentifier id)
		{
			foreach (Locale locale in this.Locales)
			{
				if (!(locale == null) && !(locale is PseudoLocale) && locale.Identifier.Equals(id))
				{
					return locale;
				}
			}
			return this.FindFallbackLocale(id);
		}

		// Token: 0x06000451 RID: 1105 RVA: 0x0000F928 File Offset: 0x0000DB28
		public Locale GetLocale(string code)
		{
			return this.GetLocale(new LocaleIdentifier(code));
		}

		// Token: 0x06000452 RID: 1106 RVA: 0x0000F936 File Offset: 0x0000DB36
		public Locale GetLocale(SystemLanguage systemLanguage)
		{
			return this.GetLocale(new LocaleIdentifier(systemLanguage));
		}

		// Token: 0x06000453 RID: 1107 RVA: 0x0000F944 File Offset: 0x0000DB44
		public void AddLocale(Locale locale)
		{
			if (locale == null)
			{
				return;
			}
			if (!(locale is PseudoLocale))
			{
				foreach (Locale locale2 in this.m_Locales)
				{
					if (!(locale2 is PseudoLocale) && locale2.Identifier == locale.Identifier)
					{
						Debug.LogWarning(string.Format("Ignoring locale {0}. The locale {1} has the same Id `{2}`", locale, locale2, locale.Identifier));
						return;
					}
				}
			}
			int num = this.m_Locales.BinarySearch(locale);
			if (num < 0)
			{
				this.m_Locales.Insert(~num, locale);
			}
		}

		// Token: 0x06000454 RID: 1108 RVA: 0x0000FA00 File Offset: 0x0000DC00
		public bool RemoveLocale(Locale locale)
		{
			if (locale == null)
			{
				return false;
			}
			bool flag = this.Locales.Remove(locale);
			LocalizationSettings instanceDontCreateDefault = LocalizationSettings.GetInstanceDontCreateDefault();
			if (instanceDontCreateDefault == null)
			{
				return flag;
			}
			instanceDontCreateDefault.OnLocaleRemoved(locale);
			return flag;
		}

		// Token: 0x06000455 RID: 1109 RVA: 0x0000FA2C File Offset: 0x0000DC2C
		public Locale FindFallbackLocale(LocaleIdentifier localeIdentifier)
		{
			CultureInfo cultureInfo = localeIdentifier.CultureInfo;
			if (cultureInfo == null)
			{
				return null;
			}
			Locale locale = null;
			cultureInfo = cultureInfo.Parent;
			while (cultureInfo != CultureInfo.InvariantCulture && locale == null)
			{
				locale = this.GetLocale(cultureInfo);
				cultureInfo = cultureInfo.Parent;
			}
			return locale;
		}

		// Token: 0x06000456 RID: 1110 RVA: 0x0000FA77 File Offset: 0x0000DC77
		public void ResetState()
		{
			this.m_Locales.Clear();
			this.m_LoadOperation = default(AsyncOperationHandle);
		}

		// Token: 0x06000457 RID: 1111 RVA: 0x0000FA90 File Offset: 0x0000DC90
		~LocalesProvider()
		{
			AddressablesInterface.SafeRelease(this.m_LoadOperation);
		}

		// Token: 0x06000458 RID: 1112 RVA: 0x0000FAC4 File Offset: 0x0000DCC4
		void IDisposable.Dispose()
		{
			this.m_Locales.Clear();
			AddressablesInterface.SafeRelease(this.m_LoadOperation);
			GC.SuppressFinalize(this);
		}

		// Token: 0x0400013D RID: 317
		private readonly List<Locale> m_Locales = new List<Locale>();

		// Token: 0x0400013E RID: 318
		private AsyncOperationHandle m_LoadOperation;
	}
}
