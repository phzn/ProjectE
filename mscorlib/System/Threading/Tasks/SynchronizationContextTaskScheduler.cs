using System;
using System.Collections.Generic;

namespace System.Threading.Tasks
{
	// Token: 0x0200037B RID: 891
	internal sealed class SynchronizationContextTaskScheduler : TaskScheduler
	{
		// Token: 0x06002529 RID: 9513 RVA: 0x0008442C File Offset: 0x0008262C
		internal SynchronizationContextTaskScheduler()
		{
			SynchronizationContext synchronizationContext = SynchronizationContext.Current;
			if (synchronizationContext == null)
			{
				throw new InvalidOperationException("The current SynchronizationContext may not be used as a TaskScheduler.");
			}
			this.m_synchronizationContext = synchronizationContext;
		}

		// Token: 0x0600252A RID: 9514 RVA: 0x0008445A File Offset: 0x0008265A
		protected internal override void QueueTask(Task task)
		{
			this.m_synchronizationContext.Post(SynchronizationContextTaskScheduler.s_postCallback, task);
		}

		// Token: 0x0600252B RID: 9515 RVA: 0x0008446D File Offset: 0x0008266D
		protected override bool TryExecuteTaskInline(Task task, bool taskWasPreviouslyQueued)
		{
			return SynchronizationContext.Current == this.m_synchronizationContext && base.TryExecuteTask(task);
		}

		// Token: 0x0600252C RID: 9516 RVA: 0x0000AF5E File Offset: 0x0000915E
		protected override IEnumerable<Task> GetScheduledTasks()
		{
			return null;
		}

		// Token: 0x17000480 RID: 1152
		// (get) Token: 0x0600252D RID: 9517 RVA: 0x000040F7 File Offset: 0x000022F7
		public override int MaximumConcurrencyLevel
		{
			get
			{
				return 1;
			}
		}

		// Token: 0x04001D54 RID: 7508
		private SynchronizationContext m_synchronizationContext;

		// Token: 0x04001D55 RID: 7509
		private static readonly SendOrPostCallback s_postCallback = delegate(object s)
		{
			((Task)s).ExecuteEntry(true);
		};
	}
}
