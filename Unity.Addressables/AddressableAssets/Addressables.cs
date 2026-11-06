using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine.AddressableAssets.ResourceLocators;
using UnityEngine.Networking;
using UnityEngine.ResourceManagement;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceLocations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.ResourceManagement.Util;
using UnityEngine.SceneManagement;

namespace UnityEngine.AddressableAssets
{
	// Token: 0x02000009 RID: 9
	public static class Addressables
	{
		// Token: 0x1700000D RID: 13
		// (get) Token: 0x06000037 RID: 55 RVA: 0x00002EE4 File Offset: 0x000010E4
		private static AddressablesImpl m_Addressables
		{
			get
			{
				return Addressables.m_AddressablesInstance;
			}
		}

		// Token: 0x1700000E RID: 14
		// (get) Token: 0x06000038 RID: 56 RVA: 0x00002EEB File Offset: 0x000010EB
		public static string Version
		{
			get
			{
				return "";
			}
		}

		// Token: 0x1700000F RID: 15
		// (get) Token: 0x06000039 RID: 57 RVA: 0x00002EF2 File Offset: 0x000010F2
		public static ResourceManager ResourceManager
		{
			get
			{
				return Addressables.m_Addressables.ResourceManager;
			}
		}

		// Token: 0x17000010 RID: 16
		// (get) Token: 0x0600003A RID: 58 RVA: 0x00002EFE File Offset: 0x000010FE
		internal static AddressablesImpl Instance
		{
			get
			{
				return Addressables.m_Addressables;
			}
		}

		// Token: 0x17000011 RID: 17
		// (get) Token: 0x0600003B RID: 59 RVA: 0x00002F05 File Offset: 0x00001105
		public static IInstanceProvider InstanceProvider
		{
			get
			{
				return Addressables.m_Addressables.InstanceProvider;
			}
		}

		// Token: 0x0600003C RID: 60 RVA: 0x00002F11 File Offset: 0x00001111
		public static string ResolveInternalId(string id)
		{
			return Addressables.m_Addressables.ResolveInternalId(id);
		}

		// Token: 0x17000012 RID: 18
		// (get) Token: 0x0600003D RID: 61 RVA: 0x00002F1E File Offset: 0x0000111E
		// (set) Token: 0x0600003E RID: 62 RVA: 0x00002F2A File Offset: 0x0000112A
		public static Func<IResourceLocation, string> InternalIdTransformFunc
		{
			get
			{
				return Addressables.m_Addressables.InternalIdTransformFunc;
			}
			set
			{
				Addressables.m_Addressables.InternalIdTransformFunc = value;
			}
		}

		// Token: 0x17000013 RID: 19
		// (get) Token: 0x0600003F RID: 63 RVA: 0x00002F37 File Offset: 0x00001137
		// (set) Token: 0x06000040 RID: 64 RVA: 0x00002F43 File Offset: 0x00001143
		public static Action<UnityWebRequest> WebRequestOverride
		{
			get
			{
				return Addressables.m_Addressables.WebRequestOverride;
			}
			set
			{
				Addressables.m_Addressables.WebRequestOverride = value;
			}
		}

		// Token: 0x17000014 RID: 20
		// (get) Token: 0x06000041 RID: 65 RVA: 0x00002F50 File Offset: 0x00001150
		public static string StreamingAssetsSubFolder
		{
			get
			{
				return Addressables.m_Addressables.StreamingAssetsSubFolder;
			}
		}

		// Token: 0x17000015 RID: 21
		// (get) Token: 0x06000042 RID: 66 RVA: 0x00002F5C File Offset: 0x0000115C
		public static string BuildPath
		{
			get
			{
				return Addressables.m_Addressables.BuildPath;
			}
		}

		// Token: 0x17000016 RID: 22
		// (get) Token: 0x06000043 RID: 67 RVA: 0x00002F68 File Offset: 0x00001168
		public static string PlayerBuildDataPath
		{
			get
			{
				return Addressables.m_Addressables.PlayerBuildDataPath;
			}
		}

		// Token: 0x17000017 RID: 23
		// (get) Token: 0x06000044 RID: 68 RVA: 0x00002F74 File Offset: 0x00001174
		public static string RuntimePath
		{
			get
			{
				return Addressables.m_Addressables.RuntimePath;
			}
		}

		// Token: 0x17000018 RID: 24
		// (get) Token: 0x06000045 RID: 69 RVA: 0x00002F80 File Offset: 0x00001180
		public static IEnumerable<IResourceLocator> ResourceLocators
		{
			get
			{
				return Addressables.m_Addressables.ResourceLocators;
			}
		}

		// Token: 0x06000046 RID: 70 RVA: 0x00002F8C File Offset: 0x0000118C
		[Conditional("ADDRESSABLES_LOG_ALL")]
		internal static void InternalSafeSerializationLog(string msg, LogType logType = LogType.Log)
		{
			if (Addressables.m_AddressablesInstance == null)
			{
				return;
			}
			switch (logType)
			{
			case LogType.Error:
				Addressables.m_AddressablesInstance.LogError(msg);
				return;
			case LogType.Assert:
				break;
			case LogType.Warning:
				Addressables.m_AddressablesInstance.LogWarning(msg);
				return;
			case LogType.Log:
				Addressables.m_AddressablesInstance.Log(msg);
				break;
			default:
				return;
			}
		}

		// Token: 0x06000047 RID: 71 RVA: 0x00002FDC File Offset: 0x000011DC
		[Conditional("ADDRESSABLES_LOG_ALL")]
		internal static void InternalSafeSerializationLogFormat(string format, LogType logType = LogType.Log, params object[] args)
		{
			if (Addressables.m_AddressablesInstance == null)
			{
				return;
			}
			switch (logType)
			{
			case LogType.Error:
				Addressables.m_AddressablesInstance.LogErrorFormat(format, args);
				return;
			case LogType.Assert:
				break;
			case LogType.Warning:
				Addressables.m_AddressablesInstance.LogWarningFormat(format, args);
				return;
			case LogType.Log:
				Addressables.m_AddressablesInstance.LogFormat(format, args);
				break;
			default:
				return;
			}
		}

		// Token: 0x06000048 RID: 72 RVA: 0x0000302E File Offset: 0x0000122E
		[Conditional("ADDRESSABLES_LOG_ALL")]
		public static void Log(string msg)
		{
			Addressables.m_Addressables.Log(msg);
		}

		// Token: 0x06000049 RID: 73 RVA: 0x0000303B File Offset: 0x0000123B
		[Conditional("ADDRESSABLES_LOG_ALL")]
		public static void LogFormat(string format, params object[] args)
		{
			Addressables.m_Addressables.LogFormat(format, args);
		}

		// Token: 0x0600004A RID: 74 RVA: 0x00003049 File Offset: 0x00001249
		public static void LogWarning(string msg)
		{
			Addressables.m_Addressables.LogWarning(msg);
		}

		// Token: 0x0600004B RID: 75 RVA: 0x00003056 File Offset: 0x00001256
		public static void LogWarningFormat(string format, params object[] args)
		{
			Addressables.m_Addressables.LogWarningFormat(format, args);
		}

		// Token: 0x0600004C RID: 76 RVA: 0x00003064 File Offset: 0x00001264
		public static void LogError(string msg)
		{
			Addressables.m_Addressables.LogError(msg);
		}

		// Token: 0x0600004D RID: 77 RVA: 0x00003071 File Offset: 0x00001271
		public static void LogException(AsyncOperationHandle op, Exception ex)
		{
			Addressables.m_Addressables.LogException(op, ex);
		}

		// Token: 0x0600004E RID: 78 RVA: 0x0000307F File Offset: 0x0000127F
		public static void LogException(Exception ex)
		{
			Addressables.m_Addressables.LogException(ex);
		}

		// Token: 0x0600004F RID: 79 RVA: 0x0000308C File Offset: 0x0000128C
		public static void LogErrorFormat(string format, params object[] args)
		{
			Addressables.m_Addressables.LogErrorFormat(format, args);
		}

		// Token: 0x06000050 RID: 80 RVA: 0x0000309A File Offset: 0x0000129A
		[Obsolete]
		public static AsyncOperationHandle<IResourceLocator> Initialize()
		{
			return Addressables.InitializeAsync();
		}

		// Token: 0x06000051 RID: 81 RVA: 0x000030A1 File Offset: 0x000012A1
		public static AsyncOperationHandle<IResourceLocator> InitializeAsync()
		{
			return Addressables.m_Addressables.InitializeAsync();
		}

		// Token: 0x06000052 RID: 82 RVA: 0x000030AD File Offset: 0x000012AD
		public static AsyncOperationHandle<IResourceLocator> InitializeAsync(bool autoReleaseHandle)
		{
			return Addressables.m_Addressables.InitializeAsync(autoReleaseHandle);
		}

		// Token: 0x06000053 RID: 83 RVA: 0x000030BA File Offset: 0x000012BA
		[Obsolete]
		public static AsyncOperationHandle<IResourceLocator> LoadContentCatalog(string catalogPath, string providerSuffix = null)
		{
			return Addressables.LoadContentCatalogAsync(catalogPath, providerSuffix);
		}

		// Token: 0x06000054 RID: 84 RVA: 0x000030C3 File Offset: 0x000012C3
		public static AsyncOperationHandle<IResourceLocator> LoadContentCatalogAsync(string catalogPath, string providerSuffix = null)
		{
			return Addressables.m_Addressables.LoadContentCatalogAsync(catalogPath, false, providerSuffix);
		}

		// Token: 0x06000055 RID: 85 RVA: 0x000030D2 File Offset: 0x000012D2
		public static AsyncOperationHandle<IResourceLocator> LoadContentCatalogAsync(string catalogPath, bool autoReleaseHandle, string providerSuffix = null)
		{
			return Addressables.m_Addressables.LoadContentCatalogAsync(catalogPath, autoReleaseHandle, providerSuffix);
		}

		// Token: 0x17000019 RID: 25
		// (get) Token: 0x06000056 RID: 86 RVA: 0x000030E4 File Offset: 0x000012E4
		[Obsolete]
		public static AsyncOperationHandle<IResourceLocator> InitializationOperation
		{
			get
			{
				return default(AsyncOperationHandle<IResourceLocator>);
			}
		}

		// Token: 0x06000057 RID: 87 RVA: 0x000030FA File Offset: 0x000012FA
		[Obsolete]
		public static AsyncOperationHandle<TObject> LoadAsset<TObject>(IResourceLocation location)
		{
			return Addressables.LoadAssetAsync<TObject>(location);
		}

		// Token: 0x06000058 RID: 88 RVA: 0x00003102 File Offset: 0x00001302
		[Obsolete]
		public static AsyncOperationHandle<TObject> LoadAsset<TObject>(object key)
		{
			return Addressables.LoadAssetAsync<TObject>(key);
		}

		// Token: 0x06000059 RID: 89 RVA: 0x0000310A File Offset: 0x0000130A
		public static AsyncOperationHandle<TObject> LoadAssetAsync<TObject>(IResourceLocation location)
		{
			return Addressables.m_Addressables.LoadAssetAsync<TObject>(location);
		}

		// Token: 0x0600005A RID: 90 RVA: 0x00003117 File Offset: 0x00001317
		public static AsyncOperationHandle<TObject> LoadAssetAsync<TObject>(object key)
		{
			return Addressables.m_Addressables.LoadAssetAsync<TObject>(key);
		}

		// Token: 0x0600005B RID: 91 RVA: 0x00003124 File Offset: 0x00001324
		[Obsolete]
		public static AsyncOperationHandle<IList<IResourceLocation>> LoadResourceLocations(IList<object> keys, Addressables.MergeMode mode, Type type = null)
		{
			return Addressables.LoadResourceLocationsAsync(keys, mode, type);
		}

		// Token: 0x0600005C RID: 92 RVA: 0x0000312E File Offset: 0x0000132E
		[Obsolete]
		public static AsyncOperationHandle<IList<IResourceLocation>> LoadResourceLocationsAsync(IList<object> keys, Addressables.MergeMode mode, Type type = null)
		{
			return Addressables.m_Addressables.LoadResourceLocationsAsync(keys, mode, type);
		}

		// Token: 0x0600005D RID: 93 RVA: 0x0000313D File Offset: 0x0000133D
		public static AsyncOperationHandle<IList<IResourceLocation>> LoadResourceLocationsAsync(IEnumerable keys, Addressables.MergeMode mode, Type type = null)
		{
			return Addressables.m_Addressables.LoadResourceLocationsAsync(keys, mode, type);
		}

		// Token: 0x0600005E RID: 94 RVA: 0x0000314C File Offset: 0x0000134C
		[Obsolete]
		public static AsyncOperationHandle<IList<IResourceLocation>> LoadResourceLocations(object key, Type type = null)
		{
			return Addressables.LoadResourceLocationsAsync(key, type);
		}

		// Token: 0x0600005F RID: 95 RVA: 0x00003155 File Offset: 0x00001355
		public static AsyncOperationHandle<IList<IResourceLocation>> LoadResourceLocationsAsync(object key, Type type = null)
		{
			return Addressables.m_Addressables.LoadResourceLocationsAsync(key, type);
		}

		// Token: 0x06000060 RID: 96 RVA: 0x00003163 File Offset: 0x00001363
		[Obsolete]
		public static AsyncOperationHandle<IList<TObject>> LoadAssets<TObject>(IList<IResourceLocation> locations, Action<TObject> callback)
		{
			return Addressables.LoadAssetsAsync<TObject>(locations, callback);
		}

		// Token: 0x06000061 RID: 97 RVA: 0x0000316C File Offset: 0x0000136C
		public static AsyncOperationHandle<IList<TObject>> LoadAssetsAsync<TObject>(IList<IResourceLocation> locations, Action<TObject> callback)
		{
			return Addressables.m_Addressables.LoadAssetsAsync<TObject>(locations, callback, true);
		}

		// Token: 0x06000062 RID: 98 RVA: 0x0000317B File Offset: 0x0000137B
		public static AsyncOperationHandle<IList<TObject>> LoadAssetsAsync<TObject>(IList<IResourceLocation> locations, Action<TObject> callback, bool releaseDependenciesOnFailure)
		{
			return Addressables.m_Addressables.LoadAssetsAsync<TObject>(locations, callback, releaseDependenciesOnFailure);
		}

		// Token: 0x06000063 RID: 99 RVA: 0x0000318A File Offset: 0x0000138A
		[Obsolete]
		public static AsyncOperationHandle<IList<TObject>> LoadAssets<TObject>(IList<object> keys, Action<TObject> callback, Addressables.MergeMode mode)
		{
			return Addressables.LoadAssetsAsync<TObject>(keys, callback, mode);
		}

		// Token: 0x06000064 RID: 100 RVA: 0x00003194 File Offset: 0x00001394
		[Obsolete]
		public static AsyncOperationHandle<IList<TObject>> LoadAssetsAsync<TObject>(IList<object> keys, Action<TObject> callback, Addressables.MergeMode mode)
		{
			return Addressables.m_Addressables.LoadAssetsAsync<TObject>(keys, callback, mode, true);
		}

		// Token: 0x06000065 RID: 101 RVA: 0x000031A4 File Offset: 0x000013A4
		public static AsyncOperationHandle<IList<TObject>> LoadAssetsAsync<TObject>(IEnumerable keys, Action<TObject> callback, Addressables.MergeMode mode)
		{
			return Addressables.m_Addressables.LoadAssetsAsync<TObject>(keys, callback, mode, true);
		}

		// Token: 0x06000066 RID: 102 RVA: 0x000031B4 File Offset: 0x000013B4
		[Obsolete]
		public static AsyncOperationHandle<IList<TObject>> LoadAssetsAsync<TObject>(IList<object> keys, Action<TObject> callback, Addressables.MergeMode mode, bool releaseDependenciesOnFailure)
		{
			return Addressables.m_Addressables.LoadAssetsAsync<TObject>(keys, callback, mode, releaseDependenciesOnFailure);
		}

		// Token: 0x06000067 RID: 103 RVA: 0x000031C4 File Offset: 0x000013C4
		public static AsyncOperationHandle<IList<TObject>> LoadAssetsAsync<TObject>(IEnumerable keys, Action<TObject> callback, Addressables.MergeMode mode, bool releaseDependenciesOnFailure)
		{
			return Addressables.m_Addressables.LoadAssetsAsync<TObject>(keys, callback, mode, releaseDependenciesOnFailure);
		}

		// Token: 0x06000068 RID: 104 RVA: 0x000031D4 File Offset: 0x000013D4
		[Obsolete]
		public static AsyncOperationHandle<IList<TObject>> LoadAssets<TObject>(object key, Action<TObject> callback)
		{
			return Addressables.LoadAssetsAsync<TObject>(key, callback);
		}

		// Token: 0x06000069 RID: 105 RVA: 0x000031DD File Offset: 0x000013DD
		public static AsyncOperationHandle<IList<TObject>> LoadAssetsAsync<TObject>(object key, Action<TObject> callback)
		{
			return Addressables.m_Addressables.LoadAssetsAsync<TObject>(key, callback, true);
		}

		// Token: 0x0600006A RID: 106 RVA: 0x000031EC File Offset: 0x000013EC
		public static AsyncOperationHandle<IList<TObject>> LoadAssetsAsync<TObject>(object key, Action<TObject> callback, bool releaseDependenciesOnFailure)
		{
			return Addressables.m_Addressables.LoadAssetsAsync<TObject>(key, callback, releaseDependenciesOnFailure);
		}

		// Token: 0x0600006B RID: 107 RVA: 0x000031FB File Offset: 0x000013FB
		public static void Release<TObject>(TObject obj)
		{
			Addressables.m_Addressables.Release<TObject>(obj);
		}

		// Token: 0x0600006C RID: 108 RVA: 0x00003208 File Offset: 0x00001408
		public static void Release<TObject>(AsyncOperationHandle<TObject> handle)
		{
			handle.Release();
		}

		// Token: 0x0600006D RID: 109 RVA: 0x00003211 File Offset: 0x00001411
		public static void Release(AsyncOperationHandle handle)
		{
			handle.Release();
		}

		// Token: 0x0600006E RID: 110 RVA: 0x0000321A File Offset: 0x0000141A
		public static bool ReleaseInstance(GameObject instance)
		{
			return Addressables.m_Addressables.ReleaseInstance(instance);
		}

		// Token: 0x0600006F RID: 111 RVA: 0x00003227 File Offset: 0x00001427
		public static bool ReleaseInstance(AsyncOperationHandle handle)
		{
			handle.Release();
			return true;
		}

		// Token: 0x06000070 RID: 112 RVA: 0x00003231 File Offset: 0x00001431
		public static bool ReleaseInstance(AsyncOperationHandle<GameObject> handle)
		{
			handle.Release();
			return true;
		}

		// Token: 0x06000071 RID: 113 RVA: 0x0000323B File Offset: 0x0000143B
		[Obsolete]
		public static AsyncOperationHandle<long> GetDownloadSize(object key)
		{
			return Addressables.GetDownloadSizeAsync(key);
		}

		// Token: 0x06000072 RID: 114 RVA: 0x00003243 File Offset: 0x00001443
		public static AsyncOperationHandle<long> GetDownloadSizeAsync(object key)
		{
			return Addressables.m_Addressables.GetDownloadSizeAsync(key);
		}

		// Token: 0x06000073 RID: 115 RVA: 0x00003250 File Offset: 0x00001450
		public static AsyncOperationHandle<long> GetDownloadSizeAsync(string key)
		{
			return Addressables.m_Addressables.GetDownloadSizeAsync(key);
		}

		// Token: 0x06000074 RID: 116 RVA: 0x0000325D File Offset: 0x0000145D
		[Obsolete]
		public static AsyncOperationHandle<long> GetDownloadSizeAsync(IList<object> keys)
		{
			return Addressables.m_Addressables.GetDownloadSizeAsync(keys);
		}

		// Token: 0x06000075 RID: 117 RVA: 0x0000326A File Offset: 0x0000146A
		public static AsyncOperationHandle<long> GetDownloadSizeAsync(IEnumerable keys)
		{
			return Addressables.m_Addressables.GetDownloadSizeAsync(keys);
		}

		// Token: 0x06000076 RID: 118 RVA: 0x00003277 File Offset: 0x00001477
		[Obsolete]
		public static AsyncOperationHandle DownloadDependencies(object key)
		{
			return Addressables.DownloadDependenciesAsync(key, false);
		}

		// Token: 0x06000077 RID: 119 RVA: 0x00003280 File Offset: 0x00001480
		public static AsyncOperationHandle DownloadDependenciesAsync(object key, bool autoReleaseHandle = false)
		{
			return Addressables.m_Addressables.DownloadDependenciesAsync(key, autoReleaseHandle);
		}

		// Token: 0x06000078 RID: 120 RVA: 0x0000328E File Offset: 0x0000148E
		public static AsyncOperationHandle DownloadDependenciesAsync(IList<IResourceLocation> locations, bool autoReleaseHandle = false)
		{
			return Addressables.m_Addressables.DownloadDependenciesAsync(locations, autoReleaseHandle);
		}

		// Token: 0x06000079 RID: 121 RVA: 0x0000329C File Offset: 0x0000149C
		[Obsolete]
		public static AsyncOperationHandle DownloadDependenciesAsync(IList<object> keys, Addressables.MergeMode mode, bool autoReleaseHandle = false)
		{
			return Addressables.m_Addressables.DownloadDependenciesAsync(keys, mode, autoReleaseHandle);
		}

		// Token: 0x0600007A RID: 122 RVA: 0x000032AB File Offset: 0x000014AB
		public static AsyncOperationHandle DownloadDependenciesAsync(IEnumerable keys, Addressables.MergeMode mode, bool autoReleaseHandle = false)
		{
			return Addressables.m_Addressables.DownloadDependenciesAsync(keys, mode, autoReleaseHandle);
		}

		// Token: 0x0600007B RID: 123 RVA: 0x000032BA File Offset: 0x000014BA
		public static void ClearDependencyCacheAsync(object key)
		{
			Addressables.m_Addressables.ClearDependencyCacheAsync(key, true);
		}

		// Token: 0x0600007C RID: 124 RVA: 0x000032C9 File Offset: 0x000014C9
		public static void ClearDependencyCacheAsync(IList<IResourceLocation> locations)
		{
			Addressables.m_Addressables.ClearDependencyCacheAsync(locations, true);
		}

		// Token: 0x0600007D RID: 125 RVA: 0x000032D8 File Offset: 0x000014D8
		[Obsolete]
		public static void ClearDependencyCacheAsync(IList<object> keys)
		{
			Addressables.m_Addressables.ClearDependencyCacheAsync(keys, true);
		}

		// Token: 0x0600007E RID: 126 RVA: 0x000032E7 File Offset: 0x000014E7
		public static void ClearDependencyCacheAsync(IEnumerable keys)
		{
			Addressables.m_Addressables.ClearDependencyCacheAsync(keys, true);
		}

		// Token: 0x0600007F RID: 127 RVA: 0x000032F6 File Offset: 0x000014F6
		public static void ClearDependencyCacheAsync(string key)
		{
			Addressables.m_Addressables.ClearDependencyCacheAsync(key, true);
		}

		// Token: 0x06000080 RID: 128 RVA: 0x00003305 File Offset: 0x00001505
		public static AsyncOperationHandle<bool> ClearDependencyCacheAsync(object key, bool autoReleaseHandle)
		{
			return Addressables.m_Addressables.ClearDependencyCacheAsync(key, autoReleaseHandle);
		}

		// Token: 0x06000081 RID: 129 RVA: 0x00003313 File Offset: 0x00001513
		public static AsyncOperationHandle<bool> ClearDependencyCacheAsync(IList<IResourceLocation> locations, bool autoReleaseHandle)
		{
			return Addressables.m_Addressables.ClearDependencyCacheAsync(locations, autoReleaseHandle);
		}

		// Token: 0x06000082 RID: 130 RVA: 0x00003321 File Offset: 0x00001521
		[Obsolete]
		public static AsyncOperationHandle<bool> ClearDependencyCacheAsync(IList<object> keys, bool autoReleaseHandle)
		{
			return Addressables.m_Addressables.ClearDependencyCacheAsync(keys, autoReleaseHandle);
		}

		// Token: 0x06000083 RID: 131 RVA: 0x0000332F File Offset: 0x0000152F
		public static AsyncOperationHandle<bool> ClearDependencyCacheAsync(IEnumerable keys, bool autoReleaseHandle)
		{
			return Addressables.m_Addressables.ClearDependencyCacheAsync(keys, autoReleaseHandle);
		}

		// Token: 0x06000084 RID: 132 RVA: 0x0000333D File Offset: 0x0000153D
		public static AsyncOperationHandle<bool> ClearDependencyCacheAsync(string key, bool autoReleaseHandle)
		{
			return Addressables.m_Addressables.ClearDependencyCacheAsync(key, autoReleaseHandle);
		}

		// Token: 0x06000085 RID: 133 RVA: 0x0000334B File Offset: 0x0000154B
		public static ResourceLocatorInfo GetLocatorInfo(string locatorId)
		{
			return Addressables.m_Addressables.GetLocatorInfo(locatorId);
		}

		// Token: 0x06000086 RID: 134 RVA: 0x00003358 File Offset: 0x00001558
		public static ResourceLocatorInfo GetLocatorInfo(IResourceLocator locator)
		{
			return Addressables.m_Addressables.GetLocatorInfo(locator.LocatorId);
		}

		// Token: 0x06000087 RID: 135 RVA: 0x0000336A File Offset: 0x0000156A
		[Obsolete]
		public static AsyncOperationHandle<GameObject> Instantiate(IResourceLocation location, Transform parent = null, bool instantiateInWorldSpace = false, bool trackHandle = true)
		{
			return Addressables.InstantiateAsync(location, new InstantiationParameters(parent, instantiateInWorldSpace), trackHandle);
		}

		// Token: 0x06000088 RID: 136 RVA: 0x0000337A File Offset: 0x0000157A
		[Obsolete]
		public static AsyncOperationHandle<GameObject> Instantiate(IResourceLocation location, Vector3 position, Quaternion rotation, Transform parent = null, bool trackHandle = true)
		{
			return Addressables.InstantiateAsync(location, position, rotation, parent, trackHandle);
		}

		// Token: 0x06000089 RID: 137 RVA: 0x00003387 File Offset: 0x00001587
		[Obsolete]
		public static AsyncOperationHandle<GameObject> Instantiate(object key, Transform parent = null, bool instantiateInWorldSpace = false, bool trackHandle = true)
		{
			return Addressables.InstantiateAsync(key, parent, instantiateInWorldSpace, trackHandle);
		}

		// Token: 0x0600008A RID: 138 RVA: 0x00003392 File Offset: 0x00001592
		[Obsolete]
		public static AsyncOperationHandle<GameObject> Instantiate(object key, Vector3 position, Quaternion rotation, Transform parent = null, bool trackHandle = true)
		{
			return Addressables.InstantiateAsync(key, position, rotation, parent, trackHandle);
		}

		// Token: 0x0600008B RID: 139 RVA: 0x0000339F File Offset: 0x0000159F
		[Obsolete]
		public static AsyncOperationHandle<GameObject> Instantiate(object key, InstantiationParameters instantiateParameters, bool trackHandle = true)
		{
			return Addressables.InstantiateAsync(key, instantiateParameters, trackHandle);
		}

		// Token: 0x0600008C RID: 140 RVA: 0x000033A9 File Offset: 0x000015A9
		[Obsolete]
		public static AsyncOperationHandle<GameObject> Instantiate(IResourceLocation location, InstantiationParameters instantiateParameters, bool trackHandle = true)
		{
			return Addressables.InstantiateAsync(location, instantiateParameters, trackHandle);
		}

		// Token: 0x0600008D RID: 141 RVA: 0x000033B3 File Offset: 0x000015B3
		public static AsyncOperationHandle<GameObject> InstantiateAsync(IResourceLocation location, Transform parent = null, bool instantiateInWorldSpace = false, bool trackHandle = true)
		{
			return Addressables.m_Addressables.InstantiateAsync(location, new InstantiationParameters(parent, instantiateInWorldSpace), trackHandle);
		}

		// Token: 0x0600008E RID: 142 RVA: 0x000033C8 File Offset: 0x000015C8
		public static AsyncOperationHandle<GameObject> InstantiateAsync(IResourceLocation location, Vector3 position, Quaternion rotation, Transform parent = null, bool trackHandle = true)
		{
			return Addressables.m_Addressables.InstantiateAsync(location, position, rotation, parent, trackHandle);
		}

		// Token: 0x0600008F RID: 143 RVA: 0x000033DA File Offset: 0x000015DA
		public static AsyncOperationHandle<GameObject> InstantiateAsync(object key, Transform parent = null, bool instantiateInWorldSpace = false, bool trackHandle = true)
		{
			return Addressables.m_Addressables.InstantiateAsync(key, parent, instantiateInWorldSpace, trackHandle);
		}

		// Token: 0x06000090 RID: 144 RVA: 0x000033EA File Offset: 0x000015EA
		public static AsyncOperationHandle<GameObject> InstantiateAsync(object key, Vector3 position, Quaternion rotation, Transform parent = null, bool trackHandle = true)
		{
			return Addressables.m_Addressables.InstantiateAsync(key, position, rotation, parent, trackHandle);
		}

		// Token: 0x06000091 RID: 145 RVA: 0x000033FC File Offset: 0x000015FC
		public static AsyncOperationHandle<GameObject> InstantiateAsync(object key, InstantiationParameters instantiateParameters, bool trackHandle = true)
		{
			return Addressables.m_Addressables.InstantiateAsync(key, instantiateParameters, trackHandle);
		}

		// Token: 0x06000092 RID: 146 RVA: 0x0000340B File Offset: 0x0000160B
		public static AsyncOperationHandle<GameObject> InstantiateAsync(IResourceLocation location, InstantiationParameters instantiateParameters, bool trackHandle = true)
		{
			return Addressables.m_Addressables.InstantiateAsync(location, instantiateParameters, trackHandle);
		}

		// Token: 0x06000093 RID: 147 RVA: 0x0000341A File Offset: 0x0000161A
		[Obsolete]
		public static AsyncOperationHandle<SceneInstance> LoadScene(object key, LoadSceneMode loadMode = LoadSceneMode.Single, bool activateOnLoad = true, int priority = 100)
		{
			return Addressables.LoadSceneAsync(key, loadMode, activateOnLoad, priority);
		}

		// Token: 0x06000094 RID: 148 RVA: 0x00003425 File Offset: 0x00001625
		[Obsolete]
		public static AsyncOperationHandle<SceneInstance> LoadScene(IResourceLocation location, LoadSceneMode loadMode = LoadSceneMode.Single, bool activateOnLoad = true, int priority = 100)
		{
			return Addressables.LoadSceneAsync(location, loadMode, activateOnLoad, priority);
		}

		// Token: 0x06000095 RID: 149 RVA: 0x00003430 File Offset: 0x00001630
		public static AsyncOperationHandle<SceneInstance> LoadSceneAsync(object key, LoadSceneMode loadMode = LoadSceneMode.Single, bool activateOnLoad = true, int priority = 100)
		{
			return Addressables.m_Addressables.LoadSceneAsync(key, new LoadSceneParameters(loadMode), activateOnLoad, priority, true);
		}

		// Token: 0x06000096 RID: 150 RVA: 0x00003446 File Offset: 0x00001646
		public static AsyncOperationHandle<SceneInstance> LoadSceneAsync(object key, LoadSceneParameters loadSceneParameters, bool activateOnLoad = true, int priority = 100)
		{
			return Addressables.m_Addressables.LoadSceneAsync(key, loadSceneParameters, activateOnLoad, priority, true);
		}

		// Token: 0x06000097 RID: 151 RVA: 0x00003457 File Offset: 0x00001657
		public static AsyncOperationHandle<SceneInstance> LoadSceneAsync(IResourceLocation location, LoadSceneMode loadMode = LoadSceneMode.Single, bool activateOnLoad = true, int priority = 100)
		{
			return Addressables.m_Addressables.LoadSceneAsync(location, new LoadSceneParameters(loadMode), activateOnLoad, priority, true);
		}

		// Token: 0x06000098 RID: 152 RVA: 0x0000346D File Offset: 0x0000166D
		public static AsyncOperationHandle<SceneInstance> LoadSceneAsync(IResourceLocation location, LoadSceneParameters loadSceneParameters, bool activateOnLoad = true, int priority = 100)
		{
			return Addressables.m_Addressables.LoadSceneAsync(location, loadSceneParameters, activateOnLoad, priority, true);
		}

		// Token: 0x06000099 RID: 153 RVA: 0x0000347E File Offset: 0x0000167E
		[Obsolete]
		public static AsyncOperationHandle<SceneInstance> UnloadScene(SceneInstance scene, bool autoReleaseHandle = true)
		{
			return Addressables.UnloadSceneAsync(scene, autoReleaseHandle);
		}

		// Token: 0x0600009A RID: 154 RVA: 0x00003487 File Offset: 0x00001687
		[Obsolete]
		public static AsyncOperationHandle<SceneInstance> UnloadScene(AsyncOperationHandle handle, bool autoReleaseHandle = true)
		{
			return Addressables.UnloadSceneAsync(handle, autoReleaseHandle);
		}

		// Token: 0x0600009B RID: 155 RVA: 0x00003490 File Offset: 0x00001690
		[Obsolete]
		public static AsyncOperationHandle<SceneInstance> UnloadScene(AsyncOperationHandle<SceneInstance> handle, bool autoReleaseHandle = true)
		{
			return Addressables.UnloadSceneAsync(handle, autoReleaseHandle);
		}

		// Token: 0x0600009C RID: 156 RVA: 0x00003499 File Offset: 0x00001699
		[Obsolete]
		public static AsyncOperationHandle<SceneInstance> UnloadScene(AsyncOperationHandle<SceneInstance> handle, UnloadSceneOptions unloadOptions, bool autoReleaseHandle = true)
		{
			return Addressables.UnloadSceneAsync(handle, unloadOptions, autoReleaseHandle);
		}

		// Token: 0x0600009D RID: 157 RVA: 0x000034A8 File Offset: 0x000016A8
		public static AsyncOperationHandle<SceneInstance> UnloadSceneAsync(SceneInstance scene, UnloadSceneOptions unloadOptions, bool autoReleaseHandle = true)
		{
			return Addressables.m_Addressables.UnloadSceneAsync(scene, unloadOptions, autoReleaseHandle);
		}

		// Token: 0x0600009E RID: 158 RVA: 0x000034B7 File Offset: 0x000016B7
		public static AsyncOperationHandle<SceneInstance> UnloadSceneAsync(AsyncOperationHandle handle, UnloadSceneOptions unloadOptions, bool autoReleaseHandle = true)
		{
			return Addressables.m_Addressables.UnloadSceneAsync(handle, unloadOptions, autoReleaseHandle);
		}

		// Token: 0x0600009F RID: 159 RVA: 0x000034C6 File Offset: 0x000016C6
		public static AsyncOperationHandle<SceneInstance> UnloadSceneAsync(SceneInstance scene, bool autoReleaseHandle = true)
		{
			return Addressables.m_Addressables.UnloadSceneAsync(scene, UnloadSceneOptions.None, autoReleaseHandle);
		}

		// Token: 0x060000A0 RID: 160 RVA: 0x000034D5 File Offset: 0x000016D5
		public static AsyncOperationHandle<SceneInstance> UnloadSceneAsync(AsyncOperationHandle handle, bool autoReleaseHandle = true)
		{
			return Addressables.m_Addressables.UnloadSceneAsync(handle, UnloadSceneOptions.None, autoReleaseHandle);
		}

		// Token: 0x060000A1 RID: 161 RVA: 0x000034E4 File Offset: 0x000016E4
		public static AsyncOperationHandle<SceneInstance> UnloadSceneAsync(AsyncOperationHandle<SceneInstance> handle, bool autoReleaseHandle = true)
		{
			return Addressables.m_Addressables.UnloadSceneAsync(handle, UnloadSceneOptions.None, autoReleaseHandle);
		}

		// Token: 0x060000A2 RID: 162 RVA: 0x000034F3 File Offset: 0x000016F3
		public static AsyncOperationHandle<List<string>> CheckForCatalogUpdates(bool autoReleaseHandle = true)
		{
			return Addressables.m_Addressables.CheckForCatalogUpdates(autoReleaseHandle);
		}

		// Token: 0x060000A3 RID: 163 RVA: 0x00003500 File Offset: 0x00001700
		public static AsyncOperationHandle<List<IResourceLocator>> UpdateCatalogs(IEnumerable<string> catalogs = null, bool autoReleaseHandle = true)
		{
			return Addressables.m_Addressables.UpdateCatalogs(catalogs, autoReleaseHandle, false);
		}

		// Token: 0x060000A4 RID: 164 RVA: 0x0000350F File Offset: 0x0000170F
		public static AsyncOperationHandle<List<IResourceLocator>> UpdateCatalogs(bool autoCleanBundleCache, IEnumerable<string> catalogs = null, bool autoReleaseHandle = true)
		{
			return Addressables.m_Addressables.UpdateCatalogs(catalogs, autoReleaseHandle, autoCleanBundleCache);
		}

		// Token: 0x060000A5 RID: 165 RVA: 0x0000351E File Offset: 0x0000171E
		public static void AddResourceLocator(IResourceLocator locator, string localCatalogHash = null, IResourceLocation remoteCatalogLocation = null)
		{
			Addressables.m_Addressables.AddResourceLocator(locator, localCatalogHash, remoteCatalogLocation);
		}

		// Token: 0x060000A6 RID: 166 RVA: 0x0000352D File Offset: 0x0000172D
		public static void RemoveResourceLocator(IResourceLocator locator)
		{
			Addressables.m_Addressables.RemoveResourceLocator(locator);
		}

		// Token: 0x060000A7 RID: 167 RVA: 0x0000353A File Offset: 0x0000173A
		public static void ClearResourceLocators()
		{
			Addressables.m_Addressables.ClearResourceLocators();
		}

		// Token: 0x060000A8 RID: 168 RVA: 0x00003546 File Offset: 0x00001746
		public static AsyncOperationHandle<bool> CleanBundleCache(IEnumerable<string> catalogsIds = null)
		{
			return Addressables.m_Addressables.CleanBundleCache(catalogsIds, false);
		}

		// Token: 0x060000A9 RID: 169 RVA: 0x00003554 File Offset: 0x00001754
		public static ResourceLocationBase CreateCatalogLocationWithHashDependencies<T>(string remoteCatalogPath) where T : IResourceProvider
		{
			return Addressables.m_Addressables.CreateCatalogLocationWithHashDependencies<T>(remoteCatalogPath);
		}

		// Token: 0x060000AA RID: 170 RVA: 0x00003561 File Offset: 0x00001761
		public static ResourceLocationBase CreateCatalogLocationWithHashDependencies<T>(IResourceLocation remoteCatalogLocation) where T : IResourceProvider
		{
			return Addressables.m_Addressables.CreateCatalogLocationWithHashDependencies<T>(remoteCatalogLocation);
		}

		// Token: 0x060000AB RID: 171 RVA: 0x0000356E File Offset: 0x0000176E
		public static ResourceLocationBase CreateCatalogLocationWithHashDependencies<T>(string remoteCatalogPath, string remoteHashPath) where T : IResourceProvider
		{
			return Addressables.m_Addressables.CreateCatalogLocationWithHashDependencies<T>(remoteCatalogPath, remoteHashPath);
		}

		// Token: 0x04000010 RID: 16
		internal static bool reinitializeAddressables = true;

		// Token: 0x04000011 RID: 17
		internal static AddressablesImpl m_AddressablesInstance = new AddressablesImpl(new LRUCacheAllocationStrategy(1000, 1000, 100, 10));

		// Token: 0x04000012 RID: 18
		public const string kAddressablesRuntimeDataPath = "AddressablesRuntimeDataPath";

		// Token: 0x04000013 RID: 19
		private const string k_AddressablesLogConditional = "ADDRESSABLES_LOG_ALL";

		// Token: 0x04000014 RID: 20
		public const string kAddressablesRuntimeBuildLogPath = "AddressablesRuntimeBuildLog";

		// Token: 0x04000015 RID: 21
		public static string LibraryPath = "Library/com.unity.addressables/";

		// Token: 0x04000016 RID: 22
		public static string BuildReportPath = "Library/com.unity.addressables/BuildReports/";

		// Token: 0x0200002E RID: 46
		public enum MergeMode
		{
			// Token: 0x0400009D RID: 157
			None,
			// Token: 0x0400009E RID: 158
			UseFirst = 0,
			// Token: 0x0400009F RID: 159
			Union,
			// Token: 0x040000A0 RID: 160
			Intersection
		}
	}
}
