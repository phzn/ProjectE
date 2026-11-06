using System;
using UnityEngine.Playables;

namespace UnityEngine.Timeline
{
	// Token: 0x02000023 RID: 35
	internal abstract class RuntimeElement : IInterval
	{
		// Token: 0x1700009B RID: 155
		// (get) Token: 0x06000212 RID: 530
		public abstract long intervalStart { get; }

		// Token: 0x1700009C RID: 156
		// (get) Token: 0x06000213 RID: 531
		public abstract long intervalEnd { get; }

		// Token: 0x1700009D RID: 157
		// (get) Token: 0x06000214 RID: 532 RVA: 0x00008099 File Offset: 0x00006299
		// (set) Token: 0x06000215 RID: 533 RVA: 0x000080A1 File Offset: 0x000062A1
		public int intervalBit { get; set; }

		// Token: 0x1700009E RID: 158
		// (set) Token: 0x06000216 RID: 534
		public abstract bool enable { set; }

		// Token: 0x06000217 RID: 535
		public abstract void EvaluateAt(double localTime, FrameData frameData);

		// Token: 0x06000218 RID: 536
		public abstract void DisableAt(double localTime, double rootDuration, FrameData frameData);
	}
}
