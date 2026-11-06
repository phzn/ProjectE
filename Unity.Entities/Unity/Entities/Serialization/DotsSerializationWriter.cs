using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;

namespace Unity.Entities.Serialization
{
	// Token: 0x02000378 RID: 888
	internal class DotsSerializationWriter : IDisposable
	{
		// Token: 0x0600179D RID: 6045 RVA: 0x00058728 File Offset: 0x00056928
		public void Dispose()
		{
			if (this._headerWritten == 0)
			{
				this.WriteHeader();
				this._headerWritten = 1;
			}
			if (this._isDisposed == 1)
			{
				return;
			}
			Memory.Unmanaged.Free<DotsSerialization.NodeHeader>(this._rootNodeHeader, Allocator.Persistent);
			this._metadataSection.Dispose();
			this._nodesAllocation.Dispose();
			this._nodesStack.Dispose();
			this._isDisposed = 1;
		}

		// Token: 0x0600179E RID: 6046 RVA: 0x00058790 File Offset: 0x00056990
		public unsafe void WriteHeader()
		{
			long position = this._writer.Position;
			this._header.FirstLevelNodesCount = this._rootNode.NodeHeader.ChildrenCount;
			this._header.DataSectionSize = position - this._header.DataSectionOffset;
			this._header.MetadataSectionOffset = position;
			this._header.MetadataSectionSize = this._metadataSection.Length;
			this._header.NodesSectionOffset = this._header.MetadataSectionOffset + (long)this._header.MetadataSectionSize;
			this._header.NodesSectionSize = this._nodesAllocation.CurrentGlobalOffset;
			this.WriteHeaderToFile();
			this._writer.WriteBytes((void*)this._metadataSection.Ptr, this._metadataSection.Length);
			ref UnsafeList<PagedAllocation.PageInfo> pages = ref this._nodesAllocation.Pages;
			for (int i = 0; i < pages.Length; i++)
			{
				this._writer.WriteBytes((void*)pages[i].Buffer, pages[i].FreeOffset);
			}
		}

		// Token: 0x0600179F RID: 6047 RVA: 0x000588A0 File Offset: 0x00056AA0
		public unsafe BlobAssetReference<DotsSerialization.BlobHeader> WriteHeaderAndBlob()
		{
			BlobBuilder blobBuilder = new BlobBuilder(Allocator.Persistent, 65536);
			ref DotsSerialization.BlobHeader ptr = ref blobBuilder.ConstructRoot<DotsSerialization.BlobHeader>();
			long position = this._writer.Position;
			this._header.FirstLevelNodesCount = this._rootNode.NodeHeader.ChildrenCount;
			this._header.DataSectionSize = position - this._header.DataSectionOffset;
			this._header.MetadataSectionOffset = position;
			this._header.MetadataSectionSize = this._metadataSection.Length;
			this._header.NodesSectionOffset = this._header.MetadataSectionOffset + (long)this._header.MetadataSectionSize;
			this._header.NodesSectionSize = this._nodesAllocation.CurrentGlobalOffset;
			ptr.FileHeader = this._header;
			this.WriteHeaderToFile();
			this._writer.WriteBytes((void*)this._metadataSection.Ptr, this._metadataSection.Length);
			UnsafeUtility.MemCpy(blobBuilder.Allocate<byte>(ref ptr.MetadataSection, this._metadataSection.Length).GetUnsafePtr(), (void*)this._metadataSection.Ptr, (long)this._metadataSection.Length);
			int num = UnsafeUtility.SizeOf<DotsSerialization.NodeHeader>();
			ref UnsafeList<PagedAllocation.PageInfo> pages = ref this._nodesAllocation.Pages;
			int num2 = num;
			for (int i = 0; i < pages.Length; i++)
			{
				num2 += pages[i].FreeOffset;
			}
			DotsSerialization.NodeHeader* unsafePtr = (DotsSerialization.NodeHeader*)blobBuilder.Allocate<byte>(ref ptr.NodeSection, num2).GetUnsafePtr();
			UnsafeUtility.MemClear((void*)unsafePtr, (long)num);
			unsafePtr->NodeTypeHash = 0UL;
			unsafePtr->Size = num;
			unsafePtr->ChildrenCount = this._rootNode.NodeHeader.ChildrenCount;
			unsafePtr->NextSiblingOffset = -1;
			unsafePtr->DataStartingOffset = -1L;
			unsafePtr->MetadataStartingOffset = -1L;
			byte* ptr2 = (byte*)(unsafePtr + 1);
			for (int j = 0; j < pages.Length; j++)
			{
				this._writer.WriteBytes((void*)pages[j].Buffer, pages[j].FreeOffset);
				UnsafeUtility.MemCpy((void*)ptr2, (void*)pages[j].Buffer, (long)pages[j].FreeOffset);
				ptr2 += this._metadataSection.Length;
			}
			BlobAssetReference<DotsSerialization.BlobHeader> blobAssetReference = blobBuilder.CreateBlobAssetReference<DotsSerialization.BlobHeader>(Allocator.Persistent);
			blobBuilder.Dispose();
			return blobAssetReference;
		}

		// Token: 0x060017A0 RID: 6048 RVA: 0x00058B08 File Offset: 0x00056D08
		public unsafe DotsSerializationWriter.NodeHandle<T> CreateNode<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(Hash128 id = default(Hash128)) where T : struct, ValueType, IComponentData
		{
			byte* ptr = (byte*)(void*)this._nodesStack[this._nodesStack.Length - 1];
			int num;
			byte* ptr2 = this.AllocateNodeData<T>(out num);
			DotsSerializationWriter.NodeHandle<T> nodeHandle = new DotsSerializationWriter.NodeHandle<T>(this, ptr2);
			TypeManager.TypeInfo typeInfo = *TypeManager.GetTypeInfo<T>();
			ref DotsSerialization.NodeHeader asNodeHeader = ref nodeHandle.AsNodeHeader;
			asNodeHeader.NodeTypeHash = typeInfo.StableTypeHash;
			asNodeHeader.Size = typeInfo.TypeSize;
			asNodeHeader.Id = id;
			asNodeHeader.NextSiblingOffset = -1;
			asNodeHeader.DataStartingOffset = -1L;
			asNodeHeader.MetadataStartingOffset = -1L;
			((DotsSerialization.NodeHeader*)ptr)->ChildrenCount++;
			this.SetNextSiblingOffset(ptr, ptr2, num);
			IntPtr intPtr = (IntPtr)((void*)ptr2);
			this._nodesStack.Add(in intPtr);
			this._trackedNodeCounter++;
			return nodeHandle;
		}

		// Token: 0x060017A1 RID: 6049 RVA: 0x00058BC8 File Offset: 0x00056DC8
		private unsafe int GetNodeTrackedSegmentDataWrite(byte* nodeHeaderAddress)
		{
			int num;
			if (!this._trackedSegmentDataWriteByNode.TryGetValue((IntPtr)((void*)nodeHeaderAddress), out num))
			{
				return -1;
			}
			return num;
		}

		// Token: 0x060017A2 RID: 6050 RVA: 0x00058BF0 File Offset: 0x00056DF0
		private unsafe void SetNodeTrackedSegmentDataWrite(byte* nodeHeaderAddress, int value)
		{
			IntPtr intPtr = (IntPtr)((void*)nodeHeaderAddress);
			if (!this._trackedSegmentDataWriteByNode.ContainsKey(intPtr))
			{
				this._trackedSegmentDataWriteByNode.Add(intPtr, value);
				return;
			}
			this._trackedSegmentDataWriteByNode[intPtr] = value;
		}

		// Token: 0x060017A3 RID: 6051 RVA: 0x00058C30 File Offset: 0x00056E30
		private unsafe bool SetNodeMetadata<[global::System.Runtime.CompilerServices.IsUnmanaged] T, [global::System.Runtime.CompilerServices.IsUnmanaged] TB>(DotsSerializationWriter.NodeHandle<T> nodeHandle, BlobAssetReference<TB> blobAssetReference) where T : struct, ValueType, IComponentData where TB : struct, ValueType
		{
			ref DotsSerialization.NodeHeader asNodeHeader = ref nodeHandle.AsNodeHeader;
			if (asNodeHeader.MetadataStartingOffset != -1L)
			{
				return false;
			}
			int length = blobAssetReference.m_data.Header->Length;
			BlobAssetHeader blobAssetHeader = BlobAssetHeader.CreateForSerialize(length, blobAssetReference.m_data.Header->Hash);
			asNodeHeader.MetadataStartingOffset = (long)this._metadataSection.Length;
			this._metadataSection.AddRange((void*)(&blobAssetHeader), sizeof(BlobAssetHeader));
			this._metadataSection.AddRange((void*)(blobAssetReference.m_data.Header + 1), length);
			return true;
		}

		// Token: 0x060017A4 RID: 6052 RVA: 0x00058CC1 File Offset: 0x00056EC1
		private long InitWriteData<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(DotsSerializationWriter.NodeHandle<T> nodeHandle) where T : struct, ValueType, IComponentData
		{
			ref DotsSerialization.NodeHeader asNodeHeader = ref nodeHandle.AsNodeHeader;
			nodeHandle.TrackedSegmentDataWrite = this._trackedNodeCounter;
			asNodeHeader.DataStartingOffset = this._writer.Position;
			asNodeHeader.DataSize = 0L;
			return this._writer.Position;
		}

		// Token: 0x060017A5 RID: 6053 RVA: 0x00058CFC File Offset: 0x00056EFC
		private void EndWriteData<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(DotsSerializationWriter.NodeHandle<T> nodeHandle, long startPosition) where T : struct, ValueType, IComponentData
		{
			ref DotsSerialization.NodeHeader asNodeHeader = ref nodeHandle.AsNodeHeader;
			if (nodeHandle.TrackedSegmentDataWrite != this._trackedNodeCounter)
			{
				throw new InvalidOperationException(string.Format("Can't write data for the node {0} before and after processing its children. You must pack your write before or after processing the children ", asNodeHeader.Id));
			}
			asNodeHeader.DataSize = this._writer.Position - startPosition;
		}

		// Token: 0x060017A6 RID: 6054 RVA: 0x00058D50 File Offset: 0x00056F50
		private unsafe void WriteData<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(DotsSerializationWriter.NodeHandle<T> nodeHandle, void* data, int dataLength) where T : struct, ValueType, IComponentData
		{
			ref DotsSerialization.NodeHeader asNodeHeader = ref nodeHandle.AsNodeHeader;
			if (nodeHandle.TrackedSegmentDataWrite == -1)
			{
				nodeHandle.TrackedSegmentDataWrite = this._trackedNodeCounter;
				asNodeHeader.DataStartingOffset = this._writer.Position;
				asNodeHeader.DataSize = 0L;
			}
			if (nodeHandle.TrackedSegmentDataWrite != this._trackedNodeCounter)
			{
				throw new InvalidOperationException(string.Format("Can't write data for the node {0} before and after processing its children. You must pack your write before or after processing the children ", asNodeHeader.Id));
			}
			this._writer.WriteBytes(data, dataLength);
			asNodeHeader.DataSize += (long)dataLength;
		}

		// Token: 0x060017A7 RID: 6055 RVA: 0x00058DD8 File Offset: 0x00056FD8
		private unsafe void SetNextSiblingOffset(byte* parentNode, byte* newChildNode, int newChildOffset)
		{
			IntPtr intPtr;
			this._currentLastChildOffset.TryGetValue((IntPtr)((void*)parentNode), out intPtr);
			this._currentLastChildOffset.Add((IntPtr)((void*)newChildNode), IntPtr.Zero);
			if (intPtr != IntPtr.Zero)
			{
				((DotsSerialization.NodeHeader*)(void*)intPtr)->NextSiblingOffset = newChildOffset;
			}
			this._currentLastChildOffset[(IntPtr)((void*)parentNode)] = (IntPtr)((void*)newChildNode);
		}

		// Token: 0x060017A8 RID: 6056 RVA: 0x00058E40 File Offset: 0x00057040
		private unsafe byte* AllocateNodeData<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(out int nodeOffset) where T : struct, ValueType, IComponentData
		{
			int num = UnsafeUtility.SizeOf<T>();
			nodeOffset = this._nodesAllocation.CurrentGlobalOffset;
			return this._nodesAllocation.Reserve(num, true);
		}

		// Token: 0x060017A9 RID: 6057 RVA: 0x00058E6D File Offset: 0x0005706D
		private unsafe void PopNode<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(ref DotsSerializationWriter.NodeHandle<T> nodeHandle) where T : struct, ValueType, IComponentData
		{
			this._currentLastChildOffset.Remove((IntPtr)((void*)nodeHandle.NodeHeaderAddress));
			this._nodesStack.RemoveAt(this._nodesStack.Length - 1);
		}

		// Token: 0x060017AA RID: 6058 RVA: 0x00058EA0 File Offset: 0x000570A0
		internal unsafe DotsSerializationWriter(BinaryWriter writer, Hash128 fileId, FixedString64Bytes fileType)
		{
			this._writer = writer;
			this._currentLastChildOffset = new Dictionary<IntPtr, IntPtr>();
			this._metadataSection = new UnsafeList<byte>(67108864, Allocator.Persistent, NativeArrayOptions.UninitializedMemory);
			this._nodesAllocation = new PagedAllocation(Allocator.Persistent, 65536);
			this._trackedNodeCounter = 0;
			this._trackedSegmentDataWriteByNode = new Dictionary<IntPtr, int>();
			this._rootNodeHeader = (DotsSerialization.NodeHeader*)Memory.Unmanaged.Allocate((long)sizeof(DotsSerialization.NodeHeader), 16, Allocator.Persistent);
			UnsafeUtility.MemClear((void*)this._rootNodeHeader, (long)sizeof(DotsSerialization.NodeHeader));
			this._rootNodeHeader->NodeTypeHash = 0UL;
			byte* rootNodeHeader = (byte*)this._rootNodeHeader;
			this._rootNode = new DotsSerializationWriter.NodeHandle<DotsSerialization.NodeHeader>(this, rootNodeHeader);
			this._currentLastChildOffset.Add((IntPtr)((void*)rootNodeHeader), IntPtr.Zero);
			this._nodesStack = new UnsafeList<IntPtr>(16, Allocator.Persistent, NativeArrayOptions.UninitializedMemory);
			IntPtr intPtr = (IntPtr)((void*)rootNodeHeader);
			this._nodesStack.Add(in intPtr);
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
			UnsafeUtility.MemCpy(UnsafeUtility.AddressOf<long>(ref this._header.MagicValue), (void*)ptr, (long)DotsSerialization.HeaderMagic.Length);
			array = null;
			this._header.FileVersion = 78;
			this._header.FileId = fileId;
			this._header.HeaderSize = UnsafeUtility.SizeOf<DotsSerialization.FileHeader>();
			this._header.FileType = fileType;
			this._header.DataSectionOffset = (long)this._header.HeaderSize;
			this._writer.Position = this._header.DataSectionOffset;
		}

		// Token: 0x060017AB RID: 6059 RVA: 0x00059030 File Offset: 0x00057230
		private void WriteHeaderToFile()
		{
			long position = this._writer.Position;
			this._writer.Position = 0L;
			this._writer.WriteBytes(UnsafeUtility.AddressOf<DotsSerialization.FileHeader>(ref this._header), UnsafeUtility.SizeOf<DotsSerialization.FileHeader>());
			this._writer.Position = position;
		}

		// Token: 0x040008FC RID: 2300
		private readonly BinaryWriter _writer;

		// Token: 0x040008FD RID: 2301
		private DotsSerialization.FileHeader _header;

		// Token: 0x040008FE RID: 2302
		private readonly PagedAllocation _nodesAllocation;

		// Token: 0x040008FF RID: 2303
		private UnsafeList<byte> _metadataSection;

		// Token: 0x04000900 RID: 2304
		private byte _isDisposed;

		// Token: 0x04000901 RID: 2305
		private byte _headerWritten;

		// Token: 0x04000902 RID: 2306
		private int _trackedNodeCounter;

		// Token: 0x04000903 RID: 2307
		private unsafe DotsSerialization.NodeHeader* _rootNodeHeader;

		// Token: 0x04000904 RID: 2308
		private DotsSerializationWriter.NodeHandle<DotsSerialization.NodeHeader> _rootNode;

		// Token: 0x04000905 RID: 2309
		private readonly Dictionary<IntPtr, IntPtr> _currentLastChildOffset;

		// Token: 0x04000906 RID: 2310
		private UnsafeList<IntPtr> _nodesStack;

		// Token: 0x04000907 RID: 2311
		private readonly Dictionary<IntPtr, int> _trackedSegmentDataWriteByNode;

		// Token: 0x02000379 RID: 889
		public struct WriterHandle<[global::System.Runtime.CompilerServices.IsUnmanaged] T> : IDisposable where T : struct, ValueType, IComponentData
		{
			// Token: 0x060017AC RID: 6060 RVA: 0x0005907D File Offset: 0x0005727D
			internal WriterHandle(DotsSerializationWriter owner, DotsSerializationWriter.NodeHandle<T> node)
			{
				this._owner = owner;
				this._node = node;
				this._startPosition = this._owner.InitWriteData<T>(node);
			}

			// Token: 0x060017AD RID: 6061 RVA: 0x0005909F File Offset: 0x0005729F
			public void Dispose()
			{
				this._owner.EndWriteData<T>(this._node, this._startPosition);
				this._owner = null;
			}

			// Token: 0x1700020A RID: 522
			// (get) Token: 0x060017AE RID: 6062 RVA: 0x000590BF File Offset: 0x000572BF
			public BinaryWriter Writer
			{
				get
				{
					return this._owner._writer;
				}
			}

			// Token: 0x04000908 RID: 2312
			private DotsSerializationWriter _owner;

			// Token: 0x04000909 RID: 2313
			private readonly DotsSerializationWriter.NodeHandle<T> _node;

			// Token: 0x0400090A RID: 2314
			private readonly long _startPosition;
		}

		// Token: 0x0200037A RID: 890
		public struct DeferredWriterHandle<[global::System.Runtime.CompilerServices.IsUnmanaged] T> : IDisposable where T : struct, ValueType, IComponentData
		{
			// Token: 0x060017AF RID: 6063 RVA: 0x000590CC File Offset: 0x000572CC
			internal DeferredWriterHandle(DotsSerializationWriter owner, DotsSerializationWriter.NodeHandle<T> node)
			{
				this._owner = owner;
				this._node = node;
				this._writer = new MemoryBinaryWriter();
			}

			// Token: 0x1700020B RID: 523
			// (get) Token: 0x060017B0 RID: 6064 RVA: 0x000590E7 File Offset: 0x000572E7
			public bool IsValid
			{
				get
				{
					return this._owner != null;
				}
			}

			// Token: 0x060017B1 RID: 6065 RVA: 0x000590F4 File Offset: 0x000572F4
			public void Dispose()
			{
				this._node.SubmitDeferredNodeData(this._writer.Data, this._writer.Length);
				this._writer.Dispose();
				this._owner = null;
			}

			// Token: 0x1700020C RID: 524
			// (get) Token: 0x060017B2 RID: 6066 RVA: 0x00059137 File Offset: 0x00057337
			public BinaryWriter Writer
			{
				get
				{
					return this._writer;
				}
			}

			// Token: 0x0400090B RID: 2315
			private DotsSerializationWriter _owner;

			// Token: 0x0400090C RID: 2316
			private readonly DotsSerializationWriter.NodeHandle<T> _node;

			// Token: 0x0400090D RID: 2317
			private readonly MemoryBinaryWriter _writer;
		}

		// Token: 0x0200037B RID: 891
		public struct NodeHandle<[global::System.Runtime.CompilerServices.IsUnmanaged] T> : IDisposable where T : struct, ValueType, IComponentData
		{
			// Token: 0x1700020D RID: 525
			// (get) Token: 0x060017B3 RID: 6067 RVA: 0x0005913F File Offset: 0x0005733F
			// (set) Token: 0x060017B4 RID: 6068 RVA: 0x00059152 File Offset: 0x00057352
			internal int TrackedSegmentDataWrite
			{
				get
				{
					return this._owner.GetNodeTrackedSegmentDataWrite(this.NodeHeaderAddress);
				}
				set
				{
					this._owner.SetNodeTrackedSegmentDataWrite(this.NodeHeaderAddress, value);
				}
			}

			// Token: 0x1700020E RID: 526
			// (get) Token: 0x060017B5 RID: 6069 RVA: 0x00059166 File Offset: 0x00057366
			public unsafe ref T NodeHeader
			{
				get
				{
					return UnsafeUtility.AsRef<T>((void*)this.NodeHeaderAddress);
				}
			}

			// Token: 0x1700020F RID: 527
			// (get) Token: 0x060017B6 RID: 6070 RVA: 0x00059173 File Offset: 0x00057373
			internal unsafe ref DotsSerialization.NodeHeader AsNodeHeader
			{
				get
				{
					return UnsafeUtility.AsRef<DotsSerialization.NodeHeader>((void*)this.NodeHeaderAddress);
				}
			}

			// Token: 0x060017B7 RID: 6071 RVA: 0x00059180 File Offset: 0x00057380
			internal unsafe NodeHandle(DotsSerializationWriter owner, byte* nodeHeaderAddress)
			{
				this._owner = owner;
				this.NodeHeaderAddress = nodeHeaderAddress;
				this.TrackedSegmentDataWrite = -1;
			}

			// Token: 0x060017B8 RID: 6072 RVA: 0x00059197 File Offset: 0x00057397
			public void Dispose()
			{
				this._owner.PopNode<T>(ref this);
			}

			// Token: 0x060017B9 RID: 6073 RVA: 0x000591A5 File Offset: 0x000573A5
			public bool SetMetadata<[global::System.Runtime.CompilerServices.IsUnmanaged] TB>(BlobAssetReference<TB> blobAssetReference) where TB : struct, ValueType
			{
				return this._owner.SetNodeMetadata<T, TB>(this, blobAssetReference);
			}

			// Token: 0x060017BA RID: 6074 RVA: 0x000591B9 File Offset: 0x000573B9
			public unsafe void WriteData(void* data, int dataLength)
			{
				this._owner.WriteData<T>(this, data, dataLength);
			}

			// Token: 0x060017BB RID: 6075 RVA: 0x000591CE File Offset: 0x000573CE
			public DotsSerializationWriter.WriterHandle<T> GetWriterHandle()
			{
				return new DotsSerializationWriter.WriterHandle<T>(this._owner, this);
			}

			// Token: 0x060017BC RID: 6076 RVA: 0x000591E1 File Offset: 0x000573E1
			public DotsSerializationWriter.DeferredWriterHandle<T> GetDeferredWriteHandle()
			{
				return new DotsSerializationWriter.DeferredWriterHandle<T>(this._owner, this);
			}

			// Token: 0x060017BD RID: 6077 RVA: 0x000591F4 File Offset: 0x000573F4
			internal unsafe void SubmitDeferredNodeData(byte* rawData, int rawDataLength)
			{
				this.WriteData((void*)rawData, rawDataLength);
			}

			// Token: 0x0400090E RID: 2318
			private readonly DotsSerializationWriter _owner;

			// Token: 0x0400090F RID: 2319
			internal unsafe readonly byte* NodeHeaderAddress;
		}
	}
}
