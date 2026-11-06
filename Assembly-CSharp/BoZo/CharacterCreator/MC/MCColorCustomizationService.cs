using System;
using System.Collections.Generic;
using Bozo.AnimeCharacters;
using ProjectExhibitionist;
using UnityEngine;

namespace BoZo.CharacterCreator.MC
{
	// Token: 0x020007B9 RID: 1977
	public class MCColorCustomizationService : MonoBehaviour
	{
		// Token: 0x06003471 RID: 13425 RVA: 0x0012B12C File Offset: 0x0012932C
		private void Awake()
		{
			this.RebuildPresetMap();
			if (this.startupColorState == MCColorCustomizationService.StartupColorState.ApplyPresetDefaults)
			{
				this.ApplyDefaultPresetColors();
			}
		}

		// Token: 0x06003472 RID: 13426 RVA: 0x0012B144 File Offset: 0x00129344
		private void Start()
		{
			Debug.Log(string.Format("[MCColorUI] Start (autoBuildColorUI={0}, prefabAssigned={1}, containerAssigned={2})", this.autoBuildColorUI, this.colorPreviewPrefab != null, this.colorPreviewContainer != null), this);
			if (this.autoBuildColorUI)
			{
				this.BuildColorPartPreviews();
				return;
			}
			Debug.LogWarning("[MCColorUI] autoBuildColorUI is disabled: BuildColorPartPreviews was not called automatically.", this);
		}

		// Token: 0x06003473 RID: 13427 RVA: 0x0012B1A8 File Offset: 0x001293A8
		public void RebuildPresetMap()
		{
			this._presetsByPart.Clear();
			for (int i = 0; i < this.partPresets.Count; i++)
			{
				MCColorPresetSO mccolorPresetSO = this.partPresets[i];
				if (!(mccolorPresetSO == null))
				{
					this._presetsByPart[mccolorPresetSO.targetPart] = mccolorPresetSO;
				}
			}
		}

		// Token: 0x06003474 RID: 13428 RVA: 0x0012B200 File Offset: 0x00129400
		public void BuildColorPartPreviews()
		{
			this.ClearSpawnedPreviews();
			this._previewByPart.Clear();
			if (this.colorPreviewPrefab == null || this.colorPreviewContainer == null)
			{
				Debug.LogWarning(string.Format("[MCColorUI] Cannot build color previews (prefabAssigned={0}, containerAssigned={1}).", this.colorPreviewPrefab != null, this.colorPreviewContainer != null), this);
				this.RefreshAllPartPreviews();
				return;
			}
			this.RebuildPresetMap();
			foreach (KeyValuePair<MCColorPart, MCColorPresetSO> keyValuePair in this._presetsByPart)
			{
				MCColorPart key = keyValuePair.Key;
				MCColorPresetSO value = keyValuePair.Value;
				if (!(value == null))
				{
					Color color2;
					Color color = (this._currentColors.TryGetValue(key, out color2) ? color2 : value.GetDefaultColor());
					ColorPreviewUI colorPreviewUI = Object.Instantiate<ColorPreviewUI>(this.colorPreviewPrefab, this.colorPreviewContainer);
					colorPreviewUI.gameObject.SetActive(true);
					colorPreviewUI.Initialize(color, key.ToString());
					MCColorPart capturedPart = key;
					if (!this.ConfigureEmbeddedPresetUiForPreview(capturedPart, colorPreviewUI))
					{
						colorPreviewUI.OnPreviewClicked.AddListener(delegate
						{
							this.OnPartPreviewClicked(capturedPart, true);
						});
					}
					colorPreviewUI.BlendName = key.ToString();
					this._previewByPart[key] = colorPreviewUI;
					this._spawnedPreviews.Add(colorPreviewUI);
				}
			}
			this.RefreshAllPartPreviews();
		}

		// Token: 0x06003475 RID: 13429 RVA: 0x0012B3A0 File Offset: 0x001295A0
		private bool ConfigureEmbeddedPresetUiForPreview(MCColorPart part, ColorPreviewUI preview)
		{
			if (preview == null)
			{
				return false;
			}
			preview.OnEmbeddedPresetSelected.RemoveAllListeners();
			preview.OnCustomPickerRequested.RemoveAllListeners();
			int presetCount = this.GetPresetCount(part);
			if (presetCount <= 0)
			{
				preview.ConfigureSimpleMode();
				Debug.Log(string.Format("[MCColorUI] Part '{0}' => SIMPLE mode (presetCount={1})", part, presetCount), this);
				return false;
			}
			List<string> list = new List<string>(presetCount);
			for (int i = 0; i < presetCount; i++)
			{
				string text;
				if (!this.TryGetPresetDisplayName(part, i, out text))
				{
					text = string.Format("Preset {0}", i + 1);
				}
				list.Add(text);
			}
			int num = this.ResolveInitialSelectorIndex(part, presetCount);
			preview.ConfigureEmbeddedPresetMode(list, num);
			if (!preview.IsPresetMode)
			{
				Debug.LogWarning(string.Format("[MCColorUI] Part '{0}' has presets but preview '{1}' did not enter PRESET mode. Falling back to SIMPLE wiring.", part, preview.name), this);
				return false;
			}
			preview.OnEmbeddedPresetSelected.AddListener(delegate(int index)
			{
				this.OnPresetSelectorChanged(part, index);
			});
			if (this.IsCustomPickerAllowed(part))
			{
				preview.OnCustomPickerRequested.AddListener(delegate
				{
					this.OpenPartColorPicker(part);
				});
			}
			Debug.Log(string.Format("[MCColorUI] Part '{0}' => PRESET mode (presetCount={1}, pickerAllowed={2})", part, presetCount, this.IsCustomPickerAllowed(part)), this);
			return true;
		}

		// Token: 0x06003476 RID: 13430 RVA: 0x0012B514 File Offset: 0x00129714
		private Color GetCurrentOrDefaultColor(MCColorPart part)
		{
			Color color;
			if (this._currentColors.TryGetValue(part, out color))
			{
				return color;
			}
			MCColorPresetSO mccolorPresetSO;
			if (this.TryGetPresetAsset(part, out mccolorPresetSO) && mccolorPresetSO != null)
			{
				return mccolorPresetSO.GetDefaultColor();
			}
			return Color.white;
		}

		// Token: 0x06003477 RID: 13431 RVA: 0x0012B554 File Offset: 0x00129754
		private int ResolveInitialSelectorIndex(MCColorPart part, int presetCount)
		{
			if (presetCount <= 0)
			{
				return 0;
			}
			int num;
			if (this._selectedPresetIndexByPart.TryGetValue(part, out num) && num >= 0 && num < presetCount)
			{
				return num;
			}
			int num2;
			if (this.TryGetDefaultPresetIndex(part, out num2))
			{
				return Mathf.Clamp(num2, 0, presetCount - 1);
			}
			return 0;
		}

		// Token: 0x06003478 RID: 13432 RVA: 0x0012B598 File Offset: 0x00129798
		private void OnPresetSelectorChanged(MCColorPart part, int presetIndex)
		{
			this.TryApplyPreset(part, presetIndex);
		}

		// Token: 0x06003479 RID: 13433 RVA: 0x0012B5A4 File Offset: 0x001297A4
		public void ApplyDefaultPresetColors()
		{
			this.RebuildPresetMap();
			foreach (KeyValuePair<MCColorPart, MCColorPresetSO> keyValuePair in this._presetsByPart)
			{
				MCColorPresetSO value = keyValuePair.Value;
				if (!(value == null))
				{
					int num = 0;
					if (value.presets != null && value.presets.Count > 0)
					{
						num = Mathf.Clamp(value.defaultPresetIndex, 0, value.presets.Count - 1);
					}
					this._selectedPresetIndexByPart[keyValuePair.Key] = num;
					this.ApplyColorToPart(keyValuePair.Key, value.GetDefaultColor());
				}
			}
		}

		// Token: 0x0600347A RID: 13434 RVA: 0x0012B660 File Offset: 0x00129860
		public bool TryApplyPreset(MCColorPart part, int presetIndex)
		{
			MCColorPresetSO mccolorPresetSO;
			if (!this._presetsByPart.TryGetValue(part, out mccolorPresetSO) || mccolorPresetSO == null)
			{
				return false;
			}
			MCColorPresetSO.ColorPresetEntry colorPresetEntry;
			if (!mccolorPresetSO.TryGetPreset(presetIndex, out colorPresetEntry))
			{
				return false;
			}
			this._selectedPresetIndexByPart[part] = presetIndex;
			this.ApplyColorToPart(part, colorPresetEntry.color);
			this.SyncEmbeddedPreviewToPreset(part, presetIndex);
			return true;
		}

		// Token: 0x0600347B RID: 13435 RVA: 0x0012B6B8 File Offset: 0x001298B8
		public bool TrySetCustomColor(MCColorPart part, Color color)
		{
			MCColorPresetSO mccolorPresetSO;
			if (this._presetsByPart.TryGetValue(part, out mccolorPresetSO) && mccolorPresetSO != null && !mccolorPresetSO.allowCustomPicker)
			{
				return false;
			}
			this._selectedPresetIndexByPart[part] = -1;
			this.ApplyColorToPart(part, color);
			ColorPreviewUI colorPreviewUI;
			if (this._previewByPart.TryGetValue(part, out colorPreviewUI) && colorPreviewUI != null && this.GetPresetCount(part) > 0)
			{
				colorPreviewUI.SetCustomSelectionLabel(color);
			}
			return true;
		}

		// Token: 0x0600347C RID: 13436 RVA: 0x0012B728 File Offset: 0x00129928
		public bool TryGetCurrentColor(MCColorPart part, out Color color)
		{
			return this._currentColors.TryGetValue(part, out color);
		}

		// Token: 0x0600347D RID: 13437 RVA: 0x0012B737 File Offset: 0x00129937
		public bool TryGetSelectedPresetIndex(MCColorPart part, out int presetIndex)
		{
			return this._selectedPresetIndexByPart.TryGetValue(part, out presetIndex);
		}

		// Token: 0x0600347E RID: 13438 RVA: 0x0012B746 File Offset: 0x00129946
		public bool TryGetPresetAsset(MCColorPart part, out MCColorPresetSO presetAsset)
		{
			if (this._presetsByPart.Count == 0)
			{
				this.RebuildPresetMap();
			}
			return this._presetsByPart.TryGetValue(part, out presetAsset) && presetAsset != null;
		}

		// Token: 0x0600347F RID: 13439 RVA: 0x0012B774 File Offset: 0x00129974
		public bool IsCustomPickerAllowed(MCColorPart part)
		{
			MCColorPresetSO mccolorPresetSO;
			return !this.TryGetPresetAsset(part, out mccolorPresetSO) || mccolorPresetSO.allowCustomPicker;
		}

		// Token: 0x06003480 RID: 13440 RVA: 0x0012B794 File Offset: 0x00129994
		public int GetPresetCount(MCColorPart part)
		{
			MCColorPresetSO mccolorPresetSO;
			if (!this.TryGetPresetAsset(part, out mccolorPresetSO) || mccolorPresetSO.presets == null)
			{
				return 0;
			}
			return mccolorPresetSO.presets.Count;
		}

		// Token: 0x06003481 RID: 13441 RVA: 0x0012B7C4 File Offset: 0x001299C4
		public bool TryGetPresetDisplayName(MCColorPart part, int presetIndex, out string displayName)
		{
			displayName = string.Empty;
			MCColorPresetSO mccolorPresetSO;
			MCColorPresetSO.ColorPresetEntry colorPresetEntry;
			if (!this.TryGetPresetAsset(part, out mccolorPresetSO) || !mccolorPresetSO.TryGetPreset(presetIndex, out colorPresetEntry) || colorPresetEntry == null)
			{
				return false;
			}
			displayName = (string.IsNullOrWhiteSpace(colorPresetEntry.displayName) ? string.Format("Preset {0}", presetIndex + 1) : colorPresetEntry.displayName);
			return true;
		}

		// Token: 0x06003482 RID: 13442 RVA: 0x0012B820 File Offset: 0x00129A20
		public bool TryGetPresetColor(MCColorPart part, int presetIndex, out Color color)
		{
			color = Color.white;
			MCColorPresetSO mccolorPresetSO;
			MCColorPresetSO.ColorPresetEntry colorPresetEntry;
			if (!this.TryGetPresetAsset(part, out mccolorPresetSO) || !mccolorPresetSO.TryGetPreset(presetIndex, out colorPresetEntry) || colorPresetEntry == null)
			{
				return false;
			}
			color = colorPresetEntry.color;
			return true;
		}

		// Token: 0x06003483 RID: 13443 RVA: 0x0012B860 File Offset: 0x00129A60
		public bool TryGetDefaultPresetIndex(MCColorPart part, out int defaultPresetIndex)
		{
			defaultPresetIndex = 0;
			MCColorPresetSO mccolorPresetSO;
			if (!this.TryGetPresetAsset(part, out mccolorPresetSO))
			{
				return false;
			}
			int num = ((mccolorPresetSO.presets != null) ? mccolorPresetSO.presets.Count : 0);
			if (num <= 0)
			{
				return false;
			}
			defaultPresetIndex = Mathf.Clamp(mccolorPresetSO.defaultPresetIndex, 0, num - 1);
			return true;
		}

		// Token: 0x06003484 RID: 13444 RVA: 0x0012B8AC File Offset: 0x00129AAC
		public void CaptureState(CharacterStateData stateData)
		{
			if (stateData == null)
			{
				return;
			}
			if (stateData.colors == null)
			{
				stateData.colors = new List<ColorValue>();
			}
			stateData.colors.RemoveAll((ColorValue v) => v != null && string.Equals(v.category, "MCColor", StringComparison.OrdinalIgnoreCase));
			foreach (KeyValuePair<MCColorPart, Color> keyValuePair in this._currentColors)
			{
				stateData.colors.Add(new ColorValue("MCColor", keyValuePair.Key.ToString(), "Main", keyValuePair.Value));
			}
		}

		// Token: 0x06003485 RID: 13445 RVA: 0x0012B970 File Offset: 0x00129B70
		public void ApplyState(CharacterStateData stateData)
		{
			if (stateData == null || stateData.colors == null)
			{
				return;
			}
			for (int i = 0; i < stateData.colors.Count; i++)
			{
				ColorValue colorValue = stateData.colors[i];
				MCColorPart mccolorPart;
				if (colorValue != null && string.Equals(colorValue.category, "MCColor", StringComparison.OrdinalIgnoreCase) && MCColorCustomizationService.TryParsePart(colorValue.subcategory, out mccolorPart))
				{
					this.ApplyColorToPart(mccolorPart, colorValue.color);
					if (this.GetPresetCount(mccolorPart) > 0)
					{
						this._selectedPresetIndexByPart[mccolorPart] = -1;
						ColorPreviewUI colorPreviewUI;
						if (this._previewByPart.TryGetValue(mccolorPart, out colorPreviewUI) && colorPreviewUI != null)
						{
							colorPreviewUI.SetCustomSelectionLabel(colorValue.color);
						}
					}
				}
			}
		}

		// Token: 0x06003486 RID: 13446 RVA: 0x0012BA1D File Offset: 0x00129C1D
		private static bool TryParsePart(string value, out MCColorPart part)
		{
			if (!string.IsNullOrWhiteSpace(value) && Enum.TryParse<MCColorPart>(value, true, out part))
			{
				return true;
			}
			part = MCColorPart.Skin;
			return false;
		}

		// Token: 0x06003487 RID: 13447 RVA: 0x0012BA38 File Offset: 0x00129C38
		private void ApplyColorToPart(MCColorPart part, Color color)
		{
			this._currentColors[part] = color;
			for (int i = 0; i < this.bindings.Count; i++)
			{
				MCColorCustomizationService.PartMaterialBinding partMaterialBinding = this.bindings[i];
				if (partMaterialBinding != null && partMaterialBinding.part == part && !(partMaterialBinding.targetRenderer == null))
				{
					Material material = MCColorCustomizationService.GetMaterial(partMaterialBinding);
					if (!(material == null))
					{
						string text;
						string text2;
						string text3;
						string text4;
						this.ResolveLayerProperties(partMaterialBinding.colorLayer, out text, out text2, out text3, out text4);
						if (!string.IsNullOrWhiteSpace(text) && material.HasProperty(text))
						{
							material.SetColor(text, color);
						}
						if (!string.IsNullOrWhiteSpace(text2) && material.HasProperty(text2))
						{
							material.SetFloat(text2, (float)this.shaderDefaults.multiplyBlendingModeValue);
						}
						if (!string.IsNullOrWhiteSpace(text3) && material.HasProperty(text3))
						{
							material.SetFloat(text3, (float)this.shaderDefaults.enabledLayerValue);
						}
						if (!string.IsNullOrWhiteSpace(text4) && partMaterialBinding.maskTexture != null && material.HasProperty(text4))
						{
							material.SetTexture(text4, partMaterialBinding.maskTexture);
						}
					}
				}
			}
			this.RefreshPartPreview(part, color);
			if (part == MCColorPart.Skin && this.skinDecalController != null)
			{
				this.skinDecalController.SetSkinTone(color);
			}
		}

		// Token: 0x06003488 RID: 13448 RVA: 0x0012BB80 File Offset: 0x00129D80
		private void OnPartPreviewClicked(MCColorPart part, bool openPicker)
		{
			if (!openPicker)
			{
				return;
			}
			this.OpenPartColorPicker(part);
		}

		// Token: 0x06003489 RID: 13449 RVA: 0x0012BB90 File Offset: 0x00129D90
		private void OpenPartColorPicker(MCColorPart part)
		{
			if (this.IsCustomPickerAllowed(part))
			{
				if (this.colorPicker == null)
				{
					return;
				}
				Color white;
				if (!this.TryGetCurrentColor(part, out white))
				{
					white = Color.white;
				}
				this.colorPicker.ShowColorPicker(white, part.ToString(), delegate(Color color)
				{
					this.TrySetCustomColor(part, color);
				});
			}
		}

		// Token: 0x0600348A RID: 13450 RVA: 0x0012BC10 File Offset: 0x00129E10
		private void SyncEmbeddedPreviewToPreset(MCColorPart part, int presetIndex)
		{
			ColorPreviewUI colorPreviewUI;
			if (this._previewByPart.TryGetValue(part, out colorPreviewUI) && colorPreviewUI != null)
			{
				colorPreviewUI.SetEmbeddedPresetIndex(presetIndex);
			}
		}

		// Token: 0x0600348B RID: 13451 RVA: 0x0012BC40 File Offset: 0x00129E40
		private void RefreshPartPreview(MCColorPart part, Color color)
		{
			ColorPreviewUI colorPreviewUI;
			if (this._previewByPart.TryGetValue(part, out colorPreviewUI) && colorPreviewUI != null)
			{
				colorPreviewUI.PreviewColor = color;
			}
		}

		// Token: 0x0600348C RID: 13452 RVA: 0x0012BC70 File Offset: 0x00129E70
		private void RefreshAllPartPreviews()
		{
			foreach (KeyValuePair<MCColorPart, ColorPreviewUI> keyValuePair in this._previewByPart)
			{
				MCColorPart key = keyValuePair.Key;
				ColorPreviewUI value = keyValuePair.Value;
				if (!(value == null))
				{
					Color color;
					if (this._currentColors.TryGetValue(key, out color))
					{
						value.PreviewColor = color;
					}
					value.BlendName = key.ToString();
				}
			}
		}

		// Token: 0x0600348D RID: 13453 RVA: 0x0012BD00 File Offset: 0x00129F00
		private void ClearSpawnedPreviews()
		{
			for (int i = 0; i < this._spawnedPreviews.Count; i++)
			{
				if (this._spawnedPreviews[i] != null)
				{
					Object.Destroy(this._spawnedPreviews[i].gameObject);
				}
			}
			this._spawnedPreviews.Clear();
		}

		// Token: 0x0600348E RID: 13454 RVA: 0x0012BD58 File Offset: 0x00129F58
		private void ResolveLayerProperties(MCColorCustomizationService.ColorLayer layer, out string colorPropertyName, out string blendModePropertyName, out string enableLayerPropertyName, out string maskPropertyName)
		{
			if (layer == MCColorCustomizationService.ColorLayer.Main3rd)
			{
				colorPropertyName = this.shaderDefaults.main3rdColorPropertyName;
				blendModePropertyName = this.shaderDefaults.main3rdBlendModePropertyName;
				enableLayerPropertyName = this.shaderDefaults.main3rdEnablePropertyName;
				maskPropertyName = this.shaderDefaults.main3rdMaskPropertyName;
				return;
			}
			colorPropertyName = this.shaderDefaults.main2ndColorPropertyName;
			blendModePropertyName = this.shaderDefaults.main2ndBlendModePropertyName;
			enableLayerPropertyName = this.shaderDefaults.main2ndEnablePropertyName;
			maskPropertyName = this.shaderDefaults.main2ndMaskPropertyName;
		}

		// Token: 0x0600348F RID: 13455 RVA: 0x0012BDD8 File Offset: 0x00129FD8
		private static Material GetMaterial(MCColorCustomizationService.PartMaterialBinding binding)
		{
			if (binding == null || binding.targetRenderer == null)
			{
				return null;
			}
			Material[] materials = binding.targetRenderer.materials;
			if (materials == null || binding.materialIndex < 0 || binding.materialIndex >= materials.Length)
			{
				return null;
			}
			return materials[binding.materialIndex];
		}

		// Token: 0x040034CB RID: 13515
		[SerializeField]
		private MCColorCustomizationService.ShaderBindingDefaults shaderDefaults = new MCColorCustomizationService.ShaderBindingDefaults();

		// Token: 0x040034CC RID: 13516
		[SerializeField]
		private List<MCColorCustomizationService.PartMaterialBinding> bindings = new List<MCColorCustomizationService.PartMaterialBinding>();

		// Token: 0x040034CD RID: 13517
		[SerializeField]
		private List<MCColorPresetSO> partPresets = new List<MCColorPresetSO>();

		// Token: 0x040034CE RID: 13518
		[Header("Decal Integration")]
		[Tooltip("Optional: SkinDecalController to sync skin tone for matchSkinTone decals.")]
		[SerializeField]
		private SkinDecalController skinDecalController;

		// Token: 0x040034CF RID: 13519
		[Header("Initialization")]
		[SerializeField]
		private MCColorCustomizationService.StartupColorState startupColorState;

		// Token: 0x040034D0 RID: 13520
		[Header("UI (Optional)")]
		[SerializeField]
		private bool spawnPreviewsForUnboundParts = true;

		// Token: 0x040034D1 RID: 13521
		[SerializeField]
		private ColorPreviewUI colorPreviewPrefab;

		// Token: 0x040034D2 RID: 13522
		[SerializeField]
		private Transform colorPreviewContainer;

		// Token: 0x040034D3 RID: 13523
		[SerializeField]
		private ColorPickerControlExtended colorPicker;

		// Token: 0x040034D4 RID: 13524
		[SerializeField]
		private bool autoBuildColorUI = true;

		// Token: 0x040034D5 RID: 13525
		private readonly Dictionary<MCColorPart, Color> _currentColors = new Dictionary<MCColorPart, Color>();

		// Token: 0x040034D6 RID: 13526
		private readonly Dictionary<MCColorPart, MCColorPresetSO> _presetsByPart = new Dictionary<MCColorPart, MCColorPresetSO>();

		// Token: 0x040034D7 RID: 13527
		private readonly Dictionary<MCColorPart, ColorPreviewUI> _previewByPart = new Dictionary<MCColorPart, ColorPreviewUI>();

		// Token: 0x040034D8 RID: 13528
		private readonly List<ColorPreviewUI> _spawnedPreviews = new List<ColorPreviewUI>();

		// Token: 0x040034D9 RID: 13529
		private readonly Dictionary<MCColorPart, int> _selectedPresetIndexByPart = new Dictionary<MCColorPart, int>();

		// Token: 0x020007BA RID: 1978
		public enum ColorLayer
		{
			// Token: 0x040034DB RID: 13531
			Main2nd,
			// Token: 0x040034DC RID: 13532
			Main3rd
		}

		// Token: 0x020007BB RID: 1979
		[Serializable]
		public class PartMaterialBinding
		{
			// Token: 0x040034DD RID: 13533
			public MCColorPart part;

			// Token: 0x040034DE RID: 13534
			public Renderer targetRenderer;

			// Token: 0x040034DF RID: 13535
			[Min(0f)]
			public int materialIndex;

			// Token: 0x040034E0 RID: 13536
			[Header("Layer")]
			public MCColorCustomizationService.ColorLayer colorLayer;

			// Token: 0x040034E1 RID: 13537
			[Header("Optional Mask")]
			public Texture maskTexture;
		}

		// Token: 0x020007BC RID: 1980
		[Serializable]
		private class ShaderBindingDefaults
		{
			// Token: 0x040034E2 RID: 13538
			[Header("Main2nd (Liltoon)")]
			public string main2ndColorPropertyName = "_Color2nd";

			// Token: 0x040034E3 RID: 13539
			public string main2ndBlendModePropertyName = "_Main2ndTexBlendMode";

			// Token: 0x040034E4 RID: 13540
			public string main2ndEnablePropertyName = "_UseMain2ndTex";

			// Token: 0x040034E5 RID: 13541
			public string main2ndMaskPropertyName = "_Main2ndBlendMask";

			// Token: 0x040034E6 RID: 13542
			[Header("Main3rd (Liltoon)")]
			public string main3rdColorPropertyName = "_Color3rd";

			// Token: 0x040034E7 RID: 13543
			public string main3rdBlendModePropertyName = "_Main3rdTexBlendMode";

			// Token: 0x040034E8 RID: 13544
			public string main3rdEnablePropertyName = "_UseMain3rdTex";

			// Token: 0x040034E9 RID: 13545
			public string main3rdMaskPropertyName = "_Main3rdBlendMask";

			// Token: 0x040034EA RID: 13546
			[Header("Shared")]
			public int multiplyBlendingModeValue = 3;

			// Token: 0x040034EB RID: 13547
			public int enabledLayerValue = 1;
		}

		// Token: 0x020007BD RID: 1981
		public enum StartupColorState
		{
			// Token: 0x040034ED RID: 13549
			KeepSceneState,
			// Token: 0x040034EE RID: 13550
			ApplyPresetDefaults
		}
	}
}
