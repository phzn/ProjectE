using System;
using System.Runtime.CompilerServices;
using Unity.Burst;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.IO.LowLevel.Unsafe;

namespace Unity.Entities.Serialization
{
	// Token: 0x0200038D RID: 909
	[BurstCompile]
	internal struct SerializeUtilityInterop
	{
		// Token: 0x06001825 RID: 6181 RVA: 0x0005A4F1 File Offset: 0x000586F1
		[BurstCompile]
		internal unsafe static int AllocAndQueueReadChunkCommands(long readOffset, int totalChunkCount, UnsafeList<SerializeUtility.MegaChunkInfo>* megaChunkInfo, UnsafeList<ReadCommand>* readCommands)
		{
			return SerializeUtilityInterop.AllocAndQueueReadChunkCommands_00001516$BurstDirectCall.Invoke(readOffset, totalChunkCount, megaChunkInfo, readCommands);
		}

		// Token: 0x06001826 RID: 6182 RVA: 0x0005A4FC File Offset: 0x000586FC
		[BurstCompile]
		internal unsafe static void AddExistingChunk(Archetype* archetype, ChunkIndex chunk, int* sharedComponentIndices, byte* enabledBitsValuesForChunk, int* perComponentDisabledBitCount)
		{
			SerializeUtilityInterop.AddExistingChunk_00001517$BurstDirectCall.Invoke(archetype, chunk, sharedComponentIndices, enabledBitsValuesForChunk, perComponentDisabledBitCount);
		}

		// Token: 0x06001827 RID: 6183 RVA: 0x0005A50C File Offset: 0x0005870C
		[BurstCompile]
		internal unsafe static void ImportChunks(SerializeUtility.WorldDeserializationStatus* status, ref BurstableMemoryBinaryReader bufferReader, UnsafeList<EntityArchetype>* archetypes, int* sharedComponentArray, int numSharedComponents, int* sharedComponentRemap, UnsafeList<ArchetypeChunk>* blobAssetRefChunks, byte* componentEnabledBits, int* enabledBitsHierarchicalData, EntityComponentStore* ecs, ref NativeArray<int> instanceIds, UnsafeList<ArchetypeChunk>* chunksWithMetaChunkEntities, Entity* externalEntitiesReferences, int externalEntitiesRefCount, ref Entity prefabRoot)
		{
			SerializeUtilityInterop.ImportChunks_00001518$BurstDirectCall.Invoke(status, ref bufferReader, archetypes, sharedComponentArray, numSharedComponents, sharedComponentRemap, blobAssetRefChunks, componentEnabledBits, enabledBitsHierarchicalData, ecs, ref instanceIds, chunksWithMetaChunkEntities, externalEntitiesReferences, externalEntitiesRefCount, ref prefabRoot);
		}

		// Token: 0x06001828 RID: 6184 RVA: 0x0005A538 File Offset: 0x00058738
		private unsafe static void RemapSharedComponentIndices(int* destValues, Archetype* archetype, int* remappedIndices, int* sourceValues)
		{
			int num = 0;
			for (int i = 1; i < archetype->TypesCount; i++)
			{
				int num2 = archetype->TypeMemoryOrderIndexToIndexInArchetype[i] - (int)archetype->FirstSharedComponent;
				if (0 <= num2 && num2 < archetype->NumSharedComponents)
				{
					destValues[num2] = remappedIndices[sourceValues[num++]];
				}
			}
		}

		// Token: 0x06001829 RID: 6185 RVA: 0x0005A591 File Offset: 0x00058791
		private unsafe static byte* OffsetFromPointer(void* ptr, int offset)
		{
			return (byte*)ptr + offset;
		}

		// Token: 0x0600182A RID: 6186 RVA: 0x0005A598 File Offset: 0x00058798
		private unsafe static void PatchUnityObjectRefsInChunkAfterLoad(Archetype* archetype, byte* chunkBuffer, int entityCount, NativeArray<int> instanceIds)
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
						byte* ptr = SerializeUtilityInterop.OffsetFromPointer((void*)chunkBuffer, num2);
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
									SerializeUtilityInterop.PatchUnityObjectRefsInComponentAfterLoad(ptr4, unityObjectRefOffsets, unityObjectRefOffsetCount, instanceIds);
								}
								ptr2 = (BufferHeader*)SerializeUtilityInterop.OffsetFromPointer((void*)ptr2, num3);
							}
						}
						else
						{
							int num4 = (int)archetype->SizeOfs[num];
							byte* ptr5 = ptr + num4 * entityCount;
							for (byte* ptr6 = ptr; ptr6 < ptr5; ptr6 += num4)
							{
								SerializeUtilityInterop.PatchUnityObjectRefsInComponentAfterLoad(ptr6, unityObjectRefOffsets, unityObjectRefOffsetCount, instanceIds);
							}
						}
					}
				}
			}
		}

		// Token: 0x0600182B RID: 6187 RVA: 0x0005A6E0 File Offset: 0x000588E0
		internal unsafe static void PatchUnityObjectRefsInComponentAfterLoad(byte* componentData, TypeManager.EntityOffsetInfo* unityObjectRefOffsets, int unityObjectRefCount, NativeArray<int> instanceIds)
		{
			for (int i = 0; i < unityObjectRefCount; i++)
			{
				int offset = unityObjectRefOffsets[i].Offset;
				UntypedUnityObjectRef* ptr = (UntypedUnityObjectRef*)(componentData + offset);
				int instanceId = ptr->instanceId;
				int num = ((instanceId != -1) ? instanceIds[instanceId] : 0);
				ptr->instanceId = num;
			}
		}

		// Token: 0x0600182C RID: 6188 RVA: 0x0005A730 File Offset: 0x00058930
		private unsafe static void PatchBlobAssetsInChunkAfterLoad(Archetype* archetype, byte* chunkBuffer, int entityCount, byte* allBlobAssetData)
		{
			int typesCount = archetype->TypesCount;
			for (int i = 0; i < typesCount; i++)
			{
				int num = archetype->TypeMemoryOrderIndexToIndexInArchetype[i];
				ComponentTypeInArchetype componentTypeInArchetype = archetype->Types[num];
				if (!componentTypeInArchetype.IsZeroSized)
				{
					readonly ref TypeManager.TypeInfo typeInfo = ref TypeManager.GetTypeInfo(componentTypeInArchetype.TypeIndex);
					int blobAssetRefOffsetCount = typeInfo.BlobAssetRefOffsetCount;
					if (blobAssetRefOffsetCount != 0)
					{
						TypeManager.EntityOffsetInfo* blobAssetRefOffsets = TypeManager.GetBlobAssetRefOffsets(in typeInfo);
						int num2 = archetype->Offsets[num];
						byte* ptr = SerializeUtilityInterop.OffsetFromPointer((void*)chunkBuffer, num2);
						if (componentTypeInArchetype.IsBuffer)
						{
							BufferHeader* ptr2 = (BufferHeader*)ptr;
							int num3 = (int)archetype->SizeOfs[num];
							int elementSize = typeInfo.ElementSize;
							for (int j = 0; j < entityCount; j++)
							{
								byte* elementPointer = BufferHeader.GetElementPointer(ptr2);
								for (int k = 0; k < ptr2->Length; k++)
								{
									byte* ptr3 = elementPointer + k * elementSize;
									for (int l = 0; l < blobAssetRefOffsetCount; l++)
									{
										int offset = blobAssetRefOffsets[l].Offset;
										BlobAssetReferenceData* ptr4 = (BlobAssetReferenceData*)(ptr3 + offset);
										int num4 = ptr4->m_Ptr;
										byte* ptr5 = null;
										if (num4 != -1)
										{
											ptr5 = allBlobAssetData + num4;
										}
										ptr4->m_Ptr = ptr5;
									}
								}
								ptr2 = (BufferHeader*)SerializeUtilityInterop.OffsetFromPointer((void*)ptr2, num3);
							}
						}
						else if (blobAssetRefOffsetCount > 0)
						{
							int num5 = (int)archetype->SizeOfs[num];
							byte* ptr6 = ptr + num5 * entityCount;
							for (byte* ptr7 = ptr; ptr7 < ptr6; ptr7 += num5)
							{
								for (int m = 0; m < blobAssetRefOffsetCount; m++)
								{
									int offset2 = blobAssetRefOffsets[m].Offset;
									BlobAssetReferenceData* ptr8 = (BlobAssetReferenceData*)(ptr7 + offset2);
									int num6 = ptr8->m_Ptr;
									byte* ptr9 = null;
									if (num6 != -1)
									{
										ptr9 = allBlobAssetData + num6;
									}
									ptr8->m_Ptr = ptr9;
								}
							}
						}
					}
				}
			}
		}

		// Token: 0x0600182D RID: 6189 RVA: 0x0005A904 File Offset: 0x00058B04
		[BurstCompile]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal unsafe static int AllocAndQueueReadChunkCommands$BurstManaged(long readOffset, int totalChunkCount, UnsafeList<SerializeUtility.MegaChunkInfo>* megaChunkInfo, UnsafeList<ReadCommand>* readCommands)
		{
			int num = 0;
			while (totalChunkCount != num)
			{
				ChunkIndex chunkIndex;
				int num2;
				if (EntityComponentStore.AllocateContiguousChunk(totalChunkCount - num, out chunkIndex, out num2) != 0)
				{
					return totalChunkCount;
				}
				ReadCommand readCommand;
				readCommand.Buffer = (void*)chunkIndex.GetPtr();
				readCommand.Offset = readOffset;
				readCommand.Size = (long)(16384 * num2);
				readCommands->Add(in readCommand);
				SerializeUtility.MegaChunkInfo megaChunkInfo2 = new SerializeUtility.MegaChunkInfo(chunkIndex, num2);
				megaChunkInfo->Add(in megaChunkInfo2);
				num += num2;
				readOffset += (long)(16384 * num2);
			}
			return totalChunkCount;
		}

		// Token: 0x0600182E RID: 6190 RVA: 0x0005A978 File Offset: 0x00058B78
		[BurstCompile]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal unsafe static void AddExistingChunk$BurstManaged(Archetype* archetype, ChunkIndex chunk, int* sharedComponentIndices, byte* enabledBitsValuesForChunk, int* perComponentDisabledBitCount)
		{
			ChunkDataUtility.AddExistingChunk(archetype, chunk, sharedComponentIndices, enabledBitsValuesForChunk, perComponentDisabledBitCount);
		}

		// Token: 0x0600182F RID: 6191 RVA: 0x0005A988 File Offset: 0x00058B88
		[BurstCompile]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal unsafe static void ImportChunks$BurstManaged(SerializeUtility.WorldDeserializationStatus* status, ref BurstableMemoryBinaryReader bufferReader, UnsafeList<EntityArchetype>* archetypes, int* sharedComponentArray, int numSharedComponents, int* sharedComponentRemap, UnsafeList<ArchetypeChunk>* blobAssetRefChunks, byte* componentEnabledBits, int* enabledBitsHierarchicalData, EntityComponentStore* ecs, ref NativeArray<int> instanceIds, UnsafeList<ArchetypeChunk>* chunksWithMetaChunkEntities, Entity* externalEntitiesReferences, int externalEntitiesRefCount, ref Entity prefabRoot)
		{
			int num = bufferReader.ReadInt();
			int totalChunkCount = status->TotalChunkCount;
			int blobAssetSize = status->BlobAssetSize;
			void* blobAssetBuffer = status->BlobAssetBuffer;
			if (status->TotalChunkCount != num)
			{
				throw new InvalidOperationException("Internal deserialization error: total chunk count doesn't match");
			}
			int num2 = 0;
			UnsafeList<SerializeUtility.MegaChunkInfo> megaChunkInfoList = status->MegaChunkInfoList;
			int length = megaChunkInfoList.Length;
			for (int i = 0; i < length; i++)
			{
				int megaChunkSize = megaChunkInfoList[i].MegaChunkSize;
				ChunkIndex megaChunkIndex = megaChunkInfoList[i].MegaChunkIndex;
				Chunk* ptr = megaChunkIndex.GetPtr();
				for (int j = 0; j < megaChunkSize; j++)
				{
					ChunkIndex chunkIndex = new ChunkIndex(megaChunkIndex + j);
					int countForSerialization = ptr->CountForSerialization;
					chunkIndex.Count = countForSerialization;
					ptr += 16384 / sizeof(Chunk);
					num2 += countForSerialization;
				}
			}
			NativeArray<EntityRemapUtility.EntityRemapInfo> nativeArray = new NativeArray<EntityRemapUtility.EntityRemapInfo>(num2 + externalEntitiesRefCount, Allocator.Temp, NativeArrayOptions.ClearMemory);
			NativeArray<Entity> nativeArray2 = new NativeArray<Entity>(num2, Allocator.Temp, NativeArrayOptions.ClearMemory);
			EntityComponentStore.s_entityStore.Data.AllocateEntities(nativeArray2);
			if (externalEntitiesReferences != null)
			{
				EntityComponentStore.s_entityStore.Data.AllocateEntities(externalEntitiesReferences, externalEntitiesRefCount);
			}
			int num3 = 0;
			for (int k = 0; k < length; k++)
			{
				int megaChunkSize2 = megaChunkInfoList[k].MegaChunkSize;
				ChunkIndex megaChunkIndex2 = megaChunkInfoList[k].MegaChunkIndex;
				for (int l = 0; l < megaChunkSize2; l++)
				{
					ChunkIndex chunkIndex2 = new ChunkIndex(megaChunkIndex2 + l);
					Entity* buffer = (Entity*)chunkIndex2.Buffer;
					int count = chunkIndex2.Count;
					for (int m = 0; m < count; m++)
					{
						Entity entity = nativeArray2[num3 + m];
						EntityRemapUtility.AddEntityRemapping(ref nativeArray, buffer[m], entity);
					}
					num3 += count;
				}
			}
			for (int n = 0; n < externalEntitiesRefCount; n++)
			{
				Entity entity2 = new Entity
				{
					Index = num3 + n,
					Version = 1
				};
				Entity entity3 = new Entity
				{
					Index = externalEntitiesReferences[n].Index,
					Version = 0
				};
				EntityRemapUtility.AddEntityRemapping(ref nativeArray, entity2, entity3);
			}
			int num4 = 0;
			UnsafeList<SerializeUtility.MegaChunkInfo> megaChunkInfoList2 = status->MegaChunkInfoList;
			int num5 = megaChunkInfoList2[0].MegaChunkSize;
			ChunkIndex chunkIndex3 = megaChunkInfoList2[0].MegaChunkIndex;
			int num6 = 0;
			byte* ptr2 = componentEnabledBits;
			int* ptr3 = enabledBitsHierarchicalData;
			int* ptr4 = stackalloc int[(UIntPtr)64];
			for (int num7 = 0; num7 < totalChunkCount; num7++)
			{
				Archetype* archetype = archetypes->ElementAt(chunkIndex3.GetPtr()->ArchetypeIndexForSerialization).Archetype;
				ecs->SetArchetype(chunkIndex3, archetype);
				int numSharedComponents2 = archetype->NumSharedComponents;
				int* ptr5 = sharedComponentArray + num6;
				for (int num8 = 0; num8 < numSharedComponents2; num8++)
				{
					if (ptr5[num8] > numSharedComponents)
					{
						throw new ArgumentException(string.Format("Archetype uses shared component at index {0} but only {1} are available, check if the shared scene has been properly loaded.", ptr5[num8], numSharedComponents));
					}
				}
				SerializeUtilityInterop.RemapSharedComponentIndices(ptr4, archetype, sharedComponentRemap, ptr5);
				num6 += numSharedComponents2;
				int num9 = bufferReader.ReadInt();
				if (num9 > 0)
				{
					for (int num10 = 0; num10 < num9; num10++)
					{
						int num11 = bufferReader.ReadInt();
						int num12 = bufferReader.ReadInt();
						BufferHeader* ptr6 = (BufferHeader*)SerializeUtilityInterop.OffsetFromPointer((void*)chunkIndex3.Buffer, num11);
						ptr6->Pointer = (byte*)Memory.Unmanaged.Allocate((long)num12, 8, Allocator.Persistent);
						bufferReader.ReadBytes((void*)ptr6->Pointer, num12);
					}
				}
				if (blobAssetSize != 0 && archetype->HasBlobAssetRefs)
				{
					ArchetypeChunk archetypeChunk = new ArchetypeChunk(chunkIndex3, ecs);
					blobAssetRefChunks->Add(in archetypeChunk);
					SerializeUtilityInterop.PatchBlobAssetsInChunkAfterLoad(archetype, chunkIndex3.Buffer, chunkIndex3.Count, (byte*)blobAssetBuffer);
				}
				if (archetype->HasUnityObjectRefs)
				{
					SerializeUtilityInterop.PatchUnityObjectRefsInChunkAfterLoad(archetype, chunkIndex3.Buffer, chunkIndex3.Count, instanceIds);
				}
				chunkIndex3.SequenceNumber = ecs->AllocateSequenceNumber();
				chunkIndex3.MetaChunkEntity = EntityRemapUtility.RemapEntity(ref nativeArray, chunkIndex3.MetaChunkEntity);
				EntityRemapUtility.PatchEntities(archetype->ScalarEntityPatches, archetype->ScalarEntityPatchCount, archetype->BufferEntityPatches, archetype->BufferEntityPatchCount, chunkIndex3.Buffer, chunkIndex3.Count, ref nativeArray);
				ecs->ManagedChangesTracker.PatchEntities(archetype, chunkIndex3, chunkIndex3.Count, nativeArray);
				SerializeUtilityInterop.AddExistingChunk(archetype, chunkIndex3, ptr4, ptr2, ptr3);
				ptr2 += archetype->Chunks.ComponentEnabledBitsSizeTotalPerChunk;
				ptr3 += archetype->TypesCount;
				if (chunkIndex3.MetaChunkEntity != Entity.Null)
				{
					ArchetypeChunk archetypeChunk = new ArchetypeChunk(chunkIndex3, ecs);
					chunksWithMetaChunkEntities->Add(in archetypeChunk);
				}
				if (--num5 == 0 && num7 + 1 != totalChunkCount)
				{
					chunkIndex3 = megaChunkInfoList2[++num4].MegaChunkIndex;
					num5 = megaChunkInfoList2[num4].MegaChunkSize;
				}
				else
				{
					chunkIndex3 = new ChunkIndex(chunkIndex3 + 1);
				}
			}
			prefabRoot = EntityRemapUtility.RemapEntity(ref nativeArray, prefabRoot);
		}

		// Token: 0x0200038E RID: 910
		// (Invoke) Token: 0x06001831 RID: 6193
		internal unsafe delegate int AllocAndQueueReadChunkCommands_00001516$PostfixBurstDelegate(long readOffset, int totalChunkCount, UnsafeList<SerializeUtility.MegaChunkInfo>* megaChunkInfo, UnsafeList<ReadCommand>* readCommands);

		// Token: 0x0200038F RID: 911
		internal static class AllocAndQueueReadChunkCommands_00001516$BurstDirectCall
		{
			// Token: 0x06001834 RID: 6196 RVA: 0x0005AE6F File Offset: 0x0005906F
			[BurstDiscard]
			private unsafe static void GetFunctionPointerDiscard(ref IntPtr A_0)
			{
				if (SerializeUtilityInterop.AllocAndQueueReadChunkCommands_00001516$BurstDirectCall.Pointer == 0)
				{
					SerializeUtilityInterop.AllocAndQueueReadChunkCommands_00001516$BurstDirectCall.Pointer = BurstCompiler.GetILPPMethodFunctionPointer2(SerializeUtilityInterop.AllocAndQueueReadChunkCommands_00001516$BurstDirectCall.DeferredCompilation, methodof(SerializeUtilityInterop.AllocAndQueueReadChunkCommands$BurstManaged(long, int, UnsafeList<SerializeUtility.MegaChunkInfo>*, UnsafeList<ReadCommand>*)).MethodHandle, typeof(SerializeUtilityInterop.AllocAndQueueReadChunkCommands_00001516$PostfixBurstDelegate).TypeHandle);
				}
				A_0 = SerializeUtilityInterop.AllocAndQueueReadChunkCommands_00001516$BurstDirectCall.Pointer;
			}

			// Token: 0x06001835 RID: 6197 RVA: 0x0005AE9C File Offset: 0x0005909C
			private static IntPtr GetFunctionPointer()
			{
				IntPtr intPtr = (IntPtr)0;
				SerializeUtilityInterop.AllocAndQueueReadChunkCommands_00001516$BurstDirectCall.GetFunctionPointerDiscard(ref intPtr);
				return intPtr;
			}

			// Token: 0x06001836 RID: 6198 RVA: 0x0005AEB4 File Offset: 0x000590B4
			public unsafe static void Constructor()
			{
				SerializeUtilityInterop.AllocAndQueueReadChunkCommands_00001516$BurstDirectCall.DeferredCompilation = BurstCompiler.CompileILPPMethod2(methodof(SerializeUtilityInterop.AllocAndQueueReadChunkCommands(long, int, UnsafeList<SerializeUtility.MegaChunkInfo>*, UnsafeList<ReadCommand>*)).MethodHandle);
			}

			// Token: 0x06001837 RID: 6199 RVA: 0x00002287 File Offset: 0x00000487
			public static void Initialize()
			{
			}

			// Token: 0x06001838 RID: 6200 RVA: 0x0005AEC5 File Offset: 0x000590C5
			// Note: this type is marked as 'beforefieldinit'.
			static AllocAndQueueReadChunkCommands_00001516$BurstDirectCall()
			{
				SerializeUtilityInterop.AllocAndQueueReadChunkCommands_00001516$BurstDirectCall.Constructor();
			}

			// Token: 0x06001839 RID: 6201 RVA: 0x0005AECC File Offset: 0x000590CC
			public unsafe static int Invoke(long readOffset, int totalChunkCount, UnsafeList<SerializeUtility.MegaChunkInfo>* megaChunkInfo, UnsafeList<ReadCommand>* readCommands)
			{
				if (BurstCompiler.IsEnabled)
				{
					IntPtr functionPointer = SerializeUtilityInterop.AllocAndQueueReadChunkCommands_00001516$BurstDirectCall.GetFunctionPointer();
					if (functionPointer != 0)
					{
						return calli(System.Int32(System.Int64,System.Int32,Unity.Collections.LowLevel.Unsafe.UnsafeList`1<Unity.Entities.Serialization.SerializeUtility/MegaChunkInfo>*,Unity.Collections.LowLevel.Unsafe.UnsafeList`1<Unity.IO.LowLevel.Unsafe.ReadCommand>*), readOffset, totalChunkCount, megaChunkInfo, readCommands, functionPointer);
					}
				}
				return SerializeUtilityInterop.AllocAndQueueReadChunkCommands$BurstManaged(readOffset, totalChunkCount, megaChunkInfo, readCommands);
			}

			// Token: 0x0400093A RID: 2362
			private static IntPtr Pointer;

			// Token: 0x0400093B RID: 2363
			private static IntPtr DeferredCompilation;
		}

		// Token: 0x02000390 RID: 912
		// (Invoke) Token: 0x0600183B RID: 6203
		internal unsafe delegate void AddExistingChunk_00001517$PostfixBurstDelegate(Archetype* archetype, ChunkIndex chunk, int* sharedComponentIndices, byte* enabledBitsValuesForChunk, int* perComponentDisabledBitCount);

		// Token: 0x02000391 RID: 913
		internal static class AddExistingChunk_00001517$BurstDirectCall
		{
			// Token: 0x0600183E RID: 6206 RVA: 0x0005AF03 File Offset: 0x00059103
			[BurstDiscard]
			private unsafe static void GetFunctionPointerDiscard(ref IntPtr A_0)
			{
				if (SerializeUtilityInterop.AddExistingChunk_00001517$BurstDirectCall.Pointer == 0)
				{
					SerializeUtilityInterop.AddExistingChunk_00001517$BurstDirectCall.Pointer = BurstCompiler.GetILPPMethodFunctionPointer2(SerializeUtilityInterop.AddExistingChunk_00001517$BurstDirectCall.DeferredCompilation, methodof(SerializeUtilityInterop.AddExistingChunk$BurstManaged(Archetype*, ChunkIndex, int*, byte*, int*)).MethodHandle, typeof(SerializeUtilityInterop.AddExistingChunk_00001517$PostfixBurstDelegate).TypeHandle);
				}
				A_0 = SerializeUtilityInterop.AddExistingChunk_00001517$BurstDirectCall.Pointer;
			}

			// Token: 0x0600183F RID: 6207 RVA: 0x0005AF30 File Offset: 0x00059130
			private static IntPtr GetFunctionPointer()
			{
				IntPtr intPtr = (IntPtr)0;
				SerializeUtilityInterop.AddExistingChunk_00001517$BurstDirectCall.GetFunctionPointerDiscard(ref intPtr);
				return intPtr;
			}

			// Token: 0x06001840 RID: 6208 RVA: 0x0005AF48 File Offset: 0x00059148
			public unsafe static void Constructor()
			{
				SerializeUtilityInterop.AddExistingChunk_00001517$BurstDirectCall.DeferredCompilation = BurstCompiler.CompileILPPMethod2(methodof(SerializeUtilityInterop.AddExistingChunk(Archetype*, ChunkIndex, int*, byte*, int*)).MethodHandle);
			}

			// Token: 0x06001841 RID: 6209 RVA: 0x00002287 File Offset: 0x00000487
			public static void Initialize()
			{
			}

			// Token: 0x06001842 RID: 6210 RVA: 0x0005AF59 File Offset: 0x00059159
			// Note: this type is marked as 'beforefieldinit'.
			static AddExistingChunk_00001517$BurstDirectCall()
			{
				SerializeUtilityInterop.AddExistingChunk_00001517$BurstDirectCall.Constructor();
			}

			// Token: 0x06001843 RID: 6211 RVA: 0x0005AF60 File Offset: 0x00059160
			public unsafe static void Invoke(Archetype* archetype, ChunkIndex chunk, int* sharedComponentIndices, byte* enabledBitsValuesForChunk, int* perComponentDisabledBitCount)
			{
				if (BurstCompiler.IsEnabled)
				{
					IntPtr functionPointer = SerializeUtilityInterop.AddExistingChunk_00001517$BurstDirectCall.GetFunctionPointer();
					if (functionPointer != 0)
					{
						calli(System.Void(Unity.Entities.Archetype*,Unity.Entities.ChunkIndex,System.Int32*,System.Byte*,System.Int32*), archetype, chunk, sharedComponentIndices, enabledBitsValuesForChunk, perComponentDisabledBitCount, functionPointer);
						return;
					}
				}
				SerializeUtilityInterop.AddExistingChunk$BurstManaged(archetype, chunk, sharedComponentIndices, enabledBitsValuesForChunk, perComponentDisabledBitCount);
			}

			// Token: 0x0400093C RID: 2364
			private static IntPtr Pointer;

			// Token: 0x0400093D RID: 2365
			private static IntPtr DeferredCompilation;
		}

		// Token: 0x02000392 RID: 914
		// (Invoke) Token: 0x06001845 RID: 6213
		internal unsafe delegate void ImportChunks_00001518$PostfixBurstDelegate(SerializeUtility.WorldDeserializationStatus* status, ref BurstableMemoryBinaryReader bufferReader, UnsafeList<EntityArchetype>* archetypes, int* sharedComponentArray, int numSharedComponents, int* sharedComponentRemap, UnsafeList<ArchetypeChunk>* blobAssetRefChunks, byte* componentEnabledBits, int* enabledBitsHierarchicalData, EntityComponentStore* ecs, ref NativeArray<int> instanceIds, UnsafeList<ArchetypeChunk>* chunksWithMetaChunkEntities, Entity* externalEntitiesReferences, int externalEntitiesRefCount, ref Entity prefabRoot);

		// Token: 0x02000393 RID: 915
		internal static class ImportChunks_00001518$BurstDirectCall
		{
			// Token: 0x06001848 RID: 6216 RVA: 0x0005AF9B File Offset: 0x0005919B
			[BurstDiscard]
			private unsafe static void GetFunctionPointerDiscard(ref IntPtr A_0)
			{
				if (SerializeUtilityInterop.ImportChunks_00001518$BurstDirectCall.Pointer == 0)
				{
					SerializeUtilityInterop.ImportChunks_00001518$BurstDirectCall.Pointer = BurstCompiler.GetILPPMethodFunctionPointer2(SerializeUtilityInterop.ImportChunks_00001518$BurstDirectCall.DeferredCompilation, methodof(SerializeUtilityInterop.ImportChunks$BurstManaged(SerializeUtility.WorldDeserializationStatus*, ref BurstableMemoryBinaryReader, UnsafeList<EntityArchetype>*, int*, int, int*, UnsafeList<ArchetypeChunk>*, byte*, int*, EntityComponentStore*, ref NativeArray<int>, UnsafeList<ArchetypeChunk>*, Entity*, int, ref Entity)).MethodHandle, typeof(SerializeUtilityInterop.ImportChunks_00001518$PostfixBurstDelegate).TypeHandle);
				}
				A_0 = SerializeUtilityInterop.ImportChunks_00001518$BurstDirectCall.Pointer;
			}

			// Token: 0x06001849 RID: 6217 RVA: 0x0005AFC8 File Offset: 0x000591C8
			private static IntPtr GetFunctionPointer()
			{
				IntPtr intPtr = (IntPtr)0;
				SerializeUtilityInterop.ImportChunks_00001518$BurstDirectCall.GetFunctionPointerDiscard(ref intPtr);
				return intPtr;
			}

			// Token: 0x0600184A RID: 6218 RVA: 0x0005AFE0 File Offset: 0x000591E0
			public unsafe static void Constructor()
			{
				SerializeUtilityInterop.ImportChunks_00001518$BurstDirectCall.DeferredCompilation = BurstCompiler.CompileILPPMethod2(methodof(SerializeUtilityInterop.ImportChunks(SerializeUtility.WorldDeserializationStatus*, ref BurstableMemoryBinaryReader, UnsafeList<EntityArchetype>*, int*, int, int*, UnsafeList<ArchetypeChunk>*, byte*, int*, EntityComponentStore*, ref NativeArray<int>, UnsafeList<ArchetypeChunk>*, Entity*, int, ref Entity)).MethodHandle);
			}

			// Token: 0x0600184B RID: 6219 RVA: 0x00002287 File Offset: 0x00000487
			public static void Initialize()
			{
			}

			// Token: 0x0600184C RID: 6220 RVA: 0x0005AFF1 File Offset: 0x000591F1
			// Note: this type is marked as 'beforefieldinit'.
			static ImportChunks_00001518$BurstDirectCall()
			{
				SerializeUtilityInterop.ImportChunks_00001518$BurstDirectCall.Constructor();
			}

			// Token: 0x0600184D RID: 6221 RVA: 0x0005AFF8 File Offset: 0x000591F8
			public unsafe static void Invoke(SerializeUtility.WorldDeserializationStatus* status, ref BurstableMemoryBinaryReader bufferReader, UnsafeList<EntityArchetype>* archetypes, int* sharedComponentArray, int numSharedComponents, int* sharedComponentRemap, UnsafeList<ArchetypeChunk>* blobAssetRefChunks, byte* componentEnabledBits, int* enabledBitsHierarchicalData, EntityComponentStore* ecs, ref NativeArray<int> instanceIds, UnsafeList<ArchetypeChunk>* chunksWithMetaChunkEntities, Entity* externalEntitiesReferences, int externalEntitiesRefCount, ref Entity prefabRoot)
			{
				if (BurstCompiler.IsEnabled)
				{
					IntPtr functionPointer = SerializeUtilityInterop.ImportChunks_00001518$BurstDirectCall.GetFunctionPointer();
					if (functionPointer != 0)
					{
						calli(System.Void(Unity.Entities.Serialization.SerializeUtility/WorldDeserializationStatus*,Unity.Entities.Serialization.BurstableMemoryBinaryReader&,Unity.Collections.LowLevel.Unsafe.UnsafeList`1<Unity.Entities.EntityArchetype>*,System.Int32*,System.Int32,System.Int32*,Unity.Collections.LowLevel.Unsafe.UnsafeList`1<Unity.Entities.ArchetypeChunk>*,System.Byte*,System.Int32*,Unity.Entities.EntityComponentStore*,Unity.Collections.NativeArray`1<System.Int32>&,Unity.Collections.LowLevel.Unsafe.UnsafeList`1<Unity.Entities.ArchetypeChunk>*,Unity.Entities.Entity*,System.Int32,Unity.Entities.Entity&), status, ref bufferReader, archetypes, sharedComponentArray, numSharedComponents, sharedComponentRemap, blobAssetRefChunks, componentEnabledBits, enabledBitsHierarchicalData, ecs, ref instanceIds, chunksWithMetaChunkEntities, externalEntitiesReferences, externalEntitiesRefCount, ref prefabRoot, functionPointer);
						return;
					}
				}
				SerializeUtilityInterop.ImportChunks$BurstManaged(status, ref bufferReader, archetypes, sharedComponentArray, numSharedComponents, sharedComponentRemap, blobAssetRefChunks, componentEnabledBits, enabledBitsHierarchicalData, ecs, ref instanceIds, chunksWithMetaChunkEntities, externalEntitiesReferences, externalEntitiesRefCount, ref prefabRoot);
			}

			// Token: 0x0400093E RID: 2366
			private static IntPtr Pointer;

			// Token: 0x0400093F RID: 2367
			private static IntPtr DeferredCompilation;
		}
	}
}
