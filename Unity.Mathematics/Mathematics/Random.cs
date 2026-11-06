using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Unity.IL2CPP.CompilerServices;

namespace Unity.Mathematics
{
	// Token: 0x0200003E RID: 62
	[Il2CppEagerStaticClassConstruction]
	[Serializable]
	public struct Random
	{
		// Token: 0x06001E75 RID: 7797 RVA: 0x00058FB5 File Offset: 0x000571B5
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Random(uint seed)
		{
			this.state = seed;
			this.NextState();
		}

		// Token: 0x06001E76 RID: 7798 RVA: 0x00058FC5 File Offset: 0x000571C5
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Random CreateFromIndex(uint index)
		{
			return new Random(Random.WangHash(index + 62U));
		}

		// Token: 0x06001E77 RID: 7799 RVA: 0x00058FD5 File Offset: 0x000571D5
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal static uint WangHash(uint n)
		{
			n = n ^ 61U ^ (n >> 16);
			n *= 9U;
			n ^= n >> 4;
			n *= 668265261U;
			n ^= n >> 15;
			return n;
		}

		// Token: 0x06001E78 RID: 7800 RVA: 0x00059001 File Offset: 0x00057201
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void InitState(uint seed = 1851936439U)
		{
			this.state = seed;
			this.NextState();
		}

		// Token: 0x06001E79 RID: 7801 RVA: 0x00059011 File Offset: 0x00057211
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool NextBool()
		{
			return (this.NextState() & 1U) == 1U;
		}

		// Token: 0x06001E7A RID: 7802 RVA: 0x0005901E File Offset: 0x0005721E
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool2 NextBool2()
		{
			return (math.uint2(this.NextState()) & math.uint2(1U, 2U)) == 0U;
		}

		// Token: 0x06001E7B RID: 7803 RVA: 0x0005903D File Offset: 0x0005723D
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool3 NextBool3()
		{
			return (math.uint3(this.NextState()) & math.uint3(1U, 2U, 4U)) == 0U;
		}

		// Token: 0x06001E7C RID: 7804 RVA: 0x0005905D File Offset: 0x0005725D
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool4 NextBool4()
		{
			return (math.uint4(this.NextState()) & math.uint4(1U, 2U, 4U, 8U)) == 0U;
		}

		// Token: 0x06001E7D RID: 7805 RVA: 0x0005907E File Offset: 0x0005727E
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int NextInt()
		{
			return (int)(this.NextState() ^ 2147483648U);
		}

		// Token: 0x06001E7E RID: 7806 RVA: 0x0005908C File Offset: 0x0005728C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int2 NextInt2()
		{
			return math.int2((int)this.NextState(), (int)this.NextState()) ^ int.MinValue;
		}

		// Token: 0x06001E7F RID: 7807 RVA: 0x000590A9 File Offset: 0x000572A9
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int3 NextInt3()
		{
			return math.int3((int)this.NextState(), (int)this.NextState(), (int)this.NextState()) ^ int.MinValue;
		}

		// Token: 0x06001E80 RID: 7808 RVA: 0x000590CC File Offset: 0x000572CC
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int4 NextInt4()
		{
			return math.int4((int)this.NextState(), (int)this.NextState(), (int)this.NextState(), (int)this.NextState()) ^ int.MinValue;
		}

		// Token: 0x06001E81 RID: 7809 RVA: 0x000590F5 File Offset: 0x000572F5
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int NextInt(int max)
		{
			return (int)((ulong)this.NextState() * (ulong)((long)max) >> 32);
		}

		// Token: 0x06001E82 RID: 7810 RVA: 0x00059105 File Offset: 0x00057305
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int2 NextInt2(int2 max)
		{
			return math.int2((int)((ulong)this.NextState() * (ulong)((long)max.x) >> 32), (int)((ulong)this.NextState() * (ulong)((long)max.y) >> 32));
		}

		// Token: 0x06001E83 RID: 7811 RVA: 0x00059132 File Offset: 0x00057332
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int3 NextInt3(int3 max)
		{
			return math.int3((int)((ulong)this.NextState() * (ulong)((long)max.x) >> 32), (int)((ulong)this.NextState() * (ulong)((long)max.y) >> 32), (int)((ulong)this.NextState() * (ulong)((long)max.z) >> 32));
		}

		// Token: 0x06001E84 RID: 7812 RVA: 0x00059174 File Offset: 0x00057374
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int4 NextInt4(int4 max)
		{
			return math.int4((int)((ulong)this.NextState() * (ulong)((long)max.x) >> 32), (int)((ulong)this.NextState() * (ulong)((long)max.y) >> 32), (int)((ulong)this.NextState() * (ulong)((long)max.z) >> 32), (int)((ulong)this.NextState() * (ulong)((long)max.w) >> 32));
		}

		// Token: 0x06001E85 RID: 7813 RVA: 0x000591D4 File Offset: 0x000573D4
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int NextInt(int min, int max)
		{
			uint num = (uint)(max - min);
			return (int)((ulong)this.NextState() * (ulong)num >> 32) + min;
		}

		// Token: 0x06001E86 RID: 7814 RVA: 0x000591F8 File Offset: 0x000573F8
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int2 NextInt2(int2 min, int2 max)
		{
			uint2 @uint = (uint2)(max - min);
			return math.int2((int)((ulong)this.NextState() * (ulong)@uint.x >> 32), (int)((ulong)this.NextState() * (ulong)@uint.y >> 32)) + min;
		}

		// Token: 0x06001E87 RID: 7815 RVA: 0x00059244 File Offset: 0x00057444
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int3 NextInt3(int3 min, int3 max)
		{
			uint3 @uint = (uint3)(max - min);
			return math.int3((int)((ulong)this.NextState() * (ulong)@uint.x >> 32), (int)((ulong)this.NextState() * (ulong)@uint.y >> 32), (int)((ulong)this.NextState() * (ulong)@uint.z >> 32)) + min;
		}

		// Token: 0x06001E88 RID: 7816 RVA: 0x000592A4 File Offset: 0x000574A4
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int4 NextInt4(int4 min, int4 max)
		{
			uint4 @uint = (uint4)(max - min);
			return math.int4((int)((ulong)this.NextState() * (ulong)@uint.x >> 32), (int)((ulong)this.NextState() * (ulong)@uint.y >> 32), (int)((ulong)this.NextState() * (ulong)@uint.z >> 32), (int)((ulong)this.NextState() * (ulong)@uint.w >> 32)) + min;
		}

		// Token: 0x06001E89 RID: 7817 RVA: 0x00059315 File Offset: 0x00057515
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public uint NextUInt()
		{
			return this.NextState() - 1U;
		}

		// Token: 0x06001E8A RID: 7818 RVA: 0x0005931F File Offset: 0x0005751F
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public uint2 NextUInt2()
		{
			return math.uint2(this.NextState(), this.NextState()) - 1U;
		}

		// Token: 0x06001E8B RID: 7819 RVA: 0x00059338 File Offset: 0x00057538
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public uint3 NextUInt3()
		{
			return math.uint3(this.NextState(), this.NextState(), this.NextState()) - 1U;
		}

		// Token: 0x06001E8C RID: 7820 RVA: 0x00059357 File Offset: 0x00057557
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public uint4 NextUInt4()
		{
			return math.uint4(this.NextState(), this.NextState(), this.NextState(), this.NextState()) - 1U;
		}

		// Token: 0x06001E8D RID: 7821 RVA: 0x0005937C File Offset: 0x0005757C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public uint NextUInt(uint max)
		{
			return (uint)((ulong)this.NextState() * (ulong)max >> 32);
		}

		// Token: 0x06001E8E RID: 7822 RVA: 0x0005938C File Offset: 0x0005758C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public uint2 NextUInt2(uint2 max)
		{
			return math.uint2((uint)((ulong)this.NextState() * (ulong)max.x >> 32), (uint)((ulong)this.NextState() * (ulong)max.y >> 32));
		}

		// Token: 0x06001E8F RID: 7823 RVA: 0x000593B9 File Offset: 0x000575B9
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public uint3 NextUInt3(uint3 max)
		{
			return math.uint3((uint)((ulong)this.NextState() * (ulong)max.x >> 32), (uint)((ulong)this.NextState() * (ulong)max.y >> 32), (uint)((ulong)this.NextState() * (ulong)max.z >> 32));
		}

		// Token: 0x06001E90 RID: 7824 RVA: 0x000593FC File Offset: 0x000575FC
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public uint4 NextUInt4(uint4 max)
		{
			return math.uint4((uint)((ulong)this.NextState() * (ulong)max.x >> 32), (uint)((ulong)this.NextState() * (ulong)max.y >> 32), (uint)((ulong)this.NextState() * (ulong)max.z >> 32), (uint)((ulong)this.NextState() * (ulong)max.w >> 32));
		}

		// Token: 0x06001E91 RID: 7825 RVA: 0x0005945C File Offset: 0x0005765C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public uint NextUInt(uint min, uint max)
		{
			uint num = max - min;
			return (uint)((ulong)this.NextState() * (ulong)num >> 32) + min;
		}

		// Token: 0x06001E92 RID: 7826 RVA: 0x00059480 File Offset: 0x00057680
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public uint2 NextUInt2(uint2 min, uint2 max)
		{
			uint2 @uint = max - min;
			return math.uint2((uint)((ulong)this.NextState() * (ulong)@uint.x >> 32), (uint)((ulong)this.NextState() * (ulong)@uint.y >> 32)) + min;
		}

		// Token: 0x06001E93 RID: 7827 RVA: 0x000594C8 File Offset: 0x000576C8
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public uint3 NextUInt3(uint3 min, uint3 max)
		{
			uint3 @uint = max - min;
			return math.uint3((uint)((ulong)this.NextState() * (ulong)@uint.x >> 32), (uint)((ulong)this.NextState() * (ulong)@uint.y >> 32), (uint)((ulong)this.NextState() * (ulong)@uint.z >> 32)) + min;
		}

		// Token: 0x06001E94 RID: 7828 RVA: 0x00059524 File Offset: 0x00057724
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public uint4 NextUInt4(uint4 min, uint4 max)
		{
			uint4 @uint = max - min;
			return math.uint4((uint)((ulong)this.NextState() * (ulong)@uint.x >> 32), (uint)((ulong)this.NextState() * (ulong)@uint.y >> 32), (uint)((ulong)this.NextState() * (ulong)@uint.z >> 32), (uint)((ulong)this.NextState() * (ulong)@uint.w >> 32)) + min;
		}

		// Token: 0x06001E95 RID: 7829 RVA: 0x00059590 File Offset: 0x00057790
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public float NextFloat()
		{
			return math.asfloat(1065353216U | (this.NextState() >> 9)) - 1f;
		}

		// Token: 0x06001E96 RID: 7830 RVA: 0x000595AC File Offset: 0x000577AC
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public float2 NextFloat2()
		{
			return math.asfloat(1065353216U | (math.uint2(this.NextState(), this.NextState()) >> 9)) - 1f;
		}

		// Token: 0x06001E97 RID: 7831 RVA: 0x000595DF File Offset: 0x000577DF
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public float3 NextFloat3()
		{
			return math.asfloat(1065353216U | (math.uint3(this.NextState(), this.NextState(), this.NextState()) >> 9)) - 1f;
		}

		// Token: 0x06001E98 RID: 7832 RVA: 0x00059618 File Offset: 0x00057818
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public float4 NextFloat4()
		{
			return math.asfloat(1065353216U | (math.uint4(this.NextState(), this.NextState(), this.NextState(), this.NextState()) >> 9)) - 1f;
		}

		// Token: 0x06001E99 RID: 7833 RVA: 0x00059657 File Offset: 0x00057857
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public float NextFloat(float max)
		{
			return this.NextFloat() * max;
		}

		// Token: 0x06001E9A RID: 7834 RVA: 0x00059661 File Offset: 0x00057861
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public float2 NextFloat2(float2 max)
		{
			return this.NextFloat2() * max;
		}

		// Token: 0x06001E9B RID: 7835 RVA: 0x0005966F File Offset: 0x0005786F
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public float3 NextFloat3(float3 max)
		{
			return this.NextFloat3() * max;
		}

		// Token: 0x06001E9C RID: 7836 RVA: 0x0005967D File Offset: 0x0005787D
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public float4 NextFloat4(float4 max)
		{
			return this.NextFloat4() * max;
		}

		// Token: 0x06001E9D RID: 7837 RVA: 0x0005968B File Offset: 0x0005788B
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public float NextFloat(float min, float max)
		{
			return this.NextFloat() * (max - min) + min;
		}

		// Token: 0x06001E9E RID: 7838 RVA: 0x00059699 File Offset: 0x00057899
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public float2 NextFloat2(float2 min, float2 max)
		{
			return this.NextFloat2() * (max - min) + min;
		}

		// Token: 0x06001E9F RID: 7839 RVA: 0x000596B3 File Offset: 0x000578B3
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public float3 NextFloat3(float3 min, float3 max)
		{
			return this.NextFloat3() * (max - min) + min;
		}

		// Token: 0x06001EA0 RID: 7840 RVA: 0x000596CD File Offset: 0x000578CD
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public float4 NextFloat4(float4 min, float4 max)
		{
			return this.NextFloat4() * (max - min) + min;
		}

		// Token: 0x06001EA1 RID: 7841 RVA: 0x000596E8 File Offset: 0x000578E8
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public double NextDouble()
		{
			ulong num = ((ulong)this.NextState() << 20) ^ (ulong)this.NextState();
			return math.asdouble(4607182418800017408UL | num) - 1.0;
		}

		// Token: 0x06001EA2 RID: 7842 RVA: 0x00059724 File Offset: 0x00057924
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public double2 NextDouble2()
		{
			ulong num = ((ulong)this.NextState() << 20) ^ (ulong)this.NextState();
			ulong num2 = ((ulong)this.NextState() << 20) ^ (ulong)this.NextState();
			return math.double2(math.asdouble(4607182418800017408UL | num), math.asdouble(4607182418800017408UL | num2)) - 1.0;
		}

		// Token: 0x06001EA3 RID: 7843 RVA: 0x0005978C File Offset: 0x0005798C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public double3 NextDouble3()
		{
			ulong num = ((ulong)this.NextState() << 20) ^ (ulong)this.NextState();
			ulong num2 = ((ulong)this.NextState() << 20) ^ (ulong)this.NextState();
			ulong num3 = ((ulong)this.NextState() << 20) ^ (ulong)this.NextState();
			return math.double3(math.asdouble(4607182418800017408UL | num), math.asdouble(4607182418800017408UL | num2), math.asdouble(4607182418800017408UL | num3)) - 1.0;
		}

		// Token: 0x06001EA4 RID: 7844 RVA: 0x00059818 File Offset: 0x00057A18
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public double4 NextDouble4()
		{
			ulong num = ((ulong)this.NextState() << 20) ^ (ulong)this.NextState();
			ulong num2 = ((ulong)this.NextState() << 20) ^ (ulong)this.NextState();
			ulong num3 = ((ulong)this.NextState() << 20) ^ (ulong)this.NextState();
			ulong num4 = ((ulong)this.NextState() << 20) ^ (ulong)this.NextState();
			return math.double4(math.asdouble(4607182418800017408UL | num), math.asdouble(4607182418800017408UL | num2), math.asdouble(4607182418800017408UL | num3), math.asdouble(4607182418800017408UL | num4)) - 1.0;
		}

		// Token: 0x06001EA5 RID: 7845 RVA: 0x000598C4 File Offset: 0x00057AC4
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public double NextDouble(double max)
		{
			return this.NextDouble() * max;
		}

		// Token: 0x06001EA6 RID: 7846 RVA: 0x000598CE File Offset: 0x00057ACE
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public double2 NextDouble2(double2 max)
		{
			return this.NextDouble2() * max;
		}

		// Token: 0x06001EA7 RID: 7847 RVA: 0x000598DC File Offset: 0x00057ADC
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public double3 NextDouble3(double3 max)
		{
			return this.NextDouble3() * max;
		}

		// Token: 0x06001EA8 RID: 7848 RVA: 0x000598EA File Offset: 0x00057AEA
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public double4 NextDouble4(double4 max)
		{
			return this.NextDouble4() * max;
		}

		// Token: 0x06001EA9 RID: 7849 RVA: 0x000598F8 File Offset: 0x00057AF8
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public double NextDouble(double min, double max)
		{
			return this.NextDouble() * (max - min) + min;
		}

		// Token: 0x06001EAA RID: 7850 RVA: 0x00059906 File Offset: 0x00057B06
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public double2 NextDouble2(double2 min, double2 max)
		{
			return this.NextDouble2() * (max - min) + min;
		}

		// Token: 0x06001EAB RID: 7851 RVA: 0x00059920 File Offset: 0x00057B20
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public double3 NextDouble3(double3 min, double3 max)
		{
			return this.NextDouble3() * (max - min) + min;
		}

		// Token: 0x06001EAC RID: 7852 RVA: 0x0005993A File Offset: 0x00057B3A
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public double4 NextDouble4(double4 min, double4 max)
		{
			return this.NextDouble4() * (max - min) + min;
		}

		// Token: 0x06001EAD RID: 7853 RVA: 0x00059954 File Offset: 0x00057B54
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public float2 NextFloat2Direction()
		{
			float num;
			float num2;
			math.sincos(this.NextFloat() * 3.1415927f * 2f, out num, out num2);
			return math.float2(num2, num);
		}

		// Token: 0x06001EAE RID: 7854 RVA: 0x00059984 File Offset: 0x00057B84
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public double2 NextDouble2Direction()
		{
			double num;
			double num2;
			math.sincos(this.NextDouble() * 3.141592653589793 * 2.0, out num, out num2);
			return math.double2(num2, num);
		}

		// Token: 0x06001EAF RID: 7855 RVA: 0x000599BC File Offset: 0x00057BBC
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public float3 NextFloat3Direction()
		{
			float2 @float = this.NextFloat2();
			float num = @float.x * 2f - 1f;
			float num2 = math.sqrt(math.max(1f - num * num, 0f));
			float num3;
			float num4;
			math.sincos(@float.y * 3.1415927f * 2f, out num3, out num4);
			return math.float3(num4 * num2, num3 * num2, num);
		}

		// Token: 0x06001EB0 RID: 7856 RVA: 0x00059A24 File Offset: 0x00057C24
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public double3 NextDouble3Direction()
		{
			double2 @double = this.NextDouble2();
			double num = @double.x * 2.0 - 1.0;
			double num2 = math.sqrt(math.max(1.0 - num * num, 0.0));
			double num3;
			double num4;
			math.sincos(@double.y * 3.141592653589793 * 2.0, out num3, out num4);
			return math.double3(num4 * num2, num3 * num2, num);
		}

		// Token: 0x06001EB1 RID: 7857 RVA: 0x00059AA4 File Offset: 0x00057CA4
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public quaternion NextQuaternionRotation()
		{
			float3 @float = this.NextFloat3(math.float3(6.2831855f, 6.2831855f, 1f));
			float z = @float.z;
			float2 xy = @float.xy;
			float num = math.sqrt(1f - z);
			float num2 = math.sqrt(z);
			float2 float2;
			float2 float3;
			math.sincos(xy, out float2, out float3);
			quaternion quaternion = math.quaternion(num * float2.x, num * float3.x, num2 * float2.y, num2 * float3.y);
			return math.quaternion(math.select(quaternion.value, -quaternion.value, quaternion.value.w < 0f));
		}

		// Token: 0x06001EB2 RID: 7858 RVA: 0x00059B54 File Offset: 0x00057D54
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private uint NextState()
		{
			uint num = this.state;
			this.state ^= this.state << 13;
			this.state ^= this.state >> 17;
			this.state ^= this.state << 5;
			return num;
		}

		// Token: 0x06001EB3 RID: 7859 RVA: 0x00059BA8 File Offset: 0x00057DA8
		[Conditional("ENABLE_UNITY_COLLECTIONS_CHECKS")]
		private void CheckInitState()
		{
		}

		// Token: 0x06001EB4 RID: 7860 RVA: 0x00059BAA File Offset: 0x00057DAA
		[Conditional("ENABLE_UNITY_COLLECTIONS_CHECKS")]
		private static void CheckIndexForHash(uint index)
		{
			if (index == 4294967295U)
			{
				throw new ArgumentException("Index must not be uint.MaxValue");
			}
		}

		// Token: 0x06001EB5 RID: 7861 RVA: 0x00059BBB File Offset: 0x00057DBB
		[Conditional("ENABLE_UNITY_COLLECTIONS_CHECKS")]
		private void CheckState()
		{
		}

		// Token: 0x06001EB6 RID: 7862 RVA: 0x00059BBD File Offset: 0x00057DBD
		[Conditional("ENABLE_UNITY_COLLECTIONS_CHECKS")]
		private void CheckNextIntMax(int max)
		{
		}

		// Token: 0x06001EB7 RID: 7863 RVA: 0x00059BBF File Offset: 0x00057DBF
		[Conditional("ENABLE_UNITY_COLLECTIONS_CHECKS")]
		private void CheckNextIntMinMax(int min, int max)
		{
		}

		// Token: 0x06001EB8 RID: 7864 RVA: 0x00059BC1 File Offset: 0x00057DC1
		[Conditional("ENABLE_UNITY_COLLECTIONS_CHECKS")]
		private void CheckNextUIntMinMax(uint min, uint max)
		{
		}

		// Token: 0x040000F2 RID: 242
		public uint state;
	}
}
