using System;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;

namespace Unity.Entities.Content
{
	// Token: 0x02000472 RID: 1138
	internal class DefaultContentLocationService : ContentLocationService
	{
		// Token: 0x1700023D RID: 573
		// (get) Token: 0x06001C11 RID: 7185 RVA: 0x00061F6D File Offset: 0x0006016D
		public override int LocationCount
		{
			get
			{
				return this.contentLocations.Count;
			}
		}

		// Token: 0x06001C12 RID: 7186 RVA: 0x00061F7C File Offset: 0x0006017C
		public unsafe DefaultContentLocationService(string name, int priority, string catalogPath, Func<string, string> remotePathFunc)
		{
			base.Priority = priority;
			base.Name = name;
			BlobAssetReference<RemoteContentCatalogData> blobAssetReference;
			if (BlobAssetReference<RemoteContentCatalogData>.TryRead(catalogPath, 1, out blobAssetReference))
			{
				int length = blobAssetReference.Value.RemoteContentLocations.Length;
				this.contentLocations = new UnsafeHashMap<RemoteContentId, RemoteContentLocation>(length, Allocator.Persistent);
				for (int i = 0; i < length; i++)
				{
					RemoteContentCatalogData.RemoteContentLocationData remoteContentLocationData = *blobAssetReference.Value.RemoteContentLocations[i];
					if (remotePathFunc != null)
					{
						remoteContentLocationData.location.Path = remotePathFunc(remoteContentLocationData.location.Path.ToString());
					}
					this.contentLocations.Add(remoteContentLocationData.identifier, remoteContentLocationData.location);
				}
				this.contentSets = new UnsafeHashMap<FixedString512Bytes, UnsafeList<RemoteContentId>>(blobAssetReference.Value.ContentSets.Length, Allocator.Persistent);
				for (int j = 0; j < blobAssetReference.Value.ContentSets.Length; j++)
				{
					UnsafeList<RemoteContentId> unsafeList = new UnsafeList<RemoteContentId>(blobAssetReference.Value.ContentSets[j].Ids.Length, Allocator.Persistent, NativeArrayOptions.UninitializedMemory);
					ref BlobArray<int> ptr = ref blobAssetReference.Value.ContentSets[j].Ids;
					for (int k = 0; k < ptr.Length; k++)
					{
						unsafeList.Add(in blobAssetReference.Value.RemoteContentLocations[*ptr[k]].identifier);
					}
					this.contentSets.Add(blobAssetReference.Value.ContentSets[j].Name, unsafeList);
				}
				blobAssetReference.Dispose();
			}
		}

		// Token: 0x06001C13 RID: 7187 RVA: 0x00062134 File Offset: 0x00060334
		internal DefaultContentLocationService(string name, int priority, int count, Func<int, ValueTuple<RemoteContentId, RemoteContentLocation>> indexer, Func<string, string> remotePathFunc, Func<string, IEnumerable<string>> contentSetFunc)
		{
			base.Priority = priority;
			base.Name = name;
			Dictionary<string, List<RemoteContentId>> dictionary = new Dictionary<string, List<RemoteContentId>>();
			this.contentLocations = new UnsafeHashMap<RemoteContentId, RemoteContentLocation>(count, Allocator.Persistent);
			for (int i = 0; i < count; i++)
			{
				ValueTuple<RemoteContentId, RemoteContentLocation> valueTuple = indexer(i);
				if (remotePathFunc != null)
				{
					valueTuple.Item2.Path = remotePathFunc(valueTuple.Item2.Path.ToString());
				}
				this.contentLocations.Add(valueTuple.Item1, valueTuple.Item2);
				if (contentSetFunc != null)
				{
					foreach (string text in contentSetFunc(valueTuple.Item1.Name.ToString()))
					{
						List<RemoteContentId> list;
						if (!dictionary.TryGetValue(text, out list))
						{
							dictionary.Add(text, list = new List<RemoteContentId>());
						}
						list.Add(valueTuple.Item1);
					}
				}
			}
			this.contentSets = new UnsafeHashMap<FixedString512Bytes, UnsafeList<RemoteContentId>>(dictionary.Count, Allocator.Persistent);
			foreach (KeyValuePair<string, List<RemoteContentId>> keyValuePair in dictionary)
			{
				UnsafeList<RemoteContentId> unsafeList = new UnsafeList<RemoteContentId>(keyValuePair.Value.Count, Allocator.Persistent, NativeArrayOptions.UninitializedMemory);
				foreach (RemoteContentId remoteContentId in keyValuePair.Value)
				{
					unsafeList.Add(in remoteContentId);
				}
				this.contentSets.Add(keyValuePair.Key, unsafeList);
			}
		}

		// Token: 0x06001C14 RID: 7188 RVA: 0x0006231C File Offset: 0x0006051C
		public unsafe override bool TryGetLocationSet(in FixedString512Bytes setName, out RemoteContentId* idPtr, out int count)
		{
			UnsafeList<RemoteContentId> unsafeList;
			if (this.contentSets.TryGetValue(setName, out unsafeList))
			{
				idPtr = unsafeList.Ptr;
				count = unsafeList.Length;
				return true;
			}
			idPtr = default(RemoteContentId*);
			count = 0;
			return false;
		}

		// Token: 0x06001C15 RID: 7189 RVA: 0x0006235C File Offset: 0x0006055C
		public override void Dispose()
		{
			if (this.contentLocations.IsCreated)
			{
				this.contentLocations.Dispose();
			}
			if (this.contentSets.IsCreated)
			{
				foreach (KVPair<FixedString512Bytes, UnsafeList<RemoteContentId>> kvpair in this.contentSets)
				{
					kvpair.Value.Dispose();
				}
				this.contentSets.Dispose();
			}
		}

		// Token: 0x06001C16 RID: 7190 RVA: 0x000623E4 File Offset: 0x000605E4
		public override ContentLocationService.LocationStatus GetLocationStatus(RemoteContentId id)
		{
			RemoteContentLocation remoteContentLocation;
			ContentLocationService.LocationStatus locationStatus;
			if (!this.contentLocations.TryGetValue(id, out remoteContentLocation))
			{
				locationStatus = new ContentLocationService.LocationStatus
				{
					Location = default(RemoteContentLocation),
					State = ContentLocationService.ResolvingState.None
				};
				return locationStatus;
			}
			locationStatus = new ContentLocationService.LocationStatus
			{
				Location = remoteContentLocation,
				State = ContentLocationService.ResolvingState.Complete
			};
			return locationStatus;
		}

		// Token: 0x06001C17 RID: 7191 RVA: 0x00062439 File Offset: 0x00060639
		public override ContentLocationService.LocationStatus ResolveLocation(RemoteContentId id)
		{
			return this.GetLocationStatus(id);
		}

		// Token: 0x06001C18 RID: 7192 RVA: 0x00062444 File Offset: 0x00060644
		public override bool GetResolvedContentIds(ref UnsafeList<RemoteContentId> ids)
		{
			foreach (KVPair<RemoteContentId, RemoteContentLocation> kvpair in this.contentLocations)
			{
				RemoteContentId key = kvpair.Key;
				ids.Add(in key);
			}
			return true;
		}

		// Token: 0x06001C19 RID: 7193 RVA: 0x00062484 File Offset: 0x00060684
		public unsafe override bool GetResolvedRemoteContentLocations(ref NativeHashSet<RemoteContentLocation> locs)
		{
			foreach (KVPair<RemoteContentId, RemoteContentLocation> kvpair in this.contentLocations)
			{
				locs.Add(*kvpair.Value);
			}
			return true;
		}

		// Token: 0x040009EB RID: 2539
		private UnsafeHashMap<FixedString512Bytes, UnsafeList<RemoteContentId>> contentSets;

		// Token: 0x040009EC RID: 2540
		private UnsafeHashMap<RemoteContentId, RemoteContentLocation> contentLocations;
	}
}
