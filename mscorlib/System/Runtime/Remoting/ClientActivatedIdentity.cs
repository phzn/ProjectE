using System;
using System.Runtime.Remoting.Messaging;

namespace System.Runtime.Remoting
{
	// Token: 0x02000570 RID: 1392
	internal class ClientActivatedIdentity : ServerIdentity
	{
		// Token: 0x060036BC RID: 14012 RVA: 0x000C5B32 File Offset: 0x000C3D32
		public ClientActivatedIdentity(string objectUri, Type objectType)
			: base(objectUri, null, objectType)
		{
		}

		// Token: 0x060036BD RID: 14013 RVA: 0x000C5B3D File Offset: 0x000C3D3D
		public MarshalByRefObject GetServerObject()
		{
			return this._serverObject;
		}

		// Token: 0x060036BE RID: 14014 RVA: 0x000C5B45 File Offset: 0x000C3D45
		public MarshalByRefObject GetClientProxy()
		{
			return this._targetThis;
		}

		// Token: 0x060036BF RID: 14015 RVA: 0x000C5B4D File Offset: 0x000C3D4D
		public void SetClientProxy(MarshalByRefObject obj)
		{
			this._targetThis = obj;
		}

		// Token: 0x060036C0 RID: 14016 RVA: 0x000C5B56 File Offset: 0x000C3D56
		public override void OnLifetimeExpired()
		{
			base.OnLifetimeExpired();
			RemotingServices.DisposeIdentity(this);
		}

		// Token: 0x060036C1 RID: 14017 RVA: 0x000C5B64 File Offset: 0x000C3D64
		public override IMessage SyncObjectProcessMessage(IMessage msg)
		{
			if (this._serverSink == null)
			{
				bool flag = this._targetThis != null;
				this._serverSink = this._context.CreateServerObjectSinkChain(flag ? this._targetThis : this._serverObject, flag);
			}
			return this._serverSink.SyncProcessMessage(msg);
		}

		// Token: 0x060036C2 RID: 14018 RVA: 0x000C5BB4 File Offset: 0x000C3DB4
		public override IMessageCtrl AsyncObjectProcessMessage(IMessage msg, IMessageSink replySink)
		{
			if (this._serverSink == null)
			{
				bool flag = this._targetThis != null;
				this._serverSink = this._context.CreateServerObjectSinkChain(flag ? this._targetThis : this._serverObject, flag);
			}
			return this._serverSink.AsyncProcessMessage(msg, replySink);
		}

		// Token: 0x0400255E RID: 9566
		private MarshalByRefObject _targetThis;
	}
}
