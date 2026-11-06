using System;
using System.Threading;

namespace System.Runtime.Remoting.Messaging
{
	// Token: 0x02000616 RID: 1558
	[Serializable]
	internal class EnvoyTerminatorSink : IMessageSink
	{
		// Token: 0x06003AD5 RID: 15061 RVA: 0x000CE2D5 File Offset: 0x000CC4D5
		public IMessage SyncProcessMessage(IMessage msg)
		{
			return Thread.CurrentContext.GetClientContextSinkChain().SyncProcessMessage(msg);
		}

		// Token: 0x06003AD6 RID: 15062 RVA: 0x000CE2E7 File Offset: 0x000CC4E7
		public IMessageCtrl AsyncProcessMessage(IMessage msg, IMessageSink replySink)
		{
			return Thread.CurrentContext.GetClientContextSinkChain().AsyncProcessMessage(msg, replySink);
		}

		// Token: 0x170008A5 RID: 2213
		// (get) Token: 0x06003AD7 RID: 15063 RVA: 0x0000AF5E File Offset: 0x0000915E
		public IMessageSink NextSink
		{
			get
			{
				return null;
			}
		}

		// Token: 0x0400268C RID: 9868
		public static EnvoyTerminatorSink Instance = new EnvoyTerminatorSink();
	}
}
