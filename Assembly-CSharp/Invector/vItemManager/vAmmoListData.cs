using System;
using System.Collections.Generic;
using UnityEngine;

namespace Invector.vItemManager
{
	// Token: 0x020005E4 RID: 1508
	public class vAmmoListData : ScriptableObject
	{
		// Token: 0x04002344 RID: 9028
		[vHelpBox("Leave the Count value at 0 if you want to use ammo from the Inventory Attribute", vHelpBoxAttribute.MessageType.Info)]
		public List<vItemListData> itemListDatas;

		// Token: 0x04002345 RID: 9029
		[HideInInspector]
		public List<vAmmo> ammos = new List<vAmmo>();
	}
}
