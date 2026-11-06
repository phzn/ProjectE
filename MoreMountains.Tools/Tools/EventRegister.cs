using System;

namespace MoreMountains.Tools
{
	// Token: 0x020000DC RID: 220
	public static class EventRegister
	{
		// Token: 0x060004C2 RID: 1218 RVA: 0x00016672 File Offset: 0x00014872
		public static void MMEventStartListening<EventType>(this MMEventListener<EventType> caller) where EventType : struct
		{
			MMEventManager.AddListener<EventType>(caller);
		}

		// Token: 0x060004C3 RID: 1219 RVA: 0x0001667A File Offset: 0x0001487A
		public static void MMEventStopListening<EventType>(this MMEventListener<EventType> caller) where EventType : struct
		{
			MMEventManager.RemoveListener<EventType>(caller);
		}

		// Token: 0x0200041B RID: 1051
		// (Invoke) Token: 0x060019B9 RID: 6585
		public delegate void Delegate<T>(T eventType);
	}
}
