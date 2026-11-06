using System;
using System.Collections.Generic;
using Bozo.AnimeCharacters;
using Features.Localization;
using Michsky.MUIP;
using ProjectExhibitionist.CharacterCustomization.Core.Runtime;
using ProjectExhibitionist.Futanari;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace BoZo.CharacterCreator
{
	// Token: 0x020007B2 RID: 1970
	public class BlendShapeUIController : MonoBehaviour
	{
		// Token: 0x06003409 RID: 13321 RVA: 0x001280A0 File Offset: 0x001262A0
		private void Start()
		{
			this.InitializeCategoryMapping();
			if (this.headCustomizationController != null)
			{
				this.headCustomizationController.OnSubcategoryChanged += this.HandleHeadSubcategoryChanged;
				this.headCustomizationController.OnConfigTabChanged += this.HandleHeadConfigTabChanged;
			}
			if (this.bodyCustomizationController != null)
			{
				this.bodyCustomizationController.OnSubcategoryChanged += this.HandleBodySubcategoryChanged;
				this.bodyCustomizationController.OnConfigTabChanged += this.HandleBodyConfigTabChanged;
			}
			this.InitializeDefaultSelections();
		}

		// Token: 0x0600340A RID: 13322 RVA: 0x00128131 File Offset: 0x00126331
		private void OnEnable()
		{
			this.SubscribeToValueChanges();
			this.SyncCurrentValuesFromController();
			if (!string.IsNullOrEmpty(this.currentCategory) && !string.IsNullOrEmpty(this.currentSubcategory))
			{
				this.CreateUIControlsForCurrentSelection(false);
			}
			this.refreshValuesPending = true;
		}

		// Token: 0x0600340B RID: 13323 RVA: 0x00128167 File Offset: 0x00126367
		private void OnDisable()
		{
			this.UnsubscribeFromValueChanges();
		}

		// Token: 0x0600340C RID: 13324 RVA: 0x00128170 File Offset: 0x00126370
		private void SubscribeToValueChanges()
		{
			this.UnsubscribeFromValueChanges();
			if (this.blendShapeController == null)
			{
				return;
			}
			this.blendShapeController.AuthoredValuesChanged += this.RequestValueRefresh;
			this.subscribedManager = BlendShapeManager.ResolveForRig(this.blendShapeController);
			if (this.subscribedManager != null)
			{
				this.subscribedManager.OnBlendShapesApplied += this.RequestValueRefresh;
			}
		}

		// Token: 0x0600340D RID: 13325 RVA: 0x001281E0 File Offset: 0x001263E0
		private void UnsubscribeFromValueChanges()
		{
			if (this.blendShapeController != null)
			{
				this.blendShapeController.AuthoredValuesChanged -= this.RequestValueRefresh;
			}
			if (this.subscribedManager != null)
			{
				this.subscribedManager.OnBlendShapesApplied -= this.RequestValueRefresh;
			}
			this.subscribedManager = null;
		}

		// Token: 0x0600340E RID: 13326 RVA: 0x0012823E File Offset: 0x0012643E
		private void RequestValueRefresh()
		{
			this.refreshValuesPending = true;
		}

		// Token: 0x0600340F RID: 13327 RVA: 0x00128247 File Offset: 0x00126447
		private void LateUpdate()
		{
			if (!this.refreshValuesPending)
			{
				return;
			}
			this.refreshValuesPending = false;
			this.RefreshControlValues();
		}

		// Token: 0x06003410 RID: 13328 RVA: 0x00128260 File Offset: 0x00126460
		public void RefreshControlValues()
		{
			CharacterBlendShapeController characterBlendShapeController = this.blendShapeController;
			bool flag;
			if (characterBlendShapeController == null)
			{
				flag = null != null;
			}
			else
			{
				BlendShapeMapSO blendShapeMap = characterBlendShapeController.BlendShapeMap;
				flag = ((blendShapeMap != null) ? blendShapeMap.blendShapeMappings : null) != null;
			}
			if (!flag)
			{
				return;
			}
			foreach (BlendShapeMapSO.BlendShapeEntry blendShapeEntry in this.blendShapeController.BlendShapeMap.blendShapeMappings)
			{
				GameObject gameObject;
				if (blendShapeEntry != null && !string.IsNullOrEmpty(blendShapeEntry.displayName) && !(blendShapeEntry.category != this.currentCategory) && !(blendShapeEntry.subcategory != this.currentSubcategory) && this.activeControls.TryGetValue(blendShapeEntry.displayName, out gameObject) && !(gameObject == null))
				{
					float entryAuthoredUiValue = this.blendShapeController.GetEntryAuthoredUiValue(blendShapeEntry);
					float num;
					if (this.IsGateEntry(blendShapeEntry.displayName) && this.currentValues.TryGetValue(blendShapeEntry.displayName, out num) && num >= 0.5f != entryAuthoredUiValue >= 0.5f)
					{
						this.CreateUIControlsForCurrentSelection(true);
						break;
					}
					this.currentValues[blendShapeEntry.displayName] = entryAuthoredUiValue;
					Slider slider;
					if (this.activeSliders.TryGetValue(blendShapeEntry.displayName, out slider) && slider != null)
					{
						slider.SetValueWithoutNotify(entryAuthoredUiValue);
						this.UpdateValueText(gameObject, entryAuthoredUiValue, blendShapeEntry);
						BlendShapeSliderControlRefs componentInChildren = gameObject.GetComponentInChildren<BlendShapeSliderControlRefs>(true);
						if (componentInChildren != null && componentInChildren.ResetButton != null)
						{
							componentInChildren.ResetButton.gameObject.SetActive(!Mathf.Approximately(entryAuthoredUiValue, blendShapeEntry.GetUiDefaultValue()));
						}
					}
					else if (blendShapeEntry.IsBooleanControl())
					{
						Toggle componentInChildren2 = gameObject.GetComponentInChildren<Toggle>(true);
						if (componentInChildren2 != null)
						{
							componentInChildren2.SetIsOnWithoutNotify(entryAuthoredUiValue >= 0.5f);
						}
					}
					else if (blendShapeEntry.IsSelectControl())
					{
						HorizontalSelector componentInChildren3 = gameObject.GetComponentInChildren<HorizontalSelector>(true);
						if (componentInChildren3 != null && componentInChildren3.items.Count > 0)
						{
							int num2 = Mathf.Clamp(Mathf.RoundToInt(entryAuthoredUiValue), 0, componentInChildren3.items.Count - 1);
							if (componentInChildren3.index != num2 || componentInChildren3.defaultIndex != num2)
							{
								componentInChildren3.defaultIndex = num2;
								componentInChildren3.SetupSelector();
							}
						}
					}
				}
			}
		}

		// Token: 0x06003411 RID: 13329 RVA: 0x001284D4 File Offset: 0x001266D4
		private void InitializeCategoryMapping()
		{
			this.categorySubcategories.Clear();
			this.categorySubcategories["Head"] = new List<CharacterSubCategorySO>();
			this.categorySubcategories["Body"] = new List<CharacterSubCategorySO>();
			if (this.headCustomizationController != null)
			{
				List<CharacterSubCategorySO> subCategories = this.headCustomizationController.GetSubCategories();
				if (subCategories != null && subCategories.Count > 0)
				{
					using (List<CharacterSubCategorySO>.Enumerator enumerator = subCategories.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							CharacterSubCategorySO characterSubCategorySO = enumerator.Current;
							if (!(characterSubCategorySO == null))
							{
								this.categorySubcategories["Head"].Add(characterSubCategorySO);
							}
						}
						goto IL_00BA;
					}
				}
				Debug.LogWarning("No subcategories found in HeadCustomizationController!");
			}
			else
			{
				Debug.LogWarning("HeadCustomizationController is not assigned!");
			}
			IL_00BA:
			if (this.bodyCustomizationController != null)
			{
				List<CharacterSubCategorySO> subCategories2 = this.bodyCustomizationController.GetSubCategories();
				if (subCategories2 != null && subCategories2.Count > 0)
				{
					using (List<CharacterSubCategorySO>.Enumerator enumerator = subCategories2.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							CharacterSubCategorySO characterSubCategorySO2 = enumerator.Current;
							if (!(characterSubCategorySO2 == null))
							{
								this.categorySubcategories["Body"].Add(characterSubCategorySO2);
							}
						}
						return;
					}
				}
				Debug.LogWarning("No subcategories found in BodyCustomizationController!");
				return;
			}
			Debug.LogWarning("BodyCustomizationController is not assigned!");
		}

		// Token: 0x06003412 RID: 13330 RVA: 0x00128640 File Offset: 0x00126840
		private void InitializeDefaultSelections()
		{
			foreach (string text in this.categorySubcategories.Keys)
			{
				if (this.categorySubcategories[text].Count > 0)
				{
					this.SetCategory(text);
					break;
				}
			}
		}

		// Token: 0x06003413 RID: 13331 RVA: 0x001286B0 File Offset: 0x001268B0
		public void SetCategory(string category)
		{
			if (!string.IsNullOrEmpty(category) && this.categorySubcategories.ContainsKey(category))
			{
				this.currentCategory = category;
				this.SetDefaultSubcategory();
				return;
			}
			Debug.LogWarning("Invalid category: " + category);
		}

		// Token: 0x06003414 RID: 13332 RVA: 0x001286E8 File Offset: 0x001268E8
		private void OnDestroy()
		{
			this.UnsubscribeFromValueChanges();
			if (this.headCustomizationController != null)
			{
				this.headCustomizationController.OnSubcategoryChanged -= this.HandleHeadSubcategoryChanged;
				this.headCustomizationController.OnConfigTabChanged -= this.HandleHeadConfigTabChanged;
			}
			if (this.bodyCustomizationController != null)
			{
				this.bodyCustomizationController.OnSubcategoryChanged -= this.HandleBodySubcategoryChanged;
				this.bodyCustomizationController.OnConfigTabChanged -= this.HandleBodyConfigTabChanged;
			}
		}

		// Token: 0x06003415 RID: 13333 RVA: 0x00128774 File Offset: 0x00126974
		private void SetDefaultSubcategory()
		{
			if (string.IsNullOrEmpty(this.currentCategory) || !this.categorySubcategories.ContainsKey(this.currentCategory))
			{
				Debug.LogWarning("Cannot set default subcategory for invalid category: " + this.currentCategory);
				return;
			}
			List<CharacterSubCategorySO> list = this.categorySubcategories[this.currentCategory];
			if (list.Count <= 0)
			{
				Debug.LogWarning("No subcategories found for category: " + this.currentCategory);
				this.ClearSliders();
				return;
			}
			CharacterSubCategorySO characterSubCategorySO = list[0];
			this.currentSubcategory = characterSubCategorySO.subCategoryName;
			if (this.headCustomizationController != null)
			{
				this.headCustomizationController.SelectSubCategory(characterSubCategorySO);
				return;
			}
			this.SetDefaultConfigTab();
		}

		// Token: 0x06003416 RID: 13334 RVA: 0x00128823 File Offset: 0x00126A23
		private void SetDefaultConfigTab()
		{
			this.currentBlendShapeType = BlendShapeMapSO.BlendShapeType.Shape;
			this.CreateUIControlsForCurrentSelection(false);
		}

		// Token: 0x06003417 RID: 13335 RVA: 0x00128834 File Offset: 0x00126A34
		private void HandleHeadSubcategoryChanged(CharacterSubCategorySO subcategory)
		{
			if (subcategory == null)
			{
				return;
			}
			this.currentCategory = "Head";
			this.currentSubcategory = subcategory.subCategoryName;
			this.ClearSliders();
			if (subcategory.configTabs.Count > 0)
			{
				this.HandleHeadConfigTabChanged(subcategory.configTabs[0]);
				return;
			}
			Debug.LogWarning("No config tabs found for subcategory " + subcategory.subCategoryName + ", setting default config tab");
			this.SetDefaultConfigTab();
		}

		// Token: 0x06003418 RID: 13336 RVA: 0x001288AC File Offset: 0x00126AAC
		private void DetermineCategory(CharacterSubCategorySO subcategory)
		{
			if (subcategory == null)
			{
				return;
			}
			foreach (string text in this.categorySubcategories.Keys)
			{
				if (this.categorySubcategories[text].Contains(subcategory))
				{
					this.currentCategory = text;
					return;
				}
			}
			this.currentCategory = "Head";
		}

		// Token: 0x06003419 RID: 13337 RVA: 0x00128930 File Offset: 0x00126B30
		private void HandleHeadConfigTabChanged(ConfigTabSO configTab)
		{
			if (configTab == null)
			{
				return;
			}
			this.DeactivateAllContainers();
			BlendShapeMapSO.BlendShapeType blendShapeType = this.currentBlendShapeType;
			if (configTab.tabName.Equals("Shapes", StringComparison.OrdinalIgnoreCase))
			{
				this.currentBlendShapeType = BlendShapeMapSO.BlendShapeType.Shape;
			}
			else if (configTab.tabName.Equals("Expressions", StringComparison.OrdinalIgnoreCase))
			{
				this.currentBlendShapeType = BlendShapeMapSO.BlendShapeType.Expression;
			}
			else if (configTab.tabName.Equals("Style", StringComparison.OrdinalIgnoreCase))
			{
				this.currentBlendShapeType = BlendShapeMapSO.BlendShapeType.Color;
			}
			else
			{
				Debug.LogWarning("Unknown config tab: " + configTab.tabName + ", defaulting to Shape type");
				this.currentBlendShapeType = BlendShapeMapSO.BlendShapeType.Shape;
			}
			this.CreateUIControlsForCurrentSelection(false);
		}

		// Token: 0x0600341A RID: 13338 RVA: 0x001289D0 File Offset: 0x00126BD0
		public void ChangeCategory(string category)
		{
			if (string.IsNullOrEmpty(category) || !this.categorySubcategories.ContainsKey(category))
			{
				Debug.LogError("Invalid category: " + category);
				return;
			}
			if (this.currentCategory != category)
			{
				this.SetCategory(category);
			}
		}

		// Token: 0x0600341B RID: 13339 RVA: 0x00128A10 File Offset: 0x00126C10
		public void ChangeSubcategory(CharacterSubCategorySO subcategory)
		{
			if (subcategory == null)
			{
				Debug.LogError("Invalid subcategory: null");
				return;
			}
			string text = null;
			foreach (string text2 in this.categorySubcategories.Keys)
			{
				if (this.categorySubcategories[text2].Contains(subcategory))
				{
					text = text2;
					break;
				}
			}
			if (text == null)
			{
				Debug.LogError("Subcategory " + subcategory.subCategoryName + " not found in any category");
				return;
			}
			if (this.currentCategory != text)
			{
				this.SetCategory(text);
			}
			this.currentSubcategory = subcategory.subCategoryName;
			if (this.headCustomizationController != null)
			{
				this.headCustomizationController.SelectSubCategory(subcategory);
				return;
			}
			this.SetDefaultConfigTab();
		}

		// Token: 0x0600341C RID: 13340 RVA: 0x00128AF0 File Offset: 0x00126CF0
		public void ChangeSubcategoryByName(string subcategoryName)
		{
			if (string.IsNullOrEmpty(subcategoryName))
			{
				Debug.LogError("Invalid subcategory name: null or empty");
				return;
			}
			CharacterSubCategorySO characterSubCategorySO = null;
			foreach (string text in this.categorySubcategories.Keys)
			{
				foreach (CharacterSubCategorySO characterSubCategorySO2 in this.categorySubcategories[text])
				{
					if (characterSubCategorySO2.subCategoryName.Equals(subcategoryName, StringComparison.OrdinalIgnoreCase))
					{
						characterSubCategorySO = characterSubCategorySO2;
						break;
					}
				}
				if (characterSubCategorySO != null)
				{
					break;
				}
			}
			if (characterSubCategorySO == null)
			{
				Debug.LogError("No subcategory found with name: " + subcategoryName);
				return;
			}
			this.ChangeSubcategory(characterSubCategorySO);
		}

		// Token: 0x0600341D RID: 13341 RVA: 0x00128BDC File Offset: 0x00126DDC
		public void ChangeConfigType(BlendShapeMapSO.BlendShapeType configType)
		{
			if (this.currentBlendShapeType != configType)
			{
				this.currentBlendShapeType = configType;
				this.CreateUIControlsForCurrentSelection(false);
			}
		}

		// Token: 0x0600341E RID: 13342 RVA: 0x00128BF5 File Offset: 0x00126DF5
		private void HandleBodySubcategoryChanged(CharacterSubCategorySO subCategory)
		{
			if (subCategory == null)
			{
				return;
			}
			this.currentCategory = "Body";
			this.currentSubcategory = subCategory.subCategoryName;
			this.ClearSliders();
			this.CreateUIControlsForCurrentSelection(false);
		}

		// Token: 0x0600341F RID: 13343 RVA: 0x00128C28 File Offset: 0x00126E28
		private void HandleBodyConfigTabChanged(ConfigTabSO tab)
		{
			if (tab == null)
			{
				return;
			}
			this.ClearSliders();
			switch (tab.configType)
			{
			case ConfigTabSO.ConfigType.Style:
			case ConfigTabSO.ConfigType.Colors:
				this.currentBlendShapeType = BlendShapeMapSO.BlendShapeType.Color;
				break;
			case ConfigTabSO.ConfigType.Shapes:
				this.currentBlendShapeType = BlendShapeMapSO.BlendShapeType.Shape;
				break;
			case ConfigTabSO.ConfigType.Expressions:
				this.currentBlendShapeType = BlendShapeMapSO.BlendShapeType.Expression;
				break;
			default:
				this.DeactivateAllContainers();
				return;
			}
			this.CreateUIControlsForCurrentSelection(false);
		}

		// Token: 0x06003420 RID: 13344 RVA: 0x00128C90 File Offset: 0x00126E90
		private void CreateUIControlsForCurrentSelection(bool preserveScroll = false)
		{
			if (this.blendShapeController == null)
			{
				Debug.LogError("BlendShapeController is not assigned!");
				return;
			}
			float num = 1f;
			ScrollRect scrollRect = null;
			if (preserveScroll)
			{
				Transform containerForBlendShapeType = this.GetContainerForBlendShapeType(this.currentBlendShapeType);
				if (containerForBlendShapeType != null)
				{
					scrollRect = containerForBlendShapeType.GetComponentInParent<ScrollRect>(true);
					if (scrollRect != null)
					{
						num = scrollRect.verticalNormalizedPosition;
					}
				}
			}
			this.ClearSliders();
			if (string.IsNullOrEmpty(this.currentCategory))
			{
				Debug.LogError("Current category is null or empty!");
				return;
			}
			if (string.IsNullOrEmpty(this.currentSubcategory))
			{
				Debug.LogError("Current subcategory is null or empty!");
				return;
			}
			List<BlendShapeMapSO.BlendShapeEntry> availableBlendShapes = this.blendShapeController.GetAvailableBlendShapes(this.currentCategory, this.currentSubcategory, this.currentBlendShapeType);
			for (int i = 0; i < availableBlendShapes.Count; i++)
			{
				BlendShapeMapSO.BlendShapeEntry blendShapeEntry = availableBlendShapes[i];
				if (blendShapeEntry != null && !string.IsNullOrWhiteSpace(blendShapeEntry.displayName))
				{
					this.currentValues[blendShapeEntry.displayName] = this.blendShapeController.GetEntryAuthoredUiValue(blendShapeEntry);
				}
			}
			Transform containerForBlendShapeType2 = this.GetContainerForBlendShapeType(this.currentBlendShapeType);
			if (containerForBlendShapeType2 == null)
			{
				Debug.LogError(string.Format("No container assigned for blend shape type: {0}", this.currentBlendShapeType));
				return;
			}
			RectTransform rectTransform = containerForBlendShapeType2 as RectTransform;
			if (rectTransform != null)
			{
				rectTransform.localScale = Vector3.one;
				rectTransform.anchoredPosition = Vector2.zero;
			}
			containerForBlendShapeType2.gameObject.SetActive(false);
			foreach (BlendShapeMapSO.BlendShapeEntry blendShapeEntry2 in availableBlendShapes)
			{
				if (blendShapeEntry2 == null || !blendShapeEntry2.IsGated || this.IsGateOn(blendShapeEntry2.gatedByDisplayName, availableBlendShapes))
				{
					this.CreateControlForBlendShape(blendShapeEntry2, containerForBlendShapeType2);
				}
			}
			containerForBlendShapeType2.gameObject.SetActive(true);
			if (rectTransform != null)
			{
				Canvas.ForceUpdateCanvases();
				LayoutRebuilder.ForceRebuildLayoutImmediate(rectTransform);
			}
			if (!preserveScroll)
			{
				this.ResetScrollToTop(containerForBlendShapeType2);
				return;
			}
			if (scrollRect != null)
			{
				Canvas.ForceUpdateCanvases();
				if (scrollRect.content != null)
				{
					LayoutRebuilder.ForceRebuildLayoutImmediate(scrollRect.content);
				}
				scrollRect.velocity = Vector2.zero;
				scrollRect.verticalNormalizedPosition = Mathf.Clamp01(num);
				Canvas.ForceUpdateCanvases();
			}
		}

		// Token: 0x06003421 RID: 13345 RVA: 0x00128ED0 File Offset: 0x001270D0
		private void ResetScrollToTop(Transform container)
		{
			if (container == null)
			{
				return;
			}
			ScrollRect componentInParent = container.GetComponentInParent<ScrollRect>(true);
			if (componentInParent == null)
			{
				return;
			}
			Canvas.ForceUpdateCanvases();
			if (componentInParent.content != null)
			{
				LayoutRebuilder.ForceRebuildLayoutImmediate(componentInParent.content);
			}
			componentInParent.velocity = Vector2.zero;
			componentInParent.verticalNormalizedPosition = 1f;
			Canvas.ForceUpdateCanvases();
		}

		// Token: 0x06003422 RID: 13346 RVA: 0x00128F34 File Offset: 0x00127134
		private Transform GetContainerForBlendShapeType(BlendShapeMapSO.BlendShapeType type)
		{
			this.DeactivateAllContainers();
			string text = this.currentCategory.ToLower();
			Transform transform;
			if (!(text == "head") && !(text == "face"))
			{
				if (!(text == "body"))
				{
					if (!(text == "outfit"))
					{
						Debug.LogError("Unknown category: " + this.currentCategory);
						return null;
					}
					transform = this.GetOutfitContainer(type);
				}
				else
				{
					transform = this.GetBodyContainer(type);
				}
			}
			else
			{
				transform = this.GetFaceContainer(type);
			}
			if (transform != null)
			{
				transform.gameObject.SetActive(true);
			}
			else
			{
				Debug.LogError(string.Format("No container found for category: {0}, type: {1}", this.currentCategory, type));
			}
			return transform;
		}

		// Token: 0x06003423 RID: 13347 RVA: 0x00128FF4 File Offset: 0x001271F4
		private Transform GetFaceContainer(BlendShapeMapSO.BlendShapeType type)
		{
			switch (type)
			{
			case BlendShapeMapSO.BlendShapeType.Shape:
				return this.faceShapesContainer;
			case BlendShapeMapSO.BlendShapeType.Expression:
				return this.faceExpressionsContainer;
			case BlendShapeMapSO.BlendShapeType.Color:
				return this.faceStyleContainer;
			default:
				Debug.LogError(string.Format("Unknown blend shape type for face: {0}", type));
				return null;
			}
		}

		// Token: 0x06003424 RID: 13348 RVA: 0x00129040 File Offset: 0x00127240
		private Transform GetBodyContainer(BlendShapeMapSO.BlendShapeType type)
		{
			switch (type)
			{
			case BlendShapeMapSO.BlendShapeType.Shape:
				return this.bodyShapesContainer;
			case BlendShapeMapSO.BlendShapeType.Expression:
				return this.bodyExpressionsContainer;
			case BlendShapeMapSO.BlendShapeType.Color:
				return this.bodyStyleContainer;
			default:
				Debug.LogError(string.Format("Unknown blend shape type for body: {0}", type));
				return null;
			}
		}

		// Token: 0x06003425 RID: 13349 RVA: 0x0012908C File Offset: 0x0012728C
		private Transform GetOutfitContainer(BlendShapeMapSO.BlendShapeType type)
		{
			switch (type)
			{
			case BlendShapeMapSO.BlendShapeType.Shape:
				return this.outfitShapesContainer;
			case BlendShapeMapSO.BlendShapeType.Expression:
				return this.outfitExpressionsContainer;
			case BlendShapeMapSO.BlendShapeType.Color:
				return this.outfitStyleContainer;
			default:
				Debug.LogError(string.Format("Unknown blend shape type for outfit: {0}", type));
				return null;
			}
		}

		// Token: 0x06003426 RID: 13350 RVA: 0x001290D8 File Offset: 0x001272D8
		private void DeactivateAllContainers()
		{
			if (this.faceShapesContainer != null)
			{
				this.faceShapesContainer.gameObject.SetActive(false);
			}
			if (this.faceExpressionsContainer != null)
			{
				this.faceExpressionsContainer.gameObject.SetActive(false);
			}
			if (this.faceStyleContainer != null)
			{
				this.faceStyleContainer.gameObject.SetActive(false);
			}
			if (this.bodyShapesContainer != null)
			{
				this.bodyShapesContainer.gameObject.SetActive(false);
			}
			if (this.bodyExpressionsContainer != null)
			{
				this.bodyExpressionsContainer.gameObject.SetActive(false);
			}
			if (this.bodyStyleContainer != null)
			{
				this.bodyStyleContainer.gameObject.SetActive(false);
			}
			if (this.outfitShapesContainer != null)
			{
				this.outfitShapesContainer.gameObject.SetActive(false);
			}
			if (this.outfitExpressionsContainer != null)
			{
				this.outfitExpressionsContainer.gameObject.SetActive(false);
			}
			if (this.outfitStyleContainer != null)
			{
				this.outfitStyleContainer.gameObject.SetActive(false);
			}
		}

		// Token: 0x06003427 RID: 13351 RVA: 0x001291FC File Offset: 0x001273FC
		private void CreateControlForBlendShape(BlendShapeMapSO.BlendShapeEntry blendShape, Transform container)
		{
			if (container == null)
			{
				Debug.LogError("Container is null — cannot create control!");
				return;
			}
			switch (blendShape.controlMode)
			{
			case BlendShapeMapSO.BlendShapeEntry.BlendShapeControlMode.Boolean:
				this.CreateBooleanControl(blendShape, container);
				return;
			case BlendShapeMapSO.BlendShapeEntry.BlendShapeControlMode.Select:
				this.CreateSelectControl(blendShape, container);
				return;
			}
			this.CreateSliderControl(blendShape, container);
		}

		// Token: 0x06003428 RID: 13352 RVA: 0x0012925C File Offset: 0x0012745C
		private void CreateSliderControl(BlendShapeMapSO.BlendShapeEntry blendShape, Transform container)
		{
			if (this.sliderPrefab == null)
			{
				Debug.LogError("sliderPrefab is not assigned — cannot create Range/Pair control!");
				return;
			}
			GameObject controlObj = Object.Instantiate<GameObject>(this.sliderPrefab, container);
			controlObj.name = "Slider_" + blendShape.displayName;
			Slider slider = controlObj.GetComponent<Slider>();
			if (slider == null)
			{
				slider = controlObj.GetComponentInChildren<Slider>();
			}
			if (slider != null)
			{
				slider.minValue = blendShape.GetUiMinValue();
				slider.maxValue = blendShape.GetUiMaxValue();
				slider.wholeNumbers = false;
				float orInitValue = this.GetOrInitValue(blendShape);
				slider.value = orInitValue;
				this.SetLabel(controlObj, blendShape.displayName);
				Button resetBtn = this.GetSliderResetButton(controlObj);
				if (resetBtn != null)
				{
					resetBtn.gameObject.SetActive(!Mathf.Approximately(orInitValue, blendShape.GetUiDefaultValue()));
					resetBtn.onClick.RemoveAllListeners();
					resetBtn.onClick.AddListener(delegate
					{
						this.ResetControlToDefault(blendShape);
						resetBtn.gameObject.SetActive(false);
					});
				}
				slider.onValueChanged.RemoveAllListeners();
				slider.onValueChanged.AddListener(delegate(float value)
				{
					this.OnSliderValueChanged(blendShape.displayName, value);
					this.UpdateValueText(controlObj, value, blendShape);
					if (resetBtn != null)
					{
						resetBtn.gameObject.SetActive(!Mathf.Approximately(value, blendShape.GetUiDefaultValue()));
					}
				});
				this.UpdateValueText(controlObj, slider.value, blendShape);
				this.activeSliders[blendShape.displayName] = slider;
			}
			this.activeControls[blendShape.displayName] = controlObj;
		}

		// Token: 0x06003429 RID: 13353 RVA: 0x00129428 File Offset: 0x00127628
		private void CreateSelectControl(BlendShapeMapSO.BlendShapeEntry blendShape, Transform container)
		{
			GameObject gameObject = ((this.selectPrefab != null) ? this.selectPrefab : this.sliderPrefab);
			if (gameObject == null)
			{
				Debug.LogError("No select or slider prefab assigned — cannot create Select control!");
				return;
			}
			GameObject controlObj = Object.Instantiate<GameObject>(gameObject, container);
			controlObj.name = "Select_" + blendShape.displayName;
			HorizontalSelector horizontalSelector = controlObj.GetComponent<HorizontalSelector>();
			if (horizontalSelector == null)
			{
				horizontalSelector = controlObj.GetComponentInChildren<HorizontalSelector>();
			}
			if (horizontalSelector != null)
			{
				this.SetLabel(controlObj, blendShape.displayName);
				this.ConfigureHorizontalSelector(horizontalSelector, blendShape);
				this.activeControls[blendShape.displayName] = controlObj;
				return;
			}
			Slider slider = controlObj.GetComponent<Slider>();
			if (slider == null)
			{
				slider = controlObj.GetComponentInChildren<Slider>();
			}
			if (slider != null)
			{
				slider.minValue = blendShape.GetUiMinValue();
				slider.maxValue = blendShape.GetUiMaxValue();
				slider.wholeNumbers = true;
				float orInitValue = this.GetOrInitValue(blendShape);
				slider.value = orInitValue;
				this.SetLabel(controlObj, blendShape.displayName);
				slider.onValueChanged.RemoveAllListeners();
				slider.onValueChanged.AddListener(delegate(float value)
				{
					this.OnSliderValueChanged(blendShape.displayName, value);
					this.UpdateValueText(controlObj, value, blendShape);
				});
				this.UpdateValueText(controlObj, slider.value, blendShape);
				this.activeSliders[blendShape.displayName] = slider;
			}
			this.activeControls[blendShape.displayName] = controlObj;
		}

		// Token: 0x0600342A RID: 13354 RVA: 0x00129604 File Offset: 0x00127804
		private void ConfigureHorizontalSelector(HorizontalSelector hSelector, BlendShapeMapSO.BlendShapeEntry blendShape)
		{
			hSelector.items.Clear();
			int selectOptionCount = blendShape.GetSelectOptionCount();
			if (hSelector.enableIndicators && (hSelector.indicatorParent == null || hSelector.indicatorObject == null))
			{
				hSelector.enableIndicators = false;
			}
			for (int i = 0; i < selectOptionCount; i++)
			{
				string selectOptionDisplayName = blendShape.GetSelectOptionDisplayName(i);
				hSelector.CreateNewItem(string.IsNullOrEmpty(selectOptionDisplayName) ? i.ToString() : selectOptionDisplayName);
			}
			int num = Mathf.RoundToInt(this.GetOrInitValue(blendShape));
			hSelector.defaultIndex = Mathf.Clamp(num, 0, Mathf.Max(0, selectOptionCount - 1));
			hSelector.SetupSelector();
			CustomizationHorizontalSelectorLocalization.Bind(hSelector);
			hSelector.onValueChanged.RemoveAllListeners();
			hSelector.onValueChanged.AddListener(delegate(int index)
			{
				this.OnSliderValueChanged(blendShape.displayName, (float)index);
			});
		}

		// Token: 0x0600342B RID: 13355 RVA: 0x001296F4 File Offset: 0x001278F4
		private void CreateBooleanControl(BlendShapeMapSO.BlendShapeEntry blendShape, Transform container)
		{
			GameObject gameObject = ((this.checkboxPrefab != null) ? this.checkboxPrefab : this.sliderPrefab);
			if (gameObject == null)
			{
				Debug.LogError("No checkbox or slider prefab assigned — cannot create Boolean control!");
				return;
			}
			GameObject controlObj = Object.Instantiate<GameObject>(gameObject, container);
			controlObj.name = "Check_" + blendShape.displayName;
			Toggle toggle = controlObj.GetComponent<Toggle>();
			if (toggle == null)
			{
				toggle = controlObj.GetComponentInChildren<Toggle>();
			}
			if (toggle != null)
			{
				this.SetLabel(controlObj, blendShape.displayName);
				float orInitValue = this.GetOrInitValue(blendShape);
				toggle.isOn = orInitValue >= 0.5f;
				toggle.onValueChanged.RemoveAllListeners();
				toggle.onValueChanged.AddListener(delegate(bool isOn)
				{
					float num = (isOn ? 1f : 0f);
					this.currentValues[blendShape.displayName] = num;
					this.OnSliderValueChanged(blendShape.displayName, num);
				});
				this.activeControls[blendShape.displayName] = controlObj;
				return;
			}
			Slider slider = controlObj.GetComponent<Slider>();
			if (slider == null)
			{
				slider = controlObj.GetComponentInChildren<Slider>();
			}
			if (slider != null)
			{
				slider.minValue = 0f;
				slider.maxValue = 1f;
				slider.wholeNumbers = true;
				float orInitValue2 = this.GetOrInitValue(blendShape);
				slider.value = orInitValue2;
				this.SetLabel(controlObj, blendShape.displayName);
				slider.onValueChanged.RemoveAllListeners();
				slider.onValueChanged.AddListener(delegate(float value)
				{
					this.OnSliderValueChanged(blendShape.displayName, value);
					this.UpdateValueText(controlObj, value, blendShape);
				});
				this.UpdateValueText(controlObj, slider.value, blendShape);
				this.activeSliders[blendShape.displayName] = slider;
			}
			this.activeControls[blendShape.displayName] = controlObj;
		}

		// Token: 0x0600342C RID: 13356 RVA: 0x001298F8 File Offset: 0x00127AF8
		private float GetOrInitValue(BlendShapeMapSO.BlendShapeEntry blendShape)
		{
			if (this.blendShapeController != null && blendShape != null)
			{
				float entryAuthoredUiValue = this.blendShapeController.GetEntryAuthoredUiValue(blendShape);
				this.currentValues[blendShape.displayName] = entryAuthoredUiValue;
				return entryAuthoredUiValue;
			}
			float uiDefaultValue;
			if (blendShape != null && !this.currentValues.TryGetValue(blendShape.displayName, out uiDefaultValue))
			{
				uiDefaultValue = blendShape.GetUiDefaultValue();
				this.currentValues[blendShape.displayName] = uiDefaultValue;
				return uiDefaultValue;
			}
			if (blendShape == null)
			{
				return 0f;
			}
			return this.currentValues[blendShape.displayName];
		}

		// Token: 0x0600342D RID: 13357 RVA: 0x00129984 File Offset: 0x00127B84
		private void SyncCurrentValuesFromController()
		{
			if (this.blendShapeController == null || this.blendShapeController.BlendShapeMap == null || this.blendShapeController.BlendShapeMap.blendShapeMappings == null)
			{
				return;
			}
			List<BlendShapeMapSO.BlendShapeEntry> blendShapeMappings = this.blendShapeController.BlendShapeMap.blendShapeMappings;
			for (int i = 0; i < blendShapeMappings.Count; i++)
			{
				BlendShapeMapSO.BlendShapeEntry blendShapeEntry = blendShapeMappings[i];
				if (blendShapeEntry != null && !string.IsNullOrWhiteSpace(blendShapeEntry.displayName))
				{
					this.currentValues[blendShapeEntry.displayName] = this.blendShapeController.GetEntryAuthoredUiValue(blendShapeEntry);
				}
			}
		}

		// Token: 0x0600342E RID: 13358 RVA: 0x00129A1C File Offset: 0x00127C1C
		private void SetLabel(GameObject controlObj, string label)
		{
			TextMeshProUGUI componentInChildren = controlObj.GetComponentInChildren<TextMeshProUGUI>();
			if (componentInChildren != null)
			{
				TextMeshProUGUI textMeshProUGUI = componentInChildren.GetComponentInChildren<TextMeshProUGUI>();
				foreach (TextMeshProUGUI textMeshProUGUI2 in componentInChildren.GetComponentsInChildren<TextMeshProUGUI>(true))
				{
					if (textMeshProUGUI2 != componentInChildren)
					{
						textMeshProUGUI = textMeshProUGUI2;
						break;
					}
				}
				if (textMeshProUGUI != null && textMeshProUGUI != componentInChildren)
				{
					Vector4 margin = componentInChildren.margin;
					margin.z = Mathf.Max(margin.z, textMeshProUGUI.GetPreferredValues("-100%").x + 12f);
					componentInChildren.margin = margin;
					componentInChildren.fontSizeMax = componentInChildren.fontSize;
					componentInChildren.fontSizeMin = Mathf.Min(componentInChildren.fontSize, 18f);
					componentInChildren.enableAutoSizing = true;
					componentInChildren.enableWordWrapping = true;
				}
				LocalizedGameText localizedGameText = LocalizedGameText.BindSource(componentInChildren, "cielos_apps", label, null);
				if (localizedGameText == null)
				{
					return;
				}
				localizedGameText.FitTranslationToRectangle(false);
			}
		}

		// Token: 0x0600342F RID: 13359 RVA: 0x00129B00 File Offset: 0x00127D00
		private Button GetSliderResetButton(GameObject controlObj)
		{
			BlendShapeSliderControlRefs blendShapeSliderControlRefs = controlObj.GetComponent<BlendShapeSliderControlRefs>();
			if (blendShapeSliderControlRefs == null)
			{
				blendShapeSliderControlRefs = controlObj.GetComponentInChildren<BlendShapeSliderControlRefs>(true);
			}
			if (blendShapeSliderControlRefs == null)
			{
				Debug.LogWarning("[BlendShapeUI] Slider prefab for '" + controlObj.name + "' is missing BlendShapeSliderControlRefs.");
				return null;
			}
			return blendShapeSliderControlRefs.ResetButton;
		}

		// Token: 0x06003430 RID: 13360 RVA: 0x00129B50 File Offset: 0x00127D50
		private void ResetControlToDefault(BlendShapeMapSO.BlendShapeEntry blendShape)
		{
			float uiDefaultValue = blendShape.GetUiDefaultValue();
			this.currentValues[blendShape.displayName] = uiDefaultValue;
			Slider slider;
			if (this.activeSliders.TryGetValue(blendShape.displayName, out slider) && slider != null)
			{
				slider.value = uiDefaultValue;
			}
			GameObject gameObject;
			if (this.activeControls.TryGetValue(blendShape.displayName, out gameObject) && gameObject != null)
			{
				Toggle componentInChildren = gameObject.GetComponentInChildren<Toggle>();
				if (componentInChildren != null)
				{
					componentInChildren.isOn = uiDefaultValue >= 0.5f;
				}
				HorizontalSelector componentInChildren2 = gameObject.GetComponentInChildren<HorizontalSelector>();
				if (componentInChildren2 != null)
				{
					int num = Mathf.RoundToInt(uiDefaultValue);
					if (num >= 0 && num < componentInChildren2.items.Count)
					{
						componentInChildren2.defaultIndex = num;
						componentInChildren2.SetupSelector();
					}
				}
			}
			this.OnSliderValueChanged(blendShape.displayName, uiDefaultValue);
		}

		// Token: 0x06003431 RID: 13361 RVA: 0x00129C28 File Offset: 0x00127E28
		private bool IsGateOn(string gateDisplayName, List<BlendShapeMapSO.BlendShapeEntry> entries)
		{
			if (string.IsNullOrWhiteSpace(gateDisplayName))
			{
				return true;
			}
			float num;
			if (this.currentValues.TryGetValue(gateDisplayName, out num))
			{
				return num >= 0.5f;
			}
			BlendShapeMapSO.BlendShapeEntry blendShapeEntry = this.FindEntryByDisplayName(gateDisplayName, entries);
			return blendShapeEntry != null && this.blendShapeController != null && this.blendShapeController.GetEntryAuthoredUiValue(blendShapeEntry) >= 0.5f;
		}

		// Token: 0x06003432 RID: 13362 RVA: 0x00129C90 File Offset: 0x00127E90
		private BlendShapeMapSO.BlendShapeEntry FindEntryByDisplayName(string displayName, List<BlendShapeMapSO.BlendShapeEntry> entries)
		{
			if (string.IsNullOrWhiteSpace(displayName) || entries == null)
			{
				return null;
			}
			for (int i = 0; i < entries.Count; i++)
			{
				if (entries[i] != null && string.Equals(entries[i].displayName, displayName, StringComparison.OrdinalIgnoreCase))
				{
					return entries[i];
				}
			}
			return null;
		}

		// Token: 0x06003433 RID: 13363 RVA: 0x00129CE2 File Offset: 0x00127EE2
		private FutanariModeController ResolveFutanariController()
		{
			if (this.blendShapeController == null)
			{
				return null;
			}
			return this.blendShapeController.GetComponentInChildren<FutanariModeController>(true);
		}

		// Token: 0x06003434 RID: 13364 RVA: 0x00129D00 File Offset: 0x00127F00
		private bool IsGateEntry(string displayName)
		{
			if (string.IsNullOrWhiteSpace(displayName) || this.blendShapeController == null || this.blendShapeController.BlendShapeMap == null)
			{
				return false;
			}
			List<BlendShapeMapSO.BlendShapeEntry> blendShapeMappings = this.blendShapeController.BlendShapeMap.blendShapeMappings;
			if (blendShapeMappings == null)
			{
				return false;
			}
			for (int i = 0; i < blendShapeMappings.Count; i++)
			{
				if (blendShapeMappings[i] != null && string.Equals(blendShapeMappings[i].gatedByDisplayName, displayName, StringComparison.OrdinalIgnoreCase))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06003435 RID: 13365 RVA: 0x00129D80 File Offset: 0x00127F80
		private void OnSliderValueChanged(string blendShapeName, float value)
		{
			this.currentValues[blendShapeName] = value;
			BlendShapeMapSO.BlendShapeEntry blendShapeEntry = this.FindEntryByDisplayName(blendShapeName, (this.blendShapeController != null && this.blendShapeController.BlendShapeMap != null) ? this.blendShapeController.BlendShapeMap.blendShapeMappings : null);
			if (blendShapeEntry != null && blendShapeEntry.isFutanariToggle)
			{
				FutanariModeController futanariModeController = this.ResolveFutanariController();
				if (futanariModeController != null)
				{
					futanariModeController.SetFutanariMode(value >= 0.5f);
				}
			}
			if (this.blendShapeController != null)
			{
				this.blendShapeController.ApplyBlendShapesForSubcategory(this.currentCategory, this.currentSubcategory, this.currentBlendShapeType, this.currentValues);
			}
			else
			{
				Debug.LogWarning("BlendShapeController is not assigned!");
			}
			if (this.IsGateEntry(blendShapeName))
			{
				this.CreateUIControlsForCurrentSelection(true);
			}
		}

		// Token: 0x06003436 RID: 13366 RVA: 0x00129E50 File Offset: 0x00128050
		private void UpdateValueText(GameObject sliderObj, float value, BlendShapeMapSO.BlendShapeEntry blendShape)
		{
			TextMeshProUGUI[] componentsInChildren = sliderObj.GetComponentsInChildren<TextMeshProUGUI>();
			if (componentsInChildren.Length > 1)
			{
				if (blendShape != null && blendShape.IsBooleanControl())
				{
					LocalizedGameText localizedGameText = LocalizedGameText.BindSource(componentsInChildren[1], "cielos_apps", (value >= 0.5f) ? "ON" : "OFF", null);
					if (localizedGameText == null)
					{
						return;
					}
					localizedGameText.FitTranslationToRectangle(false);
					return;
				}
				else if (blendShape != null && blendShape.IsPairControl())
				{
					if (Mathf.Abs(value) < 0.001f)
					{
						componentsInChildren[1].text = "0%";
						return;
					}
					if (value < 0f)
					{
						componentsInChildren[1].text = string.Format("-{0}%", Mathf.RoundToInt(Mathf.Abs(value) * 100f));
						return;
					}
					componentsInChildren[1].text = string.Format("{0}%", Mathf.RoundToInt(value * 100f));
					return;
				}
				else if (blendShape != null && blendShape.IsSelectControl())
				{
					int num = Mathf.RoundToInt(value);
					string selectOptionDisplayName = blendShape.GetSelectOptionDisplayName(num);
					LocalizedGameText localizedGameText2 = LocalizedGameText.BindSource(componentsInChildren[1], "cielos_apps", string.IsNullOrEmpty(selectOptionDisplayName) ? num.ToString() : selectOptionDisplayName, null);
					if (localizedGameText2 == null)
					{
						return;
					}
					localizedGameText2.FitTranslationToRectangle(false);
					return;
				}
				else
				{
					componentsInChildren[1].text = string.Format("{0}%", Mathf.RoundToInt(value));
				}
			}
		}

		// Token: 0x06003437 RID: 13367 RVA: 0x00129F84 File Offset: 0x00128184
		private void ClearSliders()
		{
			foreach (GameObject gameObject in this.activeControls.Values)
			{
				if (gameObject != null)
				{
					Object.Destroy(gameObject);
				}
			}
			this.activeControls.Clear();
			foreach (Slider slider in this.activeSliders.Values)
			{
				if (slider != null && slider.gameObject != null)
				{
					Object.Destroy(slider.gameObject);
				}
			}
			this.activeSliders.Clear();
			this.ClearContainer(this.faceShapesContainer);
			this.ClearContainer(this.faceExpressionsContainer);
			this.ClearContainer(this.faceStyleContainer);
			this.ClearContainer(this.bodyShapesContainer);
			this.ClearContainer(this.bodyExpressionsContainer);
			this.ClearContainer(this.bodyStyleContainer);
			this.ClearContainer(this.outfitShapesContainer);
			this.ClearContainer(this.outfitExpressionsContainer);
			this.ClearContainer(this.outfitStyleContainer);
		}

		// Token: 0x06003438 RID: 13368 RVA: 0x0012A0C8 File Offset: 0x001282C8
		private void ClearContainer(Transform container)
		{
			if (container == null)
			{
				return;
			}
			List<GameObject> list = new List<GameObject>();
			foreach (object obj in container)
			{
				Transform transform = (Transform)obj;
				list.Add(transform.gameObject);
			}
			foreach (GameObject gameObject in list)
			{
				Object.DestroyImmediate(gameObject);
			}
			if (container is RectTransform)
			{
				Canvas.ForceUpdateCanvases();
				LayoutRebuilder.ForceRebuildLayoutImmediate(container as RectTransform);
			}
		}

		// Token: 0x0400348D RID: 13453
		[Header("Character References")]
		[SerializeField]
		private CharacterBlendShapeController blendShapeController;

		// Token: 0x0400348E RID: 13454
		[Header("UI References")]
		[SerializeField]
		private HeadCustomizationController headCustomizationController;

		// Token: 0x0400348F RID: 13455
		[SerializeField]
		private BodyCustomizationController bodyCustomizationController;

		// Token: 0x04003490 RID: 13456
		[Header("Face Containers")]
		[SerializeField]
		private Transform faceShapesContainer;

		// Token: 0x04003491 RID: 13457
		[SerializeField]
		private Transform faceExpressionsContainer;

		// Token: 0x04003492 RID: 13458
		[SerializeField]
		private Transform faceStyleContainer;

		// Token: 0x04003493 RID: 13459
		[Header("Body Containers")]
		[SerializeField]
		private Transform bodyShapesContainer;

		// Token: 0x04003494 RID: 13460
		[SerializeField]
		private Transform bodyExpressionsContainer;

		// Token: 0x04003495 RID: 13461
		[SerializeField]
		private Transform bodyStyleContainer;

		// Token: 0x04003496 RID: 13462
		[Header("Outfit Containers")]
		[SerializeField]
		private Transform outfitShapesContainer;

		// Token: 0x04003497 RID: 13463
		[SerializeField]
		private Transform outfitExpressionsContainer;

		// Token: 0x04003498 RID: 13464
		[SerializeField]
		private Transform outfitStyleContainer;

		// Token: 0x04003499 RID: 13465
		[Header("Prefabs — one per control type")]
		[Tooltip("Slider prefab for Range and PairInOut controls")]
		[SerializeField]
		private GameObject sliderPrefab;

		// Token: 0x0400349A RID: 13466
		[Tooltip("Select/dropdown prefab for Select controls (HorizontalSelector or similar)")]
		[SerializeField]
		private GameObject selectPrefab;

		// Token: 0x0400349B RID: 13467
		[Tooltip("Checkbox/toggle prefab for Boolean controls")]
		[SerializeField]
		private GameObject checkboxPrefab;

		// Token: 0x0400349C RID: 13468
		private string currentCategory;

		// Token: 0x0400349D RID: 13469
		private string currentSubcategory;

		// Token: 0x0400349E RID: 13470
		private BlendShapeMapSO.BlendShapeType currentBlendShapeType;

		// Token: 0x0400349F RID: 13471
		private Dictionary<string, List<CharacterSubCategorySO>> categorySubcategories = new Dictionary<string, List<CharacterSubCategorySO>>();

		// Token: 0x040034A0 RID: 13472
		private Dictionary<string, GameObject> activeControls = new Dictionary<string, GameObject>();

		// Token: 0x040034A1 RID: 13473
		private Dictionary<string, Slider> activeSliders = new Dictionary<string, Slider>();

		// Token: 0x040034A2 RID: 13474
		private Dictionary<string, float> currentValues = new Dictionary<string, float>();

		// Token: 0x040034A3 RID: 13475
		private BlendShapeManager subscribedManager;

		// Token: 0x040034A4 RID: 13476
		private bool refreshValuesPending;
	}
}
