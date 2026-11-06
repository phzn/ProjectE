using System;

namespace MoreMountains.Tools
{
	// Token: 0x020000CB RID: 203
	public struct MMSoundManagerEvent
	{
		// Token: 0x06000448 RID: 1096 RVA: 0x000140BE File Offset: 0x000122BE
		public MMSoundManagerEvent(MMSoundManagerEventTypes eventType)
		{
			this.EventType = eventType;
		}

		// Token: 0x06000449 RID: 1097 RVA: 0x000140C7 File Offset: 0x000122C7
		public static void Trigger(MMSoundManagerEventTypes eventType)
		{
			MMSoundManagerEvent.e.EventType = eventType;
			MMEventManager.TriggerEvent<MMSoundManagerEvent>(MMSoundManagerEvent.e);
		}

		// Token: 0x040005A3 RID: 1443
		public MMSoundManagerEventTypes EventType;

		// Token: 0x040005A4 RID: 1444
		private static MMSoundManagerEvent e;
	}
}
