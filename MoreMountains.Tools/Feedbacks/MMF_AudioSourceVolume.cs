using System;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

namespace MoreMountains.Feedbacks
{
	// Token: 0x020001D8 RID: 472
	[AddComponentMenu("")]
	[MovedFrom(false, null, "MoreMountains.Feedbacks", null)]
	[FeedbackPath("Audio/AudioSource Volume")]
	[FeedbackHelp("This feedback lets you control the volume of a target AudioSource over time.")]
	public class MMF_AudioSourceVolume : MMF_Feedback
	{
		// Token: 0x170000FC RID: 252
		// (get) Token: 0x06000BFD RID: 3069 RVA: 0x000345DC File Offset: 0x000327DC
		// (set) Token: 0x06000BFE RID: 3070 RVA: 0x000345EA File Offset: 0x000327EA
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

		// Token: 0x170000FD RID: 253
		// (get) Token: 0x06000BFF RID: 3071 RVA: 0x000345F3 File Offset: 0x000327F3
		public override bool HasChannel
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170000FE RID: 254
		// (get) Token: 0x06000C00 RID: 3072 RVA: 0x000345F6 File Offset: 0x000327F6
		public override bool HasRandomness
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06000C01 RID: 3073 RVA: 0x000345FC File Offset: 0x000327FC
		protected override void CustomPlayFeedback(Vector3 position, float feedbacksIntensity = 1f)
		{
			if (!this.Active || !MMF_AudioSourceVolume.FeedbackTypeAuthorized)
			{
				return;
			}
			float num = this.ComputeIntensity(feedbacksIntensity, position);
			MMAudioSourceVolumeShakeEvent.Trigger(this.VolumeTween, this.FeedbackDuration, this.RemapVolumeZero, this.RemapVolumeOne, this.RelativeVolume, num, this.ChannelData, this.ResetShakerValuesAfterShake, this.ResetTargetValuesAfterShake, this.NormalPlayDirection, this.ComputedTimescaleMode, false, false);
		}

		// Token: 0x06000C02 RID: 3074 RVA: 0x00034668 File Offset: 0x00032868
		protected override void CustomStopFeedback(Vector3 position, float feedbacksIntensity = 1f)
		{
			if (!this.Active || !MMF_AudioSourceVolume.FeedbackTypeAuthorized)
			{
				return;
			}
			base.CustomStopFeedback(position, feedbacksIntensity);
			MMAudioSourceVolumeShakeEvent.Trigger(this.VolumeTween, this.FeedbackDuration, this.RemapVolumeZero, this.RemapVolumeOne, false, 1f, null, true, true, true, TimescaleModes.Scaled, true, false);
		}

		// Token: 0x06000C03 RID: 3075 RVA: 0x000346B8 File Offset: 0x000328B8
		protected override void CustomRestoreInitialValues()
		{
			if (!this.Active || !MMF_AudioSourceVolume.FeedbackTypeAuthorized)
			{
				return;
			}
			MMAudioSourceVolumeShakeEvent.Trigger(this.VolumeTween, this.FeedbackDuration, this.RemapVolumeZero, this.RemapVolumeOne, false, 1f, null, true, true, true, TimescaleModes.Scaled, false, true);
		}

		// Token: 0x04000DB4 RID: 3508
		public static bool FeedbackTypeAuthorized = true;

		// Token: 0x04000DB5 RID: 3509
		[MMFInspectorGroup("AudioSource Volume", true, 87, false, false)]
		[Tooltip("the duration of the shake, in seconds")]
		public float Duration = 2f;

		// Token: 0x04000DB6 RID: 3510
		[Tooltip("whether or not to reset shaker values after shake")]
		public bool ResetShakerValuesAfterShake = true;

		// Token: 0x04000DB7 RID: 3511
		[Tooltip("whether or not to reset the target's values after shake")]
		public bool ResetTargetValuesAfterShake = true;

		// Token: 0x04000DB8 RID: 3512
		[Tooltip("whether or not to add to the initial value")]
		public bool RelativeVolume;

		// Token: 0x04000DB9 RID: 3513
		[Tooltip("the curve used to animate the intensity value on")]
		public AnimationCurve VolumeTween = new AnimationCurve(new Keyframe[]
		{
			new Keyframe(0f, 1f),
			new Keyframe(0.5f, 0f),
			new Keyframe(1f, 1f)
		});

		// Token: 0x04000DBA RID: 3514
		[Range(-1f, 1f)]
		[Tooltip("the value to remap the curve's 0 to")]
		public float RemapVolumeZero;

		// Token: 0x04000DBB RID: 3515
		[Range(-1f, 1f)]
		[Tooltip("the value to remap the curve's 1 to")]
		public float RemapVolumeOne = 1f;
	}
}
