using System;

namespace Microsoft.Win32.SafeHandles
{
	/// <summary>Provides a safe handle that represents a key storage provider (NCRYPT_PROV_HANDLE).</summary>
	// Token: 0x02000018 RID: 24
	public sealed class SafeNCryptProviderHandle : SafeNCryptHandle
	{
		// Token: 0x0600004C RID: 76 RVA: 0x000023D1 File Offset: 0x000005D1
		protected override bool ReleaseNativeHandle()
		{
			return false;
		}
	}
}
