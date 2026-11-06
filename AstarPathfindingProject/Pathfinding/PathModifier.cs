using System;

namespace Pathfinding
{
	// Token: 0x0200011F RID: 287
	[Serializable]
	public abstract class PathModifier : IPathModifier
	{
		// Token: 0x170001C2 RID: 450
		// (get) Token: 0x060009BF RID: 2495
		public abstract int Order { get; }

		// Token: 0x060009C0 RID: 2496 RVA: 0x00032560 File Offset: 0x00030760
		public void Awake(Seeker seeker)
		{
			this.seeker = seeker;
			if (seeker != null)
			{
				seeker.RegisterModifier(this);
			}
		}

		// Token: 0x060009C1 RID: 2497 RVA: 0x00032579 File Offset: 0x00030779
		public void OnDestroy(Seeker seeker)
		{
			if (seeker != null)
			{
				seeker.DeregisterModifier(this);
			}
		}

		// Token: 0x060009C2 RID: 2498 RVA: 0x00003788 File Offset: 0x00001988
		public virtual void PreProcess(Path path)
		{
		}

		// Token: 0x060009C3 RID: 2499
		public abstract void Apply(Path path);

		// Token: 0x04000632 RID: 1586
		[NonSerialized]
		public Seeker seeker;
	}
}
