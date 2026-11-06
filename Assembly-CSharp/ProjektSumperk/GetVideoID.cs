using System;
using UnityEngine;

namespace ProjektSumperk
{
	// Token: 0x020003E7 RID: 999
	public class GetVideoID : MonoBehaviour
	{
		// Token: 0x0600197B RID: 6523 RVA: 0x000A12FA File Offset: 0x0009F4FA
		private void Start()
		{
			this.videoGallery = Object.FindObjectOfType<VideoGallery>();
		}

		// Token: 0x0600197C RID: 6524 RVA: 0x000A1307 File Offset: 0x0009F507
		public void GetVideoIdOnClick()
		{
			this.vid = base.gameObject.transform.GetSiblingIndex();
			this.videoGallery.GetVideoID(this.vid);
		}

		// Token: 0x04001A99 RID: 6809
		public int vid;

		// Token: 0x04001A9A RID: 6810
		private VideoGallery videoGallery;
	}
}
