using System;
using System.IO;
using System.Security;

namespace Microsoft.Win32.SafeHandles
{
	// Token: 0x020000B6 RID: 182
	[SecurityCritical]
	internal class SafeFindHandle : SafeHandleZeroOrMinusOneIsInvalid
	{
		// Token: 0x0600048F RID: 1167 RVA: 0x0001747F File Offset: 0x0001567F
		[SecurityCritical]
		internal SafeFindHandle()
			: base(true)
		{
		}

		// Token: 0x06000490 RID: 1168 RVA: 0x00017617 File Offset: 0x00015817
		internal SafeFindHandle(IntPtr preexistingHandle)
			: base(true)
		{
			base.SetHandle(preexistingHandle);
		}

		// Token: 0x06000491 RID: 1169 RVA: 0x00017627 File Offset: 0x00015827
		[SecurityCritical]
		protected override bool ReleaseHandle()
		{
			return MonoIO.FindCloseFile(this.handle);
		}
	}
}
