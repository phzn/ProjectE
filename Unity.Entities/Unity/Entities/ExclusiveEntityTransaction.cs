using System;
using System.Runtime.CompilerServices;
using Unity.Collections;

namespace Unity.Entities
{
	// Token: 0x02000192 RID: 402
	public struct ExclusiveEntityTransaction
	{
		// Token: 0x170000C6 RID: 198
		// (get) Token: 0x060009E3 RID: 2531 RVA: 0x00031EFE File Offset: 0x000300FE
		public EntityManager EntityManager
		{
			get
			{
				return this.m_Manager;
			}
		}

		// Token: 0x060009E4 RID: 2532 RVA: 0x00031F06 File Offset: 0x00030106
		internal ExclusiveEntityTransaction(EntityManager manager)
		{
			this.m_Manager = manager;
		}

		// Token: 0x060009E5 RID: 2533 RVA: 0x00031F0F File Offset: 0x0003010F
		internal unsafe EntityArchetype CreateArchetype(ComponentType* types, int count)
		{
			return this.m_Manager.CreateArchetype(types, count);
		}

		// Token: 0x060009E6 RID: 2534 RVA: 0x00031F1E File Offset: 0x0003011E
		public EntityArchetype CreateArchetype(params ComponentType[] types)
		{
			return this.m_Manager.CreateArchetype(types);
		}

		// Token: 0x060009E7 RID: 2535 RVA: 0x00031F2C File Offset: 0x0003012C
		public Entity CreateEntity(EntityArchetype archetype)
		{
			return this.m_Manager.CreateEntity(archetype);
		}

		// Token: 0x060009E8 RID: 2536 RVA: 0x00031F3A File Offset: 0x0003013A
		public void CreateEntity(EntityArchetype archetype, NativeArray<Entity> entities)
		{
			this.m_Manager.CreateEntity(archetype, entities);
		}

		// Token: 0x060009E9 RID: 2537 RVA: 0x00031F49 File Offset: 0x00030149
		public Entity CreateEntity(params ComponentType[] types)
		{
			return this.m_Manager.CreateEntity(types);
		}

		// Token: 0x060009EA RID: 2538 RVA: 0x00031F57 File Offset: 0x00030157
		public Entity Instantiate(Entity srcEntity)
		{
			return this.m_Manager.Instantiate(srcEntity);
		}

		// Token: 0x060009EB RID: 2539 RVA: 0x00031F65 File Offset: 0x00030165
		public void Instantiate(Entity srcEntity, NativeArray<Entity> outputEntities)
		{
			this.m_Manager.Instantiate(srcEntity, outputEntities);
		}

		// Token: 0x060009EC RID: 2540 RVA: 0x00031F74 File Offset: 0x00030174
		public void DestroyEntity(NativeArray<Entity> entities)
		{
			this.m_Manager.DestroyEntity(entities);
		}

		// Token: 0x060009ED RID: 2541 RVA: 0x00031F82 File Offset: 0x00030182
		public void DestroyEntity(NativeSlice<Entity> entities)
		{
			this.m_Manager.DestroyEntity(entities);
		}

		// Token: 0x060009EE RID: 2542 RVA: 0x00031F90 File Offset: 0x00030190
		public void DestroyEntity(Entity entity)
		{
			this.m_Manager.DestroyEntity(entity);
		}

		// Token: 0x060009EF RID: 2543 RVA: 0x00031F9E File Offset: 0x0003019E
		public void AddComponent(Entity entity, ComponentType componentType)
		{
			this.m_Manager.AddComponent(entity, componentType);
		}

		// Token: 0x060009F0 RID: 2544 RVA: 0x00031FAE File Offset: 0x000301AE
		[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[] { typeof(BurstCompatibleBufferElement) })]
		public DynamicBuffer<T> AddBuffer<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(Entity entity) where T : struct, ValueType, IBufferElementData
		{
			return this.m_Manager.AddBuffer<T>(entity);
		}

		// Token: 0x060009F1 RID: 2545 RVA: 0x00031FBC File Offset: 0x000301BC
		[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[] { typeof(BurstCompatibleBufferElement) })]
		public bool HasBuffer<T>(Entity entity) where T : struct, IBufferElementData
		{
			return this.m_Manager.HasBuffer<T>(entity);
		}

		// Token: 0x060009F2 RID: 2546 RVA: 0x00031FCA File Offset: 0x000301CA
		public void RemoveComponent(Entity entity, ComponentType type)
		{
			this.m_Manager.RemoveComponent(entity, type);
		}

		// Token: 0x060009F3 RID: 2547 RVA: 0x00031FDA File Offset: 0x000301DA
		public bool Exists(Entity entity)
		{
			return this.m_Manager.Exists(entity);
		}

		// Token: 0x060009F4 RID: 2548 RVA: 0x00031FE8 File Offset: 0x000301E8
		public bool HasComponent(Entity entity, ComponentType type)
		{
			return this.m_Manager.HasComponent(entity, type);
		}

		// Token: 0x060009F5 RID: 2549 RVA: 0x00031FF7 File Offset: 0x000301F7
		public T GetComponentData<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(Entity entity) where T : struct, ValueType, IComponentData
		{
			return this.m_Manager.GetComponentData<T>(entity);
		}

		// Token: 0x060009F6 RID: 2550 RVA: 0x00032005 File Offset: 0x00030205
		public void SetComponentData<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(Entity entity, T componentData) where T : struct, ValueType, IComponentData
		{
			this.m_Manager.SetComponentData<T>(entity, componentData);
		}

		// Token: 0x060009F7 RID: 2551 RVA: 0x00032014 File Offset: 0x00030214
		[Obsolete("Use GetSharedComponentManaged<T> (UnityUpgradable) -> GetSharedComponentManaged<T>(*)", true)]
		public T GetSharedComponentData<T>(Entity entity) where T : struct, ISharedComponentData
		{
			return default(T);
		}

		// Token: 0x060009F8 RID: 2552 RVA: 0x0003202A File Offset: 0x0003022A
		public T GetSharedComponentManaged<T>(Entity entity) where T : struct, ISharedComponentData
		{
			return this.m_Manager.GetSharedComponentManaged<T>(entity);
		}

		// Token: 0x060009F9 RID: 2553 RVA: 0x00032038 File Offset: 0x00030238
		public T GetSharedComponent<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(Entity entity) where T : struct, ValueType, ISharedComponentData
		{
			return this.m_Manager.GetSharedComponent<T>(entity);
		}

		// Token: 0x060009FA RID: 2554 RVA: 0x00002287 File Offset: 0x00000487
		[Obsolete("Use SetSharedComponentManaged<T> (UnityUpgradable) -> SetSharedComponentManaged<T>(*)", true)]
		public void SetSharedComponentData<T>(Entity entity, T componentData) where T : struct, ISharedComponentData
		{
		}

		// Token: 0x060009FB RID: 2555 RVA: 0x00032046 File Offset: 0x00030246
		public void SetSharedComponentManaged<T>(Entity entity, T componentData) where T : struct, ISharedComponentData
		{
			this.m_Manager.SetSharedComponentManaged<T>(entity, componentData);
		}

		// Token: 0x060009FC RID: 2556 RVA: 0x00032055 File Offset: 0x00030255
		public void SetSharedComponentManaged<T>(NativeArray<Entity> entities, T componentData) where T : struct, ISharedComponentData
		{
			this.m_Manager.SetSharedComponentManaged<T>(entities, componentData);
		}

		// Token: 0x060009FD RID: 2557 RVA: 0x00032064 File Offset: 0x00030264
		public void SetSharedComponent<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(Entity entity, T componentData) where T : struct, ValueType, ISharedComponentData
		{
			this.m_Manager.SetSharedComponent<T>(entity, componentData);
		}

		// Token: 0x060009FE RID: 2558 RVA: 0x00032073 File Offset: 0x00030273
		public void SetSharedComponent<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(NativeArray<Entity> entities, T componentData) where T : struct, ValueType, ISharedComponentData
		{
			this.m_Manager.SetSharedComponent<T>(entities, componentData);
		}

		// Token: 0x060009FF RID: 2559 RVA: 0x00032082 File Offset: 0x00030282
		internal void AddSharedComponentManaged<T>(NativeArray<ArchetypeChunk> chunks, T componentData) where T : struct, ISharedComponentData
		{
			this.m_Manager.AddSharedComponentManaged<T>(chunks, componentData);
		}

		// Token: 0x06000A00 RID: 2560 RVA: 0x00032091 File Offset: 0x00030291
		internal void AddSharedComponent<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(NativeArray<ArchetypeChunk> chunks, T componentData) where T : struct, ValueType, ISharedComponentData
		{
			this.m_Manager.AddSharedComponent<T>(chunks, componentData);
		}

		// Token: 0x06000A01 RID: 2561 RVA: 0x0001CFE4 File Offset: 0x0001B1E4
		[Obsolete("Use AddSharedComponentManaged<T> (UnityUpgradable) -> AddSharedComponentManaged<T>(*)", true)]
		public bool AddSharedComponentData<T>(Entity entity, T componentData) where T : struct, ISharedComponentData
		{
			return false;
		}

		// Token: 0x06000A02 RID: 2562 RVA: 0x000320A0 File Offset: 0x000302A0
		public bool AddSharedComponentManaged<T>(Entity entity, T componentData) where T : struct, ISharedComponentData
		{
			return this.m_Manager.AddSharedComponentManaged<T>(entity, componentData);
		}

		// Token: 0x06000A03 RID: 2563 RVA: 0x000320AF File Offset: 0x000302AF
		public void AddSharedComponentManaged<T>(NativeArray<Entity> entities, T componentData) where T : struct, ISharedComponentData
		{
			this.m_Manager.AddSharedComponentManaged<T>(entities, componentData);
		}

		// Token: 0x06000A04 RID: 2564 RVA: 0x000320BE File Offset: 0x000302BE
		public bool AddSharedComponent<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(Entity entity, T componentData) where T : struct, ValueType, ISharedComponentData
		{
			return this.m_Manager.AddSharedComponent<T>(entity, componentData);
		}

		// Token: 0x06000A05 RID: 2565 RVA: 0x000320CD File Offset: 0x000302CD
		public void AddSharedComponent<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(NativeArray<Entity> entities, T componentData) where T : struct, ValueType, ISharedComponentData
		{
			this.m_Manager.AddSharedComponent<T>(entities, componentData);
		}

		// Token: 0x06000A06 RID: 2566 RVA: 0x000320DC File Offset: 0x000302DC
		public DynamicBuffer<T> GetBuffer<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(Entity entity, bool isReadOnly = false) where T : struct, ValueType, IBufferElementData
		{
			return this.m_Manager.GetBuffer<T>(entity, isReadOnly);
		}

		// Token: 0x06000A07 RID: 2567 RVA: 0x000320EB File Offset: 0x000302EB
		public void SwapComponents(ArchetypeChunk leftChunk, int leftIndex, ArchetypeChunk rightChunk, int rightIndex)
		{
			this.m_Manager.SwapComponents(leftChunk, leftIndex, rightChunk, rightIndex);
		}

		// Token: 0x06000A08 RID: 2568 RVA: 0x000320FD File Offset: 0x000302FD
		public void SetComponentEnabled(Entity entity, ComponentType componentType, bool value)
		{
			this.m_Manager.SetComponentEnabled(entity, componentType, value);
		}

		// Token: 0x06000A09 RID: 2569 RVA: 0x0003210D File Offset: 0x0003030D
		[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[] { typeof(BurstCompatibleEnableableComponent) })]
		public void SetComponentEnabled<T>(Entity entity, bool value) where T : IEnableableComponent
		{
			this.m_Manager.SetComponentEnabled<T>(entity, value);
		}

		// Token: 0x06000A0A RID: 2570 RVA: 0x0003211C File Offset: 0x0003031C
		public bool IsComponentEnabled(Entity entity, ComponentType componentType)
		{
			return this.m_Manager.IsComponentEnabled(entity, componentType);
		}

		// Token: 0x06000A0B RID: 2571 RVA: 0x0003212B File Offset: 0x0003032B
		[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[] { typeof(BurstCompatibleEnableableComponent) })]
		public bool IsComponentEnabled<T>(Entity entity) where T : IEnableableComponent
		{
			return this.m_Manager.IsComponentEnabled<T>(entity);
		}

		// Token: 0x040004BF RID: 1215
		private EntityManager m_Manager;
	}
}
