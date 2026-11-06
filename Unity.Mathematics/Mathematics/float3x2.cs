using System;
using System.Runtime.CompilerServices;
using Unity.IL2CPP.CompilerServices;

namespace Unity.Mathematics
{
	// Token: 0x02000023 RID: 35
	[Il2CppEagerStaticClassConstruction]
	[Serializable]
	public struct float3x2 : IEquatable<float3x2>, IFormattable
	{
		// Token: 0x0600124F RID: 4687 RVA: 0x000350F4 File Offset: 0x000332F4
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public float3x2(float3 c0, float3 c1)
		{
			this.c0 = c0;
			this.c1 = c1;
		}

		// Token: 0x06001250 RID: 4688 RVA: 0x00035104 File Offset: 0x00033304
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public float3x2(float m00, float m01, float m10, float m11, float m20, float m21)
		{
			this.c0 = new float3(m00, m10, m20);
			this.c1 = new float3(m01, m11, m21);
		}

		// Token: 0x06001251 RID: 4689 RVA: 0x00035125 File Offset: 0x00033325
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public float3x2(float v)
		{
			this.c0 = v;
			this.c1 = v;
		}

		// Token: 0x06001252 RID: 4690 RVA: 0x00035140 File Offset: 0x00033340
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public float3x2(bool v)
		{
			this.c0 = math.select(new float3(0f), new float3(1f), v);
			this.c1 = math.select(new float3(0f), new float3(1f), v);
		}

		// Token: 0x06001253 RID: 4691 RVA: 0x00035190 File Offset: 0x00033390
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public float3x2(bool3x2 v)
		{
			this.c0 = math.select(new float3(0f), new float3(1f), v.c0);
			this.c1 = math.select(new float3(0f), new float3(1f), v.c1);
		}

		// Token: 0x06001254 RID: 4692 RVA: 0x000351E7 File Offset: 0x000333E7
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public float3x2(int v)
		{
			this.c0 = v;
			this.c1 = v;
		}

		// Token: 0x06001255 RID: 4693 RVA: 0x00035201 File Offset: 0x00033401
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public float3x2(int3x2 v)
		{
			this.c0 = v.c0;
			this.c1 = v.c1;
		}

		// Token: 0x06001256 RID: 4694 RVA: 0x00035225 File Offset: 0x00033425
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public float3x2(uint v)
		{
			this.c0 = v;
			this.c1 = v;
		}

		// Token: 0x06001257 RID: 4695 RVA: 0x0003523F File Offset: 0x0003343F
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public float3x2(uint3x2 v)
		{
			this.c0 = v.c0;
			this.c1 = v.c1;
		}

		// Token: 0x06001258 RID: 4696 RVA: 0x00035263 File Offset: 0x00033463
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public float3x2(double v)
		{
			this.c0 = (float3)v;
			this.c1 = (float3)v;
		}

		// Token: 0x06001259 RID: 4697 RVA: 0x0003527D File Offset: 0x0003347D
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public float3x2(double3x2 v)
		{
			this.c0 = (float3)v.c0;
			this.c1 = (float3)v.c1;
		}

		// Token: 0x0600125A RID: 4698 RVA: 0x000352A1 File Offset: 0x000334A1
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static implicit operator float3x2(float v)
		{
			return new float3x2(v);
		}

		// Token: 0x0600125B RID: 4699 RVA: 0x000352A9 File Offset: 0x000334A9
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static explicit operator float3x2(bool v)
		{
			return new float3x2(v);
		}

		// Token: 0x0600125C RID: 4700 RVA: 0x000352B1 File Offset: 0x000334B1
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static explicit operator float3x2(bool3x2 v)
		{
			return new float3x2(v);
		}

		// Token: 0x0600125D RID: 4701 RVA: 0x000352B9 File Offset: 0x000334B9
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static implicit operator float3x2(int v)
		{
			return new float3x2(v);
		}

		// Token: 0x0600125E RID: 4702 RVA: 0x000352C1 File Offset: 0x000334C1
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static implicit operator float3x2(int3x2 v)
		{
			return new float3x2(v);
		}

		// Token: 0x0600125F RID: 4703 RVA: 0x000352C9 File Offset: 0x000334C9
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static implicit operator float3x2(uint v)
		{
			return new float3x2(v);
		}

		// Token: 0x06001260 RID: 4704 RVA: 0x000352D1 File Offset: 0x000334D1
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static implicit operator float3x2(uint3x2 v)
		{
			return new float3x2(v);
		}

		// Token: 0x06001261 RID: 4705 RVA: 0x000352D9 File Offset: 0x000334D9
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static explicit operator float3x2(double v)
		{
			return new float3x2(v);
		}

		// Token: 0x06001262 RID: 4706 RVA: 0x000352E1 File Offset: 0x000334E1
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static explicit operator float3x2(double3x2 v)
		{
			return new float3x2(v);
		}

		// Token: 0x06001263 RID: 4707 RVA: 0x000352E9 File Offset: 0x000334E9
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float3x2 operator *(float3x2 lhs, float3x2 rhs)
		{
			return new float3x2(lhs.c0 * rhs.c0, lhs.c1 * rhs.c1);
		}

		// Token: 0x06001264 RID: 4708 RVA: 0x00035312 File Offset: 0x00033512
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float3x2 operator *(float3x2 lhs, float rhs)
		{
			return new float3x2(lhs.c0 * rhs, lhs.c1 * rhs);
		}

		// Token: 0x06001265 RID: 4709 RVA: 0x00035331 File Offset: 0x00033531
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float3x2 operator *(float lhs, float3x2 rhs)
		{
			return new float3x2(lhs * rhs.c0, lhs * rhs.c1);
		}

		// Token: 0x06001266 RID: 4710 RVA: 0x00035350 File Offset: 0x00033550
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float3x2 operator +(float3x2 lhs, float3x2 rhs)
		{
			return new float3x2(lhs.c0 + rhs.c0, lhs.c1 + rhs.c1);
		}

		// Token: 0x06001267 RID: 4711 RVA: 0x00035379 File Offset: 0x00033579
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float3x2 operator +(float3x2 lhs, float rhs)
		{
			return new float3x2(lhs.c0 + rhs, lhs.c1 + rhs);
		}

		// Token: 0x06001268 RID: 4712 RVA: 0x00035398 File Offset: 0x00033598
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float3x2 operator +(float lhs, float3x2 rhs)
		{
			return new float3x2(lhs + rhs.c0, lhs + rhs.c1);
		}

		// Token: 0x06001269 RID: 4713 RVA: 0x000353B7 File Offset: 0x000335B7
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float3x2 operator -(float3x2 lhs, float3x2 rhs)
		{
			return new float3x2(lhs.c0 - rhs.c0, lhs.c1 - rhs.c1);
		}

		// Token: 0x0600126A RID: 4714 RVA: 0x000353E0 File Offset: 0x000335E0
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float3x2 operator -(float3x2 lhs, float rhs)
		{
			return new float3x2(lhs.c0 - rhs, lhs.c1 - rhs);
		}

		// Token: 0x0600126B RID: 4715 RVA: 0x000353FF File Offset: 0x000335FF
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float3x2 operator -(float lhs, float3x2 rhs)
		{
			return new float3x2(lhs - rhs.c0, lhs - rhs.c1);
		}

		// Token: 0x0600126C RID: 4716 RVA: 0x0003541E File Offset: 0x0003361E
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float3x2 operator /(float3x2 lhs, float3x2 rhs)
		{
			return new float3x2(lhs.c0 / rhs.c0, lhs.c1 / rhs.c1);
		}

		// Token: 0x0600126D RID: 4717 RVA: 0x00035447 File Offset: 0x00033647
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float3x2 operator /(float3x2 lhs, float rhs)
		{
			return new float3x2(lhs.c0 / rhs, lhs.c1 / rhs);
		}

		// Token: 0x0600126E RID: 4718 RVA: 0x00035466 File Offset: 0x00033666
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float3x2 operator /(float lhs, float3x2 rhs)
		{
			return new float3x2(lhs / rhs.c0, lhs / rhs.c1);
		}

		// Token: 0x0600126F RID: 4719 RVA: 0x00035485 File Offset: 0x00033685
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float3x2 operator %(float3x2 lhs, float3x2 rhs)
		{
			return new float3x2(lhs.c0 % rhs.c0, lhs.c1 % rhs.c1);
		}

		// Token: 0x06001270 RID: 4720 RVA: 0x000354AE File Offset: 0x000336AE
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float3x2 operator %(float3x2 lhs, float rhs)
		{
			return new float3x2(lhs.c0 % rhs, lhs.c1 % rhs);
		}

		// Token: 0x06001271 RID: 4721 RVA: 0x000354CD File Offset: 0x000336CD
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float3x2 operator %(float lhs, float3x2 rhs)
		{
			return new float3x2(lhs % rhs.c0, lhs % rhs.c1);
		}

		// Token: 0x06001272 RID: 4722 RVA: 0x000354EC File Offset: 0x000336EC
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float3x2 operator ++(float3x2 val)
		{
			float3 @float = float3.op_Increment(val.c0);
			val.c0 = @float;
			float3 float2 = @float;
			@float = float3.op_Increment(val.c1);
			val.c1 = @float;
			return new float3x2(float2, @float);
		}

		// Token: 0x06001273 RID: 4723 RVA: 0x00035534 File Offset: 0x00033734
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float3x2 operator --(float3x2 val)
		{
			float3 @float = float3.op_Decrement(val.c0);
			val.c0 = @float;
			float3 float2 = @float;
			@float = float3.op_Decrement(val.c1);
			val.c1 = @float;
			return new float3x2(float2, @float);
		}

		// Token: 0x06001274 RID: 4724 RVA: 0x0003557A File Offset: 0x0003377A
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool3x2 operator <(float3x2 lhs, float3x2 rhs)
		{
			return new bool3x2(lhs.c0 < rhs.c0, lhs.c1 < rhs.c1);
		}

		// Token: 0x06001275 RID: 4725 RVA: 0x000355A3 File Offset: 0x000337A3
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool3x2 operator <(float3x2 lhs, float rhs)
		{
			return new bool3x2(lhs.c0 < rhs, lhs.c1 < rhs);
		}

		// Token: 0x06001276 RID: 4726 RVA: 0x000355C2 File Offset: 0x000337C2
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool3x2 operator <(float lhs, float3x2 rhs)
		{
			return new bool3x2(lhs < rhs.c0, lhs < rhs.c1);
		}

		// Token: 0x06001277 RID: 4727 RVA: 0x000355E1 File Offset: 0x000337E1
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool3x2 operator <=(float3x2 lhs, float3x2 rhs)
		{
			return new bool3x2(lhs.c0 <= rhs.c0, lhs.c1 <= rhs.c1);
		}

		// Token: 0x06001278 RID: 4728 RVA: 0x0003560A File Offset: 0x0003380A
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool3x2 operator <=(float3x2 lhs, float rhs)
		{
			return new bool3x2(lhs.c0 <= rhs, lhs.c1 <= rhs);
		}

		// Token: 0x06001279 RID: 4729 RVA: 0x00035629 File Offset: 0x00033829
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool3x2 operator <=(float lhs, float3x2 rhs)
		{
			return new bool3x2(lhs <= rhs.c0, lhs <= rhs.c1);
		}

		// Token: 0x0600127A RID: 4730 RVA: 0x00035648 File Offset: 0x00033848
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool3x2 operator >(float3x2 lhs, float3x2 rhs)
		{
			return new bool3x2(lhs.c0 > rhs.c0, lhs.c1 > rhs.c1);
		}

		// Token: 0x0600127B RID: 4731 RVA: 0x00035671 File Offset: 0x00033871
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool3x2 operator >(float3x2 lhs, float rhs)
		{
			return new bool3x2(lhs.c0 > rhs, lhs.c1 > rhs);
		}

		// Token: 0x0600127C RID: 4732 RVA: 0x00035690 File Offset: 0x00033890
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool3x2 operator >(float lhs, float3x2 rhs)
		{
			return new bool3x2(lhs > rhs.c0, lhs > rhs.c1);
		}

		// Token: 0x0600127D RID: 4733 RVA: 0x000356AF File Offset: 0x000338AF
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool3x2 operator >=(float3x2 lhs, float3x2 rhs)
		{
			return new bool3x2(lhs.c0 >= rhs.c0, lhs.c1 >= rhs.c1);
		}

		// Token: 0x0600127E RID: 4734 RVA: 0x000356D8 File Offset: 0x000338D8
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool3x2 operator >=(float3x2 lhs, float rhs)
		{
			return new bool3x2(lhs.c0 >= rhs, lhs.c1 >= rhs);
		}

		// Token: 0x0600127F RID: 4735 RVA: 0x000356F7 File Offset: 0x000338F7
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool3x2 operator >=(float lhs, float3x2 rhs)
		{
			return new bool3x2(lhs >= rhs.c0, lhs >= rhs.c1);
		}

		// Token: 0x06001280 RID: 4736 RVA: 0x00035716 File Offset: 0x00033916
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float3x2 operator -(float3x2 val)
		{
			return new float3x2(-val.c0, -val.c1);
		}

		// Token: 0x06001281 RID: 4737 RVA: 0x00035733 File Offset: 0x00033933
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float3x2 operator +(float3x2 val)
		{
			return new float3x2(+val.c0, +val.c1);
		}

		// Token: 0x06001282 RID: 4738 RVA: 0x00035750 File Offset: 0x00033950
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool3x2 operator ==(float3x2 lhs, float3x2 rhs)
		{
			return new bool3x2(lhs.c0 == rhs.c0, lhs.c1 == rhs.c1);
		}

		// Token: 0x06001283 RID: 4739 RVA: 0x00035779 File Offset: 0x00033979
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool3x2 operator ==(float3x2 lhs, float rhs)
		{
			return new bool3x2(lhs.c0 == rhs, lhs.c1 == rhs);
		}

		// Token: 0x06001284 RID: 4740 RVA: 0x00035798 File Offset: 0x00033998
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool3x2 operator ==(float lhs, float3x2 rhs)
		{
			return new bool3x2(lhs == rhs.c0, lhs == rhs.c1);
		}

		// Token: 0x06001285 RID: 4741 RVA: 0x000357B7 File Offset: 0x000339B7
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool3x2 operator !=(float3x2 lhs, float3x2 rhs)
		{
			return new bool3x2(lhs.c0 != rhs.c0, lhs.c1 != rhs.c1);
		}

		// Token: 0x06001286 RID: 4742 RVA: 0x000357E0 File Offset: 0x000339E0
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool3x2 operator !=(float3x2 lhs, float rhs)
		{
			return new bool3x2(lhs.c0 != rhs, lhs.c1 != rhs);
		}

		// Token: 0x06001287 RID: 4743 RVA: 0x000357FF File Offset: 0x000339FF
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool3x2 operator !=(float lhs, float3x2 rhs)
		{
			return new bool3x2(lhs != rhs.c0, lhs != rhs.c1);
		}

		// Token: 0x17000471 RID: 1137
		public unsafe ref float3 this[int index]
		{
			get
			{
				fixed (float3x2* ptr = &this)
				{
					return ref *(float3*)(ptr + (IntPtr)index * (IntPtr)sizeof(float3) / (IntPtr)sizeof(float3x2));
				}
			}
		}

		// Token: 0x06001289 RID: 4745 RVA: 0x0003583B File Offset: 0x00033A3B
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool Equals(float3x2 rhs)
		{
			return this.c0.Equals(rhs.c0) && this.c1.Equals(rhs.c1);
		}

		// Token: 0x0600128A RID: 4746 RVA: 0x00035864 File Offset: 0x00033A64
		public override bool Equals(object o)
		{
			if (o is float3x2)
			{
				float3x2 float3x = (float3x2)o;
				return this.Equals(float3x);
			}
			return false;
		}

		// Token: 0x0600128B RID: 4747 RVA: 0x00035889 File Offset: 0x00033A89
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public override int GetHashCode()
		{
			return (int)math.hash(this);
		}

		// Token: 0x0600128C RID: 4748 RVA: 0x00035898 File Offset: 0x00033A98
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public override string ToString()
		{
			return string.Format("float3x2({0}f, {1}f,  {2}f, {3}f,  {4}f, {5}f)", new object[]
			{
				this.c0.x,
				this.c1.x,
				this.c0.y,
				this.c1.y,
				this.c0.z,
				this.c1.z
			});
		}

		// Token: 0x0600128D RID: 4749 RVA: 0x00035928 File Offset: 0x00033B28
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public string ToString(string format, IFormatProvider formatProvider)
		{
			return string.Format("float3x2({0}f, {1}f,  {2}f, {3}f,  {4}f, {5}f)", new object[]
			{
				this.c0.x.ToString(format, formatProvider),
				this.c1.x.ToString(format, formatProvider),
				this.c0.y.ToString(format, formatProvider),
				this.c1.y.ToString(format, formatProvider),
				this.c0.z.ToString(format, formatProvider),
				this.c1.z.ToString(format, formatProvider)
			});
		}

		// Token: 0x04000090 RID: 144
		public float3 c0;

		// Token: 0x04000091 RID: 145
		public float3 c1;

		// Token: 0x04000092 RID: 146
		public static readonly float3x2 zero;
	}
}
