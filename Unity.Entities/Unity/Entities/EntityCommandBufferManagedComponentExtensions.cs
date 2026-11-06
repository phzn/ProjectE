using System;
using System.Runtime.InteropServices;

namespace Unity.Entities
{
	// Token: 0x02000139 RID: 313
	public static class EntityCommandBufferManagedComponentExtensions
	{
		// Token: 0x0600063E RID: 1598 RVA: 0x00021375 File Offset: 0x0001F575
		[SupportedInEntitiesForEach]
		public static void AddComponent<T>(this EntityCommandBuffer ecb, Entity e, T component) where T : class
		{
			EntityCommandBufferManagedComponentExtensions.AddEntityManagedComponentCommandFromMainThread<T>(ecb.m_Data, ecb.MainThreadSortKey, ECBCommand.AddManagedComponentData, e, component);
		}

		// Token: 0x0600063F RID: 1599 RVA: 0x00021390 File Offset: 0x0001F590
		[SupportedInEntitiesForEach]
		public static void AddComponent<T>(this EntityCommandBuffer ecb, Entity e) where T : class
		{
			EntityCommandBufferManagedComponentExtensions.AddEntityManagedComponentCommandFromMainThread<T>(ecb.m_Data, ecb.MainThreadSortKey, ECBCommand.AddManagedComponentData, e, default(T));
		}

		// Token: 0x06000640 RID: 1600 RVA: 0x000213BB File Offset: 0x0001F5BB
		[SupportedInEntitiesForEach]
		public static void MoveComponent<T>(this EntityCommandBuffer ecb, Entity src, Entity dst) where T : class, IComponentData, new()
		{
			if (src == dst)
			{
				return;
			}
			EntityCommandBufferManagedComponentExtensions.AddMoveManagedComponentCommandFromMainThread<T>(ecb.m_Data, ecb.MainThreadSortKey, ECBCommand.MoveManagedComponentData, src, dst);
		}

		// Token: 0x06000641 RID: 1601 RVA: 0x000213DD File Offset: 0x0001F5DD
		[SupportedInEntitiesForEach]
		public static void SetComponent<T>(this EntityCommandBuffer ecb, Entity e, T component) where T : class, IComponentData, new()
		{
			EntityCommandBufferManagedComponentExtensions.AddEntityManagedComponentCommandFromMainThread<T>(ecb.m_Data, ecb.MainThreadSortKey, ECBCommand.SetManagedComponentData, e, component);
		}

		// Token: 0x06000642 RID: 1602 RVA: 0x000213F5 File Offset: 0x0001F5F5
		[SupportedInEntitiesForEach]
		public unsafe static void SetComponentEnabled<T>(this EntityCommandBuffer ecb, Entity e, bool value) where T : class, IEnableableComponent, new()
		{
			ecb.m_Data->AddEntityComponentEnabledCommand(&ecb.m_Data->m_MainThreadChain, ecb.MainThreadSortKey, ECBCommand.SetComponentEnabled, e, TypeManager.GetTypeIndex<T>(), value);
		}

		// Token: 0x06000643 RID: 1603 RVA: 0x0002141E File Offset: 0x0001F61E
		[SupportedInEntitiesForEach]
		[Obsolete("Use AddComponent (RemovedAfter Entities 1.0) (UnityUpgradable) -> AddComponent<T>(*)")]
		public static void AddComponentForEntityQuery<T>(this EntityCommandBuffer ecb, EntityQuery query, T component) where T : class, IComponentData, new()
		{
			ecb.AddComponent(query, component);
		}

		// Token: 0x06000644 RID: 1604 RVA: 0x00021428 File Offset: 0x0001F628
		[SupportedInEntitiesForEach]
		public static void AddComponent<T>(this EntityCommandBuffer ecb, EntityQuery query, T component) where T : class, IComponentData, new()
		{
			ecb.AddComponentObject(query, component);
		}

		// Token: 0x06000645 RID: 1605 RVA: 0x00021438 File Offset: 0x0001F638
		[SupportedInEntitiesForEach]
		[Obsolete("Use SetComponent (RemovedAfter Entities 1.0) (UnityUpgradable) -> SetComponent<T>(*)")]
		public static void SetComponentForEntityQuery<T>(this EntityCommandBuffer ecb, EntityQuery query, T component) where T : class, IComponentData, new()
		{
			ecb.SetComponent(query, component);
		}

		// Token: 0x06000646 RID: 1606 RVA: 0x00021442 File Offset: 0x0001F642
		[SupportedInEntitiesForEach]
		public static void SetComponent<T>(this EntityCommandBuffer ecb, EntityQuery query, T component) where T : class, IComponentData, new()
		{
			ecb.SetComponentObject(query, component);
		}

		// Token: 0x06000647 RID: 1607 RVA: 0x00021454 File Offset: 0x0001F654
		internal unsafe static void AddEntityManagedComponentCommandFromMainThread<T>(EntityCommandBufferData* ecbd, int sortKey, ECBCommand op, Entity e, T component) where T : class
		{
			TypeIndex typeIndex = TypeManager.GetTypeIndex<T>();
			int num = EntityCommandBufferData.Align(sizeof(EntityManagedComponentCommand), EntityCommandBufferData.ALIGN_64_BIT);
			EntityCommandBufferChain* ptr = &ecbd->m_MainThreadChain;
			ecbd->ResetCommandBatching(ptr);
			EntityManagedComponentCommand* ptr2 = (EntityManagedComponentCommand*)ecbd->Reserve(ptr, sortKey, num);
			ptr2->Header.Header.CommandType = op;
			ptr2->Header.Header.TotalSize = num;
			ptr2->Header.Header.SortKey = ptr->m_LastSortKey;
			ptr2->Header.Entity = e;
			ptr2->Header.IdentityIndex = 0;
			ptr2->Header.BatchCount = 1;
			ptr2->ComponentTypeIndex = typeIndex;
			if (component != null)
			{
				ptr2->GCNode.BoxedObject = GCHandle.Alloc(component);
				ptr2->GCNode.Prev = ptr->m_Cleanup->CleanupList;
				ptr->m_Cleanup->CleanupList = &ptr2->GCNode;
				return;
			}
			ptr2->GCNode.BoxedObject = default(GCHandle);
		}

		// Token: 0x06000648 RID: 1608 RVA: 0x00021550 File Offset: 0x0001F750
		internal unsafe static void AddMoveManagedComponentCommandFromMainThread<T>(EntityCommandBufferData* ecbd, int sortKey, ECBCommand op, Entity src, Entity dst) where T : class
		{
			TypeIndex typeIndex = TypeManager.GetTypeIndex<T>();
			int num = EntityCommandBufferData.Align(sizeof(EntityMoveManagedComponentCommand), EntityCommandBufferData.ALIGN_64_BIT);
			EntityCommandBufferChain* ptr = &ecbd->m_MainThreadChain;
			ecbd->ResetCommandBatching(ptr);
			EntityMoveManagedComponentCommand* ptr2 = (EntityMoveManagedComponentCommand*)ecbd->Reserve(ptr, sortKey, num);
			ptr2->Header.Header.CommandType = op;
			ptr2->Header.Header.TotalSize = num;
			ptr2->Header.Header.SortKey = ptr->m_LastSortKey;
			ptr2->Header.Entity = dst;
			ptr2->Header.IdentityIndex = 0;
			ptr2->Header.BatchCount = 1;
			ptr2->ComponentTypeIndex = typeIndex;
			ptr2->SrcEntity = src;
		}
	}
}
