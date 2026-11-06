using System;
using MoreMountains.Feedbacks;
using MoreMountains.Tools;
using UnityEngine;

namespace MoreMountains.FeedbacksForThirdParty
{
	// Token: 0x0200032B RID: 811
	[AddComponentMenu("More Mountains/Feedbacks/Shakers/PostProcessing/MMDepthOfFieldShaker")]
	public class MMDepthOfFieldShaker : MMShaker
	{
		// Token: 0x040019D5 RID: 6613
		public bool RelativeValues = true;

		// Token: 0x040019D6 RID: 6614
		[MMInspectorGroup("Focus Distance", true, 49, false)]
		[Tooltip("the curve used to animate the focus distance value on")]
		public AnimationCurve ShakeFocusDistance = new AnimationCurve(new Keyframe[]
		{
			new Keyframe(0f, 0f),
			new Keyframe(0.5f, 1f),
			new Keyframe(1f, 0f)
		});

		// Token: 0x040019D7 RID: 6615
		[Tooltip("the value to remap the curve's 0 to")]
		public float RemapFocusDistanceZero;

		// Token: 0x040019D8 RID: 6616
		[Tooltip("the value to remap the curve's 1 to")]
		public float RemapFocusDistanceOne = 3f;

		// Token: 0x040019D9 RID: 6617
		[MMInspectorGroup("Aperture", true, 50, false)]
		[Tooltip("the curve used to animate the aperture value on")]
		public AnimationCurve ShakeAperture = new AnimationCurve(new Keyframe[]
		{
			new Keyframe(0f, 0f),
			new Keyframe(0.5f, 1f),
			new Keyframe(1f, 0f)
		});

		// Token: 0x040019DA RID: 6618
		[Tooltip("the value to remap the curve's 0 to")]
		[Range(0.1f, 32f)]
		public float RemapApertureZero;

		// Token: 0x040019DB RID: 6619
		[Tooltip("the value to remap the curve's 1 to")]
		[Range(0.1f, 32f)]
		public float RemapApertureOne;

		// Token: 0x040019DC RID: 6620
		[MMInspectorGroup("Focal Length", true, 51, false)]
		[Tooltip("the curve used to animate the focal length value on")]
		public AnimationCurve ShakeFocalLength = new AnimationCurve(new Keyframe[]
		{
			new Keyframe(0f, 0f),
			new Keyframe(0.5f, 1f),
			new Keyframe(1f, 0f)
		});

		// Token: 0x040019DD RID: 6621
		[Tooltip("the value to remap the curve's 0 to")]
		[Range(0f, 300f)]
		public float RemapFocalLengthZero;

		// Token: 0x040019DE RID: 6622
		[Tooltip("the value to remap the curve's 1 to")]
		[Range(0f, 300f)]
		public float RemapFocalLengthOne;
	}
}
