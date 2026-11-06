using System;
using System.Collections.Generic;
using Features.CensorBars;
using Features.Localization;
using Features.MainMenu;
using Features.Rank;
using TMPro;
using UnityEngine;

namespace BoZo.CharacterCreator.MC.StyleSystem.UI
{
	// Token: 0x0200080B RID: 2059
	public class AccessoryListPanelController : MonoBehaviour
	{
		// Token: 0x170009DB RID: 2523
		// (get) Token: 0x060036DD RID: 14045 RVA: 0x0013C563 File Offset: 0x0013A763
		public bool IsActive
		{
			get
			{
				return this.panelRoot != null && this.panelRoot.activeSelf;
			}
		}

		// Token: 0x170009DC RID: 2524
		// (get) Token: 0x060036DE RID: 14046 RVA: 0x0013C580 File Offset: 0x0013A780
		public GameObject CardPrefabTemplate
		{
			get
			{
				return this.cardPrefab;
			}
		}

		// Token: 0x060036DF RID: 14047 RVA: 0x0013C588 File Offset: 0x0013A788
		private void OnEnable()
		{
			if (this.styleService == null)
			{
				this.styleService = base.GetComponentInParent<MCStyleCustomizationService>(true) ?? Object.FindObjectOfType<MCStyleCustomizationService>(true);
			}
			CollectionUnlockRegistry.Changed += this.OnCollectionChanged;
			RankProgressionService.Changed += this.OnRankChanged;
		}

		// Token: 0x060036E0 RID: 14048 RVA: 0x0013C5DC File Offset: 0x0013A7DC
		private void OnDisable()
		{
			CollectionUnlockRegistry.Changed -= this.OnCollectionChanged;
			RankProgressionService.Changed -= this.OnRankChanged;
		}

		// Token: 0x060036E1 RID: 14049 RVA: 0x0013C600 File Offset: 0x0013A800
		public void Show()
		{
			this._currentGroup = string.Empty;
			List<string> list = new List<string>();
			for (int i = 0; i < this.accessories.Count; i++)
			{
				if (this.accessories[i] != null && !string.IsNullOrWhiteSpace(this.accessories[i].partId))
				{
					list.Add(this.accessories[i].partId);
				}
			}
			this.BuildVisibleCardEntries(list);
			if (this.panelRoot != null)
			{
				this.panelRoot.SetActive(true);
			}
			this.RebuildCards();
		}

		// Token: 0x060036E2 RID: 14050 RVA: 0x0013C698 File Offset: 0x0013A898
		public void ShowForGroup(string subCategoryName)
		{
			this._currentGroup = subCategoryName ?? string.Empty;
			string currentGroup = this._currentGroup;
			IReadOnlyList<string> readOnlyList;
			if (!(currentGroup == "Stuff"))
			{
				if (!(currentGroup == "Feet"))
				{
					if (!(currentGroup == "Misc"))
					{
						if (!(currentGroup == "Special Events"))
						{
							readOnlyList = Array.Empty<string>();
						}
						else
						{
							List<string> list = new List<string>();
							if (this.styleService != null && this.styleService.PartDefinitions != null)
							{
								foreach (MCStylePartDefinitionSO mcstylePartDefinitionSO in this.styleService.PartDefinitions)
								{
									if (mcstylePartDefinitionSO != null && !string.IsNullOrWhiteSpace(mcstylePartDefinitionSO.partId) && !list.Contains(mcstylePartDefinitionSO.partId))
									{
										list.Add(mcstylePartDefinitionSO.partId);
									}
								}
							}
							readOnlyList = list;
						}
					}
					else
					{
						readOnlyList = new string[] { "Outfit.Misc", "Outfit.Drape", "Outfit.Blindfold", "Outfit.Swimsuit", "Accessory.CowHeadband", "Accessory.CowTail" };
					}
				}
				else
				{
					readOnlyList = new string[] { "Outfit.Shoes", "Outfit.Socks" };
					this.EnforceFeetExclusivityOnOpen();
				}
			}
			else
			{
				readOnlyList = new string[]
				{
					"Outfit.Mask", "Accessory.Eyewear", "Accessory.Headpin", "Accessory.Collar", "Accessory.LeadChoker", "Accessory.NeckTag", "Accessory.NeckSign", "Accessory.Earphones", "Accessory.Wrist", "Accessory.NipplePiercing",
					"Accessory.LilyNecklace", "Accessory.LilyBangle"
				};
			}
			this.BuildVisibleCardEntries(readOnlyList);
			if (this.panelRoot != null)
			{
				this.panelRoot.SetActive(true);
			}
			this.RebuildCards();
		}

		// Token: 0x060036E3 RID: 14051 RVA: 0x0013C894 File Offset: 0x0013AA94
		public void Hide()
		{
			if (this.panelRoot != null)
			{
				this.panelRoot.SetActive(false);
			}
			this.SetEmptyStateVisible(false);
			this.ClearCards();
			this._visibleCards.Clear();
			this._currentGroup = string.Empty;
		}

		// Token: 0x060036E4 RID: 14052 RVA: 0x0013C8D4 File Offset: 0x0013AAD4
		private void BuildVisibleCardEntries(IReadOnlyList<string> partIds)
		{
			this._visibleCards.Clear();
			if (this.styleService == null || partIds == null)
			{
				return;
			}
			for (int i = 0; i < partIds.Count; i++)
			{
				string text = partIds[i];
				MCStylePartDefinitionSO definition = this.styleService.GetDefinition(text);
				if (!(definition == null) && definition.stylePresets != null)
				{
					if (string.Equals(text, "Accessory.Headpin", StringComparison.Ordinal) && definition.stylePresets.Count > 0)
					{
						List<int> list = this.CollectGroupedPresetIndices(definition, null);
						if (list.Count != 0)
						{
							int num = Mathf.Clamp(definition.defaultStyleIndex, 0, definition.stylePresets.Count - 1);
							if (!list.Contains(num))
							{
								num = list[0];
							}
							MCStylePresetSO mcstylePresetSO = definition.stylePresets[num];
							this._visibleCards.Add(new AccessoryListPanelController.CardEntry
							{
								cardId = text + "::variants",
								partId = text,
								presetIndex = num,
								preset = mcstylePresetSO,
								displayName = (string.IsNullOrWhiteSpace(definition.displayName) ? "Headpin" : definition.displayName),
								groupsPhysicalPresets = true,
								groupedPresetIndices = list
							});
						}
					}
					else
					{
						HashSet<string> hashSet = new HashSet<string>(StringComparer.Ordinal);
						for (int j = 0; j < definition.stylePresets.Count; j++)
						{
							MCStylePresetSO mcstylePresetSO2 = definition.stylePresets[j];
							if (this.IsPresetInCurrentGroup(mcstylePresetSO2) && CustomizationWardrobeDiscoveryPolicy.Evaluate(mcstylePresetSO2).IsDiscovered)
							{
								if (!string.IsNullOrWhiteSpace(mcstylePresetSO2.accessoryVariantGroupId))
								{
									if (hashSet.Add(mcstylePresetSO2.accessoryVariantGroupId))
									{
										this._visibleCards.Add(new AccessoryListPanelController.CardEntry
										{
											cardId = text + "::group::" + mcstylePresetSO2.accessoryVariantGroupId,
											partId = text,
											presetIndex = j,
											preset = mcstylePresetSO2,
											displayName = (string.IsNullOrWhiteSpace(mcstylePresetSO2.accessoryVariantGroupName) ? mcstylePresetSO2.displayName : mcstylePresetSO2.accessoryVariantGroupName),
											groupsPhysicalPresets = true,
											groupedPresetIndices = this.CollectGroupedPresetIndices(definition, mcstylePresetSO2.accessoryVariantGroupId)
										});
									}
								}
								else
								{
									this._visibleCards.Add(new AccessoryListPanelController.CardEntry
									{
										cardId = AccessoryListPanelController.CreateCardId(text, j),
										partId = text,
										presetIndex = j,
										preset = mcstylePresetSO2,
										displayName = (string.IsNullOrWhiteSpace(mcstylePresetSO2.displayName) ? mcstylePresetSO2.name : mcstylePresetSO2.displayName)
									});
								}
							}
						}
					}
				}
			}
		}

		// Token: 0x060036E5 RID: 14053 RVA: 0x0013CB78 File Offset: 0x0013AD78
		private bool IsPresetInCurrentGroup(MCStylePresetSO preset)
		{
			return preset != null && preset.isSpecialEventClothing == string.Equals(this._currentGroup, "Special Events", StringComparison.Ordinal);
		}

		// Token: 0x060036E6 RID: 14054 RVA: 0x0013CBA0 File Offset: 0x0013ADA0
		private List<int> CollectGroupedPresetIndices(MCStylePartDefinitionSO definition, string groupId)
		{
			List<int> list = new List<int>();
			for (int i = 0; i < definition.stylePresets.Count; i++)
			{
				MCStylePresetSO mcstylePresetSO = definition.stylePresets[i];
				if (this.IsPresetInCurrentGroup(mcstylePresetSO) && CustomizationWardrobeDiscoveryPolicy.Evaluate(mcstylePresetSO).IsDiscovered && (groupId == null || string.Equals(mcstylePresetSO.accessoryVariantGroupId, groupId, StringComparison.Ordinal)))
				{
					list.Add(i);
				}
			}
			if (groupId != null)
			{
				list.Sort(delegate(int a, int b)
				{
					int num = definition.stylePresets[a].accessoryVariantOrder.CompareTo(definition.stylePresets[b].accessoryVariantOrder);
					if (num == 0)
					{
						return a.CompareTo(b);
					}
					return num;
				});
			}
			return list;
		}

		// Token: 0x060036E7 RID: 14055 RVA: 0x0013CC34 File Offset: 0x0013AE34
		private void RebuildCards()
		{
			this.ClearCards();
			if (this.cardPrefab == null || this.cardContainer == null || this.styleService == null)
			{
				this.SetEmptyStateVisible(true);
				return;
			}
			for (int i = 0; i < this._visibleCards.Count; i++)
			{
				AccessoryListPanelController.CardEntry cardEntry = this._visibleCards[i];
				GameObject gameObject = Object.Instantiate<GameObject>(this.cardPrefab, this.cardContainer);
				gameObject.name = (cardEntry.groupsPhysicalPresets ? ("AccessoryCard_" + cardEntry.partId + "_Variants") : ("AccessoryCard_" + cardEntry.partId + "_" + cardEntry.preset.presetId));
				AccessoryCardUI accessoryCardUI = gameObject.GetComponent<AccessoryCardUI>() ?? gameObject.AddComponent<AccessoryCardUI>();
				accessoryCardUI.Setup(cardEntry.cardId, cardEntry.displayName, this.IsPresetEquipped(cardEntry), this.GetVariantCount(cardEntry), new Action<string, bool>(this.OnCardToggled), new Action<string, int>(this.OnSurfaceCycled));
				this.UpdateCardVisual(accessoryCardUI, cardEntry);
				this._cards.Add(accessoryCardUI);
			}
			this.SetEmptyStateVisible(this._cards.Count == 0);
		}

		// Token: 0x060036E8 RID: 14056 RVA: 0x0013CD6C File Offset: 0x0013AF6C
		private void SetEmptyStateVisible(bool visible)
		{
			this.EnsureEmptyState();
			bool flag = string.Equals(this._currentGroup, "Special Events", StringComparison.Ordinal);
			if (this.emptyStateTitleLabel != null)
			{
				LocalizedGameText localizedGameText = LocalizedGameText.BindSource(this.emptyStateTitleLabel, "cielos_apps", flag ? "No special event clothing unlocked" : this.emptyStateTitle, null);
				if (localizedGameText != null)
				{
					localizedGameText.FitTranslationToRectangle(false);
				}
			}
			if (this.emptyStateLabel != null)
			{
				LocalizedGameText localizedGameText2 = LocalizedGameText.BindSource(this.emptyStateLabel, "cielos_apps", flag ? "Special event clothing appears here when you meet its unlock requirements." : this.emptyStateMessage, null);
				if (localizedGameText2 != null)
				{
					localizedGameText2.FitTranslationToRectangle(false);
				}
			}
			if (this.emptyStateRoot != null)
			{
				this.emptyStateRoot.SetActive(visible);
			}
		}

		// Token: 0x060036E9 RID: 14057 RVA: 0x0013CE21 File Offset: 0x0013B021
		private void EnsureEmptyState()
		{
			if (this.emptyStateRoot != null || this.cardContainer == null)
			{
				return;
			}
			this.emptyStateRoot = WardrobeEmptyStateFactory.Create(this.cardContainer, out this.emptyStateTitleLabel, out this.emptyStateLabel);
		}

		// Token: 0x060036EA RID: 14058 RVA: 0x0013CE60 File Offset: 0x0013B060
		private void ClearCards()
		{
			for (int i = this._cards.Count - 1; i >= 0; i--)
			{
				if (this._cards[i] != null && this._cards[i].gameObject != null)
				{
					Object.Destroy(this._cards[i].gameObject);
				}
			}
			this._cards.Clear();
		}

		// Token: 0x060036EB RID: 14059 RVA: 0x0013CED4 File Offset: 0x0013B0D4
		private void OnCardToggled(string cardId, bool newState)
		{
			AccessoryListPanelController.CardEntry cardEntry = this.FindEntry(cardId);
			if (cardEntry == null || this.styleService == null)
			{
				return;
			}
			if (!newState)
			{
				if (this.IsPresetEquipped(cardEntry))
				{
					this.styleService.SetSlotEquipped(cardEntry.partId, false);
				}
			}
			else
			{
				if (string.Equals(this._currentGroup, "Feet", StringComparison.OrdinalIgnoreCase))
				{
					string text = (string.Equals(cardEntry.partId, "Outfit.Shoes", StringComparison.Ordinal) ? "Outfit.Socks" : "Outfit.Shoes");
					this.styleService.SetSlotEquipped(text, false);
				}
				if (cardEntry.groupsPhysicalPresets)
				{
					this.SelectGroupedPreset(cardEntry, this.ResolveGroupedPresetIndex(cardEntry));
				}
				else
				{
					this.SelectPreset(cardEntry);
				}
			}
			this.RefreshAllCards();
		}

		// Token: 0x060036EC RID: 14060 RVA: 0x0013CF84 File Offset: 0x0013B184
		private void OnSurfaceCycled(string cardId, int direction)
		{
			AccessoryListPanelController.CardEntry cardEntry = this.FindEntry(cardId);
			if (cardEntry == null || this.styleService == null || this.GetVariantCount(cardEntry) <= 1)
			{
				return;
			}
			if (cardEntry.groupsPhysicalPresets)
			{
				int variantCount = this.GetVariantCount(cardEntry);
				int num = cardEntry.groupedPresetIndices.IndexOf(this.ResolveGroupedPresetIndex(cardEntry));
				if (num < 0)
				{
					num = 0;
				}
				int num2 = ((num + direction) % variantCount + variantCount) % variantCount;
				if (this.SelectGroupedPreset(cardEntry, cardEntry.groupedPresetIndices[num2]))
				{
					this.RefreshAllCards();
				}
				return;
			}
			if (!this.IsPresetEquipped(cardEntry) && !this.SelectPreset(cardEntry))
			{
				return;
			}
			int slotSurfaceIndex = this.styleService.GetSlotSurfaceIndex(cardEntry.partId);
			List<int> discoveredSurfaceIndices = CustomizationWardrobeDiscoveryPolicy.GetDiscoveredSurfaceIndices(cardEntry.preset);
			int num3 = discoveredSurfaceIndices.IndexOf(slotSurfaceIndex);
			if (num3 < 0)
			{
				num3 = 0;
			}
			int num4 = ((num3 + direction) % discoveredSurfaceIndices.Count + discoveredSurfaceIndices.Count) % discoveredSurfaceIndices.Count;
			int num5 = discoveredSurfaceIndices[num4];
			this.styleService.SelectSurfaceForSlot(cardEntry.partId, num5);
			this.RefreshAllCards();
		}

		// Token: 0x060036ED RID: 14061 RVA: 0x0013D08C File Offset: 0x0013B28C
		private bool SelectPreset(AccessoryListPanelController.CardEntry entry)
		{
			MCStylePartDefinitionSO definition = this.styleService.GetDefinition(entry.partId);
			int num = ((definition != null && definition.allowNoSelection) ? 1 : 0);
			if (!this.styleService.SelectStyleForSlot(entry.partId, entry.presetIndex + num))
			{
				return false;
			}
			int num2 = CustomizationWardrobeDiscoveryPolicy.ResolvePreferredDiscoveredSurfaceIndex(entry.preset, entry.preset.ResolveDefaultSurfaceIndex());
			if (num2 >= 0)
			{
				this.styleService.SelectSurfaceForSlot(entry.partId, num2);
			}
			return true;
		}

		// Token: 0x060036EE RID: 14062 RVA: 0x0013D110 File Offset: 0x0013B310
		private bool IsPresetEquipped(AccessoryListPanelController.CardEntry entry)
		{
			if (entry == null || this.styleService == null)
			{
				return false;
			}
			if (!entry.groupsPhysicalPresets)
			{
				string text;
				return this.styleService.TryGetEquippedItem(entry.partId, out text) && string.Equals(text, entry.preset.presetId, StringComparison.Ordinal);
			}
			string text2;
			if (!this.styleService.TryGetEquippedItem(entry.partId, out text2))
			{
				return false;
			}
			MCStylePartDefinitionSO definition = this.styleService.GetDefinition(entry.partId);
			foreach (int num in entry.groupedPresetIndices)
			{
				MCStylePresetSO mcstylePresetSO = definition.stylePresets[num];
				if (mcstylePresetSO != null && string.Equals(text2, mcstylePresetSO.presetId, StringComparison.Ordinal))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x060036EF RID: 14063 RVA: 0x0013D200 File Offset: 0x0013B400
		private void RefreshAllCards()
		{
			for (int i = 0; i < this._cards.Count; i++)
			{
				AccessoryCardUI accessoryCardUI = this._cards[i];
				AccessoryListPanelController.CardEntry cardEntry = ((accessoryCardUI != null) ? this.FindEntry(accessoryCardUI.PartId) : null);
				if (!(accessoryCardUI == null) && cardEntry != null)
				{
					accessoryCardUI.SetActiveState(this.IsPresetEquipped(cardEntry));
					this.UpdateCardVisual(accessoryCardUI, cardEntry);
				}
			}
		}

		// Token: 0x060036F0 RID: 14064 RVA: 0x0013D26C File Offset: 0x0013B46C
		private void UpdateCardVisual(AccessoryCardUI card, AccessoryListPanelController.CardEntry entry)
		{
			if (card == null || entry == null || entry.preset == null)
			{
				return;
			}
			if (!entry.groupsPhysicalPresets)
			{
				int num = (this.IsPresetEquipped(entry) ? this.styleService.GetSlotSurfaceIndex(entry.partId) : AccessoryListPanelController.ResolvePreviewSurfaceIndex(entry.preset));
				Sprite sprite = entry.preset.previewIcon;
				MCSurfaceVariantSO mcsurfaceVariantSO;
				if (entry.preset.TryGetSurfaceVariant(num, out mcsurfaceVariantSO) && mcsurfaceVariantSO != null)
				{
					if (mcsurfaceVariantSO.previewIcon != null)
					{
						sprite = mcsurfaceVariantSO.previewIcon;
					}
					card.SetStyleLabel(string.IsNullOrWhiteSpace(mcsurfaceVariantSO.displayName) ? mcsurfaceVariantSO.name : mcsurfaceVariantSO.displayName);
				}
				else
				{
					card.SetStyleLabel(entry.displayName);
				}
				card.SetPreview(sprite, StreamerCensorImage.IsSensitiveStyle(entry.partId));
				return;
			}
			MCStylePartDefinitionSO definition = this.styleService.GetDefinition(entry.partId);
			int num2 = this.ResolveGroupedPresetIndex(entry);
			if (definition == null || definition.stylePresets == null || num2 < 0 || num2 >= definition.stylePresets.Count || definition.stylePresets[num2] == null)
			{
				return;
			}
			MCStylePresetSO mcstylePresetSO = definition.stylePresets[num2];
			int num3 = (this.IsPresetEquipped(entry) ? this.styleService.GetSlotSurfaceIndex(entry.partId) : AccessoryListPanelController.ResolvePreviewSurfaceIndex(mcstylePresetSO));
			Sprite sprite2 = mcstylePresetSO.previewIcon;
			MCSurfaceVariantSO mcsurfaceVariantSO2;
			if (mcstylePresetSO.TryGetSurfaceVariant(num3, out mcsurfaceVariantSO2) && mcsurfaceVariantSO2 != null && mcsurfaceVariantSO2.previewIcon != null)
			{
				sprite2 = mcsurfaceVariantSO2.previewIcon;
			}
			card.SetPreview(sprite2, StreamerCensorImage.IsSensitiveStyle(entry.partId));
			card.SetStyleLabel((!string.IsNullOrWhiteSpace(mcstylePresetSO.accessoryVariantLabel)) ? mcstylePresetSO.accessoryVariantLabel : (string.IsNullOrWhiteSpace(mcstylePresetSO.displayName) ? mcstylePresetSO.name : mcstylePresetSO.displayName));
		}

		// Token: 0x060036F1 RID: 14065 RVA: 0x0013D454 File Offset: 0x0013B654
		private int GetVariantCount(AccessoryListPanelController.CardEntry entry)
		{
			if (entry == null)
			{
				return 0;
			}
			if (!entry.groupsPhysicalPresets)
			{
				if (!(entry.preset != null))
				{
					return 0;
				}
				return CustomizationWardrobeDiscoveryPolicy.GetDiscoveredSurfaceIndices(entry.preset).Count;
			}
			else
			{
				if (entry.groupedPresetIndices == null)
				{
					return 0;
				}
				return entry.groupedPresetIndices.Count;
			}
		}

		// Token: 0x060036F2 RID: 14066 RVA: 0x0013D4A4 File Offset: 0x0013B6A4
		private int ResolveGroupedPresetIndex(AccessoryListPanelController.CardEntry entry)
		{
			MCStylePartDefinitionSO mcstylePartDefinitionSO = ((entry != null && this.styleService != null) ? this.styleService.GetDefinition(entry.partId) : null);
			if (mcstylePartDefinitionSO == null || mcstylePartDefinitionSO.stylePresets == null || mcstylePartDefinitionSO.stylePresets.Count == 0)
			{
				return 0;
			}
			string text;
			if (this.styleService.TryGetEquippedItem(entry.partId, out text))
			{
				foreach (int num in entry.groupedPresetIndices)
				{
					MCStylePresetSO mcstylePresetSO = mcstylePartDefinitionSO.stylePresets[num];
					if (mcstylePresetSO != null && string.Equals(mcstylePresetSO.presetId, text, StringComparison.Ordinal))
					{
						return num;
					}
				}
			}
			if (!entry.groupedPresetIndices.Contains(entry.presetIndex))
			{
				return entry.groupedPresetIndices[0];
			}
			return entry.presetIndex;
		}

		// Token: 0x060036F3 RID: 14067 RVA: 0x0013D5A4 File Offset: 0x0013B7A4
		private bool SelectGroupedPreset(AccessoryListPanelController.CardEntry entry, int presetIndex)
		{
			MCStylePartDefinitionSO mcstylePartDefinitionSO = ((entry != null && this.styleService != null) ? this.styleService.GetDefinition(entry.partId) : null);
			if (mcstylePartDefinitionSO == null || mcstylePartDefinitionSO.stylePresets == null || presetIndex < 0 || presetIndex >= mcstylePartDefinitionSO.stylePresets.Count || !entry.groupedPresetIndices.Contains(presetIndex) || mcstylePartDefinitionSO.stylePresets[presetIndex] == null)
			{
				return false;
			}
			int num = (mcstylePartDefinitionSO.allowNoSelection ? 1 : 0);
			bool flag = this.styleService.SelectStyleForSlot(entry.partId, presetIndex + num);
			if (flag)
			{
				entry.presetIndex = presetIndex;
				entry.preset = mcstylePartDefinitionSO.stylePresets[presetIndex];
				int num2 = CustomizationWardrobeDiscoveryPolicy.ResolvePreferredDiscoveredSurfaceIndex(entry.preset, entry.preset.ResolveDefaultSurfaceIndex());
				if (num2 >= 0)
				{
					this.styleService.SelectSurfaceForSlot(entry.partId, num2);
				}
			}
			return flag;
		}

		// Token: 0x060036F4 RID: 14068 RVA: 0x0013D68C File Offset: 0x0013B88C
		private void EnforceFeetExclusivityOnOpen()
		{
			if (this.styleService != null && this.styleService.IsSlotEquipped("Outfit.Shoes") && this.styleService.IsSlotEquipped("Outfit.Socks"))
			{
				this.styleService.SetSlotEquipped("Outfit.Socks", false);
			}
		}

		// Token: 0x060036F5 RID: 14069 RVA: 0x0013D6E0 File Offset: 0x0013B8E0
		private AccessoryListPanelController.CardEntry FindEntry(string cardId)
		{
			return this._visibleCards.Find((AccessoryListPanelController.CardEntry entry) => entry != null && entry.cardId == cardId);
		}

		// Token: 0x060036F6 RID: 14070 RVA: 0x0013D711 File Offset: 0x0013B911
		private static string CreateCardId(string partId, int presetIndex)
		{
			return string.Format("{0}::{1}", partId, presetIndex);
		}

		// Token: 0x060036F7 RID: 14071 RVA: 0x0013D724 File Offset: 0x0013B924
		private void OnCollectionChanged()
		{
			if (!this.IsActive)
			{
				return;
			}
			if (string.Equals(this._currentGroup, "Special Events", StringComparison.Ordinal))
			{
				this.ShowForGroup(this._currentGroup);
				return;
			}
			this.RebuildCards();
		}

		// Token: 0x060036F8 RID: 14072 RVA: 0x0013D755 File Offset: 0x0013B955
		private void OnRankChanged()
		{
			this.OnCollectionChanged();
		}

		// Token: 0x060036F9 RID: 14073 RVA: 0x0013D760 File Offset: 0x0013B960
		private static int ResolvePreviewSurfaceIndex(MCStylePresetSO preset)
		{
			if (preset == null)
			{
				return -1;
			}
			int num = CustomizationWardrobeDiscoveryPolicy.ResolveFirstDiscoveredSurfaceIndex(preset);
			if (num < 0)
			{
				return preset.ResolveDefaultSurfaceIndex();
			}
			return num;
		}

		// Token: 0x04003714 RID: 14100
		[Header("References")]
		[SerializeField]
		private GameObject panelRoot;

		// Token: 0x04003715 RID: 14101
		[SerializeField]
		private Transform cardContainer;

		// Token: 0x04003716 RID: 14102
		[SerializeField]
		private GameObject cardPrefab;

		// Token: 0x04003717 RID: 14103
		[Header("Empty State")]
		[SerializeField]
		private GameObject emptyStateRoot;

		// Token: 0x04003718 RID: 14104
		[SerializeField]
		private TMP_Text emptyStateTitleLabel;

		// Token: 0x04003719 RID: 14105
		[SerializeField]
		private TMP_Text emptyStateLabel;

		// Token: 0x0400371A RID: 14106
		[SerializeField]
		private string emptyStateTitle = "Nothing unlocked here yet";

		// Token: 0x0400371B RID: 14107
		[SerializeField]
		private string emptyStateMessage = "Find new clothes while exploring to add them to your wardrobe.";

		// Token: 0x0400371C RID: 14108
		[Header("Data")]
		[SerializeField]
		private MCStyleCustomizationService styleService;

		// Token: 0x0400371D RID: 14109
		[SerializeField]
		private List<AccessoryListPanelController.AccessoryEntry> accessories = new List<AccessoryListPanelController.AccessoryEntry>();

		// Token: 0x0400371E RID: 14110
		private readonly List<AccessoryCardUI> _cards = new List<AccessoryCardUI>();

		// Token: 0x0400371F RID: 14111
		private readonly List<AccessoryListPanelController.CardEntry> _visibleCards = new List<AccessoryListPanelController.CardEntry>();

		// Token: 0x04003720 RID: 14112
		private string _currentGroup = string.Empty;

		// Token: 0x0200080C RID: 2060
		[Serializable]
		public class AccessoryEntry
		{
			// Token: 0x04003721 RID: 14113
			public string partId;

			// Token: 0x04003722 RID: 14114
			public string displayNameOverride;
		}

		// Token: 0x0200080D RID: 2061
		private sealed class CardEntry
		{
			// Token: 0x04003723 RID: 14115
			public string cardId;

			// Token: 0x04003724 RID: 14116
			public string partId;

			// Token: 0x04003725 RID: 14117
			public int presetIndex;

			// Token: 0x04003726 RID: 14118
			public MCStylePresetSO preset;

			// Token: 0x04003727 RID: 14119
			public string displayName;

			// Token: 0x04003728 RID: 14120
			public bool groupsPhysicalPresets;

			// Token: 0x04003729 RID: 14121
			public List<int> groupedPresetIndices;
		}
	}
}
