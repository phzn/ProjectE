using System;
using UnityEngine;

namespace Features.Inventory
{
	// Token: 0x020010EB RID: 4331
	public interface IInventoryWholeStackWorldProvider
	{
		// Token: 0x170014CD RID: 5325
		// (get) Token: 0x06008343 RID: 33603
		int InventoryStackQuantity { get; }

		// Token: 0x06008344 RID: 33604
		InventoryItemSO PrepareWholeStackWorldInstance(GameObject instance);
	}
}
