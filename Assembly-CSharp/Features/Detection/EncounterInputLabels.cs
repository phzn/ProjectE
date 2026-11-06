using System;
using Features.Localization;
using Invector.vCharacterController;

namespace Features.Detection
{
	// Token: 0x0200128B RID: 4747
	public static class EncounterInputLabels
	{
		// Token: 0x0600935C RID: 37724 RVA: 0x003C87B0 File Offset: 0x003C69B0
		public static string ForInput(GenericInput input, InputDevice device)
		{
			if (input == null || !input.useInput)
			{
				return string.Empty;
			}
			string text;
			bool flag;
			bool flag2;
			if (device != InputDevice.Joystick)
			{
				if (device != InputDevice.Mobile)
				{
					text = input.keyboard;
					flag = input.keyboardAxis;
					flag2 = input.keyboardAxisInvert;
				}
				else
				{
					text = input.mobile;
					flag = input.mobileAxis;
					flag2 = input.mobileAxisInvert;
				}
			}
			else
			{
				text = input.joystick;
				flag = input.joystickAxis;
				flag2 = input.joystickAxisInvert;
			}
			string text2 = EncounterInputLabels.Format(text);
			if (!string.IsNullOrEmpty(text2) && flag)
			{
				return text2 + (flag2 ? " -" : " +");
			}
			return text2;
		}

		// Token: 0x0600935D RID: 37725 RVA: 0x003C8844 File Offset: 0x003C6A44
		public static string Format(string binding)
		{
			if (string.IsNullOrWhiteSpace(binding) || binding == "None")
			{
				return string.Empty;
			}
			if (binding.StartsWith("Alpha") && binding.Length == 6 && char.IsDigit(binding[5]))
			{
				return binding.Substring(5);
			}
			if (binding.StartsWith("Keypad"))
			{
				return GameLocalization.FormatSource("input", "Num {0}", new object[] { binding.Substring(6) });
			}
			uint num = <PrivateImplementationDetails>.ComputeStringHash(binding);
			string text;
			if (num <= 1231278590U)
			{
				if (num <= 882607823U)
				{
					if (num != 865830204U)
					{
						if (num == 882607823U)
						{
							if (binding == "Mouse1")
							{
								text = "Right Mouse";
								goto IL_01B3;
							}
						}
					}
					else if (binding == "Mouse0")
					{
						text = "Left Mouse";
						goto IL_01B3;
					}
				}
				else if (num != 899385442U)
				{
					if (num == 1231278590U)
					{
						if (binding == "RightControl")
						{
							text = "Right Ctrl";
							goto IL_01B3;
						}
					}
				}
				else if (binding == "Mouse2")
				{
					text = "Middle Mouse";
					goto IL_01B3;
				}
			}
			else if (num <= 3036628469U)
			{
				if (num != 1428210068U)
				{
					if (num == 3036628469U)
					{
						if (binding == "LeftControl")
						{
							text = "Left Ctrl";
							goto IL_01B3;
						}
					}
				}
				else if (binding == "LeftShift")
				{
					text = "Left Shift";
					goto IL_01B3;
				}
			}
			else if (num != 3422663135U)
			{
				if (num == 3592460967U)
				{
					if (binding == "RightShift")
					{
						text = "Right Shift";
						goto IL_01B3;
					}
				}
			}
			else if (binding == "Return")
			{
				text = "Enter";
				goto IL_01B3;
			}
			text = binding;
			IL_01B3:
			return GameLocalization.GetSource("input", text);
		}
	}
}
