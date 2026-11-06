using System;
using System.Runtime.CompilerServices;
using Unity.IL2CPP.CompilerServices;

namespace Unity.Mathematics
{
	// Token: 0x02000009 RID: 9
	[Il2CppEagerStaticClassConstruction]
	[Serializable]
	public struct bool2x4 : IEquatable<bool2x4>
	{
		// Token: 0x0600082F RID: 2095 RVA: 0x0001CACF File Offset: 0x0001ACCF
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool2x4(bool2 c0, bool2 c1, bool2 c2, bool2 c3)
		{
			this.c0 = c0;
			this.c1 = c1;
			this.c2 = c2;
			this.c3 = c3;
		}

		// Token: 0x06000830 RID: 2096 RVA: 0x0001CAEE File Offset: 0x0001ACEE
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool2x4(bool m00, bool m01, bool m02, bool m03, bool m10, bool m11, bool m12, bool m13)
		{
			this.c0 = new bool2(m00, m10);
			this.c1 = new bool2(m01, m11);
			this.c2 = new bool2(m02, m12);
			this.c3 = new bool2(m03, m13);
		}

		// Token: 0x06000831 RID: 2097 RVA: 0x0001CB29 File Offset: 0x0001AD29
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool2x4(bool v)
		{
			this.c0 = v;
			this.c1 = v;
			this.c2 = v;
			this.c3 = v;
		}

		// Token: 0x06000832 RID: 2098 RVA: 0x0001CB5B File Offset: 0x0001AD5B
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static implicit operator bool2x4(bool v)
		{
			return new bool2x4(v);
		}

		// Token: 0x06000833 RID: 2099 RVA: 0x0001CB64 File Offset: 0x0001AD64
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool2x4 operator ==(bool2x4 lhs, bool2x4 rhs)
		{
			return new bool2x4(lhs.c0 == rhs.c0, lhs.c1 == rhs.c1, lhs.c2 == rhs.c2, lhs.c3 == rhs.c3);
		}

		// Token: 0x06000834 RID: 2100 RVA: 0x0001CBBA File Offset: 0x0001ADBA
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool2x4 operator ==(bool2x4 lhs, bool rhs)
		{
			return new bool2x4(lhs.c0 == rhs, lhs.c1 == rhs, lhs.c2 == rhs, lhs.c3 == rhs);
		}

		// Token: 0x06000835 RID: 2101 RVA: 0x0001CBF1 File Offset: 0x0001ADF1
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool2x4 operator ==(bool lhs, bool2x4 rhs)
		{
			return new bool2x4(lhs == rhs.c0, lhs == rhs.c1, lhs == rhs.c2, lhs == rhs.c3);
		}

		// Token: 0x06000836 RID: 2102 RVA: 0x0001CC28 File Offset: 0x0001AE28
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool2x4 operator !=(bool2x4 lhs, bool2x4 rhs)
		{
			return new bool2x4(lhs.c0 != rhs.c0, lhs.c1 != rhs.c1, lhs.c2 != rhs.c2, lhs.c3 != rhs.c3);
		}

		// Token: 0x06000837 RID: 2103 RVA: 0x0001CC7E File Offset: 0x0001AE7E
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool2x4 operator !=(bool2x4 lhs, bool rhs)
		{
			return new bool2x4(lhs.c0 != rhs, lhs.c1 != rhs, lhs.c2 != rhs, lhs.c3 != rhs);
		}

		// Token: 0x06000838 RID: 2104 RVA: 0x0001CCB5 File Offset: 0x0001AEB5
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool2x4 operator !=(bool lhs, bool2x4 rhs)
		{
			return new bool2x4(lhs != rhs.c0, lhs != rhs.c1, lhs != rhs.c2, lhs != rhs.c3);
		}

		// Token: 0x06000839 RID: 2105 RVA: 0x0001CCEC File Offset: 0x0001AEEC
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool2x4 operator !(bool2x4 val)
		{
			return new bool2x4(!val.c0, !val.c1, !val.c2, !val.c3);
		}

		// Token: 0x0600083A RID: 2106 RVA: 0x0001CD20 File Offset: 0x0001AF20
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool2x4 operator &(bool2x4 lhs, bool2x4 rhs)
		{
			return new bool2x4(lhs.c0 & rhs.c0, lhs.c1 & rhs.c1, lhs.c2 & rhs.c2, lhs.c3 & rhs.c3);
		}

		// Token: 0x0600083B RID: 2107 RVA: 0x0001CD76 File Offset: 0x0001AF76
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool2x4 operator &(bool2x4 lhs, bool rhs)
		{
			return new bool2x4(lhs.c0 & rhs, lhs.c1 & rhs, lhs.c2 & rhs, lhs.c3 & rhs);
		}

		// Token: 0x0600083C RID: 2108 RVA: 0x0001CDAD File Offset: 0x0001AFAD
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool2x4 operator &(bool lhs, bool2x4 rhs)
		{
			return new bool2x4(lhs & rhs.c0, lhs & rhs.c1, lhs & rhs.c2, lhs & rhs.c3);
		}

		// Token: 0x0600083D RID: 2109 RVA: 0x0001CDE4 File Offset: 0x0001AFE4
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool2x4 operator |(bool2x4 lhs, bool2x4 rhs)
		{
			return new bool2x4(lhs.c0 | rhs.c0, lhs.c1 | rhs.c1, lhs.c2 | rhs.c2, lhs.c3 | rhs.c3);
		}

		// Token: 0x0600083E RID: 2110 RVA: 0x0001CE3A File Offset: 0x0001B03A
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool2x4 operator |(bool2x4 lhs, bool rhs)
		{
			return new bool2x4(lhs.c0 | rhs, lhs.c1 | rhs, lhs.c2 | rhs, lhs.c3 | rhs);
		}

		// Token: 0x0600083F RID: 2111 RVA: 0x0001CE71 File Offset: 0x0001B071
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool2x4 operator |(bool lhs, bool2x4 rhs)
		{
			return new bool2x4(lhs | rhs.c0, lhs | rhs.c1, lhs | rhs.c2, lhs | rhs.c3);
		}

		// Token: 0x06000840 RID: 2112 RVA: 0x0001CEA8 File Offset: 0x0001B0A8
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool2x4 operator ^(bool2x4 lhs, bool2x4 rhs)
		{
			return new bool2x4(lhs.c0 ^ rhs.c0, lhs.c1 ^ rhs.c1, lhs.c2 ^ rhs.c2, lhs.c3 ^ rhs.c3);
		}

		// Token: 0x06000841 RID: 2113 RVA: 0x0001CEFE File Offset: 0x0001B0FE
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool2x4 operator ^(bool2x4 lhs, bool rhs)
		{
			return new bool2x4(lhs.c0 ^ rhs, lhs.c1 ^ rhs, lhs.c2 ^ rhs, lhs.c3 ^ rhs);
		}

		// Token: 0x06000842 RID: 2114 RVA: 0x0001CF35 File Offset: 0x0001B135
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool2x4 operator ^(bool lhs, bool2x4 rhs)
		{
			return new bool2x4(lhs ^ rhs.c0, lhs ^ rhs.c1, lhs ^ rhs.c2, lhs ^ rhs.c3);
		}

		// Token: 0x17000020 RID: 32
		public unsafe ref bool2 this[int index]
		{
			get
			{
				fixed (bool2x4* ptr = &this)
				{
					return ref *(bool2*)(ptr + (IntPtr)index * (IntPtr)sizeof(bool2) / (IntPtr)sizeof(bool2x4));
				}
			}
		}

		// Token: 0x06000844 RID: 2116 RVA: 0x0001CF88 File Offset: 0x0001B188
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool Equals(bool2x4 rhs)
		{
			return this.c0.Equals(rhs.c0) && this.c1.Equals(rhs.c1) && this.c2.Equals(rhs.c2) && this.c3.Equals(rhs.c3);
		}

		// Token: 0x06000845 RID: 2117 RVA: 0x0001CFE4 File Offset: 0x0001B1E4
		public override bool Equals(object o)
		{
			if (o is bool2x4)
			{
				bool2x4 bool2x = (bool2x4)o;
				return this.Equals(bool2x);
			}
			return false;
		}

		// Token: 0x06000846 RID: 2118 RVA: 0x0001D009 File Offset: 0x0001B209
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public override int GetHashCode()
		{
			return (int)math.hash(this);
		}

		// Token: 0x06000847 RID: 2119 RVA: 0x0001D018 File Offset: 0x0001B218
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public override string ToString()
		{
			return string.Format("bool2x4({0}, {1}, {2}, {3},  {4}, {5}, {6}, {7})", new object[]
			{
				this.c0.x,
				this.c1.x,
				this.c2.x,
				this.c3.x,
				this.c0.y,
				this.c1.y,
				this.c2.y,
				this.c3.y
			});
		}

		// Token: 0x0400002C RID: 44
		public bool2 c0;

		// Token: 0x0400002D RID: 45
		public bool2 c1;

		// Token: 0x0400002E RID: 46
		public bool2 c2;

		// Token: 0x0400002F RID: 47
		public bool2 c3;
	}
}
