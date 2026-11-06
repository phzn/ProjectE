using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Unity.IL2CPP.CompilerServices;

namespace Unity.Mathematics
{
	// Token: 0x0200001A RID: 26
	[DebuggerTypeProxy(typeof(double4.DebuggerProxy))]
	[Il2CppEagerStaticClassConstruction]
	[Serializable]
	public struct double4 : IEquatable<double4>, IFormattable
	{
		// Token: 0x06000DCC RID: 3532 RVA: 0x0002A45D File Offset: 0x0002865D
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public double4(double x, double y, double z, double w)
		{
			this.x = x;
			this.y = y;
			this.z = z;
			this.w = w;
		}

		// Token: 0x06000DCD RID: 3533 RVA: 0x0002A47C File Offset: 0x0002867C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public double4(double x, double y, double2 zw)
		{
			this.x = x;
			this.y = y;
			this.z = zw.x;
			this.w = zw.y;
		}

		// Token: 0x06000DCE RID: 3534 RVA: 0x0002A4A4 File Offset: 0x000286A4
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public double4(double x, double2 yz, double w)
		{
			this.x = x;
			this.y = yz.x;
			this.z = yz.y;
			this.w = w;
		}

		// Token: 0x06000DCF RID: 3535 RVA: 0x0002A4CC File Offset: 0x000286CC
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public double4(double x, double3 yzw)
		{
			this.x = x;
			this.y = yzw.x;
			this.z = yzw.y;
			this.w = yzw.z;
		}

		// Token: 0x06000DD0 RID: 3536 RVA: 0x0002A4F9 File Offset: 0x000286F9
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public double4(double2 xy, double z, double w)
		{
			this.x = xy.x;
			this.y = xy.y;
			this.z = z;
			this.w = w;
		}

		// Token: 0x06000DD1 RID: 3537 RVA: 0x0002A521 File Offset: 0x00028721
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public double4(double2 xy, double2 zw)
		{
			this.x = xy.x;
			this.y = xy.y;
			this.z = zw.x;
			this.w = zw.y;
		}

		// Token: 0x06000DD2 RID: 3538 RVA: 0x0002A553 File Offset: 0x00028753
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public double4(double3 xyz, double w)
		{
			this.x = xyz.x;
			this.y = xyz.y;
			this.z = xyz.z;
			this.w = w;
		}

		// Token: 0x06000DD3 RID: 3539 RVA: 0x0002A580 File Offset: 0x00028780
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public double4(double4 xyzw)
		{
			this.x = xyzw.x;
			this.y = xyzw.y;
			this.z = xyzw.z;
			this.w = xyzw.w;
		}

		// Token: 0x06000DD4 RID: 3540 RVA: 0x0002A5B2 File Offset: 0x000287B2
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public double4(double v)
		{
			this.x = v;
			this.y = v;
			this.z = v;
			this.w = v;
		}

		// Token: 0x06000DD5 RID: 3541 RVA: 0x0002A5D0 File Offset: 0x000287D0
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public double4(bool v)
		{
			this.x = (v ? 1.0 : 0.0);
			this.y = (v ? 1.0 : 0.0);
			this.z = (v ? 1.0 : 0.0);
			this.w = (v ? 1.0 : 0.0);
		}

		// Token: 0x06000DD6 RID: 3542 RVA: 0x0002A654 File Offset: 0x00028854
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public double4(bool4 v)
		{
			this.x = (v.x ? 1.0 : 0.0);
			this.y = (v.y ? 1.0 : 0.0);
			this.z = (v.z ? 1.0 : 0.0);
			this.w = (v.w ? 1.0 : 0.0);
		}

		// Token: 0x06000DD7 RID: 3543 RVA: 0x0002A6E9 File Offset: 0x000288E9
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public double4(int v)
		{
			this.x = (double)v;
			this.y = (double)v;
			this.z = (double)v;
			this.w = (double)v;
		}

		// Token: 0x06000DD8 RID: 3544 RVA: 0x0002A70B File Offset: 0x0002890B
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public double4(int4 v)
		{
			this.x = (double)v.x;
			this.y = (double)v.y;
			this.z = (double)v.z;
			this.w = (double)v.w;
		}

		// Token: 0x06000DD9 RID: 3545 RVA: 0x0002A741 File Offset: 0x00028941
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public double4(uint v)
		{
			this.x = v;
			this.y = v;
			this.z = v;
			this.w = v;
		}

		// Token: 0x06000DDA RID: 3546 RVA: 0x0002A767 File Offset: 0x00028967
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public double4(uint4 v)
		{
			this.x = v.x;
			this.y = v.y;
			this.z = v.z;
			this.w = v.w;
		}

		// Token: 0x06000DDB RID: 3547 RVA: 0x0002A7A1 File Offset: 0x000289A1
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public double4(half v)
		{
			this.x = v;
			this.y = v;
			this.z = v;
			this.w = v;
		}

		// Token: 0x06000DDC RID: 3548 RVA: 0x0002A7D4 File Offset: 0x000289D4
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public double4(half4 v)
		{
			this.x = v.x;
			this.y = v.y;
			this.z = v.z;
			this.w = v.w;
		}

		// Token: 0x06000DDD RID: 3549 RVA: 0x0002A825 File Offset: 0x00028A25
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public double4(float v)
		{
			this.x = (double)v;
			this.y = (double)v;
			this.z = (double)v;
			this.w = (double)v;
		}

		// Token: 0x06000DDE RID: 3550 RVA: 0x0002A847 File Offset: 0x00028A47
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public double4(float4 v)
		{
			this.x = (double)v.x;
			this.y = (double)v.y;
			this.z = (double)v.z;
			this.w = (double)v.w;
		}

		// Token: 0x06000DDF RID: 3551 RVA: 0x0002A87D File Offset: 0x00028A7D
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static implicit operator double4(double v)
		{
			return new double4(v);
		}

		// Token: 0x06000DE0 RID: 3552 RVA: 0x0002A885 File Offset: 0x00028A85
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static explicit operator double4(bool v)
		{
			return new double4(v);
		}

		// Token: 0x06000DE1 RID: 3553 RVA: 0x0002A88D File Offset: 0x00028A8D
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static explicit operator double4(bool4 v)
		{
			return new double4(v);
		}

		// Token: 0x06000DE2 RID: 3554 RVA: 0x0002A895 File Offset: 0x00028A95
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static implicit operator double4(int v)
		{
			return new double4(v);
		}

		// Token: 0x06000DE3 RID: 3555 RVA: 0x0002A89D File Offset: 0x00028A9D
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static implicit operator double4(int4 v)
		{
			return new double4(v);
		}

		// Token: 0x06000DE4 RID: 3556 RVA: 0x0002A8A5 File Offset: 0x00028AA5
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static implicit operator double4(uint v)
		{
			return new double4(v);
		}

		// Token: 0x06000DE5 RID: 3557 RVA: 0x0002A8AD File Offset: 0x00028AAD
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static implicit operator double4(uint4 v)
		{
			return new double4(v);
		}

		// Token: 0x06000DE6 RID: 3558 RVA: 0x0002A8B5 File Offset: 0x00028AB5
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static implicit operator double4(half v)
		{
			return new double4(v);
		}

		// Token: 0x06000DE7 RID: 3559 RVA: 0x0002A8BD File Offset: 0x00028ABD
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static implicit operator double4(half4 v)
		{
			return new double4(v);
		}

		// Token: 0x06000DE8 RID: 3560 RVA: 0x0002A8C5 File Offset: 0x00028AC5
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static implicit operator double4(float v)
		{
			return new double4(v);
		}

		// Token: 0x06000DE9 RID: 3561 RVA: 0x0002A8CD File Offset: 0x00028ACD
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static implicit operator double4(float4 v)
		{
			return new double4(v);
		}

		// Token: 0x06000DEA RID: 3562 RVA: 0x0002A8D5 File Offset: 0x00028AD5
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double4 operator *(double4 lhs, double4 rhs)
		{
			return new double4(lhs.x * rhs.x, lhs.y * rhs.y, lhs.z * rhs.z, lhs.w * rhs.w);
		}

		// Token: 0x06000DEB RID: 3563 RVA: 0x0002A910 File Offset: 0x00028B10
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double4 operator *(double4 lhs, double rhs)
		{
			return new double4(lhs.x * rhs, lhs.y * rhs, lhs.z * rhs, lhs.w * rhs);
		}

		// Token: 0x06000DEC RID: 3564 RVA: 0x0002A937 File Offset: 0x00028B37
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double4 operator *(double lhs, double4 rhs)
		{
			return new double4(lhs * rhs.x, lhs * rhs.y, lhs * rhs.z, lhs * rhs.w);
		}

		// Token: 0x06000DED RID: 3565 RVA: 0x0002A95E File Offset: 0x00028B5E
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double4 operator +(double4 lhs, double4 rhs)
		{
			return new double4(lhs.x + rhs.x, lhs.y + rhs.y, lhs.z + rhs.z, lhs.w + rhs.w);
		}

		// Token: 0x06000DEE RID: 3566 RVA: 0x0002A999 File Offset: 0x00028B99
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double4 operator +(double4 lhs, double rhs)
		{
			return new double4(lhs.x + rhs, lhs.y + rhs, lhs.z + rhs, lhs.w + rhs);
		}

		// Token: 0x06000DEF RID: 3567 RVA: 0x0002A9C0 File Offset: 0x00028BC0
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double4 operator +(double lhs, double4 rhs)
		{
			return new double4(lhs + rhs.x, lhs + rhs.y, lhs + rhs.z, lhs + rhs.w);
		}

		// Token: 0x06000DF0 RID: 3568 RVA: 0x0002A9E7 File Offset: 0x00028BE7
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double4 operator -(double4 lhs, double4 rhs)
		{
			return new double4(lhs.x - rhs.x, lhs.y - rhs.y, lhs.z - rhs.z, lhs.w - rhs.w);
		}

		// Token: 0x06000DF1 RID: 3569 RVA: 0x0002AA22 File Offset: 0x00028C22
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double4 operator -(double4 lhs, double rhs)
		{
			return new double4(lhs.x - rhs, lhs.y - rhs, lhs.z - rhs, lhs.w - rhs);
		}

		// Token: 0x06000DF2 RID: 3570 RVA: 0x0002AA49 File Offset: 0x00028C49
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double4 operator -(double lhs, double4 rhs)
		{
			return new double4(lhs - rhs.x, lhs - rhs.y, lhs - rhs.z, lhs - rhs.w);
		}

		// Token: 0x06000DF3 RID: 3571 RVA: 0x0002AA70 File Offset: 0x00028C70
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double4 operator /(double4 lhs, double4 rhs)
		{
			return new double4(lhs.x / rhs.x, lhs.y / rhs.y, lhs.z / rhs.z, lhs.w / rhs.w);
		}

		// Token: 0x06000DF4 RID: 3572 RVA: 0x0002AAAB File Offset: 0x00028CAB
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double4 operator /(double4 lhs, double rhs)
		{
			return new double4(lhs.x / rhs, lhs.y / rhs, lhs.z / rhs, lhs.w / rhs);
		}

		// Token: 0x06000DF5 RID: 3573 RVA: 0x0002AAD2 File Offset: 0x00028CD2
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double4 operator /(double lhs, double4 rhs)
		{
			return new double4(lhs / rhs.x, lhs / rhs.y, lhs / rhs.z, lhs / rhs.w);
		}

		// Token: 0x06000DF6 RID: 3574 RVA: 0x0002AAF9 File Offset: 0x00028CF9
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double4 operator %(double4 lhs, double4 rhs)
		{
			return new double4(lhs.x % rhs.x, lhs.y % rhs.y, lhs.z % rhs.z, lhs.w % rhs.w);
		}

		// Token: 0x06000DF7 RID: 3575 RVA: 0x0002AB34 File Offset: 0x00028D34
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double4 operator %(double4 lhs, double rhs)
		{
			return new double4(lhs.x % rhs, lhs.y % rhs, lhs.z % rhs, lhs.w % rhs);
		}

		// Token: 0x06000DF8 RID: 3576 RVA: 0x0002AB5B File Offset: 0x00028D5B
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double4 operator %(double lhs, double4 rhs)
		{
			return new double4(lhs % rhs.x, lhs % rhs.y, lhs % rhs.z, lhs % rhs.w);
		}

		// Token: 0x06000DF9 RID: 3577 RVA: 0x0002AB84 File Offset: 0x00028D84
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double4 operator ++(double4 val)
		{
			double num = val.x + 1.0;
			val.x = num;
			double num2 = num;
			num = val.y + 1.0;
			val.y = num;
			double num3 = num;
			num = val.z + 1.0;
			val.z = num;
			double num4 = num;
			num = val.w + 1.0;
			val.w = num;
			return new double4(num2, num3, num4, num);
		}

		// Token: 0x06000DFA RID: 3578 RVA: 0x0002ABF4 File Offset: 0x00028DF4
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double4 operator --(double4 val)
		{
			double num = val.x - 1.0;
			val.x = num;
			double num2 = num;
			num = val.y - 1.0;
			val.y = num;
			double num3 = num;
			num = val.z - 1.0;
			val.z = num;
			double num4 = num;
			num = val.w - 1.0;
			val.w = num;
			return new double4(num2, num3, num4, num);
		}

		// Token: 0x06000DFB RID: 3579 RVA: 0x0002AC62 File Offset: 0x00028E62
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool4 operator <(double4 lhs, double4 rhs)
		{
			return new bool4(lhs.x < rhs.x, lhs.y < rhs.y, lhs.z < rhs.z, lhs.w < rhs.w);
		}

		// Token: 0x06000DFC RID: 3580 RVA: 0x0002ACA1 File Offset: 0x00028EA1
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool4 operator <(double4 lhs, double rhs)
		{
			return new bool4(lhs.x < rhs, lhs.y < rhs, lhs.z < rhs, lhs.w < rhs);
		}

		// Token: 0x06000DFD RID: 3581 RVA: 0x0002ACCC File Offset: 0x00028ECC
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool4 operator <(double lhs, double4 rhs)
		{
			return new bool4(lhs < rhs.x, lhs < rhs.y, lhs < rhs.z, lhs < rhs.w);
		}

		// Token: 0x06000DFE RID: 3582 RVA: 0x0002ACF8 File Offset: 0x00028EF8
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool4 operator <=(double4 lhs, double4 rhs)
		{
			return new bool4(lhs.x <= rhs.x, lhs.y <= rhs.y, lhs.z <= rhs.z, lhs.w <= rhs.w);
		}

		// Token: 0x06000DFF RID: 3583 RVA: 0x0002AD4E File Offset: 0x00028F4E
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool4 operator <=(double4 lhs, double rhs)
		{
			return new bool4(lhs.x <= rhs, lhs.y <= rhs, lhs.z <= rhs, lhs.w <= rhs);
		}

		// Token: 0x06000E00 RID: 3584 RVA: 0x0002AD85 File Offset: 0x00028F85
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool4 operator <=(double lhs, double4 rhs)
		{
			return new bool4(lhs <= rhs.x, lhs <= rhs.y, lhs <= rhs.z, lhs <= rhs.w);
		}

		// Token: 0x06000E01 RID: 3585 RVA: 0x0002ADBC File Offset: 0x00028FBC
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool4 operator >(double4 lhs, double4 rhs)
		{
			return new bool4(lhs.x > rhs.x, lhs.y > rhs.y, lhs.z > rhs.z, lhs.w > rhs.w);
		}

		// Token: 0x06000E02 RID: 3586 RVA: 0x0002ADFB File Offset: 0x00028FFB
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool4 operator >(double4 lhs, double rhs)
		{
			return new bool4(lhs.x > rhs, lhs.y > rhs, lhs.z > rhs, lhs.w > rhs);
		}

		// Token: 0x06000E03 RID: 3587 RVA: 0x0002AE26 File Offset: 0x00029026
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool4 operator >(double lhs, double4 rhs)
		{
			return new bool4(lhs > rhs.x, lhs > rhs.y, lhs > rhs.z, lhs > rhs.w);
		}

		// Token: 0x06000E04 RID: 3588 RVA: 0x0002AE54 File Offset: 0x00029054
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool4 operator >=(double4 lhs, double4 rhs)
		{
			return new bool4(lhs.x >= rhs.x, lhs.y >= rhs.y, lhs.z >= rhs.z, lhs.w >= rhs.w);
		}

		// Token: 0x06000E05 RID: 3589 RVA: 0x0002AEAA File Offset: 0x000290AA
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool4 operator >=(double4 lhs, double rhs)
		{
			return new bool4(lhs.x >= rhs, lhs.y >= rhs, lhs.z >= rhs, lhs.w >= rhs);
		}

		// Token: 0x06000E06 RID: 3590 RVA: 0x0002AEE1 File Offset: 0x000290E1
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool4 operator >=(double lhs, double4 rhs)
		{
			return new bool4(lhs >= rhs.x, lhs >= rhs.y, lhs >= rhs.z, lhs >= rhs.w);
		}

		// Token: 0x06000E07 RID: 3591 RVA: 0x0002AF18 File Offset: 0x00029118
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double4 operator -(double4 val)
		{
			return new double4(-val.x, -val.y, -val.z, -val.w);
		}

		// Token: 0x06000E08 RID: 3592 RVA: 0x0002AF3B File Offset: 0x0002913B
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double4 operator +(double4 val)
		{
			return new double4(val.x, val.y, val.z, val.w);
		}

		// Token: 0x06000E09 RID: 3593 RVA: 0x0002AF5A File Offset: 0x0002915A
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool4 operator ==(double4 lhs, double4 rhs)
		{
			return new bool4(lhs.x == rhs.x, lhs.y == rhs.y, lhs.z == rhs.z, lhs.w == rhs.w);
		}

		// Token: 0x06000E0A RID: 3594 RVA: 0x0002AF99 File Offset: 0x00029199
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool4 operator ==(double4 lhs, double rhs)
		{
			return new bool4(lhs.x == rhs, lhs.y == rhs, lhs.z == rhs, lhs.w == rhs);
		}

		// Token: 0x06000E0B RID: 3595 RVA: 0x0002AFC4 File Offset: 0x000291C4
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool4 operator ==(double lhs, double4 rhs)
		{
			return new bool4(lhs == rhs.x, lhs == rhs.y, lhs == rhs.z, lhs == rhs.w);
		}

		// Token: 0x06000E0C RID: 3596 RVA: 0x0002AFF0 File Offset: 0x000291F0
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool4 operator !=(double4 lhs, double4 rhs)
		{
			return new bool4(lhs.x != rhs.x, lhs.y != rhs.y, lhs.z != rhs.z, lhs.w != rhs.w);
		}

		// Token: 0x06000E0D RID: 3597 RVA: 0x0002B046 File Offset: 0x00029246
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool4 operator !=(double4 lhs, double rhs)
		{
			return new bool4(lhs.x != rhs, lhs.y != rhs, lhs.z != rhs, lhs.w != rhs);
		}

		// Token: 0x06000E0E RID: 3598 RVA: 0x0002B07D File Offset: 0x0002927D
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool4 operator !=(double lhs, double4 rhs)
		{
			return new bool4(lhs != rhs.x, lhs != rhs.y, lhs != rhs.z, lhs != rhs.w);
		}

		// Token: 0x17000287 RID: 647
		// (get) Token: 0x06000E0F RID: 3599 RVA: 0x0002B0B4 File Offset: 0x000292B4
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double4 xxxx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double4(this.x, this.x, this.x, this.x);
			}
		}

		// Token: 0x17000288 RID: 648
		// (get) Token: 0x06000E10 RID: 3600 RVA: 0x0002B0D3 File Offset: 0x000292D3
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double4 xxxy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double4(this.x, this.x, this.x, this.y);
			}
		}

		// Token: 0x17000289 RID: 649
		// (get) Token: 0x06000E11 RID: 3601 RVA: 0x0002B0F2 File Offset: 0x000292F2
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double4 xxxz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double4(this.x, this.x, this.x, this.z);
			}
		}

		// Token: 0x1700028A RID: 650
		// (get) Token: 0x06000E12 RID: 3602 RVA: 0x0002B111 File Offset: 0x00029311
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double4 xxxw
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double4(this.x, this.x, this.x, this.w);
			}
		}

		// Token: 0x1700028B RID: 651
		// (get) Token: 0x06000E13 RID: 3603 RVA: 0x0002B130 File Offset: 0x00029330
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double4 xxyx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double4(this.x, this.x, this.y, this.x);
			}
		}

		// Token: 0x1700028C RID: 652
		// (get) Token: 0x06000E14 RID: 3604 RVA: 0x0002B14F File Offset: 0x0002934F
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double4 xxyy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double4(this.x, this.x, this.y, this.y);
			}
		}

		// Token: 0x1700028D RID: 653
		// (get) Token: 0x06000E15 RID: 3605 RVA: 0x0002B16E File Offset: 0x0002936E
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double4 xxyz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double4(this.x, this.x, this.y, this.z);
			}
		}

		// Token: 0x1700028E RID: 654
		// (get) Token: 0x06000E16 RID: 3606 RVA: 0x0002B18D File Offset: 0x0002938D
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double4 xxyw
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double4(this.x, this.x, this.y, this.w);
			}
		}

		// Token: 0x1700028F RID: 655
		// (get) Token: 0x06000E17 RID: 3607 RVA: 0x0002B1AC File Offset: 0x000293AC
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double4 xxzx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double4(this.x, this.x, this.z, this.x);
			}
		}

		// Token: 0x17000290 RID: 656
		// (get) Token: 0x06000E18 RID: 3608 RVA: 0x0002B1CB File Offset: 0x000293CB
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double4 xxzy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double4(this.x, this.x, this.z, this.y);
			}
		}

		// Token: 0x17000291 RID: 657
		// (get) Token: 0x06000E19 RID: 3609 RVA: 0x0002B1EA File Offset: 0x000293EA
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double4 xxzz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double4(this.x, this.x, this.z, this.z);
			}
		}

		// Token: 0x17000292 RID: 658
		// (get) Token: 0x06000E1A RID: 3610 RVA: 0x0002B209 File Offset: 0x00029409
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double4 xxzw
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double4(this.x, this.x, this.z, this.w);
			}
		}

		// Token: 0x17000293 RID: 659
		// (get) Token: 0x06000E1B RID: 3611 RVA: 0x0002B228 File Offset: 0x00029428
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double4 xxwx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double4(this.x, this.x, this.w, this.x);
			}
		}

		// Token: 0x17000294 RID: 660
		// (get) Token: 0x06000E1C RID: 3612 RVA: 0x0002B247 File Offset: 0x00029447
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double4 xxwy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double4(this.x, this.x, this.w, this.y);
			}
		}

		// Token: 0x17000295 RID: 661
		// (get) Token: 0x06000E1D RID: 3613 RVA: 0x0002B266 File Offset: 0x00029466
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double4 xxwz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double4(this.x, this.x, this.w, this.z);
			}
		}

		// Token: 0x17000296 RID: 662
		// (get) Token: 0x06000E1E RID: 3614 RVA: 0x0002B285 File Offset: 0x00029485
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double4 xxww
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double4(this.x, this.x, this.w, this.w);
			}
		}

		// Token: 0x17000297 RID: 663
		// (get) Token: 0x06000E1F RID: 3615 RVA: 0x0002B2A4 File Offset: 0x000294A4
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double4 xyxx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double4(this.x, this.y, this.x, this.x);
			}
		}

		// Token: 0x17000298 RID: 664
		// (get) Token: 0x06000E20 RID: 3616 RVA: 0x0002B2C3 File Offset: 0x000294C3
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double4 xyxy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double4(this.x, this.y, this.x, this.y);
			}
		}

		// Token: 0x17000299 RID: 665
		// (get) Token: 0x06000E21 RID: 3617 RVA: 0x0002B2E2 File Offset: 0x000294E2
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double4 xyxz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double4(this.x, this.y, this.x, this.z);
			}
		}

		// Token: 0x1700029A RID: 666
		// (get) Token: 0x06000E22 RID: 3618 RVA: 0x0002B301 File Offset: 0x00029501
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double4 xyxw
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double4(this.x, this.y, this.x, this.w);
			}
		}

		// Token: 0x1700029B RID: 667
		// (get) Token: 0x06000E23 RID: 3619 RVA: 0x0002B320 File Offset: 0x00029520
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double4 xyyx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double4(this.x, this.y, this.y, this.x);
			}
		}

		// Token: 0x1700029C RID: 668
		// (get) Token: 0x06000E24 RID: 3620 RVA: 0x0002B33F File Offset: 0x0002953F
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double4 xyyy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double4(this.x, this.y, this.y, this.y);
			}
		}

		// Token: 0x1700029D RID: 669
		// (get) Token: 0x06000E25 RID: 3621 RVA: 0x0002B35E File Offset: 0x0002955E
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double4 xyyz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double4(this.x, this.y, this.y, this.z);
			}
		}

		// Token: 0x1700029E RID: 670
		// (get) Token: 0x06000E26 RID: 3622 RVA: 0x0002B37D File Offset: 0x0002957D
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double4 xyyw
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double4(this.x, this.y, this.y, this.w);
			}
		}

		// Token: 0x1700029F RID: 671
		// (get) Token: 0x06000E27 RID: 3623 RVA: 0x0002B39C File Offset: 0x0002959C
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double4 xyzx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double4(this.x, this.y, this.z, this.x);
			}
		}

		// Token: 0x170002A0 RID: 672
		// (get) Token: 0x06000E28 RID: 3624 RVA: 0x0002B3BB File Offset: 0x000295BB
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double4 xyzy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double4(this.x, this.y, this.z, this.y);
			}
		}

		// Token: 0x170002A1 RID: 673
		// (get) Token: 0x06000E29 RID: 3625 RVA: 0x0002B3DA File Offset: 0x000295DA
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double4 xyzz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double4(this.x, this.y, this.z, this.z);
			}
		}

		// Token: 0x170002A2 RID: 674
		// (get) Token: 0x06000E2A RID: 3626 RVA: 0x0002B3F9 File Offset: 0x000295F9
		// (set) Token: 0x06000E2B RID: 3627 RVA: 0x0002B418 File Offset: 0x00029618
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double4 xyzw
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double4(this.x, this.y, this.z, this.w);
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

		// Token: 0x170002A3 RID: 675
		// (get) Token: 0x06000E2C RID: 3628 RVA: 0x0002B44A File Offset: 0x0002964A
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double4 xywx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double4(this.x, this.y, this.w, this.x);
			}
		}

		// Token: 0x170002A4 RID: 676
		// (get) Token: 0x06000E2D RID: 3629 RVA: 0x0002B469 File Offset: 0x00029669
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double4 xywy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double4(this.x, this.y, this.w, this.y);
			}
		}

		// Token: 0x170002A5 RID: 677
		// (get) Token: 0x06000E2E RID: 3630 RVA: 0x0002B488 File Offset: 0x00029688
		// (set) Token: 0x06000E2F RID: 3631 RVA: 0x0002B4A7 File Offset: 0x000296A7
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double4 xywz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double4(this.x, this.y, this.w, this.z);
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

		// Token: 0x170002A6 RID: 678
		// (get) Token: 0x06000E30 RID: 3632 RVA: 0x0002B4D9 File Offset: 0x000296D9
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double4 xyww
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double4(this.x, this.y, this.w, this.w);
			}
		}

		// Token: 0x170002A7 RID: 679
		// (get) Token: 0x06000E31 RID: 3633 RVA: 0x0002B4F8 File Offset: 0x000296F8
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double4 xzxx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double4(this.x, this.z, this.x, this.x);
			}
		}

		// Token: 0x170002A8 RID: 680
		// (get) Token: 0x06000E32 RID: 3634 RVA: 0x0002B517 File Offset: 0x00029717
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double4 xzxy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double4(this.x, this.z, this.x, this.y);
			}
		}

		// Token: 0x170002A9 RID: 681
		// (get) Token: 0x06000E33 RID: 3635 RVA: 0x0002B536 File Offset: 0x00029736
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double4 xzxz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double4(this.x, this.z, this.x, this.z);
			}
		}

		// Token: 0x170002AA RID: 682
		// (get) Token: 0x06000E34 RID: 3636 RVA: 0x0002B555 File Offset: 0x00029755
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double4 xzxw
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double4(this.x, this.z, this.x, this.w);
			}
		}

		// Token: 0x170002AB RID: 683
		// (get) Token: 0x06000E35 RID: 3637 RVA: 0x0002B574 File Offset: 0x00029774
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double4 xzyx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double4(this.x, this.z, this.y, this.x);
			}
		}

		// Token: 0x170002AC RID: 684
		// (get) Token: 0x06000E36 RID: 3638 RVA: 0x0002B593 File Offset: 0x00029793
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double4 xzyy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double4(this.x, this.z, this.y, this.y);
			}
		}

		// Token: 0x170002AD RID: 685
		// (get) Token: 0x06000E37 RID: 3639 RVA: 0x0002B5B2 File Offset: 0x000297B2
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double4 xzyz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double4(this.x, this.z, this.y, this.z);
			}
		}

		// Token: 0x170002AE RID: 686
		// (get) Token: 0x06000E38 RID: 3640 RVA: 0x0002B5D1 File Offset: 0x000297D1
		// (set) Token: 0x06000E39 RID: 3641 RVA: 0x0002B5F0 File Offset: 0x000297F0
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double4 xzyw
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double4(this.x, this.z, this.y, this.w);
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

		// Token: 0x170002AF RID: 687
		// (get) Token: 0x06000E3A RID: 3642 RVA: 0x0002B622 File Offset: 0x00029822
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double4 xzzx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double4(this.x, this.z, this.z, this.x);
			}
		}

		// Token: 0x170002B0 RID: 688
		// (get) Token: 0x06000E3B RID: 3643 RVA: 0x0002B641 File Offset: 0x00029841
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double4 xzzy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double4(this.x, this.z, this.z, this.y);
			}
		}

		// Token: 0x170002B1 RID: 689
		// (get) Token: 0x06000E3C RID: 3644 RVA: 0x0002B660 File Offset: 0x00029860
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double4 xzzz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double4(this.x, this.z, this.z, this.z);
			}
		}

		// Token: 0x170002B2 RID: 690
		// (get) Token: 0x06000E3D RID: 3645 RVA: 0x0002B67F File Offset: 0x0002987F
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double4 xzzw
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double4(this.x, this.z, this.z, this.w);
			}
		}

		// Token: 0x170002B3 RID: 691
		// (get) Token: 0x06000E3E RID: 3646 RVA: 0x0002B69E File Offset: 0x0002989E
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double4 xzwx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double4(this.x, this.z, this.w, this.x);
			}
		}

		// Token: 0x170002B4 RID: 692
		// (get) Token: 0x06000E3F RID: 3647 RVA: 0x0002B6BD File Offset: 0x000298BD
		// (set) Token: 0x06000E40 RID: 3648 RVA: 0x0002B6DC File Offset: 0x000298DC
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double4 xzwy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double4(this.x, this.z, this.w, this.y);
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

		// Token: 0x170002B5 RID: 693
		// (get) Token: 0x06000E41 RID: 3649 RVA: 0x0002B70E File Offset: 0x0002990E
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double4 xzwz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double4(this.x, this.z, this.w, this.z);
			}
		}

		// Token: 0x170002B6 RID: 694
		// (get) Token: 0x06000E42 RID: 3650 RVA: 0x0002B72D File Offset: 0x0002992D
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double4 xzww
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double4(this.x, this.z, this.w, this.w);
			}
		}

		// Token: 0x170002B7 RID: 695
		// (get) Token: 0x06000E43 RID: 3651 RVA: 0x0002B74C File Offset: 0x0002994C
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double4 xwxx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double4(this.x, this.w, this.x, this.x);
			}
		}

		// Token: 0x170002B8 RID: 696
		// (get) Token: 0x06000E44 RID: 3652 RVA: 0x0002B76B File Offset: 0x0002996B
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double4 xwxy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double4(this.x, this.w, this.x, this.y);
			}
		}

		// Token: 0x170002B9 RID: 697
		// (get) Token: 0x06000E45 RID: 3653 RVA: 0x0002B78A File Offset: 0x0002998A
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double4 xwxz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double4(this.x, this.w, this.x, this.z);
			}
		}

		// Token: 0x170002BA RID: 698
		// (get) Token: 0x06000E46 RID: 3654 RVA: 0x0002B7A9 File Offset: 0x000299A9
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double4 xwxw
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double4(this.x, this.w, this.x, this.w);
			}
		}

		// Token: 0x170002BB RID: 699
		// (get) Token: 0x06000E47 RID: 3655 RVA: 0x0002B7C8 File Offset: 0x000299C8
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double4 xwyx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double4(this.x, this.w, this.y, this.x);
			}
		}

		// Token: 0x170002BC RID: 700
		// (get) Token: 0x06000E48 RID: 3656 RVA: 0x0002B7E7 File Offset: 0x000299E7
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double4 xwyy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double4(this.x, this.w, this.y, this.y);
			}
		}

		// Token: 0x170002BD RID: 701
		// (get) Token: 0x06000E49 RID: 3657 RVA: 0x0002B806 File Offset: 0x00029A06
		// (set) Token: 0x06000E4A RID: 3658 RVA: 0x0002B825 File Offset: 0x00029A25
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double4 xwyz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double4(this.x, this.w, this.y, this.z);
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

		// Token: 0x170002BE RID: 702
		// (get) Token: 0x06000E4B RID: 3659 RVA: 0x0002B857 File Offset: 0x00029A57
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double4 xwyw
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double4(this.x, this.w, this.y, this.w);
			}
		}

		// Token: 0x170002BF RID: 703
		// (get) Token: 0x06000E4C RID: 3660 RVA: 0x0002B876 File Offset: 0x00029A76
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double4 xwzx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double4(this.x, this.w, this.z, this.x);
			}
		}

		// Token: 0x170002C0 RID: 704
		// (get) Token: 0x06000E4D RID: 3661 RVA: 0x0002B895 File Offset: 0x00029A95
		// (set) Token: 0x06000E4E RID: 3662 RVA: 0x0002B8B4 File Offset: 0x00029AB4
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double4 xwzy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double4(this.x, this.w, this.z, this.y);
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

		// Token: 0x170002C1 RID: 705
		// (get) Token: 0x06000E4F RID: 3663 RVA: 0x0002B8E6 File Offset: 0x00029AE6
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double4 xwzz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double4(this.x, this.w, this.z, this.z);
			}
		}

		// Token: 0x170002C2 RID: 706
		// (get) Token: 0x06000E50 RID: 3664 RVA: 0x0002B905 File Offset: 0x00029B05
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double4 xwzw
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double4(this.x, this.w, this.z, this.w);
			}
		}

		// Token: 0x170002C3 RID: 707
		// (get) Token: 0x06000E51 RID: 3665 RVA: 0x0002B924 File Offset: 0x00029B24
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double4 xwwx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double4(this.x, this.w, this.w, this.x);
			}
		}

		// Token: 0x170002C4 RID: 708
		// (get) Token: 0x06000E52 RID: 3666 RVA: 0x0002B943 File Offset: 0x00029B43
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double4 xwwy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double4(this.x, this.w, this.w, this.y);
			}
		}

		// Token: 0x170002C5 RID: 709
		// (get) Token: 0x06000E53 RID: 3667 RVA: 0x0002B962 File Offset: 0x00029B62
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double4 xwwz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double4(this.x, this.w, this.w, this.z);
			}
		}

		// Token: 0x170002C6 RID: 710
		// (get) Token: 0x06000E54 RID: 3668 RVA: 0x0002B981 File Offset: 0x00029B81
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double4 xwww
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double4(this.x, this.w, this.w, this.w);
			}
		}

		// Token: 0x170002C7 RID: 711
		// (get) Token: 0x06000E55 RID: 3669 RVA: 0x0002B9A0 File Offset: 0x00029BA0
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double4 yxxx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double4(this.y, this.x, this.x, this.x);
			}
		}

		// Token: 0x170002C8 RID: 712
		// (get) Token: 0x06000E56 RID: 3670 RVA: 0x0002B9BF File Offset: 0x00029BBF
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double4 yxxy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double4(this.y, this.x, this.x, this.y);
			}
		}

		// Token: 0x170002C9 RID: 713
		// (get) Token: 0x06000E57 RID: 3671 RVA: 0x0002B9DE File Offset: 0x00029BDE
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double4 yxxz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double4(this.y, this.x, this.x, this.z);
			}
		}

		// Token: 0x170002CA RID: 714
		// (get) Token: 0x06000E58 RID: 3672 RVA: 0x0002B9FD File Offset: 0x00029BFD
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double4 yxxw
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double4(this.y, this.x, this.x, this.w);
			}
		}

		// Token: 0x170002CB RID: 715
		// (get) Token: 0x06000E59 RID: 3673 RVA: 0x0002BA1C File Offset: 0x00029C1C
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double4 yxyx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double4(this.y, this.x, this.y, this.x);
			}
		}

		// Token: 0x170002CC RID: 716
		// (get) Token: 0x06000E5A RID: 3674 RVA: 0x0002BA3B File Offset: 0x00029C3B
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double4 yxyy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double4(this.y, this.x, this.y, this.y);
			}
		}

		// Token: 0x170002CD RID: 717
		// (get) Token: 0x06000E5B RID: 3675 RVA: 0x0002BA5A File Offset: 0x00029C5A
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double4 yxyz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double4(this.y, this.x, this.y, this.z);
			}
		}

		// Token: 0x170002CE RID: 718
		// (get) Token: 0x06000E5C RID: 3676 RVA: 0x0002BA79 File Offset: 0x00029C79
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double4 yxyw
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double4(this.y, this.x, this.y, this.w);
			}
		}

		// Token: 0x170002CF RID: 719
		// (get) Token: 0x06000E5D RID: 3677 RVA: 0x0002BA98 File Offset: 0x00029C98
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double4 yxzx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double4(this.y, this.x, this.z, this.x);
			}
		}

		// Token: 0x170002D0 RID: 720
		// (get) Token: 0x06000E5E RID: 3678 RVA: 0x0002BAB7 File Offset: 0x00029CB7
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double4 yxzy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double4(this.y, this.x, this.z, this.y);
			}
		}

		// Token: 0x170002D1 RID: 721
		// (get) Token: 0x06000E5F RID: 3679 RVA: 0x0002BAD6 File Offset: 0x00029CD6
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double4 yxzz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double4(this.y, this.x, this.z, this.z);
			}
		}

		// Token: 0x170002D2 RID: 722
		// (get) Token: 0x06000E60 RID: 3680 RVA: 0x0002BAF5 File Offset: 0x00029CF5
		// (set) Token: 0x06000E61 RID: 3681 RVA: 0x0002BB14 File Offset: 0x00029D14
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double4 yxzw
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double4(this.y, this.x, this.z, this.w);
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

		// Token: 0x170002D3 RID: 723
		// (get) Token: 0x06000E62 RID: 3682 RVA: 0x0002BB46 File Offset: 0x00029D46
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double4 yxwx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double4(this.y, this.x, this.w, this.x);
			}
		}

		// Token: 0x170002D4 RID: 724
		// (get) Token: 0x06000E63 RID: 3683 RVA: 0x0002BB65 File Offset: 0x00029D65
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double4 yxwy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double4(this.y, this.x, this.w, this.y);
			}
		}

		// Token: 0x170002D5 RID: 725
		// (get) Token: 0x06000E64 RID: 3684 RVA: 0x0002BB84 File Offset: 0x00029D84
		// (set) Token: 0x06000E65 RID: 3685 RVA: 0x0002BBA3 File Offset: 0x00029DA3
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double4 yxwz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double4(this.y, this.x, this.w, this.z);
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

		// Token: 0x170002D6 RID: 726
		// (get) Token: 0x06000E66 RID: 3686 RVA: 0x0002BBD5 File Offset: 0x00029DD5
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double4 yxww
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double4(this.y, this.x, this.w, this.w);
			}
		}

		// Token: 0x170002D7 RID: 727
		// (get) Token: 0x06000E67 RID: 3687 RVA: 0x0002BBF4 File Offset: 0x00029DF4
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double4 yyxx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double4(this.y, this.y, this.x, this.x);
			}
		}

		// Token: 0x170002D8 RID: 728
		// (get) Token: 0x06000E68 RID: 3688 RVA: 0x0002BC13 File Offset: 0x00029E13
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double4 yyxy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double4(this.y, this.y, this.x, this.y);
			}
		}

		// Token: 0x170002D9 RID: 729
		// (get) Token: 0x06000E69 RID: 3689 RVA: 0x0002BC32 File Offset: 0x00029E32
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double4 yyxz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double4(this.y, this.y, this.x, this.z);
			}
		}

		// Token: 0x170002DA RID: 730
		// (get) Token: 0x06000E6A RID: 3690 RVA: 0x0002BC51 File Offset: 0x00029E51
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double4 yyxw
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double4(this.y, this.y, this.x, this.w);
			}
		}

		// Token: 0x170002DB RID: 731
		// (get) Token: 0x06000E6B RID: 3691 RVA: 0x0002BC70 File Offset: 0x00029E70
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double4 yyyx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double4(this.y, this.y, this.y, this.x);
			}
		}

		// Token: 0x170002DC RID: 732
		// (get) Token: 0x06000E6C RID: 3692 RVA: 0x0002BC8F File Offset: 0x00029E8F
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double4 yyyy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double4(this.y, this.y, this.y, this.y);
			}
		}

		// Token: 0x170002DD RID: 733
		// (get) Token: 0x06000E6D RID: 3693 RVA: 0x0002BCAE File Offset: 0x00029EAE
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double4 yyyz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double4(this.y, this.y, this.y, this.z);
			}
		}

		// Token: 0x170002DE RID: 734
		// (get) Token: 0x06000E6E RID: 3694 RVA: 0x0002BCCD File Offset: 0x00029ECD
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double4 yyyw
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double4(this.y, this.y, this.y, this.w);
			}
		}

		// Token: 0x170002DF RID: 735
		// (get) Token: 0x06000E6F RID: 3695 RVA: 0x0002BCEC File Offset: 0x00029EEC
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double4 yyzx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double4(this.y, this.y, this.z, this.x);
			}
		}

		// Token: 0x170002E0 RID: 736
		// (get) Token: 0x06000E70 RID: 3696 RVA: 0x0002BD0B File Offset: 0x00029F0B
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double4 yyzy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double4(this.y, this.y, this.z, this.y);
			}
		}

		// Token: 0x170002E1 RID: 737
		// (get) Token: 0x06000E71 RID: 3697 RVA: 0x0002BD2A File Offset: 0x00029F2A
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double4 yyzz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double4(this.y, this.y, this.z, this.z);
			}
		}

		// Token: 0x170002E2 RID: 738
		// (get) Token: 0x06000E72 RID: 3698 RVA: 0x0002BD49 File Offset: 0x00029F49
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double4 yyzw
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double4(this.y, this.y, this.z, this.w);
			}
		}

		// Token: 0x170002E3 RID: 739
		// (get) Token: 0x06000E73 RID: 3699 RVA: 0x0002BD68 File Offset: 0x00029F68
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double4 yywx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double4(this.y, this.y, this.w, this.x);
			}
		}

		// Token: 0x170002E4 RID: 740
		// (get) Token: 0x06000E74 RID: 3700 RVA: 0x0002BD87 File Offset: 0x00029F87
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double4 yywy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double4(this.y, this.y, this.w, this.y);
			}
		}

		// Token: 0x170002E5 RID: 741
		// (get) Token: 0x06000E75 RID: 3701 RVA: 0x0002BDA6 File Offset: 0x00029FA6
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double4 yywz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double4(this.y, this.y, this.w, this.z);
			}
		}

		// Token: 0x170002E6 RID: 742
		// (get) Token: 0x06000E76 RID: 3702 RVA: 0x0002BDC5 File Offset: 0x00029FC5
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double4 yyww
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double4(this.y, this.y, this.w, this.w);
			}
		}

		// Token: 0x170002E7 RID: 743
		// (get) Token: 0x06000E77 RID: 3703 RVA: 0x0002BDE4 File Offset: 0x00029FE4
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double4 yzxx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double4(this.y, this.z, this.x, this.x);
			}
		}

		// Token: 0x170002E8 RID: 744
		// (get) Token: 0x06000E78 RID: 3704 RVA: 0x0002BE03 File Offset: 0x0002A003
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double4 yzxy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double4(this.y, this.z, this.x, this.y);
			}
		}

		// Token: 0x170002E9 RID: 745
		// (get) Token: 0x06000E79 RID: 3705 RVA: 0x0002BE22 File Offset: 0x0002A022
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double4 yzxz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double4(this.y, this.z, this.x, this.z);
			}
		}

		// Token: 0x170002EA RID: 746
		// (get) Token: 0x06000E7A RID: 3706 RVA: 0x0002BE41 File Offset: 0x0002A041
		// (set) Token: 0x06000E7B RID: 3707 RVA: 0x0002BE60 File Offset: 0x0002A060
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double4 yzxw
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double4(this.y, this.z, this.x, this.w);
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

		// Token: 0x170002EB RID: 747
		// (get) Token: 0x06000E7C RID: 3708 RVA: 0x0002BE92 File Offset: 0x0002A092
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double4 yzyx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double4(this.y, this.z, this.y, this.x);
			}
		}

		// Token: 0x170002EC RID: 748
		// (get) Token: 0x06000E7D RID: 3709 RVA: 0x0002BEB1 File Offset: 0x0002A0B1
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double4 yzyy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double4(this.y, this.z, this.y, this.y);
			}
		}

		// Token: 0x170002ED RID: 749
		// (get) Token: 0x06000E7E RID: 3710 RVA: 0x0002BED0 File Offset: 0x0002A0D0
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double4 yzyz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double4(this.y, this.z, this.y, this.z);
			}
		}

		// Token: 0x170002EE RID: 750
		// (get) Token: 0x06000E7F RID: 3711 RVA: 0x0002BEEF File Offset: 0x0002A0EF
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double4 yzyw
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double4(this.y, this.z, this.y, this.w);
			}
		}

		// Token: 0x170002EF RID: 751
		// (get) Token: 0x06000E80 RID: 3712 RVA: 0x0002BF0E File Offset: 0x0002A10E
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double4 yzzx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double4(this.y, this.z, this.z, this.x);
			}
		}

		// Token: 0x170002F0 RID: 752
		// (get) Token: 0x06000E81 RID: 3713 RVA: 0x0002BF2D File Offset: 0x0002A12D
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double4 yzzy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double4(this.y, this.z, this.z, this.y);
			}
		}

		// Token: 0x170002F1 RID: 753
		// (get) Token: 0x06000E82 RID: 3714 RVA: 0x0002BF4C File Offset: 0x0002A14C
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double4 yzzz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double4(this.y, this.z, this.z, this.z);
			}
		}

		// Token: 0x170002F2 RID: 754
		// (get) Token: 0x06000E83 RID: 3715 RVA: 0x0002BF6B File Offset: 0x0002A16B
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double4 yzzw
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double4(this.y, this.z, this.z, this.w);
			}
		}

		// Token: 0x170002F3 RID: 755
		// (get) Token: 0x06000E84 RID: 3716 RVA: 0x0002BF8A File Offset: 0x0002A18A
		// (set) Token: 0x06000E85 RID: 3717 RVA: 0x0002BFA9 File Offset: 0x0002A1A9
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double4 yzwx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double4(this.y, this.z, this.w, this.x);
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

		// Token: 0x170002F4 RID: 756
		// (get) Token: 0x06000E86 RID: 3718 RVA: 0x0002BFDB File Offset: 0x0002A1DB
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double4 yzwy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double4(this.y, this.z, this.w, this.y);
			}
		}

		// Token: 0x170002F5 RID: 757
		// (get) Token: 0x06000E87 RID: 3719 RVA: 0x0002BFFA File Offset: 0x0002A1FA
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double4 yzwz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double4(this.y, this.z, this.w, this.z);
			}
		}

		// Token: 0x170002F6 RID: 758
		// (get) Token: 0x06000E88 RID: 3720 RVA: 0x0002C019 File Offset: 0x0002A219
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double4 yzww
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double4(this.y, this.z, this.w, this.w);
			}
		}

		// Token: 0x170002F7 RID: 759
		// (get) Token: 0x06000E89 RID: 3721 RVA: 0x0002C038 File Offset: 0x0002A238
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double4 ywxx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double4(this.y, this.w, this.x, this.x);
			}
		}

		// Token: 0x170002F8 RID: 760
		// (get) Token: 0x06000E8A RID: 3722 RVA: 0x0002C057 File Offset: 0x0002A257
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double4 ywxy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double4(this.y, this.w, this.x, this.y);
			}
		}

		// Token: 0x170002F9 RID: 761
		// (get) Token: 0x06000E8B RID: 3723 RVA: 0x0002C076 File Offset: 0x0002A276
		// (set) Token: 0x06000E8C RID: 3724 RVA: 0x0002C095 File Offset: 0x0002A295
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double4 ywxz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double4(this.y, this.w, this.x, this.z);
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

		// Token: 0x170002FA RID: 762
		// (get) Token: 0x06000E8D RID: 3725 RVA: 0x0002C0C7 File Offset: 0x0002A2C7
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double4 ywxw
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double4(this.y, this.w, this.x, this.w);
			}
		}

		// Token: 0x170002FB RID: 763
		// (get) Token: 0x06000E8E RID: 3726 RVA: 0x0002C0E6 File Offset: 0x0002A2E6
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double4 ywyx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double4(this.y, this.w, this.y, this.x);
			}
		}

		// Token: 0x170002FC RID: 764
		// (get) Token: 0x06000E8F RID: 3727 RVA: 0x0002C105 File Offset: 0x0002A305
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double4 ywyy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double4(this.y, this.w, this.y, this.y);
			}
		}

		// Token: 0x170002FD RID: 765
		// (get) Token: 0x06000E90 RID: 3728 RVA: 0x0002C124 File Offset: 0x0002A324
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double4 ywyz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double4(this.y, this.w, this.y, this.z);
			}
		}

		// Token: 0x170002FE RID: 766
		// (get) Token: 0x06000E91 RID: 3729 RVA: 0x0002C143 File Offset: 0x0002A343
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double4 ywyw
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double4(this.y, this.w, this.y, this.w);
			}
		}

		// Token: 0x170002FF RID: 767
		// (get) Token: 0x06000E92 RID: 3730 RVA: 0x0002C162 File Offset: 0x0002A362
		// (set) Token: 0x06000E93 RID: 3731 RVA: 0x0002C181 File Offset: 0x0002A381
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double4 ywzx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double4(this.y, this.w, this.z, this.x);
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

		// Token: 0x17000300 RID: 768
		// (get) Token: 0x06000E94 RID: 3732 RVA: 0x0002C1B3 File Offset: 0x0002A3B3
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double4 ywzy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double4(this.y, this.w, this.z, this.y);
			}
		}

		// Token: 0x17000301 RID: 769
		// (get) Token: 0x06000E95 RID: 3733 RVA: 0x0002C1D2 File Offset: 0x0002A3D2
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double4 ywzz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double4(this.y, this.w, this.z, this.z);
			}
		}

		// Token: 0x17000302 RID: 770
		// (get) Token: 0x06000E96 RID: 3734 RVA: 0x0002C1F1 File Offset: 0x0002A3F1
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double4 ywzw
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double4(this.y, this.w, this.z, this.w);
			}
		}

		// Token: 0x17000303 RID: 771
		// (get) Token: 0x06000E97 RID: 3735 RVA: 0x0002C210 File Offset: 0x0002A410
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double4 ywwx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double4(this.y, this.w, this.w, this.x);
			}
		}

		// Token: 0x17000304 RID: 772
		// (get) Token: 0x06000E98 RID: 3736 RVA: 0x0002C22F File Offset: 0x0002A42F
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double4 ywwy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double4(this.y, this.w, this.w, this.y);
			}
		}

		// Token: 0x17000305 RID: 773
		// (get) Token: 0x06000E99 RID: 3737 RVA: 0x0002C24E File Offset: 0x0002A44E
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double4 ywwz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double4(this.y, this.w, this.w, this.z);
			}
		}

		// Token: 0x17000306 RID: 774
		// (get) Token: 0x06000E9A RID: 3738 RVA: 0x0002C26D File Offset: 0x0002A46D
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double4 ywww
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double4(this.y, this.w, this.w, this.w);
			}
		}

		// Token: 0x17000307 RID: 775
		// (get) Token: 0x06000E9B RID: 3739 RVA: 0x0002C28C File Offset: 0x0002A48C
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double4 zxxx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double4(this.z, this.x, this.x, this.x);
			}
		}

		// Token: 0x17000308 RID: 776
		// (get) Token: 0x06000E9C RID: 3740 RVA: 0x0002C2AB File Offset: 0x0002A4AB
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double4 zxxy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double4(this.z, this.x, this.x, this.y);
			}
		}

		// Token: 0x17000309 RID: 777
		// (get) Token: 0x06000E9D RID: 3741 RVA: 0x0002C2CA File Offset: 0x0002A4CA
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double4 zxxz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double4(this.z, this.x, this.x, this.z);
			}
		}

		// Token: 0x1700030A RID: 778
		// (get) Token: 0x06000E9E RID: 3742 RVA: 0x0002C2E9 File Offset: 0x0002A4E9
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double4 zxxw
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double4(this.z, this.x, this.x, this.w);
			}
		}

		// Token: 0x1700030B RID: 779
		// (get) Token: 0x06000E9F RID: 3743 RVA: 0x0002C308 File Offset: 0x0002A508
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double4 zxyx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double4(this.z, this.x, this.y, this.x);
			}
		}

		// Token: 0x1700030C RID: 780
		// (get) Token: 0x06000EA0 RID: 3744 RVA: 0x0002C327 File Offset: 0x0002A527
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double4 zxyy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double4(this.z, this.x, this.y, this.y);
			}
		}

		// Token: 0x1700030D RID: 781
		// (get) Token: 0x06000EA1 RID: 3745 RVA: 0x0002C346 File Offset: 0x0002A546
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double4 zxyz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double4(this.z, this.x, this.y, this.z);
			}
		}

		// Token: 0x1700030E RID: 782
		// (get) Token: 0x06000EA2 RID: 3746 RVA: 0x0002C365 File Offset: 0x0002A565
		// (set) Token: 0x06000EA3 RID: 3747 RVA: 0x0002C384 File Offset: 0x0002A584
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double4 zxyw
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double4(this.z, this.x, this.y, this.w);
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

		// Token: 0x1700030F RID: 783
		// (get) Token: 0x06000EA4 RID: 3748 RVA: 0x0002C3B6 File Offset: 0x0002A5B6
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double4 zxzx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double4(this.z, this.x, this.z, this.x);
			}
		}

		// Token: 0x17000310 RID: 784
		// (get) Token: 0x06000EA5 RID: 3749 RVA: 0x0002C3D5 File Offset: 0x0002A5D5
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double4 zxzy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double4(this.z, this.x, this.z, this.y);
			}
		}

		// Token: 0x17000311 RID: 785
		// (get) Token: 0x06000EA6 RID: 3750 RVA: 0x0002C3F4 File Offset: 0x0002A5F4
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double4 zxzz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double4(this.z, this.x, this.z, this.z);
			}
		}

		// Token: 0x17000312 RID: 786
		// (get) Token: 0x06000EA7 RID: 3751 RVA: 0x0002C413 File Offset: 0x0002A613
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double4 zxzw
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double4(this.z, this.x, this.z, this.w);
			}
		}

		// Token: 0x17000313 RID: 787
		// (get) Token: 0x06000EA8 RID: 3752 RVA: 0x0002C432 File Offset: 0x0002A632
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double4 zxwx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double4(this.z, this.x, this.w, this.x);
			}
		}

		// Token: 0x17000314 RID: 788
		// (get) Token: 0x06000EA9 RID: 3753 RVA: 0x0002C451 File Offset: 0x0002A651
		// (set) Token: 0x06000EAA RID: 3754 RVA: 0x0002C470 File Offset: 0x0002A670
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double4 zxwy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double4(this.z, this.x, this.w, this.y);
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

		// Token: 0x17000315 RID: 789
		// (get) Token: 0x06000EAB RID: 3755 RVA: 0x0002C4A2 File Offset: 0x0002A6A2
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double4 zxwz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double4(this.z, this.x, this.w, this.z);
			}
		}

		// Token: 0x17000316 RID: 790
		// (get) Token: 0x06000EAC RID: 3756 RVA: 0x0002C4C1 File Offset: 0x0002A6C1
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double4 zxww
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double4(this.z, this.x, this.w, this.w);
			}
		}

		// Token: 0x17000317 RID: 791
		// (get) Token: 0x06000EAD RID: 3757 RVA: 0x0002C4E0 File Offset: 0x0002A6E0
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double4 zyxx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double4(this.z, this.y, this.x, this.x);
			}
		}

		// Token: 0x17000318 RID: 792
		// (get) Token: 0x06000EAE RID: 3758 RVA: 0x0002C4FF File Offset: 0x0002A6FF
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double4 zyxy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double4(this.z, this.y, this.x, this.y);
			}
		}

		// Token: 0x17000319 RID: 793
		// (get) Token: 0x06000EAF RID: 3759 RVA: 0x0002C51E File Offset: 0x0002A71E
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double4 zyxz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double4(this.z, this.y, this.x, this.z);
			}
		}

		// Token: 0x1700031A RID: 794
		// (get) Token: 0x06000EB0 RID: 3760 RVA: 0x0002C53D File Offset: 0x0002A73D
		// (set) Token: 0x06000EB1 RID: 3761 RVA: 0x0002C55C File Offset: 0x0002A75C
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double4 zyxw
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double4(this.z, this.y, this.x, this.w);
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

		// Token: 0x1700031B RID: 795
		// (get) Token: 0x06000EB2 RID: 3762 RVA: 0x0002C58E File Offset: 0x0002A78E
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double4 zyyx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double4(this.z, this.y, this.y, this.x);
			}
		}

		// Token: 0x1700031C RID: 796
		// (get) Token: 0x06000EB3 RID: 3763 RVA: 0x0002C5AD File Offset: 0x0002A7AD
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double4 zyyy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double4(this.z, this.y, this.y, this.y);
			}
		}

		// Token: 0x1700031D RID: 797
		// (get) Token: 0x06000EB4 RID: 3764 RVA: 0x0002C5CC File Offset: 0x0002A7CC
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double4 zyyz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double4(this.z, this.y, this.y, this.z);
			}
		}

		// Token: 0x1700031E RID: 798
		// (get) Token: 0x06000EB5 RID: 3765 RVA: 0x0002C5EB File Offset: 0x0002A7EB
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double4 zyyw
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double4(this.z, this.y, this.y, this.w);
			}
		}

		// Token: 0x1700031F RID: 799
		// (get) Token: 0x06000EB6 RID: 3766 RVA: 0x0002C60A File Offset: 0x0002A80A
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double4 zyzx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double4(this.z, this.y, this.z, this.x);
			}
		}

		// Token: 0x17000320 RID: 800
		// (get) Token: 0x06000EB7 RID: 3767 RVA: 0x0002C629 File Offset: 0x0002A829
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double4 zyzy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double4(this.z, this.y, this.z, this.y);
			}
		}

		// Token: 0x17000321 RID: 801
		// (get) Token: 0x06000EB8 RID: 3768 RVA: 0x0002C648 File Offset: 0x0002A848
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double4 zyzz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double4(this.z, this.y, this.z, this.z);
			}
		}

		// Token: 0x17000322 RID: 802
		// (get) Token: 0x06000EB9 RID: 3769 RVA: 0x0002C667 File Offset: 0x0002A867
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double4 zyzw
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double4(this.z, this.y, this.z, this.w);
			}
		}

		// Token: 0x17000323 RID: 803
		// (get) Token: 0x06000EBA RID: 3770 RVA: 0x0002C686 File Offset: 0x0002A886
		// (set) Token: 0x06000EBB RID: 3771 RVA: 0x0002C6A5 File Offset: 0x0002A8A5
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double4 zywx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double4(this.z, this.y, this.w, this.x);
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

		// Token: 0x17000324 RID: 804
		// (get) Token: 0x06000EBC RID: 3772 RVA: 0x0002C6D7 File Offset: 0x0002A8D7
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double4 zywy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double4(this.z, this.y, this.w, this.y);
			}
		}

		// Token: 0x17000325 RID: 805
		// (get) Token: 0x06000EBD RID: 3773 RVA: 0x0002C6F6 File Offset: 0x0002A8F6
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double4 zywz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double4(this.z, this.y, this.w, this.z);
			}
		}

		// Token: 0x17000326 RID: 806
		// (get) Token: 0x06000EBE RID: 3774 RVA: 0x0002C715 File Offset: 0x0002A915
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double4 zyww
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double4(this.z, this.y, this.w, this.w);
			}
		}

		// Token: 0x17000327 RID: 807
		// (get) Token: 0x06000EBF RID: 3775 RVA: 0x0002C734 File Offset: 0x0002A934
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double4 zzxx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double4(this.z, this.z, this.x, this.x);
			}
		}

		// Token: 0x17000328 RID: 808
		// (get) Token: 0x06000EC0 RID: 3776 RVA: 0x0002C753 File Offset: 0x0002A953
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double4 zzxy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double4(this.z, this.z, this.x, this.y);
			}
		}

		// Token: 0x17000329 RID: 809
		// (get) Token: 0x06000EC1 RID: 3777 RVA: 0x0002C772 File Offset: 0x0002A972
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double4 zzxz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double4(this.z, this.z, this.x, this.z);
			}
		}

		// Token: 0x1700032A RID: 810
		// (get) Token: 0x06000EC2 RID: 3778 RVA: 0x0002C791 File Offset: 0x0002A991
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double4 zzxw
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double4(this.z, this.z, this.x, this.w);
			}
		}

		// Token: 0x1700032B RID: 811
		// (get) Token: 0x06000EC3 RID: 3779 RVA: 0x0002C7B0 File Offset: 0x0002A9B0
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double4 zzyx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double4(this.z, this.z, this.y, this.x);
			}
		}

		// Token: 0x1700032C RID: 812
		// (get) Token: 0x06000EC4 RID: 3780 RVA: 0x0002C7CF File Offset: 0x0002A9CF
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double4 zzyy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double4(this.z, this.z, this.y, this.y);
			}
		}

		// Token: 0x1700032D RID: 813
		// (get) Token: 0x06000EC5 RID: 3781 RVA: 0x0002C7EE File Offset: 0x0002A9EE
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double4 zzyz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double4(this.z, this.z, this.y, this.z);
			}
		}

		// Token: 0x1700032E RID: 814
		// (get) Token: 0x06000EC6 RID: 3782 RVA: 0x0002C80D File Offset: 0x0002AA0D
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double4 zzyw
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double4(this.z, this.z, this.y, this.w);
			}
		}

		// Token: 0x1700032F RID: 815
		// (get) Token: 0x06000EC7 RID: 3783 RVA: 0x0002C82C File Offset: 0x0002AA2C
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double4 zzzx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double4(this.z, this.z, this.z, this.x);
			}
		}

		// Token: 0x17000330 RID: 816
		// (get) Token: 0x06000EC8 RID: 3784 RVA: 0x0002C84B File Offset: 0x0002AA4B
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double4 zzzy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double4(this.z, this.z, this.z, this.y);
			}
		}

		// Token: 0x17000331 RID: 817
		// (get) Token: 0x06000EC9 RID: 3785 RVA: 0x0002C86A File Offset: 0x0002AA6A
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double4 zzzz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double4(this.z, this.z, this.z, this.z);
			}
		}

		// Token: 0x17000332 RID: 818
		// (get) Token: 0x06000ECA RID: 3786 RVA: 0x0002C889 File Offset: 0x0002AA89
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double4 zzzw
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double4(this.z, this.z, this.z, this.w);
			}
		}

		// Token: 0x17000333 RID: 819
		// (get) Token: 0x06000ECB RID: 3787 RVA: 0x0002C8A8 File Offset: 0x0002AAA8
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double4 zzwx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double4(this.z, this.z, this.w, this.x);
			}
		}

		// Token: 0x17000334 RID: 820
		// (get) Token: 0x06000ECC RID: 3788 RVA: 0x0002C8C7 File Offset: 0x0002AAC7
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double4 zzwy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double4(this.z, this.z, this.w, this.y);
			}
		}

		// Token: 0x17000335 RID: 821
		// (get) Token: 0x06000ECD RID: 3789 RVA: 0x0002C8E6 File Offset: 0x0002AAE6
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double4 zzwz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double4(this.z, this.z, this.w, this.z);
			}
		}

		// Token: 0x17000336 RID: 822
		// (get) Token: 0x06000ECE RID: 3790 RVA: 0x0002C905 File Offset: 0x0002AB05
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double4 zzww
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double4(this.z, this.z, this.w, this.w);
			}
		}

		// Token: 0x17000337 RID: 823
		// (get) Token: 0x06000ECF RID: 3791 RVA: 0x0002C924 File Offset: 0x0002AB24
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double4 zwxx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double4(this.z, this.w, this.x, this.x);
			}
		}

		// Token: 0x17000338 RID: 824
		// (get) Token: 0x06000ED0 RID: 3792 RVA: 0x0002C943 File Offset: 0x0002AB43
		// (set) Token: 0x06000ED1 RID: 3793 RVA: 0x0002C962 File Offset: 0x0002AB62
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double4 zwxy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double4(this.z, this.w, this.x, this.y);
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

		// Token: 0x17000339 RID: 825
		// (get) Token: 0x06000ED2 RID: 3794 RVA: 0x0002C994 File Offset: 0x0002AB94
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double4 zwxz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double4(this.z, this.w, this.x, this.z);
			}
		}

		// Token: 0x1700033A RID: 826
		// (get) Token: 0x06000ED3 RID: 3795 RVA: 0x0002C9B3 File Offset: 0x0002ABB3
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double4 zwxw
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double4(this.z, this.w, this.x, this.w);
			}
		}

		// Token: 0x1700033B RID: 827
		// (get) Token: 0x06000ED4 RID: 3796 RVA: 0x0002C9D2 File Offset: 0x0002ABD2
		// (set) Token: 0x06000ED5 RID: 3797 RVA: 0x0002C9F1 File Offset: 0x0002ABF1
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double4 zwyx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double4(this.z, this.w, this.y, this.x);
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

		// Token: 0x1700033C RID: 828
		// (get) Token: 0x06000ED6 RID: 3798 RVA: 0x0002CA23 File Offset: 0x0002AC23
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double4 zwyy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double4(this.z, this.w, this.y, this.y);
			}
		}

		// Token: 0x1700033D RID: 829
		// (get) Token: 0x06000ED7 RID: 3799 RVA: 0x0002CA42 File Offset: 0x0002AC42
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double4 zwyz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double4(this.z, this.w, this.y, this.z);
			}
		}

		// Token: 0x1700033E RID: 830
		// (get) Token: 0x06000ED8 RID: 3800 RVA: 0x0002CA61 File Offset: 0x0002AC61
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double4 zwyw
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double4(this.z, this.w, this.y, this.w);
			}
		}

		// Token: 0x1700033F RID: 831
		// (get) Token: 0x06000ED9 RID: 3801 RVA: 0x0002CA80 File Offset: 0x0002AC80
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double4 zwzx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double4(this.z, this.w, this.z, this.x);
			}
		}

		// Token: 0x17000340 RID: 832
		// (get) Token: 0x06000EDA RID: 3802 RVA: 0x0002CA9F File Offset: 0x0002AC9F
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double4 zwzy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double4(this.z, this.w, this.z, this.y);
			}
		}

		// Token: 0x17000341 RID: 833
		// (get) Token: 0x06000EDB RID: 3803 RVA: 0x0002CABE File Offset: 0x0002ACBE
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double4 zwzz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double4(this.z, this.w, this.z, this.z);
			}
		}

		// Token: 0x17000342 RID: 834
		// (get) Token: 0x06000EDC RID: 3804 RVA: 0x0002CADD File Offset: 0x0002ACDD
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double4 zwzw
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double4(this.z, this.w, this.z, this.w);
			}
		}

		// Token: 0x17000343 RID: 835
		// (get) Token: 0x06000EDD RID: 3805 RVA: 0x0002CAFC File Offset: 0x0002ACFC
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double4 zwwx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double4(this.z, this.w, this.w, this.x);
			}
		}

		// Token: 0x17000344 RID: 836
		// (get) Token: 0x06000EDE RID: 3806 RVA: 0x0002CB1B File Offset: 0x0002AD1B
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double4 zwwy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double4(this.z, this.w, this.w, this.y);
			}
		}

		// Token: 0x17000345 RID: 837
		// (get) Token: 0x06000EDF RID: 3807 RVA: 0x0002CB3A File Offset: 0x0002AD3A
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double4 zwwz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double4(this.z, this.w, this.w, this.z);
			}
		}

		// Token: 0x17000346 RID: 838
		// (get) Token: 0x06000EE0 RID: 3808 RVA: 0x0002CB59 File Offset: 0x0002AD59
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double4 zwww
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double4(this.z, this.w, this.w, this.w);
			}
		}

		// Token: 0x17000347 RID: 839
		// (get) Token: 0x06000EE1 RID: 3809 RVA: 0x0002CB78 File Offset: 0x0002AD78
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double4 wxxx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double4(this.w, this.x, this.x, this.x);
			}
		}

		// Token: 0x17000348 RID: 840
		// (get) Token: 0x06000EE2 RID: 3810 RVA: 0x0002CB97 File Offset: 0x0002AD97
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double4 wxxy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double4(this.w, this.x, this.x, this.y);
			}
		}

		// Token: 0x17000349 RID: 841
		// (get) Token: 0x06000EE3 RID: 3811 RVA: 0x0002CBB6 File Offset: 0x0002ADB6
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double4 wxxz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double4(this.w, this.x, this.x, this.z);
			}
		}

		// Token: 0x1700034A RID: 842
		// (get) Token: 0x06000EE4 RID: 3812 RVA: 0x0002CBD5 File Offset: 0x0002ADD5
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double4 wxxw
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double4(this.w, this.x, this.x, this.w);
			}
		}

		// Token: 0x1700034B RID: 843
		// (get) Token: 0x06000EE5 RID: 3813 RVA: 0x0002CBF4 File Offset: 0x0002ADF4
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double4 wxyx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double4(this.w, this.x, this.y, this.x);
			}
		}

		// Token: 0x1700034C RID: 844
		// (get) Token: 0x06000EE6 RID: 3814 RVA: 0x0002CC13 File Offset: 0x0002AE13
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double4 wxyy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double4(this.w, this.x, this.y, this.y);
			}
		}

		// Token: 0x1700034D RID: 845
		// (get) Token: 0x06000EE7 RID: 3815 RVA: 0x0002CC32 File Offset: 0x0002AE32
		// (set) Token: 0x06000EE8 RID: 3816 RVA: 0x0002CC51 File Offset: 0x0002AE51
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double4 wxyz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double4(this.w, this.x, this.y, this.z);
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

		// Token: 0x1700034E RID: 846
		// (get) Token: 0x06000EE9 RID: 3817 RVA: 0x0002CC83 File Offset: 0x0002AE83
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double4 wxyw
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double4(this.w, this.x, this.y, this.w);
			}
		}

		// Token: 0x1700034F RID: 847
		// (get) Token: 0x06000EEA RID: 3818 RVA: 0x0002CCA2 File Offset: 0x0002AEA2
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double4 wxzx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double4(this.w, this.x, this.z, this.x);
			}
		}

		// Token: 0x17000350 RID: 848
		// (get) Token: 0x06000EEB RID: 3819 RVA: 0x0002CCC1 File Offset: 0x0002AEC1
		// (set) Token: 0x06000EEC RID: 3820 RVA: 0x0002CCE0 File Offset: 0x0002AEE0
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double4 wxzy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double4(this.w, this.x, this.z, this.y);
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

		// Token: 0x17000351 RID: 849
		// (get) Token: 0x06000EED RID: 3821 RVA: 0x0002CD12 File Offset: 0x0002AF12
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double4 wxzz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double4(this.w, this.x, this.z, this.z);
			}
		}

		// Token: 0x17000352 RID: 850
		// (get) Token: 0x06000EEE RID: 3822 RVA: 0x0002CD31 File Offset: 0x0002AF31
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double4 wxzw
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double4(this.w, this.x, this.z, this.w);
			}
		}

		// Token: 0x17000353 RID: 851
		// (get) Token: 0x06000EEF RID: 3823 RVA: 0x0002CD50 File Offset: 0x0002AF50
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double4 wxwx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double4(this.w, this.x, this.w, this.x);
			}
		}

		// Token: 0x17000354 RID: 852
		// (get) Token: 0x06000EF0 RID: 3824 RVA: 0x0002CD6F File Offset: 0x0002AF6F
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double4 wxwy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double4(this.w, this.x, this.w, this.y);
			}
		}

		// Token: 0x17000355 RID: 853
		// (get) Token: 0x06000EF1 RID: 3825 RVA: 0x0002CD8E File Offset: 0x0002AF8E
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double4 wxwz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double4(this.w, this.x, this.w, this.z);
			}
		}

		// Token: 0x17000356 RID: 854
		// (get) Token: 0x06000EF2 RID: 3826 RVA: 0x0002CDAD File Offset: 0x0002AFAD
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double4 wxww
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double4(this.w, this.x, this.w, this.w);
			}
		}

		// Token: 0x17000357 RID: 855
		// (get) Token: 0x06000EF3 RID: 3827 RVA: 0x0002CDCC File Offset: 0x0002AFCC
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double4 wyxx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double4(this.w, this.y, this.x, this.x);
			}
		}

		// Token: 0x17000358 RID: 856
		// (get) Token: 0x06000EF4 RID: 3828 RVA: 0x0002CDEB File Offset: 0x0002AFEB
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double4 wyxy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double4(this.w, this.y, this.x, this.y);
			}
		}

		// Token: 0x17000359 RID: 857
		// (get) Token: 0x06000EF5 RID: 3829 RVA: 0x0002CE0A File Offset: 0x0002B00A
		// (set) Token: 0x06000EF6 RID: 3830 RVA: 0x0002CE29 File Offset: 0x0002B029
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double4 wyxz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double4(this.w, this.y, this.x, this.z);
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

		// Token: 0x1700035A RID: 858
		// (get) Token: 0x06000EF7 RID: 3831 RVA: 0x0002CE5B File Offset: 0x0002B05B
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double4 wyxw
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double4(this.w, this.y, this.x, this.w);
			}
		}

		// Token: 0x1700035B RID: 859
		// (get) Token: 0x06000EF8 RID: 3832 RVA: 0x0002CE7A File Offset: 0x0002B07A
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double4 wyyx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double4(this.w, this.y, this.y, this.x);
			}
		}

		// Token: 0x1700035C RID: 860
		// (get) Token: 0x06000EF9 RID: 3833 RVA: 0x0002CE99 File Offset: 0x0002B099
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double4 wyyy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double4(this.w, this.y, this.y, this.y);
			}
		}

		// Token: 0x1700035D RID: 861
		// (get) Token: 0x06000EFA RID: 3834 RVA: 0x0002CEB8 File Offset: 0x0002B0B8
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double4 wyyz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double4(this.w, this.y, this.y, this.z);
			}
		}

		// Token: 0x1700035E RID: 862
		// (get) Token: 0x06000EFB RID: 3835 RVA: 0x0002CED7 File Offset: 0x0002B0D7
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double4 wyyw
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double4(this.w, this.y, this.y, this.w);
			}
		}

		// Token: 0x1700035F RID: 863
		// (get) Token: 0x06000EFC RID: 3836 RVA: 0x0002CEF6 File Offset: 0x0002B0F6
		// (set) Token: 0x06000EFD RID: 3837 RVA: 0x0002CF15 File Offset: 0x0002B115
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double4 wyzx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double4(this.w, this.y, this.z, this.x);
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

		// Token: 0x17000360 RID: 864
		// (get) Token: 0x06000EFE RID: 3838 RVA: 0x0002CF47 File Offset: 0x0002B147
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double4 wyzy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double4(this.w, this.y, this.z, this.y);
			}
		}

		// Token: 0x17000361 RID: 865
		// (get) Token: 0x06000EFF RID: 3839 RVA: 0x0002CF66 File Offset: 0x0002B166
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double4 wyzz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double4(this.w, this.y, this.z, this.z);
			}
		}

		// Token: 0x17000362 RID: 866
		// (get) Token: 0x06000F00 RID: 3840 RVA: 0x0002CF85 File Offset: 0x0002B185
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double4 wyzw
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double4(this.w, this.y, this.z, this.w);
			}
		}

		// Token: 0x17000363 RID: 867
		// (get) Token: 0x06000F01 RID: 3841 RVA: 0x0002CFA4 File Offset: 0x0002B1A4
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double4 wywx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double4(this.w, this.y, this.w, this.x);
			}
		}

		// Token: 0x17000364 RID: 868
		// (get) Token: 0x06000F02 RID: 3842 RVA: 0x0002CFC3 File Offset: 0x0002B1C3
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double4 wywy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double4(this.w, this.y, this.w, this.y);
			}
		}

		// Token: 0x17000365 RID: 869
		// (get) Token: 0x06000F03 RID: 3843 RVA: 0x0002CFE2 File Offset: 0x0002B1E2
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double4 wywz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double4(this.w, this.y, this.w, this.z);
			}
		}

		// Token: 0x17000366 RID: 870
		// (get) Token: 0x06000F04 RID: 3844 RVA: 0x0002D001 File Offset: 0x0002B201
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double4 wyww
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double4(this.w, this.y, this.w, this.w);
			}
		}

		// Token: 0x17000367 RID: 871
		// (get) Token: 0x06000F05 RID: 3845 RVA: 0x0002D020 File Offset: 0x0002B220
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double4 wzxx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double4(this.w, this.z, this.x, this.x);
			}
		}

		// Token: 0x17000368 RID: 872
		// (get) Token: 0x06000F06 RID: 3846 RVA: 0x0002D03F File Offset: 0x0002B23F
		// (set) Token: 0x06000F07 RID: 3847 RVA: 0x0002D05E File Offset: 0x0002B25E
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double4 wzxy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double4(this.w, this.z, this.x, this.y);
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

		// Token: 0x17000369 RID: 873
		// (get) Token: 0x06000F08 RID: 3848 RVA: 0x0002D090 File Offset: 0x0002B290
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double4 wzxz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double4(this.w, this.z, this.x, this.z);
			}
		}

		// Token: 0x1700036A RID: 874
		// (get) Token: 0x06000F09 RID: 3849 RVA: 0x0002D0AF File Offset: 0x0002B2AF
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double4 wzxw
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double4(this.w, this.z, this.x, this.w);
			}
		}

		// Token: 0x1700036B RID: 875
		// (get) Token: 0x06000F0A RID: 3850 RVA: 0x0002D0CE File Offset: 0x0002B2CE
		// (set) Token: 0x06000F0B RID: 3851 RVA: 0x0002D0ED File Offset: 0x0002B2ED
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double4 wzyx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double4(this.w, this.z, this.y, this.x);
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

		// Token: 0x1700036C RID: 876
		// (get) Token: 0x06000F0C RID: 3852 RVA: 0x0002D11F File Offset: 0x0002B31F
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double4 wzyy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double4(this.w, this.z, this.y, this.y);
			}
		}

		// Token: 0x1700036D RID: 877
		// (get) Token: 0x06000F0D RID: 3853 RVA: 0x0002D13E File Offset: 0x0002B33E
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double4 wzyz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double4(this.w, this.z, this.y, this.z);
			}
		}

		// Token: 0x1700036E RID: 878
		// (get) Token: 0x06000F0E RID: 3854 RVA: 0x0002D15D File Offset: 0x0002B35D
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double4 wzyw
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double4(this.w, this.z, this.y, this.w);
			}
		}

		// Token: 0x1700036F RID: 879
		// (get) Token: 0x06000F0F RID: 3855 RVA: 0x0002D17C File Offset: 0x0002B37C
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double4 wzzx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double4(this.w, this.z, this.z, this.x);
			}
		}

		// Token: 0x17000370 RID: 880
		// (get) Token: 0x06000F10 RID: 3856 RVA: 0x0002D19B File Offset: 0x0002B39B
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double4 wzzy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double4(this.w, this.z, this.z, this.y);
			}
		}

		// Token: 0x17000371 RID: 881
		// (get) Token: 0x06000F11 RID: 3857 RVA: 0x0002D1BA File Offset: 0x0002B3BA
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double4 wzzz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double4(this.w, this.z, this.z, this.z);
			}
		}

		// Token: 0x17000372 RID: 882
		// (get) Token: 0x06000F12 RID: 3858 RVA: 0x0002D1D9 File Offset: 0x0002B3D9
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double4 wzzw
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double4(this.w, this.z, this.z, this.w);
			}
		}

		// Token: 0x17000373 RID: 883
		// (get) Token: 0x06000F13 RID: 3859 RVA: 0x0002D1F8 File Offset: 0x0002B3F8
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double4 wzwx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double4(this.w, this.z, this.w, this.x);
			}
		}

		// Token: 0x17000374 RID: 884
		// (get) Token: 0x06000F14 RID: 3860 RVA: 0x0002D217 File Offset: 0x0002B417
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double4 wzwy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double4(this.w, this.z, this.w, this.y);
			}
		}

		// Token: 0x17000375 RID: 885
		// (get) Token: 0x06000F15 RID: 3861 RVA: 0x0002D236 File Offset: 0x0002B436
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double4 wzwz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double4(this.w, this.z, this.w, this.z);
			}
		}

		// Token: 0x17000376 RID: 886
		// (get) Token: 0x06000F16 RID: 3862 RVA: 0x0002D255 File Offset: 0x0002B455
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double4 wzww
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double4(this.w, this.z, this.w, this.w);
			}
		}

		// Token: 0x17000377 RID: 887
		// (get) Token: 0x06000F17 RID: 3863 RVA: 0x0002D274 File Offset: 0x0002B474
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double4 wwxx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double4(this.w, this.w, this.x, this.x);
			}
		}

		// Token: 0x17000378 RID: 888
		// (get) Token: 0x06000F18 RID: 3864 RVA: 0x0002D293 File Offset: 0x0002B493
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double4 wwxy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double4(this.w, this.w, this.x, this.y);
			}
		}

		// Token: 0x17000379 RID: 889
		// (get) Token: 0x06000F19 RID: 3865 RVA: 0x0002D2B2 File Offset: 0x0002B4B2
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double4 wwxz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double4(this.w, this.w, this.x, this.z);
			}
		}

		// Token: 0x1700037A RID: 890
		// (get) Token: 0x06000F1A RID: 3866 RVA: 0x0002D2D1 File Offset: 0x0002B4D1
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double4 wwxw
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double4(this.w, this.w, this.x, this.w);
			}
		}

		// Token: 0x1700037B RID: 891
		// (get) Token: 0x06000F1B RID: 3867 RVA: 0x0002D2F0 File Offset: 0x0002B4F0
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double4 wwyx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double4(this.w, this.w, this.y, this.x);
			}
		}

		// Token: 0x1700037C RID: 892
		// (get) Token: 0x06000F1C RID: 3868 RVA: 0x0002D30F File Offset: 0x0002B50F
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double4 wwyy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double4(this.w, this.w, this.y, this.y);
			}
		}

		// Token: 0x1700037D RID: 893
		// (get) Token: 0x06000F1D RID: 3869 RVA: 0x0002D32E File Offset: 0x0002B52E
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double4 wwyz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double4(this.w, this.w, this.y, this.z);
			}
		}

		// Token: 0x1700037E RID: 894
		// (get) Token: 0x06000F1E RID: 3870 RVA: 0x0002D34D File Offset: 0x0002B54D
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double4 wwyw
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double4(this.w, this.w, this.y, this.w);
			}
		}

		// Token: 0x1700037F RID: 895
		// (get) Token: 0x06000F1F RID: 3871 RVA: 0x0002D36C File Offset: 0x0002B56C
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double4 wwzx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double4(this.w, this.w, this.z, this.x);
			}
		}

		// Token: 0x17000380 RID: 896
		// (get) Token: 0x06000F20 RID: 3872 RVA: 0x0002D38B File Offset: 0x0002B58B
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double4 wwzy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double4(this.w, this.w, this.z, this.y);
			}
		}

		// Token: 0x17000381 RID: 897
		// (get) Token: 0x06000F21 RID: 3873 RVA: 0x0002D3AA File Offset: 0x0002B5AA
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double4 wwzz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double4(this.w, this.w, this.z, this.z);
			}
		}

		// Token: 0x17000382 RID: 898
		// (get) Token: 0x06000F22 RID: 3874 RVA: 0x0002D3C9 File Offset: 0x0002B5C9
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double4 wwzw
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double4(this.w, this.w, this.z, this.w);
			}
		}

		// Token: 0x17000383 RID: 899
		// (get) Token: 0x06000F23 RID: 3875 RVA: 0x0002D3E8 File Offset: 0x0002B5E8
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double4 wwwx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double4(this.w, this.w, this.w, this.x);
			}
		}

		// Token: 0x17000384 RID: 900
		// (get) Token: 0x06000F24 RID: 3876 RVA: 0x0002D407 File Offset: 0x0002B607
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double4 wwwy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double4(this.w, this.w, this.w, this.y);
			}
		}

		// Token: 0x17000385 RID: 901
		// (get) Token: 0x06000F25 RID: 3877 RVA: 0x0002D426 File Offset: 0x0002B626
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double4 wwwz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double4(this.w, this.w, this.w, this.z);
			}
		}

		// Token: 0x17000386 RID: 902
		// (get) Token: 0x06000F26 RID: 3878 RVA: 0x0002D445 File Offset: 0x0002B645
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double4 wwww
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double4(this.w, this.w, this.w, this.w);
			}
		}

		// Token: 0x17000387 RID: 903
		// (get) Token: 0x06000F27 RID: 3879 RVA: 0x0002D464 File Offset: 0x0002B664
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double3 xxx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double3(this.x, this.x, this.x);
			}
		}

		// Token: 0x17000388 RID: 904
		// (get) Token: 0x06000F28 RID: 3880 RVA: 0x0002D47D File Offset: 0x0002B67D
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double3 xxy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double3(this.x, this.x, this.y);
			}
		}

		// Token: 0x17000389 RID: 905
		// (get) Token: 0x06000F29 RID: 3881 RVA: 0x0002D496 File Offset: 0x0002B696
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double3 xxz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double3(this.x, this.x, this.z);
			}
		}

		// Token: 0x1700038A RID: 906
		// (get) Token: 0x06000F2A RID: 3882 RVA: 0x0002D4AF File Offset: 0x0002B6AF
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double3 xxw
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double3(this.x, this.x, this.w);
			}
		}

		// Token: 0x1700038B RID: 907
		// (get) Token: 0x06000F2B RID: 3883 RVA: 0x0002D4C8 File Offset: 0x0002B6C8
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double3 xyx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double3(this.x, this.y, this.x);
			}
		}

		// Token: 0x1700038C RID: 908
		// (get) Token: 0x06000F2C RID: 3884 RVA: 0x0002D4E1 File Offset: 0x0002B6E1
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double3 xyy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double3(this.x, this.y, this.y);
			}
		}

		// Token: 0x1700038D RID: 909
		// (get) Token: 0x06000F2D RID: 3885 RVA: 0x0002D4FA File Offset: 0x0002B6FA
		// (set) Token: 0x06000F2E RID: 3886 RVA: 0x0002D513 File Offset: 0x0002B713
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double3 xyz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double3(this.x, this.y, this.z);
			}
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			set
			{
				this.x = value.x;
				this.y = value.y;
				this.z = value.z;
			}
		}

		// Token: 0x1700038E RID: 910
		// (get) Token: 0x06000F2F RID: 3887 RVA: 0x0002D539 File Offset: 0x0002B739
		// (set) Token: 0x06000F30 RID: 3888 RVA: 0x0002D552 File Offset: 0x0002B752
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double3 xyw
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double3(this.x, this.y, this.w);
			}
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			set
			{
				this.x = value.x;
				this.y = value.y;
				this.w = value.z;
			}
		}

		// Token: 0x1700038F RID: 911
		// (get) Token: 0x06000F31 RID: 3889 RVA: 0x0002D578 File Offset: 0x0002B778
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double3 xzx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double3(this.x, this.z, this.x);
			}
		}

		// Token: 0x17000390 RID: 912
		// (get) Token: 0x06000F32 RID: 3890 RVA: 0x0002D591 File Offset: 0x0002B791
		// (set) Token: 0x06000F33 RID: 3891 RVA: 0x0002D5AA File Offset: 0x0002B7AA
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double3 xzy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double3(this.x, this.z, this.y);
			}
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			set
			{
				this.x = value.x;
				this.z = value.y;
				this.y = value.z;
			}
		}

		// Token: 0x17000391 RID: 913
		// (get) Token: 0x06000F34 RID: 3892 RVA: 0x0002D5D0 File Offset: 0x0002B7D0
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double3 xzz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double3(this.x, this.z, this.z);
			}
		}

		// Token: 0x17000392 RID: 914
		// (get) Token: 0x06000F35 RID: 3893 RVA: 0x0002D5E9 File Offset: 0x0002B7E9
		// (set) Token: 0x06000F36 RID: 3894 RVA: 0x0002D602 File Offset: 0x0002B802
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double3 xzw
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double3(this.x, this.z, this.w);
			}
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			set
			{
				this.x = value.x;
				this.z = value.y;
				this.w = value.z;
			}
		}

		// Token: 0x17000393 RID: 915
		// (get) Token: 0x06000F37 RID: 3895 RVA: 0x0002D628 File Offset: 0x0002B828
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double3 xwx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double3(this.x, this.w, this.x);
			}
		}

		// Token: 0x17000394 RID: 916
		// (get) Token: 0x06000F38 RID: 3896 RVA: 0x0002D641 File Offset: 0x0002B841
		// (set) Token: 0x06000F39 RID: 3897 RVA: 0x0002D65A File Offset: 0x0002B85A
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double3 xwy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double3(this.x, this.w, this.y);
			}
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			set
			{
				this.x = value.x;
				this.w = value.y;
				this.y = value.z;
			}
		}

		// Token: 0x17000395 RID: 917
		// (get) Token: 0x06000F3A RID: 3898 RVA: 0x0002D680 File Offset: 0x0002B880
		// (set) Token: 0x06000F3B RID: 3899 RVA: 0x0002D699 File Offset: 0x0002B899
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double3 xwz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double3(this.x, this.w, this.z);
			}
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			set
			{
				this.x = value.x;
				this.w = value.y;
				this.z = value.z;
			}
		}

		// Token: 0x17000396 RID: 918
		// (get) Token: 0x06000F3C RID: 3900 RVA: 0x0002D6BF File Offset: 0x0002B8BF
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double3 xww
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double3(this.x, this.w, this.w);
			}
		}

		// Token: 0x17000397 RID: 919
		// (get) Token: 0x06000F3D RID: 3901 RVA: 0x0002D6D8 File Offset: 0x0002B8D8
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double3 yxx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double3(this.y, this.x, this.x);
			}
		}

		// Token: 0x17000398 RID: 920
		// (get) Token: 0x06000F3E RID: 3902 RVA: 0x0002D6F1 File Offset: 0x0002B8F1
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double3 yxy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double3(this.y, this.x, this.y);
			}
		}

		// Token: 0x17000399 RID: 921
		// (get) Token: 0x06000F3F RID: 3903 RVA: 0x0002D70A File Offset: 0x0002B90A
		// (set) Token: 0x06000F40 RID: 3904 RVA: 0x0002D723 File Offset: 0x0002B923
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double3 yxz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double3(this.y, this.x, this.z);
			}
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			set
			{
				this.y = value.x;
				this.x = value.y;
				this.z = value.z;
			}
		}

		// Token: 0x1700039A RID: 922
		// (get) Token: 0x06000F41 RID: 3905 RVA: 0x0002D749 File Offset: 0x0002B949
		// (set) Token: 0x06000F42 RID: 3906 RVA: 0x0002D762 File Offset: 0x0002B962
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double3 yxw
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double3(this.y, this.x, this.w);
			}
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			set
			{
				this.y = value.x;
				this.x = value.y;
				this.w = value.z;
			}
		}

		// Token: 0x1700039B RID: 923
		// (get) Token: 0x06000F43 RID: 3907 RVA: 0x0002D788 File Offset: 0x0002B988
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double3 yyx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double3(this.y, this.y, this.x);
			}
		}

		// Token: 0x1700039C RID: 924
		// (get) Token: 0x06000F44 RID: 3908 RVA: 0x0002D7A1 File Offset: 0x0002B9A1
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double3 yyy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double3(this.y, this.y, this.y);
			}
		}

		// Token: 0x1700039D RID: 925
		// (get) Token: 0x06000F45 RID: 3909 RVA: 0x0002D7BA File Offset: 0x0002B9BA
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double3 yyz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double3(this.y, this.y, this.z);
			}
		}

		// Token: 0x1700039E RID: 926
		// (get) Token: 0x06000F46 RID: 3910 RVA: 0x0002D7D3 File Offset: 0x0002B9D3
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double3 yyw
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double3(this.y, this.y, this.w);
			}
		}

		// Token: 0x1700039F RID: 927
		// (get) Token: 0x06000F47 RID: 3911 RVA: 0x0002D7EC File Offset: 0x0002B9EC
		// (set) Token: 0x06000F48 RID: 3912 RVA: 0x0002D805 File Offset: 0x0002BA05
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double3 yzx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double3(this.y, this.z, this.x);
			}
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			set
			{
				this.y = value.x;
				this.z = value.y;
				this.x = value.z;
			}
		}

		// Token: 0x170003A0 RID: 928
		// (get) Token: 0x06000F49 RID: 3913 RVA: 0x0002D82B File Offset: 0x0002BA2B
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double3 yzy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double3(this.y, this.z, this.y);
			}
		}

		// Token: 0x170003A1 RID: 929
		// (get) Token: 0x06000F4A RID: 3914 RVA: 0x0002D844 File Offset: 0x0002BA44
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double3 yzz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double3(this.y, this.z, this.z);
			}
		}

		// Token: 0x170003A2 RID: 930
		// (get) Token: 0x06000F4B RID: 3915 RVA: 0x0002D85D File Offset: 0x0002BA5D
		// (set) Token: 0x06000F4C RID: 3916 RVA: 0x0002D876 File Offset: 0x0002BA76
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double3 yzw
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double3(this.y, this.z, this.w);
			}
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			set
			{
				this.y = value.x;
				this.z = value.y;
				this.w = value.z;
			}
		}

		// Token: 0x170003A3 RID: 931
		// (get) Token: 0x06000F4D RID: 3917 RVA: 0x0002D89C File Offset: 0x0002BA9C
		// (set) Token: 0x06000F4E RID: 3918 RVA: 0x0002D8B5 File Offset: 0x0002BAB5
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double3 ywx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double3(this.y, this.w, this.x);
			}
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			set
			{
				this.y = value.x;
				this.w = value.y;
				this.x = value.z;
			}
		}

		// Token: 0x170003A4 RID: 932
		// (get) Token: 0x06000F4F RID: 3919 RVA: 0x0002D8DB File Offset: 0x0002BADB
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double3 ywy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double3(this.y, this.w, this.y);
			}
		}

		// Token: 0x170003A5 RID: 933
		// (get) Token: 0x06000F50 RID: 3920 RVA: 0x0002D8F4 File Offset: 0x0002BAF4
		// (set) Token: 0x06000F51 RID: 3921 RVA: 0x0002D90D File Offset: 0x0002BB0D
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double3 ywz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double3(this.y, this.w, this.z);
			}
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			set
			{
				this.y = value.x;
				this.w = value.y;
				this.z = value.z;
			}
		}

		// Token: 0x170003A6 RID: 934
		// (get) Token: 0x06000F52 RID: 3922 RVA: 0x0002D933 File Offset: 0x0002BB33
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double3 yww
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double3(this.y, this.w, this.w);
			}
		}

		// Token: 0x170003A7 RID: 935
		// (get) Token: 0x06000F53 RID: 3923 RVA: 0x0002D94C File Offset: 0x0002BB4C
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double3 zxx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double3(this.z, this.x, this.x);
			}
		}

		// Token: 0x170003A8 RID: 936
		// (get) Token: 0x06000F54 RID: 3924 RVA: 0x0002D965 File Offset: 0x0002BB65
		// (set) Token: 0x06000F55 RID: 3925 RVA: 0x0002D97E File Offset: 0x0002BB7E
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double3 zxy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double3(this.z, this.x, this.y);
			}
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			set
			{
				this.z = value.x;
				this.x = value.y;
				this.y = value.z;
			}
		}

		// Token: 0x170003A9 RID: 937
		// (get) Token: 0x06000F56 RID: 3926 RVA: 0x0002D9A4 File Offset: 0x0002BBA4
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double3 zxz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double3(this.z, this.x, this.z);
			}
		}

		// Token: 0x170003AA RID: 938
		// (get) Token: 0x06000F57 RID: 3927 RVA: 0x0002D9BD File Offset: 0x0002BBBD
		// (set) Token: 0x06000F58 RID: 3928 RVA: 0x0002D9D6 File Offset: 0x0002BBD6
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double3 zxw
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double3(this.z, this.x, this.w);
			}
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			set
			{
				this.z = value.x;
				this.x = value.y;
				this.w = value.z;
			}
		}

		// Token: 0x170003AB RID: 939
		// (get) Token: 0x06000F59 RID: 3929 RVA: 0x0002D9FC File Offset: 0x0002BBFC
		// (set) Token: 0x06000F5A RID: 3930 RVA: 0x0002DA15 File Offset: 0x0002BC15
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double3 zyx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double3(this.z, this.y, this.x);
			}
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			set
			{
				this.z = value.x;
				this.y = value.y;
				this.x = value.z;
			}
		}

		// Token: 0x170003AC RID: 940
		// (get) Token: 0x06000F5B RID: 3931 RVA: 0x0002DA3B File Offset: 0x0002BC3B
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double3 zyy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double3(this.z, this.y, this.y);
			}
		}

		// Token: 0x170003AD RID: 941
		// (get) Token: 0x06000F5C RID: 3932 RVA: 0x0002DA54 File Offset: 0x0002BC54
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double3 zyz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double3(this.z, this.y, this.z);
			}
		}

		// Token: 0x170003AE RID: 942
		// (get) Token: 0x06000F5D RID: 3933 RVA: 0x0002DA6D File Offset: 0x0002BC6D
		// (set) Token: 0x06000F5E RID: 3934 RVA: 0x0002DA86 File Offset: 0x0002BC86
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double3 zyw
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double3(this.z, this.y, this.w);
			}
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			set
			{
				this.z = value.x;
				this.y = value.y;
				this.w = value.z;
			}
		}

		// Token: 0x170003AF RID: 943
		// (get) Token: 0x06000F5F RID: 3935 RVA: 0x0002DAAC File Offset: 0x0002BCAC
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double3 zzx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double3(this.z, this.z, this.x);
			}
		}

		// Token: 0x170003B0 RID: 944
		// (get) Token: 0x06000F60 RID: 3936 RVA: 0x0002DAC5 File Offset: 0x0002BCC5
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double3 zzy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double3(this.z, this.z, this.y);
			}
		}

		// Token: 0x170003B1 RID: 945
		// (get) Token: 0x06000F61 RID: 3937 RVA: 0x0002DADE File Offset: 0x0002BCDE
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double3 zzz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double3(this.z, this.z, this.z);
			}
		}

		// Token: 0x170003B2 RID: 946
		// (get) Token: 0x06000F62 RID: 3938 RVA: 0x0002DAF7 File Offset: 0x0002BCF7
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double3 zzw
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double3(this.z, this.z, this.w);
			}
		}

		// Token: 0x170003B3 RID: 947
		// (get) Token: 0x06000F63 RID: 3939 RVA: 0x0002DB10 File Offset: 0x0002BD10
		// (set) Token: 0x06000F64 RID: 3940 RVA: 0x0002DB29 File Offset: 0x0002BD29
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double3 zwx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double3(this.z, this.w, this.x);
			}
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			set
			{
				this.z = value.x;
				this.w = value.y;
				this.x = value.z;
			}
		}

		// Token: 0x170003B4 RID: 948
		// (get) Token: 0x06000F65 RID: 3941 RVA: 0x0002DB4F File Offset: 0x0002BD4F
		// (set) Token: 0x06000F66 RID: 3942 RVA: 0x0002DB68 File Offset: 0x0002BD68
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double3 zwy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double3(this.z, this.w, this.y);
			}
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			set
			{
				this.z = value.x;
				this.w = value.y;
				this.y = value.z;
			}
		}

		// Token: 0x170003B5 RID: 949
		// (get) Token: 0x06000F67 RID: 3943 RVA: 0x0002DB8E File Offset: 0x0002BD8E
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double3 zwz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double3(this.z, this.w, this.z);
			}
		}

		// Token: 0x170003B6 RID: 950
		// (get) Token: 0x06000F68 RID: 3944 RVA: 0x0002DBA7 File Offset: 0x0002BDA7
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double3 zww
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double3(this.z, this.w, this.w);
			}
		}

		// Token: 0x170003B7 RID: 951
		// (get) Token: 0x06000F69 RID: 3945 RVA: 0x0002DBC0 File Offset: 0x0002BDC0
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double3 wxx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double3(this.w, this.x, this.x);
			}
		}

		// Token: 0x170003B8 RID: 952
		// (get) Token: 0x06000F6A RID: 3946 RVA: 0x0002DBD9 File Offset: 0x0002BDD9
		// (set) Token: 0x06000F6B RID: 3947 RVA: 0x0002DBF2 File Offset: 0x0002BDF2
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double3 wxy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double3(this.w, this.x, this.y);
			}
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			set
			{
				this.w = value.x;
				this.x = value.y;
				this.y = value.z;
			}
		}

		// Token: 0x170003B9 RID: 953
		// (get) Token: 0x06000F6C RID: 3948 RVA: 0x0002DC18 File Offset: 0x0002BE18
		// (set) Token: 0x06000F6D RID: 3949 RVA: 0x0002DC31 File Offset: 0x0002BE31
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double3 wxz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double3(this.w, this.x, this.z);
			}
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			set
			{
				this.w = value.x;
				this.x = value.y;
				this.z = value.z;
			}
		}

		// Token: 0x170003BA RID: 954
		// (get) Token: 0x06000F6E RID: 3950 RVA: 0x0002DC57 File Offset: 0x0002BE57
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double3 wxw
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double3(this.w, this.x, this.w);
			}
		}

		// Token: 0x170003BB RID: 955
		// (get) Token: 0x06000F6F RID: 3951 RVA: 0x0002DC70 File Offset: 0x0002BE70
		// (set) Token: 0x06000F70 RID: 3952 RVA: 0x0002DC89 File Offset: 0x0002BE89
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double3 wyx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double3(this.w, this.y, this.x);
			}
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			set
			{
				this.w = value.x;
				this.y = value.y;
				this.x = value.z;
			}
		}

		// Token: 0x170003BC RID: 956
		// (get) Token: 0x06000F71 RID: 3953 RVA: 0x0002DCAF File Offset: 0x0002BEAF
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double3 wyy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double3(this.w, this.y, this.y);
			}
		}

		// Token: 0x170003BD RID: 957
		// (get) Token: 0x06000F72 RID: 3954 RVA: 0x0002DCC8 File Offset: 0x0002BEC8
		// (set) Token: 0x06000F73 RID: 3955 RVA: 0x0002DCE1 File Offset: 0x0002BEE1
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double3 wyz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double3(this.w, this.y, this.z);
			}
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			set
			{
				this.w = value.x;
				this.y = value.y;
				this.z = value.z;
			}
		}

		// Token: 0x170003BE RID: 958
		// (get) Token: 0x06000F74 RID: 3956 RVA: 0x0002DD07 File Offset: 0x0002BF07
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double3 wyw
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double3(this.w, this.y, this.w);
			}
		}

		// Token: 0x170003BF RID: 959
		// (get) Token: 0x06000F75 RID: 3957 RVA: 0x0002DD20 File Offset: 0x0002BF20
		// (set) Token: 0x06000F76 RID: 3958 RVA: 0x0002DD39 File Offset: 0x0002BF39
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double3 wzx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double3(this.w, this.z, this.x);
			}
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			set
			{
				this.w = value.x;
				this.z = value.y;
				this.x = value.z;
			}
		}

		// Token: 0x170003C0 RID: 960
		// (get) Token: 0x06000F77 RID: 3959 RVA: 0x0002DD5F File Offset: 0x0002BF5F
		// (set) Token: 0x06000F78 RID: 3960 RVA: 0x0002DD78 File Offset: 0x0002BF78
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double3 wzy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double3(this.w, this.z, this.y);
			}
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			set
			{
				this.w = value.x;
				this.z = value.y;
				this.y = value.z;
			}
		}

		// Token: 0x170003C1 RID: 961
		// (get) Token: 0x06000F79 RID: 3961 RVA: 0x0002DD9E File Offset: 0x0002BF9E
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double3 wzz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double3(this.w, this.z, this.z);
			}
		}

		// Token: 0x170003C2 RID: 962
		// (get) Token: 0x06000F7A RID: 3962 RVA: 0x0002DDB7 File Offset: 0x0002BFB7
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double3 wzw
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double3(this.w, this.z, this.w);
			}
		}

		// Token: 0x170003C3 RID: 963
		// (get) Token: 0x06000F7B RID: 3963 RVA: 0x0002DDD0 File Offset: 0x0002BFD0
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double3 wwx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double3(this.w, this.w, this.x);
			}
		}

		// Token: 0x170003C4 RID: 964
		// (get) Token: 0x06000F7C RID: 3964 RVA: 0x0002DDE9 File Offset: 0x0002BFE9
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double3 wwy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double3(this.w, this.w, this.y);
			}
		}

		// Token: 0x170003C5 RID: 965
		// (get) Token: 0x06000F7D RID: 3965 RVA: 0x0002DE02 File Offset: 0x0002C002
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double3 wwz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double3(this.w, this.w, this.z);
			}
		}

		// Token: 0x170003C6 RID: 966
		// (get) Token: 0x06000F7E RID: 3966 RVA: 0x0002DE1B File Offset: 0x0002C01B
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double3 www
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double3(this.w, this.w, this.w);
			}
		}

		// Token: 0x170003C7 RID: 967
		// (get) Token: 0x06000F7F RID: 3967 RVA: 0x0002DE34 File Offset: 0x0002C034
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double2 xx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double2(this.x, this.x);
			}
		}

		// Token: 0x170003C8 RID: 968
		// (get) Token: 0x06000F80 RID: 3968 RVA: 0x0002DE47 File Offset: 0x0002C047
		// (set) Token: 0x06000F81 RID: 3969 RVA: 0x0002DE5A File Offset: 0x0002C05A
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

		// Token: 0x170003C9 RID: 969
		// (get) Token: 0x06000F82 RID: 3970 RVA: 0x0002DE74 File Offset: 0x0002C074
		// (set) Token: 0x06000F83 RID: 3971 RVA: 0x0002DE87 File Offset: 0x0002C087
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double2 xz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double2(this.x, this.z);
			}
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			set
			{
				this.x = value.x;
				this.z = value.y;
			}
		}

		// Token: 0x170003CA RID: 970
		// (get) Token: 0x06000F84 RID: 3972 RVA: 0x0002DEA1 File Offset: 0x0002C0A1
		// (set) Token: 0x06000F85 RID: 3973 RVA: 0x0002DEB4 File Offset: 0x0002C0B4
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double2 xw
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double2(this.x, this.w);
			}
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			set
			{
				this.x = value.x;
				this.w = value.y;
			}
		}

		// Token: 0x170003CB RID: 971
		// (get) Token: 0x06000F86 RID: 3974 RVA: 0x0002DECE File Offset: 0x0002C0CE
		// (set) Token: 0x06000F87 RID: 3975 RVA: 0x0002DEE1 File Offset: 0x0002C0E1
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

		// Token: 0x170003CC RID: 972
		// (get) Token: 0x06000F88 RID: 3976 RVA: 0x0002DEFB File Offset: 0x0002C0FB
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double2 yy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double2(this.y, this.y);
			}
		}

		// Token: 0x170003CD RID: 973
		// (get) Token: 0x06000F89 RID: 3977 RVA: 0x0002DF0E File Offset: 0x0002C10E
		// (set) Token: 0x06000F8A RID: 3978 RVA: 0x0002DF21 File Offset: 0x0002C121
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double2 yz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double2(this.y, this.z);
			}
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			set
			{
				this.y = value.x;
				this.z = value.y;
			}
		}

		// Token: 0x170003CE RID: 974
		// (get) Token: 0x06000F8B RID: 3979 RVA: 0x0002DF3B File Offset: 0x0002C13B
		// (set) Token: 0x06000F8C RID: 3980 RVA: 0x0002DF4E File Offset: 0x0002C14E
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double2 yw
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double2(this.y, this.w);
			}
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			set
			{
				this.y = value.x;
				this.w = value.y;
			}
		}

		// Token: 0x170003CF RID: 975
		// (get) Token: 0x06000F8D RID: 3981 RVA: 0x0002DF68 File Offset: 0x0002C168
		// (set) Token: 0x06000F8E RID: 3982 RVA: 0x0002DF7B File Offset: 0x0002C17B
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double2 zx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double2(this.z, this.x);
			}
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			set
			{
				this.z = value.x;
				this.x = value.y;
			}
		}

		// Token: 0x170003D0 RID: 976
		// (get) Token: 0x06000F8F RID: 3983 RVA: 0x0002DF95 File Offset: 0x0002C195
		// (set) Token: 0x06000F90 RID: 3984 RVA: 0x0002DFA8 File Offset: 0x0002C1A8
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double2 zy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double2(this.z, this.y);
			}
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			set
			{
				this.z = value.x;
				this.y = value.y;
			}
		}

		// Token: 0x170003D1 RID: 977
		// (get) Token: 0x06000F91 RID: 3985 RVA: 0x0002DFC2 File Offset: 0x0002C1C2
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double2 zz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double2(this.z, this.z);
			}
		}

		// Token: 0x170003D2 RID: 978
		// (get) Token: 0x06000F92 RID: 3986 RVA: 0x0002DFD5 File Offset: 0x0002C1D5
		// (set) Token: 0x06000F93 RID: 3987 RVA: 0x0002DFE8 File Offset: 0x0002C1E8
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double2 zw
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double2(this.z, this.w);
			}
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			set
			{
				this.z = value.x;
				this.w = value.y;
			}
		}

		// Token: 0x170003D3 RID: 979
		// (get) Token: 0x06000F94 RID: 3988 RVA: 0x0002E002 File Offset: 0x0002C202
		// (set) Token: 0x06000F95 RID: 3989 RVA: 0x0002E015 File Offset: 0x0002C215
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double2 wx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double2(this.w, this.x);
			}
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			set
			{
				this.w = value.x;
				this.x = value.y;
			}
		}

		// Token: 0x170003D4 RID: 980
		// (get) Token: 0x06000F96 RID: 3990 RVA: 0x0002E02F File Offset: 0x0002C22F
		// (set) Token: 0x06000F97 RID: 3991 RVA: 0x0002E042 File Offset: 0x0002C242
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double2 wy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double2(this.w, this.y);
			}
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			set
			{
				this.w = value.x;
				this.y = value.y;
			}
		}

		// Token: 0x170003D5 RID: 981
		// (get) Token: 0x06000F98 RID: 3992 RVA: 0x0002E05C File Offset: 0x0002C25C
		// (set) Token: 0x06000F99 RID: 3993 RVA: 0x0002E06F File Offset: 0x0002C26F
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double2 wz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double2(this.w, this.z);
			}
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			set
			{
				this.w = value.x;
				this.z = value.y;
			}
		}

		// Token: 0x170003D6 RID: 982
		// (get) Token: 0x06000F9A RID: 3994 RVA: 0x0002E089 File Offset: 0x0002C289
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double2 ww
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double2(this.w, this.w);
			}
		}

		// Token: 0x170003D7 RID: 983
		public unsafe double this[int index]
		{
			get
			{
				fixed (double4* ptr = &this)
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

		// Token: 0x06000F9D RID: 3997 RVA: 0x0002E0D4 File Offset: 0x0002C2D4
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool Equals(double4 rhs)
		{
			return this.x == rhs.x && this.y == rhs.y && this.z == rhs.z && this.w == rhs.w;
		}

		// Token: 0x06000F9E RID: 3998 RVA: 0x0002E110 File Offset: 0x0002C310
		public override bool Equals(object o)
		{
			if (o is double4)
			{
				double4 @double = (double4)o;
				return this.Equals(@double);
			}
			return false;
		}

		// Token: 0x06000F9F RID: 3999 RVA: 0x0002E135 File Offset: 0x0002C335
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public override int GetHashCode()
		{
			return (int)math.hash(this);
		}

		// Token: 0x06000FA0 RID: 4000 RVA: 0x0002E144 File Offset: 0x0002C344
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public override string ToString()
		{
			return string.Format("double4({0}, {1}, {2}, {3})", new object[] { this.x, this.y, this.z, this.w });
		}

		// Token: 0x06000FA1 RID: 4001 RVA: 0x0002E19C File Offset: 0x0002C39C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public string ToString(string format, IFormatProvider formatProvider)
		{
			return string.Format("double4({0}, {1}, {2}, {3})", new object[]
			{
				this.x.ToString(format, formatProvider),
				this.y.ToString(format, formatProvider),
				this.z.ToString(format, formatProvider),
				this.w.ToString(format, formatProvider)
			});
		}

		// Token: 0x0400006A RID: 106
		public double x;

		// Token: 0x0400006B RID: 107
		public double y;

		// Token: 0x0400006C RID: 108
		public double z;

		// Token: 0x0400006D RID: 109
		public double w;

		// Token: 0x0400006E RID: 110
		public static readonly double4 zero;

		// Token: 0x0200005A RID: 90
		internal sealed class DebuggerProxy
		{
			// Token: 0x060024C7 RID: 9415 RVA: 0x000691CC File Offset: 0x000673CC
			public DebuggerProxy(double4 v)
			{
				this.x = v.x;
				this.y = v.y;
				this.z = v.z;
				this.w = v.w;
			}

			// Token: 0x04000158 RID: 344
			public double x;

			// Token: 0x04000159 RID: 345
			public double y;

			// Token: 0x0400015A RID: 346
			public double z;

			// Token: 0x0400015B RID: 347
			public double w;
		}
	}
}
