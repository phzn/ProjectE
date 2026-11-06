using System;
using System.Runtime.InteropServices;
using UnityEngine.Scripting;

namespace Unity.Entities
{
	// Token: 0x020001C0 RID: 448
	[Preserve]
	public static class SystemBaseDelegates
	{
		// Token: 0x020001C1 RID: 449
		// (Invoke) Token: 0x06000A54 RID: 2644
		[Preserve]
		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		public delegate void Function(IntPtr systemPtr, IntPtr state);
	}
}
