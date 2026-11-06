using System;

namespace MoreMountains.Tools
{
	// Token: 0x0200015F RID: 351
	public struct MMFadeStopEvent
	{
		// Token: 0x0600081E RID: 2078 RVA: 0x0002660F File Offset: 0x0002480F
		public MMFadeStopEvent(int id = 0, bool restore = false)
		{
			this.Restore = restore;
			this.ID = id;
		}

		// Token: 0x0600081F RID: 2079 RVA: 0x0002661F File Offset: 0x0002481F
		public static void Trigger(int id = 0, bool restore = false)
		{
			MMFadeStopEvent.e.ID = id;
			MMFadeStopEvent.e.Restore = restore;
			MMEventManager.TriggerEvent<MMFadeStopEvent>(MMFadeStopEvent.e);
		}

		// Token: 0x04000981 RID: 2433
		public int ID;

		// Token: 0x04000982 RID: 2434
		public bool Restore;

		// Token: 0x04000983 RID: 2435
		private static MMFadeStopEvent e;
	}
}
