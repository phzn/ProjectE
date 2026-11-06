using System;
using System.Collections;
using DA_Assets.Logging;
using UnityEngine;

namespace DA_Assets.FCU
{
	// Token: 0x02000029 RID: 41
	public class FcuLogger
	{
		// Token: 0x06000090 RID: 144 RVA: 0x0000346F File Offset: 0x0000166F
		public static void Debug(object log, FcuDebugSettingsFlags logType = FcuDebugSettingsFlags.LogDefault)
		{
			if (!FcuDebugSettings.Settings.HasFlag(logType))
			{
				return;
			}
			global::UnityEngine.Debug.Log(log);
		}

		// Token: 0x06000091 RID: 145 RVA: 0x0000348F File Offset: 0x0000168F
		public static bool WriteLogBeforeApiTimeout(ref int requestCount, ref int remainingTime, string log)
		{
			if (requestCount != 0 && requestCount % FcuConfig.ApiRequestsCountLimit == 0)
			{
				if (remainingTime > 0)
				{
					DALogger.Log(log);
					remainingTime -= 10;
				}
				else if (remainingTime == 0)
				{
					DALogger.Log(log);
				}
			}
			return remainingTime > 0;
		}

		// Token: 0x06000092 RID: 146 RVA: 0x000034C4 File Offset: 0x000016C4
		public static bool WriteLogBeforeEqual(ICollection list1, ICollection list2, FcuLocKey locKey, int count1, int count2, ref int tempCount)
		{
			if (list1.Count != list2.Count)
			{
				if (tempCount != list1.Count)
				{
					tempCount = list1.Count;
					DALogger.Log(locKey.Localize(new object[] { count1, count2 }));
				}
				return true;
			}
			if (tempCount != list2.Count)
			{
				DALogger.Log(locKey.Localize(new object[] { count1, count2 }));
			}
			return false;
		}

		// Token: 0x06000093 RID: 147 RVA: 0x00003549 File Offset: 0x00001749
		public static bool WriteLogBeforeEqual(ref int count1, ref int count2, string log, ref int tempCount)
		{
			if (count1 != count2)
			{
				if (tempCount != count1)
				{
					tempCount = count1;
					DALogger.Log(log);
				}
				return true;
			}
			if (tempCount != count2)
			{
				DALogger.Log(log);
			}
			return false;
		}

		// Token: 0x06000094 RID: 148 RVA: 0x00003570 File Offset: 0x00001770
		public static bool WriteLogBeforeEqual(ICollection list1, ICollection list2, FcuLocKey locKey, ref int tempCount)
		{
			if (list1.Count != list2.Count)
			{
				if (tempCount != list1.Count)
				{
					tempCount = list1.Count;
					DALogger.Log(locKey.Localize(new object[] { list1.Count, list2.Count }));
				}
				return true;
			}
			if (tempCount != list2.Count)
			{
				DALogger.Log(locKey.Localize(new object[] { list1.Count, list2.Count }));
			}
			return false;
		}
	}
}
