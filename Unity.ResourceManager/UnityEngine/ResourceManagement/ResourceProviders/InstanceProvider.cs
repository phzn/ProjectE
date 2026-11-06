using System;
using System.Collections.Generic;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace UnityEngine.ResourceManagement.ResourceProviders
{
	// Token: 0x02000038 RID: 56
	public class InstanceProvider : IInstanceProvider
	{
		// Token: 0x06000179 RID: 377 RVA: 0x00006958 File Offset: 0x00004B58
		public GameObject ProvideInstance(ResourceManager resourceManager, AsyncOperationHandle<GameObject> prefabHandle, InstantiationParameters instantiateParameters)
		{
			GameObject gameObject = instantiateParameters.Instantiate<GameObject>(prefabHandle.Result);
			this.m_InstanceObjectToPrefabHandle.Add(gameObject, prefabHandle);
			return gameObject;
		}

		// Token: 0x0600017A RID: 378 RVA: 0x00006984 File Offset: 0x00004B84
		public void ReleaseInstance(ResourceManager resourceManager, GameObject instance)
		{
			if (instance == null)
			{
				return;
			}
			AsyncOperationHandle<GameObject> asyncOperationHandle;
			if (!this.m_InstanceObjectToPrefabHandle.TryGetValue(instance, out asyncOperationHandle))
			{
				Debug.LogWarningFormat("Releasing unknown GameObject {0} to InstanceProvider.", new object[] { instance });
			}
			else
			{
				asyncOperationHandle.Release();
				this.m_InstanceObjectToPrefabHandle.Remove(instance);
			}
			if (instance != null)
			{
				if (Application.isPlaying)
				{
					Object.Destroy(instance);
					return;
				}
				Object.DestroyImmediate(instance);
			}
		}

		// Token: 0x04000096 RID: 150
		private Dictionary<GameObject, AsyncOperationHandle<GameObject>> m_InstanceObjectToPrefabHandle = new Dictionary<GameObject, AsyncOperationHandle<GameObject>>();
	}
}
