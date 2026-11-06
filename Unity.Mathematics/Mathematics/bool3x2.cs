using System;
using System.Runtime.CompilerServices;
using Unity.IL2CPP.CompilerServices;

namespace Unity.Mathematics
{
	// Token: 0x0200000B RID: 11
	[Il2CppEagerStaticClassConstruction]
	[Serializable]
	public struct bool3x2 : IEquatable<bool3x2>
	{
		// Token: 0x060008E5 RID: 2277 RVA: 0x0001E333 File Offset: 0x0001C533
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool3x2(bool3 c0, bool3 c1)
		{
			this.c0 = c0;
			this.c1 = c1;
		}

		// Token: 0x060008E6 RID: 2278 RVA: 0x0001E343 File Offset: 0x0001C543
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool3x2(bool m00, bool m01, bool m10, bool m11, bool m20, bool m21)
		{
			this.c0 = new bool3(m00, m10, m20);
			this.c1 = new bool3(m01, m11, m21);
		}

		// Token: 0x060008E7 RID: 2279 RVA: 0x0001E364 File Offset: 0x0001C564
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool3x2(bool v)
		{
			this.c0 = v;
			this.c1 = v;
		}

		// Token: 0x060008E8 RID: 2280 RVA: 0x0001E37E File Offset: 0x0001C57E
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static implicit operator bool3x2(bool v)
		{
			return new bool3x2(v);
		}

		// Token: 0x060008E9 RID: 2281 RVA: 0x0001E386 File Offset: 0x0001C586
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool3x2 operator ==(bool3x2 lhs, bool3x2 rhs)
		{
			return new bool3x2(lhs.c0 == rhs.c0, lhs.c1 == rhs.c1);
		}

		// Token: 0x060008EA RID: 2282 RVA: 0x0001E3AF File Offset: 0x0001C5AF
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool3x2 operator ==(bool3x2 lhs, bool rhs)
		{
			return new bool3x2(lhs.c0 == rhs, lhs.c1 == rhs);
		}

		// Token: 0x060008EB RID: 2283 RVA: 0x0001E3CE File Offset: 0x0001C5CE
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool3x2 operator ==(bool lhs, bool3x2 rhs)
		{
			return new bool3x2(lhs == rhs.c0, lhs == rhs.c1);
		}

		// Token: 0x060008EC RID: 2284 RVA: 0x0001E3ED File Offset: 0x0001C5ED
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool3x2 operator !=(bool3x2 lhs, bool3x2 rhs)
		{
			return new bool3x2(lhs.c0 != rhs.c0, lhs.c1 != rhs.c1);
		}

		// Token: 0x060008ED RID: 2285 RVA: 0x0001E416 File Offset: 0x0001C616
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool3x2 operator !=(bool3x2 lhs, bool rhs)
		{
			return new bool3x2(lhs.c0 != rhs, lhs.c1 != rhs);
		}

		// Token: 0x060008EE RID: 2286 RVA: 0x0001E435 File Offset: 0x0001C635
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool3x2 operator !=(bool lhs, bool3x2 rhs)
		{
			return new bool3x2(lhs != rhs.c0, lhs != rhs.c1);
		}

		// Token: 0x060008EF RID: 2287 RVA: 0x0001E454 File Offset: 0x0001C654
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool3x2 operator !(bool3x2 val)
		{
			return new bool3x2(!val.c0, !val.c1);
		}

		// Token: 0x060008F0 RID: 2288 RVA: 0x0001E471 File Offset: 0x0001C671
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool3x2 operator &(bool3x2 lhs, bool3x2 rhs)
		{
			return new bool3x2(lhs.c0 & rhs.c0, lhs.c1 & rhs.c1);
		}

		// Token: 0x060008F1 RID: 2289 RVA: 0x0001E49A File Offset: 0x0001C69A
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool3x2 operator &(bool3x2 lhs, bool rhs)
		{
			return new bool3x2(lhs.c0 & rhs, lhs.c1 & rhs);
		}

		// Token: 0x060008F2 RID: 2290 RVA: 0x0001E4B9 File Offset: 0x0001C6B9
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool3x2 operator &(bool lhs, bool3x2 rhs)
		{
			return new bool3x2(lhs & rhs.c0, lhs & rhs.c1);
		}

		// Token: 0x060008F3 RID: 2291 RVA: 0x0001E4D8 File Offset: 0x0001C6D8
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool3x2 operator |(bool3x2 lhs, bool3x2 rhs)
		{
			return new bool3x2(lhs.c0 | rhs.c0, lhs.c1 | rhs.c1);
		}

		// Token: 0x060008F4 RID: 2292 RVA: 0x0001E501 File Offset: 0x0001C701
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool3x2 operator |(bool3x2 lhs, bool rhs)
		{
			return new bool3x2(lhs.c0 | rhs, lhs.c1 | rhs);
		}

		// Token: 0x060008F5 RID: 2293 RVA: 0x0001E520 File Offset: 0x0001C720
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool3x2 operator |(bool lhs, bool3x2 rhs)
		{
			return new bool3x2(lhs | rhs.c0, lhs | rhs.c1);
		}

		// Token: 0x060008F6 RID: 2294 RVA: 0x0001E53F File Offset: 0x0001C73F
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool3x2 operator ^(bool3x2 lhs, bool3x2 rhs)
		{
			return new bool3x2(lhs.c0 ^ rhs.c0, lhs.c1 ^ rhs.c1);
		}

		// Token: 0x060008F7 RID: 2295 RVA: 0x0001E568 File Offset: 0x0001C768
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool3x2 operator ^(bool3x2 lhs, bool rhs)
		{
			return new bool3x2(lhs.c0 ^ rhs, lhs.c1 ^ rhs);
		}

		// Token: 0x060008F8 RID: 2296 RVA: 0x0001E587 File Offset: 0x0001C787
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool3x2 operator ^(bool lhs, bool3x2 rhs)
		{
			return new bool3x2(lhs ^ rhs.c0, lhs ^ rhs.c1);
		}

		// Token: 0x17000097 RID: 151
		public unsafe ref bool3 this[int index]
		{
			get
			{
				fixed (bool3x2* ptr = &this)
				{
					return ref *(bool3*)(ptr + (IntPtr)index * (IntPtr)sizeof(bool3) / (IntPtr)sizeof(bool3x2));
				}
			}
		}

		// Token: 0x060008FA RID: 2298 RVA: 0x0001E5C3 File Offset: 0x0001C7C3
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool Equals(bool3x2 rhs)
		{
			return this.c0.Equals(rhs.c0) && this.c1.Equals(rhs.c1);
		}

		// Token: 0x060008FB RID: 2299 RVA: 0x0001E5EC File Offset: 0x0001C7EC
		public override bool Equals(object o)
		{
			if (o is bool3x2)
			{
				bool3x2 bool3x = (bool3x2)o;
				return this.Equals(bool3x);
			}
			return false;
		}

		// Token: 0x060008FC RID: 2300 RVA: 0x0001E611 File Offset: 0x0001C811
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public override int GetHashCode()
		{
			return (int)math.hash(this);
		}

		// Token: 0x060008FD RID: 2301 RVA: 0x0001E620 File Offset: 0x0001C820
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public override string ToString()
		{
			return string.Format("bool3x2({0}, {1},  {2}, {3},  {4}, {5})", new object[]
			{
				this.c0.x,
				this.c1.x,
				this.c0.y,
				this.c1.y,
				this.c0.z,
				this.c1.z
			});
		}

		// Token: 0x04000033 RID: 51
		public bool3 c0;

		// Token: 0x04000034 RID: 52
		public bool3 c1;
	}
}
