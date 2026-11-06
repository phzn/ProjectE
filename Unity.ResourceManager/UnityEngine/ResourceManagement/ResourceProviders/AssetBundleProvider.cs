using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using UnityEngine.ResourceManagement.ResourceLocations;
using UnityEngine.ResourceManagement.Util;

namespace UnityEngine.ResourceManagement.ResourceProviders
{
	// Token: 0x02000031 RID: 49
	[DisplayName("AssetBundle Provider")]
	public class AssetBundleProvider : ResourceProviderBase
	{
		// Token: 0x06000157 RID: 343 RVA: 0x00006596 File Offset: 0x00004796
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
		private static void Init()
		{
			AssetBundleProvider.m_UnloadingBundles = new Dictionary<string, AssetBundleUnloadOperation>();
		}

		// Token: 0x1700003C RID: 60
		// (get) Token: 0x06000158 RID: 344 RVA: 0x000065A2 File Offset: 0x000047A2
		// (set) Token: 0x06000159 RID: 345 RVA: 0x000065A9 File Offset: 0x000047A9
		protected internal static Dictionary<string, AssetBundleUnloadOperation> UnloadingBundles
		{
			get
			{
				return AssetBundleProvider.m_UnloadingBundles;
			}
			internal set
			{
				AssetBundleProvider.m_UnloadingBundles = value;
			}
		}

		// Token: 0x1700003D RID: 61
		// (get) Token: 0x0600015A RID: 346 RVA: 0x000065B1 File Offset: 0x000047B1
		internal static int UnloadingAssetBundleCount
		{
			get
			{
				return AssetBundleProvider.m_UnloadingBundles.Count;
			}
		}

		// Token: 0x1700003E RID: 62
		// (get) Token: 0x0600015B RID: 347 RVA: 0x000065BD File Offset: 0x000047BD
		internal static int AssetBundleCount
		{
			get
			{
				return AssetBundle.GetAllLoadedAssetBundles().Count<AssetBundle>() - AssetBundleProvider.UnloadingAssetBundleCount;
			}
		}

		// Token: 0x0600015C RID: 348 RVA: 0x000065D0 File Offset: 0x000047D0
		internal static void WaitForAllUnloadingBundlesToComplete()
		{
			if (AssetBundleProvider.UnloadingAssetBundleCount > 0)
			{
				AssetBundleUnloadOperation[] array = AssetBundleProvider.m_UnloadingBundles.Values.ToArray<AssetBundleUnloadOperation>();
				for (int i = 0; i < array.Length; i++)
				{
					array[i].WaitForCompletion();
				}
			}
		}

		// Token: 0x0600015D RID: 349 RVA: 0x0000660C File Offset: 0x0000480C
		public override void Provide(ProvideHandle providerInterface)
		{
			AssetBundleUnloadOperation assetBundleUnloadOperation;
			if (AssetBundleProvider.m_UnloadingBundles.TryGetValue(providerInterface.Location.InternalId, out assetBundleUnloadOperation) && assetBundleUnloadOperation.isDone)
			{
				assetBundleUnloadOperation = null;
			}
			new AssetBundleResource().Start(providerInterface, assetBundleUnloadOperation);
		}

		// Token: 0x0600015E RID: 350 RVA: 0x00006649 File Offset: 0x00004849
		public override Type GetDefaultType(IResourceLocation location)
		{
			return typeof(IAssetBundleResource);
		}

		// Token: 0x0600015F RID: 351 RVA: 0x00006658 File Offset: 0x00004858
		public override void Release(IResourceLocation location, object asset)
		{
			if (location == null)
			{
				throw new ArgumentNullException("location");
			}
			if (asset == null)
			{
				if (!(location is DownloadOnlyLocation))
				{
					Debug.LogWarningFormat("Releasing null asset bundle from location {0}.  This is an indication that the bundle failed to load.", new object[] { location });
				}
				return;
			}
			AssetBundleResource assetBundleResource = asset as AssetBundleResource;
			if (assetBundleResource != null)
			{
				AssetBundleUnloadOperation assetBundleUnloadOperation;
				if (assetBundleResource.Unload(out assetBundleUnloadOperation))
				{
					AssetBundleProvider.m_UnloadingBundles.Add(location.InternalId, assetBundleUnloadOperation);
					assetBundleUnloadOperation.completed += delegate(AsyncOperation op)
					{
						AssetBundleProvider.m_UnloadingBundles.Remove(location.InternalId);
					};
				}
				return;
			}
		}

		// Token: 0x06000160 RID: 352 RVA: 0x000066EC File Offset: 0x000048EC
		internal virtual IOperationCacheKey CreateCacheKeyForLocation(ResourceManager rm, IResourceLocation location, Type desiredType)
		{
			return new IdCacheKey(location.GetType(), rm.TransformInternalId(location));
		}

		// Token: 0x0400008F RID: 143
		internal static Dictionary<string, AssetBundleUnloadOperation> m_UnloadingBundles = new Dictionary<string, AssetBundleUnloadOperation>();
	}
}
