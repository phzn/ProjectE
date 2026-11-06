using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace System.Threading.Tasks
{
	/// <summary>Provides task schedulers that coordinate to execute tasks while ensuring that concurrent tasks may run concurrently and exclusive tasks never do.</summary>
	// Token: 0x02000337 RID: 823
	[DebuggerDisplay("Concurrent={ConcurrentTaskCountForDebugger}, Exclusive={ExclusiveTaskCountForDebugger}, Mode={ModeForDebugger}")]
	[DebuggerTypeProxy(typeof(ConcurrentExclusiveSchedulerPair.DebugView))]
	public class ConcurrentExclusiveSchedulerPair
	{
		// Token: 0x1700040F RID: 1039
		// (get) Token: 0x0600227D RID: 8829 RVA: 0x0007C353 File Offset: 0x0007A553
		private static int DefaultMaxConcurrencyLevel
		{
			get
			{
				return Environment.ProcessorCount;
			}
		}

		// Token: 0x17000410 RID: 1040
		// (get) Token: 0x0600227E RID: 8830 RVA: 0x0007C35A File Offset: 0x0007A55A
		private object ValueLock
		{
			get
			{
				return this.m_threadProcessingMode;
			}
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Threading.Tasks.ConcurrentExclusiveSchedulerPair" /> class.</summary>
		// Token: 0x0600227F RID: 8831 RVA: 0x0007C362 File Offset: 0x0007A562
		public ConcurrentExclusiveSchedulerPair()
			: this(TaskScheduler.Default, ConcurrentExclusiveSchedulerPair.DefaultMaxConcurrencyLevel, -1)
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Threading.Tasks.ConcurrentExclusiveSchedulerPair" /> class that targets the specified scheduler.</summary>
		/// <param name="taskScheduler">The target scheduler on which this pair should execute.</param>
		// Token: 0x06002280 RID: 8832 RVA: 0x0007C375 File Offset: 0x0007A575
		public ConcurrentExclusiveSchedulerPair(TaskScheduler taskScheduler)
			: this(taskScheduler, ConcurrentExclusiveSchedulerPair.DefaultMaxConcurrencyLevel, -1)
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Threading.Tasks.ConcurrentExclusiveSchedulerPair" /> class that targets the specified scheduler with a maximum concurrency level.</summary>
		/// <param name="taskScheduler">The target scheduler on which this pair should execute.</param>
		/// <param name="maxConcurrencyLevel">The maximum number of tasks to run concurrently.</param>
		// Token: 0x06002281 RID: 8833 RVA: 0x0007C384 File Offset: 0x0007A584
		public ConcurrentExclusiveSchedulerPair(TaskScheduler taskScheduler, int maxConcurrencyLevel)
			: this(taskScheduler, maxConcurrencyLevel, -1)
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Threading.Tasks.ConcurrentExclusiveSchedulerPair" /> class that targets the specified scheduler with a maximum concurrency level and a maximum number of scheduled tasks that may be processed as a unit.</summary>
		/// <param name="taskScheduler">The target scheduler on which this pair should execute.</param>
		/// <param name="maxConcurrencyLevel">The maximum number of tasks to run concurrently.</param>
		/// <param name="maxItemsPerTask">The maximum number of tasks to process for each underlying scheduled task used by the pair.</param>
		// Token: 0x06002282 RID: 8834 RVA: 0x0007C390 File Offset: 0x0007A590
		public ConcurrentExclusiveSchedulerPair(TaskScheduler taskScheduler, int maxConcurrencyLevel, int maxItemsPerTask)
		{
			if (taskScheduler == null)
			{
				throw new ArgumentNullException("taskScheduler");
			}
			if (maxConcurrencyLevel == 0 || maxConcurrencyLevel < -1)
			{
				throw new ArgumentOutOfRangeException("maxConcurrencyLevel");
			}
			if (maxItemsPerTask == 0 || maxItemsPerTask < -1)
			{
				throw new ArgumentOutOfRangeException("maxItemsPerTask");
			}
			this.m_underlyingTaskScheduler = taskScheduler;
			this.m_maxConcurrencyLevel = maxConcurrencyLevel;
			this.m_maxItemsPerTask = maxItemsPerTask;
			int maximumConcurrencyLevel = taskScheduler.MaximumConcurrencyLevel;
			if (maximumConcurrencyLevel > 0 && maximumConcurrencyLevel < this.m_maxConcurrencyLevel)
			{
				this.m_maxConcurrencyLevel = maximumConcurrencyLevel;
			}
			if (this.m_maxConcurrencyLevel == -1)
			{
				this.m_maxConcurrencyLevel = int.MaxValue;
			}
			if (this.m_maxItemsPerTask == -1)
			{
				this.m_maxItemsPerTask = int.MaxValue;
			}
			this.m_exclusiveTaskScheduler = new ConcurrentExclusiveSchedulerPair.ConcurrentExclusiveTaskScheduler(this, 1, ConcurrentExclusiveSchedulerPair.ProcessingMode.ProcessingExclusiveTask);
			this.m_concurrentTaskScheduler = new ConcurrentExclusiveSchedulerPair.ConcurrentExclusiveTaskScheduler(this, this.m_maxConcurrencyLevel, ConcurrentExclusiveSchedulerPair.ProcessingMode.ProcessingConcurrentTasks);
		}

		/// <summary>Informs the scheduler pair that it should not accept any more tasks.</summary>
		// Token: 0x06002283 RID: 8835 RVA: 0x0007C45C File Offset: 0x0007A65C
		public void Complete()
		{
			object valueLock = this.ValueLock;
			lock (valueLock)
			{
				if (!this.CompletionRequested)
				{
					this.RequestCompletion();
					this.CleanupStateIfCompletingAndQuiesced();
				}
			}
		}

		/// <summary>Gets a <see cref="T:System.Threading.Tasks.Task" /> that will complete when the scheduler has completed processing.</summary>
		/// <returns>The asynchronous operation that will complete when the scheduler finishes processing.</returns>
		// Token: 0x17000411 RID: 1041
		// (get) Token: 0x06002284 RID: 8836 RVA: 0x0007C4AC File Offset: 0x0007A6AC
		public Task Completion
		{
			get
			{
				return this.EnsureCompletionStateInitialized().Task;
			}
		}

		// Token: 0x06002285 RID: 8837 RVA: 0x0007C4B9 File Offset: 0x0007A6B9
		private ConcurrentExclusiveSchedulerPair.CompletionState EnsureCompletionStateInitialized()
		{
			return LazyInitializer.EnsureInitialized<ConcurrentExclusiveSchedulerPair.CompletionState>(ref this.m_completionState, () => new ConcurrentExclusiveSchedulerPair.CompletionState());
		}

		// Token: 0x17000412 RID: 1042
		// (get) Token: 0x06002286 RID: 8838 RVA: 0x0007C4E5 File Offset: 0x0007A6E5
		private bool CompletionRequested
		{
			get
			{
				return this.m_completionState != null && Volatile.Read(ref this.m_completionState.m_completionRequested);
			}
		}

		// Token: 0x06002287 RID: 8839 RVA: 0x0007C501 File Offset: 0x0007A701
		private void RequestCompletion()
		{
			this.EnsureCompletionStateInitialized().m_completionRequested = true;
		}

		// Token: 0x06002288 RID: 8840 RVA: 0x0007C50F File Offset: 0x0007A70F
		private void CleanupStateIfCompletingAndQuiesced()
		{
			if (this.ReadyToComplete)
			{
				this.CompleteTaskAsync();
			}
		}

		// Token: 0x17000413 RID: 1043
		// (get) Token: 0x06002289 RID: 8841 RVA: 0x0007C520 File Offset: 0x0007A720
		private bool ReadyToComplete
		{
			get
			{
				if (!this.CompletionRequested || this.m_processingCount != 0)
				{
					return false;
				}
				ConcurrentExclusiveSchedulerPair.CompletionState completionState = this.EnsureCompletionStateInitialized();
				return (completionState.m_exceptions != null && completionState.m_exceptions.Count > 0) || (this.m_concurrentTaskScheduler.m_tasks.IsEmpty && this.m_exclusiveTaskScheduler.m_tasks.IsEmpty);
			}
		}

		// Token: 0x0600228A RID: 8842 RVA: 0x0007C584 File Offset: 0x0007A784
		private void CompleteTaskAsync()
		{
			ConcurrentExclusiveSchedulerPair.CompletionState completionState = this.EnsureCompletionStateInitialized();
			if (!completionState.m_completionQueued)
			{
				completionState.m_completionQueued = true;
				ThreadPool.QueueUserWorkItem(delegate(object state)
				{
					ConcurrentExclusiveSchedulerPair concurrentExclusiveSchedulerPair = (ConcurrentExclusiveSchedulerPair)state;
					List<Exception> exceptions = concurrentExclusiveSchedulerPair.m_completionState.m_exceptions;
					if (exceptions == null || exceptions.Count <= 0)
					{
						concurrentExclusiveSchedulerPair.m_completionState.TrySetResult(default(VoidTaskResult));
					}
					else
					{
						concurrentExclusiveSchedulerPair.m_completionState.TrySetException(exceptions);
					}
					concurrentExclusiveSchedulerPair.m_threadProcessingMode.Dispose();
				}, this);
			}
		}

		// Token: 0x0600228B RID: 8843 RVA: 0x0007C5D0 File Offset: 0x0007A7D0
		private void FaultWithTask(Task faultedTask)
		{
			ConcurrentExclusiveSchedulerPair.CompletionState completionState = this.EnsureCompletionStateInitialized();
			if (completionState.m_exceptions == null)
			{
				completionState.m_exceptions = new List<Exception>();
			}
			completionState.m_exceptions.AddRange(faultedTask.Exception.InnerExceptions);
			this.RequestCompletion();
		}

		/// <summary>Gets a <see cref="T:System.Threading.Tasks.TaskScheduler" /> that can be used to schedule tasks to this pair that may run concurrently with other tasks on this pair.</summary>
		/// <returns>An object that can be used to schedule tasks concurrently.</returns>
		// Token: 0x17000414 RID: 1044
		// (get) Token: 0x0600228C RID: 8844 RVA: 0x0007C613 File Offset: 0x0007A813
		public TaskScheduler ConcurrentScheduler
		{
			get
			{
				return this.m_concurrentTaskScheduler;
			}
		}

		/// <summary>Gets a <see cref="T:System.Threading.Tasks.TaskScheduler" /> that can be used to schedule tasks to this pair that must run exclusively with regards to other tasks on this pair.</summary>
		/// <returns>An object that can be used to schedule tasks that do not run concurrently with other tasks.</returns>
		// Token: 0x17000415 RID: 1045
		// (get) Token: 0x0600228D RID: 8845 RVA: 0x0007C61B File Offset: 0x0007A81B
		public TaskScheduler ExclusiveScheduler
		{
			get
			{
				return this.m_exclusiveTaskScheduler;
			}
		}

		// Token: 0x17000416 RID: 1046
		// (get) Token: 0x0600228E RID: 8846 RVA: 0x0007C623 File Offset: 0x0007A823
		private int ConcurrentTaskCountForDebugger
		{
			get
			{
				return this.m_concurrentTaskScheduler.m_tasks.Count;
			}
		}

		// Token: 0x17000417 RID: 1047
		// (get) Token: 0x0600228F RID: 8847 RVA: 0x0007C635 File Offset: 0x0007A835
		private int ExclusiveTaskCountForDebugger
		{
			get
			{
				return this.m_exclusiveTaskScheduler.m_tasks.Count;
			}
		}

		// Token: 0x06002290 RID: 8848 RVA: 0x0007C648 File Offset: 0x0007A848
		private void ProcessAsyncIfNecessary(bool fairly = false)
		{
			if (this.m_processingCount >= 0)
			{
				bool flag = !this.m_exclusiveTaskScheduler.m_tasks.IsEmpty;
				Task task = null;
				if (this.m_processingCount == 0 && flag)
				{
					this.m_processingCount = -1;
					try
					{
						task = new Task(delegate(object thisPair)
						{
							((ConcurrentExclusiveSchedulerPair)thisPair).ProcessExclusiveTasks();
						}, this, default(CancellationToken), ConcurrentExclusiveSchedulerPair.GetCreationOptionsForTask(fairly));
						task.Start(this.m_underlyingTaskScheduler);
						goto IL_0149;
					}
					catch
					{
						this.m_processingCount = 0;
						this.FaultWithTask(task);
						goto IL_0149;
					}
				}
				int count = this.m_concurrentTaskScheduler.m_tasks.Count;
				if (count > 0 && !flag && this.m_processingCount < this.m_maxConcurrencyLevel)
				{
					int num = 0;
					while (num < count && this.m_processingCount < this.m_maxConcurrencyLevel)
					{
						this.m_processingCount++;
						try
						{
							task = new Task(delegate(object thisPair)
							{
								((ConcurrentExclusiveSchedulerPair)thisPair).ProcessConcurrentTasks();
							}, this, default(CancellationToken), ConcurrentExclusiveSchedulerPair.GetCreationOptionsForTask(fairly));
							task.Start(this.m_underlyingTaskScheduler);
						}
						catch
						{
							this.m_processingCount--;
							this.FaultWithTask(task);
						}
						num++;
					}
				}
				IL_0149:
				this.CleanupStateIfCompletingAndQuiesced();
			}
		}

		// Token: 0x06002291 RID: 8849 RVA: 0x0007C7C0 File Offset: 0x0007A9C0
		private void ProcessExclusiveTasks()
		{
			try
			{
				this.m_threadProcessingMode.Value = ConcurrentExclusiveSchedulerPair.ProcessingMode.ProcessingExclusiveTask;
				for (int i = 0; i < this.m_maxItemsPerTask; i++)
				{
					Task task;
					if (!this.m_exclusiveTaskScheduler.m_tasks.TryDequeue(out task))
					{
						break;
					}
					if (!task.IsFaulted)
					{
						this.m_exclusiveTaskScheduler.ExecuteTask(task);
					}
				}
			}
			finally
			{
				this.m_threadProcessingMode.Value = ConcurrentExclusiveSchedulerPair.ProcessingMode.NotCurrentlyProcessing;
				object valueLock = this.ValueLock;
				lock (valueLock)
				{
					this.m_processingCount = 0;
					this.ProcessAsyncIfNecessary(true);
				}
			}
		}

		// Token: 0x06002292 RID: 8850 RVA: 0x0007C86C File Offset: 0x0007AA6C
		private void ProcessConcurrentTasks()
		{
			try
			{
				this.m_threadProcessingMode.Value = ConcurrentExclusiveSchedulerPair.ProcessingMode.ProcessingConcurrentTasks;
				for (int i = 0; i < this.m_maxItemsPerTask; i++)
				{
					Task task;
					if (!this.m_concurrentTaskScheduler.m_tasks.TryDequeue(out task))
					{
						break;
					}
					if (!task.IsFaulted)
					{
						this.m_concurrentTaskScheduler.ExecuteTask(task);
					}
					if (!this.m_exclusiveTaskScheduler.m_tasks.IsEmpty)
					{
						break;
					}
				}
			}
			finally
			{
				this.m_threadProcessingMode.Value = ConcurrentExclusiveSchedulerPair.ProcessingMode.NotCurrentlyProcessing;
				object valueLock = this.ValueLock;
				lock (valueLock)
				{
					if (this.m_processingCount > 0)
					{
						this.m_processingCount--;
					}
					this.ProcessAsyncIfNecessary(true);
				}
			}
		}

		// Token: 0x17000418 RID: 1048
		// (get) Token: 0x06002293 RID: 8851 RVA: 0x0007C93C File Offset: 0x0007AB3C
		private ConcurrentExclusiveSchedulerPair.ProcessingMode ModeForDebugger
		{
			get
			{
				if (this.m_completionState != null && this.m_completionState.Task.IsCompleted)
				{
					return ConcurrentExclusiveSchedulerPair.ProcessingMode.Completed;
				}
				ConcurrentExclusiveSchedulerPair.ProcessingMode processingMode = ConcurrentExclusiveSchedulerPair.ProcessingMode.NotCurrentlyProcessing;
				if (this.m_processingCount == -1)
				{
					processingMode |= ConcurrentExclusiveSchedulerPair.ProcessingMode.ProcessingExclusiveTask;
				}
				if (this.m_processingCount >= 1)
				{
					processingMode |= ConcurrentExclusiveSchedulerPair.ProcessingMode.ProcessingConcurrentTasks;
				}
				if (this.CompletionRequested)
				{
					processingMode |= ConcurrentExclusiveSchedulerPair.ProcessingMode.Completing;
				}
				return processingMode;
			}
		}

		// Token: 0x06002294 RID: 8852 RVA: 0x00004BF9 File Offset: 0x00002DF9
		[Conditional("DEBUG")]
		private static void ContractAssertMonitorStatus(object syncObj, bool held)
		{
		}

		// Token: 0x06002295 RID: 8853 RVA: 0x0007C990 File Offset: 0x0007AB90
		internal static TaskCreationOptions GetCreationOptionsForTask(bool isReplacementReplica = false)
		{
			TaskCreationOptions taskCreationOptions = TaskCreationOptions.DenyChildAttach;
			if (isReplacementReplica)
			{
				taskCreationOptions |= TaskCreationOptions.PreferFairness;
			}
			return taskCreationOptions;
		}

		// Token: 0x04001C61 RID: 7265
		private readonly ThreadLocal<ConcurrentExclusiveSchedulerPair.ProcessingMode> m_threadProcessingMode = new ThreadLocal<ConcurrentExclusiveSchedulerPair.ProcessingMode>();

		// Token: 0x04001C62 RID: 7266
		private readonly ConcurrentExclusiveSchedulerPair.ConcurrentExclusiveTaskScheduler m_concurrentTaskScheduler;

		// Token: 0x04001C63 RID: 7267
		private readonly ConcurrentExclusiveSchedulerPair.ConcurrentExclusiveTaskScheduler m_exclusiveTaskScheduler;

		// Token: 0x04001C64 RID: 7268
		private readonly TaskScheduler m_underlyingTaskScheduler;

		// Token: 0x04001C65 RID: 7269
		private readonly int m_maxConcurrencyLevel;

		// Token: 0x04001C66 RID: 7270
		private readonly int m_maxItemsPerTask;

		// Token: 0x04001C67 RID: 7271
		private int m_processingCount;

		// Token: 0x04001C68 RID: 7272
		private ConcurrentExclusiveSchedulerPair.CompletionState m_completionState;

		// Token: 0x04001C69 RID: 7273
		private const int UNLIMITED_PROCESSING = -1;

		// Token: 0x04001C6A RID: 7274
		private const int EXCLUSIVE_PROCESSING_SENTINEL = -1;

		// Token: 0x04001C6B RID: 7275
		private const int DEFAULT_MAXITEMSPERTASK = -1;

		// Token: 0x02000338 RID: 824
		private sealed class CompletionState : TaskCompletionSource<VoidTaskResult>
		{
			// Token: 0x04001C6C RID: 7276
			internal bool m_completionRequested;

			// Token: 0x04001C6D RID: 7277
			internal bool m_completionQueued;

			// Token: 0x04001C6E RID: 7278
			internal List<Exception> m_exceptions;
		}

		// Token: 0x02000339 RID: 825
		[DebuggerDisplay("Count={CountForDebugger}, MaxConcurrencyLevel={m_maxConcurrencyLevel}, Id={Id}")]
		[DebuggerTypeProxy(typeof(ConcurrentExclusiveSchedulerPair.ConcurrentExclusiveTaskScheduler.DebugView))]
		private sealed class ConcurrentExclusiveTaskScheduler : TaskScheduler
		{
			// Token: 0x06002297 RID: 8855 RVA: 0x0007C9B0 File Offset: 0x0007ABB0
			internal ConcurrentExclusiveTaskScheduler(ConcurrentExclusiveSchedulerPair pair, int maxConcurrencyLevel, ConcurrentExclusiveSchedulerPair.ProcessingMode processingMode)
			{
				this.m_pair = pair;
				this.m_maxConcurrencyLevel = maxConcurrencyLevel;
				this.m_processingMode = processingMode;
				IProducerConsumerQueue<Task> producerConsumerQueue2;
				if (processingMode != ConcurrentExclusiveSchedulerPair.ProcessingMode.ProcessingExclusiveTask)
				{
					IProducerConsumerQueue<Task> producerConsumerQueue = new MultiProducerMultiConsumerQueue<Task>();
					producerConsumerQueue2 = producerConsumerQueue;
				}
				else
				{
					IProducerConsumerQueue<Task> producerConsumerQueue = new SingleProducerSingleConsumerQueue<Task>();
					producerConsumerQueue2 = producerConsumerQueue;
				}
				this.m_tasks = producerConsumerQueue2;
			}

			// Token: 0x17000419 RID: 1049
			// (get) Token: 0x06002298 RID: 8856 RVA: 0x0007C9F2 File Offset: 0x0007ABF2
			public override int MaximumConcurrencyLevel
			{
				get
				{
					return this.m_maxConcurrencyLevel;
				}
			}

			// Token: 0x06002299 RID: 8857 RVA: 0x0007C9FC File Offset: 0x0007ABFC
			protected internal override void QueueTask(Task task)
			{
				object valueLock = this.m_pair.ValueLock;
				lock (valueLock)
				{
					if (this.m_pair.CompletionRequested)
					{
						throw new InvalidOperationException(base.GetType().ToString());
					}
					this.m_tasks.Enqueue(task);
					this.m_pair.ProcessAsyncIfNecessary(false);
				}
			}

			// Token: 0x0600229A RID: 8858 RVA: 0x0007CA74 File Offset: 0x0007AC74
			internal void ExecuteTask(Task task)
			{
				base.TryExecuteTask(task);
			}

			// Token: 0x0600229B RID: 8859 RVA: 0x0007CA80 File Offset: 0x0007AC80
			protected override bool TryExecuteTaskInline(Task task, bool taskWasPreviouslyQueued)
			{
				if (!taskWasPreviouslyQueued && this.m_pair.CompletionRequested)
				{
					return false;
				}
				bool flag = this.m_pair.m_underlyingTaskScheduler == TaskScheduler.Default;
				if (flag && taskWasPreviouslyQueued && !Thread.CurrentThread.IsThreadPoolThread)
				{
					return false;
				}
				if (this.m_pair.m_threadProcessingMode.Value != this.m_processingMode)
				{
					return false;
				}
				if (!flag || taskWasPreviouslyQueued)
				{
					return this.TryExecuteTaskInlineOnTargetScheduler(task);
				}
				return base.TryExecuteTask(task);
			}

			// Token: 0x0600229C RID: 8860 RVA: 0x0007CAF4 File Offset: 0x0007ACF4
			private bool TryExecuteTaskInlineOnTargetScheduler(Task task)
			{
				Task<bool> task2 = new Task<bool>(ConcurrentExclusiveSchedulerPair.ConcurrentExclusiveTaskScheduler.s_tryExecuteTaskShim, Tuple.Create<ConcurrentExclusiveSchedulerPair.ConcurrentExclusiveTaskScheduler, Task>(this, task));
				bool result;
				try
				{
					task2.RunSynchronously(this.m_pair.m_underlyingTaskScheduler);
					result = task2.Result;
				}
				catch
				{
					AggregateException exception = task2.Exception;
					throw;
				}
				finally
				{
					task2.Dispose();
				}
				return result;
			}

			// Token: 0x0600229D RID: 8861 RVA: 0x0007CB5C File Offset: 0x0007AD5C
			private static bool TryExecuteTaskShim(object state)
			{
				Tuple<ConcurrentExclusiveSchedulerPair.ConcurrentExclusiveTaskScheduler, Task> tuple = (Tuple<ConcurrentExclusiveSchedulerPair.ConcurrentExclusiveTaskScheduler, Task>)state;
				return tuple.Item1.TryExecuteTask(tuple.Item2);
			}

			// Token: 0x0600229E RID: 8862 RVA: 0x0007CB81 File Offset: 0x0007AD81
			protected override IEnumerable<Task> GetScheduledTasks()
			{
				return this.m_tasks;
			}

			// Token: 0x1700041A RID: 1050
			// (get) Token: 0x0600229F RID: 8863 RVA: 0x0007CB89 File Offset: 0x0007AD89
			private int CountForDebugger
			{
				get
				{
					return this.m_tasks.Count;
				}
			}

			// Token: 0x04001C6F RID: 7279
			private static readonly Func<object, bool> s_tryExecuteTaskShim = new Func<object, bool>(ConcurrentExclusiveSchedulerPair.ConcurrentExclusiveTaskScheduler.TryExecuteTaskShim);

			// Token: 0x04001C70 RID: 7280
			private readonly ConcurrentExclusiveSchedulerPair m_pair;

			// Token: 0x04001C71 RID: 7281
			private readonly int m_maxConcurrencyLevel;

			// Token: 0x04001C72 RID: 7282
			private readonly ConcurrentExclusiveSchedulerPair.ProcessingMode m_processingMode;

			// Token: 0x04001C73 RID: 7283
			internal readonly IProducerConsumerQueue<Task> m_tasks;

			// Token: 0x0200033A RID: 826
			private sealed class DebugView
			{
				// Token: 0x060022A1 RID: 8865 RVA: 0x0007CBA9 File Offset: 0x0007ADA9
				public DebugView(ConcurrentExclusiveSchedulerPair.ConcurrentExclusiveTaskScheduler scheduler)
				{
					this.m_taskScheduler = scheduler;
				}

				// Token: 0x1700041B RID: 1051
				// (get) Token: 0x060022A2 RID: 8866 RVA: 0x0007CBB8 File Offset: 0x0007ADB8
				public int MaximumConcurrencyLevel
				{
					get
					{
						return this.m_taskScheduler.m_maxConcurrencyLevel;
					}
				}

				// Token: 0x1700041C RID: 1052
				// (get) Token: 0x060022A3 RID: 8867 RVA: 0x0007CBC5 File Offset: 0x0007ADC5
				public IEnumerable<Task> ScheduledTasks
				{
					get
					{
						return this.m_taskScheduler.m_tasks;
					}
				}

				// Token: 0x1700041D RID: 1053
				// (get) Token: 0x060022A4 RID: 8868 RVA: 0x0007CBD2 File Offset: 0x0007ADD2
				public ConcurrentExclusiveSchedulerPair SchedulerPair
				{
					get
					{
						return this.m_taskScheduler.m_pair;
					}
				}

				// Token: 0x04001C74 RID: 7284
				private readonly ConcurrentExclusiveSchedulerPair.ConcurrentExclusiveTaskScheduler m_taskScheduler;
			}
		}

		// Token: 0x0200033B RID: 827
		private sealed class DebugView
		{
			// Token: 0x060022A5 RID: 8869 RVA: 0x0007CBDF File Offset: 0x0007ADDF
			public DebugView(ConcurrentExclusiveSchedulerPair pair)
			{
				this.m_pair = pair;
			}

			// Token: 0x1700041E RID: 1054
			// (get) Token: 0x060022A6 RID: 8870 RVA: 0x0007CBEE File Offset: 0x0007ADEE
			public ConcurrentExclusiveSchedulerPair.ProcessingMode Mode
			{
				get
				{
					return this.m_pair.ModeForDebugger;
				}
			}

			// Token: 0x1700041F RID: 1055
			// (get) Token: 0x060022A7 RID: 8871 RVA: 0x0007CBFB File Offset: 0x0007ADFB
			public IEnumerable<Task> ScheduledExclusive
			{
				get
				{
					return this.m_pair.m_exclusiveTaskScheduler.m_tasks;
				}
			}

			// Token: 0x17000420 RID: 1056
			// (get) Token: 0x060022A8 RID: 8872 RVA: 0x0007CC0D File Offset: 0x0007AE0D
			public IEnumerable<Task> ScheduledConcurrent
			{
				get
				{
					return this.m_pair.m_concurrentTaskScheduler.m_tasks;
				}
			}

			// Token: 0x17000421 RID: 1057
			// (get) Token: 0x060022A9 RID: 8873 RVA: 0x0007CC1F File Offset: 0x0007AE1F
			public int CurrentlyExecutingTaskCount
			{
				get
				{
					if (this.m_pair.m_processingCount != -1)
					{
						return this.m_pair.m_processingCount;
					}
					return 1;
				}
			}

			// Token: 0x17000422 RID: 1058
			// (get) Token: 0x060022AA RID: 8874 RVA: 0x0007CC3C File Offset: 0x0007AE3C
			public TaskScheduler TargetScheduler
			{
				get
				{
					return this.m_pair.m_underlyingTaskScheduler;
				}
			}

			// Token: 0x04001C75 RID: 7285
			private readonly ConcurrentExclusiveSchedulerPair m_pair;
		}

		// Token: 0x0200033C RID: 828
		[Flags]
		private enum ProcessingMode : byte
		{
			// Token: 0x04001C77 RID: 7287
			NotCurrentlyProcessing = 0,
			// Token: 0x04001C78 RID: 7288
			ProcessingExclusiveTask = 1,
			// Token: 0x04001C79 RID: 7289
			ProcessingConcurrentTasks = 2,
			// Token: 0x04001C7A RID: 7290
			Completing = 4,
			// Token: 0x04001C7B RID: 7291
			Completed = 8
		}
	}
}
