using System;
using MoreMountains.Feedbacks;
using MoreMountains.Tools;
using UnityEngine;

namespace MoreMountains.FeedbacksForThirdParty
{
	// Token: 0x02000300 RID: 768
	[AddComponentMenu("More Mountains/Feedbacks/Shakers/PostProcessing/MMChromaticAberrationShaker_HDRP")]
	public class MMChromaticAberrationShaker_HDRP : MMShaker
	{
		// Token: 0x040018CD RID: 6349
		[MMInspectorGroup("Chromatic Aberration Intensity", true, 44, false)]
		[Tooltip("whether or not to add to the initial value")]
		public bool RelativeIntensity;

		// Token: 0x040018CE RID: 6350
		[Tooltip("the curve used to animate the intensity value on")]
		public AnimationCurve ShakeIntensity = new AnimationCurve(new Keyframe[]
		{
			new Keyframe(0f, 0f),
			new Keyframe(0.5f, 1f),
			new Keyframe(1f, 0f)
		});

		// Token: 0x040018CF RID: 6351
		[Tooltip("the value to remap the curve's 0 to")]
		[Range(0f, 1f)]
		public float RemapIntensityZero;

		// Token: 0x040018D0 RID: 6352
		[Tooltip("the value to remap the curve's 1 to")]
		[Range(0f, 1f)]
		public float RemapIntensityOne = 1f;
	}
}
