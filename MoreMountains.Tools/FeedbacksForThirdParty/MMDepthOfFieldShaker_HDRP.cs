using System;
using MoreMountains.Feedbacks;
using MoreMountains.Tools;
using UnityEngine;

namespace MoreMountains.FeedbacksForThirdParty
{
	// Token: 0x02000304 RID: 772
	[AddComponentMenu("More Mountains/Feedbacks/Shakers/PostProcessing/MMDepthOfFieldShaker_HDRP")]
	public class MMDepthOfFieldShaker_HDRP : MMShaker
	{
		// Token: 0x040018E4 RID: 6372
		[MMInspectorGroup("Focus Distance", true, 53, false)]
		[Tooltip("whether or not to animate the focus distance")]
		public bool AnimateFocusDistance = true;

		// Token: 0x040018E5 RID: 6373
		[Tooltip("the curve used to animate the focus distance value on")]
		[MMCondition("AnimateFocusDistance", true)]
		public AnimationCurve ShakeFocusDistance = new AnimationCurve(new Keyframe[]
		{
			new Keyframe(0f, 0f),
			new Keyframe(0.5f, 1f),
			new Keyframe(1f, 0f)
		});

		// Token: 0x040018E6 RID: 6374
		[Tooltip("the value to remap the curve's 0 to")]
		[MMCondition("AnimateFocusDistance", true)]
		public float RemapFocusDistanceZero;

		// Token: 0x040018E7 RID: 6375
		[Tooltip("the value to remap the curve's 1 to")]
		[MMCondition("AnimateFocusDistance", true)]
		public float RemapFocusDistanceOne = 3f;

		// Token: 0x040018E8 RID: 6376
		[MMInspectorGroup("Near Range", true, 52, false)]
		[Header("Near Range Start")]
		[Tooltip("whether or not to animate the near range start")]
		public bool AnimateNearRangeStart;

		// Token: 0x040018E9 RID: 6377
		[Tooltip("the curve used to animate the near range start on")]
		[MMCondition("AnimateNearRangeStart", true)]
		public AnimationCurve ShakeNearRangeStart = new AnimationCurve(new Keyframe[]
		{
			new Keyframe(0f, 0f),
			new Keyframe(0.5f, 1f),
			new Keyframe(1f, 0f)
		});

		// Token: 0x040018EA RID: 6378
		[Tooltip("the value to remap the curve's 0 to")]
		[MMCondition("AnimateNearRangeStart", true)]
		public float RemapNearRangeStartZero;

		// Token: 0x040018EB RID: 6379
		[Tooltip("the value to remap the curve's 1 to")]
		[MMCondition("AnimateNearRangeStart", true)]
		public float RemapNearRangeStartOne = 3f;

		// Token: 0x040018EC RID: 6380
		[Header("Near Range End")]
		[Tooltip("whether or not to animate the near range end")]
		public bool AnimateNearRangeEnd;

		// Token: 0x040018ED RID: 6381
		[Tooltip("the curve used to animate the near range end on")]
		[MMCondition("AnimateNearRangeEnd", true)]
		public AnimationCurve ShakeNearRangeEnd = new AnimationCurve(new Keyframe[]
		{
			new Keyframe(0f, 0f),
			new Keyframe(0.5f, 1f),
			new Keyframe(1f, 0f)
		});

		// Token: 0x040018EE RID: 6382
		[Tooltip("the value to remap the curve's 0 to")]
		[MMCondition("AnimateNearRangeEnd", true)]
		public float RemapNearRangeEndZero;

		// Token: 0x040018EF RID: 6383
		[Tooltip("the value to remap the curve's 1 to")]
		[MMCondition("AnimateNearRangeEnd", true)]
		public float RemapNearRangeEndOne = 3f;

		// Token: 0x040018F0 RID: 6384
		[MMInspectorGroup("Far Range", true, 51, false)]
		[Header("Far Range Start")]
		[Tooltip("whether or not to animate the far range start")]
		public bool AnimateFarRangeStart;

		// Token: 0x040018F1 RID: 6385
		[Tooltip("the curve used to animate the far range start on")]
		[MMCondition("AnimateFarRangeStart", true)]
		public AnimationCurve ShakeFarRangeStart = new AnimationCurve(new Keyframe[]
		{
			new Keyframe(0f, 0f),
			new Keyframe(0.5f, 1f),
			new Keyframe(1f, 0f)
		});

		// Token: 0x040018F2 RID: 6386
		[Tooltip("the value to remap the curve's 0 to")]
		[MMCondition("AnimateFarRangeStart", true)]
		public float RemapFarRangeStartZero;

		// Token: 0x040018F3 RID: 6387
		[Tooltip("the value to remap the curve's 1 to")]
		[MMCondition("AnimateFarRangeStart", true)]
		public float RemapFarRangeStartOne = 3f;

		// Token: 0x040018F4 RID: 6388
		[Header("Far Range End")]
		[Tooltip("whether or not to animate the far range end")]
		public bool AnimateFarRangeEnd;

		// Token: 0x040018F5 RID: 6389
		[Tooltip("the curve used to animate the far range end on")]
		[MMCondition("AnimateFarRangeEnd", true)]
		public AnimationCurve ShakeFarRangeEnd = new AnimationCurve(new Keyframe[]
		{
			new Keyframe(0f, 0f),
			new Keyframe(0.5f, 1f),
			new Keyframe(1f, 0f)
		});

		// Token: 0x040018F6 RID: 6390
		[Tooltip("the value to remap the curve's 0 to")]
		[MMCondition("AnimateFarRangeEnd", true)]
		public float RemapFarRangeEndZero;

		// Token: 0x040018F7 RID: 6391
		[Tooltip("the value to remap the curve's 1 to")]
		[MMCondition("AnimateFarRangeEnd", true)]
		public float RemapFarRangeEndOne = 3f;
	}
}
