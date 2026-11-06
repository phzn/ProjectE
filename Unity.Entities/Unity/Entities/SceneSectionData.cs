using System;
using System.Runtime.InteropServices;
using Unity.Core.Compression;
using Unity.Mathematics;

namespace Unity.Entities
{
	// Token: 0x02000315 RID: 789
	[Serializable]
	[StructLayout(LayoutKind.Explicit)]
	public struct SceneSectionData : IComponentData, IQueryTypeParameter
	{
		// Token: 0x040007AD RID: 1965
		[FieldOffset(0)]
		public Hash128 SceneGUID;

		// Token: 0x040007AE RID: 1966
		[FieldOffset(16)]
		public int SubSectionIndex;

		// Token: 0x040007AF RID: 1967
		[FieldOffset(20)]
		public int FileSize;

		// Token: 0x040007B0 RID: 1968
		[FieldOffset(24)]
		public int ObjectReferenceCount;

		// Token: 0x040007B1 RID: 1969
		[FieldOffset(28)]
		public MinMaxAABB BoundingVolume;

		// Token: 0x040007B2 RID: 1970
		[FieldOffset(52)]
		internal Codec Codec;

		// Token: 0x040007B3 RID: 1971
		[FieldOffset(56)]
		internal int DecompressedFileSize;

		// Token: 0x040007B4 RID: 1972
		[FieldOffset(60)]
		internal int ExternalEntitiesRefRange;

		// Token: 0x040007B5 RID: 1973
		[FieldOffset(64)]
		internal RuntimeBlobHeaderRef BlobHeader;
	}
}
