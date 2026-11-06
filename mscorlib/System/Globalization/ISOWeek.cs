using System;
using System.Runtime.CompilerServices;

namespace System.Globalization
{
	// Token: 0x0200097D RID: 2429
	public static class ISOWeek
	{
		// Token: 0x0600559C RID: 21916 RVA: 0x00121094 File Offset: 0x0011F294
		public static int GetWeekOfYear(DateTime date)
		{
			int weekNumber = ISOWeek.GetWeekNumber(date);
			if (weekNumber < 1)
			{
				return ISOWeek.GetWeeksInYear(date.Year - 1);
			}
			if (weekNumber > ISOWeek.GetWeeksInYear(date.Year))
			{
				return 1;
			}
			return weekNumber;
		}

		// Token: 0x0600559D RID: 21917 RVA: 0x001210D0 File Offset: 0x0011F2D0
		public static int GetYear(DateTime date)
		{
			int weekNumber = ISOWeek.GetWeekNumber(date);
			if (weekNumber < 1)
			{
				return date.Year - 1;
			}
			if (weekNumber > ISOWeek.GetWeeksInYear(date.Year))
			{
				return date.Year + 1;
			}
			return date.Year;
		}

		// Token: 0x0600559E RID: 21918 RVA: 0x00121112 File Offset: 0x0011F312
		public static DateTime GetYearStart(int year)
		{
			return ISOWeek.ToDateTime(year, 1, DayOfWeek.Monday);
		}

		// Token: 0x0600559F RID: 21919 RVA: 0x0012111C File Offset: 0x0011F31C
		public static DateTime GetYearEnd(int year)
		{
			return ISOWeek.ToDateTime(year, ISOWeek.GetWeeksInYear(year), DayOfWeek.Sunday);
		}

		// Token: 0x060055A0 RID: 21920 RVA: 0x0012112B File Offset: 0x0011F32B
		public static int GetWeeksInYear(int year)
		{
			if (year < 1 || year > 9999)
			{
				throw new ArgumentOutOfRangeException("year", "Year must be between 1 and 9999.");
			}
			if (ISOWeek.<GetWeeksInYear>g__P|8_0(year) == 4 || ISOWeek.<GetWeeksInYear>g__P|8_0(year - 1) == 3)
			{
				return 53;
			}
			return 52;
		}

		// Token: 0x060055A1 RID: 21921 RVA: 0x00121164 File Offset: 0x0011F364
		public static DateTime ToDateTime(int year, int week, DayOfWeek dayOfWeek)
		{
			if (year < 1 || year > 9999)
			{
				throw new ArgumentOutOfRangeException("year", "Year must be between 1 and 9999.");
			}
			if (week < 1 || week > 53)
			{
				throw new ArgumentOutOfRangeException("week", "The week parameter must be in the range 1 through 53.");
			}
			if (dayOfWeek < DayOfWeek.Sunday || dayOfWeek > (DayOfWeek)7)
			{
				throw new ArgumentOutOfRangeException("dayOfWeek", "The DayOfWeek enumeration must be in the range 0 through 6.");
			}
			DateTime dateTime = new DateTime(year, 1, 4);
			int num = ISOWeek.GetWeekday(dateTime.DayOfWeek) + 3;
			int num2 = week * 7 + ISOWeek.GetWeekday(dayOfWeek) - num;
			return new DateTime(year, 1, 1).AddDays((double)(num2 - 1));
		}

		// Token: 0x060055A2 RID: 21922 RVA: 0x001211F8 File Offset: 0x0011F3F8
		private static int GetWeekNumber(DateTime date)
		{
			return (date.DayOfYear - ISOWeek.GetWeekday(date.DayOfWeek) + 10) / 7;
		}

		// Token: 0x060055A3 RID: 21923 RVA: 0x00121213 File Offset: 0x0011F413
		private static int GetWeekday(DayOfWeek dayOfWeek)
		{
			if (dayOfWeek != DayOfWeek.Sunday)
			{
				return (int)dayOfWeek;
			}
			return 7;
		}

		// Token: 0x060055A4 RID: 21924 RVA: 0x0012121B File Offset: 0x0011F41B
		[CompilerGenerated]
		internal static int <GetWeeksInYear>g__P|8_0(int y)
		{
			return (y + y / 4 - y / 100 + y / 400) % 7;
		}

		// Token: 0x0400354F RID: 13647
		private const int WeeksInLongYear = 53;

		// Token: 0x04003550 RID: 13648
		private const int WeeksInShortYear = 52;

		// Token: 0x04003551 RID: 13649
		private const int MinWeek = 1;

		// Token: 0x04003552 RID: 13650
		private const int MaxWeek = 53;
	}
}
