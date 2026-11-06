using System;
using MoreMountains.Tools;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

namespace MoreMountains.Feedbacks
{
	// Token: 0x020002A8 RID: 680
	[AddComponentMenu("")]
	[MovedFrom(false, null, "MoreMountains.Feedbacks.MMTools", null)]
	[FeedbackPath("Audio/MMSoundManager Track Fade")]
	[FeedbackHelp("This feedback will let you fade all the sounds on a specific track at once. You will need a MMSoundManager in your scene for this to work.")]
	public class MMF_MMSoundManagerTrackFade : MMF_Feedback
	{
		// Token: 0x17000207 RID: 519
		// (get) Token: 0x06001315 RID: 4885 RVA: 0x0005289F File Offset: 0x00050A9F
		public override float FeedbackDuration
		{
			get
			{
				return this.FadeDuration;
			}
		}

		// Token: 0x06001316 RID: 4886 RVA: 0x000528A7 File Offset: 0x00050AA7
		protected override void CustomPlayFeedback(Vector3 position, float feedbacksIntensity = 1f)
		{
			if (!this.Active || !MMF_MMSoundManagerTrackFade.FeedbackTypeAuthorized)
			{
				return;
			}
			MMSoundManagerTrackFadeEvent.Trigger(MMSoundManagerTrackFadeEvent.Modes.PlayFade, this.Track, this.FadeDuration, this.FinalVolume, this.FadeTween);
		}

		// Token: 0x06001317 RID: 4887 RVA: 0x000528D7 File Offset: 0x00050AD7
		protected override void CustomStopFeedback(Vector3 position, float feedbacksIntensity = 1f)
		{
			if (!this.Active || !MMF_MMSoundManagerTrackFade.FeedbackTypeAuthorized)
			{
				return;
			}
			MMSoundManagerTrackFadeEvent.Trigger(MMSoundManagerTrackFadeEvent.Modes.StopFade, this.Track, this.FadeDuration, this.FinalVolume, this.FadeTween);
		}

		// Token: 0x04001604 RID: 5636
		public static bool FeedbackTypeAuthorized = true;

		// Token: 0x04001605 RID: 5637
		[MMFInspectorGroup("MMSoundManager Track Fade", true, 30, false, false)]
		[Tooltip("the track to fade the volume on")]
		public MMSoundManager.MMSoundManagerTracks Track;

		// Token: 0x04001606 RID: 5638
		[Tooltip("the duration of the fade, in seconds")]
		public float FadeDuration = 1f;

		// Token: 0x04001607 RID: 5639
		[Tooltip("the volume to reach at the end of the fade")]
		[Range(0.0001f, 10f)]
		public float FinalVolume = 0.0001f;

		// Token: 0x04001608 RID: 5640
		[Tooltip("the tween to operate the fade on")]
		public MMTweenType FadeTween = new MMTweenType(MMTween.MMTweenCurve.EaseInOutQuartic);
	}
}
