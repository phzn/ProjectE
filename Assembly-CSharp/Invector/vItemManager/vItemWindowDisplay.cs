using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace Invector.vItemManager
{
	// Token: 0x020005CF RID: 1487
	public class vItemWindowDisplay : MonoBehaviour
	{
		// Token: 0x1700062C RID: 1580
		// (get) Token: 0x0600229C RID: 8860 RVA: 0x000C6E28 File Offset: 0x000C5028
		// (set) Token: 0x0600229D RID: 8861 RVA: 0x000C6E30 File Offset: 0x000C5030
		public virtual vItemSlot currentSelectedSlot { get; set; }

		// Token: 0x1700062D RID: 1581
		// (get) Token: 0x0600229E RID: 8862 RVA: 0x000C6E39 File Offset: 0x000C5039
		// (set) Token: 0x0600229F RID: 8863 RVA: 0x000C6E41 File Offset: 0x000C5041
		public int amount { get; set; }

		// Token: 0x060022A0 RID: 8864 RVA: 0x000C6E4C File Offset: 0x000C504C
		public virtual void OnEnable()
		{
			if (this.inventory == null)
			{
				this.inventory = base.GetComponentInParent<vInventory>();
			}
			if (this.inventory && this.itemWindow)
			{
				this.inventory.onDestroyItem.RemoveListener(new UnityAction<vItem, int>(this.OnDestroyItem));
				this.inventory.onDestroyItem.AddListener(new UnityAction<vItem, int>(this.OnDestroyItem));
				this.itemWindow.CreateEquipmentWindow(this.inventory.items, new UnityAction<vItemSlot>(this.OnSubmit), new UnityAction<vItemSlot>(this.OnSelectSlot), true);
				this.inventory.OnUpdateInventory -= this.CheckItemExits;
				this.inventory.OnUpdateInventory += this.CheckItemExits;
			}
		}

		// Token: 0x060022A1 RID: 8865 RVA: 0x000C6F2E File Offset: 0x000C512E
		public virtual void OnDisable()
		{
			if (this.inventory)
			{
				this.inventory.OnUpdateInventory -= this.CheckItemExits;
			}
		}

		// Token: 0x060022A2 RID: 8866 RVA: 0x000C6F58 File Offset: 0x000C5158
		public virtual void OnDestroyItem(vItem item, int amount)
		{
			vItemSlot vItemSlot = this.itemWindow.slots.Find((vItemSlot slot) => slot.item.Equals(item));
			if (vItemSlot != null && (vItemSlot.item == null || vItemSlot.item.amount == 0))
			{
				this.itemWindow.slots.Remove(vItemSlot);
				Object.Destroy(vItemSlot.gameObject);
			}
		}

		// Token: 0x060022A3 RID: 8867 RVA: 0x000C6FD0 File Offset: 0x000C51D0
		public virtual void OnSubmit(vItemSlot slot)
		{
			this.currentSelectedSlot = slot;
			if (slot.item)
			{
				slot.GetComponent<RectTransform>();
				if (this.optionWindow.CanOpenOptions(slot.item))
				{
					this.optionWindow.gameObject.SetActive(true);
					this.optionWindow.EnableOptions(slot);
				}
			}
		}

		// Token: 0x060022A4 RID: 8868 RVA: 0x000C7028 File Offset: 0x000C5228
		public virtual void OnSelectSlot(vItemSlot slot)
		{
			this.currentSelectedSlot = slot;
		}

		// Token: 0x060022A5 RID: 8869 RVA: 0x000C7034 File Offset: 0x000C5234
		public virtual void DropItem()
		{
			if (this.amount > 0)
			{
				this.inventory.OnDropItem(this.currentSelectedSlot.item, this.amount);
				if (this.currentSelectedSlot != null && (this.currentSelectedSlot.item == null || this.currentSelectedSlot.item.amount <= 0))
				{
					if (this.itemWindow.slots.Contains(this.currentSelectedSlot))
					{
						this.itemWindow.slots.Remove(this.currentSelectedSlot);
					}
					Object.Destroy(this.currentSelectedSlot.gameObject);
					if (this.itemWindow.slots.Count > 0)
					{
						this.SetSelectable(this.itemWindow.slots[0].gameObject);
					}
				}
			}
		}

		// Token: 0x060022A6 RID: 8870 RVA: 0x000C7110 File Offset: 0x000C5310
		public virtual void LeaveItem()
		{
			if (this.amount > 0)
			{
				this.inventory.OnDestroyItem(this.currentSelectedSlot.item, this.amount);
				if (this.currentSelectedSlot != null && (this.currentSelectedSlot.item == null || this.currentSelectedSlot.item.amount <= 0))
				{
					if (this.itemWindow.slots.Contains(this.currentSelectedSlot))
					{
						this.itemWindow.slots.Remove(this.currentSelectedSlot);
					}
					Object.Destroy(this.currentSelectedSlot.gameObject);
					if (this.itemWindow.slots.Count > 0)
					{
						this.SetSelectable(this.itemWindow.slots[0].gameObject);
					}
				}
			}
		}

		// Token: 0x060022A7 RID: 8871 RVA: 0x000C71EA File Offset: 0x000C53EA
		public virtual void UseItem()
		{
			this.inventory.OnUseItem(this.currentSelectedSlot.item);
		}

		// Token: 0x060022A8 RID: 8872 RVA: 0x000C7202 File Offset: 0x000C5402
		protected virtual void CheckItemExits()
		{
			this.itemWindow.ReloadItems(this.inventory.items);
		}

		// Token: 0x060022A9 RID: 8873 RVA: 0x000C721C File Offset: 0x000C541C
		public virtual void SetOldSelectable()
		{
			try
			{
				if (this.currentSelectedSlot != null)
				{
					this.SetSelectable(this.currentSelectedSlot.gameObject);
				}
				else if (this.itemWindow.slots.Count > 0 && this.itemWindow.slots[0] != null)
				{
					this.SetSelectable(this.itemWindow.slots[0].gameObject);
				}
			}
			catch
			{
			}
		}

		// Token: 0x060022AA RID: 8874 RVA: 0x000C72A8 File Offset: 0x000C54A8
		public virtual void SetSelectable(GameObject target)
		{
			try
			{
				PointerEventData pointerEventData = new PointerEventData(EventSystem.current);
				ExecuteEvents.Execute<IPointerExitHandler>(EventSystem.current.currentSelectedGameObject, pointerEventData, ExecuteEvents.pointerExitHandler);
				EventSystem.current.SetSelectedGameObject(target, new BaseEventData(EventSystem.current));
				ExecuteEvents.Execute<ISelectHandler>(target, pointerEventData, ExecuteEvents.selectHandler);
			}
			catch
			{
			}
		}

		// Token: 0x040022F0 RID: 8944
		public vInventory inventory;

		// Token: 0x040022F1 RID: 8945
		public vItemWindow itemWindow;

		// Token: 0x040022F2 RID: 8946
		public vItemOptionWindow optionWindow;
	}
}
