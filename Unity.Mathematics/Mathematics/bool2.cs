using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Unity.IL2CPP.CompilerServices;

namespace Unity.Mathematics
{
	// Token: 0x02000006 RID: 6
	[DebuggerTypeProxy(typeof(bool2.DebuggerProxy))]
	[Il2CppEagerStaticClassConstruction]
	[Serializable]
	public struct bool2 : IEquatable<bool2>
	{
		// Token: 0x060007C5 RID: 1989 RVA: 0x0001BD12 File Offset: 0x00019F12
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool2(bool x, bool y)
		{
			this.x = x;
			this.y = y;
		}

		// Token: 0x060007C6 RID: 1990 RVA: 0x0001BD22 File Offset: 0x00019F22
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool2(bool2 xy)
		{
			this.x = xy.x;
			this.y = xy.y;
		}

		// Token: 0x060007C7 RID: 1991 RVA: 0x0001BD3C File Offset: 0x00019F3C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool2(bool v)
		{
			this.x = v;
			this.y = v;
		}

		// Token: 0x060007C8 RID: 1992 RVA: 0x0001BD4C File Offset: 0x00019F4C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static implicit operator bool2(bool v)
		{
			return new bool2(v);
		}

		// Token: 0x060007C9 RID: 1993 RVA: 0x0001BD54 File Offset: 0x00019F54
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool2 operator ==(bool2 lhs, bool2 rhs)
		{
			return new bool2(lhs.x == rhs.x, lhs.y == rhs.y);
		}

		// Token: 0x060007CA RID: 1994 RVA: 0x0001BD77 File Offset: 0x00019F77
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool2 operator ==(bool2 lhs, bool rhs)
		{
			return new bool2(lhs.x == rhs, lhs.y == rhs);
		}

		// Token: 0x060007CB RID: 1995 RVA: 0x0001BD90 File Offset: 0x00019F90
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool2 operator ==(bool lhs, bool2 rhs)
		{
			return new bool2(lhs == rhs.x, lhs == rhs.y);
		}

		// Token: 0x060007CC RID: 1996 RVA: 0x0001BDA9 File Offset: 0x00019FA9
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool2 operator !=(bool2 lhs, bool2 rhs)
		{
			return new bool2(lhs.x != rhs.x, lhs.y != rhs.y);
		}

		// Token: 0x060007CD RID: 1997 RVA: 0x0001BDD2 File Offset: 0x00019FD2
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool2 operator !=(bool2 lhs, bool rhs)
		{
			return new bool2(lhs.x != rhs, lhs.y != rhs);
		}

		// Token: 0x060007CE RID: 1998 RVA: 0x0001BDF1 File Offset: 0x00019FF1
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool2 operator !=(bool lhs, bool2 rhs)
		{
			return new bool2(lhs != rhs.x, lhs != rhs.y);
		}

		// Token: 0x060007CF RID: 1999 RVA: 0x0001BE10 File Offset: 0x0001A010
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool2 operator !(bool2 val)
		{
			return new bool2(!val.x, !val.y);
		}

		// Token: 0x060007D0 RID: 2000 RVA: 0x0001BE29 File Offset: 0x0001A029
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool2 operator &(bool2 lhs, bool2 rhs)
		{
			return new bool2(lhs.x & rhs.x, lhs.y & rhs.y);
		}

		// Token: 0x060007D1 RID: 2001 RVA: 0x0001BE4A File Offset: 0x0001A04A
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool2 operator &(bool2 lhs, bool rhs)
		{
			return new bool2(lhs.x && rhs, lhs.y && rhs);
		}

		// Token: 0x060007D2 RID: 2002 RVA: 0x0001BE61 File Offset: 0x0001A061
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool2 operator &(bool lhs, bool2 rhs)
		{
			return new bool2(lhs & rhs.x, lhs & rhs.y);
		}

		// Token: 0x060007D3 RID: 2003 RVA: 0x0001BE78 File Offset: 0x0001A078
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool2 operator |(bool2 lhs, bool2 rhs)
		{
			return new bool2(lhs.x | rhs.x, lhs.y | rhs.y);
		}

		// Token: 0x060007D4 RID: 2004 RVA: 0x0001BE99 File Offset: 0x0001A099
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool2 operator |(bool2 lhs, bool rhs)
		{
			return new bool2(lhs.x || rhs, lhs.y || rhs);
		}

		// Token: 0x060007D5 RID: 2005 RVA: 0x0001BEB0 File Offset: 0x0001A0B0
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool2 operator |(bool lhs, bool2 rhs)
		{
			return new bool2(lhs | rhs.x, lhs | rhs.y);
		}

		// Token: 0x060007D6 RID: 2006 RVA: 0x0001BEC7 File Offset: 0x0001A0C7
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool2 operator ^(bool2 lhs, bool2 rhs)
		{
			return new bool2(lhs.x ^ rhs.x, lhs.y ^ rhs.y);
		}

		// Token: 0x060007D7 RID: 2007 RVA: 0x0001BEE8 File Offset: 0x0001A0E8
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool2 operator ^(bool2 lhs, bool rhs)
		{
			return new bool2(lhs.x ^ rhs, lhs.y ^ rhs);
		}

		// Token: 0x060007D8 RID: 2008 RVA: 0x0001BEFF File Offset: 0x0001A0FF
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool2 operator ^(bool lhs, bool2 rhs)
		{
			return new bool2(lhs ^ rhs.x, lhs ^ rhs.y);
		}

		// Token: 0x17000001 RID: 1
		// (get) Token: 0x060007D9 RID: 2009 RVA: 0x0001BF16 File Offset: 0x0001A116
		[EditorBrowsable(EditorBrowsableState.Never)]
		public bool4 xxxx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new bool4(this.x, this.x, this.x, this.x);
			}
		}

		// Token: 0x17000002 RID: 2
		// (get) Token: 0x060007DA RID: 2010 RVA: 0x0001BF35 File Offset: 0x0001A135
		[EditorBrowsable(EditorBrowsableState.Never)]
		public bool4 xxxy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new bool4(this.x, this.x, this.x, this.y);
			}
		}

		// Token: 0x17000003 RID: 3
		// (get) Token: 0x060007DB RID: 2011 RVA: 0x0001BF54 File Offset: 0x0001A154
		[EditorBrowsable(EditorBrowsableState.Never)]
		public bool4 xxyx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new bool4(this.x, this.x, this.y, this.x);
			}
		}

		// Token: 0x17000004 RID: 4
		// (get) Token: 0x060007DC RID: 2012 RVA: 0x0001BF73 File Offset: 0x0001A173
		[EditorBrowsable(EditorBrowsableState.Never)]
		public bool4 xxyy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new bool4(this.x, this.x, this.y, this.y);
			}
		}

		// Token: 0x17000005 RID: 5
		// (get) Token: 0x060007DD RID: 2013 RVA: 0x0001BF92 File Offset: 0x0001A192
		[EditorBrowsable(EditorBrowsableState.Never)]
		public bool4 xyxx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new bool4(this.x, this.y, this.x, this.x);
			}
		}

		// Token: 0x17000006 RID: 6
		// (get) Token: 0x060007DE RID: 2014 RVA: 0x0001BFB1 File Offset: 0x0001A1B1
		[EditorBrowsable(EditorBrowsableState.Never)]
		public bool4 xyxy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new bool4(this.x, this.y, this.x, this.y);
			}
		}

		// Token: 0x17000007 RID: 7
		// (get) Token: 0x060007DF RID: 2015 RVA: 0x0001BFD0 File Offset: 0x0001A1D0
		[EditorBrowsable(EditorBrowsableState.Never)]
		public bool4 xyyx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new bool4(this.x, this.y, this.y, this.x);
			}
		}

		// Token: 0x17000008 RID: 8
		// (get) Token: 0x060007E0 RID: 2016 RVA: 0x0001BFEF File Offset: 0x0001A1EF
		[EditorBrowsable(EditorBrowsableState.Never)]
		public bool4 xyyy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new bool4(this.x, this.y, this.y, this.y);
			}
		}

		// Token: 0x17000009 RID: 9
		// (get) Token: 0x060007E1 RID: 2017 RVA: 0x0001C00E File Offset: 0x0001A20E
		[EditorBrowsable(EditorBrowsableState.Never)]
		public bool4 yxxx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new bool4(this.y, this.x, this.x, this.x);
			}
		}

		// Token: 0x1700000A RID: 10
		// (get) Token: 0x060007E2 RID: 2018 RVA: 0x0001C02D File Offset: 0x0001A22D
		[EditorBrowsable(EditorBrowsableState.Never)]
		public bool4 yxxy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new bool4(this.y, this.x, this.x, this.y);
			}
		}

		// Token: 0x1700000B RID: 11
		// (get) Token: 0x060007E3 RID: 2019 RVA: 0x0001C04C File Offset: 0x0001A24C
		[EditorBrowsable(EditorBrowsableState.Never)]
		public bool4 yxyx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new bool4(this.y, this.x, this.y, this.x);
			}
		}

		// Token: 0x1700000C RID: 12
		// (get) Token: 0x060007E4 RID: 2020 RVA: 0x0001C06B File Offset: 0x0001A26B
		[EditorBrowsable(EditorBrowsableState.Never)]
		public bool4 yxyy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new bool4(this.y, this.x, this.y, this.y);
			}
		}

		// Token: 0x1700000D RID: 13
		// (get) Token: 0x060007E5 RID: 2021 RVA: 0x0001C08A File Offset: 0x0001A28A
		[EditorBrowsable(EditorBrowsableState.Never)]
		public bool4 yyxx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new bool4(this.y, this.y, this.x, this.x);
			}
		}

		// Token: 0x1700000E RID: 14
		// (get) Token: 0x060007E6 RID: 2022 RVA: 0x0001C0A9 File Offset: 0x0001A2A9
		[EditorBrowsable(EditorBrowsableState.Never)]
		public bool4 yyxy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new bool4(this.y, this.y, this.x, this.y);
			}
		}

		// Token: 0x1700000F RID: 15
		// (get) Token: 0x060007E7 RID: 2023 RVA: 0x0001C0C8 File Offset: 0x0001A2C8
		[EditorBrowsable(EditorBrowsableState.Never)]
		public bool4 yyyx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new bool4(this.y, this.y, this.y, this.x);
			}
		}

		// Token: 0x17000010 RID: 16
		// (get) Token: 0x060007E8 RID: 2024 RVA: 0x0001C0E7 File Offset: 0x0001A2E7
		[EditorBrowsable(EditorBrowsableState.Never)]
		public bool4 yyyy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new bool4(this.y, this.y, this.y, this.y);
			}
		}

		// Token: 0x17000011 RID: 17
		// (get) Token: 0x060007E9 RID: 2025 RVA: 0x0001C106 File Offset: 0x0001A306
		[EditorBrowsable(EditorBrowsableState.Never)]
		public bool3 xxx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new bool3(this.x, this.x, this.x);
			}
		}

		// Token: 0x17000012 RID: 18
		// (get) Token: 0x060007EA RID: 2026 RVA: 0x0001C11F File Offset: 0x0001A31F
		[EditorBrowsable(EditorBrowsableState.Never)]
		public bool3 xxy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new bool3(this.x, this.x, this.y);
			}
		}

		// Token: 0x17000013 RID: 19
		// (get) Token: 0x060007EB RID: 2027 RVA: 0x0001C138 File Offset: 0x0001A338
		[EditorBrowsable(EditorBrowsableState.Never)]
		public bool3 xyx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new bool3(this.x, this.y, this.x);
			}
		}

		// Token: 0x17000014 RID: 20
		// (get) Token: 0x060007EC RID: 2028 RVA: 0x0001C151 File Offset: 0x0001A351
		[EditorBrowsable(EditorBrowsableState.Never)]
		public bool3 xyy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new bool3(this.x, this.y, this.y);
			}
		}

		// Token: 0x17000015 RID: 21
		// (get) Token: 0x060007ED RID: 2029 RVA: 0x0001C16A File Offset: 0x0001A36A
		[EditorBrowsable(EditorBrowsableState.Never)]
		public bool3 yxx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new bool3(this.y, this.x, this.x);
			}
		}

		// Token: 0x17000016 RID: 22
		// (get) Token: 0x060007EE RID: 2030 RVA: 0x0001C183 File Offset: 0x0001A383
		[EditorBrowsable(EditorBrowsableState.Never)]
		public bool3 yxy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new bool3(this.y, this.x, this.y);
			}
		}

		// Token: 0x17000017 RID: 23
		// (get) Token: 0x060007EF RID: 2031 RVA: 0x0001C19C File Offset: 0x0001A39C
		[EditorBrowsable(EditorBrowsableState.Never)]
		public bool3 yyx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new bool3(this.y, this.y, this.x);
			}
		}

		// Token: 0x17000018 RID: 24
		// (get) Token: 0x060007F0 RID: 2032 RVA: 0x0001C1B5 File Offset: 0x0001A3B5
		[EditorBrowsable(EditorBrowsableState.Never)]
		public bool3 yyy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new bool3(this.y, this.y, this.y);
			}
		}

		// Token: 0x17000019 RID: 25
		// (get) Token: 0x060007F1 RID: 2033 RVA: 0x0001C1CE File Offset: 0x0001A3CE
		[EditorBrowsable(EditorBrowsableState.Never)]
		public bool2 xx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new bool2(this.x, this.x);
			}
		}

		// Token: 0x1700001A RID: 26
		// (get) Token: 0x060007F2 RID: 2034 RVA: 0x0001C1E1 File Offset: 0x0001A3E1
		// (set) Token: 0x060007F3 RID: 2035 RVA: 0x0001C1F4 File Offset: 0x0001A3F4
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

		// Token: 0x1700001B RID: 27
		// (get) Token: 0x060007F4 RID: 2036 RVA: 0x0001C20E File Offset: 0x0001A40E
		// (set) Token: 0x060007F5 RID: 2037 RVA: 0x0001C221 File Offset: 0x0001A421
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

		// Token: 0x1700001C RID: 28
		// (get) Token: 0x060007F6 RID: 2038 RVA: 0x0001C23B File Offset: 0x0001A43B
		[EditorBrowsable(EditorBrowsableState.Never)]
		public bool2 yy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new bool2(this.y, this.y);
			}
		}

		// Token: 0x1700001D RID: 29
		public unsafe bool this[int index]
		{
			get
			{
				fixed (bool2* ptr = &this)
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

		// Token: 0x060007F9 RID: 2041 RVA: 0x0001C281 File Offset: 0x0001A481
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool Equals(bool2 rhs)
		{
			return this.x == rhs.x && this.y == rhs.y;
		}

		// Token: 0x060007FA RID: 2042 RVA: 0x0001C2A4 File Offset: 0x0001A4A4
		public override bool Equals(object o)
		{
			if (o is bool2)
			{
				bool2 @bool = (bool2)o;
				return this.Equals(@bool);
			}
			return false;
		}

		// Token: 0x060007FB RID: 2043 RVA: 0x0001C2C9 File Offset: 0x0001A4C9
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public override int GetHashCode()
		{
			return (int)math.hash(this);
		}

		// Token: 0x060007FC RID: 2044 RVA: 0x0001C2D6 File Offset: 0x0001A4D6
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public override string ToString()
		{
			return string.Format("bool2({0}, {1})", this.x, this.y);
		}

		// Token: 0x04000025 RID: 37
		[MarshalAs(UnmanagedType.U1)]
		public bool x;

		// Token: 0x04000026 RID: 38
		[MarshalAs(UnmanagedType.U1)]
		public bool y;

		// Token: 0x02000055 RID: 85
		internal sealed class DebuggerProxy
		{
			// Token: 0x060024C2 RID: 9410 RVA: 0x000690FC File Offset: 0x000672FC
			public DebuggerProxy(bool2 v)
			{
				this.x = v.x;
				this.y = v.y;
			}

			// Token: 0x0400014A RID: 330
			public bool x;

			// Token: 0x0400014B RID: 331
			public bool y;
		}
	}
}
