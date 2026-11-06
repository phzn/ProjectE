using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Invector.vItemManager
{
	// Token: 0x0200054D RID: 1357
	public class vEquipmentReferenceControl : MonoBehaviour
	{
		// Token: 0x06002093 RID: 8339 RVA: 0x000C0227 File Offset: 0x000BE427
		protected virtual void Awake()
		{
			vItemManager componentInParent = base.GetComponentInParent<vItemManager>();
			componentInParent.onEquipItem.AddListener(new UnityAction<vEquipArea, vItem>(this.OnEquip));
			componentInParent.onUnequipItem.AddListener(new UnityAction<vEquipArea, vItem>(this.OnUniquip));
		}

		// Token: 0x06002094 RID: 8340 RVA: 0x000C025E File Offset: 0x000BE45E
		protected virtual void OnEquip(vEquipArea equipArea, vItem equipment)
		{
			if (equipment)
			{
				this.SetActiveEquipment(equipment, true);
			}
		}

		// Token: 0x06002095 RID: 8341 RVA: 0x000C0270 File Offset: 0x000BE470
		protected virtual void OnUniquip(vEquipArea equipArea, vItem equipment)
		{
			if (equipment)
			{
				this.SetActiveEquipment(equipment, false);
			}
		}

		// Token: 0x06002096 RID: 8342 RVA: 0x000C0284 File Offset: 0x000BE484
		public virtual void SetActiveEquipment(vItem item, bool active)
		{
			List<vEquipmentReferenceControl.vEquipmentReference> list = this.equipmentReferences.FindAll((vEquipmentReferenceControl.vEquipmentReference e) => e.id.Equals(item.id));
			for (int i = 0; i < list.Count; i++)
			{
				if (list[i].equipment)
				{
					list[i].equipment.gameObject.SetActive(active);
					if (active)
					{
						list[i].equipment.OnEquip(item);
					}
					else
					{
						list[i].equipment.OnUnequip(item);
					}
				}
			}
		}

		// Token: 0x0400214A RID: 8522
		public List<vEquipmentReferenceControl.vEquipmentReference> equipmentReferences;

		// Token: 0x0200054E RID: 1358
		[Serializable]
		public class vEquipmentReference
		{
			// Token: 0x0400214B RID: 8523
			public string name;

			// Token: 0x0400214C RID: 8524
			public int id;

			// Token: 0x0400214D RID: 8525
			public vEquipment equipment;
		}
	}
}
