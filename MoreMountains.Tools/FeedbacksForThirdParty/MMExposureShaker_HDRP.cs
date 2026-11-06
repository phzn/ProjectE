using System;
using MoreMountains.Feedbacks;
using MoreMountains.Tools;
using UnityEngine;

namespace MoreMountains.FeedbacksForThirdParty
{
	// Token: 0x02000306 RID: 774
	[AddComponentMenu("More Mountains/Feedbacks/Shakers/PostProcessing/MMExposureShaker_HDRP")]
	public class MMExposureShaker_HDRP : MMShaker
	{
		// Token: 0x040018F9 RID: 6393
		[MMInspectorGroup("Exposure Intensity", true, 46, false)]
		[Tooltip("whether or not to add to the initial value")]
		public bool RelativeIntensity;

		// Token: 0x040018FA RID: 6394
		[Tooltip("the curve used to animate the intensity value on")]
		public AnimationCurve ShakeFixedExposure = new AnimationCurve(new Keyframe[]
		{
			new Keyframe(0f, 0f),
			new Keyframe(0.5f, 1f),
			new Keyframe(1f, 0f)
		});

		// Token: 0x040018FB RID: 6395
		[Tooltip("the value to remap the curve's 0 to")]
		public float RemapFixedExposureZero = 8.5f;

		// Token: 0x040018FC RID: 6396
		[Tooltip("the value to remap the curve's 1 to")]
		public float RemapFixedExposureOne = 6f;
	}
}
