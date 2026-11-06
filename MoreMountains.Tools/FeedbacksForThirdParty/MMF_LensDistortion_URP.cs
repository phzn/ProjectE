using System;
using MoreMountains.Feedbacks;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

namespace MoreMountains.FeedbacksForThirdParty
{
	// Token: 0x0200034E RID: 846
	[AddComponentMenu("")]
	[FeedbackPath("PostProcess/Lens Distortion URP")]
	[MovedFrom(false, null, "MoreMountains.Feedbacks.URP", null)]
	[FeedbackHelp("This feedback allows you to control URP lens distortion intensity over time. It requires you have in your scene an object with a Volume with Lens Distortion active, and a MMLensDistortionShaker_URP component.")]
	public class MMF_LensDistortion_URP : MMF_Feedback
	{
		// Token: 0x170002B3 RID: 691
		// (get) Token: 0x06001729 RID: 5929 RVA: 0x00061F46 File Offset: 0x00060146
		// (set) Token: 0x0600172A RID: 5930 RVA: 0x00061F54 File Offset: 0x00060154
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

		// Token: 0x170002B4 RID: 692
		// (get) Token: 0x0600172B RID: 5931 RVA: 0x00061F5D File Offset: 0x0006015D
		public override bool HasChannel
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170002B5 RID: 693
		// (get) Token: 0x0600172C RID: 5932 RVA: 0x00061F60 File Offset: 0x00060160
		public override bool HasRandomness
		{
			get
			{
				return true;
			}
		}

		// Token: 0x0600172D RID: 5933 RVA: 0x00061F64 File Offset: 0x00060164
		protected override void CustomPlayFeedback(Vector3 position, float feedbacksIntensity = 1f)
		{
			if (!this.Active || !MMF_LensDistortion_URP.FeedbackTypeAuthorized)
			{
				return;
			}
			float num = this.ComputeIntensity(feedbacksIntensity, position);
			MMLensDistortionShakeEvent_URP.Trigger(this.Intensity, this.FeedbackDuration, this.RemapIntensityZero, this.RemapIntensityOne, this.RelativeIntensity, num, this.ChannelData, this.ResetShakerValuesAfterShake, this.ResetTargetValuesAfterShake, this.NormalPlayDirection, this.ComputedTimescaleMode, false, false);
		}

		// Token: 0x0600172E RID: 5934 RVA: 0x00061FD0 File Offset: 0x000601D0
		protected override void CustomStopFeedback(Vector3 position, float feedbacksIntensity = 1f)
		{
			if (!this.Active || !MMF_LensDistortion_URP.FeedbackTypeAuthorized)
			{
				return;
			}
			base.CustomStopFeedback(position, feedbacksIntensity);
			MMLensDistortionShakeEvent_URP.Trigger(this.Intensity, this.FeedbackDuration, this.RemapIntensityZero, this.RemapIntensityOne, this.RelativeIntensity, 1f, this.ChannelData, true, true, true, TimescaleModes.Scaled, true, false);
		}

		// Token: 0x0600172F RID: 5935 RVA: 0x0006202C File Offset: 0x0006022C
		protected override void CustomRestoreInitialValues()
		{
			if (!this.Active || !MMF_LensDistortion_URP.FeedbackTypeAuthorized)
			{
				return;
			}
			MMLensDistortionShakeEvent_URP.Trigger(this.Intensity, this.FeedbackDuration, this.RemapIntensityZero, this.RemapIntensityOne, this.RelativeIntensity, 1f, this.ChannelData, true, true, true, TimescaleModes.Scaled, false, true);
		}

		// Token: 0x06001730 RID: 5936 RVA: 0x0006207D File Offset: 0x0006027D
		public override void AutomaticShakerSetup()
		{
		}

		// Token: 0x04001A8C RID: 6796
		public static bool FeedbackTypeAuthorized = true;

		// Token: 0x04001A8D RID: 6797
		[MMFInspectorGroup("Lens Distortion", true, 22, false, false)]
		[Tooltip("the duration of the shake in seconds")]
		public float Duration = 0.8f;

		// Token: 0x04001A8E RID: 6798
		[Tooltip("whether or not to reset shaker values after shake")]
		public bool ResetShakerValuesAfterShake = true;

		// Token: 0x04001A8F RID: 6799
		[Tooltip("whether or not to reset the target's values after shake")]
		public bool ResetTargetValuesAfterShake = true;

		// Token: 0x04001A90 RID: 6800
		[MMFInspectorGroup("Intensity", true, 23, false, false)]
		[Tooltip("whether or not to add to the initial intensity value")]
		public bool RelativeIntensity;

		// Token: 0x04001A91 RID: 6801
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

		// Token: 0x04001A92 RID: 6802
		[Tooltip("the value to remap the curve's 0 to")]
		[Range(-1f, 1f)]
		public float RemapIntensityZero;

		// Token: 0x04001A93 RID: 6803
		[Tooltip("the value to remap the curve's 1 to")]
		[Range(-1f, 1f)]
		public float RemapIntensityOne = 0.5f;
	}
}
