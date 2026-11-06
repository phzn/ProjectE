using System;

namespace System.Net.Security
{
	// Token: 0x0200084B RID: 2123
	internal sealed class SafeFreeContextBufferChannelBinding_SECURITY : SafeFreeContextBufferChannelBinding
	{
		// Token: 0x0600438B RID: 17291 RVA: 0x000EB744 File Offset: 0x000E9944
		protected override bool ReleaseHandle()
		{
			return global::Interop.SspiCli.FreeContextBuffer(this.handle) == 0;
		}
	}
}
