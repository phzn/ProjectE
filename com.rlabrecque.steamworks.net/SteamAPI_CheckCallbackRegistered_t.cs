using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x0200018A RID: 394
	// (Invoke) Token: 0x06000956 RID: 2390
	[UnmanagedFunctionPointer(CallingConvention.StdCall)]
	public delegate void SteamAPI_CheckCallbackRegistered_t(int iCallbackNum);
}
