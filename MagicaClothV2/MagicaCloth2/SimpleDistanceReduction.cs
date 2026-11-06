using System;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;

namespace MagicaCloth2
{
	// Token: 0x020000E0 RID: 224
	public class SimpleDistanceReduction : StepReductionBase
	{
		// Token: 0x060003DD RID: 989 RVA: 0x00028EA0 File Offset: 0x000270A0
		public SimpleDistanceReduction(string name, VirtualMesh mesh, ReductionWorkData workingData, float startMergeLength, float endMergeLength, int maxStep, bool dontMakeLine, float joinPositionAdjustment)
			: base("SimpleDistanceReduction [" + name + "]", mesh, workingData, startMergeLength, endMergeLength, maxStep, dontMakeLine, joinPositionAdjustment)
		{
		}

		// Token: 0x060003DE RID: 990 RVA: 0x00028ECF File Offset: 0x000270CF
		public override void Dispose()
		{
			base.Dispose();
			this.gridMap.Dispose();
		}

		// Token: 0x060003DF RID: 991 RVA: 0x00028EE2 File Offset: 0x000270E2
		protected override void StepInitialize()
		{
			base.StepInitialize();
			this.gridMap = new GridMap<int>(this.vmesh.VertexCount);
		}

		// Token: 0x060003E0 RID: 992 RVA: 0x00028F00 File Offset: 0x00027100
		protected override void CustomReductionStep()
		{
			float num = this.nowMergeLength * 2f;
			this.gridMap.GetMultiHashMap().Clear();
			new SimpleDistanceReduction.InitGridJob
			{
				vcnt = this.vmesh.VertexCount,
				gridSize = num,
				localPositions = this.vmesh.localPositions.GetNativeArray(),
				joinIndices = this.workData.vertexJoinIndices,
				gridMap = this.gridMap.GetMultiHashMap()
			}.Run<SimpleDistanceReduction.InitGridJob>();
			new SimpleDistanceReduction.SearchJoinEdgeJob
			{
				vcnt = this.vmesh.VertexCount,
				gridSize = num,
				radius = this.nowMergeLength,
				localPositions = this.vmesh.localPositions.GetNativeArray(),
				joinIndices = this.workData.vertexJoinIndices,
				vertexToVertexMap = this.workData.vertexToVertexMap,
				gridMap = this.gridMap.GetMultiHashMap(),
				joinEdgeList = this.joinEdgeList
			}.Run<SimpleDistanceReduction.SearchJoinEdgeJob>();
		}

		// Token: 0x0400081B RID: 2075
		private GridMap<int> gridMap;

		// Token: 0x020000E1 RID: 225
		[BurstCompile]
		private struct InitGridJob : IJob
		{
			// Token: 0x060003E1 RID: 993 RVA: 0x00029020 File Offset: 0x00027220
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

			// Token: 0x0400081C RID: 2076
			public int vcnt;

			// Token: 0x0400081D RID: 2077
			public float gridSize;

			// Token: 0x0400081E RID: 2078
			[ReadOnly]
			public NativeArray<float3> localPositions;

			// Token: 0x0400081F RID: 2079
			[ReadOnly]
			public NativeArray<int> joinIndices;

			// Token: 0x04000820 RID: 2080
			public NativeParallelMultiHashMap<int3, int> gridMap;
		}

		// Token: 0x020000E2 RID: 226
		[BurstCompile]
		private struct SearchJoinEdgeJob : IJob
		{
			// Token: 0x060003E2 RID: 994 RVA: 0x0002906C File Offset: 0x0002726C
			public void Execute()
			{
				for (int i = 0; i < this.vcnt; i++)
				{
					if (this.joinIndices[i] < 0)
					{
						float3 @float = this.localPositions[i];
						float num = (float)math.max(this.vertexToVertexMap.CountValuesForKey((ushort)i) - 1, 1);
						foreach (int3 @int in GridMap<int>.GetArea(@float, this.radius, this.gridMap, this.gridSize))
						{
							if (this.gridMap.ContainsKey(@int))
							{
								foreach (int num2 in this.gridMap.GetValuesForKey(@int))
								{
									if (num2 != i)
									{
										float3 float2 = this.localPositions[num2];
										float num3 = math.distance(@float, float2);
										if (num3 <= this.radius)
										{
											float num4 = (float)math.max(this.vertexToVertexMap.CountValuesForKey((ushort)num2) - 1, 1);
											float num5 = num3 * (1f + (num + num4) / 2f);
											StepReductionBase.JoinEdge joinEdge = new StepReductionBase.JoinEdge
											{
												vertexPair = new int2(i, num2),
												cost = num5
											};
											this.joinEdgeList.Add(in joinEdge);
										}
									}
								}
							}
						}
					}
				}
			}

			// Token: 0x04000821 RID: 2081
			public int vcnt;

			// Token: 0x04000822 RID: 2082
			public float gridSize;

			// Token: 0x04000823 RID: 2083
			public float radius;

			// Token: 0x04000824 RID: 2084
			[ReadOnly]
			public NativeArray<float3> localPositions;

			// Token: 0x04000825 RID: 2085
			[ReadOnly]
			public NativeArray<int> joinIndices;

			// Token: 0x04000826 RID: 2086
			[ReadOnly]
			public NativeParallelMultiHashMap<ushort, ushort> vertexToVertexMap;

			// Token: 0x04000827 RID: 2087
			[ReadOnly]
			public NativeParallelMultiHashMap<int3, int> gridMap;

			// Token: 0x04000828 RID: 2088
			[WriteOnly]
			public NativeList<StepReductionBase.JoinEdge> joinEdgeList;
		}
	}
}
