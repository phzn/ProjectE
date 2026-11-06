using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using Unity.Burst;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Properties;

namespace Unity.Entities
{
	// Token: 0x02000023 RID: 35
	[StructLayout(LayoutKind.Explicit, Size = 8)]
	internal struct BlobAssetReferenceData : IEquatable<BlobAssetReferenceData>
	{
		// Token: 0x1700000A RID: 10
		// (get) Token: 0x0600007A RID: 122 RVA: 0x00002F73 File Offset: 0x00001173
		internal unsafe BlobAssetHeader* Header
		{
			get
			{
				return (BlobAssetHeader*)(this.m_Ptr - sizeof(BlobAssetHeader));
			}
		}

		// Token: 0x1700000B RID: 11
		// (get) Token: 0x0600007B RID: 123 RVA: 0x00002F82 File Offset: 0x00001182
		// (set) Token: 0x0600007C RID: 124 RVA: 0x00002F8A File Offset: 0x0000118A
		[CreateProperty]
		private long SerializedHash
		{
			get
			{
				return this.m_Align8Union;
			}
			set
			{
				this.m_Align8Union = value;
			}
		}

		// Token: 0x0600007D RID: 125 RVA: 0x00002F94 File Offset: 0x00001194
		[BurstDiscard]
		private unsafe void ValidateNonBurst()
		{
			void* ptr = null;
			try
			{
				ptr = this.Header->ValidationPtr;
			}
			catch (Exception)
			{
			}
			if (ptr != (void*)this.m_Ptr && !this.IsProtected)
			{
				throw new InvalidOperationException("The BlobAssetReference is not valid. Likely it has already been unloaded or released.");
			}
		}

		// Token: 0x0600007E RID: 126 RVA: 0x00002FE4 File Offset: 0x000011E4
		[Conditional("ENABLE_UNITY_COLLECTIONS_CHECKS")]
		[Conditional("UNITY_DOTS_DEBUG")]
		private unsafe void ValidateBurst()
		{
			if (this.Header->ValidationPtr != (void*)this.m_Ptr && !this.IsProtected)
			{
				throw new InvalidOperationException("The BlobAssetReference is not valid. Likely it has already been unloaded or released.");
			}
		}

		// Token: 0x0600007F RID: 127 RVA: 0x0000300C File Offset: 0x0000120C
		[Conditional("ENABLE_UNITY_COLLECTIONS_CHECKS")]
		[Conditional("UNITY_DOTS_DEBUG")]
		public void ValidateNotNull()
		{
			this.ValidateNonBurst();
		}

		// Token: 0x06000080 RID: 128 RVA: 0x00003014 File Offset: 0x00001214
		[Conditional("ENABLE_UNITY_COLLECTIONS_CHECKS")]
		[Conditional("UNITY_DOTS_DEBUG")]
		private void ThrowIfNull()
		{
			if (this.m_Ptr == null)
			{
				throw new NullReferenceException("The BlobAssetReference is null.");
			}
		}

		// Token: 0x06000081 RID: 129 RVA: 0x0000302B File Offset: 0x0000122B
		[Conditional("ENABLE_UNITY_COLLECTIONS_CHECKS")]
		[Conditional("UNITY_DOTS_DEBUG")]
		public void ValidateAllowNull()
		{
			if (this.m_Ptr == null)
			{
				return;
			}
			this.ValidateNonBurst();
		}

		// Token: 0x06000082 RID: 130 RVA: 0x0000303E File Offset: 0x0000123E
		[Conditional("ENABLE_UNITY_COLLECTIONS_CHECKS")]
		[Conditional("UNITY_DOTS_DEBUG")]
		private unsafe void ValidateNotDeserialized()
		{
			if (this.Header->Allocator.ToAllocator == Allocator.None)
			{
				throw new InvalidOperationException("It's not possible to release a blob asset reference that was deserialized. It will be automatically released when the scene is unloaded ");
			}
			this.Header->Invalidate();
		}

		// Token: 0x06000083 RID: 131 RVA: 0x00003069 File Offset: 0x00001269
		public unsafe void Dispose()
		{
			Memory.Unmanaged.Free<BlobAssetHeader>(this.Header, this.Header->Allocator);
			this.m_Ptr = null;
		}

		// Token: 0x06000084 RID: 132 RVA: 0x00003089 File Offset: 0x00001289
		public bool Equals(BlobAssetReferenceData other)
		{
			return this.m_Ptr == other.m_Ptr;
		}

		// Token: 0x06000085 RID: 133 RVA: 0x0000309C File Offset: 0x0000129C
		public override int GetHashCode()
		{
			if (IntPtr.Size == 8)
			{
				int num = this.m_Ptr;
				int num2 = this.m_Ptr >> (32 & (sizeof(UIntPtr) * 8 - 1));
				return (num * 397) ^ num2;
			}
			return this.m_Ptr;
		}

		// Token: 0x06000086 RID: 134 RVA: 0x000030DE File Offset: 0x000012DE
		[Conditional("ENABLE_UNITY_COLLECTIONS_CHECKS")]
		[Conditional("UNITY_DOTS_DEBUG")]
		internal unsafe void ProtectAgainstDisposal()
		{
			if (this.IsProtected)
			{
				throw new InvalidOperationException("Cannot protect this BlobAssetReference, it is already protected.");
			}
			this.Header->ValidationPtr = null - this.m_Ptr - 1;
		}

		// Token: 0x06000087 RID: 135 RVA: 0x0000310C File Offset: 0x0000130C
		[Conditional("ENABLE_UNITY_COLLECTIONS_CHECKS")]
		[Conditional("UNITY_DOTS_DEBUG")]
		internal unsafe void UnprotectAgainstDisposal()
		{
			if (!this.IsProtected)
			{
				throw new InvalidOperationException("Cannot unprotect this BlobAssetReference, it is not protected.");
			}
			this.Header->ValidationPtr = (void*)this.m_Ptr;
		}

		// Token: 0x06000088 RID: 136 RVA: 0x00003132 File Offset: 0x00001332
		[Conditional("ENABLE_UNITY_COLLECTIONS_CHECKS")]
		[Conditional("UNITY_DOTS_DEBUG")]
		private void ValidateNotProtected()
		{
			if (this.IsProtected)
			{
				throw new InvalidOperationException("This BlobAssetReference is owned by a BlobAssetStore and cannot be disposed directly.");
			}
		}

		// Token: 0x1700000C RID: 12
		// (get) Token: 0x06000089 RID: 137 RVA: 0x00003147 File Offset: 0x00001347
		private unsafe bool IsProtected
		{
			get
			{
				return this.Header->ValidationPtr == null - this.m_Ptr - 1;
			}
		}

		// Token: 0x04000031 RID: 49
		[NativeDisableUnsafePtrRestriction]
		[FieldOffset(0)]
		public unsafe byte* m_Ptr;

		// Token: 0x04000032 RID: 50
		[FieldOffset(0)]
		internal long m_Align8Union;
	}
}
