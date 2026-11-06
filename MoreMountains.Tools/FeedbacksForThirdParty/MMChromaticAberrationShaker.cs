using System;
using MoreMountains.Feedbacks;
using MoreMountains.Tools;
using UnityEngine;

namespace MoreMountains.FeedbacksForThirdParty
{
	// Token: 0x02000327 RID: 807
	[AddComponentMenu("More Mountains/Feedbacks/Shakers/PostProcessing/MMChromaticAberrationShaker")]
	public class MMChromaticAberrationShaker : MMShaker
	{
		// Token: 0x040019C0 RID: 6592
		[MMInspectorGroup("Chromatic Aberration Intensity", true, 46, false)]
		[Tooltip("whether or not to add to the initial value")]
		public bool RelativeIntensity;

		// Token: 0x040019C1 RID: 6593
		[Tooltip("the curve used to animate the intensity value on")]
		public AnimationCurve ShakeIntensity = new AnimationCurve(new Keyframe[]
		{
			new Keyframe(0f, 0f),
			new Keyframe(0.5f, 1f),
			new Keyframe(1f, 0f)
		});

		// Token: 0x040019C2 RID: 6594
		[Tooltip("the value to remap the curve's 0 to")]
		[Range(0f, 1f)]
		public float RemapIntensityZero;

		// Token: 0x040019C3 RID: 6595
		[Tooltip("the value to remap the curve's 1 to")]
		[Range(0f, 1f)]
		public float RemapIntensityOne = 1f;
	}
}
