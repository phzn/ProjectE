using System;
using System.Collections.Generic;
using BoZo.CharacterCreator.MC.StyleSystem;
using BoZo.CharacterCreator.MC.StyleSystem.UI;
using Features.Localization;
using Michsky.MUIP;
using ProjectExhibitionist.CharacterCustomization.Core;
using ProjectExhibitionist.CharacterCustomization.Core.Runtime;
using ProjectExhibitionist.SkinDecals.UI;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Bozo.AnimeCharacters
{
	// Token: 0x02000863 RID: 2147
	public class CharacterCustomizationController : MonoBehaviour
	{
		// Token: 0x1400005F RID: 95
		// (add) Token: 0x060038E2 RID: 14562 RVA: 0x0014B9CC File Offset: 0x00149BCC
		// (remove) Token: 0x060038E3 RID: 14563 RVA: 0x0014BA04 File Offset: 0x00149C04
		public event Action<CharacterSubCategorySO> OnSubcategoryChanged;

		// Token: 0x14000060 RID: 96
		// (add) Token: 0x060038E4 RID: 14564 RVA: 0x0014BA3C File Offset: 0x00149C3C
		// (remove) Token: 0x060038E5 RID: 14565 RVA: 0x0014BA74 File Offset: 0x00149C74
		public event Action<CharacterPartSO> OnPartChanged;

		// Token: 0x14000061 RID: 97
		// (add) Token: 0x060038E6 RID: 14566 RVA: 0x0014BAAC File Offset: 0x00149CAC
		// (remove) Token: 0x060038E7 RID: 14567 RVA: 0x0014BAE4 File Offset: 0x00149CE4
		public event Action<ConfigTabSO> OnConfigTabChanged;

		// Token: 0x060038E8 RID: 14568 RVA: 0x0014BB19 File Offset: 0x00149D19
		public List<CharacterSubCategorySO> GetSubCategories()
		{
			return this.subCategories;
		}

		// Token: 0x060038E9 RID: 14569 RVA: 0x0014BB24 File Offset: 0x00149D24
		public void SelectSubCategory(CharacterSubCategorySO subCategory)
		{
			if (subCategory == null)
			{
				Debug.LogError("Cannot select null subcategory");
				return;
			}
			if (!this.subCategories.Contains(subCategory))
			{
				Debug.LogWarning("Subcategory " + subCategory.subCategoryName + " is not in the list of available subcategories");
				return;
			}
			if (this.subCategorySelector != null)
			{
				this.subCategorySelector.SelectSubCategory(subCategory);
				return;
			}
			this.OnSubCategorySelected(subCategory);
		}

		// Token: 0x060038EA RID: 14570 RVA: 0x0014BB90 File Offset: 0x00149D90
		protected virtual void Start()
		{
			this.stateManager = Object.FindObjectOfType<CharacterStateManager>();
			if (this.stateManager == null)
			{
				Debug.LogWarning("CharacterStateManager non trovato nella scena. Alcune funzionalità potrebbero non funzionare correttamente.");
			}
			this.InitializeController();
		}

		// Token: 0x060038EB RID: 14571 RVA: 0x0014BBBB File Offset: 0x00149DBB
		protected virtual void OnEnable()
		{
			if (this.stateManager != null)
			{
				this.InitializeController();
			}
		}

		// Token: 0x060038EC RID: 14572 RVA: 0x0014BBD4 File Offset: 0x00149DD4
		protected virtual void OnDisable()
		{
			this.CloseEditorColorPicker();
			if (this.mcStylePanelController != null)
			{
				this.mcStylePanelController.Hide();
			}
			if (this.decalPanelController != null)
			{
				this.decalPanelController.Hide();
			}
			if (this.accessoryPanelController != null)
			{
				this.accessoryPanelController.Hide();
			}
		}

		// Token: 0x060038ED RID: 14573 RVA: 0x0014BC32 File Offset: 0x00149E32
		private void LateUpdate()
		{
			this.EnsureConfiguredTabButtonsVisible();
		}

		// Token: 0x060038EE RID: 14574 RVA: 0x0014BC3C File Offset: 0x00149E3C
		private void InitializeController()
		{
			this.ResolveCameraController();
			this.InitializeOutfitTypeMap();
			if (this.subCategorySelector != null)
			{
				this.subCategorySelector.Init(this.subCategories, new Action<CharacterSubCategorySO>(this.OnSubCategorySelected));
				return;
			}
			if (this.subCategories != null && this.subCategories.Count > 0)
			{
				this.OnSubCategorySelected(this.subCategories[0]);
			}
		}

		// Token: 0x060038EF RID: 14575 RVA: 0x0014BCAC File Offset: 0x00149EAC
		private void InitializeOutfitTypeMap()
		{
			this.outfitTypeMap.Clear();
			if (this.availableOutfitTypes != null)
			{
				foreach (OutfitType outfitType in this.availableOutfitTypes)
				{
					if (outfitType != null && !this.outfitTypeMap.ContainsKey(outfitType.TypeName))
					{
						this.outfitTypeMap.Add(outfitType.TypeName, outfitType);
					}
				}
			}
		}

		// Token: 0x060038F0 RID: 14576 RVA: 0x0014BD14 File Offset: 0x00149F14
		private OutfitType GetOutfitTypeByName(string typeName)
		{
			OutfitType outfitType;
			if (this.outfitTypeMap.TryGetValue(typeName, out outfitType))
			{
				return outfitType;
			}
			return null;
		}

		// Token: 0x060038F1 RID: 14577 RVA: 0x0014BD34 File Offset: 0x00149F34
		private void CloseEditorColorPicker()
		{
			if (!this.editorColorPickerResolved)
			{
				this.editorColorPickerResolved = true;
				Transform transform = base.transform;
				while (transform != null && this.editorColorPicker == null)
				{
					this.editorColorPicker = transform.GetComponentInChildren<ColorPickerControlExtended>(true);
					transform = transform.parent;
				}
			}
			if (this.editorColorPicker != null && this.editorColorPicker.IsOpen)
			{
				this.editorColorPicker.Close();
			}
		}

		// Token: 0x060038F2 RID: 14578 RVA: 0x0014BDAC File Offset: 0x00149FAC
		private void OnSubCategorySelected(CharacterSubCategorySO subCategory)
		{
			this.CloseEditorColorPicker();
			this.currentSubCategory = subCategory;
			CustomizationScrollResetter.ResetToTopNextFrame(this, base.transform);
			CharacterSpinner[] array = Object.FindObjectsOfType<CharacterSpinner>(true);
			float num = (string.Equals(subCategory.subCategoryName, "Butt", StringComparison.OrdinalIgnoreCase) ? 165f : 0f);
			for (int i = 0; i < array.Length; i++)
			{
				if (array[i] != null && array[i].isActiveAndEnabled && array[i].character != null)
				{
					array[i].TweenToContextYaw(num, 0.35f);
				}
			}
			this.ApplyContextCamera(subCategory, null);
			Action<CharacterSubCategorySO> onSubcategoryChanged = this.OnSubcategoryChanged;
			if (onSubcategoryChanged != null)
			{
				onSubcategoryChanged(subCategory);
			}
			if (!subCategory.hasParts)
			{
				this.partsSelectorContainer.SetActive(false);
				this.currentPart = null;
				this.SetupConfigTabManager(subCategory.configTabs);
				ConfigTabSO configTabSO;
				if (subCategory.configTabs == null)
				{
					configTabSO = null;
				}
				else
				{
					configTabSO = subCategory.configTabs.Find((ConfigTabSO t) => t != null);
				}
				ConfigTabSO configTabSO2 = configTabSO;
				if (configTabSO2 != null)
				{
					this.OnConfigTabSelected(configTabSO2);
				}
				if (this.stateManager != null)
				{
					StyleItemSO subcategoryStyle = this.stateManager.GetSubcategoryStyle(this.categoryName, subCategory);
					if (subcategoryStyle != null && this.styleSelector != null)
					{
						this.styleSelector.ShowStyleItems(subcategoryStyle.itemType);
					}
				}
				return;
			}
			this.SetupPartSelector(subCategory.parts);
			if (subCategory.parts != null && subCategory.parts.Count > 0)
			{
				this.OnPartSelected(subCategory.parts[0]);
				return;
			}
			Debug.LogWarning("Sottocategoria " + subCategory.subCategoryName + " ha hasParts=true ma nessuna parte nella lista!");
		}

		// Token: 0x060038F3 RID: 14579 RVA: 0x0014BF60 File Offset: 0x0014A160
		private void SetupPartSelector(List<CharacterPartSO> parts)
		{
			this.partsSelectorContainer.SetActive(true);
			this.partSelector.onValueChanged.RemoveAllListeners();
			this.partSelector.items.Clear();
			foreach (CharacterPartSO characterPartSO in parts)
			{
				this.partSelector.CreateNewItem(characterPartSO.partName);
			}
			this.partSelector.defaultIndex = 0;
			this.partSelector.SetupSelector();
			CustomizationHorizontalSelectorLocalization.Bind(this.partSelector);
			this.ResetPartSelectorToIdle();
			this.partSelector.UpdateContentLayout();
			this.partSelector.onValueChanged.AddListener(new UnityAction<int>(this.OnPartSelectorChanged));
		}

		// Token: 0x060038F4 RID: 14580 RVA: 0x0014C034 File Offset: 0x0014A234
		private void ResetPartSelectorToIdle()
		{
			if (this.partSelector == null)
			{
				return;
			}
			Animator selectorAnimator = this.partSelector.selectorAnimator;
			if (selectorAnimator == null || selectorAnimator.runtimeAnimatorController == null)
			{
				return;
			}
			selectorAnimator.enabled = true;
			selectorAnimator.Play("Idle", 0, 0f);
			selectorAnimator.Update(0f);
		}

		// Token: 0x060038F5 RID: 14581 RVA: 0x0014C097 File Offset: 0x0014A297
		private void OnPartSelectorChanged(int index)
		{
			if (this.currentSubCategory != null && this.currentSubCategory.parts.Count > index && index >= 0)
			{
				this.OnPartSelected(this.currentSubCategory.parts[index]);
			}
		}

		// Token: 0x060038F6 RID: 14582 RVA: 0x0014C0D8 File Offset: 0x0014A2D8
		private void OnPartSelected(CharacterPartSO part)
		{
			this.CloseEditorColorPicker();
			this.currentPart = part;
			this.ApplyContextCamera(this.currentSubCategory, part);
			this.SetupConfigTabManager(part.configTabs);
			Action<CharacterPartSO> onPartChanged = this.OnPartChanged;
			if (onPartChanged != null)
			{
				onPartChanged(part);
			}
			if (part.configTabs.Count > 0)
			{
				this.OnConfigTabSelected(part.configTabs[0]);
			}
			if (this.stateManager != null)
			{
				StyleItemSO partStyle = this.stateManager.GetPartStyle(this.categoryName, this.currentSubCategory, part);
				if (partStyle != null && this.styleSelector != null)
				{
					this.styleSelector.ShowStyleItems(partStyle.itemType);
				}
			}
		}

		// Token: 0x060038F7 RID: 14583 RVA: 0x0014C18C File Offset: 0x0014A38C
		private void ResolveCameraController()
		{
			if (this.cameraController != null)
			{
				return;
			}
			Transform transform = base.transform;
			while (transform != null && this.cameraController == null)
			{
				this.cameraController = transform.GetComponentInChildren<CameraController>(true);
				transform = transform.parent;
			}
		}

		// Token: 0x060038F8 RID: 14584 RVA: 0x0014C1DC File Offset: 0x0014A3DC
		private void ApplyContextCamera(CharacterSubCategorySO subCategory, CharacterPartSO part)
		{
			this.ResolveCameraController();
			if (this.cameraController == null || subCategory == null)
			{
				return;
			}
			if (subCategory.cameraPositionIndex >= 0f)
			{
				this.cameraController.TweenToContext(subCategory.cameraPositionIndex, subCategory.cameraVerticalOffset);
				return;
			}
			string text = string.Concat(new string[]
			{
				this.categoryName,
				" ",
				subCategory.subCategoryName,
				" ",
				(part != null) ? part.partName : string.Empty
			}).ToLowerInvariant();
			float num;
			float num2;
			if (CharacterCustomizationController.ContainsAny(text, new string[] { "feet", "foot", "shoe", "boot", "sock" }))
			{
				num = 0.4f;
				num2 = -0.9f;
			}
			else if (CharacterCustomizationController.ContainsAny(text, new string[] { "bottom", "leg", "pants", "trouser", "skirt", "underwear", "swimsuit" }))
			{
				num = 0.4f;
				num2 = -0.45f;
			}
			else if (CharacterCustomizationController.ContainsAny(text, new string[] { "hand", "glove", "arm" }))
			{
				num = 0.46f;
				num2 = 0f;
			}
			else if (CharacterCustomizationController.ContainsAny(text, new string[] { "top", "hoodie", "shirt", "coat", "jacket", "torso", "chest", "breast" }))
			{
				num = 0.48f;
				num2 = 0.05f;
			}
			else if (CharacterCustomizationController.ContainsAny(text, new string[]
			{
				"head", "hair", "eye", "brow", "mouth", "ear", "hat", "mask", "glass", "blindfold",
				"stuff"
			}))
			{
				num = 0.6f;
				num2 = 0.2f;
			}
			else
			{
				num = 0.18f;
				num2 = 0f;
			}
			this.cameraController.TweenToContext(num, num2);
		}

		// Token: 0x060038F9 RID: 14585 RVA: 0x0014C43C File Offset: 0x0014A63C
		private static bool ContainsAny(string value, params string[] tokens)
		{
			for (int i = 0; i < tokens.Length; i++)
			{
				if (value.Contains(tokens[i]))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x060038FA RID: 14586 RVA: 0x0014C468 File Offset: 0x0014A668
		private void SetupConfigTabManager(List<ConfigTabSO> tabs)
		{
			List<ConfigTabSO> list = new List<ConfigTabSO>();
			if (tabs != null)
			{
				for (int i = 0; i < tabs.Count; i++)
				{
					if (tabs[i] != null)
					{
						list.Add(tabs[i]);
					}
				}
				if (list.Count != tabs.Count)
				{
					Debug.LogWarning(string.Format("[CustomizationController] {0} configTab null ignorate (asset mancante?) in '{1}'.", tabs.Count - list.Count, this.categoryName));
				}
			}
			tabs = list;
			this.EnsureNpcColorsWindowForTabs(tabs);
			this.configTabManager.gameObject.SetActive(true);
			this.configTabManager.onWindowChange.RemoveAllListeners();
			this.HideAllTabButtons();
			this.ShowTabButtons(tabs);
			if (tabs.Count > 0)
			{
				int num = this.ResolveWindowIndexForTab(tabs[0]);
				if (num >= 0 && num < this.configTabManager.windows.Count)
				{
					this.SetActiveConfigWindow(num);
				}
				else
				{
					Debug.LogWarning("Tab " + tabs[0].tabName + " not found in index map, using first available window");
					this.SetActiveConfigWindow(0);
				}
			}
			this.configTabManager.onWindowChange.AddListener(new UnityAction<int>(this.OnConfigTabSelectorChanged));
		}

		// Token: 0x060038FB RID: 14587 RVA: 0x0014C594 File Offset: 0x0014A794
		private void SetActiveConfigWindow(int windowIndex)
		{
			if (this.configTabManager == null || this.configTabManager.windows == null || this.configTabManager.windows.Count == 0)
			{
				return;
			}
			this.configTabManager.StopAllCoroutines();
			int num = Mathf.Clamp(windowIndex, 0, this.configTabManager.windows.Count - 1);
			this.configTabManager.currentWindowIndex = num;
			for (int i = 0; i < this.configTabManager.windows.Count; i++)
			{
				WindowManager.WindowItem windowItem = this.configTabManager.windows[i];
				if (windowItem != null && !(windowItem.windowObject == null))
				{
					windowItem.windowObject.SetActive(i == num);
					if (i == num)
					{
						Animator component = windowItem.windowObject.GetComponent<Animator>();
						if (component != null && windowItem.windowObject.activeInHierarchy)
						{
							component.Play("In");
						}
						if (windowItem.buttonObject != null)
						{
							Animator component2 = windowItem.buttonObject.GetComponent<Animator>();
							if (component2 != null && windowItem.buttonObject.activeInHierarchy)
							{
								component2.Play("Hover to Pressed");
							}
						}
					}
				}
			}
		}

		// Token: 0x060038FC RID: 14588 RVA: 0x0014C6C8 File Offset: 0x0014A8C8
		private void HideAllTabButtons()
		{
			this.visibleConfigWindowIndices.Clear();
			foreach (WindowManager.WindowItem windowItem in this.configTabManager.windows)
			{
				if (windowItem.buttonObject != null)
				{
					windowItem.buttonObject.SetActive(false);
				}
			}
		}

		// Token: 0x060038FD RID: 14589 RVA: 0x0014C740 File Offset: 0x0014A940
		private void ShowTabButtons(List<ConfigTabSO> tabs)
		{
			if (tabs == null || this.configTabManager == null || this.configTabManager.windows == null)
			{
				return;
			}
			foreach (ConfigTabSO configTabSO in tabs)
			{
				int num = this.ResolveWindowIndexForTab(configTabSO);
				if (num >= 0 && num < this.configTabManager.windows.Count)
				{
					this.visibleConfigWindowIndices.Add(num);
					WindowManager.WindowItem windowItem = this.configTabManager.windows[num];
					if (windowItem != null && windowItem.buttonObject != null)
					{
						windowItem.buttonObject.SetActive(true);
						CharacterCustomizationController.RestoreTabButtonPresentation(windowItem.buttonObject);
					}
				}
			}
		}

		// Token: 0x060038FE RID: 14590 RVA: 0x0014C80C File Offset: 0x0014AA0C
		private void EnsureConfiguredTabButtonsVisible()
		{
			if (this.configTabManager == null || !this.configTabManager.gameObject.activeInHierarchy || this.configTabManager.windows == null)
			{
				return;
			}
			for (int i = 0; i < this.configTabManager.windows.Count; i++)
			{
				WindowManager.WindowItem windowItem = this.configTabManager.windows[i];
				if (windowItem != null && !(windowItem.buttonObject == null))
				{
					bool flag = this.visibleConfigWindowIndices.Contains(i);
					if (windowItem.buttonObject.activeSelf != flag)
					{
						windowItem.buttonObject.SetActive(flag);
					}
					if (flag)
					{
						CharacterCustomizationController.RestoreTabButtonPresentation(windowItem.buttonObject);
					}
				}
			}
		}

		// Token: 0x060038FF RID: 14591 RVA: 0x0014C8BC File Offset: 0x0014AABC
		private static void RestoreTabButtonPresentation(GameObject buttonObject)
		{
			if (buttonObject == null)
			{
				return;
			}
			buttonObject.transform.localScale = Vector3.one;
			CanvasGroup[] componentsInChildren = buttonObject.GetComponentsInChildren<CanvasGroup>(true);
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				componentsInChildren[i].alpha = 1f;
				componentsInChildren[i].interactable = true;
				componentsInChildren[i].blocksRaycasts = true;
			}
		}

		// Token: 0x06003900 RID: 14592 RVA: 0x0014C918 File Offset: 0x0014AB18
		public void OnConfigTabSelectorChanged(int tabIndex)
		{
			if (tabIndex < 0)
			{
				Debug.LogError(string.Format("Invalid window index: {0}", tabIndex));
				return;
			}
			ConfigTabSO.ConfigType? configType = this.MapWindowIndexToConfigType(tabIndex);
			string text = this.ResolveWindowName(tabIndex);
			ConfigTabSO configTabSO = null;
			List<ConfigTabSO> list;
			if (this.currentSubCategory.hasParts && this.currentPart != null)
			{
				list = this.currentPart.configTabs;
			}
			else
			{
				list = this.currentSubCategory.configTabs;
			}
			foreach (ConfigTabSO configTabSO2 in list)
			{
				if (configType != null && configTabSO2.configType == configType.Value)
				{
					configTabSO = configTabSO2;
					break;
				}
			}
			if (configTabSO == null)
			{
				for (int i = 0; i < list.Count; i++)
				{
					ConfigTabSO configTabSO3 = list[i];
					if (!(configTabSO3 == null) && (configTabSO3.tabName.Equals(text, StringComparison.OrdinalIgnoreCase) || (string.Equals(text, "Expressions", StringComparison.OrdinalIgnoreCase) && string.Equals(configTabSO3.tabName, "Expression", StringComparison.OrdinalIgnoreCase)) || (string.Equals(text, "Expression", StringComparison.OrdinalIgnoreCase) && string.Equals(configTabSO3.tabName, "Expressions", StringComparison.OrdinalIgnoreCase))))
					{
						configTabSO = configTabSO3;
						break;
					}
				}
			}
			if (configTabSO != null)
			{
				this.OnConfigTabSelected(configTabSO);
				return;
			}
			Debug.LogWarning("No matching tab found for name: " + text + " in available tabs");
			if (list.Count > 0)
			{
				this.OnConfigTabSelected(list[0]);
			}
		}

		// Token: 0x06003901 RID: 14593 RVA: 0x0014CAAC File Offset: 0x0014ACAC
		private int ResolveWindowIndexForTab(ConfigTabSO tab)
		{
			if (tab != null && this.configTabManager != null && this.configTabManager.windows != null)
			{
				if (tab.configType == ConfigTabSO.ConfigType.Colors)
				{
					for (int i = 0; i < this.configTabManager.windows.Count; i++)
					{
						if (CharacterCustomizationController.IsNpcColorsWindow(this.configTabManager.windows[i]))
						{
							return i;
						}
					}
					return -1;
				}
				for (int j = 0; j < this.configTabManager.windows.Count; j++)
				{
					WindowManager.WindowItem windowItem = this.configTabManager.windows[j];
					if (windowItem != null && !string.IsNullOrWhiteSpace(windowItem.windowName) && string.Equals(windowItem.windowName, tab.tabName, StringComparison.OrdinalIgnoreCase))
					{
						return j;
					}
				}
			}
			if (!(tab != null))
			{
				return -1;
			}
			return this.MapConfigTypeToWindowIndex(tab.configType);
		}

		// Token: 0x06003902 RID: 14594 RVA: 0x0014CB90 File Offset: 0x0014AD90
		private int MapConfigTypeToWindowIndex(ConfigTabSO.ConfigType configType)
		{
			switch (configType)
			{
			case ConfigTabSO.ConfigType.Style:
				return 0;
			case ConfigTabSO.ConfigType.Shapes:
				return 1;
			case ConfigTabSO.ConfigType.Expressions:
				return 2;
			case ConfigTabSO.ConfigType.Colors:
				if (this.configTabManager != null && this.configTabManager.windows != null)
				{
					for (int i = 0; i < this.configTabManager.windows.Count; i++)
					{
						if (CharacterCustomizationController.IsNpcColorsWindow(this.configTabManager.windows[i]))
						{
							return i;
						}
					}
				}
				return -1;
			case ConfigTabSO.ConfigType.Decals:
				return 3;
			case ConfigTabSO.ConfigType.Accessories:
				return 4;
			}
			return -1;
		}

		// Token: 0x06003903 RID: 14595 RVA: 0x0014CC20 File Offset: 0x0014AE20
		private ConfigTabSO.ConfigType? MapWindowIndexToConfigType(int windowIndex)
		{
			if (this.configTabManager != null && this.configTabManager.windows != null && windowIndex >= 0 && windowIndex < this.configTabManager.windows.Count && CharacterCustomizationController.IsNpcColorsWindow(this.configTabManager.windows[windowIndex]))
			{
				return new ConfigTabSO.ConfigType?(ConfigTabSO.ConfigType.Colors);
			}
			switch (windowIndex)
			{
			case 0:
				return new ConfigTabSO.ConfigType?(ConfigTabSO.ConfigType.Style);
			case 1:
				return new ConfigTabSO.ConfigType?(ConfigTabSO.ConfigType.Shapes);
			case 2:
				return new ConfigTabSO.ConfigType?(ConfigTabSO.ConfigType.Expressions);
			case 3:
				return new ConfigTabSO.ConfigType?(ConfigTabSO.ConfigType.Decals);
			case 4:
				return new ConfigTabSO.ConfigType?(ConfigTabSO.ConfigType.Accessories);
			default:
				return null;
			}
		}

		// Token: 0x06003904 RID: 14596 RVA: 0x0014CCC8 File Offset: 0x0014AEC8
		private string ResolveWindowName(int windowIndex)
		{
			if (this.configTabManager != null && this.configTabManager.windows != null && windowIndex >= 0 && windowIndex < this.configTabManager.windows.Count && this.configTabManager.windows[windowIndex] != null && !string.IsNullOrWhiteSpace(this.configTabManager.windows[windowIndex].windowName))
			{
				return this.configTabManager.windows[windowIndex].windowName;
			}
			switch (windowIndex)
			{
			case 0:
				return "Style";
			case 1:
				return "Shapes";
			case 2:
				return "Expressions";
			case 3:
				return "Decals";
			case 4:
				return "Accessories";
			default:
				return "Unknown";
			}
		}

		// Token: 0x06003905 RID: 14597 RVA: 0x0014CD90 File Offset: 0x0014AF90
		private void OnConfigTabSelected(ConfigTabSO tab)
		{
			this.CloseEditorColorPicker();
			this.currentTab = tab;
			if (tab.configType != ConfigTabSO.ConfigType.Style && this.mcStylePanelController != null)
			{
				this.mcStylePanelController.Hide();
			}
			if (tab.configType != ConfigTabSO.ConfigType.Decals && this.decalPanelController != null)
			{
				this.decalPanelController.Hide();
			}
			if (tab.configType != ConfigTabSO.ConfigType.Accessories && this.accessoryPanelController != null)
			{
				this.accessoryPanelController.Hide();
			}
			Action<ConfigTabSO> onConfigTabChanged = this.OnConfigTabChanged;
			if (onConfigTabChanged != null)
			{
				onConfigTabChanged(tab);
			}
			switch (tab.configType)
			{
			case ConfigTabSO.ConfigType.Style:
				this.SetupStyleSelector();
				return;
			case ConfigTabSO.ConfigType.Shapes:
			case ConfigTabSO.ConfigType.Expressions:
			case ConfigTabSO.ConfigType.Colors:
			case ConfigTabSO.ConfigType.Custom:
				break;
			case ConfigTabSO.ConfigType.Decals:
				if (this.decalPanelController != null)
				{
					this.decalPanelController.Show();
					return;
				}
				break;
			case ConfigTabSO.ConfigType.Accessories:
				if (this.IsNpcContext())
				{
					if (this.accessoryPanelController != null)
					{
						this.accessoryPanelController.Hide();
					}
					this.SetupStyleSelector();
					return;
				}
				if (this.accessoryPanelController != null)
				{
					this.accessoryPanelController.ShowForGroup((this.currentSubCategory != null) ? this.currentSubCategory.subCategoryName : string.Empty);
				}
				break;
			default:
				return;
			}
		}

		// Token: 0x06003906 RID: 14598 RVA: 0x0014CED0 File Offset: 0x0014B0D0
		private void EnsureNpcColorsWindowForTabs(List<ConfigTabSO> tabs)
		{
			if (this.npcColorsWindowInjected || tabs == null || this.configTabManager == null || !this.IsNpcContext())
			{
				return;
			}
			ConfigTabSO configTabSO = null;
			for (int i = 0; i < tabs.Count; i++)
			{
				if (tabs[i] != null && tabs[i].configType == ConfigTabSO.ConfigType.Colors)
				{
					configTabSO = tabs[i];
					break;
				}
			}
			if (configTabSO == null || string.IsNullOrWhiteSpace(configTabSO.tabName))
			{
				return;
			}
			for (int j = 0; j < this.configTabManager.windows.Count; j++)
			{
				if (CharacterCustomizationController.IsNpcColorsWindow(this.configTabManager.windows[j]))
				{
					this.npcColorsWindowInjected = true;
					return;
				}
			}
			if (this.configTabManager.windows.Count == 0)
			{
				return;
			}
			WindowManager.WindowItem windowItem = this.configTabManager.windows[0];
			if (windowItem == null || windowItem.windowObject == null || windowItem.buttonObject == null)
			{
				Debug.LogWarning("[CustomizationController] Template window/button mancante: impossibile iniettare la tab Colors.");
				return;
			}
			Transform transform = this.FindSiblingByNameUpwards("Colors_Category");
			ColorManager colorManager = ((transform != null) ? transform.GetComponent<ColorManager>() : null);
			Transform transform2 = ((transform != null) ? transform.Find("Content") : null);
			if (colorManager == null || transform2 == null)
			{
				Debug.LogWarning("[CustomizationController] Colors_Category/ColorManager non trovati: tab Colors non disponibile.");
				return;
			}
			GameObject gameObject = Object.Instantiate<GameObject>(windowItem.windowObject, windowItem.windowObject.transform.parent);
			gameObject.name = "__NPC_BODY_STYLE_COLORS";
			gameObject.SetActive(false);
			for (int k = gameObject.transform.childCount - 1; k >= 0; k--)
			{
				Object.Destroy(gameObject.transform.GetChild(k).gameObject);
			}
			GameObject gameObject2 = Object.Instantiate<GameObject>(transform2.gameObject, gameObject.transform);
			gameObject2.name = "Content";
			gameObject2.SetActive(true);
			RectTransform rectTransform = gameObject2.transform as RectTransform;
			if (rectTransform != null)
			{
				rectTransform.anchorMin = Vector2.zero;
				rectTransform.anchorMax = Vector2.one;
				rectTransform.offsetMin = Vector2.zero;
				rectTransform.offsetMax = Vector2.zero;
			}
			Transform transform3 = gameObject2.transform.Find("SCROLLVIEW/VIEWPORT/Colors_Container");
			if (transform3 == null)
			{
				Debug.LogWarning("[CustomizationController] Colors_Container non trovato nel clone: tab Colors non disponibile.");
				Object.Destroy(gameObject);
				return;
			}
			ColorManager colorManager2 = gameObject.AddComponent<ColorManager>();
			CharacterCreatorManager componentInParent = base.GetComponentInParent<CharacterCreatorManager>(true);
			OutfitSystem outfitSystem = ((componentInParent != null) ? componentInParent.Character : null);
			if (outfitSystem == null)
			{
				CustomizationHubPresetListController customizationHubPresetListController = Object.FindObjectOfType<CustomizationHubPresetListController>(true);
				if (customizationHubPresetListController != null && customizationHubPresetListController.NpcPreviewTarget != null)
				{
					outfitSystem = customizationHubPresetListController.NpcPreviewTarget.GetComponentInChildren<OutfitSystem>(true);
				}
			}
			colorManager2.ConfigureRuntime(colorManager.PreviewPrefab, transform3, colorManager.Picker, colorManager.PickerPanel, true, outfitSystem);
			GameObject gameObject3 = Object.Instantiate<GameObject>(windowItem.buttonObject, windowItem.buttonObject.transform.parent);
			gameObject3.name = configTabSO.tabName + " Button";
			gameObject3.SetActive(false);
			CharacterCustomizationController.SetInjectedTabButtonLabel(gameObject3, configTabSO.tabName);
			string windowName = "__NPC_BODY_STYLE_COLORS";
			ButtonManager component = gameObject3.GetComponent<ButtonManager>();
			if (component != null)
			{
				component.buttonText = configTabSO.tabName;
				component.onClick.RemoveAllListeners();
				component.onClick.AddListener(delegate
				{
					this.configTabManager.OpenPanel(windowName);
				});
			}
			Button component2 = gameObject3.GetComponent<Button>();
			if (component2 != null)
			{
				component2.onClick.RemoveAllListeners();
				component2.onClick.AddListener(delegate
				{
					this.configTabManager.OpenPanel(windowName);
				});
			}
			WindowManager.WindowItem windowItem2 = new WindowManager.WindowItem
			{
				windowName = windowName,
				windowObject = gameObject,
				buttonObject = gameObject3
			};
			this.configTabManager.windows.Add(windowItem2);
			this.npcColorsWindowInjected = true;
		}

		// Token: 0x06003907 RID: 14599 RVA: 0x0014D2D5 File Offset: 0x0014B4D5
		private static bool IsNpcColorsWindow(WindowManager.WindowItem window)
		{
			return window != null && (string.Equals(window.windowName, "__NPC_BODY_STYLE_COLORS", StringComparison.Ordinal) || (window.windowObject != null && window.windowObject.GetComponent<ColorManager>() != null));
		}

		// Token: 0x06003908 RID: 14600 RVA: 0x0014D314 File Offset: 0x0014B514
		private Transform FindSiblingByNameUpwards(string childName)
		{
			Transform transform = base.transform;
			while (transform != null)
			{
				Transform transform2 = transform.Find(childName);
				if (transform2 != null)
				{
					return transform2;
				}
				transform = transform.parent;
			}
			return null;
		}

		// Token: 0x06003909 RID: 14601 RVA: 0x0014D350 File Offset: 0x0014B550
		private static void SetInjectedTabButtonLabel(GameObject buttonObject, string label)
		{
			TMP_Text[] componentsInChildren = buttonObject.GetComponentsInChildren<TMP_Text>(true);
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				LocalizedGameText localizedGameText = LocalizedGameText.BindSource(componentsInChildren[i], "cielos_apps", label, null);
				if (localizedGameText != null)
				{
					localizedGameText.FitTranslationToRectangle(false);
				}
			}
		}

		// Token: 0x0600390A RID: 14602 RVA: 0x0014D38E File Offset: 0x0014B58E
		private bool IsNpcContext()
		{
			return this.stateManager != null && this.stateManager.ActiveTargetContext == CustomizationTargetType.Npc;
		}

		// Token: 0x0600390B RID: 14603 RVA: 0x0014D3B0 File Offset: 0x0014B5B0
		private void SetupStyleSelector()
		{
			bool flag = this.IsNpcContext();
			if (!flag && this.mcStylePanelController != null)
			{
				MCStylePartDefinitionSO mcstylePartDefinitionSO = this.ResolveStylePartDefinition();
				if (mcstylePartDefinitionSO != null)
				{
					this.mcStylePanelController.SetCompactCardPrefab((this.accessoryPanelController != null) ? this.accessoryPanelController.CardPrefabTemplate : null);
					this.mcStylePanelController.ShowForPart(mcstylePartDefinitionSO);
					return;
				}
				this.mcStylePanelController.Hide();
			}
			else if (flag && this.mcStylePanelController != null)
			{
				this.mcStylePanelController.Hide();
			}
			if (this.styleSelector == null)
			{
				Debug.LogError("Style selector is not assigned");
				return;
			}
			if (!flag && this.styleSelector.TryShowStyleItemsForContext(this.categoryName, this.currentSubCategory, this.currentPart))
			{
				return;
			}
			OutfitType outfitType = this.MapToOutfitType(this.currentSubCategory, this.currentPart);
			bool flag2 = flag && (string.Equals(this.categoryName, "Outfit", StringComparison.OrdinalIgnoreCase) || (outfitType != null && string.Equals(outfitType.name, "LowerFace", StringComparison.OrdinalIgnoreCase)));
			this.styleSelector.ShowStyleItems(outfitType, flag2);
		}

		// Token: 0x0600390C RID: 14604 RVA: 0x0014D4DC File Offset: 0x0014B6DC
		private MCStylePartDefinitionSO ResolveStylePartDefinition()
		{
			if (this.currentPart != null && this.currentPart.stylePartDefinition != null)
			{
				return this.currentPart.stylePartDefinition;
			}
			if (this.currentSubCategory != null && this.currentSubCategory.stylePartDefinition != null)
			{
				return this.currentSubCategory.stylePartDefinition;
			}
			return null;
		}

		// Token: 0x0600390D RID: 14605 RVA: 0x00080C11 File Offset: 0x0007EE11
		private OutfitType LogAndReturnOutfitType(OutfitType outfitType, string reason = "")
		{
			return outfitType;
		}

		// Token: 0x0600390E RID: 14606 RVA: 0x0014D544 File Offset: 0x0014B744
		private OutfitType MapToOutfitType(CharacterSubCategorySO subCategory, CharacterPartSO part = null)
		{
			if (subCategory == null)
			{
				return this.GetOutfitTypeByName("Body");
			}
			string text = subCategory.subCategoryName.ToLower();
			if (part != null)
			{
				string partName = part.partName;
			}
			if (this.categoryName.ToLower() == "head")
			{
				if (text.Contains("hair"))
				{
					if (part != null)
					{
						string text2 = part.partName.ToLower();
						if (text2.Contains("front"))
						{
							return this.LogAndReturnOutfitType(this.GetOutfitTypeByName("HairFront"), "(hair front part)");
						}
						if (text2.Contains("back"))
						{
							return this.LogAndReturnOutfitType(this.GetOutfitTypeByName("HairBack"), "(hair back part)");
						}
					}
					return this.LogAndReturnOutfitType(this.GetOutfitTypeByName("HairFront"), "(default hair)");
				}
				if (text.Contains("eye"))
				{
					return this.LogAndReturnOutfitType(this.GetOutfitTypeByName("Eyes"), "(head eyes)");
				}
				if (text.Contains("lower face") || text.Contains("beard"))
				{
					return this.LogAndReturnOutfitType(this.GetOutfitTypeByName("LowerFace"), "(head lower face)");
				}
				if (text.Contains("upper face"))
				{
					return this.LogAndReturnOutfitType(this.GetOutfitTypeByName("UpperFace"), "(head upper face)");
				}
				if (text.Contains("face"))
				{
					return this.LogAndReturnOutfitType(this.GetOutfitTypeByName("LowerFace"), "(head default face)");
				}
			}
			else if (this.categoryName.ToLower() == "body")
			{
				if (text.Contains("torso") || text.Contains("chest"))
				{
					return this.LogAndReturnOutfitType(this.GetOutfitTypeByName("Top"), "(body torso/chest)");
				}
				if (text.Contains("leg") || text.Contains("lower"))
				{
					return this.LogAndReturnOutfitType(this.GetOutfitTypeByName("Bottom"), "(body legs/lower)");
				}
				if (text.Contains("full") || text.Contains("complete"))
				{
					return this.LogAndReturnOutfitType(this.GetOutfitTypeByName("Overall"), "(body full/complete)");
				}
			}
			else if (this.categoryName.ToLower() == "outfit")
			{
				if (text.Contains("top"))
				{
					return this.LogAndReturnOutfitType(this.GetOutfitTypeByName("Top"), "(outfit top)");
				}
				if (text.Contains("bottom"))
				{
					return this.LogAndReturnOutfitType(this.GetOutfitTypeByName("Bottom"), "(outfit bottom)");
				}
				if (text.Contains("feet") || text.Contains("shoes"))
				{
					return this.LogAndReturnOutfitType(this.GetOutfitTypeByName("Feet"), "(outfit feet/shoes)");
				}
				if (text.Contains("hat") || text.Contains("headwear"))
				{
					return this.LogAndReturnOutfitType(this.GetOutfitTypeByName("Hat"), "(outfit hat/headwear)");
				}
				if (text.Contains("gloves") || text.Contains("hands"))
				{
					return this.LogAndReturnOutfitType(this.GetOutfitTypeByName("Gloves"), "(outfit gloves/hands)");
				}
				if (text.Contains("headacc") || text.Contains("head items") || text.Contains("stuff"))
				{
					return this.LogAndReturnOutfitType(this.GetOutfitTypeByName("HeadAcc"), "(outfit head accessory)");
				}
				if (text.Contains("glasses") || text.Contains("eyewear"))
				{
					return this.LogAndReturnOutfitType(this.GetOutfitTypeByName("UpperFace"), "(outfit glasses/eyewear)");
				}
				if (text.Contains("neck"))
				{
					return this.LogAndReturnOutfitType(this.GetOutfitTypeByName("Neck"), "(outfit neck accessories)");
				}
				if (text.Contains("waist") || text.Contains("belt"))
				{
					return this.LogAndReturnOutfitType(this.GetOutfitTypeByName("Waist"), "(outfit waist/belt)");
				}
				if (text.Contains("overall") || text.Contains("full") || text.Contains("complete"))
				{
					return this.LogAndReturnOutfitType(this.GetOutfitTypeByName("Overall"), "(outfit overall/full)");
				}
				if (text.Contains("arms"))
				{
					return this.LogAndReturnOutfitType(this.GetOutfitTypeByName("Top"), "(outfit arms -> top)");
				}
				if (text.Contains("legs"))
				{
					return this.LogAndReturnOutfitType(this.GetOutfitTypeByName("Bottom"), "(outfit legs -> bottom)");
				}
			}
			if (text.Contains("body"))
			{
				return this.LogAndReturnOutfitType(this.GetOutfitTypeByName("Body"), "(generic body)");
			}
			if (text.Contains("top"))
			{
				return this.LogAndReturnOutfitType(this.GetOutfitTypeByName("Top"), "(generic top)");
			}
			if (text.Contains("bottom"))
			{
				return this.LogAndReturnOutfitType(this.GetOutfitTypeByName("Bottom"), "(generic bottom)");
			}
			if (text.Contains("overall") || text.Contains("full") || text.Contains("complete"))
			{
				return this.LogAndReturnOutfitType(this.GetOutfitTypeByName("Overall"), "(generic overall/full)");
			}
			return this.LogAndReturnOutfitType(this.GetOutfitTypeByName("Body"), "(default fallback)");
		}

		// Token: 0x0600390F RID: 14607 RVA: 0x0014DA6C File Offset: 0x0014BC6C
		public CharacterSubCategorySO GetCurrentSubCategory()
		{
			return this.currentSubCategory;
		}

		// Token: 0x06003910 RID: 14608 RVA: 0x0014DA74 File Offset: 0x0014BC74
		public CharacterPartSO GetCurrentPart()
		{
			return this.currentPart;
		}

		// Token: 0x06003911 RID: 14609 RVA: 0x0014DA7C File Offset: 0x0014BC7C
		public ConfigTabSO GetCurrentConfigTab()
		{
			return this.currentTab;
		}

		// Token: 0x06003912 RID: 14610 RVA: 0x0014DA84 File Offset: 0x0014BC84
		public string GetCategoryName()
		{
			return this.categoryName;
		}

		// Token: 0x06003913 RID: 14611 RVA: 0x0014DA8C File Offset: 0x0014BC8C
		public void SetCategoryName(string name)
		{
			this.categoryName = name;
		}

		// Token: 0x06003914 RID: 14612 RVA: 0x0014DA95 File Offset: 0x0014BC95
		public void SetSubCategories(List<CharacterSubCategorySO> categories)
		{
			if (categories == null || categories.Count == 0)
			{
				Debug.LogWarning("Tentativo di impostare una lista di sottocategorie vuota o nulla");
				return;
			}
			this.subCategories = categories;
		}

		// Token: 0x06003915 RID: 14613 RVA: 0x0014DAB4 File Offset: 0x0014BCB4
		public virtual void ResetToDefaults()
		{
			if (this.subCategorySelector != null)
			{
				this.subCategorySelector.Init(this.subCategories, new Action<CharacterSubCategorySO>(this.OnSubCategorySelected));
			}
			if (this.subCategories != null && this.subCategories.Count > 0)
			{
				CharacterSubCategorySO characterSubCategorySO = this.subCategories[0];
				this.OnSubCategorySelected(characterSubCategorySO);
				if (this.subCategorySelector != null)
				{
					this.subCategorySelector.SelectSubCategory(characterSubCategorySO);
				}
				else
				{
					Debug.LogWarning("SubCategorySelector è null!");
				}
				if (characterSubCategorySO.hasParts && characterSubCategorySO.parts != null && characterSubCategorySO.parts.Count > 0)
				{
					this.SetupPartSelector(characterSubCategorySO.parts);
					this.OnPartSelected(characterSubCategorySO.parts[0]);
				}
				if (characterSubCategorySO.configTabs != null && characterSubCategorySO.configTabs.Count > 0)
				{
					this.SetupConfigTabManager(characterSubCategorySO.configTabs);
					this.OnConfigTabSelected(characterSubCategorySO.configTabs[0]);
				}
				this.SetupStyleSelector();
			}
		}

		// Token: 0x040039B9 RID: 14777
		private CharacterStateManager stateManager;

		// Token: 0x040039BA RID: 14778
		[Header("Category Settings")]
		[Tooltip("Nome della categoria principale (es. Head, Body, Outfit)")]
		[SerializeField]
		protected string categoryName = "Category";

		// Token: 0x040039BB RID: 14779
		[Header("UI References")]
		[SerializeField]
		private SubCategorySelector subCategorySelector;

		// Token: 0x040039BC RID: 14780
		[SerializeField]
		private HorizontalSelector partSelector;

		// Token: 0x040039BD RID: 14781
		[SerializeField]
		private GameObject partsSelectorContainer;

		// Token: 0x040039BE RID: 14782
		[SerializeField]
		private WindowManager configTabManager;

		// Token: 0x040039BF RID: 14783
		[SerializeField]
		private StyleSelector styleSelector;

		// Token: 0x040039C0 RID: 14784
		[SerializeField]
		private MCStylePanelController mcStylePanelController;

		// Token: 0x040039C1 RID: 14785
		[SerializeField]
		private DecalCustomizationPanelController decalPanelController;

		// Token: 0x040039C2 RID: 14786
		[SerializeField]
		private AccessoryListPanelController accessoryPanelController;

		// Token: 0x040039C3 RID: 14787
		[Header("Data")]
		[SerializeField]
		protected List<CharacterSubCategorySO> subCategories;

		// Token: 0x040039C4 RID: 14788
		[Header("Outfit Types")]
		[SerializeField]
		private OutfitType[] availableOutfitTypes;

		// Token: 0x040039C5 RID: 14789
		private Dictionary<string, OutfitType> outfitTypeMap = new Dictionary<string, OutfitType>();

		// Token: 0x040039C6 RID: 14790
		private CharacterSubCategorySO currentSubCategory;

		// Token: 0x040039C7 RID: 14791
		private CharacterPartSO currentPart;

		// Token: 0x040039C8 RID: 14792
		private ConfigTabSO currentTab;

		// Token: 0x040039C9 RID: 14793
		private readonly HashSet<int> visibleConfigWindowIndices = new HashSet<int>();

		// Token: 0x040039CA RID: 14794
		[Header("Camera")]
		[SerializeField]
		private CameraController cameraController;

		// Token: 0x040039CB RID: 14795
		private ColorPickerControlExtended editorColorPicker;

		// Token: 0x040039CC RID: 14796
		private bool editorColorPickerResolved;

		// Token: 0x040039CD RID: 14797
		private bool npcColorsWindowInjected;

		// Token: 0x040039CE RID: 14798
		private const string NpcColorsWindowName = "__NPC_BODY_STYLE_COLORS";
	}
}
