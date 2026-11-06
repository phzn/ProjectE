using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace System.Xml
{
	// Token: 0x0200000E RID: 14
	internal static class AsyncHelper
	{
		// Token: 0x0600001D RID: 29 RVA: 0x000022CC File Offset: 0x000004CC
		public static bool IsSuccess(this Task task)
		{
			return task.IsCompleted && task.Exception == null;
		}

		// Token: 0x0600001E RID: 30 RVA: 0x000022E1 File Offset: 0x000004E1
		public static Task CallVoidFuncWhenFinish(this Task task, Action func)
		{
			if (task.IsSuccess())
			{
				func();
				return AsyncHelper.DoneTask;
			}
			return task._CallVoidFuncWhenFinish(func);
		}

		// Token: 0x0600001F RID: 31 RVA: 0x00002300 File Offset: 0x00000500
		private static async Task _CallVoidFuncWhenFinish(this Task task, Action func)
		{
			await task.ConfigureAwait(false);
			func();
		}

		// Token: 0x06000020 RID: 32 RVA: 0x0000234B File Offset: 0x0000054B
		public static Task<bool> ReturnTaskBoolWhenFinish(this Task task, bool ret)
		{
			if (!task.IsSuccess())
			{
				return task._ReturnTaskBoolWhenFinish(ret);
			}
			if (ret)
			{
				return AsyncHelper.DoneTaskTrue;
			}
			return AsyncHelper.DoneTaskFalse;
		}

		// Token: 0x06000021 RID: 33 RVA: 0x0000236C File Offset: 0x0000056C
		public static async Task<bool> _ReturnTaskBoolWhenFinish(this Task task, bool ret)
		{
			await task.ConfigureAwait(false);
			return ret;
		}

		// Token: 0x06000022 RID: 34 RVA: 0x000023B7 File Offset: 0x000005B7
		public static Task CallTaskFuncWhenFinish(this Task task, Func<Task> func)
		{
			if (task.IsSuccess())
			{
				return func();
			}
			return AsyncHelper._CallTaskFuncWhenFinish(task, func);
		}

		// Token: 0x06000023 RID: 35 RVA: 0x000023D0 File Offset: 0x000005D0
		private static async Task _CallTaskFuncWhenFinish(Task task, Func<Task> func)
		{
			await task.ConfigureAwait(false);
			await func().ConfigureAwait(false);
		}

		// Token: 0x06000024 RID: 36 RVA: 0x0000241B File Offset: 0x0000061B
		public static Task<bool> CallBoolTaskFuncWhenFinish(this Task task, Func<Task<bool>> func)
		{
			if (task.IsSuccess())
			{
				return func();
			}
			return task._CallBoolTaskFuncWhenFinish(func);
		}

		// Token: 0x06000025 RID: 37 RVA: 0x00002434 File Offset: 0x00000634
		private static async Task<bool> _CallBoolTaskFuncWhenFinish(this Task task, Func<Task<bool>> func)
		{
			await task.ConfigureAwait(false);
			return await func().ConfigureAwait(false);
		}

		// Token: 0x06000026 RID: 38 RVA: 0x0000247F File Offset: 0x0000067F
		public static Task<bool> ContinueBoolTaskFuncWhenFalse(this Task<bool> task, Func<Task<bool>> func)
		{
			if (!task.IsSuccess())
			{
				return AsyncHelper._ContinueBoolTaskFuncWhenFalse(task, func);
			}
			if (task.Result)
			{
				return AsyncHelper.DoneTaskTrue;
			}
			return func();
		}

		// Token: 0x06000027 RID: 39 RVA: 0x000024A8 File Offset: 0x000006A8
		private static async Task<bool> _ContinueBoolTaskFuncWhenFalse(Task<bool> task, Func<Task<bool>> func)
		{
			ConfiguredTaskAwaitable<bool>.ConfiguredTaskAwaiter configuredTaskAwaiter = task.ConfigureAwait(false).GetAwaiter();
			if (!configuredTaskAwaiter.IsCompleted)
			{
				await configuredTaskAwaiter;
				ConfiguredTaskAwaitable<bool>.ConfiguredTaskAwaiter configuredTaskAwaiter2;
				configuredTaskAwaiter = configuredTaskAwaiter2;
				configuredTaskAwaiter2 = default(ConfiguredTaskAwaitable<bool>.ConfiguredTaskAwaiter);
			}
			bool flag;
			if (configuredTaskAwaiter.GetResult())
			{
				flag = true;
			}
			else
			{
				flag = await func().ConfigureAwait(false);
			}
			return flag;
		}

		// Token: 0x040004C3 RID: 1219
		public static readonly Task DoneTask = Task.FromResult<bool>(true);

		// Token: 0x040004C4 RID: 1220
		public static readonly Task<bool> DoneTaskTrue = Task.FromResult<bool>(true);

		// Token: 0x040004C5 RID: 1221
		public static readonly Task<bool> DoneTaskFalse = Task.FromResult<bool>(false);

		// Token: 0x040004C6 RID: 1222
		public static readonly Task<int> DoneTaskZero = Task.FromResult<int>(0);
	}
}
