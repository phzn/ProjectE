using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

namespace ProjektSumperk
{
	// Token: 0x0200040F RID: 1039
	public class VideoGallery : MonoBehaviour
	{
		// Token: 0x06001A63 RID: 6755 RVA: 0x000A6E58 File Offset: 0x000A5058
		private void Start()
		{
			for (int i = 0; i < this.vClip.Length; i++)
			{
				this.length[i] = this.vClip[i].length;
				GameObject gameObject = Object.Instantiate<GameObject>(this.videoPrefab);
				gameObject.transform.SetParent(this.parent, false);
				gameObject.GetComponent<GetVideoID>().vid = i;
				gameObject.transform.localPosition = new Vector3(0f, 0f, 0f);
				gameObject.transform.localEulerAngles = new Vector3(0f, 0f, 0f);
				int num = Mathf.RoundToInt((float)this.length[i]);
				int num2 = Mathf.FloorToInt((float)num / 60f);
				int num3 = Mathf.FloorToInt((float)(num - num2 * 60));
				string text = string.Format("{0:0}:{1:00}", num2, num3);
				gameObject.transform.GetChild(0).GetComponent<TMP_Text>().text = text;
				gameObject.GetComponent<Image>().sprite = this.thumbnails[i];
			}
		}

		// Token: 0x06001A64 RID: 6756 RVA: 0x00004F31 File Offset: 0x00003131
		private void Update()
		{
		}

		// Token: 0x06001A65 RID: 6757 RVA: 0x000A6F62 File Offset: 0x000A5162
		public void GetVideoID(int id)
		{
			this.videoPlayer.SetActive(true);
			this.videoPlayer.GetComponent<VideoPlayer>().clip = this.vClip[id];
			this.videoPlayer.GetComponent<VideoPlayer>().Play();
		}

		// Token: 0x04001B74 RID: 7028
		public VideoClip[] vClip;

		// Token: 0x04001B75 RID: 7029
		public double[] length;

		// Token: 0x04001B76 RID: 7030
		public Sprite[] thumbnails;

		// Token: 0x04001B77 RID: 7031
		public GameObject videoPrefab;

		// Token: 0x04001B78 RID: 7032
		public Transform parent;

		// Token: 0x04001B79 RID: 7033
		public GameObject videoPlayer;
	}
}
