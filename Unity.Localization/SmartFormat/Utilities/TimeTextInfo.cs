using System;

namespace UnityEngine.Localization.SmartFormat.Utilities
{
	// Token: 0x02000048 RID: 72
	public class TimeTextInfo
	{
		// Token: 0x06000286 RID: 646 RVA: 0x00009C10 File Offset: 0x00007E10
		public TimeTextInfo(PluralRules.PluralRuleDelegate pluralRule, string[] week, string[] day, string[] hour, string[] minute, string[] second, string[] millisecond, string[] w, string[] d, string[] h, string[] m, string[] s, string[] ms, string lessThan)
		{
			this.PluralRule = pluralRule;
			this.week = week;
			this.day = day;
			this.hour = hour;
			this.minute = minute;
			this.second = second;
			this.millisecond = millisecond;
			this.w = w;
			this.d = d;
			this.h = h;
			this.m = m;
			this.s = s;
			this.ms = ms;
			this.lessThan = lessThan;
		}

		// Token: 0x06000287 RID: 647 RVA: 0x00009C90 File Offset: 0x00007E90
		public TimeTextInfo(string week, string day, string hour, string minute, string second, string millisecond, string lessThan)
		{
			this.d = (this.h = (this.m = (this.ms = (this.s = (this.w = new string[0])))));
			this.PluralRule = (decimal d, int c) => 0;
			this.week = new string[] { week };
			this.day = new string[] { day };
			this.hour = new string[] { hour };
			this.minute = new string[] { minute };
			this.second = new string[] { second };
			this.millisecond = new string[] { millisecond };
			this.lessThan = lessThan;
		}

		// Token: 0x06000288 RID: 648 RVA: 0x00009D6C File Offset: 0x00007F6C
		private static string GetValue(PluralRules.PluralRuleDelegate pluralRule, int value, string[] units)
		{
			int num = ((units.Length == 1) ? 0 : pluralRule(value, units.Length));
			return string.Format(units[num], value);
		}

		// Token: 0x06000289 RID: 649 RVA: 0x00009DA0 File Offset: 0x00007FA0
		public string GetLessThanText(string minimumValue)
		{
			return string.Format(this.lessThan, minimumValue);
		}

		// Token: 0x0600028A RID: 650 RVA: 0x00009DB0 File Offset: 0x00007FB0
		public virtual string GetUnitText(TimeSpanFormatOptions unit, int value, bool abbr)
		{
			if (unit <= TimeSpanFormatOptions.RangeMinutes)
			{
				if (unit == TimeSpanFormatOptions.RangeMilliSeconds)
				{
					return TimeTextInfo.GetValue(this.PluralRule, value, abbr ? this.ms : this.millisecond);
				}
				if (unit == TimeSpanFormatOptions.RangeSeconds)
				{
					return TimeTextInfo.GetValue(this.PluralRule, value, abbr ? this.s : this.second);
				}
				if (unit == TimeSpanFormatOptions.RangeMinutes)
				{
					return TimeTextInfo.GetValue(this.PluralRule, value, abbr ? this.m : this.minute);
				}
			}
			else
			{
				if (unit == TimeSpanFormatOptions.RangeHours)
				{
					return TimeTextInfo.GetValue(this.PluralRule, value, abbr ? this.h : this.hour);
				}
				if (unit == TimeSpanFormatOptions.RangeDays)
				{
					return TimeTextInfo.GetValue(this.PluralRule, value, abbr ? this.d : this.day);
				}
				if (unit == TimeSpanFormatOptions.RangeWeeks)
				{
					return TimeTextInfo.GetValue(this.PluralRule, value, abbr ? this.w : this.week);
				}
			}
			return null;
		}

		// Token: 0x040000A8 RID: 168
		private readonly string[] d;

		// Token: 0x040000A9 RID: 169
		private readonly string[] day;

		// Token: 0x040000AA RID: 170
		private readonly string[] h;

		// Token: 0x040000AB RID: 171
		private readonly string[] hour;

		// Token: 0x040000AC RID: 172
		private readonly string lessThan;

		// Token: 0x040000AD RID: 173
		private readonly string[] m;

		// Token: 0x040000AE RID: 174
		private readonly string[] millisecond;

		// Token: 0x040000AF RID: 175
		private readonly string[] minute;

		// Token: 0x040000B0 RID: 176
		private readonly string[] ms;

		// Token: 0x040000B1 RID: 177
		private readonly PluralRules.PluralRuleDelegate PluralRule;

		// Token: 0x040000B2 RID: 178
		private readonly string[] s;

		// Token: 0x040000B3 RID: 179
		private readonly string[] second;

		// Token: 0x040000B4 RID: 180
		private readonly string[] w;

		// Token: 0x040000B5 RID: 181
		private readonly string[] week;
	}
}
