using System;
using System.Security;

namespace System.Threading
{
	// Token: 0x020002E5 RID: 741
	internal class _ThreadPoolWaitOrTimerCallback
	{
		// Token: 0x06002030 RID: 8240 RVA: 0x000756D8 File Offset: 0x000738D8
		[SecurityCritical]
		internal _ThreadPoolWaitOrTimerCallback(WaitOrTimerCallback waitOrTimerCallback, object state, bool compressStack, ref StackCrawlMark stackMark)
		{
			this._waitOrTimerCallback = waitOrTimerCallback;
			this._state = state;
			if (compressStack && !ExecutionContext.IsFlowSuppressed())
			{
				this._executionContext = ExecutionContext.Capture(ref stackMark, ExecutionContext.CaptureOptions.IgnoreSyncCtx | ExecutionContext.CaptureOptions.OptimizeDefaultCase);
			}
		}

		// Token: 0x06002031 RID: 8241 RVA: 0x00075706 File Offset: 0x00073906
		[SecurityCritical]
		private static void WaitOrTimerCallback_Context_t(object state)
		{
			_ThreadPoolWaitOrTimerCallback.WaitOrTimerCallback_Context(state, true);
		}

		// Token: 0x06002032 RID: 8242 RVA: 0x0007570F File Offset: 0x0007390F
		[SecurityCritical]
		private static void WaitOrTimerCallback_Context_f(object state)
		{
			_ThreadPoolWaitOrTimerCallback.WaitOrTimerCallback_Context(state, false);
		}

		// Token: 0x06002033 RID: 8243 RVA: 0x00075718 File Offset: 0x00073918
		private static void WaitOrTimerCallback_Context(object state, bool timedOut)
		{
			_ThreadPoolWaitOrTimerCallback threadPoolWaitOrTimerCallback = (_ThreadPoolWaitOrTimerCallback)state;
			threadPoolWaitOrTimerCallback._waitOrTimerCallback(threadPoolWaitOrTimerCallback._state, timedOut);
		}

		// Token: 0x06002034 RID: 8244 RVA: 0x00075740 File Offset: 0x00073940
		[SecurityCritical]
		internal static void PerformWaitOrTimerCallback(object state, bool timedOut)
		{
			_ThreadPoolWaitOrTimerCallback threadPoolWaitOrTimerCallback = (_ThreadPoolWaitOrTimerCallback)state;
			if (threadPoolWaitOrTimerCallback._executionContext == null)
			{
				threadPoolWaitOrTimerCallback._waitOrTimerCallback(threadPoolWaitOrTimerCallback._state, timedOut);
				return;
			}
			using (ExecutionContext executionContext = threadPoolWaitOrTimerCallback._executionContext.CreateCopy())
			{
				if (timedOut)
				{
					ExecutionContext.Run(executionContext, _ThreadPoolWaitOrTimerCallback._ccbt, threadPoolWaitOrTimerCallback, true);
				}
				else
				{
					ExecutionContext.Run(executionContext, _ThreadPoolWaitOrTimerCallback._ccbf, threadPoolWaitOrTimerCallback, true);
				}
			}
		}

		// Token: 0x04001B47 RID: 6983
		private WaitOrTimerCallback _waitOrTimerCallback;

		// Token: 0x04001B48 RID: 6984
		private ExecutionContext _executionContext;

		// Token: 0x04001B49 RID: 6985
		private object _state;

		// Token: 0x04001B4A RID: 6986
		[SecurityCritical]
		private static ContextCallback _ccbt = new ContextCallback(_ThreadPoolWaitOrTimerCallback.WaitOrTimerCallback_Context_t);

		// Token: 0x04001B4B RID: 6987
		[SecurityCritical]
		private static ContextCallback _ccbf = new ContextCallback(_ThreadPoolWaitOrTimerCallback.WaitOrTimerCallback_Context_f);
	}
}
