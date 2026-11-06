using System;
using UnityEngine;

namespace ProjektSumperk
{
	// Token: 0x020003FF RID: 1023
	public sealed class PhonePhotoGalleryItem : MonoBehaviour
	{
		// Token: 0x06001A05 RID: 6661 RVA: 0x000A52D0 File Offset: 0x000A34D0
		private void OnDestroy()
		{
			if (this.runtimeSprite == null)
			{
				return;
			}
			Texture2D texture = this.runtimeSprite.texture;
			Object.Destroy(this.runtimeSprite);
			if (texture != null)
			{
				Object.Destroy(texture);
			}
		}

		// Token: 0x04001B2B RID: 6955
		public Sprite runtimeSprite;
	}
}
