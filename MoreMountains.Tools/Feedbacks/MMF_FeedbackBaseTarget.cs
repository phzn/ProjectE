using System;
using MoreMountains.Tools;
using UnityEngine;

namespace MoreMountains.Feedbacks
{
	// Token: 0x020002B6 RID: 694
	public class MMF_FeedbackBaseTarget
	{
		// Token: 0x04001697 RID: 5783
		public MMPropertyReceiver Target;

		// Token: 0x04001698 RID: 5784
		public MMTweenType LevelCurve = new MMTweenType(new AnimationCurve(new Keyframe[]
		{
			new Keyframe(0f, 0f),
			new Keyframe(0.3f, 1f),
			new Keyframe(1f, 0f)
		}));

		// Token: 0x04001699 RID: 5785
		public float RemapLevelZero;

		// Token: 0x0400169A RID: 5786
		public float RemapLevelOne = 1f;

		// Token: 0x0400169B RID: 5787
		public float InstantLevel;

		// Token: 0x0400169C RID: 5788
		public float InitialLevel;
	}
}
