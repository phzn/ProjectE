using System;
using UnityEngine;

namespace Features.Inventory
{
	// Token: 0x020010FA RID: 4346
	[Serializable]
	public class RarityVariantEntry
	{
		// Token: 0x04008678 RID: 34424
		public ItemRarity tier;

		// Token: 0x04008679 RID: 34425
		[Min(0f)]
		public int surfaceIndex;

		// Token: 0x0400867A RID: 34426
		public string surfaceName;
	}
}
