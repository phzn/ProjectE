using System;
using UnityEngine.UI;

namespace Invector.vItemManager
{
	// Token: 0x020005C4 RID: 1476
	[vClassHeader("Item Options Window", true, "icon_v2", false, "")]
	public class vItemOptionWindow : vMonoBehaviour
	{
		// Token: 0x06002257 RID: 8791 RVA: 0x00004F31 File Offset: 0x00003131
		public virtual void EnableOptions(vItemSlot slot)
		{
		}

		// Token: 0x06002258 RID: 8792 RVA: 0x000C5B58 File Offset: 0x000C3D58
		protected virtual void ValidateButtons(vItem item, out bool result)
		{
			if (item.originalObject && item.originalObject.GetComponent<vEquipment>() != null)
			{
				if (this.equipItemButton)
				{
					this.equipItemButton.gameObject.SetActive(true);
				}
				if (this.useItemButton)
				{
					this.useItemButton.gameObject.SetActive(false);
				}
			}
			else
			{
				if (this.equipItemButton)
				{
					this.equipItemButton.gameObject.SetActive(false);
				}
				if (this.useItemButton)
				{
					this.useItemButton.gameObject.SetActive(true);
				}
			}
			if (this.useItemButton)
			{
				this.useItemButton.interactable = item.canBeUsed;
			}
			if (this.dropItemButton)
			{
				this.dropItemButton.interactable = item.canBeDroped;
			}
			if (this.destroyItemButton)
			{
				this.destroyItemButton.interactable = item.canBeDestroyed;
			}
			result = (this.equipItemButton && this.equipItemButton.gameObject.activeSelf) || (this.useItemButton && this.useItemButton.interactable) || (this.dropItemButton && this.dropItemButton.interactable) || (this.destroyItemButton && this.destroyItemButton.interactable);
		}

		// Token: 0x06002259 RID: 8793 RVA: 0x000C5CCC File Offset: 0x000C3ECC
		public virtual bool CanOpenOptions(vItem item)
		{
			if (item == null)
			{
				return false;
			}
			bool flag = false;
			this.ValidateButtons(item, out flag);
			return flag;
		}

		// Token: 0x0400229B RID: 8859
		public Button useItemButton;

		// Token: 0x0400229C RID: 8860
		public Button equipItemButton;

		// Token: 0x0400229D RID: 8861
		public Button dropItemButton;

		// Token: 0x0400229E RID: 8862
		public Button destroyItemButton;
	}
}
