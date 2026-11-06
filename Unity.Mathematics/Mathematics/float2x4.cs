using System;
using System.Runtime.CompilerServices;
using Unity.IL2CPP.CompilerServices;

namespace Unity.Mathematics
{
	// Token: 0x02000021 RID: 33
	[Il2CppEagerStaticClassConstruction]
	[Serializable]
	public struct float2x4 : IEquatable<float2x4>, IFormattable
	{
		// Token: 0x06001147 RID: 4423 RVA: 0x000328E7 File Offset: 0x00030AE7
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public float2x4(float2 c0, float2 c1, float2 c2, float2 c3)
		{
			this.c0 = c0;
			this.c1 = c1;
			this.c2 = c2;
			this.c3 = c3;
		}

		// Token: 0x06001148 RID: 4424 RVA: 0x00032906 File Offset: 0x00030B06
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public float2x4(float m00, float m01, float m02, float m03, float m10, float m11, float m12, float m13)
		{
			this.c0 = new float2(m00, m10);
			this.c1 = new float2(m01, m11);
			this.c2 = new float2(m02, m12);
			this.c3 = new float2(m03, m13);
		}

		// Token: 0x06001149 RID: 4425 RVA: 0x00032941 File Offset: 0x00030B41
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public float2x4(float v)
		{
			this.c0 = v;
			this.c1 = v;
			this.c2 = v;
			this.c3 = v;
		}

		// Token: 0x0600114A RID: 4426 RVA: 0x00032974 File Offset: 0x00030B74
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public float2x4(bool v)
		{
			this.c0 = math.select(new float2(0f), new float2(1f), v);
			this.c1 = math.select(new float2(0f), new float2(1f), v);
			this.c2 = math.select(new float2(0f), new float2(1f), v);
			this.c3 = math.select(new float2(0f), new float2(1f), v);
		}

		// Token: 0x0600114B RID: 4427 RVA: 0x00032A04 File Offset: 0x00030C04
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public float2x4(bool2x4 v)
		{
			this.c0 = math.select(new float2(0f), new float2(1f), v.c0);
			this.c1 = math.select(new float2(0f), new float2(1f), v.c1);
			this.c2 = math.select(new float2(0f), new float2(1f), v.c2);
			this.c3 = math.select(new float2(0f), new float2(1f), v.c3);
		}

		// Token: 0x0600114C RID: 4428 RVA: 0x00032AA5 File Offset: 0x00030CA5
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public float2x4(int v)
		{
			this.c0 = v;
			this.c1 = v;
			this.c2 = v;
			this.c3 = v;
		}

		// Token: 0x0600114D RID: 4429 RVA: 0x00032AD8 File Offset: 0x00030CD8
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public float2x4(int2x4 v)
		{
			this.c0 = v.c0;
			this.c1 = v.c1;
			this.c2 = v.c2;
			this.c3 = v.c3;
		}

		// Token: 0x0600114E RID: 4430 RVA: 0x00032B29 File Offset: 0x00030D29
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public float2x4(uint v)
		{
			this.c0 = v;
			this.c1 = v;
			this.c2 = v;
			this.c3 = v;
		}

		// Token: 0x0600114F RID: 4431 RVA: 0x00032B5C File Offset: 0x00030D5C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public float2x4(uint2x4 v)
		{
			this.c0 = v.c0;
			this.c1 = v.c1;
			this.c2 = v.c2;
			this.c3 = v.c3;
		}

		// Token: 0x06001150 RID: 4432 RVA: 0x00032BAD File Offset: 0x00030DAD
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public float2x4(double v)
		{
			this.c0 = (float2)v;
			this.c1 = (float2)v;
			this.c2 = (float2)v;
			this.c3 = (float2)v;
		}

		// Token: 0x06001151 RID: 4433 RVA: 0x00032BE0 File Offset: 0x00030DE0
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public float2x4(double2x4 v)
		{
			this.c0 = (float2)v.c0;
			this.c1 = (float2)v.c1;
			this.c2 = (float2)v.c2;
			this.c3 = (float2)v.c3;
		}

		// Token: 0x06001152 RID: 4434 RVA: 0x00032C31 File Offset: 0x00030E31
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static implicit operator float2x4(float v)
		{
			return new float2x4(v);
		}

		// Token: 0x06001153 RID: 4435 RVA: 0x00032C39 File Offset: 0x00030E39
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static explicit operator float2x4(bool v)
		{
			return new float2x4(v);
		}

		// Token: 0x06001154 RID: 4436 RVA: 0x00032C41 File Offset: 0x00030E41
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static explicit operator float2x4(bool2x4 v)
		{
			return new float2x4(v);
		}

		// Token: 0x06001155 RID: 4437 RVA: 0x00032C49 File Offset: 0x00030E49
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static implicit operator float2x4(int v)
		{
			return new float2x4(v);
		}

		// Token: 0x06001156 RID: 4438 RVA: 0x00032C51 File Offset: 0x00030E51
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static implicit operator float2x4(int2x4 v)
		{
			return new float2x4(v);
		}

		// Token: 0x06001157 RID: 4439 RVA: 0x00032C59 File Offset: 0x00030E59
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static implicit operator float2x4(uint v)
		{
			return new float2x4(v);
		}

		// Token: 0x06001158 RID: 4440 RVA: 0x00032C61 File Offset: 0x00030E61
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static implicit operator float2x4(uint2x4 v)
		{
			return new float2x4(v);
		}

		// Token: 0x06001159 RID: 4441 RVA: 0x00032C69 File Offset: 0x00030E69
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static explicit operator float2x4(double v)
		{
			return new float2x4(v);
		}

		// Token: 0x0600115A RID: 4442 RVA: 0x00032C71 File Offset: 0x00030E71
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static explicit operator float2x4(double2x4 v)
		{
			return new float2x4(v);
		}

		// Token: 0x0600115B RID: 4443 RVA: 0x00032C7C File Offset: 0x00030E7C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float2x4 operator *(float2x4 lhs, float2x4 rhs)
		{
			return new float2x4(lhs.c0 * rhs.c0, lhs.c1 * rhs.c1, lhs.c2 * rhs.c2, lhs.c3 * rhs.c3);
		}

		// Token: 0x0600115C RID: 4444 RVA: 0x00032CD2 File Offset: 0x00030ED2
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float2x4 operator *(float2x4 lhs, float rhs)
		{
			return new float2x4(lhs.c0 * rhs, lhs.c1 * rhs, lhs.c2 * rhs, lhs.c3 * rhs);
		}

		// Token: 0x0600115D RID: 4445 RVA: 0x00032D09 File Offset: 0x00030F09
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float2x4 operator *(float lhs, float2x4 rhs)
		{
			return new float2x4(lhs * rhs.c0, lhs * rhs.c1, lhs * rhs.c2, lhs * rhs.c3);
		}

		// Token: 0x0600115E RID: 4446 RVA: 0x00032D40 File Offset: 0x00030F40
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float2x4 operator +(float2x4 lhs, float2x4 rhs)
		{
			return new float2x4(lhs.c0 + rhs.c0, lhs.c1 + rhs.c1, lhs.c2 + rhs.c2, lhs.c3 + rhs.c3);
		}

		// Token: 0x0600115F RID: 4447 RVA: 0x00032D96 File Offset: 0x00030F96
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float2x4 operator +(float2x4 lhs, float rhs)
		{
			return new float2x4(lhs.c0 + rhs, lhs.c1 + rhs, lhs.c2 + rhs, lhs.c3 + rhs);
		}

		// Token: 0x06001160 RID: 4448 RVA: 0x00032DCD File Offset: 0x00030FCD
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float2x4 operator +(float lhs, float2x4 rhs)
		{
			return new float2x4(lhs + rhs.c0, lhs + rhs.c1, lhs + rhs.c2, lhs + rhs.c3);
		}

		// Token: 0x06001161 RID: 4449 RVA: 0x00032E04 File Offset: 0x00031004
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float2x4 operator -(float2x4 lhs, float2x4 rhs)
		{
			return new float2x4(lhs.c0 - rhs.c0, lhs.c1 - rhs.c1, lhs.c2 - rhs.c2, lhs.c3 - rhs.c3);
		}

		// Token: 0x06001162 RID: 4450 RVA: 0x00032E5A File Offset: 0x0003105A
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float2x4 operator -(float2x4 lhs, float rhs)
		{
			return new float2x4(lhs.c0 - rhs, lhs.c1 - rhs, lhs.c2 - rhs, lhs.c3 - rhs);
		}

		// Token: 0x06001163 RID: 4451 RVA: 0x00032E91 File Offset: 0x00031091
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float2x4 operator -(float lhs, float2x4 rhs)
		{
			return new float2x4(lhs - rhs.c0, lhs - rhs.c1, lhs - rhs.c2, lhs - rhs.c3);
		}

		// Token: 0x06001164 RID: 4452 RVA: 0x00032EC8 File Offset: 0x000310C8
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float2x4 operator /(float2x4 lhs, float2x4 rhs)
		{
			return new float2x4(lhs.c0 / rhs.c0, lhs.c1 / rhs.c1, lhs.c2 / rhs.c2, lhs.c3 / rhs.c3);
		}

		// Token: 0x06001165 RID: 4453 RVA: 0x00032F1E File Offset: 0x0003111E
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float2x4 operator /(float2x4 lhs, float rhs)
		{
			return new float2x4(lhs.c0 / rhs, lhs.c1 / rhs, lhs.c2 / rhs, lhs.c3 / rhs);
		}

		// Token: 0x06001166 RID: 4454 RVA: 0x00032F55 File Offset: 0x00031155
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float2x4 operator /(float lhs, float2x4 rhs)
		{
			return new float2x4(lhs / rhs.c0, lhs / rhs.c1, lhs / rhs.c2, lhs / rhs.c3);
		}

		// Token: 0x06001167 RID: 4455 RVA: 0x00032F8C File Offset: 0x0003118C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float2x4 operator %(float2x4 lhs, float2x4 rhs)
		{
			return new float2x4(lhs.c0 % rhs.c0, lhs.c1 % rhs.c1, lhs.c2 % rhs.c2, lhs.c3 % rhs.c3);
		}

		// Token: 0x06001168 RID: 4456 RVA: 0x00032FE2 File Offset: 0x000311E2
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float2x4 operator %(float2x4 lhs, float rhs)
		{
			return new float2x4(lhs.c0 % rhs, lhs.c1 % rhs, lhs.c2 % rhs, lhs.c3 % rhs);
		}

		// Token: 0x06001169 RID: 4457 RVA: 0x00033019 File Offset: 0x00031219
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float2x4 operator %(float lhs, float2x4 rhs)
		{
			return new float2x4(lhs % rhs.c0, lhs % rhs.c1, lhs % rhs.c2, lhs % rhs.c3);
		}

		// Token: 0x0600116A RID: 4458 RVA: 0x00033050 File Offset: 0x00031250
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float2x4 operator ++(float2x4 val)
		{
			float2 @float = float2.op_Increment(val.c0);
			val.c0 = @float;
			float2 float2 = @float;
			@float = float2.op_Increment(val.c1);
			val.c1 = @float;
			float2 float3 = @float;
			@float = float2.op_Increment(val.c2);
			val.c2 = @float;
			float2 float4 = @float;
			@float = float2.op_Increment(val.c3);
			val.c3 = @float;
			return new float2x4(float2, float3, float4, @float);
		}

		// Token: 0x0600116B RID: 4459 RVA: 0x000330CC File Offset: 0x000312CC
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float2x4 operator --(float2x4 val)
		{
			float2 @float = float2.op_Decrement(val.c0);
			val.c0 = @float;
			float2 float2 = @float;
			@float = float2.op_Decrement(val.c1);
			val.c1 = @float;
			float2 float3 = @float;
			@float = float2.op_Decrement(val.c2);
			val.c2 = @float;
			float2 float4 = @float;
			@float = float2.op_Decrement(val.c3);
			val.c3 = @float;
			return new float2x4(float2, float3, float4, @float);
		}

		// Token: 0x0600116C RID: 4460 RVA: 0x00033148 File Offset: 0x00031348
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool2x4 operator <(float2x4 lhs, float2x4 rhs)
		{
			return new bool2x4(lhs.c0 < rhs.c0, lhs.c1 < rhs.c1, lhs.c2 < rhs.c2, lhs.c3 < rhs.c3);
		}

		// Token: 0x0600116D RID: 4461 RVA: 0x0003319E File Offset: 0x0003139E
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool2x4 operator <(float2x4 lhs, float rhs)
		{
			return new bool2x4(lhs.c0 < rhs, lhs.c1 < rhs, lhs.c2 < rhs, lhs.c3 < rhs);
		}

		// Token: 0x0600116E RID: 4462 RVA: 0x000331D5 File Offset: 0x000313D5
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool2x4 operator <(float lhs, float2x4 rhs)
		{
			return new bool2x4(lhs < rhs.c0, lhs < rhs.c1, lhs < rhs.c2, lhs < rhs.c3);
		}

		// Token: 0x0600116F RID: 4463 RVA: 0x0003320C File Offset: 0x0003140C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool2x4 operator <=(float2x4 lhs, float2x4 rhs)
		{
			return new bool2x4(lhs.c0 <= rhs.c0, lhs.c1 <= rhs.c1, lhs.c2 <= rhs.c2, lhs.c3 <= rhs.c3);
		}

		// Token: 0x06001170 RID: 4464 RVA: 0x00033262 File Offset: 0x00031462
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool2x4 operator <=(float2x4 lhs, float rhs)
		{
			return new bool2x4(lhs.c0 <= rhs, lhs.c1 <= rhs, lhs.c2 <= rhs, lhs.c3 <= rhs);
		}

		// Token: 0x06001171 RID: 4465 RVA: 0x00033299 File Offset: 0x00031499
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool2x4 operator <=(float lhs, float2x4 rhs)
		{
			return new bool2x4(lhs <= rhs.c0, lhs <= rhs.c1, lhs <= rhs.c2, lhs <= rhs.c3);
		}

		// Token: 0x06001172 RID: 4466 RVA: 0x000332D0 File Offset: 0x000314D0
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool2x4 operator >(float2x4 lhs, float2x4 rhs)
		{
			return new bool2x4(lhs.c0 > rhs.c0, lhs.c1 > rhs.c1, lhs.c2 > rhs.c2, lhs.c3 > rhs.c3);
		}

		// Token: 0x06001173 RID: 4467 RVA: 0x00033326 File Offset: 0x00031526
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool2x4 operator >(float2x4 lhs, float rhs)
		{
			return new bool2x4(lhs.c0 > rhs, lhs.c1 > rhs, lhs.c2 > rhs, lhs.c3 > rhs);
		}

		// Token: 0x06001174 RID: 4468 RVA: 0x0003335D File Offset: 0x0003155D
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool2x4 operator >(float lhs, float2x4 rhs)
		{
			return new bool2x4(lhs > rhs.c0, lhs > rhs.c1, lhs > rhs.c2, lhs > rhs.c3);
		}

		// Token: 0x06001175 RID: 4469 RVA: 0x00033394 File Offset: 0x00031594
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool2x4 operator >=(float2x4 lhs, float2x4 rhs)
		{
			return new bool2x4(lhs.c0 >= rhs.c0, lhs.c1 >= rhs.c1, lhs.c2 >= rhs.c2, lhs.c3 >= rhs.c3);
		}

		// Token: 0x06001176 RID: 4470 RVA: 0x000333EA File Offset: 0x000315EA
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool2x4 operator >=(float2x4 lhs, float rhs)
		{
			return new bool2x4(lhs.c0 >= rhs, lhs.c1 >= rhs, lhs.c2 >= rhs, lhs.c3 >= rhs);
		}

		// Token: 0x06001177 RID: 4471 RVA: 0x00033421 File Offset: 0x00031621
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool2x4 operator >=(float lhs, float2x4 rhs)
		{
			return new bool2x4(lhs >= rhs.c0, lhs >= rhs.c1, lhs >= rhs.c2, lhs >= rhs.c3);
		}

		// Token: 0x06001178 RID: 4472 RVA: 0x00033458 File Offset: 0x00031658
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float2x4 operator -(float2x4 val)
		{
			return new float2x4(-val.c0, -val.c1, -val.c2, -val.c3);
		}

		// Token: 0x06001179 RID: 4473 RVA: 0x0003348B File Offset: 0x0003168B
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float2x4 operator +(float2x4 val)
		{
			return new float2x4(+val.c0, +val.c1, +val.c2, +val.c3);
		}

		// Token: 0x0600117A RID: 4474 RVA: 0x000334C0 File Offset: 0x000316C0
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool2x4 operator ==(float2x4 lhs, float2x4 rhs)
		{
			return new bool2x4(lhs.c0 == rhs.c0, lhs.c1 == rhs.c1, lhs.c2 == rhs.c2, lhs.c3 == rhs.c3);
		}

		// Token: 0x0600117B RID: 4475 RVA: 0x00033516 File Offset: 0x00031716
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool2x4 operator ==(float2x4 lhs, float rhs)
		{
			return new bool2x4(lhs.c0 == rhs, lhs.c1 == rhs, lhs.c2 == rhs, lhs.c3 == rhs);
		}

		// Token: 0x0600117C RID: 4476 RVA: 0x0003354D File Offset: 0x0003174D
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool2x4 operator ==(float lhs, float2x4 rhs)
		{
			return new bool2x4(lhs == rhs.c0, lhs == rhs.c1, lhs == rhs.c2, lhs == rhs.c3);
		}

		// Token: 0x0600117D RID: 4477 RVA: 0x00033584 File Offset: 0x00031784
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool2x4 operator !=(float2x4 lhs, float2x4 rhs)
		{
			return new bool2x4(lhs.c0 != rhs.c0, lhs.c1 != rhs.c1, lhs.c2 != rhs.c2, lhs.c3 != rhs.c3);
		}

		// Token: 0x0600117E RID: 4478 RVA: 0x000335DA File Offset: 0x000317DA
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool2x4 operator !=(float2x4 lhs, float rhs)
		{
			return new bool2x4(lhs.c0 != rhs, lhs.c1 != rhs, lhs.c2 != rhs, lhs.c3 != rhs);
		}

		// Token: 0x0600117F RID: 4479 RVA: 0x00033611 File Offset: 0x00031811
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool2x4 operator !=(float lhs, float2x4 rhs)
		{
			return new bool2x4(lhs != rhs.c0, lhs != rhs.c1, lhs != rhs.c2, lhs != rhs.c3);
		}

		// Token: 0x170003FA RID: 1018
		public unsafe ref float2 this[int index]
		{
			get
			{
				fixed (float2x4* ptr = &this)
				{
					return ref *(float2*)(ptr + (IntPtr)index * (IntPtr)sizeof(float2) / (IntPtr)sizeof(float2x4));
				}
			}
		}

		// Token: 0x06001181 RID: 4481 RVA: 0x00033664 File Offset: 0x00031864
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool Equals(float2x4 rhs)
		{
			return this.c0.Equals(rhs.c0) && this.c1.Equals(rhs.c1) && this.c2.Equals(rhs.c2) && this.c3.Equals(rhs.c3);
		}

		// Token: 0x06001182 RID: 4482 RVA: 0x000336C0 File Offset: 0x000318C0
		public override bool Equals(object o)
		{
			if (o is float2x4)
			{
				float2x4 float2x = (float2x4)o;
				return this.Equals(float2x);
			}
			return false;
		}

		// Token: 0x06001183 RID: 4483 RVA: 0x000336E5 File Offset: 0x000318E5
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public override int GetHashCode()
		{
			return (int)math.hash(this);
		}

		// Token: 0x06001184 RID: 4484 RVA: 0x000336F4 File Offset: 0x000318F4
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public override string ToString()
		{
			return string.Format("float2x4({0}f, {1}f, {2}f, {3}f,  {4}f, {5}f, {6}f, {7}f)", new object[]
			{
				this.c0.x,
				this.c1.x,
				this.c2.x,
				this.c3.x,
				this.c0.y,
				this.c1.y,
				this.c2.y,
				this.c3.y
			});
		}

		// Token: 0x06001185 RID: 4485 RVA: 0x000337AC File Offset: 0x000319AC
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public string ToString(string format, IFormatProvider formatProvider)
		{
			return string.Format("float2x4({0}f, {1}f, {2}f, {3}f,  {4}f, {5}f, {6}f, {7}f)", new object[]
			{
				this.c0.x.ToString(format, formatProvider),
				this.c1.x.ToString(format, formatProvider),
				this.c2.x.ToString(format, formatProvider),
				this.c3.x.ToString(format, formatProvider),
				this.c0.y.ToString(format, formatProvider),
				this.c1.y.ToString(format, formatProvider),
				this.c2.y.ToString(format, formatProvider),
				this.c3.y.ToString(format, formatProvider)
			});
		}

		// Token: 0x04000087 RID: 135
		public float2 c0;

		// Token: 0x04000088 RID: 136
		public float2 c1;

		// Token: 0x04000089 RID: 137
		public float2 c2;

		// Token: 0x0400008A RID: 138
		public float2 c3;

		// Token: 0x0400008B RID: 139
		public static readonly float2x4 zero;
	}
}
