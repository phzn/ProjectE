using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using UnityEngine.AddressableAssets.ResourceLocators;
using UnityEngine.Networking;
using UnityEngine.ResourceManagement;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.Profiling;
using UnityEngine.ResourceManagement.ResourceLocations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.ResourceManagement.Util;

namespace UnityEngine.AddressableAssets.ResourceProviders
{
	// Token: 0x0200001F RID: 31
	[DisplayName("Content Catalog Provider")]
	public class ContentCatalogProvider : ResourceProviderBase
	{
		// Token: 0x06000190 RID: 400 RVA: 0x00007574 File Offset: 0x00005774
		public ContentCatalogProvider(ResourceManager resourceManagerInstance)
		{
			this.m_BehaviourFlags = ProviderBehaviourFlags.CanProvideWithFailedDependencies;
		}

		// Token: 0x06000191 RID: 401 RVA: 0x0000758E File Offset: 0x0000578E
		public override void Release(IResourceLocation location, object obj)
		{
			if (this.m_LocationToCatalogLoadOpMap.ContainsKey(location))
			{
				this.m_LocationToCatalogLoadOpMap[location].Release();
				this.m_LocationToCatalogLoadOpMap.Remove(location);
			}
			base.Release(location, obj);
		}

		// Token: 0x06000192 RID: 402 RVA: 0x000075C4 File Offset: 0x000057C4
		public override void Provide(ProvideHandle providerInterface)
		{
			if (!this.m_LocationToCatalogLoadOpMap.ContainsKey(providerInterface.Location))
			{
				this.m_LocationToCatalogLoadOpMap.Add(providerInterface.Location, new ContentCatalogProvider.InternalOp());
			}
			this.m_LocationToCatalogLoadOpMap[providerInterface.Location].Start(providerInterface, this.DisableCatalogUpdateOnStart, this.IsLocalCatalogInBundle);
		}

		// Token: 0x04000051 RID: 81
		public bool DisableCatalogUpdateOnStart;

		// Token: 0x04000052 RID: 82
		public bool IsLocalCatalogInBundle;

		// Token: 0x04000053 RID: 83
		internal Dictionary<IResourceLocation, ContentCatalogProvider.InternalOp> m_LocationToCatalogLoadOpMap = new Dictionary<IResourceLocation, ContentCatalogProvider.InternalOp>();

		// Token: 0x02000053 RID: 83
		public enum DependencyHashIndex
		{
			// Token: 0x04000115 RID: 277
			Remote,
			// Token: 0x04000116 RID: 278
			Cache,
			// Token: 0x04000117 RID: 279
			Count
		}

		// Token: 0x02000054 RID: 84
		internal class InternalOp
		{
			// Token: 0x0600025C RID: 604 RVA: 0x0000980C File Offset: 0x00007A0C
			public void Start(ProvideHandle providerInterface, bool disableCatalogUpdateOnStart, bool isLocalCatalogInBundle)
			{
				this.m_ProviderInterface = providerInterface;
				this.m_DisableCatalogUpdateOnStart = disableCatalogUpdateOnStart;
				this.m_IsLocalCatalogInBundle = isLocalCatalogInBundle;
				this.m_ProviderInterface.SetWaitForCompletionCallback(new Func<bool>(this.WaitForCompletionCallback));
				this.m_LocalDataPath = null;
				this.m_RemoteHashValue = null;
				List<object> list = new List<object>();
				this.m_ProviderInterface.GetDependencies(list);
				string text = this.DetermineIdToLoad(this.m_ProviderInterface.Location, list, disableCatalogUpdateOnStart);
				bool flag = isLocalCatalogInBundle && this.CanLoadCatalogFromBundle(text, this.m_ProviderInterface.Location);
				this.LoadCatalog(text, flag);
			}

			// Token: 0x0600025D RID: 605 RVA: 0x0000989C File Offset: 0x00007A9C
			private bool WaitForCompletionCallback()
			{
				if (this.m_ContentCatalogData != null)
				{
					return true;
				}
				bool flag;
				if (this.m_BundledCatalog != null)
				{
					flag = this.m_BundledCatalog.WaitForCompletion();
				}
				else
				{
					flag = this.m_ContentCatalogDataLoadOp.IsDone;
					if (!flag)
					{
						this.m_ContentCatalogDataLoadOp.WaitForCompletion();
					}
				}
				if (flag && this.m_ContentCatalogData == null)
				{
					this.m_ProviderInterface.ResourceManager.Update(Time.unscaledDeltaTime);
				}
				return flag;
			}

			// Token: 0x0600025E RID: 606 RVA: 0x00009905 File Offset: 0x00007B05
			public void Release()
			{
				ContentCatalogData contentCatalogData = this.m_ContentCatalogData;
				if (contentCatalogData == null)
				{
					return;
				}
				contentCatalogData.CleanData();
			}

			// Token: 0x0600025F RID: 607 RVA: 0x00009917 File Offset: 0x00007B17
			internal bool CanLoadCatalogFromBundle(string idToLoad, IResourceLocation location)
			{
				return Path.GetExtension(idToLoad) == ".bundle" && idToLoad.Equals(this.GetTransformedInternalId(location));
			}

			// Token: 0x06000260 RID: 608 RVA: 0x0000993C File Offset: 0x00007B3C
			internal void LoadCatalog(string idToLoad, bool loadCatalogFromLocalBundle)
			{
				try
				{
					ProviderLoadRequestOptions providerLoadRequestOptions = null;
					ProviderLoadRequestOptions providerLoadRequestOptions2 = this.m_ProviderInterface.Location.Data as ProviderLoadRequestOptions;
					if (providerLoadRequestOptions2 != null)
					{
						providerLoadRequestOptions = providerLoadRequestOptions2.Copy();
					}
					if (loadCatalogFromLocalBundle)
					{
						int num = ((providerLoadRequestOptions != null) ? providerLoadRequestOptions.WebRequestTimeout : 0);
						this.m_BundledCatalog = new ContentCatalogProvider.InternalOp.BundledCatalog(idToLoad, num);
						this.m_BundledCatalog.OnLoaded += delegate(ContentCatalogData ccd)
						{
							this.m_ContentCatalogData = ccd;
							this.OnCatalogLoaded(ccd);
						};
						this.m_BundledCatalog.LoadCatalogFromBundleAsync();
					}
					else
					{
						ResourceLocationBase resourceLocationBase = new ResourceLocationBase(idToLoad, idToLoad, typeof(JsonAssetProvider).FullName, typeof(ContentCatalogData), Array.Empty<IResourceLocation>());
						resourceLocationBase.Data = providerLoadRequestOptions;
						this.m_ContentCatalogDataLoadOp = this.m_ProviderInterface.ResourceManager.ProvideResource<ContentCatalogData>(resourceLocationBase);
						this.m_ContentCatalogDataLoadOp.Completed += this.CatalogLoadOpCompleteCallback;
					}
				}
				catch (Exception ex)
				{
					this.m_ProviderInterface.Complete<ContentCatalogData>(null, false, ex);
				}
			}

			// Token: 0x06000261 RID: 609 RVA: 0x00009A2C File Offset: 0x00007C2C
			private void CatalogLoadOpCompleteCallback(AsyncOperationHandle<ContentCatalogData> op)
			{
				this.m_ContentCatalogData = op.Result;
				op.Release();
				this.OnCatalogLoaded(this.m_ContentCatalogData);
			}

			// Token: 0x06000262 RID: 610 RVA: 0x00009A4E File Offset: 0x00007C4E
			private string GetTransformedInternalId(IResourceLocation loc)
			{
				if (this.m_ProviderInterface.ResourceManager == null)
				{
					return loc.InternalId;
				}
				return this.m_ProviderInterface.ResourceManager.TransformInternalId(loc);
			}

			// Token: 0x06000263 RID: 611 RVA: 0x00009A78 File Offset: 0x00007C78
			internal string DetermineIdToLoad(IResourceLocation location, IList<object> dependencyObjects, bool disableCatalogUpdateOnStart = false)
			{
				string text = this.GetTransformedInternalId(location);
				if (dependencyObjects != null && location.Dependencies != null && dependencyObjects.Count == 2 && location.Dependencies.Count == 2)
				{
					string text2 = dependencyObjects[0] as string;
					this.m_LocalHashValue = dependencyObjects[1] as string;
					if (string.IsNullOrEmpty(text2) || disableCatalogUpdateOnStart)
					{
						if (!string.IsNullOrEmpty(this.m_LocalHashValue) && !this.m_Retried && !string.IsNullOrEmpty(Application.persistentDataPath))
						{
							text = this.GetTransformedInternalId(location.Dependencies[1]).Replace(".hash", ".json");
						}
						else
						{
							this.m_LocalHashValue = Hash128.Compute(text).ToString();
						}
					}
					else if (text2 == this.m_LocalHashValue && !this.m_Retried)
					{
						text = this.GetTransformedInternalId(location.Dependencies[1]).Replace(".hash", ".json");
					}
					else
					{
						text = this.GetTransformedInternalId(location.Dependencies[0]).Replace(".hash", ".json");
						this.m_RemoteHashValue = text2;
						if (!string.IsNullOrEmpty(Application.persistentDataPath))
						{
							this.m_LocalDataPath = this.GetTransformedInternalId(location.Dependencies[1]).Replace(".hash", ".json");
						}
					}
				}
				return text;
			}

			// Token: 0x06000264 RID: 612 RVA: 0x00009BE4 File Offset: 0x00007DE4
			private void OnCatalogLoaded(ContentCatalogData ccd)
			{
				if (ccd != null)
				{
					ProfilerRuntime.AddCatalog(Hash128.Parse(ccd.m_BuildResultHash));
					ccd.location = this.m_ProviderInterface.Location;
					ccd.LocalHash = this.m_LocalHashValue;
					if (!string.IsNullOrEmpty(this.m_RemoteHashValue) && !string.IsNullOrEmpty(this.m_LocalDataPath))
					{
						string directoryName = Path.GetDirectoryName(this.m_LocalDataPath);
						string localDataPath = this.m_LocalDataPath;
						try
						{
							if (!string.IsNullOrEmpty(directoryName) && !Directory.Exists(directoryName))
							{
								Directory.CreateDirectory(directoryName);
							}
							File.WriteAllText(localDataPath, JsonUtility.ToJson(ccd));
							File.WriteAllText(localDataPath.Replace(".json", ".hash"), this.m_RemoteHashValue);
						}
						catch (UnauthorizedAccessException ex)
						{
							Addressables.LogWarning("Did not save cached content catalog. Missing access permissions for location " + localDataPath + " : " + ex.Message);
							this.m_ProviderInterface.Complete<ContentCatalogData>(ccd, true, null);
							return;
						}
						catch (Exception ex2)
						{
							string transformedInternalId = this.GetTransformedInternalId(this.m_ProviderInterface.Location.Dependencies[0]);
							string text = string.Concat(new string[] { "Unable to load ContentCatalogData from location ", transformedInternalId, ". Failed to cache catalog to location ", localDataPath, "." });
							ccd = null;
							this.m_ProviderInterface.Complete<ContentCatalogData>(ccd, false, new Exception(text, ex2));
							return;
						}
						ccd.LocalHash = this.m_RemoteHashValue;
					}
					else if (string.IsNullOrEmpty(this.m_LocalDataPath) && string.IsNullOrEmpty(Application.persistentDataPath))
					{
						Addressables.LogWarning("Did not save cached content catalog because Application.persistentDataPath is an empty path.");
					}
					this.m_ProviderInterface.Complete<ContentCatalogData>(ccd, true, null);
					return;
				}
				string text2 = string.Format("Unable to load ContentCatalogData from location {0}", this.m_ProviderInterface.Location);
				if (!this.m_Retried)
				{
					this.m_Retried = true;
					string transformedInternalId2 = this.GetTransformedInternalId(this.m_ProviderInterface.Location.Dependencies[1]);
					if (this.m_ContentCatalogDataLoadOp.LocationName == transformedInternalId2.Replace(".hash", ".json"))
					{
						try
						{
							File.Delete(transformedInternalId2);
						}
						catch (Exception)
						{
							text2 = text2 + ". Unable to delete cache data from location " + transformedInternalId2;
							this.m_ProviderInterface.Complete<ContentCatalogData>(ccd, false, new Exception(text2));
							return;
						}
					}
					Addressables.LogWarning(text2 + ". Attempting to retry...");
					this.Start(this.m_ProviderInterface, this.m_DisableCatalogUpdateOnStart, this.m_IsLocalCatalogInBundle);
					return;
				}
				this.m_ProviderInterface.Complete<ContentCatalogData>(ccd, false, new Exception(text2 + " on second attempt."));
			}

			// Token: 0x04000118 RID: 280
			private string m_LocalDataPath;

			// Token: 0x04000119 RID: 281
			private string m_RemoteHashValue;

			// Token: 0x0400011A RID: 282
			internal string m_LocalHashValue;

			// Token: 0x0400011B RID: 283
			private ProvideHandle m_ProviderInterface;

			// Token: 0x0400011C RID: 284
			internal ContentCatalogData m_ContentCatalogData;

			// Token: 0x0400011D RID: 285
			private AsyncOperationHandle<ContentCatalogData> m_ContentCatalogDataLoadOp;

			// Token: 0x0400011E RID: 286
			private ContentCatalogProvider.InternalOp.BundledCatalog m_BundledCatalog;

			// Token: 0x0400011F RID: 287
			private bool m_Retried;

			// Token: 0x04000120 RID: 288
			private bool m_DisableCatalogUpdateOnStart;

			// Token: 0x04000121 RID: 289
			private bool m_IsLocalCatalogInBundle;

			// Token: 0x04000122 RID: 290
			private const string kCatalogExt = ".json";

			// Token: 0x0200005F RID: 95
			internal class BundledCatalog
			{
				// Token: 0x14000001 RID: 1
				// (add) Token: 0x06000284 RID: 644 RVA: 0x0000A110 File Offset: 0x00008310
				// (remove) Token: 0x06000285 RID: 645 RVA: 0x0000A148 File Offset: 0x00008348
				public event Action<ContentCatalogData> OnLoaded;

				// Token: 0x1700006F RID: 111
				// (get) Token: 0x06000286 RID: 646 RVA: 0x0000A17D File Offset: 0x0000837D
				public bool OpInProgress
				{
					get
					{
						return this.m_OpInProgress;
					}
				}

				// Token: 0x17000070 RID: 112
				// (get) Token: 0x06000287 RID: 647 RVA: 0x0000A185 File Offset: 0x00008385
				public bool OpIsSuccess
				{
					get
					{
						return !this.m_OpInProgress && this.m_CatalogData != null;
					}
				}

				// Token: 0x06000288 RID: 648 RVA: 0x0000A19C File Offset: 0x0000839C
				public BundledCatalog(string bundlePath, int webRequestTimeout = 0)
				{
					if (string.IsNullOrEmpty(bundlePath))
					{
						throw new ArgumentNullException("bundlePath", "Catalog bundle path is null.");
					}
					if (!bundlePath.EndsWith(".bundle", StringComparison.OrdinalIgnoreCase))
					{
						throw new ArgumentException("You must supply a valid bundle file path.");
					}
					this.m_BundlePath = bundlePath;
					this.m_WebRequestTimeout = webRequestTimeout;
				}

				// Token: 0x06000289 RID: 649 RVA: 0x0000A1F0 File Offset: 0x000083F0
				~BundledCatalog()
				{
					this.Unload();
				}

				// Token: 0x0600028A RID: 650 RVA: 0x0000A21C File Offset: 0x0000841C
				private void Unload()
				{
					AssetBundle catalogAssetBundle = this.m_CatalogAssetBundle;
					if (catalogAssetBundle != null)
					{
						catalogAssetBundle.Unload(true);
					}
					this.m_CatalogAssetBundle = null;
				}

				// Token: 0x0600028B RID: 651 RVA: 0x0000A238 File Offset: 0x00008438
				public void LoadCatalogFromBundleAsync()
				{
					if (this.m_OpInProgress)
					{
						Addressables.LogError("Operation in progress : A catalog is already being loaded. Please wait for the operation to complete.");
						return;
					}
					this.m_OpInProgress = true;
					if (!ResourceManagerConfig.ShouldPathUseWebRequest(this.m_BundlePath))
					{
						this.m_LoadBundleRequest = AssetBundle.LoadFromFileAsync(this.m_BundlePath);
						this.m_LoadBundleRequest.completed += delegate(AsyncOperation loadOp)
						{
							AssetBundleCreateRequest assetBundleCreateRequest = loadOp as AssetBundleCreateRequest;
							if (assetBundleCreateRequest != null && assetBundleCreateRequest.assetBundle != null)
							{
								this.m_CatalogAssetBundle = assetBundleCreateRequest.assetBundle;
								this.m_LoadTextAssetRequest = this.m_CatalogAssetBundle.LoadAllAssetsAsync<TextAsset>();
								if (this.m_LoadTextAssetRequest.isDone)
								{
									this.LoadTextAssetRequestComplete(this.m_LoadTextAssetRequest);
								}
								this.m_LoadTextAssetRequest.completed += this.LoadTextAssetRequestComplete;
								return;
							}
							Addressables.LogError("Unable to load dependent bundle from location : " + this.m_BundlePath);
							this.m_OpInProgress = false;
						};
						return;
					}
					UnityWebRequest assetBundle = UnityWebRequestAssetBundle.GetAssetBundle(this.m_BundlePath);
					if (this.m_WebRequestTimeout > 0)
					{
						assetBundle.timeout = this.m_WebRequestTimeout;
					}
					this.m_WebRequestQueueOperation = WebRequestQueue.QueueRequest(assetBundle);
					if (!this.m_WebRequestQueueOperation.IsDone)
					{
						WebRequestQueueOperation webRequestQueueOperation = this.m_WebRequestQueueOperation;
						webRequestQueueOperation.OnComplete = (Action<UnityWebRequestAsyncOperation>)Delegate.Combine(webRequestQueueOperation.OnComplete, new Action<UnityWebRequestAsyncOperation>(delegate(UnityWebRequestAsyncOperation asyncOp)
						{
							this.m_RequestOperation = asyncOp;
							this.m_RequestOperation.completed += this.WebRequestOperationCompleted;
						}));
						return;
					}
					this.m_RequestOperation = this.m_WebRequestQueueOperation.Result;
					if (this.m_RequestOperation.isDone)
					{
						this.WebRequestOperationCompleted(this.m_RequestOperation);
						return;
					}
					this.m_RequestOperation.completed += this.WebRequestOperationCompleted;
				}

				// Token: 0x0600028C RID: 652 RVA: 0x0000A33C File Offset: 0x0000853C
				private void WebRequestOperationCompleted(AsyncOperation op)
				{
					UnityWebRequest webRequest = (op as UnityWebRequestAsyncOperation).webRequest;
					DownloadHandlerAssetBundle downloadHandlerAssetBundle = webRequest.downloadHandler as DownloadHandlerAssetBundle;
					UnityWebRequestResult unityWebRequestResult;
					if (!UnityWebRequestUtilities.RequestHasErrors(webRequest, out unityWebRequestResult))
					{
						this.m_CatalogAssetBundle = downloadHandlerAssetBundle.assetBundle;
						this.m_LoadTextAssetRequest = this.m_CatalogAssetBundle.LoadAllAssetsAsync<TextAsset>();
						if (this.m_LoadTextAssetRequest.isDone)
						{
							this.LoadTextAssetRequestComplete(this.m_LoadTextAssetRequest);
						}
						this.m_LoadTextAssetRequest.completed += this.LoadTextAssetRequestComplete;
					}
					else
					{
						Addressables.LogError("Unable to load dependent bundle from location : " + this.m_BundlePath);
						this.m_OpInProgress = false;
					}
					webRequest.Dispose();
				}

				// Token: 0x0600028D RID: 653 RVA: 0x0000A3DC File Offset: 0x000085DC
				private void LoadTextAssetRequestComplete(AsyncOperation op)
				{
					AssetBundleRequest assetBundleRequest = op as AssetBundleRequest;
					if (assetBundleRequest != null)
					{
						TextAsset textAsset = assetBundleRequest.asset as TextAsset;
						if (textAsset != null && textAsset.text != null)
						{
							this.m_CatalogData = JsonUtility.FromJson<ContentCatalogData>(textAsset.text);
							Action<ContentCatalogData> onLoaded = this.OnLoaded;
							if (onLoaded == null)
							{
								goto IL_0060;
							}
							onLoaded(this.m_CatalogData);
							goto IL_0060;
						}
					}
					Addressables.LogError("No catalog text assets where found in bundle " + this.m_BundlePath);
					IL_0060:
					this.Unload();
					this.m_OpInProgress = false;
				}

				// Token: 0x0600028E RID: 654 RVA: 0x0000A456 File Offset: 0x00008656
				public bool WaitForCompletion()
				{
					return !(this.m_LoadBundleRequest.assetBundle == null) && (this.m_LoadTextAssetRequest.asset != null || this.m_LoadTextAssetRequest.allAssets != null);
				}

				// Token: 0x0400013F RID: 319
				private readonly string m_BundlePath;

				// Token: 0x04000140 RID: 320
				private bool m_OpInProgress;

				// Token: 0x04000141 RID: 321
				private AssetBundleCreateRequest m_LoadBundleRequest;

				// Token: 0x04000142 RID: 322
				internal AssetBundle m_CatalogAssetBundle;

				// Token: 0x04000143 RID: 323
				private AssetBundleRequest m_LoadTextAssetRequest;

				// Token: 0x04000144 RID: 324
				private ContentCatalogData m_CatalogData;

				// Token: 0x04000145 RID: 325
				private WebRequestQueueOperation m_WebRequestQueueOperation;

				// Token: 0x04000146 RID: 326
				private AsyncOperation m_RequestOperation;

				// Token: 0x04000147 RID: 327
				private int m_WebRequestTimeout;
			}
		}
	}
}
