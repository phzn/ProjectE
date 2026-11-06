using System;
using System.Runtime.Remoting.Messaging;

namespace System.Runtime.Remoting.Activation
{
	// Token: 0x020005CD RID: 1485
	internal class AppDomainLevelActivator : IActivator
	{
		// Token: 0x060038C8 RID: 14536 RVA: 0x000CAC90 File Offset: 0x000C8E90
		public AppDomainLevelActivator(string activationUrl, IActivator next)
		{
			this._activationUrl = activationUrl;
			this._next = next;
		}

		// Token: 0x17000812 RID: 2066
		// (get) Token: 0x060038C9 RID: 14537 RVA: 0x0004841D File Offset: 0x0004661D
		public ActivatorLevel Level
		{
			get
			{
				return ActivatorLevel.AppDomain;
			}
		}

		// Token: 0x17000813 RID: 2067
		// (get) Token: 0x060038CA RID: 14538 RVA: 0x000CACA6 File Offset: 0x000C8EA6
		// (set) Token: 0x060038CB RID: 14539 RVA: 0x000CACAE File Offset: 0x000C8EAE
		public IActivator NextActivator
		{
			get
			{
				return this._next;
			}
			set
			{
				this._next = value;
			}
		}

		// Token: 0x060038CC RID: 14540 RVA: 0x000CACB8 File Offset: 0x000C8EB8
		public IConstructionReturnMessage Activate(IConstructionCallMessage ctorCall)
		{
			IActivator activator = (IActivator)RemotingServices.Connect(typeof(IActivator), this._activationUrl);
			ctorCall.Activator = ctorCall.Activator.NextActivator;
			IConstructionReturnMessage constructionReturnMessage;
			try
			{
				constructionReturnMessage = activator.Activate(ctorCall);
			}
			catch (Exception ex)
			{
				return new ConstructionResponse(ex, ctorCall);
			}
			ObjRef objRef = (ObjRef)constructionReturnMessage.ReturnValue;
			if (RemotingServices.GetIdentityForUri(objRef.URI) != null)
			{
				throw new RemotingException("Inconsistent state during activation; there may be two proxies for the same object");
			}
			object obj;
			Identity orCreateClientIdentity = RemotingServices.GetOrCreateClientIdentity(objRef, null, out obj);
			RemotingServices.SetMessageTargetIdentity(ctorCall, orCreateClientIdentity);
			return constructionReturnMessage;
		}

		// Token: 0x040025F9 RID: 9721
		private string _activationUrl;

		// Token: 0x040025FA RID: 9722
		private IActivator _next;
	}
}
