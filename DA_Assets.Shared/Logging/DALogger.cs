using System;
using UnityEngine;

namespace DA_Assets.Logging
{
	// Token: 0x02000021 RID: 33
	public static class DALogger
	{
		// Token: 0x0600006C RID: 108 RVA: 0x00003422 File Offset: 0x00001622
		public static void LogException(Exception ex)
		{
			Debug.LogException(ex);
		}

		// Token: 0x0600006D RID: 109 RVA: 0x0000342A File Offset: 0x0000162A
		public static void LogError(string log)
		{
			log = DALogger.SubstringSafe(log, 15000);
			Debug.LogError(log);
		}

		// Token: 0x0600006E RID: 110 RVA: 0x0000343F File Offset: 0x0000163F
		public static void LogWarning(string log)
		{
			log = DALogger.SubstringSafe(log, 15000);
			Debug.LogWarning(log);
		}

		// Token: 0x0600006F RID: 111 RVA: 0x00003454 File Offset: 0x00001654
		public static void Log(string log)
		{
			log = DALogger.SubstringSafe(log, 15000);
			Debug.Log(log.TextBold());
		}

		// Token: 0x06000070 RID: 112 RVA: 0x0000346E File Offset: 0x0000166E
		public static void LogSuccess(string log)
		{
			Debug.Log(log.TextColor(DALogger.violetColor).TextBold());
		}

		// Token: 0x06000071 RID: 113 RVA: 0x00003485 File Offset: 0x00001685
		public static string SubstringSafe(string value, int maxLength)
		{
			if (value == null || value.Length <= maxLength)
			{
				return value;
			}
			return value.Substring(0, maxLength);
		}

		// Token: 0x04000049 RID: 73
		public static string violetColor = "#8b00ff";
	}
}
