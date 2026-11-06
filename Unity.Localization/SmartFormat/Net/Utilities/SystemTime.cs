using System;

namespace UnityEngine.Localization.SmartFormat.Net.Utilities
{
	// Token: 0x02000042 RID: 66
	internal static class SystemTime
	{
		// Token: 0x0600025A RID: 602 RVA: 0x0000855D File Offset: 0x0000675D
		public static void SetDateTime(DateTime dateTimeNow)
		{
			SystemTime.Now = () => dateTimeNow;
		}

		// Token: 0x0600025B RID: 603 RVA: 0x0000857B File Offset: 0x0000677B
		public static void SetDateTimeOffset(DateTimeOffset dateTimeOffset)
		{
			SystemTime.OffsetNow = () => dateTimeOffset;
		}

		// Token: 0x0600025C RID: 604 RVA: 0x0000859C File Offset: 0x0000679C
		public static void ResetDateTime()
		{
			SystemTime.Now = () => DateTime.Now;
			SystemTime.OffsetNow = () => DateTimeOffset.Now;
		}

		// Token: 0x0400008C RID: 140
		public static Func<DateTime> Now = () => DateTime.Now;

		// Token: 0x0400008D RID: 141
		public static Func<DateTimeOffset> OffsetNow = () => DateTimeOffset.Now;
	}
}
