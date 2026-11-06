using System;
using System.Threading;

namespace System.Net.Mime
{
	// Token: 0x020007E1 RID: 2017
	internal sealed class MultiAsyncResult : LazyAsyncResult
	{
		// Token: 0x06004091 RID: 16529 RVA: 0x000DCF24 File Offset: 0x000DB124
		internal MultiAsyncResult(object context, AsyncCallback callback, object state)
			: base(context, state, callback)
		{
			this._context = context;
		}

		// Token: 0x17000E90 RID: 3728
		// (get) Token: 0x06004092 RID: 16530 RVA: 0x000DCF36 File Offset: 0x000DB136
		internal object Context
		{
			get
			{
				return this._context;
			}
		}

		// Token: 0x06004093 RID: 16531 RVA: 0x000DCF3E File Offset: 0x000DB13E
		internal void Enter()
		{
			this.Increment();
		}

		// Token: 0x06004094 RID: 16532 RVA: 0x000DCF46 File Offset: 0x000DB146
		internal void Leave()
		{
			this.Decrement();
		}

		// Token: 0x06004095 RID: 16533 RVA: 0x000DCF4E File Offset: 0x000DB14E
		internal void Leave(object result)
		{
			base.Result = result;
			this.Decrement();
		}

		// Token: 0x06004096 RID: 16534 RVA: 0x000DCF5D File Offset: 0x000DB15D
		private void Decrement()
		{
			if (Interlocked.Decrement(ref this._outstanding) == -1)
			{
				base.InvokeCallback(base.Result);
			}
		}

		// Token: 0x06004097 RID: 16535 RVA: 0x000DCF79 File Offset: 0x000DB179
		private void Increment()
		{
			Interlocked.Increment(ref this._outstanding);
		}

		// Token: 0x06004098 RID: 16536 RVA: 0x000DCF46 File Offset: 0x000DB146
		internal void CompleteSequence()
		{
			this.Decrement();
		}

		// Token: 0x06004099 RID: 16537 RVA: 0x000DCF87 File Offset: 0x000DB187
		internal static object End(IAsyncResult result)
		{
			MultiAsyncResult multiAsyncResult = (MultiAsyncResult)result;
			multiAsyncResult.InternalWaitForCompletion();
			return multiAsyncResult.Result;
		}

		// Token: 0x040026AC RID: 9900
		private readonly object _context;

		// Token: 0x040026AD RID: 9901
		private int _outstanding;
	}
}
