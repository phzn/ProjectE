using System;
using UnityEngine;
using UnityEngine.UI;

namespace Michsky.MUIP
{
	// Token: 0x02000211 RID: 529
	[ExecuteInEditMode]
	[DisallowMultipleComponent]
	[AddComponentMenu("Modern UI Pack/Image/Icon Manager")]
	[RequireComponent(typeof(Image))]
	public class IconManager : MonoBehaviour
	{
		// Token: 0x06000ECD RID: 3789 RVA: 0x00063E00 File Offset: 0x00062000
		private void Awake()
		{
			try
			{
				if (this.iconLibrary == null)
				{
					this.iconLibrary = Resources.Load<IconLibrary>("Icon Library");
				}
				if (this.imageObject == null)
				{
					this.imageObject = base.gameObject.GetComponent<Image>();
				}
				base.enabled = true;
				this.UpdateElement();
			}
			catch
			{
				Debug.LogWarning("<b>Icon Library</b> is missing, but it should be assigned.", this);
			}
		}

		// Token: 0x06000ECE RID: 3790 RVA: 0x00063E78 File Offset: 0x00062078
		private void Update()
		{
			if (this.iconLibrary.alwaysUpdate)
			{
				this.UpdateElement();
			}
			if (Application.isPlaying && this.iconLibrary.optimizeUpdates)
			{
				base.enabled = false;
			}
		}

		// Token: 0x06000ECF RID: 3791 RVA: 0x00063EA8 File Offset: 0x000620A8
		public void UpdateElement()
		{
			if (this.iconLibrary == null)
			{
				base.enabled = false;
				return;
			}
			int i = 0;
			while (i < this.iconLibrary.icons.Count)
			{
				if (this.selectedIconID == this.iconLibrary.icons[i].iconTitle && base.gameObject.activeInHierarchy)
				{
					if (this.spriteSize == 0)
					{
						this.imageObject.sprite = this.iconLibrary.icons[i].iconSprite32;
						break;
					}
					if (this.spriteSize == 1)
					{
						this.imageObject.sprite = this.iconLibrary.icons[i].iconSprite64;
						break;
					}
					if (this.spriteSize == 2)
					{
						this.imageObject.sprite = this.iconLibrary.icons[i].iconSprite128;
						break;
					}
					if (this.spriteSize == 3)
					{
						this.imageObject.sprite = this.iconLibrary.icons[i].iconSprite256;
						break;
					}
					break;
				}
				else
				{
					i++;
				}
			}
			if (!this.iconLibrary.alwaysUpdate)
			{
				base.enabled = false;
			}
		}

		// Token: 0x06000ED0 RID: 3792 RVA: 0x00063FE8 File Offset: 0x000621E8
		public void UpdateSpriteSize(int spriteIndex, int newSize)
		{
			if (newSize == 0)
			{
				this.imageObject.sprite = this.iconLibrary.icons[spriteIndex].iconSprite32;
				return;
			}
			if (newSize == 1)
			{
				this.imageObject.sprite = this.iconLibrary.icons[spriteIndex].iconSprite64;
				return;
			}
			if (newSize == 2)
			{
				this.imageObject.sprite = this.iconLibrary.icons[spriteIndex].iconSprite128;
				return;
			}
			if (newSize == 3)
			{
				this.imageObject.sprite = this.iconLibrary.icons[spriteIndex].iconSprite256;
			}
		}

		// Token: 0x06000ED1 RID: 3793 RVA: 0x0006408C File Offset: 0x0006228C
		public void ChangeIcon(string newSprite, int preferredSize)
		{
			int num = -1;
			for (int i = 0; i < this.iconLibrary.icons.Count; i++)
			{
				if (newSprite == this.iconLibrary.icons[i].iconTitle)
				{
					num = i;
					break;
				}
			}
			if (num != -1)
			{
				this.UpdateSpriteSize(num, preferredSize);
				return;
			}
			Debug.Log("<b>[Icon Manager]</b> Cannot find an icon named '" + newSprite + "'");
		}

		// Token: 0x04000F84 RID: 3972
		public IconLibrary iconLibrary;

		// Token: 0x04000F85 RID: 3973
		public string selectedIconID;

		// Token: 0x04000F86 RID: 3974
		public int selectedIconIndex;

		// Token: 0x04000F87 RID: 3975
		[Range(0f, 3f)]
		public int spriteSize;

		// Token: 0x04000F88 RID: 3976
		private Image imageObject;

		// Token: 0x04000F89 RID: 3977
		[HideInInspector]
		public string currentSize;

		// Token: 0x04000F8A RID: 3978
		[HideInInspector]
		public bool size32;

		// Token: 0x04000F8B RID: 3979
		[HideInInspector]
		public bool size64;

		// Token: 0x04000F8C RID: 3980
		[HideInInspector]
		public bool size128;

		// Token: 0x04000F8D RID: 3981
		[HideInInspector]
		public bool size256;
	}
}
