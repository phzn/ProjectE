using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Pathfinding.Collections;
using Pathfinding.Util;
using Unity.Burst;
using Unity.Burst.Intrinsics;
using Unity.Collections;
using Unity.Entities;
using Unity.Entities.Internal;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;

namespace Pathfinding.ECS
{
	// Token: 0x0200037D RID: 893
	[UpdateBefore(typeof(SchedulePathSearchSystem))]
	[UpdateInGroup(typeof(AIMovementSystemGroup))]
	[RequireMatchingQueriesForUpdate]
	[BurstCompile]
	[CompilerGenerated]
	public struct MovementPlaneFromGraphSystem : ISystem, ISystemCompilerGenerated
	{
		// Token: 0x06001671 RID: 5745 RVA: 0x0007C864 File Offset: 0x0007AA64
		public void OnCreate(ref SystemState state)
		{
			this.entityQueryGraph = state.GetEntityQuery(new ComponentType[]
			{
				ComponentType.ReadOnly<MovementState>(),
				ComponentType.ReadWrite<AgentMovementPlane>(),
				ComponentType.ReadOnly<AgentMovementPlaneSource>()
			});
			this.entityQueryGraph.SetSharedComponentFilter<AgentMovementPlaneSource>(new AgentMovementPlaneSource
			{
				value = MovementPlaneSource.Graph
			});
			this.entityQueryNormal = state.GetEntityQuery(new ComponentType[]
			{
				ComponentType.ReadWrite<ManagedState>(),
				ComponentType.ReadOnly<LocalTransform>(),
				ComponentType.ReadWrite<AgentMovementPlane>(),
				ComponentType.ReadOnly<AgentCylinderShape>(),
				ComponentType.ReadOnly<AgentMovementPlaneSource>()
			});
			this.entityQueryNormal.AddSharedComponentFilter<AgentMovementPlaneSource>(new AgentMovementPlaneSource
			{
				value = MovementPlaneSource.NavmeshNormal
			});
			this.graphNodeQueue = GCHandle.Alloc(new List<GraphNode>(32));
		}

		// Token: 0x06001672 RID: 5746 RVA: 0x0007C93F File Offset: 0x0007AB3F
		public void OnDestroy(ref SystemState state)
		{
			this.graphNodeQueue.Free();
		}

		// Token: 0x06001673 RID: 5747 RVA: 0x0007C94C File Offset: 0x0007AB4C
		public void OnUpdate(ref SystemState systemState)
		{
			AstarPath active = AstarPath.active;
			NavGraph[] array = ((active != null) ? active.data.graphs : null);
			if (array == null)
			{
				return;
			}
			NativeArray<AgentMovementPlane> nativeArray = CollectionHelper.CreateNativeArray<AgentMovementPlane>(array.Length, systemState.WorldUpdateAllocator, NativeArrayOptions.UninitializedMemory);
			for (int i = 0; i < array.Length; i++)
			{
				nativeArray[i] = new AgentMovementPlane(MovementPlaneFromGraphSystem.MovementPlaneFromGraph(array[i]));
			}
			if (!this.entityQueryNormal.IsEmpty)
			{
				systemState.CompleteDependency();
				NativeList<Int3> nativeList = new NativeList<Int3>(16, Allocator.Temp);
				MovementPlaneFromGraphSystem.JobMovementPlaneFromNavmeshNormal jobMovementPlaneFromNavmeshNormal = default(MovementPlaneFromGraphSystem.JobMovementPlaneFromNavmeshNormal);
				jobMovementPlaneFromNavmeshNormal.dt = AIMovementSystemGroup.TimeScaledRateManager.CheapStepDeltaTime;
				jobMovementPlaneFromNavmeshNormal.vertices = nativeList;
				jobMovementPlaneFromNavmeshNormal.que = (List<GraphNode>)this.graphNodeQueue.Target;
				this.__ScheduleViaJobChunkExtension_0(jobMovementPlaneFromNavmeshNormal, this.entityQueryNormal, systemState.Dependency, ref systemState, true);
			}
			MovementPlaneFromGraphSystem.JobMovementPlaneFromGraph jobMovementPlaneFromGraph = default(MovementPlaneFromGraphSystem.JobMovementPlaneFromGraph);
			jobMovementPlaneFromGraph.movementPlanes = nativeArray;
			systemState.Dependency = this.__ScheduleViaJobChunkExtension_1(jobMovementPlaneFromGraph, this.entityQueryGraph, systemState.Dependency, ref systemState, true);
		}

		// Token: 0x06001674 RID: 5748 RVA: 0x0007CA44 File Offset: 0x0007AC44
		public static NativeMovementPlane MovementPlaneFromGraph(NavGraph graph)
		{
			NavmeshBase navmeshBase = graph as NavmeshBase;
			if (navmeshBase != null)
			{
				return new NativeMovementPlane(navmeshBase.transform.rotation);
			}
			GridGraph gridGraph = graph as GridGraph;
			if (gridGraph != null)
			{
				return new NativeMovementPlane(gridGraph.transform.rotation);
			}
			return new NativeMovementPlane(quaternion.identity);
		}

		// Token: 0x06001675 RID: 5749 RVA: 0x0007CA9C File Offset: 0x0007AC9C
		public static void SampleSmoothNavmeshNormal(TriangleMeshNode node, List<GraphNode> scratchList, NativeList<Int3> scratchBuffer, float3 position, float agentRadius, ref AgentMovementPlane agentMovementPlane, float alpha)
		{
			NativeList<Int3> nativeList = scratchBuffer;
			List<GraphNode> list = scratchList;
			nativeList.Clear();
			list.Clear();
			int i = 0;
			node.TemporaryFlag1 = true;
			list.Add(node);
			int v = node.v0;
			int v2 = node.v1;
			int v3 = node.v2;
			while (i < list.Count)
			{
				TriangleMeshNode triangleMeshNode = list[i++] as TriangleMeshNode;
				if (triangleMeshNode != null && ((triangleMeshNode.v0 == v) | (triangleMeshNode.v1 == v) | (triangleMeshNode.v2 == v) | (triangleMeshNode.v0 == v2) | (triangleMeshNode.v1 == v2) | (triangleMeshNode.v2 == v2) | (triangleMeshNode.v0 == v3) | (triangleMeshNode.v1 == v3) | (triangleMeshNode.v2 == v3)))
				{
					Int3 @int;
					Int3 int2;
					Int3 int3;
					triangleMeshNode.GetVertices(out @int, out int2, out int3);
					nativeList.Add(in @int);
					nativeList.Add(in int2);
					nativeList.Add(in int3);
					triangleMeshNode.GetConnections<List<GraphNode>>(delegate(GraphNode con, ref List<GraphNode> que)
					{
						if (!con.TemporaryFlag1)
						{
							con.TemporaryFlag1 = true;
							que.Add(con);
						}
					}, ref list, 32);
				}
			}
			for (int j = 0; j < list.Count; j++)
			{
				list[j].TemporaryFlag1 = false;
			}
			UnsafeSpan<Int3> unsafeSpan = nativeList.AsUnsafeSpan<Int3>();
			MovementPlaneFromGraphSystem.SampleSmoothTriangleNormal(ref position, ref unsafeSpan, ref agentMovementPlane, agentRadius, alpha);
		}

		// Token: 0x06001676 RID: 5750 RVA: 0x0007CC01 File Offset: 0x0007AE01
		private static float Square(float x)
		{
			return x * x;
		}

		// Token: 0x06001677 RID: 5751 RVA: 0x0007CC08 File Offset: 0x0007AE08
		private static float SinAngle(float3 a, float3 b, float3 c)
		{
			return math.sqrt(1f - MovementPlaneFromGraphSystem.Square(math.dot(math.normalizesafe(a - b, default(float3)), math.normalizesafe(c - b, default(float3)))));
		}

		// Token: 0x06001678 RID: 5752 RVA: 0x0007CC54 File Offset: 0x0007AE54
		[BurstCompile(FloatMode = FloatMode.Fast)]
		private static void SampleSmoothTriangleNormal(ref float3 position, ref UnsafeSpan<Int3> _triangleVertices, ref AgentMovementPlane agentMovementPlane, float agentRadius, float alpha)
		{
			MovementPlaneFromGraphSystem.SampleSmoothTriangleNormal_000014EB$BurstDirectCall.Invoke(ref position, ref _triangleVertices, ref agentMovementPlane, agentRadius, alpha);
		}

		// Token: 0x06001679 RID: 5753 RVA: 0x0007CC64 File Offset: 0x0007AE64
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private void __ScheduleViaJobChunkExtension_0(MovementPlaneFromGraphSystem.JobMovementPlaneFromNavmeshNormal job, EntityQuery query, JobHandle dependency, ref SystemState state, bool hasUserDefinedQuery)
		{
			this.__TypeHandle.__Pathfinding_ECS_MovementPlaneFromGraphSystem_JobMovementPlaneFromNavmeshNormal_WithoutDefaultQuery_JobEntityTypeHandle.UpdateBaseEntityIndexArray(ref job, query, ref state);
			this.__TypeHandle.__Pathfinding_ECS_MovementPlaneFromGraphSystem_JobMovementPlaneFromNavmeshNormal_WithoutDefaultQuery_JobEntityTypeHandle.AssignEntityManager(ref job, state.EntityManager);
			this.__TypeHandle.__Pathfinding_ECS_MovementPlaneFromGraphSystem_JobMovementPlaneFromNavmeshNormal_WithoutDefaultQuery_JobEntityTypeHandle.__TypeHandle.Update(ref state);
			this.__TypeHandle.__Pathfinding_ECS_MovementPlaneFromGraphSystem_JobMovementPlaneFromNavmeshNormal_WithoutDefaultQuery_JobEntityTypeHandle.Run(ref job, query);
		}

		// Token: 0x0600167A RID: 5754 RVA: 0x0007CCCC File Offset: 0x0007AECC
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private JobHandle __ScheduleViaJobChunkExtension_1(MovementPlaneFromGraphSystem.JobMovementPlaneFromGraph job, EntityQuery query, JobHandle dependency, ref SystemState state, bool hasUserDefinedQuery)
		{
			dependency = this.__TypeHandle.__Pathfinding_ECS_MovementPlaneFromGraphSystem_JobMovementPlaneFromGraph_WithoutDefaultQuery_JobEntityTypeHandle.UpdateBaseEntityIndexArray(ref job, query, dependency, ref state);
			this.__TypeHandle.__Pathfinding_ECS_MovementPlaneFromGraphSystem_JobMovementPlaneFromGraph_WithoutDefaultQuery_JobEntityTypeHandle.AssignEntityManager(ref job, state.EntityManager);
			this.__TypeHandle.__Pathfinding_ECS_MovementPlaneFromGraphSystem_JobMovementPlaneFromGraph_WithoutDefaultQuery_JobEntityTypeHandle.__TypeHandle.Update(ref state);
			return this.__TypeHandle.__Pathfinding_ECS_MovementPlaneFromGraphSystem_JobMovementPlaneFromGraph_WithoutDefaultQuery_JobEntityTypeHandle.Schedule(ref job, query, dependency);
		}

		// Token: 0x0600167B RID: 5755 RVA: 0x0007CD38 File Offset: 0x0007AF38
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private void __AssignQueries(ref SystemState state)
		{
			EntityQueryBuilder entityQueryBuilder = new EntityQueryBuilder(Allocator.Temp);
			entityQueryBuilder.Dispose();
		}

		// Token: 0x0600167C RID: 5756 RVA: 0x0007CD59 File Offset: 0x0007AF59
		public void OnCreateForCompiler(ref SystemState state)
		{
			this.__AssignQueries(ref state);
			this.__TypeHandle.__AssignHandles(ref state);
		}

		// Token: 0x0600167D RID: 5757 RVA: 0x0007CD6E File Offset: 0x0007AF6E
		[MonoPInvokeCallback(typeof(SystemBaseDelegates.Function))]
		internal unsafe static void __codegen__OnCreate(IntPtr self, IntPtr state)
		{
			((MovementPlaneFromGraphSystem*)self.ToPointer())->OnCreate(ref *(SystemState*)state.ToPointer());
		}

		// Token: 0x0600167E RID: 5758 RVA: 0x0007CD87 File Offset: 0x0007AF87
		[MonoPInvokeCallback(typeof(SystemBaseDelegates.Function))]
		internal unsafe static void __codegen__OnUpdate(IntPtr self, IntPtr state)
		{
			((MovementPlaneFromGraphSystem*)self.ToPointer())->OnUpdate(ref *(SystemState*)state.ToPointer());
		}

		// Token: 0x0600167F RID: 5759 RVA: 0x0007CDA0 File Offset: 0x0007AFA0
		[MonoPInvokeCallback(typeof(SystemBaseDelegates.Function))]
		internal unsafe static void __codegen__OnDestroy(IntPtr self, IntPtr state)
		{
			((MovementPlaneFromGraphSystem*)self.ToPointer())->OnDestroy(ref *(SystemState*)state.ToPointer());
		}

		// Token: 0x06001680 RID: 5760 RVA: 0x0007CDB9 File Offset: 0x0007AFB9
		[MonoPInvokeCallback(typeof(SystemBaseDelegates.Function))]
		internal unsafe static void __codegen__OnCreateForCompiler(IntPtr self, IntPtr state)
		{
			((MovementPlaneFromGraphSystem*)self.ToPointer())->OnCreateForCompiler(ref *(SystemState*)state.ToPointer());
		}

		// Token: 0x06001681 RID: 5761 RVA: 0x0007CDD4 File Offset: 0x0007AFD4
		[BurstCompile(FloatMode = FloatMode.Fast)]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal unsafe static void SampleSmoothTriangleNormal$BurstManaged(ref float3 position, ref UnsafeSpan<Int3> _triangleVertices, ref AgentMovementPlane agentMovementPlane, float agentRadius, float alpha)
		{
			UnsafeSpan<int3> unsafeSpan = _triangleVertices.Reinterpret<int3>();
			if (unsafeSpan.Length < 3)
			{
				throw new ArgumentException("triangleVertices.Length < 3");
			}
			int3* ptr = unsafeSpan.ptr;
			float3* ptr2;
			checked
			{
				ptr2 = stackalloc float3[unchecked((UIntPtr)3) * (UIntPtr)sizeof(float3)];
			}
			float* ptr3 = stackalloc float[(UIntPtr)12];
			*ptr2 = (ptr2[1] = (ptr2[2] = float3.zero));
			*ptr3 = (ptr3[1] = (ptr3[2] = 0f));
			float3 up = agentMovementPlane.value.up;
			for (uint num = 0U; num < unsafeSpan.length; num += 3U)
			{
				int3 @int = *unsafeSpan[num];
				int3 int2 = *unsafeSpan[num + 1U];
				int3 int3 = *unsafeSpan[num + 2U];
				float3 @float = @int * 0.001f;
				float3 float2 = int2 * 0.001f;
				float3 float3 = int3 * 0.001f;
				float3 float4 = math.normalizesafe(math.cross(float2 - @float, float3 - @float), default(float3));
				float num2 = 1f;
				float num3 = math.dot(float4, up);
				if (num3 < 0.86f)
				{
					float3 float5;
					Polygon.ClosestPointOnTriangleByRef(in @float, in float2, in float3, in position, out float5);
					float num4 = math.lengthsq(float5 - position) / MovementPlaneFromGraphSystem.Square(1.5f * agentRadius);
					float num5 = math.max(0.1f, 1f - num4);
					float num6 = (0.86f - math.max(0f, num3)) / 0.86f;
					num2 = math.lerp(1f, num5, num6);
				}
				for (int i = 0; i < 3; i++)
				{
					if (math.all(@int == ptr[i]))
					{
						float num7 = num2 * MovementPlaneFromGraphSystem.SinAngle(int3, @int, int2);
						ptr3[i] += num7;
						ptr2[i] += num7 * float4;
					}
				}
				for (int j = 0; j < 3; j++)
				{
					if (math.all(int2 == ptr[j]))
					{
						float num8 = num2 * MovementPlaneFromGraphSystem.SinAngle(@int, int2, int3);
						ptr3[j] += num8;
						ptr2[j] += num8 * float4;
					}
				}
				for (int k = 0; k < 3; k++)
				{
					if (math.all(int3 == ptr[k]))
					{
						float num9 = num2 * MovementPlaneFromGraphSystem.SinAngle(int2, int3, @int);
						ptr3[k] += num9;
						ptr2[k] += num9 * float4;
					}
				}
			}
			for (int l = 0; l < 3; l++)
			{
				if (ptr3[l] > 0f)
				{
					ptr2[l] /= ptr3[l];
				}
			}
			float3 float6 = *ptr * 0.001f;
			float3 float7 = ptr[1] * 0.001f;
			float3 float8 = ptr[2] * 0.001f;
			float3 float9 = Polygon.ClosestPointOnTriangleBarycentric(float6, float7, float8, position);
			float3 float10 = math.normalizesafe(*ptr2 * float9.x + ptr2[1] * float9.y + ptr2[2] * float9.z, default(float3));
			float3 float11 = math.lerp(up, float10, math.clamp(alpha, 0f, 1f));
			agentMovementPlane.value = agentMovementPlane.value.MatchUpDirection(float11);
		}

		// Token: 0x04000F3D RID: 3901
		public EntityQuery entityQueryGraph;

		// Token: 0x04000F3E RID: 3902
		public EntityQuery entityQueryNormal;

		// Token: 0x04000F3F RID: 3903
		private GCHandle graphNodeQueue;

		// Token: 0x04000F40 RID: 3904
		private MovementPlaneFromGraphSystem.TypeHandle __TypeHandle;

		// Token: 0x0200037E RID: 894
		[CompilerGenerated]
		private struct JobMovementPlaneFromNavmeshNormal : IJobEntity, IJobChunk
		{
			// Token: 0x06001682 RID: 5762 RVA: 0x0007D264 File Offset: 0x0007B464
			public void Execute(ManagedState managedState, in LocalTransform localTransform, ref AgentMovementPlane agentMovementPlane, in AgentCylinderShape shape)
			{
				TriangleMeshNode triangleMeshNode = managedState.pathTracer.startNode as TriangleMeshNode;
				if (triangleMeshNode != null)
				{
					float num = math.max(0.01f, shape.radius);
					MovementPlaneFromGraphSystem.SampleSmoothNavmeshNormal(triangleMeshNode, this.que, this.vertices, localTransform.Position, num, ref agentMovementPlane, this.dt * 20f);
				}
			}

			// Token: 0x06001683 RID: 5763 RVA: 0x0007D2C0 File Offset: 0x0007B4C0
			[CompilerGenerated]
			public void Execute(in ArchetypeChunk chunk, int chunkIndexInQuery, bool useEnabledMask, in v128 chunkEnabledMask)
			{
				ManagedComponentAccessor<ManagedState> managedComponentAccessor = chunk.GetManagedComponentAccessor<ManagedState>(ref this.__TypeHandle.__Pathfinding_ECS_ManagedState_RW_ComponentTypeHandle, this.__EntityManager);
				IntPtr intPtr = InternalCompilerInterface.UnsafeGetChunkNativeArrayReadOnlyIntPtr<LocalTransform>(chunk, ref this.__TypeHandle.__Unity_Transforms_LocalTransform_RO_ComponentTypeHandle);
				IntPtr intPtr2 = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtr<AgentMovementPlane>(chunk, ref this.__TypeHandle.__Pathfinding_ECS_AgentMovementPlane_RW_ComponentTypeHandle);
				IntPtr intPtr3 = InternalCompilerInterface.UnsafeGetChunkNativeArrayReadOnlyIntPtr<AgentCylinderShape>(chunk, ref this.__TypeHandle.__Pathfinding_ECS_AgentCylinderShape_RO_ComponentTypeHandle);
				int count = chunk.Count;
				int num = 0;
				if (!useEnabledMask)
				{
					for (int i = 0; i < count; i++)
					{
						ManagedState managedState = managedComponentAccessor[i];
						ref LocalTransform ptr = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<LocalTransform>(intPtr, i);
						ref AgentMovementPlane ptr2 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<AgentMovementPlane>(intPtr2, i);
						ref AgentCylinderShape ptr3 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<AgentCylinderShape>(intPtr3, i);
						this.Execute(managedState, in ptr, ref ptr2, in ptr3);
						num++;
					}
					return;
				}
				if (math.countbits(chunkEnabledMask.ULong0 ^ (chunkEnabledMask.ULong0 << 1)) + math.countbits(chunkEnabledMask.ULong1 ^ (chunkEnabledMask.ULong1 << 1)) - 1 <= 4)
				{
					int j = 0;
					int num2 = 0;
					while (InternalCompilerInterface.UnsafeTryGetNextEnabledBitRange(chunkEnabledMask, num2, out j, out num2))
					{
						while (j < num2)
						{
							ManagedState managedState2 = managedComponentAccessor[j];
							ref LocalTransform ptr4 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<LocalTransform>(intPtr, j);
							ref AgentMovementPlane ptr5 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<AgentMovementPlane>(intPtr2, j);
							ref AgentCylinderShape ptr6 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<AgentCylinderShape>(intPtr3, j);
							this.Execute(managedState2, in ptr4, ref ptr5, in ptr6);
							j++;
							num++;
						}
					}
					return;
				}
				ulong num3 = chunkEnabledMask.ULong0;
				int num4 = math.min(64, count);
				for (int k = 0; k < num4; k++)
				{
					if ((num3 & 1UL) != 0UL)
					{
						ManagedState managedState3 = managedComponentAccessor[k];
						ref LocalTransform ptr7 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<LocalTransform>(intPtr, k);
						ref AgentMovementPlane ptr8 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<AgentMovementPlane>(intPtr2, k);
						ref AgentCylinderShape ptr9 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<AgentCylinderShape>(intPtr3, k);
						this.Execute(managedState3, in ptr7, ref ptr8, in ptr9);
						num++;
					}
					num3 >>= 1;
				}
				num3 = chunkEnabledMask.ULong1;
				for (int l = 64; l < count; l++)
				{
					if ((num3 & 1UL) != 0UL)
					{
						ManagedState managedState4 = managedComponentAccessor[l];
						ref LocalTransform ptr10 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<LocalTransform>(intPtr, l);
						ref AgentMovementPlane ptr11 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<AgentMovementPlane>(intPtr2, l);
						ref AgentCylinderShape ptr12 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<AgentCylinderShape>(intPtr3, l);
						this.Execute(managedState4, in ptr10, ref ptr11, in ptr12);
						num++;
					}
					num3 >>= 1;
				}
			}

			// Token: 0x06001684 RID: 5764 RVA: 0x0007D4F8 File Offset: 0x0007B6F8
			private JobHandle __ThrowCodeGenException()
			{
				throw new Exception("This method should have been replaced by source gen.");
			}

			// Token: 0x06001685 RID: 5765 RVA: 0x0007D50F File Offset: 0x0007B70F
			public void Run()
			{
				this.__ThrowCodeGenException();
			}

			// Token: 0x06001686 RID: 5766 RVA: 0x0007D50F File Offset: 0x0007B70F
			public void RunByRef()
			{
				this.__ThrowCodeGenException();
			}

			// Token: 0x06001687 RID: 5767 RVA: 0x0007D50F File Offset: 0x0007B70F
			public void Run(EntityQuery query)
			{
				this.__ThrowCodeGenException();
			}

			// Token: 0x06001688 RID: 5768 RVA: 0x0007D50F File Offset: 0x0007B70F
			public void RunByRef(EntityQuery query)
			{
				this.__ThrowCodeGenException();
			}

			// Token: 0x06001689 RID: 5769 RVA: 0x0007D518 File Offset: 0x0007B718
			public JobHandle Schedule(JobHandle dependsOn)
			{
				return this.__ThrowCodeGenException();
			}

			// Token: 0x0600168A RID: 5770 RVA: 0x0007D518 File Offset: 0x0007B718
			public JobHandle ScheduleByRef(JobHandle dependsOn)
			{
				return this.__ThrowCodeGenException();
			}

			// Token: 0x0600168B RID: 5771 RVA: 0x0007D518 File Offset: 0x0007B718
			public JobHandle Schedule(EntityQuery query, JobHandle dependsOn)
			{
				return this.__ThrowCodeGenException();
			}

			// Token: 0x0600168C RID: 5772 RVA: 0x0007D518 File Offset: 0x0007B718
			public JobHandle ScheduleByRef(EntityQuery query, JobHandle dependsOn)
			{
				return this.__ThrowCodeGenException();
			}

			// Token: 0x0600168D RID: 5773 RVA: 0x0007D50F File Offset: 0x0007B70F
			public void Schedule()
			{
				this.__ThrowCodeGenException();
			}

			// Token: 0x0600168E RID: 5774 RVA: 0x0007D50F File Offset: 0x0007B70F
			public void ScheduleByRef()
			{
				this.__ThrowCodeGenException();
			}

			// Token: 0x0600168F RID: 5775 RVA: 0x0007D50F File Offset: 0x0007B70F
			public void Schedule(EntityQuery query)
			{
				this.__ThrowCodeGenException();
			}

			// Token: 0x06001690 RID: 5776 RVA: 0x0007D50F File Offset: 0x0007B70F
			public void ScheduleByRef(EntityQuery query)
			{
				this.__ThrowCodeGenException();
			}

			// Token: 0x06001691 RID: 5777 RVA: 0x0007D518 File Offset: 0x0007B718
			public JobHandle ScheduleParallel(JobHandle dependsOn)
			{
				return this.__ThrowCodeGenException();
			}

			// Token: 0x06001692 RID: 5778 RVA: 0x0007D518 File Offset: 0x0007B718
			public JobHandle ScheduleParallelByRef(JobHandle dependsOn)
			{
				return this.__ThrowCodeGenException();
			}

			// Token: 0x06001693 RID: 5779 RVA: 0x0007D518 File Offset: 0x0007B718
			public JobHandle ScheduleParallel(EntityQuery query, JobHandle dependsOn)
			{
				return this.__ThrowCodeGenException();
			}

			// Token: 0x06001694 RID: 5780 RVA: 0x0007D518 File Offset: 0x0007B718
			public JobHandle ScheduleParallelByRef(EntityQuery query, JobHandle dependsOn)
			{
				return this.__ThrowCodeGenException();
			}

			// Token: 0x06001695 RID: 5781 RVA: 0x0007D518 File Offset: 0x0007B718
			public JobHandle ScheduleParallel(EntityQuery query, JobHandle dependsOn, NativeArray<int> chunkBaseEntityIndices)
			{
				return this.__ThrowCodeGenException();
			}

			// Token: 0x06001696 RID: 5782 RVA: 0x0007D518 File Offset: 0x0007B718
			public JobHandle ScheduleParallelByRef(EntityQuery query, JobHandle dependsOn, NativeArray<int> chunkBaseEntityIndices)
			{
				return this.__ThrowCodeGenException();
			}

			// Token: 0x06001697 RID: 5783 RVA: 0x0007D50F File Offset: 0x0007B70F
			public void ScheduleParallel()
			{
				this.__ThrowCodeGenException();
			}

			// Token: 0x06001698 RID: 5784 RVA: 0x0007D50F File Offset: 0x0007B70F
			public void ScheduleParallelByRef()
			{
				this.__ThrowCodeGenException();
			}

			// Token: 0x06001699 RID: 5785 RVA: 0x0007D50F File Offset: 0x0007B70F
			public void ScheduleParallel(EntityQuery query)
			{
				this.__ThrowCodeGenException();
			}

			// Token: 0x0600169A RID: 5786 RVA: 0x0007D50F File Offset: 0x0007B70F
			public void ScheduleParallelByRef(EntityQuery query)
			{
				this.__ThrowCodeGenException();
			}

			// Token: 0x0600169B RID: 5787 RVA: 0x0007D520 File Offset: 0x0007B720
			void IJobChunk.Execute(in ArchetypeChunk chunk, int unfilteredChunkIndex, bool useEnabledMask, in v128 chunkEnabledMask)
			{
				this.Execute(in chunk, unfilteredChunkIndex, useEnabledMask, in chunkEnabledMask);
			}

			// Token: 0x04000F41 RID: 3905
			public float dt;

			// Token: 0x04000F42 RID: 3906
			public NativeList<Int3> vertices;

			// Token: 0x04000F43 RID: 3907
			public List<GraphNode> que;

			// Token: 0x04000F44 RID: 3908
			private MovementPlaneFromGraphSystem.JobMovementPlaneFromNavmeshNormal.InternalCompilerQueryAndHandleData.TypeHandle __TypeHandle;

			// Token: 0x04000F45 RID: 3909
			public EntityManager __EntityManager;

			// Token: 0x0200037F RID: 895
			public struct InternalCompilerQueryAndHandleData
			{
				// Token: 0x0600169C RID: 5788 RVA: 0x0007D530 File Offset: 0x0007B730
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				private void __AssignQueries(ref SystemState state)
				{
					EntityQueryBuilder entityQueryBuilder = new EntityQueryBuilder(Allocator.Temp);
					this.DefaultQuery = entityQueryBuilder.WithAll<LocalTransform>().WithAll<AgentCylinderShape>().WithAllRW<ManagedState>()
						.WithAllRW<AgentMovementPlane>()
						.Build(ref state);
					entityQueryBuilder.Reset();
					entityQueryBuilder.Dispose();
				}

				// Token: 0x0600169D RID: 5789 RVA: 0x0007D587 File Offset: 0x0007B787
				public void Init(ref SystemState state, bool assignDefaultQuery)
				{
					if (assignDefaultQuery)
					{
						this.__AssignQueries(ref state);
					}
					this.__TypeHandle.__AssignHandles(ref state);
				}

				// Token: 0x0600169E RID: 5790 RVA: 0x0007D59F File Offset: 0x0007B79F
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public void Run(ref MovementPlaneFromGraphSystem.JobMovementPlaneFromNavmeshNormal job, EntityQuery query)
				{
					job.__TypeHandle = this.__TypeHandle;
					InternalCompilerInterface.JobChunkInterface.RunByRefWithoutJobs<MovementPlaneFromGraphSystem.JobMovementPlaneFromNavmeshNormal>(ref job, query);
				}

				// Token: 0x0600169F RID: 5791 RVA: 0x0007D5B4 File Offset: 0x0007B7B4
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public JobHandle Schedule(ref MovementPlaneFromGraphSystem.JobMovementPlaneFromNavmeshNormal job, EntityQuery query, JobHandle dependency)
				{
					job.__TypeHandle = this.__TypeHandle;
					return (ref job).ScheduleByRef<MovementPlaneFromGraphSystem.JobMovementPlaneFromNavmeshNormal>(query, dependency);
				}

				// Token: 0x060016A0 RID: 5792 RVA: 0x0007D5CA File Offset: 0x0007B7CA
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public JobHandle ScheduleParallel(ref MovementPlaneFromGraphSystem.JobMovementPlaneFromNavmeshNormal job, EntityQuery query, JobHandle dependency)
				{
					job.__TypeHandle = this.__TypeHandle;
					return (ref job).ScheduleParallelByRef<MovementPlaneFromGraphSystem.JobMovementPlaneFromNavmeshNormal>(query, dependency);
				}

				// Token: 0x060016A1 RID: 5793 RVA: 0x00003788 File Offset: 0x00001988
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public void UpdateBaseEntityIndexArray(ref MovementPlaneFromGraphSystem.JobMovementPlaneFromNavmeshNormal job, EntityQuery query, ref SystemState state)
				{
				}

				// Token: 0x060016A2 RID: 5794 RVA: 0x000743B8 File Offset: 0x000725B8
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public JobHandle UpdateBaseEntityIndexArray(ref MovementPlaneFromGraphSystem.JobMovementPlaneFromNavmeshNormal job, EntityQuery query, JobHandle dependency, ref SystemState state)
				{
					return dependency;
				}

				// Token: 0x060016A3 RID: 5795 RVA: 0x0007D5E0 File Offset: 0x0007B7E0
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public void AssignEntityManager(ref MovementPlaneFromGraphSystem.JobMovementPlaneFromNavmeshNormal job, EntityManager entityManager)
				{
					job.__EntityManager = entityManager;
				}

				// Token: 0x04000F46 RID: 3910
				public MovementPlaneFromGraphSystem.JobMovementPlaneFromNavmeshNormal.InternalCompilerQueryAndHandleData.TypeHandle __TypeHandle;

				// Token: 0x04000F47 RID: 3911
				public EntityQuery DefaultQuery;

				// Token: 0x02000380 RID: 896
				public struct TypeHandle
				{
					// Token: 0x060016A4 RID: 5796 RVA: 0x0007D5EC File Offset: 0x0007B7EC
					[MethodImpl(MethodImplOptions.AggressiveInlining)]
					public void __AssignHandles(ref SystemState state)
					{
						this.__Pathfinding_ECS_ManagedState_RW_ComponentTypeHandle = state.EntityManager.GetComponentTypeHandle<ManagedState>(false);
						this.__Unity_Transforms_LocalTransform_RO_ComponentTypeHandle = state.GetComponentTypeHandle<LocalTransform>(true);
						this.__Pathfinding_ECS_AgentMovementPlane_RW_ComponentTypeHandle = state.GetComponentTypeHandle<AgentMovementPlane>(false);
						this.__Pathfinding_ECS_AgentCylinderShape_RO_ComponentTypeHandle = state.GetComponentTypeHandle<AgentCylinderShape>(true);
					}

					// Token: 0x060016A5 RID: 5797 RVA: 0x0007D635 File Offset: 0x0007B835
					public void Update(ref SystemState state)
					{
						this.__Pathfinding_ECS_ManagedState_RW_ComponentTypeHandle.Update(ref state);
						this.__Unity_Transforms_LocalTransform_RO_ComponentTypeHandle.Update(ref state);
						this.__Pathfinding_ECS_AgentMovementPlane_RW_ComponentTypeHandle.Update(ref state);
						this.__Pathfinding_ECS_AgentCylinderShape_RO_ComponentTypeHandle.Update(ref state);
					}

					// Token: 0x04000F48 RID: 3912
					public ComponentTypeHandle<ManagedState> __Pathfinding_ECS_ManagedState_RW_ComponentTypeHandle;

					// Token: 0x04000F49 RID: 3913
					[ReadOnly]
					public ComponentTypeHandle<LocalTransform> __Unity_Transforms_LocalTransform_RO_ComponentTypeHandle;

					// Token: 0x04000F4A RID: 3914
					public ComponentTypeHandle<AgentMovementPlane> __Pathfinding_ECS_AgentMovementPlane_RW_ComponentTypeHandle;

					// Token: 0x04000F4B RID: 3915
					[ReadOnly]
					public ComponentTypeHandle<AgentCylinderShape> __Pathfinding_ECS_AgentCylinderShape_RO_ComponentTypeHandle;
				}
			}

			// Token: 0x02000381 RID: 897
			public struct InternalCompiler
			{
				// Token: 0x060016A6 RID: 5798 RVA: 0x000768D5 File Offset: 0x00074AD5
				[Conditional("ENABLE_UNITY_COLLECTIONS_CHECKS")]
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public static void CheckForErrors(int scheduleType)
				{
					if (scheduleType == 2)
					{
						throw new InvalidOperationException("Tried to ScheduleParallel a job with a managed execute signature. Please use .Run or .Schedule instead.");
					}
				}
			}
		}

		// Token: 0x02000382 RID: 898
		[BurstCompile]
		[CompilerGenerated]
		private struct JobMovementPlaneFromGraph : IJobEntity, IJobChunk
		{
			// Token: 0x060016A7 RID: 5799 RVA: 0x0007D668 File Offset: 0x0007B868
			public void Execute(in MovementState movementState, ref AgentMovementPlane movementPlane)
			{
				MovementState movementState2 = movementState;
				if (movementState2.graphIndex < (uint)this.movementPlanes.Length)
				{
					movementState2 = movementState;
					movementPlane = this.movementPlanes[(int)movementState2.graphIndex];
				}
			}

			// Token: 0x060016A8 RID: 5800 RVA: 0x0007D6B0 File Offset: 0x0007B8B0
			[CompilerGenerated]
			public void Execute(in ArchetypeChunk chunk, int chunkIndexInQuery, bool useEnabledMask, in v128 chunkEnabledMask)
			{
				IntPtr intPtr = InternalCompilerInterface.UnsafeGetChunkNativeArrayReadOnlyIntPtr<MovementState>(chunk, ref this.__TypeHandle.__Pathfinding_ECS_MovementState_RO_ComponentTypeHandle);
				IntPtr intPtr2 = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtr<AgentMovementPlane>(chunk, ref this.__TypeHandle.__Pathfinding_ECS_AgentMovementPlane_RW_ComponentTypeHandle);
				int count = chunk.Count;
				int num = 0;
				if (!useEnabledMask)
				{
					for (int i = 0; i < count; i++)
					{
						ref MovementState ptr = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<MovementState>(intPtr, i);
						ref AgentMovementPlane ptr2 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<AgentMovementPlane>(intPtr2, i);
						this.Execute(in ptr, ref ptr2);
						num++;
					}
					return;
				}
				if (math.countbits(chunkEnabledMask.ULong0 ^ (chunkEnabledMask.ULong0 << 1)) + math.countbits(chunkEnabledMask.ULong1 ^ (chunkEnabledMask.ULong1 << 1)) - 1 <= 4)
				{
					int j = 0;
					int num2 = 0;
					while (InternalCompilerInterface.UnsafeTryGetNextEnabledBitRange(chunkEnabledMask, num2, out j, out num2))
					{
						while (j < num2)
						{
							ref MovementState ptr3 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<MovementState>(intPtr, j);
							ref AgentMovementPlane ptr4 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<AgentMovementPlane>(intPtr2, j);
							this.Execute(in ptr3, ref ptr4);
							j++;
							num++;
						}
					}
					return;
				}
				ulong num3 = chunkEnabledMask.ULong0;
				int num4 = math.min(64, count);
				for (int k = 0; k < num4; k++)
				{
					if ((num3 & 1UL) != 0UL)
					{
						ref MovementState ptr5 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<MovementState>(intPtr, k);
						ref AgentMovementPlane ptr6 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<AgentMovementPlane>(intPtr2, k);
						this.Execute(in ptr5, ref ptr6);
						num++;
					}
					num3 >>= 1;
				}
				num3 = chunkEnabledMask.ULong1;
				for (int l = 64; l < count; l++)
				{
					if ((num3 & 1UL) != 0UL)
					{
						ref MovementState ptr7 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<MovementState>(intPtr, l);
						ref AgentMovementPlane ptr8 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<AgentMovementPlane>(intPtr2, l);
						this.Execute(in ptr7, ref ptr8);
						num++;
					}
					num3 >>= 1;
				}
			}

			// Token: 0x060016A9 RID: 5801 RVA: 0x0007D848 File Offset: 0x0007BA48
			private JobHandle __ThrowCodeGenException()
			{
				throw new Exception("This method should have been replaced by source gen.");
			}

			// Token: 0x060016AA RID: 5802 RVA: 0x0007D85F File Offset: 0x0007BA5F
			public void Run()
			{
				this.__ThrowCodeGenException();
			}

			// Token: 0x060016AB RID: 5803 RVA: 0x0007D85F File Offset: 0x0007BA5F
			public void RunByRef()
			{
				this.__ThrowCodeGenException();
			}

			// Token: 0x060016AC RID: 5804 RVA: 0x0007D85F File Offset: 0x0007BA5F
			public void Run(EntityQuery query)
			{
				this.__ThrowCodeGenException();
			}

			// Token: 0x060016AD RID: 5805 RVA: 0x0007D85F File Offset: 0x0007BA5F
			public void RunByRef(EntityQuery query)
			{
				this.__ThrowCodeGenException();
			}

			// Token: 0x060016AE RID: 5806 RVA: 0x0007D868 File Offset: 0x0007BA68
			public JobHandle Schedule(JobHandle dependsOn)
			{
				return this.__ThrowCodeGenException();
			}

			// Token: 0x060016AF RID: 5807 RVA: 0x0007D868 File Offset: 0x0007BA68
			public JobHandle ScheduleByRef(JobHandle dependsOn)
			{
				return this.__ThrowCodeGenException();
			}

			// Token: 0x060016B0 RID: 5808 RVA: 0x0007D868 File Offset: 0x0007BA68
			public JobHandle Schedule(EntityQuery query, JobHandle dependsOn)
			{
				return this.__ThrowCodeGenException();
			}

			// Token: 0x060016B1 RID: 5809 RVA: 0x0007D868 File Offset: 0x0007BA68
			public JobHandle ScheduleByRef(EntityQuery query, JobHandle dependsOn)
			{
				return this.__ThrowCodeGenException();
			}

			// Token: 0x060016B2 RID: 5810 RVA: 0x0007D85F File Offset: 0x0007BA5F
			public void Schedule()
			{
				this.__ThrowCodeGenException();
			}

			// Token: 0x060016B3 RID: 5811 RVA: 0x0007D85F File Offset: 0x0007BA5F
			public void ScheduleByRef()
			{
				this.__ThrowCodeGenException();
			}

			// Token: 0x060016B4 RID: 5812 RVA: 0x0007D85F File Offset: 0x0007BA5F
			public void Schedule(EntityQuery query)
			{
				this.__ThrowCodeGenException();
			}

			// Token: 0x060016B5 RID: 5813 RVA: 0x0007D85F File Offset: 0x0007BA5F
			public void ScheduleByRef(EntityQuery query)
			{
				this.__ThrowCodeGenException();
			}

			// Token: 0x060016B6 RID: 5814 RVA: 0x0007D868 File Offset: 0x0007BA68
			public JobHandle ScheduleParallel(JobHandle dependsOn)
			{
				return this.__ThrowCodeGenException();
			}

			// Token: 0x060016B7 RID: 5815 RVA: 0x0007D868 File Offset: 0x0007BA68
			public JobHandle ScheduleParallelByRef(JobHandle dependsOn)
			{
				return this.__ThrowCodeGenException();
			}

			// Token: 0x060016B8 RID: 5816 RVA: 0x0007D868 File Offset: 0x0007BA68
			public JobHandle ScheduleParallel(EntityQuery query, JobHandle dependsOn)
			{
				return this.__ThrowCodeGenException();
			}

			// Token: 0x060016B9 RID: 5817 RVA: 0x0007D868 File Offset: 0x0007BA68
			public JobHandle ScheduleParallelByRef(EntityQuery query, JobHandle dependsOn)
			{
				return this.__ThrowCodeGenException();
			}

			// Token: 0x060016BA RID: 5818 RVA: 0x0007D868 File Offset: 0x0007BA68
			public JobHandle ScheduleParallel(EntityQuery query, JobHandle dependsOn, NativeArray<int> chunkBaseEntityIndices)
			{
				return this.__ThrowCodeGenException();
			}

			// Token: 0x060016BB RID: 5819 RVA: 0x0007D868 File Offset: 0x0007BA68
			public JobHandle ScheduleParallelByRef(EntityQuery query, JobHandle dependsOn, NativeArray<int> chunkBaseEntityIndices)
			{
				return this.__ThrowCodeGenException();
			}

			// Token: 0x060016BC RID: 5820 RVA: 0x0007D85F File Offset: 0x0007BA5F
			public void ScheduleParallel()
			{
				this.__ThrowCodeGenException();
			}

			// Token: 0x060016BD RID: 5821 RVA: 0x0007D85F File Offset: 0x0007BA5F
			public void ScheduleParallelByRef()
			{
				this.__ThrowCodeGenException();
			}

			// Token: 0x060016BE RID: 5822 RVA: 0x0007D85F File Offset: 0x0007BA5F
			public void ScheduleParallel(EntityQuery query)
			{
				this.__ThrowCodeGenException();
			}

			// Token: 0x060016BF RID: 5823 RVA: 0x0007D85F File Offset: 0x0007BA5F
			public void ScheduleParallelByRef(EntityQuery query)
			{
				this.__ThrowCodeGenException();
			}

			// Token: 0x060016C0 RID: 5824 RVA: 0x0007D870 File Offset: 0x0007BA70
			void IJobChunk.Execute(in ArchetypeChunk chunk, int unfilteredChunkIndex, bool useEnabledMask, in v128 chunkEnabledMask)
			{
				this.Execute(in chunk, unfilteredChunkIndex, useEnabledMask, in chunkEnabledMask);
			}

			// Token: 0x04000F4C RID: 3916
			[ReadOnly]
			public NativeArray<AgentMovementPlane> movementPlanes;

			// Token: 0x04000F4D RID: 3917
			private MovementPlaneFromGraphSystem.JobMovementPlaneFromGraph.InternalCompilerQueryAndHandleData.TypeHandle __TypeHandle;

			// Token: 0x02000383 RID: 899
			public struct InternalCompilerQueryAndHandleData
			{
				// Token: 0x060016C1 RID: 5825 RVA: 0x0007D880 File Offset: 0x0007BA80
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				private void __AssignQueries(ref SystemState state)
				{
					EntityQueryBuilder entityQueryBuilder = new EntityQueryBuilder(Allocator.Temp);
					this.DefaultQuery = entityQueryBuilder.WithAll<MovementState>().WithAllRW<AgentMovementPlane>().Build(ref state);
					entityQueryBuilder.Reset();
					entityQueryBuilder.Dispose();
				}

				// Token: 0x060016C2 RID: 5826 RVA: 0x0007D8C7 File Offset: 0x0007BAC7
				public void Init(ref SystemState state, bool assignDefaultQuery)
				{
					if (assignDefaultQuery)
					{
						this.__AssignQueries(ref state);
					}
					this.__TypeHandle.__AssignHandles(ref state);
				}

				// Token: 0x060016C3 RID: 5827 RVA: 0x0007D8DF File Offset: 0x0007BADF
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public void Run(ref MovementPlaneFromGraphSystem.JobMovementPlaneFromGraph job, EntityQuery query)
				{
					job.__TypeHandle = this.__TypeHandle;
					(ref job).RunByRef<MovementPlaneFromGraphSystem.JobMovementPlaneFromGraph>(query);
				}

				// Token: 0x060016C4 RID: 5828 RVA: 0x0007D8F4 File Offset: 0x0007BAF4
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public JobHandle Schedule(ref MovementPlaneFromGraphSystem.JobMovementPlaneFromGraph job, EntityQuery query, JobHandle dependency)
				{
					job.__TypeHandle = this.__TypeHandle;
					return (ref job).ScheduleByRef<MovementPlaneFromGraphSystem.JobMovementPlaneFromGraph>(query, dependency);
				}

				// Token: 0x060016C5 RID: 5829 RVA: 0x0007D90A File Offset: 0x0007BB0A
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public JobHandle ScheduleParallel(ref MovementPlaneFromGraphSystem.JobMovementPlaneFromGraph job, EntityQuery query, JobHandle dependency)
				{
					job.__TypeHandle = this.__TypeHandle;
					return (ref job).ScheduleParallelByRef<MovementPlaneFromGraphSystem.JobMovementPlaneFromGraph>(query, dependency);
				}

				// Token: 0x060016C6 RID: 5830 RVA: 0x00003788 File Offset: 0x00001988
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public void UpdateBaseEntityIndexArray(ref MovementPlaneFromGraphSystem.JobMovementPlaneFromGraph job, EntityQuery query, ref SystemState state)
				{
				}

				// Token: 0x060016C7 RID: 5831 RVA: 0x000743B8 File Offset: 0x000725B8
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public JobHandle UpdateBaseEntityIndexArray(ref MovementPlaneFromGraphSystem.JobMovementPlaneFromGraph job, EntityQuery query, JobHandle dependency, ref SystemState state)
				{
					return dependency;
				}

				// Token: 0x060016C8 RID: 5832 RVA: 0x00003788 File Offset: 0x00001988
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public void AssignEntityManager(ref MovementPlaneFromGraphSystem.JobMovementPlaneFromGraph job, EntityManager entityManager)
				{
				}

				// Token: 0x04000F4E RID: 3918
				public MovementPlaneFromGraphSystem.JobMovementPlaneFromGraph.InternalCompilerQueryAndHandleData.TypeHandle __TypeHandle;

				// Token: 0x04000F4F RID: 3919
				public EntityQuery DefaultQuery;

				// Token: 0x02000384 RID: 900
				public struct TypeHandle
				{
					// Token: 0x060016C9 RID: 5833 RVA: 0x0007D920 File Offset: 0x0007BB20
					[MethodImpl(MethodImplOptions.AggressiveInlining)]
					public void __AssignHandles(ref SystemState state)
					{
						this.__Pathfinding_ECS_MovementState_RO_ComponentTypeHandle = state.GetComponentTypeHandle<MovementState>(true);
						this.__Pathfinding_ECS_AgentMovementPlane_RW_ComponentTypeHandle = state.GetComponentTypeHandle<AgentMovementPlane>(false);
					}

					// Token: 0x060016CA RID: 5834 RVA: 0x0007D93C File Offset: 0x0007BB3C
					public void Update(ref SystemState state)
					{
						this.__Pathfinding_ECS_MovementState_RO_ComponentTypeHandle.Update(ref state);
						this.__Pathfinding_ECS_AgentMovementPlane_RW_ComponentTypeHandle.Update(ref state);
					}

					// Token: 0x04000F50 RID: 3920
					[ReadOnly]
					public ComponentTypeHandle<MovementState> __Pathfinding_ECS_MovementState_RO_ComponentTypeHandle;

					// Token: 0x04000F51 RID: 3921
					public ComponentTypeHandle<AgentMovementPlane> __Pathfinding_ECS_AgentMovementPlane_RW_ComponentTypeHandle;
				}
			}

			// Token: 0x02000385 RID: 901
			public struct InternalCompiler
			{
				// Token: 0x060016CB RID: 5835 RVA: 0x00003788 File Offset: 0x00001988
				[Conditional("ENABLE_UNITY_COLLECTIONS_CHECKS")]
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public static void CheckForErrors(int scheduleType)
				{
				}
			}
		}

		// Token: 0x02000386 RID: 902
		private struct TypeHandle
		{
			// Token: 0x060016CC RID: 5836 RVA: 0x0007D956 File Offset: 0x0007BB56
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public void __AssignHandles(ref SystemState state)
			{
				this.__Pathfinding_ECS_MovementPlaneFromGraphSystem_JobMovementPlaneFromNavmeshNormal_WithoutDefaultQuery_JobEntityTypeHandle.Init(ref state, false);
				this.__Pathfinding_ECS_MovementPlaneFromGraphSystem_JobMovementPlaneFromGraph_WithoutDefaultQuery_JobEntityTypeHandle.Init(ref state, false);
			}

			// Token: 0x04000F52 RID: 3922
			public MovementPlaneFromGraphSystem.JobMovementPlaneFromNavmeshNormal.InternalCompilerQueryAndHandleData __Pathfinding_ECS_MovementPlaneFromGraphSystem_JobMovementPlaneFromNavmeshNormal_WithoutDefaultQuery_JobEntityTypeHandle;

			// Token: 0x04000F53 RID: 3923
			public MovementPlaneFromGraphSystem.JobMovementPlaneFromGraph.InternalCompilerQueryAndHandleData __Pathfinding_ECS_MovementPlaneFromGraphSystem_JobMovementPlaneFromGraph_WithoutDefaultQuery_JobEntityTypeHandle;
		}

		// Token: 0x02000388 RID: 904
		// (Invoke) Token: 0x060016D1 RID: 5841
		internal delegate void SampleSmoothTriangleNormal_000014EB$PostfixBurstDelegate(ref float3 position, ref UnsafeSpan<Int3> _triangleVertices, ref AgentMovementPlane agentMovementPlane, float agentRadius, float alpha);

		// Token: 0x02000389 RID: 905
		internal static class SampleSmoothTriangleNormal_000014EB$BurstDirectCall
		{
			// Token: 0x060016D4 RID: 5844 RVA: 0x0007D997 File Offset: 0x0007BB97
			[BurstDiscard]
			private static void GetFunctionPointerDiscard(ref IntPtr A_0)
			{
				if (MovementPlaneFromGraphSystem.SampleSmoothTriangleNormal_000014EB$BurstDirectCall.Pointer == 0)
				{
					MovementPlaneFromGraphSystem.SampleSmoothTriangleNormal_000014EB$BurstDirectCall.Pointer = BurstCompiler.GetILPPMethodFunctionPointer2(MovementPlaneFromGraphSystem.SampleSmoothTriangleNormal_000014EB$BurstDirectCall.DeferredCompilation, methodof(MovementPlaneFromGraphSystem.SampleSmoothTriangleNormal$BurstManaged(ref float3, ref UnsafeSpan<Int3>, ref AgentMovementPlane, float, float)).MethodHandle, typeof(MovementPlaneFromGraphSystem.SampleSmoothTriangleNormal_000014EB$PostfixBurstDelegate).TypeHandle);
				}
				A_0 = MovementPlaneFromGraphSystem.SampleSmoothTriangleNormal_000014EB$BurstDirectCall.Pointer;
			}

			// Token: 0x060016D5 RID: 5845 RVA: 0x0007D9C4 File Offset: 0x0007BBC4
			private static IntPtr GetFunctionPointer()
			{
				IntPtr intPtr = (IntPtr)0;
				MovementPlaneFromGraphSystem.SampleSmoothTriangleNormal_000014EB$BurstDirectCall.GetFunctionPointerDiscard(ref intPtr);
				return intPtr;
			}

			// Token: 0x060016D6 RID: 5846 RVA: 0x0007D9DC File Offset: 0x0007BBDC
			public static void Constructor()
			{
				MovementPlaneFromGraphSystem.SampleSmoothTriangleNormal_000014EB$BurstDirectCall.DeferredCompilation = BurstCompiler.CompileILPPMethod2(methodof(MovementPlaneFromGraphSystem.SampleSmoothTriangleNormal(ref float3, ref UnsafeSpan<Int3>, ref AgentMovementPlane, float, float)).MethodHandle);
			}

			// Token: 0x060016D7 RID: 5847 RVA: 0x00003788 File Offset: 0x00001988
			public static void Initialize()
			{
			}

			// Token: 0x060016D8 RID: 5848 RVA: 0x0007D9ED File Offset: 0x0007BBED
			// Note: this type is marked as 'beforefieldinit'.
			static SampleSmoothTriangleNormal_000014EB$BurstDirectCall()
			{
				MovementPlaneFromGraphSystem.SampleSmoothTriangleNormal_000014EB$BurstDirectCall.Constructor();
			}

			// Token: 0x060016D9 RID: 5849 RVA: 0x0007D9F4 File Offset: 0x0007BBF4
			public static void Invoke(ref float3 position, ref UnsafeSpan<Int3> _triangleVertices, ref AgentMovementPlane agentMovementPlane, float agentRadius, float alpha)
			{
				if (BurstCompiler.IsEnabled)
				{
					IntPtr functionPointer = MovementPlaneFromGraphSystem.SampleSmoothTriangleNormal_000014EB$BurstDirectCall.GetFunctionPointer();
					if (functionPointer != 0)
					{
						calli(System.Void(Unity.Mathematics.float3&,Pathfinding.Collections.UnsafeSpan`1<Pathfinding.Int3>&,Pathfinding.ECS.AgentMovementPlane&,System.Single,System.Single), ref position, ref _triangleVertices, ref agentMovementPlane, agentRadius, alpha, functionPointer);
						return;
					}
				}
				MovementPlaneFromGraphSystem.SampleSmoothTriangleNormal$BurstManaged(ref position, ref _triangleVertices, ref agentMovementPlane, agentRadius, alpha);
			}

			// Token: 0x04000F56 RID: 3926
			private static IntPtr Pointer;

			// Token: 0x04000F57 RID: 3927
			private static IntPtr DeferredCompilation;
		}
	}
}
