using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Burst;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Jobs;
using UnityEngine;

namespace NGS.AdvancedCullingSystem.Static
{
	// Token: 0x02000016 RID: 22
	public class StaticCullingBaker : IDisposable
	{
		// Token: 0x06000087 RID: 135 RVA: 0x0000341C File Offset: 0x0000161C
		public StaticCullingBaker(GeometryTree geometryTree)
		{
			int num = 0;
			this._geometryTree = geometryTree;
			this._geometryTreeStruct = new NativeArray<StaticCullingBaker.GeometryNodeStruct>(this._geometryTree.NodesCount, Allocator.Persistent, NativeArrayOptions.ClearMemory);
			this.FillGeometryTreeStruct((GeometryTreeNode)this._geometryTree.Root, -1, false, ref num);
			this._cullingTargets = geometryTree.CullingTargets;
			this._cullingTargetsStruct = new NativeArray<StaticCullingBaker.CullingTargetStruct>(this._cullingTargets.Count, Allocator.Persistent, NativeArrayOptions.ClearMemory);
			this._cellTargetsMap = new NativeParallelHashMap<int, UnsafeList<int>>(this._geometryTreeStruct.Length, Allocator.Persistent);
			num = 0;
			Dictionary<CullingTarget, int> dictionary;
			this.FillTargetsIndexesDic(out dictionary);
			this.FillCellTargetsMap((GeometryTreeNode)this._geometryTree.Root, dictionary, ref num);
			this._startDepth = 7;
			this._maxDistance = this._geometryTree.Root.Bounds.size.magnitude;
			this._layerMask = LayerMask.GetMask(new string[] { StaticCullingPreferences.LayerName });
			this._processes = new List<StaticCullingBaker.VisibilityComputingProcess>();
		}

		// Token: 0x06000088 RID: 136 RVA: 0x00003520 File Offset: 0x00001720
		public bool Bake(VisibilityTree visibilityTree, float raysPerUnit, int maxRaysPerSource, out string error)
		{
			error = "";
			bool flag = false;
			this._raysPerUnit = raysPerUnit;
			this._maxRays = maxRaysPerSource;
			try
			{
				this._processes.Clear();
				List<VisibilityTreeNode> list = new List<VisibilityTreeNode>();
				this.FillVisibilityCells((VisibilityTreeNode)visibilityTree.Root, list);
				int num = 0;
				int i = 0;
				int count = list.Count;
				while (i < count)
				{
					while (this._processes.Count < 50 && num < count)
					{
						this._processes.Add(new StaticCullingBaker.VisibilityComputingProcess(list[num], this));
						num++;
					}
					int j = 0;
					while (j < this._processes.Count)
					{
						StaticCullingBaker.VisibilityComputingProcess visibilityComputingProcess = this._processes[j];
						bool flag2;
						visibilityComputingProcess.Update(out flag2);
						if (flag2)
						{
							visibilityComputingProcess.ApplyData();
							visibilityComputingProcess.Dispose();
							this._processes.RemoveAt(j);
							i++;
						}
						else
						{
							j++;
						}
					}
				}
				flag = i >= count;
			}
			catch (Exception ex)
			{
				error = ex.Message + "\n" + ex.StackTrace;
			}
			if (flag)
			{
				visibilityTree.SetTargets(this._geometryTree.CullingTargets.ToArray<CullingTarget>());
				visibilityTree.Optimize();
				visibilityTree.Apply();
			}
			else
			{
				foreach (StaticCullingBaker.VisibilityComputingProcess visibilityComputingProcess2 in this._processes)
				{
					visibilityComputingProcess2.Dispose();
				}
				this._processes.Clear();
			}
			return flag;
		}

		// Token: 0x06000089 RID: 137 RVA: 0x000036B4 File Offset: 0x000018B4
		public void Dispose()
		{
			foreach (StaticCullingBaker.VisibilityComputingProcess visibilityComputingProcess in this._processes)
			{
				visibilityComputingProcess.Dispose();
			}
			this._processes.Clear();
			if (this._geometryTreeStruct.IsCreated)
			{
				this._geometryTreeStruct.Dispose();
			}
			if (this._cullingTargetsStruct.IsCreated)
			{
				this._cullingTargetsStruct.Dispose();
			}
			if (this._cellTargetsMap.IsCreated)
			{
				foreach (KeyValue<int, UnsafeList<int>> keyValue in this._cellTargetsMap)
				{
					keyValue.Value.Dispose();
				}
				this._cellTargetsMap.Dispose();
			}
		}

		// Token: 0x0600008A RID: 138 RVA: 0x000037A0 File Offset: 0x000019A0
		private void FillGeometryTreeStruct(GeometryTreeNode current, int parentIndex, bool isLeft, ref int index)
		{
			StaticCullingBaker.GeometryNodeStruct geometryNodeStruct = new StaticCullingBaker.GeometryNodeStruct
			{
				index = index,
				bounds = current.Bounds,
				left = -1,
				right = -1,
				isEmpty = current.IsEmpty,
				isLeaf = current.IsLeaf
			};
			if (parentIndex >= 0)
			{
				StaticCullingBaker.GeometryNodeStruct geometryNodeStruct2 = this._geometryTreeStruct[parentIndex];
				if (isLeft)
				{
					geometryNodeStruct2.left = index;
				}
				else
				{
					geometryNodeStruct2.right = index;
				}
				this._geometryTreeStruct[parentIndex] = geometryNodeStruct2;
			}
			int num = index;
			index = num + 1;
			this._geometryTreeStruct[num] = geometryNodeStruct;
			if (current.HasChilds)
			{
				this.FillGeometryTreeStruct(current.Left, geometryNodeStruct.index, true, ref index);
				this.FillGeometryTreeStruct(current.Right, geometryNodeStruct.index, false, ref index);
			}
		}

		// Token: 0x0600008B RID: 139 RVA: 0x00003878 File Offset: 0x00001A78
		private void FillTargetsIndexesDic(out Dictionary<CullingTarget, int> targetsIndexes)
		{
			targetsIndexes = new Dictionary<CullingTarget, int>();
			for (int i = 0; i < this._cullingTargets.Count; i++)
			{
				this._cullingTargetsStruct[i] = new StaticCullingBaker.CullingTargetStruct
				{
					index = i,
					bounds = this._cullingTargets[i].Bounds
				};
				targetsIndexes.Add(this._cullingTargets[i], i);
			}
		}

		// Token: 0x0600008C RID: 140 RVA: 0x000038EC File Offset: 0x00001AEC
		private void FillCellTargetsMap(GeometryTreeNode current, Dictionary<CullingTarget, int> targetToIndexDic, ref int index)
		{
			int num = index;
			index++;
			if (current.IsLeaf)
			{
				UnsafeList<int> unsafeList;
				if (current.CullingTargets == null || current.CullingTargets.Count == 0)
				{
					unsafeList = new UnsafeList<int>(0, Allocator.Persistent, NativeArrayOptions.UninitializedMemory);
				}
				else
				{
					unsafeList = new UnsafeList<int>(current.CullingTargets.Count, Allocator.Persistent, NativeArrayOptions.UninitializedMemory);
					foreach (CullingTarget cullingTarget in current.CullingTargets)
					{
						int num2 = targetToIndexDic[cullingTarget];
						unsafeList.Add(in num2);
					}
				}
				this._cellTargetsMap.Add(num, unsafeList);
				return;
			}
			if (current.HasChilds)
			{
				this.FillCellTargetsMap(current.Left, targetToIndexDic, ref index);
				this.FillCellTargetsMap(current.Right, targetToIndexDic, ref index);
			}
		}

		// Token: 0x0600008D RID: 141 RVA: 0x000039CC File Offset: 0x00001BCC
		private void FillVisibilityCells(VisibilityTreeNode current, List<VisibilityTreeNode> result)
		{
			if (current.IsLeaf)
			{
				result.Add(current);
				return;
			}
			if (current.HasChilds)
			{
				this.FillVisibilityCells(current.Left, result);
				this.FillVisibilityCells(current.Right, result);
			}
		}

		// Token: 0x04000033 RID: 51
		private const int MAX_PROCESSES = 50;

		// Token: 0x04000034 RID: 52
		private const int COMMANDS_LIMIT = 100000;

		// Token: 0x04000035 RID: 53
		private GeometryTree _geometryTree;

		// Token: 0x04000036 RID: 54
		private IReadOnlyList<CullingTarget> _cullingTargets;

		// Token: 0x04000037 RID: 55
		private NativeArray<StaticCullingBaker.GeometryNodeStruct> _geometryTreeStruct;

		// Token: 0x04000038 RID: 56
		private NativeArray<StaticCullingBaker.CullingTargetStruct> _cullingTargetsStruct;

		// Token: 0x04000039 RID: 57
		private NativeParallelHashMap<int, UnsafeList<int>> _cellTargetsMap;

		// Token: 0x0400003A RID: 58
		private int _startDepth;

		// Token: 0x0400003B RID: 59
		private float _raysPerUnit;

		// Token: 0x0400003C RID: 60
		private int _maxRays;

		// Token: 0x0400003D RID: 61
		private float _maxDistance;

		// Token: 0x0400003E RID: 62
		private int _layerMask;

		// Token: 0x0400003F RID: 63
		private List<StaticCullingBaker.VisibilityComputingProcess> _processes;

		// Token: 0x02000017 RID: 23
		private class VisibilityComputingProcess : IDisposable
		{
			// Token: 0x0600008E RID: 142 RVA: 0x00003A00 File Offset: 0x00001C00
			public VisibilityComputingProcess(VisibilityTreeNode cell, StaticCullingBaker context)
			{
				this._cell = cell;
				this._cellPosition = this._cell.Center;
				this._cellSize = Mathf.Min(new float[]
				{
					cell.Size.x,
					cell.Size.y,
					cell.Size.z
				});
				this._geometryTreeStruct = context._geometryTreeStruct;
				this._cullingTargetsStruct = context._cullingTargetsStruct;
				this._nodeTargetsMap = context._cellTargetsMap;
				this._geometryTreeVisibility = new NativeArray<bool>(this._geometryTreeStruct.Length, Allocator.TempJob, NativeArrayOptions.ClearMemory);
				this._cullingTargetsVisibility = new NativeArray<bool>(this._cullingTargetsStruct.Length, Allocator.TempJob, NativeArrayOptions.ClearMemory);
				this._computedCullingTargets = new NativeArray<bool>(this._cullingTargetsStruct.Length, Allocator.TempJob, NativeArrayOptions.ClearMemory);
				this._raycastBatches = new NativeList<StaticCullingBaker.RaycastBatchInfo>(100000, Allocator.TempJob);
				this._commands = new NativeList<RaycastCommand>(100000 + context._maxRays, Allocator.TempJob);
				this._hits = new NativeList<RaycastHit>(100000 + context._maxRays, Allocator.TempJob);
				this._lastNodeIndex = new NativeArray<int>(1, Allocator.TempJob, NativeArrayOptions.ClearMemory);
				this._lastTargetIndex = new NativeArray<int>(1, Allocator.TempJob, NativeArrayOptions.ClearMemory);
				this._startDepth = context._startDepth;
				this._raysPerUnit = context._raysPerUnit;
				this._maxRays = context._maxRays;
				this._maxDistance = context._maxDistance;
				this._layerMask = context._layerMask;
				this._treeHeight = context._geometryTree.Height;
				this._depth = this._startDepth;
			}

			// Token: 0x0600008F RID: 143 RVA: 0x00003B98 File Offset: 0x00001D98
			public void Update(out bool finished)
			{
				finished = false;
				if (!this._handle.IsCompleted)
				{
					return;
				}
				this._handle.Complete();
				if (this._jobIndex == 0)
				{
					this.RunJob0();
					this._jobIndex = 1;
					return;
				}
				if (this._jobIndex == 1)
				{
					this.RunJob1();
					if (this._lastNodeIndex[0] != 0)
					{
						this._jobIndex = 0;
						return;
					}
					if (this._depth == this._treeHeight)
					{
						this._jobIndex = 2;
						return;
					}
					this._depth++;
					this._jobIndex = 0;
					return;
				}
				else
				{
					if (this._jobIndex == 2)
					{
						this.RunJob2();
						this._jobIndex = 3;
						return;
					}
					if (this._jobIndex != 3)
					{
						if (this._jobIndex == 4)
						{
							finished = true;
						}
						return;
					}
					this.RunJob3();
					if (this._lastNodeIndex[0] == 0 && this._lastTargetIndex[0] == 0)
					{
						this._jobIndex = 4;
						return;
					}
					this._jobIndex = 2;
					return;
				}
			}

			// Token: 0x06000090 RID: 144 RVA: 0x00003C88 File Offset: 0x00001E88
			public void ApplyData()
			{
				for (int i = 0; i < this._cullingTargetsVisibility.Length; i++)
				{
					if (this._cullingTargetsVisibility[i])
					{
						this._cell.AddVisibleCullingTarget(i);
					}
				}
			}

			// Token: 0x06000091 RID: 145 RVA: 0x00003CC8 File Offset: 0x00001EC8
			public void Dispose()
			{
				this._handle.Complete();
				this._geometryTreeVisibility.Dispose();
				this._cullingTargetsVisibility.Dispose();
				this._computedCullingTargets.Dispose();
				this._raycastBatches.Dispose();
				this._commands.Dispose();
				this._hits.Dispose();
				this._lastNodeIndex.Dispose();
				this._lastTargetIndex.Dispose();
			}

			// Token: 0x06000092 RID: 146 RVA: 0x00003D38 File Offset: 0x00001F38
			private void RunJob0()
			{
				this._handle = new StaticCullingBaker.TreeCreateRaysJob
				{
					cellPosition = this._cellPosition,
					cellSize = this._cellSize,
					geometryTreeStruct = this._geometryTreeStruct,
					geometryTreeVisibility = this._geometryTreeVisibility,
					rayBatches = this._raycastBatches,
					commands = this._commands,
					hits = this._hits,
					lastNodeIndex = this._lastNodeIndex,
					raysPerUnit = this._raysPerUnit,
					maxRays = this._maxRays,
					maxDistance = this._maxDistance,
					layerMask = this._layerMask,
					startDepth = this._startDepth,
					targetDepth = this._depth,
					commandsLimit = 100000
				}.Schedule(default(JobHandle));
			}

			// Token: 0x06000093 RID: 147 RVA: 0x00003E24 File Offset: 0x00002024
			private void RunJob1()
			{
				this._handle = new StaticCullingBaker.TreeComputeResultsJob
				{
					cellPosition = this._cellPosition,
					geometryTree = this._geometryTreeStruct,
					cullingTargetsStruct = this._cullingTargetsStruct,
					nodeTargetsMap = this._nodeTargetsMap,
					rayBatches = this._raycastBatches,
					commands = this._commands,
					hits = this._hits,
					geometryTreeVisibility = this._geometryTreeVisibility,
					cullingTargetsVisibility = this._cullingTargetsVisibility
				}.Schedule(RaycastCommand.ScheduleBatch(this._commands, this._hits, 1, default(JobHandle)));
			}

			// Token: 0x06000094 RID: 148 RVA: 0x00003EE0 File Offset: 0x000020E0
			private void RunJob2()
			{
				this._handle = new StaticCullingBaker.TargetsCreateRaysJob
				{
					cellPosition = this._cellPosition,
					cellSize = this._cellSize,
					geometryTreeStruct = this._geometryTreeStruct,
					cullingTargetsStruct = this._cullingTargetsStruct,
					geometryTreeVisibility = this._geometryTreeVisibility,
					cullingTargetsVisibility = this._cullingTargetsVisibility,
					computedCullingTargets = this._computedCullingTargets,
					nodeTargetsMap = this._nodeTargetsMap,
					rayBatches = this._raycastBatches,
					commands = this._commands,
					hits = this._hits,
					lastNodeIndex = this._lastNodeIndex,
					lastTargetIndex = this._lastTargetIndex,
					raysPerUnit = this._raysPerUnit,
					maxRays = this._maxRays,
					maxDistance = this._maxDistance,
					layerMask = this._layerMask,
					startDepth = this._startDepth,
					targetDepth = this._depth,
					commandsLimit = 100000
				}.Schedule(default(JobHandle));
			}

			// Token: 0x06000095 RID: 149 RVA: 0x00004010 File Offset: 0x00002210
			private void RunJob3()
			{
				this._handle = new StaticCullingBaker.TargetsComputeResultsJob
				{
					cellPosition = this._cellPosition,
					geometryTree = this._geometryTreeStruct,
					cullingTargetsStruct = this._cullingTargetsStruct,
					nodeTargetsMap = this._nodeTargetsMap,
					rayBatches = this._raycastBatches,
					commands = this._commands,
					hits = this._hits,
					cullingTargetsVisibility = this._cullingTargetsVisibility
				}.Schedule(RaycastCommand.ScheduleBatch(this._commands, this._hits, 1, default(JobHandle)));
			}

			// Token: 0x04000040 RID: 64
			private VisibilityTreeNode _cell;

			// Token: 0x04000041 RID: 65
			private Vector3 _cellPosition;

			// Token: 0x04000042 RID: 66
			private float _cellSize;

			// Token: 0x04000043 RID: 67
			private NativeArray<StaticCullingBaker.GeometryNodeStruct> _geometryTreeStruct;

			// Token: 0x04000044 RID: 68
			private NativeArray<StaticCullingBaker.CullingTargetStruct> _cullingTargetsStruct;

			// Token: 0x04000045 RID: 69
			private NativeParallelHashMap<int, UnsafeList<int>> _nodeTargetsMap;

			// Token: 0x04000046 RID: 70
			private NativeArray<bool> _geometryTreeVisibility;

			// Token: 0x04000047 RID: 71
			private NativeArray<bool> _cullingTargetsVisibility;

			// Token: 0x04000048 RID: 72
			private NativeArray<bool> _computedCullingTargets;

			// Token: 0x04000049 RID: 73
			private NativeList<StaticCullingBaker.RaycastBatchInfo> _raycastBatches;

			// Token: 0x0400004A RID: 74
			private NativeList<RaycastCommand> _commands;

			// Token: 0x0400004B RID: 75
			private NativeList<RaycastHit> _hits;

			// Token: 0x0400004C RID: 76
			private NativeArray<int> _lastNodeIndex;

			// Token: 0x0400004D RID: 77
			private NativeArray<int> _lastTargetIndex;

			// Token: 0x0400004E RID: 78
			private float _raysPerUnit;

			// Token: 0x0400004F RID: 79
			private int _maxRays;

			// Token: 0x04000050 RID: 80
			private float _maxDistance;

			// Token: 0x04000051 RID: 81
			private int _layerMask;

			// Token: 0x04000052 RID: 82
			private int _startDepth;

			// Token: 0x04000053 RID: 83
			private int _treeHeight;

			// Token: 0x04000054 RID: 84
			private int _depth;

			// Token: 0x04000055 RID: 85
			private JobHandle _handle;

			// Token: 0x04000056 RID: 86
			private int _jobIndex;
		}

		// Token: 0x02000018 RID: 24
		[BurstCompile]
		private struct GeometryNodeStruct
		{
			// Token: 0x04000057 RID: 87
			public int index;

			// Token: 0x04000058 RID: 88
			public int left;

			// Token: 0x04000059 RID: 89
			public int right;

			// Token: 0x0400005A RID: 90
			public Bounds bounds;

			// Token: 0x0400005B RID: 91
			public bool isLeaf;

			// Token: 0x0400005C RID: 92
			public bool isEmpty;
		}

		// Token: 0x02000019 RID: 25
		[BurstCompile]
		private struct CullingTargetStruct
		{
			// Token: 0x0400005D RID: 93
			public int index;

			// Token: 0x0400005E RID: 94
			public Bounds bounds;
		}

		// Token: 0x0200001A RID: 26
		[BurstCompile]
		private struct RaycastBatchInfo
		{
			// Token: 0x0400005F RID: 95
			public int targetIndex;

			// Token: 0x04000060 RID: 96
			public int raysStart;

			// Token: 0x04000061 RID: 97
			public int raysEnd;
		}

		// Token: 0x0200001B RID: 27
		[BurstCompile]
		private struct TreeCreateRaysJob : IJob
		{
			// Token: 0x06000096 RID: 150 RVA: 0x000040C0 File Offset: 0x000022C0
			public void Execute()
			{
				this.rayBatches.Clear();
				this.commands.Clear();
				this.hits.Clear();
				this.TraverseTree(this.geometryTreeStruct[0], 1);
				this.hits.Length = this._commandsCount;
				if (this._commandsCount <= this.commandsLimit)
				{
					this.lastNodeIndex[0] = 0;
				}
			}

			// Token: 0x06000097 RID: 151 RVA: 0x00004130 File Offset: 0x00002330
			private void TraverseTree(StaticCullingBaker.GeometryNodeStruct node, int depth)
			{
				if (node.isEmpty)
				{
					return;
				}
				if (this._commandsCount > this.commandsLimit)
				{
					return;
				}
				if (depth == this.targetDepth)
				{
					if (node.index > this.lastNodeIndex[0] && !this.geometryTreeVisibility[node.index])
					{
						this.CreateRaysBatch(node);
						if (this._commandsCount > this.commandsLimit)
						{
							this.lastNodeIndex[0] = node.index;
						}
					}
					return;
				}
				if (!this.geometryTreeVisibility[node.index] && depth >= this.startDepth)
				{
					return;
				}
				if (node.left < 0)
				{
					return;
				}
				this.TraverseTree(this.geometryTreeStruct[node.left], depth + 1);
				this.TraverseTree(this.geometryTreeStruct[node.right], depth + 1);
			}

			// Token: 0x06000098 RID: 152 RVA: 0x00004208 File Offset: 0x00002408
			private void CreateRaysBatch(StaticCullingBaker.GeometryNodeStruct node)
			{
				Bounds bounds = node.bounds;
				if (bounds.Contains(this.cellPosition))
				{
					this.rayBatches.AddNoResize(new StaticCullingBaker.RaycastBatchInfo
					{
						targetIndex = node.index,
						raysStart = -1,
						raysEnd = -1
					});
					return;
				}
				float num = Mathf.Max(Vector3.Distance(bounds.center, this.cellPosition) / this.maxDistance, 0.01f);
				int num2 = Mathf.RoundToInt(5f + bounds.size.magnitude * this.raysPerUnit * num);
				num2 = Mathf.Min(num2, this.maxRays);
				this.rayBatches.AddNoResize(new StaticCullingBaker.RaycastBatchInfo
				{
					targetIndex = node.index,
					raysStart = this._commandsCount,
					raysEnd = this._commandsCount + num2
				});
				this._commandsCount += num2;
				for (int i = 0; i < num2; i++)
				{
					Vector3 normalized = (this.GetPointInsideBoundingBox(i, bounds) - this.cellPosition).normalized;
					RaycastCommand raycastCommand = UnityAPI.NewRaycastCommand(this.cellPosition, normalized, 5000f, this.layerMask);
					this.commands.AddNoResize(raycastCommand);
				}
			}

			// Token: 0x06000099 RID: 153 RVA: 0x00004354 File Offset: 0x00002554
			private Vector3 GetPointInsideBoundingBox(int index, Bounds bounds)
			{
				Vector3 size = bounds.size;
				float num = (float)(0.5 + StaticCullingBaker.TreeCreateRaysJob.a1 * (double)index) % 1f;
				float num2 = (float)(0.5 + StaticCullingBaker.TreeCreateRaysJob.a2 * (double)index) % 1f;
				float num3 = (float)(0.5 + StaticCullingBaker.TreeCreateRaysJob.a3 * (double)index) % 1f;
				Vector3 vector = new Vector3(num * size.x, num2 * size.y, num3 * size.z);
				return bounds.min + vector;
			}

			// Token: 0x04000062 RID: 98
			private static readonly double g = 1.2207440846057596;

			// Token: 0x04000063 RID: 99
			private static readonly double a1 = 1.0 / StaticCullingBaker.TreeCreateRaysJob.g;

			// Token: 0x04000064 RID: 100
			private static readonly double a2 = 1.0 / (StaticCullingBaker.TreeCreateRaysJob.g * StaticCullingBaker.TreeCreateRaysJob.g);

			// Token: 0x04000065 RID: 101
			private static readonly double a3 = 1.0 / (StaticCullingBaker.TreeCreateRaysJob.g * StaticCullingBaker.TreeCreateRaysJob.g * StaticCullingBaker.TreeCreateRaysJob.g);

			// Token: 0x04000066 RID: 102
			[ReadOnly]
			public NativeArray<StaticCullingBaker.GeometryNodeStruct> geometryTreeStruct;

			// Token: 0x04000067 RID: 103
			[ReadOnly]
			public NativeArray<bool> geometryTreeVisibility;

			// Token: 0x04000068 RID: 104
			[WriteOnly]
			public NativeList<RaycastCommand> commands;

			// Token: 0x04000069 RID: 105
			[WriteOnly]
			public NativeList<RaycastHit> hits;

			// Token: 0x0400006A RID: 106
			[WriteOnly]
			public NativeList<StaticCullingBaker.RaycastBatchInfo> rayBatches;

			// Token: 0x0400006B RID: 107
			public NativeArray<int> lastNodeIndex;

			// Token: 0x0400006C RID: 108
			public Vector3 cellPosition;

			// Token: 0x0400006D RID: 109
			public float cellSize;

			// Token: 0x0400006E RID: 110
			public float raysPerUnit;

			// Token: 0x0400006F RID: 111
			public int maxRays;

			// Token: 0x04000070 RID: 112
			public float maxDistance;

			// Token: 0x04000071 RID: 113
			public int layerMask;

			// Token: 0x04000072 RID: 114
			public int startDepth;

			// Token: 0x04000073 RID: 115
			public int targetDepth;

			// Token: 0x04000074 RID: 116
			public int commandsLimit;

			// Token: 0x04000075 RID: 117
			private int _commandsCount;
		}

		// Token: 0x0200001C RID: 28
		[BurstCompile]
		private struct TreeComputeResultsJob : IJob
		{
			// Token: 0x0600009B RID: 155 RVA: 0x00004450 File Offset: 0x00002650
			public void Execute()
			{
				for (int i = 0; i < this.rayBatches.Length; i++)
				{
					StaticCullingBaker.RaycastBatchInfo raycastBatchInfo = this.rayBatches[i];
					StaticCullingBaker.GeometryNodeStruct geometryNodeStruct = this.geometryTree[raycastBatchInfo.targetIndex];
					if (raycastBatchInfo.raysStart < 0)
					{
						this.geometryTreeVisibility[geometryNodeStruct.index] = true;
					}
					else
					{
						int raysStart = raycastBatchInfo.raysStart;
						int raysEnd = raycastBatchInfo.raysEnd;
						int num = 0;
						int num2 = 0;
						for (int j = raysStart; j < raysEnd; j++)
						{
							RaycastHit raycastHit = this.hits[j];
							RaycastCommand raycastCommand = this.commands[j];
							Ray ray = new Ray(raycastCommand.from, raycastCommand.direction);
							float num3 = 0f;
							float num4 = raycastHit.distance;
							geometryNodeStruct.bounds.IntersectRay(ray, out num3);
							if (num4 < 0.0001f)
							{
								num4 = float.MaxValue;
							}
							else if (num2 < 5)
							{
								this.TracePoint(this.geometryTree[0], raycastHit.point);
								num2++;
							}
							if (num4 > num3)
							{
								this.TraceRay(this.geometryTree[0], ray, num4);
								num++;
								if (num >= 5)
								{
									break;
								}
							}
						}
					}
				}
			}

			// Token: 0x0600009C RID: 156 RVA: 0x00004594 File Offset: 0x00002794
			private void TraceRay(StaticCullingBaker.GeometryNodeStruct node, Ray ray, float hitDistance)
			{
				if (node.isEmpty)
				{
					return;
				}
				float num;
				if (node.bounds.IntersectRay(ray, out num) && num < hitDistance)
				{
					this.geometryTreeVisibility[node.index] = true;
					if (node.left != -1)
					{
						this.TraceRay(this.geometryTree[node.left], ray, hitDistance);
						this.TraceRay(this.geometryTree[node.right], ray, hitDistance);
					}
				}
			}

			// Token: 0x0600009D RID: 157 RVA: 0x0000460C File Offset: 0x0000280C
			private void TracePoint(StaticCullingBaker.GeometryNodeStruct node, Vector3 hitPoint)
			{
				if (node.isEmpty)
				{
					return;
				}
				if (!node.bounds.Contains(hitPoint))
				{
					return;
				}
				if (node.isLeaf)
				{
					UnsafeList<int> unsafeList = this.nodeTargetsMap[node.index];
					for (int i = 0; i < unsafeList.Length; i++)
					{
						int num = unsafeList[i];
						if (!this.cullingTargetsVisibility[num] && this.cullingTargetsStruct[num].bounds.Contains(hitPoint))
						{
							this.cullingTargetsVisibility[num] = true;
						}
					}
					return;
				}
				if (node.left >= 0)
				{
					this.TracePoint(this.geometryTree[node.left], hitPoint);
					this.TracePoint(this.geometryTree[node.right], hitPoint);
				}
			}

			// Token: 0x04000076 RID: 118
			[ReadOnly]
			public Vector3 cellPosition;

			// Token: 0x04000077 RID: 119
			[ReadOnly]
			public NativeArray<StaticCullingBaker.GeometryNodeStruct> geometryTree;

			// Token: 0x04000078 RID: 120
			[ReadOnly]
			public NativeArray<StaticCullingBaker.CullingTargetStruct> cullingTargetsStruct;

			// Token: 0x04000079 RID: 121
			[ReadOnly]
			public NativeParallelHashMap<int, UnsafeList<int>> nodeTargetsMap;

			// Token: 0x0400007A RID: 122
			[ReadOnly]
			public NativeList<StaticCullingBaker.RaycastBatchInfo> rayBatches;

			// Token: 0x0400007B RID: 123
			[ReadOnly]
			public NativeList<RaycastCommand> commands;

			// Token: 0x0400007C RID: 124
			[ReadOnly]
			public NativeList<RaycastHit> hits;

			// Token: 0x0400007D RID: 125
			public NativeArray<bool> geometryTreeVisibility;

			// Token: 0x0400007E RID: 126
			public NativeArray<bool> cullingTargetsVisibility;
		}

		// Token: 0x0200001D RID: 29
		[BurstCompile]
		private struct TargetsCreateRaysJob : IJob
		{
			// Token: 0x0600009E RID: 158 RVA: 0x000046D8 File Offset: 0x000028D8
			public void Execute()
			{
				this.rayBatches.Clear();
				this.commands.Clear();
				this.hits.Clear();
				this.TraverseTree(this.geometryTreeStruct[0], 1);
				this.hits.Length = this._commandsCount;
				if (this._commandsCount <= this.commandsLimit)
				{
					this.lastNodeIndex[0] = 0;
					this.lastTargetIndex[0] = 0;
				}
			}

			// Token: 0x0600009F RID: 159 RVA: 0x00004754 File Offset: 0x00002954
			private void TraverseTree(StaticCullingBaker.GeometryNodeStruct node, int depth)
			{
				if (node.isEmpty)
				{
					return;
				}
				if (this._commandsCount > this.commandsLimit)
				{
					return;
				}
				if (!this.geometryTreeVisibility[node.index] && depth >= this.startDepth)
				{
					return;
				}
				if (node.isLeaf && node.index >= this.lastNodeIndex[0])
				{
					UnsafeList<int> unsafeList = this.nodeTargetsMap[node.index];
					int num = this.lastTargetIndex[0];
					for (int i = 0; i < unsafeList.Length; i++)
					{
						if (num == 0 || i > num)
						{
							int num2 = unsafeList[i];
							if (!this.computedCullingTargets[num2])
							{
								if (this.cullingTargetsVisibility[num2])
								{
									this.computedCullingTargets[num2] = true;
								}
								else
								{
									this.CreateRaysBatch(this.cullingTargetsStruct[num2]);
									this.computedCullingTargets[num2] = true;
									if (this._commandsCount > this.commandsLimit)
									{
										this.lastNodeIndex[0] = node.index;
										this.lastTargetIndex[0] = i;
										break;
									}
								}
							}
						}
					}
					if (this._commandsCount <= this.commandsLimit && this.lastNodeIndex[0] == node.index)
					{
						this.lastTargetIndex[0] = 0;
					}
					return;
				}
				if (node.left < 0)
				{
					return;
				}
				this.TraverseTree(this.geometryTreeStruct[node.left], depth + 1);
				this.TraverseTree(this.geometryTreeStruct[node.right], depth + 1);
			}

			// Token: 0x060000A0 RID: 160 RVA: 0x000048EC File Offset: 0x00002AEC
			private void CreateRaysBatch(StaticCullingBaker.CullingTargetStruct target)
			{
				Bounds bounds = target.bounds;
				if (bounds.Contains(this.cellPosition))
				{
					this.rayBatches.AddNoResize(new StaticCullingBaker.RaycastBatchInfo
					{
						targetIndex = target.index,
						raysStart = -1,
						raysEnd = -1
					});
					return;
				}
				float num = Mathf.Max(Vector3.Distance(bounds.center, this.cellPosition) / this.maxDistance, 0.01f);
				int num2 = Mathf.RoundToInt(5f + bounds.size.magnitude * this.raysPerUnit * num);
				num2 = Mathf.Min(num2, this.maxRays);
				this.rayBatches.AddNoResize(new StaticCullingBaker.RaycastBatchInfo
				{
					targetIndex = target.index,
					raysStart = this._commandsCount,
					raysEnd = this._commandsCount + num2
				});
				this._commandsCount += num2;
				for (int i = 0; i < num2; i++)
				{
					Vector3 normalized = (this.GetPointInsideBoundingBox(i, bounds) - this.cellPosition).normalized;
					RaycastCommand raycastCommand = UnityAPI.NewRaycastCommand(this.cellPosition, normalized, 5000f, this.layerMask);
					this.commands.AddNoResize(raycastCommand);
				}
			}

			// Token: 0x060000A1 RID: 161 RVA: 0x00004A38 File Offset: 0x00002C38
			private Vector3 GetPointInsideBoundingBox(int index, Bounds bounds)
			{
				Vector3 size = bounds.size;
				float num = (float)(0.5 + StaticCullingBaker.TargetsCreateRaysJob.a1 * (double)index) % 1f;
				float num2 = (float)(0.5 + StaticCullingBaker.TargetsCreateRaysJob.a2 * (double)index) % 1f;
				float num3 = (float)(0.5 + StaticCullingBaker.TargetsCreateRaysJob.a3 * (double)index) % 1f;
				Vector3 vector = new Vector3(num * size.x, num2 * size.y, num3 * size.z);
				return bounds.min + vector;
			}

			// Token: 0x0400007F RID: 127
			private static readonly double g = 1.2207440846057596;

			// Token: 0x04000080 RID: 128
			private static readonly double a1 = 1.0 / StaticCullingBaker.TargetsCreateRaysJob.g;

			// Token: 0x04000081 RID: 129
			private static readonly double a2 = 1.0 / (StaticCullingBaker.TargetsCreateRaysJob.g * StaticCullingBaker.TargetsCreateRaysJob.g);

			// Token: 0x04000082 RID: 130
			private static readonly double a3 = 1.0 / (StaticCullingBaker.TargetsCreateRaysJob.g * StaticCullingBaker.TargetsCreateRaysJob.g * StaticCullingBaker.TargetsCreateRaysJob.g);

			// Token: 0x04000083 RID: 131
			[ReadOnly]
			public Vector3 cellPosition;

			// Token: 0x04000084 RID: 132
			[ReadOnly]
			public float cellSize;

			// Token: 0x04000085 RID: 133
			[ReadOnly]
			public NativeArray<StaticCullingBaker.GeometryNodeStruct> geometryTreeStruct;

			// Token: 0x04000086 RID: 134
			[ReadOnly]
			public NativeArray<StaticCullingBaker.CullingTargetStruct> cullingTargetsStruct;

			// Token: 0x04000087 RID: 135
			[ReadOnly]
			public NativeParallelHashMap<int, UnsafeList<int>> nodeTargetsMap;

			// Token: 0x04000088 RID: 136
			[ReadOnly]
			public NativeArray<bool> geometryTreeVisibility;

			// Token: 0x04000089 RID: 137
			[ReadOnly]
			public NativeArray<bool> cullingTargetsVisibility;

			// Token: 0x0400008A RID: 138
			public NativeArray<bool> computedCullingTargets;

			// Token: 0x0400008B RID: 139
			[WriteOnly]
			public NativeList<StaticCullingBaker.RaycastBatchInfo> rayBatches;

			// Token: 0x0400008C RID: 140
			[WriteOnly]
			public NativeList<RaycastCommand> commands;

			// Token: 0x0400008D RID: 141
			[WriteOnly]
			public NativeList<RaycastHit> hits;

			// Token: 0x0400008E RID: 142
			public NativeArray<int> lastNodeIndex;

			// Token: 0x0400008F RID: 143
			public NativeArray<int> lastTargetIndex;

			// Token: 0x04000090 RID: 144
			public float raysPerUnit;

			// Token: 0x04000091 RID: 145
			public int maxRays;

			// Token: 0x04000092 RID: 146
			public float maxDistance;

			// Token: 0x04000093 RID: 147
			public int layerMask;

			// Token: 0x04000094 RID: 148
			public int startDepth;

			// Token: 0x04000095 RID: 149
			public int targetDepth;

			// Token: 0x04000096 RID: 150
			public int commandsLimit;

			// Token: 0x04000097 RID: 151
			private int _commandsCount;
		}

		// Token: 0x0200001E RID: 30
		[BurstCompile]
		private struct TargetsComputeResultsJob : IJob
		{
			// Token: 0x060000A3 RID: 163 RVA: 0x00004B34 File Offset: 0x00002D34
			public void Execute()
			{
				for (int i = 0; i < this.rayBatches.Length; i++)
				{
					StaticCullingBaker.RaycastBatchInfo raycastBatchInfo = this.rayBatches[i];
					StaticCullingBaker.CullingTargetStruct cullingTargetStruct = this.cullingTargetsStruct[raycastBatchInfo.targetIndex];
					if (raycastBatchInfo.raysStart < 0)
					{
						this.cullingTargetsVisibility[cullingTargetStruct.index] = true;
					}
					else
					{
						int raysStart = raycastBatchInfo.raysStart;
						int raysEnd = raycastBatchInfo.raysEnd;
						int num = 0;
						for (int j = raysStart; j < raysEnd; j++)
						{
							RaycastHit raycastHit = this.hits[j];
							RaycastCommand raycastCommand = this.commands[j];
							Ray ray = new Ray(raycastCommand.from, raycastCommand.direction);
							float num2 = 0f;
							float num3 = raycastHit.distance;
							cullingTargetStruct.bounds.IntersectRay(ray, out num2);
							num2 -= 0.01f;
							if (num3 < 0.0001f)
							{
								num3 = float.MaxValue;
							}
							else if (num < 2)
							{
								this.TracePoint(this.geometryTree[0], raycastHit.point);
								num++;
							}
							if (num3 > num2)
							{
								this.cullingTargetsVisibility[cullingTargetStruct.index] = true;
								break;
							}
						}
					}
				}
			}

			// Token: 0x060000A4 RID: 164 RVA: 0x00004C70 File Offset: 0x00002E70
			private void TracePoint(StaticCullingBaker.GeometryNodeStruct node, Vector3 hitPoint)
			{
				if (node.isEmpty)
				{
					return;
				}
				if (!node.bounds.Contains(hitPoint))
				{
					return;
				}
				if (node.isLeaf)
				{
					UnsafeList<int> unsafeList = this.nodeTargetsMap[node.index];
					for (int i = 0; i < unsafeList.Length; i++)
					{
						int num = unsafeList[i];
						if (!this.cullingTargetsVisibility[num] && this.cullingTargetsStruct[num].bounds.Contains(hitPoint))
						{
							this.cullingTargetsVisibility[num] = true;
						}
					}
					return;
				}
				if (node.left >= 0)
				{
					this.TracePoint(this.geometryTree[node.left], hitPoint);
					this.TracePoint(this.geometryTree[node.right], hitPoint);
				}
			}

			// Token: 0x04000098 RID: 152
			[ReadOnly]
			public Vector3 cellPosition;

			// Token: 0x04000099 RID: 153
			[ReadOnly]
			public NativeArray<StaticCullingBaker.GeometryNodeStruct> geometryTree;

			// Token: 0x0400009A RID: 154
			[ReadOnly]
			public NativeArray<StaticCullingBaker.CullingTargetStruct> cullingTargetsStruct;

			// Token: 0x0400009B RID: 155
			[ReadOnly]
			public NativeParallelHashMap<int, UnsafeList<int>> nodeTargetsMap;

			// Token: 0x0400009C RID: 156
			[ReadOnly]
			public NativeList<StaticCullingBaker.RaycastBatchInfo> rayBatches;

			// Token: 0x0400009D RID: 157
			[ReadOnly]
			public NativeList<RaycastCommand> commands;

			// Token: 0x0400009E RID: 158
			[ReadOnly]
			public NativeList<RaycastHit> hits;

			// Token: 0x0400009F RID: 159
			public NativeArray<bool> cullingTargetsVisibility;
		}
	}
}
