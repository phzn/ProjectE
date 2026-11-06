using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Unity.Burst;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Content;
using Unity.Entities.Serialization;
using Unity.IO.Archive;
using Unity.Loading;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Unity.Entities.Content
{
	// Token: 0x02000487 RID: 1159
	[GenerateTestsForBurstCompatibility]
	[BurstCompile]
	public static class RuntimeContentManager
	{
		// Token: 0x17000249 RID: 585
		// (get) Token: 0x06001C59 RID: 7257 RVA: 0x00063A84 File Offset: 0x00061C84
		[ExcludeFromBurstCompatTesting("References managed objects")]
		public static string RelativeCatalogPath
		{
			get
			{
				return "ContentArchives/archive_dependencies.bin";
			}
		}

		// Token: 0x06001C5A RID: 7258 RVA: 0x00063A8B File Offset: 0x00061C8B
		[ExcludeFromBurstCompatTesting("References managed objects")]
		public static string DefaultContentFileNameFunc(string fileId)
		{
			return "ns:/RTC/" + RuntimeContentManager.ArchivePrefix + "/" + fileId;
		}

		// Token: 0x06001C5B RID: 7259 RVA: 0x00063AA2 File Offset: 0x00061CA2
		[ExcludeFromBurstCompatTesting("References managed objects")]
		public static string DefaultArchivePathFunc(string archiveId)
		{
			return "ContentArchives/" + archiveId;
		}

		// Token: 0x1700024A RID: 586
		// (get) Token: 0x06001C5C RID: 7260 RVA: 0x00063AAF File Offset: 0x00061CAF
		[ExcludeFromBurstCompatTesting("References managed objects")]
		internal static bool IsReady
		{
			get
			{
				return RuntimeContentManager.Catalog.IsCreated && !RuntimeContentManager.Catalog.IsEmpty;
			}
		}

		// Token: 0x06001C5D RID: 7261 RVA: 0x00063ACC File Offset: 0x00061CCC
		[ExcludeFromBurstCompatTesting("References managed objects")]
		public unsafe static void Initialize()
		{
			if (RuntimeContentManager.ActiveObjects.IsCreated)
			{
				Debug.LogWarning("Initialize called before Cleanup!");
				int num;
				RuntimeContentManager.Cleanup(out num);
			}
			RuntimeContentManager.currentGeneration++;
			RuntimeContentManager.Namespace = ContentNamespace.GetOrCreateNamespace("RTC");
			RuntimeContentManager.ArchivePrefix = string.Format("a{0}:", RuntimeContentManager.currentGeneration);
			RuntimeContentManager.Catalog = default(RuntimeContentCatalog);
			RuntimeContentManager.ActiveArchives = new UnsafeHashMap<ContentArchiveId, RuntimeContentManager.ActiveArchive>(2048, Allocator.Persistent);
			RuntimeContentManager.ActiveFiles = new UnsafeHashMap<ContentFileId, RuntimeContentManager.ActiveFile>(2048, Allocator.Persistent);
			RuntimeContentManager.ActiveObjects = new UnsafeHashMap<UntypedWeakReferenceId, RuntimeContentManager.ActiveObject>(2048, Allocator.Persistent);
			RuntimeContentManager.ActiveScenes = new UnsafeHashMap<int, RuntimeContentManager.ActiveScene>(1024, Allocator.Persistent);
			*RuntimeContentManager.SharedStaticObjectValueCache.Data = new ObjectValueCache(2048);
			*RuntimeContentManager.SharedStaticObjectLoadQueue.Data = new MultiProducerSingleBulkConsumerQueue<UntypedWeakReferenceId>(2048);
			*RuntimeContentManager.SharedStaticObjectReleaseQueue.Data = new MultiProducerSingleBulkConsumerQueue<UntypedWeakReferenceId>(2048);
			*RuntimeContentManager.SharedStaticLoadingObjects.Data = new UnsafeList<UntypedWeakReferenceId>(2048, Allocator.Persistent, NativeArrayOptions.UninitializedMemory);
			*RuntimeContentManager.SharedStaticDeferredSceneUnloads.Data = new UnsafeList<RuntimeContentManager.DeferredSceneDependencyUnload>(1024, Allocator.Persistent, NativeArrayOptions.UninitializedMemory);
			if (RuntimeContentManager.s_LoadObjectDelegateGCPrevention == null)
			{
				RuntimeContentManager.LoadObjectManagedDelegate loadObjectManagedDelegate = new RuntimeContentManager.LoadObjectManagedDelegate(RuntimeContentManager.LoadObjectsImpl);
				RuntimeContentManager.s_LoadObjectDelegateGCPrevention = loadObjectManagedDelegate;
				*RuntimeContentManager.s_ManagedLoadObjectTrampoline.Data = Marshal.GetFunctionPointerForDelegate<RuntimeContentManager.LoadObjectManagedDelegate>(loadObjectManagedDelegate);
			}
			if (RuntimeContentManager.s_ReleaseObjectDelegateGCPrevention == null)
			{
				RuntimeContentManager.ReleaseObjectManagedDelegate releaseObjectManagedDelegate = new RuntimeContentManager.ReleaseObjectManagedDelegate(RuntimeContentManager.ReleaseObjectsImpl);
				RuntimeContentManager.s_ReleaseObjectDelegateGCPrevention = releaseObjectManagedDelegate;
				*RuntimeContentManager.s_ManagedReleaseObjectTrampoline.Data = Marshal.GetFunctionPointerForDelegate<RuntimeContentManager.ReleaseObjectManagedDelegate>(releaseObjectManagedDelegate);
			}
			if (RuntimeContentManager.s_ProcessObjectDelegateGCPrevention == null)
			{
				RuntimeContentManager.ProcessObjectManagedDelegate processObjectManagedDelegate = new RuntimeContentManager.ProcessObjectManagedDelegate(RuntimeContentManager.UpdateLoadingObjectStatus);
				RuntimeContentManager.s_ProcessObjectDelegateGCPrevention = processObjectManagedDelegate;
				*RuntimeContentManager.s_ManagedProcessObjectTrampoline.Data = Marshal.GetFunctionPointerForDelegate<RuntimeContentManager.ProcessObjectManagedDelegate>(processObjectManagedDelegate);
			}
			if (RuntimeContentManager.s_DeferredSceneUnloadDelegateGCPrevention == null)
			{
				RuntimeContentManager.DeferredSceneUnloadManagedDelegate deferredSceneUnloadManagedDelegate = new RuntimeContentManager.DeferredSceneUnloadManagedDelegate(RuntimeContentManager.DeferredSceneUnloadImpl);
				RuntimeContentManager.s_DeferredSceneUnloadDelegateGCPrevention = deferredSceneUnloadManagedDelegate;
				*RuntimeContentManager.s_ManagedDeferredSceneUnloadTrampoline.Data = Marshal.GetFunctionPointerForDelegate<RuntimeContentManager.DeferredSceneUnloadManagedDelegate>(deferredSceneUnloadManagedDelegate);
			}
			SceneManager.sceneUnloaded += RuntimeContentManager.ReleaseSceneResources;
			DefaultWorldInitialization.DefaultWorldDestroyed += delegate
			{
				int num2;
				RuntimeContentManager.Cleanup(out num2);
			};
		}

		// Token: 0x06001C5E RID: 7262 RVA: 0x00063CFC File Offset: 0x00061EFC
		private static void ReleaseSceneResources(Scene scene)
		{
			RuntimeContentManager.ActiveScene activeScene;
			if (RuntimeContentManager.ActiveScenes.TryGetValue(scene.handle, out activeScene))
			{
				RuntimeContentManager.ActiveScenes.Remove(scene.handle);
				ContentFileId contentFileId;
				string text;
				if (!RuntimeContentManager.Catalog.TryGetSceneLocation(activeScene.SceneId, out contentFileId, out text))
				{
					throw new Exception(string.Format("Invalid scene location: {0}", activeScene.SceneId));
				}
				string text2;
				UnsafeList<ContentFileId> unsafeList;
				ContentArchiveId contentArchiveId;
				int num;
				if (!RuntimeContentManager.Catalog.TryGetFileLocation(contentFileId, out text2, out unsafeList, out contentArchiveId, out num))
				{
					throw new Exception(string.Format("Invalid file location: {0} for scene {1} with name '{2}'.", contentFileId, activeScene.SceneId, text));
				}
				RuntimeContentManager.ReleaseArchive(contentArchiveId);
				ref UnsafeList<RuntimeContentManager.DeferredSceneDependencyUnload> data = ref RuntimeContentManager.SharedStaticDeferredSceneUnloads.Data;
				RuntimeContentManager.DeferredSceneDependencyUnload deferredSceneDependencyUnload = default(RuntimeContentManager.DeferredSceneDependencyUnload);
				deferredSceneDependencyUnload.DependencyIndex = num;
				deferredSceneDependencyUnload.FileIds = unsafeList;
				data.Add(in deferredSceneDependencyUnload);
			}
		}

		// Token: 0x06001C5F RID: 7263 RVA: 0x00063DD0 File Offset: 0x00061FD0
		private static void OnShutdown(object _, EventArgs __)
		{
			int num;
			RuntimeContentManager.Cleanup(out num);
		}

		// Token: 0x06001C60 RID: 7264 RVA: 0x00063DE8 File Offset: 0x00061FE8
		[ExcludeFromBurstCompatTesting("References managed objects")]
		public static bool Cleanup(out int unreleasedObjectCount)
		{
			unreleasedObjectCount = 0;
			if (!RuntimeContentManager.ActiveObjects.IsCreated)
			{
				return true;
			}
			try
			{
				RuntimeContentManager.SharedStaticObjectLoadQueue.Data.Dispose();
				RuntimeContentManager.SharedStaticObjectReleaseQueue.Data.Dispose();
				RuntimeContentManager.SharedStaticLoadingObjects.Data.Dispose();
				if (RuntimeContentManager.ActiveObjects.IsCreated && RuntimeContentManager.ActiveObjects.Count > 0)
				{
					using (NativeArray<UntypedWeakReferenceId> keyArray = RuntimeContentManager.ActiveObjects.GetKeyArray(Allocator.Temp))
					{
						for (int i = 0; i < keyArray.Length; i++)
						{
							unreleasedObjectCount++;
							RuntimeContentManager.ActiveObject activeObject;
							while (RuntimeContentManager.ActiveObjects.TryGetValue(keyArray[i], out activeObject))
							{
								RuntimeContentManager.ReleaseObjectImpl(activeObject.ObjectReferenceId);
							}
						}
					}
				}
				if (RuntimeContentManager.ActiveScenes.IsCreated)
				{
					if (RuntimeContentManager.ActiveScenes.Count > 0)
					{
						using (NativeArray<RuntimeContentManager.ActiveScene> valueArray = RuntimeContentManager.ActiveScenes.GetValueArray(Allocator.Temp))
						{
							for (int j = 0; j < valueArray.Length; j++)
							{
								Scene scene = valueArray[j].SceneFile.Scene;
								RuntimeContentManager.UnloadScene(ref scene);
							}
						}
					}
					RuntimeContentManager.ActiveScenes.Dispose();
				}
				for (int k = 0; k < RuntimeContentManager.SharedStaticDeferredSceneUnloads.Data.Length; k++)
				{
					RuntimeContentManager.DeferredSceneDependencyUnload deferredSceneDependencyUnload = RuntimeContentManager.SharedStaticDeferredSceneUnloads.Data[k];
					RuntimeContentManager.DeferredSceneUnloadImpl(ref deferredSceneDependencyUnload);
				}
				RuntimeContentManager.SharedStaticDeferredSceneUnloads.Data.Dispose();
				if (RuntimeContentManager.ActiveArchives.IsCreated)
				{
					RuntimeContentManager.ActiveArchives.Dispose();
				}
				if (RuntimeContentManager.ActiveDependencySets.IsCreated)
				{
					for (int l = 0; l < RuntimeContentManager.ActiveDependencySets.Length; l++)
					{
						RuntimeContentManager.ActiveDependencySet activeDependencySet = RuntimeContentManager.ActiveDependencySets[l];
						if (activeDependencySet.Files.IsCreated)
						{
							activeDependencySet.Files.Dispose();
						}
					}
					RuntimeContentManager.ActiveDependencySets.Dispose();
				}
				if (RuntimeContentManager.ActiveFiles.IsCreated)
				{
					RuntimeContentManager.ActiveFiles.Dispose();
				}
				if (RuntimeContentManager.ActiveObjects.IsCreated)
				{
					RuntimeContentManager.ActiveObjects.Dispose();
				}
				if (RuntimeContentManager.Catalog.IsCreated)
				{
					RuntimeContentManager.Catalog.Dispose();
				}
				if (RuntimeContentManager.SharedStaticObjectValueCache.Data.IsCreated)
				{
					RuntimeContentManager.SharedStaticObjectValueCache.Data.Dispose();
				}
				Resources.UnloadUnusedAssets();
			}
			catch (Exception ex)
			{
				Debug.LogException(ex);
			}
			SceneManager.sceneUnloaded -= RuntimeContentManager.ReleaseSceneResources;
			AppDomain.CurrentDomain.DomainUnload -= RuntimeContentManager.OnShutdown;
			AppDomain.CurrentDomain.ProcessExit -= RuntimeContentManager.OnShutdown;
			return unreleasedObjectCount == 0;
		}

		// Token: 0x06001C61 RID: 7265 RVA: 0x000640D8 File Offset: 0x000622D8
		[ExcludeFromBurstCompatTesting("References managed engine API")]
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
		private static void RuntimeInitialization()
		{
			RuntimeContentManager.Initialize();
			DefaultWorldInitialization.DefaultWorldDestroyed += delegate
			{
				int num;
				RuntimeContentManager.Cleanup(out num);
			};
		}

		// Token: 0x06001C62 RID: 7266 RVA: 0x00064104 File Offset: 0x00062304
		[ExcludeFromBurstCompatTesting("References managed objects")]
		public static bool LoadLocalCatalogData(string catalogPath, Func<string, string> fileNameFunc, Func<string, string> archivePathFunc)
		{
			if (!RuntimeContentManager.Catalog.LoadCatalogData(catalogPath, archivePathFunc, fileNameFunc))
			{
				return false;
			}
			if (!RuntimeContentManager.ActiveDependencySets.IsCreated)
			{
				RuntimeContentManager.ActiveDependencySets = new UnsafeList<RuntimeContentManager.ActiveDependencySet>(RuntimeContentManager.Catalog.FileDependencySetCount, Allocator.Persistent, NativeArrayOptions.ClearMemory);
			}
			RuntimeContentManager.ActiveDependencySets.Resize(RuntimeContentManager.Catalog.FileDependencySetCount, NativeArrayOptions.ClearMemory);
			return true;
		}

		// Token: 0x06001C63 RID: 7267 RVA: 0x0006415F File Offset: 0x0006235F
		[ExcludeFromBurstCompatTesting("Cannot return structs")]
		public static NativeArray<UntypedWeakReferenceId> GetObjectIds(AllocatorManager.AllocatorHandle alloc)
		{
			return RuntimeContentManager.Catalog.GetObjectIds(alloc);
		}

		// Token: 0x06001C64 RID: 7268 RVA: 0x0006416C File Offset: 0x0006236C
		[ExcludeFromBurstCompatTesting("Cannot return structs")]
		public static NativeArray<UntypedWeakReferenceId> GetSceneIds(AllocatorManager.AllocatorHandle alloc)
		{
			return RuntimeContentManager.Catalog.GetSceneIds(alloc);
		}

		// Token: 0x06001C65 RID: 7269 RVA: 0x00064179 File Offset: 0x00062379
		[BurstCompile]
		[GenerateTestsForBurstCompatibility]
		public static void LoadObjectAsync(in UntypedWeakReferenceId objectId)
		{
			RuntimeContentManager.LoadObjectAsync_0000192A$BurstDirectCall.Invoke(in objectId);
		}

		// Token: 0x06001C66 RID: 7270 RVA: 0x00064181 File Offset: 0x00062381
		[BurstCompile]
		[GenerateTestsForBurstCompatibility]
		public static void ReleaseObjectAsync(in UntypedWeakReferenceId objectId)
		{
			RuntimeContentManager.ReleaseObjectAsync_0000192B$BurstDirectCall.Invoke(in objectId);
		}

		// Token: 0x06001C67 RID: 7271 RVA: 0x0006418C File Offset: 0x0006238C
		[ExcludeFromBurstCompatTesting("DeferredSceneUnload is not burstable")]
		[MonoPInvokeCallback(typeof(RuntimeContentManager.DeferredSceneUnloadManagedDelegate))]
		private static void DeferredSceneUnloadImpl(ref RuntimeContentManager.DeferredSceneDependencyUnload id)
		{
			try
			{
				RuntimeContentManager.ReleaseActiveDependencySet(id.DependencyIndex, in id.FileIds);
			}
			catch (Exception ex)
			{
				Debug.LogException(ex);
			}
		}

		// Token: 0x06001C68 RID: 7272 RVA: 0x000641C4 File Offset: 0x000623C4
		[ExcludeFromBurstCompatTesting("LoadObjectImpl is not burstable")]
		[MonoPInvokeCallback(typeof(RuntimeContentManager.LoadObjectManagedDelegate))]
		private unsafe static void LoadObjectsImpl(UntypedWeakReferenceId* pObjectIds, int count)
		{
			try
			{
				if (RuntimeContentManager.ActiveObjects.Capacity - RuntimeContentManager.ActiveObjects.Count < count)
				{
					RuntimeContentManager.ActiveObjects.Capacity = RuntimeContentManager.ActiveObjects.Count + count;
					if (RuntimeContentManager.SharedStaticLoadingObjects.Data.Capacity - RuntimeContentManager.SharedStaticLoadingObjects.Data.Length < count)
					{
						RuntimeContentManager.SharedStaticLoadingObjects.Data.Capacity = RuntimeContentManager.SharedStaticLoadingObjects.Data.Length + count;
					}
				}
				for (int i = 0; i < count; i++)
				{
					RuntimeContentManager.LoadObjectImpl(in pObjectIds[i]);
				}
			}
			catch (Exception ex)
			{
				Debug.LogException(ex);
			}
		}

		// Token: 0x06001C69 RID: 7273 RVA: 0x00064278 File Offset: 0x00062478
		[ExcludeFromBurstCompatTesting("ReleaseObjectImpl is not burstable")]
		[MonoPInvokeCallback(typeof(RuntimeContentManager.ReleaseObjectManagedDelegate))]
		private unsafe static void ReleaseObjectsImpl(UntypedWeakReferenceId* pObjectIds, int count)
		{
			try
			{
				for (int i = 0; i < count; i++)
				{
					RuntimeContentManager.ReleaseObjectImpl(pObjectIds[i]);
				}
			}
			catch (Exception ex)
			{
				Debug.LogException(ex);
			}
		}

		// Token: 0x06001C6A RID: 7274 RVA: 0x000642C0 File Offset: 0x000624C0
		[ExcludeFromBurstCompatTesting("ComputeObjectLoadingStatus is not burstable")]
		[MonoPInvokeCallback(typeof(RuntimeContentManager.ProcessObjectManagedDelegate))]
		private static void UpdateLoadingObjectStatus()
		{
			try
			{
				int i = 0;
				while (i < RuntimeContentManager.SharedStaticLoadingObjects.Data.Length)
				{
					UntypedWeakReferenceId untypedWeakReferenceId = RuntimeContentManager.SharedStaticLoadingObjects.Data[i];
					if (RuntimeContentManager.ComputeObjectLoadingStatus(untypedWeakReferenceId) >= ObjectLoadingStatus.Completed)
					{
						GCHandle objectHandleImpl = RuntimeContentManager.GetObjectHandleImpl(untypedWeakReferenceId);
						RuntimeContentManager.SharedStaticObjectValueCache.Data.SetObjectStatus(in untypedWeakReferenceId, objectHandleImpl.IsAllocated ? ObjectLoadingStatus.Completed : ObjectLoadingStatus.Error, in objectHandleImpl);
						RuntimeContentManager.SharedStaticLoadingObjects.Data.RemoveAtSwapBack(i);
					}
					else
					{
						i++;
					}
				}
			}
			catch (Exception ex)
			{
				Debug.LogException(ex);
			}
		}

		// Token: 0x06001C6B RID: 7275 RVA: 0x00064354 File Offset: 0x00062554
		[BurstCompile]
		[GenerateTestsForBurstCompatibility]
		public static void ProcessQueuedCommands()
		{
			RuntimeContentManager.ProcessQueuedCommands_00001930$BurstDirectCall.Invoke();
		}

		// Token: 0x06001C6C RID: 7276 RVA: 0x0006435C File Offset: 0x0006255C
		[ExcludeFromBurstCompatTesting("ActiveObjects is static and not shared")]
		internal static int CompletedObjectLoads()
		{
			int num = 0;
			foreach (KVPair<UntypedWeakReferenceId, RuntimeContentManager.ActiveObject> kvpair in RuntimeContentManager.ActiveObjects)
			{
				UntypedWeakReferenceId key = kvpair.Key;
				if (RuntimeContentManager.GetObjectLoadingStatus(in key) >= ObjectLoadingStatus.Completed)
				{
					num++;
				}
			}
			return num;
		}

		// Token: 0x06001C6D RID: 7277 RVA: 0x000643C0 File Offset: 0x000625C0
		[ExcludeFromBurstCompatTesting("ActiveObjects is static and not shared")]
		internal static int UnfinishedObjectLoads()
		{
			int num = RuntimeContentManager.SharedStaticObjectLoadQueue.Data.Length;
			foreach (KVPair<UntypedWeakReferenceId, RuntimeContentManager.ActiveObject> kvpair in RuntimeContentManager.ActiveObjects)
			{
				UntypedWeakReferenceId key = kvpair.Key;
				if (RuntimeContentManager.GetObjectLoadingStatus(in key) < ObjectLoadingStatus.Completed)
				{
					num++;
				}
			}
			return num;
		}

		// Token: 0x06001C6E RID: 7278 RVA: 0x00064434 File Offset: 0x00062634
		[ExcludeFromBurstCompatTesting("SharedStaticLoadingObjects is static and not shared")]
		internal static int LoadingObjectsCount()
		{
			return RuntimeContentManager.SharedStaticLoadingObjects.Data.Length;
		}

		// Token: 0x06001C6F RID: 7279 RVA: 0x00064448 File Offset: 0x00062648
		[ExcludeFromBurstCompatTesting("References static data")]
		private static void ReleaseActiveDependencySet(int depIndex, in UnsafeList<ContentFileId> deps)
		{
			RuntimeContentManager.ActiveDependencySet activeDependencySet = RuntimeContentManager.ActiveDependencySets[depIndex];
			int num = activeDependencySet.ReferenceCount - 1;
			activeDependencySet.ReferenceCount = num;
			if (num == 0 && deps.Length > 0)
			{
				for (int i = 0; i < deps.Length; i++)
				{
					UnsafeList<ContentFileId> unsafeList = deps;
					if (unsafeList[i].IsValid)
					{
						unsafeList = deps;
						RuntimeContentManager.ReleaseFile(unsafeList[i]);
					}
				}
				activeDependencySet.Files.Dispose();
			}
			RuntimeContentManager.ActiveDependencySets[depIndex] = activeDependencySet;
		}

		// Token: 0x06001C70 RID: 7280 RVA: 0x000644D4 File Offset: 0x000626D4
		[ExcludeFromBurstCompatTesting("References static data")]
		private static void LoadActiveDependencySet(int depIndex, in UnsafeList<ContentFileId> deps, out RuntimeContentManager.ActiveDependencySet activeDepSet)
		{
			activeDepSet = RuntimeContentManager.ActiveDependencySets[depIndex];
			if (activeDepSet.ReferenceCount == 0 && deps.Length > 0)
			{
				activeDepSet.Files = new UnsafeList<ContentFile>(deps.Length, Allocator.Persistent, NativeArrayOptions.UninitializedMemory);
				for (int i = 0; i < deps.Length; i++)
				{
					UnsafeList<ContentFileId> unsafeList = deps;
					ContentFileId contentFileId = unsafeList[i];
					if (contentFileId.IsValid)
					{
						activeDepSet.Files.Add(in RuntimeContentManager.LoadFile(contentFileId).File);
					}
					else
					{
						ContentFile globalTableDependency = ContentFile.GlobalTableDependency;
						activeDepSet.Files.Add(in globalTableDependency);
					}
				}
			}
			activeDepSet.ReferenceCount++;
			RuntimeContentManager.ActiveDependencySets[depIndex] = activeDepSet;
		}

		// Token: 0x06001C71 RID: 7281 RVA: 0x00064590 File Offset: 0x00062790
		[ExcludeFromBurstCompatTesting("References managed engine API and static data")]
		private unsafe static RuntimeContentManager.ActiveFile LoadFile(ContentFileId fileId)
		{
			RuntimeContentManager.ActiveFile activeFile;
			if (RuntimeContentManager.ActiveFiles.TryGetValue(fileId, out activeFile))
			{
				activeFile.ReferenceCount++;
				RuntimeContentManager.ActiveFiles[fileId] = activeFile;
				return activeFile;
			}
			string text;
			UnsafeList<ContentFileId> unsafeList;
			ContentArchiveId contentArchiveId;
			int num;
			if (!RuntimeContentManager.Catalog.TryGetFileLocation(fileId, out text, out unsafeList, out contentArchiveId, out num))
			{
				throw new Exception(string.Format("Invalid file location: {0}", fileId));
			}
			activeFile = new RuntimeContentManager.ActiveFile
			{
				ContentFileId = fileId,
				ArchiveId = contentArchiveId,
				ReferenceCount = 1
			};
			RuntimeContentManager.ActiveDependencySet activeDependencySet;
			RuntimeContentManager.LoadActiveDependencySet(num, in unsafeList, out activeDependencySet);
			RuntimeContentManager.ActiveArchive activeArchive = (contentArchiveId.IsValid ? RuntimeContentManager.LoadArchive(contentArchiveId) : default(RuntimeContentManager.ActiveArchive));
			if (fileId.IsValid && !string.IsNullOrEmpty(text) && contentArchiveId.IsValid)
			{
				activeFile.File = ContentLoadInterface.LoadContentFileAsync(RuntimeContentManager.Namespace, text, (void*)activeDependencySet.Files.Ptr, activeDependencySet.Files.Length, activeArchive.Archive.JobHandle, false);
			}
			RuntimeContentManager.ActiveFiles.TryAdd(fileId, activeFile);
			return activeFile;
		}

		// Token: 0x06001C72 RID: 7282 RVA: 0x000646A0 File Offset: 0x000628A0
		[ExcludeFromBurstCompatTesting("References managed engine API and static data")]
		private static RuntimeContentManager.ActiveArchive LoadArchive(ContentArchiveId archiveId)
		{
			RuntimeContentManager.ActiveArchive activeArchive;
			if (RuntimeContentManager.ActiveArchives.TryGetValue(archiveId, out activeArchive))
			{
				activeArchive.ReferenceCount++;
				RuntimeContentManager.ActiveArchives[archiveId] = activeArchive;
				return activeArchive;
			}
			string text;
			if (!RuntimeContentManager.Catalog.TryGetArchiveLocation(archiveId, out text))
			{
				throw new Exception(string.Format("Invalid archive location: {0}", archiveId));
			}
			activeArchive = new RuntimeContentManager.ActiveArchive
			{
				ArchiveId = archiveId,
				ReferenceCount = 1
			};
			if (archiveId.IsValid)
			{
				activeArchive.Archive = ArchiveFileInterface.MountAsync(RuntimeContentManager.Namespace, text, RuntimeContentManager.ArchivePrefix);
			}
			RuntimeContentManager.ActiveArchives.TryAdd(archiveId, activeArchive);
			return activeArchive;
		}

		// Token: 0x06001C73 RID: 7283 RVA: 0x00064744 File Offset: 0x00062944
		[ExcludeFromBurstCompatTesting("References managed engine API and static data")]
		private static ObjectLoadingStatus ComputeObjectLoadingStatus(UntypedWeakReferenceId objectId)
		{
			RuntimeContentManager.ActiveObject activeObject;
			if (!RuntimeContentManager.ActiveObjects.TryGetValue(objectId, out activeObject))
			{
				return ObjectLoadingStatus.None;
			}
			RuntimeContentManager.ActiveFile activeFile;
			if (!RuntimeContentManager.ActiveFiles.TryGetValue(activeObject.ContentFileId, out activeFile))
			{
				return ObjectLoadingStatus.Error;
			}
			LoadingStatus loadingStatus = activeFile.File.LoadingStatus;
			if (loadingStatus == LoadingStatus.Completed)
			{
				return ObjectLoadingStatus.Completed;
			}
			if (loadingStatus == LoadingStatus.InProgress)
			{
				return ObjectLoadingStatus.Loading;
			}
			return ObjectLoadingStatus.Error;
		}

		// Token: 0x06001C74 RID: 7284 RVA: 0x00064791 File Offset: 0x00062991
		[BurstCompile]
		[GenerateTestsForBurstCompatibility]
		public unsafe static void ReleaseObjects(UntypedWeakReferenceId* pObjectIds, int count)
		{
			RuntimeContentManager.ReleaseObjects_00001939$BurstDirectCall.Invoke(pObjectIds, count);
		}

		// Token: 0x06001C75 RID: 7285 RVA: 0x0006479A File Offset: 0x0006299A
		[BurstCompile]
		[GenerateTestsForBurstCompatibility]
		public unsafe static void LoadObjectsAsync(UntypedWeakReferenceId* pObjectIds, int count)
		{
			RuntimeContentManager.LoadObjectsAsync_0000193A$BurstDirectCall.Invoke(pObjectIds, count);
		}

		// Token: 0x06001C76 RID: 7286 RVA: 0x000647A4 File Offset: 0x000629A4
		[ExcludeFromBurstCompatTesting("References managed engine API and static data")]
		private static bool LoadObjectImpl(in UntypedWeakReferenceId objectId)
		{
			RuntimeContentManager.ActiveObject activeObject;
			if (RuntimeContentManager.ActiveObjects.TryGetValue(objectId, out activeObject))
			{
				activeObject.ReferenceCount++;
				RuntimeContentManager.ActiveObjects[objectId] = activeObject;
				return true;
			}
			ContentFileId contentFileId = default(ContentFileId);
			long num = 0L;
			if (!RuntimeContentManager.Catalog.TryGetObjectLocation(objectId, out contentFileId, out num))
			{
				return false;
			}
			ref ObjectValueCache data = ref RuntimeContentManager.SharedStaticObjectValueCache.Data;
			ObjectLoadingStatus objectLoadingStatus = ObjectLoadingStatus.Loading;
			GCHandle gchandle = default(GCHandle);
			data.SetObjectStatus(in objectId, objectLoadingStatus, in gchandle);
			RuntimeContentManager.SharedStaticLoadingObjects.Data.Add(in objectId);
			if (contentFileId.IsValid)
			{
				RuntimeContentManager.LoadFile(contentFileId);
			}
			activeObject = new RuntimeContentManager.ActiveObject
			{
				ObjectReferenceId = objectId,
				LocalIdentifierInFile = num,
				ContentFileId = contentFileId,
				ReferenceCount = 1
			};
			RuntimeContentManager.ActiveObjects.TryAdd(objectId, activeObject);
			return true;
		}

		// Token: 0x06001C77 RID: 7287 RVA: 0x00064888 File Offset: 0x00062A88
		[ExcludeFromBurstCompatTesting("References managed engine API and static data")]
		private static bool ReleaseObjectImpl(UntypedWeakReferenceId objectId)
		{
			RuntimeContentManager.ActiveObject activeObject;
			if (!RuntimeContentManager.ActiveObjects.TryGetValue(objectId, out activeObject))
			{
				return false;
			}
			int num = activeObject.ReferenceCount - 1;
			activeObject.ReferenceCount = num;
			if (num <= 0)
			{
				RuntimeContentManager.ActiveObjects.Remove(objectId);
				ObjectLoadingStatus objectLoadingStatus;
				if (RuntimeContentManager.SharedStaticObjectValueCache.Data.RemoveEntry(in objectId, out objectLoadingStatus) && objectLoadingStatus < ObjectLoadingStatus.Completed)
				{
					for (int i = 0; i < RuntimeContentManager.SharedStaticLoadingObjects.Data.Length; i++)
					{
						if (RuntimeContentManager.SharedStaticLoadingObjects.Data[i] == objectId)
						{
							RuntimeContentManager.SharedStaticLoadingObjects.Data.RemoveAtSwapBack(i);
							break;
						}
					}
				}
				if (activeObject.ContentFileId.IsValid)
				{
					RuntimeContentManager.ReleaseFile(activeObject.ContentFileId);
				}
			}
			else
			{
				RuntimeContentManager.ActiveObjects[objectId] = activeObject;
			}
			return true;
		}

		// Token: 0x06001C78 RID: 7288 RVA: 0x0006494C File Offset: 0x00062B4C
		[ExcludeFromBurstCompatTesting("References managed engine API and static data")]
		public static Scene LoadSceneAsync(UntypedWeakReferenceId sceneId, ContentSceneParameters loadParams)
		{
			ContentFileId contentFileId;
			string text;
			if (!RuntimeContentManager.Catalog.TryGetSceneLocation(sceneId, out contentFileId, out text))
			{
				throw new Exception(string.Format("Invalid scene id: {0}", sceneId));
			}
			string text2;
			UnsafeList<ContentFileId> unsafeList;
			ContentArchiveId contentArchiveId;
			int num;
			if (!RuntimeContentManager.Catalog.TryGetFileLocation(contentFileId, out text2, out unsafeList, out contentArchiveId, out num))
			{
				throw new Exception(string.Format("Invalid file location: {0} for scene {1} with name '{2}'.", contentFileId, sceneId, text));
			}
			RuntimeContentManager.ActiveDependencySet activeDependencySet;
			RuntimeContentManager.LoadActiveDependencySet(num, in unsafeList, out activeDependencySet);
			RuntimeContentManager.ActiveArchive activeArchive = RuntimeContentManager.LoadArchive(contentArchiveId);
			ContentSceneFile contentSceneFile = ContentLoadInterface.LoadSceneAsync(RuntimeContentManager.Namespace, text2, text, loadParams, activeDependencySet.Files.Ptr, activeDependencySet.Files.Length, activeArchive.Archive.JobHandle);
			RuntimeContentManager.ActiveScenes.Add(contentSceneFile.Scene.handle, new RuntimeContentManager.ActiveScene
			{
				SceneFile = contentSceneFile,
				SceneId = sceneId
			});
			return contentSceneFile.Scene;
		}

		// Token: 0x06001C79 RID: 7289 RVA: 0x00064A38 File Offset: 0x00062C38
		[ExcludeFromBurstCompatTesting("References managed engine API and static data")]
		public static void UnloadScene(ref Scene scene)
		{
			RuntimeContentManager.ActiveScene activeScene;
			if (!RuntimeContentManager.ActiveScenes.TryGetValue(scene.handle, out activeScene))
			{
				throw new Exception(string.Format("Invalid scene: {0}", scene));
			}
			activeScene.SceneFile.UnloadAtEndOfFrame();
			scene = default(Scene);
		}

		// Token: 0x06001C7A RID: 7290 RVA: 0x00064A88 File Offset: 0x00062C88
		[ExcludeFromBurstCompatTesting("References managed engine API and static data")]
		private static void ReleaseFile(ContentFileId fileId)
		{
			RuntimeContentManager.ActiveFile activeFile;
			if (!RuntimeContentManager.ActiveFiles.TryGetValue(fileId, out activeFile))
			{
				throw new Exception(string.Format("Attempt to release inactive file location: {0}", fileId));
			}
			int num = activeFile.ReferenceCount - 1;
			activeFile.ReferenceCount = num;
			if (num > 0)
			{
				RuntimeContentManager.ActiveFiles[fileId] = activeFile;
				return;
			}
			RuntimeContentManager.ActiveFiles.Remove(activeFile.ContentFileId);
			if (activeFile.File.IsValid)
			{
				activeFile.File.UnloadAsync();
				RuntimeContentManager.ReleaseArchive(activeFile.ArchiveId);
			}
			UnsafeList<ContentFileId> unsafeList;
			ContentArchiveId contentArchiveId;
			int num2;
			if (!RuntimeContentManager.Catalog.TryGetFileLocation(fileId, out num, out unsafeList, out contentArchiveId, out num2))
			{
				throw new Exception(string.Format("Invalid file location: {0}", fileId));
			}
			RuntimeContentManager.ReleaseActiveDependencySet(num2, in unsafeList);
		}

		// Token: 0x06001C7B RID: 7291 RVA: 0x00064B44 File Offset: 0x00062D44
		[ExcludeFromBurstCompatTesting("References managed engine API and static data")]
		private static void ReleaseArchive(ContentArchiveId archiveId)
		{
			RuntimeContentManager.ActiveArchive activeArchive;
			if (!RuntimeContentManager.ActiveArchives.TryGetValue(archiveId, out activeArchive))
			{
				throw new Exception(string.Format("Invalid archive id: {0}", archiveId));
			}
			int num = activeArchive.ReferenceCount - 1;
			activeArchive.ReferenceCount = num;
			if (num <= 0)
			{
				if (!RuntimeContentManager.Catalog.TryGetArchiveLocation(archiveId, out num))
				{
					throw new Exception(string.Format("Invalid archive location: {0}", archiveId));
				}
				RuntimeContentManager.ActiveArchives.Remove(activeArchive.ArchiveId);
				if (activeArchive.ArchiveId.IsValid)
				{
					activeArchive.Archive.Unmount();
					return;
				}
			}
			else
			{
				RuntimeContentManager.ActiveArchives[archiveId] = activeArchive;
			}
		}

		// Token: 0x06001C7C RID: 7292 RVA: 0x00064BE8 File Offset: 0x00062DE8
		[ExcludeFromBurstCompatTesting("References managed engine API and static data")]
		public static bool WaitForObjectCompletion(UntypedWeakReferenceId objectId, int timeoutMs = 0)
		{
			RuntimeContentManager.ProcessQueuedCommands();
			RuntimeContentManager.ActiveObject activeObject;
			if (!RuntimeContentManager.ActiveObjects.TryGetValue(objectId, out activeObject))
			{
				return false;
			}
			RuntimeContentManager.ActiveFile activeFile;
			if (!RuntimeContentManager.ActiveFiles.TryGetValue(activeObject.ContentFileId, out activeFile))
			{
				throw new Exception(string.Format("Unable to find file location for object id {0}", activeObject.ContentFileId));
			}
			if (!activeFile.File.IsValid || activeFile.File.LoadingStatus == LoadingStatus.Failed)
			{
				return false;
			}
			if (activeFile.File.LoadingStatus == LoadingStatus.Completed)
			{
				return true;
			}
			if (!activeFile.File.WaitForCompletion(timeoutMs))
			{
				return false;
			}
			RuntimeContentManager.UpdateLoadingObjectStatus();
			return true;
		}

		// Token: 0x06001C7D RID: 7293 RVA: 0x00064C81 File Offset: 0x00062E81
		[BurstCompile]
		[GenerateTestsForBurstCompatibility]
		public static ObjectLoadingStatus GetObjectLoadingStatus(in UntypedWeakReferenceId objectId)
		{
			return RuntimeContentManager.GetObjectLoadingStatus_00001942$BurstDirectCall.Invoke(in objectId);
		}

		// Token: 0x06001C7E RID: 7294 RVA: 0x00064C8C File Offset: 0x00062E8C
		[ExcludeFromBurstCompatTesting("References managed engine API and static data")]
		public static TObject GetObjectValue<TObject>(UntypedWeakReferenceId objectId) where TObject : Object
		{
			GCHandle gchandle = default(GCHandle);
			if (!RuntimeContentManager.GetObjectHandle(in objectId, ref gchandle))
			{
				return default(TObject);
			}
			return gchandle.Target as TObject;
		}

		// Token: 0x06001C7F RID: 7295 RVA: 0x00064CC7 File Offset: 0x00062EC7
		[BurstCompile]
		private static bool GetObjectHandle(in UntypedWeakReferenceId objectId, ref GCHandle objectHandle)
		{
			return RuntimeContentManager.GetObjectHandle_00001944$BurstDirectCall.Invoke(in objectId, ref objectHandle);
		}

		// Token: 0x06001C80 RID: 7296 RVA: 0x00064CD0 File Offset: 0x00062ED0
		[ExcludeFromBurstCompatTesting("References managed engine API and static data")]
		private static GCHandle GetObjectHandleImpl(UntypedWeakReferenceId objectId)
		{
			RuntimeContentManager.ActiveObject activeObject;
			if (!RuntimeContentManager.ActiveObjects.TryGetValue(objectId, out activeObject))
			{
				return default(GCHandle);
			}
			RuntimeContentManager.ActiveFile activeFile;
			if (!RuntimeContentManager.ActiveFiles.TryGetValue(activeObject.ContentFileId, out activeFile))
			{
				throw new Exception(string.Format("Unable to find file location for object id {0}", activeObject.ContentFileId));
			}
			if (!activeFile.File.IsValid || activeFile.File.LoadingStatus == LoadingStatus.Failed)
			{
				return default(GCHandle);
			}
			Object @object = activeFile.File.GetObject((ulong)activeObject.LocalIdentifierInFile);
			if (!(@object == null))
			{
				return GCHandle.Alloc(@object);
			}
			return default(GCHandle);
		}

		// Token: 0x06001C81 RID: 7297 RVA: 0x0001CFE4 File Offset: 0x0001B1E4
		[ExcludeFromBurstCompatTesting("References managed engine API and static data")]
		[Obsolete("This property is no longer valid.  Check the loading status of the scene returned from LoadSceneAsync.")]
		public static SceneLoadingStatus GetSceneLoadingStatus(UntypedWeakReferenceId sceneId)
		{
			return SceneLoadingStatus.InProgress;
		}

		// Token: 0x06001C82 RID: 7298 RVA: 0x00064D78 File Offset: 0x00062F78
		[ExcludeFromBurstCompatTesting("References managed engine API and static data")]
		[Obsolete("This property is no longer valid.  The scene file does not exist in all cases (e.g. play mode).")]
		public static ContentSceneFile GetSceneFileValue(UntypedWeakReferenceId sceneId)
		{
			return default(ContentSceneFile);
		}

		// Token: 0x06001C83 RID: 7299 RVA: 0x00064D90 File Offset: 0x00062F90
		[ExcludeFromBurstCompatTesting("References managed engine API and static data")]
		[Obsolete("This property is no longer valid.  Use the scene returned from LoadAsync.")]
		public static Scene GetSceneValue(UntypedWeakReferenceId sceneId)
		{
			return default(Scene);
		}

		// Token: 0x06001C85 RID: 7301 RVA: 0x00064EC0 File Offset: 0x000630C0
		[BurstCompile]
		[GenerateTestsForBurstCompatibility]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal static void LoadObjectAsync$BurstManaged(in UntypedWeakReferenceId objectId)
		{
			RuntimeContentManager.SharedStaticObjectLoadQueue.Data.Produce(in objectId);
			RuntimeContentManager.SharedStaticObjectValueCache.Data.AddEntry(in objectId);
		}

		// Token: 0x06001C86 RID: 7302 RVA: 0x00064EE2 File Offset: 0x000630E2
		[BurstCompile]
		[GenerateTestsForBurstCompatibility]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal static void ReleaseObjectAsync$BurstManaged(in UntypedWeakReferenceId objectId)
		{
			RuntimeContentManager.SharedStaticObjectReleaseQueue.Data.Produce(in objectId);
		}

		// Token: 0x06001C87 RID: 7303 RVA: 0x00064EF4 File Offset: 0x000630F4
		[BurstCompile]
		[GenerateTestsForBurstCompatibility]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal unsafe static void ProcessQueuedCommands$BurstManaged()
		{
			NativeArray<UntypedWeakReferenceId> nativeArray;
			if (RuntimeContentManager.SharedStaticObjectLoadQueue.Data.ConsumeAll(out nativeArray, Allocator.Temp))
			{
				new FunctionPointer<RuntimeContentManager.LoadObjectManagedDelegate>(*RuntimeContentManager.s_ManagedLoadObjectTrampoline.Data).Invoke((UntypedWeakReferenceId*)nativeArray.GetUnsafePtr<UntypedWeakReferenceId>(), nativeArray.Length);
				nativeArray.Dispose();
			}
			NativeArray<UntypedWeakReferenceId> nativeArray2;
			if (RuntimeContentManager.SharedStaticObjectReleaseQueue.Data.ConsumeAll(out nativeArray2, Allocator.Temp))
			{
				new FunctionPointer<RuntimeContentManager.ReleaseObjectManagedDelegate>(*RuntimeContentManager.s_ManagedReleaseObjectTrampoline.Data).Invoke((UntypedWeakReferenceId*)nativeArray2.GetUnsafePtr<UntypedWeakReferenceId>(), nativeArray2.Length);
				nativeArray2.Dispose();
			}
			if (!RuntimeContentManager.SharedStaticLoadingObjects.Data.IsEmpty)
			{
				new FunctionPointer<RuntimeContentManager.ProcessObjectManagedDelegate>(*RuntimeContentManager.s_ManagedProcessObjectTrampoline.Data).Invoke();
			}
			if (!RuntimeContentManager.SharedStaticDeferredSceneUnloads.Data.IsEmpty)
			{
				for (int i = 0; i < RuntimeContentManager.SharedStaticDeferredSceneUnloads.Data.Length; i++)
				{
					RuntimeContentManager.DeferredSceneDependencyUnload deferredSceneDependencyUnload = RuntimeContentManager.SharedStaticDeferredSceneUnloads.Data[i];
					new FunctionPointer<RuntimeContentManager.DeferredSceneUnloadManagedDelegate>(*RuntimeContentManager.s_ManagedDeferredSceneUnloadTrampoline.Data).Invoke(ref deferredSceneDependencyUnload);
				}
				RuntimeContentManager.SharedStaticDeferredSceneUnloads.Data.Clear();
			}
		}

		// Token: 0x06001C88 RID: 7304 RVA: 0x00065035 File Offset: 0x00063235
		[BurstCompile]
		[GenerateTestsForBurstCompatibility]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal unsafe static void ReleaseObjects$BurstManaged(UntypedWeakReferenceId* pObjectIds, int count)
		{
			RuntimeContentManager.SharedStaticObjectReleaseQueue.Data.Produce(pObjectIds, count);
		}

		// Token: 0x06001C89 RID: 7305 RVA: 0x00065048 File Offset: 0x00063248
		[BurstCompile]
		[GenerateTestsForBurstCompatibility]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal unsafe static void LoadObjectsAsync$BurstManaged(UntypedWeakReferenceId* pObjectIds, int count)
		{
			RuntimeContentManager.SharedStaticObjectLoadQueue.Data.Produce(pObjectIds, count);
			RuntimeContentManager.SharedStaticObjectValueCache.Data.AddEntries(pObjectIds, count);
		}

		// Token: 0x06001C8A RID: 7306 RVA: 0x0006506C File Offset: 0x0006326C
		[BurstCompile]
		[GenerateTestsForBurstCompatibility]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal static ObjectLoadingStatus GetObjectLoadingStatus$BurstManaged(in UntypedWeakReferenceId objectId)
		{
			return RuntimeContentManager.SharedStaticObjectValueCache.Data.GetLoadingStatus(in objectId);
		}

		// Token: 0x06001C8B RID: 7307 RVA: 0x0006507E File Offset: 0x0006327E
		[BurstCompile]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal static bool GetObjectHandle$BurstManaged(in UntypedWeakReferenceId objectId, ref GCHandle objectHandle)
		{
			return RuntimeContentManager.SharedStaticObjectValueCache.Data.GetObjectHandle(in objectId, ref objectHandle);
		}

		// Token: 0x04000A1C RID: 2588
		internal const string k_NameSpaceString = "RTC";

		// Token: 0x04000A1D RID: 2589
		internal const string k_ContentArchiveDirectory = "ContentArchives";

		// Token: 0x04000A1E RID: 2590
		internal const string k_ContentCatalogFilename = "archive_dependencies.bin";

		// Token: 0x04000A1F RID: 2591
		private static string ArchivePrefix;

		// Token: 0x04000A20 RID: 2592
		internal static ContentNamespace Namespace;

		// Token: 0x04000A21 RID: 2593
		private static readonly SharedStatic<ObjectValueCache> SharedStaticObjectValueCache = SharedStatic<ObjectValueCache>.GetOrCreateUnsafe(0U, 586044589639224229L, 0L);

		// Token: 0x04000A22 RID: 2594
		private static readonly SharedStatic<MultiProducerSingleBulkConsumerQueue<UntypedWeakReferenceId>> SharedStaticObjectLoadQueue = SharedStatic<MultiProducerSingleBulkConsumerQueue<UntypedWeakReferenceId>>.GetOrCreateUnsafe(0U, -7107706614480199328L, 0L);

		// Token: 0x04000A23 RID: 2595
		private static readonly SharedStatic<MultiProducerSingleBulkConsumerQueue<UntypedWeakReferenceId>> SharedStaticObjectReleaseQueue = SharedStatic<MultiProducerSingleBulkConsumerQueue<UntypedWeakReferenceId>>.GetOrCreateUnsafe(0U, 8570539438554516767L, 0L);

		// Token: 0x04000A24 RID: 2596
		private static readonly SharedStatic<UnsafeList<UntypedWeakReferenceId>> SharedStaticLoadingObjects = SharedStatic<UnsafeList<UntypedWeakReferenceId>>.GetOrCreateUnsafe(0U, 5890554262965022295L, 0L);

		// Token: 0x04000A25 RID: 2597
		private static readonly SharedStatic<UnsafeList<RuntimeContentManager.DeferredSceneDependencyUnload>> SharedStaticDeferredSceneUnloads = SharedStatic<UnsafeList<RuntimeContentManager.DeferredSceneDependencyUnload>>.GetOrCreateUnsafe(0U, 9208436503320958588L, 0L);

		// Token: 0x04000A26 RID: 2598
		private static RuntimeContentCatalog Catalog;

		// Token: 0x04000A27 RID: 2599
		private static UnsafeHashMap<ContentArchiveId, RuntimeContentManager.ActiveArchive> ActiveArchives;

		// Token: 0x04000A28 RID: 2600
		private static UnsafeHashMap<ContentFileId, RuntimeContentManager.ActiveFile> ActiveFiles;

		// Token: 0x04000A29 RID: 2601
		private static UnsafeHashMap<UntypedWeakReferenceId, RuntimeContentManager.ActiveObject> ActiveObjects;

		// Token: 0x04000A2A RID: 2602
		private static UnsafeList<RuntimeContentManager.ActiveDependencySet> ActiveDependencySets;

		// Token: 0x04000A2B RID: 2603
		private static UnsafeHashMap<int, RuntimeContentManager.ActiveScene> ActiveScenes;

		// Token: 0x04000A2C RID: 2604
		private static int currentGeneration = -1;

		// Token: 0x04000A2D RID: 2605
		private static readonly SharedStatic<IntPtr> s_ManagedDeferredSceneUnloadTrampoline = SharedStatic<IntPtr>.GetOrCreateUnsafe(0U, 3079409772809454621L, 0L);

		// Token: 0x04000A2E RID: 2606
		private static object s_DeferredSceneUnloadDelegateGCPrevention;

		// Token: 0x04000A2F RID: 2607
		private static readonly SharedStatic<IntPtr> s_ManagedLoadObjectTrampoline = SharedStatic<IntPtr>.GetOrCreateUnsafe(0U, -4905990729151978546L, 0L);

		// Token: 0x04000A30 RID: 2608
		private static object s_LoadObjectDelegateGCPrevention;

		// Token: 0x04000A31 RID: 2609
		private static readonly SharedStatic<IntPtr> s_ManagedReleaseObjectTrampoline = SharedStatic<IntPtr>.GetOrCreateUnsafe(0U, -9144847698586836639L, 0L);

		// Token: 0x04000A32 RID: 2610
		private static object s_ReleaseObjectDelegateGCPrevention;

		// Token: 0x04000A33 RID: 2611
		private static readonly SharedStatic<IntPtr> s_ManagedProcessObjectTrampoline = SharedStatic<IntPtr>.GetOrCreateUnsafe(0U, -7304402557799918069L, 0L);

		// Token: 0x04000A34 RID: 2612
		private static object s_ProcessObjectDelegateGCPrevention;

		// Token: 0x02000488 RID: 1160
		private struct ActiveArchive
		{
			// Token: 0x06001C8C RID: 7308 RVA: 0x00065091 File Offset: 0x00063291
			public override int GetHashCode()
			{
				return this.ArchiveId.GetHashCode();
			}

			// Token: 0x04000A35 RID: 2613
			public ContentArchiveId ArchiveId;

			// Token: 0x04000A36 RID: 2614
			public int ReferenceCount;

			// Token: 0x04000A37 RID: 2615
			public ArchiveHandle Archive;
		}

		// Token: 0x02000489 RID: 1161
		private struct ActiveFile
		{
			// Token: 0x06001C8D RID: 7309 RVA: 0x000650A4 File Offset: 0x000632A4
			public override int GetHashCode()
			{
				return this.ContentFileId.GetHashCode();
			}

			// Token: 0x04000A38 RID: 2616
			public ContentFileId ContentFileId;

			// Token: 0x04000A39 RID: 2617
			public int ReferenceCount;

			// Token: 0x04000A3A RID: 2618
			public ContentArchiveId ArchiveId;

			// Token: 0x04000A3B RID: 2619
			public ContentFile File;
		}

		// Token: 0x0200048A RID: 1162
		private struct ActiveObject
		{
			// Token: 0x06001C8E RID: 7310 RVA: 0x000650B7 File Offset: 0x000632B7
			public override int GetHashCode()
			{
				return this.ObjectReferenceId.GetHashCode();
			}

			// Token: 0x04000A3C RID: 2620
			public UntypedWeakReferenceId ObjectReferenceId;

			// Token: 0x04000A3D RID: 2621
			public int ReferenceCount;

			// Token: 0x04000A3E RID: 2622
			public ContentFileId ContentFileId;

			// Token: 0x04000A3F RID: 2623
			public long LocalIdentifierInFile;
		}

		// Token: 0x0200048B RID: 1163
		private struct ActiveScene
		{
			// Token: 0x04000A40 RID: 2624
			public UntypedWeakReferenceId SceneId;

			// Token: 0x04000A41 RID: 2625
			public ContentSceneFile SceneFile;
		}

		// Token: 0x0200048C RID: 1164
		private struct ActiveDependencySet
		{
			// Token: 0x04000A42 RID: 2626
			public int ReferenceCount;

			// Token: 0x04000A43 RID: 2627
			public UnsafeList<ContentFile> Files;
		}

		// Token: 0x0200048D RID: 1165
		private struct DeferredSceneDependencyUnload
		{
			// Token: 0x04000A44 RID: 2628
			public int DependencyIndex;

			// Token: 0x04000A45 RID: 2629
			public UnsafeList<ContentFileId> FileIds;
		}

		// Token: 0x0200048E RID: 1166
		private struct ObjectValueCacheType
		{
		}

		// Token: 0x0200048F RID: 1167
		private struct LoadQueueType
		{
		}

		// Token: 0x02000490 RID: 1168
		private struct ReleaseQueueType
		{
		}

		// Token: 0x02000491 RID: 1169
		private struct LoadingObjectsType
		{
		}

		// Token: 0x02000492 RID: 1170
		private struct DeferredSceneUnloadsType
		{
		}

		// Token: 0x02000493 RID: 1171
		// (Invoke) Token: 0x06001C90 RID: 7312
		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		private delegate void DeferredSceneUnloadManagedDelegate(ref RuntimeContentManager.DeferredSceneDependencyUnload id);

		// Token: 0x02000494 RID: 1172
		// (Invoke) Token: 0x06001C94 RID: 7316
		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		private unsafe delegate void LoadObjectManagedDelegate(UntypedWeakReferenceId* ids, int count);

		// Token: 0x02000495 RID: 1173
		// (Invoke) Token: 0x06001C98 RID: 7320
		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		private unsafe delegate void ReleaseObjectManagedDelegate(UntypedWeakReferenceId* ids, int count);

		// Token: 0x02000496 RID: 1174
		// (Invoke) Token: 0x06001C9C RID: 7324
		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		private delegate void ProcessObjectManagedDelegate();

		// Token: 0x02000498 RID: 1176
		// (Invoke) Token: 0x06001CA4 RID: 7332
		internal delegate void LoadObjectAsync_0000192A$PostfixBurstDelegate(in UntypedWeakReferenceId objectId);

		// Token: 0x02000499 RID: 1177
		internal static class LoadObjectAsync_0000192A$BurstDirectCall
		{
			// Token: 0x06001CA7 RID: 7335 RVA: 0x00065105 File Offset: 0x00063305
			[BurstDiscard]
			private static void GetFunctionPointerDiscard(ref IntPtr A_0)
			{
				if (RuntimeContentManager.LoadObjectAsync_0000192A$BurstDirectCall.Pointer == 0)
				{
					RuntimeContentManager.LoadObjectAsync_0000192A$BurstDirectCall.Pointer = BurstCompiler.GetILPPMethodFunctionPointer2(RuntimeContentManager.LoadObjectAsync_0000192A$BurstDirectCall.DeferredCompilation, methodof(RuntimeContentManager.LoadObjectAsync$BurstManaged(ref UntypedWeakReferenceId)).MethodHandle, typeof(RuntimeContentManager.LoadObjectAsync_0000192A$PostfixBurstDelegate).TypeHandle);
				}
				A_0 = RuntimeContentManager.LoadObjectAsync_0000192A$BurstDirectCall.Pointer;
			}

			// Token: 0x06001CA8 RID: 7336 RVA: 0x00065134 File Offset: 0x00063334
			private static IntPtr GetFunctionPointer()
			{
				IntPtr intPtr = (IntPtr)0;
				RuntimeContentManager.LoadObjectAsync_0000192A$BurstDirectCall.GetFunctionPointerDiscard(ref intPtr);
				return intPtr;
			}

			// Token: 0x06001CA9 RID: 7337 RVA: 0x0006514C File Offset: 0x0006334C
			public static void Constructor()
			{
				RuntimeContentManager.LoadObjectAsync_0000192A$BurstDirectCall.DeferredCompilation = BurstCompiler.CompileILPPMethod2(methodof(RuntimeContentManager.LoadObjectAsync(ref UntypedWeakReferenceId)).MethodHandle);
			}

			// Token: 0x06001CAA RID: 7338 RVA: 0x00002287 File Offset: 0x00000487
			public static void Initialize()
			{
			}

			// Token: 0x06001CAB RID: 7339 RVA: 0x0006515D File Offset: 0x0006335D
			// Note: this type is marked as 'beforefieldinit'.
			static LoadObjectAsync_0000192A$BurstDirectCall()
			{
				RuntimeContentManager.LoadObjectAsync_0000192A$BurstDirectCall.Constructor();
			}

			// Token: 0x06001CAC RID: 7340 RVA: 0x00065164 File Offset: 0x00063364
			public static void Invoke(in UntypedWeakReferenceId objectId)
			{
				if (BurstCompiler.IsEnabled)
				{
					IntPtr functionPointer = RuntimeContentManager.LoadObjectAsync_0000192A$BurstDirectCall.GetFunctionPointer();
					if (functionPointer != 0)
					{
						calli(System.Void(Unity.Entities.Serialization.UntypedWeakReferenceId&), ref objectId, functionPointer);
						return;
					}
				}
				RuntimeContentManager.LoadObjectAsync$BurstManaged(in objectId);
			}

			// Token: 0x04000A49 RID: 2633
			private static IntPtr Pointer;

			// Token: 0x04000A4A RID: 2634
			private static IntPtr DeferredCompilation;
		}

		// Token: 0x0200049A RID: 1178
		// (Invoke) Token: 0x06001CAE RID: 7342
		internal delegate void ReleaseObjectAsync_0000192B$PostfixBurstDelegate(in UntypedWeakReferenceId objectId);

		// Token: 0x0200049B RID: 1179
		internal static class ReleaseObjectAsync_0000192B$BurstDirectCall
		{
			// Token: 0x06001CB1 RID: 7345 RVA: 0x00065195 File Offset: 0x00063395
			[BurstDiscard]
			private static void GetFunctionPointerDiscard(ref IntPtr A_0)
			{
				if (RuntimeContentManager.ReleaseObjectAsync_0000192B$BurstDirectCall.Pointer == 0)
				{
					RuntimeContentManager.ReleaseObjectAsync_0000192B$BurstDirectCall.Pointer = BurstCompiler.GetILPPMethodFunctionPointer2(RuntimeContentManager.ReleaseObjectAsync_0000192B$BurstDirectCall.DeferredCompilation, methodof(RuntimeContentManager.ReleaseObjectAsync$BurstManaged(ref UntypedWeakReferenceId)).MethodHandle, typeof(RuntimeContentManager.ReleaseObjectAsync_0000192B$PostfixBurstDelegate).TypeHandle);
				}
				A_0 = RuntimeContentManager.ReleaseObjectAsync_0000192B$BurstDirectCall.Pointer;
			}

			// Token: 0x06001CB2 RID: 7346 RVA: 0x000651C4 File Offset: 0x000633C4
			private static IntPtr GetFunctionPointer()
			{
				IntPtr intPtr = (IntPtr)0;
				RuntimeContentManager.ReleaseObjectAsync_0000192B$BurstDirectCall.GetFunctionPointerDiscard(ref intPtr);
				return intPtr;
			}

			// Token: 0x06001CB3 RID: 7347 RVA: 0x000651DC File Offset: 0x000633DC
			public static void Constructor()
			{
				RuntimeContentManager.ReleaseObjectAsync_0000192B$BurstDirectCall.DeferredCompilation = BurstCompiler.CompileILPPMethod2(methodof(RuntimeContentManager.ReleaseObjectAsync(ref UntypedWeakReferenceId)).MethodHandle);
			}

			// Token: 0x06001CB4 RID: 7348 RVA: 0x00002287 File Offset: 0x00000487
			public static void Initialize()
			{
			}

			// Token: 0x06001CB5 RID: 7349 RVA: 0x000651ED File Offset: 0x000633ED
			// Note: this type is marked as 'beforefieldinit'.
			static ReleaseObjectAsync_0000192B$BurstDirectCall()
			{
				RuntimeContentManager.ReleaseObjectAsync_0000192B$BurstDirectCall.Constructor();
			}

			// Token: 0x06001CB6 RID: 7350 RVA: 0x000651F4 File Offset: 0x000633F4
			public static void Invoke(in UntypedWeakReferenceId objectId)
			{
				if (BurstCompiler.IsEnabled)
				{
					IntPtr functionPointer = RuntimeContentManager.ReleaseObjectAsync_0000192B$BurstDirectCall.GetFunctionPointer();
					if (functionPointer != 0)
					{
						calli(System.Void(Unity.Entities.Serialization.UntypedWeakReferenceId&), ref objectId, functionPointer);
						return;
					}
				}
				RuntimeContentManager.ReleaseObjectAsync$BurstManaged(in objectId);
			}

			// Token: 0x04000A4B RID: 2635
			private static IntPtr Pointer;

			// Token: 0x04000A4C RID: 2636
			private static IntPtr DeferredCompilation;
		}

		// Token: 0x0200049C RID: 1180
		// (Invoke) Token: 0x06001CB8 RID: 7352
		internal delegate void ProcessQueuedCommands_00001930$PostfixBurstDelegate();

		// Token: 0x0200049D RID: 1181
		internal static class ProcessQueuedCommands_00001930$BurstDirectCall
		{
			// Token: 0x06001CBB RID: 7355 RVA: 0x00065225 File Offset: 0x00063425
			[BurstDiscard]
			private static void GetFunctionPointerDiscard(ref IntPtr A_0)
			{
				if (RuntimeContentManager.ProcessQueuedCommands_00001930$BurstDirectCall.Pointer == 0)
				{
					RuntimeContentManager.ProcessQueuedCommands_00001930$BurstDirectCall.Pointer = BurstCompiler.GetILPPMethodFunctionPointer2(RuntimeContentManager.ProcessQueuedCommands_00001930$BurstDirectCall.DeferredCompilation, methodof(RuntimeContentManager.ProcessQueuedCommands$BurstManaged()).MethodHandle, typeof(RuntimeContentManager.ProcessQueuedCommands_00001930$PostfixBurstDelegate).TypeHandle);
				}
				A_0 = RuntimeContentManager.ProcessQueuedCommands_00001930$BurstDirectCall.Pointer;
			}

			// Token: 0x06001CBC RID: 7356 RVA: 0x00065254 File Offset: 0x00063454
			private static IntPtr GetFunctionPointer()
			{
				IntPtr intPtr = (IntPtr)0;
				RuntimeContentManager.ProcessQueuedCommands_00001930$BurstDirectCall.GetFunctionPointerDiscard(ref intPtr);
				return intPtr;
			}

			// Token: 0x06001CBD RID: 7357 RVA: 0x0006526C File Offset: 0x0006346C
			public static void Constructor()
			{
				RuntimeContentManager.ProcessQueuedCommands_00001930$BurstDirectCall.DeferredCompilation = BurstCompiler.CompileILPPMethod2(methodof(RuntimeContentManager.ProcessQueuedCommands()).MethodHandle);
			}

			// Token: 0x06001CBE RID: 7358 RVA: 0x00002287 File Offset: 0x00000487
			public static void Initialize()
			{
			}

			// Token: 0x06001CBF RID: 7359 RVA: 0x0006527D File Offset: 0x0006347D
			// Note: this type is marked as 'beforefieldinit'.
			static ProcessQueuedCommands_00001930$BurstDirectCall()
			{
				RuntimeContentManager.ProcessQueuedCommands_00001930$BurstDirectCall.Constructor();
			}

			// Token: 0x06001CC0 RID: 7360 RVA: 0x00065284 File Offset: 0x00063484
			public static void Invoke()
			{
				if (BurstCompiler.IsEnabled)
				{
					IntPtr functionPointer = RuntimeContentManager.ProcessQueuedCommands_00001930$BurstDirectCall.GetFunctionPointer();
					if (functionPointer != 0)
					{
						calli(System.Void(), functionPointer);
						return;
					}
				}
				RuntimeContentManager.ProcessQueuedCommands$BurstManaged();
			}

			// Token: 0x04000A4D RID: 2637
			private static IntPtr Pointer;

			// Token: 0x04000A4E RID: 2638
			private static IntPtr DeferredCompilation;
		}

		// Token: 0x0200049E RID: 1182
		// (Invoke) Token: 0x06001CC2 RID: 7362
		internal unsafe delegate void ReleaseObjects_00001939$PostfixBurstDelegate(UntypedWeakReferenceId* pObjectIds, int count);

		// Token: 0x0200049F RID: 1183
		internal static class ReleaseObjects_00001939$BurstDirectCall
		{
			// Token: 0x06001CC5 RID: 7365 RVA: 0x000652B3 File Offset: 0x000634B3
			[BurstDiscard]
			private unsafe static void GetFunctionPointerDiscard(ref IntPtr A_0)
			{
				if (RuntimeContentManager.ReleaseObjects_00001939$BurstDirectCall.Pointer == 0)
				{
					RuntimeContentManager.ReleaseObjects_00001939$BurstDirectCall.Pointer = BurstCompiler.GetILPPMethodFunctionPointer2(RuntimeContentManager.ReleaseObjects_00001939$BurstDirectCall.DeferredCompilation, methodof(RuntimeContentManager.ReleaseObjects$BurstManaged(UntypedWeakReferenceId*, int)).MethodHandle, typeof(RuntimeContentManager.ReleaseObjects_00001939$PostfixBurstDelegate).TypeHandle);
				}
				A_0 = RuntimeContentManager.ReleaseObjects_00001939$BurstDirectCall.Pointer;
			}

			// Token: 0x06001CC6 RID: 7366 RVA: 0x000652E0 File Offset: 0x000634E0
			private static IntPtr GetFunctionPointer()
			{
				IntPtr intPtr = (IntPtr)0;
				RuntimeContentManager.ReleaseObjects_00001939$BurstDirectCall.GetFunctionPointerDiscard(ref intPtr);
				return intPtr;
			}

			// Token: 0x06001CC7 RID: 7367 RVA: 0x000652F8 File Offset: 0x000634F8
			public unsafe static void Constructor()
			{
				RuntimeContentManager.ReleaseObjects_00001939$BurstDirectCall.DeferredCompilation = BurstCompiler.CompileILPPMethod2(methodof(RuntimeContentManager.ReleaseObjects(UntypedWeakReferenceId*, int)).MethodHandle);
			}

			// Token: 0x06001CC8 RID: 7368 RVA: 0x00002287 File Offset: 0x00000487
			public static void Initialize()
			{
			}

			// Token: 0x06001CC9 RID: 7369 RVA: 0x00065309 File Offset: 0x00063509
			// Note: this type is marked as 'beforefieldinit'.
			static ReleaseObjects_00001939$BurstDirectCall()
			{
				RuntimeContentManager.ReleaseObjects_00001939$BurstDirectCall.Constructor();
			}

			// Token: 0x06001CCA RID: 7370 RVA: 0x00065310 File Offset: 0x00063510
			public unsafe static void Invoke(UntypedWeakReferenceId* pObjectIds, int count)
			{
				if (BurstCompiler.IsEnabled)
				{
					IntPtr functionPointer = RuntimeContentManager.ReleaseObjects_00001939$BurstDirectCall.GetFunctionPointer();
					if (functionPointer != 0)
					{
						calli(System.Void(Unity.Entities.Serialization.UntypedWeakReferenceId*,System.Int32), pObjectIds, count, functionPointer);
						return;
					}
				}
				RuntimeContentManager.ReleaseObjects$BurstManaged(pObjectIds, count);
			}

			// Token: 0x04000A4F RID: 2639
			private static IntPtr Pointer;

			// Token: 0x04000A50 RID: 2640
			private static IntPtr DeferredCompilation;
		}

		// Token: 0x020004A0 RID: 1184
		// (Invoke) Token: 0x06001CCC RID: 7372
		internal unsafe delegate void LoadObjectsAsync_0000193A$PostfixBurstDelegate(UntypedWeakReferenceId* pObjectIds, int count);

		// Token: 0x020004A1 RID: 1185
		internal static class LoadObjectsAsync_0000193A$BurstDirectCall
		{
			// Token: 0x06001CCF RID: 7375 RVA: 0x00065343 File Offset: 0x00063543
			[BurstDiscard]
			private unsafe static void GetFunctionPointerDiscard(ref IntPtr A_0)
			{
				if (RuntimeContentManager.LoadObjectsAsync_0000193A$BurstDirectCall.Pointer == 0)
				{
					RuntimeContentManager.LoadObjectsAsync_0000193A$BurstDirectCall.Pointer = BurstCompiler.GetILPPMethodFunctionPointer2(RuntimeContentManager.LoadObjectsAsync_0000193A$BurstDirectCall.DeferredCompilation, methodof(RuntimeContentManager.LoadObjectsAsync$BurstManaged(UntypedWeakReferenceId*, int)).MethodHandle, typeof(RuntimeContentManager.LoadObjectsAsync_0000193A$PostfixBurstDelegate).TypeHandle);
				}
				A_0 = RuntimeContentManager.LoadObjectsAsync_0000193A$BurstDirectCall.Pointer;
			}

			// Token: 0x06001CD0 RID: 7376 RVA: 0x00065370 File Offset: 0x00063570
			private static IntPtr GetFunctionPointer()
			{
				IntPtr intPtr = (IntPtr)0;
				RuntimeContentManager.LoadObjectsAsync_0000193A$BurstDirectCall.GetFunctionPointerDiscard(ref intPtr);
				return intPtr;
			}

			// Token: 0x06001CD1 RID: 7377 RVA: 0x00065388 File Offset: 0x00063588
			public unsafe static void Constructor()
			{
				RuntimeContentManager.LoadObjectsAsync_0000193A$BurstDirectCall.DeferredCompilation = BurstCompiler.CompileILPPMethod2(methodof(RuntimeContentManager.LoadObjectsAsync(UntypedWeakReferenceId*, int)).MethodHandle);
			}

			// Token: 0x06001CD2 RID: 7378 RVA: 0x00002287 File Offset: 0x00000487
			public static void Initialize()
			{
			}

			// Token: 0x06001CD3 RID: 7379 RVA: 0x00065399 File Offset: 0x00063599
			// Note: this type is marked as 'beforefieldinit'.
			static LoadObjectsAsync_0000193A$BurstDirectCall()
			{
				RuntimeContentManager.LoadObjectsAsync_0000193A$BurstDirectCall.Constructor();
			}

			// Token: 0x06001CD4 RID: 7380 RVA: 0x000653A0 File Offset: 0x000635A0
			public unsafe static void Invoke(UntypedWeakReferenceId* pObjectIds, int count)
			{
				if (BurstCompiler.IsEnabled)
				{
					IntPtr functionPointer = RuntimeContentManager.LoadObjectsAsync_0000193A$BurstDirectCall.GetFunctionPointer();
					if (functionPointer != 0)
					{
						calli(System.Void(Unity.Entities.Serialization.UntypedWeakReferenceId*,System.Int32), pObjectIds, count, functionPointer);
						return;
					}
				}
				RuntimeContentManager.LoadObjectsAsync$BurstManaged(pObjectIds, count);
			}

			// Token: 0x04000A51 RID: 2641
			private static IntPtr Pointer;

			// Token: 0x04000A52 RID: 2642
			private static IntPtr DeferredCompilation;
		}

		// Token: 0x020004A2 RID: 1186
		// (Invoke) Token: 0x06001CD6 RID: 7382
		internal delegate ObjectLoadingStatus GetObjectLoadingStatus_00001942$PostfixBurstDelegate(in UntypedWeakReferenceId objectId);

		// Token: 0x020004A3 RID: 1187
		internal static class GetObjectLoadingStatus_00001942$BurstDirectCall
		{
			// Token: 0x06001CD9 RID: 7385 RVA: 0x000653D3 File Offset: 0x000635D3
			[BurstDiscard]
			private static void GetFunctionPointerDiscard(ref IntPtr A_0)
			{
				if (RuntimeContentManager.GetObjectLoadingStatus_00001942$BurstDirectCall.Pointer == 0)
				{
					RuntimeContentManager.GetObjectLoadingStatus_00001942$BurstDirectCall.Pointer = BurstCompiler.GetILPPMethodFunctionPointer2(RuntimeContentManager.GetObjectLoadingStatus_00001942$BurstDirectCall.DeferredCompilation, methodof(RuntimeContentManager.GetObjectLoadingStatus$BurstManaged(ref UntypedWeakReferenceId)).MethodHandle, typeof(RuntimeContentManager.GetObjectLoadingStatus_00001942$PostfixBurstDelegate).TypeHandle);
				}
				A_0 = RuntimeContentManager.GetObjectLoadingStatus_00001942$BurstDirectCall.Pointer;
			}

			// Token: 0x06001CDA RID: 7386 RVA: 0x00065400 File Offset: 0x00063600
			private static IntPtr GetFunctionPointer()
			{
				IntPtr intPtr = (IntPtr)0;
				RuntimeContentManager.GetObjectLoadingStatus_00001942$BurstDirectCall.GetFunctionPointerDiscard(ref intPtr);
				return intPtr;
			}

			// Token: 0x06001CDB RID: 7387 RVA: 0x00065418 File Offset: 0x00063618
			public static void Constructor()
			{
				RuntimeContentManager.GetObjectLoadingStatus_00001942$BurstDirectCall.DeferredCompilation = BurstCompiler.CompileILPPMethod2(methodof(RuntimeContentManager.GetObjectLoadingStatus(ref UntypedWeakReferenceId)).MethodHandle);
			}

			// Token: 0x06001CDC RID: 7388 RVA: 0x00002287 File Offset: 0x00000487
			public static void Initialize()
			{
			}

			// Token: 0x06001CDD RID: 7389 RVA: 0x00065429 File Offset: 0x00063629
			// Note: this type is marked as 'beforefieldinit'.
			static GetObjectLoadingStatus_00001942$BurstDirectCall()
			{
				RuntimeContentManager.GetObjectLoadingStatus_00001942$BurstDirectCall.Constructor();
			}

			// Token: 0x06001CDE RID: 7390 RVA: 0x00065430 File Offset: 0x00063630
			public static ObjectLoadingStatus Invoke(in UntypedWeakReferenceId objectId)
			{
				if (BurstCompiler.IsEnabled)
				{
					IntPtr functionPointer = RuntimeContentManager.GetObjectLoadingStatus_00001942$BurstDirectCall.GetFunctionPointer();
					if (functionPointer != 0)
					{
						return calli(Unity.Entities.Content.ObjectLoadingStatus(Unity.Entities.Serialization.UntypedWeakReferenceId&), ref objectId, functionPointer);
					}
				}
				return RuntimeContentManager.GetObjectLoadingStatus$BurstManaged(in objectId);
			}

			// Token: 0x04000A53 RID: 2643
			private static IntPtr Pointer;

			// Token: 0x04000A54 RID: 2644
			private static IntPtr DeferredCompilation;
		}

		// Token: 0x020004A4 RID: 1188
		// (Invoke) Token: 0x06001CE0 RID: 7392
		internal delegate bool GetObjectHandle_00001944$PostfixBurstDelegate(in UntypedWeakReferenceId objectId, ref GCHandle objectHandle);

		// Token: 0x020004A5 RID: 1189
		internal static class GetObjectHandle_00001944$BurstDirectCall
		{
			// Token: 0x06001CE3 RID: 7395 RVA: 0x00065461 File Offset: 0x00063661
			[BurstDiscard]
			private static void GetFunctionPointerDiscard(ref IntPtr A_0)
			{
				if (RuntimeContentManager.GetObjectHandle_00001944$BurstDirectCall.Pointer == 0)
				{
					RuntimeContentManager.GetObjectHandle_00001944$BurstDirectCall.Pointer = BurstCompiler.GetILPPMethodFunctionPointer2(RuntimeContentManager.GetObjectHandle_00001944$BurstDirectCall.DeferredCompilation, methodof(RuntimeContentManager.GetObjectHandle$BurstManaged(ref UntypedWeakReferenceId, ref GCHandle)).MethodHandle, typeof(RuntimeContentManager.GetObjectHandle_00001944$PostfixBurstDelegate).TypeHandle);
				}
				A_0 = RuntimeContentManager.GetObjectHandle_00001944$BurstDirectCall.Pointer;
			}

			// Token: 0x06001CE4 RID: 7396 RVA: 0x00065490 File Offset: 0x00063690
			private static IntPtr GetFunctionPointer()
			{
				IntPtr intPtr = (IntPtr)0;
				RuntimeContentManager.GetObjectHandle_00001944$BurstDirectCall.GetFunctionPointerDiscard(ref intPtr);
				return intPtr;
			}

			// Token: 0x06001CE5 RID: 7397 RVA: 0x000654A8 File Offset: 0x000636A8
			public static void Constructor()
			{
				RuntimeContentManager.GetObjectHandle_00001944$BurstDirectCall.DeferredCompilation = BurstCompiler.CompileILPPMethod2(methodof(RuntimeContentManager.GetObjectHandle(ref UntypedWeakReferenceId, ref GCHandle)).MethodHandle);
			}

			// Token: 0x06001CE6 RID: 7398 RVA: 0x00002287 File Offset: 0x00000487
			public static void Initialize()
			{
			}

			// Token: 0x06001CE7 RID: 7399 RVA: 0x000654B9 File Offset: 0x000636B9
			// Note: this type is marked as 'beforefieldinit'.
			static GetObjectHandle_00001944$BurstDirectCall()
			{
				RuntimeContentManager.GetObjectHandle_00001944$BurstDirectCall.Constructor();
			}

			// Token: 0x06001CE8 RID: 7400 RVA: 0x000654C0 File Offset: 0x000636C0
			public static bool Invoke(in UntypedWeakReferenceId objectId, ref GCHandle objectHandle)
			{
				if (BurstCompiler.IsEnabled)
				{
					IntPtr functionPointer = RuntimeContentManager.GetObjectHandle_00001944$BurstDirectCall.GetFunctionPointer();
					if (functionPointer != 0)
					{
						return calli(System.Boolean(Unity.Entities.Serialization.UntypedWeakReferenceId&,System.Runtime.InteropServices.GCHandle&), ref objectId, ref objectHandle, functionPointer);
					}
				}
				return RuntimeContentManager.GetObjectHandle$BurstManaged(in objectId, ref objectHandle);
			}

			// Token: 0x04000A55 RID: 2645
			private static IntPtr Pointer;

			// Token: 0x04000A56 RID: 2646
			private static IntPtr DeferredCompilation;
		}
	}
}
