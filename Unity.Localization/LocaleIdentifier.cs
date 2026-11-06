using System;
using System.Globalization;

namespace UnityEngine.Localization
{
	// Token: 0x02000005 RID: 5
	[Serializable]
	public struct LocaleIdentifier : IEquatable<LocaleIdentifier>, IComparable<LocaleIdentifier>
	{
		// Token: 0x17000005 RID: 5
		// (get) Token: 0x06000022 RID: 34 RVA: 0x000022B8 File Offset: 0x000004B8
		public string Code
		{
			get
			{
				return this.m_Code;
			}
		}

		// Token: 0x17000006 RID: 6
		// (get) Token: 0x06000023 RID: 35 RVA: 0x000022C0 File Offset: 0x000004C0
		public CultureInfo CultureInfo
		{
			get
			{
				if (this.m_CultureInfo == null && !string.IsNullOrEmpty(this.m_Code))
				{
					try
					{
						this.m_CultureInfo = CultureInfo.GetCultureInfo(this.m_Code);
					}
					catch (CultureNotFoundException)
					{
					}
				}
				return this.m_CultureInfo;
			}
		}

		// Token: 0x06000024 RID: 36 RVA: 0x00002310 File Offset: 0x00000510
		public LocaleIdentifier(string code)
		{
			this.m_Code = code;
			this.m_CultureInfo = null;
		}

		// Token: 0x06000025 RID: 37 RVA: 0x00002320 File Offset: 0x00000520
		public LocaleIdentifier(CultureInfo culture)
		{
			if (culture == null)
			{
				throw new ArgumentNullException("culture");
			}
			this.m_Code = culture.Name;
			this.m_CultureInfo = culture;
		}

		// Token: 0x06000026 RID: 38 RVA: 0x00002343 File Offset: 0x00000543
		public LocaleIdentifier(SystemLanguage systemLanguage)
		{
			this = new LocaleIdentifier(SystemLanguageConverter.GetSystemLanguageCultureCode(systemLanguage));
		}

		// Token: 0x06000027 RID: 39 RVA: 0x00002351 File Offset: 0x00000551
		public static implicit operator LocaleIdentifier(string code)
		{
			return new LocaleIdentifier(code);
		}

		// Token: 0x06000028 RID: 40 RVA: 0x00002359 File Offset: 0x00000559
		public static implicit operator LocaleIdentifier(CultureInfo culture)
		{
			return new LocaleIdentifier(culture);
		}

		// Token: 0x06000029 RID: 41 RVA: 0x00002361 File Offset: 0x00000561
		public static implicit operator LocaleIdentifier(SystemLanguage systemLanguage)
		{
			return new LocaleIdentifier(systemLanguage);
		}

		// Token: 0x0600002A RID: 42 RVA: 0x0000236C File Offset: 0x0000056C
		public override string ToString()
		{
			if (string.IsNullOrEmpty(this.m_Code))
			{
				return "undefined";
			}
			return ((this.CultureInfo != null) ? this.CultureInfo.EnglishName : "Custom") + "(" + this.Code + ")";
		}

		// Token: 0x0600002B RID: 43 RVA: 0x000023BC File Offset: 0x000005BC
		public override bool Equals(object obj)
		{
			if (obj == null)
			{
				return false;
			}
			if (obj is LocaleIdentifier)
			{
				LocaleIdentifier localeIdentifier = (LocaleIdentifier)obj;
				return this.Equals(localeIdentifier);
			}
			return false;
		}

		// Token: 0x0600002C RID: 44 RVA: 0x000023E6 File Offset: 0x000005E6
		public bool Equals(LocaleIdentifier other)
		{
			return (string.IsNullOrEmpty(other.Code) && string.IsNullOrEmpty(this.Code)) || string.Equals(this.Code, other.Code, StringComparison.OrdinalIgnoreCase);
		}

		// Token: 0x0600002D RID: 45 RVA: 0x00002418 File Offset: 0x00000618
		public override int GetHashCode()
		{
			if (string.IsNullOrEmpty(this.Code))
			{
				return base.GetHashCode();
			}
			return this.Code.GetHashCode();
		}

		// Token: 0x0600002E RID: 46 RVA: 0x00002443 File Offset: 0x00000643
		public int CompareTo(LocaleIdentifier other)
		{
			if (this.CultureInfo == null || other.CultureInfo == null)
			{
				return 1;
			}
			return string.CompareOrdinal(this.CultureInfo.EnglishName, other.CultureInfo.EnglishName);
		}

		// Token: 0x0600002F RID: 47 RVA: 0x00002474 File Offset: 0x00000674
		public static bool operator ==(LocaleIdentifier l1, LocaleIdentifier l2)
		{
			return l1.Equals(l2);
		}

		// Token: 0x06000030 RID: 48 RVA: 0x0000247E File Offset: 0x0000067E
		public static bool operator !=(LocaleIdentifier l1, LocaleIdentifier l2)
		{
			return !l1.Equals(l2);
		}

		// Token: 0x04000004 RID: 4
		[SerializeField]
		private string m_Code;

		// Token: 0x04000005 RID: 5
		private CultureInfo m_CultureInfo;
	}
}
