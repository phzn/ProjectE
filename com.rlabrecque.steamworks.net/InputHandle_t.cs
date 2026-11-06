using System;

namespace Steamworks
{
	// Token: 0x02000197 RID: 407
	[Serializable]
	public struct InputHandle_t : IEquatable<InputHandle_t>, IComparable<InputHandle_t>
	{
		// Token: 0x060009F0 RID: 2544 RVA: 0x0000F934 File Offset: 0x0000DB34
		public InputHandle_t(ulong value)
		{
			this.m_InputHandle = value;
		}

		// Token: 0x060009F1 RID: 2545 RVA: 0x0000F93D File Offset: 0x0000DB3D
		public override string ToString()
		{
			return this.m_InputHandle.ToString();
		}

		// Token: 0x060009F2 RID: 2546 RVA: 0x0000F94A File Offset: 0x0000DB4A
		public override bool Equals(object other)
		{
			return other is InputHandle_t && this == (InputHandle_t)other;
		}

		// Token: 0x060009F3 RID: 2547 RVA: 0x0000F967 File Offset: 0x0000DB67
		public override int GetHashCode()
		{
			return this.m_InputHandle.GetHashCode();
		}

		// Token: 0x060009F4 RID: 2548 RVA: 0x0000F974 File Offset: 0x0000DB74
		public static bool operator ==(InputHandle_t x, InputHandle_t y)
		{
			return x.m_InputHandle == y.m_InputHandle;
		}

		// Token: 0x060009F5 RID: 2549 RVA: 0x0000F984 File Offset: 0x0000DB84
		public static bool operator !=(InputHandle_t x, InputHandle_t y)
		{
			return !(x == y);
		}

		// Token: 0x060009F6 RID: 2550 RVA: 0x0000F990 File Offset: 0x0000DB90
		public static explicit operator InputHandle_t(ulong value)
		{
			return new InputHandle_t(value);
		}

		// Token: 0x060009F7 RID: 2551 RVA: 0x0000F998 File Offset: 0x0000DB98
		public static explicit operator ulong(InputHandle_t that)
		{
			return that.m_InputHandle;
		}

		// Token: 0x060009F8 RID: 2552 RVA: 0x0000F9A0 File Offset: 0x0000DBA0
		public bool Equals(InputHandle_t other)
		{
			return this.m_InputHandle == other.m_InputHandle;
		}

		// Token: 0x060009F9 RID: 2553 RVA: 0x0000F9B0 File Offset: 0x0000DBB0
		public int CompareTo(InputHandle_t other)
		{
			return this.m_InputHandle.CompareTo(other.m_InputHandle);
		}

		// Token: 0x04000B8C RID: 2956
		public ulong m_InputHandle;
	}
}
