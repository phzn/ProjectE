using System;
using Lofelt.NiceVibrations;
using MoreMountains.Feedbacks;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

namespace MoreMountains.FeedbacksForThirdParty
{
	// Token: 0x0200031A RID: 794
	[AddComponentMenu("")]
	[FeedbackPath("Haptics/Haptic Preset")]
	[MovedFrom(false, null, "MoreMountains.Feedbacks.NiceVibrations", null)]
	[FeedbackHelp("Use this feedback to play a preset haptic, limited but super simple predifined haptic patterns")]
	public class MMF_NVPreset : MMF_Feedback
	{
		// Token: 0x06001603 RID: 5635 RVA: 0x0005D7DE File Offset: 0x0005B9DE
		protected override void CustomPlayFeedback(Vector3 position, float feedbacksIntensity = 1f)
		{
			if (!this.Active || !MMF_NVPreset.FeedbackTypeAuthorized || !this.HapticSettings.CanPlay())
			{
				return;
			}
			this.HapticSettings.SetGamepad();
			HapticPatterns.PlayPreset(this.Preset);
		}

		// Token: 0x0400194C RID: 6476
		public static bool FeedbackTypeAuthorized = true;

		// Token: 0x0400194D RID: 6477
		[MMFInspectorGroup("Haptic Preset", true, 21, false, false)]
		[Tooltip("the preset to play with this feedback")]
		public HapticPatterns.PresetType Preset = HapticPatterns.PresetType.LightImpact;

		// Token: 0x0400194E RID: 6478
		[MMFInspectorGroup("Settings", true, 16, false, false)]
		[Tooltip("a set of settings you can tweak to specify how and when exactly this haptic should play")]
		public MMFeedbackNVSettings HapticSettings;
	}
}
