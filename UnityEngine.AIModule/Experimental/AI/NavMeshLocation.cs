using System;

namespace UnityEngine.Experimental.AI
{
	// Token: 0x02000003 RID: 3
	public struct NavMeshLocation
	{
		// Token: 0x17000001 RID: 1
		// (get) Token: 0x06000007 RID: 7 RVA: 0x0000212F File Offset: 0x0000032F
		public readonly PolygonId polygon { get; }

		// Token: 0x17000002 RID: 2
		// (get) Token: 0x06000008 RID: 8 RVA: 0x00002137 File Offset: 0x00000337
		public readonly Vector3 position { get; }

		// Token: 0x06000009 RID: 9 RVA: 0x0000213F File Offset: 0x0000033F
		internal NavMeshLocation(Vector3 position, PolygonId polygon)
		{
			this.position = position;
			this.polygon = polygon;
		}
	}
}
