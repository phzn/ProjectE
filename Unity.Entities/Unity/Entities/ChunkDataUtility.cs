using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Unity.Burst;
using Unity.Burst.CompilerServices;
using Unity.Burst.Intrinsics;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Unity.Entities
{
	// Token: 0x02000030 RID: 48
	[BurstCompile]
	internal static class ChunkDataUtility
	{
		// Token: 0x060000D6 RID: 214 RVA: 0x00003EA0 File Offset: 0x000020A0
		public unsafe static int GetIndexInTypeArray(Archetype* archetype, TypeIndex typeIndex)
		{
			ComponentTypeInArchetype* types = archetype->Types;
			int typesCount = archetype->TypesCount;
			for (int num = 0; num != typesCount; num++)
			{
				if (typeIndex == types[num].TypeIndex)
				{
					return num;
				}
			}
			return -1;
		}

		// Token: 0x060000D7 RID: 215 RVA: 0x00003EE4 File Offset: 0x000020E4
		public unsafe static int GetNextIndexInTypeArray(Archetype* archetype, TypeIndex typeIndex, int lastTypeIndexInTypeArray)
		{
			ComponentTypeInArchetype* types = archetype->Types;
			int typesCount = archetype->TypesCount;
			for (int num = lastTypeIndexInTypeArray; num != typesCount; num++)
			{
				if (typeIndex == types[num].TypeIndex)
				{
					return num;
				}
			}
			return -1;
		}

		// Token: 0x060000D8 RID: 216 RVA: 0x00003F28 File Offset: 0x00002128
		public unsafe static TypeIndex GetTypeIndexFromType(Archetype* archetype, Type componentType)
		{
			ComponentTypeInArchetype* types = archetype->Types;
			int typesCount = archetype->TypesCount;
			for (int num = 0; num != typesCount; num++)
			{
				if (componentType.IsAssignableFrom(TypeManager.GetType(types[num].TypeIndex)))
				{
					return types[num].TypeIndex;
				}
			}
			return TypeIndex.Null;
		}

		// Token: 0x060000D9 RID: 217 RVA: 0x00003F84 File Offset: 0x00002184
		public unsafe static void GetIndexInTypeArray(Archetype* archetype, TypeIndex typeIndex, ref short typeLookupCache)
		{
			ComponentTypeInArchetype* types = archetype->Types;
			int typesCount = archetype->TypesCount;
			if (typeLookupCache >= 0 && (int)typeLookupCache < typesCount && types[typeLookupCache].TypeIndex == typeIndex)
			{
				return;
			}
			for (int num = 0; num != typesCount; num++)
			{
				if (!(typeIndex != types[num].TypeIndex))
				{
					typeLookupCache = (short)num;
					return;
				}
			}
			typeLookupCache = -1;
		}

		// Token: 0x060000DA RID: 218 RVA: 0x00003FF0 File Offset: 0x000021F0
		public unsafe static void SetSharedComponentDataIndex(Entity entity, Archetype* archetype, in SharedComponentValues sharedComponentValues, TypeIndex typeIndex)
		{
			EntityComponentStore* entityComponentStore = archetype->EntityComponentStore;
			entityComponentStore->Move(entity, archetype, sharedComponentValues);
			ChunkIndex chunk = entityComponentStore->GetChunk(entity);
			int indexInTypeArray = ChunkDataUtility.GetIndexInTypeArray(archetype, typeIndex);
			uint globalSystemVersion = entityComponentStore->GlobalSystemVersion;
			archetype->Chunks.SetChangeVersion(indexInTypeArray, chunk.ListIndex, globalSystemVersion);
		}

		// Token: 0x060000DB RID: 219 RVA: 0x0000403C File Offset: 0x0000223C
		public unsafe static void SetSharedComponentDataIndex(ChunkIndex chunk, Archetype* archetype, in SharedComponentValues sharedComponentValues, TypeIndex typeIndex)
		{
			EntityComponentStore* entityComponentStore = archetype->EntityComponentStore;
			entityComponentStore->Move(chunk, archetype, sharedComponentValues);
			int indexInTypeArray = ChunkDataUtility.GetIndexInTypeArray(archetype, typeIndex);
			uint globalSystemVersion = entityComponentStore->GlobalSystemVersion;
			archetype->Chunks.SetChangeVersion(indexInTypeArray, chunk.ListIndex, globalSystemVersion);
		}

		// Token: 0x060000DC RID: 220 RVA: 0x0000407F File Offset: 0x0000227F
		public unsafe static void SetSharedComponentDataIndex(EntityBatchInChunk batch, Archetype* archetype, in SharedComponentValues sharedComponentValues, TypeIndex typeIndex)
		{
			archetype->EntityComponentStore->MoveAndSetChangeVersion(batch, archetype, sharedComponentValues, typeIndex);
		}

		// Token: 0x060000DD RID: 221 RVA: 0x00004095 File Offset: 0x00002295
		public unsafe static byte* GetComponentDataWithTypeRO(ChunkIndex chunk, Archetype* archetype, int baseEntityIndex, TypeIndex typeIndex, ref LookupCache lookupCache)
		{
			if (Hint.Unlikely(lookupCache.Archetype != archetype))
			{
				lookupCache.Update(archetype, typeIndex);
			}
			return chunk.Buffer + (lookupCache.ComponentOffset + (int)lookupCache.ComponentSizeOf * baseEntityIndex);
		}

		// Token: 0x060000DE RID: 222 RVA: 0x000040D0 File Offset: 0x000022D0
		public unsafe static byte* GetComponentDataWithTypeRW(ChunkIndex chunk, Archetype* archetype, int baseEntityIndex, TypeIndex typeIndex, uint globalSystemVersion, ref LookupCache lookupCache)
		{
			if (Hint.Unlikely(lookupCache.Archetype != archetype))
			{
				lookupCache.Update(archetype, typeIndex);
			}
			archetype->Chunks.SetChangeVersion((int)lookupCache.IndexInArchetype, chunk.ListIndex, globalSystemVersion);
			return chunk.Buffer + (lookupCache.ComponentOffset + (int)lookupCache.ComponentSizeOf * baseEntityIndex);
		}

		// Token: 0x060000DF RID: 223 RVA: 0x0000412E File Offset: 0x0000232E
		public unsafe static byte* GetComponentDataWithTypeRW(ChunkIndex chunk, int baseEntityIndex, uint globalSystemVersion, ref LookupCache lookupCache)
		{
			lookupCache.Archetype->Chunks.SetChangeVersion((int)lookupCache.IndexInArchetype, chunk.ListIndex, globalSystemVersion);
			return chunk.Buffer + (lookupCache.ComponentOffset + (int)lookupCache.ComponentSizeOf * baseEntityIndex);
		}

		// Token: 0x060000E0 RID: 224 RVA: 0x00004168 File Offset: 0x00002368
		public unsafe static byte* GetOptionalComponentDataWithTypeRO(ChunkIndex chunk, Archetype* archetype, int baseEntityIndex, TypeIndex typeIndex, ref LookupCache lookupCache)
		{
			if (Hint.Unlikely(lookupCache.Archetype != archetype))
			{
				lookupCache.Update(archetype, typeIndex);
			}
			if (Hint.Unlikely(lookupCache.IndexInArchetype == -1))
			{
				return null;
			}
			return chunk.Buffer + (lookupCache.ComponentOffset + (int)lookupCache.ComponentSizeOf * baseEntityIndex);
		}

		// Token: 0x060000E1 RID: 225 RVA: 0x000041C0 File Offset: 0x000023C0
		public unsafe static byte* GetOptionalComponentDataWithTypeRW(ChunkIndex chunk, Archetype* archetype, int baseEntityIndex, TypeIndex typeIndex, uint globalSystemVersion, ref LookupCache lookupCache)
		{
			if (Hint.Unlikely(lookupCache.Archetype != archetype))
			{
				lookupCache.Update(archetype, typeIndex);
			}
			if (Hint.Unlikely(lookupCache.IndexInArchetype == -1))
			{
				return null;
			}
			archetype->Chunks.SetChangeVersion((int)lookupCache.IndexInArchetype, chunk.ListIndex, globalSystemVersion);
			return chunk.Buffer + (lookupCache.ComponentOffset + (int)lookupCache.ComponentSizeOf * baseEntityIndex);
		}

		// Token: 0x060000E2 RID: 226 RVA: 0x00004234 File Offset: 0x00002434
		public unsafe static byte* GetComponentDataWithTypeRO(ChunkIndex chunk, Archetype* archetype, int baseEntityIndex, TypeIndex typeIndex)
		{
			int indexInTypeArray = ChunkDataUtility.GetIndexInTypeArray(archetype, typeIndex);
			int num = archetype->Offsets[indexInTypeArray];
			ushort num2 = archetype->SizeOfs[indexInTypeArray];
			return chunk.Buffer + (num + (int)num2 * baseEntityIndex);
		}

		// Token: 0x060000E3 RID: 227 RVA: 0x00004270 File Offset: 0x00002470
		public unsafe static byte* GetComponentDataWithTypeRW(ChunkIndex chunk, Archetype* archetype, int baseEntityIndex, TypeIndex typeIndex, uint globalSystemVersion)
		{
			int indexInTypeArray = ChunkDataUtility.GetIndexInTypeArray(archetype, typeIndex);
			int num = archetype->Offsets[indexInTypeArray];
			ushort num2 = archetype->SizeOfs[indexInTypeArray];
			archetype->Chunks.SetChangeVersion(indexInTypeArray, chunk.ListIndex, globalSystemVersion);
			return chunk.Buffer + (num + (int)num2 * baseEntityIndex);
		}

		// Token: 0x060000E4 RID: 228 RVA: 0x000042C4 File Offset: 0x000024C4
		public unsafe static byte* GetComponentDataRO(ChunkIndex chunk, Archetype* archetype, int baseEntityIndex, int indexInTypeArray)
		{
			int num = archetype->Offsets[indexInTypeArray];
			ushort num2 = archetype->SizeOfs[indexInTypeArray];
			return chunk.Buffer + (num + (int)num2 * baseEntityIndex);
		}

		// Token: 0x060000E5 RID: 229 RVA: 0x000042F8 File Offset: 0x000024F8
		public unsafe static byte* GetComponentDataRW(ChunkIndex chunk, Archetype* archetype, int baseEntityIndex, int indexInTypeArray, uint globalSystemVersion)
		{
			int num = archetype->Offsets[indexInTypeArray];
			ushort num2 = archetype->SizeOfs[indexInTypeArray];
			archetype->Chunks.SetChangeVersion(indexInTypeArray, chunk.ListIndex, globalSystemVersion);
			return chunk.Buffer + (num + (int)num2 * baseEntityIndex);
		}

		// Token: 0x060000E6 RID: 230 RVA: 0x00004344 File Offset: 0x00002544
		public unsafe static UnsafeBitArray GetEnabledRefRO(ChunkIndex chunk, Archetype* archetype, int indexInTypeArray)
		{
			ArchetypeChunkData chunks = archetype->Chunks;
			int num = archetype->TypeIndexInArchetypeToMemoryOrderIndex[indexInTypeArray];
			return chunks.GetEnabledArrayForTypeInChunk(num, chunk.ListIndex);
		}

		// Token: 0x060000E7 RID: 231 RVA: 0x00004374 File Offset: 0x00002574
		public unsafe static UnsafeBitArray GetEnabledRefRW(ChunkIndex chunk, Archetype* archetype, int indexInTypeArray, uint globalSystemVersion, out int* ptrChunkDisabledCount)
		{
			int listIndex = chunk.ListIndex;
			ArchetypeChunkData chunks = archetype->Chunks;
			chunks.SetChangeVersion(indexInTypeArray, listIndex, globalSystemVersion);
			int num = archetype->TypeIndexInArchetypeToMemoryOrderIndex[indexInTypeArray];
			ptrChunkDisabledCount = chunks.GetPointerToChunkDisabledCountForType(num, listIndex);
			return chunks.GetEnabledArrayForTypeInChunk(num, listIndex);
		}

		// Token: 0x060000E8 RID: 232 RVA: 0x000043BC File Offset: 0x000025BC
		public unsafe static void Copy(EntityComponentStore* entityComponentStore, ChunkIndex srcChunk, int srcIndex, ChunkIndex dstChunk, int dstIndex, int count)
		{
			Archetype* archetype = entityComponentStore->GetArchetype(srcChunk);
			byte* buffer = srcChunk.Buffer;
			byte* buffer2 = dstChunk.Buffer;
			int* offsets = archetype->Offsets;
			ushort* sizeOfs = archetype->SizeOfs;
			int typesCount = archetype->TypesCount;
			for (int i = 0; i < typesCount; i++)
			{
				int num = offsets[i];
				ushort num2 = sizeOfs[i];
				byte* ptr = buffer + (num + (int)num2 * srcIndex);
				UnsafeUtility.MemCpy((void*)(buffer2 + (num + (int)num2 * dstIndex)), (void*)ptr, (long)((int)num2 * count));
			}
		}

		// Token: 0x060000E9 RID: 233 RVA: 0x0000443C File Offset: 0x0000263C
		public unsafe static void UpdateChunkDisabledEntityCounts(ChunkIndex chunk, Archetype* arch)
		{
			int listIndex = chunk.ListIndex;
			int count = chunk.Count;
			ArchetypeChunkData chunks = arch->Chunks;
			v128* componentEnabledMaskArrayForChunk = chunks.GetComponentEnabledMaskArrayForChunk(listIndex);
			int* chunkDisabledCounts = chunks.GetChunkDisabledCounts(listIndex);
			int i = 0;
			int typesCount = arch->TypesCount;
			while (i < typesCount)
			{
				int num = EnabledBitUtility.countbits(componentEnabledMaskArrayForChunk[i]);
				chunkDisabledCounts[i] = count - num;
				i++;
			}
		}

		// Token: 0x060000EA RID: 234 RVA: 0x000044B4 File Offset: 0x000026B4
		public unsafe static void CloneEnabledBits(ChunkIndex srcChunk, Archetype* srcArch, int srcEntityIndexInChunk, ChunkIndex dstChunk, Archetype* dstArch, int dstEntityIndexInChunk, int entityCount)
		{
			int listIndex = srcChunk.ListIndex;
			int listIndex2 = dstChunk.ListIndex;
			int enableableTypesCount = srcArch->EnableableTypesCount;
			int num = 0;
			for (int i = 0; i < enableableTypesCount; i++)
			{
				int num2 = srcArch->EnableableTypeIndexInArchetype[i];
				ComponentTypeInArchetype componentTypeInArchetype = srcArch->Types[num2];
				int nextIndexInTypeArray = ChunkDataUtility.GetNextIndexInTypeArray(dstArch, componentTypeInArchetype.TypeIndex, num);
				if (nextIndexInTypeArray >= 0)
				{
					num = nextIndexInTypeArray;
					UnsafeBitArray enabledArrayForTypeInChunk = srcArch->Chunks.GetEnabledArrayForTypeInChunk(srcArch->TypeIndexInArchetypeToMemoryOrderIndex[num2], listIndex);
					dstArch->Chunks.GetEnabledArrayForTypeInChunk(dstArch->TypeIndexInArchetypeToMemoryOrderIndex[num], listIndex2).Copy(dstEntityIndexInChunk, ref enabledArrayForTypeInChunk, srcEntityIndexInChunk, entityCount);
				}
			}
		}

		// Token: 0x060000EB RID: 235 RVA: 0x00004578 File Offset: 0x00002778
		public unsafe static void ReplicateEnabledBits(ChunkIndex srcChunk, Archetype* srcArch, int srcIndex, ChunkIndex dstChunk, Archetype* dstArch, int dstIndex, int count)
		{
			int enableableTypesCount = srcArch->EnableableTypesCount;
			int num = 0;
			int listIndex = srcChunk.ListIndex;
			int listIndex2 = dstChunk.ListIndex;
			for (int i = 0; i < enableableTypesCount; i++)
			{
				int num2 = srcArch->EnableableTypeIndexInArchetype[i];
				int num3 = srcArch->TypeIndexInArchetypeToMemoryOrderIndex[num2];
				bool flag = srcArch->Chunks.GetEnabledArrayForTypeInChunk(num3, listIndex).IsSet(srcIndex);
				int nextIndexInTypeArray = ChunkDataUtility.GetNextIndexInTypeArray(dstArch, srcArch->Types[num2].TypeIndex, num);
				if (nextIndexInTypeArray >= 0)
				{
					num = nextIndexInTypeArray;
					int num4 = dstArch->TypeIndexInArchetypeToMemoryOrderIndex[num];
					dstArch->Chunks.GetEnabledArrayForTypeInChunk(num4, listIndex2).SetBits(dstIndex, flag, count);
					if (!flag)
					{
						dstArch->Chunks.AdjustChunkDisabledCountForType(num4, listIndex2, count);
					}
				}
			}
		}

		// Token: 0x060000EC RID: 236 RVA: 0x00004658 File Offset: 0x00002858
		public unsafe static void MoveEnabledBits(Archetype* srcArchetype, int srcChunkIndex, Archetype* dstArchetype, int dstChunkIndex, int srcEntityCount)
		{
			int enableableTypesCount = dstArchetype->EnableableTypesCount;
			int num = 0;
			for (int i = 0; i < enableableTypesCount; i++)
			{
				int num2 = dstArchetype->EnableableTypeIndexInArchetype[i];
				int num3 = dstArchetype->TypeIndexInArchetypeToMemoryOrderIndex[num2];
				UnsafeBitArray enabledArrayForTypeInChunk = dstArchetype->Chunks.GetEnabledArrayForTypeInChunk(num3, dstChunkIndex);
				enabledArrayForTypeInChunk.Clear();
				dstArchetype->Chunks.SetChunkDisabledCountForType(num3, dstChunkIndex, 0);
				int nextIndexInTypeArray = ChunkDataUtility.GetNextIndexInTypeArray(srcArchetype, dstArchetype->Types[num2].TypeIndex, num);
				if (nextIndexInTypeArray < 0)
				{
					enabledArrayForTypeInChunk.SetBits(0, true, srcEntityCount);
				}
				else
				{
					num = nextIndexInTypeArray;
					int num4 = srcArchetype->TypeIndexInArchetypeToMemoryOrderIndex[num];
					UnsafeBitArray enabledArrayForTypeInChunk2 = srcArchetype->Chunks.GetEnabledArrayForTypeInChunk(num4, srcChunkIndex);
					enabledArrayForTypeInChunk.Copy(0, ref enabledArrayForTypeInChunk2, 0, srcEntityCount);
					int chunkDisabledCountForType = srcArchetype->Chunks.GetChunkDisabledCountForType(num4, srcChunkIndex);
					dstArchetype->Chunks.SetChunkDisabledCountForType(num3, dstChunkIndex, chunkDisabledCountForType);
				}
			}
			ChunkDataUtility.InitializeBitsForNewChunk(srcArchetype, srcChunkIndex);
		}

		// Token: 0x060000ED RID: 237 RVA: 0x00004748 File Offset: 0x00002948
		public unsafe static void ClearPaddingBits(ChunkIndex chunk, Archetype* archetype, int startIndex, int count)
		{
			int enableableTypesCount = archetype->EnableableTypesCount;
			int listIndex = chunk.ListIndex;
			for (int i = 0; i < enableableTypesCount; i++)
			{
				int num = archetype->EnableableTypeIndexInArchetype[i];
				int num2 = archetype->TypeIndexInArchetypeToMemoryOrderIndex[num];
				archetype->Chunks.GetEnabledArrayForTypeInChunk(num2, listIndex).SetBits(startIndex, false, count);
			}
		}

		// Token: 0x060000EE RID: 238 RVA: 0x000047A5 File Offset: 0x000029A5
		public unsafe static void InitializeBitsForNewChunk(Archetype* archetype, int chunkIndex)
		{
			UnsafeUtility.MemClear((void*)archetype->Chunks.GetComponentEnabledMaskArrayForChunk(chunkIndex), (long)archetype->Chunks.ComponentEnabledBitsSizeTotalPerChunk);
			archetype->Chunks.InitializeDisabledCountForChunk(chunkIndex);
		}

		// Token: 0x060000EF RID: 239 RVA: 0x000047D0 File Offset: 0x000029D0
		public unsafe static void RemoveFromEnabledBitsHierarchicalData(ChunkIndex chunk, Archetype* archetype, int startIndex, int count)
		{
			int enableableTypesCount = archetype->EnableableTypesCount;
			int listIndex = chunk.ListIndex;
			for (int i = 0; i < enableableTypesCount; i++)
			{
				int num = archetype->EnableableTypeIndexInArchetype[i];
				int num2 = archetype->TypeIndexInArchetypeToMemoryOrderIndex[num];
				int num3 = count - archetype->Chunks.GetEnabledArrayForTypeInChunk(num2, listIndex).CountBits(startIndex, count);
				archetype->Chunks.AdjustChunkDisabledCountForType(num2, listIndex, -num3);
			}
		}

		// Token: 0x060000F0 RID: 240 RVA: 0x00004844 File Offset: 0x00002A44
		public unsafe static void CopyComponents(ChunkIndex srcChunk, Archetype* srcArch, int srcIndex, ChunkIndex dstChunk, Archetype* dstArch, int dstIndex, int count, uint dstGlobalSystemVersion)
		{
			int typesCount = srcArch->TypesCount;
			byte* buffer = srcChunk.Buffer;
			byte* buffer2 = dstChunk.Buffer;
			int* offsets = srcArch->Offsets;
			ushort* sizeOfs = srcArch->SizeOfs;
			int listIndex = dstChunk.ListIndex;
			for (int i = 1; i < typesCount; i++)
			{
				int num = offsets[i];
				ushort num2 = sizeOfs[i];
				byte* ptr = buffer + (num + (int)num2 * srcIndex);
				void* ptr2 = (void*)(buffer2 + (num + (int)num2 * dstIndex));
				dstArch->Chunks.SetChangeVersion(i, listIndex, dstGlobalSystemVersion);
				UnsafeUtility.MemCpy(ptr2, (void*)ptr, (long)((int)num2 * count));
			}
		}

		// Token: 0x060000F1 RID: 241 RVA: 0x000048DC File Offset: 0x00002ADC
		[BurstCompile]
		public unsafe static void SwapComponents(Archetype* srcArch, ChunkIndex srcChunk, int srcIndex, Archetype* dstArch, ChunkIndex dstChunk, int dstIndex, int count, uint srcGlobalSystemVersion, uint dstGlobalSystemVersion)
		{
			ChunkDataUtility.SwapComponents_000000DB$BurstDirectCall.Invoke(srcArch, srcChunk, srcIndex, dstArch, dstChunk, dstIndex, count, srcGlobalSystemVersion, dstGlobalSystemVersion);
		}

		// Token: 0x060000F2 RID: 242 RVA: 0x000048FC File Offset: 0x00002AFC
		public unsafe static void InitializeComponents(Archetype* arch, ChunkIndex dstChunk, int dstIndex, int count)
		{
			int* offsets = arch->Offsets;
			ushort* sizeOfs = arch->SizeOfs;
			int* bufferCapacities = arch->BufferCapacities;
			byte* buffer = dstChunk.Buffer;
			int typesCount = arch->TypesCount;
			ComponentTypeInArchetype* types = arch->Types;
			for (int num = 1; num != typesCount; num++)
			{
				int num2 = offsets[num];
				ushort num3 = sizeOfs[num];
				byte* ptr = buffer + (num2 + (int)num3 * dstIndex);
				if (types[num].IsBuffer)
				{
					for (int i = 0; i < count; i++)
					{
						BufferHeader.Initialize((BufferHeader*)ptr, bufferCapacities[num]);
						ptr += num3;
					}
				}
				else
				{
					UnsafeUtility.MemClear((void*)ptr, (long)((int)num3 * count));
				}
			}
			int enableableTypesCount = arch->EnableableTypesCount;
			for (int num4 = 0; num4 != enableableTypesCount; num4++)
			{
				int num5 = arch->EnableableTypeIndexInArchetype[num4];
				int num6 = arch->TypeIndexInArchetypeToMemoryOrderIndex[num5];
				arch->Chunks.GetEnabledArrayForTypeInChunk(num6, dstChunk.ListIndex).SetBits(dstIndex, true, count);
			}
		}

		// Token: 0x060000F3 RID: 243 RVA: 0x00004A08 File Offset: 0x00002C08
		public unsafe static void InitializeBuffersInChunk(byte* p, int count, int stride, int bufferCapacity)
		{
			for (int i = 0; i < count; i++)
			{
				BufferHeader.Initialize((BufferHeader*)p, bufferCapacity);
				p += stride;
			}
		}

		// Token: 0x060000F4 RID: 244 RVA: 0x00004A30 File Offset: 0x00002C30
		public unsafe static void Convert(Archetype* srcArch, ChunkIndex srcChunk, int srcIndex, Archetype* dstArch, ChunkIndex dstChunk, int dstIndex, int count)
		{
			EntityComponentStore* entityComponentStore = dstArch->EntityComponentStore;
			int num = srcArch->NonZeroSizedTypesCount - 1;
			int i = dstArch->NonZeroSizedTypesCount - 1;
			int* ptr;
			int num2;
			byte* buffer;
			byte* buffer2;
			checked
			{
				ptr = stackalloc int[unchecked((UIntPtr)(num + 1)) * 4];
				num2 = 0;
				buffer = srcChunk.Buffer;
				buffer2 = dstChunk.Buffer;
			}
			while (i >= 0)
			{
				ComponentTypeInArchetype componentTypeInArchetype = srcArch->Types[num];
				ComponentTypeInArchetype componentTypeInArchetype2 = dstArch->Types[i];
				if (componentTypeInArchetype > componentTypeInArchetype2)
				{
					ptr[(IntPtr)(num2++) * 4] = num;
					num--;
				}
				else
				{
					ushort num3 = srcArch->SizeOfs[num];
					ushort num4 = dstArch->SizeOfs[i];
					byte* ptr2 = buffer + srcArch->Offsets[num] + srcIndex * (int)num3;
					byte* ptr3 = buffer2 + dstArch->Offsets[i] + dstIndex * (int)num4;
					if (componentTypeInArchetype == componentTypeInArchetype2)
					{
						UnsafeUtility.MemCpy((void*)ptr3, (void*)ptr2, (long)(count * (int)num3));
						num--;
						i--;
					}
					else
					{
						if (componentTypeInArchetype2.IsBuffer)
						{
							ChunkDataUtility.InitializeBuffersInChunk(ptr3, count, (int)num4, dstArch->BufferCapacities[i]);
						}
						else
						{
							UnsafeUtility.MemClear((void*)ptr3, (long)(count * (int)num4));
						}
						if (componentTypeInArchetype2.IsEnableable)
						{
							int num5 = dstArch->TypeIndexInArchetypeToMemoryOrderIndex[i];
							dstArch->Chunks.GetEnabledArrayForTypeInChunk(num5, dstChunk.ListIndex).SetBits(dstIndex, true, count);
						}
						i--;
					}
				}
			}
			int num6 = srcArch->NumTagComponents - 1;
			int j = dstArch->NumTagComponents - 1;
			while (j >= 0)
			{
				num = (int)srcArch->FirstTagComponent + num6;
				i = (int)dstArch->FirstTagComponent + j;
				ComponentTypeInArchetype componentTypeInArchetype3 = srcArch->Types[num];
				ComponentTypeInArchetype componentTypeInArchetype4 = dstArch->Types[i];
				if (componentTypeInArchetype3 > componentTypeInArchetype4)
				{
					num6--;
				}
				else if (componentTypeInArchetype3 == componentTypeInArchetype4)
				{
					num6--;
					j--;
				}
				else
				{
					if (componentTypeInArchetype4.IsEnableable)
					{
						int num7 = dstArch->TypeIndexInArchetypeToMemoryOrderIndex[i];
						dstArch->Chunks.GetEnabledArrayForTypeInChunk(num7, dstChunk.ListIndex).SetBits(dstIndex, true, count);
					}
					j--;
				}
			}
			if (num2 == 0)
			{
				return;
			}
			ptr[num2] = 0;
			int num8 = 0;
			if (ptr[num8] >= (int)srcArch->FirstManagedComponent)
			{
				int num9 = entityComponentStore->ManagedChangesTracker.BeginFreeManagedComponentCommand();
				do
				{
					num = ptr[num8];
					ushort num10 = srcArch->SizeOfs[num];
					int* ptr4 = (int*)(buffer + srcArch->Offsets[num] + srcIndex * (int)num10);
					for (int k = 0; k < count; k++)
					{
						int num11 = ptr4[k];
						if (num11 != 0)
						{
							entityComponentStore->FreeManagedComponentIndex(num11);
							entityComponentStore->ManagedChangesTracker.AddToFreeManagedComponentCommand(num11);
						}
					}
				}
				while (ptr[(IntPtr)(++num8) * 4] >= (int)srcArch->FirstManagedComponent);
				entityComponentStore->ManagedChangesTracker.EndDeallocateManagedComponentCommand(num9);
			}
			while (ptr[num8] >= (int)srcArch->FirstBufferComponent)
			{
				num = ptr[num8];
				ushort num12 = srcArch->SizeOfs[num];
				byte* ptr5 = buffer + srcArch->Offsets[num] + srcIndex * (int)num12;
				for (int l = 0; l < count; l++)
				{
					BufferHeader.Destroy((BufferHeader*)ptr5);
					ptr5 += num12;
				}
				num8++;
			}
		}

		// Token: 0x060000F5 RID: 245 RVA: 0x00004D94 File Offset: 0x00002F94
		public unsafe static void MemsetUnusedChunkData(Archetype* arch, byte* chunkBuffer, byte value, int entityCount)
		{
			for (int i = 0; i < arch->TypesCount; i++)
			{
				ComponentTypeInArchetype componentTypeInArchetype = arch->Types[i];
				ushort num = arch->SizeOfs[i];
				if (componentTypeInArchetype.IsBuffer)
				{
					int elementSize = TypeManager.GetTypeInfo(componentTypeInArchetype.TypeIndex).ElementSize;
					int num2 = arch->BufferCapacities[i];
					for (int j = 0; j < entityCount; j++)
					{
						BufferHeader* ptr = (BufferHeader*)(chunkBuffer + arch->Offsets[i] + j * (int)num);
						BufferHeader.MemsetUnusedMemory(ptr, num2, elementSize, value);
					}
				}
			}
			for (int k = 0; k < arch->TypesCount - 1; k++)
			{
				int num3 = arch->TypeMemoryOrderIndexToIndexInArchetype[k];
				int num4 = arch->TypeMemoryOrderIndexToIndexInArchetype[k + 1];
				ushort num5 = arch->SizeOfs[num3];
				int num6 = arch->Offsets[num3] + entityCount * (int)num5;
				int num7 = arch->Offsets[num4];
				UnsafeUtility.MemSet((void*)(chunkBuffer + num6), value, (long)(num7 - num6));
			}
			int num8 = arch->TypeMemoryOrderIndexToIndexInArchetype[arch->TypesCount - 1];
			int num9 = arch->Offsets[num8] + entityCount * (int)arch->SizeOfs[num8];
			int num10 = 16320;
			UnsafeUtility.MemSet((void*)(chunkBuffer + num9), value, (long)(num10 - num9));
			UnsafeUtility.MemSet((void*)(chunkBuffer - 64 + 24), value, 40L);
		}

		// Token: 0x060000F6 RID: 246 RVA: 0x00004F08 File Offset: 0x00003108
		public unsafe static bool AreLayoutCompatible(Archetype* a, Archetype* b)
		{
			if (a == null || b == null || a->ChunkCapacity != b->ChunkCapacity)
			{
				return false;
			}
			int nonZeroSizedTypesCount = a->NonZeroSizedTypesCount;
			if (nonZeroSizedTypesCount != b->NonZeroSizedTypesCount)
			{
				return false;
			}
			for (int i = 0; i < nonZeroSizedTypesCount; i++)
			{
				if (a->Types[i] != b->Types[i])
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x060000F7 RID: 247 RVA: 0x00004F84 File Offset: 0x00003184
		[Conditional("ENABLE_UNITY_COLLECTIONS_CHECKS")]
		[Conditional("UNITY_DOTS_DEBUG")]
		public unsafe static void AssertAreLayoutCompatible(Archetype* a, Archetype* b)
		{
			int nonZeroSizedTypesCount = a->NonZeroSizedTypesCount;
			for (int i = 0; i < nonZeroSizedTypesCount; i++)
			{
			}
		}

		// Token: 0x060000F8 RID: 248 RVA: 0x00004FA4 File Offset: 0x000031A4
		public unsafe static void DeallocateBuffers(Archetype* archetype, ChunkIndex chunk)
		{
			int bufferComponentsEnd = archetype->BufferComponentsEnd;
			byte* buffer = chunk.Buffer;
			int count = chunk.Count;
			short num = archetype->FirstBufferComponent;
			while ((int)num < bufferComponentsEnd)
			{
				byte* ptr = buffer + archetype->Offsets[num];
				ushort num2 = archetype->SizeOfs[num];
				for (int i = 0; i < count; i++)
				{
					BufferHeader.Destroy((BufferHeader*)(ptr + (int)num2 * i));
				}
				num += 1;
			}
		}

		// Token: 0x060000F9 RID: 249 RVA: 0x00005018 File Offset: 0x00003218
		private unsafe static void ReleaseChunk(Archetype* archetype, ChunkIndex chunk)
		{
			EntityComponentStore* entityComponentStore = archetype->EntityComponentStore;
			if (archetype->NumSharedComponents > 0)
			{
				SharedComponentValues sharedComponentValues = archetype->Chunks.GetSharedComponentValues(chunk.ListIndex);
				for (int i = 0; i < archetype->NumSharedComponents; i++)
				{
					int num = *sharedComponentValues[i];
					if (EntityComponentStore.IsUnmanagedSharedComponentIndex(num))
					{
						entityComponentStore->RemoveSharedComponentReference_Unmanaged(num, 1);
					}
					else
					{
						entityComponentStore->ManagedChangesTracker.RemoveReference(num, 1);
					}
				}
			}
			if (chunk.Count < archetype->ChunkCapacity)
			{
				archetype->EmptySlotTrackingRemoveChunk(chunk);
			}
			archetype->RemoveFromChunkList(chunk, ref entityComponentStore->m_ChunkListChangesTracker);
			entityComponentStore->SetArchetype(chunk, null);
			entityComponentStore->FreeChunk(chunk);
		}

		// Token: 0x060000FA RID: 250 RVA: 0x000050B8 File Offset: 0x000032B8
		public unsafe static void SetChunkCountKeepMetaChunk(Archetype* archetype, ChunkIndex chunk, int newCount)
		{
			if (newCount == 0)
			{
				ChunkDataUtility.ReleaseChunk(archetype, chunk);
				return;
			}
			int chunkCapacity = archetype->ChunkCapacity;
			if (newCount == chunkCapacity)
			{
				archetype->EmptySlotTrackingRemoveChunk(chunk);
			}
			else if (chunk.Count == chunkCapacity)
			{
				archetype->EmptySlotTrackingAddChunk(chunk);
			}
			chunk.Count = newCount;
			archetype->Chunks.SetChunkEntityCount(chunk.ListIndex, newCount);
		}

		// Token: 0x060000FB RID: 251 RVA: 0x00005110 File Offset: 0x00003310
		public unsafe static void SetChunkCount(Archetype* archetype, ChunkIndex chunk, int newCount)
		{
			EntityComponentStore* entityComponentStore = archetype->EntityComponentStore;
			if (newCount == 0 && archetype->MetaChunkArchetype != null)
			{
				entityComponentStore->DestroyMetaChunkEntity(chunk.MetaChunkEntity);
			}
			ChunkDataUtility.SetChunkCountKeepMetaChunk(archetype, chunk, newCount);
		}

		// Token: 0x060000FC RID: 252 RVA: 0x00005148 File Offset: 0x00003348
		private unsafe static int AllocateIntoChunk(Archetype* archetype, ChunkIndex chunk, int count, out int outIndex)
		{
			outIndex = chunk.Count;
			int num = Math.Min(archetype->ChunkCapacity - outIndex, count);
			ChunkDataUtility.SetChunkCount(archetype, chunk, outIndex + num);
			archetype->EntityCount = archetype->EntityCount + num;
			return num;
		}

		// Token: 0x060000FD RID: 253 RVA: 0x00005185 File Offset: 0x00003385
		public unsafe static void Allocate(Archetype* archetype, ChunkIndex chunk, int count)
		{
			ChunkDataUtility.Allocate(archetype, chunk, null, count);
		}

		// Token: 0x060000FE RID: 254 RVA: 0x00005194 File Offset: 0x00003394
		public unsafe static void Allocate(Archetype* archetype, ChunkIndex chunk, Entity* entities, int count)
		{
			EntityComponentStore* entityComponentStore = archetype->EntityComponentStore;
			uint globalSystemVersion = entityComponentStore->GlobalSystemVersion;
			int num2;
			int num = ChunkDataUtility.AllocateIntoChunk(archetype, chunk, count, out num2);
			entityComponentStore->AllocateEntities(archetype, chunk, num2, num, entities);
			ChunkDataUtility.InitializeComponents(archetype, chunk, num2, num);
			archetype->Chunks.SetAllChangeVersion(chunk.ListIndex, globalSystemVersion);
			archetype->Chunks.SetOrderVersion(chunk.ListIndex, globalSystemVersion);
			entityComponentStore->IncrementComponentTypeOrderVersion(archetype);
		}

		// Token: 0x060000FF RID: 255 RVA: 0x000051FC File Offset: 0x000033FC
		public unsafe static void Remove(EntityComponentStore* entityComponentStore, in EntityBatchInChunk batchInChunk)
		{
			ChunkIndex chunk = batchInChunk.Chunk;
			int count = batchInChunk.Count;
			int startIndex = batchInChunk.StartIndex;
			Archetype* archetype = entityComponentStore->GetArchetype(chunk);
			int num = startIndex + count;
			int num2 = chunk.Count - num;
			int num3 = math.min(count, num2);
			if (num3 > 0)
			{
				int num4 = chunk.Count - num3;
				ChunkDataUtility.Copy(entityComponentStore, chunk, num4, chunk, startIndex, num3);
				ChunkDataUtility.RemoveFromEnabledBitsHierarchicalData(chunk, archetype, startIndex, count);
				ChunkDataUtility.CloneEnabledBits(chunk, archetype, num4, chunk, archetype, startIndex, num3);
				int num5 = chunk.Count - count;
				ChunkDataUtility.ClearPaddingBits(chunk, archetype, num5, count);
				Entity* componentDataRO = (Entity*)ChunkDataUtility.GetComponentDataRO(chunk, archetype, startIndex, 0);
				for (int i = 0; i < num3; i++)
				{
					Entity entity = componentDataRO[i];
					entityComponentStore->SetEntityInChunk(entity, new EntityInChunk
					{
						Chunk = chunk,
						IndexInChunk = startIndex + i
					});
				}
			}
			else
			{
				ChunkDataUtility.RemoveFromEnabledBitsHierarchicalData(chunk, archetype, startIndex, count);
				ChunkDataUtility.ClearPaddingBits(chunk, archetype, startIndex, count);
			}
			archetype->Chunks.SetOrderVersion(chunk.ListIndex, entityComponentStore->GlobalSystemVersion);
			entityComponentStore->IncrementComponentTypeOrderVersion(archetype);
			SharedComponentValues sharedComponentValues = archetype->Chunks.GetSharedComponentValues(chunk.ListIndex);
			entityComponentStore->ManagedChangesTracker.IncrementComponentOrderVersion(archetype, sharedComponentValues);
			int num6 = chunk.Count - count;
			ChunkDataUtility.SetChunkCount(archetype, chunk, num6);
			if (num3 > 0 && num6 != 0)
			{
				ChunkDataUtility.UpdateChunkDisabledEntityCounts(chunk, archetype);
			}
			archetype->EntityCount -= count;
		}

		// Token: 0x06000100 RID: 256 RVA: 0x00005368 File Offset: 0x00003568
		public unsafe static void ChangeArchetypeInPlace(Archetype* srcArchetype, ChunkIndex srcChunk, Archetype* dstArchetype, int* dstSharedComponentValues)
		{
			EntityComponentStore* entityComponentStore = dstArchetype->EntityComponentStore;
			SharedComponentValues sharedComponentValues = srcArchetype->Chunks.GetSharedComponentValues(srcChunk.ListIndex);
			if (srcArchetype->NumSharedComponents > 0 || dstArchetype->NumSharedComponents > 0)
			{
				int firstSharedComponent = (int)srcArchetype->FirstSharedComponent;
				int firstSharedComponent2 = (int)dstArchetype->FirstSharedComponent;
				int numSharedComponents = srcArchetype->NumSharedComponents;
				int numSharedComponents2 = dstArchetype->NumSharedComponents;
				int i = 0;
				int j = 0;
				while (j < numSharedComponents2)
				{
					if (i >= numSharedComponents)
					{
						break;
					}
					TypeIndex typeIndex = srcArchetype->Types[i + firstSharedComponent].TypeIndex;
					TypeIndex typeIndex2 = dstArchetype->Types[j + firstSharedComponent2].TypeIndex;
					if (typeIndex == typeIndex2)
					{
						int num = *sharedComponentValues[i];
						int num2 = dstSharedComponentValues[j];
						if (num != num2)
						{
							if (EntityComponentStore.IsUnmanagedSharedComponentIndex(num))
							{
								entityComponentStore->RemoveSharedComponentReference_Unmanaged(num, 1);
							}
							else
							{
								entityComponentStore->ManagedChangesTracker.RemoveReference(num, 1);
							}
							if (EntityComponentStore.IsUnmanagedSharedComponentIndex(num2))
							{
								entityComponentStore->AddSharedComponentReference_Unmanaged(num2, 1);
							}
							else
							{
								entityComponentStore->ManagedChangesTracker.AddReference(num2, 1);
							}
						}
						j++;
						i++;
					}
					else if (typeIndex2 > typeIndex)
					{
						int num3 = *sharedComponentValues[i];
						if (EntityComponentStore.IsUnmanagedSharedComponentIndex(num3))
						{
							entityComponentStore->RemoveSharedComponentReference_Unmanaged(num3, 1);
						}
						else
						{
							entityComponentStore->ManagedChangesTracker.RemoveReference(num3, 1);
						}
						i++;
					}
					else
					{
						int num4 = dstSharedComponentValues[j];
						if (EntityComponentStore.IsUnmanagedSharedComponentIndex(num4))
						{
							entityComponentStore->AddSharedComponentReference_Unmanaged(num4, 1);
						}
						else
						{
							entityComponentStore->ManagedChangesTracker.AddReference(num4, 1);
						}
						j++;
					}
				}
				while (j < numSharedComponents2)
				{
					int num5 = dstSharedComponentValues[j];
					if (EntityComponentStore.IsUnmanagedSharedComponentIndex(num5))
					{
						entityComponentStore->AddSharedComponentReference_Unmanaged(num5, 1);
					}
					else
					{
						entityComponentStore->ManagedChangesTracker.AddReference(num5, 1);
					}
					j++;
				}
				while (i < numSharedComponents)
				{
					int num6 = *sharedComponentValues[i];
					if (EntityComponentStore.IsUnmanagedSharedComponentIndex(num6))
					{
						entityComponentStore->RemoveSharedComponentReference_Unmanaged(num6, 1);
					}
					else
					{
						entityComponentStore->ManagedChangesTracker.RemoveReference(num6, 1);
					}
					i++;
				}
			}
			int count = srcChunk.Count;
			bool flag = count < srcArchetype->ChunkCapacity;
			if (flag)
			{
				srcArchetype->EmptySlotTrackingRemoveChunk(srcChunk);
			}
			int listIndex = srcChunk.ListIndex;
			if (Hint.Likely(dstArchetype != srcArchetype))
			{
				dstArchetype->AddToChunkList(srcChunk, dstSharedComponentValues, 0U, ref entityComponentStore->m_ChunkListChangesTracker);
				int listIndex2 = srcChunk.ListIndex;
				ChunkDataUtility.CloneChangeVersions(srcArchetype, listIndex, dstArchetype, listIndex2, false);
				ChunkDataUtility.InitializeBitsForNewChunk(dstArchetype, listIndex2);
				ChunkDataUtility.MoveEnabledBits(srcArchetype, listIndex, dstArchetype, listIndex2, count);
				srcChunk.ListIndex = listIndex;
				srcArchetype->RemoveFromChunkList(srcChunk, ref entityComponentStore->m_ChunkListChangesTracker);
				srcChunk.ListIndex = listIndex2;
				srcArchetype->EntityCount = srcArchetype->EntityCount - count;
				dstArchetype->EntityCount = dstArchetype->EntityCount + count;
				entityComponentStore->SetArchetype(srcChunk, dstArchetype);
				dstArchetype->Chunks.SetOrderVersion(srcChunk.ListIndex, entityComponentStore->GlobalSystemVersion);
				entityComponentStore->IncrementComponentTypeOrderVersion(srcArchetype);
				entityComponentStore->IncrementComponentTypeOrderVersion(dstArchetype);
			}
			else
			{
				int k = 0;
				int numSharedComponents3 = srcArchetype->NumSharedComponents;
				while (k < numSharedComponents3)
				{
					srcArchetype->Chunks.SetSharedComponentValue(k, listIndex, dstSharedComponentValues[k]);
					k++;
				}
			}
			if (flag)
			{
				dstArchetype->EmptySlotTrackingAddChunk(srcChunk);
			}
			if (srcArchetype->MetaChunkArchetype != dstArchetype->MetaChunkArchetype)
			{
				if (srcArchetype->MetaChunkArchetype == null)
				{
					entityComponentStore->CreateMetaEntityForChunk(srcChunk);
					return;
				}
				if (dstArchetype->MetaChunkArchetype == null)
				{
					entityComponentStore->DestroyMetaChunkEntity(srcChunk.MetaChunkEntity);
					srcChunk.MetaChunkEntity = Entity.Null;
					return;
				}
				SharedComponentValues sharedComponentValues2 = dstArchetype->Chunks.GetSharedComponentValues(entityComponentStore->GetChunk(srcChunk.MetaChunkEntity).ListIndex);
				entityComponentStore->Move(srcChunk.MetaChunkEntity, dstArchetype->MetaChunkArchetype, sharedComponentValues2);
			}
		}

		// Token: 0x06000101 RID: 257 RVA: 0x0000570C File Offset: 0x0000390C
		public unsafe static void MoveArchetype(Archetype* srcArchetype, ChunkIndex chunk, Archetype* dstArchetype, SharedComponentValues sharedComponentValues)
		{
			EntityComponentStore* entityComponentStore = dstArchetype->EntityComponentStore;
			uint globalSystemVersion = entityComponentStore->GlobalSystemVersion;
			int count = chunk.Count;
			bool flag = count < srcArchetype->ChunkCapacity;
			if (flag)
			{
				srcArchetype->EmptySlotTrackingRemoveChunk(chunk);
			}
			int listIndex = chunk.ListIndex;
			dstArchetype->AddToChunkList(chunk, sharedComponentValues, globalSystemVersion, ref entityComponentStore->m_ChunkListChangesTracker);
			int listIndex2 = chunk.ListIndex;
			ChunkDataUtility.InitializeBitsForNewChunk(dstArchetype, listIndex2);
			ChunkDataUtility.MoveEnabledBits(srcArchetype, listIndex, dstArchetype, listIndex2, count);
			chunk.ListIndex = listIndex;
			srcArchetype->RemoveFromChunkList(chunk, ref entityComponentStore->m_ChunkListChangesTracker);
			chunk.ListIndex = listIndex2;
			entityComponentStore->SetArchetype(chunk, dstArchetype);
			if (flag)
			{
				dstArchetype->EmptySlotTrackingAddChunk(chunk);
			}
			srcArchetype->EntityCount = srcArchetype->EntityCount - count;
			dstArchetype->EntityCount = dstArchetype->EntityCount + count;
			entityComponentStore->IncrementComponentTypeOrderVersion(dstArchetype);
			dstArchetype->Chunks.SetOrderVersion(chunk.ListIndex, globalSystemVersion);
		}

		// Token: 0x06000102 RID: 258 RVA: 0x000057D4 File Offset: 0x000039D4
		public unsafe static void CloneChangeVersions(Archetype* srcArchetype, int chunkIndexInSrcArchetype, Archetype* dstArchetype, int chunkIndexInDstArchetype, bool dstValidExistingVersions = false)
		{
			ComponentTypeInArchetype* types = dstArchetype->Types;
			ComponentTypeInArchetype* types2 = srcArchetype->Types;
			uint globalSystemVersion = dstArchetype->EntityComponentStore->GlobalSystemVersion;
			uint globalSystemVersion2 = srcArchetype->EntityComponentStore->GlobalSystemVersion;
			int num = srcArchetype->TypesCount - 1;
			for (int i = dstArchetype->TypesCount - 1; i >= 0; i--)
			{
				ComponentTypeInArchetype componentTypeInArchetype = types[i];
				while (types2[num] > componentTypeInArchetype)
				{
					num--;
				}
				uint num2 = globalSystemVersion;
				if (types2[num] == componentTypeInArchetype)
				{
					uint changeVersion = srcArchetype->Chunks.GetChangeVersion(num, chunkIndexInSrcArchetype);
					if (dstValidExistingVersions)
					{
						uint changeVersion2 = dstArchetype->Chunks.GetChangeVersion(i, chunkIndexInDstArchetype);
						uint num3 = globalSystemVersion2 - changeVersion;
						if (globalSystemVersion - changeVersion2 < num3)
						{
							num2 = changeVersion2;
						}
						else
						{
							num2 = globalSystemVersion - num3;
						}
					}
					else
					{
						num2 = changeVersion;
					}
				}
				dstArchetype->Chunks.SetChangeVersion(i, chunkIndexInDstArchetype, num2);
			}
		}

		// Token: 0x06000103 RID: 259 RVA: 0x000058D4 File Offset: 0x00003AD4
		public unsafe static void AllocateClone(Archetype* archetype, ChunkIndex chunk, Entity* entities, int count, Entity srcEntity)
		{
			EntityComponentStore* entityComponentStore = archetype->EntityComponentStore;
			uint globalSystemVersion = entityComponentStore->GlobalSystemVersion;
			EntityInChunk entityInChunk = entityComponentStore->GetEntityInChunk(srcEntity);
			int num2;
			int num = ChunkDataUtility.AllocateIntoChunk(archetype, chunk, count, out num2);
			entityComponentStore->AllocateEntities(archetype, chunk, num2, num, entities);
			ChunkDataUtility.ReplicateComponents(entityComponentStore->GetArchetype(entityInChunk.Chunk), entityInChunk.Chunk, entityInChunk.IndexInChunk, archetype, chunk, num2, num);
			int listIndex = chunk.ListIndex;
			archetype->Chunks.SetAllChangeVersion(listIndex, globalSystemVersion);
			archetype->Chunks.SetOrderVersion(listIndex, globalSystemVersion);
			for (int i = 0; i < num; i++)
			{
				entityComponentStore->CopyName(entities[i], srcEntity);
			}
			SharedComponentValues sharedComponentValues = archetype->Chunks.GetSharedComponentValues(listIndex);
			entityComponentStore->ManagedChangesTracker.IncrementComponentOrderVersion(archetype, sharedComponentValues);
			entityComponentStore->IncrementComponentTypeOrderVersion(archetype);
		}

		// Token: 0x06000104 RID: 260 RVA: 0x000059A8 File Offset: 0x00003BA8
		public unsafe static void Deallocate(Archetype* archetype, ChunkIndex chunk)
		{
			EntityBatchInChunk entityBatchInChunk = default(EntityBatchInChunk);
			entityBatchInChunk.Chunk = chunk;
			entityBatchInChunk.StartIndex = 0;
			entityBatchInChunk.Count = chunk.Count;
			ChunkDataUtility.Deallocate(archetype, in entityBatchInChunk);
		}

		// Token: 0x06000105 RID: 261 RVA: 0x000059E4 File Offset: 0x00003BE4
		public unsafe static void Deallocate(Archetype* archetype, in EntityBatchInChunk batch)
		{
			ChunkIndex chunk = batch.Chunk;
			EntityComponentStore* entityComponentStore = archetype->EntityComponentStore;
			uint globalSystemVersion = entityComponentStore->GlobalSystemVersion;
			int startIndex = batch.StartIndex;
			int count = batch.Count;
			entityComponentStore->DeallocateDataEntitiesInChunk(chunk, archetype, startIndex, count);
			SharedComponentValues sharedComponentValues = archetype->Chunks.GetSharedComponentValues(chunk.ListIndex);
			entityComponentStore->ManagedChangesTracker.IncrementComponentOrderVersion(archetype, sharedComponentValues);
			archetype->Chunks.SetOrderVersion(chunk.ListIndex, globalSystemVersion);
			entityComponentStore->IncrementComponentTypeOrderVersion(archetype);
			archetype->EntityCount = archetype->EntityCount - count;
			int num = chunk.Count - count;
			ChunkDataUtility.SetChunkCount(archetype, chunk, num);
			if (num != 0)
			{
				ChunkDataUtility.UpdateChunkDisabledEntityCounts(chunk, archetype);
			}
		}

		// Token: 0x06000106 RID: 262 RVA: 0x00005A84 File Offset: 0x00003C84
		public unsafe static void Clone(Archetype* srcArchetype, in EntityBatchInChunk srcBatch, Archetype* dstArchetype, ChunkIndex dstChunk)
		{
			ChunkIndex chunk = srcBatch.Chunk;
			int startIndex = srcBatch.StartIndex;
			int count = srcBatch.Count;
			EntityComponentStore* entityComponentStore = dstArchetype->EntityComponentStore;
			uint globalSystemVersion = entityComponentStore->GlobalSystemVersion;
			bool flag = dstChunk.Count != 0;
			int num2;
			int num = ChunkDataUtility.AllocateIntoChunk(dstArchetype, dstChunk, count, out num2);
			ChunkDataUtility.Convert(srcArchetype, chunk, startIndex, dstArchetype, dstChunk, num2, num);
			Entity* componentDataRO = (Entity*)ChunkDataUtility.GetComponentDataRO(dstChunk, dstArchetype, num2, 0);
			for (int i = 0; i < num; i++)
			{
				Entity entity = componentDataRO[i];
				entityComponentStore->SetEntityInChunk(entity, new EntityInChunk
				{
					Chunk = dstChunk,
					IndexInChunk = num2 + i
				});
			}
			ChunkDataUtility.CloneChangeVersions(srcArchetype, chunk.ListIndex, dstArchetype, dstChunk.ListIndex, flag);
			ChunkDataUtility.CloneEnabledBits(chunk, srcArchetype, startIndex, dstChunk, dstArchetype, num2, num);
			ChunkDataUtility.UpdateChunkDisabledEntityCounts(dstChunk, dstArchetype);
			dstArchetype->Chunks.SetOrderVersion(dstChunk.ListIndex, globalSystemVersion);
			entityComponentStore->IncrementComponentTypeOrderVersion(dstArchetype);
			SharedComponentValues sharedComponentValues = dstArchetype->Chunks.GetSharedComponentValues(dstChunk.ListIndex);
			entityComponentStore->ManagedChangesTracker.IncrementComponentOrderVersion(dstArchetype, sharedComponentValues);
			if (dstArchetype->CleanupComplete)
			{
				entityComponentStore->DestroyEntities(componentDataRO, num);
			}
		}

		// Token: 0x06000107 RID: 263 RVA: 0x00005BB0 File Offset: 0x00003DB0
		private unsafe static void ReplicateComponents(Archetype* srcArchetype, ChunkIndex srcChunk, int srcIndex, Archetype* dstArchetype, ChunkIndex dstChunk, int dstBaseIndex, int count)
		{
			byte* buffer = srcChunk.Buffer;
			byte* buffer2 = dstChunk.Buffer;
			int* offsets = srcArchetype->Offsets;
			ushort* sizeOfs = srcArchetype->SizeOfs;
			int* bufferCapacities = srcArchetype->BufferCapacities;
			ComponentTypeInArchetype* types = srcArchetype->Types;
			ComponentTypeInArchetype* types2 = dstArchetype->Types;
			int* offsets2 = dstArchetype->Offsets;
			int num = 1;
			int nativeComponentsEnd = srcArchetype->NativeComponentsEnd;
			for (int num2 = 1; num2 != nativeComponentsEnd; num2++)
			{
				ref ComponentTypeInArchetype ptr = types[num2];
				ComponentTypeInArchetype componentTypeInArchetype = types2[num];
				if (!(ptr.TypeIndex != componentTypeInArchetype.TypeIndex))
				{
					ushort num3 = sizeOfs[num2];
					byte* ptr2 = buffer + (offsets[num2] + (int)num3 * srcIndex);
					UnsafeUtility.MemCpyReplicate((void*)(buffer2 + (offsets2[num] + (int)num3 * dstBaseIndex)), (void*)ptr2, (int)num3, count);
					num++;
				}
			}
			num = (int)dstArchetype->FirstBufferComponent;
			int bufferComponentsEnd = srcArchetype->BufferComponentsEnd;
			short num4 = srcArchetype->FirstBufferComponent;
			while ((int)num4 != bufferComponentsEnd)
			{
				ComponentTypeInArchetype componentTypeInArchetype2 = types[num4];
				ComponentTypeInArchetype componentTypeInArchetype3 = types2[num];
				if (!(componentTypeInArchetype2.TypeIndex != componentTypeInArchetype3.TypeIndex))
				{
					ushort num5 = sizeOfs[num4];
					byte* ptr3 = buffer + (offsets[num4] + (int)num5 * srcIndex);
					byte* ptr4 = buffer2 + (offsets2[num] + (int)num5 * dstBaseIndex);
					int num6 = bufferCapacities[num4];
					int num7 = 8;
					int elementSize = TypeManager.GetTypeInfo(componentTypeInArchetype2.TypeIndex).ElementSize;
					for (int i = 0; i < count; i++)
					{
						BufferHeader* ptr5 = (BufferHeader*)ptr3;
						BufferHeader* ptr6 = (BufferHeader*)ptr4;
						BufferHeader.Initialize(ptr6, num6);
						BufferHeader.Assign(ptr6, BufferHeader.GetElementPointer(ptr5), ptr5->Length, elementSize, num7, false, 0);
						ptr4 += num5;
					}
					num++;
				}
				num4 += 1;
			}
			ChunkDataUtility.ReplicateEnabledBits(srcChunk, srcArchetype, srcIndex, dstChunk, dstArchetype, dstBaseIndex, count);
			int* ptr7 = null;
			if (srcArchetype->HasCompanionComponents && dstArchetype->HasCompanionComponents)
			{
				int indexInTypeArray = ChunkDataUtility.GetIndexInTypeArray(srcArchetype, ManagedComponentStore.CompanionLinkTypeIndex);
				int indexInTypeArray2 = ChunkDataUtility.GetIndexInTypeArray(dstArchetype, ManagedComponentStore.CompanionLinkTypeIndex);
				int indexInTypeArray3 = ChunkDataUtility.GetIndexInTypeArray(dstArchetype, ManagedComponentStore.CompanionLinkTransformTypeIndex);
				if (indexInTypeArray != -1 && indexInTypeArray2 != -1 && indexInTypeArray3 != -1)
				{
					int* ptr8 = (int*)(buffer + offsets[indexInTypeArray] + 4 * srcIndex);
					int* ptr9 = (int*)(buffer2 + offsets2[indexInTypeArray2] + 4 * dstBaseIndex);
					int* ptr10 = (int*)(buffer2 + offsets2[indexInTypeArray3] + 4 * dstBaseIndex);
					NativeArray<int> nativeArray = NativeArrayUnsafeUtility.ConvertExistingDataToNativeArray<int>((void*)ptr9, count, Allocator.Invalid);
					NativeArray<int> nativeArray2 = NativeArrayUnsafeUtility.ConvertExistingDataToNativeArray<int>((void*)ptr10, count, Allocator.Invalid);
					Scene scene = GameObject.GetScene(*ptr8);
					GameObject.InstantiateGameObjects(*ptr8, count, nativeArray, nativeArray2, scene);
					ptr7 = ptr9;
				}
			}
			if (dstArchetype->NumManagedComponents > 0)
			{
				ChunkDataUtility.ReplicateManagedComponents(srcArchetype, srcChunk, srcIndex, dstArchetype, dstChunk, dstBaseIndex, count, ptr7);
			}
		}

		// Token: 0x06000108 RID: 264 RVA: 0x00005E7C File Offset: 0x0000407C
		private unsafe static void ReplicateManagedComponents(Archetype* srcArchetype, ChunkIndex srcChunk, int srcIndex, Archetype* dstArchetype, ChunkIndex dstChunk, int dstBaseIndex, int count, int* newCompanionLinkIds)
		{
			EntityComponentStore* entityComponentStore = dstArchetype->EntityComponentStore;
			ComponentTypeInArchetype* types = srcArchetype->Types;
			ComponentTypeInArchetype* types2 = dstArchetype->Types;
			int* offsets = srcArchetype->Offsets;
			int* offsets2 = dstArchetype->Offsets;
			int numManagedComponents = dstArchetype->NumManagedComponents;
			int num = 0;
			int num2 = 0;
			int* ptr;
			IntPtr* ptr2;
			short num3;
			int managedComponentsEnd;
			checked
			{
				ptr = stackalloc int[unchecked((UIntPtr)numManagedComponents) * 4];
				ptr2 = stackalloc IntPtr[unchecked((UIntPtr)numManagedComponents) * (UIntPtr)sizeof(IntPtr)];
				num3 = dstArchetype->FirstManagedComponent;
				managedComponentsEnd = srcArchetype->ManagedComponentsEnd;
			}
			byte* ptr3 = srcChunk.Buffer + 4 * srcIndex;
			byte* ptr4 = dstChunk.Buffer + 4 * dstBaseIndex;
			bool hasCompanionComponents = dstArchetype->HasCompanionComponents;
			short num4 = srcArchetype->FirstManagedComponent;
			while ((int)num4 != managedComponentsEnd)
			{
				ComponentTypeInArchetype componentTypeInArchetype = types[num4];
				ComponentTypeInArchetype componentTypeInArchetype2 = types2[num3];
				if (!(componentTypeInArchetype.TypeIndex != componentTypeInArchetype2.TypeIndex))
				{
					int num5 = *(int*)(ptr3 + offsets[num4]);
					byte* ptr5 = ptr4 + offsets2[num3];
					if (num5 == 0)
					{
						UnsafeUtility.MemClear((void*)ptr5, (long)(4 * count));
					}
					else if (hasCompanionComponents && TypeManager.GetTypeInfo(componentTypeInArchetype.TypeIndex).Category == TypeManager.TypeCategory.UnityEngineObject)
					{
						int num6 = numManagedComponents - num2 - 1;
						ptr[num6] = num5;
						ptr2[(IntPtr)num6 * (IntPtr)sizeof(IntPtr) / (IntPtr)sizeof(IntPtr)] = (IntPtr)((void*)ptr5);
						num2++;
					}
					else
					{
						ptr[num] = num5;
						ptr2[(IntPtr)num * (IntPtr)sizeof(IntPtr) / (IntPtr)sizeof(IntPtr)] = (IntPtr)((void*)ptr5);
						num++;
					}
					num3 += 1;
				}
				num4 += 1;
			}
			entityComponentStore->ReserveManagedComponentIndices(count * (num + num2));
			entityComponentStore->ManagedChangesTracker.CloneManagedComponentBegin(ptr, num, count);
			for (int i = 0; i < num; i++)
			{
				int* ptr6 = (int*)(void*)ptr2[(IntPtr)i * (IntPtr)sizeof(IntPtr) / (IntPtr)sizeof(IntPtr)];
				entityComponentStore->AllocateManagedComponentIndices(ptr6, count);
				entityComponentStore->ManagedChangesTracker.CloneManagedComponentAddDstIndices(ptr6, count);
			}
			if (hasCompanionComponents)
			{
				int indexInTypeArray = ChunkDataUtility.GetIndexInTypeArray(dstArchetype, ManagedComponentStore.CompanionReferenceTypeIndex);
				int* ptr7 = (int*)((indexInTypeArray == -1) ? null : (ptr4 + offsets2[indexInTypeArray]));
				Entity* ptr8 = (Entity*)(dstChunk.Buffer + (IntPtr)dstBaseIndex * (IntPtr)sizeof(Entity));
				entityComponentStore->ManagedChangesTracker.CloneCompanionComponentBegin(ptr + numManagedComponents - num2, num2, ptr8, count, ptr7, newCompanionLinkIds);
				for (int j = numManagedComponents - num2; j < numManagedComponents; j++)
				{
					int* ptr9 = (int*)(void*)ptr2[(IntPtr)j * (IntPtr)sizeof(IntPtr) / (IntPtr)sizeof(IntPtr)];
					entityComponentStore->AllocateManagedComponentIndices(ptr9, count);
					entityComponentStore->ManagedChangesTracker.CloneCompanionComponentAddDstIndices(ptr9, count);
				}
			}
		}

		// Token: 0x06000109 RID: 265 RVA: 0x00006108 File Offset: 0x00004308
		public unsafe static void ClearMissingReferences(Archetype* archetype, ChunkIndex chunk)
		{
			EntityComponentStore* entityComponentStore = archetype->EntityComponentStore;
			uint globalSystemVersion = entityComponentStore->GlobalSystemVersion;
			int typesCount = archetype->TypesCount;
			int count = chunk.Count;
			byte* buffer = chunk.Buffer;
			int listIndex = chunk.ListIndex;
			for (int i = 1; i < typesCount; i++)
			{
				ComponentTypeInArchetype componentTypeInArchetype = archetype->Types[i];
				if (componentTypeInArchetype.HasEntityReferences && !componentTypeInArchetype.IsSharedComponent && !componentTypeInArchetype.IsZeroSized)
				{
					readonly ref TypeManager.TypeInfo typeInfo = ref entityComponentStore->GetTypeInfo(componentTypeInArchetype.TypeIndex);
					byte* ptr = buffer + archetype->Offsets[i];
					ushort num = archetype->SizeOfs[i];
					bool flag = false;
					if (componentTypeInArchetype.IsBuffer)
					{
						for (int j = 0; j < count; j++)
						{
							BufferHeader* ptr2 = (BufferHeader*)(ptr + (int)num * j);
							int length = ptr2->Length;
							byte* elementPointer = BufferHeader.GetElementPointer(ptr2);
							flag |= ChunkDataUtility.ClearEntityReferences(entityComponentStore, in typeInfo, elementPointer, length);
						}
					}
					else
					{
						for (int k = 0; k < count; k++)
						{
							byte* ptr3 = ptr + (int)num * k;
							flag |= ChunkDataUtility.ClearEntityReferences(entityComponentStore, in typeInfo, ptr3, 1);
						}
					}
					if (flag)
					{
						archetype->Chunks.SetChangeVersion(i, listIndex, globalSystemVersion);
					}
				}
			}
		}

		// Token: 0x0600010A RID: 266 RVA: 0x00006250 File Offset: 0x00004450
		private unsafe static bool ClearEntityReferences(EntityComponentStore* entityComponentStore, in TypeManager.TypeInfo typeInfo, byte* address, int elementCount)
		{
			bool flag = false;
			TypeManager.EntityOffsetInfo* entityOffsets = entityComponentStore->GetEntityOffsets(in typeInfo);
			for (int i = 0; i < elementCount; i++)
			{
				byte* ptr = address + typeInfo.ElementSize * i;
				for (int j = 0; j < typeInfo.EntityOffsetCount; j++)
				{
					int offset = entityOffsets[j].Offset;
					if (!entityComponentStore->Exists(*(Entity*)(ptr + offset)))
					{
						*(Entity*)(ptr + offset) = Entity.Null;
						flag = true;
					}
				}
			}
			return flag;
		}

		// Token: 0x0600010B RID: 267 RVA: 0x000062C8 File Offset: 0x000044C8
		public unsafe static Entity GetEntityFromEntityInChunk(Archetype* archetype, EntityInChunk entityInChunk)
		{
			return *(Entity*)(entityInChunk.Chunk.Buffer + *archetype->Offsets + entityInChunk.IndexInChunk * (int)(*archetype->SizeOfs));
		}

		// Token: 0x0600010C RID: 268 RVA: 0x000062F4 File Offset: 0x000044F4
		public unsafe static void AddExistingChunk(Archetype* archetype, ChunkIndex chunk, int* sharedComponentIndices, byte* enabledBitsValuesForChunk, int* perComponentDisabledBitCount)
		{
			EntityComponentStore* entityComponentStore = archetype->EntityComponentStore;
			uint globalSystemVersion = entityComponentStore->GlobalSystemVersion;
			archetype->AddToChunkList(chunk, sharedComponentIndices, globalSystemVersion, ref entityComponentStore->m_ChunkListChangesTracker);
			archetype->EntityCount = archetype->EntityCount + chunk.Count;
			ChunkDataUtility.InitializeBitsForNewChunk(archetype, chunk.ListIndex);
			archetype->Chunks.SetEnabledBitsAndHierarchicalData(chunk.ListIndex, enabledBitsValuesForChunk, perComponentDisabledBitCount);
			for (int i = 0; i < archetype->NumSharedComponents; i++)
			{
				int num = sharedComponentIndices[i];
				if (EntityComponentStore.IsUnmanagedSharedComponentIndex(num))
				{
					entityComponentStore->AddSharedComponentReference_Unmanaged(num, 1);
				}
				else
				{
					entityComponentStore->ManagedChangesTracker.AddReference(num, 1);
				}
			}
			if (chunk.Count < archetype->ChunkCapacity)
			{
				archetype->EmptySlotTrackingAddChunk(chunk);
			}
			entityComponentStore->AddExistingEntitiesInChunk(chunk);
		}

		// Token: 0x0600010D RID: 269 RVA: 0x000063AC File Offset: 0x000045AC
		public unsafe static void AddEmptyChunk(Archetype* archetype, ChunkIndex chunk, SharedComponentValues sharedComponentValues)
		{
			EntityComponentStore* entityComponentStore = archetype->EntityComponentStore;
			uint globalSystemVersion = entityComponentStore->GlobalSystemVersion;
			entityComponentStore->SetArchetype(chunk, archetype);
			chunk.Count = 0;
			chunk.SequenceNumber = entityComponentStore->AllocateSequenceNumber();
			chunk.MetaChunkEntity = Entity.Null;
			int numSharedComponents = archetype->NumSharedComponents;
			if (numSharedComponents > 0)
			{
				for (int i = 0; i < numSharedComponents; i++)
				{
					int num = *sharedComponentValues[i];
					if (EntityComponentStore.IsUnmanagedSharedComponentIndex(num))
					{
						entityComponentStore->AddSharedComponentReference_Unmanaged(num, 1);
					}
					else
					{
						entityComponentStore->ManagedChangesTracker.AddReference(num, 1);
					}
				}
			}
			archetype->AddToChunkList(chunk, sharedComponentValues, globalSystemVersion, ref entityComponentStore->m_ChunkListChangesTracker);
			ChunkDataUtility.InitializeBitsForNewChunk(archetype, chunk.ListIndex);
			archetype->EmptySlotTrackingAddChunk(chunk);
			chunk.Flags = 0U;
		}

		// Token: 0x0600010E RID: 270 RVA: 0x00006464 File Offset: 0x00004664
		[BurstCompile]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal unsafe static void SwapComponents$BurstManaged(Archetype* srcArch, ChunkIndex srcChunk, int srcIndex, Archetype* dstArch, ChunkIndex dstChunk, int dstIndex, int count, uint srcGlobalSystemVersion, uint dstGlobalSystemVersion)
		{
			int typesCount = srcArch->TypesCount;
			byte* buffer = srcChunk.Buffer;
			byte* buffer2 = dstChunk.Buffer;
			int* offsets = srcArch->Offsets;
			ushort* sizeOfs = srcArch->SizeOfs;
			int listIndex = dstChunk.ListIndex;
			int listIndex2 = srcChunk.ListIndex;
			for (int i = 1; i < typesCount; i++)
			{
				int num = offsets[i];
				ushort num2 = sizeOfs[i];
				byte* ptr = buffer + (num + (int)num2 * srcIndex);
				byte* ptr2 = buffer2 + (num + (int)num2 * dstIndex);
				byte* ptr3 = stackalloc byte[(UIntPtr)((int)num2 * count)];
				dstArch->Chunks.SetChangeVersion(i, listIndex, dstGlobalSystemVersion);
				srcArch->Chunks.SetChangeVersion(i, listIndex2, srcGlobalSystemVersion);
				UnsafeUtility.MemCpy((void*)ptr3, (void*)ptr, (long)((int)num2 * count));
				UnsafeUtility.MemCpy((void*)ptr, (void*)ptr2, (long)((int)num2 * count));
				UnsafeUtility.MemCpy((void*)ptr2, (void*)ptr3, (long)((int)num2 * count));
			}
		}

		// Token: 0x02000031 RID: 49
		// (Invoke) Token: 0x06000110 RID: 272
		internal unsafe delegate void SwapComponents_000000DB$PostfixBurstDelegate(Archetype* srcArch, ChunkIndex srcChunk, int srcIndex, Archetype* dstArch, ChunkIndex dstChunk, int dstIndex, int count, uint srcGlobalSystemVersion, uint dstGlobalSystemVersion);

		// Token: 0x02000032 RID: 50
		internal static class SwapComponents_000000DB$BurstDirectCall
		{
			// Token: 0x06000113 RID: 275 RVA: 0x00006544 File Offset: 0x00004744
			[BurstDiscard]
			private unsafe static void GetFunctionPointerDiscard(ref IntPtr A_0)
			{
				if (ChunkDataUtility.SwapComponents_000000DB$BurstDirectCall.Pointer == 0)
				{
					ChunkDataUtility.SwapComponents_000000DB$BurstDirectCall.Pointer = BurstCompiler.GetILPPMethodFunctionPointer2(ChunkDataUtility.SwapComponents_000000DB$BurstDirectCall.DeferredCompilation, methodof(ChunkDataUtility.SwapComponents$BurstManaged(Archetype*, ChunkIndex, int, Archetype*, ChunkIndex, int, int, uint, uint)).MethodHandle, typeof(ChunkDataUtility.SwapComponents_000000DB$PostfixBurstDelegate).TypeHandle);
				}
				A_0 = ChunkDataUtility.SwapComponents_000000DB$BurstDirectCall.Pointer;
			}

			// Token: 0x06000114 RID: 276 RVA: 0x00006570 File Offset: 0x00004770
			private static IntPtr GetFunctionPointer()
			{
				IntPtr intPtr = (IntPtr)0;
				ChunkDataUtility.SwapComponents_000000DB$BurstDirectCall.GetFunctionPointerDiscard(ref intPtr);
				return intPtr;
			}

			// Token: 0x06000115 RID: 277 RVA: 0x00006588 File Offset: 0x00004788
			public unsafe static void Constructor()
			{
				ChunkDataUtility.SwapComponents_000000DB$BurstDirectCall.DeferredCompilation = BurstCompiler.CompileILPPMethod2(methodof(ChunkDataUtility.SwapComponents(Archetype*, ChunkIndex, int, Archetype*, ChunkIndex, int, int, uint, uint)).MethodHandle);
			}

			// Token: 0x06000116 RID: 278 RVA: 0x00002287 File Offset: 0x00000487
			public static void Initialize()
			{
			}

			// Token: 0x06000117 RID: 279 RVA: 0x00006599 File Offset: 0x00004799
			// Note: this type is marked as 'beforefieldinit'.
			static SwapComponents_000000DB$BurstDirectCall()
			{
				ChunkDataUtility.SwapComponents_000000DB$BurstDirectCall.Constructor();
			}

			// Token: 0x06000118 RID: 280 RVA: 0x000065A0 File Offset: 0x000047A0
			public unsafe static void Invoke(Archetype* srcArch, ChunkIndex srcChunk, int srcIndex, Archetype* dstArch, ChunkIndex dstChunk, int dstIndex, int count, uint srcGlobalSystemVersion, uint dstGlobalSystemVersion)
			{
				if (BurstCompiler.IsEnabled)
				{
					IntPtr functionPointer = ChunkDataUtility.SwapComponents_000000DB$BurstDirectCall.GetFunctionPointer();
					if (functionPointer != 0)
					{
						calli(System.Void(Unity.Entities.Archetype*,Unity.Entities.ChunkIndex,System.Int32,Unity.Entities.Archetype*,Unity.Entities.ChunkIndex,System.Int32,System.Int32,System.UInt32,System.UInt32), srcArch, srcChunk, srcIndex, dstArch, dstChunk, dstIndex, count, srcGlobalSystemVersion, dstGlobalSystemVersion, functionPointer);
						return;
					}
				}
				ChunkDataUtility.SwapComponents$BurstManaged(srcArch, srcChunk, srcIndex, dstArch, dstChunk, dstIndex, count, srcGlobalSystemVersion, dstGlobalSystemVersion);
			}

			// Token: 0x04000045 RID: 69
			private static IntPtr Pointer;

			// Token: 0x04000046 RID: 70
			private static IntPtr DeferredCompilation;
		}
	}
}
