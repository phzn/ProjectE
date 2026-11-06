using System;

namespace Features.Inventory
{
	// Token: 0x020010E9 RID: 4329
	public interface IInventoryStackMergeable
	{
		// Token: 0x0600833F RID: 33599
		bool CanMergeWith(InventoryItemSO incoming);

		// Token: 0x06008340 RID: 33600
		bool MergeFrom(InventoryItemSO incoming);
	}
}
