using System;

namespace System.Threading.Tasks
{
	// Token: 0x0200036D RID: 877
	internal abstract class TaskContinuation
	{
		// Token: 0x0600247C RID: 9340
		internal abstract void Run(Task completedTask, bool bCanInlineContinuationTask);

		// Token: 0x0600247D RID: 9341 RVA: 0x00082ABC File Offset: 0x00080CBC
		protected static void InlineIfPossibleOrElseQueue(Task task, bool needsProtection)
		{
			if (needsProtection)
			{
				if (!task.MarkStarted())
				{
					return;
				}
			}
			else
			{
				task.m_stateFlags |= 65536;
			}
			try
			{
				if (!task.m_taskScheduler.TryRunInline(task, false))
				{
					task.m_taskScheduler.QueueTask(task);
				}
			}
			catch (Exception ex)
			{
				TaskSchedulerException ex2 = new TaskSchedulerException(ex);
				task.AddException(ex2);
				task.Finish(false);
			}
		}

		// Token: 0x0600247E RID: 9342
		internal abstract Delegate[] GetDelegateContinuationsForDebugger();
	}
}
