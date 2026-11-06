using System;
using UnityEngine;

namespace MoreMountains.Tools
{
	// Token: 0x020000C4 RID: 196
	[AddComponentMenu("More Mountains/Tools/Audio/MMPlaylistRemote")]
	public class MMPlaylistRemote : MonoBehaviour
	{
		// Token: 0x06000400 RID: 1024 RVA: 0x0001310A File Offset: 0x0001130A
		public virtual void Play()
		{
			MMPlaylistPlayEvent.Trigger(this.Channel);
		}

		// Token: 0x06000401 RID: 1025 RVA: 0x00013117 File Offset: 0x00011317
		public virtual void Pause()
		{
			MMPlaylistPauseEvent.Trigger(this.Channel);
		}

		// Token: 0x06000402 RID: 1026 RVA: 0x00013124 File Offset: 0x00011324
		public virtual void Stop()
		{
			MMPlaylistStopEvent.Trigger(this.Channel);
		}

		// Token: 0x06000403 RID: 1027 RVA: 0x00013131 File Offset: 0x00011331
		public virtual void PlayNextTrack()
		{
			MMPlaylistPlayNextEvent.Trigger(this.Channel);
		}

		// Token: 0x06000404 RID: 1028 RVA: 0x0001313E File Offset: 0x0001133E
		public virtual void PlaySelectedTrack()
		{
			MMPlaylistPlayIndexEvent.Trigger(this.Channel, this.TrackNumber);
		}

		// Token: 0x06000405 RID: 1029 RVA: 0x00013151 File Offset: 0x00011351
		public virtual void PlayTrack(int trackIndex)
		{
			MMPlaylistPlayIndexEvent.Trigger(this.Channel, trackIndex);
		}

		// Token: 0x06000406 RID: 1030 RVA: 0x0001315F File Offset: 0x0001135F
		protected virtual void OnTriggerEnter(Collider collider)
		{
			if (this.PlaySelectedTrackOnTriggerEnter && collider.CompareTag(this.TriggerTag))
			{
				this.PlaySelectedTrack();
			}
		}

		// Token: 0x06000407 RID: 1031 RVA: 0x0001317D File Offset: 0x0001137D
		protected virtual void OnTriggerExit(Collider collider)
		{
			if (this.PlaySelectedTrackOnTriggerExit && collider.CompareTag(this.TriggerTag))
			{
				this.PlaySelectedTrack();
			}
		}

		// Token: 0x06000408 RID: 1032 RVA: 0x0001319B File Offset: 0x0001139B
		protected virtual void OnTriggerEnter2D(Collider2D collider)
		{
			if (this.PlaySelectedTrackOnTriggerEnter && collider.CompareTag(this.TriggerTag))
			{
				this.PlaySelectedTrack();
			}
		}

		// Token: 0x06000409 RID: 1033 RVA: 0x000131B9 File Offset: 0x000113B9
		protected virtual void OnTriggerExit2D(Collider2D collider)
		{
			if (this.PlaySelectedTrackOnTriggerExit && collider.CompareTag(this.TriggerTag))
			{
				this.PlaySelectedTrack();
			}
		}

		// Token: 0x04000549 RID: 1353
		public int Channel;

		// Token: 0x0400054A RID: 1354
		public int TrackNumber;

		// Token: 0x0400054B RID: 1355
		[Header("Triggers")]
		public bool PlaySelectedTrackOnTriggerEnter = true;

		// Token: 0x0400054C RID: 1356
		public bool PlaySelectedTrackOnTriggerExit;

		// Token: 0x0400054D RID: 1357
		public string TriggerTag = "Player";

		// Token: 0x0400054E RID: 1358
		[Header("Test")]
		[MMInspectorButton("Play")]
		public bool PlayButton;

		// Token: 0x0400054F RID: 1359
		[MMInspectorButton("Pause")]
		public bool PauseButton;

		// Token: 0x04000550 RID: 1360
		[MMInspectorButton("Stop")]
		public bool StopButton;

		// Token: 0x04000551 RID: 1361
		[MMInspectorButton("PlayNextTrack")]
		public bool NextButton;

		// Token: 0x04000552 RID: 1362
		[MMInspectorButton("PlaySelectedTrack")]
		public bool SelectedTrackButton;
	}
}
