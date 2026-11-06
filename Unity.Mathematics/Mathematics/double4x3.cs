using System;
using System.Runtime.CompilerServices;
using Unity.IL2CPP.CompilerServices;

namespace Unity.Mathematics
{
	// Token: 0x0200001C RID: 28
	[Il2CppEagerStaticClassConstruction]
	[Serializable]
	public struct double4x3 : IEquatable<double4x3>, IFormattable
	{
		// Token: 0x06000FE1 RID: 4065 RVA: 0x0002EB3D File Offset: 0x0002CD3D
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public double4x3(double4 c0, double4 c1, double4 c2)
		{
			this.c0 = c0;
			this.c1 = c1;
			this.c2 = c2;
		}

		// Token: 0x06000FE2 RID: 4066 RVA: 0x0002EB54 File Offset: 0x0002CD54
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public double4x3(double m00, double m01, double m02, double m10, double m11, double m12, double m20, double m21, double m22, double m30, double m31, double m32)
		{
			this.c0 = new double4(m00, m10, m20, m30);
			this.c1 = new double4(m01, m11, m21, m31);
			this.c2 = new double4(m02, m12, m22, m32);
		}

		// Token: 0x06000FE3 RID: 4067 RVA: 0x0002EB8C File Offset: 0x0002CD8C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public double4x3(double v)
		{
			this.c0 = v;
			this.c1 = v;
			this.c2 = v;
		}

		// Token: 0x06000FE4 RID: 4068 RVA: 0x0002EBB4 File Offset: 0x0002CDB4
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public double4x3(bool v)
		{
			this.c0 = math.select(new double4(0.0), new double4(1.0), v);
			this.c1 = math.select(new double4(0.0), new double4(1.0), v);
			this.c2 = math.select(new double4(0.0), new double4(1.0), v);
		}

		// Token: 0x06000FE5 RID: 4069 RVA: 0x0002EC3C File Offset: 0x0002CE3C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public double4x3(bool4x3 v)
		{
			this.c0 = math.select(new double4(0.0), new double4(1.0), v.c0);
			this.c1 = math.select(new double4(0.0), new double4(1.0), v.c1);
			this.c2 = math.select(new double4(0.0), new double4(1.0), v.c2);
		}

		// Token: 0x06000FE6 RID: 4070 RVA: 0x0002ECD0 File Offset: 0x0002CED0
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public double4x3(int v)
		{
			this.c0 = v;
			this.c1 = v;
			this.c2 = v;
		}

		// Token: 0x06000FE7 RID: 4071 RVA: 0x0002ECF6 File Offset: 0x0002CEF6
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public double4x3(int4x3 v)
		{
			this.c0 = v.c0;
			this.c1 = v.c1;
			this.c2 = v.c2;
		}

		// Token: 0x06000FE8 RID: 4072 RVA: 0x0002ED2B File Offset: 0x0002CF2B
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public double4x3(uint v)
		{
			this.c0 = v;
			this.c1 = v;
			this.c2 = v;
		}

		// Token: 0x06000FE9 RID: 4073 RVA: 0x0002ED51 File Offset: 0x0002CF51
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public double4x3(uint4x3 v)
		{
			this.c0 = v.c0;
			this.c1 = v.c1;
			this.c2 = v.c2;
		}

		// Token: 0x06000FEA RID: 4074 RVA: 0x0002ED86 File Offset: 0x0002CF86
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public double4x3(float v)
		{
			this.c0 = v;
			this.c1 = v;
			this.c2 = v;
		}

		// Token: 0x06000FEB RID: 4075 RVA: 0x0002EDAC File Offset: 0x0002CFAC
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public double4x3(float4x3 v)
		{
			this.c0 = v.c0;
			this.c1 = v.c1;
			this.c2 = v.c2;
		}

		// Token: 0x06000FEC RID: 4076 RVA: 0x0002EDE1 File Offset: 0x0002CFE1
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static implicit operator double4x3(double v)
		{
			return new double4x3(v);
		}

		// Token: 0x06000FED RID: 4077 RVA: 0x0002EDE9 File Offset: 0x0002CFE9
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static explicit operator double4x3(bool v)
		{
			return new double4x3(v);
		}

		// Token: 0x06000FEE RID: 4078 RVA: 0x0002EDF1 File Offset: 0x0002CFF1
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static explicit operator double4x3(bool4x3 v)
		{
			return new double4x3(v);
		}

		// Token: 0x06000FEF RID: 4079 RVA: 0x0002EDF9 File Offset: 0x0002CFF9
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static implicit operator double4x3(int v)
		{
			return new double4x3(v);
		}

		// Token: 0x06000FF0 RID: 4080 RVA: 0x0002EE01 File Offset: 0x0002D001
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static implicit operator double4x3(int4x3 v)
		{
			return new double4x3(v);
		}

		// Token: 0x06000FF1 RID: 4081 RVA: 0x0002EE09 File Offset: 0x0002D009
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static implicit operator double4x3(uint v)
		{
			return new double4x3(v);
		}

		// Token: 0x06000FF2 RID: 4082 RVA: 0x0002EE11 File Offset: 0x0002D011
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static implicit operator double4x3(uint4x3 v)
		{
			return new double4x3(v);
		}

		// Token: 0x06000FF3 RID: 4083 RVA: 0x0002EE19 File Offset: 0x0002D019
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static implicit operator double4x3(float v)
		{
			return new double4x3(v);
		}

		// Token: 0x06000FF4 RID: 4084 RVA: 0x0002EE21 File Offset: 0x0002D021
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static implicit operator double4x3(float4x3 v)
		{
			return new double4x3(v);
		}

		// Token: 0x06000FF5 RID: 4085 RVA: 0x0002EE29 File Offset: 0x0002D029
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double4x3 operator *(double4x3 lhs, double4x3 rhs)
		{
			return new double4x3(lhs.c0 * rhs.c0, lhs.c1 * rhs.c1, lhs.c2 * rhs.c2);
		}

		// Token: 0x06000FF6 RID: 4086 RVA: 0x0002EE63 File Offset: 0x0002D063
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double4x3 operator *(double4x3 lhs, double rhs)
		{
			return new double4x3(lhs.c0 * rhs, lhs.c1 * rhs, lhs.c2 * rhs);
		}

		// Token: 0x06000FF7 RID: 4087 RVA: 0x0002EE8E File Offset: 0x0002D08E
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double4x3 operator *(double lhs, double4x3 rhs)
		{
			return new double4x3(lhs * rhs.c0, lhs * rhs.c1, lhs * rhs.c2);
		}

		// Token: 0x06000FF8 RID: 4088 RVA: 0x0002EEB9 File Offset: 0x0002D0B9
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double4x3 operator +(double4x3 lhs, double4x3 rhs)
		{
			return new double4x3(lhs.c0 + rhs.c0, lhs.c1 + rhs.c1, lhs.c2 + rhs.c2);
		}

		// Token: 0x06000FF9 RID: 4089 RVA: 0x0002EEF3 File Offset: 0x0002D0F3
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double4x3 operator +(double4x3 lhs, double rhs)
		{
			return new double4x3(lhs.c0 + rhs, lhs.c1 + rhs, lhs.c2 + rhs);
		}

		// Token: 0x06000FFA RID: 4090 RVA: 0x0002EF1E File Offset: 0x0002D11E
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double4x3 operator +(double lhs, double4x3 rhs)
		{
			return new double4x3(lhs + rhs.c0, lhs + rhs.c1, lhs + rhs.c2);
		}

		// Token: 0x06000FFB RID: 4091 RVA: 0x0002EF49 File Offset: 0x0002D149
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double4x3 operator -(double4x3 lhs, double4x3 rhs)
		{
			return new double4x3(lhs.c0 - rhs.c0, lhs.c1 - rhs.c1, lhs.c2 - rhs.c2);
		}

		// Token: 0x06000FFC RID: 4092 RVA: 0x0002EF83 File Offset: 0x0002D183
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double4x3 operator -(double4x3 lhs, double rhs)
		{
			return new double4x3(lhs.c0 - rhs, lhs.c1 - rhs, lhs.c2 - rhs);
		}

		// Token: 0x06000FFD RID: 4093 RVA: 0x0002EFAE File Offset: 0x0002D1AE
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double4x3 operator -(double lhs, double4x3 rhs)
		{
			return new double4x3(lhs - rhs.c0, lhs - rhs.c1, lhs - rhs.c2);
		}

		// Token: 0x06000FFE RID: 4094 RVA: 0x0002EFD9 File Offset: 0x0002D1D9
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double4x3 operator /(double4x3 lhs, double4x3 rhs)
		{
			return new double4x3(lhs.c0 / rhs.c0, lhs.c1 / rhs.c1, lhs.c2 / rhs.c2);
		}

		// Token: 0x06000FFF RID: 4095 RVA: 0x0002F013 File Offset: 0x0002D213
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double4x3 operator /(double4x3 lhs, double rhs)
		{
			return new double4x3(lhs.c0 / rhs, lhs.c1 / rhs, lhs.c2 / rhs);
		}

		// Token: 0x06001000 RID: 4096 RVA: 0x0002F03E File Offset: 0x0002D23E
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double4x3 operator /(double lhs, double4x3 rhs)
		{
			return new double4x3(lhs / rhs.c0, lhs / rhs.c1, lhs / rhs.c2);
		}

		// Token: 0x06001001 RID: 4097 RVA: 0x0002F069 File Offset: 0x0002D269
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double4x3 operator %(double4x3 lhs, double4x3 rhs)
		{
			return new double4x3(lhs.c0 % rhs.c0, lhs.c1 % rhs.c1, lhs.c2 % rhs.c2);
		}

		// Token: 0x06001002 RID: 4098 RVA: 0x0002F0A3 File Offset: 0x0002D2A3
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double4x3 operator %(double4x3 lhs, double rhs)
		{
			return new double4x3(lhs.c0 % rhs, lhs.c1 % rhs, lhs.c2 % rhs);
		}

		// Token: 0x06001003 RID: 4099 RVA: 0x0002F0CE File Offset: 0x0002D2CE
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double4x3 operator %(double lhs, double4x3 rhs)
		{
			return new double4x3(lhs % rhs.c0, lhs % rhs.c1, lhs % rhs.c2);
		}

		// Token: 0x06001004 RID: 4100 RVA: 0x0002F0FC File Offset: 0x0002D2FC
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double4x3 operator ++(double4x3 val)
		{
			double4 @double = double4.op_Increment(val.c0);
			val.c0 = @double;
			double4 double2 = @double;
			@double = double4.op_Increment(val.c1);
			val.c1 = @double;
			double4 double3 = @double;
			@double = double4.op_Increment(val.c2);
			val.c2 = @double;
			return new double4x3(double2, double3, @double);
		}

		// Token: 0x06001005 RID: 4101 RVA: 0x0002F15C File Offset: 0x0002D35C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double4x3 operator --(double4x3 val)
		{
			double4 @double = double4.op_Decrement(val.c0);
			val.c0 = @double;
			double4 double2 = @double;
			@double = double4.op_Decrement(val.c1);
			val.c1 = @double;
			double4 double3 = @double;
			@double = double4.op_Decrement(val.c2);
			val.c2 = @double;
			return new double4x3(double2, double3, @double);
		}

		// Token: 0x06001006 RID: 4102 RVA: 0x0002F1BC File Offset: 0x0002D3BC
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool4x3 operator <(double4x3 lhs, double4x3 rhs)
		{
			return new bool4x3(lhs.c0 < rhs.c0, lhs.c1 < rhs.c1, lhs.c2 < rhs.c2);
		}

		// Token: 0x06001007 RID: 4103 RVA: 0x0002F1F6 File Offset: 0x0002D3F6
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool4x3 operator <(double4x3 lhs, double rhs)
		{
			return new bool4x3(lhs.c0 < rhs, lhs.c1 < rhs, lhs.c2 < rhs);
		}

		// Token: 0x06001008 RID: 4104 RVA: 0x0002F221 File Offset: 0x0002D421
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool4x3 operator <(double lhs, double4x3 rhs)
		{
			return new bool4x3(lhs < rhs.c0, lhs < rhs.c1, lhs < rhs.c2);
		}

		// Token: 0x06001009 RID: 4105 RVA: 0x0002F24C File Offset: 0x0002D44C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool4x3 operator <=(double4x3 lhs, double4x3 rhs)
		{
			return new bool4x3(lhs.c0 <= rhs.c0, lhs.c1 <= rhs.c1, lhs.c2 <= rhs.c2);
		}

		// Token: 0x0600100A RID: 4106 RVA: 0x0002F286 File Offset: 0x0002D486
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool4x3 operator <=(double4x3 lhs, double rhs)
		{
			return new bool4x3(lhs.c0 <= rhs, lhs.c1 <= rhs, lhs.c2 <= rhs);
		}

		// Token: 0x0600100B RID: 4107 RVA: 0x0002F2B1 File Offset: 0x0002D4B1
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool4x3 operator <=(double lhs, double4x3 rhs)
		{
			return new bool4x3(lhs <= rhs.c0, lhs <= rhs.c1, lhs <= rhs.c2);
		}

		// Token: 0x0600100C RID: 4108 RVA: 0x0002F2DC File Offset: 0x0002D4DC
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool4x3 operator >(double4x3 lhs, double4x3 rhs)
		{
			return new bool4x3(lhs.c0 > rhs.c0, lhs.c1 > rhs.c1, lhs.c2 > rhs.c2);
		}

		// Token: 0x0600100D RID: 4109 RVA: 0x0002F316 File Offset: 0x0002D516
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool4x3 operator >(double4x3 lhs, double rhs)
		{
			return new bool4x3(lhs.c0 > rhs, lhs.c1 > rhs, lhs.c2 > rhs);
		}

		// Token: 0x0600100E RID: 4110 RVA: 0x0002F341 File Offset: 0x0002D541
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool4x3 operator >(double lhs, double4x3 rhs)
		{
			return new bool4x3(lhs > rhs.c0, lhs > rhs.c1, lhs > rhs.c2);
		}

		// Token: 0x0600100F RID: 4111 RVA: 0x0002F36C File Offset: 0x0002D56C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool4x3 operator >=(double4x3 lhs, double4x3 rhs)
		{
			return new bool4x3(lhs.c0 >= rhs.c0, lhs.c1 >= rhs.c1, lhs.c2 >= rhs.c2);
		}

		// Token: 0x06001010 RID: 4112 RVA: 0x0002F3A6 File Offset: 0x0002D5A6
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool4x3 operator >=(double4x3 lhs, double rhs)
		{
			return new bool4x3(lhs.c0 >= rhs, lhs.c1 >= rhs, lhs.c2 >= rhs);
		}

		// Token: 0x06001011 RID: 4113 RVA: 0x0002F3D1 File Offset: 0x0002D5D1
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool4x3 operator >=(double lhs, double4x3 rhs)
		{
			return new bool4x3(lhs >= rhs.c0, lhs >= rhs.c1, lhs >= rhs.c2);
		}

		// Token: 0x06001012 RID: 4114 RVA: 0x0002F3FC File Offset: 0x0002D5FC
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double4x3 operator -(double4x3 val)
		{
			return new double4x3(-val.c0, -val.c1, -val.c2);
		}

		// Token: 0x06001013 RID: 4115 RVA: 0x0002F424 File Offset: 0x0002D624
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double4x3 operator +(double4x3 val)
		{
			return new double4x3(+val.c0, +val.c1, +val.c2);
		}

		// Token: 0x06001014 RID: 4116 RVA: 0x0002F44C File Offset: 0x0002D64C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool4x3 operator ==(double4x3 lhs, double4x3 rhs)
		{
			return new bool4x3(lhs.c0 == rhs.c0, lhs.c1 == rhs.c1, lhs.c2 == rhs.c2);
		}

		// Token: 0x06001015 RID: 4117 RVA: 0x0002F486 File Offset: 0x0002D686
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool4x3 operator ==(double4x3 lhs, double rhs)
		{
			return new bool4x3(lhs.c0 == rhs, lhs.c1 == rhs, lhs.c2 == rhs);
		}

		// Token: 0x06001016 RID: 4118 RVA: 0x0002F4B1 File Offset: 0x0002D6B1
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool4x3 operator ==(double lhs, double4x3 rhs)
		{
			return new bool4x3(lhs == rhs.c0, lhs == rhs.c1, lhs == rhs.c2);
		}

		// Token: 0x06001017 RID: 4119 RVA: 0x0002F4DC File Offset: 0x0002D6DC
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool4x3 operator !=(double4x3 lhs, double4x3 rhs)
		{
			return new bool4x3(lhs.c0 != rhs.c0, lhs.c1 != rhs.c1, lhs.c2 != rhs.c2);
		}

		// Token: 0x06001018 RID: 4120 RVA: 0x0002F516 File Offset: 0x0002D716
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool4x3 operator !=(double4x3 lhs, double rhs)
		{
			return new bool4x3(lhs.c0 != rhs, lhs.c1 != rhs, lhs.c2 != rhs);
		}

		// Token: 0x06001019 RID: 4121 RVA: 0x0002F541 File Offset: 0x0002D741
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool4x3 operator !=(double lhs, double4x3 rhs)
		{
			return new bool4x3(lhs != rhs.c0, lhs != rhs.c1, lhs != rhs.c2);
		}

		// Token: 0x170003D9 RID: 985
		public unsafe ref double4 this[int index]
		{
			get
			{
				fixed (double4x3* ptr = &this)
				{
					return ref *(double4*)(ptr + (IntPtr)index * (IntPtr)sizeof(double4) / (IntPtr)sizeof(double4x3));
				}
			}
		}

		// Token: 0x0600101B RID: 4123 RVA: 0x0002F587 File Offset: 0x0002D787
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool Equals(double4x3 rhs)
		{
			return this.c0.Equals(rhs.c0) && this.c1.Equals(rhs.c1) && this.c2.Equals(rhs.c2);
		}

		// Token: 0x0600101C RID: 4124 RVA: 0x0002F5C4 File Offset: 0x0002D7C4
		public override bool Equals(object o)
		{
			if (o is double4x3)
			{
				double4x3 double4x = (double4x3)o;
				return this.Equals(double4x);
			}
			return false;
		}

		// Token: 0x0600101D RID: 4125 RVA: 0x0002F5E9 File Offset: 0x0002D7E9
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public override int GetHashCode()
		{
			return (int)math.hash(this);
		}

		// Token: 0x0600101E RID: 4126 RVA: 0x0002F5F8 File Offset: 0x0002D7F8
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public override string ToString()
		{
			return string.Format("double4x3({0}, {1}, {2},  {3}, {4}, {5},  {6}, {7}, {8},  {9}, {10}, {11})", new object[]
			{
				this.c0.x,
				this.c1.x,
				this.c2.x,
				this.c0.y,
				this.c1.y,
				this.c2.y,
				this.c0.z,
				this.c1.z,
				this.c2.z,
				this.c0.w,
				this.c1.w,
				this.c2.w
			});
		}

		// Token: 0x0600101F RID: 4127 RVA: 0x0002F700 File Offset: 0x0002D900
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public string ToString(string format, IFormatProvider formatProvider)
		{
			return string.Format("double4x3({0}, {1}, {2},  {3}, {4}, {5},  {6}, {7}, {8},  {9}, {10}, {11})", new object[]
			{
				this.c0.x.ToString(format, formatProvider),
				this.c1.x.ToString(format, formatProvider),
				this.c2.x.ToString(format, formatProvider),
				this.c0.y.ToString(format, formatProvider),
				this.c1.y.ToString(format, formatProvider),
				this.c2.y.ToString(format, formatProvider),
				this.c0.z.ToString(format, formatProvider),
				this.c1.z.ToString(format, formatProvider),
				this.c2.z.ToString(format, formatProvider),
				this.c0.w.ToString(format, formatProvider),
				this.c1.w.ToString(format, formatProvider),
				this.c2.w.ToString(format, formatProvider)
			});
		}

		// Token: 0x04000072 RID: 114
		public double4 c0;

		// Token: 0x04000073 RID: 115
		public double4 c1;

		// Token: 0x04000074 RID: 116
		public double4 c2;

		// Token: 0x04000075 RID: 117
		public static readonly double4x3 zero;
	}
}
