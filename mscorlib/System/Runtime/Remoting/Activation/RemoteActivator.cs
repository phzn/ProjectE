using System;
using System.Runtime.Remoting.Lifetime;
using System.Runtime.Remoting.Messaging;

namespace System.Runtime.Remoting.Activation
{
	// Token: 0x020005D4 RID: 1492
	internal class RemoteActivator : MarshalByRefObject, IActivator
	{
		// Token: 0x060038E5 RID: 14565 RVA: 0x000CAE78 File Offset: 0x000C9078
		public IConstructionReturnMessage Activate(IConstructionCallMessage msg)
		{
			if (!RemotingConfiguration.IsActivationAllowed(msg.ActivationType))
			{
				throw new RemotingException("The type " + msg.ActivationTypeName + " is not allowed to be client activated");
			}
			object[] array = null;
			if (msg.ActivationType.IsContextful)
			{
				array = new object[]
				{
					new RemoteActivationAttribute(msg.ContextProperties)
				};
			}
			return new ConstructionResponse(RemotingServices.Marshal((MarshalByRefObject)Activator.CreateInstance(msg.ActivationType, msg.Args, array)), null, msg);
		}

		// Token: 0x060038E6 RID: 14566 RVA: 0x000CAEF4 File Offset: 0x000C90F4
		public override object InitializeLifetimeService()
		{
			ILease lease = (ILease)base.InitializeLifetimeService();
			if (lease.CurrentState == LeaseState.Initial)
			{
				lease.InitialLeaseTime = TimeSpan.FromMinutes(30.0);
				lease.SponsorshipTimeout = TimeSpan.FromMinutes(1.0);
				lease.RenewOnCallTime = TimeSpan.FromMinutes(10.0);
			}
			return lease;
		}

		// Token: 0x1700081F RID: 2079
		// (get) Token: 0x060038E7 RID: 14567 RVA: 0x000472CC File Offset: 0x000454CC
		public ActivatorLevel Level
		{
			get
			{
				throw new NotSupportedException();
			}
		}

		// Token: 0x17000820 RID: 2080
		// (get) Token: 0x060038E8 RID: 14568 RVA: 0x000472CC File Offset: 0x000454CC
		// (set) Token: 0x060038E9 RID: 14569 RVA: 0x000472CC File Offset: 0x000454CC
		public IActivator NextActivator
		{
			get
			{
				throw new NotSupportedException();
			}
			set
			{
				throw new NotSupportedException();
			}
		}
	}
}
