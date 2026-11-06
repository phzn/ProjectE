using System;
using System.Collections.Generic;
using UnityEngine;

namespace Invector.vItemManager
{
	// Token: 0x020005F0 RID: 1520
	[Serializable]
	public class vAmmo
	{
		// Token: 0x06002338 RID: 9016 RVA: 0x000C9062 File Offset: 0x000C7262
		public vAmmo()
		{
			this.ammoItems = new List<vItem>();
		}

		// Token: 0x06002339 RID: 9017 RVA: 0x000C909C File Offset: 0x000C729C
		public vAmmo(string ammoName, int ammoID, int amount = 0)
		{
			this.ammoName = ammoName;
			this.ammoID = ammoID;
			this._count = amount;
			this.ammoItems = new List<vItem>();
		}

		// Token: 0x0600233A RID: 9018 RVA: 0x000C90F4 File Offset: 0x000C72F4
		public vAmmo(int ammoID, int amount = 0)
		{
			this.ammoID = ammoID;
			this._count = amount;
			this.ammoItems = new List<vItem>();
		}

		// Token: 0x0600233B RID: 9019 RVA: 0x000C9148 File Offset: 0x000C7348
		public vAmmo(vAmmo ammo)
		{
			this.ammoName = ammo.ammoName;
			this.ammoID = ammo.ammoID;
			this.ammoItems = ammo.ammoItems;
			this._count = ammo.count;
			this.ammoItems = new List<vItem>();
		}

		// Token: 0x1700063D RID: 1597
		// (get) Token: 0x0600233C RID: 9020 RVA: 0x000C91BC File Offset: 0x000C73BC
		public virtual int count
		{
			get
			{
				int num = 0;
				if (this.ammoItems != null && this.ammoItems.Count > 0)
				{
					for (int i = 0; i < this.ammoItems.Count; i++)
					{
						if (this.ammoItems[i])
						{
							num += this.ammoItems[i].amount;
						}
					}
				}
				return this._count + num;
			}
		}

		// Token: 0x0600233D RID: 9021 RVA: 0x000C9228 File Offset: 0x000C7428
		public virtual void Use()
		{
			vItem vItem = this.ammoItems.Find((vItem a) => a.amount > 0);
			if (vItem)
			{
				vItem.amount--;
				if (vItem.amount == 0)
				{
					this.ammoItems.Remove(vItem);
					this.onDestroyAmmoItem(vItem);
				}
				return;
			}
			if (this._count > 0)
			{
				this._count--;
			}
		}

		// Token: 0x0600233E RID: 9022 RVA: 0x000C92B0 File Offset: 0x000C74B0
		public virtual void Use(int amout)
		{
			for (int i = 0; i < amout; i++)
			{
				this.Use();
			}
		}

		// Token: 0x0600233F RID: 9023 RVA: 0x000C92CF File Offset: 0x000C74CF
		public void AddAmmo(int amount)
		{
			this._count += amount;
		}

		// Token: 0x04002356 RID: 9046
		public string ammoName;

		// Token: 0x04002357 RID: 9047
		[Tooltip("Ammo ID - if is using ItemManager, make sure your AmmoManager and ItemListData use the same ID")]
		public int ammoID;

		// Token: 0x04002358 RID: 9048
		[Tooltip("Don't need to setup if you're using a Inventory System")]
		[SerializeField]
		private int _count;

		// Token: 0x04002359 RID: 9049
		public List<vItem> ammoItems;

		// Token: 0x0400235A RID: 9050
		public vAmmo.OnDestroyItem onDestroyAmmoItem = delegate
		{
		};

		// Token: 0x020005F1 RID: 1521
		// (Invoke) Token: 0x06002341 RID: 9025
		public delegate void OnDestroyItem(vItem item);
	}
}
