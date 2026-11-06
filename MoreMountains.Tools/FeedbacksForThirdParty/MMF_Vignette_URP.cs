using System;
using MoreMountains.Feedbacks;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

namespace MoreMountains.FeedbacksForThirdParty
{
	// Token: 0x02000352 RID: 850
	[AddComponentMenu("")]
	[FeedbackPath("PostProcess/Vignette URP")]
	[MovedFrom(false, null, "MoreMountains.Feedbacks.URP", null)]
	[FeedbackHelp("This feedback allows you to control vignette intensity over time. It requires you have in your scene an object with a Volume with Vignette active, and a MMVignetteShaker_URP component.")]
	public class MMF_Vignette_URP : MMF_Feedback
	{
		// Token: 0x170002C0 RID: 704
		// (get) Token: 0x06001756 RID: 5974 RVA: 0x00062DBE File Offset: 0x00060FBE
		// (set) Token: 0x06001757 RID: 5975 RVA: 0x00062DCC File Offset: 0x00060FCC
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

		// Token: 0x170002C1 RID: 705
		// (get) Token: 0x06001758 RID: 5976 RVA: 0x00062DD5 File Offset: 0x00060FD5
		public override bool HasChannel
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170002C2 RID: 706
		// (get) Token: 0x06001759 RID: 5977 RVA: 0x00062DD8 File Offset: 0x00060FD8
		public override bool HasRandomness
		{
			get
			{
				return true;
			}
		}

		// Token: 0x0600175A RID: 5978 RVA: 0x00062DDC File Offset: 0x00060FDC
		protected override void CustomPlayFeedback(Vector3 position, float feedbacksIntensity = 1f)
		{
			if (!this.Active || !MMF_Vignette_URP.FeedbackTypeAuthorized)
			{
				return;
			}
			float num = this.ComputeIntensity(feedbacksIntensity, position);
			MMVignetteShakeEvent_URP.Trigger(this.Intensity, this.FeedbackDuration, this.RemapIntensityZero, this.RemapIntensityOne, this.RelativeIntensity, num, this.ChannelData, this.ResetShakerValuesAfterShake, this.ResetTargetValuesAfterShake, this.NormalPlayDirection, this.ComputedTimescaleMode, false, false, this.InterpolateColor, this.ColorCurve, this.RemapColorZero, this.RemapColorOne, this.TargetColor);
		}

		// Token: 0x0600175B RID: 5979 RVA: 0x00062E64 File Offset: 0x00061064
		protected override void CustomStopFeedback(Vector3 position, float feedbacksIntensity = 1f)
		{
			base.CustomStopFeedback(position, feedbacksIntensity);
			if (!this.Active || !MMF_Vignette_URP.FeedbackTypeAuthorized)
			{
				return;
			}
			MMVignetteShakeEvent_URP.Trigger(this.Intensity, this.FeedbackDuration, this.RemapIntensityZero, this.RemapIntensityOne, this.RelativeIntensity, 1f, this.ChannelData, true, true, true, TimescaleModes.Scaled, true, false, false, null, 0f, 1f, default(Color));
		}

		// Token: 0x0600175C RID: 5980 RVA: 0x00062ED4 File Offset: 0x000610D4
		protected override void CustomRestoreInitialValues()
		{
			if (!this.Active || !MMF_Vignette_URP.FeedbackTypeAuthorized)
			{
				return;
			}
			MMVignetteShakeEvent_URP.Trigger(this.Intensity, this.FeedbackDuration, this.RemapIntensityZero, this.RemapIntensityOne, this.RelativeIntensity, 1f, this.ChannelData, true, true, true, TimescaleModes.Scaled, false, true, false, null, 0f, 1f, default(Color));
		}

		// Token: 0x0600175D RID: 5981 RVA: 0x00062F3A File Offset: 0x0006113A
		public override void AutomaticShakerSetup()
		{
		}

		// Token: 0x04001AD4 RID: 6868
		public static bool FeedbackTypeAuthorized = true;

		// Token: 0x04001AD5 RID: 6869
		[MMFInspectorGroup("Vignette", true, 28, false, false)]
		[Tooltip("the duration of the shake, in seconds")]
		public float Duration = 0.2f;

		// Token: 0x04001AD6 RID: 6870
		[Tooltip("whether or not to reset shaker values after shake")]
		public bool ResetShakerValuesAfterShake = true;

		// Token: 0x04001AD7 RID: 6871
		[Tooltip("whether or not to reset the target's values after shake")]
		public bool ResetTargetValuesAfterShake = true;

		// Token: 0x04001AD8 RID: 6872
		[MMFInspectorGroup("Intensity", true, 29, false, false)]
		[Tooltip("the curve to animate the intensity on")]
		public AnimationCurve Intensity = new AnimationCurve(new Keyframe[]
		{
			new Keyframe(0f, 0f),
			new Keyframe(0.5f, 1f),
			new Keyframe(1f, 0f)
		});

		// Token: 0x04001AD9 RID: 6873
		[Tooltip("the value to remap the curve's zero to")]
		[Range(0f, 1f)]
		public float RemapIntensityZero;

		// Token: 0x04001ADA RID: 6874
		[Tooltip("the value to remap the curve's one to")]
		[Range(0f, 1f)]
		public float RemapIntensityOne = 1f;

		// Token: 0x04001ADB RID: 6875
		[Tooltip("whether or not to add to the initial intensity")]
		public bool RelativeIntensity;

		// Token: 0x04001ADC RID: 6876
		[Header("Color")]
		[Tooltip("whether or not to also animate the vignette's color")]
		public bool InterpolateColor;

		// Token: 0x04001ADD RID: 6877
		[Tooltip("the curve to animate the color on")]
		public AnimationCurve ColorCurve = new AnimationCurve(new Keyframe[]
		{
			new Keyframe(0f, 0f),
			new Keyframe(0.05f, 1f),
			new Keyframe(0.95f, 1f),
			new Keyframe(1f, 0f)
		});

		// Token: 0x04001ADE RID: 6878
		[Tooltip("the value to remap the curve's 0 to")]
		[Range(0f, 1f)]
		public float RemapColorZero;

		// Token: 0x04001ADF RID: 6879
		[Tooltip("the value to remap the curve's 1 to")]
		[Range(0f, 1f)]
		public float RemapColorOne = 1f;

		// Token: 0x04001AE0 RID: 6880
		[Tooltip("the color to lerp towards")]
		public Color TargetColor = Color.red;
	}
}
