using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Invector.vItemManager
{
	// Token: 0x020005F5 RID: 1525
	public class vAmmoDisplayImage : MonoBehaviour
	{
		// Token: 0x06002351 RID: 9041 RVA: 0x000C94E0 File Offset: 0x000C76E0
		public void ChangeAmmoDisplayImage(int id)
		{
			if (this.currentAmmoId != id && this.displayImages != null)
			{
				vAmmoDisplayImage.vDisplayImage vDisplayImage = this.displayImages.Find((vAmmoDisplayImage.vDisplayImage d) => d.ammoId.Equals(id));
				if (vDisplayImage != null)
				{
					this.displayImage.sprite = vDisplayImage.ammoImage;
				}
				else
				{
					this.displayImage.sprite = this.defaultAmmoImage;
				}
				this.currentAmmoId = id;
			}
		}

		// Token: 0x04002368 RID: 9064
		public Image displayImage;

		// Token: 0x04002369 RID: 9065
		public Sprite defaultAmmoImage;

		// Token: 0x0400236A RID: 9066
		public List<vAmmoDisplayImage.vDisplayImage> displayImages = new List<vAmmoDisplayImage.vDisplayImage>();

		// Token: 0x0400236B RID: 9067
		private int currentAmmoId;

		// Token: 0x020005F6 RID: 1526
		[Serializable]
		public class vDisplayImage
		{
			// Token: 0x0400236C RID: 9068
			public Sprite ammoImage;

			// Token: 0x0400236D RID: 9069
			public int ammoId;
		}
	}
}
