using System;
using System.Runtime.CompilerServices;
using Unity.Burst;
using Unity.Burst.Intrinsics;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;

namespace Unity.Transforms
{
	// Token: 0x0200000F RID: 15
	[BurstCompile]
	[RequireMatchingQueriesForUpdate]
	[WorldSystemFilter(WorldSystemFilterFlags.Default | WorldSystemFilterFlags.Editor, WorldSystemFilterFlags.Default)]
	[UpdateInGroup(typeof(TransformSystemGroup))]
	[UpdateAfter(typeof(ParentSystem))]
	[CompilerGenerated]
	public struct LocalToWorldSystem : ISystem, ISystemCompilerGenerated
	{
		// Token: 0x06000035 RID: 53 RVA: 0x00002E7C File Offset: 0x0000107C
		[BurstCompile]
		public void OnCreate(ref SystemState state)
		{
			EntityQueryBuilder entityQueryBuilder = new EntityQueryBuilder(Allocator.Temp).WithAll<LocalTransform>().WithAllRW<LocalToWorld>().WithNone<Parent>()
				.WithOptions(EntityQueryOptions.FilterWriteGroup);
			this._worldSpaceQuery = state.GetEntityQuery(in entityQueryBuilder);
			entityQueryBuilder = new EntityQueryBuilder(Allocator.Temp).WithAll<LocalTransform, Child>().WithAllRW<LocalToWorld>().WithNone<Parent>();
			this._hierarchyRootsQuery = state.GetEntityQuery(in entityQueryBuilder);
			entityQueryBuilder = new EntityQueryBuilder(Allocator.Temp).WithAll<LocalTransform, Parent>().WithAllRW<LocalToWorld>().WithOptions(EntityQueryOptions.FilterWriteGroup);
			this._localToWorldWriteGroupMask = state.GetEntityQuery(in entityQueryBuilder).GetEntityQueryMask();
			this._localTransformTypeHandleRO = state.GetComponentTypeHandle<LocalTransform>(true);
			this._postTransformMatrixTypeHandleRO = state.GetComponentTypeHandle<PostTransformMatrix>(true);
			this._localToWorldTypeHandleRW = state.GetComponentTypeHandle<LocalToWorld>(false);
			this._childLookupRO = state.GetBufferLookup<Child>(true);
			this._localTransformLookupRO = state.GetComponentLookup<LocalTransform>(true);
			this._postTransformMatrixLookupRO = state.GetComponentLookup<PostTransformMatrix>(true);
			this._localToWorldLookupRW = state.GetComponentLookup<LocalToWorld>(false);
		}

		// Token: 0x06000036 RID: 54 RVA: 0x00002F8C File Offset: 0x0000118C
		[BurstCompile]
		public void OnUpdate(ref SystemState state)
		{
			this._localTransformTypeHandleRO.Update(ref state);
			this._postTransformMatrixTypeHandleRO.Update(ref state);
			this._localToWorldTypeHandleRW.Update(ref state);
			this._childLookupRO.Update(ref state);
			this._localTransformLookupRO.Update(ref state);
			this._postTransformMatrixLookupRO.Update(ref state);
			this._localToWorldLookupRW.Update(ref state);
			LocalToWorldSystem.ComputeWorldSpaceLocalToWorldJob computeWorldSpaceLocalToWorldJob = default(LocalToWorldSystem.ComputeWorldSpaceLocalToWorldJob);
			computeWorldSpaceLocalToWorldJob.LocalTransformTypeHandleRO = this._localTransformTypeHandleRO;
			computeWorldSpaceLocalToWorldJob.PostTransformMatrixTypeHandleRO = this._postTransformMatrixTypeHandleRO;
			computeWorldSpaceLocalToWorldJob.LocalToWorldTypeHandleRW = this._localToWorldTypeHandleRW;
			computeWorldSpaceLocalToWorldJob.LastSystemVersion = state.LastSystemVersion;
			LocalToWorldSystem.ComputeWorldSpaceLocalToWorldJob computeWorldSpaceLocalToWorldJob2 = computeWorldSpaceLocalToWorldJob;
			JobHandle jobHandle = (ref computeWorldSpaceLocalToWorldJob2).ScheduleParallelByRef<LocalToWorldSystem.ComputeWorldSpaceLocalToWorldJob>(this._worldSpaceQuery, state.Dependency);
			if (this._hierarchyRootsQuery.IsEmptyIgnoreFilter)
			{
				state.Dependency = jobHandle;
				return;
			}
			JobHandle jobHandle2;
			NativeList<Entity> nativeList = this._hierarchyRootsQuery.ToEntityListAsync(state.WorldUpdateAllocator, state.Dependency, out jobHandle2);
			LocalToWorldSystem.ComputeHierarchyLocalToWorldJob computeHierarchyLocalToWorldJob = default(LocalToWorldSystem.ComputeHierarchyLocalToWorldJob);
			computeHierarchyLocalToWorldJob.RootEntities = nativeList.AsDeferredJobArray();
			computeHierarchyLocalToWorldJob.LocalToWorldWriteGroupMask = this._localToWorldWriteGroupMask;
			computeHierarchyLocalToWorldJob.ChildLookupRO = this._childLookupRO;
			computeHierarchyLocalToWorldJob.LocalTransformLookupRO = this._localTransformLookupRO;
			computeHierarchyLocalToWorldJob.PostTransformMatrixLookupRO = this._postTransformMatrixLookupRO;
			computeHierarchyLocalToWorldJob.LocalToWorldLookupRW = this._localToWorldLookupRW;
			computeHierarchyLocalToWorldJob.LastSystemVersion = state.LastSystemVersion;
			LocalToWorldSystem.ComputeHierarchyLocalToWorldJob computeHierarchyLocalToWorldJob2 = computeHierarchyLocalToWorldJob;
			state.Dependency = (ref computeHierarchyLocalToWorldJob2).ScheduleByRef<LocalToWorldSystem.ComputeHierarchyLocalToWorldJob, Entity>(nativeList, 1, JobHandle.CombineDependencies(jobHandle, jobHandle2));
		}

		// Token: 0x06000037 RID: 55 RVA: 0x0000283B File Offset: 0x00000A3B
		public void OnCreateForCompiler(ref SystemState state)
		{
		}

		// Token: 0x06000038 RID: 56 RVA: 0x000030F2 File Offset: 0x000012F2
		[BurstCompile]
		[MonoPInvokeCallback(typeof(SystemBaseDelegates.Function))]
		internal static void __codegen__OnCreate(IntPtr self, IntPtr state)
		{
			LocalToWorldSystem.__codegen__OnCreate_00000022$BurstDirectCall.Invoke(self, state);
		}

		// Token: 0x06000039 RID: 57 RVA: 0x000030FB File Offset: 0x000012FB
		[BurstCompile]
		[MonoPInvokeCallback(typeof(SystemBaseDelegates.Function))]
		internal static void __codegen__OnUpdate(IntPtr self, IntPtr state)
		{
			LocalToWorldSystem.__codegen__OnUpdate_00000023$BurstDirectCall.Invoke(self, state);
		}

		// Token: 0x0600003A RID: 58 RVA: 0x00003104 File Offset: 0x00001304
		[MonoPInvokeCallback(typeof(SystemBaseDelegates.Function))]
		internal unsafe static void __codegen__OnCreateForCompiler(IntPtr self, IntPtr state)
		{
			((LocalToWorldSystem*)self.ToPointer())->OnCreateForCompiler(ref *(SystemState*)state.ToPointer());
		}

		// Token: 0x0600003B RID: 59 RVA: 0x0000311D File Offset: 0x0000131D
		[BurstCompile]
		[MonoPInvokeCallback(typeof(SystemBaseDelegates.Function))]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal unsafe static void __codegen__OnCreate$BurstManaged(IntPtr self, IntPtr state)
		{
			((LocalToWorldSystem*)self.ToPointer())->OnCreate(ref *(SystemState*)state.ToPointer());
		}

		// Token: 0x0600003C RID: 60 RVA: 0x00003136 File Offset: 0x00001336
		[BurstCompile]
		[MonoPInvokeCallback(typeof(SystemBaseDelegates.Function))]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal unsafe static void __codegen__OnUpdate$BurstManaged(IntPtr self, IntPtr state)
		{
			((LocalToWorldSystem*)self.ToPointer())->OnUpdate(ref *(SystemState*)state.ToPointer());
		}

		// Token: 0x0400002E RID: 46
		private EntityQuery _worldSpaceQuery;

		// Token: 0x0400002F RID: 47
		private EntityQuery _hierarchyRootsQuery;

		// Token: 0x04000030 RID: 48
		private EntityQueryMask _localToWorldWriteGroupMask;

		// Token: 0x04000031 RID: 49
		private ComponentTypeHandle<LocalTransform> _localTransformTypeHandleRO;

		// Token: 0x04000032 RID: 50
		private ComponentTypeHandle<PostTransformMatrix> _postTransformMatrixTypeHandleRO;

		// Token: 0x04000033 RID: 51
		private ComponentTypeHandle<LocalToWorld> _localToWorldTypeHandleRW;

		// Token: 0x04000034 RID: 52
		private BufferLookup<Child> _childLookupRO;

		// Token: 0x04000035 RID: 53
		private ComponentLookup<LocalTransform> _localTransformLookupRO;

		// Token: 0x04000036 RID: 54
		private ComponentLookup<PostTransformMatrix> _postTransformMatrixLookupRO;

		// Token: 0x04000037 RID: 55
		private ComponentLookup<LocalToWorld> _localToWorldLookupRW;

		// Token: 0x02000010 RID: 16
		[BurstCompile]
		private struct ComputeWorldSpaceLocalToWorldJob : IJobChunk
		{
			// Token: 0x0600003D RID: 61 RVA: 0x00003150 File Offset: 0x00001350
			public unsafe void Execute(in ArchetypeChunk chunk, int unfilteredChunkIndex, bool useEnabledMask, in v128 chunkEnabledMask)
			{
				LocalTransform* requiredComponentDataPtrRO = (LocalTransform*)chunk.GetRequiredComponentDataPtrRO<LocalTransform>(ref this.LocalTransformTypeHandleRO);
				if (chunk.DidChange<LocalTransform>(ref this.LocalTransformTypeHandleRO, this.LastSystemVersion) || chunk.DidChange<PostTransformMatrix>(ref this.PostTransformMatrixTypeHandleRO, this.LastSystemVersion))
				{
					LocalToWorld* requiredComponentDataPtrRW = (LocalToWorld*)chunk.GetRequiredComponentDataPtrRW<LocalToWorld>(ref this.LocalToWorldTypeHandleRW);
					PostTransformMatrix* componentDataPtrRO = chunk.GetComponentDataPtrRO<PostTransformMatrix>(ref this.PostTransformMatrixTypeHandleRO);
					if (componentDataPtrRO != null)
					{
						int i = 0;
						int count = chunk.Count;
						while (i < count)
						{
							requiredComponentDataPtrRW[i].Value = math.mul(requiredComponentDataPtrRO[i].ToMatrix(), componentDataPtrRO[i].Value);
							i++;
						}
						return;
					}
					int j = 0;
					int count2 = chunk.Count;
					while (j < count2)
					{
						requiredComponentDataPtrRW[j].Value = requiredComponentDataPtrRO[j].ToMatrix();
						j++;
					}
				}
			}

			// Token: 0x0600003E RID: 62 RVA: 0x0000323A File Offset: 0x0000143A
			void IJobChunk.Execute(in ArchetypeChunk chunk, int unfilteredChunkIndex, bool useEnabledMask, in v128 chunkEnabledMask)
			{
				this.Execute(in chunk, unfilteredChunkIndex, useEnabledMask, in chunkEnabledMask);
			}

			// Token: 0x04000038 RID: 56
			[ReadOnly]
			public ComponentTypeHandle<LocalTransform> LocalTransformTypeHandleRO;

			// Token: 0x04000039 RID: 57
			[ReadOnly]
			public ComponentTypeHandle<PostTransformMatrix> PostTransformMatrixTypeHandleRO;

			// Token: 0x0400003A RID: 58
			public ComponentTypeHandle<LocalToWorld> LocalToWorldTypeHandleRW;

			// Token: 0x0400003B RID: 59
			public uint LastSystemVersion;
		}

		// Token: 0x02000011 RID: 17
		[BurstCompile]
		private struct ComputeHierarchyLocalToWorldJob : IJobParallelForDefer
		{
			// Token: 0x0600003F RID: 63 RVA: 0x00003248 File Offset: 0x00001448
			private void ChildLocalToWorldFromTransformMatrix(in float4x4 parentLocalToWorld, Entity childEntity, bool updateChildrenTransform)
			{
				updateChildrenTransform = updateChildrenTransform || this.PostTransformMatrixLookupRO.DidChange(childEntity, this.LastSystemVersion) || this.LocalTransformLookupRO.DidChange(childEntity, this.LastSystemVersion);
				DynamicBuffer<Child> dynamicBuffer;
				bool flag = this.ChildLookupRO.TryGetBuffer(childEntity, out dynamicBuffer);
				if (updateChildrenTransform && this.LocalToWorldWriteGroupMask.MatchesIgnoreFilter(childEntity))
				{
					LocalTransform localTransform = this.LocalTransformLookupRO[childEntity];
					float4x4 float4x = math.mul(parentLocalToWorld, localTransform.ToMatrix());
					if (this.PostTransformMatrixLookupRO.HasComponent(childEntity))
					{
						float4x = math.mul(float4x, this.PostTransformMatrixLookupRO[childEntity].Value);
					}
					this.LocalToWorldLookupRW[childEntity] = new LocalToWorld
					{
						Value = float4x
					};
					if (flag)
					{
						int i = 0;
						int length = dynamicBuffer.Length;
						while (i < length)
						{
							this.ChildLocalToWorldFromTransformMatrix(in float4x, dynamicBuffer[i].Value, updateChildrenTransform);
							i++;
						}
						return;
					}
				}
				else if (flag)
				{
					float4x4 float4x = this.LocalToWorldLookupRW[childEntity].Value;
					updateChildrenTransform = this.LocalToWorldLookupRW.DidChange(childEntity, this.LastSystemVersion);
					int j = 0;
					int length2 = dynamicBuffer.Length;
					while (j < length2)
					{
						this.ChildLocalToWorldFromTransformMatrix(in float4x, dynamicBuffer[j].Value, updateChildrenTransform);
						j++;
					}
				}
			}

			// Token: 0x06000040 RID: 64 RVA: 0x000033A0 File Offset: 0x000015A0
			public void Execute(int index)
			{
				Entity entity = this.RootEntities[index];
				DynamicBuffer<Child> dynamicBuffer;
				if (this.ChildLookupRO.TryGetBuffer(entity, out dynamicBuffer))
				{
					bool flag = this.ChildLookupRO.DidChange(entity, this.LastSystemVersion) || this.LocalToWorldLookupRW.DidChange(entity, this.LastSystemVersion);
					float4x4 value = this.LocalToWorldLookupRW[entity].Value;
					int i = 0;
					int length = dynamicBuffer.Length;
					while (i < length)
					{
						this.ChildLocalToWorldFromTransformMatrix(in value, dynamicBuffer[i].Value, flag);
						i++;
					}
				}
			}

			// Token: 0x0400003C RID: 60
			[ReadOnly]
			public NativeArray<Entity> RootEntities;

			// Token: 0x0400003D RID: 61
			[ReadOnly]
			public EntityQueryMask LocalToWorldWriteGroupMask;

			// Token: 0x0400003E RID: 62
			[ReadOnly]
			public BufferLookup<Child> ChildLookupRO;

			// Token: 0x0400003F RID: 63
			[ReadOnly]
			public ComponentLookup<LocalTransform> LocalTransformLookupRO;

			// Token: 0x04000040 RID: 64
			[ReadOnly]
			public ComponentLookup<PostTransformMatrix> PostTransformMatrixLookupRO;

			// Token: 0x04000041 RID: 65
			[NativeDisableContainerSafetyRestriction]
			public ComponentLookup<LocalToWorld> LocalToWorldLookupRW;

			// Token: 0x04000042 RID: 66
			public uint LastSystemVersion;
		}

		// Token: 0x02000012 RID: 18
		// (Invoke) Token: 0x06000042 RID: 66
		internal delegate void __codegen__OnCreate_00000022$PostfixBurstDelegate(IntPtr self, IntPtr state);

		// Token: 0x02000013 RID: 19
		internal static class __codegen__OnCreate_00000022$BurstDirectCall
		{
			// Token: 0x06000045 RID: 69 RVA: 0x00003437 File Offset: 0x00001637
			[BurstDiscard]
			private static void GetFunctionPointerDiscard(ref IntPtr A_0)
			{
				if (LocalToWorldSystem.__codegen__OnCreate_00000022$BurstDirectCall.Pointer == 0)
				{
					LocalToWorldSystem.__codegen__OnCreate_00000022$BurstDirectCall.Pointer = BurstCompiler.GetILPPMethodFunctionPointer2(LocalToWorldSystem.__codegen__OnCreate_00000022$BurstDirectCall.DeferredCompilation, methodof(LocalToWorldSystem.__codegen__OnCreate$BurstManaged(IntPtr, IntPtr)).MethodHandle, typeof(LocalToWorldSystem.__codegen__OnCreate_00000022$PostfixBurstDelegate).TypeHandle);
				}
				A_0 = LocalToWorldSystem.__codegen__OnCreate_00000022$BurstDirectCall.Pointer;
			}

			// Token: 0x06000046 RID: 70 RVA: 0x00003464 File Offset: 0x00001664
			private static IntPtr GetFunctionPointer()
			{
				IntPtr intPtr = (IntPtr)0;
				LocalToWorldSystem.__codegen__OnCreate_00000022$BurstDirectCall.GetFunctionPointerDiscard(ref intPtr);
				return intPtr;
			}

			// Token: 0x06000047 RID: 71 RVA: 0x0000347C File Offset: 0x0000167C
			public static void Constructor()
			{
				LocalToWorldSystem.__codegen__OnCreate_00000022$BurstDirectCall.DeferredCompilation = BurstCompiler.CompileILPPMethod2(methodof(LocalToWorldSystem.__codegen__OnCreate(IntPtr, IntPtr)).MethodHandle);
			}

			// Token: 0x06000048 RID: 72 RVA: 0x0000283B File Offset: 0x00000A3B
			public static void Initialize()
			{
			}

			// Token: 0x06000049 RID: 73 RVA: 0x0000348D File Offset: 0x0000168D
			// Note: this type is marked as 'beforefieldinit'.
			static __codegen__OnCreate_00000022$BurstDirectCall()
			{
				LocalToWorldSystem.__codegen__OnCreate_00000022$BurstDirectCall.Constructor();
			}

			// Token: 0x0600004A RID: 74 RVA: 0x00003494 File Offset: 0x00001694
			public static void Invoke(IntPtr self, IntPtr state)
			{
				if (BurstCompiler.IsEnabled)
				{
					IntPtr functionPointer = LocalToWorldSystem.__codegen__OnCreate_00000022$BurstDirectCall.GetFunctionPointer();
					if (functionPointer != 0)
					{
						calli(System.Void(System.IntPtr,System.IntPtr), self, state, functionPointer);
						return;
					}
				}
				LocalToWorldSystem.__codegen__OnCreate$BurstManaged(self, state);
			}

			// Token: 0x04000043 RID: 67
			private static IntPtr Pointer;

			// Token: 0x04000044 RID: 68
			private static IntPtr DeferredCompilation;
		}

		// Token: 0x02000014 RID: 20
		// (Invoke) Token: 0x0600004C RID: 76
		internal delegate void __codegen__OnUpdate_00000023$PostfixBurstDelegate(IntPtr self, IntPtr state);

		// Token: 0x02000015 RID: 21
		internal static class __codegen__OnUpdate_00000023$BurstDirectCall
		{
			// Token: 0x0600004F RID: 79 RVA: 0x000034C7 File Offset: 0x000016C7
			[BurstDiscard]
			private static void GetFunctionPointerDiscard(ref IntPtr A_0)
			{
				if (LocalToWorldSystem.__codegen__OnUpdate_00000023$BurstDirectCall.Pointer == 0)
				{
					LocalToWorldSystem.__codegen__OnUpdate_00000023$BurstDirectCall.Pointer = BurstCompiler.GetILPPMethodFunctionPointer2(LocalToWorldSystem.__codegen__OnUpdate_00000023$BurstDirectCall.DeferredCompilation, methodof(LocalToWorldSystem.__codegen__OnUpdate$BurstManaged(IntPtr, IntPtr)).MethodHandle, typeof(LocalToWorldSystem.__codegen__OnUpdate_00000023$PostfixBurstDelegate).TypeHandle);
				}
				A_0 = LocalToWorldSystem.__codegen__OnUpdate_00000023$BurstDirectCall.Pointer;
			}

			// Token: 0x06000050 RID: 80 RVA: 0x000034F4 File Offset: 0x000016F4
			private static IntPtr GetFunctionPointer()
			{
				IntPtr intPtr = (IntPtr)0;
				LocalToWorldSystem.__codegen__OnUpdate_00000023$BurstDirectCall.GetFunctionPointerDiscard(ref intPtr);
				return intPtr;
			}

			// Token: 0x06000051 RID: 81 RVA: 0x0000350C File Offset: 0x0000170C
			public static void Constructor()
			{
				LocalToWorldSystem.__codegen__OnUpdate_00000023$BurstDirectCall.DeferredCompilation = BurstCompiler.CompileILPPMethod2(methodof(LocalToWorldSystem.__codegen__OnUpdate(IntPtr, IntPtr)).MethodHandle);
			}

			// Token: 0x06000052 RID: 82 RVA: 0x0000283B File Offset: 0x00000A3B
			public static void Initialize()
			{
			}

			// Token: 0x06000053 RID: 83 RVA: 0x0000351D File Offset: 0x0000171D
			// Note: this type is marked as 'beforefieldinit'.
			static __codegen__OnUpdate_00000023$BurstDirectCall()
			{
				LocalToWorldSystem.__codegen__OnUpdate_00000023$BurstDirectCall.Constructor();
			}

			// Token: 0x06000054 RID: 84 RVA: 0x00003524 File Offset: 0x00001724
			public static void Invoke(IntPtr self, IntPtr state)
			{
				if (BurstCompiler.IsEnabled)
				{
					IntPtr functionPointer = LocalToWorldSystem.__codegen__OnUpdate_00000023$BurstDirectCall.GetFunctionPointer();
					if (functionPointer != 0)
					{
						calli(System.Void(System.IntPtr,System.IntPtr), self, state, functionPointer);
						return;
					}
				}
				LocalToWorldSystem.__codegen__OnUpdate$BurstManaged(self, state);
			}

			// Token: 0x04000045 RID: 69
			private static IntPtr Pointer;

			// Token: 0x04000046 RID: 70
			private static IntPtr DeferredCompilation;
		}
	}
}
