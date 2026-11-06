using System;
using System.Runtime.Serialization;

namespace System.Globalization
{
	// Token: 0x0200098A RID: 2442
	[Serializable]
	internal class GregorianCalendarHelper
	{
		// Token: 0x17000E64 RID: 3684
		// (get) Token: 0x060056A2 RID: 22178 RVA: 0x00124F51 File Offset: 0x00123151
		internal int MaxYear
		{
			get
			{
				return this.m_maxYear;
			}
		}

		// Token: 0x060056A3 RID: 22179 RVA: 0x00124F5C File Offset: 0x0012315C
		internal GregorianCalendarHelper(Calendar cal, EraInfo[] eraInfo)
		{
			this.m_Cal = cal;
			this.m_EraInfo = eraInfo;
			this.m_minDate = this.m_Cal.MinSupportedDateTime;
			this.m_maxYear = this.m_EraInfo[0].maxEraYear;
			this.m_minYear = this.m_EraInfo[0].minEraYear;
		}

		// Token: 0x060056A4 RID: 22180 RVA: 0x00124FC0 File Offset: 0x001231C0
		private int GetYearOffset(int year, int era, bool throwOnError)
		{
			if (year < 0)
			{
				if (throwOnError)
				{
					throw new ArgumentOutOfRangeException("year", "Non-negative number required.");
				}
				return -1;
			}
			else
			{
				if (era == 0)
				{
					era = this.m_Cal.CurrentEraValue;
				}
				int i = 0;
				while (i < this.m_EraInfo.Length)
				{
					if (era == this.m_EraInfo[i].era)
					{
						if (year >= this.m_EraInfo[i].minEraYear)
						{
							if (year <= this.m_EraInfo[i].maxEraYear)
							{
								return this.m_EraInfo[i].yearOffset;
							}
							if (!AppContextSwitches.EnforceJapaneseEraYearRanges)
							{
								int num = year - this.m_EraInfo[i].maxEraYear;
								for (int j = i - 1; j >= 0; j--)
								{
									if (num <= this.m_EraInfo[j].maxEraYear)
									{
										return this.m_EraInfo[i].yearOffset;
									}
									num -= this.m_EraInfo[j].maxEraYear;
								}
							}
						}
						if (throwOnError)
						{
							throw new ArgumentOutOfRangeException("year", string.Format(CultureInfo.CurrentCulture, "Valid values are between {0} and {1}, inclusive.", this.m_EraInfo[i].minEraYear, this.m_EraInfo[i].maxEraYear));
						}
						break;
					}
					else
					{
						i++;
					}
				}
				if (throwOnError)
				{
					throw new ArgumentOutOfRangeException("era", "Era value was not valid.");
				}
				return -1;
			}
		}

		// Token: 0x060056A5 RID: 22181 RVA: 0x001250F8 File Offset: 0x001232F8
		internal int GetGregorianYear(int year, int era)
		{
			return this.GetYearOffset(year, era, true) + year;
		}

		// Token: 0x060056A6 RID: 22182 RVA: 0x00125105 File Offset: 0x00123305
		internal bool IsValidYear(int year, int era)
		{
			return this.GetYearOffset(year, era, false) >= 0;
		}

		// Token: 0x060056A7 RID: 22183 RVA: 0x00125118 File Offset: 0x00123318
		internal virtual int GetDatePart(long ticks, int part)
		{
			this.CheckTicksRange(ticks);
			int i = (int)(ticks / 864000000000L);
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
			if (part == 0)
			{
				return num * 400 + num2 * 100 + num3 * 4 + num4 + 1;
			}
			i -= num4 * 365;
			if (part == 1)
			{
				return i + 1;
			}
			int[] array = ((num4 == 3 && (num3 != 24 || num2 == 3)) ? GregorianCalendarHelper.DaysToMonth366 : GregorianCalendarHelper.DaysToMonth365);
			int num5 = i >> 6;
			while (i >= array[num5])
			{
				num5++;
			}
			if (part == 2)
			{
				return num5;
			}
			return i - array[num5 - 1] + 1;
		}

		// Token: 0x060056A8 RID: 22184 RVA: 0x00125200 File Offset: 0x00123400
		internal static long GetAbsoluteDate(int year, int month, int day)
		{
			if (year >= 1 && year <= 9999 && month >= 1 && month <= 12)
			{
				int[] array = ((year % 4 == 0 && (year % 100 != 0 || year % 400 == 0)) ? GregorianCalendarHelper.DaysToMonth366 : GregorianCalendarHelper.DaysToMonth365);
				if (day >= 1 && day <= array[month] - array[month - 1])
				{
					int num = year - 1;
					return (long)(num * 365 + num / 4 - num / 100 + num / 400 + array[month - 1] + day - 1);
				}
			}
			throw new ArgumentOutOfRangeException(null, Environment.GetResourceString("Year, Month, and Day parameters describe an un-representable DateTime."));
		}

		// Token: 0x060056A9 RID: 22185 RVA: 0x0012528B File Offset: 0x0012348B
		internal static long DateToTicks(int year, int month, int day)
		{
			return GregorianCalendarHelper.GetAbsoluteDate(year, month, day) * 864000000000L;
		}

		// Token: 0x060056AA RID: 22186 RVA: 0x001252A0 File Offset: 0x001234A0
		internal static long TimeToTicks(int hour, int minute, int second, int millisecond)
		{
			if (hour < 0 || hour >= 24 || minute < 0 || minute >= 60 || second < 0 || second >= 60)
			{
				throw new ArgumentOutOfRangeException(null, Environment.GetResourceString("Hour, Minute, and Second parameters describe an un-representable DateTime."));
			}
			if (millisecond < 0 || millisecond >= 1000)
			{
				throw new ArgumentOutOfRangeException("millisecond", string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Valid values are between {0} and {1}, inclusive."), 0, 999));
			}
			return TimeSpan.TimeToTicks(hour, minute, second) + (long)millisecond * 10000L;
		}

		// Token: 0x060056AB RID: 22187 RVA: 0x00125328 File Offset: 0x00123528
		internal void CheckTicksRange(long ticks)
		{
			if (ticks < this.m_Cal.MinSupportedDateTime.Ticks || ticks > this.m_Cal.MaxSupportedDateTime.Ticks)
			{
				throw new ArgumentOutOfRangeException("time", string.Format(CultureInfo.InvariantCulture, Environment.GetResourceString("Specified time is not supported in this calendar. It should be between {0} (Gregorian date) and {1} (Gregorian date), inclusive."), this.m_Cal.MinSupportedDateTime, this.m_Cal.MaxSupportedDateTime));
			}
		}

		// Token: 0x060056AC RID: 22188 RVA: 0x001253A0 File Offset: 0x001235A0
		public DateTime AddMonths(DateTime time, int months)
		{
			if (months < -120000 || months > 120000)
			{
				throw new ArgumentOutOfRangeException("months", string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Valid values are between {0} and {1}, inclusive."), -120000, 120000));
			}
			this.CheckTicksRange(time.Ticks);
			int num = this.GetDatePart(time.Ticks, 0);
			int num2 = this.GetDatePart(time.Ticks, 2);
			int num3 = this.GetDatePart(time.Ticks, 3);
			int num4 = num2 - 1 + months;
			if (num4 >= 0)
			{
				num2 = num4 % 12 + 1;
				num += num4 / 12;
			}
			else
			{
				num2 = 12 + (num4 + 1) % 12;
				num += (num4 - 11) / 12;
			}
			int[] array = ((num % 4 == 0 && (num % 100 != 0 || num % 400 == 0)) ? GregorianCalendarHelper.DaysToMonth366 : GregorianCalendarHelper.DaysToMonth365);
			int num5 = array[num2] - array[num2 - 1];
			if (num3 > num5)
			{
				num3 = num5;
			}
			long num6 = GregorianCalendarHelper.DateToTicks(num, num2, num3) + time.Ticks % 864000000000L;
			Calendar.CheckAddResult(num6, this.m_Cal.MinSupportedDateTime, this.m_Cal.MaxSupportedDateTime);
			return new DateTime(num6);
		}

		// Token: 0x060056AD RID: 22189 RVA: 0x001254CA File Offset: 0x001236CA
		public DateTime AddYears(DateTime time, int years)
		{
			return this.AddMonths(time, years * 12);
		}

		// Token: 0x060056AE RID: 22190 RVA: 0x001254D7 File Offset: 0x001236D7
		public int GetDayOfMonth(DateTime time)
		{
			return this.GetDatePart(time.Ticks, 3);
		}

		// Token: 0x060056AF RID: 22191 RVA: 0x001254E7 File Offset: 0x001236E7
		public DayOfWeek GetDayOfWeek(DateTime time)
		{
			this.CheckTicksRange(time.Ticks);
			return (DayOfWeek)((time.Ticks / 864000000000L + 1L) % 7L);
		}

		// Token: 0x060056B0 RID: 22192 RVA: 0x0012550E File Offset: 0x0012370E
		public int GetDayOfYear(DateTime time)
		{
			return this.GetDatePart(time.Ticks, 1);
		}

		// Token: 0x060056B1 RID: 22193 RVA: 0x00125520 File Offset: 0x00123720
		public int GetDaysInMonth(int year, int month, int era)
		{
			year = this.GetGregorianYear(year, era);
			if (month < 1 || month > 12)
			{
				throw new ArgumentOutOfRangeException("month", Environment.GetResourceString("Month must be between one and twelve."));
			}
			int[] array = ((year % 4 == 0 && (year % 100 != 0 || year % 400 == 0)) ? GregorianCalendarHelper.DaysToMonth366 : GregorianCalendarHelper.DaysToMonth365);
			return array[month] - array[month - 1];
		}

		// Token: 0x060056B2 RID: 22194 RVA: 0x0012557F File Offset: 0x0012377F
		public int GetDaysInYear(int year, int era)
		{
			year = this.GetGregorianYear(year, era);
			if (year % 4 != 0 || (year % 100 == 0 && year % 400 != 0))
			{
				return 365;
			}
			return 366;
		}

		// Token: 0x060056B3 RID: 22195 RVA: 0x001255AC File Offset: 0x001237AC
		public int GetEra(DateTime time)
		{
			long ticks = time.Ticks;
			for (int i = 0; i < this.m_EraInfo.Length; i++)
			{
				if (ticks >= this.m_EraInfo[i].ticks)
				{
					return this.m_EraInfo[i].era;
				}
			}
			throw new ArgumentOutOfRangeException(Environment.GetResourceString("Time value was out of era range."));
		}

		// Token: 0x17000E65 RID: 3685
		// (get) Token: 0x060056B4 RID: 22196 RVA: 0x00125604 File Offset: 0x00123804
		public int[] Eras
		{
			get
			{
				if (this.m_eras == null)
				{
					this.m_eras = new int[this.m_EraInfo.Length];
					for (int i = 0; i < this.m_EraInfo.Length; i++)
					{
						this.m_eras[i] = this.m_EraInfo[i].era;
					}
				}
				return (int[])this.m_eras.Clone();
			}
		}

		// Token: 0x060056B5 RID: 22197 RVA: 0x00125664 File Offset: 0x00123864
		public int GetMonth(DateTime time)
		{
			return this.GetDatePart(time.Ticks, 2);
		}

		// Token: 0x060056B6 RID: 22198 RVA: 0x00125674 File Offset: 0x00123874
		public int GetMonthsInYear(int year, int era)
		{
			year = this.GetGregorianYear(year, era);
			return 12;
		}

		// Token: 0x060056B7 RID: 22199 RVA: 0x00125684 File Offset: 0x00123884
		public int GetYear(DateTime time)
		{
			long ticks = time.Ticks;
			int datePart = this.GetDatePart(ticks, 0);
			for (int i = 0; i < this.m_EraInfo.Length; i++)
			{
				if (ticks >= this.m_EraInfo[i].ticks)
				{
					return datePart - this.m_EraInfo[i].yearOffset;
				}
			}
			throw new ArgumentException(Environment.GetResourceString("No Era was supplied."));
		}

		// Token: 0x060056B8 RID: 22200 RVA: 0x001256E4 File Offset: 0x001238E4
		public int GetYear(int year, DateTime time)
		{
			long ticks = time.Ticks;
			for (int i = 0; i < this.m_EraInfo.Length; i++)
			{
				if (ticks >= this.m_EraInfo[i].ticks)
				{
					return year - this.m_EraInfo[i].yearOffset;
				}
			}
			throw new ArgumentException(Environment.GetResourceString("No Era was supplied."));
		}

		// Token: 0x060056B9 RID: 22201 RVA: 0x0012573C File Offset: 0x0012393C
		public bool IsLeapDay(int year, int month, int day, int era)
		{
			if (day < 1 || day > this.GetDaysInMonth(year, month, era))
			{
				throw new ArgumentOutOfRangeException("day", string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Valid values are between {0} and {1}, inclusive."), 1, this.GetDaysInMonth(year, month, era)));
			}
			return this.IsLeapYear(year, era) && (month == 2 && day == 29);
		}

		// Token: 0x060056BA RID: 22202 RVA: 0x001257A7 File Offset: 0x001239A7
		public int GetLeapMonth(int year, int era)
		{
			year = this.GetGregorianYear(year, era);
			return 0;
		}

		// Token: 0x060056BB RID: 22203 RVA: 0x001257B4 File Offset: 0x001239B4
		public bool IsLeapMonth(int year, int month, int era)
		{
			year = this.GetGregorianYear(year, era);
			if (month < 1 || month > 12)
			{
				throw new ArgumentOutOfRangeException("month", string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Valid values are between {0} and {1}, inclusive."), 1, 12));
			}
			return false;
		}

		// Token: 0x060056BC RID: 22204 RVA: 0x00125801 File Offset: 0x00123A01
		public bool IsLeapYear(int year, int era)
		{
			year = this.GetGregorianYear(year, era);
			return year % 4 == 0 && (year % 100 != 0 || year % 400 == 0);
		}

		// Token: 0x060056BD RID: 22205 RVA: 0x00125828 File Offset: 0x00123A28
		public DateTime ToDateTime(int year, int month, int day, int hour, int minute, int second, int millisecond, int era)
		{
			year = this.GetGregorianYear(year, era);
			long num = GregorianCalendarHelper.DateToTicks(year, month, day) + GregorianCalendarHelper.TimeToTicks(hour, minute, second, millisecond);
			this.CheckTicksRange(num);
			return new DateTime(num);
		}

		// Token: 0x060056BE RID: 22206 RVA: 0x00125864 File Offset: 0x00123A64
		public virtual int GetWeekOfYear(DateTime time, CalendarWeekRule rule, DayOfWeek firstDayOfWeek)
		{
			this.CheckTicksRange(time.Ticks);
			return GregorianCalendar.GetDefaultInstance().GetWeekOfYear(time, rule, firstDayOfWeek);
		}

		// Token: 0x060056BF RID: 22207 RVA: 0x00125880 File Offset: 0x00123A80
		public int ToFourDigitYear(int year, int twoDigitYearMax)
		{
			if (year < 0)
			{
				throw new ArgumentOutOfRangeException("year", Environment.GetResourceString("Positive number required."));
			}
			if (year < 100)
			{
				int num = year % 100;
				return (twoDigitYearMax / 100 - ((num > twoDigitYearMax % 100) ? 1 : 0)) * 100 + num;
			}
			if (year < this.m_minYear || year > this.m_maxYear)
			{
				throw new ArgumentOutOfRangeException("year", string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Valid values are between {0} and {1}, inclusive."), this.m_minYear, this.m_maxYear));
			}
			return year;
		}

		// Token: 0x040035F4 RID: 13812
		internal const long TicksPerMillisecond = 10000L;

		// Token: 0x040035F5 RID: 13813
		internal const long TicksPerSecond = 10000000L;

		// Token: 0x040035F6 RID: 13814
		internal const long TicksPerMinute = 600000000L;

		// Token: 0x040035F7 RID: 13815
		internal const long TicksPerHour = 36000000000L;

		// Token: 0x040035F8 RID: 13816
		internal const long TicksPerDay = 864000000000L;

		// Token: 0x040035F9 RID: 13817
		internal const int MillisPerSecond = 1000;

		// Token: 0x040035FA RID: 13818
		internal const int MillisPerMinute = 60000;

		// Token: 0x040035FB RID: 13819
		internal const int MillisPerHour = 3600000;

		// Token: 0x040035FC RID: 13820
		internal const int MillisPerDay = 86400000;

		// Token: 0x040035FD RID: 13821
		internal const int DaysPerYear = 365;

		// Token: 0x040035FE RID: 13822
		internal const int DaysPer4Years = 1461;

		// Token: 0x040035FF RID: 13823
		internal const int DaysPer100Years = 36524;

		// Token: 0x04003600 RID: 13824
		internal const int DaysPer400Years = 146097;

		// Token: 0x04003601 RID: 13825
		internal const int DaysTo10000 = 3652059;

		// Token: 0x04003602 RID: 13826
		internal const long MaxMillis = 315537897600000L;

		// Token: 0x04003603 RID: 13827
		internal const int DatePartYear = 0;

		// Token: 0x04003604 RID: 13828
		internal const int DatePartDayOfYear = 1;

		// Token: 0x04003605 RID: 13829
		internal const int DatePartMonth = 2;

		// Token: 0x04003606 RID: 13830
		internal const int DatePartDay = 3;

		// Token: 0x04003607 RID: 13831
		internal static readonly int[] DaysToMonth365 = new int[]
		{
			0, 31, 59, 90, 120, 151, 181, 212, 243, 273,
			304, 334, 365
		};

		// Token: 0x04003608 RID: 13832
		internal static readonly int[] DaysToMonth366 = new int[]
		{
			0, 31, 60, 91, 121, 152, 182, 213, 244, 274,
			305, 335, 366
		};

		// Token: 0x04003609 RID: 13833
		[OptionalField(VersionAdded = 1)]
		internal int m_maxYear = 9999;

		// Token: 0x0400360A RID: 13834
		[OptionalField(VersionAdded = 1)]
		internal int m_minYear;

		// Token: 0x0400360B RID: 13835
		internal Calendar m_Cal;

		// Token: 0x0400360C RID: 13836
		[OptionalField(VersionAdded = 1)]
		internal EraInfo[] m_EraInfo;

		// Token: 0x0400360D RID: 13837
		[OptionalField(VersionAdded = 1)]
		internal int[] m_eras;

		// Token: 0x0400360E RID: 13838
		[OptionalField(VersionAdded = 1)]
		internal DateTime m_minDate;
	}
}
