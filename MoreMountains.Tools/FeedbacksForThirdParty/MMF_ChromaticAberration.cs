using System;
using MoreMountains.Feedbacks;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

namespace MoreMountains.FeedbacksForThirdParty
{
	// Token: 0x0200031C RID: 796
	[AddComponentMenu("")]
	[MovedFrom(false, null, "MoreMountains.Feedbacks.PostProcessing", null)]
	[FeedbackHelp("This feedback allows you to control chromatic aberration intensity over time. It requires you have in your scene an object with a PostProcessVolume with Chromatic Aberration active, and a MMChromaticAberrationShaker component.")]
	public class MMF_ChromaticAberration : MMF_Feedback
	{
		// Token: 0x17000285 RID: 645
		// (get) Token: 0x06001610 RID: 5648 RVA: 0x0005DA78 File Offset: 0x0005BC78
		// (set) Token: 0x06001611 RID: 5649 RVA: 0x0005DA86 File Offset: 0x0005BC86
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

		// Token: 0x17000286 RID: 646
		// (get) Token: 0x06001612 RID: 5650 RVA: 0x0005DA8F File Offset: 0x0005BC8F
		public override bool HasChannel
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000287 RID: 647
		// (get) Token: 0x06001613 RID: 5651 RVA: 0x0005DA92 File Offset: 0x0005BC92
		public override bool HasRandomness
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06001614 RID: 5652 RVA: 0x0005DA98 File Offset: 0x0005BC98
		protected override void CustomPlayFeedback(Vector3 position, float feedbacksIntensity = 1f)
		{
			if (!this.Active || !MMF_ChromaticAberration.FeedbackTypeAuthorized)
			{
				return;
			}
			float num = this.ComputeIntensity(feedbacksIntensity, position);
			MMChromaticAberrationShakeEvent.Trigger(this.Intensity, this.FeedbackDuration, this.RemapIntensityZero, this.RemapIntensityOne, this.RelativeIntensity, num, this.ChannelData, this.ResetShakerValuesAfterShake, this.ResetTargetValuesAfterShake, this.NormalPlayDirection, this.ComputedTimescaleMode, false, false);
		}

		// Token: 0x06001615 RID: 5653 RVA: 0x0005DB04 File Offset: 0x0005BD04
		protected override void CustomStopFeedback(Vector3 position, float feedbacksIntensity = 1f)
		{
			if (!this.Active || !MMF_ChromaticAberration.FeedbackTypeAuthorized)
			{
				return;
			}
			base.CustomStopFeedback(position, feedbacksIntensity);
			MMChromaticAberrationShakeEvent.Trigger(this.Intensity, this.FeedbackDuration, this.RemapIntensityZero, this.RemapIntensityOne, this.RelativeIntensity, 1f, null, true, true, true, TimescaleModes.Scaled, true, false);
		}

		// Token: 0x06001616 RID: 5654 RVA: 0x0005DB58 File Offset: 0x0005BD58
		protected override void CustomRestoreInitialValues()
		{
			if (!this.Active || !MMF_ChromaticAberration.FeedbackTypeAuthorized)
			{
				return;
			}
			MMChromaticAberrationShakeEvent.Trigger(this.Intensity, this.FeedbackDuration, this.RemapIntensityZero, this.RemapIntensityOne, this.RelativeIntensity, 1f, null, true, true, true, TimescaleModes.Scaled, false, true);
		}

		// Token: 0x06001617 RID: 5655 RVA: 0x0005DBA4 File Offset: 0x0005BDA4
		public override void AutomaticShakerSetup()
		{
		}

		// Token: 0x0400195A RID: 6490
		public static bool FeedbackTypeAuthorized = true;

		// Token: 0x0400195B RID: 6491
		[MMFInspectorGroup("Chromatic Aberration", true, 44, false, false)]
		[Tooltip("the duration of the shake, in seconds")]
		public float Duration = 0.2f;

		// Token: 0x0400195C RID: 6492
		[Tooltip("whether or not to reset shaker values after shake")]
		public bool ResetShakerValuesAfterShake = true;

		// Token: 0x0400195D RID: 6493
		[Tooltip("whether or not to reset the target's values after shake")]
		public bool ResetTargetValuesAfterShake = true;

		// Token: 0x0400195E RID: 6494
		[Tooltip("the value to remap the curve's 0 to")]
		[Range(0f, 1f)]
		public float RemapIntensityZero;

		// Token: 0x0400195F RID: 6495
		[Tooltip("the value to remap the curve's 1 to")]
		[Range(0f, 1f)]
		public float RemapIntensityOne = 1f;

		// Token: 0x04001960 RID: 6496
		[MMFInspectorGroup("Intensity", true, 45, false, false)]
		[Tooltip("the curve to animate the intensity on")]
		public AnimationCurve Intensity = new AnimationCurve(new Keyframe[]
		{
			new Keyframe(0f, 0f),
			new Keyframe(0.5f, 1f),
			new Keyframe(1f, 0f)
		});

		// Token: 0x04001961 RID: 6497
		[Tooltip("the multiplier to apply to the intensity curve")]
		[Range(0f, 1f)]
		public float Amplitude = 1f;

		// Token: 0x04001962 RID: 6498
		[Tooltip("whether or not to add to the initial intensity")]
		public bool RelativeIntensity;
	}
}
