using System;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;

namespace MagicaCloth2
{
	// Token: 0x020000E3 RID: 227
	public abstract class StepReductionBase : IDisposable
	{
		// Token: 0x060003E3 RID: 995 RVA: 0x0002920C File Offset: 0x0002740C
		public StepReductionBase()
		{
		}

		// Token: 0x060003E4 RID: 996 RVA: 0x00029220 File Offset: 0x00027420
		public StepReductionBase(string name, VirtualMesh mesh, ReductionWorkData workingData, float startMergeLength, float endMergeLength, int maxStep, bool dontMakeLine, float joinPositionAdjustment)
		{
			this.name = name;
			this.vmesh = mesh;
			this.workData = workingData;
			this.result = ResultCode.None;
			this.startMergeLength = math.max(startMergeLength, 1E-09f);
			this.endMergeLength = math.max(endMergeLength, 1E-09f);
			this.maxStep = math.min(maxStep, 100);
			this.dontMakeLine = dontMakeLine;
			this.joinPositionAdjustment = joinPositionAdjustment;
		}

		// Token: 0x060003E5 RID: 997 RVA: 0x000292A4 File Offset: 0x000274A4
		public virtual void Dispose()
		{
			if (this.joinEdgeList.IsCreated)
			{
				this.joinEdgeList.Dispose();
			}
			if (this.completeVertexSet.IsCreated)
			{
				this.completeVertexSet.Dispose();
			}
			if (this.removePairList.IsCreated)
			{
				this.removePairList.Dispose();
			}
			if (this.resultArray.IsCreated)
			{
				this.resultArray.Dispose();
			}
		}

		// Token: 0x17000072 RID: 114
		// (get) Token: 0x060003E6 RID: 998 RVA: 0x00029311 File Offset: 0x00027511
		public ResultCode Result
		{
			get
			{
				return this.result;
			}
		}

		// Token: 0x060003E7 RID: 999 RVA: 0x0002931C File Offset: 0x0002751C
		public ResultCode Reduction()
		{
			this.result.Clear();
			try
			{
				this.StepInitialize();
				this.InitStep();
				while (this.nowStepIndex < this.maxStep)
				{
					this.ReductionStep();
					this.nowStepIndex++;
					if (this.IsEndStep())
					{
						break;
					}
					this.NextStep();
				}
				if (this.nowStepIndex < this.maxStep)
				{
					this.ReductionStep();
					this.nowStepIndex++;
				}
				this.UpdateReductionResultJob();
				int num = 0;
				for (int i = 0; i < this.nowStepIndex; i++)
				{
					num += this.resultArray[i];
				}
				this.workData.removeVertexCount += num;
				this.result.SetSuccess();
			}
			catch (Exception ex)
			{
				Debug.LogError(ex);
				if (!this.result.IsError())
				{
					if (this is SimpleDistanceReduction)
					{
						this.result.SetError(Define.Result.Reduction_SimpleDistanceException);
					}
					else if (this is ShapeDistanceReduction)
					{
						this.result.SetError(Define.Result.Reduction_ShapeDistanceException);
					}
					else
					{
						this.result.SetError(Define.Result.Reduction_Exception);
					}
				}
			}
			return this.result;
		}

		// Token: 0x060003E8 RID: 1000 RVA: 0x0002944C File Offset: 0x0002764C
		private void InitStep()
		{
			this.nowStepIndex = 0;
			this.nowMergeLength = this.startMergeLength;
			this.nowStepScale = 2f;
		}

		// Token: 0x060003E9 RID: 1001 RVA: 0x0002946C File Offset: 0x0002766C
		private bool IsEndStep()
		{
			return this.nowMergeLength == this.endMergeLength;
		}

		// Token: 0x060003EA RID: 1002 RVA: 0x0002947C File Offset: 0x0002767C
		private void NextStep()
		{
			this.nowStepScale = math.max(this.nowStepScale * 0.93f, 1.1f);
			this.nowMergeLength = math.min(this.nowMergeLength * this.nowStepScale, this.endMergeLength);
		}

		// Token: 0x060003EB RID: 1003 RVA: 0x000294B8 File Offset: 0x000276B8
		private void ReductionStep()
		{
			this.PreReductionStep();
			this.CustomReductionStep();
			this.PostReductionStep();
		}

		// Token: 0x060003EC RID: 1004 RVA: 0x000294CC File Offset: 0x000276CC
		protected virtual void StepInitialize()
		{
			int vertexCount = this.vmesh.VertexCount;
			this.joinEdgeList = new NativeList<StepReductionBase.JoinEdge>(vertexCount / 4, Allocator.Persistent);
			this.completeVertexSet = new NativeParallelHashSet<int>(vertexCount, Allocator.Persistent);
			this.removePairList = new NativeList<int2>(vertexCount, Allocator.Persistent);
			this.resultArray = new NativeArray<int>(this.maxStep, Allocator.Persistent, NativeArrayOptions.ClearMemory);
		}

		// Token: 0x060003ED RID: 1005 RVA: 0x0000706D File Offset: 0x0000526D
		protected virtual void CustomReductionStep()
		{
		}

		// Token: 0x060003EE RID: 1006 RVA: 0x00029530 File Offset: 0x00027730
		private void PreReductionStep()
		{
			this.joinEdgeList.Clear();
		}

		// Token: 0x060003EF RID: 1007 RVA: 0x0002953D File Offset: 0x0002773D
		private void PostReductionStep()
		{
			this.SortJoinEdge();
			this.DetermineJoinEdge();
			this.RunJoinEdge();
			this.UpdateJoinAndLink();
		}

		// Token: 0x060003F0 RID: 1008 RVA: 0x00029557 File Offset: 0x00027757
		private void SortJoinEdge()
		{
			this.joinEdgeList.Sort<StepReductionBase.JoinEdge>();
		}

		// Token: 0x060003F1 RID: 1009 RVA: 0x00029564 File Offset: 0x00027764
		private void DetermineJoinEdge()
		{
			new StepReductionBase.DeterminJoinEdgeJob
			{
				stepIndex = this.nowStepIndex,
				mergeLength = this.nowMergeLength,
				joinEdgeList = this.joinEdgeList,
				completeVertexSet = this.completeVertexSet,
				removePairList = this.removePairList,
				resultArray = this.resultArray
			}.Run<StepReductionBase.DeterminJoinEdgeJob>();
		}

		// Token: 0x060003F2 RID: 1010 RVA: 0x000295D0 File Offset: 0x000277D0
		private void RunJoinEdge()
		{
			new StepReductionBase.JoinPairJob
			{
				joinPositionAdjustment = this.joinPositionAdjustment,
				removePairList = this.removePairList,
				localPositions = this.vmesh.localPositions.GetNativeArray(),
				localNormals = this.vmesh.localNormals.GetNativeArray(),
				joinIndices = this.workData.vertexJoinIndices,
				vertexToVertexMap = this.workData.vertexToVertexMap,
				boneWeights = this.vmesh.boneWeights.GetNativeArray(),
				attributes = this.vmesh.attributes.GetNativeArray()
			}.Run<StepReductionBase.JoinPairJob>();
		}

		// Token: 0x060003F3 RID: 1011 RVA: 0x00029688 File Offset: 0x00027888
		private void UpdateJoinAndLink()
		{
			new StepReductionBase.UpdateJoinIndexJob
			{
				joinIndices = this.workData.vertexJoinIndices
			}.Run(this.vmesh.VertexCount);
			new StepReductionBase.UpdateLinkIndexJob
			{
				joinIndices = this.workData.vertexJoinIndices,
				vertexToVertexMap = this.workData.vertexToVertexMap
			}.Run(this.vmesh.VertexCount);
		}

		// Token: 0x060003F4 RID: 1012 RVA: 0x00029700 File Offset: 0x00027900
		private void UpdateReductionResultJob()
		{
			new StepReductionBase.FinalMergeVertexJob
			{
				joinIndices = this.workData.vertexJoinIndices,
				localNormals = this.vmesh.localNormals.GetNativeArray(),
				boneWeights = this.vmesh.boneWeights.GetNativeArray()
			}.Run(this.vmesh.VertexCount);
		}

		// Token: 0x060003F5 RID: 1013 RVA: 0x00029768 File Offset: 0x00027968
		protected static bool CheckJoin2(in NativeParallelMultiHashMap<ushort, ushort> vertexToVertexMap, int vindex, int tvindex, bool dontMakeLine)
		{
			FixedList512Bytes<ushort> fixedList512Bytes = default(FixedList512Bytes<ushort>);
			NativeParallelMultiHashMap<ushort, ushort> nativeParallelMultiHashMap = vertexToVertexMap;
			foreach (ushort num in nativeParallelMultiHashMap.GetValuesForKey((ushort)vindex))
			{
				if ((int)num != vindex && (int)num != tvindex)
				{
					(ref fixedList512Bytes).MC2Set(num);
				}
			}
			nativeParallelMultiHashMap = vertexToVertexMap;
			foreach (ushort num2 in nativeParallelMultiHashMap.GetValuesForKey((ushort)tvindex))
			{
				if ((int)num2 != vindex && (int)num2 != tvindex)
				{
					(ref fixedList512Bytes).MC2Set(num2);
				}
			}
			if (fixedList512Bytes.Length == 0)
			{
				return false;
			}
			if (dontMakeLine)
			{
				FixedList512Bytes<ushort> fixedList512Bytes2 = default(FixedList512Bytes<ushort>);
				(ref fixedList512Bytes2).MC2Push(fixedList512Bytes[0]);
				while (fixedList512Bytes2.Length > 0)
				{
					ushort num3 = (ref fixedList512Bytes2).MC2Pop<ushort>();
					if ((ref fixedList512Bytes).Contains(num3))
					{
						(ref fixedList512Bytes).MC2RemoveItemAtSwapBack(num3);
						nativeParallelMultiHashMap = vertexToVertexMap;
						foreach (ushort num4 in nativeParallelMultiHashMap.GetValuesForKey(num3))
						{
							if ((ref fixedList512Bytes).Contains(num4))
							{
								(ref fixedList512Bytes2).MC2Push(num4);
							}
						}
					}
				}
				if (fixedList512Bytes.Length > 0)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x060003F6 RID: 1014 RVA: 0x000298F4 File Offset: 0x00027AF4
		protected static bool CheckJoin(in NativeArray<FixedList128Bytes<ushort>> vertexToVertexArray, int vindex, int tvindex, in FixedList128Bytes<ushort> vlist, in FixedList128Bytes<ushort> tvlist, bool dontMakeLine)
		{
			FixedList128Bytes<ushort> fixedList128Bytes = default(FixedList128Bytes<ushort>);
			for (int i = 0; i < vlist.Length; i++)
			{
				int num = (int)vlist[i];
				if (num != vindex && num != tvindex)
				{
					(ref fixedList128Bytes).MC2SetLimit((ushort)num);
				}
			}
			for (int j = 0; j < tvlist.Length; j++)
			{
				int num2 = (int)tvlist[j];
				if (num2 != vindex && num2 != tvindex)
				{
					(ref fixedList128Bytes).MC2SetLimit((ushort)num2);
				}
			}
			if (fixedList128Bytes.Length == 0)
			{
				return false;
			}
			if (dontMakeLine)
			{
				FixedList512Bytes<ushort> fixedList512Bytes = default(FixedList512Bytes<ushort>);
				(ref fixedList512Bytes).MC2Push(fixedList128Bytes[0]);
				while (fixedList512Bytes.Length > 0)
				{
					ushort num3 = (ref fixedList512Bytes).MC2Pop<ushort>();
					if ((ref fixedList128Bytes).Contains(num3))
					{
						(ref fixedList128Bytes).MC2RemoveItemAtSwapBack(num3);
						NativeArray<FixedList128Bytes<ushort>> nativeArray = vertexToVertexArray;
						FixedList128Bytes<ushort> fixedList128Bytes2 = nativeArray[(int)num3];
						for (int k = 0; k < fixedList128Bytes2.Length; k++)
						{
							ushort num4 = fixedList128Bytes2[k];
							if ((ref fixedList128Bytes).Contains(num4))
							{
								(ref fixedList512Bytes).MC2Push(num4);
							}
						}
					}
				}
				if (fixedList128Bytes.Length > 0)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x04000829 RID: 2089
		protected string name = string.Empty;

		// Token: 0x0400082A RID: 2090
		protected VirtualMesh vmesh;

		// Token: 0x0400082B RID: 2091
		protected ReductionWorkData workData;

		// Token: 0x0400082C RID: 2092
		protected ResultCode result;

		// Token: 0x0400082D RID: 2093
		protected float startMergeLength;

		// Token: 0x0400082E RID: 2094
		protected float endMergeLength;

		// Token: 0x0400082F RID: 2095
		protected int maxStep;

		// Token: 0x04000830 RID: 2096
		protected bool dontMakeLine;

		// Token: 0x04000831 RID: 2097
		protected float joinPositionAdjustment;

		// Token: 0x04000832 RID: 2098
		protected int nowStepIndex;

		// Token: 0x04000833 RID: 2099
		protected float nowMergeLength;

		// Token: 0x04000834 RID: 2100
		protected float nowStepScale;

		// Token: 0x04000835 RID: 2101
		protected NativeList<StepReductionBase.JoinEdge> joinEdgeList;

		// Token: 0x04000836 RID: 2102
		private NativeParallelHashSet<int> completeVertexSet;

		// Token: 0x04000837 RID: 2103
		private NativeList<int2> removePairList;

		// Token: 0x04000838 RID: 2104
		private NativeArray<int> resultArray;

		// Token: 0x020000E4 RID: 228
		public struct JoinEdge : IComparable<StepReductionBase.JoinEdge>
		{
			// Token: 0x060003F7 RID: 1015 RVA: 0x00029A0C File Offset: 0x00027C0C
			public bool Contains(in int2 pair)
			{
				return this.vertexPair.x == pair.x || this.vertexPair.x == pair.y || this.vertexPair.y == pair.x || this.vertexPair.y == pair.y;
			}

			// Token: 0x060003F8 RID: 1016 RVA: 0x00029A68 File Offset: 0x00027C68
			public int CompareTo(StepReductionBase.JoinEdge other)
			{
				if (this.cost == other.cost)
				{
					return 0;
				}
				if (this.cost >= other.cost)
				{
					return 1;
				}
				return -1;
			}

			// Token: 0x04000839 RID: 2105
			public int2 vertexPair;

			// Token: 0x0400083A RID: 2106
			public float cost;
		}

		// Token: 0x020000E5 RID: 229
		[BurstCompile]
		private struct DeterminJoinEdgeJob : IJob
		{
			// Token: 0x060003F9 RID: 1017 RVA: 0x00029A8C File Offset: 0x00027C8C
			public void Execute()
			{
				this.completeVertexSet.Clear();
				this.removePairList.Clear();
				int num = 0;
				for (int i = 0; i < this.joinEdgeList.Length; i++)
				{
					int2 vertexPair = this.joinEdgeList[i].vertexPair;
					if (!this.completeVertexSet.Contains(vertexPair.x) && !this.completeVertexSet.Contains(vertexPair.y))
					{
						int2 xy = vertexPair.xy;
						this.removePairList.Add(in xy);
						this.completeVertexSet.Add(vertexPair.x);
						this.completeVertexSet.Add(vertexPair.y);
						num++;
					}
				}
				this.resultArray[this.stepIndex] = num;
			}

			// Token: 0x0400083B RID: 2107
			public int stepIndex;

			// Token: 0x0400083C RID: 2108
			public float mergeLength;

			// Token: 0x0400083D RID: 2109
			[ReadOnly]
			public NativeList<StepReductionBase.JoinEdge> joinEdgeList;

			// Token: 0x0400083E RID: 2110
			public NativeParallelHashSet<int> completeVertexSet;

			// Token: 0x0400083F RID: 2111
			public NativeList<int2> removePairList;

			// Token: 0x04000840 RID: 2112
			public NativeArray<int> resultArray;
		}

		// Token: 0x020000E6 RID: 230
		[BurstCompile]
		private struct JoinPairJob : IJob
		{
			// Token: 0x060003FA RID: 1018 RVA: 0x00029B54 File Offset: 0x00027D54
			public void Execute()
			{
				for (int i = 0; i < this.removePairList.Length; i++)
				{
					int2 @int = this.removePairList[i];
					int x = @int.x;
					int y = @int.y;
					float3 @float = this.localPositions[x];
					float3 float2 = this.localPositions[y];
					float3 float3 = this.localNormals[x];
					float3 float4 = this.localNormals[y];
					this.joinIndices[x] = y;
					float num = (float)math.max(this.vertexToVertexMap.CountValuesForKey((ushort)x) - 1, 1);
					float num2 = (float)math.max(this.vertexToVertexMap.CountValuesForKey((ushort)y) - 1, 1);
					float num3 = num2 / (num + num2);
					num3 = math.lerp(num3, 0.5f, this.joinPositionAdjustment);
					float3 float5 = math.lerp(float2, @float, num3);
					this.localPositions[y] = float5;
					this.localNormals[y] = float3 + float4;
					FixedList512Bytes<ushort> fixedList512Bytes = default(FixedList512Bytes<ushort>);
					foreach (ushort num4 in this.vertexToVertexMap.GetValuesForKey((ushort)x))
					{
						if ((int)num4 != x && (int)num4 != y)
						{
							(ref fixedList512Bytes).MC2Set(num4);
						}
					}
					foreach (ushort num5 in this.vertexToVertexMap.GetValuesForKey((ushort)y))
					{
						if ((int)num5 != x && (int)num5 != y)
						{
							(ref fixedList512Bytes).MC2Set(num5);
						}
					}
					this.vertexToVertexMap.Remove((ushort)y);
					for (int j = 0; j < fixedList512Bytes.Length; j++)
					{
						this.vertexToVertexMap.Add((ushort)y, fixedList512Bytes[j]);
					}
					VirtualMeshBoneWeight virtualMeshBoneWeight = this.boneWeights[y];
					VirtualMeshBoneWeight virtualMeshBoneWeight2 = this.boneWeights[x];
					virtualMeshBoneWeight.AddWeight(in virtualMeshBoneWeight2);
					this.boneWeights[y] = virtualMeshBoneWeight;
					VertexAttribute vertexAttribute = this.attributes[x];
					VertexAttribute vertexAttribute2 = this.attributes[y];
					this.attributes[y] = VertexAttribute.JoinAttribute(vertexAttribute, vertexAttribute2);
					this.attributes[x] = VertexAttribute.Invalid;
				}
			}

			// Token: 0x04000841 RID: 2113
			public float joinPositionAdjustment;

			// Token: 0x04000842 RID: 2114
			[ReadOnly]
			public NativeList<int2> removePairList;

			// Token: 0x04000843 RID: 2115
			public NativeArray<float3> localPositions;

			// Token: 0x04000844 RID: 2116
			public NativeArray<float3> localNormals;

			// Token: 0x04000845 RID: 2117
			public NativeParallelMultiHashMap<ushort, ushort> vertexToVertexMap;

			// Token: 0x04000846 RID: 2118
			public NativeArray<VirtualMeshBoneWeight> boneWeights;

			// Token: 0x04000847 RID: 2119
			public NativeArray<VertexAttribute> attributes;

			// Token: 0x04000848 RID: 2120
			public NativeArray<int> joinIndices;
		}

		// Token: 0x020000E7 RID: 231
		[BurstCompile]
		private struct UpdateJoinIndexJob : IJobParallelFor
		{
			// Token: 0x060003FB RID: 1019 RVA: 0x00029DC4 File Offset: 0x00027FC4
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

			// Token: 0x04000849 RID: 2121
			[NativeDisableParallelForRestriction]
			public NativeArray<int> joinIndices;
		}

		// Token: 0x020000E8 RID: 232
		[BurstCompile]
		private struct UpdateLinkIndexJob : IJobParallelFor
		{
			// Token: 0x060003FC RID: 1020 RVA: 0x00029E10 File Offset: 0x00028010
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

			// Token: 0x0400084A RID: 2122
			[NativeDisableParallelForRestriction]
			public NativeArray<int> joinIndices;

			// Token: 0x0400084B RID: 2123
			public NativeParallelMultiHashMap<ushort, ushort> vertexToVertexMap;
		}

		// Token: 0x020000E9 RID: 233
		[BurstCompile]
		private struct FinalMergeVertexJob : IJobParallelFor
		{
			// Token: 0x060003FD RID: 1021 RVA: 0x00029EDC File Offset: 0x000280DC
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

			// Token: 0x0400084C RID: 2124
			[ReadOnly]
			public NativeArray<int> joinIndices;

			// Token: 0x0400084D RID: 2125
			public NativeArray<float3> localNormals;

			// Token: 0x0400084E RID: 2126
			public NativeArray<VirtualMeshBoneWeight> boneWeights;
		}
	}
}
