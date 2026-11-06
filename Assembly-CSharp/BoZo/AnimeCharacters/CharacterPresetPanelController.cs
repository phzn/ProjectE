using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Features.Localization;
using Features.NPC;
using Michsky.MUIP;
using ProjectExhibitionist.CharacterCustomization.Core;
using ProjectExhibitionist.CharacterCustomization.Core.Presets;
using ProjectExhibitionist.CharacterCustomization.Core.Runtime;
using ProjectExhibitionist.CharacterCustomization.Core.Storage;
using ProjectExhibitionist.Core;
using ProjectExhibitionist.UI;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Bozo.AnimeCharacters
{
	// Token: 0x02000867 RID: 2151
	public class CharacterPresetPanelController : MonoBehaviour
	{
		// Token: 0x17000A00 RID: 2560
		// (get) Token: 0x0600394D RID: 14669 RVA: 0x0014F124 File Offset: 0x0014D324
		// (set) Token: 0x0600394E RID: 14670 RVA: 0x0014F12C File Offset: 0x0014D32C
		public bool IsBuildingGrid { get; private set; }

		// Token: 0x0600394F RID: 14671 RVA: 0x0014F138 File Offset: 0x0014D338
		private void Awake()
		{
			this.ApplyScrollSensitivity();
			this.ConfigureHoldProgressInputPassthrough();
			this.BindCreateButtons();
			this.BindConfirmModalEvents();
			if (this.duplicatePresetButton != null)
			{
				this.duplicatePresetButton.onClick.AddListener(new UnityAction(this.HandleDuplicatePreset));
			}
			if (this.deletePresetButton != null)
			{
				this.deletePresetButton.onClick.AddListener(new UnityAction(this.HandleDeletePreset));
			}
			if (this.refreshButton != null)
			{
				this.refreshButton.onClick.AddListener(new UnityAction(this.HandleRefresh));
			}
		}

		// Token: 0x06003950 RID: 14672 RVA: 0x0014F1DC File Offset: 0x0014D3DC
		private void BindCreateButtons()
		{
			if (this.createPresetButton == null && this.createSystemPresetButton == null)
			{
				return;
			}
			bool flag = this.createPresetButton != null && (this.createButtonOpensDraftFlow || this.createSystemPresetButton == null);
			if (this.createPresetButton != null)
			{
				this.createPresetButton.onClick.RemoveListener(new UnityAction(this.HandleCreatePreset));
				this.createPresetButton.onClick.RemoveListener(new UnityAction(this.HandleOpenNewPresetDraft));
				this.createPresetButton.onClick.AddListener(flag ? new UnityAction(this.HandleOpenNewPresetDraft) : new UnityAction(this.HandleCreatePreset));
			}
			if (this.createSystemPresetButton != null)
			{
				this.createSystemPresetButton.onClick.RemoveListener(new UnityAction(this.HandleOpenNewPresetDraft));
				this.createSystemPresetButton.onClick.AddListener(new UnityAction(this.HandleOpenNewPresetDraft));
			}
			if (flag)
			{
				this.LogDraftFlow("BindCreateButtons using createPresetButton as draft trigger.");
			}
		}

		// Token: 0x06003951 RID: 14673 RVA: 0x0014F2F8 File Offset: 0x0014D4F8
		private void BindConfirmModalEvents()
		{
			if (this._confirmModalEventsBound || this.presetActionConfirmModal == null)
			{
				return;
			}
			this.presetActionConfirmModal.onConfirm.AddListener(new UnityAction(this.HandleConfirmModalConfirmed));
			this.presetActionConfirmModal.onCancel.AddListener(new UnityAction(this.ClearPendingPresetAction));
			this._confirmModalEventsBound = true;
		}

		// Token: 0x06003952 RID: 14674 RVA: 0x0014F35C File Offset: 0x0014D55C
		private void OnEnable()
		{
			NpcRosterPreferences.Changed += this.HandleNpcRosterChanged;
			if (this.charactersManager == null)
			{
				this.charactersManager = Object.FindObjectOfType<CharactersManager>();
			}
			if (this.navigationRouter == null)
			{
				this.navigationRouter = Object.FindObjectOfType<CustomizationNavigationRouter>(true);
			}
			if (this.previewStateManager == null)
			{
				this.previewStateManager = CharacterStateManager.Instance;
			}
			this.BindScrollListener();
			if (this.hubPresetListController != null)
			{
				this.hubPresetListController.OnPresetsLoaded += this.HandleHubPresetsLoaded;
			}
			else if (this.charactersManager != null)
			{
				CharactersManager charactersManager = this.charactersManager;
				charactersManager.OnPresetListUpdated = (Action<string, CustomizationTargetType, List<CustomizationPresetData>>)Delegate.Combine(charactersManager.OnPresetListUpdated, new Action<string, CustomizationTargetType, List<CustomizationPresetData>>(this.HandlePresetListUpdated));
			}
			if (this.charactersManager != null)
			{
				CharactersManager charactersManager2 = this.charactersManager;
				charactersManager2.OnSelectedCharacterChanged = (Action<string>)Delegate.Combine(charactersManager2.OnSelectedCharacterChanged, new Action<string>(this.HandleSelectedCharacterChanged));
			}
			this.HandleRefresh();
		}

		// Token: 0x06003953 RID: 14675 RVA: 0x0014F463 File Offset: 0x0014D663
		private void Update()
		{
			this.UpdateHoldToSetInUse();
			if (this._instructionsController != ControllerInput.IsActive || this._instructionsGlyphVersion != ControllerGlyphs.Version)
			{
				this.RefreshPresetInstructions();
			}
		}

		// Token: 0x06003954 RID: 14676 RVA: 0x0014F48B File Offset: 0x0014D68B
		private void LateUpdate()
		{
			this.UpdatePresetInstructionsLayout();
			this.ApplyNpcRosterLayout();
		}

		// Token: 0x06003955 RID: 14677 RVA: 0x0014F49C File Offset: 0x0014D69C
		private void OnDisable()
		{
			NpcRosterPreferences.Changed -= this.HandleNpcRosterChanged;
			if (this.hubPresetListController != null)
			{
				this.hubPresetListController.OnPresetsLoaded -= this.HandleHubPresetsLoaded;
			}
			if (this.charactersManager != null)
			{
				CharactersManager charactersManager = this.charactersManager;
				charactersManager.OnPresetListUpdated = (Action<string, CustomizationTargetType, List<CustomizationPresetData>>)Delegate.Remove(charactersManager.OnPresetListUpdated, new Action<string, CustomizationTargetType, List<CustomizationPresetData>>(this.HandlePresetListUpdated));
				CharactersManager charactersManager2 = this.charactersManager;
				charactersManager2.OnSelectedCharacterChanged = (Action<string>)Delegate.Remove(charactersManager2.OnSelectedCharacterChanged, new Action<string>(this.HandleSelectedCharacterChanged));
			}
			this.CancelHoldToSetInUse();
			this.ClearPendingPresetAction();
			this._suppressNextPresetClick = false;
			this._suppressNextPointerDrivenClick = false;
			this.UnbindScrollListener();
			if (this._gridBuildCoroutine != null)
			{
				base.StopCoroutine(this._gridBuildCoroutine);
				this._gridBuildCoroutine = null;
			}
			this.IsBuildingGrid = false;
		}

		// Token: 0x06003956 RID: 14678 RVA: 0x0014F57D File Offset: 0x0014D77D
		public void BindCharacter(string characterId)
		{
			this._currentCharacterId = characterId;
			this._selectedPresetId = null;
			this.RequestRefresh();
		}

		// Token: 0x06003957 RID: 14679 RVA: 0x0014F594 File Offset: 0x0014D794
		private void HandleRefresh()
		{
			if (this.charactersManager == null)
			{
				return;
			}
			string selectedCharacterId = this.charactersManager.GetSelectedCharacterId();
			if (!string.IsNullOrWhiteSpace(selectedCharacterId))
			{
				this._currentCharacterId = selectedCharacterId;
			}
			this.RequestRefresh();
		}

		// Token: 0x06003958 RID: 14680 RVA: 0x0014F5D4 File Offset: 0x0014D7D4
		private void RequestRefresh()
		{
			if (this.charactersManager == null || string.IsNullOrWhiteSpace(this._currentCharacterId))
			{
				this.ClearGrid();
				this._currentPresets.Clear();
				this._selectedPresetId = null;
				this.RefreshActionStates();
				this.UpdateTargetLabel();
				this.UpdateSelectedPresetLabel();
				return;
			}
			this.charactersManager.RefreshPresetsForCharacter(this._currentCharacterId);
		}

		// Token: 0x06003959 RID: 14681 RVA: 0x0014F638 File Offset: 0x0014D838
		private void HandlePresetListUpdated(string characterId, CustomizationTargetType targetType, List<CustomizationPresetData> presets)
		{
			if (string.IsNullOrWhiteSpace(this._currentCharacterId))
			{
				return;
			}
			if (this._currentCharacterId != characterId)
			{
				return;
			}
			this._currentTargetType = targetType;
			this._currentPresets.Clear();
			if (presets != null)
			{
				this._currentPresets.AddRange(presets);
			}
			this.UpdateNpcFilterVisibility();
			this.RebuildFilteredView();
			this.EnsureSelectedPresetStillValid();
			this.RefreshSelectionVisualState();
			this.RefreshActionStates();
			this.UpdateTargetLabel();
			this.UpdateSelectedPresetLabel();
			this.ApplySelectedPresetPreview();
		}

		// Token: 0x0600395A RID: 14682 RVA: 0x0014F6B3 File Offset: 0x0014D8B3
		private void HandlePresetPointerDown(CustomizationPresetData preset, Vector2 _)
		{
			if (!this.CanStartHoldForPreset(preset))
			{
				return;
			}
			this.SetPreviewSelectionFromPointerDown(preset);
			this._isHoldingSetInUse = true;
			this._holdStartTimeUnscaled = Time.unscaledTime;
			this._holdPreset = preset;
			this._holdDelayPassed = false;
			this.HideHoldProgress();
		}

		// Token: 0x0600395B RID: 14683 RVA: 0x0014F6EC File Offset: 0x0014D8EC
		private void HandlePresetPointerUp(CustomizationPresetData preset, Vector2 _)
		{
			if (!this._isHoldingSetInUse)
			{
				return;
			}
			if (this._holdPreset != null && preset != null && this._holdPreset.presetId == preset.presetId)
			{
				if (this._holdDelayPassed)
				{
					this.TryPlayHoldCanceledCardFeedback(this._holdPreset);
				}
				this.CancelHoldToSetInUse();
			}
		}

		// Token: 0x0600395C RID: 14684 RVA: 0x0014F740 File Offset: 0x0014D940
		private void HandlePresetPointerExit(CustomizationPresetData preset, Vector2 _)
		{
			if (!this._isHoldingSetInUse)
			{
				return;
			}
			if (this._holdPreset != null && preset != null && this._holdPreset.presetId == preset.presetId)
			{
				if (this._holdDelayPassed)
				{
					this.TryPlayHoldCanceledCardFeedback(this._holdPreset);
				}
				this._suppressNextPointerDrivenClick = false;
				this.CancelHoldToSetInUse();
			}
		}

		// Token: 0x0600395D RID: 14685 RVA: 0x0014F79A File Offset: 0x0014D99A
		private void HandleSelectedCharacterChanged(string characterId)
		{
			if (string.IsNullOrWhiteSpace(characterId))
			{
				return;
			}
			this._currentCharacterId = characterId;
			this._selectedPresetId = null;
			this.RequestRefresh();
		}

		// Token: 0x0600395E RID: 14686 RVA: 0x0014F7BC File Offset: 0x0014D9BC
		private void RebuildGrid()
		{
			if (this._gridBuildCoroutine != null)
			{
				base.StopCoroutine(this._gridBuildCoroutine);
				this._gridBuildCoroutine = null;
			}
			this.ClearGrid();
			if (this.presetGridContainer == null || this.presetCardPrefab == null)
			{
				return;
			}
			this.ResetScrollToTop();
			this._gridBuildCoroutine = base.StartCoroutine(this.BuildInitialGridRoutine());
		}

		// Token: 0x0600395F RID: 14687 RVA: 0x0014F81F File Offset: 0x0014DA1F
		private int AppendNextPage()
		{
			return this.AppendCards(Mathf.Max(1, this.presetsPerPage));
		}

		// Token: 0x06003960 RID: 14688 RVA: 0x0014F834 File Offset: 0x0014DA34
		private int AppendCards(int count)
		{
			if (this.presetGridContainer == null || this.presetCardPrefab == null)
			{
				return 0;
			}
			int num = Mathf.Max(1, count);
			int num2 = Mathf.Min(this._renderedCount + num, this._filteredPresets.Count);
			int num3 = 0;
			for (int i = this._renderedCount; i < num2; i++)
			{
				CustomizationPresetData customizationPresetData = this._filteredPresets[i];
				if (customizationPresetData != null && !string.IsNullOrWhiteSpace(customizationPresetData.presetId))
				{
					CharacterCard characterCard = Object.Instantiate<CharacterCard>(this.presetCardPrefab, this.presetGridContainer);
					characterCard.gameObject.SetActive(true);
					characterCard.InitializePreset(customizationPresetData, new Action<CustomizationPresetData>(this.HandlePresetSingleClick), new Action<CustomizationPresetData>(this.HandlePresetDoubleClick), new Action<CustomizationPresetData, Vector2>(this.HandlePresetPointerDown), new Action<CustomizationPresetData, Vector2>(this.HandlePresetPointerUp), new Action<CustomizationPresetData, Vector2>(this.HandlePresetPointerExit), new Action<CustomizationPresetData, Vector2>(this.HandlePresetRightClick));
					this._spawnedCards[customizationPresetData.presetId] = characterCard;
					if (this._currentTargetType == CustomizationTargetType.Npc)
					{
						characterCard.ConfigureNpcRoster(NpcRosterPreferences.IsEnabled(customizationPresetData), NpcRosterPreferences.IsEnabled(customizationPresetData), new Action<CustomizationPresetData>(this.HandleNpcRosterToggle));
					}
					num3++;
				}
			}
			this._renderedCount = num2;
			if (num3 > 0)
			{
				this.RefreshSelectionVisualState();
			}
			return num3;
		}

		// Token: 0x06003961 RID: 14689 RVA: 0x0014F984 File Offset: 0x0014DB84
		private IEnumerator BuildInitialGridRoutine()
		{
			this.IsBuildingGrid = true;
			int initialTarget = Mathf.Min(this._filteredPresets.Count, Mathf.Max(1, this.presetsPerPage));
			int budget = Mathf.Max(1, this.cardsPerFrame);
			while (this._renderedCount < initialTarget)
			{
				this.AppendCards(Mathf.Min(budget, initialTarget - this._renderedCount));
				yield return null;
			}
			int safety = 64;
			while (this._renderedCount < this._filteredPresets.Count)
			{
				int num = safety;
				safety = num - 1;
				if (num <= 0)
				{
					break;
				}
				Canvas.ForceUpdateCanvases();
				if (this.presetScrollRect == null || this.presetScrollRect.content == null || this.presetScrollRect.viewport == null || this.presetScrollRect.content.rect.height > this.presetScrollRect.viewport.rect.height)
				{
					break;
				}
				this.AppendCards(budget);
				yield return null;
			}
			this.IsBuildingGrid = false;
			this._gridBuildCoroutine = null;
			yield break;
		}

		// Token: 0x06003962 RID: 14690 RVA: 0x0014F994 File Offset: 0x0014DB94
		private void ClearGrid()
		{
			this._spawnedCards.Clear();
			this._renderedCount = 0;
			if (this.presetGridContainer == null)
			{
				return;
			}
			for (int i = this.presetGridContainer.childCount - 1; i >= 0; i--)
			{
				Object.Destroy(this.presetGridContainer.GetChild(i).gameObject);
			}
		}

		// Token: 0x06003963 RID: 14691 RVA: 0x0014F9F0 File Offset: 0x0014DBF0
		private void HandleScrollChanged(Vector2 normalizedPosition)
		{
			if (this._renderedCount >= this._filteredPresets.Count)
			{
				return;
			}
			if (normalizedPosition.y <= this.infiniteScrollThreshold)
			{
				this.AppendNextPage();
			}
		}

		// Token: 0x06003964 RID: 14692 RVA: 0x0014FA1C File Offset: 0x0014DC1C
		private void EnsureViewportFilled()
		{
			if (this.presetScrollRect == null || this.presetScrollRect.content == null || this.presetScrollRect.viewport == null)
			{
				return;
			}
			int num = 64;
			while (this._renderedCount < this._filteredPresets.Count && num-- > 0)
			{
				Canvas.ForceUpdateCanvases();
				float height = this.presetScrollRect.content.rect.height;
				float height2 = this.presetScrollRect.viewport.rect.height;
				if (height > height2 || this.AppendNextPage() == 0)
				{
					break;
				}
			}
		}

		// Token: 0x06003965 RID: 14693 RVA: 0x0014FABF File Offset: 0x0014DCBF
		private void ResetScrollToTop()
		{
			if (this.presetScrollRect != null)
			{
				this.presetScrollRect.verticalNormalizedPosition = 1f;
			}
		}

		// Token: 0x06003966 RID: 14694 RVA: 0x0014FAE0 File Offset: 0x0014DCE0
		private void HandlePresetSingleClick(CustomizationPresetData preset)
		{
			if (preset == null)
			{
				return;
			}
			if (this._suppressNextPointerDrivenClick)
			{
				this._suppressNextPointerDrivenClick = false;
				return;
			}
			if (this._suppressNextPresetClick)
			{
				this._suppressNextPresetClick = false;
				return;
			}
			if (this.IsPresetAlreadySelected(preset))
			{
				return;
			}
			this._selectedPresetId = preset.presetId;
			if (this.hubPresetListController != null)
			{
				this.hubPresetListController.SelectPreset(this._selectedPresetId, true, false);
			}
			this.RefreshSelectionVisualState();
			this.RefreshActionStates();
			this.UpdateSelectedPresetLabel();
			this.ApplySelectedPresetPreview();
		}

		// Token: 0x06003967 RID: 14695 RVA: 0x0014FB64 File Offset: 0x0014DD64
		private void HandlePresetDoubleClick(CustomizationPresetData preset)
		{
			if (!this.CanEditPreset(preset))
			{
				return;
			}
			CharacterDataExtended characterDataExtended = ((this.charactersManager != null) ? this.charactersManager.GetCharacterById(this._currentCharacterId) : null);
			if (this._currentTargetType == CustomizationTargetType.MainCharacter)
			{
				bool flag = characterDataExtended != null && characterDataExtended.isMainCharacter;
				this.navigationRouter.OpenMainCharacterCustomization(this._currentCharacterId, flag, preset.presetId, false);
				return;
			}
			if (this._currentTargetType == CustomizationTargetType.Npc)
			{
				this.navigationRouter.OpenNpcCustomization(this._currentCharacterId, false, preset.presetId, false);
			}
		}

		// Token: 0x06003968 RID: 14696 RVA: 0x0014FBF0 File Offset: 0x0014DDF0
		private bool CanEditPreset(CustomizationPresetData preset)
		{
			return preset != null && !string.IsNullOrWhiteSpace(this._currentCharacterId) && this.navigationRouter != null && (this._currentTargetType == CustomizationTargetType.MainCharacter || this._currentTargetType == CustomizationTargetType.Npc) && (!preset.isSystemPreset || this._currentTargetType == CustomizationTargetType.Npc);
		}

		// Token: 0x06003969 RID: 14697 RVA: 0x0014FC44 File Offset: 0x0014DE44
		private void HandleCreatePreset()
		{
			string text;
			if (!this.TryGetInputName(out text))
			{
				ToastService.Show("Name the look before saving it.", "New look", null, -1f, "cielos_apps");
				return;
			}
			if (this.charactersManager == null || string.IsNullOrWhiteSpace(this._currentCharacterId))
			{
				return;
			}
			CustomizationPresetData customizationPresetData;
			string text2;
			if (this.charactersManager.TryCreatePresetForCharacter(this._currentCharacterId, text, out customizationPresetData, out text2, null, null))
			{
				this._selectedPresetId = ((customizationPresetData != null) ? customizationPresetData.presetId : this._selectedPresetId);
				ToastService.ShowFormat("cielos_apps", "'{0}' is ready in your wardrobe.", new object[] { text }, "Look saved", null, -1f);
				this.ClearInput();
				return;
			}
			ToastService.Show(string.IsNullOrWhiteSpace(text2) ? "Failed to save the preset." : text2, "Ops...", null, -1f, "cielos_apps");
		}

		// Token: 0x0600396A RID: 14698 RVA: 0x0014FD14 File Offset: 0x0014DF14
		private void HandleCreateSystemPreset()
		{
			string text;
			if (!this.TryGetInputName(out text))
			{
				ToastService.Show("Name the look before saving it.", "New look", null, -1f, "cielos_apps");
				return;
			}
			if (this.charactersManager == null || string.IsNullOrWhiteSpace(this._currentCharacterId))
			{
				return;
			}
			CustomizationPresetData customizationPresetData;
			if (this.charactersManager.TryCreateSystemPresetForCharacter(this._currentCharacterId, text, out customizationPresetData))
			{
				this._selectedPresetId = ((customizationPresetData != null) ? customizationPresetData.presetId : this._selectedPresetId);
				ToastService.ShowFormat("cielos_apps", "System preset '{0}' created.", new object[] { text }, "System look", null, -1f);
				this.ClearInput();
				return;
			}
			ToastService.Show("Could not create the system preset.", "Oops...", null, -1f, "cielos_apps");
		}

		// Token: 0x0600396B RID: 14699 RVA: 0x0014FDD4 File Offset: 0x0014DFD4
		private void HandleOpenNewPresetDraft()
		{
			if (this.navigationRouter == null)
			{
				ToastService.Show("Customization router not found in scene.", "Oops...", null, -1f, "cielos_apps");
				return;
			}
			CustomizationTargetType customizationTargetType = this.ResolveDraftTargetType();
			this.LogDraftFlow(string.Format("HandleOpenNewPresetDraft target={0} currentTarget={1} currentCharacterId='{2}'", customizationTargetType, this._currentTargetType, this._currentCharacterId));
			if (customizationTargetType == CustomizationTargetType.MainCharacter)
			{
				this.OpenMainCharacterNewPresetDraft();
				return;
			}
			if (customizationTargetType == CustomizationTargetType.Npc)
			{
				this.OpenNpcNewPresetDraft();
				return;
			}
			ToastService.Show("Choose Main Character or NPC first.", "Action blocked", null, -1f, "cielos_apps");
		}

		// Token: 0x0600396C RID: 14700 RVA: 0x0014FE68 File Offset: 0x0014E068
		private void OpenMainCharacterNewPresetDraft()
		{
			CustomizationSystemPresetConfig customizationSystemPresetConfig = ((this.hubPresetListController != null) ? this.hubPresetListController.SystemPresetConfig : null);
			CharacterObject characterObject = ((customizationSystemPresetConfig != null) ? customizationSystemPresetConfig.MainCharacterBasePreset : null);
			if (this.hubPresetListController == null || characterObject == null)
			{
				ToastService.Show("System default for Ciel is not configured.", "Oops...", null, -1f, "cielos_apps");
				return;
			}
			if (!this.hubPresetListController.TryApplySystemPresetAssetToPreview(characterObject, CustomizationTargetType.MainCharacter))
			{
				ToastService.Show("Could not load Ciel's system default.", "Oops...", null, -1f, "cielos_apps");
				return;
			}
			string text = this.ResolveMainCharacterId(customizationSystemPresetConfig);
			this.LogDraftFlow(string.Concat(new string[]
			{
				"OpenMainCharacterNewPresetDraft characterId='",
				text,
				"' baseAsset='",
				(characterObject != null) ? characterObject.name : "<null>",
				"'"
			}));
			base.StartCoroutine(this.OpenMainCharacterDraftWhenBaseReady(text));
		}

		// Token: 0x0600396D RID: 14701 RVA: 0x0014FF60 File Offset: 0x0014E160
		private void OpenNpcNewPresetDraft()
		{
			CharacterObject characterObject = this.npcNewPresetBaseAsset;
			if (characterObject == null && this.hubPresetListController != null && this.hubPresetListController.SystemPresetConfig != null)
			{
				characterObject = this.hubPresetListController.SystemPresetConfig.NpcNewPresetBaseAsset;
			}
			if (characterObject == null)
			{
				ToastService.Show("Assign an NPC base asset in CustomizationSystemPresetConfig (NpcNewPresetBaseAsset field).", "Setup required", null, -1f, "cielos_apps");
				return;
			}
			if (this.hubPresetListController == null)
			{
				ToastService.Show("Preset list controller not found.", "Oops...", null, -1f, "cielos_apps");
				return;
			}
			if (!this.hubPresetListController.TryApplySystemPresetAssetToPreview(characterObject, CustomizationTargetType.Npc))
			{
				ToastService.Show("Could not load the NPC system default.", "Oops...", null, -1f, "cielos_apps");
				return;
			}
			string text = this.ResolveNpcDraftCharacterId(characterObject);
			this.LogDraftFlow(string.Concat(new string[] { "OpenNpcNewPresetDraft characterId='", text, "' baseAsset='", characterObject.name, "'" }));
			base.StartCoroutine(this.OpenNpcDraftWhenBaseReady(text));
		}

		// Token: 0x0600396E RID: 14702 RVA: 0x00150072 File Offset: 0x0014E272
		private IEnumerator OpenMainCharacterDraftWhenBaseReady(string characterId)
		{
			yield return null;
			float timeoutAt = Time.unscaledTime + 15f;
			while (this.hubPresetListController != null && this.hubPresetListController.IsPreviewLoading && Time.unscaledTime < timeoutAt)
			{
				yield return null;
			}
			this.navigationRouter.OpenMainCharacterCustomization(characterId, true, string.Empty, true);
			yield break;
		}

		// Token: 0x0600396F RID: 14703 RVA: 0x00150088 File Offset: 0x0014E288
		private IEnumerator OpenNpcDraftWhenBaseReady(string characterId)
		{
			yield return null;
			float timeoutAt = Time.unscaledTime + 15f;
			while (this.hubPresetListController != null && this.hubPresetListController.IsPreviewLoading && Time.unscaledTime < timeoutAt)
			{
				yield return null;
			}
			this.navigationRouter.OpenNpcCustomization(characterId, false, string.Empty, true);
			yield break;
		}

		// Token: 0x06003970 RID: 14704 RVA: 0x0015009E File Offset: 0x0014E29E
		private CustomizationTargetType ResolveDraftTargetType()
		{
			if (this.hubPresetListController != null && this.hubPresetListController.CurrentTargetType != CustomizationTargetType.None)
			{
				return this.hubPresetListController.CurrentTargetType;
			}
			return this._currentTargetType;
		}

		// Token: 0x06003971 RID: 14705 RVA: 0x001500D0 File Offset: 0x0014E2D0
		private string ResolveMainCharacterId(CustomizationSystemPresetConfig systemConfig)
		{
			if (this.charactersManager != null)
			{
				CharacterDataExtended mainCharacter = this.charactersManager.GetMainCharacter();
				if (mainCharacter != null && !string.IsNullOrWhiteSpace(mainCharacter.characterId))
				{
					return mainCharacter.characterId;
				}
			}
			if (systemConfig != null && !string.IsNullOrWhiteSpace(systemConfig.MainCharacterId))
			{
				return systemConfig.MainCharacterId;
			}
			return "Ciel";
		}

		// Token: 0x06003972 RID: 14706 RVA: 0x00150130 File Offset: 0x0014E330
		private string ResolveNpcDraftCharacterId(CharacterObject baseAsset)
		{
			if (this.hubPresetListController != null)
			{
				CustomizationPresetData selectedPreset = this.hubPresetListController.SelectedPreset;
				if (selectedPreset != null && selectedPreset.targetType == CustomizationTargetType.Npc && !string.IsNullOrWhiteSpace(selectedPreset.characterId))
				{
					return selectedPreset.characterId;
				}
			}
			if (!string.IsNullOrWhiteSpace(this._currentCharacterId) && this.charactersManager != null)
			{
				CharacterDataExtended characterById = this.charactersManager.GetCharacterById(this._currentCharacterId);
				if (characterById != null && !characterById.isMainCharacter)
				{
					return characterById.characterId;
				}
			}
			if (!(baseAsset != null))
			{
				return string.Empty;
			}
			return baseAsset.name;
		}

		// Token: 0x06003973 RID: 14707 RVA: 0x001501CB File Offset: 0x0014E3CB
		private void LogDraftFlow(string message)
		{
			if (!this.verboseDraftFlowLogs)
			{
				return;
			}
			Debug.Log("[PresetDraftFlow] " + message);
		}

		// Token: 0x06003974 RID: 14708 RVA: 0x001501E8 File Offset: 0x0014E3E8
		private void HandleDuplicatePreset()
		{
			CustomizationPresetData customizationPresetData;
			if (!this.TryGetSelectedPreset(out customizationPresetData))
			{
				ToastService.Show("Select a look to clone first.", "Duplicate look", null, -1f, "cielos_apps");
				return;
			}
			string text;
			if (!this.TryGetInputName(out text))
			{
				text = customizationPresetData.presetName + " Copy";
			}
			if (this.charactersManager == null || string.IsNullOrWhiteSpace(this._currentCharacterId))
			{
				return;
			}
			text = this.ResolveUniquePresetName(text);
			this.ExecuteDuplicatePreset(customizationPresetData, text);
		}

		// Token: 0x06003975 RID: 14709 RVA: 0x00150264 File Offset: 0x0014E464
		private void ExecuteDuplicatePreset(CustomizationPresetData selectedPreset, string duplicatedName)
		{
			if (selectedPreset == null || string.IsNullOrWhiteSpace(duplicatedName))
			{
				return;
			}
			if (this.charactersManager == null || string.IsNullOrWhiteSpace(this._currentCharacterId))
			{
				return;
			}
			if (selectedPreset.isSystemPreset)
			{
				if (this._currentTargetType == CustomizationTargetType.Npc && this.hubPresetListController != null)
				{
					CustomizationPresetData customizationPresetData;
					string text;
					if (this.hubPresetListController.TryDuplicateSystemNpcToCustom(selectedPreset.presetId, duplicatedName, out customizationPresetData, out text))
					{
						this._selectedPresetId = ((customizationPresetData != null) ? customizationPresetData.presetId : this._selectedPresetId);
						this.ClearInput();
						ToastService.ShowFormat("cielos_apps", "'{0}' is born from a system NPC.", new object[] { duplicatedName }, "Clone created", null, -1f);
						return;
					}
					ToastService.Show(string.IsNullOrWhiteSpace(text) ? "Clone failed. Try again." : text, "Oops...", null, -1f, "cielos_apps");
					return;
				}
				else
				{
					CharacterStateData characterStateData = ((this.previewStateManager != null) ? this.previewStateManager.ConvertToCharacterStateData() : selectedPreset.stateData);
					string text2 = CharacterPresetPanelController.DuplicatePresetIconFile(selectedPreset.iconFileName, duplicatedName);
					CustomizationPresetData customizationPresetData2;
					string text3;
					if (this.charactersManager.TryCreatePresetForCharacter(this._currentCharacterId, duplicatedName, out customizationPresetData2, out text3, characterStateData, text2))
					{
						this._selectedPresetId = ((customizationPresetData2 != null) ? customizationPresetData2.presetId : this._selectedPresetId);
						this.ClearInput();
						ToastService.ShowFormat("cielos_apps", "'{0}' is born from a system look.", new object[] { duplicatedName }, "Clone created", null, -1f);
						return;
					}
					ToastService.Show("Clone failed. Try again in a moment.", "Oops...", null, -1f, "cielos_apps");
					return;
				}
			}
			else
			{
				CustomizationPresetData customizationPresetData3;
				if (this.charactersManager.TryDuplicatePresetForCharacter(this._currentCharacterId, selectedPreset.presetId, duplicatedName, out customizationPresetData3))
				{
					if (this._currentTargetType == CustomizationTargetType.Npc && customizationPresetData3 != null && !selectedPreset.isSystemPreset)
					{
						CharacterData characterData = NpcLookStore.LoadLook(selectedPreset.presetId);
						if (characterData != null)
						{
							NpcLookStore.Save(customizationPresetData3.presetId, characterData, duplicatedName, customizationPresetData3.iconFileName);
						}
					}
					this._selectedPresetId = ((customizationPresetData3 != null) ? customizationPresetData3.presetId : this._selectedPresetId);
					this.ClearInput();
					ToastService.ShowFormat("cielos_apps", "'{0}' has been duplicated.", new object[] { duplicatedName }, "Clone created", null, -1f);
					return;
				}
				ToastService.Show("Could not duplicate this look.", "Oops...", null, -1f, "cielos_apps");
				return;
			}
		}

		// Token: 0x06003976 RID: 14710 RVA: 0x0015049C File Offset: 0x0014E69C
		private void HandleDeletePreset()
		{
			this.HandleDeletePreset(null);
		}

		// Token: 0x06003977 RID: 14711 RVA: 0x001504A8 File Offset: 0x0014E6A8
		private void HandleDeletePreset(CustomizationPresetData explicitPreset)
		{
			CustomizationPresetData customizationPresetData = explicitPreset;
			if (customizationPresetData == null && !this.TryGetSelectedPreset(out customizationPresetData))
			{
				ToastService.Show("Select a look to delete first.", "Delete look", null, -1f, "cielos_apps");
				return;
			}
			if (this.charactersManager == null || string.IsNullOrWhiteSpace(this._currentCharacterId))
			{
				return;
			}
			if (!this.charactersManager.CanDeletePreset(customizationPresetData))
			{
				ToastService.Show("This look is protected. You can't delete it.", "Action blocked", null, -1f, "cielos_apps");
				return;
			}
			if (this.TryOpenDeleteConfirmModal(customizationPresetData))
			{
				return;
			}
			this.ExecuteDeletePreset(customizationPresetData);
		}

		// Token: 0x06003978 RID: 14712 RVA: 0x00150538 File Offset: 0x0014E738
		private void ExecuteDeletePreset(CustomizationPresetData selectedPreset)
		{
			if (selectedPreset == null)
			{
				return;
			}
			if (this.charactersManager == null || string.IsNullOrWhiteSpace(this._currentCharacterId))
			{
				return;
			}
			bool flag = this._currentTargetType == CustomizationTargetType.Npc && !selectedPreset.isSystemPreset;
			string text = (flag ? selectedPreset.presetId : null);
			if (this.charactersManager.TryDeletePresetForCharacter(this._currentCharacterId, selectedPreset.presetId))
			{
				if (flag && !string.IsNullOrWhiteSpace(text))
				{
					NpcLookStore.Delete(text);
					NpcRosterPreferences.Remove(text);
				}
				this._selectedPresetId = null;
				ToastService.ShowFormat("cielos_apps", "'{0}' is gone from the wardrobe.", new object[] { selectedPreset.presetName }, "Look deleted", null, -1f);
				return;
			}
			ToastService.Show("Deletion failed. Try again.", "Oops...", null, -1f, "cielos_apps");
		}

		// Token: 0x06003979 RID: 14713 RVA: 0x00150604 File Offset: 0x0014E804
		private bool TryOpenDeleteConfirmModal(CustomizationPresetData preset)
		{
			if (this.presetActionConfirmModal == null)
			{
				return false;
			}
			this._pendingPresetAction = CharacterPresetPanelController.PendingPresetAction.Delete;
			this._pendingActionPreset = preset;
			this.presetActionConfirmModal.titleText = this.deleteConfirmTitle;
			this.presetActionConfirmModal.descriptionText = string.Format(this.deleteConfirmDescriptionTemplate, (preset != null) ? preset.presetName : "preset");
			this.presetActionConfirmModal.showCancelButton = true;
			this.presetActionConfirmModal.showConfirmButton = true;
			this.presetActionConfirmModal.UpdateUI();
			LocalizedGameText localizedGameText = LocalizedGameText.BindSource(this.presetActionConfirmModal.windowTitle, "cielos_apps", this.deleteConfirmTitle, null);
			if (localizedGameText != null)
			{
				localizedGameText.FitTranslationToRectangle(false);
			}
			LocalizedGameText localizedGameText2 = LocalizedGameText.BindFormat(this.presetActionConfirmModal.windowDescription, "cielos_apps", this.deleteConfirmDescriptionTemplate, new object[] { (preset != null) ? preset.presetName : GameLocalization.GetSource("cielos_apps", "preset") });
			if (localizedGameText2 != null)
			{
				localizedGameText2.FitTranslationToRectangle(false);
			}
			this.presetActionConfirmModal.Open();
			return true;
		}

		// Token: 0x0600397A RID: 14714 RVA: 0x00150708 File Offset: 0x0014E908
		private void HandleConfirmModalConfirmed()
		{
			int pendingPresetAction = (int)this._pendingPresetAction;
			CustomizationPresetData pendingActionPreset = this._pendingActionPreset;
			this.ClearPendingPresetAction();
			if (pendingPresetAction == 1)
			{
				this.ExecuteDeletePreset(pendingActionPreset);
			}
		}

		// Token: 0x0600397B RID: 14715 RVA: 0x00150732 File Offset: 0x0014E932
		private void ClearPendingPresetAction()
		{
			this._pendingPresetAction = CharacterPresetPanelController.PendingPresetAction.None;
			this._pendingActionPreset = null;
		}

		// Token: 0x0600397C RID: 14716 RVA: 0x00150744 File Offset: 0x0014E944
		private void HandlePresetRightClick(CustomizationPresetData preset, Vector2 screenPosition)
		{
			this.CancelHoldToSetInUse();
			if (preset == null || this.presetContextMenuManager == null)
			{
				return;
			}
			this._contextMenuTargetPreset = preset;
			Transform transform = ((this.presetContextMenuManager.contextContent != null) ? this.presetContextMenuManager.contextContent.transform.Find("Item List") : null);
			if (transform == null)
			{
				Debug.LogWarning("[PresetPanel] Context menu Item List not found under contextContent.");
				return;
			}
			foreach (object obj in transform)
			{
				Transform transform2 = (Transform)obj;
				transform2.gameObject.SetActive(false);
				Object.Destroy(transform2.gameObject);
			}
			bool flag = this.charactersManager != null && this.charactersManager.CanDeletePreset(preset);
			if (this.presetContextMenuManager.contextButton != null)
			{
				if (this.CanEditPreset(preset))
				{
					this.BuildContextMenuItem(transform, "Edit", this.contextMenuEditIcon, new UnityAction(this.OnContextMenuEdit));
				}
				if (this.CanStartHoldForPreset(preset))
				{
					this.BuildContextMenuItem(transform, "Use", this.contextMenuUseIcon, new UnityAction(this.OnContextMenuUse));
				}
				this.BuildContextMenuItem(transform, this.contextMenuDuplicateLabel, this.contextMenuDuplicateIcon, new UnityAction(this.OnContextMenuDuplicate));
				if (flag)
				{
					this.BuildContextMenuItem(transform, this.contextMenuDeleteLabel, this.contextMenuDeleteIcon, new UnityAction(this.OnContextMenuDelete));
				}
			}
			if (ControllerInput.IsActive)
			{
				this.PositionControllerContextMenu(screenPosition);
			}
			else
			{
				this.presetContextMenuManager.SetContextMenuPosition();
			}
			this.presetContextMenuManager.Open();
			if (this.presetContextMenuManager.contextContent != null)
			{
				Button componentInChildren = transform.GetComponentInChildren<Button>();
				ControllerNavigationScope.Configure(this.presetContextMenuManager.contextContent, new Action(this.presetContextMenuManager.Close), (componentInChildren != null) ? componentInChildren.gameObject : null, 100, false);
			}
			if (this.presetContextMenuManager.contextContent != null)
			{
				LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)this.presetContextMenuManager.contextContent.transform);
			}
			Canvas.ForceUpdateCanvases();
		}

		// Token: 0x0600397D RID: 14717 RVA: 0x0015097C File Offset: 0x0014EB7C
		private void PositionControllerContextMenu(Vector2 screenPosition)
		{
			RectTransform rectTransform = this.presetContextMenuManager.transform as RectTransform;
			RectTransform rectTransform2 = ((rectTransform != null) ? (rectTransform.parent as RectTransform) : null);
			Canvas mainCanvas = this.presetContextMenuManager.mainCanvas;
			Camera camera = ((mainCanvas != null && mainCanvas.renderMode != RenderMode.ScreenSpaceOverlay) ? mainCanvas.worldCamera : null);
			Vector3 vector;
			if (rectTransform2 != null && RectTransformUtility.ScreenPointToWorldPointInRectangle(rectTransform2, screenPosition, camera, out vector))
			{
				rectTransform.position = vector;
			}
			RectTransform rectTransform3 = this.presetContextMenuManager.contextContent.transform as RectTransform;
			if (rectTransform3 == null)
			{
				return;
			}
			bool flag = screenPosition.x > (float)Screen.width * 0.5f;
			bool flag2 = screenPosition.y > (float)Screen.height * 0.5f;
			rectTransform3.pivot = new Vector2(flag ? 1f : 0f, flag2 ? 1f : 0f);
			rectTransform3.anchoredPosition = new Vector2(flag ? (-15f) : 15f, flag2 ? (-10f) : 10f);
		}

		// Token: 0x0600397E RID: 14718 RVA: 0x00150A9C File Offset: 0x0014EC9C
		private void OnContextMenuEdit()
		{
			CustomizationPresetData contextMenuTargetPreset = this._contextMenuTargetPreset;
			this._contextMenuTargetPreset = null;
			if (this.CanEditPreset(contextMenuTargetPreset))
			{
				this.HandlePresetDoubleClick(contextMenuTargetPreset);
			}
		}

		// Token: 0x0600397F RID: 14719 RVA: 0x00150AC8 File Offset: 0x0014ECC8
		private void OnContextMenuUse()
		{
			CustomizationPresetData contextMenuTargetPreset = this._contextMenuTargetPreset;
			this._contextMenuTargetPreset = null;
			if (!this.CanStartHoldForPreset(contextMenuTargetPreset))
			{
				return;
			}
			this._holdPreset = contextMenuTargetPreset;
			this.ConfirmHoldToSetInUse();
			this._suppressNextPresetClick = false;
		}

		// Token: 0x06003980 RID: 14720 RVA: 0x00150B04 File Offset: 0x0014ED04
		private void BuildContextMenuItem(Transform parent, string label, Sprite icon, UnityAction onClick)
		{
			GameObject gameObject = Object.Instantiate<GameObject>(this.presetContextMenuManager.contextButton, parent);
			gameObject.transform.localPosition = Vector3.zero;
			TextMeshProUGUI componentInChildren = gameObject.GetComponentInChildren<TextMeshProUGUI>();
			if (componentInChildren != null)
			{
				LocalizedGameText localizedGameText = LocalizedGameText.BindSource(componentInChildren, "cielos_apps", label, null);
				if (localizedGameText != null)
				{
					localizedGameText.FitTranslationToRectangle(false);
				}
			}
			Transform transform = gameObject.transform.Find("Icon");
			if (transform != null)
			{
				Image component = transform.GetComponent<Image>();
				if (component != null)
				{
					if (icon != null)
					{
						component.sprite = icon;
					}
					else
					{
						component.color = new Color(0f, 0f, 0f, 0f);
					}
				}
			}
			Button component2 = gameObject.GetComponent<Button>();
			if (component2 != null)
			{
				component2.onClick.AddListener(onClick);
				component2.onClick.AddListener(new UnityAction(this.presetContextMenuManager.Close));
			}
		}

		// Token: 0x06003981 RID: 14721 RVA: 0x00150BF0 File Offset: 0x0014EDF0
		private void OnContextMenuDuplicate()
		{
			CustomizationPresetData contextMenuTargetPreset = this._contextMenuTargetPreset;
			this._contextMenuTargetPreset = null;
			if (contextMenuTargetPreset == null || this.charactersManager == null || string.IsNullOrWhiteSpace(this._currentCharacterId))
			{
				return;
			}
			string text = contextMenuTargetPreset.presetName + " Copy";
			this.ExecuteDuplicatePreset(contextMenuTargetPreset, text);
		}

		// Token: 0x06003982 RID: 14722 RVA: 0x00150C44 File Offset: 0x0014EE44
		private void OnContextMenuDelete()
		{
			CustomizationPresetData contextMenuTargetPreset = this._contextMenuTargetPreset;
			this._contextMenuTargetPreset = null;
			if (contextMenuTargetPreset == null)
			{
				return;
			}
			this.HandleDeletePreset(contextMenuTargetPreset);
		}

		// Token: 0x06003983 RID: 14723 RVA: 0x00150C6A File Offset: 0x0014EE6A
		private bool CanStartHoldForPreset(CustomizationPresetData preset)
		{
			return this.enableMainCharacterHoldToSetInUse && preset != null && !preset.isDefault && this._currentTargetType == CustomizationTargetType.MainCharacter && !string.IsNullOrWhiteSpace(preset.presetId);
		}

		// Token: 0x06003984 RID: 14724 RVA: 0x00150C9C File Offset: 0x0014EE9C
		private void UpdateHoldToSetInUse()
		{
			if (!this._isHoldingSetInUse || this._holdPreset == null)
			{
				return;
			}
			float num = Mathf.Max(0f, this.holdToSetInUseStartDelaySeconds);
			float num2 = Time.unscaledTime - this._holdStartTimeUnscaled;
			if (num2 < num)
			{
				return;
			}
			if (!this._holdDelayPassed)
			{
				this._holdDelayPassed = true;
				this.ShowHoldProgress(0f);
			}
			float num3 = Mathf.Max(0.05f, this.holdToSetInUseDurationSeconds);
			float num4 = Mathf.Clamp01((num2 - num) / num3);
			this.SetHoldProgressFill(num4);
			if (num4 < 1f)
			{
				return;
			}
			this.ConfirmHoldToSetInUse();
		}

		// Token: 0x06003985 RID: 14725 RVA: 0x00150D2C File Offset: 0x0014EF2C
		private void ConfirmHoldToSetInUse()
		{
			CustomizationPresetData holdPreset = this._holdPreset;
			this.CancelHoldToSetInUse();
			if (holdPreset == null)
			{
				return;
			}
			if (this._currentTargetType != CustomizationTargetType.MainCharacter)
			{
				ToastService.Show("Here you can only preview NPCs.", "Preview only", null, -1f, "cielos_apps");
				return;
			}
			bool flag;
			if (this.hubPresetListController != null)
			{
				flag = this.hubPresetListController.SelectPreset(holdPreset.presetId, true, true);
			}
			else
			{
				flag = this.charactersManager != null && !string.IsNullOrWhiteSpace(this._currentCharacterId) && this.charactersManager.TrySetActivePresetForCharacter(this._currentCharacterId, holdPreset.presetId);
			}
			if (!flag)
			{
				ToastService.Show("Couldn't set it right now. Try again.", "Oops...", null, -1f, "cielos_apps");
				return;
			}
			this._selectedPresetId = holdPreset.presetId;
			this._suppressNextPresetClick = true;
			this.RefreshSelectionVisualState();
			this.RefreshActionStates();
			this.UpdateSelectedPresetLabel();
			this.ApplySelectedPresetPreview();
			this.TryPlaySetInUseConfirmedCardFeedback(holdPreset);
			MainCharacterAppearanceEvents.RaiseInUsePresetChanged();
			ToastService.ShowFormat("cielos_apps", "'{0}' is Ciel's active look.", new object[] { holdPreset.presetName }, "Look in use", null, -1f);
			if (this.hubPresetListController != null)
			{
				this.hubPresetListController.RefreshCurrentPresets();
				return;
			}
			if (this.charactersManager != null && !string.IsNullOrWhiteSpace(this._currentCharacterId))
			{
				this.charactersManager.RefreshPresetsForCharacter(this._currentCharacterId);
			}
		}

		// Token: 0x06003986 RID: 14726 RVA: 0x00150E8C File Offset: 0x0014F08C
		private void CancelHoldToSetInUse()
		{
			this._isHoldingSetInUse = false;
			this._holdPreset = null;
			this._holdDelayPassed = false;
			this.HideHoldProgress();
		}

		// Token: 0x06003987 RID: 14727 RVA: 0x00150EAC File Offset: 0x0014F0AC
		private void ShowHoldProgress(float fill)
		{
			this.ConfigureHoldProgressInputPassthrough();
			GameObject gameObject = this.GetHoldProgressRoot();
			if (gameObject == null)
			{
				return;
			}
			gameObject.SetActive(true);
			this.SetHoldProgressFill(fill);
		}

		// Token: 0x06003988 RID: 14728 RVA: 0x00150EE0 File Offset: 0x0014F0E0
		private void HideHoldProgress()
		{
			GameObject gameObject = this.GetHoldProgressRoot();
			if (gameObject != null)
			{
				gameObject.SetActive(false);
			}
			this.SetHoldProgressFill(0f);
		}

		// Token: 0x06003989 RID: 14729 RVA: 0x00150F0F File Offset: 0x0014F10F
		private void SetHoldProgressFill(float value)
		{
			if (this.holdProgressBar != null)
			{
				this.holdProgressBar.SetValue(Mathf.Clamp01(value) * 100f);
			}
		}

		// Token: 0x0600398A RID: 14730 RVA: 0x00150F36 File Offset: 0x0014F136
		private GameObject GetHoldProgressRoot()
		{
			if (this.holdProgressRoot != null)
			{
				return this.holdProgressRoot;
			}
			if (!(this.holdProgressBar != null))
			{
				return null;
			}
			return this.holdProgressBar.gameObject;
		}

		// Token: 0x0600398B RID: 14731 RVA: 0x00150F68 File Offset: 0x0014F168
		private void ConfigureHoldProgressInputPassthrough()
		{
			if (this._holdProgressInputConfigured)
			{
				return;
			}
			GameObject gameObject = this.GetHoldProgressRoot();
			if (gameObject == null)
			{
				return;
			}
			CanvasGroup component = gameObject.GetComponent<CanvasGroup>();
			if (component != null)
			{
				component.blocksRaycasts = false;
				component.interactable = false;
			}
			Graphic[] componentsInChildren = gameObject.GetComponentsInChildren<Graphic>(true);
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				if (componentsInChildren[i] != null)
				{
					componentsInChildren[i].raycastTarget = false;
				}
			}
			this._holdProgressInputConfigured = true;
		}

		// Token: 0x0600398C RID: 14732 RVA: 0x00150FE0 File Offset: 0x0014F1E0
		private void TryPlaySetInUseConfirmedCardFeedback(CustomizationPresetData preset)
		{
			if (preset == null || string.IsNullOrWhiteSpace(preset.presetId))
			{
				return;
			}
			CharacterCard characterCard;
			if (this._spawnedCards.TryGetValue(preset.presetId, out characterCard) && characterCard != null)
			{
				characterCard.PlaySetInUseConfirmedFeedback();
			}
		}

		// Token: 0x0600398D RID: 14733 RVA: 0x00151024 File Offset: 0x0014F224
		private void TryPlayHoldCanceledCardFeedback(CustomizationPresetData preset)
		{
			if (preset == null || string.IsNullOrWhiteSpace(preset.presetId))
			{
				return;
			}
			CharacterCard characterCard;
			if (this._spawnedCards.TryGetValue(preset.presetId, out characterCard) && characterCard != null)
			{
				characterCard.PlayHoldCanceledFeedback();
			}
		}

		// Token: 0x0600398E RID: 14734 RVA: 0x00151068 File Offset: 0x0014F268
		private void SetPreviewSelectionFromPointerDown(CustomizationPresetData preset)
		{
			if (preset == null || string.IsNullOrWhiteSpace(preset.presetId))
			{
				return;
			}
			this._suppressNextPointerDrivenClick = true;
			if (this.IsPresetAlreadySelected(preset))
			{
				return;
			}
			this._selectedPresetId = preset.presetId;
			if (this.hubPresetListController != null)
			{
				this.hubPresetListController.SelectPreset(this._selectedPresetId, true, false);
			}
			this.RefreshSelectionVisualState();
			this.RefreshActionStates();
			this.UpdateSelectedPresetLabel();
			this.ApplySelectedPresetPreview();
		}

		// Token: 0x0600398F RID: 14735 RVA: 0x001510E0 File Offset: 0x0014F2E0
		private bool IsPresetAlreadySelected(CustomizationPresetData preset)
		{
			if (preset == null || !(this._selectedPresetId == preset.presetId))
			{
				return false;
			}
			if (!(this.hubPresetListController == null))
			{
				CustomizationPresetData selectedPreset = this.hubPresetListController.SelectedPreset;
				return ((selectedPreset != null) ? selectedPreset.presetId : null) == preset.presetId;
			}
			return true;
		}

		// Token: 0x06003990 RID: 14736 RVA: 0x00151138 File Offset: 0x0014F338
		private bool TryGetSelectedPreset(out CustomizationPresetData selectedPreset)
		{
			selectedPreset = null;
			if (string.IsNullOrWhiteSpace(this._selectedPresetId))
			{
				return false;
			}
			for (int i = 0; i < this._currentPresets.Count; i++)
			{
				CustomizationPresetData customizationPresetData = this._currentPresets[i];
				if (customizationPresetData != null && customizationPresetData.presetId == this._selectedPresetId)
				{
					selectedPreset = customizationPresetData;
					return true;
				}
			}
			return false;
		}

		// Token: 0x06003991 RID: 14737 RVA: 0x00151196 File Offset: 0x0014F396
		private bool TryGetInputName(out string name)
		{
			string text;
			if (!(this.presetNameInput != null))
			{
				text = string.Empty;
			}
			else
			{
				string text2 = this.presetNameInput.text;
				text = ((text2 != null) ? text2.Trim() : null);
			}
			name = text;
			return !string.IsNullOrWhiteSpace(name);
		}

		// Token: 0x06003992 RID: 14738 RVA: 0x001511D0 File Offset: 0x0014F3D0
		private void ClearInput()
		{
			if (this.presetNameInput != null)
			{
				this.presetNameInput.text = string.Empty;
			}
		}

		// Token: 0x06003993 RID: 14739 RVA: 0x001511F0 File Offset: 0x0014F3F0
		private void EnsureSelectedPresetStillValid()
		{
			CustomizationPresetData customizationPresetData;
			if (this.TryGetSelectedPreset(out customizationPresetData))
			{
				return;
			}
			CustomizationPresetData customizationPresetData2 = CharacterPresetPanelController.FindActivePreset(this._currentPresets);
			if (customizationPresetData2 != null)
			{
				this._selectedPresetId = customizationPresetData2.presetId;
				return;
			}
			string text;
			if (this._currentPresets.Count <= 0)
			{
				text = null;
			}
			else
			{
				CustomizationPresetData customizationPresetData3 = this._currentPresets[0];
				text = ((customizationPresetData3 != null) ? customizationPresetData3.presetId : null);
			}
			this._selectedPresetId = text;
		}

		// Token: 0x06003994 RID: 14740 RVA: 0x00151254 File Offset: 0x0014F454
		private void ApplySelectedPresetPreview()
		{
			if (this.previewStateManager == null)
			{
				return;
			}
			CustomizationPresetData customizationPresetData;
			if (!this.TryGetSelectedPreset(out customizationPresetData))
			{
				return;
			}
			this.previewStateManager.SetActiveContext(this._currentTargetType);
			this.previewStateManager.LoadFromCharacterStateData(customizationPresetData.stateData, this._currentTargetType);
		}

		// Token: 0x06003995 RID: 14741 RVA: 0x001512A4 File Offset: 0x0014F4A4
		private void HandleHubPresetsLoaded(List<CustomizationPresetData> presets)
		{
			if (this.hubPresetListController != null)
			{
				this._currentTargetType = this.hubPresetListController.CurrentTargetType;
				this._currentCharacterId = this.hubPresetListController.CurrentCharacterId;
			}
			this._currentPresets.Clear();
			if (presets != null)
			{
				this._currentPresets.AddRange(presets);
			}
			this.UpdateNpcFilterVisibility();
			this.RebuildFilteredView();
			this.EnsureSelectedPresetStillValid();
			this.RefreshSelectionVisualState();
			this.RefreshActionStates();
			this.UpdateTargetLabel();
			this.UpdateSelectedPresetLabel();
			this.ApplySelectedPresetPreview();
		}

		// Token: 0x06003996 RID: 14742 RVA: 0x0015132C File Offset: 0x0014F52C
		private void RebuildFilteredView()
		{
			this._filteredPresets.Clear();
			bool flag = this._currentTargetType == CustomizationTargetType.Npc;
			for (int i = 0; i < this._currentPresets.Count; i++)
			{
				CustomizationPresetData customizationPresetData = this._currentPresets[i];
				if (customizationPresetData != null && (!flag || this.PassesNpcFilter(customizationPresetData)))
				{
					this._filteredPresets.Add(customizationPresetData);
				}
			}
			this.RebuildGrid();
			this.RefreshNpcRosterSummary();
		}

		// Token: 0x06003997 RID: 14743 RVA: 0x00151398 File Offset: 0x0014F598
		private bool PassesNpcFilter(CustomizationPresetData preset)
		{
			if ((this._npcOriginFilter == 1 && !preset.isSystemPreset) || (this._npcOriginFilter == 2 && preset.isSystemPreset))
			{
				return false;
			}
			bool flag = NpcRosterPreferences.IsEnabled(preset);
			return (this._npcStatusFilter != 1 || flag) && (this._npcStatusFilter != 2 || !flag) && ((preset.gender == Gender.Male && this._maleFilterOn) || (preset.gender == Gender.Female && this._femaleFilterOn)) && (string.IsNullOrWhiteSpace(this._npcSearch) || (preset.presetName ?? string.Empty).IndexOf(this._npcSearch, StringComparison.OrdinalIgnoreCase) >= 0);
		}

		// Token: 0x06003998 RID: 14744 RVA: 0x00151442 File Offset: 0x0014F642
		private void UpdateNpcFilterVisibility()
		{
			this.EnsureNpcRosterUI();
			this.EnsureNpcFilterBound();
			if (this.npcFilterRoot != null)
			{
				this.npcFilterRoot.SetActive(this._currentTargetType == CustomizationTargetType.Npc);
			}
			this.SetNpcRosterVisible(this._currentTargetType == CustomizationTargetType.Npc);
		}

		// Token: 0x06003999 RID: 14745 RVA: 0x00151484 File Offset: 0x0014F684
		private void EnsureNpcFilterBound()
		{
			if (this._npcFilterBound)
			{
				return;
			}
			if (this.maleFilterToggle != null)
			{
				this._maleFilterOn = this.maleFilterToggle.isOn;
				this.maleFilterToggle.onValueChanged.AddListener(new UnityAction<bool>(this.OnMaleFilterChanged));
			}
			if (this.femaleFilterToggle != null)
			{
				this._femaleFilterOn = this.femaleFilterToggle.isOn;
				this.femaleFilterToggle.onValueChanged.AddListener(new UnityAction<bool>(this.OnFemaleFilterChanged));
			}
			if (this.npcSearchInput != null)
			{
				this._npcSearch = this.npcSearchInput.text ?? string.Empty;
				this.npcSearchInput.onValueChanged.AddListener(new UnityAction<string>(this.OnNpcSearchChanged));
			}
			this._npcFilterBound = true;
		}

		// Token: 0x0600399A RID: 14746 RVA: 0x0015155B File Offset: 0x0014F75B
		private void OnMaleFilterChanged(bool value)
		{
			this._maleFilterOn = value;
			this.RebuildFilteredView();
		}

		// Token: 0x0600399B RID: 14747 RVA: 0x0015156A File Offset: 0x0014F76A
		private void OnFemaleFilterChanged(bool value)
		{
			this._femaleFilterOn = value;
			this.RebuildFilteredView();
		}

		// Token: 0x0600399C RID: 14748 RVA: 0x00151579 File Offset: 0x0014F779
		private void OnNpcSearchChanged(string value)
		{
			this._npcSearch = value ?? string.Empty;
			this.RebuildFilteredView();
		}

		// Token: 0x0600399D RID: 14749 RVA: 0x00151594 File Offset: 0x0014F794
		private void ApplyScrollSensitivity()
		{
			if (this.presetScrollRect == null && this.presetGridContainer != null)
			{
				this.presetScrollRect = this.presetGridContainer.GetComponentInParent<ScrollRect>();
			}
			if (this.presetScrollRect != null && this.scrollSensitivityOverride > 0f)
			{
				this.presetScrollRect.scrollSensitivity = this.scrollSensitivityOverride;
			}
			this.BindScrollListener();
		}

		// Token: 0x0600399E RID: 14750 RVA: 0x00151600 File Offset: 0x0014F800
		private void BindScrollListener()
		{
			if (this._scrollListenerBound || this.presetScrollRect == null)
			{
				return;
			}
			this.presetScrollRect.onValueChanged.AddListener(new UnityAction<Vector2>(this.HandleScrollChanged));
			this._scrollListenerBound = true;
		}

		// Token: 0x0600399F RID: 14751 RVA: 0x0015163C File Offset: 0x0014F83C
		private void UnbindScrollListener()
		{
			if (!this._scrollListenerBound || this.presetScrollRect == null)
			{
				return;
			}
			this.presetScrollRect.onValueChanged.RemoveListener(new UnityAction<Vector2>(this.HandleScrollChanged));
			this._scrollListenerBound = false;
		}

		// Token: 0x060039A0 RID: 14752 RVA: 0x00151678 File Offset: 0x0014F878
		private static CustomizationPresetData FindActivePreset(List<CustomizationPresetData> presets)
		{
			for (int i = 0; i < presets.Count; i++)
			{
				CustomizationPresetData customizationPresetData = presets[i];
				if (customizationPresetData != null && customizationPresetData.isDefault)
				{
					return customizationPresetData;
				}
			}
			for (int j = 0; j < presets.Count; j++)
			{
				CustomizationPresetData customizationPresetData2 = presets[j];
				if (customizationPresetData2 != null && customizationPresetData2.isActive)
				{
					return customizationPresetData2;
				}
			}
			return null;
		}

		// Token: 0x060039A1 RID: 14753 RVA: 0x001516D4 File Offset: 0x0014F8D4
		private void RefreshSelectionVisualState()
		{
			foreach (KeyValuePair<string, CharacterCard> keyValuePair in this._spawnedCards)
			{
				if (!(keyValuePair.Value == null))
				{
					keyValuePair.Value.SetSelected(keyValuePair.Key == this._selectedPresetId);
				}
			}
		}

		// Token: 0x060039A2 RID: 14754 RVA: 0x00151750 File Offset: 0x0014F950
		private void RefreshActionStates()
		{
			CustomizationPresetData customizationPresetData;
			bool flag = this.TryGetSelectedPreset(out customizationPresetData);
			if (this.duplicatePresetButton != null)
			{
				this.duplicatePresetButton.interactable = flag;
			}
			if (this.deletePresetButton != null)
			{
				this.deletePresetButton.interactable = flag && this.charactersManager != null && this.charactersManager.CanDeletePreset(customizationPresetData);
			}
		}

		// Token: 0x060039A3 RID: 14755 RVA: 0x001517BC File Offset: 0x0014F9BC
		private void UpdateSelectedPresetLabel()
		{
			if (this.selectedPresetLabel == null)
			{
				return;
			}
			CustomizationPresetData customizationPresetData;
			if (!this.TryGetSelectedPreset(out customizationPresetData))
			{
				LocalizedGameText localizedGameText = LocalizedGameText.BindSource(this.selectedPresetLabel, "cielos_apps", "Preset: None", null);
				if (localizedGameText == null)
				{
					return;
				}
				localizedGameText.FitTranslationToRectangle(false);
				return;
			}
			else
			{
				LocalizedGameText localizedGameText2 = LocalizedGameText.BindFormat(this.selectedPresetLabel, "cielos_apps", "Preset: {0}", new object[] { customizationPresetData.presetName });
				if (localizedGameText2 == null)
				{
					return;
				}
				localizedGameText2.FitTranslationToRectangle(false);
				return;
			}
		}

		// Token: 0x060039A4 RID: 14756 RVA: 0x00151834 File Offset: 0x0014FA34
		private void UpdateTargetLabel()
		{
			this.RefreshPresetInstructions();
			if (this.selectedTargetLabel == null)
			{
				return;
			}
			TMP_Text tmp_Text = this.selectedTargetLabel;
			CustomizationTargetType currentTargetType = this._currentTargetType;
			string text;
			if (currentTargetType != CustomizationTargetType.MainCharacter)
			{
				if (currentTargetType != CustomizationTargetType.Npc)
				{
					text = "Target: None";
				}
				else
				{
					text = "Target: NPC";
				}
			}
			else
			{
				text = "Target: Main Character";
			}
			LocalizedGameText localizedGameText = LocalizedGameText.BindSource(tmp_Text, "cielos_apps", text, null);
			if (localizedGameText == null)
			{
				return;
			}
			localizedGameText.FitTranslationToRectangle(false);
		}

		// Token: 0x060039A5 RID: 14757 RVA: 0x0015189C File Offset: 0x0014FA9C
		private void RefreshPresetInstructions()
		{
			if (this._presetInstructionsRoot == null)
			{
				this.CreatePresetInstructions();
			}
			if (this._presetInstructionsLabel == null)
			{
				return;
			}
			this._instructionsController = ControllerInput.IsActive;
			this._instructionsGlyphVersion = ControllerGlyphs.Version;
			LocalizedGameText localizedGameText = LocalizedGameText.BindComputed(this._presetInstructionsLabel, new Func<string>(this.PresetInstructions));
			if (localizedGameText != null)
			{
				localizedGameText.FitTranslationToRectangle(false);
			}
			this._presetInstructionsLastWidth = -1f;
			this.UpdatePresetInstructionsLayout();
		}

		// Token: 0x060039A6 RID: 14758 RVA: 0x00151918 File Offset: 0x0014FB18
		private string PresetInstructions()
		{
			if (ControllerInput.IsActive)
			{
				return string.Concat(new string[]
				{
					ControllerGlyphs.Format("A"),
					"  ",
					GameLocalization.GetSource("cielos_apps", "Preview preset"),
					"\n",
					ControllerGlyphs.Format("Y"),
					"  ",
					GameLocalization.GetSource("cielos_apps", "Edit"),
					"   ",
					ControllerGlyphs.Format("X"),
					"  ",
					GameLocalization.GetSource("cielos_apps", "Preset options")
				});
			}
			bool flag = this._currentTargetType == CustomizationTargetType.MainCharacter && this.enableMainCharacterHoldToSetInUse;
			return GameLocalization.GetSource("cielos_apps", flag ? "<b><color=#FFC1D5>HOLD LEFT MOUSE</color></b> on a preset to use it.\n" : "<b><color=#FFC1D5>LEFT CLICK</color></b> a preset to preview it.\n") + GameLocalization.GetSource("cielos_apps", "<b><color=#FFC1D5>RIGHT CLICK</color></b> a preset for more options.");
		}

		// Token: 0x060039A7 RID: 14759 RVA: 0x00151A04 File Offset: 0x0014FC04
		private void CreatePresetInstructions()
		{
			if (this.presetScrollRect == null)
			{
				this.ApplyScrollSensitivity();
			}
			if (this.presetScrollRect == null || !(this.presetScrollRect.transform.parent is RectTransform))
			{
				return;
			}
			this._presetInstructionsScrollRect = (RectTransform)this.presetScrollRect.transform;
			this._presetInstructionsOriginalScrollOffsetMin = ((this._rosterScroll != null) ? this._rosterScrollMin : this._presetInstructionsScrollRect.offsetMin);
			this._presetInstructionsOriginalScrollOffsetMax = ((this._rosterScroll != null) ? this._rosterScrollMax : this._presetInstructionsScrollRect.offsetMax);
			GameObject gameObject = new GameObject("Preset Mouse Instructions", new Type[]
			{
				typeof(RectTransform),
				typeof(Image),
				typeof(LayoutElement)
			});
			gameObject.layer = this.presetScrollRect.gameObject.layer;
			gameObject.transform.SetParent(this._presetInstructionsScrollRect.parent, false);
			gameObject.GetComponent<LayoutElement>().ignoreLayout = true;
			Image component = gameObject.GetComponent<Image>();
			component.color = new Color(0.16f, 0.09f, 0.22f, 0.98f);
			component.raycastTarget = false;
			this._presetInstructionsRoot = (RectTransform)gameObject.transform;
			this._presetInstructionsRoot.anchorMin = this._presetInstructionsScrollRect.anchorMin;
			this._presetInstructionsRoot.anchorMax = new Vector2(this._presetInstructionsScrollRect.anchorMax.x, this._presetInstructionsScrollRect.anchorMin.y);
			this._presetInstructionsRoot.pivot = new Vector2(this._presetInstructionsScrollRect.pivot.x, 0f);
			this._presetInstructionsRoot.anchoredPosition = new Vector2(this._presetInstructionsScrollRect.anchoredPosition.x, this._presetInstructionsOriginalScrollOffsetMin.y);
			this._presetInstructionsRoot.sizeDelta = new Vector2(this._presetInstructionsScrollRect.sizeDelta.x, 72f);
			GameObject gameObject2 = new GameObject("Instructions", new Type[]
			{
				typeof(RectTransform),
				typeof(TextMeshProUGUI)
			});
			gameObject2.layer = gameObject.layer;
			gameObject2.transform.SetParent(gameObject.transform, false);
			this._presetInstructionsLabel = gameObject2.GetComponent<TextMeshProUGUI>();
			if (this.presetCardPrefab != null)
			{
				TMP_Text componentInChildren = this.presetCardPrefab.GetComponentInChildren<TMP_Text>(true);
				if (componentInChildren != null && componentInChildren.font != null)
				{
					this._presetInstructionsLabel.font = componentInChildren.font;
				}
			}
			this._presetInstructionsLabel.fontSize = 20f;
			this._presetInstructionsLabel.enableAutoSizing = true;
			this._presetInstructionsLabel.fontSizeMin = 14f;
			this._presetInstructionsLabel.fontSizeMax = 20f;
			this._presetInstructionsLabel.color = Color.white;
			this._presetInstructionsLabel.alignment = TextAlignmentOptions.MidlineLeft;
			this._presetInstructionsLabel.enableWordWrapping = false;
			this._presetInstructionsLabel.lineSpacing = 4f;
			this._presetInstructionsLabel.raycastTarget = false;
			RectTransform rectTransform = this._presetInstructionsLabel.rectTransform;
			rectTransform.anchorMin = Vector2.zero;
			rectTransform.anchorMax = Vector2.one;
			rectTransform.offsetMin = new Vector2(16f, 10f);
			rectTransform.offsetMax = new Vector2(-16f, -10f);
		}

		// Token: 0x060039A8 RID: 14760 RVA: 0x00151D80 File Offset: 0x0014FF80
		private void UpdatePresetInstructionsLayout()
		{
			if (this._presetInstructionsRoot == null || this._presetInstructionsLabel == null || this._presetInstructionsScrollRect == null)
			{
				return;
			}
			float num = Mathf.Max(1f, this._presetInstructionsRoot.rect.width - 32f);
			if (Mathf.Approximately(num, this._presetInstructionsLastWidth))
			{
				return;
			}
			this._presetInstructionsLastWidth = num;
			float num2 = Mathf.Max(72f, Mathf.Ceil(this._presetInstructionsLabel.GetPreferredValues(num, float.PositiveInfinity).y) + 20f);
			this._presetInstructionsRoot.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, num2);
			this._presetInstructionsScrollRect.offsetMax = this._presetInstructionsOriginalScrollOffsetMax;
			this._presetInstructionsScrollRect.offsetMin = new Vector2(this._presetInstructionsOriginalScrollOffsetMin.x, this._presetInstructionsOriginalScrollOffsetMin.y + num2 + 8f);
		}

		// Token: 0x060039A9 RID: 14761 RVA: 0x00151E6C File Offset: 0x0015006C
		private static string DuplicatePresetIconFile(string sourceIconFileName, string duplicatedPresetName)
		{
			if (string.IsNullOrWhiteSpace(sourceIconFileName))
			{
				return null;
			}
			string text = Path.Combine(GameDataPaths.CustomizationPresetIconsDirectory, sourceIconFileName);
			if (!File.Exists(text))
			{
				return null;
			}
			string text5;
			try
			{
				GameDataPaths.EnsureCustomizationDirectories();
				string text2 = CharacterPresetPanelController.SanitizeFileName(string.IsNullOrWhiteSpace(duplicatedPresetName) ? "preset_copy" : duplicatedPresetName);
				string text3 = string.Format("{0}_{1:yyyyMMddHHmmss}.png", text2, DateTime.UtcNow);
				string text4 = Path.Combine(GameDataPaths.CustomizationPresetIconsDirectory, text3);
				File.Copy(text, text4, true);
				text5 = text3;
			}
			catch
			{
				text5 = null;
			}
			return text5;
		}

		// Token: 0x060039AA RID: 14762 RVA: 0x00151EFC File Offset: 0x001500FC
		private string ResolveUniquePresetName(string baseName)
		{
			if (this._currentPresets == null || this._currentPresets.Count == 0)
			{
				return baseName;
			}
			if (!this.PresetNameExistsInCurrentList(baseName))
			{
				return baseName;
			}
			for (int i = 2; i <= 99; i++)
			{
				string text = string.Format("{0} ({1})", baseName, i);
				if (!this.PresetNameExistsInCurrentList(text))
				{
					return text;
				}
			}
			return string.Format("{0} ({1:HHmmss})", baseName, DateTime.UtcNow);
		}

		// Token: 0x060039AB RID: 14763 RVA: 0x00151F6C File Offset: 0x0015016C
		private bool PresetNameExistsInCurrentList(string name)
		{
			string text = ((name != null) ? name.Trim() : null);
			if (string.IsNullOrWhiteSpace(text))
			{
				return false;
			}
			for (int i = 0; i < this._currentPresets.Count; i++)
			{
				CustomizationPresetData customizationPresetData = this._currentPresets[i];
				if (customizationPresetData != null)
				{
					string presetName = customizationPresetData.presetName;
					if (string.Equals((presetName != null) ? presetName.Trim() : null, text, StringComparison.OrdinalIgnoreCase))
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x060039AC RID: 14764 RVA: 0x00151FD4 File Offset: 0x001501D4
		private static string SanitizeFileName(string value)
		{
			if (string.IsNullOrWhiteSpace(value))
			{
				return "Preset";
			}
			foreach (char c in Path.GetInvalidFileNameChars())
			{
				value = value.Replace(c, '_');
			}
			return value;
		}

		// Token: 0x060039AD RID: 14765 RVA: 0x00152014 File Offset: 0x00150214
		private void EnsureNpcRosterUI()
		{
			if (this._rosterToolbar != null || this.presetScrollRect == null || this.presetGridContainer == null)
			{
				return;
			}
			this._rosterScroll = this.presetScrollRect.transform as RectTransform;
			if (!(this._rosterScroll == null))
			{
				RectTransform rectTransform = this._rosterScroll.parent as RectTransform;
				if (rectTransform != null)
				{
					this._rosterScrollMin = this._rosterScroll.offsetMin;
					this._rosterScrollMax = this._rosterScroll.offsetMax;
					this._npcSearch = string.Empty;
					this._npcOriginFilter = (this._npcStatusFilter = 0);
					this._maleFilterOn = (this._femaleFilterOn = true);
					TMP_Text tmp_Text = ((this.presetCardPrefab != null) ? this.presetCardPrefab.GetComponentInChildren<TMP_Text>(true) : null);
					this._rosterFont = ((tmp_Text != null) ? LocalizedGameText.SourceFont(tmp_Text) : TMP_Settings.defaultFontAsset);
					this._rosterGrid = this.presetGridContainer.GetComponent<GridLayoutGroup>();
					if (this._rosterGrid != null)
					{
						this._rosterOriginalCellSize = this._rosterGrid.cellSize;
					}
					this._rosterMinimum = NPCRosterSession.MinimumEligible;
					Transform transform = ((rectTransform.parent != null) ? rectTransform.parent.Find("Controls") : null);
					if (transform != null)
					{
						foreach (TMP_Text tmp_Text2 in transform.GetComponentsInChildren<TMP_Text>(true))
						{
							if (tmp_Text2.text.IndexOf("Manage", StringComparison.OrdinalIgnoreCase) >= 0 || !(tmp_Text2.text != GameLocalization.GetSource("cielos_apps", "Manage Your Characters")))
							{
								this._rosterHeading = tmp_Text2;
								this._rosterOriginalHeading = "Manage Your Characters";
								break;
							}
						}
					}
					this._rosterToolbar = CharacterPresetPanelController.RosterRect("NPC Library Filters", rectTransform, Vector2.up, Vector2.one, new Vector2(0.5f, 1f), Vector2.zero, new Vector2(0f, 112f));
					CharacterPresetPanelController.RosterBackground(this._rosterToolbar, CharacterPresetPanelController.RosterControl);
					this._rosterToolbar.gameObject.AddComponent<LayoutElement>().ignoreLayout = true;
					if (this.npcFilterRoot != null)
					{
						this.npcFilterRoot.SetActive(false);
					}
					this.npcFilterRoot = this._rosterToolbar.gameObject;
					RectTransform rectTransform2 = CharacterPresetPanelController.RosterRect("Search", this._rosterToolbar, Vector2.up, Vector2.one, new Vector2(0.5f, 1f), new Vector2(0f, -8f), new Vector2(-32f, 36f));
					Image image = CharacterPresetPanelController.RosterBackground(rectTransform2, CharacterPresetPanelController.RosterInk);
					TMP_InputField input = rectTransform2.gameObject.AddComponent<TMP_InputField>();
					input.targetGraphic = image;
					input.lineType = TMP_InputField.LineType.SingleLine;
					input.characterLimit = 80;
					RectTransform rectTransform3 = CharacterPresetPanelController.RosterRect("Text Area", rectTransform2, Vector2.zero, Vector2.one, new Vector2(0.5f, 0.5f), Vector2.zero, Vector2.zero);
					rectTransform3.offsetMin = new Vector2(12f, 3f);
					rectTransform3.offsetMax = new Vector2(-40f, -3f);
					rectTransform3.gameObject.AddComponent<RectMask2D>();
					input.textViewport = rectTransform3;
					input.textComponent = this.RosterLabel("Text", rectTransform3, "", 19f, Color.white);
					TextMeshProUGUI textMeshProUGUI = this.RosterLabel("Placeholder", rectTransform3, "Search NPCs...", 19f, CharacterPresetPanelController.RosterMuted);
					input.placeholder = textMeshProUGUI;
					input.customCaretColor = true;
					input.caretColor = CharacterPresetPanelController.RosterPale;
					this.npcSearchInput = input;
					input.onValueChanged.AddListener(new UnityAction<string>(this.OnNpcSearchChanged));
					this._npcFilterBound = true;
					CharacterPresetPanelController.RosterPlace(this.RosterButton("Clear Search", rectTransform2, "X", delegate
					{
						input.text = string.Empty;
					}).transform as RectTransform, new Vector2(1f, 0.5f), new Vector2(1f, 0.5f), new Vector2(1f, 0.5f), new Vector2(-2f, 0f), new Vector2(34f, 32f));
					this.CreateRosterDropdown(this._rosterToolbar, "Source", new string[] { "All sources", "System", "Custom" }, 0, delegate(int value)
					{
						this._npcOriginFilter = value;
						this.RebuildFilteredView();
					});
					this.CreateRosterDropdown(this._rosterToolbar, "Gender", new string[] { "All genders", "Male", "Female" }, 1, delegate(int value)
					{
						this._maleFilterOn = value != 2;
						this._femaleFilterOn = value != 1;
						this.RebuildFilteredView();
					});
					this.CreateRosterDropdown(this._rosterToolbar, "Status", new string[] { "All states", "Active", "Inactive" }, 2, delegate(int value)
					{
						this._npcStatusFilter = value;
						this.RebuildFilteredView();
					});
					this._rosterCount = this.RosterLabel("Results", this._rosterToolbar, "", 16f, CharacterPresetPanelController.RosterMuted);
					CharacterPresetPanelController.RosterPlace(this._rosterCount.rectTransform, Vector2.zero, Vector2.right, new Vector2(0.5f, 0f), new Vector2(0f, 2f), new Vector2(-32f, 24f));
					this._rosterFooter = CharacterPresetPanelController.RosterRect("NPC Selection Summary", rectTransform, Vector2.zero, Vector2.right, new Vector2(0.5f, 0f), Vector2.zero, new Vector2(0f, 90f));
					this._rosterFooter.gameObject.AddComponent<LayoutElement>().ignoreLayout = true;
					CharacterPresetPanelController.RosterBackground(this._rosterFooter, CharacterPresetPanelController.RosterInk);
					CharacterPresetPanelController.RosterBackground(CharacterPresetPanelController.RosterRect("Divider", this._rosterFooter, Vector2.up, Vector2.one, new Vector2(0.5f, 1f), Vector2.zero, new Vector2(-32f, 1f)), new Color(CharacterPresetPanelController.RosterPale.r, CharacterPresetPanelController.RosterPale.g, CharacterPresetPanelController.RosterPale.b, 0.24f));
					this._rosterCustomCount = this.RosterLabel("Custom Slots", this._rosterFooter, "", 18f, CharacterPresetPanelController.RosterPale);
					CharacterPresetPanelController.RosterPlace(this._rosterCustomCount.rectTransform, Vector2.up, new Vector2(0.5f, 1f), new Vector2(0f, 1f), new Vector2(16f, -5f), new Vector2(-16f, 27f));
					this._rosterAvailableCount = this.RosterLabel("Active Count", this._rosterFooter, "", 18f, Color.white);
					this._rosterAvailableCount.alignment = TextAlignmentOptions.MidlineRight;
					CharacterPresetPanelController.RosterPlace(this._rosterAvailableCount.rectTransform, new Vector2(0.5f, 1f), Vector2.one, new Vector2(1f, 1f), new Vector2(-16f, -5f), new Vector2(-16f, 27f));
					this._rosterMaleCount = this.CreateRosterGenderCount(Gender.Male, 0);
					this._rosterFemaleCount = this.CreateRosterGenderCount(Gender.Female, 1);
					this._rosterStatus = this.RosterLabel("Minimum Status", this._rosterFooter, "", 16f, CharacterPresetPanelController.RosterMuted);
					CharacterPresetPanelController.RosterPlace(this._rosterStatus.rectTransform, Vector2.up, Vector2.one, new Vector2(0.5f, 1f), new Vector2(-87f, -62f), new Vector2(-206f, 24f));
					this._rosterFill = this.RosterButton("Complete Selection", this._rosterFooter, "Add system NPCs", delegate
					{
						int num = NpcRosterPreferences.EnableSystemToMinimumPerGender(this._currentPresets, this._rosterMinimum);
						if (num > 0)
						{
							ToastService.ShowFormat("cielos_apps", "Enabled {0} system NPCs.", new object[] { num }, "NPC selection", null, -1f);
							return;
						}
						ToastService.Show("No more compatible system NPCs are available.", "NPC selection", null, -1f, "cielos_apps");
					});
					CharacterPresetPanelController.RosterPlace((RectTransform)this._rosterFill.transform, Vector2.one, Vector2.one, Vector2.one, new Vector2(-16f, -62f), new Vector2(158f, 25f));
					CharacterPresetPanelController.RosterPlace(this.RosterLabel("Mouse Hint", this._rosterFooter, "Click to preview  /  Double-click to edit  /  Right-click for options", 14f, CharacterPresetPanelController.RosterMuted).rectTransform, Vector2.zero, Vector2.right, new Vector2(0.5f, 0f), new Vector2(0f, 7f), new Vector2(-32f, 23f));
					this._rosterEmpty = this.RosterLabel("No Matching NPCs", this.presetScrollRect.viewport, "No NPCs match these filters.\nTry a different name or clear a filter.", 21f, CharacterPresetPanelController.RosterMuted);
					this._rosterEmpty.alignment = TextAlignmentOptions.Center;
					this._rosterEmpty.enableWordWrapping = true;
					this._rosterEmpty.gameObject.SetActive(false);
					return;
				}
			}
		}

		// Token: 0x060039AE RID: 14766 RVA: 0x00152938 File Offset: 0x00150B38
		private TextMeshProUGUI CreateRosterGenderCount(Gender gender, int column)
		{
			RectTransform rectTransform = CharacterPresetPanelController.RosterRect(gender.ToString() + " Active", this._rosterFooter, new Vector2((float)column * 0.5f, 1f), new Vector2((float)(column + 1) * 0.5f, 1f), new Vector2(0.5f, 1f), new Vector2(0f, -33f), new Vector2(-32f, 24f));
			Sprite sprite = ((this.presetCardPrefab != null) ? this.presetCardPrefab.GetNpcGenderIcon(gender) : null);
			TextMeshProUGUI textMeshProUGUI = this.RosterLabel("Count", rectTransform, "", 17f, CharacterPresetPanelController.RosterMuted);
			if (sprite != null)
			{
				Image image = CharacterPresetPanelController.RosterRect("Gender", rectTransform, Vector2.zero, Vector2.up, new Vector2(0f, 0.5f), Vector2.zero, new Vector2(19f, 0f)).gameObject.AddComponent<Image>();
				image.sprite = sprite;
				image.preserveAspect = true;
				image.color = CharacterPresetPanelController.RosterMuted;
				image.raycastTarget = false;
				textMeshProUGUI.rectTransform.offsetMin = new Vector2(27f, 0f);
			}
			else
			{
				TextMeshProUGUI textMeshProUGUI2 = this.RosterLabel("Gender", rectTransform, gender.ToString(), 17f, CharacterPresetPanelController.RosterMuted);
				textMeshProUGUI2.rectTransform.anchorMax = Vector2.up;
				textMeshProUGUI2.rectTransform.sizeDelta = new Vector2(64f, 0f);
				textMeshProUGUI2.rectTransform.pivot = new Vector2(0f, 0.5f);
				textMeshProUGUI.rectTransform.offsetMin = new Vector2(68f, 0f);
			}
			return textMeshProUGUI;
		}

		// Token: 0x060039AF RID: 14767 RVA: 0x00152B04 File Offset: 0x00150D04
		private void SetNpcRosterVisible(bool visible)
		{
			if (this._rosterToolbar == null)
			{
				return;
			}
			this._rosterToolbar.gameObject.SetActive(visible);
			this._rosterFooter.gameObject.SetActive(visible);
			if (this._rosterGrid != null)
			{
				this._rosterGrid.cellSize = (visible ? new Vector2(this._rosterOriginalCellSize.x, 300f) : this._rosterOriginalCellSize);
			}
			if (this._rosterHeading != null)
			{
				LocalizedGameText localizedGameText = LocalizedGameText.BindSource(this._rosterHeading, "cielos_apps", visible ? "Your NPCs" : this._rosterOriginalHeading, null);
				if (localizedGameText != null)
				{
					localizedGameText.FitTranslationToRectangle(false);
				}
			}
			if (this._presetInstructionsRoot != null)
			{
				this._presetInstructionsRoot.gameObject.SetActive(!visible);
			}
			if (!visible)
			{
				this._rosterEmpty.gameObject.SetActive(false);
				if (this._rosterLayoutWasVisible)
				{
					this._rosterScroll.offsetMax = this._rosterScrollMax;
					this._rosterScroll.offsetMin = this._rosterScrollMin;
					this._presetInstructionsLastWidth = -1f;
				}
			}
			this._rosterLayoutWasVisible = visible;
			this.ApplyNpcRosterLayout();
		}

		// Token: 0x060039B0 RID: 14768 RVA: 0x00152C30 File Offset: 0x00150E30
		private void ApplyNpcRosterLayout()
		{
			if (this._rosterScroll == null || this._currentTargetType != CustomizationTargetType.Npc)
			{
				return;
			}
			if (this._presetInstructionsRoot != null)
			{
				this._presetInstructionsRoot.gameObject.SetActive(false);
			}
			float num = ((this._rosterFooter != null) ? this._rosterFooter.sizeDelta.y : 90f);
			Vector2 vector = new Vector2(this._rosterScrollMin.x, num + 4f);
			Vector2 vector2 = new Vector2(this._rosterScrollMax.x, -112f);
			if (this._rosterScroll.offsetMin != vector)
			{
				this._rosterScroll.offsetMin = vector;
			}
			if (this._rosterScroll.offsetMax != vector2)
			{
				this._rosterScroll.offsetMax = vector2;
			}
		}

		// Token: 0x060039B1 RID: 14769 RVA: 0x00152D08 File Offset: 0x00150F08
		private void RefreshNpcRosterSummary()
		{
			if (this._rosterCustomCount == null || this._currentTargetType != CustomizationTargetType.Npc)
			{
				return;
			}
			int num = NpcRosterPreferences.CountEnabledCustom(this._currentPresets);
			int num2 = NpcRosterPreferences.CountEnabled(this._currentPresets);
			int male = NpcRosterPreferences.CountEnabled(this._currentPresets, Gender.Male);
			int female = NpcRosterPreferences.CountEnabled(this._currentPresets, Gender.Female);
			LocalizedGameText localizedGameText = LocalizedGameText.BindFormat(this._rosterCount, "cielos_apps", "{0} of {1} NPCs", new object[]
			{
				this._filteredPresets.Count,
				this._currentPresets.Count
			});
			if (localizedGameText != null)
			{
				localizedGameText.FitTranslationToRectangle(false);
			}
			LocalizedGameText localizedGameText2 = LocalizedGameText.BindFormat(this._rosterCustomCount, "cielos_apps", "CUSTOM ACTIVE  {0} / {1}", new object[] { num, 4 });
			if (localizedGameText2 != null)
			{
				localizedGameText2.FitTranslationToRectangle(false);
			}
			LocalizedGameText localizedGameText3 = LocalizedGameText.BindFormat(this._rosterAvailableCount, "cielos_apps", "ACTIVE  {0}", new object[] { num2 });
			if (localizedGameText3 != null)
			{
				localizedGameText3.FitTranslationToRectangle(false);
			}
			LocalizedGameText localizedGameText4 = LocalizedGameText.BindFormat(this._rosterMaleCount, "cielos_apps", "{0}  /  MIN {1}", new object[] { male, this._rosterMinimum });
			if (localizedGameText4 != null)
			{
				localizedGameText4.FitTranslationToRectangle(false);
			}
			LocalizedGameText localizedGameText5 = LocalizedGameText.BindFormat(this._rosterFemaleCount, "cielos_apps", "{0}  /  MIN {1}", new object[] { female, this._rosterMinimum });
			if (localizedGameText5 != null)
			{
				localizedGameText5.FitTranslationToRectangle(false);
			}
			bool maleShortfall = male < this._rosterMinimum;
			bool femaleShortfall = female < this._rosterMinimum;
			bool flag = maleShortfall | femaleShortfall;
			this._rosterMaleCount.color = (maleShortfall ? CharacterPresetPanelController.RosterPale : CharacterPresetPanelController.RosterMuted);
			this._rosterFemaleCount.color = (femaleShortfall ? CharacterPresetPanelController.RosterPale : CharacterPresetPanelController.RosterMuted);
			this._rosterAvailableCount.color = (flag ? CharacterPresetPanelController.RosterPale : Color.white);
			this._rosterFill.gameObject.SetActive(flag);
			this._rosterStatus.gameObject.SetActive(flag);
			LocalizedGameText localizedGameText6 = LocalizedGameText.BindComputed(this._rosterStatus, delegate
			{
				if (maleShortfall & femaleShortfall)
				{
					return GameLocalization.GetSource("cielos_apps", "Enable more male and female NPCs.");
				}
				if (maleShortfall)
				{
					return GameLocalization.FormatSource("cielos_apps", "Enable {0} more male NPCs.", new object[] { this._rosterMinimum - male });
				}
				if (!femaleShortfall)
				{
					return string.Empty;
				}
				return GameLocalization.FormatSource("cielos_apps", "Enable {0} more female NPCs.", new object[] { this._rosterMinimum - female });
			});
			if (localizedGameText6 != null)
			{
				localizedGameText6.FitTranslationToRectangle(false);
			}
			this._rosterFooter.sizeDelta = new Vector2(0f, flag ? 120f : 90f);
			this.ApplyNpcRosterLayout();
			this._rosterEmpty.gameObject.SetActive(this._filteredPresets.Count == 0);
		}

		// Token: 0x060039B2 RID: 14770 RVA: 0x00152FD0 File Offset: 0x001511D0
		private void HandleNpcRosterToggle(CustomizationPresetData preset)
		{
			string text;
			if (!NpcRosterPreferences.TrySetEnabled(preset, !NpcRosterPreferences.IsEnabled(preset), this._currentPresets, this._rosterMinimum, out text))
			{
				string text2 = ((preset != null && preset.gender == Gender.Female) ? "female" : "male");
				if (text == string.Format("Keep at least {0} {1} NPCs active.", this._rosterMinimum, text2))
				{
					ToastService.ShowFormat("cielos_apps", (text2 == "female") ? "Keep at least {0} female NPCs active." : "Keep at least {0} male NPCs active.", new object[] { this._rosterMinimum }, "NPC selection", null, -1f);
					return;
				}
				ToastService.Show(text, "NPC selection", null, -1f, "cielos_apps");
			}
		}

		// Token: 0x060039B3 RID: 14771 RVA: 0x00153090 File Offset: 0x00151290
		private void HandleNpcRosterChanged()
		{
			if (this._currentTargetType != CustomizationTargetType.Npc)
			{
				return;
			}
			if (this._npcStatusFilter != 0)
			{
				this.RebuildFilteredView();
				return;
			}
			foreach (CustomizationPresetData customizationPresetData in this._currentPresets)
			{
				CharacterCard characterCard;
				if (customizationPresetData != null && !string.IsNullOrEmpty(customizationPresetData.presetId) && this._spawnedCards.TryGetValue(customizationPresetData.presetId, out characterCard) && characterCard != null)
				{
					characterCard.RefreshNpcRoster(NpcRosterPreferences.IsEnabled(customizationPresetData), NpcRosterPreferences.IsEnabled(customizationPresetData));
				}
			}
			this.RefreshNpcRosterSummary();
		}

		// Token: 0x060039B4 RID: 14772 RVA: 0x0015313C File Offset: 0x0015133C
		private TMP_Dropdown CreateRosterDropdown(RectTransform parent, string name, string[] options, int column, UnityAction<int> changed)
		{
			RectTransform rectTransform = CharacterPresetPanelController.RosterRect(name + " Filter", parent, new Vector2((float)column / 3f, 1f), new Vector2(((float)column + 1f) / 3f, 1f), new Vector2(0.5f, 1f), Vector2.zero, new Vector2(0f, 34f));
			rectTransform.offsetMin = new Vector2((column == 0) ? 16f : 4f, -86f);
			rectTransform.offsetMax = new Vector2((column == 2) ? (-16f) : (-4f), -52f);
			Image image = CharacterPresetPanelController.RosterBackground(rectTransform, CharacterPresetPanelController.RosterInk);
			TMP_Dropdown tmp_Dropdown = rectTransform.gameObject.AddComponent<TMP_Dropdown>();
			tmp_Dropdown.targetGraphic = image;
			TextMeshProUGUI textMeshProUGUI = this.RosterLabel("Value", rectTransform, "", 18f, Color.white);
			textMeshProUGUI.rectTransform.offsetMin = new Vector2(10f, 0f);
			textMeshProUGUI.rectTransform.offsetMax = new Vector2(-26f, 0f);
			tmp_Dropdown.captionText = textMeshProUGUI;
			TextMeshProUGUI textMeshProUGUI2 = this.RosterLabel("Arrow", rectTransform, "v", 16f, CharacterPresetPanelController.RosterPale);
			CharacterPresetPanelController.RosterPlace(textMeshProUGUI2.rectTransform, Vector2.right, Vector2.one, new Vector2(1f, 0.5f), new Vector2(-9f, 0f), new Vector2(14f, 0f));
			textMeshProUGUI2.alignment = TextAlignmentOptions.Center;
			RectTransform rectTransform2 = CharacterPresetPanelController.RosterRect("Template", rectTransform, Vector2.zero, Vector2.right, new Vector2(0.5f, 1f), new Vector2(0f, -3f), new Vector2(0f, 116f));
			CharacterPresetPanelController.RosterBackground(rectTransform2, CharacterPresetPanelController.RosterInk);
			ScrollRect scrollRect = rectTransform2.gameObject.AddComponent<ScrollRect>();
			scrollRect.horizontal = false;
			scrollRect.movementType = ScrollRect.MovementType.Clamped;
			RectTransform rectTransform3 = CharacterPresetPanelController.RosterRect("Viewport", rectTransform2, Vector2.zero, Vector2.one, new Vector2(0.5f, 0.5f), Vector2.zero, new Vector2(-4f, -4f));
			rectTransform3.gameObject.AddComponent<RectMask2D>();
			RectTransform rectTransform4 = CharacterPresetPanelController.RosterRect("Content", rectTransform3, Vector2.up, Vector2.one, new Vector2(0.5f, 1f), Vector2.zero, new Vector2(0f, 36f));
			RectTransform rectTransform5 = CharacterPresetPanelController.RosterRect("Item", rectTransform4, Vector2.up, Vector2.one, new Vector2(0.5f, 1f), Vector2.zero, new Vector2(0f, 36f));
			Toggle toggle = rectTransform5.gameObject.AddComponent<Toggle>();
			Image image2 = CharacterPresetPanelController.RosterBackground(rectTransform5, CharacterPresetPanelController.RosterControl);
			toggle.targetGraphic = image2;
			RectTransform rectTransform6 = CharacterPresetPanelController.RosterRect("Selected", rectTransform5, Vector2.zero, Vector2.up, new Vector2(0f, 0.5f), new Vector2(0f, 0f), new Vector2(3f, 0f));
			toggle.graphic = CharacterPresetPanelController.RosterBackground(rectTransform6, CharacterPresetPanelController.RosterPink);
			TextMeshProUGUI textMeshProUGUI3 = this.RosterLabel("Item Label", rectTransform5, "Option", 18f, Color.white);
			textMeshProUGUI3.rectTransform.offsetMin = new Vector2(12f, 0f);
			textMeshProUGUI3.rectTransform.offsetMax = new Vector2(-6f, 0f);
			tmp_Dropdown.itemText = textMeshProUGUI3;
			tmp_Dropdown.template = rectTransform2;
			scrollRect.viewport = rectTransform3;
			scrollRect.content = rectTransform4;
			tmp_Dropdown.AddOptions(new List<string>(options));
			CustomizationDropdownLocalization.Bind(tmp_Dropdown, options);
			tmp_Dropdown.RefreshShownValue();
			tmp_Dropdown.onValueChanged.AddListener(changed);
			rectTransform2.gameObject.SetActive(false);
			return tmp_Dropdown;
		}

		// Token: 0x060039B5 RID: 14773 RVA: 0x00153510 File Offset: 0x00151710
		private static RectTransform RosterRect(string name, Transform parent, Vector2 min, Vector2 max, Vector2 pivot, Vector2 position, Vector2 size)
		{
			RectTransform rectTransform = (RectTransform)new GameObject(name, new Type[] { typeof(RectTransform) })
			{
				layer = parent.gameObject.layer
			}.transform;
			rectTransform.SetParent(parent, false);
			CharacterPresetPanelController.RosterPlace(rectTransform, min, max, pivot, position, size);
			return rectTransform;
		}

		// Token: 0x060039B6 RID: 14774 RVA: 0x00153567 File Offset: 0x00151767
		private static void RosterPlace(RectTransform rect, Vector2 min, Vector2 max, Vector2 pivot, Vector2 position, Vector2 size)
		{
			rect.anchorMin = min;
			rect.anchorMax = max;
			rect.pivot = pivot;
			rect.sizeDelta = size;
			rect.anchoredPosition = position;
		}

		// Token: 0x060039B7 RID: 14775 RVA: 0x0015358E File Offset: 0x0015178E
		private static Image RosterBackground(RectTransform rect, Color color)
		{
			Image image = rect.gameObject.AddComponent<Image>();
			image.color = color;
			return image;
		}

		// Token: 0x060039B8 RID: 14776 RVA: 0x001535A4 File Offset: 0x001517A4
		private TextMeshProUGUI RosterLabel(string name, Transform parent, string text, float size, Color color)
		{
			TextMeshProUGUI textMeshProUGUI = CharacterPresetPanelController.RosterRect(name, parent, Vector2.zero, Vector2.one, new Vector2(0.5f, 0.5f), Vector2.zero, Vector2.zero).gameObject.AddComponent<TextMeshProUGUI>();
			textMeshProUGUI.font = this._rosterFont;
			textMeshProUGUI.text = text;
			textMeshProUGUI.fontSize = size;
			textMeshProUGUI.color = color;
			textMeshProUGUI.alignment = TextAlignmentOptions.MidlineLeft;
			textMeshProUGUI.enableWordWrapping = false;
			textMeshProUGUI.overflowMode = TextOverflowModes.Ellipsis;
			textMeshProUGUI.raycastTarget = false;
			if (!string.IsNullOrEmpty(text) && text != "Option")
			{
				LocalizedGameText localizedGameText = LocalizedGameText.BindSource(textMeshProUGUI, "cielos_apps", text, null);
				if (localizedGameText != null)
				{
					localizedGameText.FitTranslationToRectangle(false);
				}
			}
			else
			{
				LocalizedGameText.BindFontOnly(textMeshProUGUI);
			}
			return textMeshProUGUI;
		}

		// Token: 0x060039B9 RID: 14777 RVA: 0x00153664 File Offset: 0x00151864
		private Button RosterButton(string name, Transform parent, string text, UnityAction click)
		{
			RectTransform rectTransform = CharacterPresetPanelController.RosterRect(name, parent, Vector2.zero, Vector2.one, new Vector2(0.5f, 0.5f), Vector2.zero, Vector2.zero);
			Image image = CharacterPresetPanelController.RosterBackground(rectTransform, CharacterPresetPanelController.RosterControl);
			Button button = rectTransform.gameObject.AddComponent<Button>();
			button.targetGraphic = image;
			ColorBlock colors = button.colors;
			colors.highlightedColor = new Color(1.17f, 1.12f, 1.17f, 1f);
			colors.pressedColor = new Color(0.8f, 0.8f, 0.8f, 1f);
			button.colors = colors;
			this.RosterLabel("Label", rectTransform, text, 17f, CharacterPresetPanelController.RosterPale).alignment = TextAlignmentOptions.Center;
			button.onClick.AddListener(click);
			return button;
		}

		// Token: 0x040039E3 RID: 14819
		[Header("References")]
		[SerializeField]
		private CharactersManager charactersManager;

		// Token: 0x040039E4 RID: 14820
		[SerializeField]
		private CustomizationNavigationRouter navigationRouter;

		// Token: 0x040039E5 RID: 14821
		[SerializeField]
		private CustomizationHubPresetListController hubPresetListController;

		// Token: 0x040039E6 RID: 14822
		[SerializeField]
		private CharacterStateManager previewStateManager;

		// Token: 0x040039E7 RID: 14823
		[Header("Grid UI")]
		[SerializeField]
		private Transform presetGridContainer;

		// Token: 0x040039E8 RID: 14824
		[SerializeField]
		private CharacterCard presetCardPrefab;

		// Token: 0x040039E9 RID: 14825
		[SerializeField]
		private ScrollRect presetScrollRect;

		// Token: 0x040039EA RID: 14826
		[Tooltip("Override scroll sensitivity at runtime. 0 = keep scene value.")]
		[SerializeField]
		private float scrollSensitivityOverride = 25f;

		// Token: 0x040039EB RID: 14827
		[Tooltip("How many preset cards to instantiate per page (infinite scroll batch size).")]
		[SerializeField]
		private int presetsPerPage = 20;

		// Token: 0x040039EC RID: 14828
		[Tooltip("Append the next page when the vertical scroll falls below this normalized position (0 = bottom).")]
		[SerializeField]
		private float infiniteScrollThreshold = 0.15f;

		// Token: 0x040039ED RID: 14829
		[Tooltip("Initial card creation budget per frame. Keeps large NPC lists responsive.")]
		[SerializeField]
		private int cardsPerFrame = 4;

		// Token: 0x040039EE RID: 14830
		[Header("Action UI")]
		[SerializeField]
		private TMP_InputField presetNameInput;

		// Token: 0x040039EF RID: 14831
		[SerializeField]
		private Button createPresetButton;

		// Token: 0x040039F0 RID: 14832
		[SerializeField]
		private Button createSystemPresetButton;

		// Token: 0x040039F1 RID: 14833
		[SerializeField]
		private Button duplicatePresetButton;

		// Token: 0x040039F2 RID: 14834
		[SerializeField]
		private Button deletePresetButton;

		// Token: 0x040039F3 RID: 14835
		[SerializeField]
		private Button refreshButton;

		// Token: 0x040039F4 RID: 14836
		[SerializeField]
		private TextMeshProUGUI selectedTargetLabel;

		// Token: 0x040039F5 RID: 14837
		[SerializeField]
		private TextMeshProUGUI selectedPresetLabel;

		// Token: 0x040039F6 RID: 14838
		[Header("New Preset Draft")]
		[Tooltip("Used as base preview when opening a NEW NPC preset draft flow.")]
		[SerializeField]
		private CharacterObject npcNewPresetBaseAsset;

		// Token: 0x040039F7 RID: 14839
		[SerializeField]
		private bool verboseDraftFlowLogs;

		// Token: 0x040039F8 RID: 14840
		[Tooltip("If true, Add/Create button opens draft flow instead of immediate save.")]
		[SerializeField]
		private bool createButtonOpensDraftFlow = true;

		// Token: 0x040039F9 RID: 14841
		[Header("Context Menu (Right-Click)")]
		[SerializeField]
		private ContextMenuManager presetContextMenuManager;

		// Token: 0x040039FA RID: 14842
		[SerializeField]
		private Sprite contextMenuEditIcon;

		// Token: 0x040039FB RID: 14843
		[SerializeField]
		private Sprite contextMenuUseIcon;

		// Token: 0x040039FC RID: 14844
		[SerializeField]
		private string contextMenuDuplicateLabel = "Duplicate";

		// Token: 0x040039FD RID: 14845
		[SerializeField]
		private Sprite contextMenuDuplicateIcon;

		// Token: 0x040039FE RID: 14846
		[SerializeField]
		private string contextMenuDeleteLabel = "Delete";

		// Token: 0x040039FF RID: 14847
		[SerializeField]
		private Sprite contextMenuDeleteIcon;

		// Token: 0x04003A00 RID: 14848
		[Header("Action Confirmation (Delete)")]
		[SerializeField]
		private ModalWindowManager presetActionConfirmModal;

		// Token: 0x04003A01 RID: 14849
		[SerializeField]
		private string deleteConfirmTitle = "Confirm deletion";

		// Token: 0x04003A02 RID: 14850
		[SerializeField]
		private string deleteConfirmDescriptionTemplate = "Permanently delete '{0}'?";

		// Token: 0x04003A03 RID: 14851
		[Header("Set In Use (MC Only)")]
		[SerializeField]
		private bool enableMainCharacterHoldToSetInUse = true;

		// Token: 0x04003A04 RID: 14852
		[SerializeField]
		private float holdToSetInUseStartDelaySeconds = 0.4f;

		// Token: 0x04003A05 RID: 14853
		[SerializeField]
		private float holdToSetInUseDurationSeconds = 0.85f;

		// Token: 0x04003A06 RID: 14854
		[SerializeField]
		private ProgressBar holdProgressBar;

		// Token: 0x04003A07 RID: 14855
		[SerializeField]
		private GameObject holdProgressRoot;

		// Token: 0x04003A08 RID: 14856
		[Header("NPC Filter (NPC tab only)")]
		[Tooltip("Riga UI col filtro genere + ricerca. Visibile solo sul tab NPC.")]
		[SerializeField]
		private GameObject npcFilterRoot;

		// Token: 0x04003A09 RID: 14857
		[Tooltip("Toggle MUIP 'Male' (default ON).")]
		[SerializeField]
		private SwitchManager maleFilterToggle;

		// Token: 0x04003A0A RID: 14858
		[Tooltip("Toggle MUIP 'Female' (default ON).")]
		[SerializeField]
		private SwitchManager femaleFilterToggle;

		// Token: 0x04003A0B RID: 14859
		[Tooltip("Campo ricerca per nome/cognome.")]
		[SerializeField]
		private TMP_InputField npcSearchInput;

		// Token: 0x04003A0C RID: 14860
		private readonly List<CustomizationPresetData> _currentPresets = new List<CustomizationPresetData>();

		// Token: 0x04003A0D RID: 14861
		private readonly List<CustomizationPresetData> _filteredPresets = new List<CustomizationPresetData>();

		// Token: 0x04003A0E RID: 14862
		private bool _maleFilterOn = true;

		// Token: 0x04003A0F RID: 14863
		private bool _femaleFilterOn = true;

		// Token: 0x04003A10 RID: 14864
		private string _npcSearch = string.Empty;

		// Token: 0x04003A11 RID: 14865
		private bool _npcFilterBound;

		// Token: 0x04003A12 RID: 14866
		private readonly Dictionary<string, CharacterCard> _spawnedCards = new Dictionary<string, CharacterCard>();

		// Token: 0x04003A13 RID: 14867
		private int _renderedCount;

		// Token: 0x04003A14 RID: 14868
		private bool _scrollListenerBound;

		// Token: 0x04003A15 RID: 14869
		private RectTransform _presetInstructionsRoot;

		// Token: 0x04003A16 RID: 14870
		private TextMeshProUGUI _presetInstructionsLabel;

		// Token: 0x04003A17 RID: 14871
		private RectTransform _presetInstructionsScrollRect;

		// Token: 0x04003A18 RID: 14872
		private Vector2 _presetInstructionsOriginalScrollOffsetMin;

		// Token: 0x04003A19 RID: 14873
		private Vector2 _presetInstructionsOriginalScrollOffsetMax;

		// Token: 0x04003A1A RID: 14874
		private float _presetInstructionsLastWidth = -1f;

		// Token: 0x04003A1B RID: 14875
		private bool _instructionsController;

		// Token: 0x04003A1C RID: 14876
		private int _instructionsGlyphVersion = -1;

		// Token: 0x04003A1D RID: 14877
		private Coroutine _gridBuildCoroutine;

		// Token: 0x04003A1F RID: 14879
		private string _currentCharacterId;

		// Token: 0x04003A20 RID: 14880
		private string _selectedPresetId;

		// Token: 0x04003A21 RID: 14881
		private CustomizationTargetType _currentTargetType;

		// Token: 0x04003A22 RID: 14882
		private bool _isHoldingSetInUse;

		// Token: 0x04003A23 RID: 14883
		private float _holdStartTimeUnscaled;

		// Token: 0x04003A24 RID: 14884
		private CustomizationPresetData _holdPreset;

		// Token: 0x04003A25 RID: 14885
		private bool _holdDelayPassed;

		// Token: 0x04003A26 RID: 14886
		private bool _suppressNextPresetClick;

		// Token: 0x04003A27 RID: 14887
		private bool _suppressNextPointerDrivenClick;

		// Token: 0x04003A28 RID: 14888
		private bool _holdProgressInputConfigured;

		// Token: 0x04003A29 RID: 14889
		private bool _confirmModalEventsBound;

		// Token: 0x04003A2A RID: 14890
		private CharacterPresetPanelController.PendingPresetAction _pendingPresetAction;

		// Token: 0x04003A2B RID: 14891
		private CustomizationPresetData _pendingActionPreset;

		// Token: 0x04003A2C RID: 14892
		private CustomizationPresetData _contextMenuTargetPreset;

		// Token: 0x04003A2D RID: 14893
		private static readonly Color RosterInk = new Color(0.19f, 0.11f, 0.25f, 1f);

		// Token: 0x04003A2E RID: 14894
		private static readonly Color RosterControl = new Color(0.25f, 0.16f, 0.32f, 1f);

		// Token: 0x04003A2F RID: 14895
		private static readonly Color RosterPink = new Color(0.894f, 0.349f, 0.467f, 1f);

		// Token: 0x04003A30 RID: 14896
		private static readonly Color RosterPale = new Color(1f, 0.757f, 0.835f, 1f);

		// Token: 0x04003A31 RID: 14897
		private static readonly Color RosterMuted = new Color(0.79f, 0.73f, 0.83f, 1f);

		// Token: 0x04003A32 RID: 14898
		private const float RosterToolbarHeight = 112f;

		// Token: 0x04003A33 RID: 14899
		private const float RosterFooterHeight = 90f;

		// Token: 0x04003A34 RID: 14900
		private const float RosterWarningFooterHeight = 120f;

		// Token: 0x04003A35 RID: 14901
		private int _npcOriginFilter;

		// Token: 0x04003A36 RID: 14902
		private int _npcStatusFilter;

		// Token: 0x04003A37 RID: 14903
		private RectTransform _rosterToolbar;

		// Token: 0x04003A38 RID: 14904
		private RectTransform _rosterFooter;

		// Token: 0x04003A39 RID: 14905
		private TextMeshProUGUI _rosterCount;

		// Token: 0x04003A3A RID: 14906
		private TextMeshProUGUI _rosterCustomCount;

		// Token: 0x04003A3B RID: 14907
		private TextMeshProUGUI _rosterAvailableCount;

		// Token: 0x04003A3C RID: 14908
		private TextMeshProUGUI _rosterMaleCount;

		// Token: 0x04003A3D RID: 14909
		private TextMeshProUGUI _rosterFemaleCount;

		// Token: 0x04003A3E RID: 14910
		private TextMeshProUGUI _rosterStatus;

		// Token: 0x04003A3F RID: 14911
		private TextMeshProUGUI _rosterEmpty;

		// Token: 0x04003A40 RID: 14912
		private Button _rosterFill;

		// Token: 0x04003A41 RID: 14913
		private TMP_FontAsset _rosterFont;

		// Token: 0x04003A42 RID: 14914
		private GridLayoutGroup _rosterGrid;

		// Token: 0x04003A43 RID: 14915
		private Vector2 _rosterOriginalCellSize;

		// Token: 0x04003A44 RID: 14916
		private RectTransform _rosterScroll;

		// Token: 0x04003A45 RID: 14917
		private Vector2 _rosterScrollMin;

		// Token: 0x04003A46 RID: 14918
		private Vector2 _rosterScrollMax;

		// Token: 0x04003A47 RID: 14919
		private bool _rosterLayoutWasVisible;

		// Token: 0x04003A48 RID: 14920
		private int _rosterMinimum = 20;

		// Token: 0x04003A49 RID: 14921
		private TMP_Text _rosterHeading;

		// Token: 0x04003A4A RID: 14922
		private string _rosterOriginalHeading;

		// Token: 0x02000868 RID: 2152
		private enum PendingPresetAction
		{
			// Token: 0x04003A4C RID: 14924
			None,
			// Token: 0x04003A4D RID: 14925
			Delete
		}
	}
}
