using System;
using System.Runtime.InteropServices;
using System.Text;

namespace Steamworks
{
	// Token: 0x02000189 RID: 393
	// (Invoke) Token: 0x06000952 RID: 2386
	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	public delegate void SteamAPIWarningMessageHook_t(int nSeverity, StringBuilder pchDebugText);
}
