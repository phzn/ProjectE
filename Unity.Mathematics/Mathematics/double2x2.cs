using System;
using System.Runtime.CompilerServices;
using Unity.IL2CPP.CompilerServices;

namespace Unity.Mathematics
{
	// Token: 0x02000013 RID: 19
	[Il2CppEagerStaticClassConstruction]
	[Serializable]
	public struct double2x2 : IEquatable<double2x2>, IFormattable
	{
		// Token: 0x06000B89 RID: 2953 RVA: 0x00024116 File Offset: 0x00022316
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public double2x2(double2 c0, double2 c1)
		{
			this.c0 = c0;
			this.c1 = c1;
		}

		// Token: 0x06000B8A RID: 2954 RVA: 0x00024126 File Offset: 0x00022326
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public double2x2(double m00, double m01, double m10, double m11)
		{
			this.c0 = new double2(m00, m10);
			this.c1 = new double2(m01, m11);
		}

		// Token: 0x06000B8B RID: 2955 RVA: 0x00024143 File Offset: 0x00022343
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public double2x2(double v)
		{
			this.c0 = v;
			this.c1 = v;
		}

		// Token: 0x06000B8C RID: 2956 RVA: 0x00024160 File Offset: 0x00022360
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public double2x2(bool v)
		{
			this.c0 = math.select(new double2(0.0), new double2(1.0), v);
			this.c1 = math.select(new double2(0.0), new double2(1.0), v);
		}

		// Token: 0x06000B8D RID: 2957 RVA: 0x000241C0 File Offset: 0x000223C0
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public double2x2(bool2x2 v)
		{
			this.c0 = math.select(new double2(0.0), new double2(1.0), v.c0);
			this.c1 = math.select(new double2(0.0), new double2(1.0), v.c1);
		}

		// Token: 0x06000B8E RID: 2958 RVA: 0x00024227 File Offset: 0x00022427
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public double2x2(int v)
		{
			this.c0 = v;
			this.c1 = v;
		}

		// Token: 0x06000B8F RID: 2959 RVA: 0x00024241 File Offset: 0x00022441
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public double2x2(int2x2 v)
		{
			this.c0 = v.c0;
			this.c1 = v.c1;
		}

		// Token: 0x06000B90 RID: 2960 RVA: 0x00024265 File Offset: 0x00022465
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public double2x2(uint v)
		{
			this.c0 = v;
			this.c1 = v;
		}

		// Token: 0x06000B91 RID: 2961 RVA: 0x0002427F File Offset: 0x0002247F
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public double2x2(uint2x2 v)
		{
			this.c0 = v.c0;
			this.c1 = v.c1;
		}

		// Token: 0x06000B92 RID: 2962 RVA: 0x000242A3 File Offset: 0x000224A3
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public double2x2(float v)
		{
			this.c0 = v;
			this.c1 = v;
		}

		// Token: 0x06000B93 RID: 2963 RVA: 0x000242BD File Offset: 0x000224BD
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public double2x2(float2x2 v)
		{
			this.c0 = v.c0;
			this.c1 = v.c1;
		}

		// Token: 0x06000B94 RID: 2964 RVA: 0x000242E1 File Offset: 0x000224E1
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static implicit operator double2x2(double v)
		{
			return new double2x2(v);
		}

		// Token: 0x06000B95 RID: 2965 RVA: 0x000242E9 File Offset: 0x000224E9
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static explicit operator double2x2(bool v)
		{
			return new double2x2(v);
		}

		// Token: 0x06000B96 RID: 2966 RVA: 0x000242F1 File Offset: 0x000224F1
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static explicit operator double2x2(bool2x2 v)
		{
			return new double2x2(v);
		}

		// Token: 0x06000B97 RID: 2967 RVA: 0x000242F9 File Offset: 0x000224F9
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static implicit operator double2x2(int v)
		{
			return new double2x2(v);
		}

		// Token: 0x06000B98 RID: 2968 RVA: 0x00024301 File Offset: 0x00022501
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static implicit operator double2x2(int2x2 v)
		{
			return new double2x2(v);
		}

		// Token: 0x06000B99 RID: 2969 RVA: 0x00024309 File Offset: 0x00022509
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static implicit operator double2x2(uint v)
		{
			return new double2x2(v);
		}

		// Token: 0x06000B9A RID: 2970 RVA: 0x00024311 File Offset: 0x00022511
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static implicit operator double2x2(uint2x2 v)
		{
			return new double2x2(v);
		}

		// Token: 0x06000B9B RID: 2971 RVA: 0x00024319 File Offset: 0x00022519
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static implicit operator double2x2(float v)
		{
			return new double2x2(v);
		}

		// Token: 0x06000B9C RID: 2972 RVA: 0x00024321 File Offset: 0x00022521
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static implicit operator double2x2(float2x2 v)
		{
			return new double2x2(v);
		}

		// Token: 0x06000B9D RID: 2973 RVA: 0x00024329 File Offset: 0x00022529
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double2x2 operator *(double2x2 lhs, double2x2 rhs)
		{
			return new double2x2(lhs.c0 * rhs.c0, lhs.c1 * rhs.c1);
		}

		// Token: 0x06000B9E RID: 2974 RVA: 0x00024352 File Offset: 0x00022552
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double2x2 operator *(double2x2 lhs, double rhs)
		{
			return new double2x2(lhs.c0 * rhs, lhs.c1 * rhs);
		}

		// Token: 0x06000B9F RID: 2975 RVA: 0x00024371 File Offset: 0x00022571
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double2x2 operator *(double lhs, double2x2 rhs)
		{
			return new double2x2(lhs * rhs.c0, lhs * rhs.c1);
		}

		// Token: 0x06000BA0 RID: 2976 RVA: 0x00024390 File Offset: 0x00022590
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double2x2 operator +(double2x2 lhs, double2x2 rhs)
		{
			return new double2x2(lhs.c0 + rhs.c0, lhs.c1 + rhs.c1);
		}

		// Token: 0x06000BA1 RID: 2977 RVA: 0x000243B9 File Offset: 0x000225B9
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double2x2 operator +(double2x2 lhs, double rhs)
		{
			return new double2x2(lhs.c0 + rhs, lhs.c1 + rhs);
		}

		// Token: 0x06000BA2 RID: 2978 RVA: 0x000243D8 File Offset: 0x000225D8
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double2x2 operator +(double lhs, double2x2 rhs)
		{
			return new double2x2(lhs + rhs.c0, lhs + rhs.c1);
		}

		// Token: 0x06000BA3 RID: 2979 RVA: 0x000243F7 File Offset: 0x000225F7
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double2x2 operator -(double2x2 lhs, double2x2 rhs)
		{
			return new double2x2(lhs.c0 - rhs.c0, lhs.c1 - rhs.c1);
		}

		// Token: 0x06000BA4 RID: 2980 RVA: 0x00024420 File Offset: 0x00022620
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double2x2 operator -(double2x2 lhs, double rhs)
		{
			return new double2x2(lhs.c0 - rhs, lhs.c1 - rhs);
		}

		// Token: 0x06000BA5 RID: 2981 RVA: 0x0002443F File Offset: 0x0002263F
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double2x2 operator -(double lhs, double2x2 rhs)
		{
			return new double2x2(lhs - rhs.c0, lhs - rhs.c1);
		}

		// Token: 0x06000BA6 RID: 2982 RVA: 0x0002445E File Offset: 0x0002265E
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double2x2 operator /(double2x2 lhs, double2x2 rhs)
		{
			return new double2x2(lhs.c0 / rhs.c0, lhs.c1 / rhs.c1);
		}

		// Token: 0x06000BA7 RID: 2983 RVA: 0x00024487 File Offset: 0x00022687
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double2x2 operator /(double2x2 lhs, double rhs)
		{
			return new double2x2(lhs.c0 / rhs, lhs.c1 / rhs);
		}

		// Token: 0x06000BA8 RID: 2984 RVA: 0x000244A6 File Offset: 0x000226A6
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double2x2 operator /(double lhs, double2x2 rhs)
		{
			return new double2x2(lhs / rhs.c0, lhs / rhs.c1);
		}

		// Token: 0x06000BA9 RID: 2985 RVA: 0x000244C5 File Offset: 0x000226C5
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double2x2 operator %(double2x2 lhs, double2x2 rhs)
		{
			return new double2x2(lhs.c0 % rhs.c0, lhs.c1 % rhs.c1);
		}

		// Token: 0x06000BAA RID: 2986 RVA: 0x000244EE File Offset: 0x000226EE
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double2x2 operator %(double2x2 lhs, double rhs)
		{
			return new double2x2(lhs.c0 % rhs, lhs.c1 % rhs);
		}

		// Token: 0x06000BAB RID: 2987 RVA: 0x0002450D File Offset: 0x0002270D
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double2x2 operator %(double lhs, double2x2 rhs)
		{
			return new double2x2(lhs % rhs.c0, lhs % rhs.c1);
		}

		// Token: 0x06000BAC RID: 2988 RVA: 0x0002452C File Offset: 0x0002272C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double2x2 operator ++(double2x2 val)
		{
			double2 @double = double2.op_Increment(val.c0);
			val.c0 = @double;
			double2 double2 = @double;
			@double = double2.op_Increment(val.c1);
			val.c1 = @double;
			return new double2x2(double2, @double);
		}

		// Token: 0x06000BAD RID: 2989 RVA: 0x00024574 File Offset: 0x00022774
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double2x2 operator --(double2x2 val)
		{
			double2 @double = double2.op_Decrement(val.c0);
			val.c0 = @double;
			double2 double2 = @double;
			@double = double2.op_Decrement(val.c1);
			val.c1 = @double;
			return new double2x2(double2, @double);
		}

		// Token: 0x06000BAE RID: 2990 RVA: 0x000245BA File Offset: 0x000227BA
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool2x2 operator <(double2x2 lhs, double2x2 rhs)
		{
			return new bool2x2(lhs.c0 < rhs.c0, lhs.c1 < rhs.c1);
		}

		// Token: 0x06000BAF RID: 2991 RVA: 0x000245E3 File Offset: 0x000227E3
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool2x2 operator <(double2x2 lhs, double rhs)
		{
			return new bool2x2(lhs.c0 < rhs, lhs.c1 < rhs);
		}

		// Token: 0x06000BB0 RID: 2992 RVA: 0x00024602 File Offset: 0x00022802
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool2x2 operator <(double lhs, double2x2 rhs)
		{
			return new bool2x2(lhs < rhs.c0, lhs < rhs.c1);
		}

		// Token: 0x06000BB1 RID: 2993 RVA: 0x00024621 File Offset: 0x00022821
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool2x2 operator <=(double2x2 lhs, double2x2 rhs)
		{
			return new bool2x2(lhs.c0 <= rhs.c0, lhs.c1 <= rhs.c1);
		}

		// Token: 0x06000BB2 RID: 2994 RVA: 0x0002464A File Offset: 0x0002284A
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool2x2 operator <=(double2x2 lhs, double rhs)
		{
			return new bool2x2(lhs.c0 <= rhs, lhs.c1 <= rhs);
		}

		// Token: 0x06000BB3 RID: 2995 RVA: 0x00024669 File Offset: 0x00022869
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool2x2 operator <=(double lhs, double2x2 rhs)
		{
			return new bool2x2(lhs <= rhs.c0, lhs <= rhs.c1);
		}

		// Token: 0x06000BB4 RID: 2996 RVA: 0x00024688 File Offset: 0x00022888
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool2x2 operator >(double2x2 lhs, double2x2 rhs)
		{
			return new bool2x2(lhs.c0 > rhs.c0, lhs.c1 > rhs.c1);
		}

		// Token: 0x06000BB5 RID: 2997 RVA: 0x000246B1 File Offset: 0x000228B1
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool2x2 operator >(double2x2 lhs, double rhs)
		{
			return new bool2x2(lhs.c0 > rhs, lhs.c1 > rhs);
		}

		// Token: 0x06000BB6 RID: 2998 RVA: 0x000246D0 File Offset: 0x000228D0
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool2x2 operator >(double lhs, double2x2 rhs)
		{
			return new bool2x2(lhs > rhs.c0, lhs > rhs.c1);
		}

		// Token: 0x06000BB7 RID: 2999 RVA: 0x000246EF File Offset: 0x000228EF
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool2x2 operator >=(double2x2 lhs, double2x2 rhs)
		{
			return new bool2x2(lhs.c0 >= rhs.c0, lhs.c1 >= rhs.c1);
		}

		// Token: 0x06000BB8 RID: 3000 RVA: 0x00024718 File Offset: 0x00022918
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool2x2 operator >=(double2x2 lhs, double rhs)
		{
			return new bool2x2(lhs.c0 >= rhs, lhs.c1 >= rhs);
		}

		// Token: 0x06000BB9 RID: 3001 RVA: 0x00024737 File Offset: 0x00022937
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool2x2 operator >=(double lhs, double2x2 rhs)
		{
			return new bool2x2(lhs >= rhs.c0, lhs >= rhs.c1);
		}

		// Token: 0x06000BBA RID: 3002 RVA: 0x00024756 File Offset: 0x00022956
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double2x2 operator -(double2x2 val)
		{
			return new double2x2(-val.c0, -val.c1);
		}

		// Token: 0x06000BBB RID: 3003 RVA: 0x00024773 File Offset: 0x00022973
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double2x2 operator +(double2x2 val)
		{
			return new double2x2(+val.c0, +val.c1);
		}

		// Token: 0x06000BBC RID: 3004 RVA: 0x00024790 File Offset: 0x00022990
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool2x2 operator ==(double2x2 lhs, double2x2 rhs)
		{
			return new bool2x2(lhs.c0 == rhs.c0, lhs.c1 == rhs.c1);
		}

		// Token: 0x06000BBD RID: 3005 RVA: 0x000247B9 File Offset: 0x000229B9
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool2x2 operator ==(double2x2 lhs, double rhs)
		{
			return new bool2x2(lhs.c0 == rhs, lhs.c1 == rhs);
		}

		// Token: 0x06000BBE RID: 3006 RVA: 0x000247D8 File Offset: 0x000229D8
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool2x2 operator ==(double lhs, double2x2 rhs)
		{
			return new bool2x2(lhs == rhs.c0, lhs == rhs.c1);
		}

		// Token: 0x06000BBF RID: 3007 RVA: 0x000247F7 File Offset: 0x000229F7
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool2x2 operator !=(double2x2 lhs, double2x2 rhs)
		{
			return new bool2x2(lhs.c0 != rhs.c0, lhs.c1 != rhs.c1);
		}

		// Token: 0x06000BC0 RID: 3008 RVA: 0x00024820 File Offset: 0x00022A20
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool2x2 operator !=(double2x2 lhs, double rhs)
		{
			return new bool2x2(lhs.c0 != rhs, lhs.c1 != rhs);
		}

		// Token: 0x06000BC1 RID: 3009 RVA: 0x0002483F File Offset: 0x00022A3F
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool2x2 operator !=(double lhs, double2x2 rhs)
		{
			return new bool2x2(lhs != rhs.c0, lhs != rhs.c1);
		}

		// Token: 0x1700020B RID: 523
		public unsafe ref double2 this[int index]
		{
			get
			{
				fixed (double2x2* ptr = &this)
				{
					return ref *(double2*)(ptr + (IntPtr)index * (IntPtr)sizeof(double2) / (IntPtr)sizeof(double2x2));
				}
			}
		}

		// Token: 0x06000BC3 RID: 3011 RVA: 0x0002487B File Offset: 0x00022A7B
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool Equals(double2x2 rhs)
		{
			return this.c0.Equals(rhs.c0) && this.c1.Equals(rhs.c1);
		}

		// Token: 0x06000BC4 RID: 3012 RVA: 0x000248A4 File Offset: 0x00022AA4
		public override bool Equals(object o)
		{
			if (o is double2x2)
			{
				double2x2 double2x = (double2x2)o;
				return this.Equals(double2x);
			}
			return false;
		}

		// Token: 0x06000BC5 RID: 3013 RVA: 0x000248C9 File Offset: 0x00022AC9
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public override int GetHashCode()
		{
			return (int)math.hash(this);
		}

		// Token: 0x06000BC6 RID: 3014 RVA: 0x000248D8 File Offset: 0x00022AD8
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public override string ToString()
		{
			return string.Format("double2x2({0}, {1},  {2}, {3})", new object[]
			{
				this.c0.x,
				this.c1.x,
				this.c0.y,
				this.c1.y
			});
		}

		// Token: 0x06000BC7 RID: 3015 RVA: 0x00024944 File Offset: 0x00022B44
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public string ToString(string format, IFormatProvider formatProvider)
		{
			return string.Format("double2x2({0}, {1},  {2}, {3})", new object[]
			{
				this.c0.x.ToString(format, formatProvider),
				this.c1.x.ToString(format, formatProvider),
				this.c0.y.ToString(format, formatProvider),
				this.c1.y.ToString(format, formatProvider)
			});
		}

		// Token: 0x0400004C RID: 76
		public double2 c0;

		// Token: 0x0400004D RID: 77
		public double2 c1;

		// Token: 0x0400004E RID: 78
		public static readonly double2x2 identity = new double2x2(1.0, 0.0, 0.0, 1.0);

		// Token: 0x0400004F RID: 79
		public static readonly double2x2 zero;
	}
}
