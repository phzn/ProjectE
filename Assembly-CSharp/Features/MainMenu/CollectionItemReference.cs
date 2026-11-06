using System;
using Features.Inventory;
using UnityEngine;

namespace Features.MainMenu
{
	// Token: 0x02001030 RID: 4144
	public sealed class CollectionItemReference : ScriptableObject
	{
		// Token: 0x17001435 RID: 5173
		// (get) Token: 0x06007D81 RID: 32129 RVA: 0x00336AE0 File Offset: 0x00334CE0
		public InventoryItemSO Item
		{
			get
			{
				return this.item;
			}
		}

		// Token: 0x04008161 RID: 33121
		[SerializeField]
		private InventoryItemSO item;
	}
}
