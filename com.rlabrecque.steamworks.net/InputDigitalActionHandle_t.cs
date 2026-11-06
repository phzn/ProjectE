using System;

namespace Steamworks
{
	// Token: 0x02000196 RID: 406
	[Serializable]
	public struct InputDigitalActionHandle_t : IEquatable<InputDigitalActionHandle_t>, IComparable<InputDigitalActionHandle_t>
	{
		// Token: 0x060009E6 RID: 2534 RVA: 0x0000F8A5 File Offset: 0x0000DAA5
		public InputDigitalActionHandle_t(ulong value)
		{
			this.m_InputDigitalActionHandle = value;
		}

		// Token: 0x060009E7 RID: 2535 RVA: 0x0000F8AE File Offset: 0x0000DAAE
		public override string ToString()
		{
			return this.m_InputDigitalActionHandle.ToString();
		}

		// Token: 0x060009E8 RID: 2536 RVA: 0x0000F8BB File Offset: 0x0000DABB
		public override bool Equals(object other)
		{
			return other is InputDigitalActionHandle_t && this == (InputDigitalActionHandle_t)other;
		}

		// Token: 0x060009E9 RID: 2537 RVA: 0x0000F8D8 File Offset: 0x0000DAD8
		public override int GetHashCode()
		{
			return this.m_InputDigitalActionHandle.GetHashCode();
		}

		// Token: 0x060009EA RID: 2538 RVA: 0x0000F8E5 File Offset: 0x0000DAE5
		public static bool operator ==(InputDigitalActionHandle_t x, InputDigitalActionHandle_t y)
		{
			return x.m_InputDigitalActionHandle == y.m_InputDigitalActionHandle;
		}

		// Token: 0x060009EB RID: 2539 RVA: 0x0000F8F5 File Offset: 0x0000DAF5
		public static bool operator !=(InputDigitalActionHandle_t x, InputDigitalActionHandle_t y)
		{
			return !(x == y);
		}

		// Token: 0x060009EC RID: 2540 RVA: 0x0000F901 File Offset: 0x0000DB01
		public static explicit operator InputDigitalActionHandle_t(ulong value)
		{
			return new InputDigitalActionHandle_t(value);
		}

		// Token: 0x060009ED RID: 2541 RVA: 0x0000F909 File Offset: 0x0000DB09
		public static explicit operator ulong(InputDigitalActionHandle_t that)
		{
			return that.m_InputDigitalActionHandle;
		}

		// Token: 0x060009EE RID: 2542 RVA: 0x0000F911 File Offset: 0x0000DB11
		public bool Equals(InputDigitalActionHandle_t other)
		{
			return this.m_InputDigitalActionHandle == other.m_InputDigitalActionHandle;
		}

		// Token: 0x060009EF RID: 2543 RVA: 0x0000F921 File Offset: 0x0000DB21
		public int CompareTo(InputDigitalActionHandle_t other)
		{
			return this.m_InputDigitalActionHandle.CompareTo(other.m_InputDigitalActionHandle);
		}

		// Token: 0x04000B8B RID: 2955
		public ulong m_InputDigitalActionHandle;
	}
}
