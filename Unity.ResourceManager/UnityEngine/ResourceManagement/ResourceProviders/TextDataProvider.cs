using System;
using System.ComponentModel;
using System.IO;
using UnityEngine.Networking;
using UnityEngine.ResourceManagement.Exceptions;
using UnityEngine.ResourceManagement.Util;

namespace UnityEngine.ResourceManagement.ResourceProviders
{
	// Token: 0x02000045 RID: 69
	[DisplayName("Text Data Provider")]
	public class TextDataProvider : ResourceProviderBase
	{
		// Token: 0x17000052 RID: 82
		// (get) Token: 0x060001B4 RID: 436 RVA: 0x00006F08 File Offset: 0x00005108
		// (set) Token: 0x060001B5 RID: 437 RVA: 0x00006F10 File Offset: 0x00005110
		public bool IgnoreFailures { get; set; }

		// Token: 0x060001B6 RID: 438 RVA: 0x00006F19 File Offset: 0x00005119
		public virtual object Convert(Type type, string text)
		{
			return text;
		}

		// Token: 0x060001B7 RID: 439 RVA: 0x00006F1C File Offset: 0x0000511C
		public override void Provide(ProvideHandle provideHandle)
		{
			new TextDataProvider.InternalOp().Start(provideHandle, this);
		}

		// Token: 0x0200007C RID: 124
		internal class InternalOp
		{
			// Token: 0x06000387 RID: 903 RVA: 0x0000CB93 File Offset: 0x0000AD93
			private float GetPercentComplete()
			{
				if (this.m_RequestOperation == null)
				{
					return 0f;
				}
				return this.m_RequestOperation.progress;
			}

			// Token: 0x06000388 RID: 904 RVA: 0x0000CBB0 File Offset: 0x0000ADB0
			public void Start(ProvideHandle provideHandle, TextDataProvider rawProvider)
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
					string text2 = File.ReadAllText(text);
					object obj = this.ConvertText(text2);
					this.m_PI.Complete<object>(obj, obj != null, (obj == null) ? new Exception(string.Format("Unable to load asset of type {0} from location {1}.", this.m_PI.Type, this.m_PI.Location)) : null);
					this.m_Complete = true;
					return;
				}
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
			}

			// Token: 0x06000389 RID: 905 RVA: 0x0000CD0C File Offset: 0x0000AF0C
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

			// Token: 0x0600038A RID: 906 RVA: 0x0000CD64 File Offset: 0x0000AF64
			private void RequestOperation_completed(AsyncOperation op)
			{
				if (this.m_Complete)
				{
					return;
				}
				UnityWebRequestAsyncOperation unityWebRequestAsyncOperation = op as UnityWebRequestAsyncOperation;
				string text = null;
				Exception ex = null;
				if (unityWebRequestAsyncOperation != null)
				{
					UnityWebRequest webRequest = unityWebRequestAsyncOperation.webRequest;
					UnityWebRequestResult unityWebRequestResult;
					if (!UnityWebRequestUtilities.RequestHasErrors(webRequest, out unityWebRequestResult))
					{
						text = webRequest.downloadHandler.text;
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
				this.CompleteOperation(text, ex);
			}

			// Token: 0x0600038B RID: 907 RVA: 0x0000CDF4 File Offset: 0x0000AFF4
			protected void CompleteOperation(string text, Exception exception)
			{
				object obj = null;
				if (!string.IsNullOrEmpty(text))
				{
					obj = this.ConvertText(text);
				}
				this.m_PI.Complete<object>(obj, obj != null || this.m_IgnoreFailures, exception);
				this.m_Complete = true;
			}

			// Token: 0x0600038C RID: 908 RVA: 0x0000CE34 File Offset: 0x0000B034
			private object ConvertText(string text)
			{
				object obj;
				try
				{
					obj = this.m_Provider.Convert(this.m_PI.Type, text);
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

			// Token: 0x0600038D RID: 909 RVA: 0x0000CE80 File Offset: 0x0000B080
			protected virtual void SendWebRequest(string path)
			{
				try
				{
					path = path.Replace('\\', '/');
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
					if (this.m_RequestQueueOperation.IsDone)
					{
						this.m_RequestOperation = this.m_RequestQueueOperation.Result;
						if (this.m_RequestOperation.isDone)
						{
							this.RequestOperation_completed(this.m_RequestOperation);
						}
						else
						{
							this.m_RequestOperation.completed += this.RequestOperation_completed;
						}
					}
					else
					{
						WebRequestQueueOperation requestQueueOperation = this.m_RequestQueueOperation;
						requestQueueOperation.OnComplete = (Action<UnityWebRequestAsyncOperation>)Delegate.Combine(requestQueueOperation.OnComplete, new Action<UnityWebRequestAsyncOperation>(delegate(UnityWebRequestAsyncOperation asyncOperation)
						{
							this.m_RequestOperation = asyncOperation;
							this.m_RequestOperation.completed += this.RequestOperation_completed;
						}));
					}
				}
				catch (UriFormatException ex)
				{
					throw new UriFormatException("Invalid path '" + path + "'", ex);
				}
			}

			// Token: 0x04000179 RID: 377
			private TextDataProvider m_Provider;

			// Token: 0x0400017A RID: 378
			private UnityWebRequestAsyncOperation m_RequestOperation;

			// Token: 0x0400017B RID: 379
			private WebRequestQueueOperation m_RequestQueueOperation;

			// Token: 0x0400017C RID: 380
			private ProvideHandle m_PI;

			// Token: 0x0400017D RID: 381
			private bool m_IgnoreFailures;

			// Token: 0x0400017E RID: 382
			private bool m_Complete;

			// Token: 0x0400017F RID: 383
			private int m_Timeout;
		}
	}
}
