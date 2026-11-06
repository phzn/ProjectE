using System;

namespace System.Net.NetworkInformation
{
	// Token: 0x02000750 RID: 1872
	internal class Win32TcpStatistics : TcpStatistics
	{
		// Token: 0x06003B29 RID: 15145 RVA: 0x000CBF18 File Offset: 0x000CA118
		public Win32TcpStatistics(Win32_MIB_TCPSTATS info)
		{
			this.info = info;
		}

		// Token: 0x17000D45 RID: 3397
		// (get) Token: 0x06003B2A RID: 15146 RVA: 0x000CBF27 File Offset: 0x000CA127
		public override long ConnectionsAccepted
		{
			get
			{
				return (long)((ulong)this.info.PassiveOpens);
			}
		}

		// Token: 0x17000D46 RID: 3398
		// (get) Token: 0x06003B2B RID: 15147 RVA: 0x000CBF35 File Offset: 0x000CA135
		public override long ConnectionsInitiated
		{
			get
			{
				return (long)((ulong)this.info.ActiveOpens);
			}
		}

		// Token: 0x17000D47 RID: 3399
		// (get) Token: 0x06003B2C RID: 15148 RVA: 0x000CBF43 File Offset: 0x000CA143
		public override long CumulativeConnections
		{
			get
			{
				return (long)((ulong)this.info.NumConns);
			}
		}

		// Token: 0x17000D48 RID: 3400
		// (get) Token: 0x06003B2D RID: 15149 RVA: 0x000CBF51 File Offset: 0x000CA151
		public override long CurrentConnections
		{
			get
			{
				return (long)((ulong)this.info.CurrEstab);
			}
		}

		// Token: 0x17000D49 RID: 3401
		// (get) Token: 0x06003B2E RID: 15150 RVA: 0x000CBF5F File Offset: 0x000CA15F
		public override long ErrorsReceived
		{
			get
			{
				return (long)((ulong)this.info.InErrs);
			}
		}

		// Token: 0x17000D4A RID: 3402
		// (get) Token: 0x06003B2F RID: 15151 RVA: 0x000CBF6D File Offset: 0x000CA16D
		public override long FailedConnectionAttempts
		{
			get
			{
				return (long)((ulong)this.info.AttemptFails);
			}
		}

		// Token: 0x17000D4B RID: 3403
		// (get) Token: 0x06003B30 RID: 15152 RVA: 0x000CBF7B File Offset: 0x000CA17B
		public override long MaximumConnections
		{
			get
			{
				return (long)((ulong)this.info.MaxConn);
			}
		}

		// Token: 0x17000D4C RID: 3404
		// (get) Token: 0x06003B31 RID: 15153 RVA: 0x000CBF89 File Offset: 0x000CA189
		public override long MaximumTransmissionTimeout
		{
			get
			{
				return (long)((ulong)this.info.RtoMax);
			}
		}

		// Token: 0x17000D4D RID: 3405
		// (get) Token: 0x06003B32 RID: 15154 RVA: 0x000CBF97 File Offset: 0x000CA197
		public override long MinimumTransmissionTimeout
		{
			get
			{
				return (long)((ulong)this.info.RtoMin);
			}
		}

		// Token: 0x17000D4E RID: 3406
		// (get) Token: 0x06003B33 RID: 15155 RVA: 0x000CBFA5 File Offset: 0x000CA1A5
		public override long ResetConnections
		{
			get
			{
				return (long)((ulong)this.info.EstabResets);
			}
		}

		// Token: 0x17000D4F RID: 3407
		// (get) Token: 0x06003B34 RID: 15156 RVA: 0x000CBFB3 File Offset: 0x000CA1B3
		public override long ResetsSent
		{
			get
			{
				return (long)((ulong)this.info.OutRsts);
			}
		}

		// Token: 0x17000D50 RID: 3408
		// (get) Token: 0x06003B35 RID: 15157 RVA: 0x000CBFC1 File Offset: 0x000CA1C1
		public override long SegmentsReceived
		{
			get
			{
				return (long)((ulong)this.info.InSegs);
			}
		}

		// Token: 0x17000D51 RID: 3409
		// (get) Token: 0x06003B36 RID: 15158 RVA: 0x000CBFCF File Offset: 0x000CA1CF
		public override long SegmentsResent
		{
			get
			{
				return (long)((ulong)this.info.RetransSegs);
			}
		}

		// Token: 0x17000D52 RID: 3410
		// (get) Token: 0x06003B37 RID: 15159 RVA: 0x000CBFDD File Offset: 0x000CA1DD
		public override long SegmentsSent
		{
			get
			{
				return (long)((ulong)this.info.OutSegs);
			}
		}

		// Token: 0x0400234B RID: 9035
		private Win32_MIB_TCPSTATS info;
	}
}
