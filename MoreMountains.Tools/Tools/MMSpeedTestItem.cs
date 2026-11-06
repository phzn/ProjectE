using System;
using System.Diagnostics;

namespace MoreMountains.Tools
{
	// Token: 0x02000077 RID: 119
	public struct MMSpeedTestItem
	{
		// Token: 0x060002B4 RID: 692 RVA: 0x0000DC2C File Offset: 0x0000BE2C
		public MMSpeedTestItem(string testID)
		{
			this.TestID = testID;
			this.Timer = Stopwatch.StartNew();
		}

		// Token: 0x040003E7 RID: 999
		public string TestID;

		// Token: 0x040003E8 RID: 1000
		public Stopwatch Timer;
	}
}
