using System;
using MoreMountains.Feedbacks;
using MoreMountains.Tools;
using UnityEngine;

namespace MoreMountains.FeedbacksForThirdParty
{
	// Token: 0x02000312 RID: 786
	[AddComponentMenu("More Mountains/Feedbacks/Shakers/PostProcessing/MMWhiteBalanceShaker_HDRP")]
	public class MMWhiteBalanceShaker_HDRP : MMShaker
	{
		// Token: 0x0400191C RID: 6428
		[Tooltip("whether or not to add to the initial value")]
		public bool RelativeValues = true;

		// Token: 0x0400191D RID: 6429
		[MMInspectorGroup("Temperature", true, 47, false)]
		[Tooltip("the curve used to animate the temperature value on")]
		public AnimationCurve ShakeTemperature = new AnimationCurve(new Keyframe[]
		{
			new Keyframe(0f, 0f),
			new Keyframe(0.5f, 1f),
			new Keyframe(1f, 0f)
		});

		// Token: 0x0400191E RID: 6430
		[Tooltip("the value to remap the curve's 0 to")]
		[Range(-100f, 100f)]
		public float RemapTemperatureZero;

		// Token: 0x0400191F RID: 6431
		[Tooltip("the value to remap the curve's 1 to")]
		[Range(-100f, 100f)]
		public float RemapTemperatureOne = 100f;

		// Token: 0x04001920 RID: 6432
		[MMInspectorGroup("Tint", true, 48, false)]
		[Tooltip("the curve used to animate the tint value on")]
		public AnimationCurve ShakeTint = new AnimationCurve(new Keyframe[]
		{
			new Keyframe(0f, 0f),
			new Keyframe(0.5f, 1f),
			new Keyframe(1f, 0f)
		});

		// Token: 0x04001921 RID: 6433
		[Tooltip("the value to remap the curve's 0 to")]
		[Range(-100f, 100f)]
		public float RemapTintZero;

		// Token: 0x04001922 RID: 6434
		[Tooltip("the value to remap the curve's 1 to")]
		[Range(-100f, 100f)]
		public float RemapTintOne = 100f;
	}
}
