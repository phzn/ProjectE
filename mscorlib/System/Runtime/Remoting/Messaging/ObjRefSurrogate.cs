using System;
using System.Runtime.Serialization;

namespace System.Runtime.Remoting.Messaging
{
	// Token: 0x02000633 RID: 1587
	internal class ObjRefSurrogate : ISerializationSurrogate
	{
		// Token: 0x06003BE6 RID: 15334 RVA: 0x000D09A2 File Offset: 0x000CEBA2
		public virtual void GetObjectData(object obj, SerializationInfo si, StreamingContext sc)
		{
			if (obj == null || si == null)
			{
				throw new ArgumentNullException();
			}
			((ObjRef)obj).GetObjectData(si, sc);
			si.AddValue("fIsMarshalled", 0);
		}

		// Token: 0x06003BE7 RID: 15335 RVA: 0x000D09C9 File Offset: 0x000CEBC9
		public virtual object SetObjectData(object obj, SerializationInfo si, StreamingContext sc, ISurrogateSelector selector)
		{
			throw new NotSupportedException("Do not use RemotingSurrogateSelector when deserializating");
		}
	}
}
