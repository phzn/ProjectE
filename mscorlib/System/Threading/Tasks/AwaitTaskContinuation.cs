using System;
using System.Runtime.CompilerServices;
using Internal.Runtime.Augments;

namespace System.Threading.Tasks
{
	// Token: 0x02000373 RID: 883
	internal class AwaitTaskContinuation : TaskContinuation, IThreadPoolWorkItem
	{
		// Token: 0x06002490 RID: 9360 RVA: 0x00082E20 File Offset: 0x00081020
		internal AwaitTaskContinuation(Action action, bool flowExecutionContext)
		{
			this.m_action = action;
			if (flowExecutionContext)
			{
				this.m_capturedContext = ExecutionContext.Capture();
			}
		}

		// Token: 0x06002491 RID: 9361 RVA: 0x00082E40 File Offset: 0x00081040
		protected Task CreateTask(Action<object> action, object state, TaskScheduler scheduler)
		{
			return new Task(action, state, null, default(CancellationToken), TaskCreationOptions.None, InternalTaskOptions.QueuedByRuntime, scheduler);
		}

		// Token: 0x06002492 RID: 9362 RVA: 0x00082E65 File Offset: 0x00081065
		internal override void Run(Task ignored, bool canInlineContinuationTask)
		{
			if (canInlineContinuationTask && AwaitTaskContinuation.IsValidLocationForInlining)
			{
				this.RunCallback(AwaitTaskContinuation.GetInvokeActionCallback(), this.m_action, ref Task.t_currentTask);
				return;
			}
			ThreadPool.UnsafeQueueCustomWorkItem(this, false);
		}

		// Token: 0x1700046C RID: 1132
		// (get) Token: 0x06002493 RID: 9363 RVA: 0x00082E90 File Offset: 0x00081090
		internal static bool IsValidLocationForInlining
		{
			get
			{
				SynchronizationContext synchronizationContext = SynchronizationContext.Current;
				if (synchronizationContext != null && synchronizationContext.GetType() != typeof(SynchronizationContext))
				{
					return false;
				}
				TaskScheduler internalCurrent = TaskScheduler.InternalCurrent;
				return internalCurrent == null || internalCurrent == TaskScheduler.Default;
			}
		}

		// Token: 0x06002494 RID: 9364 RVA: 0x00082ED2 File Offset: 0x000810D2
		void IThreadPoolWorkItem.ExecuteWorkItem()
		{
			if (this.m_capturedContext == null)
			{
				this.m_action();
				return;
			}
			ExecutionContext.Run(this.m_capturedContext, AwaitTaskContinuation.GetInvokeActionCallback(), this.m_action);
		}

		// Token: 0x06002495 RID: 9365 RVA: 0x00082EFE File Offset: 0x000810FE
		private static void InvokeAction(object state)
		{
			((Action)state)();
		}

		// Token: 0x06002496 RID: 9366 RVA: 0x00082F0C File Offset: 0x0008110C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		protected static ContextCallback GetInvokeActionCallback()
		{
			ContextCallback contextCallback = AwaitTaskContinuation.s_invokeActionCallback;
			if (contextCallback == null)
			{
				contextCallback = (AwaitTaskContinuation.s_invokeActionCallback = new ContextCallback(AwaitTaskContinuation.InvokeAction));
			}
			return contextCallback;
		}

		// Token: 0x06002497 RID: 9367 RVA: 0x00082F38 File Offset: 0x00081138
		protected void RunCallback(ContextCallback callback, object state, ref Task currentTask)
		{
			Task task = currentTask;
			SynchronizationContext currentExplicit = SynchronizationContext.CurrentExplicit;
			try
			{
				if (task != null)
				{
					currentTask = null;
				}
				callback(state);
			}
			catch (Exception ex)
			{
				AwaitTaskContinuation.ThrowAsyncIfNecessary(ex);
			}
			finally
			{
				if (task != null)
				{
					currentTask = task;
				}
				SynchronizationContext.SetSynchronizationContext(currentExplicit);
			}
		}

		// Token: 0x06002498 RID: 9368 RVA: 0x00082F90 File Offset: 0x00081190
		internal static void RunOrScheduleAction(Action action, bool allowInlining, ref Task currentTask)
		{
			if (!allowInlining || !AwaitTaskContinuation.IsValidLocationForInlining)
			{
				AwaitTaskContinuation.UnsafeScheduleAction(action);
				return;
			}
			Task task = currentTask;
			try
			{
				if (task != null)
				{
					currentTask = null;
				}
				action();
			}
			catch (Exception ex)
			{
				AwaitTaskContinuation.ThrowAsyncIfNecessary(ex);
			}
			finally
			{
				if (task != null)
				{
					currentTask = task;
				}
			}
		}

		// Token: 0x06002499 RID: 9369 RVA: 0x00082FEC File Offset: 0x000811EC
		internal static void UnsafeScheduleAction(Action action)
		{
			ThreadPool.UnsafeQueueCustomWorkItem(new AwaitTaskContinuation(action, false), false);
		}

		// Token: 0x0600249A RID: 9370 RVA: 0x00082FFB File Offset: 0x000811FB
		protected static void ThrowAsyncIfNecessary(Exception exc)
		{
			RuntimeAugments.ReportUnhandledException(exc);
		}

		// Token: 0x0600249B RID: 9371 RVA: 0x00083003 File Offset: 0x00081203
		internal override Delegate[] GetDelegateContinuationsForDebugger()
		{
			return new Delegate[] { AsyncMethodBuilderCore.TryGetStateMachineForDebugger(this.m_action) };
		}

		// Token: 0x0600249C RID: 9372 RVA: 0x00004BF9 File Offset: 0x00002DF9
		public void MarkAborted(ThreadAbortException e)
		{
		}

		// Token: 0x04001D3C RID: 7484
		private readonly ExecutionContext m_capturedContext;

		// Token: 0x04001D3D RID: 7485
		protected readonly Action m_action;

		// Token: 0x04001D3E RID: 7486
		private static ContextCallback s_invokeActionCallback;
	}
}
