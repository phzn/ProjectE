using System;
using System.IO;
using System.Threading;
using UnityEngine.Networking;
using UnityEngine.Profiling;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.Exceptions;
using UnityEngine.ResourceManagement.Profiling;
using UnityEngine.ResourceManagement.ResourceLocations;
using UnityEngine.ResourceManagement.Util;

namespace UnityEngine.ResourceManagement.ResourceProviders
{
	// Token: 0x02000030 RID: 48
	public class AssetBundleResource : IAssetBundleResource, IUpdateReceiver
	{
		// Token: 0x1700003A RID: 58
		// (get) Token: 0x0600013A RID: 314 RVA: 0x000057CF File Offset: 0x000039CF
		private bool HasTimedOut
		{
			get
			{
				return this.m_TimeoutTimer >= (float)this.m_Options.Timeout && this.m_TimeoutOverFrames > 5;
			}
		}

		// Token: 0x1700003B RID: 59
		// (get) Token: 0x0600013B RID: 315 RVA: 0x000057F0 File Offset: 0x000039F0
		internal long BytesToDownload
		{
			get
			{
				if (this.m_BytesToDownload == -1L)
				{
					if (this.m_Options != null)
					{
						this.m_BytesToDownload = this.m_Options.ComputeSize(this.m_ProvideHandle.Location, this.m_ProvideHandle.ResourceManager);
					}
					else
					{
						this.m_BytesToDownload = 0L;
					}
				}
				return this.m_BytesToDownload;
			}
		}

		// Token: 0x0600013C RID: 316 RVA: 0x00005848 File Offset: 0x00003A48
		internal UnityWebRequest CreateWebRequest(IResourceLocation loc)
		{
			string text = this.m_ProvideHandle.ResourceManager.TransformInternalId(loc);
			return this.CreateWebRequest(text);
		}

		// Token: 0x0600013D RID: 317 RVA: 0x00005870 File Offset: 0x00003A70
		internal UnityWebRequest CreateWebRequest(string url)
		{
			Uri uri = new Uri(Uri.UnescapeDataString(url).Replace(" ", "%20"));
			if (this.m_Options == null)
			{
				this.m_Source = BundleSource.Download;
				this.AddBundleToProfiler(ContentStatus.Downloading, this.m_Source);
				return UnityWebRequestAssetBundle.GetAssetBundle(uri);
			}
			UnityWebRequest unityWebRequest;
			if (!string.IsNullOrEmpty(this.m_Options.Hash))
			{
				CachedAssetBundle cachedAssetBundle = new CachedAssetBundle(this.m_Options.BundleName, Hash128.Parse(this.m_Options.Hash));
				this.m_Source = (Caching.IsVersionCached(cachedAssetBundle) ? BundleSource.Cache : BundleSource.Download);
				if (this.m_Options.UseCrcForCachedBundle || this.m_Source == BundleSource.Download)
				{
					unityWebRequest = UnityWebRequestAssetBundle.GetAssetBundle(uri, cachedAssetBundle, this.m_Options.Crc);
				}
				else
				{
					unityWebRequest = UnityWebRequestAssetBundle.GetAssetBundle(uri, cachedAssetBundle, 0U);
				}
			}
			else
			{
				this.m_Source = BundleSource.Download;
				unityWebRequest = UnityWebRequestAssetBundle.GetAssetBundle(uri, this.m_Options.Crc);
			}
			if (this.m_Options.RedirectLimit > 0)
			{
				unityWebRequest.redirectLimit = this.m_Options.RedirectLimit;
			}
			if (this.m_ProvideHandle.ResourceManager.CertificateHandlerInstance != null)
			{
				unityWebRequest.certificateHandler = this.m_ProvideHandle.ResourceManager.CertificateHandlerInstance;
				unityWebRequest.disposeCertificateHandlerOnDispose = false;
			}
			Action<UnityWebRequest> webRequestOverride = this.m_ProvideHandle.ResourceManager.WebRequestOverride;
			if (webRequestOverride != null)
			{
				webRequestOverride(unityWebRequest);
			}
			return unityWebRequest;
		}

		// Token: 0x0600013E RID: 318 RVA: 0x000059C0 File Offset: 0x00003BC0
		public AssetBundleRequest GetAssetPreloadRequest()
		{
			if (this.m_PreloadCompleted || this.GetAssetBundle() == null)
			{
				return null;
			}
			if (this.m_Options.AssetLoadMode == AssetLoadMode.AllPackedAssetsAndDependencies)
			{
				if (this.m_PreloadRequest == null)
				{
					this.m_PreloadRequest = this.m_AssetBundle.LoadAllAssetsAsync();
					this.m_PreloadRequest.completed += delegate(AsyncOperation operation)
					{
						this.m_PreloadCompleted = true;
					};
				}
				return this.m_PreloadRequest;
			}
			return null;
		}

		// Token: 0x0600013F RID: 319 RVA: 0x00005A2B File Offset: 0x00003C2B
		private float PercentComplete()
		{
			if (this.m_RequestOperation == null)
			{
				return 0f;
			}
			return this.m_RequestOperation.progress;
		}

		// Token: 0x06000140 RID: 320 RVA: 0x00005A48 File Offset: 0x00003C48
		private DownloadStatus GetDownloadStatus()
		{
			if (this.m_Options == null)
			{
				return default(DownloadStatus);
			}
			DownloadStatus downloadStatus = new DownloadStatus
			{
				TotalBytes = this.BytesToDownload,
				IsDone = (this.PercentComplete() >= 1f)
			};
			if (this.BytesToDownload > 0L)
			{
				if (this.m_WebRequestQueueOperation != null && string.IsNullOrEmpty(this.m_WebRequestQueueOperation.m_WebRequest.error))
				{
					this.m_DownloadedBytes = (long)this.m_WebRequestQueueOperation.m_WebRequest.downloadedBytes;
				}
				else if (this.m_RequestOperation != null)
				{
					UnityWebRequestAsyncOperation unityWebRequestAsyncOperation = this.m_RequestOperation as UnityWebRequestAsyncOperation;
					if (unityWebRequestAsyncOperation != null && string.IsNullOrEmpty(unityWebRequestAsyncOperation.webRequest.error))
					{
						this.m_DownloadedBytes = (long)unityWebRequestAsyncOperation.webRequest.downloadedBytes;
					}
				}
			}
			downloadStatus.DownloadedBytes = this.m_DownloadedBytes;
			return downloadStatus;
		}

		// Token: 0x06000141 RID: 321 RVA: 0x00005B1E File Offset: 0x00003D1E
		public AssetBundle GetAssetBundle()
		{
			bool isValid = this.m_ProvideHandle.IsValid;
			return this.m_AssetBundle;
		}

		// Token: 0x06000142 RID: 322 RVA: 0x00005B34 File Offset: 0x00003D34
		private void AddBundleToProfiler(ContentStatus status, BundleSource source)
		{
			if (!Profiler.enabled)
			{
				return;
			}
			if (!this.m_ProvideHandle.IsValid)
			{
				return;
			}
			if (status == ContentStatus.Active && this.m_AssetBundle == null)
			{
				ProfilerRuntime.BundleReleased(this.m_Options.BundleName);
				return;
			}
			ProfilerRuntime.AddBundleOperation(this.m_ProvideHandle, this.m_Options, status, source);
		}

		// Token: 0x06000143 RID: 323 RVA: 0x00005B91 File Offset: 0x00003D91
		private void RemoveBundleFromProfiler()
		{
			if (this.m_Options == null)
			{
				return;
			}
			ProfilerRuntime.BundleReleased(this.m_Options.BundleName);
		}

		// Token: 0x06000144 RID: 324 RVA: 0x00005BAC File Offset: 0x00003DAC
		private void OnUnloadOperationComplete(AsyncOperation op)
		{
			this.m_UnloadOperation = null;
			this.BeginOperation();
		}

		// Token: 0x06000145 RID: 325 RVA: 0x00005BBC File Offset: 0x00003DBC
		public void Start(ProvideHandle provideHandle, AssetBundleUnloadOperation unloadOp)
		{
			this.m_Retries = 0;
			this.m_AssetBundle = null;
			this.m_RequestOperation = null;
			this.m_ProvideHandle = provideHandle;
			this.m_Options = this.m_ProvideHandle.Location.Data as AssetBundleRequestOptions;
			this.m_BytesToDownload = -1L;
			this.m_ProvideHandle.SetProgressCallback(new Func<float>(this.PercentComplete));
			this.m_ProvideHandle.SetDownloadProgressCallbacks(new Func<DownloadStatus>(this.GetDownloadStatus));
			this.m_ProvideHandle.SetWaitForCompletionCallback(new Func<bool>(this.WaitForCompletionHandler));
			this.m_UnloadOperation = unloadOp;
			if (this.m_UnloadOperation != null && !this.m_UnloadOperation.isDone)
			{
				this.m_UnloadOperation.completed += this.OnUnloadOperationComplete;
				return;
			}
			this.BeginOperation();
		}

		// Token: 0x06000146 RID: 326 RVA: 0x00005C88 File Offset: 0x00003E88
		private bool WaitForCompletionHandler()
		{
			if (this.m_UnloadOperation != null && !this.m_UnloadOperation.isDone)
			{
				this.m_UnloadOperation.completed -= this.OnUnloadOperationComplete;
				this.m_UnloadOperation.WaitForCompletion();
				this.m_UnloadOperation = null;
				this.BeginOperation();
			}
			if (this.m_RequestOperation == null)
			{
				if (this.m_WebRequestQueueOperation == null)
				{
					return false;
				}
				WebRequestQueue.WaitForRequestToBeActive(this.m_WebRequestQueueOperation, 1);
			}
			UnityWebRequestAsyncOperation unityWebRequestAsyncOperation = this.m_RequestOperation as UnityWebRequestAsyncOperation;
			if (unityWebRequestAsyncOperation != null)
			{
				while (!UnityWebRequestUtilities.IsAssetBundleDownloaded(unityWebRequestAsyncOperation))
				{
					Thread.Sleep(1);
				}
				if (this.m_Source == BundleSource.Cache)
				{
					object obj;
					if (unityWebRequestAsyncOperation == null)
					{
						obj = null;
					}
					else
					{
						UnityWebRequest webRequest = unityWebRequestAsyncOperation.webRequest;
						obj = ((webRequest != null) ? webRequest.downloadHandler : null);
					}
					DownloadHandlerAssetBundle downloadHandlerAssetBundle = (DownloadHandlerAssetBundle)obj;
					if (downloadHandlerAssetBundle.autoLoadAssetBundle)
					{
						this.m_AssetBundle = downloadHandlerAssetBundle.assetBundle;
					}
				}
				WebRequestQueue.DequeueRequest(unityWebRequestAsyncOperation);
				if (!this.m_RequestCompletedCallbackCalled)
				{
					this.m_RequestOperation.completed -= this.WebRequestOperationCompleted;
					this.WebRequestOperationCompleted(this.m_RequestOperation);
				}
			}
			if (!this.m_Completed && this.m_Source == BundleSource.Local && !this.m_RequestCompletedCallbackCalled)
			{
				this.m_RequestOperation.completed -= this.LocalRequestOperationCompleted;
				this.LocalRequestOperationCompleted(this.m_RequestOperation);
			}
			if (!this.m_Completed && this.m_RequestOperation.isDone)
			{
				this.m_ProvideHandle.Complete<AssetBundleResource>(this, this.m_AssetBundle != null, null);
				this.m_Completed = true;
			}
			return this.m_Completed;
		}

		// Token: 0x06000147 RID: 327 RVA: 0x00005DF9 File Offset: 0x00003FF9
		private void AddCallbackInvokeIfDone(AsyncOperation operation, Action<AsyncOperation> callback)
		{
			if (operation.isDone)
			{
				callback(operation);
				return;
			}
			operation.completed += callback;
		}

		// Token: 0x06000148 RID: 328 RVA: 0x00005E12 File Offset: 0x00004012
		public static void GetLoadInfo(ProvideHandle handle, out AssetBundleResource.LoadType loadType, out string path)
		{
			AssetBundleResource.GetLoadInfo(handle.Location, handle.ResourceManager, out loadType, out path);
		}

		// Token: 0x06000149 RID: 329 RVA: 0x00005E2C File Offset: 0x0000402C
		internal static void GetLoadInfo(IResourceLocation location, ResourceManager resourceManager, out AssetBundleResource.LoadType loadType, out string path)
		{
			AssetBundleRequestOptions assetBundleRequestOptions = ((location != null) ? location.Data : null) as AssetBundleRequestOptions;
			if (assetBundleRequestOptions == null)
			{
				loadType = AssetBundleResource.LoadType.None;
				path = null;
				return;
			}
			path = resourceManager.TransformInternalId(location);
			if (Application.platform == RuntimePlatform.Android && path.StartsWith("jar:", StringComparison.Ordinal))
			{
				loadType = (assetBundleRequestOptions.UseUnityWebRequestForLocalBundles ? AssetBundleResource.LoadType.Web : AssetBundleResource.LoadType.Local);
			}
			else if (ResourceManagerConfig.ShouldPathUseWebRequest(path))
			{
				loadType = AssetBundleResource.LoadType.Web;
			}
			else if (assetBundleRequestOptions.UseUnityWebRequestForLocalBundles)
			{
				path = "file:///" + Path.GetFullPath(path);
				loadType = AssetBundleResource.LoadType.Web;
			}
			else
			{
				loadType = AssetBundleResource.LoadType.Local;
			}
			if (loadType == AssetBundleResource.LoadType.Web)
			{
				path = path.Replace('\\', '/');
			}
		}

		// Token: 0x0600014A RID: 330 RVA: 0x00005ECC File Offset: 0x000040CC
		private void BeginOperation()
		{
			this.m_DownloadedBytes = 0L;
			this.m_RequestCompletedCallbackCalled = false;
			AssetBundleResource.LoadType loadType;
			AssetBundleResource.GetLoadInfo(this.m_ProvideHandle, out loadType, out this.m_TransformedInternalId);
			if (loadType == AssetBundleResource.LoadType.Local)
			{
				if (this.m_ProvideHandle.Location is DownloadOnlyLocation)
				{
					this.m_Source = BundleSource.Local;
					this.m_RequestOperation = null;
					this.m_ProvideHandle.Complete<AssetBundleResource>(null, true, null);
					this.m_Completed = true;
					return;
				}
				this.LoadLocalBundle();
				return;
			}
			else
			{
				if (loadType == AssetBundleResource.LoadType.Web)
				{
					this.m_WebRequestQueueOperation = this.EnqueueWebRequest(this.m_TransformedInternalId);
					this.AddBeginWebRequestHandler(this.m_WebRequestQueueOperation);
					return;
				}
				this.m_Source = BundleSource.None;
				this.m_RequestOperation = null;
				this.m_ProvideHandle.Complete<AssetBundleResource>(null, false, new RemoteProviderException(string.Format("Invalid path in AssetBundleProvider: '{0}'.", this.m_TransformedInternalId), this.m_ProvideHandle.Location, null, null));
				this.m_Completed = true;
				return;
			}
		}

		// Token: 0x0600014B RID: 331 RVA: 0x00005FA4 File Offset: 0x000041A4
		private void LoadLocalBundle()
		{
			this.m_Source = BundleSource.Local;
			this.m_RequestOperation = AssetBundle.LoadFromFileAsync(this.m_TransformedInternalId, (this.m_Options == null) ? 0U : this.m_Options.Crc);
			this.AddBundleToProfiler(ContentStatus.Loading, this.m_Source);
			this.AddCallbackInvokeIfDone(this.m_RequestOperation, new Action<AsyncOperation>(this.LocalRequestOperationCompleted));
		}

		// Token: 0x0600014C RID: 332 RVA: 0x00006005 File Offset: 0x00004205
		internal WebRequestQueueOperation EnqueueWebRequest(string internalId)
		{
			UnityWebRequest unityWebRequest = this.CreateWebRequest(internalId);
			((DownloadHandlerAssetBundle)unityWebRequest.downloadHandler).autoLoadAssetBundle = !(this.m_ProvideHandle.Location is DownloadOnlyLocation);
			unityWebRequest.disposeDownloadHandlerOnDispose = false;
			return WebRequestQueue.QueueRequest(unityWebRequest);
		}

		// Token: 0x0600014D RID: 333 RVA: 0x00006040 File Offset: 0x00004240
		internal void AddBeginWebRequestHandler(WebRequestQueueOperation webRequestQueueOperation)
		{
			if (webRequestQueueOperation.IsDone)
			{
				this.BeginWebRequestOperation(webRequestQueueOperation.Result);
				return;
			}
			this.AddBundleToProfiler(ContentStatus.Queue, this.m_Source);
			webRequestQueueOperation.OnComplete = (Action<UnityWebRequestAsyncOperation>)Delegate.Combine(webRequestQueueOperation.OnComplete, new Action<UnityWebRequestAsyncOperation>(delegate(UnityWebRequestAsyncOperation asyncOp)
			{
				this.BeginWebRequestOperation(asyncOp);
			}));
		}

		// Token: 0x0600014E RID: 334 RVA: 0x00006094 File Offset: 0x00004294
		private void BeginWebRequestOperation(AsyncOperation asyncOp)
		{
			this.m_TimeoutTimer = 0f;
			this.m_TimeoutOverFrames = 0;
			this.m_LastDownloadedByteCount = 0UL;
			this.m_RequestOperation = asyncOp;
			if (this.m_RequestOperation == null || this.m_RequestOperation.isDone)
			{
				this.WebRequestOperationCompleted(this.m_RequestOperation);
				return;
			}
			if (this.m_Options.Timeout > 0)
			{
				this.m_ProvideHandle.ResourceManager.AddUpdateReceiver(this);
			}
			this.AddBundleToProfiler((this.m_Source == BundleSource.Cache) ? ContentStatus.Loading : ContentStatus.Downloading, this.m_Source);
			this.m_RequestOperation.completed += this.WebRequestOperationCompleted;
		}

		// Token: 0x0600014F RID: 335 RVA: 0x00006134 File Offset: 0x00004334
		public void Update(float unscaledDeltaTime)
		{
			if (this.m_RequestOperation != null)
			{
				UnityWebRequestAsyncOperation unityWebRequestAsyncOperation = this.m_RequestOperation as UnityWebRequestAsyncOperation;
				if (unityWebRequestAsyncOperation != null && !unityWebRequestAsyncOperation.isDone)
				{
					if (this.m_LastDownloadedByteCount != unityWebRequestAsyncOperation.webRequest.downloadedBytes)
					{
						this.m_TimeoutTimer = 0f;
						this.m_TimeoutOverFrames = 0;
						this.m_LastDownloadedByteCount = unityWebRequestAsyncOperation.webRequest.downloadedBytes;
						this.m_LastFrameCount = -1;
						this.m_TimeSecSinceLastUpdate = 0f;
						return;
					}
					float num = unscaledDeltaTime;
					if (this.m_LastFrameCount == Time.frameCount)
					{
						num = Time.realtimeSinceStartup - this.m_TimeSecSinceLastUpdate;
					}
					this.m_TimeoutTimer += num;
					if (this.HasTimedOut)
					{
						unityWebRequestAsyncOperation.webRequest.Abort();
					}
					this.m_TimeoutOverFrames++;
					this.m_LastFrameCount = Time.frameCount;
					this.m_TimeSecSinceLastUpdate = Time.realtimeSinceStartup;
				}
			}
		}

		// Token: 0x06000150 RID: 336 RVA: 0x00006213 File Offset: 0x00004413
		private void LocalRequestOperationCompleted(AsyncOperation op)
		{
			if (this.m_RequestCompletedCallbackCalled)
			{
				return;
			}
			this.m_RequestCompletedCallbackCalled = true;
			this.CompleteBundleLoad((op as AssetBundleCreateRequest).assetBundle);
		}

		// Token: 0x06000151 RID: 337 RVA: 0x00006238 File Offset: 0x00004438
		private void CompleteBundleLoad(AssetBundle bundle)
		{
			this.m_AssetBundle = bundle;
			this.AddBundleToProfiler(ContentStatus.Active, this.m_Source);
			if (this.m_AssetBundle != null)
			{
				this.m_ProvideHandle.Complete<AssetBundleResource>(this, true, null);
			}
			else
			{
				this.m_ProvideHandle.Complete<AssetBundleResource>(null, false, new RemoteProviderException(string.Format("Invalid path in AssetBundleProvider: '{0}'.", this.m_TransformedInternalId), this.m_ProvideHandle.Location, null, null));
			}
			this.m_Completed = true;
		}

		// Token: 0x06000152 RID: 338 RVA: 0x000062B4 File Offset: 0x000044B4
		private void WebRequestOperationCompleted(AsyncOperation op)
		{
			if (this.m_RequestCompletedCallbackCalled)
			{
				return;
			}
			this.m_RequestCompletedCallbackCalled = true;
			if (this.m_Options.Timeout > 0)
			{
				this.m_ProvideHandle.ResourceManager.RemoveUpdateReciever(this);
			}
			UnityWebRequestAsyncOperation unityWebRequestAsyncOperation = op as UnityWebRequestAsyncOperation;
			UnityWebRequest unityWebRequest = ((unityWebRequestAsyncOperation != null) ? unityWebRequestAsyncOperation.webRequest : null);
			DownloadHandlerAssetBundle downloadHandlerAssetBundle = ((unityWebRequest != null) ? unityWebRequest.downloadHandler : null) as DownloadHandlerAssetBundle;
			UnityWebRequestResult unityWebRequestResult = null;
			if (unityWebRequest != null && !UnityWebRequestUtilities.RequestHasErrors(unityWebRequest, out unityWebRequestResult))
			{
				if (!this.m_Completed)
				{
					if (!(this.m_ProvideHandle.Location is DownloadOnlyLocation))
					{
						this.m_AssetBundle = downloadHandlerAssetBundle.assetBundle;
					}
					this.AddBundleToProfiler(ContentStatus.Active, this.m_Source);
					downloadHandlerAssetBundle.Dispose();
					this.m_ProvideHandle.Complete<AssetBundleResource>(this, true, null);
					this.m_Completed = true;
				}
				if (!string.IsNullOrEmpty(this.m_Options.Hash) && this.m_Options.ClearOtherCachedVersionsWhenLoaded)
				{
					Caching.ClearOtherCachedVersions(this.m_Options.BundleName, Hash128.Parse(this.m_Options.Hash));
				}
			}
			else
			{
				if (this.HasTimedOut)
				{
					unityWebRequestResult.Error = "Request timeout";
				}
				unityWebRequest = this.m_WebRequestQueueOperation.m_WebRequest;
				if (unityWebRequestResult == null)
				{
					unityWebRequestResult = new UnityWebRequestResult(this.m_WebRequestQueueOperation.m_WebRequest);
				}
				downloadHandlerAssetBundle = unityWebRequest.downloadHandler as DownloadHandlerAssetBundle;
				downloadHandlerAssetBundle.Dispose();
				bool flag = false;
				string text = string.Format("Web request failed, retrying ({0}/{1})...\n{2}", this.m_Retries, this.m_Options.RetryCount, unityWebRequestResult);
				if (!string.IsNullOrEmpty(this.m_Options.Hash) && this.m_Source == BundleSource.Cache)
				{
					text = string.Format("Web request failed to load from cache. The cached AssetBundle will be cleared from the cache and re-downloaded. Retrying...\n{0}", unityWebRequestResult);
					Caching.ClearCachedVersion(this.m_Options.BundleName, Hash128.Parse(this.m_Options.Hash));
					if (this.m_Retries == 0 && unityWebRequestResult.ShouldRetryDownloadError())
					{
						Debug.LogFormat(text, Array.Empty<object>());
						this.BeginOperation();
						this.m_Retries++;
						flag = true;
					}
				}
				if (!flag)
				{
					if (this.m_Retries < this.m_Options.RetryCount && unityWebRequestResult.ShouldRetryDownloadError())
					{
						this.m_Retries++;
						Debug.LogFormat(text, Array.Empty<object>());
						this.BeginOperation();
					}
					else
					{
						RemoteProviderException ex = new RemoteProviderException("Unable to load asset bundle from : " + unityWebRequest.url, this.m_ProvideHandle.Location, unityWebRequestResult, null);
						this.m_ProvideHandle.Complete<AssetBundleResource>(null, false, ex);
						this.m_Completed = true;
						this.RemoveBundleFromProfiler();
					}
				}
			}
			unityWebRequest.Dispose();
		}

		// Token: 0x06000153 RID: 339 RVA: 0x0000653B File Offset: 0x0000473B
		public bool Unload(out AssetBundleUnloadOperation unloadOp)
		{
			unloadOp = null;
			if (this.m_AssetBundle != null)
			{
				unloadOp = this.m_AssetBundle.UnloadAsync(true);
				this.m_AssetBundle = null;
			}
			this.m_RequestOperation = null;
			this.RemoveBundleFromProfiler();
			return unloadOp != null;
		}

		// Token: 0x0400007A RID: 122
		private AssetBundle m_AssetBundle;

		// Token: 0x0400007B RID: 123
		private AsyncOperation m_RequestOperation;

		// Token: 0x0400007C RID: 124
		internal WebRequestQueueOperation m_WebRequestQueueOperation;

		// Token: 0x0400007D RID: 125
		internal ProvideHandle m_ProvideHandle;

		// Token: 0x0400007E RID: 126
		internal AssetBundleRequestOptions m_Options;

		// Token: 0x0400007F RID: 127
		[NonSerialized]
		private bool m_RequestCompletedCallbackCalled;

		// Token: 0x04000080 RID: 128
		private int m_Retries;

		// Token: 0x04000081 RID: 129
		private BundleSource m_Source;

		// Token: 0x04000082 RID: 130
		private long m_BytesToDownload;

		// Token: 0x04000083 RID: 131
		private long m_DownloadedBytes;

		// Token: 0x04000084 RID: 132
		private bool m_Completed;

		// Token: 0x04000085 RID: 133
		private AssetBundleUnloadOperation m_UnloadOperation;

		// Token: 0x04000086 RID: 134
		private const int k_WaitForWebRequestMainThreadSleep = 1;

		// Token: 0x04000087 RID: 135
		private string m_TransformedInternalId;

		// Token: 0x04000088 RID: 136
		private AssetBundleRequest m_PreloadRequest;

		// Token: 0x04000089 RID: 137
		private bool m_PreloadCompleted;

		// Token: 0x0400008A RID: 138
		private ulong m_LastDownloadedByteCount;

		// Token: 0x0400008B RID: 139
		private float m_TimeoutTimer;

		// Token: 0x0400008C RID: 140
		private int m_TimeoutOverFrames;

		// Token: 0x0400008D RID: 141
		private int m_LastFrameCount = -1;

		// Token: 0x0400008E RID: 142
		private float m_TimeSecSinceLastUpdate;

		// Token: 0x02000073 RID: 115
		public enum LoadType
		{
			// Token: 0x04000157 RID: 343
			None,
			// Token: 0x04000158 RID: 344
			Local,
			// Token: 0x04000159 RID: 345
			Web
		}
	}
}
