using System;
using System.Runtime.CompilerServices;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;

namespace Unity.Serialization.Json
{
	// Token: 0x02000050 RID: 80
	public struct PackedBinaryStream : IDisposable, IEquatable<PackedBinaryStream>
	{
		// Token: 0x060001C2 RID: 450 RVA: 0x00008122 File Offset: 0x00006322
		internal unsafe UnsafePackedBinaryStream* GetUnsafePtr()
		{
			return this.m_Data;
		}

		// Token: 0x17000036 RID: 54
		// (get) Token: 0x060001C3 RID: 451 RVA: 0x0000812A File Offset: 0x0000632A
		internal unsafe int TokenNextIndex
		{
			get
			{
				return this.m_Data->TokenNextIndex;
			}
		}

		// Token: 0x060001C4 RID: 452 RVA: 0x00008137 File Offset: 0x00006337
		public PackedBinaryStream(Allocator label)
		{
			this = new PackedBinaryStream(1024, 4096, label);
		}

		// Token: 0x060001C5 RID: 453 RVA: 0x0000814A File Offset: 0x0000634A
		public unsafe PackedBinaryStream(int initialTokensCapacity, int initialBufferCapacity, Allocator label)
		{
			this.m_Label = label;
			this.m_Data = (UnsafePackedBinaryStream*)UnsafeUtility.Malloc((long)sizeof(UnsafePackedBinaryStream), UnsafeUtility.AlignOf<UnsafePackedBinaryStream>(), this.m_Label);
			*this.m_Data = new UnsafePackedBinaryStream(initialTokensCapacity, initialBufferCapacity, this.m_Label);
		}

		// Token: 0x060001C6 RID: 454 RVA: 0x00008188 File Offset: 0x00006388
		public unsafe void Dispose()
		{
			this.m_Data->Dispose();
			UnsafeUtility.Free((void*)this.m_Data, this.m_Label);
			this.m_Data = null;
		}

		// Token: 0x060001C7 RID: 455 RVA: 0x000081AE File Offset: 0x000063AE
		internal unsafe bool IsValid(Handle handle)
		{
			return this.m_Data->IsValid(handle);
		}

		// Token: 0x060001C8 RID: 456 RVA: 0x000081BC File Offset: 0x000063BC
		internal unsafe BinaryToken GetToken(int tokenIndex)
		{
			return this.m_Data->GetToken(tokenIndex);
		}

		// Token: 0x060001C9 RID: 457 RVA: 0x000081CA File Offset: 0x000063CA
		internal unsafe BinaryToken GetToken(Handle handle)
		{
			return this.m_Data->GetToken(handle);
		}

		// Token: 0x060001CA RID: 458 RVA: 0x000081D8 File Offset: 0x000063D8
		internal unsafe int GetTokenIndex(Handle handle)
		{
			return this.m_Data->GetTokenIndex(handle);
		}

		// Token: 0x060001CB RID: 459 RVA: 0x000081E6 File Offset: 0x000063E6
		internal unsafe Handle GetHandle(int tokenIndex)
		{
			return this.m_Data->GetHandle(tokenIndex);
		}

		// Token: 0x060001CC RID: 460 RVA: 0x000081F4 File Offset: 0x000063F4
		internal unsafe Handle GetHandle(BinaryToken token)
		{
			return this.m_Data->GetHandle(token);
		}

		// Token: 0x060001CD RID: 461 RVA: 0x00008202 File Offset: 0x00006402
		internal unsafe Handle GetFirstChild(Handle handle)
		{
			return this.m_Data->GetFirstChild(handle);
		}

		// Token: 0x060001CE RID: 462 RVA: 0x00008210 File Offset: 0x00006410
		internal unsafe T* GetBufferPtr<[IsUnmanaged] T>(Handle handle) where T : struct, ValueType
		{
			return this.m_Data->GetBufferPtr<T>(handle);
		}

		// Token: 0x060001CF RID: 463 RVA: 0x0000821E File Offset: 0x0000641E
		internal unsafe void EnsureTokenCapacity(int newLength)
		{
			this.m_Data->EnsureTokenCapacity(newLength);
		}

		// Token: 0x060001D0 RID: 464 RVA: 0x0000822C File Offset: 0x0000642C
		internal unsafe void EnsureBufferCapacity(int length)
		{
			this.m_Data->EnsureBufferCapacity(length);
		}

		// Token: 0x060001D1 RID: 465 RVA: 0x0000823C File Offset: 0x0000643C
		internal unsafe SerializedValueView GetView(int tokenIndex)
		{
			BinaryToken binaryToken = this.m_Data->Tokens[tokenIndex];
			HandleData handleData = this.m_Data->Handles[binaryToken.HandleIndex];
			return new SerializedValueView(this.m_Data, new Handle
			{
				Index = binaryToken.HandleIndex,
				Version = handleData.DataVersion
			});
		}

		// Token: 0x060001D2 RID: 466 RVA: 0x000082B2 File Offset: 0x000064B2
		public unsafe void Clear()
		{
			this.m_Data->Clear();
		}

		// Token: 0x060001D3 RID: 467 RVA: 0x000082BF File Offset: 0x000064BF
		internal unsafe void DiscardCompleted()
		{
			this.m_Data->DiscardCompleted();
		}

		// Token: 0x060001D4 RID: 468 RVA: 0x000082CD File Offset: 0x000064CD
		public bool Equals(PackedBinaryStream other)
		{
			return this.m_Data == other.m_Data;
		}

		// Token: 0x060001D5 RID: 469 RVA: 0x000082E0 File Offset: 0x000064E0
		public override bool Equals(object obj)
		{
			if (obj is PackedBinaryStream)
			{
				PackedBinaryStream packedBinaryStream = (PackedBinaryStream)obj;
				return this.Equals(packedBinaryStream);
			}
			return false;
		}

		// Token: 0x060001D6 RID: 470 RVA: 0x00008305 File Offset: 0x00006505
		public override int GetHashCode()
		{
			return (int)((this.m_Label * (Allocator)397) ^ this.m_Data);
		}

		// Token: 0x040000E7 RID: 231
		private const int k_DefaultBufferCapacity = 4096;

		// Token: 0x040000E8 RID: 232
		private const int k_DefaultTokenCapacity = 1024;

		// Token: 0x040000E9 RID: 233
		private readonly Allocator m_Label;

		// Token: 0x040000EA RID: 234
		[NativeDisableUnsafePtrRestriction]
		private unsafe UnsafePackedBinaryStream* m_Data;
	}
}
