using System;
using Features.PhoneCompanion;
using UnityEngine;
using UnityEngine.EventSystems;

namespace ProjektSumperk
{
	// Token: 0x02000400 RID: 1024
	public sealed class PhoneVideoGalleryItem : MonoBehaviour, IPointerClickHandler, IEventSystemHandler
	{
		// Token: 0x06001A07 RID: 6663 RVA: 0x000A5312 File Offset: 0x000A3512
		public void OnPointerClick(PointerEventData eventData)
		{
			if (eventData == null || eventData.button == PointerEventData.InputButton.Left)
			{
				PhoneVideoViewer.Show(this.videoPath);
			}
		}

		// Token: 0x06001A08 RID: 6664 RVA: 0x000A532C File Offset: 0x000A352C
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

		// Token: 0x04001B2C RID: 6956
		public string videoPath;

		// Token: 0x04001B2D RID: 6957
		public Sprite runtimeSprite;
	}
}
