using System;
using MoreMountains.Feedbacks;
using MoreMountains.Tools;
using UnityEngine;

namespace MoreMountains.FeedbacksForThirdParty
{
	// Token: 0x02000308 RID: 776
	[AddComponentMenu("More Mountains/Feedbacks/Shakers/PostProcessing/MMFilmGrainShaker_HDRP")]
	public class MMFilmGrainShaker_HDRP : MMShaker
	{
		// Token: 0x040018FE RID: 6398
		[MMInspectorGroup("Film Grain Intensity", true, 47, false)]
		[Tooltip("whether or not to add to the initial value")]
		public bool RelativeIntensity;

		// Token: 0x040018FF RID: 6399
		[Tooltip("the curve used to animate the intensity value on")]
		public AnimationCurve ShakeIntensity = new AnimationCurve(new Keyframe[]
		{
			new Keyframe(0f, 0f),
			new Keyframe(0.5f, 1f),
			new Keyframe(1f, 0f)
		});

		// Token: 0x04001900 RID: 6400
		[Tooltip("the value to remap the curve's 0 to")]
		[Range(0f, 1f)]
		public float RemapIntensityZero;

		// Token: 0x04001901 RID: 6401
		[Tooltip("the value to remap the curve's 1 to")]
		[Range(0f, 1f)]
		public float RemapIntensityOne = 1f;
	}
}
