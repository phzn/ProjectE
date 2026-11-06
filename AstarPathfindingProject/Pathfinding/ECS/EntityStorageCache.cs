using System;
using System.Runtime.CompilerServices;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Entities;

namespace Pathfinding.ECS
{
	// Token: 0x02000310 RID: 784
	public struct EntityStorageCache
	{
		// Token: 0x06001322 RID: 4898 RVA: 0x00073B38 File Offset: 0x00071D38
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool Update(World world, Entity entity, out EntityManager entityManager, out EntityStorageInfo storage)
		{
			storage = this.storage;
			if (world == null || !world.IsCreated)
			{
				entityManager = default(EntityManager);
				return false;
			}
			entityManager = world.EntityManager;
			int num = entityManager.EntityOrderVersion ^ ((int)world.SequenceNumber << 8);
			if (num != this.lastWorldHash || entity != this.entity)
			{
				if (!entityManager.Exists(entity))
				{
					return false;
				}
				this.storage = (storage = entityManager.GetStorageInfo(entity));
				this.entity = entity;
				this.lastWorldHash = num;
			}
			return true;
		}

		// Token: 0x06001323 RID: 4899 RVA: 0x00073BCC File Offset: 0x00071DCC
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public unsafe bool GetComponentData<[IsUnmanaged] A>(World world, Entity entity, ref EntityAccess<A> access, out ComponentRef<A> value) where A : struct, ValueType, IComponentData
		{
			EntityManager entityManager;
			EntityStorageInfo entityStorageInfo;
			if (this.Update(world, entity, out entityManager, out entityStorageInfo))
			{
				access.Update(entityManager);
				value = new ComponentRef<A>((byte*)UnsafeUtility.AddressOf<A>(access[entityStorageInfo]));
				return true;
			}
			value = default(ComponentRef<A>);
			return false;
		}

		// Token: 0x06001324 RID: 4900 RVA: 0x00073C14 File Offset: 0x00071E14
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool GetComponentData<A>(World world, Entity entity, ref ManagedEntityAccess<A> access, out A value) where A : class, IComponentData
		{
			EntityManager entityManager;
			EntityStorageInfo entityStorageInfo;
			if (this.Update(world, entity, out entityManager, out entityStorageInfo))
			{
				access.Update(entityManager);
				value = access[entityStorageInfo];
				return true;
			}
			value = default(A);
			return false;
		}

		// Token: 0x04000E08 RID: 3592
		private EntityStorageInfo storage;

		// Token: 0x04000E09 RID: 3593
		private Entity entity;

		// Token: 0x04000E0A RID: 3594
		private int lastWorldHash;
	}
}
