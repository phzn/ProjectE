using System;
using System.Collections;
using Lofelt.NiceVibrations;
using MoreMountains.Feedbacks;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

namespace MoreMountains.FeedbacksForThirdParty
{
	// Token: 0x02000317 RID: 791
	[AddComponentMenu("")]
	[FeedbackPath("Haptics/Haptic Continuous")]
	[MovedFrom(false, null, "MoreMountains.Feedbacks.NiceVibrations", null)]
	[FeedbackHelp("Add this feedback to play a continuous haptic of the specified amplitude and frequency over a certain duration. This feedback will also let you randomize these, and modulate them over time.")]
	public class MMF_NVContinuous : MMF_Feedback
	{
		// Token: 0x17000281 RID: 641
		// (get) Token: 0x060015F6 RID: 5622 RVA: 0x0005D497 File Offset: 0x0005B697
		// (set) Token: 0x060015F7 RID: 5623 RVA: 0x0005D4A5 File Offset: 0x0005B6A5
		public override float FeedbackDuration
		{
			get
			{
				return this.ApplyTimeMultiplier(this._duration);
			}
			set
			{
				this._duration = value;
			}
		}

		// Token: 0x060015F8 RID: 5624 RVA: 0x0005D4B0 File Offset: 0x0005B6B0
		protected override void CustomPlayFeedback(Vector3 position, float feedbacksIntensity = 1f)
		{
			if (!this.Active || !MMF_NVContinuous.FeedbackTypeAuthorized || !this.HapticSettings.CanPlay())
			{
				return;
			}
			float num = Random.Range(this.MinAmplitude, this.MaxAmplitude);
			float num2 = Random.Range(this.MinFrequency, this.MaxFrequency);
			this._duration = Random.Range(this.MinDuration, this.MaxDuration);
			this.HapticSettings.SetGamepad();
			HapticPatterns.PlayConstant(num, num2, this.FeedbackDuration);
			if (this.UseRealTimeModulation)
			{
				this._coroutine = this.Owner.StartCoroutine(this.RealtimeModulationCo());
			}
		}

		// Token: 0x060015F9 RID: 5625 RVA: 0x0005D54A File Offset: 0x0005B74A
		protected virtual IEnumerator RealtimeModulationCo()
		{
			this.IsPlaying = true;
			float journey = (this.NormalPlayDirection ? 0f : this.FeedbackDuration);
			while (journey >= 0f && journey <= this.FeedbackDuration && this.FeedbackDuration > 0f)
			{
				float num = MMFeedbacksHelpers.Remap(journey, 0f, this.FeedbackDuration, 0f, 1f);
				HapticController.clipLevel = this.AmplitudeMultiplication.Evaluate(num);
				HapticController.clipFrequencyShift = this.ShiftFrequency.Evaluate(num);
				journey += (this.NormalPlayDirection ? this.FeedbackDeltaTime : (-this.FeedbackDeltaTime));
				yield return null;
			}
			HapticController.clipLevel = this.AmplitudeMultiplication.Evaluate(this.FinalNormalizedTime);
			HapticController.clipFrequencyShift = this.ShiftFrequency.Evaluate(this.FinalNormalizedTime);
			this.IsPlaying = false;
			this._coroutine = null;
			yield return null;
			yield break;
		}

		// Token: 0x060015FA RID: 5626 RVA: 0x0005D55C File Offset: 0x0005B75C
		protected override void CustomStopFeedback(Vector3 position, float feedbacksIntensity = 1f)
		{
			if (!MMF_NVContinuous.FeedbackTypeAuthorized)
			{
				return;
			}
			base.CustomStopFeedback(position, feedbacksIntensity);
			this.IsPlaying = false;
			HapticController.Stop();
			if (this.Active && this._coroutine != null)
			{
				this.Owner.StopCoroutine(this._coroutine);
				this._coroutine = null;
			}
		}

		// Token: 0x04001936 RID: 6454
		public static bool FeedbackTypeAuthorized = true;

		// Token: 0x04001937 RID: 6455
		[MMFInspectorGroup("Haptic Amplitude", true, 31, false, false)]
		[Tooltip("the minimum amplitude at which this clip should play (amplitude will be randomized between MinAmplitude and MaxAmplitude)")]
		[Range(0f, 1f)]
		public float MinAmplitude = 1f;

		// Token: 0x04001938 RID: 6456
		[Tooltip("the maximum amplitude at which this clip should play (amplitude will be randomized between MinAmplitude and MaxAmplitude)")]
		[Range(0f, 1f)]
		public float MaxAmplitude = 1f;

		// Token: 0x04001939 RID: 6457
		[MMFInspectorGroup("Haptic Frequency", true, 32, false, false)]
		[Tooltip("the minimum frequency at which this clip should play (frequency will be randomized between MinFrequency and MaxFrequency)")]
		[Range(0f, 1f)]
		public float MinFrequency = 1f;

		// Token: 0x0400193A RID: 6458
		[Tooltip("the maximum frequency at which this clip should play (frequency will be randomized between MinFrequency and MaxFrequency)")]
		[Range(0f, 1f)]
		public float MaxFrequency = 1f;

		// Token: 0x0400193B RID: 6459
		[MMFInspectorGroup("Duration", true, 33, false, false)]
		[Tooltip("the minimum duration at which this clip should play (duration will be randomized between MinDuration and MaxDuration)")]
		public float MinDuration = 1f;

		// Token: 0x0400193C RID: 6460
		[Tooltip("the maximum duration at which this clip should play (duration will be randomized between MinDuration and MaxDuration)")]
		public float MaxDuration = 1f;

		// Token: 0x0400193D RID: 6461
		[MMFInspectorGroup("Real-time Modulation", true, 34, false, false)]
		[Tooltip("whether or not to modulate the haptic signal at runtime")]
		public bool UseRealTimeModulation;

		// Token: 0x0400193E RID: 6462
		[Tooltip("if UseRealTimeModulation:true, the curve along which to modulate amplitude for this continuous haptic, over its total duration")]
		[MMFCondition("UseRealTimeModulation", true)]
		public AnimationCurve AmplitudeMultiplication = new AnimationCurve(new Keyframe[]
		{
			new Keyframe(0f, 0f),
			new Keyframe(0.5f, 1f),
			new Keyframe(1f, 0f)
		});

		// Token: 0x0400193F RID: 6463
		[Tooltip("if UseRealTimeModulation:true, the curve along which to modulate frequency for this continuous haptic, over its total duration")]
		[MMFCondition("UseRealTimeModulation", true)]
		public AnimationCurve ShiftFrequency = new AnimationCurve(new Keyframe[]
		{
			new Keyframe(0f, 0f),
			new Keyframe(0.5f, 1f),
			new Keyframe(1f, 0f)
		});

		// Token: 0x04001940 RID: 6464
		[MMFInspectorGroup("Settings", true, 16, false, false)]
		[Tooltip("a set of settings you can tweak to specify how and when exactly this haptic should play")]
		public MMFeedbackNVSettings HapticSettings;

		// Token: 0x04001941 RID: 6465
		protected Coroutine _coroutine;

		// Token: 0x04001942 RID: 6466
		protected float _duration;
	}
}
