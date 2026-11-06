using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using UnityEngine.AddressableAssets.ResourceLocators;
using UnityEngine.ResourceManagement;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceLocations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.ResourceManagement.Util;

namespace UnityEngine.AddressableAssets
{
	// Token: 0x02000016 RID: 22
	internal class CleanBundleCacheOperation : AsyncOperationBase<bool>, IUpdateReceiver
	{
		// Token: 0x06000161 RID: 353 RVA: 0x0000614B File Offset: 0x0000434B
		public CleanBundleCacheOperation(AddressablesImpl aa, bool forceSingleThreading)
		{
			this.m_Addressables = aa;
			this.m_UseMultiThreading = !forceSingleThreading && PlatformUtilities.PlatformUsesMultiThreading(Application.platform);
		}

		// Token: 0x06000162 RID: 354 RVA: 0x00006170 File Offset: 0x00004370
		public AsyncOperationHandle<bool> Start(AsyncOperationHandle<IList<AsyncOperationHandle>> depOp)
		{
			this.m_DepOp = depOp.Acquire();
			return this.m_Addressables.ResourceManager.StartOperation<bool>(this, this.m_DepOp);
		}

		// Token: 0x06000163 RID: 355 RVA: 0x0000619B File Offset: 0x0000439B
		public void CompleteInternal(bool result, bool success, string errorMsg)
		{
			this.m_DepOp.Release();
			base.Complete(result, success, errorMsg);
		}

		// Token: 0x06000164 RID: 356 RVA: 0x000061B4 File Offset: 0x000043B4
		protected override bool InvokeWaitForCompletion()
		{
			if (!this.m_DepOp.IsDone)
			{
				this.m_DepOp.WaitForCompletion();
			}
			if (!this.HasExecuted)
			{
				base.InvokeExecute();
			}
			if (this.m_EnumerationThread != null)
			{
				this.m_EnumerationThread.Join();
				this.RemoveCacheEntries();
			}
			return base.IsDone;
		}

		// Token: 0x06000165 RID: 357 RVA: 0x00006207 File Offset: 0x00004407
		protected override void Destroy()
		{
			if (this.m_DepOp.IsValid())
			{
				this.m_DepOp.Release();
			}
		}

		// Token: 0x06000166 RID: 358 RVA: 0x00006221 File Offset: 0x00004421
		public override void GetDependencies(List<AsyncOperationHandle> dependencies)
		{
			dependencies.Add(this.m_DepOp);
		}

		// Token: 0x06000167 RID: 359 RVA: 0x00006234 File Offset: 0x00004434
		protected override void Execute()
		{
			if (this.m_DepOp.Status == AsyncOperationStatus.Failed)
			{
				this.CompleteInternal(false, false, "Could not clean cache because a dependent catalog operation failed.");
				return;
			}
			HashSet<string> cacheDirsInUse = this.GetCacheDirsInUse(this.m_DepOp.Result);
			if (!Caching.ready)
			{
				this.CompleteInternal(false, false, "Cache is not ready to be accessed.");
			}
			this.m_BaseCachePath = Caching.currentCacheForWriting.path;
			if (this.m_UseMultiThreading)
			{
				this.m_EnumerationThread = new Thread(new ParameterizedThreadStart(this.DetermineCacheDirsNotInUse));
				this.m_EnumerationThread.Start(cacheDirsInUse);
				return;
			}
			this.DetermineCacheDirsNotInUse(cacheDirsInUse);
			this.RemoveCacheEntries();
		}

		// Token: 0x06000168 RID: 360 RVA: 0x000062CF File Offset: 0x000044CF
		void IUpdateReceiver.Update(float unscaledDeltaTime)
		{
			if (this.m_UseMultiThreading && !this.m_EnumerationThread.IsAlive)
			{
				this.m_EnumerationThread = null;
				this.RemoveCacheEntries();
			}
		}

		// Token: 0x06000169 RID: 361 RVA: 0x000062F4 File Offset: 0x000044F4
		private void RemoveCacheEntries()
		{
			foreach (string text in this.m_CacheDirsForRemoval)
			{
				Caching.ClearAllCachedVersions(Path.GetFileName(text));
			}
			this.CompleteInternal(true, true, null);
		}

		// Token: 0x0600016A RID: 362 RVA: 0x00006354 File Offset: 0x00004554
		private void DetermineCacheDirsNotInUse(object data)
		{
			this.DetermineCacheDirsNotInUse((HashSet<string>)data);
		}

		// Token: 0x0600016B RID: 363 RVA: 0x00006364 File Offset: 0x00004564
		private void DetermineCacheDirsNotInUse(HashSet<string> cacheDirsInUse)
		{
			this.m_CacheDirsForRemoval = new List<string>();
			if (Directory.Exists(this.m_BaseCachePath))
			{
				foreach (string text in Directory.EnumerateDirectories(this.m_BaseCachePath, "*", SearchOption.TopDirectoryOnly))
				{
					if (!cacheDirsInUse.Contains(text))
					{
						this.m_CacheDirsForRemoval.Add(text);
					}
				}
			}
		}

		// Token: 0x0600016C RID: 364 RVA: 0x000063E4 File Offset: 0x000045E4
		private HashSet<string> GetCacheDirsInUse(IList<AsyncOperationHandle> catalogOps)
		{
			HashSet<string> hashSet = new HashSet<string>();
			for (int i = 0; i < catalogOps.Count; i++)
			{
				ResourceLocationMap resourceLocationMap = catalogOps[i].Result as ResourceLocationMap;
				if (resourceLocationMap == null)
				{
					ContentCatalogData contentCatalogData = catalogOps[i].Result as ContentCatalogData;
					if (contentCatalogData == null)
					{
						return hashSet;
					}
					resourceLocationMap = contentCatalogData.CreateCustomLocator(contentCatalogData.location.PrimaryKey, null);
				}
				foreach (IList<IResourceLocation> list in resourceLocationMap.Locations.Values)
				{
					foreach (IResourceLocation resourceLocation in list)
					{
						AssetBundleRequestOptions assetBundleRequestOptions = resourceLocation.Data as AssetBundleRequestOptions;
						if (assetBundleRequestOptions != null)
						{
							AssetBundleResource.LoadType loadType;
							string text;
							AssetBundleResource.GetLoadInfo(resourceLocation, this.m_Addressables.ResourceManager, out loadType, out text);
							if (loadType == AssetBundleResource.LoadType.Web)
							{
								string text2 = Path.Combine(Caching.currentCacheForWriting.path, assetBundleRequestOptions.BundleName);
								hashSet.Add(text2);
							}
						}
					}
				}
			}
			return hashSet;
		}

		// Token: 0x04000030 RID: 48
		private AddressablesImpl m_Addressables;

		// Token: 0x04000031 RID: 49
		private AsyncOperationHandle<IList<AsyncOperationHandle>> m_DepOp;

		// Token: 0x04000032 RID: 50
		private List<string> m_CacheDirsForRemoval;

		// Token: 0x04000033 RID: 51
		private Thread m_EnumerationThread;

		// Token: 0x04000034 RID: 52
		private string m_BaseCachePath;

		// Token: 0x04000035 RID: 53
		private bool m_UseMultiThreading;
	}
}
