using System;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine.Localization.Metadata;
using UnityEngine.Localization.Pseudo;
using UnityEngine.Localization.Settings;
using UnityEngine.Pool;

namespace UnityEngine.Localization
{
	// Token: 0x02000006 RID: 6
	public class Locale : ScriptableObject, IEquatable<Locale>, IComparable<Locale>, ISerializationCallbackReceiver, IFormatProvider
	{
		// Token: 0x17000007 RID: 7
		// (get) Token: 0x06000031 RID: 49 RVA: 0x0000248B File Offset: 0x0000068B
		// (set) Token: 0x06000032 RID: 50 RVA: 0x00002493 File Offset: 0x00000693
		public LocaleIdentifier Identifier
		{
			get
			{
				return this.m_Identifier;
			}
			set
			{
				this.m_Identifier = value;
			}
		}

		// Token: 0x17000008 RID: 8
		// (get) Token: 0x06000033 RID: 51 RVA: 0x0000249C File Offset: 0x0000069C
		// (set) Token: 0x06000034 RID: 52 RVA: 0x000024A4 File Offset: 0x000006A4
		public MetadataCollection Metadata
		{
			get
			{
				return this.m_Metadata;
			}
			set
			{
				this.m_Metadata = value;
			}
		}

		// Token: 0x17000009 RID: 9
		// (get) Token: 0x06000035 RID: 53 RVA: 0x000024AD File Offset: 0x000006AD
		// (set) Token: 0x06000036 RID: 54 RVA: 0x000024B5 File Offset: 0x000006B5
		public ushort SortOrder
		{
			get
			{
				return this.m_SortOrder;
			}
			set
			{
				this.m_SortOrder = value;
			}
		}

		// Token: 0x1700000A RID: 10
		// (get) Token: 0x06000037 RID: 55 RVA: 0x000024C0 File Offset: 0x000006C0
		// (set) Token: 0x06000038 RID: 56 RVA: 0x0000250B File Offset: 0x0000070B
		public string LocaleName
		{
			get
			{
				if (!string.IsNullOrEmpty(this.m_LocaleName))
				{
					return this.m_LocaleName;
				}
				if (this.Identifier.CultureInfo != null)
				{
					return this.Identifier.CultureInfo.EnglishName;
				}
				return base.name;
			}
			set
			{
				this.m_LocaleName = value;
			}
		}

		// Token: 0x06000039 RID: 57 RVA: 0x00002514 File Offset: 0x00000714
		[Obsolete("GetFallback is obsolete, please use GetFallbacks.")]
		public virtual Locale GetFallback()
		{
			return this.GetFallbacks().GetEnumerator().Current;
		}

		// Token: 0x0600003A RID: 58 RVA: 0x00002526 File Offset: 0x00000726
		public IEnumerable<Locale> GetFallbacks()
		{
			if (this.Metadata == null)
			{
				yield break;
			}
			HashSet<Locale> processedLocales;
			using (CollectionPool<HashSet<Locale>, Locale>.Get(out processedLocales))
			{
				IList<IMetadata> entries = this.Metadata.MetadataEntries;
				int num;
				for (int i = 0; i < entries.Count; i = num)
				{
					FallbackLocale fallbackLocale = entries[i] as FallbackLocale;
					if (fallbackLocale != null && fallbackLocale.Locale != null && !processedLocales.Contains(fallbackLocale.Locale))
					{
						processedLocales.Add(fallbackLocale.Locale);
						yield return fallbackLocale.Locale;
					}
					num = i + 1;
				}
				if (processedLocales.Count == 0)
				{
					Locale locale = null;
					CultureInfo cultureInfo = this.Identifier.CultureInfo;
					if (cultureInfo != null)
					{
						while (cultureInfo != CultureInfo.InvariantCulture && locale == null)
						{
							Locale locale2 = LocalizationSettings.AvailableLocales.GetLocale(cultureInfo);
							if (locale2 != this)
							{
								locale = locale2;
							}
							cultureInfo = cultureInfo.Parent;
						}
					}
					if (locale != null)
					{
						yield return locale;
					}
				}
				entries = null;
			}
			processedLocales = null;
			PooledObject<HashSet<Locale>> pooledObject = default(PooledObject<HashSet<Locale>>);
			yield break;
			yield break;
		}

		// Token: 0x1700000B RID: 11
		// (get) Token: 0x0600003B RID: 59 RVA: 0x00002536 File Offset: 0x00000736
		// (set) Token: 0x0600003C RID: 60 RVA: 0x0000253E File Offset: 0x0000073E
		public bool UseCustomFormatter
		{
			get
			{
				return this.m_UseCustomFormatter;
			}
			set
			{
				this.m_UseCustomFormatter = value;
				this.m_Formatter = null;
			}
		}

		// Token: 0x1700000C RID: 12
		// (get) Token: 0x0600003D RID: 61 RVA: 0x0000254E File Offset: 0x0000074E
		// (set) Token: 0x0600003E RID: 62 RVA: 0x00002556 File Offset: 0x00000756
		public string CustomFormatterCode
		{
			get
			{
				return this.m_CustomFormatCultureCode;
			}
			set
			{
				this.m_CustomFormatCultureCode = value;
				this.m_Formatter = null;
			}
		}

		// Token: 0x1700000D RID: 13
		// (get) Token: 0x0600003F RID: 63 RVA: 0x00002566 File Offset: 0x00000766
		// (set) Token: 0x06000040 RID: 64 RVA: 0x00002593 File Offset: 0x00000793
		public virtual IFormatProvider Formatter
		{
			get
			{
				if (this.m_Formatter == null)
				{
					this.m_Formatter = Locale.GetFormatter(this.UseCustomFormatter, this.Identifier, this.CustomFormatterCode);
				}
				return this.m_Formatter;
			}
			set
			{
				this.m_Formatter = value;
			}
		}

		// Token: 0x06000041 RID: 65 RVA: 0x0000259C File Offset: 0x0000079C
		internal static CultureInfo GetFormatter(bool useCustom, LocaleIdentifier localeIdentifier, string customCode)
		{
			CultureInfo cultureInfo = null;
			if (useCustom)
			{
				cultureInfo = (string.IsNullOrEmpty(customCode) ? CultureInfo.InvariantCulture : new LocaleIdentifier(customCode).CultureInfo);
			}
			if (cultureInfo == null)
			{
				cultureInfo = localeIdentifier.CultureInfo;
			}
			return cultureInfo;
		}

		// Token: 0x06000042 RID: 66 RVA: 0x000025D8 File Offset: 0x000007D8
		public static Locale CreateLocale(string code)
		{
			Locale locale = ScriptableObject.CreateInstance<Locale>();
			locale.m_Identifier = new LocaleIdentifier(code);
			if (locale.m_Identifier.CultureInfo != null)
			{
				locale.name = locale.m_Identifier.CultureInfo.EnglishName;
			}
			return locale;
		}

		// Token: 0x06000043 RID: 67 RVA: 0x0000261C File Offset: 0x0000081C
		public static Locale CreateLocale(LocaleIdentifier identifier)
		{
			Locale locale = ScriptableObject.CreateInstance<Locale>();
			locale.m_Identifier = identifier;
			if (locale.m_Identifier.CultureInfo != null)
			{
				locale.LocaleName = locale.m_Identifier.CultureInfo.EnglishName;
			}
			return locale;
		}

		// Token: 0x06000044 RID: 68 RVA: 0x0000265A File Offset: 0x0000085A
		public static Locale CreateLocale(SystemLanguage language)
		{
			return Locale.CreateLocale(new LocaleIdentifier(SystemLanguageConverter.GetSystemLanguageCultureCode(language)));
		}

		// Token: 0x06000045 RID: 69 RVA: 0x0000266C File Offset: 0x0000086C
		public static Locale CreateLocale(CultureInfo cultureInfo)
		{
			return Locale.CreateLocale(new LocaleIdentifier(cultureInfo));
		}

		// Token: 0x06000046 RID: 70 RVA: 0x0000267C File Offset: 0x0000087C
		public int CompareTo(Locale other)
		{
			if (other == null)
			{
				return -1;
			}
			if (this.SortOrder != other.SortOrder)
			{
				return this.SortOrder.CompareTo(other.SortOrder);
			}
			if (base.GetType() == other.GetType())
			{
				int num = string.CompareOrdinal(this.LocaleName, other.LocaleName);
				if (num == 0)
				{
					return InstanceIdHelper.GetInstanceId(this).CompareTo(InstanceIdHelper.GetInstanceId(other));
				}
				return num;
			}
			else
			{
				if (other is PseudoLocale)
				{
					return -1;
				}
				return 1;
			}
		}

		// Token: 0x06000047 RID: 71 RVA: 0x00002701 File Offset: 0x00000901
		public void OnAfterDeserialize()
		{
			this.m_Formatter = null;
		}

		// Token: 0x06000048 RID: 72 RVA: 0x0000270A File Offset: 0x0000090A
		public void OnBeforeSerialize()
		{
			if (string.IsNullOrEmpty(this.m_LocaleName))
			{
				this.m_LocaleName = base.name;
			}
		}

		// Token: 0x06000049 RID: 73 RVA: 0x00002725 File Offset: 0x00000925
		public override string ToString()
		{
			if (!string.IsNullOrEmpty(this.LocaleName))
			{
				return this.LocaleName;
			}
			return base.name;
		}

		// Token: 0x0600004A RID: 74 RVA: 0x00002744 File Offset: 0x00000944
		public bool Equals(Locale other)
		{
			return !(other == null) && this.LocaleName == other.LocaleName && this.Identifier.Equals(other.Identifier);
		}

		// Token: 0x0600004B RID: 75 RVA: 0x00002785 File Offset: 0x00000985
		object IFormatProvider.GetFormat(Type formatType)
		{
			IFormatProvider formatter = this.Formatter;
			if (formatter == null)
			{
				return null;
			}
			return formatter.GetFormat(formatType);
		}

		// Token: 0x04000006 RID: 6
		[SerializeField]
		private LocaleIdentifier m_Identifier;

		// Token: 0x04000007 RID: 7
		[SerializeField]
		[MetadataType(MetadataType.Locale)]
		private MetadataCollection m_Metadata = new MetadataCollection();

		// Token: 0x04000008 RID: 8
		[SerializeField]
		private string m_LocaleName;

		// Token: 0x04000009 RID: 9
		[SerializeField]
		private string m_CustomFormatCultureCode;

		// Token: 0x0400000A RID: 10
		[SerializeField]
		private bool m_UseCustomFormatter;

		// Token: 0x0400000B RID: 11
		[SerializeField]
		private ushort m_SortOrder = 10000;

		// Token: 0x0400000C RID: 12
		private IFormatProvider m_Formatter;
	}
}
