using System;
using System.Runtime.CompilerServices;
using Unity.IL2CPP.CompilerServices;

namespace Unity.Mathematics
{
	// Token: 0x02000017 RID: 23
	[Il2CppEagerStaticClassConstruction]
	[Serializable]
	public struct double3x2 : IEquatable<double3x2>, IFormattable
	{
		// Token: 0x06000D0E RID: 3342 RVA: 0x00027E22 File Offset: 0x00026022
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public double3x2(double3 c0, double3 c1)
		{
			this.c0 = c0;
			this.c1 = c1;
		}

		// Token: 0x06000D0F RID: 3343 RVA: 0x00027E32 File Offset: 0x00026032
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public double3x2(double m00, double m01, double m10, double m11, double m20, double m21)
		{
			this.c0 = new double3(m00, m10, m20);
			this.c1 = new double3(m01, m11, m21);
		}

		// Token: 0x06000D10 RID: 3344 RVA: 0x00027E53 File Offset: 0x00026053
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public double3x2(double v)
		{
			this.c0 = v;
			this.c1 = v;
		}

		// Token: 0x06000D11 RID: 3345 RVA: 0x00027E70 File Offset: 0x00026070
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public double3x2(bool v)
		{
			this.c0 = math.select(new double3(0.0), new double3(1.0), v);
			this.c1 = math.select(new double3(0.0), new double3(1.0), v);
		}

		// Token: 0x06000D12 RID: 3346 RVA: 0x00027ED0 File Offset: 0x000260D0
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public double3x2(bool3x2 v)
		{
			this.c0 = math.select(new double3(0.0), new double3(1.0), v.c0);
			this.c1 = math.select(new double3(0.0), new double3(1.0), v.c1);
		}

		// Token: 0x06000D13 RID: 3347 RVA: 0x00027F37 File Offset: 0x00026137
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public double3x2(int v)
		{
			this.c0 = v;
			this.c1 = v;
		}

		// Token: 0x06000D14 RID: 3348 RVA: 0x00027F51 File Offset: 0x00026151
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public double3x2(int3x2 v)
		{
			this.c0 = v.c0;
			this.c1 = v.c1;
		}

		// Token: 0x06000D15 RID: 3349 RVA: 0x00027F75 File Offset: 0x00026175
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public double3x2(uint v)
		{
			this.c0 = v;
			this.c1 = v;
		}

		// Token: 0x06000D16 RID: 3350 RVA: 0x00027F8F File Offset: 0x0002618F
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public double3x2(uint3x2 v)
		{
			this.c0 = v.c0;
			this.c1 = v.c1;
		}

		// Token: 0x06000D17 RID: 3351 RVA: 0x00027FB3 File Offset: 0x000261B3
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public double3x2(float v)
		{
			this.c0 = v;
			this.c1 = v;
		}

		// Token: 0x06000D18 RID: 3352 RVA: 0x00027FCD File Offset: 0x000261CD
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public double3x2(float3x2 v)
		{
			this.c0 = v.c0;
			this.c1 = v.c1;
		}

		// Token: 0x06000D19 RID: 3353 RVA: 0x00027FF1 File Offset: 0x000261F1
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static implicit operator double3x2(double v)
		{
			return new double3x2(v);
		}

		// Token: 0x06000D1A RID: 3354 RVA: 0x00027FF9 File Offset: 0x000261F9
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static explicit operator double3x2(bool v)
		{
			return new double3x2(v);
		}

		// Token: 0x06000D1B RID: 3355 RVA: 0x00028001 File Offset: 0x00026201
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static explicit operator double3x2(bool3x2 v)
		{
			return new double3x2(v);
		}

		// Token: 0x06000D1C RID: 3356 RVA: 0x00028009 File Offset: 0x00026209
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static implicit operator double3x2(int v)
		{
			return new double3x2(v);
		}

		// Token: 0x06000D1D RID: 3357 RVA: 0x00028011 File Offset: 0x00026211
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static implicit operator double3x2(int3x2 v)
		{
			return new double3x2(v);
		}

		// Token: 0x06000D1E RID: 3358 RVA: 0x00028019 File Offset: 0x00026219
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static implicit operator double3x2(uint v)
		{
			return new double3x2(v);
		}

		// Token: 0x06000D1F RID: 3359 RVA: 0x00028021 File Offset: 0x00026221
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static implicit operator double3x2(uint3x2 v)
		{
			return new double3x2(v);
		}

		// Token: 0x06000D20 RID: 3360 RVA: 0x00028029 File Offset: 0x00026229
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static implicit operator double3x2(float v)
		{
			return new double3x2(v);
		}

		// Token: 0x06000D21 RID: 3361 RVA: 0x00028031 File Offset: 0x00026231
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static implicit operator double3x2(float3x2 v)
		{
			return new double3x2(v);
		}

		// Token: 0x06000D22 RID: 3362 RVA: 0x00028039 File Offset: 0x00026239
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double3x2 operator *(double3x2 lhs, double3x2 rhs)
		{
			return new double3x2(lhs.c0 * rhs.c0, lhs.c1 * rhs.c1);
		}

		// Token: 0x06000D23 RID: 3363 RVA: 0x00028062 File Offset: 0x00026262
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double3x2 operator *(double3x2 lhs, double rhs)
		{
			return new double3x2(lhs.c0 * rhs, lhs.c1 * rhs);
		}

		// Token: 0x06000D24 RID: 3364 RVA: 0x00028081 File Offset: 0x00026281
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double3x2 operator *(double lhs, double3x2 rhs)
		{
			return new double3x2(lhs * rhs.c0, lhs * rhs.c1);
		}

		// Token: 0x06000D25 RID: 3365 RVA: 0x000280A0 File Offset: 0x000262A0
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double3x2 operator +(double3x2 lhs, double3x2 rhs)
		{
			return new double3x2(lhs.c0 + rhs.c0, lhs.c1 + rhs.c1);
		}

		// Token: 0x06000D26 RID: 3366 RVA: 0x000280C9 File Offset: 0x000262C9
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double3x2 operator +(double3x2 lhs, double rhs)
		{
			return new double3x2(lhs.c0 + rhs, lhs.c1 + rhs);
		}

		// Token: 0x06000D27 RID: 3367 RVA: 0x000280E8 File Offset: 0x000262E8
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double3x2 operator +(double lhs, double3x2 rhs)
		{
			return new double3x2(lhs + rhs.c0, lhs + rhs.c1);
		}

		// Token: 0x06000D28 RID: 3368 RVA: 0x00028107 File Offset: 0x00026307
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double3x2 operator -(double3x2 lhs, double3x2 rhs)
		{
			return new double3x2(lhs.c0 - rhs.c0, lhs.c1 - rhs.c1);
		}

		// Token: 0x06000D29 RID: 3369 RVA: 0x00028130 File Offset: 0x00026330
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double3x2 operator -(double3x2 lhs, double rhs)
		{
			return new double3x2(lhs.c0 - rhs, lhs.c1 - rhs);
		}

		// Token: 0x06000D2A RID: 3370 RVA: 0x0002814F File Offset: 0x0002634F
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double3x2 operator -(double lhs, double3x2 rhs)
		{
			return new double3x2(lhs - rhs.c0, lhs - rhs.c1);
		}

		// Token: 0x06000D2B RID: 3371 RVA: 0x0002816E File Offset: 0x0002636E
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double3x2 operator /(double3x2 lhs, double3x2 rhs)
		{
			return new double3x2(lhs.c0 / rhs.c0, lhs.c1 / rhs.c1);
		}

		// Token: 0x06000D2C RID: 3372 RVA: 0x00028197 File Offset: 0x00026397
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double3x2 operator /(double3x2 lhs, double rhs)
		{
			return new double3x2(lhs.c0 / rhs, lhs.c1 / rhs);
		}

		// Token: 0x06000D2D RID: 3373 RVA: 0x000281B6 File Offset: 0x000263B6
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double3x2 operator /(double lhs, double3x2 rhs)
		{
			return new double3x2(lhs / rhs.c0, lhs / rhs.c1);
		}

		// Token: 0x06000D2E RID: 3374 RVA: 0x000281D5 File Offset: 0x000263D5
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double3x2 operator %(double3x2 lhs, double3x2 rhs)
		{
			return new double3x2(lhs.c0 % rhs.c0, lhs.c1 % rhs.c1);
		}

		// Token: 0x06000D2F RID: 3375 RVA: 0x000281FE File Offset: 0x000263FE
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double3x2 operator %(double3x2 lhs, double rhs)
		{
			return new double3x2(lhs.c0 % rhs, lhs.c1 % rhs);
		}

		// Token: 0x06000D30 RID: 3376 RVA: 0x0002821D File Offset: 0x0002641D
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double3x2 operator %(double lhs, double3x2 rhs)
		{
			return new double3x2(lhs % rhs.c0, lhs % rhs.c1);
		}

		// Token: 0x06000D31 RID: 3377 RVA: 0x0002823C File Offset: 0x0002643C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double3x2 operator ++(double3x2 val)
		{
			double3 @double = double3.op_Increment(val.c0);
			val.c0 = @double;
			double3 double2 = @double;
			@double = double3.op_Increment(val.c1);
			val.c1 = @double;
			return new double3x2(double2, @double);
		}

		// Token: 0x06000D32 RID: 3378 RVA: 0x00028284 File Offset: 0x00026484
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double3x2 operator --(double3x2 val)
		{
			double3 @double = double3.op_Decrement(val.c0);
			val.c0 = @double;
			double3 double2 = @double;
			@double = double3.op_Decrement(val.c1);
			val.c1 = @double;
			return new double3x2(double2, @double);
		}

		// Token: 0x06000D33 RID: 3379 RVA: 0x000282CA File Offset: 0x000264CA
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool3x2 operator <(double3x2 lhs, double3x2 rhs)
		{
			return new bool3x2(lhs.c0 < rhs.c0, lhs.c1 < rhs.c1);
		}

		// Token: 0x06000D34 RID: 3380 RVA: 0x000282F3 File Offset: 0x000264F3
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool3x2 operator <(double3x2 lhs, double rhs)
		{
			return new bool3x2(lhs.c0 < rhs, lhs.c1 < rhs);
		}

		// Token: 0x06000D35 RID: 3381 RVA: 0x00028312 File Offset: 0x00026512
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool3x2 operator <(double lhs, double3x2 rhs)
		{
			return new bool3x2(lhs < rhs.c0, lhs < rhs.c1);
		}

		// Token: 0x06000D36 RID: 3382 RVA: 0x00028331 File Offset: 0x00026531
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool3x2 operator <=(double3x2 lhs, double3x2 rhs)
		{
			return new bool3x2(lhs.c0 <= rhs.c0, lhs.c1 <= rhs.c1);
		}

		// Token: 0x06000D37 RID: 3383 RVA: 0x0002835A File Offset: 0x0002655A
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool3x2 operator <=(double3x2 lhs, double rhs)
		{
			return new bool3x2(lhs.c0 <= rhs, lhs.c1 <= rhs);
		}

		// Token: 0x06000D38 RID: 3384 RVA: 0x00028379 File Offset: 0x00026579
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool3x2 operator <=(double lhs, double3x2 rhs)
		{
			return new bool3x2(lhs <= rhs.c0, lhs <= rhs.c1);
		}

		// Token: 0x06000D39 RID: 3385 RVA: 0x00028398 File Offset: 0x00026598
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool3x2 operator >(double3x2 lhs, double3x2 rhs)
		{
			return new bool3x2(lhs.c0 > rhs.c0, lhs.c1 > rhs.c1);
		}

		// Token: 0x06000D3A RID: 3386 RVA: 0x000283C1 File Offset: 0x000265C1
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool3x2 operator >(double3x2 lhs, double rhs)
		{
			return new bool3x2(lhs.c0 > rhs, lhs.c1 > rhs);
		}

		// Token: 0x06000D3B RID: 3387 RVA: 0x000283E0 File Offset: 0x000265E0
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool3x2 operator >(double lhs, double3x2 rhs)
		{
			return new bool3x2(lhs > rhs.c0, lhs > rhs.c1);
		}

		// Token: 0x06000D3C RID: 3388 RVA: 0x000283FF File Offset: 0x000265FF
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool3x2 operator >=(double3x2 lhs, double3x2 rhs)
		{
			return new bool3x2(lhs.c0 >= rhs.c0, lhs.c1 >= rhs.c1);
		}

		// Token: 0x06000D3D RID: 3389 RVA: 0x00028428 File Offset: 0x00026628
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool3x2 operator >=(double3x2 lhs, double rhs)
		{
			return new bool3x2(lhs.c0 >= rhs, lhs.c1 >= rhs);
		}

		// Token: 0x06000D3E RID: 3390 RVA: 0x00028447 File Offset: 0x00026647
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool3x2 operator >=(double lhs, double3x2 rhs)
		{
			return new bool3x2(lhs >= rhs.c0, lhs >= rhs.c1);
		}

		// Token: 0x06000D3F RID: 3391 RVA: 0x00028466 File Offset: 0x00026666
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double3x2 operator -(double3x2 val)
		{
			return new double3x2(-val.c0, -val.c1);
		}

		// Token: 0x06000D40 RID: 3392 RVA: 0x00028483 File Offset: 0x00026683
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double3x2 operator +(double3x2 val)
		{
			return new double3x2(+val.c0, +val.c1);
		}

		// Token: 0x06000D41 RID: 3393 RVA: 0x000284A0 File Offset: 0x000266A0
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool3x2 operator ==(double3x2 lhs, double3x2 rhs)
		{
			return new bool3x2(lhs.c0 == rhs.c0, lhs.c1 == rhs.c1);
		}

		// Token: 0x06000D42 RID: 3394 RVA: 0x000284C9 File Offset: 0x000266C9
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool3x2 operator ==(double3x2 lhs, double rhs)
		{
			return new bool3x2(lhs.c0 == rhs, lhs.c1 == rhs);
		}

		// Token: 0x06000D43 RID: 3395 RVA: 0x000284E8 File Offset: 0x000266E8
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool3x2 operator ==(double lhs, double3x2 rhs)
		{
			return new bool3x2(lhs == rhs.c0, lhs == rhs.c1);
		}

		// Token: 0x06000D44 RID: 3396 RVA: 0x00028507 File Offset: 0x00026707
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool3x2 operator !=(double3x2 lhs, double3x2 rhs)
		{
			return new bool3x2(lhs.c0 != rhs.c0, lhs.c1 != rhs.c1);
		}

		// Token: 0x06000D45 RID: 3397 RVA: 0x00028530 File Offset: 0x00026730
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool3x2 operator !=(double3x2 lhs, double rhs)
		{
			return new bool3x2(lhs.c0 != rhs, lhs.c1 != rhs);
		}

		// Token: 0x06000D46 RID: 3398 RVA: 0x0002854F File Offset: 0x0002674F
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool3x2 operator !=(double lhs, double3x2 rhs)
		{
			return new bool3x2(lhs != rhs.c0, lhs != rhs.c1);
		}

		// Token: 0x17000284 RID: 644
		public unsafe ref double3 this[int index]
		{
			get
			{
				fixed (double3x2* ptr = &this)
				{
					return ref *(double3*)(ptr + (IntPtr)index * (IntPtr)sizeof(double3) / (IntPtr)sizeof(double3x2));
				}
			}
		}

		// Token: 0x06000D48 RID: 3400 RVA: 0x0002858B File Offset: 0x0002678B
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool Equals(double3x2 rhs)
		{
			return this.c0.Equals(rhs.c0) && this.c1.Equals(rhs.c1);
		}

		// Token: 0x06000D49 RID: 3401 RVA: 0x000285B4 File Offset: 0x000267B4
		public override bool Equals(object o)
		{
			if (o is double3x2)
			{
				double3x2 double3x = (double3x2)o;
				return this.Equals(double3x);
			}
			return false;
		}

		// Token: 0x06000D4A RID: 3402 RVA: 0x000285D9 File Offset: 0x000267D9
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public override int GetHashCode()
		{
			return (int)math.hash(this);
		}

		// Token: 0x06000D4B RID: 3403 RVA: 0x000285E8 File Offset: 0x000267E8
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public override string ToString()
		{
			return string.Format("double3x2({0}, {1},  {2}, {3},  {4}, {5})", new object[]
			{
				this.c0.x,
				this.c1.x,
				this.c0.y,
				this.c1.y,
				this.c0.z,
				this.c1.z
			});
		}

		// Token: 0x06000D4C RID: 3404 RVA: 0x00028678 File Offset: 0x00026878
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public string ToString(string format, IFormatProvider formatProvider)
		{
			return string.Format("double3x2({0}, {1},  {2}, {3},  {4}, {5})", new object[]
			{
				this.c0.x.ToString(format, formatProvider),
				this.c1.x.ToString(format, formatProvider),
				this.c0.y.ToString(format, formatProvider),
				this.c1.y.ToString(format, formatProvider),
				this.c0.z.ToString(format, formatProvider),
				this.c1.z.ToString(format, formatProvider)
			});
		}

		// Token: 0x0400005D RID: 93
		public double3 c0;

		// Token: 0x0400005E RID: 94
		public double3 c1;

		// Token: 0x0400005F RID: 95
		public static readonly double3x2 zero;
	}
}
