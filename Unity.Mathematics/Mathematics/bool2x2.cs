using System;
using System.Runtime.CompilerServices;
using Unity.IL2CPP.CompilerServices;

namespace Unity.Mathematics
{
	// Token: 0x02000007 RID: 7
	[Il2CppEagerStaticClassConstruction]
	[Serializable]
	public struct bool2x2 : IEquatable<bool2x2>
	{
		// Token: 0x060007FD RID: 2045 RVA: 0x0001C2F8 File Offset: 0x0001A4F8
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool2x2(bool2 c0, bool2 c1)
		{
			this.c0 = c0;
			this.c1 = c1;
		}

		// Token: 0x060007FE RID: 2046 RVA: 0x0001C308 File Offset: 0x0001A508
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool2x2(bool m00, bool m01, bool m10, bool m11)
		{
			this.c0 = new bool2(m00, m10);
			this.c1 = new bool2(m01, m11);
		}

		// Token: 0x060007FF RID: 2047 RVA: 0x0001C325 File Offset: 0x0001A525
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool2x2(bool v)
		{
			this.c0 = v;
			this.c1 = v;
		}

		// Token: 0x06000800 RID: 2048 RVA: 0x0001C33F File Offset: 0x0001A53F
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static implicit operator bool2x2(bool v)
		{
			return new bool2x2(v);
		}

		// Token: 0x06000801 RID: 2049 RVA: 0x0001C347 File Offset: 0x0001A547
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool2x2 operator ==(bool2x2 lhs, bool2x2 rhs)
		{
			return new bool2x2(lhs.c0 == rhs.c0, lhs.c1 == rhs.c1);
		}

		// Token: 0x06000802 RID: 2050 RVA: 0x0001C370 File Offset: 0x0001A570
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool2x2 operator ==(bool2x2 lhs, bool rhs)
		{
			return new bool2x2(lhs.c0 == rhs, lhs.c1 == rhs);
		}

		// Token: 0x06000803 RID: 2051 RVA: 0x0001C38F File Offset: 0x0001A58F
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool2x2 operator ==(bool lhs, bool2x2 rhs)
		{
			return new bool2x2(lhs == rhs.c0, lhs == rhs.c1);
		}

		// Token: 0x06000804 RID: 2052 RVA: 0x0001C3AE File Offset: 0x0001A5AE
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool2x2 operator !=(bool2x2 lhs, bool2x2 rhs)
		{
			return new bool2x2(lhs.c0 != rhs.c0, lhs.c1 != rhs.c1);
		}

		// Token: 0x06000805 RID: 2053 RVA: 0x0001C3D7 File Offset: 0x0001A5D7
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool2x2 operator !=(bool2x2 lhs, bool rhs)
		{
			return new bool2x2(lhs.c0 != rhs, lhs.c1 != rhs);
		}

		// Token: 0x06000806 RID: 2054 RVA: 0x0001C3F6 File Offset: 0x0001A5F6
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool2x2 operator !=(bool lhs, bool2x2 rhs)
		{
			return new bool2x2(lhs != rhs.c0, lhs != rhs.c1);
		}

		// Token: 0x06000807 RID: 2055 RVA: 0x0001C415 File Offset: 0x0001A615
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool2x2 operator !(bool2x2 val)
		{
			return new bool2x2(!val.c0, !val.c1);
		}

		// Token: 0x06000808 RID: 2056 RVA: 0x0001C432 File Offset: 0x0001A632
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool2x2 operator &(bool2x2 lhs, bool2x2 rhs)
		{
			return new bool2x2(lhs.c0 & rhs.c0, lhs.c1 & rhs.c1);
		}

		// Token: 0x06000809 RID: 2057 RVA: 0x0001C45B File Offset: 0x0001A65B
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool2x2 operator &(bool2x2 lhs, bool rhs)
		{
			return new bool2x2(lhs.c0 & rhs, lhs.c1 & rhs);
		}

		// Token: 0x0600080A RID: 2058 RVA: 0x0001C47A File Offset: 0x0001A67A
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool2x2 operator &(bool lhs, bool2x2 rhs)
		{
			return new bool2x2(lhs & rhs.c0, lhs & rhs.c1);
		}

		// Token: 0x0600080B RID: 2059 RVA: 0x0001C499 File Offset: 0x0001A699
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool2x2 operator |(bool2x2 lhs, bool2x2 rhs)
		{
			return new bool2x2(lhs.c0 | rhs.c0, lhs.c1 | rhs.c1);
		}

		// Token: 0x0600080C RID: 2060 RVA: 0x0001C4C2 File Offset: 0x0001A6C2
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool2x2 operator |(bool2x2 lhs, bool rhs)
		{
			return new bool2x2(lhs.c0 | rhs, lhs.c1 | rhs);
		}

		// Token: 0x0600080D RID: 2061 RVA: 0x0001C4E1 File Offset: 0x0001A6E1
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool2x2 operator |(bool lhs, bool2x2 rhs)
		{
			return new bool2x2(lhs | rhs.c0, lhs | rhs.c1);
		}

		// Token: 0x0600080E RID: 2062 RVA: 0x0001C500 File Offset: 0x0001A700
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool2x2 operator ^(bool2x2 lhs, bool2x2 rhs)
		{
			return new bool2x2(lhs.c0 ^ rhs.c0, lhs.c1 ^ rhs.c1);
		}

		// Token: 0x0600080F RID: 2063 RVA: 0x0001C529 File Offset: 0x0001A729
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool2x2 operator ^(bool2x2 lhs, bool rhs)
		{
			return new bool2x2(lhs.c0 ^ rhs, lhs.c1 ^ rhs);
		}

		// Token: 0x06000810 RID: 2064 RVA: 0x0001C548 File Offset: 0x0001A748
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool2x2 operator ^(bool lhs, bool2x2 rhs)
		{
			return new bool2x2(lhs ^ rhs.c0, lhs ^ rhs.c1);
		}

		// Token: 0x1700001E RID: 30
		public unsafe ref bool2 this[int index]
		{
			get
			{
				fixed (bool2x2* ptr = &this)
				{
					return ref *(bool2*)(ptr + (IntPtr)index * (IntPtr)sizeof(bool2) / (IntPtr)sizeof(bool2x2));
				}
			}
		}

		// Token: 0x06000812 RID: 2066 RVA: 0x0001C583 File Offset: 0x0001A783
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool Equals(bool2x2 rhs)
		{
			return this.c0.Equals(rhs.c0) && this.c1.Equals(rhs.c1);
		}

		// Token: 0x06000813 RID: 2067 RVA: 0x0001C5AC File Offset: 0x0001A7AC
		public override bool Equals(object o)
		{
			if (o is bool2x2)
			{
				bool2x2 bool2x = (bool2x2)o;
				return this.Equals(bool2x);
			}
			return false;
		}

		// Token: 0x06000814 RID: 2068 RVA: 0x0001C5D1 File Offset: 0x0001A7D1
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public override int GetHashCode()
		{
			return (int)math.hash(this);
		}

		// Token: 0x06000815 RID: 2069 RVA: 0x0001C5E0 File Offset: 0x0001A7E0
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public override string ToString()
		{
			return string.Format("bool2x2({0}, {1},  {2}, {3})", new object[]
			{
				this.c0.x,
				this.c1.x,
				this.c0.y,
				this.c1.y
			});
		}

		// Token: 0x04000027 RID: 39
		public bool2 c0;

		// Token: 0x04000028 RID: 40
		public bool2 c1;
	}
}
