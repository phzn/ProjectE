using System;
using System.Runtime.CompilerServices;
using Unity.IL2CPP.CompilerServices;

namespace Unity.Mathematics
{
	// Token: 0x02000046 RID: 70
	[Il2CppEagerStaticClassConstruction]
	[Serializable]
	public struct uint3x2 : IEquatable<uint3x2>, IFormattable
	{
		// Token: 0x060020FC RID: 8444 RVA: 0x0005F3C2 File Offset: 0x0005D5C2
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public uint3x2(uint3 c0, uint3 c1)
		{
			this.c0 = c0;
			this.c1 = c1;
		}

		// Token: 0x060020FD RID: 8445 RVA: 0x0005F3D2 File Offset: 0x0005D5D2
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public uint3x2(uint m00, uint m01, uint m10, uint m11, uint m20, uint m21)
		{
			this.c0 = new uint3(m00, m10, m20);
			this.c1 = new uint3(m01, m11, m21);
		}

		// Token: 0x060020FE RID: 8446 RVA: 0x0005F3F3 File Offset: 0x0005D5F3
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public uint3x2(uint v)
		{
			this.c0 = v;
			this.c1 = v;
		}

		// Token: 0x060020FF RID: 8447 RVA: 0x0005F40D File Offset: 0x0005D60D
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public uint3x2(bool v)
		{
			this.c0 = math.select(new uint3(0U), new uint3(1U), v);
			this.c1 = math.select(new uint3(0U), new uint3(1U), v);
		}

		// Token: 0x06002100 RID: 8448 RVA: 0x0005F43F File Offset: 0x0005D63F
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public uint3x2(bool3x2 v)
		{
			this.c0 = math.select(new uint3(0U), new uint3(1U), v.c0);
			this.c1 = math.select(new uint3(0U), new uint3(1U), v.c1);
		}

		// Token: 0x06002101 RID: 8449 RVA: 0x0005F47B File Offset: 0x0005D67B
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public uint3x2(int v)
		{
			this.c0 = (uint3)v;
			this.c1 = (uint3)v;
		}

		// Token: 0x06002102 RID: 8450 RVA: 0x0005F495 File Offset: 0x0005D695
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public uint3x2(int3x2 v)
		{
			this.c0 = (uint3)v.c0;
			this.c1 = (uint3)v.c1;
		}

		// Token: 0x06002103 RID: 8451 RVA: 0x0005F4B9 File Offset: 0x0005D6B9
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public uint3x2(float v)
		{
			this.c0 = (uint3)v;
			this.c1 = (uint3)v;
		}

		// Token: 0x06002104 RID: 8452 RVA: 0x0005F4D3 File Offset: 0x0005D6D3
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public uint3x2(float3x2 v)
		{
			this.c0 = (uint3)v.c0;
			this.c1 = (uint3)v.c1;
		}

		// Token: 0x06002105 RID: 8453 RVA: 0x0005F4F7 File Offset: 0x0005D6F7
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public uint3x2(double v)
		{
			this.c0 = (uint3)v;
			this.c1 = (uint3)v;
		}

		// Token: 0x06002106 RID: 8454 RVA: 0x0005F511 File Offset: 0x0005D711
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public uint3x2(double3x2 v)
		{
			this.c0 = (uint3)v.c0;
			this.c1 = (uint3)v.c1;
		}

		// Token: 0x06002107 RID: 8455 RVA: 0x0005F535 File Offset: 0x0005D735
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static implicit operator uint3x2(uint v)
		{
			return new uint3x2(v);
		}

		// Token: 0x06002108 RID: 8456 RVA: 0x0005F53D File Offset: 0x0005D73D
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static explicit operator uint3x2(bool v)
		{
			return new uint3x2(v);
		}

		// Token: 0x06002109 RID: 8457 RVA: 0x0005F545 File Offset: 0x0005D745
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static explicit operator uint3x2(bool3x2 v)
		{
			return new uint3x2(v);
		}

		// Token: 0x0600210A RID: 8458 RVA: 0x0005F54D File Offset: 0x0005D74D
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static explicit operator uint3x2(int v)
		{
			return new uint3x2(v);
		}

		// Token: 0x0600210B RID: 8459 RVA: 0x0005F555 File Offset: 0x0005D755
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static explicit operator uint3x2(int3x2 v)
		{
			return new uint3x2(v);
		}

		// Token: 0x0600210C RID: 8460 RVA: 0x0005F55D File Offset: 0x0005D75D
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static explicit operator uint3x2(float v)
		{
			return new uint3x2(v);
		}

		// Token: 0x0600210D RID: 8461 RVA: 0x0005F565 File Offset: 0x0005D765
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static explicit operator uint3x2(float3x2 v)
		{
			return new uint3x2(v);
		}

		// Token: 0x0600210E RID: 8462 RVA: 0x0005F56D File Offset: 0x0005D76D
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static explicit operator uint3x2(double v)
		{
			return new uint3x2(v);
		}

		// Token: 0x0600210F RID: 8463 RVA: 0x0005F575 File Offset: 0x0005D775
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static explicit operator uint3x2(double3x2 v)
		{
			return new uint3x2(v);
		}

		// Token: 0x06002110 RID: 8464 RVA: 0x0005F57D File Offset: 0x0005D77D
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint3x2 operator *(uint3x2 lhs, uint3x2 rhs)
		{
			return new uint3x2(lhs.c0 * rhs.c0, lhs.c1 * rhs.c1);
		}

		// Token: 0x06002111 RID: 8465 RVA: 0x0005F5A6 File Offset: 0x0005D7A6
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint3x2 operator *(uint3x2 lhs, uint rhs)
		{
			return new uint3x2(lhs.c0 * rhs, lhs.c1 * rhs);
		}

		// Token: 0x06002112 RID: 8466 RVA: 0x0005F5C5 File Offset: 0x0005D7C5
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint3x2 operator *(uint lhs, uint3x2 rhs)
		{
			return new uint3x2(lhs * rhs.c0, lhs * rhs.c1);
		}

		// Token: 0x06002113 RID: 8467 RVA: 0x0005F5E4 File Offset: 0x0005D7E4
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint3x2 operator +(uint3x2 lhs, uint3x2 rhs)
		{
			return new uint3x2(lhs.c0 + rhs.c0, lhs.c1 + rhs.c1);
		}

		// Token: 0x06002114 RID: 8468 RVA: 0x0005F60D File Offset: 0x0005D80D
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint3x2 operator +(uint3x2 lhs, uint rhs)
		{
			return new uint3x2(lhs.c0 + rhs, lhs.c1 + rhs);
		}

		// Token: 0x06002115 RID: 8469 RVA: 0x0005F62C File Offset: 0x0005D82C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint3x2 operator +(uint lhs, uint3x2 rhs)
		{
			return new uint3x2(lhs + rhs.c0, lhs + rhs.c1);
		}

		// Token: 0x06002116 RID: 8470 RVA: 0x0005F64B File Offset: 0x0005D84B
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint3x2 operator -(uint3x2 lhs, uint3x2 rhs)
		{
			return new uint3x2(lhs.c0 - rhs.c0, lhs.c1 - rhs.c1);
		}

		// Token: 0x06002117 RID: 8471 RVA: 0x0005F674 File Offset: 0x0005D874
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint3x2 operator -(uint3x2 lhs, uint rhs)
		{
			return new uint3x2(lhs.c0 - rhs, lhs.c1 - rhs);
		}

		// Token: 0x06002118 RID: 8472 RVA: 0x0005F693 File Offset: 0x0005D893
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint3x2 operator -(uint lhs, uint3x2 rhs)
		{
			return new uint3x2(lhs - rhs.c0, lhs - rhs.c1);
		}

		// Token: 0x06002119 RID: 8473 RVA: 0x0005F6B2 File Offset: 0x0005D8B2
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint3x2 operator /(uint3x2 lhs, uint3x2 rhs)
		{
			return new uint3x2(lhs.c0 / rhs.c0, lhs.c1 / rhs.c1);
		}

		// Token: 0x0600211A RID: 8474 RVA: 0x0005F6DB File Offset: 0x0005D8DB
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint3x2 operator /(uint3x2 lhs, uint rhs)
		{
			return new uint3x2(lhs.c0 / rhs, lhs.c1 / rhs);
		}

		// Token: 0x0600211B RID: 8475 RVA: 0x0005F6FA File Offset: 0x0005D8FA
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint3x2 operator /(uint lhs, uint3x2 rhs)
		{
			return new uint3x2(lhs / rhs.c0, lhs / rhs.c1);
		}

		// Token: 0x0600211C RID: 8476 RVA: 0x0005F719 File Offset: 0x0005D919
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint3x2 operator %(uint3x2 lhs, uint3x2 rhs)
		{
			return new uint3x2(lhs.c0 % rhs.c0, lhs.c1 % rhs.c1);
		}

		// Token: 0x0600211D RID: 8477 RVA: 0x0005F742 File Offset: 0x0005D942
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint3x2 operator %(uint3x2 lhs, uint rhs)
		{
			return new uint3x2(lhs.c0 % rhs, lhs.c1 % rhs);
		}

		// Token: 0x0600211E RID: 8478 RVA: 0x0005F761 File Offset: 0x0005D961
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint3x2 operator %(uint lhs, uint3x2 rhs)
		{
			return new uint3x2(lhs % rhs.c0, lhs % rhs.c1);
		}

		// Token: 0x0600211F RID: 8479 RVA: 0x0005F780 File Offset: 0x0005D980
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint3x2 operator ++(uint3x2 val)
		{
			uint3 @uint = uint3.op_Increment(val.c0);
			val.c0 = @uint;
			uint3 uint2 = @uint;
			@uint = uint3.op_Increment(val.c1);
			val.c1 = @uint;
			return new uint3x2(uint2, @uint);
		}

		// Token: 0x06002120 RID: 8480 RVA: 0x0005F7C8 File Offset: 0x0005D9C8
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint3x2 operator --(uint3x2 val)
		{
			uint3 @uint = uint3.op_Decrement(val.c0);
			val.c0 = @uint;
			uint3 uint2 = @uint;
			@uint = uint3.op_Decrement(val.c1);
			val.c1 = @uint;
			return new uint3x2(uint2, @uint);
		}

		// Token: 0x06002121 RID: 8481 RVA: 0x0005F80E File Offset: 0x0005DA0E
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool3x2 operator <(uint3x2 lhs, uint3x2 rhs)
		{
			return new bool3x2(lhs.c0 < rhs.c0, lhs.c1 < rhs.c1);
		}

		// Token: 0x06002122 RID: 8482 RVA: 0x0005F837 File Offset: 0x0005DA37
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool3x2 operator <(uint3x2 lhs, uint rhs)
		{
			return new bool3x2(lhs.c0 < rhs, lhs.c1 < rhs);
		}

		// Token: 0x06002123 RID: 8483 RVA: 0x0005F856 File Offset: 0x0005DA56
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool3x2 operator <(uint lhs, uint3x2 rhs)
		{
			return new bool3x2(lhs < rhs.c0, lhs < rhs.c1);
		}

		// Token: 0x06002124 RID: 8484 RVA: 0x0005F875 File Offset: 0x0005DA75
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool3x2 operator <=(uint3x2 lhs, uint3x2 rhs)
		{
			return new bool3x2(lhs.c0 <= rhs.c0, lhs.c1 <= rhs.c1);
		}

		// Token: 0x06002125 RID: 8485 RVA: 0x0005F89E File Offset: 0x0005DA9E
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool3x2 operator <=(uint3x2 lhs, uint rhs)
		{
			return new bool3x2(lhs.c0 <= rhs, lhs.c1 <= rhs);
		}

		// Token: 0x06002126 RID: 8486 RVA: 0x0005F8BD File Offset: 0x0005DABD
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool3x2 operator <=(uint lhs, uint3x2 rhs)
		{
			return new bool3x2(lhs <= rhs.c0, lhs <= rhs.c1);
		}

		// Token: 0x06002127 RID: 8487 RVA: 0x0005F8DC File Offset: 0x0005DADC
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool3x2 operator >(uint3x2 lhs, uint3x2 rhs)
		{
			return new bool3x2(lhs.c0 > rhs.c0, lhs.c1 > rhs.c1);
		}

		// Token: 0x06002128 RID: 8488 RVA: 0x0005F905 File Offset: 0x0005DB05
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool3x2 operator >(uint3x2 lhs, uint rhs)
		{
			return new bool3x2(lhs.c0 > rhs, lhs.c1 > rhs);
		}

		// Token: 0x06002129 RID: 8489 RVA: 0x0005F924 File Offset: 0x0005DB24
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool3x2 operator >(uint lhs, uint3x2 rhs)
		{
			return new bool3x2(lhs > rhs.c0, lhs > rhs.c1);
		}

		// Token: 0x0600212A RID: 8490 RVA: 0x0005F943 File Offset: 0x0005DB43
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool3x2 operator >=(uint3x2 lhs, uint3x2 rhs)
		{
			return new bool3x2(lhs.c0 >= rhs.c0, lhs.c1 >= rhs.c1);
		}

		// Token: 0x0600212B RID: 8491 RVA: 0x0005F96C File Offset: 0x0005DB6C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool3x2 operator >=(uint3x2 lhs, uint rhs)
		{
			return new bool3x2(lhs.c0 >= rhs, lhs.c1 >= rhs);
		}

		// Token: 0x0600212C RID: 8492 RVA: 0x0005F98B File Offset: 0x0005DB8B
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool3x2 operator >=(uint lhs, uint3x2 rhs)
		{
			return new bool3x2(lhs >= rhs.c0, lhs >= rhs.c1);
		}

		// Token: 0x0600212D RID: 8493 RVA: 0x0005F9AA File Offset: 0x0005DBAA
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint3x2 operator -(uint3x2 val)
		{
			return new uint3x2(-val.c0, -val.c1);
		}

		// Token: 0x0600212E RID: 8494 RVA: 0x0005F9C7 File Offset: 0x0005DBC7
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint3x2 operator +(uint3x2 val)
		{
			return new uint3x2(+val.c0, +val.c1);
		}

		// Token: 0x0600212F RID: 8495 RVA: 0x0005F9E4 File Offset: 0x0005DBE4
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint3x2 operator <<(uint3x2 x, int n)
		{
			return new uint3x2(x.c0 << n, x.c1 << n);
		}

		// Token: 0x06002130 RID: 8496 RVA: 0x0005FA03 File Offset: 0x0005DC03
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint3x2 operator >>(uint3x2 x, int n)
		{
			return new uint3x2(x.c0 >> n, x.c1 >> n);
		}

		// Token: 0x06002131 RID: 8497 RVA: 0x0005FA22 File Offset: 0x0005DC22
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool3x2 operator ==(uint3x2 lhs, uint3x2 rhs)
		{
			return new bool3x2(lhs.c0 == rhs.c0, lhs.c1 == rhs.c1);
		}

		// Token: 0x06002132 RID: 8498 RVA: 0x0005FA4B File Offset: 0x0005DC4B
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool3x2 operator ==(uint3x2 lhs, uint rhs)
		{
			return new bool3x2(lhs.c0 == rhs, lhs.c1 == rhs);
		}

		// Token: 0x06002133 RID: 8499 RVA: 0x0005FA6A File Offset: 0x0005DC6A
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool3x2 operator ==(uint lhs, uint3x2 rhs)
		{
			return new bool3x2(lhs == rhs.c0, lhs == rhs.c1);
		}

		// Token: 0x06002134 RID: 8500 RVA: 0x0005FA89 File Offset: 0x0005DC89
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool3x2 operator !=(uint3x2 lhs, uint3x2 rhs)
		{
			return new bool3x2(lhs.c0 != rhs.c0, lhs.c1 != rhs.c1);
		}

		// Token: 0x06002135 RID: 8501 RVA: 0x0005FAB2 File Offset: 0x0005DCB2
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool3x2 operator !=(uint3x2 lhs, uint rhs)
		{
			return new bool3x2(lhs.c0 != rhs, lhs.c1 != rhs);
		}

		// Token: 0x06002136 RID: 8502 RVA: 0x0005FAD1 File Offset: 0x0005DCD1
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool3x2 operator !=(uint lhs, uint3x2 rhs)
		{
			return new bool3x2(lhs != rhs.c0, lhs != rhs.c1);
		}

		// Token: 0x06002137 RID: 8503 RVA: 0x0005FAF0 File Offset: 0x0005DCF0
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint3x2 operator ~(uint3x2 val)
		{
			return new uint3x2(~val.c0, ~val.c1);
		}

		// Token: 0x06002138 RID: 8504 RVA: 0x0005FB0D File Offset: 0x0005DD0D
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint3x2 operator &(uint3x2 lhs, uint3x2 rhs)
		{
			return new uint3x2(lhs.c0 & rhs.c0, lhs.c1 & rhs.c1);
		}

		// Token: 0x06002139 RID: 8505 RVA: 0x0005FB36 File Offset: 0x0005DD36
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint3x2 operator &(uint3x2 lhs, uint rhs)
		{
			return new uint3x2(lhs.c0 & rhs, lhs.c1 & rhs);
		}

		// Token: 0x0600213A RID: 8506 RVA: 0x0005FB55 File Offset: 0x0005DD55
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint3x2 operator &(uint lhs, uint3x2 rhs)
		{
			return new uint3x2(lhs & rhs.c0, lhs & rhs.c1);
		}

		// Token: 0x0600213B RID: 8507 RVA: 0x0005FB74 File Offset: 0x0005DD74
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint3x2 operator |(uint3x2 lhs, uint3x2 rhs)
		{
			return new uint3x2(lhs.c0 | rhs.c0, lhs.c1 | rhs.c1);
		}

		// Token: 0x0600213C RID: 8508 RVA: 0x0005FB9D File Offset: 0x0005DD9D
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint3x2 operator |(uint3x2 lhs, uint rhs)
		{
			return new uint3x2(lhs.c0 | rhs, lhs.c1 | rhs);
		}

		// Token: 0x0600213D RID: 8509 RVA: 0x0005FBBC File Offset: 0x0005DDBC
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint3x2 operator |(uint lhs, uint3x2 rhs)
		{
			return new uint3x2(lhs | rhs.c0, lhs | rhs.c1);
		}

		// Token: 0x0600213E RID: 8510 RVA: 0x0005FBDB File Offset: 0x0005DDDB
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint3x2 operator ^(uint3x2 lhs, uint3x2 rhs)
		{
			return new uint3x2(lhs.c0 ^ rhs.c0, lhs.c1 ^ rhs.c1);
		}

		// Token: 0x0600213F RID: 8511 RVA: 0x0005FC04 File Offset: 0x0005DE04
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint3x2 operator ^(uint3x2 lhs, uint rhs)
		{
			return new uint3x2(lhs.c0 ^ rhs, lhs.c1 ^ rhs);
		}

		// Token: 0x06002140 RID: 8512 RVA: 0x0005FC23 File Offset: 0x0005DE23
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint3x2 operator ^(uint lhs, uint3x2 rhs)
		{
			return new uint3x2(lhs ^ rhs.c0, lhs ^ rhs.c1);
		}

		// Token: 0x17000A33 RID: 2611
		public unsafe ref uint3 this[int index]
		{
			get
			{
				fixed (uint3x2* ptr = &this)
				{
					return ref *(uint3*)(ptr + (IntPtr)index * (IntPtr)sizeof(uint3) / (IntPtr)sizeof(uint3x2));
				}
			}
		}

		// Token: 0x06002142 RID: 8514 RVA: 0x0005FC5F File Offset: 0x0005DE5F
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool Equals(uint3x2 rhs)
		{
			return this.c0.Equals(rhs.c0) && this.c1.Equals(rhs.c1);
		}

		// Token: 0x06002143 RID: 8515 RVA: 0x0005FC88 File Offset: 0x0005DE88
		public override bool Equals(object o)
		{
			if (o is uint3x2)
			{
				uint3x2 uint3x = (uint3x2)o;
				return this.Equals(uint3x);
			}
			return false;
		}

		// Token: 0x06002144 RID: 8516 RVA: 0x0005FCAD File Offset: 0x0005DEAD
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public override int GetHashCode()
		{
			return (int)math.hash(this);
		}

		// Token: 0x06002145 RID: 8517 RVA: 0x0005FCBC File Offset: 0x0005DEBC
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public override string ToString()
		{
			return string.Format("uint3x2({0}, {1},  {2}, {3},  {4}, {5})", new object[]
			{
				this.c0.x,
				this.c1.x,
				this.c0.y,
				this.c1.y,
				this.c0.z,
				this.c1.z
			});
		}

		// Token: 0x06002146 RID: 8518 RVA: 0x0005FD4C File Offset: 0x0005DF4C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public string ToString(string format, IFormatProvider formatProvider)
		{
			return string.Format("uint3x2({0}, {1},  {2}, {3},  {4}, {5})", new object[]
			{
				this.c0.x.ToString(format, formatProvider),
				this.c1.x.ToString(format, formatProvider),
				this.c0.y.ToString(format, formatProvider),
				this.c1.y.ToString(format, formatProvider),
				this.c0.z.ToString(format, formatProvider),
				this.c1.z.ToString(format, formatProvider)
			});
		}

		// Token: 0x0400010E RID: 270
		public uint3 c0;

		// Token: 0x0400010F RID: 271
		public uint3 c1;

		// Token: 0x04000110 RID: 272
		public static readonly uint3x2 zero;
	}
}
