using System;
using MoreMountains.Feedbacks;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

namespace MoreMountains.FeedbacksForThirdParty
{
	// Token: 0x020002F5 RID: 757
	[AddComponentMenu("")]
	[MovedFrom(false, null, "MoreMountains.Feedbacks.HDRP", null)]
	[FeedbackHelp("This feedback allows you to control Film Grain intensity over time. It requires you have in your scene an object with a Volume with Film Grain active, and a MMFilmGrainShaker_HDRP component.")]
	public class MMF_FilmGrain_HDRP : MMF_Feedback
	{
		// Token: 0x1700026F RID: 623
		// (get) Token: 0x0600155B RID: 5467 RVA: 0x0005B271 File Offset: 0x00059471
		// (set) Token: 0x0600155C RID: 5468 RVA: 0x0005B27F File Offset: 0x0005947F
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

		// Token: 0x17000270 RID: 624
		// (get) Token: 0x0600155D RID: 5469 RVA: 0x0005B288 File Offset: 0x00059488
		public override bool HasChannel
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000271 RID: 625
		// (get) Token: 0x0600155E RID: 5470 RVA: 0x0005B28B File Offset: 0x0005948B
		public override bool HasRandomness
		{
			get
			{
				return true;
			}
		}

		// Token: 0x0600155F RID: 5471 RVA: 0x0005B290 File Offset: 0x00059490
		protected override void CustomPlayFeedback(Vector3 position, float feedbacksIntensity = 1f)
		{
			if (!this.Active || !MMF_FilmGrain_HDRP.FeedbackTypeAuthorized)
			{
				return;
			}
			float num = this.ComputeIntensity(feedbacksIntensity, position);
			MMFilmGrainShakeEvent_HDRP.Trigger(this.Intensity, this.FeedbackDuration, this.RemapIntensityZero, this.RemapIntensityOne, this.RelativeIntensity, num, this.ChannelData, this.ResetShakerValuesAfterShake, this.ResetTargetValuesAfterShake, this.NormalPlayDirection, this.ComputedTimescaleMode, false, false);
		}

		// Token: 0x06001560 RID: 5472 RVA: 0x0005B2FC File Offset: 0x000594FC
		protected override void CustomStopFeedback(Vector3 position, float feedbacksIntensity = 1f)
		{
			if (!this.Active || !MMF_FilmGrain_HDRP.FeedbackTypeAuthorized)
			{
				return;
			}
			base.CustomStopFeedback(position, feedbacksIntensity);
			MMFilmGrainShakeEvent_HDRP.Trigger(this.Intensity, this.FeedbackDuration, this.RemapIntensityZero, this.RemapIntensityOne, this.RelativeIntensity, 1f, this.ChannelData, true, true, true, TimescaleModes.Scaled, true, false);
		}

		// Token: 0x06001561 RID: 5473 RVA: 0x0005B358 File Offset: 0x00059558
		protected override void CustomRestoreInitialValues()
		{
			if (!this.Active || !MMF_FilmGrain_HDRP.FeedbackTypeAuthorized)
			{
				return;
			}
			MMFilmGrainShakeEvent_HDRP.Trigger(this.Intensity, this.FeedbackDuration, this.RemapIntensityZero, this.RemapIntensityOne, this.RelativeIntensity, 1f, this.ChannelData, true, true, true, TimescaleModes.Scaled, false, true);
		}

		// Token: 0x06001562 RID: 5474 RVA: 0x0005B3A9 File Offset: 0x000595A9
		public override void AutomaticShakerSetup()
		{
		}

		// Token: 0x04001882 RID: 6274
		public static bool FeedbackTypeAuthorized = true;

		// Token: 0x04001883 RID: 6275
		[MMFInspectorGroup("Film Grain", true, 20, false, false)]
		[Tooltip("the duration of the shake, in seconds")]
		public float Duration = 2f;

		// Token: 0x04001884 RID: 6276
		[Tooltip("whether or not to reset shaker values after shake")]
		public bool ResetShakerValuesAfterShake = true;

		// Token: 0x04001885 RID: 6277
		[Tooltip("whether or not to reset the target's values after shake")]
		public bool ResetTargetValuesAfterShake = true;

		// Token: 0x04001886 RID: 6278
		[MMFInspectorGroup("Intensity", true, 21, false, false)]
		[Tooltip("the curve to animate the intensity on")]
		public AnimationCurve Intensity = new AnimationCurve(new Keyframe[]
		{
			new Keyframe(0f, 0f),
			new Keyframe(0.5f, 1f),
			new Keyframe(1f, 0f)
		});

		// Token: 0x04001887 RID: 6279
		[Tooltip("the value to remap the curve's zero to")]
		[Range(0f, 1f)]
		public float RemapIntensityZero;

		// Token: 0x04001888 RID: 6280
		[Tooltip("the value to remap the curve's one to")]
		[Range(0f, 1f)]
		public float RemapIntensityOne = 1f;

		// Token: 0x04001889 RID: 6281
		[Tooltip("whether or not to add to the initial intensity")]
		public bool RelativeIntensity;
	}
}
