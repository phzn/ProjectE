using System;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Scripting.APIUpdating;

namespace MoreMountains.Feedbacks
{
	// Token: 0x020001D4 RID: 468
	[AddComponentMenu("")]
	[FeedbackHelp("This feedback will let you transition to a target AudioMixer Snapshot over a specified time")]
	[MovedFrom(false, null, "MoreMountains.Feedbacks", null)]
	[FeedbackPath("Audio/AudioMixer Snapshot Transition")]
	public class MMF_AudioMixerSnapshotTransition : MMF_Feedback
	{
		// Token: 0x06000BDE RID: 3038 RVA: 0x00033FE4 File Offset: 0x000321E4
		protected override void CustomPlayFeedback(Vector3 position, float feedbacksIntensity = 1f)
		{
			if (!this.Active || !MMF_AudioMixerSnapshotTransition.FeedbackTypeAuthorized)
			{
				return;
			}
			if (this.TargetSnapshot == null)
			{
				return;
			}
			if (this.NormalPlayDirection)
			{
				this.TargetSnapshot.TransitionTo(this.TransitionDuration);
				return;
			}
			if (this.OriginalSnapshot != null)
			{
				this.OriginalSnapshot.TransitionTo(this.TransitionDuration);
				return;
			}
			this.TargetSnapshot.TransitionTo(this.TransitionDuration);
		}

		// Token: 0x04000D95 RID: 3477
		public static bool FeedbackTypeAuthorized = true;

		// Token: 0x04000D96 RID: 3478
		[MMFInspectorGroup("AudioMixer Snapshot", true, 44, false, false)]
		[Tooltip("the target audio mixer snapshot we want to transition to")]
		public AudioMixerSnapshot TargetSnapshot;

		// Token: 0x04000D97 RID: 3479
		[Tooltip("the audio mixer snapshot we want to transition from, optional, only needed if you plan to play this feedback in reverse")]
		public AudioMixerSnapshot OriginalSnapshot;

		// Token: 0x04000D98 RID: 3480
		[Tooltip("the duration, in seconds, over which to transition to the selected snapshot")]
		public float TransitionDuration = 1f;
	}
}
