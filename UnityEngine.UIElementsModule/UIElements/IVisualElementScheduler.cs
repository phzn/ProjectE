using System;

namespace UnityEngine.UIElements
{
	// Token: 0x02000409 RID: 1033
	public interface IVisualElementScheduler
	{
		// Token: 0x0600210B RID: 8459
		IVisualElementScheduledItem Execute(Action<TimerState> timerUpdateEvent);

		// Token: 0x0600210C RID: 8460
		IVisualElementScheduledItem Execute(Action updateEvent);
	}
}
