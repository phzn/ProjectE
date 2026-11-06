using System;
using System.Collections.Generic;
using Features.Inventory.Wearable;
using UnityEngine;

namespace Features.Inventory
{
	// Token: 0x020010FF RID: 4351
	public class ManualDressInventoryWatcher : MonoBehaviour
	{
		// Token: 0x0600839F RID: 33695 RVA: 0x0035B620 File Offset: 0x00359820
		private void Awake()
		{
			if (this.inventory == null)
			{
				this.inventory = PlayerInventoryManager.Instance ?? base.GetComponentInParent<PlayerInventoryManager>();
			}
			if (this.wearController == null)
			{
				this.wearController = base.GetComponentInParent<WearableUseController>() ?? Object.FindObjectOfType<WearableUseController>(true);
			}
			if (this.playerAnimator == null)
			{
				this.playerAnimator = base.GetComponentInParent<Animator>();
			}
			if (!WardrobeGameplayContext.IsPresentationOnly(this))
			{
				this.BuildLookup();
			}
		}

		// Token: 0x060083A0 RID: 33696 RVA: 0x0035B69C File Offset: 0x0035989C
		private void BuildLookup()
		{
			this._byName = new Dictionary<string, WearableItemSO>();
			foreach (WearableItemSO wearableItemSO in Resources.LoadAll<WearableItemSO>(this.wardrobeResourcesPath ?? string.Empty))
			{
				if (!(wearableItemSO == null) && !(wearableItemSO.mcStylePreset == null) && !(wearableItemSO.mcStylePreset.physicalPrefab == null) && wearableItemSO.showInGameplayInventory)
				{
					string name = wearableItemSO.mcStylePreset.physicalPrefab.name;
					if (!string.IsNullOrEmpty(name) && !this._byName.ContainsKey(name))
					{
						this._byName[name] = wearableItemSO;
					}
				}
			}
		}

		// Token: 0x060083A1 RID: 33697 RVA: 0x0035B744 File Offset: 0x00359944
		private void Update()
		{
			if (WardrobeGameplayContext.IsPresentationOnly(this))
			{
				return;
			}
			if (this._byName == null)
			{
				this.BuildLookup();
			}
			if (this.inventory == null || this.playerAnimator == null || this._byName == null || this._byName.Count == 0)
			{
				return;
			}
			int frame = this._frame;
			this._frame = frame + 1;
			if (frame < this.startupDelayFrames)
			{
				return;
			}
			if (this._frame % this.scanEveryFrames != 0)
			{
				return;
			}
			this.Scan();
		}

		// Token: 0x060083A2 RID: 33698 RVA: 0x0035B7CC File Offset: 0x003599CC
		private void Scan()
		{
			if (WardrobeGameplayContext.IsPresentationOnly(this))
			{
				return;
			}
			if (this._byName == null)
			{
				this.BuildLookup();
			}
			foreach (KeyValuePair<string, WearableItemSO> keyValuePair in this._byName)
			{
				WearableItemSO value = keyValuePair.Value;
				bool flag = this.IsGarmentOnBody(keyValuePair.Key);
				bool flag2 = this.IsInInventory(value);
				if (flag && !flag2)
				{
					int num;
					if (value.category == InventoryCategory.Item)
					{
						if (this.FindFreeItemSlot() < 0 || !this.inventory.AddItem(value, value.rarity, -1, 1f, false, true))
						{
							if (this.debugLogs)
							{
								Debug.LogWarning("[ManualDressWatcher] Hotbar piena.");
								continue;
							}
							continue;
						}
						else
						{
							num = this.FindItemSlot(value);
							if (num < 0)
							{
								continue;
							}
							WearableUseController wearableUseController = this.wearController;
							if (wearableUseController != null)
							{
								wearableUseController.MarkAlreadyWornItem(value, num);
							}
						}
					}
					else
					{
						num = this.inventory.AddClothingReturnIndex(value, false);
						if (num < 0)
						{
							if (this.debugLogs)
							{
								Debug.LogWarning("[ManualDressWatcher] Cloth slots pieni.");
								continue;
							}
							continue;
						}
						else
						{
							WearableUseController wearableUseController2 = this.wearController;
							if (wearableUseController2 != null)
							{
								wearableUseController2.MarkAlreadyWorn(value, num);
							}
						}
					}
					this._ownAdded.Add(value);
					if (this.debugLogs)
					{
						Debug.Log(string.Format("[ManualDressWatcher] '{0}' attaccato a mano → aggiunto all'inventario (slot {1}).", value.itemName, num));
					}
				}
				else if (!flag && flag2 && this._ownAdded.Contains(value))
				{
					int num2 = ((value.category == InventoryCategory.Item) ? this.FindItemSlot(value) : this.FindClothSlot(value));
					if (num2 >= 0)
					{
						if (value.category == InventoryCategory.Item)
						{
							this.inventory.DropItem(num2);
						}
						else
						{
							this.inventory.RemoveCloth(num2);
						}
					}
					this._ownAdded.Remove(value);
					if (this.debugLogs)
					{
						Debug.Log("[ManualDressWatcher] '" + value.itemName + "' staccato → rimosso dall'inventario.");
					}
				}
				else if (!flag && !flag2)
				{
					this._ownAdded.Remove(value);
				}
			}
		}

		// Token: 0x060083A3 RID: 33699 RVA: 0x0035B9F8 File Offset: 0x00359BF8
		private bool IsGarmentOnBody(string physicalPrefabName)
		{
			foreach (Transform transform in this.playerAnimator.GetComponentsInChildren<Transform>(true))
			{
				if (transform.gameObject.activeInHierarchy)
				{
					string name = transform.name;
					if ((name == physicalPrefabName || name.StartsWith(physicalPrefabName)) && transform.GetComponentInChildren<SkinnedMeshRenderer>(true) != null)
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x060083A4 RID: 33700 RVA: 0x0035BA5C File Offset: 0x00359C5C
		private bool IsInInventory(WearableItemSO item)
		{
			if (!(item != null))
			{
				return false;
			}
			if (item.category != InventoryCategory.Item)
			{
				return this.FindClothSlot(item) >= 0;
			}
			return this.FindItemSlot(item) >= 0;
		}

		// Token: 0x060083A5 RID: 33701 RVA: 0x0035BA8C File Offset: 0x00359C8C
		private int FindItemSlot(WearableItemSO item)
		{
			int maxItemSlots = this.inventory.MaxItemSlots;
			for (int i = 0; i < maxItemSlots; i++)
			{
				if (this.inventory.GetItemAt(i) == item)
				{
					return i;
				}
			}
			return -1;
		}

		// Token: 0x060083A6 RID: 33702 RVA: 0x0035BAC8 File Offset: 0x00359CC8
		private int FindFreeItemSlot()
		{
			int maxItemSlots = this.inventory.MaxItemSlots;
			for (int i = 0; i < maxItemSlots; i++)
			{
				if (this.inventory.GetItemAt(i) == null)
				{
					return i;
				}
			}
			return -1;
		}

		// Token: 0x060083A7 RID: 33703 RVA: 0x0035BB04 File Offset: 0x00359D04
		private int FindClothSlot(WearableItemSO item)
		{
			int maxClothSlots = this.inventory.MaxClothSlots;
			for (int i = 0; i < maxClothSlots; i++)
			{
				if (this.inventory.GetClothAt(i) == item)
				{
					return i;
				}
			}
			return -1;
		}

		// Token: 0x040086B3 RID: 34483
		[SerializeField]
		private PlayerInventoryManager inventory;

		// Token: 0x040086B4 RID: 34484
		[SerializeField]
		private WearableUseController wearController;

		// Token: 0x040086B5 RID: 34485
		[Tooltip("Animator del player: si scandisce il suo subtree per i capi worn.")]
		[SerializeField]
		private Animator playerAnimator;

		// Token: 0x040086B6 RID: 34486
		[Tooltip("Cartella Resources dei WearableItemSO cloth (come il ClothInventorySyncer).")]
		[SerializeField]
		private string wardrobeResourcesPath = "";

		// Token: 0x040086B7 RID: 34487
		[Tooltip("Ogni quanti frame ri-scandire (un valore basso è ok, l'operazione è leggera).")]
		[SerializeField]
		private int scanEveryFrames = 15;

		// Token: 0x040086B8 RID: 34488
		[Tooltip("Attendi N frame all'avvio prima di iniziare (lascia girare syncer/boot).")]
		[SerializeField]
		private int startupDelayFrames = 90;

		// Token: 0x040086B9 RID: 34489
		[SerializeField]
		private bool debugLogs;

		// Token: 0x040086BA RID: 34490
		private Dictionary<string, WearableItemSO> _byName;

		// Token: 0x040086BB RID: 34491
		private readonly HashSet<WearableItemSO> _ownAdded = new HashSet<WearableItemSO>();

		// Token: 0x040086BC RID: 34492
		private int _frame;
	}
}
