using System;
using System.Runtime.CompilerServices;
using Unity.IL2CPP.CompilerServices;

namespace Unity.Mathematics
{
	// Token: 0x0200000D RID: 13
	[Il2CppEagerStaticClassConstruction]
	[Serializable]
	public struct bool3x4 : IEquatable<bool3x4>
	{
		// Token: 0x06000917 RID: 2327 RVA: 0x0001EB75 File Offset: 0x0001CD75
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool3x4(bool3 c0, bool3 c1, bool3 c2, bool3 c3)
		{
			this.c0 = c0;
			this.c1 = c1;
			this.c2 = c2;
			this.c3 = c3;
		}

		// Token: 0x06000918 RID: 2328 RVA: 0x0001EB94 File Offset: 0x0001CD94
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool3x4(bool m00, bool m01, bool m02, bool m03, bool m10, bool m11, bool m12, bool m13, bool m20, bool m21, bool m22, bool m23)
		{
			this.c0 = new bool3(m00, m10, m20);
			this.c1 = new bool3(m01, m11, m21);
			this.c2 = new bool3(m02, m12, m22);
			this.c3 = new bool3(m03, m13, m23);
		}

		// Token: 0x06000919 RID: 2329 RVA: 0x0001EBE2 File Offset: 0x0001CDE2
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool3x4(bool v)
		{
			this.c0 = v;
			this.c1 = v;
			this.c2 = v;
			this.c3 = v;
		}

		// Token: 0x0600091A RID: 2330 RVA: 0x0001EC14 File Offset: 0x0001CE14
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static implicit operator bool3x4(bool v)
		{
			return new bool3x4(v);
		}

		// Token: 0x0600091B RID: 2331 RVA: 0x0001EC1C File Offset: 0x0001CE1C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool3x4 operator ==(bool3x4 lhs, bool3x4 rhs)
		{
			return new bool3x4(lhs.c0 == rhs.c0, lhs.c1 == rhs.c1, lhs.c2 == rhs.c2, lhs.c3 == rhs.c3);
		}

		// Token: 0x0600091C RID: 2332 RVA: 0x0001EC72 File Offset: 0x0001CE72
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool3x4 operator ==(bool3x4 lhs, bool rhs)
		{
			return new bool3x4(lhs.c0 == rhs, lhs.c1 == rhs, lhs.c2 == rhs, lhs.c3 == rhs);
		}

		// Token: 0x0600091D RID: 2333 RVA: 0x0001ECA9 File Offset: 0x0001CEA9
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool3x4 operator ==(bool lhs, bool3x4 rhs)
		{
			return new bool3x4(lhs == rhs.c0, lhs == rhs.c1, lhs == rhs.c2, lhs == rhs.c3);
		}

		// Token: 0x0600091E RID: 2334 RVA: 0x0001ECE0 File Offset: 0x0001CEE0
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool3x4 operator !=(bool3x4 lhs, bool3x4 rhs)
		{
			return new bool3x4(lhs.c0 != rhs.c0, lhs.c1 != rhs.c1, lhs.c2 != rhs.c2, lhs.c3 != rhs.c3);
		}

		// Token: 0x0600091F RID: 2335 RVA: 0x0001ED36 File Offset: 0x0001CF36
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool3x4 operator !=(bool3x4 lhs, bool rhs)
		{
			return new bool3x4(lhs.c0 != rhs, lhs.c1 != rhs, lhs.c2 != rhs, lhs.c3 != rhs);
		}

		// Token: 0x06000920 RID: 2336 RVA: 0x0001ED6D File Offset: 0x0001CF6D
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool3x4 operator !=(bool lhs, bool3x4 rhs)
		{
			return new bool3x4(lhs != rhs.c0, lhs != rhs.c1, lhs != rhs.c2, lhs != rhs.c3);
		}

		// Token: 0x06000921 RID: 2337 RVA: 0x0001EDA4 File Offset: 0x0001CFA4
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool3x4 operator !(bool3x4 val)
		{
			return new bool3x4(!val.c0, !val.c1, !val.c2, !val.c3);
		}

		// Token: 0x06000922 RID: 2338 RVA: 0x0001EDD8 File Offset: 0x0001CFD8
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool3x4 operator &(bool3x4 lhs, bool3x4 rhs)
		{
			return new bool3x4(lhs.c0 & rhs.c0, lhs.c1 & rhs.c1, lhs.c2 & rhs.c2, lhs.c3 & rhs.c3);
		}

		// Token: 0x06000923 RID: 2339 RVA: 0x0001EE2E File Offset: 0x0001D02E
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool3x4 operator &(bool3x4 lhs, bool rhs)
		{
			return new bool3x4(lhs.c0 & rhs, lhs.c1 & rhs, lhs.c2 & rhs, lhs.c3 & rhs);
		}

		// Token: 0x06000924 RID: 2340 RVA: 0x0001EE65 File Offset: 0x0001D065
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool3x4 operator &(bool lhs, bool3x4 rhs)
		{
			return new bool3x4(lhs & rhs.c0, lhs & rhs.c1, lhs & rhs.c2, lhs & rhs.c3);
		}

		// Token: 0x06000925 RID: 2341 RVA: 0x0001EE9C File Offset: 0x0001D09C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool3x4 operator |(bool3x4 lhs, bool3x4 rhs)
		{
			return new bool3x4(lhs.c0 | rhs.c0, lhs.c1 | rhs.c1, lhs.c2 | rhs.c2, lhs.c3 | rhs.c3);
		}

		// Token: 0x06000926 RID: 2342 RVA: 0x0001EEF2 File Offset: 0x0001D0F2
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool3x4 operator |(bool3x4 lhs, bool rhs)
		{
			return new bool3x4(lhs.c0 | rhs, lhs.c1 | rhs, lhs.c2 | rhs, lhs.c3 | rhs);
		}

		// Token: 0x06000927 RID: 2343 RVA: 0x0001EF29 File Offset: 0x0001D129
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool3x4 operator |(bool lhs, bool3x4 rhs)
		{
			return new bool3x4(lhs | rhs.c0, lhs | rhs.c1, lhs | rhs.c2, lhs | rhs.c3);
		}

		// Token: 0x06000928 RID: 2344 RVA: 0x0001EF60 File Offset: 0x0001D160
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool3x4 operator ^(bool3x4 lhs, bool3x4 rhs)
		{
			return new bool3x4(lhs.c0 ^ rhs.c0, lhs.c1 ^ rhs.c1, lhs.c2 ^ rhs.c2, lhs.c3 ^ rhs.c3);
		}

		// Token: 0x06000929 RID: 2345 RVA: 0x0001EFB6 File Offset: 0x0001D1B6
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool3x4 operator ^(bool3x4 lhs, bool rhs)
		{
			return new bool3x4(lhs.c0 ^ rhs, lhs.c1 ^ rhs, lhs.c2 ^ rhs, lhs.c3 ^ rhs);
		}

		// Token: 0x0600092A RID: 2346 RVA: 0x0001EFED File Offset: 0x0001D1ED
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool3x4 operator ^(bool lhs, bool3x4 rhs)
		{
			return new bool3x4(lhs ^ rhs.c0, lhs ^ rhs.c1, lhs ^ rhs.c2, lhs ^ rhs.c3);
		}

		// Token: 0x17000099 RID: 153
		public unsafe ref bool3 this[int index]
		{
			get
			{
				fixed (bool3x4* ptr = &this)
				{
					return ref *(bool3*)(ptr + (IntPtr)index * (IntPtr)sizeof(bool3) / (IntPtr)sizeof(bool3x4));
				}
			}
		}

		// Token: 0x0600092C RID: 2348 RVA: 0x0001F040 File Offset: 0x0001D240
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool Equals(bool3x4 rhs)
		{
			return this.c0.Equals(rhs.c0) && this.c1.Equals(rhs.c1) && this.c2.Equals(rhs.c2) && this.c3.Equals(rhs.c3);
		}

		// Token: 0x0600092D RID: 2349 RVA: 0x0001F09C File Offset: 0x0001D29C
		public override bool Equals(object o)
		{
			if (o is bool3x4)
			{
				bool3x4 bool3x = (bool3x4)o;
				return this.Equals(bool3x);
			}
			return false;
		}

		// Token: 0x0600092E RID: 2350 RVA: 0x0001F0C1 File Offset: 0x0001D2C1
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public override int GetHashCode()
		{
			return (int)math.hash(this);
		}

		// Token: 0x0600092F RID: 2351 RVA: 0x0001F0D0 File Offset: 0x0001D2D0
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public override string ToString()
		{
			return string.Format("bool3x4({0}, {1}, {2}, {3},  {4}, {5}, {6}, {7},  {8}, {9}, {10}, {11})", new object[]
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
				this.c3.z
			});
		}

		// Token: 0x04000038 RID: 56
		public bool3 c0;

		// Token: 0x04000039 RID: 57
		public bool3 c1;

		// Token: 0x0400003A RID: 58
		public bool3 c2;

		// Token: 0x0400003B RID: 59
		public bool3 c3;
	}
}
