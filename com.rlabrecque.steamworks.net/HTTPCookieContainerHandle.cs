using System;

namespace Steamworks
{
	// Token: 0x02000192 RID: 402
	[Serializable]
	public struct HTTPCookieContainerHandle : IEquatable<HTTPCookieContainerHandle>, IComparable<HTTPCookieContainerHandle>
	{
		// Token: 0x060009BC RID: 2492 RVA: 0x0000F64F File Offset: 0x0000D84F
		public HTTPCookieContainerHandle(uint value)
		{
			this.m_HTTPCookieContainerHandle = value;
		}

		// Token: 0x060009BD RID: 2493 RVA: 0x0000F658 File Offset: 0x0000D858
		public override string ToString()
		{
			return this.m_HTTPCookieContainerHandle.ToString();
		}

		// Token: 0x060009BE RID: 2494 RVA: 0x0000F665 File Offset: 0x0000D865
		public override bool Equals(object other)
		{
			return other is HTTPCookieContainerHandle && this == (HTTPCookieContainerHandle)other;
		}

		// Token: 0x060009BF RID: 2495 RVA: 0x0000F682 File Offset: 0x0000D882
		public override int GetHashCode()
		{
			return this.m_HTTPCookieContainerHandle.GetHashCode();
		}

		// Token: 0x060009C0 RID: 2496 RVA: 0x0000F68F File Offset: 0x0000D88F
		public static bool operator ==(HTTPCookieContainerHandle x, HTTPCookieContainerHandle y)
		{
			return x.m_HTTPCookieContainerHandle == y.m_HTTPCookieContainerHandle;
		}

		// Token: 0x060009C1 RID: 2497 RVA: 0x0000F69F File Offset: 0x0000D89F
		public static bool operator !=(HTTPCookieContainerHandle x, HTTPCookieContainerHandle y)
		{
			return !(x == y);
		}

		// Token: 0x060009C2 RID: 2498 RVA: 0x0000F6AB File Offset: 0x0000D8AB
		public static explicit operator HTTPCookieContainerHandle(uint value)
		{
			return new HTTPCookieContainerHandle(value);
		}

		// Token: 0x060009C3 RID: 2499 RVA: 0x0000F6B3 File Offset: 0x0000D8B3
		public static explicit operator uint(HTTPCookieContainerHandle that)
		{
			return that.m_HTTPCookieContainerHandle;
		}

		// Token: 0x060009C4 RID: 2500 RVA: 0x0000F6BB File Offset: 0x0000D8BB
		public bool Equals(HTTPCookieContainerHandle other)
		{
			return this.m_HTTPCookieContainerHandle == other.m_HTTPCookieContainerHandle;
		}

		// Token: 0x060009C5 RID: 2501 RVA: 0x0000F6CB File Offset: 0x0000D8CB
		public int CompareTo(HTTPCookieContainerHandle other)
		{
			return this.m_HTTPCookieContainerHandle.CompareTo(other.m_HTTPCookieContainerHandle);
		}

		// Token: 0x04000B85 RID: 2949
		public static readonly HTTPCookieContainerHandle Invalid = new HTTPCookieContainerHandle(0U);

		// Token: 0x04000B86 RID: 2950
		public uint m_HTTPCookieContainerHandle;
	}
}
