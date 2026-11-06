using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Bozo.AnimeCharacters;
using BoZo.CharacterCreator.MC.StyleSystem;
using BoZo.CharacterCreator.MC.StyleSystem.UI;
using ProjectExhibitionist;
using ProjectExhibitionist.CharacterCustomization.Core;
using ProjectExhibitionist.CharacterCustomization.Core.Presets;
using ProjectExhibitionist.CharacterCustomization.Core.Runtime;
using ProjectExhibitionist.Core;
using ProjectExhibitionist.Core.Rendering;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace BoZo.CharacterCreator.MC
{
	// Token: 0x020007C2 RID: 1986
	public class MCCustomizationController : MonoBehaviour
	{
		// Token: 0x170009AE RID: 2478
		// (get) Token: 0x0600349D RID: 13469 RVA: 0x0012BF91 File Offset: 0x0012A191
		public RenderTexture IconTexture
		{
			get
			{
				return this.iconTexture;
			}
		}

		// Token: 0x0600349E RID: 13470 RVA: 0x0012BF9C File Offset: 0x0012A19C
		private void Start()
		{
			this.activePreset = new MCPresetData();
			if (this.generalInfoController != null)
			{
				this.generalInfoController.EnsureInitialized();
			}
			this.BindContextualCameraEvents();
			this.InitializeCategories();
			this.SetupDefaultCamera();
			if (this.backToHubButton != null)
			{
				this.backToHubButton.onClick.RemoveAllListeners();
				this.backToHubButton.onClick.AddListener(new UnityAction(this.OnBackToHub));
			}
			if (this.clothingToggleButton != null)
			{
				this.clothingToggleButton.onClick.RemoveAllListeners();
				this.clothingToggleButton.onClick.AddListener(new UnityAction(this.ToggleNakedMC));
				this.BindClothingToggleVisual(this.clothingToggleButton);
			}
			if (this.navigationRouter == null)
			{
				this.navigationRouter = Object.FindObjectOfType<CustomizationNavigationRouter>(true);
			}
			if (this.charactersManager == null)
			{
				this.charactersManager = Object.FindObjectOfType<CharactersManager>(true);
			}
			if (this.coreOrchestrator == null)
			{
				this.coreOrchestrator = Object.FindObjectOfType<CustomizationCoreOrchestrator>(true);
			}
			if (this.coreOrchestrator != null)
			{
				this.coreOrchestrator.OnSessionInitialized -= this.HandleSessionInitialized;
				this.coreOrchestrator.OnSessionInitialized += this.HandleSessionInitialized;
				this.coreOrchestrator.OnSessionClosed -= this.HandleSessionClosed;
				this.coreOrchestrator.OnSessionClosed += this.HandleSessionClosed;
			}
			this.ConfigureStateManagerForMainCharacterContext();
			if (this.generalInfoController != null)
			{
				this.generalInfoController.OnCapturePhotoRequested += this.CapturePresetPhotoFromCurrentCamera;
				this.generalInfoController.OnRemovePhotoRequested += this.HandleRemoveCapturedPhoto;
				this.generalInfoController.SetLivePhotoPreviewTexture(this.iconTexture);
			}
			this.UpdateEditingPresetFromSession();
		}

		// Token: 0x0600349F RID: 13471 RVA: 0x0012C170 File Offset: 0x0012A370
		private void OnEnable()
		{
			if (this.iconCamera != null)
			{
				this._iconCameraWasEnabled = this.iconCamera.enabled;
				this._ownsIconCamera = true;
				this.iconCamera.enabled = false;
			}
			string text = ((this.currentCategory != null) ? this.currentCategory.categoryName : this.defaultCategory);
			this.UpdateCameraForCategory(text);
			if (this.generalInfoController != null)
			{
				this.generalInfoController.SetLivePhotoPreviewTexture(this.iconTexture);
			}
			this.UpdateEditingPresetFromSession();
			this.UpdateIconCameraFromFollowTarget();
			this.ConfigureStateManagerForMainCharacterContext();
		}

		// Token: 0x060034A0 RID: 13472 RVA: 0x0012C203 File Offset: 0x0012A403
		public void ToggleWalkMC()
		{
			this.SetWalkMC(!this._isWalkingPreview);
		}

		// Token: 0x060034A1 RID: 13473 RVA: 0x0012C214 File Offset: 0x0012A414
		public void SetWalkMC(bool walking)
		{
			Animator animator = this.ResolvePreviewAnimator();
			if (animator == null)
			{
				Debug.LogWarning("[MCCustomization] SetWalkMC: nessun Animator preview risolto.", this);
				return;
			}
			this._isWalkingPreview = walking;
			animator.SetFloat("InputMagnitude", walking ? this.walkInputMagnitude : 0f);
		}

		// Token: 0x060034A2 RID: 13474 RVA: 0x0012C25F File Offset: 0x0012A45F
		private Animator ResolvePreviewAnimator()
		{
			if (this.previewAnimator != null)
			{
				return this.previewAnimator;
			}
			if (this.blendShapeController != null)
			{
				this.previewAnimator = this.blendShapeController.GetComponentInParent<Animator>();
			}
			return this.previewAnimator;
		}

		// Token: 0x060034A3 RID: 13475 RVA: 0x0012C29C File Offset: 0x0012A49C
		public void ToggleNakedMC()
		{
			MCStyleCustomizationService mcstyleCustomizationService = this.ResolveStyleService();
			if (mcstyleCustomizationService != null)
			{
				mcstyleCustomizationService.SetPreviewClothingHidden(!mcstyleCustomizationService.IsPreviewClothingHidden);
				return;
			}
			ClothingToggleController clothingToggleController = this.ResolveClothingToggleController();
			if (clothingToggleController != null)
			{
				clothingToggleController.ToggleAllClothing();
				return;
			}
			Debug.LogWarning("[MCCustomization] ToggleNakedMC: né ClothingToggleController né MCStyleCustomizationService risolti sul preview.", this);
		}

		// Token: 0x060034A4 RID: 13476 RVA: 0x0012C2EB File Offset: 0x0012A4EB
		public void BindClothingToggleVisual(Button button)
		{
			if (button == null)
			{
				return;
			}
			(button.GetComponent<MCPreviewClothingToggleUI>() ?? button.gameObject.AddComponent<MCPreviewClothingToggleUI>()).Bind(this.ResolveStyleService());
		}

		// Token: 0x060034A5 RID: 13477 RVA: 0x0012C318 File Offset: 0x0012A518
		private ClothingToggleController ResolveClothingToggleController()
		{
			if (this._clothingToggleController == null)
			{
				MCStyleCustomizationService mcstyleCustomizationService = this.ResolveStyleService();
				if (mcstyleCustomizationService != null)
				{
					this._clothingToggleController = mcstyleCustomizationService.GetComponentInParent<ClothingToggleController>();
				}
				if (this._clothingToggleController == null)
				{
					this._clothingToggleController = Object.FindObjectOfType<ClothingToggleController>(false);
				}
			}
			return this._clothingToggleController;
		}

		// Token: 0x060034A6 RID: 13478 RVA: 0x0012C36F File Offset: 0x0012A56F
		private MCStyleCustomizationService ResolveStyleService()
		{
			if (this.styleCustomizationService == null)
			{
				this.styleCustomizationService = base.GetComponentInChildren<MCStyleCustomizationService>(true) ?? Object.FindObjectOfType<MCStyleCustomizationService>(true);
			}
			return this.styleCustomizationService;
		}

		// Token: 0x060034A7 RID: 13479 RVA: 0x0012C39C File Offset: 0x0012A59C
		private void LateUpdate()
		{
			this.RenderLiveIconPreviewIfNeeded();
			if (!this.hasAppliedEditingContext)
			{
				this.UpdateEditingPresetFromSession();
			}
		}

		// Token: 0x060034A8 RID: 13480 RVA: 0x0012C3B4 File Offset: 0x0012A5B4
		private void OnDestroy()
		{
			if (this.generalInfoController != null)
			{
				this.generalInfoController.OnCapturePhotoRequested -= this.CapturePresetPhotoFromCurrentCamera;
				this.generalInfoController.OnRemovePhotoRequested -= this.HandleRemoveCapturedPhoto;
			}
			if (this.coreOrchestrator != null)
			{
				this.coreOrchestrator.OnSessionInitialized -= this.HandleSessionInitialized;
				this.coreOrchestrator.OnSessionClosed -= this.HandleSessionClosed;
			}
			this.UnbindContextualCameraEvents();
			this.ClearCapturedPresetPhoto();
		}

		// Token: 0x060034A9 RID: 13481 RVA: 0x0012C448 File Offset: 0x0012A648
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
				MCCustomizationController.CategoryButton categoryButton = this.categoryButtons[num2];
				if (categoryButton != null && !(categoryButton.button == null) && categoryButton.button.isActiveAndEnabled && categoryButton.button.IsInteractable() && (!this.hideLegacyColorsCategory || !string.Equals(categoryButton.categoryName, "Colors", StringComparison.OrdinalIgnoreCase)))
				{
					categoryButton.button.onClick.Invoke();
					return categoryButton.button;
				}
			}
			return null;
		}

		// Token: 0x060034AA RID: 13482 RVA: 0x0012C52C File Offset: 0x0012A72C
		public Button ControllerStepSubcategory(int direction)
		{
			SubCategorySelector subCategorySelector = ((this.currentCategory != null && this.currentCategory.categoryContent != null) ? this.currentCategory.categoryContent.GetComponentInChildren<SubCategorySelector>() : null);
			if (!(subCategorySelector != null))
			{
				return null;
			}
			return subCategorySelector.ControllerStep(direction);
		}

		// Token: 0x060034AB RID: 13483 RVA: 0x0012C57C File Offset: 0x0012A77C
		private void InitializeCategories()
		{
			foreach (MCCustomizationController.CategoryButton categoryButton in this.categoryButtons)
			{
				if (((categoryButton != null) ? categoryButton.button : null) != null)
				{
					ControllerShortcutHint.Bind(categoryButton.button.transform.parent, "LB / RB", true);
					break;
				}
			}
			foreach (MCCustomizationController.CategoryButton categoryButton2 in this.categoryButtons)
			{
				bool flag = string.Equals(categoryButton2.categoryName, "Colors", StringComparison.OrdinalIgnoreCase);
				bool flag2 = this.hideLegacyColorsCategory && flag;
				if (categoryButton2.button != null)
				{
					categoryButton2.button.gameObject.SetActive(!flag2);
				}
				if (flag2)
				{
					if (categoryButton2.categoryContent != null)
					{
						categoryButton2.categoryContent.SetActive(false);
					}
				}
				else
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
						MCCustomizationController.CategoryButton captured = categoryButton2;
						categoryButton2.button.onClick.AddListener(delegate
						{
							this.SelectCategory(captured.categoryName);
						});
					}
				}
			}
			if (this.hideLegacyColorsCategory && string.Equals(this.defaultCategory, "Colors", StringComparison.OrdinalIgnoreCase))
			{
				this.defaultCategory = this.ResolveFirstVisibleCategory(this.defaultCategory);
			}
			this.SelectCategory(this.defaultCategory);
		}

		// Token: 0x060034AC RID: 13484 RVA: 0x0012C790 File Offset: 0x0012A990
		private string ResolveFirstVisibleCategory(string fallback)
		{
			for (int i = 0; i < this.categoryButtons.Count; i++)
			{
				MCCustomizationController.CategoryButton categoryButton = this.categoryButtons[i];
				if (categoryButton != null && !string.IsNullOrWhiteSpace(categoryButton.categoryName) && (!this.hideLegacyColorsCategory || !string.Equals(categoryButton.categoryName, "Colors", StringComparison.OrdinalIgnoreCase)))
				{
					return categoryButton.categoryName;
				}
			}
			return fallback;
		}

		// Token: 0x060034AD RID: 13485 RVA: 0x0012C7F4 File Offset: 0x0012A9F4
		public void SelectCategory(string categoryName)
		{
			if (string.IsNullOrEmpty(categoryName))
			{
				Debug.LogError("[MCCustomization] Cannot select null or empty category");
				return;
			}
			MCCustomizationController.CategoryButton categoryButton = this.categoryButtons.Find((MCCustomizationController.CategoryButton c) => c.categoryName == categoryName);
			if (categoryButton == null)
			{
				Debug.LogWarning("[MCCustomization] Category '" + categoryName + "' not found");
				return;
			}
			if (this.currentCategory == categoryButton)
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
			}
			if (this.currentCategory.normalState != null)
			{
				this.currentCategory.normalState.SetActive(false);
			}
			if (this.currentCategory.selectedState != null)
			{
				this.currentCategory.selectedState.SetActive(true);
			}
			this.UpdateCameraForCategory(categoryName);
			this.ApplyCurrentSubcategoryCamera(this.currentCategory.categoryContent);
			Debug.Log("[MCCustomization] Selected category: " + categoryName);
		}

		// Token: 0x060034AE RID: 13486 RVA: 0x0012C97F File Offset: 0x0012AB7F
		public string GetCurrentCategoryName()
		{
			if (this.currentCategory == null)
			{
				return string.Empty;
			}
			return this.currentCategory.categoryName;
		}

		// Token: 0x060034AF RID: 13487 RVA: 0x0012C99A File Offset: 0x0012AB9A
		private void SetupDefaultCamera()
		{
			this.UpdateCameraForCategory(this.defaultCategory);
		}

		// Token: 0x060034B0 RID: 13488 RVA: 0x0012C9A8 File Offset: 0x0012ABA8
		private void UpdateCameraForCategory(string categoryName)
		{
			if (this.cameraController == null)
			{
				return;
			}
			if (categoryName == "Head")
			{
				this.cameraController.tweenPosition(1f);
				return;
			}
			if (!(categoryName == "Body"))
			{
				this.cameraController.tweenPosition(0f);
				return;
			}
			this.cameraController.tweenPosition(0f);
		}

		// Token: 0x060034B1 RID: 13489 RVA: 0x0012CA14 File Offset: 0x0012AC14
		private void BindContextualCameraEvents()
		{
			this.UnbindContextualCameraEvents();
			for (int i = 0; i < this.categoryButtons.Count; i++)
			{
				GameObject gameObject = ((this.categoryButtons[i] != null) ? this.categoryButtons[i].categoryContent : null);
				if (!(gameObject == null))
				{
					foreach (CharacterCustomizationController characterCustomizationController in gameObject.GetComponentsInChildren<CharacterCustomizationController>(true))
					{
						if (!(characterCustomizationController == null) && !this._contextualCameraSources.Contains(characterCustomizationController))
						{
							characterCustomizationController.OnSubcategoryChanged += this.HandleContextualSubcategoryChanged;
							this._contextualCameraSources.Add(characterCustomizationController);
						}
					}
				}
			}
		}

		// Token: 0x060034B2 RID: 13490 RVA: 0x0012CAC4 File Offset: 0x0012ACC4
		private void UnbindContextualCameraEvents()
		{
			for (int i = 0; i < this._contextualCameraSources.Count; i++)
			{
				CharacterCustomizationController characterCustomizationController = this._contextualCameraSources[i];
				if (characterCustomizationController != null)
				{
					characterCustomizationController.OnSubcategoryChanged -= this.HandleContextualSubcategoryChanged;
				}
			}
			this._contextualCameraSources.Clear();
		}

		// Token: 0x060034B3 RID: 13491 RVA: 0x0012CB1C File Offset: 0x0012AD1C
		private void ApplyCurrentSubcategoryCamera(GameObject categoryContent)
		{
			if (categoryContent == null)
			{
				return;
			}
			CharacterCustomizationController componentInChildren = categoryContent.GetComponentInChildren<CharacterCustomizationController>(true);
			if (componentInChildren != null)
			{
				this.HandleContextualSubcategoryChanged(componentInChildren.GetCurrentSubCategory());
			}
		}

		// Token: 0x060034B4 RID: 13492 RVA: 0x0012CB50 File Offset: 0x0012AD50
		private void HandleContextualSubcategoryChanged(CharacterSubCategorySO subCategory)
		{
			if (this.cameraController == null || subCategory == null || subCategory.cameraPositionIndex < 0f)
			{
				return;
			}
			this.cameraController.TweenToContext(subCategory.cameraPositionIndex, subCategory.cameraVerticalOffset);
		}

		// Token: 0x060034B5 RID: 13493 RVA: 0x0012CB90 File Offset: 0x0012AD90
		public Texture2D GenerateCharacterIcon()
		{
			if (this.iconCamera == null || this.iconTexture == null)
			{
				Debug.LogWarning("[MCCustomization] Icon camera or render texture not assigned");
				return null;
			}
			RenderTexture renderTexture = null;
			RenderTexture renderTexture2 = null;
			RenderTexture renderTexture3 = null;
			Texture2D texture2D2;
			try
			{
				this.UpdateIconCameraFromFollowTarget();
				int num = ((this.iconWidth > 0) ? this.iconWidth : this.iconTexture.width);
				int num2 = ((this.iconHeight > 0) ? this.iconHeight : this.iconTexture.height);
				renderTexture = this.iconCamera.targetTexture;
				renderTexture2 = RenderTexture.active;
				renderTexture3 = RenderTexture.GetTemporary(num, num2, 24, RenderTextureFormat.ARGB32, RenderTextureReadWrite.sRGB);
				SkinDecalController skinDecalController = this.skinDecalController;
				if (skinDecalController != null)
				{
					skinDecalController.SynchronizeMeshesForCapture();
				}
				CharacterPreviewRendering.RenderPortrait(this.iconCamera, renderTexture3, false);
				RenderTexture.active = renderTexture3;
				Texture2D texture2D = new Texture2D(num, num2, TextureFormat.RGBA32, false);
				texture2D.ReadPixels(new Rect(0f, 0f, (float)num, (float)num2), 0, 0);
				texture2D.Apply();
				texture2D2 = texture2D;
			}
			catch (Exception ex)
			{
				Debug.LogError("[MCCustomization] Icon generation failed: " + ex.Message);
				texture2D2 = null;
			}
			finally
			{
				this.iconCamera.targetTexture = renderTexture;
				RenderTexture.active = renderTexture2;
				if (renderTexture3 != null)
				{
					RenderTexture.ReleaseTemporary(renderTexture3);
				}
			}
			return texture2D2;
		}

		// Token: 0x060034B6 RID: 13494 RVA: 0x0012CCDC File Offset: 0x0012AEDC
		public void SavePreset()
		{
			if (this.activePreset == null)
			{
				this.activePreset = new MCPresetData();
			}
			this.CollectCurrentState(this.activePreset);
			string presetFolderPath = this.GetPresetFolderPath();
			if (!Directory.Exists(presetFolderPath))
			{
				Directory.CreateDirectory(presetFolderPath);
			}
			string text = MCCustomizationController.SanitizeFileName(this.activePreset.presetName) + ".json";
			string text2 = Path.Combine(presetFolderPath, text);
			string text3 = JsonUtility.ToJson(this.activePreset, true);
			File.WriteAllText(text2, text3);
			this.activePresetPath = text2;
			Debug.Log("[MCCustomization] Preset saved to: " + text2);
		}

		// Token: 0x060034B7 RID: 13495 RVA: 0x0012CD6C File Offset: 0x0012AF6C
		public string GetCurrentPresetName()
		{
			if (this.generalInfoController != null)
			{
				return this.generalInfoController.GetPresetName();
			}
			if (this.activePreset != null)
			{
				return this.activePreset.presetName;
			}
			return string.Empty;
		}

		// Token: 0x060034B8 RID: 13496 RVA: 0x0012CDA1 File Offset: 0x0012AFA1
		public void RequestSavePreset(Action<bool, string> onCompleted)
		{
			if (!base.isActiveAndEnabled || this._presetSaveRoutine != null)
			{
				return;
			}
			this._presetSaveRoutine = base.StartCoroutine(this.SavePresetAtEndOfFrame(onCompleted));
		}

		// Token: 0x060034B9 RID: 13497 RVA: 0x0012CDC7 File Offset: 0x0012AFC7
		private IEnumerator SavePresetAtEndOfFrame(Action<bool, string> onCompleted)
		{
			yield return MCCustomizationController.LiveIconEndOfFrame;
			this._presetSaveRoutine = null;
			if (!base.isActiveAndEnabled)
			{
				yield break;
			}
			string text;
			bool flag = this.TrySavePreset(out text);
			if (onCompleted != null)
			{
				onCompleted(flag, text);
			}
			yield break;
		}

		// Token: 0x060034BA RID: 13498 RVA: 0x0012CDE0 File Offset: 0x0012AFE0
		public bool TrySavePreset(out string error)
		{
			error = null;
			if (this.activePreset == null)
			{
				this.activePreset = new MCPresetData();
			}
			this.CollectCurrentState(this.activePreset);
			if (string.IsNullOrWhiteSpace(this.activePreset.presetName))
			{
				error = "Please enter a preset name before saving.";
				return false;
			}
			if (this.charactersManager == null)
			{
				this.charactersManager = Object.FindObjectOfType<CharactersManager>(true);
			}
			if (this.charactersManager == null)
			{
				error = "CharactersManager missing in scene.";
				return false;
			}
			CharacterStateManager instance = CharacterStateManager.Instance;
			if (instance == null)
			{
				error = "CharacterStateManager missing in scene.";
				return false;
			}
			this.charactersManager.EnsureMainCharacterExists();
			CharacterDataExtended mainCharacter = this.charactersManager.GetMainCharacter();
			string text = ((mainCharacter != null) ? mainCharacter.characterId : string.Empty);
			if (string.IsNullOrWhiteSpace(text))
			{
				error = "Unable to resolve target character for preset save.";
				return false;
			}
			CharacterStateData characterStateData = instance.ConvertToCharacterStateData();
			MCGeneralInfoController mcgeneralInfoController = this.generalInfoController;
			if (mcgeneralInfoController != null)
			{
				mcgeneralInfoController.CaptureState(characterStateData);
			}
			this.OverrideBlendShapesFromMainCharacterController(characterStateData);
			if (this.colorCustomizationService != null)
			{
				this.colorCustomizationService.CaptureState(characterStateData);
			}
			if (this.styleCustomizationService != null)
			{
				this.styleCustomizationService.CaptureState(characterStateData);
			}
			this.CaptureSkinDecalState(characterStateData);
			string text2 = null;
			if (this.capturedPresetPhoto != null)
			{
				text2 = this.SavePresetIconAsPng(this.activePreset.presetName, this.capturedPresetPhoto);
			}
			else if (this.photoRemovedByUser)
			{
				text2 = string.Empty;
			}
			string text3 = this.ResolveEditingPresetId();
			bool flag = !string.IsNullOrWhiteSpace(text3) && !text3.StartsWith("sys_", StringComparison.OrdinalIgnoreCase);
			if (text2 == null && !this.HasExistingPresetIcon(text3, text))
			{
				Texture2D texture2D = this.GenerateCharacterIcon();
				if (texture2D != null)
				{
					text2 = this.SavePresetIconAsPng(this.activePreset.presetName, texture2D);
					Object.Destroy(texture2D);
				}
			}
			if (flag)
			{
				CustomizationPresetData customizationPresetData;
				string text4;
				if (!this.charactersManager.TryUpdatePresetForCharacter(text, text3, this.activePreset.presetName, characterStateData, text2, out customizationPresetData, out text4) || customizationPresetData == null)
				{
					error = (string.IsNullOrWhiteSpace(text4) ? "Failed to update existing preset." : text4);
					return false;
				}
				this.editingPresetId = customizationPresetData.presetId;
				if (customizationPresetData.isDefault || customizationPresetData.isActive)
				{
					MainCharacterAppearanceEvents.RaiseInUsePresetChanged();
				}
				return true;
			}
			else
			{
				CustomizationPresetData customizationPresetData2;
				string text5;
				if (!this.charactersManager.TryCreatePresetForCharacter(text, this.activePreset.presetName, out customizationPresetData2, out text5, characterStateData, text2) || customizationPresetData2 == null)
				{
					error = (string.IsNullOrWhiteSpace(text5) ? "Failed to persist preset into customization repository." : text5);
					return false;
				}
				this.editingPresetId = customizationPresetData2.presetId;
				return true;
			}
		}

		// Token: 0x060034BB RID: 13499 RVA: 0x0012D050 File Offset: 0x0012B250
		private void OverrideBlendShapesFromMainCharacterController(CharacterStateData stateData)
		{
			if (stateData == null || this.blendShapeController == null || this.blendShapeController.BlendShapeMap == null)
			{
				return;
			}
			if (stateData.blendShapes == null)
			{
				stateData.blendShapes = new List<BlendShapeValue>();
			}
			else
			{
				stateData.blendShapes.Clear();
			}
			List<BlendShapeMapSO.BlendShapeEntry> blendShapeMappings = this.blendShapeController.BlendShapeMap.blendShapeMappings;
			if (blendShapeMappings == null || blendShapeMappings.Count == 0)
			{
				return;
			}
			for (int i = 0; i < blendShapeMappings.Count; i++)
			{
				BlendShapeMapSO.BlendShapeEntry blendShapeEntry = blendShapeMappings[i];
				if (blendShapeEntry != null && !string.IsNullOrWhiteSpace(blendShapeEntry.displayName))
				{
					float entryAuthoredUiValue = this.blendShapeController.GetEntryAuthoredUiValue(blendShapeEntry);
					stateData.blendShapes.Add(new BlendShapeValue(blendShapeEntry.category, blendShapeEntry.subcategory, blendShapeEntry.displayName, entryAuthoredUiValue));
				}
			}
		}

		// Token: 0x060034BC RID: 13500 RVA: 0x0012D118 File Offset: 0x0012B318
		private void ConfigureStateManagerForMainCharacterContext()
		{
			CharacterStateManager instance = CharacterStateManager.Instance;
			if (instance == null)
			{
				return;
			}
			instance.SetActiveContext(CustomizationTargetType.MainCharacter);
			if (this.blendShapeController != null)
			{
				instance.ConfigureBlendShapeControllerForContext(CustomizationTargetType.MainCharacter, this.blendShapeController);
			}
		}

		// Token: 0x060034BD RID: 13501 RVA: 0x0012D158 File Offset: 0x0012B358
		private bool HasExistingPresetIcon(string presetId, string characterId)
		{
			if (string.IsNullOrWhiteSpace(presetId) || this.charactersManager == null)
			{
				return false;
			}
			List<CustomizationPresetData> presetsForCharacter = this.charactersManager.GetPresetsForCharacter(characterId);
			for (int i = 0; i < presetsForCharacter.Count; i++)
			{
				CustomizationPresetData customizationPresetData = presetsForCharacter[i];
				if (customizationPresetData != null && customizationPresetData.presetId == presetId)
				{
					return !string.IsNullOrWhiteSpace(customizationPresetData.iconFileName);
				}
			}
			return false;
		}

		// Token: 0x060034BE RID: 13502 RVA: 0x0012D1C4 File Offset: 0x0012B3C4
		private string SavePresetIconAsPng(string presetName, Texture2D icon)
		{
			if (icon == null)
			{
				return null;
			}
			string text4;
			try
			{
				GameDataPaths.EnsureCustomizationDirectories();
				string text = MCCustomizationController.SanitizeFileName(string.IsNullOrWhiteSpace(presetName) ? "preset" : presetName);
				string text2 = string.Format("{0}_{1:N}.png", text, Guid.NewGuid());
				string text3 = Path.Combine(GameDataPaths.CustomizationPresetIconsDirectory, text2);
				byte[] array = icon.EncodeToPNG();
				File.WriteAllBytes(text3, array);
				text4 = text2;
			}
			catch (Exception ex)
			{
				Debug.LogWarning("[MCCustomization] Failed to save preset icon: " + ex.Message);
				text4 = null;
			}
			return text4;
		}

		// Token: 0x060034BF RID: 13503 RVA: 0x0012D258 File Offset: 0x0012B458
		public bool LoadPreset(string path)
		{
			if (!File.Exists(path))
			{
				Debug.LogWarning("[MCCustomization] Preset file not found: " + path);
				return false;
			}
			string text = File.ReadAllText(path);
			this.activePreset = JsonUtility.FromJson<MCPresetData>(text);
			this.activePresetPath = path;
			this.ApplyPreset(this.activePreset);
			Debug.Log("[MCCustomization] Preset loaded from: " + path);
			return true;
		}

		// Token: 0x060034C0 RID: 13504 RVA: 0x0012D2B8 File Offset: 0x0012B4B8
		public List<string> GetAvailablePresetPaths()
		{
			string presetFolderPath = this.GetPresetFolderPath();
			if (!Directory.Exists(presetFolderPath))
			{
				return new List<string>();
			}
			return new List<string>(Directory.GetFiles(presetFolderPath, "*.json"));
		}

		// Token: 0x060034C1 RID: 13505 RVA: 0x0012D2EA File Offset: 0x0012B4EA
		private string GetPresetFolderPath()
		{
			return Path.Combine(Application.persistentDataPath, this.presetFolder);
		}

		// Token: 0x060034C2 RID: 13506 RVA: 0x0012D2FC File Offset: 0x0012B4FC
		private static string SanitizeFileName(string name)
		{
			if (string.IsNullOrWhiteSpace(name))
			{
				return "Unnamed";
			}
			foreach (char c in Path.GetInvalidFileNameChars())
			{
				name = name.Replace(c, '_');
			}
			return name;
		}

		// Token: 0x060034C3 RID: 13507 RVA: 0x0012D33C File Offset: 0x0012B53C
		public void CollectCurrentState(MCPresetData data)
		{
			if (this.generalInfoController != null)
			{
				data.presetName = this.generalInfoController.GetPresetName();
				data.bio = this.generalInfoController.GetBio();
			}
			if (this.heightController != null)
			{
				data.heightScale = this.heightController.GetCurrentScale();
			}
			if (this.blendShapeController != null && this.blendShapeController.BlendShapeMap != null)
			{
				data.blendShapeValues.Clear();
				foreach (BlendShapeMapSO.BlendShapeEntry blendShapeEntry in this.blendShapeController.BlendShapeMap.blendShapeMappings)
				{
					float entryAuthoredUiValue = this.blendShapeController.GetEntryAuthoredUiValue(blendShapeEntry);
					data.blendShapeValues.Add(new MCPresetData.BlendShapeValue
					{
						displayName = blendShapeEntry.displayName,
						value = entryAuthoredUiValue
					});
				}
			}
		}

		// Token: 0x060034C4 RID: 13508 RVA: 0x0012D440 File Offset: 0x0012B640
		private void CaptureSkinDecalState(CharacterStateData stateData)
		{
			if (stateData == null || this.skinDecalController == null)
			{
				return;
			}
			SkinDecalSaveData saveData = this.skinDecalController.GetSaveData(true);
			if (saveData == null || saveData.entries == null)
			{
				return;
			}
			if (stateData.skinDecals == null)
			{
				stateData.skinDecals = new List<SkinDecalStateValue>();
			}
			else
			{
				stateData.skinDecals.Clear();
			}
			for (int i = 0; i < saveData.entries.Count; i++)
			{
				SkinDecalSaveData.Entry entry = saveData.entries[i];
				if (entry != null && !string.IsNullOrWhiteSpace(entry.decalName))
				{
					stateData.skinDecals.Add(new SkinDecalStateValue(entry.decalName, entry.isActive, entry.opacity, entry.tint));
				}
			}
		}

		// Token: 0x060034C5 RID: 13509 RVA: 0x0012D4F4 File Offset: 0x0012B6F4
		public void ApplyPreset(MCPresetData data)
		{
			if (data == null)
			{
				return;
			}
			if (this.generalInfoController != null)
			{
				this.generalInfoController.SetPresetName(data.presetName);
				this.generalInfoController.SetBio(data.bio);
			}
			if (this.heightController != null)
			{
				this.heightController.SetScale(data.heightScale);
			}
			if (this.blendShapeController != null && this.blendShapeController.BlendShapeMap != null)
			{
				using (List<MCPresetData.BlendShapeValue>.Enumerator enumerator = data.blendShapeValues.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						MCPresetData.BlendShapeValue bsVal = enumerator.Current;
						BlendShapeMapSO.BlendShapeEntry blendShapeEntry = this.blendShapeController.BlendShapeMap.blendShapeMappings.Find((BlendShapeMapSO.BlendShapeEntry e) => e.displayName == bsVal.displayName);
						if (blendShapeEntry != null)
						{
							this.blendShapeController.ApplyEntryValue(blendShapeEntry, bsVal.value);
						}
					}
				}
			}
		}

		// Token: 0x060034C6 RID: 13510 RVA: 0x0012D600 File Offset: 0x0012B800
		private void OnBackToHub()
		{
			this.RequestBackToHub();
		}

		// Token: 0x060034C7 RID: 13511 RVA: 0x0012D608 File Offset: 0x0012B808
		public void RequestBackToHub()
		{
			Debug.Log("[MCCustomization] Back to hub requested.");
			this.ResetEditorStateForHub();
			if (this.navigationRouter != null)
			{
				this.navigationRouter.ReturnToHub();
				return;
			}
			if (this.mcEditorRoot != null)
			{
				this.mcEditorRoot.SetActive(false);
			}
			if (this.hubRoot != null)
			{
				this.hubRoot.SetActive(true);
				return;
			}
			Debug.LogWarning("[MCCustomization] No navigation target configured. Assign CustomizationNavigationRouter or Hub/Editor roots.");
		}

		// Token: 0x060034C8 RID: 13512 RVA: 0x0012D67E File Offset: 0x0012B87E
		public MCPresetData GetActivePreset()
		{
			return this.activePreset;
		}

		// Token: 0x060034C9 RID: 13513 RVA: 0x0012D686 File Offset: 0x0012B886
		public void ResetTransientEditorState()
		{
			this.ResetEditorStateForHub();
		}

		// Token: 0x060034CA RID: 13514 RVA: 0x0012D68E File Offset: 0x0012B88E
		private void CapturePresetPhotoFromCurrentCamera()
		{
			if (this._presetPhotoCaptureRoutine != null)
			{
				base.StopCoroutine(this._presetPhotoCaptureRoutine);
			}
			this._presetPhotoCaptureRoutine = base.StartCoroutine(this.CapturePresetPhotoAtEndOfFrame());
		}

		// Token: 0x060034CB RID: 13515 RVA: 0x0012D6B6 File Offset: 0x0012B8B6
		private IEnumerator CapturePresetPhotoAtEndOfFrame()
		{
			yield return MCCustomizationController.LiveIconEndOfFrame;
			this._presetPhotoCaptureRoutine = null;
			if (!base.isActiveAndEnabled)
			{
				yield break;
			}
			Texture2D texture2D = this.GenerateCharacterIcon();
			if (texture2D != null)
			{
				this.SetCapturedPresetPhoto(texture2D);
			}
			yield break;
		}

		// Token: 0x060034CC RID: 13516 RVA: 0x0012D6C5 File Offset: 0x0012B8C5
		private void HandleRemoveCapturedPhoto()
		{
			this.ClearCapturedPresetPhoto();
			this.photoRemovedByUser = true;
			if (this.generalInfoController != null)
			{
				this.generalInfoController.SetLivePhotoPreviewTexture(this.iconTexture);
			}
		}

		// Token: 0x060034CD RID: 13517 RVA: 0x0012D6F4 File Offset: 0x0012B8F4
		private void SetCapturedPresetPhoto(Texture2D capturedTexture)
		{
			if (capturedTexture == null)
			{
				return;
			}
			this.ClearCapturedPresetPhoto();
			this.capturedPresetPhoto = capturedTexture;
			this.photoRemovedByUser = false;
			if (this.generalInfoController != null)
			{
				this.capturedPresetPhotoSprite = Sprite.Create(this.capturedPresetPhoto, new Rect(0f, 0f, (float)this.capturedPresetPhoto.width, (float)this.capturedPresetPhoto.height), Vector2.one * 0.5f);
				this.generalInfoController.SetPresetPhotoPreview(this.capturedPresetPhotoSprite);
			}
		}

		// Token: 0x060034CE RID: 13518 RVA: 0x0012D788 File Offset: 0x0012B988
		private void ClearCapturedPresetPhoto()
		{
			if (this.generalInfoController != null)
			{
				this.generalInfoController.ClearPresetPhotoPreview();
			}
			if (this.capturedPresetPhotoSprite != null)
			{
				Object.Destroy(this.capturedPresetPhotoSprite);
				this.capturedPresetPhotoSprite = null;
			}
			if (this.capturedPresetPhoto != null)
			{
				Object.Destroy(this.capturedPresetPhoto);
				this.capturedPresetPhoto = null;
			}
		}

		// Token: 0x060034CF RID: 13519 RVA: 0x0012D7F0 File Offset: 0x0012B9F0
		private void UpdateIconCameraFromFollowTarget()
		{
			if (this.iconCamera == null || this.iconCameraFollowTarget == null)
			{
				return;
			}
			this.iconCamera.transform.position = this.iconCameraFollowTarget.position + this.iconCameraFollowOffset;
			if (this.iconCameraLookAtTarget)
			{
				this.iconCamera.transform.LookAt(this.iconCameraFollowTarget.position);
			}
		}

		// Token: 0x060034D0 RID: 13520 RVA: 0x0012D864 File Offset: 0x0012BA64
		private void OnDisable()
		{
			if (this._presetSaveRoutine != null)
			{
				base.StopCoroutine(this._presetSaveRoutine);
			}
			this._presetSaveRoutine = null;
			if (this._presetPhotoCaptureRoutine != null)
			{
				base.StopCoroutine(this._presetPhotoCaptureRoutine);
			}
			this._presetPhotoCaptureRoutine = null;
			if (this._liveIconRenderRoutine != null)
			{
				base.StopCoroutine(this._liveIconRenderRoutine);
			}
			this._liveIconRenderRoutine = null;
			if (this._ownsIconCamera && this.iconCamera != null)
			{
				this.iconCamera.enabled = this._iconCameraWasEnabled;
			}
			this._ownsIconCamera = false;
		}

		// Token: 0x170009AF RID: 2479
		// (get) Token: 0x060034D1 RID: 13521 RVA: 0x0012D8F0 File Offset: 0x0012BAF0
		private bool IsLiveIconPreviewVisible
		{
			get
			{
				return this.capturedPresetPhoto == null && this.generalInfoController != null && this.generalInfoController.IsLivePhotoPreviewVisible;
			}
		}

		// Token: 0x060034D2 RID: 13522 RVA: 0x0012D91C File Offset: 0x0012BB1C
		private void RenderLiveIconPreviewIfNeeded()
		{
			if (!this.IsLiveIconPreviewVisible || this.iconCamera == null || this.iconTexture == null || this._liveIconRenderRoutine != null)
			{
				return;
			}
			if (Time.unscaledTime < this._nextLiveIconRenderTime)
			{
				return;
			}
			float num = 1f / Mathf.Max(15f, this.liveIconPreviewFps);
			this._nextLiveIconRenderTime = Mathf.Max(this._nextLiveIconRenderTime + num, Time.unscaledTime);
			this._liveIconRenderRoutine = base.StartCoroutine(this.RenderLiveIconAtEndOfFrame());
		}

		// Token: 0x060034D3 RID: 13523 RVA: 0x0012D9A5 File Offset: 0x0012BBA5
		private IEnumerator RenderLiveIconAtEndOfFrame()
		{
			yield return MCCustomizationController.LiveIconEndOfFrame;
			try
			{
				if (!this.IsLiveIconPreviewVisible || this.iconCamera == null || this.iconTexture == null)
				{
					yield break;
				}
				this.UpdateIconCameraFromFollowTarget();
				SkinDecalController skinDecalController = this.skinDecalController;
				if (skinDecalController != null)
				{
					skinDecalController.SynchronizeMeshesForCapture();
				}
				this.iconCamera.targetTexture = this.iconTexture;
				CharacterPreviewRendering.RenderPortrait(this.iconCamera, this.iconTexture, false);
				yield break;
			}
			finally
			{
				this._liveIconRenderRoutine = null;
			}
			yield break;
		}

		// Token: 0x060034D4 RID: 13524 RVA: 0x0012D9B4 File Offset: 0x0012BBB4
		private string ResolveEditingPresetId()
		{
			if (!string.IsNullOrWhiteSpace(this.editingPresetId))
			{
				return this.editingPresetId;
			}
			this.UpdateEditingPresetFromSession();
			return this.editingPresetId;
		}

		// Token: 0x060034D5 RID: 13525 RVA: 0x0012D9D8 File Offset: 0x0012BBD8
		private void UpdateEditingPresetFromSession()
		{
			if (this.coreOrchestrator == null)
			{
				return;
			}
			CustomizationSessionContext currentSession = this.coreOrchestrator.CurrentSession;
			if (currentSession == null)
			{
				this.editingPresetId = null;
				this.hasAppliedEditingContext = false;
				return;
			}
			if (this.editingPresetId != currentSession.presetId)
			{
				this.hasAppliedEditingContext = false;
			}
			this.editingPresetId = currentSession.presetId;
			if (string.IsNullOrWhiteSpace(this.editingPresetId) || this.editingPresetId.StartsWith("sys_", StringComparison.OrdinalIgnoreCase))
			{
				this.hasAppliedEditingContext = true;
				return;
			}
			this.TryApplyEditingPresetContext(currentSession);
		}

		// Token: 0x060034D6 RID: 13526 RVA: 0x0012DA68 File Offset: 0x0012BC68
		private void TryApplyEditingPresetContext(CustomizationSessionContext session)
		{
			if (this.hasAppliedEditingContext)
			{
				return;
			}
			if (session == null || string.IsNullOrWhiteSpace(this.editingPresetId) || this.editingPresetId.StartsWith("sys_", StringComparison.OrdinalIgnoreCase))
			{
				return;
			}
			if (this.charactersManager == null)
			{
				return;
			}
			CustomizationPresetData customizationPresetData;
			if (!this.TryFindPresetById(this.editingPresetId, session, out customizationPresetData))
			{
				return;
			}
			if (this.generalInfoController != null)
			{
				this.generalInfoController.ApplyPresetMetadata(customizationPresetData.presetName, customizationPresetData.stateData);
			}
			if (this.activePreset == null)
			{
				this.activePreset = new MCPresetData();
			}
			this.activePreset.presetName = customizationPresetData.presetName;
			MCPresetData mcpresetData = this.activePreset;
			CharacterStateData stateData = customizationPresetData.stateData;
			mcpresetData.bio = ((stateData != null) ? stateData.mainCharacterBio : null) ?? string.Empty;
			if (!this.TryLoadCapturedPhotoFromPreset(customizationPresetData))
			{
				this.photoRemovedByUser = false;
				if (this.generalInfoController != null)
				{
					this.generalInfoController.SetLivePhotoPreviewTexture(this.iconTexture);
				}
			}
			this.hasAppliedEditingContext = true;
		}

		// Token: 0x060034D7 RID: 13527 RVA: 0x0012DB68 File Offset: 0x0012BD68
		private bool TryFindPresetById(string presetId, CustomizationSessionContext session, out CustomizationPresetData foundPreset)
		{
			foundPreset = null;
			if (this.charactersManager == null || string.IsNullOrWhiteSpace(presetId))
			{
				return false;
			}
			List<string> list = new List<string>();
			if (session != null && !string.IsNullOrWhiteSpace(session.characterId))
			{
				list.Add(session.characterId);
			}
			CharacterDataExtended mainCharacter = this.charactersManager.GetMainCharacter();
			if (mainCharacter != null && !string.IsNullOrWhiteSpace(mainCharacter.characterId) && !list.Contains(mainCharacter.characterId))
			{
				list.Add(mainCharacter.characterId);
			}
			string selectedCharacterId = this.charactersManager.GetSelectedCharacterId();
			if (!string.IsNullOrWhiteSpace(selectedCharacterId) && !list.Contains(selectedCharacterId))
			{
				list.Add(selectedCharacterId);
			}
			for (int i = 0; i < list.Count; i++)
			{
				List<CustomizationPresetData> presetsForCharacter = this.charactersManager.GetPresetsForCharacter(list[i]);
				for (int j = 0; j < presetsForCharacter.Count; j++)
				{
					CustomizationPresetData customizationPresetData = presetsForCharacter[j];
					if (customizationPresetData != null && customizationPresetData.presetId == presetId)
					{
						foundPreset = customizationPresetData;
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x060034D8 RID: 13528 RVA: 0x0012DC6C File Offset: 0x0012BE6C
		private void HandleSessionInitialized(CustomizationSessionContext session, ICharacterCustomizationBackend backend)
		{
			if (this.styleCustomizationService != null)
			{
				this.styleCustomizationService.SetPreviewClothingHidden(false);
			}
			this.hasAppliedEditingContext = false;
			this.UpdateEditingPresetFromSession();
		}

		// Token: 0x060034D9 RID: 13529 RVA: 0x0012DC95 File Offset: 0x0012BE95
		private void HandleSessionClosed(CustomizationSessionContext session)
		{
			if (this.styleCustomizationService != null)
			{
				this.styleCustomizationService.SetPreviewClothingHidden(false);
			}
			this.hasAppliedEditingContext = false;
			this.editingPresetId = null;
		}

		// Token: 0x060034DA RID: 13530 RVA: 0x0012DCC0 File Offset: 0x0012BEC0
		private bool TryLoadCapturedPhotoFromPreset(CustomizationPresetData preset)
		{
			if (preset == null || string.IsNullOrWhiteSpace(preset.iconFileName))
			{
				return false;
			}
			string text = Path.Combine(GameDataPaths.CustomizationPresetIconsDirectory, preset.iconFileName);
			if (!File.Exists(text))
			{
				return false;
			}
			bool flag;
			try
			{
				byte[] array = File.ReadAllBytes(text);
				Texture2D texture2D = new Texture2D(2, 2, TextureFormat.RGBA32, false);
				if (!texture2D.LoadImage(array))
				{
					Object.Destroy(texture2D);
					flag = false;
				}
				else
				{
					this.SetCapturedPresetPhoto(texture2D);
					flag = true;
				}
			}
			catch (Exception ex)
			{
				Debug.LogWarning("[MCCustomization] Failed to load preset icon preview for edit mode: " + ex.Message);
				flag = false;
			}
			return flag;
		}

		// Token: 0x060034DB RID: 13531 RVA: 0x0012DD58 File Offset: 0x0012BF58
		private void ResetEditorStateForHub()
		{
			this.editingPresetId = null;
			this.activePresetPath = null;
			this.activePreset = new MCPresetData();
			this.photoRemovedByUser = false;
			this.hasAppliedEditingContext = false;
			this.ClearCapturedPresetPhoto();
			if (this.generalInfoController != null)
			{
				this.generalInfoController.ResetToDefaults();
			}
			if (this.heightController != null)
			{
				this.heightController.ResetToDefault();
			}
			if (this.colorCustomizationService != null)
			{
				this.colorCustomizationService.RebuildPresetMap();
			}
			this.SelectCategory(this.defaultCategory);
		}

		// Token: 0x040034F7 RID: 13559
		[Header("Categories")]
		[SerializeField]
		private List<MCCustomizationController.CategoryButton> categoryButtons = new List<MCCustomizationController.CategoryButton>();

		// Token: 0x040034F8 RID: 13560
		[SerializeField]
		private string defaultCategory = "Head";

		// Token: 0x040034F9 RID: 13561
		[Header("Camera")]
		[SerializeField]
		private CameraController cameraController;

		// Token: 0x040034FA RID: 13562
		[Header("Icon Rendering")]
		[SerializeField]
		private Camera iconCamera;

		// Token: 0x040034FB RID: 13563
		[SerializeField]
		private RenderTexture iconTexture;

		// Token: 0x040034FC RID: 13564
		[SerializeField]
		private int iconWidth = 256;

		// Token: 0x040034FD RID: 13565
		[SerializeField]
		private int iconHeight = 256;

		// Token: 0x040034FE RID: 13566
		[SerializeField]
		private Transform iconCameraFollowTarget;

		// Token: 0x040034FF RID: 13567
		[SerializeField]
		private Vector3 iconCameraFollowOffset = new Vector3(0f, 0.1f, -0.8f);

		// Token: 0x04003500 RID: 13568
		[SerializeField]
		private bool iconCameraLookAtTarget = true;

		// Token: 0x04003501 RID: 13569
		[Tooltip("Live icon refresh rate. Icon capture on save renders at the player's texture quality.")]
		[SerializeField]
		[Range(15f, 60f)]
		private float liveIconPreviewFps = 30f;

		// Token: 0x04003502 RID: 13570
		[Header("Sub-Controllers")]
		[SerializeField]
		private CharacterBlendShapeController blendShapeController;

		// Token: 0x04003503 RID: 13571
		[SerializeField]
		private MCHeightController heightController;

		// Token: 0x04003504 RID: 13572
		[SerializeField]
		private MCGeneralInfoController generalInfoController;

		// Token: 0x04003505 RID: 13573
		[SerializeField]
		private MCColorCustomizationService colorCustomizationService;

		// Token: 0x04003506 RID: 13574
		[SerializeField]
		private MCStyleCustomizationService styleCustomizationService;

		// Token: 0x04003507 RID: 13575
		[SerializeField]
		private SkinDecalController skinDecalController;

		// Token: 0x04003508 RID: 13576
		[Header("UI Behavior")]
		[SerializeField]
		private bool hideLegacyColorsCategory = true;

		// Token: 0x04003509 RID: 13577
		[Header("Movement Preview")]
		[Tooltip("Animator del modello preview del MC (CielBase). Se nullo viene risolto dal blendShapeController.")]
		[SerializeField]
		private Animator previewAnimator;

		// Token: 0x0400350A RID: 13578
		[Tooltip("InputMagnitude usato per la camminata in-place (blend tree Free Locomotion del CielCustomizationAnimatorController).")]
		[SerializeField]
		private float walkInputMagnitude = 0.5f;

		// Token: 0x0400350B RID: 13579
		[Header("Preset Persistence")]
		[SerializeField]
		private string presetFolder = "MCPresets";

		// Token: 0x0400350C RID: 13580
		[SerializeField]
		private CharactersManager charactersManager;

		// Token: 0x0400350D RID: 13581
		[SerializeField]
		private CustomizationCoreOrchestrator coreOrchestrator;

		// Token: 0x0400350E RID: 13582
		[Header("Optional — Back Button")]
		[SerializeField]
		private Button backToHubButton;

		// Token: 0x0400350F RID: 13583
		[Header("Optional — Clothing Toggle (Naked/Clothed)")]
		[Tooltip("Bottone Naked/Clothed del preview MC. Se assegnato, l'onClick viene agganciato a ToggleNakedMC() A RUNTIME in Start (come backToHubButton), invece di affidarsi al listener persistente serializzato — che, puntando a un componente di un prefab nidificato, è fragile e a runtime spesso non scatta.")]
		[SerializeField]
		private Button clothingToggleButton;

		// Token: 0x04003510 RID: 13584
		[Header("Optional — Navigation")]
		[SerializeField]
		private CustomizationNavigationRouter navigationRouter;

		// Token: 0x04003511 RID: 13585
		[SerializeField]
		private GameObject hubRoot;

		// Token: 0x04003512 RID: 13586
		[SerializeField]
		private GameObject mcEditorRoot;

		// Token: 0x04003513 RID: 13587
		private MCCustomizationController.CategoryButton currentCategory;

		// Token: 0x04003514 RID: 13588
		private MCPresetData activePreset;

		// Token: 0x04003515 RID: 13589
		private string activePresetPath;

		// Token: 0x04003516 RID: 13590
		private string editingPresetId;

		// Token: 0x04003517 RID: 13591
		private Texture2D capturedPresetPhoto;

		// Token: 0x04003518 RID: 13592
		private Sprite capturedPresetPhotoSprite;

		// Token: 0x04003519 RID: 13593
		private bool photoRemovedByUser;

		// Token: 0x0400351A RID: 13594
		private bool hasAppliedEditingContext;

		// Token: 0x0400351B RID: 13595
		private float _nextLiveIconRenderTime;

		// Token: 0x0400351C RID: 13596
		private readonly List<CharacterCustomizationController> _contextualCameraSources = new List<CharacterCustomizationController>();

		// Token: 0x0400351D RID: 13597
		private bool _isWalkingPreview;

		// Token: 0x0400351E RID: 13598
		private ClothingToggleController _clothingToggleController;

		// Token: 0x0400351F RID: 13599
		private Coroutine _presetSaveRoutine;

		// Token: 0x04003520 RID: 13600
		private Coroutine _presetPhotoCaptureRoutine;

		// Token: 0x04003521 RID: 13601
		private bool _iconCameraWasEnabled;

		// Token: 0x04003522 RID: 13602
		private bool _ownsIconCamera;

		// Token: 0x04003523 RID: 13603
		private Coroutine _liveIconRenderRoutine;

		// Token: 0x04003524 RID: 13604
		private static readonly WaitForEndOfFrame LiveIconEndOfFrame = new WaitForEndOfFrame();

		// Token: 0x020007C3 RID: 1987
		[Serializable]
		public class CategoryButton
		{
			// Token: 0x04003525 RID: 13605
			public string categoryName;

			// Token: 0x04003526 RID: 13606
			public GameObject categoryContent;

			// Token: 0x04003527 RID: 13607
			public Button button;

			// Token: 0x04003528 RID: 13608
			public GameObject normalState;

			// Token: 0x04003529 RID: 13609
			public GameObject selectedState;
		}
	}
}
