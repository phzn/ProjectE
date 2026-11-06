using System;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;

namespace Unity.Entities.Content
{
	// Token: 0x0200046C RID: 1132
	public abstract class ContentLocationService : IDisposable
	{
		// Token: 0x1700023A RID: 570
		// (get) Token: 0x06001C03 RID: 7171 RVA: 0x00061F4B File Offset: 0x0006014B
		// (set) Token: 0x06001C04 RID: 7172 RVA: 0x00061F53 File Offset: 0x00060153
		public string Name { get; set; }

		// Token: 0x1700023B RID: 571
		// (get) Token: 0x06001C05 RID: 7173 RVA: 0x00061F5C File Offset: 0x0006015C
		// (set) Token: 0x06001C06 RID: 7174 RVA: 0x00061F64 File Offset: 0x00060164
		public int Priority { get; set; }

		// Token: 0x1700023C RID: 572
		// (get) Token: 0x06001C07 RID: 7175
		public abstract int LocationCount { get; }

		// Token: 0x06001C08 RID: 7176 RVA: 0x00002287 File Offset: 0x00000487
		public virtual void OnAddedToDeliveryService(ContentDeliveryService cds)
		{
		}

		// Token: 0x06001C09 RID: 7177
		public abstract ContentLocationService.LocationStatus GetLocationStatus(RemoteContentId id);

		// Token: 0x06001C0A RID: 7178
		public abstract ContentLocationService.LocationStatus ResolveLocation(RemoteContentId id);

		// Token: 0x06001C0B RID: 7179
		public abstract bool GetResolvedRemoteContentLocations(ref NativeHashSet<RemoteContentLocation> locs);

		// Token: 0x06001C0C RID: 7180
		public abstract bool GetResolvedContentIds(ref UnsafeList<RemoteContentId> ids);

		// Token: 0x06001C0D RID: 7181
		public unsafe abstract bool TryGetLocationSet(in FixedString512Bytes setName, out RemoteContentId* idPtr, out int count);

		// Token: 0x06001C0E RID: 7182 RVA: 0x00002287 File Offset: 0x00000487
		public virtual void Process()
		{
		}

		// Token: 0x06001C0F RID: 7183 RVA: 0x00002287 File Offset: 0x00000487
		public virtual void Dispose()
		{
		}

		// Token: 0x0200046D RID: 1133
		public enum ResolvingState
		{
			// Token: 0x040009DF RID: 2527
			None,
			// Token: 0x040009E0 RID: 2528
			Resolving,
			// Token: 0x040009E1 RID: 2529
			Complete,
			// Token: 0x040009E2 RID: 2530
			Failed
		}

		// Token: 0x0200046E RID: 1134
		public struct LocationStatus
		{
			// Token: 0x040009E3 RID: 2531
			public ContentLocationService.ResolvingState State;

			// Token: 0x040009E4 RID: 2532
			public RemoteContentLocation Location;
		}
	}
}
