using System;
using MoreMountains.Tools;
using TMPro;
using UnityEngine;

namespace MoreMountains.Feel
{
	// Token: 0x0200037C RID: 892
	[AddComponentMenu("")]
	public class PlaylistDemo : MonoBehaviour
	{
		// Token: 0x0600186E RID: 6254 RVA: 0x00067CB8 File Offset: 0x00065EB8
		protected virtual void Update()
		{
			if (this.PlaylistManager.CurrentClipDuration == 0f)
			{
				this.ProgressBar.SetBar(0f, 0f, 1f);
				return;
			}
			this.ProgressBar.SetBar(this.PlaylistManager.CurrentTime, 0f, this.PlaylistManager.CurrentClipDuration);
			this.SongDuration.text = MMTime.FloatToTimeString(this.PlaylistManager.CurrentTime, false, true, true, false) + " / " + MMTime.FloatToTimeString(this.PlaylistManager.CurrentClipDuration, false, true, true, false);
		}

		// Token: 0x0600186F RID: 6255 RVA: 0x00067D58 File Offset: 0x00065F58
		protected virtual void UpdateSongName()
		{
			int num = this.PlaylistManager.CurrentSongIndex + 1;
			this.SongName.text = num.ToString() + ". " + this.PlaylistManager.CurrentSongName;
		}

		// Token: 0x06001870 RID: 6256 RVA: 0x00067D9A File Offset: 0x00065F9A
		protected virtual void OnPlayEvent(int channel)
		{
			this.UpdateSongName();
		}

		// Token: 0x06001871 RID: 6257 RVA: 0x00067DA2 File Offset: 0x00065FA2
		protected virtual void OnEnable()
		{
			MMPlaylistNewSongStartedEvent.Register(new MMPlaylistNewSongStartedEvent.Delegate(this.OnPlayEvent));
		}

		// Token: 0x06001872 RID: 6258 RVA: 0x00067DB6 File Offset: 0x00065FB6
		protected virtual void OnDisable()
		{
			MMPlaylistNewSongStartedEvent.Unregister(new MMPlaylistNewSongStartedEvent.Delegate(this.OnPlayEvent));
		}

		// Token: 0x04001C6B RID: 7275
		public MMSMPlaylistManager PlaylistManager;

		// Token: 0x04001C6C RID: 7276
		public MMProgressBar ProgressBar;

		// Token: 0x04001C6D RID: 7277
		public TMP_Text SongName;

		// Token: 0x04001C6E RID: 7278
		public TMP_Text SongDuration;
	}
}
