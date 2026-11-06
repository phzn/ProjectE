using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Michsky.MUIP
{
	// Token: 0x0200025A RID: 602
	[ExecuteInEditMode]
	public class UIManagerDropdownItem : MonoBehaviour
	{
		// Token: 0x06000FF7 RID: 4087 RVA: 0x0006A394 File Offset: 0x00068594
		private void Awake()
		{
			if (this.UIManagerAsset == null)
			{
				this.UIManagerAsset = Resources.Load<UIManager>("MUIP Manager");
			}
			base.enabled = true;
			if (!this.UIManagerAsset.enableDynamicUpdate)
			{
				this.UpdateDropdown();
				base.enabled = false;
			}
		}

		// Token: 0x06000FF8 RID: 4088 RVA: 0x0006A3E0 File Offset: 0x000685E0
		private void Update()
		{
			if (this.UIManagerAsset == null)
			{
				return;
			}
			if (this.UIManagerAsset.enableDynamicUpdate)
			{
				this.UpdateDropdown();
			}
		}

		// Token: 0x06000FF9 RID: 4089 RVA: 0x0006A404 File Offset: 0x00068604
		private void UpdateDropdown()
		{
			if (!this.overrideFonts && this.itemText != null)
			{
				this.itemText.font = this.UIManagerAsset.dropdownItemFont;
			}
			if (!this.overrideColors)
			{
				if (this.itemBackground != null)
				{
					this.itemBackground.color = this.UIManagerAsset.dropdownItemBackgroundColor;
				}
				if (this.itemIcon != null)
				{
					this.itemIcon.color = this.UIManagerAsset.dropdownItemPrimaryColor;
				}
				if (this.itemText != null)
				{
					this.itemText.color = this.UIManagerAsset.dropdownItemPrimaryColor;
				}
			}
		}

		// Token: 0x04001171 RID: 4465
		[Header("Settings")]
		[SerializeField]
		private UIManager UIManagerAsset;

		// Token: 0x04001172 RID: 4466
		public bool overrideColors;

		// Token: 0x04001173 RID: 4467
		public bool overrideFonts;

		// Token: 0x04001174 RID: 4468
		[Header("Resources")]
		[SerializeField]
		private Image itemBackground;

		// Token: 0x04001175 RID: 4469
		[SerializeField]
		private Image itemIcon;

		// Token: 0x04001176 RID: 4470
		[SerializeField]
		private TextMeshProUGUI itemText;
	}
}
