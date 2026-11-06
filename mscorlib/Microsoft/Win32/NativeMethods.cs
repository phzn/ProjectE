using System;
using System.Runtime.CompilerServices;

namespace Microsoft.Win32
{
	// Token: 0x020000AB RID: 171
	internal static class NativeMethods
	{
		// Token: 0x0600046E RID: 1134
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern int GetCurrentProcessId();
	}
}
