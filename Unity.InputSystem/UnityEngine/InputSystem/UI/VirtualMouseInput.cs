using System;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.InputSystem.Utilities;
using UnityEngine.UI;

namespace UnityEngine.InputSystem.UI
{
	// Token: 0x02000092 RID: 146
	[AddComponentMenu("Input/Virtual Mouse")]
	[HelpURL("https://docs.unity3d.com/Packages/com.unity.inputsystem@1.14/manual/UISupport.html#virtual-mouse-cursor-control")]
	public class VirtualMouseInput : MonoBehaviour
	{
		// Token: 0x17000310 RID: 784
		// (get) Token: 0x06000BB7 RID: 2999 RVA: 0x0003E889 File Offset: 0x0003CA89
		// (set) Token: 0x06000BB8 RID: 3000 RVA: 0x0003E891 File Offset: 0x0003CA91
		public RectTransform cursorTransform
		{
			get
			{
				return this.m_CursorTransform;
			}
			set
			{
				this.m_CursorTransform = value;
			}
		}

		// Token: 0x17000311 RID: 785
		// (get) Token: 0x06000BB9 RID: 3001 RVA: 0x0003E89A File Offset: 0x0003CA9A
		// (set) Token: 0x06000BBA RID: 3002 RVA: 0x0003E8A2 File Offset: 0x0003CAA2
		public float cursorSpeed
		{
			get
			{
				return this.m_CursorSpeed;
			}
			set
			{
				this.m_CursorSpeed = value;
			}
		}

		// Token: 0x17000312 RID: 786
		// (get) Token: 0x06000BBB RID: 3003 RVA: 0x0003E8AB File Offset: 0x0003CAAB
		// (set) Token: 0x06000BBC RID: 3004 RVA: 0x0003E8B4 File Offset: 0x0003CAB4
		public VirtualMouseInput.CursorMode cursorMode
		{
			get
			{
				return this.m_CursorMode;
			}
			set
			{
				if (this.m_CursorMode == value)
				{
					return;
				}
				if (this.m_CursorMode == VirtualMouseInput.CursorMode.HardwareCursorIfAvailable && this.m_SystemMouse != null)
				{
					InputSystem.EnableDevice(this.m_SystemMouse);
					this.m_SystemMouse = null;
				}
				this.m_CursorMode = value;
				if (this.m_CursorMode == VirtualMouseInput.CursorMode.HardwareCursorIfAvailable)
				{
					this.TryEnableHardwareCursor();
					return;
				}
				if (this.m_CursorGraphic != null)
				{
					this.m_CursorGraphic.enabled = true;
				}
			}
		}

		// Token: 0x17000313 RID: 787
		// (get) Token: 0x06000BBD RID: 3005 RVA: 0x0003E91F File Offset: 0x0003CB1F
		// (set) Token: 0x06000BBE RID: 3006 RVA: 0x0003E927 File Offset: 0x0003CB27
		public Graphic cursorGraphic
		{
			get
			{
				return this.m_CursorGraphic;
			}
			set
			{
				this.m_CursorGraphic = value;
				this.TryFindCanvas();
			}
		}

		// Token: 0x17000314 RID: 788
		// (get) Token: 0x06000BBF RID: 3007 RVA: 0x0003E936 File Offset: 0x0003CB36
		// (set) Token: 0x06000BC0 RID: 3008 RVA: 0x0003E93E File Offset: 0x0003CB3E
		public float scrollSpeed
		{
			get
			{
				return this.m_ScrollSpeed;
			}
			set
			{
				this.m_ScrollSpeed = value;
			}
		}

		// Token: 0x17000315 RID: 789
		// (get) Token: 0x06000BC1 RID: 3009 RVA: 0x0003E947 File Offset: 0x0003CB47
		public Mouse virtualMouse
		{
			get
			{
				return this.m_VirtualMouse;
			}
		}

		// Token: 0x17000316 RID: 790
		// (get) Token: 0x06000BC2 RID: 3010 RVA: 0x0003E94F File Offset: 0x0003CB4F
		// (set) Token: 0x06000BC3 RID: 3011 RVA: 0x0003E957 File Offset: 0x0003CB57
		public InputActionProperty stickAction
		{
			get
			{
				return this.m_StickAction;
			}
			set
			{
				VirtualMouseInput.SetAction(ref this.m_StickAction, value);
			}
		}

		// Token: 0x17000317 RID: 791
		// (get) Token: 0x06000BC4 RID: 3012 RVA: 0x0003E965 File Offset: 0x0003CB65
		// (set) Token: 0x06000BC5 RID: 3013 RVA: 0x0003E970 File Offset: 0x0003CB70
		public InputActionProperty leftButtonAction
		{
			get
			{
				return this.m_LeftButtonAction;
			}
			set
			{
				if (this.m_ButtonActionTriggeredDelegate != null)
				{
					VirtualMouseInput.SetActionCallback(this.m_LeftButtonAction, this.m_ButtonActionTriggeredDelegate, false);
				}
				VirtualMouseInput.SetAction(ref this.m_LeftButtonAction, value);
				if (this.m_ButtonActionTriggeredDelegate != null)
				{
					VirtualMouseInput.SetActionCallback(this.m_LeftButtonAction, this.m_ButtonActionTriggeredDelegate, true);
				}
			}
		}

		// Token: 0x17000318 RID: 792
		// (get) Token: 0x06000BC6 RID: 3014 RVA: 0x0003E9BD File Offset: 0x0003CBBD
		// (set) Token: 0x06000BC7 RID: 3015 RVA: 0x0003E9C8 File Offset: 0x0003CBC8
		public InputActionProperty rightButtonAction
		{
			get
			{
				return this.m_RightButtonAction;
			}
			set
			{
				if (this.m_ButtonActionTriggeredDelegate != null)
				{
					VirtualMouseInput.SetActionCallback(this.m_RightButtonAction, this.m_ButtonActionTriggeredDelegate, false);
				}
				VirtualMouseInput.SetAction(ref this.m_RightButtonAction, value);
				if (this.m_ButtonActionTriggeredDelegate != null)
				{
					VirtualMouseInput.SetActionCallback(this.m_RightButtonAction, this.m_ButtonActionTriggeredDelegate, true);
				}
			}
		}

		// Token: 0x17000319 RID: 793
		// (get) Token: 0x06000BC8 RID: 3016 RVA: 0x0003EA15 File Offset: 0x0003CC15
		// (set) Token: 0x06000BC9 RID: 3017 RVA: 0x0003EA20 File Offset: 0x0003CC20
		public InputActionProperty middleButtonAction
		{
			get
			{
				return this.m_MiddleButtonAction;
			}
			set
			{
				if (this.m_ButtonActionTriggeredDelegate != null)
				{
					VirtualMouseInput.SetActionCallback(this.m_MiddleButtonAction, this.m_ButtonActionTriggeredDelegate, false);
				}
				VirtualMouseInput.SetAction(ref this.m_MiddleButtonAction, value);
				if (this.m_ButtonActionTriggeredDelegate != null)
				{
					VirtualMouseInput.SetActionCallback(this.m_MiddleButtonAction, this.m_ButtonActionTriggeredDelegate, true);
				}
			}
		}

		// Token: 0x1700031A RID: 794
		// (get) Token: 0x06000BCA RID: 3018 RVA: 0x0003EA6D File Offset: 0x0003CC6D
		// (set) Token: 0x06000BCB RID: 3019 RVA: 0x0003EA78 File Offset: 0x0003CC78
		public InputActionProperty forwardButtonAction
		{
			get
			{
				return this.m_ForwardButtonAction;
			}
			set
			{
				if (this.m_ButtonActionTriggeredDelegate != null)
				{
					VirtualMouseInput.SetActionCallback(this.m_ForwardButtonAction, this.m_ButtonActionTriggeredDelegate, false);
				}
				VirtualMouseInput.SetAction(ref this.m_ForwardButtonAction, value);
				if (this.m_ButtonActionTriggeredDelegate != null)
				{
					VirtualMouseInput.SetActionCallback(this.m_ForwardButtonAction, this.m_ButtonActionTriggeredDelegate, true);
				}
			}
		}

		// Token: 0x1700031B RID: 795
		// (get) Token: 0x06000BCC RID: 3020 RVA: 0x0003EAC5 File Offset: 0x0003CCC5
		// (set) Token: 0x06000BCD RID: 3021 RVA: 0x0003EAD0 File Offset: 0x0003CCD0
		public InputActionProperty backButtonAction
		{
			get
			{
				return this.m_BackButtonAction;
			}
			set
			{
				if (this.m_ButtonActionTriggeredDelegate != null)
				{
					VirtualMouseInput.SetActionCallback(this.m_BackButtonAction, this.m_ButtonActionTriggeredDelegate, false);
				}
				VirtualMouseInput.SetAction(ref this.m_BackButtonAction, value);
				if (this.m_ButtonActionTriggeredDelegate != null)
				{
					VirtualMouseInput.SetActionCallback(this.m_BackButtonAction, this.m_ButtonActionTriggeredDelegate, true);
				}
			}
		}

		// Token: 0x1700031C RID: 796
		// (get) Token: 0x06000BCE RID: 3022 RVA: 0x0003EB1D File Offset: 0x0003CD1D
		// (set) Token: 0x06000BCF RID: 3023 RVA: 0x0003EB25 File Offset: 0x0003CD25
		public InputActionProperty scrollWheelAction
		{
			get
			{
				return this.m_ScrollWheelAction;
			}
			set
			{
				VirtualMouseInput.SetAction(ref this.m_ScrollWheelAction, value);
			}
		}

		// Token: 0x06000BD0 RID: 3024 RVA: 0x0003EB34 File Offset: 0x0003CD34
		protected void OnEnable()
		{
			if (this.m_CursorMode == VirtualMouseInput.CursorMode.HardwareCursorIfAvailable)
			{
				this.TryEnableHardwareCursor();
			}
			if (this.m_VirtualMouse == null)
			{
				this.m_VirtualMouse = (Mouse)InputSystem.AddDevice("VirtualMouse", null, null);
			}
			else if (!this.m_VirtualMouse.added)
			{
				InputSystem.AddDevice(this.m_VirtualMouse);
			}
			if (this.m_CursorTransform != null)
			{
				Vector2 anchoredPosition = this.m_CursorTransform.anchoredPosition;
				InputState.Change<Vector2>(this.m_VirtualMouse.position, anchoredPosition, InputUpdateType.None, default(InputEventPtr));
				Mouse systemMouse = this.m_SystemMouse;
				if (systemMouse != null)
				{
					systemMouse.WarpCursorPosition(anchoredPosition);
				}
			}
			if (this.m_AfterInputUpdateDelegate == null)
			{
				this.m_AfterInputUpdateDelegate = new Action(this.OnAfterInputUpdate);
			}
			InputSystem.onAfterUpdate += this.m_AfterInputUpdateDelegate;
			if (this.m_ButtonActionTriggeredDelegate == null)
			{
				this.m_ButtonActionTriggeredDelegate = new Action<InputAction.CallbackContext>(this.OnButtonActionTriggered);
			}
			VirtualMouseInput.SetActionCallback(this.m_LeftButtonAction, this.m_ButtonActionTriggeredDelegate, true);
			VirtualMouseInput.SetActionCallback(this.m_RightButtonAction, this.m_ButtonActionTriggeredDelegate, true);
			VirtualMouseInput.SetActionCallback(this.m_MiddleButtonAction, this.m_ButtonActionTriggeredDelegate, true);
			VirtualMouseInput.SetActionCallback(this.m_ForwardButtonAction, this.m_ButtonActionTriggeredDelegate, true);
			VirtualMouseInput.SetActionCallback(this.m_BackButtonAction, this.m_ButtonActionTriggeredDelegate, true);
			InputAction action = this.m_StickAction.action;
			if (action != null)
			{
				action.Enable();
			}
			InputAction action2 = this.m_LeftButtonAction.action;
			if (action2 != null)
			{
				action2.Enable();
			}
			InputAction action3 = this.m_RightButtonAction.action;
			if (action3 != null)
			{
				action3.Enable();
			}
			InputAction action4 = this.m_MiddleButtonAction.action;
			if (action4 != null)
			{
				action4.Enable();
			}
			InputAction action5 = this.m_ForwardButtonAction.action;
			if (action5 != null)
			{
				action5.Enable();
			}
			InputAction action6 = this.m_BackButtonAction.action;
			if (action6 != null)
			{
				action6.Enable();
			}
			InputAction action7 = this.m_ScrollWheelAction.action;
			if (action7 == null)
			{
				return;
			}
			action7.Enable();
		}

		// Token: 0x06000BD1 RID: 3025 RVA: 0x0003ED04 File Offset: 0x0003CF04
		protected void OnDisable()
		{
			if (this.m_VirtualMouse != null && this.m_VirtualMouse.added)
			{
				InputSystem.RemoveDevice(this.m_VirtualMouse);
			}
			if (this.m_SystemMouse != null)
			{
				InputSystem.EnableDevice(this.m_SystemMouse);
				this.m_SystemMouse = null;
			}
			if (this.m_AfterInputUpdateDelegate != null)
			{
				InputSystem.onAfterUpdate -= this.m_AfterInputUpdateDelegate;
			}
			InputAction action = this.m_StickAction.action;
			if (action != null)
			{
				action.Disable();
			}
			InputAction action2 = this.m_LeftButtonAction.action;
			if (action2 != null)
			{
				action2.Disable();
			}
			InputAction action3 = this.m_RightButtonAction.action;
			if (action3 != null)
			{
				action3.Disable();
			}
			InputAction action4 = this.m_MiddleButtonAction.action;
			if (action4 != null)
			{
				action4.Disable();
			}
			InputAction action5 = this.m_ForwardButtonAction.action;
			if (action5 != null)
			{
				action5.Disable();
			}
			InputAction action6 = this.m_BackButtonAction.action;
			if (action6 != null)
			{
				action6.Disable();
			}
			InputAction action7 = this.m_ScrollWheelAction.action;
			if (action7 != null)
			{
				action7.Disable();
			}
			if (this.m_ButtonActionTriggeredDelegate != null)
			{
				VirtualMouseInput.SetActionCallback(this.m_LeftButtonAction, this.m_ButtonActionTriggeredDelegate, false);
				VirtualMouseInput.SetActionCallback(this.m_RightButtonAction, this.m_ButtonActionTriggeredDelegate, false);
				VirtualMouseInput.SetActionCallback(this.m_MiddleButtonAction, this.m_ButtonActionTriggeredDelegate, false);
				VirtualMouseInput.SetActionCallback(this.m_ForwardButtonAction, this.m_ButtonActionTriggeredDelegate, false);
				VirtualMouseInput.SetActionCallback(this.m_BackButtonAction, this.m_ButtonActionTriggeredDelegate, false);
			}
			this.m_LastTime = 0.0;
			this.m_LastStickValue = default(Vector2);
		}

		// Token: 0x06000BD2 RID: 3026 RVA: 0x0003EE75 File Offset: 0x0003D075
		private void TryFindCanvas()
		{
			Graphic cursorGraphic = this.m_CursorGraphic;
			this.m_Canvas = ((cursorGraphic != null) ? cursorGraphic.GetComponentInParent<Canvas>() : null);
		}

		// Token: 0x06000BD3 RID: 3027 RVA: 0x0003EE90 File Offset: 0x0003D090
		private unsafe void TryEnableHardwareCursor()
		{
			ReadOnlyArray<InputDevice> devices = InputSystem.devices;
			for (int i = 0; i < devices.Count; i++)
			{
				InputDevice inputDevice = devices[i];
				if (inputDevice.native)
				{
					Mouse mouse = inputDevice as Mouse;
					if (mouse != null)
					{
						this.m_SystemMouse = mouse;
						break;
					}
				}
			}
			if (this.m_SystemMouse == null)
			{
				if (this.m_CursorGraphic != null)
				{
					this.m_CursorGraphic.enabled = true;
				}
				return;
			}
			InputSystem.DisableDevice(this.m_SystemMouse, false);
			if (this.m_VirtualMouse != null)
			{
				this.m_SystemMouse.WarpCursorPosition(*this.m_VirtualMouse.position.value);
			}
			if (this.m_CursorGraphic != null)
			{
				this.m_CursorGraphic.enabled = false;
			}
		}

		// Token: 0x06000BD4 RID: 3028 RVA: 0x0003EF4C File Offset: 0x0003D14C
		private unsafe void UpdateMotion()
		{
			if (this.m_VirtualMouse == null)
			{
				return;
			}
			InputAction action = this.m_StickAction.action;
			if (action == null)
			{
				return;
			}
			Vector2 vector = action.ReadValue<Vector2>();
			if (Mathf.Approximately(0f, vector.x) && Mathf.Approximately(0f, vector.y))
			{
				this.m_LastTime = 0.0;
				this.m_LastStickValue = default(Vector2);
			}
			else
			{
				double currentTime = InputState.currentTime;
				if (Mathf.Approximately(0f, this.m_LastStickValue.x) && Mathf.Approximately(0f, this.m_LastStickValue.y))
				{
					this.m_LastTime = currentTime;
				}
				float num = (float)(currentTime - this.m_LastTime);
				Vector2 vector2 = new Vector2(this.m_CursorSpeed * vector.x * num, this.m_CursorSpeed * vector.y * num);
				Vector2 vector3 = *this.m_VirtualMouse.position.value + vector2;
				if (this.m_Canvas != null)
				{
					Rect pixelRect = this.m_Canvas.pixelRect;
					vector3.x = Mathf.Clamp(vector3.x, pixelRect.xMin, pixelRect.xMax);
					vector3.y = Mathf.Clamp(vector3.y, pixelRect.yMin, pixelRect.yMax);
				}
				InputState.Change<Vector2>(this.m_VirtualMouse.position, vector3, InputUpdateType.None, default(InputEventPtr));
				InputState.Change<Vector2>(this.m_VirtualMouse.delta, vector2, InputUpdateType.None, default(InputEventPtr));
				if (this.m_CursorTransform != null && (this.m_CursorMode == VirtualMouseInput.CursorMode.SoftwareCursor || (this.m_CursorMode == VirtualMouseInput.CursorMode.HardwareCursorIfAvailable && this.m_SystemMouse == null)))
				{
					this.m_CursorTransform.anchoredPosition = vector3;
				}
				this.m_LastStickValue = vector;
				this.m_LastTime = currentTime;
				Mouse systemMouse = this.m_SystemMouse;
				if (systemMouse != null)
				{
					systemMouse.WarpCursorPosition(vector3);
				}
			}
			InputAction action2 = this.m_ScrollWheelAction.action;
			if (action2 != null)
			{
				Vector2 vector4 = action2.ReadValue<Vector2>();
				vector4.x *= this.m_ScrollSpeed;
				vector4.y *= this.m_ScrollSpeed;
				InputState.Change<Vector2>(this.m_VirtualMouse.scroll, vector4, InputUpdateType.None, default(InputEventPtr));
			}
		}

		// Token: 0x06000BD5 RID: 3029 RVA: 0x0003F18C File Offset: 0x0003D38C
		private void OnButtonActionTriggered(InputAction.CallbackContext context)
		{
			if (this.m_VirtualMouse == null)
			{
				return;
			}
			InputAction action = context.action;
			MouseButton? mouseButton = null;
			if (action == this.m_LeftButtonAction.action)
			{
				mouseButton = new MouseButton?(MouseButton.Left);
			}
			else if (action == this.m_RightButtonAction.action)
			{
				mouseButton = new MouseButton?(MouseButton.Right);
			}
			else if (action == this.m_MiddleButtonAction.action)
			{
				mouseButton = new MouseButton?(MouseButton.Middle);
			}
			else if (action == this.m_ForwardButtonAction.action)
			{
				mouseButton = new MouseButton?(MouseButton.Forward);
			}
			else if (action == this.m_BackButtonAction.action)
			{
				mouseButton = new MouseButton?(MouseButton.Back);
			}
			if (mouseButton != null)
			{
				bool flag = context.control.IsPressed(0f);
				MouseState mouseState;
				this.m_VirtualMouse.CopyState<MouseState>(out mouseState);
				mouseState.WithButton(mouseButton.Value, flag);
				InputState.Change<MouseState>(this.m_VirtualMouse, mouseState, InputUpdateType.None, default(InputEventPtr));
			}
		}

		// Token: 0x06000BD6 RID: 3030 RVA: 0x0003F278 File Offset: 0x0003D478
		private static void SetActionCallback(InputActionProperty field, Action<InputAction.CallbackContext> callback, bool install = true)
		{
			InputAction action = field.action;
			if (action == null)
			{
				return;
			}
			if (install)
			{
				action.started += callback;
				action.canceled += callback;
				return;
			}
			action.started -= callback;
			action.canceled -= callback;
		}

		// Token: 0x06000BD7 RID: 3031 RVA: 0x0003F2B4 File Offset: 0x0003D4B4
		private static void SetAction(ref InputActionProperty field, InputActionProperty value)
		{
			InputActionProperty inputActionProperty = field;
			field = value;
			if (inputActionProperty.reference == null)
			{
				InputAction action = inputActionProperty.action;
				if (action != null && action.enabled)
				{
					action.Disable();
					if (value.reference == null)
					{
						InputAction action2 = value.action;
						if (action2 == null)
						{
							return;
						}
						action2.Enable();
					}
				}
			}
		}

		// Token: 0x06000BD8 RID: 3032 RVA: 0x0003F317 File Offset: 0x0003D517
		private void OnAfterInputUpdate()
		{
			this.UpdateMotion();
		}

		// Token: 0x0400043F RID: 1087
		[Header("Cursor")]
		[Tooltip("Whether the component should set the cursor position of the hardware mouse cursor, if one is available. If so, the software cursor pointed (to by 'Cursor Graphic') will be hidden.")]
		[SerializeField]
		private VirtualMouseInput.CursorMode m_CursorMode;

		// Token: 0x04000440 RID: 1088
		[Tooltip("The graphic that represents the software cursor. This is hidden if a hardware cursor (see 'Cursor Mode') is used.")]
		[SerializeField]
		private Graphic m_CursorGraphic;

		// Token: 0x04000441 RID: 1089
		[Tooltip("The transform for the software cursor. Will only be set if a software cursor is used (see 'Cursor Mode'). Moving the cursor updates the anchored position of the transform.")]
		[SerializeField]
		private RectTransform m_CursorTransform;

		// Token: 0x04000442 RID: 1090
		[Header("Motion")]
		[Tooltip("Speed in pixels per second with which to move the cursor. Scaled by the input from 'Stick Action'.")]
		[SerializeField]
		private float m_CursorSpeed = 400f;

		// Token: 0x04000443 RID: 1091
		[Tooltip("Scale factor to apply to 'Scroll Wheel Action' when setting the mouse 'scrollWheel' control.")]
		[SerializeField]
		private float m_ScrollSpeed = 45f;

		// Token: 0x04000444 RID: 1092
		[Space(10f)]
		[Tooltip("Vector2 action that moves the cursor left/right (X) and up/down (Y) on screen.")]
		[SerializeField]
		private InputActionProperty m_StickAction;

		// Token: 0x04000445 RID: 1093
		[Tooltip("Button action that triggers a left-click on the mouse.")]
		[SerializeField]
		private InputActionProperty m_LeftButtonAction;

		// Token: 0x04000446 RID: 1094
		[Tooltip("Button action that triggers a middle-click on the mouse.")]
		[SerializeField]
		private InputActionProperty m_MiddleButtonAction;

		// Token: 0x04000447 RID: 1095
		[Tooltip("Button action that triggers a right-click on the mouse.")]
		[SerializeField]
		private InputActionProperty m_RightButtonAction;

		// Token: 0x04000448 RID: 1096
		[Tooltip("Button action that triggers a forward button (button #4) click on the mouse.")]
		[SerializeField]
		private InputActionProperty m_ForwardButtonAction;

		// Token: 0x04000449 RID: 1097
		[Tooltip("Button action that triggers a back button (button #5) click on the mouse.")]
		[SerializeField]
		private InputActionProperty m_BackButtonAction;

		// Token: 0x0400044A RID: 1098
		[Tooltip("Vector2 action that feeds into the mouse 'scrollWheel' action (scaled by 'Scroll Speed').")]
		[SerializeField]
		private InputActionProperty m_ScrollWheelAction;

		// Token: 0x0400044B RID: 1099
		private Canvas m_Canvas;

		// Token: 0x0400044C RID: 1100
		private Mouse m_VirtualMouse;

		// Token: 0x0400044D RID: 1101
		private Mouse m_SystemMouse;

		// Token: 0x0400044E RID: 1102
		private Action m_AfterInputUpdateDelegate;

		// Token: 0x0400044F RID: 1103
		private Action<InputAction.CallbackContext> m_ButtonActionTriggeredDelegate;

		// Token: 0x04000450 RID: 1104
		private double m_LastTime;

		// Token: 0x04000451 RID: 1105
		private Vector2 m_LastStickValue;

		// Token: 0x020001DB RID: 475
		public enum CursorMode
		{
			// Token: 0x040009D3 RID: 2515
			SoftwareCursor,
			// Token: 0x040009D4 RID: 2516
			HardwareCursorIfAvailable
		}
	}
}
