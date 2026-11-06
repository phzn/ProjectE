using System;

namespace UnityEngine.XR
{
	// Token: 0x02000011 RID: 17
	internal static class TimeConverter
	{
		// Token: 0x1700001E RID: 30
		// (get) Token: 0x06000079 RID: 121 RVA: 0x0000374C File Offset: 0x0000194C
		public static DateTime now
		{
			get
			{
				return DateTime.Now;
			}
		}

		// Token: 0x0600007A RID: 122 RVA: 0x00003764 File Offset: 0x00001964
		public static long LocalDateTimeToUnixTimeMilliseconds(DateTime date)
		{
			return Convert.ToInt64((date.ToUniversalTime() - TimeConverter.s_Epoch).TotalMilliseconds);
		}

		// Token: 0x0600007B RID: 123 RVA: 0x00003794 File Offset: 0x00001994
		public static DateTime UnixTimeMillisecondsToLocalDateTime(long unixTimeInMilliseconds)
		{
			return TimeConverter.s_Epoch.AddMilliseconds((double)unixTimeInMilliseconds).ToLocalTime();
		}

		// Token: 0x0400009C RID: 156
		private static readonly DateTime s_Epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
	}
}
