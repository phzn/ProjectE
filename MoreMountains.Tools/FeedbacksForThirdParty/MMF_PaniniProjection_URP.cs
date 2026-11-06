using System;
using MoreMountains.Feedbacks;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

namespace MoreMountains.FeedbacksForThirdParty
{
	// Token: 0x02000351 RID: 849
	[AddComponentMenu("")]
	[FeedbackHelp("This feedback allows you to control Panini Projection distance and crop to fit over time. It requires you have in your scene an object with a Volume with PaniniProjection active, and a MMPaniniProjectionShaker_URP component.")]
	[FeedbackPath("PostProcess/Panini Projection URP")]
	[MovedFrom(false, null, "MoreMountains.Feedbacks.URP", null)]
	public class MMF_PaniniProjection_URP : MMF_Feedback
	{
		// Token: 0x170002BD RID: 701
		// (get) Token: 0x0600174C RID: 5964 RVA: 0x00062BF2 File Offset: 0x00060DF2
		// (set) Token: 0x0600174D RID: 5965 RVA: 0x00062C00 File Offset: 0x00060E00
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

		// Token: 0x170002BE RID: 702
		// (get) Token: 0x0600174E RID: 5966 RVA: 0x00062C09 File Offset: 0x00060E09
		public override bool HasChannel
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170002BF RID: 703
		// (get) Token: 0x0600174F RID: 5967 RVA: 0x00062C0C File Offset: 0x00060E0C
		public override bool HasRandomness
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06001750 RID: 5968 RVA: 0x00062C10 File Offset: 0x00060E10
		protected override void CustomPlayFeedback(Vector3 position, float feedbacksIntensity = 1f)
		{
			if (!this.Active || !MMF_PaniniProjection_URP.FeedbackTypeAuthorized)
			{
				return;
			}
			float num = this.ComputeIntensity(feedbacksIntensity, position);
			MMPaniniProjectionShakeEvent_URP.Trigger(this.ShakeDistance, this.FeedbackDuration, this.RemapDistanceZero, this.RemapDistanceOne, this.RelativeDistance, num, this.ChannelData, this.ResetShakerValuesAfterShake, this.ResetTargetValuesAfterShake, this.NormalPlayDirection, this.ComputedTimescaleMode, false, false);
		}

		// Token: 0x06001751 RID: 5969 RVA: 0x00062C7C File Offset: 0x00060E7C
		protected override void CustomStopFeedback(Vector3 position, float feedbacksIntensity = 1f)
		{
			if (!this.Active || !MMF_PaniniProjection_URP.FeedbackTypeAuthorized)
			{
				return;
			}
			base.CustomStopFeedback(position, feedbacksIntensity);
			MMPaniniProjectionShakeEvent_URP.Trigger(this.ShakeDistance, this.FeedbackDuration, this.RemapDistanceZero, this.RemapDistanceOne, this.RelativeDistance, 1f, this.ChannelData, true, true, true, TimescaleModes.Scaled, true, false);
		}

		// Token: 0x06001752 RID: 5970 RVA: 0x00062CD8 File Offset: 0x00060ED8
		protected override void CustomRestoreInitialValues()
		{
			if (!this.Active || !MMF_PaniniProjection_URP.FeedbackTypeAuthorized)
			{
				return;
			}
			MMPaniniProjectionShakeEvent_URP.Trigger(this.ShakeDistance, this.FeedbackDuration, this.RemapDistanceZero, this.RemapDistanceOne, this.RelativeDistance, 1f, this.ChannelData, true, true, true, TimescaleModes.Scaled, false, true);
		}

		// Token: 0x06001753 RID: 5971 RVA: 0x00062D29 File Offset: 0x00060F29
		public override void AutomaticShakerSetup()
		{
		}

		// Token: 0x04001ACC RID: 6860
		public static bool FeedbackTypeAuthorized = true;

		// Token: 0x04001ACD RID: 6861
		[MMFInspectorGroup("Panini Projection", true, 28, false, false)]
		[Tooltip("the duration of the shake, in seconds")]
		public float Duration = 0.2f;

		// Token: 0x04001ACE RID: 6862
		[Tooltip("whether or not to reset shaker values after shake")]
		public bool ResetShakerValuesAfterShake = true;

		// Token: 0x04001ACF RID: 6863
		[Tooltip("whether or not to reset the target's values after shake")]
		public bool ResetTargetValuesAfterShake = true;

		// Token: 0x04001AD0 RID: 6864
		[MMFInspectorGroup("Distance", true, 27, false, false)]
		[Tooltip("whether or not to add to the initial value")]
		public bool RelativeDistance;

		// Token: 0x04001AD1 RID: 6865
		[Tooltip("the curve used to animate the distance value on")]
		public AnimationCurve ShakeDistance = new AnimationCurve(new Keyframe[]
		{
			new Keyframe(0f, 0f),
			new Keyframe(0.5f, 1f),
			new Keyframe(1f, 0f)
		});

		// Token: 0x04001AD2 RID: 6866
		[Tooltip("the value to remap the curve's 0 to")]
		[Range(0f, 1f)]
		public float RemapDistanceZero;

		// Token: 0x04001AD3 RID: 6867
		[Tooltip("the value to remap the curve's 1 to")]
		[Range(0f, 1f)]
		public float RemapDistanceOne = 1f;
	}
}
