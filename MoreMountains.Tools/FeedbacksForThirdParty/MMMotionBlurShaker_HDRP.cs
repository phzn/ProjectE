using System;
using MoreMountains.Feedbacks;
using MoreMountains.Tools;
using UnityEngine;

namespace MoreMountains.FeedbacksForThirdParty
{
	// Token: 0x0200030C RID: 780
	[AddComponentMenu("More Mountains/Feedbacks/Shakers/PostProcessing/MMMotionBlurShaker_HDRP")]
	public class MMMotionBlurShaker_HDRP : MMShaker
	{
		// Token: 0x04001908 RID: 6408
		[MMInspectorGroup("Motion Blur Intensity", true, 48, false)]
		[Tooltip("whether or not to add to the initial value")]
		public bool RelativeIntensity;

		// Token: 0x04001909 RID: 6409
		[Tooltip("the curve used to animate the intensity value on")]
		public AnimationCurve ShakeIntensity = new AnimationCurve(new Keyframe[]
		{
			new Keyframe(0f, 0f),
			new Keyframe(0.5f, 1f),
			new Keyframe(1f, 0f)
		});

		// Token: 0x0400190A RID: 6410
		[Tooltip("the value to remap the curve's 0 to")]
		public float RemapIntensityZero;

		// Token: 0x0400190B RID: 6411
		[Tooltip("the value to remap the curve's 1 to")]
		public float RemapIntensityOne = 1000f;
	}
}
