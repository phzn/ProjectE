using System;
using System.Runtime.Serialization;

namespace UnityEngine.ResourceManagement.Exceptions
{
	// Token: 0x0200000E RID: 14
	public class ResourceManagerException : Exception
	{
		// Token: 0x0600009A RID: 154 RVA: 0x0000400C File Offset: 0x0000220C
		public ResourceManagerException()
		{
		}

		// Token: 0x0600009B RID: 155 RVA: 0x00004014 File Offset: 0x00002214
		public ResourceManagerException(string message)
			: base(message)
		{
		}

		// Token: 0x0600009C RID: 156 RVA: 0x0000401D File Offset: 0x0000221D
		public ResourceManagerException(string message, Exception innerException)
			: base(message, innerException)
		{
		}

		// Token: 0x0600009D RID: 157 RVA: 0x00004027 File Offset: 0x00002227
		protected ResourceManagerException(SerializationInfo message, StreamingContext context)
			: base(message, context)
		{
		}

		// Token: 0x0600009E RID: 158 RVA: 0x00004031 File Offset: 0x00002231
		public override string ToString()
		{
			return string.Format("{0} : {1}\n{2}", base.GetType().Name, base.Message, base.InnerException);
		}
	}
}
