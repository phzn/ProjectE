using System;
using System.Collections.Generic;
using UnityEngine;

namespace Invector.vItemManager
{
	// Token: 0x02000571 RID: 1393
	[CreateAssetMenu(menuName = "Invector/Inventory/New Item List")]
	public class vItemListData : ScriptableObject
	{
		// Token: 0x040021E3 RID: 8675
		public List<vItem> items = new List<vItem>();

		// Token: 0x040021E4 RID: 8676
		public bool inEdition;

		// Token: 0x040021E5 RID: 8677
		public bool itemsHidden = true;
	}
}
