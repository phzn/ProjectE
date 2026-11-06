using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.Localization.Metadata;
using UnityEngine.Localization.Pseudo;
using UnityEngine.Localization.Settings;
using UnityEngine.Localization.SmartFormat;
using UnityEngine.Localization.SmartFormat.Core.Formatting;

namespace UnityEngine.Localization.Tables
{
	// Token: 0x02000031 RID: 49
	public class StringTableEntry : TableEntry
	{
		// Token: 0x17000056 RID: 86
		// (get) Token: 0x060001B3 RID: 435 RVA: 0x00006681 File Offset: 0x00004881
		// (set) Token: 0x060001B4 RID: 436 RVA: 0x00006689 File Offset: 0x00004889
		public FormatCache FormatCache
		{
			get
			{
				return this.m_FormatCache;
			}
			set
			{
				this.m_FormatCache = value;
			}
		}

		// Token: 0x17000057 RID: 87
		// (get) Token: 0x060001B5 RID: 437 RVA: 0x00006692 File Offset: 0x00004892
		// (set) Token: 0x060001B6 RID: 438 RVA: 0x0000669F File Offset: 0x0000489F
		public string Value
		{
			get
			{
				return base.Data.Localized;
			}
			set
			{
				base.Data.Localized = value;
				if (this.m_FormatCache != null)
				{
					FormatCachePool.Release(this.m_FormatCache);
					this.m_FormatCache = null;
				}
			}
		}

		// Token: 0x17000058 RID: 88
		// (get) Token: 0x060001B7 RID: 439 RVA: 0x000066C7 File Offset: 0x000048C7
		// (set) Token: 0x060001B8 RID: 440 RVA: 0x000066E6 File Offset: 0x000048E6
		public bool IsSmart
		{
			get
			{
				return base.HasTagMetadata<SmartFormatTag>() || base.Data.Metadata.GetMetadata<SmartFormatTag>() != null;
			}
			set
			{
				if (value)
				{
					if (this.m_FormatCache != null)
					{
						FormatCachePool.Release(this.m_FormatCache);
						this.m_FormatCache = null;
					}
					base.AddTagMetadata<SmartFormatTag>();
					return;
				}
				base.RemoveTagMetadata<SmartFormatTag>();
			}
		}

		// Token: 0x060001B9 RID: 441 RVA: 0x00006712 File Offset: 0x00004912
		internal StringTableEntry()
		{
		}

		// Token: 0x060001BA RID: 442 RVA: 0x0000671C File Offset: 0x0000491C
		public void RemoveFromTable()
		{
			StringTable stringTable = base.Table as StringTable;
			if (stringTable == null)
			{
				Debug.LogWarning(string.Format("Failed to remove {0} with id {1} and value `{2}` as it does not belong to a table.", "StringTableEntry", base.KeyId, this.Value));
				return;
			}
			stringTable.Remove(base.KeyId);
		}

		// Token: 0x060001BB RID: 443 RVA: 0x00006774 File Offset: 0x00004974
		internal FormatCache GetOrCreateFormatCache()
		{
			if (!this.IsSmart)
			{
				return null;
			}
			if (this.m_FormatCache == null && !string.IsNullOrEmpty(base.Data.Localized))
			{
				this.m_FormatCache = FormatCachePool.Get(LocalizationSettings.StringDatabase.SmartFormatter.Parser.ParseFormat(base.Data.Localized, LocalizationSettings.StringDatabase.SmartFormatter.GetNotEmptyFormatterExtensionNames()));
				this.m_FormatCache.Table = base.Table;
			}
			return this.m_FormatCache;
		}

		// Token: 0x060001BC RID: 444 RVA: 0x000067F8 File Offset: 0x000049F8
		public string GetLocalizedString()
		{
			return this.GetLocalizedString(null, null, LocalizationSettings.SelectedLocaleAsync.Result as PseudoLocale);
		}

		// Token: 0x060001BD RID: 445 RVA: 0x00006820 File Offset: 0x00004A20
		public string GetLocalizedString(params object[] args)
		{
			return this.GetLocalizedString(null, args, LocalizationSettings.SelectedLocaleAsync.Result as PseudoLocale);
		}

		// Token: 0x060001BE RID: 446 RVA: 0x00006848 File Offset: 0x00004A48
		public string GetLocalizedString(IList<object> args)
		{
			return this.GetLocalizedString(null, args, LocalizationSettings.SelectedLocaleAsync.Result as PseudoLocale);
		}

		// Token: 0x060001BF RID: 447 RVA: 0x00006870 File Offset: 0x00004A70
		public string GetLocalizedString(IFormatProvider formatProvider, IList<object> args)
		{
			return this.GetLocalizedString(formatProvider, args, LocalizationSettings.SelectedLocaleAsync.Result as PseudoLocale);
		}

		// Token: 0x060001C0 RID: 448 RVA: 0x00006898 File Offset: 0x00004A98
		public string GetLocalizedString(IFormatProvider formatProvider, IList<object> args, PseudoLocale pseudoLocale)
		{
			if (formatProvider == null)
			{
				ILocalesProvider availableLocales = LocalizationSettings.AvailableLocales;
				formatProvider = ((availableLocales != null) ? availableLocales.GetLocale(base.Table.LocaleIdentifier) : null);
			}
			string text = null;
			if (this.IsSmart)
			{
				if (this.m_FormatCache == null)
				{
					this.m_FormatCache = this.GetOrCreateFormatCache();
				}
				text = LocalizationSettings.StringDatabase.SmartFormatter.FormatWithCache(ref this.m_FormatCache, base.Data.Localized, formatProvider, args);
			}
			else if (!string.IsNullOrEmpty(base.Data.Localized))
			{
				if (args != null && args.Count > 0)
				{
					try
					{
						text = ((formatProvider == null) ? string.Format(base.Data.Localized, (args as object[]) ?? args.ToArray<object>()) : string.Format(formatProvider, base.Data.Localized, (args as object[]) ?? args.ToArray<object>()));
						goto IL_00F8;
					}
					catch (FormatException ex)
					{
						throw new FormatException(string.Format("Input string was not in the correct format for String.Format. Ensure that the string is marked as Smart if you intended to use Smart Format.\n`{0}`\n{1}", base.Data.Localized, ex), ex);
					}
				}
				text = base.Data.Localized;
			}
			IL_00F8:
			if (pseudoLocale != null && !string.IsNullOrEmpty(text))
			{
				text = pseudoLocale.GetPseudoString(text);
			}
			return text;
		}

		// Token: 0x04000067 RID: 103
		private FormatCache m_FormatCache;
	}
}
