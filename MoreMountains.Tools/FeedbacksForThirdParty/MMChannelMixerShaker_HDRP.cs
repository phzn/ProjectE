using System;
using MoreMountains.Feedbacks;
using MoreMountains.Tools;
using UnityEngine;

namespace MoreMountains.FeedbacksForThirdParty
{
	// Token: 0x020002FE RID: 766
	[AddComponentMenu("More Mountains/Feedbacks/Shakers/PostProcessing/MMChannelMixerShaker_HDRP")]
	public class MMChannelMixerShaker_HDRP : MMShaker
	{
		// Token: 0x040018C2 RID: 6338
		public bool RelativeValues = true;

		// Token: 0x040018C3 RID: 6339
		[MMInspectorGroup("Red", true, 42, false)]
		[Tooltip("the curve used to animate the red value on")]
		public AnimationCurve ShakeRed = new AnimationCurve(new Keyframe[]
		{
			new Keyframe(0f, 0f),
			new Keyframe(0.5f, 1f),
			new Keyframe(1f, 0f)
		});

		// Token: 0x040018C4 RID: 6340
		[Tooltip("the value to remap the curve's 0 to")]
		[Range(-200f, 200f)]
		public float RemapRedZero;

		// Token: 0x040018C5 RID: 6341
		[Tooltip("the value to remap the curve's 1 to")]
		[Range(-200f, 200f)]
		public float RemapRedOne = 200f;

		// Token: 0x040018C6 RID: 6342
		[MMInspectorGroup("Green", true, 43, false)]
		[Tooltip("the curve used to animate the green value on")]
		public AnimationCurve ShakeGreen = new AnimationCurve(new Keyframe[]
		{
			new Keyframe(0f, 0f),
			new Keyframe(0.5f, 1f),
			new Keyframe(1f, 0f)
		});

		// Token: 0x040018C7 RID: 6343
		[Tooltip("the value to remap the curve's 0 to")]
		[Range(-200f, 200f)]
		public float RemapGreenZero;

		// Token: 0x040018C8 RID: 6344
		[Tooltip("the value to remap the curve's 1 to")]
		[Range(-200f, 200f)]
		public float RemapGreenOne = 200f;

		// Token: 0x040018C9 RID: 6345
		[MMInspectorGroup("Blue", true, 44, false)]
		[Tooltip("the curve used to animate the blue value on")]
		public AnimationCurve ShakeBlue = new AnimationCurve(new Keyframe[]
		{
			new Keyframe(0f, 0f),
			new Keyframe(0.5f, 1f),
			new Keyframe(1f, 0f)
		});

		// Token: 0x040018CA RID: 6346
		[Tooltip("the value to remap the curve's 0 to")]
		[Range(-200f, 200f)]
		public float RemapBlueZero;

		// Token: 0x040018CB RID: 6347
		[Tooltip("the value to remap the curve's 1 to")]
		[Range(-200f, 200f)]
		public float RemapBlueOne = 200f;
	}
}
