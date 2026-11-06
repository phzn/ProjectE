using System;
using System.IO.MemoryMappedFiles;
using Unity;

namespace Microsoft.Win32.SafeHandles
{
	/// <summary>Provides a safe handle that represents a memory-mapped file for sequential access.</summary>
	// Token: 0x02000014 RID: 20
	public sealed class SafeMemoryMappedFileHandle : SafeHandleZeroOrMinusOneIsInvalid
	{
		// Token: 0x0600003C RID: 60 RVA: 0x00002332 File Offset: 0x00000532
		public SafeMemoryMappedFileHandle(IntPtr preexistingHandle, bool ownsHandle)
			: base(ownsHandle)
		{
			this.handle = preexistingHandle;
		}

		// Token: 0x0600003D RID: 61 RVA: 0x00002342 File Offset: 0x00000542
		protected override bool ReleaseHandle()
		{
			MemoryMapImpl.CloseMapping(this.handle);
			this.handle = IntPtr.Zero;
			return true;
		}

		// Token: 0x0600003E RID: 62 RVA: 0x0000235B File Offset: 0x0000055B
		internal SafeMemoryMappedFileHandle()
		{
			global::Unity.ThrowStub.ThrowNotSupportedException();
		}
	}
}
