using System;
using System.Runtime.CompilerServices;
using Unity.Collections;
using Unity.Entities.Serialization;

namespace Unity.Entities
{
	// Token: 0x0200002A RID: 42
	public static class BlobAssetSerializeExtensions
	{
		// Token: 0x060000B0 RID: 176 RVA: 0x00003854 File Offset: 0x00001A54
		public unsafe static void Write<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(this BinaryWriter binaryWriter, BlobAssetReference<T> blob) where T : struct, ValueType
		{
			int length = blob.m_data.Header->Length;
			BlobAssetHeader blobAssetHeader = BlobAssetHeader.CreateForSerialize(length, blob.m_data.Header->Hash);
			binaryWriter.WriteBytes((void*)(&blobAssetHeader), sizeof(BlobAssetHeader));
			binaryWriter.WriteBytes((void*)(blob.m_data.Header + 1), length);
		}

		// Token: 0x060000B1 RID: 177 RVA: 0x000038B4 File Offset: 0x00001AB4
		public unsafe static BlobAssetReference<T> Read<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(this BinaryReader binaryReader) where T : struct, ValueType
		{
			BlobAssetHeader blobAssetHeader;
			binaryReader.ReadBytes((void*)(&blobAssetHeader), sizeof(BlobAssetHeader));
			byte* ptr = (byte*)Memory.Unmanaged.Allocate((long)(sizeof(BlobAssetHeader) + blobAssetHeader.Length), 16, Allocator.Persistent);
			binaryReader.ReadBytes((void*)(ptr + sizeof(BlobAssetHeader)), blobAssetHeader.Length);
			BlobAssetHeader* ptr2 = (BlobAssetHeader*)ptr;
			ptr2->Allocator = Allocator.Persistent;
			ptr2->Length = blobAssetHeader.Length;
			ptr2->ValidationPtr = (void*)(ptr + sizeof(BlobAssetHeader));
			ptr2->Hash = blobAssetHeader.Hash;
			BlobAssetReference<T> blobAssetReference;
			blobAssetReference.m_data.m_Align8Union = 0L;
			blobAssetReference.m_data.m_Ptr = ptr + sizeof(BlobAssetHeader);
			return blobAssetReference;
		}
	}
}
