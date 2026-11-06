using System;
using System.Runtime.CompilerServices;
using Unity.IL2CPP.CompilerServices;

namespace Unity.Mathematics
{
	// Token: 0x02000030 RID: 48
	[Il2CppEagerStaticClassConstruction]
	[Serializable]
	public struct int2x3 : IEquatable<int2x3>, IFormattable
	{
		// Token: 0x06001925 RID: 6437 RVA: 0x00045929 File Offset: 0x00043B29
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int2x3(int2 c0, int2 c1, int2 c2)
		{
			this.c0 = c0;
			this.c1 = c1;
			this.c2 = c2;
		}

		// Token: 0x06001926 RID: 6438 RVA: 0x00045940 File Offset: 0x00043B40
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int2x3(int m00, int m01, int m02, int m10, int m11, int m12)
		{
			this.c0 = new int2(m00, m10);
			this.c1 = new int2(m01, m11);
			this.c2 = new int2(m02, m12);
		}

		// Token: 0x06001927 RID: 6439 RVA: 0x0004596C File Offset: 0x00043B6C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int2x3(int v)
		{
			this.c0 = v;
			this.c1 = v;
			this.c2 = v;
		}

		// Token: 0x06001928 RID: 6440 RVA: 0x00045994 File Offset: 0x00043B94
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int2x3(bool v)
		{
			this.c0 = math.select(new int2(0), new int2(1), v);
			this.c1 = math.select(new int2(0), new int2(1), v);
			this.c2 = math.select(new int2(0), new int2(1), v);
		}

		// Token: 0x06001929 RID: 6441 RVA: 0x000459EC File Offset: 0x00043BEC
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int2x3(bool2x3 v)
		{
			this.c0 = math.select(new int2(0), new int2(1), v.c0);
			this.c1 = math.select(new int2(0), new int2(1), v.c1);
			this.c2 = math.select(new int2(0), new int2(1), v.c2);
		}

		// Token: 0x0600192A RID: 6442 RVA: 0x00045A50 File Offset: 0x00043C50
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int2x3(uint v)
		{
			this.c0 = (int2)v;
			this.c1 = (int2)v;
			this.c2 = (int2)v;
		}

		// Token: 0x0600192B RID: 6443 RVA: 0x00045A76 File Offset: 0x00043C76
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int2x3(uint2x3 v)
		{
			this.c0 = (int2)v.c0;
			this.c1 = (int2)v.c1;
			this.c2 = (int2)v.c2;
		}

		// Token: 0x0600192C RID: 6444 RVA: 0x00045AAB File Offset: 0x00043CAB
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int2x3(float v)
		{
			this.c0 = (int2)v;
			this.c1 = (int2)v;
			this.c2 = (int2)v;
		}

		// Token: 0x0600192D RID: 6445 RVA: 0x00045AD1 File Offset: 0x00043CD1
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int2x3(float2x3 v)
		{
			this.c0 = (int2)v.c0;
			this.c1 = (int2)v.c1;
			this.c2 = (int2)v.c2;
		}

		// Token: 0x0600192E RID: 6446 RVA: 0x00045B06 File Offset: 0x00043D06
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int2x3(double v)
		{
			this.c0 = (int2)v;
			this.c1 = (int2)v;
			this.c2 = (int2)v;
		}

		// Token: 0x0600192F RID: 6447 RVA: 0x00045B2C File Offset: 0x00043D2C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int2x3(double2x3 v)
		{
			this.c0 = (int2)v.c0;
			this.c1 = (int2)v.c1;
			this.c2 = (int2)v.c2;
		}

		// Token: 0x06001930 RID: 6448 RVA: 0x00045B61 File Offset: 0x00043D61
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static implicit operator int2x3(int v)
		{
			return new int2x3(v);
		}

		// Token: 0x06001931 RID: 6449 RVA: 0x00045B69 File Offset: 0x00043D69
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static explicit operator int2x3(bool v)
		{
			return new int2x3(v);
		}

		// Token: 0x06001932 RID: 6450 RVA: 0x00045B71 File Offset: 0x00043D71
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static explicit operator int2x3(bool2x3 v)
		{
			return new int2x3(v);
		}

		// Token: 0x06001933 RID: 6451 RVA: 0x00045B79 File Offset: 0x00043D79
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static explicit operator int2x3(uint v)
		{
			return new int2x3(v);
		}

		// Token: 0x06001934 RID: 6452 RVA: 0x00045B81 File Offset: 0x00043D81
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static explicit operator int2x3(uint2x3 v)
		{
			return new int2x3(v);
		}

		// Token: 0x06001935 RID: 6453 RVA: 0x00045B89 File Offset: 0x00043D89
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static explicit operator int2x3(float v)
		{
			return new int2x3(v);
		}

		// Token: 0x06001936 RID: 6454 RVA: 0x00045B91 File Offset: 0x00043D91
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static explicit operator int2x3(float2x3 v)
		{
			return new int2x3(v);
		}

		// Token: 0x06001937 RID: 6455 RVA: 0x00045B99 File Offset: 0x00043D99
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static explicit operator int2x3(double v)
		{
			return new int2x3(v);
		}

		// Token: 0x06001938 RID: 6456 RVA: 0x00045BA1 File Offset: 0x00043DA1
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static explicit operator int2x3(double2x3 v)
		{
			return new int2x3(v);
		}

		// Token: 0x06001939 RID: 6457 RVA: 0x00045BA9 File Offset: 0x00043DA9
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int2x3 operator *(int2x3 lhs, int2x3 rhs)
		{
			return new int2x3(lhs.c0 * rhs.c0, lhs.c1 * rhs.c1, lhs.c2 * rhs.c2);
		}

		// Token: 0x0600193A RID: 6458 RVA: 0x00045BE3 File Offset: 0x00043DE3
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int2x3 operator *(int2x3 lhs, int rhs)
		{
			return new int2x3(lhs.c0 * rhs, lhs.c1 * rhs, lhs.c2 * rhs);
		}

		// Token: 0x0600193B RID: 6459 RVA: 0x00045C0E File Offset: 0x00043E0E
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int2x3 operator *(int lhs, int2x3 rhs)
		{
			return new int2x3(lhs * rhs.c0, lhs * rhs.c1, lhs * rhs.c2);
		}

		// Token: 0x0600193C RID: 6460 RVA: 0x00045C39 File Offset: 0x00043E39
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int2x3 operator +(int2x3 lhs, int2x3 rhs)
		{
			return new int2x3(lhs.c0 + rhs.c0, lhs.c1 + rhs.c1, lhs.c2 + rhs.c2);
		}

		// Token: 0x0600193D RID: 6461 RVA: 0x00045C73 File Offset: 0x00043E73
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int2x3 operator +(int2x3 lhs, int rhs)
		{
			return new int2x3(lhs.c0 + rhs, lhs.c1 + rhs, lhs.c2 + rhs);
		}

		// Token: 0x0600193E RID: 6462 RVA: 0x00045C9E File Offset: 0x00043E9E
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int2x3 operator +(int lhs, int2x3 rhs)
		{
			return new int2x3(lhs + rhs.c0, lhs + rhs.c1, lhs + rhs.c2);
		}

		// Token: 0x0600193F RID: 6463 RVA: 0x00045CC9 File Offset: 0x00043EC9
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int2x3 operator -(int2x3 lhs, int2x3 rhs)
		{
			return new int2x3(lhs.c0 - rhs.c0, lhs.c1 - rhs.c1, lhs.c2 - rhs.c2);
		}

		// Token: 0x06001940 RID: 6464 RVA: 0x00045D03 File Offset: 0x00043F03
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int2x3 operator -(int2x3 lhs, int rhs)
		{
			return new int2x3(lhs.c0 - rhs, lhs.c1 - rhs, lhs.c2 - rhs);
		}

		// Token: 0x06001941 RID: 6465 RVA: 0x00045D2E File Offset: 0x00043F2E
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int2x3 operator -(int lhs, int2x3 rhs)
		{
			return new int2x3(lhs - rhs.c0, lhs - rhs.c1, lhs - rhs.c2);
		}

		// Token: 0x06001942 RID: 6466 RVA: 0x00045D59 File Offset: 0x00043F59
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int2x3 operator /(int2x3 lhs, int2x3 rhs)
		{
			return new int2x3(lhs.c0 / rhs.c0, lhs.c1 / rhs.c1, lhs.c2 / rhs.c2);
		}

		// Token: 0x06001943 RID: 6467 RVA: 0x00045D93 File Offset: 0x00043F93
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int2x3 operator /(int2x3 lhs, int rhs)
		{
			return new int2x3(lhs.c0 / rhs, lhs.c1 / rhs, lhs.c2 / rhs);
		}

		// Token: 0x06001944 RID: 6468 RVA: 0x00045DBE File Offset: 0x00043FBE
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int2x3 operator /(int lhs, int2x3 rhs)
		{
			return new int2x3(lhs / rhs.c0, lhs / rhs.c1, lhs / rhs.c2);
		}

		// Token: 0x06001945 RID: 6469 RVA: 0x00045DE9 File Offset: 0x00043FE9
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int2x3 operator %(int2x3 lhs, int2x3 rhs)
		{
			return new int2x3(lhs.c0 % rhs.c0, lhs.c1 % rhs.c1, lhs.c2 % rhs.c2);
		}

		// Token: 0x06001946 RID: 6470 RVA: 0x00045E23 File Offset: 0x00044023
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int2x3 operator %(int2x3 lhs, int rhs)
		{
			return new int2x3(lhs.c0 % rhs, lhs.c1 % rhs, lhs.c2 % rhs);
		}

		// Token: 0x06001947 RID: 6471 RVA: 0x00045E4E File Offset: 0x0004404E
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int2x3 operator %(int lhs, int2x3 rhs)
		{
			return new int2x3(lhs % rhs.c0, lhs % rhs.c1, lhs % rhs.c2);
		}

		// Token: 0x06001948 RID: 6472 RVA: 0x00045E7C File Offset: 0x0004407C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int2x3 operator ++(int2x3 val)
		{
			int2 @int = int2.op_Increment(val.c0);
			val.c0 = @int;
			int2 int2 = @int;
			@int = int2.op_Increment(val.c1);
			val.c1 = @int;
			int2 int3 = @int;
			@int = int2.op_Increment(val.c2);
			val.c2 = @int;
			return new int2x3(int2, int3, @int);
		}

		// Token: 0x06001949 RID: 6473 RVA: 0x00045EDC File Offset: 0x000440DC
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int2x3 operator --(int2x3 val)
		{
			int2 @int = int2.op_Decrement(val.c0);
			val.c0 = @int;
			int2 int2 = @int;
			@int = int2.op_Decrement(val.c1);
			val.c1 = @int;
			int2 int3 = @int;
			@int = int2.op_Decrement(val.c2);
			val.c2 = @int;
			return new int2x3(int2, int3, @int);
		}

		// Token: 0x0600194A RID: 6474 RVA: 0x00045F3C File Offset: 0x0004413C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool2x3 operator <(int2x3 lhs, int2x3 rhs)
		{
			return new bool2x3(lhs.c0 < rhs.c0, lhs.c1 < rhs.c1, lhs.c2 < rhs.c2);
		}

		// Token: 0x0600194B RID: 6475 RVA: 0x00045F76 File Offset: 0x00044176
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool2x3 operator <(int2x3 lhs, int rhs)
		{
			return new bool2x3(lhs.c0 < rhs, lhs.c1 < rhs, lhs.c2 < rhs);
		}

		// Token: 0x0600194C RID: 6476 RVA: 0x00045FA1 File Offset: 0x000441A1
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool2x3 operator <(int lhs, int2x3 rhs)
		{
			return new bool2x3(lhs < rhs.c0, lhs < rhs.c1, lhs < rhs.c2);
		}

		// Token: 0x0600194D RID: 6477 RVA: 0x00045FCC File Offset: 0x000441CC
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool2x3 operator <=(int2x3 lhs, int2x3 rhs)
		{
			return new bool2x3(lhs.c0 <= rhs.c0, lhs.c1 <= rhs.c1, lhs.c2 <= rhs.c2);
		}

		// Token: 0x0600194E RID: 6478 RVA: 0x00046006 File Offset: 0x00044206
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool2x3 operator <=(int2x3 lhs, int rhs)
		{
			return new bool2x3(lhs.c0 <= rhs, lhs.c1 <= rhs, lhs.c2 <= rhs);
		}

		// Token: 0x0600194F RID: 6479 RVA: 0x00046031 File Offset: 0x00044231
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool2x3 operator <=(int lhs, int2x3 rhs)
		{
			return new bool2x3(lhs <= rhs.c0, lhs <= rhs.c1, lhs <= rhs.c2);
		}

		// Token: 0x06001950 RID: 6480 RVA: 0x0004605C File Offset: 0x0004425C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool2x3 operator >(int2x3 lhs, int2x3 rhs)
		{
			return new bool2x3(lhs.c0 > rhs.c0, lhs.c1 > rhs.c1, lhs.c2 > rhs.c2);
		}

		// Token: 0x06001951 RID: 6481 RVA: 0x00046096 File Offset: 0x00044296
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool2x3 operator >(int2x3 lhs, int rhs)
		{
			return new bool2x3(lhs.c0 > rhs, lhs.c1 > rhs, lhs.c2 > rhs);
		}

		// Token: 0x06001952 RID: 6482 RVA: 0x000460C1 File Offset: 0x000442C1
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool2x3 operator >(int lhs, int2x3 rhs)
		{
			return new bool2x3(lhs > rhs.c0, lhs > rhs.c1, lhs > rhs.c2);
		}

		// Token: 0x06001953 RID: 6483 RVA: 0x000460EC File Offset: 0x000442EC
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool2x3 operator >=(int2x3 lhs, int2x3 rhs)
		{
			return new bool2x3(lhs.c0 >= rhs.c0, lhs.c1 >= rhs.c1, lhs.c2 >= rhs.c2);
		}

		// Token: 0x06001954 RID: 6484 RVA: 0x00046126 File Offset: 0x00044326
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool2x3 operator >=(int2x3 lhs, int rhs)
		{
			return new bool2x3(lhs.c0 >= rhs, lhs.c1 >= rhs, lhs.c2 >= rhs);
		}

		// Token: 0x06001955 RID: 6485 RVA: 0x00046151 File Offset: 0x00044351
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool2x3 operator >=(int lhs, int2x3 rhs)
		{
			return new bool2x3(lhs >= rhs.c0, lhs >= rhs.c1, lhs >= rhs.c2);
		}

		// Token: 0x06001956 RID: 6486 RVA: 0x0004617C File Offset: 0x0004437C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int2x3 operator -(int2x3 val)
		{
			return new int2x3(-val.c0, -val.c1, -val.c2);
		}

		// Token: 0x06001957 RID: 6487 RVA: 0x000461A4 File Offset: 0x000443A4
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int2x3 operator +(int2x3 val)
		{
			return new int2x3(+val.c0, +val.c1, +val.c2);
		}

		// Token: 0x06001958 RID: 6488 RVA: 0x000461CC File Offset: 0x000443CC
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int2x3 operator <<(int2x3 x, int n)
		{
			return new int2x3(x.c0 << n, x.c1 << n, x.c2 << n);
		}

		// Token: 0x06001959 RID: 6489 RVA: 0x000461F7 File Offset: 0x000443F7
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int2x3 operator >>(int2x3 x, int n)
		{
			return new int2x3(x.c0 >> n, x.c1 >> n, x.c2 >> n);
		}

		// Token: 0x0600195A RID: 6490 RVA: 0x00046222 File Offset: 0x00044422
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool2x3 operator ==(int2x3 lhs, int2x3 rhs)
		{
			return new bool2x3(lhs.c0 == rhs.c0, lhs.c1 == rhs.c1, lhs.c2 == rhs.c2);
		}

		// Token: 0x0600195B RID: 6491 RVA: 0x0004625C File Offset: 0x0004445C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool2x3 operator ==(int2x3 lhs, int rhs)
		{
			return new bool2x3(lhs.c0 == rhs, lhs.c1 == rhs, lhs.c2 == rhs);
		}

		// Token: 0x0600195C RID: 6492 RVA: 0x00046287 File Offset: 0x00044487
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool2x3 operator ==(int lhs, int2x3 rhs)
		{
			return new bool2x3(lhs == rhs.c0, lhs == rhs.c1, lhs == rhs.c2);
		}

		// Token: 0x0600195D RID: 6493 RVA: 0x000462B2 File Offset: 0x000444B2
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool2x3 operator !=(int2x3 lhs, int2x3 rhs)
		{
			return new bool2x3(lhs.c0 != rhs.c0, lhs.c1 != rhs.c1, lhs.c2 != rhs.c2);
		}

		// Token: 0x0600195E RID: 6494 RVA: 0x000462EC File Offset: 0x000444EC
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool2x3 operator !=(int2x3 lhs, int rhs)
		{
			return new bool2x3(lhs.c0 != rhs, lhs.c1 != rhs, lhs.c2 != rhs);
		}

		// Token: 0x0600195F RID: 6495 RVA: 0x00046317 File Offset: 0x00044517
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool2x3 operator !=(int lhs, int2x3 rhs)
		{
			return new bool2x3(lhs != rhs.c0, lhs != rhs.c1, lhs != rhs.c2);
		}

		// Token: 0x06001960 RID: 6496 RVA: 0x00046342 File Offset: 0x00044542
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int2x3 operator ~(int2x3 val)
		{
			return new int2x3(~val.c0, ~val.c1, ~val.c2);
		}

		// Token: 0x06001961 RID: 6497 RVA: 0x0004636A File Offset: 0x0004456A
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int2x3 operator &(int2x3 lhs, int2x3 rhs)
		{
			return new int2x3(lhs.c0 & rhs.c0, lhs.c1 & rhs.c1, lhs.c2 & rhs.c2);
		}

		// Token: 0x06001962 RID: 6498 RVA: 0x000463A4 File Offset: 0x000445A4
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int2x3 operator &(int2x3 lhs, int rhs)
		{
			return new int2x3(lhs.c0 & rhs, lhs.c1 & rhs, lhs.c2 & rhs);
		}

		// Token: 0x06001963 RID: 6499 RVA: 0x000463CF File Offset: 0x000445CF
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int2x3 operator &(int lhs, int2x3 rhs)
		{
			return new int2x3(lhs & rhs.c0, lhs & rhs.c1, lhs & rhs.c2);
		}

		// Token: 0x06001964 RID: 6500 RVA: 0x000463FA File Offset: 0x000445FA
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int2x3 operator |(int2x3 lhs, int2x3 rhs)
		{
			return new int2x3(lhs.c0 | rhs.c0, lhs.c1 | rhs.c1, lhs.c2 | rhs.c2);
		}

		// Token: 0x06001965 RID: 6501 RVA: 0x00046434 File Offset: 0x00044634
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int2x3 operator |(int2x3 lhs, int rhs)
		{
			return new int2x3(lhs.c0 | rhs, lhs.c1 | rhs, lhs.c2 | rhs);
		}

		// Token: 0x06001966 RID: 6502 RVA: 0x0004645F File Offset: 0x0004465F
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int2x3 operator |(int lhs, int2x3 rhs)
		{
			return new int2x3(lhs | rhs.c0, lhs | rhs.c1, lhs | rhs.c2);
		}

		// Token: 0x06001967 RID: 6503 RVA: 0x0004648A File Offset: 0x0004468A
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int2x3 operator ^(int2x3 lhs, int2x3 rhs)
		{
			return new int2x3(lhs.c0 ^ rhs.c0, lhs.c1 ^ rhs.c1, lhs.c2 ^ rhs.c2);
		}

		// Token: 0x06001968 RID: 6504 RVA: 0x000464C4 File Offset: 0x000446C4
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int2x3 operator ^(int2x3 lhs, int rhs)
		{
			return new int2x3(lhs.c0 ^ rhs, lhs.c1 ^ rhs, lhs.c2 ^ rhs);
		}

		// Token: 0x06001969 RID: 6505 RVA: 0x000464EF File Offset: 0x000446EF
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int2x3 operator ^(int lhs, int2x3 rhs)
		{
			return new int2x3(lhs ^ rhs.c0, lhs ^ rhs.c1, lhs ^ rhs.c2);
		}

		// Token: 0x170007CE RID: 1998
		public unsafe ref int2 this[int index]
		{
			get
			{
				fixed (int2x3* ptr = &this)
				{
					return ref *(int2*)(ptr + (IntPtr)index * (IntPtr)sizeof(int2) / (IntPtr)sizeof(int2x3));
				}
			}
		}

		// Token: 0x0600196B RID: 6507 RVA: 0x00046537 File Offset: 0x00044737
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool Equals(int2x3 rhs)
		{
			return this.c0.Equals(rhs.c0) && this.c1.Equals(rhs.c1) && this.c2.Equals(rhs.c2);
		}

		// Token: 0x0600196C RID: 6508 RVA: 0x00046574 File Offset: 0x00044774
		public override bool Equals(object o)
		{
			if (o is int2x3)
			{
				int2x3 int2x = (int2x3)o;
				return this.Equals(int2x);
			}
			return false;
		}

		// Token: 0x0600196D RID: 6509 RVA: 0x00046599 File Offset: 0x00044799
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public override int GetHashCode()
		{
			return (int)math.hash(this);
		}

		// Token: 0x0600196E RID: 6510 RVA: 0x000465A8 File Offset: 0x000447A8
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public override string ToString()
		{
			return string.Format("int2x3({0}, {1}, {2},  {3}, {4}, {5})", new object[]
			{
				this.c0.x,
				this.c1.x,
				this.c2.x,
				this.c0.y,
				this.c1.y,
				this.c2.y
			});
		}

		// Token: 0x0600196F RID: 6511 RVA: 0x00046638 File Offset: 0x00044838
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public string ToString(string format, IFormatProvider formatProvider)
		{
			return string.Format("int2x3({0}, {1}, {2},  {3}, {4}, {5})", new object[]
			{
				this.c0.x.ToString(format, formatProvider),
				this.c1.x.ToString(format, formatProvider),
				this.c2.x.ToString(format, formatProvider),
				this.c0.y.ToString(format, formatProvider),
				this.c1.y.ToString(format, formatProvider),
				this.c2.y.ToString(format, formatProvider)
			});
		}

		// Token: 0x040000C4 RID: 196
		public int2 c0;

		// Token: 0x040000C5 RID: 197
		public int2 c1;

		// Token: 0x040000C6 RID: 198
		public int2 c2;

		// Token: 0x040000C7 RID: 199
		public static readonly int2x3 zero;
	}
}
