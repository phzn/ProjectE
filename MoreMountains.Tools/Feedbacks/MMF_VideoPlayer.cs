using System;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;
using UnityEngine.Video;

namespace MoreMountains.Feedbacks
{
	// Token: 0x02000217 RID: 535
	[AddComponentMenu("")]
	[FeedbackHelp("This feedback lets you control video players in all sorts of ways (Play, Pause, Toggle, Stop, Prepare, StepForward, StepBackward, SetPlaybackSpeed, SetDirectAudioVolume, SetDirectAudioMute, GoToFrame, ToggleLoop)")]
	[MovedFrom(false, null, "MoreMountains.Feedbacks", null)]
	[FeedbackPath("UI/Video Player")]
	public class MMF_VideoPlayer : MMF_Feedback
	{
		// Token: 0x17000194 RID: 404
		// (get) Token: 0x06000EAE RID: 3758 RVA: 0x00041645 File Offset: 0x0003F845
		public override bool HasAutomatedTargetAcquisition
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06000EAF RID: 3759 RVA: 0x00041648 File Offset: 0x0003F848
		protected override void AutomateTargetAcquisition()
		{
			this.TargetVideoPlayer = this.FindAutomatedTarget<VideoPlayer>();
		}

		// Token: 0x06000EB0 RID: 3760 RVA: 0x00041658 File Offset: 0x0003F858
		protected override void CustomPlayFeedback(Vector3 position, float feedbacksIntensity = 1f)
		{
			if (!this.Active || !MMF_VideoPlayer.FeedbackTypeAuthorized)
			{
				return;
			}
			if (this.TargetVideoPlayer == null)
			{
				return;
			}
			switch (this.VideoAction)
			{
			case MMF_VideoPlayer.VideoActions.Play:
				this.TargetVideoPlayer.Play();
				return;
			case MMF_VideoPlayer.VideoActions.Pause:
				this.TargetVideoPlayer.Pause();
				return;
			case MMF_VideoPlayer.VideoActions.Toggle:
				if (this.TargetVideoPlayer.isPlaying)
				{
					this.TargetVideoPlayer.Pause();
					return;
				}
				this.TargetVideoPlayer.Play();
				return;
			case MMF_VideoPlayer.VideoActions.Stop:
				this.TargetVideoPlayer.Stop();
				return;
			case MMF_VideoPlayer.VideoActions.Prepare:
				this.TargetVideoPlayer.Prepare();
				return;
			case MMF_VideoPlayer.VideoActions.StepForward:
				this.TargetVideoPlayer.StepForward();
				return;
			case MMF_VideoPlayer.VideoActions.StepBackward:
				this.TargetVideoPlayer.Pause();
				this.TargetVideoPlayer.frame = this.TargetVideoPlayer.frame - 1L;
				return;
			case MMF_VideoPlayer.VideoActions.SetPlaybackSpeed:
				this.TargetVideoPlayer.playbackSpeed = this.PlaybackSpeed;
				return;
			case MMF_VideoPlayer.VideoActions.SetDirectAudioVolume:
				this.TargetVideoPlayer.SetDirectAudioVolume((ushort)this.TrackIndex, this.Volume);
				return;
			case MMF_VideoPlayer.VideoActions.SetDirectAudioMute:
				this.TargetVideoPlayer.SetDirectAudioMute((ushort)this.TrackIndex, this.Mute);
				return;
			case MMF_VideoPlayer.VideoActions.GoToFrame:
				this.TargetVideoPlayer.frame = this.TargetFrame;
				return;
			case MMF_VideoPlayer.VideoActions.ToggleLoop:
				this.TargetVideoPlayer.isLooping = !this.TargetVideoPlayer.isLooping;
				return;
			default:
				return;
			}
		}

		// Token: 0x04001103 RID: 4355
		public static bool FeedbackTypeAuthorized = true;

		// Token: 0x04001104 RID: 4356
		[MMFInspectorGroup("Video Player", true, 58, true, false)]
		[Tooltip("the Video Player to control with this feedback")]
		public VideoPlayer TargetVideoPlayer;

		// Token: 0x04001105 RID: 4357
		[Tooltip("the Video Player to control with this feedback")]
		public MMF_VideoPlayer.VideoActions VideoAction = MMF_VideoPlayer.VideoActions.Pause;

		// Token: 0x04001106 RID: 4358
		[Tooltip("the frame at which to jump when in GoToFrame mode")]
		[MMFEnumCondition("VideoAction", new int[] { 10 })]
		public long TargetFrame = 10L;

		// Token: 0x04001107 RID: 4359
		[Tooltip("the new playback speed (between 0 and 10)")]
		[MMFEnumCondition("VideoAction", new int[] { 7 })]
		public float PlaybackSpeed = 2f;

		// Token: 0x04001108 RID: 4360
		[Tooltip("the track index on which to control volume")]
		[MMFEnumCondition("VideoAction", new int[] { 9, 8 })]
		public int TrackIndex;

		// Token: 0x04001109 RID: 4361
		[Tooltip("the new volume for the specified track, between 0 and 1")]
		[MMFEnumCondition("VideoAction", new int[] { 8 })]
		public float Volume = 1f;

		// Token: 0x0400110A RID: 4362
		[Tooltip("whether to mute the track or not when that feedback plays")]
		[MMFEnumCondition("VideoAction", new int[] { 9 })]
		public bool Mute = true;

		// Token: 0x02000505 RID: 1285
		public enum VideoActions
		{
			// Token: 0x04002377 RID: 9079
			Play,
			// Token: 0x04002378 RID: 9080
			Pause,
			// Token: 0x04002379 RID: 9081
			Toggle,
			// Token: 0x0400237A RID: 9082
			Stop,
			// Token: 0x0400237B RID: 9083
			Prepare,
			// Token: 0x0400237C RID: 9084
			StepForward,
			// Token: 0x0400237D RID: 9085
			StepBackward,
			// Token: 0x0400237E RID: 9086
			SetPlaybackSpeed,
			// Token: 0x0400237F RID: 9087
			SetDirectAudioVolume,
			// Token: 0x04002380 RID: 9088
			SetDirectAudioMute,
			// Token: 0x04002381 RID: 9089
			GoToFrame,
			// Token: 0x04002382 RID: 9090
			ToggleLoop
		}
	}
}
