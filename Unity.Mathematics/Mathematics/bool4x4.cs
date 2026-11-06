using System;
using System.Runtime.CompilerServices;
using Unity.IL2CPP.CompilerServices;

namespace Unity.Mathematics
{
	// Token: 0x02000011 RID: 17
	[Il2CppEagerStaticClassConstruction]
	[Serializable]
	public struct bool4x4 : IEquatable<bool4x4>
	{
		// Token: 0x06000B0E RID: 2830 RVA: 0x00023001 File Offset: 0x00021201
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool4x4(bool4 c0, bool4 c1, bool4 c2, bool4 c3)
		{
			this.c0 = c0;
			this.c1 = c1;
			this.c2 = c2;
			this.c3 = c3;
		}

		// Token: 0x06000B0F RID: 2831 RVA: 0x00023020 File Offset: 0x00021220
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool4x4(bool m00, bool m01, bool m02, bool m03, bool m10, bool m11, bool m12, bool m13, bool m20, bool m21, bool m22, bool m23, bool m30, bool m31, bool m32, bool m33)
		{
			this.c0 = new bool4(m00, m10, m20, m30);
			this.c1 = new bool4(m01, m11, m21, m31);
			this.c2 = new bool4(m02, m12, m22, m32);
			this.c3 = new bool4(m03, m13, m23, m33);
		}

		// Token: 0x06000B10 RID: 2832 RVA: 0x00023076 File Offset: 0x00021276
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool4x4(bool v)
		{
			this.c0 = v;
			this.c1 = v;
			this.c2 = v;
			this.c3 = v;
		}

		// Token: 0x06000B11 RID: 2833 RVA: 0x000230A8 File Offset: 0x000212A8
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static implicit operator bool4x4(bool v)
		{
			return new bool4x4(v);
		}

		// Token: 0x06000B12 RID: 2834 RVA: 0x000230B0 File Offset: 0x000212B0
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool4x4 operator ==(bool4x4 lhs, bool4x4 rhs)
		{
			return new bool4x4(lhs.c0 == rhs.c0, lhs.c1 == rhs.c1, lhs.c2 == rhs.c2, lhs.c3 == rhs.c3);
		}

		// Token: 0x06000B13 RID: 2835 RVA: 0x00023106 File Offset: 0x00021306
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool4x4 operator ==(bool4x4 lhs, bool rhs)
		{
			return new bool4x4(lhs.c0 == rhs, lhs.c1 == rhs, lhs.c2 == rhs, lhs.c3 == rhs);
		}

		// Token: 0x06000B14 RID: 2836 RVA: 0x0002313D File Offset: 0x0002133D
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool4x4 operator ==(bool lhs, bool4x4 rhs)
		{
			return new bool4x4(lhs == rhs.c0, lhs == rhs.c1, lhs == rhs.c2, lhs == rhs.c3);
		}

		// Token: 0x06000B15 RID: 2837 RVA: 0x00023174 File Offset: 0x00021374
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool4x4 operator !=(bool4x4 lhs, bool4x4 rhs)
		{
			return new bool4x4(lhs.c0 != rhs.c0, lhs.c1 != rhs.c1, lhs.c2 != rhs.c2, lhs.c3 != rhs.c3);
		}

		// Token: 0x06000B16 RID: 2838 RVA: 0x000231CA File Offset: 0x000213CA
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool4x4 operator !=(bool4x4 lhs, bool rhs)
		{
			return new bool4x4(lhs.c0 != rhs, lhs.c1 != rhs, lhs.c2 != rhs, lhs.c3 != rhs);
		}

		// Token: 0x06000B17 RID: 2839 RVA: 0x00023201 File Offset: 0x00021401
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool4x4 operator !=(bool lhs, bool4x4 rhs)
		{
			return new bool4x4(lhs != rhs.c0, lhs != rhs.c1, lhs != rhs.c2, lhs != rhs.c3);
		}

		// Token: 0x06000B18 RID: 2840 RVA: 0x00023238 File Offset: 0x00021438
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool4x4 operator !(bool4x4 val)
		{
			return new bool4x4(!val.c0, !val.c1, !val.c2, !val.c3);
		}

		// Token: 0x06000B19 RID: 2841 RVA: 0x0002326C File Offset: 0x0002146C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool4x4 operator &(bool4x4 lhs, bool4x4 rhs)
		{
			return new bool4x4(lhs.c0 & rhs.c0, lhs.c1 & rhs.c1, lhs.c2 & rhs.c2, lhs.c3 & rhs.c3);
		}

		// Token: 0x06000B1A RID: 2842 RVA: 0x000232C2 File Offset: 0x000214C2
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool4x4 operator &(bool4x4 lhs, bool rhs)
		{
			return new bool4x4(lhs.c0 & rhs, lhs.c1 & rhs, lhs.c2 & rhs, lhs.c3 & rhs);
		}

		// Token: 0x06000B1B RID: 2843 RVA: 0x000232F9 File Offset: 0x000214F9
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool4x4 operator &(bool lhs, bool4x4 rhs)
		{
			return new bool4x4(lhs & rhs.c0, lhs & rhs.c1, lhs & rhs.c2, lhs & rhs.c3);
		}

		// Token: 0x06000B1C RID: 2844 RVA: 0x00023330 File Offset: 0x00021530
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool4x4 operator |(bool4x4 lhs, bool4x4 rhs)
		{
			return new bool4x4(lhs.c0 | rhs.c0, lhs.c1 | rhs.c1, lhs.c2 | rhs.c2, lhs.c3 | rhs.c3);
		}

		// Token: 0x06000B1D RID: 2845 RVA: 0x00023386 File Offset: 0x00021586
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool4x4 operator |(bool4x4 lhs, bool rhs)
		{
			return new bool4x4(lhs.c0 | rhs, lhs.c1 | rhs, lhs.c2 | rhs, lhs.c3 | rhs);
		}

		// Token: 0x06000B1E RID: 2846 RVA: 0x000233BD File Offset: 0x000215BD
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool4x4 operator |(bool lhs, bool4x4 rhs)
		{
			return new bool4x4(lhs | rhs.c0, lhs | rhs.c1, lhs | rhs.c2, lhs | rhs.c3);
		}

		// Token: 0x06000B1F RID: 2847 RVA: 0x000233F4 File Offset: 0x000215F4
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool4x4 operator ^(bool4x4 lhs, bool4x4 rhs)
		{
			return new bool4x4(lhs.c0 ^ rhs.c0, lhs.c1 ^ rhs.c1, lhs.c2 ^ rhs.c2, lhs.c3 ^ rhs.c3);
		}

		// Token: 0x06000B20 RID: 2848 RVA: 0x0002344A File Offset: 0x0002164A
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool4x4 operator ^(bool4x4 lhs, bool rhs)
		{
			return new bool4x4(lhs.c0 ^ rhs, lhs.c1 ^ rhs, lhs.c2 ^ rhs, lhs.c3 ^ rhs);
		}

		// Token: 0x06000B21 RID: 2849 RVA: 0x00023481 File Offset: 0x00021681
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool4x4 operator ^(bool lhs, bool4x4 rhs)
		{
			return new bool4x4(lhs ^ rhs.c0, lhs ^ rhs.c1, lhs ^ rhs.c2, lhs ^ rhs.c3);
		}

		// Token: 0x170001ED RID: 493
		public unsafe ref bool4 this[int index]
		{
			get
			{
				fixed (bool4x4* ptr = &this)
				{
					return ref *(bool4*)(ptr + (IntPtr)index * (IntPtr)sizeof(bool4) / (IntPtr)sizeof(bool4x4));
				}
			}
		}

		// Token: 0x06000B23 RID: 2851 RVA: 0x000234D4 File Offset: 0x000216D4
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool Equals(bool4x4 rhs)
		{
			return this.c0.Equals(rhs.c0) && this.c1.Equals(rhs.c1) && this.c2.Equals(rhs.c2) && this.c3.Equals(rhs.c3);
		}

		// Token: 0x06000B24 RID: 2852 RVA: 0x00023530 File Offset: 0x00021730
		public override bool Equals(object o)
		{
			if (o is bool4x4)
			{
				bool4x4 bool4x = (bool4x4)o;
				return this.Equals(bool4x);
			}
			return false;
		}

		// Token: 0x06000B25 RID: 2853 RVA: 0x00023555 File Offset: 0x00021755
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public override int GetHashCode()
		{
			return (int)math.hash(this);
		}

		// Token: 0x06000B26 RID: 2854 RVA: 0x00023564 File Offset: 0x00021764
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public override string ToString()
		{
			return string.Format("bool4x4({0}, {1}, {2}, {3},  {4}, {5}, {6}, {7},  {8}, {9}, {10}, {11},  {12}, {13}, {14}, {15})", new object[]
			{
				this.c0.x,
				this.c1.x,
				this.c2.x,
				this.c3.x,
				this.c0.y,
				this.c1.y,
				this.c2.y,
				this.c3.y,
				this.c0.z,
				this.c1.z,
				this.c2.z,
				this.c3.z,
				this.c0.w,
				this.c1.w,
				this.c2.w,
				this.c3.w
			});
		}

		// Token: 0x04000045 RID: 69
		public bool4 c0;

		// Token: 0x04000046 RID: 70
		public bool4 c1;

		// Token: 0x04000047 RID: 71
		public bool4 c2;

		// Token: 0x04000048 RID: 72
		public bool4 c3;
	}
}
