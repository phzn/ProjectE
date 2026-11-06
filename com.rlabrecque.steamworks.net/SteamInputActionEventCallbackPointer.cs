using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x02000198 RID: 408
	// (Invoke) Token: 0x060009FB RID: 2555
	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	public delegate void SteamInputActionEventCallbackPointer(IntPtr SteamInputActionEvent);
}
