using System;
using UnityEngine;
using UnityEngine.Events;

namespace Invector.vItemManager
{
	// Token: 0x02000525 RID: 1317
	[vClassHeader("Use Item Event Trigger", true, "icon_v2", false, "", useHelpBox = true, helpBoxText = "This script enable ItemUsage when TriggerEnter and disable onTriggerExit", openClose = false)]
	public class UseItemEventTrigger : vMonoBehaviour
	{
		// Token: 0x06001FE7 RID: 8167 RVA: 0x000BD7D0 File Offset: 0x000BB9D0
		public virtual void OnTriggerEnter(Collider other)
		{
			if (other.gameObject.CompareTag("Player"))
			{
				this.itemManager = other.GetComponent<vItemManager>();
				if (this.itemManager)
				{
					this.itemEvent.targetItem = this.itemManager.GetItem(this.itemEvent.id);
					if (this.itemEvent.targetItem)
					{
						this.itemEvent.ChangeItemUsageDelay();
						this.itemManager.onUseItem.AddListener(new UnityAction<vItem>(this.OnUseItem));
						this.itemManager.onOpenCloseInventory.AddListener(new UnityAction<bool>(this.itemEvent.OnOpenInventory));
						this.itemEvent.targetItem.canBeUsed = true;
					}
				}
			}
		}

		// Token: 0x06001FE8 RID: 8168 RVA: 0x000BD89C File Offset: 0x000BBA9C
		protected virtual void OnUseItem(vItem item)
		{
			if (this.itemManager && this.itemEvent.id == item.id)
			{
				this.itemManager.inventory.CloseInventory();
				this.itemManager.onUseItem.RemoveListener(new UnityAction<vItem>(this.OnUseItem));
				this.itemManager.onOpenCloseInventory.RemoveListener(new UnityAction<bool>(this.itemEvent.OnOpenInventory));
				this.itemEvent.onUse.Invoke();
				this.itemEvent.ResetItemUsageDelay();
				this.itemEvent.targetItem = null;
			}
		}

		// Token: 0x06001FE9 RID: 8169 RVA: 0x000BD944 File Offset: 0x000BBB44
		public virtual void OnTriggerExit(Collider other)
		{
			if (other.gameObject.CompareTag("Player") && this.itemManager)
			{
				this.itemManager.onUseItem.RemoveListener(new UnityAction<vItem>(this.OnUseItem));
				this.itemManager.onOpenCloseInventory.RemoveListener(new UnityAction<bool>(this.itemEvent.OnOpenInventory));
				if (this.itemEvent.targetItem)
				{
					this.itemEvent.targetItem.canBeUsed = false;
					this.itemEvent.ResetItemUsageDelay();
					this.itemEvent.targetItem = null;
				}
			}
		}

		// Token: 0x040020A7 RID: 8359
		public UseItemEventTrigger.OnUseItemEvent itemEvent;

		// Token: 0x040020A8 RID: 8360
		protected vItemManager itemManager;

		// Token: 0x02000526 RID: 1318
		[Serializable]
		public class OnUseItemEvent
		{
			// Token: 0x06001FEB RID: 8171 RVA: 0x000BD9EB File Offset: 0x000BBBEB
			public void OnOpenInventory(bool value)
			{
				if (this.canUseWithOpenInventory || !this.targetItem)
				{
					return;
				}
				this.targetItem.canBeUsed = !value;
			}

			// Token: 0x06001FEC RID: 8172 RVA: 0x000BDA12 File Offset: 0x000BBC12
			public void ChangeItemUsageDelay()
			{
				if (!this.overrideItemUsageDelay || this.targetItem == null)
				{
					return;
				}
				this.defaultDelay = this.targetItem.enableDelayTime;
				this.targetItem.enableDelayTime = this.newDeleyTime;
			}

			// Token: 0x06001FED RID: 8173 RVA: 0x000BDA4D File Offset: 0x000BBC4D
			public void ResetItemUsageDelay()
			{
				if (!this.overrideItemUsageDelay || this.targetItem == null)
				{
					return;
				}
				this.targetItem.enableDelayTime = this.defaultDelay;
			}

			// Token: 0x040020A9 RID: 8361
			internal vItem targetItem;

			// Token: 0x040020AA RID: 8362
			public int id;

			// Token: 0x040020AB RID: 8363
			[vHelpBox("Check this to enable the menu UI Button 'Use' on the Inventory Window", vHelpBoxAttribute.MessageType.None)]
			public bool canUseWithOpenInventory;

			// Token: 0x040020AC RID: 8364
			[vHelpBox("Override the Delay to use this Item", vHelpBoxAttribute.MessageType.None)]
			public bool overrideItemUsageDelay;

			// Token: 0x040020AD RID: 8365
			[vHideInInspector("overrideItemUsageDelay", false)]
			public float newDeleyTime;

			// Token: 0x040020AE RID: 8366
			internal float defaultDelay;

			// Token: 0x040020AF RID: 8367
			public UnityEvent onUse;
		}
	}
}
