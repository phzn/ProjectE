using System;
using System.Runtime.CompilerServices;
using Unity.IL2CPP.CompilerServices;

namespace Unity.Mathematics
{
	// Token: 0x02000034 RID: 52
	[Il2CppEagerStaticClassConstruction]
	[Serializable]
	public struct int3x3 : IEquatable<int3x3>, IFormattable
	{
		// Token: 0x06001AD5 RID: 6869 RVA: 0x00049C57 File Offset: 0x00047E57
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int3x3(int3 c0, int3 c1, int3 c2)
		{
			this.c0 = c0;
			this.c1 = c1;
			this.c2 = c2;
		}

		// Token: 0x06001AD6 RID: 6870 RVA: 0x00049C6E File Offset: 0x00047E6E
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int3x3(int m00, int m01, int m02, int m10, int m11, int m12, int m20, int m21, int m22)
		{
			this.c0 = new int3(m00, m10, m20);
			this.c1 = new int3(m01, m11, m21);
			this.c2 = new int3(m02, m12, m22);
		}

		// Token: 0x06001AD7 RID: 6871 RVA: 0x00049CA0 File Offset: 0x00047EA0
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int3x3(int v)
		{
			this.c0 = v;
			this.c1 = v;
			this.c2 = v;
		}

		// Token: 0x06001AD8 RID: 6872 RVA: 0x00049CC8 File Offset: 0x00047EC8
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int3x3(bool v)
		{
			this.c0 = math.select(new int3(0), new int3(1), v);
			this.c1 = math.select(new int3(0), new int3(1), v);
			this.c2 = math.select(new int3(0), new int3(1), v);
		}

		// Token: 0x06001AD9 RID: 6873 RVA: 0x00049D20 File Offset: 0x00047F20
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int3x3(bool3x3 v)
		{
			this.c0 = math.select(new int3(0), new int3(1), v.c0);
			this.c1 = math.select(new int3(0), new int3(1), v.c1);
			this.c2 = math.select(new int3(0), new int3(1), v.c2);
		}

		// Token: 0x06001ADA RID: 6874 RVA: 0x00049D84 File Offset: 0x00047F84
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int3x3(uint v)
		{
			this.c0 = (int3)v;
			this.c1 = (int3)v;
			this.c2 = (int3)v;
		}

		// Token: 0x06001ADB RID: 6875 RVA: 0x00049DAA File Offset: 0x00047FAA
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int3x3(uint3x3 v)
		{
			this.c0 = (int3)v.c0;
			this.c1 = (int3)v.c1;
			this.c2 = (int3)v.c2;
		}

		// Token: 0x06001ADC RID: 6876 RVA: 0x00049DDF File Offset: 0x00047FDF
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int3x3(float v)
		{
			this.c0 = (int3)v;
			this.c1 = (int3)v;
			this.c2 = (int3)v;
		}

		// Token: 0x06001ADD RID: 6877 RVA: 0x00049E05 File Offset: 0x00048005
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int3x3(float3x3 v)
		{
			this.c0 = (int3)v.c0;
			this.c1 = (int3)v.c1;
			this.c2 = (int3)v.c2;
		}

		// Token: 0x06001ADE RID: 6878 RVA: 0x00049E3A File Offset: 0x0004803A
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int3x3(double v)
		{
			this.c0 = (int3)v;
			this.c1 = (int3)v;
			this.c2 = (int3)v;
		}

		// Token: 0x06001ADF RID: 6879 RVA: 0x00049E60 File Offset: 0x00048060
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int3x3(double3x3 v)
		{
			this.c0 = (int3)v.c0;
			this.c1 = (int3)v.c1;
			this.c2 = (int3)v.c2;
		}

		// Token: 0x06001AE0 RID: 6880 RVA: 0x00049E95 File Offset: 0x00048095
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static implicit operator int3x3(int v)
		{
			return new int3x3(v);
		}

		// Token: 0x06001AE1 RID: 6881 RVA: 0x00049E9D File Offset: 0x0004809D
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static explicit operator int3x3(bool v)
		{
			return new int3x3(v);
		}

		// Token: 0x06001AE2 RID: 6882 RVA: 0x00049EA5 File Offset: 0x000480A5
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static explicit operator int3x3(bool3x3 v)
		{
			return new int3x3(v);
		}

		// Token: 0x06001AE3 RID: 6883 RVA: 0x00049EAD File Offset: 0x000480AD
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static explicit operator int3x3(uint v)
		{
			return new int3x3(v);
		}

		// Token: 0x06001AE4 RID: 6884 RVA: 0x00049EB5 File Offset: 0x000480B5
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static explicit operator int3x3(uint3x3 v)
		{
			return new int3x3(v);
		}

		// Token: 0x06001AE5 RID: 6885 RVA: 0x00049EBD File Offset: 0x000480BD
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static explicit operator int3x3(float v)
		{
			return new int3x3(v);
		}

		// Token: 0x06001AE6 RID: 6886 RVA: 0x00049EC5 File Offset: 0x000480C5
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static explicit operator int3x3(float3x3 v)
		{
			return new int3x3(v);
		}

		// Token: 0x06001AE7 RID: 6887 RVA: 0x00049ECD File Offset: 0x000480CD
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static explicit operator int3x3(double v)
		{
			return new int3x3(v);
		}

		// Token: 0x06001AE8 RID: 6888 RVA: 0x00049ED5 File Offset: 0x000480D5
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static explicit operator int3x3(double3x3 v)
		{
			return new int3x3(v);
		}

		// Token: 0x06001AE9 RID: 6889 RVA: 0x00049EDD File Offset: 0x000480DD
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int3x3 operator *(int3x3 lhs, int3x3 rhs)
		{
			return new int3x3(lhs.c0 * rhs.c0, lhs.c1 * rhs.c1, lhs.c2 * rhs.c2);
		}

		// Token: 0x06001AEA RID: 6890 RVA: 0x00049F17 File Offset: 0x00048117
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int3x3 operator *(int3x3 lhs, int rhs)
		{
			return new int3x3(lhs.c0 * rhs, lhs.c1 * rhs, lhs.c2 * rhs);
		}

		// Token: 0x06001AEB RID: 6891 RVA: 0x00049F42 File Offset: 0x00048142
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int3x3 operator *(int lhs, int3x3 rhs)
		{
			return new int3x3(lhs * rhs.c0, lhs * rhs.c1, lhs * rhs.c2);
		}

		// Token: 0x06001AEC RID: 6892 RVA: 0x00049F6D File Offset: 0x0004816D
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int3x3 operator +(int3x3 lhs, int3x3 rhs)
		{
			return new int3x3(lhs.c0 + rhs.c0, lhs.c1 + rhs.c1, lhs.c2 + rhs.c2);
		}

		// Token: 0x06001AED RID: 6893 RVA: 0x00049FA7 File Offset: 0x000481A7
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int3x3 operator +(int3x3 lhs, int rhs)
		{
			return new int3x3(lhs.c0 + rhs, lhs.c1 + rhs, lhs.c2 + rhs);
		}

		// Token: 0x06001AEE RID: 6894 RVA: 0x00049FD2 File Offset: 0x000481D2
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int3x3 operator +(int lhs, int3x3 rhs)
		{
			return new int3x3(lhs + rhs.c0, lhs + rhs.c1, lhs + rhs.c2);
		}

		// Token: 0x06001AEF RID: 6895 RVA: 0x00049FFD File Offset: 0x000481FD
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int3x3 operator -(int3x3 lhs, int3x3 rhs)
		{
			return new int3x3(lhs.c0 - rhs.c0, lhs.c1 - rhs.c1, lhs.c2 - rhs.c2);
		}

		// Token: 0x06001AF0 RID: 6896 RVA: 0x0004A037 File Offset: 0x00048237
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int3x3 operator -(int3x3 lhs, int rhs)
		{
			return new int3x3(lhs.c0 - rhs, lhs.c1 - rhs, lhs.c2 - rhs);
		}

		// Token: 0x06001AF1 RID: 6897 RVA: 0x0004A062 File Offset: 0x00048262
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int3x3 operator -(int lhs, int3x3 rhs)
		{
			return new int3x3(lhs - rhs.c0, lhs - rhs.c1, lhs - rhs.c2);
		}

		// Token: 0x06001AF2 RID: 6898 RVA: 0x0004A08D File Offset: 0x0004828D
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int3x3 operator /(int3x3 lhs, int3x3 rhs)
		{
			return new int3x3(lhs.c0 / rhs.c0, lhs.c1 / rhs.c1, lhs.c2 / rhs.c2);
		}

		// Token: 0x06001AF3 RID: 6899 RVA: 0x0004A0C7 File Offset: 0x000482C7
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int3x3 operator /(int3x3 lhs, int rhs)
		{
			return new int3x3(lhs.c0 / rhs, lhs.c1 / rhs, lhs.c2 / rhs);
		}

		// Token: 0x06001AF4 RID: 6900 RVA: 0x0004A0F2 File Offset: 0x000482F2
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int3x3 operator /(int lhs, int3x3 rhs)
		{
			return new int3x3(lhs / rhs.c0, lhs / rhs.c1, lhs / rhs.c2);
		}

		// Token: 0x06001AF5 RID: 6901 RVA: 0x0004A11D File Offset: 0x0004831D
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int3x3 operator %(int3x3 lhs, int3x3 rhs)
		{
			return new int3x3(lhs.c0 % rhs.c0, lhs.c1 % rhs.c1, lhs.c2 % rhs.c2);
		}

		// Token: 0x06001AF6 RID: 6902 RVA: 0x0004A157 File Offset: 0x00048357
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int3x3 operator %(int3x3 lhs, int rhs)
		{
			return new int3x3(lhs.c0 % rhs, lhs.c1 % rhs, lhs.c2 % rhs);
		}

		// Token: 0x06001AF7 RID: 6903 RVA: 0x0004A182 File Offset: 0x00048382
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int3x3 operator %(int lhs, int3x3 rhs)
		{
			return new int3x3(lhs % rhs.c0, lhs % rhs.c1, lhs % rhs.c2);
		}

		// Token: 0x06001AF8 RID: 6904 RVA: 0x0004A1B0 File Offset: 0x000483B0
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int3x3 operator ++(int3x3 val)
		{
			int3 @int = int3.op_Increment(val.c0);
			val.c0 = @int;
			int3 int2 = @int;
			@int = int3.op_Increment(val.c1);
			val.c1 = @int;
			int3 int3 = @int;
			@int = int3.op_Increment(val.c2);
			val.c2 = @int;
			return new int3x3(int2, int3, @int);
		}

		// Token: 0x06001AF9 RID: 6905 RVA: 0x0004A210 File Offset: 0x00048410
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int3x3 operator --(int3x3 val)
		{
			int3 @int = int3.op_Decrement(val.c0);
			val.c0 = @int;
			int3 int2 = @int;
			@int = int3.op_Decrement(val.c1);
			val.c1 = @int;
			int3 int3 = @int;
			@int = int3.op_Decrement(val.c2);
			val.c2 = @int;
			return new int3x3(int2, int3, @int);
		}

		// Token: 0x06001AFA RID: 6906 RVA: 0x0004A270 File Offset: 0x00048470
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool3x3 operator <(int3x3 lhs, int3x3 rhs)
		{
			return new bool3x3(lhs.c0 < rhs.c0, lhs.c1 < rhs.c1, lhs.c2 < rhs.c2);
		}

		// Token: 0x06001AFB RID: 6907 RVA: 0x0004A2AA File Offset: 0x000484AA
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool3x3 operator <(int3x3 lhs, int rhs)
		{
			return new bool3x3(lhs.c0 < rhs, lhs.c1 < rhs, lhs.c2 < rhs);
		}

		// Token: 0x06001AFC RID: 6908 RVA: 0x0004A2D5 File Offset: 0x000484D5
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool3x3 operator <(int lhs, int3x3 rhs)
		{
			return new bool3x3(lhs < rhs.c0, lhs < rhs.c1, lhs < rhs.c2);
		}

		// Token: 0x06001AFD RID: 6909 RVA: 0x0004A300 File Offset: 0x00048500
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool3x3 operator <=(int3x3 lhs, int3x3 rhs)
		{
			return new bool3x3(lhs.c0 <= rhs.c0, lhs.c1 <= rhs.c1, lhs.c2 <= rhs.c2);
		}

		// Token: 0x06001AFE RID: 6910 RVA: 0x0004A33A File Offset: 0x0004853A
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool3x3 operator <=(int3x3 lhs, int rhs)
		{
			return new bool3x3(lhs.c0 <= rhs, lhs.c1 <= rhs, lhs.c2 <= rhs);
		}

		// Token: 0x06001AFF RID: 6911 RVA: 0x0004A365 File Offset: 0x00048565
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool3x3 operator <=(int lhs, int3x3 rhs)
		{
			return new bool3x3(lhs <= rhs.c0, lhs <= rhs.c1, lhs <= rhs.c2);
		}

		// Token: 0x06001B00 RID: 6912 RVA: 0x0004A390 File Offset: 0x00048590
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool3x3 operator >(int3x3 lhs, int3x3 rhs)
		{
			return new bool3x3(lhs.c0 > rhs.c0, lhs.c1 > rhs.c1, lhs.c2 > rhs.c2);
		}

		// Token: 0x06001B01 RID: 6913 RVA: 0x0004A3CA File Offset: 0x000485CA
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool3x3 operator >(int3x3 lhs, int rhs)
		{
			return new bool3x3(lhs.c0 > rhs, lhs.c1 > rhs, lhs.c2 > rhs);
		}

		// Token: 0x06001B02 RID: 6914 RVA: 0x0004A3F5 File Offset: 0x000485F5
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool3x3 operator >(int lhs, int3x3 rhs)
		{
			return new bool3x3(lhs > rhs.c0, lhs > rhs.c1, lhs > rhs.c2);
		}

		// Token: 0x06001B03 RID: 6915 RVA: 0x0004A420 File Offset: 0x00048620
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool3x3 operator >=(int3x3 lhs, int3x3 rhs)
		{
			return new bool3x3(lhs.c0 >= rhs.c0, lhs.c1 >= rhs.c1, lhs.c2 >= rhs.c2);
		}

		// Token: 0x06001B04 RID: 6916 RVA: 0x0004A45A File Offset: 0x0004865A
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool3x3 operator >=(int3x3 lhs, int rhs)
		{
			return new bool3x3(lhs.c0 >= rhs, lhs.c1 >= rhs, lhs.c2 >= rhs);
		}

		// Token: 0x06001B05 RID: 6917 RVA: 0x0004A485 File Offset: 0x00048685
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool3x3 operator >=(int lhs, int3x3 rhs)
		{
			return new bool3x3(lhs >= rhs.c0, lhs >= rhs.c1, lhs >= rhs.c2);
		}

		// Token: 0x06001B06 RID: 6918 RVA: 0x0004A4B0 File Offset: 0x000486B0
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int3x3 operator -(int3x3 val)
		{
			return new int3x3(-val.c0, -val.c1, -val.c2);
		}

		// Token: 0x06001B07 RID: 6919 RVA: 0x0004A4D8 File Offset: 0x000486D8
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int3x3 operator +(int3x3 val)
		{
			return new int3x3(+val.c0, +val.c1, +val.c2);
		}

		// Token: 0x06001B08 RID: 6920 RVA: 0x0004A500 File Offset: 0x00048700
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int3x3 operator <<(int3x3 x, int n)
		{
			return new int3x3(x.c0 << n, x.c1 << n, x.c2 << n);
		}

		// Token: 0x06001B09 RID: 6921 RVA: 0x0004A52B File Offset: 0x0004872B
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int3x3 operator >>(int3x3 x, int n)
		{
			return new int3x3(x.c0 >> n, x.c1 >> n, x.c2 >> n);
		}

		// Token: 0x06001B0A RID: 6922 RVA: 0x0004A556 File Offset: 0x00048756
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool3x3 operator ==(int3x3 lhs, int3x3 rhs)
		{
			return new bool3x3(lhs.c0 == rhs.c0, lhs.c1 == rhs.c1, lhs.c2 == rhs.c2);
		}

		// Token: 0x06001B0B RID: 6923 RVA: 0x0004A590 File Offset: 0x00048790
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool3x3 operator ==(int3x3 lhs, int rhs)
		{
			return new bool3x3(lhs.c0 == rhs, lhs.c1 == rhs, lhs.c2 == rhs);
		}

		// Token: 0x06001B0C RID: 6924 RVA: 0x0004A5BB File Offset: 0x000487BB
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool3x3 operator ==(int lhs, int3x3 rhs)
		{
			return new bool3x3(lhs == rhs.c0, lhs == rhs.c1, lhs == rhs.c2);
		}

		// Token: 0x06001B0D RID: 6925 RVA: 0x0004A5E6 File Offset: 0x000487E6
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool3x3 operator !=(int3x3 lhs, int3x3 rhs)
		{
			return new bool3x3(lhs.c0 != rhs.c0, lhs.c1 != rhs.c1, lhs.c2 != rhs.c2);
		}

		// Token: 0x06001B0E RID: 6926 RVA: 0x0004A620 File Offset: 0x00048820
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool3x3 operator !=(int3x3 lhs, int rhs)
		{
			return new bool3x3(lhs.c0 != rhs, lhs.c1 != rhs, lhs.c2 != rhs);
		}

		// Token: 0x06001B0F RID: 6927 RVA: 0x0004A64B File Offset: 0x0004884B
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool3x3 operator !=(int lhs, int3x3 rhs)
		{
			return new bool3x3(lhs != rhs.c0, lhs != rhs.c1, lhs != rhs.c2);
		}

		// Token: 0x06001B10 RID: 6928 RVA: 0x0004A676 File Offset: 0x00048876
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int3x3 operator ~(int3x3 val)
		{
			return new int3x3(~val.c0, ~val.c1, ~val.c2);
		}

		// Token: 0x06001B11 RID: 6929 RVA: 0x0004A69E File Offset: 0x0004889E
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int3x3 operator &(int3x3 lhs, int3x3 rhs)
		{
			return new int3x3(lhs.c0 & rhs.c0, lhs.c1 & rhs.c1, lhs.c2 & rhs.c2);
		}

		// Token: 0x06001B12 RID: 6930 RVA: 0x0004A6D8 File Offset: 0x000488D8
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int3x3 operator &(int3x3 lhs, int rhs)
		{
			return new int3x3(lhs.c0 & rhs, lhs.c1 & rhs, lhs.c2 & rhs);
		}

		// Token: 0x06001B13 RID: 6931 RVA: 0x0004A703 File Offset: 0x00048903
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int3x3 operator &(int lhs, int3x3 rhs)
		{
			return new int3x3(lhs & rhs.c0, lhs & rhs.c1, lhs & rhs.c2);
		}

		// Token: 0x06001B14 RID: 6932 RVA: 0x0004A72E File Offset: 0x0004892E
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int3x3 operator |(int3x3 lhs, int3x3 rhs)
		{
			return new int3x3(lhs.c0 | rhs.c0, lhs.c1 | rhs.c1, lhs.c2 | rhs.c2);
		}

		// Token: 0x06001B15 RID: 6933 RVA: 0x0004A768 File Offset: 0x00048968
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int3x3 operator |(int3x3 lhs, int rhs)
		{
			return new int3x3(lhs.c0 | rhs, lhs.c1 | rhs, lhs.c2 | rhs);
		}

		// Token: 0x06001B16 RID: 6934 RVA: 0x0004A793 File Offset: 0x00048993
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int3x3 operator |(int lhs, int3x3 rhs)
		{
			return new int3x3(lhs | rhs.c0, lhs | rhs.c1, lhs | rhs.c2);
		}

		// Token: 0x06001B17 RID: 6935 RVA: 0x0004A7BE File Offset: 0x000489BE
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int3x3 operator ^(int3x3 lhs, int3x3 rhs)
		{
			return new int3x3(lhs.c0 ^ rhs.c0, lhs.c1 ^ rhs.c1, lhs.c2 ^ rhs.c2);
		}

		// Token: 0x06001B18 RID: 6936 RVA: 0x0004A7F8 File Offset: 0x000489F8
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int3x3 operator ^(int3x3 lhs, int rhs)
		{
			return new int3x3(lhs.c0 ^ rhs, lhs.c1 ^ rhs, lhs.c2 ^ rhs);
		}

		// Token: 0x06001B19 RID: 6937 RVA: 0x0004A823 File Offset: 0x00048A23
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int3x3 operator ^(int lhs, int3x3 rhs)
		{
			return new int3x3(lhs ^ rhs.c0, lhs ^ rhs.c1, lhs ^ rhs.c2);
		}

		// Token: 0x17000847 RID: 2119
		public unsafe ref int3 this[int index]
		{
			get
			{
				fixed (int3x3* ptr = &this)
				{
					return ref *(int3*)(ptr + (IntPtr)index * (IntPtr)sizeof(int3) / (IntPtr)sizeof(int3x3));
				}
			}
		}

		// Token: 0x06001B1B RID: 6939 RVA: 0x0004A86B File Offset: 0x00048A6B
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool Equals(int3x3 rhs)
		{
			return this.c0.Equals(rhs.c0) && this.c1.Equals(rhs.c1) && this.c2.Equals(rhs.c2);
		}

		// Token: 0x06001B1C RID: 6940 RVA: 0x0004A8A8 File Offset: 0x00048AA8
		public override bool Equals(object o)
		{
			if (o is int3x3)
			{
				int3x3 int3x = (int3x3)o;
				return this.Equals(int3x);
			}
			return false;
		}

		// Token: 0x06001B1D RID: 6941 RVA: 0x0004A8CD File Offset: 0x00048ACD
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public override int GetHashCode()
		{
			return (int)math.hash(this);
		}

		// Token: 0x06001B1E RID: 6942 RVA: 0x0004A8DC File Offset: 0x00048ADC
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public override string ToString()
		{
			return string.Format("int3x3({0}, {1}, {2},  {3}, {4}, {5},  {6}, {7}, {8})", new object[]
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

		// Token: 0x06001B1F RID: 6943 RVA: 0x0004A9A8 File Offset: 0x00048BA8
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public string ToString(string format, IFormatProvider formatProvider)
		{
			return string.Format("int3x3({0}, {1}, {2},  {3}, {4}, {5},  {6}, {7}, {8})", new object[]
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

		// Token: 0x040000D4 RID: 212
		public int3 c0;

		// Token: 0x040000D5 RID: 213
		public int3 c1;

		// Token: 0x040000D6 RID: 214
		public int3 c2;

		// Token: 0x040000D7 RID: 215
		public static readonly int3x3 identity = new int3x3(1, 0, 0, 0, 1, 0, 0, 0, 1);

		// Token: 0x040000D8 RID: 216
		public static readonly int3x3 zero;
	}
}
