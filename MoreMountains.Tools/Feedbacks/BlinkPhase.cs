using System;

namespace MoreMountains.Feedbacks
{
	// Token: 0x02000236 RID: 566
	[Serializable]
	public class BlinkPhase
	{
		// Token: 0x04001218 RID: 4632
		public float PhaseDuration = 1f;

		// Token: 0x04001219 RID: 4633
		public float OffDuration = 0.2f;

		// Token: 0x0400121A RID: 4634
		public float OnDuration = 0.1f;

		// Token: 0x0400121B RID: 4635
		public float OffLerpDuration = 0.05f;

		// Token: 0x0400121C RID: 4636
		public float OnLerpDuration = 0.05f;
	}
}
