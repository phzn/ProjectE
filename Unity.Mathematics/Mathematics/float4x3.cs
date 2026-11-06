using System;
using System.Runtime.CompilerServices;
using Unity.IL2CPP.CompilerServices;

namespace Unity.Mathematics
{
	// Token: 0x02000028 RID: 40
	[Il2CppEagerStaticClassConstruction]
	[Serializable]
	public struct float4x3 : IEquatable<float4x3>, IFormattable
	{
		// Token: 0x0600153E RID: 5438 RVA: 0x0003C835 File Offset: 0x0003AA35
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public float4x3(float4 c0, float4 c1, float4 c2)
		{
			this.c0 = c0;
			this.c1 = c1;
			this.c2 = c2;
		}

		// Token: 0x0600153F RID: 5439 RVA: 0x0003C84C File Offset: 0x0003AA4C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public float4x3(float m00, float m01, float m02, float m10, float m11, float m12, float m20, float m21, float m22, float m30, float m31, float m32)
		{
			this.c0 = new float4(m00, m10, m20, m30);
			this.c1 = new float4(m01, m11, m21, m31);
			this.c2 = new float4(m02, m12, m22, m32);
		}

		// Token: 0x06001540 RID: 5440 RVA: 0x0003C884 File Offset: 0x0003AA84
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public float4x3(float v)
		{
			this.c0 = v;
			this.c1 = v;
			this.c2 = v;
		}

		// Token: 0x06001541 RID: 5441 RVA: 0x0003C8AC File Offset: 0x0003AAAC
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public float4x3(bool v)
		{
			this.c0 = math.select(new float4(0f), new float4(1f), v);
			this.c1 = math.select(new float4(0f), new float4(1f), v);
			this.c2 = math.select(new float4(0f), new float4(1f), v);
		}

		// Token: 0x06001542 RID: 5442 RVA: 0x0003C91C File Offset: 0x0003AB1C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public float4x3(bool4x3 v)
		{
			this.c0 = math.select(new float4(0f), new float4(1f), v.c0);
			this.c1 = math.select(new float4(0f), new float4(1f), v.c1);
			this.c2 = math.select(new float4(0f), new float4(1f), v.c2);
		}

		// Token: 0x06001543 RID: 5443 RVA: 0x0003C998 File Offset: 0x0003AB98
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public float4x3(int v)
		{
			this.c0 = v;
			this.c1 = v;
			this.c2 = v;
		}

		// Token: 0x06001544 RID: 5444 RVA: 0x0003C9BE File Offset: 0x0003ABBE
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public float4x3(int4x3 v)
		{
			this.c0 = v.c0;
			this.c1 = v.c1;
			this.c2 = v.c2;
		}

		// Token: 0x06001545 RID: 5445 RVA: 0x0003C9F3 File Offset: 0x0003ABF3
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public float4x3(uint v)
		{
			this.c0 = v;
			this.c1 = v;
			this.c2 = v;
		}

		// Token: 0x06001546 RID: 5446 RVA: 0x0003CA19 File Offset: 0x0003AC19
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public float4x3(uint4x3 v)
		{
			this.c0 = v.c0;
			this.c1 = v.c1;
			this.c2 = v.c2;
		}

		// Token: 0x06001547 RID: 5447 RVA: 0x0003CA4E File Offset: 0x0003AC4E
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public float4x3(double v)
		{
			this.c0 = (float4)v;
			this.c1 = (float4)v;
			this.c2 = (float4)v;
		}

		// Token: 0x06001548 RID: 5448 RVA: 0x0003CA74 File Offset: 0x0003AC74
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public float4x3(double4x3 v)
		{
			this.c0 = (float4)v.c0;
			this.c1 = (float4)v.c1;
			this.c2 = (float4)v.c2;
		}

		// Token: 0x06001549 RID: 5449 RVA: 0x0003CAA9 File Offset: 0x0003ACA9
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static implicit operator float4x3(float v)
		{
			return new float4x3(v);
		}

		// Token: 0x0600154A RID: 5450 RVA: 0x0003CAB1 File Offset: 0x0003ACB1
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static explicit operator float4x3(bool v)
		{
			return new float4x3(v);
		}

		// Token: 0x0600154B RID: 5451 RVA: 0x0003CAB9 File Offset: 0x0003ACB9
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static explicit operator float4x3(bool4x3 v)
		{
			return new float4x3(v);
		}

		// Token: 0x0600154C RID: 5452 RVA: 0x0003CAC1 File Offset: 0x0003ACC1
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static implicit operator float4x3(int v)
		{
			return new float4x3(v);
		}

		// Token: 0x0600154D RID: 5453 RVA: 0x0003CAC9 File Offset: 0x0003ACC9
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static implicit operator float4x3(int4x3 v)
		{
			return new float4x3(v);
		}

		// Token: 0x0600154E RID: 5454 RVA: 0x0003CAD1 File Offset: 0x0003ACD1
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static implicit operator float4x3(uint v)
		{
			return new float4x3(v);
		}

		// Token: 0x0600154F RID: 5455 RVA: 0x0003CAD9 File Offset: 0x0003ACD9
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static implicit operator float4x3(uint4x3 v)
		{
			return new float4x3(v);
		}

		// Token: 0x06001550 RID: 5456 RVA: 0x0003CAE1 File Offset: 0x0003ACE1
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static explicit operator float4x3(double v)
		{
			return new float4x3(v);
		}

		// Token: 0x06001551 RID: 5457 RVA: 0x0003CAE9 File Offset: 0x0003ACE9
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static explicit operator float4x3(double4x3 v)
		{
			return new float4x3(v);
		}

		// Token: 0x06001552 RID: 5458 RVA: 0x0003CAF1 File Offset: 0x0003ACF1
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float4x3 operator *(float4x3 lhs, float4x3 rhs)
		{
			return new float4x3(lhs.c0 * rhs.c0, lhs.c1 * rhs.c1, lhs.c2 * rhs.c2);
		}

		// Token: 0x06001553 RID: 5459 RVA: 0x0003CB2B File Offset: 0x0003AD2B
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float4x3 operator *(float4x3 lhs, float rhs)
		{
			return new float4x3(lhs.c0 * rhs, lhs.c1 * rhs, lhs.c2 * rhs);
		}

		// Token: 0x06001554 RID: 5460 RVA: 0x0003CB56 File Offset: 0x0003AD56
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float4x3 operator *(float lhs, float4x3 rhs)
		{
			return new float4x3(lhs * rhs.c0, lhs * rhs.c1, lhs * rhs.c2);
		}

		// Token: 0x06001555 RID: 5461 RVA: 0x0003CB81 File Offset: 0x0003AD81
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float4x3 operator +(float4x3 lhs, float4x3 rhs)
		{
			return new float4x3(lhs.c0 + rhs.c0, lhs.c1 + rhs.c1, lhs.c2 + rhs.c2);
		}

		// Token: 0x06001556 RID: 5462 RVA: 0x0003CBBB File Offset: 0x0003ADBB
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float4x3 operator +(float4x3 lhs, float rhs)
		{
			return new float4x3(lhs.c0 + rhs, lhs.c1 + rhs, lhs.c2 + rhs);
		}

		// Token: 0x06001557 RID: 5463 RVA: 0x0003CBE6 File Offset: 0x0003ADE6
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float4x3 operator +(float lhs, float4x3 rhs)
		{
			return new float4x3(lhs + rhs.c0, lhs + rhs.c1, lhs + rhs.c2);
		}

		// Token: 0x06001558 RID: 5464 RVA: 0x0003CC11 File Offset: 0x0003AE11
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float4x3 operator -(float4x3 lhs, float4x3 rhs)
		{
			return new float4x3(lhs.c0 - rhs.c0, lhs.c1 - rhs.c1, lhs.c2 - rhs.c2);
		}

		// Token: 0x06001559 RID: 5465 RVA: 0x0003CC4B File Offset: 0x0003AE4B
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float4x3 operator -(float4x3 lhs, float rhs)
		{
			return new float4x3(lhs.c0 - rhs, lhs.c1 - rhs, lhs.c2 - rhs);
		}

		// Token: 0x0600155A RID: 5466 RVA: 0x0003CC76 File Offset: 0x0003AE76
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float4x3 operator -(float lhs, float4x3 rhs)
		{
			return new float4x3(lhs - rhs.c0, lhs - rhs.c1, lhs - rhs.c2);
		}

		// Token: 0x0600155B RID: 5467 RVA: 0x0003CCA1 File Offset: 0x0003AEA1
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float4x3 operator /(float4x3 lhs, float4x3 rhs)
		{
			return new float4x3(lhs.c0 / rhs.c0, lhs.c1 / rhs.c1, lhs.c2 / rhs.c2);
		}

		// Token: 0x0600155C RID: 5468 RVA: 0x0003CCDB File Offset: 0x0003AEDB
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float4x3 operator /(float4x3 lhs, float rhs)
		{
			return new float4x3(lhs.c0 / rhs, lhs.c1 / rhs, lhs.c2 / rhs);
		}

		// Token: 0x0600155D RID: 5469 RVA: 0x0003CD06 File Offset: 0x0003AF06
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float4x3 operator /(float lhs, float4x3 rhs)
		{
			return new float4x3(lhs / rhs.c0, lhs / rhs.c1, lhs / rhs.c2);
		}

		// Token: 0x0600155E RID: 5470 RVA: 0x0003CD31 File Offset: 0x0003AF31
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float4x3 operator %(float4x3 lhs, float4x3 rhs)
		{
			return new float4x3(lhs.c0 % rhs.c0, lhs.c1 % rhs.c1, lhs.c2 % rhs.c2);
		}

		// Token: 0x0600155F RID: 5471 RVA: 0x0003CD6B File Offset: 0x0003AF6B
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float4x3 operator %(float4x3 lhs, float rhs)
		{
			return new float4x3(lhs.c0 % rhs, lhs.c1 % rhs, lhs.c2 % rhs);
		}

		// Token: 0x06001560 RID: 5472 RVA: 0x0003CD96 File Offset: 0x0003AF96
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float4x3 operator %(float lhs, float4x3 rhs)
		{
			return new float4x3(lhs % rhs.c0, lhs % rhs.c1, lhs % rhs.c2);
		}

		// Token: 0x06001561 RID: 5473 RVA: 0x0003CDC4 File Offset: 0x0003AFC4
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float4x3 operator ++(float4x3 val)
		{
			float4 @float = float4.op_Increment(val.c0);
			val.c0 = @float;
			float4 float2 = @float;
			@float = float4.op_Increment(val.c1);
			val.c1 = @float;
			float4 float3 = @float;
			@float = float4.op_Increment(val.c2);
			val.c2 = @float;
			return new float4x3(float2, float3, @float);
		}

		// Token: 0x06001562 RID: 5474 RVA: 0x0003CE24 File Offset: 0x0003B024
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float4x3 operator --(float4x3 val)
		{
			float4 @float = float4.op_Decrement(val.c0);
			val.c0 = @float;
			float4 float2 = @float;
			@float = float4.op_Decrement(val.c1);
			val.c1 = @float;
			float4 float3 = @float;
			@float = float4.op_Decrement(val.c2);
			val.c2 = @float;
			return new float4x3(float2, float3, @float);
		}

		// Token: 0x06001563 RID: 5475 RVA: 0x0003CE84 File Offset: 0x0003B084
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool4x3 operator <(float4x3 lhs, float4x3 rhs)
		{
			return new bool4x3(lhs.c0 < rhs.c0, lhs.c1 < rhs.c1, lhs.c2 < rhs.c2);
		}

		// Token: 0x06001564 RID: 5476 RVA: 0x0003CEBE File Offset: 0x0003B0BE
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool4x3 operator <(float4x3 lhs, float rhs)
		{
			return new bool4x3(lhs.c0 < rhs, lhs.c1 < rhs, lhs.c2 < rhs);
		}

		// Token: 0x06001565 RID: 5477 RVA: 0x0003CEE9 File Offset: 0x0003B0E9
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool4x3 operator <(float lhs, float4x3 rhs)
		{
			return new bool4x3(lhs < rhs.c0, lhs < rhs.c1, lhs < rhs.c2);
		}

		// Token: 0x06001566 RID: 5478 RVA: 0x0003CF14 File Offset: 0x0003B114
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool4x3 operator <=(float4x3 lhs, float4x3 rhs)
		{
			return new bool4x3(lhs.c0 <= rhs.c0, lhs.c1 <= rhs.c1, lhs.c2 <= rhs.c2);
		}

		// Token: 0x06001567 RID: 5479 RVA: 0x0003CF4E File Offset: 0x0003B14E
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool4x3 operator <=(float4x3 lhs, float rhs)
		{
			return new bool4x3(lhs.c0 <= rhs, lhs.c1 <= rhs, lhs.c2 <= rhs);
		}

		// Token: 0x06001568 RID: 5480 RVA: 0x0003CF79 File Offset: 0x0003B179
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool4x3 operator <=(float lhs, float4x3 rhs)
		{
			return new bool4x3(lhs <= rhs.c0, lhs <= rhs.c1, lhs <= rhs.c2);
		}

		// Token: 0x06001569 RID: 5481 RVA: 0x0003CFA4 File Offset: 0x0003B1A4
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool4x3 operator >(float4x3 lhs, float4x3 rhs)
		{
			return new bool4x3(lhs.c0 > rhs.c0, lhs.c1 > rhs.c1, lhs.c2 > rhs.c2);
		}

		// Token: 0x0600156A RID: 5482 RVA: 0x0003CFDE File Offset: 0x0003B1DE
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool4x3 operator >(float4x3 lhs, float rhs)
		{
			return new bool4x3(lhs.c0 > rhs, lhs.c1 > rhs, lhs.c2 > rhs);
		}

		// Token: 0x0600156B RID: 5483 RVA: 0x0003D009 File Offset: 0x0003B209
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool4x3 operator >(float lhs, float4x3 rhs)
		{
			return new bool4x3(lhs > rhs.c0, lhs > rhs.c1, lhs > rhs.c2);
		}

		// Token: 0x0600156C RID: 5484 RVA: 0x0003D034 File Offset: 0x0003B234
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool4x3 operator >=(float4x3 lhs, float4x3 rhs)
		{
			return new bool4x3(lhs.c0 >= rhs.c0, lhs.c1 >= rhs.c1, lhs.c2 >= rhs.c2);
		}

		// Token: 0x0600156D RID: 5485 RVA: 0x0003D06E File Offset: 0x0003B26E
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool4x3 operator >=(float4x3 lhs, float rhs)
		{
			return new bool4x3(lhs.c0 >= rhs, lhs.c1 >= rhs, lhs.c2 >= rhs);
		}

		// Token: 0x0600156E RID: 5486 RVA: 0x0003D099 File Offset: 0x0003B299
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool4x3 operator >=(float lhs, float4x3 rhs)
		{
			return new bool4x3(lhs >= rhs.c0, lhs >= rhs.c1, lhs >= rhs.c2);
		}

		// Token: 0x0600156F RID: 5487 RVA: 0x0003D0C4 File Offset: 0x0003B2C4
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float4x3 operator -(float4x3 val)
		{
			return new float4x3(-val.c0, -val.c1, -val.c2);
		}

		// Token: 0x06001570 RID: 5488 RVA: 0x0003D0EC File Offset: 0x0003B2EC
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float4x3 operator +(float4x3 val)
		{
			return new float4x3(+val.c0, +val.c1, +val.c2);
		}

		// Token: 0x06001571 RID: 5489 RVA: 0x0003D114 File Offset: 0x0003B314
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool4x3 operator ==(float4x3 lhs, float4x3 rhs)
		{
			return new bool4x3(lhs.c0 == rhs.c0, lhs.c1 == rhs.c1, lhs.c2 == rhs.c2);
		}

		// Token: 0x06001572 RID: 5490 RVA: 0x0003D14E File Offset: 0x0003B34E
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool4x3 operator ==(float4x3 lhs, float rhs)
		{
			return new bool4x3(lhs.c0 == rhs, lhs.c1 == rhs, lhs.c2 == rhs);
		}

		// Token: 0x06001573 RID: 5491 RVA: 0x0003D179 File Offset: 0x0003B379
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool4x3 operator ==(float lhs, float4x3 rhs)
		{
			return new bool4x3(lhs == rhs.c0, lhs == rhs.c1, lhs == rhs.c2);
		}

		// Token: 0x06001574 RID: 5492 RVA: 0x0003D1A4 File Offset: 0x0003B3A4
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool4x3 operator !=(float4x3 lhs, float4x3 rhs)
		{
			return new bool4x3(lhs.c0 != rhs.c0, lhs.c1 != rhs.c1, lhs.c2 != rhs.c2);
		}

		// Token: 0x06001575 RID: 5493 RVA: 0x0003D1DE File Offset: 0x0003B3DE
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool4x3 operator !=(float4x3 lhs, float rhs)
		{
			return new bool4x3(lhs.c0 != rhs, lhs.c1 != rhs, lhs.c2 != rhs);
		}

		// Token: 0x06001576 RID: 5494 RVA: 0x0003D209 File Offset: 0x0003B409
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool4x3 operator !=(float lhs, float4x3 rhs)
		{
			return new bool4x3(lhs != rhs.c0, lhs != rhs.c1, lhs != rhs.c2);
		}

		// Token: 0x170005C6 RID: 1478
		public unsafe ref float4 this[int index]
		{
			get
			{
				fixed (float4x3* ptr = &this)
				{
					return ref *(float4*)(ptr + (IntPtr)index * (IntPtr)sizeof(float4) / (IntPtr)sizeof(float4x3));
				}
			}
		}

		// Token: 0x06001578 RID: 5496 RVA: 0x0003D24F File Offset: 0x0003B44F
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool Equals(float4x3 rhs)
		{
			return this.c0.Equals(rhs.c0) && this.c1.Equals(rhs.c1) && this.c2.Equals(rhs.c2);
		}

		// Token: 0x06001579 RID: 5497 RVA: 0x0003D28C File Offset: 0x0003B48C
		public override bool Equals(object o)
		{
			if (o is float4x3)
			{
				float4x3 float4x = (float4x3)o;
				return this.Equals(float4x);
			}
			return false;
		}

		// Token: 0x0600157A RID: 5498 RVA: 0x0003D2B1 File Offset: 0x0003B4B1
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public override int GetHashCode()
		{
			return (int)math.hash(this);
		}

		// Token: 0x0600157B RID: 5499 RVA: 0x0003D2C0 File Offset: 0x0003B4C0
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public override string ToString()
		{
			return string.Format("float4x3({0}f, {1}f, {2}f,  {3}f, {4}f, {5}f,  {6}f, {7}f, {8}f,  {9}f, {10}f, {11}f)", new object[]
			{
				this.c0.x,
				this.c1.x,
				this.c2.x,
				this.c0.y,
				this.c1.y,
				this.c2.y,
				this.c0.z,
				this.c1.z,
				this.c2.z,
				this.c0.w,
				this.c1.w,
				this.c2.w
			});
		}

		// Token: 0x0600157C RID: 5500 RVA: 0x0003D3C8 File Offset: 0x0003B5C8
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public string ToString(string format, IFormatProvider formatProvider)
		{
			return string.Format("float4x3({0}f, {1}f, {2}f,  {3}f, {4}f, {5}f,  {6}f, {7}f, {8}f,  {9}f, {10}f, {11}f)", new object[]
			{
				this.c0.x.ToString(format, formatProvider),
				this.c1.x.ToString(format, formatProvider),
				this.c2.x.ToString(format, formatProvider),
				this.c0.y.ToString(format, formatProvider),
				this.c1.y.ToString(format, formatProvider),
				this.c2.y.ToString(format, formatProvider),
				this.c0.z.ToString(format, formatProvider),
				this.c1.z.ToString(format, formatProvider),
				this.c2.z.ToString(format, formatProvider),
				this.c0.w.ToString(format, formatProvider),
				this.c1.w.ToString(format, formatProvider),
				this.c2.w.ToString(format, formatProvider)
			});
		}

		// Token: 0x040000A5 RID: 165
		public float4 c0;

		// Token: 0x040000A6 RID: 166
		public float4 c1;

		// Token: 0x040000A7 RID: 167
		public float4 c2;

		// Token: 0x040000A8 RID: 168
		public static readonly float4x3 zero;
	}
}
