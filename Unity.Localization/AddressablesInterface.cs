using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.AddressableAssets;
using UnityEngine.AddressableAssets.ResourceLocators;
using UnityEngine.Localization.Operations;
using UnityEngine.ResourceManagement;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceLocations;

namespace UnityEngine.Localization
{
	// Token: 0x02000003 RID: 3
	internal class AddressablesInterface
	{
		// Token: 0x17000001 RID: 1
		// (get) Token: 0x06000003 RID: 3 RVA: 0x000020C6 File Offset: 0x000002C6
		// (set) Token: 0x06000004 RID: 4 RVA: 0x000020DE File Offset: 0x000002DE
		public static AddressablesInterface Instance
		{
			get
			{
				if (AddressablesInterface.s_Instance == null)
				{
					AddressablesInterface.s_Instance = new AddressablesInterface();
				}
				return AddressablesInterface.s_Instance;
			}
			set
			{
				AddressablesInterface.s_Instance = value;
			}
		}

		// Token: 0x17000002 RID: 2
		// (get) Token: 0x06000005 RID: 5 RVA: 0x000020E6 File Offset: 0x000002E6
		public static ResourceManager ResourceManager
		{
			get
			{
				return Addressables.ResourceManager;
			}
		}

		// Token: 0x06000006 RID: 6 RVA: 0x000020ED File Offset: 0x000002ED
		public static void Acquire(AsyncOperationHandle handle)
		{
			AddressablesInterface.Instance.AcquireInternal(handle);
		}

		// Token: 0x06000007 RID: 7 RVA: 0x000020FA File Offset: 0x000002FA
		public static void Release(AsyncOperationHandle handle)
		{
			AddressablesInterface.Instance.ReleaseInternal(handle);
		}

		// Token: 0x06000008 RID: 8 RVA: 0x00002107 File Offset: 0x00000307
		public static void SafeRelease(AsyncOperationHandle handle)
		{
			if (handle.IsValid())
			{
				AddressablesInterface.Instance.ReleaseInternal(handle);
			}
		}

		// Token: 0x06000009 RID: 9 RVA: 0x0000211D File Offset: 0x0000031D
		public static void ReleaseAndReset<TObject>(ref AsyncOperationHandle<TObject> handle)
		{
			if (handle.IsValid())
			{
				AddressablesInterface.Instance.ReleaseInternal(handle);
				handle = default(AsyncOperationHandle<TObject>);
			}
		}

		// Token: 0x0600000A RID: 10 RVA: 0x00002144 File Offset: 0x00000344
		public static AsyncOperationHandle<IList<AsyncOperationHandle>> CreateGroupOperation(List<AsyncOperationHandle> asyncOperations)
		{
			foreach (AsyncOperationHandle asyncOperationHandle in asyncOperations)
			{
				AddressablesInterface.Acquire(asyncOperationHandle);
			}
			LocalizationGroupOperation localizationGroupOperation = LocalizationGroupOperation.Pool.Get();
			localizationGroupOperation.Init(asyncOperations, true, false);
			return AddressablesInterface.ResourceManager.StartOperation<IList<AsyncOperationHandle>>(localizationGroupOperation, default(AsyncOperationHandle));
		}

		// Token: 0x0600000B RID: 11 RVA: 0x000021B8 File Offset: 0x000003B8
		public static AsyncOperationHandle<IList<IResourceLocation>> LoadResourceLocationsWithLabelsAsync(IEnumerable labels, Addressables.MergeMode mode, Type type = null)
		{
			return AddressablesInterface.Instance.LoadResourceLocationsWithLabelsAsyncInternal(labels, mode, type);
		}

		// Token: 0x0600000C RID: 12 RVA: 0x000021C7 File Offset: 0x000003C7
		public static AsyncOperationHandle<IList<IResourceLocation>> LoadTableLocationsAsync(string tableName, LocaleIdentifier id, Type type)
		{
			return AddressablesInterface.Instance.LoadTableLocationsAsyncInternal(tableName, id, type);
		}

		// Token: 0x0600000D RID: 13 RVA: 0x000021D6 File Offset: 0x000003D6
		public static AsyncOperationHandle<IList<TObject>> LoadAssetsFromLocations<TObject>(IList<IResourceLocation> locations, Action<TObject> callback)
		{
			return AddressablesInterface.Instance.LoadAssetsFromLocationsInternal<TObject>(locations, callback);
		}

		// Token: 0x0600000E RID: 14 RVA: 0x000021E4 File Offset: 0x000003E4
		public static AsyncOperationHandle<TObject> LoadAssetFromGUID<TObject>(string guid) where TObject : class
		{
			return AddressablesInterface.Instance.LoadAssetFromGUIDInternal<TObject>(guid);
		}

		// Token: 0x0600000F RID: 15 RVA: 0x000021F1 File Offset: 0x000003F1
		public static AsyncOperationHandle<TObject> LoadAssetFromName<TObject>(string name) where TObject : class
		{
			return AddressablesInterface.Instance.LoadAssetFromNameInternal<TObject>(name);
		}

		// Token: 0x06000010 RID: 16 RVA: 0x000021FE File Offset: 0x000003FE
		public static AsyncOperationHandle<TObject> LoadTableFromLocation<TObject>(IResourceLocation location) where TObject : class
		{
			return AddressablesInterface.Instance.LoadTableFromLocationInternal<TObject>(location);
		}

		// Token: 0x06000011 RID: 17 RVA: 0x0000220B File Offset: 0x0000040B
		public static AsyncOperationHandle<IList<TObject>> LoadAssetsWithLabel<TObject>(string label, Action<TObject> callback) where TObject : class
		{
			return AddressablesInterface.Instance.LoadAssetsWithLabelInternal<TObject>(label, callback);
		}

		// Token: 0x06000012 RID: 18 RVA: 0x00002219 File Offset: 0x00000419
		internal virtual void AcquireInternal(AsyncOperationHandle handle)
		{
			AddressablesInterface.ResourceManager.Acquire(handle);
		}

		// Token: 0x06000013 RID: 19 RVA: 0x00002226 File Offset: 0x00000426
		internal virtual void ReleaseInternal(AsyncOperationHandle handle)
		{
			Addressables.Release(handle);
		}

		// Token: 0x06000014 RID: 20 RVA: 0x0000222E File Offset: 0x0000042E
		internal virtual AsyncOperationHandle<IList<IResourceLocation>> LoadResourceLocationsWithLabelsAsyncInternal(IEnumerable labels, Addressables.MergeMode mode, Type type = null)
		{
			return Addressables.LoadResourceLocationsAsync(labels, mode, type);
		}

		// Token: 0x06000015 RID: 21 RVA: 0x00002238 File Offset: 0x00000438
		internal virtual AsyncOperationHandle<IList<IResourceLocation>> LoadTableLocationsAsyncInternal(string tableName, LocaleIdentifier id, Type type)
		{
			return Addressables.LoadResourceLocationsAsync(AddressHelper.GetTableAddress(tableName, id), type);
		}

		// Token: 0x06000016 RID: 22 RVA: 0x00002247 File Offset: 0x00000447
		internal virtual AsyncOperationHandle<IList<TObject>> LoadAssetsFromLocationsInternal<TObject>(IList<IResourceLocation> locations, Action<TObject> callback)
		{
			return Addressables.LoadAssetsAsync<TObject>(locations, callback);
		}

		// Token: 0x06000017 RID: 23 RVA: 0x00002250 File Offset: 0x00000450
		internal virtual AsyncOperationHandle<TObject> LoadAssetFromGUIDInternal<TObject>(string guid) where TObject : class
		{
			return Addressables.LoadAssetAsync<TObject>(guid);
		}

		// Token: 0x06000018 RID: 24 RVA: 0x00002258 File Offset: 0x00000458
		internal virtual AsyncOperationHandle<TObject> LoadAssetFromNameInternal<TObject>(string name) where TObject : class
		{
			return Addressables.LoadAssetAsync<TObject>(name);
		}

		// Token: 0x06000019 RID: 25 RVA: 0x00002260 File Offset: 0x00000460
		internal virtual AsyncOperationHandle<TObject> LoadTableFromLocationInternal<TObject>(IResourceLocation location) where TObject : class
		{
			return Addressables.LoadAssetAsync<TObject>(location);
		}

		// Token: 0x0600001A RID: 26 RVA: 0x00002268 File Offset: 0x00000468
		internal virtual AsyncOperationHandle<IList<TObject>> LoadAssetsWithLabelInternal<TObject>(string label, Action<TObject> callback) where TObject : class
		{
			return Addressables.LoadAssetsAsync<TObject>(label, callback);
		}

		// Token: 0x0600001B RID: 27 RVA: 0x00002271 File Offset: 0x00000471
		internal virtual AsyncOperationHandle<IResourceLocator> InitializeAddressablesAsync()
		{
			return Addressables.InitializeAsync();
		}

		// Token: 0x04000001 RID: 1
		private static AddressablesInterface s_Instance;
	}
}
