using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Michsky.MUIP
{
	// Token: 0x02000252 RID: 594
	[ExecuteInEditMode]
	public class UIManagerAnimatedIcon : MonoBehaviour
	{
		// Token: 0x06000FE3 RID: 4067 RVA: 0x00069888 File Offset: 0x00067A88
		private void Awake()
		{
			if (this.UIManagerAsset == null)
			{
				this.UIManagerAsset = Resources.Load<UIManager>("MUIP Manager");
			}
			base.enabled = true;
			if (!this.UIManagerAsset.enableDynamicUpdate)
			{
				this.UpdateAnimatedIcon();
				base.enabled = false;
			}
		}

		// Token: 0x06000FE4 RID: 4068 RVA: 0x000698D4 File Offset: 0x00067AD4
		private void Update()
		{
			if (this.UIManagerAsset == null)
			{
				return;
			}
			if (this.UIManagerAsset.enableDynamicUpdate)
			{
				this.UpdateAnimatedIcon();
			}
		}

		// Token: 0x06000FE5 RID: 4069 RVA: 0x000698F8 File Offset: 0x00067AF8
		private void UpdateAnimatedIcon()
		{
			for (int i = 0; i < this.images.Count; i++)
			{
				if (!(this.images[i] == null))
				{
					this.images[i].GetComponent<Image>().color = this.UIManagerAsset.animatedIconColor;
				}
			}
			for (int j = 0; j < this.imagesWithAlpha.Count; j++)
			{
				if (!(this.imagesWithAlpha[j] == null))
				{
					Image component = this.imagesWithAlpha[j].GetComponent<Image>();
					component.color = new Color(this.UIManagerAsset.animatedIconColor.r, this.UIManagerAsset.animatedIconColor.g, this.UIManagerAsset.animatedIconColor.b, component.color.a);
				}
			}
		}

		// Token: 0x04001144 RID: 4420
		[Header("Settings")]
		public UIManager UIManagerAsset;

		// Token: 0x04001145 RID: 4421
		[Header("Resources")]
		public List<GameObject> images = new List<GameObject>();

		// Token: 0x04001146 RID: 4422
		public List<GameObject> imagesWithAlpha = new List<GameObject>();
	}
}
