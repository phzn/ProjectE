using System;

namespace Steamworks
{
	// Token: 0x02000193 RID: 403
	[Serializable]
	public struct HTTPRequestHandle : IEquatable<HTTPRequestHandle>, IComparable<HTTPRequestHandle>
	{
		// Token: 0x060009C7 RID: 2503 RVA: 0x0000F6EB File Offset: 0x0000D8EB
		public HTTPRequestHandle(uint value)
		{
			this.m_HTTPRequestHandle = value;
		}

		// Token: 0x060009C8 RID: 2504 RVA: 0x0000F6F4 File Offset: 0x0000D8F4
		public override string ToString()
		{
			return this.m_HTTPRequestHandle.ToString();
		}

		// Token: 0x060009C9 RID: 2505 RVA: 0x0000F701 File Offset: 0x0000D901
		public override bool Equals(object other)
		{
			return other is HTTPRequestHandle && this == (HTTPRequestHandle)other;
		}

		// Token: 0x060009CA RID: 2506 RVA: 0x0000F71E File Offset: 0x0000D91E
		public override int GetHashCode()
		{
			return this.m_HTTPRequestHandle.GetHashCode();
		}

		// Token: 0x060009CB RID: 2507 RVA: 0x0000F72B File Offset: 0x0000D92B
		public static bool operator ==(HTTPRequestHandle x, HTTPRequestHandle y)
		{
			return x.m_HTTPRequestHandle == y.m_HTTPRequestHandle;
		}

		// Token: 0x060009CC RID: 2508 RVA: 0x0000F73B File Offset: 0x0000D93B
		public static bool operator !=(HTTPRequestHandle x, HTTPRequestHandle y)
		{
			return !(x == y);
		}

		// Token: 0x060009CD RID: 2509 RVA: 0x0000F747 File Offset: 0x0000D947
		public static explicit operator HTTPRequestHandle(uint value)
		{
			return new HTTPRequestHandle(value);
		}

		// Token: 0x060009CE RID: 2510 RVA: 0x0000F74F File Offset: 0x0000D94F
		public static explicit operator uint(HTTPRequestHandle that)
		{
			return that.m_HTTPRequestHandle;
		}

		// Token: 0x060009CF RID: 2511 RVA: 0x0000F757 File Offset: 0x0000D957
		public bool Equals(HTTPRequestHandle other)
		{
			return this.m_HTTPRequestHandle == other.m_HTTPRequestHandle;
		}

		// Token: 0x060009D0 RID: 2512 RVA: 0x0000F767 File Offset: 0x0000D967
		public int CompareTo(HTTPRequestHandle other)
		{
			return this.m_HTTPRequestHandle.CompareTo(other.m_HTTPRequestHandle);
		}

		// Token: 0x04000B87 RID: 2951
		public static readonly HTTPRequestHandle Invalid = new HTTPRequestHandle(0U);

		// Token: 0x04000B88 RID: 2952
		public uint m_HTTPRequestHandle;
	}
}
