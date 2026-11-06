using System;
using System.Security;

namespace System.Threading
{
	// Token: 0x020002DD RID: 733
	internal static class ThreadPoolGlobals
	{
		// Token: 0x170003CF RID: 975
		// (get) Token: 0x0600200A RID: 8202 RVA: 0x000749ED File Offset: 0x00072BED
		public static bool tpHosted
		{
			get
			{
				return ThreadPool.IsThreadPoolHosted();
			}
		}

		// Token: 0x04001B29 RID: 6953
		public const uint tpQuantum = 30U;

		// Token: 0x04001B2A RID: 6954
		public static int processorCount = Environment.ProcessorCount;

		// Token: 0x04001B2B RID: 6955
		public static volatile bool vmTpInitialized;

		// Token: 0x04001B2C RID: 6956
		public static bool enableWorkerTracking;

		// Token: 0x04001B2D RID: 6957
		[SecurityCritical]
		public static readonly ThreadPoolWorkQueue workQueue = new ThreadPoolWorkQueue();
	}
}
