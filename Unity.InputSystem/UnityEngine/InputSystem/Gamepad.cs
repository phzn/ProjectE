using System;
using System.ComponentModel;
using UnityEngine.InputSystem.Controls;
using UnityEngine.InputSystem.Haptics;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.InputSystem.Utilities;

namespace UnityEngine.InputSystem
{
	// Token: 0x02000037 RID: 55
	[InputControlLayout(stateType = typeof(GamepadState), isGenericTypeOfDevice = true)]
	public class Gamepad : InputDevice, IDualMotorRumble, IHaptics
	{
		// Token: 0x17000120 RID: 288
		// (get) Token: 0x0600046E RID: 1134 RVA: 0x000138AB File Offset: 0x00011AAB
		// (set) Token: 0x0600046F RID: 1135 RVA: 0x000138B3 File Offset: 0x00011AB3
		public ButtonControl buttonWest { get; protected set; }

		// Token: 0x17000121 RID: 289
		// (get) Token: 0x06000470 RID: 1136 RVA: 0x000138BC File Offset: 0x00011ABC
		// (set) Token: 0x06000471 RID: 1137 RVA: 0x000138C4 File Offset: 0x00011AC4
		public ButtonControl buttonNorth { get; protected set; }

		// Token: 0x17000122 RID: 290
		// (get) Token: 0x06000472 RID: 1138 RVA: 0x000138CD File Offset: 0x00011ACD
		// (set) Token: 0x06000473 RID: 1139 RVA: 0x000138D5 File Offset: 0x00011AD5
		public ButtonControl buttonSouth { get; protected set; }

		// Token: 0x17000123 RID: 291
		// (get) Token: 0x06000474 RID: 1140 RVA: 0x000138DE File Offset: 0x00011ADE
		// (set) Token: 0x06000475 RID: 1141 RVA: 0x000138E6 File Offset: 0x00011AE6
		public ButtonControl buttonEast { get; protected set; }

		// Token: 0x17000124 RID: 292
		// (get) Token: 0x06000476 RID: 1142 RVA: 0x000138EF File Offset: 0x00011AEF
		// (set) Token: 0x06000477 RID: 1143 RVA: 0x000138F7 File Offset: 0x00011AF7
		public ButtonControl leftStickButton { get; protected set; }

		// Token: 0x17000125 RID: 293
		// (get) Token: 0x06000478 RID: 1144 RVA: 0x00013900 File Offset: 0x00011B00
		// (set) Token: 0x06000479 RID: 1145 RVA: 0x00013908 File Offset: 0x00011B08
		public ButtonControl rightStickButton { get; protected set; }

		// Token: 0x17000126 RID: 294
		// (get) Token: 0x0600047A RID: 1146 RVA: 0x00013911 File Offset: 0x00011B11
		// (set) Token: 0x0600047B RID: 1147 RVA: 0x00013919 File Offset: 0x00011B19
		public ButtonControl startButton { get; protected set; }

		// Token: 0x17000127 RID: 295
		// (get) Token: 0x0600047C RID: 1148 RVA: 0x00013922 File Offset: 0x00011B22
		// (set) Token: 0x0600047D RID: 1149 RVA: 0x0001392A File Offset: 0x00011B2A
		public ButtonControl selectButton { get; protected set; }

		// Token: 0x17000128 RID: 296
		// (get) Token: 0x0600047E RID: 1150 RVA: 0x00013933 File Offset: 0x00011B33
		// (set) Token: 0x0600047F RID: 1151 RVA: 0x0001393B File Offset: 0x00011B3B
		public DpadControl dpad { get; protected set; }

		// Token: 0x17000129 RID: 297
		// (get) Token: 0x06000480 RID: 1152 RVA: 0x00013944 File Offset: 0x00011B44
		// (set) Token: 0x06000481 RID: 1153 RVA: 0x0001394C File Offset: 0x00011B4C
		public ButtonControl leftShoulder { get; protected set; }

		// Token: 0x1700012A RID: 298
		// (get) Token: 0x06000482 RID: 1154 RVA: 0x00013955 File Offset: 0x00011B55
		// (set) Token: 0x06000483 RID: 1155 RVA: 0x0001395D File Offset: 0x00011B5D
		public ButtonControl rightShoulder { get; protected set; }

		// Token: 0x1700012B RID: 299
		// (get) Token: 0x06000484 RID: 1156 RVA: 0x00013966 File Offset: 0x00011B66
		// (set) Token: 0x06000485 RID: 1157 RVA: 0x0001396E File Offset: 0x00011B6E
		public StickControl leftStick { get; protected set; }

		// Token: 0x1700012C RID: 300
		// (get) Token: 0x06000486 RID: 1158 RVA: 0x00013977 File Offset: 0x00011B77
		// (set) Token: 0x06000487 RID: 1159 RVA: 0x0001397F File Offset: 0x00011B7F
		public StickControl rightStick { get; protected set; }

		// Token: 0x1700012D RID: 301
		// (get) Token: 0x06000488 RID: 1160 RVA: 0x00013988 File Offset: 0x00011B88
		// (set) Token: 0x06000489 RID: 1161 RVA: 0x00013990 File Offset: 0x00011B90
		public ButtonControl leftTrigger { get; protected set; }

		// Token: 0x1700012E RID: 302
		// (get) Token: 0x0600048A RID: 1162 RVA: 0x00013999 File Offset: 0x00011B99
		// (set) Token: 0x0600048B RID: 1163 RVA: 0x000139A1 File Offset: 0x00011BA1
		public ButtonControl rightTrigger { get; protected set; }

		// Token: 0x1700012F RID: 303
		// (get) Token: 0x0600048C RID: 1164 RVA: 0x000139AA File Offset: 0x00011BAA
		public ButtonControl aButton
		{
			get
			{
				return this.buttonSouth;
			}
		}

		// Token: 0x17000130 RID: 304
		// (get) Token: 0x0600048D RID: 1165 RVA: 0x000139B2 File Offset: 0x00011BB2
		public ButtonControl bButton
		{
			get
			{
				return this.buttonEast;
			}
		}

		// Token: 0x17000131 RID: 305
		// (get) Token: 0x0600048E RID: 1166 RVA: 0x000139BA File Offset: 0x00011BBA
		public ButtonControl xButton
		{
			get
			{
				return this.buttonWest;
			}
		}

		// Token: 0x17000132 RID: 306
		// (get) Token: 0x0600048F RID: 1167 RVA: 0x000139C2 File Offset: 0x00011BC2
		public ButtonControl yButton
		{
			get
			{
				return this.buttonNorth;
			}
		}

		// Token: 0x17000133 RID: 307
		// (get) Token: 0x06000490 RID: 1168 RVA: 0x000139CA File Offset: 0x00011BCA
		public ButtonControl triangleButton
		{
			get
			{
				return this.buttonNorth;
			}
		}

		// Token: 0x17000134 RID: 308
		// (get) Token: 0x06000491 RID: 1169 RVA: 0x000139D2 File Offset: 0x00011BD2
		public ButtonControl squareButton
		{
			get
			{
				return this.buttonWest;
			}
		}

		// Token: 0x17000135 RID: 309
		// (get) Token: 0x06000492 RID: 1170 RVA: 0x000139DA File Offset: 0x00011BDA
		public ButtonControl circleButton
		{
			get
			{
				return this.buttonEast;
			}
		}

		// Token: 0x17000136 RID: 310
		// (get) Token: 0x06000493 RID: 1171 RVA: 0x000139E2 File Offset: 0x00011BE2
		public ButtonControl crossButton
		{
			get
			{
				return this.buttonSouth;
			}
		}

		// Token: 0x17000137 RID: 311
		public ButtonControl this[GamepadButton button]
		{
			get
			{
				switch (button)
				{
				case GamepadButton.DpadUp:
					return this.dpad.up;
				case GamepadButton.DpadDown:
					return this.dpad.down;
				case GamepadButton.DpadLeft:
					return this.dpad.left;
				case GamepadButton.DpadRight:
					return this.dpad.right;
				case GamepadButton.North:
					return this.buttonNorth;
				case GamepadButton.East:
					return this.buttonEast;
				case GamepadButton.South:
					return this.buttonSouth;
				case GamepadButton.West:
					return this.buttonWest;
				case GamepadButton.LeftStick:
					return this.leftStickButton;
				case GamepadButton.RightStick:
					return this.rightStickButton;
				case GamepadButton.LeftShoulder:
					return this.leftShoulder;
				case GamepadButton.RightShoulder:
					return this.rightShoulder;
				case GamepadButton.Start:
					return this.startButton;
				case GamepadButton.Select:
					return this.selectButton;
				default:
					if (button == GamepadButton.LeftTrigger)
					{
						return this.leftTrigger;
					}
					if (button != GamepadButton.RightTrigger)
					{
						throw new InvalidEnumArgumentException("button", (int)button, typeof(GamepadButton));
					}
					return this.rightTrigger;
				}
			}
		}

		// Token: 0x17000138 RID: 312
		// (get) Token: 0x06000495 RID: 1173 RVA: 0x00013ADF File Offset: 0x00011CDF
		// (set) Token: 0x06000496 RID: 1174 RVA: 0x00013AE6 File Offset: 0x00011CE6
		public static Gamepad current { get; private set; }

		// Token: 0x17000139 RID: 313
		// (get) Token: 0x06000497 RID: 1175 RVA: 0x00013AEE File Offset: 0x00011CEE
		public new static ReadOnlyArray<Gamepad> all
		{
			get
			{
				return new ReadOnlyArray<Gamepad>(Gamepad.s_Gamepads, 0, Gamepad.s_GamepadCount);
			}
		}

		// Token: 0x06000498 RID: 1176 RVA: 0x00013B00 File Offset: 0x00011D00
		protected override void FinishSetup()
		{
			this.buttonWest = base.GetChildControl<ButtonControl>("buttonWest");
			this.buttonNorth = base.GetChildControl<ButtonControl>("buttonNorth");
			this.buttonSouth = base.GetChildControl<ButtonControl>("buttonSouth");
			this.buttonEast = base.GetChildControl<ButtonControl>("buttonEast");
			this.startButton = base.GetChildControl<ButtonControl>("start");
			this.selectButton = base.GetChildControl<ButtonControl>("select");
			this.leftStickButton = base.GetChildControl<ButtonControl>("leftStickPress");
			this.rightStickButton = base.GetChildControl<ButtonControl>("rightStickPress");
			this.dpad = base.GetChildControl<DpadControl>("dpad");
			this.leftShoulder = base.GetChildControl<ButtonControl>("leftShoulder");
			this.rightShoulder = base.GetChildControl<ButtonControl>("rightShoulder");
			this.leftStick = base.GetChildControl<StickControl>("leftStick");
			this.rightStick = base.GetChildControl<StickControl>("rightStick");
			this.leftTrigger = base.GetChildControl<ButtonControl>("leftTrigger");
			this.rightTrigger = base.GetChildControl<ButtonControl>("rightTrigger");
			base.FinishSetup();
		}

		// Token: 0x06000499 RID: 1177 RVA: 0x00013C12 File Offset: 0x00011E12
		public override void MakeCurrent()
		{
			base.MakeCurrent();
			Gamepad.current = this;
		}

		// Token: 0x0600049A RID: 1178 RVA: 0x00013C20 File Offset: 0x00011E20
		protected override void OnAdded()
		{
			ArrayHelpers.AppendWithCapacity<Gamepad>(ref Gamepad.s_Gamepads, ref Gamepad.s_GamepadCount, this, 10);
		}

		// Token: 0x0600049B RID: 1179 RVA: 0x00013C38 File Offset: 0x00011E38
		protected override void OnRemoved()
		{
			if (Gamepad.current == this)
			{
				Gamepad.current = null;
			}
			int num = Gamepad.s_Gamepads.IndexOfReference(this, Gamepad.s_GamepadCount);
			if (num != -1)
			{
				Gamepad.s_Gamepads.EraseAtWithCapacity<Gamepad>(ref Gamepad.s_GamepadCount, num);
			}
		}

		// Token: 0x0600049C RID: 1180 RVA: 0x00013C78 File Offset: 0x00011E78
		public virtual void PauseHaptics()
		{
			this.m_Rumble.PauseHaptics(this);
		}

		// Token: 0x0600049D RID: 1181 RVA: 0x00013C86 File Offset: 0x00011E86
		public virtual void ResumeHaptics()
		{
			this.m_Rumble.ResumeHaptics(this);
		}

		// Token: 0x0600049E RID: 1182 RVA: 0x00013C94 File Offset: 0x00011E94
		public virtual void ResetHaptics()
		{
			this.m_Rumble.ResetHaptics(this);
		}

		// Token: 0x0600049F RID: 1183 RVA: 0x00013CA2 File Offset: 0x00011EA2
		public virtual void SetMotorSpeeds(float lowFrequency, float highFrequency)
		{
			this.m_Rumble.SetMotorSpeeds(this, lowFrequency, highFrequency);
		}

		// Token: 0x0400015E RID: 350
		private DualMotorRumble m_Rumble;

		// Token: 0x0400015F RID: 351
		private static int s_GamepadCount;

		// Token: 0x04000160 RID: 352
		private static Gamepad[] s_Gamepads;
	}
}
