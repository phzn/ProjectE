using System;

namespace UnityEngine.Timeline
{
	// Token: 0x02000025 RID: 37
	public interface IMarker
	{
		// Token: 0x170000A5 RID: 165
		// (get) Token: 0x06000225 RID: 549
		// (set) Token: 0x06000226 RID: 550
		double time { get; set; }

		// Token: 0x170000A6 RID: 166
		// (get) Token: 0x06000227 RID: 551
		TrackAsset parent { get; }

		// Token: 0x06000228 RID: 552
		void Initialize(TrackAsset parent);
	}
}
