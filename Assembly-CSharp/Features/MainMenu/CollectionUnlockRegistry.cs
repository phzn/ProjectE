using System;
using System.Collections.Generic;
using BoZo.CharacterCreator.MC.StyleSystem;
using Features.Detection;
using Features.Inventory;
using Features.Inventory.Wearable;
using Features.PlayerEmotes;
using Features.Rank;
using ProjectExhibitionist.PoiTravel;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Features.MainMenu
{
	// Token: 0x02001036 RID: 4150
	public static class CollectionUnlockRegistry
	{
		// Token: 0x06007D9B RID: 32155 RVA: 0x00337699 File Offset: 0x00335899
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
		private static void ResetInteriorVisits()
		{
			CollectionUnlockRegistry.InteriorVisits.Clear();
			SceneManager.sceneUnloaded -= CollectionUnlockRegistry.ForgetInteriorVisits;
		}

		// Token: 0x06007D9C RID: 32156 RVA: 0x003376B6 File Offset: 0x003358B6
		private static void ForgetInteriorVisits(Scene scene)
		{
			CollectionUnlockRegistry.InteriorVisits.Remove(scene.handle);
		}

		// Token: 0x140000E3 RID: 227
		// (add) Token: 0x06007D9D RID: 32157 RVA: 0x003376CC File Offset: 0x003358CC
		// (remove) Token: 0x06007D9E RID: 32158 RVA: 0x00337700 File Offset: 0x00335900
		public static event Action Changed;

		// Token: 0x140000E4 RID: 228
		// (add) Token: 0x06007D9F RID: 32159 RVA: 0x00337734 File Offset: 0x00335934
		// (remove) Token: 0x06007DA0 RID: 32160 RVA: 0x00337768 File Offset: 0x00335968
		public static event Action<InventoryItemSO, ItemRarity, int> GameplayItemDiscovered;

		// Token: 0x06007DA1 RID: 32161 RVA: 0x0033779C File Offset: 0x0033599C
		public static void ResetWardrobeDiscoveriesToDefaults()
		{
			foreach (WearableItemSO wearableItemSO in Resources.LoadAll<WearableItemSO>("Wardrobe"))
			{
				if (!(wearableItemSO == null))
				{
					string text = CollectionUnlockRegistry.StableId(wearableItemSO.itemId, wearableItemSO.name);
					string text2 = "Collection.Item." + text;
					PlayerPrefs.DeleteKey(text2);
					PlayerPrefs.DeleteKey(text2 + ".BestRarity");
					CollectionUnlockRegistry.DeleteVariantProgress(text2, text, -1);
					HashSet<int> hashSet = new HashSet<int>();
					if (wearableItemSO.mcStylePreset != null)
					{
						for (int j = 0; j < wearableItemSO.mcStylePreset.SurfaceCount; j++)
						{
							hashSet.Add(j);
						}
					}
					if (wearableItemSO.rarityVariants != null)
					{
						for (int k = 0; k < wearableItemSO.rarityVariants.Count; k++)
						{
							RarityVariantEntry rarityVariantEntry = wearableItemSO.rarityVariants[k];
							if (rarityVariantEntry != null && rarityVariantEntry.surfaceIndex >= 0)
							{
								hashSet.Add(rarityVariantEntry.surfaceIndex);
							}
						}
					}
					foreach (int num in hashSet)
					{
						CollectionUnlockRegistry.DeleteVariantProgress(text2, text, num);
					}
				}
			}
			PlayerPrefs.Save();
			Action changed = CollectionUnlockRegistry.Changed;
			if (changed == null)
			{
				return;
			}
			changed();
		}

		// Token: 0x06007DA2 RID: 32162 RVA: 0x003378FC File Offset: 0x00335AFC
		public static void UnlockItem(InventoryItemSO item)
		{
			if (item == null)
			{
				return;
			}
			CollectionUnlockRegistry.Unlock("Collection.Item." + CollectionUnlockRegistry.StableId(item.itemId, item.name));
		}

		// Token: 0x06007DA3 RID: 32163 RVA: 0x00337928 File Offset: 0x00335B28
		public static void RegisterItemFound(InventoryItemSO item, ItemRarity instanceRarity, int variantIndex)
		{
			if (item == null || RankProgressionService.SuppressProgressionRewards)
			{
				return;
			}
			string text = CollectionUnlockRegistry.ItemKey(item);
			string text2 = CollectionUnlockRegistry.VariantKey(text, -1) + ".Count";
			if (CollectionUnlockRegistry.IsUnlocked(text) && !PlayerPrefs.HasKey(text2) && !CollectionUnlockRegistry.HasRecordedSurfaceFinds(item, text))
			{
				PlayerPrefs.SetInt(text2, 1);
			}
			string text3 = CollectionUnlockRegistry.VariantKey(text, variantIndex) + ".Count";
			int @int = PlayerPrefs.GetInt(text3, 0);
			if (@int < 2147483647)
			{
				PlayerPrefs.SetInt(text3, @int + 1);
			}
			int num = Mathf.Clamp((int)instanceRarity, 0, 2);
			string text4 = text + ".BestRarity";
			if (!PlayerPrefs.HasKey(text4) || num > PlayerPrefs.GetInt(text4, 0))
			{
				PlayerPrefs.SetInt(text4, num);
			}
			PlayerPrefs.SetInt(text, 1);
			PlayerPrefs.Save();
			Action changed = CollectionUnlockRegistry.Changed;
			if (changed == null)
			{
				return;
			}
			changed();
		}

		// Token: 0x06007DA4 RID: 32164 RVA: 0x003379FC File Offset: 0x00335BFC
		public static void RegisterGameplayItemFound(InventoryItemSO item, ItemRarity instanceRarity, int variantIndex, Component source = null)
		{
			if (item == null || WardrobeGameplayContext.IsPresentationOnly(source))
			{
				return;
			}
			int num = CollectionUnlockRegistry.ResolveCollectionVariantIndex(item, variantIndex);
			bool flag = !CollectionUnlockRegistry.IsVariantUnlocked(item, num);
			CollectionUnlockRegistry.RegisterItemFound(item, instanceRarity, variantIndex);
			if (flag && !RankProgressionService.SuppressProgressionRewards && CollectionUnlockRegistry.IsVariantUnlocked(item, num))
			{
				Action<InventoryItemSO, ItemRarity, int> gameplayItemDiscovered = CollectionUnlockRegistry.GameplayItemDiscovered;
				if (gameplayItemDiscovered == null)
				{
					return;
				}
				gameplayItemDiscovered(item, instanceRarity, variantIndex);
			}
		}

		// Token: 0x06007DA5 RID: 32165 RVA: 0x00337A5C File Offset: 0x00335C5C
		private static int ResolveCollectionVariantIndex(InventoryItemSO item, int variantIndex)
		{
			if (variantIndex < 0)
			{
				WearableItemSO wearableItemSO = item as WearableItemSO;
				if (wearableItemSO != null && !(wearableItemSO.mcStylePreset == null) && item.rarityVariants != null)
				{
					int num = wearableItemSO.mcStylePreset.ResolveDefaultSurfaceIndex();
					foreach (RarityVariantEntry rarityVariantEntry in item.rarityVariants)
					{
						if (rarityVariantEntry != null && rarityVariantEntry.surfaceIndex == num)
						{
							return num;
						}
					}
					return variantIndex;
				}
			}
			return variantIndex;
		}

		// Token: 0x06007DA6 RID: 32166 RVA: 0x00337AF0 File Offset: 0x00335CF0
		public static bool HasNonStarterWardrobeDiscovery()
		{
			WardrobeMetadataIndex wardrobeMetadataIndex = WardrobeMetadataIndex.Load();
			if (wardrobeMetadataIndex == null)
			{
				return false;
			}
			for (int i = 0; i < wardrobeMetadataIndex.Entries.Count; i++)
			{
				WardrobeMetadataIndex.Entry entry = wardrobeMetadataIndex.Entries[i];
				if (entry != null)
				{
					bool flag = CollectionUnlockRegistry.IsDefaultWardrobeItem(entry.itemId);
					if (!flag && CollectionUnlockRegistry.IsUnlocked(CollectionUnlockRegistry.ItemKeyMetadata(entry)))
					{
						return true;
					}
					if (flag)
					{
						int num = (entry.hasPreset ? entry.surfaceCount : 0);
						for (int j = 0; j < num; j++)
						{
							if (CollectionUnlockRegistry.GetFoundCountMetadata(entry, j) > 0)
							{
								return true;
							}
						}
					}
				}
			}
			return false;
		}

		// Token: 0x06007DA7 RID: 32167 RVA: 0x00337B88 File Offset: 0x00335D88
		public static int GetFoundCount(InventoryItemSO item, int variantIndex)
		{
			if (item == null)
			{
				return 0;
			}
			string text = CollectionUnlockRegistry.ItemKey(item);
			int num = Mathf.Max(0, PlayerPrefs.GetInt(CollectionUnlockRegistry.VariantKey(text, variantIndex) + ".Count", 0));
			bool flag;
			if (variantIndex >= 0)
			{
				WearableItemSO wearableItemSO = item as WearableItemSO;
				if (wearableItemSO != null && wearableItemSO.mcStylePreset != null)
				{
					flag = variantIndex == wearableItemSO.mcStylePreset.ResolveDefaultSurfaceIndex();
					goto IL_005D;
				}
			}
			flag = false;
			IL_005D:
			bool flag2 = flag;
			if (flag2)
			{
				int num2 = Mathf.Max(0, PlayerPrefs.GetInt(CollectionUnlockRegistry.VariantKey(text, -1) + ".Count", 0));
				num = (int)Math.Min(2147483647L, (long)num + (long)num2);
			}
			if (num > 0)
			{
				return num;
			}
			if ((variantIndex < 0 || flag2) && CollectionUnlockRegistry.IsUnlocked(text) && !CollectionUnlockRegistry.HasRecordedSurfaceFinds(item, text))
			{
				return 1;
			}
			return num;
		}

		// Token: 0x06007DA8 RID: 32168 RVA: 0x00337C4C File Offset: 0x00335E4C
		private static bool HasRecordedSurfaceFinds(InventoryItemSO item, string itemKey)
		{
			WearableItemSO wearableItemSO = item as WearableItemSO;
			if (wearableItemSO != null && wearableItemSO.mcStylePreset != null)
			{
				for (int i = 0; i < wearableItemSO.mcStylePreset.SurfaceCount; i++)
				{
					if (PlayerPrefs.GetInt(CollectionUnlockRegistry.VariantKey(itemKey, i) + ".Count", 0) > 0)
					{
						return true;
					}
				}
			}
			if (item.rarityVariants != null)
			{
				foreach (RarityVariantEntry rarityVariantEntry in item.rarityVariants)
				{
					if (rarityVariantEntry != null && PlayerPrefs.GetInt(CollectionUnlockRegistry.VariantKey(itemKey, rarityVariantEntry.surfaceIndex) + ".Count", 0) > 0)
					{
						return true;
					}
				}
				return false;
			}
			return false;
		}

		// Token: 0x06007DA9 RID: 32169 RVA: 0x00337D18 File Offset: 0x00335F18
		public static bool IsVariantUnlocked(InventoryItemSO item, int variantIndex)
		{
			return item != null && variantIndex >= -1 && (CollectionUnlockRegistry.IsAdminVariantAvailable(item, variantIndex) || CreatorContentUnlocks.IsVariantAvailable(item, variantIndex) || CollectionUnlockRegistry.IsVariantEarned(item, variantIndex));
		}

		// Token: 0x06007DAA RID: 32170 RVA: 0x00337D44 File Offset: 0x00335F44
		public static bool IsVariantEarned(InventoryItemSO item, int variantIndex)
		{
			return item != null && variantIndex >= -1 && (CollectionUnlockRegistry.GetFoundCount(item, variantIndex) > 0 || CollectionUnlockRegistry.IsRankDefaultVariant(item, variantIndex));
		}

		// Token: 0x06007DAB RID: 32171 RVA: 0x00337D68 File Offset: 0x00335F68
		private static bool IsAdminItemAvailable(InventoryItemSO item)
		{
			if (RankProgressionService.IsAdminRankActive && item != null && !item.isVaulted)
			{
				WearableItemSO wearableItemSO = item as WearableItemSO;
				return wearableItemSO == null || wearableItemSO.mcStylePreset == null || !wearableItemSO.mcStylePreset.isVaulted;
			}
			return false;
		}

		// Token: 0x06007DAC RID: 32172 RVA: 0x00337DB8 File Offset: 0x00335FB8
		private static bool IsAdminVariantAvailable(InventoryItemSO item, int variantIndex)
		{
			if (!CollectionUnlockRegistry.IsAdminItemAvailable(item) || variantIndex < -1)
			{
				return false;
			}
			if (variantIndex == -1)
			{
				return true;
			}
			WearableItemSO wearableItemSO = item as WearableItemSO;
			if (wearableItemSO != null && wearableItemSO.mcStylePreset != null)
			{
				MCSurfaceVariantSO mcsurfaceVariantSO;
				return wearableItemSO.mcStylePreset.TryGetSurfaceVariant(variantIndex, out mcsurfaceVariantSO) && mcsurfaceVariantSO != null;
			}
			if (item.rarityVariants != null)
			{
				foreach (RarityVariantEntry rarityVariantEntry in item.rarityVariants)
				{
					if (rarityVariantEntry != null && rarityVariantEntry.surfaceIndex == variantIndex)
					{
						return true;
					}
				}
				return false;
			}
			return false;
		}

		// Token: 0x06007DAD RID: 32173 RVA: 0x00337E68 File Offset: 0x00336068
		public static bool IsRankDefaultVariant(InventoryItemSO item, int variantIndex)
		{
			WearableItemSO wearableItemSO = item as WearableItemSO;
			if (wearableItemSO == null || !RankWardrobePolicy.GetUnlockedItems().Contains(wearableItemSO))
			{
				return false;
			}
			int num = ((wearableItemSO.mcStylePreset != null) ? wearableItemSO.mcStylePreset.ResolveDefaultSurfaceIndex() : (-1));
			return variantIndex == num;
		}

		// Token: 0x06007DAE RID: 32174 RVA: 0x00337EB0 File Offset: 0x003360B0
		public static bool IsVariantNew(InventoryItemSO item, int variantIndex)
		{
			return !RankProgressionService.IsAdminRankActive && !CreatorContentUnlocks.IsActive && CollectionUnlockRegistry.IsVariantUnlocked(item, variantIndex) && !CollectionUnlockRegistry.IsUnlocked("Collection.Seen." + CollectionUnlockRegistry.VariantKey(CollectionUnlockRegistry.StableId(item.itemId, item.name), variantIndex));
		}

		// Token: 0x06007DAF RID: 32175 RVA: 0x00337EFF File Offset: 0x003360FF
		public static void MarkVariantSeen(InventoryItemSO item, int variantIndex)
		{
			if (item == null)
			{
				return;
			}
			CollectionUnlockRegistry.MarkSeen("Collection.Seen." + CollectionUnlockRegistry.VariantKey(CollectionUnlockRegistry.StableId(item.itemId, item.name), variantIndex));
		}

		// Token: 0x06007DB0 RID: 32176 RVA: 0x00337F31 File Offset: 0x00336131
		public static void MarkVariantUnseen(InventoryItemSO item, int variantIndex)
		{
			if (item == null || RankProgressionService.SuppressProgressionRewards)
			{
				return;
			}
			PlayerPrefs.DeleteKey("Collection.Seen." + CollectionUnlockRegistry.VariantKey(CollectionUnlockRegistry.StableId(item.itemId, item.name), variantIndex));
			PlayerPrefs.Save();
		}

		// Token: 0x06007DB1 RID: 32177 RVA: 0x00337F70 File Offset: 0x00336170
		public static ItemRarity GetBestRarity(InventoryItemSO item)
		{
			if (item == null)
			{
				return ItemRarity.Common;
			}
			string text = CollectionUnlockRegistry.ItemKey(item) + ".BestRarity";
			int rarity = (int)item.rarity;
			return (ItemRarity)Mathf.Clamp(PlayerPrefs.GetInt(text, rarity), 0, 2);
		}

		// Token: 0x06007DB2 RID: 32178 RVA: 0x00337FAC File Offset: 0x003361AC
		public static bool IsItemUnlocked(InventoryItemSO item)
		{
			if (item == null)
			{
				return false;
			}
			if (CollectionUnlockRegistry.IsAdminItemAvailable(item) || CreatorContentUnlocks.IsItemAvailable(item))
			{
				return true;
			}
			string text = CollectionUnlockRegistry.ItemKey(item);
			WearableItemSO wearableItemSO = item as WearableItemSO;
			if (wearableItemSO != null && CollectionUnlockRegistry.IsRankDefaultVariant(item, (wearableItemSO.mcStylePreset != null) ? wearableItemSO.mcStylePreset.ResolveDefaultSurfaceIndex() : (-1)))
			{
				return true;
			}
			if (CollectionUnlockRegistry.IsUnlocked(text) || CollectionUnlockRegistry.GetFoundCount(item, -1) > 0)
			{
				return true;
			}
			if (item.rarityVariants == null)
			{
				return false;
			}
			for (int i = 0; i < item.rarityVariants.Count; i++)
			{
				RarityVariantEntry rarityVariantEntry = item.rarityVariants[i];
				if (rarityVariantEntry != null && CollectionUnlockRegistry.GetFoundCount(item, rarityVariantEntry.surfaceIndex) > 0)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06007DB3 RID: 32179 RVA: 0x00338063 File Offset: 0x00336263
		public static void UnlockPoi(PoiEmoteProfileSO poi)
		{
			if (poi == null)
			{
				return;
			}
			CollectionUnlockRegistry.Unlock("Collection.POI." + CollectionUnlockRegistry.StableId(poi.PoiId, poi.name));
		}

		// Token: 0x06007DB4 RID: 32180 RVA: 0x00338090 File Offset: 0x00336290
		public static void RegisterPoiVisit(PoiEmoteProfileSO poi)
		{
			CollectionUnlockRegistry.RegisterPoiVisit(poi, default(Scene));
		}

		// Token: 0x06007DB5 RID: 32181 RVA: 0x003380AC File Offset: 0x003362AC
		public static void RegisterPoiVisit(PoiEmoteProfileSO poi, Scene visitScene)
		{
			if (poi == null || RankProgressionService.SuppressProgressionRewards)
			{
				return;
			}
			string text = CollectionUnlockRegistry.PoiKey(poi);
			if (visitScene.IsValid() && visitScene.isLoaded && PoiTravelState.IsPoiSceneName(visitScene.name))
			{
				HashSet<string> hashSet;
				if (!CollectionUnlockRegistry.InteriorVisits.TryGetValue(visitScene.handle, out hashSet))
				{
					hashSet = new HashSet<string>(StringComparer.Ordinal);
					CollectionUnlockRegistry.InteriorVisits.Add(visitScene.handle, hashSet);
					SceneManager.sceneUnloaded -= CollectionUnlockRegistry.ForgetInteriorVisits;
					SceneManager.sceneUnloaded += CollectionUnlockRegistry.ForgetInteriorVisits;
				}
				if (!hashSet.Add(text))
				{
					return;
				}
			}
			string text2 = text + ".Count";
			int @int = PlayerPrefs.GetInt(text2, 0);
			if (@int < 2147483647)
			{
				PlayerPrefs.SetInt(text2, @int + 1);
			}
			PlayerPrefs.SetInt(text, 1);
			PlayerPrefs.Save();
		}

		// Token: 0x06007DB6 RID: 32182 RVA: 0x00338180 File Offset: 0x00336380
		public static int GetPoiVisitCount(PoiEmoteProfileSO poi)
		{
			if (poi == null)
			{
				return 0;
			}
			string text = CollectionUnlockRegistry.PoiKey(poi);
			int @int = PlayerPrefs.GetInt(text + ".Count", 0);
			if (@int <= 0 && CollectionUnlockRegistry.IsUnlocked(text))
			{
				return 1;
			}
			return Mathf.Max(0, @int);
		}

		// Token: 0x06007DB7 RID: 32183 RVA: 0x003381C6 File Offset: 0x003363C6
		public static bool IsPoiUnlocked(PoiEmoteProfileSO poi)
		{
			return poi != null && (RankProgressionService.IsAdminRankActive || CreatorContentUnlocks.IsActive || CollectionUnlockRegistry.IsPoiDiscovered(poi));
		}

		// Token: 0x06007DB8 RID: 32184 RVA: 0x003381E9 File Offset: 0x003363E9
		public static bool IsPoiDiscovered(PoiEmoteProfileSO poi)
		{
			return poi != null && CollectionUnlockRegistry.GetPoiVisitCount(poi) > 0;
		}

		// Token: 0x06007DB9 RID: 32185 RVA: 0x003381FF File Offset: 0x003363FF
		public static bool IsPoiNew(PoiEmoteProfileSO poi)
		{
			return !RankProgressionService.IsAdminRankActive && !CreatorContentUnlocks.IsActive && CollectionUnlockRegistry.IsPoiUnlocked(poi) && !CollectionUnlockRegistry.IsUnlocked("Collection.Seen.POI." + CollectionUnlockRegistry.StableId(poi.PoiId, poi.name));
		}

		// Token: 0x06007DBA RID: 32186 RVA: 0x0033823C File Offset: 0x0033643C
		public static void MarkPoiSeen(PoiEmoteProfileSO poi)
		{
			if (poi != null)
			{
				CollectionUnlockRegistry.MarkSeen("Collection.Seen.POI." + CollectionUnlockRegistry.StableId(poi.PoiId, poi.name));
			}
		}

		// Token: 0x06007DBB RID: 32187 RVA: 0x00338267 File Offset: 0x00336467
		public static void UnlockEmote(EmoteDefinitionSO emote)
		{
			if (emote == null)
			{
				return;
			}
			CollectionUnlockRegistry.Unlock("Collection.Emote." + CollectionUnlockRegistry.StableId(emote.EmoteId, emote.name));
		}

		// Token: 0x06007DBC RID: 32188 RVA: 0x00338294 File Offset: 0x00336494
		public static bool IsEmoteUnlocked(EmoteDefinitionSO emote)
		{
			return !(emote == null) && (RankProgressionService.IsAdminRankActive || CreatorContentUnlocks.IsActive || emote.UnlockedByDefault || (!string.IsNullOrWhiteSpace(emote.UnlockKey) && PlayerPrefs.GetInt(emote.UnlockKey, 0) != 0) || CollectionUnlockRegistry.IsUnlocked("Collection.Emote." + CollectionUnlockRegistry.StableId(emote.EmoteId, emote.name)));
		}

		// Token: 0x06007DBD RID: 32189 RVA: 0x00338301 File Offset: 0x00336501
		public static bool IsEmoteNew(EmoteDefinitionSO emote)
		{
			return !RankProgressionService.IsAdminRankActive && !CreatorContentUnlocks.IsActive && CollectionUnlockRegistry.IsEmoteUnlocked(emote) && !CollectionUnlockRegistry.IsUnlocked("Collection.Seen.Emote." + CollectionUnlockRegistry.StableId(emote.EmoteId, emote.name));
		}

		// Token: 0x06007DBE RID: 32190 RVA: 0x0033833E File Offset: 0x0033653E
		public static void MarkEmoteSeen(EmoteDefinitionSO emote)
		{
			if (emote != null)
			{
				CollectionUnlockRegistry.MarkSeen("Collection.Seen.Emote." + CollectionUnlockRegistry.StableId(emote.EmoteId, emote.name));
			}
		}

		// Token: 0x06007DBF RID: 32191 RVA: 0x00338369 File Offset: 0x00336569
		private static void Unlock(string key)
		{
			if (RankProgressionService.SuppressProgressionRewards || PlayerPrefs.GetInt(key, 0) != 0)
			{
				return;
			}
			PlayerPrefs.SetInt(key, 1);
			PlayerPrefs.Save();
		}

		// Token: 0x06007DC0 RID: 32192 RVA: 0x00338388 File Offset: 0x00336588
		private static bool IsUnlocked(string key)
		{
			return PlayerPrefs.GetInt(key, 0) != 0;
		}

		// Token: 0x06007DC1 RID: 32193 RVA: 0x00338394 File Offset: 0x00336594
		private static void MarkSeen(string key)
		{
			if (RankProgressionService.SuppressProgressionRewards || CollectionUnlockRegistry.IsUnlocked(key))
			{
				return;
			}
			PlayerPrefs.SetInt(key, 1);
			PlayerPrefs.Save();
		}

		// Token: 0x06007DC2 RID: 32194 RVA: 0x003383B2 File Offset: 0x003365B2
		private static string ItemKey(InventoryItemSO item)
		{
			return "Collection.Item." + CollectionUnlockRegistry.StableId(item.itemId, item.name);
		}

		// Token: 0x06007DC3 RID: 32195 RVA: 0x003383CF File Offset: 0x003365CF
		private static string PoiKey(PoiEmoteProfileSO poi)
		{
			return "Collection.POI." + CollectionUnlockRegistry.StableId(poi.PoiId, poi.name);
		}

		// Token: 0x06007DC4 RID: 32196 RVA: 0x003383EC File Offset: 0x003365EC
		private static string VariantKey(string itemKey, int variantIndex)
		{
			return itemKey + ((variantIndex < 0) ? ".VBase" : (".V" + variantIndex.ToString()));
		}

		// Token: 0x06007DC5 RID: 32197 RVA: 0x00338410 File Offset: 0x00336610
		private static void DeleteVariantProgress(string itemKey, string stableId, int variantIndex)
		{
			PlayerPrefs.DeleteKey(CollectionUnlockRegistry.VariantKey(itemKey, variantIndex) + ".Count");
			PlayerPrefs.DeleteKey("Collection.Seen." + CollectionUnlockRegistry.VariantKey(stableId, variantIndex));
		}

		// Token: 0x06007DC6 RID: 32198 RVA: 0x0033843E File Offset: 0x0033663E
		private static string StableId(string preferred, string fallback)
		{
			return (string.IsNullOrWhiteSpace(preferred) ? fallback : preferred).Trim().Replace(' ', '_');
		}

		// Token: 0x06007DC7 RID: 32199 RVA: 0x0033845C File Offset: 0x0033665C
		private static bool IsDefaultWardrobeItem(string itemId)
		{
			for (int i = 0; i < CollectionUnlockRegistry.LegacyAutoGrantedWardrobeItemIds.Length; i++)
			{
				if (string.Equals(CollectionUnlockRegistry.LegacyAutoGrantedWardrobeItemIds[i], itemId, StringComparison.OrdinalIgnoreCase))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06007DC8 RID: 32200 RVA: 0x00338490 File Offset: 0x00336690
		public static int GetFoundCountMetadata(WardrobeMetadataIndex.Entry item, int variantIndex)
		{
			if (item == null)
			{
				return 0;
			}
			string text = CollectionUnlockRegistry.ItemKeyMetadata(item);
			int num = Mathf.Max(0, PlayerPrefs.GetInt(CollectionUnlockRegistry.VariantKey(text, variantIndex) + ".Count", 0));
			bool flag = variantIndex >= 0 && item.isWearable && item.hasPreset && variantIndex == item.defaultSurfaceIndex;
			if (flag)
			{
				int num2 = Mathf.Max(0, PlayerPrefs.GetInt(CollectionUnlockRegistry.VariantKey(text, -1) + ".Count", 0));
				num = (int)Math.Min(2147483647L, (long)num + (long)num2);
			}
			if (num > 0)
			{
				return num;
			}
			if ((variantIndex >= 0 && !flag) || !CollectionUnlockRegistry.IsUnlocked(text) || CollectionUnlockRegistry.HasRecordedSurfaceFindsMetadata(item, text))
			{
				return num;
			}
			return 1;
		}

		// Token: 0x06007DC9 RID: 32201 RVA: 0x00338540 File Offset: 0x00336740
		private static bool HasRecordedSurfaceFindsMetadata(WardrobeMetadataIndex.Entry item, string key)
		{
			if (item.isWearable && item.hasPreset)
			{
				for (int i = 0; i < item.surfaceCount; i++)
				{
					if (PlayerPrefs.GetInt(CollectionUnlockRegistry.VariantKey(key, i) + ".Count", 0) > 0)
					{
						return true;
					}
				}
			}
			if (item.rarityVariants != null)
			{
				foreach (WardrobeMetadataIndex.RarityEntry rarityEntry in item.rarityVariants)
				{
					if (rarityEntry != null && rarityEntry.exists && PlayerPrefs.GetInt(CollectionUnlockRegistry.VariantKey(key, rarityEntry.surfaceIndex) + ".Count", 0) > 0)
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x06007DCA RID: 32202 RVA: 0x003385D8 File Offset: 0x003367D8
		public static bool IsVariantUnlockedMetadata(WardrobeMetadataIndex.Entry item, int variantIndex)
		{
			return item != null && variantIndex >= -1 && (CollectionUnlockRegistry.IsAccessVariantAvailableMetadata(item, variantIndex) || CollectionUnlockRegistry.IsVariantEarnedMetadata(item, variantIndex));
		}

		// Token: 0x06007DCB RID: 32203 RVA: 0x003385F5 File Offset: 0x003367F5
		public static bool IsVariantEarnedMetadata(WardrobeMetadataIndex.Entry item, int variantIndex)
		{
			return item != null && variantIndex >= -1 && (CollectionUnlockRegistry.GetFoundCountMetadata(item, variantIndex) > 0 || CollectionUnlockRegistry.IsRankDefaultVariantMetadata(item, variantIndex));
		}

		// Token: 0x06007DCC RID: 32204 RVA: 0x00338613 File Offset: 0x00336813
		private static bool IsAccessItemAvailableMetadata(WardrobeMetadataIndex.Entry item)
		{
			return (RankProgressionService.IsAdminRankActive || CreatorContentUnlocks.IsActive) && item != null && !item.isVaulted && (!item.isWearable || !item.hasPreset || !item.presetIsVaulted);
		}

		// Token: 0x06007DCD RID: 32205 RVA: 0x0033864C File Offset: 0x0033684C
		private static bool IsAccessVariantAvailableMetadata(WardrobeMetadataIndex.Entry item, int variantIndex)
		{
			if (!CollectionUnlockRegistry.IsAccessItemAvailableMetadata(item) || variantIndex < -1)
			{
				return false;
			}
			if (variantIndex == -1)
			{
				return true;
			}
			if (item.isWearable && item.hasPreset)
			{
				WardrobeMetadataIndex.SurfaceEntry surfaceEntry;
				return item.TryGetSurface(variantIndex, out surfaceEntry);
			}
			if (item.rarityVariants != null)
			{
				foreach (WardrobeMetadataIndex.RarityEntry rarityEntry in item.rarityVariants)
				{
					if (rarityEntry != null && rarityEntry.exists && rarityEntry.surfaceIndex == variantIndex)
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x06007DCE RID: 32206 RVA: 0x003386C0 File Offset: 0x003368C0
		public static bool IsRankDefaultVariantMetadata(WardrobeMetadataIndex.Entry item, int variantIndex)
		{
			if (item == null || !item.isWearable || variantIndex != (item.hasPreset ? item.defaultSurfaceIndex : (-1)) || item.rankIds == null || item.rankIds.Length == 0)
			{
				return false;
			}
			RankProgressionConfigSO config = RankProgressionService.Config;
			if (config == null || config.ranksInOrder == null)
			{
				return false;
			}
			int num = Math.Min(RankProgressionService.RankIndex, config.ranksInOrder.Count - 1);
			for (int i = 0; i <= num; i++)
			{
				RankDefinitionSO rankDefinitionSO = config.ranksInOrder[i];
				if (!(rankDefinitionSO == null))
				{
					foreach (string text in item.rankIds)
					{
						if (string.Equals(rankDefinitionSO.rankId, text, StringComparison.Ordinal))
						{
							return true;
						}
					}
				}
			}
			return false;
		}

		// Token: 0x06007DCF RID: 32207 RVA: 0x00338788 File Offset: 0x00336988
		public static bool IsItemUnlockedMetadata(WardrobeMetadataIndex.Entry item)
		{
			if (item == null)
			{
				return false;
			}
			if (CollectionUnlockRegistry.IsAccessItemAvailableMetadata(item) || CollectionUnlockRegistry.IsRankDefaultVariantMetadata(item, item.hasPreset ? item.defaultSurfaceIndex : (-1)) || CollectionUnlockRegistry.IsUnlocked(CollectionUnlockRegistry.ItemKeyMetadata(item)) || CollectionUnlockRegistry.GetFoundCountMetadata(item, -1) > 0)
			{
				return true;
			}
			if (item.rarityVariants != null)
			{
				foreach (WardrobeMetadataIndex.RarityEntry rarityEntry in item.rarityVariants)
				{
					if (rarityEntry != null && rarityEntry.exists && CollectionUnlockRegistry.GetFoundCountMetadata(item, rarityEntry.surfaceIndex) > 0)
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x06007DD0 RID: 32208 RVA: 0x00338814 File Offset: 0x00336A14
		public static bool IsVariantNewMetadata(WardrobeMetadataIndex.Entry item, int variantIndex)
		{
			return !RankProgressionService.IsAdminRankActive && !CreatorContentUnlocks.IsActive && CollectionUnlockRegistry.IsVariantUnlockedMetadata(item, variantIndex) && !CollectionUnlockRegistry.IsUnlocked("Collection.Seen." + CollectionUnlockRegistry.VariantKey(CollectionUnlockRegistry.StableId(item.itemId, item.assetName), variantIndex));
		}

		// Token: 0x06007DD1 RID: 32209 RVA: 0x00338863 File Offset: 0x00336A63
		public static void MarkVariantSeenMetadata(WardrobeMetadataIndex.Entry item, int variantIndex)
		{
			if (item != null)
			{
				CollectionUnlockRegistry.MarkSeen("Collection.Seen." + CollectionUnlockRegistry.VariantKey(CollectionUnlockRegistry.StableId(item.itemId, item.assetName), variantIndex));
			}
		}

		// Token: 0x06007DD2 RID: 32210 RVA: 0x0033888E File Offset: 0x00336A8E
		private static string ItemKeyMetadata(WardrobeMetadataIndex.Entry item)
		{
			return "Collection.Item." + CollectionUnlockRegistry.StableId(item.itemId, item.assetName);
		}

		// Token: 0x04008178 RID: 33144
		private const string ItemPrefix = "Collection.Item.";

		// Token: 0x04008179 RID: 33145
		private const string PoiPrefix = "Collection.POI.";

		// Token: 0x0400817A RID: 33146
		private const string EmotePrefix = "Collection.Emote.";

		// Token: 0x0400817B RID: 33147
		private const string SeenPrefix = "Collection.Seen.";

		// Token: 0x0400817C RID: 33148
		private const string CountSuffix = ".Count";

		// Token: 0x0400817D RID: 33149
		private const string BestRaritySuffix = ".BestRarity";

		// Token: 0x0400817E RID: 33150
		private static readonly Dictionary<int, HashSet<string>> InteriorVisits = new Dictionary<int, HashSet<string>>();

		// Token: 0x0400817F RID: 33151
		private static readonly string[] LegacyAutoGrantedWardrobeItemIds = new string[] { "cloth_top", "cloth_bottom", "cloth_shoes" };
	}
}
