using System;

namespace System.Threading.Tasks
{
	// Token: 0x0200034B RID: 843
	internal static class GenericDelegateCache<TAntecedentResult, TResult>
	{
		// Token: 0x04001CA6 RID: 7334
		internal static Func<Task<Task>, object, TResult> CWAnyFuncDelegate = delegate(Task<Task> wrappedWinner, object state)
		{
			Func<Task<TAntecedentResult>, TResult> func = (Func<Task<TAntecedentResult>, TResult>)state;
			Task<TAntecedentResult> task = (Task<TAntecedentResult>)wrappedWinner.Result;
			return func(task);
		};

		// Token: 0x04001CA7 RID: 7335
		internal static Func<Task<Task>, object, TResult> CWAnyActionDelegate = delegate(Task<Task> wrappedWinner, object state)
		{
			Action<Task<TAntecedentResult>> action = (Action<Task<TAntecedentResult>>)state;
			Task<TAntecedentResult> task2 = (Task<TAntecedentResult>)wrappedWinner.Result;
			action(task2);
			return default(TResult);
		};

		// Token: 0x04001CA8 RID: 7336
		internal static Func<Task<Task<TAntecedentResult>[]>, object, TResult> CWAllFuncDelegate = delegate(Task<Task<TAntecedentResult>[]> wrappedAntecedents, object state)
		{
			wrappedAntecedents.NotifyDebuggerOfWaitCompletionIfNecessary();
			return ((Func<Task<TAntecedentResult>[], TResult>)state)(wrappedAntecedents.Result);
		};

		// Token: 0x04001CA9 RID: 7337
		internal static Func<Task<Task<TAntecedentResult>[]>, object, TResult> CWAllActionDelegate = delegate(Task<Task<TAntecedentResult>[]> wrappedAntecedents, object state)
		{
			wrappedAntecedents.NotifyDebuggerOfWaitCompletionIfNecessary();
			((Action<Task<TAntecedentResult>[]>)state)(wrappedAntecedents.Result);
			return default(TResult);
		};
	}
}
