using System;
using System.Collections;
using System.Collections.Generic;
using BoZo.CharacterCreator.MC.StyleSystem;
using Features.Inventory.Wearable;
using Features.Prison;
using Features.Rank;
using Features.RunModes;
using ProjectExhibitionist.CharacterCustomization.Core.Runtime;
using UnityEngine;

namespace Features.Inventory
{
	// Token: 0x020010E4 RID: 4324
	public class ClothInventorySyncer : MonoBehaviour
	{
		// Token: 0x170014C7 RID: 5319
		// (get) Token: 0x06008321 RID: 33569 RVA: 0x0035823D File Offset: 0x0035643D
		// (set) Token: 0x06008322 RID: 33570 RVA: 0x00358245 File Offset: 0x00356445
		public bool SyncCompleted { get; private set; }

		// Token: 0x06008323 RID: 33571 RVA: 0x00358250 File Offset: 0x00356450
		private static bool RankDictatesOutfit()
		{
			RankRuntimeController rankRuntimeController = RankRuntimeController.Current;
			return !RunModeService.UsesHomePresetOutfit && !RunModeService.UsesPresetOutfit && (RunModeService.ShouldStartClean || (RunModeService.IsAtHome && RankWardrobePolicy.IsRestricted) || (rankRuntimeController != null && RankProgressionService.CurrentRank != null && RankProgressionService.CurrentRank.startOutfitReplacesCustom));
		}

		// Token: 0x06008324 RID: 33572 RVA: 0x003582B0 File Offset: 0x003564B0
		private void OnEnable()
		{
			this._synced = false;
			this.SyncCompleted = false;
			if (WardrobeGameplayContext.IsPresentationOnly(this))
			{
				this.SyncCompleted = true;
				return;
			}
			MainCharacterAppearanceEvents.InUsePresetChanged += this.Resync;
			this._syncRoutine = base.StartCoroutine(this.SyncWhenReady());
		}

		// Token: 0x06008325 RID: 33573 RVA: 0x003582FE File Offset: 0x003564FE
		private void OnDisable()
		{
			MainCharacterAppearanceEvents.InUsePresetChanged -= this.Resync;
			if (this._syncRoutine != null)
			{
				base.StopCoroutine(this._syncRoutine);
			}
			this._syncRoutine = null;
		}

		// Token: 0x06008326 RID: 33574 RVA: 0x0035832C File Offset: 0x0035652C
		private IEnumerator SyncWhenReady()
		{
			int i = 0;
			int num;
			while (i < 240 && !this.ResolveRefs())
			{
				yield return null;
				num = i;
				i = num + 1;
			}
			if (this.inventory == null || this.wearController == null || this.styleService == null)
			{
				if (this.debugLogs)
				{
					Debug.LogWarning("[ClothSync] Could not resolve inventory/wearController/styleService; sync skipped.");
				}
				this.SyncCompleted = true;
				this._syncRoutine = null;
				yield break;
			}
			for (i = 0; i < Mathf.Max(0, this.settleFrames); i = num + 1)
			{
				yield return null;
				num = i;
			}
			if (ClothInventorySyncer.RankDictatesOutfit())
			{
				if (this.debugLogs)
				{
					Debug.Log("[ClothSync] Rank detta l'outfit: mirror del preset saltato.");
				}
				this.SyncCompleted = true;
				this._syncRoutine = null;
				yield break;
			}
			this.SyncEquippedClothes();
			this.SyncCompleted = true;
			this._syncRoutine = null;
			yield break;
		}

		// Token: 0x06008327 RID: 33575 RVA: 0x0035833C File Offset: 0x0035653C
		private bool ResolveRefs()
		{
			if (this.inventory == null)
			{
				PlayerInventoryManager playerInventoryManager;
				if ((playerInventoryManager = base.GetComponentInParent<PlayerInventoryManager>()) == null)
				{
					playerInventoryManager = base.GetComponentInChildren<PlayerInventoryManager>(true) ?? PlayerInventoryManager.Instance;
				}
				this.inventory = playerInventoryManager;
			}
			if (this.wearController != null && WardrobeGameplayContext.IsPresentationOnly(this.wearController))
			{
				this.wearController = null;
			}
			if (this.wearController == null)
			{
				this.wearController = base.GetComponentInChildren<WearableUseController>(true) ?? base.GetComponentInParent<WearableUseController>();
			}
			if (this.wearController == null && this.inventory != null)
			{
				this.wearController = this.inventory.GetComponentInChildren<WearableUseController>(true);
			}
			if (this.styleService != null && WardrobeGameplayContext.IsPresentationOnly(this.styleService))
			{
				this.styleService = null;
			}
			if (this.styleService == null)
			{
				this.styleService = base.GetComponentInParent<MCStyleCustomizationService>() ?? base.GetComponentInChildren<MCStyleCustomizationService>(true);
			}
			if (this.styleService == null && this.inventory != null)
			{
				this.styleService = this.inventory.GetComponentInChildren<MCStyleCustomizationService>(true);
			}
			if (this.styleService == null && this.wearController != null)
			{
				this.styleService = this.wearController.StyleService;
			}
			return this.inventory != null && this.wearController != null && this.styleService != null;
		}

		// Token: 0x06008328 RID: 33576 RVA: 0x003584B8 File Offset: 0x003566B8
		public void Resync()
		{
			if (!base.isActiveAndEnabled)
			{
				return;
			}
			if (WardrobeGameplayContext.IsPresentationOnly(this))
			{
				return;
			}
			if (this._syncRoutine != null)
			{
				base.StopCoroutine(this._syncRoutine);
			}
			this._syncRoutine = base.StartCoroutine(this.ResyncWhenReady());
		}

		// Token: 0x06008329 RID: 33577 RVA: 0x003584F2 File Offset: 0x003566F2
		private IEnumerator ResyncWhenReady()
		{
			this.SyncCompleted = false;
			int i = 0;
			int num;
			while (i < 240 && !this.ResolveRefs())
			{
				yield return null;
				num = i;
				i = num + 1;
			}
			if (this.inventory == null || this.wearController == null || this.styleService == null)
			{
				Debug.LogWarning("[ClothSync] Resync fallito: player/inventory/style non disponibili.");
				this.SyncCompleted = true;
				this._syncRoutine = null;
				yield break;
			}
			yield return null;
			for (i = 0; i < Mathf.Max(0, this.settleFrames); i = num + 1)
			{
				yield return null;
				num = i;
			}
			if (WardrobeGameplayContext.IsPresentationOnly(this))
			{
				this.SyncCompleted = true;
				this._syncRoutine = null;
				yield break;
			}
			if (ClothInventorySyncer.RankDictatesOutfit())
			{
				this.ClearMirroredClothes();
				this._synced = false;
				this.SyncCompleted = true;
				this._syncRoutine = null;
				yield break;
			}
			this.ClearMirroredClothes();
			this._synced = false;
			this.SyncEquippedClothes();
			this.SyncCompleted = true;
			this._syncRoutine = null;
			Debug.Log(string.Format("[ClothSync] Resync preset completato: {0} item funzionali specchiati.", this._mirroredItems.Count));
			yield break;
		}

		// Token: 0x0600832A RID: 33578 RVA: 0x00358504 File Offset: 0x00356704
		private void ClearMirroredClothes()
		{
			for (int i = 0; i < this._mirroredItems.Count; i++)
			{
				WearableItemSO wearableItemSO = this._mirroredItems[i];
				if (!(wearableItemSO == null))
				{
					if (wearableItemSO.category == InventoryCategory.Item)
					{
						int j = 0;
						while (j < this.inventory.MaxItemSlots)
						{
							if (this.inventory.GetItemAt(j) == wearableItemSO)
							{
								if (this.wearController != null)
								{
									this.wearController.RemoveMirroredItem(wearableItemSO, j);
									break;
								}
								this.inventory.DropItem(j);
								break;
							}
							else
							{
								j++;
							}
						}
					}
					else
					{
						int k = 0;
						while (k < this.inventory.MaxClothSlots)
						{
							if (this.inventory.GetClothAt(k) == wearableItemSO)
							{
								if (this.wearController != null)
								{
									this.wearController.RemoveMirroredCloth(wearableItemSO, k);
									break;
								}
								this.inventory.RemoveCloth(k);
								break;
							}
							else
							{
								k++;
							}
						}
					}
				}
			}
			this._mirroredItems.Clear();
		}

		// Token: 0x0600832B RID: 33579 RVA: 0x00358608 File Offset: 0x00356808
		private void SyncEquippedClothes()
		{
			if (WardrobeGameplayContext.IsPresentationOnly(this))
			{
				return;
			}
			if (this._synced)
			{
				return;
			}
			this._synced = true;
			Dictionary<MCStylePresetSO, WearableItemSO> dictionary = this.BuildWardrobeLookup();
			PrisonNakedOverride componentInParent = base.GetComponentInParent<PrisonNakedOverride>();
			bool flag = componentInParent != null && componentInParent.isActiveAndEnabled;
			int num = 0;
			IReadOnlyList<MCStylePartDefinitionSO> partDefinitions = this.styleService.PartDefinitions;
			for (int i = 0; i < partDefinitions.Count; i++)
			{
				MCStylePartDefinitionSO mcstylePartDefinitionSO = partDefinitions[i];
				if (!(mcstylePartDefinitionSO == null) && !string.IsNullOrEmpty(mcstylePartDefinitionSO.partId) && !mcstylePartDefinitionSO.isBodyAppearance && (!flag || !RankWardrobePolicy.IsWardrobePart(mcstylePartDefinitionSO.partId)) && this.styleService.IsSlotEquipped(mcstylePartDefinitionSO.partId))
				{
					MCStylePresetSO activePreset = this.styleService.GetActivePreset(mcstylePartDefinitionSO.partId);
					if (activePreset == null)
					{
						if (this.debugLogs)
						{
							Debug.LogWarning("[ClothSync] Slot '" + mcstylePartDefinitionSO.partId + "' equipped but no active preset.");
						}
					}
					else
					{
						int slotSurfaceIndex = this.styleService.GetSlotSurfaceIndex(mcstylePartDefinitionSO.partId);
						MCSurfaceVariantSO mcsurfaceVariantSO;
						activePreset.TryGetSurfaceVariant(slotSurfaceIndex, out mcsurfaceVariantSO);
						WearableItemSO wearableItemSO;
						if (!dictionary.TryGetValue(activePreset, out wearableItemSO) || wearableItemSO == null)
						{
							if (this.debugLogs)
							{
								Debug.LogWarning(string.Concat(new string[] { "[ClothSync] No wardrobe item for preset '", activePreset.name, "' in slot '", mcstylePartDefinitionSO.partId, "'." }));
							}
						}
						else if (!wearableItemSO.ResolveShowInGameplayInventory(mcsurfaceVariantSO))
						{
							if (this.debugLogs)
							{
								Debug.Log(string.Concat(new string[] { "[ClothSync] Skipping aesthetic-only '", wearableItemSO.itemName, "' (", mcstylePartDefinitionSO.partId, ")." }));
							}
						}
						else if (wearableItemSO.category == InventoryCategory.Item)
						{
							int num2 = this.FindItemSlot(wearableItemSO);
							bool flag2 = num2 < 0;
							if (flag2)
							{
								if (this.FindFreeItemSlot() < 0 || !this.inventory.AddItem(wearableItemSO, wearableItemSO.rarity, slotSurfaceIndex, 1f, false, true))
								{
									if (this.debugLogs)
									{
										Debug.LogWarning("[ClothSync] Hotbar full; cannot mirror worn item '" + wearableItemSO.itemName + "'.");
										goto IL_02D2;
									}
									goto IL_02D2;
								}
								else
								{
									num2 = this.FindItemSlot(wearableItemSO);
								}
							}
							if (num2 >= 0)
							{
								WearableUseController wearableUseController = this.wearController;
								if (wearableUseController != null)
								{
									wearableUseController.MarkAlreadyWornItem(wearableItemSO, num2);
								}
								if (flag2)
								{
									this._mirroredItems.Add(wearableItemSO);
								}
								num++;
							}
						}
						else
						{
							int num3 = this.inventory.AddMirroredClothingReturnIndex(wearableItemSO, slotSurfaceIndex);
							if (num3 < 0)
							{
								if (this.debugLogs)
								{
									Debug.LogWarning("[ClothSync] Cloth slots full.");
									break;
								}
								break;
							}
							else
							{
								WearableUseController wearableUseController2 = this.wearController;
								if (wearableUseController2 != null)
								{
									wearableUseController2.MarkAlreadyWorn(wearableItemSO, num3);
								}
								this._mirroredItems.Add(wearableItemSO);
								num++;
							}
						}
					}
				}
				IL_02D2:;
			}
			if (this.debugLogs)
			{
				Debug.Log(string.Format("[ClothSync] Mirrored {0} equipped garment(s) into the cloth slots.", num));
			}
		}

		// Token: 0x0600832C RID: 33580 RVA: 0x00358918 File Offset: 0x00356B18
		private int FindItemSlot(InventoryItemSO item)
		{
			for (int i = 0; i < this.inventory.MaxItemSlots; i++)
			{
				if (this.inventory.GetItemAt(i) == item)
				{
					return i;
				}
			}
			return -1;
		}

		// Token: 0x0600832D RID: 33581 RVA: 0x00358954 File Offset: 0x00356B54
		private int FindFreeItemSlot()
		{
			for (int i = 0; i < this.inventory.MaxItemSlots; i++)
			{
				if (this.inventory.GetItemAt(i) == null)
				{
					return i;
				}
			}
			return -1;
		}

		// Token: 0x0600832E RID: 33582 RVA: 0x00358990 File Offset: 0x00356B90
		private Dictionary<MCStylePresetSO, WearableItemSO> BuildWardrobeLookup()
		{
			Dictionary<MCStylePresetSO, WearableItemSO> dictionary = new Dictionary<MCStylePresetSO, WearableItemSO>();
			foreach (WearableItemSO wearableItemSO in Resources.LoadAll<WearableItemSO>(this.wardrobeResourcesPath ?? string.Empty))
			{
				if (!(wearableItemSO == null) && !(wearableItemSO.mcStylePreset == null) && !dictionary.ContainsKey(wearableItemSO.mcStylePreset))
				{
					dictionary[wearableItemSO.mcStylePreset] = wearableItemSO;
				}
			}
			return dictionary;
		}

		// Token: 0x0400863A RID: 34362
		[Header("References")]
		[SerializeField]
		private PlayerInventoryManager inventory;

		// Token: 0x0400863B RID: 34363
		[SerializeField]
		private WearableUseController wearController;

		// Token: 0x0400863C RID: 34364
		[SerializeField]
		private MCStyleCustomizationService styleService;

		// Token: 0x0400863D RID: 34365
		[Header("Settings")]
		[Tooltip("Resources path (folder) the wardrobe cloth items are loaded from.")]
		[SerializeField]
		private string wardrobeResourcesPath = "";

		// Token: 0x0400863E RID: 34366
		[Tooltip("Frames to wait for the rig/customization to finish applying before syncing.")]
		[SerializeField]
		private int settleFrames = 2;

		// Token: 0x0400863F RID: 34367
		[SerializeField]
		private bool debugLogs;

		// Token: 0x04008640 RID: 34368
		private bool _synced;

		// Token: 0x04008641 RID: 34369
		private Coroutine _syncRoutine;

		// Token: 0x04008643 RID: 34371
		private readonly List<WearableItemSO> _mirroredItems = new List<WearableItemSO>();
	}
}
