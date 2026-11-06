using System;

namespace Steamworks
{
	// Token: 0x02000195 RID: 405
	[Serializable]
	public struct InputAnalogActionHandle_t : IEquatable<InputAnalogActionHandle_t>, IComparable<InputAnalogActionHandle_t>
	{
		// Token: 0x060009DC RID: 2524 RVA: 0x0000F816 File Offset: 0x0000DA16
		public InputAnalogActionHandle_t(ulong value)
		{
			this.m_InputAnalogActionHandle = value;
		}

		// Token: 0x060009DD RID: 2525 RVA: 0x0000F81F File Offset: 0x0000DA1F
		public override string ToString()
		{
			return this.m_InputAnalogActionHandle.ToString();
		}

		// Token: 0x060009DE RID: 2526 RVA: 0x0000F82C File Offset: 0x0000DA2C
		public override bool Equals(object other)
		{
			return other is InputAnalogActionHandle_t && this == (InputAnalogActionHandle_t)other;
		}

		// Token: 0x060009DF RID: 2527 RVA: 0x0000F849 File Offset: 0x0000DA49
		public override int GetHashCode()
		{
			return this.m_InputAnalogActionHandle.GetHashCode();
		}

		// Token: 0x060009E0 RID: 2528 RVA: 0x0000F856 File Offset: 0x0000DA56
		public static bool operator ==(InputAnalogActionHandle_t x, InputAnalogActionHandle_t y)
		{
			return x.m_InputAnalogActionHandle == y.m_InputAnalogActionHandle;
		}

		// Token: 0x060009E1 RID: 2529 RVA: 0x0000F866 File Offset: 0x0000DA66
		public static bool operator !=(InputAnalogActionHandle_t x, InputAnalogActionHandle_t y)
		{
			return !(x == y);
		}

		// Token: 0x060009E2 RID: 2530 RVA: 0x0000F872 File Offset: 0x0000DA72
		public static explicit operator InputAnalogActionHandle_t(ulong value)
		{
			return new InputAnalogActionHandle_t(value);
		}

		// Token: 0x060009E3 RID: 2531 RVA: 0x0000F87A File Offset: 0x0000DA7A
		public static explicit operator ulong(InputAnalogActionHandle_t that)
		{
			return that.m_InputAnalogActionHandle;
		}

		// Token: 0x060009E4 RID: 2532 RVA: 0x0000F882 File Offset: 0x0000DA82
		public bool Equals(InputAnalogActionHandle_t other)
		{
			return this.m_InputAnalogActionHandle == other.m_InputAnalogActionHandle;
		}

		// Token: 0x060009E5 RID: 2533 RVA: 0x0000F892 File Offset: 0x0000DA92
		public int CompareTo(InputAnalogActionHandle_t other)
		{
			return this.m_InputAnalogActionHandle.CompareTo(other.m_InputAnalogActionHandle);
		}

		// Token: 0x04000B8A RID: 2954
		public ulong m_InputAnalogActionHandle;
	}
}
