using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Unity.IL2CPP.CompilerServices;

namespace Unity.Mathematics
{
	// Token: 0x02000032 RID: 50
	[DebuggerTypeProxy(typeof(int3.DebuggerProxy))]
	[Il2CppEagerStaticClassConstruction]
	[Serializable]
	public struct int3 : IEquatable<int3>, IFormattable
	{
		// Token: 0x060019BB RID: 6587 RVA: 0x00047909 File Offset: 0x00045B09
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int3(int x, int y, int z)
		{
			this.x = x;
			this.y = y;
			this.z = z;
		}

		// Token: 0x060019BC RID: 6588 RVA: 0x00047920 File Offset: 0x00045B20
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int3(int x, int2 yz)
		{
			this.x = x;
			this.y = yz.x;
			this.z = yz.y;
		}

		// Token: 0x060019BD RID: 6589 RVA: 0x00047941 File Offset: 0x00045B41
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int3(int2 xy, int z)
		{
			this.x = xy.x;
			this.y = xy.y;
			this.z = z;
		}

		// Token: 0x060019BE RID: 6590 RVA: 0x00047962 File Offset: 0x00045B62
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int3(int3 xyz)
		{
			this.x = xyz.x;
			this.y = xyz.y;
			this.z = xyz.z;
		}

		// Token: 0x060019BF RID: 6591 RVA: 0x00047988 File Offset: 0x00045B88
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int3(int v)
		{
			this.x = v;
			this.y = v;
			this.z = v;
		}

		// Token: 0x060019C0 RID: 6592 RVA: 0x0004799F File Offset: 0x00045B9F
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int3(bool v)
		{
			this.x = (v ? 1 : 0);
			this.y = (v ? 1 : 0);
			this.z = (v ? 1 : 0);
		}

		// Token: 0x060019C1 RID: 6593 RVA: 0x000479C8 File Offset: 0x00045BC8
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int3(bool3 v)
		{
			this.x = (v.x ? 1 : 0);
			this.y = (v.y ? 1 : 0);
			this.z = (v.z ? 1 : 0);
		}

		// Token: 0x060019C2 RID: 6594 RVA: 0x00047A00 File Offset: 0x00045C00
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int3(uint v)
		{
			this.x = (int)v;
			this.y = (int)v;
			this.z = (int)v;
		}

		// Token: 0x060019C3 RID: 6595 RVA: 0x00047A17 File Offset: 0x00045C17
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int3(uint3 v)
		{
			this.x = (int)v.x;
			this.y = (int)v.y;
			this.z = (int)v.z;
		}

		// Token: 0x060019C4 RID: 6596 RVA: 0x00047A3D File Offset: 0x00045C3D
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int3(float v)
		{
			this.x = (int)v;
			this.y = (int)v;
			this.z = (int)v;
		}

		// Token: 0x060019C5 RID: 6597 RVA: 0x00047A57 File Offset: 0x00045C57
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int3(float3 v)
		{
			this.x = (int)v.x;
			this.y = (int)v.y;
			this.z = (int)v.z;
		}

		// Token: 0x060019C6 RID: 6598 RVA: 0x00047A80 File Offset: 0x00045C80
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int3(double v)
		{
			this.x = (int)v;
			this.y = (int)v;
			this.z = (int)v;
		}

		// Token: 0x060019C7 RID: 6599 RVA: 0x00047A9A File Offset: 0x00045C9A
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int3(double3 v)
		{
			this.x = (int)v.x;
			this.y = (int)v.y;
			this.z = (int)v.z;
		}

		// Token: 0x060019C8 RID: 6600 RVA: 0x00047AC3 File Offset: 0x00045CC3
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static implicit operator int3(int v)
		{
			return new int3(v);
		}

		// Token: 0x060019C9 RID: 6601 RVA: 0x00047ACB File Offset: 0x00045CCB
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static explicit operator int3(bool v)
		{
			return new int3(v);
		}

		// Token: 0x060019CA RID: 6602 RVA: 0x00047AD3 File Offset: 0x00045CD3
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static explicit operator int3(bool3 v)
		{
			return new int3(v);
		}

		// Token: 0x060019CB RID: 6603 RVA: 0x00047ADB File Offset: 0x00045CDB
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static explicit operator int3(uint v)
		{
			return new int3(v);
		}

		// Token: 0x060019CC RID: 6604 RVA: 0x00047AE3 File Offset: 0x00045CE3
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static explicit operator int3(uint3 v)
		{
			return new int3(v);
		}

		// Token: 0x060019CD RID: 6605 RVA: 0x00047AEB File Offset: 0x00045CEB
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static explicit operator int3(float v)
		{
			return new int3(v);
		}

		// Token: 0x060019CE RID: 6606 RVA: 0x00047AF3 File Offset: 0x00045CF3
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static explicit operator int3(float3 v)
		{
			return new int3(v);
		}

		// Token: 0x060019CF RID: 6607 RVA: 0x00047AFB File Offset: 0x00045CFB
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static explicit operator int3(double v)
		{
			return new int3(v);
		}

		// Token: 0x060019D0 RID: 6608 RVA: 0x00047B03 File Offset: 0x00045D03
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static explicit operator int3(double3 v)
		{
			return new int3(v);
		}

		// Token: 0x060019D1 RID: 6609 RVA: 0x00047B0B File Offset: 0x00045D0B
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int3 operator *(int3 lhs, int3 rhs)
		{
			return new int3(lhs.x * rhs.x, lhs.y * rhs.y, lhs.z * rhs.z);
		}

		// Token: 0x060019D2 RID: 6610 RVA: 0x00047B39 File Offset: 0x00045D39
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int3 operator *(int3 lhs, int rhs)
		{
			return new int3(lhs.x * rhs, lhs.y * rhs, lhs.z * rhs);
		}

		// Token: 0x060019D3 RID: 6611 RVA: 0x00047B58 File Offset: 0x00045D58
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int3 operator *(int lhs, int3 rhs)
		{
			return new int3(lhs * rhs.x, lhs * rhs.y, lhs * rhs.z);
		}

		// Token: 0x060019D4 RID: 6612 RVA: 0x00047B77 File Offset: 0x00045D77
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int3 operator +(int3 lhs, int3 rhs)
		{
			return new int3(lhs.x + rhs.x, lhs.y + rhs.y, lhs.z + rhs.z);
		}

		// Token: 0x060019D5 RID: 6613 RVA: 0x00047BA5 File Offset: 0x00045DA5
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int3 operator +(int3 lhs, int rhs)
		{
			return new int3(lhs.x + rhs, lhs.y + rhs, lhs.z + rhs);
		}

		// Token: 0x060019D6 RID: 6614 RVA: 0x00047BC4 File Offset: 0x00045DC4
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int3 operator +(int lhs, int3 rhs)
		{
			return new int3(lhs + rhs.x, lhs + rhs.y, lhs + rhs.z);
		}

		// Token: 0x060019D7 RID: 6615 RVA: 0x00047BE3 File Offset: 0x00045DE3
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int3 operator -(int3 lhs, int3 rhs)
		{
			return new int3(lhs.x - rhs.x, lhs.y - rhs.y, lhs.z - rhs.z);
		}

		// Token: 0x060019D8 RID: 6616 RVA: 0x00047C11 File Offset: 0x00045E11
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int3 operator -(int3 lhs, int rhs)
		{
			return new int3(lhs.x - rhs, lhs.y - rhs, lhs.z - rhs);
		}

		// Token: 0x060019D9 RID: 6617 RVA: 0x00047C30 File Offset: 0x00045E30
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int3 operator -(int lhs, int3 rhs)
		{
			return new int3(lhs - rhs.x, lhs - rhs.y, lhs - rhs.z);
		}

		// Token: 0x060019DA RID: 6618 RVA: 0x00047C4F File Offset: 0x00045E4F
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int3 operator /(int3 lhs, int3 rhs)
		{
			return new int3(lhs.x / rhs.x, lhs.y / rhs.y, lhs.z / rhs.z);
		}

		// Token: 0x060019DB RID: 6619 RVA: 0x00047C7D File Offset: 0x00045E7D
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int3 operator /(int3 lhs, int rhs)
		{
			return new int3(lhs.x / rhs, lhs.y / rhs, lhs.z / rhs);
		}

		// Token: 0x060019DC RID: 6620 RVA: 0x00047C9C File Offset: 0x00045E9C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int3 operator /(int lhs, int3 rhs)
		{
			return new int3(lhs / rhs.x, lhs / rhs.y, lhs / rhs.z);
		}

		// Token: 0x060019DD RID: 6621 RVA: 0x00047CBB File Offset: 0x00045EBB
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int3 operator %(int3 lhs, int3 rhs)
		{
			return new int3(lhs.x % rhs.x, lhs.y % rhs.y, lhs.z % rhs.z);
		}

		// Token: 0x060019DE RID: 6622 RVA: 0x00047CE9 File Offset: 0x00045EE9
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int3 operator %(int3 lhs, int rhs)
		{
			return new int3(lhs.x % rhs, lhs.y % rhs, lhs.z % rhs);
		}

		// Token: 0x060019DF RID: 6623 RVA: 0x00047D08 File Offset: 0x00045F08
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int3 operator %(int lhs, int3 rhs)
		{
			return new int3(lhs % rhs.x, lhs % rhs.y, lhs % rhs.z);
		}

		// Token: 0x060019E0 RID: 6624 RVA: 0x00047D28 File Offset: 0x00045F28
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int3 operator ++(int3 val)
		{
			int num = val.x + 1;
			val.x = num;
			int num2 = num;
			num = val.y + 1;
			val.y = num;
			int num3 = num;
			num = val.z + 1;
			val.z = num;
			return new int3(num2, num3, num);
		}

		// Token: 0x060019E1 RID: 6625 RVA: 0x00047D68 File Offset: 0x00045F68
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int3 operator --(int3 val)
		{
			int num = val.x - 1;
			val.x = num;
			int num2 = num;
			num = val.y - 1;
			val.y = num;
			int num3 = num;
			num = val.z - 1;
			val.z = num;
			return new int3(num2, num3, num);
		}

		// Token: 0x060019E2 RID: 6626 RVA: 0x00047DA7 File Offset: 0x00045FA7
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool3 operator <(int3 lhs, int3 rhs)
		{
			return new bool3(lhs.x < rhs.x, lhs.y < rhs.y, lhs.z < rhs.z);
		}

		// Token: 0x060019E3 RID: 6627 RVA: 0x00047DD8 File Offset: 0x00045FD8
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool3 operator <(int3 lhs, int rhs)
		{
			return new bool3(lhs.x < rhs, lhs.y < rhs, lhs.z < rhs);
		}

		// Token: 0x060019E4 RID: 6628 RVA: 0x00047DFA File Offset: 0x00045FFA
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool3 operator <(int lhs, int3 rhs)
		{
			return new bool3(lhs < rhs.x, lhs < rhs.y, lhs < rhs.z);
		}

		// Token: 0x060019E5 RID: 6629 RVA: 0x00047E1C File Offset: 0x0004601C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool3 operator <=(int3 lhs, int3 rhs)
		{
			return new bool3(lhs.x <= rhs.x, lhs.y <= rhs.y, lhs.z <= rhs.z);
		}

		// Token: 0x060019E6 RID: 6630 RVA: 0x00047E56 File Offset: 0x00046056
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool3 operator <=(int3 lhs, int rhs)
		{
			return new bool3(lhs.x <= rhs, lhs.y <= rhs, lhs.z <= rhs);
		}

		// Token: 0x060019E7 RID: 6631 RVA: 0x00047E81 File Offset: 0x00046081
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool3 operator <=(int lhs, int3 rhs)
		{
			return new bool3(lhs <= rhs.x, lhs <= rhs.y, lhs <= rhs.z);
		}

		// Token: 0x060019E8 RID: 6632 RVA: 0x00047EAC File Offset: 0x000460AC
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool3 operator >(int3 lhs, int3 rhs)
		{
			return new bool3(lhs.x > rhs.x, lhs.y > rhs.y, lhs.z > rhs.z);
		}

		// Token: 0x060019E9 RID: 6633 RVA: 0x00047EDD File Offset: 0x000460DD
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool3 operator >(int3 lhs, int rhs)
		{
			return new bool3(lhs.x > rhs, lhs.y > rhs, lhs.z > rhs);
		}

		// Token: 0x060019EA RID: 6634 RVA: 0x00047EFF File Offset: 0x000460FF
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool3 operator >(int lhs, int3 rhs)
		{
			return new bool3(lhs > rhs.x, lhs > rhs.y, lhs > rhs.z);
		}

		// Token: 0x060019EB RID: 6635 RVA: 0x00047F21 File Offset: 0x00046121
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool3 operator >=(int3 lhs, int3 rhs)
		{
			return new bool3(lhs.x >= rhs.x, lhs.y >= rhs.y, lhs.z >= rhs.z);
		}

		// Token: 0x060019EC RID: 6636 RVA: 0x00047F5B File Offset: 0x0004615B
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool3 operator >=(int3 lhs, int rhs)
		{
			return new bool3(lhs.x >= rhs, lhs.y >= rhs, lhs.z >= rhs);
		}

		// Token: 0x060019ED RID: 6637 RVA: 0x00047F86 File Offset: 0x00046186
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool3 operator >=(int lhs, int3 rhs)
		{
			return new bool3(lhs >= rhs.x, lhs >= rhs.y, lhs >= rhs.z);
		}

		// Token: 0x060019EE RID: 6638 RVA: 0x00047FB1 File Offset: 0x000461B1
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int3 operator -(int3 val)
		{
			return new int3(-val.x, -val.y, -val.z);
		}

		// Token: 0x060019EF RID: 6639 RVA: 0x00047FCD File Offset: 0x000461CD
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int3 operator +(int3 val)
		{
			return new int3(val.x, val.y, val.z);
		}

		// Token: 0x060019F0 RID: 6640 RVA: 0x00047FE6 File Offset: 0x000461E6
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int3 operator <<(int3 x, int n)
		{
			return new int3(x.x << n, x.y << n, x.z << n);
		}

		// Token: 0x060019F1 RID: 6641 RVA: 0x0004800E File Offset: 0x0004620E
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int3 operator >>(int3 x, int n)
		{
			return new int3(x.x >> n, x.y >> n, x.z >> n);
		}

		// Token: 0x060019F2 RID: 6642 RVA: 0x00048036 File Offset: 0x00046236
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool3 operator ==(int3 lhs, int3 rhs)
		{
			return new bool3(lhs.x == rhs.x, lhs.y == rhs.y, lhs.z == rhs.z);
		}

		// Token: 0x060019F3 RID: 6643 RVA: 0x00048067 File Offset: 0x00046267
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool3 operator ==(int3 lhs, int rhs)
		{
			return new bool3(lhs.x == rhs, lhs.y == rhs, lhs.z == rhs);
		}

		// Token: 0x060019F4 RID: 6644 RVA: 0x00048089 File Offset: 0x00046289
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool3 operator ==(int lhs, int3 rhs)
		{
			return new bool3(lhs == rhs.x, lhs == rhs.y, lhs == rhs.z);
		}

		// Token: 0x060019F5 RID: 6645 RVA: 0x000480AB File Offset: 0x000462AB
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool3 operator !=(int3 lhs, int3 rhs)
		{
			return new bool3(lhs.x != rhs.x, lhs.y != rhs.y, lhs.z != rhs.z);
		}

		// Token: 0x060019F6 RID: 6646 RVA: 0x000480E5 File Offset: 0x000462E5
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool3 operator !=(int3 lhs, int rhs)
		{
			return new bool3(lhs.x != rhs, lhs.y != rhs, lhs.z != rhs);
		}

		// Token: 0x060019F7 RID: 6647 RVA: 0x00048110 File Offset: 0x00046310
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool3 operator !=(int lhs, int3 rhs)
		{
			return new bool3(lhs != rhs.x, lhs != rhs.y, lhs != rhs.z);
		}

		// Token: 0x060019F8 RID: 6648 RVA: 0x0004813B File Offset: 0x0004633B
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int3 operator ~(int3 val)
		{
			return new int3(~val.x, ~val.y, ~val.z);
		}

		// Token: 0x060019F9 RID: 6649 RVA: 0x00048157 File Offset: 0x00046357
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int3 operator &(int3 lhs, int3 rhs)
		{
			return new int3(lhs.x & rhs.x, lhs.y & rhs.y, lhs.z & rhs.z);
		}

		// Token: 0x060019FA RID: 6650 RVA: 0x00048185 File Offset: 0x00046385
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int3 operator &(int3 lhs, int rhs)
		{
			return new int3(lhs.x & rhs, lhs.y & rhs, lhs.z & rhs);
		}

		// Token: 0x060019FB RID: 6651 RVA: 0x000481A4 File Offset: 0x000463A4
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int3 operator &(int lhs, int3 rhs)
		{
			return new int3(lhs & rhs.x, lhs & rhs.y, lhs & rhs.z);
		}

		// Token: 0x060019FC RID: 6652 RVA: 0x000481C3 File Offset: 0x000463C3
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int3 operator |(int3 lhs, int3 rhs)
		{
			return new int3(lhs.x | rhs.x, lhs.y | rhs.y, lhs.z | rhs.z);
		}

		// Token: 0x060019FD RID: 6653 RVA: 0x000481F1 File Offset: 0x000463F1
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int3 operator |(int3 lhs, int rhs)
		{
			return new int3(lhs.x | rhs, lhs.y | rhs, lhs.z | rhs);
		}

		// Token: 0x060019FE RID: 6654 RVA: 0x00048210 File Offset: 0x00046410
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int3 operator |(int lhs, int3 rhs)
		{
			return new int3(lhs | rhs.x, lhs | rhs.y, lhs | rhs.z);
		}

		// Token: 0x060019FF RID: 6655 RVA: 0x0004822F File Offset: 0x0004642F
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int3 operator ^(int3 lhs, int3 rhs)
		{
			return new int3(lhs.x ^ rhs.x, lhs.y ^ rhs.y, lhs.z ^ rhs.z);
		}

		// Token: 0x06001A00 RID: 6656 RVA: 0x0004825D File Offset: 0x0004645D
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int3 operator ^(int3 lhs, int rhs)
		{
			return new int3(lhs.x ^ rhs, lhs.y ^ rhs, lhs.z ^ rhs);
		}

		// Token: 0x06001A01 RID: 6657 RVA: 0x0004827C File Offset: 0x0004647C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int3 operator ^(int lhs, int3 rhs)
		{
			return new int3(lhs ^ rhs.x, lhs ^ rhs.y, lhs ^ rhs.z);
		}

		// Token: 0x170007D0 RID: 2000
		// (get) Token: 0x06001A02 RID: 6658 RVA: 0x0004829B File Offset: 0x0004649B
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int4 xxxx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new int4(this.x, this.x, this.x, this.x);
			}
		}

		// Token: 0x170007D1 RID: 2001
		// (get) Token: 0x06001A03 RID: 6659 RVA: 0x000482BA File Offset: 0x000464BA
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int4 xxxy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new int4(this.x, this.x, this.x, this.y);
			}
		}

		// Token: 0x170007D2 RID: 2002
		// (get) Token: 0x06001A04 RID: 6660 RVA: 0x000482D9 File Offset: 0x000464D9
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int4 xxxz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new int4(this.x, this.x, this.x, this.z);
			}
		}

		// Token: 0x170007D3 RID: 2003
		// (get) Token: 0x06001A05 RID: 6661 RVA: 0x000482F8 File Offset: 0x000464F8
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int4 xxyx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new int4(this.x, this.x, this.y, this.x);
			}
		}

		// Token: 0x170007D4 RID: 2004
		// (get) Token: 0x06001A06 RID: 6662 RVA: 0x00048317 File Offset: 0x00046517
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int4 xxyy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new int4(this.x, this.x, this.y, this.y);
			}
		}

		// Token: 0x170007D5 RID: 2005
		// (get) Token: 0x06001A07 RID: 6663 RVA: 0x00048336 File Offset: 0x00046536
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int4 xxyz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new int4(this.x, this.x, this.y, this.z);
			}
		}

		// Token: 0x170007D6 RID: 2006
		// (get) Token: 0x06001A08 RID: 6664 RVA: 0x00048355 File Offset: 0x00046555
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int4 xxzx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new int4(this.x, this.x, this.z, this.x);
			}
		}

		// Token: 0x170007D7 RID: 2007
		// (get) Token: 0x06001A09 RID: 6665 RVA: 0x00048374 File Offset: 0x00046574
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int4 xxzy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new int4(this.x, this.x, this.z, this.y);
			}
		}

		// Token: 0x170007D8 RID: 2008
		// (get) Token: 0x06001A0A RID: 6666 RVA: 0x00048393 File Offset: 0x00046593
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int4 xxzz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new int4(this.x, this.x, this.z, this.z);
			}
		}

		// Token: 0x170007D9 RID: 2009
		// (get) Token: 0x06001A0B RID: 6667 RVA: 0x000483B2 File Offset: 0x000465B2
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int4 xyxx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new int4(this.x, this.y, this.x, this.x);
			}
		}

		// Token: 0x170007DA RID: 2010
		// (get) Token: 0x06001A0C RID: 6668 RVA: 0x000483D1 File Offset: 0x000465D1
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int4 xyxy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new int4(this.x, this.y, this.x, this.y);
			}
		}

		// Token: 0x170007DB RID: 2011
		// (get) Token: 0x06001A0D RID: 6669 RVA: 0x000483F0 File Offset: 0x000465F0
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int4 xyxz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new int4(this.x, this.y, this.x, this.z);
			}
		}

		// Token: 0x170007DC RID: 2012
		// (get) Token: 0x06001A0E RID: 6670 RVA: 0x0004840F File Offset: 0x0004660F
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int4 xyyx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new int4(this.x, this.y, this.y, this.x);
			}
		}

		// Token: 0x170007DD RID: 2013
		// (get) Token: 0x06001A0F RID: 6671 RVA: 0x0004842E File Offset: 0x0004662E
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int4 xyyy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new int4(this.x, this.y, this.y, this.y);
			}
		}

		// Token: 0x170007DE RID: 2014
		// (get) Token: 0x06001A10 RID: 6672 RVA: 0x0004844D File Offset: 0x0004664D
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int4 xyyz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new int4(this.x, this.y, this.y, this.z);
			}
		}

		// Token: 0x170007DF RID: 2015
		// (get) Token: 0x06001A11 RID: 6673 RVA: 0x0004846C File Offset: 0x0004666C
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int4 xyzx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new int4(this.x, this.y, this.z, this.x);
			}
		}

		// Token: 0x170007E0 RID: 2016
		// (get) Token: 0x06001A12 RID: 6674 RVA: 0x0004848B File Offset: 0x0004668B
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int4 xyzy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new int4(this.x, this.y, this.z, this.y);
			}
		}

		// Token: 0x170007E1 RID: 2017
		// (get) Token: 0x06001A13 RID: 6675 RVA: 0x000484AA File Offset: 0x000466AA
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int4 xyzz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new int4(this.x, this.y, this.z, this.z);
			}
		}

		// Token: 0x170007E2 RID: 2018
		// (get) Token: 0x06001A14 RID: 6676 RVA: 0x000484C9 File Offset: 0x000466C9
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int4 xzxx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new int4(this.x, this.z, this.x, this.x);
			}
		}

		// Token: 0x170007E3 RID: 2019
		// (get) Token: 0x06001A15 RID: 6677 RVA: 0x000484E8 File Offset: 0x000466E8
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int4 xzxy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new int4(this.x, this.z, this.x, this.y);
			}
		}

		// Token: 0x170007E4 RID: 2020
		// (get) Token: 0x06001A16 RID: 6678 RVA: 0x00048507 File Offset: 0x00046707
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int4 xzxz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new int4(this.x, this.z, this.x, this.z);
			}
		}

		// Token: 0x170007E5 RID: 2021
		// (get) Token: 0x06001A17 RID: 6679 RVA: 0x00048526 File Offset: 0x00046726
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int4 xzyx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new int4(this.x, this.z, this.y, this.x);
			}
		}

		// Token: 0x170007E6 RID: 2022
		// (get) Token: 0x06001A18 RID: 6680 RVA: 0x00048545 File Offset: 0x00046745
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int4 xzyy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new int4(this.x, this.z, this.y, this.y);
			}
		}

		// Token: 0x170007E7 RID: 2023
		// (get) Token: 0x06001A19 RID: 6681 RVA: 0x00048564 File Offset: 0x00046764
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int4 xzyz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new int4(this.x, this.z, this.y, this.z);
			}
		}

		// Token: 0x170007E8 RID: 2024
		// (get) Token: 0x06001A1A RID: 6682 RVA: 0x00048583 File Offset: 0x00046783
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int4 xzzx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new int4(this.x, this.z, this.z, this.x);
			}
		}

		// Token: 0x170007E9 RID: 2025
		// (get) Token: 0x06001A1B RID: 6683 RVA: 0x000485A2 File Offset: 0x000467A2
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int4 xzzy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new int4(this.x, this.z, this.z, this.y);
			}
		}

		// Token: 0x170007EA RID: 2026
		// (get) Token: 0x06001A1C RID: 6684 RVA: 0x000485C1 File Offset: 0x000467C1
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int4 xzzz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new int4(this.x, this.z, this.z, this.z);
			}
		}

		// Token: 0x170007EB RID: 2027
		// (get) Token: 0x06001A1D RID: 6685 RVA: 0x000485E0 File Offset: 0x000467E0
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int4 yxxx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new int4(this.y, this.x, this.x, this.x);
			}
		}

		// Token: 0x170007EC RID: 2028
		// (get) Token: 0x06001A1E RID: 6686 RVA: 0x000485FF File Offset: 0x000467FF
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int4 yxxy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new int4(this.y, this.x, this.x, this.y);
			}
		}

		// Token: 0x170007ED RID: 2029
		// (get) Token: 0x06001A1F RID: 6687 RVA: 0x0004861E File Offset: 0x0004681E
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int4 yxxz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new int4(this.y, this.x, this.x, this.z);
			}
		}

		// Token: 0x170007EE RID: 2030
		// (get) Token: 0x06001A20 RID: 6688 RVA: 0x0004863D File Offset: 0x0004683D
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int4 yxyx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new int4(this.y, this.x, this.y, this.x);
			}
		}

		// Token: 0x170007EF RID: 2031
		// (get) Token: 0x06001A21 RID: 6689 RVA: 0x0004865C File Offset: 0x0004685C
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int4 yxyy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new int4(this.y, this.x, this.y, this.y);
			}
		}

		// Token: 0x170007F0 RID: 2032
		// (get) Token: 0x06001A22 RID: 6690 RVA: 0x0004867B File Offset: 0x0004687B
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int4 yxyz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new int4(this.y, this.x, this.y, this.z);
			}
		}

		// Token: 0x170007F1 RID: 2033
		// (get) Token: 0x06001A23 RID: 6691 RVA: 0x0004869A File Offset: 0x0004689A
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int4 yxzx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new int4(this.y, this.x, this.z, this.x);
			}
		}

		// Token: 0x170007F2 RID: 2034
		// (get) Token: 0x06001A24 RID: 6692 RVA: 0x000486B9 File Offset: 0x000468B9
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int4 yxzy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new int4(this.y, this.x, this.z, this.y);
			}
		}

		// Token: 0x170007F3 RID: 2035
		// (get) Token: 0x06001A25 RID: 6693 RVA: 0x000486D8 File Offset: 0x000468D8
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int4 yxzz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new int4(this.y, this.x, this.z, this.z);
			}
		}

		// Token: 0x170007F4 RID: 2036
		// (get) Token: 0x06001A26 RID: 6694 RVA: 0x000486F7 File Offset: 0x000468F7
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int4 yyxx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new int4(this.y, this.y, this.x, this.x);
			}
		}

		// Token: 0x170007F5 RID: 2037
		// (get) Token: 0x06001A27 RID: 6695 RVA: 0x00048716 File Offset: 0x00046916
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int4 yyxy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new int4(this.y, this.y, this.x, this.y);
			}
		}

		// Token: 0x170007F6 RID: 2038
		// (get) Token: 0x06001A28 RID: 6696 RVA: 0x00048735 File Offset: 0x00046935
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int4 yyxz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new int4(this.y, this.y, this.x, this.z);
			}
		}

		// Token: 0x170007F7 RID: 2039
		// (get) Token: 0x06001A29 RID: 6697 RVA: 0x00048754 File Offset: 0x00046954
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int4 yyyx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new int4(this.y, this.y, this.y, this.x);
			}
		}

		// Token: 0x170007F8 RID: 2040
		// (get) Token: 0x06001A2A RID: 6698 RVA: 0x00048773 File Offset: 0x00046973
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int4 yyyy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new int4(this.y, this.y, this.y, this.y);
			}
		}

		// Token: 0x170007F9 RID: 2041
		// (get) Token: 0x06001A2B RID: 6699 RVA: 0x00048792 File Offset: 0x00046992
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int4 yyyz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new int4(this.y, this.y, this.y, this.z);
			}
		}

		// Token: 0x170007FA RID: 2042
		// (get) Token: 0x06001A2C RID: 6700 RVA: 0x000487B1 File Offset: 0x000469B1
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int4 yyzx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new int4(this.y, this.y, this.z, this.x);
			}
		}

		// Token: 0x170007FB RID: 2043
		// (get) Token: 0x06001A2D RID: 6701 RVA: 0x000487D0 File Offset: 0x000469D0
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int4 yyzy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new int4(this.y, this.y, this.z, this.y);
			}
		}

		// Token: 0x170007FC RID: 2044
		// (get) Token: 0x06001A2E RID: 6702 RVA: 0x000487EF File Offset: 0x000469EF
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int4 yyzz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new int4(this.y, this.y, this.z, this.z);
			}
		}

		// Token: 0x170007FD RID: 2045
		// (get) Token: 0x06001A2F RID: 6703 RVA: 0x0004880E File Offset: 0x00046A0E
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int4 yzxx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new int4(this.y, this.z, this.x, this.x);
			}
		}

		// Token: 0x170007FE RID: 2046
		// (get) Token: 0x06001A30 RID: 6704 RVA: 0x0004882D File Offset: 0x00046A2D
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int4 yzxy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new int4(this.y, this.z, this.x, this.y);
			}
		}

		// Token: 0x170007FF RID: 2047
		// (get) Token: 0x06001A31 RID: 6705 RVA: 0x0004884C File Offset: 0x00046A4C
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int4 yzxz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new int4(this.y, this.z, this.x, this.z);
			}
		}

		// Token: 0x17000800 RID: 2048
		// (get) Token: 0x06001A32 RID: 6706 RVA: 0x0004886B File Offset: 0x00046A6B
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int4 yzyx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new int4(this.y, this.z, this.y, this.x);
			}
		}

		// Token: 0x17000801 RID: 2049
		// (get) Token: 0x06001A33 RID: 6707 RVA: 0x0004888A File Offset: 0x00046A8A
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int4 yzyy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new int4(this.y, this.z, this.y, this.y);
			}
		}

		// Token: 0x17000802 RID: 2050
		// (get) Token: 0x06001A34 RID: 6708 RVA: 0x000488A9 File Offset: 0x00046AA9
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int4 yzyz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new int4(this.y, this.z, this.y, this.z);
			}
		}

		// Token: 0x17000803 RID: 2051
		// (get) Token: 0x06001A35 RID: 6709 RVA: 0x000488C8 File Offset: 0x00046AC8
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int4 yzzx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new int4(this.y, this.z, this.z, this.x);
			}
		}

		// Token: 0x17000804 RID: 2052
		// (get) Token: 0x06001A36 RID: 6710 RVA: 0x000488E7 File Offset: 0x00046AE7
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int4 yzzy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new int4(this.y, this.z, this.z, this.y);
			}
		}

		// Token: 0x17000805 RID: 2053
		// (get) Token: 0x06001A37 RID: 6711 RVA: 0x00048906 File Offset: 0x00046B06
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int4 yzzz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new int4(this.y, this.z, this.z, this.z);
			}
		}

		// Token: 0x17000806 RID: 2054
		// (get) Token: 0x06001A38 RID: 6712 RVA: 0x00048925 File Offset: 0x00046B25
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int4 zxxx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new int4(this.z, this.x, this.x, this.x);
			}
		}

		// Token: 0x17000807 RID: 2055
		// (get) Token: 0x06001A39 RID: 6713 RVA: 0x00048944 File Offset: 0x00046B44
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int4 zxxy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new int4(this.z, this.x, this.x, this.y);
			}
		}

		// Token: 0x17000808 RID: 2056
		// (get) Token: 0x06001A3A RID: 6714 RVA: 0x00048963 File Offset: 0x00046B63
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int4 zxxz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new int4(this.z, this.x, this.x, this.z);
			}
		}

		// Token: 0x17000809 RID: 2057
		// (get) Token: 0x06001A3B RID: 6715 RVA: 0x00048982 File Offset: 0x00046B82
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int4 zxyx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new int4(this.z, this.x, this.y, this.x);
			}
		}

		// Token: 0x1700080A RID: 2058
		// (get) Token: 0x06001A3C RID: 6716 RVA: 0x000489A1 File Offset: 0x00046BA1
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int4 zxyy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new int4(this.z, this.x, this.y, this.y);
			}
		}

		// Token: 0x1700080B RID: 2059
		// (get) Token: 0x06001A3D RID: 6717 RVA: 0x000489C0 File Offset: 0x00046BC0
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int4 zxyz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new int4(this.z, this.x, this.y, this.z);
			}
		}

		// Token: 0x1700080C RID: 2060
		// (get) Token: 0x06001A3E RID: 6718 RVA: 0x000489DF File Offset: 0x00046BDF
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int4 zxzx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new int4(this.z, this.x, this.z, this.x);
			}
		}

		// Token: 0x1700080D RID: 2061
		// (get) Token: 0x06001A3F RID: 6719 RVA: 0x000489FE File Offset: 0x00046BFE
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int4 zxzy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new int4(this.z, this.x, this.z, this.y);
			}
		}

		// Token: 0x1700080E RID: 2062
		// (get) Token: 0x06001A40 RID: 6720 RVA: 0x00048A1D File Offset: 0x00046C1D
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int4 zxzz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new int4(this.z, this.x, this.z, this.z);
			}
		}

		// Token: 0x1700080F RID: 2063
		// (get) Token: 0x06001A41 RID: 6721 RVA: 0x00048A3C File Offset: 0x00046C3C
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int4 zyxx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new int4(this.z, this.y, this.x, this.x);
			}
		}

		// Token: 0x17000810 RID: 2064
		// (get) Token: 0x06001A42 RID: 6722 RVA: 0x00048A5B File Offset: 0x00046C5B
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int4 zyxy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new int4(this.z, this.y, this.x, this.y);
			}
		}

		// Token: 0x17000811 RID: 2065
		// (get) Token: 0x06001A43 RID: 6723 RVA: 0x00048A7A File Offset: 0x00046C7A
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int4 zyxz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new int4(this.z, this.y, this.x, this.z);
			}
		}

		// Token: 0x17000812 RID: 2066
		// (get) Token: 0x06001A44 RID: 6724 RVA: 0x00048A99 File Offset: 0x00046C99
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int4 zyyx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new int4(this.z, this.y, this.y, this.x);
			}
		}

		// Token: 0x17000813 RID: 2067
		// (get) Token: 0x06001A45 RID: 6725 RVA: 0x00048AB8 File Offset: 0x00046CB8
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int4 zyyy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new int4(this.z, this.y, this.y, this.y);
			}
		}

		// Token: 0x17000814 RID: 2068
		// (get) Token: 0x06001A46 RID: 6726 RVA: 0x00048AD7 File Offset: 0x00046CD7
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int4 zyyz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new int4(this.z, this.y, this.y, this.z);
			}
		}

		// Token: 0x17000815 RID: 2069
		// (get) Token: 0x06001A47 RID: 6727 RVA: 0x00048AF6 File Offset: 0x00046CF6
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int4 zyzx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new int4(this.z, this.y, this.z, this.x);
			}
		}

		// Token: 0x17000816 RID: 2070
		// (get) Token: 0x06001A48 RID: 6728 RVA: 0x00048B15 File Offset: 0x00046D15
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int4 zyzy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new int4(this.z, this.y, this.z, this.y);
			}
		}

		// Token: 0x17000817 RID: 2071
		// (get) Token: 0x06001A49 RID: 6729 RVA: 0x00048B34 File Offset: 0x00046D34
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int4 zyzz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new int4(this.z, this.y, this.z, this.z);
			}
		}

		// Token: 0x17000818 RID: 2072
		// (get) Token: 0x06001A4A RID: 6730 RVA: 0x00048B53 File Offset: 0x00046D53
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int4 zzxx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new int4(this.z, this.z, this.x, this.x);
			}
		}

		// Token: 0x17000819 RID: 2073
		// (get) Token: 0x06001A4B RID: 6731 RVA: 0x00048B72 File Offset: 0x00046D72
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int4 zzxy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new int4(this.z, this.z, this.x, this.y);
			}
		}

		// Token: 0x1700081A RID: 2074
		// (get) Token: 0x06001A4C RID: 6732 RVA: 0x00048B91 File Offset: 0x00046D91
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int4 zzxz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new int4(this.z, this.z, this.x, this.z);
			}
		}

		// Token: 0x1700081B RID: 2075
		// (get) Token: 0x06001A4D RID: 6733 RVA: 0x00048BB0 File Offset: 0x00046DB0
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int4 zzyx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new int4(this.z, this.z, this.y, this.x);
			}
		}

		// Token: 0x1700081C RID: 2076
		// (get) Token: 0x06001A4E RID: 6734 RVA: 0x00048BCF File Offset: 0x00046DCF
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int4 zzyy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new int4(this.z, this.z, this.y, this.y);
			}
		}

		// Token: 0x1700081D RID: 2077
		// (get) Token: 0x06001A4F RID: 6735 RVA: 0x00048BEE File Offset: 0x00046DEE
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int4 zzyz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new int4(this.z, this.z, this.y, this.z);
			}
		}

		// Token: 0x1700081E RID: 2078
		// (get) Token: 0x06001A50 RID: 6736 RVA: 0x00048C0D File Offset: 0x00046E0D
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int4 zzzx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new int4(this.z, this.z, this.z, this.x);
			}
		}

		// Token: 0x1700081F RID: 2079
		// (get) Token: 0x06001A51 RID: 6737 RVA: 0x00048C2C File Offset: 0x00046E2C
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int4 zzzy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new int4(this.z, this.z, this.z, this.y);
			}
		}

		// Token: 0x17000820 RID: 2080
		// (get) Token: 0x06001A52 RID: 6738 RVA: 0x00048C4B File Offset: 0x00046E4B
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int4 zzzz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new int4(this.z, this.z, this.z, this.z);
			}
		}

		// Token: 0x17000821 RID: 2081
		// (get) Token: 0x06001A53 RID: 6739 RVA: 0x00048C6A File Offset: 0x00046E6A
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int3 xxx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new int3(this.x, this.x, this.x);
			}
		}

		// Token: 0x17000822 RID: 2082
		// (get) Token: 0x06001A54 RID: 6740 RVA: 0x00048C83 File Offset: 0x00046E83
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int3 xxy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new int3(this.x, this.x, this.y);
			}
		}

		// Token: 0x17000823 RID: 2083
		// (get) Token: 0x06001A55 RID: 6741 RVA: 0x00048C9C File Offset: 0x00046E9C
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int3 xxz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new int3(this.x, this.x, this.z);
			}
		}

		// Token: 0x17000824 RID: 2084
		// (get) Token: 0x06001A56 RID: 6742 RVA: 0x00048CB5 File Offset: 0x00046EB5
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int3 xyx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new int3(this.x, this.y, this.x);
			}
		}

		// Token: 0x17000825 RID: 2085
		// (get) Token: 0x06001A57 RID: 6743 RVA: 0x00048CCE File Offset: 0x00046ECE
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int3 xyy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new int3(this.x, this.y, this.y);
			}
		}

		// Token: 0x17000826 RID: 2086
		// (get) Token: 0x06001A58 RID: 6744 RVA: 0x00048CE7 File Offset: 0x00046EE7
		// (set) Token: 0x06001A59 RID: 6745 RVA: 0x00048D00 File Offset: 0x00046F00
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

		// Token: 0x17000827 RID: 2087
		// (get) Token: 0x06001A5A RID: 6746 RVA: 0x00048D26 File Offset: 0x00046F26
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int3 xzx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new int3(this.x, this.z, this.x);
			}
		}

		// Token: 0x17000828 RID: 2088
		// (get) Token: 0x06001A5B RID: 6747 RVA: 0x00048D3F File Offset: 0x00046F3F
		// (set) Token: 0x06001A5C RID: 6748 RVA: 0x00048D58 File Offset: 0x00046F58
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

		// Token: 0x17000829 RID: 2089
		// (get) Token: 0x06001A5D RID: 6749 RVA: 0x00048D7E File Offset: 0x00046F7E
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int3 xzz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new int3(this.x, this.z, this.z);
			}
		}

		// Token: 0x1700082A RID: 2090
		// (get) Token: 0x06001A5E RID: 6750 RVA: 0x00048D97 File Offset: 0x00046F97
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int3 yxx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new int3(this.y, this.x, this.x);
			}
		}

		// Token: 0x1700082B RID: 2091
		// (get) Token: 0x06001A5F RID: 6751 RVA: 0x00048DB0 File Offset: 0x00046FB0
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int3 yxy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new int3(this.y, this.x, this.y);
			}
		}

		// Token: 0x1700082C RID: 2092
		// (get) Token: 0x06001A60 RID: 6752 RVA: 0x00048DC9 File Offset: 0x00046FC9
		// (set) Token: 0x06001A61 RID: 6753 RVA: 0x00048DE2 File Offset: 0x00046FE2
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

		// Token: 0x1700082D RID: 2093
		// (get) Token: 0x06001A62 RID: 6754 RVA: 0x00048E08 File Offset: 0x00047008
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int3 yyx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new int3(this.y, this.y, this.x);
			}
		}

		// Token: 0x1700082E RID: 2094
		// (get) Token: 0x06001A63 RID: 6755 RVA: 0x00048E21 File Offset: 0x00047021
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int3 yyy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new int3(this.y, this.y, this.y);
			}
		}

		// Token: 0x1700082F RID: 2095
		// (get) Token: 0x06001A64 RID: 6756 RVA: 0x00048E3A File Offset: 0x0004703A
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int3 yyz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new int3(this.y, this.y, this.z);
			}
		}

		// Token: 0x17000830 RID: 2096
		// (get) Token: 0x06001A65 RID: 6757 RVA: 0x00048E53 File Offset: 0x00047053
		// (set) Token: 0x06001A66 RID: 6758 RVA: 0x00048E6C File Offset: 0x0004706C
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

		// Token: 0x17000831 RID: 2097
		// (get) Token: 0x06001A67 RID: 6759 RVA: 0x00048E92 File Offset: 0x00047092
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int3 yzy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new int3(this.y, this.z, this.y);
			}
		}

		// Token: 0x17000832 RID: 2098
		// (get) Token: 0x06001A68 RID: 6760 RVA: 0x00048EAB File Offset: 0x000470AB
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int3 yzz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new int3(this.y, this.z, this.z);
			}
		}

		// Token: 0x17000833 RID: 2099
		// (get) Token: 0x06001A69 RID: 6761 RVA: 0x00048EC4 File Offset: 0x000470C4
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int3 zxx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new int3(this.z, this.x, this.x);
			}
		}

		// Token: 0x17000834 RID: 2100
		// (get) Token: 0x06001A6A RID: 6762 RVA: 0x00048EDD File Offset: 0x000470DD
		// (set) Token: 0x06001A6B RID: 6763 RVA: 0x00048EF6 File Offset: 0x000470F6
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

		// Token: 0x17000835 RID: 2101
		// (get) Token: 0x06001A6C RID: 6764 RVA: 0x00048F1C File Offset: 0x0004711C
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int3 zxz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new int3(this.z, this.x, this.z);
			}
		}

		// Token: 0x17000836 RID: 2102
		// (get) Token: 0x06001A6D RID: 6765 RVA: 0x00048F35 File Offset: 0x00047135
		// (set) Token: 0x06001A6E RID: 6766 RVA: 0x00048F4E File Offset: 0x0004714E
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

		// Token: 0x17000837 RID: 2103
		// (get) Token: 0x06001A6F RID: 6767 RVA: 0x00048F74 File Offset: 0x00047174
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int3 zyy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new int3(this.z, this.y, this.y);
			}
		}

		// Token: 0x17000838 RID: 2104
		// (get) Token: 0x06001A70 RID: 6768 RVA: 0x00048F8D File Offset: 0x0004718D
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int3 zyz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new int3(this.z, this.y, this.z);
			}
		}

		// Token: 0x17000839 RID: 2105
		// (get) Token: 0x06001A71 RID: 6769 RVA: 0x00048FA6 File Offset: 0x000471A6
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int3 zzx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new int3(this.z, this.z, this.x);
			}
		}

		// Token: 0x1700083A RID: 2106
		// (get) Token: 0x06001A72 RID: 6770 RVA: 0x00048FBF File Offset: 0x000471BF
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int3 zzy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new int3(this.z, this.z, this.y);
			}
		}

		// Token: 0x1700083B RID: 2107
		// (get) Token: 0x06001A73 RID: 6771 RVA: 0x00048FD8 File Offset: 0x000471D8
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int3 zzz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new int3(this.z, this.z, this.z);
			}
		}

		// Token: 0x1700083C RID: 2108
		// (get) Token: 0x06001A74 RID: 6772 RVA: 0x00048FF1 File Offset: 0x000471F1
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int2 xx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new int2(this.x, this.x);
			}
		}

		// Token: 0x1700083D RID: 2109
		// (get) Token: 0x06001A75 RID: 6773 RVA: 0x00049004 File Offset: 0x00047204
		// (set) Token: 0x06001A76 RID: 6774 RVA: 0x00049017 File Offset: 0x00047217
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

		// Token: 0x1700083E RID: 2110
		// (get) Token: 0x06001A77 RID: 6775 RVA: 0x00049031 File Offset: 0x00047231
		// (set) Token: 0x06001A78 RID: 6776 RVA: 0x00049044 File Offset: 0x00047244
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

		// Token: 0x1700083F RID: 2111
		// (get) Token: 0x06001A79 RID: 6777 RVA: 0x0004905E File Offset: 0x0004725E
		// (set) Token: 0x06001A7A RID: 6778 RVA: 0x00049071 File Offset: 0x00047271
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

		// Token: 0x17000840 RID: 2112
		// (get) Token: 0x06001A7B RID: 6779 RVA: 0x0004908B File Offset: 0x0004728B
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int2 yy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new int2(this.y, this.y);
			}
		}

		// Token: 0x17000841 RID: 2113
		// (get) Token: 0x06001A7C RID: 6780 RVA: 0x0004909E File Offset: 0x0004729E
		// (set) Token: 0x06001A7D RID: 6781 RVA: 0x000490B1 File Offset: 0x000472B1
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

		// Token: 0x17000842 RID: 2114
		// (get) Token: 0x06001A7E RID: 6782 RVA: 0x000490CB File Offset: 0x000472CB
		// (set) Token: 0x06001A7F RID: 6783 RVA: 0x000490DE File Offset: 0x000472DE
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

		// Token: 0x17000843 RID: 2115
		// (get) Token: 0x06001A80 RID: 6784 RVA: 0x000490F8 File Offset: 0x000472F8
		// (set) Token: 0x06001A81 RID: 6785 RVA: 0x0004910B File Offset: 0x0004730B
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

		// Token: 0x17000844 RID: 2116
		// (get) Token: 0x06001A82 RID: 6786 RVA: 0x00049125 File Offset: 0x00047325
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int2 zz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new int2(this.z, this.z);
			}
		}

		// Token: 0x17000845 RID: 2117
		public unsafe int this[int index]
		{
			get
			{
				fixed (int3* ptr = &this)
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

		// Token: 0x06001A85 RID: 6789 RVA: 0x00049170 File Offset: 0x00047370
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool Equals(int3 rhs)
		{
			return this.x == rhs.x && this.y == rhs.y && this.z == rhs.z;
		}

		// Token: 0x06001A86 RID: 6790 RVA: 0x000491A0 File Offset: 0x000473A0
		public override bool Equals(object o)
		{
			if (o is int3)
			{
				int3 @int = (int3)o;
				return this.Equals(@int);
			}
			return false;
		}

		// Token: 0x06001A87 RID: 6791 RVA: 0x000491C5 File Offset: 0x000473C5
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public override int GetHashCode()
		{
			return (int)math.hash(this);
		}

		// Token: 0x06001A88 RID: 6792 RVA: 0x000491D2 File Offset: 0x000473D2
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public override string ToString()
		{
			return string.Format("int3({0}, {1}, {2})", this.x, this.y, this.z);
		}

		// Token: 0x06001A89 RID: 6793 RVA: 0x000491FF File Offset: 0x000473FF
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public string ToString(string format, IFormatProvider formatProvider)
		{
			return string.Format("int3({0}, {1}, {2})", this.x.ToString(format, formatProvider), this.y.ToString(format, formatProvider), this.z.ToString(format, formatProvider));
		}

		// Token: 0x040000CD RID: 205
		public int x;

		// Token: 0x040000CE RID: 206
		public int y;

		// Token: 0x040000CF RID: 207
		public int z;

		// Token: 0x040000D0 RID: 208
		public static readonly int3 zero;

		// Token: 0x02000062 RID: 98
		internal sealed class DebuggerProxy
		{
			// Token: 0x060024CF RID: 9423 RVA: 0x0006932C File Offset: 0x0006752C
			public DebuggerProxy(int3 v)
			{
				this.x = v.x;
				this.y = v.y;
				this.z = v.z;
			}

			// Token: 0x04000170 RID: 368
			public int x;

			// Token: 0x04000171 RID: 369
			public int y;

			// Token: 0x04000172 RID: 370
			public int z;
		}
	}
}
