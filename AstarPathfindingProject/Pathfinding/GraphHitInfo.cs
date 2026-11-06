using System;
using UnityEngine;

namespace Pathfinding
{
	// Token: 0x02000028 RID: 40
	public struct GraphHitInfo
	{
		// Token: 0x170000B6 RID: 182
		// (get) Token: 0x06000250 RID: 592 RVA: 0x0000B000 File Offset: 0x00009200
		public readonly float distance
		{
			get
			{
				return (this.point - this.origin).magnitude;
			}
		}

		// Token: 0x0400014B RID: 331
		public Vector3 origin;

		// Token: 0x0400014C RID: 332
		public Vector3 point;

		// Token: 0x0400014D RID: 333
		public GraphNode node;

		// Token: 0x0400014E RID: 334
		public Vector3 tangentOrigin;

		// Token: 0x0400014F RID: 335
		public Vector3 tangent;
	}
}
