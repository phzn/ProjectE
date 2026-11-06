using System;
using UnityEngine;

namespace Pathfinding
{
	// Token: 0x02000040 RID: 64
	internal static class ThreadCountExtensions
	{
		// Token: 0x060002AB RID: 683 RVA: 0x0000C008 File Offset: 0x0000A208
		public static int ToConcreteThreadCount(this ThreadCount count)
		{
			if (count != ThreadCount.AutomaticLowLoad && count != ThreadCount.AutomaticHighLoad)
			{
				return (int)count;
			}
			int num = Mathf.Max(1, SystemInfo.processorCount);
			int num2 = SystemInfo.systemMemorySize;
			if (num2 <= 0)
			{
				Debug.LogError("Machine reporting that is has <= 0 bytes of RAM. This is definitely not true, assuming 1 GiB");
				num2 = 1024;
			}
			if (num <= 1)
			{
				return 0;
			}
			if (num2 <= 512)
			{
				return 0;
			}
			if (count == ThreadCount.AutomaticHighLoad)
			{
				if (num2 <= 1024)
				{
					num = Math.Min(num, 2);
				}
				num = Math.Min(num, Math.Max(8, num / 2 + 2));
			}
			else
			{
				num /= 2;
				num = Mathf.Max(1, num);
				if (num2 <= 1024)
				{
					num = Math.Min(num, 2);
				}
				num = Math.Min(num, 6);
			}
			return num;
		}
	}
}
