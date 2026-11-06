using System;
using System.Runtime.CompilerServices;
using System.Threading;
using Unity.Collections;

namespace Unity.Entities.LowLevel
{
	// Token: 0x020004C8 RID: 1224
	[GenerateTestsForBurstCompatibility]
	internal struct SpinLock
	{
		// Token: 0x06001D65 RID: 7525 RVA: 0x00066814 File Offset: 0x00064A14
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void Acquire()
		{
			while (Interlocked.CompareExchange(ref this.m_Lock, 1, 0) != 0)
			{
				while (Volatile.Read(ref this.m_Lock) == 1)
				{
				}
			}
		}

		// Token: 0x06001D66 RID: 7526 RVA: 0x00066835 File Offset: 0x00064A35
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool TryAcquire()
		{
			return Volatile.Read(ref this.m_Lock) == 0 && Interlocked.CompareExchange(ref this.m_Lock, 1, 0) == 0;
		}

		// Token: 0x06001D67 RID: 7527 RVA: 0x00066856 File Offset: 0x00064A56
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool TryAcquire(bool spin)
		{
			if (spin)
			{
				this.Acquire();
				return true;
			}
			return this.TryAcquire();
		}

		// Token: 0x06001D68 RID: 7528 RVA: 0x00066869 File Offset: 0x00064A69
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void Release()
		{
			Volatile.Write(ref this.m_Lock, 0);
		}

		// Token: 0x04000A90 RID: 2704
		private int m_Lock;
	}
}
