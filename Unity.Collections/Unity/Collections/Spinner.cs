using System;
using System.Runtime.CompilerServices;
using System.Threading;

namespace Unity.Collections
{
	// Token: 0x02000019 RID: 25
	[GenerateTestsForBurstCompatibility]
	internal struct Spinner
	{
		// Token: 0x06000049 RID: 73 RVA: 0x000027D4 File Offset: 0x000009D4
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal void Acquire()
		{
			while (Interlocked.CompareExchange(ref this.m_Lock, 1, 0) != 0)
			{
				while (Volatile.Read(ref this.m_Lock) == 1)
				{
				}
			}
		}

		// Token: 0x0600004A RID: 74 RVA: 0x000027F5 File Offset: 0x000009F5
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal bool TryAcquire()
		{
			return Volatile.Read(ref this.m_Lock) == 0 && Interlocked.CompareExchange(ref this.m_Lock, 1, 0) == 0;
		}

		// Token: 0x0600004B RID: 75 RVA: 0x00002816 File Offset: 0x00000A16
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal bool TryAcquire(bool spin)
		{
			if (spin)
			{
				this.Acquire();
				return true;
			}
			return this.TryAcquire();
		}

		// Token: 0x0600004C RID: 76 RVA: 0x00002829 File Offset: 0x00000A29
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal void Release()
		{
			Volatile.Write(ref this.m_Lock, 0);
		}

		// Token: 0x0400000E RID: 14
		private int m_Lock;
	}
}
