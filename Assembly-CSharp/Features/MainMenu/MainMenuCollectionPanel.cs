using System;
using System.Collections;
using System.Collections.Generic;
using Features.CensorBars;
using Features.Inventory;
using Features.Inventory.Wearable;
using Features.Localization;
using Features.MatchRecap;
using Features.PlayerEmotes;
using Features.Rank;
using ProjectExhibitionist.Minimap;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Features.MainMenu
{
	// Token: 0x0200104A RID: 4170
	public sealed class MainMenuCollectionPanel : MonoBehaviour
	{
		// Token: 0x1700145D RID: 5213
		// (get) Token: 0x06007EA3 RID: 32419 RVA: 0x0033C7CC File Offset: 0x0033A9CC
		private CollectionMetadataCatalog.Entry[] ItemDefinitions
		{
			get
			{
				CollectionMetadataCatalog.Entry[] array;
				if ((array = this._resolvedItemDefinitions) == null)
				{
					CollectionMetadataCatalog collectionMetadataCatalog = CollectionItemCatalog.LoadMetadata();
					array = (this._resolvedItemDefinitions = ((collectionMetadataCatalog != null) ? collectionMetadataCatalog.Entries : null) ?? Array.Empty<CollectionMetadataCatalog.Entry>());
				}
				return array;
			}
		}

		// Token: 0x06007EA4 RID: 32420 RVA: 0x0033C806 File Offset: 0x0033AA06
		private void OnEnable()
		{
			PlayerStatsService.Changed += this.RefreshVisibleStats;
			RankProgressionService.Changed += this.Refresh;
			CollectionUnlockRegistry.Changed += this.Refresh;
			this.ShowRoot();
		}

		// Token: 0x06007EA5 RID: 32421 RVA: 0x0033C844 File Offset: 0x0033AA44
		private void OnDisable()
		{
			PlayerStatsService.Changed -= this.RefreshVisibleStats;
			RankProgressionService.Changed -= this.Refresh;
			CollectionUnlockRegistry.Changed -= this.Refresh;
			this.ClearCards();
			this.ResetPreview();
		}

		// Token: 0x06007EA6 RID: 32422 RVA: 0x0033C890 File Offset: 0x0033AA90
		public void Refresh()
		{
			this.RefreshRoot();
			if (this.IsTabPageActive())
			{
				this.RefreshTab(this._activeTab);
				return;
			}
			if (this.statsPage != null && this.statsPage.activeSelf)
			{
				this.RefreshStats();
			}
		}

		// Token: 0x06007EA7 RID: 32423 RVA: 0x0033C8CE File Offset: 0x0033AACE
		public void ShowRoot()
		{
			this.ClearCards();
			this.ResetPreview();
			this.ShowOnly(this.rootPage, "MAIN MENU");
			this.RefreshRoot();
		}

		// Token: 0x06007EA8 RID: 32424 RVA: 0x0033C8F4 File Offset: 0x0033AAF4
		public void ShowStats()
		{
			this.ClearCards();
			this.ResetPreview();
			this.ShowOnly(this.statsPage, "COLLECTION / STATS");
			this.RefreshStats();
			if (this._statsScroll != null)
			{
				this._statsScroll.StopMovement();
				this._statsScroll.verticalNormalizedPosition = 1f;
			}
		}

		// Token: 0x06007EA9 RID: 32425 RVA: 0x0033C94D File Offset: 0x0033AB4D
		public void ShowItems()
		{
			this.ShowTab(MainMenuCollectionPanel.CollectionTab.Items);
		}

		// Token: 0x06007EAA RID: 32426 RVA: 0x0033C956 File Offset: 0x0033AB56
		public void ShowClothes()
		{
			this.ShowTab(MainMenuCollectionPanel.CollectionTab.Clothes);
		}

		// Token: 0x06007EAB RID: 32427 RVA: 0x0033C95F File Offset: 0x0033AB5F
		public void ShowPoi()
		{
			this.ShowTab(MainMenuCollectionPanel.CollectionTab.Poi);
		}

		// Token: 0x06007EAC RID: 32428 RVA: 0x0033C968 File Offset: 0x0033AB68
		public bool TryHandleBack()
		{
			if (this.rootPage != null && this.rootPage.activeSelf)
			{
				return false;
			}
			this.ShowRoot();
			return true;
		}

		// Token: 0x06007EAD RID: 32429 RVA: 0x0033C98E File Offset: 0x0033AB8E
		public void Back()
		{
			this.TryHandleBack();
		}

		// Token: 0x06007EAE RID: 32430 RVA: 0x0033C997 File Offset: 0x0033AB97
		public void SetSeenTrackingEnabled(bool enabled)
		{
			this._seenTrackingEnabled = enabled;
		}

		// Token: 0x06007EAF RID: 32431 RVA: 0x0033C9A0 File Offset: 0x0033ABA0
		public void CountUnlocked(MainMenuCollectionPanel.CollectionTab tab, out int unlocked, out int total)
		{
			unlocked = 0;
			total = 0;
			switch (tab)
			{
			case MainMenuCollectionPanel.CollectionTab.Items:
				this.CountItems(false, ref unlocked, ref total);
				return;
			case MainMenuCollectionPanel.CollectionTab.Clothes:
				this.CountItems(true, ref unlocked, ref total);
				return;
			case MainMenuCollectionPanel.CollectionTab.Poi:
			{
				if (this.poiDefinitions == null)
				{
					return;
				}
				for (int i = 0; i < this.poiDefinitions.Length; i++)
				{
					PoiEmoteProfileSO poiEmoteProfileSO = this.poiDefinitions[i];
					if (!(poiEmoteProfileSO == null))
					{
						total++;
						if (CollectionUnlockRegistry.IsPoiUnlocked(poiEmoteProfileSO))
						{
							unlocked++;
						}
					}
				}
				return;
			}
			default:
				return;
			}
		}

		// Token: 0x06007EB0 RID: 32432 RVA: 0x0033CA1C File Offset: 0x0033AC1C
		public bool HasNewDiscoveries()
		{
			if (this.ItemDefinitions != null)
			{
				for (int i = 0; i < this.ItemDefinitions.Length; i++)
				{
					CollectionMetadataCatalog.Entry entry = this.ItemDefinitions[i];
					if (MainMenuCollectionPanel.IsCollectibleItem(entry))
					{
						WardrobeMetadataIndex.Entry item = entry.item;
						if (item.rarityVariants != null && item.rarityVariants.Length != 0)
						{
							for (int j = 0; j < item.rarityVariants.Length; j++)
							{
								WardrobeMetadataIndex.RarityEntry rarityEntry = item.rarityVariants[j];
								if (rarityEntry != null && rarityEntry.exists && CollectionUnlockRegistry.IsVariantNewMetadata(item, rarityEntry.surfaceIndex))
								{
									return true;
								}
							}
						}
						else if (CollectionUnlockRegistry.IsVariantNewMetadata(item, -1))
						{
							return true;
						}
					}
				}
			}
			if (this.poiDefinitions != null)
			{
				for (int k = 0; k < this.poiDefinitions.Length; k++)
				{
					if (CollectionUnlockRegistry.IsPoiNew(this.poiDefinitions[k]))
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x06007EB1 RID: 32433 RVA: 0x0033CAEA File Offset: 0x0033ACEA
		private void ShowTab(MainMenuCollectionPanel.CollectionTab tab)
		{
			this._activeTab = tab;
			this.ResetPreview();
			this.ShowOnly(this.PageFor(tab), "COLLECTION / " + MainMenuCollectionPanel.TabLabel(tab));
			this.RefreshTab(tab);
		}

		// Token: 0x06007EB2 RID: 32434 RVA: 0x0033CB20 File Offset: 0x0033AD20
		private void RefreshTab(MainMenuCollectionPanel.CollectionTab tab)
		{
			this.ClearCards();
			int num = 0;
			int num2 = 0;
			switch (tab)
			{
			case MainMenuCollectionPanel.CollectionTab.Items:
				this.PopulateItems(false, ref num, ref num2);
				break;
			case MainMenuCollectionPanel.CollectionTab.Clothes:
				this.PopulateItems(true, ref num, ref num2);
				break;
			case MainMenuCollectionPanel.CollectionTab.Poi:
				this.PopulatePoi(ref num, ref num2);
				break;
			}
			if (this.summaryLabels != null && tab < (MainMenuCollectionPanel.CollectionTab)this.summaryLabels.Length && this.summaryLabels[(int)tab] != null)
			{
				int capturedUnlocked = num;
				int capturedTotal = num2;
				if (tab == MainMenuCollectionPanel.CollectionTab.Clothes)
				{
					LocalizedGameText.BindComputed(this.summaryLabels[(int)tab], () => GameLocalization.Format("menu.collection.unlocked_colorways", "{0} / {1} COLORWAYS UNLOCKED", new object[] { capturedUnlocked, capturedTotal }));
				}
				else
				{
					LocalizedGameText.BindComputed(this.summaryLabels[(int)tab], () => GameLocalization.Format("menu.collection.unlocked_entries", "{0} / {1} ENTRIES UNLOCKED", new object[] { capturedUnlocked, capturedTotal }));
				}
			}
			if (this.emptyLabels != null && tab < (MainMenuCollectionPanel.CollectionTab)this.emptyLabels.Length && this.emptyLabels[(int)tab] != null)
			{
				this.emptyLabels[(int)tab].gameObject.SetActive(num2 == 0);
				LocalizedGameText.BindSource(this.emptyLabels[(int)tab], "settings", "No entries available in this category.", null);
			}
			this.UpdateGridLayout(tab);
			this.RefreshRoot();
		}

		// Token: 0x06007EB3 RID: 32435 RVA: 0x0033CC41 File Offset: 0x0033AE41
		private void RefreshRoot()
		{
			this.SetCompletion(MainMenuCollectionPanel.CollectionTab.Items, this.itemsCompletionLabel);
			this.SetCompletion(MainMenuCollectionPanel.CollectionTab.Clothes, this.clothesCompletionLabel);
			this.SetCompletion(MainMenuCollectionPanel.CollectionTab.Poi, this.poiCompletionLabel);
		}

		// Token: 0x06007EB4 RID: 32436 RVA: 0x0033CC6C File Offset: 0x0033AE6C
		private void SetCompletion(MainMenuCollectionPanel.CollectionTab tab, TMP_Text label)
		{
			int num;
			int num2;
			this.CountUnlocked(tab, out num, out num2);
			if (label != null)
			{
				label.text = string.Format("{0}%", MainMenuCollectionPanel.Percent(num, num2));
			}
		}

		// Token: 0x06007EB5 RID: 32437 RVA: 0x0033CCAC File Offset: 0x0033AEAC
		private void RefreshStats()
		{
			if (this.statsValues == null)
			{
				return;
			}
			this.EnsureActivityRows();
			PlayerStatsData snapshot = PlayerStatsService.GetSnapshot();
			string rankName = ((RankProgressionService.CurrentRank != null) ? RankProgressionService.CurrentRank.displayName : "UNRANKED");
			string[] array = new string[]
			{
				rankName,
				RankProgressionService.Level.ToString(),
				string.Format("{0:0}  /  {1:0} TOTAL", RankProgressionService.Xp, RankProgressionService.TotalXpEarned),
				snapshot.runsPlayed.ToString(),
				string.Format("{0:0} m", snapshot.totalDistanceMeters),
				string.Format("{0:0} m", snapshot.bestDistanceMeters),
				string.Format("{0:0} m", snapshot.totalVehicleDistanceMeters),
				snapshot.totalTimesSeen.ToString(),
				snapshot.totalEmotes.ToString(),
				string.Format("{0}  /  BEST {1}", snapshot.totalLocationsVisited, snapshot.bestLocationsVisitedInRun),
				snapshot.totalCarSurfs.ToString()
			};
			int num = 0;
			while (num < this.statsValues.Length && num < array.Length)
			{
				if (!(this.statsValues[num] == null))
				{
					int num2 = num;
					string text = array[num];
					if (num == 0)
					{
						string capturedRankName = rankName;
						LocalizedGameText.BindComputed(this.statsValues[num], delegate
						{
							if (!(rankName == "UNRANKED"))
							{
								return GameLocalization.GetSource("rank", capturedRankName);
							}
							return GameLocalization.GetSource("settings", "UNRANKED");
						});
					}
					else if (num == 2)
					{
						float capturedXp = RankProgressionService.Xp;
						float capturedTotalXp = RankProgressionService.TotalXpEarned;
						LocalizedGameText.BindComputed(this.statsValues[num], () => GameLocalization.Format("menu.collection.xp", "{0:0}  /  {1:0} TOTAL", new object[] { capturedXp, capturedTotalXp }));
					}
					else if (num == 9)
					{
						int capturedVisited = snapshot.totalLocationsVisited;
						int capturedBest = snapshot.bestLocationsVisitedInRun;
						LocalizedGameText.BindComputed(this.statsValues[num], () => GameLocalization.Format("menu.collection.locations", "{0}  / BEST {1}", new object[] { capturedVisited, capturedBest }));
					}
					else
					{
						this.statsValues[num2].text = text;
					}
				}
				num++;
			}
			if (this._activityStatsValues != null)
			{
				string[] array2 = MainMenuCollectionPanel.BuildActivityValues(snapshot);
				for (int i = 0; i < this._activityStatsValues.Length; i++)
				{
					if (this._activityStatsValues[i] != null)
					{
						this._activityStatsValues[i].text = array2[i];
					}
				}
			}
		}

		// Token: 0x06007EB6 RID: 32438 RVA: 0x0033CF4C File Offset: 0x0033B14C
		private static string[] BuildActivityValues(PlayerStatsData stats)
		{
			return new string[]
			{
				MainMenuCollectionPanel.FormatDuration(stats.totalActiveGameplaySeconds),
				MainMenuCollectionPanel.FormatDuration(stats.totalPlayTimeSeconds),
				string.Format("{0}  /  {1:0} m", MainMenuCollectionPanel.FormatDuration(stats.totalWalkingSeconds), stats.totalWalkingMeters),
				string.Format("{0}  /  {1:0} m", MainMenuCollectionPanel.FormatDuration(stats.totalRunningSeconds), stats.totalRunningMeters),
				MainMenuCollectionPanel.FormatDuration(stats.totalEmoteSeconds),
				MainMenuCollectionPanel.FormatDuration(stats.totalVehicleSeconds),
				MainMenuCollectionPanel.FormatDuration(stats.totalCarSeconds),
				MainMenuCollectionPanel.FormatDuration(stats.totalBikeSeconds),
				MainMenuCollectionPanel.FormatDuration(stats.totalBoardSeconds),
				MainMenuCollectionPanel.FormatDuration(stats.totalSkatesSeconds),
				MainMenuCollectionPanel.FormatDuration(stats.totalRoofSeconds),
				stats.totalSteps.ToString("N0")
			};
		}

		// Token: 0x06007EB7 RID: 32439 RVA: 0x0033D03C File Offset: 0x0033B23C
		private static string FormatDuration(float seconds)
		{
			int num = Mathf.Max(0, Mathf.FloorToInt(seconds));
			if (num >= 3600)
			{
				return string.Format("{0:0}h {1:00}m {2:00}s", num / 3600, num / 60 % 60, num % 60);
			}
			return string.Format("{0:0}m {1:00}s", num / 60, num % 60);
		}

		// Token: 0x06007EB8 RID: 32440 RVA: 0x0033D0A8 File Offset: 0x0033B2A8
		private void EnsureActivityRows()
		{
			if (this._activityStatsValues != null || this.statsValues.Length == 0 || this.statsValues[0] == null)
			{
				return;
			}
			RectTransform rectTransform = this.statsValues[0].transform.parent.parent as RectTransform;
			if (rectTransform == null || rectTransform.name != "StatsList")
			{
				return;
			}
			Transform parent = rectTransform.parent;
			Transform transform = parent.Find("ActivityStatsList");
			RectTransform rectTransform2 = MainMenuCollectionPanel.NewStatsRect("StatsViewport", parent);
			rectTransform2.anchorMin = Vector2.zero;
			rectTransform2.anchorMax = Vector2.one;
			rectTransform2.offsetMin = new Vector2(0f, rectTransform.offsetMin.y);
			rectTransform2.offsetMax = new Vector2(0f, rectTransform.offsetMax.y);
			rectTransform2.gameObject.AddComponent<Image>().color = Color.clear;
			rectTransform2.gameObject.AddComponent<RectMask2D>();
			this._statsScroll = rectTransform2.gameObject.AddComponent<ScrollRect>();
			this._statsScroll.viewport = rectTransform2;
			this._statsScroll.content = rectTransform;
			this._statsScroll.horizontal = false;
			this._statsScroll.vertical = true;
			this._statsScroll.movementType = ScrollRect.MovementType.Clamped;
			rectTransform2.gameObject.AddComponent<SmoothCollectionScroll>();
			rectTransform.SetParent(rectTransform2, false);
			rectTransform.anchorMin = new Vector2(0f, 1f);
			rectTransform.anchorMax = Vector2.one;
			rectTransform.pivot = new Vector2(0.5f, 1f);
			rectTransform.offsetMin = new Vector2(0f, 0f);
			rectTransform.offsetMax = new Vector2(-22f, 0f);
			this._activityStatsValues = new TMP_Text[MainMenuCollectionPanel.ActivityLabels.Length];
			for (int i = 0; i < MainMenuCollectionPanel.ActivityLabels.Length; i++)
			{
				Transform transform2 = ((transform != null && transform.childCount > 0) ? transform.GetChild(0) : Object.Instantiate<Transform>(this.statsValues[0].transform.parent, rectTransform));
				transform2.SetParent(rectTransform, false);
				transform2.name = "Row - " + MainMenuCollectionPanel.ActivityLabels[i];
				Transform transform3 = transform2.Find("Label");
				TMP_Text tmp_Text = ((transform3 != null) ? transform3.GetComponent<TMP_Text>() : null);
				if (tmp_Text != null)
				{
					int index = i;
					LocalizedGameText.BindComputed(tmp_Text, delegate
					{
						if (!(GameLocalization.CurrentCode == "it"))
						{
							return MainMenuCollectionPanel.ActivityLabels[index];
						}
						return MainMenuCollectionPanel.ActivityLabelsIt[index];
					});
				}
				TMP_Text[] activityStatsValues = this._activityStatsValues;
				int num = i;
				Transform transform4 = transform2.Find("Value");
				activityStatsValues[num] = ((transform4 != null) ? transform4.GetComponent<TMP_Text>() : null);
			}
			if (transform != null)
			{
				transform.gameObject.SetActive(false);
			}
			for (int j = 0; j < rectTransform.childCount; j++)
			{
				RectTransform rectTransform3 = rectTransform.GetChild(j) as RectTransform;
				if (!(rectTransform3 == null))
				{
					rectTransform3.anchorMin = new Vector2(0f, 1f);
					rectTransform3.anchorMax = Vector2.one;
					rectTransform3.offsetMin = new Vector2(0f, -((float)j * 57f + 49f));
					rectTransform3.offsetMax = new Vector2(0f, (float)(-(float)j) * 57f);
				}
			}
			rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, (float)rectTransform.childCount * 57f - 8f);
			RectTransform rectTransform4 = MainMenuCollectionPanel.NewStatsRect("Scrollbar", rectTransform2);
			rectTransform4.anchorMin = new Vector2(1f, 0f);
			rectTransform4.anchorMax = Vector2.one;
			rectTransform4.offsetMin = new Vector2(-12f, 2f);
			rectTransform4.offsetMax = new Vector2(-4f, -2f);
			rectTransform4.gameObject.AddComponent<Image>().color = new Color(1f, 0.78f, 0.89f, 0.14f);
			Scrollbar scrollbar = rectTransform4.gameObject.AddComponent<Scrollbar>();
			scrollbar.direction = Scrollbar.Direction.BottomToTop;
			RectTransform rectTransform5 = MainMenuCollectionPanel.NewStatsRect("Handle", rectTransform4);
			rectTransform5.anchorMin = Vector2.zero;
			rectTransform5.anchorMax = Vector2.one;
			rectTransform5.offsetMin = (rectTransform5.offsetMax = Vector2.zero);
			Image image = rectTransform5.gameObject.AddComponent<Image>();
			image.color = new Color(1f, 0.31f, 0.61f, 0.88f);
			scrollbar.handleRect = rectTransform5;
			scrollbar.targetGraphic = image;
			this._statsScroll.verticalScrollbar = scrollbar;
			this._statsScroll.verticalScrollbarVisibility = ScrollRect.ScrollbarVisibility.AutoHide;
		}

		// Token: 0x06007EB9 RID: 32441 RVA: 0x0013F605 File Offset: 0x0013D805
		private static RectTransform NewStatsRect(string name, Transform parent)
		{
			RectTransform component = new GameObject(name, new Type[] { typeof(RectTransform) }).GetComponent<RectTransform>();
			component.SetParent(parent, false);
			return component;
		}

		// Token: 0x06007EBA RID: 32442 RVA: 0x0033D53B File Offset: 0x0033B73B
		private void RefreshVisibleStats()
		{
			if (this.statsPage != null && this.statsPage.activeSelf)
			{
				this.RefreshStats();
			}
		}

		// Token: 0x06007EBB RID: 32443 RVA: 0x0033D560 File Offset: 0x0033B760
		private void CountItems(bool clothes, ref int unlocked, ref int total)
		{
			if (this.ItemDefinitions == null)
			{
				return;
			}
			for (int i = 0; i < this.ItemDefinitions.Length; i++)
			{
				CollectionMetadataCatalog.Entry entry = this.ItemDefinitions[i];
				if (MainMenuCollectionPanel.IsCollectibleItem(entry) && entry.item.category == InventoryCategory.Clothing == clothes)
				{
					WardrobeMetadataIndex.Entry item = entry.item;
					if (item.rarityVariants != null && item.rarityVariants.Length != 0)
					{
						for (int j = 0; j < item.rarityVariants.Length; j++)
						{
							WardrobeMetadataIndex.RarityEntry rarityEntry = item.rarityVariants[j];
							if (rarityEntry != null && rarityEntry.exists)
							{
								total++;
								if (CollectionUnlockRegistry.IsVariantUnlockedMetadata(item, rarityEntry.surfaceIndex))
								{
									unlocked++;
								}
							}
						}
					}
					else
					{
						total++;
						if (CollectionUnlockRegistry.IsVariantUnlockedMetadata(item, -1))
						{
							unlocked++;
						}
					}
				}
			}
		}

		// Token: 0x06007EBC RID: 32444 RVA: 0x0033D628 File Offset: 0x0033B828
		private void PopulateItems(bool clothes, ref int unlocked, ref int total)
		{
			if (this.ItemDefinitions == null)
			{
				return;
			}
			for (int i = 0; i < this.ItemDefinitions.Length; i++)
			{
				CollectionMetadataCatalog.Entry entry = this.ItemDefinitions[i];
				if (MainMenuCollectionPanel.IsCollectibleItem(entry) && entry.item.category == InventoryCategory.Clothing == clothes)
				{
					WardrobeMetadataIndex.Entry item = entry.item;
					if (item.rarityVariants != null && item.rarityVariants.Length != 0)
					{
						for (int j = 0; j < item.rarityVariants.Length; j++)
						{
							WardrobeMetadataIndex.RarityEntry rarityEntry = item.rarityVariants[j];
							if (rarityEntry != null && rarityEntry.exists)
							{
								this.CreateItemCard(entry, rarityEntry.surfaceIndex, rarityEntry.tier, ref unlocked, ref total);
							}
						}
					}
					else
					{
						this.CreateItemCard(entry, -1, item.rarity, ref unlocked, ref total);
					}
				}
			}
		}

		// Token: 0x06007EBD RID: 32445 RVA: 0x0033D6E4 File Offset: 0x0033B8E4
		private void CreateItemCard(CollectionMetadataCatalog.Entry definition, int variantIndex, ItemRarity displayRarity, ref int unlocked, ref int total)
		{
			WardrobeMetadataIndex.Entry item = definition.item;
			total++;
			bool isUnlocked = CollectionUnlockRegistry.IsVariantUnlockedMetadata(item, variantIndex);
			bool flag = CollectionUnlockRegistry.IsVariantNewMetadata(item, variantIndex);
			if (isUnlocked)
			{
				unlocked++;
			}
			string text = (string.IsNullOrWhiteSpace(item.itemName) ? item.assetName : item.itemName);
			string text2 = MainMenuCollectionPanel.ResolveVariantName(item, variantIndex, false);
			string text3 = (isUnlocked ? (text + (string.IsNullOrWhiteSpace(text2) ? "" : (" — " + text2))) : "???");
			string text4 = (isUnlocked ? ((CollectionUnlockRegistry.GetFoundCountMetadata(item, variantIndex) > 0) ? string.Format("Found x{0}", CollectionUnlockRegistry.GetFoundCountMetadata(item, variantIndex)) : "Unlocked by rank") : "Keep exploring to discover this entry.");
			if (isUnlocked && this._icons == null)
			{
				this._icons = Resources.Load<CollectionIconCatalog>("CollectionIconCatalog");
			}
			Sprite sprite = ((isUnlocked && this._icons != null) ? this._icons.ResolveIcon(item.assetGuid, variantIndex) : null);
			GameObject gameObject = this.CreateCard(this._activeTab, sprite, text3, isUnlocked ? displayRarity.ToString().ToUpperInvariant() : "UNDISCOVERED", text4, isUnlocked, flag);
			if (gameObject != null)
			{
				Transform transform = gameObject.transform.Find("Icon");
				StreamerCensorImage.Bind((transform != null) ? transform.GetComponent<Image>() : null, item.isSensitiveVisual);
				int capturedFoundCount = CollectionUnlockRegistry.GetFoundCountMetadata(item, variantIndex);
				string capturedItemName = text;
				ItemRarity capturedRarity = displayRarity;
				Transform transform2 = gameObject.transform.Find("Title");
				TMP_Text tmp_Text = ((transform2 != null) ? transform2.GetComponent<TMP_Text>() : null);
				Transform transform3 = gameObject.transform.Find("Category");
				TMP_Text tmp_Text2 = ((transform3 != null) ? transform3.GetComponent<TMP_Text>() : null);
				Transform transform4 = gameObject.transform.Find("Description");
				TMP_Text tmp_Text3 = ((transform4 != null) ? transform4.GetComponent<TMP_Text>() : null);
				if (tmp_Text != null)
				{
					LocalizedGameText.BindComputed(tmp_Text, delegate
					{
						if (!isUnlocked)
						{
							return GameLocalization.GetSource("settings", "???");
						}
						return MainMenuCollectionPanel.ComposeItemTitle(capturedItemName, MainMenuCollectionPanel.ResolveVariantName(item, variantIndex, true));
					});
				}
				if (tmp_Text2 != null)
				{
					LocalizedGameText.BindComputed(tmp_Text2, delegate
					{
						if (!isUnlocked)
						{
							return GameLocalization.GetSource("settings", "UNDISCOVERED");
						}
						return MainMenuCollectionPanel.RarityToString(capturedRarity);
					});
				}
				if (tmp_Text3 != null)
				{
					LocalizedGameText.BindComputed(tmp_Text3, delegate
					{
						if (!isUnlocked)
						{
							return GameLocalization.GetSource("settings", "Keep exploring to discover this entry.");
						}
						if (capturedFoundCount <= 0)
						{
							return GameLocalization.GetSource("settings", "Unlocked by rank");
						}
						return GameLocalization.Format("menu.collection.found", "Found x{0}", new object[] { capturedFoundCount });
					});
				}
			}
			if ((gameObject != null) & isUnlocked)
			{
				Transform transform5 = gameObject.transform.Find("Category");
				TMP_Text tmp_Text4 = ((transform5 != null) ? transform5.GetComponent<TMP_Text>() : null);
				if (tmp_Text4 != null && RaritySettingsSO.Instance != null)
				{
					tmp_Text4.color = RaritySettingsSO.Instance.GetColor(displayRarity);
				}
				Button component = gameObject.GetComponent<Button>();
				if (component != null)
				{
					CollectionMetadataCatalog.Entry capturedItem = definition;
					int capturedVariant = variantIndex;
					GameObject capturedCard = gameObject;
					component.onClick.AddListener(delegate
					{
						this.ShowItemPreview(capturedItem, capturedVariant);
						if (!this._seenTrackingEnabled)
						{
							return;
						}
						CollectionUnlockRegistry.MarkVariantSeenMetadata(capturedItem.item, capturedVariant);
						Transform transform6 = ((capturedCard != null) ? capturedCard.transform.Find("New") : null);
						if (transform6 != null)
						{
							transform6.gameObject.SetActive(false);
						}
					});
				}
			}
		}

		// Token: 0x06007EBE RID: 32446 RVA: 0x0033DAA0 File Offset: 0x0033BCA0
		private void PopulatePoi(ref int unlocked, ref int total)
		{
			if (this.poiDefinitions == null)
			{
				return;
			}
			for (int i = 0; i < this.poiDefinitions.Length; i++)
			{
				PoiEmoteProfileSO poiEmoteProfileSO = this.poiDefinitions[i];
				if (!(poiEmoteProfileSO == null))
				{
					total++;
					bool isUnlocked = CollectionUnlockRegistry.IsPoiUnlocked(poiEmoteProfileSO);
					bool flag = CollectionUnlockRegistry.IsPoiNew(poiEmoteProfileSO);
					if (isUnlocked)
					{
						unlocked++;
					}
					string text = (isUnlocked ? (string.IsNullOrWhiteSpace(poiEmoteProfileSO.DisplayName) ? poiEmoteProfileSO.PoiId : poiEmoteProfileSO.DisplayName) : "???");
					string text2 = (isUnlocked ? (string.IsNullOrWhiteSpace(poiEmoteProfileSO.Description) ? string.Format("Visited x{0}", CollectionUnlockRegistry.GetPoiVisitCount(poiEmoteProfileSO)) : string.Format("{0}\nVisited x{1}", poiEmoteProfileSO.Description, CollectionUnlockRegistry.GetPoiVisitCount(poiEmoteProfileSO))) : "Visit this location to reveal it.");
					GameObject gameObject = this.CreateCard(MainMenuCollectionPanel.CollectionTab.Poi, poiEmoteProfileSO.PreviewImage, text, isUnlocked ? "POINT OF INTEREST" : "UNDISCOVERED", text2, isUnlocked, flag);
					if (gameObject != null)
					{
						string capturedPoiName = (string.IsNullOrWhiteSpace(poiEmoteProfileSO.DisplayName) ? poiEmoteProfileSO.PoiId : poiEmoteProfileSO.DisplayName);
						string capturedPoiDescription = poiEmoteProfileSO.Description;
						int capturedVisits = CollectionUnlockRegistry.GetPoiVisitCount(poiEmoteProfileSO);
						Transform transform = gameObject.transform.Find("Title");
						TMP_Text tmp_Text = ((transform != null) ? transform.GetComponent<TMP_Text>() : null);
						Transform transform2 = gameObject.transform.Find("Category");
						TMP_Text tmp_Text2 = ((transform2 != null) ? transform2.GetComponent<TMP_Text>() : null);
						Transform transform3 = gameObject.transform.Find("Description");
						TMP_Text tmp_Text3 = ((transform3 != null) ? transform3.GetComponent<TMP_Text>() : null);
						if (tmp_Text != null)
						{
							LocalizedGameText.BindComputed(tmp_Text, delegate
							{
								if (!isUnlocked)
								{
									return GameLocalization.GetSource("settings", "???");
								}
								return GameLocalization.GetSource("poi", capturedPoiName);
							});
						}
						if (tmp_Text2 != null)
						{
							LocalizedGameText.BindComputed(tmp_Text2, delegate
							{
								if (!isUnlocked)
								{
									return GameLocalization.GetSource("settings", "UNDISCOVERED");
								}
								return GameLocalization.GetSource("settings", "POINT OF INTEREST");
							});
						}
						if (tmp_Text3 != null)
						{
							LocalizedGameText.BindComputed(tmp_Text3, delegate
							{
								if (!isUnlocked)
								{
									return GameLocalization.GetSource("settings", "Visit this location to reveal it.");
								}
								if (!string.IsNullOrWhiteSpace(capturedPoiDescription))
								{
									return GameLocalization.GetSource("poi", capturedPoiDescription) + "\n" + GameLocalization.Format("menu.collection.visited", "Visited x{0}", new object[] { capturedVisits });
								}
								return GameLocalization.Format("menu.collection.visited", "Visited x{0}", new object[] { capturedVisits });
							});
						}
					}
					MainMenuCollectionPanel.ApplyPoiMapIcon(gameObject, poiEmoteProfileSO, isUnlocked);
					if (isUnlocked && this._seenTrackingEnabled)
					{
						CollectionUnlockRegistry.MarkPoiSeen(poiEmoteProfileSO);
					}
				}
			}
		}

		// Token: 0x06007EBF RID: 32447 RVA: 0x0033DCF8 File Offset: 0x0033BEF8
		private static void ApplyPoiMapIcon(GameObject card, PoiEmoteProfileSO poi, bool isUnlocked)
		{
			if (card == null || poi == null || !isUnlocked)
			{
				return;
			}
			MiniMapPoiKind miniMapPoiKind = MainMenuCollectionPanel.ResolvePoiMapKind(poi.PoiId);
			if (miniMapPoiKind == MiniMapPoiKind.None)
			{
				return;
			}
			RectTransform rectTransform = card.transform.Find("Icon") as RectTransform;
			if (rectTransform == null)
			{
				return;
			}
			Image component = rectTransform.GetComponent<Image>();
			if (component != null)
			{
				component.enabled = false;
			}
			GameObject gameObject = new GameObject("Maps POI Icon", new Type[]
			{
				typeof(RectTransform),
				typeof(CanvasRenderer),
				typeof(ProceduralMiniMapPoiIconGraphic)
			});
			RectTransform rectTransform2 = (RectTransform)gameObject.transform;
			rectTransform2.SetParent(rectTransform, false);
			rectTransform2.anchorMin = (rectTransform2.anchorMax = Vector2.one * 0.5f);
			rectTransform2.anchoredPosition = Vector2.zero;
			rectTransform2.sizeDelta = Vector2.one * 64f;
			ProceduralMiniMapPoiIconGraphic component2 = gameObject.GetComponent<ProceduralMiniMapPoiIconGraphic>();
			component2.ConfigureMapMarker(miniMapPoiKind, false, false, 30f);
			component2.raycastTarget = false;
		}

		// Token: 0x06007EC0 RID: 32448 RVA: 0x0033DE04 File Offset: 0x0033C004
		private static MiniMapPoiKind ResolvePoiMapKind(string poiId)
		{
			string text = ((poiId != null) ? poiId.Trim().ToLowerInvariant() : null);
			uint num = <PrivateImplementationDetails>.ComputeStringHash(text);
			if (num <= 2060279869U)
			{
				if (num <= 1413858991U)
				{
					if (num != 491721833U)
					{
						if (num != 1135095924U)
						{
							if (num != 1413858991U)
							{
								return MiniMapPoiKind.None;
							}
							if (!(text == "gymnasium"))
							{
								return MiniMapPoiKind.None;
							}
							return MiniMapPoiKind.Stadium;
						}
						else if (!(text == "artgallery"))
						{
							return MiniMapPoiKind.None;
						}
					}
					else
					{
						if (!(text == "university"))
						{
							return MiniMapPoiKind.None;
						}
						return MiniMapPoiKind.University;
					}
				}
				else if (num != 1486560014U)
				{
					if (num != 1668812473U)
					{
						if (num != 2060279869U)
						{
							return MiniMapPoiKind.None;
						}
						if (!(text == "gallery"))
						{
							return MiniMapPoiKind.None;
						}
					}
					else
					{
						if (!(text == "park"))
						{
							return MiniMapPoiKind.None;
						}
						return MiniMapPoiKind.Park;
					}
				}
				else
				{
					if (!(text == "mallgym"))
					{
						return MiniMapPoiKind.None;
					}
					return MiniMapPoiKind.Mall;
				}
				return MiniMapPoiKind.Gallery;
			}
			if (num <= 3536372366U)
			{
				if (num != 2447210883U)
				{
					if (num != 3129335658U)
					{
						if (num != 3536372366U)
						{
							return MiniMapPoiKind.None;
						}
						if (!(text == "home"))
						{
							return MiniMapPoiKind.None;
						}
						return MiniMapPoiKind.Home;
					}
					else
					{
						if (!(text == "metro"))
						{
							return MiniMapPoiKind.None;
						}
						return MiniMapPoiKind.Metro;
					}
				}
				else
				{
					if (!(text == "pool"))
					{
						return MiniMapPoiKind.None;
					}
					return MiniMapPoiKind.Pool;
				}
			}
			else if (num != 3672355424U)
			{
				if (num != 3936202235U)
				{
					if (num != 4117826158U)
					{
						return MiniMapPoiKind.None;
					}
					if (!(text == "kombini"))
					{
						return MiniMapPoiKind.None;
					}
					return MiniMapPoiKind.Kombini;
				}
				else
				{
					if (!(text == "mall"))
					{
						return MiniMapPoiKind.None;
					}
					return MiniMapPoiKind.Mall;
				}
			}
			else if (!(text == "stadium"))
			{
				return MiniMapPoiKind.None;
			}
			return MiniMapPoiKind.Stadium;
		}

		// Token: 0x06007EC1 RID: 32449 RVA: 0x0033DFB4 File Offset: 0x0033C1B4
		private GameObject CreateCard(MainMenuCollectionPanel.CollectionTab tab, Sprite icon, string title, string category, string description, bool isUnlocked, bool isNew)
		{
			if (this.contentRoots == null || this.cardTemplates == null || tab >= (MainMenuCollectionPanel.CollectionTab)this.contentRoots.Length || tab >= (MainMenuCollectionPanel.CollectionTab)this.cardTemplates.Length || this.contentRoots[(int)tab] == null || this.cardTemplates[(int)tab] == null)
			{
				return null;
			}
			GameObject gameObject = Object.Instantiate<GameObject>(this.cardTemplates[(int)tab], this.contentRoots[(int)tab]);
			gameObject.name = "Card - " + title;
			gameObject.SetActive(true);
			this._runtimeCards.Add(gameObject);
			Transform transform = gameObject.transform.Find("Icon");
			Image image = ((transform != null) ? transform.GetComponent<Image>() : null);
			if (image != null)
			{
				image.sprite = (isUnlocked ? icon : null);
				image.enabled = isUnlocked && icon != null;
				image.color = Color.white;
			}
			Transform transform2 = gameObject.transform.Find("LockedPlaceholder");
			if (transform2 != null)
			{
				transform2.gameObject.SetActive(!isUnlocked);
			}
			Transform transform3 = gameObject.transform.Find("Title");
			if (transform3 != null)
			{
				transform3.GetComponent<TMP_Text>();
			}
			Transform transform4 = gameObject.transform.Find("Category");
			if (transform4 != null)
			{
				transform4.GetComponent<TMP_Text>();
			}
			Transform transform5 = gameObject.transform.Find("Description");
			if (transform5 != null)
			{
				transform5.GetComponent<TMP_Text>();
			}
			Transform transform6 = gameObject.transform.Find("New");
			if (transform6 != null)
			{
				transform6.gameObject.SetActive(isNew);
			}
			Button component = gameObject.GetComponent<Button>();
			if (component != null)
			{
				component.interactable = isUnlocked;
			}
			return gameObject;
		}

		// Token: 0x06007EC2 RID: 32450 RVA: 0x0033E15C File Offset: 0x0033C35C
		private void ShowItemPreview(CollectionMetadataCatalog.Entry item, int variantIndex)
		{
			this.ResetPreview();
			if (this.previewStage != null && item != null && base.isActiveAndEnabled)
			{
				base.StartCoroutine(this.LoadItemPreview(item, variantIndex, this._previewGeneration));
			}
		}

		// Token: 0x06007EC3 RID: 32451 RVA: 0x0033E192 File Offset: 0x0033C392
		private IEnumerator LoadItemPreview(CollectionMetadataCatalog.Entry item, int variantIndex, int generation)
		{
			ResourceRequest request = item.LoadPreviewAsync();
			if (request == null)
			{
				yield break;
			}
			yield return request;
			this.CompleteItemPreview(item, variantIndex, generation, request.asset);
			yield break;
		}

		// Token: 0x06007EC4 RID: 32452 RVA: 0x0033E1B8 File Offset: 0x0033C3B8
		private void CompleteItemPreview(CollectionMetadataCatalog.Entry item, int variantIndex, int generation, Object asset)
		{
			if (generation != this._previewGeneration || !base.isActiveAndEnabled || this.previewStage == null)
			{
				return;
			}
			InventoryItemSO inventoryItemSO = item.ResolvePreview(asset);
			if (inventoryItemSO != null)
			{
				this.previewStage.Show(inventoryItemSO, variantIndex);
			}
		}

		// Token: 0x06007EC5 RID: 32453 RVA: 0x0033E204 File Offset: 0x0033C404
		private void ResetPreview()
		{
			this._previewGeneration++;
			if (this.previewStage != null)
			{
				this.previewStage.Hide();
			}
		}

		// Token: 0x06007EC6 RID: 32454 RVA: 0x0033E230 File Offset: 0x0033C430
		private void ShowOnly(GameObject target, string kicker)
		{
			GameObject[] array = new GameObject[] { this.rootPage, this.statsPage, this.itemsPage, this.clothesPage, this.poiPage };
			for (int i = 0; i < array.Length; i++)
			{
				if (array[i] != null)
				{
					array[i].SetActive(array[i] == target);
				}
			}
			if (this.previewPane != null)
			{
				this.previewPane.SetActive(target == this.itemsPage || target == this.clothesPage);
			}
			if (this.pageKicker != null)
			{
				LocalizedGameText.BindSource(this.pageKicker, "settings", kicker, null);
			}
		}

		// Token: 0x06007EC7 RID: 32455 RVA: 0x0033E2F2 File Offset: 0x0033C4F2
		private GameObject PageFor(MainMenuCollectionPanel.CollectionTab tab)
		{
			if (tab == MainMenuCollectionPanel.CollectionTab.Clothes)
			{
				return this.clothesPage;
			}
			if (tab != MainMenuCollectionPanel.CollectionTab.Poi)
			{
				return this.itemsPage;
			}
			return this.poiPage;
		}

		// Token: 0x06007EC8 RID: 32456 RVA: 0x0033E314 File Offset: 0x0033C514
		private bool IsTabPageActive()
		{
			GameObject gameObject = this.PageFor(this._activeTab);
			return gameObject != null && gameObject.activeSelf;
		}

		// Token: 0x06007EC9 RID: 32457 RVA: 0x0033E340 File Offset: 0x0033C540
		private static bool IsCollectibleItem(CollectionMetadataCatalog.Entry definition)
		{
			WardrobeMetadataIndex.Entry entry = ((definition != null) ? definition.item : null);
			return entry != null && !entry.isVaulted && !definition.excluded && (CollarAvailabilityPolicy.GetPartId(entry.itemId) == null || CollarAvailabilityPolicy.IsRankUnlocked) && (!entry.isWearable || entry.showInGameplayInventory || entry.hasGameplaySurfaceOverride);
		}

		// Token: 0x06007ECA RID: 32458 RVA: 0x0033E39C File Offset: 0x0033C59C
		private void UpdateGridLayout(MainMenuCollectionPanel.CollectionTab tab)
		{
			if (this.contentRoots == null || tab >= (MainMenuCollectionPanel.CollectionTab)this.contentRoots.Length || this.contentRoots[(int)tab] == null)
			{
				return;
			}
			RectTransform rectTransform = this.contentRoots[(int)tab];
			GridLayoutGroup component = rectTransform.GetComponent<GridLayoutGroup>();
			if (component == null)
			{
				return;
			}
			int num = ((component.constraint == GridLayoutGroup.Constraint.FixedColumnCount) ? Mathf.Max(1, component.constraintCount) : 1);
			int num2 = Mathf.CeilToInt((float)this._runtimeCards.Count / (float)num);
			float num3 = (float)(component.padding.top + component.padding.bottom);
			if (num2 > 0)
			{
				num3 += (float)num2 * component.cellSize.y + (float)(num2 - 1) * component.spacing.y;
			}
			ScrollRect componentInParent = rectTransform.GetComponentInParent<ScrollRect>();
			float num4 = ((componentInParent != null && componentInParent.viewport != null) ? componentInParent.viewport.rect.height : 0f);
			rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, Mathf.Max(num4, num3));
			LayoutRebuilder.ForceRebuildLayoutImmediate(rectTransform);
			if (componentInParent != null)
			{
				componentInParent.StopMovement();
				componentInParent.verticalNormalizedPosition = 1f;
			}
		}

		// Token: 0x06007ECB RID: 32459 RVA: 0x0033E4D0 File Offset: 0x0033C6D0
		private static string ResolveVariantName(WardrobeMetadataIndex.Entry item, int variantIndex, bool localized = false)
		{
			WardrobeMetadataIndex.SurfaceEntry surfaceEntry;
			if (item.TryGetSurface(variantIndex, out surfaceEntry))
			{
				if (!string.IsNullOrWhiteSpace(surfaceEntry.displayName))
				{
					return surfaceEntry.displayName;
				}
				return surfaceEntry.assetName;
			}
			else
			{
				if (variantIndex < 0)
				{
					return string.Empty;
				}
				if (item.rarityVariants != null)
				{
					for (int i = 0; i < item.rarityVariants.Length; i++)
					{
						WardrobeMetadataIndex.RarityEntry rarityEntry = item.rarityVariants[i];
						if (rarityEntry != null && rarityEntry.exists && rarityEntry.surfaceIndex == variantIndex)
						{
							return rarityEntry.surfaceName;
						}
					}
				}
				if (!localized)
				{
					return "COLORWAY " + (variantIndex + 1).ToString();
				}
				return GameLocalization.Format("menu.collection.colorway", "COLORWAY {0}", new object[] { variantIndex + 1 });
			}
		}

		// Token: 0x06007ECC RID: 32460 RVA: 0x0033E588 File Offset: 0x0033C788
		private static string ComposeItemTitle(string itemName, string variantName)
		{
			string source = GameLocalization.GetSource("items", itemName);
			if (string.IsNullOrWhiteSpace(variantName))
			{
				return source;
			}
			string source2 = GameLocalization.GetSource("item_variant", variantName);
			return GameLocalization.Format("menu.collection.item_variant", "{0} — {1}", new object[] { source, source2 });
		}

		// Token: 0x06007ECD RID: 32461 RVA: 0x0033E5D4 File Offset: 0x0033C7D4
		private static string RarityToString(ItemRarity rarity)
		{
			return GameLocalization.GetSource("rarity", rarity.ToString().ToUpperInvariant());
		}

		// Token: 0x06007ECE RID: 32462 RVA: 0x0033E5F2 File Offset: 0x0033C7F2
		private static int Percent(int unlocked, int total)
		{
			if (total <= 0)
			{
				return 0;
			}
			return Mathf.RoundToInt((float)unlocked * 100f / (float)total);
		}

		// Token: 0x06007ECF RID: 32463 RVA: 0x0033E60A File Offset: 0x0033C80A
		private static string TabLabel(MainMenuCollectionPanel.CollectionTab tab)
		{
			if (tab == MainMenuCollectionPanel.CollectionTab.Clothes)
			{
				return "CLOTHES";
			}
			if (tab != MainMenuCollectionPanel.CollectionTab.Poi)
			{
				return "ITEMS";
			}
			return "POI";
		}

		// Token: 0x06007ED0 RID: 32464 RVA: 0x0033E628 File Offset: 0x0033C828
		private void ClearCards()
		{
			for (int i = 0; i < this._runtimeCards.Count; i++)
			{
				if (this._runtimeCards[i] != null)
				{
					Object.Destroy(this._runtimeCards[i]);
				}
			}
			this._runtimeCards.Clear();
			this._icons = null;
		}

		// Token: 0x0400823A RID: 33338
		[Header("Catalogue")]
		[SerializeField]
		[HideInInspector]
		private InventoryItemSO[] itemDefinitions;

		// Token: 0x0400823B RID: 33339
		[SerializeField]
		[HideInInspector]
		private InventoryItemSO[] excludedItemDefinitions;

		// Token: 0x0400823C RID: 33340
		[SerializeField]
		private PoiEmoteProfileSO[] poiDefinitions;

		// Token: 0x0400823D RID: 33341
		[Header("Pages")]
		[SerializeField]
		private GameObject rootPage;

		// Token: 0x0400823E RID: 33342
		[SerializeField]
		private GameObject statsPage;

		// Token: 0x0400823F RID: 33343
		[SerializeField]
		private GameObject itemsPage;

		// Token: 0x04008240 RID: 33344
		[SerializeField]
		private GameObject clothesPage;

		// Token: 0x04008241 RID: 33345
		[SerializeField]
		private GameObject poiPage;

		// Token: 0x04008242 RID: 33346
		[SerializeField]
		private TMP_Text pageKicker;

		// Token: 0x04008243 RID: 33347
		[Header("Root completion")]
		[SerializeField]
		private TMP_Text itemsCompletionLabel;

		// Token: 0x04008244 RID: 33348
		[SerializeField]
		private TMP_Text clothesCompletionLabel;

		// Token: 0x04008245 RID: 33349
		[SerializeField]
		private TMP_Text poiCompletionLabel;

		// Token: 0x04008246 RID: 33350
		[Header("Catalogue grids (Items, Clothes, POI)")]
		[SerializeField]
		private RectTransform[] contentRoots;

		// Token: 0x04008247 RID: 33351
		[SerializeField]
		private GameObject[] cardTemplates;

		// Token: 0x04008248 RID: 33352
		[SerializeField]
		private TMP_Text[] summaryLabels;

		// Token: 0x04008249 RID: 33353
		[SerializeField]
		private TMP_Text[] emptyLabels;

		// Token: 0x0400824A RID: 33354
		[Header("Stats")]
		[SerializeField]
		private TMP_Text[] statsValues;

		// Token: 0x0400824B RID: 33355
		private TMP_Text[] _activityStatsValues;

		// Token: 0x0400824C RID: 33356
		private ScrollRect _statsScroll;

		// Token: 0x0400824D RID: 33357
		private static readonly string[] ActivityLabels = new string[]
		{
			"TOTAL PLAYTIME", "RUN PLAYTIME", "WALKING", "RUNNING", "EMOTING", "VEHICLES", "CAR / SCOOTER / MOTORBIKE", "BICYCLE", "SKATEBOARD", "SKATES",
			"CAR SURF", "FOOTSTEPS"
		};

		// Token: 0x0400824E RID: 33358
		private static readonly string[] ActivityLabelsIt = new string[]
		{
			"TEMPO DI GIOCO", "TEMPO NELLE CORSE", "CAMMINATA", "CORSA", "EMOTE", "VEICOLI", "AUTO / SCOOTER / MOTO", "BICICLETTA", "SKATEBOARD", "PATTINI",
			"SURF AUTO", "PASSI"
		};

		// Token: 0x0400824F RID: 33359
		[Header("Persistent item preview")]
		[SerializeField]
		private GameObject previewPane;

		// Token: 0x04008250 RID: 33360
		[SerializeField]
		private ItemPreviewStage previewStage;

		// Token: 0x04008251 RID: 33361
		private readonly List<GameObject> _runtimeCards = new List<GameObject>();

		// Token: 0x04008252 RID: 33362
		private MainMenuCollectionPanel.CollectionTab _activeTab;

		// Token: 0x04008253 RID: 33363
		private bool _seenTrackingEnabled = true;

		// Token: 0x04008254 RID: 33364
		private CollectionMetadataCatalog.Entry[] _resolvedItemDefinitions;

		// Token: 0x04008255 RID: 33365
		private CollectionIconCatalog _icons;

		// Token: 0x04008256 RID: 33366
		private int _previewGeneration;

		// Token: 0x0200104B RID: 4171
		public enum CollectionTab
		{
			// Token: 0x04008258 RID: 33368
			Items,
			// Token: 0x04008259 RID: 33369
			Clothes,
			// Token: 0x0400825A RID: 33370
			Poi
		}
	}
}
