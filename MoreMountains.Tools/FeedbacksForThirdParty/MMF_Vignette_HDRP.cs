using System;
using MoreMountains.Feedbacks;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

namespace MoreMountains.FeedbacksForThirdParty
{
	// Token: 0x020002F9 RID: 761
	[AddComponentMenu("")]
	[MovedFrom(false, null, "MoreMountains.Feedbacks.HDRP", null)]
	[FeedbackHelp("This feedback allows you to control vignette intensity over time. It requires you have in your scene an object with a Volume with Vignette active, and a MMVignetteShaker_HDRP component.")]
	public class MMF_Vignette_HDRP : MMF_Feedback
	{
		// Token: 0x1700027B RID: 635
		// (get) Token: 0x06001583 RID: 5507 RVA: 0x0005BA3E File Offset: 0x00059C3E
		// (set) Token: 0x06001584 RID: 5508 RVA: 0x0005BA4C File Offset: 0x00059C4C
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

		// Token: 0x1700027C RID: 636
		// (get) Token: 0x06001585 RID: 5509 RVA: 0x0005BA55 File Offset: 0x00059C55
		public override bool HasChannel
		{
			get
			{
				return true;
			}
		}

		// Token: 0x1700027D RID: 637
		// (get) Token: 0x06001586 RID: 5510 RVA: 0x0005BA58 File Offset: 0x00059C58
		public override bool HasRandomness
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06001587 RID: 5511 RVA: 0x0005BA5C File Offset: 0x00059C5C
		protected override void CustomPlayFeedback(Vector3 position, float feedbacksIntensity = 1f)
		{
			if (!this.Active || !MMF_Vignette_HDRP.FeedbackTypeAuthorized)
			{
				return;
			}
			float num = this.ComputeIntensity(feedbacksIntensity, position);
			MMVignetteShakeEvent_HDRP.Trigger(this.Intensity, this.FeedbackDuration, this.RemapIntensityZero, this.RemapIntensityOne, this.RelativeIntensity, num, this.ChannelData, this.ResetShakerValuesAfterShake, this.ResetTargetValuesAfterShake, this.NormalPlayDirection, this.ComputedTimescaleMode, false, false, this.InterpolateColor, this.ColorCurve, this.RemapColorZero, this.RemapColorOne, this.TargetColor);
		}

		// Token: 0x06001588 RID: 5512 RVA: 0x0005BAE4 File Offset: 0x00059CE4
		protected override void CustomStopFeedback(Vector3 position, float feedbacksIntensity = 1f)
		{
			if (!this.Active || !MMF_Vignette_HDRP.FeedbackTypeAuthorized)
			{
				return;
			}
			base.CustomStopFeedback(position, feedbacksIntensity);
			MMVignetteShakeEvent_HDRP.Trigger(this.Intensity, this.FeedbackDuration, this.RemapIntensityZero, this.RemapIntensityOne, this.RelativeIntensity, 1f, this.ChannelData, true, true, true, TimescaleModes.Scaled, true, false, false, null, 0f, 1f, default(Color));
		}

		// Token: 0x06001589 RID: 5513 RVA: 0x0005BB54 File Offset: 0x00059D54
		protected override void CustomRestoreInitialValues()
		{
			if (!this.Active || !MMF_Vignette_HDRP.FeedbackTypeAuthorized)
			{
				return;
			}
			MMVignetteShakeEvent_HDRP.Trigger(this.Intensity, this.FeedbackDuration, this.RemapIntensityZero, this.RemapIntensityOne, this.RelativeIntensity, 1f, this.ChannelData, true, true, true, TimescaleModes.Scaled, false, true, false, null, 0f, 1f, default(Color));
		}

		// Token: 0x0600158A RID: 5514 RVA: 0x0005BBBA File Offset: 0x00059DBA
		public override void AutomaticShakerSetup()
		{
		}

		// Token: 0x040018A2 RID: 6306
		public static bool FeedbackTypeAuthorized = true;

		// Token: 0x040018A3 RID: 6307
		[MMFInspectorGroup("Vignette", true, 28, false, false)]
		[Tooltip("the duration of the shake, in seconds")]
		public float Duration = 0.2f;

		// Token: 0x040018A4 RID: 6308
		[Tooltip("whether or not to reset shaker values after shake")]
		public bool ResetShakerValuesAfterShake = true;

		// Token: 0x040018A5 RID: 6309
		[Tooltip("whether or not to reset the target's values after shake")]
		public bool ResetTargetValuesAfterShake = true;

		// Token: 0x040018A6 RID: 6310
		[MMFInspectorGroup("Intensity", true, 29, false, false)]
		[Tooltip("the curve to animate the intensity on")]
		public AnimationCurve Intensity = new AnimationCurve(new Keyframe[]
		{
			new Keyframe(0f, 0f),
			new Keyframe(0.5f, 1f),
			new Keyframe(1f, 0f)
		});

		// Token: 0x040018A7 RID: 6311
		[Tooltip("the value to remap the curve's zero to")]
		[Range(0f, 1f)]
		public float RemapIntensityZero;

		// Token: 0x040018A8 RID: 6312
		[Tooltip("the value to remap the curve's one to")]
		[Range(0f, 1f)]
		public float RemapIntensityOne = 1f;

		// Token: 0x040018A9 RID: 6313
		[Tooltip("whether or not to add to the initial intensity")]
		public bool RelativeIntensity;

		// Token: 0x040018AA RID: 6314
		[Header("Color")]
		[Tooltip("whether or not to also animate the vignette's color")]
		public bool InterpolateColor;

		// Token: 0x040018AB RID: 6315
		[Tooltip("the curve to animate the color on")]
		public AnimationCurve ColorCurve = new AnimationCurve(new Keyframe[]
		{
			new Keyframe(0f, 0f),
			new Keyframe(0.05f, 1f),
			new Keyframe(0.95f, 1f),
			new Keyframe(1f, 0f)
		});

		// Token: 0x040018AC RID: 6316
		[Tooltip("the value to remap the curve's 0 to")]
		[Range(0f, 1f)]
		public float RemapColorZero;

		// Token: 0x040018AD RID: 6317
		[Tooltip("the value to remap the curve's 1 to")]
		[Range(0f, 1f)]
		public float RemapColorOne = 1f;

		// Token: 0x040018AE RID: 6318
		[Tooltip("the color to lerp towards")]
		public Color TargetColor = Color.red;
	}
}
