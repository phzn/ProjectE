using System;
using UnityEngine;

namespace ProjectExhibitionist.Minimap
{
	// Token: 0x020009B3 RID: 2483
	internal struct MiniMapPoiVisual
	{
		// Token: 0x0400457A RID: 17786
		public Vector3 center;

		// Token: 0x0400457B RID: 17787
		public Vector3 visitCenter;

		// Token: 0x0400457C RID: 17788
		public MiniMapPoiKind kind;

		// Token: 0x0400457D RID: 17789
		public bool previewOnly;

		// Token: 0x0400457E RID: 17790
		public bool explicitMarker;

		// Token: 0x0400457F RID: 17791
		public bool discovered;

		// Token: 0x04004580 RID: 17792
		public Vector2 footprintSize;

		// Token: 0x04004581 RID: 17793
		public float footprintYaw;

		// Token: 0x04004582 RID: 17794
		public BoxCollider[] interiorVolumes;

		// Token: 0x04004583 RID: 17795
		public MiniMapPoiDiscoveryMode discoveryMode;
	}
}
