using System;
using System.Runtime.CompilerServices;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.IO.LowLevel.Unsafe;

namespace Unity.Entities.Serialization
{
	// Token: 0x0200037F RID: 895
	internal class DotsSerializationReader : IDisposable
	{
		// Token: 0x060017CC RID: 6092 RVA: 0x00059530 File Offset: 0x00057730
		internal unsafe DotsSerializationReader(BinaryReader reader)
		{
			this._reader = reader;
			byte[] array;
			byte* ptr;
			if ((array = DotsSerialization.HeaderMagic) == null || array.Length == 0)
			{
				ptr = null;
			}
			else
			{
				ptr = &array[0];
			}
			DotsSerialization.FileHeader fileHeader;
			this._reader.ReadBytes((void*)(&fileHeader), UnsafeUtility.SizeOf<DotsSerialization.FileHeader>());
			if (UnsafeUtility.MemCmp((void*)(&fileHeader), (void*)ptr, (long)DotsSerialization.HeaderMagic.Length) != 0)
			{
				throw new Exception("The file doesn't seem to be a DOTS Serialization file.");
			}
			array = null;
			if (fileHeader.FileVersion != 78)
			{
				throw new Exception(string.Format("Version mismatch! The file was written with version {0} but the latest version is {1}.", fileHeader.FileVersion, 78));
			}
			int num = UnsafeUtility.SizeOf<DotsSerialization.NodeHeader>();
			this._rootNodeHeader = (DotsSerialization.NodeHeader*)Memory.Unmanaged.Allocate((long)(fileHeader.NodesSectionSize + num), 16, Allocator.Persistent);
			this._nodesSection = (byte*)(this._rootNodeHeader + 1);
			this._reader.Position = fileHeader.NodesSectionOffset;
			this._reader.ReadBytes((void*)this._nodesSection, fileHeader.NodesSectionSize);
			UnsafeUtility.MemClear((void*)this._rootNodeHeader, (long)num);
			this._rootNodeHeader->NodeTypeHash = 0UL;
			this._rootNodeHeader->Size = num;
			this._rootNodeHeader->ChildrenCount = fileHeader.FirstLevelNodesCount;
			this._rootNodeHeader->NextSiblingOffset = -1;
			this._rootNodeHeader->DataStartingOffset = -1L;
			this._rootNodeHeader->MetadataStartingOffset = -1L;
			this._metadataSection = (byte*)Memory.Unmanaged.Allocate((long)fileHeader.MetadataSectionSize, 16, Allocator.Persistent);
			this._reader.Position = fileHeader.MetadataSectionOffset;
			this._reader.ReadBytes((void*)this._metadataSection, fileHeader.MetadataSectionSize);
		}

		// Token: 0x060017CD RID: 6093 RVA: 0x000596C1 File Offset: 0x000578C1
		internal unsafe DotsSerializationReader(ref DotsSerialization.BlobHeader blobHeader)
		{
			this._rootNodeHeader = (DotsSerialization.NodeHeader*)blobHeader.NodeSection.GetUnsafePtr();
			this._nodesSection = (byte*)(this._rootNodeHeader + 1);
			this._metadataSection = (byte*)blobHeader.MetadataSection.GetUnsafePtr();
		}

		// Token: 0x17000210 RID: 528
		// (get) Token: 0x060017CE RID: 6094 RVA: 0x000596FE File Offset: 0x000578FE
		public DotsSerializationReader.NodeHandle RootNode
		{
			get
			{
				return new DotsSerializationReader.NodeHandle(this, -UnsafeUtility.SizeOf<DotsSerialization.NodeHeader>());
			}
		}

		// Token: 0x17000211 RID: 529
		// (get) Token: 0x060017CF RID: 6095 RVA: 0x0005970C File Offset: 0x0005790C
		public BinaryReader Reader
		{
			get
			{
				return this._reader;
			}
		}

		// Token: 0x060017D0 RID: 6096 RVA: 0x00059714 File Offset: 0x00057914
		public void Dispose()
		{
			Memory.Unmanaged.Free<DotsSerialization.NodeHeader>(this._rootNodeHeader, Allocator.Persistent);
			Memory.Unmanaged.Free<byte>(this._metadataSection, Allocator.Persistent);
		}

		// Token: 0x060017D1 RID: 6097 RVA: 0x00059738 File Offset: 0x00057938
		private unsafe bool ReadData(DotsSerializationReader.NodeHandle nodeHandle, byte* buffer, long startingOffset, long readSize)
		{
			if (readSize == -1L)
			{
				readSize = nodeHandle.DataSize;
			}
			if (startingOffset + readSize > nodeHandle.DataSize)
			{
				return false;
			}
			ref DotsSerialization.NodeHeader asNodeHeader = ref nodeHandle.AsNodeHeader;
			this._reader.Position = asNodeHeader.DataStartingOffset + startingOffset;
			this._reader.ReadBytes((void*)buffer, (int)readSize);
			return true;
		}

		// Token: 0x060017D2 RID: 6098 RVA: 0x00059790 File Offset: 0x00057990
		private unsafe BlobAssetReference<T> GetMetadata<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(DotsSerializationReader.NodeHandle nodeHandle) where T : struct, ValueType
		{
			ref DotsSerialization.NodeHeader asNodeHeader = ref nodeHandle.AsNodeHeader;
			if (!asNodeHeader.HasMetadata)
			{
				return default(BlobAssetReference<T>);
			}
			byte* ptr = this._metadataSection + asNodeHeader.MetadataStartingOffset;
			BlobAssetHeader* ptr2 = (BlobAssetHeader*)ptr;
			ptr2->Allocator = Allocator.None;
			ptr2->ValidationPtr = (void*)(ptr + sizeof(BlobAssetHeader));
			BlobAssetReference<T> blobAssetReference;
			blobAssetReference.m_data.m_Align8Union = 0L;
			blobAssetReference.m_data.m_Ptr = ptr + sizeof(BlobAssetHeader);
			return blobAssetReference;
		}

		// Token: 0x04000914 RID: 2324
		private readonly BinaryReader _reader;

		// Token: 0x04000915 RID: 2325
		private unsafe readonly DotsSerialization.NodeHeader* _rootNodeHeader;

		// Token: 0x04000916 RID: 2326
		private unsafe readonly byte* _nodesSection;

		// Token: 0x04000917 RID: 2327
		private unsafe readonly byte* _metadataSection;

		// Token: 0x02000380 RID: 896
		public struct ReaderHandle : IDisposable
		{
			// Token: 0x060017D3 RID: 6099 RVA: 0x00059808 File Offset: 0x00057A08
			internal unsafe ReaderHandle(DotsSerializationReader.NodeHandle node)
			{
				this._buffer = (byte*)Memory.Unmanaged.Allocate(node.DataSize, 16, Allocator.TempJob);
				node.ReadData(this._buffer, 0L, -1L);
				this._reader = new MemoryBinaryReader(this._buffer, node.DataSize);
			}

			// Token: 0x060017D4 RID: 6100 RVA: 0x00059859 File Offset: 0x00057A59
			public void Dispose()
			{
				Memory.Unmanaged.Free<byte>(this._buffer, Allocator.TempJob);
				this._reader.Dispose();
			}

			// Token: 0x17000212 RID: 530
			// (get) Token: 0x060017D5 RID: 6101 RVA: 0x00059877 File Offset: 0x00057A77
			public BinaryReader Reader
			{
				get
				{
					return this._reader;
				}
			}

			// Token: 0x04000918 RID: 2328
			private unsafe readonly byte* _buffer;

			// Token: 0x04000919 RID: 2329
			private readonly MemoryBinaryReader _reader;
		}

		// Token: 0x02000381 RID: 897
		public struct NodeHandle
		{
			// Token: 0x17000213 RID: 531
			// (get) Token: 0x060017D6 RID: 6102 RVA: 0x0005987F File Offset: 0x00057A7F
			private unsafe byte* SectionBaseAddress
			{
				get
				{
					return this._owner._nodesSection;
				}
			}

			// Token: 0x060017D7 RID: 6103 RVA: 0x0005988C File Offset: 0x00057A8C
			private unsafe ref DotsSerialization.NodeHeader GetHeader(int offset)
			{
				return UnsafeUtility.AsRef<DotsSerialization.NodeHeader>((void*)(this.SectionBaseAddress + offset));
			}

			// Token: 0x17000214 RID: 532
			// (get) Token: 0x060017D8 RID: 6104 RVA: 0x0005989B File Offset: 0x00057A9B
			public ref DotsSerialization.NodeHeader AsNodeHeader
			{
				get
				{
					return this.GetHeader(this._nodeHeaderOffset);
				}
			}

			// Token: 0x17000215 RID: 533
			// (get) Token: 0x060017D9 RID: 6105 RVA: 0x000598A9 File Offset: 0x00057AA9
			public bool IsValid
			{
				get
				{
					return this._owner != null;
				}
			}

			// Token: 0x17000216 RID: 534
			// (get) Token: 0x060017DA RID: 6106 RVA: 0x000598B4 File Offset: 0x00057AB4
			public int ChildrenCount
			{
				get
				{
					return this.AsNodeHeader.ChildrenCount;
				}
			}

			// Token: 0x17000217 RID: 535
			// (get) Token: 0x060017DB RID: 6107 RVA: 0x000598C1 File Offset: 0x00057AC1
			public ulong NodeTypeHash
			{
				get
				{
					return this.AsNodeHeader.NodeTypeHash;
				}
			}

			// Token: 0x17000218 RID: 536
			// (get) Token: 0x060017DC RID: 6108 RVA: 0x000598CE File Offset: 0x00057ACE
			public Type NodeDotNetType
			{
				get
				{
					return TypeManager.GetType(TypeManager.GetTypeIndexFromStableTypeHash(this.AsNodeHeader.NodeTypeHash));
				}
			}

			// Token: 0x060017DD RID: 6109 RVA: 0x000598E5 File Offset: 0x00057AE5
			public unsafe ref T As<[global::System.Runtime.CompilerServices.IsUnmanaged] T>() where T : struct, ValueType
			{
				return UnsafeUtility.AsRef<T>((void*)(this.SectionBaseAddress + this._nodeHeaderOffset));
			}

			// Token: 0x060017DE RID: 6110 RVA: 0x000598FC File Offset: 0x00057AFC
			public bool MoveToNextChild(ref DotsSerializationReader.NodeHandle node)
			{
				if (!node.IsValid)
				{
					if (this.AsNodeHeader.ChildrenCount == 0)
					{
						return false;
					}
					node = new DotsSerializationReader.NodeHandle(this._owner, this._nodeHeaderOffset + this.AsNodeHeader.Size);
					return true;
				}
				else
				{
					int nextSiblingOffset = node.AsNodeHeader.NextSiblingOffset;
					if ((long)nextSiblingOffset == -1L)
					{
						node = default(DotsSerializationReader.NodeHandle);
						return false;
					}
					node = new DotsSerializationReader.NodeHandle(this._owner, nextSiblingOffset);
					return true;
				}
			}

			// Token: 0x060017DF RID: 6111 RVA: 0x00059973 File Offset: 0x00057B73
			public DotsSerializationReader.NodeHandle FindNode<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(int nestedLevel = 2147483647) where T : struct, ValueType
			{
				return this.FindByType(TypeManager.GetTypeInfo<T>().StableTypeHash, nestedLevel);
			}

			// Token: 0x060017E0 RID: 6112 RVA: 0x00059986 File Offset: 0x00057B86
			public DotsSerializationReader.ReaderHandle GetReaderHandle()
			{
				return new DotsSerializationReader.ReaderHandle(this);
			}

			// Token: 0x060017E1 RID: 6113 RVA: 0x00059994 File Offset: 0x00057B94
			internal DotsSerializationReader.NodeHandle.PrefetchState PrefetchRawDataRead(out ReadCommand readCommand)
			{
				DotsSerializationReader.NodeHandle.PrefetchState prefetchState = default(DotsSerializationReader.NodeHandle.PrefetchState);
				prefetchState._buffer = Memory.Unmanaged.Allocate(this.DataSize, 16, Allocator.Persistent);
				prefetchState._size = this.DataSize;
				ref DotsSerialization.NodeHeader asNodeHeader = ref this.AsNodeHeader;
				readCommand.Buffer = prefetchState._buffer;
				readCommand.Offset = asNodeHeader.DataStartingOffset;
				readCommand.Size = prefetchState._size;
				return prefetchState;
			}

			// Token: 0x060017E2 RID: 6114 RVA: 0x000599FC File Offset: 0x00057BFC
			private DotsSerializationReader.NodeHandle FindByType(ulong nodeType, int nestedLevel)
			{
				DotsSerializationReader.NodeHandle nodeHandle = default(DotsSerializationReader.NodeHandle);
				if (nestedLevel < 0)
				{
					return nodeHandle;
				}
				while (this.MoveToNextChild(ref nodeHandle))
				{
					if (nodeHandle.AsNodeHeader.NodeTypeHash == nodeType)
					{
						return new DotsSerializationReader.NodeHandle(this._owner, nodeHandle._nodeHeaderOffset);
					}
				}
				if (nestedLevel == 0)
				{
					return default(DotsSerializationReader.NodeHandle);
				}
				nodeHandle = default(DotsSerializationReader.NodeHandle);
				while (this.MoveToNextChild(ref nodeHandle))
				{
					DotsSerializationReader.NodeHandle nodeHandle2 = nodeHandle.FindByType(nodeType, nestedLevel - 1);
					if (nodeHandle2.IsValid)
					{
						return nodeHandle2;
					}
				}
				return default(DotsSerializationReader.NodeHandle);
			}

			// Token: 0x060017E3 RID: 6115 RVA: 0x00059A83 File Offset: 0x00057C83
			public bool TryFindNode(Hash128 id, out DotsSerializationReader.NodeHandle node, int nestedLevel = 2147483647)
			{
				node = this.FindNode(id, nestedLevel);
				return node.IsValid;
			}

			// Token: 0x060017E4 RID: 6116 RVA: 0x00059A9C File Offset: 0x00057C9C
			public DotsSerializationReader.NodeHandle FindNode(Hash128 id, int nestedLevel = 2147483647)
			{
				DotsSerializationReader.NodeHandle nodeHandle = default(DotsSerializationReader.NodeHandle);
				if (nestedLevel <= 0)
				{
					return nodeHandle;
				}
				while (this.MoveToNextChild(ref nodeHandle))
				{
					if (nodeHandle.AsNodeHeader.Id == id)
					{
						return new DotsSerializationReader.NodeHandle(this._owner, nodeHandle._nodeHeaderOffset);
					}
				}
				if (nestedLevel == 1)
				{
					return default(DotsSerializationReader.NodeHandle);
				}
				nodeHandle = default(DotsSerializationReader.NodeHandle);
				while (this.MoveToNextChild(ref nodeHandle))
				{
					DotsSerializationReader.NodeHandle nodeHandle2 = nodeHandle.FindNode(id, nestedLevel - 1);
					if (nodeHandle2.IsValid)
					{
						return nodeHandle2;
					}
				}
				return default(DotsSerializationReader.NodeHandle);
			}

			// Token: 0x060017E5 RID: 6117 RVA: 0x00059B29 File Offset: 0x00057D29
			internal NodeHandle(DotsSerializationReader owner, int nodeHeaderOffset)
			{
				this._owner = owner;
				this._nodeHeaderOffset = nodeHeaderOffset;
			}

			// Token: 0x060017E6 RID: 6118 RVA: 0x00059B39 File Offset: 0x00057D39
			public BlobAssetReference<T> GetMetadata<[global::System.Runtime.CompilerServices.IsUnmanaged] T>() where T : struct, ValueType
			{
				return this._owner.GetMetadata<T>(this);
			}

			// Token: 0x17000219 RID: 537
			// (get) Token: 0x060017E7 RID: 6119 RVA: 0x00059B4C File Offset: 0x00057D4C
			public bool HasMetadata
			{
				get
				{
					return this.AsNodeHeader.MetadataStartingOffset != -1L;
				}
			}

			// Token: 0x1700021A RID: 538
			// (get) Token: 0x060017E8 RID: 6120 RVA: 0x00059B60 File Offset: 0x00057D60
			public long DataStartingOffset
			{
				get
				{
					return this.AsNodeHeader.DataStartingOffset;
				}
			}

			// Token: 0x1700021B RID: 539
			// (get) Token: 0x060017E9 RID: 6121 RVA: 0x00059B6D File Offset: 0x00057D6D
			public long DataSize
			{
				get
				{
					return this.AsNodeHeader.DataSize;
				}
			}

			// Token: 0x060017EA RID: 6122 RVA: 0x00059B7A File Offset: 0x00057D7A
			public unsafe bool ReadData(byte* buffer, long startingOffset = 0L, long readSize = -1L)
			{
				return this._owner.ReadData(this, buffer, startingOffset, readSize);
			}

			// Token: 0x0400091A RID: 2330
			private readonly DotsSerializationReader _owner;

			// Token: 0x0400091B RID: 2331
			private readonly int _nodeHeaderOffset;

			// Token: 0x02000382 RID: 898
			public struct PrefetchState : IDisposable
			{
				// Token: 0x060017EB RID: 6123 RVA: 0x00059B90 File Offset: 0x00057D90
				public void Dispose()
				{
					Memory.Unmanaged.Free(this._buffer, Allocator.Persistent);
					this._buffer = null;
				}

				// Token: 0x060017EC RID: 6124 RVA: 0x00059BAB File Offset: 0x00057DAB
				public unsafe MemoryBinaryReader CreateStream()
				{
					return new MemoryBinaryReader((byte*)this._buffer, this._size);
				}

				// Token: 0x0400091C RID: 2332
				[NativeDisableUnsafePtrRestriction]
				internal unsafe void* _buffer;

				// Token: 0x0400091D RID: 2333
				internal long _size;
			}
		}
	}
}
