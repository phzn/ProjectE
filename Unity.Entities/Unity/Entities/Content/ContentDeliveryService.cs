using System;
using System.Collections.Generic;
using System.IO;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;

namespace Unity.Entities.Content
{
	// Token: 0x02000460 RID: 1120
	public class ContentDeliveryService : IDisposable
	{
		// Token: 0x1700022F RID: 559
		// (get) Token: 0x06001BB6 RID: 7094 RVA: 0x000603F5 File Offset: 0x0005E5F5
		public IEnumerable<ContentLocationService> LocationServices
		{
			get
			{
				return this.locationServices.Values;
			}
		}

		// Token: 0x17000230 RID: 560
		// (get) Token: 0x06001BB7 RID: 7095 RVA: 0x00060402 File Offset: 0x0005E602
		public IEnumerable<ContentDownloadService> DownloadServices
		{
			get
			{
				return this.downloadServices.Values;
			}
		}

		// Token: 0x06001BB8 RID: 7096 RVA: 0x00060410 File Offset: 0x0005E610
		public ContentDeliveryService()
		{
			this.downloadStates = new UnsafeHashMap<RemoteContentId, ContentDeliveryService.DeliveryStatus>(8, Allocator.Persistent);
			this.activeDownloads = new UnsafeList<RemoteContentId>(8, Allocator.Persistent, NativeArrayOptions.UninitializedMemory);
			this.contentSets = new UnsafeHashMap<RemoteContentId, ContentDeliveryService.ContentSet>(4, Allocator.Persistent);
			this.locationServices = new SortedList<int, ContentLocationService>(1, default(ContentDeliveryService.DescendingOrderComparer));
			this.downloadServices = new SortedList<int, ContentDownloadService>(1, default(ContentDeliveryService.DescendingOrderComparer));
		}

		// Token: 0x06001BB9 RID: 7097 RVA: 0x00060490 File Offset: 0x0005E690
		public string RemapContentPath(string originalPath)
		{
			FixedString512Bytes fixedString512Bytes = originalPath;
			RemoteContentId remoteContentId = new RemoteContentId(in fixedString512Bytes);
			ContentDeliveryService.DeliveryStatus deliveryStatus = this.ProcessDownload(in remoteContentId);
			Action<string> logFunc = ContentDeliveryGlobalState.LogFunc;
			if (logFunc != null)
			{
				logFunc(string.Format("RemapContentPath id [{0},{1}] with relative path {2}, new path ={3}", new object[]
				{
					remoteContentId.Name,
					remoteContentId.Hash,
					originalPath,
					deliveryStatus.DownloadStatus.LocalPath
				}));
			}
			if (deliveryStatus.State != ContentDeliveryService.DeliveryState.ContentDownloaded)
			{
				return originalPath;
			}
			return deliveryStatus.DownloadStatus.LocalPath.ToString();
		}

		// Token: 0x06001BBA RID: 7098 RVA: 0x0006052C File Offset: 0x0005E72C
		public void AddDownloadService(ContentDownloadService service)
		{
			using (IEnumerator<KeyValuePair<int, ContentDownloadService>> enumerator = this.downloadServices.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					KeyValuePair<int, ContentDownloadService> keyValuePair = enumerator.Current;
					if (keyValuePair.Value.Name == service.Name)
					{
						this.downloadServices.Remove(keyValuePair.Key);
						break;
					}
				}
				goto IL_0067;
			}
			IL_0057:
			int priority = service.Priority;
			service.Priority = priority + 1;
			IL_0067:
			if (!this.downloadServices.TryAdd(service.Priority, service))
			{
				goto IL_0057;
			}
			service.OnAddedToDeliveryService(this);
			Action<string> logFunc = ContentDeliveryGlobalState.LogFunc;
			if (logFunc == null)
			{
				return;
			}
			logFunc("Added download service " + service.Name);
		}

		// Token: 0x06001BBB RID: 7099 RVA: 0x000605EC File Offset: 0x0005E7EC
		private ContentDownloadService GetDownloadServiceForLocation(in RemoteContentLocation location)
		{
			foreach (KeyValuePair<int, ContentDownloadService> keyValuePair in this.downloadServices)
			{
				if (keyValuePair.Value.CanDownload(location))
				{
					return keyValuePair.Value;
				}
			}
			return null;
		}

		// Token: 0x06001BBC RID: 7100 RVA: 0x00060654 File Offset: 0x0005E854
		public void AddLocationService(ContentLocationService service)
		{
			using (IEnumerator<KeyValuePair<int, ContentLocationService>> enumerator = this.locationServices.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					KeyValuePair<int, ContentLocationService> keyValuePair = enumerator.Current;
					if (keyValuePair.Value.Name == service.Name)
					{
						this.locationServices.Remove(keyValuePair.Key);
						keyValuePair.Value.Dispose();
						break;
					}
				}
				goto IL_0073;
			}
			IL_0063:
			int priority = service.Priority;
			service.Priority = priority + 1;
			IL_0073:
			if (!this.locationServices.TryAdd(service.Priority, service))
			{
				goto IL_0063;
			}
			service.OnAddedToDeliveryService(this);
			Action<string> logFunc = ContentDeliveryGlobalState.LogFunc;
			if (logFunc == null)
			{
				return;
			}
			logFunc("Added location service " + service.Name);
		}

		// Token: 0x06001BBD RID: 7101 RVA: 0x00060720 File Offset: 0x0005E920
		public bool AccumulateContentSize(ref int entryCount, ref long totalBytes, ref long cachedBytes, ref long uncachedBytes)
		{
			NativeHashSet<RemoteContentLocation> nativeHashSet = new NativeHashSet<RemoteContentLocation>(32, Allocator.Temp);
			foreach (ContentLocationService contentLocationService in this.locationServices.Values)
			{
				contentLocationService.GetResolvedRemoteContentLocations(ref nativeHashSet);
			}
			this.AccumulateContentSize(nativeHashSet, ref entryCount, ref totalBytes, ref cachedBytes, ref uncachedBytes);
			nativeHashSet.Dispose();
			return true;
		}

		// Token: 0x06001BBE RID: 7102 RVA: 0x00060798 File Offset: 0x0005E998
		public unsafe void AccumulateContentSize(in FixedString512Bytes setName, ref int entryCount, ref long totalBytes, ref long cachedBytes, ref long uncachedBytes)
		{
			NativeHashSet<RemoteContentLocation> nativeHashSet = new NativeHashSet<RemoteContentLocation>(32, Allocator.Temp);
			foreach (ContentLocationService contentLocationService in this.locationServices.Values)
			{
				RemoteContentId* ptr;
				int num;
				if (contentLocationService.TryGetLocationSet(in setName, out ptr, out num))
				{
					for (int i = 0; i < num; i++)
					{
						RemoteContentId remoteContentId = ptr[i];
						nativeHashSet.Add(contentLocationService.ResolveLocation(remoteContentId).Location);
					}
				}
			}
			this.AccumulateContentSize(nativeHashSet, ref entryCount, ref totalBytes, ref cachedBytes, ref uncachedBytes);
			nativeHashSet.Dispose();
		}

		// Token: 0x06001BBF RID: 7103 RVA: 0x0006084C File Offset: 0x0005EA4C
		private void AccumulateContentSize(NativeHashSet<RemoteContentLocation> locations, ref int entryCount, ref long totalBytes, ref long cachedBytes, ref long uncachedBytes)
		{
			foreach (RemoteContentLocation remoteContentLocation in locations)
			{
				entryCount++;
				totalBytes += remoteContentLocation.Size;
				if (this.GetDownloadServiceForLocation(in remoteContentLocation).GetDownloadStatus(in remoteContentLocation).DownloadState == ContentDownloadService.State.Complete)
				{
					cachedBytes += remoteContentLocation.Size;
				}
				else
				{
					uncachedBytes += remoteContentLocation.Size;
				}
			}
		}

		// Token: 0x06001BC0 RID: 7104 RVA: 0x000608D8 File Offset: 0x0005EAD8
		public bool AccumulateDownloadStats(in RemoteContentId id, ref long totalBytes, ref long downloadedBytes)
		{
			ContentDeliveryService.DeliveryStatus deliveryStatus;
			if (this.downloadStates.TryGetValue(id, out deliveryStatus))
			{
				if (deliveryStatus.LocationStatus.State != ContentLocationService.ResolvingState.Complete)
				{
					return false;
				}
				this.GetDownloadServiceForLocation(in deliveryStatus.LocationStatus.Location).GetDownloadProgress(deliveryStatus.LocationStatus.Location, ref totalBytes, ref downloadedBytes);
				return true;
			}
			else
			{
				ContentDeliveryService.ContentSet contentSet;
				if (this.contentSets.TryGetValue(id, out contentSet))
				{
					for (int i = 0; i < contentSet.remoteIds.Length; i++)
					{
						RemoteContentId remoteContentId = contentSet.remoteIds[i];
						this.AccumulateDownloadStats(in remoteContentId, ref totalBytes, ref downloadedBytes);
					}
					return true;
				}
				return false;
			}
		}

		// Token: 0x06001BC1 RID: 7105 RVA: 0x00060978 File Offset: 0x0005EB78
		public void CleanCache()
		{
			try
			{
				NativeHashSet<RemoteContentLocation> nativeHashSet = new NativeHashSet<RemoteContentLocation>(32, Allocator.Temp);
				foreach (KeyValuePair<int, ContentLocationService> keyValuePair in this.locationServices)
				{
					keyValuePair.Value.GetResolvedRemoteContentLocations(ref nativeHashSet);
				}
				HashSet<string> hashSet = new HashSet<string>();
				foreach (RemoteContentLocation remoteContentLocation in nativeHashSet)
				{
					string text;
					if (this.GetDownloadServiceForLocation(in remoteContentLocation).GetLocalCacheFilePath(remoteContentLocation, out text))
					{
						hashSet.Add(text);
					}
				}
				nativeHashSet.Dispose();
				int num = 0;
				foreach (KeyValuePair<int, ContentDownloadService> keyValuePair2 in this.downloadServices)
				{
					foreach (string text2 in Directory.GetFiles(keyValuePair2.Value.CacheRoot, "*.*", SearchOption.AllDirectories))
					{
						if (!hashSet.Contains(text2))
						{
							Action<string> logFunc = ContentDeliveryGlobalState.LogFunc;
							if (logFunc != null)
							{
								logFunc("Deleting file " + text2 + " from cache");
							}
							File.Delete(text2);
							string directoryName = Path.GetDirectoryName(text2);
							if (Directory.GetFiles(directoryName).Length == 0)
							{
								Directory.Delete(directoryName, true);
							}
							num++;
						}
					}
				}
				Action<string> logFunc2 = ContentDeliveryGlobalState.LogFunc;
				if (logFunc2 != null)
				{
					logFunc2(string.Format("Cleaned cache, {0} files deleted.", num));
				}
			}
			catch (Exception ex)
			{
				Debug.LogException(ex);
			}
		}

		// Token: 0x06001BC2 RID: 7106 RVA: 0x00060B6C File Offset: 0x0005ED6C
		public void GetDeliveryStatus(in RemoteContentId id, ref NativeList<ContentDeliveryService.DeliveryStatus> results)
		{
			ContentDeliveryService.DeliveryStatus deliveryStatus;
			if (this.downloadStates.TryGetValue(id, out deliveryStatus))
			{
				results.Add(in deliveryStatus);
				return;
			}
			ContentDeliveryService.ContentSet contentSet;
			if (this.contentSets.TryGetValue(id, out contentSet))
			{
				for (int i = 0; i < contentSet.remoteIds.Length; i++)
				{
					RemoteContentId remoteContentId = contentSet.remoteIds[i];
					this.GetDeliveryStatus(in remoteContentId, ref results);
				}
			}
		}

		// Token: 0x06001BC3 RID: 7107 RVA: 0x00060BDC File Offset: 0x0005EDDC
		private ContentLocationService.LocationStatus GetLocationStatus(in RemoteContentId id)
		{
			foreach (KeyValuePair<int, ContentLocationService> keyValuePair in this.locationServices)
			{
				ContentLocationService.LocationStatus locationStatus = keyValuePair.Value.GetLocationStatus(id);
				if (locationStatus.State != ContentLocationService.ResolvingState.None)
				{
					return locationStatus;
				}
			}
			return new ContentLocationService.LocationStatus
			{
				State = ContentLocationService.ResolvingState.Failed
			};
		}

		// Token: 0x06001BC4 RID: 7108 RVA: 0x00060C58 File Offset: 0x0005EE58
		private ContentLocationService.LocationStatus ResolveLocation(in RemoteContentId id)
		{
			foreach (KeyValuePair<int, ContentLocationService> keyValuePair in this.locationServices)
			{
				ContentLocationService.LocationStatus locationStatus = keyValuePair.Value.ResolveLocation(id);
				if (locationStatus.State != ContentLocationService.ResolvingState.None)
				{
					return locationStatus;
				}
			}
			return new ContentLocationService.LocationStatus
			{
				State = ContentLocationService.ResolvingState.Failed
			};
		}

		// Token: 0x06001BC5 RID: 7109 RVA: 0x00060CD4 File Offset: 0x0005EED4
		public void CancelAllDeliveries()
		{
			foreach (KVPair<RemoteContentId, ContentDeliveryService.DeliveryStatus> kvpair in this.downloadStates)
			{
				RemoteContentId key = kvpair.Key;
				this.CancelDelivery(in key);
			}
			foreach (KVPair<RemoteContentId, ContentDeliveryService.ContentSet> kvpair2 in this.contentSets)
			{
				kvpair2.Value.Dispose();
			}
			this.contentSets.Clear();
		}

		// Token: 0x06001BC6 RID: 7110 RVA: 0x00060D84 File Offset: 0x0005EF84
		public ContentDeliveryService.DeliveryStatus GetDeliveryStatus(in RemoteContentId id)
		{
			RemoteContentId remoteContentId = id;
			if (remoteContentId.IsValid)
			{
				ContentDeliveryService.DeliveryStatus deliveryStatus;
				if (this.downloadStates.TryGetValue(id, out deliveryStatus))
				{
					if (deliveryStatus.State != ContentDeliveryService.DeliveryState.ContentDownloaded)
					{
						if (deliveryStatus.LocationStatus.State < ContentLocationService.ResolvingState.Complete)
						{
							deliveryStatus.LocationStatus = this.GetLocationStatus(in id);
						}
						if (deliveryStatus.LocationStatus.State == ContentLocationService.ResolvingState.Complete)
						{
							deliveryStatus.DownloadStatus = this.GetDownloadServiceForLocation(in deliveryStatus.LocationStatus.Location).GetDownloadStatus(in deliveryStatus.LocationStatus.Location);
						}
						this.downloadStates[id] = deliveryStatus;
					}
					return deliveryStatus;
				}
				ContentDeliveryService.ContentSet contentSet;
				if (this.contentSets.TryGetValue(id, out contentSet))
				{
					ContentDeliveryService.DeliveryStatus deliveryStatus2 = new ContentDeliveryService.DeliveryStatus
					{
						ContentId = id
					};
					deliveryStatus2.LocationStatus.State = ContentLocationService.ResolvingState.Failed;
					deliveryStatus2.DownloadStatus.DownloadState = ContentDownloadService.State.Failed;
					for (int i = 0; i < contentSet.remoteIds.Length; i++)
					{
						remoteContentId = contentSet.remoteIds[i];
						ContentDeliveryService.DeliveryStatus deliveryStatus3 = this.GetDeliveryStatus(in remoteContentId);
						deliveryStatus2.DownloadStatus.BytesDownloaded = deliveryStatus2.DownloadStatus.BytesDownloaded + deliveryStatus3.DownloadStatus.BytesDownloaded;
						if (deliveryStatus3.DownloadStatus.DownloadState < deliveryStatus2.DownloadStatus.DownloadState)
						{
							deliveryStatus2.DownloadStatus.DownloadState = deliveryStatus3.DownloadStatus.DownloadState;
						}
						deliveryStatus2.LocationStatus.Location.Size = deliveryStatus2.LocationStatus.Location.Size + deliveryStatus3.LocationStatus.Location.Size;
						if (deliveryStatus3.LocationStatus.State < deliveryStatus2.LocationStatus.State)
						{
							deliveryStatus2.LocationStatus.State = deliveryStatus3.LocationStatus.State;
						}
					}
					return deliveryStatus2;
				}
			}
			return new ContentDeliveryService.DeliveryStatus
			{
				ContentId = id
			};
		}

		// Token: 0x06001BC7 RID: 7111 RVA: 0x00060F6C File Offset: 0x0005F16C
		public RemoteContentId DeliverContent(string url, Hash128 hash, long size, uint crc = 0U)
		{
			RemoteContentLocation remoteContentLocation = default(RemoteContentLocation);
			remoteContentLocation.Path = url;
			remoteContentLocation.Hash = hash;
			remoteContentLocation.Size = size;
			remoteContentLocation.Crc = crc;
			return this.DeliverContent(in remoteContentLocation);
		}

		// Token: 0x06001BC8 RID: 7112 RVA: 0x00060FB0 File Offset: 0x0005F1B0
		public RemoteContentId DeliverContent(in RemoteContentLocation loc)
		{
			ContentDownloadService downloadServiceForLocation = this.GetDownloadServiceForLocation(in loc);
			RemoteContentId remoteContentId = new RemoteContentId(in loc.Path);
			Hash128 hash = loc.Hash;
			if (!hash.IsValid && this.downloadStates.ContainsKey(remoteContentId))
			{
				this.downloadStates.Remove(remoteContentId);
				int num = this.activeDownloads.IndexOf(remoteContentId);
				if (num >= 0)
				{
					this.activeDownloads.RemoveAtSwapBack(num);
				}
				downloadServiceForLocation.CancelDownload(loc);
			}
			if (!this.downloadStates.ContainsKey(remoteContentId))
			{
				ContentDeliveryService.DeliveryStatus deliveryStatus = new ContentDeliveryService.DeliveryStatus
				{
					ContentId = remoteContentId
				};
				deliveryStatus.LocationStatus = new ContentLocationService.LocationStatus
				{
					Location = loc,
					State = ContentLocationService.ResolvingState.Complete
				};
				deliveryStatus.DownloadStatus = downloadServiceForLocation.DownloadContent(in loc);
				this.downloadStates[remoteContentId] = deliveryStatus;
				this.activeDownloads.Add(in remoteContentId);
			}
			return remoteContentId;
		}

		// Token: 0x06001BC9 RID: 7113 RVA: 0x00061098 File Offset: 0x0005F298
		public bool CancelDelivery(in RemoteContentId id)
		{
			Action<string> logFunc = ContentDeliveryGlobalState.LogFunc;
			if (logFunc != null)
			{
				logFunc(string.Format("Cancelling content delivery for {0}.", id));
			}
			ContentDeliveryService.DeliveryStatus deliveryStatus = this.GetDeliveryStatus(in id);
			if (deliveryStatus.State == ContentDeliveryService.DeliveryState.None)
			{
				return false;
			}
			int num = this.activeDownloads.IndexOf(id);
			if (num >= 0)
			{
				this.activeDownloads.RemoveAtSwapBack(num);
			}
			if (deliveryStatus.State >= ContentDeliveryService.DeliveryState.LocationResolved)
			{
				this.GetDownloadServiceForLocation(in deliveryStatus.LocationStatus.Location).CancelDownload(deliveryStatus.LocationStatus.Location);
			}
			deliveryStatus.DownloadStatus.DownloadState = ContentDownloadService.State.Cancelled;
			this.downloadStates[id] = deliveryStatus;
			return true;
		}

		// Token: 0x06001BCA RID: 7114 RVA: 0x0006114C File Offset: 0x0005F34C
		public void DeliverContent(in RemoteContentId id)
		{
			Action<string> logFunc = ContentDeliveryGlobalState.LogFunc;
			if (logFunc != null)
			{
				logFunc(string.Format("Delivering content for {0}.", id));
			}
			ContentDeliveryService.DeliveryStatus deliveryStatus;
			if (this.downloadStates.TryGetValue(id, out deliveryStatus) && (deliveryStatus.State == ContentDeliveryService.DeliveryState.Failed || deliveryStatus.State == ContentDeliveryService.DeliveryState.Cancelled))
			{
				this.downloadStates.Remove(id);
			}
			if (!this.downloadStates.ContainsKey(id))
			{
				this.ProcessDownload(in id);
				this.activeDownloads.Add(in id);
			}
		}

		// Token: 0x06001BCB RID: 7115 RVA: 0x000611E0 File Offset: 0x0005F3E0
		public unsafe RemoteContentId DeliverContent(in FixedString512Bytes setName)
		{
			Action<string> logFunc = ContentDeliveryGlobalState.LogFunc;
			if (logFunc != null)
			{
				logFunc(string.Format("Delivering content set for {0}.", setName));
			}
			foreach (KeyValuePair<int, ContentLocationService> keyValuePair in this.locationServices)
			{
				RemoteContentId* ptr;
				int num;
				if (keyValuePair.Value.TryGetLocationSet(in setName, out ptr, out num))
				{
					return this.DeliverContent(ptr, num);
				}
			}
			return default(RemoteContentId);
		}

		// Token: 0x06001BCC RID: 7116 RVA: 0x00061278 File Offset: 0x0005F478
		public unsafe RemoteContentId DeliverContent(RemoteContentId* remoteIds, int length)
		{
			RemoteContentId remoteContentIdentifier = this.GetRemoteContentIdentifier(remoteIds, length);
			if (!this.contentSets.ContainsKey(remoteContentIdentifier))
			{
				UnsafeList<RemoteContentId> unsafeList = new UnsafeList<RemoteContentId>(length, Allocator.Persistent, NativeArrayOptions.UninitializedMemory);
				unsafeList.AddRangeNoResize((void*)remoteIds, length);
				for (int i = 0; i < length; i++)
				{
					this.DeliverContent(in remoteIds[i]);
				}
				ContentDeliveryService.ContentSet contentSet = new ContentDeliveryService.ContentSet
				{
					remoteIds = unsafeList
				};
				this.contentSets.Add(remoteContentIdentifier, contentSet);
			}
			return remoteContentIdentifier;
		}

		// Token: 0x06001BCD RID: 7117 RVA: 0x000612F2 File Offset: 0x0005F4F2
		public RemoteContentId DeliverContent(in UnsafeList<RemoteContentId> remoteIds)
		{
			return this.DeliverContent(remoteIds.Ptr, remoteIds.Length);
		}

		// Token: 0x06001BCE RID: 7118 RVA: 0x00061308 File Offset: 0x0005F508
		private unsafe RemoteContentId GetRemoteContentIdentifier(RemoteContentId* ids, int length)
		{
			List<RemoteContentId> list = new List<RemoteContentId>(length);
			for (int i = 0; i < length; i++)
			{
				RemoteContentId remoteContentId = ids[i];
				if (remoteContentId.IsValid)
				{
					list.Add(remoteContentId);
				}
			}
			if (list.Count == 0)
			{
				return default(RemoteContentId);
			}
			Hash128 hash = Hash128.Compute<RemoteContentId>(list);
			FixedString512Bytes fixedString512Bytes = hash.ToString();
			return new RemoteContentId(in fixedString512Bytes, hash);
		}

		// Token: 0x06001BCF RID: 7119 RVA: 0x00061384 File Offset: 0x0005F584
		private ContentDeliveryService.DeliveryStatus ProcessDownload(in RemoteContentId id)
		{
			ContentDeliveryService.DeliveryStatus deliveryStatus = this.GetDeliveryStatus(in id);
			if (deliveryStatus.State == ContentDeliveryService.DeliveryState.None)
			{
				deliveryStatus.LocationStatus = this.ResolveLocation(in id);
				this.downloadStates[id] = deliveryStatus;
			}
			if (deliveryStatus.State == ContentDeliveryService.DeliveryState.LocationResolved)
			{
				deliveryStatus.DownloadStatus = this.GetDownloadServiceForLocation(in deliveryStatus.LocationStatus.Location).DownloadContent(in deliveryStatus.LocationStatus.Location);
				this.downloadStates[id] = deliveryStatus;
			}
			return deliveryStatus;
		}

		// Token: 0x06001BD0 RID: 7120 RVA: 0x0006140C File Offset: 0x0005F60C
		public void Process()
		{
			foreach (KeyValuePair<int, ContentLocationService> keyValuePair in this.locationServices)
			{
				keyValuePair.Value.Process();
			}
			foreach (KeyValuePair<int, ContentDownloadService> keyValuePair2 in this.downloadServices)
			{
				keyValuePair2.Value.Process();
			}
			for (int i = this.activeDownloads.Length - 1; i >= 0; i--)
			{
				RemoteContentId remoteContentId = this.activeDownloads[i];
				if (this.ProcessDownload(in remoteContentId).State >= ContentDeliveryService.DeliveryState.ContentDownloaded)
				{
					this.activeDownloads.RemoveAtSwapBack(i);
				}
			}
		}

		// Token: 0x06001BD1 RID: 7121 RVA: 0x000614EC File Offset: 0x0005F6EC
		public void Dispose()
		{
			try
			{
				if (this.locationServices != null)
				{
					foreach (KeyValuePair<int, ContentLocationService> keyValuePair in this.locationServices)
					{
						keyValuePair.Value.Dispose();
					}
					this.locationServices = null;
				}
				if (this.downloadServices != null)
				{
					foreach (KeyValuePair<int, ContentDownloadService> keyValuePair2 in this.downloadServices)
					{
						keyValuePair2.Value.Dispose();
					}
					this.downloadServices = null;
				}
				if (this.contentSets.IsCreated)
				{
					foreach (KVPair<RemoteContentId, ContentDeliveryService.ContentSet> kvpair in this.contentSets)
					{
						kvpair.Value.Dispose();
					}
					this.contentSets.Dispose();
				}
				if (this.downloadStates.IsCreated)
				{
					this.downloadStates.Dispose();
				}
				if (this.activeDownloads.IsCreated)
				{
					this.activeDownloads.Dispose();
				}
			}
			catch (Exception ex)
			{
				Debug.LogException(ex);
			}
		}

		// Token: 0x040009A9 RID: 2473
		private SortedList<int, ContentLocationService> locationServices;

		// Token: 0x040009AA RID: 2474
		private SortedList<int, ContentDownloadService> downloadServices;

		// Token: 0x040009AB RID: 2475
		private UnsafeHashMap<RemoteContentId, ContentDeliveryService.ContentSet> contentSets;

		// Token: 0x040009AC RID: 2476
		private UnsafeList<RemoteContentId> activeDownloads;

		// Token: 0x040009AD RID: 2477
		private UnsafeHashMap<RemoteContentId, ContentDeliveryService.DeliveryStatus> downloadStates;

		// Token: 0x02000461 RID: 1121
		public enum DeliveryState
		{
			// Token: 0x040009AF RID: 2479
			None,
			// Token: 0x040009B0 RID: 2480
			ResolvingLocation,
			// Token: 0x040009B1 RID: 2481
			LocationResolved,
			// Token: 0x040009B2 RID: 2482
			DownloadingContent,
			// Token: 0x040009B3 RID: 2483
			ContentDownloaded,
			// Token: 0x040009B4 RID: 2484
			Cancelled,
			// Token: 0x040009B5 RID: 2485
			Failed
		}

		// Token: 0x02000462 RID: 1122
		public struct DeliveryStatus
		{
			// Token: 0x17000231 RID: 561
			// (get) Token: 0x06001BD2 RID: 7122 RVA: 0x00061674 File Offset: 0x0005F874
			public ContentDeliveryService.DeliveryState State
			{
				get
				{
					if (this.DownloadStatus.DownloadState == ContentDownloadService.State.Failed)
					{
						return ContentDeliveryService.DeliveryState.Failed;
					}
					if (this.DownloadStatus.DownloadState == ContentDownloadService.State.Cancelled)
					{
						return ContentDeliveryService.DeliveryState.Cancelled;
					}
					if (this.DownloadStatus.DownloadState == ContentDownloadService.State.Complete)
					{
						return ContentDeliveryService.DeliveryState.ContentDownloaded;
					}
					if (this.DownloadStatus.DownloadState == ContentDownloadService.State.Downloading)
					{
						return ContentDeliveryService.DeliveryState.DownloadingContent;
					}
					if (this.LocationStatus.State == ContentLocationService.ResolvingState.Failed)
					{
						return ContentDeliveryService.DeliveryState.Failed;
					}
					if (this.LocationStatus.State == ContentLocationService.ResolvingState.Complete)
					{
						return ContentDeliveryService.DeliveryState.LocationResolved;
					}
					if (this.LocationStatus.State == ContentLocationService.ResolvingState.Resolving)
					{
						return ContentDeliveryService.DeliveryState.ResolvingLocation;
					}
					return ContentDeliveryService.DeliveryState.None;
				}
			}

			// Token: 0x040009B6 RID: 2486
			public RemoteContentId ContentId;

			// Token: 0x040009B7 RID: 2487
			public ContentLocationService.LocationStatus LocationStatus;

			// Token: 0x040009B8 RID: 2488
			public ContentDownloadService.DownloadStatus DownloadStatus;
		}

		// Token: 0x02000463 RID: 1123
		private struct ContentSet : IDisposable
		{
			// Token: 0x06001BD3 RID: 7123 RVA: 0x000616F2 File Offset: 0x0005F8F2
			public void Dispose()
			{
				this.remoteIds.Dispose();
			}

			// Token: 0x040009B9 RID: 2489
			public UnsafeList<RemoteContentId> remoteIds;
		}

		// Token: 0x02000464 RID: 1124
		private struct DescendingOrderComparer : IComparer<int>
		{
			// Token: 0x06001BD4 RID: 7124 RVA: 0x000616FF File Offset: 0x0005F8FF
			public int Compare(int x, int y)
			{
				return y - x;
			}
		}
	}
}
