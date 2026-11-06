using System;

namespace UnityEngine.Timeline
{
	// Token: 0x02000022 RID: 34
	internal abstract class RuntimeClipBase : RuntimeElement
	{
		// Token: 0x17000097 RID: 151
		// (get) Token: 0x0600020D RID: 525
		public abstract double start { get; }

		// Token: 0x17000098 RID: 152
		// (get) Token: 0x0600020E RID: 526
		public abstract double duration { get; }

		// Token: 0x17000099 RID: 153
		// (get) Token: 0x0600020F RID: 527 RVA: 0x00008070 File Offset: 0x00006270
		public override long intervalStart
		{
			get
			{
				return DiscreteTime.GetNearestTick(this.start);
			}
		}

		// Token: 0x1700009A RID: 154
		// (get) Token: 0x06000210 RID: 528 RVA: 0x0000807D File Offset: 0x0000627D
		public override long intervalEnd
		{
			get
			{
				return DiscreteTime.GetNearestTick(this.start + this.duration);
			}
		}
	}
}
