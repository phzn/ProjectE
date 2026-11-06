using System;
using System.Runtime.CompilerServices;
using Unity.IL2CPP.CompilerServices;

namespace Unity.Mathematics
{
	// Token: 0x0200001B RID: 27
	[Il2CppEagerStaticClassConstruction]
	[Serializable]
	public struct double4x2 : IEquatable<double4x2>, IFormattable
	{
		// Token: 0x06000FA2 RID: 4002 RVA: 0x0002E1F9 File Offset: 0x0002C3F9
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public double4x2(double4 c0, double4 c1)
		{
			this.c0 = c0;
			this.c1 = c1;
		}

		// Token: 0x06000FA3 RID: 4003 RVA: 0x0002E209 File Offset: 0x0002C409
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public double4x2(double m00, double m01, double m10, double m11, double m20, double m21, double m30, double m31)
		{
			this.c0 = new double4(m00, m10, m20, m30);
			this.c1 = new double4(m01, m11, m21, m31);
		}

		// Token: 0x06000FA4 RID: 4004 RVA: 0x0002E22E File Offset: 0x0002C42E
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public double4x2(double v)
		{
			this.c0 = v;
			this.c1 = v;
		}

		// Token: 0x06000FA5 RID: 4005 RVA: 0x0002E248 File Offset: 0x0002C448
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public double4x2(bool v)
		{
			this.c0 = math.select(new double4(0.0), new double4(1.0), v);
			this.c1 = math.select(new double4(0.0), new double4(1.0), v);
		}

		// Token: 0x06000FA6 RID: 4006 RVA: 0x0002E2A8 File Offset: 0x0002C4A8
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public double4x2(bool4x2 v)
		{
			this.c0 = math.select(new double4(0.0), new double4(1.0), v.c0);
			this.c1 = math.select(new double4(0.0), new double4(1.0), v.c1);
		}

		// Token: 0x06000FA7 RID: 4007 RVA: 0x0002E30F File Offset: 0x0002C50F
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public double4x2(int v)
		{
			this.c0 = v;
			this.c1 = v;
		}

		// Token: 0x06000FA8 RID: 4008 RVA: 0x0002E329 File Offset: 0x0002C529
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public double4x2(int4x2 v)
		{
			this.c0 = v.c0;
			this.c1 = v.c1;
		}

		// Token: 0x06000FA9 RID: 4009 RVA: 0x0002E34D File Offset: 0x0002C54D
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public double4x2(uint v)
		{
			this.c0 = v;
			this.c1 = v;
		}

		// Token: 0x06000FAA RID: 4010 RVA: 0x0002E367 File Offset: 0x0002C567
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public double4x2(uint4x2 v)
		{
			this.c0 = v.c0;
			this.c1 = v.c1;
		}

		// Token: 0x06000FAB RID: 4011 RVA: 0x0002E38B File Offset: 0x0002C58B
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public double4x2(float v)
		{
			this.c0 = v;
			this.c1 = v;
		}

		// Token: 0x06000FAC RID: 4012 RVA: 0x0002E3A5 File Offset: 0x0002C5A5
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public double4x2(float4x2 v)
		{
			this.c0 = v.c0;
			this.c1 = v.c1;
		}

		// Token: 0x06000FAD RID: 4013 RVA: 0x0002E3C9 File Offset: 0x0002C5C9
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static implicit operator double4x2(double v)
		{
			return new double4x2(v);
		}

		// Token: 0x06000FAE RID: 4014 RVA: 0x0002E3D1 File Offset: 0x0002C5D1
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static explicit operator double4x2(bool v)
		{
			return new double4x2(v);
		}

		// Token: 0x06000FAF RID: 4015 RVA: 0x0002E3D9 File Offset: 0x0002C5D9
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static explicit operator double4x2(bool4x2 v)
		{
			return new double4x2(v);
		}

		// Token: 0x06000FB0 RID: 4016 RVA: 0x0002E3E1 File Offset: 0x0002C5E1
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static implicit operator double4x2(int v)
		{
			return new double4x2(v);
		}

		// Token: 0x06000FB1 RID: 4017 RVA: 0x0002E3E9 File Offset: 0x0002C5E9
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static implicit operator double4x2(int4x2 v)
		{
			return new double4x2(v);
		}

		// Token: 0x06000FB2 RID: 4018 RVA: 0x0002E3F1 File Offset: 0x0002C5F1
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static implicit operator double4x2(uint v)
		{
			return new double4x2(v);
		}

		// Token: 0x06000FB3 RID: 4019 RVA: 0x0002E3F9 File Offset: 0x0002C5F9
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static implicit operator double4x2(uint4x2 v)
		{
			return new double4x2(v);
		}

		// Token: 0x06000FB4 RID: 4020 RVA: 0x0002E401 File Offset: 0x0002C601
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static implicit operator double4x2(float v)
		{
			return new double4x2(v);
		}

		// Token: 0x06000FB5 RID: 4021 RVA: 0x0002E409 File Offset: 0x0002C609
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static implicit operator double4x2(float4x2 v)
		{
			return new double4x2(v);
		}

		// Token: 0x06000FB6 RID: 4022 RVA: 0x0002E411 File Offset: 0x0002C611
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double4x2 operator *(double4x2 lhs, double4x2 rhs)
		{
			return new double4x2(lhs.c0 * rhs.c0, lhs.c1 * rhs.c1);
		}

		// Token: 0x06000FB7 RID: 4023 RVA: 0x0002E43A File Offset: 0x0002C63A
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double4x2 operator *(double4x2 lhs, double rhs)
		{
			return new double4x2(lhs.c0 * rhs, lhs.c1 * rhs);
		}

		// Token: 0x06000FB8 RID: 4024 RVA: 0x0002E459 File Offset: 0x0002C659
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double4x2 operator *(double lhs, double4x2 rhs)
		{
			return new double4x2(lhs * rhs.c0, lhs * rhs.c1);
		}

		// Token: 0x06000FB9 RID: 4025 RVA: 0x0002E478 File Offset: 0x0002C678
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double4x2 operator +(double4x2 lhs, double4x2 rhs)
		{
			return new double4x2(lhs.c0 + rhs.c0, lhs.c1 + rhs.c1);
		}

		// Token: 0x06000FBA RID: 4026 RVA: 0x0002E4A1 File Offset: 0x0002C6A1
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double4x2 operator +(double4x2 lhs, double rhs)
		{
			return new double4x2(lhs.c0 + rhs, lhs.c1 + rhs);
		}

		// Token: 0x06000FBB RID: 4027 RVA: 0x0002E4C0 File Offset: 0x0002C6C0
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double4x2 operator +(double lhs, double4x2 rhs)
		{
			return new double4x2(lhs + rhs.c0, lhs + rhs.c1);
		}

		// Token: 0x06000FBC RID: 4028 RVA: 0x0002E4DF File Offset: 0x0002C6DF
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double4x2 operator -(double4x2 lhs, double4x2 rhs)
		{
			return new double4x2(lhs.c0 - rhs.c0, lhs.c1 - rhs.c1);
		}

		// Token: 0x06000FBD RID: 4029 RVA: 0x0002E508 File Offset: 0x0002C708
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double4x2 operator -(double4x2 lhs, double rhs)
		{
			return new double4x2(lhs.c0 - rhs, lhs.c1 - rhs);
		}

		// Token: 0x06000FBE RID: 4030 RVA: 0x0002E527 File Offset: 0x0002C727
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double4x2 operator -(double lhs, double4x2 rhs)
		{
			return new double4x2(lhs - rhs.c0, lhs - rhs.c1);
		}

		// Token: 0x06000FBF RID: 4031 RVA: 0x0002E546 File Offset: 0x0002C746
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double4x2 operator /(double4x2 lhs, double4x2 rhs)
		{
			return new double4x2(lhs.c0 / rhs.c0, lhs.c1 / rhs.c1);
		}

		// Token: 0x06000FC0 RID: 4032 RVA: 0x0002E56F File Offset: 0x0002C76F
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double4x2 operator /(double4x2 lhs, double rhs)
		{
			return new double4x2(lhs.c0 / rhs, lhs.c1 / rhs);
		}

		// Token: 0x06000FC1 RID: 4033 RVA: 0x0002E58E File Offset: 0x0002C78E
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double4x2 operator /(double lhs, double4x2 rhs)
		{
			return new double4x2(lhs / rhs.c0, lhs / rhs.c1);
		}

		// Token: 0x06000FC2 RID: 4034 RVA: 0x0002E5AD File Offset: 0x0002C7AD
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double4x2 operator %(double4x2 lhs, double4x2 rhs)
		{
			return new double4x2(lhs.c0 % rhs.c0, lhs.c1 % rhs.c1);
		}

		// Token: 0x06000FC3 RID: 4035 RVA: 0x0002E5D6 File Offset: 0x0002C7D6
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double4x2 operator %(double4x2 lhs, double rhs)
		{
			return new double4x2(lhs.c0 % rhs, lhs.c1 % rhs);
		}

		// Token: 0x06000FC4 RID: 4036 RVA: 0x0002E5F5 File Offset: 0x0002C7F5
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double4x2 operator %(double lhs, double4x2 rhs)
		{
			return new double4x2(lhs % rhs.c0, lhs % rhs.c1);
		}

		// Token: 0x06000FC5 RID: 4037 RVA: 0x0002E614 File Offset: 0x0002C814
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double4x2 operator ++(double4x2 val)
		{
			double4 @double = double4.op_Increment(val.c0);
			val.c0 = @double;
			double4 double2 = @double;
			@double = double4.op_Increment(val.c1);
			val.c1 = @double;
			return new double4x2(double2, @double);
		}

		// Token: 0x06000FC6 RID: 4038 RVA: 0x0002E65C File Offset: 0x0002C85C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double4x2 operator --(double4x2 val)
		{
			double4 @double = double4.op_Decrement(val.c0);
			val.c0 = @double;
			double4 double2 = @double;
			@double = double4.op_Decrement(val.c1);
			val.c1 = @double;
			return new double4x2(double2, @double);
		}

		// Token: 0x06000FC7 RID: 4039 RVA: 0x0002E6A2 File Offset: 0x0002C8A2
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool4x2 operator <(double4x2 lhs, double4x2 rhs)
		{
			return new bool4x2(lhs.c0 < rhs.c0, lhs.c1 < rhs.c1);
		}

		// Token: 0x06000FC8 RID: 4040 RVA: 0x0002E6CB File Offset: 0x0002C8CB
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool4x2 operator <(double4x2 lhs, double rhs)
		{
			return new bool4x2(lhs.c0 < rhs, lhs.c1 < rhs);
		}

		// Token: 0x06000FC9 RID: 4041 RVA: 0x0002E6EA File Offset: 0x0002C8EA
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool4x2 operator <(double lhs, double4x2 rhs)
		{
			return new bool4x2(lhs < rhs.c0, lhs < rhs.c1);
		}

		// Token: 0x06000FCA RID: 4042 RVA: 0x0002E709 File Offset: 0x0002C909
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool4x2 operator <=(double4x2 lhs, double4x2 rhs)
		{
			return new bool4x2(lhs.c0 <= rhs.c0, lhs.c1 <= rhs.c1);
		}

		// Token: 0x06000FCB RID: 4043 RVA: 0x0002E732 File Offset: 0x0002C932
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool4x2 operator <=(double4x2 lhs, double rhs)
		{
			return new bool4x2(lhs.c0 <= rhs, lhs.c1 <= rhs);
		}

		// Token: 0x06000FCC RID: 4044 RVA: 0x0002E751 File Offset: 0x0002C951
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool4x2 operator <=(double lhs, double4x2 rhs)
		{
			return new bool4x2(lhs <= rhs.c0, lhs <= rhs.c1);
		}

		// Token: 0x06000FCD RID: 4045 RVA: 0x0002E770 File Offset: 0x0002C970
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool4x2 operator >(double4x2 lhs, double4x2 rhs)
		{
			return new bool4x2(lhs.c0 > rhs.c0, lhs.c1 > rhs.c1);
		}

		// Token: 0x06000FCE RID: 4046 RVA: 0x0002E799 File Offset: 0x0002C999
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool4x2 operator >(double4x2 lhs, double rhs)
		{
			return new bool4x2(lhs.c0 > rhs, lhs.c1 > rhs);
		}

		// Token: 0x06000FCF RID: 4047 RVA: 0x0002E7B8 File Offset: 0x0002C9B8
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool4x2 operator >(double lhs, double4x2 rhs)
		{
			return new bool4x2(lhs > rhs.c0, lhs > rhs.c1);
		}

		// Token: 0x06000FD0 RID: 4048 RVA: 0x0002E7D7 File Offset: 0x0002C9D7
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool4x2 operator >=(double4x2 lhs, double4x2 rhs)
		{
			return new bool4x2(lhs.c0 >= rhs.c0, lhs.c1 >= rhs.c1);
		}

		// Token: 0x06000FD1 RID: 4049 RVA: 0x0002E800 File Offset: 0x0002CA00
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool4x2 operator >=(double4x2 lhs, double rhs)
		{
			return new bool4x2(lhs.c0 >= rhs, lhs.c1 >= rhs);
		}

		// Token: 0x06000FD2 RID: 4050 RVA: 0x0002E81F File Offset: 0x0002CA1F
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool4x2 operator >=(double lhs, double4x2 rhs)
		{
			return new bool4x2(lhs >= rhs.c0, lhs >= rhs.c1);
		}

		// Token: 0x06000FD3 RID: 4051 RVA: 0x0002E83E File Offset: 0x0002CA3E
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double4x2 operator -(double4x2 val)
		{
			return new double4x2(-val.c0, -val.c1);
		}

		// Token: 0x06000FD4 RID: 4052 RVA: 0x0002E85B File Offset: 0x0002CA5B
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double4x2 operator +(double4x2 val)
		{
			return new double4x2(+val.c0, +val.c1);
		}

		// Token: 0x06000FD5 RID: 4053 RVA: 0x0002E878 File Offset: 0x0002CA78
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool4x2 operator ==(double4x2 lhs, double4x2 rhs)
		{
			return new bool4x2(lhs.c0 == rhs.c0, lhs.c1 == rhs.c1);
		}

		// Token: 0x06000FD6 RID: 4054 RVA: 0x0002E8A1 File Offset: 0x0002CAA1
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool4x2 operator ==(double4x2 lhs, double rhs)
		{
			return new bool4x2(lhs.c0 == rhs, lhs.c1 == rhs);
		}

		// Token: 0x06000FD7 RID: 4055 RVA: 0x0002E8C0 File Offset: 0x0002CAC0
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool4x2 operator ==(double lhs, double4x2 rhs)
		{
			return new bool4x2(lhs == rhs.c0, lhs == rhs.c1);
		}

		// Token: 0x06000FD8 RID: 4056 RVA: 0x0002E8DF File Offset: 0x0002CADF
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool4x2 operator !=(double4x2 lhs, double4x2 rhs)
		{
			return new bool4x2(lhs.c0 != rhs.c0, lhs.c1 != rhs.c1);
		}

		// Token: 0x06000FD9 RID: 4057 RVA: 0x0002E908 File Offset: 0x0002CB08
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool4x2 operator !=(double4x2 lhs, double rhs)
		{
			return new bool4x2(lhs.c0 != rhs, lhs.c1 != rhs);
		}

		// Token: 0x06000FDA RID: 4058 RVA: 0x0002E927 File Offset: 0x0002CB27
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool4x2 operator !=(double lhs, double4x2 rhs)
		{
			return new bool4x2(lhs != rhs.c0, lhs != rhs.c1);
		}

		// Token: 0x170003D8 RID: 984
		public unsafe ref double4 this[int index]
		{
			get
			{
				fixed (double4x2* ptr = &this)
				{
					return ref *(double4*)(ptr + (IntPtr)index * (IntPtr)sizeof(double4) / (IntPtr)sizeof(double4x2));
				}
			}
		}

		// Token: 0x06000FDC RID: 4060 RVA: 0x0002E963 File Offset: 0x0002CB63
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool Equals(double4x2 rhs)
		{
			return this.c0.Equals(rhs.c0) && this.c1.Equals(rhs.c1);
		}

		// Token: 0x06000FDD RID: 4061 RVA: 0x0002E98C File Offset: 0x0002CB8C
		public override bool Equals(object o)
		{
			if (o is double4x2)
			{
				double4x2 double4x = (double4x2)o;
				return this.Equals(double4x);
			}
			return false;
		}

		// Token: 0x06000FDE RID: 4062 RVA: 0x0002E9B1 File Offset: 0x0002CBB1
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public override int GetHashCode()
		{
			return (int)math.hash(this);
		}

		// Token: 0x06000FDF RID: 4063 RVA: 0x0002E9C0 File Offset: 0x0002CBC0
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public override string ToString()
		{
			return string.Format("double4x2({0}, {1},  {2}, {3},  {4}, {5},  {6}, {7})", new object[]
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

		// Token: 0x06000FE0 RID: 4064 RVA: 0x0002EA78 File Offset: 0x0002CC78
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public string ToString(string format, IFormatProvider formatProvider)
		{
			return string.Format("double4x2({0}, {1},  {2}, {3},  {4}, {5},  {6}, {7})", new object[]
			{
				this.c0.x.ToString(format, formatProvider),
				this.c1.x.ToString(format, formatProvider),
				this.c0.y.ToString(format, formatProvider),
				this.c1.y.ToString(format, formatProvider),
				this.c0.z.ToString(format, formatProvider),
				this.c1.z.ToString(format, formatProvider),
				this.c0.w.ToString(format, formatProvider),
				this.c1.w.ToString(format, formatProvider)
			});
		}

		// Token: 0x0400006F RID: 111
		public double4 c0;

		// Token: 0x04000070 RID: 112
		public double4 c1;

		// Token: 0x04000071 RID: 113
		public static readonly double4x2 zero;
	}
}
