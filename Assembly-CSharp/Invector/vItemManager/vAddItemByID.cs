using System;
using UnityEngine;

namespace Invector.vItemManager
{
	// Token: 0x02000527 RID: 1319
	[vClassHeader("Add Item By ID", "This is a simple example on how to add items using script", openClose = false)]
	public class vAddItemByID : vMonoBehaviour
	{
		// Token: 0x06001FEF RID: 8175 RVA: 0x000BDA78 File Offset: 0x000BBC78
		private void OnTriggerEnter(Collider other)
		{
			if (other.gameObject.CompareTag("Player"))
			{
				vItemManager component = other.gameObject.GetComponent<vItemManager>();
				if (component)
				{
					component.CollectItem(new ItemReference(this.id)
					{
						amount = this.amount,
						addToEquipArea = this.addToEquipArea,
						autoEquip = this.autoEquip,
						indexArea = this.indexOfEquipArea
					}, 0f, 2f, false);
					if (this.destroyAfter)
					{
						Object.Destroy(base.gameObject);
					}
				}
			}
		}

		// Token: 0x040020B0 RID: 8368
		public int id;

		// Token: 0x040020B1 RID: 8369
		public int amount;

		// Token: 0x040020B2 RID: 8370
		public bool addToEquipArea = true;

		// Token: 0x040020B3 RID: 8371
		[vHideInInspector("addToEquipArea", false)]
		public bool autoEquip;

		// Token: 0x040020B4 RID: 8372
		public bool destroyAfter;

		// Token: 0x040020B5 RID: 8373
		[vHideInInspector("addToEquipArea", false)]
		public int indexOfEquipArea;
	}
}
