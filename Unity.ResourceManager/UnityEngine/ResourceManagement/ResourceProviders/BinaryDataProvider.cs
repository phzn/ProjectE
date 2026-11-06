using System;
using System.ComponentModel;
using System.IO;
using UnityEngine.Networking;
using UnityEngine.ResourceManagement.Exceptions;
using UnityEngine.ResourceManagement.Util;

namespace UnityEngine.ResourceManagement.ResourceProviders
{
	// Token: 0x02000034 RID: 52
	[DisplayName("Binary Data Provider")]
	internal class BinaryDataProvider : ResourceProviderBase
	{
		// Token: 0x1700003F RID: 63
		// (get) Token: 0x06000168 RID: 360 RVA: 0x00006825 File Offset: 0x00004A25
		// (set) Token: 0x06000169 RID: 361 RVA: 0x0000682D File Offset: 0x00004A2D
		public bool IgnoreFailures { get; set; }

		// Token: 0x0600016A RID: 362 RVA: 0x00006836 File Offset: 0x00004A36
		public virtual object Convert(Type type, byte[] data)
		{
			return data;
		}

		// Token: 0x0600016B RID: 363 RVA: 0x00006839 File Offset: 0x00004A39
		public override void Provide(ProvideHandle provideHandle)
		{
			new BinaryDataProvider.InternalOp().Start(provideHandle, this);
		}

		// Token: 0x02000075 RID: 117
		internal class InternalOp
		{
			// Token: 0x06000353 RID: 851 RVA: 0x0000B9D8 File Offset: 0x00009BD8
			private float GetPercentComplete()
			{
				if (this.m_RequestOperation == null)
				{
					return 0f;
				}
				return this.m_RequestOperation.progress;
			}

			// Token: 0x06000354 RID: 852 RVA: 0x0000B9F4 File Offset: 0x00009BF4
			public void Start(ProvideHandle provideHandle, BinaryDataProvider rawProvider)
			{
				this.m_PI = provideHandle;
				this.m_PI.SetWaitForCompletionCallback(new Func<bool>(this.WaitForCompletionHandler));
				provideHandle.SetProgressCallback(new Func<float>(this.GetPercentComplete));
				this.m_Provider = rawProvider;
				ProviderLoadRequestOptions providerLoadRequestOptions = this.m_PI.Location.Data as ProviderLoadRequestOptions;
				if (providerLoadRequestOptions != null)
				{
					this.m_IgnoreFailures = providerLoadRequestOptions.IgnoreFailures;
					this.m_Timeout = providerLoadRequestOptions.WebRequestTimeout;
				}
				else
				{
					this.m_IgnoreFailures = rawProvider.IgnoreFailures;
					this.m_Timeout = 0;
				}
				string text = this.m_PI.ResourceManager.TransformInternalId(this.m_PI.Location);
				if (ResourceManagerConfig.ShouldPathUseWebRequest(text))
				{
					this.SendWebRequest(text);
					return;
				}
				if (File.Exists(text))
				{
					if (text.EndsWith(".json"))
					{
						throw new Exception("Trying to read non binary data at path '" + text + "'.");
					}
					byte[] array = File.ReadAllBytes(text);
					object obj = this.ConvertBytes(array);
					this.m_PI.Complete<object>(obj, obj != null, (obj == null) ? new Exception(string.Format("Unable to load asset of type {0} from location {1}.", this.m_PI.Type, this.m_PI.Location)) : null);
					this.m_Complete = true;
					return;
				}
				else
				{
					Exception ex = null;
					if (this.m_IgnoreFailures)
					{
						this.m_PI.Complete<object>(null, true, ex);
						this.m_Complete = true;
						return;
					}
					ex = new Exception(string.Format("Invalid path in TextDataProvider : '{0}'.", text));
					this.m_PI.Complete<object>(null, false, ex);
					this.m_Complete = true;
					return;
				}
			}

			// Token: 0x06000355 RID: 853 RVA: 0x0000BB74 File Offset: 0x00009D74
			private bool WaitForCompletionHandler()
			{
				if (this.m_Complete)
				{
					return true;
				}
				if (this.m_RequestOperation != null)
				{
					if (this.m_RequestOperation.isDone && !this.m_Complete)
					{
						this.RequestOperation_completed(this.m_RequestOperation);
					}
					else if (!this.m_RequestOperation.isDone)
					{
						return false;
					}
				}
				return this.m_Complete;
			}

			// Token: 0x06000356 RID: 854 RVA: 0x0000BBCC File Offset: 0x00009DCC
			private void RequestOperation_completed(AsyncOperation op)
			{
				if (this.m_Complete)
				{
					return;
				}
				UnityWebRequestAsyncOperation unityWebRequestAsyncOperation = op as UnityWebRequestAsyncOperation;
				byte[] array = null;
				Exception ex = null;
				if (unityWebRequestAsyncOperation != null)
				{
					UnityWebRequest webRequest = unityWebRequestAsyncOperation.webRequest;
					UnityWebRequestResult unityWebRequestResult;
					if (!UnityWebRequestUtilities.RequestHasErrors(webRequest, out unityWebRequestResult))
					{
						array = webRequest.downloadHandler.data;
					}
					else
					{
						ex = new RemoteProviderException("TextDataProvider : unable to load from url : " + webRequest.url, this.m_PI.Location, unityWebRequestResult, null);
					}
					webRequest.Dispose();
				}
				else
				{
					ex = new RemoteProviderException("TextDataProvider unable to load from unknown url", this.m_PI.Location, null, null);
				}
				this.CompleteOperation(array, ex);
			}

			// Token: 0x06000357 RID: 855 RVA: 0x0000BC5C File Offset: 0x00009E5C
			protected void CompleteOperation(byte[] data, Exception exception)
			{
				object obj = null;
				if (data != null && data.Length != 0)
				{
					obj = this.ConvertBytes(data);
				}
				this.m_PI.Complete<object>(obj, obj != null || this.m_IgnoreFailures, exception);
				this.m_Complete = true;
			}

			// Token: 0x06000358 RID: 856 RVA: 0x0000BC9C File Offset: 0x00009E9C
			private object ConvertBytes(byte[] data)
			{
				object obj;
				try
				{
					obj = this.m_Provider.Convert(this.m_PI.Type, data);
				}
				catch (Exception ex)
				{
					if (!this.m_IgnoreFailures)
					{
						Debug.LogException(ex);
					}
					obj = null;
				}
				return obj;
			}

			// Token: 0x06000359 RID: 857 RVA: 0x0000BCE8 File Offset: 0x00009EE8
			protected virtual void SendWebRequest(string path)
			{
				UnityWebRequest unityWebRequest = new UnityWebRequest(path, "GET", new DownloadHandlerBuffer(), null);
				if (this.m_Timeout > 0)
				{
					unityWebRequest.timeout = this.m_Timeout;
				}
				Action<UnityWebRequest> webRequestOverride = this.m_PI.ResourceManager.WebRequestOverride;
				if (webRequestOverride != null)
				{
					webRequestOverride(unityWebRequest);
				}
				this.m_RequestQueueOperation = WebRequestQueue.QueueRequest(unityWebRequest);
				if (!this.m_RequestQueueOperation.IsDone)
				{
					WebRequestQueueOperation requestQueueOperation = this.m_RequestQueueOperation;
					requestQueueOperation.OnComplete = (Action<UnityWebRequestAsyncOperation>)Delegate.Combine(requestQueueOperation.OnComplete, new Action<UnityWebRequestAsyncOperation>(delegate(UnityWebRequestAsyncOperation asyncOperation)
					{
						this.m_RequestOperation = asyncOperation;
						this.m_RequestOperation.completed += this.RequestOperation_completed;
					}));
					return;
				}
				this.m_RequestOperation = this.m_RequestQueueOperation.Result;
				if (this.m_RequestOperation.isDone)
				{
					this.RequestOperation_completed(this.m_RequestOperation);
					return;
				}
				this.m_RequestOperation.completed += this.RequestOperation_completed;
			}

			// Token: 0x0400015B RID: 347
			private BinaryDataProvider m_Provider;

			// Token: 0x0400015C RID: 348
			private UnityWebRequestAsyncOperation m_RequestOperation;

			// Token: 0x0400015D RID: 349
			private WebRequestQueueOperation m_RequestQueueOperation;

			// Token: 0x0400015E RID: 350
			private ProvideHandle m_PI;

			// Token: 0x0400015F RID: 351
			private bool m_IgnoreFailures;

			// Token: 0x04000160 RID: 352
			private bool m_Complete;

			// Token: 0x04000161 RID: 353
			private int m_Timeout;
		}
	}
}
