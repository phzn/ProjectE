using System;
using System.Collections;
using System.Globalization;

namespace System
{
	// Token: 0x02000109 RID: 265
	[Obsolete("System.CurrentSystemTimeZone has been deprecated.  Please investigate the use of System.TimeZoneInfo.Local instead.")]
	[Serializable]
	internal class CurrentSystemTimeZone : TimeZone
	{
		// Token: 0x06000984 RID: 2436 RVA: 0x00025534 File Offset: 0x00023734
		internal CurrentSystemTimeZone()
		{
			TimeZoneInfo local = TimeZoneInfo.Local;
			this.m_ticksOffset = local.BaseUtcOffset.Ticks;
			this.m_standardName = local.StandardName;
			this.m_daylightName = local.DaylightName;
		}

		// Token: 0x170000B3 RID: 179
		// (get) Token: 0x06000985 RID: 2437 RVA: 0x00025584 File Offset: 0x00023784
		public override string StandardName
		{
			get
			{
				return this.m_standardName;
			}
		}

		// Token: 0x170000B4 RID: 180
		// (get) Token: 0x06000986 RID: 2438 RVA: 0x0002558C File Offset: 0x0002378C
		public override string DaylightName
		{
			get
			{
				return this.m_daylightName;
			}
		}

		// Token: 0x06000987 RID: 2439 RVA: 0x00025594 File Offset: 0x00023794
		internal long GetUtcOffsetFromUniversalTime(DateTime time, ref bool isAmbiguousLocalDst)
		{
			TimeSpan timeSpan = new TimeSpan(this.m_ticksOffset);
			DaylightTime daylightChanges = this.GetDaylightChanges(time.Year);
			isAmbiguousLocalDst = false;
			if (daylightChanges == null || daylightChanges.Delta.Ticks == 0L)
			{
				return timeSpan.Ticks;
			}
			DateTime dateTime = daylightChanges.Start - timeSpan;
			DateTime dateTime2 = daylightChanges.End - timeSpan - daylightChanges.Delta;
			DateTime dateTime3;
			DateTime dateTime4;
			if (daylightChanges.Delta.Ticks > 0L)
			{
				dateTime3 = dateTime2 - daylightChanges.Delta;
				dateTime4 = dateTime2;
			}
			else
			{
				dateTime3 = dateTime;
				dateTime4 = dateTime - daylightChanges.Delta;
			}
			bool flag;
			if (dateTime > dateTime2)
			{
				flag = time < dateTime2 || time >= dateTime;
			}
			else
			{
				flag = time >= dateTime && time < dateTime2;
			}
			if (flag)
			{
				timeSpan += daylightChanges.Delta;
				if (time >= dateTime3 && time < dateTime4)
				{
					isAmbiguousLocalDst = true;
				}
			}
			return timeSpan.Ticks;
		}

		// Token: 0x06000988 RID: 2440 RVA: 0x000256A0 File Offset: 0x000238A0
		public override DateTime ToLocalTime(DateTime time)
		{
			if (time.Kind == DateTimeKind.Local)
			{
				return time;
			}
			bool flag = false;
			long utcOffsetFromUniversalTime = this.GetUtcOffsetFromUniversalTime(time, ref flag);
			long num = time.Ticks + utcOffsetFromUniversalTime;
			if (num > 3155378975999999999L)
			{
				return new DateTime(3155378975999999999L, DateTimeKind.Local);
			}
			if (num < 0L)
			{
				return new DateTime(0L, DateTimeKind.Local);
			}
			return new DateTime(num, DateTimeKind.Local, flag);
		}

		// Token: 0x06000989 RID: 2441 RVA: 0x00025701 File Offset: 0x00023901
		public override DaylightTime GetDaylightChanges(int year)
		{
			if (year < 1 || year > 9999)
			{
				throw new ArgumentOutOfRangeException("year", SR.Format("Valid values are between {0} and {1}, inclusive.", 1, 9999));
			}
			return this.GetCachedDaylightChanges(year);
		}

		// Token: 0x0600098A RID: 2442 RVA: 0x0002573C File Offset: 0x0002393C
		private static DaylightTime CreateDaylightChanges(int year)
		{
			DaylightTime daylightTime = null;
			if (TimeZoneInfo.Local.SupportsDaylightSavingTime)
			{
				foreach (TimeZoneInfo.AdjustmentRule adjustmentRule in TimeZoneInfo.Local.GetAdjustmentRules())
				{
					if (adjustmentRule.DateStart.Year <= year && adjustmentRule.DateEnd.Year >= year && adjustmentRule.DaylightDelta != TimeSpan.Zero)
					{
						DateTime dateTime = TimeZoneInfo.TransitionTimeToDateTime(year, adjustmentRule.DaylightTransitionStart);
						DateTime dateTime2 = TimeZoneInfo.TransitionTimeToDateTime(year, adjustmentRule.DaylightTransitionEnd);
						TimeSpan daylightDelta = adjustmentRule.DaylightDelta;
						daylightTime = new DaylightTime(dateTime, dateTime2, daylightDelta);
						break;
					}
				}
			}
			if (daylightTime == null)
			{
				daylightTime = new DaylightTime(DateTime.MinValue, DateTime.MinValue, TimeSpan.Zero);
			}
			return daylightTime;
		}

		// Token: 0x0600098B RID: 2443 RVA: 0x000257FC File Offset: 0x000239FC
		public override TimeSpan GetUtcOffset(DateTime time)
		{
			if (time.Kind == DateTimeKind.Utc)
			{
				return TimeSpan.Zero;
			}
			return new TimeSpan(TimeZone.CalculateUtcOffset(time, this.GetDaylightChanges(time.Year)).Ticks + this.m_ticksOffset);
		}

		// Token: 0x0600098C RID: 2444 RVA: 0x00025840 File Offset: 0x00023A40
		private DaylightTime GetCachedDaylightChanges(int year)
		{
			object obj = year;
			if (!this.m_CachedDaylightChanges.Contains(obj))
			{
				DaylightTime daylightTime = CurrentSystemTimeZone.CreateDaylightChanges(year);
				Hashtable cachedDaylightChanges = this.m_CachedDaylightChanges;
				lock (cachedDaylightChanges)
				{
					if (!this.m_CachedDaylightChanges.Contains(obj))
					{
						this.m_CachedDaylightChanges.Add(obj, daylightTime);
					}
				}
			}
			return (DaylightTime)this.m_CachedDaylightChanges[obj];
		}

		// Token: 0x0400108E RID: 4238
		private long m_ticksOffset;

		// Token: 0x0400108F RID: 4239
		private string m_standardName;

		// Token: 0x04001090 RID: 4240
		private string m_daylightName;

		// Token: 0x04001091 RID: 4241
		private readonly Hashtable m_CachedDaylightChanges = new Hashtable();
	}
}
