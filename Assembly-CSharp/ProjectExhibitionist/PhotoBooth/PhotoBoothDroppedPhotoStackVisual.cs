using System;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectExhibitionist.PhotoBooth
{
	// Token: 0x02000978 RID: 2424
	[DisallowMultipleComponent]
	public sealed class PhotoBoothDroppedPhotoStackVisual : MonoBehaviour
	{
		// Token: 0x060042A7 RID: 17063 RVA: 0x0018E1AA File Offset: 0x0018C3AA
		public void TrackDisplay(PhotoBoothPhotoArchive.LoadedPhoto display)
		{
			if (display != null)
			{
				this._displayCopies.Add(display);
			}
		}

		// Token: 0x060042A8 RID: 17064 RVA: 0x0018E1BC File Offset: 0x0018C3BC
		private void OnDestroy()
		{
			for (int i = 0; i < this._displayCopies.Count; i++)
			{
				PhotoBoothPhotoArchive.LoadedPhoto loadedPhoto = this._displayCopies[i];
				if (loadedPhoto != null)
				{
					loadedPhoto.DestroyTextures();
				}
			}
			this._displayCopies.Clear();
		}

		// Token: 0x040042BD RID: 17085
		private readonly List<PhotoBoothPhotoArchive.LoadedPhoto> _displayCopies = new List<PhotoBoothPhotoArchive.LoadedPhoto>(16);
	}
}
