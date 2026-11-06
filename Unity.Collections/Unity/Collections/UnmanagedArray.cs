using System;
using System.Runtime.CompilerServices;

namespace Unity.Collections
{
	// Token: 0x020000D7 RID: 215
	internal struct UnmanagedArray<[global::System.Runtime.CompilerServices.IsUnmanaged] T> : IDisposable where T : struct, ValueType
	{
		// Token: 0x1700011E RID: 286
		// (get) Token: 0x060009F2 RID: 2546 RVA: 0x0001D243 File Offset: 0x0001B443
		public int Length
		{
			get
			{
				return this.m_length;
			}
		}

		// Token: 0x060009F3 RID: 2547 RVA: 0x0001D24B File Offset: 0x0001B44B
		public unsafe UnmanagedArray(int length, AllocatorManager.AllocatorHandle allocator)
		{
			this.m_pointer = (IntPtr)((void*)Memory.Unmanaged.Array.Allocate<T>((long)length, allocator));
			this.m_length = length;
			this.m_allocator = allocator;
		}

		// Token: 0x060009F4 RID: 2548 RVA: 0x0001D26E File Offset: 0x0001B46E
		public unsafe void Dispose()
		{
			Memory.Unmanaged.Free<T>((T*)(void*)this.m_pointer, Allocator.Persistent);
		}

		// Token: 0x060009F5 RID: 2549 RVA: 0x0001D286 File Offset: 0x0001B486
		public unsafe T* GetUnsafePointer()
		{
			return (T*)(void*)this.m_pointer;
		}

		// Token: 0x1700011F RID: 287
		public unsafe ref T this[int index]
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return ref *(T*)((byte*)(void*)this.m_pointer + (IntPtr)index * (IntPtr)sizeof(T));
			}
		}

		// Token: 0x04000416 RID: 1046
		private IntPtr m_pointer;

		// Token: 0x04000417 RID: 1047
		private int m_length;

		// Token: 0x04000418 RID: 1048
		private AllocatorManager.AllocatorHandle m_allocator;
	}
}
