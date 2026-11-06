using System;
using System.Collections.Generic;
using System.Globalization;

namespace System.Net.Mime
{
	// Token: 0x020007E8 RID: 2024
	internal class SmtpDateTime
	{
		// Token: 0x060040CF RID: 16591 RVA: 0x000DDF08 File Offset: 0x000DC108
		internal static Dictionary<string, TimeSpan> InitializeShortHandLookups()
		{
			return new Dictionary<string, TimeSpan>
			{
				{
					"UT",
					TimeSpan.Zero
				},
				{
					"GMT",
					TimeSpan.Zero
				},
				{
					"EDT",
					new TimeSpan(-4, 0, 0)
				},
				{
					"EST",
					new TimeSpan(-5, 0, 0)
				},
				{
					"CDT",
					new TimeSpan(-5, 0, 0)
				},
				{
					"CST",
					new TimeSpan(-6, 0, 0)
				},
				{
					"MDT",
					new TimeSpan(-6, 0, 0)
				},
				{
					"MST",
					new TimeSpan(-7, 0, 0)
				},
				{
					"PDT",
					new TimeSpan(-7, 0, 0)
				},
				{
					"PST",
					new TimeSpan(-8, 0, 0)
				}
			};
		}

		// Token: 0x060040D0 RID: 16592 RVA: 0x000DDFDC File Offset: 0x000DC1DC
		internal SmtpDateTime(DateTime value)
		{
			this._date = value;
			switch (value.Kind)
			{
			case DateTimeKind.Unspecified:
				this._unknownTimeZone = true;
				return;
			case DateTimeKind.Utc:
				this._timeZone = TimeSpan.Zero;
				return;
			case DateTimeKind.Local:
			{
				TimeSpan utcOffset = TimeZoneInfo.Local.GetUtcOffset(value);
				this._timeZone = this.ValidateAndGetSanitizedTimeSpan(utcOffset);
				return;
			}
			default:
				return;
			}
		}

		// Token: 0x060040D1 RID: 16593 RVA: 0x000DE040 File Offset: 0x000DC240
		internal SmtpDateTime(string value)
		{
			string text;
			this._date = this.ParseValue(value, out text);
			if (!this.TryParseTimeZoneString(text, out this._timeZone))
			{
				this._unknownTimeZone = true;
			}
		}

		// Token: 0x17000E99 RID: 3737
		// (get) Token: 0x060040D2 RID: 16594 RVA: 0x000DE078 File Offset: 0x000DC278
		internal DateTime Date
		{
			get
			{
				if (this._unknownTimeZone)
				{
					return DateTime.SpecifyKind(this._date, DateTimeKind.Unspecified);
				}
				DateTimeOffset dateTimeOffset = new DateTimeOffset(this._date, this._timeZone);
				return dateTimeOffset.LocalDateTime;
			}
		}

		// Token: 0x060040D3 RID: 16595 RVA: 0x000DE0B4 File Offset: 0x000DC2B4
		public override string ToString()
		{
			return string.Format("{0} {1}", this.FormatDate(this._date), this._unknownTimeZone ? "-0000" : this.TimeSpanToOffset(this._timeZone));
		}

		// Token: 0x060040D4 RID: 16596 RVA: 0x000DE0E8 File Offset: 0x000DC2E8
		internal void ValidateAndGetTimeZoneOffsetValues(string offset, out bool positive, out int hours, out int minutes)
		{
			if (offset.Length != 5)
			{
				throw new FormatException("The date is in an invalid format.");
			}
			positive = offset.StartsWith("+", StringComparison.Ordinal);
			if (!int.TryParse(offset.Substring(1, 2), NumberStyles.None, CultureInfo.InvariantCulture, out hours))
			{
				throw new FormatException("The date is in an invalid format.");
			}
			if (!int.TryParse(offset.Substring(3, 2), NumberStyles.None, CultureInfo.InvariantCulture, out minutes))
			{
				throw new FormatException("The date is in an invalid format.");
			}
			if (minutes > 59)
			{
				throw new FormatException("The date is in an invalid format.");
			}
		}

		// Token: 0x060040D5 RID: 16597 RVA: 0x000DE16C File Offset: 0x000DC36C
		internal void ValidateTimeZoneShortHandValue(string value)
		{
			for (int i = 0; i < value.Length; i++)
			{
				if (!char.IsLetter(value, i))
				{
					throw new FormatException(SR.Format("An invalid character was found in the mail header: '{0}'.", value));
				}
			}
		}

		// Token: 0x060040D6 RID: 16598 RVA: 0x000DE1A4 File Offset: 0x000DC3A4
		internal string FormatDate(DateTime value)
		{
			return value.ToString("ddd, dd MMM yyyy HH:mm:ss", CultureInfo.InvariantCulture);
		}

		// Token: 0x060040D7 RID: 16599 RVA: 0x000DE1B8 File Offset: 0x000DC3B8
		internal DateTime ParseValue(string data, out string timeZone)
		{
			if (string.IsNullOrEmpty(data))
			{
				throw new FormatException("The date is in an invalid format.");
			}
			int num = data.IndexOf(':');
			if (num == -1)
			{
				throw new FormatException(SR.Format("An invalid character was found in the mail header: '{0}'.", data));
			}
			int num2 = data.IndexOfAny(SmtpDateTime.s_allowedWhiteSpaceChars, num);
			if (num2 == -1)
			{
				throw new FormatException(SR.Format("An invalid character was found in the mail header: '{0}'.", data));
			}
			DateTime dateTime;
			if (!DateTime.TryParseExact(data.Substring(0, num2).Trim(), SmtpDateTime.s_validDateTimeFormats, CultureInfo.InvariantCulture, DateTimeStyles.AllowWhiteSpaces, out dateTime))
			{
				throw new FormatException("The date is in an invalid format.");
			}
			string text = data.Substring(num2).Trim();
			int num3 = text.IndexOfAny(SmtpDateTime.s_allowedWhiteSpaceChars);
			if (num3 != -1)
			{
				text = text.Substring(0, num3);
			}
			if (string.IsNullOrEmpty(text))
			{
				throw new FormatException("The date is in an invalid format.");
			}
			timeZone = text;
			return dateTime;
		}

		// Token: 0x060040D8 RID: 16600 RVA: 0x000DE284 File Offset: 0x000DC484
		internal bool TryParseTimeZoneString(string timeZoneString, out TimeSpan timeZone)
		{
			if (timeZoneString == "-0000")
			{
				timeZone = TimeSpan.Zero;
				return false;
			}
			if (timeZoneString[0] == '+' || timeZoneString[0] == '-')
			{
				bool flag;
				int num;
				int num2;
				this.ValidateAndGetTimeZoneOffsetValues(timeZoneString, out flag, out num, out num2);
				if (!flag)
				{
					if (num != 0)
					{
						num *= -1;
					}
					else if (num2 != 0)
					{
						num2 *= -1;
					}
				}
				timeZone = new TimeSpan(num, num2, 0);
				return true;
			}
			this.ValidateTimeZoneShortHandValue(timeZoneString);
			return SmtpDateTime.s_timeZoneOffsetLookup.TryGetValue(timeZoneString, out timeZone);
		}

		// Token: 0x060040D9 RID: 16601 RVA: 0x000DE304 File Offset: 0x000DC504
		internal TimeSpan ValidateAndGetSanitizedTimeSpan(TimeSpan span)
		{
			TimeSpan timeSpan = new TimeSpan(span.Days, span.Hours, span.Minutes, 0, 0);
			if (Math.Abs(timeSpan.Ticks) > 3599400000000L)
			{
				throw new FormatException("The date is in an invalid format.");
			}
			return timeSpan;
		}

		// Token: 0x060040DA RID: 16602 RVA: 0x000DE354 File Offset: 0x000DC554
		internal string TimeSpanToOffset(TimeSpan span)
		{
			if (span.Ticks == 0L)
			{
				return "+0000";
			}
			uint num = (uint)Math.Abs(Math.Floor(span.TotalHours));
			uint num2 = (uint)Math.Abs(span.Minutes);
			string text = ((span.Ticks > 0L) ? "+" : "-");
			if (num < 10U)
			{
				text += "0";
			}
			text += num.ToString();
			if (num2 < 10U)
			{
				text += "0";
			}
			return text + num2.ToString();
		}

		// Token: 0x040026CD RID: 9933
		internal const string UnknownTimeZoneDefaultOffset = "-0000";

		// Token: 0x040026CE RID: 9934
		internal const string UtcDefaultTimeZoneOffset = "+0000";

		// Token: 0x040026CF RID: 9935
		internal const int OffsetLength = 5;

		// Token: 0x040026D0 RID: 9936
		internal const int MaxMinuteValue = 59;

		// Token: 0x040026D1 RID: 9937
		internal const string DateFormatWithDayOfWeek = "ddd, dd MMM yyyy HH:mm:ss";

		// Token: 0x040026D2 RID: 9938
		internal const string DateFormatWithoutDayOfWeek = "dd MMM yyyy HH:mm:ss";

		// Token: 0x040026D3 RID: 9939
		internal const string DateFormatWithDayOfWeekAndNoSeconds = "ddd, dd MMM yyyy HH:mm";

		// Token: 0x040026D4 RID: 9940
		internal const string DateFormatWithoutDayOfWeekAndNoSeconds = "dd MMM yyyy HH:mm";

		// Token: 0x040026D5 RID: 9941
		internal static readonly string[] s_validDateTimeFormats = new string[] { "ddd, dd MMM yyyy HH:mm:ss", "dd MMM yyyy HH:mm:ss", "ddd, dd MMM yyyy HH:mm", "dd MMM yyyy HH:mm" };

		// Token: 0x040026D6 RID: 9942
		internal static readonly char[] s_allowedWhiteSpaceChars = new char[] { ' ', '\t' };

		// Token: 0x040026D7 RID: 9943
		internal static readonly Dictionary<string, TimeSpan> s_timeZoneOffsetLookup = SmtpDateTime.InitializeShortHandLookups();

		// Token: 0x040026D8 RID: 9944
		internal const long TimeSpanMaxTicks = 3599400000000L;

		// Token: 0x040026D9 RID: 9945
		internal const int OffsetMaxValue = 9959;

		// Token: 0x040026DA RID: 9946
		private readonly DateTime _date;

		// Token: 0x040026DB RID: 9947
		private readonly TimeSpan _timeZone;

		// Token: 0x040026DC RID: 9948
		private readonly bool _unknownTimeZone;
	}
}
