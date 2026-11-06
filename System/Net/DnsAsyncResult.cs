using System;
using System.Threading;

namespace System.Net
{
	// Token: 0x0200067D RID: 1661
	internal class DnsAsyncResult : IAsyncResult
	{
		// Token: 0x0600344A RID: 13386 RVA: 0x000B63D6 File Offset: 0x000B45D6
		public DnsAsyncResult(AsyncCallback cb, object state)
		{
			this.callback = cb;
			this.state = state;
		}

		// Token: 0x0600344B RID: 13387 RVA: 0x000B63EC File Offset: 0x000B45EC
		public void SetCompleted(bool synch, IPHostEntry entry, Exception e)
		{
			this.synch = synch;
			this.entry = entry;
			this.exc = e;
			lock (this)
			{
				if (this.is_completed)
				{
					return;
				}
				this.is_completed = true;
				if (this.handle != null)
				{
					this.handle.Set();
				}
			}
			if (this.callback != null)
			{
				ThreadPool.QueueUserWorkItem(DnsAsyncResult.internal_cb, this);
			}
		}

		// Token: 0x0600344C RID: 13388 RVA: 0x000B6470 File Offset: 0x000B4670
		public void SetCompleted(bool synch, Exception e)
		{
			this.SetCompleted(synch, null, e);
		}

		// Token: 0x0600344D RID: 13389 RVA: 0x000B647B File Offset: 0x000B467B
		public void SetCompleted(bool synch, IPHostEntry entry)
		{
			this.SetCompleted(synch, entry, null);
		}

		// Token: 0x0600344E RID: 13390 RVA: 0x000B6488 File Offset: 0x000B4688
		private static void CB(object _this)
		{
			DnsAsyncResult dnsAsyncResult = (DnsAsyncResult)_this;
			dnsAsyncResult.callback(dnsAsyncResult);
		}

		// Token: 0x17000A8B RID: 2699
		// (get) Token: 0x0600344F RID: 13391 RVA: 0x000B64A8 File Offset: 0x000B46A8
		public object AsyncState
		{
			get
			{
				return this.state;
			}
		}

		// Token: 0x17000A8C RID: 2700
		// (get) Token: 0x06003450 RID: 13392 RVA: 0x000B64B0 File Offset: 0x000B46B0
		public WaitHandle AsyncWaitHandle
		{
			get
			{
				lock (this)
				{
					if (this.handle == null)
					{
						this.handle = new ManualResetEvent(this.is_completed);
					}
				}
				return this.handle;
			}
		}

		// Token: 0x17000A8D RID: 2701
		// (get) Token: 0x06003451 RID: 13393 RVA: 0x000B6504 File Offset: 0x000B4704
		public Exception Exception
		{
			get
			{
				return this.exc;
			}
		}

		// Token: 0x17000A8E RID: 2702
		// (get) Token: 0x06003452 RID: 13394 RVA: 0x000B650C File Offset: 0x000B470C
		public IPHostEntry HostEntry
		{
			get
			{
				return this.entry;
			}
		}

		// Token: 0x17000A8F RID: 2703
		// (get) Token: 0x06003453 RID: 13395 RVA: 0x000B6514 File Offset: 0x000B4714
		public bool CompletedSynchronously
		{
			get
			{
				return this.synch;
			}
		}

		// Token: 0x17000A90 RID: 2704
		// (get) Token: 0x06003454 RID: 13396 RVA: 0x000B651C File Offset: 0x000B471C
		public bool IsCompleted
		{
			get
			{
				bool flag2;
				lock (this)
				{
					flag2 = this.is_completed;
				}
				return flag2;
			}
		}

		// Token: 0x04001E85 RID: 7813
		private static WaitCallback internal_cb = new WaitCallback(DnsAsyncResult.CB);

		// Token: 0x04001E86 RID: 7814
		private ManualResetEvent handle;

		// Token: 0x04001E87 RID: 7815
		private bool synch;

		// Token: 0x04001E88 RID: 7816
		private bool is_completed;

		// Token: 0x04001E89 RID: 7817
		private AsyncCallback callback;

		// Token: 0x04001E8A RID: 7818
		private object state;

		// Token: 0x04001E8B RID: 7819
		private IPHostEntry entry;

		// Token: 0x04001E8C RID: 7820
		private Exception exc;
	}
}
