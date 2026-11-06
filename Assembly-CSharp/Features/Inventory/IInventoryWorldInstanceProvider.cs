using System;
using UnityEngine;

namespace Features.Inventory
{
	// Token: 0x020010EA RID: 4330
	public interface IInventoryWorldInstanceProvider
	{
		// Token: 0x06008341 RID: 33601
		InventoryItemSO PrepareWorldInstance(GameObject instance);

		// Token: 0x06008342 RID: 33602
		void CommitWorldInstance();
	}
}
