using System;
using System.Runtime.Serialization;

namespace Unity.Serialization
{
	// Token: 0x02000008 RID: 8
	[Serializable]
	public class ParseErrorException : Exception
	{
		// Token: 0x06000008 RID: 8 RVA: 0x00002174 File Offset: 0x00000374
		public ParseErrorException(string message)
			: base(message)
		{
		}

		// Token: 0x06000009 RID: 9 RVA: 0x0000217D File Offset: 0x0000037D
		public ParseErrorException(string message, Exception inner)
			: base(message, inner)
		{
		}

		// Token: 0x0600000A RID: 10 RVA: 0x00002187 File Offset: 0x00000387
		protected ParseErrorException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}
	}
}
