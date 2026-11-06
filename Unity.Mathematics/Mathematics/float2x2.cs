using System;
using System.Runtime.CompilerServices;
using Unity.IL2CPP.CompilerServices;

namespace Unity.Mathematics
{
	// Token: 0x0200001F RID: 31
	[Il2CppEagerStaticClassConstruction]
	[Serializable]
	public struct float2x2 : IEquatable<float2x2>, IFormattable
	{
		// Token: 0x060010C4 RID: 4292 RVA: 0x00031444 File Offset: 0x0002F644
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public float2x2(float2 c0, float2 c1)
		{
			this.c0 = c0;
			this.c1 = c1;
		}

		// Token: 0x060010C5 RID: 4293 RVA: 0x00031454 File Offset: 0x0002F654
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public float2x2(float m00, float m01, float m10, float m11)
		{
			this.c0 = new float2(m00, m10);
			this.c1 = new float2(m01, m11);
		}

		// Token: 0x060010C6 RID: 4294 RVA: 0x00031471 File Offset: 0x0002F671
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public float2x2(float v)
		{
			this.c0 = v;
			this.c1 = v;
		}

		// Token: 0x060010C7 RID: 4295 RVA: 0x0003148C File Offset: 0x0002F68C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public float2x2(bool v)
		{
			this.c0 = math.select(new float2(0f), new float2(1f), v);
			this.c1 = math.select(new float2(0f), new float2(1f), v);
		}

		// Token: 0x060010C8 RID: 4296 RVA: 0x000314DC File Offset: 0x0002F6DC
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public float2x2(bool2x2 v)
		{
			this.c0 = math.select(new float2(0f), new float2(1f), v.c0);
			this.c1 = math.select(new float2(0f), new float2(1f), v.c1);
		}

		// Token: 0x060010C9 RID: 4297 RVA: 0x00031533 File Offset: 0x0002F733
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public float2x2(int v)
		{
			this.c0 = v;
			this.c1 = v;
		}

		// Token: 0x060010CA RID: 4298 RVA: 0x0003154D File Offset: 0x0002F74D
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public float2x2(int2x2 v)
		{
			this.c0 = v.c0;
			this.c1 = v.c1;
		}

		// Token: 0x060010CB RID: 4299 RVA: 0x00031571 File Offset: 0x0002F771
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public float2x2(uint v)
		{
			this.c0 = v;
			this.c1 = v;
		}

		// Token: 0x060010CC RID: 4300 RVA: 0x0003158B File Offset: 0x0002F78B
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public float2x2(uint2x2 v)
		{
			this.c0 = v.c0;
			this.c1 = v.c1;
		}

		// Token: 0x060010CD RID: 4301 RVA: 0x000315AF File Offset: 0x0002F7AF
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public float2x2(double v)
		{
			this.c0 = (float2)v;
			this.c1 = (float2)v;
		}

		// Token: 0x060010CE RID: 4302 RVA: 0x000315C9 File Offset: 0x0002F7C9
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public float2x2(double2x2 v)
		{
			this.c0 = (float2)v.c0;
			this.c1 = (float2)v.c1;
		}

		// Token: 0x060010CF RID: 4303 RVA: 0x000315ED File Offset: 0x0002F7ED
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static implicit operator float2x2(float v)
		{
			return new float2x2(v);
		}

		// Token: 0x060010D0 RID: 4304 RVA: 0x000315F5 File Offset: 0x0002F7F5
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static explicit operator float2x2(bool v)
		{
			return new float2x2(v);
		}

		// Token: 0x060010D1 RID: 4305 RVA: 0x000315FD File Offset: 0x0002F7FD
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static explicit operator float2x2(bool2x2 v)
		{
			return new float2x2(v);
		}

		// Token: 0x060010D2 RID: 4306 RVA: 0x00031605 File Offset: 0x0002F805
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static implicit operator float2x2(int v)
		{
			return new float2x2(v);
		}

		// Token: 0x060010D3 RID: 4307 RVA: 0x0003160D File Offset: 0x0002F80D
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static implicit operator float2x2(int2x2 v)
		{
			return new float2x2(v);
		}

		// Token: 0x060010D4 RID: 4308 RVA: 0x00031615 File Offset: 0x0002F815
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static implicit operator float2x2(uint v)
		{
			return new float2x2(v);
		}

		// Token: 0x060010D5 RID: 4309 RVA: 0x0003161D File Offset: 0x0002F81D
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static implicit operator float2x2(uint2x2 v)
		{
			return new float2x2(v);
		}

		// Token: 0x060010D6 RID: 4310 RVA: 0x00031625 File Offset: 0x0002F825
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static explicit operator float2x2(double v)
		{
			return new float2x2(v);
		}

		// Token: 0x060010D7 RID: 4311 RVA: 0x0003162D File Offset: 0x0002F82D
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static explicit operator float2x2(double2x2 v)
		{
			return new float2x2(v);
		}

		// Token: 0x060010D8 RID: 4312 RVA: 0x00031635 File Offset: 0x0002F835
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float2x2 operator *(float2x2 lhs, float2x2 rhs)
		{
			return new float2x2(lhs.c0 * rhs.c0, lhs.c1 * rhs.c1);
		}

		// Token: 0x060010D9 RID: 4313 RVA: 0x0003165E File Offset: 0x0002F85E
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float2x2 operator *(float2x2 lhs, float rhs)
		{
			return new float2x2(lhs.c0 * rhs, lhs.c1 * rhs);
		}

		// Token: 0x060010DA RID: 4314 RVA: 0x0003167D File Offset: 0x0002F87D
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float2x2 operator *(float lhs, float2x2 rhs)
		{
			return new float2x2(lhs * rhs.c0, lhs * rhs.c1);
		}

		// Token: 0x060010DB RID: 4315 RVA: 0x0003169C File Offset: 0x0002F89C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float2x2 operator +(float2x2 lhs, float2x2 rhs)
		{
			return new float2x2(lhs.c0 + rhs.c0, lhs.c1 + rhs.c1);
		}

		// Token: 0x060010DC RID: 4316 RVA: 0x000316C5 File Offset: 0x0002F8C5
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float2x2 operator +(float2x2 lhs, float rhs)
		{
			return new float2x2(lhs.c0 + rhs, lhs.c1 + rhs);
		}

		// Token: 0x060010DD RID: 4317 RVA: 0x000316E4 File Offset: 0x0002F8E4
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float2x2 operator +(float lhs, float2x2 rhs)
		{
			return new float2x2(lhs + rhs.c0, lhs + rhs.c1);
		}

		// Token: 0x060010DE RID: 4318 RVA: 0x00031703 File Offset: 0x0002F903
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float2x2 operator -(float2x2 lhs, float2x2 rhs)
		{
			return new float2x2(lhs.c0 - rhs.c0, lhs.c1 - rhs.c1);
		}

		// Token: 0x060010DF RID: 4319 RVA: 0x0003172C File Offset: 0x0002F92C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float2x2 operator -(float2x2 lhs, float rhs)
		{
			return new float2x2(lhs.c0 - rhs, lhs.c1 - rhs);
		}

		// Token: 0x060010E0 RID: 4320 RVA: 0x0003174B File Offset: 0x0002F94B
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float2x2 operator -(float lhs, float2x2 rhs)
		{
			return new float2x2(lhs - rhs.c0, lhs - rhs.c1);
		}

		// Token: 0x060010E1 RID: 4321 RVA: 0x0003176A File Offset: 0x0002F96A
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float2x2 operator /(float2x2 lhs, float2x2 rhs)
		{
			return new float2x2(lhs.c0 / rhs.c0, lhs.c1 / rhs.c1);
		}

		// Token: 0x060010E2 RID: 4322 RVA: 0x00031793 File Offset: 0x0002F993
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float2x2 operator /(float2x2 lhs, float rhs)
		{
			return new float2x2(lhs.c0 / rhs, lhs.c1 / rhs);
		}

		// Token: 0x060010E3 RID: 4323 RVA: 0x000317B2 File Offset: 0x0002F9B2
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float2x2 operator /(float lhs, float2x2 rhs)
		{
			return new float2x2(lhs / rhs.c0, lhs / rhs.c1);
		}

		// Token: 0x060010E4 RID: 4324 RVA: 0x000317D1 File Offset: 0x0002F9D1
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float2x2 operator %(float2x2 lhs, float2x2 rhs)
		{
			return new float2x2(lhs.c0 % rhs.c0, lhs.c1 % rhs.c1);
		}

		// Token: 0x060010E5 RID: 4325 RVA: 0x000317FA File Offset: 0x0002F9FA
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float2x2 operator %(float2x2 lhs, float rhs)
		{
			return new float2x2(lhs.c0 % rhs, lhs.c1 % rhs);
		}

		// Token: 0x060010E6 RID: 4326 RVA: 0x00031819 File Offset: 0x0002FA19
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float2x2 operator %(float lhs, float2x2 rhs)
		{
			return new float2x2(lhs % rhs.c0, lhs % rhs.c1);
		}

		// Token: 0x060010E7 RID: 4327 RVA: 0x00031838 File Offset: 0x0002FA38
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float2x2 operator ++(float2x2 val)
		{
			float2 @float = float2.op_Increment(val.c0);
			val.c0 = @float;
			float2 float2 = @float;
			@float = float2.op_Increment(val.c1);
			val.c1 = @float;
			return new float2x2(float2, @float);
		}

		// Token: 0x060010E8 RID: 4328 RVA: 0x00031880 File Offset: 0x0002FA80
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float2x2 operator --(float2x2 val)
		{
			float2 @float = float2.op_Decrement(val.c0);
			val.c0 = @float;
			float2 float2 = @float;
			@float = float2.op_Decrement(val.c1);
			val.c1 = @float;
			return new float2x2(float2, @float);
		}

		// Token: 0x060010E9 RID: 4329 RVA: 0x000318C6 File Offset: 0x0002FAC6
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool2x2 operator <(float2x2 lhs, float2x2 rhs)
		{
			return new bool2x2(lhs.c0 < rhs.c0, lhs.c1 < rhs.c1);
		}

		// Token: 0x060010EA RID: 4330 RVA: 0x000318EF File Offset: 0x0002FAEF
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool2x2 operator <(float2x2 lhs, float rhs)
		{
			return new bool2x2(lhs.c0 < rhs, lhs.c1 < rhs);
		}

		// Token: 0x060010EB RID: 4331 RVA: 0x0003190E File Offset: 0x0002FB0E
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool2x2 operator <(float lhs, float2x2 rhs)
		{
			return new bool2x2(lhs < rhs.c0, lhs < rhs.c1);
		}

		// Token: 0x060010EC RID: 4332 RVA: 0x0003192D File Offset: 0x0002FB2D
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool2x2 operator <=(float2x2 lhs, float2x2 rhs)
		{
			return new bool2x2(lhs.c0 <= rhs.c0, lhs.c1 <= rhs.c1);
		}

		// Token: 0x060010ED RID: 4333 RVA: 0x00031956 File Offset: 0x0002FB56
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool2x2 operator <=(float2x2 lhs, float rhs)
		{
			return new bool2x2(lhs.c0 <= rhs, lhs.c1 <= rhs);
		}

		// Token: 0x060010EE RID: 4334 RVA: 0x00031975 File Offset: 0x0002FB75
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool2x2 operator <=(float lhs, float2x2 rhs)
		{
			return new bool2x2(lhs <= rhs.c0, lhs <= rhs.c1);
		}

		// Token: 0x060010EF RID: 4335 RVA: 0x00031994 File Offset: 0x0002FB94
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool2x2 operator >(float2x2 lhs, float2x2 rhs)
		{
			return new bool2x2(lhs.c0 > rhs.c0, lhs.c1 > rhs.c1);
		}

		// Token: 0x060010F0 RID: 4336 RVA: 0x000319BD File Offset: 0x0002FBBD
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool2x2 operator >(float2x2 lhs, float rhs)
		{
			return new bool2x2(lhs.c0 > rhs, lhs.c1 > rhs);
		}

		// Token: 0x060010F1 RID: 4337 RVA: 0x000319DC File Offset: 0x0002FBDC
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool2x2 operator >(float lhs, float2x2 rhs)
		{
			return new bool2x2(lhs > rhs.c0, lhs > rhs.c1);
		}

		// Token: 0x060010F2 RID: 4338 RVA: 0x000319FB File Offset: 0x0002FBFB
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool2x2 operator >=(float2x2 lhs, float2x2 rhs)
		{
			return new bool2x2(lhs.c0 >= rhs.c0, lhs.c1 >= rhs.c1);
		}

		// Token: 0x060010F3 RID: 4339 RVA: 0x00031A24 File Offset: 0x0002FC24
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool2x2 operator >=(float2x2 lhs, float rhs)
		{
			return new bool2x2(lhs.c0 >= rhs, lhs.c1 >= rhs);
		}

		// Token: 0x060010F4 RID: 4340 RVA: 0x00031A43 File Offset: 0x0002FC43
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool2x2 operator >=(float lhs, float2x2 rhs)
		{
			return new bool2x2(lhs >= rhs.c0, lhs >= rhs.c1);
		}

		// Token: 0x060010F5 RID: 4341 RVA: 0x00031A62 File Offset: 0x0002FC62
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float2x2 operator -(float2x2 val)
		{
			return new float2x2(-val.c0, -val.c1);
		}

		// Token: 0x060010F6 RID: 4342 RVA: 0x00031A7F File Offset: 0x0002FC7F
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float2x2 operator +(float2x2 val)
		{
			return new float2x2(+val.c0, +val.c1);
		}

		// Token: 0x060010F7 RID: 4343 RVA: 0x00031A9C File Offset: 0x0002FC9C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool2x2 operator ==(float2x2 lhs, float2x2 rhs)
		{
			return new bool2x2(lhs.c0 == rhs.c0, lhs.c1 == rhs.c1);
		}

		// Token: 0x060010F8 RID: 4344 RVA: 0x00031AC5 File Offset: 0x0002FCC5
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool2x2 operator ==(float2x2 lhs, float rhs)
		{
			return new bool2x2(lhs.c0 == rhs, lhs.c1 == rhs);
		}

		// Token: 0x060010F9 RID: 4345 RVA: 0x00031AE4 File Offset: 0x0002FCE4
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool2x2 operator ==(float lhs, float2x2 rhs)
		{
			return new bool2x2(lhs == rhs.c0, lhs == rhs.c1);
		}

		// Token: 0x060010FA RID: 4346 RVA: 0x00031B03 File Offset: 0x0002FD03
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool2x2 operator !=(float2x2 lhs, float2x2 rhs)
		{
			return new bool2x2(lhs.c0 != rhs.c0, lhs.c1 != rhs.c1);
		}

		// Token: 0x060010FB RID: 4347 RVA: 0x00031B2C File Offset: 0x0002FD2C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool2x2 operator !=(float2x2 lhs, float rhs)
		{
			return new bool2x2(lhs.c0 != rhs, lhs.c1 != rhs);
		}

		// Token: 0x060010FC RID: 4348 RVA: 0x00031B4B File Offset: 0x0002FD4B
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool2x2 operator !=(float lhs, float2x2 rhs)
		{
			return new bool2x2(lhs != rhs.c0, lhs != rhs.c1);
		}

		// Token: 0x170003F8 RID: 1016
		public unsafe ref float2 this[int index]
		{
			get
			{
				fixed (float2x2* ptr = &this)
				{
					return ref *(float2*)(ptr + (IntPtr)index * (IntPtr)sizeof(float2) / (IntPtr)sizeof(float2x2));
				}
			}
		}

		// Token: 0x060010FE RID: 4350 RVA: 0x00031B87 File Offset: 0x0002FD87
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool Equals(float2x2 rhs)
		{
			return this.c0.Equals(rhs.c0) && this.c1.Equals(rhs.c1);
		}

		// Token: 0x060010FF RID: 4351 RVA: 0x00031BB0 File Offset: 0x0002FDB0
		public override bool Equals(object o)
		{
			if (o is float2x2)
			{
				float2x2 float2x = (float2x2)o;
				return this.Equals(float2x);
			}
			return false;
		}

		// Token: 0x06001100 RID: 4352 RVA: 0x00031BD5 File Offset: 0x0002FDD5
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public override int GetHashCode()
		{
			return (int)math.hash(this);
		}

		// Token: 0x06001101 RID: 4353 RVA: 0x00031BE4 File Offset: 0x0002FDE4
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public override string ToString()
		{
			return string.Format("float2x2({0}f, {1}f,  {2}f, {3}f)", new object[]
			{
				this.c0.x,
				this.c1.x,
				this.c0.y,
				this.c1.y
			});
		}

		// Token: 0x06001102 RID: 4354 RVA: 0x00031C50 File Offset: 0x0002FE50
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public string ToString(string format, IFormatProvider formatProvider)
		{
			return string.Format("float2x2({0}f, {1}f,  {2}f, {3}f)", new object[]
			{
				this.c0.x.ToString(format, formatProvider),
				this.c1.x.ToString(format, formatProvider),
				this.c0.y.ToString(format, formatProvider),
				this.c1.y.ToString(format, formatProvider)
			});
		}

		// Token: 0x06001103 RID: 4355 RVA: 0x00031CC4 File Offset: 0x0002FEC4
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float2x2 Rotate(float angle)
		{
			float num;
			float num2;
			math.sincos(angle, out num, out num2);
			return math.float2x2(num2, -num, num, num2);
		}

		// Token: 0x06001104 RID: 4356 RVA: 0x00031CE5 File Offset: 0x0002FEE5
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float2x2 Scale(float s)
		{
			return math.float2x2(s, 0f, 0f, s);
		}

		// Token: 0x06001105 RID: 4357 RVA: 0x00031CF8 File Offset: 0x0002FEF8
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float2x2 Scale(float x, float y)
		{
			return math.float2x2(x, 0f, 0f, y);
		}

		// Token: 0x06001106 RID: 4358 RVA: 0x00031D0B File Offset: 0x0002FF0B
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float2x2 Scale(float2 v)
		{
			return float2x2.Scale(v.x, v.y);
		}

		// Token: 0x0400007F RID: 127
		public float2 c0;

		// Token: 0x04000080 RID: 128
		public float2 c1;

		// Token: 0x04000081 RID: 129
		public static readonly float2x2 identity = new float2x2(1f, 0f, 0f, 1f);

		// Token: 0x04000082 RID: 130
		public static readonly float2x2 zero;
	}
}
