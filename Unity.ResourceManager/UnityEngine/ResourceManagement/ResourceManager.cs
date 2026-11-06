using System;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.Exceptions;
using UnityEngine.ResourceManagement.Profiling;
using UnityEngine.ResourceManagement.ResourceLocations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.ResourceManagement.Util;
using UnityEngine.SceneManagement;

namespace UnityEngine.ResourceManagement
{
	// Token: 0x0200000A RID: 10
	public class ResourceManager : IDisposable
	{
		// Token: 0x1700000A RID: 10
		// (get) Token: 0x06000042 RID: 66 RVA: 0x00002CA9 File Offset: 0x00000EA9
		// (set) Token: 0x06000043 RID: 67 RVA: 0x00002CB0 File Offset: 0x00000EB0
		public static Action<AsyncOperationHandle, Exception> ExceptionHandler { get; set; }

		// Token: 0x1700000B RID: 11
		// (get) Token: 0x06000044 RID: 68 RVA: 0x00002CB8 File Offset: 0x00000EB8
		// (set) Token: 0x06000045 RID: 69 RVA: 0x00002CC0 File Offset: 0x00000EC0
		public Func<IResourceLocation, string> InternalIdTransformFunc { get; set; }

		// Token: 0x06000046 RID: 70 RVA: 0x00002CC9 File Offset: 0x00000EC9
		public string TransformInternalId(IResourceLocation location)
		{
			if (this.InternalIdTransformFunc != null)
			{
				return this.InternalIdTransformFunc(location);
			}
			return location.InternalId;
		}

		// Token: 0x1700000C RID: 12
		// (get) Token: 0x06000047 RID: 71 RVA: 0x00002CE6 File Offset: 0x00000EE6
		// (set) Token: 0x06000048 RID: 72 RVA: 0x00002CEE File Offset: 0x00000EEE
		public Action<UnityWebRequest> WebRequestOverride { get; set; }

		// Token: 0x1700000D RID: 13
		// (get) Token: 0x06000049 RID: 73 RVA: 0x00002CF7 File Offset: 0x00000EF7
		internal int OperationCacheCount
		{
			get
			{
				return this.m_AssetOperationCache.Count;
			}
		}

		// Token: 0x1700000E RID: 14
		// (get) Token: 0x0600004A RID: 74 RVA: 0x00002D04 File Offset: 0x00000F04
		internal int InstanceOperationCount
		{
			get
			{
				return this.m_TrackedInstanceOperations.Count;
			}
		}

		// Token: 0x1700000F RID: 15
		// (get) Token: 0x0600004B RID: 75 RVA: 0x00002D11 File Offset: 0x00000F11
		internal int DeferredCompleteCallbacksCount
		{
			get
			{
				return this.m_DeferredCompleteCallbacks.Count;
			}
		}

		// Token: 0x17000010 RID: 16
		// (get) Token: 0x0600004C RID: 76 RVA: 0x00002D1E File Offset: 0x00000F1E
		internal int DeferredCallbackCount
		{
			get
			{
				List<ResourceManager.DeferredCallbackRegisterRequest> deferredCallbacksToRegister = this.m_DeferredCallbacksToRegister;
				if (deferredCallbacksToRegister == null)
				{
					return 0;
				}
				return deferredCallbacksToRegister.Count;
			}
		}

		// Token: 0x0600004D RID: 77 RVA: 0x00002D31 File Offset: 0x00000F31
		public void AddUpdateReceiver(IUpdateReceiver receiver)
		{
			if (receiver == null)
			{
				return;
			}
			this.m_UpdateReceivers.Add(receiver);
		}

		// Token: 0x0600004E RID: 78 RVA: 0x00002D43 File Offset: 0x00000F43
		public void RemoveUpdateReciever(IUpdateReceiver receiver)
		{
			if (receiver == null)
			{
				return;
			}
			if (this.m_UpdatingReceivers)
			{
				if (this.m_UpdateReceiversToRemove == null)
				{
					this.m_UpdateReceiversToRemove = new List<IUpdateReceiver>();
				}
				this.m_UpdateReceiversToRemove.Add(receiver);
				return;
			}
			this.m_UpdateReceivers.Remove(receiver);
		}

		// Token: 0x17000011 RID: 17
		// (get) Token: 0x0600004F RID: 79 RVA: 0x00002D7E File Offset: 0x00000F7E
		// (set) Token: 0x06000050 RID: 80 RVA: 0x00002D86 File Offset: 0x00000F86
		public IAllocationStrategy Allocator
		{
			get
			{
				return this.m_allocator;
			}
			set
			{
				this.m_allocator = value;
			}
		}

		// Token: 0x17000012 RID: 18
		// (get) Token: 0x06000051 RID: 81 RVA: 0x00002D8F File Offset: 0x00000F8F
		public IList<IResourceProvider> ResourceProviders
		{
			get
			{
				return this.m_ResourceProviders;
			}
		}

		// Token: 0x17000013 RID: 19
		// (get) Token: 0x06000052 RID: 82 RVA: 0x00002D97 File Offset: 0x00000F97
		// (set) Token: 0x06000053 RID: 83 RVA: 0x00002D9F File Offset: 0x00000F9F
		public CertificateHandler CertificateHandlerInstance { get; set; }

		// Token: 0x06000054 RID: 84 RVA: 0x00002DA8 File Offset: 0x00000FA8
		public ResourceManager(IAllocationStrategy alloc = null)
		{
			this.m_ReleaseOpNonCached = new Action<IAsyncOperation>(this.OnOperationDestroyNonCached);
			this.m_ReleaseOpCached = new Action<IAsyncOperation>(this.OnOperationDestroyCached);
			this.m_ReleaseInstanceOp = new Action<IAsyncOperation>(this.OnInstanceOperationDestroy);
			IAllocationStrategy allocationStrategy;
			if (alloc != null)
			{
				allocationStrategy = alloc;
			}
			else
			{
				IAllocationStrategy allocationStrategy2 = new LRUCacheAllocationStrategy(1000, 1000, 100, 10);
				allocationStrategy = allocationStrategy2;
			}
			this.m_allocator = allocationStrategy;
			this.m_ResourceProviders.OnElementAdded += new Action<IResourceProvider>(this.OnObjectAdded);
			this.m_ResourceProviders.OnElementRemoved += new Action<IResourceProvider>(this.OnObjectRemoved);
			this.m_UpdateReceivers.OnElementAdded += delegate(IUpdateReceiver x)
			{
				this.RegisterForCallbacks();
			};
			ProfilerRuntime.Initialise();
		}

		// Token: 0x06000055 RID: 85 RVA: 0x00002EBC File Offset: 0x000010BC
		private void OnObjectAdded(object obj)
		{
			IUpdateReceiver updateReceiver = obj as IUpdateReceiver;
			if (updateReceiver != null)
			{
				this.AddUpdateReceiver(updateReceiver);
			}
		}

		// Token: 0x06000056 RID: 86 RVA: 0x00002EDC File Offset: 0x000010DC
		private void OnObjectRemoved(object obj)
		{
			IUpdateReceiver updateReceiver = obj as IUpdateReceiver;
			if (updateReceiver != null)
			{
				this.RemoveUpdateReciever(updateReceiver);
			}
		}

		// Token: 0x06000057 RID: 87 RVA: 0x00002EFA File Offset: 0x000010FA
		internal void RegisterForCallbacks()
		{
			if (this.CallbackHooksEnabled && !this.m_RegisteredForCallbacks)
			{
				this.m_RegisteredForCallbacks = true;
				ComponentSingleton<MonoBehaviourCallbackHooks>.Instance.OnUpdateDelegate += this.Update;
			}
		}

		// Token: 0x06000058 RID: 88 RVA: 0x00002F29 File Offset: 0x00001129
		[Obsolete("ClearDiagnosticsCallback is Obsolete, use ClearDiagnosticCallbacks instead.")]
		public void ClearDiagnosticsCallback()
		{
			this.m_diagnosticsHandler = null;
			this.m_obsoleteDiagnosticsHandler = null;
		}

		// Token: 0x06000059 RID: 89 RVA: 0x00002F39 File Offset: 0x00001139
		public void ClearDiagnosticCallbacks()
		{
			this.m_diagnosticsHandler = null;
			this.m_obsoleteDiagnosticsHandler = null;
		}

		// Token: 0x0600005A RID: 90 RVA: 0x00002F49 File Offset: 0x00001149
		public void UnregisterDiagnosticCallback(Action<ResourceManager.DiagnosticEventContext> func)
		{
			if (this.m_diagnosticsHandler != null)
			{
				this.m_diagnosticsHandler = (Action<ResourceManager.DiagnosticEventContext>)Delegate.Remove(this.m_diagnosticsHandler, func);
				return;
			}
			Debug.LogError("No Diagnostic callbacks registered, cannot remove callback.");
		}

		// Token: 0x0600005B RID: 91 RVA: 0x00002F75 File Offset: 0x00001175
		[Obsolete]
		public void RegisterDiagnosticCallback(Action<AsyncOperationHandle, ResourceManager.DiagnosticEventType, int, object> func)
		{
			this.m_obsoleteDiagnosticsHandler = func;
		}

		// Token: 0x0600005C RID: 92 RVA: 0x00002F7E File Offset: 0x0000117E
		public void RegisterDiagnosticCallback(Action<ResourceManager.DiagnosticEventContext> func)
		{
			this.m_diagnosticsHandler = (Action<ResourceManager.DiagnosticEventContext>)Delegate.Combine(this.m_diagnosticsHandler, func);
		}

		// Token: 0x0600005D RID: 93 RVA: 0x00002F98 File Offset: 0x00001198
		internal void PostDiagnosticEvent(ResourceManager.DiagnosticEventContext context)
		{
			Action<ResourceManager.DiagnosticEventContext> diagnosticsHandler = this.m_diagnosticsHandler;
			if (diagnosticsHandler != null)
			{
				diagnosticsHandler(context);
			}
			if (this.m_obsoleteDiagnosticsHandler == null)
			{
				return;
			}
			this.m_obsoleteDiagnosticsHandler(context.OperationHandle, context.Type, context.EventValue, string.IsNullOrEmpty(context.Error) ? context.Context : context.Error);
		}

		// Token: 0x0600005E RID: 94 RVA: 0x00003000 File Offset: 0x00001200
		public IResourceProvider GetResourceProvider(Type t, IResourceLocation location)
		{
			if (location != null)
			{
				IResourceProvider resourceProvider = null;
				int num = location.ProviderId.GetHashCode() * 31 + ((t == null) ? 0 : t.GetHashCode());
				if (!this.m_providerMap.TryGetValue(num, out resourceProvider))
				{
					for (int i = 0; i < this.ResourceProviders.Count; i++)
					{
						IResourceProvider resourceProvider2 = this.ResourceProviders[i];
						if (resourceProvider2.ProviderId.Equals(location.ProviderId, StringComparison.Ordinal) && (t == null || resourceProvider2.CanProvide(t, location)))
						{
							this.m_providerMap.Add(num, resourceProvider = resourceProvider2);
							break;
						}
					}
				}
				return resourceProvider;
			}
			return null;
		}

		// Token: 0x0600005F RID: 95 RVA: 0x000030A8 File Offset: 0x000012A8
		private Type GetDefaultTypeForLocation(IResourceLocation loc)
		{
			IResourceProvider resourceProvider = this.GetResourceProvider(null, loc);
			if (resourceProvider == null)
			{
				return typeof(object);
			}
			Type defaultType = resourceProvider.GetDefaultType(loc);
			if (!(defaultType != null))
			{
				return typeof(object);
			}
			return defaultType;
		}

		// Token: 0x06000060 RID: 96 RVA: 0x000030EC File Offset: 0x000012EC
		private int CalculateLocationsHash(IList<IResourceLocation> locations, Type t = null)
		{
			if (locations == null || locations.Count == 0)
			{
				return 0;
			}
			int num = 17;
			foreach (IResourceLocation resourceLocation in locations)
			{
				Type type = ((t != null) ? t : this.GetDefaultTypeForLocation(resourceLocation));
				num = num * 31 + resourceLocation.Hash(type);
			}
			return num;
		}

		// Token: 0x06000061 RID: 97 RVA: 0x00003160 File Offset: 0x00001360
		private AsyncOperationHandle ProvideResource(IResourceLocation location, Type desiredType = null, bool releaseDependenciesOnFailure = true)
		{
			if (location == null)
			{
				throw new ArgumentNullException("location");
			}
			IResourceProvider resourceProvider = null;
			if (desiredType == null)
			{
				resourceProvider = this.GetResourceProvider(desiredType, location);
				if (resourceProvider == null)
				{
					UnknownResourceProviderException ex = new UnknownResourceProviderException(location);
					return this.CreateCompletedOperationInternal<object>(null, false, ex, releaseDependenciesOnFailure);
				}
				desiredType = resourceProvider.GetDefaultType(location);
			}
			if (resourceProvider == null)
			{
				resourceProvider = this.GetResourceProvider(desiredType, location);
			}
			IOperationCacheKey operationCacheKey = this.CreateCacheKeyForLocation(resourceProvider, location, desiredType);
			IAsyncOperation asyncOperation;
			if (this.m_AssetOperationCache.TryGetValue(operationCacheKey, out asyncOperation))
			{
				asyncOperation.IncrementReferenceCount();
				return new AsyncOperationHandle(asyncOperation, location.ToString());
			}
			Type type;
			if (!this.m_ProviderOperationTypeCache.TryGetValue(desiredType, out type))
			{
				this.m_ProviderOperationTypeCache.Add(desiredType, type = typeof(ProviderOperation<>).MakeGenericType(new Type[] { desiredType }));
			}
			asyncOperation = this.CreateOperation<IAsyncOperation>(type, type.GetHashCode(), operationCacheKey, this.m_ReleaseOpCached);
			int dependencyHashCode = location.DependencyHashCode;
			AsyncOperationHandle<IList<AsyncOperationHandle>> asyncOperationHandle = (location.HasDependencies ? this.ProvideResourceGroupCached(location.Dependencies, dependencyHashCode, null, null, releaseDependenciesOnFailure) : default(AsyncOperationHandle<IList<AsyncOperationHandle>>));
			((IGenericProviderOperation)asyncOperation).Init(this, resourceProvider, location, asyncOperationHandle, releaseDependenciesOnFailure);
			AsyncOperationHandle asyncOperationHandle2 = this.StartOperation(asyncOperation, asyncOperationHandle);
			asyncOperationHandle2.LocationName = location.ToString();
			if (asyncOperationHandle.IsValid())
			{
				asyncOperationHandle.Release();
			}
			return asyncOperationHandle2;
		}

		// Token: 0x06000062 RID: 98 RVA: 0x000032A8 File Offset: 0x000014A8
		internal IAsyncOperation GetOperationFromCache(IResourceLocation location, Type desiredType)
		{
			IResourceProvider resourceProvider = this.GetResourceProvider(desiredType, location);
			IOperationCacheKey operationCacheKey = this.CreateCacheKeyForLocation(resourceProvider, location, desiredType);
			IAsyncOperation asyncOperation;
			if (this.m_AssetOperationCache.TryGetValue(operationCacheKey, out asyncOperation))
			{
				return asyncOperation;
			}
			return null;
		}

		// Token: 0x06000063 RID: 99 RVA: 0x000032DC File Offset: 0x000014DC
		internal IOperationCacheKey CreateCacheKeyForLocation(IResourceProvider provider, IResourceLocation location, Type desiredType = null)
		{
			AssetBundleProvider assetBundleProvider = provider as AssetBundleProvider;
			if (assetBundleProvider != null)
			{
				return assetBundleProvider.CreateCacheKeyForLocation(this, location, desiredType);
			}
			return new LocationCacheKey(location, desiredType);
		}

		// Token: 0x06000064 RID: 100 RVA: 0x00003304 File Offset: 0x00001504
		public AsyncOperationHandle<TObject> ProvideResource<TObject>(IResourceLocation location)
		{
			return this.ProvideResource(location, typeof(TObject), true).Convert<TObject>();
		}

		// Token: 0x06000065 RID: 101 RVA: 0x0000332B File Offset: 0x0000152B
		public AsyncOperationHandle<TObject> StartOperation<TObject>(AsyncOperationBase<TObject> operation, AsyncOperationHandle dependency)
		{
			operation.Start(this, dependency, this.m_UpdateCallbacks);
			return operation.Handle;
		}

		// Token: 0x06000066 RID: 102 RVA: 0x00003341 File Offset: 0x00001541
		internal AsyncOperationHandle StartOperation(IAsyncOperation operation, AsyncOperationHandle dependency)
		{
			operation.Start(this, dependency, this.m_UpdateCallbacks);
			return operation.Handle;
		}

		// Token: 0x06000067 RID: 103 RVA: 0x00003357 File Offset: 0x00001557
		private void OnInstanceOperationDestroy(IAsyncOperation o)
		{
			this.m_TrackedInstanceOperations.Remove(o as ResourceManager.InstanceOperation);
			this.Allocator.Release(o.GetType().GetHashCode(), o);
		}

		// Token: 0x06000068 RID: 104 RVA: 0x00003382 File Offset: 0x00001582
		private void OnOperationDestroyNonCached(IAsyncOperation o)
		{
			this.Allocator.Release(o.GetType().GetHashCode(), o);
		}

		// Token: 0x06000069 RID: 105 RVA: 0x0000339C File Offset: 0x0000159C
		private void OnOperationDestroyCached(IAsyncOperation o)
		{
			this.Allocator.Release(o.GetType().GetHashCode(), o);
			ICachable cachable = o as ICachable;
			if (((cachable != null) ? cachable.Key : null) != null)
			{
				this.RemoveOperationFromCache(cachable.Key);
				cachable.Key = null;
			}
		}

		// Token: 0x0600006A RID: 106 RVA: 0x000033EC File Offset: 0x000015EC
		internal T CreateOperation<T>(Type actualType, int typeHash, IOperationCacheKey cacheKey, Action<IAsyncOperation> onDestroyAction) where T : IAsyncOperation
		{
			if (cacheKey == null)
			{
				T t = (T)((object)this.Allocator.New(actualType, typeHash));
				t.OnDestroy = onDestroyAction;
				return t;
			}
			T t2 = (T)((object)this.Allocator.New(actualType, typeHash));
			t2.OnDestroy = onDestroyAction;
			ICachable cachable = t2 as ICachable;
			if (cachable != null)
			{
				cachable.Key = cacheKey;
				this.AddOperationToCache(cacheKey, t2);
			}
			return t2;
		}

		// Token: 0x0600006B RID: 107 RVA: 0x00003466 File Offset: 0x00001666
		internal void AddOperationToCache(IOperationCacheKey key, IAsyncOperation operation)
		{
			if (!this.IsOperationCached(key))
			{
				this.m_AssetOperationCache.Add(key, operation);
			}
		}

		// Token: 0x0600006C RID: 108 RVA: 0x0000347E File Offset: 0x0000167E
		internal bool RemoveOperationFromCache(IOperationCacheKey key)
		{
			return !this.IsOperationCached(key) || this.m_AssetOperationCache.Remove(key);
		}

		// Token: 0x0600006D RID: 109 RVA: 0x00003497 File Offset: 0x00001697
		internal bool IsOperationCached(IOperationCacheKey key)
		{
			return this.m_AssetOperationCache.ContainsKey(key);
		}

		// Token: 0x0600006E RID: 110 RVA: 0x000034A5 File Offset: 0x000016A5
		internal int CachedOperationCount()
		{
			return this.m_AssetOperationCache.Count;
		}

		// Token: 0x0600006F RID: 111 RVA: 0x000034B2 File Offset: 0x000016B2
		internal void ClearOperationCache()
		{
			this.m_AssetOperationCache.Clear();
		}

		// Token: 0x06000070 RID: 112 RVA: 0x000034C0 File Offset: 0x000016C0
		public AsyncOperationHandle<TObject> CreateCompletedOperation<TObject>(TObject result, string errorMsg)
		{
			bool flag = string.IsNullOrEmpty(errorMsg);
			return this.CreateCompletedOperationInternal<TObject>(result, flag, (!flag) ? new Exception(errorMsg) : null, true);
		}

		// Token: 0x06000071 RID: 113 RVA: 0x000034E9 File Offset: 0x000016E9
		public AsyncOperationHandle<TObject> CreateCompletedOperationWithException<TObject>(TObject result, Exception exception)
		{
			return this.CreateCompletedOperationInternal<TObject>(result, exception == null, exception, true);
		}

		// Token: 0x06000072 RID: 114 RVA: 0x000034F8 File Offset: 0x000016F8
		internal AsyncOperationHandle<TObject> CreateCompletedOperationInternal<TObject>(TObject result, bool success, Exception exception, bool releaseDependenciesOnFailure = true)
		{
			ResourceManager.CompletedOperation<TObject> completedOperation = this.CreateOperation<ResourceManager.CompletedOperation<TObject>>(typeof(ResourceManager.CompletedOperation<TObject>), typeof(ResourceManager.CompletedOperation<TObject>).GetHashCode(), null, this.m_ReleaseOpNonCached);
			completedOperation.Init(result, success, exception, releaseDependenciesOnFailure);
			return this.StartOperation<TObject>(completedOperation, default(AsyncOperationHandle));
		}

		// Token: 0x06000073 RID: 115 RVA: 0x00003547 File Offset: 0x00001747
		public void Release(AsyncOperationHandle handle)
		{
			handle.Release();
		}

		// Token: 0x06000074 RID: 116 RVA: 0x00003550 File Offset: 0x00001750
		public AsyncOperationHandle<TObject> Acquire<TObject>(AsyncOperationHandle<TObject> handle)
		{
			return handle.Acquire();
		}

		// Token: 0x06000075 RID: 117 RVA: 0x00003559 File Offset: 0x00001759
		public void Acquire(AsyncOperationHandle handle)
		{
			handle.Acquire();
		}

		// Token: 0x06000076 RID: 118 RVA: 0x00003564 File Offset: 0x00001764
		private GroupOperation AcquireGroupOpFromCache(IOperationCacheKey key)
		{
			IAsyncOperation asyncOperation;
			if (this.m_AssetOperationCache.TryGetValue(key, out asyncOperation))
			{
				asyncOperation.IncrementReferenceCount();
				return (GroupOperation)asyncOperation;
			}
			return null;
		}

		// Token: 0x06000077 RID: 119 RVA: 0x00003590 File Offset: 0x00001790
		public AsyncOperationHandle<IList<AsyncOperationHandle>> CreateGroupOperation<T>(IList<IResourceLocation> locations)
		{
			GroupOperation groupOperation = this.CreateOperation<GroupOperation>(typeof(GroupOperation), ResourceManager.s_GroupOperationTypeHash, null, this.m_ReleaseOpNonCached);
			List<AsyncOperationHandle> list = new List<AsyncOperationHandle>(locations.Count);
			foreach (IResourceLocation resourceLocation in locations)
			{
				list.Add(this.ProvideResource<T>(resourceLocation));
			}
			groupOperation.Init(list, true, false);
			return this.StartOperation<IList<AsyncOperationHandle>>(groupOperation, default(AsyncOperationHandle));
		}

		// Token: 0x06000078 RID: 120 RVA: 0x00003628 File Offset: 0x00001828
		internal AsyncOperationHandle<IList<AsyncOperationHandle>> CreateGroupOperation<T>(IList<IResourceLocation> locations, bool allowFailedDependencies)
		{
			GroupOperation groupOperation = this.CreateOperation<GroupOperation>(typeof(GroupOperation), ResourceManager.s_GroupOperationTypeHash, null, this.m_ReleaseOpNonCached);
			List<AsyncOperationHandle> list = new List<AsyncOperationHandle>(locations.Count);
			foreach (IResourceLocation resourceLocation in locations)
			{
				list.Add(this.ProvideResource<T>(resourceLocation));
			}
			GroupOperation.GroupOperationSettings groupOperationSettings = GroupOperation.GroupOperationSettings.None;
			if (allowFailedDependencies)
			{
				groupOperationSettings |= GroupOperation.GroupOperationSettings.AllowFailedDependencies;
			}
			groupOperation.Init(list, groupOperationSettings);
			return this.StartOperation<IList<AsyncOperationHandle>>(groupOperation, default(AsyncOperationHandle));
		}

		// Token: 0x06000079 RID: 121 RVA: 0x000036C8 File Offset: 0x000018C8
		public AsyncOperationHandle<IList<AsyncOperationHandle>> CreateGenericGroupOperation(List<AsyncOperationHandle> operations, bool releasedCachedOpOnComplete = false)
		{
			GroupOperation groupOperation = this.CreateOperation<GroupOperation>(typeof(GroupOperation), ResourceManager.s_GroupOperationTypeHash, new AsyncOpHandlesCacheKey(operations), releasedCachedOpOnComplete ? this.m_ReleaseOpCached : this.m_ReleaseOpNonCached);
			groupOperation.Init(operations, true, false);
			return this.StartOperation<IList<AsyncOperationHandle>>(groupOperation, default(AsyncOperationHandle));
		}

		// Token: 0x0600007A RID: 122 RVA: 0x0000371C File Offset: 0x0000191C
		internal AsyncOperationHandle<IList<AsyncOperationHandle>> ProvideResourceGroupCached(IList<IResourceLocation> locations, int groupHash, Type desiredType, Action<AsyncOperationHandle> callback, bool releaseDependenciesOnFailure = true)
		{
			DependenciesCacheKey dependenciesCacheKey = new DependenciesCacheKey(locations, groupHash);
			GroupOperation groupOperation = this.AcquireGroupOpFromCache(dependenciesCacheKey);
			AsyncOperationHandle<IList<AsyncOperationHandle>> asyncOperationHandle;
			if (groupOperation == null)
			{
				groupOperation = this.CreateOperation<GroupOperation>(typeof(GroupOperation), ResourceManager.s_GroupOperationTypeHash, dependenciesCacheKey, this.m_ReleaseOpCached);
				List<AsyncOperationHandle> list = new List<AsyncOperationHandle>(locations.Count);
				foreach (IResourceLocation resourceLocation in locations)
				{
					list.Add(this.ProvideResource(resourceLocation, desiredType, releaseDependenciesOnFailure));
				}
				groupOperation.Init(list, releaseDependenciesOnFailure, false);
				asyncOperationHandle = this.StartOperation<IList<AsyncOperationHandle>>(groupOperation, default(AsyncOperationHandle));
			}
			else
			{
				asyncOperationHandle = groupOperation.Handle;
			}
			if (callback != null)
			{
				IList<AsyncOperationHandle> dependentOps = groupOperation.GetDependentOps();
				for (int i = 0; i < dependentOps.Count; i++)
				{
					dependentOps[i].Completed += callback;
				}
			}
			return asyncOperationHandle;
		}

		// Token: 0x0600007B RID: 123 RVA: 0x00003810 File Offset: 0x00001A10
		public AsyncOperationHandle<IList<TObject>> ProvideResources<TObject>(IList<IResourceLocation> locations, Action<TObject> callback = null)
		{
			return this.ProvideResources<TObject>(locations, true, callback);
		}

		// Token: 0x0600007C RID: 124 RVA: 0x0000381C File Offset: 0x00001A1C
		public AsyncOperationHandle<IList<TObject>> ProvideResources<TObject>(IList<IResourceLocation> locations, bool releaseDependenciesOnFailure, Action<TObject> callback = null)
		{
			if (locations == null)
			{
				return this.CreateCompletedOperation<IList<TObject>>(null, "Null Location");
			}
			Action<AsyncOperationHandle> action = null;
			if (callback != null)
			{
				action = delegate(AsyncOperationHandle x)
				{
					callback((TObject)((object)x.Result));
				};
			}
			AsyncOperationHandle<IList<AsyncOperationHandle>> asyncOperationHandle = this.ProvideResourceGroupCached(locations, this.CalculateLocationsHash(locations, typeof(TObject)), typeof(TObject), action, releaseDependenciesOnFailure);
			AsyncOperationHandle<IList<TObject>> asyncOperationHandle2 = this.CreateChainOperation<IList<TObject>>(asyncOperationHandle, delegate(AsyncOperationHandle resultHandle)
			{
				AsyncOperationHandle<IList<AsyncOperationHandle>> asyncOperationHandle3 = resultHandle.Convert<IList<AsyncOperationHandle>>();
				List<TObject> list = new List<TObject>();
				Exception ex = null;
				if (asyncOperationHandle3.Status == AsyncOperationStatus.Succeeded)
				{
					using (IEnumerator<AsyncOperationHandle> enumerator = asyncOperationHandle3.Result.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							AsyncOperationHandle asyncOperationHandle4 = enumerator.Current;
							list.Add(asyncOperationHandle4.Convert<TObject>().Result);
						}
						goto IL_00F5;
					}
				}
				bool flag = false;
				if (!releaseDependenciesOnFailure)
				{
					foreach (AsyncOperationHandle asyncOperationHandle5 in asyncOperationHandle3.Result)
					{
						if (asyncOperationHandle5.Status == AsyncOperationStatus.Succeeded)
						{
							list.Add(asyncOperationHandle5.Convert<TObject>().Result);
							flag = true;
						}
						else
						{
							list.Add(default(TObject));
						}
					}
				}
				if (!flag)
				{
					list = null;
					ex = new ResourceManagerException("ProvideResources failed", asyncOperationHandle3.OperationException);
				}
				else
				{
					ex = new ResourceManagerException("Partial success in ProvideResources.  Some items failed to load. See earlier logs for more info.", asyncOperationHandle3.OperationException);
				}
				IL_00F5:
				return this.CreateCompletedOperationInternal<IList<TObject>>(list, ex == null, ex, releaseDependenciesOnFailure);
			}, releaseDependenciesOnFailure);
			asyncOperationHandle.Release();
			return asyncOperationHandle2;
		}

		// Token: 0x0600007D RID: 125 RVA: 0x000038BC File Offset: 0x00001ABC
		public AsyncOperationHandle<TObject> CreateChainOperation<TObject, TObjectDependency>(AsyncOperationHandle<TObjectDependency> dependentOp, Func<AsyncOperationHandle<TObjectDependency>, AsyncOperationHandle<TObject>> callback)
		{
			ChainOperation<TObject, TObjectDependency> chainOperation = this.CreateOperation<ChainOperation<TObject, TObjectDependency>>(typeof(ChainOperation<TObject, TObjectDependency>), typeof(ChainOperation<TObject, TObjectDependency>).GetHashCode(), null, null);
			chainOperation.Init(dependentOp, callback, true);
			return this.StartOperation<TObject>(chainOperation, dependentOp);
		}

		// Token: 0x0600007E RID: 126 RVA: 0x00003904 File Offset: 0x00001B04
		public AsyncOperationHandle<TObject> CreateChainOperation<TObject>(AsyncOperationHandle dependentOp, Func<AsyncOperationHandle, AsyncOperationHandle<TObject>> callback)
		{
			ChainOperationTypelessDepedency<TObject> chainOperationTypelessDepedency = new ChainOperationTypelessDepedency<TObject>();
			chainOperationTypelessDepedency.Init(dependentOp, callback, true);
			return this.StartOperation<TObject>(chainOperationTypelessDepedency, dependentOp);
		}

		// Token: 0x0600007F RID: 127 RVA: 0x00003928 File Offset: 0x00001B28
		public AsyncOperationHandle<TObject> CreateChainOperation<TObject, TObjectDependency>(AsyncOperationHandle<TObjectDependency> dependentOp, Func<AsyncOperationHandle<TObjectDependency>, AsyncOperationHandle<TObject>> callback, bool releaseDependenciesOnFailure = true)
		{
			ChainOperation<TObject, TObjectDependency> chainOperation = this.CreateOperation<ChainOperation<TObject, TObjectDependency>>(typeof(ChainOperation<TObject, TObjectDependency>), typeof(ChainOperation<TObject, TObjectDependency>).GetHashCode(), null, null);
			chainOperation.Init(dependentOp, callback, releaseDependenciesOnFailure);
			return this.StartOperation<TObject>(chainOperation, dependentOp);
		}

		// Token: 0x06000080 RID: 128 RVA: 0x00003970 File Offset: 0x00001B70
		public AsyncOperationHandle<TObject> CreateChainOperation<TObject>(AsyncOperationHandle dependentOp, Func<AsyncOperationHandle, AsyncOperationHandle<TObject>> callback, bool releaseDependenciesOnFailure = true)
		{
			ChainOperationTypelessDepedency<TObject> chainOperationTypelessDepedency = new ChainOperationTypelessDepedency<TObject>();
			chainOperationTypelessDepedency.Init(dependentOp, callback, releaseDependenciesOnFailure);
			return this.StartOperation<TObject>(chainOperationTypelessDepedency, dependentOp);
		}

		// Token: 0x06000081 RID: 129 RVA: 0x00003994 File Offset: 0x00001B94
		public AsyncOperationHandle<SceneInstance> ProvideScene(ISceneProvider sceneProvider, IResourceLocation location, LoadSceneMode loadSceneMode, bool activateOnLoad, int priority)
		{
			if (sceneProvider == null)
			{
				throw new NullReferenceException("sceneProvider is null");
			}
			return sceneProvider.ProvideScene(this, location, new LoadSceneParameters(loadSceneMode), activateOnLoad, priority);
		}

		// Token: 0x06000082 RID: 130 RVA: 0x000039B6 File Offset: 0x00001BB6
		public AsyncOperationHandle<SceneInstance> ProvideScene(ISceneProvider sceneProvider, IResourceLocation location, LoadSceneParameters loadSceneParameters, bool activateOnLoad, int priority)
		{
			if (sceneProvider == null)
			{
				throw new NullReferenceException("sceneProvider is null");
			}
			return sceneProvider.ProvideScene(this, location, loadSceneParameters, activateOnLoad, priority);
		}

		// Token: 0x06000083 RID: 131 RVA: 0x000039D3 File Offset: 0x00001BD3
		public AsyncOperationHandle<SceneInstance> ReleaseScene(ISceneProvider sceneProvider, AsyncOperationHandle<SceneInstance> sceneLoadHandle)
		{
			if (sceneProvider == null)
			{
				throw new NullReferenceException("sceneProvider is null");
			}
			return sceneProvider.ReleaseScene(this, sceneLoadHandle);
		}

		// Token: 0x06000084 RID: 132 RVA: 0x000039EC File Offset: 0x00001BEC
		public AsyncOperationHandle<GameObject> ProvideInstance(IInstanceProvider provider, IResourceLocation location, InstantiationParameters instantiateParameters)
		{
			if (provider == null)
			{
				throw new NullReferenceException("provider is null.  Assign a valid IInstanceProvider object before using.");
			}
			if (location == null)
			{
				throw new ArgumentNullException("location");
			}
			AsyncOperationHandle<GameObject> asyncOperationHandle = this.ProvideResource<GameObject>(location);
			ResourceManager.InstanceOperation instanceOperation = this.CreateOperation<ResourceManager.InstanceOperation>(typeof(ResourceManager.InstanceOperation), ResourceManager.s_InstanceOperationTypeHash, null, this.m_ReleaseInstanceOp);
			instanceOperation.Init(this, provider, instantiateParameters, asyncOperationHandle);
			this.m_TrackedInstanceOperations.Add(instanceOperation);
			return this.StartOperation<GameObject>(instanceOperation, asyncOperationHandle);
		}

		// Token: 0x06000085 RID: 133 RVA: 0x00003A60 File Offset: 0x00001C60
		public void CleanupSceneInstances(Scene scene)
		{
			List<ResourceManager.InstanceOperation> list = null;
			foreach (ResourceManager.InstanceOperation instanceOperation in this.m_TrackedInstanceOperations)
			{
				if (instanceOperation.Result == null && scene == instanceOperation.InstanceScene())
				{
					if (list == null)
					{
						list = new List<ResourceManager.InstanceOperation>();
					}
					list.Add(instanceOperation);
				}
			}
			if (list != null)
			{
				foreach (ResourceManager.InstanceOperation instanceOperation2 in list)
				{
					this.m_TrackedInstanceOperations.Remove(instanceOperation2);
					instanceOperation2.DecrementReferenceCount();
				}
			}
		}

		// Token: 0x06000086 RID: 134 RVA: 0x00003B2C File Offset: 0x00001D2C
		private void ExecuteDeferredCallbacks()
		{
			this.m_InsideExecuteDeferredCallbacksMethod = true;
			for (int i = 0; i < this.m_DeferredCompleteCallbacks.Count; i++)
			{
				this.m_DeferredCompleteCallbacks[i].InvokeCompletionEvent();
				this.m_DeferredCompleteCallbacks[i].DecrementReferenceCount();
			}
			this.m_DeferredCompleteCallbacks.Clear();
			this.m_InsideExecuteDeferredCallbacksMethod = false;
		}

		// Token: 0x06000087 RID: 135 RVA: 0x00003B8C File Offset: 0x00001D8C
		internal void RegisterForDeferredCallback(IAsyncOperation op, bool incrementRefCount = true)
		{
			if (this.CallbackHooksEnabled && this.m_InsideExecuteDeferredCallbacksMethod)
			{
				if (this.m_DeferredCallbacksToRegister == null)
				{
					this.m_DeferredCallbacksToRegister = new List<ResourceManager.DeferredCallbackRegisterRequest>();
				}
				this.m_DeferredCallbacksToRegister.Add(new ResourceManager.DeferredCallbackRegisterRequest
				{
					operation = op,
					incrementRefCount = incrementRefCount
				});
				return;
			}
			if (incrementRefCount)
			{
				op.IncrementReferenceCount();
			}
			this.m_DeferredCompleteCallbacks.Add(op);
			this.RegisterForCallbacks();
		}

		// Token: 0x06000088 RID: 136 RVA: 0x00003BFC File Offset: 0x00001DFC
		internal void Update(float unscaledDeltaTime)
		{
			if (this.m_InsideUpdateMethod)
			{
				throw new Exception("Reentering the Update method is not allowed.  This can happen when calling WaitForCompletion on an operation while inside of a callback.");
			}
			this.m_InsideUpdateMethod = true;
			this.m_UpdateCallbacks.Invoke(unscaledDeltaTime);
			this.m_UpdatingReceivers = true;
			for (int i = 0; i < this.m_UpdateReceivers.Count; i++)
			{
				this.m_UpdateReceivers[i].Update(unscaledDeltaTime);
			}
			this.m_UpdatingReceivers = false;
			if (this.m_UpdateReceiversToRemove != null)
			{
				foreach (IUpdateReceiver updateReceiver in this.m_UpdateReceiversToRemove)
				{
					this.m_UpdateReceivers.Remove(updateReceiver);
				}
				this.m_UpdateReceiversToRemove = null;
			}
			if (this.m_DeferredCallbacksToRegister != null)
			{
				foreach (ResourceManager.DeferredCallbackRegisterRequest deferredCallbackRegisterRequest in this.m_DeferredCallbacksToRegister)
				{
					this.RegisterForDeferredCallback(deferredCallbackRegisterRequest.operation, deferredCallbackRegisterRequest.incrementRefCount);
				}
				this.m_DeferredCallbacksToRegister = null;
			}
			this.ExecuteDeferredCallbacks();
			this.m_InsideUpdateMethod = false;
		}

		// Token: 0x06000089 RID: 137 RVA: 0x00003D2C File Offset: 0x00001F2C
		public void Dispose()
		{
			if (ComponentSingleton<MonoBehaviourCallbackHooks>.Exists && this.m_RegisteredForCallbacks)
			{
				ComponentSingleton<MonoBehaviourCallbackHooks>.Instance.OnUpdateDelegate -= this.Update;
				this.m_RegisteredForCallbacks = false;
			}
		}

		// Token: 0x04000018 RID: 24
		internal bool postProfilerEvents;

		// Token: 0x0400001C RID: 28
		internal bool CallbackHooksEnabled = true;

		// Token: 0x0400001D RID: 29
		private ListWithEvents<IResourceProvider> m_ResourceProviders = new ListWithEvents<IResourceProvider>();

		// Token: 0x0400001E RID: 30
		private IAllocationStrategy m_allocator;

		// Token: 0x0400001F RID: 31
		internal ListWithEvents<IUpdateReceiver> m_UpdateReceivers = new ListWithEvents<IUpdateReceiver>();

		// Token: 0x04000020 RID: 32
		private List<IUpdateReceiver> m_UpdateReceiversToRemove;

		// Token: 0x04000021 RID: 33
		private bool m_UpdatingReceivers;

		// Token: 0x04000022 RID: 34
		private bool m_InsideUpdateMethod;

		// Token: 0x04000023 RID: 35
		internal Dictionary<int, IResourceProvider> m_providerMap = new Dictionary<int, IResourceProvider>();

		// Token: 0x04000024 RID: 36
		private Dictionary<IOperationCacheKey, IAsyncOperation> m_AssetOperationCache = new Dictionary<IOperationCacheKey, IAsyncOperation>();

		// Token: 0x04000025 RID: 37
		private HashSet<ResourceManager.InstanceOperation> m_TrackedInstanceOperations = new HashSet<ResourceManager.InstanceOperation>();

		// Token: 0x04000026 RID: 38
		internal DelegateList<float> m_UpdateCallbacks = DelegateList<float>.CreateWithGlobalCache();

		// Token: 0x04000027 RID: 39
		private List<IAsyncOperation> m_DeferredCompleteCallbacks = new List<IAsyncOperation>();

		// Token: 0x04000028 RID: 40
		private bool m_InsideExecuteDeferredCallbacksMethod;

		// Token: 0x04000029 RID: 41
		private List<ResourceManager.DeferredCallbackRegisterRequest> m_DeferredCallbacksToRegister;

		// Token: 0x0400002A RID: 42
		private Action<AsyncOperationHandle, ResourceManager.DiagnosticEventType, int, object> m_obsoleteDiagnosticsHandler;

		// Token: 0x0400002B RID: 43
		private Action<ResourceManager.DiagnosticEventContext> m_diagnosticsHandler;

		// Token: 0x0400002C RID: 44
		private Action<IAsyncOperation> m_ReleaseOpNonCached;

		// Token: 0x0400002D RID: 45
		private Action<IAsyncOperation> m_ReleaseOpCached;

		// Token: 0x0400002E RID: 46
		private Action<IAsyncOperation> m_ReleaseInstanceOp;

		// Token: 0x0400002F RID: 47
		private static int s_GroupOperationTypeHash = typeof(GroupOperation).GetHashCode();

		// Token: 0x04000030 RID: 48
		private static int s_InstanceOperationTypeHash = typeof(ResourceManager.InstanceOperation).GetHashCode();

		// Token: 0x04000032 RID: 50
		private bool m_RegisteredForCallbacks;

		// Token: 0x04000033 RID: 51
		private Dictionary<Type, Type> m_ProviderOperationTypeCache = new Dictionary<Type, Type>();

		// Token: 0x02000063 RID: 99
		public enum DiagnosticEventType
		{
			// Token: 0x04000129 RID: 297
			AsyncOperationFail,
			// Token: 0x0400012A RID: 298
			AsyncOperationCreate,
			// Token: 0x0400012B RID: 299
			AsyncOperationPercentComplete,
			// Token: 0x0400012C RID: 300
			AsyncOperationComplete,
			// Token: 0x0400012D RID: 301
			AsyncOperationReferenceCount,
			// Token: 0x0400012E RID: 302
			AsyncOperationDestroy
		}

		// Token: 0x02000064 RID: 100
		public struct DiagnosticEventContext
		{
			// Token: 0x170000C8 RID: 200
			// (get) Token: 0x060002F8 RID: 760 RVA: 0x00009F81 File Offset: 0x00008181
			public readonly AsyncOperationHandle OperationHandle { get; }

			// Token: 0x170000C9 RID: 201
			// (get) Token: 0x060002F9 RID: 761 RVA: 0x00009F89 File Offset: 0x00008189
			public readonly ResourceManager.DiagnosticEventType Type { get; }

			// Token: 0x170000CA RID: 202
			// (get) Token: 0x060002FA RID: 762 RVA: 0x00009F91 File Offset: 0x00008191
			public readonly int EventValue { get; }

			// Token: 0x170000CB RID: 203
			// (get) Token: 0x060002FB RID: 763 RVA: 0x00009F99 File Offset: 0x00008199
			public readonly IResourceLocation Location { get; }

			// Token: 0x170000CC RID: 204
			// (get) Token: 0x060002FC RID: 764 RVA: 0x00009FA1 File Offset: 0x000081A1
			public readonly object Context { get; }

			// Token: 0x170000CD RID: 205
			// (get) Token: 0x060002FD RID: 765 RVA: 0x00009FA9 File Offset: 0x000081A9
			public readonly string Error { get; }

			// Token: 0x060002FE RID: 766 RVA: 0x00009FB4 File Offset: 0x000081B4
			public DiagnosticEventContext(AsyncOperationHandle op, ResourceManager.DiagnosticEventType type, int eventValue = 0, string error = null, object context = null)
			{
				this.OperationHandle = op;
				this.Type = type;
				this.EventValue = eventValue;
				IResourceLocation resourceLocation;
				if (op.m_InternalOp != null)
				{
					IGenericProviderOperation genericProviderOperation = op.m_InternalOp as IGenericProviderOperation;
					if (genericProviderOperation != null)
					{
						resourceLocation = genericProviderOperation.Location;
						goto IL_0036;
					}
				}
				resourceLocation = null;
				IL_0036:
				this.Location = resourceLocation;
				this.Error = error;
				this.Context = context;
			}
		}

		// Token: 0x02000065 RID: 101
		private struct DeferredCallbackRegisterRequest
		{
			// Token: 0x04000135 RID: 309
			internal IAsyncOperation operation;

			// Token: 0x04000136 RID: 310
			internal bool incrementRefCount;
		}

		// Token: 0x02000066 RID: 102
		private class CompletedOperation<TObject> : AsyncOperationBase<TObject>
		{
			// Token: 0x06000300 RID: 768 RVA: 0x0000A014 File Offset: 0x00008214
			public void Init(TObject result, bool success, string errorMsg, bool releaseDependenciesOnFailure = true)
			{
				this.Init(result, success, (!string.IsNullOrEmpty(errorMsg)) ? new Exception(errorMsg) : null, releaseDependenciesOnFailure);
			}

			// Token: 0x06000301 RID: 769 RVA: 0x0000A031 File Offset: 0x00008231
			public void Init(TObject result, bool success, Exception exception, bool releaseDependenciesOnFailure = true)
			{
				base.Result = result;
				this.m_Success = success;
				this.m_Exception = exception;
				this.m_ReleaseDependenciesOnFailure = releaseDependenciesOnFailure;
			}

			// Token: 0x170000CE RID: 206
			// (get) Token: 0x06000302 RID: 770 RVA: 0x0000A050 File Offset: 0x00008250
			protected override string DebugName
			{
				get
				{
					return "CompletedOperation";
				}
			}

			// Token: 0x06000303 RID: 771 RVA: 0x0000A057 File Offset: 0x00008257
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

			// Token: 0x06000304 RID: 772 RVA: 0x0000A07E File Offset: 0x0000827E
			protected override void Execute()
			{
				base.Complete(base.Result, this.m_Success, this.m_Exception, this.m_ReleaseDependenciesOnFailure);
			}

			// Token: 0x04000137 RID: 311
			private bool m_Success;

			// Token: 0x04000138 RID: 312
			private Exception m_Exception;

			// Token: 0x04000139 RID: 313
			private bool m_ReleaseDependenciesOnFailure;
		}

		// Token: 0x02000067 RID: 103
		internal class InstanceOperation : AsyncOperationBase<GameObject>
		{
			// Token: 0x06000305 RID: 773 RVA: 0x0000A09E File Offset: 0x0000829E
			public void Init(ResourceManager rm, IInstanceProvider instanceProvider, InstantiationParameters instantiationParams, AsyncOperationHandle<GameObject> dependency)
			{
				this.m_RM = rm;
				this.m_dependency = dependency;
				this.m_instanceProvider = instanceProvider;
				this.m_instantiationParams = instantiationParams;
				this.m_scene = default(Scene);
			}

			// Token: 0x06000306 RID: 774 RVA: 0x0000A0CC File Offset: 0x000082CC
			internal override DownloadStatus GetDownloadStatus(HashSet<object> visited)
			{
				if (!this.m_dependency.IsValid())
				{
					return new DownloadStatus
					{
						IsDone = base.IsDone
					};
				}
				return this.m_dependency.InternalGetDownloadStatus(visited);
			}

			// Token: 0x06000307 RID: 775 RVA: 0x0000A109 File Offset: 0x00008309
			public override void GetDependencies(List<AsyncOperationHandle> deps)
			{
				deps.Add(this.m_dependency);
			}

			// Token: 0x170000CF RID: 207
			// (get) Token: 0x06000308 RID: 776 RVA: 0x0000A11C File Offset: 0x0000831C
			protected override string DebugName
			{
				get
				{
					if (this.m_instanceProvider == null)
					{
						return "Instance<Invalid>";
					}
					return string.Format("Instance<{0}>({1}", this.m_instanceProvider.GetType().Name, this.m_dependency.IsValid() ? this.m_dependency.DebugName : "Invalid");
				}
			}

			// Token: 0x06000309 RID: 777 RVA: 0x0000A170 File Offset: 0x00008370
			public Scene InstanceScene()
			{
				return this.m_scene;
			}

			// Token: 0x0600030A RID: 778 RVA: 0x0000A178 File Offset: 0x00008378
			protected override void Destroy()
			{
				this.m_instanceProvider.ReleaseInstance(this.m_RM, this.m_instance);
			}

			// Token: 0x170000D0 RID: 208
			// (get) Token: 0x0600030B RID: 779 RVA: 0x0000A191 File Offset: 0x00008391
			protected override float Progress
			{
				get
				{
					return this.m_dependency.PercentComplete;
				}
			}

			// Token: 0x0600030C RID: 780 RVA: 0x0000A1A0 File Offset: 0x000083A0
			protected override bool InvokeWaitForCompletion()
			{
				if (this.m_dependency.IsValid() && !this.m_dependency.IsDone)
				{
					this.m_dependency.WaitForCompletion();
				}
				ResourceManager rm = this.m_RM;
				if (rm != null)
				{
					rm.Update(Time.unscaledDeltaTime);
				}
				if (this.m_instance == null && !this.HasExecuted)
				{
					base.InvokeExecute();
				}
				return base.IsDone;
			}

			// Token: 0x0600030D RID: 781 RVA: 0x0000A20C File Offset: 0x0000840C
			protected override void Execute()
			{
				Exception operationException = this.m_dependency.OperationException;
				if (this.m_dependency.Status == AsyncOperationStatus.Succeeded)
				{
					this.m_instance = this.m_instanceProvider.ProvideInstance(this.m_RM, this.m_dependency, this.m_instantiationParams);
					if (this.m_instance != null)
					{
						this.m_scene = this.m_instance.scene;
					}
					base.Complete(this.m_instance, true, null);
					return;
				}
				base.Complete(this.m_instance, false, string.Format("Dependency operation failed with {0}.", operationException));
			}

			// Token: 0x0400013A RID: 314
			private AsyncOperationHandle<GameObject> m_dependency;

			// Token: 0x0400013B RID: 315
			private InstantiationParameters m_instantiationParams;

			// Token: 0x0400013C RID: 316
			private IInstanceProvider m_instanceProvider;

			// Token: 0x0400013D RID: 317
			private GameObject m_instance;

			// Token: 0x0400013E RID: 318
			private Scene m_scene;
		}
	}
}
