using System;
using System.Collections;
using System.Collections.Generic;
using Bozo.AnimeCharacters;
using DA_Assets.CR;
using Features.Localization;
using Features.MainMenu;
using ProjectExhibitionist.CharacterCustomization.Core;
using ProjectExhibitionist.CharacterCustomization.Core.Runtime;
using ProjectExhibitionist.Core;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000007 RID: 7
public class CharacterCreatorManager : MonoBehaviour
{
	// Token: 0x17000009 RID: 9
	// (get) Token: 0x06000038 RID: 56 RVA: 0x00002DE6 File Offset: 0x00000FE6
	public OutfitSystem Character
	{
		get
		{
			return this.outfitSystem;
		}
	}

	// Token: 0x1700000A RID: 10
	// (get) Token: 0x06000039 RID: 57 RVA: 0x00002DEE File Offset: 0x00000FEE
	public RenderTexture IconTexture
	{
		get
		{
			return this.iconTexture;
		}
	}

	// Token: 0x0600003A RID: 58 RVA: 0x00002DF8 File Offset: 0x00000FF8
	public Button ControllerStepCategory(int direction)
	{
		if (direction == 0 || this.categoryButtons.Count == 0)
		{
			return null;
		}
		int num = this.categoryButtons.IndexOf(this.currentCategory);
		for (int i = 1; i <= this.categoryButtons.Count; i++)
		{
			int num2 = (Mathf.Max(0, num) + ((direction > 0) ? i : (-i)) + this.categoryButtons.Count) % this.categoryButtons.Count;
			CharacterCreatorManager.CategoryButton categoryButton = this.categoryButtons[num2];
			Button button = categoryButton.button;
			if (!(button == null) && button.isActiveAndEnabled && button.IsInteractable() && (!this.metadataOnlySystemNpc || CharacterCreatorManager.IsMetadataCategory(categoryButton.categoryName)))
			{
				button.onClick.Invoke();
				return button;
			}
		}
		return null;
	}

	// Token: 0x0600003B RID: 59 RVA: 0x00002EC8 File Offset: 0x000010C8
	public Button ControllerStepSubcategory(int direction)
	{
		SubCategorySelector subCategorySelector = ((this.currentCategory != null && this.currentCategory.categoryContent != null) ? this.currentCategory.categoryContent.GetComponentInChildren<SubCategorySelector>() : null);
		if (!(subCategorySelector != null))
		{
			return null;
		}
		return subCategorySelector.ControllerStep(direction);
	}

	// Token: 0x0600003C RID: 60 RVA: 0x00002F18 File Offset: 0x00001118
	private void OnEnable()
	{
		if (this.npcPhotoPreview != null && this.npcLivePreviewRoutine == null)
		{
			this.npcLivePreviewRoutine = base.StartCoroutine(this.RenderNpcLivePreview(this.npcPhotoPreview));
		}
		this.ResolveSessionOrchestrator();
		if (this.sessionOrchestrator != null)
		{
			this.sessionOrchestrator.OnSessionInitialized -= this.HandleSessionInitialized;
			this.sessionOrchestrator.OnSessionInitialized += this.HandleSessionInitialized;
			this.ApplySessionRestrictions(this.sessionOrchestrator.CurrentSession);
		}
	}

	// Token: 0x0600003D RID: 61 RVA: 0x00002FA8 File Offset: 0x000011A8
	private void OnDisable()
	{
		if (this.npcLivePreviewRoutine != null)
		{
			base.StopCoroutine(this.npcLivePreviewRoutine);
		}
		this.npcLivePreviewRoutine = null;
		if (this.sessionOrchestrator != null)
		{
			this.sessionOrchestrator.OnSessionInitialized -= this.HandleSessionInitialized;
		}
	}

	// Token: 0x0600003E RID: 62 RVA: 0x00002FF8 File Offset: 0x000011F8
	private void Start()
	{
		this.PolishNpcNavigation();
		this.BuildNpcPhotoSectionV2();
		this.InitializeCategories();
		this.categoriesInitialized = true;
		this.ApplySessionRestrictions((this.sessionOrchestrator != null) ? this.sessionOrchestrator.CurrentSession : null);
		this.SetupCamera();
		this.CacheDefaultCharacterState();
	}

	// Token: 0x0600003F RID: 63 RVA: 0x0000304C File Offset: 0x0000124C
	private void BuildNpcPhotoSectionV2()
	{
		Transform transform = base.transform.Find("Toolbar/Tools/Main_Panel/General_Category/Content/SCROLLVIEW/VIEWPORT/Info_Container");
		if (transform == null || transform.Find("NPC Photo Section") != null)
		{
			return;
		}
		GameObject gameObject = new GameObject("NPC Photo Section", new Type[]
		{
			typeof(RectTransform),
			typeof(VerticalLayoutGroup),
			typeof(LayoutElement)
		});
		gameObject.layer = base.gameObject.layer;
		gameObject.transform.SetParent(transform, false);
		gameObject.transform.SetSiblingIndex(0);
		RectTransform component = gameObject.GetComponent<RectTransform>();
		component.sizeDelta = new Vector2(component.sizeDelta.x, 410f);
		LayoutElement component2 = gameObject.GetComponent<LayoutElement>();
		component2.minHeight = 410f;
		component2.preferredHeight = 410f;
		VerticalLayoutGroup component3 = gameObject.GetComponent<VerticalLayoutGroup>();
		component3.spacing = 10f;
		component3.childAlignment = TextAnchor.UpperLeft;
		component3.childControlWidth = true;
		component3.childControlHeight = false;
		component3.childForceExpandWidth = true;
		component3.childForceExpandHeight = false;
		CharacterCreatorManager.CreateNpcPhotoText(gameObject.transform, "Title", "NPC PICTURE", 22f, 500f, 36f).fontStyle = FontStyles.Bold | FontStyles.SmallCaps;
		GameObject gameObject2 = new GameObject("Photo Row", new Type[]
		{
			typeof(RectTransform),
			typeof(HorizontalLayoutGroup),
			typeof(LayoutElement)
		});
		gameObject2.layer = base.gameObject.layer;
		gameObject2.transform.SetParent(gameObject.transform, false);
		RectTransform component4 = gameObject2.GetComponent<RectTransform>();
		component4.sizeDelta = new Vector2(component4.sizeDelta.x, 350f);
		LayoutElement component5 = gameObject2.GetComponent<LayoutElement>();
		component5.minHeight = 350f;
		component5.preferredHeight = 350f;
		HorizontalLayoutGroup component6 = gameObject2.GetComponent<HorizontalLayoutGroup>();
		component6.spacing = 24f;
		component6.childAlignment = TextAnchor.MiddleLeft;
		component6.childControlWidth = false;
		component6.childControlHeight = false;
		component6.childForceExpandWidth = false;
		component6.childForceExpandHeight = false;
		GameObject gameObject3 = new GameObject("Photo Frame", new Type[]
		{
			typeof(RectTransform),
			typeof(CanvasRenderer),
			typeof(Image),
			typeof(Button),
			typeof(LayoutElement)
		});
		gameObject3.layer = base.gameObject.layer;
		gameObject3.transform.SetParent(gameObject2.transform, false);
		gameObject3.GetComponent<RectTransform>().sizeDelta = new Vector2(244f, 244f);
		LayoutElement component7 = gameObject3.GetComponent<LayoutElement>();
		component7.minWidth = (component7.preferredWidth = 244f);
		component7.minHeight = (component7.preferredHeight = 244f);
		gameObject3.GetComponent<Image>().color = new Color32(56, 35, 70, byte.MaxValue);
		gameObject3.AddComponent<CornerRounder>().SetRadii(new Vector4(14f, 14f, 14f, 14f));
		Outline outline = gameObject3.AddComponent<Outline>();
		outline.effectColor = new Color32(157, 86, 190, byte.MaxValue);
		outline.effectDistance = new Vector2(3f, -3f);
		GameObject gameObject4 = new GameObject("Preview", new Type[]
		{
			typeof(RectTransform),
			typeof(CanvasRenderer),
			typeof(RawImage)
		});
		gameObject4.layer = base.gameObject.layer;
		gameObject4.transform.SetParent(gameObject3.transform, false);
		RectTransform component8 = gameObject4.GetComponent<RectTransform>();
		component8.anchorMin = Vector2.zero;
		component8.anchorMax = Vector2.one;
		component8.offsetMin = new Vector2(6f, 6f);
		component8.offsetMax = new Vector2(-6f, -6f);
		AspectRatioFitter photoAspect = gameObject4.AddComponent<AspectRatioFitter>();
		photoAspect.aspectMode = AspectRatioFitter.AspectMode.FitInParent;
		photoAspect.aspectRatio = 1f;
		RawImage preview = gameObject4.GetComponent<RawImage>();
		preview.texture = this.EnsureNpcLivePreviewTexture();
		preview.color = Color.clear;
		preview.raycastTarget = false;
		this.npcPhotoPreview = preview;
		gameObject4.AddComponent<CornerRounder>().SetRadii(new Vector4(10f, 10f, 10f, 10f));
		this.npcLivePreviewRoutine = base.StartCoroutine(this.RenderNpcLivePreview(preview));
		GameObject gameObject5 = new GameObject("Photo Actions", new Type[]
		{
			typeof(RectTransform),
			typeof(VerticalLayoutGroup),
			typeof(LayoutElement)
		});
		gameObject5.layer = base.gameObject.layer;
		gameObject5.transform.SetParent(gameObject2.transform, false);
		gameObject5.GetComponent<RectTransform>().sizeDelta = new Vector2(225f, 344f);
		LayoutElement component9 = gameObject5.GetComponent<LayoutElement>();
		component9.preferredWidth = 225f;
		component9.preferredHeight = 344f;
		VerticalLayoutGroup component10 = gameObject5.GetComponent<VerticalLayoutGroup>();
		component10.padding = new RectOffset(0, 0, 68, 0);
		component10.spacing = 12f;
		component10.childAlignment = TextAnchor.UpperCenter;
		component10.childControlWidth = false;
		component10.childControlHeight = false;
		component10.childForceExpandWidth = false;
		component10.childForceExpandHeight = false;
		TextMeshProUGUI textMeshProUGUI = CharacterCreatorManager.CreateNpcPhotoText(gameObject5.transform, "Action Title", "TAKE A PHOTO", 18f, 225f, 28f);
		textMeshProUGUI.fontStyle = FontStyles.Bold | FontStyles.SmallCaps;
		textMeshProUGUI.alignment = TextAlignmentOptions.Center;
		TextMeshProUGUI textMeshProUGUI2 = CharacterCreatorManager.CreateNpcPhotoText(gameObject5.transform, "Hint", "Capture the current NPC preview for the preset card.", 14f, 225f, 54f);
		textMeshProUGUI2.alignment = TextAlignmentOptions.Center;
		textMeshProUGUI2.enableWordWrapping = true;
		TextMeshProUGUI captureLabel;
		Button button = CharacterCreatorManager.CreateNpcPhotoButton(gameObject5.transform, "Capture Photo", "TAKE PHOTO", new Color32(231, 84, 123, byte.MaxValue), out captureLabel);
		TextMeshProUGUI textMeshProUGUI3;
		Button clearButton = CharacterCreatorManager.CreateNpcPhotoButton(gameObject5.transform, "Clear Photo", "USE LIVE PREVIEW", new Color32(91, 52, 108, byte.MaxValue), out textMeshProUGUI3);
		clearButton.gameObject.SetActive(false);
		TextMeshProUGUI status = CharacterCreatorManager.CreateNpcPhotoText(gameObject5.transform, "Status", string.Empty, 12f, 225f, 24f);
		status.fontStyle = FontStyles.Bold | FontStyles.SmallCaps;
		status.alignment = TextAlignmentOptions.Center;
		status.color = new Color32(240, 111, 150, byte.MaxValue);
		this.npcPhotoCaptureLabel = captureLabel;
		this.npcPhotoStatus = status;
		this.npcPhotoClearButton = clearButton;
		Action capture = delegate
		{
			NpcCustomizationController npcCustomizationController = Object.FindObjectOfType<NpcCustomizationController>(true);
			Texture2D texture2D = ((npcCustomizationController != null) ? npcCustomizationController.CapturePresetPhoto() : this.GenerateCharacterIcon());
			if (texture2D == null)
			{
				return;
			}
			preview.texture = texture2D;
			photoAspect.aspectRatio = (float)texture2D.width / (float)texture2D.height;
			preview.color = Color.white;
			LocalizedGameText localizedGameText = LocalizedGameText.BindSource(captureLabel, "cielos_apps", "RETAKE PHOTO", null);
			if (localizedGameText != null)
			{
				localizedGameText.FitTranslationToRectangle(false);
			}
			clearButton.gameObject.SetActive(true);
			LocalizedGameText localizedGameText2 = LocalizedGameText.BindSource(status, "cielos_apps", "PHOTO READY", null);
			if (localizedGameText2 == null)
			{
				return;
			}
			localizedGameText2.FitTranslationToRectangle(false);
		};
		button.onClick.AddListener(delegate
		{
			capture();
		});
		gameObject3.GetComponent<Button>().onClick.AddListener(delegate
		{
			capture();
		});
		clearButton.onClick.AddListener(delegate
		{
			NpcCustomizationController npcCustomizationController2 = Object.FindObjectOfType<NpcCustomizationController>(true);
			if (npcCustomizationController2 != null)
			{
				npcCustomizationController2.ClearCapturedPresetPhoto();
			}
			preview.texture = this.EnsureNpcLivePreviewTexture();
			photoAspect.aspectRatio = 1f;
			LocalizedGameText localizedGameText3 = LocalizedGameText.BindSource(captureLabel, "cielos_apps", "TAKE PHOTO", null);
			if (localizedGameText3 != null)
			{
				localizedGameText3.FitTranslationToRectangle(false);
			}
			clearButton.gameObject.SetActive(false);
			LocalizedGameText.BindSource(status, "cielos_apps", string.Empty, null);
		});
		MainMenuButtonFx mainMenuButtonFx = gameObject3.AddComponent<MainMenuButtonFx>();
		mainMenuButtonFx.SetScaleEffectsEnabled(false);
		mainMenuButtonFx.SetTintChildGraphics(false);
		LayoutRebuilder.ForceRebuildLayoutImmediate(transform as RectTransform);
	}

	// Token: 0x06000040 RID: 64 RVA: 0x000037C0 File Offset: 0x000019C0
	private RenderTexture EnsureNpcLivePreviewTexture()
	{
		if (this.npcLivePreviewTexture != null)
		{
			return this.npcLivePreviewTexture;
		}
		int num = 512;
		int num2 = 512;
		this.npcLivePreviewTexture = new RenderTexture(num, num2, 24, RenderTextureFormat.ARGB32, RenderTextureReadWrite.sRGB)
		{
			name = "NPC Creator Live Photo Preview",
			antiAliasing = 1,
			useMipMap = false,
			autoGenerateMips = false
		};
		this.npcLivePreviewTexture.Create();
		return this.npcLivePreviewTexture;
	}

	// Token: 0x06000041 RID: 65 RVA: 0x00003831 File Offset: 0x00001A31
	private IEnumerator RenderNpcLivePreview(RawImage preview)
	{
		RenderTexture target = this.EnsureNpcLivePreviewTexture();
		WaitForSecondsRealtime interval = new WaitForSecondsRealtime(0.1f);
		while (preview != null)
		{
			if (preview.isActiveAndEnabled && preview.texture == target)
			{
				if (this.npcPhotoController == null)
				{
					this.npcPhotoController = Object.FindObjectOfType<NpcCustomizationController>(true);
				}
				if (this.npcPhotoController != null && this.npcPhotoController.RenderPresetPreview(target, false))
				{
					preview.color = Color.white;
				}
			}
			yield return interval;
		}
		this.npcLivePreviewRoutine = null;
		yield break;
	}

	// Token: 0x06000042 RID: 66 RVA: 0x00003848 File Offset: 0x00001A48
	private void OnDestroy()
	{
		if (this.npcLivePreviewRoutine != null)
		{
			base.StopCoroutine(this.npcLivePreviewRoutine);
			this.npcLivePreviewRoutine = null;
		}
		if (this.npcLivePreviewTexture != null)
		{
			this.npcLivePreviewTexture.Release();
			Object.Destroy(this.npcLivePreviewTexture);
			this.npcLivePreviewTexture = null;
		}
	}

	// Token: 0x06000043 RID: 67 RVA: 0x0000389C File Offset: 0x00001A9C
	private static Button CreateNpcPhotoButton(Transform parent, string objectName, string label, Color color, out TextMeshProUGUI labelText)
	{
		GameObject gameObject = new GameObject(objectName, new Type[]
		{
			typeof(RectTransform),
			typeof(CanvasRenderer),
			typeof(Image),
			typeof(Button),
			typeof(LayoutElement)
		});
		gameObject.layer = parent.gameObject.layer;
		gameObject.transform.SetParent(parent, false);
		gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(190f, 42f);
		LayoutElement component = gameObject.GetComponent<LayoutElement>();
		component.preferredWidth = 190f;
		component.preferredHeight = 42f;
		gameObject.GetComponent<Image>().color = color;
		gameObject.AddComponent<CornerRounder>().SetRadii(new Vector4(9f, 9f, 9f, 9f));
		Button component2 = gameObject.GetComponent<Button>();
		ColorBlock colors = component2.colors;
		colors.normalColor = Color.white;
		colors.highlightedColor = new Color(1f, 0.88f, 0.95f, 1f);
		colors.pressedColor = new Color(0.8f, 0.68f, 0.78f, 1f);
		colors.fadeDuration = 0.08f;
		component2.colors = colors;
		labelText = CharacterCreatorManager.CreateNpcPhotoText(gameObject.transform, "Label", label, 13f, 190f, 42f);
		RectTransform rectTransform = labelText.rectTransform;
		rectTransform.anchorMin = Vector2.zero;
		rectTransform.anchorMax = Vector2.one;
		rectTransform.offsetMin = Vector2.zero;
		rectTransform.offsetMax = Vector2.zero;
		labelText.fontStyle = FontStyles.Bold | FontStyles.SmallCaps;
		labelText.alignment = TextAlignmentOptions.Center;
		labelText.raycastTarget = false;
		MainMenuButtonFx mainMenuButtonFx = gameObject.AddComponent<MainMenuButtonFx>();
		mainMenuButtonFx.SetScaleEffectsEnabled(false);
		mainMenuButtonFx.SetTintChildGraphics(false);
		return component2;
	}

	// Token: 0x06000044 RID: 68 RVA: 0x00003A70 File Offset: 0x00001C70
	private void BuildNpcPhotoSection()
	{
		Transform transform = base.transform.Find("Toolbar/Tools/Main_Panel/General_Category/Content/SCROLLVIEW/VIEWPORT/Info_Container");
		if (transform == null || transform.Find("NPC Photo Section") != null)
		{
			return;
		}
		GameObject gameObject = new GameObject("NPC Photo Section", new Type[]
		{
			typeof(RectTransform),
			typeof(VerticalLayoutGroup),
			typeof(LayoutElement)
		});
		gameObject.layer = base.gameObject.layer;
		gameObject.transform.SetParent(transform, false);
		gameObject.transform.SetSiblingIndex(0);
		gameObject.GetComponent<LayoutElement>().preferredHeight = 330f;
		VerticalLayoutGroup component = gameObject.GetComponent<VerticalLayoutGroup>();
		component.spacing = 10f;
		component.childAlignment = TextAnchor.UpperLeft;
		component.childControlWidth = false;
		component.childControlHeight = false;
		component.childForceExpandWidth = false;
		component.childForceExpandHeight = false;
		CharacterCreatorManager.CreateNpcPhotoText(gameObject.transform, "Title", "NPC PICTURE", 22f, 500f, 36f).fontStyle = FontStyles.Bold | FontStyles.SmallCaps;
		GameObject gameObject2 = new GameObject("Photo Frame", new Type[]
		{
			typeof(RectTransform),
			typeof(CanvasRenderer),
			typeof(Image),
			typeof(Button),
			typeof(LayoutElement)
		});
		gameObject2.layer = base.gameObject.layer;
		gameObject2.transform.SetParent(gameObject.transform, false);
		gameObject2.GetComponent<RectTransform>().sizeDelta = new Vector2(230f, 260f);
		LayoutElement component2 = gameObject2.GetComponent<LayoutElement>();
		component2.preferredWidth = 230f;
		component2.preferredHeight = 260f;
		gameObject2.GetComponent<Image>().color = new Color32(56, 35, 70, byte.MaxValue);
		gameObject2.AddComponent<CornerRounder>().SetRadii(new Vector4(14f, 14f, 14f, 14f));
		Outline outline = gameObject2.AddComponent<Outline>();
		outline.effectColor = new Color32(157, 86, 190, byte.MaxValue);
		outline.effectDistance = new Vector2(3f, -3f);
		GameObject gameObject3 = new GameObject("Preview", new Type[]
		{
			typeof(RectTransform),
			typeof(CanvasRenderer),
			typeof(RawImage)
		});
		gameObject3.layer = base.gameObject.layer;
		gameObject3.transform.SetParent(gameObject2.transform, false);
		RectTransform component3 = gameObject3.GetComponent<RectTransform>();
		component3.anchorMin = Vector2.zero;
		component3.anchorMax = Vector2.one;
		component3.offsetMin = new Vector2(8f, 40f);
		component3.offsetMax = new Vector2(-8f, -8f);
		RawImage preview = gameObject3.GetComponent<RawImage>();
		preview.texture = this.iconTexture;
		preview.color = Color.white;
		preview.raycastTarget = false;
		TextMeshProUGUI hint = CharacterCreatorManager.CreateNpcPhotoText(gameObject2.transform, "Hint", "CLICK TO TAKE A PIC", 14f, 212f, 32f);
		RectTransform rectTransform = hint.rectTransform;
		rectTransform.anchorMin = new Vector2(0.5f, 0f);
		rectTransform.anchorMax = new Vector2(0.5f, 0f);
		rectTransform.pivot = new Vector2(0.5f, 0f);
		rectTransform.anchoredPosition = new Vector2(0f, 5f);
		hint.fontStyle = FontStyles.Bold | FontStyles.SmallCaps;
		hint.alignment = TextAlignmentOptions.Center;
		hint.raycastTarget = false;
		gameObject2.GetComponent<Button>().onClick.AddListener(delegate
		{
			NpcCustomizationController npcCustomizationController = Object.FindObjectOfType<NpcCustomizationController>(true);
			Texture2D texture2D = ((npcCustomizationController != null) ? npcCustomizationController.CapturePresetPhoto() : this.GenerateCharacterIcon());
			if (texture2D != null)
			{
				preview.texture = texture2D;
				LocalizedGameText localizedGameText = LocalizedGameText.BindSource(hint, "cielos_apps", "PHOTO CAPTURED · CLICK TO RETAKE", null);
				if (localizedGameText == null)
				{
					return;
				}
				localizedGameText.FitTranslationToRectangle(false);
			}
		});
		MainMenuButtonFx mainMenuButtonFx = gameObject2.AddComponent<MainMenuButtonFx>();
		mainMenuButtonFx.SetScaleEffectsEnabled(false);
		mainMenuButtonFx.SetTintChildGraphics(false);
	}

	// Token: 0x06000045 RID: 69 RVA: 0x00003E70 File Offset: 0x00002070
	private static TextMeshProUGUI CreateNpcPhotoText(Transform parent, string objectName, string value, float fontSize, float width, float height)
	{
		GameObject gameObject = new GameObject(objectName, new Type[]
		{
			typeof(RectTransform),
			typeof(CanvasRenderer),
			typeof(TextMeshProUGUI),
			typeof(LayoutElement)
		});
		gameObject.layer = parent.gameObject.layer;
		gameObject.transform.SetParent(parent, false);
		gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(width, height);
		LayoutElement component = gameObject.GetComponent<LayoutElement>();
		component.preferredWidth = width;
		component.preferredHeight = height;
		TextMeshProUGUI component2 = gameObject.GetComponent<TextMeshProUGUI>();
		component2.fontSize = fontSize;
		component2.color = Color.white;
		component2.alignment = TextAlignmentOptions.Left;
		if (!string.IsNullOrEmpty(value))
		{
			LocalizedGameText localizedGameText = LocalizedGameText.BindSource(component2, "cielos_apps", value, null);
			if (localizedGameText != null)
			{
				localizedGameText.FitTranslationToRectangle(false);
			}
		}
		else
		{
			LocalizedGameText.BindFontOnly(component2);
		}
		return component2;
	}

	// Token: 0x06000046 RID: 70 RVA: 0x00003F54 File Offset: 0x00002154
	private void PolishNpcNavigation()
	{
		foreach (CharacterCreatorManager.CategoryButton categoryButton in this.categoryButtons)
		{
			if (categoryButton != null)
			{
				if (string.Equals(categoryButton.categoryName, "Colors", StringComparison.OrdinalIgnoreCase))
				{
					if (categoryButton.button != null)
					{
						categoryButton.button.gameObject.SetActive(false);
					}
					if (categoryButton.categoryContent != null)
					{
						categoryButton.categoryContent.SetActive(false);
					}
				}
				else
				{
					CharacterCreatorManager.PolishNavigationState(categoryButton.normalState);
					CharacterCreatorManager.PolishNavigationState(categoryButton.selectedState);
				}
			}
		}
	}

	// Token: 0x06000047 RID: 71 RVA: 0x00004008 File Offset: 0x00002208
	private void ResolveSessionOrchestrator()
	{
		if (this.sessionOrchestrator == null)
		{
			this.sessionOrchestrator = Object.FindObjectOfType<CustomizationCoreOrchestrator>(true);
		}
	}

	// Token: 0x06000048 RID: 72 RVA: 0x00004024 File Offset: 0x00002224
	private void HandleSessionInitialized(CustomizationSessionContext context, ICharacterCustomizationBackend backend)
	{
		this.ApplySessionRestrictions(context);
	}

	// Token: 0x06000049 RID: 73 RVA: 0x00004030 File Offset: 0x00002230
	private void ApplySessionRestrictions(CustomizationSessionContext context)
	{
		this.metadataOnlySystemNpc = context != null && context.targetType == CustomizationTargetType.Npc && NpcCustomizationController.IsSystemPresetId(context.presetId);
		for (int i = 0; i < this.categoryButtons.Count; i++)
		{
			CharacterCreatorManager.CategoryButton categoryButton = this.categoryButtons[i];
			if (categoryButton != null)
			{
				bool flag = !string.Equals(categoryButton.categoryName, "Colors", StringComparison.OrdinalIgnoreCase) && (!this.metadataOnlySystemNpc || CharacterCreatorManager.IsMetadataCategory(categoryButton.categoryName));
				if (categoryButton.button != null)
				{
					categoryButton.button.gameObject.SetActive(flag);
					categoryButton.button.interactable = flag;
				}
				if (!flag && categoryButton.categoryContent != null)
				{
					categoryButton.categoryContent.SetActive(false);
				}
			}
		}
		Transform transform = base.transform.Find("Toolbar/Tools/Main_Panel/General_Category/Content/SCROLLVIEW/VIEWPORT/Info_Container/NPC Photo Section");
		if (transform != null)
		{
			transform.gameObject.SetActive(!this.metadataOnlySystemNpc);
		}
		CharacterGeneralInfoManager componentInChildren = base.GetComponentInChildren<CharacterGeneralInfoManager>(true);
		if (componentInChildren != null)
		{
			componentInChildren.SetGenderInteractable(!this.metadataOnlySystemNpc);
		}
		if (this.categoriesInitialized)
		{
			string text = (this.metadataOnlySystemNpc ? "General" : this.defaultCategory);
			if (this.currentCategory == null || (this.metadataOnlySystemNpc && !CharacterCreatorManager.IsMetadataCategory(this.currentCategory.categoryName)) || (this.currentCategory.categoryContent != null && !this.currentCategory.categoryContent.activeSelf))
			{
				this.currentCategory = null;
				this.SelectCategory(text);
			}
		}
	}

	// Token: 0x0600004A RID: 74 RVA: 0x000041C5 File Offset: 0x000023C5
	private static bool IsMetadataCategory(string categoryName)
	{
		return string.Equals(categoryName, "General", StringComparison.OrdinalIgnoreCase) || string.Equals(categoryName, "Advanced", StringComparison.OrdinalIgnoreCase);
	}

	// Token: 0x0600004B RID: 75 RVA: 0x000041E4 File Offset: 0x000023E4
	private static void PolishNavigationState(GameObject stateRoot)
	{
		if (stateRoot == null)
		{
			return;
		}
		foreach (Image image in stateRoot.GetComponentsInChildren<Image>(true))
		{
			if (!(image == null) && !(image.transform == stateRoot.transform) && (image.name.IndexOf("icon", StringComparison.OrdinalIgnoreCase) >= 0 || image.transform.childCount == 0))
			{
				image.color = Color.white;
				image.preserveAspect = true;
				image.raycastTarget = false;
			}
		}
		TMP_Text[] componentsInChildren2 = stateRoot.GetComponentsInChildren<TMP_Text>(true);
		for (int j = 0; j < componentsInChildren2.Length; j++)
		{
			componentsInChildren2[j].color = Color.white;
			componentsInChildren2[j].raycastTarget = false;
		}
	}

	// Token: 0x0600004C RID: 76 RVA: 0x000042A4 File Offset: 0x000024A4
	private void CacheDefaultCharacterState()
	{
		if (this.outfitSystem == null)
		{
			Debug.LogWarning("CharacterCreatorManager: OutfitSystem not assigned, cannot cache default state");
			return;
		}
		CharacterData characterData = BMAC_SaveSystem.GetCharacterData(this.outfitSystem);
		this.defaultCharacterData = this.CloneCharacterData(characterData);
	}

	// Token: 0x0600004D RID: 77 RVA: 0x000042E4 File Offset: 0x000024E4
	private void SetupCamera()
	{
		if (this.cameraController != null && this.defaultCategory != null)
		{
			string text = this.defaultCategory;
			if (text == "Head")
			{
				this.cameraController.tweenPosition(1f);
				return;
			}
			if (text == "Body")
			{
				this.cameraController.tweenPosition(0f);
				return;
			}
			this.cameraController.tweenPosition(0f);
		}
	}

	// Token: 0x0600004E RID: 78 RVA: 0x0000435C File Offset: 0x0000255C
	private void InitializeCategories()
	{
		foreach (CharacterCreatorManager.CategoryButton categoryButton in this.categoryButtons)
		{
			if (((categoryButton != null) ? categoryButton.button : null) != null)
			{
				ControllerShortcutHint.Bind(categoryButton.button.transform.parent, "LB / RB", true);
				break;
			}
		}
		foreach (CharacterCreatorManager.CategoryButton categoryButton2 in this.categoryButtons)
		{
			if (categoryButton2.categoryContent != null)
			{
				categoryButton2.categoryContent.SetActive(false);
			}
			if (categoryButton2.normalState != null)
			{
				categoryButton2.normalState.SetActive(true);
			}
			if (categoryButton2.selectedState != null)
			{
				categoryButton2.selectedState.SetActive(false);
			}
			if (categoryButton2.button != null)
			{
				categoryButton2.button.onClick.RemoveAllListeners();
				CharacterCreatorManager.CategoryButton capturedCategory = categoryButton2;
				categoryButton2.button.onClick.AddListener(delegate
				{
					this.SelectCategory(capturedCategory.categoryName);
				});
			}
		}
		this.SelectCategory(this.metadataOnlySystemNpc ? "General" : this.defaultCategory);
	}

	// Token: 0x0600004F RID: 79 RVA: 0x000044D4 File Offset: 0x000026D4
	public void ResetNavigationForNewSession()
	{
		ColorManager.ClearTemporaryItemSelections();
		if (!this.categoriesInitialized)
		{
			return;
		}
		this.currentCategory = null;
		for (int i = 0; i < this.categoryButtons.Count; i++)
		{
			CharacterCreatorManager.CategoryButton categoryButton = this.categoryButtons[i];
			if (categoryButton != null)
			{
				if (categoryButton.categoryContent != null)
				{
					categoryButton.categoryContent.SetActive(false);
				}
				if (categoryButton.normalState != null)
				{
					categoryButton.normalState.SetActive(true);
				}
				if (categoryButton.selectedState != null)
				{
					categoryButton.selectedState.SetActive(false);
				}
			}
		}
		this.SelectCategory(this.metadataOnlySystemNpc ? "General" : this.defaultCategory);
		this.ResetNpcPhotoUiForSession();
		this.SetupCamera();
	}

	// Token: 0x06000050 RID: 80 RVA: 0x00004594 File Offset: 0x00002794
	private void ResetNpcPhotoUiForSession()
	{
		if (this.npcPhotoPreview == null)
		{
			return;
		}
		this.npcPhotoPreview.texture = this.EnsureNpcLivePreviewTexture();
		this.npcPhotoPreview.color = Color.clear;
		if (this.npcPhotoCaptureLabel != null)
		{
			LocalizedGameText localizedGameText = LocalizedGameText.BindSource(this.npcPhotoCaptureLabel, "cielos_apps", "TAKE PHOTO", null);
			if (localizedGameText != null)
			{
				localizedGameText.FitTranslationToRectangle(false);
			}
		}
		if (this.npcPhotoStatus != null)
		{
			LocalizedGameText.BindSource(this.npcPhotoStatus, "cielos_apps", string.Empty, null);
		}
		if (this.npcPhotoClearButton != null)
		{
			this.npcPhotoClearButton.gameObject.SetActive(false);
		}
		if (this.npcLivePreviewRoutine != null)
		{
			base.StopCoroutine(this.npcLivePreviewRoutine);
		}
		this.npcLivePreviewRoutine = base.StartCoroutine(this.RenderNpcLivePreview(this.npcPhotoPreview));
	}

	// Token: 0x06000051 RID: 81 RVA: 0x00004674 File Offset: 0x00002874
	public void SelectCategory(string categoryName)
	{
		if (string.IsNullOrEmpty(categoryName))
		{
			Debug.LogError("Cannot select null or empty category");
			return;
		}
		if (this.metadataOnlySystemNpc && !CharacterCreatorManager.IsMetadataCategory(categoryName))
		{
			Debug.LogWarning("System NPC appearance category '" + categoryName + "' is locked (merged mesh).", this);
			return;
		}
		CharacterCreatorManager.CategoryButton categoryButton = this.categoryButtons.Find((CharacterCreatorManager.CategoryButton c) => c.categoryName == categoryName);
		if (categoryButton == null)
		{
			Debug.LogWarning("Category " + categoryName + " not found");
			return;
		}
		if (this.currentCategory == categoryButton && categoryButton.categoryContent != null && categoryButton.categoryContent.activeSelf)
		{
			return;
		}
		if (this.currentCategory != null)
		{
			if (this.currentCategory.categoryContent != null)
			{
				this.currentCategory.categoryContent.SetActive(false);
			}
			if (this.currentCategory.normalState != null)
			{
				this.currentCategory.normalState.SetActive(true);
			}
			if (this.currentCategory.selectedState != null)
			{
				this.currentCategory.selectedState.SetActive(false);
			}
		}
		this.currentCategory = categoryButton;
		if (this.currentCategory.categoryContent != null)
		{
			this.currentCategory.categoryContent.SetActive(true);
			CustomizationScrollResetter.ResetToTopNextFrame(this, this.currentCategory.categoryContent.transform);
		}
		if (this.currentCategory.normalState != null)
		{
			this.currentCategory.normalState.SetActive(false);
		}
		if (this.currentCategory.selectedState != null)
		{
			this.currentCategory.selectedState.SetActive(true);
		}
		Debug.Log("Selected category: " + categoryName);
	}

	// Token: 0x06000052 RID: 82 RVA: 0x00004844 File Offset: 0x00002A44
	public string GetCurrentCategoryName()
	{
		if (this.currentCategory == null)
		{
			return string.Empty;
		}
		return this.currentCategory.categoryName;
	}

	// Token: 0x06000053 RID: 83 RVA: 0x00004860 File Offset: 0x00002A60
	public async void ResetCharacter()
	{
		if (this.outfitSystem == null)
		{
			Debug.LogWarning("CharacterCreatorManager: OutfitSystem not assigned, cannot reset character");
		}
		else
		{
			if (this.defaultCharacterData == null)
			{
				this.CacheDefaultCharacterState();
				if (this.defaultCharacterData == null)
				{
					Debug.LogWarning("CharacterCreatorManager: Default character data unavailable, aborting reset");
					return;
				}
			}
			CharacterData characterData = this.CloneCharacterData(this.defaultCharacterData);
			try
			{
				await BMAC_SaveSystem.LoadCharacter(this.outfitSystem, characterData, false, true);
				Debug.Log("Character reset - default template reapplied");
			}
			catch (Exception ex)
			{
				Debug.LogError("Character reset failed: " + ex.Message);
			}
		}
	}

	// Token: 0x06000054 RID: 84 RVA: 0x00004897 File Offset: 0x00002A97
	private CharacterData CloneCharacterData(CharacterData source)
	{
		if (source == null)
		{
			return null;
		}
		return JsonUtility.FromJson<CharacterData>(JsonUtility.ToJson(source));
	}

	// Token: 0x06000055 RID: 85 RVA: 0x000048AC File Offset: 0x00002AAC
	public Texture2D GenerateCharacterIcon()
	{
		if (this.iconCamera == null || this.iconTexture == null)
		{
			Debug.LogWarning("Icon camera or render texture not assigned - skipping icon generation");
			return null;
		}
		Texture2D texture2D2;
		try
		{
			Debug.Log("=== ICON CAMERA DEBUG ===");
			Debug.Log(string.Format("Camera Position: {0}", this.iconCamera.transform.position));
			Debug.Log(string.Format("Camera Rotation: {0}", this.iconCamera.transform.rotation.eulerAngles));
			Debug.Log(string.Format("Camera FOV: {0}", this.iconCamera.fieldOfView));
			Debug.Log(string.Format("Camera Clear Flags: {0}", this.iconCamera.clearFlags));
			Debug.Log(string.Format("Camera Background: {0}", this.iconCamera.backgroundColor));
			Debug.Log(string.Format("Camera Culling Mask: {0}", this.iconCamera.cullingMask));
			Debug.Log(string.Format("RenderTexture Size: {0}x{1}", this.iconTexture.width, this.iconTexture.height));
			RenderTexture targetTexture = this.iconCamera.targetTexture;
			this.iconCamera.targetTexture = this.iconTexture;
			this.iconCamera.Render();
			RenderTexture.active = this.iconTexture;
			Texture2D texture2D = new Texture2D(this.iconWidth, this.iconHeight, TextureFormat.RGBA32, false);
			Rect rect = new Rect(0f, 0f, (float)this.iconWidth, (float)this.iconHeight);
			texture2D.ReadPixels(rect, 0, 0);
			texture2D.Apply();
			RenderTexture.active = null;
			this.iconCamera.targetTexture = targetTexture;
			Debug.Log(string.Format("Generated character icon: {0}x{1}", texture2D.width, texture2D.height));
			Debug.Log("=== ICON GENERATION COMPLETE ===");
			texture2D2 = texture2D;
		}
		catch (Exception ex)
		{
			Debug.LogError("Failed to generate character icon: " + ex.Message);
			texture2D2 = null;
		}
		return texture2D2;
	}

	// Token: 0x06000056 RID: 86 RVA: 0x00004AE0 File Offset: 0x00002CE0
	[ContextMenu("Auto Configure Icon Camera")]
	public void AutoConfigureIconCamera()
	{
		if (this.iconCamera == null)
		{
			Debug.LogError("Icon Camera not assigned!");
			return;
		}
		if (this.outfitSystem == null)
		{
			Debug.LogError("Outfit System not assigned!");
			return;
		}
		Debug.Log("=== AUTO CONFIGURING ICON CAMERA ===");
		Vector3 vector = this.outfitSystem.transform.position + Vector3.up * 1.6f;
		Vector3 vector2 = new Vector3(0f, 0.2f, -2.5f);
		this.iconCamera.transform.position = vector + vector2;
		this.iconCamera.transform.LookAt(vector);
		this.iconCamera.fieldOfView = 25f;
		this.iconCamera.clearFlags = CameraClearFlags.Color;
		this.iconCamera.backgroundColor = new Color(0f, 0f, 0f, 0f);
		if (this.iconCamera.cullingMask == 0)
		{
			this.iconCamera.cullingMask = -1;
		}
		Debug.Log(string.Format("Camera positioned at: {0}", this.iconCamera.transform.position));
		Debug.Log(string.Format("Camera looking at: {0}", vector));
		Debug.Log(string.Format("Camera FOV set to: {0}", this.iconCamera.fieldOfView));
		Debug.Log("=== AUTO CONFIGURATION COMPLETE ===");
	}

	// Token: 0x06000057 RID: 87 RVA: 0x00004C48 File Offset: 0x00002E48
	[ContextMenu("Test Camera View")]
	public void TestCameraView()
	{
		if (this.iconCamera == null)
		{
			Debug.LogError("Icon Camera not assigned!");
			return;
		}
		Camera main = Camera.main;
		if (main != null)
		{
			main.enabled = false;
			this.iconCamera.enabled = true;
			Debug.Log("Icon camera is now active - you can see what it's looking at");
			Debug.Log("Use 'Reset Camera View' to return to normal view");
		}
	}

	// Token: 0x06000058 RID: 88 RVA: 0x00004CA4 File Offset: 0x00002EA4
	[ContextMenu("Reset Camera View")]
	public void ResetCameraView()
	{
		Camera main = Camera.main;
		if (main != null)
		{
			main.enabled = true;
		}
		if (this.iconCamera != null)
		{
			this.iconCamera.enabled = false;
		}
		Debug.Log("Camera view reset to normal");
	}

	// Token: 0x04000010 RID: 16
	[Header("Categorie")]
	[SerializeField]
	private List<CharacterCreatorManager.CategoryButton> categoryButtons = new List<CharacterCreatorManager.CategoryButton>();

	// Token: 0x04000011 RID: 17
	[SerializeField]
	private CameraController cameraController;

	// Token: 0x04000012 RID: 18
	[SerializeField]
	private string defaultCategory = "Head";

	// Token: 0x04000013 RID: 19
	[Header("Character System")]
	[SerializeField]
	private OutfitSystem outfitSystem;

	// Token: 0x04000014 RID: 20
	[SerializeField]
	private Camera iconCamera;

	// Token: 0x04000015 RID: 21
	[SerializeField]
	private RenderTexture iconTexture;

	// Token: 0x04000016 RID: 22
	[SerializeField]
	private int iconWidth = 256;

	// Token: 0x04000017 RID: 23
	[SerializeField]
	private int iconHeight = 256;

	// Token: 0x04000018 RID: 24
	private CharacterCreatorManager.CategoryButton currentCategory;

	// Token: 0x04000019 RID: 25
	private CharacterData defaultCharacterData;

	// Token: 0x0400001A RID: 26
	private RenderTexture npcLivePreviewTexture;

	// Token: 0x0400001B RID: 27
	private NpcCustomizationController npcPhotoController;

	// Token: 0x0400001C RID: 28
	private Coroutine npcLivePreviewRoutine;

	// Token: 0x0400001D RID: 29
	private RawImage npcPhotoPreview;

	// Token: 0x0400001E RID: 30
	private TextMeshProUGUI npcPhotoCaptureLabel;

	// Token: 0x0400001F RID: 31
	private TextMeshProUGUI npcPhotoStatus;

	// Token: 0x04000020 RID: 32
	private Button npcPhotoClearButton;

	// Token: 0x04000021 RID: 33
	private bool categoriesInitialized;

	// Token: 0x04000022 RID: 34
	private CustomizationCoreOrchestrator sessionOrchestrator;

	// Token: 0x04000023 RID: 35
	private bool metadataOnlySystemNpc;

	// Token: 0x02000008 RID: 8
	[Serializable]
	public class CategoryButton
	{
		// Token: 0x04000024 RID: 36
		public string categoryName;

		// Token: 0x04000025 RID: 37
		public GameObject categoryContent;

		// Token: 0x04000026 RID: 38
		public Button button;

		// Token: 0x04000027 RID: 39
		public GameObject normalState;

		// Token: 0x04000028 RID: 40
		public GameObject selectedState;
	}
}
