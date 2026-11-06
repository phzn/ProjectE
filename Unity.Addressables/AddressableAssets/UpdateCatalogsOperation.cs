using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.AddressableAssets.ResourceLocators;
using UnityEngine.AddressableAssets.ResourceProviders;
using UnityEngine.ResourceManagement;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceLocations;
using UnityEngine.ResourceManagement.ResourceProviders;

namespace UnityEngine.AddressableAssets
{
	// Token: 0x02000017 RID: 23
	internal class UpdateCatalogsOperation : AsyncOperationBase<List<IResourceLocator>>
	{
		// Token: 0x0600016D RID: 365 RVA: 0x0000652C File Offset: 0x0000472C
		public UpdateCatalogsOperation(AddressablesImpl aa)
		{
			this.m_Addressables = aa;
		}

		// Token: 0x0600016E RID: 366 RVA: 0x0000653C File Offset: 0x0000473C
		public AsyncOperationHandle<List<IResourceLocator>> Start(IEnumerable<string> catalogIds, bool autoCleanBundleCache)
		{
			this.m_LocatorInfos = new List<ResourceLocatorInfo>();
			List<IResourceLocation> list = new List<IResourceLocation>();
			foreach (string text in catalogIds)
			{
				if (text != null)
				{
					ResourceLocatorInfo locatorInfo = this.m_Addressables.GetLocatorInfo(text);
					list.Add(locatorInfo.CatalogLocation);
					this.m_LocatorInfos.Add(locatorInfo);
				}
			}
			if (list.Count == 0)
			{
				return this.m_Addressables.ResourceManager.CreateCompletedOperation<List<IResourceLocator>>(null, "Content update not available.");
			}
			ContentCatalogProvider contentCatalogProvider = this.m_Addressables.ResourceManager.ResourceProviders.FirstOrDefault<IResourceProvider>((IResourceProvider rp) => rp.GetType() == typeof(ContentCatalogProvider)) as ContentCatalogProvider;
			if (contentCatalogProvider != null)
			{
				contentCatalogProvider.DisableCatalogUpdateOnStart = false;
			}
			this.m_DepOp = this.m_Addressables.ResourceManager.CreateGroupOperation<object>(list);
			this.m_AutoCleanBundleCache = autoCleanBundleCache;
			return this.m_Addressables.ResourceManager.StartOperation<List<IResourceLocator>>(this, this.m_DepOp);
		}

		// Token: 0x0600016F RID: 367 RVA: 0x00006658 File Offset: 0x00004858
		protected override bool InvokeWaitForCompletion()
		{
			if (base.IsDone)
			{
				return true;
			}
			if (this.m_DepOp.IsValid() && !this.m_DepOp.IsDone)
			{
				this.m_DepOp.WaitForCompletion();
			}
			ResourceManager rm = this.m_RM;
			if (rm != null)
			{
				rm.Update(Time.unscaledDeltaTime);
			}
			if (!this.HasExecuted)
			{
				base.InvokeExecute();
			}
			if (this.m_CleanCacheOp.IsValid() && !this.m_CleanCacheOp.IsDone)
			{
				this.m_CleanCacheOp.WaitForCompletion();
			}
			this.m_Addressables.ResourceManager.Update(Time.unscaledDeltaTime);
			return base.IsDone;
		}

		// Token: 0x06000170 RID: 368 RVA: 0x000066FA File Offset: 0x000048FA
		protected override void Destroy()
		{
			this.m_DepOp.Release();
		}

		// Token: 0x06000171 RID: 369 RVA: 0x00006707 File Offset: 0x00004907
		public override void GetDependencies(List<AsyncOperationHandle> dependencies)
		{
			dependencies.Add(this.m_DepOp);
		}

		// Token: 0x06000172 RID: 370 RVA: 0x0000671C File Offset: 0x0000491C
		protected override void Execute()
		{
			List<IResourceLocator> list = new List<IResourceLocator>(this.m_DepOp.Result.Count);
			for (int i = 0; i < this.m_DepOp.Result.Count; i++)
			{
				IResourceLocator resourceLocator = this.m_DepOp.Result[i].Result as IResourceLocator;
				string text = null;
				IResourceLocation resourceLocation = null;
				if (resourceLocator == null)
				{
					ContentCatalogData contentCatalogData = this.m_DepOp.Result[i].Result as ContentCatalogData;
					resourceLocator = contentCatalogData.CreateCustomLocator(contentCatalogData.location.PrimaryKey, null);
					text = contentCatalogData.LocalHash;
					resourceLocation = contentCatalogData.location;
				}
				this.m_LocatorInfos[i].UpdateContent(resourceLocator, text, resourceLocation);
				list.Add(this.m_LocatorInfos[i].Locator);
			}
			if (this.m_AutoCleanBundleCache)
			{
				this.m_CleanCacheOp = this.m_Addressables.CleanBundleCache(this.m_DepOp, false);
				this.OnCleanCacheCompleted(this.m_CleanCacheOp, list);
				return;
			}
			if (this.m_DepOp.Status == AsyncOperationStatus.Succeeded)
			{
				base.Complete(list, true, null);
				return;
			}
			if (this.m_DepOp.Status == AsyncOperationStatus.Failed)
			{
				base.Complete(list, false, "Cannot update catalogs. Failed to load catalog: " + this.m_DepOp.OperationException.Message);
				return;
			}
			string text2 = "Cannot update catalogs. Catalog loading operation is still in progress when it should already be completed. ";
			text2 += ((this.m_DepOp.OperationException != null) ? this.m_DepOp.OperationException.Message : "");
			base.Complete(list, false, text2);
		}

		// Token: 0x06000173 RID: 371 RVA: 0x000068AC File Offset: 0x00004AAC
		private void OnCleanCacheCompleted(AsyncOperationHandle<bool> handle, List<IResourceLocator> catalogs)
		{
			handle.Completed += delegate(AsyncOperationHandle<bool> obj)
			{
				bool flag = obj.Status == AsyncOperationStatus.Succeeded;
				this.Complete(catalogs, flag, flag ? null : string.Format("{0}, status={1}, result={2} catalogs updated, but failed to clean bundle cache.", obj.DebugName, obj.Status, obj.Result));
			};
		}

		// Token: 0x04000036 RID: 54
		private AddressablesImpl m_Addressables;

		// Token: 0x04000037 RID: 55
		private List<ResourceLocatorInfo> m_LocatorInfos;

		// Token: 0x04000038 RID: 56
		internal AsyncOperationHandle<IList<AsyncOperationHandle>> m_DepOp;

		// Token: 0x04000039 RID: 57
		private AsyncOperationHandle<bool> m_CleanCacheOp;

		// Token: 0x0400003A RID: 58
		private bool m_AutoCleanBundleCache;
	}
}
