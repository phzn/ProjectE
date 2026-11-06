using System;
using UnityEngine.InputSystem.Controls;
using UnityEngine.InputSystem.Haptics;
using UnityEngine.InputSystem.HID;
using UnityEngine.InputSystem.Layouts;

namespace UnityEngine.InputSystem.DualShock
{
	// Token: 0x020000A2 RID: 162
	[InputControlLayout(displayName = "PlayStation Controller")]
	public class DualShockGamepad : Gamepad, IDualShockHaptics, IDualMotorRumble, IHaptics
	{
		// Token: 0x17000358 RID: 856
		// (get) Token: 0x06000C88 RID: 3208 RVA: 0x00042CD6 File Offset: 0x00040ED6
		// (set) Token: 0x06000C89 RID: 3209 RVA: 0x00042CDE File Offset: 0x00040EDE
		[InputControl(name = "buttonWest", displayName = "Square", shortDisplayName = "Square")]
		[InputControl(name = "buttonNorth", displayName = "Triangle", shortDisplayName = "Triangle")]
		[InputControl(name = "buttonEast", displayName = "Circle", shortDisplayName = "Circle")]
		[InputControl(name = "buttonSouth", displayName = "Cross", shortDisplayName = "Cross")]
		[InputControl]
		public ButtonControl touchpadButton { get; protected set; }

		// Token: 0x17000359 RID: 857
		// (get) Token: 0x06000C8A RID: 3210 RVA: 0x00042CE7 File Offset: 0x00040EE7
		// (set) Token: 0x06000C8B RID: 3211 RVA: 0x00042CEF File Offset: 0x00040EEF
		[InputControl(name = "start", displayName = "Options")]
		public ButtonControl optionsButton { get; protected set; }

		// Token: 0x1700035A RID: 858
		// (get) Token: 0x06000C8C RID: 3212 RVA: 0x00042CF8 File Offset: 0x00040EF8
		// (set) Token: 0x06000C8D RID: 3213 RVA: 0x00042D00 File Offset: 0x00040F00
		[InputControl(name = "select", displayName = "Share")]
		public ButtonControl shareButton { get; protected set; }

		// Token: 0x1700035B RID: 859
		// (get) Token: 0x06000C8E RID: 3214 RVA: 0x00042D09 File Offset: 0x00040F09
		// (set) Token: 0x06000C8F RID: 3215 RVA: 0x00042D11 File Offset: 0x00040F11
		[InputControl(name = "leftShoulder", displayName = "L1", shortDisplayName = "L1")]
		public ButtonControl L1 { get; protected set; }

		// Token: 0x1700035C RID: 860
		// (get) Token: 0x06000C90 RID: 3216 RVA: 0x00042D1A File Offset: 0x00040F1A
		// (set) Token: 0x06000C91 RID: 3217 RVA: 0x00042D22 File Offset: 0x00040F22
		[InputControl(name = "rightShoulder", displayName = "R1", shortDisplayName = "R1")]
		public ButtonControl R1 { get; protected set; }

		// Token: 0x1700035D RID: 861
		// (get) Token: 0x06000C92 RID: 3218 RVA: 0x00042D2B File Offset: 0x00040F2B
		// (set) Token: 0x06000C93 RID: 3219 RVA: 0x00042D33 File Offset: 0x00040F33
		[InputControl(name = "leftTrigger", displayName = "L2", shortDisplayName = "L2")]
		public ButtonControl L2 { get; protected set; }

		// Token: 0x1700035E RID: 862
		// (get) Token: 0x06000C94 RID: 3220 RVA: 0x00042D3C File Offset: 0x00040F3C
		// (set) Token: 0x06000C95 RID: 3221 RVA: 0x00042D44 File Offset: 0x00040F44
		[InputControl(name = "rightTrigger", displayName = "R2", shortDisplayName = "R2")]
		public ButtonControl R2 { get; protected set; }

		// Token: 0x1700035F RID: 863
		// (get) Token: 0x06000C96 RID: 3222 RVA: 0x00042D4D File Offset: 0x00040F4D
		// (set) Token: 0x06000C97 RID: 3223 RVA: 0x00042D55 File Offset: 0x00040F55
		[InputControl(name = "leftStickPress", displayName = "L3", shortDisplayName = "L3")]
		public ButtonControl L3 { get; protected set; }

		// Token: 0x17000360 RID: 864
		// (get) Token: 0x06000C98 RID: 3224 RVA: 0x00042D5E File Offset: 0x00040F5E
		// (set) Token: 0x06000C99 RID: 3225 RVA: 0x00042D66 File Offset: 0x00040F66
		[InputControl(name = "rightStickPress", displayName = "R3", shortDisplayName = "R3")]
		public ButtonControl R3 { get; protected set; }

		// Token: 0x17000361 RID: 865
		// (get) Token: 0x06000C9A RID: 3226 RVA: 0x00042D6F File Offset: 0x00040F6F
		// (set) Token: 0x06000C9B RID: 3227 RVA: 0x00042D76 File Offset: 0x00040F76
		public new static DualShockGamepad current { get; private set; }

		// Token: 0x17000362 RID: 866
		// (get) Token: 0x06000C9C RID: 3228 RVA: 0x00042D7E File Offset: 0x00040F7E
		// (set) Token: 0x06000C9D RID: 3229 RVA: 0x00042D86 File Offset: 0x00040F86
		internal HID.HIDDeviceDescriptor hidDescriptor { get; private set; }

		// Token: 0x06000C9E RID: 3230 RVA: 0x00042D8F File Offset: 0x00040F8F
		public override void MakeCurrent()
		{
			base.MakeCurrent();
			DualShockGamepad.current = this;
		}

		// Token: 0x06000C9F RID: 3231 RVA: 0x00042D9D File Offset: 0x00040F9D
		protected override void OnRemoved()
		{
			base.OnRemoved();
			if (DualShockGamepad.current == this)
			{
				DualShockGamepad.current = null;
			}
		}

		// Token: 0x06000CA0 RID: 3232 RVA: 0x00042DB4 File Offset: 0x00040FB4
		protected override void FinishSetup()
		{
			base.FinishSetup();
			this.touchpadButton = base.GetChildControl<ButtonControl>("touchpadButton");
			this.optionsButton = base.startButton;
			this.shareButton = base.selectButton;
			this.L1 = base.leftShoulder;
			this.R1 = base.rightShoulder;
			this.L2 = base.leftTrigger;
			this.R2 = base.rightTrigger;
			this.L3 = base.leftStickButton;
			this.R3 = base.rightStickButton;
			if (this.m_Description.capabilities != null && this.m_Description.interfaceName == "HID")
			{
				this.hidDescriptor = HID.HIDDeviceDescriptor.FromJson(this.m_Description.capabilities);
			}
		}

		// Token: 0x06000CA1 RID: 3233 RVA: 0x00042E72 File Offset: 0x00041072
		public virtual void SetLightBarColor(Color color)
		{
		}
	}
}
