using System;
using System.ComponentModel;

namespace Steamworks
{
	// Token: 0x02000173 RID: 371
	[EditorBrowsable(EditorBrowsableState.Never)]
	public abstract class Callback
	{
		// Token: 0x17000023 RID: 35
		// (get) Token: 0x0600089F RID: 2207
		public abstract bool IsGameServer { get; }

		// Token: 0x060008A0 RID: 2208
		internal abstract Type GetCallbackType();

		// Token: 0x060008A1 RID: 2209
		internal abstract void OnRunCallback(IntPtr pvParam);

		// Token: 0x060008A2 RID: 2210
		internal abstract void SetUnregistered();
	}
}
