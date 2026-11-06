using System;

namespace Pathfinding
{
	// Token: 0x02000023 RID: 35
	public class RichSpecial : RichPathPart
	{
		// Token: 0x170000B2 RID: 178
		// (get) Token: 0x06000220 RID: 544 RVA: 0x0000A55C File Offset: 0x0000875C
		public FakeTransform first
		{
			get
			{
				return new FakeTransform
				{
					position = this.nodeLink.relativeStart,
					rotation = (this.nodeLink.isReverse ? this.nodeLink.link.end.rotation : this.nodeLink.link.start.rotation)
				};
			}
		}

		// Token: 0x170000B3 RID: 179
		// (get) Token: 0x06000221 RID: 545 RVA: 0x0000A5C4 File Offset: 0x000087C4
		public FakeTransform second
		{
			get
			{
				return new FakeTransform
				{
					position = this.nodeLink.relativeEnd,
					rotation = (this.nodeLink.isReverse ? this.nodeLink.link.start.rotation : this.nodeLink.link.end.rotation)
				};
			}
		}

		// Token: 0x170000B4 RID: 180
		// (get) Token: 0x06000222 RID: 546 RVA: 0x0000A62C File Offset: 0x0000882C
		public bool reverse
		{
			get
			{
				return this.nodeLink.isReverse;
			}
		}

		// Token: 0x06000223 RID: 547 RVA: 0x0000A639 File Offset: 0x00008839
		public override void OnEnterPool()
		{
			this.nodeLink = default(OffMeshLinks.OffMeshLinkTracer);
		}

		// Token: 0x06000224 RID: 548 RVA: 0x0000A647 File Offset: 0x00008847
		public RichSpecial Initialize(OffMeshLinks.OffMeshLinkTracer nodeLink)
		{
			this.nodeLink = nodeLink;
			return this;
		}

		// Token: 0x04000123 RID: 291
		public OffMeshLinks.OffMeshLinkTracer nodeLink;
	}
}
