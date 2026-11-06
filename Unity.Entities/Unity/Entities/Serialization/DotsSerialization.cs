using System;
using Unity.Collections;

namespace Unity.Entities.Serialization
{
	// Token: 0x02000370 RID: 880
	internal static class DotsSerialization
	{
		// Token: 0x06001798 RID: 6040 RVA: 0x000586E5 File Offset: 0x000568E5
		public static DotsSerializationWriter CreateWriter(BinaryWriter writer, Hash128 fileId, FixedString64Bytes fileType)
		{
			return new DotsSerializationWriter(writer, fileId, fileType);
		}

		// Token: 0x06001799 RID: 6041 RVA: 0x000586EF File Offset: 0x000568EF
		public static DotsSerializationReader CreateReader(BinaryReader reader)
		{
			return new DotsSerializationReader(reader);
		}

		// Token: 0x0600179A RID: 6042 RVA: 0x000586F7 File Offset: 0x000568F7
		public static DotsSerializationReader CreateReader(ref DotsSerialization.BlobHeader header)
		{
			return new DotsSerializationReader(ref header);
		}

		// Token: 0x040008DB RID: 2267
		internal static readonly byte[] HeaderMagic = new byte[] { 68, 79, 84, 83, 66, 73, 78, 33 };

		// Token: 0x040008DC RID: 2268
		internal const ulong RootNodeHash = 0UL;

		// Token: 0x02000371 RID: 881
		public struct FileHeader
		{
			// Token: 0x040008DD RID: 2269
			public long MagicValue;

			// Token: 0x040008DE RID: 2270
			public int FileVersion;

			// Token: 0x040008DF RID: 2271
			public int HeaderSize;

			// Token: 0x040008E0 RID: 2272
			public Hash128 FileId;

			// Token: 0x040008E1 RID: 2273
			public FixedString64Bytes FileType;

			// Token: 0x040008E2 RID: 2274
			public int FirstLevelNodesCount;

			// Token: 0x040008E3 RID: 2275
			public long NodesSectionOffset;

			// Token: 0x040008E4 RID: 2276
			public int NodesSectionSize;

			// Token: 0x040008E5 RID: 2277
			public long MetadataSectionOffset;

			// Token: 0x040008E6 RID: 2278
			public int MetadataSectionSize;

			// Token: 0x040008E7 RID: 2279
			public long DataSectionOffset;

			// Token: 0x040008E8 RID: 2280
			public long DataSectionSize;
		}

		// Token: 0x02000372 RID: 882
		public struct BlobHeader
		{
			// Token: 0x040008E9 RID: 2281
			public DotsSerialization.FileHeader FileHeader;

			// Token: 0x040008EA RID: 2282
			public BlobArray<byte> NodeSection;

			// Token: 0x040008EB RID: 2283
			public BlobArray<byte> MetadataSection;
		}

		// Token: 0x02000373 RID: 883
		public struct NodeHeader : IComponentData, IQueryTypeParameter
		{
			// Token: 0x17000209 RID: 521
			// (get) Token: 0x0600179C RID: 6044 RVA: 0x00058717 File Offset: 0x00056917
			public bool HasMetadata
			{
				get
				{
					return this.MetadataStartingOffset != -1L;
				}
			}

			// Token: 0x040008EC RID: 2284
			public ulong NodeTypeHash;

			// Token: 0x040008ED RID: 2285
			public Hash128 Id;

			// Token: 0x040008EE RID: 2286
			public int Size;

			// Token: 0x040008EF RID: 2287
			public int NextSiblingOffset;

			// Token: 0x040008F0 RID: 2288
			public int ChildrenCount;

			// Token: 0x040008F1 RID: 2289
			public long MetadataStartingOffset;

			// Token: 0x040008F2 RID: 2290
			public int MetadataSize;

			// Token: 0x040008F3 RID: 2291
			public long DataStartingOffset;

			// Token: 0x040008F4 RID: 2292
			public long DataSize;
		}

		// Token: 0x02000374 RID: 884
		public struct FolderNode : IComponentData, IQueryTypeParameter
		{
			// Token: 0x040008F5 RID: 2293
			public DotsSerialization.NodeHeader Header;
		}

		// Token: 0x02000375 RID: 885
		public struct StringTableNode : IComponentData, IQueryTypeParameter
		{
			// Token: 0x040008F6 RID: 2294
			public DotsSerialization.NodeHeader Header;

			// Token: 0x040008F7 RID: 2295
			public int StringCount;
		}

		// Token: 0x02000376 RID: 886
		public struct RevisionedRawDataNode : IComponentData, IQueryTypeParameter
		{
			// Token: 0x040008F8 RID: 2296
			public DotsSerialization.NodeHeader Header;

			// Token: 0x040008F9 RID: 2297
			public int Revision;
		}

		// Token: 0x02000377 RID: 887
		public struct TypeNamesNode : IComponentData, IQueryTypeParameter
		{
			// Token: 0x040008FA RID: 2298
			public DotsSerialization.NodeHeader Header;

			// Token: 0x040008FB RID: 2299
			public int TypeCount;
		}
	}
}
