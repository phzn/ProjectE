using System;
using System.Diagnostics;
using System.Security;
using System.Security.Permissions;
using System.Threading;
using System.Threading.Tasks;

namespace System.Runtime.CompilerServices
{
	/// <summary>Represents a builder for asynchronous methods that return a task.</summary>
	// Token: 0x0200081B RID: 2075
	[HostProtection(SecurityAction.LinkDemand, Synchronization = true, ExternalThreading = true)]
	public struct AsyncTaskMethodBuilder
	{
		/// <summary>Creates an instance of the <see cref="T:System.Runtime.CompilerServices.AsyncTaskMethodBuilder" /> class.</summary>
		/// <returns>A new instance of the builder.</returns>
		// Token: 0x0600466D RID: 18029 RVA: 0x000E6544 File Offset: 0x000E4744
		public static AsyncTaskMethodBuilder Create()
		{
			return default(AsyncTaskMethodBuilder);
		}

		/// <summary>Begins running the builder with the associated state machine.</summary>
		/// <param name="stateMachine">The state machine instance, passed by reference.</param>
		/// <typeparam name="TStateMachine">The type of the state machine.</typeparam>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="stateMachine" /> is <see langword="null" />.</exception>
		// Token: 0x0600466E RID: 18030 RVA: 0x000E655C File Offset: 0x000E475C
		[SecuritySafeCritical]
		[DebuggerStepThrough]
		public void Start<TStateMachine>(ref TStateMachine stateMachine) where TStateMachine : IAsyncStateMachine
		{
			if (stateMachine == null)
			{
				throw new ArgumentNullException("stateMachine");
			}
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

		/// <summary>Associates the builder with the specified state machine.</summary>
		/// <param name="stateMachine">The state machine instance to associate with the builder.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="stateMachine" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.InvalidOperationException">The state machine was previously set.</exception>
		// Token: 0x0600466F RID: 18031 RVA: 0x000E65BC File Offset: 0x000E47BC
		public void SetStateMachine(IAsyncStateMachine stateMachine)
		{
			this.m_builder.SetStateMachine(stateMachine);
		}

		/// <summary>Schedules the state machine to proceed to the next action when the specified awaiter completes.</summary>
		/// <param name="awaiter">The awaiter.</param>
		/// <param name="stateMachine">The state machine.</param>
		/// <typeparam name="TAwaiter">The type of the awaiter.</typeparam>
		/// <typeparam name="TStateMachine">The type of the state machine.</typeparam>
		// Token: 0x06004670 RID: 18032 RVA: 0x000E65CA File Offset: 0x000E47CA
		public void AwaitOnCompleted<TAwaiter, TStateMachine>(ref TAwaiter awaiter, ref TStateMachine stateMachine) where TAwaiter : INotifyCompletion where TStateMachine : IAsyncStateMachine
		{
			this.m_builder.AwaitOnCompleted<TAwaiter, TStateMachine>(ref awaiter, ref stateMachine);
		}

		/// <summary>Schedules the state machine to proceed to the next action when the specified awaiter completes. This method can be called from partially trusted code.</summary>
		/// <param name="awaiter">The awaiter.</param>
		/// <param name="stateMachine">The state machine.</param>
		/// <typeparam name="TAwaiter">The type of the awaiter.</typeparam>
		/// <typeparam name="TStateMachine">The type of the state machine.</typeparam>
		// Token: 0x06004671 RID: 18033 RVA: 0x000E65D9 File Offset: 0x000E47D9
		public void AwaitUnsafeOnCompleted<TAwaiter, TStateMachine>(ref TAwaiter awaiter, ref TStateMachine stateMachine) where TAwaiter : ICriticalNotifyCompletion where TStateMachine : IAsyncStateMachine
		{
			this.m_builder.AwaitUnsafeOnCompleted<TAwaiter, TStateMachine>(ref awaiter, ref stateMachine);
		}

		/// <summary>Gets the task for this builder.</summary>
		/// <returns>The task for this builder.</returns>
		/// <exception cref="T:System.InvalidOperationException">The builder is not initialized.</exception>
		// Token: 0x17000AD6 RID: 2774
		// (get) Token: 0x06004672 RID: 18034 RVA: 0x000E65E8 File Offset: 0x000E47E8
		public Task Task
		{
			get
			{
				return this.m_builder.Task;
			}
		}

		/// <summary>Marks the task as successfully completed.</summary>
		/// <exception cref="T:System.InvalidOperationException">The task has already completed.  
		///  -or-  
		///  The builder is not initialized.</exception>
		// Token: 0x06004673 RID: 18035 RVA: 0x000E65F5 File Offset: 0x000E47F5
		public void SetResult()
		{
			this.m_builder.SetResult(AsyncTaskMethodBuilder.s_cachedCompleted);
		}

		/// <summary>Marks the task as failed and binds the specified exception to the task.</summary>
		/// <param name="exception">The exception to bind to the task.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="exception" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.InvalidOperationException">The task has already completed.  
		///  -or-  
		///  The builder is not initialized.</exception>
		// Token: 0x06004674 RID: 18036 RVA: 0x000E6607 File Offset: 0x000E4807
		public void SetException(Exception exception)
		{
			this.m_builder.SetException(exception);
		}

		// Token: 0x06004675 RID: 18037 RVA: 0x000E6615 File Offset: 0x000E4815
		internal void SetNotificationForWaitCompletion(bool enabled)
		{
			this.m_builder.SetNotificationForWaitCompletion(enabled);
		}

		// Token: 0x17000AD7 RID: 2775
		// (get) Token: 0x06004676 RID: 18038 RVA: 0x000E6623 File Offset: 0x000E4823
		internal object ObjectIdForDebugger
		{
			get
			{
				return this.Task;
			}
		}

		// Token: 0x04002D59 RID: 11609
		private static readonly Task<VoidTaskResult> s_cachedCompleted = AsyncTaskMethodBuilder<VoidTaskResult>.s_defaultResultTask;

		// Token: 0x04002D5A RID: 11610
		private AsyncTaskMethodBuilder<VoidTaskResult> m_builder;
	}
}
