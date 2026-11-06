using System;
using MoreMountains.Feedbacks;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

namespace MoreMountains.FeedbacksForThirdParty
{
	// Token: 0x020002F3 RID: 755
	[AddComponentMenu("")]
	[MovedFrom(false, null, "MoreMountains.Feedbacks.HDRP", null)]
	[FeedbackHelp("This feedback allows you to control HDRP Depth of Field focus distance or near/far ranges over time.It requires you have in your scene an object with a Volume with Depth of Field active, and a MMDepthOfFieldShaker_HDRP component.")]
	public class MMF_DepthOfField_HDRP : MMF_Feedback
	{
		// Token: 0x17000269 RID: 617
		// (get) Token: 0x06001547 RID: 5447 RVA: 0x0005ACA5 File Offset: 0x00058EA5
		// (set) Token: 0x06001548 RID: 5448 RVA: 0x0005ACB3 File Offset: 0x00058EB3
		public override float FeedbackDuration
		{
			get
			{
				return this.ApplyTimeMultiplier(this.Duration);
			}
			set
			{
				this.Duration = value;
			}
		}

		// Token: 0x1700026A RID: 618
		// (get) Token: 0x06001549 RID: 5449 RVA: 0x0005ACBC File Offset: 0x00058EBC
		public override bool HasChannel
		{
			get
			{
				return true;
			}
		}

		// Token: 0x1700026B RID: 619
		// (get) Token: 0x0600154A RID: 5450 RVA: 0x0005ACBF File Offset: 0x00058EBF
		public override bool HasRandomness
		{
			get
			{
				return true;
			}
		}

		// Token: 0x0600154B RID: 5451 RVA: 0x0005ACC4 File Offset: 0x00058EC4
		protected override void CustomPlayFeedback(Vector3 position, float feedbacksIntensity = 1f)
		{
			if (!this.Active || !MMF_DepthOfField_HDRP.FeedbackTypeAuthorized)
			{
				return;
			}
			float num = this.ComputeIntensity(feedbacksIntensity, position);
			MMDepthOfFieldShakeEvent_HDRP.Trigger(this.Duration, num, this.ChannelData, this.ResetShakerValuesAfterShake, this.ResetTargetValuesAfterShake, this.NormalPlayDirection, this.ComputedTimescaleMode, false, false, this.AnimateFocusDistance, this.ShakeFocusDistance, this.RemapFocusDistanceZero, this.RemapFocusDistanceOne, this.AnimateNearRangeStart, this.ShakeNearRangeStart, this.RemapNearRangeStartZero, this.RemapNearRangeStartOne, this.AnimateNearRangeEnd, this.ShakeNearRangeEnd, this.RemapNearRangeEndZero, this.RemapNearRangeEndOne, this.AnimateFarRangeStart, this.ShakeFarRangeStart, this.RemapFarRangeStartZero, this.RemapFarRangeStartOne, this.AnimateFarRangeEnd, this.ShakeFarRangeEnd, this.RemapFarRangeEndZero, this.RemapFarRangeEndOne);
		}

		// Token: 0x0600154C RID: 5452 RVA: 0x0005AD90 File Offset: 0x00058F90
		protected override void CustomStopFeedback(Vector3 position, float feedbacksIntensity = 1f)
		{
			if (!this.Active || !MMF_DepthOfField_HDRP.FeedbackTypeAuthorized)
			{
				return;
			}
			base.CustomStopFeedback(position, feedbacksIntensity);
			MMDepthOfFieldShakeEvent_HDRP.Trigger(this.Duration, 1f, this.ChannelData, true, true, true, TimescaleModes.Scaled, true, false, false, null, 0f, 1f, false, null, 0f, 0f, false, null, 0f, 0f, false, null, 0f, 0f, false, null, 0f, 0f);
		}

		// Token: 0x0600154D RID: 5453 RVA: 0x0005AE10 File Offset: 0x00059010
		protected override void CustomRestoreInitialValues()
		{
			if (!this.Active || !MMF_DepthOfField_HDRP.FeedbackTypeAuthorized)
			{
				return;
			}
			MMDepthOfFieldShakeEvent_HDRP.Trigger(this.Duration, 1f, this.ChannelData, true, true, true, TimescaleModes.Scaled, false, true, false, null, 0f, 1f, false, null, 0f, 0f, false, null, 0f, 0f, false, null, 0f, 0f, false, null, 0f, 0f);
		}

		// Token: 0x0600154E RID: 5454 RVA: 0x0005AE85 File Offset: 0x00059085
		public override void AutomaticShakerSetup()
		{
		}

		// Token: 0x04001862 RID: 6242
		public static bool FeedbackTypeAuthorized = true;

		// Token: 0x04001863 RID: 6243
		[MMFInspectorGroup("Depth of Field", true, 28, false, false)]
		[Tooltip("the duration of the shake, in seconds")]
		public float Duration = 0.2f;

		// Token: 0x04001864 RID: 6244
		[Tooltip("whether or not to reset shaker values after shake")]
		public bool ResetShakerValuesAfterShake = true;

		// Token: 0x04001865 RID: 6245
		[Tooltip("whether or not to reset the target's values after shake")]
		public bool ResetTargetValuesAfterShake = true;

		// Token: 0x04001866 RID: 6246
		[MMFInspectorGroup("Focus Distance", true, 53, false, false)]
		[Tooltip("whether or not to animate the focus distance")]
		public bool AnimateFocusDistance = true;

		// Token: 0x04001867 RID: 6247
		[Tooltip("the curve used to animate the focus distance value on")]
		[MMFCondition("AnimateFocusDistance", true)]
		public AnimationCurve ShakeFocusDistance = new AnimationCurve(new Keyframe[]
		{
			new Keyframe(0f, 0f),
			new Keyframe(0.5f, 1f),
			new Keyframe(1f, 0f)
		});

		// Token: 0x04001868 RID: 6248
		[Tooltip("the value to remap the curve's 0 to")]
		[MMFCondition("AnimateFocusDistance", true)]
		public float RemapFocusDistanceZero;

		// Token: 0x04001869 RID: 6249
		[Tooltip("the value to remap the curve's 1 to")]
		[MMFCondition("AnimateFocusDistance", true)]
		public float RemapFocusDistanceOne = 3f;

		// Token: 0x0400186A RID: 6250
		[MMFInspectorGroup("Near Range", true, 52, false, false)]
		[Header("Near Range Start")]
		[Tooltip("whether or not to animate the near range start")]
		public bool AnimateNearRangeStart;

		// Token: 0x0400186B RID: 6251
		[Tooltip("the curve used to animate the near range start on")]
		[MMFCondition("AnimateNearRangeStart", true)]
		public AnimationCurve ShakeNearRangeStart = new AnimationCurve(new Keyframe[]
		{
			new Keyframe(0f, 0f),
			new Keyframe(0.5f, 1f),
			new Keyframe(1f, 0f)
		});

		// Token: 0x0400186C RID: 6252
		[Tooltip("the value to remap the curve's 0 to")]
		[MMFCondition("AnimateNearRangeStart", true)]
		public float RemapNearRangeStartZero;

		// Token: 0x0400186D RID: 6253
		[Tooltip("the value to remap the curve's 1 to")]
		[MMFCondition("AnimateNearRangeStart", true)]
		public float RemapNearRangeStartOne = 3f;

		// Token: 0x0400186E RID: 6254
		[Header("Near Range End")]
		[Tooltip("whether or not to animate the near range end")]
		public bool AnimateNearRangeEnd;

		// Token: 0x0400186F RID: 6255
		[Tooltip("the curve used to animate the near range end on")]
		[MMFCondition("AnimateNearRangeEnd", true)]
		public AnimationCurve ShakeNearRangeEnd = new AnimationCurve(new Keyframe[]
		{
			new Keyframe(0f, 0f),
			new Keyframe(0.5f, 1f),
			new Keyframe(1f, 0f)
		});

		// Token: 0x04001870 RID: 6256
		[Tooltip("the value to remap the curve's 0 to")]
		[MMFCondition("AnimateNearRangeEnd", true)]
		public float RemapNearRangeEndZero;

		// Token: 0x04001871 RID: 6257
		[Tooltip("the value to remap the curve's 1 to")]
		[MMFCondition("AnimateNearRangeEnd", true)]
		public float RemapNearRangeEndOne = 3f;

		// Token: 0x04001872 RID: 6258
		[MMFInspectorGroup("Far Range", true, 51, false, false)]
		[Header("Far Range Start")]
		[Tooltip("whether or not to animate the far range start")]
		public bool AnimateFarRangeStart;

		// Token: 0x04001873 RID: 6259
		[Tooltip("the curve used to animate the far range start on")]
		[MMFCondition("AnimateFarRangeStart", true)]
		public AnimationCurve ShakeFarRangeStart = new AnimationCurve(new Keyframe[]
		{
			new Keyframe(0f, 0f),
			new Keyframe(0.5f, 1f),
			new Keyframe(1f, 0f)
		});

		// Token: 0x04001874 RID: 6260
		[Tooltip("the value to remap the curve's 0 to")]
		[MMFCondition("AnimateFarRangeStart", true)]
		public float RemapFarRangeStartZero;

		// Token: 0x04001875 RID: 6261
		[Tooltip("the value to remap the curve's 1 to")]
		[MMFCondition("AnimateFarRangeStart", true)]
		public float RemapFarRangeStartOne = 3f;

		// Token: 0x04001876 RID: 6262
		[Header("Far Range End")]
		[Tooltip("whether or not to animate the far range end")]
		public bool AnimateFarRangeEnd;

		// Token: 0x04001877 RID: 6263
		[Tooltip("the curve used to animate the far range end on")]
		[MMFCondition("AnimateFarRangeEnd", true)]
		public AnimationCurve ShakeFarRangeEnd = new AnimationCurve(new Keyframe[]
		{
			new Keyframe(0f, 0f),
			new Keyframe(0.5f, 1f),
			new Keyframe(1f, 0f)
		});

		// Token: 0x04001878 RID: 6264
		[Tooltip("the value to remap the curve's 0 to")]
		[MMFCondition("AnimateFarRangeEnd", true)]
		public float RemapFarRangeEndZero;

		// Token: 0x04001879 RID: 6265
		[Tooltip("the value to remap the curve's 1 to")]
		[MMFCondition("AnimateFarRangeEnd", true)]
		public float RemapFarRangeEndOne = 3f;
	}
}
