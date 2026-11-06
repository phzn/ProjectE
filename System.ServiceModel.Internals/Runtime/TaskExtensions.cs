using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace System.Runtime
{
	// Token: 0x0200002D RID: 45
	internal static class TaskExtensions
	{
		// Token: 0x06000153 RID: 339 RVA: 0x00005DFC File Offset: 0x00003FFC
		public static IAsyncResult AsAsyncResult<T>(this Task<T> task, AsyncCallback callback, object state)
		{
			if (task == null)
			{
				throw Fx.Exception.ArgumentNull("task");
			}
			if (task.Status == TaskStatus.Created)
			{
				throw Fx.Exception.AsError(new InvalidOperationException("SFx Task Not Started"));
			}
			TaskCompletionSource<T> tcs = new TaskCompletionSource<T>(state);
			task.ContinueWith(delegate(Task<T> t)
			{
				if (t.IsFaulted)
				{
					tcs.TrySetException(t.Exception.InnerExceptions);
				}
				else if (t.IsCanceled)
				{
					tcs.TrySetCanceled();
				}
				else
				{
					tcs.TrySetResult(t.Result);
				}
				if (callback != null)
				{
					callback(tcs.Task);
				}
			}, TaskContinuationOptions.ExecuteSynchronously);
			return tcs.Task;
		}

		// Token: 0x06000154 RID: 340 RVA: 0x00005E78 File Offset: 0x00004078
		public static IAsyncResult AsAsyncResult(this Task task, AsyncCallback callback, object state)
		{
			if (task == null)
			{
				throw Fx.Exception.ArgumentNull("task");
			}
			if (task.Status == TaskStatus.Created)
			{
				throw Fx.Exception.AsError(new InvalidOperationException("SFx Task Not Started"));
			}
			TaskCompletionSource<object> tcs = new TaskCompletionSource<object>(state);
			task.ContinueWith(delegate(Task t)
			{
				if (t.IsFaulted)
				{
					tcs.TrySetException(t.Exception.InnerExceptions);
				}
				else if (t.IsCanceled)
				{
					tcs.TrySetCanceled();
				}
				else
				{
					tcs.TrySetResult(null);
				}
				if (callback != null)
				{
					callback(tcs.Task);
				}
			}, TaskContinuationOptions.ExecuteSynchronously);
			return tcs.Task;
		}

		// Token: 0x06000155 RID: 341 RVA: 0x00005EF1 File Offset: 0x000040F1
		public static ConfiguredTaskAwaitable SuppressContextFlow(this Task task)
		{
			return task.ConfigureAwait(false);
		}

		// Token: 0x06000156 RID: 342 RVA: 0x00005EFA File Offset: 0x000040FA
		public static ConfiguredTaskAwaitable<T> SuppressContextFlow<T>(this Task<T> task)
		{
			return task.ConfigureAwait(false);
		}

		// Token: 0x06000157 RID: 343 RVA: 0x00005F03 File Offset: 0x00004103
		public static ConfiguredTaskAwaitable ContinueOnCapturedContextFlow(this Task task)
		{
			return task.ConfigureAwait(true);
		}

		// Token: 0x06000158 RID: 344 RVA: 0x00005F0C File Offset: 0x0000410C
		public static ConfiguredTaskAwaitable<T> ContinueOnCapturedContextFlow<T>(this Task<T> task)
		{
			return task.ConfigureAwait(true);
		}

		// Token: 0x06000159 RID: 345 RVA: 0x00005F18 File Offset: 0x00004118
		public static void Wait<TException>(this Task task)
		{
			try
			{
				task.Wait();
			}
			catch (AggregateException ex)
			{
				throw Fx.Exception.AsError<TException>(ex);
			}
		}

		// Token: 0x0600015A RID: 346 RVA: 0x00005F4C File Offset: 0x0000414C
		public static bool Wait<TException>(this Task task, int millisecondsTimeout)
		{
			bool flag;
			try
			{
				flag = task.Wait(millisecondsTimeout);
			}
			catch (AggregateException ex)
			{
				throw Fx.Exception.AsError<TException>(ex);
			}
			return flag;
		}

		// Token: 0x0600015B RID: 347 RVA: 0x00005F84 File Offset: 0x00004184
		public static bool Wait<TException>(this Task task, TimeSpan timeout)
		{
			bool flag;
			try
			{
				if (timeout == TimeSpan.MaxValue)
				{
					flag = task.Wait(-1);
				}
				else
				{
					flag = task.Wait(timeout);
				}
			}
			catch (AggregateException ex)
			{
				throw Fx.Exception.AsError<TException>(ex);
			}
			return flag;
		}

		// Token: 0x0600015C RID: 348 RVA: 0x00005FD0 File Offset: 0x000041D0
		public static void Wait(this Task task, TimeSpan timeout, Action<Exception, TimeSpan, string> exceptionConverter, string operationType)
		{
			bool flag = false;
			try
			{
				if (timeout > TimeoutHelper.MaxWait)
				{
					task.Wait();
				}
				else
				{
					flag = !task.Wait(timeout);
				}
			}
			catch (Exception ex)
			{
				if (Fx.IsFatal(ex) || exceptionConverter == null)
				{
					throw;
				}
				exceptionConverter(ex, timeout, operationType);
			}
			if (flag)
			{
				throw Fx.Exception.AsError(new TimeoutException(InternalSR.TaskTimedOutError(timeout)));
			}
		}

		// Token: 0x0600015D RID: 349 RVA: 0x00006048 File Offset: 0x00004248
		public static Task<TBase> Upcast<TDerived, TBase>(this Task<TDerived> task) where TDerived : TBase
		{
			if (task.Status != TaskStatus.RanToCompletion)
			{
				return task.UpcastPrivate<TDerived, TBase>();
			}
			return Task.FromResult<TBase>((TBase)((object)task.Result));
		}

		// Token: 0x0600015E RID: 350 RVA: 0x00006070 File Offset: 0x00004270
		private static async Task<TBase> UpcastPrivate<TDerived, TBase>(this Task<TDerived> task) where TDerived : TBase
		{
			ConfiguredTaskAwaitable<TDerived>.ConfiguredTaskAwaiter configuredTaskAwaiter = task.ConfigureAwait(false).GetAwaiter();
			if (!configuredTaskAwaiter.IsCompleted)
			{
				await configuredTaskAwaiter;
				ConfiguredTaskAwaitable<TDerived>.ConfiguredTaskAwaiter configuredTaskAwaiter2;
				configuredTaskAwaiter = configuredTaskAwaiter2;
				configuredTaskAwaiter2 = default(ConfiguredTaskAwaitable<TDerived>.ConfiguredTaskAwaiter);
			}
			return (TBase)((object)configuredTaskAwaiter.GetResult());
		}
	}
}
