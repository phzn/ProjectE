using System;
using System.Runtime.CompilerServices;
using Unity.IL2CPP.CompilerServices;

namespace Unity.Mathematics
{
	// Token: 0x02000010 RID: 16
	[Il2CppEagerStaticClassConstruction]
	[Serializable]
	public struct bool4x3 : IEquatable<bool4x3>
	{
		// Token: 0x06000AF5 RID: 2805 RVA: 0x00022AF9 File Offset: 0x00020CF9
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool4x3(bool4 c0, bool4 c1, bool4 c2)
		{
			this.c0 = c0;
			this.c1 = c1;
			this.c2 = c2;
		}

		// Token: 0x06000AF6 RID: 2806 RVA: 0x00022B10 File Offset: 0x00020D10
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool4x3(bool m00, bool m01, bool m02, bool m10, bool m11, bool m12, bool m20, bool m21, bool m22, bool m30, bool m31, bool m32)
		{
			this.c0 = new bool4(m00, m10, m20, m30);
			this.c1 = new bool4(m01, m11, m21, m31);
			this.c2 = new bool4(m02, m12, m22, m32);
		}

		// Token: 0x06000AF7 RID: 2807 RVA: 0x00022B48 File Offset: 0x00020D48
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool4x3(bool v)
		{
			this.c0 = v;
			this.c1 = v;
			this.c2 = v;
		}

		// Token: 0x06000AF8 RID: 2808 RVA: 0x00022B6E File Offset: 0x00020D6E
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static implicit operator bool4x3(bool v)
		{
			return new bool4x3(v);
		}

		// Token: 0x06000AF9 RID: 2809 RVA: 0x00022B76 File Offset: 0x00020D76
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool4x3 operator ==(bool4x3 lhs, bool4x3 rhs)
		{
			return new bool4x3(lhs.c0 == rhs.c0, lhs.c1 == rhs.c1, lhs.c2 == rhs.c2);
		}

		// Token: 0x06000AFA RID: 2810 RVA: 0x00022BB0 File Offset: 0x00020DB0
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool4x3 operator ==(bool4x3 lhs, bool rhs)
		{
			return new bool4x3(lhs.c0 == rhs, lhs.c1 == rhs, lhs.c2 == rhs);
		}

		// Token: 0x06000AFB RID: 2811 RVA: 0x00022BDB File Offset: 0x00020DDB
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool4x3 operator ==(bool lhs, bool4x3 rhs)
		{
			return new bool4x3(lhs == rhs.c0, lhs == rhs.c1, lhs == rhs.c2);
		}

		// Token: 0x06000AFC RID: 2812 RVA: 0x00022C06 File Offset: 0x00020E06
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool4x3 operator !=(bool4x3 lhs, bool4x3 rhs)
		{
			return new bool4x3(lhs.c0 != rhs.c0, lhs.c1 != rhs.c1, lhs.c2 != rhs.c2);
		}

		// Token: 0x06000AFD RID: 2813 RVA: 0x00022C40 File Offset: 0x00020E40
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool4x3 operator !=(bool4x3 lhs, bool rhs)
		{
			return new bool4x3(lhs.c0 != rhs, lhs.c1 != rhs, lhs.c2 != rhs);
		}

		// Token: 0x06000AFE RID: 2814 RVA: 0x00022C6B File Offset: 0x00020E6B
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool4x3 operator !=(bool lhs, bool4x3 rhs)
		{
			return new bool4x3(lhs != rhs.c0, lhs != rhs.c1, lhs != rhs.c2);
		}

		// Token: 0x06000AFF RID: 2815 RVA: 0x00022C96 File Offset: 0x00020E96
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool4x3 operator !(bool4x3 val)
		{
			return new bool4x3(!val.c0, !val.c1, !val.c2);
		}

		// Token: 0x06000B00 RID: 2816 RVA: 0x00022CBE File Offset: 0x00020EBE
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool4x3 operator &(bool4x3 lhs, bool4x3 rhs)
		{
			return new bool4x3(lhs.c0 & rhs.c0, lhs.c1 & rhs.c1, lhs.c2 & rhs.c2);
		}

		// Token: 0x06000B01 RID: 2817 RVA: 0x00022CF8 File Offset: 0x00020EF8
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool4x3 operator &(bool4x3 lhs, bool rhs)
		{
			return new bool4x3(lhs.c0 & rhs, lhs.c1 & rhs, lhs.c2 & rhs);
		}

		// Token: 0x06000B02 RID: 2818 RVA: 0x00022D23 File Offset: 0x00020F23
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool4x3 operator &(bool lhs, bool4x3 rhs)
		{
			return new bool4x3(lhs & rhs.c0, lhs & rhs.c1, lhs & rhs.c2);
		}

		// Token: 0x06000B03 RID: 2819 RVA: 0x00022D4E File Offset: 0x00020F4E
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool4x3 operator |(bool4x3 lhs, bool4x3 rhs)
		{
			return new bool4x3(lhs.c0 | rhs.c0, lhs.c1 | rhs.c1, lhs.c2 | rhs.c2);
		}

		// Token: 0x06000B04 RID: 2820 RVA: 0x00022D88 File Offset: 0x00020F88
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool4x3 operator |(bool4x3 lhs, bool rhs)
		{
			return new bool4x3(lhs.c0 | rhs, lhs.c1 | rhs, lhs.c2 | rhs);
		}

		// Token: 0x06000B05 RID: 2821 RVA: 0x00022DB3 File Offset: 0x00020FB3
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool4x3 operator |(bool lhs, bool4x3 rhs)
		{
			return new bool4x3(lhs | rhs.c0, lhs | rhs.c1, lhs | rhs.c2);
		}

		// Token: 0x06000B06 RID: 2822 RVA: 0x00022DDE File Offset: 0x00020FDE
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool4x3 operator ^(bool4x3 lhs, bool4x3 rhs)
		{
			return new bool4x3(lhs.c0 ^ rhs.c0, lhs.c1 ^ rhs.c1, lhs.c2 ^ rhs.c2);
		}

		// Token: 0x06000B07 RID: 2823 RVA: 0x00022E18 File Offset: 0x00021018
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool4x3 operator ^(bool4x3 lhs, bool rhs)
		{
			return new bool4x3(lhs.c0 ^ rhs, lhs.c1 ^ rhs, lhs.c2 ^ rhs);
		}

		// Token: 0x06000B08 RID: 2824 RVA: 0x00022E43 File Offset: 0x00021043
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool4x3 operator ^(bool lhs, bool4x3 rhs)
		{
			return new bool4x3(lhs ^ rhs.c0, lhs ^ rhs.c1, lhs ^ rhs.c2);
		}

		// Token: 0x170001EC RID: 492
		public unsafe ref bool4 this[int index]
		{
			get
			{
				fixed (bool4x3* ptr = &this)
				{
					return ref *(bool4*)(ptr + (IntPtr)index * (IntPtr)sizeof(bool4) / (IntPtr)sizeof(bool4x3));
				}
			}
		}

		// Token: 0x06000B0A RID: 2826 RVA: 0x00022E8B File Offset: 0x0002108B
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool Equals(bool4x3 rhs)
		{
			return this.c0.Equals(rhs.c0) && this.c1.Equals(rhs.c1) && this.c2.Equals(rhs.c2);
		}

		// Token: 0x06000B0B RID: 2827 RVA: 0x00022EC8 File Offset: 0x000210C8
		public override bool Equals(object o)
		{
			if (o is bool4x3)
			{
				bool4x3 bool4x = (bool4x3)o;
				return this.Equals(bool4x);
			}
			return false;
		}

		// Token: 0x06000B0C RID: 2828 RVA: 0x00022EED File Offset: 0x000210ED
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public override int GetHashCode()
		{
			return (int)math.hash(this);
		}

		// Token: 0x06000B0D RID: 2829 RVA: 0x00022EFC File Offset: 0x000210FC
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public override string ToString()
		{
			return string.Format("bool4x3({0}, {1}, {2},  {3}, {4}, {5},  {6}, {7}, {8},  {9}, {10}, {11})", new object[]
			{
				this.c0.x,
				this.c1.x,
				this.c2.x,
				this.c0.y,
				this.c1.y,
				this.c2.y,
				this.c0.z,
				this.c1.z,
				this.c2.z,
				this.c0.w,
				this.c1.w,
				this.c2.w
			});
		}

		// Token: 0x04000042 RID: 66
		public bool4 c0;

		// Token: 0x04000043 RID: 67
		public bool4 c1;

		// Token: 0x04000044 RID: 68
		public bool4 c2;
	}
}
