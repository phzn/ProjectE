using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using Unity.Burst;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.IO.LowLevel.Unsafe;
using Unity.Mathematics;
using Unity.Serialization.Binary;
using UnityEngine;

namespace Unity.Entities.Serialization
{
	// Token: 0x02000394 RID: 916
	public static class SerializeUtility
	{
		// Token: 0x0600184E RID: 6222 RVA: 0x0005B05C File Offset: 0x0005925C
		private static UnsafeList<EntityArchetype> ReadArchetypes(BinaryReader reader, NativeArray<TypeIndex> types, ExclusiveEntityTransaction entityManager, out int totalEntityCount)
		{
			int num = reader.ReadInt();
			UnsafeList<EntityArchetype> unsafeList = new UnsafeList<EntityArchetype>(num, Allocator.Temp, NativeArrayOptions.UninitializedMemory);
			totalEntityCount = 0;
			NativeList<ComponentType> nativeList = new NativeList<ComponentType>(Allocator.Temp);
			for (int i = 0; i < num; i++)
			{
				int num2 = reader.ReadInt();
				totalEntityCount += num2;
				int num3 = reader.ReadInt();
				nativeList.Clear();
				for (int j = 0; j < num3; j++)
				{
					TypeIndex typeIndex = new TypeIndex
					{
						Value = reader.ReadInt()
					};
					int index = typeIndex.Index;
					TypeIndex typeIndex2 = types[index];
					if (TypeManager.IsChunkComponent(typeIndex))
					{
						typeIndex2 = TypeManager.MakeChunkComponentTypeIndex(typeIndex2);
					}
					ComponentType componentType = ComponentType.FromTypeIndex(typeIndex2);
					nativeList.Add(in componentType);
				}
				EntityArchetype entityArchetype = entityManager.EntityManager.CreateArchetypeWithoutSimulateComponent(nativeList.GetUnsafePtr<ComponentType>(), nativeList.Length);
				unsafeList.Add(in entityArchetype);
			}
			nativeList.Dispose();
			return unsafeList;
		}

		// Token: 0x0600184F RID: 6223 RVA: 0x0005B154 File Offset: 0x00059354
		private static NativeArray<TypeIndex> ReadTypeArray(BinaryReader reader, DotsSerializationReader dotsReader)
		{
			SerializeUtility.<>c__DisplayClass10_0 CS$<>8__locals1;
			CS$<>8__locals1.dotsReader = dotsReader;
			CS$<>8__locals1.isNameLoaded = false;
			CS$<>8__locals1.hasTypesName = false;
			CS$<>8__locals1.namesFromStableHash = default(UnsafeParallelHashMap<ulong, int>);
			CS$<>8__locals1.stringStable = default(StringTableReaderHandle);
			int num = reader.ReadInt();
			NativeArray<TypeIndex> nativeArray = new NativeArray<TypeIndex>(num, Allocator.Temp, NativeArrayOptions.ClearMemory);
			int i = 0;
			while (i < num)
			{
				ulong num2 = reader.ReadULong();
				TypeIndex typeIndexFromStableTypeHash = TypeManager.GetTypeIndexFromStableTypeHash(num2);
				if (typeIndexFromStableTypeHash == TypeIndex.Null)
				{
					FixedString512Bytes fixedString512Bytes;
					if (!SerializeUtility.<ReadTypeArray>g__GetTypeName|10_0(num2, out fixedString512Bytes, ref CS$<>8__locals1))
					{
						throw new ArgumentException(string.Concat(new string[]
						{
							"Cannot find TypeIndex for type hash ",
							num2.ToString(),
							". Check in the debug file ExportedTypes.log of your project Logs folder (<projectName>/Logs) the corresponding Component type name for the type hash ",
							num2.ToString(),
							". And ensure your runtime depends on all assemblies defining the Component types your data uses."
						}));
					}
					throw new ArgumentException(string.Format("Cannot find Type {0} for type hash {1}. Ensure your runtime depends on all assemblies defining the Component types your data uses.", fixedString512Bytes, num2.ToString()));
				}
				else
				{
					nativeArray[i] = typeIndexFromStableTypeHash;
					i++;
				}
			}
			if (CS$<>8__locals1.hasTypesName)
			{
				CS$<>8__locals1.namesFromStableHash.Dispose();
				CS$<>8__locals1.stringStable.Dispose();
			}
			return nativeArray;
		}

		// Token: 0x06001850 RID: 6224 RVA: 0x0005B268 File Offset: 0x00059468
		internal unsafe static UnsafePtrList<Archetype> GetAllArchetypes(EntityComponentStore* entityComponentStore, AllocatorManager.AllocatorHandle allocator)
		{
			int num = 0;
			for (int i = 0; i < entityComponentStore->m_Archetypes.Length; i++)
			{
				Archetype* ptr = *(IntPtr*)(entityComponentStore->m_Archetypes.Ptr + (IntPtr)i * (IntPtr)sizeof(Archetype*) / (IntPtr)sizeof(Archetype*));
				if (ptr->EntityCount > 0 && !ptr->HasSystemInstanceComponents)
				{
					num++;
				}
			}
			UnsafePtrList<Archetype> unsafePtrList = new UnsafePtrList<Archetype>(num, allocator, NativeArrayOptions.UninitializedMemory);
			unsafePtrList.Resize(num, NativeArrayOptions.UninitializedMemory);
			num = 0;
			for (int j = 0; j < entityComponentStore->m_Archetypes.Length; j++)
			{
				Archetype* ptr2 = *(IntPtr*)(entityComponentStore->m_Archetypes.Ptr + (IntPtr)j * (IntPtr)sizeof(Archetype*) / (IntPtr)sizeof(Archetype*));
				if (ptr2->EntityCount > 0 && !ptr2->HasSystemInstanceComponents)
				{
					*(IntPtr*)(unsafePtrList.Ptr + (IntPtr)(num++) * (IntPtr)sizeof(Archetype*)) = *(IntPtr*)(entityComponentStore->m_Archetypes.Ptr + (IntPtr)j * (IntPtr)sizeof(Archetype*) / (IntPtr)sizeof(Archetype*));
				}
			}
			NativeSortExtension.Sort<IntPtr, StableArchetypeCompare>((IntPtr*)unsafePtrList.Ptr, unsafePtrList.Length, default(StableArchetypeCompare));
			return unsafePtrList;
		}

		// Token: 0x06001851 RID: 6225 RVA: 0x0005B360 File Offset: 0x00059560
		public static void SerializeWorld(EntityManager entityManager, BinaryWriter writer)
		{
			int num = entityManager.HighestEntityIndex() + 1;
			NativeArray<EntityRemapUtility.EntityRemapInfo> nativeArray = new NativeArray<EntityRemapUtility.EntityRemapInfo>(num, Allocator.Temp, NativeArrayOptions.ClearMemory);
			SerializeUtility.SerializeWorldInternal(entityManager, writer, default(UnityObjectRefMap), nativeArray, default(NativeParallelHashSet<UntypedWeakReferenceId>), SerializeUtility.Settings.Default, false);
			nativeArray.Dispose();
		}

		// Token: 0x06001852 RID: 6226 RVA: 0x0005B3AC File Offset: 0x000595AC
		public static void SerializeWorld(EntityManager entityManager, BinaryWriter writer, out object[] referencedObjects)
		{
			int num = entityManager.HighestEntityIndex() + 1;
			NativeArray<EntityRemapUtility.EntityRemapInfo> nativeArray = new NativeArray<EntityRemapUtility.EntityRemapInfo>(num, Allocator.Temp, NativeArrayOptions.ClearMemory);
			UnityObjectRefMap unityObjectRefMap = new UnityObjectRefMap(Allocator.Temp);
			SerializeUtility.SerializeWorldInternal(entityManager, writer, unityObjectRefMap, nativeArray, default(NativeParallelHashSet<UntypedWeakReferenceId>), SerializeUtility.Settings.Default, false);
			object[] array = unityObjectRefMap.ToObjectArray();
			referencedObjects = array;
			nativeArray.Dispose();
			unityObjectRefMap.Dispose();
		}

		// Token: 0x06001853 RID: 6227 RVA: 0x0005B40C File Offset: 0x0005960C
		public static void SerializeWorld(EntityManager entityManager, BinaryWriter writer, NativeArray<EntityRemapUtility.EntityRemapInfo> entityRemapInfos)
		{
			SerializeUtility.SerializeWorldInternal(entityManager, writer, default(UnityObjectRefMap), entityRemapInfos, default(NativeParallelHashSet<UntypedWeakReferenceId>), SerializeUtility.Settings.Default, false);
		}

		// Token: 0x06001854 RID: 6228 RVA: 0x0005B43C File Offset: 0x0005963C
		public static void SerializeWorld(EntityManager entityManager, BinaryWriter writer, out object[] referencedObjects, NativeArray<EntityRemapUtility.EntityRemapInfo> entityRemapInfos)
		{
			UnityObjectRefMap unityObjectRefMap = new UnityObjectRefMap(Allocator.Temp);
			SerializeUtility.SerializeWorldInternal(entityManager, writer, unityObjectRefMap, entityRemapInfos, default(NativeParallelHashSet<UntypedWeakReferenceId>), SerializeUtility.Settings.Default, false);
			object[] array = unityObjectRefMap.ToObjectArray();
			referencedObjects = array;
			unityObjectRefMap.Dispose();
		}

		// Token: 0x06001855 RID: 6229 RVA: 0x0005B47C File Offset: 0x0005967C
		public static Entity GetSceneSectionEntity(int sectionIndex, EntityManager manager, ref EntityQuery cachedSceneSectionEntityQuery, bool createIfMissing = true)
		{
			if (cachedSceneSectionEntityQuery == default(EntityQuery))
			{
				EntityQueryBuilder entityQueryBuilder = new EntityQueryBuilder(Allocator.Temp).WithAll<SectionMetadataSetup>();
				cachedSceneSectionEntityQuery = manager.CreateEntityQuery(in entityQueryBuilder);
			}
			SectionMetadataSetup sectionMetadataSetup = new SectionMetadataSetup
			{
				SceneSectionIndex = sectionIndex
			};
			cachedSceneSectionEntityQuery.SetSharedComponentFilter<SectionMetadataSetup>(sectionMetadataSetup);
			Entity entity;
			using (NativeArray<Entity> nativeArray = cachedSceneSectionEntityQuery.ToEntityArray(Allocator.TempJob))
			{
				if (nativeArray.Length == 0)
				{
					if (!createIfMissing)
					{
						entity = Entity.Null;
					}
					else
					{
						Entity entity2 = manager.CreateEntity();
						manager.AddSharedComponent<SectionMetadataSetup>(entity2, sectionMetadataSetup);
						entity = entity2;
					}
				}
				else
				{
					if (nativeArray.Length != 1)
					{
						throw new InvalidOperationException(string.Format("Multiple scene section entities with section index {0} found", sectionIndex));
					}
					entity = nativeArray[0];
				}
			}
			return entity;
		}

		// Token: 0x06001856 RID: 6230 RVA: 0x0005B568 File Offset: 0x00059768
		private unsafe static void FillReadCommands(DotsSerializationReader dotsReader, UnsafeList<ReadCommand>* readCommands, out SerializeUtility.WorldDeserializationStatus status)
		{
			status = default(SerializeUtility.WorldDeserializationStatus);
			DotsSerializationReader.NodeHandle nodeHandle = dotsReader.RootNode.FindNode(SerializeUtility.WorldNodeType, int.MaxValue);
			DotsSerializationReader.NodeHandle nodeHandle2 = nodeHandle.FindNode(SerializeUtility.ArchetypesNodeType, int.MaxValue);
			if (!nodeHandle2.IsValid)
			{
				throw new InvalidOperationException("Binary entity scene file must contain a ArchetypesNodeType");
			}
			ReadCommand readCommand;
			status.ArchetypePrefetchState = nodeHandle2.PrefetchRawDataRead(out readCommand);
			readCommands->Add(in readCommand);
			DotsSerializationReader.NodeHandle nodeHandle3;
			if (nodeHandle.TryFindNode(SerializeUtility.BlobAssetsNodeType, out nodeHandle3, 2147483647))
			{
				status.BlobAssetSize = (int)nodeHandle3.DataSize;
				status.BlobAssetBuffer = Memory.Unmanaged.Allocate((long)status.BlobAssetSize, 16, Allocator.Persistent);
				ReadCommand readCommand2 = default(ReadCommand);
				readCommand2.Buffer = status.BlobAssetBuffer;
				readCommand2.Offset = nodeHandle3.DataStartingOffset;
				readCommand2.Size = (long)status.BlobAssetSize;
				readCommands->Add(in readCommand2);
			}
			DotsSerializationReader.NodeHandle nodeHandle4 = nodeHandle.FindNode(SerializeUtility.SharedAndManagedComponentsNodeType, int.MaxValue);
			if (!nodeHandle4.IsValid)
			{
				throw new InvalidOperationException("Binary entity scene file must contain a SharedAndManagedComponentsNodeType");
			}
			ReadCommand readCommand3;
			status.SharedComponentPrefetchState = nodeHandle4.PrefetchRawDataRead(out readCommand3);
			readCommands->Add(in readCommand3);
			DotsSerializationReader.NodeHandle nodeHandle5 = nodeHandle.FindNode(SerializeUtility.EnabledBitsNodeType, int.MaxValue);
			if (!nodeHandle5.IsValid)
			{
				throw new InvalidOperationException("Binary entity scene file must contain an EnabledBitsNodeType");
			}
			ReadCommand readCommand4;
			status.EnabledBitsPrefetchState = nodeHandle5.PrefetchRawDataRead(out readCommand4);
			readCommands->Add(in readCommand4);
			DotsSerializationReader.NodeHandle nodeHandle6 = nodeHandle.FindNode(SerializeUtility.ChunksNodeType, int.MaxValue);
			if (!nodeHandle6.IsValid)
			{
				throw new InvalidOperationException("Binary entity scene file must contain a ChunksNodeType");
			}
			int num = (int)(nodeHandle6.DataSize / 16384L);
			long dataStartingOffset = nodeHandle6.DataStartingOffset;
			UnsafeList<SerializeUtility.MegaChunkInfo> unsafeList = new UnsafeList<SerializeUtility.MegaChunkInfo>(8, Allocator.Persistent, NativeArrayOptions.UninitializedMemory);
			if (SerializeUtilityInterop.AllocAndQueueReadChunkCommands(dataStartingOffset, num, &unsafeList, readCommands) != num)
			{
				throw new Exception("Contiguous Chunk allocation failed");
			}
			status.MegaChunkInfoList = unsafeList;
			status.TotalChunkCount = num;
			DotsSerializationReader.NodeHandle nodeHandle7 = nodeHandle.FindNode(SerializeUtility.BufferDataNodeType, int.MaxValue);
			if (!nodeHandle7.IsValid)
			{
				throw new InvalidOperationException("Binary entity scene file must contain a BufferDataNodeType");
			}
			ReadCommand readCommand5;
			status.BufferElementPrefetchState = nodeHandle7.PrefetchRawDataRead(out readCommand5);
			readCommands->Add(in readCommand5);
			DotsSerializationReader.NodeHandle nodeHandle8;
			if (nodeHandle.TryFindNode(SerializeUtility.PrefabNodeType, out nodeHandle8, 2147483647))
			{
				ReadCommand readCommand6;
				status.PrefabPrefetchState = nodeHandle8.PrefetchRawDataRead(out readCommand6);
				readCommands->Add(in readCommand6);
			}
		}

		// Token: 0x06001857 RID: 6231 RVA: 0x0005B7A8 File Offset: 0x000599A8
		internal unsafe static ReadHandle BeginDeserializeWorld(string serializationFilePathName, DotsSerializationReader dotsReader, out SerializeUtility.WorldDeserializationStatus status, out UnsafeList<ReadCommand> readCommands)
		{
			UnsafeList<ReadCommand> unsafeList = new UnsafeList<ReadCommand>(1, Allocator.Persistent, NativeArrayOptions.UninitializedMemory);
			SerializeUtility.FillReadCommands(dotsReader, &unsafeList, out status);
			readCommands = unsafeList;
			return AsyncReadManager.Read(serializationFilePathName, readCommands.Ptr, (uint)readCommands.Length, "", 0UL, AssetLoadingSubsystem.Scripts);
		}

		// Token: 0x06001858 RID: 6232 RVA: 0x0005B7F0 File Offset: 0x000599F0
		internal unsafe static void EndDeserializeWorld(ExclusiveEntityTransaction manager, DotsSerializationReader dotsReader, ref SerializeUtility.WorldDeserializationStatus status, out SerializeUtility.WorldDeserializationResult deserializationResult, int externalEntitiesRefRange, int sceneSectionIndex, NativeArray<int> unityObjects)
		{
			deserializationResult = default(SerializeUtility.WorldDeserializationResult);
			EntityDataAccess* checkedEntityDataAccess = manager.EntityManager.GetCheckedEntityDataAccess();
			EntityComponentStore* entityComponentStore = checkedEntityDataAccess->EntityComponentStore;
			ManagedComponentStore managedComponentStore = checkedEntityDataAccess->ManagedComponentStore;
			if (entityComponentStore->CountEntities() != 0)
			{
				using (EntityQuery entityQuery = manager.EntityManager.CreateEntityQuery(new ComponentType[] { ComponentType.ReadWrite<SystemInstance>() }))
				{
					if (!entityQuery.IsEmptyIgnoreFilter)
					{
						SerializeUtility.<EndDeserializeWorld>g__CleanupWhenDeserializationIsInterrupted|34_0(ref status);
						throw new ArgumentException("DeserializeWorld can only be used on completely empty World. Please create a new empty World and add Systems after loading is complete instead.");
					}
				}
				if (entityComponentStore->CountEntities() != 0)
				{
					SerializeUtility.<EndDeserializeWorld>g__CleanupWhenDeserializationIsInterrupted|34_0(ref status);
					throw new ArgumentException("DeserializeWorld can only be used on completely empty EntityManager. Please create a new empty World and use EntityManager.MoveEntitiesFrom to move the loaded entities into the destination world instead.");
				}
			}
			NativeList<int> nativeList = new NativeList<int>(Allocator.Temp);
			BlobAssetOwner blobAssetOwner = default(BlobAssetOwner);
			NativeArray<TypeIndex> nativeArray;
			UnsafeList<EntityArchetype> unsafeList;
			using (MemoryBinaryReader memoryBinaryReader = status.ArchetypePrefetchState.CreateStream())
			{
				nativeArray = SerializeUtility.ReadTypeArray(memoryBinaryReader, dotsReader);
				int num;
				unsafeList = SerializeUtility.ReadArchetypes(memoryBinaryReader, nativeArray, manager, out num);
			}
			int blobAssetSize = status.BlobAssetSize;
			void* blobAssetBuffer = status.BlobAssetBuffer;
			if (blobAssetSize != 0)
			{
				blobAssetOwner = new BlobAssetOwner(blobAssetBuffer, status.BlobAssetSize);
			}
			UnsafeList<ArchetypeChunk> unsafeList2 = new UnsafeList<ArchetypeChunk>(32, Allocator.Temp, NativeArrayOptions.UninitializedMemory);
			MemoryBinaryReader memoryBinaryReader2 = status.SharedComponentPrefetchState.CreateStream();
			int num2 = memoryBinaryReader2.ReadInt();
			NativeArray<int> nativeArray2 = new NativeArray<int>(num2, Allocator.Temp, NativeArrayOptions.ClearMemory);
			memoryBinaryReader2.ReadArray<int>(nativeArray2, num2);
			int num3 = 0;
			nativeList.Add(in num3);
			int num4 = SerializeUtility.ReadUnmanagedSharedComponents(manager, memoryBinaryReader2, nativeList, (SerializeUtility.WorldDeserializationStatus*)UnsafeUtility.AddressOf<SerializeUtility.WorldDeserializationStatus>(ref status), unityObjects);
			int num5 = SerializeUtility.ReadManagedSharedComponents(manager, memoryBinaryReader2, nativeList, unityObjects, blobAssetBuffer);
			int num6 = num4 + num5;
			memoryBinaryReader2.Dispose();
			byte* ptr = (byte*)status.EnabledBitsPrefetchState._buffer;
			int num7 = *(int*)ptr;
			ptr += 4;
			int* ptr2 = (int*)(ptr + num7 + 4);
			int totalChunkCount = status.TotalChunkCount;
			UnsafeList<ArchetypeChunk> unsafeList3 = new UnsafeList<ArchetypeChunk>(totalChunkCount, Allocator.Temp, NativeArrayOptions.UninitializedMemory);
			using (BurstableMemoryBinaryReader burstableMemoryBinaryReader = (BurstableMemoryBinaryReader)status.BufferElementPrefetchState.CreateStream())
			{
				BurstableMemoryBinaryReader burstableMemoryBinaryReader2 = burstableMemoryBinaryReader;
				NativeArray<Entity> nativeArray3 = default(NativeArray<Entity>);
				if (sceneSectionIndex > 0)
				{
					Entity entity = manager.CreateEntity(Array.Empty<ComponentType>());
					DynamicBuffer<ExternalEntityReference> dynamicBuffer = manager.AddBuffer<ExternalEntityReference>(entity);
					dynamicBuffer.ResizeUninitialized(externalEntitiesRefRange);
					nativeArray3 = dynamicBuffer.AsNativeArray().Reinterpret<Entity>();
				}
				if (status.PrefabPrefetchState._buffer != null)
				{
					if (status.PrefabPrefetchState._size != (long)sizeof(Entity))
					{
						throw new InvalidOperationException(string.Format("Internal deserialization error: Unexpected size of PrefabPrefetchState Expected:{0} Actual:{1}", sizeof(Entity), status.PrefabPrefetchState._size));
					}
					deserializationResult.PrefabRoot = *(Entity*)status.PrefabPrefetchState._buffer;
				}
				SerializeUtilityInterop.ImportChunks((SerializeUtility.WorldDeserializationStatus*)UnsafeUtility.AddressOf<SerializeUtility.WorldDeserializationStatus>(ref status), ref burstableMemoryBinaryReader2, &unsafeList, (int*)nativeArray2.GetUnsafePtr<int>(), num6, nativeList.GetUnsafePtr<int>(), &unsafeList2, ptr, ptr2, entityComponentStore, ref unityObjects, &unsafeList3, (Entity*)(nativeArray3.IsCreated ? nativeArray3.GetUnsafePtr<Entity>() : null), nativeArray3.Length, ref deserializationResult.PrefabRoot);
				status.BlobAssetBuffer = null;
				for (int i = 0; i < unsafeList3.Length; i++)
				{
					ChunkIndex chunk = unsafeList3[i].m_Chunk;
					manager.SetComponentData<ChunkHeader>(chunk.MetaChunkEntity, new ChunkHeader
					{
						ArchetypeChunk = unsafeList3[i]
					});
				}
				unsafeList3.Dispose();
				unsafeList.Dispose();
			}
			managedComponentStore.Playback(ref entityComponentStore->ManagedChangesTracker);
			entityComponentStore->InvalidateChunkListCacheForChangedArchetypes();
			if (blobAssetSize != 0)
			{
				NativeArray<ArchetypeChunk> nativeArray4 = new NativeArray<ArchetypeChunk>(unsafeList2.Length, Allocator.Temp, NativeArrayOptions.UninitializedMemory);
				UnsafeUtility.MemCpy(nativeArray4.GetUnsafePtr<ArchetypeChunk>(), (void*)unsafeList2.Ptr, (long)(sizeof(ArchetypeChunk) * unsafeList2.Length));
				manager.AddSharedComponent<BlobAssetOwner>(nativeArray4, blobAssetOwner);
				unsafeList2.Dispose();
				nativeArray4.Dispose();
			}
			status.Dispose();
			blobAssetOwner.Release();
			nativeArray.Dispose();
			for (int j = 0; j < num6; j++)
			{
				int num8 = nativeList[j + 1];
				checkedEntityDataAccess->RemoveSharedComponentReference(num8, 1);
			}
			nativeList.Dispose();
		}

		// Token: 0x06001859 RID: 6233 RVA: 0x0005BC18 File Offset: 0x00059E18
		public static void DeserializeWorld(ExclusiveEntityTransaction manager, BinaryReader reader, object[] unityObjectArray = null)
		{
			NativeArray<int> nativeArray = default(NativeArray<int>);
			if (unityObjectArray != null && unityObjectArray.Length != 0)
			{
				nativeArray = new NativeArray<int>(unityObjectArray.Length, Allocator.Temp, NativeArrayOptions.ClearMemory);
				for (int i = 0; i < unityObjectArray.Length; i++)
				{
					nativeArray[i] = ((Object)unityObjectArray[i]).GetInstanceID();
				}
			}
			else
			{
				nativeArray = new NativeArray<int>(0, Allocator.Temp, NativeArrayOptions.ClearMemory);
			}
			SerializeUtility.WorldDeserializationResult worldDeserializationResult;
			SerializeUtility.DeserializeWorldInternal(manager, reader, out worldDeserializationResult, 0, 0, nativeArray);
			nativeArray.Dispose();
		}

		// Token: 0x0600185A RID: 6234 RVA: 0x0005BC84 File Offset: 0x00059E84
		internal unsafe static void DeserializeWorldInternal(ExclusiveEntityTransaction manager, BinaryReader reader, out SerializeUtility.WorldDeserializationResult deserializationResult, int externalEntitiesRefRange, int sceneSectionIndex, NativeArray<int> unityObjects)
		{
			if (reader is StreamBinaryReader)
			{
				using (DotsSerializationReader dotsSerializationReader = DotsSerialization.CreateReader(reader))
				{
					SerializeUtility.WorldDeserializationStatus worldDeserializationStatus;
					UnsafeList<ReadCommand> unsafeList;
					SerializeUtility.BeginDeserializeWorld(((StreamBinaryReader)reader).FilePath, dotsSerializationReader, out worldDeserializationStatus, out unsafeList).JobHandle.Complete();
					unsafeList.Dispose();
					SerializeUtility.EndDeserializeWorld(manager, dotsSerializationReader, ref worldDeserializationStatus, out deserializationResult, externalEntitiesRefRange, sceneSectionIndex, unityObjects);
					return;
				}
			}
			using (DotsSerializationReader dotsSerializationReader2 = DotsSerialization.CreateReader(reader))
			{
				UnsafeList<ReadCommand> unsafeList2 = new UnsafeList<ReadCommand>(1, Allocator.Temp, NativeArrayOptions.UninitializedMemory);
				SerializeUtility.WorldDeserializationStatus worldDeserializationStatus2;
				SerializeUtility.FillReadCommands(dotsSerializationReader2, &unsafeList2, out worldDeserializationStatus2);
				for (int i = 0; i < unsafeList2.Length; i++)
				{
					reader.Position = unsafeList2[i].Offset;
					reader.ReadBytes(unsafeList2[i].Buffer, (int)unsafeList2[i].Size);
				}
				SerializeUtility.EndDeserializeWorld(manager, dotsSerializationReader2, ref worldDeserializationStatus2, out deserializationResult, externalEntitiesRefRange, sceneSectionIndex, unityObjects);
			}
		}

		// Token: 0x0600185B RID: 6235 RVA: 0x0005BD98 File Offset: 0x00059F98
		internal unsafe static BlobAssetReference<DotsSerialization.BlobHeader> SerializeWorldInternal(EntityManager entityManager, BinaryWriter writer, UnityObjectRefMap unityObjectRefs, NativeArray<EntityRemapUtility.EntityRemapInfo> entityRemapInfos, NativeParallelHashSet<UntypedWeakReferenceId> weakAssetRefs, SerializeUtility.Settings settings, bool buildBlobHeader = false)
		{
			BlobAssetReference<DotsSerialization.BlobHeader> blobAssetReference = default(BlobAssetReference<DotsSerialization.BlobHeader>);
			using (DotsSerializationWriter dotsSerializationWriter = DotsSerialization.CreateWriter(writer, SerializeUtility.WorldFileType, "EntityBinaryFile"))
			{
				using (dotsSerializationWriter.CreateNode<DotsSerialization.FolderNode>(SerializeUtility.WorldNodeType))
				{
					EntityDataAccess* checkedEntityDataAccess = entityManager.GetCheckedEntityDataAccess();
					EntityComponentStore* entityComponentStore = checkedEntityDataAccess->EntityComponentStore;
					ManagedComponentStore managedComponentStore = checkedEntityDataAccess->ManagedComponentStore;
					if (settings.RequiresDebugSection)
					{
						using (dotsSerializationWriter.CreateNode<DotsSerialization.FolderNode>(SerializeUtility.DebugSectionNodeType))
						{
							SerializeUtility.WriteTypeNames(dotsSerializationWriter, entityComponentStore);
						}
					}
					using (UnsafePtrList<Archetype> unsafePtrList = SerializeUtility.WriteArchetypesNode(dotsSerializationWriter, entityComponentStore))
					{
						NativeArray<int> nativeArray;
						NativeArray<int> nativeArray2;
						NativeArray<int> nativeArray3;
						NativeArray<int> nativeArray4;
						SerializeUtility.BuildSharedComponents(unsafePtrList, out nativeArray, out nativeArray2, out nativeArray3, out nativeArray4);
						NativeParallelHashMap<BlobAssetPtr, int> nativeParallelHashMap;
						NativeArray<int> nativeArray5;
						SerializeUtility.WriteBlobAssetNode(entityManager, dotsSerializationWriter, nativeArray2, unsafePtrList, out nativeParallelHashMap, out nativeArray5);
						using (nativeParallelHashMap)
						{
							using (nativeArray5)
							{
								int num = SerializeUtility.GenerateRemapInfo(entityManager, unsafePtrList, entityRemapInfos);
								SerializeUtility.WriteSharedAndManagedComponents(entityManager, entityRemapInfos, dotsSerializationWriter, nativeArray, unsafePtrList, nativeArray3, nativeArray4, nativeParallelHashMap, nativeArray5, weakAssetRefs, unityObjectRefs);
								SerializeUtility.WriteEnabledBits(dotsSerializationWriter, unsafePtrList);
								using (NativeList<SerializeUtility.BufferPatchRecord> nativeList = new NativeList<SerializeUtility.BufferPatchRecord>(128, Allocator.Temp))
								{
									using (NativeList<IntPtr> nativeList2 = new NativeList<IntPtr>(128, Allocator.Temp))
									{
										using (NativeList<int> nativeList3 = new NativeList<int>(128, Allocator.Temp))
										{
											Chunk* ptr = stackalloc Chunk[(UIntPtr)16384];
											SerializeUtility.WriteChunks(entityRemapInfos, dotsSerializationWriter, unsafePtrList, ptr, managedComponentStore, nativeArray5, nativeParallelHashMap, nativeList, nativeList3, nativeList2, weakAssetRefs, unityObjectRefs);
											int num2 = 0;
											for (int i = 0; i < nativeList3.Length; i++)
											{
												num2 += nativeList3[i];
											}
											SerializeUtility.WriteBufferDataElements(dotsSerializationWriter, num, unsafePtrList, nativeList3, nativeList, nativeList2);
										}
									}
								}
								Entity entity = EntityRemapUtility.RemapEntity(ref entityRemapInfos, settings.PrefabRoot);
								if (entity != Entity.Null)
								{
									SerializeUtility.WritePrefabNode(dotsSerializationWriter, entity);
								}
							}
						}
					}
					if (buildBlobHeader)
					{
						blobAssetReference = dotsSerializationWriter.WriteHeaderAndBlob();
					}
				}
			}
			return blobAssetReference;
		}

		// Token: 0x0600185C RID: 6236 RVA: 0x0005C07C File Offset: 0x0005A27C
		private unsafe static void WriteBufferDataElements(DotsSerializationWriter dotsWriter, int totalChunkCount, UnsafePtrList<Archetype> archetypeArray, NativeList<int> bufferPatchesCountPerChunk, NativeList<SerializeUtility.BufferPatchRecord> bufferPatches, NativeList<IntPtr> bufferDataList)
		{
			using (DotsSerializationWriter.NodeHandle<DotsSerialization.RevisionedRawDataNode> nodeHandle = dotsWriter.CreateNode<DotsSerialization.RevisionedRawDataNode>(SerializeUtility.BufferDataNodeType))
			{
				using (DotsSerializationWriter.WriterHandle<DotsSerialization.RevisionedRawDataNode> writerHandle = nodeHandle.GetWriterHandle())
				{
					nodeHandle.NodeHeader.Revision = 1;
					BinaryWriter writer = writerHandle.Writer;
					writer.Write(totalChunkCount);
					int num = 0;
					int num2 = 0;
					int num3 = 0;
					for (int i = 0; i < archetypeArray.Length; i++)
					{
						Archetype* ptr = *(IntPtr*)(archetypeArray.Ptr + (IntPtr)i * (IntPtr)sizeof(Archetype*) / (IntPtr)sizeof(Archetype*));
						for (int j = 0; j < ptr->Chunks.Count; j++)
						{
							int num4 = bufferPatchesCountPerChunk[num++];
							writer.Write(num4);
							if (num4 > 0)
							{
								for (int k = 0; k < num4; k++)
								{
									SerializeUtility.BufferPatchRecord bufferPatchRecord = bufferPatches[num2 + k];
									void* ptr2 = (void*)bufferDataList[num3++];
									writer.Write(bufferPatchRecord.ChunkOffset);
									writer.Write(bufferPatchRecord.AllocSizeBytes);
									writer.WriteBytes(ptr2, bufferPatchRecord.AllocSizeBytes);
									Memory.Unmanaged.Free(ptr2, Allocator.TempJob);
								}
								num2 += num4;
							}
						}
					}
				}
			}
		}

		// Token: 0x0600185D RID: 6237 RVA: 0x0005C1F0 File Offset: 0x0005A3F0
		private unsafe static void WriteChunks(NativeArray<EntityRemapUtility.EntityRemapInfo> entityRemapInfos, DotsSerializationWriter dotsWriter, UnsafePtrList<Archetype> archetypeArray, Chunk* tempChunk, ManagedComponentStore mcs, NativeArray<int> blobAssetOffsets, NativeParallelHashMap<BlobAssetPtr, int> blobAssetMap, NativeList<SerializeUtility.BufferPatchRecord> bufferPatches, NativeList<int> bufferPatchesCountPerChunk, NativeList<IntPtr> bufferDataList, NativeParallelHashSet<UntypedWeakReferenceId> weakAssetRefs, UnityObjectRefMap unityObjectRefs)
		{
			using (DotsSerializationWriter.NodeHandle<DotsSerialization.RevisionedRawDataNode> nodeHandle = dotsWriter.CreateNode<DotsSerialization.RevisionedRawDataNode>(SerializeUtility.ChunksNodeType))
			{
				using (DotsSerializationWriter.WriterHandle<DotsSerialization.RevisionedRawDataNode> writerHandle = nodeHandle.GetWriterHandle())
				{
					nodeHandle.NodeHeader.Revision = 1;
					BinaryWriter writer = writerHandle.Writer;
					int num = 1;
					for (int i = 0; i < archetypeArray.Length; i++)
					{
						Archetype* ptr = *(IntPtr*)(archetypeArray.Ptr + (IntPtr)i * (IntPtr)sizeof(Archetype*) / (IntPtr)sizeof(Archetype*));
						for (int j = 0; j < ptr->Chunks.Count; j++)
						{
							ChunkIndex chunkIndex = ptr->Chunks[j];
							UnsafeUtility.MemCpy((void*)tempChunk, (void*)chunkIndex.GetPtr(), 16384L);
							byte* ptr2 = &tempChunk->Buffer.FixedElementField;
							int count = chunkIndex.Count;
							tempChunk->CountForSerialization = count;
							tempChunk->metaChunkEntity = EntityRemapUtility.RemapEntity(ref entityRemapInfos, tempChunk->metaChunkEntity);
							BufferHeader.PatchAfterCloningChunk(ptr, ptr2, count);
							SerializeUtility.PatchManagedComponentIndices(ptr2, count, ptr, ref num, mcs);
							EntityRemapUtility.PatchEntities(ptr->ScalarEntityPatches, ptr->ScalarEntityPatchCount, ptr->BufferEntityPatches, ptr->BufferEntityPatchCount, ptr2, count, ref entityRemapInfos);
							if (ptr->HasBlobAssetRefs)
							{
								SerializeUtility.PatchBlobAssetsInChunkBeforeSave(ptr, ptr2, count, blobAssetOffsets, blobAssetMap);
							}
							if (ptr->HasWeakAssetRefs)
							{
								SerializeUtility.GetWeakAssetRefsInChunk(ptr, ptr2, count, weakAssetRefs);
							}
							if (ptr->HasUnityObjectRefs)
							{
								SerializeUtility.GetAndPatchUnityObjectRefsInChunkBeforeSave(ptr, ptr2, count, unityObjectRefs);
							}
							SerializeUtility.ClearChunkHeaderComponents(ptr, ptr2, count);
							ChunkDataUtility.MemsetUnusedChunkData(ptr, ptr2, 0, count);
							int length = bufferPatches.Length;
							SerializeUtility.FillBufferPatchRecordsAndClearBufferPointer(ptr, ptr2, count, bufferPatches, bufferDataList);
							int num2 = bufferPatches.Length - length;
							bufferPatchesCountPerChunk.Add(in num2);
							tempChunk->ArchetypeIndexForSerialization = i;
							writer.WriteBytes((void*)tempChunk, 16384);
						}
					}
				}
			}
		}

		// Token: 0x0600185E RID: 6238 RVA: 0x0005C400 File Offset: 0x0005A600
		private unsafe static void WriteSharedAndManagedComponents(EntityManager entityManager, NativeArray<EntityRemapUtility.EntityRemapInfo> entityRemapInfos, DotsSerializationWriter dotsWriter, NativeArray<int> sharedComponentArrays, UnsafePtrList<Archetype> archetypeArray, NativeArray<int> managedSharedComponentIndices, NativeArray<int> unmanagedSharedComponentIndices, NativeParallelHashMap<BlobAssetPtr, int> blobAssetMap, NativeArray<int> blobAssetOffsets, NativeParallelHashSet<UntypedWeakReferenceId> weakAssetRefs, UnityObjectRefMap unityObjectRefs)
		{
			using (DotsSerializationWriter.NodeHandle<DotsSerialization.RevisionedRawDataNode> nodeHandle = dotsWriter.CreateNode<DotsSerialization.RevisionedRawDataNode>(SerializeUtility.SharedAndManagedComponentsNodeType))
			{
				using (DotsSerializationWriter.WriterHandle<DotsSerialization.RevisionedRawDataNode> writerHandle = nodeHandle.GetWriterHandle())
				{
					BinaryWriter writer = writerHandle.Writer;
					writer.Write(sharedComponentArrays.Length);
					writer.WriteArray<int>(sharedComponentArrays);
					sharedComponentArrays.Dispose();
					SerializeUtility.WriteUnmanagedSharedComponents(entityManager, unmanagedSharedComponentIndices, writer, unityObjectRefs, (EntityRemapUtility.EntityRemapInfo*)entityRemapInfos.GetUnsafePtr<EntityRemapUtility.EntityRemapInfo>(), blobAssetMap, blobAssetOffsets);
					SerializeUtility.WriteSharedAndManagedComponents(entityManager, archetypeArray, managedSharedComponentIndices, writer, unityObjectRefs, (EntityRemapUtility.EntityRemapInfo*)entityRemapInfos.GetUnsafePtr<EntityRemapUtility.EntityRemapInfo>(), blobAssetMap, blobAssetOffsets, weakAssetRefs);
				}
			}
		}

		// Token: 0x0600185F RID: 6239 RVA: 0x0005C4AC File Offset: 0x0005A6AC
		private unsafe static void WriteEnabledBits(DotsSerializationWriter dotsWriter, UnsafePtrList<Archetype> archetypeArray)
		{
			int num = 0;
			int num2 = 0;
			for (int i = 0; i < archetypeArray.Length; i++)
			{
				Archetype* ptr = archetypeArray[i];
				num += ptr->Chunks.Count * (int)ptr->Chunks.ComponentEnabledBitsSizeTotalPerChunk;
				num2 += ptr->Chunks.Count * (int)ptr->Chunks.ComponentEnabledBitsHierarchicalDataSizePerChunk;
			}
			using (DotsSerializationWriter.NodeHandle<DotsSerialization.RevisionedRawDataNode> nodeHandle = dotsWriter.CreateNode<DotsSerialization.RevisionedRawDataNode>(SerializeUtility.EnabledBitsNodeType))
			{
				using (DotsSerializationWriter.WriterHandle<DotsSerialization.RevisionedRawDataNode> writerHandle = nodeHandle.GetWriterHandle())
				{
					BinaryWriter writer = writerHandle.Writer;
					writer.Write(num);
					for (int j = 0; j < archetypeArray.Length; j++)
					{
						Archetype* ptr2 = archetypeArray[j];
						ArchetypeChunkData chunks = ptr2->Chunks;
						byte* pointerToComponentEnabledArrayForArchetype = chunks.GetPointerToComponentEnabledArrayForArchetype();
						int num3 = chunks.Count * (int)ptr2->Chunks.ComponentEnabledBitsSizeTotalPerChunk;
						writer.WriteBytes((void*)pointerToComponentEnabledArrayForArchetype, num3);
					}
					writer.Write(num2);
					for (int k = 0; k < archetypeArray.Length; k++)
					{
						Archetype* ptr3 = archetypeArray[k];
						ArchetypeChunkData chunks2 = ptr3->Chunks;
						int* pointerToChunkDisabledCountForArchetype = chunks2.GetPointerToChunkDisabledCountForArchetype();
						int num4 = chunks2.Count * (int)ptr3->Chunks.ComponentEnabledBitsHierarchicalDataSizePerChunk;
						writer.WriteBytes((void*)pointerToChunkDisabledCountForArchetype, num4);
					}
				}
			}
		}

		// Token: 0x06001860 RID: 6240 RVA: 0x0005C624 File Offset: 0x0005A824
		private static void WriteBlobAssetNode(EntityManager entityManager, DotsSerializationWriter dotsWriter, NativeArray<int> sharedComponentsToSerialize, UnsafePtrList<Archetype> archetypeArray, out NativeParallelHashMap<BlobAssetPtr, int> blobAssetMap, out NativeArray<int> blobAssetOffsets)
		{
			using (DotsSerializationWriter.NodeHandle<DotsSerialization.RevisionedRawDataNode> nodeHandle = dotsWriter.CreateNode<DotsSerialization.RevisionedRawDataNode>(SerializeUtility.BlobAssetsNodeType))
			{
				using (DotsSerializationWriter.WriterHandle<DotsSerialization.RevisionedRawDataNode> writerHandle = nodeHandle.GetWriterHandle())
				{
					nodeHandle.NodeHeader.Revision = 1;
					NativeList<BlobAssetPtr> nativeList;
					SerializeUtility.GatherAllUsedBlobAssets(entityManager, sharedComponentsToSerialize, archetypeArray, out nativeList, out blobAssetMap);
					SerializeUtility.WriteBlobAssetBatch(writerHandle.Writer, nativeList.AsArray(), out blobAssetOffsets);
					nativeList.Dispose();
				}
			}
		}

		// Token: 0x06001861 RID: 6241 RVA: 0x0005C6B4 File Offset: 0x0005A8B4
		private unsafe static void WritePrefabNode(DotsSerializationWriter dotsWriter, Entity prefabRoot)
		{
			using (DotsSerializationWriter.NodeHandle<DotsSerialization.RevisionedRawDataNode> nodeHandle = dotsWriter.CreateNode<DotsSerialization.RevisionedRawDataNode>(SerializeUtility.PrefabNodeType))
			{
				using (DotsSerializationWriter.WriterHandle<DotsSerialization.RevisionedRawDataNode> writerHandle = nodeHandle.GetWriterHandle())
				{
					nodeHandle.NodeHeader.Revision = 1;
					writerHandle.Writer.WriteBytes((void*)(&prefabRoot), sizeof(Entity));
				}
			}
		}

		// Token: 0x06001862 RID: 6242 RVA: 0x0005C734 File Offset: 0x0005A934
		internal unsafe static int CalculateBlobAssetBatchTotalSize(NativeArray<BlobAssetPtr> blobAssets, out NativeArray<int> blobAssetOffsets)
		{
			blobAssetOffsets = new NativeArray<int>(blobAssets.Length, Allocator.Temp, NativeArrayOptions.ClearMemory);
			int num = sizeof(BlobAssetBatch);
			for (int i = 0; i < blobAssets.Length; i++)
			{
				num += sizeof(BlobAssetHeader);
				blobAssetOffsets[i] = num;
				num += SerializeUtility.Align16(blobAssets[i].Header->Length);
			}
			return num;
		}

		// Token: 0x06001863 RID: 6243 RVA: 0x0005C79C File Offset: 0x0005A99C
		internal unsafe static void WriteBlobAssetBatch(BinaryWriter writer, NativeArray<BlobAssetPtr> blobAssets, int totalBlobAssetBatchSize)
		{
			BlobAssetBatch blobAssetBatch = BlobAssetBatch.CreateForSerialize(blobAssets.Length, totalBlobAssetBatchSize);
			writer.WriteBytes((void*)(&blobAssetBatch), sizeof(BlobAssetBatch));
			int4 zero = int4.zero;
			for (int i = 0; i < blobAssets.Length; i++)
			{
				int length = blobAssets[i].Header->Length;
				ulong hash = blobAssets[i].Header->Hash;
				BlobAssetHeader blobAssetHeader = BlobAssetHeader.CreateForSerialize(SerializeUtility.Align16(length), hash);
				writer.WriteBytes((void*)(&blobAssetHeader), sizeof(BlobAssetHeader));
				writer.WriteBytes((void*)(blobAssets[i].Header + 1), length);
				writer.WriteBytes((void*)(&zero), blobAssetHeader.Length - length);
			}
		}

		// Token: 0x06001864 RID: 6244 RVA: 0x0005C850 File Offset: 0x0005AA50
		internal static void WriteBlobAssetBatch(BinaryWriter writer, NativeArray<BlobAssetPtr> blobAssets, out NativeArray<int> blobAssetOffsets)
		{
			int num = SerializeUtility.CalculateBlobAssetBatchTotalSize(blobAssets, out blobAssetOffsets);
			SerializeUtility.WriteBlobAssetBatch(writer, blobAssets, num);
		}

		// Token: 0x06001865 RID: 6245 RVA: 0x0005C870 File Offset: 0x0005AA70
		private static void BuildSharedComponents(UnsafePtrList<Archetype> archetypeArray, out NativeArray<int> sharedComponentArrays, out NativeArray<int> sharedComponentsToSerialize, out NativeArray<int> managedSharedComponents, out NativeArray<int> unmanagedSharedComponents)
		{
			int num;
			NativeParallelHashMap<int, int> nativeParallelHashMap = SerializeUtility.GatherSharedComponents(archetypeArray, out num);
			NativeArray<int> nativeArray = new NativeArray<int>(num, Allocator.Temp, NativeArrayOptions.ClearMemory);
			SerializeUtility.FillSharedComponentArrays(nativeArray, archetypeArray, nativeParallelHashMap);
			sharedComponentsToSerialize = new NativeArray<int>(nativeParallelHashMap.Count() - 1, Allocator.Temp, NativeArrayOptions.ClearMemory);
			using (NativeArray<int> keyArray = nativeParallelHashMap.GetKeyArray(Allocator.Temp))
			{
				int num2 = 0;
				foreach (int num3 in keyArray)
				{
					int num4;
					if (num3 != 0 && nativeParallelHashMap.TryGetValue(num3, out num4))
					{
						sharedComponentsToSerialize[num4 - 1] = num3;
						if (EntityComponentStore.IsUnmanagedSharedComponentIndex(num3))
						{
							num2++;
						}
					}
				}
				managedSharedComponents = new NativeArray<int>(sharedComponentsToSerialize.Length - num2, Allocator.Temp, NativeArrayOptions.ClearMemory);
				unmanagedSharedComponents = new NativeArray<int>(num2, Allocator.Temp, NativeArrayOptions.ClearMemory);
				NativeArray<int> nativeArray2 = new NativeArray<int>(sharedComponentsToSerialize.Length, Allocator.Temp, NativeArrayOptions.ClearMemory);
				int i = 0;
				int num5 = 0;
				int num6 = 0;
				while (i < sharedComponentsToSerialize.Length)
				{
					int num7 = sharedComponentsToSerialize[i];
					if (EntityComponentStore.IsUnmanagedSharedComponentIndex(num7))
					{
						nativeArray2[i] = num5 + 1;
						unmanagedSharedComponents[num5++] = num7;
					}
					else
					{
						nativeArray2[i] = num6 + num2 + 1;
						managedSharedComponents[num6++] = num7;
					}
					i++;
				}
				if (num2 > 0 && num2 < sharedComponentsToSerialize.Length)
				{
					sharedComponentArrays = new NativeArray<int>(nativeArray.Length, Allocator.Temp, NativeArrayOptions.ClearMemory);
					for (int j = 0; j < sharedComponentArrays.Length; j++)
					{
						int num8 = nativeArray[j];
						sharedComponentArrays[j] = ((num8 != 0) ? nativeArray2[num8 - 1] : 0);
					}
				}
				else
				{
					sharedComponentArrays = nativeArray;
				}
			}
		}

		// Token: 0x06001866 RID: 6246 RVA: 0x0005CA64 File Offset: 0x0005AC64
		private unsafe static void WriteTypeNames(DotsSerializationWriter dotsWriter, EntityComponentStore* entityComponentStore)
		{
			using (StringTableWriterHandle stringTableWriterHandle = dotsWriter.CreateStringTableNode(SerializeUtility.TypesNameStringTableNodeType))
			{
				using (DotsSerializationWriter.NodeHandle<DotsSerialization.TypeNamesNode> nodeHandle = dotsWriter.CreateNode<DotsSerialization.TypeNamesNode>(SerializeUtility.TypesNameNodeType))
				{
					using (DotsSerializationWriter.WriterHandle<DotsSerialization.TypeNamesNode> writerHandle = nodeHandle.GetWriterHandle())
					{
						UnsafeParallelHashSet<TypeIndex> unsafeParallelHashSet = new UnsafeParallelHashSet<TypeIndex>(1024, Allocator.Temp);
						UnsafePtrList<Archetype> allArchetypes = SerializeUtility.GetAllArchetypes(entityComponentStore, Allocator.Temp);
						for (int num = 0; num != allArchetypes.Length; num++)
						{
							Archetype* ptr = *(IntPtr*)(allArchetypes.Ptr + (IntPtr)num * (IntPtr)sizeof(Archetype*) / (IntPtr)sizeof(Archetype*));
							for (int i = 0; i < ptr->TypesCount; i++)
							{
								TypeIndex typeIndex = ptr->Types[i].TypeIndex;
								unsafeParallelHashSet.Add(typeIndex);
							}
						}
						BinaryWriter writer = writerHandle.Writer;
						int num2 = unsafeParallelHashSet.Count();
						nodeHandle.NodeHeader.TypeCount = num2;
						foreach (TypeIndex typeIndex2 in unsafeParallelHashSet)
						{
							readonly ref TypeManager.TypeInfo typeInfo = ref TypeManager.GetTypeInfo(typeIndex2);
							NativeText.ReadOnly debugTypeName = typeInfo.DebugTypeName;
							writer.Write(typeInfo.StableTypeHash);
							writer.Write(stringTableWriterHandle.WriteString(debugTypeName.ToString()));
						}
						unsafeParallelHashSet.Dispose();
					}
				}
			}
		}

		// Token: 0x06001867 RID: 6247 RVA: 0x0005CC30 File Offset: 0x0005AE30
		private unsafe static UnsafePtrList<Archetype> WriteArchetypesNode(DotsSerializationWriter dotsWriter, EntityComponentStore* entityComponentStore)
		{
			UnsafePtrList<Archetype> allArchetypes;
			using (DotsSerializationWriter.NodeHandle<DotsSerialization.RevisionedRawDataNode> nodeHandle = dotsWriter.CreateNode<DotsSerialization.RevisionedRawDataNode>(SerializeUtility.ArchetypesNodeType))
			{
				using (DotsSerializationWriter.WriterHandle<DotsSerialization.RevisionedRawDataNode> writerHandle = nodeHandle.GetWriterHandle())
				{
					nodeHandle.NodeHeader.Revision = 1;
					BinaryWriter writer = writerHandle.Writer;
					allArchetypes = SerializeUtility.GetAllArchetypes(entityComponentStore, Allocator.Temp);
					UnsafeHashMap<ulong, int> unsafeHashMap = new UnsafeHashMap<ulong, int>(allArchetypes.Length * 8, Allocator.Temp);
					for (int num = 0; num != allArchetypes.Length; num++)
					{
						Archetype* ptr = *(IntPtr*)(allArchetypes.Ptr + (IntPtr)num * (IntPtr)sizeof(Archetype*) / (IntPtr)sizeof(Archetype*));
						for (int i = 0; i < ptr->TypesCount; i++)
						{
							TypeManager.TypeInfo typeInfo = *TypeManager.GetTypeInfo(ptr->Types[i].TypeIndex);
							ulong stableTypeHash = typeInfo.StableTypeHash;
							SerializeUtility.ValidateTypeForSerialization(typeInfo);
							unsafeHashMap.TryAdd(stableTypeHash, num);
						}
					}
					using (NativeArray<ulong> keyArray = unsafeHashMap.GetKeyArray(Allocator.Temp))
					{
						writer.Write(keyArray.Length);
						foreach (ulong num2 in keyArray)
						{
							writer.Write(num2);
						}
						for (int j = 0; j < keyArray.Length; j++)
						{
							unsafeHashMap[keyArray[j]] = j;
						}
					}
					SerializeUtility.WriteArchetypes(writer, allArchetypes, unsafeHashMap);
					unsafeHashMap.Dispose();
				}
			}
			return allArchetypes;
		}

		// Token: 0x06001868 RID: 6248 RVA: 0x0005CE20 File Offset: 0x0005B020
		private static int Align16(int x)
		{
			return (x + 15) & -16;
		}

		// Token: 0x06001869 RID: 6249 RVA: 0x0005CE2C File Offset: 0x0005B02C
		private unsafe static void PatchManagedComponentIndices(byte* chunkBuffer, int entityCount, Archetype* archetype, ref int currentManagedIndex, ManagedComponentStore managedComponentStore)
		{
			for (int i = 0; i < archetype->NumManagedComponents; i++)
			{
				int num = archetype->TypeMemoryOrderIndexToIndexInArchetype[i + (int)archetype->FirstManagedComponent];
				int num2 = archetype->Offsets[num];
				int* ptr = (int*)(chunkBuffer + num2);
				for (int j = 0; j < entityCount; j++)
				{
					if (ptr[j] != 0)
					{
						if (managedComponentStore.GetManagedComponent(ptr[j]) == null)
						{
							ptr[j] = 0;
						}
						else
						{
							ref int ptr2 = ref ptr[j];
							int num3 = currentManagedIndex;
							currentManagedIndex = num3 + 1;
							ptr2 = num3;
						}
					}
				}
			}
		}

		// Token: 0x0600186A RID: 6250 RVA: 0x0005CEBC File Offset: 0x0005B0BC
		private unsafe static void WriteSharedAndManagedComponents(EntityManager entityManager, UnsafePtrList<Archetype> archetypeArray, NativeArray<int> sharedComponentIndices, BinaryWriter writer, UnityObjectRefMap unityObjectRefs, EntityRemapUtility.EntityRemapInfo* remapping, NativeParallelHashMap<BlobAssetPtr, int> blobAssetMap, NativeArray<int> blobAssetOffsets, NativeParallelHashSet<UntypedWeakReferenceId> weakAssetRefs)
		{
			int num = 0;
			UnsafeAppendBuffer unsafeAppendBuffer = new UnsafeAppendBuffer(0, 16, Allocator.Temp);
			EntityDataAccess* checkedEntityDataAccess = entityManager.GetCheckedEntityDataAccess();
			ManagedComponentStore managedComponentStore = checkedEntityDataAccess->ManagedComponentStore;
			NativeArray<SerializeUtility.SharedComponentRecord> nativeArray = new NativeArray<SerializeUtility.SharedComponentRecord>(sharedComponentIndices.Length, Allocator.Temp, NativeArrayOptions.ClearMemory);
			ManagedObjectBinaryWriter managedObjectBinaryWriter = new ManagedObjectBinaryWriter(&unsafeAppendBuffer, unityObjectRefs);
			SerializeUtility.ManagedObjectWriterAdapter managedObjectWriterAdapter = new SerializeUtility.ManagedObjectWriterAdapter(remapping, blobAssetMap, blobAssetOffsets, weakAssetRefs);
			managedObjectBinaryWriter.AddAdapter(managedObjectWriterAdapter);
			managedObjectWriterAdapter.SerializeEntityReferences = false;
			for (int i = 0; i < sharedComponentIndices.Length; i++)
			{
				int num2 = sharedComponentIndices[i];
				object sharedComponentDataNonDefaultBoxed = checkedEntityDataAccess->GetSharedComponentDataNonDefaultBoxed(num2);
				Type type = sharedComponentDataNonDefaultBoxed.GetType();
				readonly ref TypeManager.TypeInfo typeInfo = ref TypeManager.GetTypeInfo(TypeManager.GetTypeIndex(type));
				object obj = Convert.ChangeType(sharedComponentDataNonDefaultBoxed, type);
				managedObjectBinaryWriter.WriteObject(obj);
				nativeArray[i] = new SerializeUtility.SharedComponentRecord
				{
					StableTypeHash = typeInfo.StableTypeHash,
					ComponentSize = -1
				};
			}
			managedObjectWriterAdapter.SerializeEntityReferences = true;
			for (int j = 0; j < archetypeArray.Length; j++)
			{
				Archetype* ptr = *(IntPtr*)(archetypeArray.Ptr + (IntPtr)j * (IntPtr)sizeof(Archetype*) / (IntPtr)sizeof(Archetype*));
				if (ptr->NumManagedComponents != 0)
				{
					for (int k = 0; k < ptr->Chunks.Count; k++)
					{
						ChunkIndex chunkIndex = ptr->Chunks[k];
						for (int l = 0; l < ptr->NumManagedComponents; l++)
						{
							int num3 = ptr->TypeMemoryOrderIndexToIndexInArchetype[l + (int)ptr->FirstManagedComponent];
							int* componentDataRO = (int*)ChunkDataUtility.GetComponentDataRO(chunkIndex, ptr, 0, num3);
							readonly ref TypeManager.TypeInfo typeInfo2 = ref TypeManager.GetTypeInfo(ptr->Types[num3].TypeIndex);
							int m = 0;
							int count = chunkIndex.Count;
							while (m < count)
							{
								if (componentDataRO[m] != 0)
								{
									object managedComponent = managedComponentStore.GetManagedComponent(componentDataRO[m]);
									if (managedComponent != null)
									{
										if (managedComponent.GetType() != typeInfo2.Type)
										{
											throw new InvalidOperationException(string.Format("Managed object type {0} doesn't match component type in archetype {1}", managedComponent.GetType(), typeInfo2.Type));
										}
										num++;
										unsafeAppendBuffer.Add<ulong>(typeInfo2.StableTypeHash);
										managedObjectBinaryWriter.WriteObject(managedComponent);
									}
								}
								m++;
							}
						}
					}
				}
			}
			writer.Write(nativeArray.Length);
			writer.WriteArray<SerializeUtility.SharedComponentRecord>(nativeArray);
			writer.Write(unsafeAppendBuffer.Length);
			writer.Write(num);
			writer.WriteBytes((void*)unsafeAppendBuffer.Ptr, unsafeAppendBuffer.Length);
			nativeArray.Dispose();
			unsafeAppendBuffer.Dispose();
		}

		// Token: 0x0600186B RID: 6251 RVA: 0x0005D148 File Offset: 0x0005B348
		private unsafe static void WriteUnmanagedSharedComponents(EntityManager entityManager, NativeArray<int> unmanagedSharedComponentIndices, BinaryWriter writer, UnityObjectRefMap unityObjectRefs, EntityRemapUtility.EntityRemapInfo* remapping, NativeParallelHashMap<BlobAssetPtr, int> blobAssetMap, NativeArray<int> blobAssetOffsets)
		{
			UnsafeAppendBuffer unsafeAppendBuffer = new UnsafeAppendBuffer(0, 16, Allocator.Temp);
			EntityDataAccess* checkedEntityDataAccess = entityManager.GetCheckedEntityDataAccess();
			NativeArray<SerializeUtility.SharedComponentRecord> nativeArray = new NativeArray<SerializeUtility.SharedComponentRecord>(unmanagedSharedComponentIndices.Length, Allocator.Temp, NativeArrayOptions.ClearMemory);
			for (int i = 0; i < unmanagedSharedComponentIndices.Length; i++)
			{
				int num = unmanagedSharedComponentIndices[i];
				nativeArray[i] = SerializeUtility.WriteUnmanagedSharedComponent(num, remapping, blobAssetMap, blobAssetOffsets, checkedEntityDataAccess, ref unsafeAppendBuffer, unityObjectRefs);
			}
			writer.Write(nativeArray.Length);
			writer.WriteArray<SerializeUtility.SharedComponentRecord>(nativeArray);
			writer.Write(unsafeAppendBuffer.Length);
			writer.WriteBytes((void*)unsafeAppendBuffer.Ptr, unsafeAppendBuffer.Length);
			nativeArray.Dispose();
			unsafeAppendBuffer.Dispose();
		}

		// Token: 0x0600186C RID: 6252 RVA: 0x0005D1F4 File Offset: 0x0005B3F4
		private unsafe static SerializeUtility.SharedComponentRecord WriteUnmanagedSharedComponent(int sharedComponentIndex, EntityRemapUtility.EntityRemapInfo* remapping, NativeParallelHashMap<BlobAssetPtr, int> blobAssetMap, NativeArray<int> blobAssetOffsets, EntityDataAccess* access, ref UnsafeAppendBuffer allManagedObjectsBuffer, UnityObjectRefMap unityObjectRefs)
		{
			TypeIndex componentTypeFromSharedComponentIndex = EntityComponentStore.GetComponentTypeFromSharedComponentIndex(sharedComponentIndex);
			byte* sharedComponentDataAddr_Unmanaged = (byte*)access->EntityComponentStore->GetSharedComponentDataAddr_Unmanaged(sharedComponentIndex, componentTypeFromSharedComponentIndex);
			readonly ref TypeManager.TypeInfo typeInfo = ref TypeManager.GetTypeInfo(componentTypeFromSharedComponentIndex);
			int typeSize = typeInfo.TypeSize;
			SerializeUtility.SharedComponentRecord sharedComponentRecord = new SerializeUtility.SharedComponentRecord
			{
				StableTypeHash = typeInfo.StableTypeHash,
				ComponentSize = typeSize
			};
			if (typeInfo.BlobAssetRefOffsetCount > 0)
			{
				SerializeUtility.PatchBlobAssetRefInfoBeforeSave(sharedComponentDataAddr_Unmanaged, TypeManager.GetBlobAssetRefOffsets(in typeInfo), typeInfo.BlobAssetRefOffsetCount, blobAssetOffsets, blobAssetMap);
			}
			if (typeInfo.HasUnityObjectRefs)
			{
				SerializeUtility.GetAndPatchUnityObjectRefsInComponentBeforeSave(sharedComponentDataAddr_Unmanaged, TypeManager.GetUnityObjectRefOffsets(in typeInfo), typeInfo.UnityObjectRefOffsetCount, unityObjectRefs);
			}
			allManagedObjectsBuffer.Add((void*)sharedComponentDataAddr_Unmanaged, typeSize);
			return sharedComponentRecord;
		}

		// Token: 0x0600186D RID: 6253 RVA: 0x0005D288 File Offset: 0x0005B488
		private unsafe static int ReadUnmanagedSharedComponents(ExclusiveEntityTransaction manager, BinaryReader reader, NativeList<int> sharedComponentRemap, SerializeUtility.WorldDeserializationStatus* status, NativeArray<int> unityObjects)
		{
			byte* blobAssetBuffer = (byte*)status->BlobAssetBuffer;
			NativeArray<SerializeUtility.SharedComponentRecord> nativeArray;
			int num = SerializeUtility.ReadSharedComponentMetadata(reader, out nativeArray);
			int num2 = reader.ReadInt();
			int num5;
			using (UnsafeAppendBuffer unsafeAppendBuffer = new UnsafeAppendBuffer(num2, 16, Allocator.Temp))
			{
				unsafeAppendBuffer.ResizeUninitialized(num2);
				reader.ReadBytes((void*)unsafeAppendBuffer.Ptr, num2);
				UnsafeAppendBuffer.Reader reader2 = unsafeAppendBuffer.AsReader();
				EntityDataAccess* checkedEntityDataAccess = manager.EntityManager.GetCheckedEntityDataAccess();
				for (int i = 0; i < nativeArray.Length; i++)
				{
					SerializeUtility.SharedComponentRecord sharedComponentRecord = nativeArray[i];
					TypeIndex typeIndexFromStableTypeHash = TypeManager.GetTypeIndexFromStableTypeHash(sharedComponentRecord.StableTypeHash);
					byte* ptr = (byte*)reader2.ReadNext(sharedComponentRecord.ComponentSize);
					TypeManager.TypeInfo typeInfo = *TypeManager.GetTypeInfo(typeIndexFromStableTypeHash);
					int blobAssetRefOffsetCount = typeInfo.BlobAssetRefOffsetCount;
					if (blobAssetRefOffsetCount > 0)
					{
						TypeManager.EntityOffsetInfo* blobAssetRefOffsets = TypeManager.GetBlobAssetRefOffsets(in typeInfo);
						for (int j = 0; j < blobAssetRefOffsetCount; j++)
						{
							int offset = blobAssetRefOffsets[j].Offset;
							BlobAssetReferenceData* ptr2 = (BlobAssetReferenceData*)(ptr + offset);
							int num3 = ptr2->m_Ptr;
							byte* ptr3 = null;
							if (num3 != -1)
							{
								ptr3 = blobAssetBuffer + num3;
							}
							ptr2->m_Ptr = ptr3;
						}
					}
					if (typeInfo.HasUnityObjectRefs)
					{
						SerializeUtilityInterop.PatchUnityObjectRefsInComponentAfterLoad(ptr, TypeManager.GetUnityObjectRefOffsets(in typeInfo), typeInfo.UnityObjectRefOffsetCount, unityObjects);
					}
					int hashCode = TypeManager.GetHashCode((void*)ptr, typeIndexFromStableTypeHash);
					int num4 = checkedEntityDataAccess->EntityComponentStore->InsertSharedComponent_Unmanaged(typeIndexFromStableTypeHash, hashCode, (void*)ptr, null);
					sharedComponentRemap.Add(in num4);
				}
				num5 = num;
			}
			return num5;
		}

		// Token: 0x0600186E RID: 6254 RVA: 0x0005D420 File Offset: 0x0005B620
		private unsafe static int ReadManagedSharedComponents(ExclusiveEntityTransaction manager, BinaryReader reader, NativeList<int> sharedComponentRemap, NativeArray<int> unityObjects, void* blobAssetBuffer)
		{
			EntityDataAccess* checkedEntityDataAccess = manager.EntityManager.GetCheckedEntityDataAccess();
			EntityComponentStore* entityComponentStore = checkedEntityDataAccess->EntityComponentStore;
			ManagedComponentStore managedComponentStore = checkedEntityDataAccess->ManagedComponentStore;
			NativeArray<SerializeUtility.SharedComponentRecord> nativeArray;
			int num = SerializeUtility.ReadSharedComponentMetadata(reader, out nativeArray);
			int num2 = reader.ReadInt();
			int num3 = reader.ReadInt();
			UnsafeAppendBuffer unsafeAppendBuffer = new UnsafeAppendBuffer(num2, 16, Allocator.Temp);
			unsafeAppendBuffer.ResizeUninitialized(num2);
			reader.ReadBytes((void*)unsafeAppendBuffer.Ptr, num2);
			UnsafeAppendBuffer.Reader reader2 = unsafeAppendBuffer.AsReader();
			ManagedObjectBinaryReader managedObjectBinaryReader = new ManagedObjectBinaryReader(&reader2, unityObjects);
			managedObjectBinaryReader.AddAdapter(new SerializeUtility.ManagedObjectReaderAdapter((byte*)blobAssetBuffer));
			SerializeUtility.ReadSharedComponents(manager, ref reader2, managedObjectBinaryReader, num2, sharedComponentRemap, nativeArray);
			managedComponentStore.ResetManagedComponentStoreForDeserialization(num3, ref *entityComponentStore);
			for (int i = 0; i < num3; i++)
			{
				Type type = TypeManager.GetTypeInfo(TypeManager.GetTypeIndexFromStableTypeHash(reader2.ReadNext<ulong>())).Type;
				object obj = managedObjectBinaryReader.ReadObject(type);
				managedComponentStore.SetManagedComponentValue(i + 1, obj);
			}
			unsafeAppendBuffer.Dispose();
			return num;
		}

		// Token: 0x0600186F RID: 6255 RVA: 0x0005D510 File Offset: 0x0005B710
		private unsafe static void ReadSharedComponents(ExclusiveEntityTransaction manager, ref UnsafeAppendBuffer.Reader reader, ManagedObjectBinaryReader managedDataReader, int expectedReadSize, NativeList<int> sharedComponentRemap, NativeArray<SerializeUtility.SharedComponentRecord> sharedComponentRecordArray)
		{
			EntityDataAccess* checkedEntityDataAccess = manager.EntityManager.GetCheckedEntityDataAccess();
			for (int i = 0; i < sharedComponentRecordArray.Length; i++)
			{
				TypeIndex typeIndexFromStableTypeHash = TypeManager.GetTypeIndexFromStableTypeHash(sharedComponentRecordArray[i].StableTypeHash);
				readonly ref TypeManager.TypeInfo typeInfo = ref TypeManager.GetTypeInfo(typeIndexFromStableTypeHash);
				object obj = managedDataReader.ReadObject(typeInfo.Type);
				int hashCode = TypeManager.GetHashCode(obj, typeInfo.TypeIndex);
				int num = checkedEntityDataAccess->InsertSharedComponentAssumeNonDefault(typeIndexFromStableTypeHash, hashCode, obj);
				sharedComponentRemap.Add(in num);
			}
		}

		// Token: 0x06001870 RID: 6256 RVA: 0x0005D590 File Offset: 0x0005B790
		private static bool IsTypeValidForSerialization(Type type, HashSet<Type> validTypes)
		{
			if (validTypes.Contains(type))
			{
				return true;
			}
			if (type.GetCustomAttribute<ChunkSerializableAttribute>() != null)
			{
				return true;
			}
			foreach (FieldInfo fieldInfo in type.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
			{
				if (!fieldInfo.IsStatic)
				{
					if (fieldInfo.FieldType.IsPointer || fieldInfo.FieldType == typeof(UIntPtr) || fieldInfo.FieldType == typeof(IntPtr))
					{
						return false;
					}
					if (fieldInfo.FieldType.IsValueType && !fieldInfo.FieldType.IsPrimitive && !fieldInfo.FieldType.IsEnum && !SerializeUtility.IsTypeValidForSerialization(fieldInfo.FieldType, validTypes))
					{
						return false;
					}
				}
			}
			validTypes.Add(type);
			return true;
		}

		// Token: 0x06001871 RID: 6257 RVA: 0x0005D658 File Offset: 0x0005B858
		[BurstDiscard]
		private static void ValidateTypeForSerialization(TypeManager.TypeInfo typeInfo)
		{
			if (typeInfo.TypeIndex.IsChunkSerializable)
			{
				return;
			}
			if (typeInfo.Category == TypeManager.TypeCategory.ISharedComponentData && typeInfo.TypeIndex.HasEntityReferences)
			{
				throw new ArgumentException(string.Format("Shared component type '{0}' might contain a (potentially nested) Entity field. ", typeInfo.Type) + "Serializing of shared components with Entity fields is not supported as Entity references are not patched when deserializing. If for whatever reason this component should still be serialized, add the [ChunkSerializable] attribute to your type to bypass this error.");
			}
			throw new ArgumentException(string.Format("Blittable component type '{0}' contains a (potentially nested) pointer field. ", typeInfo.Type) + "Serializing bare pointers will likely lead to runtime errors. Remove this field and consider serializing the data it points to another way such as by using a BlobAssetReference or a [Serializable] ISharedComponent. If for whatever reason the pointer field should in fact be serialized, add the [ChunkSerializable] attribute to your type to bypass this error.");
		}

		// Token: 0x06001872 RID: 6258 RVA: 0x0005D6D0 File Offset: 0x0005B8D0
		private static int ReadSharedComponentMetadata(BinaryReader reader, out NativeArray<SerializeUtility.SharedComponentRecord> sharedComponentRecordArray)
		{
			int num = reader.ReadInt();
			sharedComponentRecordArray = new NativeArray<SerializeUtility.SharedComponentRecord>(num, Allocator.Temp, NativeArrayOptions.ClearMemory);
			reader.ReadArray<SerializeUtility.SharedComponentRecord>(sharedComponentRecordArray, num);
			return num;
		}

		// Token: 0x06001873 RID: 6259 RVA: 0x0005D700 File Offset: 0x0005B900
		private unsafe static void GatherAllUsedBlobAssets(EntityManager entityManager, NativeArray<int> sharedComponentIndices, UnsafePtrList<Archetype> archetypeArray, out NativeList<BlobAssetPtr> blobAssets, out NativeParallelHashMap<BlobAssetPtr, int> blobAssetMap)
		{
			blobAssetMap = new NativeParallelHashMap<BlobAssetPtr, int>(100, Allocator.TempJob);
			blobAssets = new NativeList<BlobAssetPtr>(100, Allocator.TempJob);
			EntityDataAccess* checkedEntityDataAccess = entityManager.GetCheckedEntityDataAccess();
			for (int i = 0; i < archetypeArray.Length; i++)
			{
				Archetype* ptr = *(IntPtr*)(archetypeArray.Ptr + (IntPtr)i * (IntPtr)sizeof(Archetype*) / (IntPtr)sizeof(Archetype*));
				if (ptr->HasBlobAssetRefs)
				{
					int typesCount = ptr->TypesCount;
					for (int j = 0; j < ptr->Chunks.Count; j++)
					{
						ChunkIndex chunkIndex = ptr->Chunks[j];
						int count = chunkIndex.Count;
						for (int k = 0; k < typesCount; k++)
						{
							int num = ptr->TypeMemoryOrderIndexToIndexInArchetype[k];
							ComponentTypeInArchetype componentTypeInArchetype = ptr->Types[num];
							if (!componentTypeInArchetype.IsZeroSized && !componentTypeInArchetype.IsManagedComponent)
							{
								readonly ref TypeManager.TypeInfo typeInfo = ref TypeManager.GetTypeInfo(componentTypeInArchetype.TypeIndex);
								int blobAssetRefOffsetCount = typeInfo.BlobAssetRefOffsetCount;
								if (blobAssetRefOffsetCount != 0)
								{
									TypeManager.EntityOffsetInfo* blobAssetRefOffsets = TypeManager.GetBlobAssetRefOffsets(in typeInfo);
									byte* buffer = chunkIndex.Buffer;
									if (blobAssetRefOffsetCount > 0)
									{
										int num2 = ptr->Offsets[num];
										byte* ptr2 = SerializeUtility.OffsetFromPointer((void*)buffer, num2);
										if (componentTypeInArchetype.IsBuffer)
										{
											BufferHeader* ptr3 = (BufferHeader*)ptr2;
											int num3 = (int)ptr->SizeOfs[num];
											int elementSize = typeInfo.ElementSize;
											for (int l = 0; l < count; l++)
											{
												byte* elementPointer = BufferHeader.GetElementPointer(ptr3);
												byte* ptr4 = elementPointer + ptr3->Length * elementSize;
												for (byte* ptr5 = elementPointer; ptr5 < ptr4; ptr5 += elementSize)
												{
													SerializeUtility.AddBlobAssetRefInfo(ptr5, blobAssetRefOffsets, blobAssetRefOffsetCount, ref blobAssetMap, ref blobAssets);
												}
												ptr3 = (BufferHeader*)SerializeUtility.OffsetFromPointer((void*)ptr3, num3);
											}
										}
										else
										{
											int num4 = (int)ptr->SizeOfs[num];
											byte* ptr6 = ptr2 + num4 * count;
											for (byte* ptr7 = ptr2; ptr7 < ptr6; ptr7 += num4)
											{
												SerializeUtility.AddBlobAssetRefInfo(ptr7, blobAssetRefOffsets, blobAssetRefOffsetCount, ref blobAssetMap, ref blobAssets);
											}
										}
									}
								}
							}
						}
						SharedComponentValues sharedComponentValues = ptr->Chunks.GetSharedComponentValues(chunkIndex.ListIndex);
						for (int m = 0; m < ptr->NumSharedComponents; m++)
						{
							int num5 = *sharedComponentValues[m];
							if (TypeManager.GetTypeInfo(EntityComponentStore.GetComponentTypeFromSharedComponentIndex(num5)).HasBlobAssetRefs && EntityComponentStore.IsUnmanagedSharedComponentIndex(num5))
							{
								TypeIndex componentTypeFromSharedComponentIndex = EntityComponentStore.GetComponentTypeFromSharedComponentIndex(num5);
								readonly ref TypeManager.TypeInfo typeInfo2 = ref TypeManager.GetTypeInfo(componentTypeFromSharedComponentIndex);
								int blobAssetRefOffsetCount2 = typeInfo2.BlobAssetRefOffsetCount;
								if (blobAssetRefOffsetCount2 > 0)
								{
									byte* sharedComponentDataAddr_Unmanaged = (byte*)checkedEntityDataAccess->EntityComponentStore->GetSharedComponentDataAddr_Unmanaged(num5, componentTypeFromSharedComponentIndex);
									TypeManager.EntityOffsetInfo* blobAssetRefOffsets2 = TypeManager.GetBlobAssetRefOffsets(in typeInfo2);
									SerializeUtility.AddBlobAssetRefInfo(sharedComponentDataAddr_Unmanaged, blobAssetRefOffsets2, blobAssetRefOffsetCount2, ref blobAssetMap, ref blobAssets);
								}
							}
						}
					}
				}
			}
			ManagedComponentStore managedComponentStore = checkedEntityDataAccess->ManagedComponentStore;
			ManagedObjectBlobs managedObjectBlobs = new ManagedObjectBlobs();
			for (int n = 0; n < sharedComponentIndices.Length; n++)
			{
				int num6 = sharedComponentIndices[n];
				if (checkedEntityDataAccess->HasBlobReferences(num6) && !EntityComponentStore.IsUnmanagedSharedComponentIndex(num6))
				{
					object sharedComponentDataNonDefaultBoxed = checkedEntityDataAccess->GetSharedComponentDataNonDefaultBoxed(num6);
					managedObjectBlobs.GatherBlobAssetReferences(sharedComponentDataNonDefaultBoxed, blobAssets, blobAssetMap);
				}
			}
			for (int num7 = 0; num7 < archetypeArray.Length; num7++)
			{
				Archetype* ptr8 = *(IntPtr*)(archetypeArray.Ptr + (IntPtr)num7 * (IntPtr)sizeof(Archetype*) / (IntPtr)sizeof(Archetype*));
				if (ptr8->NumManagedComponents != 0)
				{
					for (int num8 = 0; num8 < ptr8->Chunks.Count; num8++)
					{
						ChunkIndex chunkIndex2 = ptr8->Chunks[num8];
						for (int num9 = 0; num9 < ptr8->NumManagedComponents; num9++)
						{
							int num10 = ptr8->TypeMemoryOrderIndexToIndexInArchetype[(int)ptr8->FirstManagedComponent + num9];
							int* componentDataRO = (int*)ChunkDataUtility.GetComponentDataRO(chunkIndex2, ptr8, 0, num10);
							readonly ref TypeManager.TypeInfo typeInfo3 = ref TypeManager.GetTypeInfo(ptr8->Types[num10].TypeIndex);
							int num11 = 0;
							int count2 = chunkIndex2.Count;
							while (num11 < count2)
							{
								if (componentDataRO[num11] != 0)
								{
									object managedComponent = managedComponentStore.GetManagedComponent(componentDataRO[num11]);
									if (managedComponent != null)
									{
										if (managedComponent.GetType() != typeInfo3.Type)
										{
											throw new InvalidOperationException(string.Format("Managed object type {0} doesn't match component type in archetype {1}", managedComponent.GetType(), typeInfo3.Type));
										}
										if (typeInfo3.HasBlobAssetRefs)
										{
											managedObjectBlobs.GatherBlobAssetReferences(managedComponent, blobAssets, blobAssetMap);
										}
									}
								}
								num11++;
							}
						}
					}
				}
			}
		}

		// Token: 0x06001874 RID: 6260 RVA: 0x0005DB6C File Offset: 0x0005BD6C
		private unsafe static void AddBlobAssetRefInfo(byte* componentData, TypeManager.EntityOffsetInfo* blobAssetRefOffsets, int blobAssetRefCount, ref NativeParallelHashMap<BlobAssetPtr, int> blobAssetMap, ref NativeList<BlobAssetPtr> blobAssets)
		{
			for (int i = 0; i < blobAssetRefCount; i++)
			{
				int offset = blobAssetRefOffsets[i].Offset;
				BlobAssetReferenceData* ptr = (BlobAssetReferenceData*)(componentData + offset);
				if (ptr->m_Ptr != null)
				{
					BlobAssetPtr blobAssetPtr = new BlobAssetPtr(ptr->Header);
					int length;
					if (!blobAssetMap.TryGetValue(blobAssetPtr, out length))
					{
						length = blobAssets.Length;
						blobAssets.Add(in blobAssetPtr);
						blobAssetMap.TryAdd(blobAssetPtr, length);
					}
				}
			}
		}

		// Token: 0x06001875 RID: 6261 RVA: 0x0005DBD8 File Offset: 0x0005BDD8
		private unsafe static void GetAndPatchUnityObjectRefsInChunkBeforeSave(Archetype* archetype, byte* chunkBuffer, int entityCount, UnityObjectRefMap unityObjectRefs)
		{
			int typesCount = archetype->TypesCount;
			for (int i = 0; i < typesCount; i++)
			{
				int num = archetype->TypeMemoryOrderIndexToIndexInArchetype[i];
				ComponentTypeInArchetype componentTypeInArchetype = archetype->Types[num];
				if (!componentTypeInArchetype.IsZeroSized && !componentTypeInArchetype.IsManagedComponent)
				{
					readonly ref TypeManager.TypeInfo typeInfo = ref TypeManager.GetTypeInfo(componentTypeInArchetype.TypeIndex);
					int unityObjectRefOffsetCount = typeInfo.UnityObjectRefOffsetCount;
					if (unityObjectRefOffsetCount != 0)
					{
						TypeManager.EntityOffsetInfo* unityObjectRefOffsets = TypeManager.GetUnityObjectRefOffsets(in typeInfo);
						int num2 = archetype->Offsets[num];
						byte* ptr = SerializeUtility.OffsetFromPointer((void*)chunkBuffer, num2);
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
									SerializeUtility.GetAndPatchUnityObjectRefsInComponentBeforeSave(ptr4, unityObjectRefOffsets, unityObjectRefOffsetCount, unityObjectRefs);
								}
								ptr2 = (BufferHeader*)SerializeUtility.OffsetFromPointer((void*)ptr2, num3);
							}
						}
						else
						{
							int num4 = (int)archetype->SizeOfs[num];
							byte* ptr5 = ptr + num4 * entityCount;
							for (byte* ptr6 = ptr; ptr6 < ptr5; ptr6 += num4)
							{
								SerializeUtility.GetAndPatchUnityObjectRefsInComponentBeforeSave(ptr6, unityObjectRefOffsets, unityObjectRefOffsetCount, unityObjectRefs);
							}
						}
					}
				}
			}
		}

		// Token: 0x06001876 RID: 6262 RVA: 0x0005DD20 File Offset: 0x0005BF20
		private unsafe static void GetAndPatchUnityObjectRefsInComponentBeforeSave(byte* componentData, TypeManager.EntityOffsetInfo* unityObjectRefOffsets, int unityObjectRefCount, UnityObjectRefMap unityObjectRefs)
		{
			for (int i = 0; i < unityObjectRefCount; i++)
			{
				int offset = unityObjectRefOffsets[i].Offset;
				UntypedUnityObjectRef* ptr = (UntypedUnityObjectRef*)(componentData + offset);
				int num = unityObjectRefs.Add(ptr->instanceId);
				ptr->instanceId = num;
			}
		}

		// Token: 0x06001877 RID: 6263 RVA: 0x0005DD64 File Offset: 0x0005BF64
		private unsafe static void GetWeakAssetRefsInChunk(Archetype* archetype, byte* chunkBuffer, int entityCount, NativeParallelHashSet<UntypedWeakReferenceId> weakAssetRefs)
		{
			int typesCount = archetype->TypesCount;
			for (int i = 0; i < typesCount; i++)
			{
				int num = archetype->TypeMemoryOrderIndexToIndexInArchetype[i];
				ComponentTypeInArchetype componentTypeInArchetype = archetype->Types[num];
				if (!componentTypeInArchetype.IsZeroSized && !componentTypeInArchetype.IsManagedComponent)
				{
					readonly ref TypeManager.TypeInfo typeInfo = ref TypeManager.GetTypeInfo(componentTypeInArchetype.TypeIndex);
					int weakAssetRefOffsetCount = typeInfo.WeakAssetRefOffsetCount;
					if (weakAssetRefOffsetCount != 0)
					{
						TypeManager.EntityOffsetInfo* weakAssetRefOffsets = TypeManager.GetWeakAssetRefOffsets(in typeInfo);
						int num2 = archetype->Offsets[num];
						byte* ptr = SerializeUtility.OffsetFromPointer((void*)chunkBuffer, num2);
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
									SerializeUtility.GetWeakAssetRefsInComponent(ptr4, weakAssetRefOffsets, weakAssetRefOffsetCount, weakAssetRefs);
								}
								ptr2 = (BufferHeader*)SerializeUtility.OffsetFromPointer((void*)ptr2, num3);
							}
						}
						else
						{
							int num4 = (int)archetype->SizeOfs[num];
							byte* ptr5 = ptr + num4 * entityCount;
							for (byte* ptr6 = ptr; ptr6 < ptr5; ptr6 += num4)
							{
								SerializeUtility.GetWeakAssetRefsInComponent(ptr6, weakAssetRefOffsets, weakAssetRefOffsetCount, weakAssetRefs);
							}
						}
					}
				}
			}
		}

		// Token: 0x06001878 RID: 6264 RVA: 0x0005DEAC File Offset: 0x0005C0AC
		private unsafe static void GetWeakAssetRefsInComponent(byte* componentData, TypeManager.EntityOffsetInfo* weakAssetRefOffsets, int weakAssetRefCount, NativeParallelHashSet<UntypedWeakReferenceId> weakAssetRefs)
		{
			for (int i = 0; i < weakAssetRefCount; i++)
			{
				UntypedWeakReferenceId untypedWeakReferenceId = *(UntypedWeakReferenceId*)(componentData + weakAssetRefOffsets[i].Offset);
				weakAssetRefs.Add(untypedWeakReferenceId);
			}
		}

		// Token: 0x06001879 RID: 6265 RVA: 0x0005DEE8 File Offset: 0x0005C0E8
		private unsafe static void PatchBlobAssetsInChunkBeforeSave(Archetype* archetype, byte* chunkBuffer, int entityCount, NativeArray<int> blobAssetOffsets, NativeParallelHashMap<BlobAssetPtr, int> blobAssetMap)
		{
			int typesCount = archetype->TypesCount;
			for (int i = 0; i < typesCount; i++)
			{
				int num = archetype->TypeMemoryOrderIndexToIndexInArchetype[i];
				ComponentTypeInArchetype componentTypeInArchetype = archetype->Types[num];
				if (!componentTypeInArchetype.IsZeroSized && !componentTypeInArchetype.IsManagedComponent)
				{
					readonly ref TypeManager.TypeInfo typeInfo = ref TypeManager.GetTypeInfo(componentTypeInArchetype.TypeIndex);
					int blobAssetRefOffsetCount = typeInfo.BlobAssetRefOffsetCount;
					if (blobAssetRefOffsetCount != 0)
					{
						TypeManager.EntityOffsetInfo* blobAssetRefOffsets = TypeManager.GetBlobAssetRefOffsets(in typeInfo);
						int num2 = archetype->Offsets[num];
						byte* ptr = SerializeUtility.OffsetFromPointer((void*)chunkBuffer, num2);
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
									SerializeUtility.PatchBlobAssetRefInfoBeforeSave(ptr4, blobAssetRefOffsets, blobAssetRefOffsetCount, blobAssetOffsets, blobAssetMap);
								}
								ptr2 = (BufferHeader*)SerializeUtility.OffsetFromPointer((void*)ptr2, num3);
							}
						}
						else if (blobAssetRefOffsetCount > 0)
						{
							int num4 = (int)archetype->SizeOfs[num];
							byte* ptr5 = ptr + num4 * entityCount;
							for (byte* ptr6 = ptr; ptr6 < ptr5; ptr6 += num4)
							{
								SerializeUtility.PatchBlobAssetRefInfoBeforeSave(ptr6, blobAssetRefOffsets, blobAssetRefOffsetCount, blobAssetOffsets, blobAssetMap);
							}
						}
					}
				}
			}
		}

		// Token: 0x0600187A RID: 6266 RVA: 0x0005E03C File Offset: 0x0005C23C
		private unsafe static void PatchBlobAssetRefInfoBeforeSave(byte* componentData, TypeManager.EntityOffsetInfo* blobAssetRefOffsets, int blobAssetRefCount, NativeArray<int> blobAssetOffsets, NativeParallelHashMap<BlobAssetPtr, int> blobAssetMap)
		{
			for (int i = 0; i < blobAssetRefCount; i++)
			{
				int offset = blobAssetRefOffsets[i].Offset;
				BlobAssetReferenceData* ptr = (BlobAssetReferenceData*)(componentData + offset);
				int num = -1;
				if (ptr->m_Ptr != null)
				{
					num = blobAssetMap[new BlobAssetPtr(ptr->Header)];
					num = blobAssetOffsets[num];
				}
				ptr->m_Ptr = num;
			}
		}

		// Token: 0x0600187B RID: 6267 RVA: 0x0005E09C File Offset: 0x0005C29C
		private unsafe static void FillBufferPatchRecordsAndClearBufferPointer(Archetype* archetype, byte* chunkBuffer, int entityCount, NativeList<SerializeUtility.BufferPatchRecord> bufferPatches, NativeList<IntPtr> bufferPtrs)
		{
			for (int i = 0; i < archetype->TypesCount; i++)
			{
				int num = archetype->TypeMemoryOrderIndexToIndexInArchetype[i];
				ComponentTypeInArchetype componentTypeInArchetype = archetype->Types[num];
				if (componentTypeInArchetype.IsBuffer)
				{
					int elementSize = TypeManager.GetTypeInfo(componentTypeInArchetype.TypeIndex).ElementSize;
					for (int j = 0; j < entityCount; j++)
					{
						int num2 = archetype->Offsets[num];
						ushort num3 = archetype->SizeOfs[num];
						BufferHeader* ptr = (BufferHeader*)(chunkBuffer + (num2 + (int)num3 * j));
						if (ptr->Pointer != null)
						{
							int num4 = elementSize * ptr->Capacity;
							SerializeUtility.BufferPatchRecord bufferPatchRecord = default(SerializeUtility.BufferPatchRecord);
							bufferPatchRecord.ChunkOffset = (int)((long)((byte*)ptr - (byte*)chunkBuffer));
							bufferPatchRecord.AllocSizeBytes = num4;
							bufferPatches.Add(in bufferPatchRecord);
							IntPtr intPtr = (IntPtr)((void*)ptr->Pointer);
							bufferPtrs.Add(in intPtr);
							ptr->Pointer = null;
						}
					}
				}
			}
		}

		// Token: 0x0600187C RID: 6268 RVA: 0x0005E1A0 File Offset: 0x0005C3A0
		private unsafe static void FillSharedComponentIndexRemap(int* remapArray, Archetype* archetype)
		{
			int num = 0;
			for (int i = 1; i < archetype->TypesCount; i++)
			{
				int num2 = archetype->TypeMemoryOrderIndexToIndexInArchetype[i] - (int)archetype->FirstSharedComponent;
				if (0 <= num2 && num2 < archetype->NumSharedComponents)
				{
					remapArray[num++] = num2;
				}
			}
		}

		// Token: 0x0600187D RID: 6269 RVA: 0x0005E1F0 File Offset: 0x0005C3F0
		private unsafe static void FillSharedComponentArrays(NativeArray<int> sharedComponentArrays, UnsafePtrList<Archetype> archetypeArray, NativeParallelHashMap<int, int> sharedComponentMapping)
		{
			int* ptr = stackalloc int[(UIntPtr)64];
			int num = 0;
			for (int i = 0; i < archetypeArray.Length; i++)
			{
				Archetype* ptr2 = *(IntPtr*)(archetypeArray.Ptr + (IntPtr)i * (IntPtr)sizeof(Archetype*) / (IntPtr)sizeof(Archetype*));
				int numSharedComponents = ptr2->NumSharedComponents;
				if (numSharedComponents != 0)
				{
					SerializeUtility.FillSharedComponentIndexRemap(ptr, ptr2);
					ArchetypeChunkData chunks = ptr2->Chunks;
					for (int j = 0; j < chunks.Count; j++)
					{
						SharedComponentValues sharedComponentValues = chunks.GetSharedComponentValues(chunks[j].ListIndex);
						for (int k = 0; k < numSharedComponents; k++)
						{
							int num2 = ptr[k];
							sharedComponentArrays[num++] = sharedComponentMapping[*sharedComponentValues[num2]];
						}
					}
				}
			}
		}

		// Token: 0x0600187E RID: 6270 RVA: 0x0005E2B4 File Offset: 0x0005C4B4
		private unsafe static NativeParallelHashMap<int, int> GatherSharedComponents(UnsafePtrList<Archetype> archetypeArray, out int sharedComponentArraysTotalCount)
		{
			sharedComponentArraysTotalCount = 0;
			NativeParallelHashMap<int, int> nativeParallelHashMap = new NativeParallelHashMap<int, int>(1024, Allocator.Temp);
			nativeParallelHashMap.TryAdd(0, 0);
			int num = 1;
			for (int i = 0; i < archetypeArray.Length; i++)
			{
				Archetype* ptr = *(IntPtr*)(archetypeArray.Ptr + (IntPtr)i * (IntPtr)sizeof(Archetype*) / (IntPtr)sizeof(Archetype*));
				sharedComponentArraysTotalCount += ptr->Chunks.Count * ptr->NumSharedComponents;
				int numSharedComponents = ptr->NumSharedComponents;
				for (int j = 0; j < numSharedComponents; j++)
				{
					int* sharedComponentValueArrayForType = ptr->Chunks.GetSharedComponentValueArrayForType(j);
					for (int k = 0; k < ptr->Chunks.Count; k++)
					{
						int num2 = sharedComponentValueArrayForType[k];
						int num3;
						if (!nativeParallelHashMap.TryGetValue(num2, out num3))
						{
							nativeParallelHashMap.TryAdd(num2, num++);
						}
					}
				}
			}
			return nativeParallelHashMap;
		}

		// Token: 0x0600187F RID: 6271 RVA: 0x0005E390 File Offset: 0x0005C590
		private unsafe static void ClearChunkHeaderComponents(Archetype* archetype, byte* chunkBuffer, int entityCount)
		{
			TypeIndex typeIndex = TypeManager.GetTypeIndex<ChunkHeader>();
			int indexInTypeArray = ChunkDataUtility.GetIndexInTypeArray(archetype, typeIndex);
			if (indexInTypeArray == -1)
			{
				return;
			}
			int num = archetype->Offsets[indexInTypeArray];
			ChunkHeader* ptr = (ChunkHeader*)(chunkBuffer + num);
			for (int i = 0; i < entityCount; i++)
			{
				ptr[i] = ChunkHeader.Null;
			}
		}

		// Token: 0x06001880 RID: 6272 RVA: 0x0005A591 File Offset: 0x00058791
		private unsafe static byte* OffsetFromPointer(void* ptr, int offset)
		{
			return (byte*)ptr + offset;
		}

		// Token: 0x06001881 RID: 6273 RVA: 0x0005E3E8 File Offset: 0x0005C5E8
		private unsafe static void WriteArchetypes(BinaryWriter writer, UnsafePtrList<Archetype> archetypeArray, UnsafeHashMap<ulong, int> typeHashToIndexMap)
		{
			writer.Write(archetypeArray.Length);
			for (int i = 0; i < archetypeArray.Length; i++)
			{
				Archetype* ptr = *(IntPtr*)(archetypeArray.Ptr + (IntPtr)i * (IntPtr)sizeof(Archetype*) / (IntPtr)sizeof(Archetype*));
				writer.Write(ptr->EntityCount);
				writer.Write(ptr->TypesCount - 1);
				for (int j = 1; j < ptr->TypesCount; j++)
				{
					ComponentTypeInArchetype componentTypeInArchetype = ptr->Types[j];
					int num = (componentTypeInArchetype.IsChunkComponent ? 536870912 : 0);
					ulong stableTypeHash = TypeManager.GetTypeInfo(componentTypeInArchetype.TypeIndex).StableTypeHash;
					writer.Write(typeHashToIndexMap[stableTypeHash] | num);
				}
			}
		}

		// Token: 0x06001882 RID: 6274 RVA: 0x0005E4A4 File Offset: 0x0005C6A4
		private unsafe static int GenerateRemapInfo(EntityManager entityManager, UnsafePtrList<Archetype> archetypeArray, NativeArray<EntityRemapUtility.EntityRemapInfo> entityRemapInfos)
		{
			int num = 0;
			int num2 = 0;
			for (int i = 0; i < archetypeArray.Length; i++)
			{
				Archetype* ptr = *(IntPtr*)(archetypeArray.Ptr + (IntPtr)i * (IntPtr)sizeof(Archetype*) / (IntPtr)sizeof(Archetype*));
				for (int j = 0; j < ptr->Chunks.Count; j++)
				{
					ChunkIndex chunkIndex = ptr->Chunks[j];
					int k = 0;
					int count = chunkIndex.Count;
					while (k < count)
					{
						Entity entity = *(Entity*)ChunkDataUtility.GetComponentDataRO(chunkIndex, ptr, k, 0);
						EntityRemapUtility.AddEntityRemapping(ref entityRemapInfos, entity, new Entity
						{
							Version = 1,
							Index = num
						});
						num++;
						k++;
					}
					num2++;
				}
			}
			return num2;
		}

		// Token: 0x06001884 RID: 6276 RVA: 0x0005E624 File Offset: 0x0005C824
		[CompilerGenerated]
		internal static bool <ReadTypeArray>g__GetTypeName|10_0(ulong stableHash, out FixedString512Bytes name, ref SerializeUtility.<>c__DisplayClass10_0 A_2)
		{
			if (!A_2.isNameLoaded)
			{
				A_2.isNameLoaded = true;
				DotsSerializationReader.NodeHandle nodeHandle = A_2.dotsReader.RootNode.FindNode(SerializeUtility.DebugSectionNodeType, 2);
				if (nodeHandle.IsValid)
				{
					A_2.hasTypesName = true;
					A_2.stringStable = A_2.dotsReader.OpenStringTableNode(nodeHandle.FindNode<DotsSerialization.StringTableNode>(int.MaxValue));
					DotsSerializationReader.NodeHandle nodeHandle2 = nodeHandle.FindNode<DotsSerialization.TypeNamesNode>(int.MaxValue);
					int typeCount = nodeHandle2.As<DotsSerialization.TypeNamesNode>().TypeCount;
					A_2.namesFromStableHash = new UnsafeParallelHashMap<ulong, int>(typeCount, Allocator.Temp);
					using (DotsSerializationReader.ReaderHandle readerHandle = nodeHandle2.GetReaderHandle())
					{
						BinaryReader reader = readerHandle.Reader;
						for (int i = 0; i < typeCount; i++)
						{
							A_2.namesFromStableHash.Add(reader.ReadULong(), reader.ReadInt());
						}
					}
				}
			}
			if (!A_2.hasTypesName)
			{
				name = default(FixedString512Bytes);
				return false;
			}
			name = A_2.stringStable.GetString512(A_2.namesFromStableHash[stableHash]);
			return true;
		}

		// Token: 0x06001885 RID: 6277 RVA: 0x0005E744 File Offset: 0x0005C944
		[CompilerGenerated]
		internal static void <EndDeserializeWorld>g__CleanupWhenDeserializationIsInterrupted|34_0(ref SerializeUtility.WorldDeserializationStatus status)
		{
			UnsafeList<SerializeUtility.MegaChunkInfo> megaChunkInfoList = status.MegaChunkInfoList;
			int length = megaChunkInfoList.Length;
			for (int i = 0; i < length; i++)
			{
				int megaChunkSize = megaChunkInfoList[i].MegaChunkSize;
				EntityComponentStore.FreeContiguousChunks(new ChunkIndex(megaChunkInfoList[i].MegaChunkIndex), megaChunkSize);
			}
			status.Dispose();
		}

		// Token: 0x04000940 RID: 2368
		public const int CurrentFileFormatVersion = 78;

		// Token: 0x04000941 RID: 2369
		internal const int MaxSubsceneHeaderSize = 65536;

		// Token: 0x04000942 RID: 2370
		internal const string k_ExportedTypesDebugLogFileName = "ExportedTypes.log";

		// Token: 0x04000943 RID: 2371
		internal static Hash128 WorldFileType = new Hash128("7F090F7311DF4BA5BA3094AA33D0FAA7");

		// Token: 0x04000944 RID: 2372
		internal static Hash128 WorldNodeType = new Hash128("E9FDFDEB4775443ABB236D4858232894");

		// Token: 0x04000945 RID: 2373
		internal static Hash128 DebugSectionNodeType = new Hash128("582BAFE96EC44A72ACD9CCBC850FDBC4");

		// Token: 0x04000946 RID: 2374
		internal static Hash128 TypesNameStringTableNodeType = new Hash128("4847374145504072849AAC7B1921E7FD");

		// Token: 0x04000947 RID: 2375
		internal static Hash128 TypesNameNodeType = new Hash128("828510FAD38F4EA78CD908D4D780388B");

		// Token: 0x04000948 RID: 2376
		internal static Hash128 ArchetypesNodeType = new Hash128("F5364E1CCB62466A9883F6F9554D4F0C");

		// Token: 0x04000949 RID: 2377
		internal static Hash128 SharedAndManagedComponentsNodeType = new Hash128("D565355C5CF34C0DBBD4A06ADDA948B1");

		// Token: 0x0400094A RID: 2378
		internal static Hash128 EnabledBitsNodeType = new Hash128("5846E500EA614C1DA94AFB85AFD8F4F4");

		// Token: 0x0400094B RID: 2379
		internal static Hash128 BlobAssetsNodeType = new Hash128("9A26954FF1ED4CC5A64E8AAD4F64773A");

		// Token: 0x0400094C RID: 2380
		internal static Hash128 ChunksNodeType = new Hash128("2EA7CE3325F04D7A84CEAB46790C628A");

		// Token: 0x0400094D RID: 2381
		internal static Hash128 BufferDataNodeType = new Hash128("E33124BFAC2649D792DE36E4611DDD70");

		// Token: 0x0400094E RID: 2382
		internal static Hash128 PrefabNodeType = new Hash128("2A84A183583A4FAD8CB7105AE3C47598");

		// Token: 0x02000395 RID: 917
		internal struct Settings
		{
			// Token: 0x17000225 RID: 549
			// (get) Token: 0x06001886 RID: 6278 RVA: 0x0005E79E File Offset: 0x0005C99E
			internal bool RequiresDebugSection
			{
				get
				{
					return this.SerializeComponentTypeNames;
				}
			}

			// Token: 0x0400094F RID: 2383
			internal static readonly SerializeUtility.Settings Default = new SerializeUtility.Settings
			{
				SerializeComponentTypeNames = true,
				PrefabRoot = Entity.Null
			};

			// Token: 0x04000950 RID: 2384
			internal bool SerializeComponentTypeNames;

			// Token: 0x04000951 RID: 2385
			internal Entity PrefabRoot;
		}

		// Token: 0x02000396 RID: 918
		private class ManagedObjectWriterAdapter : IBinaryAdapter<Entity>, IBinaryAdapter, IBinaryAdapter<BlobAssetReferenceData>, IBinaryAdapter<UntypedWeakReferenceId>
		{
			// Token: 0x17000226 RID: 550
			// (get) Token: 0x06001888 RID: 6280 RVA: 0x0005E7D7 File Offset: 0x0005C9D7
			// (set) Token: 0x06001889 RID: 6281 RVA: 0x0005E7DF File Offset: 0x0005C9DF
			public bool SerializeEntityReferences { get; set; }

			// Token: 0x0600188A RID: 6282 RVA: 0x0005E7E8 File Offset: 0x0005C9E8
			public unsafe ManagedObjectWriterAdapter(EntityRemapUtility.EntityRemapInfo* entityRemapInfo, NativeParallelHashMap<BlobAssetPtr, int> blobAssetMap, NativeArray<int> blobAssetOffsets, NativeParallelHashSet<UntypedWeakReferenceId> weakAssetRefs)
			{
				this.SerializeEntityReferences = true;
				this.m_EntityRemapInfo = entityRemapInfo;
				this.m_BlobAssetMap = blobAssetMap;
				this.m_BlobAssetOffsets = blobAssetOffsets;
				this.m_WeakAssetRefs = weakAssetRefs;
			}

			// Token: 0x0600188B RID: 6283 RVA: 0x0005E814 File Offset: 0x0005CA14
			unsafe void IBinaryAdapter<Entity>.Serialize(in BinarySerializationContext<Entity> context, Entity value)
			{
				if (this.SerializeEntityReferences)
				{
					value = EntityRemapUtility.RemapEntity(this.m_EntityRemapInfo, value);
					context.Writer->Add<int>(value.Index);
					context.Writer->Add<int>(value.Version);
					return;
				}
				throw new ArgumentException("Tried to serialized an Entity reference however entity reference serialization has been explicitly disabled.");
			}

			// Token: 0x0600188C RID: 6284 RVA: 0x0005E864 File Offset: 0x0005CA64
			unsafe void IBinaryAdapter<BlobAssetReferenceData>.Serialize(in BinarySerializationContext<BlobAssetReferenceData> context, BlobAssetReferenceData value)
			{
				int num = -1;
				if (value.m_Ptr != null)
				{
					int num2;
					if (!this.m_BlobAssetMap.TryGetValue(new BlobAssetPtr(value.Header), out num2))
					{
						throw new InvalidOperationException("Trying to serialize a BlobAssetReference but the asset has not been included in the batch.");
					}
					num = this.m_BlobAssetOffsets[num2];
				}
				context.Writer->Add<int>(num);
			}

			// Token: 0x0600188D RID: 6285 RVA: 0x0005E8C4 File Offset: 0x0005CAC4
			unsafe void IBinaryAdapter<UntypedWeakReferenceId>.Serialize(in BinarySerializationContext<UntypedWeakReferenceId> context, UntypedWeakReferenceId value)
			{
				if (this.m_WeakAssetRefs.IsCreated)
				{
					this.m_WeakAssetRefs.Add(value);
				}
				context.Writer->Add<UntypedWeakReferenceId>(value);
			}

			// Token: 0x0600188E RID: 6286 RVA: 0x0005E8FC File Offset: 0x0005CAFC
			Entity IBinaryAdapter<Entity>.Deserialize(in BinaryDeserializationContext<Entity> context)
			{
				throw new InvalidOperationException("ManagedObjectWriterAdapter should only be used for writing and never for reading!");
			}

			// Token: 0x0600188F RID: 6287 RVA: 0x0005E914 File Offset: 0x0005CB14
			BlobAssetReferenceData IBinaryAdapter<BlobAssetReferenceData>.Deserialize(in BinaryDeserializationContext<BlobAssetReferenceData> context)
			{
				throw new InvalidOperationException("ManagedObjectWriterAdapter should only be used for writing and never for reading!");
			}

			// Token: 0x06001890 RID: 6288 RVA: 0x0005E92C File Offset: 0x0005CB2C
			UntypedWeakReferenceId IBinaryAdapter<UntypedWeakReferenceId>.Deserialize(in BinaryDeserializationContext<UntypedWeakReferenceId> context)
			{
				throw new InvalidOperationException("ManagedObjectWriterAdapter should only be used for writing and never for reading!");
			}

			// Token: 0x04000953 RID: 2387
			private unsafe readonly EntityRemapUtility.EntityRemapInfo* m_EntityRemapInfo;

			// Token: 0x04000954 RID: 2388
			private readonly NativeParallelHashMap<BlobAssetPtr, int> m_BlobAssetMap;

			// Token: 0x04000955 RID: 2389
			private readonly NativeParallelHashSet<UntypedWeakReferenceId> m_WeakAssetRefs;

			// Token: 0x04000956 RID: 2390
			private readonly NativeArray<int> m_BlobAssetOffsets;
		}

		// Token: 0x02000397 RID: 919
		private class ManagedObjectReaderAdapter : IBinaryAdapter<Entity>, IBinaryAdapter, IBinaryAdapter<BlobAssetReferenceData>
		{
			// Token: 0x06001891 RID: 6289 RVA: 0x0005E943 File Offset: 0x0005CB43
			public unsafe ManagedObjectReaderAdapter(byte* blobAssetBatch)
			{
				this.m_BlobAssetBatch = blobAssetBatch;
			}

			// Token: 0x06001892 RID: 6290 RVA: 0x0005E952 File Offset: 0x0005CB52
			void IBinaryAdapter<Entity>.Serialize(in BinarySerializationContext<Entity> context, Entity value)
			{
				throw new InvalidOperationException("ManagedObjectReaderAdapter should only be used for reading and never for writing!");
			}

			// Token: 0x06001893 RID: 6291 RVA: 0x0005E952 File Offset: 0x0005CB52
			void IBinaryAdapter<BlobAssetReferenceData>.Serialize(in BinarySerializationContext<BlobAssetReferenceData> context, BlobAssetReferenceData value)
			{
				throw new InvalidOperationException("ManagedObjectReaderAdapter should only be used for reading and never for writing!");
			}

			// Token: 0x06001894 RID: 6292 RVA: 0x0005E960 File Offset: 0x0005CB60
			unsafe Entity IBinaryAdapter<Entity>.Deserialize(in BinaryDeserializationContext<Entity> context)
			{
				int num;
				context.Reader->ReadNext<int>(out num);
				int num2;
				context.Reader->ReadNext<int>(out num2);
				return new Entity
				{
					Index = num,
					Version = num2
				};
			}

			// Token: 0x06001895 RID: 6293 RVA: 0x0005E9A0 File Offset: 0x0005CBA0
			unsafe BlobAssetReferenceData IBinaryAdapter<BlobAssetReferenceData>.Deserialize(in BinaryDeserializationContext<BlobAssetReferenceData> context)
			{
				int num;
				context.Reader->ReadNext<int>(out num);
				if (num != -1)
				{
					return new BlobAssetReferenceData
					{
						m_Ptr = this.m_BlobAssetBatch + num
					};
				}
				return default(BlobAssetReferenceData);
			}

			// Token: 0x04000957 RID: 2391
			private unsafe readonly byte* m_BlobAssetBatch;
		}

		// Token: 0x02000398 RID: 920
		internal struct BufferPatchRecord
		{
			// Token: 0x04000958 RID: 2392
			public int ChunkOffset;

			// Token: 0x04000959 RID: 2393
			public int AllocSizeBytes;
		}

		// Token: 0x02000399 RID: 921
		internal struct BlobAssetRefPatchRecord
		{
			// Token: 0x0400095A RID: 2394
			public int ChunkOffset;

			// Token: 0x0400095B RID: 2395
			public int BlobDataOffset;
		}

		// Token: 0x0200039A RID: 922
		internal struct SharedComponentRecord
		{
			// Token: 0x0400095C RID: 2396
			public ulong StableTypeHash;

			// Token: 0x0400095D RID: 2397
			public int ComponentSize;
		}

		// Token: 0x0200039B RID: 923
		internal struct MegaChunkInfo
		{
			// Token: 0x06001896 RID: 6294 RVA: 0x0005E9E0 File Offset: 0x0005CBE0
			public MegaChunkInfo(ChunkIndex fistChunkIndex, int chunkCount)
			{
				this.MegaChunkSize = chunkCount;
				this.MegaChunkIndex = fistChunkIndex;
			}

			// Token: 0x0400095E RID: 2398
			public int MegaChunkSize;

			// Token: 0x0400095F RID: 2399
			public ChunkIndex MegaChunkIndex;
		}

		// Token: 0x0200039C RID: 924
		internal struct WorldDeserializationStatus
		{
			// Token: 0x06001897 RID: 6295 RVA: 0x0005E9F0 File Offset: 0x0005CBF0
			public void Dispose()
			{
				this.MegaChunkInfoList.Dispose();
				this.ArchetypePrefetchState.Dispose();
				this.SharedComponentPrefetchState.Dispose();
				this.EnabledBitsPrefetchState.Dispose();
				this.BufferElementPrefetchState.Dispose();
				this.PrefabPrefetchState.Dispose();
				Memory.Unmanaged.Free(this.BlobAssetBuffer, Allocator.Persistent);
			}

			// Token: 0x04000960 RID: 2400
			internal UnsafeList<SerializeUtility.MegaChunkInfo> MegaChunkInfoList;

			// Token: 0x04000961 RID: 2401
			public DotsSerializationReader.NodeHandle.PrefetchState ArchetypePrefetchState;

			// Token: 0x04000962 RID: 2402
			[NativeDisableUnsafePtrRestriction]
			public unsafe void* BlobAssetBuffer;

			// Token: 0x04000963 RID: 2403
			public int BlobAssetSize;

			// Token: 0x04000964 RID: 2404
			public DotsSerializationReader.NodeHandle.PrefetchState SharedComponentPrefetchState;

			// Token: 0x04000965 RID: 2405
			public DotsSerializationReader.NodeHandle.PrefetchState EnabledBitsPrefetchState;

			// Token: 0x04000966 RID: 2406
			public DotsSerializationReader.NodeHandle.PrefetchState BufferElementPrefetchState;

			// Token: 0x04000967 RID: 2407
			public DotsSerializationReader.NodeHandle.PrefetchState PrefabPrefetchState;

			// Token: 0x04000968 RID: 2408
			public int TotalChunkCount;
		}

		// Token: 0x0200039D RID: 925
		internal struct WorldDeserializationResult
		{
			// Token: 0x04000969 RID: 2409
			internal Entity PrefabRoot;
		}
	}
}
