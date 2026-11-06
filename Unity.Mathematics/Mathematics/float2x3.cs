using System;
using System.Runtime.CompilerServices;
using Unity.IL2CPP.CompilerServices;

namespace Unity.Mathematics
{
	// Token: 0x02000020 RID: 32
	[Il2CppEagerStaticClassConstruction]
	[Serializable]
	public struct float2x3 : IEquatable<float2x3>, IFormattable
	{
		// Token: 0x06001108 RID: 4360 RVA: 0x00031D3E File Offset: 0x0002FF3E
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public float2x3(float2 c0, float2 c1, float2 c2)
		{
			this.c0 = c0;
			this.c1 = c1;
			this.c2 = c2;
		}

		// Token: 0x06001109 RID: 4361 RVA: 0x00031D55 File Offset: 0x0002FF55
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public float2x3(float m00, float m01, float m02, float m10, float m11, float m12)
		{
			this.c0 = new float2(m00, m10);
			this.c1 = new float2(m01, m11);
			this.c2 = new float2(m02, m12);
		}

		// Token: 0x0600110A RID: 4362 RVA: 0x00031D81 File Offset: 0x0002FF81
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public float2x3(float v)
		{
			this.c0 = v;
			this.c1 = v;
			this.c2 = v;
		}

		// Token: 0x0600110B RID: 4363 RVA: 0x00031DA8 File Offset: 0x0002FFA8
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public float2x3(bool v)
		{
			this.c0 = math.select(new float2(0f), new float2(1f), v);
			this.c1 = math.select(new float2(0f), new float2(1f), v);
			this.c2 = math.select(new float2(0f), new float2(1f), v);
		}

		// Token: 0x0600110C RID: 4364 RVA: 0x00031E18 File Offset: 0x00030018
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public float2x3(bool2x3 v)
		{
			this.c0 = math.select(new float2(0f), new float2(1f), v.c0);
			this.c1 = math.select(new float2(0f), new float2(1f), v.c1);
			this.c2 = math.select(new float2(0f), new float2(1f), v.c2);
		}

		// Token: 0x0600110D RID: 4365 RVA: 0x00031E94 File Offset: 0x00030094
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public float2x3(int v)
		{
			this.c0 = v;
			this.c1 = v;
			this.c2 = v;
		}

		// Token: 0x0600110E RID: 4366 RVA: 0x00031EBA File Offset: 0x000300BA
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public float2x3(int2x3 v)
		{
			this.c0 = v.c0;
			this.c1 = v.c1;
			this.c2 = v.c2;
		}

		// Token: 0x0600110F RID: 4367 RVA: 0x00031EEF File Offset: 0x000300EF
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public float2x3(uint v)
		{
			this.c0 = v;
			this.c1 = v;
			this.c2 = v;
		}

		// Token: 0x06001110 RID: 4368 RVA: 0x00031F15 File Offset: 0x00030115
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public float2x3(uint2x3 v)
		{
			this.c0 = v.c0;
			this.c1 = v.c1;
			this.c2 = v.c2;
		}

		// Token: 0x06001111 RID: 4369 RVA: 0x00031F4A File Offset: 0x0003014A
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public float2x3(double v)
		{
			this.c0 = (float2)v;
			this.c1 = (float2)v;
			this.c2 = (float2)v;
		}

		// Token: 0x06001112 RID: 4370 RVA: 0x00031F70 File Offset: 0x00030170
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public float2x3(double2x3 v)
		{
			this.c0 = (float2)v.c0;
			this.c1 = (float2)v.c1;
			this.c2 = (float2)v.c2;
		}

		// Token: 0x06001113 RID: 4371 RVA: 0x00031FA5 File Offset: 0x000301A5
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static implicit operator float2x3(float v)
		{
			return new float2x3(v);
		}

		// Token: 0x06001114 RID: 4372 RVA: 0x00031FAD File Offset: 0x000301AD
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static explicit operator float2x3(bool v)
		{
			return new float2x3(v);
		}

		// Token: 0x06001115 RID: 4373 RVA: 0x00031FB5 File Offset: 0x000301B5
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static explicit operator float2x3(bool2x3 v)
		{
			return new float2x3(v);
		}

		// Token: 0x06001116 RID: 4374 RVA: 0x00031FBD File Offset: 0x000301BD
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static implicit operator float2x3(int v)
		{
			return new float2x3(v);
		}

		// Token: 0x06001117 RID: 4375 RVA: 0x00031FC5 File Offset: 0x000301C5
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static implicit operator float2x3(int2x3 v)
		{
			return new float2x3(v);
		}

		// Token: 0x06001118 RID: 4376 RVA: 0x00031FCD File Offset: 0x000301CD
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static implicit operator float2x3(uint v)
		{
			return new float2x3(v);
		}

		// Token: 0x06001119 RID: 4377 RVA: 0x00031FD5 File Offset: 0x000301D5
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static implicit operator float2x3(uint2x3 v)
		{
			return new float2x3(v);
		}

		// Token: 0x0600111A RID: 4378 RVA: 0x00031FDD File Offset: 0x000301DD
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static explicit operator float2x3(double v)
		{
			return new float2x3(v);
		}

		// Token: 0x0600111B RID: 4379 RVA: 0x00031FE5 File Offset: 0x000301E5
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static explicit operator float2x3(double2x3 v)
		{
			return new float2x3(v);
		}

		// Token: 0x0600111C RID: 4380 RVA: 0x00031FED File Offset: 0x000301ED
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float2x3 operator *(float2x3 lhs, float2x3 rhs)
		{
			return new float2x3(lhs.c0 * rhs.c0, lhs.c1 * rhs.c1, lhs.c2 * rhs.c2);
		}

		// Token: 0x0600111D RID: 4381 RVA: 0x00032027 File Offset: 0x00030227
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float2x3 operator *(float2x3 lhs, float rhs)
		{
			return new float2x3(lhs.c0 * rhs, lhs.c1 * rhs, lhs.c2 * rhs);
		}

		// Token: 0x0600111E RID: 4382 RVA: 0x00032052 File Offset: 0x00030252
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float2x3 operator *(float lhs, float2x3 rhs)
		{
			return new float2x3(lhs * rhs.c0, lhs * rhs.c1, lhs * rhs.c2);
		}

		// Token: 0x0600111F RID: 4383 RVA: 0x0003207D File Offset: 0x0003027D
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float2x3 operator +(float2x3 lhs, float2x3 rhs)
		{
			return new float2x3(lhs.c0 + rhs.c0, lhs.c1 + rhs.c1, lhs.c2 + rhs.c2);
		}

		// Token: 0x06001120 RID: 4384 RVA: 0x000320B7 File Offset: 0x000302B7
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float2x3 operator +(float2x3 lhs, float rhs)
		{
			return new float2x3(lhs.c0 + rhs, lhs.c1 + rhs, lhs.c2 + rhs);
		}

		// Token: 0x06001121 RID: 4385 RVA: 0x000320E2 File Offset: 0x000302E2
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float2x3 operator +(float lhs, float2x3 rhs)
		{
			return new float2x3(lhs + rhs.c0, lhs + rhs.c1, lhs + rhs.c2);
		}

		// Token: 0x06001122 RID: 4386 RVA: 0x0003210D File Offset: 0x0003030D
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float2x3 operator -(float2x3 lhs, float2x3 rhs)
		{
			return new float2x3(lhs.c0 - rhs.c0, lhs.c1 - rhs.c1, lhs.c2 - rhs.c2);
		}

		// Token: 0x06001123 RID: 4387 RVA: 0x00032147 File Offset: 0x00030347
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float2x3 operator -(float2x3 lhs, float rhs)
		{
			return new float2x3(lhs.c0 - rhs, lhs.c1 - rhs, lhs.c2 - rhs);
		}

		// Token: 0x06001124 RID: 4388 RVA: 0x00032172 File Offset: 0x00030372
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float2x3 operator -(float lhs, float2x3 rhs)
		{
			return new float2x3(lhs - rhs.c0, lhs - rhs.c1, lhs - rhs.c2);
		}

		// Token: 0x06001125 RID: 4389 RVA: 0x0003219D File Offset: 0x0003039D
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float2x3 operator /(float2x3 lhs, float2x3 rhs)
		{
			return new float2x3(lhs.c0 / rhs.c0, lhs.c1 / rhs.c1, lhs.c2 / rhs.c2);
		}

		// Token: 0x06001126 RID: 4390 RVA: 0x000321D7 File Offset: 0x000303D7
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float2x3 operator /(float2x3 lhs, float rhs)
		{
			return new float2x3(lhs.c0 / rhs, lhs.c1 / rhs, lhs.c2 / rhs);
		}

		// Token: 0x06001127 RID: 4391 RVA: 0x00032202 File Offset: 0x00030402
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float2x3 operator /(float lhs, float2x3 rhs)
		{
			return new float2x3(lhs / rhs.c0, lhs / rhs.c1, lhs / rhs.c2);
		}

		// Token: 0x06001128 RID: 4392 RVA: 0x0003222D File Offset: 0x0003042D
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float2x3 operator %(float2x3 lhs, float2x3 rhs)
		{
			return new float2x3(lhs.c0 % rhs.c0, lhs.c1 % rhs.c1, lhs.c2 % rhs.c2);
		}

		// Token: 0x06001129 RID: 4393 RVA: 0x00032267 File Offset: 0x00030467
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float2x3 operator %(float2x3 lhs, float rhs)
		{
			return new float2x3(lhs.c0 % rhs, lhs.c1 % rhs, lhs.c2 % rhs);
		}

		// Token: 0x0600112A RID: 4394 RVA: 0x00032292 File Offset: 0x00030492
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float2x3 operator %(float lhs, float2x3 rhs)
		{
			return new float2x3(lhs % rhs.c0, lhs % rhs.c1, lhs % rhs.c2);
		}

		// Token: 0x0600112B RID: 4395 RVA: 0x000322C0 File Offset: 0x000304C0
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float2x3 operator ++(float2x3 val)
		{
			float2 @float = float2.op_Increment(val.c0);
			val.c0 = @float;
			float2 float2 = @float;
			@float = float2.op_Increment(val.c1);
			val.c1 = @float;
			float2 float3 = @float;
			@float = float2.op_Increment(val.c2);
			val.c2 = @float;
			return new float2x3(float2, float3, @float);
		}

		// Token: 0x0600112C RID: 4396 RVA: 0x00032320 File Offset: 0x00030520
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float2x3 operator --(float2x3 val)
		{
			float2 @float = float2.op_Decrement(val.c0);
			val.c0 = @float;
			float2 float2 = @float;
			@float = float2.op_Decrement(val.c1);
			val.c1 = @float;
			float2 float3 = @float;
			@float = float2.op_Decrement(val.c2);
			val.c2 = @float;
			return new float2x3(float2, float3, @float);
		}

		// Token: 0x0600112D RID: 4397 RVA: 0x00032380 File Offset: 0x00030580
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool2x3 operator <(float2x3 lhs, float2x3 rhs)
		{
			return new bool2x3(lhs.c0 < rhs.c0, lhs.c1 < rhs.c1, lhs.c2 < rhs.c2);
		}

		// Token: 0x0600112E RID: 4398 RVA: 0x000323BA File Offset: 0x000305BA
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool2x3 operator <(float2x3 lhs, float rhs)
		{
			return new bool2x3(lhs.c0 < rhs, lhs.c1 < rhs, lhs.c2 < rhs);
		}

		// Token: 0x0600112F RID: 4399 RVA: 0x000323E5 File Offset: 0x000305E5
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool2x3 operator <(float lhs, float2x3 rhs)
		{
			return new bool2x3(lhs < rhs.c0, lhs < rhs.c1, lhs < rhs.c2);
		}

		// Token: 0x06001130 RID: 4400 RVA: 0x00032410 File Offset: 0x00030610
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool2x3 operator <=(float2x3 lhs, float2x3 rhs)
		{
			return new bool2x3(lhs.c0 <= rhs.c0, lhs.c1 <= rhs.c1, lhs.c2 <= rhs.c2);
		}

		// Token: 0x06001131 RID: 4401 RVA: 0x0003244A File Offset: 0x0003064A
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool2x3 operator <=(float2x3 lhs, float rhs)
		{
			return new bool2x3(lhs.c0 <= rhs, lhs.c1 <= rhs, lhs.c2 <= rhs);
		}

		// Token: 0x06001132 RID: 4402 RVA: 0x00032475 File Offset: 0x00030675
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool2x3 operator <=(float lhs, float2x3 rhs)
		{
			return new bool2x3(lhs <= rhs.c0, lhs <= rhs.c1, lhs <= rhs.c2);
		}

		// Token: 0x06001133 RID: 4403 RVA: 0x000324A0 File Offset: 0x000306A0
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool2x3 operator >(float2x3 lhs, float2x3 rhs)
		{
			return new bool2x3(lhs.c0 > rhs.c0, lhs.c1 > rhs.c1, lhs.c2 > rhs.c2);
		}

		// Token: 0x06001134 RID: 4404 RVA: 0x000324DA File Offset: 0x000306DA
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool2x3 operator >(float2x3 lhs, float rhs)
		{
			return new bool2x3(lhs.c0 > rhs, lhs.c1 > rhs, lhs.c2 > rhs);
		}

		// Token: 0x06001135 RID: 4405 RVA: 0x00032505 File Offset: 0x00030705
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool2x3 operator >(float lhs, float2x3 rhs)
		{
			return new bool2x3(lhs > rhs.c0, lhs > rhs.c1, lhs > rhs.c2);
		}

		// Token: 0x06001136 RID: 4406 RVA: 0x00032530 File Offset: 0x00030730
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool2x3 operator >=(float2x3 lhs, float2x3 rhs)
		{
			return new bool2x3(lhs.c0 >= rhs.c0, lhs.c1 >= rhs.c1, lhs.c2 >= rhs.c2);
		}

		// Token: 0x06001137 RID: 4407 RVA: 0x0003256A File Offset: 0x0003076A
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool2x3 operator >=(float2x3 lhs, float rhs)
		{
			return new bool2x3(lhs.c0 >= rhs, lhs.c1 >= rhs, lhs.c2 >= rhs);
		}

		// Token: 0x06001138 RID: 4408 RVA: 0x00032595 File Offset: 0x00030795
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool2x3 operator >=(float lhs, float2x3 rhs)
		{
			return new bool2x3(lhs >= rhs.c0, lhs >= rhs.c1, lhs >= rhs.c2);
		}

		// Token: 0x06001139 RID: 4409 RVA: 0x000325C0 File Offset: 0x000307C0
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float2x3 operator -(float2x3 val)
		{
			return new float2x3(-val.c0, -val.c1, -val.c2);
		}

		// Token: 0x0600113A RID: 4410 RVA: 0x000325E8 File Offset: 0x000307E8
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float2x3 operator +(float2x3 val)
		{
			return new float2x3(+val.c0, +val.c1, +val.c2);
		}

		// Token: 0x0600113B RID: 4411 RVA: 0x00032610 File Offset: 0x00030810
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool2x3 operator ==(float2x3 lhs, float2x3 rhs)
		{
			return new bool2x3(lhs.c0 == rhs.c0, lhs.c1 == rhs.c1, lhs.c2 == rhs.c2);
		}

		// Token: 0x0600113C RID: 4412 RVA: 0x0003264A File Offset: 0x0003084A
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool2x3 operator ==(float2x3 lhs, float rhs)
		{
			return new bool2x3(lhs.c0 == rhs, lhs.c1 == rhs, lhs.c2 == rhs);
		}

		// Token: 0x0600113D RID: 4413 RVA: 0x00032675 File Offset: 0x00030875
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool2x3 operator ==(float lhs, float2x3 rhs)
		{
			return new bool2x3(lhs == rhs.c0, lhs == rhs.c1, lhs == rhs.c2);
		}

		// Token: 0x0600113E RID: 4414 RVA: 0x000326A0 File Offset: 0x000308A0
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool2x3 operator !=(float2x3 lhs, float2x3 rhs)
		{
			return new bool2x3(lhs.c0 != rhs.c0, lhs.c1 != rhs.c1, lhs.c2 != rhs.c2);
		}

		// Token: 0x0600113F RID: 4415 RVA: 0x000326DA File Offset: 0x000308DA
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool2x3 operator !=(float2x3 lhs, float rhs)
		{
			return new bool2x3(lhs.c0 != rhs, lhs.c1 != rhs, lhs.c2 != rhs);
		}

		// Token: 0x06001140 RID: 4416 RVA: 0x00032705 File Offset: 0x00030905
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool2x3 operator !=(float lhs, float2x3 rhs)
		{
			return new bool2x3(lhs != rhs.c0, lhs != rhs.c1, lhs != rhs.c2);
		}

		// Token: 0x170003F9 RID: 1017
		public unsafe ref float2 this[int index]
		{
			get
			{
				fixed (float2x3* ptr = &this)
				{
					return ref *(float2*)(ptr + (IntPtr)index * (IntPtr)sizeof(float2) / (IntPtr)sizeof(float2x3));
				}
			}
		}

		// Token: 0x06001142 RID: 4418 RVA: 0x0003274B File Offset: 0x0003094B
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool Equals(float2x3 rhs)
		{
			return this.c0.Equals(rhs.c0) && this.c1.Equals(rhs.c1) && this.c2.Equals(rhs.c2);
		}

		// Token: 0x06001143 RID: 4419 RVA: 0x00032788 File Offset: 0x00030988
		public override bool Equals(object o)
		{
			if (o is float2x3)
			{
				float2x3 float2x = (float2x3)o;
				return this.Equals(float2x);
			}
			return false;
		}

		// Token: 0x06001144 RID: 4420 RVA: 0x000327AD File Offset: 0x000309AD
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public override int GetHashCode()
		{
			return (int)math.hash(this);
		}

		// Token: 0x06001145 RID: 4421 RVA: 0x000327BC File Offset: 0x000309BC
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public override string ToString()
		{
			return string.Format("float2x3({0}f, {1}f, {2}f,  {3}f, {4}f, {5}f)", new object[]
			{
				this.c0.x,
				this.c1.x,
				this.c2.x,
				this.c0.y,
				this.c1.y,
				this.c2.y
			});
		}

		// Token: 0x06001146 RID: 4422 RVA: 0x0003284C File Offset: 0x00030A4C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public string ToString(string format, IFormatProvider formatProvider)
		{
			return string.Format("float2x3({0}f, {1}f, {2}f,  {3}f, {4}f, {5}f)", new object[]
			{
				this.c0.x.ToString(format, formatProvider),
				this.c1.x.ToString(format, formatProvider),
				this.c2.x.ToString(format, formatProvider),
				this.c0.y.ToString(format, formatProvider),
				this.c1.y.ToString(format, formatProvider),
				this.c2.y.ToString(format, formatProvider)
			});
		}

		// Token: 0x04000083 RID: 131
		public float2 c0;

		// Token: 0x04000084 RID: 132
		public float2 c1;

		// Token: 0x04000085 RID: 133
		public float2 c2;

		// Token: 0x04000086 RID: 134
		public static readonly float2x3 zero;
	}
}
