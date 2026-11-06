using System;
using MoreMountains.Feedbacks;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

namespace MoreMountains.FeedbacksForThirdParty
{
	// Token: 0x02000320 RID: 800
	[AddComponentMenu("")]
	[MovedFrom(false, null, "MoreMountains.Feedbacks.PostProcessing", null)]
	[FeedbackHelp("This feedback allows you to control lens distortion intensity over time. It requires you have in your scene an object with a PostProcessVolume with Lens Distortion active, and a MMLensDistortionShaker component.")]
	public class MMF_LensDistortion : MMF_Feedback
	{
		// Token: 0x17000290 RID: 656
		// (get) Token: 0x06001636 RID: 5686 RVA: 0x0005E3E5 File Offset: 0x0005C5E5
		// (set) Token: 0x06001637 RID: 5687 RVA: 0x0005E3F3 File Offset: 0x0005C5F3
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

		// Token: 0x17000291 RID: 657
		// (get) Token: 0x06001638 RID: 5688 RVA: 0x0005E3FC File Offset: 0x0005C5FC
		public override bool HasChannel
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000292 RID: 658
		// (get) Token: 0x06001639 RID: 5689 RVA: 0x0005E3FF File Offset: 0x0005C5FF
		public override bool HasRandomness
		{
			get
			{
				return true;
			}
		}

		// Token: 0x0600163A RID: 5690 RVA: 0x0005E404 File Offset: 0x0005C604
		protected override void CustomPlayFeedback(Vector3 position, float feedbacksIntensity = 1f)
		{
			if (!this.Active || !MMF_LensDistortion.FeedbackTypeAuthorized)
			{
				return;
			}
			float num = this.ComputeIntensity(feedbacksIntensity, position);
			MMLensDistortionShakeEvent.Trigger(this.Intensity, this.FeedbackDuration, this.RemapIntensityZero, this.RemapIntensityOne, this.RelativeIntensity, num, this.ChannelData, this.ResetShakerValuesAfterShake, this.ResetTargetValuesAfterShake, this.NormalPlayDirection, this.ComputedTimescaleMode, false, false);
		}

		// Token: 0x0600163B RID: 5691 RVA: 0x0005E470 File Offset: 0x0005C670
		protected override void CustomStopFeedback(Vector3 position, float feedbacksIntensity = 1f)
		{
			if (!this.Active || !MMF_LensDistortion.FeedbackTypeAuthorized)
			{
				return;
			}
			base.CustomStopFeedback(position, feedbacksIntensity);
			MMLensDistortionShakeEvent.Trigger(this.Intensity, this.FeedbackDuration, this.RemapIntensityZero, this.RemapIntensityOne, this.RelativeIntensity, 1f, null, true, true, true, TimescaleModes.Scaled, true, false);
		}

		// Token: 0x0600163C RID: 5692 RVA: 0x0005E4C4 File Offset: 0x0005C6C4
		protected override void CustomRestoreInitialValues()
		{
			if (!this.Active || !MMF_LensDistortion.FeedbackTypeAuthorized)
			{
				return;
			}
			MMLensDistortionShakeEvent.Trigger(this.Intensity, this.FeedbackDuration, this.RemapIntensityZero, this.RemapIntensityOne, this.RelativeIntensity, 1f, null, true, true, true, TimescaleModes.Scaled, false, true);
		}

		// Token: 0x0600163D RID: 5693 RVA: 0x0005E510 File Offset: 0x0005C710
		public override void AutomaticShakerSetup()
		{
		}

		// Token: 0x0400198D RID: 6541
		public static bool FeedbackTypeAuthorized = true;

		// Token: 0x0400198E RID: 6542
		[MMFInspectorGroup("Lens Distortion", true, 56, false, false)]
		[Tooltip("the duration of the shake in seconds")]
		public float Duration = 0.5f;

		// Token: 0x0400198F RID: 6543
		[Tooltip("whether or not to reset shaker values after shake")]
		public bool ResetShakerValuesAfterShake = true;

		// Token: 0x04001990 RID: 6544
		[Tooltip("whether or not to reset the target's values after shake")]
		public bool ResetTargetValuesAfterShake = true;

		// Token: 0x04001991 RID: 6545
		[MMFInspectorGroup("Intensity", true, 57, false, false)]
		[Tooltip("whether or not to add to the initial intensity value")]
		public bool RelativeIntensity;

		// Token: 0x04001992 RID: 6546
		[Tooltip("the curve to animate the intensity on")]
		public AnimationCurve Intensity = new AnimationCurve(new Keyframe[]
		{
			new Keyframe(0f, 0f),
			new Keyframe(0.2f, 1f),
			new Keyframe(0.25f, -1f),
			new Keyframe(0.35f, 0.7f),
			new Keyframe(0.4f, -0.7f),
			new Keyframe(0.6f, 0.3f),
			new Keyframe(0.65f, -0.3f),
			new Keyframe(0.8f, 0.1f),
			new Keyframe(0.85f, -0.1f),
			new Keyframe(1f, 0f)
		});

		// Token: 0x04001993 RID: 6547
		[Tooltip("the value to remap the curve's 0 to")]
		[Range(-100f, 100f)]
		public float RemapIntensityZero;

		// Token: 0x04001994 RID: 6548
		[Tooltip("the value to remap the curve's 1 to")]
		[Range(-100f, 100f)]
		public float RemapIntensityOne = 40f;
	}
}
