using System;
using System.Diagnostics;

namespace Pathfinding.Jobs
{
	// Token: 0x02000193 RID: 403
	public struct TimeSlice
	{
		// Token: 0x170001F7 RID: 503
		// (get) Token: 0x06000BFE RID: 3070 RVA: 0x0004216F File Offset: 0x0004036F
		public bool isInfinite
		{
			get
			{
				return this.endTick == long.MaxValue;
			}
		}

		// Token: 0x170001F8 RID: 504
		// (get) Token: 0x06000BFF RID: 3071 RVA: 0x00042182 File Offset: 0x00040382
		public bool expired
		{
			get
			{
				return Stopwatch.GetTimestamp() > this.endTick;
			}
		}

		// Token: 0x06000C00 RID: 3072 RVA: 0x00042194 File Offset: 0x00040394
		public static TimeSlice MillisFromNow(float millis)
		{
			return new TimeSlice
			{
				endTick = Stopwatch.GetTimestamp() + (long)(millis * 10000f)
			};
		}

		// Token: 0x040007E6 RID: 2022
		public long endTick;

		// Token: 0x040007E7 RID: 2023
		public static readonly TimeSlice Infinite = new TimeSlice
		{
			endTick = long.MaxValue
		};
	}
}
