using System;
using System.Runtime.InteropServices;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Entities.Serialization;

namespace Unity.Entities
{
	// Token: 0x02000314 RID: 788
	[StructLayout(LayoutKind.Explicit, Size = 8)]
	internal struct RuntimeBlobHeaderRef
	{
		// Token: 0x170001AC RID: 428
		// (get) Token: 0x0600153B RID: 5435 RVA: 0x00050843 File Offset: 0x0004EA43
		public ref DotsSerialization.BlobHeader Value
		{
			get
			{
				return UnsafeUtility.As<long, BlobAssetReference<DotsSerialization.BlobHeader>>(ref this.m_BlobAssetRefStorage).Value;
			}
		}

		// Token: 0x0600153C RID: 5436 RVA: 0x00050858 File Offset: 0x0004EA58
		public unsafe static implicit operator RuntimeBlobHeaderRef(BlobAssetReference<DotsSerialization.BlobHeader> assetRef)
		{
			RuntimeBlobHeaderRef runtimeBlobHeaderRef = default(RuntimeBlobHeaderRef);
			*UnsafeUtility.As<long, BlobAssetReference<DotsSerialization.BlobHeader>>(ref runtimeBlobHeaderRef.m_BlobAssetRefStorage) = assetRef;
			return runtimeBlobHeaderRef;
		}

		// Token: 0x0600153D RID: 5437 RVA: 0x00050880 File Offset: 0x0004EA80
		public unsafe static implicit operator BlobAssetReference<DotsSerialization.BlobHeader>(RuntimeBlobHeaderRef clip)
		{
			return *UnsafeUtility.As<long, BlobAssetReference<DotsSerialization.BlobHeader>>(ref clip.m_BlobAssetRefStorage);
		}

		// Token: 0x0600153E RID: 5438 RVA: 0x00050894 File Offset: 0x0004EA94
		public unsafe RuntimeBlobHeaderRef Resolve(BlobAssetOwner blobAssetOwner)
		{
			BlobAssetReference<DotsSerialization.BlobHeader> blobAssetReference = default(BlobAssetReference<DotsSerialization.BlobHeader>);
			blobAssetReference.m_data.m_Ptr = (byte*)(blobAssetOwner.BlobAssetBatchPtr + this.m_BlobAssetRefStorage / (long)sizeof(BlobAssetBatch));
			return blobAssetReference;
		}

		// Token: 0x040007AC RID: 1964
		[FieldOffset(0)]
		internal long m_BlobAssetRefStorage;
	}
}
