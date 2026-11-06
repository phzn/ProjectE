using System;

namespace MoreMountains.Tools
{
	// Token: 0x02000138 RID: 312
	public struct MMPersistenceEvent
	{
		// Token: 0x060006F4 RID: 1780 RVA: 0x000221BB File Offset: 0x000203BB
		public MMPersistenceEvent(MMPersistenceEventType eventType, string persistenceID)
		{
			this.PersistenceEventType = eventType;
			this.PersistenceID = persistenceID;
		}

		// Token: 0x060006F5 RID: 1781 RVA: 0x000221CB File Offset: 0x000203CB
		public static void Trigger(MMPersistenceEventType eventType, string persistencyID)
		{
			MMPersistenceEvent.e.PersistenceEventType = eventType;
			MMPersistenceEvent.e.PersistenceID = persistencyID;
			MMEventManager.TriggerEvent<MMPersistenceEvent>(MMPersistenceEvent.e);
		}

		// Token: 0x04000890 RID: 2192
		public MMPersistenceEventType PersistenceEventType;

		// Token: 0x04000891 RID: 2193
		public string PersistenceID;

		// Token: 0x04000892 RID: 2194
		private static MMPersistenceEvent e;
	}
}
