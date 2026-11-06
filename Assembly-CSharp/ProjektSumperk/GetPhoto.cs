using System;
using UnityEngine;
using UnityEngine.UI;

namespace ProjektSumperk
{
	// Token: 0x020003E6 RID: 998
	public class GetPhoto : MonoBehaviour
	{
		// Token: 0x06001978 RID: 6520 RVA: 0x00004F31 File Offset: 0x00003131
		private void Start()
		{
		}

		// Token: 0x06001979 RID: 6521 RVA: 0x000A12BB File Offset: 0x0009F4BB
		public void GetPhotoImagesOnClick()
		{
			this.photoGallery = Object.FindObjectOfType<PhotoGallery>();
			this.sprite = base.gameObject.transform.GetChild(0).GetComponent<Image>().sprite;
			this.photoGallery.GetPhotoImages(this.sprite);
		}

		// Token: 0x04001A97 RID: 6807
		public Sprite sprite;

		// Token: 0x04001A98 RID: 6808
		private PhotoGallery photoGallery;
	}
}
