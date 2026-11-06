using System;
using System.Runtime.CompilerServices;
using Unity.IL2CPP.CompilerServices;

namespace Unity.Mathematics
{
	// Token: 0x02000008 RID: 8
	[Il2CppEagerStaticClassConstruction]
	[Serializable]
	public struct bool2x3 : IEquatable<bool2x3>
	{
		// Token: 0x06000816 RID: 2070 RVA: 0x0001C649 File Offset: 0x0001A849
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool2x3(bool2 c0, bool2 c1, bool2 c2)
		{
			this.c0 = c0;
			this.c1 = c1;
			this.c2 = c2;
		}

		// Token: 0x06000817 RID: 2071 RVA: 0x0001C660 File Offset: 0x0001A860
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool2x3(bool m00, bool m01, bool m02, bool m10, bool m11, bool m12)
		{
			this.c0 = new bool2(m00, m10);
			this.c1 = new bool2(m01, m11);
			this.c2 = new bool2(m02, m12);
		}

		// Token: 0x06000818 RID: 2072 RVA: 0x0001C68C File Offset: 0x0001A88C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool2x3(bool v)
		{
			this.c0 = v;
			this.c1 = v;
			this.c2 = v;
		}

		// Token: 0x06000819 RID: 2073 RVA: 0x0001C6B2 File Offset: 0x0001A8B2
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static implicit operator bool2x3(bool v)
		{
			return new bool2x3(v);
		}

		// Token: 0x0600081A RID: 2074 RVA: 0x0001C6BA File Offset: 0x0001A8BA
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool2x3 operator ==(bool2x3 lhs, bool2x3 rhs)
		{
			return new bool2x3(lhs.c0 == rhs.c0, lhs.c1 == rhs.c1, lhs.c2 == rhs.c2);
		}

		// Token: 0x0600081B RID: 2075 RVA: 0x0001C6F4 File Offset: 0x0001A8F4
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool2x3 operator ==(bool2x3 lhs, bool rhs)
		{
			return new bool2x3(lhs.c0 == rhs, lhs.c1 == rhs, lhs.c2 == rhs);
		}

		// Token: 0x0600081C RID: 2076 RVA: 0x0001C71F File Offset: 0x0001A91F
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool2x3 operator ==(bool lhs, bool2x3 rhs)
		{
			return new bool2x3(lhs == rhs.c0, lhs == rhs.c1, lhs == rhs.c2);
		}

		// Token: 0x0600081D RID: 2077 RVA: 0x0001C74A File Offset: 0x0001A94A
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool2x3 operator !=(bool2x3 lhs, bool2x3 rhs)
		{
			return new bool2x3(lhs.c0 != rhs.c0, lhs.c1 != rhs.c1, lhs.c2 != rhs.c2);
		}

		// Token: 0x0600081E RID: 2078 RVA: 0x0001C784 File Offset: 0x0001A984
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool2x3 operator !=(bool2x3 lhs, bool rhs)
		{
			return new bool2x3(lhs.c0 != rhs, lhs.c1 != rhs, lhs.c2 != rhs);
		}

		// Token: 0x0600081F RID: 2079 RVA: 0x0001C7AF File Offset: 0x0001A9AF
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool2x3 operator !=(bool lhs, bool2x3 rhs)
		{
			return new bool2x3(lhs != rhs.c0, lhs != rhs.c1, lhs != rhs.c2);
		}

		// Token: 0x06000820 RID: 2080 RVA: 0x0001C7DA File Offset: 0x0001A9DA
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool2x3 operator !(bool2x3 val)
		{
			return new bool2x3(!val.c0, !val.c1, !val.c2);
		}

		// Token: 0x06000821 RID: 2081 RVA: 0x0001C802 File Offset: 0x0001AA02
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool2x3 operator &(bool2x3 lhs, bool2x3 rhs)
		{
			return new bool2x3(lhs.c0 & rhs.c0, lhs.c1 & rhs.c1, lhs.c2 & rhs.c2);
		}

		// Token: 0x06000822 RID: 2082 RVA: 0x0001C83C File Offset: 0x0001AA3C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool2x3 operator &(bool2x3 lhs, bool rhs)
		{
			return new bool2x3(lhs.c0 & rhs, lhs.c1 & rhs, lhs.c2 & rhs);
		}

		// Token: 0x06000823 RID: 2083 RVA: 0x0001C867 File Offset: 0x0001AA67
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool2x3 operator &(bool lhs, bool2x3 rhs)
		{
			return new bool2x3(lhs & rhs.c0, lhs & rhs.c1, lhs & rhs.c2);
		}

		// Token: 0x06000824 RID: 2084 RVA: 0x0001C892 File Offset: 0x0001AA92
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool2x3 operator |(bool2x3 lhs, bool2x3 rhs)
		{
			return new bool2x3(lhs.c0 | rhs.c0, lhs.c1 | rhs.c1, lhs.c2 | rhs.c2);
		}

		// Token: 0x06000825 RID: 2085 RVA: 0x0001C8CC File Offset: 0x0001AACC
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool2x3 operator |(bool2x3 lhs, bool rhs)
		{
			return new bool2x3(lhs.c0 | rhs, lhs.c1 | rhs, lhs.c2 | rhs);
		}

		// Token: 0x06000826 RID: 2086 RVA: 0x0001C8F7 File Offset: 0x0001AAF7
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool2x3 operator |(bool lhs, bool2x3 rhs)
		{
			return new bool2x3(lhs | rhs.c0, lhs | rhs.c1, lhs | rhs.c2);
		}

		// Token: 0x06000827 RID: 2087 RVA: 0x0001C922 File Offset: 0x0001AB22
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool2x3 operator ^(bool2x3 lhs, bool2x3 rhs)
		{
			return new bool2x3(lhs.c0 ^ rhs.c0, lhs.c1 ^ rhs.c1, lhs.c2 ^ rhs.c2);
		}

		// Token: 0x06000828 RID: 2088 RVA: 0x0001C95C File Offset: 0x0001AB5C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool2x3 operator ^(bool2x3 lhs, bool rhs)
		{
			return new bool2x3(lhs.c0 ^ rhs, lhs.c1 ^ rhs, lhs.c2 ^ rhs);
		}

		// Token: 0x06000829 RID: 2089 RVA: 0x0001C987 File Offset: 0x0001AB87
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool2x3 operator ^(bool lhs, bool2x3 rhs)
		{
			return new bool2x3(lhs ^ rhs.c0, lhs ^ rhs.c1, lhs ^ rhs.c2);
		}

		// Token: 0x1700001F RID: 31
		public unsafe ref bool2 this[int index]
		{
			get
			{
				fixed (bool2x3* ptr = &this)
				{
					return ref *(bool2*)(ptr + (IntPtr)index * (IntPtr)sizeof(bool2) / (IntPtr)sizeof(bool2x3));
				}
			}
		}

		// Token: 0x0600082B RID: 2091 RVA: 0x0001C9CF File Offset: 0x0001ABCF
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool Equals(bool2x3 rhs)
		{
			return this.c0.Equals(rhs.c0) && this.c1.Equals(rhs.c1) && this.c2.Equals(rhs.c2);
		}

		// Token: 0x0600082C RID: 2092 RVA: 0x0001CA0C File Offset: 0x0001AC0C
		public override bool Equals(object o)
		{
			if (o is bool2x3)
			{
				bool2x3 bool2x = (bool2x3)o;
				return this.Equals(bool2x);
			}
			return false;
		}

		// Token: 0x0600082D RID: 2093 RVA: 0x0001CA31 File Offset: 0x0001AC31
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public override int GetHashCode()
		{
			return (int)math.hash(this);
		}

		// Token: 0x0600082E RID: 2094 RVA: 0x0001CA40 File Offset: 0x0001AC40
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public override string ToString()
		{
			return string.Format("bool2x3({0}, {1}, {2},  {3}, {4}, {5})", new object[]
			{
				this.c0.x,
				this.c1.x,
				this.c2.x,
				this.c0.y,
				this.c1.y,
				this.c2.y
			});
		}

		// Token: 0x04000029 RID: 41
		public bool2 c0;

		// Token: 0x0400002A RID: 42
		public bool2 c1;

		// Token: 0x0400002B RID: 43
		public bool2 c2;
	}
}
