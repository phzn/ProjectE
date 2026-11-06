using System;
using UnityEngine;
using UnityEngine.Events;

namespace Invector.vItemManager
{
	// Token: 0x02000533 RID: 1331
	[vClassHeader("Remove Current Item", false, "icon_v2", false, "")]
	public class vRemoveCurrentItem : vMonoBehaviour
	{
		// Token: 0x0600201A RID: 8218 RVA: 0x000BE350 File Offset: 0x000BC550
		private void OnTriggerEnter(Collider other)
		{
			if (other.gameObject.CompareTag("Player"))
			{
				vItemManager component = other.gameObject.GetComponent<vItemManager>();
				if (component)
				{
					if (this.type == vRemoveCurrentItem.Type.UnequipItem)
					{
						component.UnequipCurrentEquipedItem(this.equipArea, this.immediate);
					}
					else if (this.type == vRemoveCurrentItem.Type.DestroyItem)
					{
						component.DestroyCurrentEquipedItem(this.equipArea, this.immediate);
					}
					else
					{
						component.DropCurrentEquippedItem(this.equipArea, this.immediate);
					}
				}
				this.OnTriggerEnterEvent.Invoke();
			}
		}

		// Token: 0x040020E5 RID: 8421
		public vRemoveCurrentItem.Type type;

		// Token: 0x040020E6 RID: 8422
		[Tooltip("Immediately equip the item ignoring the Equip animation")]
		public bool immediate = true;

		// Token: 0x040020E7 RID: 8423
		[Tooltip("Equip Area of your Inventory Prefab")]
		public int equipArea;

		// Token: 0x040020E8 RID: 8424
		public UnityEvent OnTriggerEnterEvent;

		// Token: 0x02000534 RID: 1332
		public enum Type
		{
			// Token: 0x040020EA RID: 8426
			UnequipItem,
			// Token: 0x040020EB RID: 8427
			DestroyItem,
			// Token: 0x040020EC RID: 8428
			DropItem
		}
	}
}
