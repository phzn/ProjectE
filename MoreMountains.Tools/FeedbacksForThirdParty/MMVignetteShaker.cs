using System;
using MoreMountains.Feedbacks;
using MoreMountains.Tools;
using UnityEngine;

namespace MoreMountains.FeedbacksForThirdParty
{
	// Token: 0x0200032F RID: 815
	[AddComponentMenu("More Mountains/Feedbacks/Shakers/PostProcessing/MMVignetteShaker")]
	public class MMVignetteShaker : MMShaker
	{
		// Token: 0x040019E5 RID: 6629
		[MMInspectorGroup("Vignette Intensity", true, 53, false)]
		[Tooltip("whether or not to add to the initial value")]
		public bool RelativeIntensity = true;

		// Token: 0x040019E6 RID: 6630
		[Tooltip("the curve used to animate the intensity value on")]
		public AnimationCurve ShakeIntensity = new AnimationCurve(new Keyframe[]
		{
			new Keyframe(0f, 0f),
			new Keyframe(0.5f, 1f),
			new Keyframe(1f, 0f)
		});

		// Token: 0x040019E7 RID: 6631
		[Tooltip("the value to remap the curve's 0 to")]
		[Range(0f, 1f)]
		public float RemapIntensityZero;

		// Token: 0x040019E8 RID: 6632
		[Tooltip("the value to remap the curve's 1 to")]
		[Range(0f, 1f)]
		public float RemapIntensityOne = 0.1f;

		// Token: 0x040019E9 RID: 6633
		[MMInspectorGroup("Vignette Color", true, 51, false)]
		[Tooltip("whether or not to also animate the vignette's color")]
		public bool InterpolateColor;

		// Token: 0x040019EA RID: 6634
		[Tooltip("the curve to animate the color on")]
		public AnimationCurve ColorCurve = new AnimationCurve(new Keyframe[]
		{
			new Keyframe(0f, 0f),
			new Keyframe(0.05f, 1f),
			new Keyframe(0.95f, 1f),
			new Keyframe(1f, 0f)
		});

		// Token: 0x040019EB RID: 6635
		[Tooltip("the value to remap the curve's 0 to")]
		[Range(0f, 1f)]
		public float RemapColorZero;

		// Token: 0x040019EC RID: 6636
		[Tooltip("the value to remap the curve's 1 to")]
		[Range(0f, 1f)]
		public float RemapColorOne = 1f;

		// Token: 0x040019ED RID: 6637
		[Tooltip("the color to lerp towards")]
		public Color TargetColor = Color.red;
	}
}
