using System;

namespace System.Net.Security
{
	// Token: 0x02000844 RID: 2116
	internal sealed class SafeFreeContextBuffer_SECURITY : SafeFreeContextBuffer
	{
		// Token: 0x06004372 RID: 17266 RVA: 0x000EB73C File Offset: 0x000E993C
		internal SafeFreeContextBuffer_SECURITY()
		{
		}

		// Token: 0x06004373 RID: 17267 RVA: 0x000EB744 File Offset: 0x000E9944
		protected override bool ReleaseHandle()
		{
			return global::Interop.SspiCli.FreeContextBuffer(this.handle) == 0;
		}
	}
}
