using System;
using UnityEngine;

namespace MoreMountains.Tools
{
	// Token: 0x0200002A RID: 42
	public struct MMDebugMenuButtonEvent
	{
		// Token: 0x14000002 RID: 2
		// (add) Token: 0x060000F4 RID: 244 RVA: 0x000057A8 File Offset: 0x000039A8
		// (remove) Token: 0x060000F5 RID: 245 RVA: 0x000057DC File Offset: 0x000039DC
		private static event MMDebugMenuButtonEvent.Delegate OnEvent;

		// Token: 0x060000F6 RID: 246 RVA: 0x0000580F File Offset: 0x00003A0F
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
		private static void RuntimeInitialization()
		{
			MMDebugMenuButtonEvent.OnEvent = null;
		}

		// Token: 0x060000F7 RID: 247 RVA: 0x00005817 File Offset: 0x00003A17
		public static void Register(MMDebugMenuButtonEvent.Delegate callback)
		{
			MMDebugMenuButtonEvent.OnEvent += callback;
		}

		// Token: 0x060000F8 RID: 248 RVA: 0x0000581F File Offset: 0x00003A1F
		public static void Unregister(MMDebugMenuButtonEvent.Delegate callback)
		{
			MMDebugMenuButtonEvent.OnEvent -= callback;
		}

		// Token: 0x060000F9 RID: 249 RVA: 0x00005827 File Offset: 0x00003A27
		public static void Trigger(string buttonEventName, bool active = true, MMDebugMenuButtonEvent.EventModes eventMode = MMDebugMenuButtonEvent.EventModes.FromButton)
		{
			MMDebugMenuButtonEvent.Delegate onEvent = MMDebugMenuButtonEvent.OnEvent;
			if (onEvent == null)
			{
				return;
			}
			onEvent(buttonEventName, active, eventMode);
		}

		// Token: 0x020003AA RID: 938
		public enum EventModes
		{
			// Token: 0x04001D76 RID: 7542
			FromButton,
			// Token: 0x04001D77 RID: 7543
			SetButton
		}

		// Token: 0x020003AB RID: 939
		// (Invoke) Token: 0x0600190A RID: 6410
		public delegate void Delegate(string buttonEventName, bool active = true, MMDebugMenuButtonEvent.EventModes eventMode = MMDebugMenuButtonEvent.EventModes.FromButton);
	}
}
