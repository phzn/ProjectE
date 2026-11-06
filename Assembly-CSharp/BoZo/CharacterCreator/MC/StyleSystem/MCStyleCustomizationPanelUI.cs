using System;
using System.Collections;
using System.Collections.Generic;
using Bozo.AnimeCharacters;
using BoZo.CharacterCreator.MC.StyleSystem.UI;
using Features.Localization;
using Michsky.MUIP;
using ProjectExhibitionist.CharacterCustomization.Core.Runtime;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace BoZo.CharacterCreator.MC.StyleSystem
{
	// Token: 0x020007E0 RID: 2016
	public class MCStyleCustomizationPanelUI : MonoBehaviour
	{
		// Token: 0x06003559 RID: 13657 RVA: 0x0012FE28 File Offset: 0x0012E028
		private void Awake()
		{
			this.EnsureOptionsLayout();
			if (this.textureSelector != null)
			{
				this.textureSelector.onValueChanged.AddListener(new UnityAction<int>(this.OnTextureSelectorChanged));
			}
			this.BindPresetPreview(this.colorPresetPreview);
			this.BindColorPreview(this.primaryColorPreview, MCColorChannelId.Primary);
			this.BindColorPreview(this.secondaryColorPreview, MCColorChannelId.Secondary);
			if (this.closeButton != null)
			{
				this.closeButton.onClick.RemoveListener(new UnityAction(this.OnCloseButtonClicked));
				this.closeButton.onClick.AddListener(new UnityAction(this.OnCloseButtonClicked));
			}
			if (this.closeButtonManager != null)
			{
				this.closeButtonManager.onClick.RemoveListener(new UnityAction(this.OnCloseButtonClicked));
				this.closeButtonManager.onClick.AddListener(new UnityAction(this.OnCloseButtonClicked));
			}
		}

		// Token: 0x0600355A RID: 13658 RVA: 0x0012FF17 File Offset: 0x0012E117
		private void OnDisable()
		{
			this.CloseColorPickerIfOpen();
			this.ClearShaderRows();
			this.ClearHairMorphRows();
			if (this._layoutRefresh != null)
			{
				base.StopCoroutine(this._layoutRefresh);
			}
			this._layoutRefresh = null;
		}

		// Token: 0x0600355B RID: 13659 RVA: 0x0012FF48 File Offset: 0x0012E148
		private void OnDestroy()
		{
			this.CloseColorPickerIfOpen();
			this.ClearShaderRows();
			this.ClearHairMorphRows();
			if (this.textureSelector != null)
			{
				this.textureSelector.onValueChanged.RemoveListener(new UnityAction<int>(this.OnTextureSelectorChanged));
			}
			this.UnbindPresetPreview(this.colorPresetPreview);
			this.UnbindColorPreview(this.primaryColorPreview);
			this.UnbindColorPreview(this.secondaryColorPreview);
			if (this.styleService != null)
			{
				this.styleService.OnActiveStateChanged -= this.Refresh;
			}
			if (this.closeButton != null)
			{
				this.closeButton.onClick.RemoveListener(new UnityAction(this.OnCloseButtonClicked));
			}
			if (this.closeButtonManager != null)
			{
				this.closeButtonManager.onClick.RemoveListener(new UnityAction(this.OnCloseButtonClicked));
			}
		}

		// Token: 0x0600355C RID: 13660 RVA: 0x00130030 File Offset: 0x0012E230
		public void SetService(MCStyleCustomizationService service)
		{
			if (this.styleService != null)
			{
				this.styleService.OnActiveStateChanged -= this.Refresh;
			}
			this.styleService = service;
			if (this.styleService != null)
			{
				this.styleService.OnActiveStateChanged += this.Refresh;
			}
			this._closedByUser = false;
			this.Refresh();
		}

		// Token: 0x0600355D RID: 13661 RVA: 0x0013009C File Offset: 0x0012E29C
		public bool HasCustomizableOptions()
		{
			this.BuildDiscoveredSurfaceOptions(null);
			return this.styleService != null && (this._visibleSurfaceIndices.Count > 1 || this.styleService.IsColorSelectionVisible() || this.styleService.HasActiveExposedSliders() || this.styleService.GetActiveHairMorphControls().Count > 0);
		}

		// Token: 0x0600355E RID: 13662 RVA: 0x001300FD File Offset: 0x0012E2FD
		public void Reopen()
		{
			this._closedByUser = false;
			this.Refresh();
		}

		// Token: 0x0600355F RID: 13663 RVA: 0x0013010C File Offset: 0x0012E30C
		public void Refresh()
		{
			if (this._isHairMorphDriving)
			{
				return;
			}
			this.EnsureOptionsLayout();
			if (!this._isColorPickerDriving)
			{
				this.CloseColorPickerIfOpen();
			}
			bool flag = this.styleService != null;
			List<string> list = new List<string>();
			this.BuildDiscoveredSurfaceOptions(list);
			bool flag2 = flag && this._visibleSurfaceIndices.Count > 1;
			bool flag3 = flag && this.styleService.IsColorSelectionVisible();
			bool flag4 = flag && this.styleService.HasActiveExposedSliders();
			bool flag5 = flag && this.styleService.GetActiveHairMorphControls().Count > 0;
			bool flag6 = flag && (flag2 || flag3 || flag4 || flag5) && !this._closedByUser;
			if (this.panelRoot != null)
			{
				this.panelRoot.SetActive(flag6);
			}
			if (!flag6)
			{
				this.ClearShaderRows();
				this.ClearHairMorphRows();
				return;
			}
			if (this.textureSectionRoot != null)
			{
				this.textureSectionRoot.SetActive(flag2);
			}
			if (this.colorSectionRoot != null)
			{
				this.colorSectionRoot.SetActive(flag3);
			}
			if (this.secondaryColorRoot != null)
			{
				this.secondaryColorRoot.SetActive(this.styleService.IsSecondaryChannelVisible());
			}
			this.RefreshShaderControls(flag4);
			bool flag7 = this.RefreshHairMorphControls(flag5);
			this._suppressEvents = true;
			int selectedSurfaceIndex = this.styleService.GetSelectedSurfaceIndex();
			int num = this._visibleSurfaceIndices.IndexOf(selectedSurfaceIndex);
			MCStyleCustomizationPanelUI.PopulateSelector(this.textureSelector, list, Mathf.Max(0, num));
			this._suppressEvents = false;
			this.RefreshPresetRow();
			this.RefreshColorSwatch(this.primaryColorPreview, MCColorChannelId.Primary);
			this.RefreshColorSwatch(this.secondaryColorPreview, MCColorChannelId.Secondary);
			if (this._layoutRefresh != null)
			{
				base.StopCoroutine(this._layoutRefresh);
			}
			if (base.isActiveAndEnabled)
			{
				this._layoutRefresh = base.StartCoroutine(this.RebuildOptionsLayout(flag7));
			}
		}

		// Token: 0x06003560 RID: 13664 RVA: 0x001302E0 File Offset: 0x0012E4E0
		private void OnTextureSelectorChanged(int index)
		{
			if (this._suppressEvents || this.styleService == null || index < 0 || index >= this._visibleSurfaceIndices.Count)
			{
				return;
			}
			if (this.styleService.SelectSurface(this._visibleSurfaceIndices[index]))
			{
				this.Refresh();
			}
		}

		// Token: 0x06003561 RID: 13665 RVA: 0x00130338 File Offset: 0x0012E538
		private void BuildDiscoveredSurfaceOptions(List<string> names)
		{
			this._visibleSurfaceIndices.Clear();
			if (this.styleService == null || this.styleService.ActiveDefinition == null)
			{
				return;
			}
			MCStylePresetSO activePreset = this.styleService.GetActivePreset(this.styleService.ActiveDefinition.partId);
			if (activePreset == null)
			{
				return;
			}
			List<int> discoveredSurfaceIndices = CustomizationWardrobeDiscoveryPolicy.GetDiscoveredSurfaceIndices(activePreset);
			for (int i = 0; i < discoveredSurfaceIndices.Count; i++)
			{
				int num = discoveredSurfaceIndices[i];
				MCSurfaceVariantSO mcsurfaceVariantSO;
				if (activePreset.TryGetSurfaceVariant(num, out mcsurfaceVariantSO) && !(mcsurfaceVariantSO == null))
				{
					this._visibleSurfaceIndices.Add(num);
					if (names != null)
					{
						names.Add(string.IsNullOrWhiteSpace(mcsurfaceVariantSO.displayName) ? mcsurfaceVariantSO.name : mcsurfaceVariantSO.displayName);
					}
				}
			}
		}

		// Token: 0x06003562 RID: 13666 RVA: 0x00130400 File Offset: 0x0012E600
		private void OnEmbeddedPresetSelectorChanged(int index)
		{
			if (this._suppressEvents || this.styleService == null)
			{
				return;
			}
			if (this.styleService.SelectColorPreset(index))
			{
				this.Refresh();
			}
		}

		// Token: 0x06003563 RID: 13667 RVA: 0x0013042D File Offset: 0x0012E62D
		private void BindPresetPreview(ColorPreviewUI preview)
		{
			if (preview == null)
			{
				return;
			}
			preview.OnEmbeddedPresetSelected.RemoveAllListeners();
			preview.OnEmbeddedPresetSelected.AddListener(new UnityAction<int>(this.OnEmbeddedPresetSelectorChanged));
		}

		// Token: 0x06003564 RID: 13668 RVA: 0x0013045B File Offset: 0x0012E65B
		private void UnbindPresetPreview(ColorPreviewUI preview)
		{
			if (preview == null)
			{
				return;
			}
			preview.OnEmbeddedPresetSelected.RemoveAllListeners();
		}

		// Token: 0x06003565 RID: 13669 RVA: 0x00130474 File Offset: 0x0012E674
		private void BindColorPreview(ColorPreviewUI preview, MCColorChannelId channel)
		{
			if (preview == null)
			{
				return;
			}
			preview.OnPreviewClicked.RemoveAllListeners();
			preview.OnPreviewClicked.AddListener(delegate
			{
				this.OnColorPreviewClicked(channel);
			});
		}

		// Token: 0x06003566 RID: 13670 RVA: 0x001304C1 File Offset: 0x0012E6C1
		private void UnbindColorPreview(ColorPreviewUI preview)
		{
			if (preview == null)
			{
				return;
			}
			preview.OnPreviewClicked.RemoveAllListeners();
		}

		// Token: 0x06003567 RID: 13671 RVA: 0x001304D8 File Offset: 0x0012E6D8
		private void OnColorPreviewClicked(MCColorChannelId channel)
		{
			if (this.styleService == null || this.colorPicker == null || !this.styleService.IsCustomPickerAllowed())
			{
				return;
			}
			Color white = Color.white;
			this.styleService.TryGetCurrentChannelColor(channel, out white);
			string text = ((channel == MCColorChannelId.Primary) ? "Primary" : "Secondary");
			this.colorPicker.ShowColorPicker(white, text, delegate(Color color)
			{
				this._isColorPickerDriving = true;
				try
				{
					if (this.styleService.SetCustomColor(channel, color))
					{
						this.Refresh();
					}
				}
				finally
				{
					this._isColorPickerDriving = false;
				}
			});
		}

		// Token: 0x06003568 RID: 13672 RVA: 0x0013056B File Offset: 0x0012E76B
		private void OnCloseButtonClicked()
		{
			this._closedByUser = true;
			this.CloseColorPickerIfOpen();
			if (this.panelRoot != null)
			{
				this.panelRoot.SetActive(false);
			}
		}

		// Token: 0x06003569 RID: 13673 RVA: 0x00130594 File Offset: 0x0012E794
		private void CloseColorPickerIfOpen()
		{
			if (this.colorPicker != null)
			{
				this.colorPicker.Close();
			}
		}

		// Token: 0x0600356A RID: 13674 RVA: 0x001305B0 File Offset: 0x0012E7B0
		private static void PopulateSelector(HorizontalSelector selector, List<string> names, int selectedIndex)
		{
			if (selector == null)
			{
				return;
			}
			if (selector.enableIndicators && (selector.indicatorParent == null || selector.indicatorObject == null))
			{
				selector.enableIndicators = false;
			}
			selector.items.Clear();
			if (names != null)
			{
				for (int i = 0; i < names.Count; i++)
				{
					selector.CreateNewItem(string.IsNullOrWhiteSpace(names[i]) ? string.Format("Item {0}", i + 1) : names[i]);
				}
			}
			int num = ((names != null && names.Count > 0) ? Mathf.Clamp(selectedIndex, 0, names.Count - 1) : 0);
			selector.defaultIndex = num;
			selector.SetupSelector();
			CustomizationHorizontalSelectorLocalization.Bind(selector);
			MCStyleSurfaceSelectorLayout.Bind(selector);
		}

		// Token: 0x0600356B RID: 13675 RVA: 0x00130678 File Offset: 0x0012E878
		private void RefreshPresetRow()
		{
			if (this.colorPresetPreview == null || this.styleService == null)
			{
				return;
			}
			List<string> colorPresetNames = this.styleService.GetColorPresetNames();
			int selectedColorPresetIndex = this.styleService.GetSelectedColorPresetIndex();
			if (colorPresetNames != null && colorPresetNames.Count > 0)
			{
				this.colorPresetPreview.gameObject.SetActive(true);
				this.colorPresetPreview.ConfigureEmbeddedPresetMode(colorPresetNames, selectedColorPresetIndex);
				Color color;
				if (this.styleService.TryGetCurrentChannelColor(MCColorChannelId.Primary, out color))
				{
					this.colorPresetPreview.PreviewColor = color;
					return;
				}
			}
			else
			{
				this.colorPresetPreview.gameObject.SetActive(false);
			}
		}

		// Token: 0x0600356C RID: 13676 RVA: 0x00130718 File Offset: 0x0012E918
		private void RefreshColorSwatch(ColorPreviewUI preview, MCColorChannelId channel)
		{
			if (preview == null || this.styleService == null)
			{
				return;
			}
			bool flag = channel == MCColorChannelId.Primary || this.styleService.IsSecondaryChannelVisible();
			preview.gameObject.SetActive(flag);
			if (!flag)
			{
				return;
			}
			preview.ConfigureSimpleMode();
			string text = ((channel == MCColorChannelId.Primary) ? "Primary" : "Secondary");
			Color color;
			if (this.styleService.TryGetCurrentChannelColor(channel, out color))
			{
				preview.Initialize(color, text);
				return;
			}
			preview.Initialize(Color.white, text);
		}

		// Token: 0x0600356D RID: 13677 RVA: 0x0013079C File Offset: 0x0012E99C
		private void RefreshShaderControls(bool hasShaderSliders)
		{
			if (this.shaderControlsRoot != null)
			{
				this.shaderControlsRoot.SetActive(hasShaderSliders);
			}
			this.ClearShaderRows();
			if (!hasShaderSliders || this.styleService == null || this.shaderSliderContainer == null || this.shaderSliderRowPrefab == null)
			{
				return;
			}
			List<MCShaderParameterOverrideSO.ShaderOverrideEntry> activeExposedSliders = this.styleService.GetActiveExposedSliders();
			for (int i = 0; i < activeExposedSliders.Count; i++)
			{
				MCShaderParameterOverrideSO.ShaderOverrideEntry shaderOverrideEntry = activeExposedSliders[i];
				if (shaderOverrideEntry != null && !string.IsNullOrWhiteSpace(shaderOverrideEntry.propertyName))
				{
					MCStyleShaderSliderRowUI mcstyleShaderSliderRowUI = Object.Instantiate<MCStyleShaderSliderRowUI>(this.shaderSliderRowPrefab, this.shaderSliderContainer);
					mcstyleShaderSliderRowUI.gameObject.SetActive(true);
					float shaderSliderValue = this.styleService.GetShaderSliderValue(shaderOverrideEntry.propertyName);
					float shaderSliderDefaultValue = this.styleService.GetShaderSliderDefaultValue(shaderOverrideEntry.propertyName);
					mcstyleShaderSliderRowUI.Configure(string.IsNullOrWhiteSpace(shaderOverrideEntry.sliderDisplayName) ? shaderOverrideEntry.propertyName : shaderOverrideEntry.sliderDisplayName, shaderOverrideEntry.propertyName, shaderOverrideEntry.sliderMin, shaderOverrideEntry.sliderMax, shaderOverrideEntry.sliderStep, shaderSliderValue, shaderSliderDefaultValue, new Action<string, float>(this.OnShaderSliderValueChanged));
					this._spawnedShaderRows.Add(mcstyleShaderSliderRowUI);
				}
			}
		}

		// Token: 0x0600356E RID: 13678 RVA: 0x001308D4 File Offset: 0x0012EAD4
		private void ClearShaderRows()
		{
			for (int i = 0; i < this._spawnedShaderRows.Count; i++)
			{
				if (this._spawnedShaderRows[i] != null)
				{
					Object.Destroy(this._spawnedShaderRows[i].gameObject);
				}
			}
			this._spawnedShaderRows.Clear();
		}

		// Token: 0x0600356F RID: 13679 RVA: 0x0013092C File Offset: 0x0012EB2C
		private void OnShaderSliderValueChanged(string propertyName, float value)
		{
			if (this.styleService == null || string.IsNullOrWhiteSpace(propertyName))
			{
				return;
			}
			this.styleService.SetShaderSliderValue(propertyName, value);
		}

		// Token: 0x06003570 RID: 13680 RVA: 0x00130954 File Offset: 0x0012EB54
		private void EnsureOptionsLayout()
		{
			if (this._optionsScroll == null && this.textureSectionRoot != null)
			{
				RectTransform rectTransform = this.textureSectionRoot.transform.parent as RectTransform;
				if (rectTransform != null)
				{
					this._optionsScroll = MCStyleOptionsLayout.MakeScrollable(rectTransform, rectTransform.parent);
				}
			}
			MCStyleSurfaceSelectorLayout.Bind(this.textureSelector);
			if (this._hairMorphSection != null || this._optionsScroll == null)
			{
				return;
			}
			this._hairMorphSection = new GameObject("Hairstyle", new Type[]
			{
				typeof(RectTransform),
				typeof(VerticalLayoutGroup)
			});
			this._hairMorphSection.transform.SetParent(this._optionsScroll.content, false);
			VerticalLayoutGroup component = this._hairMorphSection.GetComponent<VerticalLayoutGroup>();
			component.spacing = 8f;
			component.childControlHeight = (component.childControlWidth = true);
			component.childForceExpandHeight = false;
			component.childForceExpandWidth = true;
			TMP_Text componentInChildren = this.textureSectionRoot.GetComponentInChildren<TMP_Text>(true);
			if (componentInChildren != null)
			{
				TMP_Text tmp_Text = Object.Instantiate<TMP_Text>(componentInChildren, this._hairMorphSection.transform);
				tmp_Text.name = "Hairstyle Heading";
				LocalizedGameText.BindSource(tmp_Text, "cielos_apps", "HAIRSTYLE", null);
				tmp_Text.raycastTarget = false;
			}
			GameObject gameObject = new GameObject("Morph Controls", new Type[]
			{
				typeof(RectTransform),
				typeof(VerticalLayoutGroup)
			});
			gameObject.transform.SetParent(this._hairMorphSection.transform, false);
			VerticalLayoutGroup component2 = gameObject.GetComponent<VerticalLayoutGroup>();
			component2.spacing = 8f;
			component2.childControlHeight = (component2.childControlWidth = true);
			component2.childForceExpandHeight = false;
			component2.childForceExpandWidth = true;
			this._hairMorphContainer = gameObject.transform;
			this._hairMorphSection.SetActive(false);
		}

		// Token: 0x06003571 RID: 13681 RVA: 0x00130B28 File Offset: 0x0012ED28
		private bool RefreshHairMorphControls(bool visible)
		{
			MCStylePresetSO mcstylePresetSO = ((this.styleService != null && this.styleService.ActiveDefinition != null) ? this.styleService.GetActivePreset(this.styleService.ActiveDefinition.partId) : null);
			bool flag = mcstylePresetSO != this._shownHairPreset;
			this.ClearHairMorphRows();
			this._shownHairPreset = mcstylePresetSO;
			if (this._hairMorphSection != null)
			{
				this._hairMorphSection.SetActive(visible);
			}
			if (!visible || this.shaderSliderRowPrefab == null || this._hairMorphContainer == null)
			{
				return flag;
			}
			foreach (MCStylePresetSO.HairMorphControl hairMorphControl in this.styleService.GetActiveHairMorphControls())
			{
				MCStyleShaderSliderRowUI mcstyleShaderSliderRowUI = Object.Instantiate<MCStyleShaderSliderRowUI>(this.shaderSliderRowPrefab, this._hairMorphContainer);
				mcstyleShaderSliderRowUI.gameObject.name = "Hair Morph - " + hairMorphControl.id;
				mcstyleShaderSliderRowUI.gameObject.SetActive(true);
				MCStyleOptionsLayout.FitSliderRow(mcstyleShaderSliderRowUI);
				mcstyleShaderSliderRowUI.Configure(hairMorphControl.displayName, hairMorphControl.id, hairMorphControl.min, hairMorphControl.max, hairMorphControl.step, this.styleService.GetHairMorphValue(hairMorphControl.id), hairMorphControl.defaultValue, new Action<string, float>(this.OnHairMorphChanged));
				this._hairMorphRows.Add(mcstyleShaderSliderRowUI);
			}
			return flag;
		}

		// Token: 0x06003572 RID: 13682 RVA: 0x00130CB0 File Offset: 0x0012EEB0
		private void ClearHairMorphRows()
		{
			foreach (MCStyleShaderSliderRowUI mcstyleShaderSliderRowUI in this._hairMorphRows)
			{
				if (mcstyleShaderSliderRowUI != null)
				{
					mcstyleShaderSliderRowUI.gameObject.SetActive(false);
					Object.Destroy(mcstyleShaderSliderRowUI.gameObject);
				}
			}
			this._hairMorphRows.Clear();
		}

		// Token: 0x06003573 RID: 13683 RVA: 0x00130D28 File Offset: 0x0012EF28
		private void OnHairMorphChanged(string id, float value)
		{
			if (this.styleService == null)
			{
				return;
			}
			this._isHairMorphDriving = true;
			try
			{
				this.styleService.SetHairMorphValue(id, value);
			}
			finally
			{
				this._isHairMorphDriving = false;
			}
		}

		// Token: 0x06003574 RID: 13684 RVA: 0x00130D74 File Offset: 0x0012EF74
		private IEnumerator RebuildOptionsLayout(bool resetScroll)
		{
			yield return null;
			if (this._optionsScroll != null)
			{
				Canvas.ForceUpdateCanvases();
				LayoutRebuilder.ForceRebuildLayoutImmediate(this._optionsScroll.content);
				if (resetScroll)
				{
					this._optionsScroll.StopMovement();
					this._optionsScroll.verticalNormalizedPosition = 1f;
				}
			}
			this._layoutRefresh = null;
			yield break;
		}

		// Token: 0x06003575 RID: 13685 RVA: 0x00130D8A File Offset: 0x0012EF8A
		private void OnApplicationQuit()
		{
			this.ClearShaderRows();
		}

		// Token: 0x040035AF RID: 13743
		[Header("Dependencies")]
		[SerializeField]
		private MCStyleCustomizationService styleService;

		// Token: 0x040035B0 RID: 13744
		[Header("Section Roots")]
		[SerializeField]
		private GameObject panelRoot;

		// Token: 0x040035B1 RID: 13745
		[SerializeField]
		private GameObject textureSectionRoot;

		// Token: 0x040035B2 RID: 13746
		[SerializeField]
		private GameObject colorSectionRoot;

		// Token: 0x040035B3 RID: 13747
		[SerializeField]
		private GameObject secondaryColorRoot;

		// Token: 0x040035B4 RID: 13748
		[Header("Texture")]
		[SerializeField]
		private HorizontalSelector textureSelector;

		// Token: 0x040035B5 RID: 13749
		[Header("Color")]
		[Tooltip("Dedicated preset navigation row with embedded HorizontalSelector.")]
		[SerializeField]
		private ColorPreviewUI colorPresetPreview;

		// Token: 0x040035B6 RID: 13750
		[SerializeField]
		private ColorPreviewUI primaryColorPreview;

		// Token: 0x040035B7 RID: 13751
		[SerializeField]
		private ColorPreviewUI secondaryColorPreview;

		// Token: 0x040035B8 RID: 13752
		[SerializeField]
		private ColorPickerControlExtended colorPicker;

		// Token: 0x040035B9 RID: 13753
		[Header("Shader Controls (Optional)")]
		[SerializeField]
		private GameObject shaderControlsRoot;

		// Token: 0x040035BA RID: 13754
		[SerializeField]
		private Transform shaderSliderContainer;

		// Token: 0x040035BB RID: 13755
		[SerializeField]
		private MCStyleShaderSliderRowUI shaderSliderRowPrefab;

		// Token: 0x040035BC RID: 13756
		[Header("Actions")]
		[SerializeField]
		private Button closeButton;

		// Token: 0x040035BD RID: 13757
		[SerializeField]
		private ButtonManager closeButtonManager;

		// Token: 0x040035BE RID: 13758
		private bool _suppressEvents;

		// Token: 0x040035BF RID: 13759
		private bool _closedByUser;

		// Token: 0x040035C0 RID: 13760
		private bool _isColorPickerDriving;

		// Token: 0x040035C1 RID: 13761
		private readonly List<MCStyleShaderSliderRowUI> _spawnedShaderRows = new List<MCStyleShaderSliderRowUI>();

		// Token: 0x040035C2 RID: 13762
		private readonly List<int> _visibleSurfaceIndices = new List<int>();

		// Token: 0x040035C3 RID: 13763
		private readonly List<MCStyleShaderSliderRowUI> _hairMorphRows = new List<MCStyleShaderSliderRowUI>();

		// Token: 0x040035C4 RID: 13764
		private GameObject _hairMorphSection;

		// Token: 0x040035C5 RID: 13765
		private Transform _hairMorphContainer;

		// Token: 0x040035C6 RID: 13766
		private ScrollRect _optionsScroll;

		// Token: 0x040035C7 RID: 13767
		private MCStylePresetSO _shownHairPreset;

		// Token: 0x040035C8 RID: 13768
		private bool _isHairMorphDriving;

		// Token: 0x040035C9 RID: 13769
		private Coroutine _layoutRefresh;
	}
}
