using System;
using System.Collections.Generic;
using UnityEngine.Localization.Metadata;
using UnityEngine.Localization.Operations;
using UnityEngine.Pool;
using UnityEngine.ResourceManagement;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace UnityEngine.Localization.Tables
{
	// Token: 0x02000028 RID: 40
	public class AssetTable : DetailedLocalizationTable<AssetTableEntry>, IPreloadRequired
	{
		// Token: 0x1700003A RID: 58
		// (get) Token: 0x06000123 RID: 291 RVA: 0x00004D78 File Offset: 0x00002F78
		private ResourceManager ResourceManager
		{
			get
			{
				return AddressablesInterface.ResourceManager;
			}
		}

		// Token: 0x1700003B RID: 59
		// (get) Token: 0x06000124 RID: 292 RVA: 0x00004D7F File Offset: 0x00002F7F
		public virtual AsyncOperationHandle PreloadOperation
		{
			get
			{
				if (!this.m_PreloadOperationHandle.IsValid())
				{
					this.m_PreloadOperationHandle = this.PreloadAssets();
				}
				return this.m_PreloadOperationHandle;
			}
		}

		// Token: 0x06000125 RID: 293 RVA: 0x00004DA0 File Offset: 0x00002FA0
		private AsyncOperationHandle PreloadAssets()
		{
			PreloadAssetTableMetadata preloadAssetTableMetadata = base.GetMetadata<PreloadAssetTableMetadata>() ?? base.SharedData.Metadata.GetMetadata<PreloadAssetTableMetadata>();
			if (preloadAssetTableMetadata == null || preloadAssetTableMetadata.Behaviour > PreloadAssetTableMetadata.PreloadBehaviour.NoPreload)
			{
				List<AsyncOperationHandle> list = CollectionPool<List<AsyncOperationHandle>, AsyncOperationHandle>.Get();
				foreach (AssetTableEntry assetTableEntry in base.Values)
				{
					if (!assetTableEntry.IsEmpty && !assetTableEntry.PreloadAsyncOperation.IsValid())
					{
						assetTableEntry.PreloadAsyncOperation = AddressablesInterface.LoadAssetFromGUID<Object[]>(assetTableEntry.Guid);
						list.Add(assetTableEntry.PreloadAsyncOperation);
					}
				}
				if (list.Count > 0)
				{
					return AddressablesInterface.CreateGroupOperation(list);
				}
				CollectionPool<List<AsyncOperationHandle>, AsyncOperationHandle>.Release(list);
			}
			return this.ResourceManager.CreateCompletedOperation<AssetTable>(this, null);
		}

		// Token: 0x06000126 RID: 294 RVA: 0x00004E84 File Offset: 0x00003084
		public AsyncOperationHandle<TObject> GetAssetAsync<TObject>(TableEntryReference entryReference) where TObject : Object
		{
			AssetTableEntry entryFromReference = base.GetEntryFromReference(entryReference);
			if (entryFromReference == null)
			{
				string text = entryReference.ResolveKeyName(base.SharedData);
				return this.ResourceManager.CreateCompletedOperation<TObject>(default(TObject), "Could not find asset with key \"" + text + "\"");
			}
			return this.GetAssetAsync<TObject>(entryFromReference);
		}

		// Token: 0x06000127 RID: 295 RVA: 0x00004ED8 File Offset: 0x000030D8
		internal AsyncOperationHandle<TObject> GetAssetAsync<TObject>(AssetTableEntry entry) where TObject : Object
		{
			if (entry.AsyncOperation.IsValid())
			{
				try
				{
					return entry.AsyncOperation.Convert<TObject>();
				}
				catch (InvalidCastException)
				{
					AddressablesInterface.Release(entry.AsyncOperation);
					entry.AsyncOperation = default(AsyncOperationHandle);
				}
			}
			if (entry.IsEmpty)
			{
				AsyncOperationHandle<TObject> asyncOperationHandle = this.ResourceManager.CreateCompletedOperation<TObject>(default(TObject), null);
				entry.AsyncOperation = asyncOperationHandle;
				return asyncOperationHandle;
			}
			LoadSubAssetOperation<TObject> loadSubAssetOperation = LoadSubAssetOperation<TObject>.Pool.Get();
			loadSubAssetOperation.Init(entry.PreloadAsyncOperation, entry.Address, entry.IsSubAsset, entry.SubAssetName);
			AsyncOperationHandle<TObject> asyncOperationHandle2 = this.ResourceManager.StartOperation<TObject>(loadSubAssetOperation, entry.PreloadAsyncOperation);
			entry.AsyncOperation = asyncOperationHandle2;
			return asyncOperationHandle2;
		}

		// Token: 0x06000128 RID: 296 RVA: 0x00004FB8 File Offset: 0x000031B8
		public void ReleaseAssets()
		{
			if (this.m_PreloadOperationHandle.IsValid())
			{
				AddressablesInterface.Release(this.m_PreloadOperationHandle);
				this.m_PreloadOperationHandle = default(AsyncOperationHandle);
			}
			foreach (AssetTableEntry assetTableEntry in base.Values)
			{
				this.ReleaseAsset(assetTableEntry);
			}
		}

		// Token: 0x06000129 RID: 297 RVA: 0x0000502C File Offset: 0x0000322C
		public void ReleaseAsset(AssetTableEntry entry)
		{
			if (entry == null)
			{
				return;
			}
			if (entry.PreloadAsyncOperation.IsValid())
			{
				AddressablesInterface.Release(entry.PreloadAsyncOperation);
				entry.PreloadAsyncOperation = default(AsyncOperationHandle<Object[]>);
			}
			if (entry.AsyncOperation.IsValid())
			{
				AddressablesInterface.Release(entry.AsyncOperation);
				entry.AsyncOperation = default(AsyncOperationHandle);
			}
		}

		// Token: 0x0600012A RID: 298 RVA: 0x00005096 File Offset: 0x00003296
		public void ReleaseAsset(TableEntryReference entry)
		{
			this.ReleaseAsset(base.GetEntryFromReference(entry));
		}

		// Token: 0x0600012B RID: 299 RVA: 0x000050A5 File Offset: 0x000032A5
		public override AssetTableEntry CreateTableEntry()
		{
			return new AssetTableEntry
			{
				Table = this,
				Data = new TableEntryData()
			};
		}

		// Token: 0x04000046 RID: 70
		private AsyncOperationHandle m_PreloadOperationHandle;
	}
}
