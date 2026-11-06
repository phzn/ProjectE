using System;

namespace UnityEngine.UIElements
{
	// Token: 0x020002BB RID: 699
	internal abstract class ScheduledItem
	{
		// Token: 0x17000441 RID: 1089
		// (get) Token: 0x06001440 RID: 5184 RVA: 0x00048180 File Offset: 0x00046380
		// (set) Token: 0x06001441 RID: 5185 RVA: 0x00048188 File Offset: 0x00046388
		public long startMs { get; set; }

		// Token: 0x17000442 RID: 1090
		// (get) Token: 0x06001442 RID: 5186 RVA: 0x00048191 File Offset: 0x00046391
		// (set) Token: 0x06001443 RID: 5187 RVA: 0x00048199 File Offset: 0x00046399
		public long delayMs { get; set; }

		// Token: 0x17000443 RID: 1091
		// (get) Token: 0x06001444 RID: 5188 RVA: 0x000481A2 File Offset: 0x000463A2
		// (set) Token: 0x06001445 RID: 5189 RVA: 0x000481AA File Offset: 0x000463AA
		public long intervalMs { get; set; }

		// Token: 0x17000444 RID: 1092
		// (get) Token: 0x06001446 RID: 5190 RVA: 0x000481B3 File Offset: 0x000463B3
		// (set) Token: 0x06001447 RID: 5191 RVA: 0x000481BB File Offset: 0x000463BB
		public long endTimeMs { get; private set; }

		// Token: 0x06001448 RID: 5192 RVA: 0x000481C4 File Offset: 0x000463C4
		public ScheduledItem()
		{
			this.ResetStartTime();
			this.timerUpdateStopCondition = ScheduledItem.OnceCondition;
		}

		// Token: 0x06001449 RID: 5193 RVA: 0x000481E0 File Offset: 0x000463E0
		protected void ResetStartTime()
		{
			this.startMs = Panel.TimeSinceStartupMs();
		}

		// Token: 0x0600144A RID: 5194 RVA: 0x000481EF File Offset: 0x000463EF
		public void SetDuration(long durationMs)
		{
			this.endTimeMs = this.startMs + durationMs;
		}

		// Token: 0x0600144B RID: 5195
		public abstract void PerformTimerUpdate(TimerState state);

		// Token: 0x0600144C RID: 5196 RVA: 0x00003CD2 File Offset: 0x00001ED2
		internal virtual void OnItemUnscheduled()
		{
		}

		// Token: 0x0600144D RID: 5197 RVA: 0x00048204 File Offset: 0x00046404
		public virtual bool ShouldUnschedule()
		{
			bool flag = this.timerUpdateStopCondition != null;
			return flag && this.timerUpdateStopCondition();
		}

		// Token: 0x0400096B RID: 2411
		public Func<bool> timerUpdateStopCondition;

		// Token: 0x0400096C RID: 2412
		public static readonly Func<bool> OnceCondition = () => true;

		// Token: 0x0400096D RID: 2413
		public static readonly Func<bool> ForeverCondition = () => false;
	}
}
