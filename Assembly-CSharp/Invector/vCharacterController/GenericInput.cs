using System;
using System.Collections;
using ProjectExhibitionist.Core;
using UnityEngine;

namespace Invector.vCharacterController
{
	// Token: 0x02000694 RID: 1684
	[Serializable]
	public class GenericInput
	{
		// Token: 0x17000714 RID: 1812
		// (get) Token: 0x0600279E RID: 10142 RVA: 0x000E2004 File Offset: 0x000E0204
		protected InputDevice inputDevice
		{
			get
			{
				if (vInput.instance.inputDevice == InputDevice.Mobile)
				{
					return InputDevice.Mobile;
				}
				if (!ControllerInput.IsActive)
				{
					return InputDevice.MouseKeyboard;
				}
				return InputDevice.Joystick;
			}
		}

		// Token: 0x17000715 RID: 1813
		// (get) Token: 0x0600279F RID: 10143 RVA: 0x000E2020 File Offset: 0x000E0220
		public bool isAxis
		{
			get
			{
				bool flag = false;
				switch (this.inputDevice)
				{
				case InputDevice.MouseKeyboard:
					flag = this.keyboardAxis;
					break;
				case InputDevice.Joystick:
					flag = this.joystickAxis;
					break;
				case InputDevice.Mobile:
					flag = this.mobileAxis;
					break;
				}
				return flag;
			}
		}

		// Token: 0x17000716 RID: 1814
		// (get) Token: 0x060027A0 RID: 10144 RVA: 0x000E2064 File Offset: 0x000E0264
		public bool isAxisInvert
		{
			get
			{
				bool flag = false;
				switch (this.inputDevice)
				{
				case InputDevice.MouseKeyboard:
					flag = this.keyboardAxisInvert;
					break;
				case InputDevice.Joystick:
					flag = this.joystickAxisInvert;
					break;
				case InputDevice.Mobile:
					flag = this.mobileAxisInvert;
					break;
				}
				return flag;
			}
		}

		// Token: 0x060027A1 RID: 10145 RVA: 0x000E20A8 File Offset: 0x000E02A8
		public GenericInput(string keyboard, string joystick, string mobile)
		{
			this.keyboard = keyboard;
			this.joystick = joystick;
			this.mobile = mobile;
			GenericInput.GenerateKeyCodeHash();
		}

		// Token: 0x060027A2 RID: 10146 RVA: 0x000E20E4 File Offset: 0x000E02E4
		public GenericInput(string keyboard, bool keyboardAxis, string joystick, bool joystickAxis, string mobile, bool mobileAxis)
		{
			this.keyboard = keyboard;
			this.keyboardAxis = keyboardAxis;
			this.joystick = joystick;
			this.joystickAxis = joystickAxis;
			this.mobile = mobile;
			this.mobileAxis = mobileAxis;
			GenericInput.GenerateKeyCodeHash();
		}

		// Token: 0x060027A3 RID: 10147 RVA: 0x000E2144 File Offset: 0x000E0344
		public GenericInput(string keyboard, bool keyboardAxis, bool keyboardInvert, string joystick, bool joystickAxis, bool joystickInvert, string mobile, bool mobileAxis, bool mobileInvert)
		{
			this.keyboard = keyboard;
			this.keyboardAxis = keyboardAxis;
			this.keyboardAxisInvert = keyboardInvert;
			this.joystick = joystick;
			this.joystickAxis = joystickAxis;
			this.joystickAxisInvert = joystickInvert;
			this.mobile = mobile;
			this.mobileAxis = mobileAxis;
			this.mobileAxisInvert = mobileInvert;
			GenericInput.GenerateKeyCodeHash();
		}

		// Token: 0x17000717 RID: 1815
		// (get) Token: 0x060027A4 RID: 10148 RVA: 0x000E21BC File Offset: 0x000E03BC
		public string buttonName
		{
			get
			{
				KeyCode keyCode;
				if (ControllerInput.IsActive && !this.preserveJoystickBinding && ControllerGameplayBridge.TryKey(this.keyboard, out keyCode) && ControllerBindings.HasKey(keyCode))
				{
					return ControllerInput.ForKey(keyCode);
				}
				if (!(vInput.instance != null))
				{
					return string.Empty;
				}
				if (this.inputDevice == InputDevice.MouseKeyboard)
				{
					return this.keyboard ?? string.Empty;
				}
				if (this.inputDevice == InputDevice.Joystick)
				{
					return this.joystick;
				}
				return this.mobile;
			}
		}

		// Token: 0x17000718 RID: 1816
		// (get) Token: 0x060027A5 RID: 10149 RVA: 0x000E2238 File Offset: 0x000E0438
		public bool isKey
		{
			get
			{
				if (!(vInput.instance != null) || this.isUnityInput)
				{
					return false;
				}
				if (string.IsNullOrEmpty(this.lastButtonName) || this.lastButtonName != this.buttonName)
				{
					this.m_isKey = Enum.IsDefined(typeof(KeyCode), this.buttonName);
					this.lastButtonName = this.buttonName;
				}
				if (this.m_isKey)
				{
					return true;
				}
				this.isUnityInput = true;
				return false;
			}
		}

		// Token: 0x17000719 RID: 1817
		// (get) Token: 0x060027A6 RID: 10150 RVA: 0x000E22B5 File Offset: 0x000E04B5
		public KeyCode key
		{
			get
			{
				return (KeyCode)GenericInput.keyCodeHash[this.buttonName];
			}
		}

		// Token: 0x060027A7 RID: 10151 RVA: 0x000E22CC File Offset: 0x000E04CC
		private static void GenerateKeyCodeHash()
		{
			if (GenericInput.keyCodeHash.Count > 0)
			{
				return;
			}
			foreach (string text in Enum.GetNames(typeof(KeyCode)))
			{
				GenericInput.keyCodeHash.Add(text, (KeyCode)Enum.Parse(typeof(KeyCode), text));
			}
		}

		// Token: 0x060027A8 RID: 10152 RVA: 0x000E2330 File Offset: 0x000E0530
		public bool GetButton()
		{
			bool flag;
			if (this.TryControllerButton(0, out flag))
			{
				return flag;
			}
			if (string.IsNullOrEmpty(this.buttonName) || !this.IsButtonAvailable(this.buttonName))
			{
				return false;
			}
			if (this.isAxis)
			{
				return this.GetAxisButton(0.5f);
			}
			if (this.inputDevice == InputDevice.Mobile)
			{
				return true;
			}
			if (this.inputDevice == InputDevice.MouseKeyboard)
			{
				if (this.isKey)
				{
					if (Input.GetKey(this.key))
					{
						return true;
					}
				}
				else if (Input.GetButton(this.buttonName))
				{
					return true;
				}
			}
			else if (this.inputDevice == InputDevice.Joystick && Input.GetButton(this.buttonName))
			{
				return true;
			}
			return false;
		}

		// Token: 0x060027A9 RID: 10153 RVA: 0x000E23D0 File Offset: 0x000E05D0
		public bool GetButtonDown()
		{
			bool flag;
			if (this.TryControllerButton(1, out flag))
			{
				return flag;
			}
			if (string.IsNullOrEmpty(this.buttonName) || !this.IsButtonAvailable(this.buttonName))
			{
				return false;
			}
			if (this.isAxis)
			{
				return this.GetAxisButtonDown(0.5f);
			}
			if (this.inputDevice == InputDevice.Mobile)
			{
				return true;
			}
			if (this.inputDevice == InputDevice.MouseKeyboard)
			{
				if (this.isKey)
				{
					if (Input.GetKeyDown(this.key))
					{
						return true;
					}
				}
				else if (Input.GetButtonDown(this.buttonName))
				{
					return true;
				}
			}
			else if (this.inputDevice == InputDevice.Joystick && Input.GetButtonDown(this.buttonName))
			{
				return true;
			}
			return false;
		}

		// Token: 0x060027AA RID: 10154 RVA: 0x000E2470 File Offset: 0x000E0670
		public bool GetButtonUp()
		{
			bool flag;
			if (this.TryControllerButton(2, out flag))
			{
				return flag;
			}
			if (string.IsNullOrEmpty(this.buttonName) || !this.IsButtonAvailable(this.buttonName))
			{
				return false;
			}
			if (this.isAxis)
			{
				return this.GetAxisButtonUp();
			}
			if (this.inputDevice == InputDevice.Mobile)
			{
				return true;
			}
			if (this.inputDevice == InputDevice.MouseKeyboard)
			{
				if (this.isKey)
				{
					if (Input.GetKeyUp(this.key))
					{
						return true;
					}
				}
				else if (Input.GetButtonUp(this.buttonName))
				{
					return true;
				}
			}
			else if (this.inputDevice == InputDevice.Joystick && Input.GetButtonUp(this.buttonName))
			{
				return true;
			}
			return false;
		}

		// Token: 0x060027AB RID: 10155 RVA: 0x000E2508 File Offset: 0x000E0708
		public float GetAxis()
		{
			float num;
			if (this.TryControllerAxis(out num))
			{
				return num;
			}
			if (string.IsNullOrEmpty(this.buttonName) || !this.IsButtonAvailable(this.buttonName) || this.isKey)
			{
				return 0f;
			}
			if (this.inputDevice != InputDevice.Mobile)
			{
				if (this.inputDevice == InputDevice.MouseKeyboard)
				{
					return Input.GetAxis(this.buttonName);
				}
				if (this.inputDevice == InputDevice.Joystick)
				{
					return Input.GetAxis(this.buttonName);
				}
			}
			return 0f;
		}

		// Token: 0x060027AC RID: 10156 RVA: 0x000E2584 File Offset: 0x000E0784
		public float GetAxisRaw()
		{
			float num;
			if (this.TryControllerAxis(out num))
			{
				return num;
			}
			if (string.IsNullOrEmpty(this.buttonName) || !this.IsButtonAvailable(this.buttonName) || this.isKey)
			{
				return 0f;
			}
			if (this.inputDevice != InputDevice.Mobile)
			{
				if (this.inputDevice == InputDevice.MouseKeyboard)
				{
					return Input.GetAxisRaw(this.buttonName);
				}
				if (this.inputDevice == InputDevice.Joystick)
				{
					return Input.GetAxisRaw(this.buttonName);
				}
			}
			return 0f;
		}

		// Token: 0x060027AD RID: 10157 RVA: 0x000E2600 File Offset: 0x000E0800
		public bool GetDoubleButtonDown(float inputTime = 1f)
		{
			if (string.IsNullOrEmpty(this.buttonName) || !this.IsButtonAvailable(this.buttonName))
			{
				return false;
			}
			if (this.multTapCounter == 0 && this.GetButtonDown())
			{
				this.multTapTimer = Time.time;
				this.multTapCounter = 1;
				return false;
			}
			if (this.multTapCounter == 1 && this.GetButtonDown())
			{
				float num = this.multTapTimer + inputTime;
				bool flag = Time.time < num;
				this.multTapTimer = 0f;
				this.multTapCounter = 0;
				return flag;
			}
			if (this.multTapCounter == 1 && this.multTapTimer + inputTime < Time.time)
			{
				this.multTapTimer = 0f;
				this.multTapCounter = 0;
			}
			return false;
		}

		// Token: 0x060027AE RID: 10158 RVA: 0x000E26B0 File Offset: 0x000E08B0
		public bool GetButtonTimer(float inputTime = 2f)
		{
			if (string.IsNullOrEmpty(this.buttonName) || !this.IsButtonAvailable(this.buttonName))
			{
				return false;
			}
			if (this.GetButtonDown() && !this.inButtomTimer)
			{
				this.lastTimeTheButtonWasPressed = Time.time + 0.1f;
				this.timeButtonWasPressed = Time.time;
				this.inButtomTimer = true;
			}
			if (!this.inButtomTimer)
			{
				return false;
			}
			bool flag = this.timeButtonWasPressed + inputTime - Time.time <= 0f;
			if (!this.GetButton() || this.lastTimeTheButtonWasPressed < Time.time)
			{
				this.inButtomTimer = false;
				return false;
			}
			this.lastTimeTheButtonWasPressed = Time.time + 0.1f;
			if (flag)
			{
				this.inButtomTimer = false;
			}
			return flag;
		}

		// Token: 0x060027AF RID: 10159 RVA: 0x000E276C File Offset: 0x000E096C
		public bool GetButtonTimer(ref float currentTimer, float inputTime = 2f)
		{
			if (string.IsNullOrEmpty(this.buttonName) || !this.IsButtonAvailable(this.buttonName))
			{
				return false;
			}
			if (this.GetButtonDown() && !this.inButtomTimer)
			{
				this.lastTimeTheButtonWasPressed = Time.time + 0.1f;
				this.timeButtonWasPressed = Time.time;
				this.inButtomTimer = true;
			}
			if (!this.inButtomTimer)
			{
				return false;
			}
			float num = this.timeButtonWasPressed + inputTime;
			currentTimer = num - Time.time;
			bool flag = num - Time.time <= 0f;
			if (!this.GetButton() || this.lastTimeTheButtonWasPressed < Time.time)
			{
				this.inButtomTimer = false;
				return false;
			}
			this.lastTimeTheButtonWasPressed = Time.time + 0.1f;
			if (flag)
			{
				this.inButtomTimer = false;
			}
			return flag;
		}

		// Token: 0x060027B0 RID: 10160 RVA: 0x000E2834 File Offset: 0x000E0A34
		public bool GetButtonTimer(ref float currentTimer, ref bool upAfterPressed, float inputTime = 2f)
		{
			if (string.IsNullOrEmpty(this.buttonName) || !this.IsButtonAvailable(this.buttonName))
			{
				return false;
			}
			if (this.GetButtonDown())
			{
				this.lastTimeTheButtonWasPressed = Time.time + 0.1f;
				this.timeButtonWasPressed = Time.time;
				this.inButtomTimer = true;
			}
			if (!this.inButtomTimer)
			{
				return false;
			}
			float num = this.timeButtonWasPressed + inputTime;
			currentTimer = (inputTime - (num - Time.time)) / inputTime;
			bool flag = num - Time.time <= 0f;
			if (!this.GetButton() || this.lastTimeTheButtonWasPressed < Time.time)
			{
				this.inButtomTimer = false;
				upAfterPressed = true;
				return false;
			}
			upAfterPressed = false;
			this.lastTimeTheButtonWasPressed = Time.time + 0.1f;
			if (flag)
			{
				this.inButtomTimer = false;
			}
			return flag;
		}

		// Token: 0x060027B1 RID: 10161 RVA: 0x000E28FC File Offset: 0x000E0AFC
		public bool GetAxisButton(float value = 0.5f)
		{
			if (string.IsNullOrEmpty(this.buttonName) || !this.IsButtonAvailable(this.buttonName))
			{
				return false;
			}
			if (this.isAxisInvert)
			{
				value *= -1f;
			}
			if (value > 0f)
			{
				return this.GetAxisRaw() >= value;
			}
			return value < 0f && this.GetAxisRaw() <= value;
		}

		// Token: 0x060027B2 RID: 10162 RVA: 0x000E2964 File Offset: 0x000E0B64
		public bool GetAxisButtonDown(float value = 0.5f)
		{
			if (string.IsNullOrEmpty(this.buttonName) || !this.IsButtonAvailable(this.buttonName))
			{
				return false;
			}
			if (this.isAxisInvert)
			{
				value *= -1f;
			}
			if (value > 0f)
			{
				if (!this.isAxisInUse && this.GetAxisRaw() >= value)
				{
					this.isAxisInUse = true;
					return true;
				}
				if (this.isAxisInUse && this.GetAxisRaw() == 0f)
				{
					this.isAxisInUse = false;
				}
			}
			else if (value < 0f)
			{
				if (!this.isAxisInUse && this.GetAxisRaw() <= value)
				{
					this.isAxisInUse = true;
					return true;
				}
				if (this.isAxisInUse && this.GetAxisRaw() == 0f)
				{
					this.isAxisInUse = false;
				}
			}
			return false;
		}

		// Token: 0x060027B3 RID: 10163 RVA: 0x000E2A20 File Offset: 0x000E0C20
		public bool GetAxisButtonUp()
		{
			if (string.IsNullOrEmpty(this.buttonName) || !this.IsButtonAvailable(this.buttonName))
			{
				return false;
			}
			if (this.isAxisInUse && this.GetAxisRaw() == 0f)
			{
				this.isAxisInUse = false;
				return true;
			}
			if (!this.isAxisInUse && this.GetAxisRaw() != 0f)
			{
				this.isAxisInUse = true;
			}
			return false;
		}

		// Token: 0x060027B4 RID: 10164 RVA: 0x000E2A88 File Offset: 0x000E0C88
		private bool TryControllerButton(int phase, out bool value)
		{
			value = false;
			if (!ControllerInput.IsActive)
			{
				return false;
			}
			if (!this.useInput || TextInputFocus.BlocksGameplayInput)
			{
				return true;
			}
			KeyCode keyCode;
			if (!this.preserveJoystickBinding && ControllerGameplayBridge.TryKey(this.keyboard, out keyCode) && ControllerBindings.HasKey(keyCode))
			{
				value = ControllerGameplayBridge.Read(keyCode, phase);
				return true;
			}
			if (!ControllerGameplayBridge.AllowsAxes)
			{
				return true;
			}
			ControllerButton controllerButton;
			if (ControllerBindings.TryLegacyButton(this.joystick, out controllerButton))
			{
				value = ((phase == 0) ? ControllerInput.GetButton(controllerButton) : ((phase == 1) ? ControllerInput.GetButtonDown(controllerButton) : ControllerInput.GetButtonUp(controllerButton)));
				if (value && phase == 1 && controllerButton == ControllerButton.East)
				{
					if (EscapeInputCoordinator.ConsumedThisFrame && this._controllerCancelFrame != Time.frameCount)
					{
						value = false;
					}
					else
					{
						this._controllerCancelFrame = Time.frameCount;
						EscapeInputCoordinator.Consume(null);
					}
				}
			}
			return true;
		}

		// Token: 0x060027B5 RID: 10165 RVA: 0x000E2B4C File Offset: 0x000E0D4C
		private bool TryControllerAxis(out float value)
		{
			value = 0f;
			if (!ControllerInput.IsActive)
			{
				return false;
			}
			if (!this.useInput || TextInputFocus.BlocksGameplayInput || !ControllerGameplayBridge.AllowsAxes)
			{
				return true;
			}
			value = ControllerInput.GetAxis((!this.preserveJoystickBinding && !string.IsNullOrEmpty(this.keyboard)) ? this.keyboard : this.joystick);
			return true;
		}

		// Token: 0x060027B6 RID: 10166 RVA: 0x000E2BAC File Offset: 0x000E0DAC
		private bool IsButtonAvailable(string btnName)
		{
			if (ControllerInput.IsActive)
			{
				return this.useInput && !TextInputFocus.BlocksGameplayInput;
			}
			if (!this.useInput)
			{
				return false;
			}
			if (TextInputFocus.BlocksGameplayInput)
			{
				this.inButtomTimer = false;
				this.multTapCounter = 0;
				this.isAxisInUse = false;
				return false;
			}
			bool flag;
			try
			{
				if (this.isKey)
				{
					flag = true;
				}
				else
				{
					Input.GetButton(this.buttonName);
					flag = true;
				}
			}
			catch (Exception ex)
			{
				if (this.isUnityInput)
				{
					this.isUnityInput = false;
					flag = this.isKey;
				}
				else
				{
					Debug.LogWarning(" Failure to try access Unity Input or KeyCode : " + this.buttonName + "  \n  " + ex.Message);
					flag = false;
				}
			}
			return flag;
		}

		// Token: 0x0400282D RID: 10285
		public bool useInput = true;

		// Token: 0x0400282E RID: 10286
		[NonSerialized]
		public bool preserveJoystickBinding;

		// Token: 0x0400282F RID: 10287
		private int _controllerCancelFrame = -1;

		// Token: 0x04002830 RID: 10288
		[SerializeField]
		private bool isAxisInUse;

		// Token: 0x04002831 RID: 10289
		[SerializeField]
		private bool isUnityInput;

		// Token: 0x04002832 RID: 10290
		[SerializeField]
		public string keyboard;

		// Token: 0x04002833 RID: 10291
		[SerializeField]
		public bool keyboardAxis;

		// Token: 0x04002834 RID: 10292
		[SerializeField]
		public string joystick;

		// Token: 0x04002835 RID: 10293
		[SerializeField]
		public bool joystickAxis;

		// Token: 0x04002836 RID: 10294
		[SerializeField]
		public string mobile;

		// Token: 0x04002837 RID: 10295
		[SerializeField]
		public bool mobileAxis;

		// Token: 0x04002838 RID: 10296
		[SerializeField]
		public bool joystickAxisInvert;

		// Token: 0x04002839 RID: 10297
		[SerializeField]
		public bool keyboardAxisInvert;

		// Token: 0x0400283A RID: 10298
		[SerializeField]
		public bool mobileAxisInvert;

		// Token: 0x0400283B RID: 10299
		public float timeButtonWasPressed;

		// Token: 0x0400283C RID: 10300
		public float lastTimeTheButtonWasPressed;

		// Token: 0x0400283D RID: 10301
		public bool inButtomTimer;

		// Token: 0x0400283E RID: 10302
		private float multTapTimer;

		// Token: 0x0400283F RID: 10303
		private int multTapCounter;

		// Token: 0x04002840 RID: 10304
		private string lastButtonName = "";

		// Token: 0x04002841 RID: 10305
		private bool m_isKey;

		// Token: 0x04002842 RID: 10306
		private static Hashtable keyCodeHash = new Hashtable();
	}
}
