using System;
using MoreMountains.Feedbacks;
using MoreMountains.Tools;
using UnityEngine;

namespace MoreMountains.FeedbacksForThirdParty
{
	// Token: 0x02000325 RID: 805
	[AddComponentMenu("More Mountains/Feedbacks/Shakers/PostProcessing/MMBloomShaker")]
	public class MMBloomShaker : MMShaker
	{
		// Token: 0x040019B8 RID: 6584
		public bool RelativeValues = true;

		// Token: 0x040019B9 RID: 6585
		[MMInspectorGroup("Bloom Intensity", true, 45, false)]
		[Tooltip("the curve used to animate the intensity value on")]
		public AnimationCurve ShakeIntensity = new AnimationCurve(new Keyframe[]
		{
			new Keyframe(0f, 0f),
			new Keyframe(0.5f, 1f),
			new Keyframe(1f, 0f)
		});

		// Token: 0x040019BA RID: 6586
		[Tooltip("the value to remap the curve's 0 to")]
		public float RemapIntensityZero;

		// Token: 0x040019BB RID: 6587
		[Tooltip("the value to remap the curve's 1 to")]
		public float RemapIntensityOne = 10f;

		// Token: 0x040019BC RID: 6588
		[MMInspectorGroup("Bloom Threshold", true, 46, false)]
		[Tooltip("the curve used to animate the threshold value on")]
		public AnimationCurve ShakeThreshold = new AnimationCurve(new Keyframe[]
		{
			new Keyframe(0f, 0f),
			new Keyframe(0.5f, 1f),
			new Keyframe(1f, 0f)
		});

		// Token: 0x040019BD RID: 6589
		[Tooltip("the value to remap the curve's 0 to")]
		public float RemapThresholdZero;

		// Token: 0x040019BE RID: 6590
		[Tooltip("the value to remap the curve's 1 to")]
		public float RemapThresholdOne;
	}
}
