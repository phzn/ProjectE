using System;
using System.Runtime.CompilerServices;
using Unity.IL2CPP.CompilerServices;

namespace Unity.Mathematics
{
	// Token: 0x0200000F RID: 15
	[Il2CppEagerStaticClassConstruction]
	[Serializable]
	public struct bool4x2 : IEquatable<bool4x2>
	{
		// Token: 0x06000ADC RID: 2780 RVA: 0x00022755 File Offset: 0x00020955
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool4x2(bool4 c0, bool4 c1)
		{
			this.c0 = c0;
			this.c1 = c1;
		}

		// Token: 0x06000ADD RID: 2781 RVA: 0x00022765 File Offset: 0x00020965
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool4x2(bool m00, bool m01, bool m10, bool m11, bool m20, bool m21, bool m30, bool m31)
		{
			this.c0 = new bool4(m00, m10, m20, m30);
			this.c1 = new bool4(m01, m11, m21, m31);
		}

		// Token: 0x06000ADE RID: 2782 RVA: 0x0002278A File Offset: 0x0002098A
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool4x2(bool v)
		{
			this.c0 = v;
			this.c1 = v;
		}

		// Token: 0x06000ADF RID: 2783 RVA: 0x000227A4 File Offset: 0x000209A4
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static implicit operator bool4x2(bool v)
		{
			return new bool4x2(v);
		}

		// Token: 0x06000AE0 RID: 2784 RVA: 0x000227AC File Offset: 0x000209AC
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool4x2 operator ==(bool4x2 lhs, bool4x2 rhs)
		{
			return new bool4x2(lhs.c0 == rhs.c0, lhs.c1 == rhs.c1);
		}

		// Token: 0x06000AE1 RID: 2785 RVA: 0x000227D5 File Offset: 0x000209D5
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool4x2 operator ==(bool4x2 lhs, bool rhs)
		{
			return new bool4x2(lhs.c0 == rhs, lhs.c1 == rhs);
		}

		// Token: 0x06000AE2 RID: 2786 RVA: 0x000227F4 File Offset: 0x000209F4
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool4x2 operator ==(bool lhs, bool4x2 rhs)
		{
			return new bool4x2(lhs == rhs.c0, lhs == rhs.c1);
		}

		// Token: 0x06000AE3 RID: 2787 RVA: 0x00022813 File Offset: 0x00020A13
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool4x2 operator !=(bool4x2 lhs, bool4x2 rhs)
		{
			return new bool4x2(lhs.c0 != rhs.c0, lhs.c1 != rhs.c1);
		}

		// Token: 0x06000AE4 RID: 2788 RVA: 0x0002283C File Offset: 0x00020A3C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool4x2 operator !=(bool4x2 lhs, bool rhs)
		{
			return new bool4x2(lhs.c0 != rhs, lhs.c1 != rhs);
		}

		// Token: 0x06000AE5 RID: 2789 RVA: 0x0002285B File Offset: 0x00020A5B
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool4x2 operator !=(bool lhs, bool4x2 rhs)
		{
			return new bool4x2(lhs != rhs.c0, lhs != rhs.c1);
		}

		// Token: 0x06000AE6 RID: 2790 RVA: 0x0002287A File Offset: 0x00020A7A
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool4x2 operator !(bool4x2 val)
		{
			return new bool4x2(!val.c0, !val.c1);
		}

		// Token: 0x06000AE7 RID: 2791 RVA: 0x00022897 File Offset: 0x00020A97
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool4x2 operator &(bool4x2 lhs, bool4x2 rhs)
		{
			return new bool4x2(lhs.c0 & rhs.c0, lhs.c1 & rhs.c1);
		}

		// Token: 0x06000AE8 RID: 2792 RVA: 0x000228C0 File Offset: 0x00020AC0
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool4x2 operator &(bool4x2 lhs, bool rhs)
		{
			return new bool4x2(lhs.c0 & rhs, lhs.c1 & rhs);
		}

		// Token: 0x06000AE9 RID: 2793 RVA: 0x000228DF File Offset: 0x00020ADF
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool4x2 operator &(bool lhs, bool4x2 rhs)
		{
			return new bool4x2(lhs & rhs.c0, lhs & rhs.c1);
		}

		// Token: 0x06000AEA RID: 2794 RVA: 0x000228FE File Offset: 0x00020AFE
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool4x2 operator |(bool4x2 lhs, bool4x2 rhs)
		{
			return new bool4x2(lhs.c0 | rhs.c0, lhs.c1 | rhs.c1);
		}

		// Token: 0x06000AEB RID: 2795 RVA: 0x00022927 File Offset: 0x00020B27
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool4x2 operator |(bool4x2 lhs, bool rhs)
		{
			return new bool4x2(lhs.c0 | rhs, lhs.c1 | rhs);
		}

		// Token: 0x06000AEC RID: 2796 RVA: 0x00022946 File Offset: 0x00020B46
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool4x2 operator |(bool lhs, bool4x2 rhs)
		{
			return new bool4x2(lhs | rhs.c0, lhs | rhs.c1);
		}

		// Token: 0x06000AED RID: 2797 RVA: 0x00022965 File Offset: 0x00020B65
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool4x2 operator ^(bool4x2 lhs, bool4x2 rhs)
		{
			return new bool4x2(lhs.c0 ^ rhs.c0, lhs.c1 ^ rhs.c1);
		}

		// Token: 0x06000AEE RID: 2798 RVA: 0x0002298E File Offset: 0x00020B8E
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool4x2 operator ^(bool4x2 lhs, bool rhs)
		{
			return new bool4x2(lhs.c0 ^ rhs, lhs.c1 ^ rhs);
		}

		// Token: 0x06000AEF RID: 2799 RVA: 0x000229AD File Offset: 0x00020BAD
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool4x2 operator ^(bool lhs, bool4x2 rhs)
		{
			return new bool4x2(lhs ^ rhs.c0, lhs ^ rhs.c1);
		}

		// Token: 0x170001EB RID: 491
		public unsafe ref bool4 this[int index]
		{
			get
			{
				fixed (bool4x2* ptr = &this)
				{
					return ref *(bool4*)(ptr + (IntPtr)index * (IntPtr)sizeof(bool4) / (IntPtr)sizeof(bool4x2));
				}
			}
		}

		// Token: 0x06000AF1 RID: 2801 RVA: 0x000229E7 File Offset: 0x00020BE7
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool Equals(bool4x2 rhs)
		{
			return this.c0.Equals(rhs.c0) && this.c1.Equals(rhs.c1);
		}

		// Token: 0x06000AF2 RID: 2802 RVA: 0x00022A10 File Offset: 0x00020C10
		public override bool Equals(object o)
		{
			if (o is bool4x2)
			{
				bool4x2 bool4x = (bool4x2)o;
				return this.Equals(bool4x);
			}
			return false;
		}

		// Token: 0x06000AF3 RID: 2803 RVA: 0x00022A35 File Offset: 0x00020C35
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public override int GetHashCode()
		{
			return (int)math.hash(this);
		}

		// Token: 0x06000AF4 RID: 2804 RVA: 0x00022A44 File Offset: 0x00020C44
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public override string ToString()
		{
			return string.Format("bool4x2({0}, {1},  {2}, {3},  {4}, {5},  {6}, {7})", new object[]
			{
				this.c0.x,
				this.c1.x,
				this.c0.y,
				this.c1.y,
				this.c0.z,
				this.c1.z,
				this.c0.w,
				this.c1.w
			});
		}

		// Token: 0x04000040 RID: 64
		public bool4 c0;

		// Token: 0x04000041 RID: 65
		public bool4 c1;
	}
}
