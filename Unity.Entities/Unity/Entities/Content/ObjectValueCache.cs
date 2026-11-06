using System;
using System.Runtime.InteropServices;
using Unity.Burst;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Entities.LowLevel;
using Unity.Entities.Serialization;

namespace Unity.Entities.Content
{
	// Token: 0x020004B8 RID: 1208
	[BurstCompile]
	internal struct ObjectValueCache : IDisposable
	{
		// Token: 0x1700024F RID: 591
		// (get) Token: 0x06001D19 RID: 7449 RVA: 0x00065B76 File Offset: 0x00063D76
		public bool IsCreated
		{
			get
			{
				return this.Values.IsCreated;
			}
		}

		// Token: 0x06001D1A RID: 7450 RVA: 0x00065B83 File Offset: 0x00063D83
		public ObjectValueCache(int initialCapacity)
		{
			this.spinLock = default(SpinLock);
			this.Values = new UnsafeHashMap<UntypedWeakReferenceId, ObjectValueCache.ObjectCacheEntry>(initialCapacity, Allocator.Persistent);
		}

		// Token: 0x06001D1B RID: 7451 RVA: 0x00065BA4 File Offset: 0x00063DA4
		public ObjectLoadingStatus GetLoadingStatus(in UntypedWeakReferenceId id)
		{
			ObjectLoadingStatus objectLoadingStatus = ObjectLoadingStatus.None;
			this.spinLock.Acquire();
			ObjectValueCache.ObjectCacheEntry objectCacheEntry;
			if (this.Values.TryGetValue(id, out objectCacheEntry))
			{
				objectLoadingStatus = objectCacheEntry.LoadingStatus;
			}
			this.spinLock.Release();
			return objectLoadingStatus;
		}

		// Token: 0x06001D1C RID: 7452 RVA: 0x00065BE8 File Offset: 0x00063DE8
		public bool GetObjectHandle(in UntypedWeakReferenceId id, ref GCHandle objHandle)
		{
			this.spinLock.Acquire();
			ObjectValueCache.ObjectCacheEntry objectCacheEntry;
			if (this.Values.TryGetValue(id, out objectCacheEntry))
			{
				objHandle = objectCacheEntry.ObjectHandle;
			}
			this.spinLock.Release();
			return objHandle.IsAllocated;
		}

		// Token: 0x06001D1D RID: 7453 RVA: 0x00065C34 File Offset: 0x00063E34
		public void Dispose()
		{
			this.spinLock.Acquire();
			foreach (KVPair<UntypedWeakReferenceId, ObjectValueCache.ObjectCacheEntry> kvpair in this.Values)
			{
				kvpair.Value.Dispose();
			}
			this.Values.Dispose();
			this.spinLock.Release();
		}

		// Token: 0x06001D1E RID: 7454 RVA: 0x00065CB0 File Offset: 0x00063EB0
		internal void AddEntry(in UntypedWeakReferenceId objectId)
		{
			this.spinLock.Acquire();
			this.Values.TryAdd(objectId, new ObjectValueCache.ObjectCacheEntry
			{
				LoadingStatus = ObjectLoadingStatus.Queued
			});
			this.spinLock.Release();
		}

		// Token: 0x06001D1F RID: 7455 RVA: 0x00065CF8 File Offset: 0x00063EF8
		internal unsafe void AddEntries(UntypedWeakReferenceId* objectIds, int count)
		{
			this.spinLock.Acquire();
			if (this.Values.Capacity - this.Values.Count < count)
			{
				this.Values.Capacity = this.Values.Count + count;
			}
			for (int i = 0; i < count; i++)
			{
				this.Values.TryAdd(objectIds[i], new ObjectValueCache.ObjectCacheEntry
				{
					LoadingStatus = ObjectLoadingStatus.Queued
				});
			}
			this.spinLock.Release();
		}

		// Token: 0x06001D20 RID: 7456 RVA: 0x00065D88 File Offset: 0x00063F88
		internal void SetObjectStatus(in UntypedWeakReferenceId objectId, ObjectLoadingStatus status, in GCHandle objHandle)
		{
			this.spinLock.Acquire();
			this.Values[objectId] = new ObjectValueCache.ObjectCacheEntry
			{
				LoadingStatus = status,
				ObjectHandle = objHandle
			};
			this.spinLock.Release();
		}

		// Token: 0x06001D21 RID: 7457 RVA: 0x00065DDC File Offset: 0x00063FDC
		internal bool RemoveEntry(in UntypedWeakReferenceId objectId, out ObjectLoadingStatus status)
		{
			bool flag = false;
			status = ObjectLoadingStatus.None;
			this.spinLock.Acquire();
			ObjectValueCache.ObjectCacheEntry objectCacheEntry;
			if (this.Values.TryGetValue(objectId, out objectCacheEntry))
			{
				status = objectCacheEntry.LoadingStatus;
				this.Values.Remove(objectId);
				objectCacheEntry.Dispose();
				flag = true;
			}
			this.spinLock.Release();
			return flag;
		}

		// Token: 0x06001D22 RID: 7458 RVA: 0x00065E3D File Offset: 0x0006403D
		internal int Count()
		{
			this.spinLock.Acquire();
			int count = this.Values.Count;
			this.spinLock.Release();
			return count;
		}

		// Token: 0x04000A7A RID: 2682
		private SpinLock spinLock;

		// Token: 0x04000A7B RID: 2683
		private UnsafeHashMap<UntypedWeakReferenceId, ObjectValueCache.ObjectCacheEntry> Values;

		// Token: 0x020004B9 RID: 1209
		private struct ObjectCacheEntry : IDisposable
		{
			// Token: 0x06001D23 RID: 7459 RVA: 0x00065E60 File Offset: 0x00064060
			public void Dispose()
			{
				if (this.ObjectHandle.IsAllocated)
				{
					this.ObjectHandle.Free();
				}
			}

			// Token: 0x04000A7C RID: 2684
			public ObjectLoadingStatus LoadingStatus;

			// Token: 0x04000A7D RID: 2685
			public GCHandle ObjectHandle;
		}
	}
}
