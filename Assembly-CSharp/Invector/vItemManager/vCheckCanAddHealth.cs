using System;
using System.Collections.Generic;
using Invector.vCharacterController;

namespace Invector.vItemManager
{
	// Token: 0x02000528 RID: 1320
	[vClassHeader("Check if can Add Health", "Simple Example to verify if the health item can be used based on the character's health is full or not.", openClose = false)]
	public class vCheckCanAddHealth : vMonoBehaviour
	{
		// Token: 0x06001FF1 RID: 8177 RVA: 0x000BDB1C File Offset: 0x000BBD1C
		protected virtual void Start()
		{
			if (this.itemManager == null)
			{
				if (this.getInParent)
				{
					this.itemManager = base.GetComponentInParent<vItemManager>();
				}
				else
				{
					this.itemManager = base.GetComponent<vItemManager>();
				}
			}
			if (this.tpController == null)
			{
				if (this.getInParent)
				{
					this.tpController = base.GetComponentInParent<vThirdPersonController>();
				}
				else
				{
					this.tpController = base.GetComponent<vThirdPersonController>();
				}
			}
			if (this.itemManager)
			{
				this.itemManager.canUseItemDelegate -= this.CanUseItem;
				this.itemManager.canUseItemDelegate += this.CanUseItem;
			}
		}

		// Token: 0x06001FF2 RID: 8178 RVA: 0x000BDBC8 File Offset: 0x000BBDC8
		protected virtual void OnDestroy()
		{
			vItemManager component = base.GetComponent<vItemManager>();
			if (component)
			{
				component.canUseItemDelegate -= this.CanUseItem;
			}
		}

		// Token: 0x06001FF3 RID: 8179 RVA: 0x000BDBF8 File Offset: 0x000BBDF8
		protected virtual void CanUseItem(vItem item, ref List<bool> validateResult)
		{
			if (item.GetItemAttribute(vItemAttributes.Health) != null)
			{
				bool flag = this.tpController.currentHealth < (float)this.tpController.maxHealth;
				if (flag != this.canUse || !this.firstRun)
				{
					this.canUse = flag;
					this.firstRun = true;
					vHUDController.instance.ShowText(flag ? "Increase health" : ("Can't use " + item.name + " because your health is full"), 4f, 0.5f);
				}
				if (!flag)
				{
					validateResult.Add(flag);
				}
			}
		}

		// Token: 0x040020B6 RID: 8374
		public vItemManager itemManager;

		// Token: 0x040020B7 RID: 8375
		public vThirdPersonController tpController;

		// Token: 0x040020B8 RID: 8376
		public bool getInParent = true;

		// Token: 0x040020B9 RID: 8377
		protected bool canUse;

		// Token: 0x040020BA RID: 8378
		protected bool firstRun;
	}
}
