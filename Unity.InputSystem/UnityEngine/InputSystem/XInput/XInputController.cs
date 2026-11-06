using System;
using UnityEngine.InputSystem.Controls;
using UnityEngine.InputSystem.Layouts;

namespace UnityEngine.InputSystem.XInput
{
	// Token: 0x0200007C RID: 124
	[InputControlLayout(displayName = "Xbox Controller")]
	public class XInputController : Gamepad
	{
		// Token: 0x170002B8 RID: 696
		// (get) Token: 0x06000A84 RID: 2692 RVA: 0x00038D79 File Offset: 0x00036F79
		// (set) Token: 0x06000A85 RID: 2693 RVA: 0x00038D81 File Offset: 0x00036F81
		[InputControl(name = "buttonSouth", displayName = "A")]
		[InputControl(name = "buttonEast", displayName = "B")]
		[InputControl(name = "buttonWest", displayName = "X")]
		[InputControl(name = "buttonNorth", displayName = "Y")]
		[InputControl(name = "leftShoulder", displayName = "Left Bumper", shortDisplayName = "LB")]
		[InputControl(name = "rightShoulder", displayName = "Right Bumper", shortDisplayName = "RB")]
		[InputControl(name = "leftTrigger", shortDisplayName = "LT")]
		[InputControl(name = "rightTrigger", shortDisplayName = "RT")]
		[InputControl(name = "start", displayName = "Menu", alias = "menu")]
		[InputControl(name = "select", displayName = "View", alias = "view")]
		public ButtonControl menu { get; protected set; }

		// Token: 0x170002B9 RID: 697
		// (get) Token: 0x06000A86 RID: 2694 RVA: 0x00038D8A File Offset: 0x00036F8A
		// (set) Token: 0x06000A87 RID: 2695 RVA: 0x00038D92 File Offset: 0x00036F92
		public ButtonControl view { get; protected set; }

		// Token: 0x170002BA RID: 698
		// (get) Token: 0x06000A88 RID: 2696 RVA: 0x00038D9B File Offset: 0x00036F9B
		public XInputController.DeviceSubType subType
		{
			get
			{
				if (!this.m_HaveParsedCapabilities)
				{
					this.ParseCapabilities();
				}
				return this.m_SubType;
			}
		}

		// Token: 0x170002BB RID: 699
		// (get) Token: 0x06000A89 RID: 2697 RVA: 0x00038DB1 File Offset: 0x00036FB1
		public XInputController.DeviceFlags flags
		{
			get
			{
				if (!this.m_HaveParsedCapabilities)
				{
					this.ParseCapabilities();
				}
				return this.m_Flags;
			}
		}

		// Token: 0x06000A8A RID: 2698 RVA: 0x00038DC7 File Offset: 0x00036FC7
		protected override void FinishSetup()
		{
			base.FinishSetup();
			this.menu = base.startButton;
			this.view = base.selectButton;
		}

		// Token: 0x06000A8B RID: 2699 RVA: 0x00038DE8 File Offset: 0x00036FE8
		private void ParseCapabilities()
		{
			if (!string.IsNullOrEmpty(base.description.capabilities))
			{
				XInputController.Capabilities capabilities = JsonUtility.FromJson<XInputController.Capabilities>(base.description.capabilities);
				this.m_SubType = capabilities.subType;
				this.m_Flags = capabilities.flags;
			}
			this.m_HaveParsedCapabilities = true;
		}

		// Token: 0x040003B1 RID: 945
		private bool m_HaveParsedCapabilities;

		// Token: 0x040003B2 RID: 946
		private XInputController.DeviceSubType m_SubType;

		// Token: 0x040003B3 RID: 947
		private XInputController.DeviceFlags m_Flags;

		// Token: 0x020001CA RID: 458
		internal enum DeviceType
		{
			// Token: 0x0400096B RID: 2411
			Gamepad
		}

		// Token: 0x020001CB RID: 459
		public enum DeviceSubType
		{
			// Token: 0x0400096D RID: 2413
			Unknown,
			// Token: 0x0400096E RID: 2414
			Gamepad,
			// Token: 0x0400096F RID: 2415
			Wheel,
			// Token: 0x04000970 RID: 2416
			ArcadeStick,
			// Token: 0x04000971 RID: 2417
			FlightStick,
			// Token: 0x04000972 RID: 2418
			DancePad,
			// Token: 0x04000973 RID: 2419
			Guitar,
			// Token: 0x04000974 RID: 2420
			GuitarAlternate,
			// Token: 0x04000975 RID: 2421
			DrumKit,
			// Token: 0x04000976 RID: 2422
			GuitarBass = 11,
			// Token: 0x04000977 RID: 2423
			ArcadePad = 19
		}

		// Token: 0x020001CC RID: 460
		[Flags]
		public new enum DeviceFlags
		{
			// Token: 0x04000979 RID: 2425
			ForceFeedbackSupported = 1,
			// Token: 0x0400097A RID: 2426
			Wireless = 2,
			// Token: 0x0400097B RID: 2427
			VoiceSupported = 4,
			// Token: 0x0400097C RID: 2428
			PluginModulesSupported = 8,
			// Token: 0x0400097D RID: 2429
			NoNavigation = 16
		}

		// Token: 0x020001CD RID: 461
		[Serializable]
		internal struct Capabilities
		{
			// Token: 0x0400097E RID: 2430
			public XInputController.DeviceType type;

			// Token: 0x0400097F RID: 2431
			public XInputController.DeviceSubType subType;

			// Token: 0x04000980 RID: 2432
			public XInputController.DeviceFlags flags;
		}
	}
}
