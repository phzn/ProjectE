using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Unity.IL2CPP.CompilerServices;

namespace Unity.Mathematics
{
	// Token: 0x02000041 RID: 65
	[DebuggerTypeProxy(typeof(uint2.DebuggerProxy))]
	[Il2CppEagerStaticClassConstruction]
	[Serializable]
	public struct uint2 : IEquatable<uint2>, IFormattable
	{
		// Token: 0x06001EE1 RID: 7905 RVA: 0x0005A5F0 File Offset: 0x000587F0
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public uint2(uint x, uint y)
		{
			this.x = x;
			this.y = y;
		}

		// Token: 0x06001EE2 RID: 7906 RVA: 0x0005A600 File Offset: 0x00058800
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public uint2(uint2 xy)
		{
			this.x = xy.x;
			this.y = xy.y;
		}

		// Token: 0x06001EE3 RID: 7907 RVA: 0x0005A61A File Offset: 0x0005881A
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public uint2(uint v)
		{
			this.x = v;
			this.y = v;
		}

		// Token: 0x06001EE4 RID: 7908 RVA: 0x0005A62A File Offset: 0x0005882A
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public uint2(bool v)
		{
			this.x = (v ? 1U : 0U);
			this.y = (v ? 1U : 0U);
		}

		// Token: 0x06001EE5 RID: 7909 RVA: 0x0005A646 File Offset: 0x00058846
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public uint2(bool2 v)
		{
			this.x = (v.x ? 1U : 0U);
			this.y = (v.y ? 1U : 0U);
		}

		// Token: 0x06001EE6 RID: 7910 RVA: 0x0005A66C File Offset: 0x0005886C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public uint2(int v)
		{
			this.x = (uint)v;
			this.y = (uint)v;
		}

		// Token: 0x06001EE7 RID: 7911 RVA: 0x0005A67C File Offset: 0x0005887C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public uint2(int2 v)
		{
			this.x = (uint)v.x;
			this.y = (uint)v.y;
		}

		// Token: 0x06001EE8 RID: 7912 RVA: 0x0005A696 File Offset: 0x00058896
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public uint2(float v)
		{
			this.x = (uint)v;
			this.y = (uint)v;
		}

		// Token: 0x06001EE9 RID: 7913 RVA: 0x0005A6A8 File Offset: 0x000588A8
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public uint2(float2 v)
		{
			this.x = (uint)v.x;
			this.y = (uint)v.y;
		}

		// Token: 0x06001EEA RID: 7914 RVA: 0x0005A6C4 File Offset: 0x000588C4
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public uint2(double v)
		{
			this.x = (uint)v;
			this.y = (uint)v;
		}

		// Token: 0x06001EEB RID: 7915 RVA: 0x0005A6D6 File Offset: 0x000588D6
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public uint2(double2 v)
		{
			this.x = (uint)v.x;
			this.y = (uint)v.y;
		}

		// Token: 0x06001EEC RID: 7916 RVA: 0x0005A6F2 File Offset: 0x000588F2
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static implicit operator uint2(uint v)
		{
			return new uint2(v);
		}

		// Token: 0x06001EED RID: 7917 RVA: 0x0005A6FA File Offset: 0x000588FA
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static explicit operator uint2(bool v)
		{
			return new uint2(v);
		}

		// Token: 0x06001EEE RID: 7918 RVA: 0x0005A702 File Offset: 0x00058902
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static explicit operator uint2(bool2 v)
		{
			return new uint2(v);
		}

		// Token: 0x06001EEF RID: 7919 RVA: 0x0005A70A File Offset: 0x0005890A
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static explicit operator uint2(int v)
		{
			return new uint2(v);
		}

		// Token: 0x06001EF0 RID: 7920 RVA: 0x0005A712 File Offset: 0x00058912
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static explicit operator uint2(int2 v)
		{
			return new uint2(v);
		}

		// Token: 0x06001EF1 RID: 7921 RVA: 0x0005A71A File Offset: 0x0005891A
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static explicit operator uint2(float v)
		{
			return new uint2(v);
		}

		// Token: 0x06001EF2 RID: 7922 RVA: 0x0005A722 File Offset: 0x00058922
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static explicit operator uint2(float2 v)
		{
			return new uint2(v);
		}

		// Token: 0x06001EF3 RID: 7923 RVA: 0x0005A72A File Offset: 0x0005892A
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static explicit operator uint2(double v)
		{
			return new uint2(v);
		}

		// Token: 0x06001EF4 RID: 7924 RVA: 0x0005A732 File Offset: 0x00058932
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static explicit operator uint2(double2 v)
		{
			return new uint2(v);
		}

		// Token: 0x06001EF5 RID: 7925 RVA: 0x0005A73A File Offset: 0x0005893A
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint2 operator *(uint2 lhs, uint2 rhs)
		{
			return new uint2(lhs.x * rhs.x, lhs.y * rhs.y);
		}

		// Token: 0x06001EF6 RID: 7926 RVA: 0x0005A75B File Offset: 0x0005895B
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint2 operator *(uint2 lhs, uint rhs)
		{
			return new uint2(lhs.x * rhs, lhs.y * rhs);
		}

		// Token: 0x06001EF7 RID: 7927 RVA: 0x0005A772 File Offset: 0x00058972
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint2 operator *(uint lhs, uint2 rhs)
		{
			return new uint2(lhs * rhs.x, lhs * rhs.y);
		}

		// Token: 0x06001EF8 RID: 7928 RVA: 0x0005A789 File Offset: 0x00058989
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint2 operator +(uint2 lhs, uint2 rhs)
		{
			return new uint2(lhs.x + rhs.x, lhs.y + rhs.y);
		}

		// Token: 0x06001EF9 RID: 7929 RVA: 0x0005A7AA File Offset: 0x000589AA
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint2 operator +(uint2 lhs, uint rhs)
		{
			return new uint2(lhs.x + rhs, lhs.y + rhs);
		}

		// Token: 0x06001EFA RID: 7930 RVA: 0x0005A7C1 File Offset: 0x000589C1
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint2 operator +(uint lhs, uint2 rhs)
		{
			return new uint2(lhs + rhs.x, lhs + rhs.y);
		}

		// Token: 0x06001EFB RID: 7931 RVA: 0x0005A7D8 File Offset: 0x000589D8
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint2 operator -(uint2 lhs, uint2 rhs)
		{
			return new uint2(lhs.x - rhs.x, lhs.y - rhs.y);
		}

		// Token: 0x06001EFC RID: 7932 RVA: 0x0005A7F9 File Offset: 0x000589F9
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint2 operator -(uint2 lhs, uint rhs)
		{
			return new uint2(lhs.x - rhs, lhs.y - rhs);
		}

		// Token: 0x06001EFD RID: 7933 RVA: 0x0005A810 File Offset: 0x00058A10
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint2 operator -(uint lhs, uint2 rhs)
		{
			return new uint2(lhs - rhs.x, lhs - rhs.y);
		}

		// Token: 0x06001EFE RID: 7934 RVA: 0x0005A827 File Offset: 0x00058A27
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint2 operator /(uint2 lhs, uint2 rhs)
		{
			return new uint2(lhs.x / rhs.x, lhs.y / rhs.y);
		}

		// Token: 0x06001EFF RID: 7935 RVA: 0x0005A848 File Offset: 0x00058A48
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint2 operator /(uint2 lhs, uint rhs)
		{
			return new uint2(lhs.x / rhs, lhs.y / rhs);
		}

		// Token: 0x06001F00 RID: 7936 RVA: 0x0005A85F File Offset: 0x00058A5F
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint2 operator /(uint lhs, uint2 rhs)
		{
			return new uint2(lhs / rhs.x, lhs / rhs.y);
		}

		// Token: 0x06001F01 RID: 7937 RVA: 0x0005A876 File Offset: 0x00058A76
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint2 operator %(uint2 lhs, uint2 rhs)
		{
			return new uint2(lhs.x % rhs.x, lhs.y % rhs.y);
		}

		// Token: 0x06001F02 RID: 7938 RVA: 0x0005A897 File Offset: 0x00058A97
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint2 operator %(uint2 lhs, uint rhs)
		{
			return new uint2(lhs.x % rhs, lhs.y % rhs);
		}

		// Token: 0x06001F03 RID: 7939 RVA: 0x0005A8AE File Offset: 0x00058AAE
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint2 operator %(uint lhs, uint2 rhs)
		{
			return new uint2(lhs % rhs.x, lhs % rhs.y);
		}

		// Token: 0x06001F04 RID: 7940 RVA: 0x0005A8C8 File Offset: 0x00058AC8
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint2 operator ++(uint2 val)
		{
			uint num = val.x + 1U;
			val.x = num;
			uint num2 = num;
			num = val.y + 1U;
			val.y = num;
			return new uint2(num2, num);
		}

		// Token: 0x06001F05 RID: 7941 RVA: 0x0005A8F8 File Offset: 0x00058AF8
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint2 operator --(uint2 val)
		{
			uint num = val.x - 1U;
			val.x = num;
			uint num2 = num;
			num = val.y - 1U;
			val.y = num;
			return new uint2(num2, num);
		}

		// Token: 0x06001F06 RID: 7942 RVA: 0x0005A928 File Offset: 0x00058B28
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool2 operator <(uint2 lhs, uint2 rhs)
		{
			return new bool2(lhs.x < rhs.x, lhs.y < rhs.y);
		}

		// Token: 0x06001F07 RID: 7943 RVA: 0x0005A94B File Offset: 0x00058B4B
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool2 operator <(uint2 lhs, uint rhs)
		{
			return new bool2(lhs.x < rhs, lhs.y < rhs);
		}

		// Token: 0x06001F08 RID: 7944 RVA: 0x0005A964 File Offset: 0x00058B64
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool2 operator <(uint lhs, uint2 rhs)
		{
			return new bool2(lhs < rhs.x, lhs < rhs.y);
		}

		// Token: 0x06001F09 RID: 7945 RVA: 0x0005A97D File Offset: 0x00058B7D
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool2 operator <=(uint2 lhs, uint2 rhs)
		{
			return new bool2(lhs.x <= rhs.x, lhs.y <= rhs.y);
		}

		// Token: 0x06001F0A RID: 7946 RVA: 0x0005A9A6 File Offset: 0x00058BA6
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool2 operator <=(uint2 lhs, uint rhs)
		{
			return new bool2(lhs.x <= rhs, lhs.y <= rhs);
		}

		// Token: 0x06001F0B RID: 7947 RVA: 0x0005A9C5 File Offset: 0x00058BC5
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool2 operator <=(uint lhs, uint2 rhs)
		{
			return new bool2(lhs <= rhs.x, lhs <= rhs.y);
		}

		// Token: 0x06001F0C RID: 7948 RVA: 0x0005A9E4 File Offset: 0x00058BE4
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool2 operator >(uint2 lhs, uint2 rhs)
		{
			return new bool2(lhs.x > rhs.x, lhs.y > rhs.y);
		}

		// Token: 0x06001F0D RID: 7949 RVA: 0x0005AA07 File Offset: 0x00058C07
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool2 operator >(uint2 lhs, uint rhs)
		{
			return new bool2(lhs.x > rhs, lhs.y > rhs);
		}

		// Token: 0x06001F0E RID: 7950 RVA: 0x0005AA20 File Offset: 0x00058C20
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool2 operator >(uint lhs, uint2 rhs)
		{
			return new bool2(lhs > rhs.x, lhs > rhs.y);
		}

		// Token: 0x06001F0F RID: 7951 RVA: 0x0005AA39 File Offset: 0x00058C39
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool2 operator >=(uint2 lhs, uint2 rhs)
		{
			return new bool2(lhs.x >= rhs.x, lhs.y >= rhs.y);
		}

		// Token: 0x06001F10 RID: 7952 RVA: 0x0005AA62 File Offset: 0x00058C62
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool2 operator >=(uint2 lhs, uint rhs)
		{
			return new bool2(lhs.x >= rhs, lhs.y >= rhs);
		}

		// Token: 0x06001F11 RID: 7953 RVA: 0x0005AA81 File Offset: 0x00058C81
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool2 operator >=(uint lhs, uint2 rhs)
		{
			return new bool2(lhs >= rhs.x, lhs >= rhs.y);
		}

		// Token: 0x06001F12 RID: 7954 RVA: 0x0005AAA0 File Offset: 0x00058CA0
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint2 operator -(uint2 val)
		{
			return new uint2((uint)(-(uint)((ulong)val.x)), (uint)(-(uint)((ulong)val.y)));
		}

		// Token: 0x06001F13 RID: 7955 RVA: 0x0005AAB9 File Offset: 0x00058CB9
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint2 operator +(uint2 val)
		{
			return new uint2(val.x, val.y);
		}

		// Token: 0x06001F14 RID: 7956 RVA: 0x0005AACC File Offset: 0x00058CCC
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint2 operator <<(uint2 x, int n)
		{
			return new uint2(x.x << n, x.y << n);
		}

		// Token: 0x06001F15 RID: 7957 RVA: 0x0005AAE9 File Offset: 0x00058CE9
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint2 operator >>(uint2 x, int n)
		{
			return new uint2(x.x >> n, x.y >> n);
		}

		// Token: 0x06001F16 RID: 7958 RVA: 0x0005AB06 File Offset: 0x00058D06
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool2 operator ==(uint2 lhs, uint2 rhs)
		{
			return new bool2(lhs.x == rhs.x, lhs.y == rhs.y);
		}

		// Token: 0x06001F17 RID: 7959 RVA: 0x0005AB29 File Offset: 0x00058D29
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool2 operator ==(uint2 lhs, uint rhs)
		{
			return new bool2(lhs.x == rhs, lhs.y == rhs);
		}

		// Token: 0x06001F18 RID: 7960 RVA: 0x0005AB42 File Offset: 0x00058D42
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool2 operator ==(uint lhs, uint2 rhs)
		{
			return new bool2(lhs == rhs.x, lhs == rhs.y);
		}

		// Token: 0x06001F19 RID: 7961 RVA: 0x0005AB5B File Offset: 0x00058D5B
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool2 operator !=(uint2 lhs, uint2 rhs)
		{
			return new bool2(lhs.x != rhs.x, lhs.y != rhs.y);
		}

		// Token: 0x06001F1A RID: 7962 RVA: 0x0005AB84 File Offset: 0x00058D84
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool2 operator !=(uint2 lhs, uint rhs)
		{
			return new bool2(lhs.x != rhs, lhs.y != rhs);
		}

		// Token: 0x06001F1B RID: 7963 RVA: 0x0005ABA3 File Offset: 0x00058DA3
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool2 operator !=(uint lhs, uint2 rhs)
		{
			return new bool2(lhs != rhs.x, lhs != rhs.y);
		}

		// Token: 0x06001F1C RID: 7964 RVA: 0x0005ABC2 File Offset: 0x00058DC2
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint2 operator ~(uint2 val)
		{
			return new uint2(~val.x, ~val.y);
		}

		// Token: 0x06001F1D RID: 7965 RVA: 0x0005ABD7 File Offset: 0x00058DD7
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint2 operator &(uint2 lhs, uint2 rhs)
		{
			return new uint2(lhs.x & rhs.x, lhs.y & rhs.y);
		}

		// Token: 0x06001F1E RID: 7966 RVA: 0x0005ABF8 File Offset: 0x00058DF8
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint2 operator &(uint2 lhs, uint rhs)
		{
			return new uint2(lhs.x & rhs, lhs.y & rhs);
		}

		// Token: 0x06001F1F RID: 7967 RVA: 0x0005AC0F File Offset: 0x00058E0F
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint2 operator &(uint lhs, uint2 rhs)
		{
			return new uint2(lhs & rhs.x, lhs & rhs.y);
		}

		// Token: 0x06001F20 RID: 7968 RVA: 0x0005AC26 File Offset: 0x00058E26
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint2 operator |(uint2 lhs, uint2 rhs)
		{
			return new uint2(lhs.x | rhs.x, lhs.y | rhs.y);
		}

		// Token: 0x06001F21 RID: 7969 RVA: 0x0005AC47 File Offset: 0x00058E47
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint2 operator |(uint2 lhs, uint rhs)
		{
			return new uint2(lhs.x | rhs, lhs.y | rhs);
		}

		// Token: 0x06001F22 RID: 7970 RVA: 0x0005AC5E File Offset: 0x00058E5E
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint2 operator |(uint lhs, uint2 rhs)
		{
			return new uint2(lhs | rhs.x, lhs | rhs.y);
		}

		// Token: 0x06001F23 RID: 7971 RVA: 0x0005AC75 File Offset: 0x00058E75
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint2 operator ^(uint2 lhs, uint2 rhs)
		{
			return new uint2(lhs.x ^ rhs.x, lhs.y ^ rhs.y);
		}

		// Token: 0x06001F24 RID: 7972 RVA: 0x0005AC96 File Offset: 0x00058E96
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint2 operator ^(uint2 lhs, uint rhs)
		{
			return new uint2(lhs.x ^ rhs, lhs.y ^ rhs);
		}

		// Token: 0x06001F25 RID: 7973 RVA: 0x0005ACAD File Offset: 0x00058EAD
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint2 operator ^(uint lhs, uint2 rhs)
		{
			return new uint2(lhs ^ rhs.x, lhs ^ rhs.y);
		}

		// Token: 0x1700099D RID: 2461
		// (get) Token: 0x06001F26 RID: 7974 RVA: 0x0005ACC4 File Offset: 0x00058EC4
		[EditorBrowsable(EditorBrowsableState.Never)]
		public uint4 xxxx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new uint4(this.x, this.x, this.x, this.x);
			}
		}

		// Token: 0x1700099E RID: 2462
		// (get) Token: 0x06001F27 RID: 7975 RVA: 0x0005ACE3 File Offset: 0x00058EE3
		[EditorBrowsable(EditorBrowsableState.Never)]
		public uint4 xxxy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new uint4(this.x, this.x, this.x, this.y);
			}
		}

		// Token: 0x1700099F RID: 2463
		// (get) Token: 0x06001F28 RID: 7976 RVA: 0x0005AD02 File Offset: 0x00058F02
		[EditorBrowsable(EditorBrowsableState.Never)]
		public uint4 xxyx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new uint4(this.x, this.x, this.y, this.x);
			}
		}

		// Token: 0x170009A0 RID: 2464
		// (get) Token: 0x06001F29 RID: 7977 RVA: 0x0005AD21 File Offset: 0x00058F21
		[EditorBrowsable(EditorBrowsableState.Never)]
		public uint4 xxyy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new uint4(this.x, this.x, this.y, this.y);
			}
		}

		// Token: 0x170009A1 RID: 2465
		// (get) Token: 0x06001F2A RID: 7978 RVA: 0x0005AD40 File Offset: 0x00058F40
		[EditorBrowsable(EditorBrowsableState.Never)]
		public uint4 xyxx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new uint4(this.x, this.y, this.x, this.x);
			}
		}

		// Token: 0x170009A2 RID: 2466
		// (get) Token: 0x06001F2B RID: 7979 RVA: 0x0005AD5F File Offset: 0x00058F5F
		[EditorBrowsable(EditorBrowsableState.Never)]
		public uint4 xyxy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new uint4(this.x, this.y, this.x, this.y);
			}
		}

		// Token: 0x170009A3 RID: 2467
		// (get) Token: 0x06001F2C RID: 7980 RVA: 0x0005AD7E File Offset: 0x00058F7E
		[EditorBrowsable(EditorBrowsableState.Never)]
		public uint4 xyyx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new uint4(this.x, this.y, this.y, this.x);
			}
		}

		// Token: 0x170009A4 RID: 2468
		// (get) Token: 0x06001F2D RID: 7981 RVA: 0x0005AD9D File Offset: 0x00058F9D
		[EditorBrowsable(EditorBrowsableState.Never)]
		public uint4 xyyy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new uint4(this.x, this.y, this.y, this.y);
			}
		}

		// Token: 0x170009A5 RID: 2469
		// (get) Token: 0x06001F2E RID: 7982 RVA: 0x0005ADBC File Offset: 0x00058FBC
		[EditorBrowsable(EditorBrowsableState.Never)]
		public uint4 yxxx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new uint4(this.y, this.x, this.x, this.x);
			}
		}

		// Token: 0x170009A6 RID: 2470
		// (get) Token: 0x06001F2F RID: 7983 RVA: 0x0005ADDB File Offset: 0x00058FDB
		[EditorBrowsable(EditorBrowsableState.Never)]
		public uint4 yxxy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new uint4(this.y, this.x, this.x, this.y);
			}
		}

		// Token: 0x170009A7 RID: 2471
		// (get) Token: 0x06001F30 RID: 7984 RVA: 0x0005ADFA File Offset: 0x00058FFA
		[EditorBrowsable(EditorBrowsableState.Never)]
		public uint4 yxyx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new uint4(this.y, this.x, this.y, this.x);
			}
		}

		// Token: 0x170009A8 RID: 2472
		// (get) Token: 0x06001F31 RID: 7985 RVA: 0x0005AE19 File Offset: 0x00059019
		[EditorBrowsable(EditorBrowsableState.Never)]
		public uint4 yxyy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new uint4(this.y, this.x, this.y, this.y);
			}
		}

		// Token: 0x170009A9 RID: 2473
		// (get) Token: 0x06001F32 RID: 7986 RVA: 0x0005AE38 File Offset: 0x00059038
		[EditorBrowsable(EditorBrowsableState.Never)]
		public uint4 yyxx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new uint4(this.y, this.y, this.x, this.x);
			}
		}

		// Token: 0x170009AA RID: 2474
		// (get) Token: 0x06001F33 RID: 7987 RVA: 0x0005AE57 File Offset: 0x00059057
		[EditorBrowsable(EditorBrowsableState.Never)]
		public uint4 yyxy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new uint4(this.y, this.y, this.x, this.y);
			}
		}

		// Token: 0x170009AB RID: 2475
		// (get) Token: 0x06001F34 RID: 7988 RVA: 0x0005AE76 File Offset: 0x00059076
		[EditorBrowsable(EditorBrowsableState.Never)]
		public uint4 yyyx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new uint4(this.y, this.y, this.y, this.x);
			}
		}

		// Token: 0x170009AC RID: 2476
		// (get) Token: 0x06001F35 RID: 7989 RVA: 0x0005AE95 File Offset: 0x00059095
		[EditorBrowsable(EditorBrowsableState.Never)]
		public uint4 yyyy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new uint4(this.y, this.y, this.y, this.y);
			}
		}

		// Token: 0x170009AD RID: 2477
		// (get) Token: 0x06001F36 RID: 7990 RVA: 0x0005AEB4 File Offset: 0x000590B4
		[EditorBrowsable(EditorBrowsableState.Never)]
		public uint3 xxx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new uint3(this.x, this.x, this.x);
			}
		}

		// Token: 0x170009AE RID: 2478
		// (get) Token: 0x06001F37 RID: 7991 RVA: 0x0005AECD File Offset: 0x000590CD
		[EditorBrowsable(EditorBrowsableState.Never)]
		public uint3 xxy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new uint3(this.x, this.x, this.y);
			}
		}

		// Token: 0x170009AF RID: 2479
		// (get) Token: 0x06001F38 RID: 7992 RVA: 0x0005AEE6 File Offset: 0x000590E6
		[EditorBrowsable(EditorBrowsableState.Never)]
		public uint3 xyx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new uint3(this.x, this.y, this.x);
			}
		}

		// Token: 0x170009B0 RID: 2480
		// (get) Token: 0x06001F39 RID: 7993 RVA: 0x0005AEFF File Offset: 0x000590FF
		[EditorBrowsable(EditorBrowsableState.Never)]
		public uint3 xyy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new uint3(this.x, this.y, this.y);
			}
		}

		// Token: 0x170009B1 RID: 2481
		// (get) Token: 0x06001F3A RID: 7994 RVA: 0x0005AF18 File Offset: 0x00059118
		[EditorBrowsable(EditorBrowsableState.Never)]
		public uint3 yxx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new uint3(this.y, this.x, this.x);
			}
		}

		// Token: 0x170009B2 RID: 2482
		// (get) Token: 0x06001F3B RID: 7995 RVA: 0x0005AF31 File Offset: 0x00059131
		[EditorBrowsable(EditorBrowsableState.Never)]
		public uint3 yxy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new uint3(this.y, this.x, this.y);
			}
		}

		// Token: 0x170009B3 RID: 2483
		// (get) Token: 0x06001F3C RID: 7996 RVA: 0x0005AF4A File Offset: 0x0005914A
		[EditorBrowsable(EditorBrowsableState.Never)]
		public uint3 yyx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new uint3(this.y, this.y, this.x);
			}
		}

		// Token: 0x170009B4 RID: 2484
		// (get) Token: 0x06001F3D RID: 7997 RVA: 0x0005AF63 File Offset: 0x00059163
		[EditorBrowsable(EditorBrowsableState.Never)]
		public uint3 yyy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new uint3(this.y, this.y, this.y);
			}
		}

		// Token: 0x170009B5 RID: 2485
		// (get) Token: 0x06001F3E RID: 7998 RVA: 0x0005AF7C File Offset: 0x0005917C
		[EditorBrowsable(EditorBrowsableState.Never)]
		public uint2 xx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new uint2(this.x, this.x);
			}
		}

		// Token: 0x170009B6 RID: 2486
		// (get) Token: 0x06001F3F RID: 7999 RVA: 0x0005AF8F File Offset: 0x0005918F
		// (set) Token: 0x06001F40 RID: 8000 RVA: 0x0005AFA2 File Offset: 0x000591A2
		[EditorBrowsable(EditorBrowsableState.Never)]
		public uint2 xy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new uint2(this.x, this.y);
			}
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			set
			{
				this.x = value.x;
				this.y = value.y;
			}
		}

		// Token: 0x170009B7 RID: 2487
		// (get) Token: 0x06001F41 RID: 8001 RVA: 0x0005AFBC File Offset: 0x000591BC
		// (set) Token: 0x06001F42 RID: 8002 RVA: 0x0005AFCF File Offset: 0x000591CF
		[EditorBrowsable(EditorBrowsableState.Never)]
		public uint2 yx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new uint2(this.y, this.x);
			}
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			set
			{
				this.y = value.x;
				this.x = value.y;
			}
		}

		// Token: 0x170009B8 RID: 2488
		// (get) Token: 0x06001F43 RID: 8003 RVA: 0x0005AFE9 File Offset: 0x000591E9
		[EditorBrowsable(EditorBrowsableState.Never)]
		public uint2 yy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new uint2(this.y, this.y);
			}
		}

		// Token: 0x170009B9 RID: 2489
		public unsafe uint this[int index]
		{
			get
			{
				fixed (uint2* ptr = &this)
				{
					return ((uint*)ptr)[index];
				}
			}
			set
			{
				fixed (uint* ptr = &this.x)
				{
					ptr[index] = value;
				}
			}
		}

		// Token: 0x06001F46 RID: 8006 RVA: 0x0005B034 File Offset: 0x00059234
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool Equals(uint2 rhs)
		{
			return this.x == rhs.x && this.y == rhs.y;
		}

		// Token: 0x06001F47 RID: 8007 RVA: 0x0005B054 File Offset: 0x00059254
		public override bool Equals(object o)
		{
			if (o is uint2)
			{
				uint2 @uint = (uint2)o;
				return this.Equals(@uint);
			}
			return false;
		}

		// Token: 0x06001F48 RID: 8008 RVA: 0x0005B079 File Offset: 0x00059279
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public override int GetHashCode()
		{
			return (int)math.hash(this);
		}

		// Token: 0x06001F49 RID: 8009 RVA: 0x0005B086 File Offset: 0x00059286
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public override string ToString()
		{
			return string.Format("uint2({0}, {1})", this.x, this.y);
		}

		// Token: 0x06001F4A RID: 8010 RVA: 0x0005B0A8 File Offset: 0x000592A8
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public string ToString(string format, IFormatProvider formatProvider)
		{
			return string.Format("uint2({0}, {1})", this.x.ToString(format, formatProvider), this.y.ToString(format, formatProvider));
		}

		// Token: 0x040000FA RID: 250
		public uint x;

		// Token: 0x040000FB RID: 251
		public uint y;

		// Token: 0x040000FC RID: 252
		public static readonly uint2 zero;

		// Token: 0x02000064 RID: 100
		internal sealed class DebuggerProxy
		{
			// Token: 0x060024D1 RID: 9425 RVA: 0x00069390 File Offset: 0x00067590
			public DebuggerProxy(uint2 v)
			{
				this.x = v.x;
				this.y = v.y;
			}

			// Token: 0x04000177 RID: 375
			public uint x;

			// Token: 0x04000178 RID: 376
			public uint y;
		}
	}
}
