using System;
using UnityEngine.ResourceManagement.Util;

namespace UnityEngine.ResourceManagement.AsyncOperations
{
	// Token: 0x02000056 RID: 86
	internal interface ICachable
	{
		// Token: 0x17000079 RID: 121
		// (get) Token: 0x06000216 RID: 534
		// (set) Token: 0x06000217 RID: 535
		IOperationCacheKey Key { get; set; }
	}
}
