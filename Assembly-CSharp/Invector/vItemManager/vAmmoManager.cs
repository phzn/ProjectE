using System;
using System.Collections.Generic;
using Invector.vShooter;
using UnityEngine;
using UnityEngine.Events;

namespace Invector.vItemManager
{
	// Token: 0x020005E5 RID: 1509
	[vClassHeader("Ammo Manager", true, "icon_v2", false, "", iconName = "ammoIcon")]
	public class vAmmoManager : vMonoBehaviour
	{
		// Token: 0x06002312 RID: 8978 RVA: 0x000C8938 File Offset: 0x000C6B38
		private void Start()
		{
			this.itemManager = base.GetComponent<vItemManager>();
			if (this.itemManager)
			{
				this.itemManager.onAddItem.AddListener(new UnityAction<vItem>(this.AddAmmo));
				this.itemManager.onCollectItemRef.AddListener(new UnityAction<ItemReference>(this.AddAmmo));
				this.itemManager.onDropItem.AddListener(new UnityAction<vItem, int>(this.DropAmmo));
				this.itemManager.onDestroyItem.AddListener(new UnityAction<vItem, int>(this.LeaveAmmo));
				this.itemManager.onChangeItemAmount.AddListener(new UnityAction<vItem>(this.ChangeItemAmount));
				this.itemManager.onLoadItems.AddListener(new UnityAction(this.ReloadAllAmmoItems));
			}
			if (this.ammoListData)
			{
				this.ammos.Clear();
				for (int i = 0; i < this.ammoListData.ammos.Count; i++)
				{
					vAmmo vAmmo = new vAmmo(this.ammoListData.ammos[i]);
					vAmmo.onDestroyAmmoItem = new vAmmo.OnDestroyItem(this.OnDestroyAmmoItem);
					this.ammos.Add(vAmmo);
				}
			}
		}

		// Token: 0x06002313 RID: 8979 RVA: 0x000C8A74 File Offset: 0x000C6C74
		public vAmmo GetAmmo(int id)
		{
			return this.ammos.Find((vAmmo a) => a.ammoID == id);
		}

		// Token: 0x06002314 RID: 8980 RVA: 0x000C8AA8 File Offset: 0x000C6CA8
		public void AddAmmo(int id, int amount)
		{
			vAmmo vAmmo = this.ammos.Find((vAmmo a) => a.ammoID == id);
			if (vAmmo != null)
			{
				vAmmo.AddAmmo(amount);
			}
			this.UpdateTotalAmmo();
		}

		// Token: 0x06002315 RID: 8981 RVA: 0x000C8AEC File Offset: 0x000C6CEC
		public void AddAmmo(string ammoName, int id, int amount)
		{
			vAmmo vAmmo = this.ammos.Find((vAmmo a) => a.ammoID == id);
			if (vAmmo == null)
			{
				vAmmo = new vAmmo(ammoName, id, amount);
				this.ammos.Add(vAmmo);
				vAmmo.onDestroyAmmoItem = new vAmmo.OnDestroyItem(this.OnDestroyAmmoItem);
			}
			else if (vAmmo != null)
			{
				vAmmo.AddAmmo(amount);
			}
			this.UpdateTotalAmmo();
		}

		// Token: 0x06002316 RID: 8982 RVA: 0x000C8B60 File Offset: 0x000C6D60
		public void AddAmmo(vItem item)
		{
			if (item.type == vItemType.Ammo)
			{
				vAmmo vAmmo = this.ammos.Find((vAmmo a) => a.ammoID == item.id);
				if (vAmmo == null)
				{
					vAmmo = new vAmmo(item.name, item.id, 0);
					this.ammos.Add(vAmmo);
					vAmmo.onDestroyAmmoItem = new vAmmo.OnDestroyItem(this.OnDestroyAmmoItem);
				}
				vAmmo.ammoItems.Add(item);
			}
			else
			{
				vItemAttribute itemAttribute = item.GetItemAttribute(vItemAttributes.AmmoCount);
				if (itemAttribute != null)
				{
					vShooterWeapon component = item.originalObject.GetComponent<vShooterWeapon>();
					if (component != null && itemAttribute != null && itemAttribute.value > component.clipSize)
					{
						int num = itemAttribute.value - component.clipSize;
						itemAttribute.value -= num;
						ItemReference itemReference = new ItemReference(component.ammoID);
						itemReference.amount = num;
						this.itemManager.CollectItem(itemReference, 0f, 0f, true);
					}
				}
			}
			this.UpdateTotalAmmo();
		}

		// Token: 0x06002317 RID: 8983 RVA: 0x000C8C84 File Offset: 0x000C6E84
		public void AddAmmo(ItemReference item)
		{
			vItemAttribute vItemAttribute = item.attributes.Find((vItemAttribute a) => a.name == vItemAttributes.AmmoCount);
			vItem vItem = this.itemManager.itemListData.items.Find((vItem i) => i.id == item.id);
			if (vItemAttribute != null && vItem != null)
			{
				vShooterWeapon component = vItem.originalObject.GetComponent<vShooterWeapon>();
				if (component != null)
				{
					ItemReference itemReference = new ItemReference(component.ammoID);
					itemReference.amount = vItemAttribute.value;
					this.itemManager.CollectItem(itemReference, 0f, 0f, true);
				}
			}
			this.UpdateTotalAmmo();
		}

		// Token: 0x06002318 RID: 8984 RVA: 0x000C8D4C File Offset: 0x000C6F4C
		protected void ChangeItemAmount(vItem item)
		{
			if (item.type == vItemType.Ammo && this.ammos.Find((vAmmo a) => a.ammoID == item.id) == null)
			{
				vAmmo vAmmo = new vAmmo(item.name, item.id, item.amount);
				this.ammos.Add(vAmmo);
				vAmmo.onDestroyAmmoItem = new vAmmo.OnDestroyItem(this.OnDestroyAmmoItem);
			}
			this.UpdateTotalAmmo();
		}

		// Token: 0x06002319 RID: 8985 RVA: 0x000C8DDC File Offset: 0x000C6FDC
		public void LeaveAmmo(vItem item, int amount)
		{
			if (item.type == vItemType.Ammo)
			{
				vAmmo vAmmo = this.ammos.Find((vAmmo a) => a.ammoID == item.id);
				if (vAmmo != null && item.amount - amount <= 0 && vAmmo.ammoItems.Contains(item))
				{
					vAmmo.ammoItems.Remove(item);
				}
			}
			this.UpdateTotalAmmo();
		}

		// Token: 0x0600231A RID: 8986 RVA: 0x000C8E5C File Offset: 0x000C705C
		public void DropAmmo(vItem item, int amount)
		{
			if (item.type == vItemType.Ammo)
			{
				vAmmo vAmmo = this.ammos.Find((vAmmo a) => a.ammoID == item.id);
				if (vAmmo != null && item.amount - amount <= 0 && vAmmo.ammoItems.Contains(item))
				{
					vAmmo.ammoItems.Remove(item);
				}
			}
			this.UpdateTotalAmmo();
		}

		// Token: 0x0600231B RID: 8987 RVA: 0x000C8EDA File Offset: 0x000C70DA
		public void UpdateTotalAmmo()
		{
			this.updateTotalAmmo();
		}

		// Token: 0x0600231C RID: 8988 RVA: 0x000C8EE8 File Offset: 0x000C70E8
		public void ReloadAllAmmoItems()
		{
			List<vItem> list = this.itemManager.items.FindAll((vItem item) => item.type == vItemType.Ammo);
			this.ammos.Clear();
			for (int i = 0; i < list.Count; i++)
			{
				this.AddAmmo(list[i]);
			}
		}

		// Token: 0x0600231D RID: 8989 RVA: 0x000C8F4E File Offset: 0x000C714E
		private void OnDestroyAmmoItem(vItem item)
		{
			if (this.itemManager)
			{
				this.itemManager.DestroyItem(item, item.amount);
			}
		}

		// Token: 0x04002346 RID: 9030
		public vAmmoListData ammoListData;

		// Token: 0x04002347 RID: 9031
		[HideInInspector]
		public vItemManager itemManager;

		// Token: 0x04002348 RID: 9032
		public List<vAmmo> ammos = new List<vAmmo>();

		// Token: 0x04002349 RID: 9033
		public vAmmoManager.OnUpdateTotalAmmo updateTotalAmmo = delegate
		{
		};

		// Token: 0x020005E6 RID: 1510
		// (Invoke) Token: 0x06002320 RID: 8992
		public delegate void OnUpdateTotalAmmo();
	}
}
