using System;
using System.Collections.Generic;
using Invector.vItemManager;
using UnityEngine;

// Token: 0x0200012B RID: 299
public class vItemDisplay3D : MonoBehaviour
{
	// Token: 0x06000A0A RID: 2570 RVA: 0x00045BDD File Offset: 0x00043DDD
	public virtual void Display(vItemSlot slot)
	{
		if (slot)
		{
			this.Display(slot.item);
		}
	}

	// Token: 0x06000A0B RID: 2571 RVA: 0x00045BF4 File Offset: 0x00043DF4
	public virtual void Display(int id)
	{
		vItemDisplay3D.vDisplay vDisplay = this.displays.Find((vItemDisplay3D.vDisplay d) => d.itemId.Equals(id));
		if (vDisplay != null)
		{
			if (this.currentItemModel)
			{
				this.currentItemModel.SetActive(false);
			}
			vDisplay.itemModel.SetActive(true);
			this.currentItemModel = vDisplay.itemModel;
		}
	}

	// Token: 0x06000A0C RID: 2572 RVA: 0x00045C5A File Offset: 0x00043E5A
	public virtual void Display(vItem item)
	{
		if (item)
		{
			this.Display(item.id);
		}
	}

	// Token: 0x04000945 RID: 2373
	public GameObject currentItemModel;

	// Token: 0x04000946 RID: 2374
	public List<vItemDisplay3D.vDisplay> displays;

	// Token: 0x0200012C RID: 300
	[Serializable]
	public class vDisplay
	{
		// Token: 0x04000947 RID: 2375
		public int itemId;

		// Token: 0x04000948 RID: 2376
		public GameObject itemModel;
	}
}
