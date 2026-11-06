using System;
using Unity.Mathematics;

namespace Unity.Entities
{
	// Token: 0x02000021 RID: 33
	internal readonly struct BlobAssetPtr : IEquatable<BlobAssetPtr>
	{
		// Token: 0x17000007 RID: 7
		// (get) Token: 0x06000072 RID: 114 RVA: 0x00002EB6 File Offset: 0x000010B6
		public unsafe void* Data
		{
			get
			{
				return (void*)(this.Header + 1);
			}
		}

		// Token: 0x17000008 RID: 8
		// (get) Token: 0x06000073 RID: 115 RVA: 0x00002EC5 File Offset: 0x000010C5
		public unsafe int Length
		{
			get
			{
				return this.Header->Length;
			}
		}

		// Token: 0x17000009 RID: 9
		// (get) Token: 0x06000074 RID: 116 RVA: 0x00002ED2 File Offset: 0x000010D2
		public unsafe ulong Hash
		{
			get
			{
				return this.Header->Hash;
			}
		}

		// Token: 0x06000075 RID: 117 RVA: 0x00002EDF File Offset: 0x000010DF
		public unsafe BlobAssetPtr(BlobAssetHeader* header)
		{
			this.Header = header;
		}

		// Token: 0x06000076 RID: 118 RVA: 0x00002EE8 File Offset: 0x000010E8
		public bool Equals(BlobAssetPtr other)
		{
			return this.Header == other.Header;
		}

		// Token: 0x06000077 RID: 119 RVA: 0x00002EF8 File Offset: 0x000010F8
		public unsafe override int GetHashCode()
		{
			BlobAssetHeader* header = this.Header;
			return (int)math.hash((void*)(&header), sizeof(BlobAssetHeader*), 0U);
		}

		// Token: 0x0400002B RID: 43
		public unsafe readonly BlobAssetHeader* Header;
	}
}
