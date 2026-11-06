using System;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;

namespace Unity.Entities
{
	// Token: 0x02000080 RID: 128
	public readonly struct EntityChangeSet : IDisposable
	{
		// Token: 0x060002CB RID: 715 RVA: 0x0000C8F4 File Offset: 0x0000AAF4
		public EntityChangeSet(int createdEntityCount, int destroyedEntityCount, int nameChangedCount, NativeArray<EntityGuid> entities, NativeArray<ComponentTypeHash> typeHashes, NativeArray<FixedString64Bytes> names, NativeArray<EntityGuid> nameChangedEntityGuids, NativeArray<PackedComponent> addComponents, NativeArray<FilteredArchetype> addArchetypes, NativeArray<PackedComponent> removeComponents, NativeArray<PackedComponentDataChange> setComponents, NativeArray<byte> componentData, NativeArray<EntityReferenceChange> entityReferenceChanges, NativeArray<BlobAssetReferenceChange> blobAssetReferenceChanges, PackedManagedComponentDataChange[] setManagedComponents, PackedSharedComponentDataChange[] setSharedComponents, UnsafeAppendBuffer unmanagedSharedComponentData, NativeArray<LinkedEntityGroupChange> linkedEntityGroupAdditions, NativeArray<LinkedEntityGroupChange> linkedEntityGroupRemovals, NativeArray<BlobAssetChange> createdBlobAssets, NativeArray<ulong> destroyedBlobAssets, NativeArray<byte> blobAssetData)
		{
			this.CreatedEntityCount = createdEntityCount;
			this.DestroyedEntityCount = destroyedEntityCount;
			this.NameChangedCount = nameChangedCount;
			this.Entities = entities;
			this.TypeHashes = typeHashes;
			this.Names = names;
			this.NameChangedEntityGuids = nameChangedEntityGuids;
			this.AddComponents = addComponents;
			this.AddArchetypes = addArchetypes;
			this.RemoveComponents = removeComponents;
			this.SetManagedComponents = setManagedComponents;
			this.SetComponents = setComponents;
			this.ComponentData = componentData;
			this.EntityReferenceChanges = entityReferenceChanges;
			this.BlobAssetReferenceChanges = blobAssetReferenceChanges;
			this.SetSharedComponents = setSharedComponents;
			this.UnmanagedSharedComponentData = unmanagedSharedComponentData;
			this.LinkedEntityGroupAdditions = linkedEntityGroupAdditions;
			this.LinkedEntityGroupRemovals = linkedEntityGroupRemovals;
			this.CreatedBlobAssets = createdBlobAssets;
			this.DestroyedBlobAssets = destroyedBlobAssets;
			this.BlobAssetData = blobAssetData;
			this.IsCreated = true;
		}

		// Token: 0x17000082 RID: 130
		// (get) Token: 0x060002CC RID: 716 RVA: 0x0000C9B5 File Offset: 0x0000ABB5
		public bool IsCreated { get; }

		// Token: 0x17000083 RID: 131
		// (get) Token: 0x060002CD RID: 717 RVA: 0x0000C9BD File Offset: 0x0000ABBD
		public bool HasChanges
		{
			get
			{
				return this.HasChangesIncludeNames(false, false);
			}
		}

		// Token: 0x060002CE RID: 718 RVA: 0x0000C9C8 File Offset: 0x0000ABC8
		internal bool HasChangesIncludeNames(bool ignoreNameChangeCount = false, bool ignoreData = false)
		{
			bool flag = this.CreatedEntityCount != 0 || this.DestroyedEntityCount != 0 || this.AddComponents.Length != 0 || this.AddArchetypes.Length != 0 || this.RemoveComponents.Length != 0 || this.SetComponents.Length != 0 || this.EntityReferenceChanges.Length != 0 || this.BlobAssetReferenceChanges.Length != 0 || this.SetManagedComponents.Length != 0 || this.SetSharedComponents.Length != 0 || this.LinkedEntityGroupAdditions.Length != 0 || this.LinkedEntityGroupRemovals.Length != 0 || this.CreatedBlobAssets.Length != 0 || this.DestroyedBlobAssets.Length != 0;
			if (!ignoreData)
			{
				flag = flag || this.ComponentData.Length != 0 || this.BlobAssetData.Length != 0;
			}
			if (!ignoreNameChangeCount)
			{
				flag = flag || this.NameChangedCount != 0;
			}
			return flag;
		}

		// Token: 0x060002CF RID: 719 RVA: 0x0000CAFC File Offset: 0x0000ACFC
		public unsafe void Dispose()
		{
			if (!this.IsCreated)
			{
				return;
			}
			foreach (PackedSharedComponentDataChange packedSharedComponentDataChange in this.SetSharedComponents)
			{
				if ((packedSharedComponentDataChange.UnmanagedSharedValueDataOffsetWithManagedFlag & -2147483648) != 0)
				{
					IRefCounted refCounted = packedSharedComponentDataChange.BoxedSharedValue as IRefCounted;
					if (refCounted != null)
					{
						refCounted.Release();
					}
				}
				else
				{
					TypeIndex typeIndexFromStableTypeHash = TypeManager.GetTypeIndexFromStableTypeHash(this.TypeHashes[packedSharedComponentDataChange.Component.PackedTypeIndex].StableTypeHash);
					if (typeIndexFromStableTypeHash.IsRefCounted)
					{
						IRefCounted refCounted2 = TypeManager.ConstructComponentFromBuffer(typeIndexFromStableTypeHash, (void*)(this.UnmanagedSharedComponentData.Ptr + (packedSharedComponentDataChange.UnmanagedSharedValueDataOffsetWithManagedFlag & int.MaxValue))) as IRefCounted;
						if (refCounted2 != null)
						{
							refCounted2.Release();
						}
					}
				}
			}
			int companionLinkPackedTypeIndex = EntityChangeSet.GetCompanionLinkPackedTypeIndex(this.TypeHashes);
			foreach (PackedManagedComponentDataChange packedManagedComponentDataChange in this.SetManagedComponents)
			{
				if (packedManagedComponentDataChange.Component.PackedTypeIndex != companionLinkPackedTypeIndex)
				{
					IDisposable disposable = packedManagedComponentDataChange.BoxedValue as IDisposable;
					if (disposable != null)
					{
						disposable.Dispose();
					}
				}
			}
			this.Entities.Dispose();
			this.TypeHashes.Dispose();
			this.Names.Dispose();
			this.NameChangedEntityGuids.Dispose();
			this.AddComponents.Dispose();
			foreach (FilteredArchetype filteredArchetype in this.AddArchetypes)
			{
				UnsafeList<TypeIndex> typeIndices = filteredArchetype.TypeIndices;
				typeIndices.Dispose();
				UnsafeList<int> packedEntityIndices = filteredArchetype.PackedEntityIndices;
				packedEntityIndices.Dispose();
			}
			this.AddArchetypes.Dispose();
			this.RemoveComponents.Dispose();
			this.SetComponents.Dispose();
			this.ComponentData.Dispose();
			this.EntityReferenceChanges.Dispose();
			this.BlobAssetReferenceChanges.Dispose();
			this.LinkedEntityGroupAdditions.Dispose();
			this.LinkedEntityGroupRemovals.Dispose();
			this.CreatedBlobAssets.Dispose();
			this.DestroyedBlobAssets.Dispose();
			this.BlobAssetData.Dispose();
			if (this.UnmanagedSharedComponentData.IsCreated)
			{
				this.UnmanagedSharedComponentData.Dispose();
			}
		}

		// Token: 0x060002D0 RID: 720 RVA: 0x0000CD80 File Offset: 0x0000AF80
		internal static ulong GetCompanionLinkStableTypeHash()
		{
			if (EntityChangeSet.CompanionLinkStableTypeHash != 0UL)
			{
				return EntityChangeSet.CompanionLinkStableTypeHash;
			}
			EntityChangeSet.CompanionLinkStableTypeHash = TypeManager.GetTypeInfo(ManagedComponentStore.CompanionReferenceTypeIndex).StableTypeHash;
			return EntityChangeSet.CompanionLinkStableTypeHash;
		}

		// Token: 0x060002D1 RID: 721 RVA: 0x0000CDA8 File Offset: 0x0000AFA8
		internal static int GetCompanionLinkPackedTypeIndex(NativeArray<ComponentTypeHash> typeHashes)
		{
			ulong companionLinkStableTypeHash = EntityChangeSet.GetCompanionLinkStableTypeHash();
			for (int i = 0; i < typeHashes.Length; i++)
			{
				if (typeHashes[i].StableTypeHash == companionLinkStableTypeHash)
				{
					return i;
				}
			}
			return -1;
		}

		// Token: 0x040000DB RID: 219
		public readonly int CreatedEntityCount;

		// Token: 0x040000DC RID: 220
		public readonly int DestroyedEntityCount;

		// Token: 0x040000DD RID: 221
		public readonly int NameChangedCount;

		// Token: 0x040000DE RID: 222
		public readonly NativeArray<EntityGuid> Entities;

		// Token: 0x040000DF RID: 223
		public readonly NativeArray<ComponentTypeHash> TypeHashes;

		// Token: 0x040000E0 RID: 224
		public readonly NativeArray<FixedString64Bytes> Names;

		// Token: 0x040000E1 RID: 225
		public readonly NativeArray<EntityGuid> NameChangedEntityGuids;

		// Token: 0x040000E2 RID: 226
		public readonly NativeArray<PackedComponent> AddComponents;

		// Token: 0x040000E3 RID: 227
		public readonly NativeArray<FilteredArchetype> AddArchetypes;

		// Token: 0x040000E4 RID: 228
		public readonly NativeArray<PackedComponent> RemoveComponents;

		// Token: 0x040000E5 RID: 229
		public readonly NativeArray<PackedComponentDataChange> SetComponents;

		// Token: 0x040000E6 RID: 230
		public readonly NativeArray<byte> ComponentData;

		// Token: 0x040000E7 RID: 231
		public readonly NativeArray<EntityReferenceChange> EntityReferenceChanges;

		// Token: 0x040000E8 RID: 232
		public readonly NativeArray<BlobAssetReferenceChange> BlobAssetReferenceChanges;

		// Token: 0x040000E9 RID: 233
		public readonly PackedManagedComponentDataChange[] SetManagedComponents;

		// Token: 0x040000EA RID: 234
		public readonly PackedSharedComponentDataChange[] SetSharedComponents;

		// Token: 0x040000EB RID: 235
		public readonly UnsafeAppendBuffer UnmanagedSharedComponentData;

		// Token: 0x040000EC RID: 236
		public readonly NativeArray<LinkedEntityGroupChange> LinkedEntityGroupAdditions;

		// Token: 0x040000ED RID: 237
		public readonly NativeArray<LinkedEntityGroupChange> LinkedEntityGroupRemovals;

		// Token: 0x040000EE RID: 238
		public readonly NativeArray<BlobAssetChange> CreatedBlobAssets;

		// Token: 0x040000EF RID: 239
		public readonly NativeArray<ulong> DestroyedBlobAssets;

		// Token: 0x040000F0 RID: 240
		public readonly NativeArray<byte> BlobAssetData;

		// Token: 0x040000F2 RID: 242
		private static ulong CompanionLinkStableTypeHash;
	}
}
