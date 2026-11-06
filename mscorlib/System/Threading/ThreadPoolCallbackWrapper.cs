using System;
using Internal.Runtime.Augments;

namespace System.Threading
{
	// Token: 0x020002B4 RID: 692
	internal struct ThreadPoolCallbackWrapper
	{
		// Token: 0x06001E51 RID: 7761 RVA: 0x000704FC File Offset: 0x0006E6FC
		public static ThreadPoolCallbackWrapper Enter()
		{
			return new ThreadPoolCallbackWrapper
			{
				_currentThread = RuntimeThread.InitializeThreadPoolThread()
			};
		}

		// Token: 0x06001E52 RID: 7762 RVA: 0x0007051E File Offset: 0x0006E71E
		public void Exit(bool resetThread = true)
		{
			if (resetThread)
			{
				this._currentThread.ResetThreadPoolThread();
			}
		}

		// Token: 0x04001A9F RID: 6815
		private RuntimeThread _currentThread;
	}
}
