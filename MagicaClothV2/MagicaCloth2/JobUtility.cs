using System;
using System.Runtime.CompilerServices;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;

namespace MagicaCloth2
{
	// Token: 0x020000F1 RID: 241
	public static class JobUtility
	{
		// Token: 0x06000449 RID: 1097 RVA: 0x0002A9B4 File Offset: 0x00028BB4
		public static JobHandle Fill(NativeArray<int> array, int length, int value, JobHandle dependsOn = default(JobHandle))
		{
			return new JobUtility.FillJob<int>
			{
				value = value,
				array = array
			}.Schedule(length, 32, dependsOn);
		}

		// Token: 0x0600044A RID: 1098 RVA: 0x0002A9E4 File Offset: 0x00028BE4
		public static JobHandle Fill(NativeArray<Vector4> array, int length, Vector4 value, JobHandle dependsOn = default(JobHandle))
		{
			return new JobUtility.FillJob<Vector4>
			{
				value = value,
				array = array
			}.Schedule(length, 32, dependsOn);
		}

		// Token: 0x0600044B RID: 1099 RVA: 0x0002AA14 File Offset: 0x00028C14
		public static JobHandle Fill(NativeArray<VirtualMeshBoneWeight> array, int length, VirtualMeshBoneWeight value, JobHandle dependsOn = default(JobHandle))
		{
			return new JobUtility.FillJob<VirtualMeshBoneWeight>
			{
				value = value,
				array = array
			}.Schedule(length, 32, dependsOn);
		}

		// Token: 0x0600044C RID: 1100 RVA: 0x0002AA44 File Offset: 0x00028C44
		public static JobHandle Fill(NativeArray<byte> array, int length, byte value, JobHandle dependsOn = default(JobHandle))
		{
			return new JobUtility.FillJob<byte>
			{
				value = value,
				array = array
			}.Schedule(length, 32, dependsOn);
		}

		// Token: 0x0600044D RID: 1101 RVA: 0x0002AA74 File Offset: 0x00028C74
		public static void FillRun(NativeArray<int> array, int length, int value)
		{
			new JobUtility.FillJob<int>
			{
				value = value,
				array = array
			}.Run(length);
		}

		// Token: 0x0600044E RID: 1102 RVA: 0x0002AAA0 File Offset: 0x00028CA0
		public static void FillRun(NativeArray<Vector4> array, int length, Vector4 value)
		{
			new JobUtility.FillJob<Vector4>
			{
				value = value,
				array = array
			}.Run(length);
		}

		// Token: 0x0600044F RID: 1103 RVA: 0x0002AACC File Offset: 0x00028CCC
		public static void FillRun(NativeArray<quaternion> array, int length, quaternion value)
		{
			new JobUtility.FillJob<quaternion>
			{
				value = value,
				array = array
			}.Run(length);
		}

		// Token: 0x06000450 RID: 1104 RVA: 0x0002AAF8 File Offset: 0x00028CF8
		public static void FillRun(NativeArray<VirtualMeshBoneWeight> array, int length, VirtualMeshBoneWeight value)
		{
			new JobUtility.FillJob<VirtualMeshBoneWeight>
			{
				value = value,
				array = array
			}.Run(length);
		}

		// Token: 0x06000451 RID: 1105 RVA: 0x0002AB24 File Offset: 0x00028D24
		public static JobHandle Fill(NativeArray<int> array, int startIndex, int length, int value, JobHandle dependsOn = default(JobHandle))
		{
			return new JobUtility.FillJob2<int>
			{
				value = value,
				startIndex = startIndex,
				array = array
			}.Schedule(length, 32, dependsOn);
		}

		// Token: 0x06000452 RID: 1106 RVA: 0x0002AB5C File Offset: 0x00028D5C
		public static JobHandle Fill(NativeReference<int> reference, int value, JobHandle dependsOn = default(JobHandle))
		{
			return new JobUtility.FillRefJob<int>
			{
				value = value,
				reference = reference
			}.Schedule(dependsOn);
		}

		// Token: 0x06000453 RID: 1107 RVA: 0x0002AB88 File Offset: 0x00028D88
		public static JobHandle SerialNumber(NativeArray<int> array, int length, JobHandle dependsOn = default(JobHandle))
		{
			return new JobUtility.SerialNumberJob
			{
				array = array
			}.Schedule(length, 32, dependsOn);
		}

		// Token: 0x06000454 RID: 1108 RVA: 0x0002ABB0 File Offset: 0x00028DB0
		public static void SerialNumberRun(NativeArray<int> array, int length)
		{
			new JobUtility.SerialNumberJob
			{
				array = array
			}.Run(length);
		}

		// Token: 0x06000455 RID: 1109 RVA: 0x0002ABD4 File Offset: 0x00028DD4
		public static JobHandle ConvertHashSetToNativeList(NativeParallelHashSet<int> hashSet, NativeList<int> list, JobHandle dependsOn = default(JobHandle))
		{
			return new JobUtility.ConvertHashSetToListJob<int>
			{
				hashSet = hashSet,
				list = list
			}.Schedule(dependsOn);
		}

		// Token: 0x06000456 RID: 1110 RVA: 0x0002AC00 File Offset: 0x00028E00
		public static JobHandle ConvertHashSetKeyToNativeList(NativeParallelHashSet<int2> hashSet, NativeList<int2> keyList, JobHandle dependsOn = default(JobHandle))
		{
			return new JobUtility.ConvertHashSetKeyToListJob<int2>
			{
				hashSet = hashSet,
				list = keyList
			}.Schedule(dependsOn);
		}

		// Token: 0x06000457 RID: 1111 RVA: 0x0002AC2C File Offset: 0x00028E2C
		public static JobHandle ConvertHashSetKeyToNativeList(NativeParallelHashSet<int4> hashSet, NativeList<int4> keyList, JobHandle dependsOn = default(JobHandle))
		{
			return new JobUtility.ConvertHashSetKeyToListJob<int4>
			{
				hashSet = hashSet,
				list = keyList
			}.Schedule(dependsOn);
		}

		// Token: 0x06000458 RID: 1112 RVA: 0x0002AC58 File Offset: 0x00028E58
		public static JobHandle CalcAABB(NativeArray<float3> positions, int length, NativeReference<AABB> outAABB, JobHandle dependsOn = default(JobHandle))
		{
			return new JobUtility.CalcAABBJob
			{
				length = length,
				positions = positions,
				outAABB = outAABB
			}.Schedule(dependsOn);
		}

		// Token: 0x06000459 RID: 1113 RVA: 0x0002AC8C File Offset: 0x00028E8C
		public static void CalcAABBRun(NativeArray<float3> positions, int length, NativeReference<AABB> outAABB)
		{
			new JobUtility.CalcAABBJob
			{
				length = length,
				positions = positions,
				outAABB = outAABB
			}.Run<JobUtility.CalcAABBJob>();
		}

		// Token: 0x0600045A RID: 1114 RVA: 0x0002ACC0 File Offset: 0x00028EC0
		public static JobHandle CalcAABB(NativeList<float3> positions, NativeReference<AABB> outAABB, JobHandle dependsOn = default(JobHandle))
		{
			return new JobUtility.CalcAABBDeferJob
			{
				positions = positions,
				outAABB = outAABB
			}.Schedule(dependsOn);
		}

		// Token: 0x0600045B RID: 1115 RVA: 0x0002ACEC File Offset: 0x00028EEC
		public static void CalcAABBRun(NativeList<float3> positions, NativeReference<AABB> outAABB)
		{
			new JobUtility.CalcAABBDeferJob
			{
				positions = positions,
				outAABB = outAABB
			}.Run<JobUtility.CalcAABBDeferJob>();
		}

		// Token: 0x0600045C RID: 1116 RVA: 0x0002AD18 File Offset: 0x00028F18
		private static AABB CalcAABBInternal(in NativeArray<float3> positions, int length)
		{
			NativeArray<float3> nativeArray = positions;
			if (nativeArray.Length == 0)
			{
				return default(AABB);
			}
			float3 @float = float.MaxValue;
			float3 float2 = float.MinValue;
			for (int i = 0; i < length; i++)
			{
				nativeArray = positions;
				float3 float3 = nativeArray[i];
				@float = math.min(@float, float3);
				float2 = math.max(float2, float3);
			}
			return new AABB(in @float, in float2);
		}

		// Token: 0x0600045D RID: 1117 RVA: 0x0002AD94 File Offset: 0x00028F94
		public static JobHandle CalcUVWithSphereMapping(NativeArray<float3> positions, int length, NativeReference<AABB> aabb, NativeArray<float2> outUVs, JobHandle dependsOn = default(JobHandle))
		{
			return new JobUtility.CalcUVJob
			{
				positions = positions,
				aabb = aabb,
				uvs = outUVs
			}.Schedule(length, 32, dependsOn);
		}

		// Token: 0x0600045E RID: 1118 RVA: 0x0002ADCC File Offset: 0x00028FCC
		public static void CalcUVWithSphereMappingRun(NativeArray<float3> positions, int length, NativeReference<AABB> aabb, NativeArray<float2> outUVs)
		{
			new JobUtility.CalcUVJob
			{
				positions = positions,
				aabb = aabb,
				uvs = outUVs
			}.Run(length);
		}

		// Token: 0x0600045F RID: 1119 RVA: 0x0002AE00 File Offset: 0x00029000
		public static JobHandle TransformPosition(NativeArray<float3> positions, int length, in float4x4 toM, JobHandle dependsOn = default(JobHandle))
		{
			return new JobUtility.TransformPositionJob
			{
				toM = toM,
				positions = positions
			}.Schedule(length, 32, dependsOn);
		}

		// Token: 0x06000460 RID: 1120 RVA: 0x0002AE34 File Offset: 0x00029034
		public static void TransformPositionRun(NativeArray<float3> positions, int length, in float4x4 toM)
		{
			new JobUtility.TransformPositionJob
			{
				toM = toM,
				positions = positions
			}.Run(length);
		}

		// Token: 0x06000461 RID: 1121 RVA: 0x0002AE68 File Offset: 0x00029068
		public static JobHandle TransformPosition(NativeArray<float3> srcPositions, NativeArray<float3> dstPositions, int length, in float4x4 toM, JobHandle dependsOn = default(JobHandle))
		{
			return new JobUtility.TransformPositionJob2
			{
				toM = toM,
				srcPositions = srcPositions,
				dstPositions = dstPositions
			}.Schedule(length, 32, dependsOn);
		}

		// Token: 0x06000462 RID: 1122 RVA: 0x0002AEA8 File Offset: 0x000290A8
		public static void TransformPositionRun(NativeArray<float3> srcPositions, NativeArray<float3> dstPositions, int length, in float4x4 toM)
		{
			new JobUtility.TransformPositionJob2
			{
				toM = toM,
				srcPositions = srcPositions,
				dstPositions = dstPositions
			}.Run(length);
		}

		// Token: 0x06000463 RID: 1123 RVA: 0x0002AEE4 File Offset: 0x000290E4
		public static NativeParallelMultiHashMap<int, ushort> ToNativeMultiHashMap(in NativeArray<uint> indexArray, in NativeArray<ushort> dataArray)
		{
			NativeArray<ushort> nativeArray = dataArray;
			NativeParallelMultiHashMap<int, ushort> nativeParallelMultiHashMap = new NativeParallelMultiHashMap<int, ushort>(nativeArray.Length, Allocator.Persistent);
			new JobUtility.ConvertArrayToMapJob<ushort>
			{
				indexArray = indexArray,
				dataArray = dataArray,
				map = nativeParallelMultiHashMap
			}.Run<JobUtility.ConvertArrayToMapJob<ushort>>();
			return nativeParallelMultiHashMap;
		}

		// Token: 0x06000464 RID: 1124 RVA: 0x0002AF40 File Offset: 0x00029140
		public static JobHandle ClearReference(NativeReference<int> reference, JobHandle jobHandle)
		{
			return new JobUtility.ClearReferenceJob
			{
				reference = reference
			}.Schedule(jobHandle);
		}

		// Token: 0x020000F2 RID: 242
		[BurstCompile]
		private struct FillJob<[IsUnmanaged] T> : IJobParallelFor where T : struct, ValueType
		{
			// Token: 0x06000465 RID: 1125 RVA: 0x0002AF64 File Offset: 0x00029164
			public void Execute(int index)
			{
				this.array[index] = this.value;
			}

			// Token: 0x0400086A RID: 2154
			public T value;

			// Token: 0x0400086B RID: 2155
			[WriteOnly]
			public NativeArray<T> array;
		}

		// Token: 0x020000F3 RID: 243
		[BurstCompile]
		private struct FillJob2<[IsUnmanaged] T> : IJobParallelFor where T : struct, ValueType
		{
			// Token: 0x06000466 RID: 1126 RVA: 0x0002AF78 File Offset: 0x00029178
			public void Execute(int index)
			{
				this.array[this.startIndex + index] = this.value;
			}

			// Token: 0x0400086C RID: 2156
			public T value;

			// Token: 0x0400086D RID: 2157
			public int startIndex;

			// Token: 0x0400086E RID: 2158
			[NativeDisableParallelForRestriction]
			[WriteOnly]
			public NativeArray<T> array;
		}

		// Token: 0x020000F4 RID: 244
		[BurstCompile]
		private struct FillRefJob<[IsUnmanaged] T> : IJob where T : struct, ValueType
		{
			// Token: 0x06000467 RID: 1127 RVA: 0x0002AF93 File Offset: 0x00029193
			public void Execute()
			{
				this.reference.Value = this.value;
			}

			// Token: 0x0400086F RID: 2159
			public T value;

			// Token: 0x04000870 RID: 2160
			[WriteOnly]
			public NativeReference<T> reference;
		}

		// Token: 0x020000F5 RID: 245
		[BurstCompile]
		private struct SerialNumberJob : IJobParallelFor
		{
			// Token: 0x06000468 RID: 1128 RVA: 0x0002AFA6 File Offset: 0x000291A6
			public void Execute(int index)
			{
				this.array[index] = index;
			}

			// Token: 0x04000871 RID: 2161
			[WriteOnly]
			public NativeArray<int> array;
		}

		// Token: 0x020000F6 RID: 246
		[BurstCompile]
		private struct ConvertHashSetToListJob<[IsUnmanaged] T> : IJob where T : struct, ValueType, IEquatable<T>
		{
			// Token: 0x06000469 RID: 1129 RVA: 0x0002AFB8 File Offset: 0x000291B8
			public void Execute()
			{
				foreach (T t in this.hashSet)
				{
					this.list.AddNoResize(t);
				}
			}

			// Token: 0x04000872 RID: 2162
			[ReadOnly]
			public NativeParallelHashSet<T> hashSet;

			// Token: 0x04000873 RID: 2163
			[WriteOnly]
			public NativeList<T> list;
		}

		// Token: 0x020000F7 RID: 247
		[BurstCompile]
		private struct ConvertHashSetKeyToListJob<[IsUnmanaged] T> : IJob where T : struct, ValueType, IEquatable<T>
		{
			// Token: 0x0600046A RID: 1130 RVA: 0x0002B010 File Offset: 0x00029210
			public void Execute()
			{
				foreach (T t in this.hashSet)
				{
					this.list.Add(in t);
				}
			}

			// Token: 0x04000874 RID: 2164
			[ReadOnly]
			public NativeParallelHashSet<T> hashSet;

			// Token: 0x04000875 RID: 2165
			[WriteOnly]
			public NativeList<T> list;
		}

		// Token: 0x020000F8 RID: 248
		[BurstCompile]
		private struct CalcAABBJob : IJob
		{
			// Token: 0x0600046B RID: 1131 RVA: 0x0002B06C File Offset: 0x0002926C
			public void Execute()
			{
				this.outAABB.Value = JobUtility.CalcAABBInternal(in this.positions, this.length);
			}

			// Token: 0x04000876 RID: 2166
			public int length;

			// Token: 0x04000877 RID: 2167
			[ReadOnly]
			public NativeArray<float3> positions;

			// Token: 0x04000878 RID: 2168
			public NativeReference<AABB> outAABB;
		}

		// Token: 0x020000F9 RID: 249
		[BurstCompile]
		private struct CalcAABBDeferJob : IJob
		{
			// Token: 0x0600046C RID: 1132 RVA: 0x0002B08C File Offset: 0x0002928C
			public void Execute()
			{
				NativeArray<float3> nativeArray = this.positions.AsArray();
				this.outAABB.Value = JobUtility.CalcAABBInternal(in nativeArray, this.positions.Length);
			}

			// Token: 0x04000879 RID: 2169
			[ReadOnly]
			public NativeList<float3> positions;

			// Token: 0x0400087A RID: 2170
			public NativeReference<AABB> outAABB;
		}

		// Token: 0x020000FA RID: 250
		[BurstCompile]
		private struct CalcUVJob : IJobParallelFor
		{
			// Token: 0x0600046D RID: 1133 RVA: 0x0002B0C4 File Offset: 0x000292C4
			public void Execute(int index)
			{
				float3 @float = this.positions[index] - this.aabb.Value.Center;
				@float = math.normalize(@float);
				float num = math.atan2(@float.x, @float.z);
				num = math.clamp(math.unlerp(-3.1415927f, 3.1415927f, num), 0f, 1f);
				float num2 = math.dot(math.up(), @float);
				num2 = math.clamp(math.unlerp(1f, -1f, num2), 0f, 1f);
				float2 float2 = new float2(num2, num);
				float num3 = (float)index * 0.0001234f;
				float2 = float2 * 10f + num3;
				this.uvs[index] = float2;
			}

			// Token: 0x0400087B RID: 2171
			[ReadOnly]
			public NativeArray<float3> positions;

			// Token: 0x0400087C RID: 2172
			[ReadOnly]
			public NativeReference<AABB> aabb;

			// Token: 0x0400087D RID: 2173
			[WriteOnly]
			public NativeArray<float2> uvs;
		}

		// Token: 0x020000FB RID: 251
		[BurstCompile]
		public struct AddIntDataCopyJob : IJobParallelFor
		{
			// Token: 0x0600046E RID: 1134 RVA: 0x0002B190 File Offset: 0x00029390
			public void Execute(int index)
			{
				int num = this.dstOffset + index;
				int num2 = this.srcData[index];
				num2 += this.addData;
				this.dstData[num] = num2;
			}

			// Token: 0x0400087E RID: 2174
			public int dstOffset;

			// Token: 0x0400087F RID: 2175
			public int addData;

			// Token: 0x04000880 RID: 2176
			[ReadOnly]
			public NativeArray<int> srcData;

			// Token: 0x04000881 RID: 2177
			[NativeDisableParallelForRestriction]
			[WriteOnly]
			public NativeArray<int> dstData;
		}

		// Token: 0x020000FC RID: 252
		[BurstCompile]
		public struct AddInt2DataCopyJob : IJobParallelFor
		{
			// Token: 0x0600046F RID: 1135 RVA: 0x0002B1CC File Offset: 0x000293CC
			public void Execute(int index)
			{
				int num = this.dstOffset + index;
				int2 @int = this.srcData[index];
				@int += this.addData;
				this.dstData[num] = @int;
			}

			// Token: 0x04000882 RID: 2178
			public int dstOffset;

			// Token: 0x04000883 RID: 2179
			public int2 addData;

			// Token: 0x04000884 RID: 2180
			[ReadOnly]
			public NativeArray<int2> srcData;

			// Token: 0x04000885 RID: 2181
			[NativeDisableParallelForRestriction]
			[WriteOnly]
			public NativeArray<int2> dstData;
		}

		// Token: 0x020000FD RID: 253
		[BurstCompile]
		public struct AddInt3DataCopyJob : IJobParallelFor
		{
			// Token: 0x06000470 RID: 1136 RVA: 0x0002B20C File Offset: 0x0002940C
			public void Execute(int index)
			{
				int num = this.dstOffset + index;
				int3 @int = this.srcData[index];
				@int += this.addData;
				this.dstData[num] = @int;
			}

			// Token: 0x04000886 RID: 2182
			public int dstOffset;

			// Token: 0x04000887 RID: 2183
			public int3 addData;

			// Token: 0x04000888 RID: 2184
			[ReadOnly]
			public NativeArray<int3> srcData;

			// Token: 0x04000889 RID: 2185
			[NativeDisableParallelForRestriction]
			[WriteOnly]
			public NativeArray<int3> dstData;
		}

		// Token: 0x020000FE RID: 254
		[BurstCompile]
		public struct TransformPositionJob : IJobParallelFor
		{
			// Token: 0x06000471 RID: 1137 RVA: 0x0002B24C File Offset: 0x0002944C
			public void Execute(int vindex)
			{
				float3 @float = this.positions[vindex];
				this.positions[vindex] = MathUtility.TransformPoint(in @float, in this.toM);
			}

			// Token: 0x0400088A RID: 2186
			public float4x4 toM;

			// Token: 0x0400088B RID: 2187
			public NativeArray<float3> positions;
		}

		// Token: 0x020000FF RID: 255
		[BurstCompile]
		public struct TransformPositionJob2 : IJobParallelFor
		{
			// Token: 0x06000472 RID: 1138 RVA: 0x0002B280 File Offset: 0x00029480
			public void Execute(int vindex)
			{
				float3 @float = this.srcPositions[vindex];
				this.dstPositions[vindex] = MathUtility.TransformPoint(in @float, in this.toM);
			}

			// Token: 0x0400088C RID: 2188
			public float4x4 toM;

			// Token: 0x0400088D RID: 2189
			[ReadOnly]
			public NativeArray<float3> srcPositions;

			// Token: 0x0400088E RID: 2190
			[WriteOnly]
			public NativeArray<float3> dstPositions;
		}

		// Token: 0x02000100 RID: 256
		[BurstCompile]
		private struct ConvertArrayToMapJob<[IsUnmanaged] TData> : IJob where TData : struct, ValueType
		{
			// Token: 0x06000473 RID: 1139 RVA: 0x0002B2B4 File Offset: 0x000294B4
			public void Execute()
			{
				int length = this.indexArray.Length;
				for (int i = 0; i < length; i++)
				{
					int num;
					int num2;
					DataUtility.Unpack12_20(this.indexArray[i], out num, out num2);
					for (int j = 0; j < num; j++)
					{
						TData tdata = this.dataArray[num2 + j];
						this.map.Add(i, tdata);
					}
				}
			}

			// Token: 0x0400088F RID: 2191
			[ReadOnly]
			public NativeArray<uint> indexArray;

			// Token: 0x04000890 RID: 2192
			[ReadOnly]
			public NativeArray<TData> dataArray;

			// Token: 0x04000891 RID: 2193
			[WriteOnly]
			public NativeParallelMultiHashMap<int, TData> map;
		}

		// Token: 0x02000101 RID: 257
		[BurstCompile]
		private struct ClearReferenceJob : IJob
		{
			// Token: 0x06000474 RID: 1140 RVA: 0x0002B31D File Offset: 0x0002951D
			public void Execute()
			{
				this.reference.Value = 0;
			}

			// Token: 0x04000892 RID: 2194
			public NativeReference<int> reference;
		}
	}
}
