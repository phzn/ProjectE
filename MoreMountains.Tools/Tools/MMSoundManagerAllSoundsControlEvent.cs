using System;

namespace MoreMountains.Tools
{
	// Token: 0x020000C9 RID: 201
	public struct MMSoundManagerAllSoundsControlEvent
	{
		// Token: 0x06000446 RID: 1094 RVA: 0x0001409E File Offset: 0x0001229E
		public MMSoundManagerAllSoundsControlEvent(MMSoundManagerAllSoundsControlEventTypes eventType)
		{
			this.EventType = eventType;
		}

		// Token: 0x06000447 RID: 1095 RVA: 0x000140A7 File Offset: 0x000122A7
		public static void Trigger(MMSoundManagerAllSoundsControlEventTypes eventType)
		{
			MMSoundManagerAllSoundsControlEvent.e.EventType = eventType;
			MMEventManager.TriggerEvent<MMSoundManagerAllSoundsControlEvent>(MMSoundManagerAllSoundsControlEvent.e);
		}

		// Token: 0x0400059C RID: 1436
		public MMSoundManagerAllSoundsControlEventTypes EventType;

		// Token: 0x0400059D RID: 1437
		private static MMSoundManagerAllSoundsControlEvent e;
	}
}
