using System;
using MoreMountains.Feedbacks;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

namespace MoreMountains.FeedbacksForThirdParty
{
	// Token: 0x0200034B RID: 843
	[AddComponentMenu("")]
	[FeedbackHelp("This feedback allows you to control URP depth of field focus distance, aperture and focal length over time. It requires you have in your scene an object with a Volume with Depth of Field active, and a MMDepthOfFieldShaker_URP component.")]
	[FeedbackPath("PostProcess/Depth Of Field URP")]
	[MovedFrom(false, null, "MoreMountains.Feedbacks.URP", null)]
	public class MMF_DepthOfField_URP : MMF_Feedback
	{
		// Token: 0x170002AB RID: 683
		// (get) Token: 0x0600170C RID: 5900 RVA: 0x0006186D File Offset: 0x0005FA6D
		// (set) Token: 0x0600170D RID: 5901 RVA: 0x0006187B File Offset: 0x0005FA7B
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

		// Token: 0x170002AC RID: 684
		// (get) Token: 0x0600170E RID: 5902 RVA: 0x00061884 File Offset: 0x0005FA84
		public override bool HasChannel
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170002AD RID: 685
		// (get) Token: 0x0600170F RID: 5903 RVA: 0x00061887 File Offset: 0x0005FA87
		public override bool HasRandomness
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06001710 RID: 5904 RVA: 0x0006188C File Offset: 0x0005FA8C
		protected override void CustomPlayFeedback(Vector3 position, float feedbacksIntensity = 1f)
		{
			if (!this.Active || !MMF_DepthOfField_URP.FeedbackTypeAuthorized)
			{
				return;
			}
			float num = this.ComputeIntensity(feedbacksIntensity, position);
			MMDepthOfFieldShakeEvent_URP.Trigger(this.ShakeFocusDistance, this.FeedbackDuration, this.RemapFocusDistanceZero, this.RemapFocusDistanceOne, this.ShakeAperture, this.RemapApertureZero, this.RemapApertureOne, this.ShakeFocalLength, this.RemapFocalLengthZero, this.RemapFocalLengthOne, this.RelativeValues, num, this.ChannelData, this.ResetShakerValuesAfterShake, this.ResetTargetValuesAfterShake, this.NormalPlayDirection, this.ComputedTimescaleMode, false, false);
		}

		// Token: 0x06001711 RID: 5905 RVA: 0x0006191C File Offset: 0x0005FB1C
		protected override void CustomStopFeedback(Vector3 position, float feedbacksIntensity = 1f)
		{
			if (!this.Active || !MMF_DepthOfField_URP.FeedbackTypeAuthorized)
			{
				return;
			}
			base.CustomStopFeedback(position, feedbacksIntensity);
			MMDepthOfFieldShakeEvent_URP.Trigger(this.ShakeFocusDistance, this.FeedbackDuration, this.RemapFocusDistanceZero, this.RemapFocusDistanceOne, this.ShakeAperture, this.RemapApertureZero, this.RemapApertureOne, this.ShakeFocalLength, this.RemapFocalLengthZero, this.RemapFocalLengthOne, this.RelativeValues, 1f, this.ChannelData, true, true, true, TimescaleModes.Scaled, true, false);
		}

		// Token: 0x06001712 RID: 5906 RVA: 0x0006199C File Offset: 0x0005FB9C
		protected override void CustomRestoreInitialValues()
		{
			if (!this.Active || !MMF_DepthOfField_URP.FeedbackTypeAuthorized)
			{
				return;
			}
			MMDepthOfFieldShakeEvent_URP.Trigger(this.ShakeFocusDistance, this.FeedbackDuration, this.RemapFocusDistanceZero, this.RemapFocusDistanceOne, this.ShakeAperture, this.RemapApertureZero, this.RemapApertureOne, this.ShakeFocalLength, this.RemapFocalLengthZero, this.RemapFocalLengthOne, this.RelativeValues, 1f, this.ChannelData, true, true, true, TimescaleModes.Scaled, false, true);
		}

		// Token: 0x06001713 RID: 5907 RVA: 0x00061A11 File Offset: 0x0005FC11
		public override void AutomaticShakerSetup()
		{
		}

		// Token: 0x04001A6E RID: 6766
		public static bool FeedbackTypeAuthorized = true;

		// Token: 0x04001A6F RID: 6767
		[MMFInspectorGroup("Depth Of Field", true, 49, false, false)]
		[Tooltip("the duration of the shake, in seconds")]
		public float ShakeDuration = 2f;

		// Token: 0x04001A70 RID: 6768
		[Tooltip("whether or not to add to the initial values")]
		public bool RelativeValues = true;

		// Token: 0x04001A71 RID: 6769
		[Tooltip("whether or not to reset shaker values after shake")]
		public bool ResetShakerValuesAfterShake = true;

		// Token: 0x04001A72 RID: 6770
		[Tooltip("whether or not to reset the target's values after shake")]
		public bool ResetTargetValuesAfterShake = true;

		// Token: 0x04001A73 RID: 6771
		[MMFInspectorGroup("Focus Distance", true, 50, false, false)]
		[Tooltip("the curve used to animate the focus distance value on")]
		public AnimationCurve ShakeFocusDistance = new AnimationCurve(new Keyframe[]
		{
			new Keyframe(0f, 0f),
			new Keyframe(0.5f, 1f),
			new Keyframe(1f, 0f)
		});

		// Token: 0x04001A74 RID: 6772
		[Tooltip("the value to remap the curve's 0 to")]
		public float RemapFocusDistanceZero;

		// Token: 0x04001A75 RID: 6773
		[Tooltip("the value to remap the curve's 1 to")]
		public float RemapFocusDistanceOne = 3f;

		// Token: 0x04001A76 RID: 6774
		[MMFInspectorGroup("Aperture", true, 51, false, false)]
		[Tooltip("the curve used to animate the aperture value on")]
		public AnimationCurve ShakeAperture = new AnimationCurve(new Keyframe[]
		{
			new Keyframe(0f, 0f),
			new Keyframe(0.5f, 1f),
			new Keyframe(1f, 0f)
		});

		// Token: 0x04001A77 RID: 6775
		[Tooltip("the value to remap the curve's 0 to")]
		[Range(0.1f, 32f)]
		public float RemapApertureZero = 0.1f;

		// Token: 0x04001A78 RID: 6776
		[Tooltip("the value to remap the curve's 1 to")]
		[Range(0.1f, 32f)]
		public float RemapApertureOne = 32f;

		// Token: 0x04001A79 RID: 6777
		[MMFInspectorGroup("Focal Length", true, 20, false, false)]
		[Tooltip("the curve used to animate the focal length value on")]
		public AnimationCurve ShakeFocalLength = new AnimationCurve(new Keyframe[]
		{
			new Keyframe(0f, 0f),
			new Keyframe(0.5f, 1f),
			new Keyframe(1f, 0f)
		});

		// Token: 0x04001A7A RID: 6778
		[Tooltip("the value to remap the curve's 0 to")]
		[Range(0f, 300f)]
		public float RemapFocalLengthZero;

		// Token: 0x04001A7B RID: 6779
		[Tooltip("the value to remap the curve's 1 to")]
		[Range(0f, 300f)]
		public float RemapFocalLengthOne;
	}
}
