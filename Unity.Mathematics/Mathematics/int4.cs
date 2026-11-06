using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Unity.IL2CPP.CompilerServices;

namespace Unity.Mathematics
{
	// Token: 0x02000036 RID: 54
	[DebuggerTypeProxy(typeof(int4.DebuggerProxy))]
	[Il2CppEagerStaticClassConstruction]
	[Serializable]
	public struct int4 : IEquatable<int4>, IFormattable
	{
		// Token: 0x06001B6C RID: 7020 RVA: 0x0004BD95 File Offset: 0x00049F95
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int4(int x, int y, int z, int w)
		{
			this.x = x;
			this.y = y;
			this.z = z;
			this.w = w;
		}

		// Token: 0x06001B6D RID: 7021 RVA: 0x0004BDB4 File Offset: 0x00049FB4
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int4(int x, int y, int2 zw)
		{
			this.x = x;
			this.y = y;
			this.z = zw.x;
			this.w = zw.y;
		}

		// Token: 0x06001B6E RID: 7022 RVA: 0x0004BDDC File Offset: 0x00049FDC
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int4(int x, int2 yz, int w)
		{
			this.x = x;
			this.y = yz.x;
			this.z = yz.y;
			this.w = w;
		}

		// Token: 0x06001B6F RID: 7023 RVA: 0x0004BE04 File Offset: 0x0004A004
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int4(int x, int3 yzw)
		{
			this.x = x;
			this.y = yzw.x;
			this.z = yzw.y;
			this.w = yzw.z;
		}

		// Token: 0x06001B70 RID: 7024 RVA: 0x0004BE31 File Offset: 0x0004A031
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int4(int2 xy, int z, int w)
		{
			this.x = xy.x;
			this.y = xy.y;
			this.z = z;
			this.w = w;
		}

		// Token: 0x06001B71 RID: 7025 RVA: 0x0004BE59 File Offset: 0x0004A059
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int4(int2 xy, int2 zw)
		{
			this.x = xy.x;
			this.y = xy.y;
			this.z = zw.x;
			this.w = zw.y;
		}

		// Token: 0x06001B72 RID: 7026 RVA: 0x0004BE8B File Offset: 0x0004A08B
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int4(int3 xyz, int w)
		{
			this.x = xyz.x;
			this.y = xyz.y;
			this.z = xyz.z;
			this.w = w;
		}

		// Token: 0x06001B73 RID: 7027 RVA: 0x0004BEB8 File Offset: 0x0004A0B8
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int4(int4 xyzw)
		{
			this.x = xyzw.x;
			this.y = xyzw.y;
			this.z = xyzw.z;
			this.w = xyzw.w;
		}

		// Token: 0x06001B74 RID: 7028 RVA: 0x0004BEEA File Offset: 0x0004A0EA
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int4(int v)
		{
			this.x = v;
			this.y = v;
			this.z = v;
			this.w = v;
		}

		// Token: 0x06001B75 RID: 7029 RVA: 0x0004BF08 File Offset: 0x0004A108
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int4(bool v)
		{
			this.x = (v ? 1 : 0);
			this.y = (v ? 1 : 0);
			this.z = (v ? 1 : 0);
			this.w = (v ? 1 : 0);
		}

		// Token: 0x06001B76 RID: 7030 RVA: 0x0004BF40 File Offset: 0x0004A140
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int4(bool4 v)
		{
			this.x = (v.x ? 1 : 0);
			this.y = (v.y ? 1 : 0);
			this.z = (v.z ? 1 : 0);
			this.w = (v.w ? 1 : 0);
		}

		// Token: 0x06001B77 RID: 7031 RVA: 0x0004BF95 File Offset: 0x0004A195
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int4(uint v)
		{
			this.x = (int)v;
			this.y = (int)v;
			this.z = (int)v;
			this.w = (int)v;
		}

		// Token: 0x06001B78 RID: 7032 RVA: 0x0004BFB3 File Offset: 0x0004A1B3
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int4(uint4 v)
		{
			this.x = (int)v.x;
			this.y = (int)v.y;
			this.z = (int)v.z;
			this.w = (int)v.w;
		}

		// Token: 0x06001B79 RID: 7033 RVA: 0x0004BFE5 File Offset: 0x0004A1E5
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int4(float v)
		{
			this.x = (int)v;
			this.y = (int)v;
			this.z = (int)v;
			this.w = (int)v;
		}

		// Token: 0x06001B7A RID: 7034 RVA: 0x0004C007 File Offset: 0x0004A207
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int4(float4 v)
		{
			this.x = (int)v.x;
			this.y = (int)v.y;
			this.z = (int)v.z;
			this.w = (int)v.w;
		}

		// Token: 0x06001B7B RID: 7035 RVA: 0x0004C03D File Offset: 0x0004A23D
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int4(double v)
		{
			this.x = (int)v;
			this.y = (int)v;
			this.z = (int)v;
			this.w = (int)v;
		}

		// Token: 0x06001B7C RID: 7036 RVA: 0x0004C05F File Offset: 0x0004A25F
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int4(double4 v)
		{
			this.x = (int)v.x;
			this.y = (int)v.y;
			this.z = (int)v.z;
			this.w = (int)v.w;
		}

		// Token: 0x06001B7D RID: 7037 RVA: 0x0004C095 File Offset: 0x0004A295
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static implicit operator int4(int v)
		{
			return new int4(v);
		}

		// Token: 0x06001B7E RID: 7038 RVA: 0x0004C09D File Offset: 0x0004A29D
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static explicit operator int4(bool v)
		{
			return new int4(v);
		}

		// Token: 0x06001B7F RID: 7039 RVA: 0x0004C0A5 File Offset: 0x0004A2A5
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static explicit operator int4(bool4 v)
		{
			return new int4(v);
		}

		// Token: 0x06001B80 RID: 7040 RVA: 0x0004C0AD File Offset: 0x0004A2AD
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static explicit operator int4(uint v)
		{
			return new int4(v);
		}

		// Token: 0x06001B81 RID: 7041 RVA: 0x0004C0B5 File Offset: 0x0004A2B5
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static explicit operator int4(uint4 v)
		{
			return new int4(v);
		}

		// Token: 0x06001B82 RID: 7042 RVA: 0x0004C0BD File Offset: 0x0004A2BD
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static explicit operator int4(float v)
		{
			return new int4(v);
		}

		// Token: 0x06001B83 RID: 7043 RVA: 0x0004C0C5 File Offset: 0x0004A2C5
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static explicit operator int4(float4 v)
		{
			return new int4(v);
		}

		// Token: 0x06001B84 RID: 7044 RVA: 0x0004C0CD File Offset: 0x0004A2CD
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static explicit operator int4(double v)
		{
			return new int4(v);
		}

		// Token: 0x06001B85 RID: 7045 RVA: 0x0004C0D5 File Offset: 0x0004A2D5
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static explicit operator int4(double4 v)
		{
			return new int4(v);
		}

		// Token: 0x06001B86 RID: 7046 RVA: 0x0004C0DD File Offset: 0x0004A2DD
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int4 operator *(int4 lhs, int4 rhs)
		{
			return new int4(lhs.x * rhs.x, lhs.y * rhs.y, lhs.z * rhs.z, lhs.w * rhs.w);
		}

		// Token: 0x06001B87 RID: 7047 RVA: 0x0004C118 File Offset: 0x0004A318
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int4 operator *(int4 lhs, int rhs)
		{
			return new int4(lhs.x * rhs, lhs.y * rhs, lhs.z * rhs, lhs.w * rhs);
		}

		// Token: 0x06001B88 RID: 7048 RVA: 0x0004C13F File Offset: 0x0004A33F
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int4 operator *(int lhs, int4 rhs)
		{
			return new int4(lhs * rhs.x, lhs * rhs.y, lhs * rhs.z, lhs * rhs.w);
		}

		// Token: 0x06001B89 RID: 7049 RVA: 0x0004C166 File Offset: 0x0004A366
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int4 operator +(int4 lhs, int4 rhs)
		{
			return new int4(lhs.x + rhs.x, lhs.y + rhs.y, lhs.z + rhs.z, lhs.w + rhs.w);
		}

		// Token: 0x06001B8A RID: 7050 RVA: 0x0004C1A1 File Offset: 0x0004A3A1
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int4 operator +(int4 lhs, int rhs)
		{
			return new int4(lhs.x + rhs, lhs.y + rhs, lhs.z + rhs, lhs.w + rhs);
		}

		// Token: 0x06001B8B RID: 7051 RVA: 0x0004C1C8 File Offset: 0x0004A3C8
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int4 operator +(int lhs, int4 rhs)
		{
			return new int4(lhs + rhs.x, lhs + rhs.y, lhs + rhs.z, lhs + rhs.w);
		}

		// Token: 0x06001B8C RID: 7052 RVA: 0x0004C1EF File Offset: 0x0004A3EF
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int4 operator -(int4 lhs, int4 rhs)
		{
			return new int4(lhs.x - rhs.x, lhs.y - rhs.y, lhs.z - rhs.z, lhs.w - rhs.w);
		}

		// Token: 0x06001B8D RID: 7053 RVA: 0x0004C22A File Offset: 0x0004A42A
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int4 operator -(int4 lhs, int rhs)
		{
			return new int4(lhs.x - rhs, lhs.y - rhs, lhs.z - rhs, lhs.w - rhs);
		}

		// Token: 0x06001B8E RID: 7054 RVA: 0x0004C251 File Offset: 0x0004A451
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int4 operator -(int lhs, int4 rhs)
		{
			return new int4(lhs - rhs.x, lhs - rhs.y, lhs - rhs.z, lhs - rhs.w);
		}

		// Token: 0x06001B8F RID: 7055 RVA: 0x0004C278 File Offset: 0x0004A478
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int4 operator /(int4 lhs, int4 rhs)
		{
			return new int4(lhs.x / rhs.x, lhs.y / rhs.y, lhs.z / rhs.z, lhs.w / rhs.w);
		}

		// Token: 0x06001B90 RID: 7056 RVA: 0x0004C2B3 File Offset: 0x0004A4B3
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int4 operator /(int4 lhs, int rhs)
		{
			return new int4(lhs.x / rhs, lhs.y / rhs, lhs.z / rhs, lhs.w / rhs);
		}

		// Token: 0x06001B91 RID: 7057 RVA: 0x0004C2DA File Offset: 0x0004A4DA
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int4 operator /(int lhs, int4 rhs)
		{
			return new int4(lhs / rhs.x, lhs / rhs.y, lhs / rhs.z, lhs / rhs.w);
		}

		// Token: 0x06001B92 RID: 7058 RVA: 0x0004C301 File Offset: 0x0004A501
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int4 operator %(int4 lhs, int4 rhs)
		{
			return new int4(lhs.x % rhs.x, lhs.y % rhs.y, lhs.z % rhs.z, lhs.w % rhs.w);
		}

		// Token: 0x06001B93 RID: 7059 RVA: 0x0004C33C File Offset: 0x0004A53C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int4 operator %(int4 lhs, int rhs)
		{
			return new int4(lhs.x % rhs, lhs.y % rhs, lhs.z % rhs, lhs.w % rhs);
		}

		// Token: 0x06001B94 RID: 7060 RVA: 0x0004C363 File Offset: 0x0004A563
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int4 operator %(int lhs, int4 rhs)
		{
			return new int4(lhs % rhs.x, lhs % rhs.y, lhs % rhs.z, lhs % rhs.w);
		}

		// Token: 0x06001B95 RID: 7061 RVA: 0x0004C38C File Offset: 0x0004A58C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int4 operator ++(int4 val)
		{
			int num = val.x + 1;
			val.x = num;
			int num2 = num;
			num = val.y + 1;
			val.y = num;
			int num3 = num;
			num = val.z + 1;
			val.z = num;
			int num4 = num;
			num = val.w + 1;
			val.w = num;
			return new int4(num2, num3, num4, num);
		}

		// Token: 0x06001B96 RID: 7062 RVA: 0x0004C3DC File Offset: 0x0004A5DC
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int4 operator --(int4 val)
		{
			int num = val.x - 1;
			val.x = num;
			int num2 = num;
			num = val.y - 1;
			val.y = num;
			int num3 = num;
			num = val.z - 1;
			val.z = num;
			int num4 = num;
			num = val.w - 1;
			val.w = num;
			return new int4(num2, num3, num4, num);
		}

		// Token: 0x06001B97 RID: 7063 RVA: 0x0004C42A File Offset: 0x0004A62A
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool4 operator <(int4 lhs, int4 rhs)
		{
			return new bool4(lhs.x < rhs.x, lhs.y < rhs.y, lhs.z < rhs.z, lhs.w < rhs.w);
		}

		// Token: 0x06001B98 RID: 7064 RVA: 0x0004C469 File Offset: 0x0004A669
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool4 operator <(int4 lhs, int rhs)
		{
			return new bool4(lhs.x < rhs, lhs.y < rhs, lhs.z < rhs, lhs.w < rhs);
		}

		// Token: 0x06001B99 RID: 7065 RVA: 0x0004C494 File Offset: 0x0004A694
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool4 operator <(int lhs, int4 rhs)
		{
			return new bool4(lhs < rhs.x, lhs < rhs.y, lhs < rhs.z, lhs < rhs.w);
		}

		// Token: 0x06001B9A RID: 7066 RVA: 0x0004C4C0 File Offset: 0x0004A6C0
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool4 operator <=(int4 lhs, int4 rhs)
		{
			return new bool4(lhs.x <= rhs.x, lhs.y <= rhs.y, lhs.z <= rhs.z, lhs.w <= rhs.w);
		}

		// Token: 0x06001B9B RID: 7067 RVA: 0x0004C516 File Offset: 0x0004A716
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool4 operator <=(int4 lhs, int rhs)
		{
			return new bool4(lhs.x <= rhs, lhs.y <= rhs, lhs.z <= rhs, lhs.w <= rhs);
		}

		// Token: 0x06001B9C RID: 7068 RVA: 0x0004C54D File Offset: 0x0004A74D
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool4 operator <=(int lhs, int4 rhs)
		{
			return new bool4(lhs <= rhs.x, lhs <= rhs.y, lhs <= rhs.z, lhs <= rhs.w);
		}

		// Token: 0x06001B9D RID: 7069 RVA: 0x0004C584 File Offset: 0x0004A784
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool4 operator >(int4 lhs, int4 rhs)
		{
			return new bool4(lhs.x > rhs.x, lhs.y > rhs.y, lhs.z > rhs.z, lhs.w > rhs.w);
		}

		// Token: 0x06001B9E RID: 7070 RVA: 0x0004C5C3 File Offset: 0x0004A7C3
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool4 operator >(int4 lhs, int rhs)
		{
			return new bool4(lhs.x > rhs, lhs.y > rhs, lhs.z > rhs, lhs.w > rhs);
		}

		// Token: 0x06001B9F RID: 7071 RVA: 0x0004C5EE File Offset: 0x0004A7EE
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool4 operator >(int lhs, int4 rhs)
		{
			return new bool4(lhs > rhs.x, lhs > rhs.y, lhs > rhs.z, lhs > rhs.w);
		}

		// Token: 0x06001BA0 RID: 7072 RVA: 0x0004C61C File Offset: 0x0004A81C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool4 operator >=(int4 lhs, int4 rhs)
		{
			return new bool4(lhs.x >= rhs.x, lhs.y >= rhs.y, lhs.z >= rhs.z, lhs.w >= rhs.w);
		}

		// Token: 0x06001BA1 RID: 7073 RVA: 0x0004C672 File Offset: 0x0004A872
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool4 operator >=(int4 lhs, int rhs)
		{
			return new bool4(lhs.x >= rhs, lhs.y >= rhs, lhs.z >= rhs, lhs.w >= rhs);
		}

		// Token: 0x06001BA2 RID: 7074 RVA: 0x0004C6A9 File Offset: 0x0004A8A9
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool4 operator >=(int lhs, int4 rhs)
		{
			return new bool4(lhs >= rhs.x, lhs >= rhs.y, lhs >= rhs.z, lhs >= rhs.w);
		}

		// Token: 0x06001BA3 RID: 7075 RVA: 0x0004C6E0 File Offset: 0x0004A8E0
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int4 operator -(int4 val)
		{
			return new int4(-val.x, -val.y, -val.z, -val.w);
		}

		// Token: 0x06001BA4 RID: 7076 RVA: 0x0004C703 File Offset: 0x0004A903
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int4 operator +(int4 val)
		{
			return new int4(val.x, val.y, val.z, val.w);
		}

		// Token: 0x06001BA5 RID: 7077 RVA: 0x0004C722 File Offset: 0x0004A922
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int4 operator <<(int4 x, int n)
		{
			return new int4(x.x << n, x.y << n, x.z << n, x.w << n);
		}

		// Token: 0x06001BA6 RID: 7078 RVA: 0x0004C755 File Offset: 0x0004A955
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int4 operator >>(int4 x, int n)
		{
			return new int4(x.x >> n, x.y >> n, x.z >> n, x.w >> n);
		}

		// Token: 0x06001BA7 RID: 7079 RVA: 0x0004C788 File Offset: 0x0004A988
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool4 operator ==(int4 lhs, int4 rhs)
		{
			return new bool4(lhs.x == rhs.x, lhs.y == rhs.y, lhs.z == rhs.z, lhs.w == rhs.w);
		}

		// Token: 0x06001BA8 RID: 7080 RVA: 0x0004C7C7 File Offset: 0x0004A9C7
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool4 operator ==(int4 lhs, int rhs)
		{
			return new bool4(lhs.x == rhs, lhs.y == rhs, lhs.z == rhs, lhs.w == rhs);
		}

		// Token: 0x06001BA9 RID: 7081 RVA: 0x0004C7F2 File Offset: 0x0004A9F2
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool4 operator ==(int lhs, int4 rhs)
		{
			return new bool4(lhs == rhs.x, lhs == rhs.y, lhs == rhs.z, lhs == rhs.w);
		}

		// Token: 0x06001BAA RID: 7082 RVA: 0x0004C820 File Offset: 0x0004AA20
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool4 operator !=(int4 lhs, int4 rhs)
		{
			return new bool4(lhs.x != rhs.x, lhs.y != rhs.y, lhs.z != rhs.z, lhs.w != rhs.w);
		}

		// Token: 0x06001BAB RID: 7083 RVA: 0x0004C876 File Offset: 0x0004AA76
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool4 operator !=(int4 lhs, int rhs)
		{
			return new bool4(lhs.x != rhs, lhs.y != rhs, lhs.z != rhs, lhs.w != rhs);
		}

		// Token: 0x06001BAC RID: 7084 RVA: 0x0004C8AD File Offset: 0x0004AAAD
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool4 operator !=(int lhs, int4 rhs)
		{
			return new bool4(lhs != rhs.x, lhs != rhs.y, lhs != rhs.z, lhs != rhs.w);
		}

		// Token: 0x06001BAD RID: 7085 RVA: 0x0004C8E4 File Offset: 0x0004AAE4
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int4 operator ~(int4 val)
		{
			return new int4(~val.x, ~val.y, ~val.z, ~val.w);
		}

		// Token: 0x06001BAE RID: 7086 RVA: 0x0004C907 File Offset: 0x0004AB07
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int4 operator &(int4 lhs, int4 rhs)
		{
			return new int4(lhs.x & rhs.x, lhs.y & rhs.y, lhs.z & rhs.z, lhs.w & rhs.w);
		}

		// Token: 0x06001BAF RID: 7087 RVA: 0x0004C942 File Offset: 0x0004AB42
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int4 operator &(int4 lhs, int rhs)
		{
			return new int4(lhs.x & rhs, lhs.y & rhs, lhs.z & rhs, lhs.w & rhs);
		}

		// Token: 0x06001BB0 RID: 7088 RVA: 0x0004C969 File Offset: 0x0004AB69
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int4 operator &(int lhs, int4 rhs)
		{
			return new int4(lhs & rhs.x, lhs & rhs.y, lhs & rhs.z, lhs & rhs.w);
		}

		// Token: 0x06001BB1 RID: 7089 RVA: 0x0004C990 File Offset: 0x0004AB90
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int4 operator |(int4 lhs, int4 rhs)
		{
			return new int4(lhs.x | rhs.x, lhs.y | rhs.y, lhs.z | rhs.z, lhs.w | rhs.w);
		}

		// Token: 0x06001BB2 RID: 7090 RVA: 0x0004C9CB File Offset: 0x0004ABCB
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int4 operator |(int4 lhs, int rhs)
		{
			return new int4(lhs.x | rhs, lhs.y | rhs, lhs.z | rhs, lhs.w | rhs);
		}

		// Token: 0x06001BB3 RID: 7091 RVA: 0x0004C9F2 File Offset: 0x0004ABF2
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int4 operator |(int lhs, int4 rhs)
		{
			return new int4(lhs | rhs.x, lhs | rhs.y, lhs | rhs.z, lhs | rhs.w);
		}

		// Token: 0x06001BB4 RID: 7092 RVA: 0x0004CA19 File Offset: 0x0004AC19
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int4 operator ^(int4 lhs, int4 rhs)
		{
			return new int4(lhs.x ^ rhs.x, lhs.y ^ rhs.y, lhs.z ^ rhs.z, lhs.w ^ rhs.w);
		}

		// Token: 0x06001BB5 RID: 7093 RVA: 0x0004CA54 File Offset: 0x0004AC54
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int4 operator ^(int4 lhs, int rhs)
		{
			return new int4(lhs.x ^ rhs, lhs.y ^ rhs, lhs.z ^ rhs, lhs.w ^ rhs);
		}

		// Token: 0x06001BB6 RID: 7094 RVA: 0x0004CA7B File Offset: 0x0004AC7B
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int4 operator ^(int lhs, int4 rhs)
		{
			return new int4(lhs ^ rhs.x, lhs ^ rhs.y, lhs ^ rhs.z, lhs ^ rhs.w);
		}

		// Token: 0x17000849 RID: 2121
		// (get) Token: 0x06001BB7 RID: 7095 RVA: 0x0004CAA2 File Offset: 0x0004ACA2
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int4 xxxx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new int4(this.x, this.x, this.x, this.x);
			}
		}

		// Token: 0x1700084A RID: 2122
		// (get) Token: 0x06001BB8 RID: 7096 RVA: 0x0004CAC1 File Offset: 0x0004ACC1
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int4 xxxy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new int4(this.x, this.x, this.x, this.y);
			}
		}

		// Token: 0x1700084B RID: 2123
		// (get) Token: 0x06001BB9 RID: 7097 RVA: 0x0004CAE0 File Offset: 0x0004ACE0
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int4 xxxz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new int4(this.x, this.x, this.x, this.z);
			}
		}

		// Token: 0x1700084C RID: 2124
		// (get) Token: 0x06001BBA RID: 7098 RVA: 0x0004CAFF File Offset: 0x0004ACFF
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int4 xxxw
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new int4(this.x, this.x, this.x, this.w);
			}
		}

		// Token: 0x1700084D RID: 2125
		// (get) Token: 0x06001BBB RID: 7099 RVA: 0x0004CB1E File Offset: 0x0004AD1E
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int4 xxyx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new int4(this.x, this.x, this.y, this.x);
			}
		}

		// Token: 0x1700084E RID: 2126
		// (get) Token: 0x06001BBC RID: 7100 RVA: 0x0004CB3D File Offset: 0x0004AD3D
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int4 xxyy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new int4(this.x, this.x, this.y, this.y);
			}
		}

		// Token: 0x1700084F RID: 2127
		// (get) Token: 0x06001BBD RID: 7101 RVA: 0x0004CB5C File Offset: 0x0004AD5C
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int4 xxyz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new int4(this.x, this.x, this.y, this.z);
			}
		}

		// Token: 0x17000850 RID: 2128
		// (get) Token: 0x06001BBE RID: 7102 RVA: 0x0004CB7B File Offset: 0x0004AD7B
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int4 xxyw
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new int4(this.x, this.x, this.y, this.w);
			}
		}

		// Token: 0x17000851 RID: 2129
		// (get) Token: 0x06001BBF RID: 7103 RVA: 0x0004CB9A File Offset: 0x0004AD9A
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int4 xxzx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new int4(this.x, this.x, this.z, this.x);
			}
		}

		// Token: 0x17000852 RID: 2130
		// (get) Token: 0x06001BC0 RID: 7104 RVA: 0x0004CBB9 File Offset: 0x0004ADB9
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int4 xxzy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new int4(this.x, this.x, this.z, this.y);
			}
		}

		// Token: 0x17000853 RID: 2131
		// (get) Token: 0x06001BC1 RID: 7105 RVA: 0x0004CBD8 File Offset: 0x0004ADD8
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int4 xxzz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new int4(this.x, this.x, this.z, this.z);
			}
		}

		// Token: 0x17000854 RID: 2132
		// (get) Token: 0x06001BC2 RID: 7106 RVA: 0x0004CBF7 File Offset: 0x0004ADF7
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int4 xxzw
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new int4(this.x, this.x, this.z, this.w);
			}
		}

		// Token: 0x17000855 RID: 2133
		// (get) Token: 0x06001BC3 RID: 7107 RVA: 0x0004CC16 File Offset: 0x0004AE16
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int4 xxwx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new int4(this.x, this.x, this.w, this.x);
			}
		}

		// Token: 0x17000856 RID: 2134
		// (get) Token: 0x06001BC4 RID: 7108 RVA: 0x0004CC35 File Offset: 0x0004AE35
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int4 xxwy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new int4(this.x, this.x, this.w, this.y);
			}
		}

		// Token: 0x17000857 RID: 2135
		// (get) Token: 0x06001BC5 RID: 7109 RVA: 0x0004CC54 File Offset: 0x0004AE54
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int4 xxwz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new int4(this.x, this.x, this.w, this.z);
			}
		}

		// Token: 0x17000858 RID: 2136
		// (get) Token: 0x06001BC6 RID: 7110 RVA: 0x0004CC73 File Offset: 0x0004AE73
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int4 xxww
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new int4(this.x, this.x, this.w, this.w);
			}
		}

		// Token: 0x17000859 RID: 2137
		// (get) Token: 0x06001BC7 RID: 7111 RVA: 0x0004CC92 File Offset: 0x0004AE92
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int4 xyxx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new int4(this.x, this.y, this.x, this.x);
			}
		}

		// Token: 0x1700085A RID: 2138
		// (get) Token: 0x06001BC8 RID: 7112 RVA: 0x0004CCB1 File Offset: 0x0004AEB1
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int4 xyxy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new int4(this.x, this.y, this.x, this.y);
			}
		}

		// Token: 0x1700085B RID: 2139
		// (get) Token: 0x06001BC9 RID: 7113 RVA: 0x0004CCD0 File Offset: 0x0004AED0
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int4 xyxz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new int4(this.x, this.y, this.x, this.z);
			}
		}

		// Token: 0x1700085C RID: 2140
		// (get) Token: 0x06001BCA RID: 7114 RVA: 0x0004CCEF File Offset: 0x0004AEEF
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int4 xyxw
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new int4(this.x, this.y, this.x, this.w);
			}
		}

		// Token: 0x1700085D RID: 2141
		// (get) Token: 0x06001BCB RID: 7115 RVA: 0x0004CD0E File Offset: 0x0004AF0E
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int4 xyyx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new int4(this.x, this.y, this.y, this.x);
			}
		}

		// Token: 0x1700085E RID: 2142
		// (get) Token: 0x06001BCC RID: 7116 RVA: 0x0004CD2D File Offset: 0x0004AF2D
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int4 xyyy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new int4(this.x, this.y, this.y, this.y);
			}
		}

		// Token: 0x1700085F RID: 2143
		// (get) Token: 0x06001BCD RID: 7117 RVA: 0x0004CD4C File Offset: 0x0004AF4C
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int4 xyyz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new int4(this.x, this.y, this.y, this.z);
			}
		}

		// Token: 0x17000860 RID: 2144
		// (get) Token: 0x06001BCE RID: 7118 RVA: 0x0004CD6B File Offset: 0x0004AF6B
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int4 xyyw
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new int4(this.x, this.y, this.y, this.w);
			}
		}

		// Token: 0x17000861 RID: 2145
		// (get) Token: 0x06001BCF RID: 7119 RVA: 0x0004CD8A File Offset: 0x0004AF8A
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int4 xyzx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new int4(this.x, this.y, this.z, this.x);
			}
		}

		// Token: 0x17000862 RID: 2146
		// (get) Token: 0x06001BD0 RID: 7120 RVA: 0x0004CDA9 File Offset: 0x0004AFA9
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int4 xyzy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new int4(this.x, this.y, this.z, this.y);
			}
		}

		// Token: 0x17000863 RID: 2147
		// (get) Token: 0x06001BD1 RID: 7121 RVA: 0x0004CDC8 File Offset: 0x0004AFC8
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int4 xyzz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new int4(this.x, this.y, this.z, this.z);
			}
		}

		// Token: 0x17000864 RID: 2148
		// (get) Token: 0x06001BD2 RID: 7122 RVA: 0x0004CDE7 File Offset: 0x0004AFE7
		// (set) Token: 0x06001BD3 RID: 7123 RVA: 0x0004CE06 File Offset: 0x0004B006
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int4 xyzw
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new int4(this.x, this.y, this.z, this.w);
			}
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			set
			{
				this.x = value.x;
				this.y = value.y;
				this.z = value.z;
				this.w = value.w;
			}
		}

		// Token: 0x17000865 RID: 2149
		// (get) Token: 0x06001BD4 RID: 7124 RVA: 0x0004CE38 File Offset: 0x0004B038
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int4 xywx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new int4(this.x, this.y, this.w, this.x);
			}
		}

		// Token: 0x17000866 RID: 2150
		// (get) Token: 0x06001BD5 RID: 7125 RVA: 0x0004CE57 File Offset: 0x0004B057
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int4 xywy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new int4(this.x, this.y, this.w, this.y);
			}
		}

		// Token: 0x17000867 RID: 2151
		// (get) Token: 0x06001BD6 RID: 7126 RVA: 0x0004CE76 File Offset: 0x0004B076
		// (set) Token: 0x06001BD7 RID: 7127 RVA: 0x0004CE95 File Offset: 0x0004B095
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int4 xywz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new int4(this.x, this.y, this.w, this.z);
			}
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			set
			{
				this.x = value.x;
				this.y = value.y;
				this.w = value.z;
				this.z = value.w;
			}
		}

		// Token: 0x17000868 RID: 2152
		// (get) Token: 0x06001BD8 RID: 7128 RVA: 0x0004CEC7 File Offset: 0x0004B0C7
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int4 xyww
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new int4(this.x, this.y, this.w, this.w);
			}
		}

		// Token: 0x17000869 RID: 2153
		// (get) Token: 0x06001BD9 RID: 7129 RVA: 0x0004CEE6 File Offset: 0x0004B0E6
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int4 xzxx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new int4(this.x, this.z, this.x, this.x);
			}
		}

		// Token: 0x1700086A RID: 2154
		// (get) Token: 0x06001BDA RID: 7130 RVA: 0x0004CF05 File Offset: 0x0004B105
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int4 xzxy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new int4(this.x, this.z, this.x, this.y);
			}
		}

		// Token: 0x1700086B RID: 2155
		// (get) Token: 0x06001BDB RID: 7131 RVA: 0x0004CF24 File Offset: 0x0004B124
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int4 xzxz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new int4(this.x, this.z, this.x, this.z);
			}
		}

		// Token: 0x1700086C RID: 2156
		// (get) Token: 0x06001BDC RID: 7132 RVA: 0x0004CF43 File Offset: 0x0004B143
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int4 xzxw
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new int4(this.x, this.z, this.x, this.w);
			}
		}

		// Token: 0x1700086D RID: 2157
		// (get) Token: 0x06001BDD RID: 7133 RVA: 0x0004CF62 File Offset: 0x0004B162
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int4 xzyx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new int4(this.x, this.z, this.y, this.x);
			}
		}

		// Token: 0x1700086E RID: 2158
		// (get) Token: 0x06001BDE RID: 7134 RVA: 0x0004CF81 File Offset: 0x0004B181
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int4 xzyy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new int4(this.x, this.z, this.y, this.y);
			}
		}

		// Token: 0x1700086F RID: 2159
		// (get) Token: 0x06001BDF RID: 7135 RVA: 0x0004CFA0 File Offset: 0x0004B1A0
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int4 xzyz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new int4(this.x, this.z, this.y, this.z);
			}
		}

		// Token: 0x17000870 RID: 2160
		// (get) Token: 0x06001BE0 RID: 7136 RVA: 0x0004CFBF File Offset: 0x0004B1BF
		// (set) Token: 0x06001BE1 RID: 7137 RVA: 0x0004CFDE File Offset: 0x0004B1DE
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int4 xzyw
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new int4(this.x, this.z, this.y, this.w);
			}
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			set
			{
				this.x = value.x;
				this.z = value.y;
				this.y = value.z;
				this.w = value.w;
			}
		}

		// Token: 0x17000871 RID: 2161
		// (get) Token: 0x06001BE2 RID: 7138 RVA: 0x0004D010 File Offset: 0x0004B210
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int4 xzzx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new int4(this.x, this.z, this.z, this.x);
			}
		}

		// Token: 0x17000872 RID: 2162
		// (get) Token: 0x06001BE3 RID: 7139 RVA: 0x0004D02F File Offset: 0x0004B22F
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int4 xzzy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new int4(this.x, this.z, this.z, this.y);
			}
		}

		// Token: 0x17000873 RID: 2163
		// (get) Token: 0x06001BE4 RID: 7140 RVA: 0x0004D04E File Offset: 0x0004B24E
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int4 xzzz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new int4(this.x, this.z, this.z, this.z);
			}
		}

		// Token: 0x17000874 RID: 2164
		// (get) Token: 0x06001BE5 RID: 7141 RVA: 0x0004D06D File Offset: 0x0004B26D
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int4 xzzw
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new int4(this.x, this.z, this.z, this.w);
			}
		}

		// Token: 0x17000875 RID: 2165
		// (get) Token: 0x06001BE6 RID: 7142 RVA: 0x0004D08C File Offset: 0x0004B28C
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int4 xzwx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new int4(this.x, this.z, this.w, this.x);
			}
		}

		// Token: 0x17000876 RID: 2166
		// (get) Token: 0x06001BE7 RID: 7143 RVA: 0x0004D0AB File Offset: 0x0004B2AB
		// (set) Token: 0x06001BE8 RID: 7144 RVA: 0x0004D0CA File Offset: 0x0004B2CA
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int4 xzwy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new int4(this.x, this.z, this.w, this.y);
			}
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			set
			{
				this.x = value.x;
				this.z = value.y;
				this.w = value.z;
				this.y = value.w;
			}
		}

		// Token: 0x17000877 RID: 2167
		// (get) Token: 0x06001BE9 RID: 7145 RVA: 0x0004D0FC File Offset: 0x0004B2FC
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int4 xzwz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new int4(this.x, this.z, this.w, this.z);
			}
		}

		// Token: 0x17000878 RID: 2168
		// (get) Token: 0x06001BEA RID: 7146 RVA: 0x0004D11B File Offset: 0x0004B31B
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int4 xzww
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new int4(this.x, this.z, this.w, this.w);
			}
		}

		// Token: 0x17000879 RID: 2169
		// (get) Token: 0x06001BEB RID: 7147 RVA: 0x0004D13A File Offset: 0x0004B33A
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int4 xwxx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new int4(this.x, this.w, this.x, this.x);
			}
		}

		// Token: 0x1700087A RID: 2170
		// (get) Token: 0x06001BEC RID: 7148 RVA: 0x0004D159 File Offset: 0x0004B359
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int4 xwxy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new int4(this.x, this.w, this.x, this.y);
			}
		}

		// Token: 0x1700087B RID: 2171
		// (get) Token: 0x06001BED RID: 7149 RVA: 0x0004D178 File Offset: 0x0004B378
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int4 xwxz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new int4(this.x, this.w, this.x, this.z);
			}
		}

		// Token: 0x1700087C RID: 2172
		// (get) Token: 0x06001BEE RID: 7150 RVA: 0x0004D197 File Offset: 0x0004B397
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int4 xwxw
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new int4(this.x, this.w, this.x, this.w);
			}
		}

		// Token: 0x1700087D RID: 2173
		// (get) Token: 0x06001BEF RID: 7151 RVA: 0x0004D1B6 File Offset: 0x0004B3B6
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int4 xwyx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new int4(this.x, this.w, this.y, this.x);
			}
		}

		// Token: 0x1700087E RID: 2174
		// (get) Token: 0x06001BF0 RID: 7152 RVA: 0x0004D1D5 File Offset: 0x0004B3D5
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int4 xwyy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new int4(this.x, this.w, this.y, this.y);
			}
		}

		// Token: 0x1700087F RID: 2175
		// (get) Token: 0x06001BF1 RID: 7153 RVA: 0x0004D1F4 File Offset: 0x0004B3F4
		// (set) Token: 0x06001BF2 RID: 7154 RVA: 0x0004D213 File Offset: 0x0004B413
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int4 xwyz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new int4(this.x, this.w, this.y, this.z);
			}
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			set
			{
				this.x = value.x;
				this.w = value.y;
				this.y = value.z;
				this.z = value.w;
			}
		}

		// Token: 0x17000880 RID: 2176
		// (get) Token: 0x06001BF3 RID: 7155 RVA: 0x0004D245 File Offset: 0x0004B445
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int4 xwyw
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new int4(this.x, this.w, this.y, this.w);
			}
		}

		// Token: 0x17000881 RID: 2177
		// (get) Token: 0x06001BF4 RID: 7156 RVA: 0x0004D264 File Offset: 0x0004B464
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int4 xwzx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new int4(this.x, this.w, this.z, this.x);
			}
		}

		// Token: 0x17000882 RID: 2178
		// (get) Token: 0x06001BF5 RID: 7157 RVA: 0x0004D283 File Offset: 0x0004B483
		// (set) Token: 0x06001BF6 RID: 7158 RVA: 0x0004D2A2 File Offset: 0x0004B4A2
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int4 xwzy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new int4(this.x, this.w, this.z, this.y);
			}
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			set
			{
				this.x = value.x;
				this.w = value.y;
				this.z = value.z;
				this.y = value.w;
			}
		}

		// Token: 0x17000883 RID: 2179
		// (get) Token: 0x06001BF7 RID: 7159 RVA: 0x0004D2D4 File Offset: 0x0004B4D4
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int4 xwzz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new int4(this.x, this.w, this.z, this.z);
			}
		}

		// Token: 0x17000884 RID: 2180
		// (get) Token: 0x06001BF8 RID: 7160 RVA: 0x0004D2F3 File Offset: 0x0004B4F3
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int4 xwzw
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new int4(this.x, this.w, this.z, this.w);
			}
		}

		// Token: 0x17000885 RID: 2181
		// (get) Token: 0x06001BF9 RID: 7161 RVA: 0x0004D312 File Offset: 0x0004B512
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int4 xwwx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new int4(this.x, this.w, this.w, this.x);
			}
		}

		// Token: 0x17000886 RID: 2182
		// (get) Token: 0x06001BFA RID: 7162 RVA: 0x0004D331 File Offset: 0x0004B531
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int4 xwwy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new int4(this.x, this.w, this.w, this.y);
			}
		}

		// Token: 0x17000887 RID: 2183
		// (get) Token: 0x06001BFB RID: 7163 RVA: 0x0004D350 File Offset: 0x0004B550
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int4 xwwz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new int4(this.x, this.w, this.w, this.z);
			}
		}

		// Token: 0x17000888 RID: 2184
		// (get) Token: 0x06001BFC RID: 7164 RVA: 0x0004D36F File Offset: 0x0004B56F
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int4 xwww
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new int4(this.x, this.w, this.w, this.w);
			}
		}

		// Token: 0x17000889 RID: 2185
		// (get) Token: 0x06001BFD RID: 7165 RVA: 0x0004D38E File Offset: 0x0004B58E
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int4 yxxx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new int4(this.y, this.x, this.x, this.x);
			}
		}

		// Token: 0x1700088A RID: 2186
		// (get) Token: 0x06001BFE RID: 7166 RVA: 0x0004D3AD File Offset: 0x0004B5AD
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int4 yxxy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new int4(this.y, this.x, this.x, this.y);
			}
		}

		// Token: 0x1700088B RID: 2187
		// (get) Token: 0x06001BFF RID: 7167 RVA: 0x0004D3CC File Offset: 0x0004B5CC
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int4 yxxz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new int4(this.y, this.x, this.x, this.z);
			}
		}

		// Token: 0x1700088C RID: 2188
		// (get) Token: 0x06001C00 RID: 7168 RVA: 0x0004D3EB File Offset: 0x0004B5EB
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int4 yxxw
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new int4(this.y, this.x, this.x, this.w);
			}
		}

		// Token: 0x1700088D RID: 2189
		// (get) Token: 0x06001C01 RID: 7169 RVA: 0x0004D40A File Offset: 0x0004B60A
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int4 yxyx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new int4(this.y, this.x, this.y, this.x);
			}
		}

		// Token: 0x1700088E RID: 2190
		// (get) Token: 0x06001C02 RID: 7170 RVA: 0x0004D429 File Offset: 0x0004B629
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int4 yxyy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new int4(this.y, this.x, this.y, this.y);
			}
		}

		// Token: 0x1700088F RID: 2191
		// (get) Token: 0x06001C03 RID: 7171 RVA: 0x0004D448 File Offset: 0x0004B648
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int4 yxyz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new int4(this.y, this.x, this.y, this.z);
			}
		}

		// Token: 0x17000890 RID: 2192
		// (get) Token: 0x06001C04 RID: 7172 RVA: 0x0004D467 File Offset: 0x0004B667
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int4 yxyw
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new int4(this.y, this.x, this.y, this.w);
			}
		}

		// Token: 0x17000891 RID: 2193
		// (get) Token: 0x06001C05 RID: 7173 RVA: 0x0004D486 File Offset: 0x0004B686
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int4 yxzx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new int4(this.y, this.x, this.z, this.x);
			}
		}

		// Token: 0x17000892 RID: 2194
		// (get) Token: 0x06001C06 RID: 7174 RVA: 0x0004D4A5 File Offset: 0x0004B6A5
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int4 yxzy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new int4(this.y, this.x, this.z, this.y);
			}
		}

		// Token: 0x17000893 RID: 2195
		// (get) Token: 0x06001C07 RID: 7175 RVA: 0x0004D4C4 File Offset: 0x0004B6C4
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int4 yxzz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new int4(this.y, this.x, this.z, this.z);
			}
		}

		// Token: 0x17000894 RID: 2196
		// (get) Token: 0x06001C08 RID: 7176 RVA: 0x0004D4E3 File Offset: 0x0004B6E3
		// (set) Token: 0x06001C09 RID: 7177 RVA: 0x0004D502 File Offset: 0x0004B702
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int4 yxzw
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new int4(this.y, this.x, this.z, this.w);
			}
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			set
			{
				this.y = value.x;
				this.x = value.y;
				this.z = value.z;
				this.w = value.w;
			}
		}

		// Token: 0x17000895 RID: 2197
		// (get) Token: 0x06001C0A RID: 7178 RVA: 0x0004D534 File Offset: 0x0004B734
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int4 yxwx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new int4(this.y, this.x, this.w, this.x);
			}
		}

		// Token: 0x17000896 RID: 2198
		// (get) Token: 0x06001C0B RID: 7179 RVA: 0x0004D553 File Offset: 0x0004B753
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int4 yxwy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new int4(this.y, this.x, this.w, this.y);
			}
		}

		// Token: 0x17000897 RID: 2199
		// (get) Token: 0x06001C0C RID: 7180 RVA: 0x0004D572 File Offset: 0x0004B772
		// (set) Token: 0x06001C0D RID: 7181 RVA: 0x0004D591 File Offset: 0x0004B791
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int4 yxwz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new int4(this.y, this.x, this.w, this.z);
			}
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			set
			{
				this.y = value.x;
				this.x = value.y;
				this.w = value.z;
				this.z = value.w;
			}
		}

		// Token: 0x17000898 RID: 2200
		// (get) Token: 0x06001C0E RID: 7182 RVA: 0x0004D5C3 File Offset: 0x0004B7C3
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int4 yxww
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new int4(this.y, this.x, this.w, this.w);
			}
		}

		// Token: 0x17000899 RID: 2201
		// (get) Token: 0x06001C0F RID: 7183 RVA: 0x0004D5E2 File Offset: 0x0004B7E2
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int4 yyxx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new int4(this.y, this.y, this.x, this.x);
			}
		}

		// Token: 0x1700089A RID: 2202
		// (get) Token: 0x06001C10 RID: 7184 RVA: 0x0004D601 File Offset: 0x0004B801
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int4 yyxy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new int4(this.y, this.y, this.x, this.y);
			}
		}

		// Token: 0x1700089B RID: 2203
		// (get) Token: 0x06001C11 RID: 7185 RVA: 0x0004D620 File Offset: 0x0004B820
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int4 yyxz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new int4(this.y, this.y, this.x, this.z);
			}
		}

		// Token: 0x1700089C RID: 2204
		// (get) Token: 0x06001C12 RID: 7186 RVA: 0x0004D63F File Offset: 0x0004B83F
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int4 yyxw
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new int4(this.y, this.y, this.x, this.w);
			}
		}

		// Token: 0x1700089D RID: 2205
		// (get) Token: 0x06001C13 RID: 7187 RVA: 0x0004D65E File Offset: 0x0004B85E
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int4 yyyx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new int4(this.y, this.y, this.y, this.x);
			}
		}

		// Token: 0x1700089E RID: 2206
		// (get) Token: 0x06001C14 RID: 7188 RVA: 0x0004D67D File Offset: 0x0004B87D
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int4 yyyy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new int4(this.y, this.y, this.y, this.y);
			}
		}

		// Token: 0x1700089F RID: 2207
		// (get) Token: 0x06001C15 RID: 7189 RVA: 0x0004D69C File Offset: 0x0004B89C
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int4 yyyz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new int4(this.y, this.y, this.y, this.z);
			}
		}

		// Token: 0x170008A0 RID: 2208
		// (get) Token: 0x06001C16 RID: 7190 RVA: 0x0004D6BB File Offset: 0x0004B8BB
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int4 yyyw
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new int4(this.y, this.y, this.y, this.w);
			}
		}

		// Token: 0x170008A1 RID: 2209
		// (get) Token: 0x06001C17 RID: 7191 RVA: 0x0004D6DA File Offset: 0x0004B8DA
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int4 yyzx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new int4(this.y, this.y, this.z, this.x);
			}
		}

		// Token: 0x170008A2 RID: 2210
		// (get) Token: 0x06001C18 RID: 7192 RVA: 0x0004D6F9 File Offset: 0x0004B8F9
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int4 yyzy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new int4(this.y, this.y, this.z, this.y);
			}
		}

		// Token: 0x170008A3 RID: 2211
		// (get) Token: 0x06001C19 RID: 7193 RVA: 0x0004D718 File Offset: 0x0004B918
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int4 yyzz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new int4(this.y, this.y, this.z, this.z);
			}
		}

		// Token: 0x170008A4 RID: 2212
		// (get) Token: 0x06001C1A RID: 7194 RVA: 0x0004D737 File Offset: 0x0004B937
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int4 yyzw
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new int4(this.y, this.y, this.z, this.w);
			}
		}

		// Token: 0x170008A5 RID: 2213
		// (get) Token: 0x06001C1B RID: 7195 RVA: 0x0004D756 File Offset: 0x0004B956
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int4 yywx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new int4(this.y, this.y, this.w, this.x);
			}
		}

		// Token: 0x170008A6 RID: 2214
		// (get) Token: 0x06001C1C RID: 7196 RVA: 0x0004D775 File Offset: 0x0004B975
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int4 yywy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new int4(this.y, this.y, this.w, this.y);
			}
		}

		// Token: 0x170008A7 RID: 2215
		// (get) Token: 0x06001C1D RID: 7197 RVA: 0x0004D794 File Offset: 0x0004B994
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int4 yywz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new int4(this.y, this.y, this.w, this.z);
			}
		}

		// Token: 0x170008A8 RID: 2216
		// (get) Token: 0x06001C1E RID: 7198 RVA: 0x0004D7B3 File Offset: 0x0004B9B3
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int4 yyww
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new int4(this.y, this.y, this.w, this.w);
			}
		}

		// Token: 0x170008A9 RID: 2217
		// (get) Token: 0x06001C1F RID: 7199 RVA: 0x0004D7D2 File Offset: 0x0004B9D2
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int4 yzxx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new int4(this.y, this.z, this.x, this.x);
			}
		}

		// Token: 0x170008AA RID: 2218
		// (get) Token: 0x06001C20 RID: 7200 RVA: 0x0004D7F1 File Offset: 0x0004B9F1
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int4 yzxy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new int4(this.y, this.z, this.x, this.y);
			}
		}

		// Token: 0x170008AB RID: 2219
		// (get) Token: 0x06001C21 RID: 7201 RVA: 0x0004D810 File Offset: 0x0004BA10
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int4 yzxz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new int4(this.y, this.z, this.x, this.z);
			}
		}

		// Token: 0x170008AC RID: 2220
		// (get) Token: 0x06001C22 RID: 7202 RVA: 0x0004D82F File Offset: 0x0004BA2F
		// (set) Token: 0x06001C23 RID: 7203 RVA: 0x0004D84E File Offset: 0x0004BA4E
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int4 yzxw
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new int4(this.y, this.z, this.x, this.w);
			}
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			set
			{
				this.y = value.x;
				this.z = value.y;
				this.x = value.z;
				this.w = value.w;
			}
		}

		// Token: 0x170008AD RID: 2221
		// (get) Token: 0x06001C24 RID: 7204 RVA: 0x0004D880 File Offset: 0x0004BA80
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int4 yzyx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new int4(this.y, this.z, this.y, this.x);
			}
		}

		// Token: 0x170008AE RID: 2222
		// (get) Token: 0x06001C25 RID: 7205 RVA: 0x0004D89F File Offset: 0x0004BA9F
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int4 yzyy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new int4(this.y, this.z, this.y, this.y);
			}
		}

		// Token: 0x170008AF RID: 2223
		// (get) Token: 0x06001C26 RID: 7206 RVA: 0x0004D8BE File Offset: 0x0004BABE
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int4 yzyz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new int4(this.y, this.z, this.y, this.z);
			}
		}

		// Token: 0x170008B0 RID: 2224
		// (get) Token: 0x06001C27 RID: 7207 RVA: 0x0004D8DD File Offset: 0x0004BADD
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int4 yzyw
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new int4(this.y, this.z, this.y, this.w);
			}
		}

		// Token: 0x170008B1 RID: 2225
		// (get) Token: 0x06001C28 RID: 7208 RVA: 0x0004D8FC File Offset: 0x0004BAFC
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int4 yzzx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new int4(this.y, this.z, this.z, this.x);
			}
		}

		// Token: 0x170008B2 RID: 2226
		// (get) Token: 0x06001C29 RID: 7209 RVA: 0x0004D91B File Offset: 0x0004BB1B
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int4 yzzy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new int4(this.y, this.z, this.z, this.y);
			}
		}

		// Token: 0x170008B3 RID: 2227
		// (get) Token: 0x06001C2A RID: 7210 RVA: 0x0004D93A File Offset: 0x0004BB3A
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int4 yzzz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new int4(this.y, this.z, this.z, this.z);
			}
		}

		// Token: 0x170008B4 RID: 2228
		// (get) Token: 0x06001C2B RID: 7211 RVA: 0x0004D959 File Offset: 0x0004BB59
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int4 yzzw
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new int4(this.y, this.z, this.z, this.w);
			}
		}

		// Token: 0x170008B5 RID: 2229
		// (get) Token: 0x06001C2C RID: 7212 RVA: 0x0004D978 File Offset: 0x0004BB78
		// (set) Token: 0x06001C2D RID: 7213 RVA: 0x0004D997 File Offset: 0x0004BB97
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int4 yzwx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new int4(this.y, this.z, this.w, this.x);
			}
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			set
			{
				this.y = value.x;
				this.z = value.y;
				this.w = value.z;
				this.x = value.w;
			}
		}

		// Token: 0x170008B6 RID: 2230
		// (get) Token: 0x06001C2E RID: 7214 RVA: 0x0004D9C9 File Offset: 0x0004BBC9
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int4 yzwy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new int4(this.y, this.z, this.w, this.y);
			}
		}

		// Token: 0x170008B7 RID: 2231
		// (get) Token: 0x06001C2F RID: 7215 RVA: 0x0004D9E8 File Offset: 0x0004BBE8
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int4 yzwz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new int4(this.y, this.z, this.w, this.z);
			}
		}

		// Token: 0x170008B8 RID: 2232
		// (get) Token: 0x06001C30 RID: 7216 RVA: 0x0004DA07 File Offset: 0x0004BC07
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int4 yzww
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new int4(this.y, this.z, this.w, this.w);
			}
		}

		// Token: 0x170008B9 RID: 2233
		// (get) Token: 0x06001C31 RID: 7217 RVA: 0x0004DA26 File Offset: 0x0004BC26
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int4 ywxx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new int4(this.y, this.w, this.x, this.x);
			}
		}

		// Token: 0x170008BA RID: 2234
		// (get) Token: 0x06001C32 RID: 7218 RVA: 0x0004DA45 File Offset: 0x0004BC45
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int4 ywxy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new int4(this.y, this.w, this.x, this.y);
			}
		}

		// Token: 0x170008BB RID: 2235
		// (get) Token: 0x06001C33 RID: 7219 RVA: 0x0004DA64 File Offset: 0x0004BC64
		// (set) Token: 0x06001C34 RID: 7220 RVA: 0x0004DA83 File Offset: 0x0004BC83
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int4 ywxz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new int4(this.y, this.w, this.x, this.z);
			}
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			set
			{
				this.y = value.x;
				this.w = value.y;
				this.x = value.z;
				this.z = value.w;
			}
		}

		// Token: 0x170008BC RID: 2236
		// (get) Token: 0x06001C35 RID: 7221 RVA: 0x0004DAB5 File Offset: 0x0004BCB5
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int4 ywxw
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new int4(this.y, this.w, this.x, this.w);
			}
		}

		// Token: 0x170008BD RID: 2237
		// (get) Token: 0x06001C36 RID: 7222 RVA: 0x0004DAD4 File Offset: 0x0004BCD4
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int4 ywyx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new int4(this.y, this.w, this.y, this.x);
			}
		}

		// Token: 0x170008BE RID: 2238
		// (get) Token: 0x06001C37 RID: 7223 RVA: 0x0004DAF3 File Offset: 0x0004BCF3
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int4 ywyy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new int4(this.y, this.w, this.y, this.y);
			}
		}

		// Token: 0x170008BF RID: 2239
		// (get) Token: 0x06001C38 RID: 7224 RVA: 0x0004DB12 File Offset: 0x0004BD12
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int4 ywyz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new int4(this.y, this.w, this.y, this.z);
			}
		}

		// Token: 0x170008C0 RID: 2240
		// (get) Token: 0x06001C39 RID: 7225 RVA: 0x0004DB31 File Offset: 0x0004BD31
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int4 ywyw
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new int4(this.y, this.w, this.y, this.w);
			}
		}

		// Token: 0x170008C1 RID: 2241
		// (get) Token: 0x06001C3A RID: 7226 RVA: 0x0004DB50 File Offset: 0x0004BD50
		// (set) Token: 0x06001C3B RID: 7227 RVA: 0x0004DB6F File Offset: 0x0004BD6F
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int4 ywzx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new int4(this.y, this.w, this.z, this.x);
			}
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			set
			{
				this.y = value.x;
				this.w = value.y;
				this.z = value.z;
				this.x = value.w;
			}
		}

		// Token: 0x170008C2 RID: 2242
		// (get) Token: 0x06001C3C RID: 7228 RVA: 0x0004DBA1 File Offset: 0x0004BDA1
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int4 ywzy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new int4(this.y, this.w, this.z, this.y);
			}
		}

		// Token: 0x170008C3 RID: 2243
		// (get) Token: 0x06001C3D RID: 7229 RVA: 0x0004DBC0 File Offset: 0x0004BDC0
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int4 ywzz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new int4(this.y, this.w, this.z, this.z);
			}
		}

		// Token: 0x170008C4 RID: 2244
		// (get) Token: 0x06001C3E RID: 7230 RVA: 0x0004DBDF File Offset: 0x0004BDDF
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int4 ywzw
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new int4(this.y, this.w, this.z, this.w);
			}
		}

		// Token: 0x170008C5 RID: 2245
		// (get) Token: 0x06001C3F RID: 7231 RVA: 0x0004DBFE File Offset: 0x0004BDFE
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int4 ywwx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new int4(this.y, this.w, this.w, this.x);
			}
		}

		// Token: 0x170008C6 RID: 2246
		// (get) Token: 0x06001C40 RID: 7232 RVA: 0x0004DC1D File Offset: 0x0004BE1D
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int4 ywwy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new int4(this.y, this.w, this.w, this.y);
			}
		}

		// Token: 0x170008C7 RID: 2247
		// (get) Token: 0x06001C41 RID: 7233 RVA: 0x0004DC3C File Offset: 0x0004BE3C
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int4 ywwz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new int4(this.y, this.w, this.w, this.z);
			}
		}

		// Token: 0x170008C8 RID: 2248
		// (get) Token: 0x06001C42 RID: 7234 RVA: 0x0004DC5B File Offset: 0x0004BE5B
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int4 ywww
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new int4(this.y, this.w, this.w, this.w);
			}
		}

		// Token: 0x170008C9 RID: 2249
		// (get) Token: 0x06001C43 RID: 7235 RVA: 0x0004DC7A File Offset: 0x0004BE7A
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int4 zxxx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new int4(this.z, this.x, this.x, this.x);
			}
		}

		// Token: 0x170008CA RID: 2250
		// (get) Token: 0x06001C44 RID: 7236 RVA: 0x0004DC99 File Offset: 0x0004BE99
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int4 zxxy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new int4(this.z, this.x, this.x, this.y);
			}
		}

		// Token: 0x170008CB RID: 2251
		// (get) Token: 0x06001C45 RID: 7237 RVA: 0x0004DCB8 File Offset: 0x0004BEB8
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int4 zxxz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new int4(this.z, this.x, this.x, this.z);
			}
		}

		// Token: 0x170008CC RID: 2252
		// (get) Token: 0x06001C46 RID: 7238 RVA: 0x0004DCD7 File Offset: 0x0004BED7
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int4 zxxw
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new int4(this.z, this.x, this.x, this.w);
			}
		}

		// Token: 0x170008CD RID: 2253
		// (get) Token: 0x06001C47 RID: 7239 RVA: 0x0004DCF6 File Offset: 0x0004BEF6
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int4 zxyx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new int4(this.z, this.x, this.y, this.x);
			}
		}

		// Token: 0x170008CE RID: 2254
		// (get) Token: 0x06001C48 RID: 7240 RVA: 0x0004DD15 File Offset: 0x0004BF15
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int4 zxyy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new int4(this.z, this.x, this.y, this.y);
			}
		}

		// Token: 0x170008CF RID: 2255
		// (get) Token: 0x06001C49 RID: 7241 RVA: 0x0004DD34 File Offset: 0x0004BF34
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int4 zxyz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new int4(this.z, this.x, this.y, this.z);
			}
		}

		// Token: 0x170008D0 RID: 2256
		// (get) Token: 0x06001C4A RID: 7242 RVA: 0x0004DD53 File Offset: 0x0004BF53
		// (set) Token: 0x06001C4B RID: 7243 RVA: 0x0004DD72 File Offset: 0x0004BF72
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int4 zxyw
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new int4(this.z, this.x, this.y, this.w);
			}
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			set
			{
				this.z = value.x;
				this.x = value.y;
				this.y = value.z;
				this.w = value.w;
			}
		}

		// Token: 0x170008D1 RID: 2257
		// (get) Token: 0x06001C4C RID: 7244 RVA: 0x0004DDA4 File Offset: 0x0004BFA4
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int4 zxzx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new int4(this.z, this.x, this.z, this.x);
			}
		}

		// Token: 0x170008D2 RID: 2258
		// (get) Token: 0x06001C4D RID: 7245 RVA: 0x0004DDC3 File Offset: 0x0004BFC3
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int4 zxzy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new int4(this.z, this.x, this.z, this.y);
			}
		}

		// Token: 0x170008D3 RID: 2259
		// (get) Token: 0x06001C4E RID: 7246 RVA: 0x0004DDE2 File Offset: 0x0004BFE2
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int4 zxzz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new int4(this.z, this.x, this.z, this.z);
			}
		}

		// Token: 0x170008D4 RID: 2260
		// (get) Token: 0x06001C4F RID: 7247 RVA: 0x0004DE01 File Offset: 0x0004C001
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int4 zxzw
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new int4(this.z, this.x, this.z, this.w);
			}
		}

		// Token: 0x170008D5 RID: 2261
		// (get) Token: 0x06001C50 RID: 7248 RVA: 0x0004DE20 File Offset: 0x0004C020
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int4 zxwx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new int4(this.z, this.x, this.w, this.x);
			}
		}

		// Token: 0x170008D6 RID: 2262
		// (get) Token: 0x06001C51 RID: 7249 RVA: 0x0004DE3F File Offset: 0x0004C03F
		// (set) Token: 0x06001C52 RID: 7250 RVA: 0x0004DE5E File Offset: 0x0004C05E
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int4 zxwy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new int4(this.z, this.x, this.w, this.y);
			}
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			set
			{
				this.z = value.x;
				this.x = value.y;
				this.w = value.z;
				this.y = value.w;
			}
		}

		// Token: 0x170008D7 RID: 2263
		// (get) Token: 0x06001C53 RID: 7251 RVA: 0x0004DE90 File Offset: 0x0004C090
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int4 zxwz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new int4(this.z, this.x, this.w, this.z);
			}
		}

		// Token: 0x170008D8 RID: 2264
		// (get) Token: 0x06001C54 RID: 7252 RVA: 0x0004DEAF File Offset: 0x0004C0AF
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int4 zxww
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new int4(this.z, this.x, this.w, this.w);
			}
		}

		// Token: 0x170008D9 RID: 2265
		// (get) Token: 0x06001C55 RID: 7253 RVA: 0x0004DECE File Offset: 0x0004C0CE
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int4 zyxx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new int4(this.z, this.y, this.x, this.x);
			}
		}

		// Token: 0x170008DA RID: 2266
		// (get) Token: 0x06001C56 RID: 7254 RVA: 0x0004DEED File Offset: 0x0004C0ED
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int4 zyxy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new int4(this.z, this.y, this.x, this.y);
			}
		}

		// Token: 0x170008DB RID: 2267
		// (get) Token: 0x06001C57 RID: 7255 RVA: 0x0004DF0C File Offset: 0x0004C10C
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int4 zyxz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new int4(this.z, this.y, this.x, this.z);
			}
		}

		// Token: 0x170008DC RID: 2268
		// (get) Token: 0x06001C58 RID: 7256 RVA: 0x0004DF2B File Offset: 0x0004C12B
		// (set) Token: 0x06001C59 RID: 7257 RVA: 0x0004DF4A File Offset: 0x0004C14A
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int4 zyxw
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new int4(this.z, this.y, this.x, this.w);
			}
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			set
			{
				this.z = value.x;
				this.y = value.y;
				this.x = value.z;
				this.w = value.w;
			}
		}

		// Token: 0x170008DD RID: 2269
		// (get) Token: 0x06001C5A RID: 7258 RVA: 0x0004DF7C File Offset: 0x0004C17C
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int4 zyyx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new int4(this.z, this.y, this.y, this.x);
			}
		}

		// Token: 0x170008DE RID: 2270
		// (get) Token: 0x06001C5B RID: 7259 RVA: 0x0004DF9B File Offset: 0x0004C19B
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int4 zyyy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new int4(this.z, this.y, this.y, this.y);
			}
		}

		// Token: 0x170008DF RID: 2271
		// (get) Token: 0x06001C5C RID: 7260 RVA: 0x0004DFBA File Offset: 0x0004C1BA
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int4 zyyz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new int4(this.z, this.y, this.y, this.z);
			}
		}

		// Token: 0x170008E0 RID: 2272
		// (get) Token: 0x06001C5D RID: 7261 RVA: 0x0004DFD9 File Offset: 0x0004C1D9
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int4 zyyw
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new int4(this.z, this.y, this.y, this.w);
			}
		}

		// Token: 0x170008E1 RID: 2273
		// (get) Token: 0x06001C5E RID: 7262 RVA: 0x0004DFF8 File Offset: 0x0004C1F8
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int4 zyzx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new int4(this.z, this.y, this.z, this.x);
			}
		}

		// Token: 0x170008E2 RID: 2274
		// (get) Token: 0x06001C5F RID: 7263 RVA: 0x0004E017 File Offset: 0x0004C217
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int4 zyzy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new int4(this.z, this.y, this.z, this.y);
			}
		}

		// Token: 0x170008E3 RID: 2275
		// (get) Token: 0x06001C60 RID: 7264 RVA: 0x0004E036 File Offset: 0x0004C236
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int4 zyzz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new int4(this.z, this.y, this.z, this.z);
			}
		}

		// Token: 0x170008E4 RID: 2276
		// (get) Token: 0x06001C61 RID: 7265 RVA: 0x0004E055 File Offset: 0x0004C255
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int4 zyzw
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new int4(this.z, this.y, this.z, this.w);
			}
		}

		// Token: 0x170008E5 RID: 2277
		// (get) Token: 0x06001C62 RID: 7266 RVA: 0x0004E074 File Offset: 0x0004C274
		// (set) Token: 0x06001C63 RID: 7267 RVA: 0x0004E093 File Offset: 0x0004C293
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int4 zywx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new int4(this.z, this.y, this.w, this.x);
			}
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			set
			{
				this.z = value.x;
				this.y = value.y;
				this.w = value.z;
				this.x = value.w;
			}
		}

		// Token: 0x170008E6 RID: 2278
		// (get) Token: 0x06001C64 RID: 7268 RVA: 0x0004E0C5 File Offset: 0x0004C2C5
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int4 zywy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new int4(this.z, this.y, this.w, this.y);
			}
		}

		// Token: 0x170008E7 RID: 2279
		// (get) Token: 0x06001C65 RID: 7269 RVA: 0x0004E0E4 File Offset: 0x0004C2E4
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int4 zywz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new int4(this.z, this.y, this.w, this.z);
			}
		}

		// Token: 0x170008E8 RID: 2280
		// (get) Token: 0x06001C66 RID: 7270 RVA: 0x0004E103 File Offset: 0x0004C303
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int4 zyww
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new int4(this.z, this.y, this.w, this.w);
			}
		}

		// Token: 0x170008E9 RID: 2281
		// (get) Token: 0x06001C67 RID: 7271 RVA: 0x0004E122 File Offset: 0x0004C322
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int4 zzxx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new int4(this.z, this.z, this.x, this.x);
			}
		}

		// Token: 0x170008EA RID: 2282
		// (get) Token: 0x06001C68 RID: 7272 RVA: 0x0004E141 File Offset: 0x0004C341
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int4 zzxy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new int4(this.z, this.z, this.x, this.y);
			}
		}

		// Token: 0x170008EB RID: 2283
		// (get) Token: 0x06001C69 RID: 7273 RVA: 0x0004E160 File Offset: 0x0004C360
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int4 zzxz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new int4(this.z, this.z, this.x, this.z);
			}
		}

		// Token: 0x170008EC RID: 2284
		// (get) Token: 0x06001C6A RID: 7274 RVA: 0x0004E17F File Offset: 0x0004C37F
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int4 zzxw
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new int4(this.z, this.z, this.x, this.w);
			}
		}

		// Token: 0x170008ED RID: 2285
		// (get) Token: 0x06001C6B RID: 7275 RVA: 0x0004E19E File Offset: 0x0004C39E
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int4 zzyx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new int4(this.z, this.z, this.y, this.x);
			}
		}

		// Token: 0x170008EE RID: 2286
		// (get) Token: 0x06001C6C RID: 7276 RVA: 0x0004E1BD File Offset: 0x0004C3BD
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int4 zzyy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new int4(this.z, this.z, this.y, this.y);
			}
		}

		// Token: 0x170008EF RID: 2287
		// (get) Token: 0x06001C6D RID: 7277 RVA: 0x0004E1DC File Offset: 0x0004C3DC
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int4 zzyz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new int4(this.z, this.z, this.y, this.z);
			}
		}

		// Token: 0x170008F0 RID: 2288
		// (get) Token: 0x06001C6E RID: 7278 RVA: 0x0004E1FB File Offset: 0x0004C3FB
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int4 zzyw
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new int4(this.z, this.z, this.y, this.w);
			}
		}

		// Token: 0x170008F1 RID: 2289
		// (get) Token: 0x06001C6F RID: 7279 RVA: 0x0004E21A File Offset: 0x0004C41A
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int4 zzzx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new int4(this.z, this.z, this.z, this.x);
			}
		}

		// Token: 0x170008F2 RID: 2290
		// (get) Token: 0x06001C70 RID: 7280 RVA: 0x0004E239 File Offset: 0x0004C439
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int4 zzzy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new int4(this.z, this.z, this.z, this.y);
			}
		}

		// Token: 0x170008F3 RID: 2291
		// (get) Token: 0x06001C71 RID: 7281 RVA: 0x0004E258 File Offset: 0x0004C458
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int4 zzzz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new int4(this.z, this.z, this.z, this.z);
			}
		}

		// Token: 0x170008F4 RID: 2292
		// (get) Token: 0x06001C72 RID: 7282 RVA: 0x0004E277 File Offset: 0x0004C477
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int4 zzzw
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new int4(this.z, this.z, this.z, this.w);
			}
		}

		// Token: 0x170008F5 RID: 2293
		// (get) Token: 0x06001C73 RID: 7283 RVA: 0x0004E296 File Offset: 0x0004C496
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int4 zzwx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new int4(this.z, this.z, this.w, this.x);
			}
		}

		// Token: 0x170008F6 RID: 2294
		// (get) Token: 0x06001C74 RID: 7284 RVA: 0x0004E2B5 File Offset: 0x0004C4B5
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int4 zzwy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new int4(this.z, this.z, this.w, this.y);
			}
		}

		// Token: 0x170008F7 RID: 2295
		// (get) Token: 0x06001C75 RID: 7285 RVA: 0x0004E2D4 File Offset: 0x0004C4D4
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int4 zzwz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new int4(this.z, this.z, this.w, this.z);
			}
		}

		// Token: 0x170008F8 RID: 2296
		// (get) Token: 0x06001C76 RID: 7286 RVA: 0x0004E2F3 File Offset: 0x0004C4F3
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int4 zzww
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new int4(this.z, this.z, this.w, this.w);
			}
		}

		// Token: 0x170008F9 RID: 2297
		// (get) Token: 0x06001C77 RID: 7287 RVA: 0x0004E312 File Offset: 0x0004C512
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int4 zwxx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new int4(this.z, this.w, this.x, this.x);
			}
		}

		// Token: 0x170008FA RID: 2298
		// (get) Token: 0x06001C78 RID: 7288 RVA: 0x0004E331 File Offset: 0x0004C531
		// (set) Token: 0x06001C79 RID: 7289 RVA: 0x0004E350 File Offset: 0x0004C550
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int4 zwxy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new int4(this.z, this.w, this.x, this.y);
			}
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			set
			{
				this.z = value.x;
				this.w = value.y;
				this.x = value.z;
				this.y = value.w;
			}
		}

		// Token: 0x170008FB RID: 2299
		// (get) Token: 0x06001C7A RID: 7290 RVA: 0x0004E382 File Offset: 0x0004C582
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int4 zwxz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new int4(this.z, this.w, this.x, this.z);
			}
		}

		// Token: 0x170008FC RID: 2300
		// (get) Token: 0x06001C7B RID: 7291 RVA: 0x0004E3A1 File Offset: 0x0004C5A1
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int4 zwxw
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new int4(this.z, this.w, this.x, this.w);
			}
		}

		// Token: 0x170008FD RID: 2301
		// (get) Token: 0x06001C7C RID: 7292 RVA: 0x0004E3C0 File Offset: 0x0004C5C0
		// (set) Token: 0x06001C7D RID: 7293 RVA: 0x0004E3DF File Offset: 0x0004C5DF
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int4 zwyx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new int4(this.z, this.w, this.y, this.x);
			}
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			set
			{
				this.z = value.x;
				this.w = value.y;
				this.y = value.z;
				this.x = value.w;
			}
		}

		// Token: 0x170008FE RID: 2302
		// (get) Token: 0x06001C7E RID: 7294 RVA: 0x0004E411 File Offset: 0x0004C611
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int4 zwyy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new int4(this.z, this.w, this.y, this.y);
			}
		}

		// Token: 0x170008FF RID: 2303
		// (get) Token: 0x06001C7F RID: 7295 RVA: 0x0004E430 File Offset: 0x0004C630
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int4 zwyz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new int4(this.z, this.w, this.y, this.z);
			}
		}

		// Token: 0x17000900 RID: 2304
		// (get) Token: 0x06001C80 RID: 7296 RVA: 0x0004E44F File Offset: 0x0004C64F
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int4 zwyw
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new int4(this.z, this.w, this.y, this.w);
			}
		}

		// Token: 0x17000901 RID: 2305
		// (get) Token: 0x06001C81 RID: 7297 RVA: 0x0004E46E File Offset: 0x0004C66E
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int4 zwzx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new int4(this.z, this.w, this.z, this.x);
			}
		}

		// Token: 0x17000902 RID: 2306
		// (get) Token: 0x06001C82 RID: 7298 RVA: 0x0004E48D File Offset: 0x0004C68D
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int4 zwzy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new int4(this.z, this.w, this.z, this.y);
			}
		}

		// Token: 0x17000903 RID: 2307
		// (get) Token: 0x06001C83 RID: 7299 RVA: 0x0004E4AC File Offset: 0x0004C6AC
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int4 zwzz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new int4(this.z, this.w, this.z, this.z);
			}
		}

		// Token: 0x17000904 RID: 2308
		// (get) Token: 0x06001C84 RID: 7300 RVA: 0x0004E4CB File Offset: 0x0004C6CB
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int4 zwzw
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new int4(this.z, this.w, this.z, this.w);
			}
		}

		// Token: 0x17000905 RID: 2309
		// (get) Token: 0x06001C85 RID: 7301 RVA: 0x0004E4EA File Offset: 0x0004C6EA
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int4 zwwx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new int4(this.z, this.w, this.w, this.x);
			}
		}

		// Token: 0x17000906 RID: 2310
		// (get) Token: 0x06001C86 RID: 7302 RVA: 0x0004E509 File Offset: 0x0004C709
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int4 zwwy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new int4(this.z, this.w, this.w, this.y);
			}
		}

		// Token: 0x17000907 RID: 2311
		// (get) Token: 0x06001C87 RID: 7303 RVA: 0x0004E528 File Offset: 0x0004C728
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int4 zwwz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new int4(this.z, this.w, this.w, this.z);
			}
		}

		// Token: 0x17000908 RID: 2312
		// (get) Token: 0x06001C88 RID: 7304 RVA: 0x0004E547 File Offset: 0x0004C747
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int4 zwww
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new int4(this.z, this.w, this.w, this.w);
			}
		}

		// Token: 0x17000909 RID: 2313
		// (get) Token: 0x06001C89 RID: 7305 RVA: 0x0004E566 File Offset: 0x0004C766
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int4 wxxx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new int4(this.w, this.x, this.x, this.x);
			}
		}

		// Token: 0x1700090A RID: 2314
		// (get) Token: 0x06001C8A RID: 7306 RVA: 0x0004E585 File Offset: 0x0004C785
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int4 wxxy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new int4(this.w, this.x, this.x, this.y);
			}
		}

		// Token: 0x1700090B RID: 2315
		// (get) Token: 0x06001C8B RID: 7307 RVA: 0x0004E5A4 File Offset: 0x0004C7A4
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int4 wxxz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new int4(this.w, this.x, this.x, this.z);
			}
		}

		// Token: 0x1700090C RID: 2316
		// (get) Token: 0x06001C8C RID: 7308 RVA: 0x0004E5C3 File Offset: 0x0004C7C3
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int4 wxxw
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new int4(this.w, this.x, this.x, this.w);
			}
		}

		// Token: 0x1700090D RID: 2317
		// (get) Token: 0x06001C8D RID: 7309 RVA: 0x0004E5E2 File Offset: 0x0004C7E2
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int4 wxyx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new int4(this.w, this.x, this.y, this.x);
			}
		}

		// Token: 0x1700090E RID: 2318
		// (get) Token: 0x06001C8E RID: 7310 RVA: 0x0004E601 File Offset: 0x0004C801
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int4 wxyy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new int4(this.w, this.x, this.y, this.y);
			}
		}

		// Token: 0x1700090F RID: 2319
		// (get) Token: 0x06001C8F RID: 7311 RVA: 0x0004E620 File Offset: 0x0004C820
		// (set) Token: 0x06001C90 RID: 7312 RVA: 0x0004E63F File Offset: 0x0004C83F
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int4 wxyz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new int4(this.w, this.x, this.y, this.z);
			}
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			set
			{
				this.w = value.x;
				this.x = value.y;
				this.y = value.z;
				this.z = value.w;
			}
		}

		// Token: 0x17000910 RID: 2320
		// (get) Token: 0x06001C91 RID: 7313 RVA: 0x0004E671 File Offset: 0x0004C871
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int4 wxyw
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new int4(this.w, this.x, this.y, this.w);
			}
		}

		// Token: 0x17000911 RID: 2321
		// (get) Token: 0x06001C92 RID: 7314 RVA: 0x0004E690 File Offset: 0x0004C890
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int4 wxzx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new int4(this.w, this.x, this.z, this.x);
			}
		}

		// Token: 0x17000912 RID: 2322
		// (get) Token: 0x06001C93 RID: 7315 RVA: 0x0004E6AF File Offset: 0x0004C8AF
		// (set) Token: 0x06001C94 RID: 7316 RVA: 0x0004E6CE File Offset: 0x0004C8CE
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int4 wxzy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new int4(this.w, this.x, this.z, this.y);
			}
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			set
			{
				this.w = value.x;
				this.x = value.y;
				this.z = value.z;
				this.y = value.w;
			}
		}

		// Token: 0x17000913 RID: 2323
		// (get) Token: 0x06001C95 RID: 7317 RVA: 0x0004E700 File Offset: 0x0004C900
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int4 wxzz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new int4(this.w, this.x, this.z, this.z);
			}
		}

		// Token: 0x17000914 RID: 2324
		// (get) Token: 0x06001C96 RID: 7318 RVA: 0x0004E71F File Offset: 0x0004C91F
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int4 wxzw
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new int4(this.w, this.x, this.z, this.w);
			}
		}

		// Token: 0x17000915 RID: 2325
		// (get) Token: 0x06001C97 RID: 7319 RVA: 0x0004E73E File Offset: 0x0004C93E
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int4 wxwx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new int4(this.w, this.x, this.w, this.x);
			}
		}

		// Token: 0x17000916 RID: 2326
		// (get) Token: 0x06001C98 RID: 7320 RVA: 0x0004E75D File Offset: 0x0004C95D
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int4 wxwy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new int4(this.w, this.x, this.w, this.y);
			}
		}

		// Token: 0x17000917 RID: 2327
		// (get) Token: 0x06001C99 RID: 7321 RVA: 0x0004E77C File Offset: 0x0004C97C
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int4 wxwz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new int4(this.w, this.x, this.w, this.z);
			}
		}

		// Token: 0x17000918 RID: 2328
		// (get) Token: 0x06001C9A RID: 7322 RVA: 0x0004E79B File Offset: 0x0004C99B
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int4 wxww
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new int4(this.w, this.x, this.w, this.w);
			}
		}

		// Token: 0x17000919 RID: 2329
		// (get) Token: 0x06001C9B RID: 7323 RVA: 0x0004E7BA File Offset: 0x0004C9BA
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int4 wyxx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new int4(this.w, this.y, this.x, this.x);
			}
		}

		// Token: 0x1700091A RID: 2330
		// (get) Token: 0x06001C9C RID: 7324 RVA: 0x0004E7D9 File Offset: 0x0004C9D9
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int4 wyxy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new int4(this.w, this.y, this.x, this.y);
			}
		}

		// Token: 0x1700091B RID: 2331
		// (get) Token: 0x06001C9D RID: 7325 RVA: 0x0004E7F8 File Offset: 0x0004C9F8
		// (set) Token: 0x06001C9E RID: 7326 RVA: 0x0004E817 File Offset: 0x0004CA17
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int4 wyxz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new int4(this.w, this.y, this.x, this.z);
			}
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			set
			{
				this.w = value.x;
				this.y = value.y;
				this.x = value.z;
				this.z = value.w;
			}
		}

		// Token: 0x1700091C RID: 2332
		// (get) Token: 0x06001C9F RID: 7327 RVA: 0x0004E849 File Offset: 0x0004CA49
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int4 wyxw
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new int4(this.w, this.y, this.x, this.w);
			}
		}

		// Token: 0x1700091D RID: 2333
		// (get) Token: 0x06001CA0 RID: 7328 RVA: 0x0004E868 File Offset: 0x0004CA68
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int4 wyyx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new int4(this.w, this.y, this.y, this.x);
			}
		}

		// Token: 0x1700091E RID: 2334
		// (get) Token: 0x06001CA1 RID: 7329 RVA: 0x0004E887 File Offset: 0x0004CA87
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int4 wyyy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new int4(this.w, this.y, this.y, this.y);
			}
		}

		// Token: 0x1700091F RID: 2335
		// (get) Token: 0x06001CA2 RID: 7330 RVA: 0x0004E8A6 File Offset: 0x0004CAA6
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int4 wyyz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new int4(this.w, this.y, this.y, this.z);
			}
		}

		// Token: 0x17000920 RID: 2336
		// (get) Token: 0x06001CA3 RID: 7331 RVA: 0x0004E8C5 File Offset: 0x0004CAC5
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int4 wyyw
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new int4(this.w, this.y, this.y, this.w);
			}
		}

		// Token: 0x17000921 RID: 2337
		// (get) Token: 0x06001CA4 RID: 7332 RVA: 0x0004E8E4 File Offset: 0x0004CAE4
		// (set) Token: 0x06001CA5 RID: 7333 RVA: 0x0004E903 File Offset: 0x0004CB03
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int4 wyzx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new int4(this.w, this.y, this.z, this.x);
			}
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			set
			{
				this.w = value.x;
				this.y = value.y;
				this.z = value.z;
				this.x = value.w;
			}
		}

		// Token: 0x17000922 RID: 2338
		// (get) Token: 0x06001CA6 RID: 7334 RVA: 0x0004E935 File Offset: 0x0004CB35
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int4 wyzy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new int4(this.w, this.y, this.z, this.y);
			}
		}

		// Token: 0x17000923 RID: 2339
		// (get) Token: 0x06001CA7 RID: 7335 RVA: 0x0004E954 File Offset: 0x0004CB54
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int4 wyzz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new int4(this.w, this.y, this.z, this.z);
			}
		}

		// Token: 0x17000924 RID: 2340
		// (get) Token: 0x06001CA8 RID: 7336 RVA: 0x0004E973 File Offset: 0x0004CB73
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int4 wyzw
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new int4(this.w, this.y, this.z, this.w);
			}
		}

		// Token: 0x17000925 RID: 2341
		// (get) Token: 0x06001CA9 RID: 7337 RVA: 0x0004E992 File Offset: 0x0004CB92
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int4 wywx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new int4(this.w, this.y, this.w, this.x);
			}
		}

		// Token: 0x17000926 RID: 2342
		// (get) Token: 0x06001CAA RID: 7338 RVA: 0x0004E9B1 File Offset: 0x0004CBB1
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int4 wywy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new int4(this.w, this.y, this.w, this.y);
			}
		}

		// Token: 0x17000927 RID: 2343
		// (get) Token: 0x06001CAB RID: 7339 RVA: 0x0004E9D0 File Offset: 0x0004CBD0
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int4 wywz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new int4(this.w, this.y, this.w, this.z);
			}
		}

		// Token: 0x17000928 RID: 2344
		// (get) Token: 0x06001CAC RID: 7340 RVA: 0x0004E9EF File Offset: 0x0004CBEF
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int4 wyww
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new int4(this.w, this.y, this.w, this.w);
			}
		}

		// Token: 0x17000929 RID: 2345
		// (get) Token: 0x06001CAD RID: 7341 RVA: 0x0004EA0E File Offset: 0x0004CC0E
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int4 wzxx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new int4(this.w, this.z, this.x, this.x);
			}
		}

		// Token: 0x1700092A RID: 2346
		// (get) Token: 0x06001CAE RID: 7342 RVA: 0x0004EA2D File Offset: 0x0004CC2D
		// (set) Token: 0x06001CAF RID: 7343 RVA: 0x0004EA4C File Offset: 0x0004CC4C
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int4 wzxy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new int4(this.w, this.z, this.x, this.y);
			}
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			set
			{
				this.w = value.x;
				this.z = value.y;
				this.x = value.z;
				this.y = value.w;
			}
		}

		// Token: 0x1700092B RID: 2347
		// (get) Token: 0x06001CB0 RID: 7344 RVA: 0x0004EA7E File Offset: 0x0004CC7E
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int4 wzxz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new int4(this.w, this.z, this.x, this.z);
			}
		}

		// Token: 0x1700092C RID: 2348
		// (get) Token: 0x06001CB1 RID: 7345 RVA: 0x0004EA9D File Offset: 0x0004CC9D
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int4 wzxw
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new int4(this.w, this.z, this.x, this.w);
			}
		}

		// Token: 0x1700092D RID: 2349
		// (get) Token: 0x06001CB2 RID: 7346 RVA: 0x0004EABC File Offset: 0x0004CCBC
		// (set) Token: 0x06001CB3 RID: 7347 RVA: 0x0004EADB File Offset: 0x0004CCDB
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int4 wzyx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new int4(this.w, this.z, this.y, this.x);
			}
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			set
			{
				this.w = value.x;
				this.z = value.y;
				this.y = value.z;
				this.x = value.w;
			}
		}

		// Token: 0x1700092E RID: 2350
		// (get) Token: 0x06001CB4 RID: 7348 RVA: 0x0004EB0D File Offset: 0x0004CD0D
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int4 wzyy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new int4(this.w, this.z, this.y, this.y);
			}
		}

		// Token: 0x1700092F RID: 2351
		// (get) Token: 0x06001CB5 RID: 7349 RVA: 0x0004EB2C File Offset: 0x0004CD2C
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int4 wzyz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new int4(this.w, this.z, this.y, this.z);
			}
		}

		// Token: 0x17000930 RID: 2352
		// (get) Token: 0x06001CB6 RID: 7350 RVA: 0x0004EB4B File Offset: 0x0004CD4B
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int4 wzyw
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new int4(this.w, this.z, this.y, this.w);
			}
		}

		// Token: 0x17000931 RID: 2353
		// (get) Token: 0x06001CB7 RID: 7351 RVA: 0x0004EB6A File Offset: 0x0004CD6A
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int4 wzzx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new int4(this.w, this.z, this.z, this.x);
			}
		}

		// Token: 0x17000932 RID: 2354
		// (get) Token: 0x06001CB8 RID: 7352 RVA: 0x0004EB89 File Offset: 0x0004CD89
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int4 wzzy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new int4(this.w, this.z, this.z, this.y);
			}
		}

		// Token: 0x17000933 RID: 2355
		// (get) Token: 0x06001CB9 RID: 7353 RVA: 0x0004EBA8 File Offset: 0x0004CDA8
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int4 wzzz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new int4(this.w, this.z, this.z, this.z);
			}
		}

		// Token: 0x17000934 RID: 2356
		// (get) Token: 0x06001CBA RID: 7354 RVA: 0x0004EBC7 File Offset: 0x0004CDC7
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int4 wzzw
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new int4(this.w, this.z, this.z, this.w);
			}
		}

		// Token: 0x17000935 RID: 2357
		// (get) Token: 0x06001CBB RID: 7355 RVA: 0x0004EBE6 File Offset: 0x0004CDE6
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int4 wzwx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new int4(this.w, this.z, this.w, this.x);
			}
		}

		// Token: 0x17000936 RID: 2358
		// (get) Token: 0x06001CBC RID: 7356 RVA: 0x0004EC05 File Offset: 0x0004CE05
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int4 wzwy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new int4(this.w, this.z, this.w, this.y);
			}
		}

		// Token: 0x17000937 RID: 2359
		// (get) Token: 0x06001CBD RID: 7357 RVA: 0x0004EC24 File Offset: 0x0004CE24
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int4 wzwz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new int4(this.w, this.z, this.w, this.z);
			}
		}

		// Token: 0x17000938 RID: 2360
		// (get) Token: 0x06001CBE RID: 7358 RVA: 0x0004EC43 File Offset: 0x0004CE43
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int4 wzww
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new int4(this.w, this.z, this.w, this.w);
			}
		}

		// Token: 0x17000939 RID: 2361
		// (get) Token: 0x06001CBF RID: 7359 RVA: 0x0004EC62 File Offset: 0x0004CE62
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int4 wwxx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new int4(this.w, this.w, this.x, this.x);
			}
		}

		// Token: 0x1700093A RID: 2362
		// (get) Token: 0x06001CC0 RID: 7360 RVA: 0x0004EC81 File Offset: 0x0004CE81
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int4 wwxy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new int4(this.w, this.w, this.x, this.y);
			}
		}

		// Token: 0x1700093B RID: 2363
		// (get) Token: 0x06001CC1 RID: 7361 RVA: 0x0004ECA0 File Offset: 0x0004CEA0
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int4 wwxz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new int4(this.w, this.w, this.x, this.z);
			}
		}

		// Token: 0x1700093C RID: 2364
		// (get) Token: 0x06001CC2 RID: 7362 RVA: 0x0004ECBF File Offset: 0x0004CEBF
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int4 wwxw
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new int4(this.w, this.w, this.x, this.w);
			}
		}

		// Token: 0x1700093D RID: 2365
		// (get) Token: 0x06001CC3 RID: 7363 RVA: 0x0004ECDE File Offset: 0x0004CEDE
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int4 wwyx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new int4(this.w, this.w, this.y, this.x);
			}
		}

		// Token: 0x1700093E RID: 2366
		// (get) Token: 0x06001CC4 RID: 7364 RVA: 0x0004ECFD File Offset: 0x0004CEFD
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int4 wwyy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new int4(this.w, this.w, this.y, this.y);
			}
		}

		// Token: 0x1700093F RID: 2367
		// (get) Token: 0x06001CC5 RID: 7365 RVA: 0x0004ED1C File Offset: 0x0004CF1C
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int4 wwyz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new int4(this.w, this.w, this.y, this.z);
			}
		}

		// Token: 0x17000940 RID: 2368
		// (get) Token: 0x06001CC6 RID: 7366 RVA: 0x0004ED3B File Offset: 0x0004CF3B
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int4 wwyw
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new int4(this.w, this.w, this.y, this.w);
			}
		}

		// Token: 0x17000941 RID: 2369
		// (get) Token: 0x06001CC7 RID: 7367 RVA: 0x0004ED5A File Offset: 0x0004CF5A
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int4 wwzx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new int4(this.w, this.w, this.z, this.x);
			}
		}

		// Token: 0x17000942 RID: 2370
		// (get) Token: 0x06001CC8 RID: 7368 RVA: 0x0004ED79 File Offset: 0x0004CF79
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int4 wwzy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new int4(this.w, this.w, this.z, this.y);
			}
		}

		// Token: 0x17000943 RID: 2371
		// (get) Token: 0x06001CC9 RID: 7369 RVA: 0x0004ED98 File Offset: 0x0004CF98
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int4 wwzz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new int4(this.w, this.w, this.z, this.z);
			}
		}

		// Token: 0x17000944 RID: 2372
		// (get) Token: 0x06001CCA RID: 7370 RVA: 0x0004EDB7 File Offset: 0x0004CFB7
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int4 wwzw
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new int4(this.w, this.w, this.z, this.w);
			}
		}

		// Token: 0x17000945 RID: 2373
		// (get) Token: 0x06001CCB RID: 7371 RVA: 0x0004EDD6 File Offset: 0x0004CFD6
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int4 wwwx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new int4(this.w, this.w, this.w, this.x);
			}
		}

		// Token: 0x17000946 RID: 2374
		// (get) Token: 0x06001CCC RID: 7372 RVA: 0x0004EDF5 File Offset: 0x0004CFF5
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int4 wwwy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new int4(this.w, this.w, this.w, this.y);
			}
		}

		// Token: 0x17000947 RID: 2375
		// (get) Token: 0x06001CCD RID: 7373 RVA: 0x0004EE14 File Offset: 0x0004D014
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int4 wwwz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new int4(this.w, this.w, this.w, this.z);
			}
		}

		// Token: 0x17000948 RID: 2376
		// (get) Token: 0x06001CCE RID: 7374 RVA: 0x0004EE33 File Offset: 0x0004D033
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int4 wwww
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new int4(this.w, this.w, this.w, this.w);
			}
		}

		// Token: 0x17000949 RID: 2377
		// (get) Token: 0x06001CCF RID: 7375 RVA: 0x0004EE52 File Offset: 0x0004D052
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int3 xxx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new int3(this.x, this.x, this.x);
			}
		}

		// Token: 0x1700094A RID: 2378
		// (get) Token: 0x06001CD0 RID: 7376 RVA: 0x0004EE6B File Offset: 0x0004D06B
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int3 xxy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new int3(this.x, this.x, this.y);
			}
		}

		// Token: 0x1700094B RID: 2379
		// (get) Token: 0x06001CD1 RID: 7377 RVA: 0x0004EE84 File Offset: 0x0004D084
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int3 xxz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new int3(this.x, this.x, this.z);
			}
		}

		// Token: 0x1700094C RID: 2380
		// (get) Token: 0x06001CD2 RID: 7378 RVA: 0x0004EE9D File Offset: 0x0004D09D
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int3 xxw
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new int3(this.x, this.x, this.w);
			}
		}

		// Token: 0x1700094D RID: 2381
		// (get) Token: 0x06001CD3 RID: 7379 RVA: 0x0004EEB6 File Offset: 0x0004D0B6
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int3 xyx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new int3(this.x, this.y, this.x);
			}
		}

		// Token: 0x1700094E RID: 2382
		// (get) Token: 0x06001CD4 RID: 7380 RVA: 0x0004EECF File Offset: 0x0004D0CF
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int3 xyy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new int3(this.x, this.y, this.y);
			}
		}

		// Token: 0x1700094F RID: 2383
		// (get) Token: 0x06001CD5 RID: 7381 RVA: 0x0004EEE8 File Offset: 0x0004D0E8
		// (set) Token: 0x06001CD6 RID: 7382 RVA: 0x0004EF01 File Offset: 0x0004D101
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int3 xyz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new int3(this.x, this.y, this.z);
			}
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			set
			{
				this.x = value.x;
				this.y = value.y;
				this.z = value.z;
			}
		}

		// Token: 0x17000950 RID: 2384
		// (get) Token: 0x06001CD7 RID: 7383 RVA: 0x0004EF27 File Offset: 0x0004D127
		// (set) Token: 0x06001CD8 RID: 7384 RVA: 0x0004EF40 File Offset: 0x0004D140
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int3 xyw
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new int3(this.x, this.y, this.w);
			}
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			set
			{
				this.x = value.x;
				this.y = value.y;
				this.w = value.z;
			}
		}

		// Token: 0x17000951 RID: 2385
		// (get) Token: 0x06001CD9 RID: 7385 RVA: 0x0004EF66 File Offset: 0x0004D166
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int3 xzx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new int3(this.x, this.z, this.x);
			}
		}

		// Token: 0x17000952 RID: 2386
		// (get) Token: 0x06001CDA RID: 7386 RVA: 0x0004EF7F File Offset: 0x0004D17F
		// (set) Token: 0x06001CDB RID: 7387 RVA: 0x0004EF98 File Offset: 0x0004D198
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int3 xzy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new int3(this.x, this.z, this.y);
			}
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			set
			{
				this.x = value.x;
				this.z = value.y;
				this.y = value.z;
			}
		}

		// Token: 0x17000953 RID: 2387
		// (get) Token: 0x06001CDC RID: 7388 RVA: 0x0004EFBE File Offset: 0x0004D1BE
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int3 xzz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new int3(this.x, this.z, this.z);
			}
		}

		// Token: 0x17000954 RID: 2388
		// (get) Token: 0x06001CDD RID: 7389 RVA: 0x0004EFD7 File Offset: 0x0004D1D7
		// (set) Token: 0x06001CDE RID: 7390 RVA: 0x0004EFF0 File Offset: 0x0004D1F0
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int3 xzw
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new int3(this.x, this.z, this.w);
			}
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			set
			{
				this.x = value.x;
				this.z = value.y;
				this.w = value.z;
			}
		}

		// Token: 0x17000955 RID: 2389
		// (get) Token: 0x06001CDF RID: 7391 RVA: 0x0004F016 File Offset: 0x0004D216
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int3 xwx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new int3(this.x, this.w, this.x);
			}
		}

		// Token: 0x17000956 RID: 2390
		// (get) Token: 0x06001CE0 RID: 7392 RVA: 0x0004F02F File Offset: 0x0004D22F
		// (set) Token: 0x06001CE1 RID: 7393 RVA: 0x0004F048 File Offset: 0x0004D248
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int3 xwy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new int3(this.x, this.w, this.y);
			}
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			set
			{
				this.x = value.x;
				this.w = value.y;
				this.y = value.z;
			}
		}

		// Token: 0x17000957 RID: 2391
		// (get) Token: 0x06001CE2 RID: 7394 RVA: 0x0004F06E File Offset: 0x0004D26E
		// (set) Token: 0x06001CE3 RID: 7395 RVA: 0x0004F087 File Offset: 0x0004D287
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int3 xwz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new int3(this.x, this.w, this.z);
			}
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			set
			{
				this.x = value.x;
				this.w = value.y;
				this.z = value.z;
			}
		}

		// Token: 0x17000958 RID: 2392
		// (get) Token: 0x06001CE4 RID: 7396 RVA: 0x0004F0AD File Offset: 0x0004D2AD
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int3 xww
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new int3(this.x, this.w, this.w);
			}
		}

		// Token: 0x17000959 RID: 2393
		// (get) Token: 0x06001CE5 RID: 7397 RVA: 0x0004F0C6 File Offset: 0x0004D2C6
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int3 yxx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new int3(this.y, this.x, this.x);
			}
		}

		// Token: 0x1700095A RID: 2394
		// (get) Token: 0x06001CE6 RID: 7398 RVA: 0x0004F0DF File Offset: 0x0004D2DF
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int3 yxy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new int3(this.y, this.x, this.y);
			}
		}

		// Token: 0x1700095B RID: 2395
		// (get) Token: 0x06001CE7 RID: 7399 RVA: 0x0004F0F8 File Offset: 0x0004D2F8
		// (set) Token: 0x06001CE8 RID: 7400 RVA: 0x0004F111 File Offset: 0x0004D311
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int3 yxz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new int3(this.y, this.x, this.z);
			}
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			set
			{
				this.y = value.x;
				this.x = value.y;
				this.z = value.z;
			}
		}

		// Token: 0x1700095C RID: 2396
		// (get) Token: 0x06001CE9 RID: 7401 RVA: 0x0004F137 File Offset: 0x0004D337
		// (set) Token: 0x06001CEA RID: 7402 RVA: 0x0004F150 File Offset: 0x0004D350
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int3 yxw
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new int3(this.y, this.x, this.w);
			}
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			set
			{
				this.y = value.x;
				this.x = value.y;
				this.w = value.z;
			}
		}

		// Token: 0x1700095D RID: 2397
		// (get) Token: 0x06001CEB RID: 7403 RVA: 0x0004F176 File Offset: 0x0004D376
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int3 yyx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new int3(this.y, this.y, this.x);
			}
		}

		// Token: 0x1700095E RID: 2398
		// (get) Token: 0x06001CEC RID: 7404 RVA: 0x0004F18F File Offset: 0x0004D38F
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int3 yyy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new int3(this.y, this.y, this.y);
			}
		}

		// Token: 0x1700095F RID: 2399
		// (get) Token: 0x06001CED RID: 7405 RVA: 0x0004F1A8 File Offset: 0x0004D3A8
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int3 yyz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new int3(this.y, this.y, this.z);
			}
		}

		// Token: 0x17000960 RID: 2400
		// (get) Token: 0x06001CEE RID: 7406 RVA: 0x0004F1C1 File Offset: 0x0004D3C1
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int3 yyw
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new int3(this.y, this.y, this.w);
			}
		}

		// Token: 0x17000961 RID: 2401
		// (get) Token: 0x06001CEF RID: 7407 RVA: 0x0004F1DA File Offset: 0x0004D3DA
		// (set) Token: 0x06001CF0 RID: 7408 RVA: 0x0004F1F3 File Offset: 0x0004D3F3
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int3 yzx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new int3(this.y, this.z, this.x);
			}
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			set
			{
				this.y = value.x;
				this.z = value.y;
				this.x = value.z;
			}
		}

		// Token: 0x17000962 RID: 2402
		// (get) Token: 0x06001CF1 RID: 7409 RVA: 0x0004F219 File Offset: 0x0004D419
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int3 yzy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new int3(this.y, this.z, this.y);
			}
		}

		// Token: 0x17000963 RID: 2403
		// (get) Token: 0x06001CF2 RID: 7410 RVA: 0x0004F232 File Offset: 0x0004D432
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int3 yzz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new int3(this.y, this.z, this.z);
			}
		}

		// Token: 0x17000964 RID: 2404
		// (get) Token: 0x06001CF3 RID: 7411 RVA: 0x0004F24B File Offset: 0x0004D44B
		// (set) Token: 0x06001CF4 RID: 7412 RVA: 0x0004F264 File Offset: 0x0004D464
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int3 yzw
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new int3(this.y, this.z, this.w);
			}
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			set
			{
				this.y = value.x;
				this.z = value.y;
				this.w = value.z;
			}
		}

		// Token: 0x17000965 RID: 2405
		// (get) Token: 0x06001CF5 RID: 7413 RVA: 0x0004F28A File Offset: 0x0004D48A
		// (set) Token: 0x06001CF6 RID: 7414 RVA: 0x0004F2A3 File Offset: 0x0004D4A3
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int3 ywx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new int3(this.y, this.w, this.x);
			}
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			set
			{
				this.y = value.x;
				this.w = value.y;
				this.x = value.z;
			}
		}

		// Token: 0x17000966 RID: 2406
		// (get) Token: 0x06001CF7 RID: 7415 RVA: 0x0004F2C9 File Offset: 0x0004D4C9
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int3 ywy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new int3(this.y, this.w, this.y);
			}
		}

		// Token: 0x17000967 RID: 2407
		// (get) Token: 0x06001CF8 RID: 7416 RVA: 0x0004F2E2 File Offset: 0x0004D4E2
		// (set) Token: 0x06001CF9 RID: 7417 RVA: 0x0004F2FB File Offset: 0x0004D4FB
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int3 ywz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new int3(this.y, this.w, this.z);
			}
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			set
			{
				this.y = value.x;
				this.w = value.y;
				this.z = value.z;
			}
		}

		// Token: 0x17000968 RID: 2408
		// (get) Token: 0x06001CFA RID: 7418 RVA: 0x0004F321 File Offset: 0x0004D521
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int3 yww
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new int3(this.y, this.w, this.w);
			}
		}

		// Token: 0x17000969 RID: 2409
		// (get) Token: 0x06001CFB RID: 7419 RVA: 0x0004F33A File Offset: 0x0004D53A
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int3 zxx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new int3(this.z, this.x, this.x);
			}
		}

		// Token: 0x1700096A RID: 2410
		// (get) Token: 0x06001CFC RID: 7420 RVA: 0x0004F353 File Offset: 0x0004D553
		// (set) Token: 0x06001CFD RID: 7421 RVA: 0x0004F36C File Offset: 0x0004D56C
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int3 zxy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new int3(this.z, this.x, this.y);
			}
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			set
			{
				this.z = value.x;
				this.x = value.y;
				this.y = value.z;
			}
		}

		// Token: 0x1700096B RID: 2411
		// (get) Token: 0x06001CFE RID: 7422 RVA: 0x0004F392 File Offset: 0x0004D592
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int3 zxz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new int3(this.z, this.x, this.z);
			}
		}

		// Token: 0x1700096C RID: 2412
		// (get) Token: 0x06001CFF RID: 7423 RVA: 0x0004F3AB File Offset: 0x0004D5AB
		// (set) Token: 0x06001D00 RID: 7424 RVA: 0x0004F3C4 File Offset: 0x0004D5C4
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int3 zxw
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new int3(this.z, this.x, this.w);
			}
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			set
			{
				this.z = value.x;
				this.x = value.y;
				this.w = value.z;
			}
		}

		// Token: 0x1700096D RID: 2413
		// (get) Token: 0x06001D01 RID: 7425 RVA: 0x0004F3EA File Offset: 0x0004D5EA
		// (set) Token: 0x06001D02 RID: 7426 RVA: 0x0004F403 File Offset: 0x0004D603
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int3 zyx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new int3(this.z, this.y, this.x);
			}
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			set
			{
				this.z = value.x;
				this.y = value.y;
				this.x = value.z;
			}
		}

		// Token: 0x1700096E RID: 2414
		// (get) Token: 0x06001D03 RID: 7427 RVA: 0x0004F429 File Offset: 0x0004D629
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int3 zyy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new int3(this.z, this.y, this.y);
			}
		}

		// Token: 0x1700096F RID: 2415
		// (get) Token: 0x06001D04 RID: 7428 RVA: 0x0004F442 File Offset: 0x0004D642
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int3 zyz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new int3(this.z, this.y, this.z);
			}
		}

		// Token: 0x17000970 RID: 2416
		// (get) Token: 0x06001D05 RID: 7429 RVA: 0x0004F45B File Offset: 0x0004D65B
		// (set) Token: 0x06001D06 RID: 7430 RVA: 0x0004F474 File Offset: 0x0004D674
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int3 zyw
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new int3(this.z, this.y, this.w);
			}
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			set
			{
				this.z = value.x;
				this.y = value.y;
				this.w = value.z;
			}
		}

		// Token: 0x17000971 RID: 2417
		// (get) Token: 0x06001D07 RID: 7431 RVA: 0x0004F49A File Offset: 0x0004D69A
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int3 zzx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new int3(this.z, this.z, this.x);
			}
		}

		// Token: 0x17000972 RID: 2418
		// (get) Token: 0x06001D08 RID: 7432 RVA: 0x0004F4B3 File Offset: 0x0004D6B3
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int3 zzy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new int3(this.z, this.z, this.y);
			}
		}

		// Token: 0x17000973 RID: 2419
		// (get) Token: 0x06001D09 RID: 7433 RVA: 0x0004F4CC File Offset: 0x0004D6CC
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int3 zzz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new int3(this.z, this.z, this.z);
			}
		}

		// Token: 0x17000974 RID: 2420
		// (get) Token: 0x06001D0A RID: 7434 RVA: 0x0004F4E5 File Offset: 0x0004D6E5
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int3 zzw
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new int3(this.z, this.z, this.w);
			}
		}

		// Token: 0x17000975 RID: 2421
		// (get) Token: 0x06001D0B RID: 7435 RVA: 0x0004F4FE File Offset: 0x0004D6FE
		// (set) Token: 0x06001D0C RID: 7436 RVA: 0x0004F517 File Offset: 0x0004D717
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int3 zwx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new int3(this.z, this.w, this.x);
			}
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			set
			{
				this.z = value.x;
				this.w = value.y;
				this.x = value.z;
			}
		}

		// Token: 0x17000976 RID: 2422
		// (get) Token: 0x06001D0D RID: 7437 RVA: 0x0004F53D File Offset: 0x0004D73D
		// (set) Token: 0x06001D0E RID: 7438 RVA: 0x0004F556 File Offset: 0x0004D756
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int3 zwy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new int3(this.z, this.w, this.y);
			}
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			set
			{
				this.z = value.x;
				this.w = value.y;
				this.y = value.z;
			}
		}

		// Token: 0x17000977 RID: 2423
		// (get) Token: 0x06001D0F RID: 7439 RVA: 0x0004F57C File Offset: 0x0004D77C
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int3 zwz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new int3(this.z, this.w, this.z);
			}
		}

		// Token: 0x17000978 RID: 2424
		// (get) Token: 0x06001D10 RID: 7440 RVA: 0x0004F595 File Offset: 0x0004D795
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int3 zww
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new int3(this.z, this.w, this.w);
			}
		}

		// Token: 0x17000979 RID: 2425
		// (get) Token: 0x06001D11 RID: 7441 RVA: 0x0004F5AE File Offset: 0x0004D7AE
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int3 wxx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new int3(this.w, this.x, this.x);
			}
		}

		// Token: 0x1700097A RID: 2426
		// (get) Token: 0x06001D12 RID: 7442 RVA: 0x0004F5C7 File Offset: 0x0004D7C7
		// (set) Token: 0x06001D13 RID: 7443 RVA: 0x0004F5E0 File Offset: 0x0004D7E0
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int3 wxy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new int3(this.w, this.x, this.y);
			}
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			set
			{
				this.w = value.x;
				this.x = value.y;
				this.y = value.z;
			}
		}

		// Token: 0x1700097B RID: 2427
		// (get) Token: 0x06001D14 RID: 7444 RVA: 0x0004F606 File Offset: 0x0004D806
		// (set) Token: 0x06001D15 RID: 7445 RVA: 0x0004F61F File Offset: 0x0004D81F
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int3 wxz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new int3(this.w, this.x, this.z);
			}
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			set
			{
				this.w = value.x;
				this.x = value.y;
				this.z = value.z;
			}
		}

		// Token: 0x1700097C RID: 2428
		// (get) Token: 0x06001D16 RID: 7446 RVA: 0x0004F645 File Offset: 0x0004D845
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int3 wxw
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new int3(this.w, this.x, this.w);
			}
		}

		// Token: 0x1700097D RID: 2429
		// (get) Token: 0x06001D17 RID: 7447 RVA: 0x0004F65E File Offset: 0x0004D85E
		// (set) Token: 0x06001D18 RID: 7448 RVA: 0x0004F677 File Offset: 0x0004D877
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int3 wyx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new int3(this.w, this.y, this.x);
			}
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			set
			{
				this.w = value.x;
				this.y = value.y;
				this.x = value.z;
			}
		}

		// Token: 0x1700097E RID: 2430
		// (get) Token: 0x06001D19 RID: 7449 RVA: 0x0004F69D File Offset: 0x0004D89D
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int3 wyy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new int3(this.w, this.y, this.y);
			}
		}

		// Token: 0x1700097F RID: 2431
		// (get) Token: 0x06001D1A RID: 7450 RVA: 0x0004F6B6 File Offset: 0x0004D8B6
		// (set) Token: 0x06001D1B RID: 7451 RVA: 0x0004F6CF File Offset: 0x0004D8CF
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int3 wyz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new int3(this.w, this.y, this.z);
			}
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			set
			{
				this.w = value.x;
				this.y = value.y;
				this.z = value.z;
			}
		}

		// Token: 0x17000980 RID: 2432
		// (get) Token: 0x06001D1C RID: 7452 RVA: 0x0004F6F5 File Offset: 0x0004D8F5
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int3 wyw
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new int3(this.w, this.y, this.w);
			}
		}

		// Token: 0x17000981 RID: 2433
		// (get) Token: 0x06001D1D RID: 7453 RVA: 0x0004F70E File Offset: 0x0004D90E
		// (set) Token: 0x06001D1E RID: 7454 RVA: 0x0004F727 File Offset: 0x0004D927
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int3 wzx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new int3(this.w, this.z, this.x);
			}
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			set
			{
				this.w = value.x;
				this.z = value.y;
				this.x = value.z;
			}
		}

		// Token: 0x17000982 RID: 2434
		// (get) Token: 0x06001D1F RID: 7455 RVA: 0x0004F74D File Offset: 0x0004D94D
		// (set) Token: 0x06001D20 RID: 7456 RVA: 0x0004F766 File Offset: 0x0004D966
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int3 wzy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new int3(this.w, this.z, this.y);
			}
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			set
			{
				this.w = value.x;
				this.z = value.y;
				this.y = value.z;
			}
		}

		// Token: 0x17000983 RID: 2435
		// (get) Token: 0x06001D21 RID: 7457 RVA: 0x0004F78C File Offset: 0x0004D98C
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int3 wzz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new int3(this.w, this.z, this.z);
			}
		}

		// Token: 0x17000984 RID: 2436
		// (get) Token: 0x06001D22 RID: 7458 RVA: 0x0004F7A5 File Offset: 0x0004D9A5
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int3 wzw
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new int3(this.w, this.z, this.w);
			}
		}

		// Token: 0x17000985 RID: 2437
		// (get) Token: 0x06001D23 RID: 7459 RVA: 0x0004F7BE File Offset: 0x0004D9BE
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int3 wwx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new int3(this.w, this.w, this.x);
			}
		}

		// Token: 0x17000986 RID: 2438
		// (get) Token: 0x06001D24 RID: 7460 RVA: 0x0004F7D7 File Offset: 0x0004D9D7
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int3 wwy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new int3(this.w, this.w, this.y);
			}
		}

		// Token: 0x17000987 RID: 2439
		// (get) Token: 0x06001D25 RID: 7461 RVA: 0x0004F7F0 File Offset: 0x0004D9F0
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int3 wwz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new int3(this.w, this.w, this.z);
			}
		}

		// Token: 0x17000988 RID: 2440
		// (get) Token: 0x06001D26 RID: 7462 RVA: 0x0004F809 File Offset: 0x0004DA09
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int3 www
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new int3(this.w, this.w, this.w);
			}
		}

		// Token: 0x17000989 RID: 2441
		// (get) Token: 0x06001D27 RID: 7463 RVA: 0x0004F822 File Offset: 0x0004DA22
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int2 xx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new int2(this.x, this.x);
			}
		}

		// Token: 0x1700098A RID: 2442
		// (get) Token: 0x06001D28 RID: 7464 RVA: 0x0004F835 File Offset: 0x0004DA35
		// (set) Token: 0x06001D29 RID: 7465 RVA: 0x0004F848 File Offset: 0x0004DA48
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int2 xy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new int2(this.x, this.y);
			}
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			set
			{
				this.x = value.x;
				this.y = value.y;
			}
		}

		// Token: 0x1700098B RID: 2443
		// (get) Token: 0x06001D2A RID: 7466 RVA: 0x0004F862 File Offset: 0x0004DA62
		// (set) Token: 0x06001D2B RID: 7467 RVA: 0x0004F875 File Offset: 0x0004DA75
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int2 xz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new int2(this.x, this.z);
			}
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			set
			{
				this.x = value.x;
				this.z = value.y;
			}
		}

		// Token: 0x1700098C RID: 2444
		// (get) Token: 0x06001D2C RID: 7468 RVA: 0x0004F88F File Offset: 0x0004DA8F
		// (set) Token: 0x06001D2D RID: 7469 RVA: 0x0004F8A2 File Offset: 0x0004DAA2
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int2 xw
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new int2(this.x, this.w);
			}
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			set
			{
				this.x = value.x;
				this.w = value.y;
			}
		}

		// Token: 0x1700098D RID: 2445
		// (get) Token: 0x06001D2E RID: 7470 RVA: 0x0004F8BC File Offset: 0x0004DABC
		// (set) Token: 0x06001D2F RID: 7471 RVA: 0x0004F8CF File Offset: 0x0004DACF
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int2 yx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new int2(this.y, this.x);
			}
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			set
			{
				this.y = value.x;
				this.x = value.y;
			}
		}

		// Token: 0x1700098E RID: 2446
		// (get) Token: 0x06001D30 RID: 7472 RVA: 0x0004F8E9 File Offset: 0x0004DAE9
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int2 yy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new int2(this.y, this.y);
			}
		}

		// Token: 0x1700098F RID: 2447
		// (get) Token: 0x06001D31 RID: 7473 RVA: 0x0004F8FC File Offset: 0x0004DAFC
		// (set) Token: 0x06001D32 RID: 7474 RVA: 0x0004F90F File Offset: 0x0004DB0F
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int2 yz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new int2(this.y, this.z);
			}
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			set
			{
				this.y = value.x;
				this.z = value.y;
			}
		}

		// Token: 0x17000990 RID: 2448
		// (get) Token: 0x06001D33 RID: 7475 RVA: 0x0004F929 File Offset: 0x0004DB29
		// (set) Token: 0x06001D34 RID: 7476 RVA: 0x0004F93C File Offset: 0x0004DB3C
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int2 yw
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new int2(this.y, this.w);
			}
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			set
			{
				this.y = value.x;
				this.w = value.y;
			}
		}

		// Token: 0x17000991 RID: 2449
		// (get) Token: 0x06001D35 RID: 7477 RVA: 0x0004F956 File Offset: 0x0004DB56
		// (set) Token: 0x06001D36 RID: 7478 RVA: 0x0004F969 File Offset: 0x0004DB69
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int2 zx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new int2(this.z, this.x);
			}
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			set
			{
				this.z = value.x;
				this.x = value.y;
			}
		}

		// Token: 0x17000992 RID: 2450
		// (get) Token: 0x06001D37 RID: 7479 RVA: 0x0004F983 File Offset: 0x0004DB83
		// (set) Token: 0x06001D38 RID: 7480 RVA: 0x0004F996 File Offset: 0x0004DB96
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int2 zy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new int2(this.z, this.y);
			}
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			set
			{
				this.z = value.x;
				this.y = value.y;
			}
		}

		// Token: 0x17000993 RID: 2451
		// (get) Token: 0x06001D39 RID: 7481 RVA: 0x0004F9B0 File Offset: 0x0004DBB0
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int2 zz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new int2(this.z, this.z);
			}
		}

		// Token: 0x17000994 RID: 2452
		// (get) Token: 0x06001D3A RID: 7482 RVA: 0x0004F9C3 File Offset: 0x0004DBC3
		// (set) Token: 0x06001D3B RID: 7483 RVA: 0x0004F9D6 File Offset: 0x0004DBD6
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int2 zw
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new int2(this.z, this.w);
			}
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			set
			{
				this.z = value.x;
				this.w = value.y;
			}
		}

		// Token: 0x17000995 RID: 2453
		// (get) Token: 0x06001D3C RID: 7484 RVA: 0x0004F9F0 File Offset: 0x0004DBF0
		// (set) Token: 0x06001D3D RID: 7485 RVA: 0x0004FA03 File Offset: 0x0004DC03
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int2 wx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new int2(this.w, this.x);
			}
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			set
			{
				this.w = value.x;
				this.x = value.y;
			}
		}

		// Token: 0x17000996 RID: 2454
		// (get) Token: 0x06001D3E RID: 7486 RVA: 0x0004FA1D File Offset: 0x0004DC1D
		// (set) Token: 0x06001D3F RID: 7487 RVA: 0x0004FA30 File Offset: 0x0004DC30
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int2 wy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new int2(this.w, this.y);
			}
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			set
			{
				this.w = value.x;
				this.y = value.y;
			}
		}

		// Token: 0x17000997 RID: 2455
		// (get) Token: 0x06001D40 RID: 7488 RVA: 0x0004FA4A File Offset: 0x0004DC4A
		// (set) Token: 0x06001D41 RID: 7489 RVA: 0x0004FA5D File Offset: 0x0004DC5D
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int2 wz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new int2(this.w, this.z);
			}
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			set
			{
				this.w = value.x;
				this.z = value.y;
			}
		}

		// Token: 0x17000998 RID: 2456
		// (get) Token: 0x06001D42 RID: 7490 RVA: 0x0004FA77 File Offset: 0x0004DC77
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int2 ww
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new int2(this.w, this.w);
			}
		}

		// Token: 0x17000999 RID: 2457
		public unsafe int this[int index]
		{
			get
			{
				fixed (int4* ptr = &this)
				{
					return ((int*)ptr)[index];
				}
			}
			set
			{
				fixed (int* ptr = &this.x)
				{
					ptr[index] = value;
				}
			}
		}

		// Token: 0x06001D45 RID: 7493 RVA: 0x0004FAC4 File Offset: 0x0004DCC4
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool Equals(int4 rhs)
		{
			return this.x == rhs.x && this.y == rhs.y && this.z == rhs.z && this.w == rhs.w;
		}

		// Token: 0x06001D46 RID: 7494 RVA: 0x0004FB00 File Offset: 0x0004DD00
		public override bool Equals(object o)
		{
			if (o is int4)
			{
				int4 @int = (int4)o;
				return this.Equals(@int);
			}
			return false;
		}

		// Token: 0x06001D47 RID: 7495 RVA: 0x0004FB25 File Offset: 0x0004DD25
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public override int GetHashCode()
		{
			return (int)math.hash(this);
		}

		// Token: 0x06001D48 RID: 7496 RVA: 0x0004FB34 File Offset: 0x0004DD34
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public override string ToString()
		{
			return string.Format("int4({0}, {1}, {2}, {3})", new object[] { this.x, this.y, this.z, this.w });
		}

		// Token: 0x06001D49 RID: 7497 RVA: 0x0004FB8C File Offset: 0x0004DD8C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public string ToString(string format, IFormatProvider formatProvider)
		{
			return string.Format("int4({0}, {1}, {2}, {3})", new object[]
			{
				this.x.ToString(format, formatProvider),
				this.y.ToString(format, formatProvider),
				this.z.ToString(format, formatProvider),
				this.w.ToString(format, formatProvider)
			});
		}

		// Token: 0x040000DE RID: 222
		public int x;

		// Token: 0x040000DF RID: 223
		public int y;

		// Token: 0x040000E0 RID: 224
		public int z;

		// Token: 0x040000E1 RID: 225
		public int w;

		// Token: 0x040000E2 RID: 226
		public static readonly int4 zero;

		// Token: 0x02000063 RID: 99
		internal sealed class DebuggerProxy
		{
			// Token: 0x060024D0 RID: 9424 RVA: 0x00069358 File Offset: 0x00067558
			public DebuggerProxy(int4 v)
			{
				this.x = v.x;
				this.y = v.y;
				this.z = v.z;
				this.w = v.w;
			}

			// Token: 0x04000173 RID: 371
			public int x;

			// Token: 0x04000174 RID: 372
			public int y;

			// Token: 0x04000175 RID: 373
			public int z;

			// Token: 0x04000176 RID: 374
			public int w;
		}
	}
}
