using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Invector.vItemManager
{
	// Token: 0x02000545 RID: 1349
	public class vEquipAreaControl : MonoBehaviour
	{
		// Token: 0x06002070 RID: 8304 RVA: 0x000BFC20 File Offset: 0x000BDE20
		protected virtual void Start()
		{
			this.equipAreas = base.GetComponentsInChildren<vEquipArea>().vToList<vEquipArea>();
			foreach (vEquipArea vEquipArea in this.equipAreas)
			{
				vEquipArea.onPickUpItemCallBack = new vEquipArea.OnPickUpItem(this.OnPickUpItemCallBack);
			}
			vInventory componentInParent = base.GetComponentInParent<vInventory>();
			if (componentInParent)
			{
				componentInParent.onOpenCloseInventory.AddListener(new UnityAction<bool>(this.OnOpen));
			}
		}

		// Token: 0x06002071 RID: 8305 RVA: 0x00004F31 File Offset: 0x00003131
		public virtual void OnOpen(bool value)
		{
		}

		// Token: 0x06002072 RID: 8306 RVA: 0x000BFCB8 File Offset: 0x000BDEB8
		public virtual void OnPickUpItemCallBack(vEquipArea area, vItemSlot slot)
		{
			Predicate<vEquipSlot> <>9__0;
			for (int i = 0; i < this.equipAreas.Count; i++)
			{
				List<vEquipSlot> equipSlots = this.equipAreas[i].equipSlots;
				Predicate<vEquipSlot> predicate;
				if ((predicate = <>9__0) == null)
				{
					predicate = (<>9__0 = (vEquipSlot slotInArea) => slotInArea != slot && slotInArea.item != null && slotInArea.item == slot.item);
				}
				List<vEquipSlot> list = equipSlots.FindAll(predicate);
				for (int j = 0; j < list.Count; j++)
				{
					this.equipAreas[i].UnequipItem(list[j]);
				}
			}
			this.CheckTwoHandItem(area, slot);
		}

		// Token: 0x06002073 RID: 8307 RVA: 0x000BFD5C File Offset: 0x000BDF5C
		protected virtual void CheckTwoHandItem(vEquipArea area, vItemSlot slot)
		{
			if (slot.item == null)
			{
				return;
			}
			vEquipArea vEquipArea = this.equipAreas.Find((vEquipArea _area) => _area != null && area.equipPointName.Equals("LeftArm") && _area.currentEquippedItem != null);
			if (area.equipPointName.Equals("LeftArm"))
			{
				vEquipArea = this.equipAreas.Find((vEquipArea _area) => _area != null && area.equipPointName.Equals("RightArm") && _area.currentEquippedItem != null);
			}
			else if (!area.equipPointName.Equals("RightArm"))
			{
				return;
			}
			if (vEquipArea != null && (slot.item.twoHandWeapon || vEquipArea.currentEquippedItem.twoHandWeapon))
			{
				vEquipArea.onUnequipItem.Invoke(vEquipArea, slot.item);
				vEquipArea.UnequipItem(slot as vEquipSlot);
			}
		}

		// Token: 0x0400212E RID: 8494
		[HideInInspector]
		public List<vEquipArea> equipAreas;
	}
}
