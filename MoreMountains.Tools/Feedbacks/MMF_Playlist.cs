using System;
using MoreMountains.Tools;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

namespace MoreMountains.Feedbacks
{
	// Token: 0x020002A9 RID: 681
	[AddComponentMenu("")]
	[FeedbackHelp("This feedback will let you pilot a MMPlaylist")]
	[MovedFrom(false, null, "MoreMountains.Feedbacks.MMTools", null)]
	[FeedbackPath("Audio/MMPlaylist")]
	public class MMF_Playlist : MMF_Feedback
	{
		// Token: 0x0600131A RID: 4890 RVA: 0x0005293C File Offset: 0x00050B3C
		protected override void CustomPlayFeedback(Vector3 position, float feedbacksIntensity = 1f)
		{
			if (!this.Active || !MMF_Playlist.FeedbackTypeAuthorized)
			{
				return;
			}
			switch (this.Mode)
			{
			case MMF_Playlist.Modes.Play:
				MMPlaylistPlayEvent.Trigger(this.Channel);
				return;
			case MMF_Playlist.Modes.PlayNext:
				MMPlaylistPlayNextEvent.Trigger(this.Channel);
				return;
			case MMF_Playlist.Modes.PlayPrevious:
				MMPlaylistPlayPreviousEvent.Trigger(this.Channel);
				return;
			case MMF_Playlist.Modes.Stop:
				MMPlaylistStopEvent.Trigger(this.Channel);
				return;
			case MMF_Playlist.Modes.Pause:
				MMPlaylistPauseEvent.Trigger(this.Channel);
				return;
			case MMF_Playlist.Modes.PlaySongAt:
				MMPlaylistPlayIndexEvent.Trigger(this.Channel, this.SongIndex);
				return;
			case MMF_Playlist.Modes.SetVolumeMultiplier:
				MMPlaylistVolumeMultiplierEvent.Trigger(this.Channel, this.VolumeMultiplier, this.ApplyVolumeMultiplierInstantly);
				return;
			case MMF_Playlist.Modes.ChangePlaylist:
				MMPlaylistChangeEvent.Trigger(this.Channel, this.NewPlaylist, this.ChangePlaylistAndPlay);
				return;
			default:
				throw new ArgumentOutOfRangeException();
			}
		}

		// Token: 0x04001609 RID: 5641
		public static bool FeedbackTypeAuthorized = true;

		// Token: 0x0400160A RID: 5642
		[MMFInspectorGroup("MMPlaylist", true, 13, false, false)]
		[Tooltip("the action to call on the playlist")]
		public MMF_Playlist.Modes Mode = MMF_Playlist.Modes.PlayNext;

		// Token: 0x0400160B RID: 5643
		[Tooltip("the index of the song to play")]
		[MMEnumCondition("Mode", new int[] { 5 })]
		public int SongIndex;

		// Token: 0x0400160C RID: 5644
		[Tooltip("the volume multiplier to apply")]
		[MMEnumCondition("Mode", new int[] { 6 })]
		public float VolumeMultiplier = 1f;

		// Token: 0x0400160D RID: 5645
		[Tooltip("whether to apply the volume multiplier instantly (true) or only when the next song starts playing (false)")]
		[MMEnumCondition("Mode", new int[] { 6 })]
		public bool ApplyVolumeMultiplierInstantly;

		// Token: 0x0400160E RID: 5646
		[Tooltip("in change playlist mode, the playlist to which to switch to. Only works with MMSMPlaylistManager")]
		[MMEnumCondition("Mode", new int[] { 7 })]
		public MMSMPlaylist NewPlaylist;

		// Token: 0x0400160F RID: 5647
		[Tooltip("in change playlist mode, whether or not to play the new playlist after the switch. Only works with MMSMPlaylistManager")]
		[MMEnumCondition("Mode", new int[] { 7 })]
		public bool ChangePlaylistAndPlay = true;

		// Token: 0x04001610 RID: 5648
		protected Coroutine _coroutine;

		// Token: 0x0200053C RID: 1340
		public enum Modes
		{
			// Token: 0x04002409 RID: 9225
			Play,
			// Token: 0x0400240A RID: 9226
			PlayNext,
			// Token: 0x0400240B RID: 9227
			PlayPrevious,
			// Token: 0x0400240C RID: 9228
			Stop,
			// Token: 0x0400240D RID: 9229
			Pause,
			// Token: 0x0400240E RID: 9230
			PlaySongAt,
			// Token: 0x0400240F RID: 9231
			SetVolumeMultiplier,
			// Token: 0x04002410 RID: 9232
			ChangePlaylist
		}
	}
}
