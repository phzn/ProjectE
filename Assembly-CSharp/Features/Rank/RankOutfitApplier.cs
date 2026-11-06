using System;
using System.Collections;
using System.Collections.Generic;
using BoZo.CharacterCreator.MC.StyleSystem;
using Features.Inventory;
using Features.Inventory.Wearable;
using Features.RunModes;
using ProjectExhibitionist;
using ProjectExhibitionist.UI;
using UnityEngine;

namespace Features.Rank
{
	// Token: 0x02000DA9 RID: 3497
	public class RankOutfitApplier
	{
		// Token: 0x170010EC RID: 4332
		// (get) Token: 0x060066F8 RID: 26360 RVA: 0x00292E4C File Offset: 0x0029104C
		// (set) Token: 0x060066F9 RID: 26361 RVA: 0x00292E54 File Offset: 0x00291054
		public MCStyleCustomizationService.WardrobeSnapshot AppearanceRollback { private get; set; }

		// Token: 0x060066FA RID: 26362 RVA: 0x00292E60 File Offset: 0x00291060
		public void ClearAppliedRankItems()
		{
			if (this._appliedRankItems.Count == 0)
			{
				return;
			}
			PlayerInventoryManager instance = PlayerInventoryManager.Instance;
			if (instance == null)
			{
				this._appliedRankItems.Clear();
				this._appliedRankClothSlots.Clear();
				return;
			}
			int num = 0;
			foreach (WearableItemSO wearableItemSO in this._appliedRankItems)
			{
				if (!(wearableItemSO == null))
				{
					if (wearableItemSO.category == InventoryCategory.Item)
					{
						for (int i = 0; i < instance.MaxItemSlots; i++)
						{
							if (!(instance.GetItemAt(i) != wearableItemSO))
							{
								instance.DropItem(i);
								num++;
								break;
							}
						}
					}
					else
					{
						foreach (KeyValuePair<int, WearableItemSO> keyValuePair in this._appliedRankClothSlots)
						{
							if (!(keyValuePair.Value != wearableItemSO) && !(instance.GetClothAt(keyValuePair.Key) != wearableItemSO))
							{
								instance.RemoveCloth(keyValuePair.Key);
								num++;
								break;
							}
						}
					}
				}
			}
			this._appliedRankItems.Clear();
			this._appliedRankClothSlots.Clear();
			Debug.Log(string.Format("[RankOutfitApplier] Ripristino preset: rimossi {0} item creati dal rank.", num));
		}

		// Token: 0x060066FB RID: 26363 RVA: 0x00292FD0 File Offset: 0x002911D0
		public IEnumerator ApplyRoutine(RankDefinitionSO rank)
		{
			if (rank == null)
			{
				yield break;
			}
			if (RunModeService.UsesHomePresetOutfit || (RunModeService.UsesPresetOutfit && !RunModeService.HasPendingCollarHandoff))
			{
				Debug.Log("[RankOutfitApplier] Customization outfit: rank outfit override skipped.");
				if (RunModeService.IsGameplayBoot)
				{
					yield return FreeRunStartingItems.GrantWhenPlayerReady();
				}
				yield break;
			}
			yield return null;
			WearableUseController wearUse = null;
			PlayerInventoryManager inventory = null;
			int attempt;
			int num;
			for (attempt = 0; attempt < 300; attempt = num + 1)
			{
				inventory = PlayerInventoryManager.Instance;
				wearUse = CollarRunHandoff.ResolveWearController(inventory);
				if (inventory != null && wearUse != null && wearUse.gameObject.activeInHierarchy)
				{
					break;
				}
				yield return null;
				num = attempt;
			}
			if (inventory == null || wearUse == null || !wearUse.gameObject.activeInHierarchy)
			{
				Debug.LogWarning("[RankOutfitApplier] Player/inventario non risolti: outfit rank non applicato.");
				yield break;
			}
			RankRuntimeController homeHost = RankRuntimeController.Current;
			while (homeHost != null && homeHost.IsAtHome && !RunModeService.IsAtHome)
			{
				yield return null;
			}
			ClothInventorySyncer syncer = ((wearUse.StyleService != null) ? wearUse.StyleService.GetComponentInChildren<ClothInventorySyncer>(true) : inventory.GetComponentInChildren<ClothInventorySyncer>(true));
			attempt = 0;
			while (attempt < 300 && syncer != null && !syncer.SyncCompleted)
			{
				yield return null;
				num = attempt;
				attempt = num + 1;
			}
			RankRuntimeController rankRuntimeController = RankRuntimeController.Current;
			if (RunModeService.UsesHomePresetOutfit || (rankRuntimeController != null && rankRuntimeController.UsesCustomizationPresetInCurrentContext))
			{
				yield break;
			}
			if (RunModeService.UsesPresetOutfit)
			{
				RankOutfitApplier.ApplyPresetCollarHandoff(inventory, wearUse);
				FreeRunStartingItems.GrantOnce(inventory);
				yield break;
			}
			this.ApplyNow(rank, inventory, wearUse, RunModeService.ShouldStartClean);
			FreeRunStartingItems.GrantOnce(inventory);
			yield break;
		}

		// Token: 0x060066FC RID: 26364 RVA: 0x00292FE8 File Offset: 0x002911E8
		private void ApplyNow(RankDefinitionSO rank, PlayerInventoryManager inventory, WearableUseController wearUse, bool forceCleanStart)
		{
			bool flag = RunModeService.IsAtHome || (RankRuntimeController.Current != null && RankRuntimeController.Current.IsAtHome);
			forceCleanStart &= !flag;
			bool flag2 = !flag && RunModeService.HasPendingCollarHandoff;
			CollarRunHandoff collarRunHandoff = (flag2 ? RunModeService.PreparedCollarHandoff : CollarRunHandoff.Capture(inventory, wearUse));
			if (!CollarAvailabilityPolicy.IsRankUnlocked)
			{
				collarRunHandoff = null;
			}
			Dictionary<int, WearableItemSO> dictionary = new Dictionary<int, WearableItemSO>(this._appliedRankClothSlots);
			MCStyleCustomizationService styleService = wearUse.StyleService;
			List<WearableItemSO> list = new List<WearableItemSO>();
			Dictionary<WearableItemSO, int> dictionary2 = new Dictionary<WearableItemSO, int>();
			if (!forceCleanStart)
			{
				if (RankWardrobePolicy.IsRestricted && styleService != null)
				{
					foreach (MCStylePartDefinitionSO mcstylePartDefinitionSO in styleService.PartDefinitions)
					{
						if (!(mcstylePartDefinitionSO == null) && RankWardrobePolicy.IsWardrobePart(mcstylePartDefinitionSO.partId))
						{
							WearableItemSO mappedWearable = CustomizationWardrobeDiscoveryPolicy.GetMappedWearable(styleService.GetActivePreset(mcstylePartDefinitionSO.partId));
							if (RankWardrobePolicy.IsAllowed(mappedWearable))
							{
								list.Add(mappedWearable);
								dictionary2[mappedWearable] = styleService.GetSlotSurfaceIndex(mcstylePartDefinitionSO.partId);
							}
						}
					}
					list = RankWardrobePolicy.ResolveStartingOutfit(list, flag);
				}
				else if (rank.startOutfitItems != null)
				{
					foreach (WearableItemSO wearableItemSO in rank.startOutfitItems)
					{
						if (wearableItemSO != null && !list.Contains(wearableItemSO))
						{
							list.Add(wearableItemSO);
						}
					}
				}
			}
			HashSet<int> hashSet = new HashSet<int>();
			Dictionary<WearableItemSO, int> dictionary3 = new Dictionary<WearableItemSO, int>();
			for (int i = 0; i < inventory.MaxClothSlots; i++)
			{
				WearableItemSO wearableItemSO2 = inventory.GetClothAt(i) as WearableItemSO;
				if (!flag || !CollarAvailabilityPolicy.IsCollar(wearableItemSO2))
				{
					WearableItemSO wearableItemSO3;
					if (!flag || wearUse.IsClothSlotWorn(i) || (dictionary.TryGetValue(i, out wearableItemSO3) && wearableItemSO3 == wearableItemSO2))
					{
						hashSet.Add(i);
					}
					if (flag && wearableItemSO2 != null && list.Contains(wearableItemSO2) && !dictionary3.ContainsKey(wearableItemSO2))
					{
						dictionary3.Add(wearableItemSO2, i);
					}
				}
			}
			if (flag)
			{
				int num = 0;
				for (int j = 0; j < inventory.MaxClothSlots; j++)
				{
					if (!dictionary3.ContainsValue(j) && (inventory.GetClothAt(j) == null || hashSet.Contains(j)))
					{
						num++;
					}
				}
				if (list.Count - dictionary3.Count > num)
				{
					if (styleService != null && this.AppearanceRollback != null)
					{
						styleService.RestoreWardrobeState(this.AppearanceRollback);
					}
					this.AppearanceRollback = null;
					Debug.LogWarning("[RankOutfitApplier] Not enough clothing slots; previous outfit preserved.");
					ToastService.Show("Make room in your wardrobe before changing outfits.", "Wardrobe", null, -1f, "rank_ui");
					return;
				}
			}
			this.AppearanceRollback = null;
			this._appliedRankItems.Clear();
			this._appliedRankClothSlots.Clear();
			ClothingToggleController clothingToggleController = ((styleService != null) ? styleService.GetComponentInParent<ClothingToggleController>() : null);
			if (clothingToggleController == null && inventory != null)
			{
				clothingToggleController = inventory.GetComponentInChildren<ClothingToggleController>(true);
			}
			if (clothingToggleController != null)
			{
				string[] array = RankOutfitApplier.ResolveAestheticPrefixes();
				foreach (string text in new List<string>(clothingToggleController.ManagedSlots))
				{
					if (!RankOutfitApplier.IsAestheticSlot(text, array))
					{
						clothingToggleController.SetSlotVisible(text, false);
					}
				}
			}
			if (styleService != null)
			{
				styleService.SetFunctionalWardrobeHidden(true);
			}
			else if (clothingToggleController == null)
			{
				Debug.LogWarning("[RankOutfitApplier] ClothingToggleController e MCStyleCustomizationService non trovati: outfit base non spogliato.");
			}
			int maxClothSlots = inventory.MaxClothSlots;
			for (int k = 0; k < maxClothSlots; k++)
			{
				WearableItemSO wearableItemSO4 = inventory.GetClothAt(k) as WearableItemSO;
				if (!flag || (hashSet.Contains(k) && !dictionary3.ContainsValue(k)))
				{
					if (wearableItemSO4 != null && wearUse.IsClothSlotWorn(k))
					{
						wearUse.Unwear(wearableItemSO4);
					}
					inventory.RemoveCloth(k);
				}
			}
			int maxItemSlots = inventory.MaxItemSlots;
			if (!flag)
			{
				for (int l = 0; l < maxItemSlots; l++)
				{
					inventory.DropItem(l);
				}
			}
			if (!forceCleanStart && RankWardrobePolicy.IsRestricted && styleService != null)
			{
				using (IEnumerator<MCStylePartDefinitionSO> enumerator = styleService.PartDefinitions.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						MCStylePartDefinitionSO definition = enumerator.Current;
						if (!(definition == null) && RankWardrobePolicy.IsWardrobePart(definition.partId))
						{
							MCStylePresetSO preset = styleService.GetActivePreset(definition.partId);
							if (!(preset == null) && !list.Exists((WearableItemSO item) => item.mcStylePartId == definition.partId && item.mcStylePreset == preset))
							{
								styleService.SetSlotEquipped(definition.partId, false);
							}
						}
					}
				}
			}
			bool flag3 = RunModeService.IsGameplayBoot && RunModeService.IsClassicTutorialPhase;
			foreach (WearableItemSO wearableItemSO5 in list)
			{
				if (!(wearableItemSO5 == null))
				{
					int num3;
					int num2 = inventory.AddClothingReturnIndex(wearableItemSO5, false, wearableItemSO5.rarity, dictionary2.TryGetValue(wearableItemSO5, out num3) ? num3 : (-1), 1f, flag3);
					if (num2 >= 0)
					{
						int num4;
						if (dictionary3.ContainsKey(wearableItemSO5) && dictionary2.TryGetValue(wearableItemSO5, out num4))
						{
							inventory.SetClothInstanceLook(num2, inventory.GetClothRarityAt(num2), num4);
						}
						if (wearUse.IsWorn(wearableItemSO5))
						{
							wearUse.Unwear(wearableItemSO5);
						}
						wearUse.WearClothFromSlot(wearableItemSO5, num2);
						WearableItemSO wearableItemSO6;
						if (!dictionary3.ContainsKey(wearableItemSO5) || (dictionary.TryGetValue(num2, out wearableItemSO6) && wearableItemSO6 == wearableItemSO5))
						{
							this._appliedRankItems.Add(wearableItemSO5);
							this._appliedRankClothSlots[num2] = wearableItemSO5;
						}
					}
					else
					{
						Debug.LogWarning("[RankOutfitApplier] Inventario pieno: impossibile aggiungere '" + wearableItemSO5.itemName + "'.");
					}
				}
			}
			if (!flag)
			{
				CollarRunHandoff.ClearLoadedCollar(inventory, wearUse);
			}
			bool flag4 = collarRunHandoff == null || collarRunHandoff.Restore(inventory, wearUse);
			if (flag2 && RunModeService.IsGameplayBoot && flag4)
			{
				RunModeService.CompleteCollarHandoff();
			}
			if (!flag4)
			{
				Debug.LogWarning("[RankOutfitApplier] Carried collar could not be restored; handoff retained for retry.");
			}
			Debug.Log(forceCleanStart ? string.Format("[RankOutfitApplier] Clean start applicato (accessori consentiti preservati, collare trasferito: {0}).", collarRunHandoff != null && flag4) : string.Format("[RankOutfitApplier] Outfit rank '{0}' applicato ({1} capi, inventario svuotato).", rank.displayName, list.Count));
		}

		// Token: 0x060066FD RID: 26365 RVA: 0x002936D0 File Offset: 0x002918D0
		private static void ApplyPresetCollarHandoff(PlayerInventoryManager inventory, WearableUseController wearUse)
		{
			if (!RunModeService.HasPendingCollarHandoff)
			{
				return;
			}
			CollarRunHandoff collarRunHandoff = RunModeService.PreparedCollarHandoff;
			if (!CollarAvailabilityPolicy.IsRankUnlocked)
			{
				collarRunHandoff = null;
			}
			CollarRunHandoff.ClearLoadedCollar(inventory, wearUse);
			bool flag = collarRunHandoff == null || collarRunHandoff.Restore(inventory, wearUse);
			if (flag && RunModeService.IsGameplayBoot)
			{
				RunModeService.CompleteCollarHandoff();
			}
			if (!flag)
			{
				Debug.LogWarning("[RankOutfitApplier] Carried collar could not be restored; handoff retained for retry.");
			}
		}

		// Token: 0x060066FE RID: 26366 RVA: 0x00293724 File Offset: 0x00291924
		private static string[] ResolveAestheticPrefixes()
		{
			RankProgressionConfigSO config = RankProgressionService.Config;
			if (!(config != null) || config.aestheticSlotPrefixes == null || config.aestheticSlotPrefixes.Length == 0)
			{
				return new string[] { "Accessory." };
			}
			return config.aestheticSlotPrefixes;
		}

		// Token: 0x060066FF RID: 26367 RVA: 0x00293768 File Offset: 0x00291968
		private static bool IsAestheticSlot(string partId, string[] aestheticPrefixes)
		{
			if (string.IsNullOrEmpty(partId) || aestheticPrefixes == null)
			{
				return false;
			}
			foreach (string text in aestheticPrefixes)
			{
				if (!string.IsNullOrEmpty(text) && partId.StartsWith(text, StringComparison.Ordinal))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0400699A RID: 27034
		private const int MaxResolveAttempts = 300;

		// Token: 0x0400699B RID: 27035
		private readonly HashSet<WearableItemSO> _appliedRankItems = new HashSet<WearableItemSO>();

		// Token: 0x0400699C RID: 27036
		private readonly Dictionary<int, WearableItemSO> _appliedRankClothSlots = new Dictionary<int, WearableItemSO>();
	}
}
