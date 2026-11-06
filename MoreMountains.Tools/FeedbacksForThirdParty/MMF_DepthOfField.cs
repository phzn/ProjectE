using System;
using MoreMountains.Feedbacks;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

namespace MoreMountains.FeedbacksForThirdParty
{
	// Token: 0x0200031E RID: 798
	[AddComponentMenu("")]
	[FeedbackHelp("This feedback allows you to control depth of field focus distance, aperture and focal length over time. It requires you have in your scene an object with a PostProcessVolume with Depth of Field active, and a MMDepthOfFieldShaker component.")]
	[MovedFrom(false, null, "MoreMountains.Feedbacks.PostProcessing", null)]
	public class MMF_DepthOfField : MMF_Feedback
	{
		// Token: 0x1700028B RID: 651
		// (get) Token: 0x06001624 RID: 5668 RVA: 0x0005DFE3 File Offset: 0x0005C1E3
		// (set) Token: 0x06001625 RID: 5669 RVA: 0x0005DFF1 File Offset: 0x0005C1F1
		public override float FeedbackDuration
		{
			get
			{
				return this.ApplyTimeMultiplier(this.ShakeDuration);
			}
			set
			{
				this.ShakeDuration = value;
			}
		}

		// Token: 0x1700028C RID: 652
		// (get) Token: 0x06001626 RID: 5670 RVA: 0x0005DFFA File Offset: 0x0005C1FA
		public override bool HasChannel
		{
			get
			{
				return true;
			}
		}

		// Token: 0x1700028D RID: 653
		// (get) Token: 0x06001627 RID: 5671 RVA: 0x0005DFFD File Offset: 0x0005C1FD
		public override bool HasRandomness
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06001628 RID: 5672 RVA: 0x0005E000 File Offset: 0x0005C200
		protected override void CustomPlayFeedback(Vector3 position, float feedbacksIntensity = 1f)
		{
			if (!this.Active || !MMF_DepthOfField.FeedbackTypeAuthorized)
			{
				return;
			}
			float num = this.ComputeIntensity(feedbacksIntensity, position);
			MMDepthOfFieldShakeEvent.Trigger(this.ShakeFocusDistance, this.FeedbackDuration, this.RemapFocusDistanceZero, this.RemapFocusDistanceOne, this.ShakeAperture, this.RemapApertureZero, this.RemapApertureOne, this.ShakeFocalLength, this.RemapFocalLengthZero, this.RemapFocalLengthOne, this.RelativeValues, num, this.ChannelData, this.ResetShakerValuesAfterShake, this.ResetTargetValuesAfterShake, this.NormalPlayDirection, this.ComputedTimescaleMode, false, false);
		}

		// Token: 0x06001629 RID: 5673 RVA: 0x0005E090 File Offset: 0x0005C290
		protected override void CustomStopFeedback(Vector3 position, float feedbacksIntensity = 1f)
		{
			if (!this.Active || !MMF_DepthOfField.FeedbackTypeAuthorized)
			{
				return;
			}
			base.CustomStopFeedback(position, feedbacksIntensity);
			MMDepthOfFieldShakeEvent.Trigger(this.ShakeFocusDistance, this.FeedbackDuration, this.RemapFocusDistanceZero, this.RemapFocusDistanceOne, this.ShakeAperture, this.RemapApertureZero, this.RemapApertureOne, this.ShakeFocalLength, this.RemapFocalLengthZero, this.RemapFocalLengthOne, this.RelativeValues, 1f, null, true, true, true, TimescaleModes.Scaled, true, false);
		}

		// Token: 0x0600162A RID: 5674 RVA: 0x0005E108 File Offset: 0x0005C308
		protected override void CustomRestoreInitialValues()
		{
			if (!this.Active || !MMF_DepthOfField.FeedbackTypeAuthorized)
			{
				return;
			}
			MMDepthOfFieldShakeEvent.Trigger(this.ShakeFocusDistance, this.FeedbackDuration, this.RemapFocusDistanceZero, this.RemapFocusDistanceOne, this.ShakeAperture, this.RemapApertureZero, this.RemapApertureOne, this.ShakeFocalLength, this.RemapFocalLengthZero, this.RemapFocalLengthOne, this.RelativeValues, 1f, null, true, true, true, TimescaleModes.Scaled, false, true);
		}

		// Token: 0x0600162B RID: 5675 RVA: 0x0005E178 File Offset: 0x0005C378
		public override void AutomaticShakerSetup()
		{
		}

		// Token: 0x04001976 RID: 6518
		public static bool FeedbackTypeAuthorized = true;

		// Token: 0x04001977 RID: 6519
		[MMFInspectorGroup("Depth Of Field", true, 51, false, false)]
		[Tooltip("the duration of the shake, in seconds")]
		public float ShakeDuration = 2f;

		// Token: 0x04001978 RID: 6520
		[Tooltip("whether or not to add to the initial values")]
		public bool RelativeValues = true;

		// Token: 0x04001979 RID: 6521
		[Tooltip("whether or not to reset shaker values after shake")]
		public bool ResetShakerValuesAfterShake = true;

		// Token: 0x0400197A RID: 6522
		[Tooltip("whether or not to reset the target's values after shake")]
		public bool ResetTargetValuesAfterShake = true;

		// Token: 0x0400197B RID: 6523
		[MMFInspectorGroup("Focus Distance", true, 52, false, false)]
		[Tooltip("the curve used to animate the focus distance value on")]
		public AnimationCurve ShakeFocusDistance = new AnimationCurve(new Keyframe[]
		{
			new Keyframe(0f, 0f),
			new Keyframe(0.5f, 1f),
			new Keyframe(1f, 0f)
		});

		// Token: 0x0400197C RID: 6524
		[Tooltip("the value to remap the curve's 0 to")]
		public float RemapFocusDistanceZero = 4f;

		// Token: 0x0400197D RID: 6525
		[Tooltip("the value to remap the curve's 1 to")]
		public float RemapFocusDistanceOne = 50f;

		// Token: 0x0400197E RID: 6526
		[MMFInspectorGroup("Aperture", true, 53, false, false)]
		[Tooltip("the curve used to animate the aperture value on")]
		public AnimationCurve ShakeAperture = new AnimationCurve(new Keyframe[]
		{
			new Keyframe(0f, 0f),
			new Keyframe(0.5f, 1f),
			new Keyframe(1f, 0f)
		});

		// Token: 0x0400197F RID: 6527
		[Tooltip("the value to remap the curve's 0 to")]
		[Range(0.1f, 32f)]
		public float RemapApertureZero = 0.6f;

		// Token: 0x04001980 RID: 6528
		[Tooltip("the value to remap the curve's 1 to")]
		[Range(0.1f, 32f)]
		public float RemapApertureOne = 0.2f;

		// Token: 0x04001981 RID: 6529
		[MMFInspectorGroup("Focal Length", true, 54, false, false)]
		[Tooltip("the curve used to animate the focal length value on")]
		public AnimationCurve ShakeFocalLength = new AnimationCurve(new Keyframe[]
		{
			new Keyframe(0f, 0f),
			new Keyframe(0.5f, 1f),
			new Keyframe(1f, 0f)
		});

		// Token: 0x04001982 RID: 6530
		[Tooltip("the value to remap the curve's 0 to")]
		[Range(0f, 300f)]
		public float RemapFocalLengthZero = 27.5f;

		// Token: 0x04001983 RID: 6531
		[Tooltip("the value to remap the curve's 1 to")]
		[Range(0f, 300f)]
		public float RemapFocalLengthOne = 27.5f;
	}
}
