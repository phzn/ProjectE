using System;
using MoreMountains.Feedbacks;
using MoreMountains.Tools;
using UnityEngine;

namespace MoreMountains.FeedbacksForThirdParty
{
	// Token: 0x020002FC RID: 764
	[AddComponentMenu("More Mountains/Feedbacks/Shakers/PostProcessing/MMBloomShaker_HDRP")]
	public class MMBloomShaker_HDRP : MMShaker
	{
		// Token: 0x040018BA RID: 6330
		public bool RelativeValues = true;

		// Token: 0x040018BB RID: 6331
		[MMInspectorGroup("Bloom Intensity", true, 42, false)]
		[Tooltip("the curve used to animate the intensity value on")]
		public AnimationCurve ShakeIntensity = new AnimationCurve(new Keyframe[]
		{
			new Keyframe(0f, 0f),
			new Keyframe(0.5f, 1f),
			new Keyframe(1f, 0f)
		});

		// Token: 0x040018BC RID: 6332
		[Tooltip("the value to remap the curve's 0 to")]
		public float RemapIntensityZero;

		// Token: 0x040018BD RID: 6333
		[Tooltip("the value to remap the curve's 1 to")]
		public float RemapIntensityOne = 1f;

		// Token: 0x040018BE RID: 6334
		[MMInspectorGroup("Bloom Threshold", true, 43, false)]
		[Tooltip("the curve used to animate the threshold value on")]
		public AnimationCurve ShakeThreshold = new AnimationCurve(new Keyframe[]
		{
			new Keyframe(0f, 0f),
			new Keyframe(0.5f, 1f),
			new Keyframe(1f, 0f)
		});

		// Token: 0x040018BF RID: 6335
		[Tooltip("the value to remap the curve's 0 to")]
		public float RemapThresholdZero;

		// Token: 0x040018C0 RID: 6336
		[Tooltip("the value to remap the curve's 1 to")]
		public float RemapThresholdOne;
	}
}
