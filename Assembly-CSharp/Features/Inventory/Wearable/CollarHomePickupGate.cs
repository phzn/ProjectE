using System;
using Features.Inventory.Placement;
using Features.Rank;
using UnityEngine;

namespace Features.Inventory.Wearable
{
	// Token: 0x0200110E RID: 4366
	[DisallowMultipleComponent]
	public sealed class CollarHomePickupGate : MonoBehaviour
	{
		// Token: 0x06008487 RID: 33927 RVA: 0x0036019E File Offset: 0x0035E39E
		private void OnEnable()
		{
			RankProgressionService.Changed += this.RefreshAvailability;
			this.BindInventory();
			this.RefreshAvailability();
		}

		// Token: 0x06008488 RID: 33928 RVA: 0x003601BD File Offset: 0x0035E3BD
		private void OnDisable()
		{
			RankProgressionService.Changed -= this.RefreshAvailability;
			this.UnbindInventory();
		}

		// Token: 0x06008489 RID: 33929 RVA: 0x003601D6 File Offset: 0x0035E3D6
		private void Update()
		{
			if (Time.unscaledTime < this._nextInventoryCheck)
			{
				return;
			}
			this._nextInventoryCheck = Time.unscaledTime + 0.5f;
			if (this._inventory != PlayerInventoryManager.Instance)
			{
				this.BindInventory();
				this.RefreshAvailability();
			}
		}

		// Token: 0x0600848A RID: 33930 RVA: 0x00360215 File Offset: 0x0035E415
		private void BindInventory()
		{
			this.UnbindInventory();
			this._inventory = PlayerInventoryManager.Instance;
			if (this._inventory != null)
			{
				this._inventory.OnClothAdded += this.OnClothAdded;
			}
		}

		// Token: 0x0600848B RID: 33931 RVA: 0x0036024D File Offset: 0x0035E44D
		private void UnbindInventory()
		{
			if (this._inventory != null)
			{
				this._inventory.OnClothAdded -= this.OnClothAdded;
			}
			this._inventory = null;
		}

		// Token: 0x0600848C RID: 33932 RVA: 0x0036027B File Offset: 0x0035E47B
		private void OnClothAdded(int slot, InventoryItemSO item)
		{
			if (!this.IsThisPickup(item))
			{
				return;
			}
			this._claimedThisVisit = true;
			this.RefreshAvailability();
		}

		// Token: 0x0600848D RID: 33933 RVA: 0x00360294 File Offset: 0x0035E494
		public void RefreshAvailability()
		{
			if (this.pickupRoot == null || this.pickupRoot == base.gameObject)
			{
				return;
			}
			if (this._inventory != null)
			{
				for (int i = 0; i < this._inventory.MaxClothSlots; i++)
				{
					if (this.IsThisPickup(this._inventory.GetClothAt(i)))
					{
						this._claimedThisVisit = true;
						break;
					}
				}
			}
			bool flag = CollarAvailabilityPolicy.IsRankUnlocked && !this._claimedThisVisit;
			if (this.pickupRoot.activeSelf != flag)
			{
				this.pickupRoot.SetActive(flag);
			}
		}

		// Token: 0x0600848E RID: 33934 RVA: 0x00360334 File Offset: 0x0035E534
		private bool IsThisPickup(InventoryItemSO item)
		{
			if (!CollarAvailabilityPolicy.IsCollar(item))
			{
				return false;
			}
			if (this.itemData == null && this.pickupRoot != null)
			{
				InventoryPickup componentInChildren = this.pickupRoot.GetComponentInChildren<InventoryPickup>(true);
				this.itemData = ((componentInChildren != null) ? componentInChildren.ItemData : null);
			}
			return item.itemId == ((this.itemData != null) ? this.itemData.itemId : "cloth_collar");
		}

		// Token: 0x04008751 RID: 34641
		[SerializeField]
		private GameObject pickupRoot;

		// Token: 0x04008752 RID: 34642
		[SerializeField]
		private InventoryItemSO itemData;

		// Token: 0x04008753 RID: 34643
		private PlayerInventoryManager _inventory;

		// Token: 0x04008754 RID: 34644
		private float _nextInventoryCheck;

		// Token: 0x04008755 RID: 34645
		private bool _claimedThisVisit;
	}
}
