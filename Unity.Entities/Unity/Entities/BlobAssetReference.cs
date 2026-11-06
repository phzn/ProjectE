using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Entities.DebugProxies;
using Unity.Entities.Serialization;
using Unity.Mathematics;
using Unity.Properties;

namespace Unity.Entities
{
	// Token: 0x02000024 RID: 36
	[ChunkSerializable]
	[DebuggerTypeProxy(typeof(BlobAssetReferenceProxy<>))]
	[DebuggerDisplay("BlobAssetReference")]
	public struct BlobAssetReference<[global::System.Runtime.CompilerServices.IsUnmanaged] T> : IDisposable, IEquatable<BlobAssetReference<T>> where T : struct, ValueType
	{
		// Token: 0x1700000D RID: 13
		// (get) Token: 0x0600008A RID: 138 RVA: 0x00003164 File Offset: 0x00001364
		public bool IsCreated
		{
			get
			{
				return this.m_data.m_Ptr != null;
			}
		}

		// Token: 0x0600008B RID: 139 RVA: 0x00003178 File Offset: 0x00001378
		public unsafe void* GetUnsafePtr()
		{
			return (void*)this.m_data.m_Ptr;
		}

		// Token: 0x0600008C RID: 140 RVA: 0x00003185 File Offset: 0x00001385
		public void Dispose()
		{
			this.m_data.Dispose();
		}

		// Token: 0x1700000E RID: 14
		// (get) Token: 0x0600008D RID: 141 RVA: 0x00003192 File Offset: 0x00001392
		public unsafe ref T Value
		{
			get
			{
				return UnsafeUtility.AsRef<T>((void*)this.m_data.m_Ptr);
			}
		}

		// Token: 0x0600008E RID: 142 RVA: 0x000031A4 File Offset: 0x000013A4
		public unsafe static BlobAssetReference<T> Create(void* ptr, int length)
		{
			byte* ptr2 = (byte*)Memory.Unmanaged.Allocate((long)(sizeof(BlobAssetHeader) + length), 16, Allocator.Persistent);
			UnsafeUtility.MemCpy((void*)(ptr2 + sizeof(BlobAssetHeader)), ptr, (long)length);
			BlobAssetHeader* ptr3 = (BlobAssetHeader*)ptr2;
			*ptr3 = default(BlobAssetHeader);
			ptr3->Length = length;
			ptr3->Allocator = Allocator.Persistent;
			ptr3->Hash = (ulong)math.hash(ptr, length, 0U);
			BlobAssetReference<T> blobAssetReference;
			blobAssetReference.m_data.m_Align8Union = 0L;
			ptr3->ValidationPtr = (void*)(blobAssetReference.m_data.m_Ptr = ptr2 + sizeof(BlobAssetHeader));
			return blobAssetReference;
		}

		// Token: 0x0600008F RID: 143 RVA: 0x00003230 File Offset: 0x00001430
		public unsafe static BlobAssetReference<T> Create(byte[] data)
		{
			fixed (byte* ptr = &data[0])
			{
				return BlobAssetReference<T>.Create((void*)ptr, data.Length);
			}
		}

		// Token: 0x06000090 RID: 144 RVA: 0x0000324F File Offset: 0x0000144F
		public static BlobAssetReference<T> Create(T value)
		{
			return BlobAssetReference<T>.Create(UnsafeUtility.AddressOf<T>(ref value), UnsafeUtility.SizeOf<T>());
		}

		// Token: 0x06000091 RID: 145 RVA: 0x00003264 File Offset: 0x00001464
		internal static BlobAssetReference<T> Create(BlobAssetReferenceData blobData)
		{
			return new BlobAssetReference<T>
			{
				m_data = blobData
			};
		}

		// Token: 0x06000092 RID: 146 RVA: 0x00003282 File Offset: 0x00001482
		public static bool TryRead<U>(U binaryReader, int version, out BlobAssetReference<T> result) where U : BinaryReader
		{
			if (binaryReader.ReadInt() != version)
			{
				result = default(BlobAssetReference<T>);
				return false;
			}
			result = binaryReader.Read<T>();
			return true;
		}

		// Token: 0x06000093 RID: 147 RVA: 0x000032B0 File Offset: 0x000014B0
		public static bool TryRead(string path, int version, out BlobAssetReference<T> result)
		{
			if (string.IsNullOrEmpty(path))
			{
				result = default(BlobAssetReference<T>);
				return false;
			}
			bool flag;
			using (StreamBinaryReader streamBinaryReader = new StreamBinaryReader(path, (long)(UnsafeUtility.SizeOf<T>() + 4)))
			{
				flag = BlobAssetReference<T>.TryRead<StreamBinaryReader>(streamBinaryReader, version, out result);
			}
			return flag;
		}

		// Token: 0x06000094 RID: 148 RVA: 0x00003304 File Offset: 0x00001504
		internal unsafe static bool TryReadInplace(byte* data, long size, int version, out BlobAssetReference<T> result, out int numBytesRead)
		{
			result = default(BlobAssetReference<T>);
			numBytesRead = 0;
			if (size < (long)(4 + sizeof(BlobAssetHeader)))
			{
				return false;
			}
			if (*(int*)data != version)
			{
				return false;
			}
			ref BlobAssetHeader ptr = ref *(BlobAssetHeader*)(data + 4);
			if (size < (long)(4 + sizeof(BlobAssetHeader) + ptr.Length))
			{
				return false;
			}
			numBytesRead = 4 + sizeof(BlobAssetHeader) + ptr.Length;
			byte* ptr2 = data + 4;
			ptr.ValidationPtr = (void*)(ptr2 + sizeof(BlobAssetHeader));
			BlobAssetReference<T> blobAssetReference;
			blobAssetReference.m_data.m_Align8Union = 0L;
			blobAssetReference.m_data.m_Ptr = ptr2 + sizeof(BlobAssetHeader);
			result = blobAssetReference;
			return true;
		}

		// Token: 0x06000095 RID: 149 RVA: 0x00003398 File Offset: 0x00001598
		public static void Write<U>(U writer, BlobBuilder builder, int version) where U : BinaryWriter
		{
			using (BlobAssetReference<T> blobAssetReference = builder.CreateBlobAssetReference<T>(Allocator.TempJob))
			{
				writer.Write(version);
				writer.Write<T>(blobAssetReference);
			}
		}

		// Token: 0x06000096 RID: 150 RVA: 0x000033EC File Offset: 0x000015EC
		public static void Write(BlobBuilder builder, string path, int version)
		{
			using (StreamBinaryWriter streamBinaryWriter = new StreamBinaryWriter(path, 65536))
			{
				BlobAssetReference<T>.Write<StreamBinaryWriter>(streamBinaryWriter, builder, version);
			}
		}

		// Token: 0x1700000F RID: 15
		// (get) Token: 0x06000097 RID: 151 RVA: 0x0000342C File Offset: 0x0000162C
		public static BlobAssetReference<T> Null
		{
			get
			{
				return default(BlobAssetReference<T>);
			}
		}

		// Token: 0x06000098 RID: 152 RVA: 0x00003442 File Offset: 0x00001642
		public static bool operator ==(BlobAssetReference<T> lhs, BlobAssetReference<T> rhs)
		{
			return lhs.m_data.m_Ptr == rhs.m_data.m_Ptr;
		}

		// Token: 0x06000099 RID: 153 RVA: 0x0000345C File Offset: 0x0000165C
		public static bool operator !=(BlobAssetReference<T> lhs, BlobAssetReference<T> rhs)
		{
			return lhs.m_data.m_Ptr != rhs.m_data.m_Ptr;
		}

		// Token: 0x0600009A RID: 154 RVA: 0x00003479 File Offset: 0x00001679
		public bool Equals(BlobAssetReference<T> other)
		{
			return this.m_data.Equals(other.m_data);
		}

		// Token: 0x0600009B RID: 155 RVA: 0x0000348C File Offset: 0x0000168C
		public override bool Equals(object obj)
		{
			return this == (BlobAssetReference<T>)obj;
		}

		// Token: 0x0600009C RID: 156 RVA: 0x0000349F File Offset: 0x0000169F
		public override int GetHashCode()
		{
			return this.m_data.GetHashCode();
		}

		// Token: 0x0600009D RID: 157 RVA: 0x000034B2 File Offset: 0x000016B2
		internal BlobAssetPtr ToBlobAssetPtr()
		{
			return new BlobAssetPtr(this.m_data.Header);
		}

		// Token: 0x04000033 RID: 51
		[CreateProperty]
		internal BlobAssetReferenceData m_data;
	}
}
