using System;
using System.ComponentModel;

namespace Steamworks
{
	// Token: 0x02000175 RID: 373
	[EditorBrowsable(EditorBrowsableState.Never)]
	public abstract class CallResult
	{
		// Token: 0x060008B1 RID: 2225
		protected internal abstract Type GetCallbackType();

		// Token: 0x060008B2 RID: 2226
		protected internal abstract void OnRunCallResult(IntPtr pvParam, bool bFailed, ulong hSteamAPICall);

		// Token: 0x060008B3 RID: 2227
		protected internal abstract void SetUnregistered();
	}
}
