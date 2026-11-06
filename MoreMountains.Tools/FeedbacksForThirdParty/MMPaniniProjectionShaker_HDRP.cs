using System;
using MoreMountains.Feedbacks;
using MoreMountains.Tools;
using UnityEngine;

namespace MoreMountains.FeedbacksForThirdParty
{
	// Token: 0x0200030E RID: 782
	[AddComponentMenu("More Mountains/Feedbacks/Shakers/PostProcessing/MMPaniniProjectionShaker_HDRP")]
	public class MMPaniniProjectionShaker_HDRP : MMShaker
	{
		// Token: 0x0400190D RID: 6413
		[MMInspectorGroup("Panini Projection Distance", true, 49, false)]
		[Tooltip("whether or not to add to the initial value")]
		public bool RelativeDistance;

		// Token: 0x0400190E RID: 6414
		[Tooltip("the curve used to animate the distance value on")]
		public AnimationCurve ShakeDistance = new AnimationCurve(new Keyframe[]
		{
			new Keyframe(0f, 0f),
			new Keyframe(0.5f, 1f),
			new Keyframe(1f, 0f)
		});

		// Token: 0x0400190F RID: 6415
		[Tooltip("the value to remap the curve's 0 to")]
		[Range(0f, 1f)]
		public float RemapDistanceZero;

		// Token: 0x04001910 RID: 6416
		[Tooltip("the value to remap the curve's 1 to")]
		[Range(0f, 1f)]
		public float RemapDistanceOne = 1f;
	}
}
