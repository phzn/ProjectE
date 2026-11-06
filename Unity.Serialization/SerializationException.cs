using System;

namespace Unity.Serialization
{
	// Token: 0x0200000C RID: 12
	[Serializable]
	public class SerializationException : Exception
	{
		// Token: 0x06000015 RID: 21 RVA: 0x00002174 File Offset: 0x00000374
		public SerializationException(string message)
			: base(message)
		{
		}
	}
}
