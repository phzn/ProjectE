using System;
using System.Collections.Generic;
using Invector.vCharacterController.vActions;
using UnityEngine;

namespace Invector.vItemManager
{
	// Token: 0x02000565 RID: 1381
	public class vItemCollection : vTriggerGenericAction
	{
		// Token: 0x06002107 RID: 8455 RVA: 0x000C15A1 File Offset: 0x000BF7A1
		protected override void Start()
		{
			base.Start();
		}

		// Token: 0x040021A7 RID: 8615
		[vEditorToolbar("Item Collection", false, "", false, false)]
		[Header("--- Item Collection Options ---")]
		[Tooltip("List of items you want to use")]
		public vItemListData itemListData;

		// Token: 0x040021A8 RID: 8616
		[Tooltip("Delay to actually collect the items, you can use to match with animations of getting the item")]
		public float onCollectDelay;

		// Token: 0x040021A9 RID: 8617
		[Tooltip("Drag and drop the prefab ItemCollectionDisplay inside the UI gameObject to show a text of the items you've collected")]
		public float textDelay = 0.25f;

		// Token: 0x040021AA RID: 8618
		[Tooltip("Ignore the Enable/Disable animation of your item, you can assign an animation to your item in the ItemListData")]
		public bool ignoreItemAnimation;

		// Token: 0x040021AB RID: 8619
		[HideInInspector]
		public List<vItemType> itemsFilter = new List<vItemType> { vItemType.Consumable };

		// Token: 0x040021AC RID: 8620
		[HideInInspector]
		public List<ItemReference> items = new List<ItemReference>();
	}
}
