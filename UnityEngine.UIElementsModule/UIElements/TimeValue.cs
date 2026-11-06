using System;
using System.Globalization;

namespace UnityEngine.UIElements
{
	// Token: 0x02000311 RID: 785
	public struct TimeValue : IEquatable<TimeValue>
	{
		// Token: 0x17000686 RID: 1670
		// (get) Token: 0x06001B17 RID: 6935 RVA: 0x0006AA5C File Offset: 0x00068C5C
		// (set) Token: 0x06001B18 RID: 6936 RVA: 0x0006AA64 File Offset: 0x00068C64
		public float value
		{
			get
			{
				return this.m_Value;
			}
			set
			{
				this.m_Value = value;
			}
		}

		// Token: 0x17000687 RID: 1671
		// (get) Token: 0x06001B19 RID: 6937 RVA: 0x0006AA6D File Offset: 0x00068C6D
		// (set) Token: 0x06001B1A RID: 6938 RVA: 0x0006AA75 File Offset: 0x00068C75
		public TimeUnit unit
		{
			get
			{
				return this.m_Unit;
			}
			set
			{
				this.m_Unit = value;
			}
		}

		// Token: 0x06001B1B RID: 6939 RVA: 0x0006AA7E File Offset: 0x00068C7E
		public TimeValue(float value)
		{
			this = new TimeValue(value, TimeUnit.Second);
		}

		// Token: 0x06001B1C RID: 6940 RVA: 0x0006AA8A File Offset: 0x00068C8A
		public TimeValue(float value, TimeUnit unit)
		{
			this.m_Value = value;
			this.m_Unit = unit;
		}

		// Token: 0x06001B1D RID: 6941 RVA: 0x0006AA9C File Offset: 0x00068C9C
		public static implicit operator TimeValue(float value)
		{
			return new TimeValue(value, TimeUnit.Second);
		}

		// Token: 0x06001B1E RID: 6942 RVA: 0x0006AAB8 File Offset: 0x00068CB8
		public static bool operator ==(TimeValue lhs, TimeValue rhs)
		{
			return lhs.m_Value == rhs.m_Value && lhs.m_Unit == rhs.m_Unit;
		}

		// Token: 0x06001B1F RID: 6943 RVA: 0x0006AAEC File Offset: 0x00068CEC
		public static bool operator !=(TimeValue lhs, TimeValue rhs)
		{
			return !(lhs == rhs);
		}

		// Token: 0x06001B20 RID: 6944 RVA: 0x0006AB08 File Offset: 0x00068D08
		public bool Equals(TimeValue other)
		{
			return other == this;
		}

		// Token: 0x06001B21 RID: 6945 RVA: 0x0006AB28 File Offset: 0x00068D28
		public override bool Equals(object obj)
		{
			bool flag;
			if (obj is TimeValue)
			{
				TimeValue timeValue = (TimeValue)obj;
				flag = this.Equals(timeValue);
			}
			else
			{
				flag = false;
			}
			return flag;
		}

		// Token: 0x06001B22 RID: 6946 RVA: 0x0006AB54 File Offset: 0x00068D54
		public override int GetHashCode()
		{
			return (this.m_Value.GetHashCode() * 397) ^ (int)this.m_Unit;
		}

		// Token: 0x06001B23 RID: 6947 RVA: 0x0006AB80 File Offset: 0x00068D80
		public override string ToString()
		{
			string text = this.value.ToString(CultureInfo.InvariantCulture.NumberFormat);
			string text2 = string.Empty;
			TimeUnit unit = this.unit;
			TimeUnit timeUnit = unit;
			if (timeUnit != TimeUnit.Second)
			{
				if (timeUnit == TimeUnit.Millisecond)
				{
					text2 = "ms";
				}
			}
			else
			{
				text2 = "s";
			}
			return text + text2;
		}

		// Token: 0x04000B09 RID: 2825
		private float m_Value;

		// Token: 0x04000B0A RID: 2826
		private TimeUnit m_Unit;
	}
}
