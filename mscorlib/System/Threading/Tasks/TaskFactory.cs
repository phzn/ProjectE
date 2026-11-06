using System;
using Internal.Runtime.Augments;

namespace System.Threading.Tasks
{
	/// <summary>Provides support for creating and scheduling <see cref="T:System.Threading.Tasks.Task`1" /> objects.</summary>
	/// <typeparam name="TResult">The return value of the <see cref="T:System.Threading.Tasks.Task`1" /> objects that the methods of this class create.</typeparam>
	// Token: 0x02000343 RID: 835
	public class TaskFactory<TResult>
	{
		// Token: 0x17000430 RID: 1072
		// (get) Token: 0x060022FF RID: 8959 RVA: 0x0007D5F9 File Offset: 0x0007B7F9
		private TaskScheduler DefaultScheduler
		{
			get
			{
				if (this.m_defaultScheduler == null)
				{
					return TaskScheduler.Current;
				}
				return this.m_defaultScheduler;
			}
		}

		// Token: 0x06002300 RID: 8960 RVA: 0x0007D60F File Offset: 0x0007B80F
		private TaskScheduler GetDefaultScheduler(Task currTask)
		{
			if (this.m_defaultScheduler != null)
			{
				return this.m_defaultScheduler;
			}
			if (currTask != null && (currTask.CreationOptions & TaskCreationOptions.HideScheduler) == TaskCreationOptions.None)
			{
				return currTask.ExecutingTaskScheduler;
			}
			return TaskScheduler.Default;
		}

		/// <summary>Initializes a <see cref="T:System.Threading.Tasks.TaskFactory`1" /> instance with the default configuration.</summary>
		// Token: 0x06002301 RID: 8961 RVA: 0x0007D63C File Offset: 0x0007B83C
		public TaskFactory()
			: this(default(CancellationToken), TaskCreationOptions.None, TaskContinuationOptions.None, null)
		{
		}

		/// <summary>Initializes a <see cref="T:System.Threading.Tasks.TaskFactory`1" /> instance with the default configuration.</summary>
		/// <param name="cancellationToken">The default cancellation token that will be assigned to tasks created by this <see cref="T:System.Threading.Tasks.TaskFactory" /> unless another cancellation token is explicitly specified when calling the factory methods.</param>
		// Token: 0x06002302 RID: 8962 RVA: 0x0007D65B File Offset: 0x0007B85B
		public TaskFactory(CancellationToken cancellationToken)
			: this(cancellationToken, TaskCreationOptions.None, TaskContinuationOptions.None, null)
		{
		}

		/// <summary>Initializes a <see cref="T:System.Threading.Tasks.TaskFactory`1" /> instance with the specified configuration.</summary>
		/// <param name="scheduler">The scheduler to use to schedule any tasks created with this <see cref="T:System.Threading.Tasks.TaskFactory`1" />. A null value indicates that the current <see cref="T:System.Threading.Tasks.TaskScheduler" /> should be used.</param>
		// Token: 0x06002303 RID: 8963 RVA: 0x0007D668 File Offset: 0x0007B868
		public TaskFactory(TaskScheduler scheduler)
			: this(default(CancellationToken), TaskCreationOptions.None, TaskContinuationOptions.None, scheduler)
		{
		}

		/// <summary>Initializes a <see cref="T:System.Threading.Tasks.TaskFactory`1" /> instance with the specified configuration.</summary>
		/// <param name="creationOptions">The default options to use when creating tasks with this <see cref="T:System.Threading.Tasks.TaskFactory`1" />.</param>
		/// <param name="continuationOptions">The default options to use when creating continuation tasks with this <see cref="T:System.Threading.Tasks.TaskFactory`1" />.</param>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="creationOptions" /> or <paramref name="continuationOptions" /> specifies an invalid value.</exception>
		// Token: 0x06002304 RID: 8964 RVA: 0x0007D688 File Offset: 0x0007B888
		public TaskFactory(TaskCreationOptions creationOptions, TaskContinuationOptions continuationOptions)
			: this(default(CancellationToken), creationOptions, continuationOptions, null)
		{
		}

		/// <summary>Initializes a <see cref="T:System.Threading.Tasks.TaskFactory`1" /> instance with the specified configuration.</summary>
		/// <param name="cancellationToken">The default cancellation token that will be assigned to tasks created by this <see cref="T:System.Threading.Tasks.TaskFactory" /> unless another cancellation token is explicitly specified when calling the factory methods.</param>
		/// <param name="creationOptions">The default options to use when creating tasks with this <see cref="T:System.Threading.Tasks.TaskFactory`1" />.</param>
		/// <param name="continuationOptions">The default options to use when creating continuation tasks with this <see cref="T:System.Threading.Tasks.TaskFactory`1" />.</param>
		/// <param name="scheduler">The default scheduler to use to schedule any tasks created with this <see cref="T:System.Threading.Tasks.TaskFactory`1" />. A null value indicates that <see cref="P:System.Threading.Tasks.TaskScheduler.Current" /> should be used.</param>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="creationOptions" /> or <paramref name="continuationOptions" /> specifies an invalid value.</exception>
		// Token: 0x06002305 RID: 8965 RVA: 0x0007D6A7 File Offset: 0x0007B8A7
		public TaskFactory(CancellationToken cancellationToken, TaskCreationOptions creationOptions, TaskContinuationOptions continuationOptions, TaskScheduler scheduler)
		{
			TaskFactory.CheckMultiTaskContinuationOptions(continuationOptions);
			TaskFactory.CheckCreationOptions(creationOptions);
			this.m_defaultCancellationToken = cancellationToken;
			this.m_defaultScheduler = scheduler;
			this.m_defaultCreationOptions = creationOptions;
			this.m_defaultContinuationOptions = continuationOptions;
		}

		/// <summary>Gets the default cancellation token for this task factory.</summary>
		/// <returns>The default cancellation token for this task factory.</returns>
		// Token: 0x17000431 RID: 1073
		// (get) Token: 0x06002306 RID: 8966 RVA: 0x0007D6D8 File Offset: 0x0007B8D8
		public CancellationToken CancellationToken
		{
			get
			{
				return this.m_defaultCancellationToken;
			}
		}

		/// <summary>Gets the task scheduler for this task factory.</summary>
		/// <returns>The task scheduler for this task factory.</returns>
		// Token: 0x17000432 RID: 1074
		// (get) Token: 0x06002307 RID: 8967 RVA: 0x0007D6E0 File Offset: 0x0007B8E0
		public TaskScheduler Scheduler
		{
			get
			{
				return this.m_defaultScheduler;
			}
		}

		/// <summary>Gets the <see cref="T:System.Threading.Tasks.TaskCreationOptions" /> enumeration value for this task factory.</summary>
		/// <returns>One of the enumeration values that specifies the default creation options for this task factory.</returns>
		// Token: 0x17000433 RID: 1075
		// (get) Token: 0x06002308 RID: 8968 RVA: 0x0007D6E8 File Offset: 0x0007B8E8
		public TaskCreationOptions CreationOptions
		{
			get
			{
				return this.m_defaultCreationOptions;
			}
		}

		/// <summary>Gets the <see cref="T:System.Threading.Tasks.TaskContinuationOptions" /> enumeration value for this task factory.</summary>
		/// <returns>One of the enumeration values that specifies the default continuation options for this task factory.</returns>
		// Token: 0x17000434 RID: 1076
		// (get) Token: 0x06002309 RID: 8969 RVA: 0x0007D6F0 File Offset: 0x0007B8F0
		public TaskContinuationOptions ContinuationOptions
		{
			get
			{
				return this.m_defaultContinuationOptions;
			}
		}

		/// <summary>Creates and starts a task.</summary>
		/// <param name="function">A function delegate that returns the future result to be available through the task.</param>
		/// <returns>The started task.</returns>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="function" /> argument is <see langword="null" />.</exception>
		// Token: 0x0600230A RID: 8970 RVA: 0x0007D6F8 File Offset: 0x0007B8F8
		public Task<TResult> StartNew(Func<TResult> function)
		{
			Task internalCurrent = Task.InternalCurrent;
			return Task<TResult>.StartNew(internalCurrent, function, this.m_defaultCancellationToken, this.m_defaultCreationOptions, InternalTaskOptions.None, this.GetDefaultScheduler(internalCurrent));
		}

		/// <summary>Creates and starts a task.</summary>
		/// <param name="function">A function delegate that returns the future result to be available through the task.</param>
		/// <param name="cancellationToken">The cancellation token that will be assigned to the new task.</param>
		/// <returns>The started task.</returns>
		/// <exception cref="T:System.ObjectDisposedException">The cancellation token source that created <paramref name="cancellationToken" /> has already been disposed.</exception>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="function" /> argument is <see langword="null" />.</exception>
		// Token: 0x0600230B RID: 8971 RVA: 0x0007D728 File Offset: 0x0007B928
		public Task<TResult> StartNew(Func<TResult> function, CancellationToken cancellationToken)
		{
			Task internalCurrent = Task.InternalCurrent;
			return Task<TResult>.StartNew(internalCurrent, function, cancellationToken, this.m_defaultCreationOptions, InternalTaskOptions.None, this.GetDefaultScheduler(internalCurrent));
		}

		/// <summary>Creates and starts a task.</summary>
		/// <param name="function">A function delegate that returns the future result to be available through the task.</param>
		/// <param name="creationOptions">One of the enumeration values that controls the behavior of the created task.</param>
		/// <returns>The started <see cref="T:System.Threading.Tasks.Task`1" />.</returns>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="function" /> argument is <see langword="null" />.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The <paramref name="creationOptions" /> parameter specifies an invalid value.</exception>
		// Token: 0x0600230C RID: 8972 RVA: 0x0007D754 File Offset: 0x0007B954
		public Task<TResult> StartNew(Func<TResult> function, TaskCreationOptions creationOptions)
		{
			Task internalCurrent = Task.InternalCurrent;
			return Task<TResult>.StartNew(internalCurrent, function, this.m_defaultCancellationToken, creationOptions, InternalTaskOptions.None, this.GetDefaultScheduler(internalCurrent));
		}

		/// <summary>Creates and starts a task.</summary>
		/// <param name="function">A function delegate that returns the future result to be available through the task.</param>
		/// <param name="cancellationToken">The cancellation token that will be assigned to the new task.</param>
		/// <param name="creationOptions">One of the enumeration values that controls the behavior of the created task.</param>
		/// <param name="scheduler">The task scheduler that is used to schedule the created task.</param>
		/// <returns>The started task.</returns>
		/// <exception cref="T:System.ObjectDisposedException">The cancellation token source that created <paramref name="cancellationToken" /> has already been disposed.</exception>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="function" /> argument is <see langword="null" />.  
		///  -or-  
		///  The <paramref name="scheduler" /> argument is <see langword="null" />.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The <paramref name="creationOptions" /> parameter specifies an invalid value.</exception>
		// Token: 0x0600230D RID: 8973 RVA: 0x0007D77D File Offset: 0x0007B97D
		public Task<TResult> StartNew(Func<TResult> function, CancellationToken cancellationToken, TaskCreationOptions creationOptions, TaskScheduler scheduler)
		{
			return Task<TResult>.StartNew(Task.InternalCurrentIfAttached(creationOptions), function, cancellationToken, creationOptions, InternalTaskOptions.None, scheduler);
		}

		/// <summary>Creates and starts a task.</summary>
		/// <param name="function">A function delegate that returns the future result to be available through the task.</param>
		/// <param name="state">An object that contains data to be used by the <paramref name="function" /> delegate.</param>
		/// <returns>The started task.</returns>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="function" /> argument is <see langword="null" />.</exception>
		// Token: 0x0600230E RID: 8974 RVA: 0x0007D790 File Offset: 0x0007B990
		public Task<TResult> StartNew(Func<object, TResult> function, object state)
		{
			Task internalCurrent = Task.InternalCurrent;
			return Task<TResult>.StartNew(internalCurrent, function, state, this.m_defaultCancellationToken, this.m_defaultCreationOptions, InternalTaskOptions.None, this.GetDefaultScheduler(internalCurrent));
		}

		/// <summary>Creates and starts a task.</summary>
		/// <param name="function">A function delegate that returns the future result to be available through the task.</param>
		/// <param name="state">An object that contains data to be used by the <paramref name="function" /> delegate.</param>
		/// <param name="cancellationToken">The cancellation token that will be assigned to the new task.</param>
		/// <returns>The started task.</returns>
		/// <exception cref="T:System.ObjectDisposedException">The cancellation token source that created <paramref name="cancellationToken" /> has already been disposed.</exception>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="function" /> argument is <see langword="null" />.</exception>
		// Token: 0x0600230F RID: 8975 RVA: 0x0007D7C0 File Offset: 0x0007B9C0
		public Task<TResult> StartNew(Func<object, TResult> function, object state, CancellationToken cancellationToken)
		{
			Task internalCurrent = Task.InternalCurrent;
			return Task<TResult>.StartNew(internalCurrent, function, state, cancellationToken, this.m_defaultCreationOptions, InternalTaskOptions.None, this.GetDefaultScheduler(internalCurrent));
		}

		/// <summary>Creates and starts a task.</summary>
		/// <param name="function">A function delegate that returns the future result to be available through the task.</param>
		/// <param name="state">An object that contains data to be used by the <paramref name="function" /> delegate.</param>
		/// <param name="creationOptions">One of the enumeration values that controls the behavior of the created task.</param>
		/// <returns>The started task.</returns>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="function" /> argument is <see langword="null" />.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The <paramref name="creationOptions" /> parameter specifies an invalid value.</exception>
		// Token: 0x06002310 RID: 8976 RVA: 0x0007D7EC File Offset: 0x0007B9EC
		public Task<TResult> StartNew(Func<object, TResult> function, object state, TaskCreationOptions creationOptions)
		{
			Task internalCurrent = Task.InternalCurrent;
			return Task<TResult>.StartNew(internalCurrent, function, state, this.m_defaultCancellationToken, creationOptions, InternalTaskOptions.None, this.GetDefaultScheduler(internalCurrent));
		}

		/// <summary>Creates and starts a task.</summary>
		/// <param name="function">A function delegate that returns the future result to be available through the task.</param>
		/// <param name="state">An object that contains data to be used by the <paramref name="function" /> delegate.</param>
		/// <param name="cancellationToken">The cancellation token that will be assigned to the new task.</param>
		/// <param name="creationOptions">One of the enumeration values that controls the behavior of the created task.</param>
		/// <param name="scheduler">The task scheduler that is used to schedule the created task.</param>
		/// <returns>The started task.</returns>
		/// <exception cref="T:System.ObjectDisposedException">The cancellation token source that created <paramref name="cancellationToken" /> has already been disposed.</exception>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="function" /> argument is <see langword="null" />.  
		///  -or-  
		///  The <paramref name="scheduler" /> argument is <see langword="null" />.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The <paramref name="creationOptions" /> parameter specifies an invalid value.</exception>
		// Token: 0x06002311 RID: 8977 RVA: 0x0007D816 File Offset: 0x0007BA16
		public Task<TResult> StartNew(Func<object, TResult> function, object state, CancellationToken cancellationToken, TaskCreationOptions creationOptions, TaskScheduler scheduler)
		{
			return Task<TResult>.StartNew(Task.InternalCurrentIfAttached(creationOptions), function, state, cancellationToken, creationOptions, InternalTaskOptions.None, scheduler);
		}

		// Token: 0x06002312 RID: 8978 RVA: 0x0007D82C File Offset: 0x0007BA2C
		private static void FromAsyncCoreLogic(IAsyncResult iar, Func<IAsyncResult, TResult> endFunction, Action<IAsyncResult> endAction, Task<TResult> promise, bool requiresSynchronization)
		{
			Exception ex = null;
			OperationCanceledException ex2 = null;
			TResult tresult = default(TResult);
			try
			{
				if (endFunction != null)
				{
					tresult = endFunction(iar);
				}
				else
				{
					endAction(iar);
				}
			}
			catch (OperationCanceledException ex2)
			{
			}
			catch (Exception ex)
			{
			}
			finally
			{
				if (ex2 != null)
				{
					promise.TrySetCanceled(ex2.CancellationToken, ex2);
				}
				else if (ex != null)
				{
					promise.TrySetException(ex);
				}
				else
				{
					if (DebuggerSupport.LoggingOn)
					{
						DebuggerSupport.TraceOperationCompletion(CausalityTraceLevel.Required, promise, AsyncStatus.Completed);
					}
					DebuggerSupport.RemoveFromActiveTasks(promise);
					if (requiresSynchronization)
					{
						promise.TrySetResult(tresult);
					}
					else
					{
						promise.DangerousSetResult(tresult);
					}
				}
			}
		}

		/// <summary>Creates a task that executes an end method function when a specified <see cref="T:System.IAsyncResult" /> completes.</summary>
		/// <param name="asyncResult">The <see cref="T:System.IAsyncResult" /> whose completion should trigger the processing of the <paramref name="endMethod" />.</param>
		/// <param name="endMethod">The function delegate that processes the completed <paramref name="asyncResult" />.</param>
		/// <returns>A <see cref="T:System.Threading.Tasks.Task`1" /> that represents the asynchronous operation.</returns>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="asyncResult" /> argument is <see langword="null" />.  
		///  -or-  
		///  The <paramref name="endMethod" /> argument is <see langword="null" />.</exception>
		// Token: 0x06002313 RID: 8979 RVA: 0x0007D8D4 File Offset: 0x0007BAD4
		public Task<TResult> FromAsync(IAsyncResult asyncResult, Func<IAsyncResult, TResult> endMethod)
		{
			return TaskFactory<TResult>.FromAsyncImpl(asyncResult, endMethod, null, this.m_defaultCreationOptions, this.DefaultScheduler);
		}

		/// <summary>Creates a task that executes an end method function when a specified <see cref="T:System.IAsyncResult" /> completes.</summary>
		/// <param name="asyncResult">The <see cref="T:System.IAsyncResult" /> whose completion should trigger the processing of the <paramref name="endMethod" />.</param>
		/// <param name="endMethod">The function delegate that processes the completed <paramref name="asyncResult" />.</param>
		/// <param name="creationOptions">One of the enumeration values that controls the behavior of the created task.</param>
		/// <returns>A task that represents the asynchronous operation.</returns>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="asyncResult" /> argument is <see langword="null" />.  
		///  -or-  
		///  The <paramref name="endMethod" /> argument is <see langword="null" />.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The <paramref name="creationOptions" /> argument specifies an invalid value.</exception>
		// Token: 0x06002314 RID: 8980 RVA: 0x0007D8EA File Offset: 0x0007BAEA
		public Task<TResult> FromAsync(IAsyncResult asyncResult, Func<IAsyncResult, TResult> endMethod, TaskCreationOptions creationOptions)
		{
			return TaskFactory<TResult>.FromAsyncImpl(asyncResult, endMethod, null, creationOptions, this.DefaultScheduler);
		}

		/// <summary>Creates a task that executes an end method function when a specified <see cref="T:System.IAsyncResult" /> completes.</summary>
		/// <param name="asyncResult">The <see cref="T:System.IAsyncResult" /> whose completion should trigger the processing of the <paramref name="endMethod" />.</param>
		/// <param name="endMethod">The function delegate that processes the completed <paramref name="asyncResult" />.</param>
		/// <param name="creationOptions">One of the enumeration values that controls the behavior of the created task.</param>
		/// <param name="scheduler">The task scheduler that is used to schedule the task that executes the end method.</param>
		/// <returns>The created task that represents the asynchronous operation.</returns>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="asyncResult" /> argument is <see langword="null" />.  
		///  -or-  
		///  The <paramref name="endMethod" /> argument is <see langword="null" />.  
		///  -or-  
		///  The <paramref name="scheduler" /> argument is <see langword="null" />.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The <paramref name="creationOptions" /> parameter specifies an invalid value.</exception>
		// Token: 0x06002315 RID: 8981 RVA: 0x0007D8FB File Offset: 0x0007BAFB
		public Task<TResult> FromAsync(IAsyncResult asyncResult, Func<IAsyncResult, TResult> endMethod, TaskCreationOptions creationOptions, TaskScheduler scheduler)
		{
			return TaskFactory<TResult>.FromAsyncImpl(asyncResult, endMethod, null, creationOptions, scheduler);
		}

		// Token: 0x06002316 RID: 8982 RVA: 0x0007D908 File Offset: 0x0007BB08
		internal static Task<TResult> FromAsyncImpl(IAsyncResult asyncResult, Func<IAsyncResult, TResult> endFunction, Action<IAsyncResult> endAction, TaskCreationOptions creationOptions, TaskScheduler scheduler)
		{
			if (asyncResult == null)
			{
				throw new ArgumentNullException("asyncResult");
			}
			if (endFunction == null && endAction == null)
			{
				throw new ArgumentNullException("endMethod");
			}
			if (scheduler == null)
			{
				throw new ArgumentNullException("scheduler");
			}
			TaskFactory.CheckFromAsyncOptions(creationOptions, false);
			Task<TResult> promise = new Task<TResult>(null, creationOptions);
			Task t = new Task(new Action<object>(delegate
			{
				TaskFactory<TResult>.FromAsyncCoreLogic(asyncResult, endFunction, endAction, promise, true);
			}), null, null, default(CancellationToken), TaskCreationOptions.None, InternalTaskOptions.None, null);
			if (asyncResult.IsCompleted)
			{
				try
				{
					t.InternalRunSynchronously(scheduler, false);
					goto IL_00EE;
				}
				catch (Exception ex)
				{
					promise.TrySetException(ex);
					goto IL_00EE;
				}
			}
			ThreadPool.RegisterWaitForSingleObject(asyncResult.AsyncWaitHandle, delegate
			{
				try
				{
					t.InternalRunSynchronously(scheduler, false);
				}
				catch (Exception ex2)
				{
					promise.TrySetException(ex2);
				}
			}, null, -1, true);
			IL_00EE:
			return promise;
		}

		/// <summary>Creates a task that represents a pair of begin and end methods that conform to the Asynchronous Programming Model pattern.</summary>
		/// <param name="beginMethod">The delegate that begins the asynchronous operation.</param>
		/// <param name="endMethod">The delegate that ends the asynchronous operation.</param>
		/// <param name="state">An object containing data to be used by the <paramref name="beginMethod" /> delegate.</param>
		/// <returns>The created task that represents the asynchronous operation.</returns>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="beginMethod" /> argument is <see langword="null" />.  
		///  -or-  
		///  The <paramref name="endMethod" /> argument is <see langword="null" />.</exception>
		// Token: 0x06002317 RID: 8983 RVA: 0x0007DA1C File Offset: 0x0007BC1C
		public Task<TResult> FromAsync(Func<AsyncCallback, object, IAsyncResult> beginMethod, Func<IAsyncResult, TResult> endMethod, object state)
		{
			return TaskFactory<TResult>.FromAsyncImpl(beginMethod, endMethod, null, state, this.m_defaultCreationOptions);
		}

		/// <summary>Creates a task that represents a pair of begin and end methods that conform to the Asynchronous Programming Model pattern.</summary>
		/// <param name="beginMethod">The delegate that begins the asynchronous operation.</param>
		/// <param name="endMethod">The delegate that ends the asynchronous operation.</param>
		/// <param name="state">An object containing data to be used by the <paramref name="beginMethod" /> delegate.</param>
		/// <param name="creationOptions">One of the enumeration values that controls the behavior of the created task.</param>
		/// <returns>The created <see cref="T:System.Threading.Tasks.Task`1" /> that represents the asynchronous operation.</returns>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="beginMethod" /> argument is <see langword="null" />.  
		///  -or-  
		///  The <paramref name="endMethod" /> argument is <see langword="null" />.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The <paramref name="creationOptions" /> argument specifies an invalid value.</exception>
		// Token: 0x06002318 RID: 8984 RVA: 0x0007DA2D File Offset: 0x0007BC2D
		public Task<TResult> FromAsync(Func<AsyncCallback, object, IAsyncResult> beginMethod, Func<IAsyncResult, TResult> endMethod, object state, TaskCreationOptions creationOptions)
		{
			return TaskFactory<TResult>.FromAsyncImpl(beginMethod, endMethod, null, state, creationOptions);
		}

		// Token: 0x06002319 RID: 8985 RVA: 0x0007DA3C File Offset: 0x0007BC3C
		internal static Task<TResult> FromAsyncImpl(Func<AsyncCallback, object, IAsyncResult> beginMethod, Func<IAsyncResult, TResult> endFunction, Action<IAsyncResult> endAction, object state, TaskCreationOptions creationOptions)
		{
			if (beginMethod == null)
			{
				throw new ArgumentNullException("beginMethod");
			}
			if (endFunction == null && endAction == null)
			{
				throw new ArgumentNullException("endMethod");
			}
			TaskFactory.CheckFromAsyncOptions(creationOptions, true);
			Task<TResult> promise = new Task<TResult>(state, creationOptions);
			if (DebuggerSupport.LoggingOn)
			{
				DebuggerSupport.TraceOperationCreation(CausalityTraceLevel.Required, promise, "TaskFactory.FromAsync: " + ((beginMethod != null) ? beginMethod.ToString() : null), 0UL);
			}
			DebuggerSupport.AddToActiveTasks(promise);
			try
			{
				IAsyncResult asyncResult = beginMethod(delegate(IAsyncResult iar)
				{
					if (!iar.CompletedSynchronously)
					{
						TaskFactory<TResult>.FromAsyncCoreLogic(iar, endFunction, endAction, promise, true);
					}
				}, state);
				if (asyncResult.CompletedSynchronously)
				{
					TaskFactory<TResult>.FromAsyncCoreLogic(asyncResult, endFunction, endAction, promise, false);
				}
			}
			catch
			{
				if (DebuggerSupport.LoggingOn)
				{
					DebuggerSupport.TraceOperationCompletion(CausalityTraceLevel.Required, promise, AsyncStatus.Error);
				}
				DebuggerSupport.RemoveFromActiveTasks(promise);
				promise.TrySetResult(default(TResult));
				throw;
			}
			return promise;
		}

		/// <summary>Creates a task that represents a pair of begin and end methods that conform to the Asynchronous Programming Model pattern.</summary>
		/// <param name="beginMethod">The delegate that begins the asynchronous operation.</param>
		/// <param name="endMethod">The delegate that ends the asynchronous operation.</param>
		/// <param name="arg1">The first argument passed to the <paramref name="beginMethod" /> delegate.</param>
		/// <param name="state">An object containing data to be used by the <paramref name="beginMethod" /> delegate.</param>
		/// <typeparam name="TArg1">The type of the first argument passed to the <paramref name="beginMethod" /> delegate.</typeparam>
		/// <returns>The created task that represents the asynchronous operation.</returns>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="beginMethod" /> argument is <see langword="null" />.  
		///  -or-  
		///  The <paramref name="endMethod" /> argument is <see langword="null" />.</exception>
		// Token: 0x0600231A RID: 8986 RVA: 0x0007DB58 File Offset: 0x0007BD58
		public Task<TResult> FromAsync<TArg1>(Func<TArg1, AsyncCallback, object, IAsyncResult> beginMethod, Func<IAsyncResult, TResult> endMethod, TArg1 arg1, object state)
		{
			return TaskFactory<TResult>.FromAsyncImpl<TArg1>(beginMethod, endMethod, null, arg1, state, this.m_defaultCreationOptions);
		}

		/// <summary>Creates a task that represents a pair of begin and end methods that conform to the Asynchronous Programming Model pattern.</summary>
		/// <param name="beginMethod">The delegate that begins the asynchronous operation.</param>
		/// <param name="endMethod">The delegate that ends the asynchronous operation.</param>
		/// <param name="arg1">The first argument passed to the <paramref name="beginMethod" /> delegate.</param>
		/// <param name="state">An object containing data to be used by the <paramref name="beginMethod" /> delegate.</param>
		/// <param name="creationOptions">One of the enumeration values that controls the behavior of the created task.</param>
		/// <typeparam name="TArg1">The type of the first argument passed to the <paramref name="beginMethod" /> delegate.</typeparam>
		/// <returns>The created task that represents the asynchronous operation.</returns>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="beginMethod" /> argument is <see langword="null" />.  
		///  -or-  
		///  The <paramref name="endMethod" /> argument is <see langword="null" />.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The <paramref name="creationOptions" /> parameter specifies an invalid value.</exception>
		// Token: 0x0600231B RID: 8987 RVA: 0x0007DB6B File Offset: 0x0007BD6B
		public Task<TResult> FromAsync<TArg1>(Func<TArg1, AsyncCallback, object, IAsyncResult> beginMethod, Func<IAsyncResult, TResult> endMethod, TArg1 arg1, object state, TaskCreationOptions creationOptions)
		{
			return TaskFactory<TResult>.FromAsyncImpl<TArg1>(beginMethod, endMethod, null, arg1, state, creationOptions);
		}

		// Token: 0x0600231C RID: 8988 RVA: 0x0007DB7C File Offset: 0x0007BD7C
		internal static Task<TResult> FromAsyncImpl<TArg1>(Func<TArg1, AsyncCallback, object, IAsyncResult> beginMethod, Func<IAsyncResult, TResult> endFunction, Action<IAsyncResult> endAction, TArg1 arg1, object state, TaskCreationOptions creationOptions)
		{
			if (beginMethod == null)
			{
				throw new ArgumentNullException("beginMethod");
			}
			if (endFunction == null && endAction == null)
			{
				throw new ArgumentNullException("endFunction");
			}
			TaskFactory.CheckFromAsyncOptions(creationOptions, true);
			Task<TResult> promise = new Task<TResult>(state, creationOptions);
			if (DebuggerSupport.LoggingOn)
			{
				DebuggerSupport.TraceOperationCreation(CausalityTraceLevel.Required, promise, "TaskFactory.FromAsync: " + ((beginMethod != null) ? beginMethod.ToString() : null), 0UL);
			}
			DebuggerSupport.AddToActiveTasks(promise);
			try
			{
				IAsyncResult asyncResult = beginMethod(arg1, delegate(IAsyncResult iar)
				{
					if (!iar.CompletedSynchronously)
					{
						TaskFactory<TResult>.FromAsyncCoreLogic(iar, endFunction, endAction, promise, true);
					}
				}, state);
				if (asyncResult.CompletedSynchronously)
				{
					TaskFactory<TResult>.FromAsyncCoreLogic(asyncResult, endFunction, endAction, promise, false);
				}
			}
			catch
			{
				if (DebuggerSupport.LoggingOn)
				{
					DebuggerSupport.TraceOperationCompletion(CausalityTraceLevel.Required, promise, AsyncStatus.Error);
				}
				DebuggerSupport.RemoveFromActiveTasks(promise);
				promise.TrySetResult(default(TResult));
				throw;
			}
			return promise;
		}

		/// <summary>Creates a task that represents a pair of begin and end methods that conform to the Asynchronous Programming Model pattern.</summary>
		/// <param name="beginMethod">The delegate that begins the asynchronous operation.</param>
		/// <param name="endMethod">The delegate that ends the asynchronous operation.</param>
		/// <param name="arg1">The first argument passed to the <paramref name="beginMethod" /> delegate.</param>
		/// <param name="arg2">The second argument passed to the <paramref name="beginMethod" /> delegate.</param>
		/// <param name="state">An object containing data to be used by the <paramref name="beginMethod" /> delegate.</param>
		/// <typeparam name="TArg1">The type of the second argument passed to <paramref name="beginMethod" /> delegate.</typeparam>
		/// <typeparam name="TArg2">The type of the first argument passed to the <paramref name="beginMethod" /> delegate.</typeparam>
		/// <returns>The created task that represents the asynchronous operation.</returns>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="beginMethod" /> argument is <see langword="null" />.  
		///  -or-  
		///  The <paramref name="endMethod" /> argument is <see langword="null" />.</exception>
		// Token: 0x0600231D RID: 8989 RVA: 0x0007DC9C File Offset: 0x0007BE9C
		public Task<TResult> FromAsync<TArg1, TArg2>(Func<TArg1, TArg2, AsyncCallback, object, IAsyncResult> beginMethod, Func<IAsyncResult, TResult> endMethod, TArg1 arg1, TArg2 arg2, object state)
		{
			return TaskFactory<TResult>.FromAsyncImpl<TArg1, TArg2>(beginMethod, endMethod, null, arg1, arg2, state, this.m_defaultCreationOptions);
		}

		/// <summary>Creates a task that represents a pair of begin and end methods that conform to the Asynchronous Programming Model pattern.</summary>
		/// <param name="beginMethod">The delegate that begins the asynchronous operation.</param>
		/// <param name="endMethod">The delegate that ends the asynchronous operation.</param>
		/// <param name="arg1">The first argument passed to the <paramref name="beginMethod" /> delegate.</param>
		/// <param name="arg2">The second argument passed to the <paramref name="beginMethod" /> delegate.</param>
		/// <param name="state">An object containing data to be used by the <paramref name="beginMethod" /> delegate.</param>
		/// <param name="creationOptions">An object that controls the behavior of the created <see cref="T:System.Threading.Tasks.Task`1" />.</param>
		/// <typeparam name="TArg1">The type of the second argument passed to <paramref name="beginMethod" /> delegate.</typeparam>
		/// <typeparam name="TArg2">The type of the first argument passed to the <paramref name="beginMethod" /> delegate.</typeparam>
		/// <returns>The created task that represents the asynchronous operation.</returns>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="beginMethod" /> argument is <see langword="null" />.  
		///  -or-  
		///  The <paramref name="endMethod" /> argument is <see langword="null" />.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The <paramref name="creationOptions" /> parameter specifies an invalid value.</exception>
		// Token: 0x0600231E RID: 8990 RVA: 0x0007DCB1 File Offset: 0x0007BEB1
		public Task<TResult> FromAsync<TArg1, TArg2>(Func<TArg1, TArg2, AsyncCallback, object, IAsyncResult> beginMethod, Func<IAsyncResult, TResult> endMethod, TArg1 arg1, TArg2 arg2, object state, TaskCreationOptions creationOptions)
		{
			return TaskFactory<TResult>.FromAsyncImpl<TArg1, TArg2>(beginMethod, endMethod, null, arg1, arg2, state, creationOptions);
		}

		// Token: 0x0600231F RID: 8991 RVA: 0x0007DCC4 File Offset: 0x0007BEC4
		internal static Task<TResult> FromAsyncImpl<TArg1, TArg2>(Func<TArg1, TArg2, AsyncCallback, object, IAsyncResult> beginMethod, Func<IAsyncResult, TResult> endFunction, Action<IAsyncResult> endAction, TArg1 arg1, TArg2 arg2, object state, TaskCreationOptions creationOptions)
		{
			if (beginMethod == null)
			{
				throw new ArgumentNullException("beginMethod");
			}
			if (endFunction == null && endAction == null)
			{
				throw new ArgumentNullException("endMethod");
			}
			TaskFactory.CheckFromAsyncOptions(creationOptions, true);
			Task<TResult> promise = new Task<TResult>(state, creationOptions);
			if (DebuggerSupport.LoggingOn)
			{
				DebuggerSupport.TraceOperationCreation(CausalityTraceLevel.Required, promise, "TaskFactory.FromAsync: " + ((beginMethod != null) ? beginMethod.ToString() : null), 0UL);
			}
			DebuggerSupport.AddToActiveTasks(promise);
			try
			{
				IAsyncResult asyncResult = beginMethod(arg1, arg2, delegate(IAsyncResult iar)
				{
					if (!iar.CompletedSynchronously)
					{
						TaskFactory<TResult>.FromAsyncCoreLogic(iar, endFunction, endAction, promise, true);
					}
				}, state);
				if (asyncResult.CompletedSynchronously)
				{
					TaskFactory<TResult>.FromAsyncCoreLogic(asyncResult, endFunction, endAction, promise, false);
				}
			}
			catch
			{
				if (DebuggerSupport.LoggingOn)
				{
					DebuggerSupport.TraceOperationCompletion(CausalityTraceLevel.Required, promise, AsyncStatus.Error);
				}
				DebuggerSupport.RemoveFromActiveTasks(promise);
				promise.TrySetResult(default(TResult));
				throw;
			}
			return promise;
		}

		/// <summary>Creates a task that represents a pair of begin and end methods that conform to the Asynchronous Programming Model pattern.</summary>
		/// <param name="beginMethod">The delegate that begins the asynchronous operation.</param>
		/// <param name="endMethod">The delegate that ends the asynchronous operation.</param>
		/// <param name="arg1">The first argument passed to the <paramref name="beginMethod" /> delegate.</param>
		/// <param name="arg2">The second argument passed to the <paramref name="beginMethod" /> delegate.</param>
		/// <param name="arg3">The third argument passed to the <paramref name="beginMethod" /> delegate.</param>
		/// <param name="state">An object containing data to be used by the <paramref name="beginMethod" /> delegate.</param>
		/// <typeparam name="TArg1">The type of the second argument passed to <paramref name="beginMethod" /> delegate.</typeparam>
		/// <typeparam name="TArg2">The type of the third argument passed to <paramref name="beginMethod" /> delegate.</typeparam>
		/// <typeparam name="TArg3">The type of the first argument passed to the <paramref name="beginMethod" /> delegate.</typeparam>
		/// <returns>The created task that represents the asynchronous operation.</returns>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="beginMethod" /> argument is <see langword="null" />.  
		///  -or-  
		///  The <paramref name="endMethod" /> argument is <see langword="null" />.</exception>
		// Token: 0x06002320 RID: 8992 RVA: 0x0007DDE4 File Offset: 0x0007BFE4
		public Task<TResult> FromAsync<TArg1, TArg2, TArg3>(Func<TArg1, TArg2, TArg3, AsyncCallback, object, IAsyncResult> beginMethod, Func<IAsyncResult, TResult> endMethod, TArg1 arg1, TArg2 arg2, TArg3 arg3, object state)
		{
			return TaskFactory<TResult>.FromAsyncImpl<TArg1, TArg2, TArg3>(beginMethod, endMethod, null, arg1, arg2, arg3, state, this.m_defaultCreationOptions);
		}

		/// <summary>Creates a task that represents a pair of begin and end methods that conform to the Asynchronous Programming Model pattern.</summary>
		/// <param name="beginMethod">The delegate that begins the asynchronous operation.</param>
		/// <param name="endMethod">The delegate that ends the asynchronous operation.</param>
		/// <param name="arg1">The first argument passed to the <paramref name="beginMethod" /> delegate.</param>
		/// <param name="arg2">The second argument passed to the <paramref name="beginMethod" /> delegate.</param>
		/// <param name="arg3">The third argument passed to the <paramref name="beginMethod" /> delegate.</param>
		/// <param name="state">An object containing data to be used by the <paramref name="beginMethod" /> delegate.</param>
		/// <param name="creationOptions">An object that controls the behavior of the created task.</param>
		/// <typeparam name="TArg1">The type of the second argument passed to <paramref name="beginMethod" /> delegate.</typeparam>
		/// <typeparam name="TArg2">The type of the third argument passed to <paramref name="beginMethod" /> delegate.</typeparam>
		/// <typeparam name="TArg3">The type of the first argument passed to the <paramref name="beginMethod" /> delegate.</typeparam>
		/// <returns>The created task that represents the asynchronous operation.</returns>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="beginMethod" /> argument is <see langword="null" />.  
		///  -or-  
		///  The <paramref name="endMethod" /> argument is <see langword="null" />.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The <paramref name="creationOptions" /> parameter specifies an invalid value.</exception>
		// Token: 0x06002321 RID: 8993 RVA: 0x0007DDFB File Offset: 0x0007BFFB
		public Task<TResult> FromAsync<TArg1, TArg2, TArg3>(Func<TArg1, TArg2, TArg3, AsyncCallback, object, IAsyncResult> beginMethod, Func<IAsyncResult, TResult> endMethod, TArg1 arg1, TArg2 arg2, TArg3 arg3, object state, TaskCreationOptions creationOptions)
		{
			return TaskFactory<TResult>.FromAsyncImpl<TArg1, TArg2, TArg3>(beginMethod, endMethod, null, arg1, arg2, arg3, state, creationOptions);
		}

		// Token: 0x06002322 RID: 8994 RVA: 0x0007DE10 File Offset: 0x0007C010
		internal static Task<TResult> FromAsyncImpl<TArg1, TArg2, TArg3>(Func<TArg1, TArg2, TArg3, AsyncCallback, object, IAsyncResult> beginMethod, Func<IAsyncResult, TResult> endFunction, Action<IAsyncResult> endAction, TArg1 arg1, TArg2 arg2, TArg3 arg3, object state, TaskCreationOptions creationOptions)
		{
			if (beginMethod == null)
			{
				throw new ArgumentNullException("beginMethod");
			}
			if (endFunction == null && endAction == null)
			{
				throw new ArgumentNullException("endMethod");
			}
			TaskFactory.CheckFromAsyncOptions(creationOptions, true);
			Task<TResult> promise = new Task<TResult>(state, creationOptions);
			if (DebuggerSupport.LoggingOn)
			{
				DebuggerSupport.TraceOperationCreation(CausalityTraceLevel.Required, promise, "TaskFactory.FromAsync: " + ((beginMethod != null) ? beginMethod.ToString() : null), 0UL);
			}
			DebuggerSupport.AddToActiveTasks(promise);
			try
			{
				IAsyncResult asyncResult = beginMethod(arg1, arg2, arg3, delegate(IAsyncResult iar)
				{
					if (!iar.CompletedSynchronously)
					{
						TaskFactory<TResult>.FromAsyncCoreLogic(iar, endFunction, endAction, promise, true);
					}
				}, state);
				if (asyncResult.CompletedSynchronously)
				{
					TaskFactory<TResult>.FromAsyncCoreLogic(asyncResult, endFunction, endAction, promise, false);
				}
			}
			catch
			{
				if (DebuggerSupport.LoggingOn)
				{
					DebuggerSupport.TraceOperationCompletion(CausalityTraceLevel.Required, promise, AsyncStatus.Error);
				}
				DebuggerSupport.RemoveFromActiveTasks(promise);
				promise.TrySetResult(default(TResult));
				throw;
			}
			return promise;
		}

		// Token: 0x06002323 RID: 8995 RVA: 0x0007DF34 File Offset: 0x0007C134
		internal static Task<TResult> FromAsyncTrim<TInstance, TArgs>(TInstance thisRef, TArgs args, Func<TInstance, TArgs, AsyncCallback, object, IAsyncResult> beginMethod, Func<TInstance, IAsyncResult, TResult> endMethod) where TInstance : class
		{
			TaskFactory<TResult>.FromAsyncTrimPromise<TInstance> fromAsyncTrimPromise = new TaskFactory<TResult>.FromAsyncTrimPromise<TInstance>(thisRef, endMethod);
			IAsyncResult asyncResult = beginMethod(thisRef, args, TaskFactory<TResult>.FromAsyncTrimPromise<TInstance>.s_completeFromAsyncResult, fromAsyncTrimPromise);
			if (asyncResult.CompletedSynchronously)
			{
				fromAsyncTrimPromise.Complete(thisRef, endMethod, asyncResult, false);
			}
			return fromAsyncTrimPromise;
		}

		// Token: 0x06002324 RID: 8996 RVA: 0x0007DF6C File Offset: 0x0007C16C
		private static Task<TResult> CreateCanceledTask(TaskContinuationOptions continuationOptions, CancellationToken ct)
		{
			TaskCreationOptions taskCreationOptions;
			InternalTaskOptions internalTaskOptions;
			Task.CreationOptionsFromContinuationOptions(continuationOptions, out taskCreationOptions, out internalTaskOptions);
			return new Task<TResult>(true, default(TResult), taskCreationOptions, ct);
		}

		/// <summary>Creates a continuation task that will be started upon the completion of a set of provided tasks.</summary>
		/// <param name="tasks">The array of tasks from which to continue.</param>
		/// <param name="continuationFunction">The function delegate to execute asynchronously when all tasks in the <paramref name="tasks" /> array have completed.</param>
		/// <returns>The new continuation task.</returns>
		/// <exception cref="T:System.ObjectDisposedException">One of the elements in the <paramref name="tasks" /> array has been disposed.</exception>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="tasks" /> array is <see langword="null" />.  
		/// -or-  
		/// The <paramref name="continuationFunction" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.ArgumentException">The <paramref name="tasks" /> array contains a null value or is empty.</exception>
		// Token: 0x06002325 RID: 8997 RVA: 0x0007DF94 File Offset: 0x0007C194
		public Task<TResult> ContinueWhenAll(Task[] tasks, Func<Task[], TResult> continuationFunction)
		{
			if (continuationFunction == null)
			{
				throw new ArgumentNullException("continuationFunction");
			}
			return TaskFactory<TResult>.ContinueWhenAllImpl(tasks, continuationFunction, this.m_defaultContinuationOptions, this.m_defaultCancellationToken, this.DefaultScheduler);
		}

		/// <summary>Creates a continuation task that will be started upon the completion of a set of provided tasks.</summary>
		/// <param name="tasks">The array of tasks from which to continue.</param>
		/// <param name="continuationFunction">The function delegate to execute asynchronously when all tasks in the <paramref name="tasks" /> array have completed.</param>
		/// <param name="cancellationToken">The cancellation token that will be assigned to the new continuation task.</param>
		/// <returns>The new continuation task.</returns>
		/// <exception cref="T:System.ObjectDisposedException">One of the elements in the <paramref name="tasks" /> array has been disposed.  
		///  -or-  
		///  The <see cref="T:System.Threading.CancellationTokenSource" /> that created <paramref name="cancellationToken" /> has already been disposed.</exception>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="tasks" /> array is <see langword="null" />.  
		///  -or-  
		///  <paramref name="continuationFunction" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.ArgumentException">The <paramref name="tasks" /> array contains a null value or is empty.</exception>
		// Token: 0x06002326 RID: 8998 RVA: 0x0007DFBD File Offset: 0x0007C1BD
		public Task<TResult> ContinueWhenAll(Task[] tasks, Func<Task[], TResult> continuationFunction, CancellationToken cancellationToken)
		{
			if (continuationFunction == null)
			{
				throw new ArgumentNullException("continuationFunction");
			}
			return TaskFactory<TResult>.ContinueWhenAllImpl(tasks, continuationFunction, this.m_defaultContinuationOptions, cancellationToken, this.DefaultScheduler);
		}

		/// <summary>Creates a continuation task that will be started upon the completion of a set of provided Tasks.</summary>
		/// <param name="tasks">The array of tasks from which to continue.</param>
		/// <param name="continuationFunction">The function delegate to execute asynchronously when all tasks in the <paramref name="tasks" /> array have completed.</param>
		/// <param name="continuationOptions">One of the enumeration values that controls the behavior of the created continuation task. The NotOn* or OnlyOn* values are not valid.</param>
		/// <returns>The new continuation task.</returns>
		/// <exception cref="T:System.ObjectDisposedException">One of the elements in the <paramref name="tasks" /> array has been disposed.</exception>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="tasks" /> array is <see langword="null" />.  
		///  -or-  
		///  The <paramref name="continuationFunction" /> argument is <see langword="null" />.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The <paramref name="continuationOptions" /> argument specifies an invalid value.</exception>
		/// <exception cref="T:System.ArgumentException">The <paramref name="tasks" /> array contains a null value or is empty.</exception>
		// Token: 0x06002327 RID: 8999 RVA: 0x0007DFE1 File Offset: 0x0007C1E1
		public Task<TResult> ContinueWhenAll(Task[] tasks, Func<Task[], TResult> continuationFunction, TaskContinuationOptions continuationOptions)
		{
			if (continuationFunction == null)
			{
				throw new ArgumentNullException("continuationFunction");
			}
			return TaskFactory<TResult>.ContinueWhenAllImpl(tasks, continuationFunction, continuationOptions, this.m_defaultCancellationToken, this.DefaultScheduler);
		}

		/// <summary>Creates a continuation task that will be started upon the completion of a set of provided Tasks.</summary>
		/// <param name="tasks">The array of tasks from which to continue.</param>
		/// <param name="continuationFunction">The function delegate to execute asynchronously when all tasks in the <paramref name="tasks" /> array have completed.</param>
		/// <param name="cancellationToken">The cancellation token that will be assigned to the new continuation task.</param>
		/// <param name="continuationOptions">One of the enumeration values that controls the behavior of the created continuation task. The NotOn* or OnlyOn* values are not valid.</param>
		/// <param name="scheduler">The scheduler that is used to schedule the created continuation task.</param>
		/// <returns>The new continuation task.</returns>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="tasks" /> array is <see langword="null" />.  
		///  -or-  
		///  The <paramref name="continuationFunction" /> argument is <see langword="null" />.  
		///  -or-  
		///  The <paramref name="scheduler" /> argument is <see langword="null" />.</exception>
		/// <exception cref="T:System.ArgumentException">The <paramref name="tasks" /> array contains a null value or is empty.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="continuationOptions" /> specifies an invalid value.</exception>
		/// <exception cref="T:System.ObjectDisposedException">One of the elements in the <paramref name="tasks" /> array has been disposed.  
		///  -or-  
		///  The <see cref="T:System.Threading.CancellationTokenSource" /> that created <paramref name="cancellationToken" /> has already been disposed.</exception>
		// Token: 0x06002328 RID: 9000 RVA: 0x0007E005 File Offset: 0x0007C205
		public Task<TResult> ContinueWhenAll(Task[] tasks, Func<Task[], TResult> continuationFunction, CancellationToken cancellationToken, TaskContinuationOptions continuationOptions, TaskScheduler scheduler)
		{
			if (continuationFunction == null)
			{
				throw new ArgumentNullException("continuationFunction");
			}
			return TaskFactory<TResult>.ContinueWhenAllImpl(tasks, continuationFunction, continuationOptions, cancellationToken, scheduler);
		}

		/// <summary>Creates a continuation task that will be started upon the completion of a set of provided tasks.</summary>
		/// <param name="tasks">The array of tasks from which to continue.</param>
		/// <param name="continuationFunction">The function delegate to execute asynchronously when all tasks in the <paramref name="tasks" /> array have completed.</param>
		/// <typeparam name="TAntecedentResult">The type of the result of the antecedent <paramref name="tasks" />.</typeparam>
		/// <returns>The new continuation task.</returns>
		/// <exception cref="T:System.ObjectDisposedException">One of the elements in the <paramref name="tasks" /> array has been disposed.</exception>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="tasks" /> array is <see langword="null" />.  
		///  -or-  
		///  The <paramref name="continuationFunction" /> argument is <see langword="null" />.</exception>
		/// <exception cref="T:System.ArgumentException">The <paramref name="tasks" /> array contains a null value or is empty.</exception>
		// Token: 0x06002329 RID: 9001 RVA: 0x0007E021 File Offset: 0x0007C221
		public Task<TResult> ContinueWhenAll<TAntecedentResult>(Task<TAntecedentResult>[] tasks, Func<Task<TAntecedentResult>[], TResult> continuationFunction)
		{
			if (continuationFunction == null)
			{
				throw new ArgumentNullException("continuationFunction");
			}
			return TaskFactory<TResult>.ContinueWhenAllImpl<TAntecedentResult>(tasks, continuationFunction, this.m_defaultContinuationOptions, this.m_defaultCancellationToken, this.DefaultScheduler);
		}

		/// <summary>Creates a continuation task that will be started upon the completion of a set of provided tasks.</summary>
		/// <param name="tasks">The array of tasks from which to continue.</param>
		/// <param name="continuationFunction">The function delegate to execute asynchronously when all tasks in the <paramref name="tasks" /> array have completed.</param>
		/// <param name="cancellationToken">The cancellation token that will be assigned to the new continuation task.</param>
		/// <typeparam name="TAntecedentResult">The type of the result of the antecedent <paramref name="tasks" />.</typeparam>
		/// <returns>The new continuation task.</returns>
		/// <exception cref="T:System.ObjectDisposedException">One of the elements in the <paramref name="tasks" /> array has been disposed.  
		///  -or-  
		///  The <see cref="T:System.Threading.CancellationTokenSource" /> that created <paramref name="cancellationToken" /> has already been disposed.</exception>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="tasks" /> array is <see langword="null" />.  
		///  -or-  
		///  The <paramref name="continuationFunction" /> argument is <see langword="null" />.</exception>
		/// <exception cref="T:System.ArgumentException">The <paramref name="tasks" /> array contains a null value or is empty.</exception>
		// Token: 0x0600232A RID: 9002 RVA: 0x0007E04A File Offset: 0x0007C24A
		public Task<TResult> ContinueWhenAll<TAntecedentResult>(Task<TAntecedentResult>[] tasks, Func<Task<TAntecedentResult>[], TResult> continuationFunction, CancellationToken cancellationToken)
		{
			if (continuationFunction == null)
			{
				throw new ArgumentNullException("continuationFunction");
			}
			return TaskFactory<TResult>.ContinueWhenAllImpl<TAntecedentResult>(tasks, continuationFunction, this.m_defaultContinuationOptions, cancellationToken, this.DefaultScheduler);
		}

		/// <summary>Creates a continuation task that will be started upon the completion of a set of provided tasks.</summary>
		/// <param name="tasks">The array of tasks from which to continue.</param>
		/// <param name="continuationFunction">The function delegate to execute asynchronously when all tasks in the <paramref name="tasks" /> array have completed.</param>
		/// <param name="continuationOptions">One of the enumeration values that controls the behavior of the created continuation task. The NotOn* or OnlyOn* values are not valid.</param>
		/// <typeparam name="TAntecedentResult">The type of the result of the antecedent <paramref name="tasks" />.</typeparam>
		/// <returns>The new continuation task.</returns>
		/// <exception cref="T:System.ObjectDisposedException">One of the elements in the <paramref name="tasks" /> array has been disposed.</exception>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="tasks" /> array is <see langword="null" />.  
		///  -or-  
		///  The <paramref name="continuationFunction" /> argument is <see langword="null" />.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The <paramref name="continuationOptions" /> argument specifies an invalid value.</exception>
		/// <exception cref="T:System.ArgumentException">The <paramref name="tasks" /> array contains a null value or is empty.</exception>
		// Token: 0x0600232B RID: 9003 RVA: 0x0007E06E File Offset: 0x0007C26E
		public Task<TResult> ContinueWhenAll<TAntecedentResult>(Task<TAntecedentResult>[] tasks, Func<Task<TAntecedentResult>[], TResult> continuationFunction, TaskContinuationOptions continuationOptions)
		{
			if (continuationFunction == null)
			{
				throw new ArgumentNullException("continuationFunction");
			}
			return TaskFactory<TResult>.ContinueWhenAllImpl<TAntecedentResult>(tasks, continuationFunction, continuationOptions, this.m_defaultCancellationToken, this.DefaultScheduler);
		}

		/// <summary>Creates a continuation task that will be started upon the completion of a set of provided tasks.</summary>
		/// <param name="tasks">The array of tasks from which to continue.</param>
		/// <param name="continuationFunction">The function delegate to execute asynchronously when all tasks in the <paramref name="tasks" /> array have completed.</param>
		/// <param name="cancellationToken">The cancellation token that will be assigned to the new continuation task.</param>
		/// <param name="continuationOptions">One of the enumeration values that controls the behavior of the created continuation task. The NotOn* or OnlyOn* values are not valid.</param>
		/// <param name="scheduler">The scheduler that is used to schedule the created continuation task.</param>
		/// <typeparam name="TAntecedentResult">The type of the result of the antecedent <paramref name="tasks" />.</typeparam>
		/// <returns>The new continuation task.</returns>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="tasks" /> array is <see langword="null" />.  
		///  -or-  
		///  The <paramref name="continuationFunction" /> argument is <see langword="null" />.  
		///  -or-  
		///  The <paramref name="scheduler" /> argument is <see langword="null" />.</exception>
		/// <exception cref="T:System.ArgumentException">The <paramref name="tasks" /> array contains a null value or is empty.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The <paramref name="continuationOptions" /> argument specifies an invalid value.</exception>
		/// <exception cref="T:System.ObjectDisposedException">One of the elements in the <paramref name="tasks" /> array has been disposed.  
		///  -or-  
		///  The <see cref="T:System.Threading.CancellationTokenSource" /> that created <paramref name="cancellationToken" /> has already been disposed.</exception>
		// Token: 0x0600232C RID: 9004 RVA: 0x0007E092 File Offset: 0x0007C292
		public Task<TResult> ContinueWhenAll<TAntecedentResult>(Task<TAntecedentResult>[] tasks, Func<Task<TAntecedentResult>[], TResult> continuationFunction, CancellationToken cancellationToken, TaskContinuationOptions continuationOptions, TaskScheduler scheduler)
		{
			if (continuationFunction == null)
			{
				throw new ArgumentNullException("continuationFunction");
			}
			return TaskFactory<TResult>.ContinueWhenAllImpl<TAntecedentResult>(tasks, continuationFunction, continuationOptions, cancellationToken, scheduler);
		}

		// Token: 0x0600232D RID: 9005 RVA: 0x0007E0B0 File Offset: 0x0007C2B0
		internal static Task<TResult> ContinueWhenAllImpl<TAntecedentResult>(Task<TAntecedentResult>[] tasks, Func<Task<TAntecedentResult>[], TResult> continuationFunction, TaskContinuationOptions continuationOptions, CancellationToken cancellationToken, TaskScheduler scheduler)
		{
			TaskFactory.CheckMultiTaskContinuationOptions(continuationOptions);
			if (tasks == null)
			{
				throw new ArgumentNullException("tasks");
			}
			if (scheduler == null)
			{
				throw new ArgumentNullException("scheduler");
			}
			Task<TAntecedentResult>[] array = TaskFactory.CheckMultiContinuationTasksAndCopy<TAntecedentResult>(tasks);
			if (cancellationToken.IsCancellationRequested && (continuationOptions & TaskContinuationOptions.LazyCancellation) == TaskContinuationOptions.None)
			{
				return TaskFactory<TResult>.CreateCanceledTask(continuationOptions, cancellationToken);
			}
			return TaskFactory.CommonCWAllLogic<TAntecedentResult>(array).ContinueWith<TResult>(GenericDelegateCache<TAntecedentResult, TResult>.CWAllFuncDelegate, continuationFunction, scheduler, cancellationToken, continuationOptions);
		}

		// Token: 0x0600232E RID: 9006 RVA: 0x0007E114 File Offset: 0x0007C314
		internal static Task<TResult> ContinueWhenAllImpl<TAntecedentResult>(Task<TAntecedentResult>[] tasks, Action<Task<TAntecedentResult>[]> continuationAction, TaskContinuationOptions continuationOptions, CancellationToken cancellationToken, TaskScheduler scheduler)
		{
			TaskFactory.CheckMultiTaskContinuationOptions(continuationOptions);
			if (tasks == null)
			{
				throw new ArgumentNullException("tasks");
			}
			if (scheduler == null)
			{
				throw new ArgumentNullException("scheduler");
			}
			Task<TAntecedentResult>[] array = TaskFactory.CheckMultiContinuationTasksAndCopy<TAntecedentResult>(tasks);
			if (cancellationToken.IsCancellationRequested && (continuationOptions & TaskContinuationOptions.LazyCancellation) == TaskContinuationOptions.None)
			{
				return TaskFactory<TResult>.CreateCanceledTask(continuationOptions, cancellationToken);
			}
			return TaskFactory.CommonCWAllLogic<TAntecedentResult>(array).ContinueWith<TResult>(GenericDelegateCache<TAntecedentResult, TResult>.CWAllActionDelegate, continuationAction, scheduler, cancellationToken, continuationOptions);
		}

		// Token: 0x0600232F RID: 9007 RVA: 0x0007E178 File Offset: 0x0007C378
		internal static Task<TResult> ContinueWhenAllImpl(Task[] tasks, Func<Task[], TResult> continuationFunction, TaskContinuationOptions continuationOptions, CancellationToken cancellationToken, TaskScheduler scheduler)
		{
			TaskFactory.CheckMultiTaskContinuationOptions(continuationOptions);
			if (tasks == null)
			{
				throw new ArgumentNullException("tasks");
			}
			if (scheduler == null)
			{
				throw new ArgumentNullException("scheduler");
			}
			Task[] array = TaskFactory.CheckMultiContinuationTasksAndCopy(tasks);
			if (cancellationToken.IsCancellationRequested && (continuationOptions & TaskContinuationOptions.LazyCancellation) == TaskContinuationOptions.None)
			{
				return TaskFactory<TResult>.CreateCanceledTask(continuationOptions, cancellationToken);
			}
			return TaskFactory.CommonCWAllLogic(array).ContinueWith<TResult>(delegate(Task<Task[]> completedTasks, object state)
			{
				completedTasks.NotifyDebuggerOfWaitCompletionIfNecessary();
				return ((Func<Task[], TResult>)state)(completedTasks.Result);
			}, continuationFunction, scheduler, cancellationToken, continuationOptions);
		}

		// Token: 0x06002330 RID: 9008 RVA: 0x0007E1F8 File Offset: 0x0007C3F8
		internal static Task<TResult> ContinueWhenAllImpl(Task[] tasks, Action<Task[]> continuationAction, TaskContinuationOptions continuationOptions, CancellationToken cancellationToken, TaskScheduler scheduler)
		{
			TaskFactory.CheckMultiTaskContinuationOptions(continuationOptions);
			if (tasks == null)
			{
				throw new ArgumentNullException("tasks");
			}
			if (scheduler == null)
			{
				throw new ArgumentNullException("scheduler");
			}
			Task[] array = TaskFactory.CheckMultiContinuationTasksAndCopy(tasks);
			if (cancellationToken.IsCancellationRequested && (continuationOptions & TaskContinuationOptions.LazyCancellation) == TaskContinuationOptions.None)
			{
				return TaskFactory<TResult>.CreateCanceledTask(continuationOptions, cancellationToken);
			}
			return TaskFactory.CommonCWAllLogic(array).ContinueWith<TResult>(delegate(Task<Task[]> completedTasks, object state)
			{
				completedTasks.NotifyDebuggerOfWaitCompletionIfNecessary();
				((Action<Task[]>)state)(completedTasks.Result);
				return default(TResult);
			}, continuationAction, scheduler, cancellationToken, continuationOptions);
		}

		/// <summary>Creates a continuation task that will be started upon the completion of any task in the provided set.</summary>
		/// <param name="tasks">The array of tasks from which to continue when one task completes.</param>
		/// <param name="continuationFunction">The function delegate to execute asynchronously when one task in the <paramref name="tasks" /> array completes.</param>
		/// <returns>The new continuation task.</returns>
		/// <exception cref="T:System.ObjectDisposedException">One of the elements in the <paramref name="tasks" /> array has been disposed.</exception>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="tasks" /> array is <see langword="null" />.  
		///  -or-  
		///  The <paramref name="continuationFunction" /> argument is <see langword="null" />.</exception>
		/// <exception cref="T:System.ArgumentException">The <paramref name="tasks" /> array contains a null value or is empty.</exception>
		// Token: 0x06002331 RID: 9009 RVA: 0x0007E275 File Offset: 0x0007C475
		public Task<TResult> ContinueWhenAny(Task[] tasks, Func<Task, TResult> continuationFunction)
		{
			if (continuationFunction == null)
			{
				throw new ArgumentNullException("continuationFunction");
			}
			return TaskFactory<TResult>.ContinueWhenAnyImpl(tasks, continuationFunction, this.m_defaultContinuationOptions, this.m_defaultCancellationToken, this.DefaultScheduler);
		}

		/// <summary>Creates a continuation task that will be started upon the completion of any task in the provided set.</summary>
		/// <param name="tasks">The array of tasks from which to continue when one task completes.</param>
		/// <param name="continuationFunction">The function delegate to execute asynchronously when one task in the <paramref name="tasks" /> array completes.</param>
		/// <param name="cancellationToken">The cancellation token that will be assigned to the new continuation task.</param>
		/// <returns>The new continuation task.</returns>
		/// <exception cref="T:System.ObjectDisposedException">One of the elements in the <paramref name="tasks" /> array has been disposed.  
		///  -or-  
		///  The <see cref="T:System.Threading.CancellationTokenSource" /> that created <paramref name="cancellationToken" /> has already been disposed.</exception>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="tasks" /> array is null.  
		///  -or-  
		///  The <paramref name="continuationFunction" /> argument is null.</exception>
		/// <exception cref="T:System.ArgumentException">The <paramref name="tasks" /> array contains a null value.  
		///  -or-  
		///  The <paramref name="tasks" /> array is empty.</exception>
		// Token: 0x06002332 RID: 9010 RVA: 0x0007E29E File Offset: 0x0007C49E
		public Task<TResult> ContinueWhenAny(Task[] tasks, Func<Task, TResult> continuationFunction, CancellationToken cancellationToken)
		{
			if (continuationFunction == null)
			{
				throw new ArgumentNullException("continuationFunction");
			}
			return TaskFactory<TResult>.ContinueWhenAnyImpl(tasks, continuationFunction, this.m_defaultContinuationOptions, cancellationToken, this.DefaultScheduler);
		}

		/// <summary>Creates a continuation task that will be started upon the completion of any task in the provided set.</summary>
		/// <param name="tasks">The array of tasks from which to continue when one task completes.</param>
		/// <param name="continuationFunction">The function delegate to execute asynchronously when one task in the <paramref name="tasks" /> array completes.</param>
		/// <param name="continuationOptions">One of the enumeration values that controls the behavior of the created continuation task. The <see langword="NotOn" /> or <see langword="OnlyOn" /> values are not valid.</param>
		/// <returns>The new continuation task.</returns>
		/// <exception cref="T:System.ObjectDisposedException">One of the elements in the <paramref name="tasks" /> array has been disposed.</exception>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="tasks" /> array is <see langword="null" />.  
		///  -or-  
		///  The <paramref name="continuationFunction" /> argument is <see langword="null" />.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The <paramref name="continuationOptions" /> argument specifies an invalid enumeration value.</exception>
		/// <exception cref="T:System.ArgumentException">The <paramref name="tasks" /> array contains a null value.  
		///  -or-  
		///  The <paramref name="tasks" /> array is empty.</exception>
		// Token: 0x06002333 RID: 9011 RVA: 0x0007E2C2 File Offset: 0x0007C4C2
		public Task<TResult> ContinueWhenAny(Task[] tasks, Func<Task, TResult> continuationFunction, TaskContinuationOptions continuationOptions)
		{
			if (continuationFunction == null)
			{
				throw new ArgumentNullException("continuationFunction");
			}
			return TaskFactory<TResult>.ContinueWhenAnyImpl(tasks, continuationFunction, continuationOptions, this.m_defaultCancellationToken, this.DefaultScheduler);
		}

		/// <summary>Creates a continuation task that will be started upon the completion of any task in the provided set.</summary>
		/// <param name="tasks">The array of tasks from which to continue when one task completes.</param>
		/// <param name="continuationFunction">The function delegate to execute asynchronously when one task in the <paramref name="tasks" /> array completes.</param>
		/// <param name="cancellationToken">The cancellation token that will be assigned to the new continuation task.</param>
		/// <param name="continuationOptions">One of the enumeration values that controls the behavior of the created continuation task. The <see langword="NotOn" /> or <see langword="OnlyOn" /> values are not valid.</param>
		/// <param name="scheduler">The task scheduler that is used to schedule the created continuation task.</param>
		/// <returns>The new continuation task.</returns>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="tasks" /> array is <see langword="null" />.  
		///  -or-  
		///  The <paramref name="continuationFunction" /> argument is <see langword="null" />.  
		///  -or-  
		///  The <paramref name="scheduler" /> argument is <see langword="null" />.</exception>
		/// <exception cref="T:System.ArgumentException">The <paramref name="tasks" /> array contains a null value.  
		///  -or-  
		///  The <paramref name="tasks" /> array is empty.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The <paramref name="continuationOptions" /> argument specifies an invalid <see cref="T:System.Threading.Tasks.TaskContinuationOptions" /> value.</exception>
		/// <exception cref="T:System.ObjectDisposedException">One of the elements in the <paramref name="tasks" /> array has been disposed.  
		///  -or-  
		///  The <see cref="T:System.Threading.CancellationTokenSource" /> that created <paramref name="cancellationToken" /> has already been disposed.</exception>
		// Token: 0x06002334 RID: 9012 RVA: 0x0007E2E6 File Offset: 0x0007C4E6
		public Task<TResult> ContinueWhenAny(Task[] tasks, Func<Task, TResult> continuationFunction, CancellationToken cancellationToken, TaskContinuationOptions continuationOptions, TaskScheduler scheduler)
		{
			if (continuationFunction == null)
			{
				throw new ArgumentNullException("continuationFunction");
			}
			return TaskFactory<TResult>.ContinueWhenAnyImpl(tasks, continuationFunction, continuationOptions, cancellationToken, scheduler);
		}

		/// <summary>Creates a continuation task that will be started upon the completion of any task in the provided set.</summary>
		/// <param name="tasks">The array of tasks from which to continue when one task completes.</param>
		/// <param name="continuationFunction">The function delegate to execute asynchronously when one task in the <paramref name="tasks" /> array completes.</param>
		/// <typeparam name="TAntecedentResult">The type of the result of the antecedent <paramref name="tasks" />.</typeparam>
		/// <returns>The new continuation <see cref="T:System.Threading.Tasks.Task`1" />.</returns>
		/// <exception cref="T:System.ObjectDisposedException">One of the elements in the <paramref name="tasks" /> array has been disposed.</exception>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="tasks" /> array is <see langword="null" />.  
		///  -or-  
		///  The <paramref name="continuationFunction" /> argument is <see langword="null" />.</exception>
		/// <exception cref="T:System.ArgumentException">The <paramref name="tasks" /> array contains a null value.  
		///  -or-  
		///  The <paramref name="tasks" /> array is empty.</exception>
		// Token: 0x06002335 RID: 9013 RVA: 0x0007E302 File Offset: 0x0007C502
		public Task<TResult> ContinueWhenAny<TAntecedentResult>(Task<TAntecedentResult>[] tasks, Func<Task<TAntecedentResult>, TResult> continuationFunction)
		{
			if (continuationFunction == null)
			{
				throw new ArgumentNullException("continuationFunction");
			}
			return TaskFactory<TResult>.ContinueWhenAnyImpl<TAntecedentResult>(tasks, continuationFunction, this.m_defaultContinuationOptions, this.m_defaultCancellationToken, this.DefaultScheduler);
		}

		/// <summary>Creates a continuation task that will be started upon the completion of any task in the provided set.</summary>
		/// <param name="tasks">The array of tasks from which to continue when one task completes.</param>
		/// <param name="continuationFunction">The function delegate to execute asynchronously when one task in the <paramref name="tasks" /> array completes.</param>
		/// <param name="cancellationToken">The cancellation token that will be assigned to the new continuation task.</param>
		/// <typeparam name="TAntecedentResult">The type of the result of the antecedent <paramref name="tasks" />.</typeparam>
		/// <returns>The new continuation task.</returns>
		/// <exception cref="T:System.ObjectDisposedException">One of the elements in the <paramref name="tasks" /> array has been disposed.  
		///  -or-  
		///  The <see cref="T:System.Threading.CancellationTokenSource" /> that created <paramref name="cancellationToken" /> has already been disposed.</exception>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="tasks" /> array is <see langword="null" />.  
		///  -or-  
		///  The <paramref name="continuationFunction" /> argument is <see langword="null" />.</exception>
		/// <exception cref="T:System.ArgumentException">The <paramref name="tasks" /> array contains a null value.  
		///  -or-  
		///  The <paramref name="tasks" /> array is empty.</exception>
		// Token: 0x06002336 RID: 9014 RVA: 0x0007E32B File Offset: 0x0007C52B
		public Task<TResult> ContinueWhenAny<TAntecedentResult>(Task<TAntecedentResult>[] tasks, Func<Task<TAntecedentResult>, TResult> continuationFunction, CancellationToken cancellationToken)
		{
			if (continuationFunction == null)
			{
				throw new ArgumentNullException("continuationFunction");
			}
			return TaskFactory<TResult>.ContinueWhenAnyImpl<TAntecedentResult>(tasks, continuationFunction, this.m_defaultContinuationOptions, cancellationToken, this.DefaultScheduler);
		}

		/// <summary>Creates a continuation task that will be started upon the completion of any task in the provided set.</summary>
		/// <param name="tasks">The array of tasks from which to continue when one task completes.</param>
		/// <param name="continuationFunction">The function delegate to execute asynchronously when one task in the <paramref name="tasks" /> array completes.</param>
		/// <param name="continuationOptions">One of the enumeration values that controls the behavior of the created continuation task. The <see langword="NotOn" /> or <see langword="OnlyOn" /> values are not valid.</param>
		/// <typeparam name="TAntecedentResult">The type of the result of the antecedent <paramref name="tasks" />.</typeparam>
		/// <returns>The new continuation <see cref="T:System.Threading.Tasks.Task`1" />.</returns>
		/// <exception cref="T:System.ObjectDisposedException">One of the elements in the <paramref name="tasks" /> array has been disposed.</exception>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="tasks" /> array is <see langword="null" />.  
		///  -or-  
		///  The <paramref name="continuationFunction" /> argument is <see langword="null" />.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The <paramref name="continuationOptions" /> argument specifies an invalid enumeration value.</exception>
		/// <exception cref="T:System.ArgumentException">The <paramref name="tasks" /> array contains a null value.  
		///  -or-  
		///  The <paramref name="tasks" /> array is empty.</exception>
		// Token: 0x06002337 RID: 9015 RVA: 0x0007E34F File Offset: 0x0007C54F
		public Task<TResult> ContinueWhenAny<TAntecedentResult>(Task<TAntecedentResult>[] tasks, Func<Task<TAntecedentResult>, TResult> continuationFunction, TaskContinuationOptions continuationOptions)
		{
			if (continuationFunction == null)
			{
				throw new ArgumentNullException("continuationFunction");
			}
			return TaskFactory<TResult>.ContinueWhenAnyImpl<TAntecedentResult>(tasks, continuationFunction, continuationOptions, this.m_defaultCancellationToken, this.DefaultScheduler);
		}

		/// <summary>Creates a continuation task that will be started upon the completion of any task in the provided set.</summary>
		/// <param name="tasks">The array of tasks from which to continue when one task completes.</param>
		/// <param name="continuationFunction">The function delegate to execute asynchronously when one task in the <paramref name="tasks" /> array completes.</param>
		/// <param name="cancellationToken">The cancellation token that will be assigned to the new continuation task.</param>
		/// <param name="continuationOptions">One of the enumeration values that controls the behavior of the created continuation task. The <see langword="NotOn" /> or <see langword="OnlyOn" /> values are not valid.</param>
		/// <param name="scheduler">The <see cref="T:System.Threading.Tasks.TaskScheduler" /> that is used to schedule the created continuation <see cref="T:System.Threading.Tasks.Task`1" />.</param>
		/// <typeparam name="TAntecedentResult">The type of the result of the antecedent <paramref name="tasks" />.</typeparam>
		/// <returns>The new continuation <see cref="T:System.Threading.Tasks.Task`1" />.</returns>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="tasks" /> array is <see langword="null" />.  
		///  -or-  
		///  The <paramref name="continuationFunction" /> argument is <see langword="null" />.  
		///  -or-  
		///  The <paramref name="scheduler" /> argument is null.</exception>
		/// <exception cref="T:System.ArgumentException">The <paramref name="tasks" /> array contains a null value.  
		///  -or-  
		///  The <paramref name="tasks" /> array is empty.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The <paramref name="continuationOptions" /> argument specifies an invalid TaskContinuationOptions value.</exception>
		/// <exception cref="T:System.ObjectDisposedException">One of the elements in the <paramref name="tasks" /> array has been disposed.  
		///  -or-  
		///  The <see cref="T:System.Threading.CancellationTokenSource" /> that created <paramref name="cancellationToken" /> has already been disposed.</exception>
		// Token: 0x06002338 RID: 9016 RVA: 0x0007E373 File Offset: 0x0007C573
		public Task<TResult> ContinueWhenAny<TAntecedentResult>(Task<TAntecedentResult>[] tasks, Func<Task<TAntecedentResult>, TResult> continuationFunction, CancellationToken cancellationToken, TaskContinuationOptions continuationOptions, TaskScheduler scheduler)
		{
			if (continuationFunction == null)
			{
				throw new ArgumentNullException("continuationFunction");
			}
			return TaskFactory<TResult>.ContinueWhenAnyImpl<TAntecedentResult>(tasks, continuationFunction, continuationOptions, cancellationToken, scheduler);
		}

		// Token: 0x06002339 RID: 9017 RVA: 0x0007E390 File Offset: 0x0007C590
		internal static Task<TResult> ContinueWhenAnyImpl(Task[] tasks, Action<Task> continuationAction, TaskContinuationOptions continuationOptions, CancellationToken cancellationToken, TaskScheduler scheduler)
		{
			TaskFactory.CheckMultiTaskContinuationOptions(continuationOptions);
			if (tasks == null)
			{
				throw new ArgumentNullException("tasks");
			}
			if (tasks.Length == 0)
			{
				throw new ArgumentException("The tasks argument contains no tasks.", "tasks");
			}
			if (scheduler == null)
			{
				throw new ArgumentNullException("scheduler");
			}
			Task<Task> task = TaskFactory.CommonCWAnyLogic(tasks);
			if (cancellationToken.IsCancellationRequested && (continuationOptions & TaskContinuationOptions.LazyCancellation) == TaskContinuationOptions.None)
			{
				return TaskFactory<TResult>.CreateCanceledTask(continuationOptions, cancellationToken);
			}
			return task.ContinueWith<TResult>(delegate(Task<Task> completedTask, object state)
			{
				((Action<Task>)state)(completedTask.Result);
				return default(TResult);
			}, continuationAction, scheduler, cancellationToken, continuationOptions);
		}

		// Token: 0x0600233A RID: 9018 RVA: 0x0007E41C File Offset: 0x0007C61C
		internal static Task<TResult> ContinueWhenAnyImpl(Task[] tasks, Func<Task, TResult> continuationFunction, TaskContinuationOptions continuationOptions, CancellationToken cancellationToken, TaskScheduler scheduler)
		{
			TaskFactory.CheckMultiTaskContinuationOptions(continuationOptions);
			if (tasks == null)
			{
				throw new ArgumentNullException("tasks");
			}
			if (tasks.Length == 0)
			{
				throw new ArgumentException("The tasks argument contains no tasks.", "tasks");
			}
			if (scheduler == null)
			{
				throw new ArgumentNullException("scheduler");
			}
			Task<Task> task = TaskFactory.CommonCWAnyLogic(tasks);
			if (cancellationToken.IsCancellationRequested && (continuationOptions & TaskContinuationOptions.LazyCancellation) == TaskContinuationOptions.None)
			{
				return TaskFactory<TResult>.CreateCanceledTask(continuationOptions, cancellationToken);
			}
			return task.ContinueWith<TResult>((Task<Task> completedTask, object state) => ((Func<Task, TResult>)state)(completedTask.Result), continuationFunction, scheduler, cancellationToken, continuationOptions);
		}

		// Token: 0x0600233B RID: 9019 RVA: 0x0007E4A8 File Offset: 0x0007C6A8
		internal static Task<TResult> ContinueWhenAnyImpl<TAntecedentResult>(Task<TAntecedentResult>[] tasks, Func<Task<TAntecedentResult>, TResult> continuationFunction, TaskContinuationOptions continuationOptions, CancellationToken cancellationToken, TaskScheduler scheduler)
		{
			TaskFactory.CheckMultiTaskContinuationOptions(continuationOptions);
			if (tasks == null)
			{
				throw new ArgumentNullException("tasks");
			}
			if (tasks.Length == 0)
			{
				throw new ArgumentException("The tasks argument contains no tasks.", "tasks");
			}
			if (scheduler == null)
			{
				throw new ArgumentNullException("scheduler");
			}
			Task<Task> task = TaskFactory.CommonCWAnyLogic(tasks);
			if (cancellationToken.IsCancellationRequested && (continuationOptions & TaskContinuationOptions.LazyCancellation) == TaskContinuationOptions.None)
			{
				return TaskFactory<TResult>.CreateCanceledTask(continuationOptions, cancellationToken);
			}
			return task.ContinueWith<TResult>(GenericDelegateCache<TAntecedentResult, TResult>.CWAnyFuncDelegate, continuationFunction, scheduler, cancellationToken, continuationOptions);
		}

		// Token: 0x0600233C RID: 9020 RVA: 0x0007E51C File Offset: 0x0007C71C
		internal static Task<TResult> ContinueWhenAnyImpl<TAntecedentResult>(Task<TAntecedentResult>[] tasks, Action<Task<TAntecedentResult>> continuationAction, TaskContinuationOptions continuationOptions, CancellationToken cancellationToken, TaskScheduler scheduler)
		{
			TaskFactory.CheckMultiTaskContinuationOptions(continuationOptions);
			if (tasks == null)
			{
				throw new ArgumentNullException("tasks");
			}
			if (tasks.Length == 0)
			{
				throw new ArgumentException("The tasks argument contains no tasks.", "tasks");
			}
			if (scheduler == null)
			{
				throw new ArgumentNullException("scheduler");
			}
			Task<Task> task = TaskFactory.CommonCWAnyLogic(tasks);
			if (cancellationToken.IsCancellationRequested && (continuationOptions & TaskContinuationOptions.LazyCancellation) == TaskContinuationOptions.None)
			{
				return TaskFactory<TResult>.CreateCanceledTask(continuationOptions, cancellationToken);
			}
			return task.ContinueWith<TResult>(GenericDelegateCache<TAntecedentResult, TResult>.CWAnyActionDelegate, continuationAction, scheduler, cancellationToken, continuationOptions);
		}

		// Token: 0x04001C88 RID: 7304
		private CancellationToken m_defaultCancellationToken;

		// Token: 0x04001C89 RID: 7305
		private TaskScheduler m_defaultScheduler;

		// Token: 0x04001C8A RID: 7306
		private TaskCreationOptions m_defaultCreationOptions;

		// Token: 0x04001C8B RID: 7307
		private TaskContinuationOptions m_defaultContinuationOptions;

		// Token: 0x02000344 RID: 836
		private sealed class FromAsyncTrimPromise<TInstance> : Task<TResult> where TInstance : class
		{
			// Token: 0x0600233D RID: 9021 RVA: 0x0007E58E File Offset: 0x0007C78E
			internal FromAsyncTrimPromise(TInstance thisRef, Func<TInstance, IAsyncResult, TResult> endMethod)
			{
				this.m_thisRef = thisRef;
				this.m_endMethod = endMethod;
			}

			// Token: 0x0600233E RID: 9022 RVA: 0x0007E5A4 File Offset: 0x0007C7A4
			internal static void CompleteFromAsyncResult(IAsyncResult asyncResult)
			{
				if (asyncResult == null)
				{
					throw new ArgumentNullException("asyncResult");
				}
				TaskFactory<TResult>.FromAsyncTrimPromise<TInstance> fromAsyncTrimPromise = asyncResult.AsyncState as TaskFactory<TResult>.FromAsyncTrimPromise<TInstance>;
				if (fromAsyncTrimPromise == null)
				{
					throw new ArgumentException("Either the IAsyncResult object did not come from the corresponding async method on this type, or the End method was called multiple times with the same IAsyncResult.", "asyncResult");
				}
				TInstance thisRef = fromAsyncTrimPromise.m_thisRef;
				Func<TInstance, IAsyncResult, TResult> endMethod = fromAsyncTrimPromise.m_endMethod;
				fromAsyncTrimPromise.m_thisRef = default(TInstance);
				fromAsyncTrimPromise.m_endMethod = null;
				if (endMethod == null)
				{
					throw new ArgumentException("Either the IAsyncResult object did not come from the corresponding async method on this type, or the End method was called multiple times with the same IAsyncResult.", "asyncResult");
				}
				if (!asyncResult.CompletedSynchronously)
				{
					fromAsyncTrimPromise.Complete(thisRef, endMethod, asyncResult, true);
				}
			}

			// Token: 0x0600233F RID: 9023 RVA: 0x0007E624 File Offset: 0x0007C824
			internal void Complete(TInstance thisRef, Func<TInstance, IAsyncResult, TResult> endMethod, IAsyncResult asyncResult, bool requiresSynchronization)
			{
				try
				{
					TResult tresult = endMethod(thisRef, asyncResult);
					if (requiresSynchronization)
					{
						base.TrySetResult(tresult);
					}
					else
					{
						base.DangerousSetResult(tresult);
					}
				}
				catch (OperationCanceledException ex)
				{
					base.TrySetCanceled(ex.CancellationToken, ex);
				}
				catch (Exception ex2)
				{
					base.TrySetException(ex2);
				}
			}

			// Token: 0x04001C8C RID: 7308
			internal static readonly AsyncCallback s_completeFromAsyncResult = new AsyncCallback(TaskFactory<TResult>.FromAsyncTrimPromise<TInstance>.CompleteFromAsyncResult);

			// Token: 0x04001C8D RID: 7309
			private TInstance m_thisRef;

			// Token: 0x04001C8E RID: 7310
			private Func<TInstance, IAsyncResult, TResult> m_endMethod;
		}
	}
}
