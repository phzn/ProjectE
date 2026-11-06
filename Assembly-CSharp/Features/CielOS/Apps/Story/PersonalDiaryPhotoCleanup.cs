using System;
using System.Collections.Generic;
using Features.PhoneCompanion;
using UnityEngine;

namespace Features.CielOS.Apps.Story
{
	// Token: 0x0200142D RID: 5165
	internal sealed class PersonalDiaryPhotoCleanup : MonoBehaviour
	{
		// Token: 0x06009F9B RID: 40859 RVA: 0x0041D6D2 File Offset: 0x0041B8D2
		private void Update()
		{
			if (PhotoPolaroidViewer.IsOpen)
			{
				return;
			}
			if (this.Sprites != null)
			{
				CielOSStoryApp.DestroyDiarySprites(this.Sprites);
			}
			this.Sprites = null;
			Object.Destroy(base.gameObject);
		}

		// Token: 0x06009F9C RID: 40860 RVA: 0x0041D701 File Offset: 0x0041B901
		private void OnDestroy()
		{
			if (this.Sprites != null)
			{
				CielOSStoryApp.DestroyDiarySprites(this.Sprites);
			}
			this.Sprites = null;
		}

		// Token: 0x0400A381 RID: 41857
		internal List<Sprite> Sprites;
	}
}
