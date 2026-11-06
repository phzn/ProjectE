using System;
using Unity.Entities;
using Unity.Mathematics;

namespace Pathfinding.ECS
{
	// Token: 0x020002E9 RID: 745
	public struct AgentOffMeshLinkTraversal : IComponentData, IQueryTypeParameter
	{
		// Token: 0x17000300 RID: 768
		// (get) Token: 0x060012B4 RID: 4788 RVA: 0x00072BF1 File Offset: 0x00070DF1
		[Obsolete("Use relativeStart instead")]
		public float3 firstPosition
		{
			get
			{
				return this.relativeStart;
			}
		}

		// Token: 0x17000301 RID: 769
		// (get) Token: 0x060012B5 RID: 4789 RVA: 0x00072BF9 File Offset: 0x00070DF9
		[Obsolete("Use relativeEnd instead")]
		public float3 secondPosition
		{
			get
			{
				return this.relativeEnd;
			}
		}

		// Token: 0x060012B6 RID: 4790 RVA: 0x00072C01 File Offset: 0x00070E01
		public AgentOffMeshLinkTraversal(OffMeshLinks.OffMeshLinkTracer linkInfo)
		{
			this.relativeStart = linkInfo.relativeStart;
			this.relativeEnd = linkInfo.relativeEnd;
			this.isReverse = linkInfo.isReverse;
		}

		// Token: 0x04000DA3 RID: 3491
		public float3 relativeStart;

		// Token: 0x04000DA4 RID: 3492
		public float3 relativeEnd;

		// Token: 0x04000DA5 RID: 3493
		public bool isReverse;
	}
}
