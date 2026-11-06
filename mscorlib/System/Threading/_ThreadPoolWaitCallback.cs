using System;
using System.Security;

namespace System.Threading
{
	// Token: 0x020002E3 RID: 739
	internal static class _ThreadPoolWaitCallback
	{
		// Token: 0x06002028 RID: 8232 RVA: 0x000755F4 File Offset: 0x000737F4
		[SecurityCritical]
		internal static bool PerformWaitCallback()
		{
			return ThreadPoolWorkQueue.Dispatch();
		}
	}
}
