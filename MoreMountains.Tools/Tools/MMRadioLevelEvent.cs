using System;
using UnityEngine;

namespace MoreMountains.Tools
{
	// Token: 0x02000128 RID: 296
	public struct MMRadioLevelEvent
	{
		// Token: 0x14000013 RID: 19
		// (add) Token: 0x060006A2 RID: 1698 RVA: 0x000218BC File Offset: 0x0001FABC
		// (remove) Token: 0x060006A3 RID: 1699 RVA: 0x000218F0 File Offset: 0x0001FAF0
		private static event MMRadioLevelEvent.Delegate OnEvent;

		// Token: 0x060006A4 RID: 1700 RVA: 0x00021923 File Offset: 0x0001FB23
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
		private static void RuntimeInitialization()
		{
			MMRadioLevelEvent.OnEvent = null;
		}

		// Token: 0x060006A5 RID: 1701 RVA: 0x0002192B File Offset: 0x0001FB2B
		public static void Register(MMRadioLevelEvent.Delegate callback)
		{
			MMRadioLevelEvent.OnEvent += callback;
		}

		// Token: 0x060006A6 RID: 1702 RVA: 0x00021933 File Offset: 0x0001FB33
		public static void Unregister(MMRadioLevelEvent.Delegate callback)
		{
			MMRadioLevelEvent.OnEvent -= callback;
		}

		// Token: 0x060006A7 RID: 1703 RVA: 0x0002193B File Offset: 0x0001FB3B
		public static void Trigger(int channel, float level)
		{
			MMRadioLevelEvent.Delegate onEvent = MMRadioLevelEvent.OnEvent;
			if (onEvent == null)
			{
				return;
			}
			onEvent(channel, level);
		}

		// Token: 0x02000441 RID: 1089
		// (Invoke) Token: 0x06001A32 RID: 6706
		public delegate void Delegate(int channel, float level);
	}
}
