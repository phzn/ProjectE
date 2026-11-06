using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using UnityEngine.AddressableAssets.Initialization;
using UnityEngine.AddressableAssets.ResourceLocators;
using UnityEngine.AddressableAssets.ResourceProviders;
using UnityEngine.Networking;
using UnityEngine.ResourceManagement;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceLocations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.ResourceManagement.Util;
using UnityEngine.SceneManagement;

namespace UnityEngine.AddressableAssets
{
	// Token: 0x0200000A RID: 10
	internal class AddressablesImpl : IEqualityComparer<IResourceLocation>
	{
		// Token: 0x1700001A RID: 26
		// (get) Token: 0x060000AD RID: 173 RVA: 0x000035B5 File Offset: 0x000017B5
		// (set) Token: 0x060000AE RID: 174 RVA: 0x000035C0 File Offset: 0x000017C0
		public IInstanceProvider InstanceProvider
		{
			get
			{
				return this.m_InstanceProvider;
			}
			set
			{
				this.m_InstanceProvider = value;
				IUpdateReceiver updateReceiver = this.m_InstanceProvider as IUpdateReceiver;
				if (updateReceiver != null)
				{
					this.m_ResourceManager.AddUpdateReceiver(updateReceiver);
				}
			}
		}

		// Token: 0x1700001B RID: 27
		// (get) Token: 0x060000AF RID: 175 RVA: 0x000035EF File Offset: 0x000017EF
		public ResourceManager ResourceManager
		{
			get
			{
				return this.m_ResourceManager;
			}
		}

		// Token: 0x1700001C RID: 28
		// (get) Token: 0x060000B0 RID: 176 RVA: 0x000035F7 File Offset: 0x000017F7
		// (set) Token: 0x060000B1 RID: 177 RVA: 0x000035FF File Offset: 0x000017FF
		public int CatalogRequestsTimeout
		{
			get
			{
				return this.m_CatalogRequestsTimeout;
			}
			set
			{
				this.m_CatalogRequestsTimeout = value;
			}
		}

		// Token: 0x1700001D RID: 29
		// (get) Token: 0x060000B2 RID: 178 RVA: 0x00003608 File Offset: 0x00001808
		internal int SceneOperationCount
		{
			get
			{
				return this.m_SceneInstances.Count;
			}
		}

		// Token: 0x1700001E RID: 30
		// (get) Token: 0x060000B3 RID: 179 RVA: 0x00003615 File Offset: 0x00001815
		internal int TrackedHandleCount
		{
			get
			{
				return this.m_resultToHandle.Count;
			}
		}

		// Token: 0x060000B4 RID: 180 RVA: 0x00003624 File Offset: 0x00001824
		public AddressablesImpl(IAllocationStrategy alloc)
		{
			this.m_ResourceManager = new ResourceManager(alloc);
			SceneManager.sceneUnloaded += this.OnSceneUnloaded;
		}

		// Token: 0x060000B5 RID: 181 RVA: 0x00003675 File Offset: 0x00001875
		internal void ReleaseSceneManagerOperation()
		{
			SceneManager.sceneUnloaded -= this.OnSceneUnloaded;
		}

		// Token: 0x1700001F RID: 31
		// (get) Token: 0x060000B6 RID: 182 RVA: 0x00003688 File Offset: 0x00001888
		// (set) Token: 0x060000B7 RID: 183 RVA: 0x00003695 File Offset: 0x00001895
		public Func<IResourceLocation, string> InternalIdTransformFunc
		{
			get
			{
				return this.ResourceManager.InternalIdTransformFunc;
			}
			set
			{
				this.ResourceManager.InternalIdTransformFunc = value;
			}
		}

		// Token: 0x17000020 RID: 32
		// (get) Token: 0x060000B8 RID: 184 RVA: 0x000036A3 File Offset: 0x000018A3
		// (set) Token: 0x060000B9 RID: 185 RVA: 0x000036B0 File Offset: 0x000018B0
		public Action<UnityWebRequest> WebRequestOverride
		{
			get
			{
				return this.ResourceManager.WebRequestOverride;
			}
			set
			{
				this.ResourceManager.WebRequestOverride = value;
			}
		}

		// Token: 0x17000021 RID: 33
		// (get) Token: 0x060000BA RID: 186 RVA: 0x000036C0 File Offset: 0x000018C0
		public AsyncOperationHandle ChainOperation
		{
			get
			{
				if (!this.hasStartedInitialization)
				{
					return this.InitializeAsync();
				}
				if (this.m_InitializationOperation.IsValid() && !this.m_InitializationOperation.IsDone)
				{
					return this.m_InitializationOperation;
				}
				if (this.m_ActiveUpdateOperation.IsValid() && !this.m_ActiveUpdateOperation.IsDone)
				{
					return this.m_ActiveUpdateOperation;
				}
				Debug.LogWarning("ChainOperation property should not be accessed unless ShouldChainRequest is true.");
				return default(AsyncOperationHandle);
			}
		}

		// Token: 0x17000022 RID: 34
		// (get) Token: 0x060000BB RID: 187 RVA: 0x00003740 File Offset: 0x00001940
		internal bool ShouldChainRequest
		{
			get
			{
				return !this.hasStartedInitialization || (this.m_InitializationOperation.IsValid() && !this.m_InitializationOperation.IsDone) || (this.m_ActiveUpdateOperation.IsValid() && !this.m_ActiveUpdateOperation.IsDone);
			}
		}

		// Token: 0x060000BC RID: 188 RVA: 0x00003790 File Offset: 0x00001990
		internal void OnSceneUnloaded(Scene scene)
		{
			foreach (AsyncOperationHandle asyncOperationHandle in this.m_SceneInstances)
			{
				if (!asyncOperationHandle.IsValid())
				{
					this.m_SceneInstances.Remove(asyncOperationHandle);
					break;
				}
				AsyncOperationHandle<SceneInstance> asyncOperationHandle2 = asyncOperationHandle.Convert<SceneInstance>();
				if (asyncOperationHandle2.Result.Scene == scene)
				{
					this.m_SceneInstances.Remove(asyncOperationHandle);
					this.m_resultToHandle.Remove(asyncOperationHandle.Result);
					this.SceneProvider.ReleaseScene(this.m_ResourceManager, asyncOperationHandle2).ReleaseHandleOnCompletion();
					break;
				}
			}
			this.m_ResourceManager.CleanupSceneInstances(scene);
		}

		// Token: 0x17000023 RID: 35
		// (get) Token: 0x060000BD RID: 189 RVA: 0x00003860 File Offset: 0x00001A60
		public string StreamingAssetsSubFolder
		{
			get
			{
				return "aa";
			}
		}

		// Token: 0x17000024 RID: 36
		// (get) Token: 0x060000BE RID: 190 RVA: 0x00003867 File Offset: 0x00001A67
		public string BuildPath
		{
			get
			{
				return Addressables.LibraryPath + this.StreamingAssetsSubFolder + "/" + PlatformMappingService.GetPlatformPathSubFolder();
			}
		}

		// Token: 0x17000025 RID: 37
		// (get) Token: 0x060000BF RID: 191 RVA: 0x00003883 File Offset: 0x00001A83
		public string PlayerBuildDataPath
		{
			get
			{
				return Application.streamingAssetsPath + "/" + this.StreamingAssetsSubFolder;
			}
		}

		// Token: 0x17000026 RID: 38
		// (get) Token: 0x060000C0 RID: 192 RVA: 0x0000389A File Offset: 0x00001A9A
		public string RuntimePath
		{
			get
			{
				return this.PlayerBuildDataPath;
			}
		}

		// Token: 0x060000C1 RID: 193 RVA: 0x000038A2 File Offset: 0x00001AA2
		public void Log(string msg)
		{
			Debug.Log(msg);
		}

		// Token: 0x060000C2 RID: 194 RVA: 0x000038AA File Offset: 0x00001AAA
		public void LogFormat(string format, params object[] args)
		{
			Debug.LogFormat(format, args);
		}

		// Token: 0x060000C3 RID: 195 RVA: 0x000038B3 File Offset: 0x00001AB3
		public void LogWarning(string msg)
		{
			Debug.LogWarning(msg);
		}

		// Token: 0x060000C4 RID: 196 RVA: 0x000038BB File Offset: 0x00001ABB
		public void LogWarningFormat(string format, params object[] args)
		{
			Debug.LogWarningFormat(format, args);
		}

		// Token: 0x060000C5 RID: 197 RVA: 0x000038C4 File Offset: 0x00001AC4
		public void LogError(string msg)
		{
			Debug.LogError(msg);
		}

		// Token: 0x060000C6 RID: 198 RVA: 0x000038CC File Offset: 0x00001ACC
		public void LogException(AsyncOperationHandle op, Exception ex)
		{
			if (op.Status == AsyncOperationStatus.Failed)
			{
				Debug.LogError(ex.ToString());
			}
		}

		// Token: 0x060000C7 RID: 199 RVA: 0x000038E3 File Offset: 0x00001AE3
		public void LogException(Exception ex)
		{
		}

		// Token: 0x060000C8 RID: 200 RVA: 0x000038E5 File Offset: 0x00001AE5
		public void LogErrorFormat(string format, params object[] args)
		{
			Debug.LogErrorFormat(format, args);
		}

		// Token: 0x060000C9 RID: 201 RVA: 0x000038F0 File Offset: 0x00001AF0
		public string ResolveInternalId(string id)
		{
			string text = AddressablesRuntimeProperties.EvaluateString(id);
			if (text.Length >= 260 && text.StartsWith(Application.dataPath, StringComparison.Ordinal))
			{
				text = text.Substring(Application.dataPath.Length + 1);
			}
			return text;
		}

		// Token: 0x17000027 RID: 39
		// (get) Token: 0x060000CA RID: 202 RVA: 0x00003933 File Offset: 0x00001B33
		public IEnumerable<IResourceLocator> ResourceLocators
		{
			get
			{
				return this.m_ResourceLocators.Select<ResourceLocatorInfo, IResourceLocator>((ResourceLocatorInfo l) => l.Locator);
			}
		}

		// Token: 0x060000CB RID: 203 RVA: 0x0000395F File Offset: 0x00001B5F
		public void AddResourceLocator(IResourceLocator loc, string localCatalogHash = null, IResourceLocation remoteCatalogLocation = null)
		{
			this.m_ResourceLocators.Add(new ResourceLocatorInfo(loc, localCatalogHash, remoteCatalogLocation));
		}

		// Token: 0x060000CC RID: 204 RVA: 0x00003974 File Offset: 0x00001B74
		public void RemoveResourceLocator(IResourceLocator loc)
		{
			this.m_ResourceLocators.RemoveAll((ResourceLocatorInfo l) => l.Locator == loc);
		}

		// Token: 0x060000CD RID: 205 RVA: 0x000039A6 File Offset: 0x00001BA6
		public void ClearResourceLocators()
		{
			this.m_ResourceLocators.Clear();
		}

		// Token: 0x060000CE RID: 206 RVA: 0x000039B4 File Offset: 0x00001BB4
		internal bool GetResourceLocations(object key, Type type, out IList<IResourceLocation> locations)
		{
			if (type == null && key is AssetReference)
			{
				type = (key as AssetReference).SubObjectType;
			}
			key = this.EvaluateKey(key);
			locations = null;
			HashSet<IResourceLocation> hashSet = null;
			using (List<ResourceLocatorInfo>.Enumerator enumerator = this.m_ResourceLocators.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					IList<IResourceLocation> list;
					if (enumerator.Current.Locator.Locate(key, type, out list))
					{
						if (locations == null)
						{
							locations = list;
						}
						else
						{
							if (hashSet == null)
							{
								hashSet = new HashSet<IResourceLocation>();
								foreach (IResourceLocation resourceLocation in locations)
								{
									hashSet.Add(resourceLocation);
								}
							}
							hashSet.UnionWith(list);
						}
					}
				}
			}
			if (hashSet == null)
			{
				return locations != null;
			}
			locations = new List<IResourceLocation>(hashSet);
			return true;
		}

		// Token: 0x060000CF RID: 207 RVA: 0x00003AA4 File Offset: 0x00001CA4
		internal bool GetResourceLocations(IEnumerable keys, Type type, Addressables.MergeMode merge, out IList<IResourceLocation> locations)
		{
			locations = null;
			HashSet<IResourceLocation> hashSet = null;
			foreach (object obj in keys)
			{
				IList<IResourceLocation> list;
				if (this.GetResourceLocations(obj, type, out list))
				{
					if (locations == null)
					{
						locations = list;
						if (merge == Addressables.MergeMode.None)
						{
							return true;
						}
					}
					else
					{
						if (hashSet == null)
						{
							hashSet = new HashSet<IResourceLocation>(locations, this);
						}
						if (merge == Addressables.MergeMode.Intersection)
						{
							hashSet.IntersectWith(list);
						}
						else if (merge == Addressables.MergeMode.Union)
						{
							hashSet.UnionWith(list);
						}
					}
				}
				else if (merge == Addressables.MergeMode.Intersection)
				{
					locations = null;
					return false;
				}
			}
			if (hashSet == null)
			{
				return locations != null;
			}
			if (hashSet.Count == 0)
			{
				locations = null;
				return false;
			}
			locations = new List<IResourceLocation>(hashSet);
			return true;
		}

		// Token: 0x060000D0 RID: 208 RVA: 0x00003B6C File Offset: 0x00001D6C
		public AsyncOperationHandle<IResourceLocator> InitializeAsync(string runtimeDataPath, string providerSuffix = null, bool autoReleaseHandle = true)
		{
			if (this.hasStartedInitialization)
			{
				if (this.m_InitializationOperation.IsValid())
				{
					return this.m_InitializationOperation;
				}
				AsyncOperationHandle<IResourceLocator> asyncOperationHandle = this.ResourceManager.CreateCompletedOperation<IResourceLocator>(this.m_ResourceLocators[0].Locator, null);
				if (autoReleaseHandle)
				{
					asyncOperationHandle.ReleaseHandleOnCompletion();
				}
				return asyncOperationHandle;
			}
			else
			{
				if (ResourceManager.ExceptionHandler == null)
				{
					ResourceManager.ExceptionHandler = new Action<AsyncOperationHandle, Exception>(this.LogException);
				}
				this.hasStartedInitialization = true;
				if (this.m_InitializationOperation.IsValid())
				{
					return this.m_InitializationOperation;
				}
				GC.KeepAlive(Application.streamingAssetsPath);
				GC.KeepAlive(Application.persistentDataPath);
				if (string.IsNullOrEmpty(runtimeDataPath))
				{
					return this.ResourceManager.CreateCompletedOperation<IResourceLocator>(null, string.Format("Invalid Key: {0}", runtimeDataPath));
				}
				this.m_OnHandleCompleteAction = new Action<AsyncOperationHandle>(this.OnHandleCompleted);
				this.m_OnSceneHandleCompleteAction = new Action<AsyncOperationHandle>(this.OnSceneHandleCompleted);
				this.m_OnHandleDestroyedAction = new Action<AsyncOperationHandle>(this.OnHandleDestroyed);
				if (!this.m_InitializationOperation.IsValid())
				{
					this.m_InitializationOperation = InitializationOperation.CreateInitializationOperation(this, runtimeDataPath, providerSuffix);
				}
				if (autoReleaseHandle)
				{
					this.m_InitializationOperation.ReleaseHandleOnCompletion();
				}
				return this.m_InitializationOperation;
			}
		}

		// Token: 0x060000D1 RID: 209 RVA: 0x00003C8C File Offset: 0x00001E8C
		public AsyncOperationHandle<IResourceLocator> InitializeAsync()
		{
			string text = this.RuntimePath + "/settings.json";
			return this.InitializeAsync(this.ResolveInternalId(text), null, true);
		}

		// Token: 0x060000D2 RID: 210 RVA: 0x00003CBC File Offset: 0x00001EBC
		public AsyncOperationHandle<IResourceLocator> InitializeAsync(bool autoReleaseHandle)
		{
			string text = this.RuntimePath + "/settings.json";
			return this.InitializeAsync(this.ResolveInternalId(text), null, autoReleaseHandle);
		}

		// Token: 0x060000D3 RID: 211 RVA: 0x00003CE9 File Offset: 0x00001EE9
		public ResourceLocationBase CreateCatalogLocationWithHashDependencies<T>(IResourceLocation catalogLocation) where T : IResourceProvider
		{
			return this.CreateCatalogLocationWithHashDependencies<T>(catalogLocation.InternalId);
		}

		// Token: 0x060000D4 RID: 212 RVA: 0x00003CF8 File Offset: 0x00001EF8
		public ResourceLocationBase CreateCatalogLocationWithHashDependencies<T>(string catalogLocation) where T : IResourceProvider
		{
			string text = catalogLocation.Replace(".json", ".hash");
			return this.CreateCatalogLocationWithHashDependencies<T>(catalogLocation, text);
		}

		// Token: 0x060000D5 RID: 213 RVA: 0x00003D20 File Offset: 0x00001F20
		public ResourceLocationBase CreateCatalogLocationWithHashDependencies<T>(string catalogPath, string hashFilePath) where T : IResourceProvider
		{
			ResourceLocationBase resourceLocationBase = new ResourceLocationBase(catalogPath, catalogPath, typeof(T).FullName, typeof(IResourceLocator), Array.Empty<IResourceLocation>())
			{
				Data = new ProviderLoadRequestOptions
				{
					IgnoreFailures = false,
					WebRequestTimeout = this.CatalogRequestsTimeout
				}
			};
			if (!string.IsNullOrEmpty(hashFilePath))
			{
				ProviderLoadRequestOptions providerLoadRequestOptions = new ProviderLoadRequestOptions
				{
					IgnoreFailures = true,
					WebRequestTimeout = this.CatalogRequestsTimeout
				};
				string text = hashFilePath;
				if (ResourceManagerConfig.IsPathRemote(hashFilePath))
				{
					text = ResourceManagerConfig.StripQueryParameters(hashFilePath);
				}
				string text2 = this.ResolveInternalId("{UnityEngine.Application.persistentDataPath}/com.unity.addressables/" + text.GetHashCode().ToString() + ".hash");
				ResourceLocationBase resourceLocationBase2 = new ResourceLocationBase(hashFilePath, hashFilePath, typeof(TextDataProvider).FullName, typeof(string), Array.Empty<IResourceLocation>())
				{
					Data = providerLoadRequestOptions.Copy()
				};
				resourceLocationBase.Dependencies.Add(resourceLocationBase2);
				ResourceLocationBase resourceLocationBase3 = new ResourceLocationBase(text2, text2, typeof(TextDataProvider).FullName, typeof(string), Array.Empty<IResourceLocation>())
				{
					Data = providerLoadRequestOptions.Copy()
				};
				resourceLocationBase.Dependencies.Add(resourceLocationBase3);
			}
			return resourceLocationBase;
		}

		// Token: 0x060000D6 RID: 214 RVA: 0x00003E48 File Offset: 0x00002048
		[Conditional("UNITY_EDITOR")]
		private void QueueEditorUpdateIfNeeded()
		{
		}

		// Token: 0x060000D7 RID: 215 RVA: 0x00003E4C File Offset: 0x0000204C
		public AsyncOperationHandle<IResourceLocator> LoadContentCatalogAsync(string catalogPath, bool autoReleaseHandle = true, string providerSuffix = null)
		{
			ResourceLocationBase resourceLocationBase = this.CreateCatalogLocationWithHashDependencies<ContentCatalogProvider>(catalogPath);
			if (this.ShouldChainRequest)
			{
				return this.ResourceManager.CreateChainOperation<IResourceLocator>(this.ChainOperation, (AsyncOperationHandle op) => this.LoadContentCatalogAsync(catalogPath, autoReleaseHandle, providerSuffix));
			}
			AsyncOperationHandle<IResourceLocator> asyncOperationHandle = InitializationOperation.LoadContentCatalog(this, resourceLocationBase, providerSuffix, null);
			if (autoReleaseHandle)
			{
				asyncOperationHandle.ReleaseHandleOnCompletion();
			}
			return asyncOperationHandle;
		}

		// Token: 0x060000D8 RID: 216 RVA: 0x00003ECD File Offset: 0x000020CD
		private AsyncOperationHandle<SceneInstance> TrackHandle(AsyncOperationHandle<SceneInstance> handle)
		{
			handle.Completed += delegate(AsyncOperationHandle<SceneInstance> sceneHandle)
			{
				this.m_OnSceneHandleCompleteAction(sceneHandle);
			};
			return handle;
		}

		// Token: 0x060000D9 RID: 217 RVA: 0x00003EE3 File Offset: 0x000020E3
		private AsyncOperationHandle<TObject> TrackHandle<TObject>(AsyncOperationHandle<TObject> handle)
		{
			handle.CompletedTypeless += this.m_OnHandleCompleteAction;
			return handle;
		}

		// Token: 0x060000DA RID: 218 RVA: 0x00003EF3 File Offset: 0x000020F3
		private AsyncOperationHandle TrackHandle(AsyncOperationHandle handle)
		{
			handle.Completed += this.m_OnHandleCompleteAction;
			return handle;
		}

		// Token: 0x060000DB RID: 219 RVA: 0x00003F03 File Offset: 0x00002103
		internal void ClearTrackHandles()
		{
			this.m_resultToHandle.Clear();
		}

		// Token: 0x060000DC RID: 220 RVA: 0x00003F10 File Offset: 0x00002110
		public AsyncOperationHandle<TObject> LoadAssetAsync<TObject>(IResourceLocation location)
		{
			if (this.ShouldChainRequest)
			{
				return this.TrackHandle<TObject>(this.LoadAssetWithChain<TObject>(this.ChainOperation, location));
			}
			return this.TrackHandle<TObject>(this.ResourceManager.ProvideResource<TObject>(location));
		}

		// Token: 0x060000DD RID: 221 RVA: 0x00003F40 File Offset: 0x00002140
		private AsyncOperationHandle<TObject> LoadAssetWithChain<TObject>(AsyncOperationHandle dep, IResourceLocation loc)
		{
			return this.ResourceManager.CreateChainOperation<TObject>(dep, (AsyncOperationHandle op) => this.LoadAssetAsync<TObject>(loc));
		}

		// Token: 0x060000DE RID: 222 RVA: 0x00003F7C File Offset: 0x0000217C
		private AsyncOperationHandle<TObject> LoadAssetWithChain<TObject>(AsyncOperationHandle dep, object key)
		{
			return this.ResourceManager.CreateChainOperation<TObject>(dep, (AsyncOperationHandle op) => this.LoadAssetAsync<TObject>(key));
		}

		// Token: 0x060000DF RID: 223 RVA: 0x00003FB8 File Offset: 0x000021B8
		public AsyncOperationHandle<TObject> LoadAssetAsync<TObject>(object key)
		{
			if (this.ShouldChainRequest)
			{
				return this.TrackHandle<TObject>(this.LoadAssetWithChain<TObject>(this.ChainOperation, key));
			}
			key = this.EvaluateKey(key);
			Type type = typeof(TObject);
			if (type.IsArray)
			{
				type = type.GetElementType();
			}
			else if (type.IsGenericType && typeof(IList<>) == type.GetGenericTypeDefinition())
			{
				type = type.GetGenericArguments()[0];
			}
			using (List<ResourceLocatorInfo>.Enumerator enumerator = this.m_ResourceLocators.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					IList<IResourceLocation> list;
					if (enumerator.Current.Locator.Locate(key, type, out list))
					{
						foreach (IResourceLocation resourceLocation in list)
						{
							if (this.ResourceManager.GetResourceProvider(typeof(TObject), resourceLocation) != null)
							{
								return this.TrackHandle<TObject>(this.ResourceManager.ProvideResource<TObject>(resourceLocation));
							}
						}
					}
				}
			}
			return this.ResourceManager.CreateCompletedOperationWithException<TObject>(default(TObject), new InvalidKeyException(key, type, this));
		}

		// Token: 0x060000E0 RID: 224 RVA: 0x00004100 File Offset: 0x00002300
		public AsyncOperationHandle<IList<IResourceLocation>> LoadResourceLocationsWithChain(AsyncOperationHandle dep, IEnumerable keys, Addressables.MergeMode mode, Type type)
		{
			return this.ResourceManager.CreateChainOperation<IList<IResourceLocation>>(dep, (AsyncOperationHandle op) => this.LoadResourceLocationsAsync(keys, mode, type));
		}

		// Token: 0x060000E1 RID: 225 RVA: 0x00004148 File Offset: 0x00002348
		public AsyncOperationHandle<IList<IResourceLocation>> LoadResourceLocationsAsync(IEnumerable keys, Addressables.MergeMode mode, Type type = null)
		{
			if (this.ShouldChainRequest)
			{
				return this.TrackHandle<IList<IResourceLocation>>(this.LoadResourceLocationsWithChain(this.ChainOperation, keys, mode, type));
			}
			AddressablesImpl.LoadResourceLocationKeysOp loadResourceLocationKeysOp = new AddressablesImpl.LoadResourceLocationKeysOp();
			loadResourceLocationKeysOp.Init(this, type, keys, mode);
			return this.TrackHandle<IList<IResourceLocation>>(this.ResourceManager.StartOperation<IList<IResourceLocation>>(loadResourceLocationKeysOp, default(AsyncOperationHandle)));
		}

		// Token: 0x060000E2 RID: 226 RVA: 0x000041A0 File Offset: 0x000023A0
		public AsyncOperationHandle<IList<IResourceLocation>> LoadResourceLocationsWithChain(AsyncOperationHandle dep, object key, Type type)
		{
			return this.ResourceManager.CreateChainOperation<IList<IResourceLocation>>(dep, (AsyncOperationHandle op) => this.LoadResourceLocationsAsync(key, type));
		}

		// Token: 0x060000E3 RID: 227 RVA: 0x000041E0 File Offset: 0x000023E0
		public AsyncOperationHandle<IList<IResourceLocation>> LoadResourceLocationsAsync(object key, Type type = null)
		{
			if (this.ShouldChainRequest)
			{
				return this.TrackHandle<IList<IResourceLocation>>(this.LoadResourceLocationsWithChain(this.ChainOperation, key, type));
			}
			AddressablesImpl.LoadResourceLocationKeyOp loadResourceLocationKeyOp = new AddressablesImpl.LoadResourceLocationKeyOp();
			loadResourceLocationKeyOp.Init(this, type, key);
			return this.TrackHandle<IList<IResourceLocation>>(this.ResourceManager.StartOperation<IList<IResourceLocation>>(loadResourceLocationKeyOp, default(AsyncOperationHandle)));
		}

		// Token: 0x060000E4 RID: 228 RVA: 0x00004234 File Offset: 0x00002434
		public AsyncOperationHandle<IList<TObject>> LoadAssetsAsync<TObject>(IList<IResourceLocation> locations, Action<TObject> callback, bool releaseDependenciesOnFailure)
		{
			if (this.ShouldChainRequest)
			{
				return this.TrackHandle<IList<TObject>>(this.LoadAssetsWithChain<TObject>(this.ChainOperation, locations, callback, releaseDependenciesOnFailure));
			}
			return this.TrackHandle<IList<TObject>>(this.ResourceManager.ProvideResources<TObject>(locations, releaseDependenciesOnFailure, callback));
		}

		// Token: 0x060000E5 RID: 229 RVA: 0x00004268 File Offset: 0x00002468
		private AsyncOperationHandle<IList<TObject>> LoadAssetsWithChain<TObject>(AsyncOperationHandle dep, IList<IResourceLocation> locations, Action<TObject> callback, bool releaseDependenciesOnFailure)
		{
			return this.ResourceManager.CreateChainOperation<IList<TObject>>(dep, (AsyncOperationHandle op) => this.LoadAssetsAsync<TObject>(locations, callback, releaseDependenciesOnFailure));
		}

		// Token: 0x060000E6 RID: 230 RVA: 0x000042B0 File Offset: 0x000024B0
		private AsyncOperationHandle<IList<TObject>> LoadAssetsWithChain<TObject>(AsyncOperationHandle dep, IEnumerable keys, Action<TObject> callback, Addressables.MergeMode mode, bool releaseDependenciesOnFailure)
		{
			return this.ResourceManager.CreateChainOperation<IList<TObject>>(dep, (AsyncOperationHandle op) => this.LoadAssetsAsync<TObject>(keys, callback, mode, releaseDependenciesOnFailure));
		}

		// Token: 0x060000E7 RID: 231 RVA: 0x00004300 File Offset: 0x00002500
		public AsyncOperationHandle<IList<TObject>> LoadAssetsAsync<TObject>(IEnumerable keys, Action<TObject> callback, Addressables.MergeMode mode, bool releaseDependenciesOnFailure)
		{
			if (this.ShouldChainRequest)
			{
				return this.TrackHandle<IList<TObject>>(this.LoadAssetsWithChain<TObject>(this.ChainOperation, keys, callback, mode, releaseDependenciesOnFailure));
			}
			IList<IResourceLocation> list;
			if (!this.GetResourceLocations(keys, typeof(TObject), mode, out list))
			{
				return this.ResourceManager.CreateCompletedOperationWithException<IList<TObject>>(null, new InvalidKeyException(keys, typeof(TObject), mode, this));
			}
			return this.LoadAssetsAsync<TObject>(list, callback, releaseDependenciesOnFailure);
		}

		// Token: 0x060000E8 RID: 232 RVA: 0x0000436C File Offset: 0x0000256C
		private AsyncOperationHandle<IList<TObject>> LoadAssetsWithChain<TObject>(AsyncOperationHandle dep, object key, Action<TObject> callback, bool releaseDependenciesOnFailure)
		{
			return this.ResourceManager.CreateChainOperation<IList<TObject>>(dep, (AsyncOperationHandle op2) => this.LoadAssetsAsync<TObject>(key, callback, releaseDependenciesOnFailure));
		}

		// Token: 0x060000E9 RID: 233 RVA: 0x000043B4 File Offset: 0x000025B4
		public AsyncOperationHandle<IList<TObject>> LoadAssetsAsync<TObject>(object key, Action<TObject> callback, bool releaseDependenciesOnFailure)
		{
			if (this.ShouldChainRequest)
			{
				return this.TrackHandle<IList<TObject>>(this.LoadAssetsWithChain<TObject>(this.ChainOperation, key, callback, releaseDependenciesOnFailure));
			}
			IList<IResourceLocation> list;
			if (!this.GetResourceLocations(key, typeof(TObject), out list))
			{
				return this.ResourceManager.CreateCompletedOperationWithException<IList<TObject>>(null, new InvalidKeyException(key, typeof(TObject), this));
			}
			return this.LoadAssetsAsync<TObject>(list, callback, releaseDependenciesOnFailure);
		}

		// Token: 0x060000EA RID: 234 RVA: 0x0000441B File Offset: 0x0000261B
		private void OnHandleDestroyed(AsyncOperationHandle handle)
		{
			if (handle.Status == AsyncOperationStatus.Succeeded)
			{
				this.m_resultToHandle.Remove(handle.Result);
			}
		}

		// Token: 0x060000EB RID: 235 RVA: 0x0000443C File Offset: 0x0000263C
		private void OnSceneHandleCompleted(AsyncOperationHandle handle)
		{
			if (handle.Status == AsyncOperationStatus.Succeeded)
			{
				this.m_SceneInstances.Add(handle);
				if (!this.m_resultToHandle.ContainsKey(handle.Result))
				{
					this.m_resultToHandle.Add(handle.Result, handle);
					handle.Destroyed += this.m_OnHandleDestroyedAction;
				}
			}
		}

		// Token: 0x060000EC RID: 236 RVA: 0x00004494 File Offset: 0x00002694
		private void OnHandleCompleted(AsyncOperationHandle handle)
		{
			if (handle.Status == AsyncOperationStatus.Succeeded && !this.m_resultToHandle.ContainsKey(handle.Result))
			{
				this.m_resultToHandle.Add(handle.Result, handle);
				handle.Destroyed += this.m_OnHandleDestroyedAction;
			}
		}

		// Token: 0x060000ED RID: 237 RVA: 0x000044D4 File Offset: 0x000026D4
		public void Release<TObject>(TObject obj)
		{
			if (obj == null)
			{
				this.LogWarning("Addressables.Release() - trying to release null object.");
				return;
			}
			AsyncOperationHandle asyncOperationHandle;
			if (this.m_resultToHandle.TryGetValue(obj, out asyncOperationHandle))
			{
				asyncOperationHandle.Release();
				return;
			}
			this.LogError("Addressables.Release was called on an object that Addressables was not previously aware of.  Thus nothing is being released");
		}

		// Token: 0x060000EE RID: 238 RVA: 0x0000451D File Offset: 0x0000271D
		public void Release<TObject>(AsyncOperationHandle<TObject> handle)
		{
			this.m_ResourceManager.Release(handle);
		}

		// Token: 0x060000EF RID: 239 RVA: 0x00004530 File Offset: 0x00002730
		public void Release(AsyncOperationHandle handle)
		{
			this.m_ResourceManager.Release(handle);
		}

		// Token: 0x060000F0 RID: 240 RVA: 0x00004540 File Offset: 0x00002740
		private AsyncOperationHandle<long> GetDownloadSizeWithChain(AsyncOperationHandle dep, object key)
		{
			return this.ResourceManager.CreateChainOperation<long>(dep, (AsyncOperationHandle op) => this.GetDownloadSizeAsync(key));
		}

		// Token: 0x060000F1 RID: 241 RVA: 0x0000457C File Offset: 0x0000277C
		private AsyncOperationHandle<long> ComputeCatalogSizeWithChain(IResourceLocation catalogLoc)
		{
			if (!catalogLoc.HasDependencies)
			{
				return this.ResourceManager.CreateCompletedOperation<long>(0L, "Attempting to get the remote header size of a content catalog, but no dependencies pointing to a remote location could be found for location " + catalogLoc.InternalId + ". Catalog location dependencies can be setup using CreateCatalogLocationWithHashDependencies");
			}
			AsyncOperationHandle asyncOperationHandle = this.ResourceManager.ProvideResource<string>(catalogLoc.Dependencies[0]);
			return this.ResourceManager.CreateChainOperation<long>(asyncOperationHandle, delegate(AsyncOperationHandle op)
			{
				try
				{
					Hash128 hash = Hash128.Parse(op.Result.ToString());
					if (!this.IsCatalogCached(catalogLoc, hash))
					{
						return this.GetRemoteCatalogHeaderSize(catalogLoc);
					}
				}
				catch (Exception ex)
				{
					return this.ResourceManager.CreateCompletedOperation<long>(0L, string.Format("Fetching the remote catalog size failed. {0}", ex));
				}
				return this.ResourceManager.CreateCompletedOperation<long>(0L, string.Empty);
			});
		}

		// Token: 0x060000F2 RID: 242 RVA: 0x0000460C File Offset: 0x0000280C
		internal bool IsCatalogCached(IResourceLocation catalogLoc, Hash128 remoteHash)
		{
			return catalogLoc.HasDependencies && catalogLoc.Dependencies.Count == 2 && File.Exists(catalogLoc.Dependencies[1].InternalId) && !(remoteHash != Hash128.Parse(File.ReadAllText(catalogLoc.Dependencies[1].InternalId)));
		}

		// Token: 0x060000F3 RID: 243 RVA: 0x00004670 File Offset: 0x00002870
		internal AsyncOperationHandle<long> GetRemoteCatalogHeaderSize(IResourceLocation catalogLoc)
		{
			if (!catalogLoc.HasDependencies)
			{
				return this.ResourceManager.CreateCompletedOperation<long>(0L, "Attempting to get the remote header size of a content catalog, but no dependencies pointing to a remote location could be found for location " + catalogLoc.InternalId + ". Catalog location dependencies can be setup using CreateCatalogLocationWithHashDependencies");
			}
			AsyncOperationBase<UnityWebRequest> asyncOperationBase = new UnityWebRequestOperation(new UnityWebRequest(catalogLoc.Dependencies[0].InternalId.Replace(".hash", ".json"), "HEAD"));
			return this.ResourceManager.CreateChainOperation<long, UnityWebRequest>(this.ResourceManager.StartOperation<UnityWebRequest>(asyncOperationBase, default(AsyncOperationHandle)), delegate(AsyncOperationHandle<UnityWebRequest> getOp)
			{
				UnityWebRequest result = getOp.Result;
				string text = ((result != null) ? result.GetResponseHeader("Content-Length") : null);
				long num;
				if (text != null && long.TryParse(text, out num))
				{
					return this.ResourceManager.CreateCompletedOperation<long>(num, "");
				}
				return this.ResourceManager.CreateCompletedOperation<long>(0L, "Attempting to get the remote header of a catalog failed.");
			});
		}

		// Token: 0x060000F4 RID: 244 RVA: 0x00004704 File Offset: 0x00002904
		private AsyncOperationHandle<long> GetDownloadSizeWithChain(AsyncOperationHandle dep, IEnumerable keys)
		{
			return this.ResourceManager.CreateChainOperation<long>(dep, (AsyncOperationHandle op) => this.GetDownloadSizeAsync(keys));
		}

		// Token: 0x060000F5 RID: 245 RVA: 0x0000473D File Offset: 0x0000293D
		public AsyncOperationHandle<long> GetDownloadSizeAsync(object key)
		{
			return this.GetDownloadSizeAsync(new object[] { key });
		}

		// Token: 0x060000F6 RID: 246 RVA: 0x00004750 File Offset: 0x00002950
		public AsyncOperationHandle<long> GetDownloadSizeAsync(IEnumerable keys)
		{
			if (this.ShouldChainRequest)
			{
				return this.TrackHandle<long>(this.GetDownloadSizeWithChain(this.ChainOperation, keys));
			}
			List<IResourceLocation> list = new List<IResourceLocation>();
			foreach (object obj in keys)
			{
				IList<IResourceLocation> list2;
				if (obj is IList<IResourceLocation>)
				{
					list2 = obj as IList<IResourceLocation>;
				}
				else if (obj is IResourceLocation)
				{
					using (List<ResourceLocatorInfo>.Enumerator enumerator2 = this.m_ResourceLocators.GetEnumerator())
					{
						while (enumerator2.MoveNext())
						{
							if (enumerator2.Current.CatalogLocation == obj as IResourceLocation)
							{
								return this.ComputeCatalogSizeWithChain(obj as IResourceLocation);
							}
						}
					}
					list2 = new List<IResourceLocation>(1) { obj as IResourceLocation };
				}
				else if (!this.GetResourceLocations(obj, typeof(object), out list2))
				{
					return this.ResourceManager.CreateCompletedOperationWithException<long>(0L, new InvalidKeyException(obj, typeof(object), this));
				}
				foreach (IResourceLocation resourceLocation in list2)
				{
					if (resourceLocation.HasDependencies)
					{
						list.AddRange(resourceLocation.Dependencies);
					}
				}
			}
			long num = 0L;
			foreach (IResourceLocation resourceLocation2 in list.Distinct<IResourceLocation>())
			{
				ILocationSizeData locationSizeData = resourceLocation2.Data as ILocationSizeData;
				if (locationSizeData != null)
				{
					num += locationSizeData.ComputeSize(resourceLocation2, this.ResourceManager);
				}
			}
			return this.ResourceManager.CreateCompletedOperation<long>(num, string.Empty);
		}

		// Token: 0x060000F7 RID: 247 RVA: 0x00004980 File Offset: 0x00002B80
		private AsyncOperationHandle DownloadDependenciesAsyncWithChain(AsyncOperationHandle dep, object key, bool autoReleaseHandle)
		{
			AsyncOperationHandle<IList<IAssetBundleResource>> asyncOperationHandle = this.ResourceManager.CreateChainOperation<IList<IAssetBundleResource>>(dep, (AsyncOperationHandle op) => this.DownloadDependenciesAsync(key, false).Convert<IList<IAssetBundleResource>>());
			if (autoReleaseHandle)
			{
				asyncOperationHandle.ReleaseHandleOnCompletion();
			}
			return asyncOperationHandle;
		}

		// Token: 0x060000F8 RID: 248 RVA: 0x000049CC File Offset: 0x00002BCC
		internal static void WrapAsDownloadLocations(List<IResourceLocation> locations)
		{
			for (int i = 0; i < locations.Count; i++)
			{
				locations[i] = new DownloadOnlyLocation(locations[i]);
			}
		}

		// Token: 0x060000F9 RID: 249 RVA: 0x00004A00 File Offset: 0x00002C00
		private static List<IResourceLocation> GatherDependenciesFromLocations(IList<IResourceLocation> locations)
		{
			HashSet<IResourceLocation> hashSet = new HashSet<IResourceLocation>();
			foreach (IResourceLocation resourceLocation in locations)
			{
				if (resourceLocation.ResourceType == typeof(IAssetBundleResource))
				{
					hashSet.Add(resourceLocation);
				}
				if (resourceLocation.HasDependencies)
				{
					foreach (IResourceLocation resourceLocation2 in resourceLocation.Dependencies)
					{
						if (resourceLocation2.ResourceType == typeof(IAssetBundleResource))
						{
							hashSet.Add(resourceLocation2);
						}
					}
				}
			}
			return new List<IResourceLocation>(hashSet);
		}

		// Token: 0x060000FA RID: 250 RVA: 0x00004AD0 File Offset: 0x00002CD0
		public AsyncOperationHandle DownloadDependenciesAsync(object key, bool autoReleaseHandle = false)
		{
			if (this.ShouldChainRequest)
			{
				return this.DownloadDependenciesAsyncWithChain(this.ChainOperation, key, autoReleaseHandle);
			}
			IList<IResourceLocation> list;
			if (!this.GetResourceLocations(key, typeof(object), out list))
			{
				AsyncOperationHandle<IList<IAssetBundleResource>> asyncOperationHandle = this.ResourceManager.CreateCompletedOperationWithException<IList<IAssetBundleResource>>(null, new InvalidKeyException(key, typeof(object), this));
				if (autoReleaseHandle)
				{
					asyncOperationHandle.ReleaseHandleOnCompletion();
				}
				return asyncOperationHandle;
			}
			List<IResourceLocation> list2 = AddressablesImpl.GatherDependenciesFromLocations(list);
			AddressablesImpl.WrapAsDownloadLocations(list2);
			AsyncOperationHandle<IList<IAssetBundleResource>> asyncOperationHandle2 = this.LoadAssetsAsync<IAssetBundleResource>(list2, null, true);
			if (autoReleaseHandle)
			{
				asyncOperationHandle2.ReleaseHandleOnCompletion();
			}
			return asyncOperationHandle2;
		}

		// Token: 0x060000FB RID: 251 RVA: 0x00004B60 File Offset: 0x00002D60
		private AsyncOperationHandle DownloadDependenciesAsyncWithChain(AsyncOperationHandle dep, IList<IResourceLocation> locations, bool autoReleaseHandle)
		{
			AsyncOperationHandle<IList<IAssetBundleResource>> asyncOperationHandle = this.ResourceManager.CreateChainOperation<IList<IAssetBundleResource>>(dep, (AsyncOperationHandle op) => this.DownloadDependenciesAsync(locations, false).Convert<IList<IAssetBundleResource>>());
			if (autoReleaseHandle)
			{
				asyncOperationHandle.ReleaseHandleOnCompletion();
			}
			return asyncOperationHandle;
		}

		// Token: 0x060000FC RID: 252 RVA: 0x00004BAC File Offset: 0x00002DAC
		public AsyncOperationHandle DownloadDependenciesAsync(IList<IResourceLocation> locations, bool autoReleaseHandle = false)
		{
			if (this.ShouldChainRequest)
			{
				return this.DownloadDependenciesAsyncWithChain(this.ChainOperation, locations, autoReleaseHandle);
			}
			List<IResourceLocation> list = AddressablesImpl.GatherDependenciesFromLocations(locations);
			AddressablesImpl.WrapAsDownloadLocations(list);
			AsyncOperationHandle<IList<IAssetBundleResource>> asyncOperationHandle = this.LoadAssetsAsync<IAssetBundleResource>(list, null, true);
			if (autoReleaseHandle)
			{
				asyncOperationHandle.ReleaseHandleOnCompletion();
			}
			return asyncOperationHandle;
		}

		// Token: 0x060000FD RID: 253 RVA: 0x00004BF8 File Offset: 0x00002DF8
		private AsyncOperationHandle DownloadDependenciesAsyncWithChain(AsyncOperationHandle dep, IEnumerable keys, Addressables.MergeMode mode, bool autoReleaseHandle)
		{
			AsyncOperationHandle<IList<IAssetBundleResource>> asyncOperationHandle = this.ResourceManager.CreateChainOperation<IList<IAssetBundleResource>>(dep, (AsyncOperationHandle op) => this.DownloadDependenciesAsync(keys, mode, false).Convert<IList<IAssetBundleResource>>());
			if (autoReleaseHandle)
			{
				asyncOperationHandle.ReleaseHandleOnCompletion();
			}
			return asyncOperationHandle;
		}

		// Token: 0x060000FE RID: 254 RVA: 0x00004C4C File Offset: 0x00002E4C
		public AsyncOperationHandle DownloadDependenciesAsync(IEnumerable keys, Addressables.MergeMode mode, bool autoReleaseHandle = false)
		{
			if (this.ShouldChainRequest)
			{
				return this.DownloadDependenciesAsyncWithChain(this.ChainOperation, keys, mode, autoReleaseHandle);
			}
			IList<IResourceLocation> list;
			if (!this.GetResourceLocations(keys, typeof(object), mode, out list))
			{
				AsyncOperationHandle<IList<IAssetBundleResource>> asyncOperationHandle = this.ResourceManager.CreateCompletedOperationWithException<IList<IAssetBundleResource>>(null, new InvalidKeyException(keys, typeof(object), mode, this));
				if (autoReleaseHandle)
				{
					asyncOperationHandle.ReleaseHandleOnCompletion();
				}
				return asyncOperationHandle;
			}
			List<IResourceLocation> list2 = AddressablesImpl.GatherDependenciesFromLocations(list);
			AddressablesImpl.WrapAsDownloadLocations(list2);
			AsyncOperationHandle<IList<IAssetBundleResource>> asyncOperationHandle2 = this.LoadAssetsAsync<IAssetBundleResource>(list2, null, true);
			if (autoReleaseHandle)
			{
				asyncOperationHandle2.ReleaseHandleOnCompletion();
			}
			return asyncOperationHandle2;
		}

		// Token: 0x060000FF RID: 255 RVA: 0x00004CE0 File Offset: 0x00002EE0
		internal bool ClearDependencyCacheForKey(object key)
		{
			bool flag = true;
			IList<IResourceLocation> list = null;
			IList<IResourceLocation> list2;
			if (key is IResourceLocation && (key as IResourceLocation).HasDependencies)
			{
				list = AddressablesImpl.GatherDependenciesFromLocations((key as IResourceLocation).Dependencies);
			}
			else if (this.GetResourceLocations(key, typeof(object), out list2))
			{
				list = AddressablesImpl.GatherDependenciesFromLocations(list2);
			}
			if (list != null)
			{
				foreach (IResourceLocation resourceLocation in list)
				{
					AssetBundleRequestOptions assetBundleRequestOptions = resourceLocation.Data as AssetBundleRequestOptions;
					if (assetBundleRequestOptions != null)
					{
						string bundleName = assetBundleRequestOptions.BundleName;
						if (this.m_ResourceManager.GetOperationFromCache(resourceLocation, typeof(IAssetBundleResource)) != null)
						{
							Debug.LogWarning(string.Concat(new string[] { "Attempting to clear cached version including ", bundleName, ", while ", bundleName, " is currently loaded." }));
							if (!string.IsNullOrEmpty(assetBundleRequestOptions.Hash))
							{
								Hash128 hash = Hash128.Parse(assetBundleRequestOptions.Hash);
								Caching.ClearOtherCachedVersions(bundleName, hash);
							}
						}
						else
						{
							flag = flag && Caching.ClearAllCachedVersions(bundleName);
						}
					}
				}
			}
			return flag;
		}

		// Token: 0x06000100 RID: 256 RVA: 0x00004E18 File Offset: 0x00003018
		internal void AutoReleaseHandleOnTypelessCompletion<TObject>(AsyncOperationHandle<TObject> handle)
		{
			handle.CompletedTypeless += delegate(AsyncOperationHandle op)
			{
				op.Release();
			};
		}

		// Token: 0x06000101 RID: 257 RVA: 0x00004E40 File Offset: 0x00003040
		public AsyncOperationHandle<bool> ClearDependencyCacheAsync(object key, bool autoReleaseHandle)
		{
			if (this.ShouldChainRequest)
			{
				AsyncOperationHandle<bool> asyncOperationHandle = this.ResourceManager.CreateChainOperation<bool>(this.ChainOperation, (AsyncOperationHandle op) => this.ClearDependencyCacheAsync(key, autoReleaseHandle));
				if (autoReleaseHandle)
				{
					asyncOperationHandle.ReleaseHandleOnCompletion();
				}
				return asyncOperationHandle;
			}
			bool flag = this.ClearDependencyCacheForKey(key);
			AsyncOperationHandle<bool> asyncOperationHandle2 = this.ResourceManager.CreateCompletedOperation<bool>(flag, flag ? string.Empty : "Unable to clear the cache.  AssetBundle's may still be loaded for the given key.");
			if (autoReleaseHandle)
			{
				asyncOperationHandle2.ReleaseHandleOnCompletion();
			}
			return asyncOperationHandle2;
		}

		// Token: 0x06000102 RID: 258 RVA: 0x00004ED8 File Offset: 0x000030D8
		public AsyncOperationHandle<bool> ClearDependencyCacheAsync(IList<IResourceLocation> locations, bool autoReleaseHandle)
		{
			if (this.ShouldChainRequest)
			{
				AsyncOperationHandle<bool> asyncOperationHandle = this.ResourceManager.CreateChainOperation<bool>(this.ChainOperation, (AsyncOperationHandle op) => this.ClearDependencyCacheAsync(locations, autoReleaseHandle));
				if (autoReleaseHandle)
				{
					asyncOperationHandle.ReleaseHandleOnCompletion();
				}
				return asyncOperationHandle;
			}
			bool flag = true;
			foreach (IResourceLocation resourceLocation in locations)
			{
				flag = flag && this.ClearDependencyCacheForKey(resourceLocation);
			}
			AsyncOperationHandle<bool> asyncOperationHandle2 = this.ResourceManager.CreateCompletedOperation<bool>(flag, flag ? string.Empty : "Unable to clear the cache.  AssetBundle's may still be loaded for the given key(s).");
			if (autoReleaseHandle)
			{
				asyncOperationHandle2.ReleaseHandleOnCompletion();
			}
			return asyncOperationHandle2;
		}

		// Token: 0x06000103 RID: 259 RVA: 0x00004FB4 File Offset: 0x000031B4
		public AsyncOperationHandle<bool> ClearDependencyCacheAsync(IEnumerable keys, bool autoReleaseHandle)
		{
			if (this.ShouldChainRequest)
			{
				AsyncOperationHandle<bool> asyncOperationHandle = this.ResourceManager.CreateChainOperation<bool>(this.ChainOperation, (AsyncOperationHandle op) => this.ClearDependencyCacheAsync(keys, autoReleaseHandle));
				if (autoReleaseHandle)
				{
					asyncOperationHandle.ReleaseHandleOnCompletion();
				}
				return asyncOperationHandle;
			}
			bool flag = true;
			foreach (object obj in keys)
			{
				flag = flag && this.ClearDependencyCacheForKey(obj);
			}
			AsyncOperationHandle<bool> asyncOperationHandle2 = this.ResourceManager.CreateCompletedOperation<bool>(flag, flag ? string.Empty : "Unable to clear the cache.  AssetBundle's may still be loaded for the given key(s).");
			if (autoReleaseHandle)
			{
				asyncOperationHandle2.ReleaseHandleOnCompletion();
			}
			return asyncOperationHandle2;
		}

		// Token: 0x06000104 RID: 260 RVA: 0x00005098 File Offset: 0x00003298
		public AsyncOperationHandle<GameObject> InstantiateAsync(IResourceLocation location, Transform parent = null, bool instantiateInWorldSpace = false, bool trackHandle = true)
		{
			return this.InstantiateAsync(location, new InstantiationParameters(parent, instantiateInWorldSpace), trackHandle);
		}

		// Token: 0x06000105 RID: 261 RVA: 0x000050AA File Offset: 0x000032AA
		public AsyncOperationHandle<GameObject> InstantiateAsync(IResourceLocation location, Vector3 position, Quaternion rotation, Transform parent = null, bool trackHandle = true)
		{
			return this.InstantiateAsync(location, new InstantiationParameters(position, rotation, parent), trackHandle);
		}

		// Token: 0x06000106 RID: 262 RVA: 0x000050BE File Offset: 0x000032BE
		public AsyncOperationHandle<GameObject> InstantiateAsync(object key, Transform parent = null, bool instantiateInWorldSpace = false, bool trackHandle = true)
		{
			return this.InstantiateAsync(key, new InstantiationParameters(parent, instantiateInWorldSpace), trackHandle);
		}

		// Token: 0x06000107 RID: 263 RVA: 0x000050D0 File Offset: 0x000032D0
		public AsyncOperationHandle<GameObject> InstantiateAsync(object key, Vector3 position, Quaternion rotation, Transform parent = null, bool trackHandle = true)
		{
			return this.InstantiateAsync(key, new InstantiationParameters(position, rotation, parent), trackHandle);
		}

		// Token: 0x06000108 RID: 264 RVA: 0x000050E4 File Offset: 0x000032E4
		private AsyncOperationHandle<GameObject> InstantiateWithChain(AsyncOperationHandle dep, object key, InstantiationParameters instantiateParameters, bool trackHandle = true)
		{
			AsyncOperationHandle<GameObject> asyncOperationHandle = this.ResourceManager.CreateChainOperation<GameObject>(dep, (AsyncOperationHandle op) => this.InstantiateAsync(key, instantiateParameters, false));
			if (trackHandle)
			{
				asyncOperationHandle.CompletedTypeless += this.m_OnHandleCompleteAction;
			}
			return asyncOperationHandle;
		}

		// Token: 0x06000109 RID: 265 RVA: 0x00005138 File Offset: 0x00003338
		public AsyncOperationHandle<GameObject> InstantiateAsync(object key, InstantiationParameters instantiateParameters, bool trackHandle = true)
		{
			if (this.ShouldChainRequest)
			{
				return this.InstantiateWithChain(this.ChainOperation, key, instantiateParameters, trackHandle);
			}
			key = this.EvaluateKey(key);
			using (List<ResourceLocatorInfo>.Enumerator enumerator = this.m_ResourceLocators.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					IList<IResourceLocation> list;
					if (enumerator.Current.Locator.Locate(key, typeof(GameObject), out list))
					{
						return this.InstantiateAsync(list[0], instantiateParameters, trackHandle);
					}
				}
			}
			return this.ResourceManager.CreateCompletedOperationWithException<GameObject>(null, new InvalidKeyException(key, typeof(GameObject), this));
		}

		// Token: 0x0600010A RID: 266 RVA: 0x000051F0 File Offset: 0x000033F0
		private AsyncOperationHandle<GameObject> InstantiateWithChain(AsyncOperationHandle dep, IResourceLocation location, InstantiationParameters instantiateParameters, bool trackHandle = true)
		{
			AsyncOperationHandle<GameObject> asyncOperationHandle = this.ResourceManager.CreateChainOperation<GameObject>(dep, (AsyncOperationHandle op) => this.InstantiateAsync(location, instantiateParameters, false));
			if (trackHandle)
			{
				asyncOperationHandle.CompletedTypeless += this.m_OnHandleCompleteAction;
			}
			return asyncOperationHandle;
		}

		// Token: 0x0600010B RID: 267 RVA: 0x00005244 File Offset: 0x00003444
		public AsyncOperationHandle<GameObject> InstantiateAsync(IResourceLocation location, InstantiationParameters instantiateParameters, bool trackHandle = true)
		{
			if (this.ShouldChainRequest)
			{
				return this.InstantiateWithChain(this.ChainOperation, location, instantiateParameters, trackHandle);
			}
			AsyncOperationHandle<GameObject> asyncOperationHandle = this.ResourceManager.ProvideInstance(this.InstanceProvider, location, instantiateParameters);
			if (!trackHandle)
			{
				return asyncOperationHandle;
			}
			asyncOperationHandle.CompletedTypeless += this.m_OnHandleCompleteAction;
			return asyncOperationHandle;
		}

		// Token: 0x0600010C RID: 268 RVA: 0x00005290 File Offset: 0x00003490
		public bool ReleaseInstance(GameObject instance)
		{
			if (instance == null)
			{
				this.LogWarning("Addressables.ReleaseInstance() - trying to release null object.");
				return false;
			}
			AsyncOperationHandle asyncOperationHandle;
			if (this.m_resultToHandle.TryGetValue(instance, out asyncOperationHandle))
			{
				asyncOperationHandle.Release();
				return true;
			}
			return false;
		}

		// Token: 0x0600010D RID: 269 RVA: 0x000052D0 File Offset: 0x000034D0
		internal AsyncOperationHandle<SceneInstance> LoadSceneWithChain(AsyncOperationHandle dep, object key, LoadSceneParameters loadSceneParameters, bool activateOnLoad = true, int priority = 100)
		{
			return this.TrackHandle(this.ResourceManager.CreateChainOperation<SceneInstance>(dep, (AsyncOperationHandle op) => this.LoadSceneAsync(key, loadSceneParameters, activateOnLoad, priority, false)));
		}

		// Token: 0x0600010E RID: 270 RVA: 0x00005328 File Offset: 0x00003528
		internal AsyncOperationHandle<SceneInstance> LoadSceneWithChain(AsyncOperationHandle dep, IResourceLocation key, LoadSceneMode loadMode = LoadSceneMode.Single, bool activateOnLoad = true, int priority = 100)
		{
			return this.TrackHandle(this.ResourceManager.CreateChainOperation<SceneInstance>(dep, (AsyncOperationHandle op) => this.LoadSceneAsync(key, loadMode, activateOnLoad, priority, false)));
		}

		// Token: 0x0600010F RID: 271 RVA: 0x0000537E File Offset: 0x0000357E
		public AsyncOperationHandle<SceneInstance> LoadSceneAsync(object key)
		{
			return this.LoadSceneAsync(key, new LoadSceneParameters(LoadSceneMode.Single), true, 100, true);
		}

		// Token: 0x06000110 RID: 272 RVA: 0x00005394 File Offset: 0x00003594
		public AsyncOperationHandle<SceneInstance> LoadSceneAsync(object key, LoadSceneParameters loadSceneParameters, bool activateOnLoad = true, int priority = 100, bool trackHandle = true)
		{
			if (this.ShouldChainRequest)
			{
				return this.LoadSceneWithChain(this.ChainOperation, key, loadSceneParameters, activateOnLoad, priority);
			}
			IList<IResourceLocation> list;
			if (!this.GetResourceLocations(key, typeof(SceneInstance), out list))
			{
				return this.ResourceManager.CreateCompletedOperationWithException<SceneInstance>(default(SceneInstance), new InvalidKeyException(key, typeof(SceneInstance), this));
			}
			return this.LoadSceneAsync(list[0], loadSceneParameters, activateOnLoad, priority, trackHandle);
		}

		// Token: 0x06000111 RID: 273 RVA: 0x00005409 File Offset: 0x00003609
		public AsyncOperationHandle<SceneInstance> LoadSceneAsync(IResourceLocation location)
		{
			return this.LoadSceneAsync(location, new LoadSceneParameters(LoadSceneMode.Single), true, 100, true);
		}

		// Token: 0x06000112 RID: 274 RVA: 0x0000541C File Offset: 0x0000361C
		public AsyncOperationHandle<SceneInstance> LoadSceneAsync(IResourceLocation location, LoadSceneMode loadMode = LoadSceneMode.Single, bool activateOnLoad = true, int priority = 100, bool trackHandle = true)
		{
			return this.LoadSceneAsync(location, new LoadSceneParameters(loadMode), activateOnLoad, priority, trackHandle);
		}

		// Token: 0x06000113 RID: 275 RVA: 0x00005430 File Offset: 0x00003630
		public AsyncOperationHandle<SceneInstance> LoadSceneAsync(IResourceLocation location, LoadSceneParameters loadSceneParameters, bool activateOnLoad = true, int priority = 100, bool trackHandle = true)
		{
			if (this.ShouldChainRequest)
			{
				return this.LoadSceneWithChain(this.ChainOperation, location, loadSceneParameters, activateOnLoad, priority);
			}
			AsyncOperationHandle<SceneInstance> asyncOperationHandle = this.ResourceManager.ProvideScene(this.SceneProvider, location, loadSceneParameters, activateOnLoad, priority);
			if (trackHandle)
			{
				return this.TrackHandle(asyncOperationHandle);
			}
			return asyncOperationHandle;
		}

		// Token: 0x06000114 RID: 276 RVA: 0x0000547C File Offset: 0x0000367C
		public AsyncOperationHandle<SceneInstance> UnloadSceneAsync(SceneInstance scene, UnloadSceneOptions unloadOptions = UnloadSceneOptions.None, bool autoReleaseHandle = true)
		{
			AsyncOperationHandle asyncOperationHandle;
			if (!this.m_resultToHandle.TryGetValue(scene, out asyncOperationHandle))
			{
				string text = string.Format("Addressables.UnloadSceneAsync() - Cannot find handle for scene {0}", scene);
				this.LogWarning(text);
				return this.ResourceManager.CreateCompletedOperation<SceneInstance>(scene, text);
			}
			if (asyncOperationHandle.m_InternalOp.IsRunning)
			{
				return this.CreateUnloadSceneWithChain(asyncOperationHandle, unloadOptions, autoReleaseHandle);
			}
			return this.UnloadSceneAsync(asyncOperationHandle, unloadOptions, autoReleaseHandle);
		}

		// Token: 0x06000115 RID: 277 RVA: 0x000054E4 File Offset: 0x000036E4
		public AsyncOperationHandle<SceneInstance> UnloadSceneAsync(AsyncOperationHandle handle, UnloadSceneOptions unloadOptions = UnloadSceneOptions.None, bool autoReleaseHandle = true)
		{
			if (handle.m_InternalOp.IsRunning)
			{
				return this.CreateUnloadSceneWithChain(handle, unloadOptions, autoReleaseHandle);
			}
			return this.UnloadSceneAsync(handle.Convert<SceneInstance>(), unloadOptions, autoReleaseHandle);
		}

		// Token: 0x06000116 RID: 278 RVA: 0x0000550C File Offset: 0x0000370C
		public AsyncOperationHandle<SceneInstance> UnloadSceneAsync(AsyncOperationHandle<SceneInstance> handle, UnloadSceneOptions unloadOptions = UnloadSceneOptions.None, bool autoReleaseHandle = true)
		{
			if (handle.m_InternalOp.IsRunning)
			{
				return this.CreateUnloadSceneWithChain(handle, unloadOptions, autoReleaseHandle);
			}
			return this.InternalUnloadScene(handle, unloadOptions, autoReleaseHandle);
		}

		// Token: 0x06000117 RID: 279 RVA: 0x00005530 File Offset: 0x00003730
		internal AsyncOperationHandle<SceneInstance> CreateUnloadSceneWithChain(AsyncOperationHandle handle, UnloadSceneOptions unloadOptions, bool autoReleaseHandle)
		{
			return this.m_ResourceManager.CreateChainOperation<SceneInstance>(handle, (AsyncOperationHandle completedHandle) => this.InternalUnloadScene(completedHandle.Convert<SceneInstance>(), unloadOptions, autoReleaseHandle));
		}

		// Token: 0x06000118 RID: 280 RVA: 0x00005570 File Offset: 0x00003770
		internal AsyncOperationHandle<SceneInstance> CreateUnloadSceneWithChain(AsyncOperationHandle<SceneInstance> handle, UnloadSceneOptions unloadOptions, bool autoReleaseHandle)
		{
			return this.m_ResourceManager.CreateChainOperation<SceneInstance, SceneInstance>(handle, (AsyncOperationHandle<SceneInstance> completedHandle) => this.InternalUnloadScene(completedHandle, unloadOptions, autoReleaseHandle));
		}

		// Token: 0x06000119 RID: 281 RVA: 0x000055B0 File Offset: 0x000037B0
		internal AsyncOperationHandle<SceneInstance> InternalUnloadScene(AsyncOperationHandle<SceneInstance> handle, UnloadSceneOptions unloadOptions, bool autoReleaseHandle)
		{
			AsyncOperationHandle<SceneInstance> asyncOperationHandle = this.SceneProvider.ReleaseScene(this.ResourceManager, handle, unloadOptions);
			if (autoReleaseHandle)
			{
				asyncOperationHandle.ReleaseHandleOnCompletion();
			}
			return asyncOperationHandle;
		}

		// Token: 0x0600011A RID: 282 RVA: 0x000055DC File Offset: 0x000037DC
		private object EvaluateKey(object obj)
		{
			if (obj is IKeyEvaluator)
			{
				return (obj as IKeyEvaluator).RuntimeKey;
			}
			return obj;
		}

		// Token: 0x0600011B RID: 283 RVA: 0x000055F4 File Offset: 0x000037F4
		internal AsyncOperationHandle<List<string>> CheckForCatalogUpdates(bool autoReleaseHandle = true)
		{
			if (this.ShouldChainRequest)
			{
				return this.CheckForCatalogUpdatesWithChain(autoReleaseHandle);
			}
			if (this.m_ActiveCheckUpdateOperation.IsValid())
			{
				this.m_ActiveCheckUpdateOperation.Release();
			}
			this.m_ActiveCheckUpdateOperation = new CheckCatalogsOperation(this).Start(this.m_ResourceLocators);
			if (autoReleaseHandle)
			{
				this.AutoReleaseHandleOnTypelessCompletion<List<string>>(this.m_ActiveCheckUpdateOperation);
			}
			return this.m_ActiveCheckUpdateOperation;
		}

		// Token: 0x0600011C RID: 284 RVA: 0x00005658 File Offset: 0x00003858
		internal AsyncOperationHandle<List<string>> CheckForCatalogUpdatesWithChain(bool autoReleaseHandle)
		{
			return this.ResourceManager.CreateChainOperation<List<string>>(this.ChainOperation, (AsyncOperationHandle op) => this.CheckForCatalogUpdates(autoReleaseHandle));
		}

		// Token: 0x0600011D RID: 285 RVA: 0x00005698 File Offset: 0x00003898
		public ResourceLocatorInfo GetLocatorInfo(string c)
		{
			foreach (ResourceLocatorInfo resourceLocatorInfo in this.m_ResourceLocators)
			{
				if (resourceLocatorInfo.Locator.LocatorId == c)
				{
					return resourceLocatorInfo;
				}
			}
			return null;
		}

		// Token: 0x17000028 RID: 40
		// (get) Token: 0x0600011E RID: 286 RVA: 0x00005700 File Offset: 0x00003900
		internal IEnumerable<string> CatalogsWithAvailableUpdates
		{
			get
			{
				return from s in this.m_ResourceLocators
					where s.ContentUpdateAvailable
					select s.Locator.LocatorId;
			}
		}

		// Token: 0x0600011F RID: 287 RVA: 0x0000575C File Offset: 0x0000395C
		internal AsyncOperationHandle<List<IResourceLocator>> UpdateCatalogs(IEnumerable<string> catalogIds = null, bool autoReleaseHandle = true, bool autoCleanBundleCache = false)
		{
			if (this.m_ActiveUpdateOperation.IsValid())
			{
				return this.m_ActiveUpdateOperation;
			}
			if (catalogIds == null && !this.CatalogsWithAvailableUpdates.Any<string>())
			{
				return this.m_ResourceManager.CreateChainOperation<List<IResourceLocator>, List<string>>(this.CheckForCatalogUpdates(true), (AsyncOperationHandle<List<string>> depOp) => this.UpdateCatalogs(this.CatalogsWithAvailableUpdates, autoReleaseHandle, autoCleanBundleCache));
			}
			AsyncOperationHandle<List<IResourceLocator>> asyncOperationHandle = new UpdateCatalogsOperation(this).Start((catalogIds == null) ? this.CatalogsWithAvailableUpdates : catalogIds, autoCleanBundleCache);
			if (autoReleaseHandle)
			{
				this.AutoReleaseHandleOnTypelessCompletion<List<IResourceLocator>>(asyncOperationHandle);
			}
			return asyncOperationHandle;
		}

		// Token: 0x06000120 RID: 288 RVA: 0x000057F5 File Offset: 0x000039F5
		public bool Equals(IResourceLocation x, IResourceLocation y)
		{
			return x.PrimaryKey.Equals(y.PrimaryKey) && x.ResourceType.Equals(y.ResourceType) && x.InternalId.Equals(y.InternalId);
		}

		// Token: 0x06000121 RID: 289 RVA: 0x00005830 File Offset: 0x00003A30
		public int GetHashCode(IResourceLocation loc)
		{
			return loc.PrimaryKey.GetHashCode() * 31 + loc.ResourceType.GetHashCode();
		}

		// Token: 0x06000122 RID: 290 RVA: 0x0000584C File Offset: 0x00003A4C
		internal AsyncOperationHandle<bool> CleanBundleCache(IEnumerable<string> catalogIds, bool forceSingleThreading)
		{
			if (this.ShouldChainRequest)
			{
				return this.CleanBundleCacheWithChain(catalogIds, forceSingleThreading);
			}
			if (catalogIds == null)
			{
				catalogIds = this.m_ResourceLocators.Select<ResourceLocatorInfo, string>((ResourceLocatorInfo s) => s.Locator.LocatorId);
			}
			List<IResourceLocation> list = new List<IResourceLocation>();
			foreach (string text in catalogIds)
			{
				if (text != null)
				{
					ResourceLocatorInfo locatorInfo = this.GetLocatorInfo(text);
					if (locatorInfo != null && locatorInfo.CatalogLocation != null)
					{
						list.Add(locatorInfo.CatalogLocation);
					}
				}
			}
			if (list.Count == 0)
			{
				return this.ResourceManager.CreateCompletedOperation<bool>(false, "Provided catalogs do not load data from a catalog file. This can occur when using the \"Use Asset Database (fastest)\" playmode script. Bundle cache was not modified.");
			}
			return this.CleanBundleCache(this.ResourceManager.CreateGroupOperation<object>(list), forceSingleThreading);
		}

		// Token: 0x06000123 RID: 291 RVA: 0x00005924 File Offset: 0x00003B24
		internal AsyncOperationHandle<bool> CleanBundleCache(AsyncOperationHandle<IList<AsyncOperationHandle>> depOp, bool forceSingleThreading)
		{
			if (this.ShouldChainRequest)
			{
				return this.CleanBundleCacheWithChain(depOp, forceSingleThreading);
			}
			if (this.m_ActiveCleanBundleCacheOperation.IsValid() && !this.m_ActiveCleanBundleCacheOperation.IsDone)
			{
				return this.ResourceManager.CreateCompletedOperation<bool>(false, "Bundle cache is already being cleaned.");
			}
			this.m_ActiveCleanBundleCacheOperation = new CleanBundleCacheOperation(this, forceSingleThreading).Start(depOp);
			return this.m_ActiveCleanBundleCacheOperation;
		}

		// Token: 0x06000124 RID: 292 RVA: 0x00005988 File Offset: 0x00003B88
		internal AsyncOperationHandle<bool> CleanBundleCacheWithChain(AsyncOperationHandle<IList<AsyncOperationHandle>> depOp, bool forceSingleThreading)
		{
			return this.ResourceManager.CreateChainOperation<bool>(this.ChainOperation, (AsyncOperationHandle op) => this.CleanBundleCache(depOp, forceSingleThreading));
		}

		// Token: 0x06000125 RID: 293 RVA: 0x000059D0 File Offset: 0x00003BD0
		internal AsyncOperationHandle<bool> CleanBundleCacheWithChain(IEnumerable<string> catalogIds, bool forceSingleThreading)
		{
			return this.ResourceManager.CreateChainOperation<bool>(this.ChainOperation, (AsyncOperationHandle op) => this.CleanBundleCache(catalogIds, forceSingleThreading));
		}

		// Token: 0x04000017 RID: 23
		private ResourceManager m_ResourceManager;

		// Token: 0x04000018 RID: 24
		private IInstanceProvider m_InstanceProvider;

		// Token: 0x04000019 RID: 25
		private int m_CatalogRequestsTimeout;

		// Token: 0x0400001A RID: 26
		internal const string kCacheDataFolder = "{UnityEngine.Application.persistentDataPath}/com.unity.addressables/";

		// Token: 0x0400001B RID: 27
		public ISceneProvider SceneProvider;

		// Token: 0x0400001C RID: 28
		internal List<ResourceLocatorInfo> m_ResourceLocators = new List<ResourceLocatorInfo>();

		// Token: 0x0400001D RID: 29
		private AsyncOperationHandle<IResourceLocator> m_InitializationOperation;

		// Token: 0x0400001E RID: 30
		private AsyncOperationHandle<List<string>> m_ActiveCheckUpdateOperation;

		// Token: 0x0400001F RID: 31
		internal AsyncOperationHandle<List<IResourceLocator>> m_ActiveUpdateOperation;

		// Token: 0x04000020 RID: 32
		private Action<AsyncOperationHandle> m_OnHandleCompleteAction;

		// Token: 0x04000021 RID: 33
		private Action<AsyncOperationHandle> m_OnSceneHandleCompleteAction;

		// Token: 0x04000022 RID: 34
		private Action<AsyncOperationHandle> m_OnHandleDestroyedAction;

		// Token: 0x04000023 RID: 35
		private Dictionary<object, AsyncOperationHandle> m_resultToHandle = new Dictionary<object, AsyncOperationHandle>();

		// Token: 0x04000024 RID: 36
		internal HashSet<AsyncOperationHandle> m_SceneInstances = new HashSet<AsyncOperationHandle>();

		// Token: 0x04000025 RID: 37
		private AsyncOperationHandle<bool> m_ActiveCleanBundleCacheOperation;

		// Token: 0x04000026 RID: 38
		internal bool hasStartedInitialization;

		// Token: 0x0200002F RID: 47
		private class LoadResourceLocationKeyOp : AsyncOperationBase<IList<IResourceLocation>>
		{
			// Token: 0x17000065 RID: 101
			// (get) Token: 0x06000209 RID: 521 RVA: 0x00009131 File Offset: 0x00007331
			protected override string DebugName
			{
				get
				{
					return this.m_Keys.ToString();
				}
			}

			// Token: 0x0600020A RID: 522 RVA: 0x0000913E File Offset: 0x0000733E
			public void Init(AddressablesImpl aa, Type t, object keys)
			{
				this.m_Keys = keys;
				this.m_ResourceType = t;
				this.m_Addressables = aa;
			}

			// Token: 0x0600020B RID: 523 RVA: 0x00009155 File Offset: 0x00007355
			protected override bool InvokeWaitForCompletion()
			{
				ResourceManager rm = this.m_RM;
				if (rm != null)
				{
					rm.Update(Time.unscaledDeltaTime);
				}
				if (!this.HasExecuted)
				{
					base.InvokeExecute();
				}
				return true;
			}

			// Token: 0x0600020C RID: 524 RVA: 0x0000917C File Offset: 0x0000737C
			protected override void Execute()
			{
				this.m_Addressables.GetResourceLocations(this.m_Keys, this.m_ResourceType, out this.m_locations);
				if (this.m_locations == null)
				{
					this.m_locations = new List<IResourceLocation>();
				}
				base.Complete(this.m_locations, true, string.Empty);
			}

			// Token: 0x040000A1 RID: 161
			private object m_Keys;

			// Token: 0x040000A2 RID: 162
			private IList<IResourceLocation> m_locations;

			// Token: 0x040000A3 RID: 163
			private AddressablesImpl m_Addressables;

			// Token: 0x040000A4 RID: 164
			private Type m_ResourceType;
		}

		// Token: 0x02000030 RID: 48
		private class LoadResourceLocationKeysOp : AsyncOperationBase<IList<IResourceLocation>>
		{
			// Token: 0x17000066 RID: 102
			// (get) Token: 0x0600020E RID: 526 RVA: 0x000091D4 File Offset: 0x000073D4
			protected override string DebugName
			{
				get
				{
					return "LoadResourceLocationKeysOp";
				}
			}

			// Token: 0x0600020F RID: 527 RVA: 0x000091DB File Offset: 0x000073DB
			public void Init(AddressablesImpl aa, Type t, IEnumerable key, Addressables.MergeMode mergeMode)
			{
				this.m_Key = key;
				this.m_ResourceType = t;
				this.m_MergeMode = mergeMode;
				this.m_Addressables = aa;
			}

			// Token: 0x06000210 RID: 528 RVA: 0x000091FC File Offset: 0x000073FC
			protected override void Execute()
			{
				this.m_Addressables.GetResourceLocations(this.m_Key, this.m_ResourceType, this.m_MergeMode, out this.m_locations);
				if (this.m_locations == null)
				{
					this.m_locations = new List<IResourceLocation>();
				}
				base.Complete(this.m_locations, true, string.Empty);
			}

			// Token: 0x06000211 RID: 529 RVA: 0x00009252 File Offset: 0x00007452
			protected override bool InvokeWaitForCompletion()
			{
				ResourceManager rm = this.m_RM;
				if (rm != null)
				{
					rm.Update(Time.unscaledDeltaTime);
				}
				if (!this.HasExecuted)
				{
					base.InvokeExecute();
				}
				return true;
			}

			// Token: 0x040000A5 RID: 165
			private IEnumerable m_Key;

			// Token: 0x040000A6 RID: 166
			private Addressables.MergeMode m_MergeMode;

			// Token: 0x040000A7 RID: 167
			private IList<IResourceLocation> m_locations;

			// Token: 0x040000A8 RID: 168
			private AddressablesImpl m_Addressables;

			// Token: 0x040000A9 RID: 169
			private Type m_ResourceType;
		}
	}
}
