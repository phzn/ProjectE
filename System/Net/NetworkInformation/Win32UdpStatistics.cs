using System;

namespace System.Net.NetworkInformation
{
	// Token: 0x02000752 RID: 1874
	internal class Win32UdpStatistics : UdpStatistics
	{
		// Token: 0x06003B38 RID: 15160 RVA: 0x000CBFEB File Offset: 0x000CA1EB
		public Win32UdpStatistics(Win32_MIB_UDPSTATS info)
		{
			this.info = info;
		}

		// Token: 0x17000D53 RID: 3411
		// (get) Token: 0x06003B39 RID: 15161 RVA: 0x000CBFFA File Offset: 0x000CA1FA
		public override long DatagramsReceived
		{
			get
			{
				return (long)((ulong)this.info.InDatagrams);
			}
		}

		// Token: 0x17000D54 RID: 3412
		// (get) Token: 0x06003B3A RID: 15162 RVA: 0x000CC008 File Offset: 0x000CA208
		public override long DatagramsSent
		{
			get
			{
				return (long)((ulong)this.info.OutDatagrams);
			}
		}

		// Token: 0x17000D55 RID: 3413
		// (get) Token: 0x06003B3B RID: 15163 RVA: 0x000CC016 File Offset: 0x000CA216
		public override long IncomingDatagramsDiscarded
		{
			get
			{
				return (long)((ulong)this.info.NoPorts);
			}
		}

		// Token: 0x17000D56 RID: 3414
		// (get) Token: 0x06003B3C RID: 15164 RVA: 0x000CC024 File Offset: 0x000CA224
		public override long IncomingDatagramsWithErrors
		{
			get
			{
				return (long)((ulong)this.info.InErrors);
			}
		}

		// Token: 0x17000D57 RID: 3415
		// (get) Token: 0x06003B3D RID: 15165 RVA: 0x000CC032 File Offset: 0x000CA232
		public override int UdpListeners
		{
			get
			{
				return this.info.NumAddrs;
			}
		}

		// Token: 0x0400235B RID: 9051
		private Win32_MIB_UDPSTATS info;
	}
}
