using System;
using UnityEngine;

namespace ProjektSumperk
{
	// Token: 0x020003E3 RID: 995
	public class GetAudio : MonoBehaviour
	{
		// Token: 0x0600196F RID: 6511 RVA: 0x000A122E File Offset: 0x0009F42E
		private void Start()
		{
			this.musicPlayer = Object.FindObjectOfType<MusicPlayerController>();
		}

		// Token: 0x06001970 RID: 6512 RVA: 0x000A123B File Offset: 0x0009F43B
		public void GetAudioOnClick()
		{
			this.clips = base.gameObject;
			this.musicPlayer.GetAudioFile(this.clips);
		}

		// Token: 0x04001A91 RID: 6801
		public GameObject clips;

		// Token: 0x04001A92 RID: 6802
		private MusicPlayerController musicPlayer;
	}
}
