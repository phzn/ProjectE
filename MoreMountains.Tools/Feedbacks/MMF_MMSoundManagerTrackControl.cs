using System;
using MoreMountains.Tools;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

namespace MoreMountains.Feedbacks
{
	// Token: 0x020002A7 RID: 679
	[AddComponentMenu("")]
	[MovedFrom(false, null, "MoreMountains.Feedbacks.MMTools", null)]
	[FeedbackPath("Audio/MMSoundManager Track Control")]
	[FeedbackHelp("This feedback will let you control all sounds playing on a specific track (master, UI, music, sfx), and play, pause, mute, unmute, resume, stop, free them all at once. You will need a MMSoundManager in your scene for this to work.")]
	public class MMF_MMSoundManagerTrackControl : MMF_Feedback
	{
		// Token: 0x06001312 RID: 4882 RVA: 0x000527B8 File Offset: 0x000509B8
		protected override void CustomPlayFeedback(Vector3 position, float feedbacksIntensity = 1f)
		{
			if (!this.Active || !MMF_MMSoundManagerTrackControl.FeedbackTypeAuthorized)
			{
				return;
			}
			switch (this.ControlMode)
			{
			case MMF_MMSoundManagerTrackControl.ControlModes.Mute:
				MMSoundManagerTrackEvent.Trigger(MMSoundManagerTrackEventTypes.MuteTrack, this.Track, 1f);
				return;
			case MMF_MMSoundManagerTrackControl.ControlModes.UnMute:
				MMSoundManagerTrackEvent.Trigger(MMSoundManagerTrackEventTypes.UnmuteTrack, this.Track, 1f);
				return;
			case MMF_MMSoundManagerTrackControl.ControlModes.SetVolume:
				MMSoundManagerTrackEvent.Trigger(MMSoundManagerTrackEventTypes.SetVolumeTrack, this.Track, this.Volume);
				return;
			case MMF_MMSoundManagerTrackControl.ControlModes.Pause:
				MMSoundManagerTrackEvent.Trigger(MMSoundManagerTrackEventTypes.PauseTrack, this.Track, 1f);
				return;
			case MMF_MMSoundManagerTrackControl.ControlModes.Play:
				MMSoundManagerTrackEvent.Trigger(MMSoundManagerTrackEventTypes.PlayTrack, this.Track, 1f);
				return;
			case MMF_MMSoundManagerTrackControl.ControlModes.Stop:
				MMSoundManagerTrackEvent.Trigger(MMSoundManagerTrackEventTypes.StopTrack, this.Track, 1f);
				return;
			case MMF_MMSoundManagerTrackControl.ControlModes.Free:
				MMSoundManagerTrackEvent.Trigger(MMSoundManagerTrackEventTypes.FreeTrack, this.Track, 1f);
				return;
			default:
				return;
			}
		}

		// Token: 0x04001600 RID: 5632
		public static bool FeedbackTypeAuthorized = true;

		// Token: 0x04001601 RID: 5633
		[MMFInspectorGroup("MMSoundManager Track Control", true, 30, false, false)]
		[Tooltip("the track to mute/unmute/pause/play/stop/free/etc")]
		public MMSoundManager.MMSoundManagerTracks Track;

		// Token: 0x04001602 RID: 5634
		[Tooltip("the selected control mode to interact with the track. Free will stop all sounds and return them to the pool")]
		public MMF_MMSoundManagerTrackControl.ControlModes ControlMode = MMF_MMSoundManagerTrackControl.ControlModes.Pause;

		// Token: 0x04001603 RID: 5635
		[Tooltip("if setting the volume, the volume to assign to the track")]
		[MMEnumCondition("ControlMode", new int[] { 2 })]
		public float Volume = 0.5f;

		// Token: 0x0200053B RID: 1339
		public enum ControlModes
		{
			// Token: 0x04002401 RID: 9217
			Mute,
			// Token: 0x04002402 RID: 9218
			UnMute,
			// Token: 0x04002403 RID: 9219
			SetVolume,
			// Token: 0x04002404 RID: 9220
			Pause,
			// Token: 0x04002405 RID: 9221
			Play,
			// Token: 0x04002406 RID: 9222
			Stop,
			// Token: 0x04002407 RID: 9223
			Free
		}
	}
}
