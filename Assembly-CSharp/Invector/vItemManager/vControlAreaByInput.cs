using System;
using System.Collections.Generic;
using Invector.vCharacterController;
using UnityEngine;
using UnityEngine.Events;

namespace Invector.vItemManager
{
	// Token: 0x0200053C RID: 1340
	[vClassHeader("Control Area By Input", "Use to select an EquipArea when you press a Input")]
	public class vControlAreaByInput : vMonoBehaviour
	{
		// Token: 0x1400003A RID: 58
		// (add) Token: 0x0600202D RID: 8237 RVA: 0x000BE900 File Offset: 0x000BCB00
		// (remove) Token: 0x0600202E RID: 8238 RVA: 0x000BE938 File Offset: 0x000BCB38
		public event vControlAreaByInput.OnSelectSlot onSelectSlot;

		// Token: 0x0600202F RID: 8239 RVA: 0x000BE970 File Offset: 0x000BCB70
		protected virtual void Start()
		{
			this.inventory = base.GetComponentInParent<vInventory>();
			for (int i = 0; i < this.slotsSelectors.Count; i++)
			{
				this.onSelectSlot += this.slotsSelectors[i].Select;
			}
			vControlAreaByInput.OnSelectSlot onSelectSlot = this.onSelectSlot;
			if (onSelectSlot == null)
			{
				return;
			}
			onSelectSlot(0);
		}

		// Token: 0x06002030 RID: 8240 RVA: 0x000BE9D0 File Offset: 0x000BCBD0
		protected virtual void Update()
		{
			if (!this.inventory || !this.equipArea || this.inventory.lockInventoryInput)
			{
				return;
			}
			for (int i = 0; i < this.slotsSelectors.Count; i++)
			{
				if (this.slotsSelectors[i].input.GetButtonDown() && this.inventory && !this.inventory.IsLocked() && !this.inventory.isOpen && this.inventory.canEquip && this.slotsSelectors[i].indexOfSlot < this.equipArea.equipSlots.Count && this.slotsSelectors[i].indexOfSlot >= 0)
				{
					this.equipArea.SetEquipSlot(this.slotsSelectors[i].indexOfSlot);
					vControlAreaByInput.OnSelectSlot onSelectSlot = this.onSelectSlot;
					if (onSelectSlot != null)
					{
						onSelectSlot(this.slotsSelectors[i].indexOfSlot);
					}
				}
				if (this.slotsSelectors[i].equipDisplay != null && this.slotsSelectors[i].indexOfSlot < this.equipArea.equipSlots.Count && this.slotsSelectors[i].indexOfSlot >= 0 && !(this.slotsSelectors[i].equipDisplay == null))
				{
					if (this.equipArea.equipSlots[this.slotsSelectors[i].indexOfSlot].item != this.slotsSelectors[i].equipDisplay.item)
					{
						this.slotsSelectors[i].equipDisplay.AddItem(this.equipArea.equipSlots[this.slotsSelectors[i].indexOfSlot].item);
					}
					else if (this.equipArea.equipSlots[this.slotsSelectors[i].indexOfSlot].item == null && this.slotsSelectors[i].equipDisplay.hasItem)
					{
						this.slotsSelectors[i].equipDisplay.RemoveItem();
					}
				}
			}
		}

		// Token: 0x040020FE RID: 8446
		public List<vControlAreaByInput.SlotsSelector> slotsSelectors;

		// Token: 0x040020FF RID: 8447
		public vEquipArea equipArea;

		// Token: 0x04002100 RID: 8448
		public vInventory inventory;

		// Token: 0x0200053D RID: 1341
		// (Invoke) Token: 0x06002033 RID: 8243
		public delegate void OnSelectSlot(int indexOfSlot);

		// Token: 0x0200053E RID: 1342
		[Serializable]
		public class SlotsSelector
		{
			// Token: 0x06002036 RID: 8246 RVA: 0x000BEC40 File Offset: 0x000BCE40
			public virtual void Select(int indexOfSlot)
			{
				if (this.indexOfSlot != indexOfSlot && this.selected)
				{
					if (this.equipDisplay)
					{
						this.equipDisplay.onDeselect.Invoke();
					}
					this.onDeselect.Invoke();
					this.selected = false;
					return;
				}
				if (this.indexOfSlot == indexOfSlot && !this.selected)
				{
					if (this.equipDisplay)
					{
						this.equipDisplay.onSelect.Invoke();
					}
					this.onSelect.Invoke();
					this.selected = true;
				}
			}

			// Token: 0x04002102 RID: 8450
			public GenericInput input;

			// Token: 0x04002103 RID: 8451
			[Tooltip("Index of EquipArea Slots list")]
			public int indexOfSlot;

			// Token: 0x04002104 RID: 8452
			[Tooltip("Equipment display, this is optional.\nControl the display base on selected slot.")]
			public vEquipmentDisplay equipDisplay;

			// Token: 0x04002105 RID: 8453
			[vReadOnly(true)]
			public bool selected;

			// Token: 0x04002106 RID: 8454
			public UnityEvent onSelect;

			// Token: 0x04002107 RID: 8455
			public UnityEvent onDeselect;
		}
	}
}
