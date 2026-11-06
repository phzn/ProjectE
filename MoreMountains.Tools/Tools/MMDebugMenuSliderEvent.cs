using System;
using UnityEngine;

namespace MoreMountains.Tools
{
	// Token: 0x02000032 RID: 50
	public struct MMDebugMenuSliderEvent
	{
		// Token: 0x14000004 RID: 4
		// (add) Token: 0x0600010E RID: 270 RVA: 0x00005A20 File Offset: 0x00003C20
		// (remove) Token: 0x0600010F RID: 271 RVA: 0x00005A54 File Offset: 0x00003C54
		private static event MMDebugMenuSliderEvent.Delegate OnEvent;

		// Token: 0x06000110 RID: 272 RVA: 0x00005A87 File Offset: 0x00003C87
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
		private static void RuntimeInitialization()
		{
			MMDebugMenuSliderEvent.OnEvent = null;
		}

		// Token: 0x06000111 RID: 273 RVA: 0x00005A8F File Offset: 0x00003C8F
		public static void Register(MMDebugMenuSliderEvent.Delegate callback)
		{
			MMDebugMenuSliderEvent.OnEvent += callback;
		}

		// Token: 0x06000112 RID: 274 RVA: 0x00005A97 File Offset: 0x00003C97
		public static void Unregister(MMDebugMenuSliderEvent.Delegate callback)
		{
			MMDebugMenuSliderEvent.OnEvent -= callback;
		}

		// Token: 0x06000113 RID: 275 RVA: 0x00005A9F File Offset: 0x00003C9F
		public static void Trigger(string sliderEventName, float value, MMDebugMenuSliderEvent.EventModes eventMode = MMDebugMenuSliderEvent.EventModes.FromSlider)
		{
			MMDebugMenuSliderEvent.Delegate onEvent = MMDebugMenuSliderEvent.OnEvent;
			if (onEvent == null)
			{
				return;
			}
			onEvent(sliderEventName, value, eventMode);
		}

		// Token: 0x020003AE RID: 942
		public enum EventModes
		{
			// Token: 0x04001D7C RID: 7548
			FromSlider,
			// Token: 0x04001D7D RID: 7549
			SetSlider
		}

		// Token: 0x020003AF RID: 943
		// (Invoke) Token: 0x06001912 RID: 6418
		public delegate void Delegate(string sliderEventName, float value, MMDebugMenuSliderEvent.EventModes eventMode = MMDebugMenuSliderEvent.EventModes.FromSlider);
	}
}
