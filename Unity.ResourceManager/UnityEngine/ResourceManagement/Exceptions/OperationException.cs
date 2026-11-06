using System;

namespace UnityEngine.ResourceManagement.Exceptions
{
	// Token: 0x02000010 RID: 16
	public class OperationException : Exception
	{
		// Token: 0x060000A8 RID: 168 RVA: 0x000040F8 File Offset: 0x000022F8
		public OperationException(string message, Exception innerException = null)
			: base(message, innerException)
		{
		}

		// Token: 0x060000A9 RID: 169 RVA: 0x00004102 File Offset: 0x00002302
		public override string ToString()
		{
			return string.Format("{0} : {1}\n{2}", base.GetType().Name, base.Message, base.InnerException);
		}
	}
}
