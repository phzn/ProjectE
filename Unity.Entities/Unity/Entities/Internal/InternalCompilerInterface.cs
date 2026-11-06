using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Unity.Burst;
using Unity.Burst.Intrinsics;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Jobs;
using Unity.Jobs.LowLevel.Unsafe;

namespace Unity.Entities.Internal
{
	// Token: 0x02000452 RID: 1106
	[BurstCompile]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public static class InternalCompilerInterface
	{
		// Token: 0x06001B4B RID: 6987 RVA: 0x0005F08F File Offset: 0x0005D28F
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static EntityStorageInfoLookup GetEntityStorageInfoLookup(ref EntityStorageInfoLookup entityStorageInfoLookup, ref SystemState state)
		{
			entityStorageInfoLookup.Update(ref state);
			return entityStorageInfoLookup;
		}

		// Token: 0x06001B4C RID: 6988 RVA: 0x0005F09E File Offset: 0x0005D29E
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool DoesEntityExist(ref EntityStorageInfoLookup entityStorageInfoLookup, ref SystemState state, Entity entity)
		{
			entityStorageInfoLookup.Update(ref state);
			return entityStorageInfoLookup.Exists(entity);
		}

		// Token: 0x06001B4D RID: 6989 RVA: 0x0005F0AE File Offset: 0x0005D2AE
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static EntityTypeHandle GetEntityTypeHandle(ref EntityTypeHandle entityTypeHandle, ref SystemState state)
		{
			entityTypeHandle.Update(ref state);
			return entityTypeHandle;
		}

		// Token: 0x06001B4E RID: 6990 RVA: 0x0005F0BD File Offset: 0x0005D2BD
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static ComponentLookup<T> GetComponentLookup<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(ref ComponentLookup<T> componentLookup, ref SystemState state) where T : struct, ValueType, IComponentData
		{
			componentLookup.Update(ref state);
			return componentLookup;
		}

		// Token: 0x06001B4F RID: 6991 RVA: 0x0005F0CC File Offset: 0x0005D2CC
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static BufferLookup<T> GetBufferLookup<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(ref BufferLookup<T> bufferLookup, ref SystemState state) where T : struct, ValueType, IBufferElementData
		{
			bufferLookup.Update(ref state);
			return bufferLookup;
		}

		// Token: 0x06001B50 RID: 6992 RVA: 0x0005F0DC File Offset: 0x0005D2DC
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static RefRO<T> GetComponentROAfterCompletingDependency<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(ref ComponentLookup<T> componentLookup, ref SystemState state, Entity entity) where T : struct, ValueType, IComponentData
		{
			componentLookup.Update(ref state);
			state.EntityManager.CompleteDependencyBeforeRO<T>();
			return componentLookup.GetRefRO(entity);
		}

		// Token: 0x06001B51 RID: 6993 RVA: 0x0005F108 File Offset: 0x0005D308
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static RefRW<T> GetComponentRWAfterCompletingDependency<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(ref ComponentLookup<T> componentLookup, ref SystemState state, Entity entity) where T : struct, ValueType, IComponentData
		{
			componentLookup.Update(ref state);
			state.EntityManager.CompleteDependencyBeforeRW<T>();
			return componentLookup.GetRefRW(entity);
		}

		// Token: 0x06001B52 RID: 6994 RVA: 0x0005F134 File Offset: 0x0005D334
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static T GetComponentAfterCompletingDependency<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(ref ComponentLookup<T> componentLookup, ref SystemState state, Entity entity) where T : struct, ValueType, IComponentData
		{
			componentLookup.Update(ref state);
			state.EntityManager.CompleteDependencyBeforeRO<T>();
			return componentLookup[entity];
		}

		// Token: 0x06001B53 RID: 6995 RVA: 0x0005F160 File Offset: 0x0005D360
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static RefRW<T> GetComponentRWAfterCompletingDependency<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(ref ComponentLookup<T> componentLookup, ref SystemState state, SystemHandle systemHandle) where T : struct, ValueType, IComponentData
		{
			componentLookup.Update(ref state);
			state.EntityManager.CompleteDependencyBeforeRW<T>();
			return componentLookup.GetRefRW(systemHandle);
		}

		// Token: 0x06001B54 RID: 6996 RVA: 0x0005F18C File Offset: 0x0005D38C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static T GetComponentAfterCompletingDependency<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(ref ComponentLookup<T> componentLookup, ref SystemState state, SystemHandle systemHandle) where T : struct, ValueType, IComponentData
		{
			componentLookup.Update(ref state);
			state.EntityManager.CompleteDependencyBeforeRO<T>();
			return componentLookup[systemHandle];
		}

		// Token: 0x06001B55 RID: 6997 RVA: 0x0005F1B8 File Offset: 0x0005D3B8
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void SetComponentAfterCompletingDependency<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(ref ComponentLookup<T> componentLookup, ref SystemState state, T component, Entity entity) where T : struct, ValueType, IComponentData
		{
			componentLookup.Update(ref state);
			state.EntityManager.CompleteDependencyBeforeRW<T>();
			componentLookup[entity] = component;
		}

		// Token: 0x06001B56 RID: 6998 RVA: 0x0005F1E4 File Offset: 0x0005D3E4
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void SetComponentAfterCompletingDependency<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(ref ComponentLookup<T> componentLookup, ref SystemState state, T component, SystemHandle systemHandle) where T : struct, ValueType, IComponentData
		{
			componentLookup.Update(ref state);
			state.EntityManager.CompleteDependencyBeforeRW<T>();
			componentLookup[systemHandle] = component;
		}

		// Token: 0x06001B57 RID: 6999 RVA: 0x0005F210 File Offset: 0x0005D410
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool HasComponentAfterCompletingDependency<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(ref ComponentLookup<T> componentLookup, ref SystemState state, Entity entity) where T : struct, ValueType, IComponentData
		{
			componentLookup.Update(ref state);
			state.EntityManager.CompleteDependencyBeforeRO<T>();
			return componentLookup.HasComponent(entity);
		}

		// Token: 0x06001B58 RID: 7000 RVA: 0x0005F23C File Offset: 0x0005D43C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool HasComponentAfterCompletingDependency<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(ref ComponentLookup<T> componentLookup, ref SystemState state, SystemHandle systemHandle) where T : struct, ValueType, IComponentData
		{
			componentLookup.Update(ref state);
			state.EntityManager.CompleteDependencyBeforeRO<T>();
			return componentLookup.HasComponent(systemHandle);
		}

		// Token: 0x06001B59 RID: 7001 RVA: 0x0005F268 File Offset: 0x0005D468
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool IsComponentEnabledAfterCompletingDependency<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(ref ComponentLookup<T> componentLookup, ref SystemState state, Entity entity) where T : struct, ValueType, IComponentData
		{
			componentLookup.Update(ref state);
			state.EntityManager.CompleteDependencyBeforeRO<T>();
			return componentLookup.IsComponentEnabled(entity);
		}

		// Token: 0x06001B5A RID: 7002 RVA: 0x0005F294 File Offset: 0x0005D494
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool IsComponentEnabledAfterCompletingDependency<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(ref ComponentLookup<T> componentLookup, ref SystemState state, SystemHandle systemHandle) where T : struct, ValueType, IComponentData
		{
			componentLookup.Update(ref state);
			state.EntityManager.CompleteDependencyBeforeRO<T>();
			return componentLookup.IsComponentEnabled(systemHandle);
		}

		// Token: 0x06001B5B RID: 7003 RVA: 0x0005F2C0 File Offset: 0x0005D4C0
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void SetComponentEnabledAfterCompletingDependency<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(ref ComponentLookup<T> componentLookup, ref SystemState state, Entity entity, bool value) where T : struct, ValueType, IComponentData
		{
			componentLookup.Update(ref state);
			state.EntityManager.CompleteDependencyBeforeRW<T>();
			componentLookup.SetComponentEnabled(entity, value);
		}

		// Token: 0x06001B5C RID: 7004 RVA: 0x0005F2EC File Offset: 0x0005D4EC
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void SetComponentEnabledAfterCompletingDependency<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(ref ComponentLookup<T> componentLookup, ref SystemState state, SystemHandle systemHandle, bool value) where T : struct, ValueType, IComponentData
		{
			componentLookup.Update(ref state);
			state.EntityManager.CompleteDependencyBeforeRW<T>();
			componentLookup.SetComponentEnabled(systemHandle, value);
		}

		// Token: 0x06001B5D RID: 7005 RVA: 0x0005F318 File Offset: 0x0005D518
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static DynamicBuffer<T> GetBufferAfterCompletingDependency<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(ref BufferLookup<T> bufferLookup, ref SystemState state, Entity entity) where T : struct, ValueType, IBufferElementData
		{
			bufferLookup.Update(ref state);
			state.EntityManager.CompleteDependencyBeforeRW<T>();
			return bufferLookup[entity];
		}

		// Token: 0x06001B5E RID: 7006 RVA: 0x0005F344 File Offset: 0x0005D544
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool HasBufferAfterCompletingDependency<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(ref BufferLookup<T> bufferLookup, ref SystemState state, Entity entity) where T : struct, ValueType, IBufferElementData
		{
			bufferLookup.Update(ref state);
			state.EntityManager.CompleteDependencyBeforeRO<T>();
			return bufferLookup.HasBuffer(entity);
		}

		// Token: 0x06001B5F RID: 7007 RVA: 0x0005F370 File Offset: 0x0005D570
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool IsBufferEnabledAfterCompletingDependency<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(ref BufferLookup<T> bufferLookup, ref SystemState state, Entity entity) where T : struct, ValueType, IBufferElementData
		{
			bufferLookup.Update(ref state);
			state.EntityManager.CompleteDependencyBeforeRO<T>();
			return bufferLookup.IsBufferEnabled(entity);
		}

		// Token: 0x06001B60 RID: 7008 RVA: 0x0005F39C File Offset: 0x0005D59C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void SetBufferEnabledAfterCompletingDependency<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(ref BufferLookup<T> bufferLookup, ref SystemState state, Entity entity, bool value) where T : struct, ValueType, IBufferElementData
		{
			bufferLookup.Update(ref state);
			state.EntityManager.CompleteDependencyBeforeRW<T>();
			bufferLookup.SetBufferEnabled(entity, value);
		}

		// Token: 0x06001B61 RID: 7009 RVA: 0x0005F3C8 File Offset: 0x0005D5C8
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static T GetAspectAfterCompletingDependency<TLookup, T>(ref TLookup aspectLookup, ref SystemState state, bool isAspectReadOnly, Entity entity) where TLookup : struct, InternalCompilerInterface.IAspectLookup<T> where T : struct, IAspect, IAspectCreate<T>
		{
			aspectLookup.Update(ref state);
			if (isAspectReadOnly)
			{
				T t = default(T);
				t.CompleteDependencyBeforeRO(ref state);
			}
			else
			{
				T t = default(T);
				t.CompleteDependencyBeforeRW(ref state);
			}
			return aspectLookup[entity];
		}

		// Token: 0x06001B62 RID: 7010 RVA: 0x0005F41E File Offset: 0x0005D61E
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static ComponentTypeHandle<T> GetComponentTypeHandle<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(ref ComponentTypeHandle<T> componentTypeHandle, ref SystemState state) where T : struct, ValueType, IComponentData
		{
			componentTypeHandle.Update(ref state);
			return componentTypeHandle;
		}

		// Token: 0x06001B63 RID: 7011 RVA: 0x0005F42D File Offset: 0x0005D62D
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static BufferTypeHandle<T> GetBufferTypeHandle<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(ref BufferTypeHandle<T> bufferTypeHandle, ref SystemState state) where T : struct, ValueType, IBufferElementData
		{
			bufferTypeHandle.Update(ref state);
			return bufferTypeHandle;
		}

		// Token: 0x06001B64 RID: 7012 RVA: 0x0005F43C File Offset: 0x0005D63C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static SharedComponentTypeHandle<T> GetSharedComponentTypeHandle<T>(ref SharedComponentTypeHandle<T> sharedComponentTypeHandle, ref SystemState state) where T : struct, ISharedComponentData
		{
			sharedComponentTypeHandle.Update(ref state);
			return sharedComponentTypeHandle;
		}

		// Token: 0x06001B65 RID: 7013 RVA: 0x0005F44B File Offset: 0x0005D64B
		public unsafe static ref T UnsafeAsRef<T>(IntPtr value) where T : struct
		{
			return UnsafeUtility.AsRef<T>((void*)value);
		}

		// Token: 0x06001B66 RID: 7014 RVA: 0x0005F458 File Offset: 0x0005D658
		public static IntPtr AddressOf<T>(ref T value) where T : struct
		{
			return (IntPtr)UnsafeUtility.AddressOf<T>(ref value);
		}

		// Token: 0x06001B67 RID: 7015 RVA: 0x0005F465 File Offset: 0x0005D665
		public unsafe static T GetComponentData<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(EntityManager manager, Entity entity, TypeIndex typeIndex, out T originalComponent) where T : struct, ValueType, IComponentData
		{
			UnsafeUtility.CopyPtrToStructure<T>((void*)manager.GetCheckedEntityDataAccess()->EntityComponentStore->GetComponentDataWithTypeRO(entity, typeIndex), out originalComponent);
			return originalComponent;
		}

		// Token: 0x06001B68 RID: 7016 RVA: 0x0005F488 File Offset: 0x0005D688
		public unsafe static void WriteComponentData<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(EntityManager manager, Entity entity, TypeIndex typeIndex, ref T lambdaComponent, ref T originalComponent) where T : struct, ValueType, IComponentData
		{
			EntityComponentStore* entityComponentStore = manager.GetCheckedEntityDataAccess()->EntityComponentStore;
			int num = UnsafeUtility.SizeOf<T>();
			bool flag;
			if (UnsafeUtility.MemCmp(UnsafeUtility.AddressOf<T>(ref lambdaComponent), UnsafeUtility.AddressOf<T>(ref originalComponent), (long)num) != 0 && entityComponentStore->HasComponent(entity, typeIndex, out flag))
			{
				byte* componentDataWithTypeRW = entityComponentStore->GetComponentDataWithTypeRW(entity, typeIndex, entityComponentStore->GlobalSystemVersion);
				UnsafeUtility.CopyStructureToPtr<T>(ref lambdaComponent, (void*)componentDataWithTypeRW);
			}
		}

		// Token: 0x06001B69 RID: 7017 RVA: 0x0005F4E0 File Offset: 0x0005D6E0
		public static void UnsafeCreateGatherEntitiesResult(ref EntityQuery query, out InternalGatherEntitiesResult result)
		{
			query.GatherEntitiesToArray(out result);
		}

		// Token: 0x06001B6A RID: 7018 RVA: 0x0005F4E9 File Offset: 0x0005D6E9
		public static void UnsafeReleaseGatheredEntities(ref EntityQuery query, ref InternalGatherEntitiesResult result)
		{
			query.ReleaseGatheredEntities(ref result);
		}

		// Token: 0x06001B6B RID: 7019 RVA: 0x0005F4F2 File Offset: 0x0005D6F2
		public unsafe static Entity UnsafeGetEntityFromGatheredEntities(ref InternalGatherEntitiesResult result, int entityIndex)
		{
			return result.EntityBuffer[entityIndex];
		}

		// Token: 0x06001B6C RID: 7020 RVA: 0x0005F509 File Offset: 0x0005D709
		public static IntPtr UnsafeGetChunkNativeArrayReadOnlyIntPtr<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(ArchetypeChunk chunk, ref ComponentTypeHandle<T> typeHandle) where T : struct, ValueType, IComponentData
		{
			return (IntPtr)chunk.GetRequiredComponentDataPtrRO<T>(ref typeHandle);
		}

		// Token: 0x06001B6D RID: 7021 RVA: 0x0005F518 File Offset: 0x0005D718
		public static IntPtr UnsafeGetChunkNativeArrayIntPtr<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(ArchetypeChunk chunk, ref ComponentTypeHandle<T> typeHandle) where T : struct, ValueType, IComponentData
		{
			return (IntPtr)chunk.GetRequiredComponentDataPtrRW<T>(ref typeHandle);
		}

		// Token: 0x06001B6E RID: 7022 RVA: 0x0005F527 File Offset: 0x0005D727
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool UnsafeTryGetNextEnabledBitRange(v128 mask, int firstIndexToCheck, out int nextRangeBegin, out int nextRangeEnd)
		{
			return EnabledBitUtility.TryGetNextRange(mask, firstIndexToCheck, out nextRangeBegin, out nextRangeEnd);
		}

		// Token: 0x06001B6F RID: 7023 RVA: 0x0005F532 File Offset: 0x0005D732
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public unsafe static IntPtr UnsafeGetChunkNativeArrayReadOnlyIntPtrWithoutChecks<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(in ArchetypeChunk chunk, ref ComponentTypeHandle<T> typeHandle) where T : struct, ValueType, IComponentData
		{
			return (IntPtr)((void*)ChunkDataUtility.GetComponentDataWithTypeRO(chunk.m_Chunk, chunk.Archetype.Archetype, 0, typeHandle.m_TypeIndex, ref typeHandle.m_LookupCache));
		}

		// Token: 0x06001B70 RID: 7024 RVA: 0x0005F55C File Offset: 0x0005D75C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public unsafe static IntPtr UnsafeGetChunkNativeArrayIntPtrWithoutChecks<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(in ArchetypeChunk chunk, ref ComponentTypeHandle<T> typeHandle) where T : struct, ValueType, IComponentData
		{
			return (IntPtr)((void*)ChunkDataUtility.GetComponentDataWithTypeRW(chunk.m_Chunk, chunk.Archetype.Archetype, 0, typeHandle.m_TypeIndex, typeHandle.GlobalSystemVersion, ref typeHandle.m_LookupCache));
		}

		// Token: 0x06001B71 RID: 7025 RVA: 0x0005F58C File Offset: 0x0005D78C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static InternalCompilerInterface.UncheckedRefRO<T> UnsafeGetUncheckedRefRO<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(IntPtr ptr, int index, ref ComponentTypeHandle<T> typeHandle) where T : struct, ValueType, IComponentData
		{
			return new InternalCompilerInterface.UncheckedRefRO<T>(ptr + UnsafeUtility.SizeOf<T>() * index);
		}

		// Token: 0x06001B72 RID: 7026 RVA: 0x0005F58C File Offset: 0x0005D78C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static InternalCompilerInterface.UncheckedRefRO<T> UnsafeGetUncheckedRefRO<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(IntPtr ptr, int index) where T : struct, ValueType, IComponentData
		{
			return new InternalCompilerInterface.UncheckedRefRO<T>(ptr + UnsafeUtility.SizeOf<T>() * index);
		}

		// Token: 0x06001B73 RID: 7027 RVA: 0x0005F5A0 File Offset: 0x0005D7A0
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static InternalCompilerInterface.UncheckedRefRW<T> UnsafeGetUncheckedRefRW<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(IntPtr ptr, int index, ref ComponentTypeHandle<T> typeHandle) where T : struct, ValueType, IComponentData
		{
			return new InternalCompilerInterface.UncheckedRefRW<T>(ptr + UnsafeUtility.SizeOf<T>() * index);
		}

		// Token: 0x06001B74 RID: 7028 RVA: 0x0005F5A0 File Offset: 0x0005D7A0
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static InternalCompilerInterface.UncheckedRefRW<T> UnsafeGetUncheckedRefRW<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(IntPtr ptr, int index) where T : struct, ValueType, IComponentData
		{
			return new InternalCompilerInterface.UncheckedRefRW<T>(ptr + UnsafeUtility.SizeOf<T>() * index);
		}

		// Token: 0x06001B75 RID: 7029 RVA: 0x0005F5B4 File Offset: 0x0005D7B4
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public unsafe static RefRO<T> GetRefRO<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(IntPtr ptr, int index, ref ComponentTypeHandle<T> typeHandle) where T : struct, ValueType, IComponentData
		{
			return new RefRO<T>((void*)((byte*)(void*)ptr + UnsafeUtility.SizeOf<T>() * index));
		}

		// Token: 0x06001B76 RID: 7030 RVA: 0x0005F5B4 File Offset: 0x0005D7B4
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public unsafe static RefRO<T> GetRefRO<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(IntPtr ptr, int index) where T : struct, ValueType, IComponentData
		{
			return new RefRO<T>((void*)((byte*)(void*)ptr + UnsafeUtility.SizeOf<T>() * index));
		}

		// Token: 0x06001B77 RID: 7031 RVA: 0x0005F5C9 File Offset: 0x0005D7C9
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public unsafe static RefRW<T> GetRefRW<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(IntPtr ptr, int index, ref ComponentTypeHandle<T> typeHandle) where T : struct, ValueType, IComponentData
		{
			return new RefRW<T>((byte*)(void*)ptr + UnsafeUtility.SizeOf<T>() * index);
		}

		// Token: 0x06001B78 RID: 7032 RVA: 0x0005F5C9 File Offset: 0x0005D7C9
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public unsafe static RefRW<T> GetRefRW<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(IntPtr ptr, int index) where T : struct, ValueType, IComponentData
		{
			return new RefRW<T>((byte*)(void*)ptr + UnsafeUtility.SizeOf<T>() * index);
		}

		// Token: 0x06001B79 RID: 7033 RVA: 0x0005F5DE File Offset: 0x0005D7DE
		public unsafe static IntPtr UnsafeGetChunkEntityArrayIntPtr(ArchetypeChunk chunk, EntityTypeHandle typeHandle)
		{
			return (IntPtr)((void*)chunk.GetEntityDataPtrRO(typeHandle));
		}

		// Token: 0x06001B7A RID: 7034 RVA: 0x0005F5ED File Offset: 0x0005D7ED
		public static IntPtr UnsafeGetEntityArrayIntPtr(NativeArray<Entity> array)
		{
			return (IntPtr)array.GetUnsafeReadOnlyPtr<Entity>();
		}

		// Token: 0x06001B7B RID: 7035 RVA: 0x0005F5FA File Offset: 0x0005D7FA
		public unsafe static T UnsafeGetCopyOfNativeArrayPtrElement<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(IntPtr nativeArrayPtr, int elementIndex) where T : struct, ValueType
		{
			return *(T*)((byte*)(void*)nativeArrayPtr + (IntPtr)elementIndex * (IntPtr)sizeof(T));
		}

		// Token: 0x06001B7C RID: 7036 RVA: 0x0005F611 File Offset: 0x0005D811
		public unsafe static ref T UnsafeGetRefToNativeArrayPtrElement<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(IntPtr nativeArrayPtr, int elementIndex) where T : struct, ValueType
		{
			return UnsafeUtility.AsRef<T>((void*)((byte*)(void*)nativeArrayPtr + (IntPtr)elementIndex * (IntPtr)sizeof(T)));
		}

		// Token: 0x06001B7D RID: 7037 RVA: 0x0005F628 File Offset: 0x0005D828
		public unsafe static ref SystemState UnsafeGetSystemStateRef(IntPtr statePtr)
		{
			return ref *(SystemState*)(void*)statePtr;
		}

		// Token: 0x06001B7E RID: 7038 RVA: 0x0005F630 File Offset: 0x0005D830
		[BurstCompile]
		public static void MergeWith(ref UnsafeList<ComponentType> mergeThese, ref UnsafeList<ComponentType> withThese)
		{
			InternalCompilerInterface.MergeWith_0000184E$BurstDirectCall.Invoke(ref mergeThese, ref withThese);
		}

		// Token: 0x06001B7F RID: 7039 RVA: 0x0005F63C File Offset: 0x0005D83C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static EntityQuery OnlyAllowedInSourceGeneratedCodeGetSingleQuery<T>(SystemBase system)
		{
			return new EntityQueryBuilder(system.WorldUpdateAllocator).WithAllRW<T>().Build(system);
		}

		// Token: 0x06001B80 RID: 7040 RVA: 0x0005F66A File Offset: 0x0005D86A
		internal static InvalidOperationException ThrowCodeGenException()
		{
			throw new InvalidOperationException("No suitable code replacement generated, this is either due to generators failing, or lack of support in your current context");
		}

		// Token: 0x06001B81 RID: 7041 RVA: 0x0005F678 File Offset: 0x0005D878
		[BurstCompile]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal unsafe static void MergeWith$BurstManaged(ref UnsafeList<ComponentType> mergeThese, ref UnsafeList<ComponentType> withThese)
		{
			mergeThese.AddRange(withThese);
			NativeHashMap<int, ComponentType.AccessMode> nativeHashMap = new NativeHashMap<int, ComponentType.AccessMode>(mergeThese.Length, Allocator.Temp);
			foreach (ComponentType componentType in mergeThese)
			{
				ComponentType.AccessMode accessMode;
				if (!nativeHashMap.TryGetValue(componentType.TypeIndex, out accessMode))
				{
					nativeHashMap.Add(componentType.TypeIndex, componentType.AccessModeType);
				}
				else
				{
					if ((accessMode == ComponentType.AccessMode.Exclude && componentType.AccessModeType != ComponentType.AccessMode.Exclude) || (accessMode != ComponentType.AccessMode.Exclude && componentType.AccessModeType == ComponentType.AccessMode.Exclude))
					{
						throw new ArgumentException("A component cannot be both excluded and included.");
					}
					if (componentType.AccessModeType == ComponentType.AccessMode.ReadWrite)
					{
						nativeHashMap[componentType.TypeIndex] = ComponentType.AccessMode.ReadWrite;
					}
				}
			}
			mergeThese.Clear();
			foreach (KVPair<int, ComponentType.AccessMode> kvpair in nativeHashMap)
			{
				ComponentType componentType2 = default(ComponentType);
				componentType2.TypeIndex = kvpair.Key;
				componentType2.AccessModeType = *kvpair.Value;
				mergeThese.Add(in componentType2);
			}
			nativeHashMap.Dispose();
		}

		// Token: 0x02000453 RID: 1107
		public interface IAspectLookup<T> where T : IAspect
		{
			// Token: 0x06001B82 RID: 7042
			void Update(ref SystemState state);

			// Token: 0x1700022A RID: 554
			T this[Entity entity] { get; }
		}

		// Token: 0x02000454 RID: 1108
		public readonly struct UncheckedRefRO<[global::System.Runtime.CompilerServices.IsUnmanaged] T> where T : struct, ValueType, IComponentData
		{
			// Token: 0x06001B84 RID: 7044 RVA: 0x0005F7CC File Offset: 0x0005D9CC
			public UncheckedRefRO(IntPtr ptr)
			{
				this._ptr = ptr;
			}

			// Token: 0x1700022B RID: 555
			// (get) Token: 0x06001B85 RID: 7045 RVA: 0x0005F7D5 File Offset: 0x0005D9D5
			public unsafe readonly ref T ValueRO
			{
				get
				{
					return (void*)this._ptr;
				}
			}

			// Token: 0x06001B86 RID: 7046 RVA: 0x0005F7E2 File Offset: 0x0005D9E2
			public unsafe static implicit operator RefRO<T>(InternalCompilerInterface.UncheckedRefRO<T> @unchecked)
			{
				return new RefRO<T>((void*)@unchecked._ptr);
			}

			// Token: 0x04000980 RID: 2432
			private readonly IntPtr _ptr;
		}

		// Token: 0x02000455 RID: 1109
		public readonly struct UncheckedRefRW<[global::System.Runtime.CompilerServices.IsUnmanaged] T> where T : struct, ValueType, IComponentData
		{
			// Token: 0x06001B87 RID: 7047 RVA: 0x0005F7F4 File Offset: 0x0005D9F4
			public UncheckedRefRW(IntPtr ptr)
			{
				this._ptr = ptr;
			}

			// Token: 0x1700022C RID: 556
			// (get) Token: 0x06001B88 RID: 7048 RVA: 0x0005F7FD File Offset: 0x0005D9FD
			public unsafe ref T ValueRW
			{
				get
				{
					return ref *(T*)(void*)this._ptr;
				}
			}

			// Token: 0x1700022D RID: 557
			// (get) Token: 0x06001B89 RID: 7049 RVA: 0x0005F7FD File Offset: 0x0005D9FD
			public unsafe readonly ref T ValueRO
			{
				get
				{
					return (void*)this._ptr;
				}
			}

			// Token: 0x06001B8A RID: 7050 RVA: 0x0005F80A File Offset: 0x0005DA0A
			public unsafe static implicit operator RefRW<T>(InternalCompilerInterface.UncheckedRefRW<T> @unchecked)
			{
				return new RefRW<T>((void*)@unchecked._ptr);
			}

			// Token: 0x04000981 RID: 2433
			private readonly IntPtr _ptr;
		}

		// Token: 0x02000456 RID: 1110
		public static class EntityQueryInterface
		{
			// Token: 0x06001B8B RID: 7051 RVA: 0x0005F81C File Offset: 0x0005DA1C
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public static bool HasComponentsRequiredForExecuteMethodToRun(ref EntityQuery userDefinedQuery, ref Span<ComponentType> componentsUsedInExecuteMethod)
			{
				return userDefinedQuery.HasComponentsRequiredForExecuteMethodToRun(ref componentsUsedInExecuteMethod);
			}
		}

		// Token: 0x02000457 RID: 1111
		public static class JobChunkInterface
		{
			// Token: 0x06001B8C RID: 7052 RVA: 0x0005F825 File Offset: 0x0005DA25
			public static void RunWithoutJobsInternal<T>(ref T jobData, ref EntityQuery query) where T : struct, IJobChunk
			{
				(ref jobData).RunByRefWithoutJobs<T>(query);
			}

			// Token: 0x06001B8D RID: 7053 RVA: 0x0005F833 File Offset: 0x0005DA33
			public static void RunWithoutJobs<T>(ref T jobData, EntityQuery query) where T : struct, IJobChunk
			{
				(ref jobData).RunByRefWithoutJobs<T>(query);
			}

			// Token: 0x06001B8E RID: 7054 RVA: 0x0005F833 File Offset: 0x0005DA33
			public static void RunByRefWithoutJobs<T>(ref T jobData, EntityQuery query) where T : struct, IJobChunk
			{
				(ref jobData).RunByRefWithoutJobs<T>(query);
			}

			// Token: 0x06001B8F RID: 7055 RVA: 0x0005F83C File Offset: 0x0005DA3C
			public static JobHandle Schedule<T>(T jobData, EntityQuery query, JobHandle dependsOn) where T : struct, IJobChunk
			{
				return JobChunkExtensions.ScheduleInternal<T>(ref jobData, query, dependsOn, ScheduleMode.Single, default(NativeArray<int>));
			}

			// Token: 0x06001B90 RID: 7056 RVA: 0x0005F85C File Offset: 0x0005DA5C
			public static JobHandle ScheduleByRef<T>(ref T jobData, EntityQuery query, JobHandle dependsOn) where T : struct, IJobChunk
			{
				return JobChunkExtensions.ScheduleInternal<T>(ref jobData, query, dependsOn, ScheduleMode.Single, default(NativeArray<int>));
			}

			// Token: 0x06001B91 RID: 7057 RVA: 0x0005F87C File Offset: 0x0005DA7C
			public static JobHandle ScheduleParallel<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(T jobData, EntityQuery query, JobHandle dependsOn) where T : struct, ValueType, IJobChunk
			{
				return JobChunkExtensions.ScheduleInternal<T>(ref jobData, query, dependsOn, ScheduleMode.Batched, default(NativeArray<int>));
			}

			// Token: 0x06001B92 RID: 7058 RVA: 0x0005F89C File Offset: 0x0005DA9C
			public static JobHandle ScheduleParallelByRef<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(ref T jobData, EntityQuery query, JobHandle dependsOn) where T : struct, ValueType, IJobChunk
			{
				return JobChunkExtensions.ScheduleInternal<T>(ref jobData, query, dependsOn, ScheduleMode.Batched, default(NativeArray<int>));
			}

			// Token: 0x06001B93 RID: 7059 RVA: 0x0005F8BB File Offset: 0x0005DABB
			public static JobHandle ScheduleParallel<T>(T jobData, EntityQuery query, JobHandle dependsOn, NativeArray<int> chunkBaseEntityIndices) where T : struct, IJobChunk
			{
				return JobChunkExtensions.ScheduleInternal<T>(ref jobData, query, dependsOn, ScheduleMode.Batched, chunkBaseEntityIndices);
			}

			// Token: 0x06001B94 RID: 7060 RVA: 0x0005F8C8 File Offset: 0x0005DAC8
			public static JobHandle ScheduleParallelByRef<T>(ref T jobData, EntityQuery query, JobHandle dependsOn, NativeArray<int> chunkBaseEntityIndices) where T : struct, IJobChunk
			{
				return JobChunkExtensions.ScheduleInternal<T>(ref jobData, query, dependsOn, ScheduleMode.Batched, chunkBaseEntityIndices);
			}
		}

		// Token: 0x02000458 RID: 1112
		// (Invoke) Token: 0x06001B96 RID: 7062
		internal delegate void MergeWith_0000184E$PostfixBurstDelegate(ref UnsafeList<ComponentType> mergeThese, ref UnsafeList<ComponentType> withThese);

		// Token: 0x02000459 RID: 1113
		internal static class MergeWith_0000184E$BurstDirectCall
		{
			// Token: 0x06001B99 RID: 7065 RVA: 0x0005F8D4 File Offset: 0x0005DAD4
			[BurstDiscard]
			private static void GetFunctionPointerDiscard(ref IntPtr A_0)
			{
				if (InternalCompilerInterface.MergeWith_0000184E$BurstDirectCall.Pointer == 0)
				{
					InternalCompilerInterface.MergeWith_0000184E$BurstDirectCall.Pointer = BurstCompiler.GetILPPMethodFunctionPointer2(InternalCompilerInterface.MergeWith_0000184E$BurstDirectCall.DeferredCompilation, methodof(InternalCompilerInterface.MergeWith$BurstManaged(ref UnsafeList<ComponentType>, ref UnsafeList<ComponentType>)).MethodHandle, typeof(InternalCompilerInterface.MergeWith_0000184E$PostfixBurstDelegate).TypeHandle);
				}
				A_0 = InternalCompilerInterface.MergeWith_0000184E$BurstDirectCall.Pointer;
			}

			// Token: 0x06001B9A RID: 7066 RVA: 0x0005F900 File Offset: 0x0005DB00
			private static IntPtr GetFunctionPointer()
			{
				IntPtr intPtr = (IntPtr)0;
				InternalCompilerInterface.MergeWith_0000184E$BurstDirectCall.GetFunctionPointerDiscard(ref intPtr);
				return intPtr;
			}

			// Token: 0x06001B9B RID: 7067 RVA: 0x0005F918 File Offset: 0x0005DB18
			public static void Constructor()
			{
				InternalCompilerInterface.MergeWith_0000184E$BurstDirectCall.DeferredCompilation = BurstCompiler.CompileILPPMethod2(methodof(InternalCompilerInterface.MergeWith(ref UnsafeList<ComponentType>, ref UnsafeList<ComponentType>)).MethodHandle);
			}

			// Token: 0x06001B9C RID: 7068 RVA: 0x00002287 File Offset: 0x00000487
			public static void Initialize()
			{
			}

			// Token: 0x06001B9D RID: 7069 RVA: 0x0005F929 File Offset: 0x0005DB29
			// Note: this type is marked as 'beforefieldinit'.
			static MergeWith_0000184E$BurstDirectCall()
			{
				InternalCompilerInterface.MergeWith_0000184E$BurstDirectCall.Constructor();
			}

			// Token: 0x06001B9E RID: 7070 RVA: 0x0005F930 File Offset: 0x0005DB30
			public static void Invoke(ref UnsafeList<ComponentType> mergeThese, ref UnsafeList<ComponentType> withThese)
			{
				if (BurstCompiler.IsEnabled)
				{
					IntPtr functionPointer = InternalCompilerInterface.MergeWith_0000184E$BurstDirectCall.GetFunctionPointer();
					if (functionPointer != 0)
					{
						calli(System.Void(Unity.Collections.LowLevel.Unsafe.UnsafeList`1<Unity.Entities.ComponentType>&,Unity.Collections.LowLevel.Unsafe.UnsafeList`1<Unity.Entities.ComponentType>&), ref mergeThese, ref withThese, functionPointer);
						return;
					}
				}
				InternalCompilerInterface.MergeWith$BurstManaged(ref mergeThese, ref withThese);
			}

			// Token: 0x04000982 RID: 2434
			private static IntPtr Pointer;

			// Token: 0x04000983 RID: 2435
			private static IntPtr DeferredCompilation;
		}
	}
}
