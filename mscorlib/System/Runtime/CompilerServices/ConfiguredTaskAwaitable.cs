using System;
using System.Diagnostics;
using System.Security;
using System.Threading.Tasks;

namespace System.Runtime.CompilerServices
{
	/// <summary>Provides an awaitable object that enables configured awaits on a task.</summary>
	// Token: 0x02000814 RID: 2068
	public readonly struct ConfiguredTaskAwaitable
	{
		// Token: 0x0600464A RID: 17994 RVA: 0x000E5DFD File Offset: 0x000E3FFD
		internal ConfiguredTaskAwaitable(Task task, bool continueOnCapturedContext)
		{
			this.m_configuredTaskAwaiter = new ConfiguredTaskAwaitable.ConfiguredTaskAwaiter(task, continueOnCapturedContext);
		}

		/// <summary>Returns an awaiter for this awaitable object.</summary>
		/// <returns>The awaiter.</returns>
		// Token: 0x0600464B RID: 17995 RVA: 0x000E5E0C File Offset: 0x000E400C
		public ConfiguredTaskAwaitable.ConfiguredTaskAwaiter GetAwaiter()
		{
			return this.m_configuredTaskAwaiter;
		}

		// Token: 0x04002D4D RID: 11597
		private readonly ConfiguredTaskAwaitable.ConfiguredTaskAwaiter m_configuredTaskAwaiter;

		/// <summary>Provides an awaiter for an awaitable (<see cref="T:System.Runtime.CompilerServices.ConfiguredTaskAwaitable" />) object.</summary>
		// Token: 0x02000815 RID: 2069
		public readonly struct ConfiguredTaskAwaiter : ICriticalNotifyCompletion, INotifyCompletion, IConfiguredTaskAwaiter
		{
			// Token: 0x0600464C RID: 17996 RVA: 0x000E5E14 File Offset: 0x000E4014
			internal ConfiguredTaskAwaiter(Task task, bool continueOnCapturedContext)
			{
				this.m_task = task;
				this.m_continueOnCapturedContext = continueOnCapturedContext;
			}

			/// <summary>Gets a value that specifies whether the task being awaited is completed.</summary>
			/// <returns>
			///   <see langword="true" /> if the task being awaited is completed; otherwise, <see langword="false" />.</returns>
			/// <exception cref="T:System.NullReferenceException">The awaiter was not properly initialized.</exception>
			// Token: 0x17000AD2 RID: 2770
			// (get) Token: 0x0600464D RID: 17997 RVA: 0x000E5E24 File Offset: 0x000E4024
			public bool IsCompleted
			{
				get
				{
					return this.m_task.IsCompleted;
				}
			}

			/// <summary>Schedules the continuation action for the task associated with this awaiter.</summary>
			/// <param name="continuation">The action to invoke when the await operation completes.</param>
			/// <exception cref="T:System.ArgumentNullException">The <paramref name="continuation" /> argument is <see langword="null" />.</exception>
			/// <exception cref="T:System.NullReferenceException">The awaiter was not properly initialized.</exception>
			// Token: 0x0600464E RID: 17998 RVA: 0x000E5E31 File Offset: 0x000E4031
			[SecuritySafeCritical]
			public void OnCompleted(Action continuation)
			{
				TaskAwaiter.OnCompletedInternal(this.m_task, continuation, this.m_continueOnCapturedContext, true);
			}

			/// <summary>Schedules the continuation action for the task associated with this awaiter.</summary>
			/// <param name="continuation">The action to invoke when the await operation completes.</param>
			/// <exception cref="T:System.ArgumentNullException">The <paramref name="continuation" /> argument is <see langword="null" />.</exception>
			/// <exception cref="T:System.NullReferenceException">The awaiter was not properly initialized.</exception>
			// Token: 0x0600464F RID: 17999 RVA: 0x000E5E46 File Offset: 0x000E4046
			[SecurityCritical]
			public void UnsafeOnCompleted(Action continuation)
			{
				TaskAwaiter.OnCompletedInternal(this.m_task, continuation, this.m_continueOnCapturedContext, false);
			}

			/// <summary>Ends the await on the completed task.</summary>
			/// <exception cref="T:System.NullReferenceException">The awaiter was not properly initialized.</exception>
			/// <exception cref="T:System.Threading.Tasks.TaskCanceledException">The task was canceled.</exception>
			/// <exception cref="T:System.Exception">The task completed in a faulted state.</exception>
			// Token: 0x06004650 RID: 18000 RVA: 0x000E5E5B File Offset: 0x000E405B
			[StackTraceHidden]
			public void GetResult()
			{
				TaskAwaiter.ValidateEnd(this.m_task);
			}

			// Token: 0x04002D4E RID: 11598
			internal readonly Task m_task;

			// Token: 0x04002D4F RID: 11599
			internal readonly bool m_continueOnCapturedContext;
		}
	}
}
