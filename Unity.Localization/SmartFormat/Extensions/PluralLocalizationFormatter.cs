using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using UnityEngine.Localization.Settings;
using UnityEngine.Localization.SmartFormat.Core.Extensions;
using UnityEngine.Localization.SmartFormat.Core.Formatting;
using UnityEngine.Localization.SmartFormat.Core.Parsing;
using UnityEngine.Localization.SmartFormat.Utilities;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace UnityEngine.Localization.SmartFormat.Extensions
{
	// Token: 0x02000073 RID: 115
	[Serializable]
	public class PluralLocalizationFormatter : FormatterBase, IFormatterLiteralExtractor
	{
		// Token: 0x1700009A RID: 154
		// (get) Token: 0x0600030E RID: 782 RVA: 0x0000BBA0 File Offset: 0x00009DA0
		// (set) Token: 0x0600030F RID: 783 RVA: 0x0000BBA8 File Offset: 0x00009DA8
		public string DefaultTwoLetterISOLanguageName
		{
			get
			{
				return this.m_DefaultTwoLetterISOLanguageName;
			}
			set
			{
				this.m_DefaultTwoLetterISOLanguageName = value;
				this.m_DefaultPluralRule = PluralRules.GetPluralRule(value);
			}
		}

		// Token: 0x06000310 RID: 784 RVA: 0x0000BBBD File Offset: 0x00009DBD
		public PluralLocalizationFormatter()
		{
			base.Names = this.DefaultNames;
		}

		// Token: 0x1700009B RID: 155
		// (get) Token: 0x06000311 RID: 785 RVA: 0x0000BBDC File Offset: 0x00009DDC
		public override string[] DefaultNames
		{
			get
			{
				return new string[] { "plural", "p", "" };
			}
		}

		// Token: 0x06000312 RID: 786 RVA: 0x0000BBFC File Offset: 0x00009DFC
		public override bool TryEvaluateFormat(IFormattingInfo formattingInfo)
		{
			Format format = formattingInfo.Format;
			object currentValue = formattingInfo.CurrentValue;
			if (format == null || format.baseString[format.startIndex] == ':')
			{
				return false;
			}
			IList<Format> list = format.Split('|');
			if (list.Count == 1)
			{
				return false;
			}
			IConvertible convertible = currentValue as IConvertible;
			decimal num;
			if (convertible != null && !(currentValue is DateTime) && !(currentValue is string) && !(currentValue is bool) && !(currentValue is Enum))
			{
				num = convertible.ToDecimal(null);
			}
			else
			{
				IEnumerable<object> enumerable = currentValue as IEnumerable<object>;
				if (enumerable == null)
				{
					return false;
				}
				num = enumerable.Count<object>();
			}
			PluralRules.PluralRuleDelegate pluralRule = this.GetPluralRule(formattingInfo);
			if (pluralRule == null)
			{
				return false;
			}
			int count = list.Count;
			int num2 = pluralRule(num, count);
			if (num2 < 0 || list.Count <= num2)
			{
				throw new FormattingException(format, "Invalid number of plural parameters", list.Last<Format>().endIndex);
			}
			Format format2 = list[num2];
			formattingInfo.Write(format2, currentValue);
			return true;
		}

		// Token: 0x06000313 RID: 787 RVA: 0x0000BCF8 File Offset: 0x00009EF8
		protected virtual PluralRules.PluralRuleDelegate GetPluralRule(IFormattingInfo formattingInfo)
		{
			string formatterOptions = formattingInfo.FormatterOptions;
			if (formatterOptions.Length != 0)
			{
				return PluralRules.GetPluralRule(formatterOptions);
			}
			IFormatProvider formatProvider = formattingInfo.FormatDetails.Provider;
			CustomPluralRuleProvider customPluralRuleProvider = (CustomPluralRuleProvider)((formatProvider != null) ? formatProvider.GetFormat(typeof(CustomPluralRuleProvider)) : null);
			if (customPluralRuleProvider != null)
			{
				return customPluralRuleProvider.GetPluralRule();
			}
			Locale locale = formatProvider as Locale;
			if (locale != null)
			{
				formatProvider = locale.Identifier.CultureInfo;
			}
			CultureInfo cultureInfo = formatProvider as CultureInfo;
			if (cultureInfo != null)
			{
				return PluralRules.GetPluralRule(cultureInfo.TwoLetterISOLanguageName);
			}
			Locale locale2 = null;
			AsyncOperationHandle<Locale> selectedLocaleAsync = LocalizationSettings.SelectedLocaleAsync;
			if (selectedLocaleAsync.IsValid() && selectedLocaleAsync.IsDone)
			{
				locale2 = selectedLocaleAsync.Result;
			}
			if (locale2 != null)
			{
				CultureInfo cultureInfo2 = locale2.Identifier.CultureInfo;
				string text;
				if (cultureInfo2 != null)
				{
					text = cultureInfo2.TwoLetterISOLanguageName;
				}
				else
				{
					text = locale2.Identifier.Code;
					if (locale2.Identifier.Code.Length > 2)
					{
						text = locale2.Identifier.Code.Substring(0, 2);
					}
				}
				return PluralRules.GetPluralRule(text);
			}
			PluralRules.PluralRuleDelegate pluralRuleDelegate;
			if ((pluralRuleDelegate = this.m_DefaultPluralRule) == null)
			{
				pluralRuleDelegate = (this.m_DefaultPluralRule = PluralRules.GetPluralRule(this.DefaultTwoLetterISOLanguageName));
			}
			return pluralRuleDelegate;
		}

		// Token: 0x06000314 RID: 788 RVA: 0x0000BE3C File Offset: 0x0000A03C
		public void WriteAllLiterals(IFormattingInfo formattingInfo)
		{
			Format format = formattingInfo.Format;
			if (format == null || format.baseString[format.startIndex] == ':')
			{
				return;
			}
			IList<Format> list = format.Split('|');
			if (list.Count == 1)
			{
				return;
			}
			for (int i = 0; i < list.Count; i++)
			{
				formattingInfo.Write(list[i], null);
			}
		}

		// Token: 0x040000C6 RID: 198
		[SerializeField]
		private string m_DefaultTwoLetterISOLanguageName = "en";

		// Token: 0x040000C7 RID: 199
		private PluralRules.PluralRuleDelegate m_DefaultPluralRule;
	}
}
