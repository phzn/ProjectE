using System;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;

namespace Unity.Entities.Serialization
{
	// Token: 0x02000383 RID: 899
	internal class PagedAllocation : IDisposable
	{
		// Token: 0x060017ED RID: 6125 RVA: 0x00059BC0 File Offset: 0x00057DC0
		public unsafe PagedAllocation(AllocatorManager.AllocatorHandle allocator, int pageSize = 65536)
		{
			this.PageSize = pageSize;
			this.Allocator = allocator;
			this.CurrentGlobalOffset = 0;
			ref UnsafeList<PagedAllocation.PageInfo> pages = ref this.Pages;
			PagedAllocation.PageInfo pageInfo = new PagedAllocation.PageInfo((byte*)Memory.Unmanaged.Allocate((long)this.PageSize, 16, this.Allocator), 0);
			pages.Add(in pageInfo);
		}

		// Token: 0x060017EE RID: 6126 RVA: 0x00059C24 File Offset: 0x00057E24
		public unsafe byte* Reserve(int size, bool clearMemory = true)
		{
			this.CurrentGlobalOffset += size;
			byte* ptr;
			if (size > this.PageSize)
			{
				ptr = (byte*)Memory.Unmanaged.Allocate((long)size, 16, this.Allocator);
				ref UnsafeList<PagedAllocation.PageInfo> pages = ref this.Pages;
				PagedAllocation.PageInfo pageInfo = new PagedAllocation.PageInfo(ptr, size);
				pages.Add(in pageInfo);
			}
			else
			{
				PagedAllocation.PageInfo* ptr2 = this.Pages.Ptr + (this.Pages.Length - 1);
				if (ptr2->FreeOffset + size <= this.PageSize)
				{
					ptr = ptr2->Buffer + ptr2->FreeOffset;
					ptr2->FreeOffset += size;
				}
				else
				{
					ptr = (byte*)Memory.Unmanaged.Allocate((long)this.PageSize, 16, this.Allocator);
					PagedAllocation.PageInfo pageInfo2 = new PagedAllocation.PageInfo(ptr, size);
					this.Pages.Add(in pageInfo2);
				}
			}
			if (clearMemory)
			{
				UnsafeUtility.MemClear((void*)ptr, (long)size);
			}
			return ptr;
		}

		// Token: 0x060017EF RID: 6127 RVA: 0x00059CF4 File Offset: 0x00057EF4
		public void Dispose()
		{
			if (this.IsDisposed)
			{
				return;
			}
			for (int i = 0; i < this.Pages.Length; i++)
			{
				Memory.Unmanaged.Free<byte>(this.Pages[i].Buffer, this.Allocator);
			}
			this.Pages.Dispose();
			this.IsDisposed = true;
		}

		// Token: 0x1700021C RID: 540
		// (get) Token: 0x060017F0 RID: 6128 RVA: 0x00059D4E File Offset: 0x00057F4E
		public ref UnsafeList<PagedAllocation.PageInfo> Pages
		{
			get
			{
				return ref this._pages;
			}
		}

		// Token: 0x1700021D RID: 541
		// (get) Token: 0x060017F1 RID: 6129 RVA: 0x00059D56 File Offset: 0x00057F56
		public int PageSize { get; }

		// Token: 0x1700021E RID: 542
		// (get) Token: 0x060017F2 RID: 6130 RVA: 0x00059D5E File Offset: 0x00057F5E
		public AllocatorManager.AllocatorHandle Allocator { get; }

		// Token: 0x1700021F RID: 543
		// (get) Token: 0x060017F3 RID: 6131 RVA: 0x00059D66 File Offset: 0x00057F66
		// (set) Token: 0x060017F4 RID: 6132 RVA: 0x00059D6E File Offset: 0x00057F6E
		public int CurrentGlobalOffset { get; private set; }

		// Token: 0x17000220 RID: 544
		// (get) Token: 0x060017F5 RID: 6133 RVA: 0x00059D77 File Offset: 0x00057F77
		// (set) Token: 0x060017F6 RID: 6134 RVA: 0x00059D7F File Offset: 0x00057F7F
		public bool IsDisposed { get; private set; }

		// Token: 0x04000922 RID: 2338
		private UnsafeList<PagedAllocation.PageInfo> _pages = new UnsafeList<PagedAllocation.PageInfo>(16, Unity.Collections.Allocator.Persistent, NativeArrayOptions.UninitializedMemory);

		// Token: 0x02000384 RID: 900
		public struct PageInfo
		{
			// Token: 0x060017F7 RID: 6135 RVA: 0x00059D88 File Offset: 0x00057F88
			public unsafe PageInfo(byte* buffer, int freeOffset)
			{
				this.FreeOffset = freeOffset;
				this.Buffer = buffer;
			}

			// Token: 0x04000923 RID: 2339
			public int FreeOffset;

			// Token: 0x04000924 RID: 2340
			public unsafe byte* Buffer;
		}
	}
}
