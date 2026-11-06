using System;
using System.Collections.Generic;
using UnityEngine.Playables;

namespace UnityEngine.Timeline
{
	// Token: 0x0200003A RID: 58
	public class TimeNotificationBehaviour : PlayableBehaviour
	{
		// Token: 0x170000B8 RID: 184
		// (set) Token: 0x060002A1 RID: 673 RVA: 0x000095CE File Offset: 0x000077CE
		public Playable timeSource
		{
			set
			{
				this.m_TimeSource = value;
			}
		}

		// Token: 0x060002A2 RID: 674 RVA: 0x000095D7 File Offset: 0x000077D7
		public static ScriptPlayable<TimeNotificationBehaviour> Create(PlayableGraph graph, double duration, DirectorWrapMode loopMode)
		{
			ScriptPlayable<TimeNotificationBehaviour> scriptPlayable = ScriptPlayable<TimeNotificationBehaviour>.Create(graph, 0);
			scriptPlayable.SetDuration(duration);
			scriptPlayable.SetTimeWrapMode(loopMode);
			scriptPlayable.SetPropagateSetTime(true);
			return scriptPlayable;
		}

		// Token: 0x060002A3 RID: 675 RVA: 0x000095F8 File Offset: 0x000077F8
		public void AddNotification(double time, INotification payload, NotificationFlags flags = NotificationFlags.Retroactive)
		{
			this.m_Notifications.Add(new TimeNotificationBehaviour.NotificationEntry
			{
				time = time,
				payload = payload,
				flags = flags
			});
			this.m_NeedSortNotifications = true;
		}

		// Token: 0x060002A4 RID: 676 RVA: 0x00009638 File Offset: 0x00007838
		public override void OnGraphStart(Playable playable)
		{
			this.SortNotifications();
			double time = playable.GetTime<Playable>();
			for (int i = 0; i < this.m_Notifications.Count; i++)
			{
				if (this.m_Notifications[i].time > time && !this.m_Notifications[i].triggerOnce)
				{
					TimeNotificationBehaviour.NotificationEntry notificationEntry = this.m_Notifications[i];
					notificationEntry.notificationFired = false;
					this.m_Notifications[i] = notificationEntry;
				}
			}
			this.m_PreviousTime = playable.GetTime<Playable>();
		}

		// Token: 0x060002A5 RID: 677 RVA: 0x000096C0 File Offset: 0x000078C0
		public override void OnBehaviourPause(Playable playable, FrameData info)
		{
			if (playable.IsDone<Playable>())
			{
				this.SortNotifications();
				for (int i = 0; i < this.m_Notifications.Count; i++)
				{
					TimeNotificationBehaviour.NotificationEntry notificationEntry = this.m_Notifications[i];
					if (!notificationEntry.notificationFired)
					{
						double duration = playable.GetDuration<Playable>();
						if (this.m_PreviousTime <= notificationEntry.time && notificationEntry.time <= duration)
						{
							TimeNotificationBehaviour.Trigger_internal(playable, info.output, ref notificationEntry);
							this.m_Notifications[i] = notificationEntry;
						}
					}
				}
			}
		}

		// Token: 0x060002A6 RID: 678 RVA: 0x00009748 File Offset: 0x00007948
		public override void PrepareFrame(Playable playable, FrameData info)
		{
			if (info.evaluationType == FrameData.EvaluationType.Evaluate)
			{
				return;
			}
			this.SyncDurationWithExternalSource(playable);
			this.SortNotifications();
			double time = playable.GetTime<Playable>();
			if (info.timeLooped)
			{
				double duration = playable.GetDuration<Playable>();
				this.TriggerNotificationsInRange(this.m_PreviousTime, duration, info, playable, true);
				double num = playable.GetDuration<Playable>() - this.m_PreviousTime;
				int num2 = (int)(((double)(info.deltaTime * info.effectiveSpeed) - num) / playable.GetDuration<Playable>());
				for (int i = 0; i < num2; i++)
				{
					this.TriggerNotificationsInRange(0.0, duration, info, playable, false);
				}
				this.TriggerNotificationsInRange(0.0, time, info, playable, false);
			}
			else
			{
				double time2 = playable.GetTime<Playable>();
				this.TriggerNotificationsInRange(this.m_PreviousTime, time2, info, playable, true);
			}
			for (int j = 0; j < this.m_Notifications.Count; j++)
			{
				TimeNotificationBehaviour.NotificationEntry notificationEntry = this.m_Notifications[j];
				if (notificationEntry.notificationFired && TimeNotificationBehaviour.CanRestoreNotification(notificationEntry, info, time, this.m_PreviousTime))
				{
					TimeNotificationBehaviour.Restore_internal(ref notificationEntry);
					this.m_Notifications[j] = notificationEntry;
				}
			}
			this.m_PreviousTime = playable.GetTime<Playable>();
		}

		// Token: 0x060002A7 RID: 679 RVA: 0x00009872 File Offset: 0x00007A72
		private void SortNotifications()
		{
			if (this.m_NeedSortNotifications)
			{
				this.m_Notifications.Sort((TimeNotificationBehaviour.NotificationEntry x, TimeNotificationBehaviour.NotificationEntry y) => x.time.CompareTo(y.time));
				this.m_NeedSortNotifications = false;
			}
		}

		// Token: 0x060002A8 RID: 680 RVA: 0x000098AD File Offset: 0x00007AAD
		private static bool CanRestoreNotification(TimeNotificationBehaviour.NotificationEntry e, FrameData info, double currentTime, double previousTime)
		{
			return !e.triggerOnce && (info.timeLooped || (previousTime > currentTime && currentTime <= e.time));
		}

		// Token: 0x060002A9 RID: 681 RVA: 0x000098D8 File Offset: 0x00007AD8
		private void TriggerNotificationsInRange(double start, double end, FrameData info, Playable playable, bool checkState)
		{
			if (start <= end)
			{
				bool isPlaying = Application.isPlaying;
				for (int i = 0; i < this.m_Notifications.Count; i++)
				{
					TimeNotificationBehaviour.NotificationEntry notificationEntry = this.m_Notifications[i];
					if (!notificationEntry.notificationFired || (!checkState && !notificationEntry.triggerOnce))
					{
						double time = notificationEntry.time;
						if (notificationEntry.prewarm && time < end && (notificationEntry.triggerInEditor || isPlaying))
						{
							TimeNotificationBehaviour.Trigger_internal(playable, info.output, ref notificationEntry);
							this.m_Notifications[i] = notificationEntry;
						}
						else if (time >= start && time <= end && (notificationEntry.triggerInEditor || isPlaying))
						{
							TimeNotificationBehaviour.Trigger_internal(playable, info.output, ref notificationEntry);
							this.m_Notifications[i] = notificationEntry;
						}
					}
				}
			}
		}

		// Token: 0x060002AA RID: 682 RVA: 0x0000999E File Offset: 0x00007B9E
		private void SyncDurationWithExternalSource(Playable playable)
		{
			if (this.m_TimeSource.IsValid<Playable>())
			{
				playable.SetDuration(this.m_TimeSource.GetDuration<Playable>());
				playable.SetTimeWrapMode(this.m_TimeSource.GetTimeWrapMode<Playable>());
			}
		}

		// Token: 0x060002AB RID: 683 RVA: 0x000099CF File Offset: 0x00007BCF
		private static void Trigger_internal(Playable playable, PlayableOutput output, ref TimeNotificationBehaviour.NotificationEntry e)
		{
			output.PushNotification(playable, e.payload, null);
			e.notificationFired = true;
		}

		// Token: 0x060002AC RID: 684 RVA: 0x000099E6 File Offset: 0x00007BE6
		private static void Restore_internal(ref TimeNotificationBehaviour.NotificationEntry e)
		{
			e.notificationFired = false;
		}

		// Token: 0x040000E3 RID: 227
		private readonly List<TimeNotificationBehaviour.NotificationEntry> m_Notifications = new List<TimeNotificationBehaviour.NotificationEntry>();

		// Token: 0x040000E4 RID: 228
		private double m_PreviousTime;

		// Token: 0x040000E5 RID: 229
		private bool m_NeedSortNotifications;

		// Token: 0x040000E6 RID: 230
		private Playable m_TimeSource;

		// Token: 0x02000076 RID: 118
		private struct NotificationEntry
		{
			// Token: 0x170000CF RID: 207
			// (get) Token: 0x06000364 RID: 868 RVA: 0x0000BC58 File Offset: 0x00009E58
			public bool triggerInEditor
			{
				get
				{
					return (this.flags & NotificationFlags.TriggerInEditMode) > (NotificationFlags)0;
				}
			}

			// Token: 0x170000D0 RID: 208
			// (get) Token: 0x06000365 RID: 869 RVA: 0x0000BC65 File Offset: 0x00009E65
			public bool prewarm
			{
				get
				{
					return (this.flags & NotificationFlags.Retroactive) > (NotificationFlags)0;
				}
			}

			// Token: 0x170000D1 RID: 209
			// (get) Token: 0x06000366 RID: 870 RVA: 0x0000BC72 File Offset: 0x00009E72
			public bool triggerOnce
			{
				get
				{
					return (this.flags & NotificationFlags.TriggerOnce) > (NotificationFlags)0;
				}
			}

			// Token: 0x04000177 RID: 375
			public double time;

			// Token: 0x04000178 RID: 376
			public INotification payload;

			// Token: 0x04000179 RID: 377
			public bool notificationFired;

			// Token: 0x0400017A RID: 378
			public NotificationFlags flags;
		}
	}
}
