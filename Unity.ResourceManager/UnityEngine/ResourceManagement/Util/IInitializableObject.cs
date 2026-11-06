using System;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace UnityEngine.ResourceManagement.Util
{
	// Token: 0x0200001E RID: 30
	public interface IInitializableObject
	{
		// Token: 0x060000E2 RID: 226
		bool Initialize(string id, string data);

		// Token: 0x060000E3 RID: 227
		AsyncOperationHandle<bool> InitializeAsync(ResourceManager rm, string id, string data);
	}
}
