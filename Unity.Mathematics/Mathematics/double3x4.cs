using System;
using System.Runtime.CompilerServices;
using Unity.IL2CPP.CompilerServices;

namespace Unity.Mathematics
{
	// Token: 0x02000019 RID: 25
	[Il2CppEagerStaticClassConstruction]
	[Serializable]
	public struct double3x4 : IEquatable<double3x4>, IFormattable
	{
		// Token: 0x06000D8D RID: 3469 RVA: 0x000293D8 File Offset: 0x000275D8
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public double3x4(double3 c0, double3 c1, double3 c2, double3 c3)
		{
			this.c0 = c0;
			this.c1 = c1;
			this.c2 = c2;
			this.c3 = c3;
		}

		// Token: 0x06000D8E RID: 3470 RVA: 0x000293F8 File Offset: 0x000275F8
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public double3x4(double m00, double m01, double m02, double m03, double m10, double m11, double m12, double m13, double m20, double m21, double m22, double m23)
		{
			this.c0 = new double3(m00, m10, m20);
			this.c1 = new double3(m01, m11, m21);
			this.c2 = new double3(m02, m12, m22);
			this.c3 = new double3(m03, m13, m23);
		}

		// Token: 0x06000D8F RID: 3471 RVA: 0x00029446 File Offset: 0x00027646
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public double3x4(double v)
		{
			this.c0 = v;
			this.c1 = v;
			this.c2 = v;
			this.c3 = v;
		}

		// Token: 0x06000D90 RID: 3472 RVA: 0x00029478 File Offset: 0x00027678
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public double3x4(bool v)
		{
			this.c0 = math.select(new double3(0.0), new double3(1.0), v);
			this.c1 = math.select(new double3(0.0), new double3(1.0), v);
			this.c2 = math.select(new double3(0.0), new double3(1.0), v);
			this.c3 = math.select(new double3(0.0), new double3(1.0), v);
		}

		// Token: 0x06000D91 RID: 3473 RVA: 0x00029528 File Offset: 0x00027728
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public double3x4(bool3x4 v)
		{
			this.c0 = math.select(new double3(0.0), new double3(1.0), v.c0);
			this.c1 = math.select(new double3(0.0), new double3(1.0), v.c1);
			this.c2 = math.select(new double3(0.0), new double3(1.0), v.c2);
			this.c3 = math.select(new double3(0.0), new double3(1.0), v.c3);
		}

		// Token: 0x06000D92 RID: 3474 RVA: 0x000295E9 File Offset: 0x000277E9
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public double3x4(int v)
		{
			this.c0 = v;
			this.c1 = v;
			this.c2 = v;
			this.c3 = v;
		}

		// Token: 0x06000D93 RID: 3475 RVA: 0x0002961C File Offset: 0x0002781C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public double3x4(int3x4 v)
		{
			this.c0 = v.c0;
			this.c1 = v.c1;
			this.c2 = v.c2;
			this.c3 = v.c3;
		}

		// Token: 0x06000D94 RID: 3476 RVA: 0x0002966D File Offset: 0x0002786D
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public double3x4(uint v)
		{
			this.c0 = v;
			this.c1 = v;
			this.c2 = v;
			this.c3 = v;
		}

		// Token: 0x06000D95 RID: 3477 RVA: 0x000296A0 File Offset: 0x000278A0
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public double3x4(uint3x4 v)
		{
			this.c0 = v.c0;
			this.c1 = v.c1;
			this.c2 = v.c2;
			this.c3 = v.c3;
		}

		// Token: 0x06000D96 RID: 3478 RVA: 0x000296F1 File Offset: 0x000278F1
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public double3x4(float v)
		{
			this.c0 = v;
			this.c1 = v;
			this.c2 = v;
			this.c3 = v;
		}

		// Token: 0x06000D97 RID: 3479 RVA: 0x00029724 File Offset: 0x00027924
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public double3x4(float3x4 v)
		{
			this.c0 = v.c0;
			this.c1 = v.c1;
			this.c2 = v.c2;
			this.c3 = v.c3;
		}

		// Token: 0x06000D98 RID: 3480 RVA: 0x00029775 File Offset: 0x00027975
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static implicit operator double3x4(double v)
		{
			return new double3x4(v);
		}

		// Token: 0x06000D99 RID: 3481 RVA: 0x0002977D File Offset: 0x0002797D
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static explicit operator double3x4(bool v)
		{
			return new double3x4(v);
		}

		// Token: 0x06000D9A RID: 3482 RVA: 0x00029785 File Offset: 0x00027985
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static explicit operator double3x4(bool3x4 v)
		{
			return new double3x4(v);
		}

		// Token: 0x06000D9B RID: 3483 RVA: 0x0002978D File Offset: 0x0002798D
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static implicit operator double3x4(int v)
		{
			return new double3x4(v);
		}

		// Token: 0x06000D9C RID: 3484 RVA: 0x00029795 File Offset: 0x00027995
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static implicit operator double3x4(int3x4 v)
		{
			return new double3x4(v);
		}

		// Token: 0x06000D9D RID: 3485 RVA: 0x0002979D File Offset: 0x0002799D
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static implicit operator double3x4(uint v)
		{
			return new double3x4(v);
		}

		// Token: 0x06000D9E RID: 3486 RVA: 0x000297A5 File Offset: 0x000279A5
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static implicit operator double3x4(uint3x4 v)
		{
			return new double3x4(v);
		}

		// Token: 0x06000D9F RID: 3487 RVA: 0x000297AD File Offset: 0x000279AD
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static implicit operator double3x4(float v)
		{
			return new double3x4(v);
		}

		// Token: 0x06000DA0 RID: 3488 RVA: 0x000297B5 File Offset: 0x000279B5
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static implicit operator double3x4(float3x4 v)
		{
			return new double3x4(v);
		}

		// Token: 0x06000DA1 RID: 3489 RVA: 0x000297C0 File Offset: 0x000279C0
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double3x4 operator *(double3x4 lhs, double3x4 rhs)
		{
			return new double3x4(lhs.c0 * rhs.c0, lhs.c1 * rhs.c1, lhs.c2 * rhs.c2, lhs.c3 * rhs.c3);
		}

		// Token: 0x06000DA2 RID: 3490 RVA: 0x00029816 File Offset: 0x00027A16
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double3x4 operator *(double3x4 lhs, double rhs)
		{
			return new double3x4(lhs.c0 * rhs, lhs.c1 * rhs, lhs.c2 * rhs, lhs.c3 * rhs);
		}

		// Token: 0x06000DA3 RID: 3491 RVA: 0x0002984D File Offset: 0x00027A4D
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double3x4 operator *(double lhs, double3x4 rhs)
		{
			return new double3x4(lhs * rhs.c0, lhs * rhs.c1, lhs * rhs.c2, lhs * rhs.c3);
		}

		// Token: 0x06000DA4 RID: 3492 RVA: 0x00029884 File Offset: 0x00027A84
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double3x4 operator +(double3x4 lhs, double3x4 rhs)
		{
			return new double3x4(lhs.c0 + rhs.c0, lhs.c1 + rhs.c1, lhs.c2 + rhs.c2, lhs.c3 + rhs.c3);
		}

		// Token: 0x06000DA5 RID: 3493 RVA: 0x000298DA File Offset: 0x00027ADA
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double3x4 operator +(double3x4 lhs, double rhs)
		{
			return new double3x4(lhs.c0 + rhs, lhs.c1 + rhs, lhs.c2 + rhs, lhs.c3 + rhs);
		}

		// Token: 0x06000DA6 RID: 3494 RVA: 0x00029911 File Offset: 0x00027B11
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double3x4 operator +(double lhs, double3x4 rhs)
		{
			return new double3x4(lhs + rhs.c0, lhs + rhs.c1, lhs + rhs.c2, lhs + rhs.c3);
		}

		// Token: 0x06000DA7 RID: 3495 RVA: 0x00029948 File Offset: 0x00027B48
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double3x4 operator -(double3x4 lhs, double3x4 rhs)
		{
			return new double3x4(lhs.c0 - rhs.c0, lhs.c1 - rhs.c1, lhs.c2 - rhs.c2, lhs.c3 - rhs.c3);
		}

		// Token: 0x06000DA8 RID: 3496 RVA: 0x0002999E File Offset: 0x00027B9E
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double3x4 operator -(double3x4 lhs, double rhs)
		{
			return new double3x4(lhs.c0 - rhs, lhs.c1 - rhs, lhs.c2 - rhs, lhs.c3 - rhs);
		}

		// Token: 0x06000DA9 RID: 3497 RVA: 0x000299D5 File Offset: 0x00027BD5
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double3x4 operator -(double lhs, double3x4 rhs)
		{
			return new double3x4(lhs - rhs.c0, lhs - rhs.c1, lhs - rhs.c2, lhs - rhs.c3);
		}

		// Token: 0x06000DAA RID: 3498 RVA: 0x00029A0C File Offset: 0x00027C0C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double3x4 operator /(double3x4 lhs, double3x4 rhs)
		{
			return new double3x4(lhs.c0 / rhs.c0, lhs.c1 / rhs.c1, lhs.c2 / rhs.c2, lhs.c3 / rhs.c3);
		}

		// Token: 0x06000DAB RID: 3499 RVA: 0x00029A62 File Offset: 0x00027C62
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double3x4 operator /(double3x4 lhs, double rhs)
		{
			return new double3x4(lhs.c0 / rhs, lhs.c1 / rhs, lhs.c2 / rhs, lhs.c3 / rhs);
		}

		// Token: 0x06000DAC RID: 3500 RVA: 0x00029A99 File Offset: 0x00027C99
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double3x4 operator /(double lhs, double3x4 rhs)
		{
			return new double3x4(lhs / rhs.c0, lhs / rhs.c1, lhs / rhs.c2, lhs / rhs.c3);
		}

		// Token: 0x06000DAD RID: 3501 RVA: 0x00029AD0 File Offset: 0x00027CD0
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double3x4 operator %(double3x4 lhs, double3x4 rhs)
		{
			return new double3x4(lhs.c0 % rhs.c0, lhs.c1 % rhs.c1, lhs.c2 % rhs.c2, lhs.c3 % rhs.c3);
		}

		// Token: 0x06000DAE RID: 3502 RVA: 0x00029B26 File Offset: 0x00027D26
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double3x4 operator %(double3x4 lhs, double rhs)
		{
			return new double3x4(lhs.c0 % rhs, lhs.c1 % rhs, lhs.c2 % rhs, lhs.c3 % rhs);
		}

		// Token: 0x06000DAF RID: 3503 RVA: 0x00029B5D File Offset: 0x00027D5D
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double3x4 operator %(double lhs, double3x4 rhs)
		{
			return new double3x4(lhs % rhs.c0, lhs % rhs.c1, lhs % rhs.c2, lhs % rhs.c3);
		}

		// Token: 0x06000DB0 RID: 3504 RVA: 0x00029B94 File Offset: 0x00027D94
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double3x4 operator ++(double3x4 val)
		{
			double3 @double = double3.op_Increment(val.c0);
			val.c0 = @double;
			double3 double2 = @double;
			@double = double3.op_Increment(val.c1);
			val.c1 = @double;
			double3 double3 = @double;
			@double = double3.op_Increment(val.c2);
			val.c2 = @double;
			double3 double4 = @double;
			@double = double3.op_Increment(val.c3);
			val.c3 = @double;
			return new double3x4(double2, double3, double4, @double);
		}

		// Token: 0x06000DB1 RID: 3505 RVA: 0x00029C10 File Offset: 0x00027E10
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double3x4 operator --(double3x4 val)
		{
			double3 @double = double3.op_Decrement(val.c0);
			val.c0 = @double;
			double3 double2 = @double;
			@double = double3.op_Decrement(val.c1);
			val.c1 = @double;
			double3 double3 = @double;
			@double = double3.op_Decrement(val.c2);
			val.c2 = @double;
			double3 double4 = @double;
			@double = double3.op_Decrement(val.c3);
			val.c3 = @double;
			return new double3x4(double2, double3, double4, @double);
		}

		// Token: 0x06000DB2 RID: 3506 RVA: 0x00029C8C File Offset: 0x00027E8C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool3x4 operator <(double3x4 lhs, double3x4 rhs)
		{
			return new bool3x4(lhs.c0 < rhs.c0, lhs.c1 < rhs.c1, lhs.c2 < rhs.c2, lhs.c3 < rhs.c3);
		}

		// Token: 0x06000DB3 RID: 3507 RVA: 0x00029CE2 File Offset: 0x00027EE2
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool3x4 operator <(double3x4 lhs, double rhs)
		{
			return new bool3x4(lhs.c0 < rhs, lhs.c1 < rhs, lhs.c2 < rhs, lhs.c3 < rhs);
		}

		// Token: 0x06000DB4 RID: 3508 RVA: 0x00029D19 File Offset: 0x00027F19
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool3x4 operator <(double lhs, double3x4 rhs)
		{
			return new bool3x4(lhs < rhs.c0, lhs < rhs.c1, lhs < rhs.c2, lhs < rhs.c3);
		}

		// Token: 0x06000DB5 RID: 3509 RVA: 0x00029D50 File Offset: 0x00027F50
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool3x4 operator <=(double3x4 lhs, double3x4 rhs)
		{
			return new bool3x4(lhs.c0 <= rhs.c0, lhs.c1 <= rhs.c1, lhs.c2 <= rhs.c2, lhs.c3 <= rhs.c3);
		}

		// Token: 0x06000DB6 RID: 3510 RVA: 0x00029DA6 File Offset: 0x00027FA6
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool3x4 operator <=(double3x4 lhs, double rhs)
		{
			return new bool3x4(lhs.c0 <= rhs, lhs.c1 <= rhs, lhs.c2 <= rhs, lhs.c3 <= rhs);
		}

		// Token: 0x06000DB7 RID: 3511 RVA: 0x00029DDD File Offset: 0x00027FDD
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool3x4 operator <=(double lhs, double3x4 rhs)
		{
			return new bool3x4(lhs <= rhs.c0, lhs <= rhs.c1, lhs <= rhs.c2, lhs <= rhs.c3);
		}

		// Token: 0x06000DB8 RID: 3512 RVA: 0x00029E14 File Offset: 0x00028014
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool3x4 operator >(double3x4 lhs, double3x4 rhs)
		{
			return new bool3x4(lhs.c0 > rhs.c0, lhs.c1 > rhs.c1, lhs.c2 > rhs.c2, lhs.c3 > rhs.c3);
		}

		// Token: 0x06000DB9 RID: 3513 RVA: 0x00029E6A File Offset: 0x0002806A
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool3x4 operator >(double3x4 lhs, double rhs)
		{
			return new bool3x4(lhs.c0 > rhs, lhs.c1 > rhs, lhs.c2 > rhs, lhs.c3 > rhs);
		}

		// Token: 0x06000DBA RID: 3514 RVA: 0x00029EA1 File Offset: 0x000280A1
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool3x4 operator >(double lhs, double3x4 rhs)
		{
			return new bool3x4(lhs > rhs.c0, lhs > rhs.c1, lhs > rhs.c2, lhs > rhs.c3);
		}

		// Token: 0x06000DBB RID: 3515 RVA: 0x00029ED8 File Offset: 0x000280D8
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool3x4 operator >=(double3x4 lhs, double3x4 rhs)
		{
			return new bool3x4(lhs.c0 >= rhs.c0, lhs.c1 >= rhs.c1, lhs.c2 >= rhs.c2, lhs.c3 >= rhs.c3);
		}

		// Token: 0x06000DBC RID: 3516 RVA: 0x00029F2E File Offset: 0x0002812E
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool3x4 operator >=(double3x4 lhs, double rhs)
		{
			return new bool3x4(lhs.c0 >= rhs, lhs.c1 >= rhs, lhs.c2 >= rhs, lhs.c3 >= rhs);
		}

		// Token: 0x06000DBD RID: 3517 RVA: 0x00029F65 File Offset: 0x00028165
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool3x4 operator >=(double lhs, double3x4 rhs)
		{
			return new bool3x4(lhs >= rhs.c0, lhs >= rhs.c1, lhs >= rhs.c2, lhs >= rhs.c3);
		}

		// Token: 0x06000DBE RID: 3518 RVA: 0x00029F9C File Offset: 0x0002819C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double3x4 operator -(double3x4 val)
		{
			return new double3x4(-val.c0, -val.c1, -val.c2, -val.c3);
		}

		// Token: 0x06000DBF RID: 3519 RVA: 0x00029FCF File Offset: 0x000281CF
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double3x4 operator +(double3x4 val)
		{
			return new double3x4(+val.c0, +val.c1, +val.c2, +val.c3);
		}

		// Token: 0x06000DC0 RID: 3520 RVA: 0x0002A004 File Offset: 0x00028204
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool3x4 operator ==(double3x4 lhs, double3x4 rhs)
		{
			return new bool3x4(lhs.c0 == rhs.c0, lhs.c1 == rhs.c1, lhs.c2 == rhs.c2, lhs.c3 == rhs.c3);
		}

		// Token: 0x06000DC1 RID: 3521 RVA: 0x0002A05A File Offset: 0x0002825A
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool3x4 operator ==(double3x4 lhs, double rhs)
		{
			return new bool3x4(lhs.c0 == rhs, lhs.c1 == rhs, lhs.c2 == rhs, lhs.c3 == rhs);
		}

		// Token: 0x06000DC2 RID: 3522 RVA: 0x0002A091 File Offset: 0x00028291
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool3x4 operator ==(double lhs, double3x4 rhs)
		{
			return new bool3x4(lhs == rhs.c0, lhs == rhs.c1, lhs == rhs.c2, lhs == rhs.c3);
		}

		// Token: 0x06000DC3 RID: 3523 RVA: 0x0002A0C8 File Offset: 0x000282C8
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool3x4 operator !=(double3x4 lhs, double3x4 rhs)
		{
			return new bool3x4(lhs.c0 != rhs.c0, lhs.c1 != rhs.c1, lhs.c2 != rhs.c2, lhs.c3 != rhs.c3);
		}

		// Token: 0x06000DC4 RID: 3524 RVA: 0x0002A11E File Offset: 0x0002831E
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool3x4 operator !=(double3x4 lhs, double rhs)
		{
			return new bool3x4(lhs.c0 != rhs, lhs.c1 != rhs, lhs.c2 != rhs, lhs.c3 != rhs);
		}

		// Token: 0x06000DC5 RID: 3525 RVA: 0x0002A155 File Offset: 0x00028355
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool3x4 operator !=(double lhs, double3x4 rhs)
		{
			return new bool3x4(lhs != rhs.c0, lhs != rhs.c1, lhs != rhs.c2, lhs != rhs.c3);
		}

		// Token: 0x17000286 RID: 646
		public unsafe ref double3 this[int index]
		{
			get
			{
				fixed (double3x4* ptr = &this)
				{
					return ref *(double3*)(ptr + (IntPtr)index * (IntPtr)sizeof(double3) / (IntPtr)sizeof(double3x4));
				}
			}
		}

		// Token: 0x06000DC7 RID: 3527 RVA: 0x0002A1A8 File Offset: 0x000283A8
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool Equals(double3x4 rhs)
		{
			return this.c0.Equals(rhs.c0) && this.c1.Equals(rhs.c1) && this.c2.Equals(rhs.c2) && this.c3.Equals(rhs.c3);
		}

		// Token: 0x06000DC8 RID: 3528 RVA: 0x0002A204 File Offset: 0x00028404
		public override bool Equals(object o)
		{
			if (o is double3x4)
			{
				double3x4 double3x = (double3x4)o;
				return this.Equals(double3x);
			}
			return false;
		}

		// Token: 0x06000DC9 RID: 3529 RVA: 0x0002A229 File Offset: 0x00028429
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public override int GetHashCode()
		{
			return (int)math.hash(this);
		}

		// Token: 0x06000DCA RID: 3530 RVA: 0x0002A238 File Offset: 0x00028438
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public override string ToString()
		{
			return string.Format("double3x4({0}, {1}, {2}, {3},  {4}, {5}, {6}, {7},  {8}, {9}, {10}, {11})", new object[]
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

		// Token: 0x06000DCB RID: 3531 RVA: 0x0002A340 File Offset: 0x00028540
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public string ToString(string format, IFormatProvider formatProvider)
		{
			return string.Format("double3x4({0}, {1}, {2}, {3},  {4}, {5}, {6}, {7},  {8}, {9}, {10}, {11})", new object[]
			{
				this.c0.x.ToString(format, formatProvider),
				this.c1.x.ToString(format, formatProvider),
				this.c2.x.ToString(format, formatProvider),
				this.c3.x.ToString(format, formatProvider),
				this.c0.y.ToString(format, formatProvider),
				this.c1.y.ToString(format, formatProvider),
				this.c2.y.ToString(format, formatProvider),
				this.c3.y.ToString(format, formatProvider),
				this.c0.z.ToString(format, formatProvider),
				this.c1.z.ToString(format, formatProvider),
				this.c2.z.ToString(format, formatProvider),
				this.c3.z.ToString(format, formatProvider)
			});
		}

		// Token: 0x04000065 RID: 101
		public double3 c0;

		// Token: 0x04000066 RID: 102
		public double3 c1;

		// Token: 0x04000067 RID: 103
		public double3 c2;

		// Token: 0x04000068 RID: 104
		public double3 c3;

		// Token: 0x04000069 RID: 105
		public static readonly double3x4 zero;
	}
}
