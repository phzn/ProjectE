using System;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;

namespace MagicaCloth2
{
	// Token: 0x020000D7 RID: 215
	public class SameDistanceReduction : IDisposable
	{
		// Token: 0x060003CB RID: 971 RVA: 0x000282FB File Offset: 0x000264FB
		public SameDistanceReduction()
		{
		}

		// Token: 0x060003CC RID: 972 RVA: 0x00028310 File Offset: 0x00026510
		public SameDistanceReduction(string name, VirtualMesh mesh, ReductionWorkData workingData, float mergeLength)
		{
			this.name = name;
			this.vmesh = mesh;
			this.workData = workingData;
			this.result = ResultCode.None;
			this.mergeLength = math.max(mergeLength, 1E-09f);
		}

		// Token: 0x060003CD RID: 973 RVA: 0x00028360 File Offset: 0x00026560
		public virtual void Dispose()
		{
			if (this.joinPairMap.IsCreated)
			{
				this.joinPairMap.Dispose();
			}
			if (this.resultRef.IsCreated)
			{
				this.resultRef.Dispose();
			}
			GridMap<int> gridMap = this.gridMap;
			if (gridMap == null)
			{
				return;
			}
			gridMap.Dispose();
		}

		// Token: 0x17000071 RID: 113
		// (get) Token: 0x060003CE RID: 974 RVA: 0x000283AD File Offset: 0x000265AD
		public ResultCode Result
		{
			get
			{
				return this.result;
			}
		}

		// Token: 0x060003CF RID: 975 RVA: 0x000283B8 File Offset: 0x000265B8
		public ResultCode Reduction()
		{
			this.result.Clear();
			try
			{
				this.gridMap = new GridMap<int>(this.vmesh.VertexCount);
				float num = this.mergeLength * 2f;
				this.joinPairMap = new NativeParallelMultiHashMap<ushort, ushort>(this.vmesh.VertexCount, Allocator.Persistent);
				this.resultRef = new NativeReference<int>(Allocator.Persistent, NativeArrayOptions.ClearMemory);
				new SameDistanceReduction.InitGridJob
				{
					vcnt = this.vmesh.VertexCount,
					gridSize = num,
					localPositions = this.vmesh.localPositions.GetNativeArray(),
					joinIndices = this.workData.vertexJoinIndices,
					gridMap = this.gridMap.GetMultiHashMap()
				}.Run<SameDistanceReduction.InitGridJob>();
				new SameDistanceReduction.SearchJoinJob
				{
					vcnt = this.vmesh.VertexCount,
					gridSize = num,
					radius = this.mergeLength,
					localPositions = this.vmesh.localPositions.GetNativeArray(),
					joinIndices = this.workData.vertexJoinIndices,
					gridMap = this.gridMap.GetMultiHashMap(),
					joinPairMap = this.joinPairMap
				}.Run<SameDistanceReduction.SearchJoinJob>();
				using (NativeList<ushort> nativeList = new NativeList<ushort>(2048, Allocator.Persistent))
				{
					new SameDistanceReduction.JoinJob2
					{
						vertexCount = this.vmesh.VertexCount,
						joinPairMap = this.joinPairMap,
						joinIndices = this.workData.vertexJoinIndices,
						vertexToVertexMap = this.workData.vertexToVertexMap,
						boneWeights = this.vmesh.boneWeights.GetNativeArray(),
						attributes = this.vmesh.attributes.GetNativeArray(),
						result = this.resultRef,
						tempList = nativeList
					}.Run<SameDistanceReduction.JoinJob2>();
					this.UpdateJoinAndLink();
					this.UpdateReductionResultJob();
					int value = this.resultRef.Value;
					this.workData.removeVertexCount += value;
					this.result.SetSuccess();
				}
			}
			catch (Exception ex)
			{
				Debug.LogError(ex);
				this.result.SetError(Define.Result.Reduction_SameDistanceException);
			}
			return this.result;
		}

		// Token: 0x060003D0 RID: 976 RVA: 0x00028644 File Offset: 0x00026844
		private void UpdateJoinAndLink()
		{
			new SameDistanceReduction.UpdateJoinIndexJob
			{
				joinIndices = this.workData.vertexJoinIndices
			}.Run(this.vmesh.VertexCount);
			new SameDistanceReduction.UpdateLinkIndexJob
			{
				joinIndices = this.workData.vertexJoinIndices,
				vertexToVertexMap = this.workData.vertexToVertexMap
			}.Run(this.vmesh.VertexCount);
		}

		// Token: 0x060003D1 RID: 977 RVA: 0x000286BC File Offset: 0x000268BC
		private void UpdateReductionResultJob()
		{
			new SameDistanceReduction.FinalMergeVertexJob
			{
				joinIndices = this.workData.vertexJoinIndices,
				localNormals = this.vmesh.localNormals.GetNativeArray(),
				boneWeights = this.vmesh.boneWeights.GetNativeArray()
			}.Run(this.vmesh.VertexCount);
		}

		// Token: 0x040007F2 RID: 2034
		private string name = string.Empty;

		// Token: 0x040007F3 RID: 2035
		private VirtualMesh vmesh;

		// Token: 0x040007F4 RID: 2036
		private ReductionWorkData workData;

		// Token: 0x040007F5 RID: 2037
		private ResultCode result;

		// Token: 0x040007F6 RID: 2038
		private float mergeLength;

		// Token: 0x040007F7 RID: 2039
		private GridMap<int> gridMap;

		// Token: 0x040007F8 RID: 2040
		private NativeParallelMultiHashMap<ushort, ushort> joinPairMap;

		// Token: 0x040007F9 RID: 2041
		private NativeReference<int> resultRef;

		// Token: 0x020000D8 RID: 216
		[BurstCompile]
		private struct InitGridJob : IJob
		{
			// Token: 0x060003D2 RID: 978 RVA: 0x00028724 File Offset: 0x00026924
			public void Execute()
			{
				for (int i = 0; i < this.vcnt; i++)
				{
					if (this.joinIndices[i] < 0)
					{
						GridMap<int>.AddGrid(this.localPositions[i], i, this.gridMap, this.gridSize);
					}
				}
			}

			// Token: 0x040007FA RID: 2042
			public int vcnt;

			// Token: 0x040007FB RID: 2043
			public float gridSize;

			// Token: 0x040007FC RID: 2044
			[ReadOnly]
			public NativeArray<float3> localPositions;

			// Token: 0x040007FD RID: 2045
			[ReadOnly]
			public NativeArray<int> joinIndices;

			// Token: 0x040007FE RID: 2046
			public NativeParallelMultiHashMap<int3, int> gridMap;
		}

		// Token: 0x020000D9 RID: 217
		[BurstCompile]
		private struct SearchJoinJob : IJob
		{
			// Token: 0x060003D3 RID: 979 RVA: 0x00028770 File Offset: 0x00026970
			public void Execute()
			{
				for (int i = 0; i < this.vcnt; i++)
				{
					if (this.joinIndices[i] < 0)
					{
						float3 @float = this.localPositions[i];
						foreach (int3 @int in GridMap<int>.GetArea(@float, this.radius, this.gridMap, this.gridSize))
						{
							if (this.gridMap.ContainsKey(@int))
							{
								foreach (int num in this.gridMap.GetValuesForKey(@int))
								{
									if (num != i)
									{
										float3 float2 = this.localPositions[num];
										if (math.distance(@float, float2) <= this.radius)
										{
											this.joinPairMap.Add((ushort)i, (ushort)num);
										}
									}
								}
							}
						}
					}
				}
			}

			// Token: 0x040007FF RID: 2047
			public int vcnt;

			// Token: 0x04000800 RID: 2048
			public float gridSize;

			// Token: 0x04000801 RID: 2049
			public float radius;

			// Token: 0x04000802 RID: 2050
			[ReadOnly]
			public NativeArray<float3> localPositions;

			// Token: 0x04000803 RID: 2051
			[ReadOnly]
			public NativeArray<int> joinIndices;

			// Token: 0x04000804 RID: 2052
			[ReadOnly]
			public NativeParallelMultiHashMap<int3, int> gridMap;

			// Token: 0x04000805 RID: 2053
			public NativeParallelMultiHashMap<ushort, ushort> joinPairMap;
		}

		// Token: 0x020000DA RID: 218
		[BurstCompile]
		private struct JoinJob2 : IJob
		{
			// Token: 0x060003D4 RID: 980 RVA: 0x0002889C File Offset: 0x00026A9C
			public void Execute()
			{
				int num = 0;
				ushort num2 = 0;
				while ((int)num2 < this.vertexCount)
				{
					if (this.joinIndices[(int)num2] < 0)
					{
						NativeParallelMultiHashMap<ushort, ushort>.Enumerator enumerator = this.joinPairMap.GetValuesForKey(num2);
						foreach (ushort num3 in enumerator)
						{
							if (this.joinIndices[(int)num3] < 0)
							{
								this.joinIndices[(int)num3] = (int)num2;
								num++;
								(ref this.vertexToVertexMap).MC2RemoveValue(num2, num3);
								this.tempList.Clear();
								foreach (ushort num4 in this.vertexToVertexMap.GetValuesForKey(num3))
								{
									this.tempList.Add(in num4);
								}
								foreach (ushort num5 in this.tempList)
								{
									if (this.joinIndices[(int)num5] < 0 && num5 != num2 && num5 != num3)
									{
										(ref this.vertexToVertexMap).MC2RemoveValue(num5, num3);
										(ref this.vertexToVertexMap).MC2UniqueAdd(num2, num5);
										(ref this.vertexToVertexMap).MC2UniqueAdd(num5, num2);
										VirtualMeshBoneWeight virtualMeshBoneWeight = this.boneWeights[(int)num2];
										VirtualMeshBoneWeight virtualMeshBoneWeight2 = this.boneWeights[(int)num3];
										virtualMeshBoneWeight.AddWeight(in virtualMeshBoneWeight2);
										this.boneWeights[(int)num2] = virtualMeshBoneWeight;
										VertexAttribute vertexAttribute = this.attributes[(int)num3];
										VertexAttribute vertexAttribute2 = this.attributes[(int)num2];
										this.attributes[(int)num2] = VertexAttribute.JoinAttribute(vertexAttribute, vertexAttribute2);
										this.attributes[(int)num3] = VertexAttribute.Invalid;
									}
								}
							}
						}
					}
					num2 += 1;
				}
				this.result.Value = num;
			}

			// Token: 0x04000806 RID: 2054
			public int vertexCount;

			// Token: 0x04000807 RID: 2055
			[ReadOnly]
			public NativeParallelMultiHashMap<ushort, ushort> joinPairMap;

			// Token: 0x04000808 RID: 2056
			public NativeArray<int> joinIndices;

			// Token: 0x04000809 RID: 2057
			public NativeParallelMultiHashMap<ushort, ushort> vertexToVertexMap;

			// Token: 0x0400080A RID: 2058
			public NativeArray<VirtualMeshBoneWeight> boneWeights;

			// Token: 0x0400080B RID: 2059
			public NativeArray<VertexAttribute> attributes;

			// Token: 0x0400080C RID: 2060
			public NativeReference<int> result;

			// Token: 0x0400080D RID: 2061
			public NativeList<ushort> tempList;
		}

		// Token: 0x020000DB RID: 219
		[BurstCompile]
		private struct UpdateJoinIndexJob : IJobParallelFor
		{
			// Token: 0x060003D5 RID: 981 RVA: 0x00028AF4 File Offset: 0x00026CF4
			public void Execute(int vindex)
			{
				int num = this.joinIndices[vindex];
				if (num >= 0)
				{
					while (this.joinIndices[num] >= 0)
					{
						num = this.joinIndices[num];
					}
					this.joinIndices[vindex] = num;
				}
			}

			// Token: 0x0400080E RID: 2062
			[NativeDisableParallelForRestriction]
			public NativeArray<int> joinIndices;
		}

		// Token: 0x020000DC RID: 220
		[BurstCompile]
		private struct UpdateLinkIndexJob : IJobParallelFor
		{
			// Token: 0x060003D6 RID: 982 RVA: 0x00028B40 File Offset: 0x00026D40
			public void Execute(int vindex)
			{
				if (this.joinIndices[vindex] >= 0)
				{
					return;
				}
				FixedList512Bytes<ushort> fixedList512Bytes = default(FixedList512Bytes<ushort>);
				using (NativeParallelMultiHashMap<ushort, ushort>.Enumerator enumerator = this.vertexToVertexMap.GetValuesForKey((ushort)vindex).GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						int num = (int)enumerator.Current;
						int num2 = this.joinIndices[num];
						if (num2 >= 0)
						{
							num = num2;
						}
						if (num != vindex)
						{
							(ref fixedList512Bytes).MC2Set((ushort)num);
						}
					}
				}
				this.vertexToVertexMap.Remove((ushort)vindex);
				for (int i = 0; i < fixedList512Bytes.Length; i++)
				{
					this.vertexToVertexMap.Add((ushort)vindex, fixedList512Bytes[i]);
				}
			}

			// Token: 0x0400080F RID: 2063
			[NativeDisableParallelForRestriction]
			public NativeArray<int> joinIndices;

			// Token: 0x04000810 RID: 2064
			public NativeParallelMultiHashMap<ushort, ushort> vertexToVertexMap;
		}

		// Token: 0x020000DD RID: 221
		[BurstCompile]
		private struct FinalMergeVertexJob : IJobParallelFor
		{
			// Token: 0x060003D7 RID: 983 RVA: 0x00028C0C File Offset: 0x00026E0C
			public void Execute(int vindex)
			{
				if (this.joinIndices[vindex] >= 0)
				{
					return;
				}
				this.localNormals[vindex] = math.normalize(this.localNormals[vindex]);
				VirtualMeshBoneWeight virtualMeshBoneWeight = this.boneWeights[vindex];
				virtualMeshBoneWeight.AdjustWeight();
				this.boneWeights[vindex] = virtualMeshBoneWeight;
			}

			// Token: 0x04000811 RID: 2065
			[ReadOnly]
			public NativeArray<int> joinIndices;

			// Token: 0x04000812 RID: 2066
			public NativeArray<float3> localNormals;

			// Token: 0x04000813 RID: 2067
			public NativeArray<VirtualMeshBoneWeight> boneWeights;
		}
	}
}
