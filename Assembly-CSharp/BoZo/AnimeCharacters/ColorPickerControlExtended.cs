using System;
using System.Collections;
using DA_Assets.CR;
using Features.Localization;
using ProjectExhibitionist.CharacterCustomization.Core.Runtime;
using ProjectExhibitionist.Core;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Bozo.AnimeCharacters
{
	// Token: 0x0200088E RID: 2190
	[DefaultExecutionOrder(-500)]
	public class ColorPickerControlExtended : MonoBehaviour
	{
		// Token: 0x17000A12 RID: 2578
		// (get) Token: 0x06003ABE RID: 15038 RVA: 0x0015CA6C File Offset: 0x0015AC6C
		public Color OutputColor
		{
			get
			{
				return this.currentColor;
			}
		}

		// Token: 0x06003ABF RID: 15039 RVA: 0x0015CA74 File Offset: 0x0015AC74
		private void Awake()
		{
			if (this.closeButton != null)
			{
				this.closeButton.onClick.AddListener(new UnityAction(this.CloseColorPicker));
			}
			if (this.applyButton != null)
			{
				this.applyButton.onClick.AddListener(new UnityAction(this.CloseColorPicker));
			}
			if (this.resetButton != null)
			{
				this.resetButton.onClick.AddListener(new UnityAction(this.ResetColor));
			}
			if (this.colorPickerPanel != null)
			{
				this.colorPickerPanel.SetActive(false);
			}
			this.currentHue = 0f;
			this.currentSat = 1f;
			this.currentVal = 1f;
			this.currentColor = Color.HSVToRGB(this.currentHue, this.currentSat, this.currentVal);
		}

		// Token: 0x06003AC0 RID: 15040 RVA: 0x0015CB58 File Offset: 0x0015AD58
		private void Start()
		{
			if (this.hueImage != null && this.hueImage.GetComponent<RectTransform>() != null)
			{
				RectTransform component = this.hueImage.GetComponent<RectTransform>();
				component.sizeDelta = new Vector2(component.sizeDelta.x, Mathf.Max(20f, component.sizeDelta.y));
			}
			this.CreateHueImage();
			this.CreateSVImage();
			this.CreateOutputImage();
			if (this.hueSlider != null)
			{
				this.hueSlider.value = this.currentHue;
				this.hueSlider.onValueChanged.AddListener(new UnityAction<float>(this.SetHue));
			}
			this.SetupEventTriggers();
			this.PolishPickerVisuals();
			this.EnsureColorUtilities();
		}

		// Token: 0x06003AC1 RID: 15041 RVA: 0x0015CC1C File Offset: 0x0015AE1C
		private void Update()
		{
			if (!this.eyedropperArmed)
			{
				return;
			}
			if (EscapeInputCoordinator.TryConsume(this))
			{
				this.SetEyedropperArmed(false);
				return;
			}
			if (!Input.GetMouseButtonDown(0))
			{
				return;
			}
			Vector2 vector = Input.mousePosition;
			RectTransform rectTransform = ((this.colorPickerPanel != null) ? (this.colorPickerPanel.transform as RectTransform) : null);
			if (rectTransform != null && RectTransformUtility.RectangleContainsScreenPoint(rectTransform, vector, null))
			{
				return;
			}
			if (this.utilityBar != null && RectTransformUtility.RectangleContainsScreenPoint(this.utilityBar, vector, null))
			{
				return;
			}
			this.SetEyedropperArmed(false);
			base.StartCoroutine(this.SampleScreenPixel(vector));
		}

		// Token: 0x06003AC2 RID: 15042 RVA: 0x0015CCC0 File Offset: 0x0015AEC0
		private void SetupEventTriggers()
		{
			if (this.satValImage != null)
			{
				Object component = this.satValImage.GetComponent<RectTransform>();
				SVImageControl component2 = this.satValImage.GetComponent<SVImageControl>();
				if (component != null && component2 == null)
				{
					EventTrigger eventTrigger = this.satValImage.gameObject.AddComponent<EventTrigger>();
					EventTrigger.Entry entry = new EventTrigger.Entry();
					entry.eventID = EventTriggerType.PointerDown;
					entry.callback.AddListener(delegate(BaseEventData data)
					{
						this.OnSVPointerDown((PointerEventData)data);
					});
					eventTrigger.triggers.Add(entry);
					EventTrigger.Entry entry2 = new EventTrigger.Entry();
					entry2.eventID = EventTriggerType.Drag;
					entry2.callback.AddListener(delegate(BaseEventData data)
					{
						this.OnSVDrag((PointerEventData)data);
					});
					eventTrigger.triggers.Add(entry2);
				}
			}
			if (this.hueImage != null && this.hueImage.GetComponent<RectTransform>() != null)
			{
				EventTrigger eventTrigger2 = this.hueImage.gameObject.AddComponent<EventTrigger>();
				EventTrigger.Entry entry3 = new EventTrigger.Entry();
				entry3.eventID = EventTriggerType.PointerDown;
				entry3.callback.AddListener(delegate(BaseEventData data)
				{
					this.OnHuePointerDown((PointerEventData)data);
				});
				eventTrigger2.triggers.Add(entry3);
				EventTrigger.Entry entry4 = new EventTrigger.Entry();
				entry4.eventID = EventTriggerType.Drag;
				entry4.callback.AddListener(delegate(BaseEventData data)
				{
					this.OnHuePointerDown((PointerEventData)data);
				});
				eventTrigger2.triggers.Add(entry4);
			}
		}

		// Token: 0x06003AC3 RID: 15043 RVA: 0x0015CE0C File Offset: 0x0015B00C
		private void OnHuePointerDown(PointerEventData eventData)
		{
			if (this.hueImage == null)
			{
				return;
			}
			Vector2 vector;
			RectTransformUtility.ScreenPointToLocalPointInRectangle(this.hueImage.rectTransform, eventData.position, eventData.pressEventCamera, out vector);
			Rect rect = this.hueImage.rectTransform.rect;
			float num = Mathf.Clamp01((vector.x - rect.xMin) / rect.width);
			this.currentHue = num;
			if (this.hueSlider != null)
			{
				this.hueSlider.SetValueWithoutNotify(this.currentHue);
			}
			this.UpdateSVImage();
			this.UpdateHueSelectorPosition();
			this.UpdateOutputImage();
			this.RefreshExternalControls();
			Debug.Log(string.Format("Hue aggiornato: {0}", this.currentHue));
		}

		// Token: 0x06003AC4 RID: 15044 RVA: 0x0015CECC File Offset: 0x0015B0CC
		public void ShowColorPicker(Material material, string propertyName, Color initialColor)
		{
			if (material == null || string.IsNullOrEmpty(propertyName))
			{
				return;
			}
			this.targetMaterial = material;
			this.targetPropertyName = propertyName;
			this.useStandaloneMode = false;
			this.OnStandaloneColorChanged = null;
			this.originalColor = initialColor;
			this.currentColor = this.originalColor;
			Color.RGBToHSV(this.currentColor, out this.currentHue, out this.currentSat, out this.currentVal);
			Debug.Log(string.Format("ShowColorPicker - Colore iniziale HSV: {0}, {1}, {2}", this.currentHue, this.currentSat, this.currentVal));
			if (this.svTexture == null)
			{
				this.CreateSVImage();
			}
			if (this.hueTexture == null)
			{
				this.CreateHueImage();
			}
			if (this.hueSlider != null)
			{
				this.hueSlider.SetValueWithoutNotify(this.currentHue);
			}
			this.PreparePanelForOpen();
			this.UpdateSVImage();
			this.UpdateHueSelectorPosition();
			this.UpdateSVSelectorPosition();
			this.RefreshExternalControls();
			this.UpdateOutputImage();
			if (this.propertyNameText != null)
			{
				LocalizedGameText localizedGameText = LocalizedGameText.BindSource(this.propertyNameText, "cielos_apps", propertyName, null);
				if (localizedGameText != null)
				{
					localizedGameText.FitTranslationToRectangle(false);
				}
			}
			if (this.hexValueText != null)
			{
				this.hexValueText.text = "#" + ColorUtility.ToHtmlStringRGB(this.currentColor);
			}
			if (base.gameObject.activeInHierarchy)
			{
				base.StartCoroutine(this.SyncSelectorsNextFrame());
			}
			Debug.Log(string.Format("Color Picker aperto con HSV: {0}, {1}, {2}", this.currentHue, this.currentSat, this.currentVal));
		}

		// Token: 0x06003AC5 RID: 15045 RVA: 0x0015D078 File Offset: 0x0015B278
		public void ShowColorPicker(Color initialColor, string displayName, Action<Color> onColorChanged = null)
		{
			this.useStandaloneMode = true;
			this.targetMaterial = null;
			this.targetPropertyName = string.Empty;
			this.OnStandaloneColorChanged = onColorChanged;
			this.colorName = displayName;
			if (this.colorNameText != null)
			{
				CustomizationPresentation.BindColorLabel(this.colorNameText, this.colorName, false);
			}
			if (this.propertyNameText != null)
			{
				LocalizedGameText.BindSource(this.propertyNameText, "cielos_apps", string.Empty, null);
			}
			this.originalColor = initialColor;
			this.currentColor = this.originalColor;
			Color.RGBToHSV(this.currentColor, out this.currentHue, out this.currentSat, out this.currentVal);
			if (this.svTexture == null)
			{
				this.CreateSVImage();
			}
			if (this.hueTexture == null)
			{
				this.CreateHueImage();
			}
			if (this.hueSlider != null)
			{
				this.hueSlider.SetValueWithoutNotify(this.currentHue);
			}
			this.PreparePanelForOpen();
			this.UpdateSVImage();
			this.UpdateHueSelectorPosition();
			this.UpdateSVSelectorPosition();
			this.RefreshExternalControls();
			this.UpdateOutputImage();
			if (this.hexValueText != null)
			{
				this.hexValueText.text = "#" + ColorUtility.ToHtmlStringRGB(this.currentColor);
			}
			if (base.gameObject.activeInHierarchy)
			{
				base.StartCoroutine(this.SyncSelectorsNextFrame());
			}
		}

		// Token: 0x06003AC6 RID: 15046 RVA: 0x0015D1D4 File Offset: 0x0015B3D4
		private IEnumerator SyncSelectorsNextFrame()
		{
			yield return null;
			if (this.colorPickerPanel == null || !this.colorPickerPanel.activeInHierarchy)
			{
				yield break;
			}
			this.PreparePanelForOpen();
			this.UpdateHueSelectorPosition();
			this.UpdateSVSelectorPosition();
			this.RefreshExternalControls();
			yield break;
		}

		// Token: 0x06003AC7 RID: 15047 RVA: 0x0015D1E4 File Offset: 0x0015B3E4
		private void RefreshExternalControls()
		{
			SVImageControl[] array = Object.FindObjectsByType<SVImageControl>(FindObjectsSortMode.None);
			for (int i = 0; i < array.Length; i++)
			{
				array[i].UpdatePickerPosition(this.currentSat, this.currentVal);
			}
			HueSliderControl[] array2 = Object.FindObjectsByType<HueSliderControl>(FindObjectsSortMode.None);
			for (int j = 0; j < array2.Length; j++)
			{
				array2[j].UpdateColorPreview();
			}
		}

		// Token: 0x06003AC8 RID: 15048 RVA: 0x0015D238 File Offset: 0x0015B438
		private void PreparePanelForOpen()
		{
			ControllerNavigationScope.Configure(this.colorPickerPanel, new Action(this.CloseColorPicker), (this.closeButton != null) ? this.closeButton.gameObject : null, 100, false);
			if (this.colorPickerPanel != null && !this.colorPickerPanel.activeSelf)
			{
				this.colorPickerPanel.SetActive(true);
			}
			if (this.closeButton != null)
			{
				this.closeButton.onClick.RemoveListener(new UnityAction(this.CloseColorPicker));
				this.closeButton.onClick.AddListener(new UnityAction(this.CloseColorPicker));
			}
			Canvas.ForceUpdateCanvases();
			if (this.hueImage != null)
			{
				LayoutRebuilder.ForceRebuildLayoutImmediate(this.hueImage.rectTransform);
			}
			if (this.satValImage != null)
			{
				LayoutRebuilder.ForceRebuildLayoutImmediate(this.satValImage.rectTransform);
			}
			this.PolishPickerVisuals();
			this.EnsureColorUtilities();
		}

		// Token: 0x06003AC9 RID: 15049 RVA: 0x0015D338 File Offset: 0x0015B538
		private void PolishPickerVisuals()
		{
			if (this.colorPickerPanel != null && this.colorPickerPanel.GetComponent<Image>() != null)
			{
				CornerRounder cornerRounder = this.colorPickerPanel.GetComponent<CornerRounder>();
				if (cornerRounder == null)
				{
					cornerRounder = this.colorPickerPanel.AddComponent<CornerRounder>();
				}
				cornerRounder.SetRadii(new Vector4(12f, 12f, 12f, 12f));
			}
			ColorPickerControlExtended.PolishGradientGraphic(this.satValImage, 12f, true);
			ColorPickerControlExtended.PolishGradientGraphic(this.hueImage, 10f, false);
			ColorPickerControlExtended.PolishGradientGraphic(this.outputImage, 10f, false);
		}

		// Token: 0x06003ACA RID: 15050 RVA: 0x0015D3DC File Offset: 0x0015B5DC
		private static void PolishGradientGraphic(RawImage graphic, float radius, bool clearParentBacking)
		{
			if (graphic == null)
			{
				return;
			}
			CornerRounder cornerRounder = graphic.GetComponent<CornerRounder>();
			if (cornerRounder == null)
			{
				cornerRounder = graphic.gameObject.AddComponent<CornerRounder>();
			}
			cornerRounder.SetRadii(new Vector4(radius, radius, radius, radius));
			if (clearParentBacking && graphic.transform.parent != null)
			{
				Image component = graphic.transform.parent.GetComponent<Image>();
				if (component != null && component.sprite == null)
				{
					component.color = Color.clear;
					component.raycastTarget = false;
				}
			}
		}

		// Token: 0x06003ACB RID: 15051 RVA: 0x0015D470 File Offset: 0x0015B670
		private void EnsureColorUtilities()
		{
			if (this.colorPickerPanel == null || this.utilityBar != null)
			{
				return;
			}
			Transform transform = this.colorPickerPanel.transform.Find("Color Utilities");
			if (transform != null)
			{
				this.utilityBar = transform as RectTransform;
				LayoutElement layoutElement = transform.GetComponent<LayoutElement>();
				if (layoutElement == null)
				{
					layoutElement = transform.gameObject.AddComponent<LayoutElement>();
				}
				layoutElement.ignoreLayout = true;
				ColorPickerControlExtended.ApplyUtilityBarGeometry(this.utilityBar);
				return;
			}
			GameObject gameObject = new GameObject("Color Utilities", new Type[]
			{
				typeof(RectTransform),
				typeof(CanvasRenderer),
				typeof(Image),
				typeof(HorizontalLayoutGroup),
				typeof(LayoutElement)
			});
			gameObject.layer = this.colorPickerPanel.layer;
			gameObject.transform.SetParent(this.colorPickerPanel.transform, false);
			gameObject.GetComponent<LayoutElement>().ignoreLayout = true;
			this.utilityBar = gameObject.GetComponent<RectTransform>();
			ColorPickerControlExtended.ApplyUtilityBarGeometry(this.utilityBar);
			gameObject.GetComponent<Image>().color = new Color32(45, 25, 58, 245);
			gameObject.AddComponent<CornerRounder>().SetRadii(new Vector4(10f, 10f, 10f, 10f));
			HorizontalLayoutGroup component = gameObject.GetComponent<HorizontalLayoutGroup>();
			component.padding = new RectOffset(5, 5, 4, 4);
			component.spacing = 5f;
			component.childAlignment = TextAnchor.MiddleCenter;
			component.childControlWidth = true;
			component.childControlHeight = true;
			component.childForceExpandWidth = true;
			component.childForceExpandHeight = true;
			TMP_Text tmp_Text = this.CreateUtilityHexLabel(gameObject.transform);
			if (this.hexValueText == null)
			{
				this.hexValueText = tmp_Text;
			}
			this.copyButtonLabel = this.CreateUtilityButton(gameObject.transform, "Copy", new UnityAction(this.CopyCurrentColor));
			this.pasteButtonLabel = this.CreateUtilityButton(gameObject.transform, "Paste", new UnityAction(this.PasteColorFromClipboard));
			this.eyedropperButtonLabel = this.CreateUtilityButton(gameObject.transform, "Pick", new UnityAction(this.ToggleEyedropper));
		}

		// Token: 0x06003ACC RID: 15052 RVA: 0x0015D6A4 File Offset: 0x0015B8A4
		private static void ApplyUtilityBarGeometry(RectTransform bar)
		{
			if (bar == null)
			{
				return;
			}
			bar.anchorMin = new Vector2(0f, 0f);
			bar.anchorMax = new Vector2(1f, 0f);
			bar.pivot = new Vector2(0.5f, 1f);
			bar.anchoredPosition = new Vector2(0f, -6f);
			bar.sizeDelta = new Vector2(-8f, 34f);
		}

		// Token: 0x06003ACD RID: 15053 RVA: 0x0015D724 File Offset: 0x0015B924
		private TMP_Text CreateUtilityHexLabel(Transform parent)
		{
			GameObject gameObject = new GameObject("Hex", new Type[]
			{
				typeof(RectTransform),
				typeof(CanvasRenderer),
				typeof(TextMeshProUGUI),
				typeof(LayoutElement)
			});
			gameObject.layer = parent.gameObject.layer;
			gameObject.transform.SetParent(parent, false);
			LayoutElement component = gameObject.GetComponent<LayoutElement>();
			component.minWidth = 64f;
			component.preferredWidth = 64f;
			component.flexibleWidth = 0f;
			TextMeshProUGUI component2 = gameObject.GetComponent<TextMeshProUGUI>();
			component2.text = "#" + ColorUtility.ToHtmlStringRGB(this.currentColor);
			component2.fontSize = 11f;
			component2.fontStyle = FontStyles.Bold;
			component2.alignment = TextAlignmentOptions.Center;
			component2.color = Color.white;
			component2.raycastTarget = false;
			return component2;
		}

		// Token: 0x06003ACE RID: 15054 RVA: 0x0015D808 File Offset: 0x0015BA08
		private TMP_Text CreateUtilityButton(Transform parent, string label, UnityAction action)
		{
			GameObject gameObject = new GameObject(label, new Type[]
			{
				typeof(RectTransform),
				typeof(CanvasRenderer),
				typeof(Image),
				typeof(Button),
				typeof(LayoutElement)
			});
			gameObject.layer = parent.gameObject.layer;
			gameObject.transform.SetParent(parent, false);
			LayoutElement component = gameObject.GetComponent<LayoutElement>();
			component.minWidth = 52f;
			component.minHeight = 24f;
			component.flexibleWidth = 1f;
			gameObject.GetComponent<Image>().color = new Color32(100, 50, 120, byte.MaxValue);
			gameObject.AddComponent<CornerRounder>().SetRadii(new Vector4(7f, 7f, 7f, 7f));
			Button component2 = gameObject.GetComponent<Button>();
			component2.onClick.AddListener(action);
			ColorBlock colors = component2.colors;
			colors.normalColor = Color.white;
			colors.highlightedColor = new Color(1f, 0.82f, 0.94f, 1f);
			colors.pressedColor = new Color(0.82f, 0.64f, 0.78f, 1f);
			colors.fadeDuration = 0.08f;
			component2.colors = colors;
			GameObject gameObject2 = new GameObject("Label", new Type[]
			{
				typeof(RectTransform),
				typeof(CanvasRenderer),
				typeof(TextMeshProUGUI)
			});
			gameObject2.layer = parent.gameObject.layer;
			gameObject2.transform.SetParent(gameObject.transform, false);
			RectTransform component3 = gameObject2.GetComponent<RectTransform>();
			component3.anchorMin = Vector2.zero;
			component3.anchorMax = Vector2.one;
			component3.offsetMin = Vector2.zero;
			component3.offsetMax = Vector2.zero;
			TextMeshProUGUI component4 = gameObject2.GetComponent<TextMeshProUGUI>();
			component4.fontSize = 11f;
			component4.fontStyle = FontStyles.Bold;
			component4.alignment = TextAlignmentOptions.Center;
			component4.color = Color.white;
			component4.raycastTarget = false;
			LocalizedGameText localizedGameText = LocalizedGameText.BindSource(component4, "cielos_apps", label, (string value) => value.ToUpperInvariant());
			if (localizedGameText == null)
			{
				return component4;
			}
			localizedGameText.FitTranslationToRectangle(false);
			return component4;
		}

		// Token: 0x06003ACF RID: 15055 RVA: 0x0015DA58 File Offset: 0x0015BC58
		private void CopyCurrentColor()
		{
			GUIUtility.systemCopyBuffer = "#" + ColorUtility.ToHtmlStringRGB(this.currentColor);
			if (this.copyButtonLabel != null)
			{
				base.StartCoroutine(ColorPickerControlExtended.FlashUtilityLabel(this.copyButtonLabel, "COPIED", "COPY"));
			}
		}

		// Token: 0x06003AD0 RID: 15056 RVA: 0x0015DAAC File Offset: 0x0015BCAC
		private void PasteColorFromClipboard()
		{
			string text = ((GUIUtility.systemCopyBuffer != null) ? GUIUtility.systemCopyBuffer.Trim() : string.Empty);
			if (!text.StartsWith("#"))
			{
				text = "#" + text;
			}
			Color color;
			if (ColorUtility.TryParseHtmlString(text, out color))
			{
				color.a = 1f;
				this.SetColorFromUtility(color);
				if (this.pasteButtonLabel != null)
				{
					base.StartCoroutine(ColorPickerControlExtended.FlashUtilityLabel(this.pasteButtonLabel, "APPLIED", "PASTE"));
					return;
				}
			}
			else if (this.pasteButtonLabel != null)
			{
				base.StartCoroutine(ColorPickerControlExtended.FlashUtilityLabel(this.pasteButtonLabel, "INVALID", "PASTE"));
			}
		}

		// Token: 0x06003AD1 RID: 15057 RVA: 0x0015DB5D File Offset: 0x0015BD5D
		private void ToggleEyedropper()
		{
			this.SetEyedropperArmed(!this.eyedropperArmed);
		}

		// Token: 0x06003AD2 RID: 15058 RVA: 0x0015DB70 File Offset: 0x0015BD70
		private void SetEyedropperArmed(bool armed)
		{
			this.eyedropperArmed = armed;
			if (this.eyedropperButtonLabel != null)
			{
				LocalizedGameText localizedGameText = LocalizedGameText.BindSource(this.eyedropperButtonLabel, "cielos_apps", armed ? "CLICK COLOR" : "PICK", null);
				if (localizedGameText == null)
				{
					return;
				}
				localizedGameText.FitTranslationToRectangle(false);
			}
		}

		// Token: 0x06003AD3 RID: 15059 RVA: 0x0015DBBD File Offset: 0x0015BDBD
		private IEnumerator SampleScreenPixel(Vector2 screenPoint)
		{
			yield return new WaitForEndOfFrame();
			Texture2D texture2D = ScreenCapture.CaptureScreenshotAsTexture();
			if (texture2D == null)
			{
				yield break;
			}
			int num = Mathf.Clamp(Mathf.RoundToInt(screenPoint.x), 0, texture2D.width - 1);
			int num2 = Mathf.Clamp(Mathf.RoundToInt(screenPoint.y), 0, texture2D.height - 1);
			Color pixel = texture2D.GetPixel(num, num2);
			pixel.a = 1f;
			Object.Destroy(texture2D);
			this.SetColorFromUtility(pixel);
			yield break;
		}

		// Token: 0x06003AD4 RID: 15060 RVA: 0x0015DBD4 File Offset: 0x0015BDD4
		private void SetColorFromUtility(Color color)
		{
			this.currentColor = color;
			Color.RGBToHSV(color, out this.currentHue, out this.currentSat, out this.currentVal);
			if (this.hueSlider != null)
			{
				this.hueSlider.SetValueWithoutNotify(this.currentHue);
			}
			this.UpdateSVImage();
			this.UpdateHueSelectorPosition();
			this.UpdateSVSelectorPosition();
			this.UpdateOutputImage();
			this.RefreshExternalControls();
		}

		// Token: 0x06003AD5 RID: 15061 RVA: 0x0015DC3D File Offset: 0x0015BE3D
		private static IEnumerator FlashUtilityLabel(TMP_Text label, string temporary, string normal)
		{
			LocalizedGameText localizedGameText = LocalizedGameText.BindSource(label, "cielos_apps", temporary, null);
			if (localizedGameText != null)
			{
				localizedGameText.FitTranslationToRectangle(false);
			}
			yield return new WaitForSecondsRealtime(0.8f);
			if (label != null)
			{
				LocalizedGameText localizedGameText2 = LocalizedGameText.BindSource(label, "cielos_apps", normal, null);
				if (localizedGameText2 != null)
				{
					localizedGameText2.FitTranslationToRectangle(false);
				}
			}
			yield break;
		}

		// Token: 0x06003AD6 RID: 15062 RVA: 0x0015DC5C File Offset: 0x0015BE5C
		public void ShowColorPicker(Material material, string propertyName, string displayName)
		{
			if (material == null || string.IsNullOrEmpty(propertyName))
			{
				return;
			}
			this.colorName = displayName;
			if (this.colorNameText != null)
			{
				CustomizationPresentation.BindColorLabel(this.colorNameText, this.colorName, false);
			}
			this.ShowColorPicker(material, propertyName, material.GetColor(propertyName));
		}

		// Token: 0x06003AD7 RID: 15063 RVA: 0x0015DCB4 File Offset: 0x0015BEB4
		private void CreateHueImage()
		{
			if (this.hueImage == null)
			{
				return;
			}
			this.hueTexture = new Texture2D(256, 16);
			this.hueTexture.wrapMode = TextureWrapMode.Clamp;
			for (int i = 0; i < this.hueTexture.width; i++)
			{
				Color color = Color.HSVToRGB((float)i / (float)this.hueTexture.width, 1f, 1f);
				for (int j = 0; j < this.hueTexture.height; j++)
				{
					this.hueTexture.SetPixel(i, j, color);
				}
			}
			this.hueTexture.Apply();
			this.hueImage.texture = this.hueTexture;
			RectTransform component = this.hueImage.GetComponent<RectTransform>();
			if (component != null)
			{
				component.anchorMin = new Vector2(0f, 0.5f);
				component.anchorMax = new Vector2(1f, 0.5f);
				component.sizeDelta = new Vector2(0f, 20f);
				if (component.gameObject.activeInHierarchy)
				{
					LayoutRebuilder.ForceRebuildLayoutImmediate(component);
				}
			}
		}

		// Token: 0x06003AD8 RID: 15064 RVA: 0x0015DDCC File Offset: 0x0015BFCC
		private void CreateSVImage()
		{
			if (this.satValImage == null)
			{
				return;
			}
			this.svTexture = new Texture2D(256, 256);
			this.svTexture.wrapMode = TextureWrapMode.Clamp;
			for (int i = 0; i < this.svTexture.height; i++)
			{
				for (int j = 0; j < this.svTexture.width; j++)
				{
					this.svTexture.SetPixel(j, i, Color.HSVToRGB(this.currentHue, (float)j / (float)this.svTexture.width, (float)i / (float)this.svTexture.height));
				}
			}
			this.svTexture.Apply();
			this.satValImage.texture = this.svTexture;
			RectTransform component = this.satValImage.GetComponent<RectTransform>();
			if (component != null)
			{
				component.anchorMin = new Vector2(0f, 0f);
				component.anchorMax = new Vector2(1f, 1f);
				component.offsetMin = Vector2.zero;
				component.offsetMax = Vector2.zero;
			}
		}

		// Token: 0x06003AD9 RID: 15065 RVA: 0x0015DEDC File Offset: 0x0015C0DC
		private void CreateOutputImage()
		{
			if (this.outputImage == null)
			{
				return;
			}
			this.outputTexture = new Texture2D(64, 64);
			this.outputTexture.wrapMode = TextureWrapMode.Clamp;
			Color color = Color.HSVToRGB(this.currentHue, this.currentSat, this.currentVal);
			this.currentColor = color;
			for (int i = 0; i < this.outputTexture.height; i++)
			{
				for (int j = 0; j < this.outputTexture.width; j++)
				{
					this.outputTexture.SetPixel(j, i, color);
				}
			}
			this.outputTexture.Apply();
			this.outputImage.texture = this.outputTexture;
		}

		// Token: 0x06003ADA RID: 15066 RVA: 0x0015DF88 File Offset: 0x0015C188
		private void UpdateOutputImage()
		{
			if (this.outputImage == null || this.outputTexture == null)
			{
				return;
			}
			Color color = Color.HSVToRGB(this.currentHue, this.currentSat, this.currentVal);
			this.currentColor = color;
			for (int i = 0; i < this.outputTexture.height; i++)
			{
				for (int j = 0; j < this.outputTexture.width; j++)
				{
					this.outputTexture.SetPixel(j, i, color);
				}
			}
			this.outputTexture.Apply();
			if (this.previewSwatch != null)
			{
				this.previewSwatch.color = color;
			}
			if (this.hexValueText != null)
			{
				this.hexValueText.text = "#" + ColorUtility.ToHtmlStringRGB(color);
			}
			if (!(this.targetMaterial != null) || !this.targetMaterial.HasProperty(this.targetPropertyName))
			{
				if (this.useStandaloneMode)
				{
					Action<Color> onStandaloneColorChanged = this.OnStandaloneColorChanged;
					if (onStandaloneColorChanged == null)
					{
						return;
					}
					onStandaloneColorChanged(color);
				}
				return;
			}
			this.targetMaterial.SetColor(this.targetPropertyName, color);
			Action<Material, string, Color> onColorChanged = this.OnColorChanged;
			if (onColorChanged == null)
			{
				return;
			}
			onColorChanged(this.targetMaterial, this.targetPropertyName, color);
		}

		// Token: 0x06003ADB RID: 15067 RVA: 0x0015E0C3 File Offset: 0x0015C2C3
		public void SetSV(float S, float V)
		{
			this.currentSat = S;
			this.currentVal = V;
			this.UpdateOutputImage();
			this.UpdateSVSelectorPosition();
			this.RefreshExternalControls();
		}

		// Token: 0x06003ADC RID: 15068 RVA: 0x0015E0E5 File Offset: 0x0015C2E5
		public void SetHue(float H)
		{
			this.currentHue = H;
			this.UpdateSVImage();
			this.UpdateOutputImage();
			this.UpdateHueSelectorPosition();
			this.RefreshExternalControls();
			Debug.Log(string.Format("Hue impostato a: {0}", this.currentHue));
		}

		// Token: 0x06003ADD RID: 15069 RVA: 0x0015E120 File Offset: 0x0015C320
		public void UpdateSVImage()
		{
			if (this.svTexture == null)
			{
				Debug.LogWarning("svTexture è null, la creo ora");
				if (!(this.satValImage != null))
				{
					Debug.LogError("satValImage è null, impossibile creare la texture");
					return;
				}
				this.svTexture = new Texture2D(256, 256);
				this.svTexture.wrapMode = TextureWrapMode.Clamp;
				this.satValImage.texture = this.svTexture;
			}
			for (int i = 0; i < this.svTexture.height; i++)
			{
				for (int j = 0; j < this.svTexture.width; j++)
				{
					this.svTexture.SetPixel(j, i, Color.HSVToRGB(this.currentHue, (float)j / (float)this.svTexture.width, (float)i / (float)this.svTexture.height));
				}
			}
			this.svTexture.Apply();
			this.UpdateOutputImage();
			this.UpdateSVSelectorPosition();
		}

		// Token: 0x06003ADE RID: 15070 RVA: 0x0015E20C File Offset: 0x0015C40C
		private void OnSVPointerDown(PointerEventData eventData)
		{
			if (this.satValImage == null)
			{
				return;
			}
			Vector2 vector;
			RectTransformUtility.ScreenPointToLocalPointInRectangle(this.satValImage.rectTransform, eventData.position, eventData.pressEventCamera, out vector);
			Rect rect = this.satValImage.rectTransform.rect;
			float num = Mathf.Clamp01((vector.x - rect.xMin) / rect.width);
			float num2 = Mathf.Clamp01((vector.y - rect.yMin) / rect.height);
			this.SetSV(num, num2);
			Debug.Log(string.Format("SV aggiornati: S={0}, V={1}", num, num2));
		}

		// Token: 0x06003ADF RID: 15071 RVA: 0x0015E2B3 File Offset: 0x0015C4B3
		private void OnSVDrag(PointerEventData eventData)
		{
			this.OnSVPointerDown(eventData);
		}

		// Token: 0x06003AE0 RID: 15072 RVA: 0x0015E2BC File Offset: 0x0015C4BC
		private void UpdateSVSelectorPosition()
		{
			if (this.satValSelector == null || this.satValImage == null)
			{
				return;
			}
			Rect rect = this.satValImage.rectTransform.rect;
			if (rect.width <= 1f || rect.height <= 1f)
			{
				return;
			}
			float num = Mathf.Lerp(rect.xMin, rect.xMax, Mathf.Clamp01(this.currentSat));
			float num2 = Mathf.Lerp(rect.yMin, rect.yMax, Mathf.Clamp01(this.currentVal));
			this.satValSelector.anchoredPosition = new Vector2(num, num2);
			Debug.Log(string.Format("SV Selector position: {0}, {1} - Sat: {2}, Val: {3}", new object[] { num, num2, this.currentSat, this.currentVal }));
		}

		// Token: 0x06003AE1 RID: 15073 RVA: 0x0015E3A8 File Offset: 0x0015C5A8
		private void UpdateHueSelectorPosition()
		{
			if (this.hueSelector == null || this.hueImage == null)
			{
				return;
			}
			Rect rect = this.hueImage.rectTransform.rect;
			if (rect.width <= 1f)
			{
				return;
			}
			float num = 5f;
			float num2 = rect.width - num * 2f;
			float num3 = rect.xMin + num;
			float num4 = Mathf.Lerp(num3, num3 + num2, Mathf.Clamp01(this.currentHue));
			this.hueSelector.anchoredPosition = new Vector2(num4, 0f);
			HueSliderControl[] array = Object.FindObjectsByType<HueSliderControl>(FindObjectsSortMode.None);
			for (int i = 0; i < array.Length; i++)
			{
				array[i].UpdateColorPreview();
			}
			Debug.Log(string.Format("Hue Selector position: {0} - Hue: {1}", num4, this.currentHue));
		}

		// Token: 0x06003AE2 RID: 15074 RVA: 0x0015E480 File Offset: 0x0015C680
		private void CloseColorPicker()
		{
			this.SetEyedropperArmed(false);
			base.StopAllCoroutines();
			if (this.targetMaterial != null && this.targetMaterial.HasProperty(this.targetPropertyName))
			{
				this.targetMaterial.SetColor(this.targetPropertyName, this.currentColor);
				Action<Material, string, Color> onColorChanged = this.OnColorChanged;
				if (onColorChanged != null)
				{
					onColorChanged(this.targetMaterial, this.targetPropertyName, this.currentColor);
				}
				Debug.Log(string.Concat(new string[]
				{
					"Colore applicato alla chiusura: ",
					ColorUtility.ToHtmlStringRGB(this.currentColor),
					" a ",
					this.targetPropertyName,
					" su ",
					this.targetMaterial.name
				}));
			}
			else if (this.useStandaloneMode)
			{
				Action<Color> onStandaloneColorChanged = this.OnStandaloneColorChanged;
				if (onStandaloneColorChanged != null)
				{
					onStandaloneColorChanged(this.currentColor);
				}
			}
			if (this.colorPickerPanel != null)
			{
				this.colorPickerPanel.SetActive(false);
			}
			Action onColorPickerClosed = this.OnColorPickerClosed;
			if (onColorPickerClosed != null)
			{
				onColorPickerClosed();
			}
			this.targetMaterial = null;
			this.targetPropertyName = "";
			this.OnColorChanged = null;
			this.OnStandaloneColorChanged = null;
			this.useStandaloneMode = false;
		}

		// Token: 0x17000A13 RID: 2579
		// (get) Token: 0x06003AE3 RID: 15075 RVA: 0x0015E5BB File Offset: 0x0015C7BB
		public bool IsOpen
		{
			get
			{
				return this.colorPickerPanel != null && this.colorPickerPanel.activeSelf;
			}
		}

		// Token: 0x06003AE4 RID: 15076 RVA: 0x0015E5D8 File Offset: 0x0015C7D8
		public void Close()
		{
			this.SetEyedropperArmed(false);
			base.StopAllCoroutines();
			if (this.colorPickerPanel != null)
			{
				this.colorPickerPanel.SetActive(false);
			}
			Action onColorPickerClosed = this.OnColorPickerClosed;
			if (onColorPickerClosed != null)
			{
				onColorPickerClosed();
			}
			this.targetMaterial = null;
			this.targetPropertyName = "";
			this.OnColorChanged = null;
			this.OnStandaloneColorChanged = null;
			this.useStandaloneMode = false;
		}

		// Token: 0x06003AE5 RID: 15077 RVA: 0x0015E644 File Offset: 0x0015C844
		private void ApplyColor()
		{
			this.CloseColorPicker();
		}

		// Token: 0x06003AE6 RID: 15078 RVA: 0x0015E64C File Offset: 0x0015C84C
		private void ResetColor()
		{
			if (!(this.targetMaterial != null) || !this.targetMaterial.HasProperty(this.targetPropertyName))
			{
				if (this.useStandaloneMode)
				{
					this.currentColor = this.originalColor;
					Color.RGBToHSV(this.originalColor, out this.currentHue, out this.currentSat, out this.currentVal);
					if (this.hueSlider != null)
					{
						this.hueSlider.value = this.currentHue;
					}
					this.UpdateSVImage();
					this.UpdateOutputImage();
					Action<Color> onStandaloneColorChanged = this.OnStandaloneColorChanged;
					if (onStandaloneColorChanged == null)
					{
						return;
					}
					onStandaloneColorChanged(this.originalColor);
				}
				return;
			}
			this.targetMaterial.SetColor(this.targetPropertyName, this.originalColor);
			this.currentColor = this.originalColor;
			Color.RGBToHSV(this.originalColor, out this.currentHue, out this.currentSat, out this.currentVal);
			if (this.hueSlider != null)
			{
				this.hueSlider.value = this.currentHue;
			}
			this.UpdateSVImage();
			this.UpdateOutputImage();
			Action<Material, string, Color> onColorChanged = this.OnColorChanged;
			if (onColorChanged == null)
			{
				return;
			}
			onColorChanged(this.targetMaterial, this.targetPropertyName, this.originalColor);
		}

		// Token: 0x04003B04 RID: 15108
		[Header("Picker")]
		public float currentHue;

		// Token: 0x04003B05 RID: 15109
		public float currentSat;

		// Token: 0x04003B06 RID: 15110
		public float currentVal;

		// Token: 0x04003B07 RID: 15111
		private Color currentColor;

		// Token: 0x04003B08 RID: 15112
		private Color originalColor;

		// Token: 0x04003B09 RID: 15113
		[SerializeField]
		private RawImage hueImage;

		// Token: 0x04003B0A RID: 15114
		[SerializeField]
		private RawImage satValImage;

		// Token: 0x04003B0B RID: 15115
		[SerializeField]
		private RawImage outputImage;

		// Token: 0x04003B0C RID: 15116
		[SerializeField]
		private Slider hueSlider;

		// Token: 0x04003B0D RID: 15117
		private Texture2D hueTexture;

		// Token: 0x04003B0E RID: 15118
		private Texture2D svTexture;

		// Token: 0x04003B0F RID: 15119
		private Texture2D outputTexture;

		// Token: 0x04003B10 RID: 15120
		[Header("Target Material")]
		[SerializeField]
		private Material targetMaterial;

		// Token: 0x04003B11 RID: 15121
		[SerializeField]
		private string targetPropertyName = "_Color_1";

		// Token: 0x04003B12 RID: 15122
		[SerializeField]
		private TMP_Text propertyNameText;

		// Token: 0x04003B13 RID: 15123
		[SerializeField]
		private TMP_Text colorNameText;

		// Token: 0x04003B14 RID: 15124
		[SerializeField]
		private TMP_Text hexValueText;

		// Token: 0x04003B15 RID: 15125
		[SerializeField]
		private Image previewSwatch;

		// Token: 0x04003B16 RID: 15126
		private string colorName;

		// Token: 0x04003B17 RID: 15127
		[Header("UI")]
		[SerializeField]
		private GameObject colorPickerPanel;

		// Token: 0x04003B18 RID: 15128
		[SerializeField]
		private Button closeButton;

		// Token: 0x04003B19 RID: 15129
		[SerializeField]
		private Button applyButton;

		// Token: 0x04003B1A RID: 15130
		[SerializeField]
		private Button resetButton;

		// Token: 0x04003B1B RID: 15131
		[SerializeField]
		private RectTransform satValSelector;

		// Token: 0x04003B1C RID: 15132
		[SerializeField]
		private RectTransform hueSelector;

		// Token: 0x04003B1D RID: 15133
		private RectTransform utilityBar;

		// Token: 0x04003B1E RID: 15134
		private TMP_Text copyButtonLabel;

		// Token: 0x04003B1F RID: 15135
		private TMP_Text pasteButtonLabel;

		// Token: 0x04003B20 RID: 15136
		private TMP_Text eyedropperButtonLabel;

		// Token: 0x04003B21 RID: 15137
		private bool eyedropperArmed;

		// Token: 0x04003B22 RID: 15138
		public Action<Material, string, Color> OnColorChanged;

		// Token: 0x04003B23 RID: 15139
		public Action<Color> OnStandaloneColorChanged;

		// Token: 0x04003B24 RID: 15140
		private bool useStandaloneMode;

		// Token: 0x04003B25 RID: 15141
		public Action OnColorPickerClosed;
	}
}
