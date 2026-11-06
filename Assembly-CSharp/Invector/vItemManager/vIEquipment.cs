using System;
using UnityEngine;

namespace Invector.vItemManager
{
	// Token: 0x02000552 RID: 1362
	[Obsolete("Class is no longer used and will be removed in the future")]
	public interface vIEquipment
	{
		// Token: 0x170005FB RID: 1531
		// (get) Token: 0x060020A5 RID: 8357
		Transform transform { get; }

		// Token: 0x170005FC RID: 1532
		// (get) Token: 0x060020A6 RID: 8358
		GameObject gameObject { get; }

		// Token: 0x170005FD RID: 1533
		// (get) Token: 0x060020A7 RID: 8359
		bool isEquiped { get; }

		// Token: 0x170005FE RID: 1534
		// (get) Token: 0x060020A8 RID: 8360
		// (set) Token: 0x060020A9 RID: 8361
		EquipPoint equipPoint { get; set; }

		// Token: 0x170005FF RID: 1535
		// (get) Token: 0x060020AA RID: 8362
		vItem referenceItem { get; }

		// Token: 0x060020AB RID: 8363
		void OnEquip(vItem item);

		// Token: 0x060020AC RID: 8364
		void OnUnequip(vItem item);
	}
}
