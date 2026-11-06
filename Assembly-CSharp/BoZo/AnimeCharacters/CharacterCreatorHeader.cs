using System;
using System.Collections;
using BoZo.CharacterCreator.MC;
using Features.Localization;
using Features.MainMenu;
using ProjectExhibitionist.CharacterCustomization.Core.Presets;
using ProjectExhibitionist.CharacterCustomization.Core.Runtime;
using ProjectExhibitionist.UI;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Bozo.AnimeCharacters
{
	// Token: 0x02000860 RID: 2144
	public class CharacterCreatorHeader : MonoBehaviour
	{
		// Token: 0x060038B5 RID: 14517 RVA: 0x0014AC89 File Offset: 0x00148E89
		private void Awake()
		{
			this.ResolveHeaderReferences();
			if (this.navigationRouter == null)
			{
				this.navigationRouter = Object.FindObjectOfType<CustomizationNavigationRouter>(true);
			}
			this.InitializeComponents();
			this.SetupButtons();
			this.SetupInitialState();
		}

		// Token: 0x060038B6 RID: 14518 RVA: 0x0014ACC0 File Offset: 0x00148EC0
		private void ResolveHeaderReferences()
		{
			foreach (Button button in base.GetComponentsInChildren<Button>(true))
			{
				if (!(button == null))
				{
					string name = button.name;
					if (this.backButton == null && name.IndexOf("back", StringComparison.OrdinalIgnoreCase) >= 0)
					{
						this.backButton = button;
					}
					else if (this.saveButton == null && name.IndexOf("save", StringComparison.OrdinalIgnoreCase) >= 0)
					{
						this.saveButton = button;
					}
				}
			}
			if (this.characterNameText == null)
			{
				TextMeshProUGUI[] componentsInChildren2 = base.GetComponentsInChildren<TextMeshProUGUI>(true);
				for (int j = 0; j < componentsInChildren2.Length; j++)
				{
					string name2 = componentsInChildren2[j].name;
					if (name2.IndexOf("character", StringComparison.OrdinalIgnoreCase) >= 0 || name2.IndexOf("name", StringComparison.OrdinalIgnoreCase) >= 0)
					{
						this.characterNameText = componentsInChildren2[j];
						return;
					}
				}
			}
		}

		// Token: 0x060038B7 RID: 14519 RVA: 0x0014ADA3 File Offset: 0x00148FA3
		private void OnEnable()
		{
			this.SubscribeToEvents();
			this.EnsureButtonsVisibility();
			base.StartCoroutine(this.ReassertHeaderVisualsEndOfFrame());
			this.SyncNameFromSelectedPresetOrFallback();
		}

		// Token: 0x060038B8 RID: 14520 RVA: 0x0014ADC4 File Offset: 0x00148FC4
		private void SyncNameFromSelectedPresetOrFallback()
		{
			if (this.hubPresetListController == null)
			{
				this.hubPresetListController = Object.FindObjectOfType<CustomizationHubPresetListController>(true);
			}
			if (this.hubPresetListController != null && this.hubPresetListController.SelectedPreset != null && !string.IsNullOrWhiteSpace(this.hubPresetListController.SelectedPreset.presetName))
			{
				this.UpdateNameDisplay(this.hubPresetListController.SelectedPreset.presetName);
				return;
			}
			this.UpdateNameDisplay(null);
		}

		// Token: 0x060038B9 RID: 14521 RVA: 0x0014AE3B File Offset: 0x0014903B
		private void OnDisable()
		{
			this.UnsubscribeFromEvents();
		}

		// Token: 0x060038BA RID: 14522 RVA: 0x0014AE43 File Offset: 0x00149043
		private void InitializeComponents()
		{
			this.mcCustomizationController = Object.FindObjectOfType<MCCustomizationController>(true);
			this.npcCustomizationController = Object.FindObjectOfType<NpcCustomizationController>(true);
			this.hubPresetListController = Object.FindObjectOfType<CustomizationHubPresetListController>(true);
		}

		// Token: 0x060038BB RID: 14523 RVA: 0x0014AE6C File Offset: 0x0014906C
		private void SetupButtons()
		{
			if (this.backButton != null)
			{
				this.backButton.onClick.AddListener(new UnityAction(this.OnBackButtonClicked));
			}
			if (this.saveButton != null)
			{
				this.saveButton.onClick.AddListener(new UnityAction(this.OnSaveButtonClicked));
			}
		}

		// Token: 0x060038BC RID: 14524 RVA: 0x0014AECD File Offset: 0x001490CD
		private void SetupInitialState()
		{
			if (this.useLegacyLocalToast && this.toastPanel != null)
			{
				this.toastPanel.SetActive(false);
			}
			this.EnsureButtonsVisibility();
			this.InitializeNameDisplay();
		}

		// Token: 0x060038BD RID: 14525 RVA: 0x0014AF00 File Offset: 0x00149100
		private void InitializeNameDisplay()
		{
			if (CharacterStateManager.Instance != null)
			{
				string currentCharacterName = CharacterStateManager.Instance.GetCurrentCharacterName();
				this.UpdateNameDisplay(currentCharacterName);
				return;
			}
			this.UpdateNameDisplay("New Character");
		}

		// Token: 0x060038BE RID: 14526 RVA: 0x0014AF38 File Offset: 0x00149138
		private void SubscribeToEvents()
		{
			this.SubscribeToHubPresetList();
			if (this.isSubscribedToStateManager)
			{
				return;
			}
			CharacterStateManager instance = CharacterStateManager.Instance;
			if (instance == null)
			{
				return;
			}
			instance.OnCharacterSaved += this.OnCharacterSaved;
			instance.OnNewCharacterCreated += this.OnNewCharacterCreated;
			instance.OnCharacterNameChanged += this.OnCharacterNameChanged;
			this.isSubscribedToStateManager = true;
		}

		// Token: 0x060038BF RID: 14527 RVA: 0x0014AFA4 File Offset: 0x001491A4
		private void UnsubscribeFromEvents()
		{
			this.UnsubscribeFromHubPresetList();
			if (!this.isSubscribedToStateManager)
			{
				return;
			}
			CharacterStateManager instance = CharacterStateManager.Instance;
			if (instance != null)
			{
				instance.OnCharacterSaved -= this.OnCharacterSaved;
				instance.OnNewCharacterCreated -= this.OnNewCharacterCreated;
				instance.OnCharacterNameChanged -= this.OnCharacterNameChanged;
			}
			this.isSubscribedToStateManager = false;
		}

		// Token: 0x060038C0 RID: 14528 RVA: 0x0014B00C File Offset: 0x0014920C
		private void SubscribeToHubPresetList()
		{
			if (this.isSubscribedToHubPresetList)
			{
				return;
			}
			if (this.hubPresetListController == null)
			{
				this.hubPresetListController = Object.FindObjectOfType<CustomizationHubPresetListController>(true);
			}
			if (this.hubPresetListController == null)
			{
				return;
			}
			this.hubPresetListController.OnPresetSelected += this.OnHubPresetSelected;
			this.hubPresetListController.OnNewPresetDraftStarted += this.OnHubNewPresetDraftStarted;
			this.isSubscribedToHubPresetList = true;
		}

		// Token: 0x060038C1 RID: 14529 RVA: 0x0014B080 File Offset: 0x00149280
		private void UnsubscribeFromHubPresetList()
		{
			if (!this.isSubscribedToHubPresetList)
			{
				return;
			}
			if (this.hubPresetListController != null)
			{
				this.hubPresetListController.OnPresetSelected -= this.OnHubPresetSelected;
				this.hubPresetListController.OnNewPresetDraftStarted -= this.OnHubNewPresetDraftStarted;
			}
			this.isSubscribedToHubPresetList = false;
		}

		// Token: 0x060038C2 RID: 14530 RVA: 0x0014B0D9 File Offset: 0x001492D9
		private void OnHubPresetSelected(CustomizationPresetData preset)
		{
			if (preset == null || string.IsNullOrWhiteSpace(preset.presetName))
			{
				return;
			}
			this.UpdateNameDisplay(preset.presetName);
		}

		// Token: 0x060038C3 RID: 14531 RVA: 0x0014B0F8 File Offset: 0x001492F8
		private void OnHubNewPresetDraftStarted()
		{
			string text = ((CharacterStateManager.Instance != null) ? CharacterStateManager.Instance.defaultCharacterName : "New Character");
			this.UpdateNameDisplay(text);
		}

		// Token: 0x060038C4 RID: 14532 RVA: 0x0014B12B File Offset: 0x0014932B
		private void OnCharacterNameChanged(string characterName)
		{
			this.UpdateNameDisplay(characterName);
		}

		// Token: 0x060038C5 RID: 14533 RVA: 0x0014B134 File Offset: 0x00149334
		private void UpdateNameDisplay(string characterName = null)
		{
			if (characterName == null && CharacterStateManager.Instance != null)
			{
				characterName = CharacterStateManager.Instance.GetCurrentCharacterName();
			}
			if (this.characterNameText != null)
			{
				this.characterNameText.gameObject.SetActive(true);
				LocalizedGameText.BindComputed(this.characterNameText, () => characterName ?? GameLocalization.GetSource("cielos_apps", "New Character"));
			}
		}

		// Token: 0x060038C6 RID: 14534 RVA: 0x0014B1AC File Offset: 0x001493AC
		private void OnBackButtonClicked()
		{
			if (this.mcCustomizationController != null && this.mcCustomizationController.gameObject.activeInHierarchy)
			{
				this.mcCustomizationController.RequestBackToHub();
				return;
			}
			if (this.navigationRouter != null)
			{
				this.navigationRouter.ReturnToHub();
				return;
			}
			Debug.LogError("No CustomizationNavigationRouter available for back navigation! Please assign it in the inspector.");
		}

		// Token: 0x060038C7 RID: 14535 RVA: 0x0014B20C File Offset: 0x0014940C
		private void OnSaveButtonClicked()
		{
			if (this.mcCustomizationController != null && this.mcCustomizationController.gameObject.activeInHierarchy)
			{
				if (string.IsNullOrWhiteSpace(this.mcCustomizationController.GetCurrentPresetName()))
				{
					this.ShowToast("Please enter a preset name before saving!", null);
					return;
				}
				this.mcCustomizationController.RequestSavePreset(delegate(bool saved, string mcError)
				{
					if (this == null || !base.isActiveAndEnabled)
					{
						return;
					}
					if (saved)
					{
						this.ShowToast("Preset saved successfully!", "Saved");
						return;
					}
					string text2 = (string.IsNullOrWhiteSpace(mcError) ? "Error: Could not save preset" : mcError);
					this.ShowToast(text2, "Error");
				});
				return;
			}
			else
			{
				if (this.npcCustomizationController == null)
				{
					this.npcCustomizationController = Object.FindObjectOfType<NpcCustomizationController>(true);
				}
				if (this.npcCustomizationController != null)
				{
					string text;
					if (this.npcCustomizationController.TrySavePreset(out text))
					{
						this.ShowToast("NPC saved!", "Saved");
						return;
					}
					this.ShowToast(string.IsNullOrWhiteSpace(text) ? "Error saving NPC" : text, "Error");
					return;
				}
				else
				{
					if (!(CharacterStateManager.Instance != null))
					{
						this.ShowToast("Error: Could not save character", "Error");
						return;
					}
					string currentCharacterName = CharacterStateManager.Instance.GetCurrentCharacterName();
					if (currentCharacterName == CharacterStateManager.Instance.defaultCharacterName)
					{
						this.ShowToast("Please enter a character name before saving!", "Save");
						return;
					}
					CharacterStateManager.Instance.SaveCurrentCharacter(currentCharacterName);
					this.ShowToast("Saving character...", "Save");
					return;
				}
			}
		}

		// Token: 0x060038C8 RID: 14536 RVA: 0x0014B33C File Offset: 0x0014953C
		private void ShowToast(string message, string title = null)
		{
			CustomizationPresentation.ShowError(message, title, this.useLegacyLocalToast ? this.toastText : null);
			if (this.useLegacyLocalToast && this.toastPanel != null && this.toastText != null)
			{
				this.toastPanel.SetActive(true);
				base.Invoke("HideToast", this.toastDuration);
			}
		}

		// Token: 0x060038C9 RID: 14537 RVA: 0x0014B3A2 File Offset: 0x001495A2
		private void HideToast()
		{
			if (this.toastPanel != null)
			{
				this.toastPanel.SetActive(false);
			}
		}

		// Token: 0x060038CA RID: 14538 RVA: 0x0014B3BE File Offset: 0x001495BE
		public void ForceSave()
		{
			this.OnSaveButtonClicked();
		}

		// Token: 0x060038CB RID: 14539 RVA: 0x0014B3C6 File Offset: 0x001495C6
		public void ForceBack()
		{
			this.OnBackButtonClicked();
		}

		// Token: 0x060038CC RID: 14540 RVA: 0x0014B3CE File Offset: 0x001495CE
		public void EnsureUIVisibility()
		{
			this.EnsureButtonsVisibility();
		}

		// Token: 0x060038CD RID: 14541 RVA: 0x0014B3D8 File Offset: 0x001495D8
		[ContextMenu("Debug Navigation References")]
		public void DebugNavigationReferences()
		{
			Debug.Log("=== HEADER NAVIGATION DEBUG ===");
			Debug.Log("Script on GameObject: " + base.gameObject.name);
			Debug.Log(string.Format("CustomizationNavigationRouter reference: {0}", this.navigationRouter));
			Debug.Log("=== HEADER DEBUG COMPLETE ===");
		}

		// Token: 0x060038CE RID: 14542 RVA: 0x0014B428 File Offset: 0x00149628
		public void SwitchToCharacterCreator()
		{
			this.SwitchToCharacterCreator(true);
		}

		// Token: 0x060038CF RID: 14543 RVA: 0x0014B434 File Offset: 0x00149634
		public void SwitchToCharacterCreator(bool createNewCharacter = true)
		{
			if (this.navigationRouter != null)
			{
				this.navigationRouter.OpenCharacterCreator(createNewCharacter);
				this.EnsureButtonsVisibility();
				return;
			}
			Debug.LogError("[" + base.gameObject.name + "] CustomizationNavigationRouter not assigned! Please assign it in the Inspector.");
		}

		// Token: 0x060038D0 RID: 14544 RVA: 0x0014B484 File Offset: 0x00149684
		private void OnCharacterSaved(string characterName, bool success)
		{
			if (success)
			{
				ToastService.ShowFormat("cielos_apps", "Character '{0}' saved successfully!", new object[] { characterName }, "Saved", null, -1f);
				if (this.useLegacyLocalToast && this.toastPanel != null && this.toastText != null)
				{
					LocalizedGameText localizedGameText = LocalizedGameText.BindFormat(this.toastText, "cielos_apps", "Character '{0}' saved successfully!", new object[] { characterName });
					if (localizedGameText != null)
					{
						localizedGameText.FitTranslationToRectangle(false);
					}
					this.toastPanel.SetActive(true);
					base.Invoke("HideToast", this.toastDuration);
					return;
				}
			}
			else
			{
				this.ShowToast("Error: Could not save character", "Error");
			}
		}

		// Token: 0x060038D1 RID: 14545 RVA: 0x0014B538 File Offset: 0x00149738
		private void OnNewCharacterCreated()
		{
			this.UpdateNameDisplay(null);
		}

		// Token: 0x060038D2 RID: 14546 RVA: 0x0014B541 File Offset: 0x00149741
		private void EnsureButtonsVisibility()
		{
			CharacterCreatorHeader.RestoreHeaderButton(this.backButton);
			CharacterCreatorHeader.RestoreHeaderButton(this.saveButton);
		}

		// Token: 0x060038D3 RID: 14547 RVA: 0x0014B55C File Offset: 0x0014975C
		private static void RestoreHeaderButton(Button button)
		{
			if (button == null)
			{
				return;
			}
			button.gameObject.SetActive(true);
			button.interactable = true;
			MainMenuButtonFx component = button.GetComponent<MainMenuButtonFx>();
			if (component != null)
			{
				component.SetScaleEffectsEnabled(false);
			}
			button.transform.localScale = Vector3.one;
			if (button.name.IndexOf("back", StringComparison.OrdinalIgnoreCase) >= 0)
			{
				CharacterCreatorHeader.RestoreBackButtonVisuals(button);
			}
		}

		// Token: 0x060038D4 RID: 14548 RVA: 0x0014B5C8 File Offset: 0x001497C8
		private static void RestoreBackButtonVisuals(Button button)
		{
			CharacterCreatorHeader.EnsureIconOnlyBack(button);
			CanvasGroup[] componentsInChildren = button.GetComponentsInChildren<CanvasGroup>(true);
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				componentsInChildren[i].alpha = 1f;
			}
			foreach (Graphic graphic in button.GetComponentsInChildren<Graphic>(true))
			{
				if (!(graphic == null))
				{
					Image image = graphic as Image;
					if (image == null || image.sprite != null)
					{
						graphic.gameObject.SetActive(true);
						graphic.enabled = true;
						Color color = graphic.color;
						color.a = 1f;
						if (graphic.transform != button.transform)
						{
							color.r = 1f;
							color.g = 1f;
							color.b = 1f;
						}
						graphic.color = color;
						graphic.transform.localScale = Vector3.one;
					}
				}
			}
			Graphic[] componentsInChildren2;
			Transform transform = ((componentsInChildren2.Length != 0) ? componentsInChildren2[componentsInChildren2.Length - 1].transform : null);
			while (transform != null && transform != button.transform)
			{
				transform.localScale = Vector3.one;
				transform = transform.parent;
			}
		}

		// Token: 0x060038D5 RID: 14549 RVA: 0x0014B70C File Offset: 0x0014990C
		private static void EnsureIconOnlyBack(Button button)
		{
			Transform transform = button.transform.Find("Runtime Back Label");
			if (transform != null)
			{
				Object.Destroy(transform.gameObject);
			}
			Mask component = button.GetComponent<Mask>();
			if (component != null)
			{
				component.enabled = false;
			}
			LayoutElement layoutElement = button.GetComponent<LayoutElement>();
			if (layoutElement == null)
			{
				layoutElement = button.gameObject.AddComponent<LayoutElement>();
			}
			layoutElement.minWidth = 68f;
			layoutElement.preferredWidth = 68f;
			layoutElement.minHeight = 63f;
			layoutElement.preferredHeight = 63f;
			bool flag = false;
			foreach (Image image in button.GetComponentsInChildren<Image>(true))
			{
				if (!(image == null) && !(image.transform == button.transform) && !(image.sprite == null))
				{
					flag = true;
					image.gameObject.SetActive(true);
					image.enabled = true;
					image.transform.localScale = Vector3.one;
				}
			}
			if (!flag)
			{
				Debug.LogWarning("[CreatorHeader] Back button '" + button.name + "' has no icon sprite to display.");
			}
		}

		// Token: 0x060038D6 RID: 14550 RVA: 0x0014B834 File Offset: 0x00149A34
		private IEnumerator ReassertHeaderVisualsEndOfFrame()
		{
			yield return new WaitForEndOfFrame();
			this.EnsureButtonsVisibility();
			if (this.backButton != null)
			{
				RectTransform rectTransform = this.backButton.transform as RectTransform;
				Debug.Log(string.Format("[CreatorHeader] Back '{0}' — activeInHierarchy={1}, ", this.backButton.name, this.backButton.gameObject.activeInHierarchy) + string.Format("size={0}, lossyScale={1}", (rectTransform != null) ? rectTransform.rect.size.ToString() : "?", this.backButton.transform.lossyScale));
			}
			yield break;
		}

		// Token: 0x060038D7 RID: 14551 RVA: 0x0014AE3B File Offset: 0x0014903B
		private void OnDestroy()
		{
			this.UnsubscribeFromEvents();
		}

		// Token: 0x040039A5 RID: 14757
		[Header("UI References")]
		public Button backButton;

		// Token: 0x040039A6 RID: 14758
		public Button saveButton;

		// Token: 0x040039A7 RID: 14759
		public TextMeshProUGUI characterNameText;

		// Token: 0x040039A8 RID: 14760
		[Header("Navigation")]
		[SerializeField]
		private CustomizationNavigationRouter navigationRouter;

		// Token: 0x040039A9 RID: 14761
		[Header("Toast Settings")]
		public GameObject toastPanel;

		// Token: 0x040039AA RID: 14762
		public TextMeshProUGUI toastText;

		// Token: 0x040039AB RID: 14763
		public float toastDuration = 2f;

		// Token: 0x040039AC RID: 14764
		[SerializeField]
		private bool useLegacyLocalToast;

		// Token: 0x040039AD RID: 14765
		private MCCustomizationController mcCustomizationController;

		// Token: 0x040039AE RID: 14766
		private NpcCustomizationController npcCustomizationController;

		// Token: 0x040039AF RID: 14767
		private CustomizationHubPresetListController hubPresetListController;

		// Token: 0x040039B0 RID: 14768
		private bool isSubscribedToStateManager;

		// Token: 0x040039B1 RID: 14769
		private bool isSubscribedToHubPresetList;
	}
}
