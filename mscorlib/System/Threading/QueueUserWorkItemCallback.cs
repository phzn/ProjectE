using System;
using System.Security;

namespace System.Threading
{
	// Token: 0x020002E4 RID: 740
	internal sealed class QueueUserWorkItemCallback : IThreadPoolWorkItem
	{
		// Token: 0x06002029 RID: 8233 RVA: 0x000755FB File Offset: 0x000737FB
		[SecurityCritical]
		internal QueueUserWorkItemCallback(WaitCallback waitCallback, object stateObj, bool compressStack, ref StackCrawlMark stackMark)
		{
			this.callback = waitCallback;
			this.state = stateObj;
			if (compressStack && !ExecutionContext.IsFlowSuppressed())
			{
				this.context = ExecutionContext.Capture(ref stackMark, ExecutionContext.CaptureOptions.IgnoreSyncCtx | ExecutionContext.CaptureOptions.OptimizeDefaultCase);
			}
		}

		// Token: 0x0600202A RID: 8234 RVA: 0x00075629 File Offset: 0x00073829
		internal QueueUserWorkItemCallback(WaitCallback waitCallback, object stateObj, ExecutionContext ec)
		{
			this.callback = waitCallback;
			this.state = stateObj;
			this.context = ec;
		}

		// Token: 0x0600202B RID: 8235 RVA: 0x00075646 File Offset: 0x00073846
		[SecurityCritical]
		void IThreadPoolWorkItem.ExecuteWorkItem()
		{
			if (this.context == null)
			{
				WaitCallback waitCallback = this.callback;
				this.callback = null;
				waitCallback(this.state);
				return;
			}
			ExecutionContext.Run(this.context, QueueUserWorkItemCallback.ccb, this, true);
		}

		// Token: 0x0600202C RID: 8236 RVA: 0x00004BF9 File Offset: 0x00002DF9
		[SecurityCritical]
		void IThreadPoolWorkItem.MarkAborted(ThreadAbortException tae)
		{
		}

		// Token: 0x0600202D RID: 8237 RVA: 0x0007567C File Offset: 0x0007387C
		[SecurityCritical]
		private static void WaitCallback_Context(object state)
		{
			QueueUserWorkItemCallback queueUserWorkItemCallback = (QueueUserWorkItemCallback)state;
			queueUserWorkItemCallback.callback(queueUserWorkItemCallback.state);
		}

		// Token: 0x04001B43 RID: 6979
		private WaitCallback callback;

		// Token: 0x04001B44 RID: 6980
		private ExecutionContext context;

		// Token: 0x04001B45 RID: 6981
		private object state;

		// Token: 0x04001B46 RID: 6982
		[SecurityCritical]
		internal static ContextCallback ccb = new ContextCallback(QueueUserWorkItemCallback.WaitCallback_Context);
	}
}
