using System;
using System.Collections.Generic;

namespace UnityEngine.UIElements
{
	// Token: 0x020002BD RID: 701
	internal class TimerEventScheduler : IScheduler
	{
		// Token: 0x06001453 RID: 5203 RVA: 0x0004826C File Offset: 0x0004646C
		public void Schedule(ScheduledItem item)
		{
			bool flag = item == null;
			if (!flag)
			{
				bool flag2 = item == null;
				if (flag2)
				{
					throw new NotSupportedException("Scheduled Item type is not supported by this scheduler");
				}
				bool transactionMode = this.m_TransactionMode;
				if (transactionMode)
				{
					bool flag3 = this.m_UnscheduleTransactions.Remove(item);
					if (!flag3)
					{
						bool flag4 = this.m_ScheduledItems.Contains(item) || this.m_ScheduleTransactions.Contains(item);
						if (flag4)
						{
							throw new ArgumentException("Cannot schedule function " + item + " more than once");
						}
						this.m_ScheduleTransactions.Add(item);
					}
				}
				else
				{
					bool flag5 = this.m_ScheduledItems.Contains(item);
					if (flag5)
					{
						throw new ArgumentException("Cannot schedule function " + item + " more than once");
					}
					this.m_ScheduledItems.Add(item);
				}
			}
		}

		// Token: 0x06001454 RID: 5204 RVA: 0x00048344 File Offset: 0x00046544
		public ScheduledItem ScheduleOnce(Action<TimerState> timerUpdateEvent, long delayMs)
		{
			TimerEventScheduler.TimerEventSchedulerItem timerEventSchedulerItem = new TimerEventScheduler.TimerEventSchedulerItem(timerUpdateEvent)
			{
				delayMs = delayMs
			};
			this.Schedule(timerEventSchedulerItem);
			return timerEventSchedulerItem;
		}

		// Token: 0x06001455 RID: 5205 RVA: 0x00048370 File Offset: 0x00046570
		public ScheduledItem ScheduleUntil(Action<TimerState> timerUpdateEvent, long delayMs, long intervalMs, Func<bool> stopCondition)
		{
			TimerEventScheduler.TimerEventSchedulerItem timerEventSchedulerItem = new TimerEventScheduler.TimerEventSchedulerItem(timerUpdateEvent)
			{
				delayMs = delayMs,
				intervalMs = intervalMs,
				timerUpdateStopCondition = stopCondition
			};
			this.Schedule(timerEventSchedulerItem);
			return timerEventSchedulerItem;
		}

		// Token: 0x06001456 RID: 5206 RVA: 0x000483AC File Offset: 0x000465AC
		public ScheduledItem ScheduleForDuration(Action<TimerState> timerUpdateEvent, long delayMs, long intervalMs, long durationMs)
		{
			TimerEventScheduler.TimerEventSchedulerItem timerEventSchedulerItem = new TimerEventScheduler.TimerEventSchedulerItem(timerUpdateEvent)
			{
				delayMs = delayMs,
				intervalMs = intervalMs,
				timerUpdateStopCondition = null
			};
			timerEventSchedulerItem.SetDuration(durationMs);
			this.Schedule(timerEventSchedulerItem);
			return timerEventSchedulerItem;
		}

		// Token: 0x06001457 RID: 5207 RVA: 0x000483F0 File Offset: 0x000465F0
		private bool RemovedScheduledItemAt(int index)
		{
			bool flag = index >= 0;
			bool flag3;
			if (flag)
			{
				bool flag2 = index <= this.m_LastUpdatedIndex;
				if (flag2)
				{
					this.m_LastUpdatedIndex--;
				}
				this.m_ScheduledItems.RemoveAt(index);
				flag3 = true;
			}
			else
			{
				flag3 = false;
			}
			return flag3;
		}

		// Token: 0x06001458 RID: 5208 RVA: 0x00048440 File Offset: 0x00046640
		public void Unschedule(ScheduledItem item)
		{
			bool flag = item != null;
			if (flag)
			{
				bool transactionMode = this.m_TransactionMode;
				if (transactionMode)
				{
					bool flag2 = this.m_UnscheduleTransactions.Contains(item);
					if (flag2)
					{
						throw new ArgumentException("Cannot unschedule scheduled function twice" + ((item != null) ? item.ToString() : null));
					}
					bool flag3 = this.m_ScheduleTransactions.Remove(item);
					if (!flag3)
					{
						bool flag4 = this.m_ScheduledItems.Contains(item);
						if (!flag4)
						{
							throw new ArgumentException("Cannot unschedule unknown scheduled function " + ((item != null) ? item.ToString() : null));
						}
						this.m_UnscheduleTransactions.Add(item);
					}
				}
				else
				{
					bool flag5 = !this.PrivateUnSchedule(item);
					if (flag5)
					{
						throw new ArgumentException("Cannot unschedule unknown scheduled function " + ((item != null) ? item.ToString() : null));
					}
				}
				item.OnItemUnscheduled();
			}
		}

		// Token: 0x06001459 RID: 5209 RVA: 0x0004852C File Offset: 0x0004672C
		private bool PrivateUnSchedule(ScheduledItem sItem)
		{
			return this.m_ScheduleTransactions.Remove(sItem) || this.RemovedScheduledItemAt(this.m_ScheduledItems.IndexOf(sItem));
		}

		// Token: 0x0600145A RID: 5210 RVA: 0x00048564 File Offset: 0x00046764
		public void UpdateScheduledEvents()
		{
			try
			{
				this.m_TransactionMode = true;
				long num = Panel.TimeSinceStartupMs();
				int count = this.m_ScheduledItems.Count;
				int num2 = this.m_LastUpdatedIndex + 1;
				bool flag = num2 >= count;
				if (flag)
				{
					num2 = 0;
				}
				for (int i = 0; i < count; i++)
				{
					int num3 = num2 + i;
					bool flag2 = num3 >= count;
					if (flag2)
					{
						num3 -= count;
					}
					ScheduledItem scheduledItem = this.m_ScheduledItems[num3];
					bool flag3 = false;
					bool flag4 = num - scheduledItem.delayMs >= scheduledItem.startMs;
					if (flag4)
					{
						TimerState timerState = new TimerState
						{
							start = scheduledItem.startMs,
							now = num
						};
						bool flag5 = !this.m_UnscheduleTransactions.Contains(scheduledItem);
						if (flag5)
						{
							scheduledItem.PerformTimerUpdate(timerState);
						}
						scheduledItem.startMs = num;
						scheduledItem.delayMs = scheduledItem.intervalMs;
						bool flag6 = scheduledItem.ShouldUnschedule();
						if (flag6)
						{
							flag3 = true;
						}
					}
					bool flag7 = flag3 || (scheduledItem.endTimeMs > 0L && num > scheduledItem.endTimeMs);
					if (flag7)
					{
						bool flag8 = !this.m_UnscheduleTransactions.Contains(scheduledItem);
						if (flag8)
						{
							this.Unschedule(scheduledItem);
						}
					}
					this.m_LastUpdatedIndex = num3;
				}
			}
			finally
			{
				this.m_TransactionMode = false;
				foreach (ScheduledItem scheduledItem2 in this.m_UnscheduleTransactions)
				{
					this.PrivateUnSchedule(scheduledItem2);
				}
				this.m_UnscheduleTransactions.Clear();
				foreach (ScheduledItem scheduledItem3 in this.m_ScheduleTransactions)
				{
					this.Schedule(scheduledItem3);
				}
				this.m_ScheduleTransactions.Clear();
			}
		}

		// Token: 0x04000973 RID: 2419
		private readonly List<ScheduledItem> m_ScheduledItems = new List<ScheduledItem>();

		// Token: 0x04000974 RID: 2420
		private bool m_TransactionMode;

		// Token: 0x04000975 RID: 2421
		private readonly List<ScheduledItem> m_ScheduleTransactions = new List<ScheduledItem>();

		// Token: 0x04000976 RID: 2422
		private readonly HashSet<ScheduledItem> m_UnscheduleTransactions = new HashSet<ScheduledItem>();

		// Token: 0x04000977 RID: 2423
		internal bool disableThrottling = false;

		// Token: 0x04000978 RID: 2424
		private int m_LastUpdatedIndex = -1;

		// Token: 0x020002BE RID: 702
		private class TimerEventSchedulerItem : ScheduledItem
		{
			// Token: 0x0600145C RID: 5212 RVA: 0x000487F4 File Offset: 0x000469F4
			public TimerEventSchedulerItem(Action<TimerState> updateEvent)
			{
				this.m_TimerUpdateEvent = updateEvent;
			}

			// Token: 0x0600145D RID: 5213 RVA: 0x00048805 File Offset: 0x00046A05
			public override void PerformTimerUpdate(TimerState state)
			{
				Action<TimerState> timerUpdateEvent = this.m_TimerUpdateEvent;
				if (timerUpdateEvent != null)
				{
					timerUpdateEvent(state);
				}
			}

			// Token: 0x0600145E RID: 5214 RVA: 0x0004881C File Offset: 0x00046A1C
			public override string ToString()
			{
				return this.m_TimerUpdateEvent.ToString();
			}

			// Token: 0x04000979 RID: 2425
			private readonly Action<TimerState> m_TimerUpdateEvent;
		}
	}
}
