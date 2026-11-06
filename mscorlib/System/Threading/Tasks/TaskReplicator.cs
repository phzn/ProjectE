using System;
using System.Collections.Concurrent;

namespace System.Threading.Tasks
{
	// Token: 0x02000332 RID: 818
	internal class TaskReplicator
	{
		// Token: 0x06002269 RID: 8809 RVA: 0x0007C084 File Offset: 0x0007A284
		private TaskReplicator(ParallelOptions options, bool stopOnFirstFailure)
		{
			this._scheduler = options.TaskScheduler ?? TaskScheduler.Current;
			this._stopOnFirstFailure = stopOnFirstFailure;
		}

		// Token: 0x0600226A RID: 8810 RVA: 0x0007C0B4 File Offset: 0x0007A2B4
		public static void Run<TState>(TaskReplicator.ReplicatableUserAction<TState> action, ParallelOptions options, bool stopOnFirstFailure)
		{
			int num = ((options.EffectiveMaxConcurrencyLevel > 0) ? options.EffectiveMaxConcurrencyLevel : int.MaxValue);
			TaskReplicator taskReplicator = new TaskReplicator(options, stopOnFirstFailure);
			new TaskReplicator.Replica<TState>(taskReplicator, num, 1073741823, action).Start();
			TaskReplicator.Replica replica;
			while (taskReplicator._pendingReplicas.TryDequeue(out replica))
			{
				replica.Wait();
			}
			if (taskReplicator._exceptions != null)
			{
				throw new AggregateException(taskReplicator._exceptions);
			}
		}

		// Token: 0x0600226B RID: 8811 RVA: 0x0007C120 File Offset: 0x0007A320
		private static int GenerateCooperativeMultitaskingTaskTimeout()
		{
			int processorCount = PlatformHelper.ProcessorCount;
			int tickCount = Environment.TickCount;
			return 100 + tickCount % processorCount * 50;
		}

		// Token: 0x04001C50 RID: 7248
		private readonly TaskScheduler _scheduler;

		// Token: 0x04001C51 RID: 7249
		private readonly bool _stopOnFirstFailure;

		// Token: 0x04001C52 RID: 7250
		private readonly ConcurrentQueue<TaskReplicator.Replica> _pendingReplicas = new ConcurrentQueue<TaskReplicator.Replica>();

		// Token: 0x04001C53 RID: 7251
		private ConcurrentQueue<Exception> _exceptions;

		// Token: 0x04001C54 RID: 7252
		private bool _stopReplicating;

		// Token: 0x04001C55 RID: 7253
		private const int CooperativeMultitaskingTaskTimeout_Min = 100;

		// Token: 0x04001C56 RID: 7254
		private const int CooperativeMultitaskingTaskTimeout_Increment = 50;

		// Token: 0x04001C57 RID: 7255
		private const int CooperativeMultitaskingTaskTimeout_RootTask = 1073741823;

		// Token: 0x02000333 RID: 819
		// (Invoke) Token: 0x0600226D RID: 8813
		public delegate void ReplicatableUserAction<TState>(ref TState replicaState, int timeout, out bool yieldedBeforeCompletion);

		// Token: 0x02000334 RID: 820
		private abstract class Replica
		{
			// Token: 0x06002270 RID: 8816 RVA: 0x0007C144 File Offset: 0x0007A344
			protected Replica(TaskReplicator replicator, int maxConcurrency, int timeout)
			{
				this._replicator = replicator;
				this._timeout = timeout;
				this._remainingConcurrency = maxConcurrency - 1;
				this._pendingTask = new Task(delegate(object s)
				{
					((TaskReplicator.Replica)s).Execute();
				}, this);
				this._replicator._pendingReplicas.Enqueue(this);
			}

			// Token: 0x06002271 RID: 8817 RVA: 0x0007C1AC File Offset: 0x0007A3AC
			public void Start()
			{
				this._pendingTask.RunSynchronously(this._replicator._scheduler);
			}

			// Token: 0x06002272 RID: 8818 RVA: 0x0007C1C8 File Offset: 0x0007A3C8
			public void Wait()
			{
				Task pendingTask;
				while ((pendingTask = this._pendingTask) != null)
				{
					pendingTask.Wait();
				}
			}

			// Token: 0x06002273 RID: 8819 RVA: 0x0007C1EC File Offset: 0x0007A3EC
			public void Execute()
			{
				try
				{
					if (!this._replicator._stopReplicating && this._remainingConcurrency > 0)
					{
						this.CreateNewReplica();
						this._remainingConcurrency = 0;
					}
					bool flag;
					this.ExecuteAction(out flag);
					if (flag)
					{
						this._pendingTask = new Task(delegate(object s)
						{
							((TaskReplicator.Replica)s).Execute();
						}, this, CancellationToken.None, TaskCreationOptions.None);
						this._pendingTask.Start(this._replicator._scheduler);
					}
					else
					{
						this._replicator._stopReplicating = true;
						this._pendingTask = null;
					}
				}
				catch (Exception ex)
				{
					LazyInitializer.EnsureInitialized<ConcurrentQueue<Exception>>(ref this._replicator._exceptions).Enqueue(ex);
					if (this._replicator._stopOnFirstFailure)
					{
						this._replicator._stopReplicating = true;
					}
					this._pendingTask = null;
				}
			}

			// Token: 0x06002274 RID: 8820
			protected abstract void CreateNewReplica();

			// Token: 0x06002275 RID: 8821
			protected abstract void ExecuteAction(out bool yieldedBeforeCompletion);

			// Token: 0x04001C58 RID: 7256
			protected readonly TaskReplicator _replicator;

			// Token: 0x04001C59 RID: 7257
			protected readonly int _timeout;

			// Token: 0x04001C5A RID: 7258
			protected int _remainingConcurrency;

			// Token: 0x04001C5B RID: 7259
			protected volatile Task _pendingTask;
		}

		// Token: 0x02000336 RID: 822
		private sealed class Replica<TState> : TaskReplicator.Replica
		{
			// Token: 0x0600227A RID: 8826 RVA: 0x0007C2F1 File Offset: 0x0007A4F1
			public Replica(TaskReplicator replicator, int maxConcurrency, int timeout, TaskReplicator.ReplicatableUserAction<TState> action)
				: base(replicator, maxConcurrency, timeout)
			{
				this._action = action;
			}

			// Token: 0x0600227B RID: 8827 RVA: 0x0007C304 File Offset: 0x0007A504
			protected override void CreateNewReplica()
			{
				new TaskReplicator.Replica<TState>(this._replicator, this._remainingConcurrency, TaskReplicator.GenerateCooperativeMultitaskingTaskTimeout(), this._action)._pendingTask.Start(this._replicator._scheduler);
			}

			// Token: 0x0600227C RID: 8828 RVA: 0x0007C339 File Offset: 0x0007A539
			protected override void ExecuteAction(out bool yieldedBeforeCompletion)
			{
				this._action(ref this._state, this._timeout, out yieldedBeforeCompletion);
			}

			// Token: 0x04001C5F RID: 7263
			private readonly TaskReplicator.ReplicatableUserAction<TState> _action;

			// Token: 0x04001C60 RID: 7264
			private TState _state;
		}
	}
}
