using System;

namespace UnityEngine.UIElements
{
	// Token: 0x020002BA RID: 698
	internal interface IScheduler
	{
		// Token: 0x0600143A RID: 5178
		ScheduledItem ScheduleOnce(Action<TimerState> timerUpdateEvent, long delayMs);

		// Token: 0x0600143B RID: 5179
		ScheduledItem ScheduleUntil(Action<TimerState> timerUpdateEvent, long delayMs, long intervalMs, Func<bool> stopCondition = null);

		// Token: 0x0600143C RID: 5180
		ScheduledItem ScheduleForDuration(Action<TimerState> timerUpdateEvent, long delayMs, long intervalMs, long durationMs);

		// Token: 0x0600143D RID: 5181
		void Unschedule(ScheduledItem item);

		// Token: 0x0600143E RID: 5182
		void Schedule(ScheduledItem item);

		// Token: 0x0600143F RID: 5183
		void UpdateScheduledEvents();
	}
}
