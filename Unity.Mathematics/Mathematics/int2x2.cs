using System;
using System.Runtime.CompilerServices;
using Unity.IL2CPP.CompilerServices;

namespace Unity.Mathematics
{
	// Token: 0x0200002F RID: 47
	[Il2CppEagerStaticClassConstruction]
	[Serializable]
	public struct int2x2 : IEquatable<int2x2>, IFormattable
	{
		// Token: 0x060018D9 RID: 6361 RVA: 0x00044F46 File Offset: 0x00043146
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int2x2(int2 c0, int2 c1)
		{
			this.c0 = c0;
			this.c1 = c1;
		}

		// Token: 0x060018DA RID: 6362 RVA: 0x00044F56 File Offset: 0x00043156
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int2x2(int m00, int m01, int m10, int m11)
		{
			this.c0 = new int2(m00, m10);
			this.c1 = new int2(m01, m11);
		}

		// Token: 0x060018DB RID: 6363 RVA: 0x00044F73 File Offset: 0x00043173
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int2x2(int v)
		{
			this.c0 = v;
			this.c1 = v;
		}

		// Token: 0x060018DC RID: 6364 RVA: 0x00044F8D File Offset: 0x0004318D
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int2x2(bool v)
		{
			this.c0 = math.select(new int2(0), new int2(1), v);
			this.c1 = math.select(new int2(0), new int2(1), v);
		}

		// Token: 0x060018DD RID: 6365 RVA: 0x00044FBF File Offset: 0x000431BF
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int2x2(bool2x2 v)
		{
			this.c0 = math.select(new int2(0), new int2(1), v.c0);
			this.c1 = math.select(new int2(0), new int2(1), v.c1);
		}

		// Token: 0x060018DE RID: 6366 RVA: 0x00044FFB File Offset: 0x000431FB
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int2x2(uint v)
		{
			this.c0 = (int2)v;
			this.c1 = (int2)v;
		}

		// Token: 0x060018DF RID: 6367 RVA: 0x00045015 File Offset: 0x00043215
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int2x2(uint2x2 v)
		{
			this.c0 = (int2)v.c0;
			this.c1 = (int2)v.c1;
		}

		// Token: 0x060018E0 RID: 6368 RVA: 0x00045039 File Offset: 0x00043239
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int2x2(float v)
		{
			this.c0 = (int2)v;
			this.c1 = (int2)v;
		}

		// Token: 0x060018E1 RID: 6369 RVA: 0x00045053 File Offset: 0x00043253
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int2x2(float2x2 v)
		{
			this.c0 = (int2)v.c0;
			this.c1 = (int2)v.c1;
		}

		// Token: 0x060018E2 RID: 6370 RVA: 0x00045077 File Offset: 0x00043277
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int2x2(double v)
		{
			this.c0 = (int2)v;
			this.c1 = (int2)v;
		}

		// Token: 0x060018E3 RID: 6371 RVA: 0x00045091 File Offset: 0x00043291
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int2x2(double2x2 v)
		{
			this.c0 = (int2)v.c0;
			this.c1 = (int2)v.c1;
		}

		// Token: 0x060018E4 RID: 6372 RVA: 0x000450B5 File Offset: 0x000432B5
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static implicit operator int2x2(int v)
		{
			return new int2x2(v);
		}

		// Token: 0x060018E5 RID: 6373 RVA: 0x000450BD File Offset: 0x000432BD
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static explicit operator int2x2(bool v)
		{
			return new int2x2(v);
		}

		// Token: 0x060018E6 RID: 6374 RVA: 0x000450C5 File Offset: 0x000432C5
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static explicit operator int2x2(bool2x2 v)
		{
			return new int2x2(v);
		}

		// Token: 0x060018E7 RID: 6375 RVA: 0x000450CD File Offset: 0x000432CD
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static explicit operator int2x2(uint v)
		{
			return new int2x2(v);
		}

		// Token: 0x060018E8 RID: 6376 RVA: 0x000450D5 File Offset: 0x000432D5
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static explicit operator int2x2(uint2x2 v)
		{
			return new int2x2(v);
		}

		// Token: 0x060018E9 RID: 6377 RVA: 0x000450DD File Offset: 0x000432DD
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static explicit operator int2x2(float v)
		{
			return new int2x2(v);
		}

		// Token: 0x060018EA RID: 6378 RVA: 0x000450E5 File Offset: 0x000432E5
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static explicit operator int2x2(float2x2 v)
		{
			return new int2x2(v);
		}

		// Token: 0x060018EB RID: 6379 RVA: 0x000450ED File Offset: 0x000432ED
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static explicit operator int2x2(double v)
		{
			return new int2x2(v);
		}

		// Token: 0x060018EC RID: 6380 RVA: 0x000450F5 File Offset: 0x000432F5
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static explicit operator int2x2(double2x2 v)
		{
			return new int2x2(v);
		}

		// Token: 0x060018ED RID: 6381 RVA: 0x000450FD File Offset: 0x000432FD
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int2x2 operator *(int2x2 lhs, int2x2 rhs)
		{
			return new int2x2(lhs.c0 * rhs.c0, lhs.c1 * rhs.c1);
		}

		// Token: 0x060018EE RID: 6382 RVA: 0x00045126 File Offset: 0x00043326
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int2x2 operator *(int2x2 lhs, int rhs)
		{
			return new int2x2(lhs.c0 * rhs, lhs.c1 * rhs);
		}

		// Token: 0x060018EF RID: 6383 RVA: 0x00045145 File Offset: 0x00043345
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int2x2 operator *(int lhs, int2x2 rhs)
		{
			return new int2x2(lhs * rhs.c0, lhs * rhs.c1);
		}

		// Token: 0x060018F0 RID: 6384 RVA: 0x00045164 File Offset: 0x00043364
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int2x2 operator +(int2x2 lhs, int2x2 rhs)
		{
			return new int2x2(lhs.c0 + rhs.c0, lhs.c1 + rhs.c1);
		}

		// Token: 0x060018F1 RID: 6385 RVA: 0x0004518D File Offset: 0x0004338D
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int2x2 operator +(int2x2 lhs, int rhs)
		{
			return new int2x2(lhs.c0 + rhs, lhs.c1 + rhs);
		}

		// Token: 0x060018F2 RID: 6386 RVA: 0x000451AC File Offset: 0x000433AC
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int2x2 operator +(int lhs, int2x2 rhs)
		{
			return new int2x2(lhs + rhs.c0, lhs + rhs.c1);
		}

		// Token: 0x060018F3 RID: 6387 RVA: 0x000451CB File Offset: 0x000433CB
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int2x2 operator -(int2x2 lhs, int2x2 rhs)
		{
			return new int2x2(lhs.c0 - rhs.c0, lhs.c1 - rhs.c1);
		}

		// Token: 0x060018F4 RID: 6388 RVA: 0x000451F4 File Offset: 0x000433F4
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int2x2 operator -(int2x2 lhs, int rhs)
		{
			return new int2x2(lhs.c0 - rhs, lhs.c1 - rhs);
		}

		// Token: 0x060018F5 RID: 6389 RVA: 0x00045213 File Offset: 0x00043413
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int2x2 operator -(int lhs, int2x2 rhs)
		{
			return new int2x2(lhs - rhs.c0, lhs - rhs.c1);
		}

		// Token: 0x060018F6 RID: 6390 RVA: 0x00045232 File Offset: 0x00043432
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int2x2 operator /(int2x2 lhs, int2x2 rhs)
		{
			return new int2x2(lhs.c0 / rhs.c0, lhs.c1 / rhs.c1);
		}

		// Token: 0x060018F7 RID: 6391 RVA: 0x0004525B File Offset: 0x0004345B
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int2x2 operator /(int2x2 lhs, int rhs)
		{
			return new int2x2(lhs.c0 / rhs, lhs.c1 / rhs);
		}

		// Token: 0x060018F8 RID: 6392 RVA: 0x0004527A File Offset: 0x0004347A
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int2x2 operator /(int lhs, int2x2 rhs)
		{
			return new int2x2(lhs / rhs.c0, lhs / rhs.c1);
		}

		// Token: 0x060018F9 RID: 6393 RVA: 0x00045299 File Offset: 0x00043499
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int2x2 operator %(int2x2 lhs, int2x2 rhs)
		{
			return new int2x2(lhs.c0 % rhs.c0, lhs.c1 % rhs.c1);
		}

		// Token: 0x060018FA RID: 6394 RVA: 0x000452C2 File Offset: 0x000434C2
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int2x2 operator %(int2x2 lhs, int rhs)
		{
			return new int2x2(lhs.c0 % rhs, lhs.c1 % rhs);
		}

		// Token: 0x060018FB RID: 6395 RVA: 0x000452E1 File Offset: 0x000434E1
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int2x2 operator %(int lhs, int2x2 rhs)
		{
			return new int2x2(lhs % rhs.c0, lhs % rhs.c1);
		}

		// Token: 0x060018FC RID: 6396 RVA: 0x00045300 File Offset: 0x00043500
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int2x2 operator ++(int2x2 val)
		{
			int2 @int = int2.op_Increment(val.c0);
			val.c0 = @int;
			int2 int2 = @int;
			@int = int2.op_Increment(val.c1);
			val.c1 = @int;
			return new int2x2(int2, @int);
		}

		// Token: 0x060018FD RID: 6397 RVA: 0x00045348 File Offset: 0x00043548
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int2x2 operator --(int2x2 val)
		{
			int2 @int = int2.op_Decrement(val.c0);
			val.c0 = @int;
			int2 int2 = @int;
			@int = int2.op_Decrement(val.c1);
			val.c1 = @int;
			return new int2x2(int2, @int);
		}

		// Token: 0x060018FE RID: 6398 RVA: 0x0004538E File Offset: 0x0004358E
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool2x2 operator <(int2x2 lhs, int2x2 rhs)
		{
			return new bool2x2(lhs.c0 < rhs.c0, lhs.c1 < rhs.c1);
		}

		// Token: 0x060018FF RID: 6399 RVA: 0x000453B7 File Offset: 0x000435B7
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool2x2 operator <(int2x2 lhs, int rhs)
		{
			return new bool2x2(lhs.c0 < rhs, lhs.c1 < rhs);
		}

		// Token: 0x06001900 RID: 6400 RVA: 0x000453D6 File Offset: 0x000435D6
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool2x2 operator <(int lhs, int2x2 rhs)
		{
			return new bool2x2(lhs < rhs.c0, lhs < rhs.c1);
		}

		// Token: 0x06001901 RID: 6401 RVA: 0x000453F5 File Offset: 0x000435F5
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool2x2 operator <=(int2x2 lhs, int2x2 rhs)
		{
			return new bool2x2(lhs.c0 <= rhs.c0, lhs.c1 <= rhs.c1);
		}

		// Token: 0x06001902 RID: 6402 RVA: 0x0004541E File Offset: 0x0004361E
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool2x2 operator <=(int2x2 lhs, int rhs)
		{
			return new bool2x2(lhs.c0 <= rhs, lhs.c1 <= rhs);
		}

		// Token: 0x06001903 RID: 6403 RVA: 0x0004543D File Offset: 0x0004363D
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool2x2 operator <=(int lhs, int2x2 rhs)
		{
			return new bool2x2(lhs <= rhs.c0, lhs <= rhs.c1);
		}

		// Token: 0x06001904 RID: 6404 RVA: 0x0004545C File Offset: 0x0004365C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool2x2 operator >(int2x2 lhs, int2x2 rhs)
		{
			return new bool2x2(lhs.c0 > rhs.c0, lhs.c1 > rhs.c1);
		}

		// Token: 0x06001905 RID: 6405 RVA: 0x00045485 File Offset: 0x00043685
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool2x2 operator >(int2x2 lhs, int rhs)
		{
			return new bool2x2(lhs.c0 > rhs, lhs.c1 > rhs);
		}

		// Token: 0x06001906 RID: 6406 RVA: 0x000454A4 File Offset: 0x000436A4
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool2x2 operator >(int lhs, int2x2 rhs)
		{
			return new bool2x2(lhs > rhs.c0, lhs > rhs.c1);
		}

		// Token: 0x06001907 RID: 6407 RVA: 0x000454C3 File Offset: 0x000436C3
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool2x2 operator >=(int2x2 lhs, int2x2 rhs)
		{
			return new bool2x2(lhs.c0 >= rhs.c0, lhs.c1 >= rhs.c1);
		}

		// Token: 0x06001908 RID: 6408 RVA: 0x000454EC File Offset: 0x000436EC
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool2x2 operator >=(int2x2 lhs, int rhs)
		{
			return new bool2x2(lhs.c0 >= rhs, lhs.c1 >= rhs);
		}

		// Token: 0x06001909 RID: 6409 RVA: 0x0004550B File Offset: 0x0004370B
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool2x2 operator >=(int lhs, int2x2 rhs)
		{
			return new bool2x2(lhs >= rhs.c0, lhs >= rhs.c1);
		}

		// Token: 0x0600190A RID: 6410 RVA: 0x0004552A File Offset: 0x0004372A
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int2x2 operator -(int2x2 val)
		{
			return new int2x2(-val.c0, -val.c1);
		}

		// Token: 0x0600190B RID: 6411 RVA: 0x00045547 File Offset: 0x00043747
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int2x2 operator +(int2x2 val)
		{
			return new int2x2(+val.c0, +val.c1);
		}

		// Token: 0x0600190C RID: 6412 RVA: 0x00045564 File Offset: 0x00043764
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int2x2 operator <<(int2x2 x, int n)
		{
			return new int2x2(x.c0 << n, x.c1 << n);
		}

		// Token: 0x0600190D RID: 6413 RVA: 0x00045583 File Offset: 0x00043783
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int2x2 operator >>(int2x2 x, int n)
		{
			return new int2x2(x.c0 >> n, x.c1 >> n);
		}

		// Token: 0x0600190E RID: 6414 RVA: 0x000455A2 File Offset: 0x000437A2
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool2x2 operator ==(int2x2 lhs, int2x2 rhs)
		{
			return new bool2x2(lhs.c0 == rhs.c0, lhs.c1 == rhs.c1);
		}

		// Token: 0x0600190F RID: 6415 RVA: 0x000455CB File Offset: 0x000437CB
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool2x2 operator ==(int2x2 lhs, int rhs)
		{
			return new bool2x2(lhs.c0 == rhs, lhs.c1 == rhs);
		}

		// Token: 0x06001910 RID: 6416 RVA: 0x000455EA File Offset: 0x000437EA
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool2x2 operator ==(int lhs, int2x2 rhs)
		{
			return new bool2x2(lhs == rhs.c0, lhs == rhs.c1);
		}

		// Token: 0x06001911 RID: 6417 RVA: 0x00045609 File Offset: 0x00043809
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool2x2 operator !=(int2x2 lhs, int2x2 rhs)
		{
			return new bool2x2(lhs.c0 != rhs.c0, lhs.c1 != rhs.c1);
		}

		// Token: 0x06001912 RID: 6418 RVA: 0x00045632 File Offset: 0x00043832
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool2x2 operator !=(int2x2 lhs, int rhs)
		{
			return new bool2x2(lhs.c0 != rhs, lhs.c1 != rhs);
		}

		// Token: 0x06001913 RID: 6419 RVA: 0x00045651 File Offset: 0x00043851
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool2x2 operator !=(int lhs, int2x2 rhs)
		{
			return new bool2x2(lhs != rhs.c0, lhs != rhs.c1);
		}

		// Token: 0x06001914 RID: 6420 RVA: 0x00045670 File Offset: 0x00043870
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int2x2 operator ~(int2x2 val)
		{
			return new int2x2(~val.c0, ~val.c1);
		}

		// Token: 0x06001915 RID: 6421 RVA: 0x0004568D File Offset: 0x0004388D
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int2x2 operator &(int2x2 lhs, int2x2 rhs)
		{
			return new int2x2(lhs.c0 & rhs.c0, lhs.c1 & rhs.c1);
		}

		// Token: 0x06001916 RID: 6422 RVA: 0x000456B6 File Offset: 0x000438B6
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int2x2 operator &(int2x2 lhs, int rhs)
		{
			return new int2x2(lhs.c0 & rhs, lhs.c1 & rhs);
		}

		// Token: 0x06001917 RID: 6423 RVA: 0x000456D5 File Offset: 0x000438D5
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int2x2 operator &(int lhs, int2x2 rhs)
		{
			return new int2x2(lhs & rhs.c0, lhs & rhs.c1);
		}

		// Token: 0x06001918 RID: 6424 RVA: 0x000456F4 File Offset: 0x000438F4
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int2x2 operator |(int2x2 lhs, int2x2 rhs)
		{
			return new int2x2(lhs.c0 | rhs.c0, lhs.c1 | rhs.c1);
		}

		// Token: 0x06001919 RID: 6425 RVA: 0x0004571D File Offset: 0x0004391D
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int2x2 operator |(int2x2 lhs, int rhs)
		{
			return new int2x2(lhs.c0 | rhs, lhs.c1 | rhs);
		}

		// Token: 0x0600191A RID: 6426 RVA: 0x0004573C File Offset: 0x0004393C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int2x2 operator |(int lhs, int2x2 rhs)
		{
			return new int2x2(lhs | rhs.c0, lhs | rhs.c1);
		}

		// Token: 0x0600191B RID: 6427 RVA: 0x0004575B File Offset: 0x0004395B
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int2x2 operator ^(int2x2 lhs, int2x2 rhs)
		{
			return new int2x2(lhs.c0 ^ rhs.c0, lhs.c1 ^ rhs.c1);
		}

		// Token: 0x0600191C RID: 6428 RVA: 0x00045784 File Offset: 0x00043984
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int2x2 operator ^(int2x2 lhs, int rhs)
		{
			return new int2x2(lhs.c0 ^ rhs, lhs.c1 ^ rhs);
		}

		// Token: 0x0600191D RID: 6429 RVA: 0x000457A3 File Offset: 0x000439A3
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int2x2 operator ^(int lhs, int2x2 rhs)
		{
			return new int2x2(lhs ^ rhs.c0, lhs ^ rhs.c1);
		}

		// Token: 0x170007CD RID: 1997
		public unsafe ref int2 this[int index]
		{
			get
			{
				fixed (int2x2* ptr = &this)
				{
					return ref *(int2*)(ptr + (IntPtr)index * (IntPtr)sizeof(int2) / (IntPtr)sizeof(int2x2));
				}
			}
		}

		// Token: 0x0600191F RID: 6431 RVA: 0x000457DF File Offset: 0x000439DF
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool Equals(int2x2 rhs)
		{
			return this.c0.Equals(rhs.c0) && this.c1.Equals(rhs.c1);
		}

		// Token: 0x06001920 RID: 6432 RVA: 0x00045808 File Offset: 0x00043A08
		public override bool Equals(object o)
		{
			if (o is int2x2)
			{
				int2x2 int2x = (int2x2)o;
				return this.Equals(int2x);
			}
			return false;
		}

		// Token: 0x06001921 RID: 6433 RVA: 0x0004582D File Offset: 0x00043A2D
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public override int GetHashCode()
		{
			return (int)math.hash(this);
		}

		// Token: 0x06001922 RID: 6434 RVA: 0x0004583C File Offset: 0x00043A3C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public override string ToString()
		{
			return string.Format("int2x2({0}, {1},  {2}, {3})", new object[]
			{
				this.c0.x,
				this.c1.x,
				this.c0.y,
				this.c1.y
			});
		}

		// Token: 0x06001923 RID: 6435 RVA: 0x000458A8 File Offset: 0x00043AA8
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public string ToString(string format, IFormatProvider formatProvider)
		{
			return string.Format("int2x2({0}, {1},  {2}, {3})", new object[]
			{
				this.c0.x.ToString(format, formatProvider),
				this.c1.x.ToString(format, formatProvider),
				this.c0.y.ToString(format, formatProvider),
				this.c1.y.ToString(format, formatProvider)
			});
		}

		// Token: 0x040000C0 RID: 192
		public int2 c0;

		// Token: 0x040000C1 RID: 193
		public int2 c1;

		// Token: 0x040000C2 RID: 194
		public static readonly int2x2 identity = new int2x2(1, 0, 0, 1);

		// Token: 0x040000C3 RID: 195
		public static readonly int2x2 zero;
	}
}
