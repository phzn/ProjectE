using System;
using UnityEngine;

namespace ProjectExhibitionist.Core
{
	// Token: 0x02000A46 RID: 2630
	public static class ControllerBindings
	{
		// Token: 0x06004ACE RID: 19150 RVA: 0x001D1FC4 File Offset: 0x001D01C4
		internal static bool Read(KeyCode key)
		{
			bool modifier = ControllerInput.Modifier;
			ControllerButton controllerButton;
			bool flag;
			if (ControllerBindings.TryButton(key, out controllerButton, out flag))
			{
				return modifier == flag && ControllerInput.GetButton(controllerButton);
			}
			if (key <= KeyCode.D)
			{
				if (key <= KeyCode.Escape)
				{
					if (key == KeyCode.Return)
					{
						return ControllerInput.GetButton(ControllerButton.South);
					}
					if (key == KeyCode.Escape)
					{
						return ControllerInput.GetButton(ControllerButton.Start) || (!modifier && ControllerInput.GetButton(ControllerButton.East));
					}
				}
				else
				{
					if (key == KeyCode.A)
					{
						return ControllerInput.Move.x < -0.45f;
					}
					if (key == KeyCode.D)
					{
						return ControllerInput.Move.x > 0.45f;
					}
				}
			}
			else if (key <= KeyCode.W)
			{
				if (key == KeyCode.S)
				{
					return ControllerInput.Move.y < -0.45f;
				}
				if (key == KeyCode.W)
				{
					return ControllerInput.Move.y > 0.45f;
				}
			}
			else
			{
				if (key == KeyCode.UpArrow)
				{
					return modifier && ControllerInput.GetButton(ControllerButton.DpadUp);
				}
				if (key == KeyCode.DownArrow)
				{
					return modifier && ControllerInput.GetButton(ControllerButton.DpadDown);
				}
			}
			return false;
		}

		// Token: 0x06004ACF RID: 19151 RVA: 0x001D20C8 File Offset: 0x001D02C8
		public static bool TryButton(KeyCode key, out ControllerButton button, out bool modifier)
		{
			modifier = false;
			if (key <= KeyCode.CapsLock)
			{
				if (key <= KeyCode.Space)
				{
					if (key == KeyCode.Tab)
					{
						button = ControllerButton.North;
						modifier = true;
						return true;
					}
					if (key != KeyCode.Space)
					{
						goto IL_0167;
					}
					button = ControllerButton.South;
					return true;
				}
				else
				{
					switch (key)
					{
					case KeyCode.B:
						button = ControllerButton.DpadLeft;
						modifier = true;
						return true;
					case KeyCode.C:
						button = ControllerButton.North;
						return true;
					case KeyCode.D:
					case KeyCode.L:
					case KeyCode.M:
					case KeyCode.O:
					case KeyCode.P:
					case KeyCode.S:
					case KeyCode.T:
					case KeyCode.U:
					case KeyCode.W:
					case KeyCode.Y:
						goto IL_0167;
					case KeyCode.E:
						button = ControllerButton.West;
						return true;
					case KeyCode.F:
						button = ControllerButton.RightTrigger;
						return true;
					case KeyCode.G:
						button = ControllerButton.South;
						modifier = true;
						return true;
					case KeyCode.H:
						button = ControllerButton.RightShoulder;
						modifier = true;
						return true;
					case KeyCode.I:
						button = ControllerButton.DpadUp;
						return true;
					case KeyCode.J:
						button = ControllerButton.RightTrigger;
						modifier = true;
						return true;
					case KeyCode.K:
						button = ControllerButton.DpadDown;
						return true;
					case KeyCode.N:
						button = ControllerButton.DpadRight;
						modifier = true;
						return true;
					case KeyCode.Q:
						button = ControllerButton.LeftShoulder;
						modifier = true;
						return true;
					case KeyCode.R:
						button = ControllerButton.West;
						modifier = true;
						return true;
					case KeyCode.V:
						button = ControllerButton.RightStick;
						return true;
					case KeyCode.X:
						break;
					case KeyCode.Z:
						button = ControllerButton.East;
						modifier = true;
						return true;
					default:
						if (key != KeyCode.CapsLock)
						{
							goto IL_0167;
						}
						button = ControllerButton.Select;
						modifier = true;
						return true;
					}
				}
			}
			else if (key <= KeyCode.LeftControl)
			{
				if (key == KeyCode.LeftShift)
				{
					button = ControllerButton.LeftStick;
					return true;
				}
				if (key != KeyCode.LeftControl)
				{
					goto IL_0167;
				}
				button = ControllerButton.LeftStick;
				modifier = true;
				return true;
			}
			else
			{
				if (key == KeyCode.Mouse0)
				{
					button = ControllerButton.RightShoulder;
					return true;
				}
				if (key != KeyCode.Mouse1)
				{
					goto IL_0167;
				}
			}
			button = ControllerButton.LeftShoulder;
			return true;
			IL_0167:
			button = ControllerButton.South;
			return false;
		}

		// Token: 0x06004AD0 RID: 19152 RVA: 0x001D2240 File Offset: 0x001D0440
		public static bool HasKey(KeyCode key)
		{
			KeyCode[] keys = ControllerBindings.Keys;
			for (int i = 0; i < keys.Length; i++)
			{
				if (keys[i] == key)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06004AD1 RID: 19153 RVA: 0x001D226C File Offset: 0x001D046C
		public static string Label(KeyCode key)
		{
			ControllerButton controllerButton;
			bool flag;
			if (ControllerBindings.TryButton(key, out controllerButton, out flag))
			{
				return (flag ? "LT + " : "") + ControllerBindings.Label(controllerButton);
			}
			if (key <= KeyCode.D)
			{
				if (key <= KeyCode.Escape)
				{
					if (key == KeyCode.Return)
					{
						return "A";
					}
					if (key != KeyCode.Escape)
					{
						goto IL_00B8;
					}
					return "B / Menu";
				}
				else if (key != KeyCode.A && key != KeyCode.D)
				{
					goto IL_00B8;
				}
			}
			else if (key <= KeyCode.P)
			{
				if (key == KeyCode.M)
				{
					return "Hold View";
				}
				if (key != KeyCode.P)
				{
					goto IL_00B8;
				}
				return "View";
			}
			else if (key != KeyCode.S && key != KeyCode.W)
			{
				switch (key)
				{
				case KeyCode.UpArrow:
					return "LT + D-Pad Up";
				case KeyCode.DownArrow:
					return "LT + D-Pad Down";
				case KeyCode.RightArrow:
					return "LT + D-Pad Right";
				case KeyCode.LeftArrow:
					return "LT + D-Pad Left";
				default:
					goto IL_00B8;
				}
			}
			return "LS";
			IL_00B8:
			return key.ToString();
		}

		// Token: 0x06004AD2 RID: 19154 RVA: 0x001D2340 File Offset: 0x001D0540
		public static string Label(ControllerButton button)
		{
			switch (button)
			{
			case ControllerButton.South:
				return "A";
			case ControllerButton.East:
				return "B";
			case ControllerButton.West:
				return "X";
			case ControllerButton.North:
				return "Y";
			case ControllerButton.LeftShoulder:
				return "LB";
			case ControllerButton.RightShoulder:
				return "RB";
			case ControllerButton.LeftTrigger:
				return "LT";
			case ControllerButton.RightTrigger:
				return "RT";
			case ControllerButton.LeftStick:
				return "L3";
			case ControllerButton.RightStick:
				return "R3";
			case ControllerButton.Start:
				return "Menu";
			case ControllerButton.Select:
				return "View";
			case ControllerButton.DpadUp:
				return "D-Pad Up";
			case ControllerButton.DpadDown:
				return "D-Pad Down";
			case ControllerButton.DpadLeft:
				return "D-Pad Left";
			default:
				return "D-Pad Right";
			}
		}

		// Token: 0x06004AD3 RID: 19155 RVA: 0x001D23F0 File Offset: 0x001D05F0
		public static bool TryLegacyButton(string name, out ControllerButton button)
		{
			uint num = <PrivateImplementationDetails>.ComputeStringHash(name);
			if (num <= 2012048776U)
			{
				if (num <= 1030614021U)
				{
					if (num <= 272361348U)
					{
						if (num != 182978943U)
						{
							if (num != 235163378U)
							{
								if (num != 272361348U)
								{
									goto IL_0424;
								}
								if (!(name == "LeftStickButton"))
								{
									goto IL_0424;
								}
							}
							else
							{
								if (!(name == "R3"))
								{
									goto IL_0424;
								}
								goto IL_03FA;
							}
						}
						else
						{
							if (!(name == "Start"))
							{
								goto IL_0424;
							}
							button = ControllerButton.Start;
							return true;
						}
					}
					else if (num != 367118664U)
					{
						if (num != 934753068U)
						{
							if (num != 1030614021U)
							{
								goto IL_0424;
							}
							if (!(name == "DPadLeft"))
							{
								goto IL_0424;
							}
							button = ControllerButton.DpadLeft;
							return true;
						}
						else
						{
							if (!(name == "LeftTrigger"))
							{
								goto IL_0424;
							}
							goto IL_03EB;
						}
					}
					else if (!(name == "L3"))
					{
						goto IL_0424;
					}
				}
				else
				{
					if (num > 1565262825U)
					{
						if (num <= 1761926707U)
						{
							if (num != 1726105803U)
							{
								if (num != 1761926707U)
								{
									goto IL_0424;
								}
								if (!(name == "RT"))
								{
									goto IL_0424;
								}
							}
							else
							{
								if (!(name == "LB"))
								{
									goto IL_0424;
								}
								goto IL_03E1;
							}
						}
						else if (num != 1853917539U)
						{
							if (num != 2012048776U)
							{
								goto IL_0424;
							}
							if (!(name == "LeftStickClick"))
							{
								goto IL_0424;
							}
							goto IL_03F5;
						}
						else if (!(name == "RightTrigger"))
						{
							goto IL_0424;
						}
						button = ControllerButton.RightTrigger;
						return true;
					}
					if (num != 1049176909U)
					{
						if (num != 1305824403U)
						{
							if (num != 1565262825U)
							{
								goto IL_0424;
							}
							if (!(name == "RightStickClick"))
							{
								goto IL_0424;
							}
							goto IL_03FA;
						}
						else
						{
							if (!(name == "LeftBumper"))
							{
								goto IL_0424;
							}
							goto IL_03E1;
						}
					}
					else
					{
						if (!(name == "Select"))
						{
							goto IL_0424;
						}
						goto IL_0406;
					}
				}
				IL_03F5:
				button = ControllerButton.LeftStick;
				return true;
			}
			if (num <= 3264564162U)
			{
				if (num <= 2131034325U)
				{
					if (num != 2083978775U)
					{
						if (num != 2095213421U)
						{
							if (num != 2131034325U)
							{
								goto IL_0424;
							}
							if (!(name == "RB"))
							{
								goto IL_0424;
							}
						}
						else
						{
							if (!(name == "LT"))
							{
								goto IL_0424;
							}
							goto IL_03EB;
						}
					}
					else
					{
						if (!(name == "RightStickButton"))
						{
							goto IL_0424;
						}
						goto IL_03FA;
					}
				}
				else if (num <= 3085282398U)
				{
					if (num != 2199480070U)
					{
						if (num != 3085282398U)
						{
							goto IL_0424;
						}
						if (!(name == "DPadRight"))
						{
							goto IL_0424;
						}
						button = ControllerButton.DpadRight;
						return true;
					}
					else if (!(name == "RightBumper"))
					{
						goto IL_0424;
					}
				}
				else if (num != 3247264816U)
				{
					if (num != 3264564162U)
					{
						goto IL_0424;
					}
					if (!(name == "Back"))
					{
						goto IL_0424;
					}
					goto IL_0406;
				}
				else
				{
					if (!(name == "DPadDown"))
					{
						goto IL_0424;
					}
					button = ControllerButton.DpadDown;
					return true;
				}
			}
			else if (num <= 3399794141U)
			{
				if (num != 3289118412U)
				{
					if (num != 3339451269U)
					{
						if (num != 3399794141U)
						{
							goto IL_0424;
						}
						if (!(name == "RightShoulder"))
						{
							goto IL_0424;
						}
					}
					else
					{
						if (!(name == "B"))
						{
							goto IL_0424;
						}
						button = ControllerButton.East;
						return true;
					}
				}
				else
				{
					if (!(name == "A"))
					{
						goto IL_0424;
					}
					button = ControllerButton.South;
					return true;
				}
			}
			else if (num <= 3691781268U)
			{
				if (num != 3548454985U)
				{
					if (num != 3691781268U)
					{
						goto IL_0424;
					}
					if (!(name == "Y"))
					{
						goto IL_0424;
					}
					button = ControllerButton.North;
					return true;
				}
				else
				{
					if (!(name == "DPadUp"))
					{
						goto IL_0424;
					}
					button = ControllerButton.DpadUp;
					return true;
				}
			}
			else if (num != 3708558887U)
			{
				if (num != 4211078808U)
				{
					goto IL_0424;
				}
				if (!(name == "LeftShoulder"))
				{
					goto IL_0424;
				}
				goto IL_03E1;
			}
			else
			{
				if (!(name == "X"))
				{
					goto IL_0424;
				}
				button = ControllerButton.West;
				return true;
			}
			button = ControllerButton.RightShoulder;
			return true;
			IL_03E1:
			button = ControllerButton.LeftShoulder;
			return true;
			IL_03EB:
			button = ControllerButton.LeftTrigger;
			return true;
			IL_03FA:
			button = ControllerButton.RightStick;
			return true;
			IL_0406:
			button = ControllerButton.Select;
			return true;
			IL_0424:
			button = ControllerButton.South;
			return false;
		}

		// Token: 0x04004CD3 RID: 19667
		internal static readonly KeyCode[] Keys = new KeyCode[]
		{
			KeyCode.Space,
			KeyCode.Return,
			KeyCode.Mouse0,
			KeyCode.Mouse1,
			KeyCode.E,
			KeyCode.F,
			KeyCode.C,
			KeyCode.X,
			KeyCode.V,
			KeyCode.I,
			KeyCode.T,
			KeyCode.P,
			KeyCode.M,
			KeyCode.Escape,
			KeyCode.LeftShift,
			KeyCode.LeftControl,
			KeyCode.Tab,
			KeyCode.R,
			KeyCode.G,
			KeyCode.Q,
			KeyCode.H,
			KeyCode.UpArrow,
			KeyCode.DownArrow,
			KeyCode.LeftArrow,
			KeyCode.RightArrow,
			KeyCode.W,
			KeyCode.A,
			KeyCode.S,
			KeyCode.D,
			KeyCode.Z,
			KeyCode.B,
			KeyCode.N,
			KeyCode.J,
			KeyCode.K,
			KeyCode.CapsLock
		};
	}
}
