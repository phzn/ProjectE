using System;
using Invector.vCharacterController;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Invector.vItemManager
{
	// Token: 0x02000548 RID: 1352
	public class vEquipItemToArea : MonoBehaviour
	{
		// Token: 0x0600207A RID: 8314 RVA: 0x000BFEC0 File Offset: 0x000BE0C0
		private void Start()
		{
			this.onEquipAction = delegate
			{
				this.onEquip.Invoke();
			};
			for (int i = 0; i < this.areasToEquip.Length; i++)
			{
				if (this.areasToEquip[i].optionalButton)
				{
					vEquipItemToArea.AreaToEquip areaToEquip = this.areasToEquip[i];
					this.areasToEquip[i].optionalButton.onClick.AddListener(delegate
					{
						this.Equip(areaToEquip);
					});
				}
			}
		}

		// Token: 0x0600207B RID: 8315 RVA: 0x000BFF44 File Offset: 0x000BE144
		protected virtual void Update()
		{
			if (this.itemWindow && this.itemWindow.currentSelectedSlot && this.itemWindow.currentSelectedSlot.item)
			{
				for (int i = 0; i < this.areasToEquip.Length; i++)
				{
					this.areasToEquip[i].CheckInput(this.itemWindow.currentSelectedSlot.item, this.onEquip);
				}
			}
			if (this.cancelInput.GetButtonDown())
			{
				this.onCancel.Invoke();
			}
		}

		// Token: 0x0600207C RID: 8316 RVA: 0x000BFFD8 File Offset: 0x000BE1D8
		protected virtual void Equip(vEquipItemToArea.AreaToEquip areaToEquip)
		{
			if (this.itemWindow && this.itemWindow.currentSelectedSlot && this.itemWindow.currentSelectedSlot.item)
			{
				areaToEquip.Equip(this.itemWindow.currentSelectedSlot.item);
				this.onEquip.Invoke();
			}
		}

		// Token: 0x0600207D RID: 8317 RVA: 0x000C003C File Offset: 0x000BE23C
		public virtual void Equip(int index)
		{
			if (index < this.areasToEquip.Length)
			{
				vEquipItemToArea.AreaToEquip areaToEquip = this.areasToEquip[index];
				this.Equip(areaToEquip);
			}
		}

		// Token: 0x04002132 RID: 8498
		public vItemWindowDisplay itemWindow;

		// Token: 0x04002133 RID: 8499
		public vEquipItemToArea.AreaToEquip[] areasToEquip;

		// Token: 0x04002134 RID: 8500
		public GenericInput cancelInput = new GenericInput("Escape", "B", "B");

		// Token: 0x04002135 RID: 8501
		public UnityEvent onEquip;

		// Token: 0x04002136 RID: 8502
		public UnityEvent onCancel;

		// Token: 0x04002137 RID: 8503
		protected UnityAction onEquipAction;

		// Token: 0x02000549 RID: 1353
		[Serializable]
		public class AreaToEquip
		{
			// Token: 0x06002080 RID: 8320 RVA: 0x000C0093 File Offset: 0x000BE293
			internal void CheckInput(vItem item, UnityEvent onEquip)
			{
				if (this.input.GetButtonDown())
				{
					this.Equip(item);
					if (onEquip != null)
					{
						onEquip.Invoke();
					}
				}
			}

			// Token: 0x06002081 RID: 8321 RVA: 0x000C00B2 File Offset: 0x000BE2B2
			internal void Equip(vItem item)
			{
				if (this.area)
				{
					this.area.AddItemToEquipSlot(this.slotIndex, item, false);
				}
			}

			// Token: 0x04002138 RID: 8504
			[Tooltip("Target EquipArea to Equip")]
			public vEquipArea area;

			// Token: 0x04002139 RID: 8505
			[Tooltip("Target EquipSlot of the EquipArea to Equip")]
			public int slotIndex;

			// Token: 0x0400213A RID: 8506
			[Tooltip("Optional button to equip on press")]
			public Button optionalButton;

			// Token: 0x0400213B RID: 8507
			[Tooltip("Input to equip on press")]
			public GenericInput input = new GenericInput("Alpha 1", "B", "B");
		}
	}
}
