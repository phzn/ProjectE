using System;
using System.Runtime.CompilerServices;
using Unity.IL2CPP.CompilerServices;

namespace Unity.Mathematics
{
	// Token: 0x02000018 RID: 24
	[Il2CppEagerStaticClassConstruction]
	[Serializable]
	public struct double3x3 : IEquatable<double3x3>, IFormattable
	{
		// Token: 0x06000D4D RID: 3405 RVA: 0x00028713 File Offset: 0x00026913
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public double3x3(double3 c0, double3 c1, double3 c2)
		{
			this.c0 = c0;
			this.c1 = c1;
			this.c2 = c2;
		}

		// Token: 0x06000D4E RID: 3406 RVA: 0x0002872A File Offset: 0x0002692A
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public double3x3(double m00, double m01, double m02, double m10, double m11, double m12, double m20, double m21, double m22)
		{
			this.c0 = new double3(m00, m10, m20);
			this.c1 = new double3(m01, m11, m21);
			this.c2 = new double3(m02, m12, m22);
		}

		// Token: 0x06000D4F RID: 3407 RVA: 0x0002875C File Offset: 0x0002695C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public double3x3(double v)
		{
			this.c0 = v;
			this.c1 = v;
			this.c2 = v;
		}

		// Token: 0x06000D50 RID: 3408 RVA: 0x00028784 File Offset: 0x00026984
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public double3x3(bool v)
		{
			this.c0 = math.select(new double3(0.0), new double3(1.0), v);
			this.c1 = math.select(new double3(0.0), new double3(1.0), v);
			this.c2 = math.select(new double3(0.0), new double3(1.0), v);
		}

		// Token: 0x06000D51 RID: 3409 RVA: 0x0002880C File Offset: 0x00026A0C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public double3x3(bool3x3 v)
		{
			this.c0 = math.select(new double3(0.0), new double3(1.0), v.c0);
			this.c1 = math.select(new double3(0.0), new double3(1.0), v.c1);
			this.c2 = math.select(new double3(0.0), new double3(1.0), v.c2);
		}

		// Token: 0x06000D52 RID: 3410 RVA: 0x000288A0 File Offset: 0x00026AA0
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public double3x3(int v)
		{
			this.c0 = v;
			this.c1 = v;
			this.c2 = v;
		}

		// Token: 0x06000D53 RID: 3411 RVA: 0x000288C6 File Offset: 0x00026AC6
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public double3x3(int3x3 v)
		{
			this.c0 = v.c0;
			this.c1 = v.c1;
			this.c2 = v.c2;
		}

		// Token: 0x06000D54 RID: 3412 RVA: 0x000288FB File Offset: 0x00026AFB
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public double3x3(uint v)
		{
			this.c0 = v;
			this.c1 = v;
			this.c2 = v;
		}

		// Token: 0x06000D55 RID: 3413 RVA: 0x00028921 File Offset: 0x00026B21
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public double3x3(uint3x3 v)
		{
			this.c0 = v.c0;
			this.c1 = v.c1;
			this.c2 = v.c2;
		}

		// Token: 0x06000D56 RID: 3414 RVA: 0x00028956 File Offset: 0x00026B56
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public double3x3(float v)
		{
			this.c0 = v;
			this.c1 = v;
			this.c2 = v;
		}

		// Token: 0x06000D57 RID: 3415 RVA: 0x0002897C File Offset: 0x00026B7C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public double3x3(float3x3 v)
		{
			this.c0 = v.c0;
			this.c1 = v.c1;
			this.c2 = v.c2;
		}

		// Token: 0x06000D58 RID: 3416 RVA: 0x000289B1 File Offset: 0x00026BB1
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static implicit operator double3x3(double v)
		{
			return new double3x3(v);
		}

		// Token: 0x06000D59 RID: 3417 RVA: 0x000289B9 File Offset: 0x00026BB9
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static explicit operator double3x3(bool v)
		{
			return new double3x3(v);
		}

		// Token: 0x06000D5A RID: 3418 RVA: 0x000289C1 File Offset: 0x00026BC1
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static explicit operator double3x3(bool3x3 v)
		{
			return new double3x3(v);
		}

		// Token: 0x06000D5B RID: 3419 RVA: 0x000289C9 File Offset: 0x00026BC9
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static implicit operator double3x3(int v)
		{
			return new double3x3(v);
		}

		// Token: 0x06000D5C RID: 3420 RVA: 0x000289D1 File Offset: 0x00026BD1
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static implicit operator double3x3(int3x3 v)
		{
			return new double3x3(v);
		}

		// Token: 0x06000D5D RID: 3421 RVA: 0x000289D9 File Offset: 0x00026BD9
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static implicit operator double3x3(uint v)
		{
			return new double3x3(v);
		}

		// Token: 0x06000D5E RID: 3422 RVA: 0x000289E1 File Offset: 0x00026BE1
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static implicit operator double3x3(uint3x3 v)
		{
			return new double3x3(v);
		}

		// Token: 0x06000D5F RID: 3423 RVA: 0x000289E9 File Offset: 0x00026BE9
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static implicit operator double3x3(float v)
		{
			return new double3x3(v);
		}

		// Token: 0x06000D60 RID: 3424 RVA: 0x000289F1 File Offset: 0x00026BF1
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static implicit operator double3x3(float3x3 v)
		{
			return new double3x3(v);
		}

		// Token: 0x06000D61 RID: 3425 RVA: 0x000289F9 File Offset: 0x00026BF9
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double3x3 operator *(double3x3 lhs, double3x3 rhs)
		{
			return new double3x3(lhs.c0 * rhs.c0, lhs.c1 * rhs.c1, lhs.c2 * rhs.c2);
		}

		// Token: 0x06000D62 RID: 3426 RVA: 0x00028A33 File Offset: 0x00026C33
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double3x3 operator *(double3x3 lhs, double rhs)
		{
			return new double3x3(lhs.c0 * rhs, lhs.c1 * rhs, lhs.c2 * rhs);
		}

		// Token: 0x06000D63 RID: 3427 RVA: 0x00028A5E File Offset: 0x00026C5E
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double3x3 operator *(double lhs, double3x3 rhs)
		{
			return new double3x3(lhs * rhs.c0, lhs * rhs.c1, lhs * rhs.c2);
		}

		// Token: 0x06000D64 RID: 3428 RVA: 0x00028A89 File Offset: 0x00026C89
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double3x3 operator +(double3x3 lhs, double3x3 rhs)
		{
			return new double3x3(lhs.c0 + rhs.c0, lhs.c1 + rhs.c1, lhs.c2 + rhs.c2);
		}

		// Token: 0x06000D65 RID: 3429 RVA: 0x00028AC3 File Offset: 0x00026CC3
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double3x3 operator +(double3x3 lhs, double rhs)
		{
			return new double3x3(lhs.c0 + rhs, lhs.c1 + rhs, lhs.c2 + rhs);
		}

		// Token: 0x06000D66 RID: 3430 RVA: 0x00028AEE File Offset: 0x00026CEE
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double3x3 operator +(double lhs, double3x3 rhs)
		{
			return new double3x3(lhs + rhs.c0, lhs + rhs.c1, lhs + rhs.c2);
		}

		// Token: 0x06000D67 RID: 3431 RVA: 0x00028B19 File Offset: 0x00026D19
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double3x3 operator -(double3x3 lhs, double3x3 rhs)
		{
			return new double3x3(lhs.c0 - rhs.c0, lhs.c1 - rhs.c1, lhs.c2 - rhs.c2);
		}

		// Token: 0x06000D68 RID: 3432 RVA: 0x00028B53 File Offset: 0x00026D53
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double3x3 operator -(double3x3 lhs, double rhs)
		{
			return new double3x3(lhs.c0 - rhs, lhs.c1 - rhs, lhs.c2 - rhs);
		}

		// Token: 0x06000D69 RID: 3433 RVA: 0x00028B7E File Offset: 0x00026D7E
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double3x3 operator -(double lhs, double3x3 rhs)
		{
			return new double3x3(lhs - rhs.c0, lhs - rhs.c1, lhs - rhs.c2);
		}

		// Token: 0x06000D6A RID: 3434 RVA: 0x00028BA9 File Offset: 0x00026DA9
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double3x3 operator /(double3x3 lhs, double3x3 rhs)
		{
			return new double3x3(lhs.c0 / rhs.c0, lhs.c1 / rhs.c1, lhs.c2 / rhs.c2);
		}

		// Token: 0x06000D6B RID: 3435 RVA: 0x00028BE3 File Offset: 0x00026DE3
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double3x3 operator /(double3x3 lhs, double rhs)
		{
			return new double3x3(lhs.c0 / rhs, lhs.c1 / rhs, lhs.c2 / rhs);
		}

		// Token: 0x06000D6C RID: 3436 RVA: 0x00028C0E File Offset: 0x00026E0E
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double3x3 operator /(double lhs, double3x3 rhs)
		{
			return new double3x3(lhs / rhs.c0, lhs / rhs.c1, lhs / rhs.c2);
		}

		// Token: 0x06000D6D RID: 3437 RVA: 0x00028C39 File Offset: 0x00026E39
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double3x3 operator %(double3x3 lhs, double3x3 rhs)
		{
			return new double3x3(lhs.c0 % rhs.c0, lhs.c1 % rhs.c1, lhs.c2 % rhs.c2);
		}

		// Token: 0x06000D6E RID: 3438 RVA: 0x00028C73 File Offset: 0x00026E73
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double3x3 operator %(double3x3 lhs, double rhs)
		{
			return new double3x3(lhs.c0 % rhs, lhs.c1 % rhs, lhs.c2 % rhs);
		}

		// Token: 0x06000D6F RID: 3439 RVA: 0x00028C9E File Offset: 0x00026E9E
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double3x3 operator %(double lhs, double3x3 rhs)
		{
			return new double3x3(lhs % rhs.c0, lhs % rhs.c1, lhs % rhs.c2);
		}

		// Token: 0x06000D70 RID: 3440 RVA: 0x00028CCC File Offset: 0x00026ECC
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double3x3 operator ++(double3x3 val)
		{
			double3 @double = double3.op_Increment(val.c0);
			val.c0 = @double;
			double3 double2 = @double;
			@double = double3.op_Increment(val.c1);
			val.c1 = @double;
			double3 double3 = @double;
			@double = double3.op_Increment(val.c2);
			val.c2 = @double;
			return new double3x3(double2, double3, @double);
		}

		// Token: 0x06000D71 RID: 3441 RVA: 0x00028D2C File Offset: 0x00026F2C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double3x3 operator --(double3x3 val)
		{
			double3 @double = double3.op_Decrement(val.c0);
			val.c0 = @double;
			double3 double2 = @double;
			@double = double3.op_Decrement(val.c1);
			val.c1 = @double;
			double3 double3 = @double;
			@double = double3.op_Decrement(val.c2);
			val.c2 = @double;
			return new double3x3(double2, double3, @double);
		}

		// Token: 0x06000D72 RID: 3442 RVA: 0x00028D8C File Offset: 0x00026F8C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool3x3 operator <(double3x3 lhs, double3x3 rhs)
		{
			return new bool3x3(lhs.c0 < rhs.c0, lhs.c1 < rhs.c1, lhs.c2 < rhs.c2);
		}

		// Token: 0x06000D73 RID: 3443 RVA: 0x00028DC6 File Offset: 0x00026FC6
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool3x3 operator <(double3x3 lhs, double rhs)
		{
			return new bool3x3(lhs.c0 < rhs, lhs.c1 < rhs, lhs.c2 < rhs);
		}

		// Token: 0x06000D74 RID: 3444 RVA: 0x00028DF1 File Offset: 0x00026FF1
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool3x3 operator <(double lhs, double3x3 rhs)
		{
			return new bool3x3(lhs < rhs.c0, lhs < rhs.c1, lhs < rhs.c2);
		}

		// Token: 0x06000D75 RID: 3445 RVA: 0x00028E1C File Offset: 0x0002701C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool3x3 operator <=(double3x3 lhs, double3x3 rhs)
		{
			return new bool3x3(lhs.c0 <= rhs.c0, lhs.c1 <= rhs.c1, lhs.c2 <= rhs.c2);
		}

		// Token: 0x06000D76 RID: 3446 RVA: 0x00028E56 File Offset: 0x00027056
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool3x3 operator <=(double3x3 lhs, double rhs)
		{
			return new bool3x3(lhs.c0 <= rhs, lhs.c1 <= rhs, lhs.c2 <= rhs);
		}

		// Token: 0x06000D77 RID: 3447 RVA: 0x00028E81 File Offset: 0x00027081
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool3x3 operator <=(double lhs, double3x3 rhs)
		{
			return new bool3x3(lhs <= rhs.c0, lhs <= rhs.c1, lhs <= rhs.c2);
		}

		// Token: 0x06000D78 RID: 3448 RVA: 0x00028EAC File Offset: 0x000270AC
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool3x3 operator >(double3x3 lhs, double3x3 rhs)
		{
			return new bool3x3(lhs.c0 > rhs.c0, lhs.c1 > rhs.c1, lhs.c2 > rhs.c2);
		}

		// Token: 0x06000D79 RID: 3449 RVA: 0x00028EE6 File Offset: 0x000270E6
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool3x3 operator >(double3x3 lhs, double rhs)
		{
			return new bool3x3(lhs.c0 > rhs, lhs.c1 > rhs, lhs.c2 > rhs);
		}

		// Token: 0x06000D7A RID: 3450 RVA: 0x00028F11 File Offset: 0x00027111
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool3x3 operator >(double lhs, double3x3 rhs)
		{
			return new bool3x3(lhs > rhs.c0, lhs > rhs.c1, lhs > rhs.c2);
		}

		// Token: 0x06000D7B RID: 3451 RVA: 0x00028F3C File Offset: 0x0002713C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool3x3 operator >=(double3x3 lhs, double3x3 rhs)
		{
			return new bool3x3(lhs.c0 >= rhs.c0, lhs.c1 >= rhs.c1, lhs.c2 >= rhs.c2);
		}

		// Token: 0x06000D7C RID: 3452 RVA: 0x00028F76 File Offset: 0x00027176
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool3x3 operator >=(double3x3 lhs, double rhs)
		{
			return new bool3x3(lhs.c0 >= rhs, lhs.c1 >= rhs, lhs.c2 >= rhs);
		}

		// Token: 0x06000D7D RID: 3453 RVA: 0x00028FA1 File Offset: 0x000271A1
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool3x3 operator >=(double lhs, double3x3 rhs)
		{
			return new bool3x3(lhs >= rhs.c0, lhs >= rhs.c1, lhs >= rhs.c2);
		}

		// Token: 0x06000D7E RID: 3454 RVA: 0x00028FCC File Offset: 0x000271CC
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double3x3 operator -(double3x3 val)
		{
			return new double3x3(-val.c0, -val.c1, -val.c2);
		}

		// Token: 0x06000D7F RID: 3455 RVA: 0x00028FF4 File Offset: 0x000271F4
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double3x3 operator +(double3x3 val)
		{
			return new double3x3(+val.c0, +val.c1, +val.c2);
		}

		// Token: 0x06000D80 RID: 3456 RVA: 0x0002901C File Offset: 0x0002721C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool3x3 operator ==(double3x3 lhs, double3x3 rhs)
		{
			return new bool3x3(lhs.c0 == rhs.c0, lhs.c1 == rhs.c1, lhs.c2 == rhs.c2);
		}

		// Token: 0x06000D81 RID: 3457 RVA: 0x00029056 File Offset: 0x00027256
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool3x3 operator ==(double3x3 lhs, double rhs)
		{
			return new bool3x3(lhs.c0 == rhs, lhs.c1 == rhs, lhs.c2 == rhs);
		}

		// Token: 0x06000D82 RID: 3458 RVA: 0x00029081 File Offset: 0x00027281
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool3x3 operator ==(double lhs, double3x3 rhs)
		{
			return new bool3x3(lhs == rhs.c0, lhs == rhs.c1, lhs == rhs.c2);
		}

		// Token: 0x06000D83 RID: 3459 RVA: 0x000290AC File Offset: 0x000272AC
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool3x3 operator !=(double3x3 lhs, double3x3 rhs)
		{
			return new bool3x3(lhs.c0 != rhs.c0, lhs.c1 != rhs.c1, lhs.c2 != rhs.c2);
		}

		// Token: 0x06000D84 RID: 3460 RVA: 0x000290E6 File Offset: 0x000272E6
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool3x3 operator !=(double3x3 lhs, double rhs)
		{
			return new bool3x3(lhs.c0 != rhs, lhs.c1 != rhs, lhs.c2 != rhs);
		}

		// Token: 0x06000D85 RID: 3461 RVA: 0x00029111 File Offset: 0x00027311
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool3x3 operator !=(double lhs, double3x3 rhs)
		{
			return new bool3x3(lhs != rhs.c0, lhs != rhs.c1, lhs != rhs.c2);
		}

		// Token: 0x17000285 RID: 645
		public unsafe ref double3 this[int index]
		{
			get
			{
				fixed (double3x3* ptr = &this)
				{
					return ref *(double3*)(ptr + (IntPtr)index * (IntPtr)sizeof(double3) / (IntPtr)sizeof(double3x3));
				}
			}
		}

		// Token: 0x06000D87 RID: 3463 RVA: 0x00029157 File Offset: 0x00027357
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool Equals(double3x3 rhs)
		{
			return this.c0.Equals(rhs.c0) && this.c1.Equals(rhs.c1) && this.c2.Equals(rhs.c2);
		}

		// Token: 0x06000D88 RID: 3464 RVA: 0x00029194 File Offset: 0x00027394
		public override bool Equals(object o)
		{
			if (o is double3x3)
			{
				double3x3 double3x = (double3x3)o;
				return this.Equals(double3x);
			}
			return false;
		}

		// Token: 0x06000D89 RID: 3465 RVA: 0x000291B9 File Offset: 0x000273B9
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public override int GetHashCode()
		{
			return (int)math.hash(this);
		}

		// Token: 0x06000D8A RID: 3466 RVA: 0x000291C8 File Offset: 0x000273C8
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public override string ToString()
		{
			return string.Format("double3x3({0}, {1}, {2},  {3}, {4}, {5},  {6}, {7}, {8})", new object[]
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

		// Token: 0x06000D8B RID: 3467 RVA: 0x00029294 File Offset: 0x00027494
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public string ToString(string format, IFormatProvider formatProvider)
		{
			return string.Format("double3x3({0}, {1}, {2},  {3}, {4}, {5},  {6}, {7}, {8})", new object[]
			{
				this.c0.x.ToString(format, formatProvider),
				this.c1.x.ToString(format, formatProvider),
				this.c2.x.ToString(format, formatProvider),
				this.c0.y.ToString(format, formatProvider),
				this.c1.y.ToString(format, formatProvider),
				this.c2.y.ToString(format, formatProvider),
				this.c0.z.ToString(format, formatProvider),
				this.c1.z.ToString(format, formatProvider),
				this.c2.z.ToString(format, formatProvider)
			});
		}

		// Token: 0x04000060 RID: 96
		public double3 c0;

		// Token: 0x04000061 RID: 97
		public double3 c1;

		// Token: 0x04000062 RID: 98
		public double3 c2;

		// Token: 0x04000063 RID: 99
		public static readonly double3x3 identity = new double3x3(1.0, 0.0, 0.0, 0.0, 1.0, 0.0, 0.0, 0.0, 1.0);

		// Token: 0x04000064 RID: 100
		public static readonly double3x3 zero;
	}
}
