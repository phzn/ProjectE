using System;
using MoreMountains.Feedbacks;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

namespace MoreMountains.FeedbacksForThirdParty
{
	// Token: 0x020002FA RID: 762
	[AddComponentMenu("")]
	[FeedbackHelp("This feedback allows you to control white balance temperature and tint over time. It requires you have in your scene an object with a Volume with WhiteBalance active, and a MMWhiteBalanceShaker_HDRP component.")]
	[MovedFrom(false, null, "MoreMountains.Feedbacks.HDRP", null)]
	public class MMF_WhiteBalance_HDRP : MMF_Feedback
	{
		// Token: 0x1700027E RID: 638
		// (get) Token: 0x0600158D RID: 5517 RVA: 0x0005BCCD File Offset: 0x00059ECD
		// (set) Token: 0x0600158E RID: 5518 RVA: 0x0005BCDB File Offset: 0x00059EDB
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

		// Token: 0x1700027F RID: 639
		// (get) Token: 0x0600158F RID: 5519 RVA: 0x0005BCE4 File Offset: 0x00059EE4
		public override bool HasChannel
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000280 RID: 640
		// (get) Token: 0x06001590 RID: 5520 RVA: 0x0005BCE7 File Offset: 0x00059EE7
		public override bool HasRandomness
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06001591 RID: 5521 RVA: 0x0005BCEC File Offset: 0x00059EEC
		protected override void CustomPlayFeedback(Vector3 position, float feedbacksIntensity = 1f)
		{
			if (!this.Active || !MMF_WhiteBalance_HDRP.FeedbackTypeAuthorized)
			{
				return;
			}
			float num = this.ComputeIntensity(feedbacksIntensity, position);
			MMWhiteBalanceShakeEvent_HDRP.Trigger(this.ShakeTemperature, this.FeedbackDuration, this.RemapTemperatureZero, this.RemapTemperatureOne, this.ShakeTint, this.RemapTintZero, this.RemapTintOne, this.RelativeValues, num, this.ChannelData, this.ResetShakerValuesAfterShake, this.ResetTargetValuesAfterShake, this.NormalPlayDirection, this.ComputedTimescaleMode, false, false);
		}

		// Token: 0x06001592 RID: 5522 RVA: 0x0005BD68 File Offset: 0x00059F68
		protected override void CustomStopFeedback(Vector3 position, float feedbacksIntensity = 1f)
		{
			if (!this.Active || !MMF_WhiteBalance_HDRP.FeedbackTypeAuthorized)
			{
				return;
			}
			base.CustomStopFeedback(position, feedbacksIntensity);
			MMWhiteBalanceShakeEvent_HDRP.Trigger(this.ShakeTemperature, this.FeedbackDuration, this.RemapTemperatureZero, this.RemapTemperatureOne, this.ShakeTint, this.RemapTintZero, this.RemapTintOne, this.RelativeValues, 1f, this.ChannelData, true, true, true, TimescaleModes.Scaled, true, false);
		}

		// Token: 0x06001593 RID: 5523 RVA: 0x0005BDD4 File Offset: 0x00059FD4
		protected override void CustomRestoreInitialValues()
		{
			if (!this.Active || !MMF_WhiteBalance_HDRP.FeedbackTypeAuthorized)
			{
				return;
			}
			MMWhiteBalanceShakeEvent_HDRP.Trigger(this.ShakeTemperature, this.FeedbackDuration, this.RemapTemperatureZero, this.RemapTemperatureOne, this.ShakeTint, this.RemapTintZero, this.RemapTintOne, this.RelativeValues, 1f, this.ChannelData, true, true, true, TimescaleModes.Scaled, false, true);
		}

		// Token: 0x06001594 RID: 5524 RVA: 0x0005BE37 File Offset: 0x0005A037
		public override void AutomaticShakerSetup()
		{
		}

		// Token: 0x040018AF RID: 6319
		public static bool FeedbackTypeAuthorized = true;

		// Token: 0x040018B0 RID: 6320
		[MMFInspectorGroup("White Balance", true, 32, false, false)]
		[Tooltip("the duration of the shake, in seconds")]
		public float ShakeDuration = 1f;

		// Token: 0x040018B1 RID: 6321
		[Tooltip("whether or not to add to the initial value")]
		public bool RelativeValues = true;

		// Token: 0x040018B2 RID: 6322
		[Tooltip("whether or not to reset shaker values after shake")]
		public bool ResetShakerValuesAfterShake = true;

		// Token: 0x040018B3 RID: 6323
		[Tooltip("whether or not to reset the target's values after shake")]
		public bool ResetTargetValuesAfterShake = true;

		// Token: 0x040018B4 RID: 6324
		[MMFInspectorGroup("Temperature", true, 33, false, false)]
		[Tooltip("the curve used to animate the temperature value on")]
		public AnimationCurve ShakeTemperature = new AnimationCurve(new Keyframe[]
		{
			new Keyframe(0f, 0f),
			new Keyframe(0.5f, 1f),
			new Keyframe(1f, 0f)
		});

		// Token: 0x040018B5 RID: 6325
		[Tooltip("the value to remap the curve's 0 to")]
		[Range(-100f, 100f)]
		public float RemapTemperatureZero;

		// Token: 0x040018B6 RID: 6326
		[Tooltip("the value to remap the curve's 1 to")]
		[Range(-100f, 100f)]
		public float RemapTemperatureOne = 100f;

		// Token: 0x040018B7 RID: 6327
		[MMFInspectorGroup("Tint", true, 34, false, false)]
		[Tooltip("the curve used to animate the tint value on")]
		public AnimationCurve ShakeTint = new AnimationCurve(new Keyframe[]
		{
			new Keyframe(0f, 0f),
			new Keyframe(0.5f, 1f),
			new Keyframe(1f, 0f)
		});

		// Token: 0x040018B8 RID: 6328
		[Tooltip("the value to remap the curve's 0 to")]
		[Range(-100f, 100f)]
		public float RemapTintZero;

		// Token: 0x040018B9 RID: 6329
		[Tooltip("the value to remap the curve's 1 to")]
		[Range(-100f, 100f)]
		public float RemapTintOne = 100f;
	}
}
