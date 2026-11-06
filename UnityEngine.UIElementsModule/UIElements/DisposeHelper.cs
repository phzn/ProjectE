using System;
using System.Diagnostics;

namespace UnityEngine.UIElements
{
	// Token: 0x0200015E RID: 350
	internal class DisposeHelper
	{
		// Token: 0x06000B77 RID: 2935 RVA: 0x0002DB84 File Offset: 0x0002BD84
		[Conditional("UNITY_UIELEMENTS_DEBUG_DISPOSE")]
		public static void NotifyMissingDispose(IDisposable disposable)
		{
			bool flag = disposable == null;
			if (!flag)
			{
				Debug.LogError("An IDisposable instance of type '" + disposable.GetType().FullName + "' has not been disposed.");
			}
		}

		// Token: 0x06000B78 RID: 2936 RVA: 0x0002DBBC File Offset: 0x0002BDBC
		public static void NotifyDisposedUsed(IDisposable disposable)
		{
			Debug.LogError("An instance of type '" + disposable.GetType().FullName + "' is being used although it has been disposed.");
		}
	}
}
