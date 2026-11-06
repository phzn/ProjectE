using System;
using UnityEngine;
using UnityEngine.Events;

namespace Invector.vItemManager
{
	// Token: 0x02000567 RID: 1383
	public class vItemCollectionDisplay_v2 : MonoBehaviour
	{
		// Token: 0x0600210C RID: 8460 RVA: 0x000C1629 File Offset: 0x000BF829
		protected virtual void Start()
		{
			this.itemManager.onCollectItem.AddListener(new UnityAction<vItemManager.CollectedItemInfo>(this.OnAddItem));
		}

		// Token: 0x0600210D RID: 8461 RVA: 0x000C1648 File Offset: 0x000BF848
		protected virtual void OnAddItem(vItemManager.CollectedItemInfo info)
		{
			vItemDisplay vItemDisplay = Object.Instantiate<vItemDisplay>(this.displayPrefab);
			vItemDisplay.transform.SetParent(this.content, false);
			vItemDisplay.DisplayItem(info);
			vItemDisplay.transform.SetAsFirstSibling();
			Object.Destroy(vItemDisplay.gameObject, this.displayTime);
		}

		// Token: 0x040021AF RID: 8623
		public vItemManager itemManager;

		// Token: 0x040021B0 RID: 8624
		public vItemDisplay displayPrefab;

		// Token: 0x040021B1 RID: 8625
		public RectTransform content;

		// Token: 0x040021B2 RID: 8626
		public float displayTime = 3f;
	}
}
