using System;

namespace System.Net.NetworkInformation
{
	// Token: 0x02000704 RID: 1796
	internal class SystemTcpConnectionInformation : TcpConnectionInformation
	{
		// Token: 0x06003994 RID: 14740 RVA: 0x000C8E70 File Offset: 0x000C7070
		public SystemTcpConnectionInformation(IPEndPoint local, IPEndPoint remote, TcpState state)
		{
			this.localEndPoint = local;
			this.remoteEndPoint = remote;
			this.state = state;
		}

		// Token: 0x17000C88 RID: 3208
		// (get) Token: 0x06003995 RID: 14741 RVA: 0x000C8E8D File Offset: 0x000C708D
		public override TcpState State
		{
			get
			{
				return this.state;
			}
		}

		// Token: 0x17000C89 RID: 3209
		// (get) Token: 0x06003996 RID: 14742 RVA: 0x000C8E95 File Offset: 0x000C7095
		public override IPEndPoint LocalEndPoint
		{
			get
			{
				return this.localEndPoint;
			}
		}

		// Token: 0x17000C8A RID: 3210
		// (get) Token: 0x06003997 RID: 14743 RVA: 0x000C8E9D File Offset: 0x000C709D
		public override IPEndPoint RemoteEndPoint
		{
			get
			{
				return this.remoteEndPoint;
			}
		}

		// Token: 0x040021CB RID: 8651
		private IPEndPoint localEndPoint;

		// Token: 0x040021CC RID: 8652
		private IPEndPoint remoteEndPoint;

		// Token: 0x040021CD RID: 8653
		private TcpState state;
	}
}
