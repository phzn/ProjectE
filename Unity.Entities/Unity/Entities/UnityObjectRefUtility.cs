using System;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Profiling;
using Unity.Properties;
using UnityEngine;

namespace Unity.Entities
{
	// Token: 0x02000288 RID: 648
	internal static class UnityObjectRefUtility
	{
		// Token: 0x0600113D RID: 4413 RVA: 0x00048CEC File Offset: 0x00046EEC
		private unsafe static void AddInstanceIDRefsFromComponent(byte* componentData, TypeManager.EntityOffsetInfo* unityObjectRefOffsets, int unityObjectRefCount, UnsafeHashSet<int>* instanceIDRefs)
		{
			for (int i = 0; i < unityObjectRefCount; i++)
			{
				int offset = unityObjectRefOffsets[i].Offset;
				UntypedUnityObjectRef* ptr = (UntypedUnityObjectRef*)(componentData + offset);
				instanceIDRefs->Add(ptr->instanceId);
			}
		}

		// Token: 0x0600113E RID: 4414 RVA: 0x00048D28 File Offset: 0x00046F28
		private unsafe static void AddInstanceIDRefsFromChunk(Archetype* archetype, int entityCount, byte* chunkBuffer, UnsafeHashSet<int>* instanceIDRefs)
		{
			int typesCount = archetype->TypesCount;
			for (int i = 0; i < typesCount; i++)
			{
				int num = archetype->TypeMemoryOrderIndexToIndexInArchetype[i];
				ComponentTypeInArchetype componentTypeInArchetype = archetype->Types[num];
				if (!componentTypeInArchetype.IsZeroSized && !componentTypeInArchetype.IsManagedComponent && !(componentTypeInArchetype.TypeIndex == ManagedComponentStore.CompanionLinkTypeIndex) && !(componentTypeInArchetype.TypeIndex == ManagedComponentStore.CompanionLinkTransformTypeIndex))
				{
					readonly ref TypeManager.TypeInfo typeInfo = ref TypeManager.GetTypeInfo(componentTypeInArchetype.TypeIndex);
					int unityObjectRefOffsetCount = typeInfo.UnityObjectRefOffsetCount;
					if (unityObjectRefOffsetCount != 0)
					{
						TypeManager.EntityOffsetInfo* unityObjectRefOffsets = TypeManager.GetUnityObjectRefOffsets(in typeInfo);
						int num2 = archetype->Offsets[num];
						byte* ptr = chunkBuffer + num2;
						if (componentTypeInArchetype.IsBuffer)
						{
							BufferHeader* ptr2 = (BufferHeader*)ptr;
							int num3 = (int)archetype->SizeOfs[num];
							int elementSize = typeInfo.ElementSize;
							for (int j = 0; j < entityCount; j++)
							{
								byte* elementPointer = BufferHeader.GetElementPointer(ptr2);
								byte* ptr3 = elementPointer + ptr2->Length * elementSize;
								for (byte* ptr4 = elementPointer; ptr4 < ptr3; ptr4 += elementSize)
								{
									UnityObjectRefUtility.AddInstanceIDRefsFromComponent(ptr4, unityObjectRefOffsets, unityObjectRefOffsetCount, instanceIDRefs);
								}
								ptr2 += num3 / sizeof(BufferHeader);
							}
						}
						else
						{
							int num4 = (int)archetype->SizeOfs[num];
							byte* ptr5 = ptr + num4 * entityCount;
							for (byte* ptr6 = ptr; ptr6 < ptr5; ptr6 += num4)
							{
								UnityObjectRefUtility.AddInstanceIDRefsFromComponent(ptr6, unityObjectRefOffsets, unityObjectRefOffsetCount, instanceIDRefs);
							}
						}
					}
				}
			}
		}

		// Token: 0x0600113F RID: 4415 RVA: 0x00048E94 File Offset: 0x00047094
		private unsafe static void AddInstanceIDRefsFromAllChunks(Archetype* archetype, UnsafeHashSet<int>* instanceIDRefs)
		{
			for (int i = 0; i < archetype->Chunks.Count; i++)
			{
				ChunkIndex chunkIndex = archetype->Chunks[i];
				byte* ptr = &chunkIndex.GetPtr()->Buffer.FixedElementField;
				UnityObjectRefUtility.AddInstanceIDRefsFromChunk(archetype, chunkIndex.Count, ptr, instanceIDRefs);
			}
		}

		// Token: 0x06001140 RID: 4416 RVA: 0x00048EE8 File Offset: 0x000470E8
		private unsafe static void AddInstanceIDRefsFromUnmanagedSharedComponents(EntityDataAccess* access, Archetype* archetype, UnsafeHashSet<int>* instanceIDRefs)
		{
			int numSharedComponents = archetype->NumSharedComponents;
			for (int i = 0; i < numSharedComponents; i++)
			{
				int* sharedComponentValueArrayForType = archetype->Chunks.GetSharedComponentValueArrayForType(i);
				for (int j = 0; j < archetype->Chunks.Count; j++)
				{
					int num = sharedComponentValueArrayForType[j];
					if (EntityComponentStore.IsUnmanagedSharedComponentIndex(num))
					{
						TypeIndex componentTypeFromSharedComponentIndex = EntityComponentStore.GetComponentTypeFromSharedComponentIndex(num);
						readonly ref TypeManager.TypeInfo typeInfo = ref TypeManager.GetTypeInfo(componentTypeFromSharedComponentIndex);
						int unityObjectRefOffsetCount = typeInfo.UnityObjectRefOffsetCount;
						if (unityObjectRefOffsetCount != 0)
						{
							TypeManager.EntityOffsetInfo* unityObjectRefOffsets = TypeManager.GetUnityObjectRefOffsets(in typeInfo);
							byte* sharedComponentDataAddr_Unmanaged = (byte*)access->EntityComponentStore->GetSharedComponentDataAddr_Unmanaged(num, componentTypeFromSharedComponentIndex);
							UnityObjectRefUtility.AddInstanceIDRefsFromComponent(sharedComponentDataAddr_Unmanaged, unityObjectRefOffsets, unityObjectRefOffsetCount, instanceIDRefs);
						}
					}
				}
			}
		}

		// Token: 0x06001141 RID: 4417 RVA: 0x00048F88 File Offset: 0x00047188
		private unsafe static void AddInstanceIDRefsFromManagedComponents(EntityDataAccess* access, UnsafeHashSet<int>* instanceIDRefs)
		{
			using (UnityObjectRefUtility.ManagedUnityObjectRefCollector managedUnityObjectRefCollector = new UnityObjectRefUtility.ManagedUnityObjectRefCollector(instanceIDRefs))
			{
				for (int i = 0; i < access->ManagedComponentStore.m_ManagedComponentData.Length; i++)
				{
					object obj = access->ManagedComponentStore.m_ManagedComponentData[i];
					if (obj != null)
					{
						readonly ref TypeManager.TypeInfo typeInfo = ref TypeManager.GetTypeInfo(TypeManager.GetTypeIndex(obj.GetType()));
						if (typeInfo.HasUnityObjectRefs && !(typeInfo.TypeIndex == ManagedComponentStore.CompanionReferenceTypeIndex))
						{
							managedUnityObjectRefCollector.CollectReferences(ref obj);
						}
					}
				}
				int sharedComponentCount = access->ManagedComponentStore.GetSharedComponentCount();
				for (int j = 0; j < sharedComponentCount; j++)
				{
					object obj2 = access->ManagedComponentStore.m_SharedComponentData[j];
					if (obj2 != null && TypeManager.GetTypeInfo(TypeManager.GetTypeIndex(obj2.GetType())).HasUnityObjectRefs)
					{
						managedUnityObjectRefCollector.CollectReferences(ref obj2);
					}
				}
			}
		}

		// Token: 0x06001142 RID: 4418 RVA: 0x0004906C File Offset: 0x0004726C
		private unsafe static void AdditionalRootsHandlerDelegate(IntPtr state)
		{
			using (UnsafeHashSet<int> unsafeHashSet = new UnsafeHashSet<int>(256, Allocator.Temp))
			{
				foreach (World world in World.s_AllWorlds)
				{
					EntityDataAccess* checkedEntityDataAccessExclusive = world.EntityManager.GetCheckedEntityDataAccessExclusive();
					checkedEntityDataAccessExclusive->CompleteAllTrackedJobs();
					for (int i = 0; i < checkedEntityDataAccessExclusive->EntityComponentStore->m_Archetypes.Length; i++)
					{
						Archetype* ptr = *(IntPtr*)(checkedEntityDataAccessExclusive->EntityComponentStore->m_Archetypes.Ptr + (IntPtr)i * (IntPtr)sizeof(Archetype*) / (IntPtr)sizeof(Archetype*));
						if (ptr->HasUnityObjectRefs)
						{
							UnityObjectRefUtility.AddInstanceIDRefsFromAllChunks(ptr, &unsafeHashSet);
							UnityObjectRefUtility.AddInstanceIDRefsFromUnmanagedSharedComponents(checkedEntityDataAccessExclusive, ptr, &unsafeHashSet);
							UnityObjectRefUtility.AddInstanceIDRefsFromManagedComponents(checkedEntityDataAccessExclusive, &unsafeHashSet);
						}
					}
				}
				if (unsafeHashSet.Count != 0)
				{
					using (NativeArray<int> nativeArray = unsafeHashSet.ToNativeArray(Allocator.Temp))
					{
						ResourcesAPIInternal.EntitiesAssetGC.MarkInstanceIDsAsRoot((IntPtr)nativeArray.GetUnsafePtr<int>(), nativeArray.Length, state);
					}
				}
			}
		}

		// Token: 0x06001143 RID: 4419 RVA: 0x000491B0 File Offset: 0x000473B0
		[RuntimeInitializeOnLoadMethod]
		private static void EditorInitializeOnLoadMethod()
		{
			ResourcesAPIInternal.EntitiesAssetGC.RegisterAdditionalRootsHandler(new ResourcesAPIInternal.EntitiesAssetGC.AdditionalRootsHandlerDelegate(UnityObjectRefUtility.AdditionalRootsHandlerDelegate));
		}

		// Token: 0x04000697 RID: 1687
		private static ProfilerMarker s_AddFromChunks = new ProfilerMarker("AddFromChunks");

		// Token: 0x04000698 RID: 1688
		private static ProfilerMarker s_AddFromUnmanagedSharedComponents = new ProfilerMarker("AddFromUnmanagaedSharedComponents");

		// Token: 0x04000699 RID: 1689
		private static ProfilerMarker s_AddFromManagedComponents = new ProfilerMarker("AddFromManagedComponents");

		// Token: 0x0400069A RID: 1690
		private static ProfilerMarker s_AddFromManagedSharedComponents = new ProfilerMarker("AddFromManagedSharedComponents");

		// Token: 0x02000289 RID: 649
		private class ManagedUnityObjectRefCollector : IPropertyBagVisitor, IPropertyVisitor, ITypedVisit<UntypedUnityObjectRef>, IDisposable
		{
			// Token: 0x06001145 RID: 4421 RVA: 0x00049201 File Offset: 0x00047401
			public unsafe ManagedUnityObjectRefCollector(UnsafeHashSet<int>* instanceIDRefs)
			{
				this.m_InstanceIDRefs = instanceIDRefs;
				this.m_References = new UnsafeHashSet<int>(256, Allocator.Temp);
			}

			// Token: 0x06001146 RID: 4422 RVA: 0x00049226 File Offset: 0x00047426
			private IPropertyBag GetPropertyBag(object obj)
			{
				return PropertyBag.GetPropertyBag(obj.GetType());
			}

			// Token: 0x06001147 RID: 4423 RVA: 0x00049233 File Offset: 0x00047433
			public void CollectReferences(ref object obj)
			{
				this.GetPropertyBag(obj).Accept(this, ref obj);
			}

			// Token: 0x06001148 RID: 4424 RVA: 0x00049244 File Offset: 0x00047444
			void IPropertyBagVisitor.Visit<TContainer>(IPropertyBag<TContainer> properties, ref TContainer container)
			{
				foreach (IProperty<TContainer> property in properties.GetProperties(ref container))
				{
					property.Accept(this, ref container);
				}
			}

			// Token: 0x06001149 RID: 4425 RVA: 0x0004929C File Offset: 0x0004749C
			void IPropertyVisitor.Visit<TContainer, TValue>(Property<TContainer, TValue> property, ref TContainer container)
			{
				if (TypeTraits<TValue>.CanBeNull || !TypeTraits<TValue>.IsValueType)
				{
					return;
				}
				TValue value = property.GetValue(ref container);
				ITypedVisit<TValue> typedVisit = this as ITypedVisit<TValue>;
				if (typedVisit != null)
				{
					typedVisit.Visit<TContainer>(property, ref container, ref value);
					return;
				}
				PropertyContainer.Accept<TValue>(this, ref value, default(VisitParameters));
			}

			// Token: 0x0600114A RID: 4426 RVA: 0x000492E6 File Offset: 0x000474E6
			unsafe void ITypedVisit<UntypedUnityObjectRef>.Visit<TContainer>(Property<TContainer, UntypedUnityObjectRef> property, ref TContainer container, ref UntypedUnityObjectRef value)
			{
				this.m_InstanceIDRefs->Add(value.instanceId);
			}

			// Token: 0x0600114B RID: 4427 RVA: 0x000492FA File Offset: 0x000474FA
			public void Dispose()
			{
				this.m_References.Dispose();
			}

			// Token: 0x0400069B RID: 1691
			private UnsafeHashSet<int> m_References;

			// Token: 0x0400069C RID: 1692
			private unsafe UnsafeHashSet<int>* m_InstanceIDRefs;
		}
	}
}
