using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Pathfinding.Drawing;
using Pathfinding.PID;
using Unity.Burst;
using Unity.Burst.Intrinsics;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Entities;
using Unity.Entities.Internal;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Profiling;
using Unity.Transforms;
using UnityEngine;

namespace Pathfinding.ECS
{
	// Token: 0x0200031F RID: 799
	[BurstCompile]
	[WithNone(new Type[] { typeof(AgentOffMeshLinkTraversal) })]
	[WithAll(new Type[]
	{
		typeof(SimulateMovement),
		typeof(SimulateMovementControl)
	})]
	[CompilerGenerated]
	public struct JobControl : IJobEntity, IJobEntityChunkBeginEnd, IJobChunk
	{
		// Token: 0x0600139E RID: 5022 RVA: 0x00074CE8 File Offset: 0x00072EE8
		public static float3 ClampToNavmesh(float3 position, float3 closestOnNavmesh, in AgentCylinderShape shape, in AgentMovementPlane movementPlane)
		{
			float3 up = movementPlane.value.up;
			float num = math.dot(position - closestOnNavmesh, up);
			num = math.max(num, -shape.height * 0.4f);
			position = closestOnNavmesh + num * up;
			return position;
		}

		// Token: 0x0600139F RID: 5023 RVA: 0x00074D34 File Offset: 0x00072F34
		public static float3 ClampToNavmeshContinuously(float3 position, float3 closestOnNavmesh, in AgentCylinderShape shape, in AgentMovementPlane movementPlane, float movementSpeed, float dt)
		{
			if (dt <= 0f)
			{
				return position;
			}
			float3 @float = JobControl.ClampToNavmesh(position, closestOnNavmesh, in shape, in movementPlane);
			float3 float2 = @float - position;
			float num = math.length(float2);
			float num2 = math.max(0.5f, movementSpeed) * dt;
			if (num <= num2)
			{
				return @float;
			}
			return position + float2 * (num2 / num);
		}

		// Token: 0x060013A0 RID: 5024 RVA: 0x00074D8C File Offset: 0x00072F8C
		public bool OnChunkBegin(in ArchetypeChunk chunk, int unfilteredChunkIndex, bool useEnabledMask, in v128 chunkEnabledMask)
		{
			if (!this.edgesScratch.IsCreated)
			{
				this.edgesScratch = new NativeList<float2>(64, Allocator.Temp);
			}
			if (!this.indicesScratch.IsCreated)
			{
				this.indicesScratch = new NativeList<int>(64, Allocator.Temp);
			}
			return true;
		}

		// Token: 0x060013A1 RID: 5025 RVA: 0x00003788 File Offset: 0x00001988
		public void OnChunkEnd(in ArchetypeChunk chunk, int unfilteredChunkIndex, bool useEnabledMask, in v128 chunkEnabledMask, bool chunkWasExecuted)
		{
		}

		// Token: 0x060013A2 RID: 5026 RVA: 0x00074DDC File Offset: 0x00072FDC
		public void Execute(ref LocalTransform transform, ref MovementState state, in DestinationPoint destination, in AgentCylinderShape shape, in AgentMovementPlane movementPlane, in MovementSettings settings, in ResolvedMovement resolvedMovement, ref MovementControl controlOutput)
		{
			float3 @float = JobControl.ClampToNavmeshContinuously(transform.Position, state.closestOnNavmesh, in shape, in movementPlane, settings.follower.speed * math.abs(transform.Scale), this.dt);
			this.edgesScratch.Clear();
			float num = math.abs(transform.Scale);
			PIDMovement follower = settings.follower;
			follower.ScaleByAgentScale(num);
			follower.desiredWallDistance *= resolvedMovement.turningRadiusMultiplier;
			if (state.isOnValidNode)
			{
				Bounds bounds = PIDMovement.InterestingEdgeBounds(ref follower, state.closestOnNavmesh, state.nextCorner, shape.height, movementPlane.value);
				this.navmeshEdgeData.GetEdgesInRange(state.hierarchicalNodeIndex, bounds, this.edgesScratch, this.indicesScratch, movementPlane.value);
			}
			float num2 = (state.traversingLastPart ? math.max(0f, settings.stopDistance - 0.005f) : 0f);
			float num3 = math.max(0f, state.remainingDistanceToEndOfPart - num2);
			float num4 = movementPlane.value.ToPlane(transform.Rotation) - state.rotationOffset - state.rotationOffset2;
			transform.Position = @float;
			if (this.dt <= 1E-06f)
			{
				controlOutput.hierarchicalNodeIndex = -1;
				return;
			}
			if (!math.isfinite(num3))
			{
				controlOutput = new MovementControl
				{
					targetPoint = @float,
					speed = 0f,
					endOfPath = @float,
					maxSpeed = settings.follower.speed,
					overrideLocalAvoidance = false,
					hierarchicalNodeIndex = state.hierarchicalNodeIndex,
					targetRotation = resolvedMovement.targetRotation,
					targetRotationHint = resolvedMovement.targetRotation,
					rotationSpeed = settings.follower.maxRotationSpeed,
					targetRotationOffset = state.rotationOffset
				};
				return;
			}
			if (settings.isStopped || !state.hasValidEndPoint)
			{
				MovementControl movementControl = default(MovementControl);
				movementControl.targetPoint = @float + math.normalizesafe(resolvedMovement.targetPoint - @float, default(float3)) * 10f;
				PIDMovement follower2 = settings.follower;
				movementControl.speed = follower2.Accelerate(resolvedMovement.speed, settings.follower.slowdownTime, -this.dt);
				movementControl.endOfPath = state.endOfPath;
				movementControl.maxSpeed = settings.follower.speed;
				movementControl.overrideLocalAvoidance = false;
				movementControl.hierarchicalNodeIndex = state.hierarchicalNodeIndex;
				movementControl.targetRotation = num4;
				movementControl.targetRotationHint = num4;
				movementControl.rotationSpeed = settings.follower.maxRotationSpeed;
				movementControl.targetRotationOffset = state.rotationOffset;
				controlOutput = movementControl;
				return;
			}
			PIDMovement.ControlParams controlParams = new PIDMovement.ControlParams
			{
				edges = this.edgesScratch.AsArray(),
				nextCorner = state.nextCorner,
				agentRadius = shape.radius,
				facingDirectionAtEndOfPath = destination.facingDirection,
				endOfPath = state.endOfPath,
				remainingDistance = num3,
				closestOnNavmesh = state.closestOnNavmesh,
				debugFlags = PIDMovement.DebugFlags.Nothing,
				p = @float,
				rotation = num4,
				maxDesiredWallDistance = state.followerState.maxDesiredWallDistance,
				speed = controlOutput.speed,
				movementPlane = movementPlane.value
			};
			AnglePIDControlOutput2D anglePIDControlOutput2D = PIDMovement.Control(ref follower, this.dt, ref controlParams, ref this.draw, out state.followerState.maxDesiredWallDistance);
			float3 float2 = movementPlane.value.ToWorld(anglePIDControlOutput2D.positionDelta, 0f);
			float num5 = math.length(float2) / this.dt;
			controlOutput = new MovementControl
			{
				targetPoint = @float + math.normalizesafe(float2, default(float3)) * num3,
				speed = num5,
				endOfPath = state.endOfPath,
				maxSpeed = follower.speed * 1.1f,
				overrideLocalAvoidance = false,
				hierarchicalNodeIndex = state.hierarchicalNodeIndex,
				targetRotation = num4 + anglePIDControlOutput2D.rotationDelta,
				targetRotationHint = num4 + AstarMath.DeltaAngle(num4, anglePIDControlOutput2D.targetRotation),
				rotationSpeed = math.abs(anglePIDControlOutput2D.rotationDelta / this.dt),
				targetRotationOffset = state.rotationOffset
			};
		}

		// Token: 0x060013A3 RID: 5027 RVA: 0x00075278 File Offset: 0x00073478
		[CompilerGenerated]
		public void Execute(in ArchetypeChunk chunk, int chunkIndexInQuery, bool useEnabledMask, in v128 chunkEnabledMask)
		{
			bool flag = this.OnChunkBegin(in chunk, chunkIndexInQuery, useEnabledMask, in chunkEnabledMask);
			if (flag)
			{
				IntPtr intPtr = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtr<LocalTransform>(chunk, ref this.__TypeHandle.__Unity_Transforms_LocalTransform_RW_ComponentTypeHandle);
				IntPtr intPtr2 = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtr<MovementState>(chunk, ref this.__TypeHandle.__Pathfinding_ECS_MovementState_RW_ComponentTypeHandle);
				IntPtr intPtr3 = InternalCompilerInterface.UnsafeGetChunkNativeArrayReadOnlyIntPtr<DestinationPoint>(chunk, ref this.__TypeHandle.__Pathfinding_ECS_DestinationPoint_RO_ComponentTypeHandle);
				IntPtr intPtr4 = InternalCompilerInterface.UnsafeGetChunkNativeArrayReadOnlyIntPtr<AgentCylinderShape>(chunk, ref this.__TypeHandle.__Pathfinding_ECS_AgentCylinderShape_RO_ComponentTypeHandle);
				IntPtr intPtr5 = InternalCompilerInterface.UnsafeGetChunkNativeArrayReadOnlyIntPtr<AgentMovementPlane>(chunk, ref this.__TypeHandle.__Pathfinding_ECS_AgentMovementPlane_RO_ComponentTypeHandle);
				IntPtr intPtr6 = InternalCompilerInterface.UnsafeGetChunkNativeArrayReadOnlyIntPtr<MovementSettings>(chunk, ref this.__TypeHandle.__Pathfinding_ECS_MovementSettings_RO_ComponentTypeHandle);
				IntPtr intPtr7 = InternalCompilerInterface.UnsafeGetChunkNativeArrayReadOnlyIntPtr<ResolvedMovement>(chunk, ref this.__TypeHandle.__Pathfinding_ECS_ResolvedMovement_RO_ComponentTypeHandle);
				IntPtr intPtr8 = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtr<MovementControl>(chunk, ref this.__TypeHandle.__Pathfinding_ECS_MovementControl_RW_ComponentTypeHandle);
				int count = chunk.Count;
				int num = 0;
				if (!useEnabledMask)
				{
					for (int i = 0; i < count; i++)
					{
						ref LocalTransform ptr = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<LocalTransform>(intPtr, i);
						ref MovementState ptr2 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<MovementState>(intPtr2, i);
						ref DestinationPoint ptr3 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<DestinationPoint>(intPtr3, i);
						ref AgentCylinderShape ptr4 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<AgentCylinderShape>(intPtr4, i);
						ref AgentMovementPlane ptr5 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<AgentMovementPlane>(intPtr5, i);
						ref MovementSettings ptr6 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<MovementSettings>(intPtr6, i);
						ref ResolvedMovement ptr7 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<ResolvedMovement>(intPtr7, i);
						ref MovementControl ptr8 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<MovementControl>(intPtr8, i);
						this.Execute(ref ptr, ref ptr2, in ptr3, in ptr4, in ptr5, in ptr6, in ptr7, ref ptr8);
						num++;
					}
				}
				else if (math.countbits(chunkEnabledMask.ULong0 ^ (chunkEnabledMask.ULong0 << 1)) + math.countbits(chunkEnabledMask.ULong1 ^ (chunkEnabledMask.ULong1 << 1)) - 1 <= 4)
				{
					int j = 0;
					int num2 = 0;
					while (InternalCompilerInterface.UnsafeTryGetNextEnabledBitRange(chunkEnabledMask, num2, out j, out num2))
					{
						while (j < num2)
						{
							ref LocalTransform ptr9 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<LocalTransform>(intPtr, j);
							ref MovementState ptr10 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<MovementState>(intPtr2, j);
							ref DestinationPoint ptr11 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<DestinationPoint>(intPtr3, j);
							ref AgentCylinderShape ptr12 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<AgentCylinderShape>(intPtr4, j);
							ref AgentMovementPlane ptr13 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<AgentMovementPlane>(intPtr5, j);
							ref MovementSettings ptr14 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<MovementSettings>(intPtr6, j);
							ref ResolvedMovement ptr15 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<ResolvedMovement>(intPtr7, j);
							ref MovementControl ptr16 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<MovementControl>(intPtr8, j);
							this.Execute(ref ptr9, ref ptr10, in ptr11, in ptr12, in ptr13, in ptr14, in ptr15, ref ptr16);
							j++;
							num++;
						}
					}
				}
				else
				{
					ulong num3 = chunkEnabledMask.ULong0;
					int num4 = math.min(64, count);
					for (int k = 0; k < num4; k++)
					{
						if ((num3 & 1UL) != 0UL)
						{
							ref LocalTransform ptr17 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<LocalTransform>(intPtr, k);
							ref MovementState ptr18 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<MovementState>(intPtr2, k);
							ref DestinationPoint ptr19 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<DestinationPoint>(intPtr3, k);
							ref AgentCylinderShape ptr20 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<AgentCylinderShape>(intPtr4, k);
							ref AgentMovementPlane ptr21 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<AgentMovementPlane>(intPtr5, k);
							ref MovementSettings ptr22 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<MovementSettings>(intPtr6, k);
							ref ResolvedMovement ptr23 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<ResolvedMovement>(intPtr7, k);
							ref MovementControl ptr24 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<MovementControl>(intPtr8, k);
							this.Execute(ref ptr17, ref ptr18, in ptr19, in ptr20, in ptr21, in ptr22, in ptr23, ref ptr24);
							num++;
						}
						num3 >>= 1;
					}
					num3 = chunkEnabledMask.ULong1;
					for (int l = 64; l < count; l++)
					{
						if ((num3 & 1UL) != 0UL)
						{
							ref LocalTransform ptr25 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<LocalTransform>(intPtr, l);
							ref MovementState ptr26 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<MovementState>(intPtr2, l);
							ref DestinationPoint ptr27 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<DestinationPoint>(intPtr3, l);
							ref AgentCylinderShape ptr28 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<AgentCylinderShape>(intPtr4, l);
							ref AgentMovementPlane ptr29 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<AgentMovementPlane>(intPtr5, l);
							ref MovementSettings ptr30 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<MovementSettings>(intPtr6, l);
							ref ResolvedMovement ptr31 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<ResolvedMovement>(intPtr7, l);
							ref MovementControl ptr32 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<MovementControl>(intPtr8, l);
							this.Execute(ref ptr25, ref ptr26, in ptr27, in ptr28, in ptr29, in ptr30, in ptr31, ref ptr32);
							num++;
						}
						num3 >>= 1;
					}
				}
			}
			this.OnChunkEnd(in chunk, chunkIndexInQuery, useEnabledMask, in chunkEnabledMask, flag);
		}

		// Token: 0x060013A4 RID: 5028 RVA: 0x00075618 File Offset: 0x00073818
		private JobHandle __ThrowCodeGenException()
		{
			throw new Exception("This method should have been replaced by source gen.");
		}

		// Token: 0x060013A5 RID: 5029 RVA: 0x0007562F File Offset: 0x0007382F
		public void Run()
		{
			this.__ThrowCodeGenException();
		}

		// Token: 0x060013A6 RID: 5030 RVA: 0x0007562F File Offset: 0x0007382F
		public void RunByRef()
		{
			this.__ThrowCodeGenException();
		}

		// Token: 0x060013A7 RID: 5031 RVA: 0x0007562F File Offset: 0x0007382F
		public void Run(EntityQuery query)
		{
			this.__ThrowCodeGenException();
		}

		// Token: 0x060013A8 RID: 5032 RVA: 0x0007562F File Offset: 0x0007382F
		public void RunByRef(EntityQuery query)
		{
			this.__ThrowCodeGenException();
		}

		// Token: 0x060013A9 RID: 5033 RVA: 0x00075638 File Offset: 0x00073838
		public JobHandle Schedule(JobHandle dependsOn)
		{
			return this.__ThrowCodeGenException();
		}

		// Token: 0x060013AA RID: 5034 RVA: 0x00075638 File Offset: 0x00073838
		public JobHandle ScheduleByRef(JobHandle dependsOn)
		{
			return this.__ThrowCodeGenException();
		}

		// Token: 0x060013AB RID: 5035 RVA: 0x00075638 File Offset: 0x00073838
		public JobHandle Schedule(EntityQuery query, JobHandle dependsOn)
		{
			return this.__ThrowCodeGenException();
		}

		// Token: 0x060013AC RID: 5036 RVA: 0x00075638 File Offset: 0x00073838
		public JobHandle ScheduleByRef(EntityQuery query, JobHandle dependsOn)
		{
			return this.__ThrowCodeGenException();
		}

		// Token: 0x060013AD RID: 5037 RVA: 0x0007562F File Offset: 0x0007382F
		public void Schedule()
		{
			this.__ThrowCodeGenException();
		}

		// Token: 0x060013AE RID: 5038 RVA: 0x0007562F File Offset: 0x0007382F
		public void ScheduleByRef()
		{
			this.__ThrowCodeGenException();
		}

		// Token: 0x060013AF RID: 5039 RVA: 0x0007562F File Offset: 0x0007382F
		public void Schedule(EntityQuery query)
		{
			this.__ThrowCodeGenException();
		}

		// Token: 0x060013B0 RID: 5040 RVA: 0x0007562F File Offset: 0x0007382F
		public void ScheduleByRef(EntityQuery query)
		{
			this.__ThrowCodeGenException();
		}

		// Token: 0x060013B1 RID: 5041 RVA: 0x00075638 File Offset: 0x00073838
		public JobHandle ScheduleParallel(JobHandle dependsOn)
		{
			return this.__ThrowCodeGenException();
		}

		// Token: 0x060013B2 RID: 5042 RVA: 0x00075638 File Offset: 0x00073838
		public JobHandle ScheduleParallelByRef(JobHandle dependsOn)
		{
			return this.__ThrowCodeGenException();
		}

		// Token: 0x060013B3 RID: 5043 RVA: 0x00075638 File Offset: 0x00073838
		public JobHandle ScheduleParallel(EntityQuery query, JobHandle dependsOn)
		{
			return this.__ThrowCodeGenException();
		}

		// Token: 0x060013B4 RID: 5044 RVA: 0x00075638 File Offset: 0x00073838
		public JobHandle ScheduleParallelByRef(EntityQuery query, JobHandle dependsOn)
		{
			return this.__ThrowCodeGenException();
		}

		// Token: 0x060013B5 RID: 5045 RVA: 0x00075638 File Offset: 0x00073838
		public JobHandle ScheduleParallel(EntityQuery query, JobHandle dependsOn, NativeArray<int> chunkBaseEntityIndices)
		{
			return this.__ThrowCodeGenException();
		}

		// Token: 0x060013B6 RID: 5046 RVA: 0x00075638 File Offset: 0x00073838
		public JobHandle ScheduleParallelByRef(EntityQuery query, JobHandle dependsOn, NativeArray<int> chunkBaseEntityIndices)
		{
			return this.__ThrowCodeGenException();
		}

		// Token: 0x060013B7 RID: 5047 RVA: 0x0007562F File Offset: 0x0007382F
		public void ScheduleParallel()
		{
			this.__ThrowCodeGenException();
		}

		// Token: 0x060013B8 RID: 5048 RVA: 0x0007562F File Offset: 0x0007382F
		public void ScheduleParallelByRef()
		{
			this.__ThrowCodeGenException();
		}

		// Token: 0x060013B9 RID: 5049 RVA: 0x0007562F File Offset: 0x0007382F
		public void ScheduleParallel(EntityQuery query)
		{
			this.__ThrowCodeGenException();
		}

		// Token: 0x060013BA RID: 5050 RVA: 0x0007562F File Offset: 0x0007382F
		public void ScheduleParallelByRef(EntityQuery query)
		{
			this.__ThrowCodeGenException();
		}

		// Token: 0x060013BC RID: 5052 RVA: 0x00075651 File Offset: 0x00073851
		bool IJobEntityChunkBeginEnd.OnChunkBegin(in ArchetypeChunk chunk, int unfilteredChunkIndex, bool useEnabledMask, in v128 chunkEnabledMask)
		{
			return this.OnChunkBegin(in chunk, unfilteredChunkIndex, useEnabledMask, in chunkEnabledMask);
		}

		// Token: 0x060013BD RID: 5053 RVA: 0x0007565E File Offset: 0x0007385E
		void IJobEntityChunkBeginEnd.OnChunkEnd(in ArchetypeChunk chunk, int unfilteredChunkIndex, bool useEnabledMask, in v128 chunkEnabledMask, bool chunkWasExecuted)
		{
			this.OnChunkEnd(in chunk, unfilteredChunkIndex, useEnabledMask, in chunkEnabledMask, chunkWasExecuted);
		}

		// Token: 0x060013BE RID: 5054 RVA: 0x0007566D File Offset: 0x0007386D
		void IJobChunk.Execute(in ArchetypeChunk chunk, int unfilteredChunkIndex, bool useEnabledMask, in v128 chunkEnabledMask)
		{
			this.Execute(in chunk, unfilteredChunkIndex, useEnabledMask, in chunkEnabledMask);
		}

		// Token: 0x04000E2E RID: 3630
		public float dt;

		// Token: 0x04000E2F RID: 3631
		public CommandBuilder draw;

		// Token: 0x04000E30 RID: 3632
		[ReadOnly]
		[NativeDisableContainerSafetyRestriction]
		public NavmeshEdges.NavmeshBorderData navmeshEdgeData;

		// Token: 0x04000E31 RID: 3633
		[NativeDisableContainerSafetyRestriction]
		public NativeList<float2> edgesScratch;

		// Token: 0x04000E32 RID: 3634
		[NativeDisableContainerSafetyRestriction]
		public NativeList<int> indicesScratch;

		// Token: 0x04000E33 RID: 3635
		private static readonly ProfilerMarker MarkerConvertObstacles = new ProfilerMarker("ConvertObstacles");

		// Token: 0x04000E34 RID: 3636
		private JobControl.InternalCompilerQueryAndHandleData.TypeHandle __TypeHandle;

		// Token: 0x02000320 RID: 800
		public struct InternalCompilerQueryAndHandleData
		{
			// Token: 0x060013BF RID: 5055 RVA: 0x0007567C File Offset: 0x0007387C
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			private void __AssignQueries(ref SystemState state)
			{
				EntityQueryBuilder entityQueryBuilder = new EntityQueryBuilder(Allocator.Temp);
				this.DefaultQuery = entityQueryBuilder.WithNone<AgentOffMeshLinkTraversal>().WithAll<DestinationPoint>().WithAll<AgentCylinderShape>()
					.WithAll<AgentMovementPlane>()
					.WithAll<MovementSettings>()
					.WithAll<ResolvedMovement>()
					.WithAll<SimulateMovement>()
					.WithAll<SimulateMovementControl>()
					.WithAllRW<LocalTransform>()
					.WithAllRW<MovementState>()
					.WithAllRW<MovementControl>()
					.Build(ref state);
				entityQueryBuilder.Reset();
				entityQueryBuilder.Dispose();
			}

			// Token: 0x060013C0 RID: 5056 RVA: 0x0007570B File Offset: 0x0007390B
			public void Init(ref SystemState state, bool assignDefaultQuery)
			{
				if (assignDefaultQuery)
				{
					this.__AssignQueries(ref state);
				}
				this.__TypeHandle.__AssignHandles(ref state);
			}

			// Token: 0x060013C1 RID: 5057 RVA: 0x00075723 File Offset: 0x00073923
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public void Run(ref JobControl job, EntityQuery query)
			{
				job.__TypeHandle = this.__TypeHandle;
				(ref job).RunByRef<JobControl>(query);
			}

			// Token: 0x060013C2 RID: 5058 RVA: 0x00075738 File Offset: 0x00073938
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public JobHandle Schedule(ref JobControl job, EntityQuery query, JobHandle dependency)
			{
				job.__TypeHandle = this.__TypeHandle;
				return (ref job).ScheduleByRef<JobControl>(query, dependency);
			}

			// Token: 0x060013C3 RID: 5059 RVA: 0x0007574E File Offset: 0x0007394E
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public JobHandle ScheduleParallel(ref JobControl job, EntityQuery query, JobHandle dependency)
			{
				job.__TypeHandle = this.__TypeHandle;
				return (ref job).ScheduleParallelByRef<JobControl>(query, dependency);
			}

			// Token: 0x060013C4 RID: 5060 RVA: 0x00003788 File Offset: 0x00001988
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public void UpdateBaseEntityIndexArray(ref JobControl job, EntityQuery query, ref SystemState state)
			{
			}

			// Token: 0x060013C5 RID: 5061 RVA: 0x000743B8 File Offset: 0x000725B8
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public JobHandle UpdateBaseEntityIndexArray(ref JobControl job, EntityQuery query, JobHandle dependency, ref SystemState state)
			{
				return dependency;
			}

			// Token: 0x060013C6 RID: 5062 RVA: 0x00003788 File Offset: 0x00001988
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public void AssignEntityManager(ref JobControl job, EntityManager entityManager)
			{
			}

			// Token: 0x04000E35 RID: 3637
			public JobControl.InternalCompilerQueryAndHandleData.TypeHandle __TypeHandle;

			// Token: 0x04000E36 RID: 3638
			public EntityQuery DefaultQuery;

			// Token: 0x02000321 RID: 801
			public struct TypeHandle
			{
				// Token: 0x060013C7 RID: 5063 RVA: 0x00075764 File Offset: 0x00073964
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public void __AssignHandles(ref SystemState state)
				{
					this.__Unity_Transforms_LocalTransform_RW_ComponentTypeHandle = state.GetComponentTypeHandle<LocalTransform>(false);
					this.__Pathfinding_ECS_MovementState_RW_ComponentTypeHandle = state.GetComponentTypeHandle<MovementState>(false);
					this.__Pathfinding_ECS_DestinationPoint_RO_ComponentTypeHandle = state.GetComponentTypeHandle<DestinationPoint>(true);
					this.__Pathfinding_ECS_AgentCylinderShape_RO_ComponentTypeHandle = state.GetComponentTypeHandle<AgentCylinderShape>(true);
					this.__Pathfinding_ECS_AgentMovementPlane_RO_ComponentTypeHandle = state.GetComponentTypeHandle<AgentMovementPlane>(true);
					this.__Pathfinding_ECS_MovementSettings_RO_ComponentTypeHandle = state.GetComponentTypeHandle<MovementSettings>(true);
					this.__Pathfinding_ECS_ResolvedMovement_RO_ComponentTypeHandle = state.GetComponentTypeHandle<ResolvedMovement>(true);
					this.__Pathfinding_ECS_MovementControl_RW_ComponentTypeHandle = state.GetComponentTypeHandle<MovementControl>(false);
				}

				// Token: 0x060013C8 RID: 5064 RVA: 0x000757DC File Offset: 0x000739DC
				public void Update(ref SystemState state)
				{
					this.__Unity_Transforms_LocalTransform_RW_ComponentTypeHandle.Update(ref state);
					this.__Pathfinding_ECS_MovementState_RW_ComponentTypeHandle.Update(ref state);
					this.__Pathfinding_ECS_DestinationPoint_RO_ComponentTypeHandle.Update(ref state);
					this.__Pathfinding_ECS_AgentCylinderShape_RO_ComponentTypeHandle.Update(ref state);
					this.__Pathfinding_ECS_AgentMovementPlane_RO_ComponentTypeHandle.Update(ref state);
					this.__Pathfinding_ECS_MovementSettings_RO_ComponentTypeHandle.Update(ref state);
					this.__Pathfinding_ECS_ResolvedMovement_RO_ComponentTypeHandle.Update(ref state);
					this.__Pathfinding_ECS_MovementControl_RW_ComponentTypeHandle.Update(ref state);
				}

				// Token: 0x04000E37 RID: 3639
				public ComponentTypeHandle<LocalTransform> __Unity_Transforms_LocalTransform_RW_ComponentTypeHandle;

				// Token: 0x04000E38 RID: 3640
				public ComponentTypeHandle<MovementState> __Pathfinding_ECS_MovementState_RW_ComponentTypeHandle;

				// Token: 0x04000E39 RID: 3641
				[ReadOnly]
				public ComponentTypeHandle<DestinationPoint> __Pathfinding_ECS_DestinationPoint_RO_ComponentTypeHandle;

				// Token: 0x04000E3A RID: 3642
				[ReadOnly]
				public ComponentTypeHandle<AgentCylinderShape> __Pathfinding_ECS_AgentCylinderShape_RO_ComponentTypeHandle;

				// Token: 0x04000E3B RID: 3643
				[ReadOnly]
				public ComponentTypeHandle<AgentMovementPlane> __Pathfinding_ECS_AgentMovementPlane_RO_ComponentTypeHandle;

				// Token: 0x04000E3C RID: 3644
				[ReadOnly]
				public ComponentTypeHandle<MovementSettings> __Pathfinding_ECS_MovementSettings_RO_ComponentTypeHandle;

				// Token: 0x04000E3D RID: 3645
				[ReadOnly]
				public ComponentTypeHandle<ResolvedMovement> __Pathfinding_ECS_ResolvedMovement_RO_ComponentTypeHandle;

				// Token: 0x04000E3E RID: 3646
				public ComponentTypeHandle<MovementControl> __Pathfinding_ECS_MovementControl_RW_ComponentTypeHandle;
			}
		}

		// Token: 0x02000322 RID: 802
		public struct InternalCompiler
		{
			// Token: 0x060013C9 RID: 5065 RVA: 0x00003788 File Offset: 0x00001988
			[Conditional("ENABLE_UNITY_COLLECTIONS_CHECKS")]
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public static void CheckForErrors(int scheduleType)
			{
			}
		}
	}
}
