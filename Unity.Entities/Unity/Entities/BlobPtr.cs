using System;
using System.Diagnostics;
using Unity.Collections.LowLevel.Unsafe;

namespace Unity.Entities
{
	// Token: 0x02000025 RID: 37
	[MayOnlyLiveInBlobStorage]
	[DebuggerDisplay("Cannot display the value of a BlobPtr by itself. Please inspect the root BlobAssetReference instead.")]
	public struct BlobPtr<T> where T : struct
	{
		// Token: 0x17000010 RID: 16
		// (get) Token: 0x0600009E RID: 158 RVA: 0x000034C4 File Offset: 0x000016C4
		public bool IsValid
		{
			get
			{
				return this.m_OffsetPtr != 0;
			}
		}

		// Token: 0x0600009F RID: 159 RVA: 0x000034CF File Offset: 0x000016CF
		[Conditional("ENABLE_UNITY_COLLECTIONS_CHECKS")]
		[Conditional("UNITY_DOTS_DEBUG")]
		private void AssertIsValid()
		{
			if (!this.IsValid)
			{
				throw new InvalidOperationException("The accessed BlobPtr hasn't been allocated.");
			}
		}

		// Token: 0x17000011 RID: 17
		// (get) Token: 0x060000A0 RID: 160 RVA: 0x000034E4 File Offset: 0x000016E4
		public unsafe ref T Value
		{
			get
			{
				fixed (int* ptr = &this.m_OffsetPtr)
				{
					return UnsafeUtility.AsRef<T>((void*)((byte*)ptr + this.m_OffsetPtr));
				}
			}
		}

		// Token: 0x060000A1 RID: 161 RVA: 0x00003508 File Offset: 0x00001708
		public unsafe void* GetUnsafePtr()
		{
			if (this.m_OffsetPtr == 0)
			{
				return null;
			}
			fixed (int* ptr = &this.m_OffsetPtr)
			{
				return (void*)((byte*)ptr + this.m_OffsetPtr);
			}
		}

		// Token: 0x04000034 RID: 52
		internal int m_OffsetPtr;
	}
}
