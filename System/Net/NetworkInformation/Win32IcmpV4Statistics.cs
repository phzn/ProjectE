using System;

namespace System.Net.NetworkInformation
{
	// Token: 0x02000732 RID: 1842
	internal class Win32IcmpV4Statistics : IcmpV4Statistics
	{
		// Token: 0x06003AC7 RID: 15047 RVA: 0x000CB5B8 File Offset: 0x000C97B8
		public Win32IcmpV4Statistics(Win32_MIBICMPINFO info)
		{
			this.iin = info.InStats;
			this.iout = info.OutStats;
		}

		// Token: 0x17000CFC RID: 3324
		// (get) Token: 0x06003AC8 RID: 15048 RVA: 0x000CB5D8 File Offset: 0x000C97D8
		public override long AddressMaskRepliesReceived
		{
			get
			{
				return (long)((ulong)this.iin.AddrMaskReps);
			}
		}

		// Token: 0x17000CFD RID: 3325
		// (get) Token: 0x06003AC9 RID: 15049 RVA: 0x000CB5E6 File Offset: 0x000C97E6
		public override long AddressMaskRepliesSent
		{
			get
			{
				return (long)((ulong)this.iout.AddrMaskReps);
			}
		}

		// Token: 0x17000CFE RID: 3326
		// (get) Token: 0x06003ACA RID: 15050 RVA: 0x000CB5F4 File Offset: 0x000C97F4
		public override long AddressMaskRequestsReceived
		{
			get
			{
				return (long)((ulong)this.iin.AddrMasks);
			}
		}

		// Token: 0x17000CFF RID: 3327
		// (get) Token: 0x06003ACB RID: 15051 RVA: 0x000CB602 File Offset: 0x000C9802
		public override long AddressMaskRequestsSent
		{
			get
			{
				return (long)((ulong)this.iout.AddrMasks);
			}
		}

		// Token: 0x17000D00 RID: 3328
		// (get) Token: 0x06003ACC RID: 15052 RVA: 0x000CB610 File Offset: 0x000C9810
		public override long DestinationUnreachableMessagesReceived
		{
			get
			{
				return (long)((ulong)this.iin.DestUnreachs);
			}
		}

		// Token: 0x17000D01 RID: 3329
		// (get) Token: 0x06003ACD RID: 15053 RVA: 0x000CB61E File Offset: 0x000C981E
		public override long DestinationUnreachableMessagesSent
		{
			get
			{
				return (long)((ulong)this.iout.DestUnreachs);
			}
		}

		// Token: 0x17000D02 RID: 3330
		// (get) Token: 0x06003ACE RID: 15054 RVA: 0x000CB62C File Offset: 0x000C982C
		public override long EchoRepliesReceived
		{
			get
			{
				return (long)((ulong)this.iin.EchoReps);
			}
		}

		// Token: 0x17000D03 RID: 3331
		// (get) Token: 0x06003ACF RID: 15055 RVA: 0x000CB63A File Offset: 0x000C983A
		public override long EchoRepliesSent
		{
			get
			{
				return (long)((ulong)this.iout.EchoReps);
			}
		}

		// Token: 0x17000D04 RID: 3332
		// (get) Token: 0x06003AD0 RID: 15056 RVA: 0x000CB648 File Offset: 0x000C9848
		public override long EchoRequestsReceived
		{
			get
			{
				return (long)((ulong)this.iin.Echos);
			}
		}

		// Token: 0x17000D05 RID: 3333
		// (get) Token: 0x06003AD1 RID: 15057 RVA: 0x000CB656 File Offset: 0x000C9856
		public override long EchoRequestsSent
		{
			get
			{
				return (long)((ulong)this.iout.Echos);
			}
		}

		// Token: 0x17000D06 RID: 3334
		// (get) Token: 0x06003AD2 RID: 15058 RVA: 0x000CB664 File Offset: 0x000C9864
		public override long ErrorsReceived
		{
			get
			{
				return (long)((ulong)this.iin.Errors);
			}
		}

		// Token: 0x17000D07 RID: 3335
		// (get) Token: 0x06003AD3 RID: 15059 RVA: 0x000CB672 File Offset: 0x000C9872
		public override long ErrorsSent
		{
			get
			{
				return (long)((ulong)this.iout.Errors);
			}
		}

		// Token: 0x17000D08 RID: 3336
		// (get) Token: 0x06003AD4 RID: 15060 RVA: 0x000CB680 File Offset: 0x000C9880
		public override long MessagesReceived
		{
			get
			{
				return (long)((ulong)this.iin.Msgs);
			}
		}

		// Token: 0x17000D09 RID: 3337
		// (get) Token: 0x06003AD5 RID: 15061 RVA: 0x000CB68E File Offset: 0x000C988E
		public override long MessagesSent
		{
			get
			{
				return (long)((ulong)this.iout.Msgs);
			}
		}

		// Token: 0x17000D0A RID: 3338
		// (get) Token: 0x06003AD6 RID: 15062 RVA: 0x000CB69C File Offset: 0x000C989C
		public override long ParameterProblemsReceived
		{
			get
			{
				return (long)((ulong)this.iin.ParmProbs);
			}
		}

		// Token: 0x17000D0B RID: 3339
		// (get) Token: 0x06003AD7 RID: 15063 RVA: 0x000CB6AA File Offset: 0x000C98AA
		public override long ParameterProblemsSent
		{
			get
			{
				return (long)((ulong)this.iout.ParmProbs);
			}
		}

		// Token: 0x17000D0C RID: 3340
		// (get) Token: 0x06003AD8 RID: 15064 RVA: 0x000CB6B8 File Offset: 0x000C98B8
		public override long RedirectsReceived
		{
			get
			{
				return (long)((ulong)this.iin.Redirects);
			}
		}

		// Token: 0x17000D0D RID: 3341
		// (get) Token: 0x06003AD9 RID: 15065 RVA: 0x000CB6C6 File Offset: 0x000C98C6
		public override long RedirectsSent
		{
			get
			{
				return (long)((ulong)this.iout.Redirects);
			}
		}

		// Token: 0x17000D0E RID: 3342
		// (get) Token: 0x06003ADA RID: 15066 RVA: 0x000CB6D4 File Offset: 0x000C98D4
		public override long SourceQuenchesReceived
		{
			get
			{
				return (long)((ulong)this.iin.SrcQuenchs);
			}
		}

		// Token: 0x17000D0F RID: 3343
		// (get) Token: 0x06003ADB RID: 15067 RVA: 0x000CB6E2 File Offset: 0x000C98E2
		public override long SourceQuenchesSent
		{
			get
			{
				return (long)((ulong)this.iout.SrcQuenchs);
			}
		}

		// Token: 0x17000D10 RID: 3344
		// (get) Token: 0x06003ADC RID: 15068 RVA: 0x000CB6F0 File Offset: 0x000C98F0
		public override long TimeExceededMessagesReceived
		{
			get
			{
				return (long)((ulong)this.iin.TimeExcds);
			}
		}

		// Token: 0x17000D11 RID: 3345
		// (get) Token: 0x06003ADD RID: 15069 RVA: 0x000CB6FE File Offset: 0x000C98FE
		public override long TimeExceededMessagesSent
		{
			get
			{
				return (long)((ulong)this.iout.TimeExcds);
			}
		}

		// Token: 0x17000D12 RID: 3346
		// (get) Token: 0x06003ADE RID: 15070 RVA: 0x000CB70C File Offset: 0x000C990C
		public override long TimestampRepliesReceived
		{
			get
			{
				return (long)((ulong)this.iin.TimestampReps);
			}
		}

		// Token: 0x17000D13 RID: 3347
		// (get) Token: 0x06003ADF RID: 15071 RVA: 0x000CB71A File Offset: 0x000C991A
		public override long TimestampRepliesSent
		{
			get
			{
				return (long)((ulong)this.iout.TimestampReps);
			}
		}

		// Token: 0x17000D14 RID: 3348
		// (get) Token: 0x06003AE0 RID: 15072 RVA: 0x000CB728 File Offset: 0x000C9928
		public override long TimestampRequestsReceived
		{
			get
			{
				return (long)((ulong)this.iin.Timestamps);
			}
		}

		// Token: 0x17000D15 RID: 3349
		// (get) Token: 0x06003AE1 RID: 15073 RVA: 0x000CB736 File Offset: 0x000C9936
		public override long TimestampRequestsSent
		{
			get
			{
				return (long)((ulong)this.iout.Timestamps);
			}
		}

		// Token: 0x0400227F RID: 8831
		private Win32_MIBICMPSTATS iin;

		// Token: 0x04002280 RID: 8832
		private Win32_MIBICMPSTATS iout;
	}
}
