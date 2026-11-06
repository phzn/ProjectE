using System;
using System.Runtime.CompilerServices;
using Unity.IL2CPP.CompilerServices;

namespace Unity.Mathematics
{
	// Token: 0x0200000C RID: 12
	[Il2CppEagerStaticClassConstruction]
	[Serializable]
	public struct bool3x3 : IEquatable<bool3x3>
	{
		// Token: 0x060008FE RID: 2302 RVA: 0x0001E6AF File Offset: 0x0001C8AF
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool3x3(bool3 c0, bool3 c1, bool3 c2)
		{
			this.c0 = c0;
			this.c1 = c1;
			this.c2 = c2;
		}

		// Token: 0x060008FF RID: 2303 RVA: 0x0001E6C6 File Offset: 0x0001C8C6
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool3x3(bool m00, bool m01, bool m02, bool m10, bool m11, bool m12, bool m20, bool m21, bool m22)
		{
			this.c0 = new bool3(m00, m10, m20);
			this.c1 = new bool3(m01, m11, m21);
			this.c2 = new bool3(m02, m12, m22);
		}

		// Token: 0x06000900 RID: 2304 RVA: 0x0001E6F8 File Offset: 0x0001C8F8
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool3x3(bool v)
		{
			this.c0 = v;
			this.c1 = v;
			this.c2 = v;
		}

		// Token: 0x06000901 RID: 2305 RVA: 0x0001E71E File Offset: 0x0001C91E
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static implicit operator bool3x3(bool v)
		{
			return new bool3x3(v);
		}

		// Token: 0x06000902 RID: 2306 RVA: 0x0001E726 File Offset: 0x0001C926
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool3x3 operator ==(bool3x3 lhs, bool3x3 rhs)
		{
			return new bool3x3(lhs.c0 == rhs.c0, lhs.c1 == rhs.c1, lhs.c2 == rhs.c2);
		}

		// Token: 0x06000903 RID: 2307 RVA: 0x0001E760 File Offset: 0x0001C960
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool3x3 operator ==(bool3x3 lhs, bool rhs)
		{
			return new bool3x3(lhs.c0 == rhs, lhs.c1 == rhs, lhs.c2 == rhs);
		}

		// Token: 0x06000904 RID: 2308 RVA: 0x0001E78B File Offset: 0x0001C98B
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool3x3 operator ==(bool lhs, bool3x3 rhs)
		{
			return new bool3x3(lhs == rhs.c0, lhs == rhs.c1, lhs == rhs.c2);
		}

		// Token: 0x06000905 RID: 2309 RVA: 0x0001E7B6 File Offset: 0x0001C9B6
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool3x3 operator !=(bool3x3 lhs, bool3x3 rhs)
		{
			return new bool3x3(lhs.c0 != rhs.c0, lhs.c1 != rhs.c1, lhs.c2 != rhs.c2);
		}

		// Token: 0x06000906 RID: 2310 RVA: 0x0001E7F0 File Offset: 0x0001C9F0
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool3x3 operator !=(bool3x3 lhs, bool rhs)
		{
			return new bool3x3(lhs.c0 != rhs, lhs.c1 != rhs, lhs.c2 != rhs);
		}

		// Token: 0x06000907 RID: 2311 RVA: 0x0001E81B File Offset: 0x0001CA1B
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool3x3 operator !=(bool lhs, bool3x3 rhs)
		{
			return new bool3x3(lhs != rhs.c0, lhs != rhs.c1, lhs != rhs.c2);
		}

		// Token: 0x06000908 RID: 2312 RVA: 0x0001E846 File Offset: 0x0001CA46
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool3x3 operator !(bool3x3 val)
		{
			return new bool3x3(!val.c0, !val.c1, !val.c2);
		}

		// Token: 0x06000909 RID: 2313 RVA: 0x0001E86E File Offset: 0x0001CA6E
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool3x3 operator &(bool3x3 lhs, bool3x3 rhs)
		{
			return new bool3x3(lhs.c0 & rhs.c0, lhs.c1 & rhs.c1, lhs.c2 & rhs.c2);
		}

		// Token: 0x0600090A RID: 2314 RVA: 0x0001E8A8 File Offset: 0x0001CAA8
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool3x3 operator &(bool3x3 lhs, bool rhs)
		{
			return new bool3x3(lhs.c0 & rhs, lhs.c1 & rhs, lhs.c2 & rhs);
		}

		// Token: 0x0600090B RID: 2315 RVA: 0x0001E8D3 File Offset: 0x0001CAD3
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool3x3 operator &(bool lhs, bool3x3 rhs)
		{
			return new bool3x3(lhs & rhs.c0, lhs & rhs.c1, lhs & rhs.c2);
		}

		// Token: 0x0600090C RID: 2316 RVA: 0x0001E8FE File Offset: 0x0001CAFE
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool3x3 operator |(bool3x3 lhs, bool3x3 rhs)
		{
			return new bool3x3(lhs.c0 | rhs.c0, lhs.c1 | rhs.c1, lhs.c2 | rhs.c2);
		}

		// Token: 0x0600090D RID: 2317 RVA: 0x0001E938 File Offset: 0x0001CB38
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool3x3 operator |(bool3x3 lhs, bool rhs)
		{
			return new bool3x3(lhs.c0 | rhs, lhs.c1 | rhs, lhs.c2 | rhs);
		}

		// Token: 0x0600090E RID: 2318 RVA: 0x0001E963 File Offset: 0x0001CB63
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool3x3 operator |(bool lhs, bool3x3 rhs)
		{
			return new bool3x3(lhs | rhs.c0, lhs | rhs.c1, lhs | rhs.c2);
		}

		// Token: 0x0600090F RID: 2319 RVA: 0x0001E98E File Offset: 0x0001CB8E
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool3x3 operator ^(bool3x3 lhs, bool3x3 rhs)
		{
			return new bool3x3(lhs.c0 ^ rhs.c0, lhs.c1 ^ rhs.c1, lhs.c2 ^ rhs.c2);
		}

		// Token: 0x06000910 RID: 2320 RVA: 0x0001E9C8 File Offset: 0x0001CBC8
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool3x3 operator ^(bool3x3 lhs, bool rhs)
		{
			return new bool3x3(lhs.c0 ^ rhs, lhs.c1 ^ rhs, lhs.c2 ^ rhs);
		}

		// Token: 0x06000911 RID: 2321 RVA: 0x0001E9F3 File Offset: 0x0001CBF3
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool3x3 operator ^(bool lhs, bool3x3 rhs)
		{
			return new bool3x3(lhs ^ rhs.c0, lhs ^ rhs.c1, lhs ^ rhs.c2);
		}

		// Token: 0x17000098 RID: 152
		public unsafe ref bool3 this[int index]
		{
			get
			{
				fixed (bool3x3* ptr = &this)
				{
					return ref *(bool3*)(ptr + (IntPtr)index * (IntPtr)sizeof(bool3) / (IntPtr)sizeof(bool3x3));
				}
			}
		}

		// Token: 0x06000913 RID: 2323 RVA: 0x0001EA3B File Offset: 0x0001CC3B
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool Equals(bool3x3 rhs)
		{
			return this.c0.Equals(rhs.c0) && this.c1.Equals(rhs.c1) && this.c2.Equals(rhs.c2);
		}

		// Token: 0x06000914 RID: 2324 RVA: 0x0001EA78 File Offset: 0x0001CC78
		public override bool Equals(object o)
		{
			if (o is bool3x3)
			{
				bool3x3 bool3x = (bool3x3)o;
				return this.Equals(bool3x);
			}
			return false;
		}

		// Token: 0x06000915 RID: 2325 RVA: 0x0001EA9D File Offset: 0x0001CC9D
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public override int GetHashCode()
		{
			return (int)math.hash(this);
		}

		// Token: 0x06000916 RID: 2326 RVA: 0x0001EAAC File Offset: 0x0001CCAC
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public override string ToString()
		{
			return string.Format("bool3x3({0}, {1}, {2},  {3}, {4}, {5},  {6}, {7}, {8})", new object[]
			{
				this.c0.x,
				this.c1.x,
				this.c2.x,
				this.c0.y,
				this.c1.y,
				this.c2.y,
				this.c0.z,
				this.c1.z,
				this.c2.z
			});
		}

		// Token: 0x04000035 RID: 53
		public bool3 c0;

		// Token: 0x04000036 RID: 54
		public bool3 c1;

		// Token: 0x04000037 RID: 55
		public bool3 c2;
	}
}
