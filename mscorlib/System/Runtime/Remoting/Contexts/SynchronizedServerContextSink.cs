using System;
using System.Runtime.Remoting.Messaging;

namespace System.Runtime.Remoting.Contexts
{
	// Token: 0x020005A0 RID: 1440
	internal class SynchronizedServerContextSink : IMessageSink
	{
		// Token: 0x060037F4 RID: 14324 RVA: 0x000C90D8 File Offset: 0x000C72D8
		public SynchronizedServerContextSink(IMessageSink next, SynchronizationAttribute att)
		{
			this._att = att;
			this._next = next;
		}

		// Token: 0x170007D7 RID: 2007
		// (get) Token: 0x060037F5 RID: 14325 RVA: 0x000C90EE File Offset: 0x000C72EE
		public IMessageSink NextSink
		{
			get
			{
				return this._next;
			}
		}

		// Token: 0x060037F6 RID: 14326 RVA: 0x000C90F6 File Offset: 0x000C72F6
		public IMessageCtrl AsyncProcessMessage(IMessage msg, IMessageSink replySink)
		{
			this._att.AcquireLock();
			replySink = new SynchronizedContextReplySink(replySink, this._att, false);
			return this._next.AsyncProcessMessage(msg, replySink);
		}

		// Token: 0x060037F7 RID: 14327 RVA: 0x000C9120 File Offset: 0x000C7320
		public IMessage SyncProcessMessage(IMessage msg)
		{
			this._att.AcquireLock();
			IMessage message;
			try
			{
				message = this._next.SyncProcessMessage(msg);
			}
			finally
			{
				this._att.ReleaseLock();
			}
			return message;
		}

		// Token: 0x040025C2 RID: 9666
		private IMessageSink _next;

		// Token: 0x040025C3 RID: 9667
		private SynchronizationAttribute _att;
	}
}
