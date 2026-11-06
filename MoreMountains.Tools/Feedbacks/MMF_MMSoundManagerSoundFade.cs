using System;
using MoreMountains.Tools;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

namespace MoreMountains.Feedbacks
{
	// Token: 0x020002A6 RID: 678
	[AddComponentMenu("")]
	[MovedFrom(false, null, "MoreMountains.Feedbacks.MMTools", null)]
	[FeedbackPath("Audio/MMSoundManager Sound Fade")]
	[FeedbackHelp("This feedback lets you trigger fades on a specific sound via the MMSoundManager. You will need a MMSoundManager in your scene for this to work.")]
	public class MMF_MMSoundManagerSoundFade : MMF_Feedback
	{
		// Token: 0x0600130E RID: 4878 RVA: 0x00052723 File Offset: 0x00050923
		protected override void CustomPlayFeedback(Vector3 position, float feedbacksIntensity = 1f)
		{
			if (!this.Active || !MMF_MMSoundManagerSoundFade.FeedbackTypeAuthorized)
			{
				return;
			}
			MMSoundManagerSoundFadeEvent.Trigger(MMSoundManagerSoundFadeEvent.Modes.PlayFade, this.SoundID, this.FadeDuration, this.FinalVolume, this.FadeTween);
		}

		// Token: 0x0600130F RID: 4879 RVA: 0x00052753 File Offset: 0x00050953
		protected override void CustomStopFeedback(Vector3 position, float feedbacksIntensity = 1f)
		{
			if (!this.Active || !MMF_MMSoundManagerSoundFade.FeedbackTypeAuthorized)
			{
				return;
			}
			MMSoundManagerSoundFadeEvent.Trigger(MMSoundManagerSoundFadeEvent.Modes.StopFade, this.SoundID, this.FadeDuration, this.FinalVolume, this.FadeTween);
		}

		// Token: 0x040015FA RID: 5626
		public static bool FeedbackTypeAuthorized = true;

		// Token: 0x040015FB RID: 5627
		[MMFInspectorGroup("MMSoundManager Sound Fade", true, 30, false, false)]
		[Tooltip("the ID of the sound you want to fade. Has to match the ID you specified when playing the sound initially")]
		public int SoundID;

		// Token: 0x040015FC RID: 5628
		[Tooltip("the duration of the fade, in seconds")]
		public float FadeDuration = 1f;

		// Token: 0x040015FD RID: 5629
		[Tooltip("the volume towards which to fade")]
		[Range(0.0001f, 10f)]
		public float FinalVolume = 0.0001f;

		// Token: 0x040015FE RID: 5630
		[Tooltip("the tween to apply over the fade")]
		public MMTweenType FadeTween = new MMTweenType(MMTween.MMTweenCurve.EaseInOutQuartic);

		// Token: 0x040015FF RID: 5631
		protected AudioSource _targetAudioSource;
	}
}
