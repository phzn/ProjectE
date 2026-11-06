using System;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace UnityEngine.Localization
{
	// Token: 0x02000020 RID: 32
	internal static class AsyncOperationUtility
	{
		// Token: 0x060000FB RID: 251 RVA: 0x000044E7 File Offset: 0x000026E7
		public static T SynchronousLoad<T>(AsyncOperationHandle<T> op)
		{
			if (!op.IsDone)
			{
				return op.WaitForCompletion();
			}
			return op.Result;
		}
	}
}
