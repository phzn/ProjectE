using System;
using System.Diagnostics;
using Unity.Collections.LowLevel.Unsafe;

namespace Unity.Collections
{
	// Token: 0x0200004B RID: 75
	public struct DoubleRewindableAllocators : IDisposable
	{
		// Token: 0x060001F7 RID: 503 RVA: 0x000069D4 File Offset: 0x00004BD4
		public unsafe void Update()
		{
			RewindableAllocator* ptr = (RewindableAllocator*)UnsafeUtility.AddressOf<RewindableAllocator>(this.UpdateAllocatorHelper0.Allocator);
			RewindableAllocator* ptr2 = (RewindableAllocator*)UnsafeUtility.AddressOf<RewindableAllocator>(this.UpdateAllocatorHelper1.Allocator);
			this.Pointer = ((this.Pointer == ptr) ? ptr2 : ptr);
			this.Allocator.Rewind();
		}

		// Token: 0x060001F8 RID: 504 RVA: 0x00006A21 File Offset: 0x00004C21
		[Conditional("ENABLE_UNITY_COLLECTIONS_CHECKS")]
		[Conditional("UNITY_DOTS_DEBUG")]
		private void CheckIsCreated()
		{
			if (!this.IsCreated)
			{
				throw new InvalidOperationException("DoubleRewindableAllocators is not created.");
			}
		}

		// Token: 0x17000037 RID: 55
		// (get) Token: 0x060001F9 RID: 505 RVA: 0x00006A36 File Offset: 0x00004C36
		public unsafe ref RewindableAllocator Allocator
		{
			get
			{
				return UnsafeUtility.AsRef<RewindableAllocator>((void*)this.Pointer);
			}
		}

		// Token: 0x17000038 RID: 56
		// (get) Token: 0x060001FA RID: 506 RVA: 0x00006A43 File Offset: 0x00004C43
		public bool IsCreated
		{
			get
			{
				return this.Pointer != null;
			}
		}

		// Token: 0x060001FB RID: 507 RVA: 0x00006A52 File Offset: 0x00004C52
		public DoubleRewindableAllocators(AllocatorManager.AllocatorHandle backingAllocator, int initialSizeInBytes)
		{
			this = default(DoubleRewindableAllocators);
			this.Initialize(backingAllocator, initialSizeInBytes);
		}

		// Token: 0x060001FC RID: 508 RVA: 0x00006A64 File Offset: 0x00004C64
		public void Initialize(AllocatorManager.AllocatorHandle backingAllocator, int initialSizeInBytes)
		{
			this.UpdateAllocatorHelper0 = new AllocatorHelper<RewindableAllocator>(backingAllocator, false, 0);
			this.UpdateAllocatorHelper1 = new AllocatorHelper<RewindableAllocator>(backingAllocator, false, 0);
			this.UpdateAllocatorHelper0.Allocator.Initialize(initialSizeInBytes, false);
			this.UpdateAllocatorHelper1.Allocator.Initialize(initialSizeInBytes, false);
			this.Pointer = null;
			this.Update();
		}

		// Token: 0x060001FD RID: 509 RVA: 0x00006AC0 File Offset: 0x00004CC0
		public void Dispose()
		{
			if (!this.IsCreated)
			{
				return;
			}
			this.UpdateAllocatorHelper0.Allocator.Dispose();
			this.UpdateAllocatorHelper1.Allocator.Dispose();
			this.UpdateAllocatorHelper0.Dispose();
			this.UpdateAllocatorHelper1.Dispose();
		}

		// Token: 0x17000039 RID: 57
		// (get) Token: 0x060001FE RID: 510 RVA: 0x00006B0C File Offset: 0x00004D0C
		// (set) Token: 0x060001FF RID: 511 RVA: 0x00006B1E File Offset: 0x00004D1E
		internal bool EnableBlockFree
		{
			get
			{
				return this.UpdateAllocatorHelper0.Allocator.EnableBlockFree;
			}
			set
			{
				this.UpdateAllocatorHelper0.Allocator.EnableBlockFree = value;
				this.UpdateAllocatorHelper1.Allocator.EnableBlockFree = value;
			}
		}

		// Token: 0x040000BE RID: 190
		private unsafe RewindableAllocator* Pointer;

		// Token: 0x040000BF RID: 191
		private AllocatorHelper<RewindableAllocator> UpdateAllocatorHelper0;

		// Token: 0x040000C0 RID: 192
		private AllocatorHelper<RewindableAllocator> UpdateAllocatorHelper1;
	}
}
