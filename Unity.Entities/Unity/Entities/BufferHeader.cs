using System;
using System.Runtime.InteropServices;
using Unity.Burst;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;

namespace Unity.Entities
{
	// Token: 0x020002E6 RID: 742
	[NoAlias]
	[StructLayout(LayoutKind.Explicit)]
	internal struct BufferHeader
	{
		// Token: 0x06001424 RID: 5156 RVA: 0x0004DD93 File Offset: 0x0004BF93
		public unsafe static byte* GetElementPointer(BufferHeader* header)
		{
			if (header->Pointer != null)
			{
				return header->Pointer;
			}
			return (byte*)(header + 1);
		}

		// Token: 0x06001425 RID: 5157 RVA: 0x0004DDB0 File Offset: 0x0004BFB0
		public unsafe static void EnsureCapacity(BufferHeader* header, int count, int typeSize, int alignment, BufferHeader.TrashMode trashMode, bool useMemoryInitPattern, byte memoryInitPattern)
		{
			if (count <= header->Capacity)
			{
				return;
			}
			int num = Math.Max(8, Math.Max(2 * header->Capacity, count));
			BufferHeader.SetCapacity(header, num, typeSize, alignment, trashMode, useMemoryInitPattern, memoryInitPattern, 0);
		}

		// Token: 0x06001426 RID: 5158 RVA: 0x0004DDEC File Offset: 0x0004BFEC
		public unsafe static void SetCapacity(BufferHeader* header, int count, int typeSize, int alignment, BufferHeader.TrashMode trashMode, bool useMemoryInitPattern, byte memoryInitPattern, int internalCapacity)
		{
			if (count == header->Capacity)
			{
				return;
			}
			long num = (long)count * (long)typeSize;
			byte* elementPointer = BufferHeader.GetElementPointer(header);
			byte* ptr = (byte*)((count <= internalCapacity) ? (header + 1) : ((BufferHeader*)Memory.Unmanaged.Allocate(num, alignment, Allocator.Persistent)));
			if (elementPointer != ptr)
			{
				if (trashMode == BufferHeader.TrashMode.RetainOldData)
				{
					long num2 = Math.Min((long)header->Capacity, (long)count) * (long)typeSize;
					UnsafeUtility.MemCpy((void*)ptr, (void*)elementPointer, num2);
				}
				if (header->Pointer != null)
				{
					Memory.Unmanaged.Free<byte>(header->Pointer, Allocator.Persistent);
				}
			}
			header->Pointer = ((ptr == (byte*)(header + 1)) ? null : ptr);
			header->Capacity = count;
		}

		// Token: 0x06001427 RID: 5159 RVA: 0x0004DE8C File Offset: 0x0004C08C
		public unsafe static void Assign(BufferHeader* header, byte* source, int count, int typeSize, int alignment, bool useMemoryInitPattern, byte memoryInitPattern)
		{
			BufferHeader.EnsureCapacity(header, count, typeSize, alignment, BufferHeader.TrashMode.TrashOldData, useMemoryInitPattern, memoryInitPattern);
			UnsafeUtility.MemCpy((void*)BufferHeader.GetElementPointer(header), (void*)source, (long)typeSize * (long)count);
			header->Length = count;
		}

		// Token: 0x06001428 RID: 5160 RVA: 0x0004DEB5 File Offset: 0x0004C0B5
		public unsafe static void Initialize(BufferHeader* header, int bufferCapacity)
		{
			header->Pointer = null;
			header->Length = 0;
			header->Capacity = bufferCapacity;
		}

		// Token: 0x06001429 RID: 5161 RVA: 0x0004DECD File Offset: 0x0004C0CD
		public unsafe static void Destroy(BufferHeader* header)
		{
			if (header->Pointer != null)
			{
				Memory.Unmanaged.Free<byte>(header->Pointer, Allocator.Persistent);
			}
			BufferHeader.Initialize(header, 0);
		}

		// Token: 0x0600142A RID: 5162 RVA: 0x0004DEF1 File Offset: 0x0004C0F1
		public unsafe static void FreeBufferPtr(void* ptr)
		{
			Memory.Unmanaged.Free(ptr, Allocator.Persistent);
		}

		// Token: 0x0600142B RID: 5163 RVA: 0x0004DF00 File Offset: 0x0004C100
		public unsafe static void PatchAfterCloningChunk(Archetype* archetype, byte* chunkBuffer, int entityCount)
		{
			int i = 0;
			int typesCount = archetype->TypesCount;
			while (i < typesCount)
			{
				ComponentTypeInArchetype componentTypeInArchetype = archetype->Types[i];
				if (componentTypeInArchetype.IsBuffer)
				{
					readonly ref TypeManager.TypeInfo typeInfo = ref TypeManager.GetTypeInfo(componentTypeInArchetype.TypeIndex);
					ushort num = archetype->SizeOfs[i];
					int num2 = archetype->Offsets[i];
					for (int j = 0; j < entityCount; j++)
					{
						int num3 = num2 + (int)num * j;
						BufferHeader* ptr = (BufferHeader*)(chunkBuffer + num3);
						if (ptr->Pointer != null)
						{
							BufferHeader bufferHeader = *ptr;
							long num4 = (long)ptr->Capacity * (long)typeInfo.ElementSize;
							long num5 = (long)ptr->Length * (long)typeInfo.ElementSize;
							bufferHeader.Pointer = (byte*)Memory.Unmanaged.Allocate(num4, 16, Allocator.Persistent);
							UnsafeUtility.MemCpy((void*)bufferHeader.Pointer, (void*)ptr->Pointer, num5);
							*ptr = bufferHeader;
						}
					}
				}
				i++;
			}
		}

		// Token: 0x0600142C RID: 5164 RVA: 0x0004E004 File Offset: 0x0004C204
		public unsafe static void MemsetUnusedMemory(BufferHeader* bufferHeader, int internalCapacity, int elementSize, byte value)
		{
			if (bufferHeader->Pointer != null)
			{
				byte* ptr = (byte*)(bufferHeader + 1);
				UnsafeUtility.MemSet((void*)ptr, value, (long)(internalCapacity * elementSize));
			}
			int num = bufferHeader->Capacity - bufferHeader->Length;
			int length = bufferHeader->Length;
			UnsafeUtility.MemSet((void*)(BufferHeader.GetElementPointer(bufferHeader) + length * elementSize), value, (long)(num * elementSize));
		}

		// Token: 0x0400075E RID: 1886
		public const int kMinimumCapacity = 8;

		// Token: 0x0400075F RID: 1887
		[NoAlias]
		[FieldOffset(0)]
		public unsafe byte* Pointer;

		// Token: 0x04000760 RID: 1888
		[FieldOffset(8)]
		public int Length;

		// Token: 0x04000761 RID: 1889
		[FieldOffset(12)]
		public int Capacity;

		// Token: 0x020002E7 RID: 743
		public enum TrashMode
		{
			// Token: 0x04000763 RID: 1891
			TrashOldData,
			// Token: 0x04000764 RID: 1892
			RetainOldData
		}
	}
}
