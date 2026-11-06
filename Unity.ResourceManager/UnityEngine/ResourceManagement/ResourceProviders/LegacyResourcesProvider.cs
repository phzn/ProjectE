using System;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine.ResourceManagement.ResourceLocations;
using UnityEngine.ResourceManagement.Util;

namespace UnityEngine.ResourceManagement.ResourceProviders
{
	// Token: 0x02000041 RID: 65
	[DisplayName("Assets from Legacy Resources")]
	public class LegacyResourcesProvider : ResourceProviderBase
	{
		// Token: 0x0600019C RID: 412 RVA: 0x00006BAC File Offset: 0x00004DAC
		public override void Provide(ProvideHandle pi)
		{
			Type type = pi.Type;
			bool flag = type.IsGenericType && typeof(IList<>) == type.GetGenericTypeDefinition();
			string text = pi.ResourceManager.TransformInternalId(pi.Location);
			if (type.IsArray || flag)
			{
				object obj;
				if (type.IsArray)
				{
					obj = ResourceManagerConfig.CreateArrayResult(type, Resources.LoadAll(text, type.GetElementType()));
				}
				else
				{
					obj = ResourceManagerConfig.CreateListResult(type, Resources.LoadAll(text, type.GetGenericArguments()[0]));
				}
				pi.Complete<object>(obj, obj != null, (obj == null) ? new Exception(string.Format("Unable to load asset of type {0} from location {1}.", pi.Type, pi.Location)) : null);
				return;
			}
			string text2;
			string text3;
			if (ResourceManagerConfig.ExtractKeyAndSubKey(text, out text2, out text3))
			{
				Object[] array = Resources.LoadAll(text2, pi.Type);
				object obj2 = null;
				foreach (Object @object in array)
				{
					if (@object.name == text3 && pi.Type.IsAssignableFrom(@object.GetType()))
					{
						obj2 = @object;
						break;
					}
				}
				pi.Complete<object>(obj2, obj2 != null, (obj2 == null) ? new Exception(string.Format("Unable to load asset of type {0} from location {1}.", pi.Type, pi.Location)) : null);
				return;
			}
			new LegacyResourcesProvider.InternalOp().Start(pi);
		}

		// Token: 0x0600019D RID: 413 RVA: 0x00006D0C File Offset: 0x00004F0C
		public override void Release(IResourceLocation location, object asset)
		{
			if (location == null)
			{
				throw new ArgumentNullException("location");
			}
			Object @object = asset as Object;
			if (@object != null && !(@object is GameObject))
			{
				Resources.UnloadAsset(@object);
			}
		}

		// Token: 0x02000077 RID: 119
		internal class InternalOp
		{
			// Token: 0x06000369 RID: 873 RVA: 0x0000C3A8 File Offset: 0x0000A5A8
			public void Start(ProvideHandle provideHandle)
			{
				this.m_ProvideHandle = provideHandle;
				provideHandle.SetProgressCallback(new Func<float>(this.PercentComplete));
				provideHandle.SetWaitForCompletionCallback(new Func<bool>(this.WaitForCompletionHandler));
				this.m_RequestOperation = Resources.LoadAsync(this.m_ProvideHandle.ResourceManager.TransformInternalId(this.m_ProvideHandle.Location), this.m_ProvideHandle.Type);
				this.m_RequestOperation.completed += this.AsyncOperationCompleted;
			}

			// Token: 0x0600036A RID: 874 RVA: 0x0000C42C File Offset: 0x0000A62C
			private bool WaitForCompletionHandler()
			{
				if (!this.m_RequestOperation.isDone && Mathf.Approximately(this.m_RequestOperation.progress, 1f))
				{
					this.m_RequestOperation.completed -= this.AsyncOperationCompleted;
					this.AsyncOperationCompleted(this.m_RequestOperation);
					return true;
				}
				return this.m_RequestOperation != null && this.m_RequestOperation.isDone;
			}

			// Token: 0x0600036B RID: 875 RVA: 0x0000C498 File Offset: 0x0000A698
			private void AsyncOperationCompleted(AsyncOperation op)
			{
				ResourceRequest resourceRequest = op as ResourceRequest;
				object obj = ((resourceRequest != null) ? resourceRequest.asset : null);
				obj = ((obj != null && this.m_ProvideHandle.Type.IsAssignableFrom(obj.GetType())) ? obj : null);
				this.m_ProvideHandle.Complete<object>(obj, obj != null, (obj == null) ? new Exception(string.Format("Unable to load asset of type {0} from location {1}.", this.m_ProvideHandle.Type, this.m_ProvideHandle.Location)) : null);
			}

			// Token: 0x0600036C RID: 876 RVA: 0x0000C514 File Offset: 0x0000A714
			public float PercentComplete()
			{
				if (this.m_RequestOperation == null)
				{
					return 0f;
				}
				return this.m_RequestOperation.progress;
			}

			// Token: 0x04000168 RID: 360
			private ResourceRequest m_RequestOperation;

			// Token: 0x04000169 RID: 361
			private ProvideHandle m_ProvideHandle;
		}
	}
}
