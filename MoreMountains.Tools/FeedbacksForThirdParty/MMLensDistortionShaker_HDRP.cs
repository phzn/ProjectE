using System;
using MoreMountains.Feedbacks;
using MoreMountains.Tools;
using UnityEngine;

namespace MoreMountains.FeedbacksForThirdParty
{
	// Token: 0x0200030A RID: 778
	[AddComponentMenu("More Mountains/Feedbacks/Shakers/PostProcessing/MMLensDistortionShaker_HDRP")]
	public class MMLensDistortionShaker_HDRP : MMShaker
	{
		// Token: 0x04001903 RID: 6403
		[MMInspectorGroup("Lens Distortion Intensity", true, 47, false)]
		[Tooltip("whether or not to add to the initial value")]
		public bool RelativeIntensity;

		// Token: 0x04001904 RID: 6404
		[Tooltip("the curve used to animate the intensity value on")]
		public AnimationCurve ShakeIntensity = new AnimationCurve(new Keyframe[]
		{
			new Keyframe(0f, 0f),
			new Keyframe(0.2f, 1f),
			new Keyframe(0.25f, -1f),
			new Keyframe(0.35f, 0.7f),
			new Keyframe(0.4f, -0.7f),
			new Keyframe(0.6f, 0.3f),
			new Keyframe(0.65f, -0.3f),
			new Keyframe(0.8f, 0.1f),
			new Keyframe(0.85f, -0.1f),
			new Keyframe(1f, 0f)
		});

		// Token: 0x04001905 RID: 6405
		[Tooltip("the value to remap the curve's 0 to")]
		[Range(-100f, 100f)]
		public float RemapIntensityZero;

		// Token: 0x04001906 RID: 6406
		[Tooltip("the value to remap the curve's 1 to")]
		[Range(-100f, 100f)]
		public float RemapIntensityOne = 0.5f;
	}
}
