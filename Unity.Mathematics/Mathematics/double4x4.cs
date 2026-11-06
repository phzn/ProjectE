using System;
using System.Runtime.CompilerServices;
using Unity.IL2CPP.CompilerServices;

namespace Unity.Mathematics
{
	// Token: 0x0200001D RID: 29
	[Il2CppEagerStaticClassConstruction]
	[Serializable]
	public struct double4x4 : IEquatable<double4x4>, IFormattable
	{
		// Token: 0x06001020 RID: 4128 RVA: 0x0002F81D File Offset: 0x0002DA1D
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public double4x4(double4 c0, double4 c1, double4 c2, double4 c3)
		{
			this.c0 = c0;
			this.c1 = c1;
			this.c2 = c2;
			this.c3 = c3;
		}

		// Token: 0x06001021 RID: 4129 RVA: 0x0002F83C File Offset: 0x0002DA3C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public double4x4(double m00, double m01, double m02, double m03, double m10, double m11, double m12, double m13, double m20, double m21, double m22, double m23, double m30, double m31, double m32, double m33)
		{
			this.c0 = new double4(m00, m10, m20, m30);
			this.c1 = new double4(m01, m11, m21, m31);
			this.c2 = new double4(m02, m12, m22, m32);
			this.c3 = new double4(m03, m13, m23, m33);
		}

		// Token: 0x06001022 RID: 4130 RVA: 0x0002F892 File Offset: 0x0002DA92
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public double4x4(double v)
		{
			this.c0 = v;
			this.c1 = v;
			this.c2 = v;
			this.c3 = v;
		}

		// Token: 0x06001023 RID: 4131 RVA: 0x0002F8C4 File Offset: 0x0002DAC4
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public double4x4(bool v)
		{
			this.c0 = math.select(new double4(0.0), new double4(1.0), v);
			this.c1 = math.select(new double4(0.0), new double4(1.0), v);
			this.c2 = math.select(new double4(0.0), new double4(1.0), v);
			this.c3 = math.select(new double4(0.0), new double4(1.0), v);
		}

		// Token: 0x06001024 RID: 4132 RVA: 0x0002F974 File Offset: 0x0002DB74
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public double4x4(bool4x4 v)
		{
			this.c0 = math.select(new double4(0.0), new double4(1.0), v.c0);
			this.c1 = math.select(new double4(0.0), new double4(1.0), v.c1);
			this.c2 = math.select(new double4(0.0), new double4(1.0), v.c2);
			this.c3 = math.select(new double4(0.0), new double4(1.0), v.c3);
		}

		// Token: 0x06001025 RID: 4133 RVA: 0x0002FA35 File Offset: 0x0002DC35
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public double4x4(int v)
		{
			this.c0 = v;
			this.c1 = v;
			this.c2 = v;
			this.c3 = v;
		}

		// Token: 0x06001026 RID: 4134 RVA: 0x0002FA68 File Offset: 0x0002DC68
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public double4x4(int4x4 v)
		{
			this.c0 = v.c0;
			this.c1 = v.c1;
			this.c2 = v.c2;
			this.c3 = v.c3;
		}

		// Token: 0x06001027 RID: 4135 RVA: 0x0002FAB9 File Offset: 0x0002DCB9
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public double4x4(uint v)
		{
			this.c0 = v;
			this.c1 = v;
			this.c2 = v;
			this.c3 = v;
		}

		// Token: 0x06001028 RID: 4136 RVA: 0x0002FAEC File Offset: 0x0002DCEC
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public double4x4(uint4x4 v)
		{
			this.c0 = v.c0;
			this.c1 = v.c1;
			this.c2 = v.c2;
			this.c3 = v.c3;
		}

		// Token: 0x06001029 RID: 4137 RVA: 0x0002FB3D File Offset: 0x0002DD3D
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public double4x4(float v)
		{
			this.c0 = v;
			this.c1 = v;
			this.c2 = v;
			this.c3 = v;
		}

		// Token: 0x0600102A RID: 4138 RVA: 0x0002FB70 File Offset: 0x0002DD70
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public double4x4(float4x4 v)
		{
			this.c0 = v.c0;
			this.c1 = v.c1;
			this.c2 = v.c2;
			this.c3 = v.c3;
		}

		// Token: 0x0600102B RID: 4139 RVA: 0x0002FBC1 File Offset: 0x0002DDC1
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static implicit operator double4x4(double v)
		{
			return new double4x4(v);
		}

		// Token: 0x0600102C RID: 4140 RVA: 0x0002FBC9 File Offset: 0x0002DDC9
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static explicit operator double4x4(bool v)
		{
			return new double4x4(v);
		}

		// Token: 0x0600102D RID: 4141 RVA: 0x0002FBD1 File Offset: 0x0002DDD1
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static explicit operator double4x4(bool4x4 v)
		{
			return new double4x4(v);
		}

		// Token: 0x0600102E RID: 4142 RVA: 0x0002FBD9 File Offset: 0x0002DDD9
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static implicit operator double4x4(int v)
		{
			return new double4x4(v);
		}

		// Token: 0x0600102F RID: 4143 RVA: 0x0002FBE1 File Offset: 0x0002DDE1
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static implicit operator double4x4(int4x4 v)
		{
			return new double4x4(v);
		}

		// Token: 0x06001030 RID: 4144 RVA: 0x0002FBE9 File Offset: 0x0002DDE9
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static implicit operator double4x4(uint v)
		{
			return new double4x4(v);
		}

		// Token: 0x06001031 RID: 4145 RVA: 0x0002FBF1 File Offset: 0x0002DDF1
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static implicit operator double4x4(uint4x4 v)
		{
			return new double4x4(v);
		}

		// Token: 0x06001032 RID: 4146 RVA: 0x0002FBF9 File Offset: 0x0002DDF9
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static implicit operator double4x4(float v)
		{
			return new double4x4(v);
		}

		// Token: 0x06001033 RID: 4147 RVA: 0x0002FC01 File Offset: 0x0002DE01
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static implicit operator double4x4(float4x4 v)
		{
			return new double4x4(v);
		}

		// Token: 0x06001034 RID: 4148 RVA: 0x0002FC0C File Offset: 0x0002DE0C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double4x4 operator *(double4x4 lhs, double4x4 rhs)
		{
			return new double4x4(lhs.c0 * rhs.c0, lhs.c1 * rhs.c1, lhs.c2 * rhs.c2, lhs.c3 * rhs.c3);
		}

		// Token: 0x06001035 RID: 4149 RVA: 0x0002FC62 File Offset: 0x0002DE62
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double4x4 operator *(double4x4 lhs, double rhs)
		{
			return new double4x4(lhs.c0 * rhs, lhs.c1 * rhs, lhs.c2 * rhs, lhs.c3 * rhs);
		}

		// Token: 0x06001036 RID: 4150 RVA: 0x0002FC99 File Offset: 0x0002DE99
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double4x4 operator *(double lhs, double4x4 rhs)
		{
			return new double4x4(lhs * rhs.c0, lhs * rhs.c1, lhs * rhs.c2, lhs * rhs.c3);
		}

		// Token: 0x06001037 RID: 4151 RVA: 0x0002FCD0 File Offset: 0x0002DED0
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double4x4 operator +(double4x4 lhs, double4x4 rhs)
		{
			return new double4x4(lhs.c0 + rhs.c0, lhs.c1 + rhs.c1, lhs.c2 + rhs.c2, lhs.c3 + rhs.c3);
		}

		// Token: 0x06001038 RID: 4152 RVA: 0x0002FD26 File Offset: 0x0002DF26
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double4x4 operator +(double4x4 lhs, double rhs)
		{
			return new double4x4(lhs.c0 + rhs, lhs.c1 + rhs, lhs.c2 + rhs, lhs.c3 + rhs);
		}

		// Token: 0x06001039 RID: 4153 RVA: 0x0002FD5D File Offset: 0x0002DF5D
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double4x4 operator +(double lhs, double4x4 rhs)
		{
			return new double4x4(lhs + rhs.c0, lhs + rhs.c1, lhs + rhs.c2, lhs + rhs.c3);
		}

		// Token: 0x0600103A RID: 4154 RVA: 0x0002FD94 File Offset: 0x0002DF94
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double4x4 operator -(double4x4 lhs, double4x4 rhs)
		{
			return new double4x4(lhs.c0 - rhs.c0, lhs.c1 - rhs.c1, lhs.c2 - rhs.c2, lhs.c3 - rhs.c3);
		}

		// Token: 0x0600103B RID: 4155 RVA: 0x0002FDEA File Offset: 0x0002DFEA
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double4x4 operator -(double4x4 lhs, double rhs)
		{
			return new double4x4(lhs.c0 - rhs, lhs.c1 - rhs, lhs.c2 - rhs, lhs.c3 - rhs);
		}

		// Token: 0x0600103C RID: 4156 RVA: 0x0002FE21 File Offset: 0x0002E021
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double4x4 operator -(double lhs, double4x4 rhs)
		{
			return new double4x4(lhs - rhs.c0, lhs - rhs.c1, lhs - rhs.c2, lhs - rhs.c3);
		}

		// Token: 0x0600103D RID: 4157 RVA: 0x0002FE58 File Offset: 0x0002E058
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double4x4 operator /(double4x4 lhs, double4x4 rhs)
		{
			return new double4x4(lhs.c0 / rhs.c0, lhs.c1 / rhs.c1, lhs.c2 / rhs.c2, lhs.c3 / rhs.c3);
		}

		// Token: 0x0600103E RID: 4158 RVA: 0x0002FEAE File Offset: 0x0002E0AE
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double4x4 operator /(double4x4 lhs, double rhs)
		{
			return new double4x4(lhs.c0 / rhs, lhs.c1 / rhs, lhs.c2 / rhs, lhs.c3 / rhs);
		}

		// Token: 0x0600103F RID: 4159 RVA: 0x0002FEE5 File Offset: 0x0002E0E5
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double4x4 operator /(double lhs, double4x4 rhs)
		{
			return new double4x4(lhs / rhs.c0, lhs / rhs.c1, lhs / rhs.c2, lhs / rhs.c3);
		}

		// Token: 0x06001040 RID: 4160 RVA: 0x0002FF1C File Offset: 0x0002E11C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double4x4 operator %(double4x4 lhs, double4x4 rhs)
		{
			return new double4x4(lhs.c0 % rhs.c0, lhs.c1 % rhs.c1, lhs.c2 % rhs.c2, lhs.c3 % rhs.c3);
		}

		// Token: 0x06001041 RID: 4161 RVA: 0x0002FF72 File Offset: 0x0002E172
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double4x4 operator %(double4x4 lhs, double rhs)
		{
			return new double4x4(lhs.c0 % rhs, lhs.c1 % rhs, lhs.c2 % rhs, lhs.c3 % rhs);
		}

		// Token: 0x06001042 RID: 4162 RVA: 0x0002FFA9 File Offset: 0x0002E1A9
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double4x4 operator %(double lhs, double4x4 rhs)
		{
			return new double4x4(lhs % rhs.c0, lhs % rhs.c1, lhs % rhs.c2, lhs % rhs.c3);
		}

		// Token: 0x06001043 RID: 4163 RVA: 0x0002FFE0 File Offset: 0x0002E1E0
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double4x4 operator ++(double4x4 val)
		{
			double4 @double = double4.op_Increment(val.c0);
			val.c0 = @double;
			double4 double2 = @double;
			@double = double4.op_Increment(val.c1);
			val.c1 = @double;
			double4 double3 = @double;
			@double = double4.op_Increment(val.c2);
			val.c2 = @double;
			double4 double4 = @double;
			@double = double4.op_Increment(val.c3);
			val.c3 = @double;
			return new double4x4(double2, double3, double4, @double);
		}

		// Token: 0x06001044 RID: 4164 RVA: 0x0003005C File Offset: 0x0002E25C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double4x4 operator --(double4x4 val)
		{
			double4 @double = double4.op_Decrement(val.c0);
			val.c0 = @double;
			double4 double2 = @double;
			@double = double4.op_Decrement(val.c1);
			val.c1 = @double;
			double4 double3 = @double;
			@double = double4.op_Decrement(val.c2);
			val.c2 = @double;
			double4 double4 = @double;
			@double = double4.op_Decrement(val.c3);
			val.c3 = @double;
			return new double4x4(double2, double3, double4, @double);
		}

		// Token: 0x06001045 RID: 4165 RVA: 0x000300D8 File Offset: 0x0002E2D8
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool4x4 operator <(double4x4 lhs, double4x4 rhs)
		{
			return new bool4x4(lhs.c0 < rhs.c0, lhs.c1 < rhs.c1, lhs.c2 < rhs.c2, lhs.c3 < rhs.c3);
		}

		// Token: 0x06001046 RID: 4166 RVA: 0x0003012E File Offset: 0x0002E32E
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool4x4 operator <(double4x4 lhs, double rhs)
		{
			return new bool4x4(lhs.c0 < rhs, lhs.c1 < rhs, lhs.c2 < rhs, lhs.c3 < rhs);
		}

		// Token: 0x06001047 RID: 4167 RVA: 0x00030165 File Offset: 0x0002E365
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool4x4 operator <(double lhs, double4x4 rhs)
		{
			return new bool4x4(lhs < rhs.c0, lhs < rhs.c1, lhs < rhs.c2, lhs < rhs.c3);
		}

		// Token: 0x06001048 RID: 4168 RVA: 0x0003019C File Offset: 0x0002E39C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool4x4 operator <=(double4x4 lhs, double4x4 rhs)
		{
			return new bool4x4(lhs.c0 <= rhs.c0, lhs.c1 <= rhs.c1, lhs.c2 <= rhs.c2, lhs.c3 <= rhs.c3);
		}

		// Token: 0x06001049 RID: 4169 RVA: 0x000301F2 File Offset: 0x0002E3F2
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool4x4 operator <=(double4x4 lhs, double rhs)
		{
			return new bool4x4(lhs.c0 <= rhs, lhs.c1 <= rhs, lhs.c2 <= rhs, lhs.c3 <= rhs);
		}

		// Token: 0x0600104A RID: 4170 RVA: 0x00030229 File Offset: 0x0002E429
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool4x4 operator <=(double lhs, double4x4 rhs)
		{
			return new bool4x4(lhs <= rhs.c0, lhs <= rhs.c1, lhs <= rhs.c2, lhs <= rhs.c3);
		}

		// Token: 0x0600104B RID: 4171 RVA: 0x00030260 File Offset: 0x0002E460
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool4x4 operator >(double4x4 lhs, double4x4 rhs)
		{
			return new bool4x4(lhs.c0 > rhs.c0, lhs.c1 > rhs.c1, lhs.c2 > rhs.c2, lhs.c3 > rhs.c3);
		}

		// Token: 0x0600104C RID: 4172 RVA: 0x000302B6 File Offset: 0x0002E4B6
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool4x4 operator >(double4x4 lhs, double rhs)
		{
			return new bool4x4(lhs.c0 > rhs, lhs.c1 > rhs, lhs.c2 > rhs, lhs.c3 > rhs);
		}

		// Token: 0x0600104D RID: 4173 RVA: 0x000302ED File Offset: 0x0002E4ED
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool4x4 operator >(double lhs, double4x4 rhs)
		{
			return new bool4x4(lhs > rhs.c0, lhs > rhs.c1, lhs > rhs.c2, lhs > rhs.c3);
		}

		// Token: 0x0600104E RID: 4174 RVA: 0x00030324 File Offset: 0x0002E524
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool4x4 operator >=(double4x4 lhs, double4x4 rhs)
		{
			return new bool4x4(lhs.c0 >= rhs.c0, lhs.c1 >= rhs.c1, lhs.c2 >= rhs.c2, lhs.c3 >= rhs.c3);
		}

		// Token: 0x0600104F RID: 4175 RVA: 0x0003037A File Offset: 0x0002E57A
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool4x4 operator >=(double4x4 lhs, double rhs)
		{
			return new bool4x4(lhs.c0 >= rhs, lhs.c1 >= rhs, lhs.c2 >= rhs, lhs.c3 >= rhs);
		}

		// Token: 0x06001050 RID: 4176 RVA: 0x000303B1 File Offset: 0x0002E5B1
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool4x4 operator >=(double lhs, double4x4 rhs)
		{
			return new bool4x4(lhs >= rhs.c0, lhs >= rhs.c1, lhs >= rhs.c2, lhs >= rhs.c3);
		}

		// Token: 0x06001051 RID: 4177 RVA: 0x000303E8 File Offset: 0x0002E5E8
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double4x4 operator -(double4x4 val)
		{
			return new double4x4(-val.c0, -val.c1, -val.c2, -val.c3);
		}

		// Token: 0x06001052 RID: 4178 RVA: 0x0003041B File Offset: 0x0002E61B
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double4x4 operator +(double4x4 val)
		{
			return new double4x4(+val.c0, +val.c1, +val.c2, +val.c3);
		}

		// Token: 0x06001053 RID: 4179 RVA: 0x00030450 File Offset: 0x0002E650
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool4x4 operator ==(double4x4 lhs, double4x4 rhs)
		{
			return new bool4x4(lhs.c0 == rhs.c0, lhs.c1 == rhs.c1, lhs.c2 == rhs.c2, lhs.c3 == rhs.c3);
		}

		// Token: 0x06001054 RID: 4180 RVA: 0x000304A6 File Offset: 0x0002E6A6
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool4x4 operator ==(double4x4 lhs, double rhs)
		{
			return new bool4x4(lhs.c0 == rhs, lhs.c1 == rhs, lhs.c2 == rhs, lhs.c3 == rhs);
		}

		// Token: 0x06001055 RID: 4181 RVA: 0x000304DD File Offset: 0x0002E6DD
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool4x4 operator ==(double lhs, double4x4 rhs)
		{
			return new bool4x4(lhs == rhs.c0, lhs == rhs.c1, lhs == rhs.c2, lhs == rhs.c3);
		}

		// Token: 0x06001056 RID: 4182 RVA: 0x00030514 File Offset: 0x0002E714
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool4x4 operator !=(double4x4 lhs, double4x4 rhs)
		{
			return new bool4x4(lhs.c0 != rhs.c0, lhs.c1 != rhs.c1, lhs.c2 != rhs.c2, lhs.c3 != rhs.c3);
		}

		// Token: 0x06001057 RID: 4183 RVA: 0x0003056A File Offset: 0x0002E76A
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool4x4 operator !=(double4x4 lhs, double rhs)
		{
			return new bool4x4(lhs.c0 != rhs, lhs.c1 != rhs, lhs.c2 != rhs, lhs.c3 != rhs);
		}

		// Token: 0x06001058 RID: 4184 RVA: 0x000305A1 File Offset: 0x0002E7A1
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool4x4 operator !=(double lhs, double4x4 rhs)
		{
			return new bool4x4(lhs != rhs.c0, lhs != rhs.c1, lhs != rhs.c2, lhs != rhs.c3);
		}

		// Token: 0x170003DA RID: 986
		public unsafe ref double4 this[int index]
		{
			get
			{
				fixed (double4x4* ptr = &this)
				{
					return ref *(double4*)(ptr + (IntPtr)index * (IntPtr)sizeof(double4) / (IntPtr)sizeof(double4x4));
				}
			}
		}

		// Token: 0x0600105A RID: 4186 RVA: 0x000305F4 File Offset: 0x0002E7F4
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool Equals(double4x4 rhs)
		{
			return this.c0.Equals(rhs.c0) && this.c1.Equals(rhs.c1) && this.c2.Equals(rhs.c2) && this.c3.Equals(rhs.c3);
		}

		// Token: 0x0600105B RID: 4187 RVA: 0x00030650 File Offset: 0x0002E850
		public override bool Equals(object o)
		{
			if (o is double4x4)
			{
				double4x4 double4x = (double4x4)o;
				return this.Equals(double4x);
			}
			return false;
		}

		// Token: 0x0600105C RID: 4188 RVA: 0x00030675 File Offset: 0x0002E875
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public override int GetHashCode()
		{
			return (int)math.hash(this);
		}

		// Token: 0x0600105D RID: 4189 RVA: 0x00030684 File Offset: 0x0002E884
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public override string ToString()
		{
			return string.Format("double4x4({0}, {1}, {2}, {3},  {4}, {5}, {6}, {7},  {8}, {9}, {10}, {11},  {12}, {13}, {14}, {15})", new object[]
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
				this.c3.z,
				this.c0.w,
				this.c1.w,
				this.c2.w,
				this.c3.w
			});
		}

		// Token: 0x0600105E RID: 4190 RVA: 0x000307DC File Offset: 0x0002E9DC
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public string ToString(string format, IFormatProvider formatProvider)
		{
			return string.Format("double4x4({0}, {1}, {2}, {3},  {4}, {5}, {6}, {7},  {8}, {9}, {10}, {11},  {12}, {13}, {14}, {15})", new object[]
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
				this.c3.z.ToString(format, formatProvider),
				this.c0.w.ToString(format, formatProvider),
				this.c1.w.ToString(format, formatProvider),
				this.c2.w.ToString(format, formatProvider),
				this.c3.w.ToString(format, formatProvider)
			});
		}

		// Token: 0x04000076 RID: 118
		public double4 c0;

		// Token: 0x04000077 RID: 119
		public double4 c1;

		// Token: 0x04000078 RID: 120
		public double4 c2;

		// Token: 0x04000079 RID: 121
		public double4 c3;

		// Token: 0x0400007A RID: 122
		public static readonly double4x4 identity = new double4x4(1.0, 0.0, 0.0, 0.0, 0.0, 1.0, 0.0, 0.0, 0.0, 0.0, 1.0, 0.0, 0.0, 0.0, 0.0, 1.0);

		// Token: 0x0400007B RID: 123
		public static readonly double4x4 zero;
	}
}
