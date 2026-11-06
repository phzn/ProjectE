using System;
using System.Collections.Generic;
using Pathfinding.Collections;
using Pathfinding.Graphs.Grid.Jobs;
using Pathfinding.Jobs;
using Pathfinding.Util;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;

namespace Pathfinding.Graphs.Grid
{
	// Token: 0x02000209 RID: 521
	public struct GridGraphScanData
	{
		// Token: 0x17000211 RID: 529
		// (get) Token: 0x06000DD5 RID: 3541 RVA: 0x0005488A File Offset: 0x00052A8A
		[Obsolete("Use nodes.bounds or heightHitsBounds depending on if you are using the heightHits array or not")]
		public IntBounds bounds
		{
			get
			{
				return this.nodes.bounds;
			}
		}

		// Token: 0x17000212 RID: 530
		// (get) Token: 0x06000DD6 RID: 3542 RVA: 0x00054897 File Offset: 0x00052A97
		[Obsolete("Use nodes.layeredDataLayout instead")]
		public bool layeredDataLayout
		{
			get
			{
				return this.nodes.layeredDataLayout;
			}
		}

		// Token: 0x17000213 RID: 531
		// (get) Token: 0x06000DD7 RID: 3543 RVA: 0x000548A4 File Offset: 0x00052AA4
		[Obsolete("Use nodes.positions instead")]
		public NativeArray<Vector3> nodePositions
		{
			get
			{
				return this.nodes.positions;
			}
		}

		// Token: 0x17000214 RID: 532
		// (get) Token: 0x06000DD8 RID: 3544 RVA: 0x000548B1 File Offset: 0x00052AB1
		[Obsolete("Use nodes.connections instead")]
		public NativeArray<ulong> nodeConnections
		{
			get
			{
				return this.nodes.connections;
			}
		}

		// Token: 0x17000215 RID: 533
		// (get) Token: 0x06000DD9 RID: 3545 RVA: 0x000548BE File Offset: 0x00052ABE
		[Obsolete("Use nodes.penalties instead")]
		public NativeArray<uint> nodePenalties
		{
			get
			{
				return this.nodes.penalties;
			}
		}

		// Token: 0x17000216 RID: 534
		// (get) Token: 0x06000DDA RID: 3546 RVA: 0x000548CB File Offset: 0x00052ACB
		[Obsolete("Use nodes.tags instead")]
		public NativeArray<int> nodeTags
		{
			get
			{
				return this.nodes.tags;
			}
		}

		// Token: 0x17000217 RID: 535
		// (get) Token: 0x06000DDB RID: 3547 RVA: 0x000548D8 File Offset: 0x00052AD8
		[Obsolete("Use nodes.normals instead")]
		public NativeArray<float4> nodeNormals
		{
			get
			{
				return this.nodes.normals;
			}
		}

		// Token: 0x17000218 RID: 536
		// (get) Token: 0x06000DDC RID: 3548 RVA: 0x000548E5 File Offset: 0x00052AE5
		[Obsolete("Use nodes.walkable instead")]
		public NativeArray<bool> nodeWalkable
		{
			get
			{
				return this.nodes.walkable;
			}
		}

		// Token: 0x17000219 RID: 537
		// (get) Token: 0x06000DDD RID: 3549 RVA: 0x000548F2 File Offset: 0x00052AF2
		[Obsolete("Use nodes.walkableWithErosion instead")]
		public NativeArray<bool> nodeWalkableWithErosion
		{
			get
			{
				return this.nodes.walkableWithErosion;
			}
		}

		// Token: 0x06000DDE RID: 3550 RVA: 0x000548FF File Offset: 0x00052AFF
		public void SetDefaultPenalties(uint initialPenalty)
		{
			this.nodes.penalties.MemSet(initialPenalty).Schedule(this.dependencyTracker);
		}

		// Token: 0x06000DDF RID: 3551 RVA: 0x00054920 File Offset: 0x00052B20
		public void SetDefaultNodePositions(GraphTransform transform)
		{
			new JobNodeGridLayout
			{
				graphToWorld = transform.matrix,
				bounds = this.nodes.bounds,
				nodePositions = this.nodes.positions
			}.Schedule(this.dependencyTracker);
		}

		// Token: 0x06000DE0 RID: 3552 RVA: 0x00054974 File Offset: 0x00052B74
		public JobHandle HeightCheck(GraphCollision collision, float nodeWidth, int maxHits, IntBounds recalculationBounds, NativeArray<int> outLayerCount, float characterHeight, Allocator allocator)
		{
			int num = recalculationBounds.size.x * recalculationBounds.size.z;
			this.heightHits = this.dependencyTracker.NewNativeArray<RaycastHit>(num * maxHits, allocator, NativeArrayOptions.ClearMemory);
			this.heightHitsBounds = recalculationBounds;
			Vector3 vector = this.up * collision.fromHeight;
			Vector3 vector2 = -this.up * (collision.fromHeight + 0.01f);
			if (collision.thickRaycast)
			{
				if (maxHits > 1)
				{
					throw new NotImplementedException("Thick raycasts are not supported for layered grid graphs");
				}
				NativeArray<SpherecastCommand> nativeArray = this.dependencyTracker.NewNativeArray<SpherecastCommand>(num, allocator, NativeArrayOptions.ClearMemory);
				new JobPrepareGridRaycastThick
				{
					graphToWorld = this.transform.matrix,
					bounds = recalculationBounds,
					physicsScene = Physics.defaultPhysicsScene,
					raycastOffset = vector,
					raycastDirection = vector2,
					raycastMask = collision.heightMask,
					raycastCommands = nativeArray,
					radius = collision.thickRaycastDiameter * nodeWidth * 0.5f
				}.Schedule(this.dependencyTracker);
				this.dependencyTracker.ScheduleBatch(nativeArray, this.heightHits, 2048);
				new JobClampHitToRay
				{
					hits = this.heightHits,
					commands = nativeArray
				}.Schedule(this.dependencyTracker);
				outLayerCount[0] = 1;
				return default(JobHandle);
			}
			else
			{
				NativeArray<RaycastCommand> nativeArray2 = this.dependencyTracker.NewNativeArray<RaycastCommand>(num, allocator, NativeArrayOptions.ClearMemory);
				JobHandle jobHandle = new JobPrepareGridRaycast
				{
					graphToWorld = this.transform.matrix,
					bounds = recalculationBounds,
					physicsScene = Physics.defaultPhysicsScene,
					raycastOffset = vector,
					raycastDirection = vector2,
					raycastMask = collision.heightMask,
					raycastCommands = nativeArray2
				}.Schedule(this.dependencyTracker);
				if (maxHits > 1)
				{
					float num2 = characterHeight * 0.5f;
					JobHandle jobHandle2 = new JobRaycastAll(nativeArray2, this.heightHits, Physics.defaultPhysicsScene, maxHits, allocator, this.dependencyTracker, num2).Schedule(jobHandle);
					return new JobMaxHitCount
					{
						hits = this.heightHits,
						maxHits = maxHits,
						layerStride = num,
						maxHitCount = outLayerCount
					}.Schedule(jobHandle2);
				}
				this.dependencyTracker.ScheduleBatch(nativeArray2, this.heightHits, 2048);
				outLayerCount[0] = 1;
				return default(JobHandle);
			}
		}

		// Token: 0x06000DE1 RID: 3553 RVA: 0x00054BF8 File Offset: 0x00052DF8
		public void CopyHits(IntBounds recalculationBounds)
		{
			this.nodes.normals.MemSet(float4.zero).Schedule(this.dependencyTracker);
			new JobCopyHits
			{
				hits = this.heightHits,
				points = this.nodes.positions,
				normals = this.nodes.normals,
				slice = new Slice3D(this.nodes.bounds, recalculationBounds)
			}.Schedule(this.dependencyTracker);
		}

		// Token: 0x06000DE2 RID: 3554 RVA: 0x00054C84 File Offset: 0x00052E84
		public void CalculateWalkabilityFromHeightData(bool useRaycastNormal, bool unwalkableWhenNoGround, float maxSlope, float characterHeight)
		{
			new JobNodeWalkability
			{
				useRaycastNormal = useRaycastNormal,
				unwalkableWhenNoGround = unwalkableWhenNoGround,
				maxSlope = maxSlope,
				up = this.up,
				nodeNormals = this.nodes.normals,
				nodeWalkable = this.nodes.walkable,
				nodePositions = this.nodes.positions.Reinterpret<float3>(),
				characterHeight = characterHeight,
				layerStride = this.nodes.bounds.size.x * this.nodes.bounds.size.z
			}.Schedule(this.dependencyTracker);
		}

		// Token: 0x06000DE3 RID: 3555 RVA: 0x00054D44 File Offset: 0x00052F44
		public IEnumerator<JobHandle> CollisionCheck(GraphCollision collision, IntBounds calculationBounds)
		{
			if (collision.type == ColliderType.Ray && !collision.use2D)
			{
				NativeArray<bool> nativeArray = this.dependencyTracker.NewNativeArray<bool>(this.nodes.numNodes, this.nodes.allocationMethod, NativeArrayOptions.UninitializedMemory);
				collision.JobCollisionRay(this.nodes.positions, nativeArray, this.up, this.nodes.allocationMethod, this.dependencyTracker);
				this.nodes.walkable.BitwiseAndWith(nativeArray).WithLength(this.nodes.numNodes).Schedule(this.dependencyTracker);
				return null;
			}
			return new JobCheckCollisions
			{
				nodePositions = this.nodes.positions,
				collisionResult = this.nodes.walkable,
				collision = collision
			}.ExecuteMainThreadJob(this.dependencyTracker);
		}

		// Token: 0x06000DE4 RID: 3556 RVA: 0x00054E20 File Offset: 0x00053020
		public void Connections(float maxStepHeight, bool maxStepUsesSlope, IntBounds calculationBounds, NumNeighbours neighbours, bool cutCorners, bool use2D, bool useErodedWalkability, float characterHeight)
		{
			JobCalculateGridConnections jobCalculateGridConnections = new JobCalculateGridConnections
			{
				maxStepHeight = maxStepHeight,
				maxStepUsesSlope = maxStepUsesSlope,
				graphToWorld = this.transform.matrix,
				bounds = calculationBounds.Offset(-this.nodes.bounds.min),
				arrayBounds = this.nodes.bounds.size,
				neighbours = neighbours,
				use2D = use2D,
				cutCorners = cutCorners,
				nodeWalkable = (useErodedWalkability ? this.nodes.walkableWithErosion : this.nodes.walkable).AsUnsafeSpanNoChecks<bool>(),
				nodePositions = this.nodes.positions.AsUnsafeSpanNoChecks<Vector3>(),
				nodeNormals = this.nodes.normals.AsUnsafeSpanNoChecks<float4>(),
				nodeConnections = this.nodes.connections.AsUnsafeSpanNoChecks<ulong>(),
				characterHeight = characterHeight,
				layeredDataLayout = this.nodes.layeredDataLayout
			};
			if (this.dependencyTracker != null)
			{
				jobCalculateGridConnections.ScheduleBatch(calculationBounds.size.z, 20, this.dependencyTracker, default(JobHandle));
			}
			else
			{
				jobCalculateGridConnections.RunBatch(calculationBounds.size.z);
			}
			if (this.nodes.layeredDataLayout)
			{
				JobFilterDiagonalConnections jobFilterDiagonalConnections = new JobFilterDiagonalConnections
				{
					slice = new Slice3D(this.nodes.bounds, calculationBounds),
					neighbours = neighbours,
					cutCorners = cutCorners,
					nodeConnections = this.nodes.connections.AsUnsafeSpanNoChecks<ulong>()
				};
				if (this.dependencyTracker != null)
				{
					jobFilterDiagonalConnections.ScheduleBatch(calculationBounds.size.z, 20, this.dependencyTracker, default(JobHandle));
					return;
				}
				jobFilterDiagonalConnections.RunBatch(calculationBounds.size.z);
			}
		}

		// Token: 0x06000DE5 RID: 3557 RVA: 0x00055018 File Offset: 0x00053218
		public void Erosion(NumNeighbours neighbours, int erodeIterations, IntBounds erosionWriteMask, bool erosionUsesTags, int erosionStartTag, int erosionTagsPrecedenceMask)
		{
			if (!this.nodes.layeredDataLayout)
			{
				new JobErosion<FlatGridAdjacencyMapper>
				{
					bounds = this.nodes.bounds,
					writeMask = erosionWriteMask,
					neighbours = neighbours,
					nodeConnections = this.nodes.connections,
					erosion = erodeIterations,
					nodeWalkable = this.nodes.walkable,
					outNodeWalkable = this.nodes.walkableWithErosion,
					nodeTags = this.nodes.tags,
					erosionUsesTags = erosionUsesTags,
					erosionStartTag = erosionStartTag,
					erosionTagsPrecedenceMask = erosionTagsPrecedenceMask
				}.Schedule(this.dependencyTracker);
				return;
			}
			new JobErosion<LayeredGridAdjacencyMapper>
			{
				bounds = this.nodes.bounds,
				writeMask = erosionWriteMask,
				neighbours = neighbours,
				nodeConnections = this.nodes.connections,
				erosion = erodeIterations,
				nodeWalkable = this.nodes.walkable,
				outNodeWalkable = this.nodes.walkableWithErosion,
				nodeTags = this.nodes.tags,
				erosionUsesTags = erosionUsesTags,
				erosionStartTag = erosionStartTag,
				erosionTagsPrecedenceMask = erosionTagsPrecedenceMask
			}.Schedule(this.dependencyTracker);
		}

		// Token: 0x06000DE6 RID: 3558 RVA: 0x0005517C File Offset: 0x0005337C
		public unsafe void AssignNodeConnections(GridNodeBase[] nodes, int3 nodeArrayBounds, IntBounds writeBounds)
		{
			IntBounds bounds = this.nodes.bounds;
			int3 @int = writeBounds.min - bounds.min;
			UnsafeSpan<ulong> unsafeSpan = this.nodes.connections.AsUnsafeReadOnlySpan<ulong>();
			for (int i = 0; i < writeBounds.size.y; i++)
			{
				int num = (i + writeBounds.min.y) * nodeArrayBounds.x * nodeArrayBounds.z;
				for (int j = 0; j < writeBounds.size.z; j++)
				{
					int num2 = num + (j + writeBounds.min.z) * nodeArrayBounds.x + writeBounds.min.x;
					int num3 = (i + @int.y) * bounds.size.x * bounds.size.z + (j + @int.z) * bounds.size.x + @int.x;
					for (int k = 0; k < writeBounds.size.x; k++)
					{
						GridNodeBase gridNodeBase = nodes[num2 + k];
						int num4 = num3 + k;
						ulong num5 = *unsafeSpan[num4];
						if (gridNodeBase != null)
						{
							LevelGridNode levelGridNode = gridNodeBase as LevelGridNode;
							if (levelGridNode != null)
							{
								levelGridNode.SetAllConnectionInternal(num5);
							}
							else
							{
								(gridNodeBase as GridNode).SetAllConnectionInternal((int)num5);
							}
						}
					}
				}
			}
		}

		// Token: 0x040009EB RID: 2539
		public JobDependencyTracker dependencyTracker;

		// Token: 0x040009EC RID: 2540
		public Vector3 up;

		// Token: 0x040009ED RID: 2541
		public GraphTransform transform;

		// Token: 0x040009EE RID: 2542
		public GridGraphNodeData nodes;

		// Token: 0x040009EF RID: 2543
		public NativeArray<RaycastHit> heightHits;

		// Token: 0x040009F0 RID: 2544
		public IntBounds heightHitsBounds;
	}
}
