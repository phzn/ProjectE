using System;
using System.Runtime.Serialization;

namespace System.Threading
{
	/// <summary>The exception that is thrown when a failure occurs in a managed thread after the underlying operating system thread has been started, but before the thread is ready to execute user code.</summary>
	// Token: 0x02000298 RID: 664
	[Serializable]
	public sealed class ThreadStartException : SystemException
	{
		// Token: 0x06001D9F RID: 7583 RVA: 0x0006E705 File Offset: 0x0006C905
		internal ThreadStartException()
			: base("Thread failed to start.")
		{
			base.HResult = -2146233051;
		}

		// Token: 0x06001DA0 RID: 7584 RVA: 0x0006E71D File Offset: 0x0006C91D
		internal ThreadStartException(Exception reason)
			: base("Thread failed to start.", reason)
		{
			base.HResult = -2146233051;
		}

		// Token: 0x06001DA1 RID: 7585 RVA: 0x00020A69 File Offset: 0x0001EC69
		private ThreadStartException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}
	}
}
