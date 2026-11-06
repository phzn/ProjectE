using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace Invector.vItemManager
{
	// Token: 0x020005D3 RID: 1491
	[vClassHeader("OpenClose Inventory Trigger", false, "icon_v2", false, "")]
	public class vOpenCloseInventoryTrigger : vMonoBehaviour
	{
		// Token: 0x060022B8 RID: 8888 RVA: 0x000C7721 File Offset: 0x000C5921
		protected virtual IEnumerator Start()
		{
			this.inventory = (this.getComponentsInParent ? base.GetComponentInParent<vInventory>() : base.GetComponent<vInventory>());
			if (!this.inventory)
			{
				yield return new WaitForEndOfFrame();
				this.itemManager = (this.getComponentsInParent ? base.GetComponentInParent<vItemManager>() : base.GetComponent<vItemManager>());
				if (this.itemManager)
				{
					this.inventory = this.itemManager.inventory;
				}
			}
			if (this.inventory)
			{
				this.inventory.onOpenCloseInventory.AddListener(new UnityAction<bool>(this.OpenCloseInventory));
			}
			yield break;
		}

		// Token: 0x060022B9 RID: 8889 RVA: 0x000C7730 File Offset: 0x000C5930
		public void OpenCloseInventory(bool value)
		{
			if (value)
			{
				this.onOpen.Invoke();
				return;
			}
			this.onClose.Invoke();
		}

		// Token: 0x04002300 RID: 8960
		public bool getComponentsInParent = true;

		// Token: 0x04002301 RID: 8961
		public vInventory inventory;

		// Token: 0x04002302 RID: 8962
		public vItemManager itemManager;

		// Token: 0x04002303 RID: 8963
		public UnityEvent onOpen;

		// Token: 0x04002304 RID: 8964
		public UnityEvent onClose;
	}
}
