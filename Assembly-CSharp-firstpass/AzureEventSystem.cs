using System;
using System.Collections.Generic;
using UnityEngine.Events;

namespace UnityEngine.AzureSky
{
	// Token: 0x02000013 RID: 19
	[Serializable]
	public sealed class AzureEventSystem
	{
		// Token: 0x1700001C RID: 28
		// (get) Token: 0x06000047 RID: 71 RVA: 0x000028FE File Offset: 0x00000AFE
		public UnityEvent onMinuteChangeEvent
		{
			get
			{
				return this.m_onMinuteChangeEvent;
			}
		}

		// Token: 0x1700001D RID: 29
		// (get) Token: 0x06000048 RID: 72 RVA: 0x00002906 File Offset: 0x00000B06
		public UnityEvent onHourChangeEvent
		{
			get
			{
				return this.m_onHourChangeEvent;
			}
		}

		// Token: 0x1700001E RID: 30
		// (get) Token: 0x06000049 RID: 73 RVA: 0x0000290E File Offset: 0x00000B0E
		public UnityEvent onDayChangeEvent
		{
			get
			{
				return this.m_onDayChangeEvent;
			}
		}

		// Token: 0x1700001F RID: 31
		// (get) Token: 0x0600004A RID: 74 RVA: 0x00002916 File Offset: 0x00000B16
		public UnityEvent onMonthChangeEvent
		{
			get
			{
				return this.m_onMonthChangeEvent;
			}
		}

		// Token: 0x17000020 RID: 32
		// (get) Token: 0x0600004B RID: 75 RVA: 0x0000291E File Offset: 0x00000B1E
		public UnityEvent onYearChangeEvent
		{
			get
			{
				return this.m_onYearChangeEvent;
			}
		}

		// Token: 0x17000021 RID: 33
		// (get) Token: 0x0600004C RID: 76 RVA: 0x00002926 File Offset: 0x00000B26
		// (set) Token: 0x0600004D RID: 77 RVA: 0x0000292E File Offset: 0x00000B2E
		public AzureCustomEventUpdateMode customEventScanMode
		{
			get
			{
				return this.m_customEventScanMode;
			}
			set
			{
				this.m_customEventScanMode = value;
			}
		}

		// Token: 0x17000022 RID: 34
		// (get) Token: 0x0600004E RID: 78 RVA: 0x00002937 File Offset: 0x00000B37
		public List<AzureCustomEvent> customEventList
		{
			get
			{
				return this.m_customEventList;
			}
		}

		// Token: 0x0600004F RID: 79 RVA: 0x00002940 File Offset: 0x00000B40
		public void RegisterEvents()
		{
			AzureNotificationCenter.OnMinuteChanged += this.OnMinuteChange;
			AzureNotificationCenter.OnHourChanged += this.OnHourChange;
			AzureNotificationCenter.OnDayChanged += this.OnDayChange;
			AzureNotificationCenter.OnMonthChanged += this.OnMonthChange;
			AzureNotificationCenter.OnYearChanged += this.OnYearChange;
		}

		// Token: 0x06000050 RID: 80 RVA: 0x000029A4 File Offset: 0x00000BA4
		public void UnregisterEvents()
		{
			AzureNotificationCenter.OnMinuteChanged -= this.OnMinuteChange;
			AzureNotificationCenter.OnHourChanged -= this.OnHourChange;
			AzureNotificationCenter.OnDayChanged -= this.OnDayChange;
			AzureNotificationCenter.OnMonthChanged -= this.OnMonthChange;
			AzureNotificationCenter.OnYearChanged -= this.OnYearChange;
		}

		// Token: 0x06000051 RID: 81 RVA: 0x00002A06 File Offset: 0x00000C06
		private void OnMinuteChange(AzureTimeSystem timeSystem)
		{
			UnityEvent onMinuteChangeEvent = this.m_onMinuteChangeEvent;
			if (onMinuteChangeEvent != null)
			{
				onMinuteChangeEvent.Invoke();
			}
			if (this.m_customEventScanMode == AzureCustomEventUpdateMode.ByMinute)
			{
				this.ScanCustomEventList(timeSystem);
			}
		}

		// Token: 0x06000052 RID: 82 RVA: 0x00002A28 File Offset: 0x00000C28
		private void OnHourChange(AzureTimeSystem timeSystem)
		{
			UnityEvent onHourChangeEvent = this.m_onHourChangeEvent;
			if (onHourChangeEvent != null)
			{
				onHourChangeEvent.Invoke();
			}
			if (this.m_customEventScanMode == AzureCustomEventUpdateMode.ByHour)
			{
				this.ScanCustomEventList(timeSystem);
			}
		}

		// Token: 0x06000053 RID: 83 RVA: 0x00002A4B File Offset: 0x00000C4B
		private void OnDayChange(AzureTimeSystem timeSystem)
		{
			UnityEvent onDayChangeEvent = this.m_onDayChangeEvent;
			if (onDayChangeEvent == null)
			{
				return;
			}
			onDayChangeEvent.Invoke();
		}

		// Token: 0x06000054 RID: 84 RVA: 0x00002A5D File Offset: 0x00000C5D
		private void OnMonthChange(AzureTimeSystem timeSystem)
		{
			UnityEvent onMonthChangeEvent = this.m_onMonthChangeEvent;
			if (onMonthChangeEvent == null)
			{
				return;
			}
			onMonthChangeEvent.Invoke();
		}

		// Token: 0x06000055 RID: 85 RVA: 0x00002A6F File Offset: 0x00000C6F
		private void OnYearChange(AzureTimeSystem timeSystem)
		{
			UnityEvent onYearChangeEvent = this.m_onYearChangeEvent;
			if (onYearChangeEvent == null)
			{
				return;
			}
			onYearChangeEvent.Invoke();
		}

		// Token: 0x06000056 RID: 86 RVA: 0x00002A84 File Offset: 0x00000C84
		private void ScanCustomEventList(AzureTimeSystem timeSystem)
		{
			for (int i = 0; i < this.m_customEventList.Count; i++)
			{
				if (this.m_customEventList[i].eventListenersCount > 0 && (this.m_customEventList[i].year == timeSystem.year || this.m_customEventList[i].year == -1) && (this.m_customEventList[i].month == timeSystem.month || this.m_customEventList[i].month == -1) && (this.m_customEventList[i].day == timeSystem.day || this.m_customEventList[i].day == -1))
				{
					if (timeSystem.hour != this.m_customEventList[i].executedHour)
					{
						this.m_customEventList[i].isAlreadyExecutedOnThisHour = false;
					}
					if (this.m_customEventList[i].hour == timeSystem.hour || this.m_customEventList[i].hour == -1)
					{
						if (this.m_customEventList[i].minute == -1)
						{
							this.m_customEventList[i].Invoke();
						}
						else if (!this.m_customEventList[i].isAlreadyExecutedOnThisHour && timeSystem.minute >= this.m_customEventList[i].minute)
						{
							this.m_customEventList[i].executedHour = timeSystem.hour;
							this.m_customEventList[i].isAlreadyExecutedOnThisHour = true;
							this.m_customEventList[i].Invoke();
						}
					}
				}
			}
		}

		// Token: 0x04000058 RID: 88
		[SerializeField]
		private UnityEvent m_onMinuteChangeEvent;

		// Token: 0x04000059 RID: 89
		[SerializeField]
		private UnityEvent m_onHourChangeEvent;

		// Token: 0x0400005A RID: 90
		[SerializeField]
		private UnityEvent m_onDayChangeEvent;

		// Token: 0x0400005B RID: 91
		[SerializeField]
		private UnityEvent m_onMonthChangeEvent;

		// Token: 0x0400005C RID: 92
		[SerializeField]
		private UnityEvent m_onYearChangeEvent;

		// Token: 0x0400005D RID: 93
		[SerializeField]
		private AzureCustomEventUpdateMode m_customEventScanMode;

		// Token: 0x0400005E RID: 94
		[SerializeField]
		private List<AzureCustomEvent> m_customEventList = new List<AzureCustomEvent>();
	}
}
