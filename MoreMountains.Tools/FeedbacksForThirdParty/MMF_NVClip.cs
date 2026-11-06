using System;
using Lofelt.NiceVibrations;
using MoreMountains.Feedbacks;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

namespace MoreMountains.FeedbacksForThirdParty
{
	// Token: 0x02000316 RID: 790
	[AddComponentMenu("")]
	[FeedbackPath("Haptics/Haptic Clip")]
	[MovedFrom(false, null, "MoreMountains.Feedbacks.NiceVibrations", null)]
	[FeedbackHelp("This feedback will let you play a haptic clip, and randomize its level and frequency.")]
	public class MMF_NVClip : MMF_Feedback
	{
		// Token: 0x060015F2 RID: 5618 RVA: 0x0005D3AC File Offset: 0x0005B5AC
		protected override void CustomPlayFeedback(Vector3 position, float feedbacksIntensity = 1f)
		{
			if (!this.Active || !MMF_NVClip.FeedbackTypeAuthorized || !this.HapticSettings.CanPlay() || this.Clip == null)
			{
				return;
			}
			HapticController.Load(this.Clip);
			this.HapticSettings.SetGamepad();
			HapticController.fallbackPreset = this.FallbackPreset;
			HapticController.Loop(this.Loop);
			HapticController.Seek(this.SeekTime);
			HapticController.clipLevel = Random.Range(this.MinLevel, this.MaxLevel);
			HapticController.clipFrequencyShift = Random.Range(this.MinFrequencyShift, this.MaxFrequencyShift);
			HapticController.Play();
		}

		// Token: 0x060015F3 RID: 5619 RVA: 0x0005D44C File Offset: 0x0005B64C
		protected override void CustomStopFeedback(Vector3 position, float feedbacksIntensity = 1f)
		{
			if (!MMF_NVClip.FeedbackTypeAuthorized)
			{
				return;
			}
			base.CustomStopFeedback(position, feedbacksIntensity);
			this.IsPlaying = false;
			HapticController.Stop();
		}

		// Token: 0x0400192C RID: 6444
		public static bool FeedbackTypeAuthorized = true;

		// Token: 0x0400192D RID: 6445
		[MMFInspectorGroup("Haptic Clip", true, 13, true, false)]
		[Tooltip("the haptic clip to play with this feedback")]
		public HapticClip Clip;

		// Token: 0x0400192E RID: 6446
		[Tooltip("a preset to play should the device you're running your game on doesn't support playing haptic clips")]
		public HapticPatterns.PresetType FallbackPreset = HapticPatterns.PresetType.LightImpact;

		// Token: 0x0400192F RID: 6447
		[Tooltip("whether or not this clip should play on a loop, until stopped (won't work on gamepads)")]
		public bool Loop;

		// Token: 0x04001930 RID: 6448
		[Tooltip("at what timestamp this clip should start playing")]
		public float SeekTime;

		// Token: 0x04001931 RID: 6449
		[MMFInspectorGroup("Level", true, 14, false, false)]
		[Tooltip("the minimum level at which this clip should play (level will be randomized between MinLevel and MaxLevel)")]
		[Range(0f, 5f)]
		public float MinLevel = 1f;

		// Token: 0x04001932 RID: 6450
		[Tooltip("the maximum level at which this clip should play (level will be randomized between MinLevel and MaxLevel)")]
		[Range(0f, 5f)]
		public float MaxLevel = 1f;

		// Token: 0x04001933 RID: 6451
		[MMFInspectorGroup("Frequency Shift", true, 15, false, false)]
		[Tooltip("the minimum frequency shift at which this clip should play (frequency shift will be randomized between MinFrequencyShift and MaxFrequencyShift)")]
		[Range(-1f, 1f)]
		public float MinFrequencyShift;

		// Token: 0x04001934 RID: 6452
		[Tooltip("the maximum frequency shift at which this clip should play (frequency shift will be randomized between MinFrequencyShift and MaxFrequencyShift)")]
		[Range(-1f, 1f)]
		public float MaxFrequencyShift;

		// Token: 0x04001935 RID: 6453
		[MMFInspectorGroup("Settings", true, 16, false, false)]
		[Tooltip("a set of settings you can tweak to specify how and when exactly this haptic should play")]
		public MMFeedbackNVSettings HapticSettings;
	}
}
