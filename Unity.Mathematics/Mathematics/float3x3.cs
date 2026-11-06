using System;
using System.Runtime.CompilerServices;
using Unity.IL2CPP.CompilerServices;

namespace Unity.Mathematics
{
	// Token: 0x02000024 RID: 36
	[Il2CppEagerStaticClassConstruction]
	[Serializable]
	public struct float3x3 : IEquatable<float3x3>, IFormattable
	{
		// Token: 0x0600128E RID: 4750 RVA: 0x000359C3 File Offset: 0x00033BC3
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public float3x3(float3 c0, float3 c1, float3 c2)
		{
			this.c0 = c0;
			this.c1 = c1;
			this.c2 = c2;
		}

		// Token: 0x0600128F RID: 4751 RVA: 0x000359DA File Offset: 0x00033BDA
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public float3x3(float m00, float m01, float m02, float m10, float m11, float m12, float m20, float m21, float m22)
		{
			this.c0 = new float3(m00, m10, m20);
			this.c1 = new float3(m01, m11, m21);
			this.c2 = new float3(m02, m12, m22);
		}

		// Token: 0x06001290 RID: 4752 RVA: 0x00035A0C File Offset: 0x00033C0C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public float3x3(float v)
		{
			this.c0 = v;
			this.c1 = v;
			this.c2 = v;
		}

		// Token: 0x06001291 RID: 4753 RVA: 0x00035A34 File Offset: 0x00033C34
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public float3x3(bool v)
		{
			this.c0 = math.select(new float3(0f), new float3(1f), v);
			this.c1 = math.select(new float3(0f), new float3(1f), v);
			this.c2 = math.select(new float3(0f), new float3(1f), v);
		}

		// Token: 0x06001292 RID: 4754 RVA: 0x00035AA4 File Offset: 0x00033CA4
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public float3x3(bool3x3 v)
		{
			this.c0 = math.select(new float3(0f), new float3(1f), v.c0);
			this.c1 = math.select(new float3(0f), new float3(1f), v.c1);
			this.c2 = math.select(new float3(0f), new float3(1f), v.c2);
		}

		// Token: 0x06001293 RID: 4755 RVA: 0x00035B20 File Offset: 0x00033D20
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public float3x3(int v)
		{
			this.c0 = v;
			this.c1 = v;
			this.c2 = v;
		}

		// Token: 0x06001294 RID: 4756 RVA: 0x00035B46 File Offset: 0x00033D46
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public float3x3(int3x3 v)
		{
			this.c0 = v.c0;
			this.c1 = v.c1;
			this.c2 = v.c2;
		}

		// Token: 0x06001295 RID: 4757 RVA: 0x00035B7B File Offset: 0x00033D7B
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public float3x3(uint v)
		{
			this.c0 = v;
			this.c1 = v;
			this.c2 = v;
		}

		// Token: 0x06001296 RID: 4758 RVA: 0x00035BA1 File Offset: 0x00033DA1
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public float3x3(uint3x3 v)
		{
			this.c0 = v.c0;
			this.c1 = v.c1;
			this.c2 = v.c2;
		}

		// Token: 0x06001297 RID: 4759 RVA: 0x00035BD6 File Offset: 0x00033DD6
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public float3x3(double v)
		{
			this.c0 = (float3)v;
			this.c1 = (float3)v;
			this.c2 = (float3)v;
		}

		// Token: 0x06001298 RID: 4760 RVA: 0x00035BFC File Offset: 0x00033DFC
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public float3x3(double3x3 v)
		{
			this.c0 = (float3)v.c0;
			this.c1 = (float3)v.c1;
			this.c2 = (float3)v.c2;
		}

		// Token: 0x06001299 RID: 4761 RVA: 0x00035C31 File Offset: 0x00033E31
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static implicit operator float3x3(float v)
		{
			return new float3x3(v);
		}

		// Token: 0x0600129A RID: 4762 RVA: 0x00035C39 File Offset: 0x00033E39
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static explicit operator float3x3(bool v)
		{
			return new float3x3(v);
		}

		// Token: 0x0600129B RID: 4763 RVA: 0x00035C41 File Offset: 0x00033E41
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static explicit operator float3x3(bool3x3 v)
		{
			return new float3x3(v);
		}

		// Token: 0x0600129C RID: 4764 RVA: 0x00035C49 File Offset: 0x00033E49
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static implicit operator float3x3(int v)
		{
			return new float3x3(v);
		}

		// Token: 0x0600129D RID: 4765 RVA: 0x00035C51 File Offset: 0x00033E51
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static implicit operator float3x3(int3x3 v)
		{
			return new float3x3(v);
		}

		// Token: 0x0600129E RID: 4766 RVA: 0x00035C59 File Offset: 0x00033E59
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static implicit operator float3x3(uint v)
		{
			return new float3x3(v);
		}

		// Token: 0x0600129F RID: 4767 RVA: 0x00035C61 File Offset: 0x00033E61
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static implicit operator float3x3(uint3x3 v)
		{
			return new float3x3(v);
		}

		// Token: 0x060012A0 RID: 4768 RVA: 0x00035C69 File Offset: 0x00033E69
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static explicit operator float3x3(double v)
		{
			return new float3x3(v);
		}

		// Token: 0x060012A1 RID: 4769 RVA: 0x00035C71 File Offset: 0x00033E71
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static explicit operator float3x3(double3x3 v)
		{
			return new float3x3(v);
		}

		// Token: 0x060012A2 RID: 4770 RVA: 0x00035C79 File Offset: 0x00033E79
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float3x3 operator *(float3x3 lhs, float3x3 rhs)
		{
			return new float3x3(lhs.c0 * rhs.c0, lhs.c1 * rhs.c1, lhs.c2 * rhs.c2);
		}

		// Token: 0x060012A3 RID: 4771 RVA: 0x00035CB3 File Offset: 0x00033EB3
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float3x3 operator *(float3x3 lhs, float rhs)
		{
			return new float3x3(lhs.c0 * rhs, lhs.c1 * rhs, lhs.c2 * rhs);
		}

		// Token: 0x060012A4 RID: 4772 RVA: 0x00035CDE File Offset: 0x00033EDE
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float3x3 operator *(float lhs, float3x3 rhs)
		{
			return new float3x3(lhs * rhs.c0, lhs * rhs.c1, lhs * rhs.c2);
		}

		// Token: 0x060012A5 RID: 4773 RVA: 0x00035D09 File Offset: 0x00033F09
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float3x3 operator +(float3x3 lhs, float3x3 rhs)
		{
			return new float3x3(lhs.c0 + rhs.c0, lhs.c1 + rhs.c1, lhs.c2 + rhs.c2);
		}

		// Token: 0x060012A6 RID: 4774 RVA: 0x00035D43 File Offset: 0x00033F43
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float3x3 operator +(float3x3 lhs, float rhs)
		{
			return new float3x3(lhs.c0 + rhs, lhs.c1 + rhs, lhs.c2 + rhs);
		}

		// Token: 0x060012A7 RID: 4775 RVA: 0x00035D6E File Offset: 0x00033F6E
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float3x3 operator +(float lhs, float3x3 rhs)
		{
			return new float3x3(lhs + rhs.c0, lhs + rhs.c1, lhs + rhs.c2);
		}

		// Token: 0x060012A8 RID: 4776 RVA: 0x00035D99 File Offset: 0x00033F99
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float3x3 operator -(float3x3 lhs, float3x3 rhs)
		{
			return new float3x3(lhs.c0 - rhs.c0, lhs.c1 - rhs.c1, lhs.c2 - rhs.c2);
		}

		// Token: 0x060012A9 RID: 4777 RVA: 0x00035DD3 File Offset: 0x00033FD3
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float3x3 operator -(float3x3 lhs, float rhs)
		{
			return new float3x3(lhs.c0 - rhs, lhs.c1 - rhs, lhs.c2 - rhs);
		}

		// Token: 0x060012AA RID: 4778 RVA: 0x00035DFE File Offset: 0x00033FFE
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float3x3 operator -(float lhs, float3x3 rhs)
		{
			return new float3x3(lhs - rhs.c0, lhs - rhs.c1, lhs - rhs.c2);
		}

		// Token: 0x060012AB RID: 4779 RVA: 0x00035E29 File Offset: 0x00034029
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float3x3 operator /(float3x3 lhs, float3x3 rhs)
		{
			return new float3x3(lhs.c0 / rhs.c0, lhs.c1 / rhs.c1, lhs.c2 / rhs.c2);
		}

		// Token: 0x060012AC RID: 4780 RVA: 0x00035E63 File Offset: 0x00034063
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float3x3 operator /(float3x3 lhs, float rhs)
		{
			return new float3x3(lhs.c0 / rhs, lhs.c1 / rhs, lhs.c2 / rhs);
		}

		// Token: 0x060012AD RID: 4781 RVA: 0x00035E8E File Offset: 0x0003408E
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float3x3 operator /(float lhs, float3x3 rhs)
		{
			return new float3x3(lhs / rhs.c0, lhs / rhs.c1, lhs / rhs.c2);
		}

		// Token: 0x060012AE RID: 4782 RVA: 0x00035EB9 File Offset: 0x000340B9
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float3x3 operator %(float3x3 lhs, float3x3 rhs)
		{
			return new float3x3(lhs.c0 % rhs.c0, lhs.c1 % rhs.c1, lhs.c2 % rhs.c2);
		}

		// Token: 0x060012AF RID: 4783 RVA: 0x00035EF3 File Offset: 0x000340F3
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float3x3 operator %(float3x3 lhs, float rhs)
		{
			return new float3x3(lhs.c0 % rhs, lhs.c1 % rhs, lhs.c2 % rhs);
		}

		// Token: 0x060012B0 RID: 4784 RVA: 0x00035F1E File Offset: 0x0003411E
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float3x3 operator %(float lhs, float3x3 rhs)
		{
			return new float3x3(lhs % rhs.c0, lhs % rhs.c1, lhs % rhs.c2);
		}

		// Token: 0x060012B1 RID: 4785 RVA: 0x00035F4C File Offset: 0x0003414C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float3x3 operator ++(float3x3 val)
		{
			float3 @float = float3.op_Increment(val.c0);
			val.c0 = @float;
			float3 float2 = @float;
			@float = float3.op_Increment(val.c1);
			val.c1 = @float;
			float3 float3 = @float;
			@float = float3.op_Increment(val.c2);
			val.c2 = @float;
			return new float3x3(float2, float3, @float);
		}

		// Token: 0x060012B2 RID: 4786 RVA: 0x00035FAC File Offset: 0x000341AC
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float3x3 operator --(float3x3 val)
		{
			float3 @float = float3.op_Decrement(val.c0);
			val.c0 = @float;
			float3 float2 = @float;
			@float = float3.op_Decrement(val.c1);
			val.c1 = @float;
			float3 float3 = @float;
			@float = float3.op_Decrement(val.c2);
			val.c2 = @float;
			return new float3x3(float2, float3, @float);
		}

		// Token: 0x060012B3 RID: 4787 RVA: 0x0003600C File Offset: 0x0003420C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool3x3 operator <(float3x3 lhs, float3x3 rhs)
		{
			return new bool3x3(lhs.c0 < rhs.c0, lhs.c1 < rhs.c1, lhs.c2 < rhs.c2);
		}

		// Token: 0x060012B4 RID: 4788 RVA: 0x00036046 File Offset: 0x00034246
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool3x3 operator <(float3x3 lhs, float rhs)
		{
			return new bool3x3(lhs.c0 < rhs, lhs.c1 < rhs, lhs.c2 < rhs);
		}

		// Token: 0x060012B5 RID: 4789 RVA: 0x00036071 File Offset: 0x00034271
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool3x3 operator <(float lhs, float3x3 rhs)
		{
			return new bool3x3(lhs < rhs.c0, lhs < rhs.c1, lhs < rhs.c2);
		}

		// Token: 0x060012B6 RID: 4790 RVA: 0x0003609C File Offset: 0x0003429C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool3x3 operator <=(float3x3 lhs, float3x3 rhs)
		{
			return new bool3x3(lhs.c0 <= rhs.c0, lhs.c1 <= rhs.c1, lhs.c2 <= rhs.c2);
		}

		// Token: 0x060012B7 RID: 4791 RVA: 0x000360D6 File Offset: 0x000342D6
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool3x3 operator <=(float3x3 lhs, float rhs)
		{
			return new bool3x3(lhs.c0 <= rhs, lhs.c1 <= rhs, lhs.c2 <= rhs);
		}

		// Token: 0x060012B8 RID: 4792 RVA: 0x00036101 File Offset: 0x00034301
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool3x3 operator <=(float lhs, float3x3 rhs)
		{
			return new bool3x3(lhs <= rhs.c0, lhs <= rhs.c1, lhs <= rhs.c2);
		}

		// Token: 0x060012B9 RID: 4793 RVA: 0x0003612C File Offset: 0x0003432C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool3x3 operator >(float3x3 lhs, float3x3 rhs)
		{
			return new bool3x3(lhs.c0 > rhs.c0, lhs.c1 > rhs.c1, lhs.c2 > rhs.c2);
		}

		// Token: 0x060012BA RID: 4794 RVA: 0x00036166 File Offset: 0x00034366
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool3x3 operator >(float3x3 lhs, float rhs)
		{
			return new bool3x3(lhs.c0 > rhs, lhs.c1 > rhs, lhs.c2 > rhs);
		}

		// Token: 0x060012BB RID: 4795 RVA: 0x00036191 File Offset: 0x00034391
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool3x3 operator >(float lhs, float3x3 rhs)
		{
			return new bool3x3(lhs > rhs.c0, lhs > rhs.c1, lhs > rhs.c2);
		}

		// Token: 0x060012BC RID: 4796 RVA: 0x000361BC File Offset: 0x000343BC
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool3x3 operator >=(float3x3 lhs, float3x3 rhs)
		{
			return new bool3x3(lhs.c0 >= rhs.c0, lhs.c1 >= rhs.c1, lhs.c2 >= rhs.c2);
		}

		// Token: 0x060012BD RID: 4797 RVA: 0x000361F6 File Offset: 0x000343F6
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool3x3 operator >=(float3x3 lhs, float rhs)
		{
			return new bool3x3(lhs.c0 >= rhs, lhs.c1 >= rhs, lhs.c2 >= rhs);
		}

		// Token: 0x060012BE RID: 4798 RVA: 0x00036221 File Offset: 0x00034421
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool3x3 operator >=(float lhs, float3x3 rhs)
		{
			return new bool3x3(lhs >= rhs.c0, lhs >= rhs.c1, lhs >= rhs.c2);
		}

		// Token: 0x060012BF RID: 4799 RVA: 0x0003624C File Offset: 0x0003444C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float3x3 operator -(float3x3 val)
		{
			return new float3x3(-val.c0, -val.c1, -val.c2);
		}

		// Token: 0x060012C0 RID: 4800 RVA: 0x00036274 File Offset: 0x00034474
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float3x3 operator +(float3x3 val)
		{
			return new float3x3(+val.c0, +val.c1, +val.c2);
		}

		// Token: 0x060012C1 RID: 4801 RVA: 0x0003629C File Offset: 0x0003449C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool3x3 operator ==(float3x3 lhs, float3x3 rhs)
		{
			return new bool3x3(lhs.c0 == rhs.c0, lhs.c1 == rhs.c1, lhs.c2 == rhs.c2);
		}

		// Token: 0x060012C2 RID: 4802 RVA: 0x000362D6 File Offset: 0x000344D6
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool3x3 operator ==(float3x3 lhs, float rhs)
		{
			return new bool3x3(lhs.c0 == rhs, lhs.c1 == rhs, lhs.c2 == rhs);
		}

		// Token: 0x060012C3 RID: 4803 RVA: 0x00036301 File Offset: 0x00034501
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool3x3 operator ==(float lhs, float3x3 rhs)
		{
			return new bool3x3(lhs == rhs.c0, lhs == rhs.c1, lhs == rhs.c2);
		}

		// Token: 0x060012C4 RID: 4804 RVA: 0x0003632C File Offset: 0x0003452C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool3x3 operator !=(float3x3 lhs, float3x3 rhs)
		{
			return new bool3x3(lhs.c0 != rhs.c0, lhs.c1 != rhs.c1, lhs.c2 != rhs.c2);
		}

		// Token: 0x060012C5 RID: 4805 RVA: 0x00036366 File Offset: 0x00034566
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool3x3 operator !=(float3x3 lhs, float rhs)
		{
			return new bool3x3(lhs.c0 != rhs, lhs.c1 != rhs, lhs.c2 != rhs);
		}

		// Token: 0x060012C6 RID: 4806 RVA: 0x00036391 File Offset: 0x00034591
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool3x3 operator !=(float lhs, float3x3 rhs)
		{
			return new bool3x3(lhs != rhs.c0, lhs != rhs.c1, lhs != rhs.c2);
		}

		// Token: 0x17000472 RID: 1138
		public unsafe ref float3 this[int index]
		{
			get
			{
				fixed (float3x3* ptr = &this)
				{
					return ref *(float3*)(ptr + (IntPtr)index * (IntPtr)sizeof(float3) / (IntPtr)sizeof(float3x3));
				}
			}
		}

		// Token: 0x060012C8 RID: 4808 RVA: 0x000363D7 File Offset: 0x000345D7
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool Equals(float3x3 rhs)
		{
			return this.c0.Equals(rhs.c0) && this.c1.Equals(rhs.c1) && this.c2.Equals(rhs.c2);
		}

		// Token: 0x060012C9 RID: 4809 RVA: 0x00036414 File Offset: 0x00034614
		public override bool Equals(object o)
		{
			if (o is float3x3)
			{
				float3x3 float3x = (float3x3)o;
				return this.Equals(float3x);
			}
			return false;
		}

		// Token: 0x060012CA RID: 4810 RVA: 0x00036439 File Offset: 0x00034639
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public override int GetHashCode()
		{
			return (int)math.hash(this);
		}

		// Token: 0x060012CB RID: 4811 RVA: 0x00036448 File Offset: 0x00034648
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public override string ToString()
		{
			return string.Format("float3x3({0}f, {1}f, {2}f,  {3}f, {4}f, {5}f,  {6}f, {7}f, {8}f)", new object[]
			{
				this.c0.x,
				this.c1.x,
				this.c2.x,
				this.c0.y,
				this.c1.y,
				this.c2.y,
				this.c0.z,
				this.c1.z,
				this.c2.z
			});
		}

		// Token: 0x060012CC RID: 4812 RVA: 0x00036514 File Offset: 0x00034714
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public string ToString(string format, IFormatProvider formatProvider)
		{
			return string.Format("float3x3({0}f, {1}f, {2}f,  {3}f, {4}f, {5}f,  {6}f, {7}f, {8}f)", new object[]
			{
				this.c0.x.ToString(format, formatProvider),
				this.c1.x.ToString(format, formatProvider),
				this.c2.x.ToString(format, formatProvider),
				this.c0.y.ToString(format, formatProvider),
				this.c1.y.ToString(format, formatProvider),
				this.c2.y.ToString(format, formatProvider),
				this.c0.z.ToString(format, formatProvider),
				this.c1.z.ToString(format, formatProvider),
				this.c2.z.ToString(format, formatProvider)
			});
		}

		// Token: 0x060012CD RID: 4813 RVA: 0x000365EF File Offset: 0x000347EF
		public float3x3(float4x4 f4x4)
		{
			this.c0 = f4x4.c0.xyz;
			this.c1 = f4x4.c1.xyz;
			this.c2 = f4x4.c2.xyz;
		}

		// Token: 0x060012CE RID: 4814 RVA: 0x00036628 File Offset: 0x00034828
		public float3x3(quaternion q)
		{
			float4 value = q.value;
			float4 @float = value + value;
			uint3 @uint = math.uint3(2147483648U, 0U, 2147483648U);
			uint3 uint2 = math.uint3(2147483648U, 2147483648U, 0U);
			uint3 uint3 = math.uint3(0U, 2147483648U, 2147483648U);
			this.c0 = @float.y * math.asfloat(math.asuint(value.yxw) ^ @uint) - @float.z * math.asfloat(math.asuint(value.zwx) ^ uint3) + math.float3(1f, 0f, 0f);
			this.c1 = @float.z * math.asfloat(math.asuint(value.wzy) ^ uint2) - @float.x * math.asfloat(math.asuint(value.yxw) ^ @uint) + math.float3(0f, 1f, 0f);
			this.c2 = @float.x * math.asfloat(math.asuint(value.zwx) ^ uint3) - @float.y * math.asfloat(math.asuint(value.wzy) ^ uint2) + math.float3(0f, 0f, 1f);
		}

		// Token: 0x060012CF RID: 4815 RVA: 0x000367B4 File Offset: 0x000349B4
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float3x3 AxisAngle(float3 axis, float angle)
		{
			float num;
			float num2;
			math.sincos(angle, out num, out num2);
			float3 @float = axis;
			float3 yzx = @float.yzx;
			float3 zxy = @float.zxy;
			float3 float2 = @float - @float * num2;
			float4 float3 = math.float4(@float * num, num2);
			uint3 @uint = math.uint3(0U, 0U, 2147483648U);
			uint3 uint2 = math.uint3(2147483648U, 0U, 0U);
			uint3 uint3 = math.uint3(0U, 2147483648U, 0U);
			return math.float3x3(@float.x * float2 + math.asfloat(math.asuint(float3.wzy) ^ @uint), @float.y * float2 + math.asfloat(math.asuint(float3.zwx) ^ uint2), @float.z * float2 + math.asfloat(math.asuint(float3.yxw) ^ uint3));
		}

		// Token: 0x060012D0 RID: 4816 RVA: 0x000368A4 File Offset: 0x00034AA4
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float3x3 EulerXYZ(float3 xyz)
		{
			float3 @float;
			float3 float2;
			math.sincos(xyz, out @float, out float2);
			return math.float3x3(float2.y * float2.z, float2.z * @float.x * @float.y - float2.x * @float.z, float2.x * float2.z * @float.y + @float.x * @float.z, float2.y * @float.z, float2.x * float2.z + @float.x * @float.y * @float.z, float2.x * @float.y * @float.z - float2.z * @float.x, -@float.y, float2.y * @float.x, float2.x * float2.y);
		}

		// Token: 0x060012D1 RID: 4817 RVA: 0x00036984 File Offset: 0x00034B84
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float3x3 EulerXZY(float3 xyz)
		{
			float3 @float;
			float3 float2;
			math.sincos(xyz, out @float, out float2);
			return math.float3x3(float2.y * float2.z, @float.x * @float.y - float2.x * float2.y * @float.z, float2.x * @float.y + float2.y * @float.x * @float.z, @float.z, float2.x * float2.z, -float2.z * @float.x, -float2.z * @float.y, float2.y * @float.x + float2.x * @float.y * @float.z, float2.x * float2.y - @float.x * @float.y * @float.z);
		}

		// Token: 0x060012D2 RID: 4818 RVA: 0x00036A64 File Offset: 0x00034C64
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float3x3 EulerYXZ(float3 xyz)
		{
			float3 @float;
			float3 float2;
			math.sincos(xyz, out @float, out float2);
			return math.float3x3(float2.y * float2.z - @float.x * @float.y * @float.z, -float2.x * @float.z, float2.z * @float.y + float2.y * @float.x * @float.z, float2.z * @float.x * @float.y + float2.y * @float.z, float2.x * float2.z, @float.y * @float.z - float2.y * float2.z * @float.x, -float2.x * @float.y, @float.x, float2.x * float2.y);
		}

		// Token: 0x060012D3 RID: 4819 RVA: 0x00036B44 File Offset: 0x00034D44
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float3x3 EulerYZX(float3 xyz)
		{
			float3 @float;
			float3 float2;
			math.sincos(xyz, out @float, out float2);
			return math.float3x3(float2.y * float2.z, -@float.z, float2.z * @float.y, @float.x * @float.y + float2.x * float2.y * @float.z, float2.x * float2.z, float2.x * @float.y * @float.z - float2.y * @float.x, float2.y * @float.x * @float.z - float2.x * @float.y, float2.z * @float.x, float2.x * float2.y + @float.x * @float.y * @float.z);
		}

		// Token: 0x060012D4 RID: 4820 RVA: 0x00036C24 File Offset: 0x00034E24
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float3x3 EulerZXY(float3 xyz)
		{
			float3 @float;
			float3 float2;
			math.sincos(xyz, out @float, out float2);
			return math.float3x3(float2.y * float2.z + @float.x * @float.y * @float.z, float2.z * @float.x * @float.y - float2.y * @float.z, float2.x * @float.y, float2.x * @float.z, float2.x * float2.z, -@float.x, float2.y * @float.x * @float.z - float2.z * @float.y, float2.y * float2.z * @float.x + @float.y * @float.z, float2.x * float2.y);
		}

		// Token: 0x060012D5 RID: 4821 RVA: 0x00036D04 File Offset: 0x00034F04
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float3x3 EulerZYX(float3 xyz)
		{
			float3 @float;
			float3 float2;
			math.sincos(xyz, out @float, out float2);
			return math.float3x3(float2.y * float2.z, -float2.y * @float.z, @float.y, float2.z * @float.x * @float.y + float2.x * @float.z, float2.x * float2.z - @float.x * @float.y * @float.z, -float2.y * @float.x, @float.x * @float.z - float2.x * float2.z * @float.y, float2.z * @float.x + float2.x * @float.y * @float.z, float2.x * float2.y);
		}

		// Token: 0x060012D6 RID: 4822 RVA: 0x00036DE4 File Offset: 0x00034FE4
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float3x3 EulerXYZ(float x, float y, float z)
		{
			return float3x3.EulerXYZ(math.float3(x, y, z));
		}

		// Token: 0x060012D7 RID: 4823 RVA: 0x00036DF3 File Offset: 0x00034FF3
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float3x3 EulerXZY(float x, float y, float z)
		{
			return float3x3.EulerXZY(math.float3(x, y, z));
		}

		// Token: 0x060012D8 RID: 4824 RVA: 0x00036E02 File Offset: 0x00035002
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float3x3 EulerYXZ(float x, float y, float z)
		{
			return float3x3.EulerYXZ(math.float3(x, y, z));
		}

		// Token: 0x060012D9 RID: 4825 RVA: 0x00036E11 File Offset: 0x00035011
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float3x3 EulerYZX(float x, float y, float z)
		{
			return float3x3.EulerYZX(math.float3(x, y, z));
		}

		// Token: 0x060012DA RID: 4826 RVA: 0x00036E20 File Offset: 0x00035020
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float3x3 EulerZXY(float x, float y, float z)
		{
			return float3x3.EulerZXY(math.float3(x, y, z));
		}

		// Token: 0x060012DB RID: 4827 RVA: 0x00036E2F File Offset: 0x0003502F
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float3x3 EulerZYX(float x, float y, float z)
		{
			return float3x3.EulerZYX(math.float3(x, y, z));
		}

		// Token: 0x060012DC RID: 4828 RVA: 0x00036E40 File Offset: 0x00035040
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float3x3 Euler(float3 xyz, math.RotationOrder order = math.RotationOrder.ZXY)
		{
			switch (order)
			{
			case math.RotationOrder.XYZ:
				return float3x3.EulerXYZ(xyz);
			case math.RotationOrder.XZY:
				return float3x3.EulerXZY(xyz);
			case math.RotationOrder.YXZ:
				return float3x3.EulerYXZ(xyz);
			case math.RotationOrder.YZX:
				return float3x3.EulerYZX(xyz);
			case math.RotationOrder.ZXY:
				return float3x3.EulerZXY(xyz);
			case math.RotationOrder.ZYX:
				return float3x3.EulerZYX(xyz);
			default:
				return float3x3.identity;
			}
		}

		// Token: 0x060012DD RID: 4829 RVA: 0x00036E9C File Offset: 0x0003509C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float3x3 Euler(float x, float y, float z, math.RotationOrder order = math.RotationOrder.ZXY)
		{
			return float3x3.Euler(math.float3(x, y, z), order);
		}

		// Token: 0x060012DE RID: 4830 RVA: 0x00036EAC File Offset: 0x000350AC
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float3x3 RotateX(float angle)
		{
			float num;
			float num2;
			math.sincos(angle, out num, out num2);
			return math.float3x3(1f, 0f, 0f, 0f, num2, -num, 0f, num, num2);
		}

		// Token: 0x060012DF RID: 4831 RVA: 0x00036EE8 File Offset: 0x000350E8
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float3x3 RotateY(float angle)
		{
			float num;
			float num2;
			math.sincos(angle, out num, out num2);
			return math.float3x3(num2, 0f, num, 0f, 1f, 0f, -num, 0f, num2);
		}

		// Token: 0x060012E0 RID: 4832 RVA: 0x00036F24 File Offset: 0x00035124
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float3x3 RotateZ(float angle)
		{
			float num;
			float num2;
			math.sincos(angle, out num, out num2);
			return math.float3x3(num2, -num, 0f, num, num2, 0f, 0f, 0f, 1f);
		}

		// Token: 0x060012E1 RID: 4833 RVA: 0x00036F60 File Offset: 0x00035160
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float3x3 Scale(float s)
		{
			return math.float3x3(s, 0f, 0f, 0f, s, 0f, 0f, 0f, s);
		}

		// Token: 0x060012E2 RID: 4834 RVA: 0x00036F94 File Offset: 0x00035194
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float3x3 Scale(float x, float y, float z)
		{
			return math.float3x3(x, 0f, 0f, 0f, y, 0f, 0f, 0f, z);
		}

		// Token: 0x060012E3 RID: 4835 RVA: 0x00036FC7 File Offset: 0x000351C7
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float3x3 Scale(float3 v)
		{
			return float3x3.Scale(v.x, v.y, v.z);
		}

		// Token: 0x060012E4 RID: 4836 RVA: 0x00036FE0 File Offset: 0x000351E0
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float3x3 LookRotation(float3 forward, float3 up)
		{
			float3 @float = math.normalize(math.cross(up, forward));
			return math.float3x3(@float, math.cross(forward, @float), forward);
		}

		// Token: 0x060012E5 RID: 4837 RVA: 0x00037008 File Offset: 0x00035208
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float3x3 LookRotationSafe(float3 forward, float3 up)
		{
			float num = math.dot(forward, forward);
			float num2 = math.dot(up, up);
			forward *= math.rsqrt(num);
			up *= math.rsqrt(num2);
			float3 @float = math.cross(up, forward);
			float num3 = math.dot(@float, @float);
			@float *= math.rsqrt(num3);
			float num4 = math.min(math.min(num, num2), num3);
			float num5 = math.max(math.max(num, num2), num3);
			bool flag = num4 > 1E-35f && num5 < 1E+35f && math.isfinite(num) && math.isfinite(num2) && math.isfinite(num3);
			return math.float3x3(math.select(math.float3(1f, 0f, 0f), @float, flag), math.select(math.float3(0f, 1f, 0f), math.cross(forward, @float), flag), math.select(math.float3(0f, 0f, 1f), forward, flag));
		}

		// Token: 0x060012E6 RID: 4838 RVA: 0x00037104 File Offset: 0x00035304
		public static explicit operator float3x3(float4x4 f4x4)
		{
			return new float3x3(f4x4);
		}

		// Token: 0x04000093 RID: 147
		public float3 c0;

		// Token: 0x04000094 RID: 148
		public float3 c1;

		// Token: 0x04000095 RID: 149
		public float3 c2;

		// Token: 0x04000096 RID: 150
		public static readonly float3x3 identity = new float3x3(1f, 0f, 0f, 0f, 1f, 0f, 0f, 0f, 1f);

		// Token: 0x04000097 RID: 151
		public static readonly float3x3 zero;
	}
}
