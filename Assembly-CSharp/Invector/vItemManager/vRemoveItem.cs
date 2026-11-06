using System;
using UnityEngine;

namespace Invector.vItemManager
{
	// Token: 0x02000535 RID: 1333
	[vClassHeader("Remove Item", true, "icon_v2", false, "", openClose = false)]
	public class vRemoveItem : vMonoBehaviour
	{
		// Token: 0x0600201C RID: 8220 RVA: 0x000BE3E8 File Offset: 0x000BC5E8
		public void RemoveItem(Collider target)
		{
			vItemManager component = target.GetComponent<vItemManager>();
			this.RemoveItem(component);
		}

		// Token: 0x0600201D RID: 8221 RVA: 0x000BE404 File Offset: 0x000BC604
		public void RemoveItem(GameObject target)
		{
			vItemManager component = target.GetComponent<vItemManager>();
			this.RemoveItem(component);
		}

		// Token: 0x0600201E RID: 8222 RVA: 0x000BE420 File Offset: 0x000BC620
		public void RemoveItem(vItemManager itemManager)
		{
			if (itemManager)
			{
				vItem item = this.GetItem(itemManager);
				if (item != null)
				{
					if (this.type == vRemoveCurrentItem.Type.UnequipItem)
					{
						itemManager.UnequipItem(item, true);
						return;
					}
					if (this.type == vRemoveCurrentItem.Type.DestroyItem)
					{
						itemManager.DestroyItem(item, 1);
						return;
					}
					itemManager.DropItem(item, 1);
				}
			}
		}

		// Token: 0x0600201F RID: 8223 RVA: 0x000BE474 File Offset: 0x000BC674
		private vItem GetItem(vItemManager itemManager)
		{
			if (this.getItemByName)
			{
				if (itemManager.ContainItem(this.itemName))
				{
					return itemManager.GetItem(this.itemName);
				}
			}
			else if (itemManager.ContainItem(this.itemID))
			{
				return itemManager.GetItem(this.itemID);
			}
			return null;
		}

		// Token: 0x040020ED RID: 8429
		public vRemoveCurrentItem.Type type = vRemoveCurrentItem.Type.DestroyItem;

		// Token: 0x040020EE RID: 8430
		public bool getItemByName;

		// Token: 0x040020EF RID: 8431
		[vHideInInspector("getItemByName", false)]
		public string itemName;

		// Token: 0x040020F0 RID: 8432
		[vHideInInspector("getItemByName", true)]
		public int itemID;
	}
}
