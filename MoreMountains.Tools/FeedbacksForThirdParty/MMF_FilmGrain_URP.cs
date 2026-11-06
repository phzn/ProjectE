using System;
using MoreMountains.Feedbacks;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

namespace MoreMountains.FeedbacksForThirdParty
{
	// Token: 0x0200034C RID: 844
	[AddComponentMenu("")]
	[FeedbackPath("PostProcess/Film Grain URP")]
	[MovedFrom(false, null, "MoreMountains.Feedbacks.URP", null)]
	[FeedbackHelp("This feedback allows you to control Film Grain intensity over time. It requires you have in your scene an object with a Volume with Film Grain active, and a MMFilmGrainShaker_URP component.")]
	public class MMF_FilmGrain_URP : MMF_Feedback
	{
		// Token: 0x170002AE RID: 686
		// (get) Token: 0x06001716 RID: 5910 RVA: 0x00061B69 File Offset: 0x0005FD69
		// (set) Token: 0x06001717 RID: 5911 RVA: 0x00061B77 File Offset: 0x0005FD77
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

		// Token: 0x170002AF RID: 687
		// (get) Token: 0x06001718 RID: 5912 RVA: 0x00061B80 File Offset: 0x0005FD80
		public override bool HasChannel
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170002B0 RID: 688
		// (get) Token: 0x06001719 RID: 5913 RVA: 0x00061B83 File Offset: 0x0005FD83
		public override bool HasRandomness
		{
			get
			{
				return true;
			}
		}

		// Token: 0x0600171A RID: 5914 RVA: 0x00061B88 File Offset: 0x0005FD88
		protected override void CustomPlayFeedback(Vector3 position, float feedbacksIntensity = 1f)
		{
			if (!this.Active || !MMF_FilmGrain_URP.FeedbackTypeAuthorized)
			{
				return;
			}
			float num = this.ComputeIntensity(feedbacksIntensity, position);
			MMFilmGrainShakeEvent_URP.Trigger(this.Intensity, this.FeedbackDuration, this.RemapIntensityZero, this.RemapIntensityOne, this.RelativeIntensity, num, this.ChannelData, this.ResetShakerValuesAfterShake, this.ResetTargetValuesAfterShake, this.NormalPlayDirection, this.ComputedTimescaleMode, false, false);
		}

		// Token: 0x0600171B RID: 5915 RVA: 0x00061BF4 File Offset: 0x0005FDF4
		protected override void CustomStopFeedback(Vector3 position, float feedbacksIntensity = 1f)
		{
			if (!this.Active || !MMF_FilmGrain_URP.FeedbackTypeAuthorized)
			{
				return;
			}
			base.CustomStopFeedback(position, feedbacksIntensity);
			MMFilmGrainShakeEvent_URP.Trigger(this.Intensity, this.FeedbackDuration, this.RemapIntensityZero, this.RemapIntensityOne, this.RelativeIntensity, 1f, this.ChannelData, true, true, true, TimescaleModes.Scaled, true, false);
		}

		// Token: 0x0600171C RID: 5916 RVA: 0x00061C50 File Offset: 0x0005FE50
		protected override void CustomRestoreInitialValues()
		{
			if (!this.Active || !MMF_FilmGrain_URP.FeedbackTypeAuthorized)
			{
				return;
			}
			MMFilmGrainShakeEvent_URP.Trigger(this.Intensity, this.FeedbackDuration, this.RemapIntensityZero, this.RemapIntensityOne, this.RelativeIntensity, 1f, this.ChannelData, true, true, true, TimescaleModes.Scaled, false, true);
		}

		// Token: 0x0600171D RID: 5917 RVA: 0x00061CA1 File Offset: 0x0005FEA1
		public override void AutomaticShakerSetup()
		{
		}

		// Token: 0x04001A7C RID: 6780
		public static bool FeedbackTypeAuthorized = true;

		// Token: 0x04001A7D RID: 6781
		[MMFInspectorGroup("Film Grain", true, 21, false, false)]
		[Tooltip("the duration of the shake, in seconds")]
		public float Duration = 0.2f;

		// Token: 0x04001A7E RID: 6782
		[Tooltip("whether or not to reset shaker values after shake")]
		public bool ResetShakerValuesAfterShake = true;

		// Token: 0x04001A7F RID: 6783
		[Tooltip("whether or not to reset the target's values after shake")]
		public bool ResetTargetValuesAfterShake = true;

		// Token: 0x04001A80 RID: 6784
		[MMFInspectorGroup("Intensity", true, 22, false, false)]
		[Tooltip("the curve to animate the intensity on")]
		public AnimationCurve Intensity = new AnimationCurve(new Keyframe[]
		{
			new Keyframe(0f, 0f),
			new Keyframe(0.5f, 1f),
			new Keyframe(1f, 0f)
		});

		// Token: 0x04001A81 RID: 6785
		[Tooltip("the value to remap the curve's zero to")]
		[Range(0f, 1f)]
		public float RemapIntensityZero;

		// Token: 0x04001A82 RID: 6786
		[Tooltip("the value to remap the curve's one to")]
		[Range(0f, 1f)]
		public float RemapIntensityOne = 1f;

		// Token: 0x04001A83 RID: 6787
		[Tooltip("whether or not to add to the initial intensity")]
		public bool RelativeIntensity;
	}
}
