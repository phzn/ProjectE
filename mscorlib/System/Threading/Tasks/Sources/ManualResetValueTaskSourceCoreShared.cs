using System;
using System.Diagnostics;

namespace System.Threading.Tasks.Sources
{
	// Token: 0x0200038D RID: 909
	internal static class ManualResetValueTaskSourceCoreShared
	{
		// Token: 0x0600256D RID: 9581 RVA: 0x00084B99 File Offset: 0x00082D99
		[StackTraceHidden]
		internal static void ThrowInvalidOperationException()
		{
			throw new InvalidOperationException();
		}

		// Token: 0x0600256E RID: 9582 RVA: 0x00084BA0 File Offset: 0x00082DA0
		private static void CompletionSentinel(object _)
		{
			ManualResetValueTaskSourceCoreShared.ThrowInvalidOperationException();
		}

		// Token: 0x04001D8A RID: 7562
		internal static readonly Action<object> s_sentinel = new Action<object>(ManualResetValueTaskSourceCoreShared.CompletionSentinel);
	}
}
