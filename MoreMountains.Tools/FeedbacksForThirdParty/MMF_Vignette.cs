using System;
using MoreMountains.Feedbacks;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

namespace MoreMountains.FeedbacksForThirdParty
{
	// Token: 0x02000321 RID: 801
	[AddComponentMenu("")]
	[MovedFrom(false, null, "MoreMountains.Feedbacks.PostProcessing", null)]
	[FeedbackHelp("This feedback allows you to control vignette intensity over time. It requires you have in your scene an object with a PostProcessVolume with Vignette active, and a MMVignetteShaker component.")]
	public class MMF_Vignette : MMF_Feedback
	{
		// Token: 0x17000293 RID: 659
		// (get) Token: 0x06001640 RID: 5696 RVA: 0x0005E642 File Offset: 0x0005C842
		// (set) Token: 0x06001641 RID: 5697 RVA: 0x0005E650 File Offset: 0x0005C850
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

		// Token: 0x17000294 RID: 660
		// (get) Token: 0x06001642 RID: 5698 RVA: 0x0005E659 File Offset: 0x0005C859
		public override bool HasChannel
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000295 RID: 661
		// (get) Token: 0x06001643 RID: 5699 RVA: 0x0005E65C File Offset: 0x0005C85C
		public override bool HasRandomness
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06001644 RID: 5700 RVA: 0x0005E660 File Offset: 0x0005C860
		protected override void CustomPlayFeedback(Vector3 position, float feedbacksIntensity = 1f)
		{
			if (!this.Active || !MMF_Vignette.FeedbackTypeAuthorized)
			{
				return;
			}
			float num = this.ComputeIntensity(feedbacksIntensity, position);
			MMVignetteShakeEvent.Trigger(this.Intensity, this.FeedbackDuration, this.RemapIntensityZero, this.RemapIntensityOne, this.RelativeIntensity, num, this.ChannelData, this.ResetShakerValuesAfterShake, this.ResetTargetValuesAfterShake, this.NormalPlayDirection, this.ComputedTimescaleMode, false, false, this.InterpolateColor, this.ColorCurve, this.RemapColorZero, this.RemapColorOne, this.TargetColor);
		}

		// Token: 0x06001645 RID: 5701 RVA: 0x0005E6E8 File Offset: 0x0005C8E8
		protected override void CustomStopFeedback(Vector3 position, float feedbacksIntensity = 1f)
		{
			if (!this.Active || !MMF_Vignette.FeedbackTypeAuthorized)
			{
				return;
			}
			base.CustomStopFeedback(position, feedbacksIntensity);
			MMVignetteShakeEvent.Trigger(this.Intensity, this.FeedbackDuration, this.RemapIntensityZero, this.RemapIntensityOne, this.RelativeIntensity, 1f, null, true, true, true, TimescaleModes.Scaled, true, false, false, null, 0f, 1f, default(Color));
		}

		// Token: 0x06001646 RID: 5702 RVA: 0x0005E754 File Offset: 0x0005C954
		protected override void CustomRestoreInitialValues()
		{
			if (!this.Active || !MMF_Vignette.FeedbackTypeAuthorized)
			{
				return;
			}
			MMVignetteShakeEvent.Trigger(this.Intensity, this.FeedbackDuration, this.RemapIntensityZero, this.RemapIntensityOne, this.RelativeIntensity, 1f, null, true, true, true, TimescaleModes.Scaled, false, true, false, null, 0f, 1f, default(Color));
		}

		// Token: 0x06001647 RID: 5703 RVA: 0x0005E7B5 File Offset: 0x0005C9B5
		public override void AutomaticShakerSetup()
		{
		}

		// Token: 0x04001995 RID: 6549
		public static bool FeedbackTypeAuthorized = true;

		// Token: 0x04001996 RID: 6550
		[MMFInspectorGroup("Vignette", true, 58, false, false)]
		[Tooltip("the duration of the shake, in seconds")]
		public float Duration = 0.2f;

		// Token: 0x04001997 RID: 6551
		[Tooltip("whether or not to reset shaker values after shake")]
		public bool ResetShakerValuesAfterShake = true;

		// Token: 0x04001998 RID: 6552
		[Tooltip("whether or not to reset the target's values after shake")]
		public bool ResetTargetValuesAfterShake = true;

		// Token: 0x04001999 RID: 6553
		[MMFInspectorGroup("Intensity", true, 59, false, false)]
		[Tooltip("the curve to animate the intensity on")]
		public AnimationCurve Intensity = new AnimationCurve(new Keyframe[]
		{
			new Keyframe(0f, 0f),
			new Keyframe(0.5f, 1f),
			new Keyframe(1f, 0f)
		});

		// Token: 0x0400199A RID: 6554
		[Tooltip("the value to remap the intensity's zero to")]
		[Range(0f, 1f)]
		public float RemapIntensityZero;

		// Token: 0x0400199B RID: 6555
		[Tooltip("the value to remap the intensity's one to")]
		[Range(0f, 1f)]
		public float RemapIntensityOne = 1f;

		// Token: 0x0400199C RID: 6556
		[Tooltip("whether or not to add to the initial intensity")]
		public bool RelativeIntensity;

		// Token: 0x0400199D RID: 6557
		[MMFInspectorGroup("Vignette Color", true, 60, false, false)]
		[Tooltip("whether or not to also animate the vignette's color")]
		public bool InterpolateColor;

		// Token: 0x0400199E RID: 6558
		[Tooltip("the curve to animate the color on")]
		public AnimationCurve ColorCurve = new AnimationCurve(new Keyframe[]
		{
			new Keyframe(0f, 0f),
			new Keyframe(0.05f, 1f),
			new Keyframe(0.95f, 1f),
			new Keyframe(1f, 0f)
		});

		// Token: 0x0400199F RID: 6559
		[Tooltip("the value to remap the curve's 0 to")]
		[Range(0f, 1f)]
		public float RemapColorZero;

		// Token: 0x040019A0 RID: 6560
		[Tooltip("the value to remap the curve's 1 to")]
		[Range(0f, 1f)]
		public float RemapColorOne = 1f;

		// Token: 0x040019A1 RID: 6561
		[Tooltip("the color to lerp towards")]
		public Color TargetColor = Color.red;
	}
}
