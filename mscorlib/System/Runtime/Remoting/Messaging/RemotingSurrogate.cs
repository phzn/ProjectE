using System;
using System.Runtime.Serialization;

namespace System.Runtime.Remoting.Messaging
{
	// Token: 0x02000632 RID: 1586
	internal class RemotingSurrogate : ISerializationSurrogate
	{
		// Token: 0x06003BE3 RID: 15331 RVA: 0x000D0976 File Offset: 0x000CEB76
		public virtual void GetObjectData(object obj, SerializationInfo si, StreamingContext sc)
		{
			if (obj == null || si == null)
			{
				throw new ArgumentNullException();
			}
			if (RemotingServices.IsTransparentProxy(obj))
			{
				RemotingServices.GetRealProxy(obj).GetObjectData(si, sc);
				return;
			}
			RemotingServices.GetObjectData(obj, si, sc);
		}

		// Token: 0x06003BE4 RID: 15332 RVA: 0x000472CC File Offset: 0x000454CC
		public virtual object SetObjectData(object obj, SerializationInfo si, StreamingContext sc, ISurrogateSelector selector)
		{
			throw new NotSupportedException();
		}
	}
}
