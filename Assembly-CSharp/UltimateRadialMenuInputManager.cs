using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.EnhancedTouch;
using UnityEngine.UI;

// Token: 0x0200015B RID: 347
public class UltimateRadialMenuInputManager : MonoBehaviour
{
	// Token: 0x1700023B RID: 571
	// (get) Token: 0x06000B65 RID: 2917 RVA: 0x0004DD9D File Offset: 0x0004BF9D
	// (set) Token: 0x06000B66 RID: 2918 RVA: 0x0004DDA4 File Offset: 0x0004BFA4
	public static UltimateRadialMenuInputManager Instance { get; private set; }

	// Token: 0x1700023C RID: 572
	// (get) Token: 0x06000B67 RID: 2919 RVA: 0x0004DDAC File Offset: 0x0004BFAC
	// (set) Token: 0x06000B68 RID: 2920 RVA: 0x0004DDB4 File Offset: 0x0004BFB4
	public List<UltimateRadialMenuInputManager.UltimateRadialMenuInformation> UltimateRadialMenuInformations { get; private set; }

	// Token: 0x06000B69 RID: 2921 RVA: 0x0004DDBD File Offset: 0x0004BFBD
	[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
	private static void ResetStaticState()
	{
		UltimateRadialMenuInputManager.Instance = null;
	}

	// Token: 0x06000B6A RID: 2922 RVA: 0x0004DDC8 File Offset: 0x0004BFC8
	[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
	private static void RestoreSceneInstance()
	{
		UltimateRadialMenuInputManager[] array = Object.FindObjectsOfType<UltimateRadialMenuInputManager>(true);
		for (int i = 0; i < array.Length; i++)
		{
			if (array[i] != null && array[i].isActiveAndEnabled)
			{
				array[i].RestoreRuntimeState();
			}
		}
	}

	// Token: 0x1700023D RID: 573
	// (get) Token: 0x06000B6B RID: 2923 RVA: 0x0004DE07 File Offset: 0x0004C007
	// (set) Token: 0x06000B6C RID: 2924 RVA: 0x0004DE0F File Offset: 0x0004C00F
	public UltimateRadialMenuInputManager.InputDevice CurrentInputDevice { get; private protected set; }

	// Token: 0x06000B6D RID: 2925 RVA: 0x0004DE18 File Offset: 0x0004C018
	protected virtual void Awake()
	{
		if (!base.GetComponent<EventSystem>() && !base.GetComponent<UltimateRadialMenu>())
		{
			Debug.LogError(UltimateRadialMenuInputManager.FormatDebug("This component is not attached to the EventSystem in your scene or an Ultimate Radial Menu component", "Please make sure that you have only one Ultimate Radial Menu Input Manager in your scene and that it is located on the EventSystem. If you want unique controller input then place the Ultimate Radial Menu Input Manager on the Ultimate Radial Menu GameObject that you want to have unique controller input on", base.gameObject.name));
			Object.Destroy(this);
			return;
		}
		if (base.GetComponent<EventSystem>())
		{
			if (UltimateRadialMenuInputManager.Instance != null && UltimateRadialMenuInputManager.Instance != this && UltimateRadialMenuInputManager.Instance.gameObject != null && UltimateRadialMenuInputManager.Instance.gameObject.activeInHierarchy)
			{
				Object.Destroy(this);
				return;
			}
			UltimateRadialMenuInputManager.Instance = this;
		}
		if (this.UltimateRadialMenuInformations == null)
		{
			this.UltimateRadialMenuInformations = new List<UltimateRadialMenuInputManager.UltimateRadialMenuInformation>();
		}
		this.worldSpaceMask = LayerMask.GetMask(new string[] { "UI" });
	}

	// Token: 0x06000B6E RID: 2926 RVA: 0x0004DEED File Offset: 0x0004C0ED
	protected virtual void OnEnable()
	{
		this.RestoreRuntimeState();
	}

	// Token: 0x06000B6F RID: 2927 RVA: 0x0004DEF5 File Offset: 0x0004C0F5
	protected virtual void OnDisable()
	{
		this.UnbindInputSystemCallbacks();
		if (this.enhancedTouchEnabled)
		{
			EnhancedTouchSupport.Disable();
			this.enhancedTouchEnabled = false;
		}
	}

	// Token: 0x06000B70 RID: 2928 RVA: 0x0004DF11 File Offset: 0x0004C111
	protected virtual void Start()
	{
		this.UpdateCamera();
		this.BindInputSystemCallbacks();
	}

	// Token: 0x06000B71 RID: 2929 RVA: 0x0004DF20 File Offset: 0x0004C120
	public void AddRadialMenuToList(UltimateRadialMenu radialMenu, ref UltimateRadialMenuInputManager inputManager)
	{
		if (radialMenu == null)
		{
			return;
		}
		radialMenu.UpdateParentCanvas();
		if (radialMenu.ParentCanvas == null)
		{
			return;
		}
		if (UltimateRadialMenuInputManager.Instance == null && base.GetComponent<EventSystem>() != null)
		{
			UltimateRadialMenuInputManager.Instance = this;
		}
		this.EnsureInformationList();
		for (int i = 0; i < this.UltimateRadialMenuInformations.Count; i++)
		{
			UltimateRadialMenuInputManager.UltimateRadialMenuInformation ultimateRadialMenuInformation = this.UltimateRadialMenuInformations[i];
			if (ultimateRadialMenuInformation != null && ultimateRadialMenuInformation.radialMenu == radialMenu)
			{
				radialMenu.inputInformation = ultimateRadialMenuInformation;
				this.RestoreMenuBindings(ultimateRadialMenuInformation);
				inputManager = this;
				return;
			}
		}
		UltimateRadialMenuInputManager.UltimateRadialMenuInformation ultimateRadialMenuInformation2 = new UltimateRadialMenuInputManager.UltimateRadialMenuInformation
		{
			radialMenu = radialMenu,
			inputManager = this
		};
		radialMenu.inputInformation = ultimateRadialMenuInformation2;
		this.RestoreMenuBindings(ultimateRadialMenuInformation2);
		this.UltimateRadialMenuInformations.Add(ultimateRadialMenuInformation2);
		inputManager = this;
	}

	// Token: 0x06000B72 RID: 2930 RVA: 0x0004DFEC File Offset: 0x0004C1EC
	private void RestoreMenuBindings(UltimateRadialMenuInputManager.UltimateRadialMenuInformation info)
	{
		UltimateRadialMenu radialMenu = info.radialMenu;
		if (radialMenu == null)
		{
			return;
		}
		info.inputManager = this;
		radialMenu.inputInformation = info;
		RectTransform rectTransform = ((radialMenu.BaseTransform != null) ? radialMenu.BaseTransform : radialMenu.GetComponent<RectTransform>());
		if (this.centerScreenInput && this.trackInputPosition && this.trackInputSprite != null && rectTransform != null)
		{
			if (info.inputTracker == null)
			{
				Transform transform = rectTransform.Find("Input Tracker");
				if (transform != null)
				{
					info.inputTracker = transform.GetComponent<Image>();
				}
			}
			if (info.inputTracker == null)
			{
				GameObject gameObject = new GameObject("Input Tracker", new Type[]
				{
					typeof(RectTransform),
					typeof(CanvasRenderer),
					typeof(Image)
				});
				info.inputTracker = gameObject.GetComponent<Image>();
				info.inputTracker.sprite = this.trackInputSprite;
				RectTransform rectTransform2 = info.inputTracker.rectTransform;
				rectTransform2.SetParent(rectTransform, false);
				rectTransform2.SetAsLastSibling();
				rectTransform2.sizeDelta = rectTransform.sizeDelta * this.inputTrackerSize;
			}
		}
		radialMenu.OnUpdatePositioning -= info.UpdatePositioning;
		if (info.inputTracker != null)
		{
			radialMenu.OnUpdatePositioning += info.UpdatePositioning;
		}
		radialMenu.OnMenuDisabled -= info.ResetMenuPosition;
		if (this.touchInput)
		{
			radialMenu.OnMenuDisabled += info.ResetMenuPosition;
		}
	}

	// Token: 0x06000B73 RID: 2931 RVA: 0x0004E18C File Offset: 0x0004C38C
	protected void UpdateCamera()
	{
		this.mainCamera = null;
		Camera[] array = Object.FindObjectsByType<Camera>(FindObjectsSortMode.None);
		for (int i = 0; i < array.Length; i++)
		{
			if (array[i].gameObject.activeInHierarchy && array[i].enabled)
			{
				this.mainCamera = array[i];
				if (array[i].tag == "MainCamera")
				{
					break;
				}
			}
		}
	}

	// Token: 0x06000B74 RID: 2932 RVA: 0x0004E1EB File Offset: 0x0004C3EB
	public void SetMainCamera(Camera newMainCamera)
	{
		this.mainCamera = newMainCamera;
	}

	// Token: 0x06000B75 RID: 2933 RVA: 0x0004E1F4 File Offset: 0x0004C3F4
	public void SetCamerasVR(Camera newLeftEyeCamera, Camera newRightEyeCamera)
	{
		this.leftEyeCamera = newLeftEyeCamera;
		this.rightEyeCamera = newRightEyeCamera;
	}

	// Token: 0x06000B76 RID: 2934 RVA: 0x0004E204 File Offset: 0x0004C404
	protected void RaycastWorldSpaceRadialMenu(ref Vector2 input, ref float distance, Vector2 rayOrigin, int radialMenuIndex)
	{
		if (!this.UltimateRadialMenuInformations[radialMenuIndex].radialMenu.IsWorldSpaceRadialMenu)
		{
			return;
		}
		if (this.mainCamera == null || !this.mainCamera.gameObject.activeInHierarchy || !this.mainCamera.enabled)
		{
			this.UpdateCamera();
		}
		if (this.mainCamera == null)
		{
			return;
		}
		RaycastHit raycastHit;
		if (Physics.Raycast(this.mainCamera.ScreenPointToRay(rayOrigin), out raycastHit, float.PositiveInfinity, this.worldSpaceMask) && raycastHit.collider.gameObject == this.UltimateRadialMenuInformations[radialMenuIndex].radialMenu.gameObject)
		{
			Vector3 vector = this.UltimateRadialMenuInformations[radialMenuIndex].radialMenu.BaseTransform.InverseTransformPoint(raycastHit.point);
			input = vector;
			distance = Vector3.Distance(Vector2.zero, vector);
		}
	}

	// Token: 0x06000B77 RID: 2935 RVA: 0x0004E304 File Offset: 0x0004C504
	protected void CheckEnableMenu(bool buttonPressed, bool buttonReleased, int radialMenuIndex)
	{
		if (this.enableMenuSetting == UltimateRadialMenuInputManager.EnableMenuSetting.Hold)
		{
			if (buttonPressed && !this.UltimateRadialMenuInformations[radialMenuIndex].radialMenu.IsEnabled)
			{
				this.UltimateRadialMenuInformations[radialMenuIndex].radialMenu.Enable();
			}
			if (buttonReleased && this.UltimateRadialMenuInformations[radialMenuIndex].radialMenu.IsEnabled)
			{
				this.UltimateRadialMenuInformations[radialMenuIndex].radialMenu.Disable();
				return;
			}
		}
		else if (buttonPressed)
		{
			if (!this.UltimateRadialMenuInformations[radialMenuIndex].radialMenu.IsEnabled)
			{
				this.UltimateRadialMenuInformations[radialMenuIndex].radialMenu.Enable();
				return;
			}
			this.UltimateRadialMenuInformations[radialMenuIndex].radialMenu.Disable();
		}
	}

	// Token: 0x06000B78 RID: 2936 RVA: 0x0004E3C5 File Offset: 0x0004C5C5
	private static string FormatDebug(string error, string solution, string objectName)
	{
		return string.Concat(new string[] { "<b>Ultimate Radial Menu Input Manager</b>\n<color=red><b>×</b></color> <i><b>Error:</b></i> ", error, ".\n<color=green><b>√</b></color> <i><b>Solution:</b></i> ", solution, ".\n<color=#00FFFF><b>∙</b></color> <i><b>Object:</b></i> ", objectName, "\n" });
	}

	// Token: 0x06000B79 RID: 2937 RVA: 0x0004E400 File Offset: 0x0004C600
	private void Update()
	{
		this.EnsureInformationList();
		for (int i = 0; i < this.UltimateRadialMenuInformations.Count; i++)
		{
			UltimateRadialMenuInputManager.UltimateRadialMenuInformation ultimateRadialMenuInformation = this.UltimateRadialMenuInformations[i];
			if (ultimateRadialMenuInformation == null || ultimateRadialMenuInformation.radialMenu == null || ultimateRadialMenuInformation.radialMenu.ParentCanvas == null)
			{
				if (ultimateRadialMenuInformation != null && ultimateRadialMenuInformation.radialMenu != null)
				{
					ultimateRadialMenuInformation.radialMenu.OnUpdatePositioning -= ultimateRadialMenuInformation.UpdatePositioning;
					ultimateRadialMenuInformation.radialMenu.OnMenuDisabled -= ultimateRadialMenuInformation.ResetMenuPosition;
				}
				this.UltimateRadialMenuInformations.RemoveAt(i);
				i--;
			}
			else if (ultimateRadialMenuInformation.radialMenu.CanProcessInput)
			{
				bool flag = false;
				bool flag2 = false;
				bool flag3 = false;
				bool flag4 = false;
				Vector2 vector = Vector2.zero;
				float num = 0f;
				if (this.keyboardInput)
				{
					this.MouseAndKeyboardInput(ref flag, ref flag2, ref vector, ref num, ref flag3, ref flag4, i);
				}
				if (this.controllerInput)
				{
					this.ControllerInput(ref flag, ref flag2, ref vector, ref num, ref flag3, ref flag4, i);
				}
				if (this.touchInput)
				{
					this.TouchInput(ref flag, ref flag2, ref vector, ref num, ref flag3, ref flag4, i);
				}
				if (this.centerScreenInput)
				{
					this.CenterScreenInput(ref flag, ref flag2, ref vector, ref num, ref flag3, ref flag4, i);
				}
				if (this.customInput)
				{
					this.CustomInput(ref flag, ref flag2, ref vector, ref num, ref flag3, ref flag4, i);
				}
				if (this.customControllerInput)
				{
					this.CustomControllerInput(ref flag, ref flag2, ref vector, ref num, ref flag3, ref flag4, i);
				}
				if (this.customControllerCalculated)
				{
					this.CurrentInputDevice = UltimateRadialMenuInputManager.InputDevice.Controller;
					if (this.customControllerInputDown)
					{
						flag3 = true;
					}
					if (this.customControllerInputUp)
					{
						flag4 = true;
					}
					if (this.customControllerEnableMenu)
					{
						flag = true;
					}
					if (this.customControllerDisableMenu)
					{
						flag2 = true;
					}
				}
				if (this.UltimateRadialMenuInformations[i].raycastInputCalculated)
				{
					this.CurrentInputDevice = UltimateRadialMenuInputManager.InputDevice.Other;
					if (this.UltimateRadialMenuInformations[i].raycastInput != Vector2.zero)
					{
						vector = this.UltimateRadialMenuInformations[i].raycastInput;
						num = this.UltimateRadialMenuInformations[i].raycastDistance;
					}
					this.UltimateRadialMenuInformations[i].raycastInputCalculated = false;
					this.UltimateRadialMenuInformations[i].raycastInput = Vector2.zero;
					this.UltimateRadialMenuInformations[i].raycastDistance = 0f;
				}
				if (this.customInputCalculated)
				{
					if (this.customInputDown)
					{
						flag3 = true;
					}
					if (this.customInputUp)
					{
						flag4 = true;
					}
					if (this.customInputEnable)
					{
						flag = true;
					}
					if (this.customInputDisable)
					{
						flag2 = true;
					}
				}
				if (this.onMenuRelease && this.UltimateRadialMenuInformations[i].radialMenu.IsEnabled && ((this.enableMenuSetting == UltimateRadialMenuInputManager.EnableMenuSetting.Hold && flag2) || (this.enableMenuSetting == UltimateRadialMenuInputManager.EnableMenuSetting.Toggle && flag)))
				{
					flag4 = (flag3 = true);
				}
				this.UltimateRadialMenuInformations[i].radialMenu.ProcessInput(vector, num, flag3, flag4);
				if (this.enableMenuSetting != UltimateRadialMenuInputManager.EnableMenuSetting.Manual)
				{
					this.CheckEnableMenu(flag, flag2, i);
				}
			}
		}
		if (this.customControllerCalculated)
		{
			this.customControllerCalculated = false;
			this.customControllerInputDown = false;
			this.customControllerInputUp = false;
			this.customControllerEnableMenu = false;
			this.customControllerDisableMenu = false;
		}
		if (this.customInputCalculated)
		{
			this.customInputCalculated = false;
			this.customInputDown = (this.customInputUp = false);
			this.customInputEnable = (this.customInputDisable = false);
		}
	}

	// Token: 0x06000B7A RID: 2938 RVA: 0x0004E758 File Offset: 0x0004C958
	private void EnsureInformationList()
	{
		if (this.UltimateRadialMenuInformations != null)
		{
			return;
		}
		this.UltimateRadialMenuInformations = new List<UltimateRadialMenuInputManager.UltimateRadialMenuInformation>();
		foreach (UltimateRadialMenu ultimateRadialMenu in Object.FindObjectsOfType<UltimateRadialMenu>(true))
		{
			if (!(ultimateRadialMenu == null) && !(ultimateRadialMenu.inputManager != this))
			{
				ultimateRadialMenu.UpdateParentCanvas();
				if (!(ultimateRadialMenu.ParentCanvas == null))
				{
					UltimateRadialMenuInputManager.UltimateRadialMenuInformation ultimateRadialMenuInformation = ultimateRadialMenu.inputInformation;
					if (ultimateRadialMenuInformation == null || ultimateRadialMenuInformation.inputManager != this || ultimateRadialMenuInformation.radialMenu != ultimateRadialMenu)
					{
						ultimateRadialMenuInformation = new UltimateRadialMenuInputManager.UltimateRadialMenuInformation
						{
							radialMenu = ultimateRadialMenu,
							inputManager = this
						};
					}
					this.UltimateRadialMenuInformations.Add(ultimateRadialMenuInformation);
					this.RestoreMenuBindings(ultimateRadialMenuInformation);
				}
			}
		}
	}

	// Token: 0x06000B7B RID: 2939 RVA: 0x0004E810 File Offset: 0x0004CA10
	private void RestoreRuntimeState()
	{
		if (base.GetComponent<EventSystem>() && (UltimateRadialMenuInputManager.Instance == null || UltimateRadialMenuInputManager.Instance == this || UltimateRadialMenuInputManager.Instance.gameObject == null || !UltimateRadialMenuInputManager.Instance.gameObject.activeInHierarchy))
		{
			UltimateRadialMenuInputManager.Instance = this;
		}
		this.worldSpaceMask = LayerMask.GetMask(new string[] { "UI" });
		this.EnsureInformationList();
		for (int i = 0; i < this.UltimateRadialMenuInformations.Count; i++)
		{
			if (this.UltimateRadialMenuInformations[i] != null)
			{
				this.RestoreMenuBindings(this.UltimateRadialMenuInformations[i]);
			}
		}
		if (this.mainCamera == null || !this.mainCamera.isActiveAndEnabled)
		{
			this.UpdateCamera();
		}
		this.BindInputSystemCallbacks();
	}

	// Token: 0x06000B7C RID: 2940 RVA: 0x0004E8F0 File Offset: 0x0004CAF0
	private void BindInputSystemCallbacks()
	{
		this.UnbindInputSystemCallbacks();
		if (this.touchInput && !this.enhancedTouchEnabled)
		{
			EnhancedTouchSupport.Enable();
			this.enhancedTouchEnabled = true;
		}
		if (!this.customControllerInput)
		{
			return;
		}
		if (this.customControllerJoystick != null && this.customControllerJoystick.bindings.Count > 0)
		{
			this.customControllerJoystick.Enable();
		}
		if (this.customControllerInteract != null && this.customControllerInteract.bindings.Count > 0)
		{
			this.customControllerInteract.Enable();
			this.customControllerInteract.performed += this.CustomControllerInputDown;
			this.customControllerInteract.canceled += this.CustomControllerInputUp;
		}
		if (this.enableMenuSetting != UltimateRadialMenuInputManager.EnableMenuSetting.Manual && this.customControllerEnable != null && this.customControllerEnable.bindings.Count > 0)
		{
			this.customControllerEnable.Enable();
			this.customControllerEnable.performed += this.CustomControllerEnable;
			this.customControllerEnable.canceled += this.CustomControllerDisable;
		}
	}

	// Token: 0x06000B7D RID: 2941 RVA: 0x0004EA08 File Offset: 0x0004CC08
	private void UnbindInputSystemCallbacks()
	{
		if (this.customControllerInteract != null)
		{
			this.customControllerInteract.performed -= this.CustomControllerInputDown;
			this.customControllerInteract.canceled -= this.CustomControllerInputUp;
		}
		if (this.customControllerEnable != null)
		{
			this.customControllerEnable.performed -= this.CustomControllerEnable;
			this.customControllerEnable.canceled -= this.CustomControllerDisable;
		}
	}

	// Token: 0x06000B7E RID: 2942 RVA: 0x0004EA84 File Offset: 0x0004CC84
	public virtual void MouseAndKeyboardInput(ref bool enableMenu, ref bool disableMenu, ref Vector2 input, ref float distance, ref bool inputDown, ref bool inputUp, int radialMenuIndex)
	{
		Vector2 vector = Vector2.zero;
		Mouse device = InputSystem.GetDevice<Mouse>();
		if (device == null)
		{
			return;
		}
		if (!device.enabled && Touchscreen.current != null && Touchscreen.current.enabled)
		{
			Debug.LogError(UltimateRadialMenuInputManager.FormatDebug("Mouse device is disabled by Unity", "Please disable the Simulated Touchscreen option. To do this, go to Window > Analysis > Input Debugger. In this window, disable the Simulated Touchscreen by going to Options > Simulate Touch Input From Mouse Or Pen", "NA"));
		}
		vector = device.position.ReadValue();
		if (this.mouseInteractButton != UltimateRadialMenuInputManager.MouseInteractButtons.Right)
		{
			if (device.leftButton.wasPressedThisFrame)
			{
				inputDown = true;
			}
			if (device.leftButton.wasReleasedThisFrame)
			{
				inputUp = true;
			}
		}
		if (this.mouseInteractButton != UltimateRadialMenuInputManager.MouseInteractButtons.Left)
		{
			if (device.rightButton.wasPressedThisFrame)
			{
				inputDown = true;
			}
			if (device.rightButton.wasReleasedThisFrame)
			{
				inputUp = true;
			}
		}
		Keyboard device2 = InputSystem.GetDevice<Keyboard>();
		if (this.enableMenuSetting != UltimateRadialMenuInputManager.EnableMenuSetting.Manual && this.keyboardEnableKey != UltimateRadialMenuInputManager.KeyboardEnableKeys.EnableManually && device2 != null)
		{
			switch (this.keyboardEnableKey)
			{
			default:
				enableMenu = device2.tabKey.wasPressedThisFrame;
				disableMenu = device2.tabKey.wasReleasedThisFrame;
				break;
			case UltimateRadialMenuInputManager.KeyboardEnableKeys.LeftAlt:
				enableMenu = device2.leftAltKey.wasPressedThisFrame;
				disableMenu = device2.leftAltKey.wasReleasedThisFrame;
				break;
			case UltimateRadialMenuInputManager.KeyboardEnableKeys.LeftControl:
				enableMenu = device2.leftCtrlKey.wasPressedThisFrame;
				if (!enableMenu)
				{
					enableMenu = device2.leftCommandKey.wasPressedThisFrame;
				}
				disableMenu = device2.leftCtrlKey.wasReleasedThisFrame;
				if (!disableMenu)
				{
					disableMenu = device2.leftCommandKey.wasReleasedThisFrame;
				}
				break;
			case UltimateRadialMenuInputManager.KeyboardEnableKeys.CapsLock:
				enableMenu = device2.capsLockKey.wasPressedThisFrame;
				disableMenu = device2.capsLockKey.wasReleasedThisFrame;
				break;
			case UltimateRadialMenuInputManager.KeyboardEnableKeys.Tilde:
				enableMenu = device2.backquoteKey.wasPressedThisFrame;
				disableMenu = device2.backquoteKey.wasReleasedThisFrame;
				break;
			case UltimateRadialMenuInputManager.KeyboardEnableKeys.Escape:
				enableMenu = device2.escapeKey.wasPressedThisFrame;
				disableMenu = device2.escapeKey.wasReleasedThisFrame;
				break;
			}
		}
		if (this.UltimateRadialMenuInformations[radialMenuIndex].previousMouseInput == vector && this.CurrentInputDevice != UltimateRadialMenuInputManager.InputDevice.Mouse && !inputDown && !inputUp && !enableMenu && !disableMenu)
		{
			return;
		}
		this.CurrentInputDevice = UltimateRadialMenuInputManager.InputDevice.Mouse;
		this.UltimateRadialMenuInformations[radialMenuIndex].previousMouseInput = vector;
		if (this.UltimateRadialMenuInformations[radialMenuIndex].radialMenu.IsWorldSpaceRadialMenu)
		{
			this.RaycastWorldSpaceRadialMenu(ref input, ref distance, vector, radialMenuIndex);
			return;
		}
		Vector2 vector2 = vector / this.UltimateRadialMenuInformations[radialMenuIndex].radialMenu.ParentCanvas.scaleFactor - this.UltimateRadialMenuInformations[radialMenuIndex].radialMenu.ParentCanvas.GetComponent<RectTransform>().sizeDelta / 2f;
		input = (vector2 - this.UltimateRadialMenuInformations[radialMenuIndex].radialMenu.BaseTransform.localPosition) / (this.UltimateRadialMenuInformations[radialMenuIndex].radialMenu.BaseTransform.sizeDelta.x / 2f);
		distance = Vector2.Distance(this.UltimateRadialMenuInformations[radialMenuIndex].radialMenu.BaseTransform.localPosition, vector2);
	}

	// Token: 0x06000B7F RID: 2943 RVA: 0x0004EDA0 File Offset: 0x0004CFA0
	public virtual void ControllerInput(ref bool enableMenu, ref bool disableMenu, ref Vector2 input, ref float distance, ref bool inputDown, ref bool inputUp, int radialMenuIndex)
	{
		bool flag = false;
		if (enableMenu | disableMenu | inputDown | inputUp)
		{
			flag = true;
		}
		Vector2 vector = Vector2.zero;
		Gamepad device = InputSystem.GetDevice<Gamepad>();
		if (device == null)
		{
			return;
		}
		vector = ((this.joystick == UltimateRadialMenuInputManager.Joysticks.Left) ? device.leftStick.ReadValue() : device.rightStick.ReadValue());
		this.CheckControllerButtons(device, this.interactButtons, ref inputDown, ref inputUp);
		if (this.enableMenuSetting != UltimateRadialMenuInputManager.EnableMenuSetting.Manual)
		{
			this.CheckControllerButtons(device, this.enableButtons, ref enableMenu, ref disableMenu);
		}
		if (!flag && (enableMenu | disableMenu | inputDown | inputUp))
		{
			this.CurrentInputDevice = UltimateRadialMenuInputManager.InputDevice.Controller;
		}
		if (vector != Vector2.zero)
		{
			this.CurrentInputDevice = UltimateRadialMenuInputManager.InputDevice.Controller;
			if (this.invertHorizontal)
			{
				vector.x *= -1f;
			}
			if (this.invertVertical)
			{
				vector.y *= -1f;
			}
			input = vector;
			if (Vector2.Distance(Vector2.zero, vector) >= this.UltimateRadialMenuInformations[radialMenuIndex].radialMenu.minRange)
			{
				distance = Mathf.Lerp(this.UltimateRadialMenuInformations[radialMenuIndex].radialMenu.CalculatedMinRange, this.UltimateRadialMenuInformations[radialMenuIndex].radialMenu.CalculatedMaxRange, 0.5f);
			}
		}
	}

	// Token: 0x06000B80 RID: 2944 RVA: 0x0004EEE0 File Offset: 0x0004D0E0
	private void CheckControllerButtons(Gamepad gamepad, UltimateRadialMenuInputManager.ControllerButtons buttonsToCheck, ref bool buttonPress, ref bool buttonRelease)
	{
		if (buttonsToCheck <= UltimateRadialMenuInputManager.ControllerButtons.RightShoulder)
		{
			if (buttonsToCheck <= UltimateRadialMenuInputManager.ControllerButtons.LeftJoystick)
			{
				switch (buttonsToCheck)
				{
				case UltimateRadialMenuInputManager.ControllerButtons.North:
					if (!buttonPress)
					{
						buttonPress = gamepad.buttonNorth.wasPressedThisFrame;
					}
					if (!buttonRelease)
					{
						buttonRelease = gamepad.buttonNorth.wasReleasedThisFrame;
						return;
					}
					break;
				case UltimateRadialMenuInputManager.ControllerButtons.South:
					if (!buttonPress)
					{
						buttonPress = gamepad.buttonSouth.wasPressedThisFrame;
					}
					if (!buttonRelease)
					{
						buttonRelease = gamepad.buttonSouth.wasReleasedThisFrame;
						return;
					}
					break;
				case (UltimateRadialMenuInputManager.ControllerButtons)3:
					break;
				case UltimateRadialMenuInputManager.ControllerButtons.East:
					if (!buttonPress)
					{
						buttonPress = gamepad.buttonEast.wasPressedThisFrame;
					}
					if (!buttonRelease)
					{
						buttonRelease = gamepad.buttonEast.wasReleasedThisFrame;
						return;
					}
					break;
				default:
					if (buttonsToCheck != UltimateRadialMenuInputManager.ControllerButtons.West)
					{
						if (buttonsToCheck != UltimateRadialMenuInputManager.ControllerButtons.LeftJoystick)
						{
							return;
						}
						if (!buttonPress)
						{
							buttonPress = gamepad.leftStickButton.wasPressedThisFrame;
						}
						if (!buttonRelease)
						{
							buttonRelease = gamepad.leftStickButton.wasReleasedThisFrame;
							return;
						}
					}
					else
					{
						if (!buttonPress)
						{
							buttonPress = gamepad.buttonWest.wasPressedThisFrame;
						}
						if (!buttonRelease)
						{
							buttonRelease = gamepad.buttonWest.wasReleasedThisFrame;
							return;
						}
					}
					break;
				}
			}
			else if (buttonsToCheck <= UltimateRadialMenuInputManager.ControllerButtons.LeftTrigger)
			{
				if (buttonsToCheck != UltimateRadialMenuInputManager.ControllerButtons.LeftShoulder)
				{
					if (buttonsToCheck != UltimateRadialMenuInputManager.ControllerButtons.LeftTrigger)
					{
						return;
					}
					if (!buttonPress)
					{
						buttonPress = gamepad.leftTrigger.wasPressedThisFrame;
					}
					if (!buttonRelease)
					{
						buttonRelease = gamepad.leftTrigger.wasReleasedThisFrame;
						return;
					}
				}
				else
				{
					if (!buttonPress)
					{
						buttonPress = gamepad.leftShoulder.wasPressedThisFrame;
					}
					if (!buttonRelease)
					{
						buttonRelease = gamepad.leftShoulder.wasReleasedThisFrame;
						return;
					}
				}
			}
			else if (buttonsToCheck != UltimateRadialMenuInputManager.ControllerButtons.RightJoystick)
			{
				if (buttonsToCheck != UltimateRadialMenuInputManager.ControllerButtons.RightShoulder)
				{
					return;
				}
				if (!buttonPress)
				{
					buttonPress = gamepad.rightShoulder.wasPressedThisFrame;
				}
				if (!buttonRelease)
				{
					buttonRelease = gamepad.rightShoulder.wasReleasedThisFrame;
					return;
				}
			}
			else
			{
				if (!buttonPress)
				{
					buttonPress = gamepad.rightStickButton.wasPressedThisFrame;
				}
				if (!buttonRelease)
				{
					buttonRelease = gamepad.rightStickButton.wasReleasedThisFrame;
					return;
				}
			}
		}
		else if (buttonsToCheck <= UltimateRadialMenuInputManager.ControllerButtons.Select)
		{
			if (buttonsToCheck != UltimateRadialMenuInputManager.ControllerButtons.RightTrigger)
			{
				if (buttonsToCheck != UltimateRadialMenuInputManager.ControllerButtons.Start)
				{
					if (buttonsToCheck != UltimateRadialMenuInputManager.ControllerButtons.Select)
					{
						return;
					}
					if (!buttonPress)
					{
						buttonPress = gamepad.selectButton.wasPressedThisFrame;
					}
					if (!buttonRelease)
					{
						buttonRelease = gamepad.selectButton.wasReleasedThisFrame;
						return;
					}
				}
				else
				{
					if (!buttonPress)
					{
						buttonPress = gamepad.startButton.wasPressedThisFrame;
					}
					if (!buttonRelease)
					{
						buttonRelease = gamepad.startButton.wasReleasedThisFrame;
						return;
					}
				}
			}
			else
			{
				if (!buttonPress)
				{
					buttonPress = gamepad.rightTrigger.wasPressedThisFrame;
				}
				if (!buttonRelease)
				{
					buttonRelease = gamepad.rightTrigger.wasReleasedThisFrame;
					return;
				}
			}
		}
		else if (buttonsToCheck <= UltimateRadialMenuInputManager.ControllerButtons.DpadDown)
		{
			if (buttonsToCheck != UltimateRadialMenuInputManager.ControllerButtons.DpadUp)
			{
				if (buttonsToCheck != UltimateRadialMenuInputManager.ControllerButtons.DpadDown)
				{
					return;
				}
				if (!buttonPress)
				{
					buttonPress = gamepad.dpad.down.wasPressedThisFrame;
				}
				if (!buttonRelease)
				{
					buttonRelease = gamepad.dpad.down.wasReleasedThisFrame;
					return;
				}
			}
			else
			{
				if (!buttonPress)
				{
					buttonPress = gamepad.dpad.up.wasPressedThisFrame;
				}
				if (!buttonRelease)
				{
					buttonRelease = gamepad.dpad.up.wasReleasedThisFrame;
					return;
				}
			}
		}
		else if (buttonsToCheck != UltimateRadialMenuInputManager.ControllerButtons.DpadLeft)
		{
			if (buttonsToCheck != UltimateRadialMenuInputManager.ControllerButtons.DpadRight)
			{
				return;
			}
			if (!buttonPress)
			{
				buttonPress = gamepad.dpad.right.wasPressedThisFrame;
			}
			if (!buttonRelease)
			{
				buttonRelease = gamepad.dpad.right.wasReleasedThisFrame;
			}
		}
		else
		{
			if (!buttonPress)
			{
				buttonPress = gamepad.dpad.left.wasPressedThisFrame;
			}
			if (!buttonRelease)
			{
				buttonRelease = gamepad.dpad.left.wasReleasedThisFrame;
				return;
			}
		}
	}

	// Token: 0x06000B81 RID: 2945 RVA: 0x0004F250 File Offset: 0x0004D450
	public virtual void CustomControllerInput(ref bool enableMenu, ref bool disableMenu, ref Vector2 input, ref float distance, ref bool inputDown, ref bool inputUp, int radialMenuIndex)
	{
		if (this.customControllerJoystick == null)
		{
			return;
		}
		if (this.customControllerJoystick.ReadValue<Vector2>() != Vector2.zero)
		{
			this.CurrentInputDevice = UltimateRadialMenuInputManager.InputDevice.Controller;
			input = this.customControllerJoystick.ReadValue<Vector2>();
			if (Vector2.Distance(Vector2.zero, input) >= this.UltimateRadialMenuInformations[radialMenuIndex].radialMenu.minRange)
			{
				distance = Mathf.Lerp(this.UltimateRadialMenuInformations[radialMenuIndex].radialMenu.CalculatedMinRange, this.UltimateRadialMenuInformations[radialMenuIndex].radialMenu.CalculatedMaxRange, 0.5f);
			}
		}
	}

	// Token: 0x06000B82 RID: 2946 RVA: 0x0004F2F9 File Offset: 0x0004D4F9
	private void CustomControllerInputDown(InputAction.CallbackContext obj)
	{
		this.customControllerCalculated = true;
		this.customControllerInputDown = obj.performed && obj.ReadValue<float>() == 1f;
	}

	// Token: 0x06000B83 RID: 2947 RVA: 0x0004F322 File Offset: 0x0004D522
	private void CustomControllerInputUp(InputAction.CallbackContext obj)
	{
		this.customControllerCalculated = true;
		this.customControllerInputUp = obj.canceled;
	}

	// Token: 0x06000B84 RID: 2948 RVA: 0x0004F338 File Offset: 0x0004D538
	private void CustomControllerEnable(InputAction.CallbackContext obj)
	{
		this.customControllerCalculated = true;
		this.customControllerEnableMenu = obj.performed && obj.ReadValue<float>() == 1f;
	}

	// Token: 0x06000B85 RID: 2949 RVA: 0x0004F361 File Offset: 0x0004D561
	private void CustomControllerDisable(InputAction.CallbackContext obj)
	{
		this.customControllerCalculated = true;
		this.customControllerDisableMenu = obj.canceled;
	}

	// Token: 0x06000B86 RID: 2950 RVA: 0x0004F378 File Offset: 0x0004D578
	public virtual void TouchInput(ref bool enableMenu, ref bool disableMenu, ref Vector2 input, ref float distance, ref bool inputDown, ref bool inputUp, int radialMenuIndex)
	{
		int count = global::UnityEngine.InputSystem.EnhancedTouch.Touch.activeTouches.Count;
		if (count > 0)
		{
			this.CurrentInputDevice = UltimateRadialMenuInputManager.InputDevice.Touch;
			if (this.touchInformationReset)
			{
				this.touchInformationReset = false;
			}
			for (int i = 0; i < count; i++)
			{
				Vector2 vector = Vector2.zero;
				vector = global::UnityEngine.InputSystem.EnhancedTouch.Touch.activeTouches[i].screenPosition;
				bool flag = global::UnityEngine.InputSystem.EnhancedTouch.Touch.activeTouches[i].phase == global::UnityEngine.InputSystem.TouchPhase.Began;
				bool flag2 = global::UnityEngine.InputSystem.EnhancedTouch.Touch.activeTouches[i].phase == global::UnityEngine.InputSystem.TouchPhase.Ended;
				int touchId = global::UnityEngine.InputSystem.EnhancedTouch.Touch.activeTouches[i].touchId;
				if (this.UltimateRadialMenuInformations[radialMenuIndex].interactFingerID < 0 || this.UltimateRadialMenuInformations[radialMenuIndex].interactFingerID == touchId)
				{
					float num = this.UltimateRadialMenuInformations[radialMenuIndex].radialMenu.BaseTransform.sizeDelta.x / 2f;
					Vector2 vector2 = vector / this.UltimateRadialMenuInformations[radialMenuIndex].radialMenu.ParentCanvas.scaleFactor - this.UltimateRadialMenuInformations[radialMenuIndex].radialMenu.ParentCanvas.GetComponent<RectTransform>().sizeDelta / 2f;
					Vector2 vector3 = Vector2.zero;
					float num2 = 0f;
					if (this.UltimateRadialMenuInformations[radialMenuIndex].radialMenu.IsWorldSpaceRadialMenu)
					{
						this.RaycastWorldSpaceRadialMenu(ref vector3, ref num2, vector, radialMenuIndex);
					}
					else
					{
						vector3 = (vector2 - this.UltimateRadialMenuInformations[radialMenuIndex].radialMenu.BaseTransform.localPosition) / num;
						num2 = Vector2.Distance(this.UltimateRadialMenuInformations[radialMenuIndex].radialMenu.BaseTransform.localPosition, vector2);
					}
					if (flag)
					{
						if (this.enableMenuSetting != UltimateRadialMenuInputManager.EnableMenuSetting.Manual && !this.UltimateRadialMenuInformations[radialMenuIndex].radialMenu.IsWorldSpaceRadialMenu && num2 < num * this.activationRadius)
						{
							this.UltimateRadialMenuInformations[radialMenuIndex].interactFingerID = touchId;
						}
						if (this.UltimateRadialMenuInformations[radialMenuIndex].radialMenu.IsEnabled)
						{
							float num3 = this.UltimateRadialMenuInformations[radialMenuIndex].radialMenu.maxRange;
							if (this.UltimateRadialMenuInformations[radialMenuIndex].radialMenu.infiniteMaxRange)
							{
								num3 = float.PositiveInfinity;
							}
							inputDown = true;
							if (num2 > this.UltimateRadialMenuInformations[radialMenuIndex].radialMenu.CalculatedMinRange && num2 < this.UltimateRadialMenuInformations[radialMenuIndex].radialMenu.CalculatedMaxRange)
							{
								this.UltimateRadialMenuInformations[radialMenuIndex].interactFingerID = touchId;
							}
							else if (this.enableMenuSetting != UltimateRadialMenuInputManager.EnableMenuSetting.Manual && !this.UltimateRadialMenuInformations[radialMenuIndex].radialMenu.IsWorldSpaceRadialMenu && this.activationRadius > 0f && (num2 > num * num3 || num2 < this.UltimateRadialMenuInformations[radialMenuIndex].radialMenu.CalculatedMinRange))
							{
								enableMenu = (disableMenu = true);
							}
							if (this.retainTouchPosition && num2 < this.UltimateRadialMenuInformations[radialMenuIndex].radialMenu.CalculatedMinRange)
							{
								this.UltimateRadialMenuInformations[radialMenuIndex].previousTouchPos = Vector2.zero;
								this.UltimateRadialMenuInformations[radialMenuIndex].previousTouchDist = 0f;
							}
						}
					}
					if (this.retainTouchPosition || this.UltimateRadialMenuInformations[radialMenuIndex].interactFingerID != -1)
					{
						if (this.enableMenuSetting != UltimateRadialMenuInputManager.EnableMenuSetting.Manual && !this.UltimateRadialMenuInformations[radialMenuIndex].radialMenu.IsWorldSpaceRadialMenu && !this.UltimateRadialMenuInformations[radialMenuIndex].radialMenu.IsEnabled)
						{
							if (num2 < num * this.activationRadius)
							{
								this.UltimateRadialMenuInformations[radialMenuIndex].currentHoldTime += Time.deltaTime;
								if (this.UltimateRadialMenuInformations[radialMenuIndex].currentHoldTime >= this.activationHoldTime)
								{
									this.UltimateRadialMenuInformations[radialMenuIndex].currentHoldTime = 0f;
									enableMenu = true;
									this.UltimateRadialMenuInformations[radialMenuIndex].touchActivatedRadialMenu = true;
									if (this.dynamicPositioning)
									{
										this.UltimateRadialMenuInformations[radialMenuIndex].radialMenu.SetPosition(vector2, true);
									}
								}
							}
						}
						else
						{
							this.CurrentInputDevice = UltimateRadialMenuInputManager.InputDevice.Touch;
							input = vector3;
							distance = num2;
						}
						if (flag2)
						{
							this.UltimateRadialMenuInformations[radialMenuIndex].interactFingerID = -1;
							this.UltimateRadialMenuInformations[radialMenuIndex].currentHoldTime = 0f;
							if (this.enableMenuSetting == UltimateRadialMenuInputManager.EnableMenuSetting.Manual || this.activationRadius <= 0f)
							{
								inputUp = true;
							}
							else
							{
								inputDown = (inputUp = true);
							}
							if (this.enableMenuSetting == UltimateRadialMenuInputManager.EnableMenuSetting.Hold && !this.UltimateRadialMenuInformations[radialMenuIndex].radialMenu.IsWorldSpaceRadialMenu && this.activationRadius > 0f)
							{
								disableMenu = true;
							}
						}
					}
				}
			}
		}
		else if (!this.touchInformationReset)
		{
			this.touchInformationReset = true;
			for (int j = 0; j < this.UltimateRadialMenuInformations.Count; j++)
			{
				if (this.UltimateRadialMenuInformations[j] != null)
				{
					this.UltimateRadialMenuInformations[j].currentHoldTime = 0f;
					this.UltimateRadialMenuInformations[j].interactFingerID = -1;
				}
			}
		}
		if (this.retainTouchPosition)
		{
			if (this.UltimateRadialMenuInformations[radialMenuIndex].interactFingerID < 0)
			{
				input = this.UltimateRadialMenuInformations[radialMenuIndex].previousTouchPos;
				distance = this.UltimateRadialMenuInformations[radialMenuIndex].previousTouchDist;
			}
			this.UltimateRadialMenuInformations[radialMenuIndex].previousTouchPos = input;
			this.UltimateRadialMenuInformations[radialMenuIndex].previousTouchDist = distance;
		}
	}

	// Token: 0x06000B87 RID: 2951 RVA: 0x0004F994 File Offset: 0x0004DB94
	public virtual void CenterScreenInput(ref bool enableMenu, ref bool disableMenu, ref Vector2 input, ref float distance, ref bool inputDown, ref bool inputUp, int radialMenuIndex)
	{
		if (!this.UltimateRadialMenuInformations[radialMenuIndex].radialMenu.IsWorldSpaceRadialMenu)
		{
			return;
		}
		Vector2 vector = Vector2.zero;
		float num = 0f;
		if (this.virtualReality)
		{
			if (this.leftEyeCamera == null || this.rightEyeCamera == null)
			{
				Debug.LogError(UltimateRadialMenuInputManager.FormatDebug("The left or right eye cameras are unassigned", "Please ensure that they are assigned in the inspector. If you need to update them at runtime, please use the SetCamerasVR() function", base.gameObject.name));
				return;
			}
			RaycastHit raycastHit;
			if (Physics.Raycast(new Ray(Vector3.Lerp(this.leftEyeCamera.transform.position, this.rightEyeCamera.transform.position, 0.5f), this.leftEyeCamera.transform.forward), out raycastHit, float.PositiveInfinity, this.worldSpaceMask) && raycastHit.collider.gameObject == this.UltimateRadialMenuInformations[radialMenuIndex].radialMenu.gameObject && (!this.forwardInputOnly || this.UltimateRadialMenuInformations[radialMenuIndex].radialMenu.BaseTransform.InverseTransformPoint(this.leftEyeCamera.transform.position).z <= 0f))
			{
				Vector3 vector2 = this.UltimateRadialMenuInformations[radialMenuIndex].radialMenu.BaseTransform.InverseTransformPoint(raycastHit.point);
				vector = vector2;
				num = Vector3.Distance(Vector2.zero, vector2);
			}
		}
		else
		{
			if (this.mainCamera == null || !this.mainCamera.isActiveAndEnabled)
			{
				this.UpdateCamera();
			}
			if (this.mainCamera == null)
			{
				return;
			}
			if (!this.forwardInputOnly || this.UltimateRadialMenuInformations[radialMenuIndex].radialMenu.BaseTransform.InverseTransformPoint(this.mainCamera.transform.position).z <= 0f)
			{
				this.RaycastWorldSpaceRadialMenu(ref vector, ref num, new Vector3((float)(Screen.width / 2), (float)(Screen.height / 2), 0f), radialMenuIndex);
			}
		}
		if (num > 0f && num <= this.UltimateRadialMenuInformations[radialMenuIndex].radialMenu.CalculatedMaxRange)
		{
			if (this.enableWhenInRange && !this.UltimateRadialMenuInformations[radialMenuIndex].radialMenu.IsEnabled)
			{
				this.UltimateRadialMenuInformations[radialMenuIndex].radialMenu.Enable();
			}
			if (this.trackInputPosition && this.UltimateRadialMenuInformations[radialMenuIndex].inputTracker != null)
			{
				this.UltimateRadialMenuInformations[radialMenuIndex].inputTracker.enabled = true;
			}
		}
		else if ((num > this.UltimateRadialMenuInformations[radialMenuIndex].radialMenu.CalculatedMaxRange || num <= 0f) && this.UltimateRadialMenuInformations[radialMenuIndex].radialMenu.IsEnabled)
		{
			if (this.enableWhenInRange)
			{
				this.UltimateRadialMenuInformations[radialMenuIndex].radialMenu.Disable();
			}
			if (this.trackInputPosition && this.UltimateRadialMenuInformations[radialMenuIndex].inputTracker != null)
			{
				this.UltimateRadialMenuInformations[radialMenuIndex].inputTracker.enabled = false;
			}
		}
		if (vector != Vector2.zero)
		{
			if (this.trackInputPosition && this.UltimateRadialMenuInformations[radialMenuIndex].inputTracker != null)
			{
				this.UltimateRadialMenuInformations[radialMenuIndex].inputTracker.rectTransform.localPosition = vector;
			}
			if (num >= this.UltimateRadialMenuInformations[radialMenuIndex].radialMenu.CalculatedMinRange && num <= this.UltimateRadialMenuInformations[radialMenuIndex].radialMenu.CalculatedMaxRange)
			{
				this.CurrentInputDevice = UltimateRadialMenuInputManager.InputDevice.CenterScreen;
				input = vector;
				distance = num;
			}
			if (this.interactOnHover && this.interactHoverTime > 0f)
			{
				if (distance >= this.UltimateRadialMenuInformations[radialMenuIndex].radialMenu.CalculatedMinRange && distance <= this.UltimateRadialMenuInformations[radialMenuIndex].radialMenu.CalculatedMaxRange)
				{
					if (this.UltimateRadialMenuInformations[radialMenuIndex].radialMenu.CurrentButtonIndex >= 0 && this.UltimateRadialMenuInformations[radialMenuIndex].currentButtonIndex != this.UltimateRadialMenuInformations[radialMenuIndex].radialMenu.CurrentButtonIndex)
					{
						this.UltimateRadialMenuInformations[radialMenuIndex].currentButtonIndex = this.UltimateRadialMenuInformations[radialMenuIndex].radialMenu.CurrentButtonIndex;
						this.UltimateRadialMenuInformations[radialMenuIndex].interactHoldTime = 0f;
					}
					if (this.UltimateRadialMenuInformations[radialMenuIndex].currentButtonIndex == this.UltimateRadialMenuInformations[radialMenuIndex].radialMenu.CurrentButtonIndex && this.UltimateRadialMenuInformations[radialMenuIndex].interactHoldTime <= this.interactHoverTime)
					{
						this.UltimateRadialMenuInformations[radialMenuIndex].interactHoldTime += Time.deltaTime;
						if (this.UltimateRadialMenuInformations[radialMenuIndex].interactHoldTime >= this.interactHoverTime)
						{
							inputDown = (inputUp = true);
							return;
						}
					}
				}
				else
				{
					if (this.UltimateRadialMenuInformations[radialMenuIndex].interactHoldTime > 0f)
					{
						this.UltimateRadialMenuInformations[radialMenuIndex].interactHoldTime = 0f;
					}
					if (this.UltimateRadialMenuInformations[radialMenuIndex].currentButtonIndex > 0)
					{
						this.UltimateRadialMenuInformations[radialMenuIndex].currentButtonIndex = -1;
					}
				}
			}
		}
	}

	// Token: 0x06000B88 RID: 2952 RVA: 0x00004F31 File Offset: 0x00003131
	public virtual void CustomInput(ref bool enableMenu, ref bool disableMenu, ref Vector2 input, ref float distance, ref bool inputDown, ref bool inputUp, int radialMenuIndex)
	{
	}

	// Token: 0x06000B89 RID: 2953 RVA: 0x0004FF4C File Offset: 0x0004E14C
	public void SendRaycastInput(Vector3 rayStart, Vector3 rayEnd)
	{
		this.SendRaycastInput(rayStart, (rayEnd - rayStart).normalized, Vector3.Distance(rayStart, rayEnd));
	}

	// Token: 0x06000B8A RID: 2954 RVA: 0x0004FF78 File Offset: 0x0004E178
	public void SendRaycastInput(Vector3 rayOrigin, Vector3 rayDirection, float rayDistance)
	{
		this.EnsureInformationList();
		for (int i = 0; i < this.UltimateRadialMenuInformations.Count; i++)
		{
			if (this.UltimateRadialMenuInformations[i] != null && !(this.UltimateRadialMenuInformations[i].radialMenu == null))
			{
				if (!this.UltimateRadialMenuInformations[i].radialMenu.IsWorldSpaceRadialMenu)
				{
					return;
				}
				if (this.mainCamera == null || !this.mainCamera.gameObject.activeInHierarchy || !this.mainCamera.enabled)
				{
					this.UpdateCamera();
				}
				Ray ray = new Ray(rayOrigin, rayDirection);
				Vector2 vector = Vector2.zero;
				RaycastHit raycastHit;
				if (Physics.Raycast(ray, out raycastHit, rayDistance, this.worldSpaceMask) && raycastHit.collider.gameObject == this.UltimateRadialMenuInformations[i].radialMenu.gameObject)
				{
					Vector3 vector2 = this.UltimateRadialMenuInformations[i].radialMenu.BaseTransform.InverseTransformPoint(raycastHit.point);
					vector = vector2;
					float num = Vector3.Distance(Vector2.zero, vector2);
					this.UltimateRadialMenuInformations[i].raycastInputCalculated = true;
					this.UltimateRadialMenuInformations[i].raycastInput = vector;
					this.UltimateRadialMenuInformations[i].raycastDistance = num;
					this.CurrentInputDevice = UltimateRadialMenuInputManager.InputDevice.Other;
				}
			}
		}
	}

	// Token: 0x06000B8B RID: 2955 RVA: 0x000500EF File Offset: 0x0004E2EF
	public void TriggerInputDown()
	{
		this.customInputCalculated = true;
		this.customInputDown = true;
	}

	// Token: 0x06000B8C RID: 2956 RVA: 0x000500FF File Offset: 0x0004E2FF
	public void TriggerInputUp()
	{
		this.customInputCalculated = true;
		this.customInputUp = true;
	}

	// Token: 0x06000B8D RID: 2957 RVA: 0x00050110 File Offset: 0x0004E310
	public void TriggerInputInteract()
	{
		this.customInputCalculated = true;
		this.customInputDown = (this.customInputUp = true);
	}

	// Token: 0x06000B8E RID: 2958 RVA: 0x00050134 File Offset: 0x0004E334
	public void TriggerEnable()
	{
		this.customInputCalculated = true;
		this.customInputEnable = true;
	}

	// Token: 0x06000B8F RID: 2959 RVA: 0x00050144 File Offset: 0x0004E344
	public void TriggerDisable()
	{
		this.customInputCalculated = true;
		this.customInputDisable = true;
	}

	// Token: 0x04000AD5 RID: 2773
	protected Camera mainCamera;

	// Token: 0x04000AD7 RID: 2775
	private LayerMask worldSpaceMask;

	// Token: 0x04000AD8 RID: 2776
	private bool customInputCalculated;

	// Token: 0x04000AD9 RID: 2777
	private bool customInputDown;

	// Token: 0x04000ADA RID: 2778
	private bool customInputUp;

	// Token: 0x04000ADB RID: 2779
	private bool customInputEnable;

	// Token: 0x04000ADC RID: 2780
	private bool customInputDisable;

	// Token: 0x04000ADE RID: 2782
	[Header("Interact Settings")]
	[Tooltip("The action required to invoke the radial button.")]
	public UltimateRadialMenuInputManager.InvokeAction invokeAction;

	// Token: 0x04000ADF RID: 2783
	[Tooltip("Determines whether or not the Ultimate Radial Menu will interact with the current button when the menu is disabled.")]
	public bool onMenuRelease;

	// Token: 0x04000AE0 RID: 2784
	[Tooltip("Determines if the Ultimate Radial Menu should be disabled when the interaction occurs. \n\nNOTE: World space radial menus will not be disabled on interact. They must be disabled manually.")]
	public bool disableOnInteract;

	// Token: 0x04000AE1 RID: 2785
	[Tooltip("Determines if this Input Manager should handle enabling and disabling the radial menus in the scene, and how that should be done.")]
	public UltimateRadialMenuInputManager.EnableMenuSetting enableMenuSetting = UltimateRadialMenuInputManager.EnableMenuSetting.Hold;

	// Token: 0x04000AE2 RID: 2786
	[Header("Mouse and Keyboard Settings")]
	[Tooltip("Determines if mouse and keyboard input should be used to send to the Ultimate Radial Menu.")]
	public bool keyboardInput = true;

	// Token: 0x04000AE3 RID: 2787
	[Tooltip("The mouse button to be used to interact with the radial menus.")]
	public UltimateRadialMenuInputManager.MouseInteractButtons mouseInteractButton;

	// Token: 0x04000AE4 RID: 2788
	[Tooltip("The keyboard key to be used to enable/disable the radial menus.")]
	public UltimateRadialMenuInputManager.KeyboardEnableKeys keyboardEnableKey = UltimateRadialMenuInputManager.KeyboardEnableKeys.Tab;

	// Token: 0x04000AE5 RID: 2789
	[Header("Controller Settings")]
	[Tooltip("Determines if controller input should be used to send to the Ultimate Radial Menu.")]
	public bool controllerInput;

	// Token: 0x04000AE6 RID: 2790
	[Tooltip("Determines if the horizontal input should be inverted or not.")]
	public bool invertHorizontal;

	// Token: 0x04000AE7 RID: 2791
	[Tooltip("Determines if the vertical input should be inverted or not.")]
	public bool invertVertical;

	// Token: 0x04000AE8 RID: 2792
	[Tooltip("The controller joystick to be used for navigating the radial menu.")]
	public UltimateRadialMenuInputManager.Joysticks joystick;

	// Token: 0x04000AE9 RID: 2793
	[Tooltip("The buttons to be used for interacting with the radial menu buttons.")]
	public UltimateRadialMenuInputManager.ControllerButtons interactButtons;

	// Token: 0x04000AEA RID: 2794
	[Tooltip("The buttons to be used for enabling the radial menu.")]
	public UltimateRadialMenuInputManager.ControllerButtons enableButtons;

	// Token: 0x04000AEB RID: 2795
	[Header("Touch Settings")]
	[Tooltip("Determines if touch input should be used to send to the Ultimate Radial Menu.")]
	public bool touchInput;

	// Token: 0x04000AEC RID: 2796
	[Tooltip("Should the radial menu move to the initial touch position?")]
	public bool dynamicPositioning;

	// Token: 0x04000AED RID: 2797
	[Range(0f, 2f)]
	[Tooltip("The activation radius for enabling the menu.")]
	public float activationRadius = 0.25f;

	// Token: 0x04000AEE RID: 2798
	[Tooltip("Time in seconds that the user needs to hold the touch within the activation radius.")]
	public float activationHoldTime = 0.25f;

	// Token: 0x04000AEF RID: 2799
	private bool touchInformationReset = true;

	// Token: 0x04000AF0 RID: 2800
	[Tooltip("Should the touch input position be stored and used even if the touch is released?")]
	public bool retainTouchPosition;

	// Token: 0x04000AF1 RID: 2801
	[Header("Center Screen Settings")]
	[Tooltip("Determines if the menu should activated by the center of the screen.")]
	public bool centerScreenInput;

	// Token: 0x04000AF2 RID: 2802
	[SerializeField]
	[Tooltip("Should hovering over the menu for an amount of time interact with the menu?")]
	private bool interactOnHover;

	// Token: 0x04000AF3 RID: 2803
	[SerializeField]
	[Tooltip("Time is seconds the player must hover over a button to interact with it.")]
	private float interactHoverTime = 1f;

	// Token: 0x04000AF4 RID: 2804
	[Tooltip("Should the calculations use two cameras in order to calculate the center of where the player is looking?")]
	[SerializeField]
	private bool virtualReality;

	// Token: 0x04000AF5 RID: 2805
	[SerializeField]
	private Camera leftEyeCamera;

	// Token: 0x04000AF6 RID: 2806
	[SerializeField]
	private Camera rightEyeCamera;

	// Token: 0x04000AF7 RID: 2807
	[SerializeField]
	[Tooltip("Should this Input Manager use a image to display the position of the input?")]
	private bool trackInputPosition;

	// Token: 0x04000AF8 RID: 2808
	[SerializeField]
	[Tooltip("The sprite to be used for tracking the input.")]
	private Sprite trackInputSprite;

	// Token: 0x04000AF9 RID: 2809
	[SerializeField]
	[Range(0.01f, 0.25f)]
	[Tooltip("The size of the input tracker image.")]
	private float inputTrackerSize = 0.1f;

	// Token: 0x04000AFA RID: 2810
	[SerializeField]
	[Tooltip("When enabled, the input manager will only send input to the menus when the cameras are in front of the menu, not behind.")]
	private bool forwardInputOnly;

	// Token: 0x04000AFB RID: 2811
	[SerializeField]
	[Tooltip("Should the input manager enable the menu when the input is within range?")]
	private bool enableWhenInRange;

	// Token: 0x04000AFC RID: 2812
	[Header("Custom Input Settings")]
	public bool customInput;

	// Token: 0x04000AFD RID: 2813
	public bool customControllerInput;

	// Token: 0x04000AFE RID: 2814
	public InputAction customControllerJoystick;

	// Token: 0x04000AFF RID: 2815
	public InputAction customControllerInteract;

	// Token: 0x04000B00 RID: 2816
	public InputAction customControllerEnable;

	// Token: 0x04000B01 RID: 2817
	private bool customControllerCalculated;

	// Token: 0x04000B02 RID: 2818
	private bool customControllerInputDown;

	// Token: 0x04000B03 RID: 2819
	private bool customControllerInputUp;

	// Token: 0x04000B04 RID: 2820
	private bool customControllerEnableMenu;

	// Token: 0x04000B05 RID: 2821
	private bool customControllerDisableMenu;

	// Token: 0x04000B06 RID: 2822
	private bool enhancedTouchEnabled;

	// Token: 0x0200015C RID: 348
	[Serializable]
	public class UltimateRadialMenuInformation
	{
		// Token: 0x06000B91 RID: 2961 RVA: 0x000501AF File Offset: 0x0004E3AF
		public void ResetMenuPosition()
		{
			if (!this.touchActivatedRadialMenu)
			{
				return;
			}
			this.touchActivatedRadialMenu = false;
			if (this.radialMenu != null)
			{
				this.radialMenu.ResetPosition();
			}
		}

		// Token: 0x06000B92 RID: 2962 RVA: 0x000501DC File Offset: 0x0004E3DC
		public void UpdatePositioning()
		{
			if (this.inputTracker == null || this.radialMenu == null || this.radialMenu.BaseTransform == null || this.inputManager == null)
			{
				return;
			}
			this.inputTracker.rectTransform.sizeDelta = this.radialMenu.BaseTransform.sizeDelta * this.inputManager.inputTrackerSize;
		}

		// Token: 0x04000B07 RID: 2823
		public UltimateRadialMenuInputManager inputManager;

		// Token: 0x04000B08 RID: 2824
		public UltimateRadialMenu radialMenu;

		// Token: 0x04000B09 RID: 2825
		public Vector2 previousMouseInput = Vector2.zero;

		// Token: 0x04000B0A RID: 2826
		public float interactHoldTime;

		// Token: 0x04000B0B RID: 2827
		public int currentButtonIndex = -1;

		// Token: 0x04000B0C RID: 2828
		public Image inputTracker;

		// Token: 0x04000B0D RID: 2829
		public bool raycastInputCalculated;

		// Token: 0x04000B0E RID: 2830
		public Vector2 raycastInput;

		// Token: 0x04000B0F RID: 2831
		public float raycastDistance;

		// Token: 0x04000B10 RID: 2832
		public float currentHoldTime;

		// Token: 0x04000B11 RID: 2833
		public int interactFingerID = -1;

		// Token: 0x04000B12 RID: 2834
		public Vector2 previousTouchPos = Vector2.zero;

		// Token: 0x04000B13 RID: 2835
		public float previousTouchDist;

		// Token: 0x04000B14 RID: 2836
		public bool touchActivatedRadialMenu;
	}

	// Token: 0x0200015D RID: 349
	public enum InputDevice
	{
		// Token: 0x04000B16 RID: 2838
		None,
		// Token: 0x04000B17 RID: 2839
		Mouse,
		// Token: 0x04000B18 RID: 2840
		Controller,
		// Token: 0x04000B19 RID: 2841
		Touch,
		// Token: 0x04000B1A RID: 2842
		CenterScreen,
		// Token: 0x04000B1B RID: 2843
		Other
	}

	// Token: 0x0200015E RID: 350
	public enum InvokeAction
	{
		// Token: 0x04000B1D RID: 2845
		OnButtonDown,
		// Token: 0x04000B1E RID: 2846
		OnButtonClick,
		// Token: 0x04000B1F RID: 2847
		OnButtonUp,
		// Token: 0x04000B20 RID: 2848
		OnButtonEnter
	}

	// Token: 0x0200015F RID: 351
	public enum EnableMenuSetting
	{
		// Token: 0x04000B22 RID: 2850
		Toggle,
		// Token: 0x04000B23 RID: 2851
		Hold,
		// Token: 0x04000B24 RID: 2852
		Manual
	}

	// Token: 0x02000160 RID: 352
	public enum MouseInteractButtons
	{
		// Token: 0x04000B26 RID: 2854
		Left,
		// Token: 0x04000B27 RID: 2855
		Right,
		// Token: 0x04000B28 RID: 2856
		Both
	}

	// Token: 0x02000161 RID: 353
	public enum KeyboardEnableKeys
	{
		// Token: 0x04000B2A RID: 2858
		EnableManually,
		// Token: 0x04000B2B RID: 2859
		Tab,
		// Token: 0x04000B2C RID: 2860
		LeftAlt,
		// Token: 0x04000B2D RID: 2861
		LeftControl,
		// Token: 0x04000B2E RID: 2862
		CapsLock,
		// Token: 0x04000B2F RID: 2863
		Tilde,
		// Token: 0x04000B30 RID: 2864
		Escape
	}

	// Token: 0x02000162 RID: 354
	public enum Joysticks
	{
		// Token: 0x04000B32 RID: 2866
		Left,
		// Token: 0x04000B33 RID: 2867
		Right
	}

	// Token: 0x02000163 RID: 355
	public enum ControllerButtons
	{
		// Token: 0x04000B35 RID: 2869
		Nothing,
		// Token: 0x04000B36 RID: 2870
		North,
		// Token: 0x04000B37 RID: 2871
		South,
		// Token: 0x04000B38 RID: 2872
		East = 4,
		// Token: 0x04000B39 RID: 2873
		West = 8,
		// Token: 0x04000B3A RID: 2874
		LeftJoystick = 16,
		// Token: 0x04000B3B RID: 2875
		LeftShoulder = 32,
		// Token: 0x04000B3C RID: 2876
		LeftTrigger = 64,
		// Token: 0x04000B3D RID: 2877
		RightJoystick = 128,
		// Token: 0x04000B3E RID: 2878
		RightShoulder = 256,
		// Token: 0x04000B3F RID: 2879
		RightTrigger = 512,
		// Token: 0x04000B40 RID: 2880
		Start = 1024,
		// Token: 0x04000B41 RID: 2881
		Select = 2048,
		// Token: 0x04000B42 RID: 2882
		DpadUp = 4096,
		// Token: 0x04000B43 RID: 2883
		DpadDown = 8192,
		// Token: 0x04000B44 RID: 2884
		DpadLeft = 16384,
		// Token: 0x04000B45 RID: 2885
		DpadRight = 32768
	}
}
