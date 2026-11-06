using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Invector.vItemManager
{
	// Token: 0x0200052E RID: 1326
	[vClassHeader("Check Items in Inventory", true, "icon_v2", false, "", openClose = false)]
	public class vCheckItemsInInventory : vMonoBehaviour
	{
		// Token: 0x06002005 RID: 8197 RVA: 0x000BDFD3 File Offset: 0x000BC1D3
		private IEnumerator Start()
		{
			yield return new WaitForEndOfFrame();
			if (this.itemManager)
			{
				this.itemManager.inventory.OnUpdateInventory += this.CheckItemExists;
			}
			yield break;
		}

		// Token: 0x06002006 RID: 8198 RVA: 0x000BDFE2 File Offset: 0x000BC1E2
		private void OnValidate()
		{
			if (!this.itemManager)
			{
				this.itemManager = base.GetComponent<vItemManager>();
				if (!this.itemManager)
				{
					this.itemManager = base.GetComponentInParent<vItemManager>();
				}
			}
		}

		// Token: 0x06002007 RID: 8199 RVA: 0x000BE018 File Offset: 0x000BC218
		public void CheckItemExists()
		{
			for (int i = 0; i < this.itemIDEvents.Count; i++)
			{
				vCheckItemsInInventory.CheckItemIDEvent checkItemIDEvent = this.itemIDEvents[i];
				this.CheckItemID(checkItemIDEvent);
			}
		}

		// Token: 0x06002008 RID: 8200 RVA: 0x000BE04F File Offset: 0x000BC24F
		private void CheckItemID(vCheckItemsInInventory.CheckItemIDEvent check)
		{
			if (check.Check(this.itemManager))
			{
				check.onContainItem.Invoke();
				return;
			}
			check.onNotContainItem.Invoke();
		}

		// Token: 0x040020D0 RID: 8400
		public vItemManager itemManager;

		// Token: 0x040020D1 RID: 8401
		public List<vCheckItemsInInventory.CheckItemIDEvent> itemIDEvents;

		// Token: 0x0200052F RID: 1327
		[Serializable]
		public class CheckItemIDEvent
		{
			// Token: 0x0600200A RID: 8202 RVA: 0x000BE078 File Offset: 0x000BC278
			public bool Check(vItemManager itemManager)
			{
				bool flag = true;
				for (int i = 0; i < this.itemIds.Count; i++)
				{
					vCheckItemsInInventory.ItemID itemID = this.itemIds[i];
					if (itemID.verifyAmmount && itemManager.GetAllAmount(itemID.id) < itemID.ammount)
					{
						flag = false;
						break;
					}
					if (!itemID.verifyAmmount && !itemManager.ContainItem(itemID.id))
					{
						flag = false;
						break;
					}
				}
				return flag;
			}

			// Token: 0x040020D2 RID: 8402
			public string name;

			// Token: 0x040020D3 RID: 8403
			public List<vCheckItemsInInventory.ItemID> itemIds;

			// Token: 0x040020D4 RID: 8404
			public UnityEvent onContainItem;

			// Token: 0x040020D5 RID: 8405
			public UnityEvent onNotContainItem;
		}

		// Token: 0x02000530 RID: 1328
		[Serializable]
		public class ItemID
		{
			// Token: 0x040020D6 RID: 8406
			public int id;

			// Token: 0x040020D7 RID: 8407
			public bool verifyAmmount;

			// Token: 0x040020D8 RID: 8408
			public int ammount;
		}
	}
}
