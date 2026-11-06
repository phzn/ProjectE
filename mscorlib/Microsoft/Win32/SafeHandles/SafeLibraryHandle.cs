using System;

namespace Microsoft.Win32.SafeHandles
{
	// Token: 0x020000B0 RID: 176
	internal sealed class SafeLibraryHandle : SafeHandleZeroOrMinusOneIsInvalid
	{
		// Token: 0x06000475 RID: 1141 RVA: 0x0001747F File Offset: 0x0001567F
		internal SafeLibraryHandle()
			: base(true)
		{
		}

		// Token: 0x06000476 RID: 1142 RVA: 0x00017488 File Offset: 0x00015688
		internal SafeLibraryHandle(bool ownsHandle)
			: base(ownsHandle)
		{
		}

		// Token: 0x06000477 RID: 1143 RVA: 0x00017491 File Offset: 0x00015691
		protected override bool ReleaseHandle()
		{
			return Interop.Kernel32.FreeLibrary(this.handle);
		}
	}
}
