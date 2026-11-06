using System;
using UnityEngine;
using UnityEngine.Events;

namespace Invector.vItemManager
{
	// Token: 0x02000532 RID: 1330
	[vClassHeader("Contains Item Trigger", "Simple trigger to check if the Player has a specific Item, you can also use Events to trigger something if you have the item.", openClose = false)]
	public class vContainsItemTrigger : vMonoBehaviour
	{
		// Token: 0x06002013 RID: 8211 RVA: 0x000BE16C File Offset: 0x000BC36C
		public void OnTriggerEnter(Collider other)
		{
			if (other.gameObject.CompareTag("Player"))
			{
				vItemManager component = other.GetComponent<vItemManager>();
				if (component)
				{
					this.CheckItem(component);
				}
			}
		}

		// Token: 0x06002014 RID: 8212 RVA: 0x000BE1A4 File Offset: 0x000BC3A4
		public void OnTriggerStay(Collider other)
		{
			if (!this.useTriggerStay)
			{
				return;
			}
			if (other.gameObject.CompareTag("Player"))
			{
				this.itemManager = other.GetComponent<vItemManager>();
				if (this.itemManager)
				{
					this.CheckItem(this.itemManager);
				}
			}
		}

		// Token: 0x06002015 RID: 8213 RVA: 0x000BE1F1 File Offset: 0x000BC3F1
		public void OnTriggerExit(Collider other)
		{
			if (other.gameObject.CompareTag("Player"))
			{
				this.onExit.Invoke();
			}
		}

		// Token: 0x06002016 RID: 8214 RVA: 0x000BE210 File Offset: 0x000BC410
		public void RemoveDesiredItem()
		{
			if (this.itemManager)
			{
				if (this.getItemByName)
				{
					if (this.ContainsItem(this.itemManager))
					{
						this.itemManager.DestroyItem(this.itemManager.GetItem(this.itemName), (this.desiredAmount > 1) ? this.desiredAmount : 1);
						return;
					}
				}
				else if (this.ContainsItem(this.itemManager))
				{
					this.itemManager.DestroyItem(this.itemManager.GetItem(this.itemID), (this.desiredAmount > 1) ? this.desiredAmount : 1);
				}
			}
		}

		// Token: 0x06002017 RID: 8215 RVA: 0x000BE2AE File Offset: 0x000BC4AE
		protected virtual void CheckItem(vItemManager itemManager)
		{
			if (itemManager == null)
			{
				return;
			}
			if (this.ContainsItem(itemManager))
			{
				this.onContains.Invoke();
				return;
			}
			this.onNotContains.Invoke();
		}

		// Token: 0x06002018 RID: 8216 RVA: 0x000BE2DC File Offset: 0x000BC4DC
		protected bool ContainsItem(vItemManager itemManager)
		{
			if (this.desiredAmount <= 1)
			{
				if (!this.getItemByName)
				{
					return itemManager.ContainItem(this.itemID);
				}
				return itemManager.ContainItem(this.itemName);
			}
			else
			{
				if (!this.getItemByName)
				{
					return itemManager.ContainItem(this.itemID, this.desiredAmount);
				}
				return itemManager.ContainItem(this.itemName, this.desiredAmount);
			}
		}

		// Token: 0x040020DC RID: 8412
		public bool getItemByName;

		// Token: 0x040020DD RID: 8413
		[vHideInInspector("getItemByName", false)]
		public string itemName;

		// Token: 0x040020DE RID: 8414
		[vHideInInspector("getItemByName", true)]
		public int itemID;

		// Token: 0x040020DF RID: 8415
		public bool useTriggerStay;

		// Token: 0x040020E0 RID: 8416
		public int desiredAmount = 1;

		// Token: 0x040020E1 RID: 8417
		[Header("OnTriggerEnter/Stay")]
		public UnityEvent onContains;

		// Token: 0x040020E2 RID: 8418
		public UnityEvent onNotContains;

		// Token: 0x040020E3 RID: 8419
		[Header("OnTriggerExit")]
		public UnityEvent onExit;

		// Token: 0x040020E4 RID: 8420
		public vItemManager itemManager;
	}
}
