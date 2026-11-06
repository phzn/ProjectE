using System;
using System.Globalization;
using UnityEngine.Localization.SmartFormat.Core.Extensions;
using UnityEngine.Localization.SmartFormat.Core.Parsing;
using UnityEngine.Localization.SmartFormat.Net.Utilities;
using UnityEngine.Localization.SmartFormat.Utilities;

namespace UnityEngine.Localization.SmartFormat.Extensions
{
	// Token: 0x02000078 RID: 120
	[Serializable]
	public class TimeFormatter : FormatterBase
	{
		// Token: 0x0600032F RID: 815 RVA: 0x0000C505 File Offset: 0x0000A705
		public TimeFormatter()
		{
			base.Names = this.DefaultNames;
		}

		// Token: 0x170000A4 RID: 164
		// (get) Token: 0x06000330 RID: 816 RVA: 0x0000C52F File Offset: 0x0000A72F
		public override string[] DefaultNames
		{
			get
			{
				return new string[] { "timespan", "time", "t", "" };
			}
		}

		// Token: 0x170000A5 RID: 165
		// (get) Token: 0x06000331 RID: 817 RVA: 0x0000C557 File Offset: 0x0000A757
		// (set) Token: 0x06000332 RID: 818 RVA: 0x0000C55F File Offset: 0x0000A75F
		public TimeSpanFormatOptions DefaultFormatOptions
		{
			get
			{
				return this.m_DefaultFormatOptions;
			}
			set
			{
				this.m_DefaultFormatOptions = value;
			}
		}

		// Token: 0x170000A6 RID: 166
		// (get) Token: 0x06000333 RID: 819 RVA: 0x0000C568 File Offset: 0x0000A768
		// (set) Token: 0x06000334 RID: 820 RVA: 0x0000C570 File Offset: 0x0000A770
		public string DefaultTwoLetterISOLanguageName
		{
			get
			{
				return this.m_DefaultTwoLetterIsoLanguageName;
			}
			set
			{
				if (CommonLanguagesTimeTextInfo.GetTimeTextInfo(value) == null)
				{
					throw new ArgumentException("Language '" + value + "' for value is not implemented.");
				}
				this.m_DefaultTwoLetterIsoLanguageName = value;
			}
		}

		// Token: 0x06000335 RID: 821 RVA: 0x0000C598 File Offset: 0x0000A798
		public override bool TryEvaluateFormat(IFormattingInfo formattingInfo)
		{
			Format format = formattingInfo.Format;
			object currentValue = formattingInfo.CurrentValue;
			if (format != null && format.HasNested)
			{
				return false;
			}
			string text;
			if (!string.IsNullOrEmpty(formattingInfo.FormatterOptions))
			{
				text = formattingInfo.FormatterOptions;
			}
			else if (format != null)
			{
				text = format.GetLiteralText();
			}
			else
			{
				text = string.Empty;
			}
			TimeSpan timeSpan2;
			if (currentValue is TimeSpan)
			{
				TimeSpan timeSpan = (TimeSpan)currentValue;
				timeSpan2 = timeSpan;
			}
			else if (currentValue is DateTime)
			{
				DateTime dateTime = (DateTime)currentValue;
				if (!(formattingInfo.FormatterOptions != string.Empty))
				{
					return false;
				}
				timeSpan2 = SystemTime.Now().ToUniversalTime().Subtract(dateTime.ToUniversalTime());
			}
			else
			{
				if (!(currentValue is DateTimeOffset))
				{
					return false;
				}
				DateTimeOffset dateTimeOffset = (DateTimeOffset)currentValue;
				if (!(formattingInfo.FormatterOptions != string.Empty))
				{
					return false;
				}
				timeSpan2 = SystemTime.OffsetNow().UtcDateTime.Subtract(dateTimeOffset.UtcDateTime);
			}
			TimeTextInfo timeTextInfo = this.GetTimeTextInfo(formattingInfo.FormatDetails.Provider);
			if (timeTextInfo == null)
			{
				return false;
			}
			TimeSpanFormatOptions timeSpanFormatOptions = TimeSpanFormatOptionsConverter.Parse(text);
			string text2 = timeSpan2.ToTimeString(timeSpanFormatOptions, timeTextInfo);
			formattingInfo.Write(text2);
			return true;
		}

		// Token: 0x06000336 RID: 822 RVA: 0x0000C6D4 File Offset: 0x0000A8D4
		private TimeTextInfo GetTimeTextInfo(IFormatProvider provider)
		{
			if (provider == null)
			{
				return CommonLanguagesTimeTextInfo.GetTimeTextInfo(this.DefaultTwoLetterISOLanguageName);
			}
			TimeTextInfo timeTextInfo = provider.GetFormat(typeof(TimeTextInfo)) as TimeTextInfo;
			if (timeTextInfo != null)
			{
				return timeTextInfo;
			}
			CultureInfo cultureInfo = provider as CultureInfo;
			if (cultureInfo == null)
			{
				return CommonLanguagesTimeTextInfo.GetTimeTextInfo(this.DefaultTwoLetterISOLanguageName);
			}
			return CommonLanguagesTimeTextInfo.GetTimeTextInfo(cultureInfo.TwoLetterISOLanguageName);
		}

		// Token: 0x040000D1 RID: 209
		[SerializeField]
		private TimeSpanFormatOptions m_DefaultFormatOptions = TimeSpanUtility.DefaultFormatOptions;

		// Token: 0x040000D2 RID: 210
		private string m_DefaultTwoLetterIsoLanguageName = "en";
	}
}
