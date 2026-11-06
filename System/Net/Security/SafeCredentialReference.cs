using System;
using Microsoft.Win32.SafeHandles;

namespace System.Net.Security
{
	// Token: 0x02000847 RID: 2119
	internal sealed class SafeCredentialReference : CriticalHandleMinusOneIsInvalid
	{
		// Token: 0x0600437D RID: 17277 RVA: 0x000EB91C File Offset: 0x000E9B1C
		internal static SafeCredentialReference CreateReference(SafeFreeCredentials target)
		{
			SafeCredentialReference safeCredentialReference = new SafeCredentialReference(target);
			if (safeCredentialReference.IsInvalid)
			{
				return null;
			}
			return safeCredentialReference;
		}

		// Token: 0x0600437E RID: 17278 RVA: 0x000EB93C File Offset: 0x000E9B3C
		private SafeCredentialReference(SafeFreeCredentials target)
		{
			bool flag = false;
			target.DangerousAddRef(ref flag);
			this.Target = target;
			base.SetHandle(new IntPtr(0));
		}

		// Token: 0x0600437F RID: 17279 RVA: 0x000EB96C File Offset: 0x000E9B6C
		protected override bool ReleaseHandle()
		{
			SafeFreeCredentials target = this.Target;
			if (target != null)
			{
				target.DangerousRelease();
			}
			this.Target = null;
			return true;
		}

		// Token: 0x040028CE RID: 10446
		internal SafeFreeCredentials Target;
	}
}
