using System;

namespace System.Data.SqlClient
{
	// Token: 0x02000260 RID: 608
	internal class RoutingInfo
	{
		// Token: 0x17000534 RID: 1332
		// (get) Token: 0x06001CBF RID: 7359 RVA: 0x00089180 File Offset: 0x00087380
		// (set) Token: 0x06001CC0 RID: 7360 RVA: 0x00089188 File Offset: 0x00087388
		internal byte Protocol { get; private set; }

		// Token: 0x17000535 RID: 1333
		// (get) Token: 0x06001CC1 RID: 7361 RVA: 0x00089191 File Offset: 0x00087391
		// (set) Token: 0x06001CC2 RID: 7362 RVA: 0x00089199 File Offset: 0x00087399
		internal ushort Port { get; private set; }

		// Token: 0x17000536 RID: 1334
		// (get) Token: 0x06001CC3 RID: 7363 RVA: 0x000891A2 File Offset: 0x000873A2
		// (set) Token: 0x06001CC4 RID: 7364 RVA: 0x000891AA File Offset: 0x000873AA
		internal string ServerName { get; private set; }

		// Token: 0x06001CC5 RID: 7365 RVA: 0x000891B3 File Offset: 0x000873B3
		internal RoutingInfo(byte protocol, ushort port, string servername)
		{
			this.Protocol = protocol;
			this.Port = port;
			this.ServerName = servername;
		}
	}
}
