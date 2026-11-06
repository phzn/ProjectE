using System;

namespace Steamworks
{
	// Token: 0x020001B9 RID: 441
	[Serializable]
	public struct AppId_t : IEquatable<AppId_t>, IComparable<AppId_t>
	{
		// Token: 0x06000B18 RID: 2840 RVA: 0x0001098C File Offset: 0x0000EB8C
		public AppId_t(uint value)
		{
			this.m_AppId = value;
		}

		// Token: 0x06000B19 RID: 2841 RVA: 0x00010995 File Offset: 0x0000EB95
		public override string ToString()
		{
			return this.m_AppId.ToString();
		}

		// Token: 0x06000B1A RID: 2842 RVA: 0x000109A2 File Offset: 0x0000EBA2
		public override bool Equals(object other)
		{
			return other is AppId_t && this == (AppId_t)other;
		}

		// Token: 0x06000B1B RID: 2843 RVA: 0x000109BF File Offset: 0x0000EBBF
		public override int GetHashCode()
		{
			return this.m_AppId.GetHashCode();
		}

		// Token: 0x06000B1C RID: 2844 RVA: 0x000109CC File Offset: 0x0000EBCC
		public static bool operator ==(AppId_t x, AppId_t y)
		{
			return x.m_AppId == y.m_AppId;
		}

		// Token: 0x06000B1D RID: 2845 RVA: 0x000109DC File Offset: 0x0000EBDC
		public static bool operator !=(AppId_t x, AppId_t y)
		{
			return !(x == y);
		}

		// Token: 0x06000B1E RID: 2846 RVA: 0x000109E8 File Offset: 0x0000EBE8
		public static explicit operator AppId_t(uint value)
		{
			return new AppId_t(value);
		}

		// Token: 0x06000B1F RID: 2847 RVA: 0x000109F0 File Offset: 0x0000EBF0
		public static explicit operator uint(AppId_t that)
		{
			return that.m_AppId;
		}

		// Token: 0x06000B20 RID: 2848 RVA: 0x000109F8 File Offset: 0x0000EBF8
		public bool Equals(AppId_t other)
		{
			return this.m_AppId == other.m_AppId;
		}

		// Token: 0x06000B21 RID: 2849 RVA: 0x00010A08 File Offset: 0x0000EC08
		public int CompareTo(AppId_t other)
		{
			return this.m_AppId.CompareTo(other.m_AppId);
		}

		// Token: 0x04000BF2 RID: 3058
		public static readonly AppId_t Invalid = new AppId_t(0U);

		// Token: 0x04000BF3 RID: 3059
		public uint m_AppId;
	}
}
