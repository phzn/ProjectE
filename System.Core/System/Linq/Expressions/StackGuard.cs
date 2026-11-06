using System;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace System.Linq.Expressions
{
	// Token: 0x02000296 RID: 662
	internal sealed class StackGuard
	{
		// Token: 0x0600131A RID: 4890 RVA: 0x0003C7A6 File Offset: 0x0003A9A6
		public bool TryEnterOnCurrentStack()
		{
			if (RuntimeHelpers.TryEnsureSufficientExecutionStack())
			{
				return true;
			}
			if (this._executionStackCount < 1024)
			{
				return false;
			}
			throw new InsufficientExecutionStackException();
		}

		// Token: 0x0600131B RID: 4891 RVA: 0x0003C7C5 File Offset: 0x0003A9C5
		public void RunOnEmptyStack<T1, T2>(Action<T1, T2> action, T1 arg1, T2 arg2)
		{
			this.RunOnEmptyStackCore<object>(delegate(object s)
			{
				Tuple<Action<T1, T2>, T1, T2> tuple = (Tuple<Action<T1, T2>, T1, T2>)s;
				tuple.Item1(tuple.Item2, tuple.Item3);
				return null;
			}, Tuple.Create<Action<T1, T2>, T1, T2>(action, arg1, arg2));
		}

		// Token: 0x0600131C RID: 4892 RVA: 0x0003C7F5 File Offset: 0x0003A9F5
		public void RunOnEmptyStack<T1, T2, T3>(Action<T1, T2, T3> action, T1 arg1, T2 arg2, T3 arg3)
		{
			this.RunOnEmptyStackCore<object>(delegate(object s)
			{
				Tuple<Action<T1, T2, T3>, T1, T2, T3> tuple = (Tuple<Action<T1, T2, T3>, T1, T2, T3>)s;
				tuple.Item1(tuple.Item2, tuple.Item3, tuple.Item4);
				return null;
			}, Tuple.Create<Action<T1, T2, T3>, T1, T2, T3>(action, arg1, arg2, arg3));
		}

		// Token: 0x0600131D RID: 4893 RVA: 0x0003C827 File Offset: 0x0003AA27
		public R RunOnEmptyStack<T1, T2, R>(Func<T1, T2, R> action, T1 arg1, T2 arg2)
		{
			return this.RunOnEmptyStackCore<R>(delegate(object s)
			{
				Tuple<Func<T1, T2, R>, T1, T2> tuple = (Tuple<Func<T1, T2, R>, T1, T2>)s;
				return tuple.Item1(tuple.Item2, tuple.Item3);
			}, Tuple.Create<Func<T1, T2, R>, T1, T2>(action, arg1, arg2));
		}

		// Token: 0x0600131E RID: 4894 RVA: 0x0003C856 File Offset: 0x0003AA56
		public R RunOnEmptyStack<T1, T2, T3, R>(Func<T1, T2, T3, R> action, T1 arg1, T2 arg2, T3 arg3)
		{
			return this.RunOnEmptyStackCore<R>(delegate(object s)
			{
				Tuple<Func<T1, T2, T3, R>, T1, T2, T3> tuple = (Tuple<Func<T1, T2, T3, R>, T1, T2, T3>)s;
				return tuple.Item1(tuple.Item2, tuple.Item3, tuple.Item4);
			}, Tuple.Create<Func<T1, T2, T3, R>, T1, T2, T3>(action, arg1, arg2, arg3));
		}

		// Token: 0x0600131F RID: 4895 RVA: 0x0003C888 File Offset: 0x0003AA88
		private R RunOnEmptyStackCore<R>(Func<object, R> action, object state)
		{
			this._executionStackCount++;
			R result;
			try
			{
				Task<R> task = Task.Factory.StartNew<R>(action, state, CancellationToken.None, TaskCreationOptions.DenyChildAttach, TaskScheduler.Default);
				TaskAwaiter<R> awaiter = task.GetAwaiter();
				if (!awaiter.IsCompleted)
				{
					((IAsyncResult)task).AsyncWaitHandle.WaitOne();
				}
				result = awaiter.GetResult();
			}
			finally
			{
				this._executionStackCount--;
			}
			return result;
		}

		// Token: 0x04000A4C RID: 2636
		private const int MaxExecutionStackCount = 1024;

		// Token: 0x04000A4D RID: 2637
		private int _executionStackCount;
	}
}
