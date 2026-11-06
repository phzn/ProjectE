using System;
using System.Collections.Generic;
using Bozo.AnimeCharacters;
using BoZo.CharacterCreator;
using BoZo.CharacterCreator.MC;
using DA_Assets.DAB;
using Features.CielOS;
using Features.CielOS.Apps;
using Features.CielOS.Apps.Dialogues;
using Features.CielOS.Apps.Social;
using Features.CielOS.Panel;
using Features.CielOS.WindowManager;
using Features.Detection;
using Features.GameOver.Newspaper;
using Features.Inventory.UI;
using Features.Localization;
using Features.MainMenu;
using Features.PhoneCompanion;
using Features.RapidActions;
using Features.RunModes;
using Michsky.MUIP;
using ProjectExhibitionist.Core.Rendering;
using ProjectExhibitionist.Minimap;
using ProjectExhibitionist.PhotoBooth;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace ProjectExhibitionist.Core
{
	// Token: 0x02000A40 RID: 2624
	[DefaultExecutionOrder(-650)]
	public sealed class ControllerUINavigation : MonoBehaviour
	{
		// Token: 0x06004A7C RID: 19068 RVA: 0x001CE0B3 File Offset: 0x001CC2B3
		private CharacterCreatorManager CustomizationManager()
		{
			if (!this.CanUseCustomizationShortcuts())
			{
				return null;
			}
			return this._scope.GetComponentInChildren<CharacterCreatorManager>();
		}

		// Token: 0x06004A7D RID: 19069 RVA: 0x001CE0CA File Offset: 0x001CC2CA
		private bool CanUseCustomizationShortcuts()
		{
			return this._scope != null && !ControllerTextKeyboard.IsOpen && !GraphicsSettingsRuntimeUI.AnyOpen && this._scope.GetComponent<ControllerNavigationScope>() == null && !this._dropdownOpen;
		}

		// Token: 0x06004A7E RID: 19070 RVA: 0x001CE106 File Offset: 0x001CC306
		private MCCustomizationController MainCharacterCustomizationManager()
		{
			if (!this.CanUseCustomizationShortcuts())
			{
				return null;
			}
			return this._scope.GetComponentInChildren<MCCustomizationController>();
		}

		// Token: 0x06004A7F RID: 19071 RVA: 0x001CE11D File Offset: 0x001CC31D
		private bool HasCustomizationEditor()
		{
			return this.CustomizationManager() != null || this.MainCharacterCustomizationManager() != null;
		}

		// Token: 0x06004A80 RID: 19072 RVA: 0x001CE13C File Offset: 0x001CC33C
		private bool HandleCustomizationBack()
		{
			CharacterCreatorManager characterCreatorManager = this.CustomizationManager();
			MCCustomizationController mccustomizationController = this.MainCharacterCustomizationManager();
			Transform transform = ((mccustomizationController != null) ? mccustomizationController.transform : ((characterCreatorManager != null) ? characterCreatorManager.transform : null));
			CharacterCreatorHeader characterCreatorHeader = ((transform != null) ? transform.GetComponentInChildren<CharacterCreatorHeader>() : null);
			if (characterCreatorHeader == null || characterCreatorHeader.backButton == null || !ControllerUINavigation.IsUsable(characterCreatorHeader.backButton.gameObject))
			{
				return false;
			}
			ControllerUINavigation.Activate(characterCreatorHeader.backButton.gameObject, this._events);
			return true;
		}

		// Token: 0x06004A81 RID: 19073 RVA: 0x001CE1D0 File Offset: 0x001CC3D0
		private bool HandleAppShortcuts()
		{
			if (ControllerTextKeyboard.IsOpen || GraphicsSettingsRuntimeUI.AnyOpen || this._dropdownOpen)
			{
				return false;
			}
			CielOSRunModesApp cielOSRunModesApp = ((this._scope != null) ? this._scope.GetComponentInChildren<CielOSRunModesApp>() : null);
			if (cielOSRunModesApp != null && this._scope.GetComponent<ControllerNavigationScope>() == null && ControllerInput.GetButtonDown(ControllerButton.Start) && cielOSRunModesApp.ControllerStartRun(this._events))
			{
				ControllerInput.ConsumeUIInput();
				EscapeInputCoordinator.Consume(cielOSRunModesApp);
				return true;
			}
			if (CielOSController.AnyOpen && ControllerInput.GetButtonDown(ControllerButton.Select))
			{
				CielOSAppMenu cielOSAppMenu = Object.FindFirstObjectByType<CielOSAppMenu>();
				if (cielOSAppMenu != null && (this._scope == null || this._scope.GetComponent<ControllerNavigationScope>() == null || cielOSAppMenu.IsOpen))
				{
					cielOSAppMenu.Toggle();
					ControllerInput.ConsumeUIInput();
					this._nextScan = 0f;
					return true;
				}
			}
			GameObject gameObject = ((this._events != null) ? this._events.currentSelectedGameObject : null);
			CharacterCard characterCard = ((gameObject != null) ? gameObject.GetComponentInParent<CharacterCard>() : null);
			if (characterCard != null && this.CanUseCustomizationShortcuts() && ControllerInput.GetButtonDown(ControllerButton.North) && characterCard.ControllerEditPreset())
			{
				ControllerInput.ConsumeUIInput();
				this._nextScan = 0f;
				return true;
			}
			CharacterCreatorManager characterCreatorManager = this.CustomizationManager();
			MCCustomizationController mccustomizationController = this.MainCharacterCustomizationManager();
			if (characterCreatorManager == null && mccustomizationController == null)
			{
				return this.HandleDialogueReorder();
			}
			CharacterCreatorHeader componentInChildren = ((mccustomizationController != null) ? mccustomizationController.transform : characterCreatorManager.transform).GetComponentInChildren<CharacterCreatorHeader>();
			if (componentInChildren != null && componentInChildren.saveButton != null)
			{
				ControllerButtonHint.Bind(componentInChildren.saveButton.gameObject, "Y");
			}
			int num = (ControllerInput.GetButtonDown(ControllerButton.RightShoulder) ? 1 : (ControllerInput.GetButtonDown(ControllerButton.LeftShoulder) ? (-1) : 0));
			Button button = null;
			if (num != 0)
			{
				button = ((mccustomizationController != null) ? (ControllerInput.Modifier ? mccustomizationController.ControllerStepSubcategory(num) : mccustomizationController.ControllerStepCategory(num)) : (ControllerInput.Modifier ? characterCreatorManager.ControllerStepSubcategory(num) : characterCreatorManager.ControllerStepCategory(num)));
			}
			if (ControllerInput.GetButtonDown(ControllerButton.North))
			{
				CharacterCreatorHeader characterCreatorHeader = ((mccustomizationController != null) ? mccustomizationController.transform : characterCreatorManager.transform).GetComponentInChildren<CharacterCreatorHeader>();
				if (characterCreatorHeader == null)
				{
					characterCreatorHeader = this._scope.GetComponentInChildren<CharacterCreatorHeader>();
				}
				if (characterCreatorHeader != null && characterCreatorHeader.saveButton != null && ControllerUINavigation.IsUsable(characterCreatorHeader.saveButton.gameObject))
				{
					ControllerUINavigation.Activate(characterCreatorHeader.saveButton.gameObject, this._events);
				}
				ControllerInput.ConsumeUIInput();
				this._nextScan = 0f;
				return true;
			}
			if (num == 0)
			{
				return false;
			}
			if (button != null)
			{
				this.Select(button.gameObject);
				this.RememberSelection();
			}
			ControllerInput.ConsumeUIInput();
			this._nextScan = 0f;
			return true;
		}

		// Token: 0x06004A82 RID: 19074 RVA: 0x001CE4C0 File Offset: 0x001CC6C0
		private bool HandleDialogueReorder()
		{
			GameObject currentSelectedGameObject = this._events.currentSelectedGameObject;
			if (currentSelectedGameObject == null || this._scope == null || this._scope.GetComponent<ControllerNavigationScope>() != null)
			{
				return false;
			}
			DialogueLineRowDrag dialogueLineRowDrag = this.FindDialogueRow(currentSelectedGameObject.transform);
			if (dialogueLineRowDrag == null)
			{
				return false;
			}
			int num = (ControllerInput.GetButtonDown(ControllerButton.LeftShoulder) ? (-1) : (ControllerInput.GetButtonDown(ControllerButton.RightShoulder) ? 1 : 0));
			if (num == 0)
			{
				return false;
			}
			this.RememberSelection();
			dialogueLineRowDrag.MoveWithController(num);
			ControllerInput.ConsumeUIInput();
			this._nextScan = 0f;
			return true;
		}

		// Token: 0x06004A83 RID: 19075 RVA: 0x001CE558 File Offset: 0x001CC758
		private DialogueLineRowDrag FindDialogueRow(Transform selected)
		{
			Transform transform = selected;
			while (transform != null && transform != this._scope)
			{
				DialogueLineRowDrag componentInChildren = transform.GetComponentInChildren<DialogueLineRowDrag>();
				if (componentInChildren != null && componentInChildren.Row == transform)
				{
					return componentInChildren;
				}
				transform = transform.parent;
			}
			return null;
		}

		// Token: 0x17000C91 RID: 3217
		// (get) Token: 0x06004A84 RID: 19076 RVA: 0x001CE5A8 File Offset: 0x001CC7A8
		public static Transform CurrentScope
		{
			get
			{
				if (!(ControllerUINavigation._instance != null))
				{
					return null;
				}
				return ControllerUINavigation._instance._scope;
			}
		}

		// Token: 0x17000C92 RID: 3218
		// (get) Token: 0x06004A85 RID: 19077 RVA: 0x001CE5C4 File Offset: 0x001CC7C4
		public static GameObject CurrentSelection
		{
			get
			{
				if (!(ControllerUINavigation._instance != null) || !(ControllerUINavigation._instance._scope != null) || !(EventSystem.current != null) || !ControllerUINavigation._instance._targets.Contains(EventSystem.current.currentSelectedGameObject))
				{
					return null;
				}
				return EventSystem.current.currentSelectedGameObject;
			}
		}

		// Token: 0x17000C93 RID: 3219
		// (get) Token: 0x06004A86 RID: 19078 RVA: 0x001CE624 File Offset: 0x001CC824
		public static string ContextHelp
		{
			get
			{
				if (!(ControllerUINavigation._instance != null))
				{
					return string.Empty;
				}
				return ControllerUINavigation._instance.BuildContextHelp();
			}
		}

		// Token: 0x17000C94 RID: 3220
		// (get) Token: 0x06004A87 RID: 19079 RVA: 0x001CE644 File Offset: 0x001CC844
		private static bool ExclusiveSelfie
		{
			get
			{
				return SmartphoneController.IsAnySelfieActive && !ControllerTextKeyboard.IsOpen && !GraphicsSettingsRuntimeUI.AnyOpen && !PhotoPolaroidViewer.IsOpen && !PhoneVideoViewer.IsOpen && !ProceduralMiniMapController.IsMapExpanded && (!(InventoryDialogController.Instance != null) || !InventoryDialogController.Instance.IsOpen);
			}
		}

		// Token: 0x17000C95 RID: 3221
		// (get) Token: 0x06004A88 RID: 19080 RVA: 0x001CE69C File Offset: 0x001CC89C
		public static bool OwnsInput
		{
			get
			{
				return !GameplayInputGate.IsLoading && !ControllerUINavigation.ExclusiveSelfie && (!ControllerWorldPointer.IsWorldPointerActive || ControllerWorldPointer.WantsUIFocus || GraphicsSettingsRuntimeUI.AnyOpen) && (ControllerTextKeyboard.IsOpen || GameplayInputGate.IsFrontend || CielOSController.AnyOpen || GraphicsSettingsRuntimeUI.AnyOpen || PhotoPolaroidViewer.IsOpen || PhoneVideoViewer.IsOpen || (InventoryDialogController.Instance != null && InventoryDialogController.Instance.IsOpen) || (SmartphoneController.IsPhonePointerActive && !SmartphoneController.IsAnySelfieActive) || ProceduralMiniMapController.IsMapExpanded || (ControllerUINavigation._instance != null && Cursor.lockState == CursorLockMode.None && ControllerUINavigation._instance._scope != null && ControllerUINavigation._instance._scope.gameObject.activeInHierarchy && ControllerUINavigation._instance._targets.Count > 0));
			}
		}

		// Token: 0x06004A89 RID: 19081 RVA: 0x001CE790 File Offset: 0x001CC990
		public static bool CanControlPreview(Component owner)
		{
			if (owner == null || ControllerTextKeyboard.IsOpen || GraphicsSettingsRuntimeUI.AnyOpen)
			{
				return false;
			}
			GameObject gameObject = ((EventSystem.current != null) ? EventSystem.current.currentSelectedGameObject : null);
			if (gameObject != null && (gameObject.GetComponentInParent<ColorPickerControlExtended>() != null || gameObject.GetComponentInParent<MenuDialogShell>() != null))
			{
				return false;
			}
			CielOSWindow componentInParent = owner.GetComponentInParent<CielOSWindow>();
			return componentInParent == null || (ControllerUINavigation._instance != null && ControllerUINavigation._instance._scope == componentInParent.transform);
		}

		// Token: 0x06004A8A RID: 19082 RVA: 0x001CE82E File Offset: 0x001CCA2E
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
		private static void ResetRuntime()
		{
			ControllerUINavigation._instance = null;
		}

		// Token: 0x06004A8B RID: 19083 RVA: 0x001CE838 File Offset: 0x001CCA38
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
		private static void Bootstrap()
		{
			if (ControllerUINavigation._instance != null)
			{
				return;
			}
			GameObject gameObject = new GameObject("Controller UI Navigation");
			Object.DontDestroyOnLoad(gameObject);
			ControllerUINavigation._instance = gameObject.AddComponent<ControllerUINavigation>();
			ControllerGameplayBridge.UIFocusQuery = () => ControllerUINavigation.OwnsInput;
		}

		// Token: 0x06004A8C RID: 19084 RVA: 0x001CE894 File Offset: 0x001CCA94
		private void OnDisable()
		{
			this.RestoreEventNavigation();
			if (this._phoneFocusCanvas != null)
			{
				this._phoneFocusCanvas.enabled = false;
			}
			if (this._focusBorder != null)
			{
				this._focusBorder.gameObject.SetActive(false);
			}
			if (ControllerUINavigation._instance == this)
			{
				ControllerGameplayBridge.UIFocusQuery = null;
			}
		}

		// Token: 0x06004A8D RID: 19085 RVA: 0x001CE8F4 File Offset: 0x001CCAF4
		private void Update()
		{
			if (Application.isPlaying && !Application.isFocused)
			{
				this.RestoreEventNavigation();
				if (this._focusBorder != null)
				{
					this._focusBorder.gameObject.SetActive(false);
				}
				this._nextScan = 0f;
				return;
			}
			if (this._events != EventSystem.current)
			{
				this.RestoreEventNavigation();
				this._events = EventSystem.current;
				this._nextScan = 0f;
			}
			if (this._events == null || !ControllerInput.IsControllerActive || GameplayInputGate.IsLoading)
			{
				this.RestoreEventNavigation();
				if (this._focusBorder != null)
				{
					this._focusBorder.gameObject.SetActive(false);
				}
				return;
			}
			if (!this._ownsNavigation)
			{
				this._savedNavigation = this._events.sendNavigationEvents;
				this._events.sendNavigationEvents = false;
				this._ownsNavigation = true;
			}
			if (this._sceneHandle != SceneManager.GetActiveScene().handle)
			{
				this._sceneHandle = SceneManager.GetActiveScene().handle;
				this._remembered.Clear();
				this._focusMemory.Clear();
				this._scope = null;
				this._targets.Clear();
				this._nextScan = 0f;
			}
			if ((ProceduralMiniMapController.IsMapExpanded && !GraphicsSettingsRuntimeUI.AnyOpen) || ControllerUINavigation.DedicatedControllerSurfaceOpen() || ControllerUINavigation.ExclusiveSelfie || (ControllerWorldPointer.IsWorldPointerActive && !ControllerWorldPointer.WantsUIFocus && !GraphicsSettingsRuntimeUI.AnyOpen))
			{
				this._scope = null;
				this._targets.Clear();
				this._nextScan = 0f;
				if (this._focusBorder != null)
				{
					this._focusBorder.gameObject.SetActive(false);
				}
				return;
			}
			if (Time.unscaledTime >= this._nextScan)
			{
				this.RebuildTargets();
				this._nextScan = Time.unscaledTime + 0.25f;
			}
			if (this._targets.Count == 0)
			{
				if (this._focusBorder != null)
				{
					this._focusBorder.gameObject.SetActive(false);
				}
				if (!ControllerInput.UiInputConsumedThisFrame && ControllerInput.CancelPressed && this.HandleCancel())
				{
					ControllerInput.ConsumeUIInput();
					this._nextScan = 0f;
				}
				return;
			}
			this.ResolveFocus();
			if (ControllerInput.UiInputConsumedThisFrame)
			{
				return;
			}
			if (ControllerInput.CancelPressed && this.HandleCancel())
			{
				ControllerInput.ConsumeUIInput();
				this._nextScan = 0f;
				return;
			}
			if (SmartphoneController.IsControllerAdjustingPhoneView)
			{
				this.UpdateBorder();
				return;
			}
			if (this.HandleSliderInput())
			{
				this.UpdateBorder();
				this.RememberSelection();
				return;
			}
			if (this.HandleAppShortcuts())
			{
				return;
			}
			CielOSSnakeApp cielOSSnakeApp = ((this._scope != null) ? this._scope.GetComponentInChildren<CielOSSnakeApp>() : null);
			if (cielOSSnakeApp != null)
			{
				cielOSSnakeApp.HandleControllerInput(ControllerInput.Navigate, ControllerInput.SubmitPressed);
				if (this._focusBorder != null)
				{
					this._focusBorder.gameObject.SetActive(false);
				}
				if (ControllerInput.SubmitPressed)
				{
					ControllerInput.ConsumeUIInput();
				}
				return;
			}
			Vector2 vector = ControllerUINavigation.Cardinal(ControllerInput.Navigate);
			if (vector == Vector2.zero)
			{
				this._lastDirection = Vector2.zero;
				this._nextMove = 0f;
			}
			else if (vector != this._lastDirection || Time.unscaledTime >= this._nextMove)
			{
				this.Move(vector);
				this._nextMove = Time.unscaledTime + ((vector != this._lastDirection) ? 0.36f : 0.11f);
				this._lastDirection = vector;
			}
			if (ControllerInput.SubmitPressed)
			{
				this.SubmitCurrentSelection();
				ControllerInput.ConsumeUIInput();
				this._nextScan = 0f;
			}
			if (ControllerInput.GetButtonDown(ControllerButton.West))
			{
				GameObject gameObject = this._events.currentSelectedGameObject;
				CharacterCard characterCard = ((gameObject != null) ? gameObject.GetComponentInParent<CharacterCard>() : null);
				if (characterCard != null)
				{
					gameObject = characterCard.gameObject;
				}
				if (gameObject != null && (gameObject.GetComponent<CielOSRightClickable>() != null || gameObject.GetComponent<CielXRightClick>() != null || gameObject.GetComponent<CharacterCard>() != null))
				{
					PointerEventData pointerEventData = ControllerUINavigation.Pointer(gameObject, this._events);
					pointerEventData.button = PointerEventData.InputButton.Right;
					ExecuteEvents.Execute<IPointerClickHandler>(gameObject, pointerEventData, ExecuteEvents.pointerClickHandler);
					ControllerInput.ConsumeUIInput();
					this._nextScan = 0f;
				}
			}
			this.ScrollSelected(ControllerInput.Look);
			this.UpdateBorder();
			this.RememberSelection();
		}

		// Token: 0x06004A8E RID: 19086 RVA: 0x001CED2F File Offset: 0x001CCF2F
		private void RestoreEventNavigation()
		{
			if (this._ownsNavigation && this._events != null)
			{
				this._events.sendNavigationEvents = this._savedNavigation;
			}
			this._ownsNavigation = false;
		}

		// Token: 0x06004A8F RID: 19087 RVA: 0x001CED60 File Offset: 0x001CCF60
		private void RebuildTargets()
		{
			this._targets.Clear();
			this._unique.Clear();
			if (!GameplayInputGate.IsFrontend && !CielOSController.AnyOpen && !GraphicsSettingsRuntimeUI.AnyOpen && (!SmartphoneController.IsPhonePointerActive || SmartphoneController.IsAnySelfieActive) && !ControllerTextKeyboard.IsOpen && Cursor.lockState != CursorLockMode.None)
			{
				return;
			}
			this.CollectUIComponents();
			foreach (MonoBehaviour monoBehaviour in this._uiComponents)
			{
				if (!(monoBehaviour == null) && (monoBehaviour.isActiveAndEnabled || monoBehaviour is Selectable || monoBehaviour is DAButton || monoBehaviour is ButtonManager))
				{
					if (monoBehaviour is EventTrigger)
					{
						if (!ControllerUINavigation.HasActivationTrigger(monoBehaviour))
						{
							continue;
						}
					}
					else if (!(monoBehaviour is Selectable) && !(monoBehaviour is ISubmitHandler) && !(monoBehaviour is IPointerClickHandler))
					{
						continue;
					}
					GameObject gameObject = monoBehaviour.gameObject;
					if (this._unique.Add(gameObject) && ControllerUINavigation.IsFocusable(gameObject) && !ControllerUINavigation.IsExcluded(gameObject.transform) && !(monoBehaviour is ScrollRect) && !(monoBehaviour is Scrollbar))
					{
						RectTransform rectTransform = gameObject.transform as RectTransform;
						if (!(rectTransform == null) && (rectTransform.rect.width <= 1500f || rectTransform.rect.height <= 800f))
						{
							this._targets.Add(gameObject);
							Slider slider = monoBehaviour as Slider;
							if (slider != null)
							{
								ControllerSliderHint.Ensure(slider);
							}
						}
					}
				}
			}
			Transform scope = this.ResolveScope();
			if (scope != this._scope)
			{
				this.RememberSelection();
				this._scope = scope;
			}
			this._targets.RemoveAll((GameObject go) => go == null || (scope != null && !go.transform.IsChildOf(scope)));
		}

		// Token: 0x06004A90 RID: 19088 RVA: 0x001CEF64 File Offset: 0x001CD164
		private void CollectUIComponents()
		{
			this._uiComponents.Clear();
			this._uniqueComponents.Clear();
			this._scannedAncestors.Clear();
			foreach (BaseRaycaster baseRaycaster in RaycasterManager.GetRaycasters())
			{
				GraphicRaycaster graphicRaycaster = baseRaycaster as GraphicRaycaster;
				if (graphicRaycaster != null && graphicRaycaster.isActiveAndEnabled)
				{
					Canvas component = graphicRaycaster.GetComponent<Canvas>();
					if (!(component == null) && component.isActiveAndEnabled && (component.renderMode != RenderMode.WorldSpace || graphicRaycaster is PhoneScreenRaycaster))
					{
						graphicRaycaster.GetComponentsInChildren<MonoBehaviour>(false, this._componentBuffer);
						this.AddUIComponents();
						Transform transform = graphicRaycaster.transform.parent;
						while (transform != null && this._scannedAncestors.Add(transform))
						{
							transform.GetComponents<MonoBehaviour>(this._componentBuffer);
							this.AddUIComponents();
							transform = transform.parent;
						}
					}
				}
			}
		}

		// Token: 0x06004A91 RID: 19089 RVA: 0x001CF064 File Offset: 0x001CD264
		private void AddUIComponents()
		{
			foreach (MonoBehaviour monoBehaviour in this._componentBuffer)
			{
				if (monoBehaviour != null && this._uniqueComponents.Add(monoBehaviour))
				{
					this._uiComponents.Add(monoBehaviour);
				}
			}
		}

		// Token: 0x06004A92 RID: 19090 RVA: 0x001CF0D4 File Offset: 0x001CD2D4
		private IEnumerable<T> UIComponents<T>() where T : MonoBehaviour
		{
			foreach (MonoBehaviour monoBehaviour in this._uiComponents)
			{
				T t = monoBehaviour as T;
				if (t != null && t != null)
				{
					yield return t;
				}
			}
			List<MonoBehaviour>.Enumerator enumerator = default(List<MonoBehaviour>.Enumerator);
			yield break;
			yield break;
		}

		// Token: 0x06004A93 RID: 19091 RVA: 0x001CF0E4 File Offset: 0x001CD2E4
		private void Select(GameObject target)
		{
			if (target == null)
			{
				return;
			}
			this._events.SetSelectedGameObject(target);
			ControllerUINavigation.Reveal(target);
		}

		// Token: 0x06004A94 RID: 19092 RVA: 0x001CF104 File Offset: 0x001CD304
		private void Move(Vector2 direction)
		{
			GameObject currentSelectedGameObject = this._events.currentSelectedGameObject;
			if (currentSelectedGameObject == null)
			{
				return;
			}
			MainMenuArrowSelector componentInParent = currentSelectedGameObject.GetComponentInParent<MainMenuArrowSelector>();
			if (componentInParent != null && direction.x != 0f && ControllerUINavigation.IsUsable(currentSelectedGameObject))
			{
				if (direction.x > 0f)
				{
					componentInParent.Next();
					return;
				}
				componentInParent.Prev();
				return;
			}
			else
			{
				if (direction.y != 0f && this.MoveBetweenSettingsRows(currentSelectedGameObject, direction))
				{
					return;
				}
				GameObject gameObject = null;
				float num = float.PositiveInfinity;
				Vector2 vector = ControllerUINavigation.ScreenCenter(currentSelectedGameObject);
				ScrollRect componentInParent2 = currentSelectedGameObject.GetComponentInParent<ScrollRect>();
				foreach (GameObject gameObject2 in this._targets)
				{
					if (!(gameObject2 == currentSelectedGameObject) && ControllerUINavigation.IsFocusable(gameObject2) && ((componentInParent2 != null && componentInParent2.content != null && gameObject2.transform.IsChildOf(componentInParent2.content)) || this.IsVisible(gameObject2)))
					{
						float num2 = ControllerUINavigation.DirectionalScore(vector, ControllerUINavigation.ScreenCenter(gameObject2), direction);
						if (num2 < num)
						{
							num = num2;
							gameObject = gameObject2;
						}
					}
				}
				if (gameObject != null)
				{
					this.Select(gameObject);
				}
				return;
			}
		}

		// Token: 0x06004A95 RID: 19093 RVA: 0x001CF254 File Offset: 0x001CD454
		public static Vector2 Cardinal(Vector2 value)
		{
			if (value.sqrMagnitude < 0.25f)
			{
				return Vector2.zero;
			}
			if (Mathf.Abs(value.x) <= Mathf.Abs(value.y))
			{
				return new Vector2(0f, Mathf.Sign(value.y));
			}
			return new Vector2(Mathf.Sign(value.x), 0f);
		}

		// Token: 0x06004A96 RID: 19094 RVA: 0x001CF2B8 File Offset: 0x001CD4B8
		public static float DirectionalScore(Vector2 origin, Vector2 target, Vector2 direction)
		{
			Vector2 vector = target - origin;
			float num = Vector2.Dot(vector, direction);
			if (num < 2f)
			{
				return float.PositiveInfinity;
			}
			float num2 = Mathf.Abs(vector.x * direction.y - vector.y * direction.x);
			return num + num2 * 3f + num2 * num2 / Mathf.Max(1f, num);
		}

		// Token: 0x06004A97 RID: 19095 RVA: 0x001CF320 File Offset: 0x001CD520
		private void ScrollSelected(Vector2 value)
		{
			if (value.sqrMagnitude < 0.09f || this._events.currentSelectedGameObject == null)
			{
				return;
			}
			ColorPickerControlExtended componentInParent = this._events.currentSelectedGameObject.GetComponentInParent<ColorPickerControlExtended>();
			if (componentInParent != null && componentInParent.IsOpen)
			{
				if (ControllerInput.GetButton(ControllerButton.LeftTrigger))
				{
					componentInParent.SetHue(Mathf.Repeat(componentInParent.currentHue + value.x * Time.unscaledDeltaTime * 0.45f, 1f));
					return;
				}
				componentInParent.SetSV(Mathf.Clamp01(componentInParent.currentSat + value.x * Time.unscaledDeltaTime * 0.6f), Mathf.Clamp01(componentInParent.currentVal + value.y * Time.unscaledDeltaTime * 0.6f));
				return;
			}
			else
			{
				CielOSImageViewerApp cielOSImageViewerApp = ((this._scope != null) ? this._scope.GetComponentInChildren<CielOSImageViewerApp>() : null);
				if (cielOSImageViewerApp != null)
				{
					cielOSImageViewerApp.AdjustControllerView(value, ControllerInput.Modifier);
					return;
				}
				if (ControllerInput.Modifier || ControllerInput.GetButton(ControllerButton.LeftShoulder))
				{
					return;
				}
				ScrollRect scrollRect = this.ResolveScrollTarget(this._events.currentSelectedGameObject);
				if (scrollRect == null || !scrollRect.isActiveAndEnabled || scrollRect.content == null)
				{
					return;
				}
				RectTransform rectTransform = ((scrollRect.viewport != null) ? scrollRect.viewport : ((RectTransform)scrollRect.transform));
				float num = 650f * Time.unscaledDeltaTime;
				if (scrollRect.vertical)
				{
					scrollRect.verticalNormalizedPosition += value.y * num / Mathf.Max(1f, scrollRect.content.rect.height - rectTransform.rect.height);
				}
				if (scrollRect.horizontal)
				{
					scrollRect.horizontalNormalizedPosition += value.x * num / Mathf.Max(1f, scrollRect.content.rect.width - rectTransform.rect.width);
				}
				scrollRect.StopMovement();
				ControllerUINavigation.SyncControllerScroll(scrollRect);
				return;
			}
		}

		// Token: 0x06004A98 RID: 19096 RVA: 0x001CF530 File Offset: 0x001CD730
		private static void SyncControllerScroll(ScrollRect scroll)
		{
			MonoBehaviour[] components = scroll.GetComponents<MonoBehaviour>();
			for (int i = 0; i < components.Length; i++)
			{
				IControllerScrollSync controllerScrollSync = components[i] as IControllerScrollSync;
				if (controllerScrollSync != null)
				{
					controllerScrollSync.SyncControllerScroll();
				}
			}
		}

		// Token: 0x06004A99 RID: 19097 RVA: 0x001CF564 File Offset: 0x001CD764
		public static void Reveal(GameObject target)
		{
			if (target == null)
			{
				return;
			}
			ScrollRect componentInParent = target.GetComponentInParent<ScrollRect>();
			if (componentInParent == null || componentInParent.content == null)
			{
				return;
			}
			RectTransform rectTransform = ((componentInParent.viewport != null) ? componentInParent.viewport : (componentInParent.transform as RectTransform));
			RectTransform rectTransform2 = target.transform as RectTransform;
			if (rectTransform == null || rectTransform2 == null)
			{
				return;
			}
			Canvas.ForceUpdateCanvases();
			Bounds bounds = RectTransformUtility.CalculateRelativeRectTransformBounds(rectTransform, rectTransform2);
			Rect rect = rectTransform.rect;
			Vector2 zero = Vector2.zero;
			if (componentInParent.vertical)
			{
				zero.y = ((bounds.max.y > rect.yMax) ? (rect.yMax - bounds.max.y) : ((bounds.min.y < rect.yMin) ? (rect.yMin - bounds.min.y) : 0f));
			}
			if (componentInParent.horizontal)
			{
				zero.x = ((bounds.min.x < rect.xMin) ? (rect.xMin - bounds.min.x) : ((bounds.max.x > rect.xMax) ? (rect.xMax - bounds.max.x) : 0f));
			}
			if (zero.sqrMagnitude > 0.001f)
			{
				componentInParent.content.position += rectTransform.TransformVector(zero);
				componentInParent.StopMovement();
				ControllerUINavigation.SyncControllerScroll(componentInParent);
			}
		}

		// Token: 0x06004A9A RID: 19098 RVA: 0x001CF708 File Offset: 0x001CD908
		private void UpdateBorder()
		{
			GameObject currentSelectedGameObject = this._events.currentSelectedGameObject;
			if (currentSelectedGameObject == null || !(currentSelectedGameObject.transform is RectTransform))
			{
				return;
			}
			if (this._focusBorder == null)
			{
				GameObject gameObject = new GameObject("Controller Focus", new Type[]
				{
					typeof(RectTransform),
					typeof(CanvasRenderer),
					typeof(LayoutElement),
					typeof(ControllerFocusGraphic)
				});
				gameObject.GetComponent<LayoutElement>().ignoreLayout = true;
				gameObject.GetComponent<ControllerFocusGraphic>().raycastTarget = false;
				this._focusBorder = (RectTransform)gameObject.transform;
			}
			if (this._lastSelected != currentSelectedGameObject || this._focusBorder.parent != currentSelectedGameObject.transform)
			{
				this._focusBorder.SetParent(currentSelectedGameObject.transform, false);
				this._focusBorder.anchorMin = Vector2.zero;
				this._focusBorder.anchorMax = Vector2.one;
				this._focusBorder.offsetMin = new Vector2(2f, 2f);
				this._focusBorder.offsetMax = new Vector2(-2f, -2f);
				this._lastSelected = currentSelectedGameObject;
			}
			if (!this._focusBorder.gameObject.activeSelf)
			{
				this._focusBorder.gameObject.SetActive(true);
			}
			if (this._focusBorder.GetSiblingIndex() != this._focusBorder.parent.childCount - 1)
			{
				this._focusBorder.SetAsLastSibling();
			}
		}

		// Token: 0x06004A9B RID: 19099 RVA: 0x001CF894 File Offset: 0x001CDA94
		private static bool DedicatedControllerSurfaceOpen()
		{
			return !ControllerTextKeyboard.IsOpen && !GraphicsSettingsRuntimeUI.AnyOpen && ((InventoryDialogController.Instance != null && InventoryDialogController.Instance.IsOpen) || RapidActionsHUDController.AnyWheelOpen || PhotoBoothController.AnySessionActive || EmoteWheelUI.AnyWheelOpen || SelfieRadialMenuController.AnyWheelOpen || (NewspaperController.Instance != null && NewspaperController.Instance.IsShowing));
		}

		// Token: 0x06004A9C RID: 19100 RVA: 0x001CF904 File Offset: 0x001CDB04
		private static bool IsExcluded(Transform transform)
		{
			Transform transform2 = transform;
			while (transform2 != null)
			{
				if (transform2.GetComponent<InventoryDialogController>() != null || transform2.GetComponent<InventoryHUD>() != null || transform2.GetComponent<ClothHUDController>() != null || transform2.GetComponent<PhotoBoothPanel>() != null || transform2.GetComponent<EmoteWheelUI>() != null || transform2.GetComponent<RapidActionsHUDController>() != null || transform2.GetComponent<SelfieRadialMenuController>() != null)
				{
					return true;
				}
				transform2 = transform2.parent;
			}
			return false;
		}

		// Token: 0x06004A9D RID: 19101 RVA: 0x001CF98C File Offset: 0x001CDB8C
		private static bool HasActivationTrigger(MonoBehaviour component)
		{
			EventTrigger eventTrigger = component as EventTrigger;
			if (eventTrigger == null || eventTrigger.triggers == null)
			{
				return false;
			}
			foreach (EventTrigger.Entry entry in eventTrigger.triggers)
			{
				if (entry.eventID == EventTriggerType.PointerClick || entry.eventID == EventTriggerType.PointerDown)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06004A9E RID: 19102 RVA: 0x001CFA0C File Offset: 0x001CDC0C
		public static bool IsFocusable(GameObject target)
		{
			if (target == null || !target.activeInHierarchy || !(target.transform is RectTransform))
			{
				return false;
			}
			Transform transform = target.transform;
			while (transform != null)
			{
				bool flag = false;
				foreach (CanvasGroup canvasGroup in transform.GetComponents<CanvasGroup>())
				{
					if (canvasGroup.enabled)
					{
						if (canvasGroup.alpha < 0.05f || !canvasGroup.blocksRaycasts)
						{
							return false;
						}
						if (canvasGroup.ignoreParentGroups)
						{
							flag = true;
						}
					}
				}
				if (flag)
				{
					break;
				}
				transform = transform.parent;
			}
			Canvas componentInParent = target.GetComponentInParent<Canvas>();
			return componentInParent != null && componentInParent.isActiveAndEnabled;
		}

		// Token: 0x06004A9F RID: 19103 RVA: 0x001CFABC File Offset: 0x001CDCBC
		public static bool IsUsable(GameObject target)
		{
			if (target == null || !target.activeInHierarchy || !(target.transform is RectTransform))
			{
				return false;
			}
			Selectable component = target.GetComponent<Selectable>();
			if (component != null && (!component.isActiveAndEnabled || !component.IsInteractable()))
			{
				return false;
			}
			ButtonManager component2 = target.GetComponent<ButtonManager>();
			bool flag = component != null && component.isActiveAndEnabled;
			if (component2 != null && !flag && (!component2.isActiveAndEnabled || !component2.isInteractable))
			{
				return false;
			}
			DAButton component3 = target.GetComponent<DAButton>();
			if (component3 != null && !flag && (!component3.isActiveAndEnabled || !component3.Interactable))
			{
				return false;
			}
			CustomDropdown component4 = target.GetComponent<CustomDropdown>();
			if (component4 != null && (!component4.isActiveAndEnabled || !component4.isInteractable))
			{
				return false;
			}
			Transform transform = target.transform;
			while (transform != null)
			{
				bool flag2 = false;
				foreach (CanvasGroup canvasGroup in transform.GetComponents<CanvasGroup>())
				{
					if (canvasGroup.enabled)
					{
						if (canvasGroup.alpha < 0.05f || !canvasGroup.interactable || !canvasGroup.blocksRaycasts)
						{
							return false;
						}
						if (canvasGroup.ignoreParentGroups)
						{
							flag2 = true;
						}
					}
				}
				if (flag2)
				{
					break;
				}
				transform = transform.parent;
			}
			Canvas componentInParent = target.GetComponentInParent<Canvas>();
			return componentInParent != null && componentInParent.isActiveAndEnabled;
		}

		// Token: 0x06004AA0 RID: 19104 RVA: 0x001CFC28 File Offset: 0x001CDE28
		private bool IsVisible(GameObject target)
		{
			if (!ControllerUINavigation.IsFocusable(target))
			{
				return false;
			}
			Vector2 vector = ControllerUINavigation.ScreenCenter(target);
			if (vector.x < 0f || vector.x > (float)Screen.width || vector.y < 0f || vector.y > (float)Screen.height)
			{
				return false;
			}
			PointerEventData pointerEventData = new PointerEventData(this._events)
			{
				position = vector
			};
			this._hits.Clear();
			this._events.RaycastAll(pointerEventData, this._hits);
			foreach (RaycastResult raycastResult in this._hits)
			{
				if (!(raycastResult.gameObject == null) && !(raycastResult.module == null) && raycastResult.module is GraphicRaycaster)
				{
					return raycastResult.gameObject == target || raycastResult.gameObject.transform.IsChildOf(target.transform);
				}
			}
			return false;
		}

		// Token: 0x06004AA1 RID: 19105 RVA: 0x001CFD48 File Offset: 0x001CDF48
		private static Canvas SortingCanvas(Transform transform)
		{
			Canvas canvas = transform.GetComponentInParent<Canvas>();
			if (canvas == null)
			{
				return null;
			}
			while (!canvas.overrideSorting && canvas.transform.parent != null)
			{
				Canvas componentInParent = canvas.transform.parent.GetComponentInParent<Canvas>();
				if (componentInParent == null)
				{
					break;
				}
				canvas = componentInParent;
			}
			return canvas;
		}

		// Token: 0x06004AA2 RID: 19106 RVA: 0x001CFDA0 File Offset: 0x001CDFA0
		private static int CompareDrawOrder(Transform left, Transform right)
		{
			Canvas canvas = ControllerUINavigation.SortingCanvas(left);
			Canvas canvas2 = ControllerUINavigation.SortingCanvas(right);
			if (canvas != null && canvas2 != null)
			{
				int num = SortingLayer.GetLayerValueFromID(canvas.sortingLayerID).CompareTo(SortingLayer.GetLayerValueFromID(canvas2.sortingLayerID));
				if (num != 0)
				{
					return num;
				}
				int num2 = canvas.sortingOrder.CompareTo(canvas2.sortingOrder);
				if (num2 != 0)
				{
					return num2;
				}
			}
			int i = 0;
			int j = 0;
			Transform transform = left;
			while (transform != null)
			{
				i++;
				transform = transform.parent;
			}
			Transform transform2 = right;
			while (transform2 != null)
			{
				j++;
				transform2 = transform2.parent;
			}
			while (i > j)
			{
				left = left.parent;
				i--;
			}
			while (j > i)
			{
				right = right.parent;
				j--;
			}
			while (left != null && right != null && left.parent != right.parent)
			{
				left = left.parent;
				right = right.parent;
			}
			if (!(left != null) || !(right != null))
			{
				return 0;
			}
			return left.GetSiblingIndex().CompareTo(right.GetSiblingIndex());
		}

		// Token: 0x06004AA3 RID: 19107 RVA: 0x001CFED4 File Offset: 0x001CE0D4
		private static Vector2 ScreenCenter(GameObject target)
		{
			RectTransform rectTransform = (RectTransform)target.transform;
			Canvas componentInParent = target.GetComponentInParent<Canvas>();
			return RectTransformUtility.WorldToScreenPoint((componentInParent != null && componentInParent.renderMode != RenderMode.ScreenSpaceOverlay) ? componentInParent.worldCamera : null, rectTransform.TransformPoint(rectTransform.rect.center));
		}

		// Token: 0x06004AA4 RID: 19108 RVA: 0x001CFF2C File Offset: 0x001CE12C
		private static PointerEventData Pointer(GameObject target, EventSystem events)
		{
			PointerEventData pointerEventData = new PointerEventData(events)
			{
				position = ControllerUINavigation.ScreenCenter(target),
				button = PointerEventData.InputButton.Left,
				clickCount = 1,
				eligibleForClick = true,
				pointerPress = target,
				rawPointerPress = target
			};
			Canvas componentInParent = target.GetComponentInParent<Canvas>();
			GraphicRaycaster graphicRaycaster = ((componentInParent != null) ? componentInParent.GetComponent<GraphicRaycaster>() : null);
			pointerEventData.pointerPressRaycast = new RaycastResult
			{
				gameObject = target,
				module = graphicRaycaster,
				screenPosition = pointerEventData.position
			};
			pointerEventData.pointerCurrentRaycast = pointerEventData.pointerPressRaycast;
			return pointerEventData;
		}

		// Token: 0x06004AA5 RID: 19109 RVA: 0x001CFFC4 File Offset: 0x001CE1C4
		public static void Activate(GameObject target, EventSystem events)
		{
			if (target == null || events == null || !ControllerUINavigation.IsFocusable(target))
			{
				return;
			}
			if (!ControllerUINavigation.IsUsable(target))
			{
				MainMenuUiFeedbackAudio.PlayDenied();
				return;
			}
			Object component = target.GetComponent<TMP_InputField>();
			InputField component2 = target.GetComponent<InputField>();
			if (component != null || component2 != null)
			{
				ControllerTextKeyboard.Open(target);
				return;
			}
			bool flag = false;
			foreach (MonoBehaviour monoBehaviour in target.GetComponents<MonoBehaviour>())
			{
				if (!(monoBehaviour == null) && monoBehaviour.isActiveAndEnabled && monoBehaviour is ISubmitHandler)
				{
					EventTrigger eventTrigger = monoBehaviour as EventTrigger;
					if (eventTrigger == null)
					{
						flag = true;
						break;
					}
					if (eventTrigger.triggers != null)
					{
						using (List<EventTrigger.Entry>.Enumerator enumerator = eventTrigger.triggers.GetEnumerator())
						{
							while (enumerator.MoveNext())
							{
								if (enumerator.Current.eventID == EventTriggerType.Submit)
								{
									flag = true;
								}
							}
						}
					}
				}
			}
			if (flag)
			{
				ExecuteEvents.Execute<ISubmitHandler>(target, new BaseEventData(events), ExecuteEvents.submitHandler);
				return;
			}
			PointerEventData pointerEventData = ControllerUINavigation.Pointer(target, events);
			ExecuteEvents.Execute<IPointerDownHandler>(target, pointerEventData, ExecuteEvents.pointerDownHandler);
			ExecuteEvents.Execute<IPointerUpHandler>(target, pointerEventData, ExecuteEvents.pointerUpHandler);
			ExecuteEvents.Execute<IPointerClickHandler>(target, pointerEventData, ExecuteEvents.pointerClickHandler);
		}

		// Token: 0x06004AA6 RID: 19110 RVA: 0x001D0114 File Offset: 0x001CE314
		private bool HandleCancel()
		{
			if (ControllerTextKeyboard.IsOpen)
			{
				ControllerTextKeyboard.Cancel();
				return true;
			}
			if (this._muipDropdown != null && this._muipDropdown.isOn)
			{
				this._muipDropdown.Animate();
				return true;
			}
			if (this._dropdownOpen && this._dropdownOwner != null)
			{
				TMP_Dropdown component = this._dropdownOwner.GetComponent<TMP_Dropdown>();
				if (component != null)
				{
					component.Hide();
					return true;
				}
				Dropdown component2 = this._dropdownOwner.GetComponent<Dropdown>();
				if (component2 != null)
				{
					component2.Hide();
					return true;
				}
			}
			GameObject gameObject = this._events.currentSelectedGameObject;
			if (gameObject != null && this._scope != null && !gameObject.transform.IsChildOf(this._scope))
			{
				gameObject = null;
			}
			if (ControllerUINavigation.DispatchCancel(gameObject, this._events))
			{
				return true;
			}
			ControllerNavigationScope controllerNavigationScope = ((this._scope != null) ? this._scope.GetComponent<ControllerNavigationScope>() : null);
			if (controllerNavigationScope != null && controllerNavigationScope.Cancel != null)
			{
				controllerNavigationScope.Cancel();
				return true;
			}
			if (this._scope != null && this._scope.GetComponentInParent<MenuDialogShell>() != null)
			{
				return false;
			}
			if (GraphicsSettingsRuntimeUI.AnyOpen)
			{
				return false;
			}
			if (PhotoPolaroidViewer.IsOpen || PhoneVideoViewer.IsOpen)
			{
				return false;
			}
			CielOSRunModesApp cielOSRunModesApp = ((this._scope != null) ? this._scope.GetComponentInChildren<CielOSRunModesApp>() : null);
			if (cielOSRunModesApp != null && cielOSRunModesApp.ControllerBack())
			{
				return true;
			}
			if (this.HandleCustomizationBack())
			{
				return true;
			}
			GameObject gameObject2 = null;
			foreach (GameObject gameObject3 in this._targets)
			{
				if (ControllerUINavigation.IsUsable(gameObject3) && this.IsVisible(gameObject3) && ControllerUINavigation.IsBackControl(gameObject3.name) && (gameObject2 == null || ControllerUINavigation.CompareDrawOrder(gameObject3.transform, gameObject2.transform) > 0))
				{
					gameObject2 = gameObject3;
				}
			}
			if (gameObject2 != null)
			{
				ControllerUINavigation.Activate(gameObject2, this._events);
				return true;
			}
			CielOSWindow cielOSWindow = ((this._scope != null) ? this._scope.GetComponentInParent<CielOSWindow>() : null);
			if (cielOSWindow != null)
			{
				cielOSWindow.Close();
				return true;
			}
			return false;
		}

		// Token: 0x06004AA7 RID: 19111 RVA: 0x001D0374 File Offset: 0x001CE574
		public static bool DispatchCancel(GameObject selected, EventSystem events)
		{
			if (selected == null || events == null)
			{
				return false;
			}
			Transform transform = selected.transform;
			while (transform != null)
			{
				bool flag = false;
				foreach (MonoBehaviour monoBehaviour in transform.GetComponents<MonoBehaviour>())
				{
					if (!(monoBehaviour == null) && monoBehaviour.isActiveAndEnabled && monoBehaviour is ICancelHandler)
					{
						if (monoBehaviour is TMP_Dropdown || monoBehaviour is Dropdown)
						{
							Transform transform2 = monoBehaviour.transform.Find("Dropdown List");
							if (transform2 == null || !transform2.gameObject.activeInHierarchy)
							{
								goto IL_00F2;
							}
						}
						EventTrigger eventTrigger = monoBehaviour as EventTrigger;
						if (eventTrigger == null)
						{
							flag = true;
							break;
						}
						if (eventTrigger.triggers != null)
						{
							using (List<EventTrigger.Entry>.Enumerator enumerator = eventTrigger.triggers.GetEnumerator())
							{
								while (enumerator.MoveNext())
								{
									if (enumerator.Current.eventID == EventTriggerType.Cancel)
									{
										flag = true;
									}
								}
							}
						}
					}
					IL_00F2:;
				}
				if (flag)
				{
					ExecuteEvents.Execute<ICancelHandler>(transform.gameObject, new BaseEventData(events), ExecuteEvents.cancelHandler);
					return true;
				}
				transform = transform.parent;
			}
			return false;
		}

		// Token: 0x06004AA8 RID: 19112 RVA: 0x001D04C0 File Offset: 0x001CE6C0
		public static bool IsBackControl(string name)
		{
			if (string.IsNullOrEmpty(name))
			{
				return false;
			}
			string text = name.Replace(" ", "").Replace("_", "").ToLowerInvariant();
			return text == "back" || text == "backbutton" || text == "buttonback" || text == "close" || text == "closebutton" || text == "buttonclose" || text == "cancel" || text == "cancelbutton" || text == "buttoncancel";
		}

		// Token: 0x06004AA9 RID: 19113 RVA: 0x001D0574 File Offset: 0x001CE774
		public static void RequestFocus(GameObject target)
		{
			if (target == null)
			{
				return;
			}
			if (EventSystem.current != null)
			{
				EventSystem.current.SetSelectedGameObject(target);
			}
			if (ControllerUINavigation._instance == null)
			{
				return;
			}
			ControllerUINavigation._instance._nextScan = 0f;
			ControllerUINavigation._instance._recoveredFocusFrame = Time.frameCount;
		}

		// Token: 0x06004AAA RID: 19114 RVA: 0x001D05D0 File Offset: 0x001CE7D0
		internal static void PrepareScope(ControllerNavigationScope popup)
		{
			if (ControllerUINavigation._instance == null || popup == null)
			{
				return;
			}
			ControllerUINavigation._instance._focusMemory.Remove(popup.transform);
			ControllerUINavigation._instance._nextScan = 0f;
			GameObject gameObject = ((ControllerUINavigation._instance._events != null) ? ControllerUINavigation._instance._events.currentSelectedGameObject : null);
			if (ControllerInput.IsActive && gameObject != null && gameObject.transform.IsChildOf(popup.transform))
			{
				ControllerUINavigation._instance._events.SetSelectedGameObject(null);
			}
		}

		// Token: 0x06004AAB RID: 19115 RVA: 0x001D0674 File Offset: 0x001CE874
		private Transform ResolveScope()
		{
			if (ControllerTextKeyboard.Root != null)
			{
				return ControllerTextKeyboard.Root;
			}
			this.CollectUIComponents();
			Transform transform = null;
			GameObject gameObject = null;
			this._muipDropdown = null;
			foreach (TMP_Dropdown tmp_Dropdown in this.UIComponents<TMP_Dropdown>())
			{
				this.ConsiderDropdown(tmp_Dropdown.transform, ref transform, ref gameObject);
			}
			foreach (Dropdown dropdown in this.UIComponents<Dropdown>())
			{
				this.ConsiderDropdown(dropdown.transform, ref transform, ref gameObject);
			}
			foreach (CustomDropdown customDropdown in this.UIComponents<CustomDropdown>())
			{
				Transform transform2 = ((customDropdown.listRect != null) ? customDropdown.listRect : customDropdown.itemParent);
				if (customDropdown.isActiveAndEnabled && customDropdown.isOn && !(transform2 == null) && transform2.gameObject.activeInHierarchy && (transform == null || ControllerUINavigation.CompareDrawOrder(transform2, transform) > 0))
				{
					transform = transform2;
					gameObject = customDropdown.gameObject;
					this._muipDropdown = customDropdown;
				}
			}
			this._dropdownOpen = transform != null;
			if (this._dropdownOpen)
			{
				this._dropdownOwner = gameObject;
				return transform;
			}
			ControllerNavigationScope controllerNavigationScope = null;
			foreach (ControllerNavigationScope controllerNavigationScope2 in this.UIComponents<ControllerNavigationScope>())
			{
				if (controllerNavigationScope2.isActiveAndEnabled && (controllerNavigationScope2.WaitForInitialFocus || this.ScopeHasVisibleControl(controllerNavigationScope2.transform) || this.IsVisible(controllerNavigationScope2.gameObject)) && (controllerNavigationScope == null || controllerNavigationScope2.Priority > controllerNavigationScope.Priority || (controllerNavigationScope2.Priority == controllerNavigationScope.Priority && ControllerUINavigation.CompareDrawOrder(controllerNavigationScope2.transform, controllerNavigationScope.transform) > 0)))
				{
					controllerNavigationScope = controllerNavigationScope2;
				}
			}
			if (controllerNavigationScope != null && controllerNavigationScope.Priority >= 0)
			{
				return controllerNavigationScope.transform;
			}
			MenuDialogShell menuDialogShell = null;
			foreach (MenuDialogShell menuDialogShell2 in this.UIComponents<MenuDialogShell>())
			{
				if (menuDialogShell2.isActiveAndEnabled && this.ScopeHasVisibleControl(menuDialogShell2.transform) && (menuDialogShell == null || ControllerUINavigation.CompareDrawOrder(menuDialogShell2.transform, menuDialogShell.transform) > 0))
				{
					menuDialogShell = menuDialogShell2;
				}
			}
			if (menuDialogShell != null)
			{
				return menuDialogShell.transform;
			}
			if (controllerNavigationScope != null)
			{
				return controllerNavigationScope.transform;
			}
			CielOSWindow cielOSWindow = null;
			foreach (CielOSWindowManager cielOSWindowManager in this.UIComponents<CielOSWindowManager>())
			{
				CielOSWindow focusedWindow = cielOSWindowManager.FocusedWindow;
				if (focusedWindow != null && (cielOSWindow == null || ControllerUINavigation.CompareDrawOrder(focusedWindow.transform, cielOSWindow.transform) > 0))
				{
					cielOSWindow = focusedWindow;
				}
			}
			if (cielOSWindow != null && !GraphicsSettingsRuntimeUI.AnyOpen)
			{
				return cielOSWindow.transform;
			}
			GameObject gameObject2 = null;
			foreach (GameObject gameObject3 in this._targets)
			{
				if (this.IsVisible(gameObject3) && (gameObject2 == null || ControllerUINavigation.CompareDrawOrder(gameObject3.transform, gameObject2.transform) > 0))
				{
					gameObject2 = gameObject3;
				}
			}
			Canvas canvas = ((gameObject2 != null) ? ControllerUINavigation.SortingCanvas(gameObject2.transform) : null);
			if (!(canvas != null))
			{
				return null;
			}
			return canvas.transform;
		}

		// Token: 0x06004AAC RID: 19116 RVA: 0x001D0A94 File Offset: 0x001CEC94
		private void ConsiderDropdown(Transform owner, ref Transform top, ref GameObject source)
		{
			Transform transform = owner.Find("Dropdown List");
			if (transform == null || !transform.gameObject.activeInHierarchy)
			{
				return;
			}
			if (top == null || ControllerUINavigation.CompareDrawOrder(transform, top) > 0)
			{
				top = transform;
				source = owner.gameObject;
			}
		}

		// Token: 0x06004AAD RID: 19117 RVA: 0x001D0AE4 File Offset: 0x001CECE4
		private bool ScopeHasVisibleControl(Transform scope)
		{
			foreach (GameObject gameObject in this._targets)
			{
				if (gameObject != null && gameObject.transform.IsChildOf(scope) && this.IsVisible(gameObject))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06004AAE RID: 19118 RVA: 0x001D0B58 File Offset: 0x001CED58
		private void RememberSelection()
		{
			GameObject gameObject = ((this._events != null) ? this._events.currentSelectedGameObject : null);
			if (this._scope == null || gameObject == null || !gameObject.transform.IsChildOf(this._scope) || !ControllerUINavigation.IsFocusable(gameObject) || this.CrossesFocusBoundary(gameObject.transform))
			{
				return;
			}
			this._remembered[this._scope] = gameObject;
			Dictionary<Transform, ControllerUINavigation.FocusMemory> focusMemory = this._focusMemory;
			Transform scope = this._scope;
			ControllerUINavigation.FocusMemory focusMemory2 = new ControllerUINavigation.FocusMemory();
			focusMemory2.Target = gameObject;
			focusMemory2.Path = ControllerUINavigation.RelativePath(this._scope, gameObject.transform);
			focusMemory2.Position = ControllerUINavigation.ScreenCenter(gameObject);
			CharacterCard componentInParent = gameObject.GetComponentInParent<CharacterCard>();
			focusMemory2.PresetId = ((componentInParent != null) ? componentInParent.GetPresetId() : null);
			focusMemory[scope] = focusMemory2;
		}

		// Token: 0x06004AAF RID: 19119 RVA: 0x001D0C30 File Offset: 0x001CEE30
		private bool CrossesFocusBoundary(Transform selected)
		{
			Transform transform = selected;
			while (transform != null && transform != this._scope)
			{
				if (transform.GetComponent<ControllerNavigationScope>() != null || transform.GetComponent<MenuDialogShell>() != null || transform.GetComponent<CielOSWindow>() != null || transform == ControllerTextKeyboard.Root)
				{
					return true;
				}
				if (transform.name == "Dropdown List" && transform.parent != null && (transform.parent.GetComponent<TMP_Dropdown>() != null || transform.parent.GetComponent<Dropdown>() != null))
				{
					return true;
				}
				CustomDropdown componentInParent = transform.GetComponentInParent<CustomDropdown>();
				Transform transform2 = ((componentInParent != null) ? ((componentInParent.listRect != null) ? componentInParent.listRect : componentInParent.itemParent) : null);
				if (componentInParent != null && componentInParent.isOn && transform2 == transform)
				{
					return true;
				}
				transform = transform.parent;
			}
			return false;
		}

		// Token: 0x06004AB0 RID: 19120 RVA: 0x001D0D34 File Offset: 0x001CEF34
		private static string RelativePath(Transform root, Transform target)
		{
			string text = target.name;
			Transform transform = target.parent;
			while (transform != null && transform != root)
			{
				text = transform.name + "/" + text;
				transform = transform.parent;
			}
			if (!(target == root))
			{
				return text;
			}
			return string.Empty;
		}

		// Token: 0x06004AB1 RID: 19121 RVA: 0x001D0D8C File Offset: 0x001CEF8C
		private void ResolveFocus()
		{
			GameObject currentSelectedGameObject = this._events.currentSelectedGameObject;
			if (!this._dropdownOpen && this._dropdownOwner != null)
			{
				GameObject dropdownOwner = this._dropdownOwner;
				this._dropdownOwner = null;
				if (this.IsFocusCandidate(dropdownOwner))
				{
					this.Select(dropdownOwner);
					this._recoveredFocusFrame = Time.frameCount;
					return;
				}
			}
			if (this.IsFocusCandidate(currentSelectedGameObject))
			{
				return;
			}
			GameObject gameObject = null;
			if (this._muipDropdown != null && this._muipDropdown.itemParent != null)
			{
				int selectedItemIndex = this._muipDropdown.selectedItemIndex;
				if (selectedItemIndex >= 0 && selectedItemIndex < this._muipDropdown.itemParent.childCount)
				{
					GameObject gameObject2 = this._muipDropdown.itemParent.GetChild(selectedItemIndex).gameObject;
					if (this._targets.Contains(gameObject2) && ControllerUINavigation.IsFocusable(gameObject2))
					{
						this.Select(gameObject2);
						this._recoveredFocusFrame = Time.frameCount;
						return;
					}
				}
			}
			ControllerUINavigation.FocusMemory focusMemory;
			if (this._scope != null && this._focusMemory.TryGetValue(this._scope, out focusMemory))
			{
				if (this.IsFocusCandidate(focusMemory.Target))
				{
					gameObject = focusMemory.Target;
				}
				if (gameObject == null && !string.IsNullOrEmpty(focusMemory.PresetId))
				{
					foreach (GameObject gameObject3 in this._targets)
					{
						CharacterCard characterCard = ((gameObject3 != null) ? gameObject3.GetComponentInParent<CharacterCard>() : null);
						if (characterCard != null && characterCard.GetPresetId() == focusMemory.PresetId && this.IsFocusCandidate(gameObject3))
						{
							gameObject = gameObject3;
							break;
						}
					}
					CharacterPresetPanelController componentInChildren = this._scope.GetComponentInChildren<CharacterPresetPanelController>();
					if (gameObject == null && componentInChildren != null && componentInChildren.IsBuildingGrid)
					{
						this._events.SetSelectedGameObject(null);
						return;
					}
				}
				Transform transform = ((gameObject == null && !string.IsNullOrEmpty(focusMemory.Path)) ? this._scope.Find(focusMemory.Path) : null);
				if (transform != null && this.IsFocusCandidate(transform.gameObject))
				{
					gameObject = transform.gameObject;
				}
				if (gameObject == null)
				{
					float num = float.PositiveInfinity;
					foreach (GameObject gameObject4 in this._targets)
					{
						if (this.IsFocusCandidate(gameObject4))
						{
							float sqrMagnitude = (ControllerUINavigation.ScreenCenter(gameObject4) - focusMemory.Position).sqrMagnitude;
							if (sqrMagnitude < num)
							{
								num = sqrMagnitude;
								gameObject = gameObject4;
							}
						}
					}
				}
			}
			ControllerNavigationScope controllerNavigationScope = ((this._scope != null) ? this._scope.GetComponent<ControllerNavigationScope>() : null);
			if (gameObject == null && controllerNavigationScope != null && this.IsFocusCandidate(controllerNavigationScope.InitialFocus))
			{
				gameObject = controllerNavigationScope.InitialFocus;
			}
			if (gameObject == null && controllerNavigationScope != null && controllerNavigationScope.WaitForInitialFocus && controllerNavigationScope.InitialFocus != null)
			{
				this._events.SetSelectedGameObject(null);
				return;
			}
			if (gameObject == null && this._scope != null)
			{
				RectTransform[] componentsInChildren = this._scope.GetComponentsInChildren<RectTransform>();
				int i = 0;
				while (i < componentsInChildren.Length)
				{
					RectTransform rectTransform = componentsInChildren[i];
					if (!(rectTransform.name != "btn - Go to Home"))
					{
						if (this.IsFocusCandidate(rectTransform.gameObject))
						{
							gameObject = rectTransform.gameObject;
							break;
						}
						this._events.SetSelectedGameObject(null);
						return;
					}
					else
					{
						i++;
					}
				}
			}
			if (gameObject == null)
			{
				CielOSWindow cielOSWindow = ((this._scope != null) ? this._scope.GetComponent<CielOSWindow>() : null);
				bool flag = cielOSWindow != null && cielOSWindow.Content != null && this.ScopeHasVisibleControl(cielOSWindow.Content);
				Vector2 vector = Vector2.zero;
				foreach (GameObject gameObject5 in this._targets)
				{
					if (this.IsFocusCandidate(gameObject5) && (!flag || gameObject5.transform.IsChildOf(cielOSWindow.Content)))
					{
						Vector2 vector2 = ControllerUINavigation.ScreenCenter(gameObject5);
						if (gameObject == null || vector2.y > vector.y + 10f || (Mathf.Abs(vector2.y - vector.y) <= 10f && vector2.x < vector.x))
						{
							gameObject = gameObject5;
							vector = vector2;
						}
					}
				}
			}
			if (gameObject == null)
			{
				return;
			}
			this.Select(gameObject);
			this._recoveredFocusFrame = Time.frameCount;
		}

		// Token: 0x06004AB2 RID: 19122 RVA: 0x001D128C File Offset: 0x001CF48C
		private bool IsFocusCandidate(GameObject target)
		{
			return target != null && this._targets.Contains(target) && ControllerUINavigation.IsFocusable(target) && this.IsVisible(target);
		}

		// Token: 0x06004AB3 RID: 19123 RVA: 0x001D12B8 File Offset: 0x001CF4B8
		private void SubmitCurrentSelection()
		{
			if (this._recoveredFocusFrame == Time.frameCount)
			{
				return;
			}
			GameObject currentSelectedGameObject = this._events.currentSelectedGameObject;
			if (!this.IsFocusCandidate(currentSelectedGameObject))
			{
				return;
			}
			this.RememberSelection();
			ControllerUINavigation.Activate(currentSelectedGameObject, this._events);
		}

		// Token: 0x06004AB4 RID: 19124 RVA: 0x001D12FC File Offset: 0x001CF4FC
		private bool MoveBetweenSettingsRows(GameObject selected, Vector2 direction)
		{
			ControllerNavigationRow componentInParent = selected.GetComponentInParent<ControllerNavigationRow>();
			if (componentInParent == null)
			{
				return false;
			}
			GameObject gameObject = null;
			float num = float.PositiveInfinity;
			float num2 = float.PositiveInfinity;
			Vector2 vector = ControllerUINavigation.ScreenCenter(componentInParent.gameObject);
			foreach (GameObject gameObject2 in this._targets)
			{
				ControllerNavigationRow componentInParent2 = gameObject2.GetComponentInParent<ControllerNavigationRow>();
				if (!(componentInParent2 == null) && !(componentInParent2 == componentInParent) && ControllerUINavigation.AreInSameSettingsRowGroup(componentInParent, componentInParent2) && ControllerUINavigation.IsFocusable(gameObject2))
				{
					float num3 = Vector2.Dot(ControllerUINavigation.ScreenCenter(componentInParent2.gameObject) - vector, direction);
					if (num3 >= 2f)
					{
						ScrollRect componentInParent3 = selected.GetComponentInParent<ScrollRect>();
						if (this.IsVisible(gameObject2) || (!(componentInParent3 == null) && gameObject2.transform.IsChildOf(componentInParent3.content)))
						{
							float num4 = Mathf.Abs(ControllerUINavigation.ScreenCenter(gameObject2).x - ControllerUINavigation.ScreenCenter(selected).x);
							if (num3 < num - 2f || (Mathf.Abs(num3 - num) < 2f && num4 < num2))
							{
								gameObject = gameObject2;
								num = num3;
								num2 = num4;
							}
						}
					}
				}
			}
			if (gameObject == null)
			{
				return false;
			}
			this.Select(gameObject);
			return true;
		}

		// Token: 0x06004AB5 RID: 19125 RVA: 0x001D146C File Offset: 0x001CF66C
		private static bool AreInSameSettingsRowGroup(ControllerNavigationRow first, ControllerNavigationRow second)
		{
			ControllerNavigationScope componentInParent = first.GetComponentInParent<ControllerNavigationScope>();
			ControllerNavigationScope componentInParent2 = second.GetComponentInParent<ControllerNavigationScope>();
			if (componentInParent != null || componentInParent2 != null)
			{
				return componentInParent != null && componentInParent == componentInParent2;
			}
			return first.transform.parent == second.transform.parent;
		}

		// Token: 0x06004AB6 RID: 19126 RVA: 0x001D14C8 File Offset: 0x001CF6C8
		private ScrollRect ResolveScrollTarget(GameObject selected)
		{
			ScrollRect scrollRect = ((selected != null) ? selected.GetComponentInParent<ScrollRect>() : null);
			if (scrollRect != null && scrollRect.isActiveAndEnabled)
			{
				return scrollRect;
			}
			if (this._scope == null || selected == null)
			{
				return null;
			}
			ScrollRect scrollRect2 = null;
			float num = float.PositiveInfinity;
			foreach (ScrollRect scrollRect3 in this._scope.GetComponentsInChildren<ScrollRect>())
			{
				if (scrollRect3.isActiveAndEnabled && !(scrollRect3.content == null) && this.IsVisible(scrollRect3.gameObject))
				{
					float sqrMagnitude = (ControllerUINavigation.ScreenCenter(scrollRect3.gameObject) - ControllerUINavigation.ScreenCenter(selected)).sqrMagnitude;
					if (sqrMagnitude < num)
					{
						num = sqrMagnitude;
						scrollRect2 = scrollRect3;
					}
				}
			}
			return scrollRect2;
		}

		// Token: 0x06004AB7 RID: 19127 RVA: 0x001D1598 File Offset: 0x001CF798
		private string BuildContextHelp()
		{
			if (this._scope == null)
			{
				return string.Empty;
			}
			if (ControllerTextKeyboard.IsOpen)
			{
				return "A: type   X: delete   Y: shift   LB / RB: caret   Menu: done   B: cancel";
			}
			if (this._dropdownOpen)
			{
				return "D-pad: option   A: select   B: cancel";
			}
			if (this.HasCustomizationEditor())
			{
				return "LB / RB: category   LT + LB / RB: subcategory   Y: save   LT + RS: preview   A: select   B: back";
			}
			if (this._events != null && this._events.currentSelectedGameObject != null && this.FindDialogueRow(this._events.currentSelectedGameObject.transform) != null)
			{
				return "A: edit   LB / RB: reorder line   B: back   RS: scroll";
			}
			if (CielOSController.AnyOpen)
			{
				return "LS / D-pad: navigate   A: select   B: back   X: context   View: apps   RS: scroll";
			}
			return "LS / D-pad: navigate   A: select   B: back   RS: scroll";
		}

		// Token: 0x06004AB8 RID: 19128 RVA: 0x001D163C File Offset: 0x001CF83C
		private void LateUpdate()
		{
			GameObject gameObject = ((this._events != null) ? this._events.currentSelectedGameObject : null);
			PhoneScreenRaycaster phoneScreenRaycaster = ((gameObject != null) ? gameObject.GetComponentInParent<PhoneScreenRaycaster>() : null);
			if (!ControllerInput.IsActive || !SmartphoneController.IsPhonePointerActive || SmartphoneController.IsAnySelfieActive || GraphicsSettingsRuntimeUI.AnyOpen || ControllerTextKeyboard.IsOpen || GameplayInputGate.IsLoading || !base.isActiveAndEnabled || (Application.isPlaying && !Application.isFocused) || !(phoneScreenRaycaster != null) || !gameObject.activeInHierarchy || !this._targets.Contains(gameObject))
			{
				if (this._phoneFocusCanvas != null)
				{
					this._phoneFocusCanvas.enabled = false;
				}
				return;
			}
			RectTransform rectTransform = gameObject.transform as RectTransform;
			Camera eventCamera = phoneScreenRaycaster.eventCamera;
			if (rectTransform == null || eventCamera == null)
			{
				return;
			}
			rectTransform.GetWorldCorners(this._phoneFocusCorners);
			Vector2 vector = new Vector2(float.MaxValue, float.MaxValue);
			Vector2 vector2 = -vector;
			foreach (Vector3 vector3 in this._phoneFocusCorners)
			{
				Vector3 vector4 = eventCamera.WorldToScreenPoint(vector3);
				if (vector4.z <= 0f)
				{
					if (this._phoneFocusCanvas != null)
					{
						this._phoneFocusCanvas.enabled = false;
					}
					return;
				}
				vector = Vector2.Min(vector, vector4);
				vector2 = Vector2.Max(vector2, vector4);
			}
			this.EnsurePhoneFocus();
			this._phoneFocusCanvas.enabled = true;
			this._phoneFocusRect.anchoredPosition = (vector + vector2) * 0.5f;
			this._phoneFocusRect.sizeDelta = vector2 - vector + Vector2.one * 6f;
			if (this._phoneFocusTarget == gameObject)
			{
				return;
			}
			this._phoneFocusTarget = gameObject;
			TMP_Text componentInChildren = gameObject.GetComponentInChildren<TMP_Text>();
			Text text = ((componentInChildren == null) ? gameObject.GetComponentInChildren<Text>() : null);
			this._phoneFocusLabel.text = ((componentInChildren != null) ? componentInChildren.text : ((text != null) ? text.text : gameObject.name));
		}

		// Token: 0x06004AB9 RID: 19129 RVA: 0x001D1880 File Offset: 0x001CFA80
		private void EnsurePhoneFocus()
		{
			if (this._phoneFocusCanvas != null)
			{
				return;
			}
			GameObject gameObject = new GameObject("Phone controller selection", new Type[]
			{
				typeof(RectTransform),
				typeof(Canvas)
			});
			gameObject.transform.SetParent(base.transform, false);
			this._phoneFocusCanvas = gameObject.GetComponent<Canvas>();
			this._phoneFocusCanvas.renderMode = RenderMode.ScreenSpaceOverlay;
			this._phoneFocusCanvas.sortingOrder = 31900;
			GameObject gameObject2 = new GameObject("Selected app", new Type[]
			{
				typeof(RectTransform),
				typeof(ControllerFocusGraphic)
			});
			gameObject2.transform.SetParent(gameObject.transform, false);
			gameObject2.GetComponent<ControllerFocusGraphic>().raycastTarget = false;
			this._phoneFocusRect = (RectTransform)gameObject2.transform;
			RectTransform phoneFocusRect = this._phoneFocusRect;
			Vector2 vector = (this._phoneFocusRect.anchorMax = Vector2.zero);
			phoneFocusRect.anchorMin = vector;
			GameObject gameObject3 = new GameObject("App name", new Type[]
			{
				typeof(RectTransform),
				typeof(TextMeshProUGUI)
			});
			gameObject3.transform.SetParent(gameObject2.transform, false);
			this._phoneFocusLabel = gameObject3.GetComponent<TextMeshProUGUI>();
			this._phoneFocusLabel.fontSize = 19f;
			this._phoneFocusLabel.alignment = TextAlignmentOptions.Center;
			this._phoneFocusLabel.raycastTarget = false;
			this._phoneFocusLabel.enableWordWrapping = false;
			this._phoneFocusLabel.overflowMode = TextOverflowModes.Ellipsis;
			RectTransform rectTransform = this._phoneFocusLabel.rectTransform;
			RectTransform rectTransform2 = this._phoneFocusLabel.rectTransform;
			vector = new Vector2(0.5f, 0f);
			rectTransform2.anchorMax = vector;
			rectTransform.anchorMin = vector;
			this._phoneFocusLabel.rectTransform.pivot = new Vector2(0.5f, 1f);
			this._phoneFocusLabel.rectTransform.anchoredPosition = new Vector2(0f, -5f);
			this._phoneFocusLabel.rectTransform.sizeDelta = new Vector2(240f, 28f);
			LocalizedGameText.BindFontOnly(this._phoneFocusLabel);
		}

		// Token: 0x06004ABA RID: 19130 RVA: 0x001D1AA4 File Offset: 0x001CFCA4
		private bool HandleSliderInput()
		{
			GameObject currentSelectedGameObject = this._events.currentSelectedGameObject;
			Slider slider = ((currentSelectedGameObject != null) ? currentSelectedGameObject.GetComponent<Slider>() : null);
			if (slider == null || ControllerInput.Modifier)
			{
				this._adjustingSlider = null;
				this._sliderDirection = 0;
				return false;
			}
			if (ControllerInput.GetButtonDown(ControllerButton.West))
			{
				Button button = ControllerUINavigation.FindSliderReset(slider);
				if (button != null && ControllerUINavigation.IsFocusable(button.gameObject))
				{
					ControllerUINavigation.Activate(button.gameObject, this._events);
				}
				else
				{
					MainMenuUiFeedbackAudio.PlayDenied();
				}
				ControllerInput.ConsumeUIInput();
				return true;
			}
			int num = (ControllerInput.GetButton(ControllerButton.RightShoulder) ? 1 : 0) - (ControllerInput.GetButton(ControllerButton.LeftShoulder) ? 1 : 0);
			if (num == 0)
			{
				this._sliderDirection = 0;
				return false;
			}
			bool flag = slider != this._adjustingSlider || num != this._sliderDirection;
			if (flag || Time.unscaledTime >= this._nextSliderStep)
			{
				ControllerUINavigation.AdjustSlider(slider, num);
				this._nextSliderStep = Time.unscaledTime + (flag ? 0.32f : 0.07f);
			}
			this._adjustingSlider = slider;
			this._sliderDirection = num;
			ControllerInput.ConsumeUIInput();
			return true;
		}

		// Token: 0x06004ABB RID: 19131 RVA: 0x001D1BC4 File Offset: 0x001CFDC4
		public static void AdjustSlider(Slider slider, int direction)
		{
			if (slider == null || direction == 0)
			{
				return;
			}
			if (!ControllerUINavigation.IsUsable(slider.gameObject))
			{
				MainMenuUiFeedbackAudio.PlayDenied();
				return;
			}
			float num = (slider.wholeNumbers ? 1f : ((slider.maxValue - slider.minValue) * 0.01f));
			slider.value += Mathf.Sign((float)direction) * num;
		}

		// Token: 0x06004ABC RID: 19132 RVA: 0x001D1C2C File Offset: 0x001CFE2C
		public static Button FindSliderReset(Slider slider)
		{
			if (slider == null)
			{
				return null;
			}
			ControllerSliderHint component = slider.GetComponent<ControllerSliderHint>();
			if (component != null && component.ResetButton != null)
			{
				return component.ResetButton;
			}
			Transform transform = slider.transform;
			while (transform != null && !(transform.GetComponent<ScrollRect>() != null) && !(transform.GetComponent<Canvas>() != null) && transform.GetComponentsInChildren<Slider>(true).Length <= 1)
			{
				BlendShapeSliderControlRefs componentInChildren = transform.GetComponentInChildren<BlendShapeSliderControlRefs>(true);
				if (componentInChildren != null && componentInChildren.ResetButton != null)
				{
					return componentInChildren.ResetButton;
				}
				foreach (Button button in transform.GetComponentsInChildren<Button>(true))
				{
					string text = button.name.Replace(" ", "").Replace("_", "").ToLowerInvariant();
					if (text.Contains("reset") && !text.Contains("page") && !text.Contains("all"))
					{
						return button;
					}
				}
				transform = transform.parent;
			}
			return null;
		}

		// Token: 0x04004CA7 RID: 19623
		private static ControllerUINavigation _instance;

		// Token: 0x04004CA8 RID: 19624
		private readonly List<GameObject> _targets = new List<GameObject>();

		// Token: 0x04004CA9 RID: 19625
		private readonly HashSet<GameObject> _unique = new HashSet<GameObject>();

		// Token: 0x04004CAA RID: 19626
		private readonly List<MonoBehaviour> _uiComponents = new List<MonoBehaviour>();

		// Token: 0x04004CAB RID: 19627
		private readonly List<MonoBehaviour> _componentBuffer = new List<MonoBehaviour>();

		// Token: 0x04004CAC RID: 19628
		private readonly HashSet<MonoBehaviour> _uniqueComponents = new HashSet<MonoBehaviour>();

		// Token: 0x04004CAD RID: 19629
		private readonly HashSet<Transform> _scannedAncestors = new HashSet<Transform>();

		// Token: 0x04004CAE RID: 19630
		private readonly List<RaycastResult> _hits = new List<RaycastResult>();

		// Token: 0x04004CAF RID: 19631
		private readonly Dictionary<Transform, GameObject> _remembered = new Dictionary<Transform, GameObject>();

		// Token: 0x04004CB0 RID: 19632
		private readonly Dictionary<Transform, ControllerUINavigation.FocusMemory> _focusMemory = new Dictionary<Transform, ControllerUINavigation.FocusMemory>();

		// Token: 0x04004CB1 RID: 19633
		private EventSystem _events;

		// Token: 0x04004CB2 RID: 19634
		private bool _savedNavigation;

		// Token: 0x04004CB3 RID: 19635
		private bool _ownsNavigation;

		// Token: 0x04004CB4 RID: 19636
		private float _nextScan;

		// Token: 0x04004CB5 RID: 19637
		private float _nextMove;

		// Token: 0x04004CB6 RID: 19638
		private Vector2 _lastDirection;

		// Token: 0x04004CB7 RID: 19639
		private Transform _scope;

		// Token: 0x04004CB8 RID: 19640
		private GameObject _lastSelected;

		// Token: 0x04004CB9 RID: 19641
		private RectTransform _focusBorder;

		// Token: 0x04004CBA RID: 19642
		private int _sceneHandle;

		// Token: 0x04004CBB RID: 19643
		private int _recoveredFocusFrame = -1;

		// Token: 0x04004CBC RID: 19644
		private GameObject _dropdownOwner;

		// Token: 0x04004CBD RID: 19645
		private CustomDropdown _muipDropdown;

		// Token: 0x04004CBE RID: 19646
		private bool _dropdownOpen;

		// Token: 0x04004CBF RID: 19647
		private Canvas _phoneFocusCanvas;

		// Token: 0x04004CC0 RID: 19648
		private RectTransform _phoneFocusRect;

		// Token: 0x04004CC1 RID: 19649
		private TextMeshProUGUI _phoneFocusLabel;

		// Token: 0x04004CC2 RID: 19650
		private GameObject _phoneFocusTarget;

		// Token: 0x04004CC3 RID: 19651
		private readonly Vector3[] _phoneFocusCorners = new Vector3[4];

		// Token: 0x04004CC4 RID: 19652
		private Slider _adjustingSlider;

		// Token: 0x04004CC5 RID: 19653
		private int _sliderDirection;

		// Token: 0x04004CC6 RID: 19654
		private float _nextSliderStep;

		// Token: 0x02000A41 RID: 2625
		private sealed class FocusMemory
		{
			// Token: 0x04004CC7 RID: 19655
			public GameObject Target;

			// Token: 0x04004CC8 RID: 19656
			public string Path;

			// Token: 0x04004CC9 RID: 19657
			public string PresetId;

			// Token: 0x04004CCA RID: 19658
			public Vector2 Position;
		}
	}
}
