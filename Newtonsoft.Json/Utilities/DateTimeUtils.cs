using System;
using System.Globalization;
using System.IO;
using System.Runtime.CompilerServices;
using System.Xml;

namespace Newtonsoft.Json.Utilities
{
	// Token: 0x0200004E RID: 78
	[NullableContext(1)]
	[Nullable(0)]
	internal static class DateTimeUtils
	{
		// Token: 0x06000494 RID: 1172 RVA: 0x00012CF0 File Offset: 0x00010EF0
		public static TimeSpan GetUtcOffset(this DateTime d)
		{
			return TimeZoneInfo.Local.GetUtcOffset(d);
		}

		// Token: 0x06000495 RID: 1173 RVA: 0x00012CFD File Offset: 0x00010EFD
		public static XmlDateTimeSerializationMode ToSerializationMode(DateTimeKind kind)
		{
			switch (kind)
			{
			case DateTimeKind.Unspecified:
				return XmlDateTimeSerializationMode.Unspecified;
			case DateTimeKind.Utc:
				return XmlDateTimeSerializationMode.Utc;
			case DateTimeKind.Local:
				return XmlDateTimeSerializationMode.Local;
			default:
				throw MiscellaneousUtils.CreateArgumentOutOfRangeException("kind", kind, "Unexpected DateTimeKind value.");
			}
		}

		// Token: 0x06000496 RID: 1174 RVA: 0x00012D30 File Offset: 0x00010F30
		internal static DateTime EnsureDateTime(DateTime value, DateTimeZoneHandling timeZone)
		{
			switch (timeZone)
			{
			case DateTimeZoneHandling.Local:
				value = DateTimeUtils.SwitchToLocalTime(value);
				break;
			case DateTimeZoneHandling.Utc:
				value = DateTimeUtils.SwitchToUtcTime(value);
				break;
			case DateTimeZoneHandling.Unspecified:
				value = new DateTime(value.Ticks, DateTimeKind.Unspecified);
				break;
			case DateTimeZoneHandling.RoundtripKind:
				break;
			default:
				throw new ArgumentException("Invalid date time handling value.");
			}
			return value;
		}

		// Token: 0x06000497 RID: 1175 RVA: 0x00012D88 File Offset: 0x00010F88
		private static DateTime SwitchToLocalTime(DateTime value)
		{
			switch (value.Kind)
			{
			case DateTimeKind.Unspecified:
				return new DateTime(value.Ticks, DateTimeKind.Local);
			case DateTimeKind.Utc:
				return value.ToLocalTime();
			case DateTimeKind.Local:
				return value;
			default:
				return value;
			}
		}

		// Token: 0x06000498 RID: 1176 RVA: 0x00012DCC File Offset: 0x00010FCC
		private static DateTime SwitchToUtcTime(DateTime value)
		{
			switch (value.Kind)
			{
			case DateTimeKind.Unspecified:
				return new DateTime(value.Ticks, DateTimeKind.Utc);
			case DateTimeKind.Utc:
				return value;
			case DateTimeKind.Local:
				return value.ToUniversalTime();
			default:
				return value;
			}
		}

		// Token: 0x06000499 RID: 1177 RVA: 0x00012E0E File Offset: 0x0001100E
		private static long ToUniversalTicks(DateTime dateTime)
		{
			if (dateTime.Kind == DateTimeKind.Utc)
			{
				return dateTime.Ticks;
			}
			return DateTimeUtils.ToUniversalTicks(dateTime, dateTime.GetUtcOffset());
		}

		// Token: 0x0600049A RID: 1178 RVA: 0x00012E30 File Offset: 0x00011030
		private static long ToUniversalTicks(DateTime dateTime, TimeSpan offset)
		{
			if (dateTime.Kind == DateTimeKind.Utc || dateTime == DateTime.MaxValue || dateTime == DateTime.MinValue)
			{
				return dateTime.Ticks;
			}
			long num = dateTime.Ticks - offset.Ticks;
			if (num > 3155378975999999999L)
			{
				return 3155378975999999999L;
			}
			if (num < 0L)
			{
				return 0L;
			}
			return num;
		}

		// Token: 0x0600049B RID: 1179 RVA: 0x00012E98 File Offset: 0x00011098
		internal static long ConvertDateTimeToJavaScriptTicks(DateTime dateTime, TimeSpan offset)
		{
			return DateTimeUtils.UniversalTicksToJavaScriptTicks(DateTimeUtils.ToUniversalTicks(dateTime, offset));
		}

		// Token: 0x0600049C RID: 1180 RVA: 0x00012EA6 File Offset: 0x000110A6
		internal static long ConvertDateTimeToJavaScriptTicks(DateTime dateTime)
		{
			return DateTimeUtils.ConvertDateTimeToJavaScriptTicks(dateTime, true);
		}

		// Token: 0x0600049D RID: 1181 RVA: 0x00012EAF File Offset: 0x000110AF
		internal static long ConvertDateTimeToJavaScriptTicks(DateTime dateTime, bool convertToUtc)
		{
			return DateTimeUtils.UniversalTicksToJavaScriptTicks(convertToUtc ? DateTimeUtils.ToUniversalTicks(dateTime) : dateTime.Ticks);
		}

		// Token: 0x0600049E RID: 1182 RVA: 0x00012EC8 File Offset: 0x000110C8
		private static long UniversalTicksToJavaScriptTicks(long universalTicks)
		{
			return (universalTicks - DateTimeUtils.InitialJavaScriptDateTicks) / 10000L;
		}

		// Token: 0x0600049F RID: 1183 RVA: 0x00012ED8 File Offset: 0x000110D8
		internal static DateTime ConvertJavaScriptTicksToDateTime(long javaScriptTicks)
		{
			return new DateTime(javaScriptTicks * 10000L + DateTimeUtils.InitialJavaScriptDateTicks, DateTimeKind.Utc);
		}

		// Token: 0x060004A0 RID: 1184 RVA: 0x00012EF0 File Offset: 0x000110F0
		internal static bool TryParseDateTimeIso(StringReference text, DateTimeZoneHandling dateTimeZoneHandling, out DateTime dt)
		{
			DateTimeParser dateTimeParser = default(DateTimeParser);
			if (!dateTimeParser.Parse(text.Chars, text.StartIndex, text.Length))
			{
				dt = default(DateTime);
				return false;
			}
			DateTime dateTime = DateTimeUtils.CreateDateTime(dateTimeParser);
			switch (dateTimeParser.Zone)
			{
			case ParserTimeZone.Utc:
				dateTime = new DateTime(dateTime.Ticks, DateTimeKind.Utc);
				break;
			case ParserTimeZone.LocalWestOfUtc:
			{
				TimeSpan timeSpan = new TimeSpan(dateTimeParser.ZoneHour, dateTimeParser.ZoneMinute, 0);
				long num = dateTime.Ticks + timeSpan.Ticks;
				if (num <= DateTime.MaxValue.Ticks)
				{
					dateTime = new DateTime(num, DateTimeKind.Utc).ToLocalTime();
				}
				else
				{
					num += dateTime.GetUtcOffset().Ticks;
					if (num > DateTime.MaxValue.Ticks)
					{
						num = DateTime.MaxValue.Ticks;
					}
					dateTime = new DateTime(num, DateTimeKind.Local);
				}
				break;
			}
			case ParserTimeZone.LocalEastOfUtc:
			{
				TimeSpan timeSpan2 = new TimeSpan(dateTimeParser.ZoneHour, dateTimeParser.ZoneMinute, 0);
				long num = dateTime.Ticks - timeSpan2.Ticks;
				if (num >= DateTime.MinValue.Ticks)
				{
					dateTime = new DateTime(num, DateTimeKind.Utc).ToLocalTime();
				}
				else
				{
					num += dateTime.GetUtcOffset().Ticks;
					if (num < DateTime.MinValue.Ticks)
					{
						num = DateTime.MinValue.Ticks;
					}
					dateTime = new DateTime(num, DateTimeKind.Local);
				}
				break;
			}
			}
			dt = DateTimeUtils.EnsureDateTime(dateTime, dateTimeZoneHandling);
			return true;
		}

		// Token: 0x060004A1 RID: 1185 RVA: 0x00013084 File Offset: 0x00011284
		internal static bool TryParseDateTimeOffsetIso(StringReference text, out DateTimeOffset dt)
		{
			DateTimeParser dateTimeParser = default(DateTimeParser);
			if (!dateTimeParser.Parse(text.Chars, text.StartIndex, text.Length))
			{
				dt = default(DateTimeOffset);
				return false;
			}
			DateTime dateTime = DateTimeUtils.CreateDateTime(dateTimeParser);
			TimeSpan utcOffset;
			switch (dateTimeParser.Zone)
			{
			case ParserTimeZone.Utc:
				utcOffset = new TimeSpan(0L);
				break;
			case ParserTimeZone.LocalWestOfUtc:
				utcOffset = new TimeSpan(-dateTimeParser.ZoneHour, -dateTimeParser.ZoneMinute, 0);
				break;
			case ParserTimeZone.LocalEastOfUtc:
				utcOffset = new TimeSpan(dateTimeParser.ZoneHour, dateTimeParser.ZoneMinute, 0);
				break;
			default:
				utcOffset = TimeZoneInfo.Local.GetUtcOffset(dateTime);
				break;
			}
			long num = dateTime.Ticks - utcOffset.Ticks;
			if (num < 0L || num > 3155378975999999999L)
			{
				dt = default(DateTimeOffset);
				return false;
			}
			dt = new DateTimeOffset(dateTime, utcOffset);
			return true;
		}

		// Token: 0x060004A2 RID: 1186 RVA: 0x00013164 File Offset: 0x00011364
		private static DateTime CreateDateTime(DateTimeParser dateTimeParser)
		{
			bool flag;
			if (dateTimeParser.Hour == 24)
			{
				flag = true;
				dateTimeParser.Hour = 0;
			}
			else
			{
				flag = false;
			}
			DateTime dateTime = new DateTime(dateTimeParser.Year, dateTimeParser.Month, dateTimeParser.Day, dateTimeParser.Hour, dateTimeParser.Minute, dateTimeParser.Second);
			dateTime = dateTime.AddTicks((long)dateTimeParser.Fraction);
			if (flag)
			{
				dateTime = dateTime.AddDays(1.0);
			}
			return dateTime;
		}

		// Token: 0x060004A3 RID: 1187 RVA: 0x000131D8 File Offset: 0x000113D8
		internal static bool TryParseDateTime(StringReference s, DateTimeZoneHandling dateTimeZoneHandling, [Nullable(2)] string dateFormatString, CultureInfo culture, out DateTime dt)
		{
			if (s.Length > 0)
			{
				int startIndex = s.StartIndex;
				if (s[startIndex] == '/')
				{
					if (s.Length >= 9 && s.StartsWith("/Date(") && s.EndsWith(")/") && DateTimeUtils.TryParseDateTimeMicrosoft(s, dateTimeZoneHandling, out dt))
					{
						return true;
					}
				}
				else if (s.Length >= 19 && s.Length <= 40 && char.IsDigit(s[startIndex]) && s[startIndex + 10] == 'T' && DateTimeUtils.TryParseDateTimeIso(s, dateTimeZoneHandling, out dt))
				{
					return true;
				}
				if (!StringUtils.IsNullOrEmpty(dateFormatString) && DateTimeUtils.TryParseDateTimeExact(s.ToString(), dateTimeZoneHandling, dateFormatString, culture, out dt))
				{
					return true;
				}
			}
			dt = default(DateTime);
			return false;
		}

		// Token: 0x060004A4 RID: 1188 RVA: 0x000132A8 File Offset: 0x000114A8
		internal static bool TryParseDateTime(string s, DateTimeZoneHandling dateTimeZoneHandling, [Nullable(2)] string dateFormatString, CultureInfo culture, out DateTime dt)
		{
			if (s.Length > 0)
			{
				if (s[0] == '/')
				{
					if (s.Length >= 9 && s.StartsWith("/Date(", StringComparison.Ordinal) && s.EndsWith(")/", StringComparison.Ordinal) && DateTimeUtils.TryParseDateTimeMicrosoft(new StringReference(s.ToCharArray(), 0, s.Length), dateTimeZoneHandling, out dt))
					{
						return true;
					}
				}
				else if (s.Length >= 19 && s.Length <= 40 && char.IsDigit(s[0]) && s[10] == 'T' && DateTime.TryParseExact(s, "yyyy-MM-ddTHH:mm:ss.FFFFFFFK", CultureInfo.InvariantCulture, DateTimeStyles.RoundtripKind, out dt))
				{
					dt = DateTimeUtils.EnsureDateTime(dt, dateTimeZoneHandling);
					return true;
				}
				if (!StringUtils.IsNullOrEmpty(dateFormatString) && DateTimeUtils.TryParseDateTimeExact(s, dateTimeZoneHandling, dateFormatString, culture, out dt))
				{
					return true;
				}
			}
			dt = default(DateTime);
			return false;
		}

		// Token: 0x060004A5 RID: 1189 RVA: 0x00013394 File Offset: 0x00011594
		internal static bool TryParseDateTimeOffset(StringReference s, [Nullable(2)] string dateFormatString, CultureInfo culture, out DateTimeOffset dt)
		{
			if (s.Length > 0)
			{
				int startIndex = s.StartIndex;
				if (s[startIndex] == '/')
				{
					if (s.Length >= 9 && s.StartsWith("/Date(") && s.EndsWith(")/") && DateTimeUtils.TryParseDateTimeOffsetMicrosoft(s, out dt))
					{
						return true;
					}
				}
				else if (s.Length >= 19 && s.Length <= 40 && char.IsDigit(s[startIndex]) && s[startIndex + 10] == 'T' && DateTimeUtils.TryParseDateTimeOffsetIso(s, out dt))
				{
					return true;
				}
				if (!StringUtils.IsNullOrEmpty(dateFormatString) && DateTimeUtils.TryParseDateTimeOffsetExact(s.ToString(), dateFormatString, culture, out dt))
				{
					return true;
				}
			}
			dt = default(DateTimeOffset);
			return false;
		}

		// Token: 0x060004A6 RID: 1190 RVA: 0x0001345C File Offset: 0x0001165C
		internal static bool TryParseDateTimeOffset(string s, [Nullable(2)] string dateFormatString, CultureInfo culture, out DateTimeOffset dt)
		{
			if (s.Length > 0)
			{
				if (s[0] == '/')
				{
					if (s.Length >= 9 && s.StartsWith("/Date(", StringComparison.Ordinal) && s.EndsWith(")/", StringComparison.Ordinal) && DateTimeUtils.TryParseDateTimeOffsetMicrosoft(new StringReference(s.ToCharArray(), 0, s.Length), out dt))
					{
						return true;
					}
				}
				else if (s.Length >= 19 && s.Length <= 40 && char.IsDigit(s[0]) && s[10] == 'T' && DateTimeOffset.TryParseExact(s, "yyyy-MM-ddTHH:mm:ss.FFFFFFFK", CultureInfo.InvariantCulture, DateTimeStyles.RoundtripKind, out dt) && DateTimeUtils.TryParseDateTimeOffsetIso(new StringReference(s.ToCharArray(), 0, s.Length), out dt))
				{
					return true;
				}
				if (!StringUtils.IsNullOrEmpty(dateFormatString) && DateTimeUtils.TryParseDateTimeOffsetExact(s, dateFormatString, culture, out dt))
				{
					return true;
				}
			}
			dt = default(DateTimeOffset);
			return false;
		}

		// Token: 0x060004A7 RID: 1191 RVA: 0x00013548 File Offset: 0x00011748
		private static bool TryParseMicrosoftDate(StringReference text, out long ticks, out TimeSpan offset, out DateTimeKind kind)
		{
			kind = DateTimeKind.Utc;
			int num = text.IndexOf('+', 7, text.Length - 8);
			if (num == -1)
			{
				num = text.IndexOf('-', 7, text.Length - 8);
			}
			if (num != -1)
			{
				kind = DateTimeKind.Local;
				if (!DateTimeUtils.TryReadOffset(text, num + text.StartIndex, out offset))
				{
					ticks = 0L;
					return false;
				}
			}
			else
			{
				offset = TimeSpan.Zero;
				num = text.Length - 2;
			}
			return ConvertUtils.Int64TryParse(text.Chars, 6 + text.StartIndex, num - 6, out ticks) == ParseResult.Success;
		}

		// Token: 0x060004A8 RID: 1192 RVA: 0x000135D4 File Offset: 0x000117D4
		private static bool TryParseDateTimeMicrosoft(StringReference text, DateTimeZoneHandling dateTimeZoneHandling, out DateTime dt)
		{
			long num;
			TimeSpan timeSpan;
			DateTimeKind dateTimeKind;
			if (!DateTimeUtils.TryParseMicrosoftDate(text, out num, out timeSpan, out dateTimeKind))
			{
				dt = default(DateTime);
				return false;
			}
			DateTime dateTime = DateTimeUtils.ConvertJavaScriptTicksToDateTime(num);
			if (dateTimeKind != DateTimeKind.Unspecified)
			{
				if (dateTimeKind != DateTimeKind.Local)
				{
					dt = dateTime;
				}
				else
				{
					dt = dateTime.ToLocalTime();
				}
			}
			else
			{
				dt = DateTime.SpecifyKind(dateTime.ToLocalTime(), DateTimeKind.Unspecified);
			}
			dt = DateTimeUtils.EnsureDateTime(dt, dateTimeZoneHandling);
			return true;
		}

		// Token: 0x060004A9 RID: 1193 RVA: 0x00013648 File Offset: 0x00011848
		private static bool TryParseDateTimeExact(string text, DateTimeZoneHandling dateTimeZoneHandling, string dateFormatString, CultureInfo culture, out DateTime dt)
		{
			DateTime dateTime;
			if (DateTime.TryParseExact(text, dateFormatString, culture, DateTimeStyles.RoundtripKind, out dateTime))
			{
				dateTime = DateTimeUtils.EnsureDateTime(dateTime, dateTimeZoneHandling);
				dt = dateTime;
				return true;
			}
			dt = default(DateTime);
			return false;
		}

		// Token: 0x060004AA RID: 1194 RVA: 0x00013684 File Offset: 0x00011884
		private static bool TryParseDateTimeOffsetMicrosoft(StringReference text, out DateTimeOffset dt)
		{
			long num;
			TimeSpan timeSpan;
			DateTimeKind dateTimeKind;
			if (!DateTimeUtils.TryParseMicrosoftDate(text, out num, out timeSpan, out dateTimeKind))
			{
				dt = default(DateTime);
				return false;
			}
			dt = new DateTimeOffset(DateTimeUtils.ConvertJavaScriptTicksToDateTime(num).Add(timeSpan).Ticks, timeSpan);
			return true;
		}

		// Token: 0x060004AB RID: 1195 RVA: 0x000136DC File Offset: 0x000118DC
		private static bool TryParseDateTimeOffsetExact(string text, string dateFormatString, CultureInfo culture, out DateTimeOffset dt)
		{
			DateTimeOffset dateTimeOffset;
			if (DateTimeOffset.TryParseExact(text, dateFormatString, culture, DateTimeStyles.RoundtripKind, out dateTimeOffset))
			{
				dt = dateTimeOffset;
				return true;
			}
			dt = default(DateTimeOffset);
			return false;
		}

		// Token: 0x060004AC RID: 1196 RVA: 0x0001370C File Offset: 0x0001190C
		private static bool TryReadOffset(StringReference offsetText, int startIndex, out TimeSpan offset)
		{
			bool flag = offsetText[startIndex] == '-';
			int num;
			if (ConvertUtils.Int32TryParse(offsetText.Chars, startIndex + 1, 2, out num) != ParseResult.Success)
			{
				offset = default(TimeSpan);
				return false;
			}
			int num2 = 0;
			if (offsetText.Length - startIndex > 5 && ConvertUtils.Int32TryParse(offsetText.Chars, startIndex + 3, 2, out num2) != ParseResult.Success)
			{
				offset = default(TimeSpan);
				return false;
			}
			offset = TimeSpan.FromHours((double)num) + TimeSpan.FromMinutes((double)num2);
			if (flag)
			{
				offset = offset.Negate();
			}
			return true;
		}

		// Token: 0x060004AD RID: 1197 RVA: 0x0001379C File Offset: 0x0001199C
		internal static void WriteDateTimeString(TextWriter writer, DateTime value, DateFormatHandling format, [Nullable(2)] string formatString, CultureInfo culture)
		{
			if (StringUtils.IsNullOrEmpty(formatString))
			{
				char[] array = new char[64];
				int num = DateTimeUtils.WriteDateTimeString(array, 0, value, null, value.Kind, format);
				writer.Write(array, 0, num);
				return;
			}
			writer.Write(value.ToString(formatString, culture));
		}

		// Token: 0x060004AE RID: 1198 RVA: 0x000137F0 File Offset: 0x000119F0
		internal static int WriteDateTimeString(char[] chars, int start, DateTime value, TimeSpan? offset, DateTimeKind kind, DateFormatHandling format)
		{
			int num2;
			if (format == DateFormatHandling.MicrosoftDateFormat)
			{
				TimeSpan timeSpan = offset ?? value.GetUtcOffset();
				long num = DateTimeUtils.ConvertDateTimeToJavaScriptTicks(value, timeSpan);
				"\\/Date(".CopyTo(0, chars, start, 7);
				num2 = start + 7;
				string text = num.ToString(CultureInfo.InvariantCulture);
				text.CopyTo(0, chars, num2, text.Length);
				num2 += text.Length;
				if (kind != DateTimeKind.Unspecified)
				{
					if (kind == DateTimeKind.Local)
					{
						num2 = DateTimeUtils.WriteDateTimeOffset(chars, num2, timeSpan, format);
					}
				}
				else if (value != DateTime.MaxValue && value != DateTime.MinValue)
				{
					num2 = DateTimeUtils.WriteDateTimeOffset(chars, num2, timeSpan, format);
				}
				")\\/".CopyTo(0, chars, num2, 3);
				num2 += 3;
			}
			else
			{
				num2 = DateTimeUtils.WriteDefaultIsoDate(chars, start, value);
				if (kind != DateTimeKind.Utc)
				{
					if (kind == DateTimeKind.Local)
					{
						num2 = DateTimeUtils.WriteDateTimeOffset(chars, num2, offset ?? value.GetUtcOffset(), format);
					}
				}
				else
				{
					chars[num2++] = 'Z';
				}
			}
			return num2;
		}

		// Token: 0x060004AF RID: 1199 RVA: 0x000138F8 File Offset: 0x00011AF8
		internal static int WriteDefaultIsoDate(char[] chars, int start, DateTime dt)
		{
			int num = 19;
			int num2;
			int num3;
			int num4;
			DateTimeUtils.GetDateValues(dt, out num2, out num3, out num4);
			DateTimeUtils.CopyIntToCharArray(chars, start, num2, 4);
			chars[start + 4] = '-';
			DateTimeUtils.CopyIntToCharArray(chars, start + 5, num3, 2);
			chars[start + 7] = '-';
			DateTimeUtils.CopyIntToCharArray(chars, start + 8, num4, 2);
			chars[start + 10] = 'T';
			DateTimeUtils.CopyIntToCharArray(chars, start + 11, dt.Hour, 2);
			chars[start + 13] = ':';
			DateTimeUtils.CopyIntToCharArray(chars, start + 14, dt.Minute, 2);
			chars[start + 16] = ':';
			DateTimeUtils.CopyIntToCharArray(chars, start + 17, dt.Second, 2);
			int num5 = (int)(dt.Ticks % 10000000L);
			if (num5 != 0)
			{
				int num6 = 7;
				while (num5 % 10 == 0)
				{
					num6--;
					num5 /= 10;
				}
				chars[start + 19] = '.';
				DateTimeUtils.CopyIntToCharArray(chars, start + 20, num5, num6);
				num += num6 + 1;
			}
			return start + num;
		}

		// Token: 0x060004B0 RID: 1200 RVA: 0x000139DD File Offset: 0x00011BDD
		private static void CopyIntToCharArray(char[] chars, int start, int value, int digits)
		{
			while (digits-- != 0)
			{
				chars[start + digits] = (char)(value % 10 + 48);
				value /= 10;
			}
		}

		// Token: 0x060004B1 RID: 1201 RVA: 0x000139FC File Offset: 0x00011BFC
		internal static int WriteDateTimeOffset(char[] chars, int start, TimeSpan offset, DateFormatHandling format)
		{
			chars[start++] = ((offset.Ticks >= 0L) ? '+' : '-');
			int num = Math.Abs(offset.Hours);
			DateTimeUtils.CopyIntToCharArray(chars, start, num, 2);
			start += 2;
			if (format == DateFormatHandling.IsoDateFormat)
			{
				chars[start++] = ':';
			}
			int num2 = Math.Abs(offset.Minutes);
			DateTimeUtils.CopyIntToCharArray(chars, start, num2, 2);
			start += 2;
			return start;
		}

		// Token: 0x060004B2 RID: 1202 RVA: 0x00013A68 File Offset: 0x00011C68
		internal static void WriteDateTimeOffsetString(TextWriter writer, DateTimeOffset value, DateFormatHandling format, [Nullable(2)] string formatString, CultureInfo culture)
		{
			if (StringUtils.IsNullOrEmpty(formatString))
			{
				char[] array = new char[64];
				int num = DateTimeUtils.WriteDateTimeString(array, 0, (format == DateFormatHandling.IsoDateFormat) ? value.DateTime : value.UtcDateTime, new TimeSpan?(value.Offset), DateTimeKind.Local, format);
				writer.Write(array, 0, num);
				return;
			}
			writer.Write(value.ToString(formatString, culture));
		}

		// Token: 0x060004B3 RID: 1203 RVA: 0x00013AC8 File Offset: 0x00011CC8
		private static void GetDateValues(DateTime td, out int year, out int month, out int day)
		{
			int i = (int)(td.Ticks / 864000000000L);
			int num = i / 146097;
			i -= num * 146097;
			int num2 = i / 36524;
			if (num2 == 4)
			{
				num2 = 3;
			}
			i -= num2 * 36524;
			int num3 = i / 1461;
			i -= num3 * 1461;
			int num4 = i / 365;
			if (num4 == 4)
			{
				num4 = 3;
			}
			year = num * 400 + num2 * 100 + num3 * 4 + num4 + 1;
			i -= num4 * 365;
			int[] array = ((num4 == 3 && (num3 != 24 || num2 == 3)) ? DateTimeUtils.DaysToMonth366 : DateTimeUtils.DaysToMonth365);
			int num5 = i >> 6;
			while (i >= array[num5])
			{
				num5++;
			}
			month = num5;
			day = i - array[num5 - 1] + 1;
		}

		// Token: 0x040001CC RID: 460
		internal static readonly long InitialJavaScriptDateTicks = 621355968000000000L;

		// Token: 0x040001CD RID: 461
		private const string IsoDateFormat = "yyyy-MM-ddTHH:mm:ss.FFFFFFFK";

		// Token: 0x040001CE RID: 462
		private const int DaysPer100Years = 36524;

		// Token: 0x040001CF RID: 463
		private const int DaysPer400Years = 146097;

		// Token: 0x040001D0 RID: 464
		private const int DaysPer4Years = 1461;

		// Token: 0x040001D1 RID: 465
		private const int DaysPerYear = 365;

		// Token: 0x040001D2 RID: 466
		private const long TicksPerDay = 864000000000L;

		// Token: 0x040001D3 RID: 467
		private static readonly int[] DaysToMonth365 = new int[]
		{
			0, 31, 59, 90, 120, 151, 181, 212, 243, 273,
			304, 334, 365
		};

		// Token: 0x040001D4 RID: 468
		private static readonly int[] DaysToMonth366 = new int[]
		{
			0, 31, 60, 91, 121, 152, 182, 213, 244, 274,
			305, 335, 366
		};
	}
}
