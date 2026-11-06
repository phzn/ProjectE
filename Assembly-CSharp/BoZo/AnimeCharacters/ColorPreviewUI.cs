using System;
using System.Collections.Generic;
using Features.Localization;
using Features.MainMenu;
using Michsky.MUIP;
using ProjectExhibitionist.CharacterCustomization.Core.Runtime;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Bozo.AnimeCharacters
{
	// Token: 0x02000893 RID: 2195
	public class ColorPreviewUI : MonoBehaviour
	{
		// Token: 0x17000A1A RID: 2586
		// (get) Token: 0x06003B01 RID: 15105 RVA: 0x0015E9DC File Offset: 0x0015CBDC
		public bool IsPresetMode
		{
			get
			{
				return this._usingEmbeddedPresetMode;
			}
		}

		// Token: 0x17000A1B RID: 2587
		// (get) Token: 0x06003B02 RID: 15106 RVA: 0x0015E9E4 File Offset: 0x0015CBE4
		// (set) Token: 0x06003B03 RID: 15107 RVA: 0x0015E9EC File Offset: 0x0015CBEC
		public int ChannelIndex { get; set; }

		// Token: 0x17000A1C RID: 2588
		// (get) Token: 0x06003B04 RID: 15108 RVA: 0x0015E9F5 File Offset: 0x0015CBF5
		// (set) Token: 0x06003B05 RID: 15109 RVA: 0x0015EA00 File Offset: 0x0015CC00
		public Color PreviewColor
		{
			get
			{
				return this._cachedColor;
			}
			set
			{
				this._cachedColor = value;
				Image activeImage = this.ActiveImage;
				if (activeImage != null)
				{
					activeImage.color = value;
				}
				this.RefreshSecondaryText(value);
			}
		}

		// Token: 0x17000A1D RID: 2589
		// (get) Token: 0x06003B06 RID: 15110 RVA: 0x0015EA32 File Offset: 0x0015CC32
		// (set) Token: 0x06003B07 RID: 15111 RVA: 0x0015EA5D File Offset: 0x0015CC5D
		public string BlendName
		{
			get
			{
				string text;
				if ((text = this._sourceBlendName) == null)
				{
					if (!(this.blendNameText != null))
					{
						return string.Empty;
					}
					text = this.blendNameText.text;
				}
				return text;
			}
			set
			{
				this._sourceBlendName = value;
				CustomizationPresentation.BindColorLabel(this.blendNameText, value, false);
			}
		}

		// Token: 0x17000A1E RID: 2590
		// (get) Token: 0x06003B08 RID: 15112 RVA: 0x0015EA74 File Offset: 0x0015CC74
		private Image ActiveImage
		{
			get
			{
				if (this._usingEmbeddedPresetMode)
				{
					if (!(this.presetPreviewImage != null))
					{
						return this.previewImage;
					}
					return this.presetPreviewImage;
				}
				else
				{
					if (!(this.simplePreviewImage != null))
					{
						return this.previewImage;
					}
					return this.simplePreviewImage;
				}
			}
		}

		// Token: 0x17000A1F RID: 2591
		// (get) Token: 0x06003B09 RID: 15113 RVA: 0x0015EAC0 File Offset: 0x0015CCC0
		private TextMeshProUGUI ActiveHexText
		{
			get
			{
				if (this._usingEmbeddedPresetMode)
				{
					if (!(this.presetHexValueText != null))
					{
						return this.hexValueText;
					}
					return this.presetHexValueText;
				}
				else
				{
					if (!(this.simpleHexValueText != null))
					{
						return this.hexValueText;
					}
					return this.simpleHexValueText;
				}
			}
		}

		// Token: 0x17000A20 RID: 2592
		// (get) Token: 0x06003B0A RID: 15114 RVA: 0x0015EB0C File Offset: 0x0015CD0C
		private Button ActiveButton
		{
			get
			{
				if (this._usingEmbeddedPresetMode)
				{
					if (!(this.presetSelectButton != null))
					{
						return this.selectButton;
					}
					return this.presetSelectButton;
				}
				else
				{
					if (!(this.simpleSelectButton != null))
					{
						return this.selectButton;
					}
					return this.simpleSelectButton;
				}
			}
		}

		// Token: 0x06003B0B RID: 15115 RVA: 0x0015EB58 File Offset: 0x0015CD58
		private void Awake()
		{
			if (this.embeddedPresetSelector != null)
			{
				this.embeddedPresetSelector.onValueChanged.RemoveListener(new UnityAction<int>(this.OnSelectorValueChanged));
				this.embeddedPresetSelector.onValueChanged.AddListener(new UnityAction<int>(this.OnSelectorValueChanged));
			}
			if (this._usingEmbeddedPresetMode)
			{
				this.ActivatePresetView();
				return;
			}
			this.ActivateSimpleView();
		}

		// Token: 0x06003B0C RID: 15116 RVA: 0x0015EBC0 File Offset: 0x0015CDC0
		private void OnDestroy()
		{
			this.UnbindButton();
			if (this.embeddedPresetSelector != null)
			{
				this.embeddedPresetSelector.onValueChanged.RemoveListener(new UnityAction<int>(this.OnSelectorValueChanged));
			}
		}

		// Token: 0x06003B0D RID: 15117 RVA: 0x0015EBF2 File Offset: 0x0015CDF2
		public void Initialize(Color color, string name)
		{
			color.a = 1f;
			this.PreviewColor = color;
			this.BlendName = name;
		}

		// Token: 0x06003B0E RID: 15118 RVA: 0x0015EC0E File Offset: 0x0015CE0E
		public void Initialize(Material material, string property, string displayName, Color color)
		{
			this.targetMaterial = material;
			this.propertyName = property;
			color.a = 1f;
			this.PreviewColor = color;
			this.BlendName = displayName;
		}

		// Token: 0x06003B0F RID: 15119 RVA: 0x0015EC39 File Offset: 0x0015CE39
		public void InitializeLinked(string displayName, Color color, List<ColorPreviewUI.MaterialPropertyPair> linkedPairs)
		{
			this.isLinkedColorGroup = true;
			this.BlendName = displayName;
			color.a = 1f;
			this.PreviewColor = color;
			this.linkedMaterialProperties = new List<ColorPreviewUI.MaterialPropertyPair>(linkedPairs);
		}

		// Token: 0x06003B10 RID: 15120 RVA: 0x0015EC68 File Offset: 0x0015CE68
		public void ConfigureSimpleMode()
		{
			this._usingEmbeddedPresetMode = false;
			this._embeddedPresetNames.Clear();
			this._presetModeSelectionLabel = string.Empty;
			this.ActivateSimpleView();
		}

		// Token: 0x06003B11 RID: 15121 RVA: 0x0015EC90 File Offset: 0x0015CE90
		public void ConfigureBodyFineTuneStyle()
		{
			base.gameObject.name = "Body Color - " + this.BlendName;
			RectTransform rectTransform = base.transform as RectTransform;
			if (rectTransform != null)
			{
				rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, 100f);
			}
			LayoutElement layoutElement = base.GetComponent<LayoutElement>();
			if (layoutElement == null)
			{
				layoutElement = base.gameObject.AddComponent<LayoutElement>();
			}
			layoutElement.minHeight = 100f;
			layoutElement.preferredHeight = 100f;
			layoutElement.flexibleHeight = 0f;
			VerticalLayoutGroup component = base.GetComponent<VerticalLayoutGroup>();
			if (component != null)
			{
				component.padding = new RectOffset(0, 0, 4, 4);
				component.spacing = 5f;
				component.childForceExpandHeight = false;
				component.childControlHeight = false;
			}
			if (this.blendNameText != null)
			{
				RectTransform rectTransform2 = this.blendNameText.transform as RectTransform;
				if (rectTransform2 != null)
				{
					rectTransform2.sizeDelta = new Vector2(rectTransform2.sizeDelta.x, 29f);
				}
				this.blendNameText.fontSize = 17f;
				this.blendNameText.fontStyle = FontStyles.Bold | FontStyles.SmallCaps;
				this.blendNameText.alignment = TextAlignmentOptions.MidlineLeft;
				this.blendNameText.characterSpacing = 0f;
				this.blendNameText.raycastTarget = false;
			}
			Button activeButton = this.ActiveButton;
			if (activeButton != null)
			{
				RectTransform rectTransform3 = activeButton.transform as RectTransform;
				if (rectTransform3 != null)
				{
					rectTransform3.sizeDelta = new Vector2(rectTransform3.sizeDelta.x, 55f);
				}
				Image component2 = activeButton.GetComponent<Image>();
				if (component2 != null)
				{
					component2.color = new Color32(42, 27, 55, 220);
					component2.maskable = true;
					activeButton.targetGraphic = component2;
				}
				ColorPreviewUI.ProtectColorGraphicsFromHover(activeButton);
			}
			Image activeImage = this.ActiveImage;
			if (activeImage != null)
			{
				activeImage.maskable = true;
				activeImage.raycastTarget = false;
			}
			TextMeshProUGUI activeHexText = this.ActiveHexText;
			if (activeHexText != null)
			{
				activeHexText.fontSize = 16f;
				activeHexText.fontStyle = FontStyles.Bold;
				activeHexText.alignment = TextAlignmentOptions.MidlineLeft;
				activeHexText.raycastTarget = false;
			}
		}

		// Token: 0x06003B12 RID: 15122 RVA: 0x0015EED4 File Offset: 0x0015D0D4
		public void ConfigureEmbeddedPresetMode(IList<string> presetNames, int selectedIndex)
		{
			if (presetNames == null || presetNames.Count <= 0)
			{
				Debug.LogWarning("[ColorPreviewUI] ConfigureEmbeddedPresetMode called with no presets, falling back to simple.", this);
				this.ConfigureSimpleMode();
				return;
			}
			this.ResolveMissingPresetRefs();
			if (this.embeddedPresetSelector == null)
			{
				Debug.LogWarning("[ColorPreviewUI] embeddedPresetSelector is null on '" + base.gameObject.name + "', cannot enter preset mode. Falling back to simple.", this);
				this.ConfigureSimpleMode();
				return;
			}
			this._usingEmbeddedPresetMode = true;
			this._embeddedPresetNames.Clear();
			for (int i = 0; i < presetNames.Count; i++)
			{
				this._embeddedPresetNames.Add(string.IsNullOrWhiteSpace(presetNames[i]) ? string.Format("Preset {0}", i + 1) : presetNames[i]);
			}
			this.ActivatePresetView();
			this.PopulateSelector(selectedIndex);
			this.EnsureSelectorMainContentVisible();
		}

		// Token: 0x06003B13 RID: 15123 RVA: 0x0015EFA4 File Offset: 0x0015D1A4
		public void SetEmbeddedPresetIndex(int presetIndex)
		{
			if (!this._usingEmbeddedPresetMode || this.embeddedPresetSelector == null || this._embeddedPresetNames.Count == 0)
			{
				return;
			}
			int num = Mathf.Clamp(presetIndex, 0, this._embeddedPresetNames.Count - 1);
			this._suppressSelectorEvent = true;
			this.embeddedPresetSelector.defaultIndex = num;
			this.embeddedPresetSelector.SetupSelector();
			CustomizationHorizontalSelectorLocalization.Bind(this.embeddedPresetSelector);
			this._suppressSelectorEvent = false;
			this.SetPresetModeSelectionLabel(this._embeddedPresetNames[num], this._cachedColor);
			this.EnsureSelectorMainContentVisible();
		}

		// Token: 0x06003B14 RID: 15124 RVA: 0x0015F038 File Offset: 0x0015D238
		public void SetCustomSelectionLabel(Color color)
		{
			this.SetPresetModeSelectionLabel("#" + ColorUtility.ToHtmlStringRGB(color).ToUpper(), color);
		}

		// Token: 0x06003B15 RID: 15125 RVA: 0x0015F058 File Offset: 0x0015D258
		private void ActivateSimpleView()
		{
			if (this.classicPreviewRoot != null)
			{
				this.classicPreviewRoot.SetActive(true);
			}
			if (this.presetPreviewRoot != null)
			{
				this.presetPreviewRoot.SetActive(false);
			}
			this.RebindButton();
			this.SyncImageColor();
			this.ValidateViewRootsAndLog("Simple");
		}

		// Token: 0x06003B16 RID: 15126 RVA: 0x0015F0B0 File Offset: 0x0015D2B0
		private void ActivatePresetView()
		{
			if (this.classicPreviewRoot != null)
			{
				this.classicPreviewRoot.SetActive(false);
			}
			if (this.presetPreviewRoot != null)
			{
				this.presetPreviewRoot.SetActive(true);
			}
			this.RebindButton();
			this.SyncImageColor();
			this.ValidateViewRootsAndLog("Preset");
		}

		// Token: 0x06003B17 RID: 15127 RVA: 0x0015F108 File Offset: 0x0015D308
		private void SyncImageColor()
		{
			Image activeImage = this.ActiveImage;
			if (activeImage != null)
			{
				activeImage.color = this._cachedColor;
			}
			this.RefreshSecondaryText(this._cachedColor);
		}

		// Token: 0x06003B18 RID: 15128 RVA: 0x0015F140 File Offset: 0x0015D340
		private void RebindButton()
		{
			Button button = this.ActiveButton;
			if (button == null)
			{
				button = base.GetComponent<Button>();
			}
			if (this._boundButton == button)
			{
				return;
			}
			this.UnbindButton();
			this._boundButton = button;
			if (this._boundButton != null)
			{
				this._boundButton.onClick.AddListener(new UnityAction(this.OnClick));
				ColorPreviewUI.ProtectColorGraphicsFromHover(this._boundButton);
			}
		}

		// Token: 0x06003B19 RID: 15129 RVA: 0x0015F1B8 File Offset: 0x0015D3B8
		private static void ProtectColorGraphicsFromHover(Button button)
		{
			if (button == null)
			{
				return;
			}
			MainMenuButtonFx mainMenuButtonFx = button.GetComponent<MainMenuButtonFx>();
			if (mainMenuButtonFx == null)
			{
				mainMenuButtonFx = button.gameObject.AddComponent<MainMenuButtonFx>();
			}
			mainMenuButtonFx.SetTintChildGraphics(false);
			mainMenuButtonFx.SetScaleEffectsEnabled(false);
		}

		// Token: 0x06003B1A RID: 15130 RVA: 0x0015F1F9 File Offset: 0x0015D3F9
		private void UnbindButton()
		{
			if (this._boundButton != null)
			{
				this._boundButton.onClick.RemoveListener(new UnityAction(this.OnClick));
				this._boundButton = null;
			}
		}

		// Token: 0x06003B1B RID: 15131 RVA: 0x0015F22C File Offset: 0x0015D42C
		private void OnClick()
		{
			this.OnPreviewClicked.Invoke();
			if (this._usingEmbeddedPresetMode)
			{
				this.OnCustomPickerRequested.Invoke();
				return;
			}
			if (this.isLinkedColorGroup)
			{
				if (this.linkedMaterialProperties.Count > 0)
				{
					this.OnLinkedColorsSelected.Invoke(this.linkedMaterialProperties, this.PreviewColor);
					return;
				}
			}
			else if (this.targetMaterial != null && !string.IsNullOrEmpty(this.propertyName))
			{
				this.OnColorSelected.Invoke(this.targetMaterial, this.propertyName, this.PreviewColor);
			}
		}

		// Token: 0x06003B1C RID: 15132 RVA: 0x0015F2C0 File Offset: 0x0015D4C0
		private void PopulateSelector(int selectedIndex)
		{
			this.embeddedPresetSelector.onValueChanged.RemoveListener(new UnityAction<int>(this.OnSelectorValueChanged));
			this.embeddedPresetSelector.items.Clear();
			for (int i = 0; i < this._embeddedPresetNames.Count; i++)
			{
				this.embeddedPresetSelector.CreateNewItem(this._embeddedPresetNames[i]);
			}
			if (this.embeddedPresetSelector.enableIndicators && (this.embeddedPresetSelector.indicatorParent == null || this.embeddedPresetSelector.indicatorObject == null))
			{
				this.embeddedPresetSelector.enableIndicators = false;
			}
			int num = Mathf.Clamp(selectedIndex, 0, this._embeddedPresetNames.Count - 1);
			this.embeddedPresetSelector.defaultIndex = num;
			this.embeddedPresetSelector.SetupSelector();
			CustomizationHorizontalSelectorLocalization.Bind(this.embeddedPresetSelector);
			this.embeddedPresetSelector.onValueChanged.AddListener(new UnityAction<int>(this.OnSelectorValueChanged));
			this.SetPresetModeSelectionLabel(this._embeddedPresetNames[num], this._cachedColor);
		}

		// Token: 0x06003B1D RID: 15133 RVA: 0x0015F3D0 File Offset: 0x0015D5D0
		private void OnSelectorValueChanged(int index)
		{
			if (this._suppressSelectorEvent || this._embeddedPresetNames.Count == 0)
			{
				return;
			}
			int num = Mathf.Clamp(index, 0, this._embeddedPresetNames.Count - 1);
			this.SetPresetModeSelectionLabel(this._embeddedPresetNames[num], this._cachedColor);
			this.OnEmbeddedPresetSelected.Invoke(num);
		}

		// Token: 0x06003B1E RID: 15134 RVA: 0x0015F42C File Offset: 0x0015D62C
		private void EnsureSelectorMainContentVisible()
		{
			if (this.embeddedPresetSelector == null)
			{
				return;
			}
			Transform transform = this.embeddedPresetSelector.transform;
			Transform transform2 = transform.Find("Main Content");
			if (transform2 != null)
			{
				if (!transform2.gameObject.activeSelf)
				{
					transform2.gameObject.SetActive(true);
				}
				CanvasGroup component = transform2.GetComponent<CanvasGroup>();
				if (component != null)
				{
					component.alpha = 1f;
				}
			}
			Transform transform3 = transform.Find("Helper Content");
			if (transform3 != null)
			{
				CanvasGroup component2 = transform3.GetComponent<CanvasGroup>();
				if (component2 != null)
				{
					component2.alpha = 0f;
				}
			}
		}

		// Token: 0x06003B1F RID: 15135 RVA: 0x0015F4CC File Offset: 0x0015D6CC
		private void RefreshSecondaryText(Color color)
		{
			TextMeshProUGUI activeHexText = this.ActiveHexText;
			if (activeHexText != null)
			{
				if (this._usingEmbeddedPresetMode)
				{
					CustomizationPresentation.BindColorLabel(activeHexText, string.IsNullOrWhiteSpace(this._presetModeSelectionLabel) ? ColorUtility.ToHtmlStringRGB(color).ToUpper() : this._presetModeSelectionLabel, false);
					return;
				}
				LocalizedGameText.BindSource(activeHexText, "cielos_apps", ColorUtility.ToHtmlStringRGB(color).ToUpper(), null);
			}
		}

		// Token: 0x06003B20 RID: 15136 RVA: 0x0015F531 File Offset: 0x0015D731
		private void SetPresetModeSelectionLabel(string label, Color color)
		{
			this._presetModeSelectionLabel = label ?? string.Empty;
			this.RefreshSecondaryText(color);
		}

		// Token: 0x06003B21 RID: 15137 RVA: 0x0015F54C File Offset: 0x0015D74C
		private void ValidateViewRootsAndLog(string mode)
		{
			bool flag = string.Equals(mode, "Simple", StringComparison.OrdinalIgnoreCase);
			bool flag2 = string.Equals(mode, "Preset", StringComparison.OrdinalIgnoreCase);
			if (flag && this.classicPreviewRoot == null && this.simplePreviewImage == null && this.previewImage == null)
			{
				if (!this._loggedMissingSimpleRoot)
				{
					this._loggedMissingSimpleRoot = true;
					Debug.LogWarning(string.Concat(new string[] { "[ColorPreviewUI] ", base.name, " mode=", mode, ": classicPreviewRoot is missing." }), this);
				}
				return;
			}
			if (flag2 && this.presetPreviewRoot == null)
			{
				if (!this._loggedMissingPresetRoot)
				{
					this._loggedMissingPresetRoot = true;
					Debug.LogWarning(string.Concat(new string[] { "[ColorPreviewUI] ", base.name, " mode=", mode, ": presetPreviewRoot is missing." }), this);
				}
				return;
			}
			if (this.classicPreviewRoot != null && this.presetPreviewRoot != null && this.classicPreviewRoot == this.presetPreviewRoot && !this._loggedSameRootReference)
			{
				this._loggedSameRootReference = true;
				Debug.LogWarning(string.Concat(new string[]
				{
					"[ColorPreviewUI] ",
					base.name,
					" mode=",
					mode,
					": classicPreviewRoot and presetPreviewRoot reference the SAME GameObject ('",
					this.classicPreviewRoot.name,
					"')."
				}), this);
			}
		}

		// Token: 0x06003B22 RID: 15138 RVA: 0x0015F6C0 File Offset: 0x0015D8C0
		private void ResolveMissingPresetRefs()
		{
			if (this.embeddedPresetSelector == null)
			{
				if (this.presetPreviewRoot != null)
				{
					this.embeddedPresetSelector = this.presetPreviewRoot.GetComponentInChildren<HorizontalSelector>(true);
				}
				if (this.embeddedPresetSelector == null)
				{
					this.embeddedPresetSelector = base.GetComponentInChildren<HorizontalSelector>(true);
				}
			}
			if (this.presetPreviewRoot != null)
			{
				if (this.presetPreviewImage == null)
				{
					this.presetPreviewImage = this.presetPreviewRoot.GetComponentInChildren<Image>(true);
				}
				if (this.presetHexValueText == null)
				{
					TextMeshProUGUI[] componentsInChildren = this.presetPreviewRoot.GetComponentsInChildren<TextMeshProUGUI>(true);
					for (int i = 0; i < componentsInChildren.Length; i++)
					{
						if (componentsInChildren[i] != null && componentsInChildren[i] != this.blendNameText)
						{
							this.presetHexValueText = componentsInChildren[i];
							break;
						}
					}
				}
				if (this.presetSelectButton == null)
				{
					this.presetSelectButton = this.presetPreviewRoot.GetComponentInChildren<Button>(true);
				}
			}
		}

		// Token: 0x04003B34 RID: 15156
		[Header("Shared Refs")]
		[SerializeField]
		private TextMeshProUGUI blendNameText;

		// Token: 0x04003B35 RID: 15157
		[Header("Simple View (Classic)")]
		[SerializeField]
		private GameObject classicPreviewRoot;

		// Token: 0x04003B36 RID: 15158
		[SerializeField]
		private Image simplePreviewImage;

		// Token: 0x04003B37 RID: 15159
		[SerializeField]
		private TextMeshProUGUI simpleHexValueText;

		// Token: 0x04003B38 RID: 15160
		[SerializeField]
		private Button simpleSelectButton;

		// Token: 0x04003B39 RID: 15161
		[Header("Preset View (MC)")]
		[SerializeField]
		private GameObject presetPreviewRoot;

		// Token: 0x04003B3A RID: 15162
		[SerializeField]
		private Image presetPreviewImage;

		// Token: 0x04003B3B RID: 15163
		[SerializeField]
		private TextMeshProUGUI presetHexValueText;

		// Token: 0x04003B3C RID: 15164
		[SerializeField]
		private Button presetSelectButton;

		// Token: 0x04003B3D RID: 15165
		[SerializeField]
		private HorizontalSelector embeddedPresetSelector;

		// Token: 0x04003B3E RID: 15166
		[Header("Legacy Fallback Refs")]
		[Tooltip("Used only when per-mode refs above are null (backward compatibility with NPC prefabs).")]
		[SerializeField]
		private Image previewImage;

		// Token: 0x04003B3F RID: 15167
		[SerializeField]
		private TextMeshProUGUI hexValueText;

		// Token: 0x04003B40 RID: 15168
		[SerializeField]
		private Button selectButton;

		// Token: 0x04003B41 RID: 15169
		private Material targetMaterial;

		// Token: 0x04003B42 RID: 15170
		private string propertyName;

		// Token: 0x04003B43 RID: 15171
		private bool isLinkedColorGroup;

		// Token: 0x04003B44 RID: 15172
		[NonSerialized]
		private List<ColorPreviewUI.MaterialPropertyPair> linkedMaterialProperties = new List<ColorPreviewUI.MaterialPropertyPair>();

		// Token: 0x04003B45 RID: 15173
		[HideInInspector]
		public UnityEvent<Material, string, Color> OnColorSelected = new UnityEvent<Material, string, Color>();

		// Token: 0x04003B46 RID: 15174
		[NonSerialized]
		public UnityEvent<List<ColorPreviewUI.MaterialPropertyPair>, Color> OnLinkedColorsSelected = new UnityEvent<List<ColorPreviewUI.MaterialPropertyPair>, Color>();

		// Token: 0x04003B47 RID: 15175
		[HideInInspector]
		public UnityEvent OnPreviewClicked = new UnityEvent();

		// Token: 0x04003B48 RID: 15176
		[HideInInspector]
		public UnityEvent<int> OnEmbeddedPresetSelected = new UnityEvent<int>();

		// Token: 0x04003B49 RID: 15177
		[HideInInspector]
		public UnityEvent OnCustomPickerRequested = new UnityEvent();

		// Token: 0x04003B4A RID: 15178
		private readonly List<string> _embeddedPresetNames = new List<string>();

		// Token: 0x04003B4B RID: 15179
		private bool _usingEmbeddedPresetMode;

		// Token: 0x04003B4C RID: 15180
		private bool _suppressSelectorEvent;

		// Token: 0x04003B4D RID: 15181
		private Color _cachedColor = Color.white;

		// Token: 0x04003B4E RID: 15182
		private string _presetModeSelectionLabel = string.Empty;

		// Token: 0x04003B4F RID: 15183
		private Button _boundButton;

		// Token: 0x04003B50 RID: 15184
		private bool _loggedMissingSimpleRoot;

		// Token: 0x04003B51 RID: 15185
		private bool _loggedMissingPresetRoot;

		// Token: 0x04003B52 RID: 15186
		private bool _loggedSameRootReference;

		// Token: 0x04003B54 RID: 15188
		private string _sourceBlendName;

		// Token: 0x02000894 RID: 2196
		[Serializable]
		public class MaterialPropertyPair
		{
			// Token: 0x06003B24 RID: 15140 RVA: 0x0015F82E File Offset: 0x0015DA2E
			public MaterialPropertyPair(Material mat, string prop)
			{
				this.material = mat;
				this.propertyName = prop;
			}

			// Token: 0x04003B55 RID: 15189
			public Material material;

			// Token: 0x04003B56 RID: 15190
			public string propertyName;
		}
	}
}
