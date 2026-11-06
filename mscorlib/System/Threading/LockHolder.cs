using System;
using System.Runtime.CompilerServices;

namespace System.Threading
{
	// Token: 0x020002B3 RID: 691
	[ReflectionBlocked]
	public struct LockHolder : IDisposable
	{
		// Token: 0x06001E4F RID: 7759 RVA: 0x000704D0 File Offset: 0x0006E6D0
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static LockHolder Hold(Lock l)
		{
			l.Acquire();
			LockHolder lockHolder;
			lockHolder._lock = l;
			return lockHolder;
		}

		// Token: 0x06001E50 RID: 7760 RVA: 0x000704EC File Offset: 0x0006E6EC
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void Dispose()
		{
			this._lock.Release();
		}

		// Token: 0x04001A9E RID: 6814
		private Lock _lock;
	}
}
