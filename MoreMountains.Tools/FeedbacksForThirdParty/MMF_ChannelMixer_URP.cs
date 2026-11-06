using System;
using MoreMountains.Feedbacks;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

namespace MoreMountains.FeedbacksForThirdParty
{
	// Token: 0x02000348 RID: 840
	[AddComponentMenu("")]
	[FeedbackHelp("This feedback allows you to control bloom intensity and threshold over time. It requires you have in your scene an object with a Volume with Bloom active, and a MMBloomShaker_URP component.")]
	[FeedbackPath("PostProcess/Channel Mixer URP")]
	[MovedFrom(false, null, "MoreMountains.Feedbacks.URP", null)]
	public class MMF_ChannelMixer_URP : MMF_Feedback
	{
		// Token: 0x170002A2 RID: 674
		// (get) Token: 0x060016EE RID: 5870 RVA: 0x00060F64 File Offset: 0x0005F164
		// (set) Token: 0x060016EF RID: 5871 RVA: 0x00060F72 File Offset: 0x0005F172
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

		// Token: 0x170002A3 RID: 675
		// (get) Token: 0x060016F0 RID: 5872 RVA: 0x00060F7B File Offset: 0x0005F17B
		public override bool HasChannel
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170002A4 RID: 676
		// (get) Token: 0x060016F1 RID: 5873 RVA: 0x00060F7E File Offset: 0x0005F17E
		public override bool HasRandomness
		{
			get
			{
				return true;
			}
		}

		// Token: 0x060016F2 RID: 5874 RVA: 0x00060F84 File Offset: 0x0005F184
		protected override void CustomPlayFeedback(Vector3 position, float feedbacksIntensity = 1f)
		{
			if (!this.Active || !MMF_ChannelMixer_URP.FeedbackTypeAuthorized)
			{
				return;
			}
			float num = this.ComputeIntensity(feedbacksIntensity, position);
			MMChannelMixerShakeEvent_URP.Trigger(this.ShakeRed, this.RemapRedZero, this.RemapRedOne, this.ShakeGreen, this.RemapGreenZero, this.RemapGreenOne, this.ShakeBlue, this.RemapBlueZero, this.RemapBlueOne, this.FeedbackDuration, this.RelativeIntensity, num, this.ChannelData, this.ResetShakerValuesAfterShake, this.ResetTargetValuesAfterShake, this.NormalPlayDirection, this.ComputedTimescaleMode, false, false);
		}

		// Token: 0x060016F3 RID: 5875 RVA: 0x00061014 File Offset: 0x0005F214
		protected override void CustomStopFeedback(Vector3 position, float feedbacksIntensity = 1f)
		{
			if (!this.Active || !MMF_ChannelMixer_URP.FeedbackTypeAuthorized)
			{
				return;
			}
			base.CustomStopFeedback(position, feedbacksIntensity);
			MMChannelMixerShakeEvent_URP.Trigger(this.ShakeRed, this.RemapRedZero, this.RemapRedOne, this.ShakeGreen, this.RemapGreenZero, this.RemapGreenOne, this.ShakeBlue, this.RemapBlueZero, this.RemapBlueOne, this.FeedbackDuration, this.RelativeIntensity, 1f, this.ChannelData, true, true, true, TimescaleModes.Scaled, true, false);
		}

		// Token: 0x060016F4 RID: 5876 RVA: 0x00061094 File Offset: 0x0005F294
		protected override void CustomRestoreInitialValues()
		{
			if (!this.Active || !MMF_ChannelMixer_URP.FeedbackTypeAuthorized)
			{
				return;
			}
			MMChannelMixerShakeEvent_URP.Trigger(this.ShakeRed, this.RemapRedZero, this.RemapRedOne, this.ShakeGreen, this.RemapGreenZero, this.RemapGreenOne, this.ShakeBlue, this.RemapBlueZero, this.RemapBlueOne, this.FeedbackDuration, this.RelativeIntensity, 1f, this.ChannelData, true, true, true, TimescaleModes.Scaled, false, true);
		}

		// Token: 0x060016F5 RID: 5877 RVA: 0x00061109 File Offset: 0x0005F309
		public override void AutomaticShakerSetup()
		{
		}

		// Token: 0x04001A42 RID: 6722
		public static bool FeedbackTypeAuthorized = true;

		// Token: 0x04001A43 RID: 6723
		[MMFInspectorGroup("Channel Mixer", true, 41, false, false)]
		[Tooltip("the duration of the shake, in seconds")]
		public float ShakeDuration = 1f;

		// Token: 0x04001A44 RID: 6724
		[Tooltip("whether or not to add to the initial intensity")]
		public bool RelativeIntensity = true;

		// Token: 0x04001A45 RID: 6725
		[Tooltip("whether or not to reset shaker values after shake")]
		public bool ResetShakerValuesAfterShake = true;

		// Token: 0x04001A46 RID: 6726
		[Tooltip("whether or not to reset the target's values after shake")]
		public bool ResetTargetValuesAfterShake = true;

		// Token: 0x04001A47 RID: 6727
		[MMFInspectorGroup("Red", true, 42, false, false)]
		[Tooltip("the curve used to animate the red value on")]
		public AnimationCurve ShakeRed = new AnimationCurve(new Keyframe[]
		{
			new Keyframe(0f, 0f),
			new Keyframe(0.5f, 1f),
			new Keyframe(1f, 0f)
		});

		// Token: 0x04001A48 RID: 6728
		[Tooltip("the value to remap the curve's 0 to")]
		[Range(-200f, 200f)]
		public float RemapRedZero;

		// Token: 0x04001A49 RID: 6729
		[Tooltip("the value to remap the curve's 1 to")]
		[Range(-200f, 200f)]
		public float RemapRedOne = -200f;

		// Token: 0x04001A4A RID: 6730
		[MMFInspectorGroup("Green", true, 43, false, false)]
		[Tooltip("the curve used to animate the green value on")]
		public AnimationCurve ShakeGreen = new AnimationCurve(new Keyframe[]
		{
			new Keyframe(0f, 0f),
			new Keyframe(0.5f, 1f),
			new Keyframe(1f, 0f)
		});

		// Token: 0x04001A4B RID: 6731
		[Tooltip("the value to remap the curve's 0 to")]
		[Range(-200f, 200f)]
		public float RemapGreenZero;

		// Token: 0x04001A4C RID: 6732
		[Tooltip("the value to remap the curve's 1 to")]
		[Range(-200f, 200f)]
		public float RemapGreenOne = 200f;

		// Token: 0x04001A4D RID: 6733
		[MMFInspectorGroup("Blue", true, 44, false, false)]
		[Tooltip("the curve used to animate the blue value on")]
		public AnimationCurve ShakeBlue = new AnimationCurve(new Keyframe[]
		{
			new Keyframe(0f, 0f),
			new Keyframe(0.5f, 1f),
			new Keyframe(1f, 0f)
		});

		// Token: 0x04001A4E RID: 6734
		[Tooltip("the value to remap the curve's 0 to")]
		[Range(-200f, 200f)]
		public float RemapBlueZero;

		// Token: 0x04001A4F RID: 6735
		[Tooltip("the value to remap the curve's 1 to")]
		[Range(-200f, 200f)]
		public float RemapBlueOne = 200f;
	}
}
