using System;
using System.Runtime.CompilerServices;
using Unity.IL2CPP.CompilerServices;
using UnityEngine;

namespace Unity.Mathematics
{
	// Token: 0x02000029 RID: 41
	[Il2CppEagerStaticClassConstruction]
	[Serializable]
	public struct float4x4 : IEquatable<float4x4>, IFormattable
	{
		// Token: 0x0600157D RID: 5501 RVA: 0x0003D4E5 File Offset: 0x0003B6E5
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public float4x4(float4 c0, float4 c1, float4 c2, float4 c3)
		{
			this.c0 = c0;
			this.c1 = c1;
			this.c2 = c2;
			this.c3 = c3;
		}

		// Token: 0x0600157E RID: 5502 RVA: 0x0003D504 File Offset: 0x0003B704
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public float4x4(float m00, float m01, float m02, float m03, float m10, float m11, float m12, float m13, float m20, float m21, float m22, float m23, float m30, float m31, float m32, float m33)
		{
			this.c0 = new float4(m00, m10, m20, m30);
			this.c1 = new float4(m01, m11, m21, m31);
			this.c2 = new float4(m02, m12, m22, m32);
			this.c3 = new float4(m03, m13, m23, m33);
		}

		// Token: 0x0600157F RID: 5503 RVA: 0x0003D55A File Offset: 0x0003B75A
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public float4x4(float v)
		{
			this.c0 = v;
			this.c1 = v;
			this.c2 = v;
			this.c3 = v;
		}

		// Token: 0x06001580 RID: 5504 RVA: 0x0003D58C File Offset: 0x0003B78C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public float4x4(bool v)
		{
			this.c0 = math.select(new float4(0f), new float4(1f), v);
			this.c1 = math.select(new float4(0f), new float4(1f), v);
			this.c2 = math.select(new float4(0f), new float4(1f), v);
			this.c3 = math.select(new float4(0f), new float4(1f), v);
		}

		// Token: 0x06001581 RID: 5505 RVA: 0x0003D61C File Offset: 0x0003B81C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public float4x4(bool4x4 v)
		{
			this.c0 = math.select(new float4(0f), new float4(1f), v.c0);
			this.c1 = math.select(new float4(0f), new float4(1f), v.c1);
			this.c2 = math.select(new float4(0f), new float4(1f), v.c2);
			this.c3 = math.select(new float4(0f), new float4(1f), v.c3);
		}

		// Token: 0x06001582 RID: 5506 RVA: 0x0003D6BD File Offset: 0x0003B8BD
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public float4x4(int v)
		{
			this.c0 = v;
			this.c1 = v;
			this.c2 = v;
			this.c3 = v;
		}

		// Token: 0x06001583 RID: 5507 RVA: 0x0003D6F0 File Offset: 0x0003B8F0
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public float4x4(int4x4 v)
		{
			this.c0 = v.c0;
			this.c1 = v.c1;
			this.c2 = v.c2;
			this.c3 = v.c3;
		}

		// Token: 0x06001584 RID: 5508 RVA: 0x0003D741 File Offset: 0x0003B941
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public float4x4(uint v)
		{
			this.c0 = v;
			this.c1 = v;
			this.c2 = v;
			this.c3 = v;
		}

		// Token: 0x06001585 RID: 5509 RVA: 0x0003D774 File Offset: 0x0003B974
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public float4x4(uint4x4 v)
		{
			this.c0 = v.c0;
			this.c1 = v.c1;
			this.c2 = v.c2;
			this.c3 = v.c3;
		}

		// Token: 0x06001586 RID: 5510 RVA: 0x0003D7C5 File Offset: 0x0003B9C5
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public float4x4(double v)
		{
			this.c0 = (float4)v;
			this.c1 = (float4)v;
			this.c2 = (float4)v;
			this.c3 = (float4)v;
		}

		// Token: 0x06001587 RID: 5511 RVA: 0x0003D7F8 File Offset: 0x0003B9F8
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public float4x4(double4x4 v)
		{
			this.c0 = (float4)v.c0;
			this.c1 = (float4)v.c1;
			this.c2 = (float4)v.c2;
			this.c3 = (float4)v.c3;
		}

		// Token: 0x06001588 RID: 5512 RVA: 0x0003D849 File Offset: 0x0003BA49
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static implicit operator float4x4(float v)
		{
			return new float4x4(v);
		}

		// Token: 0x06001589 RID: 5513 RVA: 0x0003D851 File Offset: 0x0003BA51
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static explicit operator float4x4(bool v)
		{
			return new float4x4(v);
		}

		// Token: 0x0600158A RID: 5514 RVA: 0x0003D859 File Offset: 0x0003BA59
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static explicit operator float4x4(bool4x4 v)
		{
			return new float4x4(v);
		}

		// Token: 0x0600158B RID: 5515 RVA: 0x0003D861 File Offset: 0x0003BA61
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static implicit operator float4x4(int v)
		{
			return new float4x4(v);
		}

		// Token: 0x0600158C RID: 5516 RVA: 0x0003D869 File Offset: 0x0003BA69
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static implicit operator float4x4(int4x4 v)
		{
			return new float4x4(v);
		}

		// Token: 0x0600158D RID: 5517 RVA: 0x0003D871 File Offset: 0x0003BA71
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static implicit operator float4x4(uint v)
		{
			return new float4x4(v);
		}

		// Token: 0x0600158E RID: 5518 RVA: 0x0003D879 File Offset: 0x0003BA79
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static implicit operator float4x4(uint4x4 v)
		{
			return new float4x4(v);
		}

		// Token: 0x0600158F RID: 5519 RVA: 0x0003D881 File Offset: 0x0003BA81
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static explicit operator float4x4(double v)
		{
			return new float4x4(v);
		}

		// Token: 0x06001590 RID: 5520 RVA: 0x0003D889 File Offset: 0x0003BA89
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static explicit operator float4x4(double4x4 v)
		{
			return new float4x4(v);
		}

		// Token: 0x06001591 RID: 5521 RVA: 0x0003D894 File Offset: 0x0003BA94
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float4x4 operator *(float4x4 lhs, float4x4 rhs)
		{
			return new float4x4(lhs.c0 * rhs.c0, lhs.c1 * rhs.c1, lhs.c2 * rhs.c2, lhs.c3 * rhs.c3);
		}

		// Token: 0x06001592 RID: 5522 RVA: 0x0003D8EA File Offset: 0x0003BAEA
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float4x4 operator *(float4x4 lhs, float rhs)
		{
			return new float4x4(lhs.c0 * rhs, lhs.c1 * rhs, lhs.c2 * rhs, lhs.c3 * rhs);
		}

		// Token: 0x06001593 RID: 5523 RVA: 0x0003D921 File Offset: 0x0003BB21
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float4x4 operator *(float lhs, float4x4 rhs)
		{
			return new float4x4(lhs * rhs.c0, lhs * rhs.c1, lhs * rhs.c2, lhs * rhs.c3);
		}

		// Token: 0x06001594 RID: 5524 RVA: 0x0003D958 File Offset: 0x0003BB58
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float4x4 operator +(float4x4 lhs, float4x4 rhs)
		{
			return new float4x4(lhs.c0 + rhs.c0, lhs.c1 + rhs.c1, lhs.c2 + rhs.c2, lhs.c3 + rhs.c3);
		}

		// Token: 0x06001595 RID: 5525 RVA: 0x0003D9AE File Offset: 0x0003BBAE
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float4x4 operator +(float4x4 lhs, float rhs)
		{
			return new float4x4(lhs.c0 + rhs, lhs.c1 + rhs, lhs.c2 + rhs, lhs.c3 + rhs);
		}

		// Token: 0x06001596 RID: 5526 RVA: 0x0003D9E5 File Offset: 0x0003BBE5
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float4x4 operator +(float lhs, float4x4 rhs)
		{
			return new float4x4(lhs + rhs.c0, lhs + rhs.c1, lhs + rhs.c2, lhs + rhs.c3);
		}

		// Token: 0x06001597 RID: 5527 RVA: 0x0003DA1C File Offset: 0x0003BC1C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float4x4 operator -(float4x4 lhs, float4x4 rhs)
		{
			return new float4x4(lhs.c0 - rhs.c0, lhs.c1 - rhs.c1, lhs.c2 - rhs.c2, lhs.c3 - rhs.c3);
		}

		// Token: 0x06001598 RID: 5528 RVA: 0x0003DA72 File Offset: 0x0003BC72
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float4x4 operator -(float4x4 lhs, float rhs)
		{
			return new float4x4(lhs.c0 - rhs, lhs.c1 - rhs, lhs.c2 - rhs, lhs.c3 - rhs);
		}

		// Token: 0x06001599 RID: 5529 RVA: 0x0003DAA9 File Offset: 0x0003BCA9
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float4x4 operator -(float lhs, float4x4 rhs)
		{
			return new float4x4(lhs - rhs.c0, lhs - rhs.c1, lhs - rhs.c2, lhs - rhs.c3);
		}

		// Token: 0x0600159A RID: 5530 RVA: 0x0003DAE0 File Offset: 0x0003BCE0
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float4x4 operator /(float4x4 lhs, float4x4 rhs)
		{
			return new float4x4(lhs.c0 / rhs.c0, lhs.c1 / rhs.c1, lhs.c2 / rhs.c2, lhs.c3 / rhs.c3);
		}

		// Token: 0x0600159B RID: 5531 RVA: 0x0003DB36 File Offset: 0x0003BD36
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float4x4 operator /(float4x4 lhs, float rhs)
		{
			return new float4x4(lhs.c0 / rhs, lhs.c1 / rhs, lhs.c2 / rhs, lhs.c3 / rhs);
		}

		// Token: 0x0600159C RID: 5532 RVA: 0x0003DB6D File Offset: 0x0003BD6D
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float4x4 operator /(float lhs, float4x4 rhs)
		{
			return new float4x4(lhs / rhs.c0, lhs / rhs.c1, lhs / rhs.c2, lhs / rhs.c3);
		}

		// Token: 0x0600159D RID: 5533 RVA: 0x0003DBA4 File Offset: 0x0003BDA4
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float4x4 operator %(float4x4 lhs, float4x4 rhs)
		{
			return new float4x4(lhs.c0 % rhs.c0, lhs.c1 % rhs.c1, lhs.c2 % rhs.c2, lhs.c3 % rhs.c3);
		}

		// Token: 0x0600159E RID: 5534 RVA: 0x0003DBFA File Offset: 0x0003BDFA
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float4x4 operator %(float4x4 lhs, float rhs)
		{
			return new float4x4(lhs.c0 % rhs, lhs.c1 % rhs, lhs.c2 % rhs, lhs.c3 % rhs);
		}

		// Token: 0x0600159F RID: 5535 RVA: 0x0003DC31 File Offset: 0x0003BE31
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float4x4 operator %(float lhs, float4x4 rhs)
		{
			return new float4x4(lhs % rhs.c0, lhs % rhs.c1, lhs % rhs.c2, lhs % rhs.c3);
		}

		// Token: 0x060015A0 RID: 5536 RVA: 0x0003DC68 File Offset: 0x0003BE68
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float4x4 operator ++(float4x4 val)
		{
			float4 @float = float4.op_Increment(val.c0);
			val.c0 = @float;
			float4 float2 = @float;
			@float = float4.op_Increment(val.c1);
			val.c1 = @float;
			float4 float3 = @float;
			@float = float4.op_Increment(val.c2);
			val.c2 = @float;
			float4 float4 = @float;
			@float = float4.op_Increment(val.c3);
			val.c3 = @float;
			return new float4x4(float2, float3, float4, @float);
		}

		// Token: 0x060015A1 RID: 5537 RVA: 0x0003DCE4 File Offset: 0x0003BEE4
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float4x4 operator --(float4x4 val)
		{
			float4 @float = float4.op_Decrement(val.c0);
			val.c0 = @float;
			float4 float2 = @float;
			@float = float4.op_Decrement(val.c1);
			val.c1 = @float;
			float4 float3 = @float;
			@float = float4.op_Decrement(val.c2);
			val.c2 = @float;
			float4 float4 = @float;
			@float = float4.op_Decrement(val.c3);
			val.c3 = @float;
			return new float4x4(float2, float3, float4, @float);
		}

		// Token: 0x060015A2 RID: 5538 RVA: 0x0003DD60 File Offset: 0x0003BF60
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool4x4 operator <(float4x4 lhs, float4x4 rhs)
		{
			return new bool4x4(lhs.c0 < rhs.c0, lhs.c1 < rhs.c1, lhs.c2 < rhs.c2, lhs.c3 < rhs.c3);
		}

		// Token: 0x060015A3 RID: 5539 RVA: 0x0003DDB6 File Offset: 0x0003BFB6
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool4x4 operator <(float4x4 lhs, float rhs)
		{
			return new bool4x4(lhs.c0 < rhs, lhs.c1 < rhs, lhs.c2 < rhs, lhs.c3 < rhs);
		}

		// Token: 0x060015A4 RID: 5540 RVA: 0x0003DDED File Offset: 0x0003BFED
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool4x4 operator <(float lhs, float4x4 rhs)
		{
			return new bool4x4(lhs < rhs.c0, lhs < rhs.c1, lhs < rhs.c2, lhs < rhs.c3);
		}

		// Token: 0x060015A5 RID: 5541 RVA: 0x0003DE24 File Offset: 0x0003C024
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool4x4 operator <=(float4x4 lhs, float4x4 rhs)
		{
			return new bool4x4(lhs.c0 <= rhs.c0, lhs.c1 <= rhs.c1, lhs.c2 <= rhs.c2, lhs.c3 <= rhs.c3);
		}

		// Token: 0x060015A6 RID: 5542 RVA: 0x0003DE7A File Offset: 0x0003C07A
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool4x4 operator <=(float4x4 lhs, float rhs)
		{
			return new bool4x4(lhs.c0 <= rhs, lhs.c1 <= rhs, lhs.c2 <= rhs, lhs.c3 <= rhs);
		}

		// Token: 0x060015A7 RID: 5543 RVA: 0x0003DEB1 File Offset: 0x0003C0B1
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool4x4 operator <=(float lhs, float4x4 rhs)
		{
			return new bool4x4(lhs <= rhs.c0, lhs <= rhs.c1, lhs <= rhs.c2, lhs <= rhs.c3);
		}

		// Token: 0x060015A8 RID: 5544 RVA: 0x0003DEE8 File Offset: 0x0003C0E8
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool4x4 operator >(float4x4 lhs, float4x4 rhs)
		{
			return new bool4x4(lhs.c0 > rhs.c0, lhs.c1 > rhs.c1, lhs.c2 > rhs.c2, lhs.c3 > rhs.c3);
		}

		// Token: 0x060015A9 RID: 5545 RVA: 0x0003DF3E File Offset: 0x0003C13E
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool4x4 operator >(float4x4 lhs, float rhs)
		{
			return new bool4x4(lhs.c0 > rhs, lhs.c1 > rhs, lhs.c2 > rhs, lhs.c3 > rhs);
		}

		// Token: 0x060015AA RID: 5546 RVA: 0x0003DF75 File Offset: 0x0003C175
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool4x4 operator >(float lhs, float4x4 rhs)
		{
			return new bool4x4(lhs > rhs.c0, lhs > rhs.c1, lhs > rhs.c2, lhs > rhs.c3);
		}

		// Token: 0x060015AB RID: 5547 RVA: 0x0003DFAC File Offset: 0x0003C1AC
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool4x4 operator >=(float4x4 lhs, float4x4 rhs)
		{
			return new bool4x4(lhs.c0 >= rhs.c0, lhs.c1 >= rhs.c1, lhs.c2 >= rhs.c2, lhs.c3 >= rhs.c3);
		}

		// Token: 0x060015AC RID: 5548 RVA: 0x0003E002 File Offset: 0x0003C202
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool4x4 operator >=(float4x4 lhs, float rhs)
		{
			return new bool4x4(lhs.c0 >= rhs, lhs.c1 >= rhs, lhs.c2 >= rhs, lhs.c3 >= rhs);
		}

		// Token: 0x060015AD RID: 5549 RVA: 0x0003E039 File Offset: 0x0003C239
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool4x4 operator >=(float lhs, float4x4 rhs)
		{
			return new bool4x4(lhs >= rhs.c0, lhs >= rhs.c1, lhs >= rhs.c2, lhs >= rhs.c3);
		}

		// Token: 0x060015AE RID: 5550 RVA: 0x0003E070 File Offset: 0x0003C270
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float4x4 operator -(float4x4 val)
		{
			return new float4x4(-val.c0, -val.c1, -val.c2, -val.c3);
		}

		// Token: 0x060015AF RID: 5551 RVA: 0x0003E0A3 File Offset: 0x0003C2A3
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float4x4 operator +(float4x4 val)
		{
			return new float4x4(+val.c0, +val.c1, +val.c2, +val.c3);
		}

		// Token: 0x060015B0 RID: 5552 RVA: 0x0003E0D8 File Offset: 0x0003C2D8
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool4x4 operator ==(float4x4 lhs, float4x4 rhs)
		{
			return new bool4x4(lhs.c0 == rhs.c0, lhs.c1 == rhs.c1, lhs.c2 == rhs.c2, lhs.c3 == rhs.c3);
		}

		// Token: 0x060015B1 RID: 5553 RVA: 0x0003E12E File Offset: 0x0003C32E
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool4x4 operator ==(float4x4 lhs, float rhs)
		{
			return new bool4x4(lhs.c0 == rhs, lhs.c1 == rhs, lhs.c2 == rhs, lhs.c3 == rhs);
		}

		// Token: 0x060015B2 RID: 5554 RVA: 0x0003E165 File Offset: 0x0003C365
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool4x4 operator ==(float lhs, float4x4 rhs)
		{
			return new bool4x4(lhs == rhs.c0, lhs == rhs.c1, lhs == rhs.c2, lhs == rhs.c3);
		}

		// Token: 0x060015B3 RID: 5555 RVA: 0x0003E19C File Offset: 0x0003C39C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool4x4 operator !=(float4x4 lhs, float4x4 rhs)
		{
			return new bool4x4(lhs.c0 != rhs.c0, lhs.c1 != rhs.c1, lhs.c2 != rhs.c2, lhs.c3 != rhs.c3);
		}

		// Token: 0x060015B4 RID: 5556 RVA: 0x0003E1F2 File Offset: 0x0003C3F2
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool4x4 operator !=(float4x4 lhs, float rhs)
		{
			return new bool4x4(lhs.c0 != rhs, lhs.c1 != rhs, lhs.c2 != rhs, lhs.c3 != rhs);
		}

		// Token: 0x060015B5 RID: 5557 RVA: 0x0003E229 File Offset: 0x0003C429
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool4x4 operator !=(float lhs, float4x4 rhs)
		{
			return new bool4x4(lhs != rhs.c0, lhs != rhs.c1, lhs != rhs.c2, lhs != rhs.c3);
		}

		// Token: 0x170005C7 RID: 1479
		public unsafe ref float4 this[int index]
		{
			get
			{
				fixed (float4x4* ptr = &this)
				{
					return ref *(float4*)(ptr + (IntPtr)index * (IntPtr)sizeof(float4) / (IntPtr)sizeof(float4x4));
				}
			}
		}

		// Token: 0x060015B7 RID: 5559 RVA: 0x0003E27C File Offset: 0x0003C47C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool Equals(float4x4 rhs)
		{
			return this.c0.Equals(rhs.c0) && this.c1.Equals(rhs.c1) && this.c2.Equals(rhs.c2) && this.c3.Equals(rhs.c3);
		}

		// Token: 0x060015B8 RID: 5560 RVA: 0x0003E2D8 File Offset: 0x0003C4D8
		public override bool Equals(object o)
		{
			if (o is float4x4)
			{
				float4x4 float4x = (float4x4)o;
				return this.Equals(float4x);
			}
			return false;
		}

		// Token: 0x060015B9 RID: 5561 RVA: 0x0003E2FD File Offset: 0x0003C4FD
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public override int GetHashCode()
		{
			return (int)math.hash(this);
		}

		// Token: 0x060015BA RID: 5562 RVA: 0x0003E30C File Offset: 0x0003C50C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public override string ToString()
		{
			return string.Format("float4x4({0}f, {1}f, {2}f, {3}f,  {4}f, {5}f, {6}f, {7}f,  {8}f, {9}f, {10}f, {11}f,  {12}f, {13}f, {14}f, {15}f)", new object[]
			{
				this.c0.x,
				this.c1.x,
				this.c2.x,
				this.c3.x,
				this.c0.y,
				this.c1.y,
				this.c2.y,
				this.c3.y,
				this.c0.z,
				this.c1.z,
				this.c2.z,
				this.c3.z,
				this.c0.w,
				this.c1.w,
				this.c2.w,
				this.c3.w
			});
		}

		// Token: 0x060015BB RID: 5563 RVA: 0x0003E464 File Offset: 0x0003C664
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public string ToString(string format, IFormatProvider formatProvider)
		{
			return string.Format("float4x4({0}f, {1}f, {2}f, {3}f,  {4}f, {5}f, {6}f, {7}f,  {8}f, {9}f, {10}f, {11}f,  {12}f, {13}f, {14}f, {15}f)", new object[]
			{
				this.c0.x.ToString(format, formatProvider),
				this.c1.x.ToString(format, formatProvider),
				this.c2.x.ToString(format, formatProvider),
				this.c3.x.ToString(format, formatProvider),
				this.c0.y.ToString(format, formatProvider),
				this.c1.y.ToString(format, formatProvider),
				this.c2.y.ToString(format, formatProvider),
				this.c3.y.ToString(format, formatProvider),
				this.c0.z.ToString(format, formatProvider),
				this.c1.z.ToString(format, formatProvider),
				this.c2.z.ToString(format, formatProvider),
				this.c3.z.ToString(format, formatProvider),
				this.c0.w.ToString(format, formatProvider),
				this.c1.w.ToString(format, formatProvider),
				this.c2.w.ToString(format, formatProvider),
				this.c3.w.ToString(format, formatProvider)
			});
		}

		// Token: 0x060015BC RID: 5564 RVA: 0x0003E5D9 File Offset: 0x0003C7D9
		public static implicit operator float4x4(Matrix4x4 m)
		{
			return new float4x4(m.GetColumn(0), m.GetColumn(1), m.GetColumn(2), m.GetColumn(3));
		}

		// Token: 0x060015BD RID: 5565 RVA: 0x0003E614 File Offset: 0x0003C814
		public static implicit operator Matrix4x4(float4x4 m)
		{
			return new Matrix4x4(m.c0, m.c1, m.c2, m.c3);
		}

		// Token: 0x060015BE RID: 5566 RVA: 0x0003E648 File Offset: 0x0003C848
		public float4x4(float3x3 rotation, float3 translation)
		{
			this.c0 = math.float4(rotation.c0, 0f);
			this.c1 = math.float4(rotation.c1, 0f);
			this.c2 = math.float4(rotation.c2, 0f);
			this.c3 = math.float4(translation, 1f);
		}

		// Token: 0x060015BF RID: 5567 RVA: 0x0003E6A8 File Offset: 0x0003C8A8
		public float4x4(quaternion rotation, float3 translation)
		{
			float3x3 float3x = math.float3x3(rotation);
			this.c0 = math.float4(float3x.c0, 0f);
			this.c1 = math.float4(float3x.c1, 0f);
			this.c2 = math.float4(float3x.c2, 0f);
			this.c3 = math.float4(translation, 1f);
		}

		// Token: 0x060015C0 RID: 5568 RVA: 0x0003E710 File Offset: 0x0003C910
		public float4x4(RigidTransform transform)
		{
			float3x3 float3x = math.float3x3(transform.rot);
			this.c0 = math.float4(float3x.c0, 0f);
			this.c1 = math.float4(float3x.c1, 0f);
			this.c2 = math.float4(float3x.c2, 0f);
			this.c3 = math.float4(transform.pos, 1f);
		}

		// Token: 0x060015C1 RID: 5569 RVA: 0x0003E784 File Offset: 0x0003C984
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float4x4 AxisAngle(float3 axis, float angle)
		{
			float num;
			float num2;
			math.sincos(angle, out num, out num2);
			float4 @float = math.float4(axis, 0f);
			float4 yzxx = @float.yzxx;
			float4 zxyx = @float.zxyx;
			float4 float2 = @float - @float * num2;
			float4 float3 = math.float4(@float.xyz * num, num2);
			uint4 @uint = math.uint4(0U, 0U, 2147483648U, 0U);
			uint4 uint2 = math.uint4(2147483648U, 0U, 0U, 0U);
			uint4 uint3 = math.uint4(0U, 2147483648U, 0U, 0U);
			uint4 uint4 = math.uint4(uint.MaxValue, uint.MaxValue, uint.MaxValue, 0U);
			return math.float4x4(@float.x * float2 + math.asfloat((math.asuint(float3.wzyx) ^ @uint) & uint4), @float.y * float2 + math.asfloat((math.asuint(float3.zwxx) ^ uint2) & uint4), @float.z * float2 + math.asfloat((math.asuint(float3.yxwx) ^ uint3) & uint4), math.float4(0f, 0f, 0f, 1f));
		}

		// Token: 0x060015C2 RID: 5570 RVA: 0x0003E8C0 File Offset: 0x0003CAC0
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float4x4 EulerXYZ(float3 xyz)
		{
			float3 @float;
			float3 float2;
			math.sincos(xyz, out @float, out float2);
			return math.float4x4(float2.y * float2.z, float2.z * @float.x * @float.y - float2.x * @float.z, float2.x * float2.z * @float.y + @float.x * @float.z, 0f, float2.y * @float.z, float2.x * float2.z + @float.x * @float.y * @float.z, float2.x * @float.y * @float.z - float2.z * @float.x, 0f, -@float.y, float2.y * @float.x, float2.x * float2.y, 0f, 0f, 0f, 0f, 1f);
		}

		// Token: 0x060015C3 RID: 5571 RVA: 0x0003E9C4 File Offset: 0x0003CBC4
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float4x4 EulerXZY(float3 xyz)
		{
			float3 @float;
			float3 float2;
			math.sincos(xyz, out @float, out float2);
			return math.float4x4(float2.y * float2.z, @float.x * @float.y - float2.x * float2.y * @float.z, float2.x * @float.y + float2.y * @float.x * @float.z, 0f, @float.z, float2.x * float2.z, -float2.z * @float.x, 0f, -float2.z * @float.y, float2.y * @float.x + float2.x * @float.y * @float.z, float2.x * float2.y - @float.x * @float.y * @float.z, 0f, 0f, 0f, 0f, 1f);
		}

		// Token: 0x060015C4 RID: 5572 RVA: 0x0003EAC8 File Offset: 0x0003CCC8
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float4x4 EulerYXZ(float3 xyz)
		{
			float3 @float;
			float3 float2;
			math.sincos(xyz, out @float, out float2);
			return math.float4x4(float2.y * float2.z - @float.x * @float.y * @float.z, -float2.x * @float.z, float2.z * @float.y + float2.y * @float.x * @float.z, 0f, float2.z * @float.x * @float.y + float2.y * @float.z, float2.x * float2.z, @float.y * @float.z - float2.y * float2.z * @float.x, 0f, -float2.x * @float.y, @float.x, float2.x * float2.y, 0f, 0f, 0f, 0f, 1f);
		}

		// Token: 0x060015C5 RID: 5573 RVA: 0x0003EBCC File Offset: 0x0003CDCC
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float4x4 EulerYZX(float3 xyz)
		{
			float3 @float;
			float3 float2;
			math.sincos(xyz, out @float, out float2);
			return math.float4x4(float2.y * float2.z, -@float.z, float2.z * @float.y, 0f, @float.x * @float.y + float2.x * float2.y * @float.z, float2.x * float2.z, float2.x * @float.y * @float.z - float2.y * @float.x, 0f, float2.y * @float.x * @float.z - float2.x * @float.y, float2.z * @float.x, float2.x * float2.y + @float.x * @float.y * @float.z, 0f, 0f, 0f, 0f, 1f);
		}

		// Token: 0x060015C6 RID: 5574 RVA: 0x0003ECD0 File Offset: 0x0003CED0
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float4x4 EulerZXY(float3 xyz)
		{
			float3 @float;
			float3 float2;
			math.sincos(xyz, out @float, out float2);
			return math.float4x4(float2.y * float2.z + @float.x * @float.y * @float.z, float2.z * @float.x * @float.y - float2.y * @float.z, float2.x * @float.y, 0f, float2.x * @float.z, float2.x * float2.z, -@float.x, 0f, float2.y * @float.x * @float.z - float2.z * @float.y, float2.y * float2.z * @float.x + @float.y * @float.z, float2.x * float2.y, 0f, 0f, 0f, 0f, 1f);
		}

		// Token: 0x060015C7 RID: 5575 RVA: 0x0003EDD4 File Offset: 0x0003CFD4
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float4x4 EulerZYX(float3 xyz)
		{
			float3 @float;
			float3 float2;
			math.sincos(xyz, out @float, out float2);
			return math.float4x4(float2.y * float2.z, -float2.y * @float.z, @float.y, 0f, float2.z * @float.x * @float.y + float2.x * @float.z, float2.x * float2.z - @float.x * @float.y * @float.z, -float2.y * @float.x, 0f, @float.x * @float.z - float2.x * float2.z * @float.y, float2.z * @float.x + float2.x * @float.y * @float.z, float2.x * float2.y, 0f, 0f, 0f, 0f, 1f);
		}

		// Token: 0x060015C8 RID: 5576 RVA: 0x0003EED7 File Offset: 0x0003D0D7
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float4x4 EulerXYZ(float x, float y, float z)
		{
			return float4x4.EulerXYZ(math.float3(x, y, z));
		}

		// Token: 0x060015C9 RID: 5577 RVA: 0x0003EEE6 File Offset: 0x0003D0E6
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float4x4 EulerXZY(float x, float y, float z)
		{
			return float4x4.EulerXZY(math.float3(x, y, z));
		}

		// Token: 0x060015CA RID: 5578 RVA: 0x0003EEF5 File Offset: 0x0003D0F5
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float4x4 EulerYXZ(float x, float y, float z)
		{
			return float4x4.EulerYXZ(math.float3(x, y, z));
		}

		// Token: 0x060015CB RID: 5579 RVA: 0x0003EF04 File Offset: 0x0003D104
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float4x4 EulerYZX(float x, float y, float z)
		{
			return float4x4.EulerYZX(math.float3(x, y, z));
		}

		// Token: 0x060015CC RID: 5580 RVA: 0x0003EF13 File Offset: 0x0003D113
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float4x4 EulerZXY(float x, float y, float z)
		{
			return float4x4.EulerZXY(math.float3(x, y, z));
		}

		// Token: 0x060015CD RID: 5581 RVA: 0x0003EF22 File Offset: 0x0003D122
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float4x4 EulerZYX(float x, float y, float z)
		{
			return float4x4.EulerZYX(math.float3(x, y, z));
		}

		// Token: 0x060015CE RID: 5582 RVA: 0x0003EF34 File Offset: 0x0003D134
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float4x4 Euler(float3 xyz, math.RotationOrder order = math.RotationOrder.ZXY)
		{
			switch (order)
			{
			case math.RotationOrder.XYZ:
				return float4x4.EulerXYZ(xyz);
			case math.RotationOrder.XZY:
				return float4x4.EulerXZY(xyz);
			case math.RotationOrder.YXZ:
				return float4x4.EulerYXZ(xyz);
			case math.RotationOrder.YZX:
				return float4x4.EulerYZX(xyz);
			case math.RotationOrder.ZXY:
				return float4x4.EulerZXY(xyz);
			case math.RotationOrder.ZYX:
				return float4x4.EulerZYX(xyz);
			default:
				return float4x4.identity;
			}
		}

		// Token: 0x060015CF RID: 5583 RVA: 0x0003EF90 File Offset: 0x0003D190
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float4x4 Euler(float x, float y, float z, math.RotationOrder order = math.RotationOrder.ZXY)
		{
			return float4x4.Euler(math.float3(x, y, z), order);
		}

		// Token: 0x060015D0 RID: 5584 RVA: 0x0003EFA0 File Offset: 0x0003D1A0
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float4x4 RotateX(float angle)
		{
			float num;
			float num2;
			math.sincos(angle, out num, out num2);
			return math.float4x4(1f, 0f, 0f, 0f, 0f, num2, -num, 0f, 0f, num, num2, 0f, 0f, 0f, 0f, 1f);
		}

		// Token: 0x060015D1 RID: 5585 RVA: 0x0003F000 File Offset: 0x0003D200
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float4x4 RotateY(float angle)
		{
			float num;
			float num2;
			math.sincos(angle, out num, out num2);
			return math.float4x4(num2, 0f, num, 0f, 0f, 1f, 0f, 0f, -num, 0f, num2, 0f, 0f, 0f, 0f, 1f);
		}

		// Token: 0x060015D2 RID: 5586 RVA: 0x0003F060 File Offset: 0x0003D260
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float4x4 RotateZ(float angle)
		{
			float num;
			float num2;
			math.sincos(angle, out num, out num2);
			return math.float4x4(num2, -num, 0f, 0f, num, num2, 0f, 0f, 0f, 0f, 1f, 0f, 0f, 0f, 0f, 1f);
		}

		// Token: 0x060015D3 RID: 5587 RVA: 0x0003F0C0 File Offset: 0x0003D2C0
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float4x4 Scale(float s)
		{
			return math.float4x4(s, 0f, 0f, 0f, 0f, s, 0f, 0f, 0f, 0f, s, 0f, 0f, 0f, 0f, 1f);
		}

		// Token: 0x060015D4 RID: 5588 RVA: 0x0003F118 File Offset: 0x0003D318
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float4x4 Scale(float x, float y, float z)
		{
			return math.float4x4(x, 0f, 0f, 0f, 0f, y, 0f, 0f, 0f, 0f, z, 0f, 0f, 0f, 0f, 1f);
		}

		// Token: 0x060015D5 RID: 5589 RVA: 0x0003F16E File Offset: 0x0003D36E
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float4x4 Scale(float3 scales)
		{
			return float4x4.Scale(scales.x, scales.y, scales.z);
		}

		// Token: 0x060015D6 RID: 5590 RVA: 0x0003F188 File Offset: 0x0003D388
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float4x4 Translate(float3 vector)
		{
			return math.float4x4(math.float4(1f, 0f, 0f, 0f), math.float4(0f, 1f, 0f, 0f), math.float4(0f, 0f, 1f, 0f), math.float4(vector.x, vector.y, vector.z, 1f));
		}

		// Token: 0x060015D7 RID: 5591 RVA: 0x0003F204 File Offset: 0x0003D404
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float4x4 LookAt(float3 eye, float3 target, float3 up)
		{
			float3x3 float3x = float3x3.LookRotation(math.normalize(target - eye), up);
			float4x4 float4x;
			float4x.c0 = math.float4(float3x.c0, 0f);
			float4x.c1 = math.float4(float3x.c1, 0f);
			float4x.c2 = math.float4(float3x.c2, 0f);
			float4x.c3 = math.float4(eye, 1f);
			return float4x;
		}

		// Token: 0x060015D8 RID: 5592 RVA: 0x0003F27C File Offset: 0x0003D47C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float4x4 Ortho(float width, float height, float near, float far)
		{
			float num = 1f / width;
			float num2 = 1f / height;
			float num3 = 1f / (far - near);
			return math.float4x4(2f * num, 0f, 0f, 0f, 0f, 2f * num2, 0f, 0f, 0f, 0f, -2f * num3, -(far + near) * num3, 0f, 0f, 0f, 1f);
		}

		// Token: 0x060015D9 RID: 5593 RVA: 0x0003F300 File Offset: 0x0003D500
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float4x4 OrthoOffCenter(float left, float right, float bottom, float top, float near, float far)
		{
			float num = 1f / (right - left);
			float num2 = 1f / (top - bottom);
			float num3 = 1f / (far - near);
			return math.float4x4(2f * num, 0f, 0f, -(right + left) * num, 0f, 2f * num2, 0f, -(top + bottom) * num2, 0f, 0f, -2f * num3, -(far + near) * num3, 0f, 0f, 0f, 1f);
		}

		// Token: 0x060015DA RID: 5594 RVA: 0x0003F390 File Offset: 0x0003D590
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float4x4 PerspectiveFov(float verticalFov, float aspect, float near, float far)
		{
			float num = 1f / math.tan(verticalFov * 0.5f);
			float num2 = 1f / (near - far);
			return math.float4x4(num / aspect, 0f, 0f, 0f, 0f, num, 0f, 0f, 0f, 0f, (far + near) * num2, 2f * near * far * num2, 0f, 0f, -1f, 0f);
		}

		// Token: 0x060015DB RID: 5595 RVA: 0x0003F410 File Offset: 0x0003D610
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float4x4 PerspectiveOffCenter(float left, float right, float bottom, float top, float near, float far)
		{
			float num = 1f / (near - far);
			float num2 = 1f / (right - left);
			float num3 = 1f / (top - bottom);
			return math.float4x4(2f * near * num2, 0f, (left + right) * num2, 0f, 0f, 2f * near * num3, (bottom + top) * num3, 0f, 0f, 0f, (far + near) * num, 2f * near * far * num, 0f, 0f, -1f, 0f);
		}

		// Token: 0x060015DC RID: 5596 RVA: 0x0003F4A8 File Offset: 0x0003D6A8
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float4x4 TRS(float3 translation, quaternion rotation, float3 scale)
		{
			float3x3 float3x = math.float3x3(rotation);
			return math.float4x4(math.float4(float3x.c0 * scale.x, 0f), math.float4(float3x.c1 * scale.y, 0f), math.float4(float3x.c2 * scale.z, 0f), math.float4(translation, 1f));
		}

		// Token: 0x040000A9 RID: 169
		public float4 c0;

		// Token: 0x040000AA RID: 170
		public float4 c1;

		// Token: 0x040000AB RID: 171
		public float4 c2;

		// Token: 0x040000AC RID: 172
		public float4 c3;

		// Token: 0x040000AD RID: 173
		public static readonly float4x4 identity = new float4x4(1f, 0f, 0f, 0f, 0f, 1f, 0f, 0f, 0f, 0f, 1f, 0f, 0f, 0f, 0f, 1f);

		// Token: 0x040000AE RID: 174
		public static readonly float4x4 zero;
	}
}
