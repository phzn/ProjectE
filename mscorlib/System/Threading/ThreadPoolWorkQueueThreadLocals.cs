using System;
using System.Security;

namespace System.Threading
{
	// Token: 0x020002E2 RID: 738
	internal sealed class ThreadPoolWorkQueueThreadLocals
	{
		// Token: 0x06002025 RID: 8229 RVA: 0x00075507 File Offset: 0x00073707
		public ThreadPoolWorkQueueThreadLocals(ThreadPoolWorkQueue tpq)
		{
			this.workQueue = tpq;
			this.workStealingQueue = new ThreadPoolWorkQueue.WorkStealingQueue();
			ThreadPoolWorkQueue.allThreadQueues.Add(this.workStealingQueue);
		}

		// Token: 0x06002026 RID: 8230 RVA: 0x00075548 File Offset: 0x00073748
		[SecurityCritical]
		private void CleanUp()
		{
			if (this.workStealingQueue != null)
			{
				if (this.workQueue != null)
				{
					bool flag = false;
					while (!flag)
					{
						try
						{
						}
						finally
						{
							IThreadPoolWorkItem threadPoolWorkItem = null;
							if (this.workStealingQueue.LocalPop(out threadPoolWorkItem))
							{
								this.workQueue.Enqueue(threadPoolWorkItem, true);
							}
							else
							{
								flag = true;
							}
						}
					}
				}
				ThreadPoolWorkQueue.allThreadQueues.Remove(this.workStealingQueue);
			}
		}

		// Token: 0x06002027 RID: 8231 RVA: 0x000755B4 File Offset: 0x000737B4
		[SecuritySafeCritical]
		~ThreadPoolWorkQueueThreadLocals()
		{
			if (!Environment.HasShutdownStarted && !AppDomain.CurrentDomain.IsFinalizingForUnload())
			{
				this.CleanUp();
			}
		}

		// Token: 0x04001B3F RID: 6975
		[SecurityCritical]
		[ThreadStatic]
		public static ThreadPoolWorkQueueThreadLocals threadLocals;

		// Token: 0x04001B40 RID: 6976
		public readonly ThreadPoolWorkQueue workQueue;

		// Token: 0x04001B41 RID: 6977
		public readonly ThreadPoolWorkQueue.WorkStealingQueue workStealingQueue;

		// Token: 0x04001B42 RID: 6978
		public readonly Random random = new Random(Thread.CurrentThread.ManagedThreadId);
	}
}
