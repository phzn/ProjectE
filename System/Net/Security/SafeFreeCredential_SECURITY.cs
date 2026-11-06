using System;

namespace System.Net.Security
{
	// Token: 0x02000848 RID: 2120
	internal sealed class SafeFreeCredential_SECURITY : SafeFreeCredentials
	{
		// Token: 0x06004381 RID: 17281 RVA: 0x000EB999 File Offset: 0x000E9B99
		protected override bool ReleaseHandle()
		{
			return global::Interop.SspiCli.FreeCredentialsHandle(ref this._handle) == 0;
		}
	}
}
