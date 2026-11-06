using System;
using ProjectExhibitionist.Core;
using UnityEngine;

namespace Invector.vCharacterController
{
	// Token: 0x02000691 RID: 1681
	public class vInput : MonoBehaviour
	{
		// Token: 0x14000041 RID: 65
		// (add) Token: 0x0600278E RID: 10126 RVA: 0x000E1B10 File Offset: 0x000DFD10
		// (remove) Token: 0x0600278F RID: 10127 RVA: 0x000E1B48 File Offset: 0x000DFD48
		public event vInput.OnChangeInputType onChangeInputType;

		// Token: 0x17000712 RID: 1810
		// (get) Token: 0x06002790 RID: 10128 RVA: 0x000E1B80 File Offset: 0x000DFD80
		public static vInput instance
		{
			get
			{
				if (vInput._instance == null)
				{
					vInput._instance = Object.FindObjectOfType<vInput>();
					if (vInput._instance == null)
					{
						vInput._instance = new GameObject("vInputType").AddComponent<vInput>();
						return vInput._instance;
					}
				}
				return vInput._instance;
			}
		}

		// Token: 0x06002791 RID: 10129 RVA: 0x000E1BD0 File Offset: 0x000DFDD0
		private void Start()
		{
			if (this.hud == null)
			{
				this.hud = vHUDController.instance;
			}
		}

		// Token: 0x17000713 RID: 1811
		// (get) Token: 0x06002792 RID: 10130 RVA: 0x000E1BEB File Offset: 0x000DFDEB
		// (set) Token: 0x06002793 RID: 10131 RVA: 0x000E1BF3 File Offset: 0x000DFDF3
		[HideInInspector]
		public InputDevice inputDevice
		{
			get
			{
				return this._inputType;
			}
			set
			{
				this._inputType = value;
				this.OnChangeInput();
			}
		}

		// Token: 0x06002794 RID: 10132 RVA: 0x000E1C04 File Offset: 0x000DFE04
		private void OnGUI()
		{
			if (ControllerInput.HasGamepad)
			{
				InputDevice inputDevice = (ControllerInput.IsActive ? InputDevice.Joystick : InputDevice.MouseKeyboard);
				if (this.inputDevice != inputDevice)
				{
					this.inputDevice = inputDevice;
				}
				return;
			}
			switch (this.inputDevice)
			{
			case InputDevice.MouseKeyboard:
				if (this.isJoystickInput())
				{
					this.inputDevice = InputDevice.Joystick;
					if (this.hud != null)
					{
						this.hud.controllerInput = true;
						this.hud.ShowText("Control scheme changed to Controller", 2f, 0.5f);
						return;
					}
				}
				else if (this.isMobileInput())
				{
					this.inputDevice = InputDevice.Mobile;
					if (this.hud != null)
					{
						this.hud.controllerInput = true;
						this.hud.ShowText("Control scheme changed to Mobile", 2f, 0.5f);
						return;
					}
				}
				break;
			case InputDevice.Joystick:
				if (this.isMouseKeyboard())
				{
					this.inputDevice = InputDevice.MouseKeyboard;
					if (this.hud != null)
					{
						this.hud.controllerInput = false;
						this.hud.ShowText("Control scheme changed to Keyboard/Mouse", 2f, 0.5f);
						return;
					}
				}
				else if (this.isMobileInput())
				{
					this.inputDevice = InputDevice.Mobile;
					if (this.hud != null)
					{
						this.hud.controllerInput = true;
						this.hud.ShowText("Control scheme changed to Mobile", 2f, 0.5f);
						return;
					}
				}
				break;
			case InputDevice.Mobile:
				if (this.isMouseKeyboard())
				{
					this.inputDevice = InputDevice.MouseKeyboard;
					if (this.hud != null)
					{
						this.hud.controllerInput = false;
						this.hud.ShowText("Control scheme changed to Keyboard/Mouse", 2f, 0.5f);
						return;
					}
				}
				else if (this.isJoystickInput())
				{
					this.inputDevice = InputDevice.Joystick;
					if (this.hud != null)
					{
						this.hud.controllerInput = true;
						this.hud.ShowText("Control scheme changed to Controller", 2f, 0.5f);
					}
				}
				break;
			default:
				return;
			}
		}

		// Token: 0x06002795 RID: 10133 RVA: 0x0000D91D File Offset: 0x0000BB1D
		private bool isMobileInput()
		{
			return false;
		}

		// Token: 0x06002796 RID: 10134 RVA: 0x000E1DF8 File Offset: 0x000DFFF8
		private bool isMouseKeyboard()
		{
			return Event.current.isKey || Event.current.isMouse || (Input.GetAxis("Mouse X") != 0f || Input.GetAxis("Mouse Y") != 0f);
		}

		// Token: 0x06002797 RID: 10135 RVA: 0x000E1E44 File Offset: 0x000E0044
		private bool isJoystickInput()
		{
			return Input.GetKey(KeyCode.Joystick1Button0) || Input.GetKey(KeyCode.Joystick1Button1) || Input.GetKey(KeyCode.Joystick1Button2) || Input.GetKey(KeyCode.Joystick1Button3) || Input.GetKey(KeyCode.Joystick1Button4) || Input.GetKey(KeyCode.Joystick1Button5) || Input.GetKey(KeyCode.Joystick1Button6) || Input.GetKey(KeyCode.Joystick1Button7) || Input.GetKey(KeyCode.Joystick1Button8) || Input.GetKey(KeyCode.Joystick1Button9) || Input.GetKey(KeyCode.Joystick1Button10) || Input.GetKey(KeyCode.Joystick1Button11) || Input.GetKey(KeyCode.Joystick1Button12) || Input.GetKey(KeyCode.Joystick1Button13) || Input.GetKey(KeyCode.Joystick1Button14) || Input.GetKey(KeyCode.Joystick1Button15) || Input.GetKey(KeyCode.Joystick1Button16) || Input.GetKey(KeyCode.Joystick1Button17) || Input.GetKey(KeyCode.Joystick1Button18) || Input.GetKey(KeyCode.Joystick1Button19) || (Input.GetAxis("LeftAnalogHorizontal") != 0f || Input.GetAxis("LeftAnalogVertical") != 0f || Input.GetAxis("RightAnalogHorizontal") != 0f || Input.GetAxis("RightAnalogVertical") != 0f || Input.GetAxis("LT") != 0f || Input.GetAxis("RT") != 0f || Input.GetAxis("D-Pad Horizontal") != 0f || Input.GetAxis("D-Pad Vertical") != 0f);
		}

		// Token: 0x06002798 RID: 10136 RVA: 0x000E1FE9 File Offset: 0x000E01E9
		private void OnChangeInput()
		{
			if (this.onChangeInputType != null)
			{
				this.onChangeInputType(this.inputDevice);
			}
		}

		// Token: 0x04002826 RID: 10278
		private static vInput _instance;

		// Token: 0x04002827 RID: 10279
		public vHUDController hud;

		// Token: 0x04002828 RID: 10280
		private InputDevice _inputType;

		// Token: 0x02000692 RID: 1682
		// (Invoke) Token: 0x0600279B RID: 10139
		public delegate void OnChangeInputType(InputDevice type);
	}
}
