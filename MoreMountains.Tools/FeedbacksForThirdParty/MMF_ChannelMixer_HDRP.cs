using System;
using MoreMountains.Feedbacks;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

namespace MoreMountains.FeedbacksForThirdParty
{
	// Token: 0x020002F0 RID: 752
	[AddComponentMenu("")]
	[MovedFrom(false, null, "MoreMountains.Feedbacks.HDRP", null)]
	[FeedbackHelp("This feedback allows you to control channel mixer's red, green and blue over time.It requires you have in your scene an object with a Volumewith Channel Mixer active, and a MM Channel Mixer HDRP component.")]
	public class MMF_ChannelMixer_HDRP : MMF_Feedback
	{
		// Token: 0x17000260 RID: 608
		// (get) Token: 0x06001529 RID: 5417 RVA: 0x0005A39C File Offset: 0x0005859C
		// (set) Token: 0x0600152A RID: 5418 RVA: 0x0005A3AA File Offset: 0x000585AA
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

		// Token: 0x17000261 RID: 609
		// (get) Token: 0x0600152B RID: 5419 RVA: 0x0005A3B3 File Offset: 0x000585B3
		public override bool HasChannel
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000262 RID: 610
		// (get) Token: 0x0600152C RID: 5420 RVA: 0x0005A3B6 File Offset: 0x000585B6
		public override bool HasRandomness
		{
			get
			{
				return true;
			}
		}

		// Token: 0x0600152D RID: 5421 RVA: 0x0005A3BC File Offset: 0x000585BC
		protected override void CustomPlayFeedback(Vector3 position, float feedbacksIntensity = 1f)
		{
			if (!this.Active || !MMF_ChannelMixer_HDRP.FeedbackTypeAuthorized)
			{
				return;
			}
			float num = this.ComputeIntensity(feedbacksIntensity, position);
			MMChannelMixerShakeEvent_HDRP.Trigger(this.ShakeRed, this.RemapRedZero, this.RemapRedOne, this.ShakeGreen, this.RemapGreenZero, this.RemapGreenOne, this.ShakeBlue, this.RemapBlueZero, this.RemapBlueOne, this.FeedbackDuration, this.RelativeIntensity, num, this.ChannelData, this.ResetShakerValuesAfterShake, this.ResetTargetValuesAfterShake, this.NormalPlayDirection, this.ComputedTimescaleMode, false, false);
		}

		// Token: 0x0600152E RID: 5422 RVA: 0x0005A44C File Offset: 0x0005864C
		protected override void CustomStopFeedback(Vector3 position, float feedbacksIntensity = 1f)
		{
			if (!this.Active || !MMF_ChannelMixer_HDRP.FeedbackTypeAuthorized)
			{
				return;
			}
			base.CustomStopFeedback(position, feedbacksIntensity);
			MMChannelMixerShakeEvent_HDRP.Trigger(this.ShakeRed, this.RemapRedZero, this.RemapRedOne, this.ShakeGreen, this.RemapGreenZero, this.RemapGreenOne, this.ShakeBlue, this.RemapBlueZero, this.RemapBlueOne, this.FeedbackDuration, this.RelativeIntensity, 1f, this.ChannelData, true, true, true, TimescaleModes.Scaled, true, false);
		}

		// Token: 0x0600152F RID: 5423 RVA: 0x0005A4CC File Offset: 0x000586CC
		protected override void CustomRestoreInitialValues()
		{
			if (!this.Active || !MMF_ChannelMixer_HDRP.FeedbackTypeAuthorized)
			{
				return;
			}
			MMChannelMixerShakeEvent_HDRP.Trigger(this.ShakeRed, this.RemapRedZero, this.RemapRedOne, this.ShakeGreen, this.RemapGreenZero, this.RemapGreenOne, this.ShakeBlue, this.RemapBlueZero, this.RemapBlueOne, this.FeedbackDuration, this.RelativeIntensity, 1f, this.ChannelData, true, true, true, TimescaleModes.Scaled, false, true);
		}

		// Token: 0x06001530 RID: 5424 RVA: 0x0005A541 File Offset: 0x00058741
		public override void AutomaticShakerSetup()
		{
		}

		// Token: 0x04001836 RID: 6198
		public static bool FeedbackTypeAuthorized = true;

		// Token: 0x04001837 RID: 6199
		[MMFInspectorGroup("Color Grading", true, 10, false, false)]
		[Tooltip("the duration of the shake, in seconds")]
		public float ShakeDuration = 1f;

		// Token: 0x04001838 RID: 6200
		[Tooltip("whether or not to add to the initial intensity")]
		public bool RelativeIntensity = true;

		// Token: 0x04001839 RID: 6201
		[Tooltip("whether or not to reset shaker values after shake")]
		public bool ResetShakerValuesAfterShake = true;

		// Token: 0x0400183A RID: 6202
		[Tooltip("whether or not to reset the target's values after shake")]
		public bool ResetTargetValuesAfterShake = true;

		// Token: 0x0400183B RID: 6203
		[MMFInspectorGroup("Red", true, 13, false, false)]
		[Tooltip("the curve used to animate the red value on")]
		public AnimationCurve ShakeRed = new AnimationCurve(new Keyframe[]
		{
			new Keyframe(0f, 0f),
			new Keyframe(0.5f, 1f),
			new Keyframe(1f, 0f)
		});

		// Token: 0x0400183C RID: 6204
		[Tooltip("the value to remap the curve's 0 to")]
		[Range(-200f, 200f)]
		public float RemapRedZero;

		// Token: 0x0400183D RID: 6205
		[Tooltip("the value to remap the curve's 1 to")]
		[Range(-200f, 200f)]
		public float RemapRedOne = 200f;

		// Token: 0x0400183E RID: 6206
		[MMFInspectorGroup("Green", true, 12, false, false)]
		[Tooltip("the curve used to animate the green value on")]
		public AnimationCurve ShakeGreen = new AnimationCurve(new Keyframe[]
		{
			new Keyframe(0f, 0f),
			new Keyframe(0.5f, 1f),
			new Keyframe(1f, 0f)
		});

		// Token: 0x0400183F RID: 6207
		[Tooltip("the value to remap the curve's 0 to")]
		[Range(-200f, 200f)]
		public float RemapGreenZero;

		// Token: 0x04001840 RID: 6208
		[Tooltip("the value to remap the curve's 1 to")]
		[Range(-200f, 200f)]
		public float RemapGreenOne = 200f;

		// Token: 0x04001841 RID: 6209
		[MMFInspectorGroup("Blue", true, 11, false, false)]
		[Tooltip("the curve used to animate the blue value on")]
		public AnimationCurve ShakeBlue = new AnimationCurve(new Keyframe[]
		{
			new Keyframe(0f, 0f),
			new Keyframe(0.5f, 1f),
			new Keyframe(1f, 0f)
		});

		// Token: 0x04001842 RID: 6210
		[Tooltip("the value to remap the curve's 0 to")]
		[Range(-200f, 200f)]
		public float RemapBlueZero;

		// Token: 0x04001843 RID: 6211
		[Tooltip("the value to remap the curve's 1 to")]
		[Range(-200f, 200f)]
		public float RemapBlueOne = 200f;
	}
}
