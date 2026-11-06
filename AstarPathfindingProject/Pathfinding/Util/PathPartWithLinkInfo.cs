using System;

namespace Pathfinding.Util
{
	// Token: 0x02000294 RID: 660
	public struct PathPartWithLinkInfo
	{
		// Token: 0x0600107D RID: 4221 RVA: 0x00064501 File Offset: 0x00062701
		public PathPartWithLinkInfo(int startIndex, int endIndex, OffMeshLinks.OffMeshLinkTracer linkInfo = default(OffMeshLinks.OffMeshLinkTracer))
		{
			this.startIndex = startIndex;
			this.endIndex = endIndex;
			this.linkInfo = linkInfo;
		}

		// Token: 0x1700026D RID: 621
		// (get) Token: 0x0600107E RID: 4222 RVA: 0x00064518 File Offset: 0x00062718
		public Funnel.PartType type
		{
			get
			{
				if (this.linkInfo.link == null)
				{
					return Funnel.PartType.NodeSequence;
				}
				return Funnel.PartType.OffMeshLink;
			}
		}

		// Token: 0x04000BF3 RID: 3059
		public int startIndex;

		// Token: 0x04000BF4 RID: 3060
		public int endIndex;

		// Token: 0x04000BF5 RID: 3061
		public OffMeshLinks.OffMeshLinkTracer linkInfo;
	}
}
