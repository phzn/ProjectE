using System;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Entities;

// Token: 0x02000005 RID: 5
internal struct EntityNameStoreAccess : IDisposable
{
	// Token: 0x17000001 RID: 1
	// (get) Token: 0x06000003 RID: 3 RVA: 0x00002058 File Offset: 0x00000258
	public unsafe ulong NameChangeBitsSequenceNum
	{
		get
		{
			return this.m_Data->m_NameChangeBitsSequenceNum;
		}
	}

	// Token: 0x06000004 RID: 4 RVA: 0x00002065 File Offset: 0x00000265
	public unsafe EntityNameStoreAccess(EntityComponentStore* componentStore)
	{
		this.m_EntityComponentStore = componentStore;
		this.m_Data = Memory.Unmanaged.Allocate<EntityNameStoreAccessData>(Allocator.Persistent);
		this.m_Data->m_NameChangeBitsSequenceNum = 1UL;
		this.m_EntitiesNameSet = new UnsafeHashSet<Entity>(1000, Allocator.Persistent);
	}

	// Token: 0x17000002 RID: 2
	// (get) Token: 0x06000005 RID: 5 RVA: 0x000020A2 File Offset: 0x000002A2
	public bool IsCreated
	{
		get
		{
			return this.m_Data != null;
		}
	}

	// Token: 0x06000006 RID: 6 RVA: 0x000020B1 File Offset: 0x000002B1
	public void Dispose()
	{
		Memory.Unmanaged.Free<EntityNameStoreAccessData>(this.m_Data, Allocator.Persistent);
		this.m_EntitiesNameSet.Dispose();
		this.m_Data = null;
		this.m_EntityComponentStore = null;
	}

	// Token: 0x06000007 RID: 7 RVA: 0x000020DF File Offset: 0x000002DF
	public unsafe ulong IncNameChangeBitsVersion()
	{
		this.m_Data->m_NameChangeBitsSequenceNum += 1UL;
		return this.m_Data->m_NameChangeBitsSequenceNum;
	}

	// Token: 0x06000008 RID: 8 RVA: 0x000020FD File Offset: 0x000002FD
	public unsafe void SetNameChangeBitsVersion(ulong nameChangeBitsVersion)
	{
		this.m_Data->m_NameChangeBitsSequenceNum = nameChangeBitsVersion;
	}

	// Token: 0x06000009 RID: 9 RVA: 0x0000210B File Offset: 0x0000030B
	public EntityName GetEntityNameByEntityIndex(int index)
	{
		return EntityComponentStore.s_entityStore.Data.GetEntityName(index);
	}

	// Token: 0x0600000A RID: 10 RVA: 0x0000211D File Offset: 0x0000031D
	public EntityName GetEntityName(Entity entity)
	{
		return EntityComponentStore.s_entityStore.Data.GetEntityName(entity);
	}

	// Token: 0x0600000B RID: 11 RVA: 0x0000212F File Offset: 0x0000032F
	public void SetEntityName(Entity entity, EntityName name)
	{
		EntityComponentStore.s_entityStore.Data.SetEntityName(entity, name);
	}

	// Token: 0x0600000C RID: 12 RVA: 0x00002142 File Offset: 0x00000342
	public int CountEntitiesWithNamesSet()
	{
		return this.m_EntitiesNameSet.Count;
	}

	// Token: 0x0600000D RID: 13 RVA: 0x0000214F File Offset: 0x0000034F
	public void ResetEntitiesWithNamesSet()
	{
		this.m_EntitiesNameSet.Clear();
	}

	// Token: 0x0600000E RID: 14 RVA: 0x0000215C File Offset: 0x0000035C
	public void AddEntityWithNameSet(Entity entity)
	{
		this.m_EntitiesNameSet.Add(entity);
	}

	// Token: 0x0600000F RID: 15 RVA: 0x0000216B File Offset: 0x0000036B
	public void RemoveEntityWithNameSet(Entity entity)
	{
		this.m_EntitiesNameSet.Remove(entity);
	}

	// Token: 0x06000010 RID: 16 RVA: 0x0000217C File Offset: 0x0000037C
	public unsafe void RemoveEntityWithNameSet(Entity* entities, int count)
	{
		for (int i = 0; i < count; i++)
		{
			this.m_EntitiesNameSet.Remove(entities[i]);
		}
	}

	// Token: 0x06000011 RID: 17 RVA: 0x000021B1 File Offset: 0x000003B1
	public UnsafeHashSet<Entity>.ReadOnly GetEntityWithNameSetRO()
	{
		return this.m_EntitiesNameSet.AsReadOnly();
	}

	// Token: 0x04000002 RID: 2
	public const ulong InitialNameChangeBitsSequenceNum = 1UL;

	// Token: 0x04000003 RID: 3
	[NativeDisableUnsafePtrRestriction]
	private unsafe EntityComponentStore* m_EntityComponentStore;

	// Token: 0x04000004 RID: 4
	private UnsafeHashSet<Entity> m_EntitiesNameSet;

	// Token: 0x04000005 RID: 5
	[NativeDisableUnsafePtrRestriction]
	private unsafe EntityNameStoreAccessData* m_Data;
}
