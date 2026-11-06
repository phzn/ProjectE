using System;
using UnityEngine.Events;

namespace UnityEngine.AzureSky
{
	// Token: 0x02000006 RID: 6
	[Serializable]
	public sealed class AzureCustomEvent
	{
		// Token: 0x17000013 RID: 19
		// (get) Token: 0x06000034 RID: 52 RVA: 0x00002848 File Offset: 0x00000A48
		// (set) Token: 0x06000035 RID: 53 RVA: 0x00002850 File Offset: 0x00000A50
		public int minute
		{
			get
			{
				return this.m_minute;
			}
			set
			{
				this.m_minute = value;
			}
		}

		// Token: 0x17000014 RID: 20
		// (get) Token: 0x06000036 RID: 54 RVA: 0x00002859 File Offset: 0x00000A59
		// (set) Token: 0x06000037 RID: 55 RVA: 0x00002861 File Offset: 0x00000A61
		public int hour
		{
			get
			{
				return this.m_hour;
			}
			set
			{
				this.m_hour = value;
			}
		}

		// Token: 0x17000015 RID: 21
		// (get) Token: 0x06000038 RID: 56 RVA: 0x0000286A File Offset: 0x00000A6A
		// (set) Token: 0x06000039 RID: 57 RVA: 0x00002872 File Offset: 0x00000A72
		public int day
		{
			get
			{
				return this.m_day;
			}
			set
			{
				this.m_day = value;
			}
		}

		// Token: 0x17000016 RID: 22
		// (get) Token: 0x0600003A RID: 58 RVA: 0x0000287B File Offset: 0x00000A7B
		// (set) Token: 0x0600003B RID: 59 RVA: 0x00002883 File Offset: 0x00000A83
		public int month
		{
			get
			{
				return this.m_month;
			}
			set
			{
				this.m_month = value;
			}
		}

		// Token: 0x17000017 RID: 23
		// (get) Token: 0x0600003C RID: 60 RVA: 0x0000288C File Offset: 0x00000A8C
		// (set) Token: 0x0600003D RID: 61 RVA: 0x00002894 File Offset: 0x00000A94
		public int year
		{
			get
			{
				return this.m_year;
			}
			set
			{
				this.m_year = value;
			}
		}

		// Token: 0x17000018 RID: 24
		// (get) Token: 0x0600003E RID: 62 RVA: 0x0000289D File Offset: 0x00000A9D
		// (set) Token: 0x0600003F RID: 63 RVA: 0x000028A5 File Offset: 0x00000AA5
		public int executedHour
		{
			get
			{
				return this.m_executedHour;
			}
			set
			{
				this.m_executedHour = value;
			}
		}

		// Token: 0x17000019 RID: 25
		// (get) Token: 0x06000040 RID: 64 RVA: 0x000028AE File Offset: 0x00000AAE
		// (set) Token: 0x06000041 RID: 65 RVA: 0x000028B6 File Offset: 0x00000AB6
		public bool isAlreadyExecutedOnThisHour
		{
			get
			{
				return this.m_isAlreadyExecutedOnThisHour;
			}
			set
			{
				this.m_isAlreadyExecutedOnThisHour = value;
			}
		}

		// Token: 0x1700001A RID: 26
		// (get) Token: 0x06000042 RID: 66 RVA: 0x000028BF File Offset: 0x00000ABF
		public int eventListenersCount
		{
			get
			{
				return this.m_unityEvent.GetPersistentEventCount();
			}
		}

		// Token: 0x1700001B RID: 27
		// (get) Token: 0x06000043 RID: 67 RVA: 0x000028CC File Offset: 0x00000ACC
		// (set) Token: 0x06000044 RID: 68 RVA: 0x000028D4 File Offset: 0x00000AD4
		public UnityEvent unityEvent
		{
			get
			{
				return this.m_unityEvent;
			}
			set
			{
				this.m_unityEvent = value;
			}
		}

		// Token: 0x06000045 RID: 69 RVA: 0x000028DD File Offset: 0x00000ADD
		public void Invoke()
		{
			UnityEvent unityEvent = this.m_unityEvent;
			if (unityEvent == null)
			{
				return;
			}
			unityEvent.Invoke();
		}

		// Token: 0x0400001C RID: 28
		[SerializeField]
		private int m_minute;

		// Token: 0x0400001D RID: 29
		[SerializeField]
		private int m_hour;

		// Token: 0x0400001E RID: 30
		[SerializeField]
		private int m_day;

		// Token: 0x0400001F RID: 31
		[SerializeField]
		private int m_month;

		// Token: 0x04000020 RID: 32
		[SerializeField]
		private int m_year;

		// Token: 0x04000021 RID: 33
		private int m_executedHour = -1;

		// Token: 0x04000022 RID: 34
		private bool m_isAlreadyExecutedOnThisHour;

		// Token: 0x04000023 RID: 35
		[SerializeField]
		private UnityEvent m_unityEvent;
	}
}
