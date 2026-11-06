using System;
using System.Collections.Generic;

namespace MoreMountains.Tools
{
	// Token: 0x02000078 RID: 120
	public static class MMSpeedTest
	{
		// Token: 0x060002B5 RID: 693 RVA: 0x0000DC40 File Offset: 0x0000BE40
		public static void StartTest(string testID)
		{
			if (MMSpeedTest._speedTests.ContainsKey(testID))
			{
				MMSpeedTest._speedTests.Remove(testID);
			}
			MMSpeedTestItem mmspeedTestItem = new MMSpeedTestItem(testID);
			MMSpeedTest._speedTests.Add(testID, mmspeedTestItem);
		}

		// Token: 0x060002B6 RID: 694 RVA: 0x0000DC7C File Offset: 0x0000BE7C
		public static void EndTest(string testID)
		{
			if (!MMSpeedTest._speedTests.ContainsKey(testID))
			{
				return;
			}
			MMSpeedTest._speedTests[testID].Timer.Stop();
			float num = (float)MMSpeedTest._speedTests[testID].Timer.ElapsedMilliseconds / 1000f;
			MMSpeedTest._speedTests.Remove(testID);
			MMDebug.DebugLogInfo(string.Concat(new string[]
			{
				"<color=red>MMSpeedTest</color> [Test ",
				testID,
				"] test duration : ",
				num.ToString(),
				"s"
			}), "", 3, true);
		}

		// Token: 0x040003E9 RID: 1001
		private static readonly Dictionary<string, MMSpeedTestItem> _speedTests = new Dictionary<string, MMSpeedTestItem>();
	}
}
