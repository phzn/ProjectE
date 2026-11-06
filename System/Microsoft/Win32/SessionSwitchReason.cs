using System;

namespace Microsoft.Win32
{
	/// <summary>Defines identifiers used to represent the type of a session switch event.</summary>
	// Token: 0x02000129 RID: 297
	public enum SessionSwitchReason
	{
		/// <summary>A session has been connected from the console.</summary>
		// Token: 0x040004DA RID: 1242
		ConsoleConnect = 1,
		/// <summary>A session has been disconnected from the console.</summary>
		// Token: 0x040004DB RID: 1243
		ConsoleDisconnect,
		/// <summary>A session has been connected from a remote connection.</summary>
		// Token: 0x040004DC RID: 1244
		RemoteConnect,
		/// <summary>A session has been disconnected from a remote connection.</summary>
		// Token: 0x040004DD RID: 1245
		RemoteDisconnect,
		/// <summary>A user has logged on to a session.</summary>
		// Token: 0x040004DE RID: 1246
		SessionLogon,
		/// <summary>A user has logged off from a session.</summary>
		// Token: 0x040004DF RID: 1247
		SessionLogoff,
		/// <summary>A session has been locked.</summary>
		// Token: 0x040004E0 RID: 1248
		SessionLock,
		/// <summary>A session has been unlocked.</summary>
		// Token: 0x040004E1 RID: 1249
		SessionUnlock,
		/// <summary>A session has changed its status to or from remote controlled mode.</summary>
		// Token: 0x040004E2 RID: 1250
		SessionRemoteControl
	}
}
