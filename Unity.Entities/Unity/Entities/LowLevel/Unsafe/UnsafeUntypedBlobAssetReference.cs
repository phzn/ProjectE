using System;
using System.Runtime.CompilerServices;

namespace Unity.Entities.LowLevel.Unsafe
{
	// Token: 0x020004CA RID: 1226
	[ChunkSerializable]
	public struct UnsafeUntypedBlobAssetReference : IDisposable, IEquatable<UnsafeUntypedBlobAssetReference>
	{
		// Token: 0x06001D73 RID: 7539 RVA: 0x00066920 File Offset: 0x00064B20
		public static UnsafeUntypedBlobAssetReference Create<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(BlobAssetReference<T> blob) where T : struct, ValueType
		{
			UnsafeUntypedBlobAssetReference unsafeUntypedBlobAssetReference;
			unsafeUntypedBlobAssetReference.m_data = blob.m_data;
			return unsafeUntypedBlobAssetReference;
		}

		// Token: 0x06001D74 RID: 7540 RVA: 0x0006693C File Offset: 0x00064B3C
		public BlobAssetReference<T> Reinterpret<[global::System.Runtime.CompilerServices.IsUnmanaged] T>() where T : struct, ValueType
		{
			BlobAssetReference<T> blobAssetReference;
			blobAssetReference.m_data = this.m_data;
			return blobAssetReference;
		}

		// Token: 0x06001D75 RID: 7541 RVA: 0x00066957 File Offset: 0x00064B57
		public void Dispose()
		{
			this.m_data.Dispose();
		}

		// Token: 0x06001D76 RID: 7542 RVA: 0x00066964 File Offset: 0x00064B64
		public bool Equals(UnsafeUntypedBlobAssetReference other)
		{
			return this.m_data.Equals(other.m_data);
		}

		// Token: 0x04000A92 RID: 2706
		internal BlobAssetReferenceData m_data;
	}
}
