using System;

namespace Microsoft.Win32.SafeHandles
{
	// Token: 0x02000066 RID: 102
	internal sealed class SafeLibraryHandle : SafeHandleZeroOrMinusOneIsInvalid
	{
		// Token: 0x060004B2 RID: 1202 RVA: 0x00010C67 File Offset: 0x0000EE67
		internal SafeLibraryHandle()
			: base(true)
		{
		}

		// Token: 0x060004B3 RID: 1203 RVA: 0x00010C70 File Offset: 0x0000EE70
		internal SafeLibraryHandle(bool ownsHandle)
			: base(ownsHandle)
		{
		}

		// Token: 0x060004B4 RID: 1204 RVA: 0x00010C79 File Offset: 0x0000EE79
		protected override bool ReleaseHandle()
		{
			return global::Interop.Kernel32.FreeLibrary(this.handle);
		}
	}
}
