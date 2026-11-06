using System;

namespace Microsoft.Win32
{
	/// <summary>Defines identifiers that represent how the current logon session is ending.</summary>
	// Token: 0x02000122 RID: 290
	public enum SessionEndReasons
	{
		/// <summary>The user is logging off and ending the current user session. The operating system continues to run.</summary>
		// Token: 0x040004D3 RID: 1235
		Logoff = 1,
		/// <summary>The operating system is shutting down.</summary>
		// Token: 0x040004D4 RID: 1236
		SystemShutdown
	}
}
