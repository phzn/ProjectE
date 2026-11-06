using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Pathfinding.Collections;
using Pathfinding.Graphs.Grid.Jobs;
using Pathfinding.Jobs;
using Unity.Collections;
using Unity.Jobs;
using Unity.Jobs.LowLevel.Unsafe;
using Unity.Mathematics;
using UnityEngine;

namespace Pathfinding.Graphs.Grid
{
	// Token: 0x02000207 RID: 519
	public struct GridGraphNodeData
	{
		// Token: 0x17000210 RID: 528
		// (get) Token: 0x06000DC6 RID: 3526 RVA: 0x00053EC8 File Offset: 0x000520C8
		public int layers
		{
			get
			{
				return this.bounds.size.y;
			}
		}

		// Token: 0x06000DC7 RID: 3527 RVA: 0x00053EDC File Offset: 0x000520DC
		public void AllocateBuffers(JobDependencyTracker dependencyTracker)
		{
			if (dependencyTracker != null)
			{
				this.positions = dependencyTracker.NewNativeArray<Vector3>(this.numNodes, this.allocationMethod, NativeArrayOptions.UninitializedMemory);
				this.normals = dependencyTracker.NewNativeArray<float4>(this.numNodes, this.allocationMethod, NativeArrayOptions.UninitializedMemory);
				this.connections = dependencyTracker.NewNativeArray<ulong>(this.numNodes, this.allocationMethod, NativeArrayOptions.UninitializedMemory);
				this.penalties = dependencyTracker.NewNativeArray<uint>(this.numNodes, this.allocationMethod, NativeArrayOptions.UninitializedMemory);
				this.walkable = dependencyTracker.NewNativeArray<bool>(this.numNodes, this.allocationMethod, NativeArrayOptions.UninitializedMemory);
				this.walkableWithErosion = dependencyTracker.NewNativeArray<bool>(this.numNodes, this.allocationMethod, NativeArrayOptions.UninitializedMemory);
				this.tags = dependencyTracker.NewNativeArray<int>(this.numNodes, this.allocationMethod, NativeArrayOptions.ClearMemory);
				return;
			}
			this.positions = new NativeArray<Vector3>(this.numNodes, this.allocationMethod, NativeArrayOptions.UninitializedMemory);
			this.normals = new NativeArray<float4>(this.numNodes, this.allocationMethod, NativeArrayOptions.UninitializedMemory);
			this.connections = new NativeArray<ulong>(this.numNodes, this.allocationMethod, NativeArrayOptions.UninitializedMemory);
			this.penalties = new NativeArray<uint>(this.numNodes, this.allocationMethod, NativeArrayOptions.UninitializedMemory);
			this.walkable = new NativeArray<bool>(this.numNodes, this.allocationMethod, NativeArrayOptions.UninitializedMemory);
			this.walkableWithErosion = new NativeArray<bool>(this.numNodes, this.allocationMethod, NativeArrayOptions.UninitializedMemory);
			this.tags = new NativeArray<int>(this.numNodes, this.allocationMethod, NativeArrayOptions.ClearMemory);
		}

		// Token: 0x06000DC8 RID: 3528 RVA: 0x00054048 File Offset: 0x00052248
		public void TrackBuffers(JobDependencyTracker dependencyTracker)
		{
			if (this.positions.IsCreated)
			{
				dependencyTracker.Track<Vector3>(this.positions, true);
			}
			if (this.normals.IsCreated)
			{
				dependencyTracker.Track<float4>(this.normals, true);
			}
			if (this.connections.IsCreated)
			{
				dependencyTracker.Track<ulong>(this.connections, true);
			}
			if (this.penalties.IsCreated)
			{
				dependencyTracker.Track<uint>(this.penalties, true);
			}
			if (this.walkable.IsCreated)
			{
				dependencyTracker.Track<bool>(this.walkable, true);
			}
			if (this.walkableWithErosion.IsCreated)
			{
				dependencyTracker.Track<bool>(this.walkableWithErosion, true);
			}
			if (this.tags.IsCreated)
			{
				dependencyTracker.Track<int>(this.tags, true);
			}
		}

		// Token: 0x06000DC9 RID: 3529 RVA: 0x0005410C File Offset: 0x0005230C
		public void PersistBuffers(JobDependencyTracker dependencyTracker)
		{
			dependencyTracker.Persist<Vector3>(this.positions);
			dependencyTracker.Persist<float4>(this.normals);
			dependencyTracker.Persist<ulong>(this.connections);
			dependencyTracker.Persist<uint>(this.penalties);
			dependencyTracker.Persist<bool>(this.walkable);
			dependencyTracker.Persist<bool>(this.walkableWithErosion);
			dependencyTracker.Persist<int>(this.tags);
		}

		// Token: 0x06000DCA RID: 3530 RVA: 0x00054170 File Offset: 0x00052370
		public void Dispose()
		{
			this.bounds = default(IntBounds);
			this.numNodes = 0;
			if (this.positions.IsCreated)
			{
				this.positions.Dispose();
			}
			if (this.normals.IsCreated)
			{
				this.normals.Dispose();
			}
			if (this.connections.IsCreated)
			{
				this.connections.Dispose();
			}
			if (this.penalties.IsCreated)
			{
				this.penalties.Dispose();
			}
			if (this.walkable.IsCreated)
			{
				this.walkable.Dispose();
			}
			if (this.walkableWithErosion.IsCreated)
			{
				this.walkableWithErosion.Dispose();
			}
			if (this.tags.IsCreated)
			{
				this.tags.Dispose();
			}
		}

		// Token: 0x06000DCB RID: 3531 RVA: 0x00054238 File Offset: 0x00052438
		public unsafe JobHandle Rotate2D(int dx, int dz, JobHandle dependency)
		{
			int3 size = this.bounds.size;
			IntPtr intPtr;
			checked
			{
				intPtr = stackalloc byte[unchecked((UIntPtr)7) * (UIntPtr)sizeof(JobHandle)];
				*intPtr = this.positions.Rotate3D<Vector3>(size, dx, dz).Schedule(dependency);
			}
			*(intPtr + (IntPtr)sizeof(JobHandle)) = this.normals.Rotate3D<float4>(size, dx, dz).Schedule(dependency);
			*(intPtr + (IntPtr)2 * (IntPtr)sizeof(JobHandle)) = this.connections.Rotate3D<ulong>(size, dx, dz).Schedule(dependency);
			*(intPtr + (IntPtr)3 * (IntPtr)sizeof(JobHandle)) = this.penalties.Rotate3D<uint>(size, dx, dz).Schedule(dependency);
			*(intPtr + (IntPtr)4 * (IntPtr)sizeof(JobHandle)) = this.walkable.Rotate3D<bool>(size, dx, dz).Schedule(dependency);
			*(intPtr + (IntPtr)5 * (IntPtr)sizeof(JobHandle)) = this.walkableWithErosion.Rotate3D<bool>(size, dx, dz).Schedule(dependency);
			*(intPtr + (IntPtr)6 * (IntPtr)sizeof(JobHandle)) = this.tags.Rotate3D<int>(size, dx, dz).Schedule(dependency);
			return JobHandleUnsafeUtility.CombineDependencies(intPtr, 7);
		}

		// Token: 0x06000DCC RID: 3532 RVA: 0x00054354 File Offset: 0x00052554
		public void ResizeLayerCount(int layerCount, JobDependencyTracker dependencyTracker)
		{
			if (layerCount > this.layers)
			{
				GridGraphNodeData gridGraphNodeData = this;
				this.bounds.max.y = layerCount;
				this.numNodes = this.bounds.volume;
				this.AllocateBuffers(dependencyTracker);
				this.normals.MemSet(float4.zero).Schedule(dependencyTracker);
				this.walkable.MemSet(false).Schedule(dependencyTracker);
				this.walkableWithErosion.MemSet(false).Schedule(dependencyTracker);
				new JobCopyBuffers
				{
					input = gridGraphNodeData,
					output = this,
					copyPenaltyAndTags = true,
					bounds = gridGraphNodeData.bounds
				}.Schedule(dependencyTracker);
			}
			if (layerCount < this.layers)
			{
				throw new ArgumentException("Cannot reduce the number of layers");
			}
		}

		// Token: 0x06000DCD RID: 3533 RVA: 0x00054428 File Offset: 0x00052628
		public void ReadFromNodesForConnectionCalculations(GridNodeBase[] nodes, Slice3D slice, JobHandle nodesDependsOn, NativeArray<float4> graphNodeNormals, JobDependencyTracker dependencyTracker)
		{
			this.bounds = slice.slice;
			this.numNodes = slice.slice.volume;
			this.positions = new NativeArray<Vector3>(this.numNodes, this.allocationMethod, NativeArrayOptions.UninitializedMemory);
			this.normals = new NativeArray<float4>(this.numNodes, this.allocationMethod, NativeArrayOptions.UninitializedMemory);
			this.connections = new NativeArray<ulong>(this.numNodes, this.allocationMethod, NativeArrayOptions.UninitializedMemory);
			this.walkableWithErosion = new NativeArray<bool>(this.numNodes, this.allocationMethod, NativeArrayOptions.UninitializedMemory);
			GridGraphNodeData.LightReader lightReader = new GridGraphNodeData.LightReader
			{
				nodes = nodes,
				nodePositions = this.positions.AsUnsafeSpan<Vector3>(),
				nodeWalkable = this.walkableWithErosion.AsUnsafeSpan<bool>()
			};
			GridIterationUtilities.ForEachCellIn3DSlice<GridGraphNodeData.LightReader>(slice, ref lightReader);
			this.ReadNodeNormals(slice, graphNodeNormals, dependencyTracker);
		}

		// Token: 0x06000DCE RID: 3534 RVA: 0x00054500 File Offset: 0x00052700
		private void ReadNodeNormals(Slice3D slice, NativeArray<float4> graphNodeNormals, JobDependencyTracker dependencyTracker)
		{
			if (dependencyTracker != null)
			{
				this.normals.MemSet(float4.zero).Schedule(dependencyTracker);
				new JobCopyRectangle<float4>
				{
					input = graphNodeNormals,
					output = this.normals,
					inputSlice = slice,
					outputSlice = new Slice3D(this.bounds, slice.slice)
				}.Schedule(dependencyTracker);
				return;
			}
			this.normals.AsUnsafeSpan<float4>().FillZeros<float4>();
			JobCopyRectangle<float4>.Copy(graphNodeNormals, this.normals, slice, new Slice3D(this.bounds, slice.slice));
		}

		// Token: 0x06000DCF RID: 3535 RVA: 0x0005459C File Offset: 0x0005279C
		public static GridGraphNodeData ReadFromNodes(GridNodeBase[] nodes, Slice3D slice, JobHandle nodesDependsOn, NativeArray<float4> graphNodeNormals, Allocator allocator, bool layeredDataLayout, JobDependencyTracker dependencyTracker)
		{
			GridGraphNodeData gridGraphNodeData = new GridGraphNodeData
			{
				allocationMethod = allocator,
				numNodes = slice.slice.volume,
				bounds = slice.slice,
				layeredDataLayout = layeredDataLayout
			};
			gridGraphNodeData.AllocateBuffers(dependencyTracker);
			GCHandle gchandle = GCHandle.Alloc(nodes);
			JobHandle jobHandle = new JobReadNodeData
			{
				nodesHandle = gchandle,
				nodePositions = gridGraphNodeData.positions,
				nodePenalties = gridGraphNodeData.penalties,
				nodeTags = gridGraphNodeData.tags,
				nodeConnections = gridGraphNodeData.connections,
				nodeWalkableWithErosion = gridGraphNodeData.walkableWithErosion,
				nodeWalkable = gridGraphNodeData.walkable,
				slice = slice
			}.ScheduleBatch(gridGraphNodeData.numNodes, math.max(2000, gridGraphNodeData.numNodes / 16), dependencyTracker, nodesDependsOn);
			dependencyTracker.DeferFree(gchandle, jobHandle);
			if (graphNodeNormals.IsCreated)
			{
				gridGraphNodeData.ReadNodeNormals(slice, graphNodeNormals, dependencyTracker);
			}
			return gridGraphNodeData;
		}

		// Token: 0x06000DD0 RID: 3536 RVA: 0x000546A0 File Offset: 0x000528A0
		public GridGraphNodeData ReadFromNodesAndCopy(GridNodeBase[] nodes, Slice3D slice, JobHandle nodesDependsOn, NativeArray<float4> graphNodeNormals, bool copyPenaltyAndTags, JobDependencyTracker dependencyTracker)
		{
			GridGraphNodeData gridGraphNodeData = GridGraphNodeData.ReadFromNodes(nodes, slice, nodesDependsOn, graphNodeNormals, this.allocationMethod, this.layeredDataLayout, dependencyTracker);
			gridGraphNodeData.CopyFrom(this, copyPenaltyAndTags, dependencyTracker);
			return gridGraphNodeData;
		}

		// Token: 0x06000DD1 RID: 3537 RVA: 0x000546D8 File Offset: 0x000528D8
		public void CopyFrom(GridGraphNodeData other, bool copyPenaltyAndTags, JobDependencyTracker dependencyTracker)
		{
			this.CopyFrom(other, IntBounds.Intersection(this.bounds, other.bounds), copyPenaltyAndTags, dependencyTracker);
		}

		// Token: 0x06000DD2 RID: 3538 RVA: 0x000546F4 File Offset: 0x000528F4
		public void CopyFrom(GridGraphNodeData other, IntBounds bounds, bool copyPenaltyAndTags, JobDependencyTracker dependencyTracker)
		{
			JobCopyBuffers jobCopyBuffers = new JobCopyBuffers
			{
				input = other,
				output = this,
				copyPenaltyAndTags = copyPenaltyAndTags,
				bounds = bounds
			};
			if (dependencyTracker != null)
			{
				jobCopyBuffers.Schedule(dependencyTracker);
				return;
			}
			(ref jobCopyBuffers).RunByRef<JobCopyBuffers>();
		}

		// Token: 0x06000DD3 RID: 3539 RVA: 0x00054748 File Offset: 0x00052948
		public JobHandle AssignToNodes(GridNodeBase[] nodes, int3 nodeArrayBounds, IntBounds writeMask, uint graphIndex, JobHandle nodesDependsOn, JobDependencyTracker dependencyTracker)
		{
			GCHandle gchandle = GCHandle.Alloc(nodes);
			JobHandle jobHandle = new JobWriteNodeData
			{
				nodesHandle = gchandle,
				graphIndex = graphIndex,
				nodePositions = this.positions,
				nodePenalties = this.penalties,
				nodeTags = this.tags,
				nodeConnections = this.connections,
				nodeWalkableWithErosion = this.walkableWithErosion,
				nodeWalkable = this.walkable,
				nodeArrayBounds = nodeArrayBounds,
				dataBounds = this.bounds,
				writeMask = writeMask
			}.ScheduleBatch(writeMask.volume, math.max(1000, writeMask.volume / 16), dependencyTracker, nodesDependsOn);
			dependencyTracker.DeferFree(gchandle, jobHandle);
			return jobHandle;
		}

		// Token: 0x040009DD RID: 2525
		public Allocator allocationMethod;

		// Token: 0x040009DE RID: 2526
		public int numNodes;

		// Token: 0x040009DF RID: 2527
		public IntBounds bounds;

		// Token: 0x040009E0 RID: 2528
		public NativeArray<Vector3> positions;

		// Token: 0x040009E1 RID: 2529
		public NativeArray<ulong> connections;

		// Token: 0x040009E2 RID: 2530
		public NativeArray<uint> penalties;

		// Token: 0x040009E3 RID: 2531
		public NativeArray<int> tags;

		// Token: 0x040009E4 RID: 2532
		public NativeArray<float4> normals;

		// Token: 0x040009E5 RID: 2533
		public NativeArray<bool> walkable;

		// Token: 0x040009E6 RID: 2534
		public NativeArray<bool> walkableWithErosion;

		// Token: 0x040009E7 RID: 2535
		public bool layeredDataLayout;

		// Token: 0x02000208 RID: 520
		private struct LightReader : GridIterationUtilities.ISliceAction
		{
			// Token: 0x06000DD4 RID: 3540 RVA: 0x00054810 File Offset: 0x00052A10
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe void Execute(uint outerIdx, uint innerIdx)
			{
				if ((ulong)outerIdx < (ulong)((long)this.nodes.Length))
				{
					GridNodeBase gridNodeBase = this.nodes[(int)outerIdx];
					if (gridNodeBase != null)
					{
						*this.nodePositions[innerIdx] = (Vector3)gridNodeBase.position;
						*this.nodeWalkable[innerIdx] = gridNodeBase.Walkable;
						return;
					}
				}
				*this.nodePositions[innerIdx] = Vector3.zero;
				*this.nodeWalkable[innerIdx] = false;
			}

			// Token: 0x040009E8 RID: 2536
			public GridNodeBase[] nodes;

			// Token: 0x040009E9 RID: 2537
			public UnsafeSpan<Vector3> nodePositions;

			// Token: 0x040009EA RID: 2538
			public UnsafeSpan<bool> nodeWalkable;
		}
	}
}
