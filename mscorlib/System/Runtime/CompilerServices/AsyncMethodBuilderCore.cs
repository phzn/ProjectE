using System;
using System.Diagnostics;
using System.Runtime.ExceptionServices;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Security;
using System.Threading;
using System.Threading.Tasks;

namespace System.Runtime.CompilerServices
{
	// Token: 0x0200081E RID: 2078
	internal struct AsyncMethodBuilderCore
	{
		// Token: 0x06004688 RID: 18056 RVA: 0x000E6BEC File Offset: 0x000E4DEC
		[DebuggerStepThrough]
		[SecuritySafeCritical]
		internal static void Start<TStateMachine>(ref TStateMachine stateMachine) where TStateMachine : IAsyncStateMachine
		{
			if (stateMachine == null)
			{
				throw new ArgumentNullException("stateMachine");
			}
			Thread currentThread = Thread.CurrentThread;
			ExecutionContextSwitcher executionContextSwitcher = default(ExecutionContextSwitcher);
			RuntimeHelpers.PrepareConstrainedRegions();
			try
			{
				ExecutionContext.EstablishCopyOnWriteScope(ref executionContextSwitcher);
				stateMachine.MoveNext();
			}
			finally
			{
				executionContextSwitcher.Undo();
			}
		}

		// Token: 0x06004689 RID: 18057 RVA: 0x000E6C54 File Offset: 0x000E4E54
		public void SetStateMachine(IAsyncStateMachine stateMachine)
		{
			if (stateMachine == null)
			{
				throw new ArgumentNullException("stateMachine");
			}
			if (this.m_stateMachine != null)
			{
				throw new InvalidOperationException(Environment.GetResourceString("The builder was not properly initialized."));
			}
			this.m_stateMachine = stateMachine;
		}

		// Token: 0x0600468A RID: 18058 RVA: 0x000E6C84 File Offset: 0x000E4E84
		[SecuritySafeCritical]
		internal Action GetCompletionAction(Task taskForTracing, ref AsyncMethodBuilderCore.MoveNextRunner runnerToInitialize)
		{
			Debugger.NotifyOfCrossThreadDependency();
			ExecutionContext executionContext = ExecutionContext.FastCapture();
			Action action;
			AsyncMethodBuilderCore.MoveNextRunner moveNextRunner;
			if (executionContext != null && executionContext.IsPreAllocatedDefault)
			{
				action = this.m_defaultContextAction;
				if (action != null)
				{
					return action;
				}
				moveNextRunner = new AsyncMethodBuilderCore.MoveNextRunner(executionContext, this.m_stateMachine);
				action = new Action(moveNextRunner.Run);
				if (taskForTracing != null)
				{
					action = (this.m_defaultContextAction = this.OutputAsyncCausalityEvents(taskForTracing, action));
				}
				else
				{
					this.m_defaultContextAction = action;
				}
			}
			else
			{
				moveNextRunner = new AsyncMethodBuilderCore.MoveNextRunner(executionContext, this.m_stateMachine);
				action = new Action(moveNextRunner.Run);
				if (taskForTracing != null)
				{
					action = this.OutputAsyncCausalityEvents(taskForTracing, action);
				}
			}
			if (this.m_stateMachine == null)
			{
				runnerToInitialize = moveNextRunner;
			}
			return action;
		}

		// Token: 0x0600468B RID: 18059 RVA: 0x000E6D20 File Offset: 0x000E4F20
		private Action OutputAsyncCausalityEvents(Task innerTask, Action continuation)
		{
			return AsyncMethodBuilderCore.CreateContinuationWrapper(continuation, delegate
			{
				AsyncCausalityTracer.TraceSynchronousWorkStart(CausalityTraceLevel.Required, innerTask.Id, CausalitySynchronousWork.Execution);
				continuation();
				AsyncCausalityTracer.TraceSynchronousWorkCompletion(CausalityTraceLevel.Required, CausalitySynchronousWork.Execution);
			}, innerTask);
		}

		// Token: 0x0600468C RID: 18060 RVA: 0x000E6D60 File Offset: 0x000E4F60
		internal void PostBoxInitialization(IAsyncStateMachine stateMachine, AsyncMethodBuilderCore.MoveNextRunner runner, Task builtTask)
		{
			if (builtTask != null)
			{
				if (AsyncCausalityTracer.LoggingOn)
				{
					AsyncCausalityTracer.TraceOperationCreation(CausalityTraceLevel.Required, builtTask.Id, "Async: " + stateMachine.GetType().Name, 0UL);
				}
				if (Task.s_asyncDebuggingEnabled)
				{
					Task.AddToActiveTasks(builtTask);
				}
			}
			this.m_stateMachine = stateMachine;
			this.m_stateMachine.SetStateMachine(this.m_stateMachine);
			runner.m_stateMachine = this.m_stateMachine;
		}

		// Token: 0x0600468D RID: 18061 RVA: 0x000E6DCC File Offset: 0x000E4FCC
		internal static void ThrowAsync(Exception exception, SynchronizationContext targetContext)
		{
			ExceptionDispatchInfo exceptionDispatchInfo = ExceptionDispatchInfo.Capture(exception);
			if (targetContext != null)
			{
				try
				{
					targetContext.Post(delegate(object state)
					{
						((ExceptionDispatchInfo)state).Throw();
					}, exceptionDispatchInfo);
					return;
				}
				catch (Exception ex)
				{
					exceptionDispatchInfo = ExceptionDispatchInfo.Capture(new AggregateException(new Exception[] { exception, ex }));
				}
			}
			if (!WindowsRuntimeMarshal.ReportUnhandledError(exceptionDispatchInfo.SourceException))
			{
				ThreadPool.QueueUserWorkItem(delegate(object state)
				{
					((ExceptionDispatchInfo)state).Throw();
				}, exceptionDispatchInfo);
			}
		}

		// Token: 0x0600468E RID: 18062 RVA: 0x000E6E6C File Offset: 0x000E506C
		internal static Action CreateContinuationWrapper(Action continuation, Action invokeAction, Task innerTask = null)
		{
			return new Action(new AsyncMethodBuilderCore.ContinuationWrapper(continuation, invokeAction, innerTask).Invoke);
		}

		// Token: 0x0600468F RID: 18063 RVA: 0x000E6E84 File Offset: 0x000E5084
		internal static Action TryGetStateMachineForDebugger(Action action)
		{
			object target = action.Target;
			AsyncMethodBuilderCore.MoveNextRunner moveNextRunner = target as AsyncMethodBuilderCore.MoveNextRunner;
			if (moveNextRunner != null)
			{
				return new Action(moveNextRunner.m_stateMachine.MoveNext);
			}
			AsyncMethodBuilderCore.ContinuationWrapper continuationWrapper = target as AsyncMethodBuilderCore.ContinuationWrapper;
			if (continuationWrapper != null)
			{
				return AsyncMethodBuilderCore.TryGetStateMachineForDebugger(continuationWrapper.m_continuation);
			}
			return action;
		}

		// Token: 0x06004690 RID: 18064 RVA: 0x000E6ECC File Offset: 0x000E50CC
		internal static Task TryGetContinuationTask(Action action)
		{
			if (action != null)
			{
				AsyncMethodBuilderCore.ContinuationWrapper continuationWrapper = action.Target as AsyncMethodBuilderCore.ContinuationWrapper;
				if (continuationWrapper != null)
				{
					return continuationWrapper.m_innerTask;
				}
			}
			return null;
		}

		// Token: 0x04002D63 RID: 11619
		internal IAsyncStateMachine m_stateMachine;

		// Token: 0x04002D64 RID: 11620
		internal Action m_defaultContextAction;

		// Token: 0x0200081F RID: 2079
		internal sealed class MoveNextRunner
		{
			// Token: 0x06004691 RID: 18065 RVA: 0x000E6EF3 File Offset: 0x000E50F3
			[SecurityCritical]
			internal MoveNextRunner(ExecutionContext context, IAsyncStateMachine stateMachine)
			{
				this.m_context = context;
				this.m_stateMachine = stateMachine;
			}

			// Token: 0x06004692 RID: 18066 RVA: 0x000E6F0C File Offset: 0x000E510C
			[SecuritySafeCritical]
			internal void Run()
			{
				if (this.m_context != null)
				{
					try
					{
						ContextCallback contextCallback = AsyncMethodBuilderCore.MoveNextRunner.s_invokeMoveNext;
						if (contextCallback == null)
						{
							contextCallback = (AsyncMethodBuilderCore.MoveNextRunner.s_invokeMoveNext = new ContextCallback(AsyncMethodBuilderCore.MoveNextRunner.InvokeMoveNext));
						}
						ExecutionContext.Run(this.m_context, contextCallback, this.m_stateMachine, true);
						return;
					}
					finally
					{
						this.m_context.Dispose();
					}
				}
				this.m_stateMachine.MoveNext();
			}

			// Token: 0x06004693 RID: 18067 RVA: 0x000E6F7C File Offset: 0x000E517C
			[SecurityCritical]
			private static void InvokeMoveNext(object stateMachine)
			{
				((IAsyncStateMachine)stateMachine).MoveNext();
			}

			// Token: 0x04002D65 RID: 11621
			private readonly ExecutionContext m_context;

			// Token: 0x04002D66 RID: 11622
			internal IAsyncStateMachine m_stateMachine;

			// Token: 0x04002D67 RID: 11623
			[SecurityCritical]
			private static ContextCallback s_invokeMoveNext;
		}

		// Token: 0x02000820 RID: 2080
		private class ContinuationWrapper
		{
			// Token: 0x06004694 RID: 18068 RVA: 0x000E6F89 File Offset: 0x000E5189
			internal ContinuationWrapper(Action continuation, Action invokeAction, Task innerTask)
			{
				if (innerTask == null)
				{
					innerTask = AsyncMethodBuilderCore.TryGetContinuationTask(continuation);
				}
				this.m_continuation = continuation;
				this.m_innerTask = innerTask;
				this.m_invokeAction = invokeAction;
			}

			// Token: 0x06004695 RID: 18069 RVA: 0x000E6FB1 File Offset: 0x000E51B1
			internal void Invoke()
			{
				this.m_invokeAction();
			}

			// Token: 0x04002D68 RID: 11624
			internal readonly Action m_continuation;

			// Token: 0x04002D69 RID: 11625
			private readonly Action m_invokeAction;

			// Token: 0x04002D6A RID: 11626
			internal readonly Task m_innerTask;
		}
	}
}
