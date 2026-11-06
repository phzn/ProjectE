using System;
using System.Threading;

namespace System.Net.Security
{
	// Token: 0x0200084D RID: 2125
	internal static class SSPIHandleCache
	{
		// Token: 0x06004398 RID: 17304 RVA: 0x000EBE08 File Offset: 0x000EA008
		internal static void CacheCredential(SafeFreeCredentials newHandle)
		{
			try
			{
				SafeCredentialReference safeCredentialReference = SafeCredentialReference.CreateReference(newHandle);
				if (safeCredentialReference != null)
				{
					int num = Interlocked.Increment(ref SSPIHandleCache.s_current) & 31;
					safeCredentialReference = Interlocked.Exchange<SafeCredentialReference>(ref SSPIHandleCache.s_cacheSlots[num], safeCredentialReference);
					if (safeCredentialReference != null)
					{
						safeCredentialReference.Dispose();
					}
				}
			}
			catch (Exception ex)
			{
				if (!ExceptionCheck.IsFatal(ex))
				{
					NetEventSource.Fail(null, "Attempted to throw: {e}", "CacheCredential");
				}
			}
		}

		// Token: 0x040028D0 RID: 10448
		private const int c_MaxCacheSize = 31;

		// Token: 0x040028D1 RID: 10449
		private static SafeCredentialReference[] s_cacheSlots = new SafeCredentialReference[32];

		// Token: 0x040028D2 RID: 10450
		private static int s_current = -1;
	}
}
