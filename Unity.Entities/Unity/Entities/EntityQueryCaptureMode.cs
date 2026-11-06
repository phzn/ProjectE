using System;

namespace Unity.Entities
{
	// Token: 0x0200010F RID: 271
	public enum EntityQueryCaptureMode
	{
		// Token: 0x04000315 RID: 789
		[Obsolete("The Capture-query-at-record feature will be removed in a future version of the Entities package. Use AtPlayback mode instead. If AtRecord semantics are required, use the query to generate a NativeArray of matching entities to process, and pass the array instead of the query. (RemovedAfter Entities 2.0)")]
		AtRecord,
		// Token: 0x04000316 RID: 790
		AtPlayback
	}
}
