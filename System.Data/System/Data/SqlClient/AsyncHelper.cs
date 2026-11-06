using System;
using System.Runtime.ExceptionServices;
using System.Threading;
using System.Threading.Tasks;

namespace System.Data.SqlClient
{
	// Token: 0x02000233 RID: 563
	internal static class AsyncHelper
	{
		// Token: 0x06001B0D RID: 6925 RVA: 0x0007C634 File Offset: 0x0007A834
		internal static Task CreateContinuationTask(Task task, Action onSuccess, SqlInternalConnectionTds connectionToDoom = null, Action<Exception> onFailure = null)
		{
			if (task == null)
			{
				onSuccess();
				return null;
			}
			TaskCompletionSource<object> completion = new TaskCompletionSource<object>();
			AsyncHelper.ContinueTask(task, completion, delegate
			{
				onSuccess();
				completion.SetResult(null);
			}, connectionToDoom, onFailure, null, null, null);
			return completion.Task;
		}

		// Token: 0x06001B0E RID: 6926 RVA: 0x0007C694 File Offset: 0x0007A894
		internal static Task CreateContinuationTask<T1, T2>(Task task, Action<T1, T2> onSuccess, T1 arg1, T2 arg2, SqlInternalConnectionTds connectionToDoom = null, Action<Exception> onFailure = null)
		{
			return AsyncHelper.CreateContinuationTask(task, delegate
			{
				onSuccess(arg1, arg2);
			}, connectionToDoom, onFailure);
		}

		// Token: 0x06001B0F RID: 6927 RVA: 0x0007C6D4 File Offset: 0x0007A8D4
		internal static void ContinueTask(Task task, TaskCompletionSource<object> completion, Action onSuccess, SqlInternalConnectionTds connectionToDoom = null, Action<Exception> onFailure = null, Action onCancellation = null, Func<Exception, Exception> exceptionConverter = null, SqlConnection connectionToAbort = null)
		{
			task.ContinueWith(delegate(Task tsk)
			{
				if (tsk.Exception != null)
				{
					Exception ex = tsk.Exception.InnerException;
					if (exceptionConverter != null)
					{
						ex = exceptionConverter(ex);
					}
					try
					{
						if (onFailure != null)
						{
							onFailure(ex);
						}
						return;
					}
					finally
					{
						completion.TrySetException(ex);
					}
				}
				if (tsk.IsCanceled)
				{
					try
					{
						if (onCancellation != null)
						{
							onCancellation();
						}
						return;
					}
					finally
					{
						completion.TrySetCanceled();
					}
				}
				try
				{
					onSuccess();
				}
				catch (Exception ex2)
				{
					completion.SetException(ex2);
				}
			}, TaskScheduler.Default);
		}

		// Token: 0x06001B10 RID: 6928 RVA: 0x0007C728 File Offset: 0x0007A928
		internal static void WaitForCompletion(Task task, int timeout, Action onTimeout = null, bool rethrowExceptions = true)
		{
			try
			{
				task.Wait((timeout > 0) ? (1000 * timeout) : (-1));
			}
			catch (AggregateException ex)
			{
				if (rethrowExceptions)
				{
					ExceptionDispatchInfo.Capture(ex.InnerException).Throw();
				}
			}
			if (!task.IsCompleted && onTimeout != null)
			{
				onTimeout();
			}
		}

		// Token: 0x06001B11 RID: 6929 RVA: 0x0007C784 File Offset: 0x0007A984
		internal static void SetTimeoutException(TaskCompletionSource<object> completion, int timeout, Func<Exception> exc, CancellationToken ctoken)
		{
			if (timeout > 0)
			{
				Task.Delay(timeout * 1000, ctoken).ContinueWith(delegate(Task tsk)
				{
					if (!tsk.IsCanceled && !completion.Task.IsCompleted)
					{
						completion.TrySetException(exc());
					}
				});
			}
		}
	}
}
