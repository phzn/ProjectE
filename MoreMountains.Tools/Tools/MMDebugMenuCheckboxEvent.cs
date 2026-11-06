using System;
using UnityEngine;

namespace MoreMountains.Tools
{
	// Token: 0x0200002D RID: 45
	public struct MMDebugMenuCheckboxEvent
	{
		// Token: 0x14000003 RID: 3
		// (add) Token: 0x06000100 RID: 256 RVA: 0x000058C0 File Offset: 0x00003AC0
		// (remove) Token: 0x06000101 RID: 257 RVA: 0x000058F4 File Offset: 0x00003AF4
		private static event MMDebugMenuCheckboxEvent.Delegate OnEvent;

		// Token: 0x06000102 RID: 258 RVA: 0x00005927 File Offset: 0x00003B27
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
		private static void RuntimeInitialization()
		{
			MMDebugMenuCheckboxEvent.OnEvent = null;
		}

		// Token: 0x06000103 RID: 259 RVA: 0x0000592F File Offset: 0x00003B2F
		public static void Register(MMDebugMenuCheckboxEvent.Delegate callback)
		{
			MMDebugMenuCheckboxEvent.OnEvent += callback;
		}

		// Token: 0x06000104 RID: 260 RVA: 0x00005937 File Offset: 0x00003B37
		public static void Unregister(MMDebugMenuCheckboxEvent.Delegate callback)
		{
			MMDebugMenuCheckboxEvent.OnEvent -= callback;
		}

		// Token: 0x06000105 RID: 261 RVA: 0x0000593F File Offset: 0x00003B3F
		public static void Trigger(string checkboxEventName, bool value, MMDebugMenuCheckboxEvent.EventModes eventMode = MMDebugMenuCheckboxEvent.EventModes.FromCheckbox)
		{
			MMDebugMenuCheckboxEvent.Delegate onEvent = MMDebugMenuCheckboxEvent.OnEvent;
			if (onEvent == null)
			{
				return;
			}
			onEvent(checkboxEventName, value, eventMode);
		}

		// Token: 0x020003AC RID: 940
		public enum EventModes
		{
			// Token: 0x04001D79 RID: 7545
			FromCheckbox,
			// Token: 0x04001D7A RID: 7546
			SetCheckbox
		}

		// Token: 0x020003AD RID: 941
		// (Invoke) Token: 0x0600190E RID: 6414
		public delegate void Delegate(string checkboxEventName, bool value, MMDebugMenuCheckboxEvent.EventModes eventMode = MMDebugMenuCheckboxEvent.EventModes.FromCheckbox);
	}
}
