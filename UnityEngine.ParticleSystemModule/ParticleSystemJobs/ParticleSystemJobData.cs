using System;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;

namespace UnityEngine.ParticleSystemJobs
{
	// Token: 0x02000069 RID: 105
	public struct ParticleSystemJobData
	{
		// Token: 0x170001F0 RID: 496
		// (get) Token: 0x06000761 RID: 1889 RVA: 0x00006A56 File Offset: 0x00004C56
		public readonly int count { get; }

		// Token: 0x170001F1 RID: 497
		// (get) Token: 0x06000762 RID: 1890 RVA: 0x00006A5E File Offset: 0x00004C5E
		public readonly ParticleSystemNativeArray3 positions { get; }

		// Token: 0x170001F2 RID: 498
		// (get) Token: 0x06000763 RID: 1891 RVA: 0x00006A66 File Offset: 0x00004C66
		public readonly ParticleSystemNativeArray3 velocities { get; }

		// Token: 0x170001F3 RID: 499
		// (get) Token: 0x06000764 RID: 1892 RVA: 0x00006A6E File Offset: 0x00004C6E
		public readonly ParticleSystemNativeArray3 axisOfRotations { get; }

		// Token: 0x170001F4 RID: 500
		// (get) Token: 0x06000765 RID: 1893 RVA: 0x00006A76 File Offset: 0x00004C76
		public readonly ParticleSystemNativeArray3 rotations { get; }

		// Token: 0x170001F5 RID: 501
		// (get) Token: 0x06000766 RID: 1894 RVA: 0x00006A7E File Offset: 0x00004C7E
		public readonly ParticleSystemNativeArray3 rotationalSpeeds { get; }

		// Token: 0x170001F6 RID: 502
		// (get) Token: 0x06000767 RID: 1895 RVA: 0x00006A86 File Offset: 0x00004C86
		public readonly ParticleSystemNativeArray3 sizes { get; }

		// Token: 0x170001F7 RID: 503
		// (get) Token: 0x06000768 RID: 1896 RVA: 0x00006A8E File Offset: 0x00004C8E
		public readonly NativeArray<Color32> startColors { get; }

		// Token: 0x170001F8 RID: 504
		// (get) Token: 0x06000769 RID: 1897 RVA: 0x00006A96 File Offset: 0x00004C96
		public readonly NativeArray<float> aliveTimePercent { get; }

		// Token: 0x170001F9 RID: 505
		// (get) Token: 0x0600076A RID: 1898 RVA: 0x00006A9E File Offset: 0x00004C9E
		public readonly NativeArray<float> inverseStartLifetimes { get; }

		// Token: 0x170001FA RID: 506
		// (get) Token: 0x0600076B RID: 1899 RVA: 0x00006AA6 File Offset: 0x00004CA6
		public readonly NativeArray<uint> randomSeeds { get; }

		// Token: 0x170001FB RID: 507
		// (get) Token: 0x0600076C RID: 1900 RVA: 0x00006AAE File Offset: 0x00004CAE
		public readonly ParticleSystemNativeArray4 customData1 { get; }

		// Token: 0x170001FC RID: 508
		// (get) Token: 0x0600076D RID: 1901 RVA: 0x00006AB6 File Offset: 0x00004CB6
		public readonly ParticleSystemNativeArray4 customData2 { get; }

		// Token: 0x170001FD RID: 509
		// (get) Token: 0x0600076E RID: 1902 RVA: 0x00006ABE File Offset: 0x00004CBE
		public readonly NativeArray<int> meshIndices { get; }

		// Token: 0x0600076F RID: 1903 RVA: 0x00006AC8 File Offset: 0x00004CC8
		internal ParticleSystemJobData(ref NativeParticleData nativeData)
		{
			this = default(ParticleSystemJobData);
			this.count = nativeData.count;
			this.positions = this.CreateNativeArray3(ref nativeData.positions, this.count);
			this.velocities = this.CreateNativeArray3(ref nativeData.velocities, this.count);
			this.axisOfRotations = this.CreateNativeArray3(ref nativeData.axisOfRotations, this.count);
			this.rotations = this.CreateNativeArray3(ref nativeData.rotations, this.count);
			this.rotationalSpeeds = this.CreateNativeArray3(ref nativeData.rotationalSpeeds, this.count);
			this.sizes = this.CreateNativeArray3(ref nativeData.sizes, this.count);
			this.startColors = this.CreateNativeArray<Color32>(nativeData.startColors, this.count);
			this.aliveTimePercent = this.CreateNativeArray<float>(nativeData.aliveTimePercent, this.count);
			this.inverseStartLifetimes = this.CreateNativeArray<float>(nativeData.inverseStartLifetimes, this.count);
			this.randomSeeds = this.CreateNativeArray<uint>(nativeData.randomSeeds, this.count);
			this.customData1 = this.CreateNativeArray4(ref nativeData.customData1, this.count);
			this.customData2 = this.CreateNativeArray4(ref nativeData.customData2, this.count);
			this.meshIndices = this.CreateNativeArray<int>(nativeData.meshIndices, this.count);
		}

		// Token: 0x06000770 RID: 1904 RVA: 0x00006C24 File Offset: 0x00004E24
		internal unsafe NativeArray<T> CreateNativeArray<T>(void* src, int count) where T : struct
		{
			return NativeArrayUnsafeUtility.ConvertExistingDataToNativeArray<T>(src, count, Allocator.Invalid);
		}

		// Token: 0x06000771 RID: 1905 RVA: 0x00006C40 File Offset: 0x00004E40
		internal unsafe ParticleSystemNativeArray3 CreateNativeArray3(ref NativeParticleData.Array3 ptrs, int count)
		{
			return new ParticleSystemNativeArray3
			{
				x = this.CreateNativeArray<float>((void*)ptrs.x, count),
				y = this.CreateNativeArray<float>((void*)ptrs.y, count),
				z = this.CreateNativeArray<float>((void*)ptrs.z, count)
			};
		}

		// Token: 0x06000772 RID: 1906 RVA: 0x00006C98 File Offset: 0x00004E98
		internal unsafe ParticleSystemNativeArray4 CreateNativeArray4(ref NativeParticleData.Array4 ptrs, int count)
		{
			return new ParticleSystemNativeArray4
			{
				x = this.CreateNativeArray<float>((void*)ptrs.x, count),
				y = this.CreateNativeArray<float>((void*)ptrs.y, count),
				z = this.CreateNativeArray<float>((void*)ptrs.z, count),
				w = this.CreateNativeArray<float>((void*)ptrs.w, count)
			};
		}
	}
}
