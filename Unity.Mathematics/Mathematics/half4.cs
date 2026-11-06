using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Unity.IL2CPP.CompilerServices;

namespace Unity.Mathematics
{
	// Token: 0x0200002D RID: 45
	[DebuggerTypeProxy(typeof(half4.DebuggerProxy))]
	[Il2CppEagerStaticClassConstruction]
	[Serializable]
	public struct half4 : IEquatable<half4>, IFormattable
	{
		// Token: 0x060016C4 RID: 5828 RVA: 0x00040EC6 File Offset: 0x0003F0C6
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public half4(half x, half y, half z, half w)
		{
			this.x = x;
			this.y = y;
			this.z = z;
			this.w = w;
		}

		// Token: 0x060016C5 RID: 5829 RVA: 0x00040EE5 File Offset: 0x0003F0E5
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public half4(half x, half y, half2 zw)
		{
			this.x = x;
			this.y = y;
			this.z = zw.x;
			this.w = zw.y;
		}

		// Token: 0x060016C6 RID: 5830 RVA: 0x00040F0D File Offset: 0x0003F10D
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public half4(half x, half2 yz, half w)
		{
			this.x = x;
			this.y = yz.x;
			this.z = yz.y;
			this.w = w;
		}

		// Token: 0x060016C7 RID: 5831 RVA: 0x00040F35 File Offset: 0x0003F135
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public half4(half x, half3 yzw)
		{
			this.x = x;
			this.y = yzw.x;
			this.z = yzw.y;
			this.w = yzw.z;
		}

		// Token: 0x060016C8 RID: 5832 RVA: 0x00040F62 File Offset: 0x0003F162
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public half4(half2 xy, half z, half w)
		{
			this.x = xy.x;
			this.y = xy.y;
			this.z = z;
			this.w = w;
		}

		// Token: 0x060016C9 RID: 5833 RVA: 0x00040F8A File Offset: 0x0003F18A
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public half4(half2 xy, half2 zw)
		{
			this.x = xy.x;
			this.y = xy.y;
			this.z = zw.x;
			this.w = zw.y;
		}

		// Token: 0x060016CA RID: 5834 RVA: 0x00040FBC File Offset: 0x0003F1BC
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public half4(half3 xyz, half w)
		{
			this.x = xyz.x;
			this.y = xyz.y;
			this.z = xyz.z;
			this.w = w;
		}

		// Token: 0x060016CB RID: 5835 RVA: 0x00040FE9 File Offset: 0x0003F1E9
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public half4(half4 xyzw)
		{
			this.x = xyzw.x;
			this.y = xyzw.y;
			this.z = xyzw.z;
			this.w = xyzw.w;
		}

		// Token: 0x060016CC RID: 5836 RVA: 0x0004101B File Offset: 0x0003F21B
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public half4(half v)
		{
			this.x = v;
			this.y = v;
			this.z = v;
			this.w = v;
		}

		// Token: 0x060016CD RID: 5837 RVA: 0x00041039 File Offset: 0x0003F239
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public half4(float v)
		{
			this.x = (half)v;
			this.y = (half)v;
			this.z = (half)v;
			this.w = (half)v;
		}

		// Token: 0x060016CE RID: 5838 RVA: 0x0004106C File Offset: 0x0003F26C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public half4(float4 v)
		{
			this.x = (half)v.x;
			this.y = (half)v.y;
			this.z = (half)v.z;
			this.w = (half)v.w;
		}

		// Token: 0x060016CF RID: 5839 RVA: 0x000410BD File Offset: 0x0003F2BD
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public half4(double v)
		{
			this.x = (half)v;
			this.y = (half)v;
			this.z = (half)v;
			this.w = (half)v;
		}

		// Token: 0x060016D0 RID: 5840 RVA: 0x000410F0 File Offset: 0x0003F2F0
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public half4(double4 v)
		{
			this.x = (half)v.x;
			this.y = (half)v.y;
			this.z = (half)v.z;
			this.w = (half)v.w;
		}

		// Token: 0x060016D1 RID: 5841 RVA: 0x00041141 File Offset: 0x0003F341
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static implicit operator half4(half v)
		{
			return new half4(v);
		}

		// Token: 0x060016D2 RID: 5842 RVA: 0x00041149 File Offset: 0x0003F349
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static explicit operator half4(float v)
		{
			return new half4(v);
		}

		// Token: 0x060016D3 RID: 5843 RVA: 0x00041151 File Offset: 0x0003F351
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static explicit operator half4(float4 v)
		{
			return new half4(v);
		}

		// Token: 0x060016D4 RID: 5844 RVA: 0x00041159 File Offset: 0x0003F359
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static explicit operator half4(double v)
		{
			return new half4(v);
		}

		// Token: 0x060016D5 RID: 5845 RVA: 0x00041161 File Offset: 0x0003F361
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static explicit operator half4(double4 v)
		{
			return new half4(v);
		}

		// Token: 0x060016D6 RID: 5846 RVA: 0x0004116C File Offset: 0x0003F36C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool4 operator ==(half4 lhs, half4 rhs)
		{
			return new bool4(lhs.x == rhs.x, lhs.y == rhs.y, lhs.z == rhs.z, lhs.w == rhs.w);
		}

		// Token: 0x060016D7 RID: 5847 RVA: 0x000411C2 File Offset: 0x0003F3C2
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool4 operator ==(half4 lhs, half rhs)
		{
			return new bool4(lhs.x == rhs, lhs.y == rhs, lhs.z == rhs, lhs.w == rhs);
		}

		// Token: 0x060016D8 RID: 5848 RVA: 0x000411F9 File Offset: 0x0003F3F9
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool4 operator ==(half lhs, half4 rhs)
		{
			return new bool4(lhs == rhs.x, lhs == rhs.y, lhs == rhs.z, lhs == rhs.w);
		}

		// Token: 0x060016D9 RID: 5849 RVA: 0x00041230 File Offset: 0x0003F430
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool4 operator !=(half4 lhs, half4 rhs)
		{
			return new bool4(lhs.x != rhs.x, lhs.y != rhs.y, lhs.z != rhs.z, lhs.w != rhs.w);
		}

		// Token: 0x060016DA RID: 5850 RVA: 0x00041286 File Offset: 0x0003F486
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool4 operator !=(half4 lhs, half rhs)
		{
			return new bool4(lhs.x != rhs, lhs.y != rhs, lhs.z != rhs, lhs.w != rhs);
		}

		// Token: 0x060016DB RID: 5851 RVA: 0x000412BD File Offset: 0x0003F4BD
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool4 operator !=(half lhs, half4 rhs)
		{
			return new bool4(lhs != rhs.x, lhs != rhs.y, lhs != rhs.z, lhs != rhs.w);
		}

		// Token: 0x1700065F RID: 1631
		// (get) Token: 0x060016DC RID: 5852 RVA: 0x000412F4 File Offset: 0x0003F4F4
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half4 xxxx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half4(this.x, this.x, this.x, this.x);
			}
		}

		// Token: 0x17000660 RID: 1632
		// (get) Token: 0x060016DD RID: 5853 RVA: 0x00041313 File Offset: 0x0003F513
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half4 xxxy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half4(this.x, this.x, this.x, this.y);
			}
		}

		// Token: 0x17000661 RID: 1633
		// (get) Token: 0x060016DE RID: 5854 RVA: 0x00041332 File Offset: 0x0003F532
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half4 xxxz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half4(this.x, this.x, this.x, this.z);
			}
		}

		// Token: 0x17000662 RID: 1634
		// (get) Token: 0x060016DF RID: 5855 RVA: 0x00041351 File Offset: 0x0003F551
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half4 xxxw
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half4(this.x, this.x, this.x, this.w);
			}
		}

		// Token: 0x17000663 RID: 1635
		// (get) Token: 0x060016E0 RID: 5856 RVA: 0x00041370 File Offset: 0x0003F570
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half4 xxyx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half4(this.x, this.x, this.y, this.x);
			}
		}

		// Token: 0x17000664 RID: 1636
		// (get) Token: 0x060016E1 RID: 5857 RVA: 0x0004138F File Offset: 0x0003F58F
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half4 xxyy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half4(this.x, this.x, this.y, this.y);
			}
		}

		// Token: 0x17000665 RID: 1637
		// (get) Token: 0x060016E2 RID: 5858 RVA: 0x000413AE File Offset: 0x0003F5AE
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half4 xxyz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half4(this.x, this.x, this.y, this.z);
			}
		}

		// Token: 0x17000666 RID: 1638
		// (get) Token: 0x060016E3 RID: 5859 RVA: 0x000413CD File Offset: 0x0003F5CD
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half4 xxyw
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half4(this.x, this.x, this.y, this.w);
			}
		}

		// Token: 0x17000667 RID: 1639
		// (get) Token: 0x060016E4 RID: 5860 RVA: 0x000413EC File Offset: 0x0003F5EC
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half4 xxzx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half4(this.x, this.x, this.z, this.x);
			}
		}

		// Token: 0x17000668 RID: 1640
		// (get) Token: 0x060016E5 RID: 5861 RVA: 0x0004140B File Offset: 0x0003F60B
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half4 xxzy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half4(this.x, this.x, this.z, this.y);
			}
		}

		// Token: 0x17000669 RID: 1641
		// (get) Token: 0x060016E6 RID: 5862 RVA: 0x0004142A File Offset: 0x0003F62A
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half4 xxzz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half4(this.x, this.x, this.z, this.z);
			}
		}

		// Token: 0x1700066A RID: 1642
		// (get) Token: 0x060016E7 RID: 5863 RVA: 0x00041449 File Offset: 0x0003F649
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half4 xxzw
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half4(this.x, this.x, this.z, this.w);
			}
		}

		// Token: 0x1700066B RID: 1643
		// (get) Token: 0x060016E8 RID: 5864 RVA: 0x00041468 File Offset: 0x0003F668
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half4 xxwx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half4(this.x, this.x, this.w, this.x);
			}
		}

		// Token: 0x1700066C RID: 1644
		// (get) Token: 0x060016E9 RID: 5865 RVA: 0x00041487 File Offset: 0x0003F687
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half4 xxwy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half4(this.x, this.x, this.w, this.y);
			}
		}

		// Token: 0x1700066D RID: 1645
		// (get) Token: 0x060016EA RID: 5866 RVA: 0x000414A6 File Offset: 0x0003F6A6
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half4 xxwz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half4(this.x, this.x, this.w, this.z);
			}
		}

		// Token: 0x1700066E RID: 1646
		// (get) Token: 0x060016EB RID: 5867 RVA: 0x000414C5 File Offset: 0x0003F6C5
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half4 xxww
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half4(this.x, this.x, this.w, this.w);
			}
		}

		// Token: 0x1700066F RID: 1647
		// (get) Token: 0x060016EC RID: 5868 RVA: 0x000414E4 File Offset: 0x0003F6E4
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half4 xyxx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half4(this.x, this.y, this.x, this.x);
			}
		}

		// Token: 0x17000670 RID: 1648
		// (get) Token: 0x060016ED RID: 5869 RVA: 0x00041503 File Offset: 0x0003F703
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half4 xyxy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half4(this.x, this.y, this.x, this.y);
			}
		}

		// Token: 0x17000671 RID: 1649
		// (get) Token: 0x060016EE RID: 5870 RVA: 0x00041522 File Offset: 0x0003F722
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half4 xyxz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half4(this.x, this.y, this.x, this.z);
			}
		}

		// Token: 0x17000672 RID: 1650
		// (get) Token: 0x060016EF RID: 5871 RVA: 0x00041541 File Offset: 0x0003F741
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half4 xyxw
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half4(this.x, this.y, this.x, this.w);
			}
		}

		// Token: 0x17000673 RID: 1651
		// (get) Token: 0x060016F0 RID: 5872 RVA: 0x00041560 File Offset: 0x0003F760
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half4 xyyx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half4(this.x, this.y, this.y, this.x);
			}
		}

		// Token: 0x17000674 RID: 1652
		// (get) Token: 0x060016F1 RID: 5873 RVA: 0x0004157F File Offset: 0x0003F77F
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half4 xyyy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half4(this.x, this.y, this.y, this.y);
			}
		}

		// Token: 0x17000675 RID: 1653
		// (get) Token: 0x060016F2 RID: 5874 RVA: 0x0004159E File Offset: 0x0003F79E
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half4 xyyz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half4(this.x, this.y, this.y, this.z);
			}
		}

		// Token: 0x17000676 RID: 1654
		// (get) Token: 0x060016F3 RID: 5875 RVA: 0x000415BD File Offset: 0x0003F7BD
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half4 xyyw
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half4(this.x, this.y, this.y, this.w);
			}
		}

		// Token: 0x17000677 RID: 1655
		// (get) Token: 0x060016F4 RID: 5876 RVA: 0x000415DC File Offset: 0x0003F7DC
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half4 xyzx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half4(this.x, this.y, this.z, this.x);
			}
		}

		// Token: 0x17000678 RID: 1656
		// (get) Token: 0x060016F5 RID: 5877 RVA: 0x000415FB File Offset: 0x0003F7FB
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half4 xyzy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half4(this.x, this.y, this.z, this.y);
			}
		}

		// Token: 0x17000679 RID: 1657
		// (get) Token: 0x060016F6 RID: 5878 RVA: 0x0004161A File Offset: 0x0003F81A
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half4 xyzz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half4(this.x, this.y, this.z, this.z);
			}
		}

		// Token: 0x1700067A RID: 1658
		// (get) Token: 0x060016F7 RID: 5879 RVA: 0x00041639 File Offset: 0x0003F839
		// (set) Token: 0x060016F8 RID: 5880 RVA: 0x00041658 File Offset: 0x0003F858
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half4 xyzw
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half4(this.x, this.y, this.z, this.w);
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

		// Token: 0x1700067B RID: 1659
		// (get) Token: 0x060016F9 RID: 5881 RVA: 0x0004168A File Offset: 0x0003F88A
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half4 xywx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half4(this.x, this.y, this.w, this.x);
			}
		}

		// Token: 0x1700067C RID: 1660
		// (get) Token: 0x060016FA RID: 5882 RVA: 0x000416A9 File Offset: 0x0003F8A9
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half4 xywy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half4(this.x, this.y, this.w, this.y);
			}
		}

		// Token: 0x1700067D RID: 1661
		// (get) Token: 0x060016FB RID: 5883 RVA: 0x000416C8 File Offset: 0x0003F8C8
		// (set) Token: 0x060016FC RID: 5884 RVA: 0x000416E7 File Offset: 0x0003F8E7
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half4 xywz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half4(this.x, this.y, this.w, this.z);
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

		// Token: 0x1700067E RID: 1662
		// (get) Token: 0x060016FD RID: 5885 RVA: 0x00041719 File Offset: 0x0003F919
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half4 xyww
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half4(this.x, this.y, this.w, this.w);
			}
		}

		// Token: 0x1700067F RID: 1663
		// (get) Token: 0x060016FE RID: 5886 RVA: 0x00041738 File Offset: 0x0003F938
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half4 xzxx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half4(this.x, this.z, this.x, this.x);
			}
		}

		// Token: 0x17000680 RID: 1664
		// (get) Token: 0x060016FF RID: 5887 RVA: 0x00041757 File Offset: 0x0003F957
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half4 xzxy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half4(this.x, this.z, this.x, this.y);
			}
		}

		// Token: 0x17000681 RID: 1665
		// (get) Token: 0x06001700 RID: 5888 RVA: 0x00041776 File Offset: 0x0003F976
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half4 xzxz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half4(this.x, this.z, this.x, this.z);
			}
		}

		// Token: 0x17000682 RID: 1666
		// (get) Token: 0x06001701 RID: 5889 RVA: 0x00041795 File Offset: 0x0003F995
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half4 xzxw
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half4(this.x, this.z, this.x, this.w);
			}
		}

		// Token: 0x17000683 RID: 1667
		// (get) Token: 0x06001702 RID: 5890 RVA: 0x000417B4 File Offset: 0x0003F9B4
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half4 xzyx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half4(this.x, this.z, this.y, this.x);
			}
		}

		// Token: 0x17000684 RID: 1668
		// (get) Token: 0x06001703 RID: 5891 RVA: 0x000417D3 File Offset: 0x0003F9D3
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half4 xzyy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half4(this.x, this.z, this.y, this.y);
			}
		}

		// Token: 0x17000685 RID: 1669
		// (get) Token: 0x06001704 RID: 5892 RVA: 0x000417F2 File Offset: 0x0003F9F2
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half4 xzyz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half4(this.x, this.z, this.y, this.z);
			}
		}

		// Token: 0x17000686 RID: 1670
		// (get) Token: 0x06001705 RID: 5893 RVA: 0x00041811 File Offset: 0x0003FA11
		// (set) Token: 0x06001706 RID: 5894 RVA: 0x00041830 File Offset: 0x0003FA30
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half4 xzyw
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half4(this.x, this.z, this.y, this.w);
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

		// Token: 0x17000687 RID: 1671
		// (get) Token: 0x06001707 RID: 5895 RVA: 0x00041862 File Offset: 0x0003FA62
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half4 xzzx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half4(this.x, this.z, this.z, this.x);
			}
		}

		// Token: 0x17000688 RID: 1672
		// (get) Token: 0x06001708 RID: 5896 RVA: 0x00041881 File Offset: 0x0003FA81
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half4 xzzy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half4(this.x, this.z, this.z, this.y);
			}
		}

		// Token: 0x17000689 RID: 1673
		// (get) Token: 0x06001709 RID: 5897 RVA: 0x000418A0 File Offset: 0x0003FAA0
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half4 xzzz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half4(this.x, this.z, this.z, this.z);
			}
		}

		// Token: 0x1700068A RID: 1674
		// (get) Token: 0x0600170A RID: 5898 RVA: 0x000418BF File Offset: 0x0003FABF
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half4 xzzw
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half4(this.x, this.z, this.z, this.w);
			}
		}

		// Token: 0x1700068B RID: 1675
		// (get) Token: 0x0600170B RID: 5899 RVA: 0x000418DE File Offset: 0x0003FADE
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half4 xzwx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half4(this.x, this.z, this.w, this.x);
			}
		}

		// Token: 0x1700068C RID: 1676
		// (get) Token: 0x0600170C RID: 5900 RVA: 0x000418FD File Offset: 0x0003FAFD
		// (set) Token: 0x0600170D RID: 5901 RVA: 0x0004191C File Offset: 0x0003FB1C
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half4 xzwy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half4(this.x, this.z, this.w, this.y);
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

		// Token: 0x1700068D RID: 1677
		// (get) Token: 0x0600170E RID: 5902 RVA: 0x0004194E File Offset: 0x0003FB4E
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half4 xzwz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half4(this.x, this.z, this.w, this.z);
			}
		}

		// Token: 0x1700068E RID: 1678
		// (get) Token: 0x0600170F RID: 5903 RVA: 0x0004196D File Offset: 0x0003FB6D
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half4 xzww
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half4(this.x, this.z, this.w, this.w);
			}
		}

		// Token: 0x1700068F RID: 1679
		// (get) Token: 0x06001710 RID: 5904 RVA: 0x0004198C File Offset: 0x0003FB8C
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half4 xwxx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half4(this.x, this.w, this.x, this.x);
			}
		}

		// Token: 0x17000690 RID: 1680
		// (get) Token: 0x06001711 RID: 5905 RVA: 0x000419AB File Offset: 0x0003FBAB
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half4 xwxy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half4(this.x, this.w, this.x, this.y);
			}
		}

		// Token: 0x17000691 RID: 1681
		// (get) Token: 0x06001712 RID: 5906 RVA: 0x000419CA File Offset: 0x0003FBCA
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half4 xwxz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half4(this.x, this.w, this.x, this.z);
			}
		}

		// Token: 0x17000692 RID: 1682
		// (get) Token: 0x06001713 RID: 5907 RVA: 0x000419E9 File Offset: 0x0003FBE9
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half4 xwxw
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half4(this.x, this.w, this.x, this.w);
			}
		}

		// Token: 0x17000693 RID: 1683
		// (get) Token: 0x06001714 RID: 5908 RVA: 0x00041A08 File Offset: 0x0003FC08
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half4 xwyx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half4(this.x, this.w, this.y, this.x);
			}
		}

		// Token: 0x17000694 RID: 1684
		// (get) Token: 0x06001715 RID: 5909 RVA: 0x00041A27 File Offset: 0x0003FC27
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half4 xwyy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half4(this.x, this.w, this.y, this.y);
			}
		}

		// Token: 0x17000695 RID: 1685
		// (get) Token: 0x06001716 RID: 5910 RVA: 0x00041A46 File Offset: 0x0003FC46
		// (set) Token: 0x06001717 RID: 5911 RVA: 0x00041A65 File Offset: 0x0003FC65
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half4 xwyz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half4(this.x, this.w, this.y, this.z);
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

		// Token: 0x17000696 RID: 1686
		// (get) Token: 0x06001718 RID: 5912 RVA: 0x00041A97 File Offset: 0x0003FC97
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half4 xwyw
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half4(this.x, this.w, this.y, this.w);
			}
		}

		// Token: 0x17000697 RID: 1687
		// (get) Token: 0x06001719 RID: 5913 RVA: 0x00041AB6 File Offset: 0x0003FCB6
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half4 xwzx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half4(this.x, this.w, this.z, this.x);
			}
		}

		// Token: 0x17000698 RID: 1688
		// (get) Token: 0x0600171A RID: 5914 RVA: 0x00041AD5 File Offset: 0x0003FCD5
		// (set) Token: 0x0600171B RID: 5915 RVA: 0x00041AF4 File Offset: 0x0003FCF4
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half4 xwzy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half4(this.x, this.w, this.z, this.y);
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

		// Token: 0x17000699 RID: 1689
		// (get) Token: 0x0600171C RID: 5916 RVA: 0x00041B26 File Offset: 0x0003FD26
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half4 xwzz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half4(this.x, this.w, this.z, this.z);
			}
		}

		// Token: 0x1700069A RID: 1690
		// (get) Token: 0x0600171D RID: 5917 RVA: 0x00041B45 File Offset: 0x0003FD45
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half4 xwzw
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half4(this.x, this.w, this.z, this.w);
			}
		}

		// Token: 0x1700069B RID: 1691
		// (get) Token: 0x0600171E RID: 5918 RVA: 0x00041B64 File Offset: 0x0003FD64
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half4 xwwx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half4(this.x, this.w, this.w, this.x);
			}
		}

		// Token: 0x1700069C RID: 1692
		// (get) Token: 0x0600171F RID: 5919 RVA: 0x00041B83 File Offset: 0x0003FD83
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half4 xwwy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half4(this.x, this.w, this.w, this.y);
			}
		}

		// Token: 0x1700069D RID: 1693
		// (get) Token: 0x06001720 RID: 5920 RVA: 0x00041BA2 File Offset: 0x0003FDA2
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half4 xwwz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half4(this.x, this.w, this.w, this.z);
			}
		}

		// Token: 0x1700069E RID: 1694
		// (get) Token: 0x06001721 RID: 5921 RVA: 0x00041BC1 File Offset: 0x0003FDC1
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half4 xwww
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half4(this.x, this.w, this.w, this.w);
			}
		}

		// Token: 0x1700069F RID: 1695
		// (get) Token: 0x06001722 RID: 5922 RVA: 0x00041BE0 File Offset: 0x0003FDE0
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half4 yxxx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half4(this.y, this.x, this.x, this.x);
			}
		}

		// Token: 0x170006A0 RID: 1696
		// (get) Token: 0x06001723 RID: 5923 RVA: 0x00041BFF File Offset: 0x0003FDFF
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half4 yxxy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half4(this.y, this.x, this.x, this.y);
			}
		}

		// Token: 0x170006A1 RID: 1697
		// (get) Token: 0x06001724 RID: 5924 RVA: 0x00041C1E File Offset: 0x0003FE1E
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half4 yxxz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half4(this.y, this.x, this.x, this.z);
			}
		}

		// Token: 0x170006A2 RID: 1698
		// (get) Token: 0x06001725 RID: 5925 RVA: 0x00041C3D File Offset: 0x0003FE3D
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half4 yxxw
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half4(this.y, this.x, this.x, this.w);
			}
		}

		// Token: 0x170006A3 RID: 1699
		// (get) Token: 0x06001726 RID: 5926 RVA: 0x00041C5C File Offset: 0x0003FE5C
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half4 yxyx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half4(this.y, this.x, this.y, this.x);
			}
		}

		// Token: 0x170006A4 RID: 1700
		// (get) Token: 0x06001727 RID: 5927 RVA: 0x00041C7B File Offset: 0x0003FE7B
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half4 yxyy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half4(this.y, this.x, this.y, this.y);
			}
		}

		// Token: 0x170006A5 RID: 1701
		// (get) Token: 0x06001728 RID: 5928 RVA: 0x00041C9A File Offset: 0x0003FE9A
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half4 yxyz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half4(this.y, this.x, this.y, this.z);
			}
		}

		// Token: 0x170006A6 RID: 1702
		// (get) Token: 0x06001729 RID: 5929 RVA: 0x00041CB9 File Offset: 0x0003FEB9
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half4 yxyw
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half4(this.y, this.x, this.y, this.w);
			}
		}

		// Token: 0x170006A7 RID: 1703
		// (get) Token: 0x0600172A RID: 5930 RVA: 0x00041CD8 File Offset: 0x0003FED8
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half4 yxzx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half4(this.y, this.x, this.z, this.x);
			}
		}

		// Token: 0x170006A8 RID: 1704
		// (get) Token: 0x0600172B RID: 5931 RVA: 0x00041CF7 File Offset: 0x0003FEF7
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half4 yxzy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half4(this.y, this.x, this.z, this.y);
			}
		}

		// Token: 0x170006A9 RID: 1705
		// (get) Token: 0x0600172C RID: 5932 RVA: 0x00041D16 File Offset: 0x0003FF16
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half4 yxzz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half4(this.y, this.x, this.z, this.z);
			}
		}

		// Token: 0x170006AA RID: 1706
		// (get) Token: 0x0600172D RID: 5933 RVA: 0x00041D35 File Offset: 0x0003FF35
		// (set) Token: 0x0600172E RID: 5934 RVA: 0x00041D54 File Offset: 0x0003FF54
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half4 yxzw
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half4(this.y, this.x, this.z, this.w);
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

		// Token: 0x170006AB RID: 1707
		// (get) Token: 0x0600172F RID: 5935 RVA: 0x00041D86 File Offset: 0x0003FF86
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half4 yxwx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half4(this.y, this.x, this.w, this.x);
			}
		}

		// Token: 0x170006AC RID: 1708
		// (get) Token: 0x06001730 RID: 5936 RVA: 0x00041DA5 File Offset: 0x0003FFA5
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half4 yxwy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half4(this.y, this.x, this.w, this.y);
			}
		}

		// Token: 0x170006AD RID: 1709
		// (get) Token: 0x06001731 RID: 5937 RVA: 0x00041DC4 File Offset: 0x0003FFC4
		// (set) Token: 0x06001732 RID: 5938 RVA: 0x00041DE3 File Offset: 0x0003FFE3
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half4 yxwz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half4(this.y, this.x, this.w, this.z);
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

		// Token: 0x170006AE RID: 1710
		// (get) Token: 0x06001733 RID: 5939 RVA: 0x00041E15 File Offset: 0x00040015
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half4 yxww
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half4(this.y, this.x, this.w, this.w);
			}
		}

		// Token: 0x170006AF RID: 1711
		// (get) Token: 0x06001734 RID: 5940 RVA: 0x00041E34 File Offset: 0x00040034
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half4 yyxx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half4(this.y, this.y, this.x, this.x);
			}
		}

		// Token: 0x170006B0 RID: 1712
		// (get) Token: 0x06001735 RID: 5941 RVA: 0x00041E53 File Offset: 0x00040053
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half4 yyxy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half4(this.y, this.y, this.x, this.y);
			}
		}

		// Token: 0x170006B1 RID: 1713
		// (get) Token: 0x06001736 RID: 5942 RVA: 0x00041E72 File Offset: 0x00040072
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half4 yyxz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half4(this.y, this.y, this.x, this.z);
			}
		}

		// Token: 0x170006B2 RID: 1714
		// (get) Token: 0x06001737 RID: 5943 RVA: 0x00041E91 File Offset: 0x00040091
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half4 yyxw
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half4(this.y, this.y, this.x, this.w);
			}
		}

		// Token: 0x170006B3 RID: 1715
		// (get) Token: 0x06001738 RID: 5944 RVA: 0x00041EB0 File Offset: 0x000400B0
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half4 yyyx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half4(this.y, this.y, this.y, this.x);
			}
		}

		// Token: 0x170006B4 RID: 1716
		// (get) Token: 0x06001739 RID: 5945 RVA: 0x00041ECF File Offset: 0x000400CF
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half4 yyyy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half4(this.y, this.y, this.y, this.y);
			}
		}

		// Token: 0x170006B5 RID: 1717
		// (get) Token: 0x0600173A RID: 5946 RVA: 0x00041EEE File Offset: 0x000400EE
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half4 yyyz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half4(this.y, this.y, this.y, this.z);
			}
		}

		// Token: 0x170006B6 RID: 1718
		// (get) Token: 0x0600173B RID: 5947 RVA: 0x00041F0D File Offset: 0x0004010D
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half4 yyyw
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half4(this.y, this.y, this.y, this.w);
			}
		}

		// Token: 0x170006B7 RID: 1719
		// (get) Token: 0x0600173C RID: 5948 RVA: 0x00041F2C File Offset: 0x0004012C
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half4 yyzx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half4(this.y, this.y, this.z, this.x);
			}
		}

		// Token: 0x170006B8 RID: 1720
		// (get) Token: 0x0600173D RID: 5949 RVA: 0x00041F4B File Offset: 0x0004014B
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half4 yyzy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half4(this.y, this.y, this.z, this.y);
			}
		}

		// Token: 0x170006B9 RID: 1721
		// (get) Token: 0x0600173E RID: 5950 RVA: 0x00041F6A File Offset: 0x0004016A
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half4 yyzz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half4(this.y, this.y, this.z, this.z);
			}
		}

		// Token: 0x170006BA RID: 1722
		// (get) Token: 0x0600173F RID: 5951 RVA: 0x00041F89 File Offset: 0x00040189
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half4 yyzw
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half4(this.y, this.y, this.z, this.w);
			}
		}

		// Token: 0x170006BB RID: 1723
		// (get) Token: 0x06001740 RID: 5952 RVA: 0x00041FA8 File Offset: 0x000401A8
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half4 yywx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half4(this.y, this.y, this.w, this.x);
			}
		}

		// Token: 0x170006BC RID: 1724
		// (get) Token: 0x06001741 RID: 5953 RVA: 0x00041FC7 File Offset: 0x000401C7
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half4 yywy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half4(this.y, this.y, this.w, this.y);
			}
		}

		// Token: 0x170006BD RID: 1725
		// (get) Token: 0x06001742 RID: 5954 RVA: 0x00041FE6 File Offset: 0x000401E6
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half4 yywz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half4(this.y, this.y, this.w, this.z);
			}
		}

		// Token: 0x170006BE RID: 1726
		// (get) Token: 0x06001743 RID: 5955 RVA: 0x00042005 File Offset: 0x00040205
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half4 yyww
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half4(this.y, this.y, this.w, this.w);
			}
		}

		// Token: 0x170006BF RID: 1727
		// (get) Token: 0x06001744 RID: 5956 RVA: 0x00042024 File Offset: 0x00040224
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half4 yzxx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half4(this.y, this.z, this.x, this.x);
			}
		}

		// Token: 0x170006C0 RID: 1728
		// (get) Token: 0x06001745 RID: 5957 RVA: 0x00042043 File Offset: 0x00040243
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half4 yzxy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half4(this.y, this.z, this.x, this.y);
			}
		}

		// Token: 0x170006C1 RID: 1729
		// (get) Token: 0x06001746 RID: 5958 RVA: 0x00042062 File Offset: 0x00040262
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half4 yzxz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half4(this.y, this.z, this.x, this.z);
			}
		}

		// Token: 0x170006C2 RID: 1730
		// (get) Token: 0x06001747 RID: 5959 RVA: 0x00042081 File Offset: 0x00040281
		// (set) Token: 0x06001748 RID: 5960 RVA: 0x000420A0 File Offset: 0x000402A0
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half4 yzxw
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half4(this.y, this.z, this.x, this.w);
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

		// Token: 0x170006C3 RID: 1731
		// (get) Token: 0x06001749 RID: 5961 RVA: 0x000420D2 File Offset: 0x000402D2
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half4 yzyx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half4(this.y, this.z, this.y, this.x);
			}
		}

		// Token: 0x170006C4 RID: 1732
		// (get) Token: 0x0600174A RID: 5962 RVA: 0x000420F1 File Offset: 0x000402F1
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half4 yzyy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half4(this.y, this.z, this.y, this.y);
			}
		}

		// Token: 0x170006C5 RID: 1733
		// (get) Token: 0x0600174B RID: 5963 RVA: 0x00042110 File Offset: 0x00040310
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half4 yzyz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half4(this.y, this.z, this.y, this.z);
			}
		}

		// Token: 0x170006C6 RID: 1734
		// (get) Token: 0x0600174C RID: 5964 RVA: 0x0004212F File Offset: 0x0004032F
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half4 yzyw
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half4(this.y, this.z, this.y, this.w);
			}
		}

		// Token: 0x170006C7 RID: 1735
		// (get) Token: 0x0600174D RID: 5965 RVA: 0x0004214E File Offset: 0x0004034E
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half4 yzzx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half4(this.y, this.z, this.z, this.x);
			}
		}

		// Token: 0x170006C8 RID: 1736
		// (get) Token: 0x0600174E RID: 5966 RVA: 0x0004216D File Offset: 0x0004036D
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half4 yzzy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half4(this.y, this.z, this.z, this.y);
			}
		}

		// Token: 0x170006C9 RID: 1737
		// (get) Token: 0x0600174F RID: 5967 RVA: 0x0004218C File Offset: 0x0004038C
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half4 yzzz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half4(this.y, this.z, this.z, this.z);
			}
		}

		// Token: 0x170006CA RID: 1738
		// (get) Token: 0x06001750 RID: 5968 RVA: 0x000421AB File Offset: 0x000403AB
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half4 yzzw
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half4(this.y, this.z, this.z, this.w);
			}
		}

		// Token: 0x170006CB RID: 1739
		// (get) Token: 0x06001751 RID: 5969 RVA: 0x000421CA File Offset: 0x000403CA
		// (set) Token: 0x06001752 RID: 5970 RVA: 0x000421E9 File Offset: 0x000403E9
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half4 yzwx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half4(this.y, this.z, this.w, this.x);
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

		// Token: 0x170006CC RID: 1740
		// (get) Token: 0x06001753 RID: 5971 RVA: 0x0004221B File Offset: 0x0004041B
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half4 yzwy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half4(this.y, this.z, this.w, this.y);
			}
		}

		// Token: 0x170006CD RID: 1741
		// (get) Token: 0x06001754 RID: 5972 RVA: 0x0004223A File Offset: 0x0004043A
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half4 yzwz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half4(this.y, this.z, this.w, this.z);
			}
		}

		// Token: 0x170006CE RID: 1742
		// (get) Token: 0x06001755 RID: 5973 RVA: 0x00042259 File Offset: 0x00040459
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half4 yzww
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half4(this.y, this.z, this.w, this.w);
			}
		}

		// Token: 0x170006CF RID: 1743
		// (get) Token: 0x06001756 RID: 5974 RVA: 0x00042278 File Offset: 0x00040478
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half4 ywxx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half4(this.y, this.w, this.x, this.x);
			}
		}

		// Token: 0x170006D0 RID: 1744
		// (get) Token: 0x06001757 RID: 5975 RVA: 0x00042297 File Offset: 0x00040497
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half4 ywxy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half4(this.y, this.w, this.x, this.y);
			}
		}

		// Token: 0x170006D1 RID: 1745
		// (get) Token: 0x06001758 RID: 5976 RVA: 0x000422B6 File Offset: 0x000404B6
		// (set) Token: 0x06001759 RID: 5977 RVA: 0x000422D5 File Offset: 0x000404D5
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half4 ywxz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half4(this.y, this.w, this.x, this.z);
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

		// Token: 0x170006D2 RID: 1746
		// (get) Token: 0x0600175A RID: 5978 RVA: 0x00042307 File Offset: 0x00040507
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half4 ywxw
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half4(this.y, this.w, this.x, this.w);
			}
		}

		// Token: 0x170006D3 RID: 1747
		// (get) Token: 0x0600175B RID: 5979 RVA: 0x00042326 File Offset: 0x00040526
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half4 ywyx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half4(this.y, this.w, this.y, this.x);
			}
		}

		// Token: 0x170006D4 RID: 1748
		// (get) Token: 0x0600175C RID: 5980 RVA: 0x00042345 File Offset: 0x00040545
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half4 ywyy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half4(this.y, this.w, this.y, this.y);
			}
		}

		// Token: 0x170006D5 RID: 1749
		// (get) Token: 0x0600175D RID: 5981 RVA: 0x00042364 File Offset: 0x00040564
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half4 ywyz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half4(this.y, this.w, this.y, this.z);
			}
		}

		// Token: 0x170006D6 RID: 1750
		// (get) Token: 0x0600175E RID: 5982 RVA: 0x00042383 File Offset: 0x00040583
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half4 ywyw
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half4(this.y, this.w, this.y, this.w);
			}
		}

		// Token: 0x170006D7 RID: 1751
		// (get) Token: 0x0600175F RID: 5983 RVA: 0x000423A2 File Offset: 0x000405A2
		// (set) Token: 0x06001760 RID: 5984 RVA: 0x000423C1 File Offset: 0x000405C1
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half4 ywzx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half4(this.y, this.w, this.z, this.x);
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

		// Token: 0x170006D8 RID: 1752
		// (get) Token: 0x06001761 RID: 5985 RVA: 0x000423F3 File Offset: 0x000405F3
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half4 ywzy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half4(this.y, this.w, this.z, this.y);
			}
		}

		// Token: 0x170006D9 RID: 1753
		// (get) Token: 0x06001762 RID: 5986 RVA: 0x00042412 File Offset: 0x00040612
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half4 ywzz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half4(this.y, this.w, this.z, this.z);
			}
		}

		// Token: 0x170006DA RID: 1754
		// (get) Token: 0x06001763 RID: 5987 RVA: 0x00042431 File Offset: 0x00040631
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half4 ywzw
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half4(this.y, this.w, this.z, this.w);
			}
		}

		// Token: 0x170006DB RID: 1755
		// (get) Token: 0x06001764 RID: 5988 RVA: 0x00042450 File Offset: 0x00040650
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half4 ywwx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half4(this.y, this.w, this.w, this.x);
			}
		}

		// Token: 0x170006DC RID: 1756
		// (get) Token: 0x06001765 RID: 5989 RVA: 0x0004246F File Offset: 0x0004066F
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half4 ywwy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half4(this.y, this.w, this.w, this.y);
			}
		}

		// Token: 0x170006DD RID: 1757
		// (get) Token: 0x06001766 RID: 5990 RVA: 0x0004248E File Offset: 0x0004068E
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half4 ywwz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half4(this.y, this.w, this.w, this.z);
			}
		}

		// Token: 0x170006DE RID: 1758
		// (get) Token: 0x06001767 RID: 5991 RVA: 0x000424AD File Offset: 0x000406AD
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half4 ywww
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half4(this.y, this.w, this.w, this.w);
			}
		}

		// Token: 0x170006DF RID: 1759
		// (get) Token: 0x06001768 RID: 5992 RVA: 0x000424CC File Offset: 0x000406CC
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half4 zxxx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half4(this.z, this.x, this.x, this.x);
			}
		}

		// Token: 0x170006E0 RID: 1760
		// (get) Token: 0x06001769 RID: 5993 RVA: 0x000424EB File Offset: 0x000406EB
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half4 zxxy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half4(this.z, this.x, this.x, this.y);
			}
		}

		// Token: 0x170006E1 RID: 1761
		// (get) Token: 0x0600176A RID: 5994 RVA: 0x0004250A File Offset: 0x0004070A
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half4 zxxz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half4(this.z, this.x, this.x, this.z);
			}
		}

		// Token: 0x170006E2 RID: 1762
		// (get) Token: 0x0600176B RID: 5995 RVA: 0x00042529 File Offset: 0x00040729
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half4 zxxw
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half4(this.z, this.x, this.x, this.w);
			}
		}

		// Token: 0x170006E3 RID: 1763
		// (get) Token: 0x0600176C RID: 5996 RVA: 0x00042548 File Offset: 0x00040748
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half4 zxyx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half4(this.z, this.x, this.y, this.x);
			}
		}

		// Token: 0x170006E4 RID: 1764
		// (get) Token: 0x0600176D RID: 5997 RVA: 0x00042567 File Offset: 0x00040767
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half4 zxyy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half4(this.z, this.x, this.y, this.y);
			}
		}

		// Token: 0x170006E5 RID: 1765
		// (get) Token: 0x0600176E RID: 5998 RVA: 0x00042586 File Offset: 0x00040786
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half4 zxyz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half4(this.z, this.x, this.y, this.z);
			}
		}

		// Token: 0x170006E6 RID: 1766
		// (get) Token: 0x0600176F RID: 5999 RVA: 0x000425A5 File Offset: 0x000407A5
		// (set) Token: 0x06001770 RID: 6000 RVA: 0x000425C4 File Offset: 0x000407C4
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half4 zxyw
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half4(this.z, this.x, this.y, this.w);
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

		// Token: 0x170006E7 RID: 1767
		// (get) Token: 0x06001771 RID: 6001 RVA: 0x000425F6 File Offset: 0x000407F6
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half4 zxzx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half4(this.z, this.x, this.z, this.x);
			}
		}

		// Token: 0x170006E8 RID: 1768
		// (get) Token: 0x06001772 RID: 6002 RVA: 0x00042615 File Offset: 0x00040815
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half4 zxzy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half4(this.z, this.x, this.z, this.y);
			}
		}

		// Token: 0x170006E9 RID: 1769
		// (get) Token: 0x06001773 RID: 6003 RVA: 0x00042634 File Offset: 0x00040834
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half4 zxzz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half4(this.z, this.x, this.z, this.z);
			}
		}

		// Token: 0x170006EA RID: 1770
		// (get) Token: 0x06001774 RID: 6004 RVA: 0x00042653 File Offset: 0x00040853
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half4 zxzw
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half4(this.z, this.x, this.z, this.w);
			}
		}

		// Token: 0x170006EB RID: 1771
		// (get) Token: 0x06001775 RID: 6005 RVA: 0x00042672 File Offset: 0x00040872
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half4 zxwx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half4(this.z, this.x, this.w, this.x);
			}
		}

		// Token: 0x170006EC RID: 1772
		// (get) Token: 0x06001776 RID: 6006 RVA: 0x00042691 File Offset: 0x00040891
		// (set) Token: 0x06001777 RID: 6007 RVA: 0x000426B0 File Offset: 0x000408B0
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half4 zxwy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half4(this.z, this.x, this.w, this.y);
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

		// Token: 0x170006ED RID: 1773
		// (get) Token: 0x06001778 RID: 6008 RVA: 0x000426E2 File Offset: 0x000408E2
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half4 zxwz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half4(this.z, this.x, this.w, this.z);
			}
		}

		// Token: 0x170006EE RID: 1774
		// (get) Token: 0x06001779 RID: 6009 RVA: 0x00042701 File Offset: 0x00040901
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half4 zxww
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half4(this.z, this.x, this.w, this.w);
			}
		}

		// Token: 0x170006EF RID: 1775
		// (get) Token: 0x0600177A RID: 6010 RVA: 0x00042720 File Offset: 0x00040920
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half4 zyxx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half4(this.z, this.y, this.x, this.x);
			}
		}

		// Token: 0x170006F0 RID: 1776
		// (get) Token: 0x0600177B RID: 6011 RVA: 0x0004273F File Offset: 0x0004093F
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half4 zyxy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half4(this.z, this.y, this.x, this.y);
			}
		}

		// Token: 0x170006F1 RID: 1777
		// (get) Token: 0x0600177C RID: 6012 RVA: 0x0004275E File Offset: 0x0004095E
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half4 zyxz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half4(this.z, this.y, this.x, this.z);
			}
		}

		// Token: 0x170006F2 RID: 1778
		// (get) Token: 0x0600177D RID: 6013 RVA: 0x0004277D File Offset: 0x0004097D
		// (set) Token: 0x0600177E RID: 6014 RVA: 0x0004279C File Offset: 0x0004099C
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half4 zyxw
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half4(this.z, this.y, this.x, this.w);
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

		// Token: 0x170006F3 RID: 1779
		// (get) Token: 0x0600177F RID: 6015 RVA: 0x000427CE File Offset: 0x000409CE
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half4 zyyx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half4(this.z, this.y, this.y, this.x);
			}
		}

		// Token: 0x170006F4 RID: 1780
		// (get) Token: 0x06001780 RID: 6016 RVA: 0x000427ED File Offset: 0x000409ED
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half4 zyyy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half4(this.z, this.y, this.y, this.y);
			}
		}

		// Token: 0x170006F5 RID: 1781
		// (get) Token: 0x06001781 RID: 6017 RVA: 0x0004280C File Offset: 0x00040A0C
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half4 zyyz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half4(this.z, this.y, this.y, this.z);
			}
		}

		// Token: 0x170006F6 RID: 1782
		// (get) Token: 0x06001782 RID: 6018 RVA: 0x0004282B File Offset: 0x00040A2B
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half4 zyyw
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half4(this.z, this.y, this.y, this.w);
			}
		}

		// Token: 0x170006F7 RID: 1783
		// (get) Token: 0x06001783 RID: 6019 RVA: 0x0004284A File Offset: 0x00040A4A
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half4 zyzx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half4(this.z, this.y, this.z, this.x);
			}
		}

		// Token: 0x170006F8 RID: 1784
		// (get) Token: 0x06001784 RID: 6020 RVA: 0x00042869 File Offset: 0x00040A69
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half4 zyzy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half4(this.z, this.y, this.z, this.y);
			}
		}

		// Token: 0x170006F9 RID: 1785
		// (get) Token: 0x06001785 RID: 6021 RVA: 0x00042888 File Offset: 0x00040A88
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half4 zyzz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half4(this.z, this.y, this.z, this.z);
			}
		}

		// Token: 0x170006FA RID: 1786
		// (get) Token: 0x06001786 RID: 6022 RVA: 0x000428A7 File Offset: 0x00040AA7
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half4 zyzw
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half4(this.z, this.y, this.z, this.w);
			}
		}

		// Token: 0x170006FB RID: 1787
		// (get) Token: 0x06001787 RID: 6023 RVA: 0x000428C6 File Offset: 0x00040AC6
		// (set) Token: 0x06001788 RID: 6024 RVA: 0x000428E5 File Offset: 0x00040AE5
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half4 zywx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half4(this.z, this.y, this.w, this.x);
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

		// Token: 0x170006FC RID: 1788
		// (get) Token: 0x06001789 RID: 6025 RVA: 0x00042917 File Offset: 0x00040B17
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half4 zywy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half4(this.z, this.y, this.w, this.y);
			}
		}

		// Token: 0x170006FD RID: 1789
		// (get) Token: 0x0600178A RID: 6026 RVA: 0x00042936 File Offset: 0x00040B36
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half4 zywz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half4(this.z, this.y, this.w, this.z);
			}
		}

		// Token: 0x170006FE RID: 1790
		// (get) Token: 0x0600178B RID: 6027 RVA: 0x00042955 File Offset: 0x00040B55
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half4 zyww
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half4(this.z, this.y, this.w, this.w);
			}
		}

		// Token: 0x170006FF RID: 1791
		// (get) Token: 0x0600178C RID: 6028 RVA: 0x00042974 File Offset: 0x00040B74
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half4 zzxx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half4(this.z, this.z, this.x, this.x);
			}
		}

		// Token: 0x17000700 RID: 1792
		// (get) Token: 0x0600178D RID: 6029 RVA: 0x00042993 File Offset: 0x00040B93
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half4 zzxy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half4(this.z, this.z, this.x, this.y);
			}
		}

		// Token: 0x17000701 RID: 1793
		// (get) Token: 0x0600178E RID: 6030 RVA: 0x000429B2 File Offset: 0x00040BB2
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half4 zzxz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half4(this.z, this.z, this.x, this.z);
			}
		}

		// Token: 0x17000702 RID: 1794
		// (get) Token: 0x0600178F RID: 6031 RVA: 0x000429D1 File Offset: 0x00040BD1
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half4 zzxw
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half4(this.z, this.z, this.x, this.w);
			}
		}

		// Token: 0x17000703 RID: 1795
		// (get) Token: 0x06001790 RID: 6032 RVA: 0x000429F0 File Offset: 0x00040BF0
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half4 zzyx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half4(this.z, this.z, this.y, this.x);
			}
		}

		// Token: 0x17000704 RID: 1796
		// (get) Token: 0x06001791 RID: 6033 RVA: 0x00042A0F File Offset: 0x00040C0F
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half4 zzyy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half4(this.z, this.z, this.y, this.y);
			}
		}

		// Token: 0x17000705 RID: 1797
		// (get) Token: 0x06001792 RID: 6034 RVA: 0x00042A2E File Offset: 0x00040C2E
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half4 zzyz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half4(this.z, this.z, this.y, this.z);
			}
		}

		// Token: 0x17000706 RID: 1798
		// (get) Token: 0x06001793 RID: 6035 RVA: 0x00042A4D File Offset: 0x00040C4D
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half4 zzyw
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half4(this.z, this.z, this.y, this.w);
			}
		}

		// Token: 0x17000707 RID: 1799
		// (get) Token: 0x06001794 RID: 6036 RVA: 0x00042A6C File Offset: 0x00040C6C
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half4 zzzx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half4(this.z, this.z, this.z, this.x);
			}
		}

		// Token: 0x17000708 RID: 1800
		// (get) Token: 0x06001795 RID: 6037 RVA: 0x00042A8B File Offset: 0x00040C8B
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half4 zzzy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half4(this.z, this.z, this.z, this.y);
			}
		}

		// Token: 0x17000709 RID: 1801
		// (get) Token: 0x06001796 RID: 6038 RVA: 0x00042AAA File Offset: 0x00040CAA
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half4 zzzz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half4(this.z, this.z, this.z, this.z);
			}
		}

		// Token: 0x1700070A RID: 1802
		// (get) Token: 0x06001797 RID: 6039 RVA: 0x00042AC9 File Offset: 0x00040CC9
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half4 zzzw
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half4(this.z, this.z, this.z, this.w);
			}
		}

		// Token: 0x1700070B RID: 1803
		// (get) Token: 0x06001798 RID: 6040 RVA: 0x00042AE8 File Offset: 0x00040CE8
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half4 zzwx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half4(this.z, this.z, this.w, this.x);
			}
		}

		// Token: 0x1700070C RID: 1804
		// (get) Token: 0x06001799 RID: 6041 RVA: 0x00042B07 File Offset: 0x00040D07
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half4 zzwy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half4(this.z, this.z, this.w, this.y);
			}
		}

		// Token: 0x1700070D RID: 1805
		// (get) Token: 0x0600179A RID: 6042 RVA: 0x00042B26 File Offset: 0x00040D26
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half4 zzwz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half4(this.z, this.z, this.w, this.z);
			}
		}

		// Token: 0x1700070E RID: 1806
		// (get) Token: 0x0600179B RID: 6043 RVA: 0x00042B45 File Offset: 0x00040D45
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half4 zzww
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half4(this.z, this.z, this.w, this.w);
			}
		}

		// Token: 0x1700070F RID: 1807
		// (get) Token: 0x0600179C RID: 6044 RVA: 0x00042B64 File Offset: 0x00040D64
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half4 zwxx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half4(this.z, this.w, this.x, this.x);
			}
		}

		// Token: 0x17000710 RID: 1808
		// (get) Token: 0x0600179D RID: 6045 RVA: 0x00042B83 File Offset: 0x00040D83
		// (set) Token: 0x0600179E RID: 6046 RVA: 0x00042BA2 File Offset: 0x00040DA2
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half4 zwxy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half4(this.z, this.w, this.x, this.y);
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

		// Token: 0x17000711 RID: 1809
		// (get) Token: 0x0600179F RID: 6047 RVA: 0x00042BD4 File Offset: 0x00040DD4
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half4 zwxz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half4(this.z, this.w, this.x, this.z);
			}
		}

		// Token: 0x17000712 RID: 1810
		// (get) Token: 0x060017A0 RID: 6048 RVA: 0x00042BF3 File Offset: 0x00040DF3
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half4 zwxw
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half4(this.z, this.w, this.x, this.w);
			}
		}

		// Token: 0x17000713 RID: 1811
		// (get) Token: 0x060017A1 RID: 6049 RVA: 0x00042C12 File Offset: 0x00040E12
		// (set) Token: 0x060017A2 RID: 6050 RVA: 0x00042C31 File Offset: 0x00040E31
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half4 zwyx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half4(this.z, this.w, this.y, this.x);
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

		// Token: 0x17000714 RID: 1812
		// (get) Token: 0x060017A3 RID: 6051 RVA: 0x00042C63 File Offset: 0x00040E63
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half4 zwyy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half4(this.z, this.w, this.y, this.y);
			}
		}

		// Token: 0x17000715 RID: 1813
		// (get) Token: 0x060017A4 RID: 6052 RVA: 0x00042C82 File Offset: 0x00040E82
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half4 zwyz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half4(this.z, this.w, this.y, this.z);
			}
		}

		// Token: 0x17000716 RID: 1814
		// (get) Token: 0x060017A5 RID: 6053 RVA: 0x00042CA1 File Offset: 0x00040EA1
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half4 zwyw
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half4(this.z, this.w, this.y, this.w);
			}
		}

		// Token: 0x17000717 RID: 1815
		// (get) Token: 0x060017A6 RID: 6054 RVA: 0x00042CC0 File Offset: 0x00040EC0
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half4 zwzx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half4(this.z, this.w, this.z, this.x);
			}
		}

		// Token: 0x17000718 RID: 1816
		// (get) Token: 0x060017A7 RID: 6055 RVA: 0x00042CDF File Offset: 0x00040EDF
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half4 zwzy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half4(this.z, this.w, this.z, this.y);
			}
		}

		// Token: 0x17000719 RID: 1817
		// (get) Token: 0x060017A8 RID: 6056 RVA: 0x00042CFE File Offset: 0x00040EFE
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half4 zwzz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half4(this.z, this.w, this.z, this.z);
			}
		}

		// Token: 0x1700071A RID: 1818
		// (get) Token: 0x060017A9 RID: 6057 RVA: 0x00042D1D File Offset: 0x00040F1D
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half4 zwzw
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half4(this.z, this.w, this.z, this.w);
			}
		}

		// Token: 0x1700071B RID: 1819
		// (get) Token: 0x060017AA RID: 6058 RVA: 0x00042D3C File Offset: 0x00040F3C
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half4 zwwx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half4(this.z, this.w, this.w, this.x);
			}
		}

		// Token: 0x1700071C RID: 1820
		// (get) Token: 0x060017AB RID: 6059 RVA: 0x00042D5B File Offset: 0x00040F5B
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half4 zwwy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half4(this.z, this.w, this.w, this.y);
			}
		}

		// Token: 0x1700071D RID: 1821
		// (get) Token: 0x060017AC RID: 6060 RVA: 0x00042D7A File Offset: 0x00040F7A
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half4 zwwz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half4(this.z, this.w, this.w, this.z);
			}
		}

		// Token: 0x1700071E RID: 1822
		// (get) Token: 0x060017AD RID: 6061 RVA: 0x00042D99 File Offset: 0x00040F99
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half4 zwww
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half4(this.z, this.w, this.w, this.w);
			}
		}

		// Token: 0x1700071F RID: 1823
		// (get) Token: 0x060017AE RID: 6062 RVA: 0x00042DB8 File Offset: 0x00040FB8
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half4 wxxx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half4(this.w, this.x, this.x, this.x);
			}
		}

		// Token: 0x17000720 RID: 1824
		// (get) Token: 0x060017AF RID: 6063 RVA: 0x00042DD7 File Offset: 0x00040FD7
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half4 wxxy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half4(this.w, this.x, this.x, this.y);
			}
		}

		// Token: 0x17000721 RID: 1825
		// (get) Token: 0x060017B0 RID: 6064 RVA: 0x00042DF6 File Offset: 0x00040FF6
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half4 wxxz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half4(this.w, this.x, this.x, this.z);
			}
		}

		// Token: 0x17000722 RID: 1826
		// (get) Token: 0x060017B1 RID: 6065 RVA: 0x00042E15 File Offset: 0x00041015
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half4 wxxw
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half4(this.w, this.x, this.x, this.w);
			}
		}

		// Token: 0x17000723 RID: 1827
		// (get) Token: 0x060017B2 RID: 6066 RVA: 0x00042E34 File Offset: 0x00041034
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half4 wxyx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half4(this.w, this.x, this.y, this.x);
			}
		}

		// Token: 0x17000724 RID: 1828
		// (get) Token: 0x060017B3 RID: 6067 RVA: 0x00042E53 File Offset: 0x00041053
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half4 wxyy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half4(this.w, this.x, this.y, this.y);
			}
		}

		// Token: 0x17000725 RID: 1829
		// (get) Token: 0x060017B4 RID: 6068 RVA: 0x00042E72 File Offset: 0x00041072
		// (set) Token: 0x060017B5 RID: 6069 RVA: 0x00042E91 File Offset: 0x00041091
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half4 wxyz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half4(this.w, this.x, this.y, this.z);
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

		// Token: 0x17000726 RID: 1830
		// (get) Token: 0x060017B6 RID: 6070 RVA: 0x00042EC3 File Offset: 0x000410C3
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half4 wxyw
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half4(this.w, this.x, this.y, this.w);
			}
		}

		// Token: 0x17000727 RID: 1831
		// (get) Token: 0x060017B7 RID: 6071 RVA: 0x00042EE2 File Offset: 0x000410E2
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half4 wxzx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half4(this.w, this.x, this.z, this.x);
			}
		}

		// Token: 0x17000728 RID: 1832
		// (get) Token: 0x060017B8 RID: 6072 RVA: 0x00042F01 File Offset: 0x00041101
		// (set) Token: 0x060017B9 RID: 6073 RVA: 0x00042F20 File Offset: 0x00041120
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half4 wxzy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half4(this.w, this.x, this.z, this.y);
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

		// Token: 0x17000729 RID: 1833
		// (get) Token: 0x060017BA RID: 6074 RVA: 0x00042F52 File Offset: 0x00041152
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half4 wxzz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half4(this.w, this.x, this.z, this.z);
			}
		}

		// Token: 0x1700072A RID: 1834
		// (get) Token: 0x060017BB RID: 6075 RVA: 0x00042F71 File Offset: 0x00041171
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half4 wxzw
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half4(this.w, this.x, this.z, this.w);
			}
		}

		// Token: 0x1700072B RID: 1835
		// (get) Token: 0x060017BC RID: 6076 RVA: 0x00042F90 File Offset: 0x00041190
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half4 wxwx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half4(this.w, this.x, this.w, this.x);
			}
		}

		// Token: 0x1700072C RID: 1836
		// (get) Token: 0x060017BD RID: 6077 RVA: 0x00042FAF File Offset: 0x000411AF
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half4 wxwy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half4(this.w, this.x, this.w, this.y);
			}
		}

		// Token: 0x1700072D RID: 1837
		// (get) Token: 0x060017BE RID: 6078 RVA: 0x00042FCE File Offset: 0x000411CE
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half4 wxwz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half4(this.w, this.x, this.w, this.z);
			}
		}

		// Token: 0x1700072E RID: 1838
		// (get) Token: 0x060017BF RID: 6079 RVA: 0x00042FED File Offset: 0x000411ED
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half4 wxww
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half4(this.w, this.x, this.w, this.w);
			}
		}

		// Token: 0x1700072F RID: 1839
		// (get) Token: 0x060017C0 RID: 6080 RVA: 0x0004300C File Offset: 0x0004120C
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half4 wyxx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half4(this.w, this.y, this.x, this.x);
			}
		}

		// Token: 0x17000730 RID: 1840
		// (get) Token: 0x060017C1 RID: 6081 RVA: 0x0004302B File Offset: 0x0004122B
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half4 wyxy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half4(this.w, this.y, this.x, this.y);
			}
		}

		// Token: 0x17000731 RID: 1841
		// (get) Token: 0x060017C2 RID: 6082 RVA: 0x0004304A File Offset: 0x0004124A
		// (set) Token: 0x060017C3 RID: 6083 RVA: 0x00043069 File Offset: 0x00041269
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half4 wyxz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half4(this.w, this.y, this.x, this.z);
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

		// Token: 0x17000732 RID: 1842
		// (get) Token: 0x060017C4 RID: 6084 RVA: 0x0004309B File Offset: 0x0004129B
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half4 wyxw
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half4(this.w, this.y, this.x, this.w);
			}
		}

		// Token: 0x17000733 RID: 1843
		// (get) Token: 0x060017C5 RID: 6085 RVA: 0x000430BA File Offset: 0x000412BA
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half4 wyyx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half4(this.w, this.y, this.y, this.x);
			}
		}

		// Token: 0x17000734 RID: 1844
		// (get) Token: 0x060017C6 RID: 6086 RVA: 0x000430D9 File Offset: 0x000412D9
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half4 wyyy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half4(this.w, this.y, this.y, this.y);
			}
		}

		// Token: 0x17000735 RID: 1845
		// (get) Token: 0x060017C7 RID: 6087 RVA: 0x000430F8 File Offset: 0x000412F8
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half4 wyyz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half4(this.w, this.y, this.y, this.z);
			}
		}

		// Token: 0x17000736 RID: 1846
		// (get) Token: 0x060017C8 RID: 6088 RVA: 0x00043117 File Offset: 0x00041317
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half4 wyyw
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half4(this.w, this.y, this.y, this.w);
			}
		}

		// Token: 0x17000737 RID: 1847
		// (get) Token: 0x060017C9 RID: 6089 RVA: 0x00043136 File Offset: 0x00041336
		// (set) Token: 0x060017CA RID: 6090 RVA: 0x00043155 File Offset: 0x00041355
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half4 wyzx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half4(this.w, this.y, this.z, this.x);
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

		// Token: 0x17000738 RID: 1848
		// (get) Token: 0x060017CB RID: 6091 RVA: 0x00043187 File Offset: 0x00041387
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half4 wyzy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half4(this.w, this.y, this.z, this.y);
			}
		}

		// Token: 0x17000739 RID: 1849
		// (get) Token: 0x060017CC RID: 6092 RVA: 0x000431A6 File Offset: 0x000413A6
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half4 wyzz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half4(this.w, this.y, this.z, this.z);
			}
		}

		// Token: 0x1700073A RID: 1850
		// (get) Token: 0x060017CD RID: 6093 RVA: 0x000431C5 File Offset: 0x000413C5
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half4 wyzw
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half4(this.w, this.y, this.z, this.w);
			}
		}

		// Token: 0x1700073B RID: 1851
		// (get) Token: 0x060017CE RID: 6094 RVA: 0x000431E4 File Offset: 0x000413E4
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half4 wywx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half4(this.w, this.y, this.w, this.x);
			}
		}

		// Token: 0x1700073C RID: 1852
		// (get) Token: 0x060017CF RID: 6095 RVA: 0x00043203 File Offset: 0x00041403
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half4 wywy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half4(this.w, this.y, this.w, this.y);
			}
		}

		// Token: 0x1700073D RID: 1853
		// (get) Token: 0x060017D0 RID: 6096 RVA: 0x00043222 File Offset: 0x00041422
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half4 wywz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half4(this.w, this.y, this.w, this.z);
			}
		}

		// Token: 0x1700073E RID: 1854
		// (get) Token: 0x060017D1 RID: 6097 RVA: 0x00043241 File Offset: 0x00041441
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half4 wyww
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half4(this.w, this.y, this.w, this.w);
			}
		}

		// Token: 0x1700073F RID: 1855
		// (get) Token: 0x060017D2 RID: 6098 RVA: 0x00043260 File Offset: 0x00041460
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half4 wzxx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half4(this.w, this.z, this.x, this.x);
			}
		}

		// Token: 0x17000740 RID: 1856
		// (get) Token: 0x060017D3 RID: 6099 RVA: 0x0004327F File Offset: 0x0004147F
		// (set) Token: 0x060017D4 RID: 6100 RVA: 0x0004329E File Offset: 0x0004149E
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half4 wzxy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half4(this.w, this.z, this.x, this.y);
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

		// Token: 0x17000741 RID: 1857
		// (get) Token: 0x060017D5 RID: 6101 RVA: 0x000432D0 File Offset: 0x000414D0
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half4 wzxz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half4(this.w, this.z, this.x, this.z);
			}
		}

		// Token: 0x17000742 RID: 1858
		// (get) Token: 0x060017D6 RID: 6102 RVA: 0x000432EF File Offset: 0x000414EF
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half4 wzxw
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half4(this.w, this.z, this.x, this.w);
			}
		}

		// Token: 0x17000743 RID: 1859
		// (get) Token: 0x060017D7 RID: 6103 RVA: 0x0004330E File Offset: 0x0004150E
		// (set) Token: 0x060017D8 RID: 6104 RVA: 0x0004332D File Offset: 0x0004152D
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half4 wzyx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half4(this.w, this.z, this.y, this.x);
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

		// Token: 0x17000744 RID: 1860
		// (get) Token: 0x060017D9 RID: 6105 RVA: 0x0004335F File Offset: 0x0004155F
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half4 wzyy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half4(this.w, this.z, this.y, this.y);
			}
		}

		// Token: 0x17000745 RID: 1861
		// (get) Token: 0x060017DA RID: 6106 RVA: 0x0004337E File Offset: 0x0004157E
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half4 wzyz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half4(this.w, this.z, this.y, this.z);
			}
		}

		// Token: 0x17000746 RID: 1862
		// (get) Token: 0x060017DB RID: 6107 RVA: 0x0004339D File Offset: 0x0004159D
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half4 wzyw
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half4(this.w, this.z, this.y, this.w);
			}
		}

		// Token: 0x17000747 RID: 1863
		// (get) Token: 0x060017DC RID: 6108 RVA: 0x000433BC File Offset: 0x000415BC
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half4 wzzx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half4(this.w, this.z, this.z, this.x);
			}
		}

		// Token: 0x17000748 RID: 1864
		// (get) Token: 0x060017DD RID: 6109 RVA: 0x000433DB File Offset: 0x000415DB
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half4 wzzy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half4(this.w, this.z, this.z, this.y);
			}
		}

		// Token: 0x17000749 RID: 1865
		// (get) Token: 0x060017DE RID: 6110 RVA: 0x000433FA File Offset: 0x000415FA
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half4 wzzz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half4(this.w, this.z, this.z, this.z);
			}
		}

		// Token: 0x1700074A RID: 1866
		// (get) Token: 0x060017DF RID: 6111 RVA: 0x00043419 File Offset: 0x00041619
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half4 wzzw
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half4(this.w, this.z, this.z, this.w);
			}
		}

		// Token: 0x1700074B RID: 1867
		// (get) Token: 0x060017E0 RID: 6112 RVA: 0x00043438 File Offset: 0x00041638
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half4 wzwx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half4(this.w, this.z, this.w, this.x);
			}
		}

		// Token: 0x1700074C RID: 1868
		// (get) Token: 0x060017E1 RID: 6113 RVA: 0x00043457 File Offset: 0x00041657
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half4 wzwy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half4(this.w, this.z, this.w, this.y);
			}
		}

		// Token: 0x1700074D RID: 1869
		// (get) Token: 0x060017E2 RID: 6114 RVA: 0x00043476 File Offset: 0x00041676
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half4 wzwz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half4(this.w, this.z, this.w, this.z);
			}
		}

		// Token: 0x1700074E RID: 1870
		// (get) Token: 0x060017E3 RID: 6115 RVA: 0x00043495 File Offset: 0x00041695
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half4 wzww
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half4(this.w, this.z, this.w, this.w);
			}
		}

		// Token: 0x1700074F RID: 1871
		// (get) Token: 0x060017E4 RID: 6116 RVA: 0x000434B4 File Offset: 0x000416B4
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half4 wwxx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half4(this.w, this.w, this.x, this.x);
			}
		}

		// Token: 0x17000750 RID: 1872
		// (get) Token: 0x060017E5 RID: 6117 RVA: 0x000434D3 File Offset: 0x000416D3
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half4 wwxy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half4(this.w, this.w, this.x, this.y);
			}
		}

		// Token: 0x17000751 RID: 1873
		// (get) Token: 0x060017E6 RID: 6118 RVA: 0x000434F2 File Offset: 0x000416F2
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half4 wwxz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half4(this.w, this.w, this.x, this.z);
			}
		}

		// Token: 0x17000752 RID: 1874
		// (get) Token: 0x060017E7 RID: 6119 RVA: 0x00043511 File Offset: 0x00041711
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half4 wwxw
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half4(this.w, this.w, this.x, this.w);
			}
		}

		// Token: 0x17000753 RID: 1875
		// (get) Token: 0x060017E8 RID: 6120 RVA: 0x00043530 File Offset: 0x00041730
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half4 wwyx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half4(this.w, this.w, this.y, this.x);
			}
		}

		// Token: 0x17000754 RID: 1876
		// (get) Token: 0x060017E9 RID: 6121 RVA: 0x0004354F File Offset: 0x0004174F
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half4 wwyy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half4(this.w, this.w, this.y, this.y);
			}
		}

		// Token: 0x17000755 RID: 1877
		// (get) Token: 0x060017EA RID: 6122 RVA: 0x0004356E File Offset: 0x0004176E
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half4 wwyz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half4(this.w, this.w, this.y, this.z);
			}
		}

		// Token: 0x17000756 RID: 1878
		// (get) Token: 0x060017EB RID: 6123 RVA: 0x0004358D File Offset: 0x0004178D
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half4 wwyw
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half4(this.w, this.w, this.y, this.w);
			}
		}

		// Token: 0x17000757 RID: 1879
		// (get) Token: 0x060017EC RID: 6124 RVA: 0x000435AC File Offset: 0x000417AC
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half4 wwzx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half4(this.w, this.w, this.z, this.x);
			}
		}

		// Token: 0x17000758 RID: 1880
		// (get) Token: 0x060017ED RID: 6125 RVA: 0x000435CB File Offset: 0x000417CB
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half4 wwzy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half4(this.w, this.w, this.z, this.y);
			}
		}

		// Token: 0x17000759 RID: 1881
		// (get) Token: 0x060017EE RID: 6126 RVA: 0x000435EA File Offset: 0x000417EA
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half4 wwzz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half4(this.w, this.w, this.z, this.z);
			}
		}

		// Token: 0x1700075A RID: 1882
		// (get) Token: 0x060017EF RID: 6127 RVA: 0x00043609 File Offset: 0x00041809
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half4 wwzw
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half4(this.w, this.w, this.z, this.w);
			}
		}

		// Token: 0x1700075B RID: 1883
		// (get) Token: 0x060017F0 RID: 6128 RVA: 0x00043628 File Offset: 0x00041828
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half4 wwwx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half4(this.w, this.w, this.w, this.x);
			}
		}

		// Token: 0x1700075C RID: 1884
		// (get) Token: 0x060017F1 RID: 6129 RVA: 0x00043647 File Offset: 0x00041847
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half4 wwwy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half4(this.w, this.w, this.w, this.y);
			}
		}

		// Token: 0x1700075D RID: 1885
		// (get) Token: 0x060017F2 RID: 6130 RVA: 0x00043666 File Offset: 0x00041866
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half4 wwwz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half4(this.w, this.w, this.w, this.z);
			}
		}

		// Token: 0x1700075E RID: 1886
		// (get) Token: 0x060017F3 RID: 6131 RVA: 0x00043685 File Offset: 0x00041885
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half4 wwww
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half4(this.w, this.w, this.w, this.w);
			}
		}

		// Token: 0x1700075F RID: 1887
		// (get) Token: 0x060017F4 RID: 6132 RVA: 0x000436A4 File Offset: 0x000418A4
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half3 xxx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half3(this.x, this.x, this.x);
			}
		}

		// Token: 0x17000760 RID: 1888
		// (get) Token: 0x060017F5 RID: 6133 RVA: 0x000436BD File Offset: 0x000418BD
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half3 xxy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half3(this.x, this.x, this.y);
			}
		}

		// Token: 0x17000761 RID: 1889
		// (get) Token: 0x060017F6 RID: 6134 RVA: 0x000436D6 File Offset: 0x000418D6
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half3 xxz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half3(this.x, this.x, this.z);
			}
		}

		// Token: 0x17000762 RID: 1890
		// (get) Token: 0x060017F7 RID: 6135 RVA: 0x000436EF File Offset: 0x000418EF
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half3 xxw
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half3(this.x, this.x, this.w);
			}
		}

		// Token: 0x17000763 RID: 1891
		// (get) Token: 0x060017F8 RID: 6136 RVA: 0x00043708 File Offset: 0x00041908
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half3 xyx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half3(this.x, this.y, this.x);
			}
		}

		// Token: 0x17000764 RID: 1892
		// (get) Token: 0x060017F9 RID: 6137 RVA: 0x00043721 File Offset: 0x00041921
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half3 xyy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half3(this.x, this.y, this.y);
			}
		}

		// Token: 0x17000765 RID: 1893
		// (get) Token: 0x060017FA RID: 6138 RVA: 0x0004373A File Offset: 0x0004193A
		// (set) Token: 0x060017FB RID: 6139 RVA: 0x00043753 File Offset: 0x00041953
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half3 xyz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half3(this.x, this.y, this.z);
			}
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			set
			{
				this.x = value.x;
				this.y = value.y;
				this.z = value.z;
			}
		}

		// Token: 0x17000766 RID: 1894
		// (get) Token: 0x060017FC RID: 6140 RVA: 0x00043779 File Offset: 0x00041979
		// (set) Token: 0x060017FD RID: 6141 RVA: 0x00043792 File Offset: 0x00041992
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half3 xyw
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half3(this.x, this.y, this.w);
			}
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			set
			{
				this.x = value.x;
				this.y = value.y;
				this.w = value.z;
			}
		}

		// Token: 0x17000767 RID: 1895
		// (get) Token: 0x060017FE RID: 6142 RVA: 0x000437B8 File Offset: 0x000419B8
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half3 xzx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half3(this.x, this.z, this.x);
			}
		}

		// Token: 0x17000768 RID: 1896
		// (get) Token: 0x060017FF RID: 6143 RVA: 0x000437D1 File Offset: 0x000419D1
		// (set) Token: 0x06001800 RID: 6144 RVA: 0x000437EA File Offset: 0x000419EA
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half3 xzy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half3(this.x, this.z, this.y);
			}
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			set
			{
				this.x = value.x;
				this.z = value.y;
				this.y = value.z;
			}
		}

		// Token: 0x17000769 RID: 1897
		// (get) Token: 0x06001801 RID: 6145 RVA: 0x00043810 File Offset: 0x00041A10
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half3 xzz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half3(this.x, this.z, this.z);
			}
		}

		// Token: 0x1700076A RID: 1898
		// (get) Token: 0x06001802 RID: 6146 RVA: 0x00043829 File Offset: 0x00041A29
		// (set) Token: 0x06001803 RID: 6147 RVA: 0x00043842 File Offset: 0x00041A42
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half3 xzw
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half3(this.x, this.z, this.w);
			}
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			set
			{
				this.x = value.x;
				this.z = value.y;
				this.w = value.z;
			}
		}

		// Token: 0x1700076B RID: 1899
		// (get) Token: 0x06001804 RID: 6148 RVA: 0x00043868 File Offset: 0x00041A68
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half3 xwx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half3(this.x, this.w, this.x);
			}
		}

		// Token: 0x1700076C RID: 1900
		// (get) Token: 0x06001805 RID: 6149 RVA: 0x00043881 File Offset: 0x00041A81
		// (set) Token: 0x06001806 RID: 6150 RVA: 0x0004389A File Offset: 0x00041A9A
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half3 xwy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half3(this.x, this.w, this.y);
			}
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			set
			{
				this.x = value.x;
				this.w = value.y;
				this.y = value.z;
			}
		}

		// Token: 0x1700076D RID: 1901
		// (get) Token: 0x06001807 RID: 6151 RVA: 0x000438C0 File Offset: 0x00041AC0
		// (set) Token: 0x06001808 RID: 6152 RVA: 0x000438D9 File Offset: 0x00041AD9
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half3 xwz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half3(this.x, this.w, this.z);
			}
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			set
			{
				this.x = value.x;
				this.w = value.y;
				this.z = value.z;
			}
		}

		// Token: 0x1700076E RID: 1902
		// (get) Token: 0x06001809 RID: 6153 RVA: 0x000438FF File Offset: 0x00041AFF
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half3 xww
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half3(this.x, this.w, this.w);
			}
		}

		// Token: 0x1700076F RID: 1903
		// (get) Token: 0x0600180A RID: 6154 RVA: 0x00043918 File Offset: 0x00041B18
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half3 yxx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half3(this.y, this.x, this.x);
			}
		}

		// Token: 0x17000770 RID: 1904
		// (get) Token: 0x0600180B RID: 6155 RVA: 0x00043931 File Offset: 0x00041B31
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half3 yxy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half3(this.y, this.x, this.y);
			}
		}

		// Token: 0x17000771 RID: 1905
		// (get) Token: 0x0600180C RID: 6156 RVA: 0x0004394A File Offset: 0x00041B4A
		// (set) Token: 0x0600180D RID: 6157 RVA: 0x00043963 File Offset: 0x00041B63
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half3 yxz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half3(this.y, this.x, this.z);
			}
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			set
			{
				this.y = value.x;
				this.x = value.y;
				this.z = value.z;
			}
		}

		// Token: 0x17000772 RID: 1906
		// (get) Token: 0x0600180E RID: 6158 RVA: 0x00043989 File Offset: 0x00041B89
		// (set) Token: 0x0600180F RID: 6159 RVA: 0x000439A2 File Offset: 0x00041BA2
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half3 yxw
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half3(this.y, this.x, this.w);
			}
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			set
			{
				this.y = value.x;
				this.x = value.y;
				this.w = value.z;
			}
		}

		// Token: 0x17000773 RID: 1907
		// (get) Token: 0x06001810 RID: 6160 RVA: 0x000439C8 File Offset: 0x00041BC8
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half3 yyx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half3(this.y, this.y, this.x);
			}
		}

		// Token: 0x17000774 RID: 1908
		// (get) Token: 0x06001811 RID: 6161 RVA: 0x000439E1 File Offset: 0x00041BE1
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half3 yyy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half3(this.y, this.y, this.y);
			}
		}

		// Token: 0x17000775 RID: 1909
		// (get) Token: 0x06001812 RID: 6162 RVA: 0x000439FA File Offset: 0x00041BFA
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half3 yyz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half3(this.y, this.y, this.z);
			}
		}

		// Token: 0x17000776 RID: 1910
		// (get) Token: 0x06001813 RID: 6163 RVA: 0x00043A13 File Offset: 0x00041C13
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half3 yyw
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half3(this.y, this.y, this.w);
			}
		}

		// Token: 0x17000777 RID: 1911
		// (get) Token: 0x06001814 RID: 6164 RVA: 0x00043A2C File Offset: 0x00041C2C
		// (set) Token: 0x06001815 RID: 6165 RVA: 0x00043A45 File Offset: 0x00041C45
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half3 yzx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half3(this.y, this.z, this.x);
			}
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			set
			{
				this.y = value.x;
				this.z = value.y;
				this.x = value.z;
			}
		}

		// Token: 0x17000778 RID: 1912
		// (get) Token: 0x06001816 RID: 6166 RVA: 0x00043A6B File Offset: 0x00041C6B
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half3 yzy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half3(this.y, this.z, this.y);
			}
		}

		// Token: 0x17000779 RID: 1913
		// (get) Token: 0x06001817 RID: 6167 RVA: 0x00043A84 File Offset: 0x00041C84
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half3 yzz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half3(this.y, this.z, this.z);
			}
		}

		// Token: 0x1700077A RID: 1914
		// (get) Token: 0x06001818 RID: 6168 RVA: 0x00043A9D File Offset: 0x00041C9D
		// (set) Token: 0x06001819 RID: 6169 RVA: 0x00043AB6 File Offset: 0x00041CB6
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half3 yzw
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half3(this.y, this.z, this.w);
			}
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			set
			{
				this.y = value.x;
				this.z = value.y;
				this.w = value.z;
			}
		}

		// Token: 0x1700077B RID: 1915
		// (get) Token: 0x0600181A RID: 6170 RVA: 0x00043ADC File Offset: 0x00041CDC
		// (set) Token: 0x0600181B RID: 6171 RVA: 0x00043AF5 File Offset: 0x00041CF5
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half3 ywx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half3(this.y, this.w, this.x);
			}
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			set
			{
				this.y = value.x;
				this.w = value.y;
				this.x = value.z;
			}
		}

		// Token: 0x1700077C RID: 1916
		// (get) Token: 0x0600181C RID: 6172 RVA: 0x00043B1B File Offset: 0x00041D1B
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half3 ywy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half3(this.y, this.w, this.y);
			}
		}

		// Token: 0x1700077D RID: 1917
		// (get) Token: 0x0600181D RID: 6173 RVA: 0x00043B34 File Offset: 0x00041D34
		// (set) Token: 0x0600181E RID: 6174 RVA: 0x00043B4D File Offset: 0x00041D4D
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half3 ywz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half3(this.y, this.w, this.z);
			}
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			set
			{
				this.y = value.x;
				this.w = value.y;
				this.z = value.z;
			}
		}

		// Token: 0x1700077E RID: 1918
		// (get) Token: 0x0600181F RID: 6175 RVA: 0x00043B73 File Offset: 0x00041D73
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half3 yww
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half3(this.y, this.w, this.w);
			}
		}

		// Token: 0x1700077F RID: 1919
		// (get) Token: 0x06001820 RID: 6176 RVA: 0x00043B8C File Offset: 0x00041D8C
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half3 zxx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half3(this.z, this.x, this.x);
			}
		}

		// Token: 0x17000780 RID: 1920
		// (get) Token: 0x06001821 RID: 6177 RVA: 0x00043BA5 File Offset: 0x00041DA5
		// (set) Token: 0x06001822 RID: 6178 RVA: 0x00043BBE File Offset: 0x00041DBE
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half3 zxy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half3(this.z, this.x, this.y);
			}
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			set
			{
				this.z = value.x;
				this.x = value.y;
				this.y = value.z;
			}
		}

		// Token: 0x17000781 RID: 1921
		// (get) Token: 0x06001823 RID: 6179 RVA: 0x00043BE4 File Offset: 0x00041DE4
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half3 zxz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half3(this.z, this.x, this.z);
			}
		}

		// Token: 0x17000782 RID: 1922
		// (get) Token: 0x06001824 RID: 6180 RVA: 0x00043BFD File Offset: 0x00041DFD
		// (set) Token: 0x06001825 RID: 6181 RVA: 0x00043C16 File Offset: 0x00041E16
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half3 zxw
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half3(this.z, this.x, this.w);
			}
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			set
			{
				this.z = value.x;
				this.x = value.y;
				this.w = value.z;
			}
		}

		// Token: 0x17000783 RID: 1923
		// (get) Token: 0x06001826 RID: 6182 RVA: 0x00043C3C File Offset: 0x00041E3C
		// (set) Token: 0x06001827 RID: 6183 RVA: 0x00043C55 File Offset: 0x00041E55
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half3 zyx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half3(this.z, this.y, this.x);
			}
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			set
			{
				this.z = value.x;
				this.y = value.y;
				this.x = value.z;
			}
		}

		// Token: 0x17000784 RID: 1924
		// (get) Token: 0x06001828 RID: 6184 RVA: 0x00043C7B File Offset: 0x00041E7B
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half3 zyy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half3(this.z, this.y, this.y);
			}
		}

		// Token: 0x17000785 RID: 1925
		// (get) Token: 0x06001829 RID: 6185 RVA: 0x00043C94 File Offset: 0x00041E94
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half3 zyz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half3(this.z, this.y, this.z);
			}
		}

		// Token: 0x17000786 RID: 1926
		// (get) Token: 0x0600182A RID: 6186 RVA: 0x00043CAD File Offset: 0x00041EAD
		// (set) Token: 0x0600182B RID: 6187 RVA: 0x00043CC6 File Offset: 0x00041EC6
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half3 zyw
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half3(this.z, this.y, this.w);
			}
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			set
			{
				this.z = value.x;
				this.y = value.y;
				this.w = value.z;
			}
		}

		// Token: 0x17000787 RID: 1927
		// (get) Token: 0x0600182C RID: 6188 RVA: 0x00043CEC File Offset: 0x00041EEC
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half3 zzx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half3(this.z, this.z, this.x);
			}
		}

		// Token: 0x17000788 RID: 1928
		// (get) Token: 0x0600182D RID: 6189 RVA: 0x00043D05 File Offset: 0x00041F05
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half3 zzy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half3(this.z, this.z, this.y);
			}
		}

		// Token: 0x17000789 RID: 1929
		// (get) Token: 0x0600182E RID: 6190 RVA: 0x00043D1E File Offset: 0x00041F1E
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half3 zzz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half3(this.z, this.z, this.z);
			}
		}

		// Token: 0x1700078A RID: 1930
		// (get) Token: 0x0600182F RID: 6191 RVA: 0x00043D37 File Offset: 0x00041F37
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half3 zzw
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half3(this.z, this.z, this.w);
			}
		}

		// Token: 0x1700078B RID: 1931
		// (get) Token: 0x06001830 RID: 6192 RVA: 0x00043D50 File Offset: 0x00041F50
		// (set) Token: 0x06001831 RID: 6193 RVA: 0x00043D69 File Offset: 0x00041F69
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half3 zwx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half3(this.z, this.w, this.x);
			}
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			set
			{
				this.z = value.x;
				this.w = value.y;
				this.x = value.z;
			}
		}

		// Token: 0x1700078C RID: 1932
		// (get) Token: 0x06001832 RID: 6194 RVA: 0x00043D8F File Offset: 0x00041F8F
		// (set) Token: 0x06001833 RID: 6195 RVA: 0x00043DA8 File Offset: 0x00041FA8
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half3 zwy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half3(this.z, this.w, this.y);
			}
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			set
			{
				this.z = value.x;
				this.w = value.y;
				this.y = value.z;
			}
		}

		// Token: 0x1700078D RID: 1933
		// (get) Token: 0x06001834 RID: 6196 RVA: 0x00043DCE File Offset: 0x00041FCE
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half3 zwz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half3(this.z, this.w, this.z);
			}
		}

		// Token: 0x1700078E RID: 1934
		// (get) Token: 0x06001835 RID: 6197 RVA: 0x00043DE7 File Offset: 0x00041FE7
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half3 zww
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half3(this.z, this.w, this.w);
			}
		}

		// Token: 0x1700078F RID: 1935
		// (get) Token: 0x06001836 RID: 6198 RVA: 0x00043E00 File Offset: 0x00042000
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half3 wxx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half3(this.w, this.x, this.x);
			}
		}

		// Token: 0x17000790 RID: 1936
		// (get) Token: 0x06001837 RID: 6199 RVA: 0x00043E19 File Offset: 0x00042019
		// (set) Token: 0x06001838 RID: 6200 RVA: 0x00043E32 File Offset: 0x00042032
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half3 wxy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half3(this.w, this.x, this.y);
			}
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			set
			{
				this.w = value.x;
				this.x = value.y;
				this.y = value.z;
			}
		}

		// Token: 0x17000791 RID: 1937
		// (get) Token: 0x06001839 RID: 6201 RVA: 0x00043E58 File Offset: 0x00042058
		// (set) Token: 0x0600183A RID: 6202 RVA: 0x00043E71 File Offset: 0x00042071
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half3 wxz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half3(this.w, this.x, this.z);
			}
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			set
			{
				this.w = value.x;
				this.x = value.y;
				this.z = value.z;
			}
		}

		// Token: 0x17000792 RID: 1938
		// (get) Token: 0x0600183B RID: 6203 RVA: 0x00043E97 File Offset: 0x00042097
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half3 wxw
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half3(this.w, this.x, this.w);
			}
		}

		// Token: 0x17000793 RID: 1939
		// (get) Token: 0x0600183C RID: 6204 RVA: 0x00043EB0 File Offset: 0x000420B0
		// (set) Token: 0x0600183D RID: 6205 RVA: 0x00043EC9 File Offset: 0x000420C9
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half3 wyx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half3(this.w, this.y, this.x);
			}
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			set
			{
				this.w = value.x;
				this.y = value.y;
				this.x = value.z;
			}
		}

		// Token: 0x17000794 RID: 1940
		// (get) Token: 0x0600183E RID: 6206 RVA: 0x00043EEF File Offset: 0x000420EF
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half3 wyy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half3(this.w, this.y, this.y);
			}
		}

		// Token: 0x17000795 RID: 1941
		// (get) Token: 0x0600183F RID: 6207 RVA: 0x00043F08 File Offset: 0x00042108
		// (set) Token: 0x06001840 RID: 6208 RVA: 0x00043F21 File Offset: 0x00042121
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half3 wyz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half3(this.w, this.y, this.z);
			}
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			set
			{
				this.w = value.x;
				this.y = value.y;
				this.z = value.z;
			}
		}

		// Token: 0x17000796 RID: 1942
		// (get) Token: 0x06001841 RID: 6209 RVA: 0x00043F47 File Offset: 0x00042147
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half3 wyw
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half3(this.w, this.y, this.w);
			}
		}

		// Token: 0x17000797 RID: 1943
		// (get) Token: 0x06001842 RID: 6210 RVA: 0x00043F60 File Offset: 0x00042160
		// (set) Token: 0x06001843 RID: 6211 RVA: 0x00043F79 File Offset: 0x00042179
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half3 wzx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half3(this.w, this.z, this.x);
			}
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			set
			{
				this.w = value.x;
				this.z = value.y;
				this.x = value.z;
			}
		}

		// Token: 0x17000798 RID: 1944
		// (get) Token: 0x06001844 RID: 6212 RVA: 0x00043F9F File Offset: 0x0004219F
		// (set) Token: 0x06001845 RID: 6213 RVA: 0x00043FB8 File Offset: 0x000421B8
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half3 wzy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half3(this.w, this.z, this.y);
			}
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			set
			{
				this.w = value.x;
				this.z = value.y;
				this.y = value.z;
			}
		}

		// Token: 0x17000799 RID: 1945
		// (get) Token: 0x06001846 RID: 6214 RVA: 0x00043FDE File Offset: 0x000421DE
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half3 wzz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half3(this.w, this.z, this.z);
			}
		}

		// Token: 0x1700079A RID: 1946
		// (get) Token: 0x06001847 RID: 6215 RVA: 0x00043FF7 File Offset: 0x000421F7
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half3 wzw
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half3(this.w, this.z, this.w);
			}
		}

		// Token: 0x1700079B RID: 1947
		// (get) Token: 0x06001848 RID: 6216 RVA: 0x00044010 File Offset: 0x00042210
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half3 wwx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half3(this.w, this.w, this.x);
			}
		}

		// Token: 0x1700079C RID: 1948
		// (get) Token: 0x06001849 RID: 6217 RVA: 0x00044029 File Offset: 0x00042229
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half3 wwy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half3(this.w, this.w, this.y);
			}
		}

		// Token: 0x1700079D RID: 1949
		// (get) Token: 0x0600184A RID: 6218 RVA: 0x00044042 File Offset: 0x00042242
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half3 wwz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half3(this.w, this.w, this.z);
			}
		}

		// Token: 0x1700079E RID: 1950
		// (get) Token: 0x0600184B RID: 6219 RVA: 0x0004405B File Offset: 0x0004225B
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half3 www
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half3(this.w, this.w, this.w);
			}
		}

		// Token: 0x1700079F RID: 1951
		// (get) Token: 0x0600184C RID: 6220 RVA: 0x00044074 File Offset: 0x00042274
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half2 xx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half2(this.x, this.x);
			}
		}

		// Token: 0x170007A0 RID: 1952
		// (get) Token: 0x0600184D RID: 6221 RVA: 0x00044087 File Offset: 0x00042287
		// (set) Token: 0x0600184E RID: 6222 RVA: 0x0004409A File Offset: 0x0004229A
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half2 xy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half2(this.x, this.y);
			}
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			set
			{
				this.x = value.x;
				this.y = value.y;
			}
		}

		// Token: 0x170007A1 RID: 1953
		// (get) Token: 0x0600184F RID: 6223 RVA: 0x000440B4 File Offset: 0x000422B4
		// (set) Token: 0x06001850 RID: 6224 RVA: 0x000440C7 File Offset: 0x000422C7
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half2 xz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half2(this.x, this.z);
			}
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			set
			{
				this.x = value.x;
				this.z = value.y;
			}
		}

		// Token: 0x170007A2 RID: 1954
		// (get) Token: 0x06001851 RID: 6225 RVA: 0x000440E1 File Offset: 0x000422E1
		// (set) Token: 0x06001852 RID: 6226 RVA: 0x000440F4 File Offset: 0x000422F4
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half2 xw
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half2(this.x, this.w);
			}
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			set
			{
				this.x = value.x;
				this.w = value.y;
			}
		}

		// Token: 0x170007A3 RID: 1955
		// (get) Token: 0x06001853 RID: 6227 RVA: 0x0004410E File Offset: 0x0004230E
		// (set) Token: 0x06001854 RID: 6228 RVA: 0x00044121 File Offset: 0x00042321
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half2 yx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half2(this.y, this.x);
			}
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			set
			{
				this.y = value.x;
				this.x = value.y;
			}
		}

		// Token: 0x170007A4 RID: 1956
		// (get) Token: 0x06001855 RID: 6229 RVA: 0x0004413B File Offset: 0x0004233B
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half2 yy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half2(this.y, this.y);
			}
		}

		// Token: 0x170007A5 RID: 1957
		// (get) Token: 0x06001856 RID: 6230 RVA: 0x0004414E File Offset: 0x0004234E
		// (set) Token: 0x06001857 RID: 6231 RVA: 0x00044161 File Offset: 0x00042361
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half2 yz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half2(this.y, this.z);
			}
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			set
			{
				this.y = value.x;
				this.z = value.y;
			}
		}

		// Token: 0x170007A6 RID: 1958
		// (get) Token: 0x06001858 RID: 6232 RVA: 0x0004417B File Offset: 0x0004237B
		// (set) Token: 0x06001859 RID: 6233 RVA: 0x0004418E File Offset: 0x0004238E
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half2 yw
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half2(this.y, this.w);
			}
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			set
			{
				this.y = value.x;
				this.w = value.y;
			}
		}

		// Token: 0x170007A7 RID: 1959
		// (get) Token: 0x0600185A RID: 6234 RVA: 0x000441A8 File Offset: 0x000423A8
		// (set) Token: 0x0600185B RID: 6235 RVA: 0x000441BB File Offset: 0x000423BB
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half2 zx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half2(this.z, this.x);
			}
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			set
			{
				this.z = value.x;
				this.x = value.y;
			}
		}

		// Token: 0x170007A8 RID: 1960
		// (get) Token: 0x0600185C RID: 6236 RVA: 0x000441D5 File Offset: 0x000423D5
		// (set) Token: 0x0600185D RID: 6237 RVA: 0x000441E8 File Offset: 0x000423E8
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half2 zy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half2(this.z, this.y);
			}
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			set
			{
				this.z = value.x;
				this.y = value.y;
			}
		}

		// Token: 0x170007A9 RID: 1961
		// (get) Token: 0x0600185E RID: 6238 RVA: 0x00044202 File Offset: 0x00042402
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half2 zz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half2(this.z, this.z);
			}
		}

		// Token: 0x170007AA RID: 1962
		// (get) Token: 0x0600185F RID: 6239 RVA: 0x00044215 File Offset: 0x00042415
		// (set) Token: 0x06001860 RID: 6240 RVA: 0x00044228 File Offset: 0x00042428
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half2 zw
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half2(this.z, this.w);
			}
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			set
			{
				this.z = value.x;
				this.w = value.y;
			}
		}

		// Token: 0x170007AB RID: 1963
		// (get) Token: 0x06001861 RID: 6241 RVA: 0x00044242 File Offset: 0x00042442
		// (set) Token: 0x06001862 RID: 6242 RVA: 0x00044255 File Offset: 0x00042455
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half2 wx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half2(this.w, this.x);
			}
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			set
			{
				this.w = value.x;
				this.x = value.y;
			}
		}

		// Token: 0x170007AC RID: 1964
		// (get) Token: 0x06001863 RID: 6243 RVA: 0x0004426F File Offset: 0x0004246F
		// (set) Token: 0x06001864 RID: 6244 RVA: 0x00044282 File Offset: 0x00042482
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half2 wy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half2(this.w, this.y);
			}
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			set
			{
				this.w = value.x;
				this.y = value.y;
			}
		}

		// Token: 0x170007AD RID: 1965
		// (get) Token: 0x06001865 RID: 6245 RVA: 0x0004429C File Offset: 0x0004249C
		// (set) Token: 0x06001866 RID: 6246 RVA: 0x000442AF File Offset: 0x000424AF
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half2 wz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half2(this.w, this.z);
			}
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			set
			{
				this.w = value.x;
				this.z = value.y;
			}
		}

		// Token: 0x170007AE RID: 1966
		// (get) Token: 0x06001867 RID: 6247 RVA: 0x000442C9 File Offset: 0x000424C9
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half2 ww
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half2(this.w, this.w);
			}
		}

		// Token: 0x170007AF RID: 1967
		public unsafe half this[int index]
		{
			get
			{
				fixed (half4* ptr = &this)
				{
					return ((half*)ptr)[index];
				}
			}
			set
			{
				fixed (half* ptr = &this.x)
				{
					ptr[index] = value;
				}
			}
		}

		// Token: 0x0600186A RID: 6250 RVA: 0x00044328 File Offset: 0x00042528
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool Equals(half4 rhs)
		{
			return this.x == rhs.x && this.y == rhs.y && this.z == rhs.z && this.w == rhs.w;
		}

		// Token: 0x0600186B RID: 6251 RVA: 0x00044384 File Offset: 0x00042584
		public override bool Equals(object o)
		{
			if (o is half4)
			{
				half4 half = (half4)o;
				return this.Equals(half);
			}
			return false;
		}

		// Token: 0x0600186C RID: 6252 RVA: 0x000443A9 File Offset: 0x000425A9
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public override int GetHashCode()
		{
			return (int)math.hash(this);
		}

		// Token: 0x0600186D RID: 6253 RVA: 0x000443B8 File Offset: 0x000425B8
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public override string ToString()
		{
			return string.Format("half4({0}, {1}, {2}, {3})", new object[] { this.x, this.y, this.z, this.w });
		}

		// Token: 0x0600186E RID: 6254 RVA: 0x00044410 File Offset: 0x00042610
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public string ToString(string format, IFormatProvider formatProvider)
		{
			return string.Format("half4({0}, {1}, {2}, {3})", new object[]
			{
				this.x.ToString(format, formatProvider),
				this.y.ToString(format, formatProvider),
				this.z.ToString(format, formatProvider),
				this.w.ToString(format, formatProvider)
			});
		}

		// Token: 0x040000B8 RID: 184
		public half x;

		// Token: 0x040000B9 RID: 185
		public half y;

		// Token: 0x040000BA RID: 186
		public half z;

		// Token: 0x040000BB RID: 187
		public half w;

		// Token: 0x040000BC RID: 188
		public static readonly half4 zero;

		// Token: 0x02000060 RID: 96
		internal sealed class DebuggerProxy
		{
			// Token: 0x060024CD RID: 9421 RVA: 0x000692D4 File Offset: 0x000674D4
			public DebuggerProxy(half4 v)
			{
				this.x = v.x;
				this.y = v.y;
				this.z = v.z;
				this.w = v.w;
			}

			// Token: 0x0400016A RID: 362
			public half x;

			// Token: 0x0400016B RID: 363
			public half y;

			// Token: 0x0400016C RID: 364
			public half z;

			// Token: 0x0400016D RID: 365
			public half w;
		}
	}
}
