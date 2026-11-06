using System;
using System.Threading;
using Microsoft.Win32.SafeHandles;

namespace System.Net.Security
{
	// Token: 0x02000850 RID: 2128
	internal sealed class SecurityContextTokenHandle : CriticalHandleZeroOrMinusOneIsInvalid
	{
		// Token: 0x0600439E RID: 17310 RVA: 0x000EC00C File Offset: 0x000EA20C
		private SecurityContextTokenHandle()
		{
		}

		// Token: 0x0600439F RID: 17311 RVA: 0x000EC014 File Offset: 0x000EA214
		internal IntPtr DangerousGetHandle()
		{
			return this.handle;
		}

		// Token: 0x060043A0 RID: 17312 RVA: 0x000EC01C File Offset: 0x000EA21C
		protected override bool ReleaseHandle()
		{
			return this.IsInvalid || Interlocked.Increment(ref this._disposed) != 1 || global::Interop.Kernel32.CloseHandle(this.handle);
		}

		// Token: 0x040028E9 RID: 10473
		private int _disposed;
	}
}
