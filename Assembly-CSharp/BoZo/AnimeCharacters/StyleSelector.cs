using System;
using System.Collections;
using System.Collections.Generic;
using BoZo.CharacterCreator.MC.StyleSystem;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Bozo.AnimeCharacters
{
	// Token: 0x020008AF RID: 2223
	public class StyleSelector : MonoBehaviour
	{
		// Token: 0x06003B8C RID: 15244 RVA: 0x00162040 File Offset: 0x00160240
		private void Awake()
		{
			this.stateManager = Object.FindObjectOfType<CharacterStateManager>();
			if (this.stateManager == null)
			{
				Debug.LogWarning("CharacterStateManager non trovato nella scena. Alcune funzionalità potrebbero non funzionare correttamente.");
			}
			this.headController = Object.FindObjectOfType<HeadCustomizationController>();
			if (this.mcStyleService == null)
			{
				this.mcStyleService = Object.FindObjectOfType<MCStyleCustomizationService>(true);
			}
			if (this.mcStyleCustomizationPanel == null)
			{
				this.mcStyleCustomizationPanel = Object.FindObjectOfType<MCStyleCustomizationPanelUI>(true);
			}
			if (this.mcStyleCustomizationPanel != null)
			{
				this.mcStyleCustomizationPanel.SetService(this.mcStyleService);
			}
			this._colorManager = Object.FindObjectOfType<ColorManager>(true);
		}

		// Token: 0x06003B8D RID: 15245 RVA: 0x001620DA File Offset: 0x001602DA
		private void OnEnable()
		{
			this.BindOutfitChanges();
		}

		// Token: 0x06003B8E RID: 15246 RVA: 0x001620E2 File Offset: 0x001602E2
		private void OnDisable()
		{
			this.UnbindOutfitChanges();
			this.CancelContextualRefresh();
			this.CloseContextualPicker();
		}

		// Token: 0x06003B8F RID: 15247 RVA: 0x001620F8 File Offset: 0x001602F8
		private void BindOutfitChanges()
		{
			OutfitSystem outfitSystem = ((!this._isMcStyleContext && this.styleManager != null) ? this.styleManager.OutfitSystem : null);
			if (this._observedOutfitSystem == outfitSystem)
			{
				return;
			}
			this.UnbindOutfitChanges();
			this._observedOutfitSystem = outfitSystem;
			if (this._observedOutfitSystem != null)
			{
				OutfitSystem observedOutfitSystem = this._observedOutfitSystem;
				observedOutfitSystem.OnOutfitChanged = (UnityAction<Outfit>)Delegate.Combine(observedOutfitSystem.OnOutfitChanged, new UnityAction<Outfit>(this.OnPreviewOutfitChanged));
			}
		}

		// Token: 0x06003B90 RID: 15248 RVA: 0x0016217B File Offset: 0x0016037B
		private void UnbindOutfitChanges()
		{
			if (this._observedOutfitSystem != null)
			{
				OutfitSystem observedOutfitSystem = this._observedOutfitSystem;
				observedOutfitSystem.OnOutfitChanged = (UnityAction<Outfit>)Delegate.Remove(observedOutfitSystem.OnOutfitChanged, new UnityAction<Outfit>(this.OnPreviewOutfitChanged));
			}
			this._observedOutfitSystem = null;
		}

		// Token: 0x06003B91 RID: 15249 RVA: 0x001621BC File Offset: 0x001603BC
		private void CloseContextualPicker()
		{
			if (this._colorManager != null && this._colorManager.Picker != null && this._colorManager.Picker.IsOpen)
			{
				this._colorManager.Picker.Close();
			}
		}

		// Token: 0x06003B92 RID: 15250 RVA: 0x0016220C File Offset: 0x0016040C
		private void CancelContextualRefresh()
		{
			if (this._contextualRefreshRoutine == null)
			{
				return;
			}
			base.StopCoroutine(this._contextualRefreshRoutine);
			this._contextualRefreshRoutine = null;
		}

		// Token: 0x06003B93 RID: 15251 RVA: 0x0016222C File Offset: 0x0016042C
		private void OnPreviewOutfitChanged(Outfit changedOutfit)
		{
			if (!base.isActiveAndEnabled || this._isMcStyleContext || this.itemType == null)
			{
				return;
			}
			if (changedOutfit != null && changedOutfit.Type != this.itemType)
			{
				return;
			}
			this.CloseContextualPicker();
			this.CancelContextualRefresh();
			this._contextualRefreshRoutine = base.StartCoroutine(this.RefreshLoadedOutfitNextFrame(this.itemType, this._observedOutfitSystem));
		}

		// Token: 0x06003B94 RID: 15252 RVA: 0x0016229F File Offset: 0x0016049F
		private IEnumerator RefreshLoadedOutfitNextFrame(OutfitType expectedType, OutfitSystem expectedTarget)
		{
			yield return null;
			this._contextualRefreshRoutine = null;
			if (!base.isActiveAndEnabled || this._isMcStyleContext || this.itemType != expectedType || this.styleManager == null || this.styleManager.OutfitSystem != expectedTarget)
			{
				yield break;
			}
			this.RefreshLoadedOutfitControls();
			yield break;
		}

		// Token: 0x06003B95 RID: 15253 RVA: 0x001622BC File Offset: 0x001604BC
		private void RefreshLoadedOutfitControls()
		{
			if (this.styleManager == null || this.styleManager.OutfitSystem == null || this.itemContainer == null)
			{
				return;
			}
			Outfit outfit = this.styleManager.OutfitSystem.GetOutfit(this.itemType);
			StyleItemSO styleItemSO = this.styleManager.GetEquippedStyle(this.itemType);
			if (styleItemSO == null && outfit != null)
			{
				List<StyleItemSO> stylesForType = this.styleManager.GetStylesForType(this.itemType);
				if (stylesForType.Count == 1)
				{
					styleItemSO = stylesForType[0];
				}
			}
			if (styleItemSO == null)
			{
				this.ClearActiveVisuals();
				return;
			}
			this.UpdateActiveVisuals(styleItemSO);
			this.RefreshContextualColorsForOutfit(styleItemSO, outfit, false);
		}

		// Token: 0x06003B96 RID: 15254 RVA: 0x00162378 File Offset: 0x00160578
		public bool TryShowStyleItemsForContext(string category, CharacterSubCategorySO subCategory, CharacterPartSO part)
		{
			if (this.mcStyleService == null)
			{
				return false;
			}
			MCStylePartDefinitionSO mcstylePartDefinitionSO = this.ResolveStylePartDefinitionForContext(category, subCategory, part);
			if (mcstylePartDefinitionSO == null)
			{
				return false;
			}
			this.ShowStyleItemsForPartDefinition(mcstylePartDefinitionSO);
			return true;
		}

		// Token: 0x06003B97 RID: 15255 RVA: 0x001623B2 File Offset: 0x001605B2
		private void Start()
		{
			if (this.showOnStart)
			{
				this.ShowStyleItems();
			}
		}

		// Token: 0x06003B98 RID: 15256 RVA: 0x001623C2 File Offset: 0x001605C2
		public void ShowStyleItems()
		{
			this.ShowStyleItems(this.itemType);
		}

		// Token: 0x06003B99 RID: 15257 RVA: 0x001623D0 File Offset: 0x001605D0
		public void ShowStyleItems(OutfitType type)
		{
			this.ShowStyleItems(type, false);
		}

		// Token: 0x06003B9A RID: 15258 RVA: 0x001623DC File Offset: 0x001605DC
		public void ShowStyleItems(OutfitType type, bool allowToggleOff)
		{
			this._isMcStyleContext = false;
			this.BindOutfitChanges();
			this._allowToggleOff = allowToggleOff;
			this._mcStyleIndexByItem.Clear();
			this.DestroyRuntimeMcStyleItems();
			this.itemType = type;
			this.ClearButtons();
			if (this.styleManager == null)
			{
				Debug.LogError("StyleManager non assegnato allo StyleSelector");
				return;
			}
			if (type == null)
			{
				Debug.LogWarning("[StyleSelector] Legacy Style fallback received null OutfitType. Configure MC stylePartDefinition/partId for this context.");
				if (this.mcStyleCustomizationPanel != null)
				{
					this.mcStyleCustomizationPanel.gameObject.SetActive(false);
				}
				return;
			}
			List<StyleItemSO> list = this.styleManager.GetStylesForType(type);
			if (list.Count == 0)
			{
				Debug.Log("Nessuno stile trovato per il tipo " + ((type != null) ? type.TypeName : "null"));
				return;
			}
			StyleItemSO equippedStyle = this.styleManager.GetEquippedStyle(type);
			if (equippedStyle != null && list.Contains(equippedStyle))
			{
				list = new List<StyleItemSO>(list);
				list.Remove(equippedStyle);
				list.Insert(0, equippedStyle);
			}
			foreach (StyleItemSO styleItemSO in list)
			{
				this.CreateStyleButton(styleItemSO);
			}
			this.WireRemoveOutfitButton(type);
			if (equippedStyle != null)
			{
				this.UpdateActiveVisuals(equippedStyle);
				this.RefreshContextualColors(equippedStyle);
				this.ScheduleContextualRefresh(equippedStyle, type);
			}
			else if (list.Count == 1 && this.styleManager.OutfitSystem != null && this.styleManager.OutfitSystem.GetOutfit(type) != null)
			{
				this.UpdateActiveVisuals(list[0]);
				this.RefreshContextualColorsForOutfit(list[0], this.styleManager.OutfitSystem.GetOutfit(type), true);
				this.ScheduleContextualRefresh(list[0], type);
			}
			if (this.mcStyleCustomizationPanel != null)
			{
				this.mcStyleCustomizationPanel.gameObject.SetActive(false);
			}
		}

		// Token: 0x06003B9B RID: 15259 RVA: 0x001625D0 File Offset: 0x001607D0
		private void WireRemoveOutfitButton(OutfitType type)
		{
			if (this.itemContainer == null || this.styleManager == null || type == null)
			{
				return;
			}
			Transform transform = this.itemContainer.Find("Remove_Outfit");
			if (transform == null)
			{
				return;
			}
			transform.gameObject.SetActive(!this._allowToggleOff);
			if (this._allowToggleOff)
			{
				return;
			}
			Button component = transform.GetComponent<Button>();
			if (component == null)
			{
				return;
			}
			component.onClick.RemoveAllListeners();
			OutfitType captured = type;
			component.onClick.AddListener(delegate
			{
				this.styleManager.RemoveStyle(captured);
				this.ClearActiveVisuals();
			});
		}

		// Token: 0x06003B9C RID: 15260 RVA: 0x00162684 File Offset: 0x00160884
		public void ShowStyleItemsForPartDefinition(MCStylePartDefinitionSO stylePartDefinition)
		{
			if (this.mcStyleService == null || stylePartDefinition == null)
			{
				Debug.LogWarning("[StyleSelector] MC style service or part definition missing, fallback to legacy style flow.");
				return;
			}
			this._isMcStyleContext = true;
			this.UnbindOutfitChanges();
			this._allowToggleOff = false;
			this._mcStyleIndexByItem.Clear();
			this.DestroyRuntimeMcStyleItems();
			this.ClearButtons();
			this.mcStyleService.SetActivePart(stylePartDefinition);
			int num = 0;
			if (stylePartDefinition.defaultBehavior == MCStyleDefaultBehavior.KeepSceneState)
			{
				StyleItemSO styleItemSO = ScriptableObject.CreateInstance<StyleItemSO>();
				styleItemSO.itemName = "Scene Default";
				styleItemSO.hideFlags = HideFlags.DontSave;
				this._runtimeMcStyleItems.Add(styleItemSO);
				this._mcStyleIndexByItem[styleItemSO] = num;
				this.CreateStyleButton(styleItemSO);
				num++;
			}
			for (int i = 0; i < stylePartDefinition.stylePresets.Count; i++)
			{
				MCStylePresetSO mcstylePresetSO = stylePartDefinition.stylePresets[i];
				if (!(mcstylePresetSO == null))
				{
					StyleItemSO styleItemSO2 = ScriptableObject.CreateInstance<StyleItemSO>();
					styleItemSO2.itemName = (string.IsNullOrWhiteSpace(mcstylePresetSO.displayName) ? mcstylePresetSO.name : mcstylePresetSO.displayName);
					styleItemSO2.icon = mcstylePresetSO.previewIcon;
					styleItemSO2.hideFlags = HideFlags.DontSave;
					this._runtimeMcStyleItems.Add(styleItemSO2);
					this._mcStyleIndexByItem[styleItemSO2] = num;
					this.CreateStyleButton(styleItemSO2);
					num++;
				}
			}
			this.UpdateActiveVisualsForMcState();
			if (this.mcStyleCustomizationPanel != null)
			{
				this.mcStyleCustomizationPanel.gameObject.SetActive(true);
				this.mcStyleCustomizationPanel.SetService(this.mcStyleService);
				this.mcStyleCustomizationPanel.Reopen();
			}
		}

		// Token: 0x06003B9D RID: 15261 RVA: 0x0016280C File Offset: 0x00160A0C
		private void CreateStyleButton(StyleItemSO style)
		{
			if (this.styleItemPrefab == null || this.itemContainer == null)
			{
				Debug.LogError("StyleItemPrefab o ItemContainer non assegnati");
				return;
			}
			if (style != null && style.icon == null && style.prefab != null)
			{
				return;
			}
			OutfitSelector outfitSelector = Object.Instantiate<OutfitSelector>(this.styleItemPrefab, this.itemContainer);
			this.instantiatedButtons.Add(outfitSelector.gameObject);
			outfitSelector.Init(style, this._isMcStyleContext ? null : this.styleManager, this._allowToggleOff);
			outfitSelector.OnStyleItemSelected.AddListener(new UnityAction<StyleItemSO>(this.OnStyleItemSelected));
		}

		// Token: 0x06003B9E RID: 15262 RVA: 0x001628C0 File Offset: 0x00160AC0
		private void OnStyleItemSelected(StyleItemSO style)
		{
			Debug.Log("Stile selezionato: " + style.itemName);
			if (this._isMcStyleContext)
			{
				int num;
				if (this.mcStyleService != null && style != null && this._mcStyleIndexByItem.TryGetValue(style, out num))
				{
					this.mcStyleService.SelectStyle(num);
				}
				this.UpdateActiveVisualsForMcState();
				if (this.mcStyleCustomizationPanel != null)
				{
					if (this.mcStyleCustomizationPanel.HasCustomizableOptions())
					{
						this.mcStyleCustomizationPanel.Reopen();
						return;
					}
					this.mcStyleCustomizationPanel.Refresh();
				}
				return;
			}
			StyleItemSO styleItemSO = ((this.styleManager != null) ? this.styleManager.GetEquippedStyle(style.itemType) : style);
			if (styleItemSO == null)
			{
				this.ClearActiveVisuals();
			}
			else
			{
				this.UpdateActiveVisuals(styleItemSO);
				this.RefreshContextualColors(styleItemSO);
				this.ScheduleContextualRefresh(styleItemSO, styleItemSO.itemType);
			}
			this.UpdateCharacterState(styleItemSO);
		}

		// Token: 0x06003B9F RID: 15263 RVA: 0x001629B0 File Offset: 0x00160BB0
		private void ClearActiveVisuals()
		{
			if (this.itemContainer == null)
			{
				return;
			}
			OutfitSelector[] componentsInChildren = this.itemContainer.GetComponentsInChildren<OutfitSelector>(true);
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				componentsInChildren[i].SetActiveVisual(false);
			}
		}

		// Token: 0x06003BA0 RID: 15264 RVA: 0x001629F0 File Offset: 0x00160BF0
		private void UpdateActiveVisuals(StyleItemSO selectedStyle)
		{
			if (selectedStyle == null)
			{
				return;
			}
			foreach (OutfitSelector outfitSelector in this.itemContainer.GetComponentsInChildren<OutfitSelector>(true))
			{
				StyleItemSO styleFromSelector = this.GetStyleFromSelector(outfitSelector);
				outfitSelector.SetActiveVisual(false);
				if (styleFromSelector != null && styleFromSelector == selectedStyle)
				{
					Debug.Log("Attivo lo stile " + styleFromSelector.itemName);
					outfitSelector.SetActiveVisual(true);
				}
			}
		}

		// Token: 0x06003BA1 RID: 15265 RVA: 0x00162A64 File Offset: 0x00160C64
		private void RefreshContextualColors(StyleItemSO selectedStyle)
		{
			if (this.styleManager == null || selectedStyle == null)
			{
				return;
			}
			Outfit outfit = ((this.styleManager.OutfitSystem != null) ? this.styleManager.OutfitSystem.GetOutfit(selectedStyle.itemType) : null);
			this.RefreshContextualColorsForOutfit(selectedStyle, outfit, true);
		}

		// Token: 0x06003BA2 RID: 15266 RVA: 0x00162AC0 File Offset: 0x00160CC0
		private void RefreshContextualColorsForOutfit(StyleItemSO selectedStyle, Outfit outfit, bool restoreTemporaryColors = true)
		{
			if (this._colorManager == null || selectedStyle == null || this.itemContainer == null || outfit == null)
			{
				return;
			}
			OutfitSelector[] componentsInChildren = this.itemContainer.GetComponentsInChildren<OutfitSelector>(true);
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				if (componentsInChildren[i].GetStyleItem() == selectedStyle && componentsInChildren[i].gameObject.activeInHierarchy)
				{
					this._colorManager.ShowContextualControls(componentsInChildren[i], outfit, restoreTemporaryColors);
					return;
				}
			}
		}

		// Token: 0x06003BA3 RID: 15267 RVA: 0x00162B47 File Offset: 0x00160D47
		private void ScheduleContextualRefresh(StyleItemSO selectedStyle, OutfitType expectedType)
		{
			if (!base.isActiveAndEnabled || selectedStyle == null)
			{
				return;
			}
			if (this._contextualRefreshRoutine != null)
			{
				base.StopCoroutine(this._contextualRefreshRoutine);
			}
			this._contextualRefreshRoutine = base.StartCoroutine(this.RefreshContextualControlsNextFrame(selectedStyle, expectedType));
		}

		// Token: 0x06003BA4 RID: 15268 RVA: 0x00162B83 File Offset: 0x00160D83
		private IEnumerator RefreshContextualControlsNextFrame(StyleItemSO selectedStyle, OutfitType expectedType)
		{
			yield return null;
			this._contextualRefreshRoutine = null;
			if (!base.isActiveAndEnabled || this.styleManager == null || this.itemType != expectedType)
			{
				yield break;
			}
			StyleItemSO equippedStyle = this.styleManager.GetEquippedStyle(expectedType);
			if (equippedStyle == selectedStyle)
			{
				this.RefreshContextualColors(equippedStyle);
			}
			yield break;
		}

		// Token: 0x06003BA5 RID: 15269 RVA: 0x00162BA0 File Offset: 0x00160DA0
		private StyleItemSO GetStyleFromSelector(OutfitSelector selector)
		{
			return selector.GetStyleItem();
		}

		// Token: 0x06003BA6 RID: 15270 RVA: 0x00162BA8 File Offset: 0x00160DA8
		private void UpdateCharacterState(StyleItemSO style)
		{
			if (this.stateManager == null)
			{
				return;
			}
			if (this.headController != null)
			{
				CharacterSubCategorySO currentSubCategory = this.headController.GetCurrentSubCategory();
				CharacterPartSO currentPart = this.headController.GetCurrentPart();
				if (currentPart != null && currentSubCategory != null)
				{
					this.stateManager.SetPartStyle("Head", currentSubCategory, currentPart, style);
					return;
				}
				if (currentPart != null)
				{
					this.stateManager.SetHeadPartStyle(currentPart, style);
					return;
				}
				if (currentSubCategory != null)
				{
					this.stateManager.SetHeadSubcategoryStyle(currentSubCategory, style);
				}
			}
		}

		// Token: 0x06003BA7 RID: 15271 RVA: 0x00162C40 File Offset: 0x00160E40
		private void UpdateSelectorsFromState()
		{
			if (this.stateManager == null)
			{
				return;
			}
			foreach (OutfitSelector outfitSelector in this.itemContainer.GetComponentsInChildren<OutfitSelector>(true))
			{
				StyleItemSO styleFromSelector = this.GetStyleFromSelector(outfitSelector);
				if (styleFromSelector != null)
				{
					bool flag = this.stateManager.IsStyleSelected(styleFromSelector);
					outfitSelector.SetActiveVisual(flag);
				}
			}
		}

		// Token: 0x06003BA8 RID: 15272 RVA: 0x00162CA4 File Offset: 0x00160EA4
		private void ClearButtons()
		{
			this.CancelContextualRefresh();
			foreach (GameObject gameObject in this.instantiatedButtons)
			{
				if (!(gameObject == null))
				{
					GameObject gameObject2 = gameObject;
					gameObject2.name += " (stale)";
					gameObject.SetActive(false);
					Object.Destroy(gameObject);
				}
			}
			this.instantiatedButtons.Clear();
		}

		// Token: 0x06003BA9 RID: 15273 RVA: 0x00162D30 File Offset: 0x00160F30
		private void DestroyRuntimeMcStyleItems()
		{
			for (int i = 0; i < this._runtimeMcStyleItems.Count; i++)
			{
				if (this._runtimeMcStyleItems[i] != null)
				{
					Object.Destroy(this._runtimeMcStyleItems[i]);
				}
			}
			this._runtimeMcStyleItems.Clear();
		}

		// Token: 0x06003BAA RID: 15274 RVA: 0x00162D84 File Offset: 0x00160F84
		private void UpdateActiveVisualsForMcState()
		{
			if (this.mcStyleService == null)
			{
				return;
			}
			int selectedStyleUiIndex = this.mcStyleService.GetSelectedStyleUiIndex();
			OutfitSelector[] componentsInChildren = this.itemContainer.GetComponentsInChildren<OutfitSelector>(true);
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				componentsInChildren[i].SetActiveVisual(i == selectedStyleUiIndex);
			}
		}

		// Token: 0x06003BAB RID: 15275 RVA: 0x00162DD4 File Offset: 0x00160FD4
		private MCStylePartDefinitionSO ResolveStylePartDefinitionForContext(string category, CharacterSubCategorySO subCategory, CharacterPartSO part)
		{
			if (part != null && part.stylePartDefinition != null)
			{
				return part.stylePartDefinition;
			}
			if (subCategory != null && subCategory.stylePartDefinition != null)
			{
				return subCategory.stylePartDefinition;
			}
			IReadOnlyList<MCStylePartDefinitionSO> partDefinitions = this.mcStyleService.PartDefinitions;
			if (partDefinitions == null || partDefinitions.Count == 0)
			{
				return null;
			}
			string text = (string.IsNullOrWhiteSpace(category) ? string.Empty : category.Trim());
			string text2 = ((subCategory != null && !string.IsNullOrWhiteSpace(subCategory.subCategoryName)) ? subCategory.subCategoryName.Trim() : string.Empty);
			string text3 = ((part != null && !string.IsNullOrWhiteSpace(part.partName)) ? part.partName.Trim() : string.Empty);
			List<string> list = new List<string>();
			if (!string.IsNullOrEmpty(text) && !string.IsNullOrEmpty(text2) && !string.IsNullOrEmpty(text3))
			{
				list.Add(string.Concat(new string[] { text, ".", text2, ".", text3 }));
			}
			if (!string.IsNullOrEmpty(text) && !string.IsNullOrEmpty(text2))
			{
				list.Add(text + "." + text2);
			}
			if (!string.IsNullOrEmpty(text2) && !string.IsNullOrEmpty(text3))
			{
				list.Add(text2 + "." + text3);
			}
			if (!string.IsNullOrEmpty(text2))
			{
				list.Add(text2);
			}
			for (int i = 0; i < list.Count; i++)
			{
				string text4 = list[i];
				for (int j = 0; j < partDefinitions.Count; j++)
				{
					MCStylePartDefinitionSO mcstylePartDefinitionSO = partDefinitions[j];
					if (!(mcstylePartDefinitionSO == null) && !string.IsNullOrWhiteSpace(mcstylePartDefinitionSO.partId) && string.Equals(mcstylePartDefinitionSO.partId.Trim(), text4, StringComparison.OrdinalIgnoreCase))
					{
						return mcstylePartDefinitionSO;
					}
				}
			}
			return null;
		}

		// Token: 0x04003BD9 RID: 15321
		[SerializeField]
		private OutfitSelector styleItemPrefab;

		// Token: 0x04003BDA RID: 15322
		[SerializeField]
		private Transform itemContainer;

		// Token: 0x04003BDB RID: 15323
		[SerializeField]
		private StyleManager styleManager;

		// Token: 0x04003BDC RID: 15324
		[SerializeField]
		private MCStyleCustomizationService mcStyleService;

		// Token: 0x04003BDD RID: 15325
		[SerializeField]
		private MCStyleCustomizationPanelUI mcStyleCustomizationPanel;

		// Token: 0x04003BDE RID: 15326
		[Header("Configurazione")]
		[SerializeField]
		private OutfitType itemType;

		// Token: 0x04003BDF RID: 15327
		[SerializeField]
		private bool showOnStart;

		// Token: 0x04003BE0 RID: 15328
		private CharacterStateManager stateManager;

		// Token: 0x04003BE1 RID: 15329
		private HeadCustomizationController headController;

		// Token: 0x04003BE2 RID: 15330
		private List<GameObject> instantiatedButtons = new List<GameObject>();

		// Token: 0x04003BE3 RID: 15331
		private readonly Dictionary<StyleItemSO, int> _mcStyleIndexByItem = new Dictionary<StyleItemSO, int>();

		// Token: 0x04003BE4 RID: 15332
		private readonly List<StyleItemSO> _runtimeMcStyleItems = new List<StyleItemSO>();

		// Token: 0x04003BE5 RID: 15333
		private bool _isMcStyleContext;

		// Token: 0x04003BE6 RID: 15334
		private bool _allowToggleOff;

		// Token: 0x04003BE7 RID: 15335
		private ColorManager _colorManager;

		// Token: 0x04003BE8 RID: 15336
		private Coroutine _contextualRefreshRoutine;

		// Token: 0x04003BE9 RID: 15337
		private OutfitSystem _observedOutfitSystem;
	}
}
