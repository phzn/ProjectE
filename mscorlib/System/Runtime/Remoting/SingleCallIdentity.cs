using System;
using System.Runtime.Remoting.Contexts;
using System.Runtime.Remoting.Messaging;

namespace System.Runtime.Remoting
{
	// Token: 0x02000572 RID: 1394
	internal class SingleCallIdentity : ServerIdentity
	{
		// Token: 0x060036C7 RID: 14023 RVA: 0x000C5C03 File Offset: 0x000C3E03
		public SingleCallIdentity(string objectUri, Context context, Type objectType)
			: base(objectUri, context, objectType)
		{
		}

		// Token: 0x060036C8 RID: 14024 RVA: 0x000C5D0C File Offset: 0x000C3F0C
		public override IMessage SyncObjectProcessMessage(IMessage msg)
		{
			MarshalByRefObject marshalByRefObject = (MarshalByRefObject)Activator.CreateInstance(this._objectType, true);
			if (marshalByRefObject.ObjectIdentity == null)
			{
				marshalByRefObject.ObjectIdentity = this;
			}
			IMessage message = this._context.CreateServerObjectSinkChain(marshalByRefObject, false).SyncProcessMessage(msg);
			if (marshalByRefObject is IDisposable)
			{
				((IDisposable)marshalByRefObject).Dispose();
			}
			return message;
		}

		// Token: 0x060036C9 RID: 14025 RVA: 0x000C5D60 File Offset: 0x000C3F60
		public override IMessageCtrl AsyncObjectProcessMessage(IMessage msg, IMessageSink replySink)
		{
			MarshalByRefObject marshalByRefObject = (MarshalByRefObject)Activator.CreateInstance(this._objectType, true);
			IMessageSink messageSink = this._context.CreateServerObjectSinkChain(marshalByRefObject, false);
			if (marshalByRefObject is IDisposable)
			{
				replySink = new DisposerReplySink(replySink, (IDisposable)marshalByRefObject);
			}
			return messageSink.AsyncProcessMessage(msg, replySink);
		}
	}
}
