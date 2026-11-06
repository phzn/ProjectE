using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Invector.vItemManager
{
	// Token: 0x02000529 RID: 1321
	[vClassHeader("Check Item in Inventory", true, "icon_v2", false, "", openClose = false)]
	public class vCheckItemInInventory : vMonoBehaviour
	{
		// Token: 0x06001FF5 RID: 8181 RVA: 0x000BDC94 File Offset: 0x000BBE94
		private void Awake()
		{
			if (!this.itemManager)
			{
				if (this.getInParent)
				{
					this.itemManager = base.GetComponentInParent<vItemManager>();
				}
				else
				{
					this.itemManager = base.GetComponent<vItemManager>();
				}
				if (this.itemManager)
				{
					this.itemManager.onAddItemID.AddListener(new UnityAction<int>(this.CheckItemExists));
					this.itemManager.onRemoveItemID.AddListener(new UnityAction<int>(this.CheckItemExists));
				}
			}
		}

		// Token: 0x06001FF6 RID: 8182 RVA: 0x000BDD18 File Offset: 0x000BBF18
		public void CheckOnTrigger(Collider collider)
		{
			if (collider != null)
			{
				this.itemManager = collider.gameObject.GetComponent<vItemManager>();
				if (this.itemManager)
				{
					for (int i = 0; i < this.itemIDEvents.Count; i++)
					{
						vCheckItemInInventory.CheckItemIDEvent checkItemIDEvent = this.itemIDEvents[i];
						this.CheckItemID(checkItemIDEvent);
					}
				}
			}
		}

		// Token: 0x06001FF7 RID: 8183 RVA: 0x000BDD78 File Offset: 0x000BBF78
		private void CheckItemExists(int arg1)
		{
			for (int i = 0; i < this.itemIDEvents.Count; i++)
			{
				vCheckItemInInventory.CheckItemIDEvent checkItemIDEvent = this.itemIDEvents[i];
				this.CheckItemID(checkItemIDEvent);
			}
		}

		// Token: 0x06001FF8 RID: 8184 RVA: 0x000BDDAF File Offset: 0x000BBFAF
		private void CheckItemID(vCheckItemInInventory.CheckItemIDEvent check)
		{
			if (check.Check(this.itemManager))
			{
				check.onContainItem.Invoke();
				return;
			}
			check.onNotContainItem.Invoke();
		}

		// Token: 0x040020BB RID: 8379
		protected vItemManager itemManager;

		// Token: 0x040020BC RID: 8380
		public bool getInParent = true;

		// Token: 0x040020BD RID: 8381
		public List<vCheckItemInInventory.CheckItemIDEvent> itemIDEvents;

		// Token: 0x0200052A RID: 1322
		[Serializable]
		public class CheckItemIDEvent
		{
			// Token: 0x06001FFA RID: 8186 RVA: 0x000BDDE8 File Offset: 0x000BBFE8
			public bool Check(vItemManager itemManager)
			{
				bool flag = true;
				for (int i = 0; i < this._itemsID.Count; i++)
				{
					if (!itemManager.ContainItem(this._itemsID[i]))
					{
						flag = false;
						break;
					}
				}
				return flag;
			}

			// Token: 0x040020BE RID: 8382
			public string name;

			// Token: 0x040020BF RID: 8383
			public List<int> _itemsID;

			// Token: 0x040020C0 RID: 8384
			public UnityEvent onContainItem;

			// Token: 0x040020C1 RID: 8385
			public UnityEvent onNotContainItem;
		}
	}
}
