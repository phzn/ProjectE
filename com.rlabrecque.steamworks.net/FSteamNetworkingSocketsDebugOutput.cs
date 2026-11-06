using System;
using System.Runtime.InteropServices;
using System.Text;

namespace Steamworks
{
	// Token: 0x020001A4 RID: 420
	// (Invoke) Token: 0x06000A57 RID: 2647
	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	public delegate void FSteamNetworkingSocketsDebugOutput(ESteamNetworkingSocketsDebugOutputType nType, StringBuilder pszMsg);
}
