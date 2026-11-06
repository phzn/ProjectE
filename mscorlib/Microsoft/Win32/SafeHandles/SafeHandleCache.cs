using System;
using System.Runtime.InteropServices;
using System.Threading;

namespace Microsoft.Win32.SafeHandles
{
	// Token: 0x020000AF RID: 175
	internal static class SafeHandleCache<T> where T : SafeHandle
	{
		// Token: 0x06000473 RID: 1139 RVA: 0x0001740C File Offset: 0x0001560C
		internal static T GetInvalidHandle(Func<T> invalidHandleFactory)
		{
			T t = Volatile.Read<T>(ref SafeHandleCache<T>.s_invalidHandle);
			if (t == null)
			{
				T t2 = invalidHandleFactory();
				t = Interlocked.CompareExchange<T>(ref SafeHandleCache<T>.s_invalidHandle, t2, default(T));
				if (t == null)
				{
					GC.SuppressFinalize(t2);
					t = t2;
				}
				else
				{
					t2.Dispose();
				}
			}
			return t;
		}

		// Token: 0x06000474 RID: 1140 RVA: 0x0001746B File Offset: 0x0001566B
		internal static bool IsCachedInvalidHandle(SafeHandle handle)
		{
			return handle == Volatile.Read<T>(ref SafeHandleCache<T>.s_invalidHandle);
		}

		// Token: 0x04000FB4 RID: 4020
		private static T s_invalidHandle;
	}
}
