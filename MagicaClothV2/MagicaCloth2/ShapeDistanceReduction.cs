using System;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;

namespace MagicaCloth2
{
	// Token: 0x020000DE RID: 222
	public class ShapeDistanceReduction : StepReductionBase
	{
		// Token: 0x060003D8 RID: 984 RVA: 0x00028C68 File Offset: 0x00026E68
		public ShapeDistanceReduction(string name, VirtualMesh mesh, ReductionWorkData workingData, float startMergeLength, float endMergeLength, int maxStep, bool dontMakeLine, float joinPositionAdjustment)
			: base("ShapeReduction [" + name + "]", mesh, workingData, startMergeLength, endMergeLength, maxStep, dontMakeLine, joinPositionAdjustment)
		{
		}

		// Token: 0x060003D9 RID: 985 RVA: 0x00028C97 File Offset: 0x00026E97
		public override void Dispose()
		{
			base.Dispose();
		}

		// Token: 0x060003DA RID: 986 RVA: 0x00028C9F File Offset: 0x00026E9F
		protected override void StepInitialize()
		{
			base.StepInitialize();
		}

		// Token: 0x060003DB RID: 987 RVA: 0x00028CA8 File Offset: 0x00026EA8
		protected override void CustomReductionStep()
		{
			new ShapeDistanceReduction.SearchJoinEdgeJob
			{
				vcnt = this.vmesh.VertexCount,
				radius = this.nowMergeLength,
				dontMakeLine = this.dontMakeLine,
				localPositions = this.vmesh.localPositions.GetNativeArray(),
				joinIndices = this.workData.vertexJoinIndices,
				vertexToVertexMap = this.workData.vertexToVertexMap,
				joinEdgeList = this.joinEdgeList
			}.Run<ShapeDistanceReduction.SearchJoinEdgeJob>();
		}

		// Token: 0x020000DF RID: 223
		[BurstCompile]
		private struct SearchJoinEdgeJob : IJob
		{
			// Token: 0x060003DC RID: 988 RVA: 0x00028D38 File Offset: 0x00026F38
			public void Execute()
			{
				for (int i = 0; i < this.vcnt; i++)
				{
					if (this.joinIndices[i] < 0)
					{
						int num = this.vertexToVertexMap.CountValuesForKey((ushort)i);
						if (num != 0)
						{
							float3 @float = this.localPositions[i];
							float num2 = (float)math.max(num - 1, 1);
							float num3 = float.MaxValue;
							int num4 = -1;
							foreach (ushort num5 in this.vertexToVertexMap.GetValuesForKey((ushort)i))
							{
								float3 float2 = this.localPositions[(int)num5];
								float num6 = math.distance(@float, float2);
								if (num6 <= this.radius)
								{
									float num7 = (float)math.max(this.vertexToVertexMap.CountValuesForKey(num5) - 1, 1);
									if (StepReductionBase.CheckJoin2(in this.vertexToVertexMap, i, (int)num5, this.dontMakeLine))
									{
										float num8 = num6 * (1f + (num2 + num7) / 2f);
										if (num8 < num3)
										{
											num3 = num8;
											num4 = (int)num5;
										}
									}
								}
							}
							if (num4 >= 0)
							{
								StepReductionBase.JoinEdge joinEdge = new StepReductionBase.JoinEdge
								{
									vertexPair = new int2(i, num4),
									cost = num3
								};
								this.joinEdgeList.Add(in joinEdge);
							}
						}
					}
				}
			}

			// Token: 0x04000814 RID: 2068
			public int vcnt;

			// Token: 0x04000815 RID: 2069
			public float radius;

			// Token: 0x04000816 RID: 2070
			public bool dontMakeLine;

			// Token: 0x04000817 RID: 2071
			[ReadOnly]
			public NativeArray<float3> localPositions;

			// Token: 0x04000818 RID: 2072
			[ReadOnly]
			public NativeArray<int> joinIndices;

			// Token: 0x04000819 RID: 2073
			[ReadOnly]
			public NativeParallelMultiHashMap<ushort, ushort> vertexToVertexMap;

			// Token: 0x0400081A RID: 2074
			public NativeList<StepReductionBase.JoinEdge> joinEdgeList;
		}
	}
}
