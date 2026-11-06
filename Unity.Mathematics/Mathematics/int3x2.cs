using System;
using System.Runtime.CompilerServices;
using Unity.IL2CPP.CompilerServices;

namespace Unity.Mathematics
{
	// Token: 0x02000033 RID: 51
	[Il2CppEagerStaticClassConstruction]
	[Serializable]
	public struct int3x2 : IEquatable<int3x2>, IFormattable
	{
		// Token: 0x06001A8A RID: 6794 RVA: 0x00049232 File Offset: 0x00047432
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int3x2(int3 c0, int3 c1)
		{
			this.c0 = c0;
			this.c1 = c1;
		}

		// Token: 0x06001A8B RID: 6795 RVA: 0x00049242 File Offset: 0x00047442
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int3x2(int m00, int m01, int m10, int m11, int m20, int m21)
		{
			this.c0 = new int3(m00, m10, m20);
			this.c1 = new int3(m01, m11, m21);
		}

		// Token: 0x06001A8C RID: 6796 RVA: 0x00049263 File Offset: 0x00047463
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int3x2(int v)
		{
			this.c0 = v;
			this.c1 = v;
		}

		// Token: 0x06001A8D RID: 6797 RVA: 0x0004927D File Offset: 0x0004747D
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int3x2(bool v)
		{
			this.c0 = math.select(new int3(0), new int3(1), v);
			this.c1 = math.select(new int3(0), new int3(1), v);
		}

		// Token: 0x06001A8E RID: 6798 RVA: 0x000492AF File Offset: 0x000474AF
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int3x2(bool3x2 v)
		{
			this.c0 = math.select(new int3(0), new int3(1), v.c0);
			this.c1 = math.select(new int3(0), new int3(1), v.c1);
		}

		// Token: 0x06001A8F RID: 6799 RVA: 0x000492EB File Offset: 0x000474EB
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int3x2(uint v)
		{
			this.c0 = (int3)v;
			this.c1 = (int3)v;
		}

		// Token: 0x06001A90 RID: 6800 RVA: 0x00049305 File Offset: 0x00047505
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int3x2(uint3x2 v)
		{
			this.c0 = (int3)v.c0;
			this.c1 = (int3)v.c1;
		}

		// Token: 0x06001A91 RID: 6801 RVA: 0x00049329 File Offset: 0x00047529
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int3x2(float v)
		{
			this.c0 = (int3)v;
			this.c1 = (int3)v;
		}

		// Token: 0x06001A92 RID: 6802 RVA: 0x00049343 File Offset: 0x00047543
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int3x2(float3x2 v)
		{
			this.c0 = (int3)v.c0;
			this.c1 = (int3)v.c1;
		}

		// Token: 0x06001A93 RID: 6803 RVA: 0x00049367 File Offset: 0x00047567
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int3x2(double v)
		{
			this.c0 = (int3)v;
			this.c1 = (int3)v;
		}

		// Token: 0x06001A94 RID: 6804 RVA: 0x00049381 File Offset: 0x00047581
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int3x2(double3x2 v)
		{
			this.c0 = (int3)v.c0;
			this.c1 = (int3)v.c1;
		}

		// Token: 0x06001A95 RID: 6805 RVA: 0x000493A5 File Offset: 0x000475A5
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static implicit operator int3x2(int v)
		{
			return new int3x2(v);
		}

		// Token: 0x06001A96 RID: 6806 RVA: 0x000493AD File Offset: 0x000475AD
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static explicit operator int3x2(bool v)
		{
			return new int3x2(v);
		}

		// Token: 0x06001A97 RID: 6807 RVA: 0x000493B5 File Offset: 0x000475B5
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static explicit operator int3x2(bool3x2 v)
		{
			return new int3x2(v);
		}

		// Token: 0x06001A98 RID: 6808 RVA: 0x000493BD File Offset: 0x000475BD
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static explicit operator int3x2(uint v)
		{
			return new int3x2(v);
		}

		// Token: 0x06001A99 RID: 6809 RVA: 0x000493C5 File Offset: 0x000475C5
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static explicit operator int3x2(uint3x2 v)
		{
			return new int3x2(v);
		}

		// Token: 0x06001A9A RID: 6810 RVA: 0x000493CD File Offset: 0x000475CD
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static explicit operator int3x2(float v)
		{
			return new int3x2(v);
		}

		// Token: 0x06001A9B RID: 6811 RVA: 0x000493D5 File Offset: 0x000475D5
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static explicit operator int3x2(float3x2 v)
		{
			return new int3x2(v);
		}

		// Token: 0x06001A9C RID: 6812 RVA: 0x000493DD File Offset: 0x000475DD
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static explicit operator int3x2(double v)
		{
			return new int3x2(v);
		}

		// Token: 0x06001A9D RID: 6813 RVA: 0x000493E5 File Offset: 0x000475E5
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static explicit operator int3x2(double3x2 v)
		{
			return new int3x2(v);
		}

		// Token: 0x06001A9E RID: 6814 RVA: 0x000493ED File Offset: 0x000475ED
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int3x2 operator *(int3x2 lhs, int3x2 rhs)
		{
			return new int3x2(lhs.c0 * rhs.c0, lhs.c1 * rhs.c1);
		}

		// Token: 0x06001A9F RID: 6815 RVA: 0x00049416 File Offset: 0x00047616
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int3x2 operator *(int3x2 lhs, int rhs)
		{
			return new int3x2(lhs.c0 * rhs, lhs.c1 * rhs);
		}

		// Token: 0x06001AA0 RID: 6816 RVA: 0x00049435 File Offset: 0x00047635
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int3x2 operator *(int lhs, int3x2 rhs)
		{
			return new int3x2(lhs * rhs.c0, lhs * rhs.c1);
		}

		// Token: 0x06001AA1 RID: 6817 RVA: 0x00049454 File Offset: 0x00047654
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int3x2 operator +(int3x2 lhs, int3x2 rhs)
		{
			return new int3x2(lhs.c0 + rhs.c0, lhs.c1 + rhs.c1);
		}

		// Token: 0x06001AA2 RID: 6818 RVA: 0x0004947D File Offset: 0x0004767D
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int3x2 operator +(int3x2 lhs, int rhs)
		{
			return new int3x2(lhs.c0 + rhs, lhs.c1 + rhs);
		}

		// Token: 0x06001AA3 RID: 6819 RVA: 0x0004949C File Offset: 0x0004769C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int3x2 operator +(int lhs, int3x2 rhs)
		{
			return new int3x2(lhs + rhs.c0, lhs + rhs.c1);
		}

		// Token: 0x06001AA4 RID: 6820 RVA: 0x000494BB File Offset: 0x000476BB
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int3x2 operator -(int3x2 lhs, int3x2 rhs)
		{
			return new int3x2(lhs.c0 - rhs.c0, lhs.c1 - rhs.c1);
		}

		// Token: 0x06001AA5 RID: 6821 RVA: 0x000494E4 File Offset: 0x000476E4
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int3x2 operator -(int3x2 lhs, int rhs)
		{
			return new int3x2(lhs.c0 - rhs, lhs.c1 - rhs);
		}

		// Token: 0x06001AA6 RID: 6822 RVA: 0x00049503 File Offset: 0x00047703
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int3x2 operator -(int lhs, int3x2 rhs)
		{
			return new int3x2(lhs - rhs.c0, lhs - rhs.c1);
		}

		// Token: 0x06001AA7 RID: 6823 RVA: 0x00049522 File Offset: 0x00047722
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int3x2 operator /(int3x2 lhs, int3x2 rhs)
		{
			return new int3x2(lhs.c0 / rhs.c0, lhs.c1 / rhs.c1);
		}

		// Token: 0x06001AA8 RID: 6824 RVA: 0x0004954B File Offset: 0x0004774B
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int3x2 operator /(int3x2 lhs, int rhs)
		{
			return new int3x2(lhs.c0 / rhs, lhs.c1 / rhs);
		}

		// Token: 0x06001AA9 RID: 6825 RVA: 0x0004956A File Offset: 0x0004776A
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int3x2 operator /(int lhs, int3x2 rhs)
		{
			return new int3x2(lhs / rhs.c0, lhs / rhs.c1);
		}

		// Token: 0x06001AAA RID: 6826 RVA: 0x00049589 File Offset: 0x00047789
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int3x2 operator %(int3x2 lhs, int3x2 rhs)
		{
			return new int3x2(lhs.c0 % rhs.c0, lhs.c1 % rhs.c1);
		}

		// Token: 0x06001AAB RID: 6827 RVA: 0x000495B2 File Offset: 0x000477B2
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int3x2 operator %(int3x2 lhs, int rhs)
		{
			return new int3x2(lhs.c0 % rhs, lhs.c1 % rhs);
		}

		// Token: 0x06001AAC RID: 6828 RVA: 0x000495D1 File Offset: 0x000477D1
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int3x2 operator %(int lhs, int3x2 rhs)
		{
			return new int3x2(lhs % rhs.c0, lhs % rhs.c1);
		}

		// Token: 0x06001AAD RID: 6829 RVA: 0x000495F0 File Offset: 0x000477F0
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int3x2 operator ++(int3x2 val)
		{
			int3 @int = int3.op_Increment(val.c0);
			val.c0 = @int;
			int3 int2 = @int;
			@int = int3.op_Increment(val.c1);
			val.c1 = @int;
			return new int3x2(int2, @int);
		}

		// Token: 0x06001AAE RID: 6830 RVA: 0x00049638 File Offset: 0x00047838
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int3x2 operator --(int3x2 val)
		{
			int3 @int = int3.op_Decrement(val.c0);
			val.c0 = @int;
			int3 int2 = @int;
			@int = int3.op_Decrement(val.c1);
			val.c1 = @int;
			return new int3x2(int2, @int);
		}

		// Token: 0x06001AAF RID: 6831 RVA: 0x0004967E File Offset: 0x0004787E
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool3x2 operator <(int3x2 lhs, int3x2 rhs)
		{
			return new bool3x2(lhs.c0 < rhs.c0, lhs.c1 < rhs.c1);
		}

		// Token: 0x06001AB0 RID: 6832 RVA: 0x000496A7 File Offset: 0x000478A7
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool3x2 operator <(int3x2 lhs, int rhs)
		{
			return new bool3x2(lhs.c0 < rhs, lhs.c1 < rhs);
		}

		// Token: 0x06001AB1 RID: 6833 RVA: 0x000496C6 File Offset: 0x000478C6
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool3x2 operator <(int lhs, int3x2 rhs)
		{
			return new bool3x2(lhs < rhs.c0, lhs < rhs.c1);
		}

		// Token: 0x06001AB2 RID: 6834 RVA: 0x000496E5 File Offset: 0x000478E5
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool3x2 operator <=(int3x2 lhs, int3x2 rhs)
		{
			return new bool3x2(lhs.c0 <= rhs.c0, lhs.c1 <= rhs.c1);
		}

		// Token: 0x06001AB3 RID: 6835 RVA: 0x0004970E File Offset: 0x0004790E
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool3x2 operator <=(int3x2 lhs, int rhs)
		{
			return new bool3x2(lhs.c0 <= rhs, lhs.c1 <= rhs);
		}

		// Token: 0x06001AB4 RID: 6836 RVA: 0x0004972D File Offset: 0x0004792D
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool3x2 operator <=(int lhs, int3x2 rhs)
		{
			return new bool3x2(lhs <= rhs.c0, lhs <= rhs.c1);
		}

		// Token: 0x06001AB5 RID: 6837 RVA: 0x0004974C File Offset: 0x0004794C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool3x2 operator >(int3x2 lhs, int3x2 rhs)
		{
			return new bool3x2(lhs.c0 > rhs.c0, lhs.c1 > rhs.c1);
		}

		// Token: 0x06001AB6 RID: 6838 RVA: 0x00049775 File Offset: 0x00047975
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool3x2 operator >(int3x2 lhs, int rhs)
		{
			return new bool3x2(lhs.c0 > rhs, lhs.c1 > rhs);
		}

		// Token: 0x06001AB7 RID: 6839 RVA: 0x00049794 File Offset: 0x00047994
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool3x2 operator >(int lhs, int3x2 rhs)
		{
			return new bool3x2(lhs > rhs.c0, lhs > rhs.c1);
		}

		// Token: 0x06001AB8 RID: 6840 RVA: 0x000497B3 File Offset: 0x000479B3
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool3x2 operator >=(int3x2 lhs, int3x2 rhs)
		{
			return new bool3x2(lhs.c0 >= rhs.c0, lhs.c1 >= rhs.c1);
		}

		// Token: 0x06001AB9 RID: 6841 RVA: 0x000497DC File Offset: 0x000479DC
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool3x2 operator >=(int3x2 lhs, int rhs)
		{
			return new bool3x2(lhs.c0 >= rhs, lhs.c1 >= rhs);
		}

		// Token: 0x06001ABA RID: 6842 RVA: 0x000497FB File Offset: 0x000479FB
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool3x2 operator >=(int lhs, int3x2 rhs)
		{
			return new bool3x2(lhs >= rhs.c0, lhs >= rhs.c1);
		}

		// Token: 0x06001ABB RID: 6843 RVA: 0x0004981A File Offset: 0x00047A1A
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int3x2 operator -(int3x2 val)
		{
			return new int3x2(-val.c0, -val.c1);
		}

		// Token: 0x06001ABC RID: 6844 RVA: 0x00049837 File Offset: 0x00047A37
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int3x2 operator +(int3x2 val)
		{
			return new int3x2(+val.c0, +val.c1);
		}

		// Token: 0x06001ABD RID: 6845 RVA: 0x00049854 File Offset: 0x00047A54
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int3x2 operator <<(int3x2 x, int n)
		{
			return new int3x2(x.c0 << n, x.c1 << n);
		}

		// Token: 0x06001ABE RID: 6846 RVA: 0x00049873 File Offset: 0x00047A73
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int3x2 operator >>(int3x2 x, int n)
		{
			return new int3x2(x.c0 >> n, x.c1 >> n);
		}

		// Token: 0x06001ABF RID: 6847 RVA: 0x00049892 File Offset: 0x00047A92
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool3x2 operator ==(int3x2 lhs, int3x2 rhs)
		{
			return new bool3x2(lhs.c0 == rhs.c0, lhs.c1 == rhs.c1);
		}

		// Token: 0x06001AC0 RID: 6848 RVA: 0x000498BB File Offset: 0x00047ABB
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool3x2 operator ==(int3x2 lhs, int rhs)
		{
			return new bool3x2(lhs.c0 == rhs, lhs.c1 == rhs);
		}

		// Token: 0x06001AC1 RID: 6849 RVA: 0x000498DA File Offset: 0x00047ADA
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool3x2 operator ==(int lhs, int3x2 rhs)
		{
			return new bool3x2(lhs == rhs.c0, lhs == rhs.c1);
		}

		// Token: 0x06001AC2 RID: 6850 RVA: 0x000498F9 File Offset: 0x00047AF9
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool3x2 operator !=(int3x2 lhs, int3x2 rhs)
		{
			return new bool3x2(lhs.c0 != rhs.c0, lhs.c1 != rhs.c1);
		}

		// Token: 0x06001AC3 RID: 6851 RVA: 0x00049922 File Offset: 0x00047B22
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool3x2 operator !=(int3x2 lhs, int rhs)
		{
			return new bool3x2(lhs.c0 != rhs, lhs.c1 != rhs);
		}

		// Token: 0x06001AC4 RID: 6852 RVA: 0x00049941 File Offset: 0x00047B41
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool3x2 operator !=(int lhs, int3x2 rhs)
		{
			return new bool3x2(lhs != rhs.c0, lhs != rhs.c1);
		}

		// Token: 0x06001AC5 RID: 6853 RVA: 0x00049960 File Offset: 0x00047B60
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int3x2 operator ~(int3x2 val)
		{
			return new int3x2(~val.c0, ~val.c1);
		}

		// Token: 0x06001AC6 RID: 6854 RVA: 0x0004997D File Offset: 0x00047B7D
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int3x2 operator &(int3x2 lhs, int3x2 rhs)
		{
			return new int3x2(lhs.c0 & rhs.c0, lhs.c1 & rhs.c1);
		}

		// Token: 0x06001AC7 RID: 6855 RVA: 0x000499A6 File Offset: 0x00047BA6
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int3x2 operator &(int3x2 lhs, int rhs)
		{
			return new int3x2(lhs.c0 & rhs, lhs.c1 & rhs);
		}

		// Token: 0x06001AC8 RID: 6856 RVA: 0x000499C5 File Offset: 0x00047BC5
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int3x2 operator &(int lhs, int3x2 rhs)
		{
			return new int3x2(lhs & rhs.c0, lhs & rhs.c1);
		}

		// Token: 0x06001AC9 RID: 6857 RVA: 0x000499E4 File Offset: 0x00047BE4
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int3x2 operator |(int3x2 lhs, int3x2 rhs)
		{
			return new int3x2(lhs.c0 | rhs.c0, lhs.c1 | rhs.c1);
		}

		// Token: 0x06001ACA RID: 6858 RVA: 0x00049A0D File Offset: 0x00047C0D
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int3x2 operator |(int3x2 lhs, int rhs)
		{
			return new int3x2(lhs.c0 | rhs, lhs.c1 | rhs);
		}

		// Token: 0x06001ACB RID: 6859 RVA: 0x00049A2C File Offset: 0x00047C2C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int3x2 operator |(int lhs, int3x2 rhs)
		{
			return new int3x2(lhs | rhs.c0, lhs | rhs.c1);
		}

		// Token: 0x06001ACC RID: 6860 RVA: 0x00049A4B File Offset: 0x00047C4B
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int3x2 operator ^(int3x2 lhs, int3x2 rhs)
		{
			return new int3x2(lhs.c0 ^ rhs.c0, lhs.c1 ^ rhs.c1);
		}

		// Token: 0x06001ACD RID: 6861 RVA: 0x00049A74 File Offset: 0x00047C74
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int3x2 operator ^(int3x2 lhs, int rhs)
		{
			return new int3x2(lhs.c0 ^ rhs, lhs.c1 ^ rhs);
		}

		// Token: 0x06001ACE RID: 6862 RVA: 0x00049A93 File Offset: 0x00047C93
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int3x2 operator ^(int lhs, int3x2 rhs)
		{
			return new int3x2(lhs ^ rhs.c0, lhs ^ rhs.c1);
		}

		// Token: 0x17000846 RID: 2118
		public unsafe ref int3 this[int index]
		{
			get
			{
				fixed (int3x2* ptr = &this)
				{
					return ref *(int3*)(ptr + (IntPtr)index * (IntPtr)sizeof(int3) / (IntPtr)sizeof(int3x2));
				}
			}
		}

		// Token: 0x06001AD0 RID: 6864 RVA: 0x00049ACF File Offset: 0x00047CCF
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool Equals(int3x2 rhs)
		{
			return this.c0.Equals(rhs.c0) && this.c1.Equals(rhs.c1);
		}

		// Token: 0x06001AD1 RID: 6865 RVA: 0x00049AF8 File Offset: 0x00047CF8
		public override bool Equals(object o)
		{
			if (o is int3x2)
			{
				int3x2 int3x = (int3x2)o;
				return this.Equals(int3x);
			}
			return false;
		}

		// Token: 0x06001AD2 RID: 6866 RVA: 0x00049B1D File Offset: 0x00047D1D
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public override int GetHashCode()
		{
			return (int)math.hash(this);
		}

		// Token: 0x06001AD3 RID: 6867 RVA: 0x00049B2C File Offset: 0x00047D2C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public override string ToString()
		{
			return string.Format("int3x2({0}, {1},  {2}, {3},  {4}, {5})", new object[]
			{
				this.c0.x,
				this.c1.x,
				this.c0.y,
				this.c1.y,
				this.c0.z,
				this.c1.z
			});
		}

		// Token: 0x06001AD4 RID: 6868 RVA: 0x00049BBC File Offset: 0x00047DBC
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public string ToString(string format, IFormatProvider formatProvider)
		{
			return string.Format("int3x2({0}, {1},  {2}, {3},  {4}, {5})", new object[]
			{
				this.c0.x.ToString(format, formatProvider),
				this.c1.x.ToString(format, formatProvider),
				this.c0.y.ToString(format, formatProvider),
				this.c1.y.ToString(format, formatProvider),
				this.c0.z.ToString(format, formatProvider),
				this.c1.z.ToString(format, formatProvider)
			});
		}

		// Token: 0x040000D1 RID: 209
		public int3 c0;

		// Token: 0x040000D2 RID: 210
		public int3 c1;

		// Token: 0x040000D3 RID: 211
		public static readonly int3x2 zero;
	}
}
