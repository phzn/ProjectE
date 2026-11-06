using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Unity.IL2CPP.CompilerServices;

namespace Unity.Mathematics
{
	// Token: 0x0200000A RID: 10
	[DebuggerTypeProxy(typeof(bool3.DebuggerProxy))]
	[Il2CppEagerStaticClassConstruction]
	[Serializable]
	public struct bool3 : IEquatable<bool3>
	{
		// Token: 0x06000848 RID: 2120 RVA: 0x0001D0CD File Offset: 0x0001B2CD
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool3(bool x, bool y, bool z)
		{
			this.x = x;
			this.y = y;
			this.z = z;
		}

		// Token: 0x06000849 RID: 2121 RVA: 0x0001D0E4 File Offset: 0x0001B2E4
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool3(bool x, bool2 yz)
		{
			this.x = x;
			this.y = yz.x;
			this.z = yz.y;
		}

		// Token: 0x0600084A RID: 2122 RVA: 0x0001D105 File Offset: 0x0001B305
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool3(bool2 xy, bool z)
		{
			this.x = xy.x;
			this.y = xy.y;
			this.z = z;
		}

		// Token: 0x0600084B RID: 2123 RVA: 0x0001D126 File Offset: 0x0001B326
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool3(bool3 xyz)
		{
			this.x = xyz.x;
			this.y = xyz.y;
			this.z = xyz.z;
		}

		// Token: 0x0600084C RID: 2124 RVA: 0x0001D14C File Offset: 0x0001B34C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool3(bool v)
		{
			this.x = v;
			this.y = v;
			this.z = v;
		}

		// Token: 0x0600084D RID: 2125 RVA: 0x0001D163 File Offset: 0x0001B363
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static implicit operator bool3(bool v)
		{
			return new bool3(v);
		}

		// Token: 0x0600084E RID: 2126 RVA: 0x0001D16B File Offset: 0x0001B36B
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool3 operator ==(bool3 lhs, bool3 rhs)
		{
			return new bool3(lhs.x == rhs.x, lhs.y == rhs.y, lhs.z == rhs.z);
		}

		// Token: 0x0600084F RID: 2127 RVA: 0x0001D19C File Offset: 0x0001B39C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool3 operator ==(bool3 lhs, bool rhs)
		{
			return new bool3(lhs.x == rhs, lhs.y == rhs, lhs.z == rhs);
		}

		// Token: 0x06000850 RID: 2128 RVA: 0x0001D1BE File Offset: 0x0001B3BE
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool3 operator ==(bool lhs, bool3 rhs)
		{
			return new bool3(lhs == rhs.x, lhs == rhs.y, lhs == rhs.z);
		}

		// Token: 0x06000851 RID: 2129 RVA: 0x0001D1E0 File Offset: 0x0001B3E0
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool3 operator !=(bool3 lhs, bool3 rhs)
		{
			return new bool3(lhs.x != rhs.x, lhs.y != rhs.y, lhs.z != rhs.z);
		}

		// Token: 0x06000852 RID: 2130 RVA: 0x0001D21A File Offset: 0x0001B41A
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool3 operator !=(bool3 lhs, bool rhs)
		{
			return new bool3(lhs.x != rhs, lhs.y != rhs, lhs.z != rhs);
		}

		// Token: 0x06000853 RID: 2131 RVA: 0x0001D245 File Offset: 0x0001B445
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool3 operator !=(bool lhs, bool3 rhs)
		{
			return new bool3(lhs != rhs.x, lhs != rhs.y, lhs != rhs.z);
		}

		// Token: 0x06000854 RID: 2132 RVA: 0x0001D270 File Offset: 0x0001B470
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool3 operator !(bool3 val)
		{
			return new bool3(!val.x, !val.y, !val.z);
		}

		// Token: 0x06000855 RID: 2133 RVA: 0x0001D292 File Offset: 0x0001B492
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool3 operator &(bool3 lhs, bool3 rhs)
		{
			return new bool3(lhs.x & rhs.x, lhs.y & rhs.y, lhs.z & rhs.z);
		}

		// Token: 0x06000856 RID: 2134 RVA: 0x0001D2C0 File Offset: 0x0001B4C0
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool3 operator &(bool3 lhs, bool rhs)
		{
			return new bool3(lhs.x && rhs, lhs.y && rhs, lhs.z && rhs);
		}

		// Token: 0x06000857 RID: 2135 RVA: 0x0001D2DF File Offset: 0x0001B4DF
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool3 operator &(bool lhs, bool3 rhs)
		{
			return new bool3(lhs & rhs.x, lhs & rhs.y, lhs & rhs.z);
		}

		// Token: 0x06000858 RID: 2136 RVA: 0x0001D2FE File Offset: 0x0001B4FE
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool3 operator |(bool3 lhs, bool3 rhs)
		{
			return new bool3(lhs.x | rhs.x, lhs.y | rhs.y, lhs.z | rhs.z);
		}

		// Token: 0x06000859 RID: 2137 RVA: 0x0001D32C File Offset: 0x0001B52C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool3 operator |(bool3 lhs, bool rhs)
		{
			return new bool3(lhs.x || rhs, lhs.y || rhs, lhs.z || rhs);
		}

		// Token: 0x0600085A RID: 2138 RVA: 0x0001D34B File Offset: 0x0001B54B
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool3 operator |(bool lhs, bool3 rhs)
		{
			return new bool3(lhs | rhs.x, lhs | rhs.y, lhs | rhs.z);
		}

		// Token: 0x0600085B RID: 2139 RVA: 0x0001D36A File Offset: 0x0001B56A
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool3 operator ^(bool3 lhs, bool3 rhs)
		{
			return new bool3(lhs.x ^ rhs.x, lhs.y ^ rhs.y, lhs.z ^ rhs.z);
		}

		// Token: 0x0600085C RID: 2140 RVA: 0x0001D398 File Offset: 0x0001B598
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool3 operator ^(bool3 lhs, bool rhs)
		{
			return new bool3(lhs.x ^ rhs, lhs.y ^ rhs, lhs.z ^ rhs);
		}

		// Token: 0x0600085D RID: 2141 RVA: 0x0001D3B7 File Offset: 0x0001B5B7
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool3 operator ^(bool lhs, bool3 rhs)
		{
			return new bool3(lhs ^ rhs.x, lhs ^ rhs.y, lhs ^ rhs.z);
		}

		// Token: 0x17000021 RID: 33
		// (get) Token: 0x0600085E RID: 2142 RVA: 0x0001D3D6 File Offset: 0x0001B5D6
		[EditorBrowsable(EditorBrowsableState.Never)]
		public bool4 xxxx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new bool4(this.x, this.x, this.x, this.x);
			}
		}

		// Token: 0x17000022 RID: 34
		// (get) Token: 0x0600085F RID: 2143 RVA: 0x0001D3F5 File Offset: 0x0001B5F5
		[EditorBrowsable(EditorBrowsableState.Never)]
		public bool4 xxxy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new bool4(this.x, this.x, this.x, this.y);
			}
		}

		// Token: 0x17000023 RID: 35
		// (get) Token: 0x06000860 RID: 2144 RVA: 0x0001D414 File Offset: 0x0001B614
		[EditorBrowsable(EditorBrowsableState.Never)]
		public bool4 xxxz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new bool4(this.x, this.x, this.x, this.z);
			}
		}

		// Token: 0x17000024 RID: 36
		// (get) Token: 0x06000861 RID: 2145 RVA: 0x0001D433 File Offset: 0x0001B633
		[EditorBrowsable(EditorBrowsableState.Never)]
		public bool4 xxyx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new bool4(this.x, this.x, this.y, this.x);
			}
		}

		// Token: 0x17000025 RID: 37
		// (get) Token: 0x06000862 RID: 2146 RVA: 0x0001D452 File Offset: 0x0001B652
		[EditorBrowsable(EditorBrowsableState.Never)]
		public bool4 xxyy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new bool4(this.x, this.x, this.y, this.y);
			}
		}

		// Token: 0x17000026 RID: 38
		// (get) Token: 0x06000863 RID: 2147 RVA: 0x0001D471 File Offset: 0x0001B671
		[EditorBrowsable(EditorBrowsableState.Never)]
		public bool4 xxyz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new bool4(this.x, this.x, this.y, this.z);
			}
		}

		// Token: 0x17000027 RID: 39
		// (get) Token: 0x06000864 RID: 2148 RVA: 0x0001D490 File Offset: 0x0001B690
		[EditorBrowsable(EditorBrowsableState.Never)]
		public bool4 xxzx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new bool4(this.x, this.x, this.z, this.x);
			}
		}

		// Token: 0x17000028 RID: 40
		// (get) Token: 0x06000865 RID: 2149 RVA: 0x0001D4AF File Offset: 0x0001B6AF
		[EditorBrowsable(EditorBrowsableState.Never)]
		public bool4 xxzy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new bool4(this.x, this.x, this.z, this.y);
			}
		}

		// Token: 0x17000029 RID: 41
		// (get) Token: 0x06000866 RID: 2150 RVA: 0x0001D4CE File Offset: 0x0001B6CE
		[EditorBrowsable(EditorBrowsableState.Never)]
		public bool4 xxzz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new bool4(this.x, this.x, this.z, this.z);
			}
		}

		// Token: 0x1700002A RID: 42
		// (get) Token: 0x06000867 RID: 2151 RVA: 0x0001D4ED File Offset: 0x0001B6ED
		[EditorBrowsable(EditorBrowsableState.Never)]
		public bool4 xyxx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new bool4(this.x, this.y, this.x, this.x);
			}
		}

		// Token: 0x1700002B RID: 43
		// (get) Token: 0x06000868 RID: 2152 RVA: 0x0001D50C File Offset: 0x0001B70C
		[EditorBrowsable(EditorBrowsableState.Never)]
		public bool4 xyxy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new bool4(this.x, this.y, this.x, this.y);
			}
		}

		// Token: 0x1700002C RID: 44
		// (get) Token: 0x06000869 RID: 2153 RVA: 0x0001D52B File Offset: 0x0001B72B
		[EditorBrowsable(EditorBrowsableState.Never)]
		public bool4 xyxz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new bool4(this.x, this.y, this.x, this.z);
			}
		}

		// Token: 0x1700002D RID: 45
		// (get) Token: 0x0600086A RID: 2154 RVA: 0x0001D54A File Offset: 0x0001B74A
		[EditorBrowsable(EditorBrowsableState.Never)]
		public bool4 xyyx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new bool4(this.x, this.y, this.y, this.x);
			}
		}

		// Token: 0x1700002E RID: 46
		// (get) Token: 0x0600086B RID: 2155 RVA: 0x0001D569 File Offset: 0x0001B769
		[EditorBrowsable(EditorBrowsableState.Never)]
		public bool4 xyyy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new bool4(this.x, this.y, this.y, this.y);
			}
		}

		// Token: 0x1700002F RID: 47
		// (get) Token: 0x0600086C RID: 2156 RVA: 0x0001D588 File Offset: 0x0001B788
		[EditorBrowsable(EditorBrowsableState.Never)]
		public bool4 xyyz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new bool4(this.x, this.y, this.y, this.z);
			}
		}

		// Token: 0x17000030 RID: 48
		// (get) Token: 0x0600086D RID: 2157 RVA: 0x0001D5A7 File Offset: 0x0001B7A7
		[EditorBrowsable(EditorBrowsableState.Never)]
		public bool4 xyzx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new bool4(this.x, this.y, this.z, this.x);
			}
		}

		// Token: 0x17000031 RID: 49
		// (get) Token: 0x0600086E RID: 2158 RVA: 0x0001D5C6 File Offset: 0x0001B7C6
		[EditorBrowsable(EditorBrowsableState.Never)]
		public bool4 xyzy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new bool4(this.x, this.y, this.z, this.y);
			}
		}

		// Token: 0x17000032 RID: 50
		// (get) Token: 0x0600086F RID: 2159 RVA: 0x0001D5E5 File Offset: 0x0001B7E5
		[EditorBrowsable(EditorBrowsableState.Never)]
		public bool4 xyzz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new bool4(this.x, this.y, this.z, this.z);
			}
		}

		// Token: 0x17000033 RID: 51
		// (get) Token: 0x06000870 RID: 2160 RVA: 0x0001D604 File Offset: 0x0001B804
		[EditorBrowsable(EditorBrowsableState.Never)]
		public bool4 xzxx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new bool4(this.x, this.z, this.x, this.x);
			}
		}

		// Token: 0x17000034 RID: 52
		// (get) Token: 0x06000871 RID: 2161 RVA: 0x0001D623 File Offset: 0x0001B823
		[EditorBrowsable(EditorBrowsableState.Never)]
		public bool4 xzxy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new bool4(this.x, this.z, this.x, this.y);
			}
		}

		// Token: 0x17000035 RID: 53
		// (get) Token: 0x06000872 RID: 2162 RVA: 0x0001D642 File Offset: 0x0001B842
		[EditorBrowsable(EditorBrowsableState.Never)]
		public bool4 xzxz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new bool4(this.x, this.z, this.x, this.z);
			}
		}

		// Token: 0x17000036 RID: 54
		// (get) Token: 0x06000873 RID: 2163 RVA: 0x0001D661 File Offset: 0x0001B861
		[EditorBrowsable(EditorBrowsableState.Never)]
		public bool4 xzyx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new bool4(this.x, this.z, this.y, this.x);
			}
		}

		// Token: 0x17000037 RID: 55
		// (get) Token: 0x06000874 RID: 2164 RVA: 0x0001D680 File Offset: 0x0001B880
		[EditorBrowsable(EditorBrowsableState.Never)]
		public bool4 xzyy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new bool4(this.x, this.z, this.y, this.y);
			}
		}

		// Token: 0x17000038 RID: 56
		// (get) Token: 0x06000875 RID: 2165 RVA: 0x0001D69F File Offset: 0x0001B89F
		[EditorBrowsable(EditorBrowsableState.Never)]
		public bool4 xzyz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new bool4(this.x, this.z, this.y, this.z);
			}
		}

		// Token: 0x17000039 RID: 57
		// (get) Token: 0x06000876 RID: 2166 RVA: 0x0001D6BE File Offset: 0x0001B8BE
		[EditorBrowsable(EditorBrowsableState.Never)]
		public bool4 xzzx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new bool4(this.x, this.z, this.z, this.x);
			}
		}

		// Token: 0x1700003A RID: 58
		// (get) Token: 0x06000877 RID: 2167 RVA: 0x0001D6DD File Offset: 0x0001B8DD
		[EditorBrowsable(EditorBrowsableState.Never)]
		public bool4 xzzy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new bool4(this.x, this.z, this.z, this.y);
			}
		}

		// Token: 0x1700003B RID: 59
		// (get) Token: 0x06000878 RID: 2168 RVA: 0x0001D6FC File Offset: 0x0001B8FC
		[EditorBrowsable(EditorBrowsableState.Never)]
		public bool4 xzzz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new bool4(this.x, this.z, this.z, this.z);
			}
		}

		// Token: 0x1700003C RID: 60
		// (get) Token: 0x06000879 RID: 2169 RVA: 0x0001D71B File Offset: 0x0001B91B
		[EditorBrowsable(EditorBrowsableState.Never)]
		public bool4 yxxx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new bool4(this.y, this.x, this.x, this.x);
			}
		}

		// Token: 0x1700003D RID: 61
		// (get) Token: 0x0600087A RID: 2170 RVA: 0x0001D73A File Offset: 0x0001B93A
		[EditorBrowsable(EditorBrowsableState.Never)]
		public bool4 yxxy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new bool4(this.y, this.x, this.x, this.y);
			}
		}

		// Token: 0x1700003E RID: 62
		// (get) Token: 0x0600087B RID: 2171 RVA: 0x0001D759 File Offset: 0x0001B959
		[EditorBrowsable(EditorBrowsableState.Never)]
		public bool4 yxxz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new bool4(this.y, this.x, this.x, this.z);
			}
		}

		// Token: 0x1700003F RID: 63
		// (get) Token: 0x0600087C RID: 2172 RVA: 0x0001D778 File Offset: 0x0001B978
		[EditorBrowsable(EditorBrowsableState.Never)]
		public bool4 yxyx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new bool4(this.y, this.x, this.y, this.x);
			}
		}

		// Token: 0x17000040 RID: 64
		// (get) Token: 0x0600087D RID: 2173 RVA: 0x0001D797 File Offset: 0x0001B997
		[EditorBrowsable(EditorBrowsableState.Never)]
		public bool4 yxyy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new bool4(this.y, this.x, this.y, this.y);
			}
		}

		// Token: 0x17000041 RID: 65
		// (get) Token: 0x0600087E RID: 2174 RVA: 0x0001D7B6 File Offset: 0x0001B9B6
		[EditorBrowsable(EditorBrowsableState.Never)]
		public bool4 yxyz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new bool4(this.y, this.x, this.y, this.z);
			}
		}

		// Token: 0x17000042 RID: 66
		// (get) Token: 0x0600087F RID: 2175 RVA: 0x0001D7D5 File Offset: 0x0001B9D5
		[EditorBrowsable(EditorBrowsableState.Never)]
		public bool4 yxzx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new bool4(this.y, this.x, this.z, this.x);
			}
		}

		// Token: 0x17000043 RID: 67
		// (get) Token: 0x06000880 RID: 2176 RVA: 0x0001D7F4 File Offset: 0x0001B9F4
		[EditorBrowsable(EditorBrowsableState.Never)]
		public bool4 yxzy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new bool4(this.y, this.x, this.z, this.y);
			}
		}

		// Token: 0x17000044 RID: 68
		// (get) Token: 0x06000881 RID: 2177 RVA: 0x0001D813 File Offset: 0x0001BA13
		[EditorBrowsable(EditorBrowsableState.Never)]
		public bool4 yxzz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new bool4(this.y, this.x, this.z, this.z);
			}
		}

		// Token: 0x17000045 RID: 69
		// (get) Token: 0x06000882 RID: 2178 RVA: 0x0001D832 File Offset: 0x0001BA32
		[EditorBrowsable(EditorBrowsableState.Never)]
		public bool4 yyxx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new bool4(this.y, this.y, this.x, this.x);
			}
		}

		// Token: 0x17000046 RID: 70
		// (get) Token: 0x06000883 RID: 2179 RVA: 0x0001D851 File Offset: 0x0001BA51
		[EditorBrowsable(EditorBrowsableState.Never)]
		public bool4 yyxy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new bool4(this.y, this.y, this.x, this.y);
			}
		}

		// Token: 0x17000047 RID: 71
		// (get) Token: 0x06000884 RID: 2180 RVA: 0x0001D870 File Offset: 0x0001BA70
		[EditorBrowsable(EditorBrowsableState.Never)]
		public bool4 yyxz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new bool4(this.y, this.y, this.x, this.z);
			}
		}

		// Token: 0x17000048 RID: 72
		// (get) Token: 0x06000885 RID: 2181 RVA: 0x0001D88F File Offset: 0x0001BA8F
		[EditorBrowsable(EditorBrowsableState.Never)]
		public bool4 yyyx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new bool4(this.y, this.y, this.y, this.x);
			}
		}

		// Token: 0x17000049 RID: 73
		// (get) Token: 0x06000886 RID: 2182 RVA: 0x0001D8AE File Offset: 0x0001BAAE
		[EditorBrowsable(EditorBrowsableState.Never)]
		public bool4 yyyy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new bool4(this.y, this.y, this.y, this.y);
			}
		}

		// Token: 0x1700004A RID: 74
		// (get) Token: 0x06000887 RID: 2183 RVA: 0x0001D8CD File Offset: 0x0001BACD
		[EditorBrowsable(EditorBrowsableState.Never)]
		public bool4 yyyz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new bool4(this.y, this.y, this.y, this.z);
			}
		}

		// Token: 0x1700004B RID: 75
		// (get) Token: 0x06000888 RID: 2184 RVA: 0x0001D8EC File Offset: 0x0001BAEC
		[EditorBrowsable(EditorBrowsableState.Never)]
		public bool4 yyzx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new bool4(this.y, this.y, this.z, this.x);
			}
		}

		// Token: 0x1700004C RID: 76
		// (get) Token: 0x06000889 RID: 2185 RVA: 0x0001D90B File Offset: 0x0001BB0B
		[EditorBrowsable(EditorBrowsableState.Never)]
		public bool4 yyzy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new bool4(this.y, this.y, this.z, this.y);
			}
		}

		// Token: 0x1700004D RID: 77
		// (get) Token: 0x0600088A RID: 2186 RVA: 0x0001D92A File Offset: 0x0001BB2A
		[EditorBrowsable(EditorBrowsableState.Never)]
		public bool4 yyzz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new bool4(this.y, this.y, this.z, this.z);
			}
		}

		// Token: 0x1700004E RID: 78
		// (get) Token: 0x0600088B RID: 2187 RVA: 0x0001D949 File Offset: 0x0001BB49
		[EditorBrowsable(EditorBrowsableState.Never)]
		public bool4 yzxx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new bool4(this.y, this.z, this.x, this.x);
			}
		}

		// Token: 0x1700004F RID: 79
		// (get) Token: 0x0600088C RID: 2188 RVA: 0x0001D968 File Offset: 0x0001BB68
		[EditorBrowsable(EditorBrowsableState.Never)]
		public bool4 yzxy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new bool4(this.y, this.z, this.x, this.y);
			}
		}

		// Token: 0x17000050 RID: 80
		// (get) Token: 0x0600088D RID: 2189 RVA: 0x0001D987 File Offset: 0x0001BB87
		[EditorBrowsable(EditorBrowsableState.Never)]
		public bool4 yzxz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new bool4(this.y, this.z, this.x, this.z);
			}
		}

		// Token: 0x17000051 RID: 81
		// (get) Token: 0x0600088E RID: 2190 RVA: 0x0001D9A6 File Offset: 0x0001BBA6
		[EditorBrowsable(EditorBrowsableState.Never)]
		public bool4 yzyx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new bool4(this.y, this.z, this.y, this.x);
			}
		}

		// Token: 0x17000052 RID: 82
		// (get) Token: 0x0600088F RID: 2191 RVA: 0x0001D9C5 File Offset: 0x0001BBC5
		[EditorBrowsable(EditorBrowsableState.Never)]
		public bool4 yzyy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new bool4(this.y, this.z, this.y, this.y);
			}
		}

		// Token: 0x17000053 RID: 83
		// (get) Token: 0x06000890 RID: 2192 RVA: 0x0001D9E4 File Offset: 0x0001BBE4
		[EditorBrowsable(EditorBrowsableState.Never)]
		public bool4 yzyz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new bool4(this.y, this.z, this.y, this.z);
			}
		}

		// Token: 0x17000054 RID: 84
		// (get) Token: 0x06000891 RID: 2193 RVA: 0x0001DA03 File Offset: 0x0001BC03
		[EditorBrowsable(EditorBrowsableState.Never)]
		public bool4 yzzx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new bool4(this.y, this.z, this.z, this.x);
			}
		}

		// Token: 0x17000055 RID: 85
		// (get) Token: 0x06000892 RID: 2194 RVA: 0x0001DA22 File Offset: 0x0001BC22
		[EditorBrowsable(EditorBrowsableState.Never)]
		public bool4 yzzy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new bool4(this.y, this.z, this.z, this.y);
			}
		}

		// Token: 0x17000056 RID: 86
		// (get) Token: 0x06000893 RID: 2195 RVA: 0x0001DA41 File Offset: 0x0001BC41
		[EditorBrowsable(EditorBrowsableState.Never)]
		public bool4 yzzz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new bool4(this.y, this.z, this.z, this.z);
			}
		}

		// Token: 0x17000057 RID: 87
		// (get) Token: 0x06000894 RID: 2196 RVA: 0x0001DA60 File Offset: 0x0001BC60
		[EditorBrowsable(EditorBrowsableState.Never)]
		public bool4 zxxx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new bool4(this.z, this.x, this.x, this.x);
			}
		}

		// Token: 0x17000058 RID: 88
		// (get) Token: 0x06000895 RID: 2197 RVA: 0x0001DA7F File Offset: 0x0001BC7F
		[EditorBrowsable(EditorBrowsableState.Never)]
		public bool4 zxxy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new bool4(this.z, this.x, this.x, this.y);
			}
		}

		// Token: 0x17000059 RID: 89
		// (get) Token: 0x06000896 RID: 2198 RVA: 0x0001DA9E File Offset: 0x0001BC9E
		[EditorBrowsable(EditorBrowsableState.Never)]
		public bool4 zxxz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new bool4(this.z, this.x, this.x, this.z);
			}
		}

		// Token: 0x1700005A RID: 90
		// (get) Token: 0x06000897 RID: 2199 RVA: 0x0001DABD File Offset: 0x0001BCBD
		[EditorBrowsable(EditorBrowsableState.Never)]
		public bool4 zxyx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new bool4(this.z, this.x, this.y, this.x);
			}
		}

		// Token: 0x1700005B RID: 91
		// (get) Token: 0x06000898 RID: 2200 RVA: 0x0001DADC File Offset: 0x0001BCDC
		[EditorBrowsable(EditorBrowsableState.Never)]
		public bool4 zxyy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new bool4(this.z, this.x, this.y, this.y);
			}
		}

		// Token: 0x1700005C RID: 92
		// (get) Token: 0x06000899 RID: 2201 RVA: 0x0001DAFB File Offset: 0x0001BCFB
		[EditorBrowsable(EditorBrowsableState.Never)]
		public bool4 zxyz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new bool4(this.z, this.x, this.y, this.z);
			}
		}

		// Token: 0x1700005D RID: 93
		// (get) Token: 0x0600089A RID: 2202 RVA: 0x0001DB1A File Offset: 0x0001BD1A
		[EditorBrowsable(EditorBrowsableState.Never)]
		public bool4 zxzx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new bool4(this.z, this.x, this.z, this.x);
			}
		}

		// Token: 0x1700005E RID: 94
		// (get) Token: 0x0600089B RID: 2203 RVA: 0x0001DB39 File Offset: 0x0001BD39
		[EditorBrowsable(EditorBrowsableState.Never)]
		public bool4 zxzy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new bool4(this.z, this.x, this.z, this.y);
			}
		}

		// Token: 0x1700005F RID: 95
		// (get) Token: 0x0600089C RID: 2204 RVA: 0x0001DB58 File Offset: 0x0001BD58
		[EditorBrowsable(EditorBrowsableState.Never)]
		public bool4 zxzz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new bool4(this.z, this.x, this.z, this.z);
			}
		}

		// Token: 0x17000060 RID: 96
		// (get) Token: 0x0600089D RID: 2205 RVA: 0x0001DB77 File Offset: 0x0001BD77
		[EditorBrowsable(EditorBrowsableState.Never)]
		public bool4 zyxx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new bool4(this.z, this.y, this.x, this.x);
			}
		}

		// Token: 0x17000061 RID: 97
		// (get) Token: 0x0600089E RID: 2206 RVA: 0x0001DB96 File Offset: 0x0001BD96
		[EditorBrowsable(EditorBrowsableState.Never)]
		public bool4 zyxy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new bool4(this.z, this.y, this.x, this.y);
			}
		}

		// Token: 0x17000062 RID: 98
		// (get) Token: 0x0600089F RID: 2207 RVA: 0x0001DBB5 File Offset: 0x0001BDB5
		[EditorBrowsable(EditorBrowsableState.Never)]
		public bool4 zyxz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new bool4(this.z, this.y, this.x, this.z);
			}
		}

		// Token: 0x17000063 RID: 99
		// (get) Token: 0x060008A0 RID: 2208 RVA: 0x0001DBD4 File Offset: 0x0001BDD4
		[EditorBrowsable(EditorBrowsableState.Never)]
		public bool4 zyyx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new bool4(this.z, this.y, this.y, this.x);
			}
		}

		// Token: 0x17000064 RID: 100
		// (get) Token: 0x060008A1 RID: 2209 RVA: 0x0001DBF3 File Offset: 0x0001BDF3
		[EditorBrowsable(EditorBrowsableState.Never)]
		public bool4 zyyy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new bool4(this.z, this.y, this.y, this.y);
			}
		}

		// Token: 0x17000065 RID: 101
		// (get) Token: 0x060008A2 RID: 2210 RVA: 0x0001DC12 File Offset: 0x0001BE12
		[EditorBrowsable(EditorBrowsableState.Never)]
		public bool4 zyyz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new bool4(this.z, this.y, this.y, this.z);
			}
		}

		// Token: 0x17000066 RID: 102
		// (get) Token: 0x060008A3 RID: 2211 RVA: 0x0001DC31 File Offset: 0x0001BE31
		[EditorBrowsable(EditorBrowsableState.Never)]
		public bool4 zyzx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new bool4(this.z, this.y, this.z, this.x);
			}
		}

		// Token: 0x17000067 RID: 103
		// (get) Token: 0x060008A4 RID: 2212 RVA: 0x0001DC50 File Offset: 0x0001BE50
		[EditorBrowsable(EditorBrowsableState.Never)]
		public bool4 zyzy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new bool4(this.z, this.y, this.z, this.y);
			}
		}

		// Token: 0x17000068 RID: 104
		// (get) Token: 0x060008A5 RID: 2213 RVA: 0x0001DC6F File Offset: 0x0001BE6F
		[EditorBrowsable(EditorBrowsableState.Never)]
		public bool4 zyzz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new bool4(this.z, this.y, this.z, this.z);
			}
		}

		// Token: 0x17000069 RID: 105
		// (get) Token: 0x060008A6 RID: 2214 RVA: 0x0001DC8E File Offset: 0x0001BE8E
		[EditorBrowsable(EditorBrowsableState.Never)]
		public bool4 zzxx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new bool4(this.z, this.z, this.x, this.x);
			}
		}

		// Token: 0x1700006A RID: 106
		// (get) Token: 0x060008A7 RID: 2215 RVA: 0x0001DCAD File Offset: 0x0001BEAD
		[EditorBrowsable(EditorBrowsableState.Never)]
		public bool4 zzxy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new bool4(this.z, this.z, this.x, this.y);
			}
		}

		// Token: 0x1700006B RID: 107
		// (get) Token: 0x060008A8 RID: 2216 RVA: 0x0001DCCC File Offset: 0x0001BECC
		[EditorBrowsable(EditorBrowsableState.Never)]
		public bool4 zzxz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new bool4(this.z, this.z, this.x, this.z);
			}
		}

		// Token: 0x1700006C RID: 108
		// (get) Token: 0x060008A9 RID: 2217 RVA: 0x0001DCEB File Offset: 0x0001BEEB
		[EditorBrowsable(EditorBrowsableState.Never)]
		public bool4 zzyx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new bool4(this.z, this.z, this.y, this.x);
			}
		}

		// Token: 0x1700006D RID: 109
		// (get) Token: 0x060008AA RID: 2218 RVA: 0x0001DD0A File Offset: 0x0001BF0A
		[EditorBrowsable(EditorBrowsableState.Never)]
		public bool4 zzyy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new bool4(this.z, this.z, this.y, this.y);
			}
		}

		// Token: 0x1700006E RID: 110
		// (get) Token: 0x060008AB RID: 2219 RVA: 0x0001DD29 File Offset: 0x0001BF29
		[EditorBrowsable(EditorBrowsableState.Never)]
		public bool4 zzyz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new bool4(this.z, this.z, this.y, this.z);
			}
		}

		// Token: 0x1700006F RID: 111
		// (get) Token: 0x060008AC RID: 2220 RVA: 0x0001DD48 File Offset: 0x0001BF48
		[EditorBrowsable(EditorBrowsableState.Never)]
		public bool4 zzzx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new bool4(this.z, this.z, this.z, this.x);
			}
		}

		// Token: 0x17000070 RID: 112
		// (get) Token: 0x060008AD RID: 2221 RVA: 0x0001DD67 File Offset: 0x0001BF67
		[EditorBrowsable(EditorBrowsableState.Never)]
		public bool4 zzzy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new bool4(this.z, this.z, this.z, this.y);
			}
		}

		// Token: 0x17000071 RID: 113
		// (get) Token: 0x060008AE RID: 2222 RVA: 0x0001DD86 File Offset: 0x0001BF86
		[EditorBrowsable(EditorBrowsableState.Never)]
		public bool4 zzzz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new bool4(this.z, this.z, this.z, this.z);
			}
		}

		// Token: 0x17000072 RID: 114
		// (get) Token: 0x060008AF RID: 2223 RVA: 0x0001DDA5 File Offset: 0x0001BFA5
		[EditorBrowsable(EditorBrowsableState.Never)]
		public bool3 xxx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new bool3(this.x, this.x, this.x);
			}
		}

		// Token: 0x17000073 RID: 115
		// (get) Token: 0x060008B0 RID: 2224 RVA: 0x0001DDBE File Offset: 0x0001BFBE
		[EditorBrowsable(EditorBrowsableState.Never)]
		public bool3 xxy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new bool3(this.x, this.x, this.y);
			}
		}

		// Token: 0x17000074 RID: 116
		// (get) Token: 0x060008B1 RID: 2225 RVA: 0x0001DDD7 File Offset: 0x0001BFD7
		[EditorBrowsable(EditorBrowsableState.Never)]
		public bool3 xxz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new bool3(this.x, this.x, this.z);
			}
		}

		// Token: 0x17000075 RID: 117
		// (get) Token: 0x060008B2 RID: 2226 RVA: 0x0001DDF0 File Offset: 0x0001BFF0
		[EditorBrowsable(EditorBrowsableState.Never)]
		public bool3 xyx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new bool3(this.x, this.y, this.x);
			}
		}

		// Token: 0x17000076 RID: 118
		// (get) Token: 0x060008B3 RID: 2227 RVA: 0x0001DE09 File Offset: 0x0001C009
		[EditorBrowsable(EditorBrowsableState.Never)]
		public bool3 xyy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new bool3(this.x, this.y, this.y);
			}
		}

		// Token: 0x17000077 RID: 119
		// (get) Token: 0x060008B4 RID: 2228 RVA: 0x0001DE22 File Offset: 0x0001C022
		// (set) Token: 0x060008B5 RID: 2229 RVA: 0x0001DE3B File Offset: 0x0001C03B
		[EditorBrowsable(EditorBrowsableState.Never)]
		public bool3 xyz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new bool3(this.x, this.y, this.z);
			}
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			set
			{
				this.x = value.x;
				this.y = value.y;
				this.z = value.z;
			}
		}

		// Token: 0x17000078 RID: 120
		// (get) Token: 0x060008B6 RID: 2230 RVA: 0x0001DE61 File Offset: 0x0001C061
		[EditorBrowsable(EditorBrowsableState.Never)]
		public bool3 xzx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new bool3(this.x, this.z, this.x);
			}
		}

		// Token: 0x17000079 RID: 121
		// (get) Token: 0x060008B7 RID: 2231 RVA: 0x0001DE7A File Offset: 0x0001C07A
		// (set) Token: 0x060008B8 RID: 2232 RVA: 0x0001DE93 File Offset: 0x0001C093
		[EditorBrowsable(EditorBrowsableState.Never)]
		public bool3 xzy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new bool3(this.x, this.z, this.y);
			}
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			set
			{
				this.x = value.x;
				this.z = value.y;
				this.y = value.z;
			}
		}

		// Token: 0x1700007A RID: 122
		// (get) Token: 0x060008B9 RID: 2233 RVA: 0x0001DEB9 File Offset: 0x0001C0B9
		[EditorBrowsable(EditorBrowsableState.Never)]
		public bool3 xzz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new bool3(this.x, this.z, this.z);
			}
		}

		// Token: 0x1700007B RID: 123
		// (get) Token: 0x060008BA RID: 2234 RVA: 0x0001DED2 File Offset: 0x0001C0D2
		[EditorBrowsable(EditorBrowsableState.Never)]
		public bool3 yxx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new bool3(this.y, this.x, this.x);
			}
		}

		// Token: 0x1700007C RID: 124
		// (get) Token: 0x060008BB RID: 2235 RVA: 0x0001DEEB File Offset: 0x0001C0EB
		[EditorBrowsable(EditorBrowsableState.Never)]
		public bool3 yxy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new bool3(this.y, this.x, this.y);
			}
		}

		// Token: 0x1700007D RID: 125
		// (get) Token: 0x060008BC RID: 2236 RVA: 0x0001DF04 File Offset: 0x0001C104
		// (set) Token: 0x060008BD RID: 2237 RVA: 0x0001DF1D File Offset: 0x0001C11D
		[EditorBrowsable(EditorBrowsableState.Never)]
		public bool3 yxz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new bool3(this.y, this.x, this.z);
			}
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			set
			{
				this.y = value.x;
				this.x = value.y;
				this.z = value.z;
			}
		}

		// Token: 0x1700007E RID: 126
		// (get) Token: 0x060008BE RID: 2238 RVA: 0x0001DF43 File Offset: 0x0001C143
		[EditorBrowsable(EditorBrowsableState.Never)]
		public bool3 yyx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new bool3(this.y, this.y, this.x);
			}
		}

		// Token: 0x1700007F RID: 127
		// (get) Token: 0x060008BF RID: 2239 RVA: 0x0001DF5C File Offset: 0x0001C15C
		[EditorBrowsable(EditorBrowsableState.Never)]
		public bool3 yyy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new bool3(this.y, this.y, this.y);
			}
		}

		// Token: 0x17000080 RID: 128
		// (get) Token: 0x060008C0 RID: 2240 RVA: 0x0001DF75 File Offset: 0x0001C175
		[EditorBrowsable(EditorBrowsableState.Never)]
		public bool3 yyz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new bool3(this.y, this.y, this.z);
			}
		}

		// Token: 0x17000081 RID: 129
		// (get) Token: 0x060008C1 RID: 2241 RVA: 0x0001DF8E File Offset: 0x0001C18E
		// (set) Token: 0x060008C2 RID: 2242 RVA: 0x0001DFA7 File Offset: 0x0001C1A7
		[EditorBrowsable(EditorBrowsableState.Never)]
		public bool3 yzx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new bool3(this.y, this.z, this.x);
			}
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			set
			{
				this.y = value.x;
				this.z = value.y;
				this.x = value.z;
			}
		}

		// Token: 0x17000082 RID: 130
		// (get) Token: 0x060008C3 RID: 2243 RVA: 0x0001DFCD File Offset: 0x0001C1CD
		[EditorBrowsable(EditorBrowsableState.Never)]
		public bool3 yzy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new bool3(this.y, this.z, this.y);
			}
		}

		// Token: 0x17000083 RID: 131
		// (get) Token: 0x060008C4 RID: 2244 RVA: 0x0001DFE6 File Offset: 0x0001C1E6
		[EditorBrowsable(EditorBrowsableState.Never)]
		public bool3 yzz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new bool3(this.y, this.z, this.z);
			}
		}

		// Token: 0x17000084 RID: 132
		// (get) Token: 0x060008C5 RID: 2245 RVA: 0x0001DFFF File Offset: 0x0001C1FF
		[EditorBrowsable(EditorBrowsableState.Never)]
		public bool3 zxx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new bool3(this.z, this.x, this.x);
			}
		}

		// Token: 0x17000085 RID: 133
		// (get) Token: 0x060008C6 RID: 2246 RVA: 0x0001E018 File Offset: 0x0001C218
		// (set) Token: 0x060008C7 RID: 2247 RVA: 0x0001E031 File Offset: 0x0001C231
		[EditorBrowsable(EditorBrowsableState.Never)]
		public bool3 zxy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new bool3(this.z, this.x, this.y);
			}
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			set
			{
				this.z = value.x;
				this.x = value.y;
				this.y = value.z;
			}
		}

		// Token: 0x17000086 RID: 134
		// (get) Token: 0x060008C8 RID: 2248 RVA: 0x0001E057 File Offset: 0x0001C257
		[EditorBrowsable(EditorBrowsableState.Never)]
		public bool3 zxz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new bool3(this.z, this.x, this.z);
			}
		}

		// Token: 0x17000087 RID: 135
		// (get) Token: 0x060008C9 RID: 2249 RVA: 0x0001E070 File Offset: 0x0001C270
		// (set) Token: 0x060008CA RID: 2250 RVA: 0x0001E089 File Offset: 0x0001C289
		[EditorBrowsable(EditorBrowsableState.Never)]
		public bool3 zyx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new bool3(this.z, this.y, this.x);
			}
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			set
			{
				this.z = value.x;
				this.y = value.y;
				this.x = value.z;
			}
		}

		// Token: 0x17000088 RID: 136
		// (get) Token: 0x060008CB RID: 2251 RVA: 0x0001E0AF File Offset: 0x0001C2AF
		[EditorBrowsable(EditorBrowsableState.Never)]
		public bool3 zyy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new bool3(this.z, this.y, this.y);
			}
		}

		// Token: 0x17000089 RID: 137
		// (get) Token: 0x060008CC RID: 2252 RVA: 0x0001E0C8 File Offset: 0x0001C2C8
		[EditorBrowsable(EditorBrowsableState.Never)]
		public bool3 zyz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new bool3(this.z, this.y, this.z);
			}
		}

		// Token: 0x1700008A RID: 138
		// (get) Token: 0x060008CD RID: 2253 RVA: 0x0001E0E1 File Offset: 0x0001C2E1
		[EditorBrowsable(EditorBrowsableState.Never)]
		public bool3 zzx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new bool3(this.z, this.z, this.x);
			}
		}

		// Token: 0x1700008B RID: 139
		// (get) Token: 0x060008CE RID: 2254 RVA: 0x0001E0FA File Offset: 0x0001C2FA
		[EditorBrowsable(EditorBrowsableState.Never)]
		public bool3 zzy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new bool3(this.z, this.z, this.y);
			}
		}

		// Token: 0x1700008C RID: 140
		// (get) Token: 0x060008CF RID: 2255 RVA: 0x0001E113 File Offset: 0x0001C313
		[EditorBrowsable(EditorBrowsableState.Never)]
		public bool3 zzz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new bool3(this.z, this.z, this.z);
			}
		}

		// Token: 0x1700008D RID: 141
		// (get) Token: 0x060008D0 RID: 2256 RVA: 0x0001E12C File Offset: 0x0001C32C
		[EditorBrowsable(EditorBrowsableState.Never)]
		public bool2 xx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new bool2(this.x, this.x);
			}
		}

		// Token: 0x1700008E RID: 142
		// (get) Token: 0x060008D1 RID: 2257 RVA: 0x0001E13F File Offset: 0x0001C33F
		// (set) Token: 0x060008D2 RID: 2258 RVA: 0x0001E152 File Offset: 0x0001C352
		[EditorBrowsable(EditorBrowsableState.Never)]
		public bool2 xy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new bool2(this.x, this.y);
			}
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			set
			{
				this.x = value.x;
				this.y = value.y;
			}
		}

		// Token: 0x1700008F RID: 143
		// (get) Token: 0x060008D3 RID: 2259 RVA: 0x0001E16C File Offset: 0x0001C36C
		// (set) Token: 0x060008D4 RID: 2260 RVA: 0x0001E17F File Offset: 0x0001C37F
		[EditorBrowsable(EditorBrowsableState.Never)]
		public bool2 xz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new bool2(this.x, this.z);
			}
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			set
			{
				this.x = value.x;
				this.z = value.y;
			}
		}

		// Token: 0x17000090 RID: 144
		// (get) Token: 0x060008D5 RID: 2261 RVA: 0x0001E199 File Offset: 0x0001C399
		// (set) Token: 0x060008D6 RID: 2262 RVA: 0x0001E1AC File Offset: 0x0001C3AC
		[EditorBrowsable(EditorBrowsableState.Never)]
		public bool2 yx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new bool2(this.y, this.x);
			}
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			set
			{
				this.y = value.x;
				this.x = value.y;
			}
		}

		// Token: 0x17000091 RID: 145
		// (get) Token: 0x060008D7 RID: 2263 RVA: 0x0001E1C6 File Offset: 0x0001C3C6
		[EditorBrowsable(EditorBrowsableState.Never)]
		public bool2 yy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new bool2(this.y, this.y);
			}
		}

		// Token: 0x17000092 RID: 146
		// (get) Token: 0x060008D8 RID: 2264 RVA: 0x0001E1D9 File Offset: 0x0001C3D9
		// (set) Token: 0x060008D9 RID: 2265 RVA: 0x0001E1EC File Offset: 0x0001C3EC
		[EditorBrowsable(EditorBrowsableState.Never)]
		public bool2 yz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new bool2(this.y, this.z);
			}
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			set
			{
				this.y = value.x;
				this.z = value.y;
			}
		}

		// Token: 0x17000093 RID: 147
		// (get) Token: 0x060008DA RID: 2266 RVA: 0x0001E206 File Offset: 0x0001C406
		// (set) Token: 0x060008DB RID: 2267 RVA: 0x0001E219 File Offset: 0x0001C419
		[EditorBrowsable(EditorBrowsableState.Never)]
		public bool2 zx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new bool2(this.z, this.x);
			}
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			set
			{
				this.z = value.x;
				this.x = value.y;
			}
		}

		// Token: 0x17000094 RID: 148
		// (get) Token: 0x060008DC RID: 2268 RVA: 0x0001E233 File Offset: 0x0001C433
		// (set) Token: 0x060008DD RID: 2269 RVA: 0x0001E246 File Offset: 0x0001C446
		[EditorBrowsable(EditorBrowsableState.Never)]
		public bool2 zy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new bool2(this.z, this.y);
			}
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			set
			{
				this.z = value.x;
				this.y = value.y;
			}
		}

		// Token: 0x17000095 RID: 149
		// (get) Token: 0x060008DE RID: 2270 RVA: 0x0001E260 File Offset: 0x0001C460
		[EditorBrowsable(EditorBrowsableState.Never)]
		public bool2 zz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new bool2(this.z, this.z);
			}
		}

		// Token: 0x17000096 RID: 150
		public unsafe bool this[int index]
		{
			get
			{
				fixed (bool3* ptr = &this)
				{
					return ((byte*)ptr)[index] != 0;
				}
			}
			set
			{
				fixed (bool* ptr = &this.x)
				{
					ptr[index] = value;
				}
			}
		}

		// Token: 0x060008E1 RID: 2273 RVA: 0x0001E2A5 File Offset: 0x0001C4A5
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool Equals(bool3 rhs)
		{
			return this.x == rhs.x && this.y == rhs.y && this.z == rhs.z;
		}

		// Token: 0x060008E2 RID: 2274 RVA: 0x0001E2D4 File Offset: 0x0001C4D4
		public override bool Equals(object o)
		{
			if (o is bool3)
			{
				bool3 @bool = (bool3)o;
				return this.Equals(@bool);
			}
			return false;
		}

		// Token: 0x060008E3 RID: 2275 RVA: 0x0001E2F9 File Offset: 0x0001C4F9
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public override int GetHashCode()
		{
			return (int)math.hash(this);
		}

		// Token: 0x060008E4 RID: 2276 RVA: 0x0001E306 File Offset: 0x0001C506
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public override string ToString()
		{
			return string.Format("bool3({0}, {1}, {2})", this.x, this.y, this.z);
		}

		// Token: 0x04000030 RID: 48
		[MarshalAs(UnmanagedType.U1)]
		public bool x;

		// Token: 0x04000031 RID: 49
		[MarshalAs(UnmanagedType.U1)]
		public bool y;

		// Token: 0x04000032 RID: 50
		[MarshalAs(UnmanagedType.U1)]
		public bool z;

		// Token: 0x02000056 RID: 86
		internal sealed class DebuggerProxy
		{
			// Token: 0x060024C3 RID: 9411 RVA: 0x0006911C File Offset: 0x0006731C
			public DebuggerProxy(bool3 v)
			{
				this.x = v.x;
				this.y = v.y;
				this.z = v.z;
			}

			// Token: 0x0400014C RID: 332
			public bool x;

			// Token: 0x0400014D RID: 333
			public bool y;

			// Token: 0x0400014E RID: 334
			public bool z;
		}
	}
}
