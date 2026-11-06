using System;
using MoreMountains.Feedbacks;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

namespace MoreMountains.FeedbacksForThirdParty
{
	// Token: 0x02000353 RID: 851
	[AddComponentMenu("")]
	[FeedbackHelp("This feedback allows you to control white balance temperature and tint over time. It requires you have in your scene an object with a Volume with WhiteBalance active, and a MMWhiteBalanceShaker_URP component.")]
	[FeedbackPath("PostProcess/White Balance URP")]
	[MovedFrom(false, null, "MoreMountains.Feedbacks.URP", null)]
	public class MMF_WhiteBalance_URP : MMF_Feedback
	{
		// Token: 0x170002C3 RID: 707
		// (get) Token: 0x06001760 RID: 5984 RVA: 0x0006304D File Offset: 0x0006124D
		// (set) Token: 0x06001761 RID: 5985 RVA: 0x0006305B File Offset: 0x0006125B
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

		// Token: 0x170002C4 RID: 708
		// (get) Token: 0x06001762 RID: 5986 RVA: 0x00063064 File Offset: 0x00061264
		public override bool HasChannel
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170002C5 RID: 709
		// (get) Token: 0x06001763 RID: 5987 RVA: 0x00063067 File Offset: 0x00061267
		public override bool HasRandomness
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06001764 RID: 5988 RVA: 0x0006306C File Offset: 0x0006126C
		protected override void CustomPlayFeedback(Vector3 position, float feedbacksIntensity = 1f)
		{
			if (!this.Active || !MMF_WhiteBalance_URP.FeedbackTypeAuthorized)
			{
				return;
			}
			float num = this.ComputeIntensity(feedbacksIntensity, position);
			MMWhiteBalanceShakeEvent_URP.Trigger(this.ShakeTemperature, this.FeedbackDuration, this.RemapTemperatureZero, this.RemapTemperatureOne, this.ShakeTint, this.RemapTintZero, this.RemapTintOne, this.RelativeValues, num, this.ChannelData, this.ResetShakerValuesAfterShake, this.ResetTargetValuesAfterShake, this.NormalPlayDirection, this.ComputedTimescaleMode, false, false);
		}

		// Token: 0x06001765 RID: 5989 RVA: 0x000630E8 File Offset: 0x000612E8
		protected override void CustomStopFeedback(Vector3 position, float feedbacksIntensity = 1f)
		{
			if (!this.Active || !MMF_WhiteBalance_URP.FeedbackTypeAuthorized)
			{
				return;
			}
			base.CustomStopFeedback(position, feedbacksIntensity);
			MMWhiteBalanceShakeEvent_URP.Trigger(this.ShakeTemperature, this.FeedbackDuration, this.RemapTemperatureZero, this.RemapTemperatureOne, this.ShakeTint, this.RemapTintZero, this.RemapTintOne, this.RelativeValues, 1f, this.ChannelData, true, true, true, TimescaleModes.Scaled, true, false);
		}

		// Token: 0x06001766 RID: 5990 RVA: 0x00063154 File Offset: 0x00061354
		protected override void CustomRestoreInitialValues()
		{
			if (!this.Active || !MMF_WhiteBalance_URP.FeedbackTypeAuthorized)
			{
				return;
			}
			MMWhiteBalanceShakeEvent_URP.Trigger(this.ShakeTemperature, this.FeedbackDuration, this.RemapTemperatureZero, this.RemapTemperatureOne, this.ShakeTint, this.RemapTintZero, this.RemapTintOne, this.RelativeValues, 1f, this.ChannelData, true, true, true, TimescaleModes.Scaled, false, true);
		}

		// Token: 0x06001767 RID: 5991 RVA: 0x000631B7 File Offset: 0x000613B7
		public override void AutomaticShakerSetup()
		{
		}

		// Token: 0x04001AE1 RID: 6881
		public static bool FeedbackTypeAuthorized = true;

		// Token: 0x04001AE2 RID: 6882
		[MMFInspectorGroup("White Balance", true, 29, false, false)]
		[Tooltip("the duration of the shake, in seconds")]
		public float ShakeDuration = 1f;

		// Token: 0x04001AE3 RID: 6883
		[Tooltip("whether or not to add to the initial value")]
		public bool RelativeValues = true;

		// Token: 0x04001AE4 RID: 6884
		[Tooltip("whether or not to reset shaker values after shake")]
		public bool ResetShakerValuesAfterShake = true;

		// Token: 0x04001AE5 RID: 6885
		[Tooltip("whether or not to reset the target's values after shake")]
		public bool ResetTargetValuesAfterShake = true;

		// Token: 0x04001AE6 RID: 6886
		[MMFInspectorGroup("Temperature", true, 29, false, false)]
		[Tooltip("the curve used to animate the temperature value on")]
		public AnimationCurve ShakeTemperature = new AnimationCurve(new Keyframe[]
		{
			new Keyframe(0f, 0f),
			new Keyframe(0.5f, 1f),
			new Keyframe(1f, 0f)
		});

		// Token: 0x04001AE7 RID: 6887
		[Tooltip("the value to remap the curve's 0 to")]
		[Range(-100f, 100f)]
		public float RemapTemperatureZero;

		// Token: 0x04001AE8 RID: 6888
		[Tooltip("the value to remap the curve's 1 to")]
		[Range(-100f, 100f)]
		public float RemapTemperatureOne = 100f;

		// Token: 0x04001AE9 RID: 6889
		[MMFInspectorGroup("Tint", true, 29, false, false)]
		[Tooltip("the curve used to animate the tint value on")]
		public AnimationCurve ShakeTint = new AnimationCurve(new Keyframe[]
		{
			new Keyframe(0f, 0f),
			new Keyframe(0.5f, 1f),
			new Keyframe(1f, 0f)
		});

		// Token: 0x04001AEA RID: 6890
		[Tooltip("the value to remap the curve's 0 to")]
		[Range(-100f, 100f)]
		public float RemapTintZero;

		// Token: 0x04001AEB RID: 6891
		[Tooltip("the value to remap the curve's 1 to")]
		[Range(-100f, 100f)]
		public float RemapTintOne = 100f;
	}
}
