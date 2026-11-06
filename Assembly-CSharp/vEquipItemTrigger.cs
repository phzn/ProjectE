using System;
using System.Collections.Generic;
using Invector.vItemManager;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x02000127 RID: 295
public class vEquipItemTrigger : MonoBehaviour
{
	// Token: 0x060009F8 RID: 2552 RVA: 0x00045A08 File Offset: 0x00043C08
	public void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.CompareTag("Player"))
		{
			vItemManager component = other.GetComponent<vItemManager>();
			if (component)
			{
				this.EquipItemIfExist(component);
			}
		}
	}

	// Token: 0x060009F9 RID: 2553 RVA: 0x00045A40 File Offset: 0x00043C40
	private void EquipItemIfExist(vItemManager itemManager)
	{
		if (itemManager.items.Exists((vItem i) => i.id == this.itemID))
		{
			vItem item = itemManager.items.Find((vItem i) => i.id == this.itemID);
			Predicate<vEquipSlot> <>9__3;
			int num = Array.FindIndex<vEquipArea>(itemManager.inventory.equipAreas, delegate(vEquipArea area)
			{
				List<vEquipSlot> equipSlots = area.equipSlots;
				Predicate<vEquipSlot> predicate;
				if ((predicate = <>9__3) == null)
				{
					predicate = (<>9__3 = (vEquipSlot slot) => slot.itemType.Contains(item.type));
				}
				return equipSlots.Exists(predicate);
			});
			itemManager.EquipItemToCurrentEquipSlot(item, num, true);
			this.OnEquipSuccess.Invoke();
			return;
		}
		this.OnEquipFail.Invoke();
	}

	// Token: 0x0400093B RID: 2363
	public int itemID;

	// Token: 0x0400093C RID: 2364
	public UnityEvent OnEquipSuccess;

	// Token: 0x0400093D RID: 2365
	public UnityEvent OnEquipFail;

	// Token: 0x0400093E RID: 2366
	public vItemManager itemManager;
}
