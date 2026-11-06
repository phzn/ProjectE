using System;
using Features.PhoneCompanion;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.InputSystem.LowLevel;

namespace ProjectExhibitionist.Core
{
	// Token: 0x02000A4E RID: 2638
	public static class ControllerInput
	{
		// Token: 0x17000C9F RID: 3231
		// (get) Token: 0x06004AFB RID: 19195 RVA: 0x001D33BC File Offset: 0x001D15BC
		public static bool HasGamepad
		{
			get
			{
				return Gamepad.current != null && Gamepad.current.added;
			}
		}

		// Token: 0x17000CA0 RID: 3232
		// (get) Token: 0x06004AFC RID: 19196 RVA: 0x001D33D1 File Offset: 0x001D15D1
		public static bool IsControllerActive
		{
			get
			{
				ControllerInput.Sample();
				return ControllerInput._active && ControllerInput.HasGamepad;
			}
		}

		// Token: 0x17000CA1 RID: 3233
		// (get) Token: 0x06004AFD RID: 19197 RVA: 0x001D33E6 File Offset: 0x001D15E6
		public static bool IsActive
		{
			get
			{
				return ControllerInput.IsControllerActive;
			}
		}

		// Token: 0x17000CA2 RID: 3234
		// (get) Token: 0x06004AFE RID: 19198 RVA: 0x001D33ED File Offset: 0x001D15ED
		public static bool UiInputConsumedThisFrame
		{
			get
			{
				return ControllerInput._consumedFrame == Time.frameCount;
			}
		}

		// Token: 0x06004AFF RID: 19199 RVA: 0x001D33FB File Offset: 0x001D15FB
		public static void ConsumeUIInput()
		{
			ControllerInput._consumedFrame = Time.frameCount;
		}

		// Token: 0x17000CA3 RID: 3235
		// (get) Token: 0x06004B00 RID: 19200 RVA: 0x001D3407 File Offset: 0x001D1607
		public static Vector2 Move
		{
			get
			{
				if (!ControllerInput.HasGamepad)
				{
					return Vector2.zero;
				}
				return Gamepad.current.leftStick.ReadValue();
			}
		}

		// Token: 0x17000CA4 RID: 3236
		// (get) Token: 0x06004B01 RID: 19201 RVA: 0x001D3425 File Offset: 0x001D1625
		public static Vector2 Look
		{
			get
			{
				if (!ControllerInput.HasGamepad)
				{
					return Vector2.zero;
				}
				return Gamepad.current.rightStick.ReadValue();
			}
		}

		// Token: 0x17000CA5 RID: 3237
		// (get) Token: 0x06004B02 RID: 19202 RVA: 0x001D3443 File Offset: 0x001D1643
		public static Vector2 RadialDirection
		{
			get
			{
				return ControllerInput.Look;
			}
		}

		// Token: 0x17000CA6 RID: 3238
		// (get) Token: 0x06004B03 RID: 19203 RVA: 0x001D344C File Offset: 0x001D164C
		public static Vector2 Navigate
		{
			get
			{
				if (!ControllerInput.HasGamepad || Gamepad.current.dpad.ReadValue().sqrMagnitude <= 0.1f)
				{
					return ControllerInput.Move;
				}
				return Gamepad.current.dpad.ReadValue();
			}
		}

		// Token: 0x17000CA7 RID: 3239
		// (get) Token: 0x06004B04 RID: 19204 RVA: 0x001D3493 File Offset: 0x001D1693
		public static Vector2 Navigation
		{
			get
			{
				return ControllerInput.Navigate;
			}
		}

		// Token: 0x17000CA8 RID: 3240
		// (get) Token: 0x06004B05 RID: 19205 RVA: 0x001D349A File Offset: 0x001D169A
		public static bool Modifier
		{
			get
			{
				return ControllerInput.GetButton(ControllerButton.LeftTrigger);
			}
		}

		// Token: 0x17000CA9 RID: 3241
		// (get) Token: 0x06004B06 RID: 19206 RVA: 0x001D34A2 File Offset: 0x001D16A2
		public static float Scroll
		{
			get
			{
				if (!ControllerInput.Modifier)
				{
					return 0f;
				}
				return ControllerInput.Look.y * Time.unscaledDeltaTime * 3f;
			}
		}

		// Token: 0x17000CAA RID: 3242
		// (get) Token: 0x06004B07 RID: 19207 RVA: 0x001D34C7 File Offset: 0x001D16C7
		public static float CameraHeight
		{
			get
			{
				if (!ControllerInput.Modifier || !ControllerInput.HasGamepad)
				{
					return 0f;
				}
				return Gamepad.current.dpad.y.ReadValue();
			}
		}

		// Token: 0x17000CAB RID: 3243
		// (get) Token: 0x06004B08 RID: 19208 RVA: 0x001D34F1 File Offset: 0x001D16F1
		public static bool SubmitPressed
		{
			get
			{
				return ControllerInput.GetButtonDown(ControllerButton.South);
			}
		}

		// Token: 0x17000CAC RID: 3244
		// (get) Token: 0x06004B09 RID: 19209 RVA: 0x001D34F9 File Offset: 0x001D16F9
		public static bool SubmitDown
		{
			get
			{
				return ControllerInput.SubmitPressed;
			}
		}

		// Token: 0x17000CAD RID: 3245
		// (get) Token: 0x06004B0A RID: 19210 RVA: 0x001D3500 File Offset: 0x001D1700
		public static bool CancelPressed
		{
			get
			{
				return ControllerInput.GetButtonDown(ControllerButton.East);
			}
		}

		// Token: 0x17000CAE RID: 3246
		// (get) Token: 0x06004B0B RID: 19211 RVA: 0x001D3508 File Offset: 0x001D1708
		public static bool CancelDown
		{
			get
			{
				return ControllerInput.CancelPressed;
			}
		}

		// Token: 0x17000CAF RID: 3247
		// (get) Token: 0x06004B0C RID: 19212 RVA: 0x001D350F File Offset: 0x001D170F
		public static bool DropDown
		{
			get
			{
				return ControllerInput.GetButtonDown(ControllerButton.North);
			}
		}

		// Token: 0x17000CB0 RID: 3248
		// (get) Token: 0x06004B0D RID: 19213 RVA: 0x001D3517 File Offset: 0x001D1717
		public static bool PreviousItemDown
		{
			get
			{
				return !ControllerInput.Modifier && ControllerInput.GetButtonDown(ControllerButton.DpadLeft);
			}
		}

		// Token: 0x17000CB1 RID: 3249
		// (get) Token: 0x06004B0E RID: 19214 RVA: 0x001D3529 File Offset: 0x001D1729
		public static bool NextItemDown
		{
			get
			{
				return !ControllerInput.Modifier && ControllerInput.GetButtonDown(ControllerButton.DpadRight);
			}
		}

		// Token: 0x17000CB2 RID: 3250
		// (get) Token: 0x06004B0F RID: 19215 RVA: 0x001D353B File Offset: 0x001D173B
		public static bool UseItemDown
		{
			get
			{
				return !ControllerInput.Modifier && ControllerInput.GetButtonDown(ControllerButton.RightShoulder);
			}
		}

		// Token: 0x06004B10 RID: 19216 RVA: 0x001D354C File Offset: 0x001D174C
		public static ButtonControl Control(ControllerButton button)
		{
			if (!ControllerInput.HasGamepad)
			{
				return null;
			}
			Gamepad current = Gamepad.current;
			switch (button)
			{
			case ControllerButton.South:
				return current.buttonSouth;
			case ControllerButton.East:
				return current.buttonEast;
			case ControllerButton.West:
				return current.buttonWest;
			case ControllerButton.North:
				return current.buttonNorth;
			case ControllerButton.LeftShoulder:
				return current.leftShoulder;
			case ControllerButton.RightShoulder:
				return current.rightShoulder;
			case ControllerButton.LeftTrigger:
				return current.leftTrigger;
			case ControllerButton.RightTrigger:
				return current.rightTrigger;
			case ControllerButton.LeftStick:
				return current.leftStickButton;
			case ControllerButton.RightStick:
				return current.rightStickButton;
			case ControllerButton.Start:
				return current.startButton;
			case ControllerButton.Select:
				return current.selectButton;
			case ControllerButton.DpadUp:
				return current.dpad.up;
			case ControllerButton.DpadDown:
				return current.dpad.down;
			case ControllerButton.DpadLeft:
				return current.dpad.left;
			case ControllerButton.DpadRight:
				return current.dpad.right;
			default:
				return null;
			}
		}

		// Token: 0x06004B11 RID: 19217 RVA: 0x001D3638 File Offset: 0x001D1838
		public static bool GetButton(ControllerButton button)
		{
			ButtonControl buttonControl = ControllerInput.Control(button);
			return buttonControl != null && buttonControl.isPressed;
		}

		// Token: 0x06004B12 RID: 19218 RVA: 0x001D364B File Offset: 0x001D184B
		public static bool GetButtonDown(ControllerButton button)
		{
			ControllerInput.Sample();
			return (ControllerInput._downButtons & (1U << (int)button)) > 0U;
		}

		// Token: 0x06004B13 RID: 19219 RVA: 0x001D3661 File Offset: 0x001D1861
		public static bool GetButtonUp(ControllerButton button)
		{
			ControllerInput.Sample();
			return (ControllerInput._upButtons & (1U << (int)button)) > 0U;
		}

		// Token: 0x06004B14 RID: 19220 RVA: 0x001D3677 File Offset: 0x001D1877
		public static bool GetKey(KeyCode key)
		{
			ControllerInput.Sample();
			return ControllerInput.Valid(key) && ControllerInput.Held[(int)key];
		}

		// Token: 0x06004B15 RID: 19221 RVA: 0x001D368F File Offset: 0x001D188F
		public static bool GetKeyDown(KeyCode key)
		{
			ControllerInput.Sample();
			return ControllerInput.Valid(key) && ControllerInput.Down[(int)key];
		}

		// Token: 0x06004B16 RID: 19222 RVA: 0x001D36A7 File Offset: 0x001D18A7
		public static bool GetKeyUp(KeyCode key)
		{
			ControllerInput.Sample();
			return ControllerInput.Valid(key) && ControllerInput.Up[(int)key];
		}

		// Token: 0x06004B17 RID: 19223 RVA: 0x001D36BF File Offset: 0x001D18BF
		private static bool Valid(KeyCode key)
		{
			return key >= KeyCode.None && key < (KeyCode)ControllerInput.Held.Length;
		}

		// Token: 0x06004B18 RID: 19224 RVA: 0x001D36D1 File Offset: 0x001D18D1
		public static string ForKey(KeyCode key)
		{
			return ControllerBindings.Label(key);
		}

		// Token: 0x06004B19 RID: 19225 RVA: 0x001D36DC File Offset: 0x001D18DC
		public static float GetAxis(string name)
		{
			uint num = <PrivateImplementationDetails>.ComputeStringHash(name);
			if (num <= 1638631484U)
			{
				if (num <= 511224055U)
				{
					if (num != 307656861U)
					{
						if (num != 365027448U)
						{
							if (num != 511224055U)
							{
								goto IL_0360;
							}
							if (!(name == "D-Pad Vertical"))
							{
								goto IL_0360;
							}
							if (!ControllerInput.HasGamepad)
							{
								return 0f;
							}
							return Gamepad.current.dpad.y.ReadValue();
						}
						else if (!(name == "LeftAnalogHorizontal"))
						{
							goto IL_0360;
						}
					}
					else
					{
						if (!(name == "D-Pad Horizontal"))
						{
							goto IL_0360;
						}
						if (!ControllerInput.HasGamepad)
						{
							return 0f;
						}
						return Gamepad.current.dpad.x.ReadValue();
					}
				}
				else if (num <= 934753068U)
				{
					if (num != 617847442U)
					{
						if (num != 934753068U)
						{
							goto IL_0360;
						}
						if (!(name == "LeftTrigger"))
						{
							goto IL_0360;
						}
						goto IL_0326;
					}
					else
					{
						if (!(name == "LeftAnalogVertical"))
						{
							goto IL_0360;
						}
						goto IL_024C;
					}
				}
				else if (num != 1500632157U)
				{
					if (num != 1638631484U)
					{
						goto IL_0360;
					}
					if (!(name == "Mouse X"))
					{
						goto IL_0360;
					}
					if (!ControllerInput.Modifier)
					{
						return ControllerInput.Look.x * Time.unscaledDeltaTime * 110f;
					}
					return 0f;
				}
				else
				{
					if (!(name == "RightAnalogHorizontal"))
					{
						goto IL_0360;
					}
					if (!ControllerInput.Modifier)
					{
						return ControllerInput.Look.x;
					}
					return 0f;
				}
			}
			else if (num <= 1853917539U)
			{
				if (num <= 1761926707U)
				{
					if (num != 1655409103U)
					{
						if (num != 1761926707U)
						{
							goto IL_0360;
						}
						if (!(name == "RT"))
						{
							goto IL_0360;
						}
					}
					else
					{
						if (!(name == "Mouse Y"))
						{
							goto IL_0360;
						}
						if (!ControllerInput.Modifier)
						{
							return ControllerInput.Look.y * Time.unscaledDeltaTime * 110f;
						}
						return 0f;
					}
				}
				else if (num != 1811125385U)
				{
					if (num != 1853917539U)
					{
						goto IL_0360;
					}
					if (!(name == "RightTrigger"))
					{
						goto IL_0360;
					}
				}
				else
				{
					if (!(name == "Horizontal"))
					{
						goto IL_0360;
					}
					goto IL_0234;
				}
				if (!ControllerInput.HasGamepad)
				{
					return 0f;
				}
				return Gamepad.current.rightTrigger.ReadValue();
			}
			else if (num <= 2163944795U)
			{
				if (num != 2095213421U)
				{
					if (num != 2163944795U)
					{
						goto IL_0360;
					}
					if (!(name == "Vertical"))
					{
						goto IL_0360;
					}
					goto IL_024C;
				}
				else
				{
					if (!(name == "LT"))
					{
						goto IL_0360;
					}
					goto IL_0326;
				}
			}
			else if (num != 3733513911U)
			{
				if (num != 4218367836U)
				{
					goto IL_0360;
				}
				if (!(name == "Mouse ScrollWheel"))
				{
					goto IL_0360;
				}
				return ControllerInput.Scroll;
			}
			else
			{
				if (!(name == "RightAnalogVertical"))
				{
					goto IL_0360;
				}
				if (!ControllerInput.Modifier)
				{
					return ControllerInput.Look.y;
				}
				return 0f;
			}
			IL_0234:
			if (!SmartphoneController.ControllerSelfieOwnsMovement)
			{
				return ControllerInput.Move.x;
			}
			return 0f;
			IL_024C:
			if (!SmartphoneController.ControllerSelfieOwnsMovement)
			{
				return ControllerInput.Move.y;
			}
			return 0f;
			IL_0326:
			if (!ControllerInput.HasGamepad)
			{
				return 0f;
			}
			return Gamepad.current.leftTrigger.ReadValue();
			IL_0360:
			return 0f;
		}

		// Token: 0x06004B1A RID: 19226 RVA: 0x001D3A50 File Offset: 0x001D1C50
		internal static void Sample()
		{
			if (ControllerInput._sampleFrame == Time.frameCount && ControllerInput._lastPad == (ControllerInput.HasGamepad ? Gamepad.current : null))
			{
				return;
			}
			ControllerInput._sampleFrame = Time.frameCount;
			Gamepad gamepad = (ControllerInput.HasGamepad ? Gamepad.current : null);
			bool flag = gamepad != ControllerInput._lastPad;
			ControllerInput._lastPad = gamepad;
			if (flag)
			{
				ControllerInput._selectStarted = -1f;
				ControllerInput._selectHeld = false;
			}
			uint updateCount = InputState.updateCount;
			bool flag2 = flag || updateCount != ControllerInput._edgeUpdateCount;
			ControllerInput._edgeUpdateCount = updateCount;
			uint num = 0U;
			uint num2 = 0U;
			uint num3 = 0U;
			for (int i = 0; i <= 15; i++)
			{
				ButtonControl buttonControl = ControllerInput.Control((ControllerButton)i);
				if (buttonControl != null)
				{
					uint num4 = 1U << i;
					if (buttonControl.isPressed)
					{
						num |= num4;
					}
					if (flag2 && buttonControl.wasPressedThisFrame)
					{
						num2 |= num4;
					}
					if (flag2 && buttonControl.wasReleasedThisFrame)
					{
						num3 |= num4;
					}
				}
			}
			ControllerInput._downButtons = (num & ~ControllerInput._heldButtons) | num2;
			ControllerInput._upButtons = (ControllerInput._heldButtons & ~num) | num3;
			ControllerInput._heldButtons = num;
			bool flag3 = num > 0U;
			if (gamepad != null)
			{
				flag3 |= ControllerInput.Move.sqrMagnitude > 0.09f || ControllerInput.Look.sqrMagnitude > 0.09f;
				int num5 = 0;
				while (num5 <= 15 && !flag3)
				{
					flag3 = ControllerInput.GetButtonDown((ControllerButton)num5);
					num5++;
				}
			}
			if (flag3)
			{
				ControllerInput._active = true;
			}
			else
			{
				Keyboard current = Keyboard.current;
				if (current == null || !current.anyKey.wasPressedThisFrame)
				{
					Mouse current2 = Mouse.current;
					if (current2 == null || !current2.leftButton.wasPressedThisFrame)
					{
						Mouse current3 = Mouse.current;
						if ((current3 == null || !current3.rightButton.wasPressedThisFrame) && (Mouse.current == null || Mouse.current.delta.ReadValue().sqrMagnitude <= 4f))
						{
							goto IL_01E0;
						}
					}
				}
				ControllerInput._active = false;
			}
			IL_01E0:
			if (gamepad == null)
			{
				ControllerInput._active = false;
			}
			bool flag4 = false;
			bool flag5 = false;
			if (ControllerInput.GetButtonDown(ControllerButton.Select))
			{
				ControllerInput._selectStarted = (ControllerInput.Modifier ? (-1f) : Time.realtimeSinceStartup);
				ControllerInput._selectHeld = false;
			}
			if (ControllerInput.Modifier)
			{
				ControllerInput._selectStarted = -1f;
			}
			if (ControllerInput._selectStarted >= 0f && ControllerInput.GetButton(ControllerButton.Select) && !ControllerInput._selectHeld && Time.realtimeSinceStartup - ControllerInput._selectStarted >= 0.45f)
			{
				flag5 = true;
				ControllerInput._selectHeld = true;
			}
			if (ControllerInput.GetButtonUp(ControllerButton.Select) && ControllerInput._selectStarted >= 0f)
			{
				flag4 = !ControllerInput._selectHeld;
				ControllerInput._selectStarted = -1f;
			}
			foreach (KeyCode keyCode in ControllerBindings.Keys)
			{
				int num6 = (int)keyCode;
				bool flag6 = gamepad != null && ((keyCode == KeyCode.P) ? flag4 : ((keyCode == KeyCode.M) ? flag5 : ControllerBindings.Read(keyCode)));
				bool flag7 = flag6 && !ControllerInput.Held[num6];
				ControllerButton controllerButton;
				bool flag8;
				if (flag7 && ControllerBindings.TryButton(keyCode, out controllerButton, out flag8) && !flag8)
				{
					flag7 = ControllerInput.GetButtonDown(controllerButton);
				}
				if (flag7 && keyCode == KeyCode.Escape)
				{
					flag7 = ControllerInput.GetButtonDown(ControllerButton.Start) || (!ControllerInput.Modifier && ControllerInput.GetButtonDown(ControllerButton.East));
				}
				ControllerInput.Down[num6] = flag7;
				ControllerInput.Up[num6] = !flag6 && ControllerInput.Held[num6];
				ControllerInput.Held[num6] = flag6;
			}
		}

		// Token: 0x06004B1B RID: 19227 RVA: 0x001D3DB8 File Offset: 0x001D1FB8
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
		private static void Reset()
		{
			ControllerInput._sampleFrame = (ControllerInput._consumedFrame = -1);
			ControllerInput._active = false;
			ControllerInput._lastPad = null;
			ControllerInput._selectStarted = -1f;
			ControllerInput._selectHeld = false;
			ControllerInput._heldButtons = (ControllerInput._downButtons = (ControllerInput._upButtons = 0U));
			ControllerInput._edgeUpdateCount = uint.MaxValue;
			Array.Clear(ControllerInput.Held, 0, ControllerInput.Held.Length);
			Array.Clear(ControllerInput.Down, 0, ControllerInput.Down.Length);
			Array.Clear(ControllerInput.Up, 0, ControllerInput.Up.Length);
		}

		// Token: 0x04004CF8 RID: 19704
		private static int _sampleFrame = -1;

		// Token: 0x04004CF9 RID: 19705
		private static int _consumedFrame = -1;

		// Token: 0x04004CFA RID: 19706
		private static bool _active;

		// Token: 0x04004CFB RID: 19707
		private static Gamepad _lastPad;

		// Token: 0x04004CFC RID: 19708
		private static uint _heldButtons;

		// Token: 0x04004CFD RID: 19709
		private static uint _downButtons;

		// Token: 0x04004CFE RID: 19710
		private static uint _upButtons;

		// Token: 0x04004CFF RID: 19711
		private static uint _edgeUpdateCount = uint.MaxValue;

		// Token: 0x04004D00 RID: 19712
		private static readonly bool[] Held = new bool[512];

		// Token: 0x04004D01 RID: 19713
		private static readonly bool[] Down = new bool[512];

		// Token: 0x04004D02 RID: 19714
		private static readonly bool[] Up = new bool[512];

		// Token: 0x04004D03 RID: 19715
		private static float _selectStarted = -1f;

		// Token: 0x04004D04 RID: 19716
		private static bool _selectHeld;

		// Token: 0x04004D05 RID: 19717
		private const float MapHoldSeconds = 0.45f;
	}
}
