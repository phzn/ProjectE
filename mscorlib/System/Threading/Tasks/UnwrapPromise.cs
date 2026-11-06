using System;
using System.Collections.ObjectModel;
using System.Runtime.ExceptionServices;
using Internal.Runtime.Augments;

namespace System.Threading.Tasks
{
	// Token: 0x02000367 RID: 871
	internal sealed class UnwrapPromise<TResult> : Task<TResult>, ITaskCompletionAction
	{
		// Token: 0x06002469 RID: 9321 RVA: 0x000825C0 File Offset: 0x000807C0
		public UnwrapPromise(Task outerTask, bool lookForOce)
			: base(null, outerTask.CreationOptions & TaskCreationOptions.AttachedToParent)
		{
			this._lookForOce = lookForOce;
			this._state = 0;
			if (DebuggerSupport.LoggingOn)
			{
				DebuggerSupport.TraceOperationCreation(CausalityTraceLevel.Required, this, "Task.Unwrap", 0UL);
			}
			DebuggerSupport.AddToActiveTasks(this);
			if (outerTask.IsCompleted)
			{
				this.ProcessCompletedOuterTask(outerTask);
				return;
			}
			outerTask.AddCompletionAction(this);
		}

		// Token: 0x0600246A RID: 9322 RVA: 0x0008261C File Offset: 0x0008081C
		public void Invoke(Task completingTask)
		{
			StackGuard currentStackGuard = Task.CurrentStackGuard;
			if (currentStackGuard.TryBeginInliningScope())
			{
				try
				{
					this.InvokeCore(completingTask);
					return;
				}
				finally
				{
					currentStackGuard.EndInliningScope();
				}
			}
			this.InvokeCoreAsync(completingTask);
		}

		// Token: 0x0600246B RID: 9323 RVA: 0x00082660 File Offset: 0x00080860
		private void InvokeCore(Task completingTask)
		{
			byte state = this._state;
			if (state == 0)
			{
				this.ProcessCompletedOuterTask(completingTask);
				return;
			}
			if (state != 1)
			{
				return;
			}
			this.TrySetFromTask(completingTask, false);
			this._state = 2;
		}

		// Token: 0x0600246C RID: 9324 RVA: 0x00082694 File Offset: 0x00080894
		private void InvokeCoreAsync(Task completingTask)
		{
			ThreadPool.UnsafeQueueUserWorkItem(delegate(object state)
			{
				Tuple<UnwrapPromise<TResult>, Task> tuple = (Tuple<UnwrapPromise<TResult>, Task>)state;
				tuple.Item1.InvokeCore(tuple.Item2);
			}, Tuple.Create<UnwrapPromise<TResult>, Task>(this, completingTask));
		}

		// Token: 0x0600246D RID: 9325 RVA: 0x000826C4 File Offset: 0x000808C4
		private void ProcessCompletedOuterTask(Task task)
		{
			this._state = 1;
			TaskStatus status = task.Status;
			if (status != TaskStatus.RanToCompletion)
			{
				if (status - TaskStatus.Canceled <= 1)
				{
					this.TrySetFromTask(task, this._lookForOce);
					return;
				}
			}
			else
			{
				Task<Task<TResult>> task2 = task as Task<Task<TResult>>;
				this.ProcessInnerTask((task2 != null) ? task2.Result : ((Task<Task>)task).Result);
			}
		}

		// Token: 0x0600246E RID: 9326 RVA: 0x0008271C File Offset: 0x0008091C
		private bool TrySetFromTask(Task task, bool lookForOce)
		{
			if (DebuggerSupport.LoggingOn)
			{
				DebuggerSupport.TraceOperationRelation(CausalityTraceLevel.Important, this, CausalityRelation.Join);
			}
			bool flag = false;
			switch (task.Status)
			{
			case TaskStatus.RanToCompletion:
			{
				Task<TResult> task2 = task as Task<TResult>;
				if (DebuggerSupport.LoggingOn)
				{
					DebuggerSupport.TraceOperationCompletion(CausalityTraceLevel.Required, this, AsyncStatus.Completed);
				}
				DebuggerSupport.RemoveFromActiveTasks(this);
				flag = base.TrySetResult((task2 != null) ? task2.Result : default(TResult));
				break;
			}
			case TaskStatus.Canceled:
				flag = base.TrySetCanceled(task.CancellationToken, task.GetCancellationExceptionDispatchInfo());
				break;
			case TaskStatus.Faulted:
			{
				ReadOnlyCollection<ExceptionDispatchInfo> exceptionDispatchInfos = task.GetExceptionDispatchInfos();
				ExceptionDispatchInfo exceptionDispatchInfo;
				OperationCanceledException ex;
				if (lookForOce && exceptionDispatchInfos.Count > 0 && (exceptionDispatchInfo = exceptionDispatchInfos[0]) != null && (ex = exceptionDispatchInfo.SourceException as OperationCanceledException) != null)
				{
					flag = base.TrySetCanceled(ex.CancellationToken, exceptionDispatchInfo);
				}
				else
				{
					flag = base.TrySetException(exceptionDispatchInfos);
				}
				break;
			}
			}
			return flag;
		}

		// Token: 0x0600246F RID: 9327 RVA: 0x000827F8 File Offset: 0x000809F8
		private void ProcessInnerTask(Task task)
		{
			if (task == null)
			{
				base.TrySetCanceled(default(CancellationToken));
				this._state = 2;
				return;
			}
			if (task.IsCompleted)
			{
				this.TrySetFromTask(task, false);
				this._state = 2;
				return;
			}
			task.AddCompletionAction(this);
		}

		// Token: 0x1700046B RID: 1131
		// (get) Token: 0x06002470 RID: 9328 RVA: 0x000040F7 File Offset: 0x000022F7
		public bool InvokeMayRunArbitraryCode
		{
			get
			{
				return true;
			}
		}

		// Token: 0x04001D27 RID: 7463
		private const byte STATE_WAITING_ON_OUTER_TASK = 0;

		// Token: 0x04001D28 RID: 7464
		private const byte STATE_WAITING_ON_INNER_TASK = 1;

		// Token: 0x04001D29 RID: 7465
		private const byte STATE_DONE = 2;

		// Token: 0x04001D2A RID: 7466
		private byte _state;

		// Token: 0x04001D2B RID: 7467
		private readonly bool _lookForOce;
	}
}
