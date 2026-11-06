using System;

namespace System.Threading.Tasks
{
	// Token: 0x02000371 RID: 881
	internal sealed class TaskSchedulerAwaitTaskContinuation : AwaitTaskContinuation
	{
		// Token: 0x0600248B RID: 9355 RVA: 0x00082D35 File Offset: 0x00080F35
		internal TaskSchedulerAwaitTaskContinuation(TaskScheduler scheduler, Action action, bool flowExecutionContext)
			: base(action, flowExecutionContext)
		{
			this.m_scheduler = scheduler;
		}

		// Token: 0x0600248C RID: 9356 RVA: 0x00082D48 File Offset: 0x00080F48
		internal sealed override void Run(Task ignored, bool canInlineContinuationTask)
		{
			if (this.m_scheduler == TaskScheduler.Default)
			{
				base.Run(ignored, canInlineContinuationTask);
				return;
			}
			bool flag = canInlineContinuationTask && (TaskScheduler.InternalCurrent == this.m_scheduler || ThreadPool.IsThreadPoolThread);
			Task task = base.CreateTask(delegate(object state)
			{
				try
				{
					((Action)state)();
				}
				catch (Exception ex)
				{
					AwaitTaskContinuation.ThrowAsyncIfNecessary(ex);
				}
			}, this.m_action, this.m_scheduler);
			if (flag)
			{
				TaskContinuation.InlineIfPossibleOrElseQueue(task, false);
				return;
			}
			try
			{
				task.ScheduleAndStart(false);
			}
			catch (TaskSchedulerException)
			{
			}
		}

		// Token: 0x04001D39 RID: 7481
		private readonly TaskScheduler m_scheduler;
	}
}
