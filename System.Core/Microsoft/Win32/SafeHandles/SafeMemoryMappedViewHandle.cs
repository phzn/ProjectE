using System;
using System.IO.MemoryMappedFiles;
using System.Runtime.InteropServices;
using Unity;

namespace Microsoft.Win32.SafeHandles
{
	/// <summary>Provides a safe handle that represents a view of a block of unmanaged memory for random access. </summary>
	// Token: 0x02000015 RID: 21
	public sealed class SafeMemoryMappedViewHandle : SafeBuffer
	{
		// Token: 0x0600003F RID: 63 RVA: 0x00002362 File Offset: 0x00000562
		internal SafeMemoryMappedViewHandle(IntPtr mmap_handle, IntPtr base_address, long size)
			: base(true)
		{
			this.mmap_handle = mmap_handle;
			this.handle = base_address;
			base.Initialize((ulong)size);
		}

		// Token: 0x06000040 RID: 64 RVA: 0x00002380 File Offset: 0x00000580
		internal void Flush()
		{
			MemoryMapImpl.Flush(this.mmap_handle);
		}

		// Token: 0x06000041 RID: 65 RVA: 0x0000238D File Offset: 0x0000058D
		protected override bool ReleaseHandle()
		{
			if (this.handle != (IntPtr)(-1))
			{
				return MemoryMapImpl.Unmap(this.mmap_handle);
			}
			throw new NotImplementedException();
		}

		// Token: 0x06000042 RID: 66 RVA: 0x0000235B File Offset: 0x0000055B
		internal SafeMemoryMappedViewHandle()
		{
			global::Unity.ThrowStub.ThrowNotSupportedException();
		}

		// Token: 0x040002B8 RID: 696
		private IntPtr mmap_handle;
	}
}
