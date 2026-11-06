using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x02000165 RID: 357
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct P2PSessionState_t
	{
		// Token: 0x04000AA4 RID: 2724
		public byte m_bConnectionActive;

		// Token: 0x04000AA5 RID: 2725
		public byte m_bConnecting;

		// Token: 0x04000AA6 RID: 2726
		public byte m_eP2PSessionError;

		// Token: 0x04000AA7 RID: 2727
		public byte m_bUsingRelay;

		// Token: 0x04000AA8 RID: 2728
		public int m_nBytesQueuedForSend;

		// Token: 0x04000AA9 RID: 2729
		public int m_nPacketsQueuedForSend;

		// Token: 0x04000AAA RID: 2730
		public uint m_nRemoteIP;

		// Token: 0x04000AAB RID: 2731
		public ushort m_nRemotePort;
	}
}
