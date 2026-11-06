using System;
using System.Runtime.CompilerServices;
using Unity.IL2CPP.CompilerServices;

namespace Unity.Mathematics
{
	// Token: 0x02000027 RID: 39
	[Il2CppEagerStaticClassConstruction]
	[Serializable]
	public struct float4x2 : IEquatable<float4x2>, IFormattable
	{
		// Token: 0x060014FF RID: 5375 RVA: 0x0003BF0F File Offset: 0x0003A10F
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public float4x2(float4 c0, float4 c1)
		{
			this.c0 = c0;
			this.c1 = c1;
		}

		// Token: 0x06001500 RID: 5376 RVA: 0x0003BF1F File Offset: 0x0003A11F
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public float4x2(float m00, float m01, float m10, float m11, float m20, float m21, float m30, float m31)
		{
			this.c0 = new float4(m00, m10, m20, m30);
			this.c1 = new float4(m01, m11, m21, m31);
		}

		// Token: 0x06001501 RID: 5377 RVA: 0x0003BF44 File Offset: 0x0003A144
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public float4x2(float v)
		{
			this.c0 = v;
			this.c1 = v;
		}

		// Token: 0x06001502 RID: 5378 RVA: 0x0003BF60 File Offset: 0x0003A160
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public float4x2(bool v)
		{
			this.c0 = math.select(new float4(0f), new float4(1f), v);
			this.c1 = math.select(new float4(0f), new float4(1f), v);
		}

		// Token: 0x06001503 RID: 5379 RVA: 0x0003BFB0 File Offset: 0x0003A1B0
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public float4x2(bool4x2 v)
		{
			this.c0 = math.select(new float4(0f), new float4(1f), v.c0);
			this.c1 = math.select(new float4(0f), new float4(1f), v.c1);
		}

		// Token: 0x06001504 RID: 5380 RVA: 0x0003C007 File Offset: 0x0003A207
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public float4x2(int v)
		{
			this.c0 = v;
			this.c1 = v;
		}

		// Token: 0x06001505 RID: 5381 RVA: 0x0003C021 File Offset: 0x0003A221
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public float4x2(int4x2 v)
		{
			this.c0 = v.c0;
			this.c1 = v.c1;
		}

		// Token: 0x06001506 RID: 5382 RVA: 0x0003C045 File Offset: 0x0003A245
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public float4x2(uint v)
		{
			this.c0 = v;
			this.c1 = v;
		}

		// Token: 0x06001507 RID: 5383 RVA: 0x0003C05F File Offset: 0x0003A25F
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public float4x2(uint4x2 v)
		{
			this.c0 = v.c0;
			this.c1 = v.c1;
		}

		// Token: 0x06001508 RID: 5384 RVA: 0x0003C083 File Offset: 0x0003A283
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public float4x2(double v)
		{
			this.c0 = (float4)v;
			this.c1 = (float4)v;
		}

		// Token: 0x06001509 RID: 5385 RVA: 0x0003C09D File Offset: 0x0003A29D
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public float4x2(double4x2 v)
		{
			this.c0 = (float4)v.c0;
			this.c1 = (float4)v.c1;
		}

		// Token: 0x0600150A RID: 5386 RVA: 0x0003C0C1 File Offset: 0x0003A2C1
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static implicit operator float4x2(float v)
		{
			return new float4x2(v);
		}

		// Token: 0x0600150B RID: 5387 RVA: 0x0003C0C9 File Offset: 0x0003A2C9
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static explicit operator float4x2(bool v)
		{
			return new float4x2(v);
		}

		// Token: 0x0600150C RID: 5388 RVA: 0x0003C0D1 File Offset: 0x0003A2D1
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static explicit operator float4x2(bool4x2 v)
		{
			return new float4x2(v);
		}

		// Token: 0x0600150D RID: 5389 RVA: 0x0003C0D9 File Offset: 0x0003A2D9
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static implicit operator float4x2(int v)
		{
			return new float4x2(v);
		}

		// Token: 0x0600150E RID: 5390 RVA: 0x0003C0E1 File Offset: 0x0003A2E1
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static implicit operator float4x2(int4x2 v)
		{
			return new float4x2(v);
		}

		// Token: 0x0600150F RID: 5391 RVA: 0x0003C0E9 File Offset: 0x0003A2E9
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static implicit operator float4x2(uint v)
		{
			return new float4x2(v);
		}

		// Token: 0x06001510 RID: 5392 RVA: 0x0003C0F1 File Offset: 0x0003A2F1
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static implicit operator float4x2(uint4x2 v)
		{
			return new float4x2(v);
		}

		// Token: 0x06001511 RID: 5393 RVA: 0x0003C0F9 File Offset: 0x0003A2F9
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static explicit operator float4x2(double v)
		{
			return new float4x2(v);
		}

		// Token: 0x06001512 RID: 5394 RVA: 0x0003C101 File Offset: 0x0003A301
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static explicit operator float4x2(double4x2 v)
		{
			return new float4x2(v);
		}

		// Token: 0x06001513 RID: 5395 RVA: 0x0003C109 File Offset: 0x0003A309
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float4x2 operator *(float4x2 lhs, float4x2 rhs)
		{
			return new float4x2(lhs.c0 * rhs.c0, lhs.c1 * rhs.c1);
		}

		// Token: 0x06001514 RID: 5396 RVA: 0x0003C132 File Offset: 0x0003A332
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float4x2 operator *(float4x2 lhs, float rhs)
		{
			return new float4x2(lhs.c0 * rhs, lhs.c1 * rhs);
		}

		// Token: 0x06001515 RID: 5397 RVA: 0x0003C151 File Offset: 0x0003A351
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float4x2 operator *(float lhs, float4x2 rhs)
		{
			return new float4x2(lhs * rhs.c0, lhs * rhs.c1);
		}

		// Token: 0x06001516 RID: 5398 RVA: 0x0003C170 File Offset: 0x0003A370
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float4x2 operator +(float4x2 lhs, float4x2 rhs)
		{
			return new float4x2(lhs.c0 + rhs.c0, lhs.c1 + rhs.c1);
		}

		// Token: 0x06001517 RID: 5399 RVA: 0x0003C199 File Offset: 0x0003A399
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float4x2 operator +(float4x2 lhs, float rhs)
		{
			return new float4x2(lhs.c0 + rhs, lhs.c1 + rhs);
		}

		// Token: 0x06001518 RID: 5400 RVA: 0x0003C1B8 File Offset: 0x0003A3B8
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float4x2 operator +(float lhs, float4x2 rhs)
		{
			return new float4x2(lhs + rhs.c0, lhs + rhs.c1);
		}

		// Token: 0x06001519 RID: 5401 RVA: 0x0003C1D7 File Offset: 0x0003A3D7
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float4x2 operator -(float4x2 lhs, float4x2 rhs)
		{
			return new float4x2(lhs.c0 - rhs.c0, lhs.c1 - rhs.c1);
		}

		// Token: 0x0600151A RID: 5402 RVA: 0x0003C200 File Offset: 0x0003A400
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float4x2 operator -(float4x2 lhs, float rhs)
		{
			return new float4x2(lhs.c0 - rhs, lhs.c1 - rhs);
		}

		// Token: 0x0600151B RID: 5403 RVA: 0x0003C21F File Offset: 0x0003A41F
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float4x2 operator -(float lhs, float4x2 rhs)
		{
			return new float4x2(lhs - rhs.c0, lhs - rhs.c1);
		}

		// Token: 0x0600151C RID: 5404 RVA: 0x0003C23E File Offset: 0x0003A43E
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float4x2 operator /(float4x2 lhs, float4x2 rhs)
		{
			return new float4x2(lhs.c0 / rhs.c0, lhs.c1 / rhs.c1);
		}

		// Token: 0x0600151D RID: 5405 RVA: 0x0003C267 File Offset: 0x0003A467
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float4x2 operator /(float4x2 lhs, float rhs)
		{
			return new float4x2(lhs.c0 / rhs, lhs.c1 / rhs);
		}

		// Token: 0x0600151E RID: 5406 RVA: 0x0003C286 File Offset: 0x0003A486
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float4x2 operator /(float lhs, float4x2 rhs)
		{
			return new float4x2(lhs / rhs.c0, lhs / rhs.c1);
		}

		// Token: 0x0600151F RID: 5407 RVA: 0x0003C2A5 File Offset: 0x0003A4A5
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float4x2 operator %(float4x2 lhs, float4x2 rhs)
		{
			return new float4x2(lhs.c0 % rhs.c0, lhs.c1 % rhs.c1);
		}

		// Token: 0x06001520 RID: 5408 RVA: 0x0003C2CE File Offset: 0x0003A4CE
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float4x2 operator %(float4x2 lhs, float rhs)
		{
			return new float4x2(lhs.c0 % rhs, lhs.c1 % rhs);
		}

		// Token: 0x06001521 RID: 5409 RVA: 0x0003C2ED File Offset: 0x0003A4ED
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float4x2 operator %(float lhs, float4x2 rhs)
		{
			return new float4x2(lhs % rhs.c0, lhs % rhs.c1);
		}

		// Token: 0x06001522 RID: 5410 RVA: 0x0003C30C File Offset: 0x0003A50C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float4x2 operator ++(float4x2 val)
		{
			float4 @float = float4.op_Increment(val.c0);
			val.c0 = @float;
			float4 float2 = @float;
			@float = float4.op_Increment(val.c1);
			val.c1 = @float;
			return new float4x2(float2, @float);
		}

		// Token: 0x06001523 RID: 5411 RVA: 0x0003C354 File Offset: 0x0003A554
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float4x2 operator --(float4x2 val)
		{
			float4 @float = float4.op_Decrement(val.c0);
			val.c0 = @float;
			float4 float2 = @float;
			@float = float4.op_Decrement(val.c1);
			val.c1 = @float;
			return new float4x2(float2, @float);
		}

		// Token: 0x06001524 RID: 5412 RVA: 0x0003C39A File Offset: 0x0003A59A
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool4x2 operator <(float4x2 lhs, float4x2 rhs)
		{
			return new bool4x2(lhs.c0 < rhs.c0, lhs.c1 < rhs.c1);
		}

		// Token: 0x06001525 RID: 5413 RVA: 0x0003C3C3 File Offset: 0x0003A5C3
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool4x2 operator <(float4x2 lhs, float rhs)
		{
			return new bool4x2(lhs.c0 < rhs, lhs.c1 < rhs);
		}

		// Token: 0x06001526 RID: 5414 RVA: 0x0003C3E2 File Offset: 0x0003A5E2
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool4x2 operator <(float lhs, float4x2 rhs)
		{
			return new bool4x2(lhs < rhs.c0, lhs < rhs.c1);
		}

		// Token: 0x06001527 RID: 5415 RVA: 0x0003C401 File Offset: 0x0003A601
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool4x2 operator <=(float4x2 lhs, float4x2 rhs)
		{
			return new bool4x2(lhs.c0 <= rhs.c0, lhs.c1 <= rhs.c1);
		}

		// Token: 0x06001528 RID: 5416 RVA: 0x0003C42A File Offset: 0x0003A62A
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool4x2 operator <=(float4x2 lhs, float rhs)
		{
			return new bool4x2(lhs.c0 <= rhs, lhs.c1 <= rhs);
		}

		// Token: 0x06001529 RID: 5417 RVA: 0x0003C449 File Offset: 0x0003A649
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool4x2 operator <=(float lhs, float4x2 rhs)
		{
			return new bool4x2(lhs <= rhs.c0, lhs <= rhs.c1);
		}

		// Token: 0x0600152A RID: 5418 RVA: 0x0003C468 File Offset: 0x0003A668
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool4x2 operator >(float4x2 lhs, float4x2 rhs)
		{
			return new bool4x2(lhs.c0 > rhs.c0, lhs.c1 > rhs.c1);
		}

		// Token: 0x0600152B RID: 5419 RVA: 0x0003C491 File Offset: 0x0003A691
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool4x2 operator >(float4x2 lhs, float rhs)
		{
			return new bool4x2(lhs.c0 > rhs, lhs.c1 > rhs);
		}

		// Token: 0x0600152C RID: 5420 RVA: 0x0003C4B0 File Offset: 0x0003A6B0
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool4x2 operator >(float lhs, float4x2 rhs)
		{
			return new bool4x2(lhs > rhs.c0, lhs > rhs.c1);
		}

		// Token: 0x0600152D RID: 5421 RVA: 0x0003C4CF File Offset: 0x0003A6CF
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool4x2 operator >=(float4x2 lhs, float4x2 rhs)
		{
			return new bool4x2(lhs.c0 >= rhs.c0, lhs.c1 >= rhs.c1);
		}

		// Token: 0x0600152E RID: 5422 RVA: 0x0003C4F8 File Offset: 0x0003A6F8
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool4x2 operator >=(float4x2 lhs, float rhs)
		{
			return new bool4x2(lhs.c0 >= rhs, lhs.c1 >= rhs);
		}

		// Token: 0x0600152F RID: 5423 RVA: 0x0003C517 File Offset: 0x0003A717
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool4x2 operator >=(float lhs, float4x2 rhs)
		{
			return new bool4x2(lhs >= rhs.c0, lhs >= rhs.c1);
		}

		// Token: 0x06001530 RID: 5424 RVA: 0x0003C536 File Offset: 0x0003A736
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float4x2 operator -(float4x2 val)
		{
			return new float4x2(-val.c0, -val.c1);
		}

		// Token: 0x06001531 RID: 5425 RVA: 0x0003C553 File Offset: 0x0003A753
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float4x2 operator +(float4x2 val)
		{
			return new float4x2(+val.c0, +val.c1);
		}

		// Token: 0x06001532 RID: 5426 RVA: 0x0003C570 File Offset: 0x0003A770
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool4x2 operator ==(float4x2 lhs, float4x2 rhs)
		{
			return new bool4x2(lhs.c0 == rhs.c0, lhs.c1 == rhs.c1);
		}

		// Token: 0x06001533 RID: 5427 RVA: 0x0003C599 File Offset: 0x0003A799
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool4x2 operator ==(float4x2 lhs, float rhs)
		{
			return new bool4x2(lhs.c0 == rhs, lhs.c1 == rhs);
		}

		// Token: 0x06001534 RID: 5428 RVA: 0x0003C5B8 File Offset: 0x0003A7B8
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool4x2 operator ==(float lhs, float4x2 rhs)
		{
			return new bool4x2(lhs == rhs.c0, lhs == rhs.c1);
		}

		// Token: 0x06001535 RID: 5429 RVA: 0x0003C5D7 File Offset: 0x0003A7D7
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool4x2 operator !=(float4x2 lhs, float4x2 rhs)
		{
			return new bool4x2(lhs.c0 != rhs.c0, lhs.c1 != rhs.c1);
		}

		// Token: 0x06001536 RID: 5430 RVA: 0x0003C600 File Offset: 0x0003A800
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool4x2 operator !=(float4x2 lhs, float rhs)
		{
			return new bool4x2(lhs.c0 != rhs, lhs.c1 != rhs);
		}

		// Token: 0x06001537 RID: 5431 RVA: 0x0003C61F File Offset: 0x0003A81F
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool4x2 operator !=(float lhs, float4x2 rhs)
		{
			return new bool4x2(lhs != rhs.c0, lhs != rhs.c1);
		}

		// Token: 0x170005C5 RID: 1477
		public unsafe ref float4 this[int index]
		{
			get
			{
				fixed (float4x2* ptr = &this)
				{
					return ref *(float4*)(ptr + (IntPtr)index * (IntPtr)sizeof(float4) / (IntPtr)sizeof(float4x2));
				}
			}
		}

		// Token: 0x06001539 RID: 5433 RVA: 0x0003C65B File Offset: 0x0003A85B
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool Equals(float4x2 rhs)
		{
			return this.c0.Equals(rhs.c0) && this.c1.Equals(rhs.c1);
		}

		// Token: 0x0600153A RID: 5434 RVA: 0x0003C684 File Offset: 0x0003A884
		public override bool Equals(object o)
		{
			if (o is float4x2)
			{
				float4x2 float4x = (float4x2)o;
				return this.Equals(float4x);
			}
			return false;
		}

		// Token: 0x0600153B RID: 5435 RVA: 0x0003C6A9 File Offset: 0x0003A8A9
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public override int GetHashCode()
		{
			return (int)math.hash(this);
		}

		// Token: 0x0600153C RID: 5436 RVA: 0x0003C6B8 File Offset: 0x0003A8B8
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public override string ToString()
		{
			return string.Format("float4x2({0}f, {1}f,  {2}f, {3}f,  {4}f, {5}f,  {6}f, {7}f)", new object[]
			{
				this.c0.x,
				this.c1.x,
				this.c0.y,
				this.c1.y,
				this.c0.z,
				this.c1.z,
				this.c0.w,
				this.c1.w
			});
		}

		// Token: 0x0600153D RID: 5437 RVA: 0x0003C770 File Offset: 0x0003A970
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public string ToString(string format, IFormatProvider formatProvider)
		{
			return string.Format("float4x2({0}f, {1}f,  {2}f, {3}f,  {4}f, {5}f,  {6}f, {7}f)", new object[]
			{
				this.c0.x.ToString(format, formatProvider),
				this.c1.x.ToString(format, formatProvider),
				this.c0.y.ToString(format, formatProvider),
				this.c1.y.ToString(format, formatProvider),
				this.c0.z.ToString(format, formatProvider),
				this.c1.z.ToString(format, formatProvider),
				this.c0.w.ToString(format, formatProvider),
				this.c1.w.ToString(format, formatProvider)
			});
		}

		// Token: 0x040000A2 RID: 162
		public float4 c0;

		// Token: 0x040000A3 RID: 163
		public float4 c1;

		// Token: 0x040000A4 RID: 164
		public static readonly float4x2 zero;
	}
}
