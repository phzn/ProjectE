using System;

namespace Steamworks
{
	// Token: 0x020001B7 RID: 439
	[Serializable]
	public struct TimelineEventHandle_t : IEquatable<TimelineEventHandle_t>, IComparable<TimelineEventHandle_t>
	{
		// Token: 0x06000B03 RID: 2819 RVA: 0x00010861 File Offset: 0x0000EA61
		public TimelineEventHandle_t(ulong value)
		{
			this.m_TimelineEventHandle = value;
		}

		// Token: 0x06000B04 RID: 2820 RVA: 0x0001086A File Offset: 0x0000EA6A
		public override string ToString()
		{
			return this.m_TimelineEventHandle.ToString();
		}

		// Token: 0x06000B05 RID: 2821 RVA: 0x00010877 File Offset: 0x0000EA77
		public override bool Equals(object other)
		{
			return other is TimelineEventHandle_t && this == (TimelineEventHandle_t)other;
		}

		// Token: 0x06000B06 RID: 2822 RVA: 0x00010894 File Offset: 0x0000EA94
		public override int GetHashCode()
		{
			return this.m_TimelineEventHandle.GetHashCode();
		}

		// Token: 0x06000B07 RID: 2823 RVA: 0x000108A1 File Offset: 0x0000EAA1
		public static bool operator ==(TimelineEventHandle_t x, TimelineEventHandle_t y)
		{
			return x.m_TimelineEventHandle == y.m_TimelineEventHandle;
		}

		// Token: 0x06000B08 RID: 2824 RVA: 0x000108B1 File Offset: 0x0000EAB1
		public static bool operator !=(TimelineEventHandle_t x, TimelineEventHandle_t y)
		{
			return !(x == y);
		}

		// Token: 0x06000B09 RID: 2825 RVA: 0x000108BD File Offset: 0x0000EABD
		public static explicit operator TimelineEventHandle_t(ulong value)
		{
			return new TimelineEventHandle_t(value);
		}

		// Token: 0x06000B0A RID: 2826 RVA: 0x000108C5 File Offset: 0x0000EAC5
		public static explicit operator ulong(TimelineEventHandle_t that)
		{
			return that.m_TimelineEventHandle;
		}

		// Token: 0x06000B0B RID: 2827 RVA: 0x000108CD File Offset: 0x0000EACD
		public bool Equals(TimelineEventHandle_t other)
		{
			return this.m_TimelineEventHandle == other.m_TimelineEventHandle;
		}

		// Token: 0x06000B0C RID: 2828 RVA: 0x000108DD File Offset: 0x0000EADD
		public int CompareTo(TimelineEventHandle_t other)
		{
			return this.m_TimelineEventHandle.CompareTo(other.m_TimelineEventHandle);
		}

		// Token: 0x04000BEF RID: 3055
		public ulong m_TimelineEventHandle;
	}
}
