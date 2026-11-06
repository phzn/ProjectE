using System;
using System.Runtime.CompilerServices;
using Unity.IL2CPP.CompilerServices;

namespace Unity.Mathematics
{
	// Token: 0x02000014 RID: 20
	[Il2CppEagerStaticClassConstruction]
	[Serializable]
	public struct double2x3 : IEquatable<double2x3>, IFormattable
	{
		// Token: 0x06000BC9 RID: 3017 RVA: 0x000249E5 File Offset: 0x00022BE5
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public double2x3(double2 c0, double2 c1, double2 c2)
		{
			this.c0 = c0;
			this.c1 = c1;
			this.c2 = c2;
		}

		// Token: 0x06000BCA RID: 3018 RVA: 0x000249FC File Offset: 0x00022BFC
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public double2x3(double m00, double m01, double m02, double m10, double m11, double m12)
		{
			this.c0 = new double2(m00, m10);
			this.c1 = new double2(m01, m11);
			this.c2 = new double2(m02, m12);
		}

		// Token: 0x06000BCB RID: 3019 RVA: 0x00024A28 File Offset: 0x00022C28
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public double2x3(double v)
		{
			this.c0 = v;
			this.c1 = v;
			this.c2 = v;
		}

		// Token: 0x06000BCC RID: 3020 RVA: 0x00024A50 File Offset: 0x00022C50
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public double2x3(bool v)
		{
			this.c0 = math.select(new double2(0.0), new double2(1.0), v);
			this.c1 = math.select(new double2(0.0), new double2(1.0), v);
			this.c2 = math.select(new double2(0.0), new double2(1.0), v);
		}

		// Token: 0x06000BCD RID: 3021 RVA: 0x00024AD8 File Offset: 0x00022CD8
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public double2x3(bool2x3 v)
		{
			this.c0 = math.select(new double2(0.0), new double2(1.0), v.c0);
			this.c1 = math.select(new double2(0.0), new double2(1.0), v.c1);
			this.c2 = math.select(new double2(0.0), new double2(1.0), v.c2);
		}

		// Token: 0x06000BCE RID: 3022 RVA: 0x00024B6C File Offset: 0x00022D6C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public double2x3(int v)
		{
			this.c0 = v;
			this.c1 = v;
			this.c2 = v;
		}

		// Token: 0x06000BCF RID: 3023 RVA: 0x00024B92 File Offset: 0x00022D92
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public double2x3(int2x3 v)
		{
			this.c0 = v.c0;
			this.c1 = v.c1;
			this.c2 = v.c2;
		}

		// Token: 0x06000BD0 RID: 3024 RVA: 0x00024BC7 File Offset: 0x00022DC7
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public double2x3(uint v)
		{
			this.c0 = v;
			this.c1 = v;
			this.c2 = v;
		}

		// Token: 0x06000BD1 RID: 3025 RVA: 0x00024BED File Offset: 0x00022DED
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public double2x3(uint2x3 v)
		{
			this.c0 = v.c0;
			this.c1 = v.c1;
			this.c2 = v.c2;
		}

		// Token: 0x06000BD2 RID: 3026 RVA: 0x00024C22 File Offset: 0x00022E22
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public double2x3(float v)
		{
			this.c0 = v;
			this.c1 = v;
			this.c2 = v;
		}

		// Token: 0x06000BD3 RID: 3027 RVA: 0x00024C48 File Offset: 0x00022E48
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public double2x3(float2x3 v)
		{
			this.c0 = v.c0;
			this.c1 = v.c1;
			this.c2 = v.c2;
		}

		// Token: 0x06000BD4 RID: 3028 RVA: 0x00024C7D File Offset: 0x00022E7D
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static implicit operator double2x3(double v)
		{
			return new double2x3(v);
		}

		// Token: 0x06000BD5 RID: 3029 RVA: 0x00024C85 File Offset: 0x00022E85
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static explicit operator double2x3(bool v)
		{
			return new double2x3(v);
		}

		// Token: 0x06000BD6 RID: 3030 RVA: 0x00024C8D File Offset: 0x00022E8D
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static explicit operator double2x3(bool2x3 v)
		{
			return new double2x3(v);
		}

		// Token: 0x06000BD7 RID: 3031 RVA: 0x00024C95 File Offset: 0x00022E95
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static implicit operator double2x3(int v)
		{
			return new double2x3(v);
		}

		// Token: 0x06000BD8 RID: 3032 RVA: 0x00024C9D File Offset: 0x00022E9D
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static implicit operator double2x3(int2x3 v)
		{
			return new double2x3(v);
		}

		// Token: 0x06000BD9 RID: 3033 RVA: 0x00024CA5 File Offset: 0x00022EA5
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static implicit operator double2x3(uint v)
		{
			return new double2x3(v);
		}

		// Token: 0x06000BDA RID: 3034 RVA: 0x00024CAD File Offset: 0x00022EAD
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static implicit operator double2x3(uint2x3 v)
		{
			return new double2x3(v);
		}

		// Token: 0x06000BDB RID: 3035 RVA: 0x00024CB5 File Offset: 0x00022EB5
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static implicit operator double2x3(float v)
		{
			return new double2x3(v);
		}

		// Token: 0x06000BDC RID: 3036 RVA: 0x00024CBD File Offset: 0x00022EBD
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static implicit operator double2x3(float2x3 v)
		{
			return new double2x3(v);
		}

		// Token: 0x06000BDD RID: 3037 RVA: 0x00024CC5 File Offset: 0x00022EC5
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double2x3 operator *(double2x3 lhs, double2x3 rhs)
		{
			return new double2x3(lhs.c0 * rhs.c0, lhs.c1 * rhs.c1, lhs.c2 * rhs.c2);
		}

		// Token: 0x06000BDE RID: 3038 RVA: 0x00024CFF File Offset: 0x00022EFF
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double2x3 operator *(double2x3 lhs, double rhs)
		{
			return new double2x3(lhs.c0 * rhs, lhs.c1 * rhs, lhs.c2 * rhs);
		}

		// Token: 0x06000BDF RID: 3039 RVA: 0x00024D2A File Offset: 0x00022F2A
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double2x3 operator *(double lhs, double2x3 rhs)
		{
			return new double2x3(lhs * rhs.c0, lhs * rhs.c1, lhs * rhs.c2);
		}

		// Token: 0x06000BE0 RID: 3040 RVA: 0x00024D55 File Offset: 0x00022F55
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double2x3 operator +(double2x3 lhs, double2x3 rhs)
		{
			return new double2x3(lhs.c0 + rhs.c0, lhs.c1 + rhs.c1, lhs.c2 + rhs.c2);
		}

		// Token: 0x06000BE1 RID: 3041 RVA: 0x00024D8F File Offset: 0x00022F8F
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double2x3 operator +(double2x3 lhs, double rhs)
		{
			return new double2x3(lhs.c0 + rhs, lhs.c1 + rhs, lhs.c2 + rhs);
		}

		// Token: 0x06000BE2 RID: 3042 RVA: 0x00024DBA File Offset: 0x00022FBA
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double2x3 operator +(double lhs, double2x3 rhs)
		{
			return new double2x3(lhs + rhs.c0, lhs + rhs.c1, lhs + rhs.c2);
		}

		// Token: 0x06000BE3 RID: 3043 RVA: 0x00024DE5 File Offset: 0x00022FE5
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double2x3 operator -(double2x3 lhs, double2x3 rhs)
		{
			return new double2x3(lhs.c0 - rhs.c0, lhs.c1 - rhs.c1, lhs.c2 - rhs.c2);
		}

		// Token: 0x06000BE4 RID: 3044 RVA: 0x00024E1F File Offset: 0x0002301F
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double2x3 operator -(double2x3 lhs, double rhs)
		{
			return new double2x3(lhs.c0 - rhs, lhs.c1 - rhs, lhs.c2 - rhs);
		}

		// Token: 0x06000BE5 RID: 3045 RVA: 0x00024E4A File Offset: 0x0002304A
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double2x3 operator -(double lhs, double2x3 rhs)
		{
			return new double2x3(lhs - rhs.c0, lhs - rhs.c1, lhs - rhs.c2);
		}

		// Token: 0x06000BE6 RID: 3046 RVA: 0x00024E75 File Offset: 0x00023075
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double2x3 operator /(double2x3 lhs, double2x3 rhs)
		{
			return new double2x3(lhs.c0 / rhs.c0, lhs.c1 / rhs.c1, lhs.c2 / rhs.c2);
		}

		// Token: 0x06000BE7 RID: 3047 RVA: 0x00024EAF File Offset: 0x000230AF
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double2x3 operator /(double2x3 lhs, double rhs)
		{
			return new double2x3(lhs.c0 / rhs, lhs.c1 / rhs, lhs.c2 / rhs);
		}

		// Token: 0x06000BE8 RID: 3048 RVA: 0x00024EDA File Offset: 0x000230DA
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double2x3 operator /(double lhs, double2x3 rhs)
		{
			return new double2x3(lhs / rhs.c0, lhs / rhs.c1, lhs / rhs.c2);
		}

		// Token: 0x06000BE9 RID: 3049 RVA: 0x00024F05 File Offset: 0x00023105
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double2x3 operator %(double2x3 lhs, double2x3 rhs)
		{
			return new double2x3(lhs.c0 % rhs.c0, lhs.c1 % rhs.c1, lhs.c2 % rhs.c2);
		}

		// Token: 0x06000BEA RID: 3050 RVA: 0x00024F3F File Offset: 0x0002313F
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double2x3 operator %(double2x3 lhs, double rhs)
		{
			return new double2x3(lhs.c0 % rhs, lhs.c1 % rhs, lhs.c2 % rhs);
		}

		// Token: 0x06000BEB RID: 3051 RVA: 0x00024F6A File Offset: 0x0002316A
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double2x3 operator %(double lhs, double2x3 rhs)
		{
			return new double2x3(lhs % rhs.c0, lhs % rhs.c1, lhs % rhs.c2);
		}

		// Token: 0x06000BEC RID: 3052 RVA: 0x00024F98 File Offset: 0x00023198
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double2x3 operator ++(double2x3 val)
		{
			double2 @double = double2.op_Increment(val.c0);
			val.c0 = @double;
			double2 double2 = @double;
			@double = double2.op_Increment(val.c1);
			val.c1 = @double;
			double2 double3 = @double;
			@double = double2.op_Increment(val.c2);
			val.c2 = @double;
			return new double2x3(double2, double3, @double);
		}

		// Token: 0x06000BED RID: 3053 RVA: 0x00024FF8 File Offset: 0x000231F8
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double2x3 operator --(double2x3 val)
		{
			double2 @double = double2.op_Decrement(val.c0);
			val.c0 = @double;
			double2 double2 = @double;
			@double = double2.op_Decrement(val.c1);
			val.c1 = @double;
			double2 double3 = @double;
			@double = double2.op_Decrement(val.c2);
			val.c2 = @double;
			return new double2x3(double2, double3, @double);
		}

		// Token: 0x06000BEE RID: 3054 RVA: 0x00025058 File Offset: 0x00023258
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool2x3 operator <(double2x3 lhs, double2x3 rhs)
		{
			return new bool2x3(lhs.c0 < rhs.c0, lhs.c1 < rhs.c1, lhs.c2 < rhs.c2);
		}

		// Token: 0x06000BEF RID: 3055 RVA: 0x00025092 File Offset: 0x00023292
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool2x3 operator <(double2x3 lhs, double rhs)
		{
			return new bool2x3(lhs.c0 < rhs, lhs.c1 < rhs, lhs.c2 < rhs);
		}

		// Token: 0x06000BF0 RID: 3056 RVA: 0x000250BD File Offset: 0x000232BD
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool2x3 operator <(double lhs, double2x3 rhs)
		{
			return new bool2x3(lhs < rhs.c0, lhs < rhs.c1, lhs < rhs.c2);
		}

		// Token: 0x06000BF1 RID: 3057 RVA: 0x000250E8 File Offset: 0x000232E8
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool2x3 operator <=(double2x3 lhs, double2x3 rhs)
		{
			return new bool2x3(lhs.c0 <= rhs.c0, lhs.c1 <= rhs.c1, lhs.c2 <= rhs.c2);
		}

		// Token: 0x06000BF2 RID: 3058 RVA: 0x00025122 File Offset: 0x00023322
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool2x3 operator <=(double2x3 lhs, double rhs)
		{
			return new bool2x3(lhs.c0 <= rhs, lhs.c1 <= rhs, lhs.c2 <= rhs);
		}

		// Token: 0x06000BF3 RID: 3059 RVA: 0x0002514D File Offset: 0x0002334D
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool2x3 operator <=(double lhs, double2x3 rhs)
		{
			return new bool2x3(lhs <= rhs.c0, lhs <= rhs.c1, lhs <= rhs.c2);
		}

		// Token: 0x06000BF4 RID: 3060 RVA: 0x00025178 File Offset: 0x00023378
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool2x3 operator >(double2x3 lhs, double2x3 rhs)
		{
			return new bool2x3(lhs.c0 > rhs.c0, lhs.c1 > rhs.c1, lhs.c2 > rhs.c2);
		}

		// Token: 0x06000BF5 RID: 3061 RVA: 0x000251B2 File Offset: 0x000233B2
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool2x3 operator >(double2x3 lhs, double rhs)
		{
			return new bool2x3(lhs.c0 > rhs, lhs.c1 > rhs, lhs.c2 > rhs);
		}

		// Token: 0x06000BF6 RID: 3062 RVA: 0x000251DD File Offset: 0x000233DD
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool2x3 operator >(double lhs, double2x3 rhs)
		{
			return new bool2x3(lhs > rhs.c0, lhs > rhs.c1, lhs > rhs.c2);
		}

		// Token: 0x06000BF7 RID: 3063 RVA: 0x00025208 File Offset: 0x00023408
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool2x3 operator >=(double2x3 lhs, double2x3 rhs)
		{
			return new bool2x3(lhs.c0 >= rhs.c0, lhs.c1 >= rhs.c1, lhs.c2 >= rhs.c2);
		}

		// Token: 0x06000BF8 RID: 3064 RVA: 0x00025242 File Offset: 0x00023442
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool2x3 operator >=(double2x3 lhs, double rhs)
		{
			return new bool2x3(lhs.c0 >= rhs, lhs.c1 >= rhs, lhs.c2 >= rhs);
		}

		// Token: 0x06000BF9 RID: 3065 RVA: 0x0002526D File Offset: 0x0002346D
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool2x3 operator >=(double lhs, double2x3 rhs)
		{
			return new bool2x3(lhs >= rhs.c0, lhs >= rhs.c1, lhs >= rhs.c2);
		}

		// Token: 0x06000BFA RID: 3066 RVA: 0x00025298 File Offset: 0x00023498
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double2x3 operator -(double2x3 val)
		{
			return new double2x3(-val.c0, -val.c1, -val.c2);
		}

		// Token: 0x06000BFB RID: 3067 RVA: 0x000252C0 File Offset: 0x000234C0
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double2x3 operator +(double2x3 val)
		{
			return new double2x3(+val.c0, +val.c1, +val.c2);
		}

		// Token: 0x06000BFC RID: 3068 RVA: 0x000252E8 File Offset: 0x000234E8
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool2x3 operator ==(double2x3 lhs, double2x3 rhs)
		{
			return new bool2x3(lhs.c0 == rhs.c0, lhs.c1 == rhs.c1, lhs.c2 == rhs.c2);
		}

		// Token: 0x06000BFD RID: 3069 RVA: 0x00025322 File Offset: 0x00023522
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool2x3 operator ==(double2x3 lhs, double rhs)
		{
			return new bool2x3(lhs.c0 == rhs, lhs.c1 == rhs, lhs.c2 == rhs);
		}

		// Token: 0x06000BFE RID: 3070 RVA: 0x0002534D File Offset: 0x0002354D
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool2x3 operator ==(double lhs, double2x3 rhs)
		{
			return new bool2x3(lhs == rhs.c0, lhs == rhs.c1, lhs == rhs.c2);
		}

		// Token: 0x06000BFF RID: 3071 RVA: 0x00025378 File Offset: 0x00023578
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool2x3 operator !=(double2x3 lhs, double2x3 rhs)
		{
			return new bool2x3(lhs.c0 != rhs.c0, lhs.c1 != rhs.c1, lhs.c2 != rhs.c2);
		}

		// Token: 0x06000C00 RID: 3072 RVA: 0x000253B2 File Offset: 0x000235B2
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool2x3 operator !=(double2x3 lhs, double rhs)
		{
			return new bool2x3(lhs.c0 != rhs, lhs.c1 != rhs, lhs.c2 != rhs);
		}

		// Token: 0x06000C01 RID: 3073 RVA: 0x000253DD File Offset: 0x000235DD
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool2x3 operator !=(double lhs, double2x3 rhs)
		{
			return new bool2x3(lhs != rhs.c0, lhs != rhs.c1, lhs != rhs.c2);
		}

		// Token: 0x1700020C RID: 524
		public unsafe ref double2 this[int index]
		{
			get
			{
				fixed (double2x3* ptr = &this)
				{
					return ref *(double2*)(ptr + (IntPtr)index * (IntPtr)sizeof(double2) / (IntPtr)sizeof(double2x3));
				}
			}
		}

		// Token: 0x06000C03 RID: 3075 RVA: 0x00025423 File Offset: 0x00023623
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool Equals(double2x3 rhs)
		{
			return this.c0.Equals(rhs.c0) && this.c1.Equals(rhs.c1) && this.c2.Equals(rhs.c2);
		}

		// Token: 0x06000C04 RID: 3076 RVA: 0x00025460 File Offset: 0x00023660
		public override bool Equals(object o)
		{
			if (o is double2x3)
			{
				double2x3 double2x = (double2x3)o;
				return this.Equals(double2x);
			}
			return false;
		}

		// Token: 0x06000C05 RID: 3077 RVA: 0x00025485 File Offset: 0x00023685
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public override int GetHashCode()
		{
			return (int)math.hash(this);
		}

		// Token: 0x06000C06 RID: 3078 RVA: 0x00025494 File Offset: 0x00023694
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public override string ToString()
		{
			return string.Format("double2x3({0}, {1}, {2},  {3}, {4}, {5})", new object[]
			{
				this.c0.x,
				this.c1.x,
				this.c2.x,
				this.c0.y,
				this.c1.y,
				this.c2.y
			});
		}

		// Token: 0x06000C07 RID: 3079 RVA: 0x00025524 File Offset: 0x00023724
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public string ToString(string format, IFormatProvider formatProvider)
		{
			return string.Format("double2x3({0}, {1}, {2},  {3}, {4}, {5})", new object[]
			{
				this.c0.x.ToString(format, formatProvider),
				this.c1.x.ToString(format, formatProvider),
				this.c2.x.ToString(format, formatProvider),
				this.c0.y.ToString(format, formatProvider),
				this.c1.y.ToString(format, formatProvider),
				this.c2.y.ToString(format, formatProvider)
			});
		}

		// Token: 0x04000050 RID: 80
		public double2 c0;

		// Token: 0x04000051 RID: 81
		public double2 c1;

		// Token: 0x04000052 RID: 82
		public double2 c2;

		// Token: 0x04000053 RID: 83
		public static readonly double2x3 zero;
	}
}
