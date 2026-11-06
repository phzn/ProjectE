using System;

namespace Steamworks
{
	// Token: 0x02000194 RID: 404
	[Serializable]
	public struct InputActionSetHandle_t : IEquatable<InputActionSetHandle_t>, IComparable<InputActionSetHandle_t>
	{
		// Token: 0x060009D2 RID: 2514 RVA: 0x0000F787 File Offset: 0x0000D987
		public InputActionSetHandle_t(ulong value)
		{
			this.m_InputActionSetHandle = value;
		}

		// Token: 0x060009D3 RID: 2515 RVA: 0x0000F790 File Offset: 0x0000D990
		public override string ToString()
		{
			return this.m_InputActionSetHandle.ToString();
		}

		// Token: 0x060009D4 RID: 2516 RVA: 0x0000F79D File Offset: 0x0000D99D
		public override bool Equals(object other)
		{
			return other is InputActionSetHandle_t && this == (InputActionSetHandle_t)other;
		}

		// Token: 0x060009D5 RID: 2517 RVA: 0x0000F7BA File Offset: 0x0000D9BA
		public override int GetHashCode()
		{
			return this.m_InputActionSetHandle.GetHashCode();
		}

		// Token: 0x060009D6 RID: 2518 RVA: 0x0000F7C7 File Offset: 0x0000D9C7
		public static bool operator ==(InputActionSetHandle_t x, InputActionSetHandle_t y)
		{
			return x.m_InputActionSetHandle == y.m_InputActionSetHandle;
		}

		// Token: 0x060009D7 RID: 2519 RVA: 0x0000F7D7 File Offset: 0x0000D9D7
		public static bool operator !=(InputActionSetHandle_t x, InputActionSetHandle_t y)
		{
			return !(x == y);
		}

		// Token: 0x060009D8 RID: 2520 RVA: 0x0000F7E3 File Offset: 0x0000D9E3
		public static explicit operator InputActionSetHandle_t(ulong value)
		{
			return new InputActionSetHandle_t(value);
		}

		// Token: 0x060009D9 RID: 2521 RVA: 0x0000F7EB File Offset: 0x0000D9EB
		public static explicit operator ulong(InputActionSetHandle_t that)
		{
			return that.m_InputActionSetHandle;
		}

		// Token: 0x060009DA RID: 2522 RVA: 0x0000F7F3 File Offset: 0x0000D9F3
		public bool Equals(InputActionSetHandle_t other)
		{
			return this.m_InputActionSetHandle == other.m_InputActionSetHandle;
		}

		// Token: 0x060009DB RID: 2523 RVA: 0x0000F803 File Offset: 0x0000DA03
		public int CompareTo(InputActionSetHandle_t other)
		{
			return this.m_InputActionSetHandle.CompareTo(other.m_InputActionSetHandle);
		}

		// Token: 0x04000B89 RID: 2953
		public ulong m_InputActionSetHandle;
	}
}
