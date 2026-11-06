using System;

namespace Pathfinding
{
	// Token: 0x02000115 RID: 277
	[Obsolete("Has been renamed to RecastNavmeshModifier")]
	public interface RecastMeshObj
	{
		// Token: 0x170001B6 RID: 438
		// (get) Token: 0x0600097B RID: 2427
		// (set) Token: 0x0600097C RID: 2428
		bool enabled { get; set; }

		// Token: 0x170001B7 RID: 439
		// (get) Token: 0x0600097D RID: 2429
		// (set) Token: 0x0600097E RID: 2430
		bool dynamic { get; set; }

		// Token: 0x170001B8 RID: 440
		// (get) Token: 0x0600097F RID: 2431
		// (set) Token: 0x06000980 RID: 2432
		bool solid { get; set; }

		// Token: 0x170001B9 RID: 441
		// (get) Token: 0x06000981 RID: 2433
		// (set) Token: 0x06000982 RID: 2434
		RecastNavmeshModifier.GeometrySource geometrySource { get; set; }

		// Token: 0x170001BA RID: 442
		// (get) Token: 0x06000983 RID: 2435
		// (set) Token: 0x06000984 RID: 2436
		RecastNavmeshModifier.ScanInclusion includeInScan { get; set; }

		// Token: 0x170001BB RID: 443
		// (get) Token: 0x06000985 RID: 2437
		// (set) Token: 0x06000986 RID: 2438
		int surfaceID { get; set; }

		// Token: 0x170001BC RID: 444
		// (get) Token: 0x06000987 RID: 2439
		// (set) Token: 0x06000988 RID: 2440
		RecastNavmeshModifier.Mode mode { get; set; }
	}
}
