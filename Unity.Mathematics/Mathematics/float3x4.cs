using System;
using System.Runtime.CompilerServices;
using Unity.IL2CPP.CompilerServices;

namespace Unity.Mathematics
{
	// Token: 0x02000025 RID: 37
	[Il2CppEagerStaticClassConstruction]
	[Serializable]
	public struct float3x4 : IEquatable<float3x4>, IFormattable
	{
		// Token: 0x060012E8 RID: 4840 RVA: 0x00037150 File Offset: 0x00035350
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public float3x4(float3 c0, float3 c1, float3 c2, float3 c3)
		{
			this.c0 = c0;
			this.c1 = c1;
			this.c2 = c2;
			this.c3 = c3;
		}

		// Token: 0x060012E9 RID: 4841 RVA: 0x00037170 File Offset: 0x00035370
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public float3x4(float m00, float m01, float m02, float m03, float m10, float m11, float m12, float m13, float m20, float m21, float m22, float m23)
		{
			this.c0 = new float3(m00, m10, m20);
			this.c1 = new float3(m01, m11, m21);
			this.c2 = new float3(m02, m12, m22);
			this.c3 = new float3(m03, m13, m23);
		}

		// Token: 0x060012EA RID: 4842 RVA: 0x000371BE File Offset: 0x000353BE
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public float3x4(float v)
		{
			this.c0 = v;
			this.c1 = v;
			this.c2 = v;
			this.c3 = v;
		}

		// Token: 0x060012EB RID: 4843 RVA: 0x000371F0 File Offset: 0x000353F0
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public float3x4(bool v)
		{
			this.c0 = math.select(new float3(0f), new float3(1f), v);
			this.c1 = math.select(new float3(0f), new float3(1f), v);
			this.c2 = math.select(new float3(0f), new float3(1f), v);
			this.c3 = math.select(new float3(0f), new float3(1f), v);
		}

		// Token: 0x060012EC RID: 4844 RVA: 0x00037280 File Offset: 0x00035480
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public float3x4(bool3x4 v)
		{
			this.c0 = math.select(new float3(0f), new float3(1f), v.c0);
			this.c1 = math.select(new float3(0f), new float3(1f), v.c1);
			this.c2 = math.select(new float3(0f), new float3(1f), v.c2);
			this.c3 = math.select(new float3(0f), new float3(1f), v.c3);
		}

		// Token: 0x060012ED RID: 4845 RVA: 0x00037321 File Offset: 0x00035521
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public float3x4(int v)
		{
			this.c0 = v;
			this.c1 = v;
			this.c2 = v;
			this.c3 = v;
		}

		// Token: 0x060012EE RID: 4846 RVA: 0x00037354 File Offset: 0x00035554
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public float3x4(int3x4 v)
		{
			this.c0 = v.c0;
			this.c1 = v.c1;
			this.c2 = v.c2;
			this.c3 = v.c3;
		}

		// Token: 0x060012EF RID: 4847 RVA: 0x000373A5 File Offset: 0x000355A5
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public float3x4(uint v)
		{
			this.c0 = v;
			this.c1 = v;
			this.c2 = v;
			this.c3 = v;
		}

		// Token: 0x060012F0 RID: 4848 RVA: 0x000373D8 File Offset: 0x000355D8
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public float3x4(uint3x4 v)
		{
			this.c0 = v.c0;
			this.c1 = v.c1;
			this.c2 = v.c2;
			this.c3 = v.c3;
		}

		// Token: 0x060012F1 RID: 4849 RVA: 0x00037429 File Offset: 0x00035629
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public float3x4(double v)
		{
			this.c0 = (float3)v;
			this.c1 = (float3)v;
			this.c2 = (float3)v;
			this.c3 = (float3)v;
		}

		// Token: 0x060012F2 RID: 4850 RVA: 0x0003745C File Offset: 0x0003565C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public float3x4(double3x4 v)
		{
			this.c0 = (float3)v.c0;
			this.c1 = (float3)v.c1;
			this.c2 = (float3)v.c2;
			this.c3 = (float3)v.c3;
		}

		// Token: 0x060012F3 RID: 4851 RVA: 0x000374AD File Offset: 0x000356AD
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static implicit operator float3x4(float v)
		{
			return new float3x4(v);
		}

		// Token: 0x060012F4 RID: 4852 RVA: 0x000374B5 File Offset: 0x000356B5
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static explicit operator float3x4(bool v)
		{
			return new float3x4(v);
		}

		// Token: 0x060012F5 RID: 4853 RVA: 0x000374BD File Offset: 0x000356BD
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static explicit operator float3x4(bool3x4 v)
		{
			return new float3x4(v);
		}

		// Token: 0x060012F6 RID: 4854 RVA: 0x000374C5 File Offset: 0x000356C5
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static implicit operator float3x4(int v)
		{
			return new float3x4(v);
		}

		// Token: 0x060012F7 RID: 4855 RVA: 0x000374CD File Offset: 0x000356CD
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static implicit operator float3x4(int3x4 v)
		{
			return new float3x4(v);
		}

		// Token: 0x060012F8 RID: 4856 RVA: 0x000374D5 File Offset: 0x000356D5
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static implicit operator float3x4(uint v)
		{
			return new float3x4(v);
		}

		// Token: 0x060012F9 RID: 4857 RVA: 0x000374DD File Offset: 0x000356DD
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static implicit operator float3x4(uint3x4 v)
		{
			return new float3x4(v);
		}

		// Token: 0x060012FA RID: 4858 RVA: 0x000374E5 File Offset: 0x000356E5
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static explicit operator float3x4(double v)
		{
			return new float3x4(v);
		}

		// Token: 0x060012FB RID: 4859 RVA: 0x000374ED File Offset: 0x000356ED
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static explicit operator float3x4(double3x4 v)
		{
			return new float3x4(v);
		}

		// Token: 0x060012FC RID: 4860 RVA: 0x000374F8 File Offset: 0x000356F8
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float3x4 operator *(float3x4 lhs, float3x4 rhs)
		{
			return new float3x4(lhs.c0 * rhs.c0, lhs.c1 * rhs.c1, lhs.c2 * rhs.c2, lhs.c3 * rhs.c3);
		}

		// Token: 0x060012FD RID: 4861 RVA: 0x0003754E File Offset: 0x0003574E
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float3x4 operator *(float3x4 lhs, float rhs)
		{
			return new float3x4(lhs.c0 * rhs, lhs.c1 * rhs, lhs.c2 * rhs, lhs.c3 * rhs);
		}

		// Token: 0x060012FE RID: 4862 RVA: 0x00037585 File Offset: 0x00035785
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float3x4 operator *(float lhs, float3x4 rhs)
		{
			return new float3x4(lhs * rhs.c0, lhs * rhs.c1, lhs * rhs.c2, lhs * rhs.c3);
		}

		// Token: 0x060012FF RID: 4863 RVA: 0x000375BC File Offset: 0x000357BC
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float3x4 operator +(float3x4 lhs, float3x4 rhs)
		{
			return new float3x4(lhs.c0 + rhs.c0, lhs.c1 + rhs.c1, lhs.c2 + rhs.c2, lhs.c3 + rhs.c3);
		}

		// Token: 0x06001300 RID: 4864 RVA: 0x00037612 File Offset: 0x00035812
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float3x4 operator +(float3x4 lhs, float rhs)
		{
			return new float3x4(lhs.c0 + rhs, lhs.c1 + rhs, lhs.c2 + rhs, lhs.c3 + rhs);
		}

		// Token: 0x06001301 RID: 4865 RVA: 0x00037649 File Offset: 0x00035849
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float3x4 operator +(float lhs, float3x4 rhs)
		{
			return new float3x4(lhs + rhs.c0, lhs + rhs.c1, lhs + rhs.c2, lhs + rhs.c3);
		}

		// Token: 0x06001302 RID: 4866 RVA: 0x00037680 File Offset: 0x00035880
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float3x4 operator -(float3x4 lhs, float3x4 rhs)
		{
			return new float3x4(lhs.c0 - rhs.c0, lhs.c1 - rhs.c1, lhs.c2 - rhs.c2, lhs.c3 - rhs.c3);
		}

		// Token: 0x06001303 RID: 4867 RVA: 0x000376D6 File Offset: 0x000358D6
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float3x4 operator -(float3x4 lhs, float rhs)
		{
			return new float3x4(lhs.c0 - rhs, lhs.c1 - rhs, lhs.c2 - rhs, lhs.c3 - rhs);
		}

		// Token: 0x06001304 RID: 4868 RVA: 0x0003770D File Offset: 0x0003590D
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float3x4 operator -(float lhs, float3x4 rhs)
		{
			return new float3x4(lhs - rhs.c0, lhs - rhs.c1, lhs - rhs.c2, lhs - rhs.c3);
		}

		// Token: 0x06001305 RID: 4869 RVA: 0x00037744 File Offset: 0x00035944
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float3x4 operator /(float3x4 lhs, float3x4 rhs)
		{
			return new float3x4(lhs.c0 / rhs.c0, lhs.c1 / rhs.c1, lhs.c2 / rhs.c2, lhs.c3 / rhs.c3);
		}

		// Token: 0x06001306 RID: 4870 RVA: 0x0003779A File Offset: 0x0003599A
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float3x4 operator /(float3x4 lhs, float rhs)
		{
			return new float3x4(lhs.c0 / rhs, lhs.c1 / rhs, lhs.c2 / rhs, lhs.c3 / rhs);
		}

		// Token: 0x06001307 RID: 4871 RVA: 0x000377D1 File Offset: 0x000359D1
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float3x4 operator /(float lhs, float3x4 rhs)
		{
			return new float3x4(lhs / rhs.c0, lhs / rhs.c1, lhs / rhs.c2, lhs / rhs.c3);
		}

		// Token: 0x06001308 RID: 4872 RVA: 0x00037808 File Offset: 0x00035A08
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float3x4 operator %(float3x4 lhs, float3x4 rhs)
		{
			return new float3x4(lhs.c0 % rhs.c0, lhs.c1 % rhs.c1, lhs.c2 % rhs.c2, lhs.c3 % rhs.c3);
		}

		// Token: 0x06001309 RID: 4873 RVA: 0x0003785E File Offset: 0x00035A5E
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float3x4 operator %(float3x4 lhs, float rhs)
		{
			return new float3x4(lhs.c0 % rhs, lhs.c1 % rhs, lhs.c2 % rhs, lhs.c3 % rhs);
		}

		// Token: 0x0600130A RID: 4874 RVA: 0x00037895 File Offset: 0x00035A95
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float3x4 operator %(float lhs, float3x4 rhs)
		{
			return new float3x4(lhs % rhs.c0, lhs % rhs.c1, lhs % rhs.c2, lhs % rhs.c3);
		}

		// Token: 0x0600130B RID: 4875 RVA: 0x000378CC File Offset: 0x00035ACC
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float3x4 operator ++(float3x4 val)
		{
			float3 @float = float3.op_Increment(val.c0);
			val.c0 = @float;
			float3 float2 = @float;
			@float = float3.op_Increment(val.c1);
			val.c1 = @float;
			float3 float3 = @float;
			@float = float3.op_Increment(val.c2);
			val.c2 = @float;
			float3 float4 = @float;
			@float = float3.op_Increment(val.c3);
			val.c3 = @float;
			return new float3x4(float2, float3, float4, @float);
		}

		// Token: 0x0600130C RID: 4876 RVA: 0x00037948 File Offset: 0x00035B48
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float3x4 operator --(float3x4 val)
		{
			float3 @float = float3.op_Decrement(val.c0);
			val.c0 = @float;
			float3 float2 = @float;
			@float = float3.op_Decrement(val.c1);
			val.c1 = @float;
			float3 float3 = @float;
			@float = float3.op_Decrement(val.c2);
			val.c2 = @float;
			float3 float4 = @float;
			@float = float3.op_Decrement(val.c3);
			val.c3 = @float;
			return new float3x4(float2, float3, float4, @float);
		}

		// Token: 0x0600130D RID: 4877 RVA: 0x000379C4 File Offset: 0x00035BC4
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool3x4 operator <(float3x4 lhs, float3x4 rhs)
		{
			return new bool3x4(lhs.c0 < rhs.c0, lhs.c1 < rhs.c1, lhs.c2 < rhs.c2, lhs.c3 < rhs.c3);
		}

		// Token: 0x0600130E RID: 4878 RVA: 0x00037A1A File Offset: 0x00035C1A
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool3x4 operator <(float3x4 lhs, float rhs)
		{
			return new bool3x4(lhs.c0 < rhs, lhs.c1 < rhs, lhs.c2 < rhs, lhs.c3 < rhs);
		}

		// Token: 0x0600130F RID: 4879 RVA: 0x00037A51 File Offset: 0x00035C51
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool3x4 operator <(float lhs, float3x4 rhs)
		{
			return new bool3x4(lhs < rhs.c0, lhs < rhs.c1, lhs < rhs.c2, lhs < rhs.c3);
		}

		// Token: 0x06001310 RID: 4880 RVA: 0x00037A88 File Offset: 0x00035C88
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool3x4 operator <=(float3x4 lhs, float3x4 rhs)
		{
			return new bool3x4(lhs.c0 <= rhs.c0, lhs.c1 <= rhs.c1, lhs.c2 <= rhs.c2, lhs.c3 <= rhs.c3);
		}

		// Token: 0x06001311 RID: 4881 RVA: 0x00037ADE File Offset: 0x00035CDE
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool3x4 operator <=(float3x4 lhs, float rhs)
		{
			return new bool3x4(lhs.c0 <= rhs, lhs.c1 <= rhs, lhs.c2 <= rhs, lhs.c3 <= rhs);
		}

		// Token: 0x06001312 RID: 4882 RVA: 0x00037B15 File Offset: 0x00035D15
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool3x4 operator <=(float lhs, float3x4 rhs)
		{
			return new bool3x4(lhs <= rhs.c0, lhs <= rhs.c1, lhs <= rhs.c2, lhs <= rhs.c3);
		}

		// Token: 0x06001313 RID: 4883 RVA: 0x00037B4C File Offset: 0x00035D4C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool3x4 operator >(float3x4 lhs, float3x4 rhs)
		{
			return new bool3x4(lhs.c0 > rhs.c0, lhs.c1 > rhs.c1, lhs.c2 > rhs.c2, lhs.c3 > rhs.c3);
		}

		// Token: 0x06001314 RID: 4884 RVA: 0x00037BA2 File Offset: 0x00035DA2
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool3x4 operator >(float3x4 lhs, float rhs)
		{
			return new bool3x4(lhs.c0 > rhs, lhs.c1 > rhs, lhs.c2 > rhs, lhs.c3 > rhs);
		}

		// Token: 0x06001315 RID: 4885 RVA: 0x00037BD9 File Offset: 0x00035DD9
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool3x4 operator >(float lhs, float3x4 rhs)
		{
			return new bool3x4(lhs > rhs.c0, lhs > rhs.c1, lhs > rhs.c2, lhs > rhs.c3);
		}

		// Token: 0x06001316 RID: 4886 RVA: 0x00037C10 File Offset: 0x00035E10
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool3x4 operator >=(float3x4 lhs, float3x4 rhs)
		{
			return new bool3x4(lhs.c0 >= rhs.c0, lhs.c1 >= rhs.c1, lhs.c2 >= rhs.c2, lhs.c3 >= rhs.c3);
		}

		// Token: 0x06001317 RID: 4887 RVA: 0x00037C66 File Offset: 0x00035E66
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool3x4 operator >=(float3x4 lhs, float rhs)
		{
			return new bool3x4(lhs.c0 >= rhs, lhs.c1 >= rhs, lhs.c2 >= rhs, lhs.c3 >= rhs);
		}

		// Token: 0x06001318 RID: 4888 RVA: 0x00037C9D File Offset: 0x00035E9D
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool3x4 operator >=(float lhs, float3x4 rhs)
		{
			return new bool3x4(lhs >= rhs.c0, lhs >= rhs.c1, lhs >= rhs.c2, lhs >= rhs.c3);
		}

		// Token: 0x06001319 RID: 4889 RVA: 0x00037CD4 File Offset: 0x00035ED4
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float3x4 operator -(float3x4 val)
		{
			return new float3x4(-val.c0, -val.c1, -val.c2, -val.c3);
		}

		// Token: 0x0600131A RID: 4890 RVA: 0x00037D07 File Offset: 0x00035F07
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float3x4 operator +(float3x4 val)
		{
			return new float3x4(+val.c0, +val.c1, +val.c2, +val.c3);
		}

		// Token: 0x0600131B RID: 4891 RVA: 0x00037D3C File Offset: 0x00035F3C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool3x4 operator ==(float3x4 lhs, float3x4 rhs)
		{
			return new bool3x4(lhs.c0 == rhs.c0, lhs.c1 == rhs.c1, lhs.c2 == rhs.c2, lhs.c3 == rhs.c3);
		}

		// Token: 0x0600131C RID: 4892 RVA: 0x00037D92 File Offset: 0x00035F92
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool3x4 operator ==(float3x4 lhs, float rhs)
		{
			return new bool3x4(lhs.c0 == rhs, lhs.c1 == rhs, lhs.c2 == rhs, lhs.c3 == rhs);
		}

		// Token: 0x0600131D RID: 4893 RVA: 0x00037DC9 File Offset: 0x00035FC9
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool3x4 operator ==(float lhs, float3x4 rhs)
		{
			return new bool3x4(lhs == rhs.c0, lhs == rhs.c1, lhs == rhs.c2, lhs == rhs.c3);
		}

		// Token: 0x0600131E RID: 4894 RVA: 0x00037E00 File Offset: 0x00036000
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool3x4 operator !=(float3x4 lhs, float3x4 rhs)
		{
			return new bool3x4(lhs.c0 != rhs.c0, lhs.c1 != rhs.c1, lhs.c2 != rhs.c2, lhs.c3 != rhs.c3);
		}

		// Token: 0x0600131F RID: 4895 RVA: 0x00037E56 File Offset: 0x00036056
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool3x4 operator !=(float3x4 lhs, float rhs)
		{
			return new bool3x4(lhs.c0 != rhs, lhs.c1 != rhs, lhs.c2 != rhs, lhs.c3 != rhs);
		}

		// Token: 0x06001320 RID: 4896 RVA: 0x00037E8D File Offset: 0x0003608D
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool3x4 operator !=(float lhs, float3x4 rhs)
		{
			return new bool3x4(lhs != rhs.c0, lhs != rhs.c1, lhs != rhs.c2, lhs != rhs.c3);
		}

		// Token: 0x17000473 RID: 1139
		public unsafe ref float3 this[int index]
		{
			get
			{
				fixed (float3x4* ptr = &this)
				{
					return ref *(float3*)(ptr + (IntPtr)index * (IntPtr)sizeof(float3) / (IntPtr)sizeof(float3x4));
				}
			}
		}

		// Token: 0x06001322 RID: 4898 RVA: 0x00037EE0 File Offset: 0x000360E0
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool Equals(float3x4 rhs)
		{
			return this.c0.Equals(rhs.c0) && this.c1.Equals(rhs.c1) && this.c2.Equals(rhs.c2) && this.c3.Equals(rhs.c3);
		}

		// Token: 0x06001323 RID: 4899 RVA: 0x00037F3C File Offset: 0x0003613C
		public override bool Equals(object o)
		{
			if (o is float3x4)
			{
				float3x4 float3x = (float3x4)o;
				return this.Equals(float3x);
			}
			return false;
		}

		// Token: 0x06001324 RID: 4900 RVA: 0x00037F61 File Offset: 0x00036161
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public override int GetHashCode()
		{
			return (int)math.hash(this);
		}

		// Token: 0x06001325 RID: 4901 RVA: 0x00037F70 File Offset: 0x00036170
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public override string ToString()
		{
			return string.Format("float3x4({0}f, {1}f, {2}f, {3}f,  {4}f, {5}f, {6}f, {7}f,  {8}f, {9}f, {10}f, {11}f)", new object[]
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
				this.c3.z
			});
		}

		// Token: 0x06001326 RID: 4902 RVA: 0x00038078 File Offset: 0x00036278
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public string ToString(string format, IFormatProvider formatProvider)
		{
			return string.Format("float3x4({0}f, {1}f, {2}f, {3}f,  {4}f, {5}f, {6}f, {7}f,  {8}f, {9}f, {10}f, {11}f)", new object[]
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
				this.c3.z.ToString(format, formatProvider)
			});
		}

		// Token: 0x04000098 RID: 152
		public float3 c0;

		// Token: 0x04000099 RID: 153
		public float3 c1;

		// Token: 0x0400009A RID: 154
		public float3 c2;

		// Token: 0x0400009B RID: 155
		public float3 c3;

		// Token: 0x0400009C RID: 156
		public static readonly float3x4 zero;
	}
}
