using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Unity.IL2CPP.CompilerServices;

namespace Unity.Mathematics
{
	// Token: 0x02000012 RID: 18
	[DebuggerTypeProxy(typeof(double2.DebuggerProxy))]
	[Il2CppEagerStaticClassConstruction]
	[Serializable]
	public struct double2 : IEquatable<double2>, IFormattable
	{
		// Token: 0x06000B27 RID: 2855 RVA: 0x000236B9 File Offset: 0x000218B9
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public double2(double x, double y)
		{
			this.x = x;
			this.y = y;
		}

		// Token: 0x06000B28 RID: 2856 RVA: 0x000236C9 File Offset: 0x000218C9
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public double2(double2 xy)
		{
			this.x = xy.x;
			this.y = xy.y;
		}

		// Token: 0x06000B29 RID: 2857 RVA: 0x000236E3 File Offset: 0x000218E3
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public double2(double v)
		{
			this.x = v;
			this.y = v;
		}

		// Token: 0x06000B2A RID: 2858 RVA: 0x000236F3 File Offset: 0x000218F3
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public double2(bool v)
		{
			this.x = (v ? 1.0 : 0.0);
			this.y = (v ? 1.0 : 0.0);
		}

		// Token: 0x06000B2B RID: 2859 RVA: 0x00023730 File Offset: 0x00021930
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public double2(bool2 v)
		{
			this.x = (v.x ? 1.0 : 0.0);
			this.y = (v.y ? 1.0 : 0.0);
		}

		// Token: 0x06000B2C RID: 2860 RVA: 0x00023781 File Offset: 0x00021981
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public double2(int v)
		{
			this.x = (double)v;
			this.y = (double)v;
		}

		// Token: 0x06000B2D RID: 2861 RVA: 0x00023793 File Offset: 0x00021993
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public double2(int2 v)
		{
			this.x = (double)v.x;
			this.y = (double)v.y;
		}

		// Token: 0x06000B2E RID: 2862 RVA: 0x000237AF File Offset: 0x000219AF
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public double2(uint v)
		{
			this.x = v;
			this.y = v;
		}

		// Token: 0x06000B2F RID: 2863 RVA: 0x000237C3 File Offset: 0x000219C3
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public double2(uint2 v)
		{
			this.x = v.x;
			this.y = v.y;
		}

		// Token: 0x06000B30 RID: 2864 RVA: 0x000237E1 File Offset: 0x000219E1
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public double2(half v)
		{
			this.x = v;
			this.y = v;
		}

		// Token: 0x06000B31 RID: 2865 RVA: 0x000237FB File Offset: 0x000219FB
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public double2(half2 v)
		{
			this.x = v.x;
			this.y = v.y;
		}

		// Token: 0x06000B32 RID: 2866 RVA: 0x0002381F File Offset: 0x00021A1F
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public double2(float v)
		{
			this.x = (double)v;
			this.y = (double)v;
		}

		// Token: 0x06000B33 RID: 2867 RVA: 0x00023831 File Offset: 0x00021A31
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public double2(float2 v)
		{
			this.x = (double)v.x;
			this.y = (double)v.y;
		}

		// Token: 0x06000B34 RID: 2868 RVA: 0x0002384D File Offset: 0x00021A4D
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static implicit operator double2(double v)
		{
			return new double2(v);
		}

		// Token: 0x06000B35 RID: 2869 RVA: 0x00023855 File Offset: 0x00021A55
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static explicit operator double2(bool v)
		{
			return new double2(v);
		}

		// Token: 0x06000B36 RID: 2870 RVA: 0x0002385D File Offset: 0x00021A5D
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static explicit operator double2(bool2 v)
		{
			return new double2(v);
		}

		// Token: 0x06000B37 RID: 2871 RVA: 0x00023865 File Offset: 0x00021A65
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static implicit operator double2(int v)
		{
			return new double2(v);
		}

		// Token: 0x06000B38 RID: 2872 RVA: 0x0002386D File Offset: 0x00021A6D
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static implicit operator double2(int2 v)
		{
			return new double2(v);
		}

		// Token: 0x06000B39 RID: 2873 RVA: 0x00023875 File Offset: 0x00021A75
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static implicit operator double2(uint v)
		{
			return new double2(v);
		}

		// Token: 0x06000B3A RID: 2874 RVA: 0x0002387D File Offset: 0x00021A7D
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static implicit operator double2(uint2 v)
		{
			return new double2(v);
		}

		// Token: 0x06000B3B RID: 2875 RVA: 0x00023885 File Offset: 0x00021A85
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static implicit operator double2(half v)
		{
			return new double2(v);
		}

		// Token: 0x06000B3C RID: 2876 RVA: 0x0002388D File Offset: 0x00021A8D
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static implicit operator double2(half2 v)
		{
			return new double2(v);
		}

		// Token: 0x06000B3D RID: 2877 RVA: 0x00023895 File Offset: 0x00021A95
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static implicit operator double2(float v)
		{
			return new double2(v);
		}

		// Token: 0x06000B3E RID: 2878 RVA: 0x0002389D File Offset: 0x00021A9D
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static implicit operator double2(float2 v)
		{
			return new double2(v);
		}

		// Token: 0x06000B3F RID: 2879 RVA: 0x000238A5 File Offset: 0x00021AA5
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double2 operator *(double2 lhs, double2 rhs)
		{
			return new double2(lhs.x * rhs.x, lhs.y * rhs.y);
		}

		// Token: 0x06000B40 RID: 2880 RVA: 0x000238C6 File Offset: 0x00021AC6
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double2 operator *(double2 lhs, double rhs)
		{
			return new double2(lhs.x * rhs, lhs.y * rhs);
		}

		// Token: 0x06000B41 RID: 2881 RVA: 0x000238DD File Offset: 0x00021ADD
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double2 operator *(double lhs, double2 rhs)
		{
			return new double2(lhs * rhs.x, lhs * rhs.y);
		}

		// Token: 0x06000B42 RID: 2882 RVA: 0x000238F4 File Offset: 0x00021AF4
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double2 operator +(double2 lhs, double2 rhs)
		{
			return new double2(lhs.x + rhs.x, lhs.y + rhs.y);
		}

		// Token: 0x06000B43 RID: 2883 RVA: 0x00023915 File Offset: 0x00021B15
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double2 operator +(double2 lhs, double rhs)
		{
			return new double2(lhs.x + rhs, lhs.y + rhs);
		}

		// Token: 0x06000B44 RID: 2884 RVA: 0x0002392C File Offset: 0x00021B2C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double2 operator +(double lhs, double2 rhs)
		{
			return new double2(lhs + rhs.x, lhs + rhs.y);
		}

		// Token: 0x06000B45 RID: 2885 RVA: 0x00023943 File Offset: 0x00021B43
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double2 operator -(double2 lhs, double2 rhs)
		{
			return new double2(lhs.x - rhs.x, lhs.y - rhs.y);
		}

		// Token: 0x06000B46 RID: 2886 RVA: 0x00023964 File Offset: 0x00021B64
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double2 operator -(double2 lhs, double rhs)
		{
			return new double2(lhs.x - rhs, lhs.y - rhs);
		}

		// Token: 0x06000B47 RID: 2887 RVA: 0x0002397B File Offset: 0x00021B7B
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double2 operator -(double lhs, double2 rhs)
		{
			return new double2(lhs - rhs.x, lhs - rhs.y);
		}

		// Token: 0x06000B48 RID: 2888 RVA: 0x00023992 File Offset: 0x00021B92
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double2 operator /(double2 lhs, double2 rhs)
		{
			return new double2(lhs.x / rhs.x, lhs.y / rhs.y);
		}

		// Token: 0x06000B49 RID: 2889 RVA: 0x000239B3 File Offset: 0x00021BB3
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double2 operator /(double2 lhs, double rhs)
		{
			return new double2(lhs.x / rhs, lhs.y / rhs);
		}

		// Token: 0x06000B4A RID: 2890 RVA: 0x000239CA File Offset: 0x00021BCA
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double2 operator /(double lhs, double2 rhs)
		{
			return new double2(lhs / rhs.x, lhs / rhs.y);
		}

		// Token: 0x06000B4B RID: 2891 RVA: 0x000239E1 File Offset: 0x00021BE1
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double2 operator %(double2 lhs, double2 rhs)
		{
			return new double2(lhs.x % rhs.x, lhs.y % rhs.y);
		}

		// Token: 0x06000B4C RID: 2892 RVA: 0x00023A02 File Offset: 0x00021C02
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double2 operator %(double2 lhs, double rhs)
		{
			return new double2(lhs.x % rhs, lhs.y % rhs);
		}

		// Token: 0x06000B4D RID: 2893 RVA: 0x00023A19 File Offset: 0x00021C19
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double2 operator %(double lhs, double2 rhs)
		{
			return new double2(lhs % rhs.x, lhs % rhs.y);
		}

		// Token: 0x06000B4E RID: 2894 RVA: 0x00023A30 File Offset: 0x00021C30
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double2 operator ++(double2 val)
		{
			double num = val.x + 1.0;
			val.x = num;
			double num2 = num;
			num = val.y + 1.0;
			val.y = num;
			return new double2(num2, num);
		}

		// Token: 0x06000B4F RID: 2895 RVA: 0x00023A70 File Offset: 0x00021C70
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double2 operator --(double2 val)
		{
			double num = val.x - 1.0;
			val.x = num;
			double num2 = num;
			num = val.y - 1.0;
			val.y = num;
			return new double2(num2, num);
		}

		// Token: 0x06000B50 RID: 2896 RVA: 0x00023AB0 File Offset: 0x00021CB0
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool2 operator <(double2 lhs, double2 rhs)
		{
			return new bool2(lhs.x < rhs.x, lhs.y < rhs.y);
		}

		// Token: 0x06000B51 RID: 2897 RVA: 0x00023AD3 File Offset: 0x00021CD3
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool2 operator <(double2 lhs, double rhs)
		{
			return new bool2(lhs.x < rhs, lhs.y < rhs);
		}

		// Token: 0x06000B52 RID: 2898 RVA: 0x00023AEC File Offset: 0x00021CEC
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool2 operator <(double lhs, double2 rhs)
		{
			return new bool2(lhs < rhs.x, lhs < rhs.y);
		}

		// Token: 0x06000B53 RID: 2899 RVA: 0x00023B05 File Offset: 0x00021D05
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool2 operator <=(double2 lhs, double2 rhs)
		{
			return new bool2(lhs.x <= rhs.x, lhs.y <= rhs.y);
		}

		// Token: 0x06000B54 RID: 2900 RVA: 0x00023B2E File Offset: 0x00021D2E
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool2 operator <=(double2 lhs, double rhs)
		{
			return new bool2(lhs.x <= rhs, lhs.y <= rhs);
		}

		// Token: 0x06000B55 RID: 2901 RVA: 0x00023B4D File Offset: 0x00021D4D
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool2 operator <=(double lhs, double2 rhs)
		{
			return new bool2(lhs <= rhs.x, lhs <= rhs.y);
		}

		// Token: 0x06000B56 RID: 2902 RVA: 0x00023B6C File Offset: 0x00021D6C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool2 operator >(double2 lhs, double2 rhs)
		{
			return new bool2(lhs.x > rhs.x, lhs.y > rhs.y);
		}

		// Token: 0x06000B57 RID: 2903 RVA: 0x00023B8F File Offset: 0x00021D8F
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool2 operator >(double2 lhs, double rhs)
		{
			return new bool2(lhs.x > rhs, lhs.y > rhs);
		}

		// Token: 0x06000B58 RID: 2904 RVA: 0x00023BA8 File Offset: 0x00021DA8
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool2 operator >(double lhs, double2 rhs)
		{
			return new bool2(lhs > rhs.x, lhs > rhs.y);
		}

		// Token: 0x06000B59 RID: 2905 RVA: 0x00023BC1 File Offset: 0x00021DC1
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool2 operator >=(double2 lhs, double2 rhs)
		{
			return new bool2(lhs.x >= rhs.x, lhs.y >= rhs.y);
		}

		// Token: 0x06000B5A RID: 2906 RVA: 0x00023BEA File Offset: 0x00021DEA
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool2 operator >=(double2 lhs, double rhs)
		{
			return new bool2(lhs.x >= rhs, lhs.y >= rhs);
		}

		// Token: 0x06000B5B RID: 2907 RVA: 0x00023C09 File Offset: 0x00021E09
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool2 operator >=(double lhs, double2 rhs)
		{
			return new bool2(lhs >= rhs.x, lhs >= rhs.y);
		}

		// Token: 0x06000B5C RID: 2908 RVA: 0x00023C28 File Offset: 0x00021E28
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double2 operator -(double2 val)
		{
			return new double2(-val.x, -val.y);
		}

		// Token: 0x06000B5D RID: 2909 RVA: 0x00023C3D File Offset: 0x00021E3D
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double2 operator +(double2 val)
		{
			return new double2(val.x, val.y);
		}

		// Token: 0x06000B5E RID: 2910 RVA: 0x00023C50 File Offset: 0x00021E50
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool2 operator ==(double2 lhs, double2 rhs)
		{
			return new bool2(lhs.x == rhs.x, lhs.y == rhs.y);
		}

		// Token: 0x06000B5F RID: 2911 RVA: 0x00023C73 File Offset: 0x00021E73
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool2 operator ==(double2 lhs, double rhs)
		{
			return new bool2(lhs.x == rhs, lhs.y == rhs);
		}

		// Token: 0x06000B60 RID: 2912 RVA: 0x00023C8C File Offset: 0x00021E8C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool2 operator ==(double lhs, double2 rhs)
		{
			return new bool2(lhs == rhs.x, lhs == rhs.y);
		}

		// Token: 0x06000B61 RID: 2913 RVA: 0x00023CA5 File Offset: 0x00021EA5
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool2 operator !=(double2 lhs, double2 rhs)
		{
			return new bool2(lhs.x != rhs.x, lhs.y != rhs.y);
		}

		// Token: 0x06000B62 RID: 2914 RVA: 0x00023CCE File Offset: 0x00021ECE
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool2 operator !=(double2 lhs, double rhs)
		{
			return new bool2(lhs.x != rhs, lhs.y != rhs);
		}

		// Token: 0x06000B63 RID: 2915 RVA: 0x00023CED File Offset: 0x00021EED
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool2 operator !=(double lhs, double2 rhs)
		{
			return new bool2(lhs != rhs.x, lhs != rhs.y);
		}

		// Token: 0x170001EE RID: 494
		// (get) Token: 0x06000B64 RID: 2916 RVA: 0x00023D0C File Offset: 0x00021F0C
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double4 xxxx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double4(this.x, this.x, this.x, this.x);
			}
		}

		// Token: 0x170001EF RID: 495
		// (get) Token: 0x06000B65 RID: 2917 RVA: 0x00023D2B File Offset: 0x00021F2B
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double4 xxxy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double4(this.x, this.x, this.x, this.y);
			}
		}

		// Token: 0x170001F0 RID: 496
		// (get) Token: 0x06000B66 RID: 2918 RVA: 0x00023D4A File Offset: 0x00021F4A
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double4 xxyx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double4(this.x, this.x, this.y, this.x);
			}
		}

		// Token: 0x170001F1 RID: 497
		// (get) Token: 0x06000B67 RID: 2919 RVA: 0x00023D69 File Offset: 0x00021F69
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double4 xxyy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double4(this.x, this.x, this.y, this.y);
			}
		}

		// Token: 0x170001F2 RID: 498
		// (get) Token: 0x06000B68 RID: 2920 RVA: 0x00023D88 File Offset: 0x00021F88
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double4 xyxx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double4(this.x, this.y, this.x, this.x);
			}
		}

		// Token: 0x170001F3 RID: 499
		// (get) Token: 0x06000B69 RID: 2921 RVA: 0x00023DA7 File Offset: 0x00021FA7
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double4 xyxy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double4(this.x, this.y, this.x, this.y);
			}
		}

		// Token: 0x170001F4 RID: 500
		// (get) Token: 0x06000B6A RID: 2922 RVA: 0x00023DC6 File Offset: 0x00021FC6
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double4 xyyx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double4(this.x, this.y, this.y, this.x);
			}
		}

		// Token: 0x170001F5 RID: 501
		// (get) Token: 0x06000B6B RID: 2923 RVA: 0x00023DE5 File Offset: 0x00021FE5
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double4 xyyy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double4(this.x, this.y, this.y, this.y);
			}
		}

		// Token: 0x170001F6 RID: 502
		// (get) Token: 0x06000B6C RID: 2924 RVA: 0x00023E04 File Offset: 0x00022004
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double4 yxxx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double4(this.y, this.x, this.x, this.x);
			}
		}

		// Token: 0x170001F7 RID: 503
		// (get) Token: 0x06000B6D RID: 2925 RVA: 0x00023E23 File Offset: 0x00022023
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double4 yxxy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double4(this.y, this.x, this.x, this.y);
			}
		}

		// Token: 0x170001F8 RID: 504
		// (get) Token: 0x06000B6E RID: 2926 RVA: 0x00023E42 File Offset: 0x00022042
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double4 yxyx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double4(this.y, this.x, this.y, this.x);
			}
		}

		// Token: 0x170001F9 RID: 505
		// (get) Token: 0x06000B6F RID: 2927 RVA: 0x00023E61 File Offset: 0x00022061
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double4 yxyy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double4(this.y, this.x, this.y, this.y);
			}
		}

		// Token: 0x170001FA RID: 506
		// (get) Token: 0x06000B70 RID: 2928 RVA: 0x00023E80 File Offset: 0x00022080
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double4 yyxx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double4(this.y, this.y, this.x, this.x);
			}
		}

		// Token: 0x170001FB RID: 507
		// (get) Token: 0x06000B71 RID: 2929 RVA: 0x00023E9F File Offset: 0x0002209F
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double4 yyxy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double4(this.y, this.y, this.x, this.y);
			}
		}

		// Token: 0x170001FC RID: 508
		// (get) Token: 0x06000B72 RID: 2930 RVA: 0x00023EBE File Offset: 0x000220BE
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double4 yyyx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double4(this.y, this.y, this.y, this.x);
			}
		}

		// Token: 0x170001FD RID: 509
		// (get) Token: 0x06000B73 RID: 2931 RVA: 0x00023EDD File Offset: 0x000220DD
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double4 yyyy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double4(this.y, this.y, this.y, this.y);
			}
		}

		// Token: 0x170001FE RID: 510
		// (get) Token: 0x06000B74 RID: 2932 RVA: 0x00023EFC File Offset: 0x000220FC
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double3 xxx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double3(this.x, this.x, this.x);
			}
		}

		// Token: 0x170001FF RID: 511
		// (get) Token: 0x06000B75 RID: 2933 RVA: 0x00023F15 File Offset: 0x00022115
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double3 xxy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double3(this.x, this.x, this.y);
			}
		}

		// Token: 0x17000200 RID: 512
		// (get) Token: 0x06000B76 RID: 2934 RVA: 0x00023F2E File Offset: 0x0002212E
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double3 xyx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double3(this.x, this.y, this.x);
			}
		}

		// Token: 0x17000201 RID: 513
		// (get) Token: 0x06000B77 RID: 2935 RVA: 0x00023F47 File Offset: 0x00022147
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double3 xyy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double3(this.x, this.y, this.y);
			}
		}

		// Token: 0x17000202 RID: 514
		// (get) Token: 0x06000B78 RID: 2936 RVA: 0x00023F60 File Offset: 0x00022160
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double3 yxx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double3(this.y, this.x, this.x);
			}
		}

		// Token: 0x17000203 RID: 515
		// (get) Token: 0x06000B79 RID: 2937 RVA: 0x00023F79 File Offset: 0x00022179
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double3 yxy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double3(this.y, this.x, this.y);
			}
		}

		// Token: 0x17000204 RID: 516
		// (get) Token: 0x06000B7A RID: 2938 RVA: 0x00023F92 File Offset: 0x00022192
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double3 yyx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double3(this.y, this.y, this.x);
			}
		}

		// Token: 0x17000205 RID: 517
		// (get) Token: 0x06000B7B RID: 2939 RVA: 0x00023FAB File Offset: 0x000221AB
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double3 yyy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double3(this.y, this.y, this.y);
			}
		}

		// Token: 0x17000206 RID: 518
		// (get) Token: 0x06000B7C RID: 2940 RVA: 0x00023FC4 File Offset: 0x000221C4
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double2 xx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double2(this.x, this.x);
			}
		}

		// Token: 0x17000207 RID: 519
		// (get) Token: 0x06000B7D RID: 2941 RVA: 0x00023FD7 File Offset: 0x000221D7
		// (set) Token: 0x06000B7E RID: 2942 RVA: 0x00023FEA File Offset: 0x000221EA
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double2 xy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double2(this.x, this.y);
			}
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			set
			{
				this.x = value.x;
				this.y = value.y;
			}
		}

		// Token: 0x17000208 RID: 520
		// (get) Token: 0x06000B7F RID: 2943 RVA: 0x00024004 File Offset: 0x00022204
		// (set) Token: 0x06000B80 RID: 2944 RVA: 0x00024017 File Offset: 0x00022217
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double2 yx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double2(this.y, this.x);
			}
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			set
			{
				this.y = value.x;
				this.x = value.y;
			}
		}

		// Token: 0x17000209 RID: 521
		// (get) Token: 0x06000B81 RID: 2945 RVA: 0x00024031 File Offset: 0x00022231
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double2 yy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double2(this.y, this.y);
			}
		}

		// Token: 0x1700020A RID: 522
		public unsafe double this[int index]
		{
			get
			{
				fixed (double2* ptr = &this)
				{
					return ((double*)ptr)[index];
				}
			}
			set
			{
				fixed (double* ptr = &this.x)
				{
					ptr[index] = value;
				}
			}
		}

		// Token: 0x06000B84 RID: 2948 RVA: 0x0002407C File Offset: 0x0002227C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool Equals(double2 rhs)
		{
			return this.x == rhs.x && this.y == rhs.y;
		}

		// Token: 0x06000B85 RID: 2949 RVA: 0x0002409C File Offset: 0x0002229C
		public override bool Equals(object o)
		{
			if (o is double2)
			{
				double2 @double = (double2)o;
				return this.Equals(@double);
			}
			return false;
		}

		// Token: 0x06000B86 RID: 2950 RVA: 0x000240C1 File Offset: 0x000222C1
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public override int GetHashCode()
		{
			return (int)math.hash(this);
		}

		// Token: 0x06000B87 RID: 2951 RVA: 0x000240CE File Offset: 0x000222CE
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public override string ToString()
		{
			return string.Format("double2({0}, {1})", this.x, this.y);
		}

		// Token: 0x06000B88 RID: 2952 RVA: 0x000240F0 File Offset: 0x000222F0
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public string ToString(string format, IFormatProvider formatProvider)
		{
			return string.Format("double2({0}, {1})", this.x.ToString(format, formatProvider), this.y.ToString(format, formatProvider));
		}

		// Token: 0x04000049 RID: 73
		public double x;

		// Token: 0x0400004A RID: 74
		public double y;

		// Token: 0x0400004B RID: 75
		public static readonly double2 zero;

		// Token: 0x02000058 RID: 88
		internal sealed class DebuggerProxy
		{
			// Token: 0x060024C5 RID: 9413 RVA: 0x00069180 File Offset: 0x00067380
			public DebuggerProxy(double2 v)
			{
				this.x = v.x;
				this.y = v.y;
			}

			// Token: 0x04000153 RID: 339
			public double x;

			// Token: 0x04000154 RID: 340
			public double y;
		}
	}
}
