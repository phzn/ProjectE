using System;
using UnityEngine;

namespace MoreMountains.Tools
{
	// Token: 0x020000C5 RID: 197
	[Serializable]
	public class MMSMPlaylistSong
	{
		// Token: 0x0600040B RID: 1035 RVA: 0x000131F1 File Offset: 0x000113F1
		public virtual void Initialization()
		{
			this.PlayCount = 0;
		}

		// Token: 0x04000553 RID: 1363
		[Tooltip("the name of the song, used only for organizational purposes in the inspector")]
		public string Name;

		// Token: 0x04000554 RID: 1364
		[Tooltip("the clip to play when this song plays")]
		public AudioClip Clip;

		// Token: 0x04000555 RID: 1365
		[Tooltip("the amount of time this song's been played")]
		[MMReadOnly]
		public int PlayCount;

		// Token: 0x04000556 RID: 1366
		[Tooltip("the many options to control this song")]
		public MMSoundManagerPlayOptions Options;
	}
}
