using System;

namespace System.Runtime
{
	// Token: 0x0200000B RID: 11
	internal class AsyncEventArgs<TArgument, TResult> : AsyncEventArgs<TArgument>
	{
		// Token: 0x17000006 RID: 6
		// (get) Token: 0x06000021 RID: 33 RVA: 0x000022C5 File Offset: 0x000004C5
		// (set) Token: 0x06000022 RID: 34 RVA: 0x000022CD File Offset: 0x000004CD
		public TResult Result { get; set; }
	}
}
