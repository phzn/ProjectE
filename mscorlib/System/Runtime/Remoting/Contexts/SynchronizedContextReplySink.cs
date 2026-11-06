using System;
using System.Runtime.Remoting.Messaging;

namespace System.Runtime.Remoting.Contexts
{
	// Token: 0x020005A1 RID: 1441
	internal class SynchronizedContextReplySink : IMessageSink
	{
		// Token: 0x060037F8 RID: 14328 RVA: 0x000C9164 File Offset: 0x000C7364
		public SynchronizedContextReplySink(IMessageSink next, SynchronizationAttribute att, bool newLock)
		{
			this._newLock = newLock;
			this._next = next;
			this._att = att;
		}

		// Token: 0x170007D8 RID: 2008
		// (get) Token: 0x060037F9 RID: 14329 RVA: 0x000C9181 File Offset: 0x000C7381
		public IMessageSink NextSink
		{
			get
			{
				return this._next;
			}
		}

		// Token: 0x060037FA RID: 14330 RVA: 0x000472CC File Offset: 0x000454CC
		public IMessageCtrl AsyncProcessMessage(IMessage msg, IMessageSink replySink)
		{
			throw new NotSupportedException();
		}

		// Token: 0x060037FB RID: 14331 RVA: 0x000C918C File Offset: 0x000C738C
		public IMessage SyncProcessMessage(IMessage msg)
		{
			if (this._newLock)
			{
				this._att.AcquireLock();
			}
			else
			{
				this._att.ReleaseLock();
			}
			IMessage message;
			try
			{
				message = this._next.SyncProcessMessage(msg);
			}
			finally
			{
				if (this._newLock)
				{
					this._att.ReleaseLock();
				}
			}
			return message;
		}

		// Token: 0x040025C4 RID: 9668
		private IMessageSink _next;

		// Token: 0x040025C5 RID: 9669
		private bool _newLock;

		// Token: 0x040025C6 RID: 9670
		private SynchronizationAttribute _att;
	}
}
