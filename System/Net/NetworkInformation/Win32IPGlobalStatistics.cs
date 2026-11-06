using System;

namespace System.Net.NetworkInformation
{
	// Token: 0x0200072B RID: 1835
	internal class Win32IPGlobalStatistics : IPGlobalStatistics
	{
		// Token: 0x06003A86 RID: 14982 RVA: 0x000CAFB6 File Offset: 0x000C91B6
		public Win32IPGlobalStatistics(Win32_MIB_IPSTATS info)
		{
			this.info = info;
		}

		// Token: 0x17000CC7 RID: 3271
		// (get) Token: 0x06003A87 RID: 14983 RVA: 0x000CAFC5 File Offset: 0x000C91C5
		public override int DefaultTtl
		{
			get
			{
				return this.info.DefaultTTL;
			}
		}

		// Token: 0x17000CC8 RID: 3272
		// (get) Token: 0x06003A88 RID: 14984 RVA: 0x000CAFD2 File Offset: 0x000C91D2
		public override bool ForwardingEnabled
		{
			get
			{
				return this.info.Forwarding != 0;
			}
		}

		// Token: 0x17000CC9 RID: 3273
		// (get) Token: 0x06003A89 RID: 14985 RVA: 0x000CAFE2 File Offset: 0x000C91E2
		public override int NumberOfInterfaces
		{
			get
			{
				return this.info.NumIf;
			}
		}

		// Token: 0x17000CCA RID: 3274
		// (get) Token: 0x06003A8A RID: 14986 RVA: 0x000CAFEF File Offset: 0x000C91EF
		public override int NumberOfIPAddresses
		{
			get
			{
				return this.info.NumAddr;
			}
		}

		// Token: 0x17000CCB RID: 3275
		// (get) Token: 0x06003A8B RID: 14987 RVA: 0x000CAFFC File Offset: 0x000C91FC
		public override int NumberOfRoutes
		{
			get
			{
				return this.info.NumRoutes;
			}
		}

		// Token: 0x17000CCC RID: 3276
		// (get) Token: 0x06003A8C RID: 14988 RVA: 0x000CB009 File Offset: 0x000C9209
		public override long OutputPacketRequests
		{
			get
			{
				return (long)((ulong)this.info.OutRequests);
			}
		}

		// Token: 0x17000CCD RID: 3277
		// (get) Token: 0x06003A8D RID: 14989 RVA: 0x000CB017 File Offset: 0x000C9217
		public override long OutputPacketRoutingDiscards
		{
			get
			{
				return (long)((ulong)this.info.RoutingDiscards);
			}
		}

		// Token: 0x17000CCE RID: 3278
		// (get) Token: 0x06003A8E RID: 14990 RVA: 0x000CB025 File Offset: 0x000C9225
		public override long OutputPacketsDiscarded
		{
			get
			{
				return (long)((ulong)this.info.OutDiscards);
			}
		}

		// Token: 0x17000CCF RID: 3279
		// (get) Token: 0x06003A8F RID: 14991 RVA: 0x000CB033 File Offset: 0x000C9233
		public override long OutputPacketsWithNoRoute
		{
			get
			{
				return (long)((ulong)this.info.OutNoRoutes);
			}
		}

		// Token: 0x17000CD0 RID: 3280
		// (get) Token: 0x06003A90 RID: 14992 RVA: 0x000CB041 File Offset: 0x000C9241
		public override long PacketFragmentFailures
		{
			get
			{
				return (long)((ulong)this.info.FragFails);
			}
		}

		// Token: 0x17000CD1 RID: 3281
		// (get) Token: 0x06003A91 RID: 14993 RVA: 0x000CB04F File Offset: 0x000C924F
		public override long PacketReassembliesRequired
		{
			get
			{
				return (long)((ulong)this.info.ReasmReqds);
			}
		}

		// Token: 0x17000CD2 RID: 3282
		// (get) Token: 0x06003A92 RID: 14994 RVA: 0x000CB05D File Offset: 0x000C925D
		public override long PacketReassemblyFailures
		{
			get
			{
				return (long)((ulong)this.info.ReasmFails);
			}
		}

		// Token: 0x17000CD3 RID: 3283
		// (get) Token: 0x06003A93 RID: 14995 RVA: 0x000CB06B File Offset: 0x000C926B
		public override long PacketReassemblyTimeout
		{
			get
			{
				return (long)((ulong)this.info.ReasmTimeout);
			}
		}

		// Token: 0x17000CD4 RID: 3284
		// (get) Token: 0x06003A94 RID: 14996 RVA: 0x000CB079 File Offset: 0x000C9279
		public override long PacketsFragmented
		{
			get
			{
				return (long)((ulong)this.info.FragOks);
			}
		}

		// Token: 0x17000CD5 RID: 3285
		// (get) Token: 0x06003A95 RID: 14997 RVA: 0x000CB087 File Offset: 0x000C9287
		public override long PacketsReassembled
		{
			get
			{
				return (long)((ulong)this.info.ReasmOks);
			}
		}

		// Token: 0x17000CD6 RID: 3286
		// (get) Token: 0x06003A96 RID: 14998 RVA: 0x000CB095 File Offset: 0x000C9295
		public override long ReceivedPackets
		{
			get
			{
				return (long)((ulong)this.info.InReceives);
			}
		}

		// Token: 0x17000CD7 RID: 3287
		// (get) Token: 0x06003A97 RID: 14999 RVA: 0x000CB0A3 File Offset: 0x000C92A3
		public override long ReceivedPacketsDelivered
		{
			get
			{
				return (long)((ulong)this.info.InDelivers);
			}
		}

		// Token: 0x17000CD8 RID: 3288
		// (get) Token: 0x06003A98 RID: 15000 RVA: 0x000CB0B1 File Offset: 0x000C92B1
		public override long ReceivedPacketsDiscarded
		{
			get
			{
				return (long)((ulong)this.info.InDiscards);
			}
		}

		// Token: 0x17000CD9 RID: 3289
		// (get) Token: 0x06003A99 RID: 15001 RVA: 0x000CB0BF File Offset: 0x000C92BF
		public override long ReceivedPacketsForwarded
		{
			get
			{
				return (long)((ulong)this.info.ForwDatagrams);
			}
		}

		// Token: 0x17000CDA RID: 3290
		// (get) Token: 0x06003A9A RID: 15002 RVA: 0x000CB0CD File Offset: 0x000C92CD
		public override long ReceivedPacketsWithAddressErrors
		{
			get
			{
				return (long)((ulong)this.info.InAddrErrors);
			}
		}

		// Token: 0x17000CDB RID: 3291
		// (get) Token: 0x06003A9B RID: 15003 RVA: 0x000CB0DB File Offset: 0x000C92DB
		public override long ReceivedPacketsWithHeadersErrors
		{
			get
			{
				return (long)((ulong)this.info.InHdrErrors);
			}
		}

		// Token: 0x17000CDC RID: 3292
		// (get) Token: 0x06003A9C RID: 15004 RVA: 0x000CB0E9 File Offset: 0x000C92E9
		public override long ReceivedPacketsWithUnknownProtocol
		{
			get
			{
				return (long)((ulong)this.info.InUnknownProtos);
			}
		}

		// Token: 0x0400225B RID: 8795
		private Win32_MIB_IPSTATS info;
	}
}
