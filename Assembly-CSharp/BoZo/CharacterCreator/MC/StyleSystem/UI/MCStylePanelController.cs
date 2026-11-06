using System;
using System.Collections.Generic;
using Features.CensorBars;
using Features.Localization;
using Features.MainMenu;
using Features.Rank;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace BoZo.CharacterCreator.MC.StyleSystem.UI
{
	// Token: 0x02000812 RID: 2066
	public class MCStylePanelController : MonoBehaviour
	{
		// Token: 0x06003712 RID: 14098 RVA: 0x0013DD10 File Offset: 0x0013BF10
		private void Awake()
		{
			if (this.styleService != null)
			{
				this.styleService.OnActiveStateChanged += this.OnServiceStateChanged;
			}
		}

		// Token: 0x06003713 RID: 14099 RVA: 0x0013DD37 File Offset: 0x0013BF37
		private void OnEnable()
		{
			CollectionUnlockRegistry.Changed += this.OnCollectionChanged;
			RankProgressionService.Changed += this.OnRankChanged;
		}

		// Token: 0x06003714 RID: 14100 RVA: 0x0013DD5B File Offset: 0x0013BF5B
		private void OnDisable()
		{
			CollectionUnlockRegistry.Changed -= this.OnCollectionChanged;
			RankProgressionService.Changed -= this.OnRankChanged;
		}

		// Token: 0x06003715 RID: 14101 RVA: 0x0013DD7F File Offset: 0x0013BF7F
		private void OnDestroy()
		{
			if (this.styleService != null)
			{
				this.styleService.OnActiveStateChanged -= this.OnServiceStateChanged;
			}
		}

		// Token: 0x06003716 RID: 14102 RVA: 0x0013DDA8 File Offset: 0x0013BFA8
		public bool ShowForPart(MCStylePartDefinitionSO definition)
		{
			if (definition == null || this.styleService == null)
			{
				this.Hide();
				return false;
			}
			this._currentDefinition = definition;
			this.styleService.SetCustomizationPreview(true);
			if (!this.styleService.SetActivePart(definition))
			{
				this.Hide();
				return false;
			}
			if (this.panelRoot != null)
			{
				this.panelRoot.SetActive(true);
			}
			this.RebuildCardGrid();
			this.RefreshCustomizationPanel();
			return true;
		}

		// Token: 0x06003717 RID: 14103 RVA: 0x0013DE24 File Offset: 0x0013C024
		public void Hide()
		{
			this._currentDefinition = null;
			if (this.customizationPanel != null)
			{
				this.customizationPanel.SetService(null);
			}
			if (this.panelRoot != null)
			{
				this.panelRoot.SetActive(false);
			}
			this.ClearCards();
			this.SetEmptyStateVisible(false);
		}

		// Token: 0x170009DE RID: 2526
		// (get) Token: 0x06003718 RID: 14104 RVA: 0x0013DE79 File Offset: 0x0013C079
		public bool IsActive
		{
			get
			{
				return this._currentDefinition != null && this.panelRoot != null && this.panelRoot.activeSelf;
			}
		}

		// Token: 0x06003719 RID: 14105 RVA: 0x0013DEA4 File Offset: 0x0013C0A4
		public void SetCompactCardPrefab(GameObject prefab)
		{
			this.compactCardPrefab = prefab;
		}

		// Token: 0x170009DF RID: 2527
		// (get) Token: 0x0600371A RID: 14106 RVA: 0x0013DEAD File Offset: 0x0013C0AD
		private bool UsesCompactCards
		{
			get
			{
				return this._currentDefinition != null && !this._currentDefinition.isBodyAppearance && this.compactCardPrefab != null;
			}
		}

		// Token: 0x0600371B RID: 14107 RVA: 0x0013DED8 File Offset: 0x0013C0D8
		private void RebuildCardGrid()
		{
			this.ClearCards();
			if (this.styleService == null || this._currentDefinition == null || this.cardContainer == null)
			{
				this.SetEmptyStateVisible(true);
				return;
			}
			bool usesCompactCards = this.UsesCompactCards;
			if ((usesCompactCards && this.compactCardPrefab == null) || (!usesCompactCards && this.cardPrefab == null))
			{
				this.SetEmptyStateVisible(true);
				return;
			}
			int num = (this._currentDefinition.allowNoSelection ? 1 : 0);
			int selectedStyleUiIndex = this.styleService.GetSelectedStyleUiIndex();
			int num2 = (this.styleService.IsSlotEquipped(this._currentDefinition.partId) ? (selectedStyleUiIndex - num) : (-1));
			int num3 = -1;
			if (this._currentDefinition.stylePresets != null)
			{
				for (int i = 0; i < this._currentDefinition.stylePresets.Count; i++)
				{
					MCStylePresetSO mcstylePresetSO = this._currentDefinition.stylePresets[i];
					if (!(mcstylePresetSO == null) && !mcstylePresetSO.isSpecialEventClothing && CustomizationWardrobeDiscoveryPolicy.Evaluate(mcstylePresetSO).IsDiscovered && (!RankWardrobePolicy.IsRestricted || !RankWardrobePolicy.IsWardrobePart(this._currentDefinition.partId) || RankWardrobePolicy.IsPresetUnlocked(mcstylePresetSO)))
					{
						string text = (string.IsNullOrWhiteSpace(mcstylePresetSO.displayName) ? mcstylePresetSO.name : mcstylePresetSO.displayName);
						if (usesCompactCards)
						{
							GameObject gameObject = Object.Instantiate<GameObject>(this.compactCardPrefab, this.cardContainer);
							gameObject.name = "StyleCard_" + this._currentDefinition.partId + "_" + mcstylePresetSO.presetId;
							AccessoryCardUI accessoryCardUI = gameObject.GetComponent<AccessoryCardUI>() ?? gameObject.AddComponent<AccessoryCardUI>();
							accessoryCardUI.Setup(i.ToString(), text, i == num2, CustomizationWardrobeDiscoveryPolicy.GetDiscoveredSurfaceIndices(mcstylePresetSO).Count, new Action<string, bool>(this.OnCompactCardToggled), new Action<string, int>(this.OnCompactSurfaceCycled));
							this.UpdateCompactCardVisual(accessoryCardUI, i);
							this._compactCards.Add(accessoryCardUI);
						}
						else
						{
							int count = this._spawnedCards.Count;
							MCStyleCardUI mcstyleCardUI = Object.Instantiate<MCStyleCardUI>(this.cardPrefab, this.cardContainer);
							mcstyleCardUI.gameObject.SetActive(true);
							mcstyleCardUI.Configure(count, text, mcstylePresetSO.previewIcon, StreamerCensorImage.IsSensitiveStyle(this._currentDefinition.partId));
							mcstyleCardUI.OnCardClicked.AddListener(new UnityAction<int>(this.OnCardClicked));
							this._presetIndices.Add(i);
							this._spawnedCards.Add(mcstyleCardUI);
							if (i == num2)
							{
								num3 = count;
							}
						}
					}
				}
			}
			if (!usesCompactCards && num3 >= 0)
			{
				this._spawnedCards[num3].SetSelected(true);
			}
			this._selectedCardIndex = num3;
			this.SetEmptyStateVisible(this._spawnedCards.Count == 0 && this._compactCards.Count == 0);
		}

		// Token: 0x0600371C RID: 14108 RVA: 0x0013E1C4 File Offset: 0x0013C3C4
		private void ClearCards()
		{
			for (int i = 0; i < this._spawnedCards.Count; i++)
			{
				if (this._spawnedCards[i] != null)
				{
					this._spawnedCards[i].OnCardClicked.RemoveListener(new UnityAction<int>(this.OnCardClicked));
					Object.Destroy(this._spawnedCards[i].gameObject);
				}
			}
			for (int j = 0; j < this._compactCards.Count; j++)
			{
				if (this._compactCards[j] != null)
				{
					Object.Destroy(this._compactCards[j].gameObject);
				}
			}
			this._spawnedCards.Clear();
			this._compactCards.Clear();
			this._presetIndices.Clear();
			this._selectedCardIndex = -1;
		}

		// Token: 0x0600371D RID: 14109 RVA: 0x0013E29C File Offset: 0x0013C49C
		private void SetEmptyStateVisible(bool visible)
		{
			this.EnsureEmptyState();
			if (this.emptyStateTitleLabel != null)
			{
				LocalizedGameText localizedGameText = LocalizedGameText.BindSource(this.emptyStateTitleLabel, "cielos_apps", this.emptyStateTitle, null);
				if (localizedGameText != null)
				{
					localizedGameText.FitTranslationToRectangle(false);
				}
			}
			if (this.emptyStateLabel != null)
			{
				LocalizedGameText localizedGameText2 = LocalizedGameText.BindSource(this.emptyStateLabel, "cielos_apps", this.emptyStateMessage, null);
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

		// Token: 0x0600371E RID: 14110 RVA: 0x0013E32B File Offset: 0x0013C52B
		private void EnsureEmptyState()
		{
			if (this.emptyStateRoot != null || this.cardContainer == null)
			{
				return;
			}
			this.emptyStateRoot = WardrobeEmptyStateFactory.Create(this.cardContainer, out this.emptyStateTitleLabel, out this.emptyStateLabel);
		}

		// Token: 0x0600371F RID: 14111 RVA: 0x0013E368 File Offset: 0x0013C568
		private void OnCardClicked(int cardIndex)
		{
			if (this.styleService == null || cardIndex < 0 || cardIndex >= this._presetIndices.Count || this._currentDefinition == null)
			{
				return;
			}
			if (this._selectedCardIndex == cardIndex && this.styleService.IsSlotEquipped(this._currentDefinition.partId))
			{
				if (!this.styleService.SetSlotEquipped(this._currentDefinition.partId, false))
				{
					return;
				}
				this.UpdateCardSelection(-1);
				this.RefreshCustomizationPanel();
				return;
			}
			else
			{
				int num = this._presetIndices[cardIndex];
				int num2 = (this._currentDefinition.allowNoSelection ? 1 : 0);
				if (!this.styleService.SelectStyle(num + num2))
				{
					return;
				}
				MCStylePresetSO mcstylePresetSO = this._currentDefinition.stylePresets[num];
				int num3 = CustomizationWardrobeDiscoveryPolicy.ResolvePreferredDiscoveredSurfaceIndex(mcstylePresetSO, mcstylePresetSO.ResolveDefaultSurfaceIndex());
				if (num3 >= 0)
				{
					this.styleService.SelectSurface(num3);
				}
				this.UpdateCardSelection(cardIndex);
				this.RefreshCustomizationPanel();
				return;
			}
		}

		// Token: 0x06003720 RID: 14112 RVA: 0x0013E458 File Offset: 0x0013C658
		private void UpdateCardSelection(int newIndex)
		{
			if (this._selectedCardIndex >= 0 && this._selectedCardIndex < this._spawnedCards.Count)
			{
				this._spawnedCards[this._selectedCardIndex].SetSelected(false);
			}
			this._selectedCardIndex = newIndex;
			if (this._selectedCardIndex >= 0 && this._selectedCardIndex < this._spawnedCards.Count)
			{
				this._spawnedCards[this._selectedCardIndex].SetSelected(true);
			}
		}

		// Token: 0x06003721 RID: 14113 RVA: 0x0013E4D4 File Offset: 0x0013C6D4
		private void RefreshCustomizationPanel()
		{
			if (this.customizationPanel == null)
			{
				return;
			}
			if (this.UsesCompactCards)
			{
				this.customizationPanel.SetService(null);
				this.customizationPanel.gameObject.SetActive(false);
				return;
			}
			this.customizationPanel.SetService(this.styleService);
			if (this.customizationPanel.HasCustomizableOptions())
			{
				this.customizationPanel.gameObject.SetActive(true);
				this.customizationPanel.Reopen();
				return;
			}
			this.customizationPanel.Refresh();
		}

		// Token: 0x06003722 RID: 14114 RVA: 0x0013E55C File Offset: 0x0013C75C
		private void OnServiceStateChanged()
		{
			if (this._currentDefinition == null)
			{
				return;
			}
			if (this.UsesCompactCards)
			{
				this.RefreshCompactCards();
				return;
			}
			int num = (this._currentDefinition.allowNoSelection ? 1 : 0);
			int selectedStyleUiIndex = this.styleService.GetSelectedStyleUiIndex();
			int num2 = (this.styleService.IsSlotEquipped(this._currentDefinition.partId) ? (selectedStyleUiIndex - num) : (-1));
			this.UpdateCardSelection(this._presetIndices.IndexOf(num2));
		}

		// Token: 0x06003723 RID: 14115 RVA: 0x0013E5D8 File Offset: 0x0013C7D8
		private void OnCompactCardToggled(string cardId, bool newState)
		{
			int num;
			if (!this.TryGetCompactPresetIndex(cardId, out num))
			{
				this.RefreshCompactCards();
				return;
			}
			if (!newState)
			{
				if (this.GetSelectedPresetIndex() == num)
				{
					this.styleService.SetSlotEquipped(this._currentDefinition.partId, false);
				}
			}
			else
			{
				this.SelectCompactPreset(num);
			}
			this.RefreshCompactCards();
		}

		// Token: 0x06003724 RID: 14116 RVA: 0x0013E62C File Offset: 0x0013C82C
		private void OnCompactSurfaceCycled(string cardId, int direction)
		{
			int num;
			if (!this.TryGetCompactPresetIndex(cardId, out num))
			{
				return;
			}
			List<int> discoveredSurfaceIndices = CustomizationWardrobeDiscoveryPolicy.GetDiscoveredSurfaceIndices(this._currentDefinition.stylePresets[num]);
			if (discoveredSurfaceIndices.Count <= 1)
			{
				return;
			}
			if (this.GetSelectedPresetIndex() != num && !this.SelectCompactPreset(num))
			{
				return;
			}
			int slotSurfaceIndex = this.styleService.GetSlotSurfaceIndex(this._currentDefinition.partId);
			int num2 = discoveredSurfaceIndices.IndexOf(slotSurfaceIndex);
			if (num2 < 0)
			{
				num2 = 0;
			}
			int num3 = ((num2 + direction) % discoveredSurfaceIndices.Count + discoveredSurfaceIndices.Count) % discoveredSurfaceIndices.Count;
			this.styleService.SelectSurfaceForSlot(this._currentDefinition.partId, discoveredSurfaceIndices[num3]);
			this.RefreshCompactCards();
		}

		// Token: 0x06003725 RID: 14117 RVA: 0x0013E6E0 File Offset: 0x0013C8E0
		private bool SelectCompactPreset(int presetIndex)
		{
			if (this._currentDefinition == null || this._currentDefinition.stylePresets == null || presetIndex < 0 || presetIndex >= this._currentDefinition.stylePresets.Count)
			{
				return false;
			}
			int num = (this._currentDefinition.allowNoSelection ? 1 : 0);
			if (!this.styleService.SelectStyleForSlot(this._currentDefinition.partId, presetIndex + num))
			{
				return false;
			}
			MCStylePresetSO mcstylePresetSO = this._currentDefinition.stylePresets[presetIndex];
			int num2 = CustomizationWardrobeDiscoveryPolicy.ResolvePreferredDiscoveredSurfaceIndex(mcstylePresetSO, mcstylePresetSO.ResolveDefaultSurfaceIndex());
			if (num2 >= 0)
			{
				this.styleService.SelectSurfaceForSlot(this._currentDefinition.partId, num2);
			}
			return true;
		}

		// Token: 0x06003726 RID: 14118 RVA: 0x0013E78C File Offset: 0x0013C98C
		private int GetSelectedPresetIndex()
		{
			if (this._currentDefinition == null || !this.styleService.IsSlotEquipped(this._currentDefinition.partId))
			{
				return -1;
			}
			int num = (this._currentDefinition.allowNoSelection ? 1 : 0);
			return this.styleService.GetSelectedStyleUiIndex() - num;
		}

		// Token: 0x06003727 RID: 14119 RVA: 0x0013E7E0 File Offset: 0x0013C9E0
		private bool TryGetCompactPresetIndex(string cardId, out int presetIndex)
		{
			return int.TryParse(cardId, out presetIndex) && this._currentDefinition != null && this._currentDefinition.stylePresets != null && presetIndex >= 0 && presetIndex < this._currentDefinition.stylePresets.Count;
		}

		// Token: 0x06003728 RID: 14120 RVA: 0x0013E82C File Offset: 0x0013CA2C
		private void RefreshCompactCards()
		{
			for (int i = 0; i < this._compactCards.Count; i++)
			{
				AccessoryCardUI accessoryCardUI = this._compactCards[i];
				int num;
				if (!(accessoryCardUI == null) && this.TryGetCompactPresetIndex(accessoryCardUI.PartId, out num))
				{
					accessoryCardUI.SetActiveState(this.GetSelectedPresetIndex() == num);
					this.UpdateCompactCardVisual(accessoryCardUI, num);
				}
			}
		}

		// Token: 0x06003729 RID: 14121 RVA: 0x0013E88C File Offset: 0x0013CA8C
		private void UpdateCompactCardVisual(AccessoryCardUI card, int presetIndex)
		{
			if (card == null || this._currentDefinition == null || this._currentDefinition.stylePresets == null || presetIndex < 0 || presetIndex >= this._currentDefinition.stylePresets.Count)
			{
				return;
			}
			MCStylePresetSO mcstylePresetSO = this._currentDefinition.stylePresets[presetIndex];
			if (mcstylePresetSO == null)
			{
				return;
			}
			int num = ((this.GetSelectedPresetIndex() == presetIndex) ? this.styleService.GetSlotSurfaceIndex(this._currentDefinition.partId) : CustomizationWardrobeDiscoveryPolicy.ResolveFirstDiscoveredSurfaceIndex(mcstylePresetSO));
			if (num < 0)
			{
				num = mcstylePresetSO.ResolveDefaultSurfaceIndex();
			}
			Sprite sprite = mcstylePresetSO.previewIcon;
			MCSurfaceVariantSO mcsurfaceVariantSO;
			if (mcstylePresetSO.TryGetSurfaceVariant(num, out mcsurfaceVariantSO) && mcsurfaceVariantSO != null)
			{
				if (mcsurfaceVariantSO.previewIcon != null)
				{
					sprite = mcsurfaceVariantSO.previewIcon;
				}
				card.SetStyleLabel(string.IsNullOrWhiteSpace(mcsurfaceVariantSO.displayName) ? mcsurfaceVariantSO.name : mcsurfaceVariantSO.displayName);
			}
			else
			{
				card.SetStyleLabel(string.IsNullOrWhiteSpace(mcstylePresetSO.displayName) ? mcstylePresetSO.name : mcstylePresetSO.displayName);
			}
			card.SetPreview(sprite, StreamerCensorImage.IsSensitiveStyle(this._currentDefinition.partId));
		}

		// Token: 0x0600372A RID: 14122 RVA: 0x0013E9B1 File Offset: 0x0013CBB1
		private void OnCollectionChanged()
		{
			if (!this.IsActive)
			{
				return;
			}
			this.RebuildCardGrid();
			this.RefreshCustomizationPanel();
		}

		// Token: 0x0600372B RID: 14123 RVA: 0x0013E9C8 File Offset: 0x0013CBC8
		private void OnRankChanged()
		{
			if (this.styleService != null && this.IsActive)
			{
				this.styleService.EnforceCustomizationWardrobe();
			}
			this.OnCollectionChanged();
		}

		// Token: 0x0400373B RID: 14139
		[Header("Dependencies")]
		[SerializeField]
		private MCStyleCustomizationService styleService;

		// Token: 0x0400373C RID: 14140
		[Header("Card Grid")]
		[Tooltip("Parent transform where style cards are spawned")]
		[SerializeField]
		private Transform cardContainer;

		// Token: 0x0400373D RID: 14141
		[Tooltip("Prefab for each style card (must have MCStyleCardUI + Button)")]
		[SerializeField]
		private MCStyleCardUI cardPrefab;

		// Token: 0x0400373E RID: 14142
		[Tooltip("Compact wardrobe card used by every clothing preset (same visual as Feet/Stuff/Misc)")]
		[SerializeField]
		private GameObject compactCardPrefab;

		// Token: 0x0400373F RID: 14143
		[Header("Customization Panel")]
		[Tooltip("Existing panel for surface/color customization")]
		[SerializeField]
		private MCStyleCustomizationPanelUI customizationPanel;

		// Token: 0x04003740 RID: 14144
		[Header("Panel Root")]
		[Tooltip("Root GO to toggle the entire style panel visibility")]
		[SerializeField]
		private GameObject panelRoot;

		// Token: 0x04003741 RID: 14145
		[Header("Empty State")]
		[SerializeField]
		private GameObject emptyStateRoot;

		// Token: 0x04003742 RID: 14146
		[SerializeField]
		private TMP_Text emptyStateTitleLabel;

		// Token: 0x04003743 RID: 14147
		[SerializeField]
		private TMP_Text emptyStateLabel;

		// Token: 0x04003744 RID: 14148
		[SerializeField]
		private string emptyStateTitle = "Nothing unlocked here yet";

		// Token: 0x04003745 RID: 14149
		[SerializeField]
		private string emptyStateMessage = "Find new clothes while exploring to add them to your wardrobe.";

		// Token: 0x04003746 RID: 14150
		private readonly List<MCStyleCardUI> _spawnedCards = new List<MCStyleCardUI>();

		// Token: 0x04003747 RID: 14151
		private readonly List<AccessoryCardUI> _compactCards = new List<AccessoryCardUI>();

		// Token: 0x04003748 RID: 14152
		private readonly List<int> _presetIndices = new List<int>();

		// Token: 0x04003749 RID: 14153
		private MCStylePartDefinitionSO _currentDefinition;

		// Token: 0x0400374A RID: 14154
		private int _selectedCardIndex = -1;
	}
}
