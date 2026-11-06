using System;
using System.Collections.Generic;
using UnityEngine;

namespace Unity.Jobs
{
	// Token: 0x02000006 RID: 6
	public class EarlyInitHelpers
	{
		// Token: 0x06000005 RID: 5 RVA: 0x000020CB File Offset: 0x000002CB
		static EarlyInitHelpers()
		{
			EarlyInitHelpers.FlushEarlyInits();
		}

		// Token: 0x06000006 RID: 6 RVA: 0x000020D4 File Offset: 0x000002D4
		public static void FlushEarlyInits()
		{
			while (EarlyInitHelpers.s_PendingDelegates != null)
			{
				List<EarlyInitHelpers.EarlyInitFunction> list = EarlyInitHelpers.s_PendingDelegates;
				EarlyInitHelpers.s_PendingDelegates = null;
				for (int i = 0; i < list.Count; i++)
				{
					try
					{
						list[i]();
					}
					catch (Exception ex)
					{
						Debug.LogException(ex);
					}
				}
			}
		}

		// Token: 0x06000007 RID: 7 RVA: 0x00002130 File Offset: 0x00000330
		public static void AddEarlyInitFunction(EarlyInitHelpers.EarlyInitFunction func)
		{
			if (EarlyInitHelpers.s_PendingDelegates == null)
			{
				EarlyInitHelpers.s_PendingDelegates = new List<EarlyInitHelpers.EarlyInitFunction>();
			}
			EarlyInitHelpers.s_PendingDelegates.Add(func);
		}

		// Token: 0x06000008 RID: 8 RVA: 0x0000214E File Offset: 0x0000034E
		public static void JobReflectionDataCreationFailed(Exception ex)
		{
			Debug.LogError("Failed to create job reflection data. Please refer to callstack of exception for information on which job could not produce its reflection data.");
			Debug.LogException(ex);
		}

		// Token: 0x04000006 RID: 6
		private static List<EarlyInitHelpers.EarlyInitFunction> s_PendingDelegates;

		// Token: 0x02000007 RID: 7
		// (Invoke) Token: 0x0600000B RID: 11
		public delegate void EarlyInitFunction();
	}
}
