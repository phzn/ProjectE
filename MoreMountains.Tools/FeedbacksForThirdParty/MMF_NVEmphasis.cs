using System;
using Lofelt.NiceVibrations;
using MoreMountains.Feedbacks;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

namespace MoreMountains.FeedbacksForThirdParty
{
	// Token: 0x02000319 RID: 793
	[AddComponentMenu("")]
	[FeedbackPath("Haptics/Haptic Emphasis")]
	[MovedFrom(false, null, "MoreMountains.Feedbacks.NiceVibrations", null)]
	[FeedbackHelp("Use this feedback to play an Emphasis haptics, short haptic bursts whose amplitude and frequency can be controlled in real time, also called Transients in CoreHaptics/iOS")]
	public class MMF_NVEmphasis : MMF_Feedback
	{
		// Token: 0x06001600 RID: 5632 RVA: 0x0005D744 File Offset: 0x0005B944
		protected override void CustomPlayFeedback(Vector3 position, float feedbacksIntensity = 1f)
		{
			if (!this.Active || !MMF_NVEmphasis.FeedbackTypeAuthorized || !this.HapticSettings.CanPlay())
			{
				return;
			}
			float num = Random.Range(this.MinAmplitude, this.MaxAmplitude);
			float num2 = Random.Range(this.MinFrequency, this.MaxFrequency);
			this.HapticSettings.SetGamepad();
			HapticPatterns.PlayEmphasis(num, num2);
		}

		// Token: 0x04001946 RID: 6470
		public static bool FeedbackTypeAuthorized = true;

		// Token: 0x04001947 RID: 6471
		[MMFInspectorGroup("Haptic Amplitude", true, 23, false, false)]
		[Tooltip("the minimum amplitude at which this clip should play (amplitude will be randomized between MinAmplitude and MaxAmplitude)")]
		[Range(0f, 1f)]
		public float MinAmplitude = 1f;

		// Token: 0x04001948 RID: 6472
		[Tooltip("the maximum amplitude at which this clip should play (amplitude will be randomized between MinAmplitude and MaxAmplitude)")]
		[Range(0f, 1f)]
		public float MaxAmplitude = 1f;

		// Token: 0x04001949 RID: 6473
		[MMFInspectorGroup("Haptic Frequency", true, 22, false, false)]
		[Tooltip("the minimum frequency at which this clip should play (frequency will be randomized between MinFrequency and MaxFrequency)")]
		[Range(0f, 1f)]
		public float MinFrequency = 1f;

		// Token: 0x0400194A RID: 6474
		[Tooltip("the maximum frequency at which this clip should play (frequency will be randomized between MinFrequency and MaxFrequency)")]
		[Range(0f, 1f)]
		public float MaxFrequency = 1f;

		// Token: 0x0400194B RID: 6475
		[MMFInspectorGroup("Settings", true, 16, false, false)]
		[Tooltip("a set of settings you can tweak to specify how and when exactly this haptic should play")]
		public MMFeedbackNVSettings HapticSettings;
	}
}
