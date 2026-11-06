using System;
using System.Runtime.CompilerServices;
using Unity.Collections.LowLevel.Unsafe;

namespace Unity.Entities
{
	// Token: 0x0200013C RID: 316
	public static class ECBExtensionMethods
	{
		// Token: 0x06000656 RID: 1622 RVA: 0x00021854 File Offset: 0x0001FA54
		public static void RegisterSingleton<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(this EntityCommandBufferSystem system, ref UnsafeList<EntityCommandBuffer> pendingBuffers, WorldUnmanaged world) where T : struct, ValueType, IECBSingleton, IComponentData
		{
			world.EntityManager.AddComponent(system.SystemHandle, ComponentType.ReadWrite<T>());
			ref T valueRW = ref new EntityQueryBuilder(system.WorldUpdateAllocator).WithAllRW<T>().WithOptions(EntityQueryOptions.IncludeSystems).Build(system)
				.GetSingletonRW<T>()
				.ValueRW;
			valueRW.SetPendingBufferList(ref pendingBuffers);
			valueRW.SetAllocator(system.m_EntityCommandBufferAllocator.Allocator.ToAllocator);
		}

		// Token: 0x06000657 RID: 1623 RVA: 0x000218DF File Offset: 0x0001FADF
		[Obsolete("The entityName parameter is obsolete. System entities are now used for ECB component data.")]
		public static void RegisterSingleton<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(this EntityCommandBufferSystem system, ref UnsafeList<EntityCommandBuffer> pendingBuffers, WorldUnmanaged world, string entityName) where T : struct, ValueType, IECBSingleton, IComponentData
		{
			system.RegisterSingleton<T>(ref pendingBuffers, world);
		}
	}
}
