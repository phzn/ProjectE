using System;
using MoreMountains.Tools;

namespace MoreMountains.Feedbacks
{
	// Token: 0x020002DA RID: 730
	public struct TimeScaleProperties
	{
		// Token: 0x06001474 RID: 5236 RVA: 0x00057B34 File Offset: 0x00055D34
		public override string ToString()
		{
			return string.Format("REQUESTED ts={0} time={1} lerp={2} speed={3} keep={4}", new object[] { this.TimeScale, this.Duration, this.TimeScaleLerp, this.LerpSpeed, this.Infinite });
		}

		// Token: 0x0400176A RID: 5994
		public float TimeScale;

		// Token: 0x0400176B RID: 5995
		public float Duration;

		// Token: 0x0400176C RID: 5996
		public bool TimeScaleLerp;

		// Token: 0x0400176D RID: 5997
		public float LerpSpeed;

		// Token: 0x0400176E RID: 5998
		public bool Infinite;

		// Token: 0x0400176F RID: 5999
		public MMTimeScaleLerpModes TimeScaleLerpMode;

		// Token: 0x04001770 RID: 6000
		public MMTweenType TimeScaleLerpCurve;

		// Token: 0x04001771 RID: 6001
		public float TimeScaleLerpDuration;

		// Token: 0x04001772 RID: 6002
		public bool TimeScaleLerpOnReset;

		// Token: 0x04001773 RID: 6003
		public MMTweenType TimeScaleLerpCurveOnReset;

		// Token: 0x04001774 RID: 6004
		public float TimeScaleLerpDurationOnReset;
	}
}
