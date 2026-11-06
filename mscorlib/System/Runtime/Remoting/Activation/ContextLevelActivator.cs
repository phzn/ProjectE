using System;
using System.Runtime.Remoting.Contexts;
using System.Runtime.Remoting.Messaging;

namespace System.Runtime.Remoting.Activation
{
	// Token: 0x020005CF RID: 1487
	[Serializable]
	internal class ContextLevelActivator : IActivator
	{
		// Token: 0x060038D2 RID: 14546 RVA: 0x000CAD63 File Offset: 0x000C8F63
		public ContextLevelActivator(IActivator next)
		{
			this.m_NextActivator = next;
		}

		// Token: 0x17000816 RID: 2070
		// (get) Token: 0x060038D3 RID: 14547 RVA: 0x00047F75 File Offset: 0x00046175
		public ActivatorLevel Level
		{
			get
			{
				return ActivatorLevel.Context;
			}
		}

		// Token: 0x17000817 RID: 2071
		// (get) Token: 0x060038D4 RID: 14548 RVA: 0x000CAD72 File Offset: 0x000C8F72
		// (set) Token: 0x060038D5 RID: 14549 RVA: 0x000CAD7A File Offset: 0x000C8F7A
		public IActivator NextActivator
		{
			get
			{
				return this.m_NextActivator;
			}
			set
			{
				this.m_NextActivator = value;
			}
		}

		// Token: 0x060038D6 RID: 14550 RVA: 0x000CAD84 File Offset: 0x000C8F84
		public IConstructionReturnMessage Activate(IConstructionCallMessage ctorCall)
		{
			ServerIdentity serverIdentity = RemotingServices.CreateContextBoundObjectIdentity(ctorCall.ActivationType);
			RemotingServices.SetMessageTargetIdentity(ctorCall, serverIdentity);
			ConstructionCall constructionCall = ctorCall as ConstructionCall;
			if (constructionCall == null || !constructionCall.IsContextOk)
			{
				serverIdentity.Context = Context.CreateNewContext(ctorCall);
				Context context = Context.SwitchToContext(serverIdentity.Context);
				try
				{
					return this.m_NextActivator.Activate(ctorCall);
				}
				finally
				{
					Context.SwitchToContext(context);
				}
			}
			return this.m_NextActivator.Activate(ctorCall);
		}

		// Token: 0x040025FB RID: 9723
		private IActivator m_NextActivator;
	}
}
