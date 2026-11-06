using System;
using System.Collections.Generic;
using Unity.Profiling;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceLocations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.ResourceManagement.Util;

namespace UnityEngine.ResourceManagement.Profiling
{
	// Token: 0x02000052 RID: 82
	internal static class ProfilerRuntime
	{
		// Token: 0x060001EF RID: 495 RVA: 0x000073F0 File Offset: 0x000055F0
		public static void Initialise()
		{
			ProfilerRuntime.CatalogLoadCounter.Value = 0;
			ProfilerRuntime.AssetBundleLoadCounter.Value = 0;
			ProfilerRuntime.AssetLoadCounter.Value = 0;
			ProfilerRuntime.SceneLoadCounter.Value = 0;
			ProfilerRuntime.m_CatalogData.Data.Clear();
			ProfilerRuntime.m_BundleData.Data.Clear();
			ProfilerRuntime.m_AssetData.Data.Clear();
			ProfilerRuntime.m_SceneData.Data.Clear();
			ProfilerRuntime.m_profilerEmitter.InitialiseCallbacks(new Action<float>(ProfilerRuntime.InstanceOnOnLateUpdateDelegate));
		}

		// Token: 0x060001F0 RID: 496 RVA: 0x0000747B File Offset: 0x0000567B
		private static void InstanceOnOnLateUpdateDelegate(float deltaTime)
		{
			ProfilerRuntime.PushToProfilerStream();
		}

		// Token: 0x060001F1 RID: 497 RVA: 0x00007484 File Offset: 0x00005684
		public static void AddCatalog(Hash128 buildHash)
		{
			if (!buildHash.isValid)
			{
				return;
			}
			ProfilerRuntime.m_CatalogData.Add(buildHash, new CatalogFrameData
			{
				BuildResultHash = buildHash
			});
			int value = ProfilerRuntime.CatalogLoadCounter.Value;
			ProfilerRuntime.CatalogLoadCounter.Value = value + 1;
		}

		// Token: 0x060001F2 RID: 498 RVA: 0x000074CC File Offset: 0x000056CC
		public static void AddBundleOperation(ProvideHandle handle, AssetBundleRequestOptions requestOptions, ContentStatus status, BundleSource source)
		{
			IAsyncOperation asyncOperation = handle.InternalOp as IAsyncOperation;
			if (asyncOperation == null)
			{
				throw new NullReferenceException("Could not get Bundle operation for handle loaded for Key " + handle.Location.PrimaryKey);
			}
			string bundleName = requestOptions.BundleName;
			BundleOptions bundleOptions = BundleOptions.None;
			bool flag = requestOptions.Crc > 0U;
			if (flag && source == BundleSource.Cache)
			{
				flag = requestOptions.UseCrcForCachedBundle;
			}
			if (flag)
			{
				bundleOptions |= BundleOptions.CheckSumEnabled;
			}
			if (!string.IsNullOrEmpty(requestOptions.Hash))
			{
				bundleOptions |= BundleOptions.CachingEnabled;
			}
			BundleFrameData bundleFrameData = new BundleFrameData
			{
				ReferenceCount = asyncOperation.ReferenceCount,
				BundleCode = bundleName.GetHashCode(),
				Status = status,
				LoadingOptions = bundleOptions,
				Source = source
			};
			ProfilerRuntime.m_BundleData.Add(asyncOperation, bundleFrameData);
			if (!ProfilerRuntime.m_BundleNameToOperation.ContainsKey(bundleName))
			{
				ProfilerRuntime.AssetBundleLoadCounter.Value = ProfilerRuntime.AssetBundleLoadCounter.Value + 1;
			}
			ProfilerRuntime.m_BundleNameToOperation[bundleName] = asyncOperation;
		}

		// Token: 0x060001F3 RID: 499 RVA: 0x000075B8 File Offset: 0x000057B8
		public static void BundleReleased(string bundleName)
		{
			IAsyncOperation asyncOperation;
			if (string.IsNullOrEmpty(bundleName) || !ProfilerRuntime.m_BundleNameToOperation.TryGetValue(bundleName, out asyncOperation))
			{
				return;
			}
			ProfilerRuntime.m_BundleData.Remove(asyncOperation);
			ProfilerRuntime.m_BundleNameToOperation.Remove(bundleName);
			ProfilerRuntime.AssetBundleLoadCounter.Value = ProfilerRuntime.AssetBundleLoadCounter.Value - 1;
			List<IAsyncOperation> list;
			if (ProfilerRuntime.m_BundleNameToAssetOperations.TryGetValue(bundleName, out list))
			{
				ProfilerRuntime.m_BundleNameToAssetOperations.Remove(bundleName);
				foreach (IAsyncOperation asyncOperation2 in list)
				{
					ProfilerRuntime.AssetLoadCounter.Value = ProfilerRuntime.AssetLoadCounter.Value - 1;
					ProfilerRuntime.m_AssetData.Remove(asyncOperation2);
				}
			}
		}

		// Token: 0x060001F4 RID: 500 RVA: 0x0000767C File Offset: 0x0000587C
		public static void AddAssetOperation(ProvideHandle handle, ContentStatus status)
		{
			if (!handle.IsValid)
			{
				throw new ArgumentException("Attempting to add a Asset handle to profiler that is not valid");
			}
			IAsyncOperation asyncOperation = handle.InternalOp as IAsyncOperation;
			if (asyncOperation == null)
			{
				throw new NullReferenceException("Could not get operation for InternalOp of handle loaded with primary key: " + handle.Location.PrimaryKey);
			}
			string containingBundleNameForLocation = ProfilerRuntime.GetContainingBundleNameForLocation(handle.Location);
			string text;
			if (handle.Location.InternalId.EndsWith("]"))
			{
				int num = handle.Location.InternalId.IndexOf('[');
				text = handle.Location.InternalId.Remove(num);
			}
			else
			{
				text = handle.Location.InternalId;
			}
			AssetFrameData assetFrameData = default(AssetFrameData);
			assetFrameData.AssetCode = text.GetHashCode();
			assetFrameData.ReferenceCount = asyncOperation.ReferenceCount;
			assetFrameData.BundleCode = containingBundleNameForLocation.GetHashCode();
			assetFrameData.Status = status;
			List<IAsyncOperation> list;
			if (ProfilerRuntime.m_BundleNameToAssetOperations.TryGetValue(containingBundleNameForLocation, out list))
			{
				if (!list.Contains(asyncOperation))
				{
					list.Add(asyncOperation);
				}
			}
			else
			{
				ProfilerRuntime.m_BundleNameToAssetOperations.Add(containingBundleNameForLocation, new List<IAsyncOperation> { asyncOperation });
			}
			if (ProfilerRuntime.m_AssetData.Add(asyncOperation, assetFrameData))
			{
				ProfilerRuntime.AssetLoadCounter.Value = ProfilerRuntime.AssetLoadCounter.Value + 1;
			}
		}

		// Token: 0x060001F5 RID: 501 RVA: 0x000077B8 File Offset: 0x000059B8
		private static string GetContainingBundleNameForLocation(IResourceLocation location)
		{
			if (location == null || location.Dependencies == null || location.Dependencies.Count == 0)
			{
				return "";
			}
			AssetBundleRequestOptions assetBundleRequestOptions = location.Dependencies[0].Data as AssetBundleRequestOptions;
			if (assetBundleRequestOptions == null)
			{
				Debug.LogError("Dependency bundle location does not have AssetBundleRequestOptions");
				return "";
			}
			return assetBundleRequestOptions.BundleName;
		}

		// Token: 0x060001F6 RID: 502 RVA: 0x00007814 File Offset: 0x00005A14
		public static void AddSceneOperation(AsyncOperationHandle<SceneInstance> handle, IResourceLocation location, ContentStatus status)
		{
			IAsyncOperation internalOp = handle.InternalOp;
			string containingBundleNameForLocation = ProfilerRuntime.GetContainingBundleNameForLocation(location);
			AssetFrameData assetFrameData = default(AssetFrameData);
			assetFrameData.AssetCode = location.InternalId.GetHashCode();
			assetFrameData.ReferenceCount = internalOp.ReferenceCount;
			assetFrameData.BundleCode = containingBundleNameForLocation.GetHashCode();
			assetFrameData.Status = status;
			if (ProfilerRuntime.m_SceneData.Add(internalOp, assetFrameData))
			{
				ProfilerRuntime.SceneLoadCounter.Value = ProfilerRuntime.SceneLoadCounter.Value + 1;
			}
		}

		// Token: 0x060001F7 RID: 503 RVA: 0x0000788C File Offset: 0x00005A8C
		public static void SceneReleased(AsyncOperationHandle<SceneInstance> handle)
		{
			ChainOperationTypelessDepedency<SceneInstance> chainOperationTypelessDepedency = handle.InternalOp as ChainOperationTypelessDepedency<SceneInstance>;
			if (chainOperationTypelessDepedency != null)
			{
				if (ProfilerRuntime.m_SceneData.Remove(chainOperationTypelessDepedency.WrappedOp.InternalOp))
				{
					ProfilerRuntime.SceneLoadCounter.Value = ProfilerRuntime.SceneLoadCounter.Value - 1;
					return;
				}
				Debug.LogWarning("Failed to remove scene from Addressables profiler for " + chainOperationTypelessDepedency.WrappedOp.DebugName);
				return;
			}
			else
			{
				if (ProfilerRuntime.m_SceneData.Remove(handle.InternalOp))
				{
					ProfilerRuntime.SceneLoadCounter.Value = ProfilerRuntime.SceneLoadCounter.Value - 1;
					return;
				}
				Debug.LogWarning("Failed to remove scene from Addressables profiler for " + handle.DebugName);
				return;
			}
		}

		// Token: 0x060001F8 RID: 504 RVA: 0x00007930 File Offset: 0x00005B30
		internal static int GetSceneLoadCounterValue()
		{
			return ProfilerRuntime.SceneLoadCounter.Value;
		}

		// Token: 0x060001F9 RID: 505 RVA: 0x0000793C File Offset: 0x00005B3C
		internal static void PushToProfilerStream()
		{
			if (!ProfilerRuntime.m_profilerEmitter.IsEnabled)
			{
				return;
			}
			ProfilerRuntime.RefreshChangedReferenceCounts();
			ProfilerRuntime.m_profilerEmitter.EmitFrameMetaData(ProfilerRuntime.kResourceManagerProfilerGuid, 0, ProfilerRuntime.m_CatalogData.Values);
			ProfilerRuntime.m_profilerEmitter.EmitFrameMetaData(ProfilerRuntime.kResourceManagerProfilerGuid, 1, ProfilerRuntime.m_BundleData.Values);
			ProfilerRuntime.m_profilerEmitter.EmitFrameMetaData(ProfilerRuntime.kResourceManagerProfilerGuid, 2, ProfilerRuntime.m_AssetData.Values);
			ProfilerRuntime.m_profilerEmitter.EmitFrameMetaData(ProfilerRuntime.kResourceManagerProfilerGuid, 3, ProfilerRuntime.m_SceneData.Values);
			ProfilerRuntime.m_CatalogData.Data.Clear();
		}

		// Token: 0x060001FA RID: 506 RVA: 0x000079D4 File Offset: 0x00005BD4
		private static void RefreshChangedReferenceCounts()
		{
			ProfilerRuntime.m_DataChange.Clear();
			foreach (KeyValuePair<IAsyncOperation, BundleFrameData> keyValuePair in ProfilerRuntime.m_BundleData.Data)
			{
				ValueTuple<int, float> valueTuple;
				if (ProfilerRuntime.ShouldUpdateFrameDataWithOperationData(keyValuePair.Key, keyValuePair.Value.ReferenceCount, keyValuePair.Value.PercentComplete, out valueTuple))
				{
					ProfilerRuntime.m_DataChange.Add(keyValuePair.Key, valueTuple);
				}
			}
			foreach (KeyValuePair<IAsyncOperation, ValueTuple<int, float>> keyValuePair2 in ProfilerRuntime.m_DataChange)
			{
				BundleFrameData bundleFrameData = ProfilerRuntime.m_BundleData[keyValuePair2.Key];
				bundleFrameData.ReferenceCount = keyValuePair2.Value.Item1;
				bundleFrameData.PercentComplete = keyValuePair2.Value.Item2;
				ProfilerRuntime.m_BundleData[keyValuePair2.Key] = bundleFrameData;
			}
			ProfilerRuntime.m_DataChange.Clear();
			foreach (KeyValuePair<IAsyncOperation, AssetFrameData> keyValuePair3 in ProfilerRuntime.m_AssetData.Data)
			{
				ValueTuple<int, float> valueTuple2;
				if (ProfilerRuntime.ShouldUpdateFrameDataWithOperationData(keyValuePair3.Key, keyValuePair3.Value.ReferenceCount, keyValuePair3.Value.PercentComplete, out valueTuple2))
				{
					ProfilerRuntime.m_DataChange.Add(keyValuePair3.Key, valueTuple2);
				}
			}
			foreach (KeyValuePair<IAsyncOperation, ValueTuple<int, float>> keyValuePair4 in ProfilerRuntime.m_DataChange)
			{
				AssetFrameData assetFrameData = ProfilerRuntime.m_AssetData[keyValuePair4.Key];
				assetFrameData.ReferenceCount = keyValuePair4.Value.Item1;
				assetFrameData.PercentComplete = keyValuePair4.Value.Item2;
				ProfilerRuntime.m_AssetData[keyValuePair4.Key] = assetFrameData;
			}
			ProfilerRuntime.m_DataChange.Clear();
			foreach (KeyValuePair<IAsyncOperation, AssetFrameData> keyValuePair5 in ProfilerRuntime.m_SceneData.Data)
			{
				ValueTuple<int, float> valueTuple3;
				if (ProfilerRuntime.ShouldUpdateFrameDataWithOperationData(keyValuePair5.Key, keyValuePair5.Value.ReferenceCount, keyValuePair5.Value.PercentComplete, out valueTuple3))
				{
					ProfilerRuntime.m_DataChange.Add(keyValuePair5.Key, valueTuple3);
				}
			}
			foreach (KeyValuePair<IAsyncOperation, ValueTuple<int, float>> keyValuePair6 in ProfilerRuntime.m_DataChange)
			{
				AssetFrameData assetFrameData2 = ProfilerRuntime.m_SceneData[keyValuePair6.Key];
				assetFrameData2.ReferenceCount = keyValuePair6.Value.Item1;
				assetFrameData2.PercentComplete = keyValuePair6.Value.Item2;
				ProfilerRuntime.m_SceneData[keyValuePair6.Key] = assetFrameData2;
			}
		}

		// Token: 0x060001FB RID: 507 RVA: 0x00007D0C File Offset: 0x00005F0C
		private static bool ShouldUpdateFrameDataWithOperationData(IAsyncOperation activeOperation, int frameReferenceCount, float framePercentComplete, out ValueTuple<int, float> newDataOut)
		{
			int num = activeOperation.ReferenceCount;
			switch (activeOperation.Status)
			{
			case AsyncOperationStatus.None:
				if (activeOperation.IsDone || !activeOperation.IsRunning)
				{
					num = 0;
				}
				break;
			case AsyncOperationStatus.Failed:
				num = 0;
				break;
			}
			float percentComplete = activeOperation.PercentComplete;
			newDataOut = new ValueTuple<int, float>(num, percentComplete);
			return num != frameReferenceCount || !Mathf.Approximately(percentComplete, framePercentComplete);
		}

		// Token: 0x040000C9 RID: 201
		internal static IProfilerEmitter m_profilerEmitter = new EngineEmitter();

		// Token: 0x040000CA RID: 202
		public static readonly Guid kResourceManagerProfilerGuid = new Guid("4f8a8c93-7634-4ef7-bbbc-6c9928567fa4");

		// Token: 0x040000CB RID: 203
		public const int kCatalogTag = 0;

		// Token: 0x040000CC RID: 204
		public const int kBundleDataTag = 1;

		// Token: 0x040000CD RID: 205
		public const int kAssetDataTag = 2;

		// Token: 0x040000CE RID: 206
		public const int kSceneDataTag = 3;

		// Token: 0x040000CF RID: 207
		private static ProfilerCounterValue<int> CatalogLoadCounter = new ProfilerCounterValue<int>(ProfilerCategory.Loading, "Catalogs", ProfilerMarkerDataUnit.Count);

		// Token: 0x040000D0 RID: 208
		private static ProfilerCounterValue<int> AssetBundleLoadCounter = new ProfilerCounterValue<int>(ProfilerCategory.Loading, "Asset Bundles", ProfilerMarkerDataUnit.Count);

		// Token: 0x040000D1 RID: 209
		private static ProfilerCounterValue<int> AssetLoadCounter = new ProfilerCounterValue<int>(ProfilerCategory.Loading, "Assets", ProfilerMarkerDataUnit.Count);

		// Token: 0x040000D2 RID: 210
		private static ProfilerCounterValue<int> SceneLoadCounter = new ProfilerCounterValue<int>(ProfilerCategory.Loading, "Scenes", ProfilerMarkerDataUnit.Count);

		// Token: 0x040000D3 RID: 211
		private static ProfilerFrameData<Hash128, CatalogFrameData> m_CatalogData = new ProfilerFrameData<Hash128, CatalogFrameData>(4);

		// Token: 0x040000D4 RID: 212
		private static ProfilerFrameData<IAsyncOperation, BundleFrameData> m_BundleData = new ProfilerFrameData<IAsyncOperation, BundleFrameData>(64);

		// Token: 0x040000D5 RID: 213
		private static ProfilerFrameData<IAsyncOperation, AssetFrameData> m_AssetData = new ProfilerFrameData<IAsyncOperation, AssetFrameData>(512);

		// Token: 0x040000D6 RID: 214
		private static ProfilerFrameData<IAsyncOperation, AssetFrameData> m_SceneData = new ProfilerFrameData<IAsyncOperation, AssetFrameData>(16);

		// Token: 0x040000D7 RID: 215
		private static Dictionary<string, IAsyncOperation> m_BundleNameToOperation = new Dictionary<string, IAsyncOperation>(64);

		// Token: 0x040000D8 RID: 216
		private static Dictionary<string, List<IAsyncOperation>> m_BundleNameToAssetOperations = new Dictionary<string, List<IAsyncOperation>>(512);

		// Token: 0x040000D9 RID: 217
		private static Dictionary<IAsyncOperation, ValueTuple<int, float>> m_DataChange = new Dictionary<IAsyncOperation, ValueTuple<int, float>>(64);
	}
}
