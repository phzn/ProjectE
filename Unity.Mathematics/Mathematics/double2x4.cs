using System;
using System.Runtime.CompilerServices;
using Unity.IL2CPP.CompilerServices;

namespace Unity.Mathematics
{
	// Token: 0x02000015 RID: 21
	[Il2CppEagerStaticClassConstruction]
	[Serializable]
	public struct double2x4 : IEquatable<double2x4>, IFormattable
	{
		// Token: 0x06000C08 RID: 3080 RVA: 0x000255BF File Offset: 0x000237BF
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public double2x4(double2 c0, double2 c1, double2 c2, double2 c3)
		{
			this.c0 = c0;
			this.c1 = c1;
			this.c2 = c2;
			this.c3 = c3;
		}

		// Token: 0x06000C09 RID: 3081 RVA: 0x000255DE File Offset: 0x000237DE
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public double2x4(double m00, double m01, double m02, double m03, double m10, double m11, double m12, double m13)
		{
			this.c0 = new double2(m00, m10);
			this.c1 = new double2(m01, m11);
			this.c2 = new double2(m02, m12);
			this.c3 = new double2(m03, m13);
		}

		// Token: 0x06000C0A RID: 3082 RVA: 0x00025619 File Offset: 0x00023819
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public double2x4(double v)
		{
			this.c0 = v;
			this.c1 = v;
			this.c2 = v;
			this.c3 = v;
		}

		// Token: 0x06000C0B RID: 3083 RVA: 0x0002564C File Offset: 0x0002384C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public double2x4(bool v)
		{
			this.c0 = math.select(new double2(0.0), new double2(1.0), v);
			this.c1 = math.select(new double2(0.0), new double2(1.0), v);
			this.c2 = math.select(new double2(0.0), new double2(1.0), v);
			this.c3 = math.select(new double2(0.0), new double2(1.0), v);
		}

		// Token: 0x06000C0C RID: 3084 RVA: 0x000256FC File Offset: 0x000238FC
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public double2x4(bool2x4 v)
		{
			this.c0 = math.select(new double2(0.0), new double2(1.0), v.c0);
			this.c1 = math.select(new double2(0.0), new double2(1.0), v.c1);
			this.c2 = math.select(new double2(0.0), new double2(1.0), v.c2);
			this.c3 = math.select(new double2(0.0), new double2(1.0), v.c3);
		}

		// Token: 0x06000C0D RID: 3085 RVA: 0x000257BD File Offset: 0x000239BD
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public double2x4(int v)
		{
			this.c0 = v;
			this.c1 = v;
			this.c2 = v;
			this.c3 = v;
		}

		// Token: 0x06000C0E RID: 3086 RVA: 0x000257F0 File Offset: 0x000239F0
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public double2x4(int2x4 v)
		{
			this.c0 = v.c0;
			this.c1 = v.c1;
			this.c2 = v.c2;
			this.c3 = v.c3;
		}

		// Token: 0x06000C0F RID: 3087 RVA: 0x00025841 File Offset: 0x00023A41
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public double2x4(uint v)
		{
			this.c0 = v;
			this.c1 = v;
			this.c2 = v;
			this.c3 = v;
		}

		// Token: 0x06000C10 RID: 3088 RVA: 0x00025874 File Offset: 0x00023A74
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public double2x4(uint2x4 v)
		{
			this.c0 = v.c0;
			this.c1 = v.c1;
			this.c2 = v.c2;
			this.c3 = v.c3;
		}

		// Token: 0x06000C11 RID: 3089 RVA: 0x000258C5 File Offset: 0x00023AC5
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public double2x4(float v)
		{
			this.c0 = v;
			this.c1 = v;
			this.c2 = v;
			this.c3 = v;
		}

		// Token: 0x06000C12 RID: 3090 RVA: 0x000258F8 File Offset: 0x00023AF8
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public double2x4(float2x4 v)
		{
			this.c0 = v.c0;
			this.c1 = v.c1;
			this.c2 = v.c2;
			this.c3 = v.c3;
		}

		// Token: 0x06000C13 RID: 3091 RVA: 0x00025949 File Offset: 0x00023B49
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static implicit operator double2x4(double v)
		{
			return new double2x4(v);
		}

		// Token: 0x06000C14 RID: 3092 RVA: 0x00025951 File Offset: 0x00023B51
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static explicit operator double2x4(bool v)
		{
			return new double2x4(v);
		}

		// Token: 0x06000C15 RID: 3093 RVA: 0x00025959 File Offset: 0x00023B59
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static explicit operator double2x4(bool2x4 v)
		{
			return new double2x4(v);
		}

		// Token: 0x06000C16 RID: 3094 RVA: 0x00025961 File Offset: 0x00023B61
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static implicit operator double2x4(int v)
		{
			return new double2x4(v);
		}

		// Token: 0x06000C17 RID: 3095 RVA: 0x00025969 File Offset: 0x00023B69
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static implicit operator double2x4(int2x4 v)
		{
			return new double2x4(v);
		}

		// Token: 0x06000C18 RID: 3096 RVA: 0x00025971 File Offset: 0x00023B71
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static implicit operator double2x4(uint v)
		{
			return new double2x4(v);
		}

		// Token: 0x06000C19 RID: 3097 RVA: 0x00025979 File Offset: 0x00023B79
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static implicit operator double2x4(uint2x4 v)
		{
			return new double2x4(v);
		}

		// Token: 0x06000C1A RID: 3098 RVA: 0x00025981 File Offset: 0x00023B81
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static implicit operator double2x4(float v)
		{
			return new double2x4(v);
		}

		// Token: 0x06000C1B RID: 3099 RVA: 0x00025989 File Offset: 0x00023B89
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static implicit operator double2x4(float2x4 v)
		{
			return new double2x4(v);
		}

		// Token: 0x06000C1C RID: 3100 RVA: 0x00025994 File Offset: 0x00023B94
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double2x4 operator *(double2x4 lhs, double2x4 rhs)
		{
			return new double2x4(lhs.c0 * rhs.c0, lhs.c1 * rhs.c1, lhs.c2 * rhs.c2, lhs.c3 * rhs.c3);
		}

		// Token: 0x06000C1D RID: 3101 RVA: 0x000259EA File Offset: 0x00023BEA
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double2x4 operator *(double2x4 lhs, double rhs)
		{
			return new double2x4(lhs.c0 * rhs, lhs.c1 * rhs, lhs.c2 * rhs, lhs.c3 * rhs);
		}

		// Token: 0x06000C1E RID: 3102 RVA: 0x00025A21 File Offset: 0x00023C21
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double2x4 operator *(double lhs, double2x4 rhs)
		{
			return new double2x4(lhs * rhs.c0, lhs * rhs.c1, lhs * rhs.c2, lhs * rhs.c3);
		}

		// Token: 0x06000C1F RID: 3103 RVA: 0x00025A58 File Offset: 0x00023C58
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double2x4 operator +(double2x4 lhs, double2x4 rhs)
		{
			return new double2x4(lhs.c0 + rhs.c0, lhs.c1 + rhs.c1, lhs.c2 + rhs.c2, lhs.c3 + rhs.c3);
		}

		// Token: 0x06000C20 RID: 3104 RVA: 0x00025AAE File Offset: 0x00023CAE
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double2x4 operator +(double2x4 lhs, double rhs)
		{
			return new double2x4(lhs.c0 + rhs, lhs.c1 + rhs, lhs.c2 + rhs, lhs.c3 + rhs);
		}

		// Token: 0x06000C21 RID: 3105 RVA: 0x00025AE5 File Offset: 0x00023CE5
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double2x4 operator +(double lhs, double2x4 rhs)
		{
			return new double2x4(lhs + rhs.c0, lhs + rhs.c1, lhs + rhs.c2, lhs + rhs.c3);
		}

		// Token: 0x06000C22 RID: 3106 RVA: 0x00025B1C File Offset: 0x00023D1C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double2x4 operator -(double2x4 lhs, double2x4 rhs)
		{
			return new double2x4(lhs.c0 - rhs.c0, lhs.c1 - rhs.c1, lhs.c2 - rhs.c2, lhs.c3 - rhs.c3);
		}

		// Token: 0x06000C23 RID: 3107 RVA: 0x00025B72 File Offset: 0x00023D72
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double2x4 operator -(double2x4 lhs, double rhs)
		{
			return new double2x4(lhs.c0 - rhs, lhs.c1 - rhs, lhs.c2 - rhs, lhs.c3 - rhs);
		}

		// Token: 0x06000C24 RID: 3108 RVA: 0x00025BA9 File Offset: 0x00023DA9
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double2x4 operator -(double lhs, double2x4 rhs)
		{
			return new double2x4(lhs - rhs.c0, lhs - rhs.c1, lhs - rhs.c2, lhs - rhs.c3);
		}

		// Token: 0x06000C25 RID: 3109 RVA: 0x00025BE0 File Offset: 0x00023DE0
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double2x4 operator /(double2x4 lhs, double2x4 rhs)
		{
			return new double2x4(lhs.c0 / rhs.c0, lhs.c1 / rhs.c1, lhs.c2 / rhs.c2, lhs.c3 / rhs.c3);
		}

		// Token: 0x06000C26 RID: 3110 RVA: 0x00025C36 File Offset: 0x00023E36
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double2x4 operator /(double2x4 lhs, double rhs)
		{
			return new double2x4(lhs.c0 / rhs, lhs.c1 / rhs, lhs.c2 / rhs, lhs.c3 / rhs);
		}

		// Token: 0x06000C27 RID: 3111 RVA: 0x00025C6D File Offset: 0x00023E6D
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double2x4 operator /(double lhs, double2x4 rhs)
		{
			return new double2x4(lhs / rhs.c0, lhs / rhs.c1, lhs / rhs.c2, lhs / rhs.c3);
		}

		// Token: 0x06000C28 RID: 3112 RVA: 0x00025CA4 File Offset: 0x00023EA4
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double2x4 operator %(double2x4 lhs, double2x4 rhs)
		{
			return new double2x4(lhs.c0 % rhs.c0, lhs.c1 % rhs.c1, lhs.c2 % rhs.c2, lhs.c3 % rhs.c3);
		}

		// Token: 0x06000C29 RID: 3113 RVA: 0x00025CFA File Offset: 0x00023EFA
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double2x4 operator %(double2x4 lhs, double rhs)
		{
			return new double2x4(lhs.c0 % rhs, lhs.c1 % rhs, lhs.c2 % rhs, lhs.c3 % rhs);
		}

		// Token: 0x06000C2A RID: 3114 RVA: 0x00025D31 File Offset: 0x00023F31
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double2x4 operator %(double lhs, double2x4 rhs)
		{
			return new double2x4(lhs % rhs.c0, lhs % rhs.c1, lhs % rhs.c2, lhs % rhs.c3);
		}

		// Token: 0x06000C2B RID: 3115 RVA: 0x00025D68 File Offset: 0x00023F68
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double2x4 operator ++(double2x4 val)
		{
			double2 @double = double2.op_Increment(val.c0);
			val.c0 = @double;
			double2 double2 = @double;
			@double = double2.op_Increment(val.c1);
			val.c1 = @double;
			double2 double3 = @double;
			@double = double2.op_Increment(val.c2);
			val.c2 = @double;
			double2 double4 = @double;
			@double = double2.op_Increment(val.c3);
			val.c3 = @double;
			return new double2x4(double2, double3, double4, @double);
		}

		// Token: 0x06000C2C RID: 3116 RVA: 0x00025DE4 File Offset: 0x00023FE4
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double2x4 operator --(double2x4 val)
		{
			double2 @double = double2.op_Decrement(val.c0);
			val.c0 = @double;
			double2 double2 = @double;
			@double = double2.op_Decrement(val.c1);
			val.c1 = @double;
			double2 double3 = @double;
			@double = double2.op_Decrement(val.c2);
			val.c2 = @double;
			double2 double4 = @double;
			@double = double2.op_Decrement(val.c3);
			val.c3 = @double;
			return new double2x4(double2, double3, double4, @double);
		}

		// Token: 0x06000C2D RID: 3117 RVA: 0x00025E60 File Offset: 0x00024060
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool2x4 operator <(double2x4 lhs, double2x4 rhs)
		{
			return new bool2x4(lhs.c0 < rhs.c0, lhs.c1 < rhs.c1, lhs.c2 < rhs.c2, lhs.c3 < rhs.c3);
		}

		// Token: 0x06000C2E RID: 3118 RVA: 0x00025EB6 File Offset: 0x000240B6
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool2x4 operator <(double2x4 lhs, double rhs)
		{
			return new bool2x4(lhs.c0 < rhs, lhs.c1 < rhs, lhs.c2 < rhs, lhs.c3 < rhs);
		}

		// Token: 0x06000C2F RID: 3119 RVA: 0x00025EED File Offset: 0x000240ED
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool2x4 operator <(double lhs, double2x4 rhs)
		{
			return new bool2x4(lhs < rhs.c0, lhs < rhs.c1, lhs < rhs.c2, lhs < rhs.c3);
		}

		// Token: 0x06000C30 RID: 3120 RVA: 0x00025F24 File Offset: 0x00024124
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool2x4 operator <=(double2x4 lhs, double2x4 rhs)
		{
			return new bool2x4(lhs.c0 <= rhs.c0, lhs.c1 <= rhs.c1, lhs.c2 <= rhs.c2, lhs.c3 <= rhs.c3);
		}

		// Token: 0x06000C31 RID: 3121 RVA: 0x00025F7A File Offset: 0x0002417A
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool2x4 operator <=(double2x4 lhs, double rhs)
		{
			return new bool2x4(lhs.c0 <= rhs, lhs.c1 <= rhs, lhs.c2 <= rhs, lhs.c3 <= rhs);
		}

		// Token: 0x06000C32 RID: 3122 RVA: 0x00025FB1 File Offset: 0x000241B1
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool2x4 operator <=(double lhs, double2x4 rhs)
		{
			return new bool2x4(lhs <= rhs.c0, lhs <= rhs.c1, lhs <= rhs.c2, lhs <= rhs.c3);
		}

		// Token: 0x06000C33 RID: 3123 RVA: 0x00025FE8 File Offset: 0x000241E8
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool2x4 operator >(double2x4 lhs, double2x4 rhs)
		{
			return new bool2x4(lhs.c0 > rhs.c0, lhs.c1 > rhs.c1, lhs.c2 > rhs.c2, lhs.c3 > rhs.c3);
		}

		// Token: 0x06000C34 RID: 3124 RVA: 0x0002603E File Offset: 0x0002423E
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool2x4 operator >(double2x4 lhs, double rhs)
		{
			return new bool2x4(lhs.c0 > rhs, lhs.c1 > rhs, lhs.c2 > rhs, lhs.c3 > rhs);
		}

		// Token: 0x06000C35 RID: 3125 RVA: 0x00026075 File Offset: 0x00024275
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool2x4 operator >(double lhs, double2x4 rhs)
		{
			return new bool2x4(lhs > rhs.c0, lhs > rhs.c1, lhs > rhs.c2, lhs > rhs.c3);
		}

		// Token: 0x06000C36 RID: 3126 RVA: 0x000260AC File Offset: 0x000242AC
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool2x4 operator >=(double2x4 lhs, double2x4 rhs)
		{
			return new bool2x4(lhs.c0 >= rhs.c0, lhs.c1 >= rhs.c1, lhs.c2 >= rhs.c2, lhs.c3 >= rhs.c3);
		}

		// Token: 0x06000C37 RID: 3127 RVA: 0x00026102 File Offset: 0x00024302
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool2x4 operator >=(double2x4 lhs, double rhs)
		{
			return new bool2x4(lhs.c0 >= rhs, lhs.c1 >= rhs, lhs.c2 >= rhs, lhs.c3 >= rhs);
		}

		// Token: 0x06000C38 RID: 3128 RVA: 0x00026139 File Offset: 0x00024339
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool2x4 operator >=(double lhs, double2x4 rhs)
		{
			return new bool2x4(lhs >= rhs.c0, lhs >= rhs.c1, lhs >= rhs.c2, lhs >= rhs.c3);
		}

		// Token: 0x06000C39 RID: 3129 RVA: 0x00026170 File Offset: 0x00024370
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double2x4 operator -(double2x4 val)
		{
			return new double2x4(-val.c0, -val.c1, -val.c2, -val.c3);
		}

		// Token: 0x06000C3A RID: 3130 RVA: 0x000261A3 File Offset: 0x000243A3
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double2x4 operator +(double2x4 val)
		{
			return new double2x4(+val.c0, +val.c1, +val.c2, +val.c3);
		}

		// Token: 0x06000C3B RID: 3131 RVA: 0x000261D8 File Offset: 0x000243D8
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool2x4 operator ==(double2x4 lhs, double2x4 rhs)
		{
			return new bool2x4(lhs.c0 == rhs.c0, lhs.c1 == rhs.c1, lhs.c2 == rhs.c2, lhs.c3 == rhs.c3);
		}

		// Token: 0x06000C3C RID: 3132 RVA: 0x0002622E File Offset: 0x0002442E
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool2x4 operator ==(double2x4 lhs, double rhs)
		{
			return new bool2x4(lhs.c0 == rhs, lhs.c1 == rhs, lhs.c2 == rhs, lhs.c3 == rhs);
		}

		// Token: 0x06000C3D RID: 3133 RVA: 0x00026265 File Offset: 0x00024465
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool2x4 operator ==(double lhs, double2x4 rhs)
		{
			return new bool2x4(lhs == rhs.c0, lhs == rhs.c1, lhs == rhs.c2, lhs == rhs.c3);
		}

		// Token: 0x06000C3E RID: 3134 RVA: 0x0002629C File Offset: 0x0002449C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool2x4 operator !=(double2x4 lhs, double2x4 rhs)
		{
			return new bool2x4(lhs.c0 != rhs.c0, lhs.c1 != rhs.c1, lhs.c2 != rhs.c2, lhs.c3 != rhs.c3);
		}

		// Token: 0x06000C3F RID: 3135 RVA: 0x000262F2 File Offset: 0x000244F2
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool2x4 operator !=(double2x4 lhs, double rhs)
		{
			return new bool2x4(lhs.c0 != rhs, lhs.c1 != rhs, lhs.c2 != rhs, lhs.c3 != rhs);
		}

		// Token: 0x06000C40 RID: 3136 RVA: 0x00026329 File Offset: 0x00024529
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool2x4 operator !=(double lhs, double2x4 rhs)
		{
			return new bool2x4(lhs != rhs.c0, lhs != rhs.c1, lhs != rhs.c2, lhs != rhs.c3);
		}

		// Token: 0x1700020D RID: 525
		public unsafe ref double2 this[int index]
		{
			get
			{
				fixed (double2x4* ptr = &this)
				{
					return ref *(double2*)(ptr + (IntPtr)index * (IntPtr)sizeof(double2) / (IntPtr)sizeof(double2x4));
				}
			}
		}

		// Token: 0x06000C42 RID: 3138 RVA: 0x0002637C File Offset: 0x0002457C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool Equals(double2x4 rhs)
		{
			return this.c0.Equals(rhs.c0) && this.c1.Equals(rhs.c1) && this.c2.Equals(rhs.c2) && this.c3.Equals(rhs.c3);
		}

		// Token: 0x06000C43 RID: 3139 RVA: 0x000263D8 File Offset: 0x000245D8
		public override bool Equals(object o)
		{
			if (o is double2x4)
			{
				double2x4 double2x = (double2x4)o;
				return this.Equals(double2x);
			}
			return false;
		}

		// Token: 0x06000C44 RID: 3140 RVA: 0x000263FD File Offset: 0x000245FD
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public override int GetHashCode()
		{
			return (int)math.hash(this);
		}

		// Token: 0x06000C45 RID: 3141 RVA: 0x0002640C File Offset: 0x0002460C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public override string ToString()
		{
			return string.Format("double2x4({0}, {1}, {2}, {3},  {4}, {5}, {6}, {7})", new object[]
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

		// Token: 0x06000C46 RID: 3142 RVA: 0x000264C4 File Offset: 0x000246C4
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public string ToString(string format, IFormatProvider formatProvider)
		{
			return string.Format("double2x4({0}, {1}, {2}, {3},  {4}, {5}, {6}, {7})", new object[]
			{
				this.c0.x.ToString(format, formatProvider),
				this.c1.x.ToString(format, formatProvider),
				this.c2.x.ToString(format, formatProvider),
				this.c3.x.ToString(format, formatProvider),
				this.c0.y.ToString(format, formatProvider),
				this.c1.y.ToString(format, formatProvider),
				this.c2.y.ToString(format, formatProvider),
				this.c3.y.ToString(format, formatProvider)
			});
		}

		// Token: 0x04000054 RID: 84
		public double2 c0;

		// Token: 0x04000055 RID: 85
		public double2 c1;

		// Token: 0x04000056 RID: 86
		public double2 c2;

		// Token: 0x04000057 RID: 87
		public double2 c3;

		// Token: 0x04000058 RID: 88
		public static readonly double2x4 zero;
	}
}
