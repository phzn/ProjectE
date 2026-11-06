using System;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace UnityEngine.Localization
{
	// Token: 0x0200001B RID: 27
	public interface IPreloadRequired
	{
		// Token: 0x1700002C RID: 44
		// (get) Token: 0x060000EA RID: 234
		AsyncOperationHandle PreloadOperation { get; }
	}
}
