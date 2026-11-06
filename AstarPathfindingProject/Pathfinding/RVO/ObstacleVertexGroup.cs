using System;
using Unity.Mathematics;

namespace Pathfinding.RVO
{
	// Token: 0x020002C2 RID: 706
	public struct ObstacleVertexGroup
	{
		// Token: 0x04000CA5 RID: 3237
		public ObstacleType type;

		// Token: 0x04000CA6 RID: 3238
		public int vertexCount;

		// Token: 0x04000CA7 RID: 3239
		public float3 boundsMn;

		// Token: 0x04000CA8 RID: 3240
		public float3 boundsMx;
	}
}
