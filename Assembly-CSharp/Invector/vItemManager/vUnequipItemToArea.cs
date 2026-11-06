using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Invector.vItemManager
{
	// Token: 0x020005DB RID: 1499
	public class vUnequipItemToArea : MonoBehaviour
	{
		// Token: 0x060022E0 RID: 8928 RVA: 0x000C7C70 File Offset: 0x000C5E70
		private void Start()
		{
			this.equipAreas = base.GetComponentsInChildren<vEquipArea>().vToList<vEquipArea>();
			foreach (vEquipArea vEquipArea in this.equipAreas)
			{
				vEquipArea.onSelectEquipArea.AddListener(new UnityAction<vEquipArea>(this.OnSelectArea));
			}
			this.inventory = base.GetComponentInParent<vInventory>();
		}

		// Token: 0x060022E1 RID: 8929 RVA: 0x000C7CF0 File Offset: 0x000C5EF0
		public void OnSelectArea(vEquipArea area)
		{
			this.equipArea = area;
		}

		// Token: 0x17000632 RID: 1586
		// (get) Token: 0x060022E2 RID: 8930 RVA: 0x000C7CF9 File Offset: 0x000C5EF9
		protected vEquipSlot currentSlot
		{
			get
			{
				if (!this.equipArea)
				{
					return null;
				}
				if (!this.equipArea.currentSelectedSlot)
				{
					return this.equipArea.lastSelectedSlot;
				}
				return this.equipArea.currentSelectedSlot;
			}
		}

		// Token: 0x060022E3 RID: 8931 RVA: 0x000C7D34 File Offset: 0x000C5F34
		public void UnequipItem()
		{
			if (this.equipArea && this.currentSlot != null && this.currentSlot.item != null)
			{
				this.equipArea.RemoveItemOfEquipSlot(this.currentSlot);
			}
		}

		// Token: 0x0400231D RID: 8989
		[HideInInspector]
		public List<vEquipArea> equipAreas;

		// Token: 0x0400231E RID: 8990
		protected vEquipArea equipArea;

		// Token: 0x0400231F RID: 8991
		protected vInventory inventory;
	}
}
