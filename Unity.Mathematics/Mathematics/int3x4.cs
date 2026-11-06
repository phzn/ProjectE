using System;
using System.Runtime.CompilerServices;
using Unity.IL2CPP.CompilerServices;

namespace Unity.Mathematics
{
	// Token: 0x02000035 RID: 53
	[Il2CppEagerStaticClassConstruction]
	[Serializable]
	public struct int3x4 : IEquatable<int3x4>, IFormattable
	{
		// Token: 0x06001B21 RID: 6945 RVA: 0x0004AAA4 File Offset: 0x00048CA4
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int3x4(int3 c0, int3 c1, int3 c2, int3 c3)
		{
			this.c0 = c0;
			this.c1 = c1;
			this.c2 = c2;
			this.c3 = c3;
		}

		// Token: 0x06001B22 RID: 6946 RVA: 0x0004AAC4 File Offset: 0x00048CC4
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int3x4(int m00, int m01, int m02, int m03, int m10, int m11, int m12, int m13, int m20, int m21, int m22, int m23)
		{
			this.c0 = new int3(m00, m10, m20);
			this.c1 = new int3(m01, m11, m21);
			this.c2 = new int3(m02, m12, m22);
			this.c3 = new int3(m03, m13, m23);
		}

		// Token: 0x06001B23 RID: 6947 RVA: 0x0004AB12 File Offset: 0x00048D12
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int3x4(int v)
		{
			this.c0 = v;
			this.c1 = v;
			this.c2 = v;
			this.c3 = v;
		}

		// Token: 0x06001B24 RID: 6948 RVA: 0x0004AB44 File Offset: 0x00048D44
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int3x4(bool v)
		{
			this.c0 = math.select(new int3(0), new int3(1), v);
			this.c1 = math.select(new int3(0), new int3(1), v);
			this.c2 = math.select(new int3(0), new int3(1), v);
			this.c3 = math.select(new int3(0), new int3(1), v);
		}

		// Token: 0x06001B25 RID: 6949 RVA: 0x0004ABB4 File Offset: 0x00048DB4
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int3x4(bool3x4 v)
		{
			this.c0 = math.select(new int3(0), new int3(1), v.c0);
			this.c1 = math.select(new int3(0), new int3(1), v.c1);
			this.c2 = math.select(new int3(0), new int3(1), v.c2);
			this.c3 = math.select(new int3(0), new int3(1), v.c3);
		}

		// Token: 0x06001B26 RID: 6950 RVA: 0x0004AC35 File Offset: 0x00048E35
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int3x4(uint v)
		{
			this.c0 = (int3)v;
			this.c1 = (int3)v;
			this.c2 = (int3)v;
			this.c3 = (int3)v;
		}

		// Token: 0x06001B27 RID: 6951 RVA: 0x0004AC68 File Offset: 0x00048E68
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int3x4(uint3x4 v)
		{
			this.c0 = (int3)v.c0;
			this.c1 = (int3)v.c1;
			this.c2 = (int3)v.c2;
			this.c3 = (int3)v.c3;
		}

		// Token: 0x06001B28 RID: 6952 RVA: 0x0004ACB9 File Offset: 0x00048EB9
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int3x4(float v)
		{
			this.c0 = (int3)v;
			this.c1 = (int3)v;
			this.c2 = (int3)v;
			this.c3 = (int3)v;
		}

		// Token: 0x06001B29 RID: 6953 RVA: 0x0004ACEC File Offset: 0x00048EEC
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int3x4(float3x4 v)
		{
			this.c0 = (int3)v.c0;
			this.c1 = (int3)v.c1;
			this.c2 = (int3)v.c2;
			this.c3 = (int3)v.c3;
		}

		// Token: 0x06001B2A RID: 6954 RVA: 0x0004AD3D File Offset: 0x00048F3D
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int3x4(double v)
		{
			this.c0 = (int3)v;
			this.c1 = (int3)v;
			this.c2 = (int3)v;
			this.c3 = (int3)v;
		}

		// Token: 0x06001B2B RID: 6955 RVA: 0x0004AD70 File Offset: 0x00048F70
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int3x4(double3x4 v)
		{
			this.c0 = (int3)v.c0;
			this.c1 = (int3)v.c1;
			this.c2 = (int3)v.c2;
			this.c3 = (int3)v.c3;
		}

		// Token: 0x06001B2C RID: 6956 RVA: 0x0004ADC1 File Offset: 0x00048FC1
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static implicit operator int3x4(int v)
		{
			return new int3x4(v);
		}

		// Token: 0x06001B2D RID: 6957 RVA: 0x0004ADC9 File Offset: 0x00048FC9
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static explicit operator int3x4(bool v)
		{
			return new int3x4(v);
		}

		// Token: 0x06001B2E RID: 6958 RVA: 0x0004ADD1 File Offset: 0x00048FD1
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static explicit operator int3x4(bool3x4 v)
		{
			return new int3x4(v);
		}

		// Token: 0x06001B2F RID: 6959 RVA: 0x0004ADD9 File Offset: 0x00048FD9
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static explicit operator int3x4(uint v)
		{
			return new int3x4(v);
		}

		// Token: 0x06001B30 RID: 6960 RVA: 0x0004ADE1 File Offset: 0x00048FE1
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static explicit operator int3x4(uint3x4 v)
		{
			return new int3x4(v);
		}

		// Token: 0x06001B31 RID: 6961 RVA: 0x0004ADE9 File Offset: 0x00048FE9
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static explicit operator int3x4(float v)
		{
			return new int3x4(v);
		}

		// Token: 0x06001B32 RID: 6962 RVA: 0x0004ADF1 File Offset: 0x00048FF1
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static explicit operator int3x4(float3x4 v)
		{
			return new int3x4(v);
		}

		// Token: 0x06001B33 RID: 6963 RVA: 0x0004ADF9 File Offset: 0x00048FF9
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static explicit operator int3x4(double v)
		{
			return new int3x4(v);
		}

		// Token: 0x06001B34 RID: 6964 RVA: 0x0004AE01 File Offset: 0x00049001
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static explicit operator int3x4(double3x4 v)
		{
			return new int3x4(v);
		}

		// Token: 0x06001B35 RID: 6965 RVA: 0x0004AE0C File Offset: 0x0004900C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int3x4 operator *(int3x4 lhs, int3x4 rhs)
		{
			return new int3x4(lhs.c0 * rhs.c0, lhs.c1 * rhs.c1, lhs.c2 * rhs.c2, lhs.c3 * rhs.c3);
		}

		// Token: 0x06001B36 RID: 6966 RVA: 0x0004AE62 File Offset: 0x00049062
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int3x4 operator *(int3x4 lhs, int rhs)
		{
			return new int3x4(lhs.c0 * rhs, lhs.c1 * rhs, lhs.c2 * rhs, lhs.c3 * rhs);
		}

		// Token: 0x06001B37 RID: 6967 RVA: 0x0004AE99 File Offset: 0x00049099
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int3x4 operator *(int lhs, int3x4 rhs)
		{
			return new int3x4(lhs * rhs.c0, lhs * rhs.c1, lhs * rhs.c2, lhs * rhs.c3);
		}

		// Token: 0x06001B38 RID: 6968 RVA: 0x0004AED0 File Offset: 0x000490D0
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int3x4 operator +(int3x4 lhs, int3x4 rhs)
		{
			return new int3x4(lhs.c0 + rhs.c0, lhs.c1 + rhs.c1, lhs.c2 + rhs.c2, lhs.c3 + rhs.c3);
		}

		// Token: 0x06001B39 RID: 6969 RVA: 0x0004AF26 File Offset: 0x00049126
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int3x4 operator +(int3x4 lhs, int rhs)
		{
			return new int3x4(lhs.c0 + rhs, lhs.c1 + rhs, lhs.c2 + rhs, lhs.c3 + rhs);
		}

		// Token: 0x06001B3A RID: 6970 RVA: 0x0004AF5D File Offset: 0x0004915D
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int3x4 operator +(int lhs, int3x4 rhs)
		{
			return new int3x4(lhs + rhs.c0, lhs + rhs.c1, lhs + rhs.c2, lhs + rhs.c3);
		}

		// Token: 0x06001B3B RID: 6971 RVA: 0x0004AF94 File Offset: 0x00049194
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int3x4 operator -(int3x4 lhs, int3x4 rhs)
		{
			return new int3x4(lhs.c0 - rhs.c0, lhs.c1 - rhs.c1, lhs.c2 - rhs.c2, lhs.c3 - rhs.c3);
		}

		// Token: 0x06001B3C RID: 6972 RVA: 0x0004AFEA File Offset: 0x000491EA
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int3x4 operator -(int3x4 lhs, int rhs)
		{
			return new int3x4(lhs.c0 - rhs, lhs.c1 - rhs, lhs.c2 - rhs, lhs.c3 - rhs);
		}

		// Token: 0x06001B3D RID: 6973 RVA: 0x0004B021 File Offset: 0x00049221
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int3x4 operator -(int lhs, int3x4 rhs)
		{
			return new int3x4(lhs - rhs.c0, lhs - rhs.c1, lhs - rhs.c2, lhs - rhs.c3);
		}

		// Token: 0x06001B3E RID: 6974 RVA: 0x0004B058 File Offset: 0x00049258
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int3x4 operator /(int3x4 lhs, int3x4 rhs)
		{
			return new int3x4(lhs.c0 / rhs.c0, lhs.c1 / rhs.c1, lhs.c2 / rhs.c2, lhs.c3 / rhs.c3);
		}

		// Token: 0x06001B3F RID: 6975 RVA: 0x0004B0AE File Offset: 0x000492AE
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int3x4 operator /(int3x4 lhs, int rhs)
		{
			return new int3x4(lhs.c0 / rhs, lhs.c1 / rhs, lhs.c2 / rhs, lhs.c3 / rhs);
		}

		// Token: 0x06001B40 RID: 6976 RVA: 0x0004B0E5 File Offset: 0x000492E5
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int3x4 operator /(int lhs, int3x4 rhs)
		{
			return new int3x4(lhs / rhs.c0, lhs / rhs.c1, lhs / rhs.c2, lhs / rhs.c3);
		}

		// Token: 0x06001B41 RID: 6977 RVA: 0x0004B11C File Offset: 0x0004931C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int3x4 operator %(int3x4 lhs, int3x4 rhs)
		{
			return new int3x4(lhs.c0 % rhs.c0, lhs.c1 % rhs.c1, lhs.c2 % rhs.c2, lhs.c3 % rhs.c3);
		}

		// Token: 0x06001B42 RID: 6978 RVA: 0x0004B172 File Offset: 0x00049372
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int3x4 operator %(int3x4 lhs, int rhs)
		{
			return new int3x4(lhs.c0 % rhs, lhs.c1 % rhs, lhs.c2 % rhs, lhs.c3 % rhs);
		}

		// Token: 0x06001B43 RID: 6979 RVA: 0x0004B1A9 File Offset: 0x000493A9
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int3x4 operator %(int lhs, int3x4 rhs)
		{
			return new int3x4(lhs % rhs.c0, lhs % rhs.c1, lhs % rhs.c2, lhs % rhs.c3);
		}

		// Token: 0x06001B44 RID: 6980 RVA: 0x0004B1E0 File Offset: 0x000493E0
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int3x4 operator ++(int3x4 val)
		{
			int3 @int = int3.op_Increment(val.c0);
			val.c0 = @int;
			int3 int2 = @int;
			@int = int3.op_Increment(val.c1);
			val.c1 = @int;
			int3 int3 = @int;
			@int = int3.op_Increment(val.c2);
			val.c2 = @int;
			int3 int4 = @int;
			@int = int3.op_Increment(val.c3);
			val.c3 = @int;
			return new int3x4(int2, int3, int4, @int);
		}

		// Token: 0x06001B45 RID: 6981 RVA: 0x0004B25C File Offset: 0x0004945C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int3x4 operator --(int3x4 val)
		{
			int3 @int = int3.op_Decrement(val.c0);
			val.c0 = @int;
			int3 int2 = @int;
			@int = int3.op_Decrement(val.c1);
			val.c1 = @int;
			int3 int3 = @int;
			@int = int3.op_Decrement(val.c2);
			val.c2 = @int;
			int3 int4 = @int;
			@int = int3.op_Decrement(val.c3);
			val.c3 = @int;
			return new int3x4(int2, int3, int4, @int);
		}

		// Token: 0x06001B46 RID: 6982 RVA: 0x0004B2D8 File Offset: 0x000494D8
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool3x4 operator <(int3x4 lhs, int3x4 rhs)
		{
			return new bool3x4(lhs.c0 < rhs.c0, lhs.c1 < rhs.c1, lhs.c2 < rhs.c2, lhs.c3 < rhs.c3);
		}

		// Token: 0x06001B47 RID: 6983 RVA: 0x0004B32E File Offset: 0x0004952E
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool3x4 operator <(int3x4 lhs, int rhs)
		{
			return new bool3x4(lhs.c0 < rhs, lhs.c1 < rhs, lhs.c2 < rhs, lhs.c3 < rhs);
		}

		// Token: 0x06001B48 RID: 6984 RVA: 0x0004B365 File Offset: 0x00049565
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool3x4 operator <(int lhs, int3x4 rhs)
		{
			return new bool3x4(lhs < rhs.c0, lhs < rhs.c1, lhs < rhs.c2, lhs < rhs.c3);
		}

		// Token: 0x06001B49 RID: 6985 RVA: 0x0004B39C File Offset: 0x0004959C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool3x4 operator <=(int3x4 lhs, int3x4 rhs)
		{
			return new bool3x4(lhs.c0 <= rhs.c0, lhs.c1 <= rhs.c1, lhs.c2 <= rhs.c2, lhs.c3 <= rhs.c3);
		}

		// Token: 0x06001B4A RID: 6986 RVA: 0x0004B3F2 File Offset: 0x000495F2
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool3x4 operator <=(int3x4 lhs, int rhs)
		{
			return new bool3x4(lhs.c0 <= rhs, lhs.c1 <= rhs, lhs.c2 <= rhs, lhs.c3 <= rhs);
		}

		// Token: 0x06001B4B RID: 6987 RVA: 0x0004B429 File Offset: 0x00049629
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool3x4 operator <=(int lhs, int3x4 rhs)
		{
			return new bool3x4(lhs <= rhs.c0, lhs <= rhs.c1, lhs <= rhs.c2, lhs <= rhs.c3);
		}

		// Token: 0x06001B4C RID: 6988 RVA: 0x0004B460 File Offset: 0x00049660
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool3x4 operator >(int3x4 lhs, int3x4 rhs)
		{
			return new bool3x4(lhs.c0 > rhs.c0, lhs.c1 > rhs.c1, lhs.c2 > rhs.c2, lhs.c3 > rhs.c3);
		}

		// Token: 0x06001B4D RID: 6989 RVA: 0x0004B4B6 File Offset: 0x000496B6
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool3x4 operator >(int3x4 lhs, int rhs)
		{
			return new bool3x4(lhs.c0 > rhs, lhs.c1 > rhs, lhs.c2 > rhs, lhs.c3 > rhs);
		}

		// Token: 0x06001B4E RID: 6990 RVA: 0x0004B4ED File Offset: 0x000496ED
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool3x4 operator >(int lhs, int3x4 rhs)
		{
			return new bool3x4(lhs > rhs.c0, lhs > rhs.c1, lhs > rhs.c2, lhs > rhs.c3);
		}

		// Token: 0x06001B4F RID: 6991 RVA: 0x0004B524 File Offset: 0x00049724
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool3x4 operator >=(int3x4 lhs, int3x4 rhs)
		{
			return new bool3x4(lhs.c0 >= rhs.c0, lhs.c1 >= rhs.c1, lhs.c2 >= rhs.c2, lhs.c3 >= rhs.c3);
		}

		// Token: 0x06001B50 RID: 6992 RVA: 0x0004B57A File Offset: 0x0004977A
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool3x4 operator >=(int3x4 lhs, int rhs)
		{
			return new bool3x4(lhs.c0 >= rhs, lhs.c1 >= rhs, lhs.c2 >= rhs, lhs.c3 >= rhs);
		}

		// Token: 0x06001B51 RID: 6993 RVA: 0x0004B5B1 File Offset: 0x000497B1
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool3x4 operator >=(int lhs, int3x4 rhs)
		{
			return new bool3x4(lhs >= rhs.c0, lhs >= rhs.c1, lhs >= rhs.c2, lhs >= rhs.c3);
		}

		// Token: 0x06001B52 RID: 6994 RVA: 0x0004B5E8 File Offset: 0x000497E8
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int3x4 operator -(int3x4 val)
		{
			return new int3x4(-val.c0, -val.c1, -val.c2, -val.c3);
		}

		// Token: 0x06001B53 RID: 6995 RVA: 0x0004B61B File Offset: 0x0004981B
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int3x4 operator +(int3x4 val)
		{
			return new int3x4(+val.c0, +val.c1, +val.c2, +val.c3);
		}

		// Token: 0x06001B54 RID: 6996 RVA: 0x0004B64E File Offset: 0x0004984E
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int3x4 operator <<(int3x4 x, int n)
		{
			return new int3x4(x.c0 << n, x.c1 << n, x.c2 << n, x.c3 << n);
		}

		// Token: 0x06001B55 RID: 6997 RVA: 0x0004B685 File Offset: 0x00049885
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int3x4 operator >>(int3x4 x, int n)
		{
			return new int3x4(x.c0 >> n, x.c1 >> n, x.c2 >> n, x.c3 >> n);
		}

		// Token: 0x06001B56 RID: 6998 RVA: 0x0004B6BC File Offset: 0x000498BC
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool3x4 operator ==(int3x4 lhs, int3x4 rhs)
		{
			return new bool3x4(lhs.c0 == rhs.c0, lhs.c1 == rhs.c1, lhs.c2 == rhs.c2, lhs.c3 == rhs.c3);
		}

		// Token: 0x06001B57 RID: 6999 RVA: 0x0004B712 File Offset: 0x00049912
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool3x4 operator ==(int3x4 lhs, int rhs)
		{
			return new bool3x4(lhs.c0 == rhs, lhs.c1 == rhs, lhs.c2 == rhs, lhs.c3 == rhs);
		}

		// Token: 0x06001B58 RID: 7000 RVA: 0x0004B749 File Offset: 0x00049949
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool3x4 operator ==(int lhs, int3x4 rhs)
		{
			return new bool3x4(lhs == rhs.c0, lhs == rhs.c1, lhs == rhs.c2, lhs == rhs.c3);
		}

		// Token: 0x06001B59 RID: 7001 RVA: 0x0004B780 File Offset: 0x00049980
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool3x4 operator !=(int3x4 lhs, int3x4 rhs)
		{
			return new bool3x4(lhs.c0 != rhs.c0, lhs.c1 != rhs.c1, lhs.c2 != rhs.c2, lhs.c3 != rhs.c3);
		}

		// Token: 0x06001B5A RID: 7002 RVA: 0x0004B7D6 File Offset: 0x000499D6
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool3x4 operator !=(int3x4 lhs, int rhs)
		{
			return new bool3x4(lhs.c0 != rhs, lhs.c1 != rhs, lhs.c2 != rhs, lhs.c3 != rhs);
		}

		// Token: 0x06001B5B RID: 7003 RVA: 0x0004B80D File Offset: 0x00049A0D
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool3x4 operator !=(int lhs, int3x4 rhs)
		{
			return new bool3x4(lhs != rhs.c0, lhs != rhs.c1, lhs != rhs.c2, lhs != rhs.c3);
		}

		// Token: 0x06001B5C RID: 7004 RVA: 0x0004B844 File Offset: 0x00049A44
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int3x4 operator ~(int3x4 val)
		{
			return new int3x4(~val.c0, ~val.c1, ~val.c2, ~val.c3);
		}

		// Token: 0x06001B5D RID: 7005 RVA: 0x0004B878 File Offset: 0x00049A78
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int3x4 operator &(int3x4 lhs, int3x4 rhs)
		{
			return new int3x4(lhs.c0 & rhs.c0, lhs.c1 & rhs.c1, lhs.c2 & rhs.c2, lhs.c3 & rhs.c3);
		}

		// Token: 0x06001B5E RID: 7006 RVA: 0x0004B8CE File Offset: 0x00049ACE
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int3x4 operator &(int3x4 lhs, int rhs)
		{
			return new int3x4(lhs.c0 & rhs, lhs.c1 & rhs, lhs.c2 & rhs, lhs.c3 & rhs);
		}

		// Token: 0x06001B5F RID: 7007 RVA: 0x0004B905 File Offset: 0x00049B05
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int3x4 operator &(int lhs, int3x4 rhs)
		{
			return new int3x4(lhs & rhs.c0, lhs & rhs.c1, lhs & rhs.c2, lhs & rhs.c3);
		}

		// Token: 0x06001B60 RID: 7008 RVA: 0x0004B93C File Offset: 0x00049B3C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int3x4 operator |(int3x4 lhs, int3x4 rhs)
		{
			return new int3x4(lhs.c0 | rhs.c0, lhs.c1 | rhs.c1, lhs.c2 | rhs.c2, lhs.c3 | rhs.c3);
		}

		// Token: 0x06001B61 RID: 7009 RVA: 0x0004B992 File Offset: 0x00049B92
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int3x4 operator |(int3x4 lhs, int rhs)
		{
			return new int3x4(lhs.c0 | rhs, lhs.c1 | rhs, lhs.c2 | rhs, lhs.c3 | rhs);
		}

		// Token: 0x06001B62 RID: 7010 RVA: 0x0004B9C9 File Offset: 0x00049BC9
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int3x4 operator |(int lhs, int3x4 rhs)
		{
			return new int3x4(lhs | rhs.c0, lhs | rhs.c1, lhs | rhs.c2, lhs | rhs.c3);
		}

		// Token: 0x06001B63 RID: 7011 RVA: 0x0004BA00 File Offset: 0x00049C00
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int3x4 operator ^(int3x4 lhs, int3x4 rhs)
		{
			return new int3x4(lhs.c0 ^ rhs.c0, lhs.c1 ^ rhs.c1, lhs.c2 ^ rhs.c2, lhs.c3 ^ rhs.c3);
		}

		// Token: 0x06001B64 RID: 7012 RVA: 0x0004BA56 File Offset: 0x00049C56
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int3x4 operator ^(int3x4 lhs, int rhs)
		{
			return new int3x4(lhs.c0 ^ rhs, lhs.c1 ^ rhs, lhs.c2 ^ rhs, lhs.c3 ^ rhs);
		}

		// Token: 0x06001B65 RID: 7013 RVA: 0x0004BA8D File Offset: 0x00049C8D
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int3x4 operator ^(int lhs, int3x4 rhs)
		{
			return new int3x4(lhs ^ rhs.c0, lhs ^ rhs.c1, lhs ^ rhs.c2, lhs ^ rhs.c3);
		}

		// Token: 0x17000848 RID: 2120
		public unsafe ref int3 this[int index]
		{
			get
			{
				fixed (int3x4* ptr = &this)
				{
					return ref *(int3*)(ptr + (IntPtr)index * (IntPtr)sizeof(int3) / (IntPtr)sizeof(int3x4));
				}
			}
		}

		// Token: 0x06001B67 RID: 7015 RVA: 0x0004BAE0 File Offset: 0x00049CE0
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool Equals(int3x4 rhs)
		{
			return this.c0.Equals(rhs.c0) && this.c1.Equals(rhs.c1) && this.c2.Equals(rhs.c2) && this.c3.Equals(rhs.c3);
		}

		// Token: 0x06001B68 RID: 7016 RVA: 0x0004BB3C File Offset: 0x00049D3C
		public override bool Equals(object o)
		{
			if (o is int3x4)
			{
				int3x4 int3x = (int3x4)o;
				return this.Equals(int3x);
			}
			return false;
		}

		// Token: 0x06001B69 RID: 7017 RVA: 0x0004BB61 File Offset: 0x00049D61
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public override int GetHashCode()
		{
			return (int)math.hash(this);
		}

		// Token: 0x06001B6A RID: 7018 RVA: 0x0004BB70 File Offset: 0x00049D70
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public override string ToString()
		{
			return string.Format("int3x4({0}, {1}, {2}, {3},  {4}, {5}, {6}, {7},  {8}, {9}, {10}, {11})", new object[]
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

		// Token: 0x06001B6B RID: 7019 RVA: 0x0004BC78 File Offset: 0x00049E78
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public string ToString(string format, IFormatProvider formatProvider)
		{
			return string.Format("int3x4({0}, {1}, {2}, {3},  {4}, {5}, {6}, {7},  {8}, {9}, {10}, {11})", new object[]
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

		// Token: 0x040000D9 RID: 217
		public int3 c0;

		// Token: 0x040000DA RID: 218
		public int3 c1;

		// Token: 0x040000DB RID: 219
		public int3 c2;

		// Token: 0x040000DC RID: 220
		public int3 c3;

		// Token: 0x040000DD RID: 221
		public static readonly int3x4 zero;
	}
}
