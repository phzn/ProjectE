using System;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace UnityEngine.ResourceManagement.ResourceProviders
{
	// Token: 0x02000037 RID: 55
	public interface IInstanceProvider
	{
		// Token: 0x06000177 RID: 375
		GameObject ProvideInstance(ResourceManager resourceManager, AsyncOperationHandle<GameObject> prefabHandle, InstantiationParameters instantiateParameters);

		// Token: 0x06000178 RID: 376
		void ReleaseInstance(ResourceManager resourceManager, GameObject instance);
	}
}
