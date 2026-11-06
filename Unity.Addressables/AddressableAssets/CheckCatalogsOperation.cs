using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.AddressableAssets.ResourceProviders;
using UnityEngine.ResourceManagement;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceLocations;
using UnityEngine.ResourceManagement.ResourceProviders;

namespace UnityEngine.AddressableAssets
{
	// Token: 0x02000015 RID: 21
	internal class CheckCatalogsOperation : AsyncOperationBase<List<string>>
	{
		// Token: 0x0600015A RID: 346 RVA: 0x00005E07 File Offset: 0x00004007
		public CheckCatalogsOperation(AddressablesImpl aa)
		{
			this.m_Addressables = aa;
		}

		// Token: 0x0600015B RID: 347 RVA: 0x00005E18 File Offset: 0x00004018
		public AsyncOperationHandle<List<string>> Start(List<ResourceLocatorInfo> locatorInfos)
		{
			this.m_LocatorInfos = new List<ResourceLocatorInfo>(locatorInfos.Count);
			this.m_LocalHashes = new List<string>(locatorInfos.Count);
			List<IResourceLocation> list = new List<IResourceLocation>(locatorInfos.Count);
			foreach (ResourceLocatorInfo resourceLocatorInfo in locatorInfos)
			{
				if (resourceLocatorInfo.CanUpdateContent)
				{
					list.Add(resourceLocatorInfo.HashLocation);
					this.m_LocalHashes.Add(resourceLocatorInfo.LocalHash);
					this.m_LocatorInfos.Add(resourceLocatorInfo);
				}
			}
			ContentCatalogProvider contentCatalogProvider = this.m_Addressables.ResourceManager.ResourceProviders.FirstOrDefault<IResourceProvider>((IResourceProvider rp) => rp.GetType() == typeof(ContentCatalogProvider)) as ContentCatalogProvider;
			if (contentCatalogProvider != null)
			{
				contentCatalogProvider.DisableCatalogUpdateOnStart = false;
			}
			this.m_DepOp = this.m_Addressables.ResourceManager.CreateGroupOperation<string>(list);
			return this.m_Addressables.ResourceManager.StartOperation<List<string>>(this, this.m_DepOp);
		}

		// Token: 0x0600015C RID: 348 RVA: 0x00005F38 File Offset: 0x00004138
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
			ResourceManager rm2 = this.m_RM;
			if (rm2 != null)
			{
				rm2.Update(Time.unscaledDeltaTime);
			}
			return base.IsDone;
		}

		// Token: 0x0600015D RID: 349 RVA: 0x00005FB5 File Offset: 0x000041B5
		protected override void Destroy()
		{
			this.m_DepOp.Release();
		}

		// Token: 0x0600015E RID: 350 RVA: 0x00005FC2 File Offset: 0x000041C2
		public override void GetDependencies(List<AsyncOperationHandle> dependencies)
		{
			dependencies.Add(this.m_DepOp);
		}

		// Token: 0x0600015F RID: 351 RVA: 0x00005FD8 File Offset: 0x000041D8
		internal static List<string> ProcessDependentOpResults(IList<AsyncOperationHandle> results, List<ResourceLocatorInfo> locatorInfos, List<string> localHashes, out string errorString, out bool success)
		{
			List<string> list = new List<string>();
			List<string> list2 = new List<string>();
			for (int i = 0; i < results.Count; i++)
			{
				AsyncOperationHandle asyncOperationHandle = results[i];
				string text = asyncOperationHandle.Result as string;
				if (!string.IsNullOrEmpty(text) && text != localHashes[i])
				{
					list.Add(locatorInfos[i].Locator.LocatorId);
					locatorInfos[i].ContentUpdateAvailable = true;
				}
				else if (asyncOperationHandle.OperationException != null)
				{
					list.Add(null);
					locatorInfos[i].ContentUpdateAvailable = false;
					list2.Add(asyncOperationHandle.OperationException.Message);
				}
			}
			errorString = null;
			if (list2.Count > 0)
			{
				if (list2.Count == list.Count)
				{
					list = null;
					errorString = "CheckCatalogsOperation failed with the following errors: ";
				}
				else
				{
					errorString = "Partial success in CheckCatalogsOperation with the following errors: ";
				}
				foreach (string text2 in list2)
				{
					errorString = errorString + "\n" + text2;
				}
			}
			success = list2.Count == 0;
			return list;
		}

		// Token: 0x06000160 RID: 352 RVA: 0x00006114 File Offset: 0x00004314
		protected override void Execute()
		{
			string text;
			bool flag;
			List<string> list = CheckCatalogsOperation.ProcessDependentOpResults(this.m_DepOp.Result, this.m_LocatorInfos, this.m_LocalHashes, out text, out flag);
			base.Complete(list, flag, text);
		}

		// Token: 0x0400002C RID: 44
		private AddressablesImpl m_Addressables;

		// Token: 0x0400002D RID: 45
		private List<string> m_LocalHashes;

		// Token: 0x0400002E RID: 46
		private List<ResourceLocatorInfo> m_LocatorInfos;

		// Token: 0x0400002F RID: 47
		private AsyncOperationHandle<IList<AsyncOperationHandle>> m_DepOp;
	}
}
