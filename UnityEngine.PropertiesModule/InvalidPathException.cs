using System;

namespace Unity.Properties
{
	// Token: 0x02000019 RID: 25
	[Serializable]
	public class InvalidPathException : Exception
	{
		// Token: 0x0600005D RID: 93 RVA: 0x00002F53 File Offset: 0x00001153
		public InvalidPathException(string message)
			: base(message)
		{
		}

		// Token: 0x0600005E RID: 94 RVA: 0x00002F5E File Offset: 0x0000115E
		public InvalidPathException(string message, Exception inner)
			: base(message, inner)
		{
		}
	}
}
