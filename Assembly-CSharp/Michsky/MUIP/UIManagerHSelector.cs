using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Michsky.MUIP
{
	// Token: 0x0200025B RID: 603
	[ExecuteInEditMode]
	public class UIManagerHSelector : MonoBehaviour
	{
		// Token: 0x06000FFB RID: 4091 RVA: 0x0006A4B4 File Offset: 0x000686B4
		private void Awake()
		{
			if (this.UIManagerAsset == null)
			{
				this.UIManagerAsset = Resources.Load<UIManager>("MUIP Manager");
			}
			base.enabled = true;
			if (!this.UIManagerAsset.enableDynamicUpdate)
			{
				this.UpdateSelector();
				base.enabled = false;
			}
		}

		// Token: 0x06000FFC RID: 4092 RVA: 0x0006A500 File Offset: 0x00068700
		private void Update()
		{
			if (this.UIManagerAsset == null)
			{
				return;
			}
			if (this.UIManagerAsset.enableDynamicUpdate)
			{
				this.UpdateSelector();
			}
		}

		// Token: 0x06000FFD RID: 4093 RVA: 0x0006A524 File Offset: 0x00068724
		private void UpdateSelector()
		{
			if (!this.overrideColors && this.latestColor != this.UIManagerAsset.selectorColor)
			{
				for (int i = 0; i < this.images.Count; i++)
				{
					Image component = this.images[i].GetComponent<Image>();
					component.color = new Color(this.UIManagerAsset.selectorColor.r, this.UIManagerAsset.selectorColor.g, this.UIManagerAsset.selectorColor.b, component.color.a);
				}
				for (int j = 0; j < this.imagesHighlighted.Count; j++)
				{
					Image component2 = this.imagesHighlighted[j].GetComponent<Image>();
					component2.color = new Color(this.UIManagerAsset.selectorHighlightedColor.r, this.UIManagerAsset.selectorHighlightedColor.g, this.UIManagerAsset.selectorHighlightedColor.b, component2.color.a);
				}
				this.latestColor = this.UIManagerAsset.selectorColor;
			}
			for (int k = 0; k < this.texts.Count; k++)
			{
				TextMeshProUGUI component3 = this.texts[k].GetComponent<TextMeshProUGUI>();
				if (!this.overrideColors)
				{
					component3.color = new Color(this.UIManagerAsset.selectorColor.r, this.UIManagerAsset.selectorColor.g, this.UIManagerAsset.selectorColor.b, component3.color.a);
				}
				if (!this.overrideFonts)
				{
					component3.font = this.UIManagerAsset.selectorFont;
				}
			}
		}

		// Token: 0x04001177 RID: 4471
		[Header("Settings")]
		[SerializeField]
		private UIManager UIManagerAsset;

		// Token: 0x04001178 RID: 4472
		[HideInInspector]
		public bool overrideColors;

		// Token: 0x04001179 RID: 4473
		[HideInInspector]
		public bool overrideFonts;

		// Token: 0x0400117A RID: 4474
		[Header("Resources")]
		[SerializeField]
		private List<GameObject> images = new List<GameObject>();

		// Token: 0x0400117B RID: 4475
		[SerializeField]
		private List<GameObject> imagesHighlighted = new List<GameObject>();

		// Token: 0x0400117C RID: 4476
		[SerializeField]
		private List<GameObject> texts = new List<GameObject>();

		// Token: 0x0400117D RID: 4477
		private Color latestColor;
	}
}
