using System;
using System.Threading;

namespace System.Runtime.Remoting.Activation
{
	// Token: 0x020005CE RID: 1486
	[Serializable]
	internal class ConstructionLevelActivator : IActivator
	{
		// Token: 0x17000814 RID: 2068
		// (get) Token: 0x060038CD RID: 14541 RVA: 0x0002280B File Offset: 0x00020A0B
		public ActivatorLevel Level
		{
			get
			{
				return ActivatorLevel.Construction;
			}
		}

		// Token: 0x17000815 RID: 2069
		// (get) Token: 0x060038CE RID: 14542 RVA: 0x0000AF5E File Offset: 0x0000915E
		// (set) Token: 0x060038CF RID: 14543 RVA: 0x00004BF9 File Offset: 0x00002DF9
		public IActivator NextActivator
		{
			get
			{
				return null;
			}
			set
			{
			}
		}

		// Token: 0x060038D0 RID: 14544 RVA: 0x000CAD4C File Offset: 0x000C8F4C
		public IConstructionReturnMessage Activate(IConstructionCallMessage msg)
		{
			return (IConstructionReturnMessage)Thread.CurrentContext.GetServerContextSinkChain().SyncProcessMessage(msg);
		}
	}
}
