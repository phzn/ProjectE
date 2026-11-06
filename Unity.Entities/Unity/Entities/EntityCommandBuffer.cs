using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;
using Unity.Burst;
using Unity.Burst.CompilerServices;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Jobs.LowLevel.Unsafe;
using Unity.Profiling;
using Unity.Properties;

namespace Unity.Entities
{
	// Token: 0x02000111 RID: 273
	[NativeContainer]
	[BurstCompile]
	[DebuggerTypeProxy(typeof(EntityCommandBuffer.EntityCommandBufferDebugView))]
	public struct EntityCommandBuffer : IDisposable
	{
		// Token: 0x1700008D RID: 141
		// (get) Token: 0x06000449 RID: 1097 RVA: 0x0001939E File Offset: 0x0001759E
		// (set) Token: 0x0600044A RID: 1098 RVA: 0x000193AB File Offset: 0x000175AB
		internal unsafe static int ms_CommandBufferIDAllocator
		{
			get
			{
				return *EntityCommandBuffer._ms_CommandBufferIDAllocator.Data;
			}
			set
			{
				*EntityCommandBuffer._ms_CommandBufferIDAllocator.Data = value;
			}
		}

		// Token: 0x1700008E RID: 142
		// (get) Token: 0x0600044B RID: 1099 RVA: 0x000193B9 File Offset: 0x000175B9
		// (set) Token: 0x0600044C RID: 1100 RVA: 0x000193DA File Offset: 0x000175DA
		public unsafe int MinimumChunkSize
		{
			get
			{
				if (this.m_Data->m_MinimumChunkSize <= 0)
				{
					return 4096;
				}
				return this.m_Data->m_MinimumChunkSize;
			}
			set
			{
				this.m_Data->m_MinimumChunkSize = Math.Max(0, value);
			}
		}

		// Token: 0x1700008F RID: 143
		// (get) Token: 0x0600044D RID: 1101 RVA: 0x000193EE File Offset: 0x000175EE
		// (set) Token: 0x0600044E RID: 1102 RVA: 0x00019407 File Offset: 0x00017607
		public unsafe bool ShouldPlayback
		{
			get
			{
				return this.m_Data != null && this.m_Data->m_ShouldPlayback;
			}
			set
			{
				if (this.m_Data != null)
				{
					this.m_Data->m_ShouldPlayback = value;
				}
			}
		}

		// Token: 0x17000090 RID: 144
		// (get) Token: 0x0600044F RID: 1103 RVA: 0x0001941F File Offset: 0x0001761F
		public unsafe bool IsEmpty
		{
			get
			{
				return this.m_Data == null || this.m_Data->m_RecordedChainCount == 0;
			}
		}

		// Token: 0x06000450 RID: 1104 RVA: 0x00002287 File Offset: 0x00000487
		[Conditional("ENABLE_UNITY_COLLECTIONS_CHECKS")]
		[Conditional("UNITY_DOTS_DEBUG")]
		internal void EnforceSingleThreadOwnership()
		{
		}

		// Token: 0x06000451 RID: 1105 RVA: 0x00002287 File Offset: 0x00000487
		[Conditional("ENABLE_UNITY_COLLECTIONS_CHECKS")]
		[Conditional("UNITY_DOTS_DEBUG")]
		internal void AssertDidNotPlayback()
		{
		}

		// Token: 0x06000452 RID: 1106 RVA: 0x0001943B File Offset: 0x0001763B
		public EntityCommandBuffer(AllocatorManager.AllocatorHandle allocator)
		{
			this = new EntityCommandBuffer(allocator, PlaybackPolicy.SinglePlayback);
		}

		// Token: 0x06000453 RID: 1107 RVA: 0x00019445 File Offset: 0x00017645
		public EntityCommandBuffer(Allocator label, PlaybackPolicy playbackPolicy)
		{
			this = new EntityCommandBuffer(label, playbackPolicy);
		}

		// Token: 0x06000454 RID: 1108 RVA: 0x00019454 File Offset: 0x00017654
		public unsafe EntityCommandBuffer(AllocatorManager.AllocatorHandle allocator, PlaybackPolicy playbackPolicy)
		{
			this.m_Data = (EntityCommandBufferData*)Memory.Unmanaged.Allocate((long)sizeof(EntityCommandBufferData), UnsafeUtility.AlignOf<EntityCommandBufferData>(), allocator);
			this.m_Data->m_Allocator = allocator;
			this.m_Data->m_PlaybackPolicy = playbackPolicy;
			this.m_Data->m_MinimumChunkSize = 4096;
			this.m_Data->m_ShouldPlayback = true;
			this.m_Data->m_DidPlayback = false;
			this.m_Data->m_BufferWithFixupsCount = 0;
			this.m_Data->m_BufferWithFixups = new UnsafeAtomicCounter32((void*)(&this.m_Data->m_BufferWithFixupsCount));
			this.m_Data->m_CommandBufferID = --EntityCommandBuffer.ms_CommandBufferIDAllocator;
			this.m_Data->m_MainThreadChain = default(EntityCommandBufferChain);
			this.m_Data->m_ThreadedChains = null;
			this.m_Data->m_RecordedChainCount = 0;
			this.SystemID = 0;
			this.PassedPrePlaybackValidation = 0;
			this.OriginSystemHandle = default(SystemHandle);
			this.m_Data->m_Entity = default(Entity);
			this.m_Data->m_Entity.Version = this.m_Data->m_CommandBufferID;
			this.m_Data->m_BufferWithFixups.Reset(0);
		}

		// Token: 0x17000091 RID: 145
		// (get) Token: 0x06000455 RID: 1109 RVA: 0x0001957A File Offset: 0x0001777A
		public bool IsCreated
		{
			get
			{
				return this.m_Data != null;
			}
		}

		// Token: 0x06000456 RID: 1110 RVA: 0x00019589 File Offset: 0x00017789
		[BurstCompile]
		private static void DisposeInternal(ref EntityCommandBuffer ecb)
		{
			EntityCommandBuffer.DisposeInternal_000003D2$BurstDirectCall.Invoke(ref ecb);
		}

		// Token: 0x06000457 RID: 1111 RVA: 0x00019591 File Offset: 0x00017791
		public void Dispose()
		{
			EntityCommandBuffer.DisposeInternal(ref this);
		}

		// Token: 0x06000458 RID: 1112 RVA: 0x0001959C File Offset: 0x0001779C
		private unsafe void FreeChain(EntityCommandBufferChain* chain, PlaybackPolicy playbackPolicy, bool didPlayback)
		{
			if (chain->m_Head == null)
			{
				return;
			}
			bool flag = true;
			while (chain != null)
			{
				ECBInterop.CleanupManaged(chain);
				if (playbackPolicy == PlaybackPolicy.MultiPlayback || !didPlayback)
				{
					BufferHeaderNode* prev;
					for (BufferHeaderNode* ptr = chain->m_Cleanup->BufferCleanupList; ptr != null; ptr = prev)
					{
						prev = ptr->Prev;
						BufferHeader.Destroy(&ptr->TempBuffer);
					}
				}
				chain->m_Cleanup->BufferCleanupList = null;
				EntityNode* prev2;
				for (EntityNode* ptr2 = chain->m_Cleanup->EntityArraysCleanupList; ptr2 != null; ptr2 = prev2)
				{
					prev2 = ptr2->Prev;
					Memory.Unmanaged.Free<Entity>(ptr2->Ptr, this.m_Data->m_Allocator);
				}
				chain->m_Cleanup->EntityArraysCleanupList = null;
				Memory.Unmanaged.Free<ChainCleanup>(chain->m_Cleanup, this.m_Data->m_Allocator);
				while (chain->m_Tail != null)
				{
					ECBChunk* prev3 = chain->m_Tail->Prev;
					Memory.Unmanaged.Free<ECBChunk>(chain->m_Tail, this.m_Data->m_Allocator);
					chain->m_Tail = prev3;
				}
				chain->m_Head = null;
				EntityCommandBufferChain* ptr3 = chain;
				chain = chain->m_NextChain;
				ptr3->m_NextChain = null;
				if (!flag)
				{
					Memory.Unmanaged.Free<EntityCommandBufferChain>(ptr3, this.m_Data->m_Allocator);
				}
				flag = false;
			}
		}

		// Token: 0x17000092 RID: 146
		// (get) Token: 0x06000459 RID: 1113 RVA: 0x000196C5 File Offset: 0x000178C5
		internal int MainThreadSortKey
		{
			get
			{
				return int.MaxValue;
			}
		}

		// Token: 0x0600045A RID: 1114 RVA: 0x000196CC File Offset: 0x000178CC
		[SupportedInEntitiesForEach]
		public Entity CreateEntity(EntityArchetype archetype)
		{
			return this._CreateEntity(archetype);
		}

		// Token: 0x0600045B RID: 1115 RVA: 0x000196D8 File Offset: 0x000178D8
		[SupportedInEntitiesForEach]
		public Entity CreateEntity()
		{
			return this._CreateEntity(default(EntityArchetype));
		}

		// Token: 0x0600045C RID: 1116 RVA: 0x000196F4 File Offset: 0x000178F4
		private unsafe Entity _CreateEntity(EntityArchetype archetype)
		{
			EntityCommandBufferData* data = this.m_Data;
			int num = data->m_Entity.Index - 1;
			data->m_Entity.Index = num;
			int num2 = num;
			this.m_Data->AddCreateCommand(&this.m_Data->m_MainThreadChain, this.MainThreadSortKey, ECBCommand.CreateEntity, num2, archetype, true);
			return this.m_Data->m_Entity;
		}

		// Token: 0x0600045D RID: 1117 RVA: 0x00019748 File Offset: 0x00017948
		[SupportedInEntitiesForEach]
		public unsafe Entity Instantiate(Entity e)
		{
			EntityCommandBufferData* data = this.m_Data;
			int num = data->m_Entity.Index - 1;
			data->m_Entity.Index = num;
			int num2 = num;
			this.m_Data->AddEntityCommand(&this.m_Data->m_MainThreadChain, this.MainThreadSortKey, ECBCommand.InstantiateEntity, num2, e, true);
			return this.m_Data->m_Entity;
		}

		// Token: 0x0600045E RID: 1118 RVA: 0x0001979C File Offset: 0x0001799C
		[SupportedInEntitiesForEach]
		public unsafe void Instantiate(Entity e, NativeArray<Entity> entities)
		{
			Entity entity = this.m_Data->m_Entity;
			int num = Interlocked.Add(ref this.m_Data->m_Entity.Index, -entities.Length) + entities.Length - 1;
			for (int i = 0; i < entities.Length; i++)
			{
				entity.Index = num - i;
				entities[i] = entity;
			}
			this.m_Data->AddMultipleEntityCommand(&this.m_Data->m_MainThreadChain, this.MainThreadSortKey, ECBCommand.InstantiateEntity, num, entities.Length, e, true);
		}

		// Token: 0x0600045F RID: 1119 RVA: 0x0001982A File Offset: 0x00017A2A
		[SupportedInEntitiesForEach]
		public unsafe void DestroyEntity(Entity e)
		{
			this.m_Data->AddEntityCommand(&this.m_Data->m_MainThreadChain, this.MainThreadSortKey, ECBCommand.DestroyEntity, 0, e, false);
		}

		// Token: 0x06000460 RID: 1120 RVA: 0x00019850 File Offset: 0x00017A50
		[SupportedInEntitiesForEach]
		public unsafe void DestroyEntity(NativeArray<Entity> entities)
		{
			bool flag;
			Entity* ptr = this.m_Data->CloneAndSearchForDeferredEntities(entities, out flag);
			this.m_Data->AppendMultipleEntitiesCommand(&this.m_Data->m_MainThreadChain, this.MainThreadSortKey, ECBCommand.DestroyMultipleEntities, ptr, entities.Length, flag);
		}

		// Token: 0x06000461 RID: 1121 RVA: 0x00019895 File Offset: 0x00017A95
		[SupportedInEntitiesForEach]
		public unsafe DynamicBuffer<T> AddBuffer<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(Entity e) where T : struct, ValueType, IBufferElementData
		{
			return this.m_Data->CreateBufferCommand<T>(ECBCommand.AddBuffer, &this.m_Data->m_MainThreadChain, this.MainThreadSortKey, e);
		}

		// Token: 0x06000462 RID: 1122 RVA: 0x000198B7 File Offset: 0x00017AB7
		[SupportedInEntitiesForEach]
		public unsafe DynamicBuffer<T> SetBuffer<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(Entity e) where T : struct, ValueType, IBufferElementData
		{
			return this.m_Data->CreateBufferCommand<T>(ECBCommand.SetBuffer, &this.m_Data->m_MainThreadChain, this.MainThreadSortKey, e);
		}

		// Token: 0x06000463 RID: 1123 RVA: 0x000198D9 File Offset: 0x00017AD9
		[SupportedInEntitiesForEach]
		public unsafe void AppendToBuffer<T>(Entity e, T element) where T : struct, IBufferElementData
		{
			this.m_Data->AppendToBufferCommand<T>(&this.m_Data->m_MainThreadChain, this.MainThreadSortKey, e, element);
		}

		// Token: 0x06000464 RID: 1124 RVA: 0x000198FA File Offset: 0x00017AFA
		[SupportedInEntitiesForEach]
		public unsafe void AddComponent<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(Entity e, T component) where T : struct, ValueType, IComponentData
		{
			this.m_Data->AddEntityComponentTypeWithValueCommand<T>(&this.m_Data->m_MainThreadChain, this.MainThreadSortKey, ECBCommand.AddComponent, e, component);
		}

		// Token: 0x06000465 RID: 1125 RVA: 0x0001991C File Offset: 0x00017B1C
		[SupportedInEntitiesForEach]
		public unsafe void AddComponent<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(NativeArray<Entity> entities, T component) where T : struct, ValueType, IComponentData
		{
			bool flag;
			Entity* ptr = this.m_Data->CloneAndSearchForDeferredEntities(entities, out flag);
			this.m_Data->AppendMultipleEntitiesComponentCommandWithValue<T>(&this.m_Data->m_MainThreadChain, this.MainThreadSortKey, ECBCommand.AddComponentForMultipleEntities, ptr, entities.Length, flag, component);
		}

		// Token: 0x06000466 RID: 1126 RVA: 0x00019962 File Offset: 0x00017B62
		[SupportedInEntitiesForEach]
		public unsafe void AddComponent<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(Entity e) where T : struct, ValueType, IComponentData
		{
			this.m_Data->AddEntityComponentTypeWithoutValueCommand(&this.m_Data->m_MainThreadChain, this.MainThreadSortKey, ECBCommand.AddComponent, e, ComponentType.ReadWrite<T>());
		}

		// Token: 0x06000467 RID: 1127 RVA: 0x00019988 File Offset: 0x00017B88
		[SupportedInEntitiesForEach]
		public unsafe void AddComponent<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(NativeArray<Entity> entities) where T : struct, ValueType, IComponentData
		{
			bool flag;
			Entity* ptr = this.m_Data->CloneAndSearchForDeferredEntities(entities, out flag);
			this.m_Data->AppendMultipleEntitiesComponentCommand(&this.m_Data->m_MainThreadChain, this.MainThreadSortKey, ECBCommand.AddComponentForMultipleEntities, ptr, entities.Length, flag, ComponentType.ReadWrite<T>());
		}

		// Token: 0x06000468 RID: 1128 RVA: 0x000199D2 File Offset: 0x00017BD2
		[SupportedInEntitiesForEach]
		public unsafe void AddComponent(Entity e, ComponentType componentType)
		{
			this.m_Data->AddEntityComponentTypeWithoutValueCommand(&this.m_Data->m_MainThreadChain, this.MainThreadSortKey, ECBCommand.AddComponent, e, componentType);
		}

		// Token: 0x06000469 RID: 1129 RVA: 0x000199F4 File Offset: 0x00017BF4
		[SupportedInEntitiesForEach]
		internal unsafe void UnsafeAddComponent(Entity e, TypeIndex typeIndex, int typeSize, void* componentDataPtr)
		{
			this.m_Data->UnsafeAddEntityComponentCommand(&this.m_Data->m_MainThreadChain, this.MainThreadSortKey, ECBCommand.AddComponent, e, typeIndex, typeSize, componentDataPtr);
		}

		// Token: 0x0600046A RID: 1130 RVA: 0x00019A1C File Offset: 0x00017C1C
		[SupportedInEntitiesForEach]
		public unsafe void AddComponent(NativeArray<Entity> entities, ComponentType componentType)
		{
			bool flag;
			Entity* ptr = this.m_Data->CloneAndSearchForDeferredEntities(entities, out flag);
			this.m_Data->AppendMultipleEntitiesComponentCommand(&this.m_Data->m_MainThreadChain, this.MainThreadSortKey, ECBCommand.AddComponentForMultipleEntities, ptr, entities.Length, flag, componentType);
		}

		// Token: 0x0600046B RID: 1131 RVA: 0x00019A62 File Offset: 0x00017C62
		[SupportedInEntitiesForEach]
		public unsafe void AddComponent(Entity e, in ComponentTypeSet componentTypeSet)
		{
			this.m_Data->AddEntityComponentTypesCommand(&this.m_Data->m_MainThreadChain, this.MainThreadSortKey, ECBCommand.AddMultipleComponents, e, in componentTypeSet);
		}

		// Token: 0x0600046C RID: 1132 RVA: 0x00019A84 File Offset: 0x00017C84
		[SupportedInEntitiesForEach]
		public unsafe void AddComponent(NativeArray<Entity> entities, in ComponentTypeSet componentTypeSet)
		{
			bool flag;
			Entity* ptr = this.m_Data->CloneAndSearchForDeferredEntities(entities, out flag);
			this.m_Data->AppendMultipleEntitiesMultipleComponentsCommand(&this.m_Data->m_MainThreadChain, this.MainThreadSortKey, ECBCommand.AddMultipleComponentsForMultipleEntities, ptr, entities.Length, flag, in componentTypeSet);
		}

		// Token: 0x0600046D RID: 1133 RVA: 0x00019ACA File Offset: 0x00017CCA
		[SupportedInEntitiesForEach]
		public unsafe void SetComponent<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(Entity e, T component) where T : struct, ValueType, IComponentData
		{
			this.m_Data->AddEntityComponentTypeWithValueCommand<T>(&this.m_Data->m_MainThreadChain, this.MainThreadSortKey, ECBCommand.SetComponent, e, component);
		}

		// Token: 0x0600046E RID: 1134 RVA: 0x00019AEC File Offset: 0x00017CEC
		[SupportedInEntitiesForEach]
		internal unsafe void UnsafeSetComponent(Entity e, TypeIndex typeIndex, int typeSize, void* componentDataPtr)
		{
			this.m_Data->UnsafeAddEntityComponentCommand(&this.m_Data->m_MainThreadChain, this.MainThreadSortKey, ECBCommand.SetComponent, e, typeIndex, typeSize, componentDataPtr);
		}

		// Token: 0x0600046F RID: 1135 RVA: 0x00019B11 File Offset: 0x00017D11
		[GenerateTestsForBurstCompatibility]
		public unsafe void SetEnabled(Entity e, bool value)
		{
			this.m_Data->AddEntityEnabledCommand(&this.m_Data->m_MainThreadChain, this.MainThreadSortKey, ECBCommand.SetEntityEnabled, e, value);
		}

		// Token: 0x06000470 RID: 1136 RVA: 0x00019B33 File Offset: 0x00017D33
		[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[] { typeof(BurstCompatibleEnableableComponent) })]
		[SupportedInEntitiesForEach]
		public unsafe void SetComponentEnabled<T>(Entity e, bool value) where T : struct, IEnableableComponent
		{
			this.m_Data->AddEntityComponentEnabledCommand(&this.m_Data->m_MainThreadChain, this.MainThreadSortKey, ECBCommand.SetComponentEnabled, e, TypeManager.GetTypeIndex<T>(), value);
		}

		// Token: 0x06000471 RID: 1137 RVA: 0x00019B5B File Offset: 0x00017D5B
		[SupportedInEntitiesForEach]
		public unsafe void SetComponentEnabled(Entity e, ComponentType componentType, bool value)
		{
			this.m_Data->AddEntityComponentEnabledCommand(&this.m_Data->m_MainThreadChain, this.MainThreadSortKey, ECBCommand.SetComponentEnabled, e, componentType.TypeIndex, value);
		}

		// Token: 0x06000472 RID: 1138 RVA: 0x00019B84 File Offset: 0x00017D84
		[SupportedInEntitiesForEach]
		public unsafe void SetName(Entity e, in FixedString64Bytes name)
		{
			this.m_Data->AddEntityNameCommand(&this.m_Data->m_MainThreadChain, this.MainThreadSortKey, ECBCommand.SetName, e, in name);
		}

		// Token: 0x06000473 RID: 1139 RVA: 0x00019BA7 File Offset: 0x00017DA7
		[SupportedInEntitiesForEach]
		public void RemoveComponent<T>(Entity e)
		{
			this.RemoveComponent(e, ComponentType.ReadWrite<T>());
		}

		// Token: 0x06000474 RID: 1140 RVA: 0x00019BB5 File Offset: 0x00017DB5
		[SupportedInEntitiesForEach]
		public void RemoveComponent<T>(NativeArray<Entity> entities)
		{
			this.RemoveComponent(entities, ComponentType.ReadWrite<T>());
		}

		// Token: 0x06000475 RID: 1141 RVA: 0x00019BC3 File Offset: 0x00017DC3
		[SupportedInEntitiesForEach]
		public unsafe void RemoveComponent(Entity e, ComponentType componentType)
		{
			this.m_Data->AddEntityComponentTypeWithoutValueCommand(&this.m_Data->m_MainThreadChain, this.MainThreadSortKey, ECBCommand.RemoveComponent, e, componentType);
		}

		// Token: 0x06000476 RID: 1142 RVA: 0x00019BE8 File Offset: 0x00017DE8
		[SupportedInEntitiesForEach]
		public unsafe void RemoveComponent(NativeArray<Entity> entities, ComponentType componentType)
		{
			bool flag;
			Entity* ptr = this.m_Data->CloneAndSearchForDeferredEntities(entities, out flag);
			this.m_Data->AppendMultipleEntitiesComponentCommand(&this.m_Data->m_MainThreadChain, this.MainThreadSortKey, ECBCommand.RemoveComponentForMultipleEntities, ptr, entities.Length, flag, componentType);
		}

		// Token: 0x06000477 RID: 1143 RVA: 0x00019C2E File Offset: 0x00017E2E
		[SupportedInEntitiesForEach]
		public unsafe void RemoveComponent(Entity e, in ComponentTypeSet componentTypeSet)
		{
			this.m_Data->AddEntityComponentTypesCommand(&this.m_Data->m_MainThreadChain, this.MainThreadSortKey, ECBCommand.RemoveMultipleComponents, e, in componentTypeSet);
		}

		// Token: 0x06000478 RID: 1144 RVA: 0x00019C50 File Offset: 0x00017E50
		[SupportedInEntitiesForEach]
		public unsafe void RemoveComponent(NativeArray<Entity> entities, in ComponentTypeSet componentTypeSet)
		{
			bool flag;
			Entity* ptr = this.m_Data->CloneAndSearchForDeferredEntities(entities, out flag);
			this.m_Data->AppendMultipleEntitiesMultipleComponentsCommand(&this.m_Data->m_MainThreadChain, this.MainThreadSortKey, ECBCommand.RemoveMultipleComponentsForMultipleEntities, ptr, entities.Length, flag, in componentTypeSet);
		}

		// Token: 0x06000479 RID: 1145 RVA: 0x00019C98 File Offset: 0x00017E98
		[SupportedInEntitiesForEach]
		public unsafe void AddComponent(EntityQuery entityQuery, ComponentType componentType, EntityQueryCaptureMode queryCaptureMode)
		{
			if (queryCaptureMode == EntityQueryCaptureMode.AtRecord)
			{
				this.m_Data->AppendMultipleEntitiesComponentCommand(&this.m_Data->m_MainThreadChain, this.MainThreadSortKey, ECBCommand.AddComponentForMultipleEntities, entityQuery, componentType);
				return;
			}
			this.m_Data->AppendEntityQueryComponentCommand(&this.m_Data->m_MainThreadChain, this.MainThreadSortKey, ECBCommand.AddComponentForEntityQuery, entityQuery, componentType);
		}

		// Token: 0x0600047A RID: 1146 RVA: 0x00019CED File Offset: 0x00017EED
		[SupportedInEntitiesForEach]
		[Obsolete("This method now takes an extra parameter to control when the query is evaluated. To preserve the current semantics, use EntityQueryCaptureMode.AtRecord (RemovedAfter Entities 2.0)")]
		public void AddComponent(EntityQuery entityQuery, ComponentType componentType)
		{
			this.AddComponent(entityQuery, componentType, EntityQueryCaptureMode.AtRecord);
		}

		// Token: 0x0600047B RID: 1147 RVA: 0x00019CED File Offset: 0x00017EED
		[SupportedInEntitiesForEach]
		[Obsolete("Use AddComponent (RemovedAfter Entities 1.0) (UnityUpgradable) -> AddComponent(*)")]
		public void AddComponentForEntityQuery(EntityQuery entityQuery, ComponentType componentType)
		{
			this.AddComponent(entityQuery, componentType, EntityQueryCaptureMode.AtRecord);
		}

		// Token: 0x0600047C RID: 1148 RVA: 0x00019CF8 File Offset: 0x00017EF8
		[SupportedInEntitiesForEach]
		public void AddComponent<T>(EntityQuery entityQuery, EntityQueryCaptureMode queryCaptureMode)
		{
			this.AddComponent(entityQuery, ComponentType.ReadWrite<T>(), queryCaptureMode);
		}

		// Token: 0x0600047D RID: 1149 RVA: 0x00019D07 File Offset: 0x00017F07
		[SupportedInEntitiesForEach]
		[Obsolete("This method now takes an extra parameter to control when the query is evaluated. To preserve the current semantics, use EntityQueryCaptureMode.AtRecord (RemovedAfter Entities 2.0)")]
		public void AddComponent<T>(EntityQuery entityQuery)
		{
			this.AddComponent(entityQuery, ComponentType.ReadWrite<T>(), EntityQueryCaptureMode.AtRecord);
		}

		// Token: 0x0600047E RID: 1150 RVA: 0x00019D07 File Offset: 0x00017F07
		[SupportedInEntitiesForEach]
		[Obsolete("Use AddComponent (RemovedAfter Entities 1.0) (UnityUpgradable) -> AddComponent<T>(*)")]
		public void AddComponentForEntityQuery<T>(EntityQuery entityQuery)
		{
			this.AddComponent(entityQuery, ComponentType.ReadWrite<T>(), EntityQueryCaptureMode.AtRecord);
		}

		// Token: 0x0600047F RID: 1151 RVA: 0x00019D16 File Offset: 0x00017F16
		[SupportedInEntitiesForEach]
		public unsafe void AddComponent<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(EntityQuery entityQuery, T value) where T : struct, ValueType, IComponentData
		{
			this.m_Data->AppendMultipleEntitiesComponentCommandWithValue<T>(&this.m_Data->m_MainThreadChain, this.MainThreadSortKey, ECBCommand.AddComponentForMultipleEntities, entityQuery, value);
		}

		// Token: 0x06000480 RID: 1152 RVA: 0x00019D3A File Offset: 0x00017F3A
		[SupportedInEntitiesForEach]
		[Obsolete("Use AddComponent (RemovedAfter Entities 1.0) (UnityUpgradable) -> AddComponent<T>(*)")]
		public void AddComponentForEntityQuery<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(EntityQuery entityQuery, T value) where T : struct, ValueType, IComponentData
		{
			this.AddComponent<T>(entityQuery, value);
		}

		// Token: 0x06000481 RID: 1153 RVA: 0x00019D44 File Offset: 0x00017F44
		[SupportedInEntitiesForEach]
		public unsafe void AddComponent(EntityQuery entityQuery, in ComponentTypeSet componentTypeSet, EntityQueryCaptureMode queryCaptureMode)
		{
			if (queryCaptureMode == EntityQueryCaptureMode.AtRecord)
			{
				this.m_Data->AppendMultipleEntitiesMultipleComponentsCommand(&this.m_Data->m_MainThreadChain, this.MainThreadSortKey, ECBCommand.AddMultipleComponentsForMultipleEntities, entityQuery, in componentTypeSet);
				return;
			}
			this.m_Data->AppendEntityQueryComponentTypeSetCommand(&this.m_Data->m_MainThreadChain, this.MainThreadSortKey, ECBCommand.AddMultipleComponentsForEntityQuery, entityQuery, in componentTypeSet);
		}

		// Token: 0x06000482 RID: 1154 RVA: 0x00019D99 File Offset: 0x00017F99
		[SupportedInEntitiesForEach]
		[Obsolete("This method now takes an extra parameter to control when the query is evaluated. To preserve the current semantics, use EntityQueryCaptureMode.AtRecord (RemovedAfter Entities 2.0)")]
		public void AddComponent(EntityQuery entityQuery, in ComponentTypeSet componentTypeSet)
		{
			this.AddComponent(entityQuery, in componentTypeSet, EntityQueryCaptureMode.AtRecord);
		}

		// Token: 0x06000483 RID: 1155 RVA: 0x00019D99 File Offset: 0x00017F99
		[SupportedInEntitiesForEach]
		[Obsolete("Use AddComponent (RemovedAfter Entities 1.0) (UnityUpgradable) -> AddComponent(*)")]
		public void AddComponentForEntityQuery(EntityQuery entityQuery, in ComponentTypeSet componentTypeSet)
		{
			this.AddComponent(entityQuery, in componentTypeSet, EntityQueryCaptureMode.AtRecord);
		}

		// Token: 0x06000484 RID: 1156 RVA: 0x00019DA4 File Offset: 0x00017FA4
		[SupportedInEntitiesForEach]
		public unsafe void AddSharedComponentManaged<T>(EntityQuery entityQuery, T component, EntityQueryCaptureMode queryCaptureMode) where T : struct, ISharedComponentData
		{
			bool flag = TypeManager.IsManagedSharedComponent(TypeManager.GetTypeIndex<T>());
			int num;
			bool flag2 = EntityCommandBuffer.IsDefaultObject<T>(ref component, out num);
			if (flag)
			{
				if (queryCaptureMode == EntityQueryCaptureMode.AtRecord)
				{
					this.m_Data->AppendMultipleEntitiesComponentCommandWithSharedValue<T>(&this.m_Data->m_MainThreadChain, this.MainThreadSortKey, ECBCommand.AddSharedComponentWithValueForMultipleEntities, entityQuery, num, flag2 ? null : component);
					return;
				}
				this.m_Data->AppendEntityQueryComponentCommandWithSharedValue<T>(&this.m_Data->m_MainThreadChain, this.MainThreadSortKey, ECBCommand.AddSharedComponentWithValueForEntityQuery, entityQuery, num, flag2 ? null : component);
				return;
			}
			else
			{
				void* ptr = UnsafeUtility.AddressOf<T>(ref component);
				if (queryCaptureMode == EntityQueryCaptureMode.AtRecord)
				{
					this.m_Data->AppendMultipleEntitiesCommand_WithUnmanagedSharedValue<T>(&this.m_Data->m_MainThreadChain, this.MainThreadSortKey, ECBCommand.AddUnmanagedSharedComponentValueForMultipleEntities, entityQuery, false, num, flag2 ? null : ptr);
					return;
				}
				this.m_Data->AppendEntityQueryComponentCommandWithUnmanagedSharedValue<T>(&this.m_Data->m_MainThreadChain, this.MainThreadSortKey, ECBCommand.AddUnmanagedSharedComponentValueForEntityQuery, entityQuery, num, flag2 ? null : ptr);
				return;
			}
		}

		// Token: 0x06000485 RID: 1157 RVA: 0x00019E89 File Offset: 0x00018089
		[SupportedInEntitiesForEach]
		[Obsolete("This method now takes an extra parameter to control when the query is evaluated. To preserve the current semantics, use EntityQueryCaptureMode.AtRecord (RemovedAfter Entities 2.0)")]
		public void AddSharedComponentManaged<T>(EntityQuery entityQuery, T component) where T : struct, ISharedComponentData
		{
			this.AddSharedComponentManaged<T>(entityQuery, component, EntityQueryCaptureMode.AtRecord);
		}

		// Token: 0x06000486 RID: 1158 RVA: 0x00019E89 File Offset: 0x00018089
		[SupportedInEntitiesForEach]
		[Obsolete("Use AddSharedComponentManaged (RemovedAfter Entities 1.0) (UnityUpgradable) -> AddSharedComponent<T>(*)")]
		public void AddSharedComponentForEntityQuery<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(EntityQuery entityQuery, T component) where T : struct, ValueType, ISharedComponentData
		{
			this.AddSharedComponentManaged<T>(entityQuery, component, EntityQueryCaptureMode.AtRecord);
		}

		// Token: 0x06000487 RID: 1159 RVA: 0x00019E94 File Offset: 0x00018094
		[SupportedInEntitiesForEach]
		public unsafe void AddSharedComponent<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(EntityQuery entityQuery, T component, EntityQueryCaptureMode queryCaptureMode) where T : struct, ValueType, ISharedComponentData
		{
			TypeManager.GetTypeIndex<T>();
			int num;
			bool flag = EntityCommandBuffer.IsDefaultObjectUnmanaged<T>(ref component, out num);
			void* ptr = UnsafeUtility.AddressOf<T>(ref component);
			if (queryCaptureMode == EntityQueryCaptureMode.AtRecord)
			{
				this.m_Data->AppendMultipleEntitiesCommand_WithUnmanagedSharedValue<T>(&this.m_Data->m_MainThreadChain, this.MainThreadSortKey, ECBCommand.AddUnmanagedSharedComponentValueForMultipleEntities, entityQuery, false, num, flag ? null : ptr);
				return;
			}
			this.m_Data->AppendEntityQueryComponentCommandWithUnmanagedSharedValue<T>(&this.m_Data->m_MainThreadChain, this.MainThreadSortKey, ECBCommand.AddUnmanagedSharedComponentValueForEntityQuery, entityQuery, num, flag ? null : ptr);
		}

		// Token: 0x06000488 RID: 1160 RVA: 0x00019F12 File Offset: 0x00018112
		[SupportedInEntitiesForEach]
		[Obsolete("This method now takes an extra parameter to control when the query is evaluated. To preserve the current semantics, use EntityQueryCaptureMode.AtRecord (RemovedAfter Entities 2.0)")]
		public void AddSharedComponent<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(EntityQuery entityQuery, T component) where T : struct, ValueType, ISharedComponentData
		{
			this.AddSharedComponent<T>(entityQuery, component, EntityQueryCaptureMode.AtRecord);
		}

		// Token: 0x06000489 RID: 1161 RVA: 0x00019F20 File Offset: 0x00018120
		[SupportedInEntitiesForEach]
		public unsafe void AddComponentObject(EntityQuery entityQuery, object componentData)
		{
			ComponentType componentType = componentData.GetType();
			this.m_Data->AppendMultipleEntitiesComponentCommandWithObject(&this.m_Data->m_MainThreadChain, this.MainThreadSortKey, ECBCommand.AddComponentObjectForMultipleEntities, entityQuery, componentData, componentType);
		}

		// Token: 0x0600048A RID: 1162 RVA: 0x00019F5C File Offset: 0x0001815C
		[SupportedInEntitiesForEach]
		[Obsolete("Use AddComponentObject (RemovedAfter Entities 1.0) (UnityUpgradable) -> AddComponentObject(*)")]
		public void AddComponentObjectForEntityQuery(EntityQuery entityQuery, object componentData)
		{
			this.AddComponentObject(entityQuery, componentData);
		}

		// Token: 0x0600048B RID: 1163 RVA: 0x00019F68 File Offset: 0x00018168
		[SupportedInEntitiesForEach]
		public unsafe void SetComponentObject(EntityQuery entityQuery, object componentData)
		{
			ComponentType componentType = componentData.GetType();
			this.m_Data->AppendMultipleEntitiesComponentCommandWithObject(&this.m_Data->m_MainThreadChain, this.MainThreadSortKey, ECBCommand.SetComponentObjectForMultipleEntities, entityQuery, componentData, componentType);
		}

		// Token: 0x0600048C RID: 1164 RVA: 0x00019FA4 File Offset: 0x000181A4
		[SupportedInEntitiesForEach]
		[Obsolete("Use SetComponentObject (RemovedAfter Entities 1.0) (UnityUpgradable) -> SetComponentObject(*)")]
		public void SetComponentObjectForEntityQuery(EntityQuery entityQuery, object componentData)
		{
			this.SetComponentObject(entityQuery, componentData);
		}

		// Token: 0x0600048D RID: 1165 RVA: 0x00019FB0 File Offset: 0x000181B0
		[SupportedInEntitiesForEach]
		public unsafe void SetSharedComponentManaged<T>(EntityQuery entityQuery, T component, EntityQueryCaptureMode queryCaptureMode) where T : struct, ISharedComponentData
		{
			bool flag = TypeManager.IsManagedSharedComponent(TypeManager.GetTypeIndex<T>());
			int num;
			bool flag2 = EntityCommandBuffer.IsDefaultObject<T>(ref component, out num);
			if (flag)
			{
				if (queryCaptureMode == EntityQueryCaptureMode.AtRecord)
				{
					this.m_Data->AppendMultipleEntitiesComponentCommandWithSharedValue<T>(&this.m_Data->m_MainThreadChain, this.MainThreadSortKey, ECBCommand.SetSharedComponentValueForMultipleEntities, entityQuery, num, flag2 ? null : component);
					return;
				}
				this.m_Data->AppendEntityQueryComponentCommandWithSharedValue<T>(&this.m_Data->m_MainThreadChain, this.MainThreadSortKey, ECBCommand.SetSharedComponentValueForEntityQuery, entityQuery, num, flag2 ? null : component);
				return;
			}
			else
			{
				void* ptr = UnsafeUtility.AddressOf<T>(ref component);
				if (queryCaptureMode == EntityQueryCaptureMode.AtRecord)
				{
					this.m_Data->AppendMultipleEntitiesCommand_WithUnmanagedSharedValue<T>(&this.m_Data->m_MainThreadChain, this.MainThreadSortKey, ECBCommand.SetUnmanagedSharedComponentValueForMultipleEntities, entityQuery, false, num, flag2 ? null : ptr);
					return;
				}
				this.m_Data->AppendEntityQueryComponentCommandWithUnmanagedSharedValue<T>(&this.m_Data->m_MainThreadChain, this.MainThreadSortKey, ECBCommand.SetUnmanagedSharedComponentValueForEntityQuery, entityQuery, num, flag2 ? null : ptr);
				return;
			}
		}

		// Token: 0x0600048E RID: 1166 RVA: 0x0001A095 File Offset: 0x00018295
		[SupportedInEntitiesForEach]
		[Obsolete("This method now takes an extra parameter to control when the query is evaluated. To preserve the current semantics, use EntityQueryCaptureMode.AtRecord (RemovedAfter Entities 2.0)")]
		public void SetSharedComponentManaged<T>(EntityQuery entityQuery, T component) where T : struct, ISharedComponentData
		{
			this.SetSharedComponentManaged<T>(entityQuery, component, EntityQueryCaptureMode.AtRecord);
		}

		// Token: 0x0600048F RID: 1167 RVA: 0x0001A095 File Offset: 0x00018295
		[SupportedInEntitiesForEach]
		[Obsolete("Use SetSharedComponentManaged (RemovedAfter Entities 1.0) (UnityUpgradable) -> SetSharedComponentManaged<T>(*)")]
		public void SetSharedComponentForEntityQueryManaged<T>(EntityQuery entityQuery, T component) where T : struct, ISharedComponentData
		{
			this.SetSharedComponentManaged<T>(entityQuery, component, EntityQueryCaptureMode.AtRecord);
		}

		// Token: 0x06000490 RID: 1168 RVA: 0x0001A0A0 File Offset: 0x000182A0
		[SupportedInEntitiesForEach]
		public unsafe void SetSharedComponent<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(EntityQuery entityQuery, T component, EntityQueryCaptureMode queryCaptureMode) where T : struct, ValueType, ISharedComponentData
		{
			TypeManager.GetTypeIndex<T>();
			int num;
			bool flag = EntityCommandBuffer.IsDefaultObjectUnmanaged<T>(ref component, out num);
			void* ptr = UnsafeUtility.AddressOf<T>(ref component);
			if (queryCaptureMode == EntityQueryCaptureMode.AtRecord)
			{
				this.m_Data->AppendMultipleEntitiesCommand_WithUnmanagedSharedValue<T>(&this.m_Data->m_MainThreadChain, this.MainThreadSortKey, ECBCommand.SetUnmanagedSharedComponentValueForMultipleEntities, entityQuery, false, num, flag ? null : ptr);
				return;
			}
			this.m_Data->AppendEntityQueryComponentCommandWithUnmanagedSharedValue<T>(&this.m_Data->m_MainThreadChain, this.MainThreadSortKey, ECBCommand.SetUnmanagedSharedComponentValueForEntityQuery, entityQuery, num, flag ? null : ptr);
		}

		// Token: 0x06000491 RID: 1169 RVA: 0x0001A11E File Offset: 0x0001831E
		[SupportedInEntitiesForEach]
		[Obsolete("This method now takes an extra parameter to control when the query is evaluated. To preserve the current semantics, use EntityQueryCaptureMode.AtRecord (RemovedAfter Entities 2.0)")]
		public void SetSharedComponent<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(EntityQuery entityQuery, T component) where T : struct, ValueType, ISharedComponentData
		{
			this.SetSharedComponent<T>(entityQuery, component, EntityQueryCaptureMode.AtRecord);
		}

		// Token: 0x06000492 RID: 1170 RVA: 0x0001A11E File Offset: 0x0001831E
		[SupportedInEntitiesForEach]
		[Obsolete("Use SetSharedComponent (RemovedAfter Entities 1.0) (UnityUpgradable) -> SetSharedComponent<T>(*)")]
		public void SetSharedComponentForEntityQuery<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(EntityQuery entityQuery, T component) where T : struct, ValueType, ISharedComponentData
		{
			this.SetSharedComponent<T>(entityQuery, component, EntityQueryCaptureMode.AtRecord);
		}

		// Token: 0x06000493 RID: 1171 RVA: 0x0001A12C File Offset: 0x0001832C
		[SupportedInEntitiesForEach]
		public unsafe void RemoveComponent(EntityQuery entityQuery, ComponentType componentType, EntityQueryCaptureMode queryCaptureMode)
		{
			if (queryCaptureMode == EntityQueryCaptureMode.AtRecord)
			{
				this.m_Data->AppendMultipleEntitiesComponentCommand(&this.m_Data->m_MainThreadChain, this.MainThreadSortKey, ECBCommand.RemoveComponentForMultipleEntities, entityQuery, componentType);
				return;
			}
			this.m_Data->AppendEntityQueryComponentCommand(&this.m_Data->m_MainThreadChain, this.MainThreadSortKey, ECBCommand.RemoveComponentForEntityQuery, entityQuery, componentType);
		}

		// Token: 0x06000494 RID: 1172 RVA: 0x0001A181 File Offset: 0x00018381
		[SupportedInEntitiesForEach]
		[Obsolete("This method now takes an extra parameter to control when the query is evaluated. To preserve the current semantics, use EntityQueryCaptureMode.AtRecord (RemovedAfter Entities 2.0)")]
		public void RemoveComponent(EntityQuery entityQuery, ComponentType componentType)
		{
			this.RemoveComponent(entityQuery, componentType, EntityQueryCaptureMode.AtRecord);
		}

		// Token: 0x06000495 RID: 1173 RVA: 0x0001A181 File Offset: 0x00018381
		[SupportedInEntitiesForEach]
		[Obsolete("Use RemoveComponent (RemovedAfter Entities 1.0) (UnityUpgradable) -> RemoveComponent(*)")]
		public void RemoveComponentForEntityQuery(EntityQuery entityQuery, ComponentType componentType)
		{
			this.RemoveComponent(entityQuery, componentType, EntityQueryCaptureMode.AtRecord);
		}

		// Token: 0x06000496 RID: 1174 RVA: 0x0001A18C File Offset: 0x0001838C
		[SupportedInEntitiesForEach]
		public void RemoveComponent<T>(EntityQuery entityQuery, EntityQueryCaptureMode queryCaptureMode)
		{
			this.RemoveComponent(entityQuery, ComponentType.ReadWrite<T>(), queryCaptureMode);
		}

		// Token: 0x06000497 RID: 1175 RVA: 0x0001A19B File Offset: 0x0001839B
		[SupportedInEntitiesForEach]
		[Obsolete("This method now takes an extra parameter to control when the query is evaluated. To preserve the current semantics, use EntityQueryCaptureMode.AtRecord (RemovedAfter Entities 2.0)")]
		public void RemoveComponent<T>(EntityQuery entityQuery)
		{
			this.RemoveComponent(entityQuery, ComponentType.ReadWrite<T>(), EntityQueryCaptureMode.AtRecord);
		}

		// Token: 0x06000498 RID: 1176 RVA: 0x0001A1AA File Offset: 0x000183AA
		[SupportedInEntitiesForEach]
		[Obsolete("Use RemoveComponent (RemovedAfter Entities 1.0) (UnityUpgradable) -> RemoveComponent<T>(*)")]
		public void RemoveComponentForEntityQuery<T>(EntityQuery entityQuery)
		{
			this.RemoveComponent<T>(entityQuery, EntityQueryCaptureMode.AtRecord);
		}

		// Token: 0x06000499 RID: 1177 RVA: 0x0001A1B4 File Offset: 0x000183B4
		[SupportedInEntitiesForEach]
		public unsafe void RemoveComponent(EntityQuery entityQuery, in ComponentTypeSet componentTypeSet, EntityQueryCaptureMode queryCaptureMode)
		{
			if (queryCaptureMode == EntityQueryCaptureMode.AtRecord)
			{
				this.m_Data->AppendMultipleEntitiesMultipleComponentsCommand(&this.m_Data->m_MainThreadChain, this.MainThreadSortKey, ECBCommand.RemoveMultipleComponentsForMultipleEntities, entityQuery, in componentTypeSet);
				return;
			}
			this.m_Data->AppendEntityQueryComponentTypeSetCommand(&this.m_Data->m_MainThreadChain, this.MainThreadSortKey, ECBCommand.RemoveMultipleComponentsForEntityQuery, entityQuery, in componentTypeSet);
		}

		// Token: 0x0600049A RID: 1178 RVA: 0x0001A209 File Offset: 0x00018409
		[SupportedInEntitiesForEach]
		[Obsolete("This method now takes an extra parameter to control when the query is evaluated. To preserve the current semantics, use EntityQueryCaptureMode.AtRecord (RemovedAfter Entities 2.0)")]
		public void RemoveComponent(EntityQuery entityQuery, in ComponentTypeSet componentTypeSet)
		{
			this.RemoveComponent(entityQuery, in componentTypeSet, EntityQueryCaptureMode.AtRecord);
		}

		// Token: 0x0600049B RID: 1179 RVA: 0x0001A209 File Offset: 0x00018409
		[SupportedInEntitiesForEach]
		[Obsolete("Use RemoveComponent (RemovedAfter Entities 1.0) (UnityUpgradable) -> RemoveComponent(*)")]
		public void RemoveComponentForEntityQuery(EntityQuery entityQuery, in ComponentTypeSet componentTypeSet)
		{
			this.RemoveComponent(entityQuery, in componentTypeSet, EntityQueryCaptureMode.AtRecord);
		}

		// Token: 0x0600049C RID: 1180 RVA: 0x0001A214 File Offset: 0x00018414
		[SupportedInEntitiesForEach]
		public unsafe void DestroyEntity(EntityQuery entityQuery, EntityQueryCaptureMode queryCaptureMode)
		{
			if (queryCaptureMode == EntityQueryCaptureMode.AtRecord)
			{
				this.m_Data->AppendMultipleEntitiesCommand(&this.m_Data->m_MainThreadChain, this.MainThreadSortKey, ECBCommand.DestroyMultipleEntities, entityQuery);
				return;
			}
			this.m_Data->AppendEntityQueryCommand(&this.m_Data->m_MainThreadChain, this.MainThreadSortKey, ECBCommand.DestroyForEntityQuery, entityQuery);
		}

		// Token: 0x0600049D RID: 1181 RVA: 0x0001A267 File Offset: 0x00018467
		[SupportedInEntitiesForEach]
		[Obsolete("This method now takes an extra parameter to control when the query is evaluated. To preserve the current semantics, use EntityQueryCaptureMode.AtRecord (RemovedAfter Entities 2.0)")]
		public void DestroyEntity(EntityQuery entityQuery)
		{
			this.DestroyEntity(entityQuery, EntityQueryCaptureMode.AtRecord);
		}

		// Token: 0x0600049E RID: 1182 RVA: 0x0001A267 File Offset: 0x00018467
		[SupportedInEntitiesForEach]
		[Obsolete("Use DestroyEntity (RemovedAfter Entities 1.0) (UnityUpgradable) -> DestroyEntity(*)")]
		public void DestroyEntitiesForEntityQuery(EntityQuery entityQuery)
		{
			this.DestroyEntity(entityQuery, EntityQueryCaptureMode.AtRecord);
		}

		// Token: 0x0600049F RID: 1183 RVA: 0x0001A274 File Offset: 0x00018474
		private static bool IsDefaultObject<T>(ref T component, out int hashCode) where T : struct, ISharedComponentData
		{
			T t = default(T);
			hashCode = TypeManager.GetHashCode<T>(ref component);
			return TypeManager.Equals<T>(ref t, ref component);
		}

		// Token: 0x060004A0 RID: 1184 RVA: 0x0001A29C File Offset: 0x0001849C
		private static bool IsDefaultObjectUnmanaged<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(ref T component, out int hashCode) where T : struct, ValueType, ISharedComponentData
		{
			T t = default(T);
			hashCode = TypeManager.SharedComponentGetHashCode(UnsafeUtility.AddressOf<T>(ref component), TypeManager.GetTypeIndex<T>());
			return TypeManager.SharedComponentEquals(UnsafeUtility.AddressOf<T>(ref t), UnsafeUtility.AddressOf<T>(ref component), TypeManager.GetTypeIndex<T>());
		}

		// Token: 0x060004A1 RID: 1185 RVA: 0x0001A2DC File Offset: 0x000184DC
		[SupportedInEntitiesForEach]
		public unsafe void AddSharedComponentManaged<T>(Entity e, T sharedComponent) where T : struct, ISharedComponentData
		{
			bool flag = TypeManager.IsManagedSharedComponent(TypeManager.GetTypeIndex<T>());
			int num;
			bool flag2 = EntityCommandBuffer.IsDefaultObject<T>(ref sharedComponent, out num);
			if (flag)
			{
				this.m_Data->AddEntitySharedComponentCommand<T>(&this.m_Data->m_MainThreadChain, this.MainThreadSortKey, ECBCommand.AddSharedComponentData, e, num, flag2 ? null : sharedComponent);
				return;
			}
			void* ptr = UnsafeUtility.AddressOf<T>(ref sharedComponent);
			this.m_Data->AddEntityUnmanagedSharedComponentCommand<T>(&this.m_Data->m_MainThreadChain, this.MainThreadSortKey, ECBCommand.AddUnmanagedSharedComponentData, e, num, flag2 ? null : ptr);
		}

		// Token: 0x060004A2 RID: 1186 RVA: 0x0001A360 File Offset: 0x00018560
		[SupportedInEntitiesForEach]
		public unsafe void AddSharedComponent<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(Entity e, T sharedComponent) where T : struct, ValueType, ISharedComponentData
		{
			void* ptr = UnsafeUtility.AddressOf<T>(ref sharedComponent);
			int num;
			bool flag = EntityCommandBuffer.IsDefaultObjectUnmanaged<T>(ref sharedComponent, out num);
			this.m_Data->AddEntityUnmanagedSharedComponentCommand<T>(&this.m_Data->m_MainThreadChain, this.MainThreadSortKey, ECBCommand.AddUnmanagedSharedComponentData, e, num, flag ? null : ptr);
		}

		// Token: 0x060004A3 RID: 1187 RVA: 0x0001A3A8 File Offset: 0x000185A8
		[SupportedInEntitiesForEach]
		public unsafe void AddSharedComponentManaged<T>(NativeArray<Entity> entities, T sharedComponent) where T : struct, ISharedComponentData
		{
			bool flag;
			Entity* ptr = this.m_Data->CloneAndSearchForDeferredEntities(entities, out flag);
			bool flag2 = TypeManager.IsManagedSharedComponent(TypeManager.GetTypeIndex<T>());
			int num;
			bool flag3 = EntityCommandBuffer.IsDefaultObject<T>(ref sharedComponent, out num);
			if (flag2)
			{
				this.m_Data->AppendMultipleEntitiesComponentCommandWithSharedValue<T>(&this.m_Data->m_MainThreadChain, this.MainThreadSortKey, ECBCommand.AddSharedComponentWithValueForMultipleEntities, ptr, entities.Length, flag, num, flag3 ? null : sharedComponent);
				return;
			}
			this.m_Data->AppendMultipleEntitiesCommand_WithUnmanagedSharedValue<T>(&this.m_Data->m_MainThreadChain, this.MainThreadSortKey, ECBCommand.AddUnmanagedSharedComponentValueForMultipleEntities, ptr, entities.Length, flag, num, flag3 ? null : UnsafeUtility.AddressOf<T>(ref sharedComponent));
		}

		// Token: 0x060004A4 RID: 1188 RVA: 0x0001A44C File Offset: 0x0001864C
		[SupportedInEntitiesForEach]
		public unsafe void AddSharedComponent<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(NativeArray<Entity> entities, T sharedComponent) where T : struct, ValueType, ISharedComponentData
		{
			bool flag;
			Entity* ptr = this.m_Data->CloneAndSearchForDeferredEntities(entities, out flag);
			int num;
			bool flag2 = EntityCommandBuffer.IsDefaultObjectUnmanaged<T>(ref sharedComponent, out num);
			this.m_Data->AppendMultipleEntitiesCommand_WithUnmanagedSharedValue<T>(&this.m_Data->m_MainThreadChain, this.MainThreadSortKey, ECBCommand.AddUnmanagedSharedComponentValueForMultipleEntities, ptr, entities.Length, flag, num, flag2 ? null : UnsafeUtility.AddressOf<T>(ref sharedComponent));
		}

		// Token: 0x060004A5 RID: 1189 RVA: 0x0001A4AC File Offset: 0x000186AC
		[SupportedInEntitiesForEach]
		public unsafe void SetSharedComponentManaged<T>(Entity e, T sharedComponent) where T : struct, ISharedComponentData
		{
			bool flag = TypeManager.IsManagedSharedComponent(TypeManager.GetTypeIndex<T>());
			int num;
			bool flag2 = EntityCommandBuffer.IsDefaultObject<T>(ref sharedComponent, out num);
			if (flag)
			{
				this.m_Data->AddEntitySharedComponentCommand<T>(&this.m_Data->m_MainThreadChain, this.MainThreadSortKey, ECBCommand.SetSharedComponentData, e, num, flag2 ? null : sharedComponent);
				return;
			}
			void* ptr = UnsafeUtility.AddressOf<T>(ref sharedComponent);
			this.m_Data->AddEntityUnmanagedSharedComponentCommand<T>(&this.m_Data->m_MainThreadChain, this.MainThreadSortKey, ECBCommand.SetUnmanagedSharedComponentData, e, num, flag2 ? null : ptr);
		}

		// Token: 0x060004A6 RID: 1190 RVA: 0x0001A530 File Offset: 0x00018730
		internal unsafe void UnsafeSetSharedComponentManagedNonDefault(Entity e, object sharedComponent, TypeIndex typeIndex)
		{
			int num = ((sharedComponent != null) ? TypeManager.GetHashCode(sharedComponent, typeIndex) : 0);
			if (typeIndex.IsManagedType)
			{
				this.m_Data->AddEntitySharedComponentCommand(&this.m_Data->m_MainThreadChain, this.MainThreadSortKey, ECBCommand.SetSharedComponentData, e, num, typeIndex, sharedComponent);
				return;
			}
			byte* ptr = null;
			GCHandle gchandle = default(GCHandle);
			if (sharedComponent != null)
			{
				gchandle = GCHandle.Alloc(sharedComponent, GCHandleType.Pinned);
				ptr = (byte*)(void*)gchandle.AddrOfPinnedObject();
			}
			this.m_Data->AddEntityUnmanagedSharedComponentCommand(&this.m_Data->m_MainThreadChain, this.MainThreadSortKey, ECBCommand.SetUnmanagedSharedComponentData, e, num, typeIndex, TypeManager.GetTypeInfo(typeIndex).TypeSize, (void*)ptr);
			if (ptr != null)
			{
				gchandle.Free();
			}
		}

		// Token: 0x060004A7 RID: 1191 RVA: 0x0001A5D8 File Offset: 0x000187D8
		[SupportedInEntitiesForEach]
		public unsafe void SetSharedComponent<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(Entity e, T sharedComponent) where T : struct, ValueType, ISharedComponentData
		{
			int num;
			bool flag = EntityCommandBuffer.IsDefaultObjectUnmanaged<T>(ref sharedComponent, out num);
			void* ptr = UnsafeUtility.AddressOf<T>(ref sharedComponent);
			this.m_Data->AddEntityUnmanagedSharedComponentCommand<T>(&this.m_Data->m_MainThreadChain, this.MainThreadSortKey, ECBCommand.SetUnmanagedSharedComponentData, e, num, flag ? null : ptr);
		}

		// Token: 0x060004A8 RID: 1192 RVA: 0x0001A620 File Offset: 0x00018820
		[SupportedInEntitiesForEach]
		public unsafe void SetSharedComponentManaged<T>(NativeArray<Entity> entities, T sharedComponent) where T : struct, ISharedComponentData
		{
			bool flag = TypeManager.IsManagedSharedComponent(TypeManager.GetTypeIndex<T>());
			int num;
			bool flag2 = EntityCommandBuffer.IsDefaultObject<T>(ref sharedComponent, out num);
			bool flag3;
			Entity* ptr = this.m_Data->CloneAndSearchForDeferredEntities(entities, out flag3);
			if (flag)
			{
				this.m_Data->AppendMultipleEntitiesComponentCommandWithSharedValue<T>(&this.m_Data->m_MainThreadChain, this.MainThreadSortKey, ECBCommand.SetSharedComponentValueForMultipleEntities, ptr, entities.Length, flag3, num, flag2 ? null : sharedComponent);
				return;
			}
			this.m_Data->AppendMultipleEntitiesCommand_WithUnmanagedSharedValue<T>(&this.m_Data->m_MainThreadChain, this.MainThreadSortKey, ECBCommand.SetUnmanagedSharedComponentValueForMultipleEntities, ptr, entities.Length, flag3, num, flag2 ? null : UnsafeUtility.AddressOf<T>(ref sharedComponent));
		}

		// Token: 0x060004A9 RID: 1193 RVA: 0x0001A6C4 File Offset: 0x000188C4
		[SupportedInEntitiesForEach]
		public unsafe void SetSharedComponent<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(NativeArray<Entity> entities, T sharedComponent) where T : struct, ValueType, ISharedComponentData
		{
			int num;
			bool flag = EntityCommandBuffer.IsDefaultObjectUnmanaged<T>(ref sharedComponent, out num);
			bool flag2;
			Entity* ptr = this.m_Data->CloneAndSearchForDeferredEntities(entities, out flag2);
			this.m_Data->AppendMultipleEntitiesCommand_WithUnmanagedSharedValue<T>(&this.m_Data->m_MainThreadChain, this.MainThreadSortKey, ECBCommand.SetUnmanagedSharedComponentValueForMultipleEntities, ptr, entities.Length, flag2, num, flag ? null : UnsafeUtility.AddressOf<T>(ref sharedComponent));
		}

		// Token: 0x060004AA RID: 1194 RVA: 0x0001A722 File Offset: 0x00018922
		public unsafe void AddComponentForLinkedEntityGroup<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(Entity e, EntityQueryMask mask, T component) where T : struct, ValueType, IComponentData
		{
			this.m_Data->AddLinkedEntityGroupComponentCommand<T>(&this.m_Data->m_MainThreadChain, this.MainThreadSortKey, ECBCommand.AddComponentLinkedEntityGroup, mask, e, component);
		}

		// Token: 0x060004AB RID: 1195 RVA: 0x0001A746 File Offset: 0x00018946
		public unsafe void AddComponentForLinkedEntityGroup(Entity e, EntityQueryMask mask, ComponentType componentType)
		{
			this.m_Data->AddLinkedEntityGroupTypeCommand(&this.m_Data->m_MainThreadChain, this.MainThreadSortKey, ECBCommand.AddComponentLinkedEntityGroup, mask, e, componentType);
		}

		// Token: 0x060004AC RID: 1196 RVA: 0x0001A76A File Offset: 0x0001896A
		public unsafe void SetComponentForLinkedEntityGroup<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(Entity e, EntityQueryMask mask, T component) where T : struct, ValueType, IComponentData
		{
			this.m_Data->AddLinkedEntityGroupComponentCommand<T>(&this.m_Data->m_MainThreadChain, this.MainThreadSortKey, ECBCommand.SetComponentLinkedEntityGroup, mask, e, component);
		}

		// Token: 0x060004AD RID: 1197 RVA: 0x0001A78E File Offset: 0x0001898E
		public unsafe void ReplaceComponentForLinkedEntityGroup<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(Entity e, T component) where T : struct, ValueType, IComponentData
		{
			this.m_Data->AddEntityComponentTypeWithValueCommand<T>(&this.m_Data->m_MainThreadChain, this.MainThreadSortKey, ECBCommand.ReplaceComponentLinkedEntityGroup, e, component);
		}

		// Token: 0x060004AE RID: 1198 RVA: 0x0001A7B1 File Offset: 0x000189B1
		public void Playback(EntityManager mgr)
		{
			this.PlaybackInternal(mgr.GetCheckedEntityDataAccess());
		}

		// Token: 0x060004AF RID: 1199 RVA: 0x0001A7C0 File Offset: 0x000189C0
		public void Playback(ExclusiveEntityTransaction mgr)
		{
			this.PlaybackInternal(mgr.EntityManager.GetCheckedEntityDataAccess());
		}

		// Token: 0x060004B0 RID: 1200 RVA: 0x0001A7E4 File Offset: 0x000189E4
		private unsafe void PlaybackInternal(EntityDataAccess* mgr)
		{
			if (!this.ShouldPlayback || this.m_Data == null)
			{
				return;
			}
			if (this.m_Data != null && this.m_Data->m_DidPlayback && this.m_Data->m_PlaybackPolicy == PlaybackPolicy.SinglePlayback)
			{
				throw new InvalidOperationException("Attempt to call Playback() on an EntityCommandBuffer that has already been played back.\nEntityCommandBuffers created with the SinglePlayback policy can only be played back once.");
			}
			if (EntityCommandBuffer.ENABLE_PRE_PLAYBACK_VALIDATION)
			{
				EntityCommandBuffer.EcbWalker<EntityCommandBuffer.PrePlaybackValidationProcessor> ecbWalker = new EntityCommandBuffer.EcbWalker<EntityCommandBuffer.PrePlaybackValidationProcessor>(default(EntityCommandBuffer.PrePlaybackValidationProcessor), EntityCommandBuffer.ECBProcessorType.PrePlaybackValidationProcessor);
				ecbWalker.processor.Init(mgr, this.m_Data, in this.OriginSystemHandle);
				ecbWalker.WalkChains(this);
				ecbWalker.processor.Cleanup();
				this.PassedPrePlaybackValidation = 1;
			}
			else if (EntityCommandBuffer.PLAYBACK_WITH_TRACE)
			{
				EntityCommandBuffer.EcbWalker<EntityCommandBuffer.PlaybackWithTraceProcessor> ecbWalker2 = new EntityCommandBuffer.EcbWalker<EntityCommandBuffer.PlaybackWithTraceProcessor>(default(EntityCommandBuffer.PlaybackWithTraceProcessor), EntityCommandBuffer.ECBProcessorType.PlaybackWithTraceProcessor);
				ecbWalker2.processor.Init(mgr, this.m_Data, in this.OriginSystemHandle);
				ecbWalker2.WalkChains(this);
				ecbWalker2.processor.Cleanup();
			}
			else
			{
				EntityCommandBuffer.EcbWalker<EntityCommandBuffer.PlaybackProcessor> ecbWalker3 = new EntityCommandBuffer.EcbWalker<EntityCommandBuffer.PlaybackProcessor>(default(EntityCommandBuffer.PlaybackProcessor), EntityCommandBuffer.ECBProcessorType.PlaybackProcessor);
				ecbWalker3.processor.Init(mgr, this.m_Data, in this.OriginSystemHandle);
				ecbWalker3.WalkChains(this);
				ecbWalker3.processor.Cleanup();
			}
			this.m_Data->m_DidPlayback = true;
		}

		// Token: 0x060004B1 RID: 1201 RVA: 0x0001A928 File Offset: 0x00018B28
		internal unsafe static void ProcessManagedCommand<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(T* processor, BasicCommand* header) where T : struct, ValueType, EntityCommandBuffer.IEcbProcessor
		{
			ECBCommand commandType = header->CommandType;
			switch (commandType)
			{
			case ECBCommand.AddManagedComponentData:
				processor->AddManagedComponentData(header);
				return;
			case ECBCommand.SetManagedComponentData:
				processor->SetManagedComponentData(header);
				return;
			case ECBCommand.MoveManagedComponentData:
				processor->MoveManagedComponentData(header);
				return;
			case ECBCommand.AddComponentLinkedEntityGroup:
			case ECBCommand.SetComponentLinkedEntityGroup:
			case ECBCommand.ReplaceComponentLinkedEntityGroup:
				break;
			case ECBCommand.AddSharedComponentData:
				processor->AddSharedComponentData(header);
				return;
			case ECBCommand.SetSharedComponentData:
				processor->SetSharedComponentData(header);
				break;
			default:
				switch (commandType)
				{
				case ECBCommand.AddSharedComponentWithValueForMultipleEntities:
					processor->AddSharedComponentWithValueForMultipleEntities(header);
					return;
				case ECBCommand.AddSharedComponentWithValueForEntityQuery:
					processor->AddSharedComponentWithValueForEntityQuery(header);
					return;
				case ECBCommand.SetSharedComponentValueForMultipleEntities:
					processor->SetSharedComponentValueForMultipleEntities(header);
					return;
				case ECBCommand.SetSharedComponentValueForEntityQuery:
					processor->SetSharedComponentValueForEntityQuery(header);
					return;
				case ECBCommand.AddComponentForMultipleEntities:
				case ECBCommand.AddComponentObjectForEntityQuery:
					break;
				case ECBCommand.AddComponentObjectForMultipleEntities:
					processor->AddComponentObjectForMultipleEntities(header);
					return;
				case ECBCommand.SetComponentObjectForMultipleEntities:
					processor->SetComponentObjectForMultipleEntities(header);
					return;
				default:
					return;
				}
				break;
			}
		}

		// Token: 0x060004B2 RID: 1202 RVA: 0x0001AA28 File Offset: 0x00018C28
		[Conditional("ENABLE_UNITY_COLLECTIONS_CHECKS")]
		[Conditional("UNITY_DOTS_DEBUG")]
		private static void CheckEntityNotNull(Entity entity)
		{
			if (entity == Entity.Null)
			{
				throw new InvalidOperationException("Invalid Entity.Null passed.");
			}
		}

		// Token: 0x060004B3 RID: 1203 RVA: 0x0001AA44 File Offset: 0x00018C44
		[Conditional("ENABLE_UNITY_COLLECTIONS_CHECKS")]
		[Conditional("UNITY_DOTS_DEBUG")]
		private static void CheckCommandEntity(in ECBSharedPlaybackState playbackState, Entity deferredEntity)
		{
			if (playbackState.CreateEntityBatch == null)
			{
				throw new InvalidOperationException("playbackState.CreateEntityBatch passed to SelectEntity is null (likely due to an ECB command recording an invalid temporary Entity).");
			}
			if (deferredEntity.Version != playbackState.CommandBufferID)
			{
				throw new InvalidOperationException(string.Format("Deferred Entity {0} was created by a different command buffer. Deferred Entities can only be used by the command buffer that created them.", deferredEntity));
			}
			int num = -deferredEntity.Index - 1;
			if (num < 0 || num >= playbackState.CreatedEntityCount)
			{
				throw new InvalidOperationException(string.Format("Deferred Entity {0} is out of range. Was it created by a different EntityCommandBuffer?", deferredEntity));
			}
		}

		// Token: 0x060004B4 RID: 1204 RVA: 0x0001AAB7 File Offset: 0x00018CB7
		[Conditional("ENABLE_UNITY_COLLECTIONS_CHECKS")]
		[Conditional("UNITY_DOTS_DEBUG")]
		private static void CheckEntityVersionValid(Entity entity)
		{
			if (entity.Version <= 0)
			{
				throw new InvalidOperationException("Invalid Entity version");
			}
		}

		// Token: 0x060004B5 RID: 1205 RVA: 0x0001AAD0 File Offset: 0x00018CD0
		private unsafe static Entity SelectEntity(Entity cmdEntity, in ECBSharedPlaybackState playbackState)
		{
			if (cmdEntity.Index < 0)
			{
				int num = -cmdEntity.Index - 1;
				return playbackState.CreateEntityBatch[num];
			}
			return cmdEntity;
		}

		// Token: 0x060004B6 RID: 1206 RVA: 0x0001AB07 File Offset: 0x00018D07
		private unsafe static void CommitStructuralChanges(EntityDataAccess* mgr, ref EntityComponentStore.ArchetypeChanges archetypeChanges)
		{
			mgr->EndStructuralChanges(ref archetypeChanges);
			archetypeChanges = mgr->BeginStructuralChanges();
		}

		// Token: 0x060004B7 RID: 1207 RVA: 0x0001AB1C File Offset: 0x00018D1C
		private unsafe static void FixupTemporaryEntitiesInComponentValue(byte* data, TypeIndex typeIndex, in ECBSharedPlaybackState playbackState)
		{
			EntityCommandBuffer.FixupTemporaryEntitiesInComponentValue(data, 1, typeIndex, in playbackState);
		}

		// Token: 0x060004B8 RID: 1208 RVA: 0x0001AB28 File Offset: 0x00018D28
		private unsafe static void FixupTemporaryEntitiesInComponentValue(byte* data, int count, TypeIndex typeIndex, in ECBSharedPlaybackState playbackState)
		{
			readonly ref TypeManager.TypeInfo typeInfo = ref TypeManager.GetTypeInfo(typeIndex);
			TypeManager.EntityOffsetInfo* entityOffsets = TypeManager.GetEntityOffsets(in typeInfo);
			int entityOffsetCount = typeInfo.EntityOffsetCount;
			int i = 0;
			while (i < count)
			{
				for (int j = 0; j < entityOffsetCount; j++)
				{
					Entity* ptr = (Entity*)(data + entityOffsets[j].Offset);
					if (ptr->Index < 0)
					{
						int num = -ptr->Index - 1;
						Entity entity = playbackState.CreateEntityBatch[num];
						*ptr = entity;
					}
				}
				i++;
				data += typeInfo.ElementSize;
			}
		}

		// Token: 0x060004B9 RID: 1209 RVA: 0x0001ABBC File Offset: 0x00018DBC
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private unsafe static void AddToPostPlaybackFixup(EntityBufferCommand* cmd, ref ECBSharedPlaybackState playbackState)
		{
			EntityCommandBuffer.SelectEntity(cmd->Header.Entity, in playbackState);
			ref ECBSharedPlaybackState.BufferWithFixUp buffersWithFixUp = ref *playbackState.BuffersWithFixUp;
			int lastBuffer = playbackState.LastBuffer;
			playbackState.LastBuffer = lastBuffer + 1;
			((ref buffersWithFixUp) + (IntPtr)lastBuffer * (IntPtr)sizeof(ECBSharedPlaybackState.BufferWithFixUp)).cmd = cmd;
		}

		// Token: 0x060004BA RID: 1210 RVA: 0x0001AC00 File Offset: 0x00018E00
		private unsafe static void FixupBufferContents(EntityDataAccess* mgr, EntityBufferCommand* cmd, Entity entity, ECBSharedPlaybackState playbackState)
		{
			BufferHeader* componentDataWithTypeRW = (BufferHeader*)mgr->EntityComponentStore->GetComponentDataWithTypeRW(entity, cmd->ComponentTypeIndex, mgr->EntityComponentStore->GlobalSystemVersion);
			EntityCommandBuffer.FixupTemporaryEntitiesInComponentValue(BufferHeader.GetElementPointer(componentDataWithTypeRW), componentDataWithTypeRW->Length, cmd->ComponentTypeIndex, in playbackState);
		}

		// Token: 0x060004BB RID: 1211 RVA: 0x0001AC44 File Offset: 0x00018E44
		[Conditional("ENABLE_UNITY_COLLECTIONS_CHECKS")]
		[Conditional("UNITY_DOTS_DEBUG")]
		private unsafe static void CheckBufferExistsOnEntity(EntityComponentStore* mgr, Entity entity, EntityComponentCommand* cmd)
		{
			bool flag;
			if (!mgr->HasComponent(entity, cmd->ComponentTypeIndex, out flag))
			{
				throw new InvalidOperationException(string.Format("Buffer does not exist on entity {0} (entityExists:{1}), cannot append element.", entity, flag));
			}
		}

		// Token: 0x060004BC RID: 1212 RVA: 0x00002287 File Offset: 0x00000487
		[Conditional("ENABLE_UNITY_COLLECTIONS_CHECKS")]
		[Conditional("UNITY_DOTS_DEBUG")]
		private static void AssertNoFixupInMultiPlayback(bool isFirstPlayback)
		{
		}

		// Token: 0x060004BD RID: 1213 RVA: 0x0001AC80 File Offset: 0x00018E80
		private unsafe static BufferHeader CloneBuffer(BufferHeader* srcBuffer, TypeIndex componentTypeIndex)
		{
			BufferHeader bufferHeader = default(BufferHeader);
			BufferHeader.Initialize(&bufferHeader, 0);
			int num = 8;
			ref int ptr = ref TypeManager.GetTypeInfo(componentTypeIndex).ElementSize;
			BufferHeader.Assign(&bufferHeader, BufferHeader.GetElementPointer(srcBuffer), srcBuffer->Length, ptr, num, false, 0);
			return bufferHeader;
		}

		// Token: 0x060004BE RID: 1214 RVA: 0x0001ACC8 File Offset: 0x00018EC8
		public unsafe EntityCommandBuffer.ParallelWriter AsParallelWriter()
		{
			EntityCommandBuffer.ParallelWriter parallelWriter;
			parallelWriter.m_Data = this.m_Data;
			parallelWriter.m_ThreadIndex = -1;
			if (parallelWriter.m_Data != null)
			{
				parallelWriter.m_Data->InitForParallelWriter();
			}
			return parallelWriter;
		}

		// Token: 0x17000093 RID: 147
		// (get) Token: 0x060004BF RID: 1215 RVA: 0x0001AD00 File Offset: 0x00018F00
		// (set) Token: 0x060004C0 RID: 1216 RVA: 0x0001AD10 File Offset: 0x00018F10
		public unsafe static bool PLAYBACK_WITH_TRACE
		{
			get
			{
				return *EntityCommandBuffer._PLAYBACK_WITH_TRACE.Data != 0;
			}
			set
			{
				*EntityCommandBuffer._PLAYBACK_WITH_TRACE.Data = (value ? 1 : 0);
			}
		}

		// Token: 0x17000094 RID: 148
		// (get) Token: 0x060004C1 RID: 1217 RVA: 0x0001AD24 File Offset: 0x00018F24
		// (set) Token: 0x060004C2 RID: 1218 RVA: 0x0001AD34 File Offset: 0x00018F34
		internal unsafe static bool ENABLE_PRE_PLAYBACK_VALIDATION
		{
			get
			{
				return *EntityCommandBuffer._ENABLE_PRE_PLAYBACK_VALIDATION.Data != 0;
			}
			set
			{
				*EntityCommandBuffer._ENABLE_PRE_PLAYBACK_VALIDATION.Data = (value ? 1 : 0);
			}
		}

		// Token: 0x060004C4 RID: 1220 RVA: 0x0001AD88 File Offset: 0x00018F88
		[BurstCompile]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal unsafe static void DisposeInternal$BurstManaged(ref EntityCommandBuffer ecb)
		{
			if (!ecb.IsCreated)
			{
				return;
			}
			bool flag = !ecb.m_Data->m_Allocator.IsAutoDispose;
			if (ecb.m_Data->m_ForceFullDispose && (!ecb.m_Data->m_DidPlayback || ecb.m_Data->m_PlaybackPolicy == PlaybackPolicy.MultiPlayback))
			{
				flag = true;
			}
			if (ecb.m_Data != null && flag)
			{
				ecb.FreeChain(&ecb.m_Data->m_MainThreadChain, ecb.m_Data->m_PlaybackPolicy, ecb.m_Data->m_DidPlayback);
				if (ecb.m_Data->m_ThreadedChains != null)
				{
					int threadIndexCount = JobsUtility.ThreadIndexCount;
					for (int i = 0; i < threadIndexCount; i++)
					{
						ecb.FreeChain(ecb.m_Data->m_ThreadedChains + i, ecb.m_Data->m_PlaybackPolicy, ecb.m_Data->m_DidPlayback);
					}
					ecb.m_Data->DestroyForParallelWriter();
				}
				Memory.Unmanaged.Free<EntityCommandBufferData>(ecb.m_Data, ecb.m_Data->m_Allocator);
			}
			ecb.m_Data = null;
		}

		// Token: 0x04000317 RID: 791
		private const int kDefaultMinimumChunkSize = 4096;

		// Token: 0x04000318 RID: 792
		[NativeDisableUnsafePtrRestriction]
		internal unsafe EntityCommandBufferData* m_Data;

		// Token: 0x04000319 RID: 793
		internal int SystemID;

		// Token: 0x0400031A RID: 794
		internal SystemHandle OriginSystemHandle;

		// Token: 0x0400031B RID: 795
		internal int PassedPrePlaybackValidation;

		// Token: 0x0400031C RID: 796
		private static readonly SharedStatic<int> _ms_CommandBufferIDAllocator = EntityCommandBuffer.ECBIDAllocator.Ref;

		// Token: 0x0400031D RID: 797
		private static readonly ProfilerMarker k_ProfileEcbPlayback = new ProfilerMarker("EntityCommandBuffer.Playback");

		// Token: 0x0400031E RID: 798
		private static readonly ProfilerMarker k_ProfileEcbDispose = new ProfilerMarker("EntityCommandBuffer.Dispose");

		// Token: 0x0400031F RID: 799
		private const bool kBatchableCommand = true;

		// Token: 0x04000320 RID: 800
		internal const int INITIAL_COMMANDS_CAPACITY = 8;

		// Token: 0x04000321 RID: 801
		private static readonly SharedStatic<int> _PLAYBACK_WITH_TRACE = EntityCommandBuffer.ECBPlaybackWithTrace.Ref;

		// Token: 0x04000322 RID: 802
		private static readonly SharedStatic<int> _ENABLE_PRE_PLAYBACK_VALIDATION = EntityCommandBuffer.ECBPrePlaybackValidation.Ref;

		// Token: 0x02000112 RID: 274
		private struct ECBIDAllocator
		{
			// Token: 0x04000323 RID: 803
			public static readonly SharedStatic<int> Ref = SharedStatic<int>.GetOrCreateUnsafe(0U, 4131589732049713713L, -3028002200918249970L);
		}

		// Token: 0x02000113 RID: 275
		internal enum ECBProcessorType
		{
			// Token: 0x04000325 RID: 805
			PlaybackProcessor,
			// Token: 0x04000326 RID: 806
			DebugViewProcessor,
			// Token: 0x04000327 RID: 807
			PlaybackWithTraceProcessor,
			// Token: 0x04000328 RID: 808
			PrePlaybackValidationProcessor
		}

		// Token: 0x02000114 RID: 276
		internal interface IEcbProcessor
		{
			// Token: 0x060004C6 RID: 1222
			unsafe void DestroyEntity(BasicCommand* header);

			// Token: 0x060004C7 RID: 1223
			unsafe void RemoveComponent(BasicCommand* header);

			// Token: 0x060004C8 RID: 1224
			unsafe void RemoveMultipleComponents(BasicCommand* header);

			// Token: 0x060004C9 RID: 1225
			unsafe void CreateEntity(BasicCommand* header);

			// Token: 0x060004CA RID: 1226
			unsafe void InstantiateEntity(BasicCommand* header);

			// Token: 0x060004CB RID: 1227
			unsafe void AddComponent(BasicCommand* header);

			// Token: 0x060004CC RID: 1228
			unsafe void AddMultipleComponents(BasicCommand* header);

			// Token: 0x060004CD RID: 1229
			unsafe void SetComponent(BasicCommand* header);

			// Token: 0x060004CE RID: 1230
			unsafe void SetEnabled(BasicCommand* header);

			// Token: 0x060004CF RID: 1231
			unsafe void SetComponentEnabled(BasicCommand* header);

			// Token: 0x060004D0 RID: 1232
			unsafe void SetName(BasicCommand* header);

			// Token: 0x060004D1 RID: 1233
			unsafe void AddBuffer(BasicCommand* header);

			// Token: 0x060004D2 RID: 1234
			unsafe void SetBuffer(BasicCommand* header);

			// Token: 0x060004D3 RID: 1235
			unsafe void AppendToBuffer(BasicCommand* header);

			// Token: 0x060004D4 RID: 1236
			unsafe void AddComponentForEntityQuery(BasicCommand* header);

			// Token: 0x060004D5 RID: 1237
			unsafe void AddComponentForMultipleEntities(BasicCommand* header);

			// Token: 0x060004D6 RID: 1238
			unsafe void RemoveComponentForEntityQuery(BasicCommand* header);

			// Token: 0x060004D7 RID: 1239
			unsafe void RemoveComponentForMultipleEntities(BasicCommand* header);

			// Token: 0x060004D8 RID: 1240
			unsafe void AddMultipleComponentsForMultipleEntities(BasicCommand* header);

			// Token: 0x060004D9 RID: 1241
			unsafe void AddMultipleComponentsForEntityQuery(BasicCommand* header);

			// Token: 0x060004DA RID: 1242
			unsafe void RemoveMultipleComponentsForMultipleEntities(BasicCommand* header);

			// Token: 0x060004DB RID: 1243
			unsafe void RemoveMultipleComponentsForEntityQuery(BasicCommand* header);

			// Token: 0x060004DC RID: 1244
			unsafe void DestroyMultipleEntities(BasicCommand* header);

			// Token: 0x060004DD RID: 1245
			unsafe void DestroyForEntityQuery(BasicCommand* header);

			// Token: 0x060004DE RID: 1246
			unsafe void AddComponentLinkedEntityGroup(BasicCommand* header);

			// Token: 0x060004DF RID: 1247
			unsafe void SetComponentLinkedEntityGroup(BasicCommand* header);

			// Token: 0x060004E0 RID: 1248
			unsafe void ReplaceComponentLinkedEntityGroup(BasicCommand* header);

			// Token: 0x060004E1 RID: 1249
			unsafe void AddManagedComponentData(BasicCommand* header);

			// Token: 0x060004E2 RID: 1250
			unsafe void MoveManagedComponentData(BasicCommand* header);

			// Token: 0x060004E3 RID: 1251
			unsafe void AddComponentObjectForMultipleEntities(BasicCommand* header);

			// Token: 0x060004E4 RID: 1252
			unsafe void SetComponentObjectForMultipleEntities(BasicCommand* header);

			// Token: 0x060004E5 RID: 1253
			unsafe void AddSharedComponentData(BasicCommand* header);

			// Token: 0x060004E6 RID: 1254
			unsafe void AddSharedComponentWithValueForMultipleEntities(BasicCommand* header);

			// Token: 0x060004E7 RID: 1255
			unsafe void AddSharedComponentWithValueForEntityQuery(BasicCommand* header);

			// Token: 0x060004E8 RID: 1256
			unsafe void SetSharedComponentValueForMultipleEntities(BasicCommand* header);

			// Token: 0x060004E9 RID: 1257
			unsafe void SetSharedComponentValueForEntityQuery(BasicCommand* header);

			// Token: 0x060004EA RID: 1258
			unsafe void SetManagedComponentData(BasicCommand* header);

			// Token: 0x060004EB RID: 1259
			unsafe void SetSharedComponentData(BasicCommand* header);

			// Token: 0x060004EC RID: 1260
			unsafe void AddUnmanagedSharedComponentData(BasicCommand* header);

			// Token: 0x060004ED RID: 1261
			unsafe void SetUnmanagedSharedComponentData(BasicCommand* header);

			// Token: 0x060004EE RID: 1262
			unsafe void AddUnmanagedSharedComponentValueForMultipleEntities(BasicCommand* header);

			// Token: 0x060004EF RID: 1263
			unsafe void AddUnmanagedSharedComponentValueForEntityQuery(BasicCommand* header);

			// Token: 0x060004F0 RID: 1264
			unsafe void SetUnmanagedSharedComponentValueForMultipleEntities(BasicCommand* header);

			// Token: 0x060004F1 RID: 1265
			unsafe void SetUnmanagedSharedComponentValueForEntityQuery(BasicCommand* header);

			// Token: 0x17000095 RID: 149
			// (get) Token: 0x060004F2 RID: 1266
			EntityCommandBuffer.ECBProcessorType ProcessorType { get; }
		}

		// Token: 0x02000115 RID: 277
		internal struct EcbWalker<[global::System.Runtime.CompilerServices.IsUnmanaged] T> where T : struct, ValueType, EntityCommandBuffer.IEcbProcessor
		{
			// Token: 0x060004F3 RID: 1267 RVA: 0x0001AEB3 File Offset: 0x000190B3
			public EcbWalker(T ecbProcessor, EntityCommandBuffer.ECBProcessorType type)
			{
				this.processor = ecbProcessor;
				this.processorType = type;
			}

			// Token: 0x060004F4 RID: 1268 RVA: 0x0001AEC4 File Offset: 0x000190C4
			public unsafe void WalkChains(EntityCommandBuffer ecb)
			{
				EntityCommandBufferData* data = ecb.m_Data;
				if (data->m_RecordedChainCount <= 0)
				{
					return;
				}
				fixed (EntityCommandBuffer.EcbWalker<T>* ptr = (EntityCommandBuffer.EcbWalker<T>*)(&this))
				{
					void* ptr2 = (void*)ptr;
					ECBChainPlaybackState* ptr3;
					int num;
					checked
					{
						ptr3 = stackalloc ECBChainPlaybackState[unchecked((UIntPtr)data->m_RecordedChainCount) * (UIntPtr)sizeof(ECBChainPlaybackState)];
						num = 0;
					}
					for (EntityCommandBufferChain* ptr4 = &data->m_MainThreadChain; ptr4 != null; ptr4 = ptr4->m_NextChain)
					{
						if (ptr4->m_Head != null)
						{
							ptr3[(IntPtr)(num++) * (IntPtr)sizeof(ECBChainPlaybackState)] = new ECBChainPlaybackState
							{
								Chunk = ptr4->m_Head,
								Offset = 0,
								NextSortKey = ptr4->m_Head->BaseSortKey,
								CanBurstPlayback = ptr4->m_CanBurstPlayback
							};
						}
					}
					if (data->m_ThreadedChains != null)
					{
						int threadIndexCount = JobsUtility.ThreadIndexCount;
						for (int i = 0; i < threadIndexCount; i++)
						{
							for (EntityCommandBufferChain* ptr5 = data->m_ThreadedChains + i; ptr5 != null; ptr5 = ptr5->m_NextChain)
							{
								if (ptr5->m_Head != null)
								{
									ptr3[(IntPtr)(num++) * (IntPtr)sizeof(ECBChainPlaybackState)] = new ECBChainPlaybackState
									{
										Chunk = ptr5->m_Head,
										Offset = 0,
										NextSortKey = ptr5->m_Head->BaseSortKey,
										CanBurstPlayback = ptr5->m_CanBurstPlayback
									};
								}
							}
						}
					}
					using (ECBChainPriorityQueue ecbchainPriorityQueue = new ECBChainPriorityQueue(ptr3, data->m_RecordedChainCount, Allocator.Temp))
					{
						ECBChainHeapElement ecbchainHeapElement = ecbchainPriorityQueue.Pop();
						while (ecbchainHeapElement.ChainIndex != -1)
						{
							ECBChainHeapElement ecbchainHeapElement2 = ecbchainPriorityQueue.Peek();
							ECBChainPlaybackState ecbchainPlaybackState = ptr3[ecbchainHeapElement.ChainIndex];
							ECBChainPlaybackState ecbchainPlaybackState2 = ptr3[ecbchainHeapElement.ChainIndex];
							if (ptr3[ecbchainHeapElement.ChainIndex].CanBurstPlayback)
							{
								ECBInterop.ProcessChainChunk(ptr2, (int)this.processorType, ptr3, ecbchainHeapElement.ChainIndex, ecbchainHeapElement2.ChainIndex);
							}
							else
							{
								ECBInterop._ProcessChainChunk(ptr2, (int)this.processorType, ptr3, ecbchainHeapElement.ChainIndex, ecbchainHeapElement2.ChainIndex);
							}
							if (ptr3[ecbchainHeapElement.ChainIndex].Chunk == null)
							{
								ecbchainPriorityQueue.Pop();
							}
							else
							{
								ecbchainHeapElement.SortKey = ptr3[ecbchainHeapElement.ChainIndex].NextSortKey;
								ecbchainPriorityQueue.ReplaceTop(ecbchainHeapElement);
							}
							ecbchainHeapElement = ecbchainHeapElement2;
						}
					}
				}
			}

			// Token: 0x060004F5 RID: 1269 RVA: 0x0001B150 File Offset: 0x00019350
			internal unsafe void ProcessChain(ECBChainPlaybackState* chainStates, int currentChain, int nextChain)
			{
				int num = ((nextChain != -1) ? chainStates[nextChain].NextSortKey : (-1));
				ECBChunk* ptr = chainStates[currentChain].Chunk;
				int i = chainStates[currentChain].Offset;
				while (ptr != null)
				{
					byte* ptr2 = (byte*)(ptr + 1);
					while (i < ptr->Used)
					{
						BasicCommand* ptr3 = (BasicCommand*)(ptr2 + i);
						if (nextChain != -1 && ptr3->SortKey > num)
						{
							ECBChainPlaybackState ecbchainPlaybackState = chainStates[currentChain];
							ecbchainPlaybackState.Chunk = ptr;
							ecbchainPlaybackState.Offset = i;
							ecbchainPlaybackState.NextSortKey = ptr3->SortKey;
							chainStates[currentChain] = ecbchainPlaybackState;
							return;
						}
						if (!this.ProcessUnmanagedCommand(ptr3))
						{
							ECBInterop.ProcessManagedCommand(UnsafeUtility.AddressOf<T>(ref this.processor), (int)this.processor.ProcessorType, ptr3);
						}
						i += ptr3->TotalSize;
					}
					ptr = ptr->Next;
					i = 0;
				}
				ECBChainPlaybackState ecbchainPlaybackState2 = chainStates[currentChain];
				ecbchainPlaybackState2.Chunk = null;
				ecbchainPlaybackState2.Offset = 0;
				ecbchainPlaybackState2.NextSortKey = int.MinValue;
				chainStates[currentChain] = ecbchainPlaybackState2;
			}

			// Token: 0x060004F6 RID: 1270 RVA: 0x0001B298 File Offset: 0x00019498
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			private unsafe bool ProcessUnmanagedCommand(BasicCommand* header)
			{
				switch (header->CommandType)
				{
				case ECBCommand.InstantiateEntity:
					this.processor.InstantiateEntity(header);
					return true;
				case ECBCommand.CreateEntity:
					this.processor.CreateEntity(header);
					return true;
				case ECBCommand.DestroyEntity:
					this.processor.DestroyEntity(header);
					return true;
				case ECBCommand.AddComponent:
					this.processor.AddComponent(header);
					return true;
				case ECBCommand.AddMultipleComponents:
					this.processor.AddMultipleComponents(header);
					return true;
				case ECBCommand.RemoveComponent:
					this.processor.RemoveComponent(header);
					return true;
				case ECBCommand.RemoveMultipleComponents:
					this.processor.RemoveMultipleComponents(header);
					return true;
				case ECBCommand.SetComponent:
					this.processor.SetComponent(header);
					return true;
				case ECBCommand.SetEntityEnabled:
					this.processor.SetEnabled(header);
					return true;
				case ECBCommand.SetComponentEnabled:
					this.processor.SetComponentEnabled(header);
					return true;
				case ECBCommand.SetName:
					this.processor.SetName(header);
					return true;
				case ECBCommand.AddBuffer:
					this.processor.AddBuffer(header);
					return true;
				case ECBCommand.SetBuffer:
					this.processor.SetBuffer(header);
					return true;
				case ECBCommand.AppendToBuffer:
					this.processor.AppendToBuffer(header);
					return true;
				case ECBCommand.AddComponentLinkedEntityGroup:
					this.processor.AddComponentLinkedEntityGroup(header);
					return true;
				case ECBCommand.SetComponentLinkedEntityGroup:
					this.processor.SetComponentLinkedEntityGroup(header);
					return true;
				case ECBCommand.ReplaceComponentLinkedEntityGroup:
					this.processor.ReplaceComponentLinkedEntityGroup(header);
					return true;
				case ECBCommand.AddUnmanagedSharedComponentData:
					this.processor.AddUnmanagedSharedComponentData(header);
					return true;
				case ECBCommand.SetUnmanagedSharedComponentData:
					this.processor.SetUnmanagedSharedComponentData(header);
					return true;
				case ECBCommand.AddUnmanagedSharedComponentValueForMultipleEntities:
					this.processor.AddUnmanagedSharedComponentValueForMultipleEntities(header);
					return true;
				case ECBCommand.SetUnmanagedSharedComponentValueForMultipleEntities:
					this.processor.SetUnmanagedSharedComponentValueForMultipleEntities(header);
					return true;
				case ECBCommand.AddUnmanagedSharedComponentValueForEntityQuery:
					this.processor.AddUnmanagedSharedComponentValueForEntityQuery(header);
					return true;
				case ECBCommand.SetUnmanagedSharedComponentValueForEntityQuery:
					this.processor.SetUnmanagedSharedComponentValueForEntityQuery(header);
					return true;
				case ECBCommand.AddComponentForEntityQuery:
					this.processor.AddComponentForEntityQuery(header);
					return true;
				case ECBCommand.AddMultipleComponentsForMultipleEntities:
					this.processor.AddMultipleComponentsForMultipleEntities(header);
					return true;
				case ECBCommand.AddMultipleComponentsForEntityQuery:
					this.processor.AddMultipleComponentsForEntityQuery(header);
					return true;
				case ECBCommand.RemoveComponentForEntityQuery:
					this.processor.RemoveComponentForEntityQuery(header);
					return true;
				case ECBCommand.RemoveMultipleComponentsForMultipleEntities:
					this.processor.RemoveMultipleComponentsForMultipleEntities(header);
					return true;
				case ECBCommand.RemoveMultipleComponentsForEntityQuery:
					this.processor.RemoveMultipleComponentsForEntityQuery(header);
					return true;
				case ECBCommand.AddComponentForMultipleEntities:
					this.processor.AddComponentForMultipleEntities(header);
					return true;
				case ECBCommand.RemoveComponentForMultipleEntities:
					this.processor.RemoveComponentForMultipleEntities(header);
					return true;
				case ECBCommand.DestroyMultipleEntities:
					this.processor.DestroyMultipleEntities(header);
					return true;
				case ECBCommand.DestroyForEntityQuery:
					this.processor.DestroyForEntityQuery(header);
					return true;
				}
				return false;
			}

			// Token: 0x04000329 RID: 809
			public T processor;

			// Token: 0x0400032A RID: 810
			public EntityCommandBuffer.ECBProcessorType processorType;
		}

		// Token: 0x02000116 RID: 278
		internal struct PlaybackProcessor : EntityCommandBuffer.IEcbProcessor
		{
			// Token: 0x060004F7 RID: 1271 RVA: 0x0001B600 File Offset: 0x00019800
			public unsafe void Init(EntityDataAccess* entityDataAccess, EntityCommandBufferData* data, in SystemHandle originSystemHandle)
			{
				this.mgr = entityDataAccess;
				this.playbackPolicy = data->m_PlaybackPolicy;
				this.isFirstPlayback = (data->m_DidPlayback ? 0 : 1);
				this.originSystem = originSystemHandle;
				if (data->m_RecordedChainCount > 0)
				{
					this.archetypeChanges = this.mgr->BeginStructuralChanges();
					this.trackStructuralChanges = 1;
				}
				this.entityCount = -data->m_Entity.Index;
				this.bufferCount = *data->m_BufferWithFixups.Counter;
				Entity* ptr = null;
				ECBSharedPlaybackState.BufferWithFixUp* ptr2 = null;
				if (this.entityCount > 0)
				{
					ptr = (Entity*)Memory.Unmanaged.Allocate((long)(this.entityCount * sizeof(Entity)), 4, Allocator.Temp);
				}
				if (this.bufferCount > 0)
				{
					ptr2 = (ECBSharedPlaybackState.BufferWithFixUp*)Memory.Unmanaged.Allocate((long)(this.bufferCount * sizeof(ECBSharedPlaybackState.BufferWithFixUp)), 4, Allocator.Temp);
				}
				this.playbackState = new ECBSharedPlaybackState
				{
					CommandBufferID = data->m_CommandBufferID,
					CreateEntityBatch = ptr,
					BuffersWithFixUp = ptr2,
					CreatedEntityCount = this.entityCount,
					LastBuffer = 0
				};
			}

			// Token: 0x060004F8 RID: 1272 RVA: 0x0001B714 File Offset: 0x00019914
			public unsafe void Cleanup()
			{
				for (int i = 0; i < this.playbackState.LastBuffer; i++)
				{
					EntityBufferCommand* cmd = this.playbackState.BuffersWithFixUp[i].cmd;
					Entity entity = EntityCommandBuffer.SelectEntity(cmd->Header.Entity, in this.playbackState);
					if (this.mgr->Exists(entity) && this.mgr->HasComponent(entity, ComponentType.FromTypeIndex(cmd->ComponentTypeIndex)))
					{
						EntityCommandBuffer.FixupBufferContents(this.mgr, cmd, entity, this.playbackState);
					}
				}
				Memory.Unmanaged.Free<Entity>(this.playbackState.CreateEntityBatch, Allocator.Temp);
				Memory.Unmanaged.Free<ECBSharedPlaybackState.BufferWithFixUp>(this.playbackState.BuffersWithFixUp, Allocator.Temp);
				if (this.trackStructuralChanges != 0)
				{
					this.mgr->EndStructuralChanges(ref this.archetypeChanges);
				}
			}

			// Token: 0x060004F9 RID: 1273 RVA: 0x0001B7E8 File Offset: 0x000199E8
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe void DestroyEntity(BasicCommand* header)
			{
				Entity entity = EntityCommandBuffer.SelectEntity(((EntityCommand*)header)->Entity, in this.playbackState);
				this.mgr->DestroyEntityInternalDuringStructuralChange(&entity, 1, in this.originSystem);
			}

			// Token: 0x060004FA RID: 1274 RVA: 0x0001B820 File Offset: 0x00019A20
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe void RemoveComponent(BasicCommand* header)
			{
				Entity entity = EntityCommandBuffer.SelectEntity(((EntityComponentCommand*)header)->Header.Entity, in this.playbackState);
				this.mgr->RemoveComponentDuringStructuralChange(entity, ComponentType.FromTypeIndex(((EntityComponentCommand*)header)->ComponentTypeIndex), in this.originSystem);
			}

			// Token: 0x060004FB RID: 1275 RVA: 0x0001B864 File Offset: 0x00019A64
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe void RemoveMultipleComponents(BasicCommand* header)
			{
				Entity entity = EntityCommandBuffer.SelectEntity(((EntityMultipleComponentsCommand*)header)->Header.Entity, in this.playbackState);
				ComponentTypeSet typeSet = ((EntityMultipleComponentsCommand*)header)->TypeSet;
				this.mgr->RemoveComponentDuringStructuralChange(entity, in typeSet, in this.originSystem);
			}

			// Token: 0x060004FC RID: 1276 RVA: 0x0001B8A8 File Offset: 0x00019AA8
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe void CreateEntity(BasicCommand* header)
			{
				EntityArchetype entityArchetype = ((CreateCommand*)header)->Archetype;
				if (!entityArchetype.Valid)
				{
					entityArchetype = this.mgr->GetEntityAndSimulateArchetype();
				}
				int num = -((CreateCommand*)header)->IdentityIndex - 1;
				this.mgr->CreateEntityDuringStructuralChange(entityArchetype, this.playbackState.CreateEntityBatch + num, ((CreateCommand*)header)->BatchCount, in this.originSystem);
			}

			// Token: 0x060004FD RID: 1277 RVA: 0x0001B90C File Offset: 0x00019B0C
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe void InstantiateEntity(BasicCommand* header)
			{
				int num = -((EntityCommand*)header)->IdentityIndex - 1;
				Entity entity = EntityCommandBuffer.SelectEntity(((EntityCommand*)header)->Entity, in this.playbackState);
				this.mgr->InstantiateInternalDuringStructuralChange(entity, this.playbackState.CreateEntityBatch + num, ((EntityCommand*)header)->BatchCount, in this.originSystem);
			}

			// Token: 0x060004FE RID: 1278 RVA: 0x0001B964 File Offset: 0x00019B64
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			private unsafe void SetComponentFromEmbeddedValue(EntityComponentCommand* cmd, Entity entity)
			{
				byte* ptr = (byte*)(cmd + 1);
				if (cmd->ValueRequiresEntityFixup != 0)
				{
					EntityCommandBuffer.FixupTemporaryEntitiesInComponentValue(ptr, cmd->ComponentTypeIndex, in this.playbackState);
				}
				this.mgr->SetComponentDataRaw(entity, cmd->ComponentTypeIndex, (void*)ptr, (int)cmd->ComponentSize, in this.originSystem);
			}

			// Token: 0x060004FF RID: 1279 RVA: 0x0001B9B4 File Offset: 0x00019BB4
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe void AddComponent(BasicCommand* header)
			{
				ComponentType componentType = ComponentType.FromTypeIndex(((EntityComponentCommand*)header)->ComponentTypeIndex);
				Entity entity = EntityCommandBuffer.SelectEntity(((EntityComponentCommand*)header)->Header.Entity, in this.playbackState);
				this.mgr->AddComponentDuringStructuralChange(entity, componentType, in this.originSystem);
				if (((EntityComponentCommand*)header)->ComponentSize != 0)
				{
					this.SetComponentFromEmbeddedValue((EntityComponentCommand*)header, entity);
				}
			}

			// Token: 0x06000500 RID: 1280 RVA: 0x0001BA0C File Offset: 0x00019C0C
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe void AddMultipleComponents(BasicCommand* header)
			{
				Entity entity = EntityCommandBuffer.SelectEntity(((EntityMultipleComponentsCommand*)header)->Header.Entity, in this.playbackState);
				ComponentTypeSet typeSet = ((EntityMultipleComponentsCommand*)header)->TypeSet;
				ref EntityDataAccess ptr = ref *this.mgr;
				Entity entity2 = entity;
				SystemHandle systemHandle = default(SystemHandle);
				ptr.AddMultipleComponentsDuringStructuralChange(entity2, in typeSet, in systemHandle);
			}

			// Token: 0x06000501 RID: 1281 RVA: 0x0001BA50 File Offset: 0x00019C50
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe void SetComponent(BasicCommand* header)
			{
				Entity entity = EntityCommandBuffer.SelectEntity(((EntityComponentCommand*)header)->Header.Entity, in this.playbackState);
				this.SetComponentFromEmbeddedValue((EntityComponentCommand*)header, entity);
			}

			// Token: 0x06000502 RID: 1282 RVA: 0x0001BA80 File Offset: 0x00019C80
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe void SetEnabled(BasicCommand* header)
			{
				Entity entity = EntityCommandBuffer.SelectEntity(((EntityEnabledCommand*)header)->Header.Entity, in this.playbackState);
				this.mgr->SetEnabled(entity, ((EntityEnabledCommand*)header)->IsEnabled > 0);
			}

			// Token: 0x06000503 RID: 1283 RVA: 0x0001BABC File Offset: 0x00019CBC
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe void SetComponentEnabled(BasicCommand* header)
			{
				Entity entity = EntityCommandBuffer.SelectEntity(((EntityComponentEnabledCommand*)header)->Header.Header.Entity, in this.playbackState);
				this.mgr->SetComponentEnabled(entity, ((EntityComponentEnabledCommand*)header)->ComponentTypeIndex, ((EntityComponentEnabledCommand*)header)->Header.IsEnabled > 0);
			}

			// Token: 0x06000504 RID: 1284 RVA: 0x0001BB08 File Offset: 0x00019D08
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe void SetName(BasicCommand* header)
			{
				Entity entity = EntityCommandBuffer.SelectEntity(((EntityNameCommand*)header)->Header.Entity, in this.playbackState);
				this.mgr->SetName(entity, in ((EntityNameCommand*)header)->Name);
			}

			// Token: 0x06000505 RID: 1285 RVA: 0x0001BB40 File Offset: 0x00019D40
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe void AddBuffer(BasicCommand* header)
			{
				Entity entity = EntityCommandBuffer.SelectEntity(((EntityBufferCommand*)header)->Header.Entity, in this.playbackState);
				this.mgr->AddComponentDuringStructuralChange(entity, ComponentType.FromTypeIndex(((EntityBufferCommand*)header)->ComponentTypeIndex), in this.originSystem);
				if (this.playbackPolicy == PlaybackPolicy.SinglePlayback)
				{
					this.mgr->SetBufferRaw(entity, ((EntityBufferCommand*)header)->ComponentTypeIndex, &((EntityBufferCommand*)header)->BufferNode.TempBuffer, (int)((EntityBufferCommand*)header)->ComponentSize, in this.originSystem);
					if (((EntityBufferCommand*)header)->ValueRequiresEntityFixup != 0)
					{
						EntityCommandBuffer.AddToPostPlaybackFixup((EntityBufferCommand*)header, ref this.playbackState);
						return;
					}
				}
				else
				{
					Hint.Unlikely(((EntityBufferCommand*)header)->ValueRequiresEntityFixup > 0);
					BufferHeader bufferHeader = EntityCommandBuffer.CloneBuffer(&((EntityBufferCommand*)header)->BufferNode.TempBuffer, ((EntityBufferCommand*)header)->ComponentTypeIndex);
					this.mgr->SetBufferRaw(entity, ((EntityBufferCommand*)header)->ComponentTypeIndex, &bufferHeader, (int)((EntityBufferCommand*)header)->ComponentSize, in this.originSystem);
				}
			}

			// Token: 0x06000506 RID: 1286 RVA: 0x0001BC14 File Offset: 0x00019E14
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe void SetBuffer(BasicCommand* header)
			{
				Entity entity = EntityCommandBuffer.SelectEntity(((EntityBufferCommand*)header)->Header.Entity, in this.playbackState);
				if (this.playbackPolicy == PlaybackPolicy.SinglePlayback)
				{
					this.mgr->SetBufferRaw(entity, ((EntityBufferCommand*)header)->ComponentTypeIndex, &((EntityBufferCommand*)header)->BufferNode.TempBuffer, (int)((EntityBufferCommand*)header)->ComponentSize, in this.originSystem);
					if (((EntityBufferCommand*)header)->ValueRequiresEntityFixup != 0)
					{
						EntityCommandBuffer.AddToPostPlaybackFixup((EntityBufferCommand*)header, ref this.playbackState);
						return;
					}
				}
				else
				{
					Hint.Unlikely(((EntityBufferCommand*)header)->ValueRequiresEntityFixup > 0);
					BufferHeader bufferHeader = EntityCommandBuffer.CloneBuffer(&((EntityBufferCommand*)header)->BufferNode.TempBuffer, ((EntityBufferCommand*)header)->ComponentTypeIndex);
					this.mgr->SetBufferRaw(entity, ((EntityBufferCommand*)header)->ComponentTypeIndex, &bufferHeader, (int)((EntityBufferCommand*)header)->ComponentSize, in this.originSystem);
				}
			}

			// Token: 0x06000507 RID: 1287 RVA: 0x0001BCCC File Offset: 0x00019ECC
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe void AppendToBuffer(BasicCommand* header)
			{
				Entity entity = EntityCommandBuffer.SelectEntity(((EntityComponentCommand*)header)->Header.Entity, in this.playbackState);
				BufferHeader* componentDataRW_AsBytePointer = (BufferHeader*)this.mgr->GetComponentDataRW_AsBytePointer(entity, ((EntityComponentCommand*)header)->ComponentTypeIndex);
				readonly ref TypeManager.TypeInfo typeInfo = ref TypeManager.GetTypeInfo(((EntityComponentCommand*)header)->ComponentTypeIndex);
				int alignmentInBytes = typeInfo.AlignmentInBytes;
				int elementSize = typeInfo.ElementSize;
				BufferHeader.EnsureCapacity(componentDataRW_AsBytePointer, componentDataRW_AsBytePointer->Length + 1, elementSize, alignmentInBytes, BufferHeader.TrashMode.RetainOldData, false, 0);
				int num = componentDataRW_AsBytePointer->Length * elementSize;
				UnsafeUtility.MemCpy((void*)(BufferHeader.GetElementPointer(componentDataRW_AsBytePointer) + num), (void*)(header + sizeof(EntityComponentCommand) / sizeof(BasicCommand)), (long)elementSize);
				componentDataRW_AsBytePointer->Length++;
				if (((EntityComponentCommand*)header)->ValueRequiresEntityFixup != 0)
				{
					EntityCommandBuffer.FixupTemporaryEntitiesInComponentValue(BufferHeader.GetElementPointer(componentDataRW_AsBytePointer) + num, typeInfo.TypeIndex, in this.playbackState);
				}
			}

			// Token: 0x06000508 RID: 1288 RVA: 0x0001BD87 File Offset: 0x00019F87
			private unsafe void CreateTemporaryNativeArrayView(Entity* entities, int entityCount, out NativeArray<Entity> outArray)
			{
				outArray = NativeArrayUnsafeUtility.ConvertExistingDataToNativeArray<Entity>((void*)entities, entityCount, Allocator.None);
			}

			// Token: 0x06000509 RID: 1289 RVA: 0x0001BD98 File Offset: 0x00019F98
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe void AddComponentForEntityQuery(BasicCommand* header)
			{
				ComponentType componentType = ComponentType.FromTypeIndex(((EntityQueryComponentCommand*)header)->ComponentTypeIndex);
				if (Hint.Likely(this.trackStructuralChanges > 0))
				{
					EntityCommandBuffer.CommitStructuralChanges(this.mgr, ref this.archetypeChanges);
				}
				this.mgr->AddComponentToQueryDuringStructuralChange(((EntityQueryComponentCommand*)header)->Header.QueryImpl, componentType, in this.originSystem);
			}

			// Token: 0x0600050A RID: 1290 RVA: 0x0001BDF4 File Offset: 0x00019FF4
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe void AddComponentForMultipleEntities(BasicCommand* header)
			{
				ComponentType componentType = ComponentType.FromTypeIndex(((MultipleEntitiesComponentCommand*)header)->ComponentTypeIndex);
				NativeArray<Entity> nativeArray;
				this.CreateTemporaryNativeArrayView(((MultipleEntitiesComponentCommand*)header)->Header.Entities.Ptr, ((MultipleEntitiesComponentCommand*)header)->Header.EntitiesCount, out nativeArray);
				if (((MultipleEntitiesComponentCommand*)header)->Header.SkipDeferredEntityLookup == 0)
				{
					int length = nativeArray.Length;
					for (int i = 0; i < length; i++)
					{
						if (nativeArray[i].Index < 0)
						{
							nativeArray[i] = EntityCommandBuffer.SelectEntity(nativeArray[i], in this.playbackState);
						}
					}
				}
				this.mgr->AddComponentDuringStructuralChange(nativeArray, componentType, in this.originSystem);
				if (((MultipleEntitiesComponentCommand*)header)->ComponentSize > 0)
				{
					byte* ptr = (byte*)(header + sizeof(MultipleEntitiesComponentCommand) / sizeof(BasicCommand));
					if (((MultipleEntitiesComponentCommand*)header)->ValueRequiresEntityFixup != 0)
					{
						EntityCommandBuffer.FixupTemporaryEntitiesInComponentValue(ptr, ((MultipleEntitiesComponentCommand*)header)->ComponentTypeIndex, in this.playbackState);
					}
					for (int j = 0; j < nativeArray.Length; j++)
					{
						this.mgr->SetComponentDataRaw(nativeArray[j], ((MultipleEntitiesComponentCommand*)header)->ComponentTypeIndex, (void*)(header + sizeof(MultipleEntitiesComponentCommand) / sizeof(BasicCommand)), (int)((MultipleEntitiesComponentCommand*)header)->ComponentSize, in this.originSystem);
					}
				}
			}

			// Token: 0x0600050B RID: 1291 RVA: 0x0001BF0C File Offset: 0x0001A10C
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe void RemoveComponentForEntityQuery(BasicCommand* header)
			{
				ComponentType componentType = ComponentType.FromTypeIndex(((EntityQueryComponentCommand*)header)->ComponentTypeIndex);
				if (Hint.Likely(this.trackStructuralChanges > 0))
				{
					EntityCommandBuffer.CommitStructuralChanges(this.mgr, ref this.archetypeChanges);
				}
				this.mgr->RemoveComponentFromQueryDuringStructuralChange(((EntityQueryComponentCommand*)header)->Header.QueryImpl, componentType, in this.originSystem);
			}

			// Token: 0x0600050C RID: 1292 RVA: 0x0001BF68 File Offset: 0x0001A168
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe void RemoveComponentForMultipleEntities(BasicCommand* header)
			{
				ComponentType componentType = ComponentType.FromTypeIndex(((MultipleEntitiesComponentCommand*)header)->ComponentTypeIndex);
				NativeArray<Entity> nativeArray;
				this.CreateTemporaryNativeArrayView(((MultipleEntitiesComponentCommand*)header)->Header.Entities.Ptr, ((MultipleEntitiesComponentCommand*)header)->Header.EntitiesCount, out nativeArray);
				if (((MultipleEntitiesComponentCommand*)header)->Header.SkipDeferredEntityLookup == 0)
				{
					int length = nativeArray.Length;
					for (int i = 0; i < length; i++)
					{
						if (nativeArray[i].Index < 0)
						{
							nativeArray[i] = EntityCommandBuffer.SelectEntity(nativeArray[i], in this.playbackState);
						}
					}
				}
				this.mgr->RemoveComponentDuringStructuralChange(nativeArray, componentType, in this.originSystem);
			}

			// Token: 0x0600050D RID: 1293 RVA: 0x0001C00C File Offset: 0x0001A20C
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe void AddMultipleComponentsForMultipleEntities(BasicCommand* header)
			{
				NativeArray<Entity> nativeArray;
				this.CreateTemporaryNativeArrayView(((MultipleEntitiesAndComponentsCommand*)header)->Header.Entities.Ptr, ((MultipleEntitiesAndComponentsCommand*)header)->Header.EntitiesCount, out nativeArray);
				if (((MultipleEntitiesAndComponentsCommand*)header)->Header.SkipDeferredEntityLookup == 0)
				{
					int length = nativeArray.Length;
					for (int i = 0; i < length; i++)
					{
						if (nativeArray[i].Index < 0)
						{
							nativeArray[i] = EntityCommandBuffer.SelectEntity(nativeArray[i], in this.playbackState);
						}
					}
				}
				this.mgr->AddMultipleComponentsDuringStructuralChange(nativeArray, in ((MultipleEntitiesAndComponentsCommand*)header)->TypeSet, in this.originSystem);
			}

			// Token: 0x0600050E RID: 1294 RVA: 0x0001C0A4 File Offset: 0x0001A2A4
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe void AddMultipleComponentsForEntityQuery(BasicCommand* header)
			{
				if (Hint.Likely(this.trackStructuralChanges > 0))
				{
					EntityCommandBuffer.CommitStructuralChanges(this.mgr, ref this.archetypeChanges);
				}
				this.mgr->AddComponentsToQueryDuringStructuralChange(((EntityQueryComponentTypeSetCommand*)header)->Header.QueryImpl, in ((EntityQueryComponentTypeSetCommand*)header)->TypeSet, in this.originSystem);
			}

			// Token: 0x0600050F RID: 1295 RVA: 0x0001C0F8 File Offset: 0x0001A2F8
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe void RemoveMultipleComponentsForMultipleEntities(BasicCommand* header)
			{
				NativeArray<Entity> nativeArray;
				this.CreateTemporaryNativeArrayView(((MultipleEntitiesAndComponentsCommand*)header)->Header.Entities.Ptr, ((MultipleEntitiesAndComponentsCommand*)header)->Header.EntitiesCount, out nativeArray);
				if (((MultipleEntitiesAndComponentsCommand*)header)->Header.SkipDeferredEntityLookup == 0)
				{
					int length = nativeArray.Length;
					for (int i = 0; i < length; i++)
					{
						if (nativeArray[i].Index < 0)
						{
							nativeArray[i] = EntityCommandBuffer.SelectEntity(nativeArray[i], in this.playbackState);
						}
					}
				}
				this.mgr->RemoveMultipleComponentsDuringStructuralChange(nativeArray, in ((MultipleEntitiesAndComponentsCommand*)header)->TypeSet, in this.originSystem);
			}

			// Token: 0x06000510 RID: 1296 RVA: 0x0001C190 File Offset: 0x0001A390
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe void RemoveMultipleComponentsForEntityQuery(BasicCommand* header)
			{
				if (Hint.Likely(this.trackStructuralChanges > 0))
				{
					EntityCommandBuffer.CommitStructuralChanges(this.mgr, ref this.archetypeChanges);
				}
				this.mgr->RemoveMultipleComponentsFromQueryDuringStructuralChange(((EntityQueryComponentTypeSetCommand*)header)->Header.QueryImpl, in ((EntityQueryComponentTypeSetCommand*)header)->TypeSet, in this.originSystem);
			}

			// Token: 0x06000511 RID: 1297 RVA: 0x0001C1E4 File Offset: 0x0001A3E4
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe void DestroyMultipleEntities(BasicCommand* header)
			{
				NativeArray<Entity> nativeArray;
				this.CreateTemporaryNativeArrayView(((MultipleEntitiesCommand*)header)->Entities.Ptr, ((MultipleEntitiesCommand*)header)->EntitiesCount, out nativeArray);
				if (((MultipleEntitiesCommand*)header)->SkipDeferredEntityLookup == 0)
				{
					int length = nativeArray.Length;
					for (int i = 0; i < length; i++)
					{
						if (nativeArray[i].Index < 0)
						{
							nativeArray[i] = EntityCommandBuffer.SelectEntity(nativeArray[i], in this.playbackState);
						}
					}
				}
				this.mgr->DestroyEntityDuringStructuralChange(nativeArray, in this.originSystem);
			}

			// Token: 0x06000512 RID: 1298 RVA: 0x0001C268 File Offset: 0x0001A468
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe void DestroyForEntityQuery(BasicCommand* header)
			{
				if (Hint.Likely(this.trackStructuralChanges > 0))
				{
					EntityCommandBuffer.CommitStructuralChanges(this.mgr, ref this.archetypeChanges);
				}
				this.mgr->DestroyEntitiesInQueryDuringStructuralChange(((EntityQueryCommand*)header)->QueryImpl, in this.originSystem);
			}

			// Token: 0x06000513 RID: 1299 RVA: 0x0001C2B0 File Offset: 0x0001A4B0
			public unsafe void AddComponentLinkedEntityGroup(BasicCommand* header)
			{
				Entity entity = EntityCommandBuffer.SelectEntity(((EntityQueryMaskCommand*)header)->Header.Header.Entity, in this.playbackState);
				byte* ptr = (byte*)(header + sizeof(EntityQueryMaskCommand) / sizeof(BasicCommand));
				if (((EntityQueryMaskCommand*)header)->Header.ValueRequiresEntityFixup != 0)
				{
					EntityCommandBuffer.FixupTemporaryEntitiesInComponentValue(ptr, ((EntityQueryMaskCommand*)header)->Header.ComponentTypeIndex, in this.playbackState);
				}
				if (Hint.Likely(this.trackStructuralChanges > 0))
				{
					EntityCommandBuffer.CommitStructuralChanges(this.mgr, ref this.archetypeChanges);
				}
				this.mgr->AddComponentForLinkedEntityGroup(entity, ((EntityQueryMaskCommand*)header)->Mask, ((EntityQueryMaskCommand*)header)->Header.ComponentTypeIndex, (void*)ptr, (int)((EntityQueryMaskCommand*)header)->Header.ComponentSize);
			}

			// Token: 0x06000514 RID: 1300 RVA: 0x0001C354 File Offset: 0x0001A554
			public unsafe void SetComponentLinkedEntityGroup(BasicCommand* header)
			{
				Entity entity = EntityCommandBuffer.SelectEntity(((EntityQueryMaskCommand*)header)->Header.Header.Entity, in this.playbackState);
				byte* ptr = (byte*)(header + sizeof(EntityQueryMaskCommand) / sizeof(BasicCommand));
				if (((EntityQueryMaskCommand*)header)->Header.ValueRequiresEntityFixup != 0)
				{
					EntityCommandBuffer.FixupTemporaryEntitiesInComponentValue(ptr, ((EntityQueryMaskCommand*)header)->Header.ComponentTypeIndex, in this.playbackState);
				}
				if (Hint.Likely(this.trackStructuralChanges > 0))
				{
					EntityCommandBuffer.CommitStructuralChanges(this.mgr, ref this.archetypeChanges);
				}
				this.mgr->SetComponentForLinkedEntityGroup(entity, ((EntityQueryMaskCommand*)header)->Mask, ((EntityQueryMaskCommand*)header)->Header.ComponentTypeIndex, (void*)ptr, (int)((EntityQueryMaskCommand*)header)->Header.ComponentSize);
			}

			// Token: 0x06000515 RID: 1301 RVA: 0x0001C3F8 File Offset: 0x0001A5F8
			public unsafe void ReplaceComponentLinkedEntityGroup(BasicCommand* header)
			{
				Entity entity = EntityCommandBuffer.SelectEntity(((EntityComponentCommand*)header)->Header.Entity, in this.playbackState);
				byte* ptr = (byte*)(header + sizeof(EntityComponentCommand) / sizeof(BasicCommand));
				if (((EntityComponentCommand*)header)->ValueRequiresEntityFixup != 0)
				{
					EntityCommandBuffer.FixupTemporaryEntitiesInComponentValue(ptr, ((EntityComponentCommand*)header)->ComponentTypeIndex, in this.playbackState);
				}
				this.mgr->ReplaceComponentForLinkedEntityGroup(entity, ((EntityComponentCommand*)header)->ComponentTypeIndex, (void*)ptr, (int)((EntityComponentCommand*)header)->ComponentSize);
			}

			// Token: 0x06000516 RID: 1302 RVA: 0x0001C45C File Offset: 0x0001A65C
			[BurstDiscard]
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe void AddManagedComponentData(BasicCommand* header)
			{
				Entity entity = EntityCommandBuffer.SelectEntity(((EntityManagedComponentCommand*)header)->Header.Entity, in this.playbackState);
				if (this.mgr->AddComponentDuringStructuralChange(entity, ComponentType.FromTypeIndex(((EntityManagedComponentCommand*)header)->ComponentTypeIndex), in this.originSystem))
				{
					EntityCommandBuffer.CommitStructuralChanges(this.mgr, ref this.archetypeChanges);
				}
				object boxedObject = ((EntityManagedComponentCommand*)header)->GetBoxedObject();
				if (boxedObject != null && TypeManager.HasEntityReferences(((EntityManagedComponentCommand*)header)->ComponentTypeIndex))
				{
					EntityCommandBuffer.FixupManagedComponent.FixUpComponent(boxedObject, in this.playbackState);
				}
				(ref *this.mgr).SetComponentObject(entity, ComponentType.FromTypeIndex(((EntityManagedComponentCommand*)header)->ComponentTypeIndex), boxedObject, in this.originSystem);
			}

			// Token: 0x06000517 RID: 1303 RVA: 0x0001C4F4 File Offset: 0x0001A6F4
			[BurstDiscard]
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe void MoveManagedComponentData(BasicCommand* header)
			{
				Entity entity = EntityCommandBuffer.SelectEntity(((EntityMoveManagedComponentCommand*)header)->SrcEntity, in this.playbackState);
				Entity entity2 = EntityCommandBuffer.SelectEntity(((EntityMoveManagedComponentCommand*)header)->Header.Entity, in this.playbackState);
				ComponentType componentType = ComponentType.FromTypeIndex(((EntityMoveManagedComponentCommand*)header)->ComponentTypeIndex);
				(ref *this.mgr).MoveComponentObjectDuringStructuralChange(entity, entity2, componentType, in this.originSystem);
				EntityCommandBuffer.CommitStructuralChanges(this.mgr, ref this.archetypeChanges);
			}

			// Token: 0x06000518 RID: 1304 RVA: 0x0001C560 File Offset: 0x0001A760
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe void AddUnmanagedSharedComponentData(BasicCommand* header)
			{
				Entity entity = EntityCommandBuffer.SelectEntity(((EntityUnmanagedSharedComponentCommand*)header)->Header.Entity, in this.playbackState);
				byte* ptr = (byte*)(header + sizeof(EntityUnmanagedSharedComponentCommand) / sizeof(BasicCommand));
				if (((EntityUnmanagedSharedComponentCommand*)header)->IsDefault == 0 && ((EntityUnmanagedSharedComponentCommand*)header)->ValueRequiresEntityFixup != 0)
				{
					EntityCommandBuffer.FixupTemporaryEntitiesInComponentValue(ptr, ((EntityUnmanagedSharedComponentCommand*)header)->ComponentTypeIndex, in this.playbackState);
				}
				NativeArray<Entity> nativeArray = new NativeArray<Entity>(1, Allocator.Temp, NativeArrayOptions.UninitializedMemory);
				nativeArray[0] = entity;
				this.mgr->AddSharedComponentDataAddrDefaultMustBeNullDuringStructuralChange(nativeArray, ((EntityUnmanagedSharedComponentCommand*)header)->ComponentTypeIndex, ((EntityUnmanagedSharedComponentCommand*)header)->HashCode, (void*)((((EntityUnmanagedSharedComponentCommand*)header)->IsDefault == 0) ? ptr : null));
				EntityCommandBuffer.CommitStructuralChanges(this.mgr, ref this.archetypeChanges);
			}

			// Token: 0x06000519 RID: 1305 RVA: 0x0001C5FC File Offset: 0x0001A7FC
			[BurstDiscard]
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe void AddSharedComponentData(BasicCommand* header)
			{
				Entity entity = EntityCommandBuffer.SelectEntity(((EntitySharedComponentCommand*)header)->Header.Entity, in this.playbackState);
				if (this.mgr->AddSharedComponentDataBoxedDefaultMustBeNullDuringStructuralChange(entity, ((EntitySharedComponentCommand*)header)->ComponentTypeIndex, ((EntitySharedComponentCommand*)header)->HashCode, ((EntitySharedComponentCommand*)header)->GetBoxedObject(), in this.originSystem))
				{
					EntityCommandBuffer.CommitStructuralChanges(this.mgr, ref this.archetypeChanges);
				}
			}

			// Token: 0x0600051A RID: 1306 RVA: 0x0001C65C File Offset: 0x0001A85C
			[BurstDiscard]
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe void AddComponentObjectForMultipleEntities(BasicCommand* header)
			{
				ComponentType componentType = ComponentType.FromTypeIndex(((MultipleEntitiesComponentCommandWithObject*)header)->ComponentTypeIndex);
				NativeArray<Entity> nativeArray;
				this.CreateTemporaryNativeArrayView(((MultipleEntitiesComponentCommandWithObject*)header)->Header.Entities.Ptr, ((MultipleEntitiesComponentCommandWithObject*)header)->Header.EntitiesCount, out nativeArray);
				if (((MultipleEntitiesComponentCommandWithObject*)header)->Header.SkipDeferredEntityLookup == 0)
				{
					int length = nativeArray.Length;
					for (int i = 0; i < length; i++)
					{
						if (nativeArray[i].Index < 0)
						{
							nativeArray[i] = EntityCommandBuffer.SelectEntity(nativeArray[i], in this.playbackState);
						}
					}
				}
				this.mgr->AddComponentDuringStructuralChange(nativeArray, componentType, in this.originSystem);
				EntityCommandBuffer.CommitStructuralChanges(this.mgr, ref this.archetypeChanges);
				object boxedObject = ((MultipleEntitiesComponentCommandWithObject*)header)->GetBoxedObject();
				TypeIndex componentTypeIndex = ((MultipleEntitiesComponentCommandWithObject*)header)->ComponentTypeIndex;
				if (boxedObject != null && TypeManager.HasEntityReferences(componentTypeIndex))
				{
					EntityCommandBuffer.FixupManagedComponent.FixUpComponent(boxedObject, in this.playbackState);
				}
				int length2 = nativeArray.Length;
				for (int j = 0; j < length2; j++)
				{
					(ref *this.mgr).SetComponentObject(nativeArray[j], componentType, boxedObject, in this.originSystem);
				}
			}

			// Token: 0x0600051B RID: 1307 RVA: 0x0001C770 File Offset: 0x0001A970
			[BurstDiscard]
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe void SetComponentObjectForMultipleEntities(BasicCommand* header)
			{
				ComponentType componentType = ComponentType.FromTypeIndex(((MultipleEntitiesComponentCommandWithObject*)header)->ComponentTypeIndex);
				NativeArray<Entity> nativeArray;
				this.CreateTemporaryNativeArrayView(((MultipleEntitiesComponentCommandWithObject*)header)->Header.Entities.Ptr, ((MultipleEntitiesComponentCommandWithObject*)header)->Header.EntitiesCount, out nativeArray);
				if (((MultipleEntitiesComponentCommandWithObject*)header)->Header.SkipDeferredEntityLookup == 0)
				{
					int length = nativeArray.Length;
					for (int i = 0; i < length; i++)
					{
						if (nativeArray[i].Index < 0)
						{
							nativeArray[i] = EntityCommandBuffer.SelectEntity(nativeArray[i], in this.playbackState);
						}
					}
				}
				if (!this.mgr->EntityComponentStore->ManagedChangesTracker.Empty)
				{
					EntityCommandBuffer.CommitStructuralChanges(this.mgr, ref this.archetypeChanges);
				}
				object boxedObject = ((MultipleEntitiesComponentCommandWithObject*)header)->GetBoxedObject();
				TypeIndex componentTypeIndex = ((MultipleEntitiesComponentCommandWithObject*)header)->ComponentTypeIndex;
				if (boxedObject != null && TypeManager.HasEntityReferences(componentTypeIndex))
				{
					EntityCommandBuffer.FixupManagedComponent.FixUpComponent(boxedObject, in this.playbackState);
				}
				int length2 = nativeArray.Length;
				for (int j = 0; j < length2; j++)
				{
					(ref *this.mgr).SetComponentObject(nativeArray[j], componentType, boxedObject, in this.originSystem);
				}
			}

			// Token: 0x0600051C RID: 1308 RVA: 0x0001C888 File Offset: 0x0001AA88
			[BurstDiscard]
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe void AddSharedComponentWithValueForMultipleEntities(BasicCommand* header)
			{
				NativeArray<Entity> nativeArray;
				this.CreateTemporaryNativeArrayView(((MultipleEntitiesComponentCommandWithObject*)header)->Header.Entities.Ptr, ((MultipleEntitiesComponentCommandWithObject*)header)->Header.EntitiesCount, out nativeArray);
				if (((MultipleEntitiesComponentCommandWithObject*)header)->Header.SkipDeferredEntityLookup == 0)
				{
					int length = nativeArray.Length;
					for (int i = 0; i < length; i++)
					{
						if (nativeArray[i].Index < 0)
						{
							nativeArray[i] = EntityCommandBuffer.SelectEntity(nativeArray[i], in this.playbackState);
						}
					}
				}
				object boxedObject = ((MultipleEntitiesComponentCommandWithObject*)header)->GetBoxedObject();
				int hashCode = ((MultipleEntitiesComponentCommandWithObject*)header)->HashCode;
				TypeIndex componentTypeIndex = ((MultipleEntitiesComponentCommandWithObject*)header)->ComponentTypeIndex;
				this.mgr->AddSharedComponentDataBoxedDefaultMustBeNullDuringStructuralChange(nativeArray, componentTypeIndex, hashCode, boxedObject, in this.originSystem);
				EntityCommandBuffer.CommitStructuralChanges(this.mgr, ref this.archetypeChanges);
			}

			// Token: 0x0600051D RID: 1309 RVA: 0x0001C94C File Offset: 0x0001AB4C
			[BurstDiscard]
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe void AddSharedComponentWithValueForEntityQuery(BasicCommand* header)
			{
				if (Hint.Likely(this.trackStructuralChanges > 0))
				{
					EntityCommandBuffer.CommitStructuralChanges(this.mgr, ref this.archetypeChanges);
				}
				TypeIndex componentTypeIndex = ((EntityQueryComponentCommandWithObject*)header)->Header.ComponentTypeIndex;
				int num = this.mgr->InsertSharedComponent_Managed(componentTypeIndex, ((EntityQueryComponentCommandWithObject*)header)->HashCode, ((EntityQueryComponentCommandWithObject*)header)->GetBoxedObject());
				this.mgr->AddSharedComponentDataToQueryDuringStructuralChange(((EntityQueryComponentCommandWithObject*)header)->Header.Header.QueryImpl, num, ComponentType.FromTypeIndex(componentTypeIndex), in this.originSystem);
				EntityCommandBuffer.CommitStructuralChanges(this.mgr, ref this.archetypeChanges);
			}

			// Token: 0x0600051E RID: 1310 RVA: 0x0001C9DC File Offset: 0x0001ABDC
			[BurstDiscard]
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe void SetSharedComponentValueForMultipleEntities(BasicCommand* header)
			{
				NativeArray<Entity> nativeArray;
				this.CreateTemporaryNativeArrayView(((MultipleEntitiesComponentCommandWithObject*)header)->Header.Entities.Ptr, ((MultipleEntitiesComponentCommandWithObject*)header)->Header.EntitiesCount, out nativeArray);
				if (((MultipleEntitiesComponentCommandWithObject*)header)->Header.SkipDeferredEntityLookup == 0)
				{
					int length = nativeArray.Length;
					for (int i = 0; i < length; i++)
					{
						if (nativeArray[i].Index < 0)
						{
							nativeArray[i] = EntityCommandBuffer.SelectEntity(nativeArray[i], in this.playbackState);
						}
					}
				}
				object boxedObject = ((MultipleEntitiesComponentCommandWithObject*)header)->GetBoxedObject();
				int hashCode = ((MultipleEntitiesComponentCommandWithObject*)header)->HashCode;
				TypeIndex componentTypeIndex = ((MultipleEntitiesComponentCommandWithObject*)header)->ComponentTypeIndex;
				int length2 = nativeArray.Length;
				for (int j = 0; j < length2; j++)
				{
					Entity entity = nativeArray[j];
					this.mgr->SetSharedComponentDataBoxedDefaultMustBeNullDuringStructuralChange(entity, componentTypeIndex, hashCode, boxedObject, in this.originSystem);
				}
				EntityCommandBuffer.CommitStructuralChanges(this.mgr, ref this.archetypeChanges);
			}

			// Token: 0x0600051F RID: 1311 RVA: 0x0001CAC8 File Offset: 0x0001ACC8
			[BurstDiscard]
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe void SetSharedComponentValueForEntityQuery(BasicCommand* header)
			{
				if (Hint.Likely(this.trackStructuralChanges > 0))
				{
					EntityCommandBuffer.CommitStructuralChanges(this.mgr, ref this.archetypeChanges);
				}
				TypeIndex componentTypeIndex = ((EntityQueryComponentCommandWithObject*)header)->Header.ComponentTypeIndex;
				int num = this.mgr->InsertSharedComponent_Managed(componentTypeIndex, ((EntityQueryComponentCommandWithObject*)header)->HashCode, ((EntityQueryComponentCommandWithObject*)header)->GetBoxedObject());
				this.mgr->SetSharedComponentDataOnQueryDuringStructuralChange(((EntityQueryComponentCommandWithObject*)header)->Header.Header.QueryImpl, num, ComponentType.FromTypeIndex(componentTypeIndex), in this.originSystem);
				EntityCommandBuffer.CommitStructuralChanges(this.mgr, ref this.archetypeChanges);
			}

			// Token: 0x06000520 RID: 1312 RVA: 0x0001CB58 File Offset: 0x0001AD58
			[BurstDiscard]
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe void SetManagedComponentData(BasicCommand* header)
			{
				Entity entity = EntityCommandBuffer.SelectEntity(((EntityManagedComponentCommand*)header)->Header.Entity, in this.playbackState);
				if (!this.mgr->EntityComponentStore->ManagedChangesTracker.Empty)
				{
					EntityCommandBuffer.CommitStructuralChanges(this.mgr, ref this.archetypeChanges);
				}
				object boxedObject = ((EntityManagedComponentCommand*)header)->GetBoxedObject();
				if (boxedObject != null && TypeManager.HasEntityReferences(((EntityManagedComponentCommand*)header)->ComponentTypeIndex))
				{
					EntityCommandBuffer.FixupManagedComponent.FixUpComponent(boxedObject, in this.playbackState);
				}
				(ref *this.mgr).SetComponentObject(entity, ComponentType.FromTypeIndex(((EntityManagedComponentCommand*)header)->ComponentTypeIndex), ((EntityManagedComponentCommand*)header)->GetBoxedObject(), in this.originSystem);
			}

			// Token: 0x06000521 RID: 1313 RVA: 0x0001CBEC File Offset: 0x0001ADEC
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe void SetUnmanagedSharedComponentData(BasicCommand* header)
			{
				Entity entity = EntityCommandBuffer.SelectEntity(((EntityUnmanagedSharedComponentCommand*)header)->Header.Entity, in this.playbackState);
				byte* ptr = (byte*)(header + sizeof(EntityUnmanagedSharedComponentCommand) / sizeof(BasicCommand));
				if (((EntityUnmanagedSharedComponentCommand*)header)->IsDefault == 0 && ((EntityUnmanagedSharedComponentCommand*)header)->ValueRequiresEntityFixup != 0)
				{
					EntityCommandBuffer.FixupTemporaryEntitiesInComponentValue(ptr, ((EntityUnmanagedSharedComponentCommand*)header)->ComponentTypeIndex, in this.playbackState);
				}
				NativeArray<Entity> nativeArray = new NativeArray<Entity>(1, Allocator.Temp, NativeArrayOptions.UninitializedMemory);
				nativeArray[0] = entity;
				ref EntityDataAccess ptr2 = ref *this.mgr;
				NativeArray<Entity> nativeArray2 = nativeArray;
				TypeIndex componentTypeIndex = ((EntityUnmanagedSharedComponentCommand*)header)->ComponentTypeIndex;
				int hashCode = ((EntityUnmanagedSharedComponentCommand*)header)->HashCode;
				void* ptr3 = (void*)((((EntityUnmanagedSharedComponentCommand*)header)->IsDefault == 0) ? ptr : null);
				SystemHandle systemHandle = default(SystemHandle);
				ptr2.SetSharedComponentDataAddrDefaultMustBeNullDuringStructuralChange(nativeArray2, componentTypeIndex, hashCode, ptr3, in systemHandle);
			}

			// Token: 0x06000522 RID: 1314 RVA: 0x0001CC80 File Offset: 0x0001AE80
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe void AddUnmanagedSharedComponentValueForMultipleEntities(BasicCommand* header)
			{
				NativeArray<Entity> nativeArray;
				this.CreateTemporaryNativeArrayView(((MultipleEntitiesCommand_WithUnmanagedSharedComponent*)header)->Header.Entities.Ptr, ((MultipleEntitiesCommand_WithUnmanagedSharedComponent*)header)->Header.EntitiesCount, out nativeArray);
				if (((MultipleEntitiesCommand_WithUnmanagedSharedComponent*)header)->Header.SkipDeferredEntityLookup == 0)
				{
					int length = nativeArray.Length;
					for (int i = 0; i < length; i++)
					{
						if (nativeArray[i].Index < 0)
						{
							nativeArray[i] = EntityCommandBuffer.SelectEntity(nativeArray[i], in this.playbackState);
						}
					}
				}
				int hashCode = ((MultipleEntitiesCommand_WithUnmanagedSharedComponent*)header)->HashCode;
				TypeIndex componentTypeIndex = ((MultipleEntitiesCommand_WithUnmanagedSharedComponent*)header)->ComponentTypeIndex;
				byte* ptr = (byte*)(header + sizeof(MultipleEntitiesCommand_WithUnmanagedSharedComponent) / sizeof(BasicCommand));
				if (((MultipleEntitiesCommand_WithUnmanagedSharedComponent*)header)->IsDefault == 0 && ((MultipleEntitiesCommand_WithUnmanagedSharedComponent*)header)->ValueRequiresEntityFixup != 0)
				{
					EntityCommandBuffer.FixupTemporaryEntitiesInComponentValue(ptr, componentTypeIndex, in this.playbackState);
				}
				this.mgr->AddSharedComponentDataAddrDefaultMustBeNullDuringStructuralChange(nativeArray, componentTypeIndex, hashCode, (void*)((((MultipleEntitiesCommand_WithUnmanagedSharedComponent*)header)->IsDefault == 0) ? ptr : null));
				EntityCommandBuffer.CommitStructuralChanges(this.mgr, ref this.archetypeChanges);
			}

			// Token: 0x06000523 RID: 1315 RVA: 0x0001CD6C File Offset: 0x0001AF6C
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe void AddUnmanagedSharedComponentValueForEntityQuery(BasicCommand* header)
			{
				if (Hint.Likely(this.trackStructuralChanges > 0))
				{
					EntityCommandBuffer.CommitStructuralChanges(this.mgr, ref this.archetypeChanges);
				}
				TypeIndex componentTypeIndex = ((EntityQueryComponentCommandWithUnmanagedSharedComponent*)header)->Header.ComponentTypeIndex;
				byte* ptr = (byte*)(header + sizeof(EntityQueryComponentCommandWithUnmanagedSharedComponent) / sizeof(BasicCommand));
				if (((EntityQueryComponentCommandWithUnmanagedSharedComponent*)header)->IsDefault == 0 && ((EntityQueryComponentCommandWithUnmanagedSharedComponent*)header)->ValueRequiresEntityFixup != 0)
				{
					EntityCommandBuffer.FixupTemporaryEntitiesInComponentValue(ptr, componentTypeIndex, in this.playbackState);
				}
				int num = this.mgr->InsertSharedComponent_Unmanaged(componentTypeIndex, ((EntityQueryComponentCommandWithUnmanagedSharedComponent*)header)->HashCode, (void*)((((EntityQueryComponentCommandWithUnmanagedSharedComponent*)header)->IsDefault == 1) ? null : ptr), null);
				this.mgr->AddSharedComponentDataToQueryDuringStructuralChange_Unmanaged(((EntityQueryComponentCommandWithUnmanagedSharedComponent*)header)->Header.Header.QueryImpl, num, ComponentType.FromTypeIndex(componentTypeIndex), (void*)((((EntityQueryComponentCommandWithUnmanagedSharedComponent*)header)->IsDefault == 1) ? null : ptr), in this.originSystem);
				EntityCommandBuffer.CommitStructuralChanges(this.mgr, ref this.archetypeChanges);
			}

			// Token: 0x06000524 RID: 1316 RVA: 0x0001CE38 File Offset: 0x0001B038
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe void SetUnmanagedSharedComponentValueForMultipleEntities(BasicCommand* header)
			{
				NativeArray<Entity> nativeArray;
				this.CreateTemporaryNativeArrayView(((MultipleEntitiesCommand_WithUnmanagedSharedComponent*)header)->Header.Entities.Ptr, ((MultipleEntitiesCommand_WithUnmanagedSharedComponent*)header)->Header.EntitiesCount, out nativeArray);
				if (((MultipleEntitiesCommand_WithUnmanagedSharedComponent*)header)->Header.SkipDeferredEntityLookup == 0)
				{
					int length = nativeArray.Length;
					for (int i = 0; i < length; i++)
					{
						if (nativeArray[i].Index < 0)
						{
							nativeArray[i] = EntityCommandBuffer.SelectEntity(nativeArray[i], in this.playbackState);
						}
					}
				}
				byte* ptr = (byte*)(header + sizeof(MultipleEntitiesCommand_WithUnmanagedSharedComponent) / sizeof(BasicCommand));
				if (((MultipleEntitiesCommand_WithUnmanagedSharedComponent*)header)->IsDefault == 0 && ((MultipleEntitiesCommand_WithUnmanagedSharedComponent*)header)->ValueRequiresEntityFixup != 0)
				{
					EntityCommandBuffer.FixupTemporaryEntitiesInComponentValue(ptr, ((MultipleEntitiesCommand_WithUnmanagedSharedComponent*)header)->ComponentTypeIndex, in this.playbackState);
				}
				ref EntityDataAccess ptr2 = ref *this.mgr;
				NativeArray<Entity> nativeArray2 = nativeArray;
				TypeIndex componentTypeIndex = ((MultipleEntitiesCommand_WithUnmanagedSharedComponent*)header)->ComponentTypeIndex;
				int hashCode = ((MultipleEntitiesCommand_WithUnmanagedSharedComponent*)header)->HashCode;
				void* ptr3 = (void*)((((MultipleEntitiesCommand_WithUnmanagedSharedComponent*)header)->IsDefault == 1) ? null : ptr);
				SystemHandle systemHandle = default(SystemHandle);
				ptr2.SetSharedComponentDataAddrDefaultMustBeNullDuringStructuralChange(nativeArray2, componentTypeIndex, hashCode, ptr3, in systemHandle);
			}

			// Token: 0x06000525 RID: 1317 RVA: 0x0001CF18 File Offset: 0x0001B118
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe void SetUnmanagedSharedComponentValueForEntityQuery(BasicCommand* header)
			{
				if (Hint.Likely(this.trackStructuralChanges > 0))
				{
					EntityCommandBuffer.CommitStructuralChanges(this.mgr, ref this.archetypeChanges);
				}
				TypeIndex componentTypeIndex = ((EntityQueryComponentCommandWithUnmanagedSharedComponent*)header)->Header.ComponentTypeIndex;
				byte* ptr = (byte*)(header + sizeof(EntityQueryComponentCommandWithUnmanagedSharedComponent) / sizeof(BasicCommand));
				if (((EntityQueryComponentCommandWithUnmanagedSharedComponent*)header)->IsDefault == 0 && ((EntityQueryComponentCommandWithUnmanagedSharedComponent*)header)->ValueRequiresEntityFixup != 0)
				{
					EntityCommandBuffer.FixupTemporaryEntitiesInComponentValue(ptr, componentTypeIndex, in this.playbackState);
				}
				int num = this.mgr->InsertSharedComponent_Unmanaged(componentTypeIndex, ((EntityQueryComponentCommandWithUnmanagedSharedComponent*)header)->HashCode, (void*)((((EntityQueryComponentCommandWithUnmanagedSharedComponent*)header)->IsDefault == 1) ? null : ptr), null);
				this.mgr->SetSharedComponentDataOnQueryDuringStructuralChange_Unmanaged(((EntityQueryComponentCommandWithUnmanagedSharedComponent*)header)->Header.Header.QueryImpl, num, ComponentType.FromTypeIndex(componentTypeIndex), (void*)((((EntityQueryComponentCommandWithUnmanagedSharedComponent*)header)->IsDefault == 1) ? null : ptr), in this.originSystem);
				EntityCommandBuffer.CommitStructuralChanges(this.mgr, ref this.archetypeChanges);
			}

			// Token: 0x17000096 RID: 150
			// (get) Token: 0x06000526 RID: 1318 RVA: 0x0001CFE4 File Offset: 0x0001B1E4
			public EntityCommandBuffer.ECBProcessorType ProcessorType
			{
				get
				{
					return EntityCommandBuffer.ECBProcessorType.PlaybackProcessor;
				}
			}

			// Token: 0x06000527 RID: 1319 RVA: 0x0001CFE8 File Offset: 0x0001B1E8
			public unsafe void SetSharedComponentData(BasicCommand* header)
			{
				Entity entity = EntityCommandBuffer.SelectEntity(((EntitySharedComponentCommand*)header)->Header.Entity, in this.playbackState);
				this.mgr->SetSharedComponentDataBoxedDefaultMustBeNullDuringStructuralChange(entity, ((EntitySharedComponentCommand*)header)->ComponentTypeIndex, ((EntitySharedComponentCommand*)header)->HashCode, ((EntitySharedComponentCommand*)header)->GetBoxedObject(), in this.originSystem);
			}

			// Token: 0x0400032B RID: 811
			public unsafe EntityDataAccess* mgr;

			// Token: 0x0400032C RID: 812
			public EntityComponentStore.ArchetypeChanges archetypeChanges;

			// Token: 0x0400032D RID: 813
			public ECBSharedPlaybackState playbackState;

			// Token: 0x0400032E RID: 814
			public PlaybackPolicy playbackPolicy;

			// Token: 0x0400032F RID: 815
			public byte isFirstPlayback;

			// Token: 0x04000330 RID: 816
			public int entityCount;

			// Token: 0x04000331 RID: 817
			public int bufferCount;

			// Token: 0x04000332 RID: 818
			public SystemHandle originSystem;

			// Token: 0x04000333 RID: 819
			private byte trackStructuralChanges;
		}

		// Token: 0x02000117 RID: 279
		private class FixupManagedComponent : PropertyVisitor, IVisitPropertyAdapter<Entity>, IPropertyVisitorAdapter
		{
			// Token: 0x06000528 RID: 1320 RVA: 0x000166AD File Offset: 0x000148AD
			public FixupManagedComponent()
			{
				base.AddAdapter(this);
			}

			// Token: 0x06000529 RID: 1321 RVA: 0x0001D034 File Offset: 0x0001B234
			public static void FixUpComponent(object obj, in ECBSharedPlaybackState state)
			{
				EntityCommandBuffer.FixupManagedComponent fixupManagedComponent = EntityCommandBuffer.FixupManagedComponent._CachedVisitor;
				if (EntityCommandBuffer.FixupManagedComponent._CachedVisitor == null)
				{
					fixupManagedComponent = (EntityCommandBuffer.FixupManagedComponent._CachedVisitor = new EntityCommandBuffer.FixupManagedComponent());
				}
				fixupManagedComponent.PlaybackState = state;
				PropertyContainer.Accept<object>(fixupManagedComponent, ref obj, default(VisitParameters));
			}

			// Token: 0x0600052A RID: 1322 RVA: 0x0001D078 File Offset: 0x0001B278
			unsafe void IVisitPropertyAdapter<Entity>.Visit<TContainer>(in VisitContext<TContainer, Entity> context, ref TContainer container, ref Entity value)
			{
				if (value.Index < 0)
				{
					int num = -value.Index - 1;
					Entity entity = this.PlaybackState.CreateEntityBatch[num];
					value = entity;
				}
			}

			// Token: 0x04000334 RID: 820
			[ThreadStatic]
			public static EntityCommandBuffer.FixupManagedComponent _CachedVisitor;

			// Token: 0x04000335 RID: 821
			private ECBSharedPlaybackState PlaybackState;
		}

		// Token: 0x02000118 RID: 280
		[NativeContainer]
		[NativeContainerIsAtomicWriteOnly]
		public struct ParallelWriter
		{
			// Token: 0x0600052B RID: 1323 RVA: 0x0001D0BA File Offset: 0x0001B2BA
			[Conditional("ENABLE_UNITY_COLLECTIONS_CHECKS")]
			[Conditional("UNITY_DOTS_DEBUG")]
			private void CheckWriteAccess()
			{
				if (this.m_Data == null)
				{
					throw new NullReferenceException("The EntityCommandBuffer has not been initialized! The EntityCommandBuffer needs to be passed an Allocator when created!");
				}
			}

			// Token: 0x17000097 RID: 151
			// (get) Token: 0x0600052C RID: 1324 RVA: 0x0001D0D1 File Offset: 0x0001B2D1
			private unsafe EntityCommandBufferChain* ThreadChain
			{
				get
				{
					if (this.m_ThreadIndex < 0)
					{
						return &this.m_Data->m_MainThreadChain;
					}
					return this.m_Data->m_ThreadedChains + this.m_ThreadIndex;
				}
			}

			// Token: 0x0600052D RID: 1325 RVA: 0x0001D104 File Offset: 0x0001B304
			public Entity CreateEntity(int sortKey, EntityArchetype archetype)
			{
				return this._CreateEntity(sortKey, archetype);
			}

			// Token: 0x0600052E RID: 1326 RVA: 0x0001D110 File Offset: 0x0001B310
			public Entity CreateEntity(int sortKey)
			{
				return this._CreateEntity(sortKey, default(EntityArchetype));
			}

			// Token: 0x0600052F RID: 1327 RVA: 0x0001D130 File Offset: 0x0001B330
			private unsafe Entity _CreateEntity(int sortKey, EntityArchetype archetype)
			{
				EntityCommandBufferChain* threadChain = this.ThreadChain;
				Entity entity = this.m_Data->m_Entity;
				entity.Index = Interlocked.Decrement(ref this.m_Data->m_Entity.Index);
				this.m_Data->AddCreateCommand(threadChain, sortKey, ECBCommand.CreateEntity, entity.Index, archetype, false);
				return entity;
			}

			// Token: 0x06000530 RID: 1328 RVA: 0x0001D183 File Offset: 0x0001B383
			[Conditional("ENABLE_UNITY_COLLECTIONS_CHECKS")]
			[Conditional("UNITY_DOTS_DEBUG")]
			private static void CheckNotNull(Entity e)
			{
				if (Hint.Unlikely(e == Entity.Null))
				{
					throw new ArgumentNullException("e");
				}
			}

			// Token: 0x06000531 RID: 1329 RVA: 0x0001D1A4 File Offset: 0x0001B3A4
			public unsafe Entity Instantiate(int sortKey, Entity e)
			{
				EntityCommandBufferChain* threadChain = this.ThreadChain;
				Entity entity = this.m_Data->m_Entity;
				entity.Index = Interlocked.Decrement(ref this.m_Data->m_Entity.Index);
				this.m_Data->AddEntityCommand(threadChain, sortKey, ECBCommand.InstantiateEntity, entity.Index, e, false);
				return entity;
			}

			// Token: 0x06000532 RID: 1330 RVA: 0x0001D1F8 File Offset: 0x0001B3F8
			public unsafe void Instantiate(int sortKey, Entity e, NativeArray<Entity> entities)
			{
				EntityCommandBufferChain* threadChain = this.ThreadChain;
				Entity entity = this.m_Data->m_Entity;
				int num = Interlocked.Add(ref this.m_Data->m_Entity.Index, -entities.Length) + entities.Length - 1;
				for (int i = 0; i < entities.Length; i++)
				{
					entity.Index = num - i;
					entities[i] = entity;
				}
				this.m_Data->AddMultipleEntityCommand(threadChain, sortKey, ECBCommand.InstantiateEntity, num, entities.Length, e, false);
			}

			// Token: 0x06000533 RID: 1331 RVA: 0x0001D280 File Offset: 0x0001B480
			public unsafe void DestroyEntity(int sortKey, Entity e)
			{
				EntityCommandBufferChain* threadChain = this.ThreadChain;
				this.m_Data->AddEntityCommand(threadChain, sortKey, ECBCommand.DestroyEntity, 0, e, false);
			}

			// Token: 0x06000534 RID: 1332 RVA: 0x0001D2A8 File Offset: 0x0001B4A8
			public unsafe void DestroyEntity(int sortKey, NativeArray<Entity> entities)
			{
				EntityCommandBufferChain* threadChain = this.ThreadChain;
				bool flag;
				Entity* ptr = this.m_Data->CloneAndSearchForDeferredEntities(entities, out flag);
				this.m_Data->AppendMultipleEntitiesCommand(threadChain, sortKey, ECBCommand.DestroyMultipleEntities, ptr, entities.Length, flag);
			}

			// Token: 0x06000535 RID: 1333 RVA: 0x0001D2E4 File Offset: 0x0001B4E4
			public unsafe void AddComponent<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(int sortKey, Entity e, T component) where T : struct, ValueType, IComponentData
			{
				EntityCommandBufferChain* threadChain = this.ThreadChain;
				this.m_Data->AddEntityComponentTypeWithValueCommand<T>(threadChain, sortKey, ECBCommand.AddComponent, e, component);
			}

			// Token: 0x06000536 RID: 1334 RVA: 0x0001D308 File Offset: 0x0001B508
			internal unsafe void UnsafeAddComponent(int sortKey, Entity e, TypeIndex typeIndex, int typeSize, void* componentDataPtr)
			{
				EntityCommandBufferChain* threadChain = this.ThreadChain;
				this.m_Data->UnsafeAddEntityComponentCommand(threadChain, sortKey, ECBCommand.AddComponent, e, typeIndex, typeSize, componentDataPtr);
			}

			// Token: 0x06000537 RID: 1335 RVA: 0x0001D330 File Offset: 0x0001B530
			public unsafe void AddComponent<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(int sortKey, NativeArray<Entity> entities, T component) where T : struct, ValueType, IComponentData
			{
				EntityCommandBufferChain* threadChain = this.ThreadChain;
				bool flag;
				Entity* ptr = this.m_Data->CloneAndSearchForDeferredEntities(entities, out flag);
				this.m_Data->AppendMultipleEntitiesComponentCommandWithValue<T>(threadChain, sortKey, ECBCommand.AddComponentForMultipleEntities, ptr, entities.Length, flag, component);
			}

			// Token: 0x06000538 RID: 1336 RVA: 0x0001D370 File Offset: 0x0001B570
			public unsafe void AddComponent<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(int sortKey, Entity e) where T : struct, ValueType, IComponentData
			{
				EntityCommandBufferChain* threadChain = this.ThreadChain;
				this.m_Data->AddEntityComponentTypeWithoutValueCommand(threadChain, sortKey, ECBCommand.AddComponent, e, ComponentType.ReadWrite<T>());
			}

			// Token: 0x06000539 RID: 1337 RVA: 0x0001D398 File Offset: 0x0001B598
			public unsafe void AddComponent<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(int sortKey, NativeArray<Entity> entities) where T : struct, ValueType, IComponentData
			{
				EntityCommandBufferChain* threadChain = this.ThreadChain;
				bool flag;
				Entity* ptr = this.m_Data->CloneAndSearchForDeferredEntities(entities, out flag);
				this.m_Data->AppendMultipleEntitiesComponentCommand(threadChain, sortKey, ECBCommand.AddComponentForMultipleEntities, ptr, entities.Length, flag, ComponentType.ReadWrite<T>());
			}

			// Token: 0x0600053A RID: 1338 RVA: 0x0001D3DC File Offset: 0x0001B5DC
			public unsafe void AddComponent(int sortKey, Entity e, ComponentType componentType)
			{
				EntityCommandBufferChain* threadChain = this.ThreadChain;
				this.m_Data->AddEntityComponentTypeWithoutValueCommand(threadChain, sortKey, ECBCommand.AddComponent, e, componentType);
			}

			// Token: 0x0600053B RID: 1339 RVA: 0x0001D400 File Offset: 0x0001B600
			public unsafe void AddComponent(int sortKey, NativeArray<Entity> entities, ComponentType componentType)
			{
				EntityCommandBufferChain* threadChain = this.ThreadChain;
				bool flag;
				Entity* ptr = this.m_Data->CloneAndSearchForDeferredEntities(entities, out flag);
				this.m_Data->AppendMultipleEntitiesComponentCommand(threadChain, sortKey, ECBCommand.AddComponentForMultipleEntities, ptr, entities.Length, flag, componentType);
			}

			// Token: 0x0600053C RID: 1340 RVA: 0x0001D440 File Offset: 0x0001B640
			public unsafe void AddComponent(int sortKey, Entity e, in ComponentTypeSet typeSet)
			{
				EntityCommandBufferChain* threadChain = this.ThreadChain;
				this.m_Data->AddEntityComponentTypesCommand(threadChain, sortKey, ECBCommand.AddMultipleComponents, e, in typeSet);
			}

			// Token: 0x0600053D RID: 1341 RVA: 0x0001D464 File Offset: 0x0001B664
			public unsafe void AddComponent(int sortKey, NativeArray<Entity> entities, in ComponentTypeSet typeSet)
			{
				EntityCommandBufferChain* threadChain = this.ThreadChain;
				bool flag;
				Entity* ptr = this.m_Data->CloneAndSearchForDeferredEntities(entities, out flag);
				this.m_Data->AppendMultipleEntitiesMultipleComponentsCommand(threadChain, sortKey, ECBCommand.AddMultipleComponentsForMultipleEntities, ptr, entities.Length, flag, in typeSet);
			}

			// Token: 0x0600053E RID: 1342 RVA: 0x0001D4A4 File Offset: 0x0001B6A4
			public unsafe DynamicBuffer<T> AddBuffer<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(int sortKey, Entity e) where T : struct, ValueType, IBufferElementData
			{
				EntityCommandBufferChain* threadChain = this.ThreadChain;
				return this.m_Data->CreateBufferCommand<T>(ECBCommand.AddBuffer, threadChain, sortKey, e);
			}

			// Token: 0x0600053F RID: 1343 RVA: 0x0001D4C8 File Offset: 0x0001B6C8
			public unsafe DynamicBuffer<T> SetBuffer<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(int sortKey, Entity e) where T : struct, ValueType, IBufferElementData
			{
				EntityCommandBufferChain* threadChain = this.ThreadChain;
				return this.m_Data->CreateBufferCommand<T>(ECBCommand.SetBuffer, threadChain, sortKey, e);
			}

			// Token: 0x06000540 RID: 1344 RVA: 0x0001D4EC File Offset: 0x0001B6EC
			public unsafe void AppendToBuffer<T>(int sortKey, Entity e, T element) where T : struct, IBufferElementData
			{
				EntityCommandBufferChain* threadChain = this.ThreadChain;
				this.m_Data->AppendToBufferCommand<T>(threadChain, sortKey, e, element);
			}

			// Token: 0x06000541 RID: 1345 RVA: 0x0001D510 File Offset: 0x0001B710
			public unsafe void SetComponent<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(int sortKey, Entity e, T component) where T : struct, ValueType, IComponentData
			{
				EntityCommandBufferChain* threadChain = this.ThreadChain;
				this.m_Data->AddEntityComponentTypeWithValueCommand<T>(threadChain, sortKey, ECBCommand.SetComponent, e, component);
			}

			// Token: 0x06000542 RID: 1346 RVA: 0x0001D534 File Offset: 0x0001B734
			internal unsafe void UnsafeSetComponent(int sortKey, Entity e, TypeIndex typeIndex, int typeSize, void* componentDataPtr)
			{
				EntityCommandBufferChain* threadChain = this.ThreadChain;
				this.m_Data->UnsafeAddEntityComponentCommand(threadChain, sortKey, ECBCommand.SetComponent, e, typeIndex, typeSize, componentDataPtr);
			}

			// Token: 0x06000543 RID: 1347 RVA: 0x0001D55C File Offset: 0x0001B75C
			[GenerateTestsForBurstCompatibility]
			public unsafe void SetEnabled(int sortKey, Entity e, bool value)
			{
				EntityCommandBufferChain* threadChain = this.ThreadChain;
				this.m_Data->AddEntityEnabledCommand(threadChain, sortKey, ECBCommand.SetEntityEnabled, e, value);
			}

			// Token: 0x06000544 RID: 1348 RVA: 0x0001D580 File Offset: 0x0001B780
			[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[] { typeof(BurstCompatibleEnableableComponent) })]
			public unsafe void SetComponentEnabled<T>(int sortKey, Entity e, bool value) where T : struct, IEnableableComponent
			{
				EntityCommandBufferChain* threadChain = this.ThreadChain;
				this.m_Data->AddEntityComponentEnabledCommand(threadChain, sortKey, ECBCommand.SetComponentEnabled, e, TypeManager.GetTypeIndex<T>(), value);
			}

			// Token: 0x06000545 RID: 1349 RVA: 0x0001D5AC File Offset: 0x0001B7AC
			public unsafe void SetComponentEnabled(int sortKey, Entity e, ComponentType componentType, bool value)
			{
				EntityCommandBufferChain* threadChain = this.ThreadChain;
				this.m_Data->AddEntityComponentEnabledCommand(threadChain, sortKey, ECBCommand.SetComponentEnabled, e, componentType.TypeIndex, value);
			}

			// Token: 0x06000546 RID: 1350 RVA: 0x0001D5D8 File Offset: 0x0001B7D8
			public unsafe void SetName(int sortKey, Entity e, in FixedString64Bytes name)
			{
				EntityCommandBufferChain* threadChain = this.ThreadChain;
				this.m_Data->AddEntityNameCommand(threadChain, sortKey, ECBCommand.SetName, e, in name);
			}

			// Token: 0x06000547 RID: 1351 RVA: 0x0001D5FD File Offset: 0x0001B7FD
			public void RemoveComponent<T>(int sortKey, Entity e)
			{
				this.RemoveComponent(sortKey, e, ComponentType.ReadWrite<T>());
			}

			// Token: 0x06000548 RID: 1352 RVA: 0x0001D60C File Offset: 0x0001B80C
			public void RemoveComponent<T>(int sortKey, NativeArray<Entity> entities)
			{
				this.RemoveComponent(sortKey, entities, ComponentType.ReadWrite<T>());
			}

			// Token: 0x06000549 RID: 1353 RVA: 0x0001D61C File Offset: 0x0001B81C
			public unsafe void RemoveComponent(int sortKey, Entity e, ComponentType componentType)
			{
				EntityCommandBufferChain* threadChain = this.ThreadChain;
				this.m_Data->AddEntityComponentTypeWithoutValueCommand(threadChain, sortKey, ECBCommand.RemoveComponent, e, componentType);
			}

			// Token: 0x0600054A RID: 1354 RVA: 0x0001D640 File Offset: 0x0001B840
			public unsafe void RemoveComponent(int sortKey, NativeArray<Entity> entities, ComponentType componentType)
			{
				EntityCommandBufferChain* threadChain = this.ThreadChain;
				bool flag;
				Entity* ptr = this.m_Data->CloneAndSearchForDeferredEntities(entities, out flag);
				this.m_Data->AppendMultipleEntitiesComponentCommand(threadChain, sortKey, ECBCommand.RemoveComponentForMultipleEntities, ptr, entities.Length, flag, componentType);
			}

			// Token: 0x0600054B RID: 1355 RVA: 0x0001D680 File Offset: 0x0001B880
			public unsafe void RemoveComponent(int sortKey, Entity e, in ComponentTypeSet typeSet)
			{
				EntityCommandBufferChain* threadChain = this.ThreadChain;
				this.m_Data->AddEntityComponentTypesCommand(threadChain, sortKey, ECBCommand.RemoveMultipleComponents, e, in typeSet);
			}

			// Token: 0x0600054C RID: 1356 RVA: 0x0001D6A4 File Offset: 0x0001B8A4
			public unsafe void RemoveComponent(int sortKey, NativeArray<Entity> entities, in ComponentTypeSet typeSet)
			{
				EntityCommandBufferChain* threadChain = this.ThreadChain;
				bool flag;
				Entity* ptr = this.m_Data->CloneAndSearchForDeferredEntities(entities, out flag);
				this.m_Data->AppendMultipleEntitiesMultipleComponentsCommand(threadChain, sortKey, ECBCommand.RemoveMultipleComponentsForMultipleEntities, ptr, entities.Length, flag, in typeSet);
			}

			// Token: 0x0600054D RID: 1357 RVA: 0x0001D6E4 File Offset: 0x0001B8E4
			public unsafe void AddSharedComponentManaged<T>(int sortKey, Entity e, T sharedComponent) where T : struct, ISharedComponentData
			{
				EntityCommandBufferChain* threadChain = this.ThreadChain;
				int num;
				if (EntityCommandBuffer.IsDefaultObject<T>(ref sharedComponent, out num))
				{
					this.m_Data->AddEntitySharedComponentCommand<T>(threadChain, sortKey, ECBCommand.AddSharedComponentData, e, num, null);
					return;
				}
				this.m_Data->AddEntitySharedComponentCommand<T>(threadChain, sortKey, ECBCommand.AddSharedComponentData, e, num, sharedComponent);
			}

			// Token: 0x0600054E RID: 1358 RVA: 0x0001D730 File Offset: 0x0001B930
			public unsafe void AddSharedComponent<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(int sortKey, Entity e, T sharedComponent) where T : struct, ValueType, ISharedComponentData
			{
				int num;
				void* ptr = (EntityCommandBuffer.IsDefaultObjectUnmanaged<T>(ref sharedComponent, out num) ? null : UnsafeUtility.AddressOf<T>(ref sharedComponent));
				this.m_Data->AddEntityUnmanagedSharedComponentCommand<T>(this.ThreadChain, sortKey, ECBCommand.AddUnmanagedSharedComponentData, e, num, ptr);
			}

			// Token: 0x0600054F RID: 1359 RVA: 0x0001D76C File Offset: 0x0001B96C
			public unsafe void AddSharedComponentManaged<T>(int sortKey, NativeArray<Entity> entities, T sharedComponent) where T : struct, ISharedComponentData
			{
				EntityCommandBufferChain* threadChain = this.ThreadChain;
				bool flag;
				Entity* ptr = this.m_Data->CloneAndSearchForDeferredEntities(entities, out flag);
				int num;
				bool flag2 = EntityCommandBuffer.IsDefaultObject<T>(ref sharedComponent, out num);
				if (TypeManager.IsManagedSharedComponent(TypeManager.GetTypeIndex<T>()))
				{
					this.m_Data->AppendMultipleEntitiesComponentCommandWithSharedValue<T>(threadChain, sortKey, ECBCommand.AddSharedComponentWithValueForMultipleEntities, ptr, entities.Length, flag, num, flag2 ? null : sharedComponent);
					return;
				}
				this.m_Data->AppendMultipleEntitiesCommand_WithUnmanagedSharedValue<T>(threadChain, sortKey, ECBCommand.AddUnmanagedSharedComponentValueForMultipleEntities, ptr, entities.Length, flag, num, flag2 ? null : UnsafeUtility.AddressOf<T>(ref sharedComponent));
			}

			// Token: 0x06000550 RID: 1360 RVA: 0x0001D7F8 File Offset: 0x0001B9F8
			public unsafe void AddSharedComponent<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(int sortKey, NativeArray<Entity> entities, T sharedComponent) where T : struct, ValueType, ISharedComponentData
			{
				EntityCommandBufferChain* threadChain = this.ThreadChain;
				bool flag;
				Entity* ptr = this.m_Data->CloneAndSearchForDeferredEntities(entities, out flag);
				int num;
				bool flag2 = EntityCommandBuffer.IsDefaultObjectUnmanaged<T>(ref sharedComponent, out num);
				this.m_Data->AppendMultipleEntitiesCommand_WithUnmanagedSharedValue<T>(threadChain, sortKey, ECBCommand.AddUnmanagedSharedComponentValueForMultipleEntities, ptr, entities.Length, flag, num, flag2 ? null : UnsafeUtility.AddressOf<T>(ref sharedComponent));
			}

			// Token: 0x06000551 RID: 1361 RVA: 0x0001D850 File Offset: 0x0001BA50
			public unsafe void SetSharedComponentManaged<T>(int sortKey, Entity e, T sharedComponent) where T : struct, ISharedComponentData
			{
				EntityCommandBufferChain* threadChain = this.ThreadChain;
				TypeIndex typeIndex = TypeManager.GetTypeIndex<T>();
				int num;
				bool flag = EntityCommandBuffer.IsDefaultObject<T>(ref sharedComponent, out num);
				if (TypeManager.IsManagedSharedComponent(typeIndex))
				{
					this.m_Data->AddEntitySharedComponentCommand<T>(threadChain, sortKey, ECBCommand.SetSharedComponentData, e, num, flag ? null : sharedComponent);
					return;
				}
				this.m_Data->AddEntityUnmanagedSharedComponentCommand<T>(threadChain, sortKey, ECBCommand.SetUnmanagedSharedComponentData, e, num, flag ? null : UnsafeUtility.AddressOf<T>(ref sharedComponent));
			}

			// Token: 0x06000552 RID: 1362 RVA: 0x0001D8B8 File Offset: 0x0001BAB8
			public unsafe void SetSharedComponent<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(int sortKey, Entity e, T sharedComponent) where T : struct, ValueType, ISharedComponentData
			{
				int num;
				void* ptr = (EntityCommandBuffer.IsDefaultObjectUnmanaged<T>(ref sharedComponent, out num) ? null : UnsafeUtility.AddressOf<T>(ref sharedComponent));
				this.m_Data->AddEntityUnmanagedSharedComponentCommand<T>(this.ThreadChain, sortKey, ECBCommand.SetUnmanagedSharedComponentData, e, num, ptr);
			}

			// Token: 0x06000553 RID: 1363 RVA: 0x0001D8F4 File Offset: 0x0001BAF4
			internal unsafe void UnsafeSetSharedComponentNonDefault(int sortKey, Entity e, void* componentDataPtr, TypeIndex typeIndex)
			{
				this.m_Data->AddEntityUnmanagedSharedComponentCommand(this.ThreadChain, sortKey, ECBCommand.SetUnmanagedSharedComponentData, e, TypeManager.SharedComponentGetHashCode(componentDataPtr, typeIndex), typeIndex, TypeManager.GetTypeInfo(typeIndex).TypeSize, componentDataPtr);
			}

			// Token: 0x06000554 RID: 1364 RVA: 0x0001D930 File Offset: 0x0001BB30
			public unsafe void SetSharedComponentManaged<T>(int sortKey, NativeArray<Entity> entities, T sharedComponent) where T : struct, ISharedComponentData
			{
				EntityCommandBufferChain* threadChain = this.ThreadChain;
				bool flag;
				Entity* ptr = this.m_Data->CloneAndSearchForDeferredEntities(entities, out flag);
				int num;
				bool flag2 = EntityCommandBuffer.IsDefaultObject<T>(ref sharedComponent, out num);
				if (TypeManager.IsManagedSharedComponent(TypeManager.GetTypeIndex<T>()))
				{
					this.m_Data->AppendMultipleEntitiesComponentCommandWithSharedValue<T>(threadChain, sortKey, ECBCommand.SetSharedComponentValueForMultipleEntities, ptr, entities.Length, flag, num, flag2 ? default(T) : sharedComponent);
					return;
				}
				this.m_Data->AppendMultipleEntitiesCommand_WithUnmanagedSharedValue<T>(threadChain, sortKey, ECBCommand.SetUnmanagedSharedComponentValueForMultipleEntities, ptr, entities.Length, flag, num, flag2 ? null : UnsafeUtility.AddressOf<T>(ref sharedComponent));
			}

			// Token: 0x06000555 RID: 1365 RVA: 0x0001D9C4 File Offset: 0x0001BBC4
			public unsafe void SetSharedComponent<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(int sortKey, NativeArray<Entity> entities, T sharedComponent) where T : struct, ValueType, ISharedComponentData
			{
				EntityCommandBufferChain* threadChain = this.ThreadChain;
				bool flag;
				Entity* ptr = this.m_Data->CloneAndSearchForDeferredEntities(entities, out flag);
				int num;
				bool flag2 = EntityCommandBuffer.IsDefaultObjectUnmanaged<T>(ref sharedComponent, out num);
				this.m_Data->AppendMultipleEntitiesCommand_WithUnmanagedSharedValue<T>(threadChain, sortKey, ECBCommand.SetUnmanagedSharedComponentValueForMultipleEntities, ptr, entities.Length, flag, num, flag2 ? null : UnsafeUtility.AddressOf<T>(ref sharedComponent));
			}

			// Token: 0x06000556 RID: 1366 RVA: 0x0001DA1C File Offset: 0x0001BC1C
			public unsafe void AddComponentForLinkedEntityGroup<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(int sortKey, Entity e, EntityQueryMask mask, T component) where T : struct, ValueType, IComponentData
			{
				EntityCommandBufferChain* threadChain = this.ThreadChain;
				this.m_Data->AddLinkedEntityGroupComponentCommand<T>(threadChain, sortKey, ECBCommand.AddComponentLinkedEntityGroup, mask, e, component);
			}

			// Token: 0x06000557 RID: 1367 RVA: 0x0001DA44 File Offset: 0x0001BC44
			public unsafe void AddComponentForLinkedEntityGroup(int sortKey, Entity e, EntityQueryMask mask, ComponentType componentType)
			{
				EntityCommandBufferChain* threadChain = this.ThreadChain;
				this.m_Data->AddLinkedEntityGroupTypeCommand(threadChain, sortKey, ECBCommand.AddComponentLinkedEntityGroup, mask, e, componentType);
			}

			// Token: 0x06000558 RID: 1368 RVA: 0x0001DA6C File Offset: 0x0001BC6C
			public unsafe void SetComponentForLinkedEntityGroup<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(int sortKey, Entity e, EntityQueryMask mask, T component) where T : struct, ValueType, IComponentData
			{
				EntityCommandBufferChain* threadChain = this.ThreadChain;
				this.m_Data->AddLinkedEntityGroupComponentCommand<T>(threadChain, sortKey, ECBCommand.SetComponentLinkedEntityGroup, mask, e, component);
			}

			// Token: 0x06000559 RID: 1369 RVA: 0x0001DA94 File Offset: 0x0001BC94
			public unsafe void ReplaceComponentForLinkedEntityGroup<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(int sortKey, Entity e, T component) where T : struct, ValueType, IComponentData
			{
				EntityCommandBufferChain* threadChain = this.ThreadChain;
				this.m_Data->AddEntityComponentTypeWithValueCommand<T>(threadChain, sortKey, ECBCommand.ReplaceComponentLinkedEntityGroup, e, component);
			}

			// Token: 0x04000336 RID: 822
			[NativeDisableUnsafePtrRestriction]
			internal unsafe EntityCommandBufferData* m_Data;

			// Token: 0x04000337 RID: 823
			private const bool kBatchableCommand = false;

			// Token: 0x04000338 RID: 824
			[NativeSetThreadIndex]
			internal int m_ThreadIndex;
		}

		// Token: 0x02000119 RID: 281
		private struct ECBPlaybackWithTrace
		{
			// Token: 0x04000339 RID: 825
			public static readonly SharedStatic<int> Ref = SharedStatic<int>.GetOrCreateUnsafe(0U, 4131589732049713713L, -3722823446012893478L);
		}

		// Token: 0x0200011A RID: 282
		private struct ECBPrePlaybackValidation
		{
			// Token: 0x0400033A RID: 826
			public static readonly SharedStatic<int> Ref = SharedStatic<int>.GetOrCreateUnsafe(0U, 4131589732049713713L, -1535966604136986219L);
		}

		// Token: 0x0200011B RID: 283
		internal struct PlaybackWithTraceProcessor : EntityCommandBuffer.IEcbProcessor
		{
			// Token: 0x0600055C RID: 1372 RVA: 0x0001DAF8 File Offset: 0x0001BCF8
			public unsafe void Init(EntityDataAccess* entityDataAccess, EntityCommandBufferData* data, in SystemHandle originSystemHandle)
			{
				this.playbackProcessor.Init(entityDataAccess, data, in originSystemHandle);
				this.mgr = entityDataAccess;
				this.originSystem = originSystemHandle;
				EntityCommandBuffer.PlaybackWithTraceProcessor.ExtractDebugName(this.mgr->m_WorldUnmanaged.ResolveSystemState(this.originSystem), out this.originSystemDebugName);
				EntityCommandBuffer.PlaybackWithTraceProcessor.ExtractDebugName(this.mgr->m_WorldUnmanaged.ResolveSystemState(this.mgr->m_WorldUnmanaged.ExecutingSystem), out this.currentSystemDebugName);
				Debug.Log(string.Format("Starting EntityCommandBuffer playback in {0}; recorded from {1}.", this.currentSystemDebugName, this.originSystemDebugName));
			}

			// Token: 0x0600055D RID: 1373 RVA: 0x0001DB97 File Offset: 0x0001BD97
			public void Cleanup()
			{
				this.playbackProcessor.Cleanup();
				Debug.Log(string.Format("Ending EntityCommandBuffer playback in {0}; recorded from {1}.", this.currentSystemDebugName, this.originSystemDebugName));
			}

			// Token: 0x0600055E RID: 1374 RVA: 0x0001DBC9 File Offset: 0x0001BDC9
			internal unsafe static void ExtractDebugName(SystemState* systemState, out FixedString128Bytes fixedString)
			{
				if (systemState != null)
				{
					fixedString = new FixedString128Bytes(systemState->DebugName);
					return;
				}
				fixedString = "unknown system type";
			}

			// Token: 0x0600055F RID: 1375 RVA: 0x0001DBF4 File Offset: 0x0001BDF4
			public unsafe void LogEntitiesAndComponentsCommand(Entity* entities, int count, in ComponentTypeSet typeSet, in FixedString64Bytes commandAction)
			{
				for (int i = 0; i < count; i++)
				{
					this.LogEntityAndComponentsCommand(entities[i], in typeSet, in commandAction);
				}
			}

			// Token: 0x06000560 RID: 1376 RVA: 0x0001DC28 File Offset: 0x0001BE28
			public unsafe void LogEntitiesAndComponentCommand(Entity* entities, int count, TypeIndex typeIndex, in FixedString64Bytes commandAction)
			{
				for (int i = 0; i < count; i++)
				{
					this.LogEntityAndComponentCommand(entities[i], typeIndex, in commandAction);
				}
			}

			// Token: 0x06000561 RID: 1377 RVA: 0x0001DC5C File Offset: 0x0001BE5C
			public unsafe void LogEntityAndComponentsCommand(Entity entity, ComponentTypeInArchetype* types, int count, in FixedString64Bytes commandAction)
			{
				for (int i = 0; i < count; i++)
				{
					this.LogEntityAndComponentCommand(entity, types[i].TypeIndex, in commandAction);
				}
			}

			// Token: 0x06000562 RID: 1378 RVA: 0x0001DC90 File Offset: 0x0001BE90
			public unsafe void LogEntityAndComponentsCommand(Entity entity, in ComponentTypeSet typeSet, in FixedString64Bytes commandAction)
			{
				for (int i = 0; i < typeSet.Length; i++)
				{
					this.LogEntityAndComponentCommand(entity, typeSet.UnsafeTypesPtrRO[i], in commandAction);
				}
			}

			// Token: 0x06000563 RID: 1379 RVA: 0x0001DCCC File Offset: 0x0001BECC
			public unsafe void LogEntityAndComponentCommand(Entity cmdEntity, TypeIndex typeIndex, in FixedString64Bytes commandAction)
			{
				Entity entity = EntityCommandBuffer.SelectEntity(cmdEntity, in this.playbackProcessor.playbackState);
				FixedString64Bytes fixedString64Bytes;
				this.mgr->GetName(entity, out fixedString64Bytes);
				Debug.Log(string.Format("{0} component on entity {1}({2},{3}) for component index {4}; recorded from {5}.", new object[]
				{
					commandAction,
					fixedString64Bytes,
					entity.Index,
					entity.Version,
					typeIndex.ToFixedString(),
					this.originSystemDebugName
				}));
			}

			// Token: 0x06000564 RID: 1380 RVA: 0x0001DD60 File Offset: 0x0001BF60
			public unsafe void LogEntitiesOnlyCommand(Entity* entities, int count, in FixedString64Bytes commandAction)
			{
				for (int i = 0; i < count; i++)
				{
					this.LogEntityOnlyCommand(entities[i], in commandAction);
				}
			}

			// Token: 0x06000565 RID: 1381 RVA: 0x0001DD90 File Offset: 0x0001BF90
			public unsafe void LogEntityOnlyCommand(Entity cmdEntity, in FixedString64Bytes commandAction)
			{
				Entity entity = EntityCommandBuffer.SelectEntity(cmdEntity, in this.playbackProcessor.playbackState);
				FixedString64Bytes fixedString64Bytes;
				this.mgr->GetName(entity, out fixedString64Bytes);
				Debug.Log(string.Format("{0} entity {1}({2},{3}); recorded from {4}.", new object[] { commandAction, fixedString64Bytes, entity.Index, entity.Version, this.originSystemDebugName }));
			}

			// Token: 0x06000566 RID: 1382 RVA: 0x0001DE14 File Offset: 0x0001C014
			public unsafe void LogLinkedEntityGroupCommand(Entity cmdEntity, TypeIndex typeIndex, in EntityQueryMask mask, in FixedString64Bytes commandAction)
			{
				Entity entity = EntityCommandBuffer.SelectEntity(cmdEntity, in this.playbackProcessor.playbackState);
				FixedString64Bytes fixedString64Bytes;
				this.mgr->GetName(entity, out fixedString64Bytes);
				TypeManager.GetTypeIndex<LinkedEntityGroup>();
				using (NativeArray<Entity> nativeArray = this.mgr->GetBuffer<LinkedEntityGroup>(entity, false).Reinterpret<Entity>().ToNativeArray(this.mgr->m_WorldUnmanaged.UpdateAllocator.ToAllocator))
				{
					foreach (Entity entity2 in nativeArray)
					{
						EntityQueryMask entityQueryMask = mask;
						if (entityQueryMask.MatchesIgnoreFilter(entity2))
						{
							Debug.Log(string.Format("{0} component to {1}({2},{3})'s linked entity ({4},{5}) for component index {6}; recorded from {7}.", new object[]
							{
								commandAction,
								fixedString64Bytes,
								entity.Index,
								entity.Version,
								entity2.Index,
								entity2.Version,
								typeIndex.ToFixedString(),
								this.originSystemDebugName
							}));
						}
					}
				}
			}

			// Token: 0x06000567 RID: 1383 RVA: 0x0001DF78 File Offset: 0x0001C178
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe void DestroyEntity(BasicCommand* header)
			{
				FixedString64Bytes fixedString64Bytes = "Destroying";
				this.LogEntityOnlyCommand(((EntityCommand*)header)->Entity, in fixedString64Bytes);
				this.playbackProcessor.DestroyEntity(header);
			}

			// Token: 0x06000568 RID: 1384 RVA: 0x0001DFAC File Offset: 0x0001C1AC
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe void RemoveComponent(BasicCommand* header)
			{
				FixedString64Bytes fixedString64Bytes = "Removing";
				this.LogEntityAndComponentCommand(((EntityComponentCommand*)header)->Header.Entity, ((EntityComponentCommand*)header)->ComponentTypeIndex, in fixedString64Bytes);
				this.playbackProcessor.RemoveComponent(header);
			}

			// Token: 0x06000569 RID: 1385 RVA: 0x0001DFEC File Offset: 0x0001C1EC
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe void RemoveMultipleComponents(BasicCommand* header)
			{
				FixedString64Bytes fixedString64Bytes = "Removing";
				this.LogEntityAndComponentsCommand(((EntityMultipleComponentsCommand*)header)->Header.Entity, in ((EntityMultipleComponentsCommand*)header)->TypeSet, in fixedString64Bytes);
				this.playbackProcessor.RemoveMultipleComponents(header);
			}

			// Token: 0x0600056A RID: 1386 RVA: 0x0001E02C File Offset: 0x0001C22C
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe void CreateEntity(BasicCommand* header)
			{
				Debug.Log(string.Format("Creating {0} entity; recorded from {1}.", ((CreateCommand*)header)->BatchCount, this.originSystemDebugName));
				this.playbackProcessor.CreateEntity(header);
			}

			// Token: 0x0600056B RID: 1387 RVA: 0x0001E06C File Offset: 0x0001C26C
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe void InstantiateEntity(BasicCommand* header)
			{
				Entity entity = EntityCommandBuffer.SelectEntity(((EntityCommand*)header)->Entity, in this.playbackProcessor.playbackState);
				FixedString64Bytes fixedString64Bytes;
				this.mgr->GetName(entity, out fixedString64Bytes);
				Debug.Log(string.Format("Instantiating {0} instance(s) of entity {1}({2},{3}); recorded from {4}.", new object[]
				{
					((EntityCommand*)header)->BatchCount,
					fixedString64Bytes,
					entity.Index,
					entity.Version,
					this.originSystemDebugName
				}));
				this.playbackProcessor.InstantiateEntity(header);
			}

			// Token: 0x0600056C RID: 1388 RVA: 0x0001E104 File Offset: 0x0001C304
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe void AddComponent(BasicCommand* header)
			{
				FixedString64Bytes fixedString64Bytes = "Adding";
				this.LogEntityAndComponentCommand(((EntityComponentCommand*)header)->Header.Entity, ((EntityComponentCommand*)header)->ComponentTypeIndex, in fixedString64Bytes);
				this.playbackProcessor.AddComponent(header);
			}

			// Token: 0x0600056D RID: 1389 RVA: 0x0001E144 File Offset: 0x0001C344
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe void AddMultipleComponents(BasicCommand* header)
			{
				FixedString64Bytes fixedString64Bytes = "Adding";
				this.LogEntityAndComponentsCommand(((EntityMultipleComponentsCommand*)header)->Header.Entity, in ((EntityMultipleComponentsCommand*)header)->TypeSet, in fixedString64Bytes);
				this.playbackProcessor.AddMultipleComponents(header);
			}

			// Token: 0x0600056E RID: 1390 RVA: 0x0001E184 File Offset: 0x0001C384
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe void SetComponent(BasicCommand* header)
			{
				FixedString64Bytes fixedString64Bytes = "Setting";
				this.LogEntityAndComponentCommand(((EntityComponentCommand*)header)->Header.Entity, ((EntityComponentCommand*)header)->ComponentTypeIndex, in fixedString64Bytes);
				this.playbackProcessor.SetComponent(header);
			}

			// Token: 0x0600056F RID: 1391 RVA: 0x0001E1C4 File Offset: 0x0001C3C4
			public unsafe void SetEnabled(BasicCommand* header)
			{
				Entity entity = EntityCommandBuffer.SelectEntity(((EntityEnabledCommand*)header)->Header.Entity, in this.playbackProcessor.playbackState);
				FixedString64Bytes fixedString64Bytes;
				this.mgr->GetName(entity, out fixedString64Bytes);
				FixedString32Bytes fixedString32Bytes = ((((EntityEnabledCommand*)header)->IsEnabled == 0) ? "DISABLED" : "ENABLED");
				Debug.Log(string.Format("Setting entity {0}({1},{2}) to {3}; recorded from {4}.", new object[] { fixedString64Bytes, entity.Index, entity.Version, fixedString32Bytes, this.originSystemDebugName }));
				this.playbackProcessor.SetEnabled(header);
			}

			// Token: 0x06000570 RID: 1392 RVA: 0x0001E274 File Offset: 0x0001C474
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe void SetComponentEnabled(BasicCommand* header)
			{
				Entity entity = EntityCommandBuffer.SelectEntity(((EntityComponentEnabledCommand*)header)->Header.Header.Entity, in this.playbackProcessor.playbackState);
				FixedString64Bytes fixedString64Bytes;
				this.mgr->GetName(entity, out fixedString64Bytes);
				FixedString32Bytes fixedString32Bytes = ((((EntityComponentEnabledCommand*)header)->Header.IsEnabled == 0) ? "FALSE" : "TRUE");
				Debug.Log(string.Format("Setting component enableable on entity {0}({1},{2}) for component index {3} to {4}; recorded from {5}.", new object[]
				{
					fixedString64Bytes,
					entity.Index,
					entity.Version,
					((EntityComponentEnabledCommand*)header)->ComponentTypeIndex.ToFixedString(),
					fixedString32Bytes,
					this.originSystemDebugName
				}));
				this.playbackProcessor.SetComponentEnabled(header);
			}

			// Token: 0x06000571 RID: 1393 RVA: 0x0001E344 File Offset: 0x0001C544
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe void SetName(BasicCommand* header)
			{
				Entity entity = EntityCommandBuffer.SelectEntity(((EntityNameCommand*)header)->Header.Entity, in this.playbackProcessor.playbackState);
				FixedString64Bytes fixedString64Bytes;
				this.mgr->GetName(entity, out fixedString64Bytes);
				Debug.Log(string.Format("Setting name on entity {0}({1},{2}) with name {3}; recorded from {4}.", new object[]
				{
					fixedString64Bytes,
					entity.Index,
					entity.Version,
					((EntityNameCommand*)header)->Name,
					this.originSystemDebugName
				}));
				this.playbackProcessor.SetName(header);
			}

			// Token: 0x06000572 RID: 1394 RVA: 0x0001E3E0 File Offset: 0x0001C5E0
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe void AddBuffer(BasicCommand* header)
			{
				FixedString64Bytes fixedString64Bytes = "Adding dynamic buffer";
				this.LogEntityAndComponentCommand(((EntityBufferCommand*)header)->Header.Entity, ((EntityBufferCommand*)header)->ComponentTypeIndex, in fixedString64Bytes);
				this.playbackProcessor.AddBuffer(header);
			}

			// Token: 0x06000573 RID: 1395 RVA: 0x0001E420 File Offset: 0x0001C620
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe void SetBuffer(BasicCommand* header)
			{
				FixedString64Bytes fixedString64Bytes = "Setting dynamic buffer";
				this.LogEntityAndComponentCommand(((EntityBufferCommand*)header)->Header.Entity, ((EntityBufferCommand*)header)->ComponentTypeIndex, in fixedString64Bytes);
				this.playbackProcessor.SetBuffer(header);
			}

			// Token: 0x06000574 RID: 1396 RVA: 0x0001E460 File Offset: 0x0001C660
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe void AppendToBuffer(BasicCommand* header)
			{
				FixedString64Bytes fixedString64Bytes = "Appending element to dynamic buffer";
				this.LogEntityAndComponentCommand(((EntityComponentCommand*)header)->Header.Entity, ((EntityComponentCommand*)header)->ComponentTypeIndex, in fixedString64Bytes);
				this.playbackProcessor.AppendToBuffer(header);
			}

			// Token: 0x06000575 RID: 1397 RVA: 0x0001E4A0 File Offset: 0x0001C6A0
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe void AddComponentForEntityQuery(BasicCommand* header)
			{
				FixedString64Bytes fixedString64Bytes = "Adding";
				Debug.Log(string.Format("{0} component on entity query for component index {1}; recorded from {2}.", fixedString64Bytes, ((EntityQueryComponentCommand*)header)->ComponentTypeIndex.ToFixedString(), this.originSystemDebugName));
				this.playbackProcessor.AddComponentForEntityQuery(header);
			}

			// Token: 0x06000576 RID: 1398 RVA: 0x0001E4F8 File Offset: 0x0001C6F8
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe void AddComponentForMultipleEntities(BasicCommand* header)
			{
				FixedString64Bytes fixedString64Bytes = "Adding";
				this.LogEntitiesAndComponentCommand(((MultipleEntitiesComponentCommand*)header)->Header.Entities.Ptr, ((MultipleEntitiesComponentCommand*)header)->Header.EntitiesCount, ((MultipleEntitiesComponentCommand*)header)->ComponentTypeIndex, in fixedString64Bytes);
				this.playbackProcessor.AddComponentForMultipleEntities(header);
			}

			// Token: 0x06000577 RID: 1399 RVA: 0x0001E548 File Offset: 0x0001C748
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe void RemoveComponentForEntityQuery(BasicCommand* header)
			{
				FixedString64Bytes fixedString64Bytes = "Removing";
				Debug.Log(string.Format("{0} component on entity query for component index {1}; recorded from {2}.", fixedString64Bytes, ((EntityQueryComponentCommand*)header)->ComponentTypeIndex.ToFixedString(), this.originSystemDebugName));
				this.playbackProcessor.RemoveComponentForEntityQuery(header);
			}

			// Token: 0x06000578 RID: 1400 RVA: 0x0001E5A0 File Offset: 0x0001C7A0
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe void RemoveComponentForMultipleEntities(BasicCommand* header)
			{
				FixedString64Bytes fixedString64Bytes = "Removing";
				this.LogEntitiesAndComponentCommand(((MultipleEntitiesComponentCommand*)header)->Header.Entities.Ptr, ((MultipleEntitiesComponentCommand*)header)->Header.EntitiesCount, ((MultipleEntitiesComponentCommand*)header)->ComponentTypeIndex, in fixedString64Bytes);
				this.playbackProcessor.RemoveComponentForMultipleEntities(header);
			}

			// Token: 0x06000579 RID: 1401 RVA: 0x0001E5F0 File Offset: 0x0001C7F0
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe void AddMultipleComponentsForMultipleEntities(BasicCommand* header)
			{
				FixedString64Bytes fixedString64Bytes = "Adding";
				this.LogEntitiesAndComponentsCommand(((MultipleEntitiesAndComponentsCommand*)header)->Header.Entities.Ptr, ((MultipleEntitiesAndComponentsCommand*)header)->Header.EntitiesCount, in ((MultipleEntitiesAndComponentsCommand*)header)->TypeSet, in fixedString64Bytes);
				this.playbackProcessor.AddMultipleComponentsForMultipleEntities(header);
			}

			// Token: 0x0600057A RID: 1402 RVA: 0x0001E640 File Offset: 0x0001C840
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe void AddMultipleComponentsForEntityQuery(BasicCommand* header)
			{
				FixedString64Bytes fixedString64Bytes = "Adding";
				Debug.Log(string.Format("{0} component on entity query for component set {1}; recorded from {2}.", fixedString64Bytes, ((EntityQueryComponentTypeSetCommand*)header)->TypeSet, this.originSystemDebugName));
				this.playbackProcessor.AddMultipleComponentsForEntityQuery(header);
			}

			// Token: 0x0600057B RID: 1403 RVA: 0x0001E694 File Offset: 0x0001C894
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe void RemoveMultipleComponentsForMultipleEntities(BasicCommand* header)
			{
				FixedString64Bytes fixedString64Bytes = "Removing";
				this.LogEntitiesAndComponentsCommand(((MultipleEntitiesAndComponentsCommand*)header)->Header.Entities.Ptr, ((MultipleEntitiesAndComponentsCommand*)header)->Header.EntitiesCount, in ((MultipleEntitiesAndComponentsCommand*)header)->TypeSet, in fixedString64Bytes);
				this.playbackProcessor.RemoveMultipleComponentsForMultipleEntities(header);
			}

			// Token: 0x0600057C RID: 1404 RVA: 0x0001E6E4 File Offset: 0x0001C8E4
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe void RemoveMultipleComponentsForEntityQuery(BasicCommand* header)
			{
				FixedString64Bytes fixedString64Bytes = "Removing";
				Debug.Log(string.Format("{0} component on entity query for component set {1}; recorded from {2}.", fixedString64Bytes, ((EntityQueryComponentTypeSetCommand*)header)->TypeSet, this.originSystemDebugName));
				this.playbackProcessor.RemoveMultipleComponentsForEntityQuery(header);
			}

			// Token: 0x0600057D RID: 1405 RVA: 0x0001E738 File Offset: 0x0001C938
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe void DestroyMultipleEntities(BasicCommand* header)
			{
				FixedString64Bytes fixedString64Bytes = "Destroying";
				this.LogEntitiesOnlyCommand(((MultipleEntitiesCommand*)header)->Entities.Ptr, ((MultipleEntitiesCommand*)header)->EntitiesCount, in fixedString64Bytes);
				this.playbackProcessor.DestroyMultipleEntities(header);
			}

			// Token: 0x0600057E RID: 1406 RVA: 0x0001E778 File Offset: 0x0001C978
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe void DestroyForEntityQuery(BasicCommand* header)
			{
				Debug.Log(string.Format("Destroying entities in EntityQuery recorded from {0}.", this.originSystemDebugName));
				this.playbackProcessor.DestroyMultipleEntities(header);
			}

			// Token: 0x0600057F RID: 1407 RVA: 0x0001E7B0 File Offset: 0x0001C9B0
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe void AddComponentLinkedEntityGroup(BasicCommand* header)
			{
				FixedString64Bytes fixedString64Bytes = "Adding";
				this.LogLinkedEntityGroupCommand(((EntityQueryMaskCommand*)header)->Header.Header.Entity, ((EntityQueryMaskCommand*)header)->Header.ComponentTypeIndex, in ((EntityQueryMaskCommand*)header)->Mask, in fixedString64Bytes);
				this.playbackProcessor.AddComponentLinkedEntityGroup(header);
			}

			// Token: 0x06000580 RID: 1408 RVA: 0x0001E800 File Offset: 0x0001CA00
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe void SetComponentLinkedEntityGroup(BasicCommand* header)
			{
				FixedString64Bytes fixedString64Bytes = "Setting";
				this.LogLinkedEntityGroupCommand(((EntityQueryMaskCommand*)header)->Header.Header.Entity, ((EntityQueryMaskCommand*)header)->Header.ComponentTypeIndex, in ((EntityQueryMaskCommand*)header)->Mask, in fixedString64Bytes);
				this.playbackProcessor.SetComponentLinkedEntityGroup(header);
			}

			// Token: 0x06000581 RID: 1409 RVA: 0x0001E850 File Offset: 0x0001CA50
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe void ReplaceComponentLinkedEntityGroup(BasicCommand* header)
			{
				FixedString64Bytes fixedString64Bytes = "Replacing";
				Entity entity = EntityCommandBuffer.SelectEntity(((EntityComponentCommand*)header)->Header.Entity, in this.playbackProcessor.playbackState);
				FixedString64Bytes fixedString64Bytes2;
				this.mgr->GetName(entity, out fixedString64Bytes2);
				TypeManager.GetTypeIndex<LinkedEntityGroup>();
				using (NativeArray<Entity> nativeArray = this.mgr->GetBuffer<LinkedEntityGroup>(entity, false).Reinterpret<Entity>().ToNativeArray(this.mgr->m_WorldUnmanaged.UpdateAllocator.ToAllocator))
				{
					foreach (Entity entity2 in nativeArray)
					{
						if (this.mgr->HasComponent(entity2, ComponentType.FromTypeIndex(((EntityComponentCommand*)header)->ComponentTypeIndex)))
						{
							Debug.Log(string.Format("{0} component to {1}({2},{3})'s linked entity ({4},{5}) for component index {6}; recorded from {7}.", new object[]
							{
								fixedString64Bytes,
								fixedString64Bytes2,
								entity.Index,
								entity.Version,
								entity2.Index,
								entity2.Version,
								((EntityComponentCommand*)header)->ComponentTypeIndex.ToFixedString(),
								this.originSystemDebugName
							}));
						}
					}
					this.playbackProcessor.ReplaceComponentLinkedEntityGroup(header);
				}
			}

			// Token: 0x06000582 RID: 1410 RVA: 0x0001E9E0 File Offset: 0x0001CBE0
			[BurstDiscard]
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe void AddManagedComponentData(BasicCommand* header)
			{
				FixedString64Bytes fixedString64Bytes = "Adding managed";
				this.LogEntityAndComponentCommand(((EntityManagedComponentCommand*)header)->Header.Entity, ((EntityManagedComponentCommand*)header)->ComponentTypeIndex, in fixedString64Bytes);
				this.playbackProcessor.AddManagedComponentData(header);
			}

			// Token: 0x06000583 RID: 1411 RVA: 0x0001EA20 File Offset: 0x0001CC20
			[BurstDiscard]
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe void MoveManagedComponentData(BasicCommand* header)
			{
				FixedString64Bytes fixedString64Bytes = "Moving managed";
				this.LogEntityAndComponentCommand(((EntityMoveManagedComponentCommand*)header)->Header.Entity, ((EntityMoveManagedComponentCommand*)header)->ComponentTypeIndex, in fixedString64Bytes);
				this.playbackProcessor.MoveManagedComponentData(header);
			}

			// Token: 0x06000584 RID: 1412 RVA: 0x0001EA60 File Offset: 0x0001CC60
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe void AddUnmanagedSharedComponentData(BasicCommand* header)
			{
				FixedString64Bytes fixedString64Bytes = "Adding unmanaged shared";
				this.LogEntityAndComponentCommand(((EntityUnmanagedSharedComponentCommand*)header)->Header.Entity, ((EntityUnmanagedSharedComponentCommand*)header)->ComponentTypeIndex, in fixedString64Bytes);
				this.playbackProcessor.AddUnmanagedSharedComponentData(header);
			}

			// Token: 0x06000585 RID: 1413 RVA: 0x0001EAA0 File Offset: 0x0001CCA0
			[BurstDiscard]
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe void AddSharedComponentData(BasicCommand* header)
			{
				FixedString64Bytes fixedString64Bytes = "Adding shared";
				this.LogEntityAndComponentCommand(((EntitySharedComponentCommand*)header)->Header.Entity, ((EntitySharedComponentCommand*)header)->ComponentTypeIndex, in fixedString64Bytes);
				this.playbackProcessor.AddSharedComponentData(header);
			}

			// Token: 0x06000586 RID: 1414 RVA: 0x0001EAE0 File Offset: 0x0001CCE0
			[BurstDiscard]
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe void AddComponentObjectForMultipleEntities(BasicCommand* header)
			{
				FixedString64Bytes fixedString64Bytes = "Adding";
				this.LogEntitiesAndComponentCommand(((MultipleEntitiesComponentCommandWithObject*)header)->Header.Entities.Ptr, ((MultipleEntitiesComponentCommandWithObject*)header)->Header.EntitiesCount, ((MultipleEntitiesComponentCommandWithObject*)header)->ComponentTypeIndex, in fixedString64Bytes);
				this.playbackProcessor.AddComponentObjectForMultipleEntities(header);
			}

			// Token: 0x06000587 RID: 1415 RVA: 0x0001EB30 File Offset: 0x0001CD30
			[BurstDiscard]
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe void SetComponentObjectForMultipleEntities(BasicCommand* header)
			{
				FixedString64Bytes fixedString64Bytes = "Setting";
				this.LogEntitiesAndComponentCommand(((MultipleEntitiesComponentCommandWithObject*)header)->Header.Entities.Ptr, ((MultipleEntitiesComponentCommandWithObject*)header)->Header.EntitiesCount, ((MultipleEntitiesComponentCommandWithObject*)header)->ComponentTypeIndex, in fixedString64Bytes);
				this.playbackProcessor.SetComponentObjectForMultipleEntities(header);
			}

			// Token: 0x06000588 RID: 1416 RVA: 0x0001EB80 File Offset: 0x0001CD80
			[BurstDiscard]
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe void AddSharedComponentWithValueForMultipleEntities(BasicCommand* header)
			{
				FixedString64Bytes fixedString64Bytes = "Adding shared";
				this.LogEntitiesAndComponentCommand(((MultipleEntitiesComponentCommandWithObject*)header)->Header.Entities.Ptr, ((MultipleEntitiesComponentCommandWithObject*)header)->Header.EntitiesCount, ((MultipleEntitiesComponentCommandWithObject*)header)->ComponentTypeIndex, in fixedString64Bytes);
				this.playbackProcessor.AddSharedComponentWithValueForMultipleEntities(header);
			}

			// Token: 0x06000589 RID: 1417 RVA: 0x0001EBD0 File Offset: 0x0001CDD0
			[BurstDiscard]
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe void AddSharedComponentWithValueForEntityQuery(BasicCommand* header)
			{
				FixedString64Bytes fixedString64Bytes = "Adding shared";
				Debug.Log(string.Format("{0} component on entity query for component index {1}; recorded from {2}.", fixedString64Bytes, ((EntityQueryComponentCommandWithObject*)header)->Header.ComponentTypeIndex.ToFixedString(), this.originSystemDebugName));
				this.playbackProcessor.AddSharedComponentWithValueForEntityQuery(header);
			}

			// Token: 0x0600058A RID: 1418 RVA: 0x0001EC2C File Offset: 0x0001CE2C
			[BurstDiscard]
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe void SetSharedComponentValueForMultipleEntities(BasicCommand* header)
			{
				FixedString64Bytes fixedString64Bytes = "Setting shared";
				this.LogEntitiesAndComponentCommand(((MultipleEntitiesComponentCommandWithObject*)header)->Header.Entities.Ptr, ((MultipleEntitiesComponentCommandWithObject*)header)->Header.EntitiesCount, ((MultipleEntitiesComponentCommandWithObject*)header)->ComponentTypeIndex, in fixedString64Bytes);
				this.playbackProcessor.SetSharedComponentValueForMultipleEntities(header);
			}

			// Token: 0x0600058B RID: 1419 RVA: 0x0001EC7C File Offset: 0x0001CE7C
			[BurstDiscard]
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe void SetSharedComponentValueForEntityQuery(BasicCommand* header)
			{
				FixedString64Bytes fixedString64Bytes = "Setting shared";
				Debug.Log(string.Format("{0} component on entity query for component index {1}; recorded from {2}.", fixedString64Bytes, ((EntityQueryComponentCommandWithObject*)header)->Header.ComponentTypeIndex.ToFixedString(), this.originSystemDebugName));
				this.playbackProcessor.SetSharedComponentValueForEntityQuery(header);
			}

			// Token: 0x0600058C RID: 1420 RVA: 0x0001ECD8 File Offset: 0x0001CED8
			[BurstDiscard]
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe void SetManagedComponentData(BasicCommand* header)
			{
				FixedString64Bytes fixedString64Bytes = "Setting managed";
				this.LogEntityAndComponentCommand(((EntityManagedComponentCommand*)header)->Header.Entity, ((EntityManagedComponentCommand*)header)->ComponentTypeIndex, in fixedString64Bytes);
				this.playbackProcessor.SetManagedComponentData(header);
			}

			// Token: 0x0600058D RID: 1421 RVA: 0x0001ED18 File Offset: 0x0001CF18
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe void SetUnmanagedSharedComponentData(BasicCommand* header)
			{
				FixedString64Bytes fixedString64Bytes = "Setting unmanaged shared";
				this.LogEntityAndComponentCommand(((EntityUnmanagedSharedComponentCommand*)header)->Header.Entity, ((EntityUnmanagedSharedComponentCommand*)header)->ComponentTypeIndex, in fixedString64Bytes);
				this.playbackProcessor.SetUnmanagedSharedComponentData(header);
			}

			// Token: 0x0600058E RID: 1422 RVA: 0x0001ED58 File Offset: 0x0001CF58
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe void AddUnmanagedSharedComponentValueForMultipleEntities(BasicCommand* header)
			{
				FixedString64Bytes fixedString64Bytes = "Adding unmanaged shared";
				this.LogEntitiesAndComponentCommand(((MultipleEntitiesCommand_WithUnmanagedSharedComponent*)header)->Header.Entities.Ptr, ((MultipleEntitiesCommand_WithUnmanagedSharedComponent*)header)->Header.EntitiesCount, ((MultipleEntitiesCommand_WithUnmanagedSharedComponent*)header)->ComponentTypeIndex, in fixedString64Bytes);
				this.playbackProcessor.AddUnmanagedSharedComponentValueForMultipleEntities(header);
			}

			// Token: 0x0600058F RID: 1423 RVA: 0x0001EDA8 File Offset: 0x0001CFA8
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe void AddUnmanagedSharedComponentValueForEntityQuery(BasicCommand* header)
			{
				FixedString64Bytes fixedString64Bytes = "Adding unmanaged shared";
				Debug.Log(string.Format("{0} component on entity query for component index {1}; recorded from {2}.", fixedString64Bytes, ((EntityQueryComponentCommandWithUnmanagedSharedComponent*)header)->Header.ComponentTypeIndex.ToFixedString(), this.originSystemDebugName));
				this.playbackProcessor.AddUnmanagedSharedComponentValueForEntityQuery(header);
			}

			// Token: 0x06000590 RID: 1424 RVA: 0x0001EE04 File Offset: 0x0001D004
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe void SetUnmanagedSharedComponentValueForMultipleEntities(BasicCommand* header)
			{
				FixedString64Bytes fixedString64Bytes = "Setting unmanaged shared";
				this.LogEntitiesAndComponentCommand(((MultipleEntitiesCommand_WithUnmanagedSharedComponent*)header)->Header.Entities.Ptr, ((MultipleEntitiesCommand_WithUnmanagedSharedComponent*)header)->Header.EntitiesCount, ((MultipleEntitiesCommand_WithUnmanagedSharedComponent*)header)->ComponentTypeIndex, in fixedString64Bytes);
				this.playbackProcessor.SetUnmanagedSharedComponentValueForMultipleEntities(header);
			}

			// Token: 0x06000591 RID: 1425 RVA: 0x0001EE54 File Offset: 0x0001D054
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe void SetUnmanagedSharedComponentValueForEntityQuery(BasicCommand* header)
			{
				FixedString64Bytes fixedString64Bytes = "Setting unmanaged shared";
				Debug.Log(string.Format("{0} component on entity query for component index {1}; recorded from {2}.", fixedString64Bytes, ((EntityQueryComponentCommandWithUnmanagedSharedComponent*)header)->Header.ComponentTypeIndex.ToFixedString(), this.originSystemDebugName));
				this.playbackProcessor.SetUnmanagedSharedComponentValueForEntityQuery(header);
			}

			// Token: 0x17000098 RID: 152
			// (get) Token: 0x06000592 RID: 1426 RVA: 0x0001EEAF File Offset: 0x0001D0AF
			public EntityCommandBuffer.ECBProcessorType ProcessorType
			{
				get
				{
					return EntityCommandBuffer.ECBProcessorType.PlaybackWithTraceProcessor;
				}
			}

			// Token: 0x06000593 RID: 1427 RVA: 0x0001EEB4 File Offset: 0x0001D0B4
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe void SetSharedComponentData(BasicCommand* header)
			{
				FixedString64Bytes fixedString64Bytes = "Setting shared";
				this.LogEntityAndComponentCommand(((EntitySharedComponentCommand*)header)->Header.Entity, ((EntitySharedComponentCommand*)header)->ComponentTypeIndex, in fixedString64Bytes);
				this.playbackProcessor.SetSharedComponentData(header);
			}

			// Token: 0x0400033B RID: 827
			private const string kMsgUnknownSystem = "unknown system type";

			// Token: 0x0400033C RID: 828
			public EntityCommandBuffer.PlaybackProcessor playbackProcessor;

			// Token: 0x0400033D RID: 829
			public unsafe EntityDataAccess* mgr;

			// Token: 0x0400033E RID: 830
			public SystemHandle originSystem;

			// Token: 0x0400033F RID: 831
			public FixedString128Bytes originSystemDebugName;

			// Token: 0x04000340 RID: 832
			public FixedString128Bytes currentSystemDebugName;
		}

		// Token: 0x0200011C RID: 284
		internal struct PrePlaybackValidationProcessor : EntityCommandBuffer.IEcbProcessor
		{
			// Token: 0x06000594 RID: 1428 RVA: 0x0001EEF3 File Offset: 0x0001D0F3
			public unsafe void Init(EntityDataAccess* entityDataAccess, EntityCommandBufferData* data, in SystemHandle originSystemHandle)
			{
				this.playbackProcessor.Init(entityDataAccess, data, in originSystemHandle);
				this.mgr = entityDataAccess;
			}

			// Token: 0x06000595 RID: 1429 RVA: 0x0001EF0A File Offset: 0x0001D10A
			public void Cleanup()
			{
				this.playbackProcessor.Cleanup();
			}

			// Token: 0x06000596 RID: 1430 RVA: 0x0001EF18 File Offset: 0x0001D118
			[Conditional("ENABLE_UNITY_COLLECTIONS_CHECKS")]
			[Conditional("UNITY_DOTS_DEBUG")]
			private unsafe void ThrowIfPrefabComponentInArchetype(EntityArchetype archetype)
			{
				for (int i = 0; i < archetype.TypesCount; i++)
				{
					if (archetype.Types[i].ToComponentType() == ComponentType.ReadWrite<Prefab>())
					{
						throw new InvalidOperationException("Cannot create an entity with a Prefab component in an EntityCommandBuffer.");
					}
				}
			}

			// Token: 0x06000597 RID: 1431 RVA: 0x0001EF64 File Offset: 0x0001D164
			[Conditional("ENABLE_UNITY_COLLECTIONS_CHECKS")]
			[Conditional("UNITY_DOTS_DEBUG")]
			private void ThrowIfPrefabComponent(ComponentType type)
			{
				if (type == ComponentType.ReadWrite<Prefab>())
				{
					throw new InvalidOperationException("Cannot add or remove the Prefab component to an entity within an EntityCommandBuffer.");
				}
			}

			// Token: 0x06000598 RID: 1432 RVA: 0x0001EF80 File Offset: 0x0001D180
			[Conditional("ENABLE_UNITY_COLLECTIONS_CHECKS")]
			[Conditional("UNITY_DOTS_DEBUG")]
			private void ThrowIfPrefabComponentInSet(in ComponentTypeSet typeSet)
			{
				for (int i = 0; i < typeSet.Length; i++)
				{
				}
			}

			// Token: 0x06000599 RID: 1433 RVA: 0x0001EF9E File Offset: 0x0001D19E
			[Conditional("ENABLE_UNITY_COLLECTIONS_CHECKS")]
			[Conditional("UNITY_DOTS_DEBUG")]
			private unsafe void ThrowIfPrefab(Entity e)
			{
				if (this.mgr->HasComponent(e, ComponentType.ReadWrite<Prefab>()))
				{
					throw new InvalidOperationException("Cannot create, destroy, add, or remove components an entity that has the Prefab tag within an EntityCommandBuffer.");
				}
			}

			// Token: 0x0600059A RID: 1434 RVA: 0x0001EFC0 File Offset: 0x0001D1C0
			[Conditional("ENABLE_UNITY_COLLECTIONS_CHECKS")]
			[Conditional("UNITY_DOTS_DEBUG")]
			private unsafe void ThrowIfPrefab(Entity* entities, int entityCount, bool skipDeferredEntityLookup)
			{
				if (skipDeferredEntityLookup)
				{
					for (int i = 0; i < entityCount; i++)
					{
					}
					return;
				}
				for (int j = 0; j < entityCount; j++)
				{
					EntityCommandBuffer.SelectEntity(entities[j], in this.playbackProcessor.playbackState);
				}
			}

			// Token: 0x0600059B RID: 1435 RVA: 0x0001F010 File Offset: 0x0001D210
			[Conditional("ENABLE_UNITY_COLLECTIONS_CHECKS")]
			[Conditional("UNITY_DOTS_DEBUG")]
			private void ThrowIfPrefabInSetOfEntities(NativeArray<Entity> entities)
			{
				for (int i = 0; i < entities.Length; i++)
				{
				}
			}

			// Token: 0x0600059C RID: 1436 RVA: 0x0001F030 File Offset: 0x0001D230
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe void DestroyEntity(BasicCommand* header)
			{
				EntityCommandBuffer.SelectEntity(((EntityCommand*)header)->Entity, in this.playbackProcessor.playbackState);
				this.playbackProcessor.DestroyEntity(header);
			}

			// Token: 0x0600059D RID: 1437 RVA: 0x0001F064 File Offset: 0x0001D264
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe void RemoveComponent(BasicCommand* header)
			{
				EntityCommandBuffer.SelectEntity(((EntityComponentCommand*)header)->Header.Entity, in this.playbackProcessor.playbackState);
				this.playbackProcessor.RemoveComponent(header);
			}

			// Token: 0x0600059E RID: 1438 RVA: 0x0001F09C File Offset: 0x0001D29C
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe void RemoveMultipleComponents(BasicCommand* header)
			{
				EntityCommandBuffer.SelectEntity(((EntityMultipleComponentsCommand*)header)->Header.Entity, in this.playbackProcessor.playbackState);
				EntityMultipleComponentsCommand entityMultipleComponentsCommand = *(EntityMultipleComponentsCommand*)header;
				this.playbackProcessor.RemoveMultipleComponents(header);
			}

			// Token: 0x0600059F RID: 1439 RVA: 0x0001F0DC File Offset: 0x0001D2DC
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe void CreateEntity(BasicCommand* header)
			{
				EntityArchetype entityArchetype = ((CreateCommand*)header)->Archetype;
				if (!entityArchetype.Valid)
				{
					entityArchetype = this.mgr->GetEntityAndSimulateArchetype();
				}
				this.playbackProcessor.CreateEntity(header);
			}

			// Token: 0x060005A0 RID: 1440 RVA: 0x0001F114 File Offset: 0x0001D314
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe void InstantiateEntity(BasicCommand* header)
			{
				this.playbackProcessor.InstantiateEntity(header);
			}

			// Token: 0x060005A1 RID: 1441 RVA: 0x0001F130 File Offset: 0x0001D330
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe void AddComponent(BasicCommand* header)
			{
				ComponentType.FromTypeIndex(((EntityComponentCommand*)header)->ComponentTypeIndex);
				EntityCommandBuffer.SelectEntity(((EntityComponentCommand*)header)->Header.Entity, in this.playbackProcessor.playbackState);
				this.playbackProcessor.AddComponent(header);
			}

			// Token: 0x060005A2 RID: 1442 RVA: 0x0001F174 File Offset: 0x0001D374
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe void AddMultipleComponents(BasicCommand* header)
			{
				EntityCommandBuffer.SelectEntity(((EntityMultipleComponentsCommand*)header)->Header.Entity, in this.playbackProcessor.playbackState);
				this.playbackProcessor.AddMultipleComponents(header);
			}

			// Token: 0x060005A3 RID: 1443 RVA: 0x0001F1AC File Offset: 0x0001D3AC
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe void SetComponent(BasicCommand* header)
			{
				EntityCommandBuffer.SelectEntity(((EntityComponentCommand*)header)->Header.Entity, in this.playbackProcessor.playbackState);
				this.playbackProcessor.SetComponent(header);
			}

			// Token: 0x060005A4 RID: 1444 RVA: 0x0001F1E4 File Offset: 0x0001D3E4
			public unsafe void SetEnabled(BasicCommand* header)
			{
				EntityCommandBuffer.SelectEntity(((EntityEnabledCommand*)header)->Header.Entity, in this.playbackProcessor.playbackState);
				this.playbackProcessor.SetEnabled(header);
			}

			// Token: 0x060005A5 RID: 1445 RVA: 0x0001F21C File Offset: 0x0001D41C
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe void SetComponentEnabled(BasicCommand* header)
			{
				EntityCommandBuffer.SelectEntity(((EntityComponentEnabledCommand*)header)->Header.Header.Entity, in this.playbackProcessor.playbackState);
				this.playbackProcessor.SetComponentEnabled(header);
			}

			// Token: 0x060005A6 RID: 1446 RVA: 0x0001F258 File Offset: 0x0001D458
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe void SetName(BasicCommand* header)
			{
				EntityCommandBuffer.SelectEntity(((EntityNameCommand*)header)->Header.Entity, in this.playbackProcessor.playbackState);
				this.playbackProcessor.SetName(header);
			}

			// Token: 0x060005A7 RID: 1447 RVA: 0x0001F290 File Offset: 0x0001D490
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe void AddBuffer(BasicCommand* header)
			{
				EntityCommandBuffer.SelectEntity(((EntityBufferCommand*)header)->Header.Entity, in this.playbackProcessor.playbackState);
				this.playbackProcessor.AddBuffer(header);
			}

			// Token: 0x060005A8 RID: 1448 RVA: 0x0001F2C8 File Offset: 0x0001D4C8
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe void SetBuffer(BasicCommand* header)
			{
				EntityCommandBuffer.SelectEntity(((EntityBufferCommand*)header)->Header.Entity, in this.playbackProcessor.playbackState);
				this.playbackProcessor.SetBuffer(header);
			}

			// Token: 0x060005A9 RID: 1449 RVA: 0x0001F300 File Offset: 0x0001D500
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe void AppendToBuffer(BasicCommand* header)
			{
				EntityCommandBuffer.SelectEntity(((EntityComponentCommand*)header)->Header.Entity, in this.playbackProcessor.playbackState);
				this.playbackProcessor.AppendToBuffer(header);
			}

			// Token: 0x060005AA RID: 1450 RVA: 0x0001F338 File Offset: 0x0001D538
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe void AddComponentForEntityQuery(BasicCommand* header)
			{
				ComponentType.FromTypeIndex(((EntityQueryComponentCommand*)header)->ComponentTypeIndex);
				this.playbackProcessor.AddComponentForEntityQuery(header);
			}

			// Token: 0x060005AB RID: 1451 RVA: 0x0001F360 File Offset: 0x0001D560
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe void AddComponentForMultipleEntities(BasicCommand* header)
			{
				ComponentType.FromTypeIndex(((MultipleEntitiesComponentCommand*)header)->ComponentTypeIndex);
				this.playbackProcessor.AddComponentForMultipleEntities(header);
			}

			// Token: 0x060005AC RID: 1452 RVA: 0x0001F388 File Offset: 0x0001D588
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe void RemoveComponentForEntityQuery(BasicCommand* header)
			{
				ComponentType.FromTypeIndex(((EntityQueryComponentCommand*)header)->ComponentTypeIndex);
				this.playbackProcessor.RemoveComponentForEntityQuery(header);
			}

			// Token: 0x060005AD RID: 1453 RVA: 0x0001F3B0 File Offset: 0x0001D5B0
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe void RemoveComponentForMultipleEntities(BasicCommand* header)
			{
				ComponentType.FromTypeIndex(((MultipleEntitiesComponentCommand*)header)->ComponentTypeIndex);
				this.playbackProcessor.RemoveComponentForMultipleEntities(header);
			}

			// Token: 0x060005AE RID: 1454 RVA: 0x0001F3D8 File Offset: 0x0001D5D8
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe void AddMultipleComponentsForMultipleEntities(BasicCommand* header)
			{
				this.playbackProcessor.AddMultipleComponentsForMultipleEntities(header);
			}

			// Token: 0x060005AF RID: 1455 RVA: 0x0001F3F4 File Offset: 0x0001D5F4
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe void AddMultipleComponentsForEntityQuery(BasicCommand* header)
			{
				this.playbackProcessor.AddMultipleComponentsForEntityQuery(header);
			}

			// Token: 0x060005B0 RID: 1456 RVA: 0x0001F410 File Offset: 0x0001D610
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe void RemoveMultipleComponentsForMultipleEntities(BasicCommand* header)
			{
				this.playbackProcessor.RemoveMultipleComponentsForMultipleEntities(header);
			}

			// Token: 0x060005B1 RID: 1457 RVA: 0x0001F42C File Offset: 0x0001D62C
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe void RemoveMultipleComponentsForEntityQuery(BasicCommand* header)
			{
				this.playbackProcessor.RemoveMultipleComponentsForEntityQuery(header);
			}

			// Token: 0x060005B2 RID: 1458 RVA: 0x0001F448 File Offset: 0x0001D648
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe void DestroyMultipleEntities(BasicCommand* header)
			{
				this.playbackProcessor.DestroyMultipleEntities(header);
			}

			// Token: 0x060005B3 RID: 1459 RVA: 0x0001F464 File Offset: 0x0001D664
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe void DestroyForEntityQuery(BasicCommand* header)
			{
				this.playbackProcessor.DestroyForEntityQuery(header);
			}

			// Token: 0x060005B4 RID: 1460 RVA: 0x0001F480 File Offset: 0x0001D680
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe void AddComponentLinkedEntityGroup(BasicCommand* header)
			{
				EntityCommandBuffer.SelectEntity(((EntityQueryMaskCommand*)header)->Header.Header.Entity, in this.playbackProcessor.playbackState);
				this.playbackProcessor.AddComponentLinkedEntityGroup(header);
			}

			// Token: 0x060005B5 RID: 1461 RVA: 0x0001F4BC File Offset: 0x0001D6BC
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe void SetComponentLinkedEntityGroup(BasicCommand* header)
			{
				EntityCommandBuffer.SelectEntity(((EntityQueryMaskCommand*)header)->Header.Header.Entity, in this.playbackProcessor.playbackState);
				this.playbackProcessor.SetComponentLinkedEntityGroup(header);
			}

			// Token: 0x060005B6 RID: 1462 RVA: 0x0001F4F8 File Offset: 0x0001D6F8
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe void ReplaceComponentLinkedEntityGroup(BasicCommand* header)
			{
				EntityCommandBuffer.SelectEntity(((EntityComponentCommand*)header)->Header.Entity, in this.playbackProcessor.playbackState);
				this.playbackProcessor.ReplaceComponentLinkedEntityGroup(header);
			}

			// Token: 0x060005B7 RID: 1463 RVA: 0x0001F530 File Offset: 0x0001D730
			[BurstDiscard]
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe void AddManagedComponentData(BasicCommand* header)
			{
				EntityCommandBuffer.SelectEntity(((EntityManagedComponentCommand*)header)->Header.Entity, in this.playbackProcessor.playbackState);
				this.playbackProcessor.AddManagedComponentData(header);
			}

			// Token: 0x060005B8 RID: 1464 RVA: 0x0001F568 File Offset: 0x0001D768
			[BurstDiscard]
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe void MoveManagedComponentData(BasicCommand* header)
			{
				EntityCommandBuffer.SelectEntity(((EntityMoveManagedComponentCommand*)header)->Header.Entity, in this.playbackProcessor.playbackState);
				EntityCommandBuffer.SelectEntity(((EntityMoveManagedComponentCommand*)header)->SrcEntity, in this.playbackProcessor.playbackState);
				this.playbackProcessor.MoveManagedComponentData(header);
			}

			// Token: 0x060005B9 RID: 1465 RVA: 0x0001F5B8 File Offset: 0x0001D7B8
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe void AddUnmanagedSharedComponentData(BasicCommand* header)
			{
				EntityCommandBuffer.SelectEntity(((EntityUnmanagedSharedComponentCommand*)header)->Header.Entity, in this.playbackProcessor.playbackState);
				this.playbackProcessor.AddUnmanagedSharedComponentData(header);
			}

			// Token: 0x060005BA RID: 1466 RVA: 0x0001F5F0 File Offset: 0x0001D7F0
			[BurstDiscard]
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe void AddSharedComponentData(BasicCommand* header)
			{
				EntityCommandBuffer.SelectEntity(((EntitySharedComponentCommand*)header)->Header.Entity, in this.playbackProcessor.playbackState);
				this.playbackProcessor.AddSharedComponentData(header);
			}

			// Token: 0x060005BB RID: 1467 RVA: 0x0001F628 File Offset: 0x0001D828
			[BurstDiscard]
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe void AddComponentObjectForMultipleEntities(BasicCommand* header)
			{
				ComponentType.FromTypeIndex(((MultipleEntitiesComponentCommandWithObject*)header)->ComponentTypeIndex);
				this.playbackProcessor.AddComponentObjectForMultipleEntities(header);
			}

			// Token: 0x060005BC RID: 1468 RVA: 0x0001F650 File Offset: 0x0001D850
			[BurstDiscard]
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe void SetComponentObjectForMultipleEntities(BasicCommand* header)
			{
				this.playbackProcessor.SetComponentObjectForMultipleEntities(header);
			}

			// Token: 0x060005BD RID: 1469 RVA: 0x0001F66C File Offset: 0x0001D86C
			[BurstDiscard]
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe void AddSharedComponentWithValueForMultipleEntities(BasicCommand* header)
			{
				this.playbackProcessor.AddSharedComponentWithValueForMultipleEntities(header);
			}

			// Token: 0x060005BE RID: 1470 RVA: 0x0001F688 File Offset: 0x0001D888
			[BurstDiscard]
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe void AddSharedComponentWithValueForEntityQuery(BasicCommand* header)
			{
				this.playbackProcessor.AddSharedComponentWithValueForEntityQuery(header);
			}

			// Token: 0x060005BF RID: 1471 RVA: 0x0001F6A4 File Offset: 0x0001D8A4
			[BurstDiscard]
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe void SetSharedComponentValueForMultipleEntities(BasicCommand* header)
			{
				this.playbackProcessor.SetSharedComponentValueForMultipleEntities(header);
			}

			// Token: 0x060005C0 RID: 1472 RVA: 0x0001F6C0 File Offset: 0x0001D8C0
			[BurstDiscard]
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe void SetSharedComponentValueForEntityQuery(BasicCommand* header)
			{
				this.playbackProcessor.SetSharedComponentValueForEntityQuery(header);
			}

			// Token: 0x060005C1 RID: 1473 RVA: 0x0001F6DC File Offset: 0x0001D8DC
			[BurstDiscard]
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe void SetManagedComponentData(BasicCommand* header)
			{
				EntityCommandBuffer.SelectEntity(((EntityManagedComponentCommand*)header)->Header.Entity, in this.playbackProcessor.playbackState);
				this.playbackProcessor.SetManagedComponentData(header);
			}

			// Token: 0x060005C2 RID: 1474 RVA: 0x0001F714 File Offset: 0x0001D914
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe void SetUnmanagedSharedComponentData(BasicCommand* header)
			{
				EntityCommandBuffer.SelectEntity(((EntityUnmanagedSharedComponentCommand*)header)->Header.Entity, in this.playbackProcessor.playbackState);
				this.playbackProcessor.SetUnmanagedSharedComponentData(header);
			}

			// Token: 0x060005C3 RID: 1475 RVA: 0x0001F74C File Offset: 0x0001D94C
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe void AddUnmanagedSharedComponentValueForMultipleEntities(BasicCommand* header)
			{
				this.playbackProcessor.AddUnmanagedSharedComponentValueForMultipleEntities(header);
			}

			// Token: 0x060005C4 RID: 1476 RVA: 0x0001F768 File Offset: 0x0001D968
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe void AddUnmanagedSharedComponentValueForEntityQuery(BasicCommand* header)
			{
				this.playbackProcessor.AddUnmanagedSharedComponentValueForEntityQuery(header);
			}

			// Token: 0x060005C5 RID: 1477 RVA: 0x0001F784 File Offset: 0x0001D984
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe void SetUnmanagedSharedComponentValueForMultipleEntities(BasicCommand* header)
			{
				this.playbackProcessor.SetUnmanagedSharedComponentValueForMultipleEntities(header);
			}

			// Token: 0x060005C6 RID: 1478 RVA: 0x0001F7A0 File Offset: 0x0001D9A0
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe void SetUnmanagedSharedComponentValueForEntityQuery(BasicCommand* header)
			{
				this.playbackProcessor.SetUnmanagedSharedComponentValueForEntityQuery(header);
			}

			// Token: 0x17000099 RID: 153
			// (get) Token: 0x060005C7 RID: 1479 RVA: 0x0001F7BB File Offset: 0x0001D9BB
			public EntityCommandBuffer.ECBProcessorType ProcessorType
			{
				get
				{
					return EntityCommandBuffer.ECBProcessorType.PrePlaybackValidationProcessor;
				}
			}

			// Token: 0x060005C8 RID: 1480 RVA: 0x0001F7C0 File Offset: 0x0001D9C0
			public unsafe void SetSharedComponentData(BasicCommand* header)
			{
				EntityCommandBuffer.SelectEntity(((EntitySharedComponentCommand*)header)->Header.Entity, in this.playbackProcessor.playbackState);
				this.playbackProcessor.SetSharedComponentData(header);
			}

			// Token: 0x04000341 RID: 833
			private const string kMsgUnknownSystem = "unknown system type";

			// Token: 0x04000342 RID: 834
			public EntityCommandBuffer.PlaybackProcessor playbackProcessor;

			// Token: 0x04000343 RID: 835
			public unsafe EntityDataAccess* mgr;
		}

		// Token: 0x0200011D RID: 285
		internal struct DebugViewProcessor : EntityCommandBuffer.IEcbProcessor
		{
			// Token: 0x060005C9 RID: 1481 RVA: 0x0001F7F7 File Offset: 0x0001D9F7
			public void Init(AllocatorManager.AllocatorHandle allocator)
			{
				this.commands = new UnsafePtrList<BasicCommand>(8, allocator, NativeArrayOptions.UninitializedMemory);
			}

			// Token: 0x060005CA RID: 1482 RVA: 0x0001F807 File Offset: 0x0001DA07
			public void Cleanup()
			{
				this.commands.Dispose();
			}

			// Token: 0x060005CB RID: 1483 RVA: 0x0001F814 File Offset: 0x0001DA14
			public unsafe void DestroyEntity(BasicCommand* header)
			{
				this.commands.Add((void*)header);
			}

			// Token: 0x060005CC RID: 1484 RVA: 0x0001F814 File Offset: 0x0001DA14
			public unsafe void RemoveComponent(BasicCommand* header)
			{
				this.commands.Add((void*)header);
			}

			// Token: 0x060005CD RID: 1485 RVA: 0x0001F814 File Offset: 0x0001DA14
			public unsafe void RemoveMultipleComponents(BasicCommand* header)
			{
				this.commands.Add((void*)header);
			}

			// Token: 0x060005CE RID: 1486 RVA: 0x0001F814 File Offset: 0x0001DA14
			public unsafe void CreateEntity(BasicCommand* header)
			{
				this.commands.Add((void*)header);
			}

			// Token: 0x060005CF RID: 1487 RVA: 0x0001F814 File Offset: 0x0001DA14
			public unsafe void InstantiateEntity(BasicCommand* header)
			{
				this.commands.Add((void*)header);
			}

			// Token: 0x060005D0 RID: 1488 RVA: 0x0001F814 File Offset: 0x0001DA14
			public unsafe void AddComponent(BasicCommand* header)
			{
				this.commands.Add((void*)header);
			}

			// Token: 0x060005D1 RID: 1489 RVA: 0x0001F814 File Offset: 0x0001DA14
			public unsafe void AddMultipleComponents(BasicCommand* header)
			{
				this.commands.Add((void*)header);
			}

			// Token: 0x060005D2 RID: 1490 RVA: 0x0001F814 File Offset: 0x0001DA14
			public unsafe void SetComponent(BasicCommand* header)
			{
				this.commands.Add((void*)header);
			}

			// Token: 0x060005D3 RID: 1491 RVA: 0x0001F814 File Offset: 0x0001DA14
			public unsafe void SetEnabled(BasicCommand* header)
			{
				this.commands.Add((void*)header);
			}

			// Token: 0x060005D4 RID: 1492 RVA: 0x0001F814 File Offset: 0x0001DA14
			public unsafe void SetComponentEnabled(BasicCommand* header)
			{
				this.commands.Add((void*)header);
			}

			// Token: 0x060005D5 RID: 1493 RVA: 0x0001F814 File Offset: 0x0001DA14
			public unsafe void SetName(BasicCommand* header)
			{
				this.commands.Add((void*)header);
			}

			// Token: 0x060005D6 RID: 1494 RVA: 0x0001F814 File Offset: 0x0001DA14
			public unsafe void AddBuffer(BasicCommand* header)
			{
				this.commands.Add((void*)header);
			}

			// Token: 0x060005D7 RID: 1495 RVA: 0x0001F814 File Offset: 0x0001DA14
			public unsafe void SetBuffer(BasicCommand* header)
			{
				this.commands.Add((void*)header);
			}

			// Token: 0x060005D8 RID: 1496 RVA: 0x0001F814 File Offset: 0x0001DA14
			public unsafe void AppendToBuffer(BasicCommand* header)
			{
				this.commands.Add((void*)header);
			}

			// Token: 0x060005D9 RID: 1497 RVA: 0x0001F814 File Offset: 0x0001DA14
			public unsafe void AddComponentForEntityQuery(BasicCommand* header)
			{
				this.commands.Add((void*)header);
			}

			// Token: 0x060005DA RID: 1498 RVA: 0x0001F814 File Offset: 0x0001DA14
			public unsafe void AddComponentForMultipleEntities(BasicCommand* header)
			{
				this.commands.Add((void*)header);
			}

			// Token: 0x060005DB RID: 1499 RVA: 0x0001F814 File Offset: 0x0001DA14
			public unsafe void RemoveComponentForEntityQuery(BasicCommand* header)
			{
				this.commands.Add((void*)header);
			}

			// Token: 0x060005DC RID: 1500 RVA: 0x0001F814 File Offset: 0x0001DA14
			public unsafe void RemoveComponentForMultipleEntities(BasicCommand* header)
			{
				this.commands.Add((void*)header);
			}

			// Token: 0x060005DD RID: 1501 RVA: 0x0001F814 File Offset: 0x0001DA14
			public unsafe void AddMultipleComponentsForMultipleEntities(BasicCommand* header)
			{
				this.commands.Add((void*)header);
			}

			// Token: 0x060005DE RID: 1502 RVA: 0x0001F814 File Offset: 0x0001DA14
			public unsafe void AddMultipleComponentsForEntityQuery(BasicCommand* header)
			{
				this.commands.Add((void*)header);
			}

			// Token: 0x060005DF RID: 1503 RVA: 0x0001F814 File Offset: 0x0001DA14
			public unsafe void RemoveMultipleComponentsForMultipleEntities(BasicCommand* header)
			{
				this.commands.Add((void*)header);
			}

			// Token: 0x060005E0 RID: 1504 RVA: 0x0001F814 File Offset: 0x0001DA14
			public unsafe void RemoveMultipleComponentsForEntityQuery(BasicCommand* header)
			{
				this.commands.Add((void*)header);
			}

			// Token: 0x060005E1 RID: 1505 RVA: 0x0001F814 File Offset: 0x0001DA14
			public unsafe void DestroyMultipleEntities(BasicCommand* header)
			{
				this.commands.Add((void*)header);
			}

			// Token: 0x060005E2 RID: 1506 RVA: 0x0001F814 File Offset: 0x0001DA14
			public unsafe void DestroyForEntityQuery(BasicCommand* header)
			{
				this.commands.Add((void*)header);
			}

			// Token: 0x060005E3 RID: 1507 RVA: 0x0001F814 File Offset: 0x0001DA14
			public unsafe void AddComponentLinkedEntityGroup(BasicCommand* header)
			{
				this.commands.Add((void*)header);
			}

			// Token: 0x060005E4 RID: 1508 RVA: 0x0001F814 File Offset: 0x0001DA14
			public unsafe void SetComponentLinkedEntityGroup(BasicCommand* header)
			{
				this.commands.Add((void*)header);
			}

			// Token: 0x060005E5 RID: 1509 RVA: 0x0001F814 File Offset: 0x0001DA14
			public unsafe void ReplaceComponentLinkedEntityGroup(BasicCommand* header)
			{
				this.commands.Add((void*)header);
			}

			// Token: 0x060005E6 RID: 1510 RVA: 0x0001F814 File Offset: 0x0001DA14
			public unsafe void AddManagedComponentData(BasicCommand* header)
			{
				this.commands.Add((void*)header);
			}

			// Token: 0x060005E7 RID: 1511 RVA: 0x0001F814 File Offset: 0x0001DA14
			public unsafe void MoveManagedComponentData(BasicCommand* header)
			{
				this.commands.Add((void*)header);
			}

			// Token: 0x060005E8 RID: 1512 RVA: 0x0001F814 File Offset: 0x0001DA14
			public unsafe void AddSharedComponentData(BasicCommand* header)
			{
				this.commands.Add((void*)header);
			}

			// Token: 0x060005E9 RID: 1513 RVA: 0x0001F814 File Offset: 0x0001DA14
			public unsafe void AddComponentObjectForMultipleEntities(BasicCommand* header)
			{
				this.commands.Add((void*)header);
			}

			// Token: 0x060005EA RID: 1514 RVA: 0x0001F814 File Offset: 0x0001DA14
			public unsafe void SetComponentObjectForMultipleEntities(BasicCommand* header)
			{
				this.commands.Add((void*)header);
			}

			// Token: 0x060005EB RID: 1515 RVA: 0x0001F814 File Offset: 0x0001DA14
			public unsafe void AddSharedComponentWithValueForMultipleEntities(BasicCommand* header)
			{
				this.commands.Add((void*)header);
			}

			// Token: 0x060005EC RID: 1516 RVA: 0x0001F814 File Offset: 0x0001DA14
			public unsafe void AddSharedComponentWithValueForEntityQuery(BasicCommand* header)
			{
				this.commands.Add((void*)header);
			}

			// Token: 0x060005ED RID: 1517 RVA: 0x0001F814 File Offset: 0x0001DA14
			public unsafe void SetSharedComponentValueForMultipleEntities(BasicCommand* header)
			{
				this.commands.Add((void*)header);
			}

			// Token: 0x060005EE RID: 1518 RVA: 0x0001F814 File Offset: 0x0001DA14
			public unsafe void SetSharedComponentValueForEntityQuery(BasicCommand* header)
			{
				this.commands.Add((void*)header);
			}

			// Token: 0x060005EF RID: 1519 RVA: 0x0001F814 File Offset: 0x0001DA14
			public unsafe void SetManagedComponentData(BasicCommand* header)
			{
				this.commands.Add((void*)header);
			}

			// Token: 0x060005F0 RID: 1520 RVA: 0x0001F814 File Offset: 0x0001DA14
			public unsafe void SetSharedComponentData(BasicCommand* header)
			{
				this.commands.Add((void*)header);
			}

			// Token: 0x060005F1 RID: 1521 RVA: 0x0001F814 File Offset: 0x0001DA14
			public unsafe void AddUnmanagedSharedComponentData(BasicCommand* header)
			{
				this.commands.Add((void*)header);
			}

			// Token: 0x060005F2 RID: 1522 RVA: 0x0001F814 File Offset: 0x0001DA14
			public unsafe void SetUnmanagedSharedComponentData(BasicCommand* header)
			{
				this.commands.Add((void*)header);
			}

			// Token: 0x060005F3 RID: 1523 RVA: 0x0001F814 File Offset: 0x0001DA14
			public unsafe void AddUnmanagedSharedComponentValueForMultipleEntities(BasicCommand* header)
			{
				this.commands.Add((void*)header);
			}

			// Token: 0x060005F4 RID: 1524 RVA: 0x0001F814 File Offset: 0x0001DA14
			public unsafe void AddUnmanagedSharedComponentValueForEntityQuery(BasicCommand* header)
			{
				this.commands.Add((void*)header);
			}

			// Token: 0x060005F5 RID: 1525 RVA: 0x0001F814 File Offset: 0x0001DA14
			public unsafe void SetUnmanagedSharedComponentValueForMultipleEntities(BasicCommand* header)
			{
				this.commands.Add((void*)header);
			}

			// Token: 0x060005F6 RID: 1526 RVA: 0x0001F814 File Offset: 0x0001DA14
			public unsafe void SetUnmanagedSharedComponentValueForEntityQuery(BasicCommand* header)
			{
				this.commands.Add((void*)header);
			}

			// Token: 0x1700009A RID: 154
			// (get) Token: 0x060005F7 RID: 1527 RVA: 0x000106DF File Offset: 0x0000E8DF
			public EntityCommandBuffer.ECBProcessorType ProcessorType
			{
				get
				{
					return EntityCommandBuffer.ECBProcessorType.DebugViewProcessor;
				}
			}

			// Token: 0x04000344 RID: 836
			public UnsafePtrList<BasicCommand> commands;
		}

		// Token: 0x0200011E RID: 286
		internal class BasicCommandView
		{
			// Token: 0x060005F8 RID: 1528 RVA: 0x0001F822 File Offset: 0x0001DA22
			public BasicCommandView()
			{
				this.CommandType = ECBCommand.InstantiateEntity;
				this.SortKey = int.MinValue;
				this.TotalSizeInBytes = 0;
			}

			// Token: 0x060005F9 RID: 1529 RVA: 0x0001F843 File Offset: 0x0001DA43
			public BasicCommandView(ECBCommand commandType, int sortKey, int totalSize)
			{
				this.CommandType = commandType;
				this.SortKey = sortKey;
				this.TotalSizeInBytes = totalSize;
			}

			// Token: 0x04000345 RID: 837
			public ECBCommand CommandType;

			// Token: 0x04000346 RID: 838
			public int SortKey;

			// Token: 0x04000347 RID: 839
			public int TotalSizeInBytes;
		}

		// Token: 0x0200011F RID: 287
		internal class CreateCommandView : EntityCommandBuffer.BasicCommandView
		{
			// Token: 0x060005FA RID: 1530 RVA: 0x0001F860 File Offset: 0x0001DA60
			public unsafe CreateCommandView(CreateCommand* cmd)
			{
				this.CommandType = cmd->Header.CommandType;
				this.SortKey = cmd->Header.SortKey;
				this.TotalSizeInBytes = cmd->Header.TotalSize;
				this.EntityArchetype = cmd->Archetype;
				this.EntityIdentityIndex = cmd->IdentityIndex;
				this.BatchCount = cmd->BatchCount;
			}

			// Token: 0x060005FB RID: 1531 RVA: 0x0001F8CA File Offset: 0x0001DACA
			public override string ToString()
			{
				return "Create Entity";
			}

			// Token: 0x04000348 RID: 840
			public EntityArchetype EntityArchetype;

			// Token: 0x04000349 RID: 841
			public int EntityIdentityIndex;

			// Token: 0x0400034A RID: 842
			public int BatchCount;
		}

		// Token: 0x02000120 RID: 288
		internal class EntityCommandView : EntityCommandBuffer.BasicCommandView
		{
			// Token: 0x060005FC RID: 1532 RVA: 0x0001F8D1 File Offset: 0x0001DAD1
			public EntityCommandView()
			{
				this.Entity = Entity.Null;
				this.IdentityIndex = int.MinValue;
				this.BatchCount = 0;
			}

			// Token: 0x060005FD RID: 1533 RVA: 0x0001F8F8 File Offset: 0x0001DAF8
			public unsafe EntityCommandView(EntityCommand* cmd)
			{
				this.CommandType = cmd->Header.CommandType;
				this.SortKey = cmd->Header.SortKey;
				this.TotalSizeInBytes = cmd->Header.TotalSize;
				this.Entity = cmd->Entity;
				this.IdentityIndex = cmd->IdentityIndex;
				this.BatchCount = cmd->BatchCount;
			}

			// Token: 0x060005FE RID: 1534 RVA: 0x0001F962 File Offset: 0x0001DB62
			public override string ToString()
			{
				if (this.CommandType != ECBCommand.InstantiateEntity)
				{
					return "Destroy Entity";
				}
				return string.Format("Instantiate Entity (count={0})", this.BatchCount);
			}

			// Token: 0x0400034B RID: 843
			public Entity Entity;

			// Token: 0x0400034C RID: 844
			public int IdentityIndex;

			// Token: 0x0400034D RID: 845
			public int BatchCount;
		}

		// Token: 0x02000121 RID: 289
		internal class EntityQueryCommandView : EntityCommandBuffer.BasicCommandView
		{
			// Token: 0x060005FF RID: 1535 RVA: 0x0001F987 File Offset: 0x0001DB87
			public EntityQueryCommandView()
			{
				this.CommandType = ECBCommand.InstantiateEntity;
				this.SortKey = 0;
				this.TotalSizeInBytes = 0;
				this.Query = null;
			}

			// Token: 0x06000600 RID: 1536 RVA: 0x0001F9AC File Offset: 0x0001DBAC
			public unsafe EntityQueryCommandView(EntityQueryCommand* cmd)
			{
				this.CommandType = cmd->Header.CommandType;
				this.SortKey = cmd->Header.SortKey;
				this.TotalSizeInBytes = cmd->Header.TotalSize;
				this.Query = cmd->QueryImpl;
			}

			// Token: 0x0400034E RID: 846
			public unsafe EntityQueryImpl* Query;
		}

		// Token: 0x02000122 RID: 290
		internal class EntityQueryComponentCommandView : EntityCommandBuffer.EntityQueryCommandView
		{
			// Token: 0x06000601 RID: 1537 RVA: 0x0001F9FE File Offset: 0x0001DBFE
			public EntityQueryComponentCommandView()
			{
				this.CommandType = ECBCommand.InstantiateEntity;
				this.SortKey = 0;
				this.TotalSizeInBytes = 0;
				this.Query = null;
				this.ComponentTypeIndex = 0;
			}

			// Token: 0x06000602 RID: 1538 RVA: 0x0001FA30 File Offset: 0x0001DC30
			public unsafe EntityQueryComponentCommandView(EntityQueryComponentCommand* cmd)
			{
				this.CommandType = cmd->Header.Header.CommandType;
				this.SortKey = cmd->Header.Header.SortKey;
				this.TotalSizeInBytes = cmd->Header.Header.TotalSize;
				this.Query = cmd->Header.QueryImpl;
				this.ComponentTypeIndex = cmd->ComponentTypeIndex;
			}

			// Token: 0x06000603 RID: 1539 RVA: 0x0001FAA4 File Offset: 0x0001DCA4
			public override string ToString()
			{
				string text = TypeManager.GetType(this.ComponentTypeIndex).Name + " ";
				if (this.CommandType != ECBCommand.AddComponentForEntityQuery)
				{
					return "Remove " + text + "Component from EntityQuery";
				}
				return "Add " + text + "Component to EntityQuery";
			}

			// Token: 0x0400034F RID: 847
			public TypeIndex ComponentTypeIndex;
		}

		// Token: 0x02000123 RID: 291
		internal class EntityQueryComponentTypeSetCommandView : EntityCommandBuffer.EntityQueryCommandView
		{
			// Token: 0x06000604 RID: 1540 RVA: 0x0001FAF8 File Offset: 0x0001DCF8
			public unsafe EntityQueryComponentTypeSetCommandView(EntityQueryComponentTypeSetCommand* cmd)
			{
				this.CommandType = cmd->Header.Header.CommandType;
				this.SortKey = cmd->Header.Header.SortKey;
				this.TotalSizeInBytes = cmd->Header.Header.TotalSize;
				this.Query = cmd->Header.QueryImpl;
				this.TypeSet = cmd->TypeSet;
			}

			// Token: 0x06000605 RID: 1541 RVA: 0x0001FB6A File Offset: 0x0001DD6A
			public override string ToString()
			{
				if (this.CommandType != ECBCommand.AddComponentForEntityQuery)
				{
					return string.Format("Remove {0} from EntityQuery", this.TypeSet);
				}
				return string.Format("Add {0} to EntityQuery", this.TypeSet);
			}

			// Token: 0x04000350 RID: 848
			public ComponentTypeSet TypeSet;
		}

		// Token: 0x02000124 RID: 292
		internal class MultipleEntitiesCommandView : EntityCommandBuffer.BasicCommandView
		{
			// Token: 0x06000606 RID: 1542 RVA: 0x0001FBA1 File Offset: 0x0001DDA1
			public MultipleEntitiesCommandView()
			{
				this.Entities = default(EntityNode);
				this.EntitiesCount = 0;
				this.Allocator = Unity.Collections.Allocator.Invalid;
				this.SkipDeferredEntityLookup = false;
			}

			// Token: 0x06000607 RID: 1543 RVA: 0x0001FBD0 File Offset: 0x0001DDD0
			public unsafe MultipleEntitiesCommandView(MultipleEntitiesCommand* cmd)
			{
				this.CommandType = cmd->Header.CommandType;
				this.SortKey = cmd->Header.SortKey;
				this.TotalSizeInBytes = cmd->Header.TotalSize;
				this.Entities = cmd->Entities;
				this.EntitiesCount = cmd->EntitiesCount;
				this.SkipDeferredEntityLookup = cmd->SkipDeferredEntityLookup != 0;
				this.Allocator = cmd->Allocator.ToAllocator;
			}

			// Token: 0x06000608 RID: 1544 RVA: 0x0001FC53 File Offset: 0x0001DE53
			public override string ToString()
			{
				if (this.CommandType != ECBCommand.CreateEntity)
				{
					return string.Format("Destroy {0} Entities", this.EntitiesCount);
				}
				return string.Format("Instantiate {0} Entities", this.EntitiesCount);
			}

			// Token: 0x04000351 RID: 849
			public EntityNode Entities;

			// Token: 0x04000352 RID: 850
			public int EntitiesCount;

			// Token: 0x04000353 RID: 851
			public bool SkipDeferredEntityLookup;

			// Token: 0x04000354 RID: 852
			public AllocatorManager.AllocatorHandle Allocator;
		}

		// Token: 0x02000125 RID: 293
		internal class MultipleEntitiesComponentCommandView : EntityCommandBuffer.MultipleEntitiesCommandView
		{
			// Token: 0x06000609 RID: 1545 RVA: 0x0001FC8C File Offset: 0x0001DE8C
			public unsafe MultipleEntitiesComponentCommandView(MultipleEntitiesComponentCommand* cmd, byte* componentValue)
			{
				this.CommandType = cmd->Header.Header.CommandType;
				this.SortKey = cmd->Header.Header.SortKey;
				this.TotalSizeInBytes = cmd->Header.Header.TotalSize;
				this.Entities = cmd->Header.Entities;
				this.EntitiesCount = cmd->Header.EntitiesCount;
				this.SkipDeferredEntityLookup = cmd->Header.SkipDeferredEntityLookup != 0;
				this.Allocator = cmd->Header.Allocator.ToAllocator;
				this.ComponentTypeIndex = cmd->ComponentTypeIndex;
				this.ComponentSize = cmd->ComponentSize;
				this.ValueRequiresEntityFixup = cmd->ValueRequiresEntityFixup > 0;
				if (this.ComponentSize > 0 && componentValue != null)
				{
					this.ComponentValue = TypeManager.ConstructComponentFromBuffer(this.ComponentTypeIndex, (void*)componentValue);
					return;
				}
				this.ComponentValue = null;
			}

			// Token: 0x0600060A RID: 1546 RVA: 0x0001FD84 File Offset: 0x0001DF84
			public override string ToString()
			{
				string text = TypeManager.GetType(this.ComponentTypeIndex).Name + " ";
				if (this.CommandType != ECBCommand.AddComponentForMultipleEntities)
				{
					return string.Format("Remove {0}Component from {1} Entities", text, this.EntitiesCount);
				}
				return string.Format("Add {0}Component to {1} Entities", text, this.EntitiesCount);
			}

			// Token: 0x04000355 RID: 853
			public TypeIndex ComponentTypeIndex;

			// Token: 0x04000356 RID: 854
			public short ComponentSize;

			// Token: 0x04000357 RID: 855
			public object ComponentValue;

			// Token: 0x04000358 RID: 856
			public bool ValueRequiresEntityFixup;
		}

		// Token: 0x02000126 RID: 294
		internal class MultipleEntitiesComponentCommandWithObjectView : EntityCommandBuffer.MultipleEntitiesCommandView
		{
			// Token: 0x0600060B RID: 1547 RVA: 0x0001FDE3 File Offset: 0x0001DFE3
			internal object GetBoxedObject()
			{
				if (this.GCNode.BoxedObject.IsAllocated)
				{
					return this.GCNode.BoxedObject.Target;
				}
				return null;
			}

			// Token: 0x0600060C RID: 1548 RVA: 0x0001FE0C File Offset: 0x0001E00C
			public unsafe MultipleEntitiesComponentCommandWithObjectView(MultipleEntitiesComponentCommandWithObject* cmd)
			{
				this.CommandType = cmd->Header.Header.CommandType;
				this.SortKey = cmd->Header.Header.SortKey;
				this.TotalSizeInBytes = cmd->Header.Header.TotalSize;
				this.Entities = cmd->Header.Entities;
				this.EntitiesCount = cmd->Header.EntitiesCount;
				this.SkipDeferredEntityLookup = cmd->Header.SkipDeferredEntityLookup != 0;
				this.Allocator = cmd->Header.Allocator.ToAllocator;
				this.ComponentTypeIndex = cmd->ComponentTypeIndex;
				this.HashCode = cmd->HashCode;
				this.GCNode = cmd->GCNode;
			}

			// Token: 0x0600060D RID: 1549 RVA: 0x0001FED8 File Offset: 0x0001E0D8
			public override string ToString()
			{
				string text = TypeManager.GetType(this.ComponentTypeIndex).Name + " ";
				if (this.CommandType != ECBCommand.AddComponentObjectForMultipleEntities && this.CommandType != ECBCommand.AddSharedComponentWithValueForMultipleEntities)
				{
					return string.Format("Set {0}Component to {1} Entities", text, this.EntitiesCount);
				}
				return string.Format("Add {0}Component to {1} Entities", text, this.EntitiesCount);
			}

			// Token: 0x04000359 RID: 857
			public TypeIndex ComponentTypeIndex;

			// Token: 0x0400035A RID: 858
			public int HashCode;

			// Token: 0x0400035B RID: 859
			public EntityComponentGCNode GCNode;
		}

		// Token: 0x02000127 RID: 295
		internal class EntityQueryComponentCommandWithObjectView : EntityCommandBuffer.EntityQueryComponentCommandView
		{
			// Token: 0x0600060E RID: 1550 RVA: 0x0001FF41 File Offset: 0x0001E141
			internal object GetBoxedObject()
			{
				if (this.GCNode.BoxedObject.IsAllocated)
				{
					return this.GCNode.BoxedObject.Target;
				}
				return null;
			}

			// Token: 0x0600060F RID: 1551 RVA: 0x0001FF68 File Offset: 0x0001E168
			public unsafe EntityQueryComponentCommandWithObjectView(EntityQueryComponentCommandWithObject* cmd)
			{
				this.CommandType = cmd->Header.Header.Header.CommandType;
				this.SortKey = cmd->Header.Header.Header.SortKey;
				this.TotalSizeInBytes = cmd->Header.Header.Header.TotalSize;
				this.Query = cmd->Header.Header.QueryImpl;
				this.ComponentTypeIndex = cmd->Header.ComponentTypeIndex;
				this.HashCode = cmd->HashCode;
				this.GCNode = cmd->GCNode;
			}

			// Token: 0x06000610 RID: 1552 RVA: 0x0002000C File Offset: 0x0001E20C
			public override string ToString()
			{
				string text = TypeManager.GetType(this.ComponentTypeIndex).Name + " ";
				if (this.CommandType != ECBCommand.AddSharedComponentWithValueForEntityQuery && this.CommandType != ECBCommand.AddComponentObjectForEntityQuery)
				{
					return "Set " + text + "Component to EntityQuery";
				}
				return "Add " + text + "Component to EntityQuery";
			}

			// Token: 0x0400035C RID: 860
			public int HashCode;

			// Token: 0x0400035D RID: 861
			public EntityComponentGCNode GCNode;
		}

		// Token: 0x02000128 RID: 296
		internal class MultipleEntitiesAndComponentsCommandView : EntityCommandBuffer.MultipleEntitiesCommandView
		{
			// Token: 0x06000611 RID: 1553 RVA: 0x0002006C File Offset: 0x0001E26C
			public unsafe MultipleEntitiesAndComponentsCommandView(MultipleEntitiesAndComponentsCommand* cmd)
			{
				this.CommandType = cmd->Header.Header.CommandType;
				this.SortKey = cmd->Header.Header.SortKey;
				this.TotalSizeInBytes = cmd->Header.Header.TotalSize;
				this.Entities = cmd->Header.Entities;
				this.EntitiesCount = cmd->Header.EntitiesCount;
				this.SkipDeferredEntityLookup = cmd->Header.SkipDeferredEntityLookup != 0;
				this.Allocator = cmd->Header.Allocator.ToAllocator;
				this.TypeSet = cmd->TypeSet;
			}

			// Token: 0x06000612 RID: 1554 RVA: 0x00020120 File Offset: 0x0001E320
			public override string ToString()
			{
				if (this.CommandType != ECBCommand.AddMultipleComponentsForMultipleEntities)
				{
					return string.Format("Remove {0} Components from {1} Entities", this.TypeSet.Length, this.EntitiesCount);
				}
				return string.Format("Add {0} ", this.TypeSet.Length) + string.Format("Components to {0} Entities", this.EntitiesCount);
			}

			// Token: 0x0400035E RID: 862
			public ComponentTypeSet TypeSet;
		}

		// Token: 0x02000129 RID: 297
		internal class EntityComponentCommandView : EntityCommandBuffer.EntityCommandView
		{
			// Token: 0x06000613 RID: 1555 RVA: 0x00020191 File Offset: 0x0001E391
			public EntityComponentCommandView()
			{
				this.ComponentTypeIndex = TypeIndex.Null;
				this.ComponentSize = 0;
				this.ComponentValue = null;
			}

			// Token: 0x06000614 RID: 1556 RVA: 0x000201B4 File Offset: 0x0001E3B4
			public unsafe EntityComponentCommandView(EntityComponentCommand* cmd, byte* componentValue)
			{
				this.CommandType = cmd->Header.Header.CommandType;
				this.SortKey = cmd->Header.Header.SortKey;
				this.TotalSizeInBytes = cmd->Header.Header.TotalSize;
				this.Entity = cmd->Header.Entity;
				this.IdentityIndex = cmd->Header.IdentityIndex;
				this.BatchCount = cmd->Header.BatchCount;
				this.ComponentTypeIndex = cmd->ComponentTypeIndex;
				this.ComponentSize = cmd->ComponentSize;
				this.ValueRequiresEntityFixup = cmd->ValueRequiresEntityFixup > 0;
				if (this.ComponentSize > 0 && componentValue != null)
				{
					this.ComponentValue = TypeManager.ConstructComponentFromBuffer(this.ComponentTypeIndex, (void*)componentValue);
					return;
				}
				this.ComponentValue = null;
			}

			// Token: 0x06000615 RID: 1557 RVA: 0x0002028C File Offset: 0x0001E48C
			public override string ToString()
			{
				string text = TypeManager.GetType(this.ComponentTypeIndex).Name + " ";
				ECBCommand commandType = this.CommandType;
				switch (commandType)
				{
				case ECBCommand.AddComponent:
					return "Add " + text + "Component";
				case ECBCommand.AddMultipleComponents:
				case ECBCommand.RemoveMultipleComponents:
					break;
				case ECBCommand.RemoveComponent:
					return "Remove " + text + "Component";
				case ECBCommand.SetComponent:
					return "Set " + text + "Component";
				default:
					if (commandType == ECBCommand.AppendToBuffer)
					{
						return "Append " + text + "BufferElementData";
					}
					if (commandType == ECBCommand.ReplaceComponentLinkedEntityGroup)
					{
						return "Replace " + text + "Component for LinkedEntityGroup";
					}
					break;
				}
				throw new ArgumentException("Unknown Command");
			}

			// Token: 0x0400035F RID: 863
			public TypeIndex ComponentTypeIndex;

			// Token: 0x04000360 RID: 864
			public short ComponentSize;

			// Token: 0x04000361 RID: 865
			public object ComponentValue;

			// Token: 0x04000362 RID: 866
			public bool ValueRequiresEntityFixup;
		}

		// Token: 0x0200012A RID: 298
		internal class EntityQueryMaskCommandView : EntityCommandBuffer.EntityComponentCommandView
		{
			// Token: 0x06000616 RID: 1558 RVA: 0x00020342 File Offset: 0x0001E542
			public EntityQueryMaskCommandView()
			{
				this.ComponentTypeIndex = TypeIndex.Null;
				this.ComponentSize = 0;
				this.ComponentValue = null;
			}

			// Token: 0x06000617 RID: 1559 RVA: 0x00020364 File Offset: 0x0001E564
			public unsafe EntityQueryMaskCommandView(EntityQueryMaskCommand* cmd, byte* componentValue)
			{
				this.CommandType = cmd->Header.Header.Header.CommandType;
				this.SortKey = cmd->Header.Header.Header.SortKey;
				this.TotalSizeInBytes = cmd->Header.Header.Header.TotalSize;
				this.Entity = cmd->Header.Header.Entity;
				this.IdentityIndex = cmd->Header.Header.IdentityIndex;
				this.BatchCount = cmd->Header.Header.BatchCount;
				this.Mask = cmd->Mask;
				this.ComponentTypeIndex = cmd->Header.ComponentTypeIndex;
				this.ComponentSize = cmd->Header.ComponentSize;
				this.ValueRequiresEntityFixup = cmd->Header.ValueRequiresEntityFixup > 0;
				this.ComponentValue = ((this.ComponentSize > 0 && componentValue != null) ? TypeManager.ConstructComponentFromBuffer(this.ComponentTypeIndex, (void*)componentValue) : null);
			}

			// Token: 0x06000618 RID: 1560 RVA: 0x00020470 File Offset: 0x0001E670
			public override string ToString()
			{
				string text = TypeManager.GetType(this.ComponentTypeIndex).Name + " ";
				ECBCommand commandType = this.CommandType;
				if (commandType == ECBCommand.AddComponentLinkedEntityGroup)
				{
					return "Add " + text + "Component for LinkedEntityGroup";
				}
				if (commandType != ECBCommand.SetComponentLinkedEntityGroup)
				{
					throw new ArgumentException("Unknown Command");
				}
				return "Set " + text + "Component for LinkedEntityGroup";
			}

			// Token: 0x04000363 RID: 867
			public EntityQueryMask Mask;
		}

		// Token: 0x0200012B RID: 299
		internal class EntityEnabledCommandView : EntityCommandBuffer.EntityCommandView
		{
			// Token: 0x06000619 RID: 1561 RVA: 0x000204D8 File Offset: 0x0001E6D8
			public unsafe EntityEnabledCommandView(EntityEnabledCommand* cmd)
			{
				this.CommandType = cmd->Header.Header.CommandType;
				this.SortKey = cmd->Header.Header.SortKey;
				this.TotalSizeInBytes = cmd->Header.Header.TotalSize;
				this.Entity = cmd->Header.Entity;
				this.IdentityIndex = cmd->Header.IdentityIndex;
				this.BatchCount = cmd->Header.BatchCount;
				this.IsEnabled = cmd->IsEnabled;
			}

			// Token: 0x0600061A RID: 1562 RVA: 0x0002056C File Offset: 0x0001E76C
			public override string ToString()
			{
				if (this.IsEnabled != 1)
				{
					return string.Format("Set Entity ({0},{1}) to Disabled", this.Entity.Index, this.Entity.Version);
				}
				return string.Format("Set Entity ({0},{1}) to Enabled", this.Entity.Index, this.Entity.Version);
			}

			// Token: 0x04000364 RID: 868
			public int ComponentTypeIndex;

			// Token: 0x04000365 RID: 869
			public byte IsEnabled;
		}

		// Token: 0x0200012C RID: 300
		internal class EntityComponentEnabledCommandView : EntityCommandBuffer.EntityCommandView
		{
			// Token: 0x0600061B RID: 1563 RVA: 0x000205D8 File Offset: 0x0001E7D8
			public unsafe EntityComponentEnabledCommandView(EntityComponentEnabledCommand* cmd)
			{
				this.CommandType = cmd->Header.Header.Header.CommandType;
				this.SortKey = cmd->Header.Header.Header.SortKey;
				this.TotalSizeInBytes = cmd->Header.Header.Header.TotalSize;
				this.Entity = cmd->Header.Header.Entity;
				this.IdentityIndex = cmd->Header.Header.IdentityIndex;
				this.BatchCount = cmd->Header.Header.BatchCount;
				this.IsEnabled = cmd->Header.IsEnabled;
				this.ComponentTypeIndex = cmd->ComponentTypeIndex;
			}

			// Token: 0x0600061C RID: 1564 RVA: 0x0002069C File Offset: 0x0001E89C
			public override string ToString()
			{
				string text = TypeManager.GetType(this.ComponentTypeIndex).Name + " ";
				if (this.IsEnabled == 1)
				{
					return text + "Component Disabled";
				}
				return text + "Component Enabled";
			}

			// Token: 0x04000366 RID: 870
			public TypeIndex ComponentTypeIndex;

			// Token: 0x04000367 RID: 871
			public byte IsEnabled;
		}

		// Token: 0x0200012D RID: 301
		internal class EntityNameCommandView : EntityCommandBuffer.EntityCommandView
		{
			// Token: 0x0600061D RID: 1565 RVA: 0x000206E4 File Offset: 0x0001E8E4
			public unsafe EntityNameCommandView(EntityNameCommand* cmd)
			{
				this.CommandType = cmd->Header.Header.CommandType;
				this.SortKey = cmd->Header.Header.SortKey;
				this.TotalSizeInBytes = cmd->Header.Header.TotalSize;
				this.Entity = cmd->Header.Entity;
				this.IdentityIndex = cmd->Header.IdentityIndex;
				this.BatchCount = cmd->Header.BatchCount;
				this.Name = cmd->Name;
			}

			// Token: 0x0600061E RID: 1566 RVA: 0x00020778 File Offset: 0x0001E978
			public override string ToString()
			{
				return "Set EntityName: " + this.Name.ToString();
			}

			// Token: 0x04000368 RID: 872
			public FixedString64Bytes Name;
		}

		// Token: 0x0200012E RID: 302
		internal class EntityMultipleComponentsCommandView : EntityCommandBuffer.EntityCommandView
		{
			// Token: 0x0600061F RID: 1567 RVA: 0x00020798 File Offset: 0x0001E998
			public unsafe EntityMultipleComponentsCommandView(EntityMultipleComponentsCommand* cmd)
			{
				this.CommandType = cmd->Header.Header.CommandType;
				this.SortKey = cmd->Header.Header.SortKey;
				this.TotalSizeInBytes = cmd->Header.Header.TotalSize;
				this.Entity = cmd->Header.Entity;
				this.IdentityIndex = cmd->Header.IdentityIndex;
				this.BatchCount = cmd->Header.BatchCount;
				this.TypeSet = cmd->TypeSet;
			}

			// Token: 0x06000620 RID: 1568 RVA: 0x0002082C File Offset: 0x0001EA2C
			public override string ToString()
			{
				if (this.CommandType == ECBCommand.AddMultipleComponents)
				{
					return string.Format("Add {0} Components", this.TypeSet.Length);
				}
				return string.Format("Remove {0} Components", this.TypeSet.Length);
			}

			// Token: 0x04000369 RID: 873
			public ComponentTypeSet TypeSet;
		}

		// Token: 0x0200012F RID: 303
		internal class EntityBufferCommandView : EntityCommandBuffer.EntityCommandView
		{
			// Token: 0x06000621 RID: 1569 RVA: 0x0002086C File Offset: 0x0001EA6C
			public unsafe EntityBufferCommandView(EntityBufferCommand* cmd)
			{
				this.CommandType = cmd->Header.Header.CommandType;
				this.SortKey = cmd->Header.Header.SortKey;
				this.TotalSizeInBytes = cmd->Header.Header.TotalSize;
				this.Entity = cmd->Header.Entity;
				this.IdentityIndex = cmd->Header.IdentityIndex;
				this.BatchCount = cmd->Header.BatchCount;
				this.ComponentTypeIndex = cmd->ComponentTypeIndex;
				this.ComponentSize = cmd->ComponentSize;
				this.ValueRequiresEntityFixup = cmd->ValueRequiresEntityFixup > 0;
				this.BufferNode = &cmd->BufferNode;
			}

			// Token: 0x06000622 RID: 1570 RVA: 0x00020928 File Offset: 0x0001EB28
			public override string ToString()
			{
				Type type = TypeManager.GetType(this.ComponentTypeIndex);
				string text = " " + type.Name;
				if (this.CommandType != ECBCommand.AddBuffer)
				{
					return "Set Entity Buffer" + text;
				}
				return "Add Entity Buffer" + text;
			}

			// Token: 0x0400036A RID: 874
			public TypeIndex ComponentTypeIndex;

			// Token: 0x0400036B RID: 875
			public short ComponentSize;

			// Token: 0x0400036C RID: 876
			public bool ValueRequiresEntityFixup;

			// Token: 0x0400036D RID: 877
			public unsafe BufferHeaderNode* BufferNode;
		}

		// Token: 0x02000130 RID: 304
		internal class EntityManagedComponentCommandView : EntityCommandBuffer.EntityCommandView
		{
			// Token: 0x06000623 RID: 1571 RVA: 0x00020973 File Offset: 0x0001EB73
			internal object GetBoxedObject()
			{
				if (this.GCNode.BoxedObject.IsAllocated)
				{
					return this.GCNode.BoxedObject.Target;
				}
				return null;
			}

			// Token: 0x06000624 RID: 1572 RVA: 0x0002099C File Offset: 0x0001EB9C
			public unsafe EntityManagedComponentCommandView(EntityManagedComponentCommand* cmd)
			{
				this.CommandType = cmd->Header.Header.CommandType;
				this.SortKey = cmd->Header.Header.SortKey;
				this.TotalSizeInBytes = cmd->Header.Header.TotalSize;
				this.Entity = cmd->Header.Entity;
				this.IdentityIndex = cmd->Header.IdentityIndex;
				this.BatchCount = cmd->Header.BatchCount;
				this.ComponentTypeIndex = cmd->ComponentTypeIndex;
				this.GCNode = cmd->GCNode;
			}

			// Token: 0x06000625 RID: 1573 RVA: 0x00020A3C File Offset: 0x0001EC3C
			public override string ToString()
			{
				string text = TypeManager.GetType(this.ComponentTypeIndex).Name + " ";
				if (this.CommandType != ECBCommand.AddManagedComponentData)
				{
					return "Set " + text + "Component (Managed)";
				}
				return "Add " + text + "Component (Managed)";
			}

			// Token: 0x0400036E RID: 878
			public TypeIndex ComponentTypeIndex;

			// Token: 0x0400036F RID: 879
			public EntityComponentGCNode GCNode;
		}

		// Token: 0x02000131 RID: 305
		internal class EntityMoveManagedComponentCommandView : EntityCommandBuffer.EntityCommandView
		{
			// Token: 0x06000626 RID: 1574 RVA: 0x00020A90 File Offset: 0x0001EC90
			public unsafe EntityMoveManagedComponentCommandView(EntityMoveManagedComponentCommand* cmd)
			{
				this.CommandType = cmd->Header.Header.CommandType;
				this.SortKey = cmd->Header.Header.SortKey;
				this.TotalSizeInBytes = cmd->Header.Header.TotalSize;
				this.Entity = cmd->Header.Entity;
				this.IdentityIndex = cmd->Header.IdentityIndex;
				this.BatchCount = cmd->Header.BatchCount;
				this.SrcEntity = cmd->SrcEntity;
				this.ComponentTypeIndex = cmd->ComponentTypeIndex;
			}

			// Token: 0x06000627 RID: 1575 RVA: 0x00020B30 File Offset: 0x0001ED30
			public override string ToString()
			{
				string text = TypeManager.GetType(this.ComponentTypeIndex).Name + " ";
				return "Move " + text + "Component (Managed)";
			}

			// Token: 0x04000370 RID: 880
			public TypeIndex ComponentTypeIndex;

			// Token: 0x04000371 RID: 881
			public Entity SrcEntity;
		}

		// Token: 0x02000132 RID: 306
		internal class EntityUnmanagedSharedComponentCommandView : EntityCommandBuffer.EntityCommandView
		{
			// Token: 0x06000628 RID: 1576 RVA: 0x00020B68 File Offset: 0x0001ED68
			public unsafe EntityUnmanagedSharedComponentCommandView(EntityUnmanagedSharedComponentCommand* cmd, byte* componentValue)
			{
				this.CommandType = cmd->Header.Header.CommandType;
				this.SortKey = cmd->Header.Header.SortKey;
				this.TotalSizeInBytes = cmd->Header.Header.TotalSize;
				this.Entity = cmd->Header.Entity;
				this.IdentityIndex = cmd->Header.IdentityIndex;
				this.BatchCount = cmd->Header.BatchCount;
				this.ComponentTypeIndex = cmd->ComponentTypeIndex;
				this.HashCode = cmd->HashCode;
				this.ComponentSize = (short)TypeManager.GetTypeInfo(cmd->ComponentTypeIndex).TypeSize;
				this.ValueRequiresEntityFixup = cmd->ValueRequiresEntityFixup > 0;
				this.ComponentValue = TypeManager.ConstructComponentFromBuffer(cmd->ComponentTypeIndex, (void*)componentValue);
			}

			// Token: 0x06000629 RID: 1577 RVA: 0x00020C40 File Offset: 0x0001EE40
			public override string ToString()
			{
				string text = TypeManager.GetType(this.ComponentTypeIndex).Name + " ";
				if (this.CommandType != ECBCommand.AddUnmanagedSharedComponentData)
				{
					return "Set " + text + "UnmanagedSharedComponentData";
				}
				return "Add " + text + "UnmanagedSharedComponentData";
			}

			// Token: 0x04000372 RID: 882
			public TypeIndex ComponentTypeIndex;

			// Token: 0x04000373 RID: 883
			public int HashCode;

			// Token: 0x04000374 RID: 884
			public short ComponentSize;

			// Token: 0x04000375 RID: 885
			public bool ValueRequiresEntityFixup;

			// Token: 0x04000376 RID: 886
			public object ComponentValue;
		}

		// Token: 0x02000133 RID: 307
		internal class MultipleEntitiesComponentCommandView_WithUnmanagedSharedValue : EntityCommandBuffer.MultipleEntitiesCommandView
		{
			// Token: 0x0600062A RID: 1578 RVA: 0x00020C94 File Offset: 0x0001EE94
			public unsafe MultipleEntitiesComponentCommandView_WithUnmanagedSharedValue(MultipleEntitiesCommand_WithUnmanagedSharedComponent* cmd, byte* componentValue)
			{
				this.CommandType = cmd->Header.Header.CommandType;
				this.SortKey = cmd->Header.Header.SortKey;
				this.TotalSizeInBytes = cmd->Header.Header.TotalSize;
				this.Entities = cmd->Header.Entities;
				this.EntitiesCount = cmd->Header.EntitiesCount;
				this.Allocator = cmd->Header.Allocator.ToAllocator;
				this.ComponentTypeIndex = cmd->ComponentTypeIndex;
				this.ComponentSize = cmd->ComponentSize;
				this.SkipDeferredEntityLookup = cmd->Header.SkipDeferredEntityLookup != 0;
				this.ValueRequiresEntityFixup = cmd->ValueRequiresEntityFixup > 0;
				if (this.ComponentSize > 0 && componentValue != null)
				{
					this.ComponentValue = TypeManager.ConstructComponentFromBuffer(cmd->ComponentTypeIndex, (void*)componentValue);
					return;
				}
				this.ComponentValue = null;
			}

			// Token: 0x0600062B RID: 1579 RVA: 0x00020D8C File Offset: 0x0001EF8C
			public override string ToString()
			{
				string text = TypeManager.GetType(this.ComponentTypeIndex).Name + " ";
				if (this.CommandType != ECBCommand.AddUnmanagedSharedComponentValueForMultipleEntities)
				{
					return string.Format("Set {0}Unmanaged Shared Component from {1} Entities", text, this.EntitiesCount);
				}
				return string.Format("Add {0}Unmanaged Shared Component to {1} Entities", text, this.EntitiesCount);
			}

			// Token: 0x04000377 RID: 887
			public TypeIndex ComponentTypeIndex;

			// Token: 0x04000378 RID: 888
			public int ComponentSize;

			// Token: 0x04000379 RID: 889
			public bool ValueRequiresEntityFixup;

			// Token: 0x0400037A RID: 890
			public object ComponentValue;
		}

		// Token: 0x02000134 RID: 308
		internal class EntityQueryComponentCommandView_WithUnmanagedSharedValue : EntityCommandBuffer.EntityQueryComponentCommandView
		{
			// Token: 0x0600062C RID: 1580 RVA: 0x00020DEC File Offset: 0x0001EFEC
			public unsafe EntityQueryComponentCommandView_WithUnmanagedSharedValue(EntityQueryComponentCommandWithUnmanagedSharedComponent* cmd, byte* componentValue)
			{
				this.CommandType = cmd->Header.Header.Header.CommandType;
				this.SortKey = cmd->Header.Header.Header.SortKey;
				this.TotalSizeInBytes = cmd->Header.Header.Header.TotalSize;
				this.Query = cmd->Header.Header.QueryImpl;
				this.ComponentTypeIndex = cmd->Header.ComponentTypeIndex;
				this.ComponentSize = cmd->ComponentSize;
				this.ValueRequiresEntityFixup = cmd->ValueRequiresEntityFixup > 0;
				if (this.ComponentSize > 0 && componentValue != null)
				{
					this.ComponentValue = TypeManager.ConstructComponentFromBuffer(cmd->Header.ComponentTypeIndex, (void*)componentValue);
					return;
				}
				this.ComponentValue = null;
			}

			// Token: 0x0600062D RID: 1581 RVA: 0x00020EC0 File Offset: 0x0001F0C0
			public override string ToString()
			{
				string text = TypeManager.GetType(this.ComponentTypeIndex).Name + " ";
				if (this.CommandType != ECBCommand.AddUnmanagedSharedComponentValueForEntityQuery)
				{
					return "Set " + text + "Unmanaged Shared Component from EntityQuery";
				}
				return "Add " + text + "Unmanaged Shared Component to EntityQuery";
			}

			// Token: 0x0400037B RID: 891
			public int ComponentSize;

			// Token: 0x0400037C RID: 892
			public bool ValueRequiresEntityFixup;

			// Token: 0x0400037D RID: 893
			public object ComponentValue;
		}

		// Token: 0x02000135 RID: 309
		internal class EntitySharedComponentCommandView : EntityCommandBuffer.EntityCommandView
		{
			// Token: 0x0600062E RID: 1582 RVA: 0x00020F13 File Offset: 0x0001F113
			internal object GetBoxedObject()
			{
				if (this.GCNode.BoxedObject.IsAllocated)
				{
					return this.GCNode.BoxedObject.Target;
				}
				return null;
			}

			// Token: 0x0600062F RID: 1583 RVA: 0x00020F3C File Offset: 0x0001F13C
			public unsafe EntitySharedComponentCommandView(EntitySharedComponentCommand* cmd)
			{
				this.CommandType = cmd->Header.Header.CommandType;
				this.SortKey = cmd->Header.Header.SortKey;
				this.TotalSizeInBytes = cmd->Header.Header.TotalSize;
				this.Entity = cmd->Header.Entity;
				this.IdentityIndex = cmd->Header.IdentityIndex;
				this.BatchCount = cmd->Header.BatchCount;
				this.ComponentTypeIndex = cmd->ComponentTypeIndex;
				this.HashCode = cmd->HashCode;
				this.GCNode = cmd->GCNode;
			}

			// Token: 0x06000630 RID: 1584 RVA: 0x00020FE8 File Offset: 0x0001F1E8
			public override string ToString()
			{
				string text = TypeManager.GetType(this.ComponentTypeIndex).Name + " ";
				if (this.CommandType != ECBCommand.AddSharedComponentData)
				{
					return "Set " + text + "SharedComponentData";
				}
				return "Add " + text + "SharedComponentData";
			}

			// Token: 0x0400037E RID: 894
			public TypeIndex ComponentTypeIndex;

			// Token: 0x0400037F RID: 895
			public int HashCode;

			// Token: 0x04000380 RID: 896
			public EntityComponentGCNode GCNode;
		}

		// Token: 0x02000136 RID: 310
		internal sealed class EntityCommandBufferDebugView
		{
			// Token: 0x06000631 RID: 1585 RVA: 0x0002103B File Offset: 0x0001F23B
			public EntityCommandBufferDebugView(EntityCommandBuffer ecb)
			{
				this.m_ecb = ecb;
			}

			// Token: 0x06000632 RID: 1586 RVA: 0x0002104C File Offset: 0x0001F24C
			public unsafe EntityCommandBuffer.BasicCommandView ProcessDebugViewCommand(BasicCommand* header)
			{
				switch (header->CommandType)
				{
				case ECBCommand.InstantiateEntity:
				case ECBCommand.DestroyEntity:
					return new EntityCommandBuffer.EntityCommandView((EntityCommand*)header);
				case ECBCommand.CreateEntity:
					return new EntityCommandBuffer.CreateCommandView((CreateCommand*)header);
				case ECBCommand.AddComponent:
				case ECBCommand.RemoveComponent:
				case ECBCommand.SetComponent:
				case ECBCommand.AppendToBuffer:
				case ECBCommand.ReplaceComponentLinkedEntityGroup:
				{
					byte* ptr = (byte*)((header->CommandType != ECBCommand.RemoveComponent) ? (header + sizeof(EntityComponentCommand) / sizeof(BasicCommand)) : null);
					return new EntityCommandBuffer.EntityComponentCommandView((EntityComponentCommand*)header, ptr);
				}
				case ECBCommand.AddMultipleComponents:
				case ECBCommand.RemoveMultipleComponents:
					return new EntityCommandBuffer.EntityMultipleComponentsCommandView((EntityMultipleComponentsCommand*)header);
				case ECBCommand.SetEntityEnabled:
					return new EntityCommandBuffer.EntityEnabledCommandView((EntityEnabledCommand*)header);
				case ECBCommand.SetComponentEnabled:
					return new EntityCommandBuffer.EntityComponentEnabledCommandView((EntityComponentEnabledCommand*)header);
				case ECBCommand.SetName:
					return new EntityCommandBuffer.EntityNameCommandView((EntityNameCommand*)header);
				case ECBCommand.AddBuffer:
				case ECBCommand.SetBuffer:
					return new EntityCommandBuffer.EntityBufferCommandView((EntityBufferCommand*)header);
				case ECBCommand.AddManagedComponentData:
				case ECBCommand.SetManagedComponentData:
					return new EntityCommandBuffer.EntityManagedComponentCommandView((EntityManagedComponentCommand*)header);
				case ECBCommand.MoveManagedComponentData:
					return new EntityCommandBuffer.EntityMoveManagedComponentCommandView((EntityMoveManagedComponentCommand*)header);
				case ECBCommand.AddComponentLinkedEntityGroup:
				case ECBCommand.SetComponentLinkedEntityGroup:
				{
					byte* ptr2 = (byte*)(header + sizeof(EntityQueryMaskCommand) / sizeof(BasicCommand));
					return new EntityCommandBuffer.EntityQueryMaskCommandView((EntityQueryMaskCommand*)header, ptr2);
				}
				case ECBCommand.AddSharedComponentData:
				case ECBCommand.SetSharedComponentData:
					return new EntityCommandBuffer.EntitySharedComponentCommandView((EntitySharedComponentCommand*)header);
				case ECBCommand.AddUnmanagedSharedComponentData:
				case ECBCommand.SetUnmanagedSharedComponentData:
				{
					byte* ptr3 = (byte*)(header + sizeof(EntityUnmanagedSharedComponentCommand) / sizeof(BasicCommand));
					return new EntityCommandBuffer.EntityUnmanagedSharedComponentCommandView((EntityUnmanagedSharedComponentCommand*)header, ptr3);
				}
				case ECBCommand.AddUnmanagedSharedComponentValueForMultipleEntities:
				case ECBCommand.SetUnmanagedSharedComponentValueForMultipleEntities:
				{
					byte* ptr4 = (byte*)(header + sizeof(MultipleEntitiesCommand_WithUnmanagedSharedComponent) / sizeof(BasicCommand));
					return new EntityCommandBuffer.MultipleEntitiesComponentCommandView_WithUnmanagedSharedValue((MultipleEntitiesCommand_WithUnmanagedSharedComponent*)header, ptr4);
				}
				case ECBCommand.AddUnmanagedSharedComponentValueForEntityQuery:
				case ECBCommand.SetUnmanagedSharedComponentValueForEntityQuery:
				{
					byte* ptr5 = (byte*)(header + sizeof(EntityQueryComponentCommandWithUnmanagedSharedComponent) / sizeof(BasicCommand));
					return new EntityCommandBuffer.EntityQueryComponentCommandView_WithUnmanagedSharedValue((EntityQueryComponentCommandWithUnmanagedSharedComponent*)header, ptr5);
				}
				case ECBCommand.AddComponentForEntityQuery:
				case ECBCommand.RemoveComponentForEntityQuery:
					return new EntityCommandBuffer.EntityQueryComponentCommandView((EntityQueryComponentCommand*)header);
				case ECBCommand.AddMultipleComponentsForMultipleEntities:
				case ECBCommand.RemoveMultipleComponentsForMultipleEntities:
					return new EntityCommandBuffer.MultipleEntitiesAndComponentsCommandView((MultipleEntitiesAndComponentsCommand*)header);
				case ECBCommand.AddMultipleComponentsForEntityQuery:
				case ECBCommand.RemoveMultipleComponentsForEntityQuery:
					return new EntityCommandBuffer.EntityQueryComponentTypeSetCommandView((EntityQueryComponentTypeSetCommand*)header);
				case ECBCommand.AddSharedComponentWithValueForMultipleEntities:
				case ECBCommand.SetSharedComponentValueForMultipleEntities:
				case ECBCommand.AddComponentObjectForMultipleEntities:
				case ECBCommand.SetComponentObjectForMultipleEntities:
					return new EntityCommandBuffer.MultipleEntitiesComponentCommandWithObjectView((MultipleEntitiesComponentCommandWithObject*)header);
				case ECBCommand.AddSharedComponentWithValueForEntityQuery:
				case ECBCommand.SetSharedComponentValueForEntityQuery:
					return new EntityCommandBuffer.EntityQueryComponentCommandWithObjectView((EntityQueryComponentCommandWithObject*)header);
				case ECBCommand.AddComponentForMultipleEntities:
				case ECBCommand.RemoveComponentForMultipleEntities:
				{
					byte* ptr6 = (byte*)((header->CommandType != ECBCommand.RemoveComponentForMultipleEntities) ? (header + sizeof(MultipleEntitiesComponentCommand) / sizeof(BasicCommand)) : null);
					return new EntityCommandBuffer.MultipleEntitiesComponentCommandView((MultipleEntitiesComponentCommand*)header, ptr6);
				}
				case ECBCommand.DestroyMultipleEntities:
					return new EntityCommandBuffer.MultipleEntitiesCommandView((MultipleEntitiesCommand*)header);
				case ECBCommand.DestroyForEntityQuery:
					return new EntityCommandBuffer.EntityQueryCommandView((EntityQueryCommand*)header);
				}
				return null;
			}

			// Token: 0x1700009B RID: 155
			// (get) Token: 0x06000633 RID: 1587 RVA: 0x0002123C File Offset: 0x0001F43C
			public unsafe EntityCommandBuffer.BasicCommandView[] Commands
			{
				get
				{
					EntityCommandBuffer.EcbWalker<EntityCommandBuffer.DebugViewProcessor> ecbWalker = new EntityCommandBuffer.EcbWalker<EntityCommandBuffer.DebugViewProcessor>(default(EntityCommandBuffer.DebugViewProcessor), EntityCommandBuffer.ECBProcessorType.DebugViewProcessor);
					ecbWalker.processor.Init(this.m_ecb.m_Data->m_Allocator.ToAllocator);
					ecbWalker.WalkChains(this.m_ecb);
					EntityCommandBuffer.BasicCommandView[] array = new EntityCommandBuffer.BasicCommandView[ecbWalker.processor.commands.Length];
					for (int i = 0; i < ecbWalker.processor.commands.Length; i++)
					{
						array[i] = this.ProcessDebugViewCommand(ecbWalker.processor.commands[i]);
					}
					ecbWalker.processor.Cleanup();
					return array;
				}
			}

			// Token: 0x04000381 RID: 897
			private EntityCommandBuffer m_ecb;
		}

		// Token: 0x02000137 RID: 311
		// (Invoke) Token: 0x06000635 RID: 1589
		internal delegate void DisposeInternal_000003D2$PostfixBurstDelegate(ref EntityCommandBuffer ecb);

		// Token: 0x02000138 RID: 312
		internal static class DisposeInternal_000003D2$BurstDirectCall
		{
			// Token: 0x06000638 RID: 1592 RVA: 0x000212E8 File Offset: 0x0001F4E8
			[BurstDiscard]
			private static void GetFunctionPointerDiscard(ref IntPtr A_0)
			{
				if (EntityCommandBuffer.DisposeInternal_000003D2$BurstDirectCall.Pointer == 0)
				{
					EntityCommandBuffer.DisposeInternal_000003D2$BurstDirectCall.Pointer = BurstCompiler.GetILPPMethodFunctionPointer2(EntityCommandBuffer.DisposeInternal_000003D2$BurstDirectCall.DeferredCompilation, methodof(EntityCommandBuffer.DisposeInternal$BurstManaged(ref EntityCommandBuffer)).MethodHandle, typeof(EntityCommandBuffer.DisposeInternal_000003D2$PostfixBurstDelegate).TypeHandle);
				}
				A_0 = EntityCommandBuffer.DisposeInternal_000003D2$BurstDirectCall.Pointer;
			}

			// Token: 0x06000639 RID: 1593 RVA: 0x00021314 File Offset: 0x0001F514
			private static IntPtr GetFunctionPointer()
			{
				IntPtr intPtr = (IntPtr)0;
				EntityCommandBuffer.DisposeInternal_000003D2$BurstDirectCall.GetFunctionPointerDiscard(ref intPtr);
				return intPtr;
			}

			// Token: 0x0600063A RID: 1594 RVA: 0x0002132C File Offset: 0x0001F52C
			public static void Constructor()
			{
				EntityCommandBuffer.DisposeInternal_000003D2$BurstDirectCall.DeferredCompilation = BurstCompiler.CompileILPPMethod2(methodof(EntityCommandBuffer.DisposeInternal(ref EntityCommandBuffer)).MethodHandle);
			}

			// Token: 0x0600063B RID: 1595 RVA: 0x00002287 File Offset: 0x00000487
			public static void Initialize()
			{
			}

			// Token: 0x0600063C RID: 1596 RVA: 0x0002133D File Offset: 0x0001F53D
			// Note: this type is marked as 'beforefieldinit'.
			static DisposeInternal_000003D2$BurstDirectCall()
			{
				EntityCommandBuffer.DisposeInternal_000003D2$BurstDirectCall.Constructor();
			}

			// Token: 0x0600063D RID: 1597 RVA: 0x00021344 File Offset: 0x0001F544
			public static void Invoke(ref EntityCommandBuffer ecb)
			{
				if (BurstCompiler.IsEnabled)
				{
					IntPtr functionPointer = EntityCommandBuffer.DisposeInternal_000003D2$BurstDirectCall.GetFunctionPointer();
					if (functionPointer != 0)
					{
						calli(System.Void(Unity.Entities.EntityCommandBuffer&), ref ecb, functionPointer);
						return;
					}
				}
				EntityCommandBuffer.DisposeInternal$BurstManaged(ref ecb);
			}

			// Token: 0x04000382 RID: 898
			private static IntPtr Pointer;

			// Token: 0x04000383 RID: 899
			private static IntPtr DeferredCompilation;
		}
	}
}
