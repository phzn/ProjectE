using System;

namespace System.Net.Security
{
	// Token: 0x02000849 RID: 2121
	internal sealed class SafeDeleteContext_SECURITY : SafeDeleteContext
	{
		// Token: 0x06004382 RID: 17282 RVA: 0x000EB9A9 File Offset: 0x000E9BA9
		internal SafeDeleteContext_SECURITY()
		{
		}

		// Token: 0x06004383 RID: 17283 RVA: 0x000EB9B1 File Offset: 0x000E9BB1
		protected override bool ReleaseHandle()
		{
			if (this._EffectiveCredential != null)
			{
				this._EffectiveCredential.DangerousRelease();
			}
			return global::Interop.SspiCli.DeleteSecurityContext(ref this._handle) == 0;
		}
	}
}
