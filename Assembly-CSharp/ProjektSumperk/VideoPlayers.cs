using System;
using UnityEngine;
using UnityEngine.Video;

namespace ProjektSumperk
{
	// Token: 0x02000410 RID: 1040
	public class VideoPlayers : MonoBehaviour
	{
		// Token: 0x06001A67 RID: 6759 RVA: 0x000A6F98 File Offset: 0x000A5198
		private void OnEnable()
		{
			this.ShowUIOnTap();
		}

		// Token: 0x06001A68 RID: 6760 RVA: 0x000A6FA0 File Offset: 0x000A51A0
		private void Update()
		{
			if (this.timer > 0f)
			{
				this.vpUI.SetActive(true);
			}
			else
			{
				this.vpUI.SetActive(false);
			}
			if (this.timer > 0f)
			{
				this.timer -= Time.deltaTime;
			}
		}

		// Token: 0x06001A69 RID: 6761 RVA: 0x000A6FF3 File Offset: 0x000A51F3
		public void ShowUIOnTap()
		{
			this.vpUI.SetActive(true);
			this.timer = 3f;
		}

		// Token: 0x06001A6A RID: 6762 RVA: 0x000A700C File Offset: 0x000A520C
		public void ExitVideo()
		{
			this.vp.Stop();
			this.videoPlayerPanel.SetActive(false);
			this.rt.Release();
		}

		// Token: 0x06001A6B RID: 6763 RVA: 0x000A7030 File Offset: 0x000A5230
		public void PlayVideo()
		{
			this.play.SetActive(false);
			this.pause.SetActive(true);
			this.vp.Play();
			this.timer = 3f;
		}

		// Token: 0x06001A6C RID: 6764 RVA: 0x000A7060 File Offset: 0x000A5260
		public void PauseVideo()
		{
			this.play.SetActive(true);
			this.pause.SetActive(false);
			this.vp.Pause();
			this.timer = 3f;
		}

		// Token: 0x04001B7A RID: 7034
		public VideoPlayer vp;

		// Token: 0x04001B7B RID: 7035
		public GameObject videoPlayerPanel;

		// Token: 0x04001B7C RID: 7036
		public GameObject play;

		// Token: 0x04001B7D RID: 7037
		public GameObject pause;

		// Token: 0x04001B7E RID: 7038
		public GameObject vpUI;

		// Token: 0x04001B7F RID: 7039
		private float timer;

		// Token: 0x04001B80 RID: 7040
		public RenderTexture rt;
	}
}
