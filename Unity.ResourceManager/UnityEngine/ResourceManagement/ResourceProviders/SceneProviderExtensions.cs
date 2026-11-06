using System;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.SceneManagement;

namespace UnityEngine.ResourceManagement.ResourceProviders
{
	// Token: 0x0200003F RID: 63
	internal static class SceneProviderExtensions
	{
		// Token: 0x06000199 RID: 409 RVA: 0x00006B7A File Offset: 0x00004D7A
		public static AsyncOperationHandle<SceneInstance> ReleaseScene(this ISceneProvider provider, ResourceManager resourceManager, AsyncOperationHandle<SceneInstance> sceneLoadHandle, UnloadSceneOptions unloadOptions)
		{
			if (provider is ISceneProvider2)
			{
				return ((ISceneProvider2)provider).ReleaseScene(resourceManager, sceneLoadHandle, unloadOptions);
			}
			return provider.ReleaseScene(resourceManager, sceneLoadHandle);
		}
	}
}
