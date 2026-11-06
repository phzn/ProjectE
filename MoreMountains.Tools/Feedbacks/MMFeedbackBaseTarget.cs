using System;
using MoreMountains.Tools;
using UnityEngine;

namespace MoreMountains.Feedbacks
{
	// Token: 0x020002B4 RID: 692
	public class MMFeedbackBaseTarget
	{
		// Token: 0x04001688 RID: 5768
		public MMPropertyReceiver Target;

		// Token: 0x04001689 RID: 5769
		public MMTweenType LevelCurve = new MMTweenType(new AnimationCurve(new Keyframe[]
		{
			new Keyframe(0f, 0f),
			new Keyframe(0.3f, 1f),
			new Keyframe(1f, 0f)
		}));

		// Token: 0x0400168A RID: 5770
		public float RemapLevelZero;

		// Token: 0x0400168B RID: 5771
		public float RemapLevelOne = 1f;

		// Token: 0x0400168C RID: 5772
		public float InstantLevel;

		// Token: 0x0400168D RID: 5773
		public float InitialLevel;
	}
}
