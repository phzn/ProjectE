using System;
using MoreMountains.Feedbacks;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

namespace MoreMountains.FeedbacksForThirdParty
{
	// Token: 0x02000349 RID: 841
	[AddComponentMenu("")]
	[FeedbackHelp("This feedback allows you to control chromatic aberration intensity over time. It requires you have in your scene an object with a Volume with Chromatic Aberration active, and a MMChromaticAberrationShaker_URP component.")]
	[FeedbackPath("PostProcess/Chromatic Aberration URP")]
	[MovedFrom(false, null, "MoreMountains.Feedbacks.URP", null)]
	public class MMF_ChromaticAberration_URP : MMF_Feedback
	{
		// Token: 0x170002A5 RID: 677
		// (get) Token: 0x060016F8 RID: 5880 RVA: 0x00061261 File Offset: 0x0005F461
		// (set) Token: 0x060016F9 RID: 5881 RVA: 0x0006126F File Offset: 0x0005F46F
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

		// Token: 0x170002A6 RID: 678
		// (get) Token: 0x060016FA RID: 5882 RVA: 0x00061278 File Offset: 0x0005F478
		public override bool HasChannel
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170002A7 RID: 679
		// (get) Token: 0x060016FB RID: 5883 RVA: 0x0006127B File Offset: 0x0005F47B
		public override bool HasRandomness
		{
			get
			{
				return true;
			}
		}

		// Token: 0x060016FC RID: 5884 RVA: 0x00061280 File Offset: 0x0005F480
		protected override void CustomPlayFeedback(Vector3 position, float feedbacksIntensity = 1f)
		{
			if (!this.Active || !MMF_ChromaticAberration_URP.FeedbackTypeAuthorized)
			{
				return;
			}
			float num = this.ComputeIntensity(feedbacksIntensity, position);
			MMChromaticAberrationShakeEvent_URP.Trigger(this.Intensity, this.FeedbackDuration, this.RemapIntensityZero, this.RemapIntensityOne, this.RelativeIntensity, num, this.ChannelData, this.ResetShakerValuesAfterShake, this.ResetTargetValuesAfterShake, this.NormalPlayDirection, this.ComputedTimescaleMode, false, false);
		}

		// Token: 0x060016FD RID: 5885 RVA: 0x000612EC File Offset: 0x0005F4EC
		protected override void CustomStopFeedback(Vector3 position, float feedbacksIntensity = 1f)
		{
			if (!this.Active || !MMF_ChromaticAberration_URP.FeedbackTypeAuthorized)
			{
				return;
			}
			base.CustomStopFeedback(position, feedbacksIntensity);
			MMChromaticAberrationShakeEvent_URP.Trigger(this.Intensity, this.FeedbackDuration, this.RemapIntensityZero, this.RemapIntensityOne, this.RelativeIntensity, 1f, this.ChannelData, true, true, true, TimescaleModes.Scaled, true, false);
		}

		// Token: 0x060016FE RID: 5886 RVA: 0x00061348 File Offset: 0x0005F548
		protected override void CustomRestoreInitialValues()
		{
			if (!this.Active || !MMF_ChromaticAberration_URP.FeedbackTypeAuthorized)
			{
				return;
			}
			MMChromaticAberrationShakeEvent_URP.Trigger(this.Intensity, this.FeedbackDuration, this.RemapIntensityZero, this.RemapIntensityOne, this.RelativeIntensity, 1f, this.ChannelData, true, true, true, TimescaleModes.Scaled, false, true);
		}

		// Token: 0x060016FF RID: 5887 RVA: 0x00061399 File Offset: 0x0005F599
		public override void AutomaticShakerSetup()
		{
		}

		// Token: 0x04001A50 RID: 6736
		public static bool FeedbackTypeAuthorized = true;

		// Token: 0x04001A51 RID: 6737
		[MMFInspectorGroup("Chromatic Aberration", true, 42, false, false)]
		[Tooltip("the duration of the shake, in seconds")]
		public float Duration = 0.2f;

		// Token: 0x04001A52 RID: 6738
		[Tooltip("whether or not to reset shaker values after shake")]
		public bool ResetShakerValuesAfterShake = true;

		// Token: 0x04001A53 RID: 6739
		[Tooltip("whether or not to reset the target's values after shake")]
		public bool ResetTargetValuesAfterShake = true;

		// Token: 0x04001A54 RID: 6740
		[Tooltip("the value to remap the curve's 0 to")]
		[Range(0f, 1f)]
		public float RemapIntensityZero;

		// Token: 0x04001A55 RID: 6741
		[Tooltip("the value to remap the curve's 1 to")]
		[Range(0f, 1f)]
		public float RemapIntensityOne = 1f;

		// Token: 0x04001A56 RID: 6742
		[MMFInspectorGroup("Intensity", true, 43, false, false)]
		[Tooltip("the curve to animate the intensity on")]
		public AnimationCurve Intensity = new AnimationCurve(new Keyframe[]
		{
			new Keyframe(0f, 0f),
			new Keyframe(0.5f, 1f),
			new Keyframe(1f, 0f)
		});

		// Token: 0x04001A57 RID: 6743
		[Tooltip("the multiplier to apply to the intensity curve")]
		[Range(0f, 1f)]
		public float Amplitude = 1f;

		// Token: 0x04001A58 RID: 6744
		[Tooltip("whether or not to add to the initial intensity")]
		public bool RelativeIntensity;
	}
}
