using System;
using UnityEngine;
using UnityEngine.UI;

namespace MoreMountains.Tools
{
	// Token: 0x02000148 RID: 328
	public class MMSceneLoadingImageProgress : MonoBehaviour
	{
		// Token: 0x0600073F RID: 1855 RVA: 0x000232C2 File Offset: 0x000214C2
		protected virtual void Awake()
		{
			this._image = base.gameObject.GetComponent<Image>();
		}

		// Token: 0x06000740 RID: 1856 RVA: 0x000232D5 File Offset: 0x000214D5
		public virtual void SetProgress(float newValue)
		{
			this._image.fillAmount = newValue;
		}

		// Token: 0x040008C9 RID: 2249
		protected Image _image;
	}
}
