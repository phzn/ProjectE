using System;
using System.Runtime.ExceptionServices;

namespace Internal.Runtime.Augments
{
	// Token: 0x020000C6 RID: 198
	internal class RuntimeAugments
	{
		// Token: 0x060004B4 RID: 1204 RVA: 0x00017829 File Offset: 0x00015A29
		public static void ReportUnhandledException(Exception exception)
		{
			ExceptionDispatchInfo.Capture(exception).Throw();
		}

		// Token: 0x1700007C RID: 124
		// (get) Token: 0x060004B5 RID: 1205 RVA: 0x00017836 File Offset: 0x00015A36
		internal static ReflectionExecutionDomainCallbacks Callbacks
		{
			get
			{
				return RuntimeAugments.s_reflectionExecutionDomainCallbacks;
			}
		}

		// Token: 0x04000FD0 RID: 4048
		private static ReflectionExecutionDomainCallbacks s_reflectionExecutionDomainCallbacks = new ReflectionExecutionDomainCallbacks();
	}
}
