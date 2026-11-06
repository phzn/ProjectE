using System;
using System.Threading;
using Unity.Burst.Intrinsics;

namespace Pathfinding.Sync
{
	// Token: 0x02000242 RID: 578
	internal struct SpinLock
	{
		// Token: 0x06000E79 RID: 3705 RVA: 0x0005927F File Offset: 0x0005747F
		public void Lock()
		{
			while (Interlocked.CompareExchange(ref this.locked, 1, 0) != 0)
			{
				Common.Pause();
			}
			Thread.MemoryBarrier();
		}

		// Token: 0x06000E7A RID: 3706 RVA: 0x0005929C File Offset: 0x0005749C
		public void Unlock()
		{
			Thread.MemoryBarrier();
			if (Interlocked.Exchange(ref this.locked, 0) == 0)
			{
				throw new InvalidOperationException("Trying to unlock a lock which is not locked");
			}
		}

		// Token: 0x04000ADE RID: 2782
		private volatile int locked;
	}
}
