using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Features.Localization;
using ProjectExhibitionist.Core;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ProjectExhibitionist.Interactions
{
	// Token: 0x0200090A RID: 2314
	public class InteractablePromptUI : MonoBehaviour
	{
		// Token: 0x17000A80 RID: 2688
		// (get) Token: 0x06003E8B RID: 16011 RVA: 0x00173462 File Offset: 0x00171662
		public static InteractablePromptUI Current
		{
			get
			{
				return InteractablePromptUI._current;
			}
		}

		// Token: 0x06003E8C RID: 16012 RVA: 0x00173469 File Offset: 0x00171669
		public static bool HasVisiblePromptOtherThan(InteractablePromptUI requester)
		{
			return InteractablePromptUI._current != null && InteractablePromptUI._current != requester && InteractablePromptUI._current.IsVisible;
		}

		// Token: 0x17000A81 RID: 2689
		// (get) Token: 0x06003E8D RID: 16013 RVA: 0x00173491 File Offset: 0x00171691
		public bool IsVisible
		{
			get
			{
				return this.promptCanvas != null && this.promptCanvas.gameObject.activeInHierarchy;
			}
		}

		// Token: 0x17000A82 RID: 2690
		// (get) Token: 0x06003E8E RID: 16014 RVA: 0x001734B3 File Offset: 0x001716B3
		public string DisplayedObjectName
		{
			get
			{
				if (!(this.objectNameText != null))
				{
					return string.Empty;
				}
				return this.objectNameText.text;
			}
		}

		// Token: 0x17000A83 RID: 2691
		// (get) Token: 0x06003E8F RID: 16015 RVA: 0x001734D4 File Offset: 0x001716D4
		public string DisplayedAction
		{
			get
			{
				if (!(this.actionText != null))
				{
					return string.Empty;
				}
				return this.actionText.text;
			}
		}

		// Token: 0x17000A84 RID: 2692
		// (get) Token: 0x06003E90 RID: 16016 RVA: 0x001734F5 File Offset: 0x001716F5
		public string DisplayedKey
		{
			get
			{
				if (!(this.keyText != null))
				{
					return string.Empty;
				}
				return this.keyText.text;
			}
		}

		// Token: 0x17000A85 RID: 2693
		// (get) Token: 0x06003E91 RID: 16017 RVA: 0x00173518 File Offset: 0x00171718
		public float CardWidth
		{
			get
			{
				if (!(this._canvasRect != null))
				{
					return 0f;
				}
				return this._canvasRect.rect.width;
			}
		}

		// Token: 0x17000A86 RID: 2694
		// (get) Token: 0x06003E92 RID: 16018 RVA: 0x0017354C File Offset: 0x0017174C
		public float DisplayedObjectFontSize
		{
			get
			{
				if (!(this.objectNameText != null))
				{
					return 0f;
				}
				return this.objectNameText.fontSize;
			}
		}

		// Token: 0x17000A87 RID: 2695
		// (get) Token: 0x06003E93 RID: 16019 RVA: 0x0017356D File Offset: 0x0017176D
		public float DisplayedActionFontSize
		{
			get
			{
				if (!(this.actionText != null))
				{
					return 0f;
				}
				return this.actionText.fontSize;
			}
		}

		// Token: 0x17000A88 RID: 2696
		// (get) Token: 0x06003E94 RID: 16020 RVA: 0x0017358E File Offset: 0x0017178E
		public bool IsCompactActionOnly
		{
			get
			{
				return this._compactActionOnly;
			}
		}

		// Token: 0x06003E95 RID: 16021 RVA: 0x00173596 File Offset: 0x00171796
		private void Awake()
		{
			this.InitializeIfNeeded();
		}

		// Token: 0x06003E96 RID: 16022 RVA: 0x001735A0 File Offset: 0x001717A0
		private void InitializeIfNeeded()
		{
			if (this._initialized)
			{
				return;
			}
			this._initialized = true;
			GameplayHudVisibility.RegisterCanvas(this.promptCanvas);
			this.CleanupGeneratedRuntimeChildren();
			this._mainCamera = Camera.main;
			this._objectBaseFontSize = InteractablePromptUI.ResolveBaseFontSize(this.objectNameText, 18f);
			this._actionBaseFontSize = InteractablePromptUI.ResolveBaseFontSize(this.actionText, 12f);
			this._keyBaseFontSize = Mathf.Max(9f, InteractablePromptUI.ResolveBaseFontSize(this.keyText, 12f));
			InteractablePromptUI.RepairKeycapLayout(this.keyText);
			this.ConfigureRenderingLayer(this._renderingLayerName, this.onTopSortingOrder);
			this.Hide();
		}

		// Token: 0x06003E97 RID: 16023 RVA: 0x00173648 File Offset: 0x00171848
		private void OnDestroy()
		{
			this.ReleaseOwnership();
			foreach (TMP_Text tmp_Text in this._overlayTexts)
			{
				if (tmp_Text != null)
				{
					tmp_Text.OnPreRenderText -= this.RepairSpriteOverlays;
				}
			}
			this._overlayTexts.Clear();
			InteractablePromptUI.DestroyOwnedMaterials(this._ownedImageMaterials.Values);
			InteractablePromptUI.DestroyOwnedMaterials(this._ownedSpriteMaterials.Values);
			InteractablePromptUI.DestroyOwnedMaterials(this._ownedTmpMaterials.Values);
			this._ownedImageMaterials.Clear();
			this._ownedTmpMaterials.Clear();
			if (this._holdProgressSprite != null)
			{
				if (Application.isPlaying)
				{
					Object.Destroy(this._holdProgressSprite);
				}
				else
				{
					Object.DestroyImmediate(this._holdProgressSprite);
				}
				this._holdProgressSprite = null;
			}
		}

		// Token: 0x06003E98 RID: 16024 RVA: 0x0017373C File Offset: 0x0017193C
		public void ConfigureRenderingLayer(string layerName = "UI", int sortingOrder = 100)
		{
			string text = (string.IsNullOrWhiteSpace(layerName) ? "UI" : layerName);
			int num = LayerMask.NameToLayer(text);
			if (num < 0)
			{
				text = "UI";
				num = LayerMask.NameToLayer(text);
			}
			this._renderingLayerName = text;
			this.onTopSortingOrder = sortingOrder;
			if (this.promptCanvas == null)
			{
				return;
			}
			if (num >= 0)
			{
				InteractablePromptUI.SetLayerRecursively(this.promptCanvas.transform, num);
			}
			this.promptCanvas.overrideSorting = true;
			this.promptCanvas.sortingOrder = sortingOrder;
		}

		// Token: 0x06003E99 RID: 16025 RVA: 0x001737BC File Offset: 0x001719BC
		private void OnEnable()
		{
			GameLocalization.LocaleChanged += this.RefreshLocalizedPresentation;
			PlayerActivityGate.Changed -= this.HandleActivityGateChanged;
			PlayerActivityGate.Changed += this.HandleActivityGateChanged;
			PlayerActivityGate.ResourcesChanged -= this.HandleActivityResourcesChanged;
			PlayerActivityGate.ResourcesChanged += this.HandleActivityResourcesChanged;
			this.RefreshVehiclePlacement();
			this.RefreshLocalizedPresentation();
			if (!InteractablePromptUI.CanPresent(this._handoffCandidate))
			{
				this.Hide();
			}
		}

		// Token: 0x06003E9A RID: 16026 RVA: 0x00173840 File Offset: 0x00171A40
		private void OnDisable()
		{
			GameLocalization.LocaleChanged -= this.RefreshLocalizedPresentation;
			PlayerActivityGate.Changed -= this.HandleActivityGateChanged;
			PlayerActivityGate.ResourcesChanged -= this.HandleActivityResourcesChanged;
			this._handoffCandidate = null;
			this.ReleaseOwnership();
		}

		// Token: 0x06003E9B RID: 16027 RVA: 0x00173890 File Offset: 0x00171A90
		private static float ResolveBaseFontSize(TextMeshProUGUI text, float fallback)
		{
			if (text == null)
			{
				return fallback;
			}
			float num = ((text.fontSize > 0.1f) ? text.fontSize : fallback);
			text.enableAutoSizing = false;
			text.fontSize = num;
			return num;
		}

		// Token: 0x06003E9C RID: 16028 RVA: 0x001738CE File Offset: 0x00171ACE
		private void CleanupGeneratedRuntimeChildren()
		{
			if (this.promptCanvas == null)
			{
				return;
			}
			InteractablePromptUI.CleanupGeneratedRuntimeChildren(this.promptCanvas.transform);
		}

		// Token: 0x06003E9D RID: 16029 RVA: 0x001738F0 File Offset: 0x00171AF0
		private static void CleanupGeneratedRuntimeChildren(Transform root)
		{
			for (int i = root.childCount - 1; i >= 0; i--)
			{
				Transform child = root.GetChild(i);
				if (!(child == null))
				{
					bool flag = child.name.EndsWith("_Secondary", StringComparison.Ordinal);
					bool flag2 = child.name == "HoldProgress";
					if (flag || flag2)
					{
						child.gameObject.SetActive(false);
						Object.Destroy(child.gameObject);
					}
					else
					{
						InteractablePromptUI.CleanupGeneratedRuntimeChildren(child);
					}
				}
			}
		}

		// Token: 0x06003E9E RID: 16030 RVA: 0x00173968 File Offset: 0x00171B68
		private void LateUpdate()
		{
			if (this._handoffCandidate != null && !InteractablePromptUI.CanPresent(this._handoffCandidate))
			{
				this.Hide();
				return;
			}
			this.RefreshInputPresentation();
			if (!this.lookAtCamera || this.promptCanvas == null || !this.promptCanvas.gameObject.activeSelf)
			{
				return;
			}
			if (this._mainCamera == null)
			{
				this._mainCamera = Camera.main;
			}
			if (this._mainCamera == null)
			{
				return;
			}
			this.promptCanvas.transform.LookAt(this._mainCamera.transform);
			this.promptCanvas.transform.Rotate(0f, 180f, 0f);
		}

		// Token: 0x06003E9F RID: 16031 RVA: 0x00173A24 File Offset: 0x00171C24
		public void Show(InteractionConfigSO config)
		{
			this.ShowInternal(config, false, default(Color), null, null);
		}

		// Token: 0x06003EA0 RID: 16032 RVA: 0x00173A44 File Offset: 0x00171C44
		public void Show(InteractionConfigSO config, Color objectNameColor)
		{
			this.ShowInternal(config, true, objectNameColor, null, null);
		}

		// Token: 0x06003EA1 RID: 16033 RVA: 0x00173A54 File Offset: 0x00171C54
		public void Show(InteractionConfigSO config, Func<string> objectNameResolver)
		{
			this.ShowInternal(config, false, default(Color), objectNameResolver, null);
		}

		// Token: 0x06003EA2 RID: 16034 RVA: 0x00173A74 File Offset: 0x00171C74
		public void ShowForHandoff(InteractionConfigSO config, IInteractionHandoffCandidate candidate)
		{
			this.ShowInternal(config, false, default(Color), null, candidate);
		}

		// Token: 0x06003EA3 RID: 16035 RVA: 0x00173A94 File Offset: 0x00171C94
		private void ShowInternal(InteractionConfigSO config, bool overrideObjectNameColor, Color objectNameColor, Func<string> objectNameResolver = null, IInteractionHandoffCandidate handoffCandidate = null)
		{
			this._handoffCandidate = null;
			if (config == null || !InteractablePromptUI.CanPresent(handoffCandidate))
			{
				this.Hide();
				return;
			}
			this.InitializeIfNeeded();
			this._handoffCandidate = handoffCandidate;
			if (InteractablePromptUI._current != null && InteractablePromptUI._current != this)
			{
				InteractablePromptUI._current.Hide();
			}
			InteractablePromptUI._current = this;
			if (this._sourceObjectName != config.objectName || this._objectNameResolver != objectNameResolver || objectNameResolver != null || this.objectNameText.GetComponent<LocalizedGameText>() == null)
			{
				this._sourceObjectName = config.objectName;
				this._objectNameResolver = objectNameResolver;
				if (objectNameResolver != null)
				{
					LocalizedGameText.BindComputed(this.objectNameText, objectNameResolver);
				}
				else
				{
					InteractionLocalization.BindObject(this.objectNameText, this._sourceObjectName);
				}
			}
			if (this._compactActionOnly)
			{
				this.objectNameText.gameObject.SetActive(false);
			}
			Color color = (overrideObjectNameColor ? objectNameColor : config.objectNameColor);
			this.objectNameText.color = ((color.a > 0f) ? color : Color.white);
			if (this._sourceAction != config.mainActionName || this.actionText.GetComponent<LocalizedGameText>() == null)
			{
				this._sourceAction = config.mainActionName;
				InteractionLocalization.BindAction(this.actionText, this._sourceAction);
			}
			bool flag = !string.IsNullOrEmpty(config.mainKeyDisplayLabel);
			GameObject gameObject = this.KeyChipGameObject();
			if (gameObject != null)
			{
				gameObject.SetActive(flag);
			}
			if (flag && (this._sourceKey != config.mainKeyDisplayLabel || this._mainKeyConfig != config || this.keyText.GetComponent<LocalizedGameText>() == null))
			{
				this._mainKeyConfig = config;
				this._sourceKey = config.mainKeyDisplayLabel;
				ControllerKeyHint.Bind(this.keyText, this._sourceKey, config, true);
			}
			this.FitMainCard(flag, gameObject);
			this.UpdateSecondaryRow(config);
			this.promptCanvas.gameObject.SetActive(true);
			this.ConfigureRenderingLayer(this._renderingLayerName, this.onTopSortingOrder);
			if (this.alwaysOnTop)
			{
				this.ApplyAlwaysOnTop();
			}
		}

		// Token: 0x06003EA4 RID: 16036 RVA: 0x00173CB7 File Offset: 0x00171EB7
		private void HandleActivityGateChanged(PlayerActivity _)
		{
			if (!InteractablePromptUI.CanPresent(this._handoffCandidate))
			{
				this.Hide();
			}
		}

		// Token: 0x06003EA5 RID: 16037 RVA: 0x00173CCC File Offset: 0x00171ECC
		private static bool CanPresent(IInteractionHandoffCandidate candidate)
		{
			if (PlayerActivityGate.CanStartWorldInteraction)
			{
				return true;
			}
			if (candidate != null)
			{
				Object @object = candidate as Object;
				if (@object == null || !(@object == null))
				{
					Behaviour behaviour = candidate as Behaviour;
					if (behaviour == null || behaviour.isActiveAndEnabled)
					{
						return candidate.CanPrepareInteraction && candidate.IsCandidateEligible && InteractionCandidateRegistry.IsPromptWinner(candidate);
					}
				}
			}
			return false;
		}

		// Token: 0x06003EA6 RID: 16038 RVA: 0x00173D24 File Offset: 0x00171F24
		private void RefreshLocalizedPresentation()
		{
			if (!this.IsVisible)
			{
				return;
			}
			LocalizedGameText[] componentsInChildren = this.promptCanvas.GetComponentsInChildren<LocalizedGameText>(true);
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				componentsInChildren[i].Refresh();
			}
			GameObject gameObject = this.KeyChipGameObject();
			this.FitMainCard(gameObject != null && gameObject.activeSelf, gameObject);
			if (this._secondaryActionText != null)
			{
				InteractablePromptUI.FitTextWithinWidth(this._secondaryActionText, this._actionBaseFontSize, this._lastActionTextWidth);
			}
			if (this.alwaysOnTop)
			{
				this.ApplyAlwaysOnTop();
			}
		}

		// Token: 0x06003EA7 RID: 16039 RVA: 0x00173DB0 File Offset: 0x00171FB0
		private void RefreshInputPresentation()
		{
			if (!this.IsVisible)
			{
				return;
			}
			if (this._presentedController == ControllerInput.IsActive && this._presentedGlyphVersion == ControllerGlyphs.Version)
			{
				return;
			}
			this._presentedController = ControllerInput.IsActive;
			this._presentedGlyphVersion = ControllerGlyphs.Version;
			this.RefreshLocalizedPresentation();
		}

		// Token: 0x06003EA8 RID: 16040 RVA: 0x00173E00 File Offset: 0x00172000
		private static void RepairKeycapLayout(TextMeshProUGUI text)
		{
			if (!(text == null))
			{
				RectTransform rectTransform = text.transform.parent as RectTransform;
				if (rectTransform != null)
				{
					RectTransform rectTransform2 = text.rectTransform;
					Vector2 vector = new Vector2(0.5f, 0.5f);
					rectTransform2.pivot = vector;
					rectTransform2.anchorMin = (rectTransform2.anchorMax = vector);
					rectTransform2.anchoredPosition = Vector2.zero;
					rectTransform2.sizeDelta = new Vector2(Mathf.Max(10f, rectTransform.rect.width - 2f), Mathf.Max(12f, rectTransform.rect.height));
					text.alignment = TextAlignmentOptions.Center;
					text.margin = Vector4.zero;
					text.enableWordWrapping = false;
					text.overflowMode = TextOverflowModes.Overflow;
					return;
				}
			}
		}

		// Token: 0x06003EA9 RID: 16041 RVA: 0x00173CB7 File Offset: 0x00171EB7
		private void HandleActivityResourcesChanged(PlayerContextResource _)
		{
			if (!InteractablePromptUI.CanPresent(this._handoffCandidate))
			{
				this.Hide();
			}
		}

		// Token: 0x06003EAA RID: 16042 RVA: 0x00173EC8 File Offset: 0x001720C8
		private GameObject KeyChipGameObject()
		{
			if (this.keyText == null)
			{
				return null;
			}
			Transform parent = this.keyText.transform.parent;
			if (parent != null && this.actionText != null && parent != this.actionText.transform.parent)
			{
				return parent.gameObject;
			}
			return this.keyText.gameObject;
		}

		// Token: 0x06003EAB RID: 16043 RVA: 0x00173F38 File Offset: 0x00172138
		private void UpdateSecondaryRow(InteractionConfigSO config)
		{
			if (!config.HasSecondaryAction)
			{
				if (this._secondaryRow != null)
				{
					this._secondaryRow.gameObject.SetActive(false);
					this.ApplyCanvasHeight(false);
				}
				return;
			}
			this.EnsureSecondaryRow();
			if (this._secondaryRow == null)
			{
				return;
			}
			if (this._sourceSecondaryKey != config.secondaryKeyDisplayLabel || this._secondaryKeyConfig != config)
			{
				this._secondaryKeyConfig = config;
				this._sourceSecondaryKey = config.secondaryKeyDisplayLabel;
				ControllerKeyHint.Bind(this._secondaryKeyText, this._sourceSecondaryKey, config, true);
			}
			if (this._sourceSecondaryAction != config.secondaryActionName)
			{
				this._sourceSecondaryAction = config.secondaryActionName;
				InteractionLocalization.BindAction(this._secondaryActionText, this._sourceSecondaryAction);
			}
			this.MatchSecondaryKeyToPrimary();
			InteractablePromptUI.FitTextWithinWidth(this._secondaryActionText, this._actionBaseFontSize, this._lastActionTextWidth);
			this._secondaryRow.gameObject.SetActive(true);
			this.ApplyCanvasHeight(true);
		}

		// Token: 0x06003EAC RID: 16044 RVA: 0x00174038 File Offset: 0x00172238
		private void MatchSecondaryKeyToPrimary()
		{
			if (this.keyText == null || this._secondaryKeyText == null)
			{
				return;
			}
			this._secondaryKeyText.enableAutoSizing = this.keyText.enableAutoSizing;
			this._secondaryKeyText.fontSize = this.keyText.fontSize;
			this._secondaryKeyText.fontSizeMin = this.keyText.fontSizeMin;
			this._secondaryKeyText.fontSizeMax = this.keyText.fontSizeMax;
			this._secondaryKeyText.fontStyle = this.keyText.fontStyle;
			this._secondaryKeyText.fontWeight = this.keyText.fontWeight;
			this._secondaryKeyText.alignment = this.keyText.alignment;
			this._secondaryKeyText.characterSpacing = this.keyText.characterSpacing;
			this._secondaryKeyText.rectTransform.sizeDelta = this.keyText.rectTransform.sizeDelta;
			RectTransform rectTransform = this.keyText.transform.parent as RectTransform;
			RectTransform rectTransform2 = this._secondaryKeyText.transform.parent as RectTransform;
			if (rectTransform != null && rectTransform2 != null)
			{
				rectTransform2.sizeDelta = rectTransform.sizeDelta;
				rectTransform2.localScale = rectTransform.localScale;
			}
			this._secondaryKeyText.ForceMeshUpdate(false, false);
		}

		// Token: 0x06003EAD RID: 16045 RVA: 0x00174198 File Offset: 0x00172398
		private void EnsureSecondaryRow()
		{
			if (this._secondaryRow != null)
			{
				return;
			}
			if (this.keyText == null || this.actionText == null)
			{
				return;
			}
			Transform parent = this.actionText.transform.parent;
			if (parent == null || !(parent is RectTransform))
			{
				return;
			}
			TextMeshProUGUI[] componentsInChildren = parent.GetComponentsInChildren<TextMeshProUGUI>(true);
			int num = Array.IndexOf<TextMeshProUGUI>(componentsInChildren, this.keyText);
			int num2 = Array.IndexOf<TextMeshProUGUI>(componentsInChildren, this.actionText);
			if (num < 0 || num2 < 0)
			{
				return;
			}
			GameObject gameObject = Object.Instantiate<GameObject>(parent.gameObject, parent.parent);
			gameObject.name = parent.name + "_Secondary";
			this._secondaryRow = (RectTransform)gameObject.transform;
			this._secondaryRow.SetSiblingIndex(parent.GetSiblingIndex() + 1);
			TextMeshProUGUI[] componentsInChildren2 = this._secondaryRow.GetComponentsInChildren<TextMeshProUGUI>(true);
			if (componentsInChildren2.Length != componentsInChildren.Length)
			{
				Object.Destroy(gameObject);
				this._secondaryRow = null;
				return;
			}
			this._secondaryKeyText = componentsInChildren2[num];
			this._secondaryActionText = componentsInChildren2[num2];
			InteractablePromptUI.RepairKeycapLayout(this._secondaryKeyText);
			this._secondaryRowHeight = ((RectTransform)parent).sizeDelta.y;
		}

		// Token: 0x06003EAE RID: 16046 RVA: 0x001742CC File Offset: 0x001724CC
		private void ApplyCanvasHeight(bool secondaryVisible)
		{
			if (this.promptCanvas == null)
			{
				return;
			}
			if (this._canvasRect == null)
			{
				this._canvasRect = this.promptCanvas.GetComponent<RectTransform>();
			}
			if (this._canvasRect == null)
			{
				return;
			}
			if (this._baseCanvasHeight < 0f)
			{
				this._baseCanvasHeight = this._canvasRect.sizeDelta.y;
			}
			float num = (secondaryVisible ? (this._baseCanvasHeight + this._secondaryRowHeight) : this._baseCanvasHeight);
			this._canvasRect.sizeDelta = new Vector2(this._canvasRect.sizeDelta.x, num);
		}

		// Token: 0x06003EAF RID: 16047 RVA: 0x00174374 File Offset: 0x00172574
		private void FitMainCard(bool hasKey, GameObject keyChip)
		{
			if (this.promptCanvas == null || this.objectNameText == null || this.actionText == null)
			{
				return;
			}
			if (this._canvasRect == null)
			{
				this._canvasRect = this.promptCanvas.GetComponent<RectTransform>();
			}
			if (this._canvasRect == null)
			{
				return;
			}
			if (this._baseCanvasWidth < 0f)
			{
				this._baseCanvasWidth = this._canvasRect.sizeDelta.x;
			}
			bool flag = !this._compactActionOnly && InteractablePromptUI.PrepareForMeasure(this.objectNameText, this._objectBaseFontSize);
			bool flag2 = InteractablePromptUI.PrepareForMeasure(this.actionText, this._actionBaseFontSize);
			if (hasKey)
			{
				InteractablePromptUI.RepairKeycapLayout(this.keyText);
				InteractablePromptUI.PrepareForMeasure(this.keyText, this._keyBaseFontSize);
				if (keyChip != null && keyChip.transform != this.keyText.transform)
				{
					RectTransform rectTransform = keyChip.transform as RectTransform;
					if (rectTransform != null)
					{
						float num = Mathf.Clamp(this.keyText.GetPreferredValues().x + 4f, 14f, ControllerInput.IsActive ? 56f : 42f);
						rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, num);
						rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, ControllerInput.IsActive ? 18f : 14f);
					}
				}
			}
			float num2 = ((!this._compactActionOnly && flag) ? (this.objectNameText.GetPreferredValues().x + 8f) : 0f);
			float num3 = (flag2 ? this.actionText.GetPreferredValues().x : 0f);
			float num4 = 0f;
			if (hasKey && keyChip != null)
			{
				RectTransform component = keyChip.GetComponent<RectTransform>();
				num4 = ((component != null) ? Mathf.Max(10f, component.rect.width) : 10f);
			}
			float num5 = (this._compactActionOnly ? 72f : 112f);
			float num6 = Mathf.Max(this._baseCanvasWidth, Mathf.Max(num2, num3 + num4 + 12f));
			float num7 = Mathf.Min(num5, num6);
			this._canvasRect.sizeDelta = new Vector2(num7, this._canvasRect.sizeDelta.y);
			float num8 = Mathf.Max(this._compactActionOnly ? 28f : 48f, num7 - 8f);
			this._lastActionTextWidth = Mathf.Max(this._compactActionOnly ? 28f : 46f, num7 - num4 - 12f);
			InteractablePromptUI.FitTextWithinWidth(this.objectNameText, this._objectBaseFontSize, num8);
			InteractablePromptUI.FitTextWithinWidth(this.actionText, this._actionBaseFontSize, this._lastActionTextWidth);
			if (hasKey)
			{
				InteractablePromptUI.FitTextWithinWidth(this.keyText, this._keyBaseFontSize, Mathf.Max(8f, num4 - 2f));
			}
			LayoutRebuilder.ForceRebuildLayoutImmediate(this._canvasRect);
		}

		// Token: 0x06003EB0 RID: 16048 RVA: 0x0017466C File Offset: 0x0017286C
		public void ConfigureCompactActionOnly(float worldScale = 0.0038f)
		{
			if (this._compactActionOnly)
			{
				return;
			}
			this._compactActionOnly = true;
			if (this.objectNameText != null)
			{
				this.objectNameText.gameObject.SetActive(false);
			}
			if (this.promptCanvas != null)
			{
				this._canvasRect = this.promptCanvas.GetComponent<RectTransform>();
				if (this._canvasRect != null)
				{
					this._canvasRect.sizeDelta = new Vector2(72f, 28f);
				}
			}
			if (this.actionText != null)
			{
				RectTransform rectTransform = this.actionText.transform.parent as RectTransform;
				if (rectTransform != null)
				{
					rectTransform.sizeDelta = new Vector2(68f, 18f);
				}
			}
			GameObject gameObject = this.KeyChipGameObject();
			if (gameObject != null)
			{
				RectTransform rectTransform2 = gameObject.transform as RectTransform;
				if (rectTransform2 != null)
				{
					rectTransform2.sizeDelta = new Vector2(28f, 10f);
				}
			}
			if (this.keyText != null)
			{
				this.keyText.rectTransform.sizeDelta = new Vector2(26f, 10f);
			}
			InteractablePromptUI.SetUniformWorldScale(base.transform, Mathf.Max(0.0001f, worldScale));
			this._baseCanvasWidth = -1f;
			this._baseCanvasHeight = -1f;
		}

		// Token: 0x06003EB1 RID: 16049 RVA: 0x001747B8 File Offset: 0x001729B8
		private static bool PrepareForMeasure(TextMeshProUGUI text, float baseFontSize)
		{
			if (text == null || text.font == null || text.fontSharedMaterial == null)
			{
				return false;
			}
			text.enableAutoSizing = false;
			text.fontSize = baseFontSize;
			text.enableWordWrapping = false;
			text.overflowMode = TextOverflowModes.Overflow;
			text.ForceMeshUpdate(false, false);
			return true;
		}

		// Token: 0x06003EB2 RID: 16050 RVA: 0x00174814 File Offset: 0x00172A14
		private static void FitTextWithinWidth(TextMeshProUGUI text, float baseFontSize, float maximumWidth)
		{
			if (!InteractablePromptUI.PrepareForMeasure(text, baseFontSize))
			{
				return;
			}
			float num = text.GetPreferredValues().x + 2f;
			float num2 = Mathf.Min(num, maximumWidth);
			if (num > maximumWidth)
			{
				text.enableAutoSizing = true;
				text.fontSizeMax = baseFontSize;
				text.fontSizeMin = Mathf.Min(baseFontSize, Mathf.Max(3f, baseFontSize * 0.56f));
				if (GameLocalization.CurrentCode != "en")
				{
					float num3 = baseFontSize * maximumWidth / num * 0.96f;
					text.fontSizeMin = Mathf.Min(text.fontSizeMin, Mathf.Max(3f, num3));
				}
			}
			RectTransform rectTransform = text.rectTransform;
			rectTransform.sizeDelta = new Vector2(num2, rectTransform.sizeDelta.y);
			text.ForceMeshUpdate(false, false);
			ContentSizeFitter component = text.GetComponent<ContentSizeFitter>();
			if (component != null)
			{
				component.SetLayoutHorizontal();
			}
			LayoutRebuilder.ForceRebuildLayoutImmediate(rectTransform);
		}

		// Token: 0x06003EB3 RID: 16051 RVA: 0x001748F1 File Offset: 0x00172AF1
		public bool MatchVehiclePlacement(Transform visualRoot, float worldScale = 0.0055f)
		{
			this._vehiclePlacementRoot = visualRoot;
			this._vehiclePlacementWorldScale = worldScale;
			this._placeAboveVisual = false;
			this._placementVerticalGap = 0f;
			return this.RefreshVehiclePlacement();
		}

		// Token: 0x06003EB4 RID: 16052 RVA: 0x00174919 File Offset: 0x00172B19
		public bool MatchGymPlacement(Transform visualRoot, float worldScale = 0.008525f, float verticalGap = 0.3f)
		{
			this._vehiclePlacementRoot = visualRoot;
			this._vehiclePlacementWorldScale = worldScale;
			this._placeAboveVisual = true;
			this._placementVerticalGap = Mathf.Max(0f, verticalGap);
			return this.RefreshVehiclePlacement();
		}

		// Token: 0x06003EB5 RID: 16053 RVA: 0x00174948 File Offset: 0x00172B48
		private bool RefreshVehiclePlacement()
		{
			Transform vehiclePlacementRoot = this._vehiclePlacementRoot;
			if (vehiclePlacementRoot == null)
			{
				return false;
			}
			Bounds bounds = default(Bounds);
			bool flag = false;
			foreach (Renderer renderer in vehiclePlacementRoot.GetComponentsInChildren<Renderer>(true))
			{
				if (!(renderer == null) && !renderer.transform.IsChildOf(base.transform) && (renderer is MeshRenderer || renderer is SkinnedMeshRenderer))
				{
					if (!flag)
					{
						bounds = renderer.bounds;
						flag = true;
					}
					else
					{
						bounds.Encapsulate(renderer.bounds);
					}
				}
			}
			if (!flag)
			{
				return false;
			}
			base.transform.position = (this._placeAboveVisual ? new Vector3(bounds.center.x, bounds.max.y + this._placementVerticalGap, bounds.center.z) : bounds.center);
			InteractablePromptUI.SetUniformWorldScale(base.transform, this._vehiclePlacementWorldScale);
			return true;
		}

		// Token: 0x06003EB6 RID: 16054 RVA: 0x00174A44 File Offset: 0x00172C44
		private static void SetUniformWorldScale(Transform target, float worldScale)
		{
			if (target == null)
			{
				return;
			}
			Vector3 vector = ((target.parent != null) ? target.parent.lossyScale : Vector3.one);
			target.localScale = new Vector3(InteractablePromptUI.DivideBySignedScale(worldScale, vector.x, 0.0001f), InteractablePromptUI.DivideBySignedScale(worldScale, vector.y, 0.0001f), InteractablePromptUI.DivideBySignedScale(worldScale, vector.z, 0.0001f));
		}

		// Token: 0x06003EB7 RID: 16055 RVA: 0x00174ABC File Offset: 0x00172CBC
		private static float DivideBySignedScale(float worldScale, float parentScale, float minimumScale)
		{
			float num = ((Mathf.Abs(parentScale) >= minimumScale) ? parentScale : ((parentScale < 0f) ? (-minimumScale) : minimumScale));
			return worldScale / num;
		}

		// Token: 0x06003EB8 RID: 16056 RVA: 0x00174AE6 File Offset: 0x00172CE6
		public void Hide()
		{
			this._handoffCandidate = null;
			this.SetHoldProgress(0f, false);
			if (this.promptCanvas != null)
			{
				this.promptCanvas.gameObject.SetActive(false);
			}
			this.ReleaseOwnership();
		}

		// Token: 0x06003EB9 RID: 16057 RVA: 0x00174B20 File Offset: 0x00172D20
		private void ReleaseOwnership()
		{
			if (InteractablePromptUI._current == this)
			{
				InteractablePromptUI._current = null;
			}
		}

		// Token: 0x06003EBA RID: 16058 RVA: 0x00174B38 File Offset: 0x00172D38
		public void SetHoldProgress(float normalizedProgress, bool visible)
		{
			if (visible)
			{
				this.EnsureHoldProgressBar();
			}
			if (this._holdProgressRoot == null)
			{
				return;
			}
			this._holdProgressRoot.SetActive(visible);
			if (this._holdProgressFill != null)
			{
				this._holdProgressFill.fillAmount = Mathf.Clamp01(normalizedProgress);
			}
		}

		// Token: 0x06003EBB RID: 16059 RVA: 0x00174B88 File Offset: 0x00172D88
		private void EnsureHoldProgressBar()
		{
			if (this._holdProgressRoot != null || this.promptCanvas == null)
			{
				return;
			}
			GameObject gameObject = new GameObject("HoldProgress", new Type[]
			{
				typeof(RectTransform),
				typeof(CanvasRenderer),
				typeof(Image)
			});
			RectTransform component = gameObject.GetComponent<RectTransform>();
			component.SetParent(this.promptCanvas.transform, false);
			component.anchorMin = new Vector2(0.16f, 0.055f);
			component.anchorMax = new Vector2(0.84f, 0.105f);
			component.offsetMin = Vector2.zero;
			component.offsetMax = Vector2.zero;
			Image component2 = gameObject.GetComponent<Image>();
			component2.color = new Color(0.055f, 0.035f, 0.07f, 0.88f);
			component2.raycastTarget = false;
			GameObject gameObject2 = new GameObject("Fill", new Type[]
			{
				typeof(RectTransform),
				typeof(CanvasRenderer),
				typeof(Image)
			});
			RectTransform component3 = gameObject2.GetComponent<RectTransform>();
			component3.SetParent(component, false);
			component3.anchorMin = new Vector2(0.04f, 0.18f);
			component3.anchorMax = new Vector2(0.96f, 0.82f);
			component3.offsetMin = Vector2.zero;
			component3.offsetMax = Vector2.zero;
			this._holdProgressFill = gameObject2.GetComponent<Image>();
			Texture2D whiteTexture = Texture2D.whiteTexture;
			this._holdProgressSprite = Sprite.Create(whiteTexture, new Rect(0f, 0f, (float)whiteTexture.width, (float)whiteTexture.height), new Vector2(0.5f, 0.5f));
			this._holdProgressSprite.name = "Interaction hold fill";
			this._holdProgressSprite.hideFlags = HideFlags.HideAndDontSave;
			this._holdProgressFill.sprite = this._holdProgressSprite;
			this._holdProgressFill.raycastTarget = false;
			this._holdProgressFill.color = new Color(1f, 0.3f, 0.64f, 1f);
			this._holdProgressFill.type = Image.Type.Filled;
			this._holdProgressFill.fillMethod = Image.FillMethod.Horizontal;
			this._holdProgressFill.fillOrigin = 0;
			this._holdProgressFill.fillAmount = 0f;
			if (this.alwaysOnTop)
			{
				this.ApplyAlwaysOnTopMaterial(component2);
				this.ApplyAlwaysOnTopMaterial(this._holdProgressFill);
			}
			component.SetAsLastSibling();
			this._holdProgressRoot = gameObject;
		}

		// Token: 0x06003EBC RID: 16060 RVA: 0x00174E00 File Offset: 0x00173000
		private void ApplyAlwaysOnTopMaterial(Image image)
		{
			if (image == null)
			{
				return;
			}
			Material material = image.material;
			Material material2;
			if (!this._ownedImageMaterials.TryGetValue(image, out material2) || material2 == null)
			{
				Shader shader = ((material != null) ? material.shader : Shader.Find("UI/Default"));
				if (shader == null)
				{
					return;
				}
				material2 = ((material != null) ? new Material(material) : new Material(shader));
				InteractablePromptUI.PrepareOwnedMaterial(material2, image.name + " Prompt Overlay");
				this._ownedImageMaterials[image] = material2;
			}
			else if (material != null && material != material2)
			{
				material2.shader = material.shader;
				material2.CopyPropertiesFromMaterial(material);
			}
			Shader shader2 = ((material != null) ? material.shader : material2.shader);
			Shader shader3 = this.ResolveAlwaysOnTopUiShader();
			if (shader3 != null && shader2 != null && (shader2.name == "UI/Default" || shader2.name == "UI/AlwaysOnTop"))
			{
				material2.shader = shader3;
			}
			InteractablePromptUI.ApplyAlwaysDepthTest(material2);
			image.material = material2;
		}

		// Token: 0x06003EBD RID: 16061 RVA: 0x00174F2C File Offset: 0x0017312C
		private void ApplyAlwaysOnTop()
		{
			this.promptCanvas.overrideSorting = true;
			this.promptCanvas.sortingOrder = this.onTopSortingOrder;
			foreach (Image image in this.promptCanvas.GetComponentsInChildren<Image>(true))
			{
				this.ApplyAlwaysOnTopMaterial(image);
			}
			Shader shader = this.ResolveAlwaysOnTopTmpShader();
			if (shader != null)
			{
				foreach (TextMeshProUGUI textMeshProUGUI in this.promptCanvas.GetComponentsInChildren<TextMeshProUGUI>(true))
				{
					this.ApplyAlwaysOnTopMaterial(textMeshProUGUI, shader);
					if (this._overlayTexts.Add(textMeshProUGUI))
					{
						textMeshProUGUI.OnPreRenderText += this.RepairSpriteOverlays;
					}
					textMeshProUGUI.ForceMeshUpdate(false, false);
				}
			}
			this.RepairSpriteOverlays(null);
		}

		// Token: 0x06003EBE RID: 16062 RVA: 0x00174FEC File Offset: 0x001731EC
		private void RepairSpriteOverlays(TMP_TextInfo textInfo)
		{
			if (this.promptCanvas == null || !this.alwaysOnTop)
			{
				return;
			}
			foreach (TMP_SubMeshUI tmp_SubMeshUI in this.promptCanvas.GetComponentsInChildren<TMP_SubMeshUI>(true))
			{
				if (!(tmp_SubMeshUI.spriteAsset == null) && !(tmp_SubMeshUI.sharedMaterial == null))
				{
					Material sharedMaterial = tmp_SubMeshUI.sharedMaterial;
					Material material;
					if (!this._ownedSpriteMaterials.TryGetValue(tmp_SubMeshUI, out material) || material == null)
					{
						material = new Material(sharedMaterial);
						InteractablePromptUI.PrepareOwnedMaterial(material, "Prompt controller glyph overlay");
						this._ownedSpriteMaterials[tmp_SubMeshUI] = material;
					}
					if (sharedMaterial != material)
					{
						material.CopyPropertiesFromMaterial(sharedMaterial);
					}
					Shader shader = this.ResolveAlwaysOnTopUiShader();
					if (shader != null)
					{
						material.shader = shader;
					}
					material.mainTexture = tmp_SubMeshUI.spriteAsset.spriteSheet;
					InteractablePromptUI.ApplyAlwaysDepthTest(material);
					tmp_SubMeshUI.sharedMaterial = material;
				}
			}
		}

		// Token: 0x06003EBF RID: 16063 RVA: 0x001750E8 File Offset: 0x001732E8
		private void ApplyAlwaysOnTopMaterial(TextMeshProUGUI text, Shader overlayShader)
		{
			if (text == null || overlayShader == null)
			{
				return;
			}
			Material fontSharedMaterial = text.fontSharedMaterial;
			ValueTuple<TextMeshProUGUI, TMP_FontAsset> valueTuple = new ValueTuple<TextMeshProUGUI, TMP_FontAsset>(text, text.font);
			Material material;
			if (!this._ownedTmpMaterials.TryGetValue(valueTuple, out material) || material == null)
			{
				material = ((fontSharedMaterial != null) ? new Material(fontSharedMaterial) : new Material(overlayShader));
				InteractablePromptUI.PrepareOwnedMaterial(material, text.name + " Prompt Text Overlay");
				this._ownedTmpMaterials[valueTuple] = material;
			}
			else if (fontSharedMaterial != null && fontSharedMaterial != material)
			{
				material.shader = fontSharedMaterial.shader;
				material.CopyPropertiesFromMaterial(fontSharedMaterial);
			}
			material.shader = overlayShader;
			if (text.font != null)
			{
				material.mainTexture = text.font.atlasTexture;
			}
			InteractablePromptUI.ApplyAlwaysDepthTest(material);
			text.fontSharedMaterial = material;
			text.UpdateMeshPadding();
		}

		// Token: 0x06003EC0 RID: 16064 RVA: 0x001751D1 File Offset: 0x001733D1
		private Shader ResolveAlwaysOnTopUiShader()
		{
			if (this._alwaysOnTopUiShader == null)
			{
				this._alwaysOnTopUiShader = Shader.Find("UI/AlwaysOnTop");
			}
			return this._alwaysOnTopUiShader;
		}

		// Token: 0x06003EC1 RID: 16065 RVA: 0x001751F7 File Offset: 0x001733F7
		private Shader ResolveAlwaysOnTopTmpShader()
		{
			if (this._alwaysOnTopTmpShader == null)
			{
				this._alwaysOnTopTmpShader = Shader.Find("TextMeshPro/Distance Field Always On Top");
			}
			return this._alwaysOnTopTmpShader;
		}

		// Token: 0x06003EC2 RID: 16066 RVA: 0x00175220 File Offset: 0x00173420
		private static void ApplyAlwaysDepthTest(Material material)
		{
			if (material == null)
			{
				return;
			}
			int num = 8;
			material.SetInt(InteractablePromptUI.GuiZTestModeId, num);
			if (material.HasProperty(InteractablePromptUI.ZTestId))
			{
				material.SetInt(InteractablePromptUI.ZTestId, num);
			}
			material.renderQueue = 4000;
		}

		// Token: 0x06003EC3 RID: 16067 RVA: 0x00175269 File Offset: 0x00173469
		private static void PrepareOwnedMaterial(Material material, string materialName)
		{
			material.name = materialName;
			material.hideFlags = HideFlags.HideAndDontSave;
		}

		// Token: 0x06003EC4 RID: 16068 RVA: 0x0017527C File Offset: 0x0017347C
		private static void SetLayerRecursively(Transform root, int layer)
		{
			if (root == null)
			{
				return;
			}
			root.gameObject.layer = layer;
			for (int i = 0; i < root.childCount; i++)
			{
				InteractablePromptUI.SetLayerRecursively(root.GetChild(i), layer);
			}
		}

		// Token: 0x06003EC5 RID: 16069 RVA: 0x001752C0 File Offset: 0x001734C0
		private static void DestroyOwnedMaterials(IEnumerable<Material> materials)
		{
			foreach (Material material in materials)
			{
				if (!(material == null))
				{
					if (Application.isPlaying)
					{
						Object.Destroy(material);
					}
					else
					{
						Object.DestroyImmediate(material);
					}
				}
			}
		}

		// Token: 0x04003E9A RID: 16026
		private static InteractablePromptUI _current;

		// Token: 0x04003E9B RID: 16027
		private const string DefaultRenderingLayerName = "UI";

		// Token: 0x04003E9C RID: 16028
		private const string StandardUiShaderName = "UI/Default";

		// Token: 0x04003E9D RID: 16029
		private const string AlwaysOnTopUiShaderName = "UI/AlwaysOnTop";

		// Token: 0x04003E9E RID: 16030
		private const string AlwaysOnTopTmpShaderName = "TextMeshPro/Distance Field Always On Top";

		// Token: 0x04003E9F RID: 16031
		private static readonly int GuiZTestModeId = Shader.PropertyToID("unity_GUIZTestMode");

		// Token: 0x04003EA0 RID: 16032
		private static readonly int ZTestId = Shader.PropertyToID("_ZTest");

		// Token: 0x04003EA1 RID: 16033
		public const float VehicleReferenceWorldScale = 0.0055f;

		// Token: 0x04003EA2 RID: 16034
		public const float GymReferenceWorldScale = 0.008525f;

		// Token: 0x04003EA3 RID: 16035
		public const float GymVerticalGap = 0.3f;

		// Token: 0x04003EA4 RID: 16036
		public const float CompactActionWorldScale = 0.0038f;

		// Token: 0x04003EA5 RID: 16037
		[Header("UI References")]
		[SerializeField]
		private Canvas promptCanvas;

		// Token: 0x04003EA6 RID: 16038
		[SerializeField]
		private TextMeshProUGUI objectNameText;

		// Token: 0x04003EA7 RID: 16039
		[SerializeField]
		private TextMeshProUGUI keyText;

		// Token: 0x04003EA8 RID: 16040
		[SerializeField]
		private TextMeshProUGUI actionText;

		// Token: 0x04003EA9 RID: 16041
		[Header("Settings")]
		[SerializeField]
		private bool lookAtCamera = true;

		// Token: 0x04003EAA RID: 16042
		[Tooltip("Render on top of all world geometry (ZTest Always on canvas + TMP materials)")]
		[SerializeField]
		private bool alwaysOnTop = true;

		// Token: 0x04003EAB RID: 16043
		[Tooltip("Sorting order used when alwaysOnTop is enabled")]
		[SerializeField]
		private int onTopSortingOrder = 100;

		// Token: 0x04003EAC RID: 16044
		private Camera _mainCamera;

		// Token: 0x04003EAD RID: 16045
		private bool _presentedController;

		// Token: 0x04003EAE RID: 16046
		private int _presentedGlyphVersion;

		// Token: 0x04003EAF RID: 16047
		private readonly HashSet<TMP_Text> _overlayTexts = new HashSet<TMP_Text>();

		// Token: 0x04003EB0 RID: 16048
		private readonly Dictionary<TMP_SubMeshUI, Material> _ownedSpriteMaterials = new Dictionary<TMP_SubMeshUI, Material>();

		// Token: 0x04003EB1 RID: 16049
		private bool _initialized;

		// Token: 0x04003EB2 RID: 16050
		private RectTransform _secondaryRow;

		// Token: 0x04003EB3 RID: 16051
		private TextMeshProUGUI _secondaryKeyText;

		// Token: 0x04003EB4 RID: 16052
		private TextMeshProUGUI _secondaryActionText;

		// Token: 0x04003EB5 RID: 16053
		private RectTransform _canvasRect;

		// Token: 0x04003EB6 RID: 16054
		private float _baseCanvasHeight = -1f;

		// Token: 0x04003EB7 RID: 16055
		private float _secondaryRowHeight;

		// Token: 0x04003EB8 RID: 16056
		private GameObject _holdProgressRoot;

		// Token: 0x04003EB9 RID: 16057
		private Image _holdProgressFill;

		// Token: 0x04003EBA RID: 16058
		private Sprite _holdProgressSprite;

		// Token: 0x04003EBB RID: 16059
		private float _objectBaseFontSize;

		// Token: 0x04003EBC RID: 16060
		private float _actionBaseFontSize;

		// Token: 0x04003EBD RID: 16061
		private float _keyBaseFontSize;

		// Token: 0x04003EBE RID: 16062
		private float _baseCanvasWidth = -1f;

		// Token: 0x04003EBF RID: 16063
		private float _lastActionTextWidth = 72f;

		// Token: 0x04003EC0 RID: 16064
		private Transform _vehiclePlacementRoot;

		// Token: 0x04003EC1 RID: 16065
		private float _vehiclePlacementWorldScale = 0.0055f;

		// Token: 0x04003EC2 RID: 16066
		private bool _placeAboveVisual;

		// Token: 0x04003EC3 RID: 16067
		private float _placementVerticalGap;

		// Token: 0x04003EC4 RID: 16068
		private string _renderingLayerName = "UI";

		// Token: 0x04003EC5 RID: 16069
		private Shader _alwaysOnTopUiShader;

		// Token: 0x04003EC6 RID: 16070
		private Shader _alwaysOnTopTmpShader;

		// Token: 0x04003EC7 RID: 16071
		private bool _compactActionOnly;

		// Token: 0x04003EC8 RID: 16072
		private InteractionConfigSO _mainKeyConfig;

		// Token: 0x04003EC9 RID: 16073
		private InteractionConfigSO _secondaryKeyConfig;

		// Token: 0x04003ECA RID: 16074
		private string _sourceObjectName;

		// Token: 0x04003ECB RID: 16075
		private string _sourceAction;

		// Token: 0x04003ECC RID: 16076
		private string _sourceSecondaryAction;

		// Token: 0x04003ECD RID: 16077
		private string _sourceKey;

		// Token: 0x04003ECE RID: 16078
		private string _sourceSecondaryKey;

		// Token: 0x04003ECF RID: 16079
		private Func<string> _objectNameResolver;

		// Token: 0x04003ED0 RID: 16080
		private IInteractionHandoffCandidate _handoffCandidate;

		// Token: 0x04003ED1 RID: 16081
		private readonly Dictionary<Image, Material> _ownedImageMaterials = new Dictionary<Image, Material>();

		// Token: 0x04003ED2 RID: 16082
		[TupleElementNames(new string[] { "text", "font" })]
		private readonly Dictionary<ValueTuple<TextMeshProUGUI, TMP_FontAsset>, Material> _ownedTmpMaterials = new Dictionary<ValueTuple<TextMeshProUGUI, TMP_FontAsset>, Material>();
	}
}
