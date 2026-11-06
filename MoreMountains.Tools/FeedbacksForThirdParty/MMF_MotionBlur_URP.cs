using System;
using MoreMountains.Feedbacks;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

namespace MoreMountains.FeedbacksForThirdParty
{
	// Token: 0x02000350 RID: 848
	[AddComponentMenu("")]
	[FeedbackPath("PostProcess/Motion Blur URP")]
	[MovedFrom(false, null, "MoreMountains.Feedbacks.URP", null)]
	[FeedbackHelp("This feedback allows you to control motion blur intensity over time. It requires you have in your scene an object with a Volume with MotionBlur active, and a MMMotionBlurShaker_URP component.")]
	public class MMF_MotionBlur_URP : MMF_Feedback
	{
		// Token: 0x170002BA RID: 698
		// (get) Token: 0x06001742 RID: 5954 RVA: 0x00062A26 File Offset: 0x00060C26
		// (set) Token: 0x06001743 RID: 5955 RVA: 0x00062A34 File Offset: 0x00060C34
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

		// Token: 0x170002BB RID: 699
		// (get) Token: 0x06001744 RID: 5956 RVA: 0x00062A3D File Offset: 0x00060C3D
		public override bool HasChannel
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170002BC RID: 700
		// (get) Token: 0x06001745 RID: 5957 RVA: 0x00062A40 File Offset: 0x00060C40
		public override bool HasRandomness
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06001746 RID: 5958 RVA: 0x00062A44 File Offset: 0x00060C44
		protected override void CustomPlayFeedback(Vector3 position, float feedbacksIntensity = 1f)
		{
			if (!this.Active || !MMF_MotionBlur_URP.FeedbackTypeAuthorized)
			{
				return;
			}
			float num = this.ComputeIntensity(feedbacksIntensity, position);
			MMMotionBlurShakeEvent_URP.Trigger(this.Intensity, this.FeedbackDuration, this.RemapIntensityZero, this.RemapIntensityOne, this.RelativeIntensity, num, this.ChannelData, this.ResetShakerValuesAfterShake, this.ResetTargetValuesAfterShake, this.NormalPlayDirection, this.ComputedTimescaleMode, false, false);
		}

		// Token: 0x06001747 RID: 5959 RVA: 0x00062AB0 File Offset: 0x00060CB0
		protected override void CustomStopFeedback(Vector3 position, float feedbacksIntensity = 1f)
		{
			if (!this.Active || !MMF_MotionBlur_URP.FeedbackTypeAuthorized)
			{
				return;
			}
			base.CustomStopFeedback(position, feedbacksIntensity);
			MMMotionBlurShakeEvent_URP.Trigger(this.Intensity, this.FeedbackDuration, this.RemapIntensityZero, this.RemapIntensityOne, this.RelativeIntensity, 1f, this.ChannelData, true, true, true, TimescaleModes.Scaled, true, false);
		}

		// Token: 0x06001748 RID: 5960 RVA: 0x00062B0C File Offset: 0x00060D0C
		protected override void CustomRestoreInitialValues()
		{
			if (!this.Active || !MMF_MotionBlur_URP.FeedbackTypeAuthorized)
			{
				return;
			}
			MMMotionBlurShakeEvent_URP.Trigger(this.Intensity, this.FeedbackDuration, this.RemapIntensityZero, this.RemapIntensityOne, this.RelativeIntensity, 1f, this.ChannelData, true, true, true, TimescaleModes.Scaled, false, true);
		}

		// Token: 0x06001749 RID: 5961 RVA: 0x00062B5D File Offset: 0x00060D5D
		public override void AutomaticShakerSetup()
		{
		}

		// Token: 0x04001AC4 RID: 6852
		public static bool FeedbackTypeAuthorized = true;

		// Token: 0x04001AC5 RID: 6853
		[MMFInspectorGroup("Motion Blur", true, 25, false, false)]
		[Tooltip("the duration of the shake, in seconds")]
		public float Duration = 0.2f;

		// Token: 0x04001AC6 RID: 6854
		[Tooltip("whether or not to reset shaker values after shake")]
		public bool ResetShakerValuesAfterShake = true;

		// Token: 0x04001AC7 RID: 6855
		[Tooltip("whether or not to reset the target's values after shake")]
		public bool ResetTargetValuesAfterShake = true;

		// Token: 0x04001AC8 RID: 6856
		[MMFInspectorGroup("Intensity", true, 24, false, false)]
		[Tooltip("the curve to animate the intensity on")]
		public AnimationCurve Intensity = new AnimationCurve(new Keyframe[]
		{
			new Keyframe(0f, 0f),
			new Keyframe(0.5f, 1f),
			new Keyframe(1f, 0f)
		});

		// Token: 0x04001AC9 RID: 6857
		[Tooltip("the value to which to remap the curve's zero to")]
		[Range(0f, 1f)]
		public float RemapIntensityZero;

		// Token: 0x04001ACA RID: 6858
		[Tooltip("the value to which to remap the curve's one to")]
		[Range(0f, 1f)]
		public float RemapIntensityOne = 1f;

		// Token: 0x04001ACB RID: 6859
		[Tooltip("whether or not to add to the initial intensity")]
		public bool RelativeIntensity;
	}
}
