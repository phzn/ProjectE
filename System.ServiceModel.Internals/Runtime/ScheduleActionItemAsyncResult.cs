using System;

namespace System.Runtime
{
	// Token: 0x02000029 RID: 41
	internal abstract class ScheduleActionItemAsyncResult : AsyncResult
	{
		// Token: 0x06000137 RID: 311 RVA: 0x0000585A File Offset: 0x00003A5A
		protected ScheduleActionItemAsyncResult(AsyncCallback callback, object state)
			: base(callback, state)
		{
		}

		// Token: 0x06000138 RID: 312 RVA: 0x00005864 File Offset: 0x00003A64
		protected void Schedule()
		{
			ActionItem.Schedule(ScheduleActionItemAsyncResult.doWork, this);
		}

		// Token: 0x06000139 RID: 313 RVA: 0x00005874 File Offset: 0x00003A74
		private static void DoWork(object state)
		{
			ScheduleActionItemAsyncResult scheduleActionItemAsyncResult = (ScheduleActionItemAsyncResult)state;
			Exception ex = null;
			try
			{
				scheduleActionItemAsyncResult.OnDoWork();
			}
			catch (Exception ex2)
			{
				if (Fx.IsFatal(ex2))
				{
					throw;
				}
				ex = ex2;
			}
			scheduleActionItemAsyncResult.Complete(false, ex);
		}

		// Token: 0x0600013A RID: 314
		protected abstract void OnDoWork();

		// Token: 0x0600013B RID: 315 RVA: 0x000058B8 File Offset: 0x00003AB8
		public static void End(IAsyncResult result)
		{
			AsyncResult.End<ScheduleActionItemAsyncResult>(result);
		}

		// Token: 0x040000C9 RID: 201
		private static Action<object> doWork = new Action<object>(ScheduleActionItemAsyncResult.DoWork);
	}
}
