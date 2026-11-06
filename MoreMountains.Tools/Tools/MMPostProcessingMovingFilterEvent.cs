using System;
using UnityEngine;

namespace MoreMountains.Tools
{
	// Token: 0x02000003 RID: 3
	public struct MMPostProcessingMovingFilterEvent
	{
		// Token: 0x14000001 RID: 1
		// (add) Token: 0x06000003 RID: 3 RVA: 0x000020C8 File Offset: 0x000002C8
		// (remove) Token: 0x06000004 RID: 4 RVA: 0x000020FC File Offset: 0x000002FC
		private static event MMPostProcessingMovingFilterEvent.Delegate OnEvent;

		// Token: 0x06000005 RID: 5 RVA: 0x0000212F File Offset: 0x0000032F
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
		private static void RuntimeInitialization()
		{
			MMPostProcessingMovingFilterEvent.OnEvent = null;
		}

		// Token: 0x06000006 RID: 6 RVA: 0x00002137 File Offset: 0x00000337
		public static void Register(MMPostProcessingMovingFilterEvent.Delegate callback)
		{
			MMPostProcessingMovingFilterEvent.OnEvent += callback;
		}

		// Token: 0x06000007 RID: 7 RVA: 0x0000213F File Offset: 0x0000033F
		public static void Unregister(MMPostProcessingMovingFilterEvent.Delegate callback)
		{
			MMPostProcessingMovingFilterEvent.OnEvent -= callback;
		}

		// Token: 0x06000008 RID: 8 RVA: 0x00002147 File Offset: 0x00000347
		public static void Trigger(MMTweenType curve, bool active, bool toggle, float duration, int channel = 0, bool stop = false, bool restore = false)
		{
			MMPostProcessingMovingFilterEvent.Delegate onEvent = MMPostProcessingMovingFilterEvent.OnEvent;
			if (onEvent == null)
			{
				return;
			}
			onEvent(curve, active, toggle, duration, channel, stop, restore);
		}

		// Token: 0x02000395 RID: 917
		// (Invoke) Token: 0x060018EE RID: 6382
		public delegate void Delegate(MMTweenType curve, bool active, bool toggle, float duration, int channel = 0, bool stop = false, bool restore = false);
	}
}
